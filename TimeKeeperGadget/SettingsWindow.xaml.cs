using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using UserLogic;
using UserLogic.Domain;

namespace TimeKeeperGadget
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow
    {
        private readonly Projects projects;
        private Projects currentProjects;
        private int maxNumberProjects = 5;
        #region Properties for settings
        public Projects CurrentProjects
        {
            get
            {
                return currentProjects;
            }
            set
            {
                currentProjects = value;
            }
        }

        private TimeSpan defaultTime;
        public TimeSpan DefaultTime
        {
            get
            {
                return defaultTime;
            }
            set
            {
                defaultTime = value;
                timeTextBox.Text = String.Format("{0:d2}:{1:d2}", DefaultTime.Hours, DefaultTime.Minutes);
            }
        } 
        #endregion

        public SettingsWindow(Projects curProjects)
        {
            currentProjects = curProjects;

            InitializeComponent( );
            userNamelabel.Content = Environment.UserName;
            timeTextBox.Text = String.Format("{0:d2}:{1:d2}",DefaultTime.Hours,DefaultTime.Minutes);
            projects = new Projects();
            for (int i = 0; i < 10; i++)
            {
                projects.Add( new Project{Name = "Project " + (i+1),Id = (i+1).ToString()});
            }       
            projectsListBox.ItemsSource = projects;

            currentProjectslistBox.ItemsSource = currentProjects;
          
        }

        bool  ValidateTime(string time)
        {
            Regex timeRegex = new Regex(@"(^([0-9]|[0-1][0-9]|[2][0-3]):([0-5][0-9])$)");

            return timeRegex.IsMatch(time);
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateTime(timeTextBox.Text))
            {
                defaultTime = TimeSpan.Parse(timeTextBox.Text);
                DialogResult = true;
            }
            else
            {
                MessageBox.Show("Time is not valid format");
            }
            
        }

        private void AddButtonClick(object sender, RoutedEventArgs e)
        {
            if (projectsListBox.SelectedIndex >= 0)
            {

                Project selectedProject = ((Projects) projectsListBox.ItemsSource)[projectsListBox.SelectedIndex];

                if (currentProjects.Count < maxNumberProjects)
                {
                    if (currentProjects.IsExist(selectedProject) == false)
                    {
                        currentProjects.Add(new Project {Id = selectedProject.Id, Name = selectedProject.Name});
                    }
                    else
                    {
                        MessageBox.Show("Project " + selectedProject.Name + "already exist!");
                    }

                }
                else
                {
                    MessageBox.Show("Max number of selected projects must be 5");
                }
            }

        }

        private void DeleteButtonClick(object sender, RoutedEventArgs e)
        {
            if ((currentProjects.Count > 0)&&(currentProjectslistBox.SelectedIndex >=0))
            {
                currentProjects.RemoveAt(currentProjectslistBox.SelectedIndex);
            }
        }
    }
}
