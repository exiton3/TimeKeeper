namespace Framework.Dispatcher
{
    using System.ComponentModel;

    public interface IRequestDispatcher
    {
        void Execute(object sender, DoWorkEventArgs e);
    }
}