using System;
using System.ComponentModel;
using System.Windows.Threading;

namespace TimeKeeperGadget
{
    public class TimerManger : INotifyPropertyChanged
    {
        private readonly DispatcherTimer dispatcherTimer;
        private TimeSpan durationTimeSpan;
        private string durationTime;
        private DateTime startTime;

        public TimerManger()
        {
            durationTimeSpan = new TimeSpan(0, 0, 0);
            durationTime = durationTimeSpan.ToString();
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += DispatcherTimerTick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.IsEnabled = false;
            
        }

        public string DurationTime
        {
            get { return durationTime; }
            set
            {
                durationTime = value;
                OnPropertyChanged("DurationTime");
            }
        }
       
        public bool IsStartTimer
        {
            get
            {
                return dispatcherTimer.IsEnabled;
            }
            set { dispatcherTimer.IsEnabled = value; }
        }

        public void StartTimer()
        {
            dispatcherTimer.Start();
            startTime = DateTime.Now;
        }

        public void StopTimer()
        {
            dispatcherTimer.Stop();
        }

       
        private void DispatcherTimerTick(object sender, EventArgs e)
        {
            durationTimeSpan = DateTime.Now - startTime;
            DurationTime = string.Format("{0:d2}:{1:d2}:{2:d2}", durationTimeSpan.Hours, durationTimeSpan.Minutes,
                                         durationTimeSpan.Seconds);          

        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        // Create the OnPropertyChanged method to raise the event
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        } 
        #endregion
    }
}