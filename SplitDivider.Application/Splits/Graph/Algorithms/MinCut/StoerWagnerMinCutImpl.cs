using SplitDivider.Application.Splits.Graph.Algorithms.Common;

namespace SplitDivider.Application.Splits.Graph.Algorithms;

public class StoerWagnerMinCutImpl<TVertex> where TVertex : IComparable<TVertex>
{
    private float penaltyFactor;
    
    public record CutOfThePhase {
        public int S { get; init; }
        public int T { get; init; }
        public float Weight { get; init; }
    }

    public MinCut<TVertex> ComputeMinCut(Graph<TVertex, int> g) 
    {
        if(g.VerticesCount < 2)
        {
            throw new ArgumentException("Graph must have at least two vertices");
        }
        
        // validate graph weights
        foreach (var vId in g.GetVerticesIds())
        {
            foreach (var e in g.GetEdges(vId))
            {
                if (e.Value <= 0) throw new ArgumentException("Graph must have positive edge weights");
            }
        }

        penaltyFactor = CalculatePenaltyFactor(g);

        var originalGraph = g.Copy();
        
        var currentPartition = new HashSet<int>();
        HashSet<int>? currentBestPartition = null;
        CutOfThePhase? currentBestCut = null;
        
        while (g.VerticesCount > 1) 
        {
            CutOfThePhase cutOfThePhase = MaximumAdjacencySearch(g);
            
            if (currentBestCut == null || cutOfThePhase.Weight < currentBestCut.Weight) 
            {
                currentBestCut = cutOfThePhase;
                currentBestPartition = new HashSet<int>(currentPartition);
                currentBestPartition.Add(cutOfThePhase.T);
            }
            
            currentPartition.Add(cutOfThePhase.T);
            
            // merge s and t and their edges together
            g = MergeVerticesFromCut(g, cutOfThePhase);
        }

        return ConstructMinCutResult(originalGraph, currentBestPartition);
    }

    private static float CalculatePenaltyFactor(Graph<TVertex, int> g)
    {
        int totalEdgeWeight = 0;
        int edgeCount = 0;

        foreach (var v in g.GetVerticesIds())
        {
            foreach (var e in g.GetEdges(v))
            {
                totalEdgeWeight += e.Value;
                edgeCount++;
            }
        }

        var averageEdgeWeight = totalEdgeWeight / (float)edgeCount;

        // Set PENALTY_FACTOR to be 10-20% of the average edge weight
        return 0.15f * averageEdgeWeight;
    }

    private MinCut<TVertex> ConstructMinCutResult(Graph<TVertex, int> originalGraph, HashSet<int>? partition) 
    {
        if (partition == null)
        {
            throw new Exception("partition is null");
        }
        
        var first = new Graph<TVertex, int>();
        var second = new Graph<TVertex, int>();
        
        int cutWeight = 0;

        foreach (var v in originalGraph.GetVerticesIds())
        {
            var vertex = new Vertex<TVertex>
            {
                Id = v,
                Value = originalGraph.GetVertex(v).Value
            };
            
            if (partition.Contains(v)) 
            {
                first.AddVertex(vertex);
            } 
            else 
            {
                second.AddVertex(vertex);
            }
        }

        var edgeSet = new HashSet<ValueTuple<int, int>>();
        
        foreach (var v in originalGraph.GetVerticesIds()) 
        {
            foreach (var e in originalGraph.GetEdges(v)) 
            {
                if (first.GetVerticesIds().Contains(v) && first.GetVerticesIds().Contains(e.DestinationVertexId)) 
                {
                    first.AddEdge(v, new Edge<int>
                    {
                        DestinationVertexId = e.DestinationVertexId,
                        Value = e.Value
                    });
                } 
                else if (second.GetVerticesIds().Contains(v) && second.GetVerticesIds().Contains(e.DestinationVertexId)) 
                {
                    second.AddEdge(v, new Edge<int>
                    {
                        DestinationVertexId = e.DestinationVertexId,
                        Value = e.Value
                    });
                } 
                else 
                {
                    if (edgeSet.Add(new ValueTuple<int, int>(v, e.DestinationVertexId))) 
                    {
                        cutWeight += e.Value;
                    }
                }
            }
        }

        return new MinCut<TVertex>(first, second, cutWeight);
    }

