namespace Framework.Model
{
    public class Project : Entity
    {

        public string FullName
        {
            get
            {
                if (Activity != null)
                {

                }
                return Activity == null ? Name : Name + " - " + Activity.Name;
            }
        }

        private Activity activity;

        public Activity Activity
        {
            get { return activity; }
            set { activity = value; OnPropertyChanged("Activity"); OnPropertyChanged("FullName"); }
        }

        public override bool Equals(object obj)
        {
            var project = obj as Project;
            if (project != null)
            {
                return base.Id.Equals(project.Id);
            }
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

    }
}