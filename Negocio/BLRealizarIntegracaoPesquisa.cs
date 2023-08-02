using GLB.CCT.Entidade;
using GLB.CCT.EntidadeXML.ConsultaCCT;
using GLB.CCT.Negocio.Requisicao;
using GLB.CCT.Persistencia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GLB.CCT.Negocio.EnumSistema;
using static GLB.CCT.Negocio.Requisicao.URLIntegracao;

namespace GLB.CCT.Negocio
{
    public class BLRealizarIntegracaoPesquisa
    {
        private string _nReferencia;
        private string _numeroManifestoOuConhecimento;
        private TipoPerfilEnum _tipoPefil = TipoPerfilEnum.IMPEXP;
        private TipoProcessoEnum _tipoProcesso;
        private TipoConsultaCCT _tipoConsulta; 
        public BLRealizarIntegracaoPesquisa(string nReferencia, TipoProcessoEnum tipoProcessoEnum, TipoConsultaCCT tipoConsultaCCT, string numeroManifestoOuConhecimento)
        {
            _tipoProcesso = tipoProcessoEnum;
            _tipoConsulta = tipoConsultaCCT;
            _nReferencia = nReferencia;
            _numeroManifestoOuConhecimento = numeroManifestoOuConhecimento;
        }
        public async Task<bool> Integrar()
        {
            try
            {
                switch (_tipoConsulta)
                {
                    case TipoConsultaCCT.ConsultaViagem:
                        return await IntegrarConsultaChegada();
                    case TipoConsultaCCT.ConsultaConhecimentos:
                        return await IntegrarConsultaChegada();
                    default:
                        break;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return false;
            }
        }
        private async Task<bool> IntegrarConsultaChegada()
        {
            ConsultarViagensPorConhecimento consultarViagensPorConhecimento = new ConsultarViagensPorConhecimento(_numeroManifestoOuConhecimento, _tipoPefil);
            var xmlChegadas = await consultarViagensPorConhecimento.ConsultarChegadas();
            if (xmlChegadas != null)
            {
                switch (_tipoProcesso)
                {
                    case TipoProcessoEnum.ImportacaoAerea:
                        return await IntegrarChegadaImportacaoAerea(xmlChegadas);
                    case TipoProcessoEnum.DesembaracoImportacao:
                        return await IntegrarChegadaDesembaracoImportacao(xmlChegadas);
                    default:
                        break;
                }
            }
            return true;
        }
        private async Task<bool> IntegrarChegadaImportacaoAerea(XMLConsultaChegadasCCT xmlConsulta)
        {
            BLImportacaoAerea bLImportacao = new BLImportacaoAerea();
            var entidadeImportacaoAerea = await bLImportacao.ConverterXMLChegadaEmEntidade(xmlConsulta, _nReferencia);
            await bLImportacao.AtualizarImportacaoAerea(entidadeImportacaoAerea);
            return true;

        }
        private async Task<bool> IntegrarChegadaDesembaracoImportacao(XMLConsultaChegadasCCT xmlConsulta)
        {
            BLDesembaracoImportacao bLDesembaracoImportacao = new BLDesembaracoImportacao();
            var entidadeDesembaraco = await bLDesembaracoImportacao.ConverterXMLChegadaEmEntidade(xmlConsulta, _nReferencia);
            await bLDesembaracoImportacao.AtualizarDesembaracoImportacao(entidadeDesembaraco);
            return true;
        }
        private async Task<bool> IntegrarConsultaConhecimentos()
        {
            ConsultaPorConhecimento consultaPorConhecimento = new ConsultaPorConhecimento(_numeroManifestoOuConhecimento, _tipoPefil);
            var xmlChegadas = consultaPorConhecimento.ConsultarConhecimentos();
            if (xmlChegadas != null)
            {
                switch (_tipoProcesso)
                {
                    case TipoProcessoEnum.ImportacaoAerea:
                        //return await IntegrarChegadaImportacaoAerea(xmlChegadas);
                    case TipoProcessoEnum.DesembaracoImportacao:
                        //return await IntegrarChegadaDesembaracoImportacao(xmlChegadas);
                    default:
                        break;
                }
            }
            return true;
        }

        //private async Task<bool> IntegrarConhecimentosImportacaoAerea (XMLconhecimentos xMLconhecimentos)
        //{

        //}
    }
}
