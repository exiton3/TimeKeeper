namespace Framework.Repository.Implementations
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Abstractions;
    using Model;

    public class ProjectRepository : Repository<Project>, IProjectRepository
    {

        private ObservableCollection<Project> FakeProjects()
        {
            var projects = new List<Project>();
            for (int i = 0; i < 10; i++)
            {
                projects.Add(new Project
                                 {
                                     Id = i + 1,
                                     Name = string.Format("Project {0}", (i + 1)),
                                     Activity = new Activity { Id = i + 1, Name = string.Format("Activity {0}", (i + 1)) }
                                 });
            }
            return new ObservableCollection<Project>(projects);
        }

    }

}