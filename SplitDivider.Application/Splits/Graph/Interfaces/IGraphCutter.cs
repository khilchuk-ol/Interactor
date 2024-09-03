using SplitDivider.Application.Splits.Graph.Common;

namespace SplitDivider.Application.Splits.Graph.Interfaces;

public interface IGraphCutter
{
    public (List<int> first, List<int> second) CutSplitGraph(SplitGraphDto graphDto);

    public string GetName();
}