using System;
using System.Windows.Controls;

namespace ShareDataInNetwork.User_Controls
{
    /// <summary>
    /// Логика взаимодействия для LoadingGIF.xaml
    /// </summary>
    public partial class LoadingGIF : UserControl
    {
        public LoadingGIF()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        public string Title { get; set; }

        public int MaxLength { get; set; }
    }
}
