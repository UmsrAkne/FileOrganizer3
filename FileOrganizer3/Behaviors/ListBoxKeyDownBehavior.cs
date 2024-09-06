using System.Windows.Controls;
using System.Windows.Input;
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

            if (listBox == null)
            {
                return;
            }

            switch (e.Key)
            {
                case Key.J:
                    listBox.SelectedIndex++;
                    break;

                case Key.K:
                    if (listBox.SelectedIndex - 1 >= 0)
                    {
                        listBox.SelectedIndex--;
                    }

                    break;
            }

            listBox.ScrollIntoView(listBox.SelectedItem);

            // if (lv?.DataContext is MainWindowViewModel vm)
            // {
            //     // ビューモデルの処理を書く。
            // }
        }
    }
}