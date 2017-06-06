using GalaSoft.MvvmLight;

namespace TimeKeeper.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows;
    using Dialog;
    using Framework.Model;
    using Framework.QueueManagment;
    using Framework.Repository.Abstractions;
    using Framework.Repository.Implementations;
    using GalaSoft.MvvmLight.Command;
    using Model;
    using View;


    public class MainViewModel : ViewModelBase
    {
        private TimerManager timer;
        private ObservableCollection<Project> projects;
        private readonly IProjectRepository projectRepository;
        private readonly IModalDialogService modalDialogService;

        private readonly IAppSettingsRepository appSettingsRepository;

        private string caption = "Start";
        private Project currentProject;
        private readonly AppSettings appSettings;

        #region Properties

        public string Caption
        {
            get { return caption; }
            set
            {
                caption = value;
                RaisePropertyChanged("Caption");
            }
        }
        public TimerManager Timer
        {
            get { return timer; }
            set { timer = value; }
        }

        public ObservableCollection<Project> Projects
        {
            get { return projects; }
            set
            {
                projects = value;
                RaisePropertyChanged("Projects");
            }
        }

        #endregion

        #region Commands

        public RelayCommand Exit { get; private set; }

        public RelayCommand<Project> StartTimer { get; private set; }

        public RelayCommand Options { get; private set; }

        #endregion


        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(IProjectRepository projectRepository, IModalDialogService modalDialogService, IAppSettingsRepository appSettingsRepository)
        {
            timer = new TimerManager();
            timer.Stopped += TimerStopped;
            this.projectRepository = projectRepository;
            this.appSettingsRepository = appSettingsRepository;
            this.modalDialogService = modalDialogService;

            if (IsInDesignMode)
            {
                Projects = GetDesignData();
            }
            else
            {
                appSettings = this.appSettingsRepository.Get();
                ReadSettings(appSettings);
            }

            StartTimer = new RelayCommand<Project>(OnStartTimer);
            Exit = new RelayCommand(AppExit);
            Options = new RelayCommand(OpenSettings);


        }

        private void ReadSettings( AppSettings appSet)
        {
            if (appSet != null)
            {
                timer.DefaultStopTime = !string.IsNullOrEmpty(appSet.DefaultStopTime)
                                            ? TimeSpan.Parse(appSet.DefaultStopTime)
                                            : new TimeSpan(17, 30, 0);
                Projects = appSet.CurrentProjects ?? new ObservableCollection<Project>();
                Application.Current.MainWindow.Topmost = appSet.IsTopMost;
            }
            else
            {
                Projects = new ObservableCollection<Project>();
            }
        }


        private static ObservableCollection<Project> GetDesignData()
        {
            return new ObservableCollection<Project>(new List<Project>
                                                         {
                                                             new Project {Id = 1, Name = "Project 1"},
                                                             new Project {Id = 2, Name = "Project 2"},
                                                             new Project {Id = 3, Name = "Project 3"}
                                                         });
        }

        #region Command Handlers

        private void OpenSettings()
        {
            var settings = new SettingsView();
            var manageProjectsViewModel = new ManageProjectsViewModel(projectRepository, new ActivityRepository());
            manageProjectsViewModel.SelectedProjects = new ProjectsCollection(Projects);
            var settingsViewModel = new SettingsViewModel
                                        {
                                            DefaultStopTime = timer.DefaultStopTime.ToString(),
                                            ManageProjects = manageProjectsViewModel,
                                            IsTopMost = appSettings.IsTopMost,
                                            AutoStart = appSettings.IsAutoStart

                                        };
            modalDialogService.ShowDialog(settings, settingsViewModel
                                          , resultViewModal =>
                                                {
                                                    if (settings.DialogResult.HasValue && settings.DialogResult.Value)
                                                    {

                                                        Projects = resultViewModal.ManageProjects.SelectedProjects;
                                                        timer.DefaultStopTime = TimeSpan.Parse(resultViewModal.DefaultStopTime);
                                                        ApplySettings(resultViewModal);
                                                    }
                                                });

            settingsViewModel.Cleanup();
            GC.Collect();
        }

        private void ApplySettings(SettingsViewModel resultViewModal)
        {
            appSettings.CurrentProjects = Projects;
            appSettings.DefaultStopTime = timer.DefaultStopTime.ToString();
            appSettings.IsAutoStart = resultViewModal.AutoStart;
            appSettings.IsTopMost = resultViewModal.IsTopMost;

            Application.Current.MainWindow.Topmost = appSettings.IsTopMost;

            if (appSettings.IsAutoStart)
            {
                var assemblyName = System.Reflection.Assembly.GetExecutingAssembly().Location;
                appSettings.AddToAutoRun(assemblyName);
            }
            else
            {
                appSettings.DeleteFromAutoRun();
            }
            appSettingsRepository.Save(appSettings);
        }

        private void AppExit()
        {
            if (!timer.IsTimerStopped)
            {
                timer.StopTimer();
            }
            Application.Current.MainWindow.Close();
        }

        void TimerStopped(object sender, EventArgs e)
        {
            Caption = "Start";
            var activityMessage = GetActivityMessage(currentProject);

            QueueManager<ActivityMessage>.Instance.PendingData.Enqueue(activityMessage);
        }
        private void OnStartTimer(Project project)
        {

            if (project != null)
            {

                currentProject = project;

                if (!timer.IsTimerStopped)
                {
                    timer.StopTimer();
                }
                else
                {
                    Caption = "Stop";
                    timer.StartTimer();
                }
            }

        }

        private ActivityMessage GetActivityMessage(Project project)
        {
            return new ActivityMessage
                       {
                           ActionName = "Stop",
                           StartTime = Timer.StartTime,
                           StopTime = timer.StartTime + timer.ElapsedTime,
                           ProjectId = project.Id,
                           ActivityId = project.Activity.Id
                       };
        }

        #endregion

        ////public override void Cleanup()
        ////{
        ////    // Clean up if needed

        ////    base.Cleanup();
        ////}
    }
}