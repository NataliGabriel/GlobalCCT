using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GLB.CCT.EntidadeXML.EnvioCCT
{
    [XmlRoot]
    public class SenderParty
    {
        public SenderParty()
        {

        }
        public SenderPrimaryIDXML PrimaryID { get; set; }
    }
    public class SenderPrimaryIDXML
    {
        [XmlText]
        public string PrimaryID { get; set; }
        [XmlAttribute]
        public string schemeID { get; set; }

    }
}
