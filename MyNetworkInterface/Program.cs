using System;

namespace MyNetworkInterface
{
    class Program
    {
        static void Main(string[] args)
        {
            IpMethods iPMethods = new IpMethods();
            iPMethods.GetNetworkInformationAboutPcasync();
            iPMethods.OnGettingNetworkInformationAboutPc += IPMethods_OnGettingNetworkInformationAboutPC;
            System.Threading.Thread.Sleep(5000);
            Console.Read();
        }

        private static void IPMethods_OnGettingNetworkInformationAboutPC(object sender, bool status)
        {
            IpMethods iPMethods = (IpMethods)sender;

            Console.WriteLine(iPMethods.ReturnIpAddress());
            Console.WriteLine(iPMethods.ReturnNameInNetwork());
            Console.WriteLine(iPMethods.ReturnSubnetMask());
            Console.WriteLine(iPMethods.ReturnBroadcastAddress());
        }
    }
}
