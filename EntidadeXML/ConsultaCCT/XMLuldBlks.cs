using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GLB.CCT.EntidadeXML.ConsultaCCT
{
    public class XMLuldBlks
    {
        public XMLuldBlks()
        {
            conhecimentos = new XMLconhecimentos();
        }
        public string codigoProprietarioULD { get; set; }
        public XMLconhecimentos conhecimentos { get; set; }
        public string formaTransporte { get; set; }
        public string numeroSerieULD { get; set; }
        public string tipoULD { get; set; }
    }
}
