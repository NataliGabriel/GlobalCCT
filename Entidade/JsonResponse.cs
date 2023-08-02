using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GLB.CCT.Entidade
{
    public class JsonResponse
    {
        public List<Root> Data { get; set; }
    }
    public class Root
    {
        public List<bloqueiosAtivos?> bloqueiosAtivos { get; set; }
        public List<bloqueiosBaixados?> bloqueiosBaixados { get; set; }
        public string? caixaPostalAgenteDeCargaConsolidadorEstrang { get; set; }
        public string? caixaPostalConsignatarioConhecimento { get; set; }
        public string? caixaPostalEmbarcadorEstrangeiro { get; set; }
        public string? categoriaCarga { get; set; }
        public List<chegadasTerrestres?> chegadasTerrestres { get; set; }
        public string? cidadeAgenteDeCargaConsolidadorEstrang { get; set; }
        public string? cidadeConsignatarioConhecimento { get; set; }
        public string? cidadeEmbarcadorEstrangeiro { get; set; }
        public string? cnpjResponsavelArquivo { get; set; }
        public string? codigoAeroportoDestinoConhecimento { get; set; }
        public string? codigoAeroportoOrigemConhecimento { get; set; }
        public List<contatosAgenteDeCargaConsolidadorEstrang?> contatosAgenteDeCargaConsolidadorEstrang { get; set; }
        public List<contatosConsignatarioConhecimento?> contatosConsignatarioConhecimento { get; set; }
        public List<contatosEmbarcadorEstrangeiro?> contatosEmbarcadorEstrangeiro { get; set; }
        public string? dataEmissao { get; set; }
        public string? dataHoraAssinaturaTransportador { get; set; }
        public string? descricaoResumida { get; set; }
        public List<divergencias?> divergencias { get; set; }
        public List<documentosSaida?> documentosSaida { get; set; }
        public List<dsicsApropriados?> dsicsApropriados { get; set; }
        public string? enderecoAgenteDeCargaConsolidadorEstrang { get; set; }
        public string? enderecoConsignatarioConhecimento { get; set; }
        public string? enderecoEmbarcadorEstrangeiro { get; set; }
        public frete? frete { get; set; }
        public List<hawbAssociados?> hawbAssociados { get; set; }
        public string? identificacao { get; set; }
        public string? identificacaoDocumentoConsignatario { get; set; }
        public string? indicadorNaoRecepcaoHawbAssociados { get; set; }
        public string? indicadorPartesMadeira { get; set; }
        public List<itensCarga?> itensCarga { get; set; }
        public string? localAssinaturaTransportador { get; set; }
        public List<manuseiosEspeciais?> manuseiosEspeciais { get; set; }
        public List<mawbAwbAssociados?> mawbAwbAssociados { get; set; }
        public string? nomeAgenteDeCargaConsolidadorEstrang { get; set; }
        public string? nomeAssinaturaEmbarcadorEstrangeiro { get; set; }
        public string? nomeAssinaturaTransportador { get; set; }
        public string? nomeConsignatarioConhecimento { get; set; }
        public string? nomeDocumentoConsignatario { get; set; }
        public string? nomeEmbarcadorEstrangeiro { get; set; }
        public List<outrasInfosServico?> outrasInfosServico { get; set; }
        public List<outrasPartesInteressadas?> outrasPartesInteressadas { get; set; }
        public string? paisAgenteDeCargaConsolidadorEstrang { get; set; }
        public string? paisConsignatarioConhecimento { get; set; }
        public string? paisEmbarcadorEstrangeiro { get; set; }
        public List<partesEstoque?> partesEstoque { get; set; }
        public double? pesoBrutoConhecimento { get; set; }
        public double? quantidadeVolumesConhecimento { get; set; }
        public string? razaoSocialDocumentoConsignatario { get; set; }
        public double? recintoAduaneiroDestino { get; set; }
        public string? ruc { get; set; }
        public string? situacao { get; set; }
        public List<solicitacoesServicosEspeciais?> solicitacoesServicosEspeciais { get; set; }
        public string? tipo { get; set; }
        public string? tipoDocumentoConsignatario { get; set; }
        public List<viagensAssociadas?> viagensAssociadas { get; set; }
    }

    public class bloqueiosAtivos
    {
        public string? alcanceBloqueio { get; set; }
        public string? dataHoraBloqueio { get; set; }
        public string? justificativaBloqueio { get; set; }
        public string? motivoBloqueio { get; set; }
        public string? responsavelBloqueio { get; set; }
        public string? tipoBloqueio { get; set; }
    }
    public class bloqueiosBaixados
    {
        public string? alcanceBloqueio { get; set; }
        public string? dataHoraBloqueio { get; set; }
        public string? dataHoraDesbloqueio { get; set; }
        public string? justificativaBloqueio { get; set; }
        public string? justificativaDesbloqueio { get; set; }
        public string? motivoBloqueio { get; set; }
        public string? responsavelBloqueio { get; set; }
        public string? responsavelDesbloqueio { get; set; }
        public string? tipoBloqueio { get; set; }
    }
    public class chegadasTerrestres
    {
        public int? codigoRecintoChegada { get; set; }
        public int? codigoRecintoPartida { get; set; }
        public string? codigoUaChegada { get; set; }
        public string? codigoUaPartida { get; set; }
        public string? dataHoraChegada { get; set; }
        public string? dataHoraPartida { get; set; }
        public double? numeroDta { get; set; }
        public string? placa { get; set; }
        public double? termo { get; set; }
        public string? uf { get; set; }
        public string? veiculoRegistradoNoMantra { get; set; }
    }
    public class contatosAgenteDeCargaConsolidadorEstrang
    {
        public string? email { get; set; }
        public string? nome { get; set; }
        public string? telefone { get; set; }
    }
    public class contatosConsignatarioConhecimento
    {
        public string? email { get; set; }
        public string? nome { get; set; }
        public string? telefone { get; set; }
    }
    public class contatosEmbarcadorEstrangeiro
    {
        public string? email { get; set; }
        public string? nome { get; set; }
        public string? telefone { get; set; }
    }
    public class divergencias
    {
        public string? dataDivergencia { get; set; }
        public string? momentoDivergencia { get; set; }
        public double? percentualDiferencaPeso { get; set; }
        public double? percentualDiferencaVolumes { get; set; }
        public double? pesoConhecimento { get; set; }
        public double? pesoDivergente { get; set; }
        public double? pesoManifestoVoo { get; set; }
        public double? quantidadeVolumesConhecimento { get; set; }
        public double? quantidadeVolumesDivergentes { get; set; }
        public double? quantidadeVolumesManifestoVoo { get; set; }
        public string? tipoDivergencia { get; set; }
        public string? totalParcial { get; set; }
    }
    public class documentosSaida
    {
        public string? dataHoraViculacao { get; set; }
        public double? numero { get; set; }
        public double? raDestinoDta { get; set; }
        public double? tipo { get; set; }
        public string? uaDestinoDta { get; set; }
    }
    public class dsicsApropriados
    {
        public string? identificacaoDSIC { get; set; }
    }
    #region FRETE 
    public class frete
    {
        public formaPgto? formaPgto { get; set; }
        public moedaOrigem? moedaOrigem { get; set; }
        public List<outrosEncargos?> outrosEncargos { get; set; }
        public string? pendenciaPagamento { get; set; }
        public somatorioFretePorItemCarga? somatorioFretePorItemCarga { get; set; }
        public List<totaisMoedaDestino?> totaisMoedaDestino { get; set; }
        public List<totaisMoedaOrigem?> totaisMoedaOrigem { get; set; }
    }
    public class formaPgto
    {
        public string? codigo { get; set; }
        public string descricao { get; set; }

        public valorOutrosEncargos? valorOutrosEncargos { get; set; }
        public valorPorPesoValor? valorPorPesoValor { get; set; }
    }
    public class valorOutrosEncargos
    {
        public string? codigo { get; set; }
        public string? descricao { get; set; }
    }
    public class valorPorPesoValor
    {
        public string? codigo { get; set; }
        public string? descricao { get; set; }
    }
    public class moedaOrigem
    {
        public string? codigo { get; set; }
        public string? descricao { get; set; }
    }
    public class outrosEncargos
    {
        public formaPgto? formaPgto { get; set; }
        public string? motivo { get; set; }
        public List<recebedor?> recebedor { get; set; }
        public List<tipo?> tipo { get; set; }
        public List<valorTotal?> valorTotal { get; set; }
    }
    public class recebedor
    {
        public string? codigo { get; set; }
        public string? descricao { get; set; }
    }
    public class tipo
    {
        public string? codigo { get; set; }
        public string? descricao { get; set; }
    }
    public class valorTotal
    {
        public List<moeda?> moeda { get; set; }
        public double? valor { get; set; }
    }
    public class moeda
    {
        public string? codigo { get; set; }
        public string? descricao { get; set; }
    }
    public class somatorioFretePorItemCarga
    {
        public moeda? moeda { get; set; }
        public double? valor { get; set; }
    }
    public class totaisMoedaDestino
    {
        public List<tipo?> tipo { get; set; }
        public List<valorCollect?> valorCollect { get; set; }
    }
    public class valorCollect
    {
        public moeda? moeda { get; set; }
        public double? valor { get; set; }
    }
    public class totaisMoedaOrigem
    {
        public tipo? tipo { get; set; }
        public valorCollect? valorCollect { get; set; }
        public valorPrepaid? valorPrepaid { get; set; }
    }
    public class valorPrepaid
    {
        public moeda? moeda { get; set; }
        public string? valor { get; set; }
    }
    #endregion
    public class hawbAssociados
    {
        public string? cnpjResponsavelArquivo { get; set; }
        public string? dataEmissao { get; set; }
        public string? identificacao { get; set; }
    }
    public class itensCarga
    {
        public List<double?> classificacoesMercadoria { get; set; }
        public string? descricaoMercadoria { get; set; }
        public List<ulds?> ulds { get; set; }
    }
    public class ulds
    {
        public string? codigoProprietarioULD { get; set; }
        public string? numeroSerieULD { get; set; }
        public double? quantidade { get; set; }
        public double? tara { get; set; }
        public string? tipoULD { get; set; }
    }
    public class manuseiosEspeciais
    {
        public string? codigo { get; set; }
        public string? detalhes { get; set; }
    }
    public class mawbAwbAssociados
    {
        public List<chegadasTerrestres?> chegadasTerrestres { get; set; }
        public string? cnpjResponsavelArquivo { get; set; }
        public string? dataEmissao { get; set; }
        public string? identificacao { get; set; }
    }
    public class outrasInfosServico
    {
        public string? codigo { get; set; }
        public string? detalhes { get; set; }
    }
    public class outrasPartesInteressadas
    {
        public string? caixaPostal { get; set; }
        public string? cidade { get; set; }
        public List<contatos?> contatos { get; set; }
        public string? endereco { get; set; }
        public string? nome { get; set; }
        public string? pais { get; set; }
        public int? tipo { get; set; }
    }
    public class contatos
    {
        public string? email { get; set; }
        public string? nome { get; set; }
        public string? telefone { get; set; }

    }
    public class partesEstoque
    {
        public string? cnpjResponsavelAtual { get; set; }
        public string? dataHoraSituacaoAtual { get; set; }
        public string? identificacaoViagem { get; set; }
        public double? numeroDocumentoSaida { get; set; }
        public double? pesoBrutoEstoque { get; set; }
        public double? quantidadeVolumesEstoque { get; set; }
        public double? recintoAduaneiro { get; set; }
        public string? situacaoAtual { get; set; }
        public double? tipoDocumentoSaida { get; set; }
        public string? unidadeRfb { get; set; }
    }
    public class solicitacoesServicosEspeciais
    {
        public string? codigo { get; set; }
        public string? detalhes { get; set; }
    }
    public class viagensAssociadas
    {
        public string? identificacaoViagem { get; set; }
    }
}
