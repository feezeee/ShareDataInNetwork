using System;

namespace MyNetworkInterface
{
    class Program
    {
        static void Main(string[] args)
        {
            IPMethods iPMethods = new IPMethods();
            iPMethods.GetNetworkInformationAboutPCASYNC();
            iPMethods.OnGettingNetworkInformationAboutPC += IPMethods_OnGettingNetworkInformationAboutPC;
            System.Threading.Thread.Sleep(5000);
            Console.Read();
        }

        private static void IPMethods_OnGettingNetworkInformationAboutPC(object sender, bool status)
        {
            IPMethods iPMethods = (IPMethods)sender;

            Console.WriteLine(iPMethods.ReturnIpAddress());
            Console.WriteLine(iPMethods.ReturnNameInNetwork());
            Console.WriteLine(iPMethods.ReturnSubnetMask());
            Console.WriteLine(iPMethods.ReturnBroadcastAddress());
        }
    }
}
