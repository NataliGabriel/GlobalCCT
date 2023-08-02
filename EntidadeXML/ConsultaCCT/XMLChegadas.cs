using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GLB.CCT.EntidadeXML.ConsultaCCT
{
    public class XMLChegadas
    {
        public XMLChegadas()
        {
            uldBlks = new XMLuldBlks();
        }
        public string codigoAeroportoChegada { get; set; }
        public string dataHoraChegadaEfetiva { get; set; }
        public string dataHoraChegadaPrevista { get; set; }
        public string dataHoraPartidaPrevista { get; set; }
        public string prefixoAeronaveChegadaEfetiva { get; set; }
        public string recintoAduaneiroChegada { get; set; }
        public string termoEntrada { get; set; }
        public XMLuldBlks uldBlks { get; set; }
    }
}
