using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SelfHosted
{
    public static class Utilities
    {
        public enum HostStatus
        {
            Starting,
            Running
        }
    }
    public class PortNumberServer
    {
        #region private fields

        private readonly string _portNumber;
        private string _pipeName;
        private Thread _pipeThread;

        #endregion private fields

        /// <summary>
        /// Pipe base name. The real pipe name combines the base name with the process-id
        /// </summary>
        public const string PipeBaseName = "portNumberPipe";

        /// <summary>
        /// Pipe name used for the named pipe server
        /// </summary>
        public string PipeName => _pipeName;

        /// <summary>
        /// Initializes a new instance of the PortNumberServer class with the specified port number string. 
        /// </summary>
        /// <param name="portNumber">Port number string that will be send after client connection</param>
        /// 
        /// <remarks>
        /// This constructor can be used if server and client are instantiate 
        /// in different processes. The pipe name gets the process-id as unique part.
        /// </remarks>
        public PortNumberServer(string portNumber)
        {
            _portNumber = portNumber;
            _pipeName = PipeBaseName + Process.GetCurrentProcess().Id;
        }

        /// <summary>
        /// Initializes a new instance of the PortNumberServer class with the specified port number string. 
        /// </summary>
        /// <param name="portNumber">Port number string that will be send after client connection</param>
        /// <param name="uniqeId"></param>
        /// 
        /// <remarks>
        /// This constructor can be used if server and client are instantiate 
        /// in same process. The pipe name gets a Guid as unique part.
        /// </remarks>
        public PortNumberServer(string portNumber, Guid uniqeId)
        {
            _portNumber = portNumber;
            _pipeName = PipeBaseName + uniqeId;
        }
        /// <summary>
        /// Starts the server
        /// </summary>
        public void Start()
        {
            _pipeThread = new Thread(Server);
            _pipeThread.Start();
        }
        /// <summary>
        /// Stops the server
        /// </summary>
        public void Stop()
        {
            _pipeThread?.Abort();
            // Note: Known issue. When pipe was never connected NamedPipeServerStream:WaitForConnection() blocks Thread.Abort(). 
            //       So this is a workaround.
            using (var npcs = new NamedPipeClientStream(".", PipeName, PipeDirection.In))
            {
                try
                {
                    npcs.Connect(100);
                }
                catch (TimeoutException)
                {
                    // Note: Possible use case. When pipe was connected and afterwards disconnected
                }
            }
        }

        #region private methods
        private void Server()
        {
            try
            {
                using (var pipeServer = new NamedPipeServerStream(PipeName, PipeDirection.Out))
                {
                    var ss = new StreamString(pipeServer);
                    Console.WriteLine($"PortNumberServer Pipe {PipeName} is running.");
                    while (true)
                    {
                        pipeServer.WaitForConnection();
                        ss.WriteString(_portNumber);
                        pipeServer.WaitForPipeDrain();
                        pipeServer.Disconnect();
                    }
                }
            }
            catch (ThreadAbortException)
            {
                Console.WriteLine($"PortNumberServer Pipe {PipeName} stops.");
            }
        }
        #endregion private methods
    }
    public class StreamString : IDisposable
    {
        private readonly Stream _ioStream;
        private readonly UnicodeEncoding _streamEncoding;
        private readonly StreamReader _reader;
        private readonly StreamWriter _writer;

        public const int BlockSize = 256;

        public StreamString(Stream ioStream)
        {
            _ioStream = ioStream;
            _streamEncoding = new UnicodeEncoding();

            if (_ioStream.CanRead)
            {
                _reader = new StreamReader(_ioStream, _streamEncoding);

            }
            if (_ioStream.CanWrite)
            {
                _writer = new StreamWriter(_ioStream, _streamEncoding);
            }
        }

        public void Dispose()
        {
            _reader?.Dispose();
            _writer?.Dispose();
        }

#if false
        // This is alternative code that needs to be tried out

        public string ReadString()
        {
            return _reader.ReadLine();
        }

        public void WriteString(string outString)
        {
            _writer.Write(outString);
            _writer.Flush();
        }
#else

        public string ReadString()
        {
            if (_ioStream.CanRead == false)
            {
                return null;
            }
            var len = _ioStream.ReadByte() * BlockSize;
            if (len < 0)
            {
                return null;
            }
            len += _ioStream.ReadByte();
            var inBuffer = new byte[len];
            _ioStream.Read(inBuffer, 0, len);

            return _streamEncoding.GetString(inBuffer);
        }

        public int WriteString(string outString)
        {
            var outBuffer = _streamEncoding.GetBytes(outString);
            var len = outBuffer.Length;
            if (len > ushort.MaxValue)
            {
                len = ushort.MaxValue;
            }
            _ioStream.WriteByte((byte)(len / BlockSize));
            _ioStream.WriteByte((byte)(len & (BlockSize - 1)));
            _ioStream.Write(outBuffer, 0, len);
            _ioStream.Flush();

            return outBuffer.Length + 2;
        }
#endif
    }
    public class HostStatusServer : IDisposable
    {
        #region private fields

        private Thread _pipeThread;
        private readonly object _thisLock = new object();
        private HostStatus _status = HostStatus.Starting;
        private readonly string _pipeName;
        private readonly NamedPipeServerStream _pipeServerStream;

        #endregion private fields

        /// <summary>
        /// Pipe base name. The real pipe name combines the base name with the process-id or an other unique id
        /// </summary>
        public const string PipeBaseName = "hostStatusPipe";

        /// <summary>
        /// Host Status controlled by the instantiating application
        /// </summary>
        public HostStatus Status
        {
            get
            {
                lock (_thisLock)
                {
                    return _status;
                }
            }
            set
            {
                lock (_thisLock)
                {
                    _status = value;
                }
            }
        }

        /// <summary>
        /// Pipe name used for the named pipe server
        /// </summary>
        public string PipeName => _pipeName;

        /// <summary>
        /// Initializes a new instance of the HostStatusServer class with a unique pipe name
        /// as composite of PipeBaseName and process-id
        /// </summary>
        /// 
        /// <remarks>
        /// This constructor can be used if server and client are instantiate 
        /// in different processes. The pipe name gets the process-id as unique part.
        /// </remarks>
        public HostStatusServer()
        {
            _pipeName = PipeBaseName + Process.GetCurrentProcess().Id;
            _pipeServerStream = new NamedPipeServerStream(_pipeName, PipeDirection.Out);
        }

        /// <summary>
        /// Initializes a new instance of the HostStatusServer class with a unique pipe name
        /// as composite of PipeBaseName and uniqeId
        /// </summary>
        /// <param name="uniqeId"></param>
        /// 
        /// <remarks>
        /// This constructor can be used if server and client are instantiate 
        /// in same process. The pipe name gets a Guid as unique part.
        /// </remarks>
        public HostStatusServer(Guid uniqeId)
        {
            _pipeName = PipeBaseName + uniqeId;
            _pipeServerStream = new NamedPipeServerStream(_pipeName, PipeDirection.Out);
        }

        /// <summary>
        /// Starts the server
        /// </summary>
        public void Start()
        {
            _pipeThread = new Thread(Server);
            _pipeThread.Start();
        }
        /// <summary>
        /// Stops the server
        /// </summary>
        public void Stop()
        {
            _pipeThread?.Abort();
            _pipeServerStream.Dispose();

            // Note: Known issue. When pipe was never connected NamedPipeServerStream:WaitForConnection() blocks Thread.Abort(). 
            //       So this is a workaround.
            using (var npcs = new NamedPipeClientStream(".", _pipeName, PipeDirection.In))
            {
                try
                {
                    npcs.Connect(100);
                }
                catch (TimeoutException)
                {
                    // Note: Possible use case. When pipe was connected and afterwards disconnected
                }
            }

        }
        /// <summary>
        /// Implements IDisposable interface
        /// </summary>
        public void Dispose()
        {
            _pipeServerStream.Dispose();
        }


        #region private methods
        private void Server()
        {
            try
            {
                var ss = new StreamString(_pipeServerStream);
                Console.WriteLine($"Pipe {_pipeName} is running.");
                // Todo: split Server() method into different steps with events (BeginWaitForConnection ...)
                _pipeServerStream.WaitForConnection();
                while (true)
                {
                    Console.WriteLine($"Send server status: \"{Status}\"");
                    ss.WriteString(Status.ToString());
                    _pipeServerStream.WaitForPipeDrain();
                }
            }
            catch (ThreadAbortException)
            {
                Console.WriteLine($"Pipe {_pipeName} stops.");
            }
            catch (IOException)
            {
                Console.WriteLine($"Pipe {_pipeName} disconnected.");
            }
        }
        #endregion private methods
    }
}
