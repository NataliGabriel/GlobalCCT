using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GLB.CCT.EntidadeXML.EnvioCCT
{
    [XmlRoot]
    public class RecipientParty
    {
        public RecipientParty()
        {
            
        }
        public RecipientPrimaryID PrimaryID { get; set; }
    }
    public class RecipientPrimaryID
    {
        [XmlText]
        public string PrimaryID { get; set; }
        [XmlAttribute]
        public string schemeID { get; set; }
    }
}
