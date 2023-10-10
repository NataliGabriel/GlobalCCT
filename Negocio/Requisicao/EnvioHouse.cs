using GLB.CCT.Entidade;
using GLB.CCT.EntidadeXML.ConsultaCCT;
using GLB.CCT.EntidadeXML.EnvioCCT;
using GLB.CCT.Persistencia;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;
using System.Linq;
using System.Net.Http;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml.XPath;
using static GLB.CCT.Negocio.Requisicao.URLIntegracao;

namespace GLB.CCT.Negocio.Requisicao
{
    public class EnvioHouse
    {
        private TipoPerfilEnum _tipoPerfil;
        private ImportacaoAereaRepositorio consultas = new ImportacaoAereaRepositorio();
        public EnvioHouse(TipoPerfilEnum tipoPerfilEnum)
        {
            _tipoPerfil = tipoPerfilEnum;
        }
        public async Task<bool> EnviarXFZB(HouseWaybill xml, string nReferencia)
        {
            Comunicacao comunicacao = new Comunicacao();
            try
            {
                var autenticar = await comunicacao.Autenticar(_tipoPerfil);

                if (autenticar != null)
                {
                    string enderecoXFZB = $"{URLPadraoValidacao}{URLEnvioHouseXFZB}?cnpj={consultas.BuscaCNPJ(nReferencia).Replace(".", "").Replace("/", "").Replace("-", "")}";
                    Console.WriteLine($"{URLPadraoValidacao}{URLEnvioHouseXFZB}?cnpj=****************");
                    XmlSerializer xmlSerializer = new XmlSerializer(xml.GetType());

                    string xmlString = "";
                    using (var sww = new Utf8StringWriter())
                    {
                        using (XmlWriter writer = XmlWriter.Create(sww))
                        {
                            xmlSerializer.Serialize(writer, xml);
                            xmlString = sww.ToString().FormataXML();
                        }
                    }
                    //string xmlContent = File.ReadAllText("C:\\Users\\user\\Desktop\\CCT3\\GlobalCCT\\teste.xml");

                    //string xmlSt = LoadXmlToString(xmlContent);
                    var doc = XDocument.Parse(xmlString.FormataCaratere());
                    doc.Declaration = null;
                    var xmlA = XDocument.Parse(xmlString);
                    var client = await comunicacao.RetornarClientDeEnvio(autenticar, xmlString.FormataCaratere());
                    var document = GeraCabecalhoXFZB();
                    var JeitinhoBrasileiro = GeraXML(document.ToString().Replace("/>", ">"), doc.ToString().Remove(0, 139).Replace("<TransportContractDocument />", "").Replace(" <DefinedTradeContact>", "").Replace(" <DirectTelephoneCommunication />", "").Replace("</DefinedTradeContact>", "<DefinedTradeContact />").Replace("MasterConsignment", "ns2:MasterConsignment").Replace("BusinessHeaderDocument", "ns2:BusinessHeaderDocument").Replace("HouseWaybill", "ns2:HouseWaybill").Replace("MessageHeaderDocument", "ns2:MessageHeaderDocument").FormataCaratere());
                    var xmlD = XDocument.Parse(JeitinhoBrasileiro);

                    var stringContent = new StringContent(JeitinhoBrasileiro, Encoding.UTF8, "application/xml");

                    return await PostAsyncCCT(client, stringContent, autenticar, nReferencia);

                }
            }
            catch (Exception ex)
            {
                if (ex.Message == "Request headers must contain only ASCII character.")
                {
                    MessageBox.Show("Favor verificar todos os campos dos cadastros do Importador e Exportador e/ou descrição, referente a caractere especial.", "ERRO!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                Console.WriteLine(ex.Message);
            }
            return false;
        }

        public async Task<bool> EnviarXFHL(HouseManifest xml, string nReferencia)
        {
            Comunicacao comunicacao = new Comunicacao();

            XmlDocument xmlDocument = new XmlDocument();

            int tamanhoMaximo = 12;
            var autenticar = await comunicacao.Autenticar(_tipoPerfil);
            if (autenticar != null)
            {
                XmlSerializer xmlSerializer = new XmlSerializer(xml.GetType());

                string xmlString = "";
                using (var sww = new Utf8StringWriter())
                {
                    using (XmlWriter writer = XmlWriter.Create(sww))
                    {
                        xmlSerializer.Serialize(writer, xml);
                        xmlString = sww.ToString().FormataXML();
                    }
                }

                xmlDocument.LoadXml(xmlString);
                var a = XDocument.Parse(xmlDocument.OuterXml);
                XmlNodeList itemNodes = xmlDocument.SelectNodes("//iata:IncludedHouseConsignment", GetNamespaceManager(xmlDocument));
                if (itemNodes.Count > 12)
                { 
                    List<XmlNode> primeiroIncludedHouse = ObterProximoIncludedHouse(itemNodes, 0, tamanhoMaximo);
                    var primeiraIncludedHouse = GerarXML(primeiroIncludedHouse, xmlDocument);
                    var doc = XDocument.Parse(primeiraIncludedHouse.FormataCaratere());
                    doc.Declaration = null;
                    var document = GeraHeaderXFHL();
                    var client = await comunicacao.RetornarClientDeEnvio(autenticar, primeiraIncludedHouse.Replace("<HouseManifest xmlns=\"iata:datamodel:3\" xmlns:ns2=\"iata:housemanifest:1\">", "<HouseManifest xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns=\"iata:datamodel:3\">").Replace("<IncludedHouseConsignment xmlns=\"iata:datamodel:3\">", "").Replace("Ã", "A").FormataCaratere());
                    var JeitinhoBrasileiro = doc.ToString().Replace("<IncludedHouseConsignment xmlns=\"iata:datamodel:3\">", "").Replace(" <DefinedTradeContact>", "").Replace(" <DirectTelephoneCommunication />", "").Replace("</DefinedTradeContact>", "<DefinedTradeContact />").Replace("MasterConsignment", "ns2:MasterConsignment").Replace("BusinessHeaderDocument", "ns2:BusinessHeaderDocument").Replace("HouseManifest", "ns2:HouseManifest").Replace("MessageHeaderDocument", "ns2:MessageHeaderDocument").Replace("Ã", "A").Replace("<Lote>", "").Replace("</Lote>","");
                    var xmlD = XDocument.Parse(JeitinhoBrasileiro);
                    var stringContent = new StringContent(JeitinhoBrasileiro, Encoding.UTF8, "application/xml");

                    if (PostAsyncCCT(client, stringContent, autenticar, nReferencia).Result)
                    {
                        int indice = tamanhoMaximo; int falta = 0;
                        while (indice <= itemNodes.Count)
                        {
                            List<XmlNode> loteAtual = ObterProximoIncludedHouse(itemNodes, indice, tamanhoMaximo, falta);

                            var xmlSeguintes = GerarXML(loteAtual, xmlDocument);
                            doc = null;
                            doc = XDocument.Parse(xmlSeguintes);
                            client = await comunicacao.RetornarClientDeEnvio(autenticar, xmlSeguintes.Replace("Ã", "A").Replace("Creation", "Update"));
                            JeitinhoBrasileiro = doc.ToString().Replace("Creation", "Update").Replace(" <DefinedTradeContact>", "").Replace(" <DirectTelephoneCommunication />", "").Replace("</DefinedTradeContact>", "<DefinedTradeContact />").Replace("MasterConsignment", "ns2:MasterConsignment").Replace("BusinessHeaderDocument", "ns2:BusinessHeaderDocument").Replace("HouseManifest", "ns2:HouseManifest").Replace("MessageHeaderDocument", "ns2:MessageHeaderDocument").Replace("Ã", "A");
                            xmlD = XDocument.Parse(JeitinhoBrasileiro);
                            stringContent = new StringContent(JeitinhoBrasileiro, Encoding.UTF8, "application/xml");

                            await PostAsyncCCT(client, stringContent, autenticar, nReferencia);

                            indice += tamanhoMaximo;
                            if (indice > itemNodes.Count)
                            {
                                falta = itemNodes.Count - (indice - tamanhoMaximo);
                                indice = indice - tamanhoMaximo + falta;
                            }
                        }
                        Console.ReadKey();
                    }
                }
                else
                {
                    var doc = XDocument.Parse(xmlString.FormataCaratere());
                    doc.Declaration = null;
                    var client = await comunicacao.RetornarClientDeEnvio(autenticar, xmlString.Replace("Ã", "A"));
                    var document = GeraHeaderXFHL();
                    var JeitinhoBrasileiro = GeraXML(document.ToString().Replace("/>", ">"), doc.ToString().Remove(0, 139).Replace(" <DefinedTradeContact>", "").Replace(" <DirectTelephoneCommunication />", "").Replace("</DefinedTradeContact>", "<DefinedTradeContact />").Replace("MasterConsignment", "ns2:MasterConsignment").Replace("BusinessHeaderDocument", "ns2:BusinessHeaderDocument").Replace("HouseManifest", "ns2:HouseManifest").Replace("MessageHeaderDocument", "ns2:MessageHeaderDocument").Replace("Ã", "A"));
                    var xmlD = XDocument.Parse(JeitinhoBrasileiro);

                    var stringContent = new StringContent(xmlString, Encoding.UTF8, "application/xml");
                    return await PostAsyncCCT(client, stringContent, autenticar, nReferencia);
                }
            }
            return false;
        }
        public static string GerarXML(List<XmlNode> IncludedHouse, XmlDocument xml)
        {
            try
            {
                XmlDocument xmlLote = new XmlDocument();

                XmlNode messageHeaderNode = xml.SelectSingleNode("//iata:MessageHeaderDocument", GetNamespaceManager(xml));
                XmlNode businessHeaderNode = xml.SelectSingleNode("//iata:BusinessHeaderDocument", GetNamespaceManager(xml));
                XmlNode MasterConsignmentNode = xml.SelectSingleNode("//iata:MasterConsignment", GetNamespaceManager(xml));



                XmlNode includedTareGrossWeightMeasureNode = xml.SelectSingleNode("//iata:IncludedTareGrossWeightMeasure", GetNamespaceManager(xml));
                XmlNode totalPieceQuantityNode = xml.SelectSingleNode("//iata:TotalPieceQuantity", GetNamespaceManager(xml));
                XmlNode transportContractIdNode = xml.SelectSingleNode("//iata:TransportContractDocument", GetNamespaceManager(xml));
                XmlNode originLocationIdNode = xml.SelectSingleNode("//iata:OriginLocation", GetNamespaceManager(xml));
                XmlNode finalDestinationLocationIdNode = xml.SelectSingleNode("//iata:FinalDestinationLocation", GetNamespaceManager(xml));

                if (messageHeaderNode != null && businessHeaderNode != null)
                {
                    XmlElement loteElement = xmlLote.CreateElement("HouseManifest");

                    // Adicione o conteúdo de MessageHeaderDocument ao elemento raiz do lote
                    XmlNode clonedMessageHeaderNode = xmlLote.ImportNode(messageHeaderNode, true);
                    loteElement.AppendChild(clonedMessageHeaderNode);

                    // Adicione o conteúdo de BusinessHeaderDocument ao elemento raiz do lote
                    XmlNode clonedBusinessHeaderNode = xmlLote.ImportNode(businessHeaderNode, true);
                    loteElement.AppendChild(clonedBusinessHeaderNode);

                    XmlNode clonedincludedTareGrossWeightMeasureNode = xmlLote.ImportNode(includedTareGrossWeightMeasureNode, true);
                    loteElement.AppendChild(clonedincludedTareGrossWeightMeasureNode);

                    XmlNode clonedtotalPieceQuantityNode = xmlLote.ImportNode(totalPieceQuantityNode, true);
                    loteElement.AppendChild(clonedtotalPieceQuantityNode);

                    XmlNode clonedtransportContractIdNode = xmlLote.ImportNode(transportContractIdNode, true);
                    loteElement.AppendChild(clonedtransportContractIdNode);

                    XmlNode clonedoriginLocationIdNode = xmlLote.ImportNode(originLocationIdNode, true);
                    loteElement.AppendChild(clonedoriginLocationIdNode);

                    XmlNode clonedfinalDestinationLocationIdNode = xmlLote.ImportNode(finalDestinationLocationIdNode, true);
                    loteElement.AppendChild(clonedfinalDestinationLocationIdNode);

                    // Adicione os itens do lote como filhos do novo elemento raiz
                    foreach (var itemNode in IncludedHouse)
                    {
                        XmlNode clonedNode = xmlLote.ImportNode(itemNode, true);
                        loteElement.AppendChild(clonedNode);
                    }

                    // Adicione o elemento raiz ao documento do lote
                    xmlLote.AppendChild(loteElement);

                    return xmlLote.OuterXml.Replace("TransportContractDocument xmlns=\"iata:datamodel:3\"", "TransportContractDocument").Replace("TotalPieceQuantity xmlns=\"iata:datamodel:3\"", "TotalPieceQuantity").Replace("<HouseManifest>", "<HouseManifest xmlns=\"iata:datamodel:3\" xmlns:ns2=\"iata:housemanifest:1\">").Replace("MessageHeaderDocument xmlns=\"iata:datamodel:3\"", "MessageHeaderDocument").Replace("FinalDestinationLocation xmlns=\"iata:datamodel:3\"", "FinalDestinationLocation").Replace("OriginLocation xmlns=\"iata:datamodel:3\"", "OriginLocation").Replace("IncludedTareGrossWeightMeasure unitCode=\"KGM\" xmlns=\"iata:datamodel:3\"", "IncludedTareGrossWeightMeasure unitCode=\"KGM\"").Replace("BusinessHeaderDocument xmlns=\"iata:datamodel:3\"", "BusinessHeaderDocument").Replace("IncludedHouseConsignment xmlns=\"iata:datamodel:3\"", "IncludedHouseConsignment").Replace("</BusinessHeaderDocument>", "  </BusinessHeaderDocument>\r\n  <MasterConsignment>").Replace("</HouseManifest>", "  </MasterConsignment>\r\n</HouseManifest>");
                }
                return "";
            }catch (Exception ex) { Console.WriteLine(ex.Message); return null; }
        }
        async Task<bool> PostAsyncCCT(HttpClient client, StringContent stringContent, RetornoAutenticar autenticar, string nReferencia)
        {
            Comunicacao comunicacao = new Comunicacao();
            string enderecoXFZB = $"{URLPadraoValidacao}{URLEnvioHouseXFHL}?cnpj={consultas.BuscaCNPJ(nReferencia).Replace(".", "").Replace("/", "").Replace("-", "")}";

            HttpResponseMessage response = await client.PostAsync(enderecoXFZB, stringContent);
            var responseContent = response.Content.ReadAsStringAsync().Result;
            // Lê a resposta HTTP
            if (response.IsSuccessStatusCode)
            {
                var xmlResponse = XDocument.Parse(responseContent);

                string[] protocolo = responseContent.Split("Reason");
                var result = comunicacao.RetornaResultado(autenticar, protocolo[1].Replace("</", "").Replace(">", ""));

                Console.WriteLine(result.Result);
                Console.WriteLine("Aperte qualquer botão para sair da tela");

                return true;
            }
            else
            {
                Console.WriteLine(DateTime.Now + "> Falha na requisição ENVIO XFZB: " + response.StatusCode);
                Console.WriteLine(responseContent);
                Console.WriteLine("Aperte qualquer botão para sair da tela");
                Console.ReadLine();
                return false;
            }
        }
        static List<XmlNode> ObterProximoIncludedHouse(XmlNodeList nodes, int startIndex, int tamanhoDoLote, int falta = 0)
        {
            List<XmlNode> lote = new List<XmlNode>();
            startIndex = startIndex - falta;
            for (int i = startIndex; i < startIndex + tamanhoDoLote && i < nodes.Count; i++)
            {
                lote.Add(nodes[i]);
            }
            return lote;
        }
        static XmlNamespaceManager GetNamespaceManager(XmlDocument xmlDoc)
        {
            XmlNamespaceManager namespaceManager = new XmlNamespaceManager(xmlDoc.NameTable);
            namespaceManager.AddNamespace("iata", "iata:datamodel:3");

            return namespaceManager;
        }
        static XmlNamespaceManager GetXmlNamespaceManager(XmlDocument xmlDoc)
        {
            XmlNamespaceManager nsManager = new XmlNamespaceManager(xmlDoc.NameTable);
            nsManager.AddNamespace("ns2", "iata:housemanifest:1");
            nsManager.AddNamespace("iata", "iata:datamodel:3");
            return nsManager;
        }
        static string LoadXmlToString(string xmlContent)
        {
            try
            {
                // Cria um leitor de XML usando a string de conteúdo
                using (StringReader stringReader = new StringReader(xmlContent))
                {
                    using (XmlReader xmlReader = XmlReader.Create(stringReader))
                    {
                        // Carrega o XML em uma variável string
                        XmlDocument xmlDoc = new XmlDocument();
                        xmlDoc.Load(xmlReader);
                        return xmlDoc.InnerXml;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao carregar o XML: " + ex.Message);
                return null;
            }
        }
        private XDocument GeraHeaderXFHL(XDocument document = null)
        {
            XNamespace ns2 = "iata:housemanifest:1";
            XNamespace defaultNs = "iata:datamodel:3";

            return document = new XDocument(new XDeclaration("1.0", "UTF-8", "yes"),
                                                new XElement(ns2 + "HouseManifest",
                                                    new XAttribute(name: "xmlns", value: defaultNs),
                                                    new XAttribute(XNamespace.Xmlns + "ns2", ns2)));
        }
        private XDocument GeraCabecalhoXFZB(XDocument document = null)
        {
            XNamespace defaultNs = "iata:datamodel:3";
            XNamespace ns6 = "urn:un:unece:uncefact:codelist:standard:IANA:MIMEMediaType:2009-09-01";
            XNamespace ns5 = "urn:un:unece:uncefact:codelist:standard:UNECE:TransportModeCode:2";
            XNamespace ns8 = "urn:un:unece:uncefact:codelist:standard:6:Recommendation20:6";
            XNamespace ns7 = "urn:un:unece:uncefact:codelist:standard:6:0133:40106";
            XNamespace ns9 = "urn:un:unece:uncefact:codelist:standard:UNECE:DocumentNameCode:D09A";
            XNamespace ns12 = "urn:un:unece:uncefact:codelist:standard:5:ISO42173A:2009-09-09";
            XNamespace ns11 = "urn:un:unece:uncefact:identifierlist:standard:5:ISO316612A:SecondEdition2006VI-6";
            XNamespace ns10 = "urn:un:unece:uncefact:codelist:standard:6:3055:D09A";
            XNamespace ns2 = "iata:housewaybill:1";
            XNamespace ns4 = "urn:un:unece:uncefact:codelist:standard:IANA:CharacterSetCode:2007-05-14";
            XNamespace ns3 = "urn:un:unece:uncefact:codelist:standard:UNECE:PartyRoleCode:D09A";

            return document = new XDocument(
                                                new XDeclaration("1.0", "UTF-8", "yes"),
                                                new XElement(ns2 + "HouseWaybill",
                                                new XAttribute(name: "xmlns", value: defaultNs),
                                                new XAttribute(XNamespace.Xmlns + "ns6", ns6),
                                                new XAttribute(XNamespace.Xmlns + "ns5", ns5),
                                                new XAttribute(XNamespace.Xmlns + "ns8", ns8),
                                                new XAttribute(XNamespace.Xmlns + "ns7", ns7),
                                                new XAttribute(XNamespace.Xmlns + "ns9", ns9),
                                                new XAttribute(XNamespace.Xmlns + "ns12", ns12),
                                                new XAttribute(XNamespace.Xmlns + "ns11", ns11),
                                                new XAttribute(XNamespace.Xmlns + "ns10", ns10),
                                                new XAttribute(XNamespace.Xmlns + "ns2", ns2),
                                                new XAttribute(XNamespace.Xmlns + "ns4", ns4),
                                                new XAttribute(XNamespace.Xmlns + "ns3", ns3)));
        }
        public string GeraXML(string cabecalho, string xml)
        {
            return "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?> \r\n" + cabecalho + xml;
        }
        public class Utf8StringWriter : StringWriter
        {
            public override Encoding Encoding => Encoding.UTF8;
        }
    }
}
