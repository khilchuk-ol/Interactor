namespace SplitDivider.Application.Splits.Graph.Algorithms;

public class Vertex<T> : IComparable<Vertex<T>> where T : IComparable<T>
{
    public int Id { get; set; }
    
    public T Value { get; set; }
    
    public int CompareTo(Vertex<T>? other)
    {
        if (other == null) return 1;

        var compDest = Id.CompareTo(other.Id);

        return compDest != 0 ? compDest : Value.CompareTo(other.Value);
    }
}