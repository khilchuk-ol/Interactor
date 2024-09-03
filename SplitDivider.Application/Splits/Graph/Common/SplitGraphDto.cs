using SplitDivider.Application.Splits.Graph.Algorithms.Common;

namespace SplitDivider.Application.Splits.Graph.Common;

public class SplitGraphDto
{
    public IDictionary<int, int> Connections { get; set; }
    
    public Graph<int, int> Graph { get; set; }
}