using SplitDivider.Application.Splits.Graph.Algorithms.Common;
using SplitDivider.Application.Splits.Graph.Algorithms.Grouping;
using SplitDivider.Application.Splits.Graph.Common;
using SplitDivider.Application.Splits.Graph.Interfaces;

namespace SplitDivider.Application.Splits.Graph;

// alternative algorithm
public class RelatedGroupsGraphPartitioner : IGraphPartitioner
{
    private const string _name = "Related Groups (alternative) Algorithm";

    private const double PERCENTILE = 0.9;

    public (List<int> first, List<int> second) PartitionSplitGraph(SplitGraphDto graphDto)
    {
        var percentile = CalculateMaxMinAverageWeight(graphDto.Graph);
        
        var partitioner = new RelatedGroupsImpl<int>(graphDto.Graph, percentile);

        var res = partitioner.ComputePartitioning();

        var first = res.First.Select(v => v.Id).ToList();
        var second = res.Second.Select(v => v.Id).ToList();

        return (first, second);
    }
    
    private double CalculatePercentileWeight(Graph<int, int> g, double percentile)
    {
        var allWeights = new List<double>();

        foreach (var vId in g.GetVerticesIds())
        {
            foreach (var edge in g.GetEdges(vId))
            {
                allWeights.Add(edge.Value);
            }
        }

        allWeights.Sort();
        int index = (int)(percentile * allWeights.Count);
        return allWeights[Math.Min(index, allWeights.Count - 1)];
    }
    
    double CalculateMaxMinAverageWeight(Graph<int, int> g)
    {
        double maxWeight = double.MinValue;
        double minWeight = double.MaxValue;
        
        foreach (var vId in g.GetVerticesIds())
        {
            foreach (var edge in g.GetEdges(vId))
            {
                if (edge.Value > maxWeight)
                    maxWeight = edge.Value;
                if (edge.Value < minWeight)
                    minWeight = edge.Value;
            }
        }

        return (maxWeight + minWeight) / 2.0;
    }
    
    double CalculateMedianWeight(Graph<int, int> g)
    {
        var allWeights = new List<double>();

        foreach (var vId in g.GetVerticesIds())
        {
            foreach (var edge in g.GetEdges(vId))
            {
                allWeights.Add(edge.Value);
            }
        }

        allWeights.Sort();

        int count = allWeights.Count;
        if (count % 2 == 0)
        {
            return (allWeights[count / 2 - 1] + allWeights[count / 2]) / 2.0;
        }
        else
        {
            return allWeights[count / 2];
        }
    }

    public string GetName()
    {
        return _name;
    }
}