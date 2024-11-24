using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using FileOrganizer3.ViewModels;
using FileOrganizer3.Views;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;

namespace FileOrganizer3.Models
{
    public class FileContainer : BindableBase
    {
        private readonly IDialogService dialogService;
        private int startIndex = 1;
        private ObservableCollection<FileInfoWrapper> fileInfoWrappers;

        public FileContainer(IDialogService dialogService)
        {
            FileInfoWrappers = new ObservableCollection<FileInfoWrapper>();
            this.dialogService = dialogService;
        }

        public ObservableCollection<FileInfoWrapper> FileInfoWrappers
        {
            get => fileInfoWrappers;
            set
            {
                if (SetProperty(ref fileInfoWrappers, value))
                {
                    RaisePropertyChanged(nameof(IgnoreFileCommand));
                    RaisePropertyChanged(nameof(AppendTextToNameCommand));
                    RaisePropertyChanged(nameof(AppendNumberToNameCommand));
                    RaisePropertyChanged(nameof(RenameCommand));
                    RaisePropertyChanged(nameof(ClearFilesCommand));
                    CursorManager.Items = value;
                }
            }
        }

        public int StartIndex
        {
            get => startIndex;
            set
            {
                if (SetProperty(ref startIndex, value))
                {
                    ReIndex(FileInfoWrappers);
                }
            }
        }

        public CursorManager CursorManager { get; } = new ();

        public DelegateCommand MarkCommand => new DelegateCommand(() =>
        {
            if (CursorManager.SelectedItem == null)
            {
                return;
            }

            CursorManager.SelectedItem.IsMarked = !CursorManager.SelectedItem.IsMarked;
        });

        public DelegateCommand<ExtractOption?> MarkFilesCommand => new DelegateCommand<ExtractOption?>((param) =>
        {
            if (param == null)
            {
                return;
            }

            var targets = ExtractFiles(FileInfoWrappers, param.Value);
            foreach (var fileInfoWrapper in targets)
            {
                fileInfoWrapper.IsMarked = true;
            }
        });

        /// <summary>
        /// ファイルリストから、入力された ExtractOption に基づいて対象を選択。対象のファイルの `IsMarked` を反転します。
        /// </summary>
        public DelegateCommand<ExtractOption?> ToggleMarksCommand => new DelegateCommand<ExtractOption?>((param) =>
        {
            if (param == null)
            {
                return;
            }

            var targets = ExtractFiles(FileInfoWrappers, param.Value);
            foreach (var fileInfoWrapper in targets)
            {
                fileInfoWrapper.IsMarked = !fileInfoWrapper.IsMarked;
            }
        });

        public DelegateCommand IgnoreFileCommand => new (() =>
        {
            if (CursorManager.SelectedItem == null)
            {
                return;
            }

            CursorManager.SelectedItem.IsIgnored = !CursorManager.SelectedItem.IsIgnored;
            ReIndex(FileInfoWrappers);
        });

        public DelegateCommand<RenameOption> AppendTextToNameCommand => new DelegateCommand<RenameOption>((option) =>
        {
            if (option == null)
            {
                return;
            }

            option.Text = option.IsPrefix
                ? AppSettings.Load().PrefixText
                : AppSettings.Load().SuffixText;

            var files = ExtractFiles(FileInfoWrappers, option.ExtractOption).ToList();
            foreach (var fb in files.Where(f => f.TemporaryName == string.Empty))
            {
                fb.TemporaryName = Path.GetFileNameWithoutExtension(fb.Name);
            }

            if (option.IsPrefix)
            {
                foreach (var f in files)
                {
                    f.TemporaryName = $"{option.Text}_{f.TemporaryName}";
                }

                return;
            }

            foreach (var f in files)
            {
                f.TemporaryName = $"{f.TemporaryName}_{option.Text}";
            }
        });

        public DelegateCommand<RenameOption> AppendNumberToNameCommand => new DelegateCommand<RenameOption>((option) =>
        {
            if (option == null)
            {
                return;
            }

            var files = ExtractFiles(FileInfoWrappers, option.ExtractOption).ToList();
            foreach (var fb in files.Where(f => f.TemporaryName == string.Empty))
            {
                fb.TemporaryName = Path.GetFileNameWithoutExtension(fb.Name);
            }

            var setting = AppSettings.Load();
            var digitCount = setting.FormatDigitCount != 0 ? setting.FormatDigitCount : 1;

            for (var i = 0; i < files.Count; i++)
            {
                var f = files[i];
                var num = (i + StartIndex).ToString($"D{digitCount}");

                files[i].TemporaryName = option.IsPrefix
                    ? $"{num}_{f.TemporaryName}"
                    : $"{f.TemporaryName}_{num}";
            }
        });

