using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GLB.CCT.EntidadeXML.EnvioCCT
{
    public class ConsignorPartyXML
    {
        public ConsignorPartyXML()
        {
            
        }
        public  string Name { get; set; }
        public PostalStructuredAddressXML PostalStructuredAddress { get; set; } = new PostalStructuredAddressXML();
        public DefinedTradeContactXML DefinedTradeContact { get; set; } = new DefinedTradeContactXML();

    }
}
