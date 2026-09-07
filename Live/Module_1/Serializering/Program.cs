using System.Xml;
using System.Xml.Serialization;

namespace Serializering;

internal class Program
{
    static void Main(string[] args)
    {

        List<Persoon>? people = GeneratPeople(100);
       
        //SerializeDate(people);
        
       // Console.ReadLine();

        //DeserializePeople();

        DeserilizeEfficient();
     
        
    }

    private static void DeserilizeEfficient()
    {
        XmlSerializer serializer = new XmlSerializer(typeof(Persoon));
        var fs = File.OpenRead(@"D:\testnet\people.xml");
        XmlReader reader = XmlReader.Create(fs);
       // bool sucess = false;
        while (reader.ReadToFollowing("person"))
            {
            //reader.ReadToFollowing("first-name");
            //reader.MoveToContent();
            //var s = reader.ReadInnerXml();
            var p = serializer.Deserialize(reader);
            Console.WriteLine(p);
        }
    }

    private static void DeserializePeople()
    {
        XmlSerializer serializer = new XmlSerializer(typeof(List<Persoon>));
        var fs = File.OpenRead(@"D:\testnet\people.xml");
        var people =  serializer.Deserialize(fs) as List<Persoon>;
        foreach (var p1 in people)
        {
            Console.WriteLine(p1);
        }

    }

    private static void SerializeDate(List<Persoon> people)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(List<Persoon>));
        Stream fs = File.OpenWrite(@"D:\testnet\people.xml");
        serializer.Serialize(fs, people);
        fs.Close();

    }

    private static List<Persoon> GeneratPeople(int v)
    {
        return new Bogus.Faker<Persoon>()
           .RuleFor(p => p.Voornaam, f => f.Name.FirstName())
            .RuleFor(p => p.Achernaam, f => f.Name.LastName())
            .RuleFor(p => p.Leeftijd, f => f.Random.Int(10, 125))
            .Generate(100)
            .ToList();

    }
}
