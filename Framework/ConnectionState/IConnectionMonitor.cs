namespace Framework.ConnectionState
{
    public interface IConnectionMonitor
    {
        event ConnectionStatusChangedEventHandler ConnectionStatusChangedEvent;
        bool IsConnected { get; }
    }
}