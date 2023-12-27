using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Entity;


namespace Infrastructure.Helpers
{
    public class DgmlParser
    {
        public Node ParseDgml(string filePath)
        {
            XDocument doc = XDocument.Load(filePath);
            var nodes = new Dictionary<string, Node>();
            var links = new List<KeyValuePair<string, string>>();



            // Parse nodes
            foreach (var xNode in doc.Descendants("DirectedGraph").Descendants("Nodes").Descendants("Node"))
            {
                var id = xNode.Attribute("Id").Value;
                var node = new Node(id, filePath);
                nodes[id] = node;
            }

            // Parse links
            foreach (var xLink in doc.Descendants("Links").Descendants("Link"))
            {
                var source = xLink.Attribute("Source").Value;
                var target = xLink.Attribute("Target").Value;
                links.Add(new KeyValuePair<string, string>(source, target));
            }

            // Construct the graph
            foreach (var link in links)
            {
                if (nodes.TryGetValue(link.Key, out var parentNode) &&
                    nodes.TryGetValue(link.Value, out var childNode))
                {
                    parentNode.Children.Add(childNode);
                }
            }

            // Return the root node (assuming the first node as root for this example)
            return nodes.Values.FirstOrDefault();
        }
    }
}
