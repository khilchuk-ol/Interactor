using SplitDivider.Application.Splits.Graph.Algorithms.Common;
using System.Collections.Concurrent;

namespace SplitDivider.Application.Splits.Graph.Algorithms.MinCut
{
    public class PushRelabelMinCutImpl<TVertex> where TVertex : IComparable<TVertex>
    {
        private class FlowEdge
        {
            public int From { get; }
            public int To { get; }
            public int Capacity { get; set; }
            public int Flow { get; set; }

            public FlowEdge(int from, int to, int capacity)
            {
                From = from;
                To = to;
                Capacity = capacity;
                Flow = 0;
            }

            public int ResidualCapacityTo(int vertex)
            {
                return vertex == To ? Capacity - Flow : Flow;
            }

            public void AddResidualFlowTo(int vertex, int deltaFlow)
            {
                if (vertex == To)
                    Flow += deltaFlow;
                else
                    Flow -= deltaFlow;
            }
        }

        private readonly Graph<TVertex, int> _graph;
        private readonly Dictionary<int, List<FlowEdge>> _adjacencyList;
        private readonly int _source;
        private readonly int _sink;
        private readonly Dictionary<int, int> _height;
        private readonly Dictionary<int, int> _excess;
        private readonly ConcurrentDictionary<int, bool> _visited;

        private const double PENALTY_FACTOR = 0.3;
        private double partitionImbalancePenalty;

        public PushRelabelMinCutImpl(Graph<TVertex, int> graph, int source, int sink)
        {
            _graph = graph;
            _source = source;
            _sink = sink;
            _adjacencyList = new();
            _height = new();
            _excess = new();
            _visited = new();
            partitionImbalancePenalty = 0;

            foreach (var vertexId in graph.GetVerticesIds())
            {
                _adjacencyList[vertexId] = new List<FlowEdge>();
                _height.Add(vertexId, 0);
                _excess.Add(vertexId, 0);

                _visited[vertexId] = false;
            }

            foreach (var vertexId in graph.GetVerticesIds())
            {
                foreach (var edge in graph.GetEdges(vertexId))
                {
                    var adjustedCapacity = edge.Value + vertexId % 3;
                    var flowEdge = new FlowEdge(vertexId, edge.DestinationVertexId, adjustedCapacity);
                    _adjacencyList[vertexId].Add(flowEdge);
                    _adjacencyList[edge.DestinationVertexId].Add(flowEdge);
                }
            }
        }

        public MinCut<TVertex> ComputeMinCut()
        {
            _height[_source] = _graph.VerticesCount; 

            Parallel.ForEach(_adjacencyList[_source], edge =>
            {
                if (edge.From == _source)
                {
                    edge.Flow = edge.Capacity;
                    _excess[edge.To] = edge.Capacity;
                    _excess[_source] -= edge.Capacity;
                }
            });

            bool active;
            int iterationCount = 0;
            const int MAX_ITERATIONS = 1000;
            int forcedBalanceThreshold = (int)(_graph.VerticesCount * 0.1); // e.g., 10% imbalance tolerance

            do
            {
                active = false;
                iterationCount++;

                Parallel.ForEach(_graph.GetVerticesIds(), vertexId =>
                {
                    if (vertexId == _source || vertexId == _sink || _excess[vertexId] <= 0) return;

                    bool pushed = false;
                    foreach (var edge in _adjacencyList[vertexId])
                    {
                        int otherVertex = edge.From == vertexId ? edge.To : edge.From;
                        if (edge.ResidualCapacityTo(otherVertex) > 0 && _height[vertexId] > _height[otherVertex])
                        {
                            int deltaFlow = Math.Min(_excess[vertexId], edge.ResidualCapacityTo(otherVertex));
            
                            deltaFlow = AdjustFlowForBalance(deltaFlow, vertexId, otherVertex);
            
                            edge.AddResidualFlowTo(otherVertex, deltaFlow);
                            _excess[vertexId] -= deltaFlow;
                            _excess[otherVertex] += deltaFlow;
                            pushed = true;
                            active = true;
                            if (_excess[vertexId] == 0) break;
                        }
                    }

                    if (!pushed)
                    {
                        int minHeight = int.MaxValue;
                        foreach (var edge in _adjacencyList[vertexId])
                        {
                            int otherVertex = edge.From == vertexId ? edge.To : edge.From;
                            if (edge.ResidualCapacityTo(otherVertex) > 0)
                                minHeight = Math.Min(minHeight, _height[otherVertex]);
                        }

                        if (minHeight < int.MaxValue)
                        { 
                            _height[vertexId] = minHeight + 1;
                            active = true;
                        }
                    }
                });

                if (iterationCount % 10 == 0)
                {
                    ApplyBalancingPenalty(iterationCount);
                }

            } while (active && iterationCount < MAX_ITERATIONS);

            var firstPart = new Graph<TVertex, int>();
            var secondPart = new Graph<TVertex, int>();

            int partition1Size = 0, partition2Size = 0;

            HashSet<int> assignedVertices = new HashSet<int>();

            // Start DFS with forced balancing
            Dfs(_source, firstPart, secondPart, ref partition1Size, ref partition2Size, forcedBalanceThreshold, assignedVertices);

            // Ensure any unvisited vertices are assigned
            foreach (var vertexId in _graph.GetVerticesIds())
            {
                if (!assignedVertices.Contains(vertexId))
                {
                    if (partition1Size <= partition2Size)
                    {
                        firstPart.AddVertex(_graph.GetVertex(vertexId));
                        partition1Size++;
                    }
                    else
                    {
                        secondPart.AddVertex(_graph.GetVertex(vertexId));
                        partition2Size++;
                    }
                }
            }

            int cutWeight = _excess[_sink];
            return new MinCut<TVertex>(firstPart, secondPart, cutWeight);
        }

