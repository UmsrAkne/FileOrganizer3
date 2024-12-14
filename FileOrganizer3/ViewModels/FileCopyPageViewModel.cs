using System;
using System.Collections.Generic;
using System.IO;
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
        private string fileListText = string.Empty;
        private string logText;
        private string fileDestinationPath;

        public event Action<IDialogResult> RequestClose;

        public string Title => "File Copy Page";

        public string FileListText { get => fileListText; set => SetProperty(ref fileListText, value); }

        public string LogText { get => logText; private set => SetProperty(ref logText, value); }

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
            if (string.IsNullOrWhiteSpace(FileListText) || FileInfoWrappers == null || FileInfoWrappers.Count == 0 || !Directory.Exists(FileDestinationPath))
            {
                return;
            }

            var lines = FileListText.Split(new[] { "\r\n", "\n", "\r", }, StringSplitOptions.RemoveEmptyEntries);
            CopyFiles(lines, FileInfoWrappers, FileDestinationPath);
        });

        /// <summary>
        /// FileInfoWrappers の中から、マーク済みのファイル名のリストをテキストボックスに出力します。<br/>
        /// ファイルリストは一つずつ改行で区切られたフォーマットで出力されます。
        /// </summary>
        public DelegateCommand WriteMarkedFilesToTextBoxCommand => new DelegateCommand(() =>
        {
            var newText = FileInfoWrappers.Where(f => f.IsMarked).Select(f => f.Name);

            if (string.IsNullOrWhiteSpace(FileListText) || FileListText.EndsWith(Environment.NewLine))
            {
                FileListText += string.Join(Environment.NewLine, newText);
            }
            else
            {
                FileListText += $"{Environment.NewLine}{string.Join(Environment.NewLine, newText)}";
            }
        });

        private List<FileInfoWrapper> FileInfoWrappers { get; set; }

        public bool CanCloseDialog() => true;

        public void OnDialogClosed()
        {
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            if (parameters.TryGetValue<List<FileInfoWrapper>>(nameof(FileContainer.FileInfoWrappers), out var fw))
            {
                FileInfoWrappers = fw;
            }
        }

        /// <summary>
        /// fileNames の名前を、sourceFileInfos の中から検索して destinationPath にコピーします。<br/>
        /// また、必要に応じて LogText プロパティにログを出力します。
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
                else
                {
                    LogText += $"{fileName} がソースファイルリストに含まれていないため、コピーできませんでした。\n";
                }
            }
        }
    }
}