using GLB.CCT.Entidade;
using GLB.CCT.EntidadeXML.ConsultaCCT;
using GLB.CCT.EntidadeXML.EnvioCCT;
using GLB.CCT.Persistencia;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
                    //var xmlD = XDocument.Parse(a);
                    //Console.WriteLine(xmlD);
                    HttpResponseMessage response = await client.PostAsync(enderecoXFZB, stringContent);
                    var responseContent = response.Content.ReadAsStringAsync().Result;
                    // Lê a resposta HTTP
                    if (response.IsSuccessStatusCode)
                    {
                        var xmlResponse = XDocument.Parse(responseContent);

                        Console.WriteLine(xmlResponse.ToString());
                        string[] protocolo = responseContent.Split("Reason");
                        var result = comunicacao.RetornaResultado(autenticar, protocolo[1].Replace("</", "").Replace(">", ""));

                        Console.WriteLine(result.Result);
                        //string tempFilePath = Path.Combine(Path.GetTempPath(), "response.txt");
                        //File.WriteAllText(tempFilePath, JeitinhoBrasileiro);

                        // Abrir o Bloco de Notas com o arquivo temporário
                        //System.Diagnostics.Process.Start("notepad.exe", tempFilePath);
                        Console.WriteLine("Aperte qualquer botão para sair da tela");
                        Console.ReadLine();
                        return true;
                    }
                    else
                    {
                        Console.WriteLine(DateTime.Now + "> Falha na requisição ENVIO XFZB: " + response.StatusCode);
                        Console.WriteLine(responseContent);
                        // Criar um arquivo temporário com o texto
                        //string tempFilePath = Path.Combine(Path.GetTempPath(), "responsexfzb.txt");
                        //File.WriteAllText(tempFilePath, JeitinhoBrasileiro);

                        // Abrir o Bloco de Notas com o arquivo temporário
                        //System.Diagnostics.Process.Start("notepad.exe", tempFilePath);
                        Console.WriteLine("Aperte qualquer botão para sair da tela");
                        Console.ReadLine();
                        return false;
                    }

                }
            }
            catch (Exception ex)
            {
                if(ex.Message == "Request headers must contain only ASCII character.")
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

            var autenticar = await comunicacao.Autenticar(_tipoPerfil);
            if (autenticar != null)
            {
                string enderecoXFZB = $"{URLPadraoValidacao}{URLEnvioHouseXFHL}?cnpj={consultas.BuscaCNPJ(nReferencia).Replace(".", "").Replace("/", "").Replace("-", "")}";

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
                string xmlContent = File.ReadAllText("C:\\Users\\DEV-NATALI\\Desktop\\GlobalCCT\\test.xml");

                string xmlSt = LoadXmlToString(xmlContent);

                XNamespace ns2 = "iata:housemanifest:1";

                XElement root = XElement.Parse(xmlString);

                var includedHouseConsignments = root.Descendants(ns2 + "IncludedHouseConsignment").Elements(ns2 + "IncludedHouseConsignment").ToList();

                foreach (var includedHouseConsignment in includedHouseConsignments)
                {
                    includedHouseConsignment.ReplaceWith(includedHouseConsignment.Elements());
                }

                var doc = XDocument.Parse(xmlString.FormataCaratere());
                doc.Declaration = null;
                var client = await comunicacao.RetornarClientDeEnvio(autenticar, xmlSt/*xmlString*/.Replace("Ã", "A"));
                var document = GeraHeaderXFHL();
                var JeitinhoBrasileiro = GeraXML(document.ToString().Replace("/>", ">"), doc.ToString().Remove(0, 139).Replace(" <DefinedTradeContact>", "").Replace(" <DirectTelephoneCommunication />", "").Replace("</DefinedTradeContact>", "<DefinedTradeContact />").Replace("MasterConsignment", "ns2:MasterConsignment").Replace("BusinessHeaderDocument", "ns2:BusinessHeaderDocument").Replace("HouseManifest", "ns2:HouseManifest").Replace("MessageHeaderDocument", "ns2:MessageHeaderDocument").Replace("Ã", "A"));
                var xmlD = XDocument.Parse(JeitinhoBrasileiro);

                var stringContent = new StringContent(xmlSt, Encoding.UTF8, "application/xml");
                //Console.WriteLine(xmlD);
                //var xmlD = XDocument.Parse(a);

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
                    Console.ReadLine();
                    return true;
                }
                else
                {
                    Console.WriteLine(DateTime.Now + "> Falha na requisição ENVIO XFZB: " + response.StatusCode);
                    //string tempFilePath = Path.Combine(Path.GetTempPath(), "response.txt");
                    //File.WriteAllText(tempFilePath, JeitinhoBrasileiro);
                    Console.WriteLine(responseContent);
                    // Abrir o Bloco de Notas com o arquivo temporário
                    //System.Diagnostics.Process.Start("notepad.exe", tempFilePath);
                    Console.WriteLine("Aperte qualquer botão para sair da tela");
                    Console.ReadLine();
                    return false;
                }

            }
            return false;
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
