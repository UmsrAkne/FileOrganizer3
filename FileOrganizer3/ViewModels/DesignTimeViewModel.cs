using System.Collections.ObjectModel;
using FileOrganizer3.Models;
using Prism.Mvvm;

namespace FileOrganizer3.ViewModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class DesignTimeViewModel : BindableBase, IMainWindowViewModel
    {
        private FileContainer fileContainer = new ();

        public DesignTimeViewModel()
        {
            FileContainer.FileInfoWrappers =
                new ObservableCollection<FileInfoWrapper>(DummyFileProvider.GetDummyFiles());

            FileContainer.FileInfoWrappers[1].IsMarked = true;
        }

        public TextWrapper TextWrapper { get; } = new ();

        public FileContainer FileContainer
        {
            get => fileContainer;
            set => SetProperty(ref fileContainer, value);
        }
    }
}