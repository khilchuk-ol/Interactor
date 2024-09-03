using Microsoft.AspNetCore.CookiePolicy;
using SplitDivider.Application.Common.Interfaces;

namespace SplitDivider.Infrastructure.Services;

public class FilePerformanceTracker : IPerformanceTracker
{
    private const string _fileName = "performance_log.txt";
    
    public void TrackPerformance(string operation, long timeMs, List<string> data)
    {
        using (var sw = new StreamWriter(@"Logs/" + _fileName, true))
        {
            sw.WriteLine($"{DateTime.Now}: {operation} performed in {timeMs}ms ({string.Join(", ", data)})");
        }
    }
}