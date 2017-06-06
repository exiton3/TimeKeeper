namespace Framework.ConnectionState
{
    public interface IConnectionStateStrategy
    {
        bool IsAlive(string hostnameOrAddress);
    }
}