using System;
using System.Runtime.InteropServices;
using UserLogic.Domain;

namespace UserLogic
{
    [Serializable]
    [ComVisible(true)]
    public class TimeKeeper:IDisposable
    {
        private readonly IProjectLogRepository projectLogRepository;
        private readonly ISendLogProject sendLogProject;

        public TimeKeeper(IProjectLogRepository projectLogRepository,ISendLogProject sendLogProject)
        {
            this.projectLogRepository = projectLogRepository;
            this.sendLogProject = sendLogProject;
        }

        public void  AddLogEntry(ProjectLog projectLog)
        {
            projectLogRepository.Add(projectLog);
            projectLogRepository.Save();
        }
        public void  SendLog()
        {
            foreach (var projectLog in projectLogRepository.GetProjects())
            {
              sendLogProject.Send(projectLog);   
                projectLogRepository.Delete(projectLog);
            }
            projectLogRepository.Save();
        }

        #region Implementation of IDisposable

        public void Dispose()
        {
            projectLogRepository.Save();
           // throw new NotImplementedException();
        }

        #endregion
    }
}