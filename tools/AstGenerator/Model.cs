using System.Collections.Generic;
using System.Xml.Serialization;

namespace AstGenerator
{
    public class Field
    {
        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        public string Type { get; set; }

        [XmlAttribute]
        public bool IsList { get; set; }

        [XmlAttribute]
        public bool DoNotVisit { get; set; }
    }

    public class Node
    {
        [XmlAttribute]
        public bool IsAbstract { get; set; }

        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        public string Base { get; set; }

        [XmlAttribute]
        public bool DontCreateFactoryMethod { get; set; }

        [XmlElement(ElementName = "Field", Type = typeof(Field))]
        public List<Field> Fields { get; set; }
    }

    public class Using
    {
        [XmlAttribute]
        public string Name { get; set; }
    }

    public class Tree
    {
        [XmlAttribute]
        public string Namespace { get; set; }

        [XmlArray("Usings")]
        [XmlArrayItem("Using")]
        public List<Using> UsingDirectives { get; set; }

        [XmlArray("Nodes")]
        [XmlArrayItem("Node")]
        public List<Node> Nodes { get; set; }
    }
}
