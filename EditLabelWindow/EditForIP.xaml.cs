using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
    public partial class EditForIP : Window
    {
        public enum Modes
        {
            SimplyIP = 1,
            SubnetIP = 2,
            BroadcastIP = 3
        }

        IPAddress iPAddress;
        Modes modes;
        public EditForIP()
        {
            InitializeComponent();
            ConvertIp(iPAddress);
        }
        public EditForIP(IPAddress originalIP)
        {
            InitializeComponent();
            iPAddress = originalIP;
            ConvertIp(iPAddress);
        }

        public EditForIP(IPAddress originalIP, Modes value)
        {
            InitializeComponent();
            iPAddress = originalIP;
            ConvertIp(iPAddress);
            modes = value;
        }
        private void Accept_Click(object sender, RoutedEventArgs e)
        {

            if (modes == Modes.SimplyIP)
            {
                try
                {
                    bool status = true;
                    int first = int.Parse(renameBox1.Text);
                    int second = int.Parse(renameBox2.Text);
                    int third = int.Parse(renameBox3.Text);
                    int fourth = int.Parse(renameBox2.Text);

                    if (first < 0 || first > 255)
                    {
                        MessageBox.Show("Исправьте первый октет!");
                        status = false;
                    }
                    if (second < 0 || second > 255)
                    {
                        MessageBox.Show("Исправьте второй октет!");
                        status = false;
                    }
                    if (third < 0 || third > 255)
                    {
                        MessageBox.Show("Исправьте третий октет!");
                        status = false;
                    }
                    if (fourth < 0 || fourth > 255)
                    {
                        MessageBox.Show("Исправьте четвертый октет!");
                        status = false;
                    }

                    if (status) this.DialogResult = true;
                }
                catch
                {
                    MessageBox.Show("Ошибка в конвертировании октетов!");
                }
            }

            if (modes == Modes.SubnetIP)
            {
                try
                {
                    
                    bool status = true;
                    byte first = byte.Parse(renameBox1.Text);                    
                    byte second = byte.Parse(renameBox2.Text);
                    byte third = byte.Parse(renameBox3.Text);
                    byte fourth = byte.Parse(renameBox2.Text);

                    CheckingCorrect(first);

                    if (first < 0 || first > 255)
                    {
                        MessageBox.Show("Исправьте первый октет!");
                        status = false;
                    }
                    if (second < 0 || second > 255)
                    {
                        MessageBox.Show("Исправьте второй октет!");
                        status = false;
                    }
                    if (third < 0 || third > 255)
                    {
                        MessageBox.Show("Исправьте третий октет!");
                        status = false;
                    }
                    if (fourth < 0 || fourth > 255)
                    {
                        MessageBox.Show("Исправьте четвертый октет!");
                        status = false;
                    }

                    

                    if (status) this.DialogResult = true;
                }
                catch
                {
                    MessageBox.Show("Ошибка в конвертировании октетов!");
                }
            }
        }

        private void ConvertIp (IPAddress value)
        {
            byte[] ipAdressBytes = value.GetAddressBytes();
            renameBox1.Text = ipAdressBytes[0].ToString();
            renameBox2.Text = ipAdressBytes[1].ToString();
            renameBox3.Text = ipAdressBytes[2].ToString();
            renameBox4.Text = ipAdressBytes[3].ToString();
        }

        private bool CheckingCorrect(int value)
        {
            bool status = false;
            BitArray strBytes = new BitArray(value);
            for (int i = 0; i < strBytes.Length; i++)
            {
                //if (strBytes[i] == 0 && !false)
                //{
                //    status = true;
                //}
            }
            return true;
        }

        public IPAddress Info()
        {
            string address = renameBox1.Text + "." + renameBox2.Text + "." + renameBox3.Text + "." + renameBox4.Text;
            return IPAddress.Parse(address);
        }
    }
}
