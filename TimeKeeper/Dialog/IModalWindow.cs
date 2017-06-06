namespace TimeKeeper.Dialog
{
    using System;

    public interface IModalWindow
    {
        bool? DialogResult { get; set; }
        event EventHandler Closed;
        void Show();
        bool? ShowDialog();
        object DataContext { get; set; }
        void Close();
    }

}