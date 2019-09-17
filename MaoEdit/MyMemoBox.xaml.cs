using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MaoEdit
{
    public partial class MyMemoBox : System.Windows.Controls.UserControl
    {
        delegate void DelegateRedraw();
        private DelegateRedraw redraw ;
        private int[] lineNumber;
        public static RoutedUICommand Command_UTF8 = new RoutedUICommand("_UTF8", "Utf8", typeof(MyMemoBox));
        public static RoutedUICommand Command_GB = new RoutedUICommand("_GB", "Gb", typeof(MyMemoBox));
        public static RoutedUICommand Command_BIG = new RoutedUICommand("_BIG", "Big", typeof(MyMemoBox));
        public static RoutedUICommand Command_CR = new RoutedUICommand("_CR", "CR", typeof(MyMemoBox));
        public static RoutedUICommand Command_LF = new RoutedUICommand("_LF", "LF", typeof(MyMemoBox));
        public static RoutedUICommand Command_FONT = new RoutedUICommand("_FONT", "FONT", typeof(MyMemoBox));


        public System.Drawing.Font MyFont { get; private set; }

        public string MyText
        {
            get => MyTextBox.Text;
            set => MyTextBox.Text = value;
        }

        public MyMemoBox()
        {
            CommandBindings.Add(new CommandBinding(Command_UTF8, (s, e) => { Toggle(0); }));
            CommandBindings.Add(new CommandBinding(Command_GB, (s, e) => { Toggle(1); }));
            CommandBindings.Add(new CommandBinding(Command_BIG, (s, e) => { Toggle(2); }));
            CommandBindings.Add(new CommandBinding(Command_CR, (s, e) => { Toggle(3); }));
            CommandBindings.Add(new CommandBinding(Command_LF, (s, e) => { Toggle(4); }));
            CommandBindings.Add(new CommandBinding(Command_FONT, (s, e) => { Toggle(5); }));

            InitializeComponent();

            MyFont = new System.Drawing.Font("Microsoft Sans Serif", 16);
            MyTextBox.FontFamily = new FontFamily(MyFont.FontFamily.Name);
            MyTextBox.FontSize = MyFont.Size;
            MyLineNumberLabel.FontFamily = new FontFamily(MyFont.FontFamily.Name);
            MyLineNumberLabel.FontSize = MyFont.Size;

            lineNumber = new int[0];
            
        }

        public void ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            MyLineNumberLabel.ScrollToVerticalOffset(e.VerticalOffset);
        }

        private void Toggle(int v)
        {
            switch (v)
            {
                case 0:
                    ConvertTo(Encoding.UTF8);
                    Btn_1_UTF8.IsChecked = true;
                    Btn_1_GB.IsChecked = false;
                    Btn_1_BIG.IsChecked = false;

                    break;
                case 1:
                    ConvertTo(Encoding.GetEncoding("gb2312"));
                    Btn_1_UTF8.IsChecked = false;
                    Btn_1_GB.IsChecked = true;
                    Btn_1_BIG.IsChecked = false;
                    break;
                case 2:
                    ConvertTo(Encoding.GetEncoding("big5"));
                    Btn_1_UTF8.IsChecked = false;
                    Btn_1_GB.IsChecked = false;
                    Btn_1_BIG.IsChecked = true;
                    break;
                case 3:
                    ConvertToCR();
                    Btn_2_CR.IsChecked = true;
                    Btn_2_LF.IsChecked = false;
                    break;
                case 4:
                    ConvertToLF();
                    Btn_2_CR.IsChecked = false;
                    Btn_2_LF.IsChecked = true;
                    break;
                case 5:
                    FontDialog dialog = new FontDialog();
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        MyFont = dialog.Font;
                        MyTextBox.FontFamily = new FontFamily(MyFont.FontFamily.Name);
                        MyTextBox.FontSize = MyFont.Size;
                        MyLineNumberLabel.FontFamily = new FontFamily(MyFont.FontFamily.Name);
                        MyLineNumberLabel.FontSize = MyFont.Size;
                    }
                    break;
            }
        }

        private void ConvertTo(Encoding encoding)
        {
            byte[] bytes = Encoding.Default.GetBytes(MyText);
            MyText = encoding.GetString(bytes);
        }

        private void ConvertToLF()
        {
            MyText = MyText.Replace("\r\n", "\n");
        }

        private void ConvertToCR()
        {
            char[] charArray = MyText.ToArray();
            string newText = "";
            char lastChr = '\0';
            foreach (char c in charArray)
            {
                if (c == '\n' && lastChr != '\r')
                {
                    newText += "\r\n";
                }
                if (c == '\r' && lastChr != '\r')
                {
                    newText += "\r\n";
                }
                if (c != '\n' && c != '\r')
                {
                    newText += c;
                }
                lastChr = c;
            }
            MyText = newText;
        }

        private void MakeLineNumber()
        {
            int nm = 0;
            int lineCount = MyTextBox.LineCount;
            char[] chars = MyTextBox.Text.ToArray();
            lineNumber = new int[lineCount];
            for (int i = 0; i < lineCount; i++)
            {
                int lastlineidx = MyTextBox.GetCharacterIndexFromLineIndex(i);
                char lastChar = '\0';
                if (lastlineidx > 0) lastChar = chars[lastlineidx - 1];
                if (i == 0 || lastChar == '\n')
                {
                    lineNumber[i] = nm;
                    nm++;
                }
                else
                {
                    lineNumber[i] = -1;
                }
            }
        }

        void RedrawLineNumber (){
            string lineStr = "";
            for (int i = 0; i < MyTextBox.LineCount; i++)
            {
                if (i < lineNumber.Length && lineNumber[i] != -1)
                {
                    lineStr += lineNumber[i].ToString();
                }
                lineStr += "\r\n";
            }
            MyLineNumberLabel.Text = lineStr;
        }

        private void MyTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {            
            MakeLineNumber();
            RedrawLineNumber();
        }

        private void MyTextBox_LayoutUpdated(object sender, EventArgs e)
        {            
            MakeLineNumber();
            RedrawLineNumber();
        }
    }
}
