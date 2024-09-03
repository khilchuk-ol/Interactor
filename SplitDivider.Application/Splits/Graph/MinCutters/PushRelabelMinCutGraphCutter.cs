using SplitDivider.Application.Splits.Graph.Algorithms;
using SplitDivider.Application.Splits.Graph.Algorithms.MinCut;
using SplitDivider.Application.Splits.Graph.Common;
using SplitDivider.Application.Splits.Graph.Interfaces;

namespace SplitDivider.Application.Splits.Graph;

public class PushRelabelMinCutGraphCutter : IGraphCutter
{
    private const string _name = "Push-Relabel Algorithm";

    private IHeuristic<int> _heuristic;

    public PushRelabelMinCutGraphCutter(IHeuristic<int> h)
    {
        _heuristic = h;
    }

    public (List<int> first, List<int> second) CutSplitGraph(SplitGraphDto graphDto)
    {
        var (source, sink) = _heuristic.GetSourceAndSink(graphDto.Graph);
        
        var minCut = new PushRelabelMinCutImpl<int>(graphDto.Graph, source, sink);

        var cut = minCut.ComputeMinCut();

        var verticesFirstGroup = new List<int>(cut.First.GetVerticesIds());

        var verticesSecondGroup = new List<int>(cut.Second.GetVerticesIds());

        return (verticesFirstGroup, verticesSecondGroup);
    }

    public string GetName()
    {
        return _name;
    }
}