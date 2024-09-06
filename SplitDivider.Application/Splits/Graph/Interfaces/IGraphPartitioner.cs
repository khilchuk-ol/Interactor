using SplitDivider.Application.Splits.Graph.Common;

namespace SplitDivider.Application.Splits.Graph.Interfaces;

public interface IGraphPartitioner
{
    public (List<int> first, List<int> second) PartitionSplitGraph(SplitGraphDto graphDto);

    public string GetName();
}