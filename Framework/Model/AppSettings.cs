using System;
using System.Collections.ObjectModel;
using Microsoft.Win32;

namespace Framework.Model
{
    using System.Configuration;

    [Serializable]
    public class AppSettings
    {
        private string defaultStopTime = ConfigurationManager.AppSettings["defaultStopTime"];
        private bool isAutoStart = true;
        private RegistryKey registryKey;
        private bool isTopMost;

        public AppSettings()
        {
            registryKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
        }

        [NonSerialized]
        private const string TimeKeeperName = "TimeKeeper";

        public string Url { get; set; }

        public string DefaultStopTime
        {
            get { return defaultStopTime; }
            set { defaultStopTime = value; }
        }

        public bool IsAutoStart
        {
            get { return isAutoStart; }
            set
            {
                isAutoStart = value;
            }
        }

        public bool IsTopMost
        {
            get { return isTopMost; }
            set { isTopMost = value; }
        }
        public ObservableCollection<Project> CurrentProjects { get; set; }

        #region Add and Delete from autorun

        public void AddToAutoRun(string fileName)
        {
            if (registryKey != null)
            {
                registryKey.SetValue(TimeKeeperName, fileName, RegistryValueKind.String);
                registryKey.Close();
            }


        }

        public void DeleteFromAutoRun()
        {
            if (registryKey != null)
            {
                if (registryKey.GetValue(TimeKeeperName) != null)
                {
                    registryKey.DeleteValue(TimeKeeperName);

                }
                registryKey.Close();
            }
        }

        #endregion
    }
}