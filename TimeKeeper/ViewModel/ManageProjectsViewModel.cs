using GalaSoft.MvvmLight;

namespace TimeKeeper.ViewModel
{
    using System.Collections.ObjectModel;
    using System.Threading;
    using System.Windows;
    using Framework;
    using Framework.Model;
    using Framework.Repository.Abstractions;
    using GalaSoft.MvvmLight.Command;
    using Model;


    public class ManageProjectsViewModel : ViewModelBase
    {
        private ProjectsCollection selectedProjects;

        private ObservableCollection<Project> allProjects;
        private ObservableCollection<Activity> activities;
        private readonly IProjectRepository projectRepository;
        private readonly IActivityRepository activityRepository;

        #region Design Time data

        private static ObservableCollection<Activity> observableCollectionActivities = new ObservableCollection<Activity>
                                                                                           {
                                                                                               new Activity {Id = 1, Name = "Activity 1"},
                                                                                               new Activity {Id = 2, Name = "Activity 2"},
                                                                                               new Activity {Id = 3, Name = "Activity 3"}
                                                                                           };


        ObservableCollection<Project> projects = new ObservableCollection<Project>
                                                     {
                                                         new Project{Id = 1,Name = "Project 1",Activity = observableCollectionActivities[1]},
                                                         new Project{Id = 2,Name = "Project 2",Activity = observableCollectionActivities[0]},
                                                         new Project{Id = 3,Name = "Project 3",Activity = observableCollectionActivities[2]}
                                                     };

        #endregion

        public ProjectsCollection SelectedProjects
        {
            get { return selectedProjects; }

            set
            {
                selectedProjects = value;
                RaisePropertyChanged("SelectedProjects");
            }
        }


        public ObservableCollection<Project> AllProjects
        {
            get { return allProjects; }

            set
            {
                allProjects = value;
                RaisePropertyChanged("AllProjects");
            }
        }

        public ObservableCollection<Activity> Activities
        {
            get { return activities; }
            set { activities = value; RaisePropertyChanged("Activities"); }
        }

        public RelayCommand<Project> AddProject { get; private set; }

        public RelayCommand RefreshProject { get; private set; }

        public RelayCommand<Project> DeleteProject { get; private set; }

        /// <summary>
        /// Initializes a new instance of the ManageProjectsViewModel class.
        /// </summary>

        public ManageProjectsViewModel(IProjectRepository projectRepository, IActivityRepository activityRepository)
        {
            this.projectRepository = projectRepository;
            this.activityRepository = activityRepository;

            if (IsInDesignMode)
            {
                GetDesignTimeData();
            }
            else
            {
                GetRunTimeData();
                SelectedProjects = new ProjectsCollection();
            }
            AddProject = new RelayCommand<Project>(ProjectAdded);
            RefreshProject = new RelayCommand(ProjectRefreshed);
            DeleteProject = new RelayCommand<Project>(ProjectDeleted);

        }

        private void GetRunTimeData()
        {
            //Activities = observableCollectionActivities;
            //AllProjects = projects;
            Activities = this.activityRepository.GetAll();
            AllProjects = this.projectRepository.GetAll();
            if (Activities != null && AllProjects != null && Activities.Count > 0)
            {

                foreach (var project in AllProjects)
                {
                    project.Activity = Activities[0];
                }
            }
        }

        private void GetDesignTimeData()
        {
            AllProjects = projects;
            Activities = observableCollectionActivities;
            SelectedProjects = new ProjectsCollection(projects);
            SelectedProjects.Add(projects[0]);
        }

        private void ProjectDeleted(Project project)
        {
            SelectedProjects.Remove(project);
        }

        private void ProjectAdded(Project project)
        {
            if (!SelectedProjects.IsExist(project) && project.Activity != null)
            {
                SelectedProjects.Add(new Project { Id = project.Id, Name = project.Name, Activity = project.Activity });
            }
        }

        private void ProjectRefreshed()
        {
            App.DataUpdater.RefreshData += RefreshData;
            App.DataUpdater.Update();

        }

        private void RefreshData()
        {
            GetRunTimeData();
            var selected = new ProjectsCollection();
            foreach (var oldProject in SelectedProjects)
            {
                //Check if New Project List contains Selected Project
                //Check if New Activity List contains Selected Project Activity
                if (AllProjects.Contains(oldProject) && Activities.Contains(oldProject.Activity))
                {
                    //Get Project from the server(In case Name or other data were changed)
                    var newProject = AllProjects[AllProjects.IndexOf(oldProject)];
                    //Add Project to selected List only if Project and Activity are still available
                    selected.Add(newProject);
                }
            }
            
            SelectedProjects = selected;
        }

        ////public override void Cleanup()
        ////{
        ////    // Clean own resources if needed

        ////    base.Cleanup();
        ////}
    }
}