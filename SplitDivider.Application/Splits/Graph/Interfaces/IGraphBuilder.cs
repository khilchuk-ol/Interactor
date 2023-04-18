using SplitDivider.Application.Splits.Graph.Common;

namespace SplitDivider.Application.Splits.Graph.Interfaces;

public interface IGraphBuilder
{
    public SplitGraphDto BuildGraph(Split split, List<int> userIds);
}