        private int AdjustFlowForBalance(int deltaFlow, int fromVertex, int toVertex)
        {
            int partition1Size = _visited.Count(x => x.Value);
            int partition2Size = _graph.VerticesCount - partition1Size;

            partitionImbalancePenalty = CalculateImbalancePenalty(partition1Size, partition2Size);

            if (Math.Abs(partition1Size - partition2Size) > _graph.VerticesCount * 0.2)
            {
                partitionImbalancePenalty += 0.2;
            }

            partitionImbalancePenalty = Math.Min(partitionImbalancePenalty, 1.0);

            if (partition1Size > partition2Size && _visited.ContainsKey(fromVertex) && !_visited.ContainsKey(toVertex))
            {
                deltaFlow = (int)(deltaFlow * (1 - partitionImbalancePenalty / 2));  
            }
            else if (partition2Size > partition1Size && !_visited.ContainsKey(fromVertex) && _visited.ContainsKey(toVertex))
            {
                deltaFlow = (int)(deltaFlow * (1 - partitionImbalancePenalty / 2));  
            }

            return deltaFlow;
        }

        private double CalculateImbalancePenalty(int partition1Size, int partition2Size)
        {
            int totalSize = partition1Size + partition2Size;
            double balanceFactor = Math.Abs((double)partition1Size / totalSize - 0.5);
            return balanceFactor * PENALTY_FACTOR * 2;
        }

        private void ApplyBalancingPenalty(int iterationCount)
        {
            int partition1Size = _visited.Count(x => x.Value);
            int partition2Size = _graph.VerticesCount - partition1Size;

            partitionImbalancePenalty = CalculateImbalancePenalty(partition1Size, partition2Size);

            if (iterationCount > 100)
            {
                partitionImbalancePenalty += 0.1 * (iterationCount / 100); 
                partitionImbalancePenalty = Math.Min(partitionImbalancePenalty, 1.0);
            }
        }

        private void Dfs(int vertex, Graph<TVertex, int> part1, Graph<TVertex, int> part2, ref int partition1Size, ref int partition2Size, int forcedBalanceThreshold, HashSet<int> assignedVertices)
        {
            if (_visited[vertex] || assignedVertices.Contains(vertex))
                return;

            bool forceToPart1 = partition1Size < partition2Size && (partition2Size - partition1Size) > forcedBalanceThreshold;
            bool forceToPart2 = partition2Size < partition1Size && (partition1Size - partition2Size) > forcedBalanceThreshold;

            if (forceToPart1)
            {
                _visited[vertex] = true;
                assignedVertices.Add(vertex);
                part1.AddVertex(_graph.GetVertex(vertex));
                partition1Size++;
            }
            else if (forceToPart2)
            {
                _visited[vertex] = true;
                assignedVertices.Add(vertex);
                part2.AddVertex(_graph.GetVertex(vertex));
                partition2Size++;
            }
            else
            {
                // If no forced balancing, place based on current sizes
                _visited[vertex] = true;

                if (partition1Size <= partition2Size)
                {
                    part1.AddVertex(_graph.GetVertex(vertex));
                    assignedVertices.Add(vertex);
                    partition1Size++;
                }
                else
                {
                    part2.AddVertex(_graph.GetVertex(vertex));
                    assignedVertices.Add(vertex);
                    partition2Size++;
                }
            }

            foreach (var edge in _adjacencyList[vertex])
            {
                int otherVertex = edge.From == vertex ? edge.To : edge.From;
                if (!_visited[otherVertex] && edge.ResidualCapacityTo(otherVertex) > 0)
                {
                    Dfs(otherVertex, part1, part2, ref partition1Size, ref partition2Size, forcedBalanceThreshold, assignedVertices);
                }
            }
        }
    }
}
