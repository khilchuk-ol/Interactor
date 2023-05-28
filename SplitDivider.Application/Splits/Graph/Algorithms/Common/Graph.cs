namespace SplitDivider.Application.Splits.Graph.Algorithms.Common;

public class Graph<TVertex, TEdge> where TVertex : IComparable<TVertex> where TEdge : IComparable<TEdge>
{
    public List<Vertex<TVertex>> Vertices { get; set; } = new();

    private Dictionary<int, Vertex<TVertex>> _verticesIds = new();

    private Dictionary<int, List<int>> _adjacentVertices = new();

    private Dictionary<int, List<Edge<TEdge>>> _adjacentEdges = new();
    
    public int VerticesCount => Vertices.Count;
    
    public int EdgesCount => _adjacentVertices.Count;
    
    public Graph() {}

    private Graph(List<Vertex<TVertex>> vertices, 
        Dictionary<int, Vertex<TVertex>> verticesIds,
        Dictionary<int, List<int>> adjacentVertices,
        Dictionary<int, List<Edge<TEdge>>> adjacentEdges)
    {
        Vertices = vertices;
        _verticesIds = verticesIds;
        _adjacentVertices = adjacentVertices;
        _adjacentEdges = adjacentEdges;
    }

    public List<Vertex<TVertex>> GetAdjacentVertices(int vertexId)
    {
        var res = new List<Vertex<TVertex>>();

        foreach (var v in _adjacentVertices[vertexId])
        {
            res.Add(_verticesIds[v]);
        }

        return res;
    }

    public List<int> GetVerticesIds()
    {
        return _verticesIds.Keys.ToList();
    }
    
    public void AddVertex(Vertex<TVertex> vertex, params Edge<TEdge>[] adjacents)
    {
        if (vertex == null) throw new ArgumentNullException(nameof(vertex));
        
        foreach (var edge in adjacents)
        {
            AddVertex(vertex, edge);
        }
    }

    public void AddVertex(Vertex<TVertex> vertex) 
    {
        Vertices.Add(vertex);
        _verticesIds[vertex.Id] = vertex;
    }

    public void AddVertex(Vertex<TVertex> vertex, Edge<TEdge> adjacent) 
    {
        if (vertex == null) throw new ArgumentNullException(nameof(vertex));
        if (adjacent == null) throw new ArgumentNullException(nameof(adjacent));
        
        AddVertex(vertex);
        AddEdge(vertex.Id, adjacent);
    }

    public void AddEdge(int vertexId, Edge<TEdge> edge) 
    {
        if (edge == null) throw new ArgumentNullException(nameof(edge));
        
        if (!_adjacentEdges.ContainsKey(vertexId))
        {
            _adjacentEdges[vertexId] = new() { edge };
        }
        else
        {
            _adjacentEdges[vertexId].Add(edge);
        }
        
        if (!_adjacentVertices.ContainsKey(vertexId))
        {
            _adjacentVertices[vertexId] = new() { edge.DestinationVertexId };
        }
        else
        {
            _adjacentVertices[vertexId].Add(edge.DestinationVertexId);
        }
    }
    
    public List<Edge<TEdge>> GetEdges(int vertexId)
    {
        return _adjacentEdges[vertexId];
    }

    public Edge<TEdge>? GetEdge(int source, int dest)
    {
        if (!_adjacentEdges.ContainsKey(source))
        {
            return null;
        }

        foreach (var edge in _adjacentEdges[source])
        {
            if (edge.DestinationVertexId == dest) return edge;
        }

        return null;
    }

    public Vertex<TVertex> GetVertex(int vertexId)
    {
        return _verticesIds[vertexId];
    }

    public Graph<TVertex, TEdge> Copy()
    {
        return new Graph<TVertex,TEdge>(
                new List<Vertex<TVertex>>(Vertices),
                new Dictionary<int, Vertex<TVertex>>(_verticesIds),
                new Dictionary<int, List<int>>(_adjacentVertices),
                new Dictionary<int, List<Edge<TEdge>>>(_adjacentEdges));
    }
}