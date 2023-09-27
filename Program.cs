// See https://aka.ms/new-console-template for more information
using GLB.CCT.Negocio;
using GLB.CCT.Negocio.Requisicao;
using static System.Net.Mime.MediaTypeNames;
using System;
using static GLB.CCT.Negocio.Requisicao.URLIntegracao;
using GLB.CCT.Persistencia;

class Program
{
    static Atualizacao _atualizacao = new Atualizacao();
    [STAThread]
    static public void Main(string[] args)
    {
        try
        {

            string[] arquivo = args;
            //Console.WriteLine(arquivo[0]);
            //if (args.Length > 0)
            //{
            //if (_atualizacao.ChecaAtu()) { _atualizacao.BaixaArquivoAtualizado(); }
            
            Console.WriteLine(DateTime.Now + "> Iniciando Processo...");
            Console.WriteLine("Referência: " + arquivo[0]);
            switch (arquivo[1])
            {
                case "0":
                    BLRealizarIntegracaoEnvio bLRealizarIntegracaoEnvioXFZB = new BLRealizarIntegracaoEnvio("IAPI0337-0923", EnumSistema.TipoProcessoEnum.ImportacaoAerea, EnumSistema.TipoEnvioCCT.EnviarHouseXFZB);
                    bLRealizarIntegracaoEnvioXFZB.Integrar();
                    break;
                case "1":
                    BLRealizarIntegracaoEnvio bLRealizarIntegracaoEnvioMASTER = new BLRealizarIntegracaoEnvio("IAPI0337-0923", EnumSistema.TipoProcessoEnum.ImportacaoAerea, EnumSistema.TipoEnvioCCT.EnviarMaster);
                    bLRealizarIntegracaoEnvioMASTER.Integrar();
                    break;
                case "2":
                    ConsultaPorConhecimento ConsultaConhecimento = new ConsultaPorConhecimento("HAWB12038"/*args[0]*/, TipoPerfilEnum.AGECARGA);
                    ConsultaConhecimento.ConsultarConhecimentos();
                    break;
                //case "3":
                //    BLRealizarIntegracaoEnvio bLRealizarIntegracaoEnvioMASTER = new BLRealizarIntegracaoEnvio("IARDL0017-0823", EnumSistema.TipoProcessoEnum.ImportacaoAerea, EnumSistema.TipoEnvioCCT.EnviarMaster);
                //    bLRealizarIntegracaoEnvioMASTER.Integrar();
                //    break;
            }
            Console.ReadKey(); 
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro na Referencia {args[0]} \r\n" + ex.Message);
            Console.ReadKey();
        }
    }

}