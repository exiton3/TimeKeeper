namespace Framework.Repository.Abstractions
{
    using System.Collections.ObjectModel;

    public interface IRepository<T> where T:class 
    {
        ObservableCollection<T> GetAll();
        void Save(ObservableCollection<T> items);
    }
}