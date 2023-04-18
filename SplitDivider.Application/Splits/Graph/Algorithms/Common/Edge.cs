namespace SplitDivider.Application.Splits.Graph.Algorithms;

public class Edge<T> : IComparable<Edge<T>> where T : IComparable<T>
{
    public T Value { get; set; }
    
    public int DestinationVertexId { get; set; }

    public int CompareTo(Edge<T>? other)
    {
        if (other == null) return 1;

        var compDest = DestinationVertexId.CompareTo(other.DestinationVertexId);

        return compDest != 0 ? compDest : Value.CompareTo(other.Value);
    }
}