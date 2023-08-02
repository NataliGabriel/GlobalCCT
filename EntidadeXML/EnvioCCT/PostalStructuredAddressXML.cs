using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GLB.CCT.EntidadeXML.EnvioCCT
{
    public class PostalStructuredAddressXML
    {
        public PostalStructuredAddressXML()
        {
            
        }
        [XmlElement]

        public string PostcodeCode { get; set; }
        [XmlElement]

        public string StreetName { get; set; }
        [XmlElement]

        public string CityName { get; set; }
        [XmlElement]

        public string CountryID { get; set; } = "US";
    }
}
