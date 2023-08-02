using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GLB.CCT.EntidadeXML.EnvioCCT
{
    public class MasterConsignment
    {
        public MasterConsignment() { }
        public IncludedTareGrossWeightMeasureXML IncludedTareGrossWeightMeasure { get; set; } = new IncludedTareGrossWeightMeasureXML();
        public string TotalPieceQuantity { get; set; }
        public TransportContractDocumentXML TransportContractDocument { get; set; } = new TransportContractDocumentXML();
        public OriginLocationXML OriginLocation { get; set; } = new OriginLocationXML();
        public OriginLocationXML FinalDestinationLocation { get; set; } = new OriginLocationXML();
        public IncludedHouseConsignment IncludedHouseConsignment { get; set; } = new IncludedHouseConsignment();
    }
    public class MasterConsignmentXML
    {
        public MasterConsignmentXML()
        {
            
        }
        public IncludedTareGrossWeightMeasureXML IncludedTareGrossWeightMeasure { get; set; } = new IncludedTareGrossWeightMeasureXML();
        public string TotalPieceQuantity { get; set; }
        public TransportContractDocumentXML TransportContractDocument { get; set; } = new TransportContractDocumentXML();
        public OriginLocationXML OriginLocation { get; set; } = new OriginLocationXML();
        public OriginLocationXML FinalDestinationLocation { get; set; } = new OriginLocationXML();
        public IncludedHouseConsignmentXML IncludedHouseConsignment { get; set; } = new IncludedHouseConsignmentXML();
    }
    public class TransportContractDocumentXML
    {
        public TransportContractDocumentXML()
        {
            
        }
        public string ID { get; set; }
    }
}
