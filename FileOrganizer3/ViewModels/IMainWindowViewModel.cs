using FileOrganizer3.Models;
using Prism.Commands;

namespace FileOrganizer3.ViewModels
{
    public interface IMainWindowViewModel
    {
        string Title { get; }

        FileContainer FileContainer { get; set; }

        FileContainer PlayedFileContainer { get; set; }

        FileContainer MarkedFiles { get; set; }

        FileContainer IgnoredFiles { get; set; }

        AppearanceManager AppearanceManager { get; set; }

        DelegateCommand<FileInfoWrapper> PlaySoundAndSaveCommand { get; }

        DelegateCommand<FileInfoWrapper> PlaySoundCommand { get; }

        DelegateCommand StopSoundCommand { get; }

        DelegateCommand ShowSettingPageCommand { get; }

        DelegateCommand ShowFileCopyPageCommand { get; }
    }
}