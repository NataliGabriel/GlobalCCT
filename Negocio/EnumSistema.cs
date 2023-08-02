using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GLB.CCT.Negocio
{
    public static class EnumSistema
    {
        public enum  TipoProcessoEnum
        {
            ImportacaoAerea = 1,
            DesembaracoImportacao = 9
        }
        public enum TipoConsultaCCT
        {
            ConsultaViagem =1,
            ConsultaConhecimentos = 2

        }
        public enum TipoEnvioCCT
        {
            EnviarHouseXFZB = 1,
            EnviarHouseXFHL = 2
        }
    }
}
