using System.Security.Cryptography;
using System.Text;

namespace Integrity;

class Program
{
    static void Main(string[] args)
    {
        //DemoHash();
        DemoAsymmetrisch();
    }

    private static void DemoAsymmetrisch()
    {
        MessageAsym msg = SenderAsym();
        msg = new MessageAsym(msg.Bericht + ".", msg.Signature, msg.PublicKey);
        ReceiverAsym(msg);
    }

    private static void ReceiverAsym(MessageAsym msg)
    {
        DSA dsa = new DSACryptoServiceProvider();
        dsa.FromXmlString(msg.PublicKey);
        SHA1 sha256 = SHA1.Create();
        byte[] hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(msg.Bericht));
        bool isOk = dsa.VerifyData(hash, msg.Signature, HashAlgorithmName.SHA1);
        if (isOk )
            Console.WriteLine("Bericht geldig");
        else
            Console.WriteLine("Bericht ongeldig");
    }

    private static MessageAsym SenderAsym()
    {
        DSA dsa = new DSACryptoServiceProvider();
        var pubKey = dsa.ToXmlString(false);
        string bericht = "Hello World";
        SHA1 sha256 = SHA1.Create();
        byte[] hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(bericht));
        var sign = dsa.SignData(hash, HashAlgorithmName.SHA1);
        return new MessageAsym(bericht, sign, pubKey);
    }

    private static void DemoHash()
    {
        Message msg = Sender();
        Console.WriteLine($"Bericht: {msg.Bericht}");
        Console.WriteLine($"Hash: {msg.Hash}");

        //msg = new Message(msg.Bericht + "!", msg.Hash); // Simulate a modified message
        Receiver(msg);
    }

    private static void Receiver(Message msg)
    {
        //SHA256 sha256 = SHA256.Create();
        HMACSHA256 sha256 = new HMACSHA256();
        sha256.Key = Encoding.UTF8.GetBytes("Secret");

        byte[] hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(msg.Bericht));
        if (Convert.ToBase64String(hash) == msg.Hash)
        {
            Console.WriteLine("Integriteit gecontroleerd: Bericht is ongewijzigd.");
        }
        else
        {
            Console.WriteLine("Integriteit gecontroleerd: Bericht is gewijzigd.");
        }
    }

    private static Message Sender()
    {
        string bericht = "Hello World";
        //SHA256 sha256 = SHA256.Create();
        HMACSHA256 sha256 = new HMACSHA256();
        sha256.Key = Encoding.UTF8.GetBytes("Secret");
        byte[] hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(bericht));

        return new Message(bericht, Convert.ToBase64String(hash));
    }
}

record class Message(string Bericht, string Hash);
record class MessageAsym(string Bericht, byte[] Signature, string PublicKey);
