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
        private IpParametrs _localPc;
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

        public File(int sizeOfFile, string nameOfFile, string path, IpParametrs localPc) : 
            this(sizeOfFile, nameOfFile, path, localPc, null)
        {
            SizeOfFile = sizeOfFile;
            NameOfFile = nameOfFile;
            Path = path;
            _localPc = localPc;
        }
        public File(int sizeOfFile, string nameOfFile, string path, IpParametrs localPc, IpParametrs remotePc)
        {
            SizeOfFile = sizeOfFile;
            NameOfFile = nameOfFile;
            Path = path;
            _localPc = localPc;
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


        public override void Transmit(ITransmitThrowNetwork transmitThrowNetwork, IpParametrs pcLocal, IpParametrs pcRemote, string pathRemote)
        {
            transmitThrowNetwork?.Transmit(pcLocal, Path, pcRemote, pathRemote);
        }

        public override void Transmit(ITransmitInConsole transmitInConsole)
        {
            throw new NotImplementedException();
        }
    }
}