    Graph<TVertex, int> MergeVerticesFromCut(Graph<TVertex, int> g, CutOfThePhase cutOfThePhase) 
    {
        var toReturn = new Graph<TVertex, int>();

        foreach (var v in g.GetVerticesIds()) 
        {
            // plain copy
            if (cutOfThePhase.S != v && cutOfThePhase.T != v) 
            {
                toReturn.AddVertex(g.GetVertex(v));
                
                foreach (var e in g.GetEdges(v)) 
                {
                    if (e.DestinationVertexId != cutOfThePhase.S && e.DestinationVertexId != cutOfThePhase.T) 
                    {
                        toReturn.AddEdge(v, new Edge<int>
                        {
                            DestinationVertexId = e.DestinationVertexId,
                            Value = e.Value
                        });
                    }
                }
            }

            if (cutOfThePhase.S == v) 
            {
                toReturn.AddVertex(g.GetVertex(v));
                
                foreach (var e in g.GetEdges(v)) 
                {
                    if (e.DestinationVertexId == cutOfThePhase.T) continue;
                    
                    var mergableEdge = g.GetEdge(cutOfThePhase.T, e.DestinationVertexId);
                    if (mergableEdge != null) 
                    {
                        toReturn.AddEdge(v, new Edge<int>
                        {
                            DestinationVertexId = e.DestinationVertexId,
                            Value = e.Value + mergableEdge.Value
                        });
                        
                        toReturn.AddEdge(e.DestinationVertexId, new Edge<int>
                        {
                            DestinationVertexId = v,
                            Value = e.Value + mergableEdge.Value
                        });
                    } 
                    else 
                    {
                        toReturn.AddEdge(v, new Edge<int>
                        {
                            DestinationVertexId = e.DestinationVertexId,
                            Value = e.Value
                        });
                        
                        toReturn.AddEdge(e.DestinationVertexId, new Edge<int>
                        {
                            DestinationVertexId = v,
                            Value = e.Value
                        });
                    }
                }
            }
        }

        foreach (var e in g.GetEdges(cutOfThePhase.T)) 
        {
            if (e.DestinationVertexId == cutOfThePhase.S) continue;

            var transferEdge = g.GetEdge(cutOfThePhase.S, e.DestinationVertexId);
            if (transferEdge == null) 
            {
                toReturn.AddEdge(cutOfThePhase.S, new Edge<int>
                {
                    DestinationVertexId = e.DestinationVertexId,
                    Value = e.Value
                });
                
                toReturn.AddEdge(e.DestinationVertexId, new Edge<int>
                {
                    DestinationVertexId = cutOfThePhase.S,
                    Value = e.Value
                });
            }
        }


        return toReturn;
    }

    private CutOfThePhase MaximumAdjacencySearch(Graph<TVertex, int> g) 
    {
        return MaximumAdjacencySearch(g, null);
    }
    
