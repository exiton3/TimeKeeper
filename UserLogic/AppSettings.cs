using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.IO;
using Microsoft.Win32;
using UserLogic.Domain;

namespace UserLogic
{
  
    //Change this class and new Class Serializing settings
    [Serializable]
    public class AppSettings : ICloneable
    {
        int userId;
        bool addToAutRun;        
        private string defaultTime;
        private Projects currProjects;

        [NonSerialized]
        string fname;
        [NonSerialized]
        public static string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\TimeKeeper";
        string userName;
        [NonSerialized]
        private const string timeKeeperName = "TimeKeeper";

        #region Constructors
        public AppSettings()
        { }
        public AppSettings(string fName)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            DefaultStopTime = new TimeSpan(17,30,0).ToString();
            fname = fName;
        }
        #endregion

        #region Properties
       
        public string DefaultStopTime
        {
            get { return defaultTime;}
            set { defaultTime = value;}
        }

        public Projects CurrentProjects
        {
            get
            {
                return currProjects;
            }
            set{ currProjects = value;}
        }

      
        public bool IsAutorun
        {
            get { return addToAutRun; }
            set { addToAutRun = value; }
        }

        public string FileName
        {
            get { return fname; }
            set { fname = value; }
        }

        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        public int UserId
        {
            get { return userId; }
            set { userId = value; }
        }
      
        #endregion
        
        #region Add and Delete from autorun

        public void AddToAutoRun(string flname)
        {
            RegistryKey start = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
            // if (Start.GetValue("Application", flname))
            // { }
            // else
            
                if (start != null) start.SetValue(timeKeeperName, flname, RegistryValueKind.String);
         
        }

        public void DeleteFromAutoRun()
        {
            RegistryKey start = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
            if (start != null) start.DeleteValue(timeKeeperName);
        }

        #endregion

        #region Serialize and Desirealize
        public static bool DeserializeSettings(string filename, ref AppSettings appSet)
        {
            FileInfo fl = new FileInfo(filename);
            AppSettings apSet;
            if (fl.Exists)
            {
                try
                {
   
                    XmlSerializer xmlser = new XmlSerializer(typeof(AppSettings));
                   
                    FileStream filestream = new FileStream(filename, FileMode.Open);
                
                    apSet = (AppSettings)xmlser.Deserialize(filestream);
                    appSet = (AppSettings)apSet.Clone();
                    filestream.Close();
                    return true;
                }
                catch (Exception ex)
                {

                    throw new SerializationException();
                    
                }
            }
            else
            {
                return false;
            }
        }

        public static void SerializeSettings(string filename, AppSettings appSet)
        {
            try
            {
                
                XmlSerializer xmlser = new XmlSerializer(typeof(AppSettings));
                
                FileStream filestream = new FileStream(filename, FileMode.Create);
                
                xmlser.Serialize(filestream, appSet);
                
                filestream.Close();
            }
            catch (Exception ex)
            {
                throw  new SerializationException();
            }

        }
        
        #endregion

        #region ICloneable Members

        public object Clone()
        {
            try
            {
                AppSettings apNew = new AppSettings();
                apNew.FileName = FileName;
                apNew.IsAutorun = IsAutorun;                
                apNew.UserName = UserName;
                apNew.UserId = UserId;
                apNew.DefaultStopTime = DefaultStopTime;
                apNew.CurrentProjects = CurrentProjects;
                return apNew;
            }
            catch
            {
                return null;
            }
        }

        #endregion
    }
}
