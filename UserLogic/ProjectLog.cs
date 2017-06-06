using System;
using System.Collections.Generic;

namespace UserLogic
{
    [Serializable]
    public class ProjectLog
    {
        public string UserId { get; set; }
        public string ProjectId { get; set; }
        public TimeSpan DurationTime { get; set; }
        public DateTime StartDate { get; set; }
    }
    [Serializable]
    public class ProjectLogEntries:List<ProjectLog>
    {
        
    }
}