using System.Windows;

namespace TimeKeeperGadget
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            MainWindow = new GadgetWindow();
            MainWindow.Background = System.Windows.Media.Brushes.Transparent;
            MainWindow.AllowsTransparency = true;
            MainWindow.WindowStyle = WindowStyle.None;
            MainWindow.ShowInTaskbar = false;
            MainWindow.ResizeMode = ResizeMode.NoResize;
    
           // MainWindow.SizeToContent = SizeToContent.WidthAndHeight;
            MainWindow.MouseLeftButtonDown += delegate
            {
                MainWindow.DragMove();
            };
            MainWindow.ShowDialog();
        }
    }
}
