using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace FileOrganizer3.Models
{
    public class FileContainer
    {
        public ObservableCollection<FileInfoWrapper> FileInfoWrappers { get; set; } = new ();

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
                f.Index = idx++;
            }
        }
    }
}