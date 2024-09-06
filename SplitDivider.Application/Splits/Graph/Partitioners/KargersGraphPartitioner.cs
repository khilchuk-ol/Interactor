using SplitDivider.Application.Splits.Graph.Algorithms;
using SplitDivider.Application.Splits.Graph.Common;
using SplitDivider.Application.Splits.Graph.Interfaces;

namespace SplitDivider.Application.Splits.Graph;

public class KargersGraphPartitioner : IGraphPartitioner
{
    private const string _name = "Karger's Algorithm";
    
    public (List<int> first, List<int> second) PartitionSplitGraph(SplitGraphDto graphDto)
    {
        var minCut = new KargersMinCutImpl<int>(graphDto.Graph.VerticesCount, graphDto.Graph.EdgesCount);

        var cut = minCut.ComputeMinCut(graphDto.Graph);

        var verticesFirstGroup = new List<int>(cut.First.GetVerticesIds());

        var verticesSecondGroup = new List<int>(cut.Second.GetVerticesIds());

        return (verticesFirstGroup, verticesSecondGroup);
    }

    public string GetName()
    {
        return _name;
    }
}