using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Documents;
using Prism.Commands;
using Prism.Mvvm;

namespace FileOrganizer3.Models
{
    public class FileContainer : BindableBase
    {
        private FileInfoWrapper selectedItem;
        private int startIndex = 1;

        public ObservableCollection<FileInfoWrapper> FileInfoWrappers { get; set; } = new ();

        public FileInfoWrapper SelectedItem
        {
            get => selectedItem;
            set => SetProperty(ref selectedItem, value);
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

        public DelegateCommand<RenameOption> AppendTextToNameCommand => new DelegateCommand<RenameOption>((option) =>
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

        public void AddFiles(IEnumerable<string> filePaths)
        {
            var fileInfos = filePaths.Select(p => new FileInfo(p));
            FileInfoWrappers.AddRange(fileInfos.Select(f => new FileInfoWrapper(f, null)));
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
                default:
                    return files;
            }
        }
    }
}