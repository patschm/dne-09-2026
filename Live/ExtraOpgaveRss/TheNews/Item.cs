using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace TheNews;

[XmlRoot("item")]
public class Item
{
    [XmlElement("title")]
    public string? Title { get; set; }
    [XmlElement("description")]
    public string? Description { get; set; }
    [XmlElement("category")]
    public string? Category { get; set; }
}
