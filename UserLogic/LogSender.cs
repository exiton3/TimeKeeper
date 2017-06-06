using System;
using UserLogic.Domain;

namespace UserLogic
{
    public class LogSender:ISendLogProject
    {
        #region Implementation of ISendLogProject

        public void Send(ProjectLog projectLog)
        {
            Console.WriteLine("Send project log: " + projectLog.ProjectId + " "+ projectLog.UserId);
        }

        #endregion
    }
}