using System.Collections.ObjectModel;
using Prism.Mvvm;

namespace FileOrganizer3.Models
{
    public class CursorManager : BindableBase
    {
        private FileInfoWrapper selectedItem;
        private int selectedIndex;

        public ObservableCollection<FileInfoWrapper> Items { get; set; }

        public FileInfoWrapper SelectedItem
        {
            get => selectedItem;
            set => SetProperty(ref selectedItem, value);
        }

        public int SelectedIndex { get => selectedIndex; set => SetProperty(ref selectedIndex, value); }

        public void MoveCursorUp()
        {
            SelectedIndex--;
        }

        public void MoveCursorDown()
        {
            SelectedIndex++;
        }

        public void MoveCursorToTop()
        {
            if (Items == null || Items.Count == 0)
            {
                return;
            }

            SelectedIndex = 0;
        }

        public void MoveCursorToBottom()
        {
            if (Items == null || Items.Count == 0)
            {
                return;
            }

            SelectedIndex = Items.Count - 1;
        }
    }
}