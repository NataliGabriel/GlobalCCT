using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GLB.CCT.Persistencia
{
    public class CadastrosConsultaRepositorio
    {
        private SqlConnection conexao;

        public CadastrosConsultaRepositorio()
        {
            ConexaoBanco conexaoBanco = new ConexaoBanco();
            conexao = conexaoBanco.sqlConnection();
        }
        public async Task<int?> RetornarCodigoOrigemDestinoPorSigla (string siglaOrigemDestino)
        {
            return await conexao.QueryFirstOrDefaultAsync<int?>("SELECT COD_LOCAL FROM TBL_LOCAL WHERE ATALHO_LOCAL=@siglaOrigemDestino", new { siglaOrigemDestino });
        }
    }
}
