using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyNetworkInterface
{
    public class IpMethods : IpParametrs
    {
        public IpMethods()
        {
            
        }
        public IpMethods(IpParametrs currentPc)
        {
            _currentPc = currentPc;
        }
        
        const int LocalPort = 8010; // порт для приема информации
        const int RemotePort = 8010; // порт для отправки информации

        public delegate void ReceivedInformationHandler(object sender, bool status);
        public event ReceivedInformationHandler ReceivedInformationEvent;
        
        
        private List<IpParametrs> _pcInNetwork = new List<IpParametrs>();
        
        private IpParametrs _currentPc;

        public void AddingNewPcInList(object pc)
        {
            // Переменная, найдено ли совпадение в листе
            bool status = false;

            IpParametrs _pc = (IpParametrs) pc;
            
            foreach(IpParametrs element in _pcInNetwork)
            {
                if (element.ReturnIpAddress() == _pc.ReturnIpAddress())
                {
                    status = true;
                    break;
                }
            }

            if (!status)
            {
                _pcInNetwork.Add(_pc);
            }
        }

        public List<IpParametrs> ReturnListPcInNetwork()
        {
            return _pcInNetwork;
        }

        public bool FindningInformationAboutPcInList(IpParametrs recivedPc)
        {
            foreach (var pc in _pcInNetwork)
            {
                if (pc?.ReturnIpAddress() != null && pc?.ReturnIpAddress() != recivedPc?.ReturnIpAddress())
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Отправление на широковещательный адрес сообщения
        /// </summary>
        public void SendBroadcastOfferToConnect()
        {
            try
            {
                while (true)
                {
                    //foreach (var localaddress in CurentPC)
                    //string BroadIP = "192.168.0.255";
                    //{
                    // создаем соект для работы по пратоколу UDP, в сети Internet, для передачи дейтаграмных сообщений

                    if (_currentPc is null)
                    {
                        throw new ArgumentNullException(nameof(_currentPc));
                    }

                    if (_currentPc.ReturnNameInNetwork() is null)
                    {
                        throw new ArgumentNullException(nameof(_currentPc.ReturnNameInNetwork));
                    }

                    if (_currentPc.ReturnIpAddress() is null)
                    {
                        throw new ArgumentNullException(nameof(_currentPc.ReturnIpAddress));
                    }

                    if (_currentPc.ReturnSubnetMask() is null)
                    {
                        throw new ArgumentNullException(nameof(_currentPc.ReturnSubnetMask));
                    }

                    if (_currentPc.ReturnBroadcastAddress() is null)
                    {
                        throw new ArgumentNullException(nameof(_currentPc.ReturnBroadcastAddress));
                    }

                    Socket socketForBroadcasting =
                        new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                    IPAddress broadcast = _currentPc?.ReturnBroadcastAddress();

                    var message = _currentPc.ReturnNameInNetwork();


                    byte[] buf = Encoding.ASCII.GetBytes(message); // кодируем сообщение из строки в битовый массив
                    IPEndPoint broadcastAddress = new IPEndPoint(broadcast,
                        RemotePort); // создаем полыный адрес получателя, тоесть добавляем к IP еще и прот

                    socketForBroadcasting.SendTo(buf, broadcastAddress); // отправлем сообщение на адрес получателя
                    // Console.WriteLine("Message was sent to the broadcast address");
                    Thread.Sleep(5000);

                    //}
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void ReciveBroadcastOffer()
        {
            UdpClient listener = new UdpClient(LocalPort); // для прослушивания сообщений udp приходящих на локальный порт
            IPEndPoint groupEp = new IPEndPoint(IPAddress.Any, LocalPort); // адрес приема, для приема всех сообщений
            try
            {
                while (true)
                {
                    // Console.WriteLine("Waiting for broadcast");
                    byte[] bytes = listener.Receive(ref groupEp); // получаем сообщение

                    IpParametrs recivedPc = new IpParametrs();
                    var name = Encoding.ASCII.GetString(bytes, 0, bytes.Length);

                    recivedPc.SetNameInNetwork(name);
                    recivedPc.SetIpAddress(groupEp.Address);
                    recivedPc.SetBroadcastAddress(_currentPc.ReturnBroadcastAddress());
                    recivedPc.SetSubnetMask(_currentPc.ReturnSubnetMask());
                    
                    // Вызов события при успешном определении пк
                    ReceivedInformationEvent?.Invoke(recivedPc, true);
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine(e);
                ReceivedInformationEvent?.Invoke(null, false);
            }
            finally
            {
                listener.Close();
            }
        }
        

    }

}
