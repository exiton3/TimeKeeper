namespace Framework.Model
{
    using System;

    public class ActivityMessage
    {
        public string ActionName { get; set; }

        public int ProjectId { get; set; }

        public int ActivityId { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime StopTime { get; set; }

    }
}