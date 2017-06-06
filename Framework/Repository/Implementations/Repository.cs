namespace Framework.Repository.Implementations
{
    using System.Collections.ObjectModel;
    using Abstractions;
    using Common;

    public class Repository<T>:IRepository<T> where T:class 
    {
        protected readonly string FileName;
        protected readonly SerializatorIso<ObservableCollection<T>> Serializator;

        public Repository()
        {
            FileName = typeof(T).Name + "s.xml";
            Serializator = new SerializatorIso<ObservableCollection<T>>();
        }

        public ObservableCollection<T> GetAll()
        {
            var items = Serializator.Deserialize(FileName);
            return items;
        }

        public void Save(ObservableCollection<T> items)
        {
            Serializator.Serialize(FileName, items);
        }
    }
}