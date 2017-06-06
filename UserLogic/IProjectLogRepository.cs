using System.Collections.Generic;
using UserLogic.Domain;

namespace UserLogic
{
    public interface IProjectLogRepository
    {
        void Add(ProjectLog projectLog);
        void Delete(ProjectLog projectLog);
        void Save();
        List<ProjectLog> GetProjects();
    }

   
}