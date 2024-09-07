using System.Windows;
using Prism.Commands;
using Prism.Mvvm;

namespace FileOrganizer3.Models
{
    public class AppearanceManager : BindableBase
    {
        private double fontSize = 13.0;

        public Visibility HistoryListVisibility { get; set; }

        public double FontSize { get => fontSize; set => SetProperty(ref fontSize, value); }

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