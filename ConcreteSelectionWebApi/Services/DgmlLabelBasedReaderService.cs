using System.IO;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace ConcreteSelectionWebApi.Services;

public class DgmlLabelBasedReaderService : IDgmlLabelBasedReaderService
{
    private Dictionary<string, List<string>> _nodeLinks;
    private Dictionary<string, string> _nodeLabels;
    private readonly XDocument _document;
    private const string _filePath = "master.dgml";

    public DgmlLabelBasedReaderService()
    {
        _document = XDocument.Load(_filePath);
        _nodeLinks = new Dictionary<string, List<string>>();
        _nodeLabels = new Dictionary<string, string>();

        ParseNodesAndLinks();
    }

    private void ParseNodesAndLinks()
    {
        var ns = "http://schemas.microsoft.com/vs/2009/dgml";
        var nodes = _document.Descendants(XName.Get("Node", ns));
        foreach (var node in nodes)
        {
            var id = node.Attribute("Id").Value;
            var label = node.Attribute("Label")?.Value ?? id;
            _nodeLabels[id] = label;
        }

        var links = _document.Descendants(XName.Get("Link", ns));
        foreach (var link in links)
        {
            var source = link.Attribute("Source").Value;
            var target = link.Attribute("Target").Value;

            if (_nodeLinks.ContainsKey(source))
            {
                _nodeLinks[source].Add(target);
            }
            else
            {
                _nodeLinks[source] = new List<string> { target };
            }
        }
    }

    public string GetNodeTreeByLabelAsJson(string entryPointLabel)
    {
        var entryPointId = _nodeLabels.FirstOrDefault(x => x.Value == entryPointLabel).Key;
        if (entryPointId == null)
        {
            return null; // or throw an exception if the label does not exist
        }

        var result = new Dictionary<string, object>();
        ConstructNodeTree(entryPointId, result);

        return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
    }

    private void ConstructNodeTree(string nodeId, Dictionary<string, object> currentNode)
    {
        if (!_nodeLinks.ContainsKey(nodeId)) return;

        foreach (var childId in _nodeLinks[nodeId])
        {
            var childNode = new Dictionary<string, object>();
            ConstructNodeTree(childId, childNode);
            var childLabel = _nodeLabels[childId];
            if (childNode.Count > 0)
            {
                currentNode[childLabel] = childNode;
            }
            else
            {
                currentNode[childLabel] = null;
            }
        }
    }
}

