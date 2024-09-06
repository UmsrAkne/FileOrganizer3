using FileOrganizer3.Models;
using Prism.Mvvm;

namespace FileOrganizer3.ViewModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class MainWindowViewModel : BindableBase
    {
        public MainWindowViewModel()
        {
        }

        public TextWrapper TextWrapper { get; private set; } = new ();
    }
}