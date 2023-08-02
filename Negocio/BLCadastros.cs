using GLB.CCT.Persistencia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GLB.CCT.Negocio
{
    public class BLCadastros
    {
        public async Task<int?> AcharOrigemDestinoPorSigla (string siglaLocal)
        {
			try
			{
				CadastrosConsultaRepositorio consultaRepositorio = new CadastrosConsultaRepositorio();
				return await consultaRepositorio.RetornarCodigoOrigemDestinoPorSigla(siglaLocal);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return null;
			}
        }
    }
}
