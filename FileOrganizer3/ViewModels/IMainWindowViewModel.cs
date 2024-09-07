using FileOrganizer3.Models;
using Prism.Commands;

namespace FileOrganizer3.ViewModels
{
    public interface IMainWindowViewModel
    {
        TextWrapper TextWrapper { get; }

        FileContainer FileContainer { get; set; }

        FileContainer PlayedFileContainer { get; set; }

        DelegateCommand PlaySoundCommand { get; }
    }
}