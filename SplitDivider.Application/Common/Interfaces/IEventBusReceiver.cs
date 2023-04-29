namespace SplitDivider.Application.Common.Interfaces;

public interface IEventBusReceiver : IDisposable
{
    public void StartReceiving();
}