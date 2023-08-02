using GLB.CCT.EntidadeXML.ConsultaCCT;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using static GLB.CCT.Negocio.Requisicao.URLIntegracao;

namespace GLB.CCT.Negocio.Requisicao
{
    public class ConsultarViagensPorConhecimento
    {
        private string _numeroManifesto;
        private TipoPerfilEnum _tipoPerfil;
        public ConsultarViagensPorConhecimento(string numeroManifesto, TipoPerfilEnum tipoPerfil)
        {
            _numeroManifesto = numeroManifesto;
            _tipoPerfil = tipoPerfil;
        }
        public async Task<XMLConsultaChegadasCCT> ConsultarChegadas ()
        {
            Comunicacao comunicacao = new Comunicacao();

            var autenticar = await comunicacao.Autenticar(_tipoPerfil);
            if (autenticar != null)
            {
                string enderecoViagem = string.Format(URLViagens, _numeroManifesto);
                //enderecoViagem = URLPadraoValidacao + enderecoViagem;
                var client = await comunicacao.RetornarClient(autenticar);
                HttpResponseMessage response = await client.GetAsync(enderecoViagem);
                // Lê a resposta HTTP
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStreamAsync();
                    XMLConsultaChegadasCCT xmlChegadas = await JsonSerializer.DeserializeAsync<XMLConsultaChegadasCCT>(responseContent);
                    return xmlChegadas;
                }
                else
                {
                    Console.WriteLine(DateTime.Now + "> Falha na requisição VIAGEM: " + response.StatusCode);
                    return null;
                }

            }
            return null;

        }
    }
}
