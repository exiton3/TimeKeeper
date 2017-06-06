namespace TimeKeeperGadget.Tests.ViewModelTests
{
    using System.Collections.ObjectModel;
    using Framework.Model;
    using Framework.Repository.Abstractions;
    using Moq;
    using NUnit.Framework;
    using TimeKeeper.Model;
    using TimeKeeper.ViewModel;

    [TestFixture]
    public class ManageProjectsViewModelTest
    {
        private Mock<IProjectRepository> projectRepository;
        private Mock<IActivityRepository> activityRepository;
        ObservableCollection<Project> projects;
        private ObservableCollection<Activity> activities;
        ManageProjectsViewModel manageProjectsViewModel;

        [SetUp]
        public void SetUp()
        {
            projectRepository = new Mock<IProjectRepository>();
            activityRepository = new Mock<IActivityRepository>();
            var activity = new Activity {Id = 1, Name = "Activity 1"};
            projects = new ObservableCollection<Project>
                           {
                               new Project{Id = 1,Name = "Project 1", Activity = activity},
                               new Project{Id = 2,Name = "Project 2", Activity = activity},
                               new Project{Id = 3,Name = "Project 3", Activity = activity}
                           };
            activities = new ObservableCollection<Activity>
                             {
                                 new Activity {Id = 1, Name = "Activity 1"},
                                 new Activity {Id = 1, Name = "Activity 2"},
                                 new Activity {Id = 1, Name = "Activity 3"}
                             };
            projectRepository.Setup(x => x.GetAll()).Returns(projects);

            activityRepository.Setup(x => x.GetAll()).Returns(activities);

            manageProjectsViewModel = new ManageProjectsViewModel(projectRepository.Object, activityRepository.Object);

        }

        [Test]
        public void CanAddProjectsIntoSelectedList()
        {

            //Act
            manageProjectsViewModel.AddProject.Execute(projects[1]);
            //Assert

            Assert.That(manageProjectsViewModel.SelectedProjects.Count, Is.EqualTo(1));
        }

        [Test]
        public void CouldNotAddTheSameProjectIntoSelectedList()
        {
            //Arange
            manageProjectsViewModel.SelectedProjects.Add(projects[0]);
            //Act
            manageProjectsViewModel.AddProject.Execute(projects[0]);
            //Assert
            Assert.That(manageProjectsViewModel.SelectedProjects.Count, Is.EqualTo(1));
            
        }
        [Test]
        public void CanDeleteProjectFromSelectedList()
        {
            //Arange
            manageProjectsViewModel.SelectedProjects = new ProjectsCollection(projects);
            //Act
            manageProjectsViewModel.DeleteProject.Execute(projects[1]);
            //Assert
            Assert.That(manageProjectsViewModel.SelectedProjects.Count, Is.EqualTo(2));

        }
    }
}