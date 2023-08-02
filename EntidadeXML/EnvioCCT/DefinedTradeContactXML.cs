using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GLB.CCT.EntidadeXML.EnvioCCT
{
    //[Serializable, XmlType(Namespace = "iata:datamodel:3")]
    public class DefinedTradeContactXML
    {
        public DefinedTradeContactXML()
        {
            
        }
        public DirectTelephoneCommunicationXML DirectTelephoneCommunication { get; set; } = new DirectTelephoneCommunicationXML();
    }
    //[Serializable, XmlType(Namespace ="iata:datamodel:3")]
    public class DirectTelephoneCommunicationXML
    {
        public DirectTelephoneCommunicationXML()
        {
            
        }
        public string CompleteNumber { get; set; }
    }
}
