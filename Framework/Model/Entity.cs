namespace Framework.Model
{
    using System.ComponentModel;

    public class Entity: INotifyPropertyChanged
    {
        private int id;
        private string name;
        public int Id { get { return id; } set { id = value; OnPropertyChanged("Id");} }
        public string Name
        {
            get { return name; }
            set { name = value; OnPropertyChanged("Name"); }
        }

        
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        // Create the OnPropertyChanged method to raise the event
        protected void OnPropertyChanged(string nameProp)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(nameProp));
            }
        }
        #endregion
    }
}