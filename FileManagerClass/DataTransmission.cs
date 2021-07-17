using System;

namespace FileManagerClass
{
    /// <summary>
    /// Класс, отвечающий за передачу данных каким-либо путем
    /// </summary>
    public abstract class DataTransmission
    {
        public delegate void ProcessTransmittingHandler(object sender, double percent);
        public event ProcessTransmittingHandler DataTransferStatus;
        
        public delegate void SendingStartHandler(object sender, bool status);
        public event SendingStartHandler DataSendingStart;
        
        public delegate void SendingEndHandler(object sender, bool status);
        public event SendingEndHandler DataSendingEnd;
        
        public abstract void Transmit(ITransmitThrowNetwork transmitThrowNetwork);
        public abstract void Transmit(ITransmitInConsole transmitInConsole);
    }
}