    private CutOfThePhase MaximumAdjacencySearch(Graph<TVertex, int> g, int? start)
    {
        if (start == null) start = g.GetVerticesIds().First();

        var maxAdjacencyOrderedList = new List<int> { start.Value };
        var cutWeight = new List<int>();
        var candidates = new List<int>(g.GetVerticesIds());

        candidates.Remove(start.Value);

        while (candidates.Count != 0)
        {
            int? maxNextVertex = null;
            int maxWeight = Int32.MinValue;
            int currentSetSize = maxAdjacencyOrderedList.Count;
            int remainingVertices = candidates.Count;

            foreach (var next in candidates)
            {
                int weightSum = 0;

                foreach (var s in maxAdjacencyOrderedList)
                {
                    var edge = g.GetEdge(next, s);
                    if (edge != null)
                    {
                        weightSum += edge.Value;
                    }
                }

                // Apply penalty based on imbalance and remaining vertices
                int imbalance = Math.Abs(currentSetSize + 1 - (remainingVertices - 1));
                if (imbalance < remainingVertices / 2) // Only consider adding if it won't unbalance too much
                {
                    weightSum -= (int)(imbalance * penaltyFactor);
                }

                if (weightSum > maxWeight)
                {
                    maxNextVertex = next;
                    maxWeight = weightSum;
                }
            }

            if (maxNextVertex != null)
            {
                candidates.Remove(maxNextVertex.Value);
                maxAdjacencyOrderedList.Add(maxNextVertex.Value);
            }

            cutWeight.Add(maxWeight);
        }

        var n = maxAdjacencyOrderedList.Count;

        return new CutOfThePhase
        {
            S = maxAdjacencyOrderedList.ElementAt(n - 2),
            T = maxAdjacencyOrderedList.ElementAt(n - 1),
            Weight = cutWeight.ElementAt(cutWeight.Count - 1)
        };
    }

    
    // with penalty
    // CutOfThePhase MaximumAdjacencySearch(Graph<TVertex, int> g, int? start)
    // {
    //     if (start == null) start = g.GetVerticesIds().First();
    //
    //     var maxAdjacencyOrderedList = new List<int> { start.Value };
    //     var cutWeight = new List<int>();
    //     var candidates = new List<int>(g.GetVerticesIds());
    //
    //     candidates.Remove(start.Value);
    //
    //     while (candidates.Count != 0)
    //     {
    //         int? maxNextVertex = null;
    //         int maxWeight = Int32.MinValue;
    //         int currentSetSize = maxAdjacencyOrderedList.Count;
    //
    //         foreach (var next in candidates)
    //         {
    //             int weightSum = 0;
    //
    //             foreach (var s in maxAdjacencyOrderedList)
    //             {
    //                 var edge = g.GetEdge(next, s);
    //                 if (edge != null)
    //                 {
    //                     weightSum += edge.Value;
    //                 }
    //             }
    //
    //             // Penalize selection if it leads to an unbalanced cut
    //             weightSum -= (int)(Math.Abs(currentSetSize - (candidates.Count + 1)) * penaltyFactor);
    //
    //             if (weightSum > maxWeight)
    //             {
    //                 maxNextVertex = next;
    //                 maxWeight = weightSum;
    //             }
    //         }
    //
    //         if (maxNextVertex != null)
    //         {
    //             candidates.Remove(maxNextVertex.Value);
    //             maxAdjacencyOrderedList.Add(maxNextVertex.Value);
    //         }
    //
    //         cutWeight.Add(maxWeight);
    //     }
    //
    //     var n = maxAdjacencyOrderedList.Count;
    //
    //     return new CutOfThePhase
    //     {
    //         S = maxAdjacencyOrderedList.ElementAt(n - 2),
    //         T = maxAdjacencyOrderedList.ElementAt(n - 1),
    //         Weight = cutWeight.ElementAt(cutWeight.Count - 1)
    //     };
    // }


    // initial
    // CutOfThePhase MaximumAdjacencySearch(Graph<TVertex, int> g, int? start)
    // {
    //     if (start == null) start = g.GetVerticesIds().First();
    //
    //     var maxAdjacencyOrderedList = new List<int> { start.Value };
    //     var cutWeight = new List<int>();
    //     var candidates = new List<int>(g.GetVerticesIds());
    //
    //     candidates.Remove(start.Value);
    //
    //     while (candidates.Count != 0)
    //     {
    //         int? maxNextVertex = null;
    //         int maxWeight = Int32.MinValue;
    //
    //         foreach (var next in candidates)
    //         {
    //             int weightSum = 0;
    //
    //             foreach (var s in maxAdjacencyOrderedList)
    //             {
    //                 var edge = g.GetEdge(next, s);
    //
    //                 if (edge != null)
    //                 {
    //                     weightSum += edge.Value;
    //                 }
    //             }
    //
    //             if (weightSum > maxWeight)
    //             {
    //                 maxNextVertex = next;
    //                 maxWeight = weightSum;
    //             }
    //         }
    //
    //         if (maxNextVertex != null)
    //         {
    //             candidates.Remove(maxNextVertex.Value);
    //             maxAdjacencyOrderedList.Add(maxNextVertex.Value);
    //         }
    //
    //         cutWeight.Add(maxWeight);
    //     }
    //
    //     var n = maxAdjacencyOrderedList.Count;
    //
    //     return new CutOfThePhase
    //     {
    //         S = maxAdjacencyOrderedList.ElementAt(n - 2),
    //         T = maxAdjacencyOrderedList.ElementAt(n - 1),
    //         Weight = cutWeight.ElementAt(cutWeight.Count - 1)
    //     };
    // }
}