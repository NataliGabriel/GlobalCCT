using GLB.CCT.Entidade;
using GLB.CCT.EntidadeXML.ConsultaCCT;
using GLB.CCT.EntidadeXML.EnvioCCT;
using GLB.CCT.Persistencia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace GLB.CCT.Negocio
{
    public class BLImportacaoAerea
    {
        private BLCadastros _blCadastros;
        private ImportacaoAereaRepositorio _repositorio;
        public BLImportacaoAerea()
        {
            _blCadastros = new BLCadastros();
            _repositorio = new ImportacaoAereaRepositorio();

        }
        public async Task<ImportacaoAereaEntidade> ConverterXMLChegadaEmEntidade(XMLConsultaChegadasCCT xmlChegadas, string nReferencia)
        {
            try
            {
                ImportacaoAereaEntidade importacaoAereaEntidade = new ImportacaoAereaEntidade
                {
                    N_REFERENCIA = nReferencia,
                    COD_ORIGEM = await _blCadastros.AcharOrigemDestinoPorSigla(xmlChegadas.aeroportoPartida),
                    COD_DESTINO = await _blCadastros.AcharOrigemDestinoPorSigla(xmlChegadas.chegadas.codigoAeroportoChegada),
                    DATA_CHEGADA = xmlChegadas.chegadas.dataHoraChegadaEfetiva.FormatarData(),
                    DATA_PREV_CHEGADA = xmlChegadas.chegadas.dataHoraChegadaPrevista.FormatarData(),
                    DT_EMISSAO_HOUSE = xmlChegadas.chegadas.uldBlks.conhecimentos.dataEmissao.FormatarData(),
                    DESC_MERC = xmlChegadas.chegadas.uldBlks.conhecimentos.descricaoMercadoriaManifestoVoo,
                    PESO_BRUTO = xmlChegadas.chegadas.uldBlks.conhecimentos.pesoBrutoManifestoVoo.FormatarDouble(),
                    QTD_VOLUMES = xmlChegadas.chegadas.uldBlks.conhecimentos.quantidadeVolumesManifestoVoo.FormatarInt(),
                    NR_VOO = xmlChegadas.codigoVoo,
                    ETD = xmlChegadas.dataHoraPartidaEfetiva.FormatarData(),
                    DATA_PREV_EMBARQUE = xmlChegadas.dataHoraPartidaPrevista.FormatarData()
                };
                return importacaoAereaEntidade;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }

        }
        public async Task AtualizarImportacaoAerea(ImportacaoAereaEntidade entidade)
        {
            try
            {
                await _repositorio.Atualizar(entidade);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        public async Task<HouseWaybill> ConverterEntidadeEmXFZB(ImportacaoAereaEntidade entidade)
        {
            try
            {
                Console.WriteLine(DateTime.Now + "> Convertendo Valores...");
                Console.WriteLine(entidade.NR_HOUSE + " - " + entidade.NR_MASTER);
                if (entidade.NR_HOUSE != null && entidade.NR_MASTER != null)
                {
                    HouseWaybill arquivoHouse = new HouseWaybill($"{entidade.NR_HOUSE.Replace("-", "").Replace(" ", "")}_{entidade.NR_MASTER.Remove(8, 1)}", "House Waybill", "703", "Creation");
                    arquivoHouse.MessageHeaderDocument.senderParties = new List<SenderParty>
                {
                    new SenderParty{PrimaryID = new SenderPrimaryIDXML{ PrimaryID = "RIEGESOFTWARE", schemeID = "C" } },
                    new SenderParty{PrimaryID = new SenderPrimaryIDXML{ PrimaryID = "REUAGT88FORWARDER/FRA01", schemeID = "P" } }
                };
                    arquivoHouse.MessageHeaderDocument.recipientParties = new List<RecipientParty>
                {
                    new RecipientParty{PrimaryID = new RecipientPrimaryID{ PrimaryID = "CCSPROVIDERNAME", schemeID = "C" } },
                    new RecipientParty{PrimaryID = new RecipientPrimaryID{ PrimaryID = "REUAIR08$CODE", schemeID = "P"} }
                };
                    arquivoHouse.BusinessHeaderDocument.ID = entidade.NR_HOUSE.Replace("-", "").Replace(" ", "");
                    arquivoHouse.BusinessHeaderDocument.IncludedHeaderNote = "";
                    arquivoHouse.BusinessHeaderDocument.SignatoryConsignorAuthentication.Signatory = entidade.NOME_SUB_AGENTE;
                    arquivoHouse.BusinessHeaderDocument.SignatoryCarrierAuthentication.Signatory = entidade.NOME_SUB_AGENTE;
                    arquivoHouse.BusinessHeaderDocument.SignatoryCarrierAuthentication.IssueAuthenticationLocation.Name = entidade.SIGLA_ORIGEM;
                    arquivoHouse.BusinessHeaderDocument.SignatoryCarrierAuthentication.ActualDateTime = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:sszzz");
                    arquivoHouse.MasterConsignment.IncludedTareGrossWeightMeasure.IncludedTareGrossWeightMeasure = entidade.PESO_BRUTO.FormatarNumeroParaXML();
                    arquivoHouse.MasterConsignment.TotalPieceQuantity = entidade.QTD_VOLUMES.ToString().Replace(",", ".");
                    arquivoHouse.MasterConsignment.TransportContractDocument.ID = entidade.NR_MASTER.Remove(8, 1);
                    arquivoHouse.MasterConsignment.OriginLocation.ID = entidade.SIGLA_ORIGEM.ConverterNuloParaBranco();
                    arquivoHouse.MasterConsignment.FinalDestinationLocation.ID = entidade.SIGLA_DESTINO.ConverterNuloParaBranco();
                    arquivoHouse.MasterConsignment.IncludedHouseConsignment.ID = entidade.NR_HOUSE.ConverterNuloParaBranco().Replace("-", "").Replace(" ", "");
                    if(entidade.TIPO_FRETE_HOUSE == 2)
                    {
                        arquivoHouse.MasterConsignment.IncludedHouseConsignment.NilCarriageValueIndicator = true;
                        arquivoHouse.MasterConsignment.IncludedHouseConsignment.NilCustomsValueIndicator = true;
                        arquivoHouse.MasterConsignment.IncludedHouseConsignment.NilInsuranceValueIndicator = true;
                        arquivoHouse.MasterConsignment.IncludedHouseConsignment.TotalChargePrepaidIndicator = true;
                    }
                    else
                    {
                        arquivoHouse.MasterConsignment.IncludedHouseConsignment.NilCarriageValueIndicator = false;
                        arquivoHouse.MasterConsignment.IncludedHouseConsignment.NilCustomsValueIndicator = false;
                        arquivoHouse.MasterConsignment.IncludedHouseConsignment.NilInsuranceValueIndicator = false;
                        arquivoHouse.MasterConsignment.IncludedHouseConsignment.TotalChargePrepaidIndicator = false;
                    }

                    arquivoHouse.MasterConsignment.IncludedHouseConsignment.WeightTotalChargeAmount = entidade.PESO_BRUTO.FormatarNumeroParaXML();
                    arquivoHouse.MasterConsignment.IncludedHouseConsignment.TotalPrepaidChargeAmount.TotalPrepaidChargeAmount = entidade.ValorFretePreppaid.FormatarNumeroParaXML() == "" ? "0.00" : entidade.ValorFretePreppaid.FormatarNumeroParaXML();
                    arquivoHouse.MasterConsignment.IncludedHouseConsignment.TotalPrepaidChargeAmount.currencyID = entidade.MoedaFretePreppaid == null ? entidade.MoedaFreteCollect : entidade.MoedaFretePreppaid;
                    arquivoHouse.MasterConsignment.IncludedHouseConsignment.TotalCollectChargeAmount.TotalCollectChargeAmount = entidade.ValorFreteCollect.FormatarNumeroParaXML();
                    arquivoHouse.MasterConsignment.IncludedHouseConsignment.TotalCollectChargeAmount.currencyID = entidade.MoedaFreteCollect == null ? entidade.MoedaFretePreppaid : entidade.MoedaFreteCollect;
                    arquivoHouse.MasterConsignment.IncludedHouseConsignment.IncludedTareGrossWeightMeasure.IncludedTareGrossWeightMeasure = entidade.PESO_BRUTO.FormatarNumeroParaXML();
                    arquivoHouse.MasterConsignment.IncludedHouseConsignment.PackageQuantity = "0";
                    arquivoHouse.MasterConsignment.IncludedHouseConsignment.TotalPieceQuantity = entidade.QTD_VOLUMES.ToString().Replace(",", ".");
                    arquivoHouse.MasterConsignment.IncludedHouseConsignment.SummaryDescription = entidade.DESC_MERC.ConverterNuloParaBranco();
                    if (entidade.TIPO_FRETE_HOUSE == 2)
                    {
                        arquivoHouse.MasterConsignment.IncludedHouseConsignment.TotalDisbursementPrepaidIndicator = true;
                    }
                    else
                    {
                        arquivoHouse.MasterConsignment.IncludedHouseConsignment.TotalDisbursementPrepaidIndicator = false;
                    }
                    arquivoHouse.MasterConsignment.IncludedHouseConsignment.ConsignorParty.Name = entidade.NOME_AG_EXPORTADOR.ConverterNuloParaBranco();
                    arquivoHouse.MasterConsignment.IncludedHouseConsignment.ConsignorParty.PostalStructuredAddress.PostcodeCode = entidade.CEP_AGENTE_EXPORTADOR.ConverterNuloParaBranco();
                    arquivoHouse.MasterConsignment.IncludedHouseConsignment.ConsignorParty.PostalStructuredAddress.StreetName = entidade.END_AG_EXPORTADOR.ConverterNuloParaBranco();
                    arquivoHouse.MasterConsignment.IncludedHouseConsignment.ConsignorParty.PostalStructuredAddress.CityName = entidade.CIDADE_AG_EXPORTADOR.ConverterNuloParaBranco();
                    arquivoHouse.MasterConsignment.IncludedHouseConsignment.ConsignorParty.PostalStructuredAddress.CountryID = entidade.PAIS_AG_EXPORTADOR.ConverterNuloParaBranco();
                    //arquivoHouse.MasterConsignment.IncludedHouseConsignment.ConsigneeParty.DefinedTradeContact.DirectTelephoneCommunication.CompleteNumber = entidade.TEL_AG_EMBARCADOR.ConverterNuloParaBranco();

                    arquivoHouse.MasterConsignment.IncludedHouseConsignment.ConsigneeParty.Name = entidade.NOME_CONSIGNEE.ConverterNuloParaBranco();
                    arquivoHouse.MasterConsignment.IncludedHouseConsignment.ConsigneeParty.PostalStructuredAddress.PostcodeCode = entidade.CEP_CONSIGNEE.ConverterNuloParaBranco();
                    arquivoHouse.MasterConsignment.IncludedHouseConsignment.ConsigneeParty.PostalStructuredAddress.StreetName = entidade.END_CONSIGNEE.ConverterNuloParaBranco();
                    arquivoHouse.MasterConsignment.IncludedHouseConsignment.ConsigneeParty.PostalStructuredAddress.CityName = entidade.CIDADE_CONSIGNEE.ConverterNuloParaBranco();
                    arquivoHouse.MasterConsignment.IncludedHouseConsignment.ConsigneeParty.PostalStructuredAddress.CountryID = "BR";

                    arquivoHouse.MasterConsignment.IncludedHouseConsignment.FreightForwarderParty.Name = entidade.NOME_AG_EMBARCADOR.ConverterNuloParaBranco();
                    arquivoHouse.MasterConsignment.IncludedHouseConsignment.FreightForwarderParty.PostalStructuredAddress.PostcodeCode = entidade.CEP_AGENTE_EMBARCADOR.ConverterNuloParaBranco();
                    arquivoHouse.MasterConsignment.IncludedHouseConsignment.FreightForwarderParty.PostalStructuredAddress.StreetName = entidade.END_AG_EMBARCADOR.ConverterNuloParaBranco();
                    arquivoHouse.MasterConsignment.IncludedHouseConsignment.FreightForwarderParty.PostalStructuredAddress.CityName = entidade.CIDADE_AG_EMBARCADOR.ConverterNuloParaBranco();
                    arquivoHouse.MasterConsignment.IncludedHouseConsignment.FreightForwarderParty.PostalStructuredAddress.CountryID = entidade.PAIS_EMBARCADOR.ConverterNuloParaBranco();
                    //arquivoHouse.MasterConsignment.IncludedHouseConsignment.ConsigneeParty.DefinedTradeContact.DirectTelephoneCommunication.CompleteNumber = "055"+entidade.TEL_CONSIGNEE.ConverterNuloParaBranco().Insert(3, "9").Replace("(", "").Replace(")", "")/*.Replace("-", "")*/;+
                    arquivoHouse.MasterConsignment.IncludedHouseConsignment.ApplicableTransportCargoInsurance.CoverageInsuranceParty = "";
                    arquivoHouse.MasterConsignment.IncludedHouseConsignment.OriginLocation.ID = entidade.SIGLA_ORIGEM.ConverterNuloParaBranco();
                    arquivoHouse.MasterConsignment.IncludedHouseConsignment.FinalDestinationLocation.ID = entidade.SIGLA_DESTINO.ConverterNuloParaBranco();

                    arquivoHouse.MasterConsignment.IncludedHouseConsignment.IncludedCustomsNote.AddRange(ListaDeIncludedCustomsNote(entidade.CNPJ_CPF.ConverterNuloParaBranco().Replace("-", "").Replace("/","").Replace(".", ""), entidade));

                    arquivoHouse.MasterConsignment.IncludedHouseConsignment.IncludedHouseConsignmentItem = IncluirHouseItem(entidade);


                    return arquivoHouse;
                }
                else
                {
                    Console.WriteLine("Erro = House não pode ser nulo");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
        public async Task<HouseManifest> ConverterEntidadeEmXFHL(ImportacaoAereaEntidade entidade)
        {
            try
            {
                Console.WriteLine(DateTime.Now + "> Convertendo Valores...");
                if (entidade.NR_HOUSE != null && entidade.NR_MASTER != null)
                {
                    HouseManifest arquivoHouse = new HouseManifest($"{entidade.NR_HOUSE.Replace("-", "").Replace(" ", "")}_{entidade.NR_MASTER.Remove(8, 1)}", "Cargo Manifest", "785", "Creation");
                    arquivoHouse.MessageHeaderDocument.senderParties = new List<SenderParty>
                {
                    //new SenderParty{PrimaryID = new SenderPrimaryIDXML{ PrimaryID = "RIEGESOFTWARE", schemeID = "C" } },
                    new SenderParty{PrimaryID = new SenderPrimaryIDXML{ PrimaryID = "REUAGT88FORWARDER/FRA01", schemeID = "P" } }
                };
                    arquivoHouse.MessageHeaderDocument.recipientParties = new List<RecipientParty>
                {
                    new RecipientParty{PrimaryID = new RecipientPrimaryID{ PrimaryID = "CCSPROVIDERNAME", schemeID = "C" } },
                    new RecipientParty{PrimaryID = new RecipientPrimaryID{ PrimaryID = "REUAIR08$CODE", schemeID = "P"} }
                };
                    arquivoHouse.BusinessHeaderDocument.ID = entidade.NR_MASTER.Remove(8, 1);
                    arquivoHouse.MasterConsignment.IncludedTareGrossWeightMeasure.IncludedTareGrossWeightMeasure = entidade.PESO_LIQUIDO.FormatarNumeroParaXML();
                    arquivoHouse.MasterConsignment.TotalPieceQuantity = entidade.QTD_VOLUMES.ToString().Replace(",", ".");
                    arquivoHouse.MasterConsignment.TransportContractDocument.ID = entidade.NR_MASTER.Remove(8, 1);
                    arquivoHouse.MasterConsignment.OriginLocation.ID = entidade.SIGLA_ORIGEM.ConverterNuloParaBranco();
                    arquivoHouse.MasterConsignment.FinalDestinationLocation.ID = entidade.SIGLA_DESTINO.ConverterNuloParaBranco();

                    arquivoHouse.MasterConsignment.IncludedHouseConsignment = IncluirHouseConsignment(entidade);


                    return arquivoHouse;
                }
                else
                {
                    Console.WriteLine("Erro = House não poder ser nulo");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
        private IncludedHouseConsignment IncluirHouseConsignment(ImportacaoAereaEntidade entidade)
        {
            IncludedHouseConsignment includedHouseConsignmentXML = new IncludedHouseConsignment
            {
                SequenceNumeric = "1",
                GrossWeightMeasure = new GrossWeightMeasureMXL { GrossWeightMeasure = entidade.PESO_BRUTO.FormatarNumeroParaXML() },
                TotalPieceQuantity = entidade.QTD_VOLUMES.ToString().Replace(",", "."),
                SummaryDescription = entidade.DESC_MERC.ConverterNuloParaBranco(),
                TransportContractDocument = new TransportContractDocumentXML() { ID = entidade.NR_HOUSE.ConverterNuloParaBranco().Replace("-", "") },
                OriginLocation = new OriginLocationXML() { ID = entidade.SIGLA_ORIGEM.ToString() },
                FinalDestinationLocation = new OriginLocationXML() { ID = entidade.SIGLA_DESTINO.ToString() }
            };
            return includedHouseConsignmentXML;
        }
        private IncludedHouseConsignmentItemXML IncluirHouseItem(ImportacaoAereaEntidade entidade)
        {
            IncludedHouseConsignmentItemXML includedHouseConsignmentItemXML = new IncludedHouseConsignmentItemXML
            {
                SequenceNumeric = 1,
                GrossWeightMeasure = new GrossWeightMeasureMXL { GrossWeightMeasure = entidade.PESO_BRUTO.FormatarNumeroParaXML() },
                PackageQuantity = entidade.QTD_VOLUMES.FormatarNumeroParaXML(),
                PieceQuantity = entidade.QTD_VOLUMES.FormatarNumeroParaXML(),
                Information = "NDA",
                NatureIdentificationTransportCargo = new NatureIdentificationTransportCargoXML { Identification = entidade.DESC_MERC.ConverterNuloParaBranco() },
                ApplicableFreightRateServiceCharge = new ApplicableFreightRateServiceChargeXML { ChargeableWeightMeasure = new ChargeableWeightMeasureXML { ChargeableWeightMeasure = entidade.PESO_LIQUIDO.FormatarNumeroParaXML() } }
            };
            return includedHouseConsignmentItemXML;
        }
        private List<IncludedCustomsNoteXML> ListaDeIncludedCustomsNote(string cnpjConsignee, ImportacaoAereaEntidade entidade = null)
        {
            List<IncludedCustomsNoteXML> lista = new List<IncludedCustomsNoteXML>();
            IncludedCustomsNoteXML includeCNE = new IncludedCustomsNoteXML
            {
                CountryID = "BR",
                SubjectCode = "CNE",
                ContentCode = "T",
                Content = "CNPJ" + cnpjConsignee
            };
            IncludedCustomsNoteXML includeIMP = new IncludedCustomsNoteXML
            {
                CountryID = "BR",
                SubjectCode = "IMP",
                ContentCode = "U",
                Content = "UCR8BR167017161001713DOOOOOOOO1"
            };
            IncludedCustomsNoteXML includeDI = new IncludedCustomsNoteXML
            {
                CountryID = "BR",
                SubjectCode = "OCI",
                ContentCode = "DI",
                Content = "WOOD PARTS"
            };
            IncludedCustomsNoteXML includeCCL = new IncludedCustomsNoteXML
            {
                CountryID = "BR",
                SubjectCode = "CCL",
                ContentCode = "M",
                Content = $"CUSTOMSWAREHOUSE{entidade.CODIGO_RECINTO}"
            };
            lista.Add(includeCNE);
            //lista.Add(includeIMP);
            if(entidade.CODIGO_RECINTO != "" || entidade.CODIGO_RECINTO != null)
            {
                lista.Add(includeCCL);
            }
            
            //lista.Add(includeDI);
            return lista;
        }
        public async Task<ImportacaoAereaEntidade> BuscarProcessoPorReferencia(string nReferencia)
        {
            try
            {
                return await _repositorio.Consultar(nReferencia);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

    }
}
