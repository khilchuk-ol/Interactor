using SplitDivider.Application.Splits.Graph.Algorithms.Common;

namespace SplitDivider.Application.Splits.Graph.Algorithms;

public class StoerWagnerMinCut<TVertex> where TVertex : IComparable<TVertex>
{
    public class MinCut 
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
    
    public record CutOfThePhase {
        public int S { get; init; }
        public int T { get; init; }
        public float Weight { get; init; }
    }
    
    /**
     * Computes the minimum cut (MinCut) of the given non-negatively weighted graph.
     * Running the algorithm results in two disjoint subgraphs, so that the sum of
     * weights between these two subgraphs is minimal.
     *
     * @param g weighted graph you want to cut in half
     * @return a @{@link MinCut} that contains both the first and second disjoint graph,
     * the removed edges that lead to that partition and their summed-up weight.
     */
    public MinCut ComputeMinCut(Graph<TVertex, int> g) 
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

    // we do a two-pass algorithm to reconstruct the sub-graphs:
    // - first we distribute the vertices into their respective graph
    // - second we align edges: if they cross graphs they will end in the list, otherwise in the respective graph
    private MinCut ConstructMinCutResult(Graph<TVertex, int> originalGraph, HashSet<int>? partition) 
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

        // we need to dedupe the edges for directed graphs, so we don't double-count the weights
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

        return new MinCut(first, second, cutWeight);
    }

    // bascially we're copying the whole graph into a new one, we leave the vertex "t" out of it (including the edge between "s" and "t")
    // and merge all other edges that "s" and "t" pointed together towards by summing their weight.
    // there might be left-over edges pointing from "t" but not through "s", we have to attach them to "s" as well.
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

            // on hitting "s" we are checking for the summation case (similar edge coming from "t"), otherwise just copy it over
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

        // for all edges from "t" that we haven't transferred to "s" yet, but do not go towards "s"
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

    /**
     * This iterates the given graph starting from "start" in a maximum adjacency fashion.
     * That means, it will always connect to the next vertex whose inlinks are having the largest edge weight sum.
     * This ordering implicitly gives us a "cut of the phase", as the last two added vertices are the ones with least inlink weights.
     */
    CutOfThePhase MaximumAdjacencySearch(Graph<TVertex, int> g, int? start)
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

            foreach (var next in candidates)
            {
                // compute the inlink weight sum from all vertices in "maxAdjacencyOrderedList" towards vertex "next"
                int weightSum = 0;

                foreach (var s in maxAdjacencyOrderedList)
                {
                    var edge = g.GetEdge(next, s);

                    if (edge != null)
                    {
                        weightSum += edge.Value;
                    }
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

        // we take the last two vertices in that list and their weight as a cut of the phase
        var n = maxAdjacencyOrderedList.Count;

        return new CutOfThePhase
        {
            S = maxAdjacencyOrderedList.ElementAt(n - 2),
            T = maxAdjacencyOrderedList.ElementAt(n - 1),
            Weight = cutWeight.ElementAt(cutWeight.Count - 1)
        };
    }
}