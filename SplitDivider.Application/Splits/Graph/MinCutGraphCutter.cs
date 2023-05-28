using SplitDivider.Application.Splits.Graph.Algorithms;
using SplitDivider.Application.Splits.Graph.Common;
using SplitDivider.Application.Splits.Graph.Interfaces;

namespace SplitDivider.Application.Splits.Graph;

public class MinCutGraphCutter : IGraphCutter
{
    public (List<int> first, List<int> second) CutSplitGraph(SplitGraphDto graphDto)
    {
        if (graphDto == null) throw new ArgumentNullException(nameof(graphDto));
        
        var minCut = new StoerWagnerMinCut<int>();

        var cut = minCut.ComputeMinCut(graphDto.Graph);

        var verticesFirstGroup = new List<int>(cut.First.GetVerticesIds());

        var verticesSecondGroup = new List<int>(cut.Second.GetVerticesIds());

        return (verticesFirstGroup, verticesSecondGroup);
    }
}