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
    }
}
