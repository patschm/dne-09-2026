

using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace FeedReader;

internal class Program
{
    static async Task Main(string[] args)
    {
        var client = new HttpClient();
        client.BaseAddress = new Uri("https://nu.nl");
        var response = await client.GetAsync("rss");

        if (response.IsSuccessStatusCode)
        {
           var result = UseRegularExpressions(response.Content.ReadAsStream());
           //var result = UseXmlSerializer(response.Content.ReadAsStream());
           //var result = UseLinqToXml(response.Content.ReadAsStream());

            foreach(var item in result)
            {
                ShowItem(item);
            }

        }
        else
        {
            Console.WriteLine($"Error {response.StatusCode}");
        }

            Console.ReadLine();
    }

    private static void ShowItem(Item item)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(item.Title);
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine($"== {item.Category} ==");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(item.Description);
        Console.WriteLine(new string('=', 80));
    }

    private static IEnumerable<Item> UseLinqToXml(Stream stream)
    {
        XDocument doc = XDocument.Load(stream);

        var query = from item in doc.Descendants("item")
                    select new Item
                    {
                        Title = item.Element("title")?.Value,
                        Description = item.Element("description")?.Value,
                        Category = item.Element("category")?.Value,
                    };
        return query;

    }

    private static IEnumerable<Item> UseXmlSerializer(Stream stream)
    {
        var serializer = new XmlSerializer(typeof(Item));
        var reader = XmlReader.Create(stream);
        while (reader.ReadToFollowing("item"))
        {
            var item = serializer.Deserialize(reader.ReadSubtree()) as Item;
            if (item != null) yield return item;
        }
    }

    private static IEnumerable<Item> UseRegularExpressions(Stream stream)
    {
        StreamReader rdr = new StreamReader(stream);
        string data = rdr.ReadToEnd();
        Regex reg = new Regex(@"<item>.*?<title>(?<title>.*?)</title>.*?<description>(?<desc>.*?)</description>.*?<category>(?<cat>.*?)</category>.*?</item>", RegexOptions.None);
        var mc = reg.Matches(data);
        foreach (Match it in mc)
        {
            yield return new Item
            {
                Title = it.Groups["title"].Value,
                Description = it.Groups["desc"].Value,
                Category = it.Groups["cat"].Value
            };
        }
    }
}
