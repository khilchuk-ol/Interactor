using System;
using System.Collections.Generic;
using System.Linq;
using SplitDivider.Application.Splits.Graph.Algorithms.Common;

namespace SplitDivider.Application.Splits.Graph.Algorithms.Grouping;

public class RelatedGroupsImpl<TVertex> where TVertex : IComparable<TVertex>
{
    private readonly Graph<TVertex, int> _graph;
    private readonly Dictionary<int, List<Edge<int>>> _adjacencyList = new();
    private readonly HashSet<int> _visited;
    private double _weightThreshold;

    public RelatedGroupsImpl(Graph<TVertex, int> g, double initialThreshold)
    {
        _graph = g;
        _weightThreshold = initialThreshold;
        _visited = new HashSet<int>();
        
        foreach (var vertexId in g.GetVerticesIds())
        {
            _adjacencyList[vertexId] = g.GetEdges(vertexId);
        }
    }

    public class PartitioningResult
    {
        public List<Vertex<TVertex>> First { get; set; } = new();
        
        public List<Vertex<TVertex>> Second { get; set; } = new();
    }

    public PartitioningResult ComputePartitioning()
    {
        var subgraphs = PartitionGraphIntoSubGraphs();

        var rnd = new Random();
        var res = new PartitioningResult();

        foreach (var subgraph in subgraphs)
        {
            var group = rnd.Next(1, 3);

            if (group == 1)
            {
                foreach (var vId in subgraph)
                {
                    res.First.Add(_graph.GetVertex(vId));
                }
            }
            else
            {
                foreach (var vId in subgraph)
                {
                    res.Second.Add(_graph.GetVertex(vId));
                }
            }
        }

        return res;
    }

    private List<List<int>> PartitionGraphIntoSubGraphs()
    {
        var subgraphs = new List<List<int>>();

        foreach (var vertexId in _graph.GetVerticesIds())
        {
            if (!_visited.Contains(vertexId))
            {
                var subgraph = ExtractSubgraph(vertexId);
                if (subgraph.Count > 0)
                {
                    subgraphs.Add(subgraph);
                }
            }
        }

        return subgraphs;
    }

    private List<int> ExtractSubgraph(int startVertexId)
    {
        var subgraph = new List<int>();
        var queue = new Queue<int>();
        queue.Enqueue(startVertexId);
        _visited.Add(startVertexId);

        while (queue.Count > 0)
        {
            var currentVertexId = queue.Dequeue();
            subgraph.Add(currentVertexId);

            foreach (var edge in _adjacencyList[currentVertexId])
            {
                // Check if the edge weight is above the threshold
                if (edge.Value > _weightThreshold && !_visited.Contains(edge.DestinationVertexId))
                {
                    queue.Enqueue(edge.DestinationVertexId);
                    _visited.Add(edge.DestinationVertexId);
                }
            }
        }

        // Adjust the threshold dynamically
        AdjustThreshold(subgraph.Count);

        return subgraph;
    }

    private void AdjustThreshold(int subgraphSize)
    {
        // You can make the adjustment more sophisticated
        // This is a basic implementation that slightly reduces the threshold
        // when subgraphs become too large, to limit their size
        if (subgraphSize > 10) // Arbitrary size limit, can be adjusted
        {
            _weightThreshold *= 1.05; // Increase the threshold to limit subgraph size
        }
        else if (subgraphSize < 5)
        {
            _weightThreshold *= 0.95; // Decrease the threshold for smaller subgraphs
        }
    }
}