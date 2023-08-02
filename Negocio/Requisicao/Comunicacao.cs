using GLB.CCT.EntidadeXML.ConsultaCCT;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Xml.Linq;
using static GLB.CCT.Negocio.Requisicao.URLIntegracao;

namespace GLB.CCT.Negocio.Requisicao
{
    public class Comunicacao
    {

        public async Task<RetornoAutenticar> Autenticar(TipoPerfilEnum perfil)
        {
            try
            {
                Console.WriteLine(DateTime.Now + "> Autenticando");

                RetornoAutenticar xmlRetornoAutenticar = new RetornoAutenticar();
                // Cria uma instância do cliente HTTP
                // Crie uma instância do cliente HTTP e configure o certificado
                var certificado = new Certificado();
                var httpClientHandler = new HttpClientHandler();
                httpClientHandler.ClientCertificates.Add(certificado.BuscaNome(""));
                HttpClient client = new HttpClient(httpClientHandler);

                // Configura a validação do certificado SSL
                ServicePointManager.ServerCertificateValidationCallback = ValidateServerCertificate;
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;
                ServicePointManager.ServerCertificateValidationCallback = (s, c, n, p) => { return true; };

                // Configura o cliente para usar SSL
                client.BaseAddress = new Uri(URLPadraoValidacao);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Role-Type", perfil.ToString());
                client.DefaultRequestHeaders.Add("System-Code", "XFZ");


                // Envia uma requisição HTTP GET para a URL especificada
                var response = await client.GetAsync(URLAutenticar);

                // Lê a resposta HTTP
                if (response.IsSuccessStatusCode)
                {
                    string responseContent = response.Content.ReadAsStringAsync().Result;
                    xmlRetornoAutenticar.SetToken = response.Headers.GetValues("set-token").First();
                    xmlRetornoAutenticar.CSRFToken = response.Headers.GetValues("x-csrf-token").First();

                    xmlRetornoAutenticar.httpClientHandler = httpClientHandler;
                    Console.WriteLine(DateTime.Now + "> Autenticado com sucesso");
                }
                else
                {
                    Console.WriteLine(DateTime.Now + "> Falha na requisição HTTP: " + response.StatusCode);
                }

                return xmlRetornoAutenticar;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }
        // Método para validar o certificado SSL
        private static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            if (sslPolicyErrors == SslPolicyErrors.None)
            {
                return true;
            }

            Console.WriteLine(DateTime.Now + "> Falha na validação do certificado SSL: " + sslPolicyErrors.ToString());
            return false;
        }
        public async Task<HttpClient> RetornarClient(RetornoAutenticar arquivoAutenticado)
        {
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri(URLPadraoValidacao);
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            client.DefaultRequestHeaders.Add("Authorization", arquivoAutenticado.SetToken);
            client.DefaultRequestHeaders.Add("X-CSRF-Token", arquivoAutenticado.CSRFToken);
            return client;
        }
        public async Task<HttpClient> RetornarClientDeEnvio(RetornoAutenticar arquivoAutenticado, string xml)
        {
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri(URLPadraoValidacao);
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));

            client.DefaultRequestHeaders.Add("Authorization", arquivoAutenticado.SetToken);
            client.DefaultRequestHeaders.Add("X-CSRF-Token", arquivoAutenticado.CSRFToken);
            client.DefaultRequestHeaders.Add("body", xml);
            
            return client;
        }
        public async Task<string> RetornaResultado(RetornoAutenticar arquivoAutenticado, string protocolo)
        {
            HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Clear();

            client.DefaultRequestHeaders.Add("Authorization", arquivoAutenticado.SetToken);
            client.DefaultRequestHeaders.Add("X-CSRF-Token", arquivoAutenticado.CSRFToken);
            //client.DefaultRequestHeaders.Add("body", xml);

            HttpResponseMessage a = await client.GetAsync("https://val.portalunico.siscomex.gov.br/ccta/api/ext/check/received-files/" + protocolo);
            var responseContent = a.Content.ReadAsStringAsync().Result;
            var xmlResponse = JObject.Parse(responseContent);
            return xmlResponse.ToString();

            //return "";
        }
    }
}
