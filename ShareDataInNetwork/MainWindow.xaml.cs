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
using ShareDataInNetwork.User_Controls;

namespace ShareDataInNetwork
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        IpParametrs _currentPc = new IpParametrs(); 
        IpMethods _myNetwork = new IpMethods();
        public MainWindow()
        {
            InitializeComponent();
            
            // Событие возникает при определении имени пк.
            _currentPc.OnGettingCurrentHostName += CurrentPC_OnGettingCurrentHostName;
            
            // Событие возникает при определении ip-адреса пк.
            _currentPc.OnGettingIpAddressCurentPc += CurrentPC_OnGettingIpAddressCurentPC;
            
            // Событие возникает при определении маски подсети пк.
            _currentPc.OnGettingSubnetMask += CurrentPC_OnGettingSubnetMask;
            
            // Событие возникает при определении широковещательного адреса сети, в которой находится пк.
            _currentPc.OnGettingBroadcastAddressCurentNetwork += CurrentPC_OnGettingBroadcastAddressCurentNetwork;
            
            // Событие возникает при окончании получения информации о пк.
            _currentPc.OnGettingNetworkInformationAboutPc += CurrentPC_OnGettingNetworkInformationAboutPC;

            // Вызываем метод получения сетевой информации о пк (ASYNC).
            _currentPc.GetNetworkInformationAboutPcasync();
            
            // Добавляем в List данный пк. Данный List содержит информацию о сети (к каким устройствам имеет доступ данный пк).
            _myNetwork.AddingNewPcInList(_currentPc);

            _myNetwork.ReceivedInformationEvent += AddingPc;
            
            // Определение потока. Анализ сети, в которой находится данный пк.
            Thread myThread = new Thread(new ThreadStart(CheckingSearchingStatus));
            
            // Запуск потока.
            myThread.Start();

            _myNetwork.CurrentPc = _currentPc;
            
            Thread sendBroadcastMessage = new Thread(new ThreadStart(_myNetwork.SendBroadcastOfferToConnect));
            sendBroadcastMessage.Start();
        }


        #region Получение сетевой информации о пк и отображение ее на экране приложения
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

        #region Двойной клик по лабелам для последующего изменения их содержимого
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
        /// This variable shows searching status. Эта переменная показывает статус поиска. 
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
                    
                    Thread receiveBroadcastMessage = new Thread(new ThreadStart(_myNetwork.ReciveBroadcastOffer));
                    receiveBroadcastMessage.Start();
                    
                    _search = true;
                    broadcastCurrentPc.MouseDoubleClick -= BroadcastCurrentPc_MouseDoubleClick;
                    subnetMaskCurrentPc.MouseDoubleClick -= SubnetMaskCurrentPc_MouseDoubleClick;
                    ipCurrentPc.MouseDoubleClick -= IpCurrentPc_MouseDoubleClick;
                    nameCurrentPc.MouseDoubleClick -= NameCurrentPc_MouseDoubleClick;
                    
                    Application.Current.Dispatcher.Invoke((Action)delegate
                    {
                        ResetButton.IsEnabled = false;
                        //StartSearchLoad.Opacity = 0;
                        searchingbrn.Content = "Стоп";
                        searchingbrn.IsEnabled = false;
                        _EnableBtn();
                        //Initializing_PC(currentPC.ReturnNameInNetwork(), currentPC.ReturnIpAddress());
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
                        //StartSearchLoad.Opacity = 100;
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

        private void AddingPc(object sender, bool status)
        {
            if (status)
            {
                IpParametrs remotePc = (IpParametrs) sender;
                
                Thread myThread = new Thread(new ParameterizedThreadStart(_myNetwork.AddingNewPcInList));
                myThread.Start(remotePc);
                
                Initializing_PC(remotePc.ReturnNameInNetwork(),remotePc.ReturnIpAddress());
            }
        }

        /// <summary>
        /// Initializing the found PC/Инициализация найденного компьютера
        /// </summary>
        /// <param name="name">Some name for PC/Какое-то имя компьютера</param>
        /// <param name="ip">IP address of this PC/IP адрес этого ПК</param>
        private void Initializing_PC(string name, IPAddress ip)
        {
            Application.Current.Dispatcher.Invoke((Action)delegate {
                RemotePC remotePC = new RemotePC();
                remotePC.namePc = name;
                remotePC.ipPc = ip.ToString();
                remotePC.widthForPicture = 120;
                remotePC.heightForPicture = 120;
                remotePC.maxheightForPictureh = 150;
                remotePC.maxwidthForPicture = 150;
                remotePC.widthAll = 220;
                remotePC.heightAll = 175;
                PlaceForFindedPC.Children.Add(remotePC);  //Display the initialized PC fro screen
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
            System.Diagnostics.Process.GetCurrentProcess().Dispose();
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
    }
}
