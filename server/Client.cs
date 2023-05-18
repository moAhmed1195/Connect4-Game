using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace server
{   
    internal class Client
    {
        public event EventHandler<string> MsgRecrived;
        public BinaryReader reader;
        public BinaryWriter writer;
        public NetworkStream networkStream;

        // modificaion
        public int id;
        public string userName;
        public string room="no room";
        public string status;
        public string color;
        

        public Client(TcpClient tcpClient)
        {
            networkStream = tcpClient.GetStream();
            reader = new BinaryReader(networkStream);
            writer = new BinaryWriter(networkStream);
            ReadMsgs();
        }

        private async void ReadMsgs()
        {
            while (true)
            {
                await Task.Run(async () => await ReadAsync());
            }
        }
        private async Task<string> ReadStringAsync(BinaryReader reader)
        {
            int length = reader.ReadInt32();
            byte[] buffer = new byte[length];
            await networkStream.ReadAsync(buffer, 0, length);
            return Encoding.UTF8.GetString(buffer);
        }

        private async Task ReadAsync()
        {
            try
            {
                string message = await ReadStringAsync(reader);
                if(MsgRecrived != null)
                {
                    MsgRecrived(this, message);
                }
            }
            catch (IOException ex)
            {
                // handle the exception
                // (e.g., the remote host closed the connection)
            }
        }
        public void sendMsg(string message)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(message);
            int length = bytes.Length;
            writer.Write(length);
            writer.Write(bytes);
            writer.Flush();
        }
    }
}
