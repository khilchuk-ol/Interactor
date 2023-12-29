using SplitDivider.Application.Splits.Graph.Algorithms;
using SplitDivider.Application.Splits.Graph.Common;
using SplitDivider.Application.Splits.Graph.Interfaces;

namespace SplitDivider.Application.Splits.Graph;

public class StoerWagnerMinCutGraphCutter : IGraphCutter
{
    public (List<int> first, List<int> second) CutSplitGraph(SplitGraphDto graphDto)
    {
        var minCut = new StoerWagnerMinCutImpl<int>();

        var cut = minCut.ComputeMinCut(graphDto.Graph);

        var verticesFirstGroup = new List<int>(cut.First.GetVerticesIds());

        var verticesSecondGroup = new List<int>(cut.Second.GetVerticesIds());

        return (verticesFirstGroup, verticesSecondGroup);
    }
}