namespace Framework.Repository
{
    using QueueManagment;

    public interface IQueueRepository<T> where T: class 
    {
        void Save(ObservableQueue<T> queue);

        ObservableQueue<T> Load();
    }
}