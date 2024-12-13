using System;
using System.Collections.Generic;
using FileOrganizer3.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;

namespace FileOrganizer3.ViewModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class FileCopyPageViewModel : BindableBase, IDialogAware
    {
        private string fileListText;
        private string logText;
        private string fileDestinationPath;

        public event Action<IDialogResult> RequestClose;

        public string Title => "File Copy Page";

        public string FileListText { get => fileListText; set => SetProperty(ref fileListText, value); }

        public string LogText { get => logText; set => SetProperty(ref logText, value); }

        public List<FileInfoWrapper> FileInfoWrappers { get; set; }

        public string FileDestinationPath
        {
            get => fileDestinationPath;
            set => SetProperty(ref fileDestinationPath, value);
        }

        public DelegateCommand CloseCommand => new DelegateCommand(() =>
        {
            RequestClose?.Invoke(new DialogResult());
        });

        public DelegateCommand CopyFilesCommand => new DelegateCommand(() =>
        {
        });

        public bool CanCloseDialog() => true;

        public void OnDialogClosed()
        {
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
        }
    }
}