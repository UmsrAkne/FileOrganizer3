using System.Collections.ObjectModel;
using System.IO;
using FileOrganizer3.Behaviors;
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
            FileContainer.FileInfoWrappers = new ObservableCollection<FileInfoWrapper>()
            {
                new (new FileInfo("test_test_test111.mp3")),
                new (new FileInfo("test_test_test222.mp3")),
                new (new FileInfo("test_test_test333.mp3")),
                new (new FileInfo("test_test_test444.mp3")),
            };
        }

        public TextWrapper TextWrapper { get; } = new ();

        public FileContainer FileContainer
        {
            get => fileContainer;
            set => SetProperty(ref fileContainer, value);
        }
    }
}