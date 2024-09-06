using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Prism.Commands;
using Prism.Mvvm;

namespace FileOrganizer3.Models
{
    public class FileContainer : BindableBase
    {
        private FileInfoWrapper selectedItem;

        public ObservableCollection<FileInfoWrapper> FileInfoWrappers { get; set; } = new ();

        public FileInfoWrapper SelectedItem
        {
            get => selectedItem;
            set => SetProperty(ref selectedItem, value);
        }

        public DelegateCommand MarkCommand => new DelegateCommand(() =>
        {
            if (SelectedItem == null)
            {
                return;
            }

            SelectedItem.IsMarked = !SelectedItem.IsMarked;
        });

        public DelegateCommand IgnoreFileCommand => new (() =>
        {
            if (SelectedItem == null)
            {
                return;
            }

            SelectedItem.IsIgnored = !SelectedItem.IsIgnored;
            ReIndex(FileInfoWrappers);
        });

        public void AddFiles(IEnumerable<string> filePaths)
        {
            var fileInfos = filePaths.Select(p => new FileInfo(p));
            FileInfoWrappers.AddRange(fileInfos.Select(f => new FileInfoWrapper(f)));
            ReIndex(FileInfoWrappers);
        }

        public void ReIndex(IEnumerable<FileInfoWrapper> files)
        {
            var idx = 1;

            foreach (var f in files)
            {
                f.Index = f.IsIgnored ? 0 : idx++;
            }
        }
    }
}