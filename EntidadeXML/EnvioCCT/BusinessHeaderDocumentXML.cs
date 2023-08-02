using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GLB.CCT.EntidadeXML.EnvioCCT
{
    public class BusinessHeaderDocumentXML
    {
        public BusinessHeaderDocumentXML()
        {
            
        }
        public string ID { get; set; }

        public string IncludedHeaderNote { get; set; }
        public SignatoryConsignorAuthenticationXML SignatoryConsignorAuthentication { get; set; } = new SignatoryConsignorAuthenticationXML();
        public SignatoryCarrierAuthenticationXML SignatoryCarrierAuthentication { get; set; } = new SignatoryCarrierAuthenticationXML();
    }
    public class BusinessHeaderDocument
    {
        public BusinessHeaderDocument()
        {

        }
        public string ID { get; set; } 
    }
}
