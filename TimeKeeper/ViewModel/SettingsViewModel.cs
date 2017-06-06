namespace TimeKeeper.ViewModel
{
    using System;
    using System.Configuration;
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;

    public class SettingsViewModel : ViewModelBase
    {
        private bool autoStart = true;

        private string defaultStopTime = ConfigurationManager.AppSettings["defaultStopTime"];

        private string urlAddres;
        private bool isTopMost;

        #region Properties
        public string UserName
        {
            get { return Environment.UserName; }
        }

        public string DefaultStopTime
        {
            get { return defaultStopTime; }
            set
            {
                if (defaultStopTime != value)
                {
                    defaultStopTime = value;
                    RaisePropertyChanged("DefaultStopTime");
                }
            }
        }

        public bool AutoStart
        {
            get { return autoStart; }
            set
            {
                if (value != autoStart)
                {
                    autoStart = value;
                    RaisePropertyChanged("AutoStart");
                }
            }
        }

        public bool IsTopMost
        {
            get { return isTopMost; }
            set
            {
                isTopMost = value;
                RaisePropertyChanged("IsTopMost");
            }
        }

        public string Url
        {
            get { return urlAddres; }
            set
            {
                if (urlAddres != value)
                {
                    urlAddres = value;
                    RaisePropertyChanged("Url");
                }
            }
        }

        #endregion

        public ManageProjectsViewModel ManageProjects { get; set; }

        public RelayCommand AutoStartCommand { get; private set; }

        public SettingsViewModel()
        {
            if (IsInDesignMode)
            {
                ManageProjects = new ManageProjectsViewModel(null,null);
            }
        }
    }
}