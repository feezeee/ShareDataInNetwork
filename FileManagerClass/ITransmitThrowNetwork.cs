using MyNetworkInterface;

namespace FileManagerClass
{
    public interface ITransmitThrowNetwork
    {
        void Transmit(IpParametrs pcLocal,string pathLocal, IpParametrs pcRemote, string pathRemote);
    }
}