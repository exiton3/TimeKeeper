using System;
using System.Windows;
using UserLogic;
using UserLogic.Domain;

namespace TimeKeeperGadget
{
    /// <summary>
    /// Interaction logic for GadgetWindow.xaml
    /// </summary>
    public partial class GadgetWindow : Window
    {
        private readonly TimerManger timerManger;
       
        private Projects currentProjects = new Projects();
        AppSettings appSettings = new AppSettings(FilenameSettings);
        readonly string fName = AppSettings.path + "\\LogProject.xml";
        private readonly TimeKeeper timeKeeper;
        private static readonly string FilenameSettings = AppSettings.path + "\\appsettings.xml";

        private TimeSpan defaultTime;
        public GadgetWindow()
        {
            InitializeComponent();
         
            timeKeeper = new TimeKeeper(new ProjectLogXmlRepository(fName), new LogSender());
            timerManger = new TimerManger();                           
           
           
           timerTextBlock.DataContext = timerManger;
           

            Topmost = true;
            Top = SystemParameters.WorkArea.Height - Height - 5;
            Left = SystemParameters.PrimaryScreenWidth - Width - 5;
                      
        }

        #region Load and closing window
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            #region Hide from Alt+Tab
          //  var handle = HiderAltTab.GetWindowHandle(this);
           // HiderAltTab.HideFromAltTab(handle); 
            #endregion
            defaultTime = new TimeSpan(17,30,0);

            AppSettings.DeserializeSettings(FilenameSettings, ref appSettings);

            if (appSettings != null)
            {
                currentProjects = appSettings.CurrentProjects;
                if (appSettings.DefaultStopTime.Length != 0)
                {
                    defaultTime = TimeSpan.Parse(appSettings.DefaultStopTime);
                }
                              
            }
            if (currentProjects == null)
            {
                currentProjects = new Projects();
            }
            projectcomboBox.ItemsSource = currentProjects;
            VerifyNumberOfProjects();      
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var assemblyName = System.Reflection.Assembly.GetExecutingAssembly().Location;
           
            appSettings.AddToAutoRun(assemblyName);
            if (timerManger.IsStartTimer)
            {
                AddProjectLog();
            }
        }
        
        #endregion

        private void TimeButton_Click(object sender, RoutedEventArgs e)
        {
            if (timerManger.IsStartTimer)
            {
                timerManger.StopTimer();
                timerButton.Content = "Start";
                projectcomboBox.IsEnabled = true;

                AddProjectLog();
            }
            else
            {               
                timerManger.StartTimer();
                timerButton.Content = "Stop";
                projectcomboBox.IsEnabled = false;
            }
            
        }

        #region  functions
        private void VerifyNumberOfProjects()
        {
            if (currentProjects.Count == 0)
            {
                projectcomboBox.IsEnabled = false;
                timerButton.IsEnabled = false;
                ToolTip = "Right click Enter in options and add projects!";
            }
            else
            {
                projectcomboBox.IsEnabled = true;
                timerButton.IsEnabled = true;
            }
        }
        private void AddProjectLog()
        {
            if (projectcomboBox.SelectedIndex > 0)
            {
                var project = ((Projects)projectcomboBox.ItemsSource)[projectcomboBox.SelectedIndex];
                timeKeeper.AddLogEntry(new ProjectLog
                                           {
                                               UserId = Environment.UserName,
                                               DurationTime = TimeSpan.Parse(timerManger.DurationTime),
                                               ProjectId = project.Id,
                                               StartDate = DateTime.Today
                                           });
            }
        }

        #endregion

        #region Context menu events
        private void SettingsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var settingsWindow = new SettingsWindow(currentProjects);
            settingsWindow.Owner = this;                        
            settingsWindow.DefaultTime = defaultTime;

            settingsWindow.ShowDialog();
            if (settingsWindow.DialogResult.HasValue && settingsWindow.DialogResult.Value)
            {
                appSettings.CurrentProjects = currentProjects;
                defaultTime = settingsWindow.DefaultTime;
                appSettings.DefaultStopTime = defaultTime.ToString();
                AppSettings.SerializeSettings(FilenameSettings, appSettings);
                GC.Collect();
             VerifyNumberOfProjects();
                
            }
           
        }

        private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Close();
        } 
        #endregion

       
      
    }
}
