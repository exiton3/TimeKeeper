namespace TimeKeeperGadget.Tests.Offline
{
    using Framework.ConnectionState;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class ConnectionMonitorTest
    {
        private Mock<IConnectionStateStrategy> connectionStrategy;
        private ConnectionMonitor connectionMonitor;
        const string Url = @"http://127.0.0.1";

        [SetUp]
        public void SetUp()
        {

            connectionStrategy = new Mock<IConnectionStateStrategy>();
            connectionStrategy.Setup(x => x.IsAlive(Url)).Returns(false);
            connectionMonitor = new ConnectionMonitor(Url, connectionStrategy.Object);
        }
        [Test]
        public void ConnectionStateShouldBeFalseIfNoConnectionWithUrl()
        {
            //Arange

            //Act

            //Assert
            Assert.IsFalse(connectionMonitor.IsConnected);
        }
        [Ignore]
        [Test]
        public void ConnectionStateShoudBeTrueIfNoConnectionWithServer()
        {
            //Arange
            connectionStrategy.Setup(x => x.IsAlive(Url)).Returns(true);

            //Act
            connectionMonitor.UpdateStatus();
            //Assert
            Assert.IsTrue(connectionMonitor.IsConnected);

        }
    }
}