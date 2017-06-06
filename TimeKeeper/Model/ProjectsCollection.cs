namespace TimeKeeper.Model
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Framework.Model;

    public class ProjectsCollection : ObservableCollection<Project>
    {
        public bool IsExist(Project project)
        {
            var con = from p in this
                      where (p.Id == project.Id && project.Activity != null && p.Activity.Id == project.Activity.Id)
                      select p;
            if (con.Count() == 0)
            {
                return false;
            }

            return true;
        }

        public ProjectsCollection()
        {
            
        }

        public ProjectsCollection(IEnumerable<Project> collection):base(collection)
        {
           
        }
    }
}