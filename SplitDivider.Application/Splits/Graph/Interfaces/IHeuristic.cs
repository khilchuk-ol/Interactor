using SplitDivider.Application.Splits.Graph.Algorithms.Common;

namespace SplitDivider.Application.Splits.Graph.Interfaces;

public interface IHeuristic<TVertex> where TVertex : IComparable<TVertex>
{
    public (int Source, int Sink) GetSourceAndSink(Graph<TVertex, int> g);
}