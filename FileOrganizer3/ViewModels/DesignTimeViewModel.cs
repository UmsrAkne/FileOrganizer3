using System.Collections.ObjectModel;
using FileOrganizer3.Models;
using Prism.Commands;
using Prism.Mvvm;

namespace FileOrganizer3.ViewModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class DesignTimeViewModel : BindableBase, IMainWindowViewModel
    {
        private FileContainer fileContainer = new (null);

        public DesignTimeViewModel()
        {
            FileContainer.FileInfoWrappers =
                new ObservableCollection<FileInfoWrapper>(DummyFileProvider.GetDummyFiles());

            PlayedFileContainer.FileInfoWrappers =
                new ObservableCollection<FileInfoWrapper>(DummyFileProvider.GetDummyFiles());
        }

        public TextWrapper TextWrapper { get; } = new ();

        public FileContainer FileContainer { get => fileContainer; set => SetProperty(ref fileContainer, value); }

        public FileContainer PlayedFileContainer { get; set; } = new (null);

        public FileContainer MarkedFiles { get; set; } = new (null);

        public AppearanceManager AppearanceManager { get; set; } = new ();

        public DelegateCommand<FileInfoWrapper> PlaySoundAndSaveCommand { get; } = new ((_) =>
        {
        });

        public DelegateCommand<FileInfoWrapper> PlaySoundCommand { get; } = new (_ =>
        {
        });

        public DelegateCommand StopSoundCommand => new DelegateCommand(() =>
        {
        });

        public DelegateCommand ShowSettingPageCommand { get; } = new (() => { });
}
}