        /// <summary>
        /// `FileInfoWrappers` の中で、TemporaryName が設定されているファイルをリネームします。
        /// </summary>
        public DelegateCommand RenameCommand => new DelegateCommand(() =>
        {
            var files = FileInfoWrappers.Where(f =>
                !string.IsNullOrWhiteSpace(f.TemporaryName)
                && f.TemporaryName != Path.GetFileNameWithoutExtension(f.Name));

            foreach (var fileInfoWrapper in files)
            {
                fileInfoWrapper.TemporaryName = $"{fileInfoWrapper.TemporaryName}{fileInfoWrapper.Extension}";
                fileInfoWrapper.Rename();
            }
        });

        public DelegateCommand ReverseListCommand => new DelegateCommand(() =>
        {
            FileInfoWrappers = new ObservableCollection<FileInfoWrapper>(FileInfoWrappers.Reverse());
        });

        public DelegateCommand ClearFilesCommand => new DelegateCommand(() =>
        {
            FileInfoWrappers.Clear();
        });

        public DelegateCommand ShowInputPageCommand => new DelegateCommand(() =>
        {
            var param = new DialogParameters { { nameof(InputPageViewModel.Message), "ジャンプする番号を入力してください。" }, };
            dialogService.ShowDialog(nameof(InputPage), param, result =>
            {
                if (result.Result != ButtonResult.OK)
                {
                    return;
                }

                var inputText = result.Parameters.GetValue<string>(nameof(InputPageViewModel.Text));
                if (int.TryParse(inputText, out var i))
                {
                    CursorManager.SelectedIndex = i - 1;
                }
            });
        });

        public DelegateCommand ShowSearchPageCommand => new DelegateCommand(() =>
        {
            var param = new DialogParameters { { nameof(InputPageViewModel.Message), "検索するパターンを入力してください。" }, };
            dialogService.ShowDialog(nameof(InputPage), param, result =>
            {
                if (result.Result != ButtonResult.OK)
                {
                    return;
                }

                if (result.Parameters.TryGetValue<string>(nameof(InputPageViewModel.Text), out var pattern))
                {
                    var idx = SearchIndexFromFileName(pattern, CursorManager);
                    if (idx < 0)
                    {
                        return;
                    }

                    CursorManager.SelectedIndex = idx;
                }
            });
        });

        public void AddFiles(IEnumerable<string> filePaths)
        {
            var fileInfos = filePaths.Select(p => new FileInfo(p));
            FileInfoWrappers.AddRange(fileInfos.Select(f => new FileInfoWrapper(f)).OrderBy(f => f.Name));

            ReIndex(FileInfoWrappers);
        }

        public void ReIndex(IEnumerable<FileInfoWrapper> files)
        {
            var idx = StartIndex;

            foreach (var f in files)
            {
                f.Index = f.IsIgnored ? 0 : idx++;
            }
        }

        /// <summary>
        /// 現在のカーソルのインデックスを検索開始点として、ファイル名に指定のパターンを含むファイルのインデックスを取得します。
        /// </summary>
        /// <param name="pattern">ファイル名に含まれるパターンを入力します。</param>
        /// <param name="cursorMgr">検索の開始点の取得・検索するリストの取得に使います。オブジェクト自体は変更しません。</param>
        /// <returns>マッチしたアイテムのインデックスを取得します。マッチしない場合は負の数を返します。</returns>
        private int SearchIndexFromFileName(string pattern, CursorManager cursorMgr)
        {
            var searchStartIndex = cursorMgr.SelectedIndex;
            var idx = cursorMgr.Items.Skip(searchStartIndex).ToList()
                .FindIndex(w => w.Name.Contains(pattern));

            if (idx >= 0)
            {
                return CursorManager.SelectedIndex + idx;
            }

            idx = cursorMgr.Items.Take(searchStartIndex).ToList()
                .FindIndex(w => w.Name.Contains(pattern));

            return idx < 0 ? -1 : idx;
        }

        private IEnumerable<FileInfoWrapper> ExtractFiles(IEnumerable<FileInfoWrapper> files, ExtractOption option)
        {
            switch (option)
            {
                case ExtractOption.All:
                    return files;
                case ExtractOption.Marked:
                    return files.Where(f => f.IsMarked && !f.IsIgnored);
                case ExtractOption.Ignored:
                    return files.Where(f => f.IsIgnored);
                case ExtractOption.NonIgnored:
                    return files.Where(f => !f.IsIgnored);
                default:
                    return files;
            }
        }
    }
}