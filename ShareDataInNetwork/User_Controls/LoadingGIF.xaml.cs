using System;
using System.Windows.Controls;

namespace ShareDataInNetwork.User_Controls
{
    /// <summary>
    /// Логика взаимодействия для LoadingGIF.xaml
    /// </summary>
    public partial class LoadingGif : UserControl
    {
        public LoadingGif()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        public string Title { get; set; }

        public int MaxLength { get; set; }
    }
}
