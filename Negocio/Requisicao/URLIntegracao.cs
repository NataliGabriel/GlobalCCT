using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GLB.CCT.Negocio.Requisicao
{
    public static class URLIntegracao
    {

        public static string URLPadraoValidacao = @"https://portalunico.siscomex.gov.br/ccta/api/";
        public static string URLCCT = @"ext/check/received-files/{0}";
        public static string URLConsultaConhecimentoCarga = @"ext/conhecimentos";
        public static string URLAutenticar = @"/portal/api/autenticar";
        public static string URLViagens = @"ext/viagens/{0}";
        public static string URLEnvioHouseXFZB = @"ext/incoming/xfzb/";
        public static string URLEnvioHouseXFHL = @"ext/incoming/xfhl";
        public enum TipoPerfilEnum
        {
            [Description("Declarante importador/exportador")]
            IMPEXP,
            [Description("Depositário")]
            DEPOSIT,
            [Description("Operador Portuário")]
            OPERPORT,
            [Description("Transportador")]
            TRANSPORT,
            [Description("PF – Representante de TETI")]
            TRANSPEST,
            [Description("Agente de Carga")]
            AGECARGA,
            [Description("Remessa Expressa/Correio")]
            AGEREMESS,
            [Description("Ajudante de Despachante")]
            AJUDESPAC,
            [Description("Instituição Financeira")]
            INSTFINANC,
            [Description("Ponto de Contato OEA")]
            CONTATOOEA,
            [Description("Responsável Legal OE")]
            RESPLEGAL,
            [Description("Habilitador")]
            HABILITAD,
            [Description("Receita Federal")]
            RFB
        }
    }
}
