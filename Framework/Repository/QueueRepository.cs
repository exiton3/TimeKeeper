namespace Framework.Repository
{
    using System.Collections.Generic;
    using Common;
    using Model;
    using QueueManagment;

    public class QueueRepository : IQueueRepository<ActivityMessage>
    {
        private const string FileName="queue.xml";
        

        public void Save(ObservableQueue<ActivityMessage> queue)
        {
            var list =queue.ToList();

            var serializator = new SerializatorIso<List<ActivityMessage>>();
            serializator.Serialize(FileName, list);
        }

        public ObservableQueue<ActivityMessage> Load()
        {
            var queue = new ObservableQueue<ActivityMessage>();
            var serializator = new SerializatorIso<List<ActivityMessage>>();
            var list = serializator.Deserialize(FileName);

            if (list != null) queue = new ObservableQueue<ActivityMessage>(list);

            return queue;
        }
    }
}