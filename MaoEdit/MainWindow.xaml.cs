using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MaoEdit
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static RoutedUICommand Command_Exit = new RoutedUICommand("_Exit", "Exit", typeof(MainWindow));
        public static RoutedUICommand Command_Minimaize = new RoutedUICommand("_Minimaize", "Minimaize", typeof(MainWindow));
        public static RoutedUICommand Command_Maximaize = new RoutedUICommand("_Maximaize", "Maximaize", typeof(MainWindow));
        public static RoutedUICommand Command_CloseTab = new RoutedUICommand("_CloseTab", "CloseTab", typeof(MainWindow));
        public static RoutedUICommand Command_AddTab = new RoutedUICommand("_AddTab", "AddTab", typeof(MainWindow));

        public MainWindow()
        {
            CommandBindings.Add(new CommandBinding(Command_Exit, (s, e) => { Close(); }));
            CommandBindings.Add(new CommandBinding(Command_Minimaize, (s, e) => { WindowState = WindowState.Minimized; }));
            CommandBindings.Add(new CommandBinding(Command_Maximaize, (s, e) => { WindowState = WindowState.Maximized; }));
            CommandBindings.Add(new CommandBinding(Command_CloseTab, (s, e) => { MainTabControl.Items.Remove(e.Source); }));
            CommandBindings.Add(new CommandBinding(Command_AddTab, (s, e) =>
            {
                int idx = 0;
                if (MainTabControl.Items.Count > 0)
                {
                    TabItem lastitem = (TabItem)MainTabControl.Items[MainTabControl.Items.Count - 1];
                    idx = int.Parse(lastitem.Tag.ToString());
                }

                ControlTemplate tmp = (ControlTemplate)Application.Current.Resources["MyTabItemTemple"];
                TabItem item = new TabItem();
                item.Template = tmp;
                item.Tag = idx + 1;
                item.Header = "Memo__" + (idx + 1);
                item.Content = new MyMemoBox();
                MainTabControl.Items.Add( item);
                item.IsSelected=true;
            }));
            InitializeComponent();
        }
        private void TabControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {

                if (e.ClickCount == 2)
                {
                    if (WindowState == WindowState.Maximized)
                        WindowState = WindowState.Normal;
                    else
                        WindowState = WindowState.Maximized;
                }
                else
                    DragMove();

            }
        }
        private void Window_StateChanged(object sender, EventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                Control o = (Control)FindName("MainTabControl");
                o.Margin = new Thickness(8);
            }
            if (WindowState == WindowState.Normal)
            {
                Control o = (Control)FindName("MainTabControl");
                o.Margin = new Thickness(0);
            }
        }


        private void MainTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MainTabControl.SelectedItem!=null)
            {
                TabItem item = (TabItem)MainTabControl.SelectedItem;
                item.Focus();
            }
        }
    }

}
