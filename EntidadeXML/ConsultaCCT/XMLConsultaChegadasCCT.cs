using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GLB.CCT.EntidadeXML.ConsultaCCT
{
    public class XMLConsultaChegadasCCT
    {
        public XMLConsultaChegadasCCT()
        {
            chegadas = new XMLChegadas();
        }
        public string aeroportoPartida { get; set; }
        public XMLChegadas chegadas { get; set; }
        public string ciaAerea { get; set; }
        public string cnpjResponsavelManifestoVoo { get; set; }
        public string codigoVoo { get; set; }
        public string dataHoraEnvioManifestoVoo { get; set; }
        public string dataHoraPartidaEfetiva { get; set; }
        public string dataHoraPartidaPrevista { get; set; }
        public string identificacaoViagem { get; set; }
        public string numeroUnicoViagemIntermodal { get; set; }
        public string prefixoAeronaveManifestoVoo { get; set; }
        public string situacaoViagem { get; set; }
    }
}
