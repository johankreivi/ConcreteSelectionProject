using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Entity;
using Newtonsoft.Json;


namespace Infrastructure.Helpers
{
    public class DgmlParser
    {
        public string ParseDgml(string filePath, string rootNodeName)
        {
            XDocument doc = XDocument.Load(filePath);
            XNamespace ns = "http://schemas.microsoft.com/vs/2009/dgml";

            // Parsing nodes using LINQ to XML
            var nodes = doc.Descendants(ns + "Node")
                .Select(x => new Node
                {
                    id = x.Attribute("Id").Value,
                    label = x.Attribute("Label")?.Value,
                    children = new List<Node>() // initialize children list
                })
                .ToDictionary(n => n.id);

            // Establishing parent-child relationships using links
            var links = doc.Descendants(ns + "Link");
            foreach (var link in links)
            {
                string sourceId = link.Attribute("Source").Value;
                string targetId = link.Attribute("Target").Value;

                if (nodes.ContainsKey(sourceId) && nodes.ContainsKey(targetId))
                {
                    nodes[sourceId].children.Add(nodes[targetId]);
                }
            }

            // Serialize the Node structure starting from "Konstruktionstyp1" to JSON
            string json = JsonConvert.SerializeObject(nodes[rootNodeName], Newtonsoft.Json.Formatting.Indented);
            return json;
        }
    }
}
