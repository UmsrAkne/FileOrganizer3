using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using FileOrganizer3.Models;
using FileOrganizer3.Views;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;

namespace FileOrganizer3.ViewModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class MainWindowViewModel : BindableBase, IMainWindowViewModel, IDisposable
    {
        private readonly SoundPlayer soundPlayer = new ();
        private readonly IDialogService dialogService;

        public MainWindowViewModel()
        {
            FileContainer = new FileContainer(null);
            PlayedFileContainer = new FileContainer(null);
            SetDummyData();
            AppearanceManager.FontSize = AppSettings.Load().FontSize;
            FileContainer.FileStatusChangedEventHandler += OnFileContainerOnFileStatusChangedEventHandler;
        }

        public MainWindowViewModel(IDialogService service)
        {
            dialogService = service;
            FileContainer = new FileContainer(service);
            PlayedFileContainer = new FileContainer(service);
            SetDummyData();
            AppearanceManager.FontSize = AppSettings.Load().FontSize;
            FileContainer.FileStatusChangedEventHandler += OnFileContainerOnFileStatusChangedEventHandler;
        }

        public string Title { get; private set; } = GetAppNameWithVersion();

        public FileContainer FileContainer { get; set; }

        public FileContainer PlayedFileContainer { get; set; }

        public FileContainer MarkedFiles { get; set; } = new (null) { AppendIndex = false, };

        public FileContainer IgnoredFiles { get; set; } = new (null) { AppendIndex = false, };

        public AppearanceManager AppearanceManager { get; set; } = new ();

        /// <summary>
        /// FileContainer 上で選択されいてるアイテムがサウンドファイルかを確認した後、再生し、再生履歴に記録します。
        /// PlayCommand は FileContainer に宣言するのが自然ですが、履歴ウィンドウの方にも情報を送る都合上ここに宣言しています。
        /// </summary>
        public DelegateCommand<FileInfoWrapper> PlaySoundAndSaveCommand => new DelegateCommand<FileInfoWrapper>((param) =>
        {
            if (param is not { IsSoundFile: true, })
            {
                return;
            }

            soundPlayer.PlayAudio(param);
            PlayedFileContainer.AddFiles(new[] { param.FullPath, });
        });

        /// <summary>
        /// FileContainer 上で選択されいてるアイテムがサウンドファイルかを確認した後、再生します。記録は残しません。
        /// PlayCommand は FileContainer に宣言するのが自然ですが、履歴ウィンドウの方にも情報を送る都合上ここに宣言しています。
        /// </summary>
        public DelegateCommand<FileInfoWrapper> PlaySoundCommand => new DelegateCommand<FileInfoWrapper>((param) =>
        {
            if (param is not { IsSoundFile: true, })
            {
                return;
            }

            soundPlayer.PlayAudio(param);
        });

        /// <summary>
        /// 再生中の音声を停止します。再生していない時に呼び出した場合は何も起こりません。
        /// </summary>
        public DelegateCommand StopSoundCommand => new DelegateCommand(() =>
        {
            soundPlayer.Stop();
        });

        public DelegateCommand ShowSettingPageCommand => new DelegateCommand(() =>
        {
            dialogService.ShowDialog(nameof(SettingPage), new DialogParameters(), (_) => { });
        });

        public DelegateCommand ShowFileCopyPageCommand => new DelegateCommand(() =>
        {
            var param = new DialogParameters
            {
                { nameof(FileContainer.FileInfoWrappers), FileContainer.FileInfoWrappers.ToList() },
            };

            dialogService.ShowDialog(nameof(FileCopyPage), param, (_) => { });
        });

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            soundPlayer?.Dispose();
        }

        private static string GetAppNameWithVersion()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var infoVersion = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;
            return !string.IsNullOrWhiteSpace(infoVersion)
                ? $"File Organizer3 ver:{infoVersion}"
                : "File Organizer3 (version unknown)";
        }

        [Conditional("DEBUG")]
        private void SetDummyData()
        {
            FileContainer.FileInfoWrappers =
                new ObservableCollection<FileInfoWrapper>(DummyFileProvider.GetDummyFiles());
        }

        /// <summary>
        /// マークしたファイル、無視状態を変更したファイルのリストを更新するためのイベントハンドラーです。<br/>
        /// FileContainer の中で、ファイルのマーク状態が変更された時に出るイベントにセットします。
        /// </summary>
        /// <param name="sender">イベントの発行元。このメソッドでは使用しません。</param>
        /// <param name="e">マーク状態が変更された FileInfoWrapper が Items プロパティに格納されています。</param>
        private void OnFileContainerOnFileStatusChangedEventHandler(object sender, FileStatusChangedEventArgs e)
        {
            foreach (var fileInfoWrapper in e.Items)
            {
                if (fileInfoWrapper.IsMarked)
                {
                    if (!MarkedFiles.FileInfoWrappers.Contains(fileInfoWrapper))
                    {
                        MarkedFiles.FileInfoWrappers.Add(fileInfoWrapper);
                    }
                }
                else
                {
                    MarkedFiles.FileInfoWrappers.Remove(fileInfoWrapper);
                }

                if (fileInfoWrapper.IsIgnored)
                {
                    if (!IgnoredFiles.FileInfoWrappers.Contains(fileInfoWrapper))
                    {
                        IgnoredFiles.FileInfoWrappers.Add(fileInfoWrapper);
                    }
                }
                else
                {
                    IgnoredFiles.FileInfoWrappers.Remove(fileInfoWrapper);
                }
            }
        }
    }
}