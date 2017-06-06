namespace TimeKeeperGadget.Tests.ViewModelTests
{
    using System.Collections.ObjectModel;
    using Framework.Model;
    using Framework.Repository.Abstractions;
    using Moq;
    using NUnit.Framework;
    using TimeKeeper.ViewModel;

    [TestFixture]
    public class MainViewModelTest
    {
        private Mock<IProjectRepository> projectRepository;
        private Mock<IAppSettingsRepository> appSetRepository;
        MainViewModel mainViewModel;

        [SetUp]
        public void SetUp()
        {
            projectRepository = new Mock<IProjectRepository>();
            appSetRepository =new Mock<IAppSettingsRepository>();
            projectRepository.Setup(x => x.GetAll()).Returns(
                 new ObservableCollection<Project> { new Project { Id = 1, Name = "Project 1 " } });
            appSetRepository.Setup(x => x.Get()).Returns(new AppSettings
                                                             {
                                                                 CurrentProjects =
                                                                     new ObservableCollection<Project>{ new Project {Id = 1, Name = "Project 1 "}}
                                                             });
            mainViewModel = new MainViewModel(projectRepository.Object, null,appSetRepository.Object);


        }
        [Test]
        public void CanStartTimerCommand()
        {
            //Arange

            //Act
            mainViewModel.StartTimer.Execute(new Project { Id = 1, Name = "Project1", Activity = new Activity{ Id = 1,Name = "Acivity 1"}});
            //Assert
            Assert.IsFalse(mainViewModel.Timer.IsTimerStopped);
            Assert.That(mainViewModel.Caption, Is.EqualTo("Stop"));
        }

        [Test]
        public void CanGetProjects()
        {
            //Arange

            //Act
            var projects = mainViewModel.Projects;

            //Assert
            Assert.That(projects, Is.Not.Null);
            Assert.That(projects.Count, Is.EqualTo(1));
        }
    }
}