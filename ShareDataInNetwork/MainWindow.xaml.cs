﻿using System;
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
        IPMethods MyNetwork = new IPMethods();
        IPParametrs currentPC = new IPParametrs();

        public MainWindow()
        {
            InitializeComponent();

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
                    broadcastCurrentPc.MouseLeftButtonDown += BroadcastCurrentPc_MouseLeftButtonDown;
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
                EditLabelWindow.MainWindow EditWindow = new EditLabelWindow.MainWindow(currentPC.ReturnNameInNetwork());
                if (EditWindow.ShowDialog() == true)
                {
                    myLabel.Content = EditWindow.Info;
                    currentPC.SetNameInNetwork(EditWindow.Info);
                }
            }
        }
        private void IpCurrentPc_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Label myLabel = (Label)sender;
                EditLabelWindow.EditForIP EditWindow = new EditLabelWindow.EditForIP(currentPC.ReturnIpAddress(), EditLabelWindow.EditForIP.Modes.SimplyIP);
                if (EditWindow.ShowDialog() == true)
                {
                    myLabel.Content = EditWindow.Info().ToString();
                    currentPC.SetIpAddress(EditWindow.Info());
                }
            }
        }
        private void SubnetMaskCurrentPc_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Label myLabel = (Label)sender;
                EditLabelWindow.EditForIP EditWindow = new EditLabelWindow.EditForIP(currentPC.ReturnSubnetMask(),EditLabelWindow.EditForIP.Modes.SubnetIP);
                if (EditWindow.ShowDialog() == true)
                {
                    myLabel.Content = EditWindow.Info().ToString();
                    currentPC.SetIpAddress(EditWindow.Info());
                }
            }
        }
        private void BroadcastCurrentPc_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                currentPC.GetBroadcastAddressCurentNetworkASYNC();
            }
        }

        #endregion



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
            ResetButton.IsEnabled = false;
            nameCurrentPc.Content = "";
            ipCurrentPc.Content = "";
            subnetMaskCurrentPc.Content = "";
            broadcastCurrentPc.Content = "";
            currentPC.GetNetworkInformationAboutPCASYNC();
        }

        private void MainLoadWindow_Closed(object sender, EventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
    }
}
