
using System.Security.Cryptography;
using System.Text;

namespace Confidentiality;

internal class Program
{
    private static byte[] key;
    private static byte[] iv;
    private static RSACryptoServiceProvider rsa;

    static void Main(string[] args)
    {
        //SymmetrischeDemo();
        AsymmetrischeDemo();
    }

    private static void AsymmetrischeDemo()
    {
        string pubkey = OntvangerStuurtPubKey();
        Cipher data = SenderGebruikPubKey(pubkey);
        OntvangerLeestBericht(data);
    }

    private static void OntvangerLeestBericht(Cipher data)
    {
        byte[] dt = rsa.Decrypt(data.data, true);
        Console.WriteLine(Encoding.UTF8.GetString(dt));
    }

    private static Cipher SenderGebruikPubKey(string pubkey)
    {
        var bericht = "Hello World";
        var rsa = new RSACryptoServiceProvider();
        rsa.FromXmlString(pubkey);
        byte[] data =  rsa.Encrypt(Encoding.UTF8.GetBytes(bericht), true);
        return new Cipher(data);
    }

    private static string OntvangerStuurtPubKey()
    {
        rsa = new RSACryptoServiceProvider();
        return rsa.ToXmlString(false);
    }

    private static void SymmetrischeDemo()
    {
        Cipher msg = Sender();
       // Receiver(msg);

        msg = SenderBig();
        ReceiverBig(msg);
    }

    private static void Receiver(Cipher msg)
    {
        Aes alg = Aes.Create();
        alg.Key = key;

        var data = alg.DecryptEcb(msg.data, PaddingMode.PKCS7);
        Console.WriteLine(Encoding.UTF8.GetString(data));
    }

    private static Cipher Sender()
    {
        var bericht = "Hello world";
        Aes alg = Aes.Create();
        key = alg.Key;

        byte[] crypt = alg.EncryptEcb(Encoding.UTF8.GetBytes(bericht), PaddingMode.PKCS7);
        Console.WriteLine(Encoding.UTF8.GetString(crypt));
        return new Cipher(crypt);
    }
    private static Cipher SenderBig()
    {
        var bericht = "Hello world";
        Aes alg = Aes.Create();
        key = alg.Key;
        iv = alg.IV;
        alg.Mode = CipherMode.CBC;
        using MemoryStream mem = new MemoryStream();
        {
            using CryptoStream crypt = new CryptoStream(mem, alg.CreateEncryptor(), CryptoStreamMode.Write);
            using (StreamWriter writer = new StreamWriter(crypt))
            {
                writer.Write(bericht);
            }

            return new Cipher(mem.ToArray());
        }
    }

    private static void ReceiverBig(Cipher msg)
    {
        Aes alg = Aes.Create();
        alg.Key = key;
        alg.IV = iv;
        alg.Mode = CipherMode.CFB;
        using MemoryStream mem = new MemoryStream(msg.data);
        using CryptoStream crypt = new CryptoStream(mem, alg.CreateDecryptor(), CryptoStreamMode.Read);
        using StreamReader reader = new StreamReader(crypt);
            Console.WriteLine(reader.ReadToEnd());
    }

}

record struct Cipher(byte[] data);







