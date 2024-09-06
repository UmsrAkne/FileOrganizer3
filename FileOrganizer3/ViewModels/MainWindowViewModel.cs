using System.Collections.ObjectModel;
using System.Diagnostics;
using FileOrganizer3.Behaviors;
using FileOrganizer3.Models;
using Prism.Mvvm;

namespace FileOrganizer3.ViewModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class MainWindowViewModel : BindableBase, IMainWindowViewModel
    {
        public MainWindowViewModel()
        {
            SetDummyData();
        }

        public TextWrapper TextWrapper { get; private set; } = new ();

        public FileContainer FileContainer { get; set; } = new ();

        [Conditional("DEBUG")]
        private void SetDummyData()
        {
            FileContainer.FileInfoWrappers =
                new ObservableCollection<FileInfoWrapper>(DummyFileProvider.GetDummyFiles());
        }
    }
}