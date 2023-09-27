using GLB.CCT.Negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GLB.CCT.EntidadeXML.EnvioCCT
{
    public class MessageHeaderDocument
    {
        public MessageHeaderDocument()
        {

        }
        public MessageHeaderDocument(string id, string name, string typeCode, string purposeCode)
        {
            ID = id;
            Name = name;
            TypeCode = typeCode;
            PurposeCode = purposeCode;
            IssueDateTime = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:sszzz");
            if (typeCode == "703")
                VersionID = "3.00";
            else
                VersionID = "2.00";
        }
        public string ID { get; set; }
        public string Name { get; set; }
        public string TypeCode { get; set; }
        public string IssueDateTime { get; set; }
        public string PurposeCode { get; set; }
        public string VersionID { get; set; }
        [XmlElement(ElementName = "SenderParty")]
        public List<SenderParty> senderParties { get; set; } = new List<SenderParty>();
        [XmlElement(ElementName = "RecipientParty")]
        public List<RecipientParty> recipientParties { get; set; } = new List<RecipientParty>();
    }
}
