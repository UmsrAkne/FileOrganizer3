using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using FileOrganizer3.Models;
using Prism.Commands;
using Prism.Mvvm;

namespace FileOrganizer3.ViewModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class MainWindowViewModel : BindableBase, IMainWindowViewModel, IDisposable
    {
        private readonly SoundPlayer soundPlayer = new ();

        public MainWindowViewModel()
        {
            SetDummyData();
            AppearanceManager.FontSize = AppSettings.Load().FontSize;
        }

        public TextWrapper TextWrapper { get; private set; } = new ();

        public FileContainer FileContainer { get; set; } = new ();

        public FileContainer PlayedFileContainer { get; set; } = new ();

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

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            soundPlayer?.Dispose();
        }

        [Conditional("DEBUG")]
        private void SetDummyData()
        {
            FileContainer.FileInfoWrappers =
                new ObservableCollection<FileInfoWrapper>(DummyFileProvider.GetDummyFiles());
        }
    }
}