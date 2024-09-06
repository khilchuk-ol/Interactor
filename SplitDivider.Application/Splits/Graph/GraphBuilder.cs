using System.Collections.Concurrent;
using System.Xml;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
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

    private readonly IServiceProvider _services;
    
    private const int MIN_EDGE_WEIGHT = 1;

    public GraphBuilder(IApplicationDbContext context, IServiceProvider services)
    {
        _context = context;
        _services = services;
    }

    public SplitGraphDto BuildGraph(Split split, List<int> userIds)
    {
        var qGraph = new UndirectedGraph<int, WeightedEdge<int, int>>();
        
        var graph = new Algorithms.Common.Graph<int, int>();
        var connections = new ConcurrentDictionary<int, int>();

        var now = DateTime.Now;

        foreach (var uId in userIds)
        {
            graph.AddVertex(new Vertex<int>
            {
                Id = uId,
                Value = uId
            });
        }
        
        var ctxLock = new object();
        var graphLock = new object();

        Parallel.ForEach(userIds, new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount * 10 }, userId =>
        {
            List<Relation> userRelations;
            
            lock (ctxLock)
            {
                userRelations = _context.Relations
                    .Where(r => r.UserId == userId)
                    .ToList();
            }
            
            foreach (var contactId in userIds)
            {
                if (userId == contactId) continue;

                var relations = userRelations.Where(r => r.ContactId == contactId).ToList();
                
                var intCost = 1;

                if (relations.Count > 0)
                {
                    var cost = 0.0;
                    
                    foreach (var relation in relations)
                    {
                        var interaction = InteractionType.From(relation.Interaction);

                        if (!split.ActionsWeights.Keys.Contains(interaction)) continue;
                
                        var generalWeight = split.ActionsWeights[interaction];

                        var timeDiff = now.Subtract(relation.Dt).TotalHours / 24;

                        var k = 1 / (1 + timeDiff * timeDiff);

                        var weight = generalWeight * k;

                        cost += weight;
                    }

                    intCost = Math.Max((int)(cost * 10), MIN_EDGE_WEIGHT);
                }

                var edge = new WeightedEdge<int, int>(userId, contactId, intCost / 100);

                lock (graphLock)
                {
                    qGraph.AddVerticesAndEdge(edge);
                
                    graph.AddEdge(userId, new Algorithms.Edge<int>
                    {
                        DestinationVertexId = contactId,
                        Value = intCost
                    });
                }

                connections.AddOrUpdate(userId, (_) => 1, (_, old) => ++old);
                connections.AddOrUpdate(contactId, (_) => 1, (_, old) => ++old);
            }
        });
        
        SaveGraph(split.Id, qGraph);

        return new SplitGraphDto
        {
            Graph = graph,
            Connections = connections,
        };
    }

    private void SaveGraph(int splitId, UndirectedGraph<int, WeightedEdge<int, int>> graph)
    {
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