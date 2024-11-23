using System;
using System.Collections.ObjectModel;
using System.Linq;
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
            if (SelectedIndex > 0)
            {
                SelectedIndex--;
            }
        }

        public void MoveCursorDown()
        {
            if (SelectedIndex < Items.Count - 1)
            {
                SelectedIndex++;
            }
        }

        /// <summary>
        /// 指定行数だけカーソルを上下に移動します。
        /// </summary>
        /// <param name="step">カーソルを動かす数を指定します。</param>
        /// <remarks>
        /// 新しいインデックスがコレクションのアイテム数を上回る場合や、0 より小さい場合は結果がその範囲に調節されます。
        /// </remarks>
        public void MoveCursor(int step)
        {
            if (Items == null || Items.Count == 0)
            {
                return;
            }

            var newIndex = SelectedIndex + step;
            newIndex = Math.Min(Items.Count - 1, Math.Max(newIndex, 0));
            SelectedIndex = newIndex;
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

        public void MoveCursorToNextMark()
        {
            if (Items == null || Items.Count == 0 || !Items.Any(f => f.IsMarked))
            {
                return;
            }

            if (Items.Count(f => f.IsMarked) == 1)
            {
                SelectedIndex = Items.IndexOf(Items.First(f => f.IsMarked));
                return;
            }

            // 現在のカーソル位置よりも後に対象があるケース
            var rights = Items.Skip(SelectedIndex + 1).ToList();
            if (rights.Any(f => f.IsMarked))
            {
                SelectedIndex = Items.IndexOf(rights.First(f => f.IsMarked));
                return;
            }

            // 現在のカーソル位置よりも前に対象があるケース
            var lefts = Items.Take(SelectedIndex).ToList();
            SelectedIndex = Items.IndexOf(lefts.First(f => f.IsMarked));
        }

        public void MoveCursorToPrevMark()
        {
            if (Items == null || Items.Count == 0 || !Items.Any(f => f.IsMarked))
            {
                return;
            }

            if (Items.Count(f => f.IsMarked) == 1)
            {
                SelectedIndex = Items.IndexOf(Items.First(f => f.IsMarked));
                return;
            }

            // 現在のカーソル位置よりも前に対象があるケース
            var lefts = Items.Skip(SelectedIndex + 1).Reverse().ToList();
            if (lefts.Any(f => f.IsMarked))
            {
                SelectedIndex = Items.IndexOf(lefts.First(f => f.IsMarked));
                return;
            }

            // 現在のカーソル位置よりも後に対象があるケース
            var rights = Items.Take(SelectedIndex).Reverse().ToList();
            SelectedIndex = Items.IndexOf(rights.First(f => f.IsMarked));
        }
    }
}