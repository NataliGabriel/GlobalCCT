using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GLB.CCT.EntidadeXML.EnvioCCT
{
    [XmlRoot (ElementName = "HouseWaybill", Namespace = "iata:datamodel:3")]
    public class HouseWaybill
    {
        public HouseWaybill()
        {

        }
        public HouseWaybill(string id, string name, string typeCode, string purposeCode)
        {
            MessageHeaderDocument = new MessageHeaderDocument(id, name, typeCode, purposeCode);
        }
        public MessageHeaderDocument MessageHeaderDocument { get; set; }
        public BusinessHeaderDocumentXML BusinessHeaderDocument { get; set; } = new BusinessHeaderDocumentXML();
        public MasterConsignmentXML MasterConsignment { get; set; } = new MasterConsignmentXML();
    }
    [XmlRoot(ElementName = "HouseManifest", Namespace = "iata:datamodel:3")]
    public class HouseManifest
    {
        public HouseManifest() { }
        public HouseManifest(string id, string name, string typeCode, string purposeCode)
        {
            MessageHeaderDocument = new MessageHeaderDocument(id, name, typeCode, purposeCode);
        }
        public MessageHeaderDocument MessageHeaderDocument { get; set; } = new MessageHeaderDocument();
        public BusinessHeaderDocument BusinessHeaderDocument { get; set; } = new BusinessHeaderDocument();
        public MasterConsignment MasterConsignment { get; set; } = new MasterConsignment();
    }
    
}
