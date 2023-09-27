using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GLB.CCT.Negocio
{
    public static class Extensoes
    {
        public static DateTime? FormatarData(this string strData)
        {
            try
            {
                int ano = Convert.ToInt16(strData.Substring(0, 4));
                int mes = Convert.ToInt16(strData.Substring(6, 2));
                int dia = Convert.ToInt16(strData.Substring(8, 2));
                DateTime? novaData = new DateTime(ano, mes, dia);
                return novaData;
            }
            catch (Exception)
            {
                try
                {
                    int ano = Convert.ToInt16(strData.Substring(6, 4));
                    int mes = Convert.ToInt16(strData.Substring(3, 2));
                    int dia = Convert.ToInt16(strData.Substring(0, 2));
                    DateTime? novaData = new DateTime(ano, mes, dia);
                    return novaData;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }
        public static double? FormatarDouble(this string strDouble)
        {
            double novoDouble = 0;
            double.TryParse(strDouble, out novoDouble);
            return novoDouble;

        }
        public static int? FormatarInt(this string strInt)
        {
            int novoInt = 0;
            int.TryParse(strInt, out novoInt);
            return novoInt;

        }
        public static string FormatarNumeroParaXML(this double? dblStr)
        {
            string novoStr = string.Format("{0:0.00}", dblStr).Replace(",", ".");

            return novoStr;
        }
        public static string ConverterNuloParaBranco(this string str)
        {
            if (str == "" || str == null)
            {
                return "N/A";
            }
            return str;
        }
        public static string FormataXML(this string str)
        {
            return str.Replace("(", "").Replace(")", " ").Replace("Ç", "C").Replace("É", "E");
        }
        public static string FormataCaratere(this string str)
        {
            return str.Replace("Ã", "A").Replace("Õ", "O").Replace("Ê", "E").Replace("Â", "A").Replace("Î", "I").Replace("Ô", "O").Replace("Û", "").Replace("Á", "A").Replace("É", "E").Replace("Í", "I").Replace("Ó", "O").Replace("Ú", "U").Replace("Ç", "C").Replace("À", "A").Replace("È", "E").Replace("Ì", "I").Replace("Ò", "O").Replace("Ù", "U").Replace(" "," ");
        }
        public static string ConvertName(this string str)
        {
            switch (str)
            {
                case "NOME_CONSIGNEE":
                    return "Nome Importador";
                case "CEP_CONSIGNEE":
                    return "CEP Importador";
                case "CIDADE_CONSIGNEE":
                    return "Cidade Importador";
                case "END_CONSIGNEE":
                    return "Endereço Importador";
                case "CEP_AGENTE_EMBARCADOR":
                    return "CEP Ag. Embarcador";
                case "CIDADE_AG_EMBARCADOR":
                    return "Cidade Ag. Embarcador";
                case "PAIS_EMBARCADOR":
                    return "País Ag. Embarcador";
                case "END_AG_EMBARCADOR":
                    return "Endereço Ag. Embarcador";
                case "NOME_SUB_AGENTE":
                    return "Nome Sub Agente";
                case "COD_ORIGEM":
                    return "Origem";
                case "COD_DESTINO":
                    return "Destino";
                case "NOME_AG_EXPORTADOR":
                    return "Nome Exportador";
                case "CEP_AGENTE_EXPORTADOR":
                    return "CEP Exportador";
                case "CIDADE_AG_EXPORTADOR":
                    return "Cidade Exportador";
                case "END_AG_EXPORTADOR":
                    return "Endereço Exportador";
                case "PAIS_AG_EXPORTADOR":
                    return "Sigla DUE";
                case "QTD_VOLUMES":
                    return "Volume";
                case "PESO_BRUTO":
                    return "Peso Bruto";
                case "MOEDA_FRETE":
                    return "Moeda";

            }
            return "";
        }
    }
}
