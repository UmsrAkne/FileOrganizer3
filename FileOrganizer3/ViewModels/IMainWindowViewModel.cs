using FileOrganizer3.Behaviors;
using FileOrganizer3.Models;

namespace FileOrganizer3.ViewModels
{
    public interface IMainWindowViewModel
    {
        TextWrapper TextWrapper { get; }

        FileContainer FileContainer { get; set; }
    }
}