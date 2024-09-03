namespace SplitDivider.Application.Common.Interfaces;

public interface IPerformanceTracker
{
    public void TrackPerformance(string operation, long timeMs, List<string> data);
}