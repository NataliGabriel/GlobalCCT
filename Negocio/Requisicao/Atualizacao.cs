using GLB.CCT.Entidade;
using GLB.CCT.EntidadeXML.ConsultaCCT;
using GLB.CCT.EntidadeXML.EnvioCCT;
using GLB.CCT.Persistencia;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.IO.Compression;
using System.Linq;
using System.Net;
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
    public class Atualizacao
    {
        public bool ChecaAtu()
        {
            string connectionString = "Data Source=191.252.59.68;Initial Catalog=   ;User ID=global;Password= ;";
            string sqlQuery = " ";
            return true;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            connection.Close();
                            return Convert.ToBoolean(reader["Atu"]);

                        }
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ocorreu um erro: {ex.Message}");
                    return false;
                }
            }

        }
        public async Task<bool> BaixaArquivoAtualizado()
        {
            string downloadUrl = "http://191.252.59.68:8088/CCT/CCT.zip";
            string zipFilePath = "CCT.zip";
            string extractionPath = @"\\192.168.1.5\OfficeComex\TesteExecutavel";

            try
            {
                MessageBox.Show("Existe uma versão mais atualizada do CCT, Deseja instalar?", "ATENÇÃO!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                using (WebClient client = new WebClient())
                {
                    client.DownloadFile(downloadUrl, zipFilePath);
                }

                if (Directory.Exists(extractionPath))
                {
                    Directory.Delete(extractionPath, true);
                }
                ZipFile.ExtractToDirectory(zipFilePath, extractionPath);

                File.Delete(zipFilePath);

                Console.WriteLine("Operação concluída com sucesso.");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocorreu um erro: {ex.Message}");
                return false;
            }

        }

    }
}
