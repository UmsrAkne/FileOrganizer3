using System.IO;
using System.Linq;
using Prism.Mvvm;

namespace FileOrganizer3.Models
{
    public class FileInfoWrapper : BindableBase
    {
        private int index;
        private int playCount;
        private bool isMarked;

        public FileInfoWrapper(FileInfo fileInfo)
        {
            FileInfo = fileInfo;
        }

        public string Name => FileInfo.Name;

        public int Index { get => index; set => SetProperty(ref index, value); }

        public bool IsSoundFile => new[] { ".mp3", ".ogg", ".wav", }.Contains(FileInfo.Extension.ToLower());

        public int PlayCount { get => playCount; set => SetProperty(ref playCount, value); }

        public bool IsMarked { get => isMarked; set => SetProperty(ref isMarked, value); }

        public bool IsIgnored { get; set; }

        public string TemporaryName { get; set; } = string.Empty;

        private FileInfo FileInfo { get; set; }
    }
}