namespace SplitDivider.Application.Splits.Graph.Algorithms.Common;

public class MinCut<TVertex> where TVertex: IComparable<TVertex>
{
    public MinCut(Graph<TVertex, int> first, Graph<TVertex, int> second, float minCutWeight)
    {
        First = first;
        Second = second;
        MinCutWeight = minCutWeight;
    }

    public Graph<TVertex, int> First { get; }
        
    public Graph<TVertex, int> Second { get; }
        
    public float MinCutWeight { get; }
}