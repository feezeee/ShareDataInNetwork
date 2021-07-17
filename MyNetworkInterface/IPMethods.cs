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
    public class IpMethods : IpParametrs
    {

        private List<IpParametrs> _pcInNetwork = new List<IpParametrs>();

        public void AddingNewPcInList(IpParametrs pc)
        {
            _pcInNetwork.Add(pc);
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

    }

}
