using System.Diagnostics;
using Prism.Mvvm;

namespace FileOrganizer3.Models
{
    public class TextWrapper : BindableBase
    {
        private string title;
        private string version = string.Empty;

        public TextWrapper()
        {
            Title = "FileOrganizer3";

            SetVersion();
            AddDebugMark();
        }

        private string Title
        {
            get => string.IsNullOrWhiteSpace(Version)
                ? title
                : title + " version : " + Version;
            set => SetProperty(ref title, value);
        }

        private string Version { get => version; set => SetProperty(ref version, value); }

        public override string ToString()
        {
            return Title;
        }

        [Conditional("RELEASE")]
        private void SetVersion()
        {
            Version = "20241215" + "b";
        }

        [Conditional("DEBUG")]
        private void AddDebugMark()
        {
            Title += " (Debug)";
        }
    }
}