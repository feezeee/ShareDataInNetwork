using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
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
using MyNetworkInterface;

namespace ShareDataInNetwork
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        IpMethods _myNetwork = new IpMethods();
        IpParametrs _currentPc = new IpParametrs();

        public MainWindow()
        {
            InitializeComponent();


            ///////////////////////////////////////////////////////////////////////////////////////////////////////
            ///////////////////////////////////////////////////////////////////////////////////////////////////////
            _currentPc.OnGettingCurrentHostName += CurrentPC_OnGettingCurrentHostName;
            _currentPc.OnGettingIpAddressCurentPc += CurrentPC_OnGettingIpAddressCurentPC;
            _currentPc.OnGettingSubnetMask += CurrentPC_OnGettingSubnetMask;
            _currentPc.OnGettingBroadcastAddressCurentNetwork += CurrentPC_OnGettingBroadcastAddressCurentNetwork;
            ///////////////////////////////////////////////////////////////////////////////////////////////////////
            ///////////////////////////////////////////////////////////////////////////////////////////////////////
            _currentPc.OnGettingNetworkInformationAboutPc += CurrentPC_OnGettingNetworkInformationAboutPC;



            _currentPc.GetNetworkInformationAboutPcasync();        



            _myNetwork.AddingNewPcInList(_currentPc);

            // Запускаем поток по определению начать поиск пк в сети или нет.
            Thread myThread = new Thread(new ThreadStart(CheckingSearchingStatus));
            myThread.Start(); 
        }


        #region Получение сетевой информации о пк
        private void CurrentPC_OnGettingNetworkInformationAboutPC(object sender, bool status)
        {
            Application.Current.Dispatcher.Invoke((Action)delegate
            {
                ResetButton.IsEnabled = true;
            });
        }

        private void CurrentPC_OnGettingBroadcastAddressCurentNetwork(object sender, bool status)
        {
                Application.Current.Dispatcher.Invoke((Action)delegate
                {
                    broadcastCurrentPc.Content = sender;
                    broadcastCurrentPc.MouseDoubleClick += BroadcastCurrentPc_MouseDoubleClick;
                });
             
        }
        private void CurrentPC_OnGettingSubnetMask(object sender, bool status)
        {            
            Application.Current.Dispatcher.Invoke((Action)delegate
            {
                subnetMaskCurrentPc.Content = sender;
                subnetMaskCurrentPc.MouseDoubleClick += SubnetMaskCurrentPc_MouseDoubleClick;
            });
        }
        private void CurrentPC_OnGettingIpAddressCurentPC(object sender, bool status)
        {
                Application.Current.Dispatcher.Invoke((Action)delegate
                {
                    ipCurrentPc.Content = sender;
                    ipCurrentPc.MouseDoubleClick += IpCurrentPc_MouseDoubleClick;
                });
          
        }
        private void CurrentPC_OnGettingCurrentHostName(object sender, bool status)
        {
                Application.Current.Dispatcher.Invoke((Action)delegate
                {
                    nameCurrentPc.Content = sender;
                    nameCurrentPc.MouseDoubleClick += NameCurrentPc_MouseDoubleClick;
                });
           
        }
        #endregion

        #region Двойной клик по лабелам
        private void NameCurrentPc_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Label myLabel = (Label)sender;
                EditLabelWindow.MainWindow editWindow = new EditLabelWindow.MainWindow(_currentPc.ReturnNameInNetwork());
                if (editWindow.ShowDialog() == true)
                {
                    myLabel.Content = editWindow.Info;
                    _currentPc.SetNameInNetwork(editWindow.Info);
                }
            }
        }
        private void IpCurrentPc_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Label myLabel = (Label)sender;

                EditLabelWindow.EditForIp editWindow = new EditLabelWindow.EditForIp(_currentPc.ReturnIpAddress(), EditLabelWindow.EditForIp.Modes.SimplyIp);

                if (editWindow.ShowDialog() == true)
                {
                    myLabel.Content = editWindow.Info().ToString();
                    _currentPc.SetIpAddress(editWindow.Info());
                }
            }
        }
        private void SubnetMaskCurrentPc_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Label myLabel = (Label)sender;
                EditLabelWindow.EditForIp editWindow = new EditLabelWindow.EditForIp(_currentPc.ReturnSubnetMask(),EditLabelWindow.EditForIp.Modes.SubnetIp);
                if (editWindow.ShowDialog() == true)
                {
                    myLabel.Content = editWindow.Info().ToString();
                    _currentPc.SetSubnetMask(editWindow.Info());
                }
            }
        }
        private void BroadcastCurrentPc_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                _currentPc.GetBroadcastAddressCurentNetworkAsync();
            }
        }
        #endregion



        /// <summary>
        /// This variable shows searching status Эта переменная показывает статус поиска. 
        /// </summary>
        private bool _searchingStatus = false;
        private void Searching_Click(object sender, RoutedEventArgs e)
        {
            _searchingStatus = !_searchingStatus;            
        }


        private bool _search = false;
        private void CheckingSearchingStatus()
        {
            while (true)
            {
                if (_searchingStatus && !_search)
                {
                    _search = true;
                    broadcastCurrentPc.MouseDoubleClick -= BroadcastCurrentPc_MouseDoubleClick;
                    subnetMaskCurrentPc.MouseDoubleClick -= SubnetMaskCurrentPc_MouseDoubleClick;
                    ipCurrentPc.MouseDoubleClick -= IpCurrentPc_MouseDoubleClick;
                    nameCurrentPc.MouseDoubleClick -= NameCurrentPc_MouseDoubleClick;
                    
                    Application.Current.Dispatcher.Invoke((Action)delegate
                    {
                        ResetButton.IsEnabled = false;
                        StartSearchLoad.Opacity = 0;
                        searchingbrn.Content = "Стоп";
                        searchingbrn.IsEnabled = false;
                        _EnableBtn();
                    });
                    // запускаем
                }
                if (_search && !_searchingStatus)
                {
                    _search = false;
                    broadcastCurrentPc.MouseDoubleClick += BroadcastCurrentPc_MouseDoubleClick;
                    subnetMaskCurrentPc.MouseDoubleClick += SubnetMaskCurrentPc_MouseDoubleClick;
                    ipCurrentPc.MouseDoubleClick += IpCurrentPc_MouseDoubleClick;
                    nameCurrentPc.MouseDoubleClick += NameCurrentPc_MouseDoubleClick;

                    Application.Current.Dispatcher.Invoke((Action)delegate
                    {
                        ResetButton.IsEnabled = true;
                        searchingbrn.Content = "Начать поиск";
                        StartSearchLoad.Opacity = 100;
                    });
                    // останавливаем
                }
            }            
        }

        private async void _EnableBtn()
        {
            await Task.Run(() =>
            {
                Thread.Sleep(5000); 
                Application.Current.Dispatcher.Invoke((Action)delegate
                {
                    searchingbrn.IsEnabled = true;
                });
            });
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            ResetButton.IsEnabled = false;
            nameCurrentPc.Content = "";
            ipCurrentPc.Content = "";
            subnetMaskCurrentPc.Content = "";
            broadcastCurrentPc.Content = "";
            _currentPc.GetNetworkInformationAboutPcasync();
        }

        private void MainLoadWindow_Closed(object sender, EventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
    }
}
