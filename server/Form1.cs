using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace server
{
    public partial class Form1 : Form
    {
        List<Client> clients= new List<Client>();
        Client client;
        TcpClient tcpClient;
        private int ClinetID;

        StreamWriter s_writer;

        public Form1()
        {
            InitializeComponent();
        }
        //recieved Messege From any client
        private void Client_MsgRecrived(object sender, string message)
        {
            string[] words = message.Split('=');
            // first connection and give client and id
            if (words[0] == "username") 
            {
                this.ClientsDataGridView.Rows.Add(client.id, words[1], " ", " ", " "," "," "," "," "," ","0","0"," ");
                for (int i = 0; i < ClientsDataGridView.Rows.Count; i++)
                {
                    client.sendMsg("add=" + ClientsDataGridView.Rows[i].Cells[0].Value.ToString() + '=' + ClientsDataGridView.Rows[i].Cells[1].Value.ToString()+"="+ ClientsDataGridView.Rows[i].Cells[2].Value.ToString() + "=" + ClientsDataGridView.Rows[i].Cells[5].Value.ToString());
                    if (ClientsDataGridView.Rows.Count - i == 1)
                    {
                        for (int j = 0; j < ClientsDataGridView.Rows.Count - 1; j++)
                        {
                            clients[j].sendMsg("add=" + ClientsDataGridView.Rows[i].Cells[0].Value.ToString() + '=' + ClientsDataGridView.Rows[i].Cells[1].Value.ToString()+"=" + ClientsDataGridView.Rows[i].Cells[2].Value.ToString() + "=" + ClientsDataGridView.Rows[i].Cells[5].Value.ToString());
                        }
                    }

                }
            }
            //get click from client
            else if (words[0] == "id") 
            {

                for (int i = 0; i < ClientsDataGridView.Rows.Count; i++)
                {
                    if (this.ClientsDataGridView.Rows[i].Cells[0].Value.ToString() == words[1])
                    {
                        ClientsDataGridView.Rows[i].Cells[3].Value = words[3] + "=" + words[5];
                    }
                }
            }
            // send message from client to client in same room
            else if (words[0] == "message") 
            {
                for (int i = 0; i < ClientsDataGridView.Rows.Count; i++)
                {
                    if (ClientsDataGridView.Rows[i].Cells[0].Value.ToString() == words[3])
                    {
                        ClientsDataGridView.Rows[i].Cells[4].Value = words[1];
                        for (int j = 0; j < ClientsDataGridView.Rows.Count; j++)
                        {
                            if (ClientsDataGridView.Rows[i].Cells[2].Value.ToString() == ClientsDataGridView.Rows[j].Cells[2].Value.ToString() &&
                                 ClientsDataGridView.Rows[j].Cells[5].Value.ToString() == "player" && i != j)
                            {
                                clients[j].sendMsg("RecieveMsg=" + words[1]);

                            }
                        }
                    }
                }

            }
            // play request
            else if (words[0] == "play")
            {
                int checkRooms = 0;
                for (int i = 0; i < clients.Count; i++)
                {
                    if (clients[i].room == words[4]) checkRooms++;  
                }
                if (checkRooms == 0)  // no one in this room
                {
                    for (int i = 0; i < clients.Count; i++)
                    {
                        if (clients[i].id == int.Parse(words[2])) 
                        {   clients[i].room = words[4];
                            clients[i].status = "player";
                        }

                        clients[i].sendMsg("room=" + words[4] + "=id=" + words[2] + "=status=" + "player");

                    }

                    for (int i = 0; i < ClientsDataGridView.Rows.Count; i++)
                    {
                        if (ClientsDataGridView.Rows[i].Cells[0].Value.ToString() == words[2])
                        {
                            ClientsDataGridView.Rows[i].Cells[2].Value = words[4];
                            ClientsDataGridView.Rows[i].Cells[5].Value = "player";
                        }
                    }
                }
                else if (checkRooms == 1)// only one in this room
                {   
                    for (int i = 0; i < clients.Count; i++)
                    {
                        if (clients[i].room == words[4])
                        {
                            
                        clients[i].sendMsg("accept=" + "room=" + words[4] +"=id=" + words[2]);
                            
                        }


                    }
                }
                else if (checkRooms > 1) // full room
                {   
                    MessageBox.Show("full room you can watch");
                }

            }
            // accept  request from player to play
            else if (words[0] == "acceptResponse")
            {
                if (words[1] == "yes")
                {
                    for (int i = 0; i < clients.Count; i++)
                    {
                        if (clients[i].id == int.Parse(words[5]))
                        {
                            clients[i].room = words[7];
                            clients[i].status = "player";
                           
                            if (ClientsDataGridView.Rows[i].Cells[8].Value.ToString()==" ")
                            {
                                clients[i].color = words[9];
                                ClientsDataGridView.Rows[i].Cells[8].Value = words[9];
                            }
                        }

                        clients[i].sendMsg("room=" + words[7] + "=id=" + words[5] + "=status=" + "player");

                    }
                    for (int i = 0; i < ClientsDataGridView.Rows.Count; i++)
                    {
                        if (ClientsDataGridView.Rows[i].Cells[0].Value.ToString() == words[5])
                        {
                            ClientsDataGridView.Rows[i].Cells[2].Value = words[7];
                            ClientsDataGridView.Rows[i].Cells[5].Value = "player";
                            
                        }
                       
                    }
                   
                    for (int i = 0; i < ClientsDataGridView.Rows.Count; i++)
                    {
                        if (ClientsDataGridView.Rows[i].Cells[0].Value.ToString() == words[3])
                        {
                            for (int j = 0; j < ClientsDataGridView.Rows.Count; j++)
                            {
                                if (ClientsDataGridView.Rows[j].Cells[0].Value.ToString() == words[5])
                                {                                        //word[9]
                                    clients[i].sendMsg("player2color=" + ClientsDataGridView.Rows[j].Cells[8].Value.ToString() + "=turn=" + "1" + "=player1color=" + ClientsDataGridView.Rows[i].Cells[8].Value.ToString());
                                    ClientsDataGridView.Rows[j].Cells[6].Value = ClientsDataGridView.Rows[i].Cells[6].Value.ToString();
                                    ClientsDataGridView.Rows[j].Cells[7].Value = ClientsDataGridView.Rows[i].Cells[7].Value.ToString();
                                    ClientsDataGridView.Rows[j].Cells[9].Value = ClientsDataGridView.Rows[i].Cells[9].Value.ToString();
                                    clients[j].sendMsg("drawGame=" + "col=" + ClientsDataGridView.Rows[i].Cells[6].Value.ToString() + "=row=" 
                                                     + ClientsDataGridView.Rows[i].Cells[7].Value.ToString() + "=backColor=" + ClientsDataGridView.Rows[i].Cells[9].Value.ToString() + 
                                                     "=player1Color=" + ClientsDataGridView.Rows[i].Cells[8].Value.ToString()+"=player2Color=" + 
                                                       ClientsDataGridView.Rows[j].Cells[8].Value.ToString());
                                                                                                                                                                                                                                                                                                                                                          //MessageBox.Show(ClientsDataGridView.Rows[i].Cells[6].Value.ToString());
                                }
                            }
                        }
                    }
                 

                }
                else
                {
                    for (int i = 0; i < clients.Count; i++)
                    {
                        if (clients[i].id == int.Parse(words[5]))
                        {
                            
                        clients[i].sendMsg("refused=" + words[7] + "=id=" + words[5]);
                        }


                    }
                    
                }
            }
            // send board Data to Play
            else if (words[0] == "board")
            {
                for (int i = 0; i < ClientsDataGridView.Rows.Count; i++)
                {
                    if (ClientsDataGridView.Rows[i].Cells[0].Value.ToString() == words[2])
                    {
                        ClientsDataGridView.Rows[i].Cells[6].Value = words[4];
                        ClientsDataGridView.Rows[i].Cells[7].Value = words[6];
                        ClientsDataGridView.Rows[i].Cells[9].Value = words[10];
                        if(ClientsDataGridView.Rows[i].Cells[8].Value.ToString() ==" ")
                        {
                            clients[i].color = words[8];
                            ClientsDataGridView.Rows[i].Cells[8].Value = words[8];
                        }
                     
                    }

                }
             }
            // player 1 Turn
            else if (words[0] == "turn1")
            {
                for (int i = 0; i < ClientsDataGridView.Rows.Count; i++)
                {
                    if (ClientsDataGridView.Rows[i].Cells[0].Value.ToString() == words[3])
                    {
                       for(int j = 0; j < ClientsDataGridView.Rows.Count; j++)
                        {   
                            if (ClientsDataGridView.Rows[i].Cells[2].Value.ToString() == ClientsDataGridView.Rows[j].Cells[2].Value.ToString()&&
                                ClientsDataGridView.Rows[i].Cells[5].Value.ToString() =="player" && 
                                ClientsDataGridView.Rows[j].Cells[5].Value.ToString()=="player" && i!=j)
                            {
                                clients[i].sendMsg("disableplayer1turn="+ "0" + "=id=" + clients[i].id);
                                clients[j].sendMsg("playertwoturn=" + "2" + "=id=" + clients[j].id + "=colIndex=" + words[5] + "=rowIndex=" + words[7]);
                                ClientsDataGridView.Rows[i].Cells[3].Value += "="+words[5] + "=" + words[7];
                            }
                            else if (ClientsDataGridView.Rows[i].Cells[2].Value.ToString() == ClientsDataGridView.Rows[j].Cells[2].Value.ToString() &&
                                       ClientsDataGridView.Rows[j].Cells[5].Value.ToString() == "watcher")
                            {
                                clients[j].sendMsg("move1watcher=" + "color=" + ClientsDataGridView.Rows[i].Cells[8].Value.ToString() + "=colIndex=" + words[5] + "=rowIndex=" + words[7]);
                            }
                        }
                    }

                }
            }
            //player 2 Turn
            else if (words[0] == "turn2")
            {
                for (int i = 0; i < ClientsDataGridView.Rows.Count; i++) //player 2
                {
                    if (ClientsDataGridView.Rows[i].Cells[0].Value.ToString() == words[3])
                    {
                        for (int j = 0; j < ClientsDataGridView.Rows.Count ; j++) //player1
                        {   
                            if (ClientsDataGridView.Rows[i].Cells[2].Value.ToString() == ClientsDataGridView.Rows[j].Cells[2].Value.ToString() &&
                                ClientsDataGridView.Rows[i].Cells[5].Value.ToString() =="player" && 
                                ClientsDataGridView.Rows[j].Cells[5].Value.ToString()=="player" && i!=j )
                            {

                                clients[j].sendMsg("playeroneturn=" + "1" + "=id=" + clients[j].id + "=colIndex=" + words[5] + "=rowIndex=" + words[7]);
                                clients[i].sendMsg("disableplayer2turn=" + "0" + "=id=" + clients[i].id);
                                ClientsDataGridView.Rows[i].Cells[3].Value +="="+ words[5] + "=" + words[7];

                            }
                            else if (ClientsDataGridView.Rows[i].Cells[2].Value.ToString() == ClientsDataGridView.Rows[j].Cells[2].Value.ToString() &&
                                      ClientsDataGridView.Rows[j].Cells[5].Value.ToString() == "watcher")
                            {
                                clients[j].sendMsg("move2watcher=" + "color=" + ClientsDataGridView.Rows[i].Cells[8].Value.ToString() + "=colIndex=" + words[5] + "=rowIndex=" + words[7]);
                            }
                        }
                    }

                }
            }
            //player 2 Color
            else if (words[0] == "playerColor")
            {
                for(int i = 0; i < ClientsDataGridView.Rows.Count; i++)
                {
                    if (ClientsDataGridView.Rows[i].Cells[0].Value.ToString() == words[3])
                    {
                        ClientsDataGridView.Rows[i].Cells[8].Value = words[1];
                    }
                }
            }
            //player 1 win
            else if (words[0] == "win1")
            {
                for (int i = 0; i < ClientsDataGridView.Rows.Count; i++)
                {
                    if (ClientsDataGridView.Rows[i].Cells[0].Value.ToString() == words[2])
                    {
                        for (int j = 0; j < ClientsDataGridView.Rows.Count; j++)
                        {
                            if (ClientsDataGridView.Rows[i].Cells[2].Value.ToString() == ClientsDataGridView.Rows[j].Cells[2].Value.ToString() &&
                                ClientsDataGridView.Rows[j].Cells[5].Value.ToString() == "player" &&
                                ClientsDataGridView.Rows[i].Cells[5].Value.ToString() == "player" && i != j)
                            {
                                ClientsDataGridView.Rows[i].Cells[12].Value = DateTime.Now;
                                ClientsDataGridView.Rows[j].Cells[12].Value = DateTime.Now;
                                ClientsDataGridView.Rows[i].Cells[11].Value = $"{int.Parse(ClientsDataGridView.Rows[i].Cells[11].Value.ToString()) + 1}";
                                clients[j].sendMsg("win1=" + "id=" + words[2]);
                            }
                        }
                    }
                }
               

            }
            //player 2 win
            else if (words[0] == "win2")
            {
                for (int i = 0; i < ClientsDataGridView.Rows.Count; i++)
                {
                    if (ClientsDataGridView.Rows[i].Cells[0].Value.ToString() == words[2])
                    {
                        for (int j = 0; j < ClientsDataGridView.Rows.Count; j++)
                        {
                            if (ClientsDataGridView.Rows[i].Cells[2].Value.ToString() == ClientsDataGridView.Rows[j].Cells[2].Value.ToString() &&
                                ClientsDataGridView.Rows[j].Cells[5].Value.ToString() == "player" &&
                                ClientsDataGridView.Rows[i].Cells[5].Value.ToString() == "player" && i != j)
                            {
                                ClientsDataGridView.Rows[i].Cells[12].Value = DateTime.Now;
                                ClientsDataGridView.Rows[j].Cells[12].Value = DateTime.Now;
                                ClientsDataGridView.Rows[i].Cells[11].Value = $"{int.Parse(ClientsDataGridView.Rows[i].Cells[11].Value.ToString()) + 1}";
                                clients[j].sendMsg("win2=" + "id=" + words[2]);
                            }
                        }
                    }
                }
               

            }
            // player 1 Again and save Data
            else if (words[0] == "player1Again")
            {
                for (int i = 0; i < ClientsDataGridView.Rows.Count; i++)
                {
                    if (ClientsDataGridView.Rows[i].Cells[0].Value.ToString() == words[3])
                    {
                        for (int j = 0; j < ClientsDataGridView.Rows.Count; j++)
                        {
                            if (ClientsDataGridView.Rows[i].Cells[2].Value.ToString() == ClientsDataGridView.Rows[j].Cells[2].Value.ToString() &&
                                ClientsDataGridView.Rows[j].Cells[5].Value.ToString() == "player" &&
                                ClientsDataGridView.Rows[i].Cells[5].Value.ToString() == "player" && i != j)
                            {
                                ClientsDataGridView.Rows[i].Cells[10].Value = words[1];
                                if (ClientsDataGridView.Rows[i].Cells[10].Value.ToString() != "0" && ClientsDataGridView.Rows[j].Cells[10].Value.ToString() != "0")
                                { clients[j].sendMsg("player1Again=" + words[1] + "=player2Again=" + ClientsDataGridView.Rows[j].Cells[10].Value.ToString());
                                  clients[i].sendMsg("player2Again=" + ClientsDataGridView.Rows[j].Cells[10].Value.ToString() + "=player1Again=" + ClientsDataGridView.Rows[i].Cells[10].Value.ToString());
                                   

                                    if(ClientsDataGridView.Rows[i].Cells[10].Value.ToString() == "2" || ClientsDataGridView.Rows[j].Cells[10].Value.ToString() == "2")
                                    {
                                        s_writer = File.AppendText(@"results.txt");
                                        s_writer.WriteLine("Matches Results");
                                        s_writer.WriteLine("Player1 name" + ":" + ClientsDataGridView.Rows[i].Cells[1].Value.ToString() + "  " +
                                                           "result"       + ":" + ClientsDataGridView.Rows[i].Cells[11].Value.ToString()+ "  " +
                                                           "Player2 name" + ":" + ClientsDataGridView.Rows[j].Cells[1].Value.ToString() + "  " + 
                                                           "result"       + ":" + ClientsDataGridView.Rows[j].Cells[11].Value.ToString()+ "  " +
                                                           "Date"         + ":" + ClientsDataGridView.Rows[i].Cells[12].Value.ToString());
                                        s_writer.WriteLine(s_writer.NewLine);
                                        s_writer.Close();
                                        ClientsDataGridView.Rows[i].Cells[11].Value = "0";
                                        ClientsDataGridView.Rows[j].Cells[11].Value = "0";
                                    }
                                    ClientsDataGridView.Rows[i].Cells[10].Value = "0";
                                    ClientsDataGridView.Rows[j].Cells[10].Value = "0";
                                    ClientsDataGridView.Rows[i].Cells[3].Value = " ";
                                    ClientsDataGridView.Rows[j].Cells[3].Value = " ";
                                }

                            }
                        }
                    }
                }
            }
            // player 2 Again and save Data
            else if (words[0] == "player2Again")
            {
                for (int i = 0; i < ClientsDataGridView.Rows.Count; i++)
                {
                    if (ClientsDataGridView.Rows[i].Cells[0].Value.ToString() == words[3])
                    {
                        for (int j = 0; j < ClientsDataGridView.Rows.Count; j++)
                        {
                            if (ClientsDataGridView.Rows[i].Cells[2].Value.ToString() == ClientsDataGridView.Rows[j].Cells[2].Value.ToString() &&
                                ClientsDataGridView.Rows[j].Cells[5].Value.ToString() == "player" &&
                                ClientsDataGridView.Rows[i].Cells[5].Value.ToString() == "player" && i != j)
                            {
                                ClientsDataGridView.Rows[i].Cells[10].Value = words[1];
                                if (ClientsDataGridView.Rows[i].Cells[10].Value.ToString() != "0" && ClientsDataGridView.Rows[j].Cells[10].Value.ToString() != "0")
                                {
                                    clients[i].sendMsg("player1Again=" + ClientsDataGridView.Rows[j].Cells[10].Value.ToString() + "=player2Again=" + ClientsDataGridView.Rows[i].Cells[10].Value.ToString());      
                                    clients[j].sendMsg("player2Again=" + words[1] + "=player1Again=" + ClientsDataGridView.Rows[j].Cells[10].Value.ToString());
                                   

                                    if (ClientsDataGridView.Rows[i].Cells[10].Value.ToString() == "2" || ClientsDataGridView.Rows[j].Cells[10].Value.ToString() == "2")
                                    {
                                        s_writer = File.AppendText(@"results.txt");
                                        s_writer.WriteLine("Matches Results");
                                        s_writer.WriteLine("Player1 name" + ":" + ClientsDataGridView.Rows[j].Cells[1].Value.ToString() + "  " +
                                                           "result" + ":" + ClientsDataGridView.Rows[j].Cells[11].Value.ToString() + "  " +
                                                           "Player2 name" + ":" + ClientsDataGridView.Rows[i].Cells[1].Value.ToString() + "  " +
                                                           "result" + ":" + ClientsDataGridView.Rows[i].Cells[11].Value.ToString() + "  " +
                                                           "Date" + ":" + ClientsDataGridView.Rows[j].Cells[12].Value.ToString());
                                        s_writer.WriteLine(s_writer.NewLine);
                                        s_writer.Close();
                                        ClientsDataGridView.Rows[i].Cells[11].Value = "0";
                                        ClientsDataGridView.Rows[j].Cells[11].Value = "0";
                                    }
                                    ClientsDataGridView.Rows[i].Cells[10].Value = "0";
                                    ClientsDataGridView.Rows[j].Cells[10].Value = "0";
                                    ClientsDataGridView.Rows[i].Cells[3].Value = " ";
                                    ClientsDataGridView.Rows[j].Cells[3].Value = " ";
                                }
                            }
                        }
                    }
                }
            }
            // restart new game
            else if (words[0] == "restart")
            {
                for(int i = 0; i < ClientsDataGridView.Rows.Count; i++)
                {
                    if (ClientsDataGridView.Rows[i].Cells[0].Value.ToString() == words[2])
                    {
                        clients[i].room = " ";
                        for(int j = 2; j < 10; j++)
                        {
                            ClientsDataGridView.Rows[i].Cells[j].Value = " ";
                        }
                        for(int k = 0; k < ClientsDataGridView.Rows.Count; k++)
                        {
                            if (ClientsDataGridView.Rows[i].Cells[2].Value.ToString() == ClientsDataGridView.Rows[k].Cells[2].Value.ToString()&&
                                ClientsDataGridView.Rows[k].Cells[5].Value.ToString()=="watcher")
                            {
                                ClientsDataGridView.Rows[k].Cells[2].Value = " ";
                                ClientsDataGridView.Rows[k].Cells[5].Value = " ";
                                ClientsDataGridView.Rows[k].Cells[6].Value = " ";
                                ClientsDataGridView.Rows[k].Cells[7].Value = " ";
                                ClientsDataGridView.Rows[k].Cells[9].Value = " ";

                            }
                        }
                    }
                    clients[i].sendMsg("clear=" + "id=" + words[2]);

                }
            }
            // request to watch
            else if (words[0] == "watch")
            {
                int checkRooms = 0;
                for (int i = 0; i < clients.Count; i++)
                {
                    if (clients[i].room == words[4]) checkRooms++;   
                }
                if (checkRooms > 1)
                {
                    for (int i = 0; i < clients.Count; i++)
                    {
                        if (clients[i].id == int.Parse(words[2]))
                        {
                            clients[i].room = words[4];
                            clients[i].status = "watcher";
                        }

                        clients[i].sendMsg("roomW=" + words[4] + "=id=" + words[2] + "=status=" + "watcher");

                    }
                    for (int i = 0 , player=0; i < ClientsDataGridView.Rows.Count; i++)
                    {
                        if (ClientsDataGridView.Rows[i].Cells[2].Value.ToString() == words[4] && 
                            ClientsDataGridView.Rows[i].Cells[5].Value.ToString() == "player")
                        {
                            player++;
                            for (int j = 0; j < ClientsDataGridView.Rows.Count; j++)
                            {
                                if (ClientsDataGridView.Rows[j].Cells[0].Value.ToString() == words[2] && player==1)
                                {
                                    ClientsDataGridView.Rows[j].Cells[6].Value = ClientsDataGridView.Rows[i].Cells[6].Value.ToString();
                                    ClientsDataGridView.Rows[j].Cells[7].Value = ClientsDataGridView.Rows[i].Cells[7].Value.ToString();
                                    ClientsDataGridView.Rows[j].Cells[9].Value = ClientsDataGridView.Rows[i].Cells[9].Value.ToString();
                                    clients[j].sendMsg("setWatcherBoard" + "=col=" + ClientsDataGridView.Rows[i].Cells[6].Value.ToString() + "=row=" + ClientsDataGridView.Rows[i].Cells[7].Value.ToString() + "=backColor=" + ClientsDataGridView.Rows[i].Cells[9].Value.ToString());
                                    clients[j].sendMsg("drawplayer1Moves=" + "color=" + ClientsDataGridView.Rows[i].Cells[8].Value.ToString() + "=coord" + ClientsDataGridView.Rows[i].Cells[3].Value.ToString());
                                }   
                                else if (ClientsDataGridView.Rows[j].Cells[0].Value.ToString() == words[2] && player == 2)
                                {
                                     clients[j].sendMsg("drawplayer2Moves=" + "color=" + ClientsDataGridView.Rows[i].Cells[8].Value.ToString() + "=coord" + ClientsDataGridView.Rows[i].Cells[3].Value.ToString());
                                }
                            }
                        }
                    }

                    for (int i = 0; i < ClientsDataGridView.Rows.Count; i++)
                    {
                        if (ClientsDataGridView.Rows[i].Cells[0].Value.ToString() == words[2])
                        {
                            ClientsDataGridView.Rows[i].Cells[2].Value = words[4];
                            ClientsDataGridView.Rows[i].Cells[5].Value = "watcher";

                        }
                    }

                }
                else
                {
                    MessageBox.Show("there is no running game");
                }
                
            }
            // client exit from the game
            else if (words[0] == "clientOut")
            {
                for(int i = 0; i < clients.Count; i++)
                {
                    if (clients[i].id == int.Parse(words[2]))
                    {
                        clients.RemoveAt(i);
                    }
                }
                for(int i = 0; i < ClientsDataGridView.Rows.Count; i++)
                {
                    if (ClientsDataGridView.Rows[i].Cells[0].Value.ToString() == words[2])
                    {
                        ClientsDataGridView.Rows.RemoveAt(i);
                    }
                }
                for(int i = 0; i < clients.Count; i++)
                {
                    clients[i].sendMsg("clientOut=" + "id=" + words[2]);
                }
            }
          
        }       
        // start server function
        private async void start()
        {
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            TcpListener tcpListener = new TcpListener(ip, 49300);
            tcpListener.Start();
            while (true)
            {
                tcpClient = await tcpListener.AcceptTcpClientAsync();
                client = new Client(tcpClient);
                clients.Add(client);
                client.id = ++ClinetID;
                client.MsgRecrived += Client_MsgRecrived;
                foreach (Client client in clients)
                {   if(client.id==ClinetID)
                    client.sendMsg("id="+ClinetID.ToString());
                }
                
            }
        }
        //turn server on 
        private void button2_Click(object sender, EventArgs e)
        {
            start();
            button2.Enabled = false;
        }
    
        //Turn server off
        private void button3_Click(object sender, EventArgs e)
        {
           
            this.Close();

        }
    }
}