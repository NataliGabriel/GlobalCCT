using Dapper;
using GLB.CCT.Entidade;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GLB.CCT.Persistencia
{
    public class DesembaracoImportacaoRepositorio
    {
        private SqlConnection conexao;

        public DesembaracoImportacaoRepositorio()
        {
            ConexaoBanco conexaoBanco = new ConexaoBanco();
            conexao = conexaoBanco.sqlConnection();
        }
        public async Task Atualizar(DesembaracoImportacaoEntidade entidade)
        {
            StringBuilder sqlUpdate = new StringBuilder();
            sqlUpdate.Append("UPDATE TBL_SISTEMA SET ");
            sqlUpdate.Append("CODIGO_LOCAL_EMBARQUE=@CODIGO_LOCAL_EMBARQUE, DATA_CHEGADA=@DATA_CHEGADA, DATA_PREV_CHEGADA=@DATA_PREV_CHEGADA, DATA_PREV_EMBARQUE=@DATA_PREV_EMBARQUE, CODIGO_RECINTO=@CODIGO_RECINTO, ");
            sqlUpdate.Append("NR_TERMO=@NR_TERMO, PESO_BRUTO=@PESO_BRUTO, DATA_EMB=@DATA_EMB, NR_VEICULO_TRANSP=@NR_VEICULO_TRANSP ");
            sqlUpdate.Append("WHERE N_REFERENCIA = @N_REFERENCIA");
            await conexao.ExecuteAsync(sqlUpdate.ToString(), entidade);
        }
        public async Task AtualizarVolume (VolumesDIEntidade entidade)
        {
            StringBuilder sqlUpdate = new StringBuilder();
            sqlUpdate.Append("UPDATE TBB_EMBALAGEM_CARGA_GLOBAL SET ");
            sqlUpdate.Append("QTD_EMBALAGEM=@QTD_EMBALAGEM ");
            sqlUpdate.Append("WHERE N_REFERENCIA = @N_REFERENCIA");
            await conexao.ExecuteAsync(sqlUpdate.ToString(), entidade);
        }
        public async Task InserirVolumes (VolumesDIEntidade entidade)
        {
            StringBuilder sqlUpdate = new StringBuilder();
            sqlUpdate.Append("INSERT INTO TBB_EMBALAGEM_CARGA_GLOBAL ");
            sqlUpdate.Append("(COD_SEQ, COD_SEQ_EMBALAGEM, COD_ESCRITORIO, NOSSA_REFERENCIA, CODIGO_EMBALAGEM, QTD_EMBALAGEM, MARCA, NUMERO, LACRE) ");
            sqlUpdate.Append("SELECT MAX(COD_SEQ)+1, '1' + MAX(COD_SEQ)+1, 1, @NOSSA_REFERENCIA, @CODIGO_EMBALAGEM, @QTD_EMBALAGEM, @MARCA, @NUMERO, @LACRE FROM TBB_EMBALAGEM_CARGA_GLOBAL WHERE COD_ESCRITORIO = 1 ");
            sqlUpdate.Append("WHERE N_REFERENCIA = @N_REFERENCIA");
            await conexao.ExecuteAsync(sqlUpdate.ToString(), entidade);
        }
    }
}
