using GLB.CCT.Negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace GLB.CCT.EntidadeXML.EnvioCCT
{
    public class SignatoryCarrierAuthenticationXML
    {
        public SignatoryCarrierAuthenticationXML()
        {
            
        }
        public string ActualDateTime { get; set; }
        public string Signatory { get; set; }
        public IssueAuthenticationLocationXML IssueAuthenticationLocation { get; set; } = new IssueAuthenticationLocationXML();
    }
}
