using System.IO;
using System.IO.Abstractions;
using System.Linq;
using Prism.Mvvm;

namespace FileOrganizer3.Models
{
    public class FileInfoWrapper : BindableBase
    {
        private readonly IFileSystem fileSystem = new FileSystem();

        private int index;
        private int playCount;
        private bool isMarked;
        private bool isIgnored;
        private bool playing;
        private string temporaryName = string.Empty;

        /// <summary>
        /// `FileInfoWrapper` クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="fileInfo">
        /// 初期化に使用する `FileInfo` オブジェクト。`fInfo` が指定されていない場合に使用されます。<br/>
        /// この引数は、`fileSystem` が指定されていない場合に `System.IO.Abstractions.FileInfoWrapper` を作成するために必要です。
        /// </param>
        /// <param name="fInfo">
        /// 任意の `IFileInfo` インターフェースの実装オブジェクト。指定された場合は、`FileInfo` プロパティにこのオブジェクトを設定します。<br/>
        /// </param>
        /// <param name="fileSystem">
        /// 任意の `IFileSystem` インターフェースの実装オブジェクト。<br/>
        /// `fInfo` が指定されていない場合に、`FileInfo` プロパティの初期化に使用されます。<br/>
        /// `fileSystem` 引数が指定されない場合は、`FileSystem` のデフォルト実装が使用されます。
        /// </param>
        public FileInfoWrapper(FileInfo fileInfo, IFileInfo fInfo = null, IFileSystem fileSystem = null)
        {
            if (fileSystem != null)
            {
                this.fileSystem = fileSystem;
            }

            if (fInfo != null)
            {
                FileInfo = fInfo;
                return;
            }

            FileInfo = new System.IO.Abstractions.FileInfoWrapper(this.fileSystem, fileInfo);
        }

        public string Name => FileInfo.Name;

        public int Index { get => index; set => SetProperty(ref index, value); }

        public bool IsSoundFile => new[] { ".mp3", ".ogg", ".wav", }.Contains(FileInfo.Extension.ToLower());

        public int PlayCount { get => playCount; set => SetProperty(ref playCount, value); }

        public bool IsMarked { get => isMarked; set => SetProperty(ref isMarked, value); }

        public bool IsIgnored { get => isIgnored; set => SetProperty(ref isIgnored, value); }

        public string TemporaryName { get => temporaryName; set => SetProperty(ref temporaryName, value); }

        public bool Playing { get => playing; set => SetProperty(ref playing, value); }

        public string FullPath => FileInfo.FullName;

        public string Extension => FileInfo.Extension;

        private IFileInfo FileInfo { get; set; }

        /// <summary>
        /// 格納されている FileInfo が指しているファイルの名前を変更します。
        /// TemporaryName が null や空文字の場合、また、変更先のファイル名が既に存在する場合も動作せず終了します。
        /// </summary>
        public void Rename()
        {
            if (string.IsNullOrWhiteSpace(TemporaryName))
            {
                return;
            }

            var parentDirectoryPath = FileInfo.Directory?.FullName;
            var destPath = $"{parentDirectoryPath}\\{TemporaryName}";
            if (File.Exists(destPath))
            {
                return;
            }

            FileInfo.MoveTo(destPath);
            FileInfo = new System.IO.Abstractions.FileInfoWrapper(fileSystem, new FileInfo(destPath));
            TemporaryName = string.Empty;

            RaisePropertyChanged(nameof(FullPath));
            RaisePropertyChanged(nameof(Extension));
            RaisePropertyChanged(nameof(IsSoundFile));
            RaisePropertyChanged(nameof(Extension));
        }

        public void CopyTo(string destinationPath)
        {
            FileInfo.CopyTo(destinationPath);
        }
    }
}