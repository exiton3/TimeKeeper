namespace Framework.QueueManagment
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public delegate void ItemAddedEventHandler();
    [Serializable]
    public class ObservableQueue<T> where T: class
    {
        public event ItemAddedEventHandler ItemAddedEvent;

        private readonly Queue<T> queue = new Queue<T>();

        public ObservableQueue()
        {
            
        }

        public ObservableQueue(IEnumerable<T> list )
        {
            queue = new Queue<T>(list);
        }

        public void Enqueue(T item)
        {
            queue.Enqueue(item);
            ItemAddedEventHandler handler = ItemAddedEvent;
            if (handler != null)
            {
                handler();
            }
        }
        
        public T Peek()
        {
            return queue.Peek();
        }

        public T Dequeue()
        {
            return queue.Dequeue();
        }

        public Array ToArray()
        {
            return queue.ToArray();
        }

        public List<T> ToList()
        {
            var list = (from q in queue.ToArray()
                       select q).ToList();
            return list;
        }

        public void Clear()
        {
            queue.Clear();
        }

        public int Count
        {
            get
            {
                return queue.Count;
            }
        }
    }
}