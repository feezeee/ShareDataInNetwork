using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MyNetworkInterface
{
    public class IPMethods : IPParametrs
    {

        private List<IPParametrs> PCInNetwork = new List<IPParametrs>();

        public void AddingNewPCInList(IPParametrs pc)
        {
            PCInNetwork.Add(pc);
        }

        public List<IPParametrs> ReturnListPCInNetwork()
        {
            return PCInNetwork;
        }

        public bool FindningInformationAboutPCInList(IPParametrs recivedPC)
        {
            foreach (var PC in PCInNetwork)
            {
                if (PC?.ReturnIpAddress() != null && PC?.ReturnIpAddress() != recivedPC?.ReturnIpAddress())
                {
                    return true;
                }
            }
            return false;
        }

    }

    public class IPParametrs
    {
        public IPParametrs()
        {

        }
        public IPParametrs(string nameInNetwork, IPAddress ipAddress)
        {
            _nameInNetwork = nameInNetwork;
            _ipAddress = ipAddress;
        }
        public IPParametrs(string nameInNetwork, IPAddress ipAddress, IPAddress broadcastAddress)
        {
            _nameInNetwork = nameInNetwork;
            _ipAddress = ipAddress;
            _broadcastAddress = broadcastAddress;
        }
        public IPParametrs(string nameInNetwork, IPAddress ipAddress, IPAddress broadcastAddress, IPAddress subnetMask)
        {
            _nameInNetwork = nameInNetwork;
            _ipAddress = ipAddress;
            _broadcastAddress = broadcastAddress;
            _subnetMask = subnetMask;
        }

        private string _nameInNetwork;
        private IPAddress _ipAddress;
        private IPAddress _broadcastAddress;
        private IPAddress _subnetMask;

        public string ReturnNameInNetwork()
        {
            return _nameInNetwork;
        }
        public IPAddress ReturnIpAddress()
        {
            return _ipAddress;
        }
        public IPAddress ReturnBroadcastAddress()
        {
            return _broadcastAddress;
        }
        public IPAddress ReturnSubnetMask()
        {
            return _subnetMask;
        }

        public void SetNameInNetwork(string value)
        {
            _nameInNetwork = value;
        }
        public void SetIpAddress(IPAddress value)
        {
            _ipAddress = value;
        }
        public void SetBroadcastAddress(IPAddress value)
        {
            _broadcastAddress = value;
        }
        public void SetSubnetMask(IPAddress value)
        {
            _subnetMask = value;
        }


        public delegate void GettingCurrentHostName(object sender, bool status);
        public event GettingCurrentHostName OnGettingCurrentHostName;

        public delegate void GettingIpAddressCurentPC(object sender, bool status);
        public event GettingIpAddressCurentPC OnGettingIpAddressCurentPC;

        public delegate void GettingSubnetMask(object sender, bool status);
        public event GettingSubnetMask OnGettingSubnetMask;

        public delegate void GettingBroadcastAddressCurentNetwork(object sender, bool status);
        public event GettingBroadcastAddressCurentNetwork OnGettingBroadcastAddressCurentNetwork;

        public delegate void GettingNetworkInformationAboutPC(object sender, bool status);
        public event GettingNetworkInformationAboutPC OnGettingNetworkInformationAboutPC;




        public async void GetNetworkInformationAboutPCASYNC()
        {
             bool status = true;
             await Task.Run(() =>
             {
                 
                     try
                     {
                         GetCurrentHostName();
                     }
                     catch
                     {
                         
                         OnGettingNetworkInformationAboutPC?.Invoke(this._nameInNetwork, false);
                     }

                 if (status)
                 {
                     try
                     {
                         GetIpAddressCurentPC();
                     }
                     catch
                     {
                         status = !status;
                         OnGettingNetworkInformationAboutPC?.Invoke(this._ipAddress, false);
                     }
                 }

                 if (status)
                 {
                     try
                     {
                         GetSubnetMask(ReturnIpAddress());
                     }
                     catch
                     {
                         status = !status;
                         OnGettingNetworkInformationAboutPC?.Invoke(this._subnetMask, false);
                     }
                 }

                 if (status)
                 {
                     try
                     {
                         GetBroadcastAddressCurentNetwork(ReturnIpAddress(), ReturnSubnetMask());
                     }
                     catch
                     {
                         status = !status;
                         OnGettingNetworkInformationAboutPC?.Invoke(this._broadcastAddress, false);
                     }
                 }

                 if (status) OnGettingNetworkInformationAboutPC?.Invoke(this, true);
             });
        }
        
        public void GetNetworkInformationAboutPC()
        {
            bool status = true;            

            try
            {
                GetCurrentHostName();
            }
            catch
            {

                OnGettingNetworkInformationAboutPC?.Invoke(this._nameInNetwork, false);
            }

            if (status)
            {
                try
                {
                    GetIpAddressCurentPC();
                }
                catch
                {
                    status = !status;
                    OnGettingNetworkInformationAboutPC?.Invoke(this._ipAddress, false);
                }
            }

            if (status)
            {
                try
                {
                    GetSubnetMask(ReturnIpAddress());
                }
                catch
                {
                    status = !status;
                    OnGettingNetworkInformationAboutPC?.Invoke(this._subnetMask, false);
                }
            }

            if (status)
            {
                try
                {
                    GetBroadcastAddressCurentNetwork(ReturnIpAddress(), ReturnSubnetMask());
                }
                catch
                {
                    status = !status;
                    OnGettingNetworkInformationAboutPC?.Invoke(this._broadcastAddress, false);
                }
            }

            if (status) OnGettingNetworkInformationAboutPC?.Invoke(this, true);
            else OnGettingNetworkInformationAboutPC?.Invoke(this, false);
        }


        /// <summary>
        /// Get name pc ASYNC
        /// </summary>
        /// <returns></returns>
        /// 
        public async void GetCurrentHostNameASYNC()
        {
            await Task.Run(() => GetCurrentHostName());
            //вызываем делегат
        }

        /// <summary>
        /// Get name pc
        /// </summary>
        /// <returns></returns>
        /// 
        public void GetCurrentHostName()
        {
            string name = "";
            try
            {
                //вызываем делегат

                name = Dns.GetHostName();

                if (name == null || name == "")
                {
                    Random random = new Random();
                    name = "PC " + random.Next(100000, 999999);
                }
            }
            catch
            {
                Random random = new Random();
                name = "ErrorName " + random.Next(100000, 999999); 
            }

            SetNameInNetwork(name);
            OnGettingCurrentHostName?.Invoke(name, true);
        }


        /// <summary>
        /// Get IP address curent pc ASYNC
        /// </summary>
        /// <returns></returns>
        public async void GetIpAddressCurentPCASYNC()
        {
            await Task.Run(() => GetIpAddressCurentPC());
        }

        /// <summary>
        /// Get IP address curent pc
        /// </summary>
        /// <returns></returns>
        public void GetIpAddressCurentPC()
        {
            IPAddress ipAddress = null;
            try
            {
                using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
                {
                    socket.Connect("8.8.8.8", 65530);
                    IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
                    ipAddress = endPoint.Address;
                    SetIpAddress(ipAddress);
                    OnGettingIpAddressCurentPC?.Invoke(ipAddress.ToString(), true);
                }     
            }
            catch
            {
                SetIpAddress(ipAddress);
                OnGettingIpAddressCurentPC?.Invoke("Ошибка получения ip адреса.", false);                
            }
        }


        /// <summary>
        /// Set SubnetMask ASYNC
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public async void GetSubnetMaskASYNC()
        {
            await Task.Run(() => GetSubnetMask(ReturnIpAddress()));
            //вызываем делегат

        }
        /// <summary>
        /// Set SubnetMask
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public void GetSubnetMask(IPAddress address)
        {
            try
            {
                foreach (NetworkInterface adapter in NetworkInterface.GetAllNetworkInterfaces())
                {
                    foreach (UnicastIPAddressInformation unicastIPAddressInformation in adapter.GetIPProperties().UnicastAddresses)
                    {
                        if (unicastIPAddressInformation.Address.AddressFamily == AddressFamily.InterNetwork && address.Equals(unicastIPAddressInformation.Address))
                        {
                            SetSubnetMask(unicastIPAddressInformation.IPv4Mask);
                            OnGettingSubnetMask?.Invoke(unicastIPAddressInformation.IPv4Mask.ToString(), true);
                            break;
                        }
                    }
                }
            }
            catch
            {
                SetSubnetMask(null);
                OnGettingSubnetMask?.Invoke("Fail subnet mask!", false);
            }

            // throw new ArgumentException($"Can't find subnetmask for IP address '{address}'");.
        }


        /// <summary>
        /// Set Broadcast Address in current network
        /// </summary>
        /// <param name="address"></param>
        /// <param name="subnetMask"></param>
        /// <returns></returns>
        public async void GetBroadcastAddressCurentNetworkASYNC()
        {
            await Task.Run(() => GetBroadcastAddressCurentNetwork(ReturnIpAddress(), ReturnSubnetMask()));
            //вызываем делегат

        }

        /// <summary>
        /// Return Broadcast Address in current network
        /// </summary>
        /// <param name="address"></param>
        /// <param name="subnetMask"></param>
        /// <returns></returns>
        public void GetBroadcastAddressCurentNetwork(IPAddress address, IPAddress subnetMask)
        {
            if (address == null)
            {
                OnGettingBroadcastAddressCurentNetwork?.Invoke("Ip address can not be empty.", false);
                throw new ArgumentNullException(nameof(address), "Ip address can not be empty.");
            }
            if (subnetMask == null)
            {
                OnGettingBroadcastAddressCurentNetwork?.Invoke("Subnet mask address can not be empty.", false);
                throw new ArgumentNullException(nameof(subnetMask), "Subnet mask address can not be empty.");
            }
            byte[] ipAdressBytes = address.GetAddressBytes();
            byte[] subnetMaskBytes = subnetMask.GetAddressBytes();

            if (ipAdressBytes.Length != subnetMaskBytes.Length)
                throw new ArgumentException("Lengths of IP address and subnet mask do not match.");

            byte[] broadcastAddress = new byte[ipAdressBytes.Length];
            for (int i = 0; i < broadcastAddress.Length; i++)
            {
                broadcastAddress[i] = (byte)(ipAdressBytes[i] | (subnetMaskBytes[i] ^ 255));
            }

            SetBroadcastAddress(new IPAddress(broadcastAddress));
            OnGettingBroadcastAddressCurentNetwork?.Invoke(new IPAddress(broadcastAddress).ToString(), true);
        }


    }
}
