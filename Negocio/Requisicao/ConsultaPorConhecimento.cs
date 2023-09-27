using GLB.CCT.Entidade;
using GLB.CCT.EntidadeXML.ConsultaCCT;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using GLB.CCT.Persistencia;
using System.Windows;
using static GLB.CCT.Negocio.Requisicao.URLIntegracao;

namespace GLB.CCT.Negocio.Requisicao
{
    public class ConsultaPorConhecimento
    {
        private string _numeroConhecimento;
        private TipoPerfilEnum _tipoPerfil;
        private ImportacaoAereaRepositorio _sSql = new ImportacaoAereaRepositorio();
        private string Errors = string.Empty;
        public ConsultaPorConhecimento(string numeroConhecimento, TipoPerfilEnum tipoPerfilEnum)
        {
            _numeroConhecimento = numeroConhecimento;
            _tipoPerfil = tipoPerfilEnum;
        }
        public async Task ConsultarConhecimentos()
        {
            try
            {

                Comunicacao comunicacao = new Comunicacao();
                RetornoAutenticar autenticar = await comunicacao.Autenticar(_tipoPerfil);
                if (autenticar != null)
                {
                    string enderecoConhecimento = $"{URLPadraoValidacao}{URLConsultaConhecimentoCarga}?numeroConhecimento={_numeroConhecimento}";

                    var client = await comunicacao.RetornarClient(autenticar);
                    HttpResponseMessage response = await client.GetAsync(enderecoConhecimento);
                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = response.Content.ReadAsStringAsync().Result;
                        List<Root?> responseJson = JsonConvert.DeserializeObject<List<Root?>>(responseContent);
                        if (responseJson.Count > 0)
                        {
                            ConsultarCCT_Model? model = _sSql.ConsultaCCT(_numeroConhecimento);
                            if (model == null)
                            {
                                ConsoleInsere(responseJson);
                                if (MessageBox.Show("Nenhum dado registrado! Deseja adiciona-los na base?", "Inserir Dados.", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                                {
                                    if (_sSql.InserirCCT(_numeroConhecimento, MontaValuesInsert(responseJson)))
                                    { MessageBox.Show("Dados Adicionados com Sucesso", "SUCESSO!", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
                                    else
                                    { MessageBox.Show("ERRO " + Errors, "FATAL ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error); Console.WriteLine("ERRO - " + Errors); }
                                }
                                else
                                {
                                    Console.ReadKey();
                                }
                            }
                            else
                            {
                                ConsoleUpdate();

                                if (MessageBox.Show("Já existem dados na base... Deseja atualizar as informações?", "Atualizar Dados.", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                                {
                                    if (_sSql.AtualizaCCT(_numeroConhecimento, MontaSetUpdate(responseJson)))
                                    { MessageBox.Show("Dados Atualizados com Sucesso", "SUCESSO!", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
                                    else
                                    { MessageBox.Show("ERRO: " + Errors, "FATAL ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error); Console.WriteLine("ERRO - " + Errors); }
                                }
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine(DateTime.Now + "> Falha na requisição de Consulta: " + response.StatusCode);
                        Console.WriteLine("Aperte qualquer botão para sair da tela");
                        Console.ReadLine();
                    }
                }
                else
                {
                    Console.WriteLine("Erro! Autenticação Inválida!");
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }

        }

        public void ConsoleInsere(List<Root> responseJson)
        {
            if (responseJson[0].chegadasTerrestres.Count > 0)
            {
                Console.WriteLine("Informações encontrada:");
                Console.WriteLine("URF_ENTRADA_PAIS - " + responseJson[0].chegadasTerrestres[0].codigoUaChegada);

                Console.WriteLine("DATA_CHEGADA - " + responseJson[0].chegadasTerrestres[0].dataHoraChegada);

                Console.WriteLine("DATA_EMB - " + responseJson[0].chegadasTerrestres[0].dataHoraPartida);

                Console.WriteLine("NR_TERMO - " + responseJson[0].chegadasTerrestres[0].numeroDta);

                Console.WriteLine("NR_VEICULO_TRANSP - " + responseJson[0].chegadasTerrestres[0].placa);

                if (responseJson[0].chegadasTerrestres[0].numeroDta == null)
                    Console.WriteLine("NR_TERMO - " + responseJson[0].chegadasTerrestres[0].termo);
            }
            if (responseJson[0].codigoAeroportoOrigemConhecimento != "")
                Console.WriteLine("ATALHO_LOCAL_ORIGEM - " + responseJson[0].codigoAeroportoOrigemConhecimento);
            if (responseJson[0].divergencias.Count > 0)
                Console.WriteLine("QNTD_VOLUME - " + responseJson[0].divergencias[0].quantidadeVolumesConhecimento);
            if (responseJson[0].documentosSaida.Count > 0)
            {
                Console.WriteLine("CODIGO_RECINTO - " + responseJson[0].documentosSaida[0].raDestinoDta);
                Console.WriteLine("SETOR - " + responseJson[0].documentosSaida[0].tipo);
                Console.WriteLine("URF_DESPACHO - " + responseJson[0].documentosSaida[0].uaDestinoDta);
            }
            Console.WriteLine("PESO_BRUTO - " + responseJson[0].pesoBrutoConhecimento);
            Console.WriteLine("RUC - " + responseJson[0].ruc);
        }

        public void ConsoleUpdate()
        {

        }
        public string MontaValuesInsert(List<Root> model)
        {
            string valuesSql = "";
            if (model[0].documentosSaida.Count > 0)
            {
                //if()
                valuesSql += $@"          '{model[0].documentosSaida[0].raDestinoDta}',
                                         '{model[0].documentosSaida[0].tipo}',
                                         '{model[0].documentosSaida[0].uaDestinoDta}',
                                         ";
            }
            else
            {
                valuesSql += $@" '',
                                '',
                                '',";
            }
            if (model[0].chegadasTerrestres.Count > 0)
            {
                valuesSql += $@"            '{model[0].chegadasTerrestres[0].codigoUaChegada}',
                                           '{model[0].chegadasTerrestres[0].dataHoraChegada}',
                                           '{model[0].chegadasTerrestres[0].dataHoraPartida}',
                                           '{(model[0].chegadasTerrestres[0].numeroDta == null ? model[0].chegadasTerrestres[0].termo : model[0].chegadasTerrestres[0].numeroDta)}',
                                           '{model[0].chegadasTerrestres[0].placa}',";
            }
            else
            {
                valuesSql += $@"
                                    '',
                                    '',
                                    '',
                                    '',
                                    '',
";
            }
            //valuesSql += $@"'{(model[0].codigoAeroportoOrigemConhecimento == null ? "''" : model[0].codigoAeroportoOrigemConhecimento )}',";
            if(model[0].divergencias.Count > 0)
            {
                valuesSql += $@"'{model[0].divergencias[0].quantidadeVolumesConhecimento}', ";
            }
            else
            {
                valuesSql += "'',";
            }
            valuesSql += $@"'{(model[0].pesoBrutoConhecimento == null ? "'', \r\n" : model[0].pesoBrutoConhecimento)}',";
            valuesSql += $@"'{(model[0].ruc == null ? "''" : model[0].ruc)}'";
            return valuesSql;
        }
        public string MontaSetUpdate(List<Root> model)
        {
            string sSql = "";

            if (model[0].documentosSaida.Count > 0)
            {
                //if()
                sSql += $@"      CODIGO_RECINTO = '{model[0].documentosSaida[0].raDestinoDta}',
                                 SETOR = '{model[0].documentosSaida[0].tipo}',
                                 URF_DESPACHO = '{model[0].documentosSaida[0].uaDestinoDta}',
                                         ";
            }
            if (model[0].chegadasTerrestres.Count > 0)
            {
                sSql += $@"          URF_ENTRADA_PAIS =  '{model[0].chegadasTerrestres[0].codigoUaChegada}',
                                     DATA_CHEGADA = '{model[0].chegadasTerrestres[0].dataHoraChegada}',
                                     DATA_EMB = '{model[0].chegadasTerrestres[0].dataHoraPartida}',
                                     NR_TERMO = '{(model[0].chegadasTerrestres[0].numeroDta == null ? model[0].chegadasTerrestres[0].termo : model[0].chegadasTerrestres[0].numeroDta)}',
                                     NR_VEICULO_TRANSP = '{model[0].chegadasTerrestres[0].placa}',";
            }
            if (model[0].divergencias.Count > 0)
            {
                sSql += $@" QTD_VOLUMES = '{model[0].divergencias[0].quantidadeVolumesConhecimento}', ";
            }
            else
            {
                sSql += "'',";
            }
            sSql += $@" PESO_BRUTO = '{(model[0].pesoBrutoConhecimento == null ? "'', \r\n" : model[0].pesoBrutoConhecimento)}',";
            sSql += $@" RUC = '{(model[0].ruc == null ? "''" : model[0].ruc)}'";
            return sSql;
        }
    }
}
