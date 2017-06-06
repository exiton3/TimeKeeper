using System;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using Framework.ConnectionState;
using Framework.Model;
using Framework.Repository.Abstractions;

namespace Framework
{
    using Common;
    using Common.Security;
    using TimeKeeperServiceReference;

    public class DataUpdater
    {
        private readonly IProjectRepository projectRepository;
        private readonly IActivityRepository activityRepository;
        private readonly IConnectionMonitor connectionMonitor;
        private readonly DispatcherTimer timer = new DispatcherTimer();
        private ObservableCollection<Project> projects;
        private ObservableCollection<Activity> activities;
        private readonly IEncryptor encryptor;
        public delegate void RefreshHandler();
        public delegate void ShowErrorHandler();
        public RefreshHandler RefreshData;
        public ShowErrorHandler ShowError;
        private string serviceError = null;
        private bool isConnected = false;

        public DataUpdater(IProjectRepository projectRepository, IActivityRepository activityRepository, IConnectionMonitor connectionMonitor, IEncryptor encryptor)
        {
            this.projectRepository = projectRepository;
            this.encryptor = encryptor;
            this.connectionMonitor = connectionMonitor;
            this.activityRepository = activityRepository;

            SetUpTimer();

            this.connectionMonitor.ConnectionStatusChangedEvent += ConnectionMonitorConnectionStatusChangedEvent;
            IsConnected = this.connectionMonitor.IsConnected;
        }

        public bool IsConnected
        {
            get { return isConnected; }
            set { isConnected = value; }
        }

        public string ServiceError
        {
            get { return serviceError; }
            set { serviceError = value; }
        }

        private void SetUpTimer()
        {
            timer.Interval = TimeSpan.FromMinutes(10);
            timer.Tick += TimerTick;
        }

        void ConnectionMonitorConnectionStatusChangedEvent(bool isConnected)
        {
            if (isConnected)
            {
                StartTimer();
            }
            else
            {
                StopTimer();
            }
        }

        private void StopTimer()
        {
            timer.Stop();
        }

        public void StartTimer()
        {
            timer.Start();
        }

        void TimerTick(object sender, EventArgs e)
        {
            Update();
        }

        public void Update()
        {
            //Get data from server
            //   GetFakeData();
            GetData();
            //Save data in local store
        }

        private void SaveDataInLocal()
        {
            projectRepository.Save(projects);
            activityRepository.Save(activities);
        }


        private string GetUserName()
        {
            return encryptor.Encode(Environment.UserName);
            //return encryptor.Encode("amdar");
        }
        private void GetData()
        {
            var client = new TimekeeperPortClient();
            client.getProjectsCompleted += ClientGetProjectsCompleted;
            client.getProjectsAsync(GetUserName());

        }

        void ClientGetProjectsCompleted(object sender, getProjectsCompletedEventArgs e)
        {
            projects = new ObservableCollection<Project>();
            activities = new ObservableCollection<Activity>();
            try
            {

                if (e.Error == null)
                {
                    var response = e.Result;
                    if (response != null)
                    {
                        var jsonSerializer = new JsonSerializer();

                        GetProjectsData(response, jsonSerializer);

                        GetActivitiesData(response, jsonSerializer);

                        SaveDataInLocal();
                        if (RefreshData != null)
                        {
                            RefreshData();
                        }
                    }
                    ServiceError = null;
                }
                else
                {
                    ServiceError = e.Error.Message;
                }
            }
            catch (Exception ex)
            {
                ServiceError = ex.Message;
            }
            if (ShowError != null)
            {
                ShowError();
            }
        }

        private void GetActivitiesData(TimekeeperSOAP response, JsonSerializer jsonSerializer)
        {
            var activityMapper = new ObjectMapper<Activity>(encryptor);

            var activitiesDto = jsonSerializer.Deserialize(response.activities);
            var activitesMap = activityMapper.MapCollection(activitiesDto);
            activities = new ObservableCollection<Activity>(activitesMap);
        }

        private void GetProjectsData(TimekeeperSOAP response, JsonSerializer jsonSerializer)
        {
            var projectMapper = new ObjectMapper<Project>(encryptor);

            var projectsDto = jsonSerializer.Deserialize(response.user_projects);
            var projectsm = projectMapper.MapCollection(projectsDto);

            projects = new ObservableCollection<Project>(projectsm);
        }

        #region Fake data
        private void GetFakeData()
        {
            FakeActivity();
            FakeProjects();
        }
        void FakeActivity()
        {
            activities = new ObservableCollection<Activity>();
            for (int i = 0; i < 5; i++)
            {
                activities.Add(new Activity { Id = i + 1, Name = "Activity " + (i + 1) });
            }
        }
        private void FakeProjects()
        {
            projects = new ObservableCollection<Project>();
            for (int i = 0; i < 10; i++)
            {
                projects.Add(new Project
                                 {
                                     Id = i + 1,
                                     Name = string.Format("Project {0}", (i + 1)),
                                     Activity = activities[0]
                                 }
                    );
            }

        }

        #endregion

    }
}