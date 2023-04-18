using System.Xml;
using QuikGraph;
using QuikGraph.Serialization;
using Shared.Values.ValueObjects;
using SplitDivider.Application.Common.Interfaces;
using SplitDivider.Application.Splits.Graph.Algorithms;
using SplitDivider.Application.Splits.Graph.Common;
using SplitDivider.Application.Splits.Graph.Interfaces;

namespace SplitDivider.Application.Splits.Graph;

public class GraphBuilder : IGraphBuilder
{
    private readonly IApplicationDbContext _context;

    public GraphBuilder(IApplicationDbContext context)
    {
        _context = context;
    }

    public SplitGraphDto BuildGraph(Split split, List<int> userIds)
    {
        var qGraph = new UndirectedGraph<int, QuikGraph.Edge<int>>();
        var graph = new Algorithms.Common.Graph<int, int>();
        var connections = new Dictionary<int, int>();

        var now = DateTime.Now;

        foreach (var uId in userIds)
        {
            graph.AddVertex(new Vertex<int>
            {
                Id = uId,
                Value = uId
            });
        }

        foreach (var userId in userIds)
        {
            foreach (var contactId in userIds)
            {
                if (userId == contactId) continue;
                
                var relations = _context.Relations
                    .Where(r => r.UserId == userId && r.ContactId == contactId)
                    .ToList();

                var cost = 0.0;
                
                foreach (var relation in relations)
                {
                    var interaction = InteractionType.From(relation.Interaction);

                    if (!split.ActionsWeights.Keys.Contains(interaction)) continue;
                
                    var generalWeight = split.ActionsWeights[interaction];

                    var timeDiff = now.Subtract(relation.Dt).Hours / 24;

                    var k = 1 / (1 + timeDiff * timeDiff);

                    var weight = generalWeight * k;

                    cost += weight;
                }

                var intCost = (int)(cost * 10);

                var edge = new QuikGraph.Edge<int>(userId, contactId);
                qGraph.AddVerticesAndEdge(edge);
                
                graph.AddEdge(userId, new Algorithms.Edge<int>
                {
                    DestinationVertexId = contactId,
                    Value = intCost
                });

                if (connections.Keys.Contains(userId)) connections[userId] += 1;
                else connections.Add(userId, 1);
                
                if (connections.Keys.Contains(contactId)) connections[contactId] += 1;
                else connections.Add(contactId, 1);
            }
        }
        
        SaveGraph(split.Id, qGraph);

        return new SplitGraphDto
        {
            Graph = graph,
            Connections = connections,
        };
    }

    private void SaveGraph(int splitId, UndirectedGraph<int, QuikGraph.Edge<int>> graph)
    {
        var filename = $"split{splitId}.xml";
        var fi = new FileInfo(@"../../Visualization/" + filename);

        using (var fs = fi.Open(FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read))
        {
            using (var xmlWriter = XmlWriter.Create(fs))
            {
                graph.SerializeToGraphML<int, QuikGraph.Edge<int>, UndirectedGraph<int, QuikGraph.Edge<int>>>(xmlWriter);
            }
        }
    }
}