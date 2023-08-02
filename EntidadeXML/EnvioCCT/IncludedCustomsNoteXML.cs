using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GLB.CCT.EntidadeXML.EnvioCCT
{
    [XmlRoot("IncludedCustoms")]
    public class IncludedCustomsNoteXML
    {
        public IncludedCustomsNoteXML()
        {
            
        }
        public string ContentCode { get; set; }
        public string Content { get; set; }
        public string SubjectCode { get; set; }
        public string CountryID { get; set; }
    }
}
