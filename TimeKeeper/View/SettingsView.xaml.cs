using System.Windows;

namespace TimeKeeper.View
{
    using Dialog;

    /// <summary>
    /// Description for SettingsView.
    /// </summary>
    public partial class SettingsView : IModalWindow
    { /// <summary>
        /// Initializes a new instance of the SettingsView class.
        /// </summary>
        public SettingsView()
        {
            InitializeComponent();
        }

        private void OkButtonClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}