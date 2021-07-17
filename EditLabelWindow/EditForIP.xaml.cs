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
    public partial class EditForIp : Window
    {
        public enum Modes
        {
            SimplyIp = 1,
            SubnetIp = 2,
            BroadcastIp = 3
        }

        readonly IPAddress _iPAddress;
        readonly Modes _modes;
        public EditForIp()
        {
            InitializeComponent();
            ConvertIp(_iPAddress);
        }
        public EditForIp(IPAddress originalIp)
        {
            InitializeComponent();
            _iPAddress = originalIp;
            ConvertIp(_iPAddress);
        }

        public EditForIp(IPAddress originalIp, Modes value)
        {
            InitializeComponent();
            _iPAddress = originalIp;
            ConvertIp(_iPAddress);
            _modes = value;
        }
        private void Accept_Click(object sender, RoutedEventArgs e)
        {

            if (_modes == Modes.SimplyIp)
            {
                try
                {
                    bool status = true;
                    byte first = byte.Parse(renameBox1.Text);
                    byte second = byte.Parse(renameBox2.Text);
                    byte third = byte.Parse(renameBox3.Text);
                    byte fourth = byte.Parse(renameBox4.Text);

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

            if (_modes == Modes.SubnetIp)
            {
                try
                {
                    _pred = -1;
                    bool status = true;  

                    byte first = byte.Parse(renameBox1.Text);
                    byte second = byte.Parse(renameBox2.Text);
                    byte third = byte.Parse(renameBox3.Text);
                    byte fourth = byte.Parse(renameBox4.Text);

                    if (!CheckingCorrect(first))
                    {
                        MessageBox.Show("Исправьте первый октет!");
                        status = false;
                    }
                    if (!CheckingCorrect(second))
                    {
                        MessageBox.Show("Исправьте второй октет!");
                        status = false;
                    }
                    if (!CheckingCorrect(third))
                    {
                        MessageBox.Show("Исправьте третий октет!");
                        status = false;
                    }
                    if (!CheckingCorrect(fourth))
                    {
                        MessageBox.Show("Исправьте четвертый октет!");
                        status = false;
                    }


                    if (status)
                    {
                        this.DialogResult = true;
                    }
                }
                catch
                {
                    MessageBox.Show("Ошибка в конвертировании октетов!");
                }
            }
        }

        private void ConvertIp (IPAddress value)
        {
            try
            {
                if (value != null)
                {
                    byte[] ipAdressBytes = value.GetAddressBytes();
                    renameBox1.Text = ipAdressBytes[0].ToString();
                    renameBox2.Text = ipAdressBytes[1].ToString();
                    renameBox3.Text = ipAdressBytes[2].ToString();
                    renameBox4.Text = ipAdressBytes[3].ToString();
                }
                else
                {
                    renameBox1.Text = "0";
                    renameBox2.Text = "0";
                    renameBox3.Text = "0";
                    renameBox4.Text = "0";
                }
            }
            catch
            {
                MessageBox.Show("Ошибка в конвертировании ip в string");
            }
        }


        int _pred = -1;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="pred"> Значение предыдущего октета либо 1 если число 255 либо 0 если другое</param>
        /// <returns></returns>
        private bool CheckingCorrect(int value)
        {
            try
            {
                if (value < 0 || value > 255)
                {
                    throw new ArgumentOutOfRangeException($"{value} должно быть от 0 до 255");
                }
                if (_pred == 0 && value == 0)
                {
                    return true;
                }
                if (_pred != 0 && value == 0)
                {
                    _pred = 0;
                    return true;
                }
                if (_pred != 0)
                {
                    for (int i = 7; i != -1; i--)
                    {
                        value = value - (int)Math.Pow(2, i);
                        if (value == 0 && i > 0)
                        {
                            _pred = 0;
                            return true;
                        }
                        else if (value == 0 && i == 0)
                        {
                            _pred = 1;
                            return true;
                        }
                        else if (value < 0)
                        {
                            return false;
                        }
                    }
                }
                return false;
            }
            catch
            {
                MessageBox.Show("Ошибка проверки на правильность ввода маски подсети!");
                return false;
            }
        }

        public IPAddress Info()
        {
            try
            {
                string address = renameBox1.Text + "." + renameBox2.Text + "." + renameBox3.Text + "." + renameBox4.Text;
                IPAddress ip = IPAddress.Parse(address);
                return ip;
            }
            catch
            {
                MessageBox.Show("Ошибка в преобразовании string в ip");
                return null;
            }
        }
    }
}
