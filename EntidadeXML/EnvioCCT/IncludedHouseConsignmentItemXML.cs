using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GLB.CCT.EntidadeXML.EnvioCCT
{
    public class IncludedHouseConsignmentItemXML
    {
        public IncludedHouseConsignmentItemXML()
        {
            
        }
        public int SequenceNumeric { get; set; }
        public GrossWeightMeasureMXL GrossWeightMeasure { get; set; } = new GrossWeightMeasureMXL();
        public string PackageQuantity { get; set; }
        public string PieceQuantity { get; set; }
        public string TotalPieceQuantity { get; set; }
        public string SummaryDescription { get; set; }
        public string Information { get; set; } = "NDA";
        public TransportContractDocumentXML TransportContractDocument { get; set; } = new TransportContractDocumentXML();
        public NatureIdentificationTransportCargoXML NatureIdentificationTransportCargo { get; set; } = new NatureIdentificationTransportCargoXML();
        public ApplicableFreightRateServiceChargeXML ApplicableFreightRateServiceCharge { get; set; } = new ApplicableFreightRateServiceChargeXML();
    }
    [XmlRoot]

    public class GrossWeightMeasureMXL
    {
        public GrossWeightMeasureMXL()
        {
            
        }
        [XmlText]
        public string GrossWeightMeasure { get; set; }
        [XmlAttribute]
        public string unitCode { get; set; } = "KGM";
    }
    [XmlRoot]

    public class NatureIdentificationTransportCargoXML
    {
        public NatureIdentificationTransportCargoXML()
        {
            
        }
        [XmlElement]

        public string Identification { get; set; }
    }
    [XmlRoot]

    public class ApplicableFreightRateServiceChargeXML
    {
        public ApplicableFreightRateServiceChargeXML()
        {
            
        }
        public ChargeableWeightMeasureXML ChargeableWeightMeasure { get; set; } = new ChargeableWeightMeasureXML();
    }
    public class ChargeableWeightMeasureXML
    {
        [XmlText]
        public string ChargeableWeightMeasure { get; set; }
        [XmlAttribute]
        public string unitCode { get; set; } = "KGM";
    }
}
