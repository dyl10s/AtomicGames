using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Linq;

namespace ai
{
    public class ServerConnection : IServerConnection
    {
        private readonly int Port;
        private readonly MessageSerializer Serializer;
        private TcpClient Socket;
        private TcpListener Listener;
        private StreamReader Reader;
        private StreamWriter Writer;

        public ServerConnection(int port, MessageSerializer serializer)
        {
            Port = port;
            Serializer = serializer;
        }

        public void AcceptConnection()
        {
            Listener = new TcpListener(IPAddress.Any, Port);
            Listener.Start();

            Console.WriteLine("Listening on port " + Port);
            Socket = Listener.AcceptTcpClient();
            Console.WriteLine("Server connected!");

            var stream = Socket.GetStream();
            Reader = new StreamReader(stream);
            Writer = new StreamWriter(stream);
        }

        public void Close()
        {
            Socket?.Dispose();
            Listener?.Stop();
        }

        public GameUpdate ReadUpdate()
        {
            try
            {
                string input = Reader.ReadLine();
                // Console.WriteLine("Received data from server:" + input);
                var update = Serializer.ParseUpdate(input);
                return update;
            }
            catch
            {
                return null;
            }
        }

        public void SendCommands(IEnumerable<AICommand> commandsToSend)
        {
            var message = new AICommandsMessage { Commands = commandsToSend };
            var serialized = Serializer.SerializeAICommandsMessage(message);
            if (commandsToSend.Count() > 0)
            {
                // Console.WriteLine("Writing commands to server: " + serialized);
            }
            Writer.Write(serialized);
            Writer.Flush();
        }
    }
}

