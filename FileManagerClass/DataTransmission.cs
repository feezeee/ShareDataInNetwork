using System;
using MyNetworkInterface;

namespace FileManagerClass
{
    /// <summary>
    /// Класс, отвечающий за передачу данных каким-либо путем
    /// </summary>
    public abstract class DataTransmission
    {
        public delegate void ProcessTransmittingHandler(object sender, double percent);
        /// <summary>
        /// Процент загрузки данных
        /// </summary>
        public event ProcessTransmittingHandler DataTransferStatus;
        
        public delegate void SendingStartHandler(object sender, bool status);
        /// <summary>
        /// Данные начали отправлятся
        /// </summary>
        public event SendingStartHandler DataSendingStart;
        
        public delegate void SendingEndHandler(object sender, bool status);
        /// <summary>
        /// Данные закончили отправлятся
        /// </summary>
        public event SendingEndHandler DataSendingEnd;
        
        
        public abstract void Transmit(ITransmitThrowNetwork transmitThrowNetwork, IpParametrs pcLocal, IpParametrs pcRemote, string pathRemote);
        public abstract void Transmit(ITransmitInConsole transmitInConsole);
    }
}