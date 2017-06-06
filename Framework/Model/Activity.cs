namespace Framework.Model
{
    public class Activity : Entity
    {
        public override bool Equals(object obj)
        {
            var activity = obj as Activity;
            if (activity != null)
            {
                return base.Id.Equals(activity.Id);
            }
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}