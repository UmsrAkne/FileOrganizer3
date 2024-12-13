using System;
using System.Collections.Generic;
using System.Linq;
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

        /// <summary>
        /// fileNames の名前を、sourceFileInfos の中から検索して destinationPath にコピーします。
        /// </summary>
        /// <param name="fileNames">コピーの対象とするファイル名</param>
        /// <param name="sourceFileInfos">コピーの対象を含む FileInfo のリスト</param>
        /// <param name="destinationPath">コピー先のパス</param>
        private void CopyFiles(IEnumerable<string> fileNames, IEnumerable<FileInfoWrapper> sourceFileInfos, string destinationPath)
        {
            // ファイル名がユニークでない場合はエラーが出る？
            var fileInfos = sourceFileInfos.ToDictionary(f => f.Name);

            foreach (var fileName in fileNames)
            {
                if (fileInfos.TryGetValue(fileName, out var fileInfoWrapper))
                {
                    fileInfoWrapper.CopyTo(destinationPath);
                }
            }
        }
    }
}