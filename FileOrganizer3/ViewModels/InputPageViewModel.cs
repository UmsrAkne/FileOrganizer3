using System;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;

namespace FileOrganizer3.ViewModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class InputPageViewModel : BindableBase, IDialogAware
    {
        private string title = "Input page";
        private string message;
        private string text;

        public event Action<IDialogResult> RequestClose;

        public string Title { get => title; private set => SetProperty(ref title, value); }

        public string Message { get => message; set => SetProperty(ref message, value); }

        public string Text { get => text; set => SetProperty(ref text, value); }

        public DelegateCommand CloseCommand => new DelegateCommand(() =>
        {
            var r = new DialogResult(ButtonResult.OK);
            r.Parameters.Add(nameof(Text), Text);
            RequestClose?.Invoke(r);
        });

        public DelegateCommand CancelCommand => new DelegateCommand(() =>
        {
            var r = new DialogResult(ButtonResult.Abort);
            RequestClose?.Invoke(r);
        });

        public bool CanCloseDialog() => true;

        public void OnDialogClosed()
        {
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            if (parameters.TryGetValue<string>(nameof(Message), out var msg))
            {
                Message = msg;
            }
        }
    }
}