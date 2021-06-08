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
        public MainWindow()
        {
            InitializeComponent();

            IPMethods MyNetwork = new IPMethods();
            IPParametrs currentPC = new IPParametrs();

            currentPC.OnGettingCurrentHostName += CurrentPC_OnGettingCurrentHostName;
            currentPC.OnGettingIpAddressCurentPC += CurrentPC_OnGettingIpAddressCurentPC;
            currentPC.OnGettingSubnetMask += CurrentPC_OnGettingSubnetMask;
            currentPC.OnGettingBroadcastAddressCurentNetwork += CurrentPC_OnGettingBroadcastAddressCurentNetwork;

            currentPC.OnGettingNetworkInformationAboutPC += CurrentPC_OnGettingNetworkInformationAboutPC;

            currentPC.GetNetworkInformationAboutPCASYNC();        



            MyNetwork.AddingNewPCInList(currentPC);

            // Запускаем поток по определению начать поиск пк в сети или нет.
            Thread myThread = new Thread(new ThreadStart(CheckingSearchingStatus));
            myThread.Start(); 
        }

        private void CurrentPC_OnGettingNetworkInformationAboutPC(object sender, bool status)
        {
            
        }

        private void CurrentPC_OnGettingBroadcastAddressCurentNetwork(object sender, bool status)
        {
                Application.Current.Dispatcher.Invoke((Action)delegate
                {
                    broadcastCurrentPc.Content = sender;
                });
             
        }

        private void CurrentPC_OnGettingSubnetMask(object sender, bool status)
        {            
            Application.Current.Dispatcher.Invoke((Action)delegate
            {
                subnetMaskCurrentPc.Content = sender;
            });
        }

        private void CurrentPC_OnGettingIpAddressCurentPC(object sender, bool status)
        {
                Application.Current.Dispatcher.Invoke((Action)delegate
                {
                    ipCurrentPc.Content = sender;
                });
          
        }

        private void CurrentPC_OnGettingCurrentHostName(object sender, bool status)
        {
                Application.Current.Dispatcher.Invoke((Action)delegate
                {
                    nameCurrentPc.Content = sender;
                });
           
        }




        /// <summary>
        /// This variable shows searching status Эта переменная показывает статус поиска. 
        /// </summary>
        private bool SearchingStatus = false;
        private void Searching_Click(object sender, RoutedEventArgs e)
        {
            SearchingStatus = !SearchingStatus;            
        }


        private bool Search = false;
        private void CheckingSearchingStatus()
        {
            while (true)
            {
                if (SearchingStatus && !Search)
                {
                    Search = true;
                    // запускаем
                }
                if (Search && !SearchingStatus)
                {
                    Search = false;
                    // останавливаем
                }
            }            
        }



        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
