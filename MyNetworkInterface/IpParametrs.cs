using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace MyNetworkInterface
{
    
    public class IpParametrs
    {
        public IpParametrs()
        {

        }
        public IpParametrs(string nameInNetwork, IPAddress ipAddress)
        {
            _nameInNetwork = nameInNetwork;
            _ipAddress = ipAddress;
        }
        public IpParametrs(string nameInNetwork, IPAddress ipAddress, IPAddress broadcastAddress)
        {
            _nameInNetwork = nameInNetwork;
            _ipAddress = ipAddress;
            _broadcastAddress = broadcastAddress;
        }
        public IpParametrs(string nameInNetwork, IPAddress ipAddress, IPAddress broadcastAddress, IPAddress subnetMask)
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

        public delegate void GettingIpAddressCurentPc(object sender, bool status);
        public event GettingIpAddressCurentPc OnGettingIpAddressCurentPc;

        public delegate void GettingSubnetMask(object sender, bool status);
        public event GettingSubnetMask OnGettingSubnetMask;

        public delegate void GettingBroadcastAddressCurentNetwork(object sender, bool status);
        public event GettingBroadcastAddressCurentNetwork OnGettingBroadcastAddressCurentNetwork;

        public delegate void GettingNetworkInformationAboutPc(object sender, bool status);
        public event GettingNetworkInformationAboutPc OnGettingNetworkInformationAboutPc;


        public async void GetNetworkInformationAboutPcasync()
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
                     OnGettingNetworkInformationAboutPc?.Invoke(this._nameInNetwork, false);
                 }
                     
                 if (status)
                 {
                     try
                     {
                         GetIpAddressCurentPc();
                     }
                     catch
                     {
                         status = !status;
                         OnGettingNetworkInformationAboutPc?.Invoke(this._ipAddress, false);
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
                         OnGettingNetworkInformationAboutPc?.Invoke(this._subnetMask, false);
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
                         OnGettingNetworkInformationAboutPc?.Invoke(this._broadcastAddress, false);
                     }
                 }

                 if (status) OnGettingNetworkInformationAboutPc?.Invoke(this, true);
             });
        }
        
        public void GetNetworkInformationAboutPc()
        {
            bool status = true;            

            try
            {
                GetCurrentHostName();
            }
            catch
            {

                OnGettingNetworkInformationAboutPc?.Invoke(this._nameInNetwork, false);
            }

            if (status)
            {
                try
                {
                    GetIpAddressCurentPc();
                }
                catch
                {
                    status = !status;
                    OnGettingNetworkInformationAboutPc?.Invoke(this._ipAddress, false);
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
                    OnGettingNetworkInformationAboutPc?.Invoke(this._subnetMask, false);
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
                    OnGettingNetworkInformationAboutPc?.Invoke(this._broadcastAddress, false);
                }
            }

            if (status) OnGettingNetworkInformationAboutPc?.Invoke(this, true);
            else OnGettingNetworkInformationAboutPc?.Invoke(this, false);
        }


        /// <summary>
        /// Get name pc ASYNC
        /// </summary>
        /// <returns></returns>
        /// 
        public async void GetCurrentHostNameAsync()
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
        public async void GetIpAddressCurentPcasync()
        {
            await Task.Run(() => GetIpAddressCurentPc());
        }

        /// <summary>
        /// Get IP address curent pc
        /// </summary>
        /// <returns></returns>
        public void GetIpAddressCurentPc()
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
                    OnGettingIpAddressCurentPc?.Invoke(ipAddress.ToString(), true);
                }     
            }
            catch
            {
                SetIpAddress(ipAddress);
                OnGettingIpAddressCurentPc?.Invoke("Ошибка получения ip адреса.", false);                
            }
        }


        /// <summary>
        /// Set SubnetMask ASYNC
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public async void GetSubnetMaskAsync()
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
                    foreach (UnicastIPAddressInformation unicastIpAddressInformation in adapter.GetIPProperties().UnicastAddresses)
                    {
                        if (unicastIpAddressInformation.Address.AddressFamily == AddressFamily.InterNetwork && address.Equals(unicastIpAddressInformation.Address))
                        {
                            SetSubnetMask(unicastIpAddressInformation.IPv4Mask);
                            OnGettingSubnetMask?.Invoke(unicastIpAddressInformation.IPv4Mask.ToString(), true);
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
        public async void GetBroadcastAddressCurentNetworkAsync()
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