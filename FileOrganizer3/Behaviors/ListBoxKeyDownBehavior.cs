using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using FileOrganizer3.Models;
using FileOrganizer3.ViewModels;
using Microsoft.Xaml.Behaviors;

namespace FileOrganizer3.Behaviors
{
    public class ListBoxKeyDownBehavior : Behavior<ListBox>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.KeyDown += OnKeyDown;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.KeyDown -= OnKeyDown;
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            var listBox = sender as ListBox;

            var isShiftPressed = (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift;
            var isControlPressed = (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control;

            if (listBox == null)
            {
                return;
            }

            var vm = ((MainWindowViewModel)listBox.DataContext).FileContainer;

            switch (e.Key)
            {
                case Key.G:
                    if (isShiftPressed)
                    {
                        vm.CursorManager.MoveCursorToBottom();
                        break;
                    }

                    vm.CursorManager.MoveCursorToTop();
                    break;
                case Key.J:
                    if (isShiftPressed && listBox.SelectedIndex < listBox.Items.Count - 1)
                    {
                        var index = listBox.SelectedIndex;
                        var item = listBox.SelectedItem as FileInfoWrapper;
                        if (listBox.ItemsSource is ObservableCollection<FileInfoWrapper> items)
                        {
                            items.RemoveAt(index);
                            items.Insert(index + 1, item);
                            listBox.SelectedIndex = index + 1;
                            listBox.SelectedItem = item;
                            vm?.ReIndex(items);
                        }

                        break;
                    }

                    vm.CursorManager.MoveCursorDown();
                    break;

                case Key.K:
                    if (isShiftPressed && listBox.SelectedIndex > 0)
                    {
                        var index = listBox.SelectedIndex;
                        var item = listBox.SelectedItem as FileInfoWrapper;
                        if (listBox.ItemsSource is ObservableCollection<FileInfoWrapper> items)
                        {
                            items.RemoveAt(index);
                            items.Insert(index - 1, item);
                            listBox.SelectedIndex = index - 1;
                            listBox.SelectedItem = item;
                            vm?.ReIndex(items);
                        }

                        break;
                    }

                    vm.CursorManager.MoveCursorUp();
                    break;

                case Key.N:
                    if (!isShiftPressed)
                    {
                        vm.CursorManager.MoveCursorToNextMark();
                    }
                    else
                    {
                        vm.CursorManager.MoveCursorToPrevMark();
                    }

                    break;

                case Key.Delete:
                    if (isControlPressed && listBox.SelectedIndex >= 0)
                    {
                        var index = listBox.SelectedIndex;
                        if (listBox.ItemsSource is ObservableCollection<FileInfoWrapper> items)
                        {
                            items.RemoveAt(index);
                            vm?.ReIndex(items);
                        }
                    }

                    break;
            }

            if (listBox.SelectedItem != null)
            {
                listBox.ScrollIntoView(listBox.SelectedItem);
            }

            // if (lv?.DataContext is MainWindowViewModel vm)
            // {
            //     // ビューモデルの処理を書く。
            // }
        }
    }
}