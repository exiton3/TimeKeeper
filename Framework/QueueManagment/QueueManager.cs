namespace Framework.QueueManagment
{
    using System.ComponentModel;
    using ConnectionState;
    using Dispatcher;
    using Repository;

    public class QueueManager<T> where T: class 
    {
        private ObservableQueue<T> pendingData;

        private static QueueManager<T> instance;
        private static readonly object SyncLock = new object();

        private IRequestDispatcher requestDispatcher;
        private IConnectionMonitor connectionMonitor;
        private BackgroundWorker backgroundWorker;
        private IQueueRepository<T> queueRepository; 
        private QueueManager()
        {
            
        }

        public static QueueManager<T> Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (SyncLock)
                    {
                        if (instance == null)
                        {
                            instance = new QueueManager<T>();
                        }
                    }
                }
                return instance;
            }
        }

        public ObservableQueue<T> PendingData
        {
            get
            {
                if (pendingData == null)
                {
                    pendingData = new ObservableQueue<T>();
                }
                return pendingData;
            }

        }

        public IConnectionMonitor ConnectionMonitor
        {
            get { return connectionMonitor; }
        }

        public void Initialize(ObservableQueue<T> queue, IRequestDispatcher dispatcher, IConnectionMonitor connectionManager,IQueueRepository<T> queueRepository)
        {
            pendingData = queue;
            requestDispatcher = dispatcher;
            connectionMonitor = connectionManager;
            this.queueRepository = queueRepository;
            pendingData.ItemAddedEvent += PendingDataItemAddedEvent;

            connectionManager.ConnectionStatusChangedEvent += ConnectionManagerConnectionStatusChangedEvent;
            SetUpBackgroundWorker();
            if ((connectionManager.IsConnected) && (!backgroundWorker.IsBusy) && ((PendingData.Count > 0)))
            {
                backgroundWorker.RunWorkerAsync();
            }
        }

        private void ConnectionManagerConnectionStatusChangedEvent(bool isConnected)
        {
            if (isConnected)
            {
                if (!backgroundWorker.IsBusy)
                {
                    backgroundWorker.RunWorkerAsync();
                }
            }
            else
            {
                backgroundWorker.CancelAsync();
            }
        }

        private void PendingDataItemAddedEvent()
        {
            if ((connectionMonitor.IsConnected) && (!backgroundWorker.IsBusy))
            {
                backgroundWorker.RunWorkerAsync();
            }
            else
            {
                queueRepository.Save(PendingData);
            }
        }


        #region BackGroundWorker Events

        private void SetUpBackgroundWorker()
        {
            backgroundWorker = new BackgroundWorker();
            backgroundWorker.WorkerSupportsCancellation = true;
            backgroundWorker.WorkerReportsProgress = true;

            backgroundWorker.DoWork += requestDispatcher.Execute;

            backgroundWorker.ProgressChanged += backgroundWorker_ProgressChanged;
            backgroundWorker.RunWorkerCompleted += backgroundWorker_RunWorkerCompleted;
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                // Handle cancellation
            }
            else if (e.Error != null)
            {
                // Handle error
            }
            //else
            //{
            //    // Handle completion if necessary
            //}
        }

        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //
        }

        #endregion

    }
}