using System.IO;
using System.IO.Compression;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace Streaming;

internal class Program
{
    static void Main(string[] args)
    {
        //WriteToStreamNeanderthalerWay();
        //ReadFromStreamNeanderthalerWay();
        //WriteToStreamHomeSapiensWay();
        //ReadFromStreamHomeSapiensWay();

        //WriteToStreamZipped();
        ReadFromStreamZipped();
    }

    private static void ReadFromStreamZipped()
    {
        Stream fs = File.OpenRead(@"D:\Testnet\simple2.zip");
        GZipStream gz = new GZipStream(fs, CompressionMode.Decompress);
        StreamReader rdr = new StreamReader(gz);
        string? line = null;
        while ((line = rdr.ReadLine()) != null)
        {
            Console.WriteLine(line);
        }
    }

    private static void WriteToStreamZipped()
    {
        FileInfo file = new FileInfo(@"D:\Testnet\simple2.zip");
        if (file.Exists)
        {
            file.Delete();
        }
        string text = "Hello World ";
        Stream fs = file.Create();
        GZipStream gz = new GZipStream(fs, CompressionMode.Compress);
        StreamWriter writer = new StreamWriter(gz);
        for (int i = 0; i < 1000; i++)
        {
            writer.WriteLine(text + i);
        }
        writer.Flush();
        writer.Close();
    }

    private static void ReadFromStreamHomeSapiensWay()
    {
        Stream fs = File.OpenRead(@"D:\Testnet\simple2.txt");
        StreamReader rdr = new StreamReader(fs);
        string? line = null;
        while((line = rdr.ReadLine())    != null)
        {
            Console.WriteLine(line);
        }
    }

    private static void WriteToStreamHomeSapiensWay()
    {
        FileInfo file = new FileInfo(@"D:\Testnet\simple2.txt");
        if (file.Exists)
        {
            file.Delete();
        }
        string text = "Hello World ";
        Stream fs = file.Create();
        StreamWriter writer = new StreamWriter(fs);
        for (int i = 0; i < 1000; i++)
        {
            writer.WriteLine(text + i);
        }
        writer.Flush();
        writer.Close();
    }

    private static void ReadFromStreamNeanderthalerWay()
    {
        FileInfo file = new FileInfo(@"D:\Testnet\simple.txt");
       Stream fs = file.Open(FileMode.Open, FileAccess.Read);

        byte[] buffer = new byte[8];
        int nrRead = 0;
        while ((nrRead = fs.Read(buffer)) > 0)
        { 
            string s = Encoding.UTF8.GetString(buffer);
            Console.Write($"{s}");
            Array.Clear(buffer, 0, buffer.Length);
        }
    }

    private static void WriteToStreamNeanderthalerWay()
    {
        Directory.CreateDirectory("D:\\Testnet");
        FileInfo file = new FileInfo(@"D:\Testnet\simple.txt");
        if (file.Exists)
        {
            file.Delete();
        }
        Stream fs = file.Create();

        string text = "Hello World ";
        byte[] buffer;
        for (int i = 0; i < 1000; i++)
        {
            buffer = Encoding.UTF8.GetBytes(text + i + "\r\n");
            fs.Write(buffer);
        }
        fs.Flush();
        fs.Close();
    }
}
