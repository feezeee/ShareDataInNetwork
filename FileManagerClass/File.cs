using System;
using MyNetworkInterface;

namespace FileManagerClass
{
    /// <summary>
    /// Какой-либо файл, отправить каким-либо способом
    /// </summary>
    public class File : DataTransmission
    {
        private int _sizeOfFile;
        private string _nameOfFile;
        private string _path;
        private IpParametrs _currentPc;
        private IpParametrs _remotePc;

        public File() : this(-1)
        {
            
        }
        
        public File(int sizeOfFile) : this(sizeOfFile, null)
        {
            SizeOfFile = sizeOfFile;
        }
        public File(int sizeOfFile, string nameOfFile) : this(sizeOfFile, nameOfFile, null)
        {
            SizeOfFile = sizeOfFile;
            NameOfFile = nameOfFile;
        }
        public File(int sizeOfFile, string nameOfFile, string path) : this(sizeOfFile, nameOfFile, path, null)
        {
            SizeOfFile = sizeOfFile;
            NameOfFile = nameOfFile;
            Path = path;
        }

        public File(int sizeOfFile, string nameOfFile, string path, IpParametrs currentPc) : 
            this(sizeOfFile, nameOfFile, path, currentPc, null)
        {
            SizeOfFile = sizeOfFile;
            NameOfFile = nameOfFile;
            Path = path;
            _currentPc = currentPc;
        }
        public File(int sizeOfFile, string nameOfFile, string path, IpParametrs currentPc, IpParametrs remotePc)
        {
            SizeOfFile = sizeOfFile;
            NameOfFile = nameOfFile;
            Path = path;
            _currentPc = currentPc;
            _remotePc = remotePc;
        }

        /// <summary>
        /// Размер файла
        /// </summary>
        public int SizeOfFile
        {
            get => _sizeOfFile;
            set => _sizeOfFile = value;
        }

        /// <summary>
        /// Имя файла
        /// </summary>
        public string NameOfFile
        {
            get => _nameOfFile;
            set => _nameOfFile = value;
        }
        
        /// <summary>
        /// Путь к файлу
        /// </summary>
        public string Path
        {
            get => _path;
            set => _path = value;
        }


        public override void Transmit(ITransmitThrowNetwork transmitThrowNetwork)
        {
            transmitThrowNetwork?.TransmitThrowNetwork();
        }

        public override void Transmit(ITransmitInConsole transmitInConsole)
        {
            throw new NotImplementedException();
        }
    }
}