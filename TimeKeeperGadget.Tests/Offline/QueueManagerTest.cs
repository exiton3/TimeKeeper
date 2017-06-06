namespace TimeKeeperGadget.Tests.Offline
{
    using Framework.ConnectionState;
    using Framework.Dispatcher;
    using Framework.QueueManagment;
    using Framework.Repository;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class QueueManagerTest
    {
        Mock<IRequestDispatcher> requestDispatcher;
        private Mock<IConnectionMonitor> connectionManager;
        private Mock<IQueueRepository<string>> queueRepository;
        [SetUp]
        public void SetUp()
        {
            requestDispatcher = new Mock<IRequestDispatcher>();
            connectionManager = new Mock<IConnectionMonitor>();
            queueRepository = new Mock<IQueueRepository<string>>();
        }
        [Test]
        public void ToInstanceShoudBeTheSame()
        {
            //Arange

            //Act
            var queue1 = QueueManager<string>.Instance;
            var queue2 = QueueManager<string>.Instance;
            //Assert
            Assert.That(queue1.Equals(queue2));
            Assert.That(queue1.PendingData.Equals(queue2.PendingData));
        }

        [Test]
        public void CanAddDataInQueue()
        {
            //Arange
            var queue = QueueManager<string>.Instance;
            //Act
            queue.PendingData.Enqueue("message1");
            //Assert

        }

        [Test]
        public void TwoQueueShoudBeTheSame()
        {
            //Arange
            var queue1 = QueueManager<string>.Instance;
            var queue2 = QueueManager<string>.Instance;
            //Act
            queue1.PendingData.Enqueue("message1");
            //Assert
            Assert.That(queue1.PendingData.Count == queue2.PendingData.Count);
           
            Assert.That(queue1.PendingData.Peek(), Is.EqualTo(queue2.PendingData.Peek()));
        }

        [Test]
        public void CanSetNewQueueAndTwoInstanceShoudBeTheSame()
        {
            //Arange
            var queue1 = QueueManager<string>.Instance;
            var queue2 = QueueManager<string>.Instance;
            //Act
            queue1.PendingData.Enqueue("message1");
            //Assert
            Assert.That(queue1.PendingData.Peek(), Is.EqualTo(queue2.PendingData.Peek()));
            queue2.Initialize(new ObservableQueue<string>(), requestDispatcher.Object,connectionManager.Object, queueRepository.Object);
            queue2.PendingData.Enqueue("message 2");
            Assert.That(queue1.PendingData.Peek(), Is.EqualTo(queue2.PendingData.Peek()));
           
        }
        [Test]
        public void CanHandleItemAddedEventInQueue()
        {
            //Arange
            var queue = QueueManager<string>.Instance;
            bool eventIsHandled = false;
            queue.PendingData.ItemAddedEvent += delegate
                                                    {
                                                        eventIsHandled = true;
                                                    };
            //Act
            queue.PendingData.Enqueue("Message added in queue!");
            //Assert

            Assert.IsTrue(eventIsHandled);
        }

        [Test]
        public void CanInitializeQueueManager()
        {
            //Arange
            var queueManager = QueueManager<string>.Instance;
            var queue = new ObservableQueue<string>();
            //Act
            queueManager.Initialize(queue, requestDispatcher.Object, connectionManager.Object, queueRepository.Object);

            //Assert
            Assert.IsNotNull(queueManager.PendingData);
            Assert.IsNotNull(queueManager.ConnectionMonitor);
        }
    }
}