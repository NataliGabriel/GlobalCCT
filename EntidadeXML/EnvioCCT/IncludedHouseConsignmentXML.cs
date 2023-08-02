using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GLB.CCT.EntidadeXML.EnvioCCT
{
    public class IncludedHouseConsignment
    {
        public IncludedHouseConsignment() { }
        public string SequenceNumeric { get; set; }
        public GrossWeightMeasureMXL GrossWeightMeasure { get; set; } = new GrossWeightMeasureMXL();
        public string TotalPieceQuantity { get; set; }
        public string SummaryDescription { get; set; }
        public TransportContractDocumentXML TransportContractDocument { get; set; } = new TransportContractDocumentXML();
        public OriginLocationXML OriginLocation { get; set; } = new OriginLocationXML();
        public OriginLocationXML FinalDestinationLocation { get; set; } = new OriginLocationXML();
    }
    public class IncludedHouseConsignmentXML
    {
        public IncludedHouseConsignmentXML()
        {
            
        }
        public string ID { get; set; }
        public bool NilCarriageValueIndicator { get; set; }
        public bool NilCustomsValueIndicator { get; set; }
        public bool NilInsuranceValueIndicator { get; set; }
        public bool TotalChargePrepaidIndicator { get; set; }
        public string WeightTotalChargeAmount { get; set; }
        public bool TotalDisbursementPrepaidIndicator { get; set; }
        [XmlElement]
        public TotalPrepaidChargeAmountXML TotalPrepaidChargeAmount { get; set; } = new TotalPrepaidChargeAmountXML();
        [XmlElement]
        public TotalCollectChargeAmountXML TotalCollectChargeAmount { get; set; } = new TotalCollectChargeAmountXML();
        [XmlElement]
        public IncludedTareGrossWeightMeasureXML IncludedTareGrossWeightMeasure { get; set; } = new IncludedTareGrossWeightMeasureXML();
        public string PackageQuantity { get; set; }
        public string TotalPieceQuantity { get; set; }
        [XmlElement(ElementName = "SummaryDescription")]
        public string SummaryDescription { get; set; } = "";
        [XmlElement] 
        public ConsignorPartyXML ConsignorParty { get; set; } = new ConsignorPartyXML();
        [XmlElement]
        public ConsigneePartyXML ConsigneeParty { get; set; } = new ConsigneePartyXML();
        [XmlElement]
        public ConsigneePartyXML FreightForwarderParty { get; set; } = new ConsigneePartyXML();
        [XmlElement] 
        public ApplicableTransportCargoInsuranceXML ApplicableTransportCargoInsurance { get; set; } = new ApplicableTransportCargoInsuranceXML();

        [XmlElement] 
        public OriginLocationXML OriginLocation { get; set; } = new OriginLocationXML();
        [XmlElement] 
        public OriginLocationXML FinalDestinationLocation { get; set; } = new OriginLocationXML();

        [XmlElement(ElementName = "IncludedCustomsNote")]
        public List<IncludedCustomsNoteXML> IncludedCustomsNote { get; set; } = new List<IncludedCustomsNoteXML>();
        [XmlElement]
        public IncludedHouseConsignmentItemXML IncludedHouseConsignmentItem { get; set; } = new IncludedHouseConsignmentItemXML();
    }
    [XmlRoot]
    public class TotalPrepaidChargeAmountXML
    {
        [XmlText]

        public string TotalPrepaidChargeAmount { get; set; }
        [XmlAttribute]
        public string currencyID { get; set; }
    }
    [XmlRoot]
    public class TotalCollectChargeAmountXML
    {
        [XmlText]

        public string TotalCollectChargeAmount { get; set; }
        [XmlAttribute]
        public string currencyID { get; set; }
    }
    [XmlRoot]
    public class IncludedTareGrossWeightMeasureXML
    {
        [XmlText]

        public string IncludedTareGrossWeightMeasure { get; set; }
        [XmlAttribute]
        public string unitCode { get; set; } = "KGM";
    }

    [XmlRoot]
    public class ApplicableTransportCargoInsuranceXML
    {
        [XmlElement]
        public string CoverageInsuranceParty { get; set; }
    }
}

