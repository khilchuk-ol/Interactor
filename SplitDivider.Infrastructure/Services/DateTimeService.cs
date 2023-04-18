using SplitDivider.Application.Common.Interfaces;

namespace SplitDivider.Infrastructure.Services;

public class DateTimeService : IDateTime
{
    public DateTime Now => DateTime.Now;
}
