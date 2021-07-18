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

        public delegate void PcExistInListHandler(IpParametrs pc, StatusExists status);
        public event PcExistInListHandler StatusAddedInList;

        /// <summary>
        /// Добавляет в List пк.
        /// </summary>
        /// <param name="pc"></param>
        public void AddingNewPcInList(object pc)
        {
            // Переменная, найдено ли совпадение в листе
            bool status = false;

            IpParametrs _pc = (IpParametrs) pc;

            status = ExistPcInList(_pc);
            
            if (status)
            {
                StatusAddedInList?.Invoke(_pc, StatusExists.Exists);
            }
            
            if (_pc.ReturnIpAddress() == _currentPc.ReturnIpAddress())
            {
                StatusAddedInList?.Invoke(_pc, StatusExists.LocalPc);
            }
            else if (!status)
            {
                // Добавляем в List
                _pcInNetwork.Add(_pc);
                StatusAddedInList?.Invoke(_pc, StatusExists.Added);
            }
        }

        public List<IpParametrs> ReturnListPcInNetwork()
        {
            return _pcInNetwork;
        }

        public bool ExistPcInList(IpParametrs recivedPc)
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
                    if (CurrentPc is null)
                    {
                        throw new ArgumentNullException(nameof(CurrentPc));
                    }

                    if (CurrentPc.ReturnNameInNetwork() is null)
                    {
                        throw new ArgumentNullException(nameof(CurrentPc.ReturnNameInNetwork), "Не получено имя локального пк.");
                    }

                    if (CurrentPc.ReturnIpAddress() is null)
                    {
                        throw new ArgumentNullException(nameof(CurrentPc.ReturnIpAddress), "Не получен ip адрес локального пк.");
                    }

                    if (CurrentPc.ReturnSubnetMask() is null)
                    {
                        throw new ArgumentNullException(nameof(CurrentPc.ReturnSubnetMask),"Не получена маска подсети локального пк.");
                    }

                    if (CurrentPc.ReturnBroadcastAddress() is null)
                    {
                        throw new ArgumentNullException(nameof(CurrentPc.ReturnBroadcastAddress), "Не получен широковещательный адрес локального пк.");
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

        /// <summary>
        /// Получает и обрабатывает сообщения с широковещательного адреса
        /// </summary>
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
