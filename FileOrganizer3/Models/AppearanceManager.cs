using System.Windows;
using Prism.Commands;
using Prism.Mvvm;

namespace FileOrganizer3.Models
{
    public class AppearanceManager : BindableBase
    {
        public Visibility HistoryListVisibility { get; set; }

        public double FontSize { get; set; } = 12.0;

        public DelegateCommand ToggleHistoryListVisibilityCommand => new DelegateCommand(() =>
        {
            HistoryListVisibility = HistoryListVisibility == Visibility.Visible
                ? Visibility.Collapsed
                : Visibility.Visible;
        });

        public DelegateCommand<double?> SetFontSizeCommand => new DelegateCommand<double?>((param) =>
        {
            if (param == null)
            {
                return;
            }

            FontSize = (double)param;
        });
    }
}