using SplitDivider.Application.Splits.Graph.Algorithms.Common;

namespace SplitDivider.Application.Splits.Graph.Common;

public class SplitGraphDto
{
    public Dictionary<int, int> Connections { get; set; }
    
    public Graph<int, int> Graph { get; set; }
}