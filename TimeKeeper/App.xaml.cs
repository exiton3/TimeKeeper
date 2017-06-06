using System.Windows;
using Framework;
using Framework.Repository.Implementations;
using GalaSoft.MvvmLight.Threading;

namespace TimeKeeper
{
    using System.Configuration;
    using System.ServiceModel.Configuration;
    using Framework.Common.Security;
    using Framework.ConnectionState;
    using Framework.Dispatcher;
    using Framework.Model;
    using Framework.QueueManagment;
    using Framework.Repository;
    using Model;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static QueueManager<ActivityMessage> queueManager;
        private QueueRepository queueRepository;
        private ObservableQueue<ActivityMessage> queue;
        private ConnectionMonitor connectionMonitor;
        private IRequestDispatcher requestDispatcher;
        private static DataUpdater dataUpdater;

        static App()
        {
            DispatcherHelper.Initialize();
        }

        public static DataUpdater DataUpdater
        {
            get
            {
                if (dataUpdater == null)
                {
                    dataUpdater = new DataUpdater(new ProjectRepository(), new ActivityRepository(), queueManager.ConnectionMonitor, new Base64EncryptorEx());
                }
                return dataUpdater;
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            queueRepository.Save(queueManager.PendingData);
            base.OnExit(e);

        }

        public QueueManager<ActivityMessage> QueueManager { get { return queueManager; } }

        protected override void OnStartup(StartupEventArgs e)
        {
            WpfSingleInstance.Make();

            base.OnStartup(e);

            InitializeOffline();

            dataUpdater = new DataUpdater(new ProjectRepository(),
                new ActivityRepository(), queueManager.ConnectionMonitor, new Base64EncryptorEx());
            dataUpdater.ShowError += ShowError;
            if (dataUpdater.IsConnected)
            {
                dataUpdater.Update();
                dataUpdater.StartTimer();
            }

            MainWindow = new MainWindow();
            SetWindowSettings();

            MainWindow.ShowDialog();
        }

        private void ShowError()
        {
            if (DataUpdater.ServiceError != null)
            {
                MessageBox.Show(DataUpdater.ServiceError);
            }
        }

        private void InitializeOffline()
        {
            var endPoint = (ClientSection)ConfigurationManager.GetSection("system.serviceModel/client");
            var address = endPoint.Endpoints[0].Address;
            connectionMonitor = new ConnectionMonitor(address.AbsoluteUri, new HttpConnectionStrategy());//http://www.somed34.ru/

            queueRepository = new QueueRepository();
            requestDispatcher = new RequestDispatcher(queueRepository, new Base64EncryptorEx());
            queue = queueRepository.Load();

            queueManager = QueueManager<ActivityMessage>.Instance;
            queueManager.Initialize(queue, requestDispatcher, connectionMonitor, queueRepository);
        }

        private void SetWindowSettings()
        {
            MainWindow.Background = System.Windows.Media.Brushes.Transparent;
            MainWindow.AllowsTransparency = true;
            MainWindow.WindowStyle = WindowStyle.None;
            MainWindow.ShowInTaskbar = false;
            MainWindow.ResizeMode = ResizeMode.NoResize;
            MainWindow.Top = SystemParameters.WorkArea.Height - MainWindow.Height - 5;
            MainWindow.Left = SystemParameters.PrimaryScreenWidth - MainWindow.Width - 5;
            MainWindow.MouseLeftButtonDown += delegate
           {
               MainWindow.DragMove();
           };
        }
    }
}
