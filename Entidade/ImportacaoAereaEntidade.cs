using GLB.CCT.EntidadeXML.ConsultaCCT;
using GLB.CCT.Negocio;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GLB.CCT.Entidade
{
    public class ImportacaoAereaEntidade
    {
        public string N_REFERENCIA { get; set; }
        public int? COD_ORIGEM { get; set; }
        public int? COD_DESTINO { get; set; }
        public DateTime? DATA_PREV_CHEGADA { get; set; }
        public DateTime? DATA_CHEGADA { get; set; }
        public DateTime? DATA_PREV_EMBARQUE { get; set; }
        public DateTime? DT_EMISSAO_HOUSE { get; set; }
        public string DT_EMISSAO_HOUSE_FORMAT
        {
            get
            {
                if (DT_EMISSAO_HOUSE.HasValue)
                {
                    return DT_EMISSAO_HOUSE.Value.ToString("yyyy-MM-ddTHH:mm:sszzz", CultureInfo.InvariantCulture);
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public string DESC_MERC { get; set; }
        public double? PESO_BRUTO { get; set; }
        public double? QTD_VOLUMES { get; set; }
        public string NR_VOO { get; set; }
        public DateTime? ETD { get; set; }

        //Campos que serao enviados HOUSE
        public string NR_HOUSE { get; set; }
        public int TIPO_FRETE_HOUSE { get; set; }
        public double? VALOR_FRETE { get; set; }
        public string NR_MASTER { get; set; }
        public string NOME_SUB_AGENTE { get; set; }
        public string SIGLA_ORIGEM { get; set; }
        public string SIGLA_DESTINO { get; set; }
        public string MOEDA_FRETE { get; set; }
        public string NOME_AG_EXPORTADOR { get; set; }
        public string CEP_AGENTE_EXPORTADOR { get; set; }
        public string CIDADE_AG_EXPORTADOR { get; set; }
        public string TEL_AG_EXPORTADOR { get; set; }
        public string END_AG_EXPORTADOR { get; set; }
        public string NOME_AG_EMBARCADOR { get; set; }
        public string CEP_AGENTE_EMBARCADOR { get; set; }
        public string CIDADE_AG_EMBARCADOR { get; set; }
        public string PAIS_AG_EXPORTADOR { get; set; }
        public string TEL_AG_EMBARCADOR { get; set; }
        public string END_AG_EMBARCADOR { get; set; }
        public string NOME_CONSIGNEE { get; set; }
        public string CEP_CONSIGNEE { get; set; }
        public string CIDADE_CONSIGNEE { get; set; }
        public string TEL_CONSIGNEE { get; set; }
        public string END_CONSIGNEE { get; set; }
        public string CNPJ_CPF { get; set; }
        public double? PESO_LIQUIDO { get; set; }
        public string? CODIGO_RECINTO { get; set; }
        public string PAIS_EMBARCADOR { get; set; }
        public string MoedaFretePreppaid 
        {
            get
            {
                if (TIPO_FRETE_HOUSE == 2)
                {
                    return MOEDA_FRETE;
                }
                return null;
            }
        }
        public string MoedaFreteCollect
        {
            get
            {
                if (TIPO_FRETE_HOUSE == 1)
                {
                    return MOEDA_FRETE;
                }
                return null;
            }
        }
        public double? ValorFretePreppaid
        {
            get
            {
                if (TIPO_FRETE_HOUSE == 2)
                {
                    return VALOR_FRETE;
                }
                return 0;
            }
        }
        public double? ValorFreteCollect
        {
            get
            {
                if (TIPO_FRETE_HOUSE == 1)
                {
                    return VALOR_FRETE;
                }
                return 0;
            }
        }

    }

    public class nMaster
    {
        public List<string> NR_HOUSE { get; set; }
        public nMaster()
        {
            NR_HOUSE = new List<string>();
        }
    }
}
