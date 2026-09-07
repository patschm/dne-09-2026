using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Serializering;

[XmlType("person")]
public class Persoon
{
    [XmlElement("first-name")]
    public string? Voornaam { get; set; }
    [XmlElement("last-name")]
    public string? Achernaam { get; set; }
    [XmlAttribute("age")]
    public int Leeftijd { get; set; }

    public override string ToString()
    {
        return $"{Voornaam} {Achernaam} ({Leeftijd})";
    }
}
