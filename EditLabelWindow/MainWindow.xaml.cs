using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EditLabelWindow
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _original;

        public string Original
        {
            set { _original = value; }            
        }

        public MainWindow()
        {
            InitializeComponent();
            renameBox.Text = _original;
        }
        public MainWindow(string originalText)
        {
            InitializeComponent();
            Original = originalText;
            renameBox.Text = _original;
        }
        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        public string Info
        {
            get { return renameBox.Text; }
        }
    }
}
