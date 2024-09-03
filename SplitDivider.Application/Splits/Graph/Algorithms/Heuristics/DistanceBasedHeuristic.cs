using SplitDivider.Application.Splits.Graph.Algorithms.Common;
using SplitDivider.Application.Splits.Graph.Interfaces;

namespace SplitDivider.Application.Splits.Graph.Algorithms.Heuristics;

public class DistanceBasedHeuristic<TVertex> : IHeuristic<TVertex> where TVertex : IComparable<TVertex>
{
   private static Random _random = new Random();

    public (int Source, int Sink) GetSourceAndSink(Graph<TVertex, int> graph)
    {
        // Get all vertices in the graph
        var vertices = graph.GetVerticesIds().ToList();
        
        // Select multiple random vertices as candidates (e.g., 5 candidates)
        int candidateCount = Math.Min(5, vertices.Count);
        var candidateVertices = new HashSet<int>();
        
        while (candidateVertices.Count < candidateCount)
        {
            candidateVertices.Add(vertices[_random.Next(vertices.Count)]);
        }
        
        // Run BFS from each candidate and find the farthest vertex for each
        Dictionary<int, int> farthestFromEachCandidate = new Dictionary<int, int>();
        
        foreach (var candidate in candidateVertices)
        {
            int farthestVertex = BFSFindFarthestVertex(graph, candidate);
            farthestFromEachCandidate[candidate] = farthestVertex;
        }
        
        // Find the pair of vertices that are the farthest apart
        int source = -1, sink = -1;
        int maxDistance = int.MinValue;
        
        foreach (var sourceCandidate in farthestFromEachCandidate.Keys)
        {
            foreach (var sinkCandidate in farthestFromEachCandidate.Keys)
            {
                if (sourceCandidate != sinkCandidate)
                {
                    int distance = GetDistanceBetweenVertices(graph, sourceCandidate, sinkCandidate);
                    if (distance > maxDistance)
                    {
                        maxDistance = distance;
                        source = sourceCandidate;
                        sink = sinkCandidate;
                    }
                }
            }
        }
        
        // Return the pair of vertices with the maximum distance
        return (source, sink);
    }

    private int BFSFindFarthestVertex(Graph<TVertex, int> graph, int startVertex)
    {
        var queue = new Queue<int>();
        var distances = new Dictionary<int, int>();

        foreach (var vertexId in graph.GetVerticesIds())
        {
            distances[vertexId] = -1; // Initialize distances as -1 (unvisited)
        }

        queue.Enqueue(startVertex);
        distances[startVertex] = 0; // Distance to itself is 0

        int farthestVertex = startVertex;
        int maxDistance = 0;

        while (queue.Count > 0)
        {
            int currentVertex = queue.Dequeue();

            foreach (var edge in graph.GetEdges(currentVertex))
            {
                int neighbor = edge.DestinationVertexId;
                
                // If the neighbor hasn't been visited yet
                if (distances[neighbor] == -1)
                {
                    distances[neighbor] = distances[currentVertex] + 1;
                    queue.Enqueue(neighbor);

                    // Update farthest vertex and max distance
                    if (distances[neighbor] > maxDistance)
                    {
                        maxDistance = distances[neighbor];
                        farthestVertex = neighbor;
                    }
                }
            }
        }

        return farthestVertex;
    }

    private int GetDistanceBetweenVertices(Graph<TVertex, int> graph, int vertexA, int vertexB)
    {
        // Simple BFS to compute the shortest path distance between vertexA and vertexB
        var queue = new Queue<int>();
        var distances = new Dictionary<int, int>();

        foreach (var vertexId in graph.GetVerticesIds())
        {
            distances[vertexId] = -1;
        }

        queue.Enqueue(vertexA);
        distances[vertexA] = 0;

        while (queue.Count > 0)
        {
            int currentVertex = queue.Dequeue();

            if (currentVertex == vertexB) break; // Early exit when vertexB is reached

            foreach (var edge in graph.GetEdges(currentVertex))
            {
                int neighbor = edge.DestinationVertexId;

                if (distances[neighbor] == -1)
                {
                    distances[neighbor] = distances[currentVertex] + 1;
                    queue.Enqueue(neighbor);
                }
            }
        }

        return distances[vertexB];
    }
}