using GLB.CCT.Entidade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data.SqlClient;
using System.Diagnostics.Metrics;

namespace GLB.CCT.Persistencia
{
    public class ImportacaoAereaRepositorio
    {
        private SqlConnection conexao;
        public ImportacaoAereaRepositorio()
        {
            ConexaoBanco conexaoBanco = new ConexaoBanco();
            conexao = conexaoBanco.sqlConnection();
        }
        public async Task Atualizar(ImportacaoAereaEntidade entidade)
        {
            StringBuilder sqlUpdate = new StringBuilder();
            sqlUpdate.Append("UPDATE TBL_SISTEMA SET ");
            sqlUpdate.Append("COD_ORIGEM=@COD_ORIGEM, COD_DESTINO=@COD_DESTINO, DATA_PREV_CHEGADA=@DATA_PREV_CHEGADA, DATA_CHEGADA=@DATA_CHEGADA, DATA_PREV_EMBARQUE=@DATA_PREV_EMBARQUE, ");
            sqlUpdate.Append("DT_EMISSAO_HOUSE=@DT_EMISSAO_HOUSE, DESC_MERC=@DESC_MERC, PESO_BRUTO=@PESO_BRUTO, QTD_VOLUMES=@QTD_VOLUMES, NR_VOO=@NR_VOO, ETD=@ETD ");
            sqlUpdate.Append("WHERE N_REFERENCIA = @N_REFERENCIA");
            await conexao.ExecuteAsync(sqlUpdate.ToString(), entidade);
        }
        public async Task<ImportacaoAereaEntidade> Consultar(string nReferencia)
        {
            Console.WriteLine(DateTime.Now + "> Buscando Referência...");
            try
            {
                string sSql = @$"               
                SELECT 
					SIGLA_ORIGEM, 
					SIGLA_DESTINO,
					NOME_SUB_AGENTE,
					NR_MASTER,
					NR_HOUSE,
					QTD_VOLUMES, 
					PESO_BRUTO,
					DESC_MERC, 
						(
						SELECT TOP 1
							VALOR_MOEDA 
								FROM 
									V_LANCAMENTOS 
								WHERE 
									N_REFERENCIA = V_IA.N_REFERENCIA 
								AND 
									COD_TITULO = 11
						) AS VALOR_FRETE, 
						(
						SELECT TOP 1 
							SIGLA 
								FROM 
									V_LANCAMENTOS 
								WHERE 
									N_REFERENCIA = V_IA.N_REFERENCIA 
								AND 
									COD_TITULO = 11) AS MOEDA_FRETE, 
					AG_EXPORTADOR.NOME_RED AS NOME_AG_EXPORTADOR,
					AG_EXPORTADOR.CEP AS CEP_AGENTE_EXPORTADOR,
					AG_EXPORTADOR.CIDADE AS CIDADE_AG_EXPORTADOR,
					AG_EXPORTADOR.TELEFONE AS TEL_AG_EXPORTADOR, 
					AG_EXPORTADOR.END_COMPLETO as END_AG_EXPORTADOR, 
					AG_EMBARCADOR.NOME_RED AS NOME_AG_EMBARCADOR,
					AG_EMBARCADOR.CEP AS CEP_AGENTE_EMBARCADOR, 
					AG_EMBARCADOR.CIDADE AS CIDADE_AG_EMBARCADOR,
					PAIS_AG_EXPORTADOR.SIGLA_DUE AS PAIS_AG_EXPORTADOR,
					AG_EMBARCADOR.TELEFONE AS TEL_AG_EMBARCADOR, 
					AG_EMBARCADOR.END_COMPLETO as END_AG_EMBARCADOR, 
					CONSIGNEE.NOME_RED AS NOME_CONSIGNEE,
					CONSIGNEE.CEP AS CEP_CONSIGNEE, 
					CONSIGNEE.CIDADE AS CIDADE_CONSIGNEE,
					CONSIGNEE.TELEFONE AS TEL_CONSIGNEE,
					CONSIGNEE.END_COMPLETO as END_CONSIGNEE,
					CONSIGNEE.CNPJ_CPF, 
					TIPO_FRETE_HOUSE, 
					PESO_LIQUIDO,
					AG_EXPORTADOR.COD_EXTERNO,
					CODIGO_RECINTO,
					PAIS_AG_EMBARCADOR.SIGLA_DUE AS PAIS_EMBARCADOR,
                    DT_EMISSAO_HOUSE AS DT_EMISSAO_HOUSE
				FROM 
					V_IA 
				LEFT JOIN 
					TBL_CADASTRO_GERAL AS AG_EXPORTADOR 
						ON 
							V_IA.COD_CLIENTE_EXTERIOR = AG_EXPORTADOR.COD_CADASTRO
				LEFT JOIN 
					TBL_CADASTRO_GERAL AS AG_EMBARCADOR 
						ON 
							V_IA.COD_AGENTE_EMBARCADOR = AG_EMBARCADOR.COD_CADASTRO 
				LEFT JOIN 
					TBL_PAIS AS PAIS_AG_EXPORTADOR 
						ON 
							AG_EXPORTADOR.DSP_PAIS = PAIS_AG_EXPORTADOR.COD_PAIS_SISTEMA 
				LEFT JOIN 
					TBL_PAIS AS PAIS_AG_EMBARCADOR
						ON 
							AG_EMBARCADOR.DSP_PAIS = PAIS_AG_EMBARCADOR.COD_PAIS_SISTEMA 
					LEFT JOIN 
						TBL_CADASTRO_GERAL AS CONSIGNEE 
							ON 
								V_IA.COD_CLIENTE_CONSIGNEE = CONSIGNEE.COD_CADASTRO 
				WHERE 
					N_REFERENCIA = '{nReferencia}'";
                //conexao.Open();
                return conexao.QueryFirst<ImportacaoAereaEntidade>(sSql);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                MessageBox.Show("Favor verificar se o IP está correto. Caso tenha dúvidas, entre em contato com a Global Solution", "ERRO!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.ReadKey();
                return null;
            }
        }


        public string BuscaCNPJ(string nReferencia)
        {
            string sQuery = $"SELECT CNPJ_CPF FROM V_IA LEFT JOIN TBL_CADASTRO_GERAL ON COD_CADASTRO = COD_SUB_AGENTE WHERE N_REFERENCIA = '{nReferencia}' ";
            conexao.Open();
            using (SqlCommand command = new SqlCommand(sQuery, conexao))
            {
                using SqlDataReader reader = command.ExecuteReader();
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        
                        var retorno = reader.GetString(reader.GetOrdinal("CNPJ_CPF"));
                        conexao.CloseAsync();
                        return retorno;
                    }
                    else
                    {
                        Console.WriteLine("Nenhum CPF/CNPJ Cadastrado com essa referência");
                        return "";
                    }
                }
            }
        }
        public ConsultarCCT_Model ConsultaCCT(string nIdentifica)
        {
            try
            {
                StringBuilder sqlSelect = new StringBuilder();
                sqlSelect.Append("SELECT ");
                sqlSelect.Append("  CODIGO_RECINTO AS COD_RECINTO, ");
                sqlSelect.Append("  SETOR, ");
                sqlSelect.Append("  URF_DESPACHO,  ");
                sqlSelect.Append("  URF_ENTRADA_PAIS,  ");
                sqlSelect.Append("  DATA_CHEGADA AS DT_CHEGADA, ");
                sqlSelect.Append("  DATA_EMB AS DT_EMB, ");
                sqlSelect.Append("  NR_TERMO, ");
                sqlSelect.Append("  NR_VEICULO_TRANSP, ");
                sqlSelect.Append("  QTD_ITENS,  ");
                sqlSelect.Append("  V.PESO_BRUTO,  ");
                sqlSelect.Append("  L.ATALHO_LOCAL AS ATA_LOCAL,");
                sqlSelect.Append("  T.QTD_VOLUME AS QNTD_VOL ");
                sqlSelect.Append("FROM  ");
                sqlSelect.Append("      V_BROKER V ");
                sqlSelect.Append("  INNER JOIN");
                sqlSelect.Append("      TBL_LOCAL L ");
                sqlSelect.Append("  ON ");
                sqlSelect.Append("      V.CODIGO_LOCAL_EMBARQUE = L.COD_LOCAL");
                sqlSelect.Append("  INNER JOIN");
                sqlSelect.Append("      TBL_LISTA_VOLUMES T");
                sqlSelect.Append("  ON ");
                sqlSelect.Append("      V.N_REFERENCIA = T.N_REFERENCIA ");
                sqlSelect.Append($"WHERE NR_IDENTIFICACAO = '{nIdentifica}'");
                conexao.Open();
                using (SqlCommand command = new SqlCommand(sqlSelect.ToString(), conexao))
                {
                    using SqlDataReader reader = command.ExecuteReader();
                    {
                        if (reader.HasRows)
                        {
                            return conexao.QueryFirst<ConsultarCCT_Model?>(sqlSelect.ToString());
                        }
                        else
                        {
                            return null;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadKey();
                return null;
            }
        }
        public bool InserirCCT(string nReferencia, string values)
        {
            string sSql = @$"INSERT INTO
                                    TBL_SISTEMA
                                        (
                                           CODIGO_RECINTO,
                                           SETOR,
                                           URF_DESPACHO,
                                           URF_ENTRADA_PAIS, 
                                           DATA_CHEGADA,
                                           DATA_EMB,
                                           NR_TERMO,
                                           NR_VEICULO_TRANSP,
                                           QTD_VOLUMES,
                                           PESO_BRUTO,
                                           RUC
                                        )
                                        SELECT
                                            {values}
                            FROM
                                TBL_SISTEMA
                            WHERE
                                NR_IDENTIFICACAO = '{nReferencia}'
                                        ";
            try
            {
                using (SqlCommand command = new SqlCommand(sSql, conexao))
                {
                    int i = command.ExecuteNonQuery();
                    if (i == 1)
                        return true;
                    else
                        return false;
                }

            }
            catch (Exception ex) { Console.WriteLine(ex.Message); return false; }
        }
        public bool AtualizaCCT(string nReferencia, string setValue)
        {
            try
            {
                string sSql = @$"
                                UPDATE
                                    TBL_SISTEMA
                                        SET
                                            {setValue}
                                        WHERE
                                            NR_IDENTIFICACAO = '{nReferencia}'";
                using (SqlCommand command = new SqlCommand(sSql, conexao))
                {
                    int i = command.ExecuteNonQuery();
                    if (i == 1)
                        return true;
                    else
                        return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }
        public nMaster QntdHouses(string nMaster_, string dtPrev)
        {
            string sQuery = @$"SELECT NR_HOUSE FROM V_IA WHERE NR_MASTER ='{nMaster_}' and not NR_HOUSE is null ";
            conexao.Open();
            nMaster master = new nMaster();
            using (SqlCommand command = new SqlCommand(sQuery, conexao))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string houseName = reader["NR_HOUSE"].ToString();
                        master.NR_HOUSE.Add(houseName);
                    }
                }
            }
            return master;
        }

        public ImportacaoAereaEntidade RDL(string nProcesso)
        {
            string sQuery = $"SELECT PESO_BRUTO, QTD_VOLUMES, DESC_MERC, NR_HOUSE, SIGLA_ORIGEM, SIGLA_DESTINO FROM V_IA WHERE NR_HOUSE ='{nProcesso}'";
            return conexao.QueryFirst<ImportacaoAereaEntidade?>(sQuery.ToString());
        }

    }
}
