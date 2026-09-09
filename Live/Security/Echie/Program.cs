using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Echie;

internal class Program
{
    static void Main(string[] args)
    {
        X509Store store = new X509Store(StoreName.My, StoreLocation.LocalMachine, OpenFlags.ReadOnly);
        foreach(X509Certificate2 cert in store.Certificates)
        {
            Console.WriteLine(cert.SubjectName.Name);
        }

        var alles = store.Certificates.Find(X509FindType.FindBySubjectName, "localhost", true);
        X509Certificate2 cert2 = alles.First();
        Console.WriteLine(cert2.SubjectName.Name);
        RSA rsa = cert2.GetRSAPublicKey();
        byte[] cipher = rsa.Encrypt(Encoding.UTF8.GetBytes("Hoi hoi"),RSAEncryptionPadding.OaepSHA1);
    
        RSA rsa2 = cert2.GetRSAPrivateKey();
        byte[] dataaa = rsa2.Decrypt(cipher, RSAEncryptionPadding.OaepSHA1);
        Console.WriteLine(Encoding.UTF8.GetString(dataaa));
    }
}
