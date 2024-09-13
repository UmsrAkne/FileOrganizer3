using System.Windows;
using Prism.Commands;
using Prism.Mvvm;

namespace FileOrganizer3.Models
{
    public class AppearanceManager : BindableBase
    {
        private double fontSize = 13.0;
        private GridLength historyPanelHeight = new GridLength(150.0);
        private double savedHistoryPanelHeight;
        private Visibility historyListVisibility;

        public Visibility HistoryListVisibility
        {
            get => historyListVisibility;
            set => SetProperty(ref historyListVisibility, value);
        }

        public double FontSize { get => fontSize; set => SetProperty(ref fontSize, value); }

        public GridLength HistoryPanelHeight
        {
            get => historyPanelHeight;
            set => SetProperty(ref historyPanelHeight, value);
        }

        public DelegateCommand ToggleHistoryListVisibilityCommand => new DelegateCommand(() =>
        {
            if (HistoryPanelHeight.Value == 0)
            {
                HistoryPanelHeight = new GridLength(savedHistoryPanelHeight);
                HistoryListVisibility = Visibility.Visible;
            }
            else
            {
                savedHistoryPanelHeight = HistoryPanelHeight.Value;
                HistoryPanelHeight = new GridLength(0);
                HistoryListVisibility = Visibility.Hidden;
            }
        });

        public DelegateCommand<double?> SetFontSizeCommand => new DelegateCommand<double?>((param) =>
        {
            if (param == null)
            {
                return;
            }

            FontSize = (double)param;

            var appSettings = AppSettings.Load();
            appSettings.FontSize = (double)param;
            appSettings.Save();
        });
    }
}