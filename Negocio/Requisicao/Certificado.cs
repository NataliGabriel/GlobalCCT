using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;


namespace GLB.CCT.Negocio.Requisicao
{
    public class Certificado
    {
        public X509Certificate2 BuscaNome(string Nome)
        {
            X509Certificate2 x509Certificate2 = null;
            try
            {
                X509Store x509Store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
                x509Store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
                X509Certificate2Collection collection = (X509Certificate2Collection)x509Store.Certificates;
                X509Certificate2Collection collection1 = (X509Certificate2Collection)collection.Find(X509FindType.FindByTimeValid, DateTime.Now, true);
                //X509Certificate2Collection collection2 = (X509Certificate2Collection)collection.Find(X509FindType.FindByKeyUsage, X509KeyUsageFlags.DigitalSignature, true);
                //X509Certificate2Collection scollection = (X509Certificate2Collection)collection2.Find(X509FindType.FindBySubjectName, "NOME DA EMPRESA \\ PESSOA", true);
                x509Store.Close();
                Console.WriteLine(DateTime.Now + "> Selecionar certificado...");
                if (Nome == "")
                {
                    try
                    {

                        X509Certificate2Collection certificate2Collection = X509Certificate2UI.SelectFromCollection(collection1, "Certificado(s) Digital(is) disponível(is)", "Selecione o Certificado Digital para uso no aplicativo", X509SelectionFlag.MultiSelection);

                        if (certificate2Collection.Count == 0)
                        {
                            x509Certificate2.Reset();
                            Console.WriteLine(DateTime.Now + "> Nenhum certificado escolhido", (object)"Atenção");
                        }
                        else
                        {

                            Console.WriteLine(DateTime.Now + "> Certificado selecionado...");
                            x509Certificate2 = certificate2Collection[0];
                            //string strEnviar = "enviardue";
                            //string encryptedstring = Encrypt(x509Certificate2, strEnviar);
                            //string decryptedstring = Decrypt(x509Certificate2, encryptedstring);

                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(DateTime.Now + "> Erro certificado");
                        Console.WriteLine(e.Message);
                    }
                }
                else
                {
                    X509Certificate2Collection certificate2Collection = collection1.Find(X509FindType.FindBySubjectDistinguishedName, (object)Nome, false);
                    if (certificate2Collection.Count == 0)
                    {
                        Console.WriteLine(DateTime.Now + "> Nenhum certificado válido foi encontrado com o nome informado: " + Nome, (object)"Atenção");
                        x509Certificate2.Reset();
                    }
                    else
                    {
                        //if (senhaCertificado != "")
                        //{
                        //    x509Certificate2 = new X509Certificate2(certificate2Collection[0].GetRawCertData(), senhaCertificado);
                        //}
                        //else
                        //{
                        x509Certificate2 = certificate2Collection[0];
                        //}
                    }
                }
                return x509Certificate2;
            }
            catch (Exception ex)
            {
                Console.WriteLine(DateTime.Now + "> Erro certificado 2");
                Console.WriteLine(ex.Message);
                return x509Certificate2;
            }
        }


        public static string Decrypt(X509Certificate2 x509, string stringTodecrypt)
        {
            if (x509 == null || string.IsNullOrEmpty(stringTodecrypt))
                throw new Exception("A x509 certificate and string for decryption must be provided");

            if (!x509.HasPrivateKey)
                throw new Exception("x509 certicate does not contain a private key for decryption");

            RSACryptoServiceProvider rsa = (RSACryptoServiceProvider)x509.PrivateKey;
            byte[] bytestodecrypt = Convert.FromBase64String(stringTodecrypt);
            byte[] plainbytes = rsa.Decrypt(bytestodecrypt, false);
            System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
            return enc.GetString(plainbytes);
        }
        public static string Encrypt(X509Certificate2 x509, string stringToEncrypt)
        {
            if (x509 == null || string.IsNullOrEmpty(stringToEncrypt))
                throw new Exception("A x509 certificate and string for encryption must be provided");

            RSACryptoServiceProvider rsa = (RSACryptoServiceProvider)x509.PublicKey.Key;
            byte[] bytestoEncrypt = ASCIIEncoding.ASCII.GetBytes(stringToEncrypt);
            byte[] encryptedBytes = rsa.Encrypt(bytestoEncrypt, false);
            return Convert.ToBase64String(encryptedBytes);
        }

    }
}

