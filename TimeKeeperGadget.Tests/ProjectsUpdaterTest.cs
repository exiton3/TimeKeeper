using Framework;
using Framework.ConnectionState;
using Framework.Repository.Abstractions;
using Moq;

namespace TimeKeeperGadget.Tests
{
    using NUnit.Framework;

    [TestFixture]
    public class ProjectsUpdaterTest
    {
        private Mock<IProjectRepository> projectRepository;
        private Mock<IActivityRepository> activityRepository;
        private Mock<IConnectionMonitor> connectionMonitor;

        [SetUp]
        public void Setup()
        {
            projectRepository = new Mock<IProjectRepository>();
            activityRepository = new Mock<IActivityRepository>();
            connectionMonitor  = new Mock<IConnectionMonitor>();
        }

        [Test]
        public void CanUpdateDateFromServer()
        {
            //Arange
            DataUpdater dataUpdater = new DataUpdater(projectRepository.Object,activityRepository.Object,connectionMonitor.Object,null);

            //Act
            dataUpdater.Update();
            //Assert
            
        }
    }
}
