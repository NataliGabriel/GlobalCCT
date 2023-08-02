
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
    public class BLRealizarIntegracaoEnvio
    {
        private string _nReferencia;
        private TipoPerfilEnum _tipoPefil = TipoPerfilEnum.AGECARGA;
        private TipoProcessoEnum _tipoProcesso;
        private TipoEnvioCCT _tipoEnvio;
        public BLRealizarIntegracaoEnvio(string nReferencia, TipoProcessoEnum tipoProcessoEnum, TipoEnvioCCT TipoEnvioCCT)
        {
            _tipoProcesso = tipoProcessoEnum;
            _tipoEnvio = TipoEnvioCCT;
            _nReferencia = nReferencia;
        }
        public async Task<bool> Integrar()
        {
            try
            {
                switch (_tipoEnvio)
                {
                    case TipoEnvioCCT.EnviarHouseXFZB:
                        return await IntegrarEnvioHouse();
                    case TipoEnvioCCT.EnviarHouseXFHL:
                        return await IntegrarEnvioHouse();
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
        private async Task<bool> IntegrarEnvioHouse()
        {
            switch (_tipoProcesso)
            {
                case TipoProcessoEnum.ImportacaoAerea:
                    return await  EnvioHouseImportacaoAerea();
                default:
                    break;
            }
            return true;
        }
        private async Task<bool> EnvioHouseImportacaoAerea()
        {
            BLImportacaoAerea bLImportacao = new BLImportacaoAerea();
            var entidadeImportacaoAerea = await bLImportacao.BuscarProcessoPorReferencia(_nReferencia);
            switch (_tipoEnvio)
            {
                case TipoEnvioCCT.EnviarHouseXFZB:
                    var xmlXFZB = await bLImportacao.ConverterEntidadeEmXFZB(entidadeImportacaoAerea);
                    EnvioHouse envioXFZB = new EnvioHouse(_tipoPefil);
                    await envioXFZB.EnviarXFZB(xmlXFZB, _nReferencia);
                    return true;
                    break;
                case TipoEnvioCCT.EnviarHouseXFHL:
                    var xmlXFHL = await bLImportacao.ConverterEntidadeEmXFHL(entidadeImportacaoAerea);
                    EnvioHouse envioXFHL = new EnvioHouse(_tipoPefil);
                    return await envioXFHL.EnviarXFHL(xmlXFHL, _nReferencia);

                default:
                    return false;
            }
        }
    }
}
 