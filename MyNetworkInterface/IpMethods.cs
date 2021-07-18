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
        
        const int LocalPort = 8010; // порт для приема информации
        const int RemotePort = 8010; // порт для отправки информации

        public delegate void ReceivedInformationHandler(object sender, bool status);
        public event ReceivedInformationHandler ReceivedInformationEvent;
        
        
        private List<IpParametrs> _pcInNetwork = new List<IpParametrs>();
        
        private IpParametrs _currentPc;

        public IpParametrs CurrentPc
        {
            get => _currentPc;
            set => _currentPc = value;
        }


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
            while (true)
            {
                try
                {
                    //foreach (var localaddress in CurentPC)
                    //string BroadIP = "192.168.0.255";
                    //{
                    // создаем соект для работы по пратоколу UDP, в сети Internet, для передачи дейтаграмных сообщений

                    if (CurrentPc is null)
                    {
                        throw new ArgumentNullException(nameof(CurrentPc));
                    }

                    if (CurrentPc.ReturnNameInNetwork() is null)
                    {
                        throw new ArgumentNullException(nameof(CurrentPc.ReturnNameInNetwork));
                    }

                    if (CurrentPc.ReturnIpAddress() is null)
                    {
                        throw new ArgumentNullException(nameof(CurrentPc.ReturnIpAddress));
                    }

                    if (CurrentPc.ReturnSubnetMask() is null)
                    {
                        throw new ArgumentNullException(nameof(CurrentPc.ReturnSubnetMask));
                    }

                    if (CurrentPc.ReturnBroadcastAddress() is null)
                    {
                        throw new ArgumentNullException(nameof(CurrentPc.ReturnBroadcastAddress));
                    }

                    Socket socketForBroadcasting =
                        new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                    IPAddress broadcast = CurrentPc?.ReturnBroadcastAddress();

                    var message = CurrentPc.ReturnNameInNetwork();


                    byte[] buf = Encoding.ASCII.GetBytes(message); // кодируем сообщение из строки в битовый массив
                    IPEndPoint broadcastAddress = new IPEndPoint(broadcast,
                        RemotePort); // создаем полыный адрес получателя, тоесть добавляем к IP еще и прот

                    socketForBroadcasting.SendTo(buf, broadcastAddress); // отправлем сообщение на адрес получателя
                    // Console.WriteLine("Message was sent to the broadcast address");
                    Thread.Sleep(5000);

                    //}
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
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
                    recivedPc.SetBroadcastAddress(CurrentPc.ReturnBroadcastAddress());
                    recivedPc.SetSubnetMask(CurrentPc.ReturnSubnetMask());
                    
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
