﻿using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Input;
using FileOrganizer3.Models;
using FileOrganizer3.ViewModels;
using ImTools;
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

            if (listBox == null)
            {
                return;
            }

            var vm = ((MainWindowViewModel)listBox.DataContext).FileContainer;

            switch (e.Key)
            {
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

                    listBox.SelectedIndex++;
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

                    if (listBox.SelectedIndex - 1 >= 0)
                    {
                        listBox.SelectedIndex--;
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