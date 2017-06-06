namespace TimeKeeper.Dialog
{
    using System;

    public class ModalDialogService : IModalDialogService
    {
        public void ShowDialog<TDialogViewModel>(IModalWindow view, TDialogViewModel viewModel, Action<TDialogViewModel> onDialogClose)
        {
            view.DataContext = viewModel;
            if (onDialogClose != null)
            {
                view.Closed += (sender, e) => onDialogClose(viewModel);
            }
            view.ShowDialog();
        }

        public void ShowDialog<TDialogViewModel>(IModalWindow view, TDialogViewModel viewModel)
        {
            ShowDialog(view, viewModel, null);
        }
    }
}