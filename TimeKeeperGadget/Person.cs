using System.ComponentModel;

namespace TimeKeeperGadget
{
    public class Person:INotifyPropertyChanged
    {
        private string name;
        public string Name
        {
            get { return name;}
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        // Create the OnPropertyChanged method to raise the event
        protected void OnPropertyChanged(string nameProperty)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(nameProperty));
            }
        }
    }
}