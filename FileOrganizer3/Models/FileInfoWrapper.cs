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
        private bool isIgnored;
        private bool playing;

        public FileInfoWrapper(FileInfo fileInfo)
        {
            FileInfo = fileInfo;
        }

        public string Name => FileInfo.Name;

        public int Index { get => index; set => SetProperty(ref index, value); }

        public bool IsSoundFile => new[] { ".mp3", ".ogg", ".wav", }.Contains(FileInfo.Extension.ToLower());

        public int PlayCount { get => playCount; set => SetProperty(ref playCount, value); }

        public bool IsMarked { get => isMarked; set => SetProperty(ref isMarked, value); }

        public bool IsIgnored { get => isIgnored; set => SetProperty(ref isIgnored, value); }

        public string TemporaryName { get; set; } = string.Empty;

        public bool Playing { get => playing; set => SetProperty(ref playing, value); }

        public string FullPath => FileInfo.FullName;

        public string Extension => FileInfo.Extension;

        private FileInfo FileInfo { get; set; }
    }
}