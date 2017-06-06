using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace UserLogic.Domain
{
    [Serializable]
    public class Projects : ObservableCollection<Project>
    {
        public bool IsExist(Project project)
        {
            var con = from p in this
                      where (p.Id == project.Id)
                      select p;
            if (con.Count() == 0)
            {
                return false;
            }

            return true;
        }
    }
}