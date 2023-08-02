using GLB.CCT.Entidade;
using GLB.CCT.EntidadeXML.ConsultaCCT;
using GLB.CCT.Persistencia;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GLB.CCT.Negocio
{
    public class BLDesembaracoImportacao
    {
        private BLCadastros _blCadastros;
        private DesembaracoImportacaoRepositorio _repositorio;
        public BLDesembaracoImportacao()
        {
            _blCadastros = new BLCadastros();
            _repositorio = new DesembaracoImportacaoRepositorio();
        }
        public async Task<DesembaracoImportacaoEntidade> ConverterXMLChegadaEmEntidade(XMLConsultaChegadasCCT xmlChegadas, string nReferencia)
        {
            DesembaracoImportacaoEntidade desembaracoImportacaoEntidade = new DesembaracoImportacaoEntidade
            {
                N_REFERENCIA = nReferencia,
                CODIGO_LOCAL_EMBARQUE = await _blCadastros.AcharOrigemDestinoPorSigla(xmlChegadas.aeroportoPartida),
                DATA_CHEGADA = xmlChegadas.chegadas.dataHoraChegadaEfetiva.FormatarData(),
                DATA_PREV_CHEGADA = xmlChegadas.chegadas.dataHoraChegadaPrevista.FormatarData(),
                CODIGO_RECINTO = xmlChegadas.chegadas.recintoAduaneiroChegada,
                NR_TERMO = xmlChegadas.chegadas.termoEntrada,
                PESO_BRUTO = xmlChegadas.chegadas.uldBlks.conhecimentos.pesoBrutoManifestoVoo.FormatarDouble(),
                NR_VEICULO_TRANSP = xmlChegadas.codigoVoo,
                DATA_EMB = xmlChegadas.dataHoraPartidaEfetiva.FormatarData(),
                DATA_PREV_EMBARQUE = xmlChegadas.dataHoraPartidaPrevista.FormatarData()
            };
            if (xmlChegadas.chegadas.uldBlks.conhecimentos.quantidadeVolumesManifestoVoo != null)
            {
                desembaracoImportacaoEntidade.ListaDeVolumes.Add(new VolumesDIEntidade
                {
                    NOSSA_REFERENCIA = nReferencia,
                    QTD_EMBALAGEM = xmlChegadas.chegadas.uldBlks.conhecimentos.quantidadeVolumesManifestoVoo.FormatarInt()
                });
            }
            return desembaracoImportacaoEntidade;
        }
        public async Task AtualizarDesembaracoImportacao (DesembaracoImportacaoEntidade entidade)
        {
            try
            {
                await _repositorio.Atualizar(entidade);
                if (entidade.ListaDeVolumes.Count>0)
                {
                    if (entidade.ListaDeVolumes[0].QTD_EMBALAGEM >0)
                    {
                        await _repositorio.AtualizarVolume(entidade.ListaDeVolumes.FirstOrDefault());
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
