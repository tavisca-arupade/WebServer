using System;
using System.Net.Sockets;
using System.Threading;

namespace WebServer
{
    public class HTTPListener
    {
        private TcpListener _listener;
        private int _port = 8080;
        private Dispatcher _dispatcher;
        private Socket socket;

        public HTTPListener()
        {
            _dispatcher = new Dispatcher();
            try
            {
                _listener = new TcpListener(_port);
                _listener.Start();
                Thread thread = new Thread(StartListening);
                thread.Start();
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public void StartListening()
        {
            try
            {
                socket = _listener.AcceptSocket();

                new Thread(_ =>
                {
                    _dispatcher.HandleRequest(socket);
                    socket.Shutdown(SocketShutdown.Both);
                    socket.Close();
                });
            }
            catch(SocketException se)
            {
                Console.WriteLine(se);
            }

        }
    }
}