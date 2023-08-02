// See https://aka.ms/new-console-template for more information
using GLB.CCT.Negocio;
using GLB.CCT.Negocio.Requisicao;
using static System.Net.Mime.MediaTypeNames;
using System;
using static GLB.CCT.Negocio.Requisicao.URLIntegracao;

class Program
{
    [STAThread]
    static public void Main(string[] args)
    {
        try
        {
            string[] arquivo = args;
            //Console.WriteLine(arquivo[0]);
            //if (args.Length > 0)
            //{
            Console.WriteLine(DateTime.Now + "> Iniciando Processo...");
            Console.WriteLine("Referência: " + arquivo[0]);
            switch (arquivo[1])
            {
                case "0":
                    BLRealizarIntegracaoEnvio bLRealizarIntegracaoEnvioXFZB = new BLRealizarIntegracaoEnvio(arquivo[0], EnumSistema.TipoProcessoEnum.ImportacaoAerea, EnumSistema.TipoEnvioCCT.EnviarHouseXFZB);
                    bLRealizarIntegracaoEnvioXFZB.Integrar();
                    break;
                case "1":
                    BLRealizarIntegracaoEnvio bLRealizarIntegracaoEnvioXFHL = new BLRealizarIntegracaoEnvio(arquivo[0], EnumSistema.TipoProcessoEnum.ImportacaoAerea, EnumSistema.TipoEnvioCCT.EnviarHouseXFHL);
                    bLRealizarIntegracaoEnvioXFHL.Integrar();
                    break;
                case "2":
                    ConsultaPorConhecimento ConsultaConhecimento = new ConsultaPorConhecimento("489807469"/*args[0]*/, TipoPerfilEnum.AGECARGA);
                    ConsultaConhecimento.ConsultarConhecimentos();
                    break;
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