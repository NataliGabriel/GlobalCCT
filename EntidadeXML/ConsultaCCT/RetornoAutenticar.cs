using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GLB.CCT.EntidadeXML.ConsultaCCT
{
    public class RetornoAutenticar
    {
        public string SetToken { get; set; }
        public string CSRFToken { get; set; }
        public string Expiration { get; set; }
        public HttpClientHandler httpClientHandler { get; set; } = new HttpClientHandler();
    }
}
