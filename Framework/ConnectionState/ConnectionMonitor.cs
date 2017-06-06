namespace Framework.ConnectionState
{
    using System;
    using System.Net.NetworkInformation;

    public delegate void ConnectionStatusChangedEventHandler(bool isConnected);

    public class ConnectionMonitor : IConnectionMonitor
    {
        private readonly IConnectionStateStrategy connectionStateStrategy;

        public event ConnectionStatusChangedEventHandler ConnectionStatusChangedEvent;

        public bool IsConnected { get; private set; }


        public string Address { get; set; }


        public ConnectionMonitor(string url, IConnectionStateStrategy connectionStateStrategy)
        {
            this.connectionStateStrategy = connectionStateStrategy;
            Address = url;
            IsConnected = NetworkInterface.GetIsNetworkAvailable();
            UpdateStatus();
            NetworkChange.NetworkAddressChanged += NetworkChangeNetworkAddressChanged;
            NetworkChange.NetworkAvailabilityChanged += NetworkChangeNetworkAvailabilityChanged;
        }


        public void UpdateStatus()
        {
            if (Address == null)
            {
                throw new NullReferenceException("Address was null please set url");
            }
            bool isAlive = connectionStateStrategy.IsAlive(Address);
            IsConnected = isAlive;

        }


        void NetworkChangeNetworkAvailabilityChanged(object sender, NetworkAvailabilityEventArgs e)
        {
            IsConnected = e.IsAvailable;
            UpdateStatus();
            ConnectionStatusChangedEventHandler handler = ConnectionStatusChangedEvent;
            if (handler != null) handler(IsConnected);
        }

        private void NetworkChangeNetworkAddressChanged(object sender, EventArgs e)
        {
            IsConnected = NetworkInterface.GetIsNetworkAvailable();
            UpdateStatus();
            ConnectionStatusChangedEventHandler handler = ConnectionStatusChangedEvent;
            if (handler != null) handler(IsConnected);
        }
    }
}