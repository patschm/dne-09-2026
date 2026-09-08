using System.Xml;
using System.Xml.Serialization;

namespace TheNews;

internal class Program
{
    static XmlSerializer serializer = new XmlSerializer(typeof(Item));

    static void Main(string[] args)
    {
        
        var client = new HttpClient { BaseAddress = new Uri("https://nu.nl/rss") };
        var stream = client.GetStreamAsync("").Result;
        IEnumerable<Item> items = ProcessFeed(stream);
        
        foreach(Item it in items )
        {
            Console.WriteLine(it.Category);
            Console.WriteLine(it.Title);
            Console.WriteLine(it.Description);
            Console.WriteLine(new string('=', 80));
        }
        
        foreach(int nr in GetNumbers())
        {
            Console.WriteLine(nr);
        }
    }

    private static IEnumerable<Item> ProcessFeed(Stream stream)
    {
       // List<Item> list = new List<Item>();
        var reader = XmlReader.Create(stream);
        while (reader.ReadToFollowing("item"))
        {
            var sub = reader.ReadSubtree();
            var item = serializer.Deserialize(sub) as Item;
            //list.Add(item);
            yield return item;
           
        }
        //return list;
    }

    static IEnumerable<int> GetNumbers()
    {
        yield return 1;
        Console.WriteLine("En verder...");
        yield return 2;
        Console.WriteLine("En verder...");
        yield return 3;
        Console.WriteLine("En verder...");
        yield return 4;
        Console.WriteLine("En verder...");
    }
}
