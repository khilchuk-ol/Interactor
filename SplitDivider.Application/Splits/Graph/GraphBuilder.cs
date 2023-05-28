using System.Xml;
using QuikGraph;
using QuikGraph.Serialization;
using Shared.Values.ValueObjects;
using SplitDivider.Application.Common.Interfaces;
using SplitDivider.Application.Splits.Graph.Algorithms;
using SplitDivider.Application.Splits.Graph.Common;
using SplitDivider.Application.Splits.Graph.Interfaces;
using SplitDivider.Application.Splits.Graph.Serialization.Common;

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
        if (split == null) throw new ArgumentNullException(nameof(split));
        if (userIds == null) throw new ArgumentNullException(nameof(userIds));

        var qGraph = new UndirectedGraph<int, WeightedEdge<int, int>>();
        
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
                
                var intCost = 1;

                if (relations.Count > 0)
                {
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

                    intCost = (int)(cost * 10);
                }

                var edge = new WeightedEdge<int, int>(userId, contactId, intCost / 100);
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

    private void SaveGraph(int splitId, UndirectedGraph<int, WeightedEdge<int, int>> graph)
    {
        if (graph == null) throw new ArgumentNullException(nameof(graph));
        
        var filename = $"split{splitId}.graphml";
        var fi = new FileInfo(@"Visualization/" + filename);

        using (var fs = fi.Open(FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read))
        {
            using (var xmlWriter = XmlWriter.Create(fs))
            {
                graph.SerializeToGraphML<int, WeightedEdge<int, int>, UndirectedGraph<int, WeightedEdge<int, int>>>(xmlWriter);
            }
        }
    }
}