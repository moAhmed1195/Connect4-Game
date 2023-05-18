using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Net.Sockets;
using System.Net;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading;
using System.Linq.Expressions;

namespace client
{
    public partial class Form1 : Form
    {
        BinaryReader reader;
        BinaryWriter writer;
        NetworkStream networkStream;
        public int id;
        public string toId;
        public string toRoom;
        TcpClient tcpClient = new TcpClient();

        public string userName = "client";


        private Rectangle[] boardColumns;
        public int[,] board;
        public int Rows;
        public int Columns;
        int Turn;
        //Rectangle
        Color RectColor;
        Brush RectBrush;
        //Elipse
        Color ElpColor;
        Brush ElpBrush;
        //Player 1
        Color Player1_Clr;
        Brush Player1_Brush;
        //Player 2
        Color Player2_Clr;
        Brush Player2_Brush;
        //col and row form server
        int ColIndex;
        int RowIndex;
        int Winner;

        int player1Again = 0;
        int player2Again = 0;

        public Form1()
        {
            InitializeComponent();
            RectColor = Color.Blue;
            ElpColor = Color.White;
            Player1_Clr = Color.Red;
            Player2_Clr = Color.Yellow;
            Turn = 0;
        }

        // draw board function
        public void DrawBoard(string col,string row,Color color)
        {
            //Drawing The Game Board
            Columns = int.Parse(col);
            Rows = int.Parse(row);
            boardColumns = new Rectangle[Columns];
            board = new int[Rows, Columns];
            Graphics g = panel1.CreateGraphics();
            RectBrush = new SolidBrush(color);
            ElpBrush = new SolidBrush(ElpColor);
            g.FillRectangle(RectBrush, 20, 20, Columns * 80, Rows * 80);
            //Filling the Board with Elipse
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    if (i == 0)
                    {
                        boardColumns[j] = new Rectangle(25 + 80 * j, 25, 70, Rows * 80);
                    }
                    g.FillEllipse(ElpBrush, 25 + 80 * j, 25 + 80 * i, 65, 65);
                }
            }
            Invalidate();
            
        }
        // available size of the game
        public void selectSize()
        {
            if (colTxtBox.Text == "5") { rowTxtBox.Text = "4"; }
            else if (colTxtBox.Text == "6") { rowTxtBox.Text = "5"; }
            else if (colTxtBox.Text == "8") { rowTxtBox.Text = "7"; }
            else if (colTxtBox.Text == "9") { rowTxtBox.Text = "7"; }
            else if (colTxtBox.Text == "10") { rowTxtBox.Text = "7"; }
            else { colTxtBox.Text = "7"; rowTxtBox.Text = "6"; }   
        }
        //disable buttons
        public void disabledControl()
        {
            BoardClr.Enabled = false;
            playerClr.Enabled = false;
            colTxtBox.Enabled = false;
        }
        // detect column 
        public int ColumnNumberDetector(Point MousePoint)
        {
            for (int i = 0; i < boardColumns.Length; i++)
            {
                if (MousePoint.X >= boardColumns[i].X && MousePoint.Y >= boardColumns[i].Y)
                {
                    if (MousePoint.X <= boardColumns[i].X + boardColumns[i].Width && MousePoint.Y <= boardColumns[i].Y + boardColumns[i].Height)
                    {
                        return i;               //Return The Index Of the Column
                    }
                }
            }
            return -1;                          //Out Of the Board
        }
        // detect row
        public int EmptyRowDetector(int col)
        {
            for (int i = Rows - 1; i >= 0; i--)
            {
                if (board[i, col] == 0)
                {
                    return i;
                }
            }
            return -1;
        }
        public bool AllClrEqual(int ToCheck, params int[] Numbers)
        {
            foreach (int num in Numbers)
            {
                if (num != ToCheck) { return false; }
            }
            return true;
        }
        //check winner
        public int WinnerPlayer(int Player)
        {
            //Vertical
            for (int r = 0; r < board.GetLength(0) - 3; r++)
            {
                for (int c = 0; c < board.GetLength(1); c++)
                {
                    if (AllClrEqual(Player, board[r, c], board[r + 1, c], board[r + 2, c], board[r + 3, c]))
                    {
                        return Player;
                    }
                }
            }
            //Horizontal
            for (int r = 0; r < board.GetLength(0); r++)
            {
                for (int c = 0; c < board.GetLength(1) - 3; c++)
                {
                    if (AllClrEqual(Player, board[r, c], board[r, c + 1], board[r, c + 2], board[r, c + 3]))
                    {
                        return Player;
                    }
                }
            }
            //Diagonal \
            for (int r = 0; r < board.GetLength(0) - 3; r++)
            {
                for (int c = 0; c < board.GetLength(1) - 3; c++)
                {
                    if (AllClrEqual(Player, board[r, c], board[r + 1, c + 1], board[r + 2, c + 2], board[r + 3, c + 3]))
                    {
                        return Player;
                    }
                }
            }
            //Diagonal /
            for (int r = 0; r < board.GetLength(0) - 3; r++)
            {
                for (int c = 3; c < board.GetLength(1); c++)
                {
                    if (AllClrEqual(Player, board[r, c], board[r + 1, c - 1], board[r + 2, c - 2], board[r + 3, c - 3]))
                    {
                        return Player;
                    }
                }
            }
            return -1;
        }
        //checke board is full
        public bool IsBoardFull(int[,] board)
        {
            for (int r = 0; r < board.GetLength(0); r++)
            {
                for (int c = 0; c < board.GetLength(1); c++)
                {
                    if (board[r, c] == 0)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        // click on board
        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                 ColIndex = ColumnNumberDetector(e.Location);
                if (ColIndex != -1)      //Insure That the click on board
                {
                    RowIndex = EmptyRowDetector(ColIndex);
                    if (RowIndex != -1)
                    {
                        board[RowIndex, ColIndex] = Turn;
                        //Detecting Each Turn
                        if (Turn == 1)                       //Player 1
                        {
                            Graphics g = panel1.CreateGraphics();
                            Player1_Brush = new SolidBrush(Player1_Clr);
                            g.FillEllipse(Player1_Brush, 25 + 80 * ColIndex, 25 + 80 * RowIndex, 65, 65);
                        }
                        else if (Turn == 2)               //Player 2
                        {
                            Graphics g = panel1.CreateGraphics();
                            Player2_Brush = new SolidBrush(Player2_Clr);
                            g.FillEllipse(Player2_Brush, 25 + 80 * ColIndex, 25 + 80 * RowIndex, 65, 65);

                        }
                        //Detecting The Winner
                        Winner = WinnerPlayer(Turn);
                        if (Winner != -1)
                        {
                            if (Winner == 1)
                            {
                                send_query("win1=" + "id=" + id.ToString());
                              
                                Thread thread1 = new Thread(() =>
                                {
                                    again playagain = new again();
                                    playagain.msgTxt = "Congratulation---Player 1 You Win!!";
                                    DialogResult = playagain.ShowDialog();
                                    if (DialogResult == DialogResult.Yes)
                                    {
                                        player1Again = 1;
                                        send_query("player1Again=" + player1Again + "=id=" + id.ToString());

                                    }
                                    else if (DialogResult == DialogResult.No)
                                    {
                                        player1Again = 2;
                                        send_query("player1Again=" + player1Again + "=id=" + id.ToString());

                                    }
                                });
                                thread1.Start();
                                
                            }
                            else if (Winner == 2)
                            {
                                send_query("win2=" + "id=" + id.ToString());


                                Thread thread1 = new Thread(() =>
                                {
                                    again playagain = new again();
                                    playagain.msgTxt = "Congratulation---Player 2 You Win!!";

                                    DialogResult = playagain.ShowDialog();
                                    if (DialogResult == DialogResult.Yes)
                                    {
                                        player2Again = 1;
                                        send_query("player2Again=" + player2Again + "=id=" + id.ToString());

                                    }
                                    else if (DialogResult == DialogResult.No)
                                    {
                                        player2Again = 2;
                                        send_query("player2Again=" + player2Again + "=id=" + id.ToString());

                                    }
                                });
                                thread1.Start();
                                /*
                                Thread thread1 = new Thread(() =>
                                {

                                });
                                thread1.Start();*/

                                // MessageBox.Show("Congratulation---Player 2 You Win!!");
                            }
                           
                        }
                        //If Board is Full
                        if (IsBoardFull(board))
                        {
                            MessageBox.Show("NO WINNER");
                           
                        }


                        //Changing Turns
                        if (Turn == 1)
                        {
                            send_query("turn1=" + Turn + "=id=" + id + "=colIndex=" + ColIndex + "=rowIndex=" + RowIndex);
    
                        }
                        else if (Turn == 2)
                        {
                            send_query("turn2=" + Turn + "=id=" + id + "=colIndex=" + ColIndex + "=rowIndex=" + RowIndex);
  
                        }
                       
                    }
                }
            }

            catch
            {
                MessageBox.Show("Fill the required fields");
            }
        }
       
        // connect  to server
        private async void connect()
        {
            try 
            { 
                tcpClient.Connect("127.0.0.1", 49300);
                networkStream = tcpClient.GetStream();
                networkStream = tcpClient.GetStream();
                reader = new BinaryReader(networkStream);
                writer = new BinaryWriter(networkStream);
                if(clientName.Text!="")  
                userName = clientName.Text;
                send_query("username=" + userName);

                while (true)
                {
                   await Task.Run(async () => await ReadAsync());
                }
            }

            catch
            {
                MessageBox.Show("You must connect to server first");
            }
        }

        private async Task<string> ReadStringAsync(BinaryReader reader)
        {
            int length = reader.ReadInt32();
            byte[] buffer = new byte[length];
            await networkStream.ReadAsync(buffer, 0, length);
            return Encoding.UTF8.GetString(buffer);
        }
        //read function

        private async Task ReadAsync()
        {
            try
            {
                string message = await ReadStringAsync(reader);
                string[] words = message.Split('=');
                //get id from server
                if (words[0] == "id")
                { id = int.Parse(words[1]); }
                // add player to grid view
                else if (words[0] == "add")
                {
                    dataGridView1.Rows.Add(words[1], words[2], words[3], words[4]);
                }
                // get a room
                else if (words[0] == "room")
                {   
                    colTxtBox.Enabled= true;
                    BoardClr.Enabled = true;
                    setBoardBtn.Enabled = true;
                    
                    for(int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        if (dataGridView1.Rows[i].Cells[0].Value.ToString() == words[3])
                        {
                            dataGridView1.Rows[i].Cells[2].Value = words[1];
                            dataGridView1.Rows[i].Cells[3].Value = words[5];

                        }

                    }
                   
                }
                // accept to play or not
                else if (words[0] == "accept")
                {
                    toId = words[4];
                    toRoom = words[2];
                    AcceptDialog acceptDialog=new AcceptDialog();
                    DialogResult= acceptDialog.ShowDialog();
                    if (DialogResult == DialogResult.Yes) // agree to play
                    {
                        send_query("acceptResponse=" + "yes" + "=id=" + id.ToString() + "=toId=" + toId + "=room=" + toRoom + "=color=" + Player2_Clr.Name);
                    }
                    else 
                    {
                        send_query("acceptResponse=" + "no" + "=id=" + id.ToString() + "=toId=" + toId + "=room=" + toRoom + "=color=" + Player2_Clr.Name);

                    }
                    
                }
                // refused to play
                else if (words[0] == "refused")
                {
                    MessageBox.Show("your request was refused");
                }
                // draw game 
                else if (words[0] == "drawGame")
                {

                    RectColor = ToColor(words[6], words[6]);
                    DrawBoard(words[2], words[4], ToColor(words[6], words[6] )); 
                    colTxtBox.Text = words[2];
                    rowTxtBox.Text = words[4];
                    colTxtBox.Enabled = false;
                    disabledControl();
                    setBoardBtn.Enabled = false;
                    Player2_Clr = ToColor(words[10], words[10]);
                    Player1_Clr = ToColor(words[8], words[8]);
                }
                // get player 2 disk color
                else if (words[0] == "player2color")
                {
                    Player2_Clr = ToColor(words[1], words[1]);
                    Player1_Clr = ToColor(words[5], words[5]);
                    Turn = int.Parse(words[3]);
                }
                // player 2 turn
                else if (words[0] == "playertwoturn")
                {
                    Turn = 1;
                    board[int.Parse(words[7]), int.Parse(words[5])] = Turn;
                   
                    Graphics g = panel1.CreateGraphics();
                    Player1_Brush = new SolidBrush(Player1_Clr);
                    g.FillEllipse(Player1_Brush, 25 + 80 * int.Parse(words[5]), 25 + 80 * int.Parse(words[7]), 65, 65);
                 

                    Winner = WinnerPlayer(Turn);
                    if(Winner == 1)
                    {
                        Turn = 0;
                    }
                    else
                    {
                    Turn = int.Parse(words[1]);

                    }
                    
                    
                }
                //player 1 turn
                else if (words[0] == "playeroneturn")
                {
                    Turn = 2;
                    board[int.Parse(words[7]), int.Parse(words[5])] = Turn;

                    Graphics g = panel1.CreateGraphics();
                    Player2_Brush = new SolidBrush(Player2_Clr);
                    g.FillEllipse(Player2_Brush, 25 + 80 * int.Parse(words[5]), 25 + 80 * int.Parse(words[7]), 65, 65);

                    Winner = WinnerPlayer(Turn);
                    if (Winner==2)
                    {
                        Turn = 0;
                    }
                    else
                    {

                    Turn = int.Parse(words[1]);
                    }
                    
                    
                }
                // disable player 1
                else if (words[0] == "disableplayer1turn")
                {
                    Turn = int.Parse(words[1]);
                }
                //disable player2
                else if (words[0] == "disableplayer2turn")
                {
                    Turn = int.Parse(words[1]);
                }
                //player1 win
                else if (words[0] == "win1")
                {
                    
                    Thread thread1 = new Thread(() =>
                    {
                        again playagain = new again();
                        playagain.msgTxt = "Player 2 You Lose";
                        DialogResult = playagain.ShowDialog();
                        if (DialogResult == DialogResult.Yes)
                        {
                            player2Again = 1;
                            send_query("player2Again=" + player2Again + "=id=" + id.ToString());
  
                        }
                        else if (DialogResult == DialogResult.No)
                        {
                            player2Again = 2;
                            send_query("player2Again=" + player2Again + "=id=" + id.ToString());

                        }
                    });

                    thread1.Start();

                   
                }
                //player2 win
                else if (words[0] == "win2")
                {
                    
                    Thread thread1 = new Thread(() =>
                    {
                        again playagain = new again();
                        playagain.msgTxt = "Player 1 You Lose";
                        DialogResult = playagain.ShowDialog();
                        if (DialogResult == DialogResult.Yes)
                        {

                            player1Again = 1;
                            send_query("player1Again=" + player1Again + "=id=" + id.ToString());
      
                        }
                        else if (DialogResult == DialogResult.No)
                        {
                            player1Again = 2;
                            send_query("player1Again=" + player1Again + "=id=" + id.ToString());

                        }
                    });
                    thread1.Start();

                  
                    
                }
                // player1 say again or not
                else if (words[0] == "player1Again")
                {   
                    player1Again = 0;
                    player2Again = 0;
                    player1Again = int.Parse(words[1]);
                    player2Again = int.Parse(words[3]);
                   if(player1Again== 1 && player2Again == 1)
                    {
                        //new game
                        DrawBoard(colTxtBox.Text, rowTxtBox.Text, RectColor);
                        for (int i = 0; i < Rows; i++)
                        {
                            for (int j = 0; j < Columns; j++)
                            {
                                board[i, j] = 0;
                            }
                        }
                        Turn = 1;

                    }
                   else
                    {
                        //player2 out
                        panel1.ForeColor = panel1.BackColor;
                        setBoardBtn.Enabled = true;
                        BoardClr.Enabled = true;
                        playerClr.Enabled = true;
                        colTxtBox.Enabled = true;

                        send_query("restart=" + "id=" + id.ToString());

                    }
                }
                // player2 say again or not
                else if (words[0] == "player2Again")
                {
                    //player1 application
                    player1Again = 0;
                    player2Again = 0;
                    player2Again =  int.Parse(words[1]);
                    player1Again =  int.Parse(words[3]);
                    if (player1Again == 1 && player2Again == 1)
                    {
                        //new game
                        DrawBoard(colTxtBox.Text, rowTxtBox.Text, RectColor);
                        for(int i = 0; i < Rows; i++)
                        {
                            for(int j = 0; j < Columns; j++)
                            {
                                board[i, j] = 0;
                            }
                        }
                        Turn = 1;
                    }
                    else if(player1Again==2 || player2Again==2)
                    {
                        //player2 out
                        panel1.ForeColor = panel1.BackColor;
                        setBoardBtn.Enabled = true;
                        BoardClr.Enabled = true;
                        playerClr.Enabled = true;
                        colTxtBox.Enabled = true;

                        send_query("restart=" + "id=" + id.ToString());

                    }
                }
                // clear data from grid
                else if (words[0] == "clear")
                {
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        if (dataGridView1.Rows[i].Cells[0].Value.ToString() == words[2])
                        {
                            dataGridView1.Rows[i].Cells[2].Value = " ";
                            dataGridView1.Rows[i].Cells[3].Value = " ";
                            

                        }

                    }
                }
                // give room to watcher
                else if (words[0] == "roomW")
                {
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        if (dataGridView1.Rows[i].Cells[0].Value.ToString() == words[3])
                        {
                            dataGridView1.Rows[i].Cells[2].Value = words[1];
                            dataGridView1.Rows[i].Cells[3].Value = words[5];

                        }

                    }
                }
                //set board to watcher
                else if (words[0] == "setWatcherBoard")
                {
                    DrawBoard(words[2], words[4], ToColor(words[6], words[6]));
                }
                // player 1 prev. moves to watcher
                else if (words[0] == "drawplayer1Moves")
                {
                    Graphics g = panel1.CreateGraphics();
                    Player1_Brush = new SolidBrush(ToColor(words[2], words[2]));
                    for (int i =4 ; i < words.Length; i += 2)
                    {
                        g.FillEllipse(Player1_Brush, 25 + 80 * int.Parse(words[i]), 25 + 80 * int.Parse(words[i+1]), 65, 65);
                    }
                }
                // player 2 prev.moves to watcher
                else if (words[0] == "drawplayer2Moves")
                {   
                    Graphics g = panel1.CreateGraphics();
                    Player2_Brush = new SolidBrush(ToColor(words[2], words[2]));                    for (int i = 4; i < words.Length; i += 2)
                    {
                        g.FillEllipse(Player2_Brush, 25 + 80 * int.Parse(words[i]), 25 + 80 * int.Parse(words[i + 1]), 65, 65);
                    }
                }
                //player 1 moves
                else if (words[0] == "move1watcher")
                {
                    Graphics g = panel1.CreateGraphics();
                    Player1_Brush = new SolidBrush(ToColor(words[2], words[2]));                    
                    g.FillEllipse(Player1_Brush, 25 + 80 * int.Parse(words[4]), 25 + 80 * int.Parse(words[6]), 65, 65);
                    
                }
                //player 2 moves
                else if (words[0] == "move2watcher")
                {
                    Graphics g = panel1.CreateGraphics();
                    Player2_Brush = new SolidBrush(ToColor(words[2], words[2]));
                    g.FillEllipse(Player2_Brush, 25 + 80 * int.Parse(words[4]), 25 + 80 * int.Parse(words[6]), 65, 65);

                }
                // recieve msg from another player
                else if (words[0] == "RecieveMsg")
                {
                    txtReceivedMsg.Text += $"{words[1]}{Environment.NewLine}";

                }
                // another player out
                else if (words[0] == "clientOut")
                {
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        if (dataGridView1.Rows[i].Cells[0].Value.ToString() == words[2])
                        {
                            dataGridView1.Rows.RemoveAt(i);

                        }

                    }
                }
               
                
            }
            catch (IOException ex)
            {
                
            }
        }
        //connect to server
       
        private void button1_Click(object sender, EventArgs e)
        {
             
            connect();
            clientName.Enabled = false; 
            button3.Enabled = true;
            playbtn.Enabled = true;
            watchbtn.Enabled = true;
            RoomTxtBox.Enabled = true;
            button1.Enabled = false;
            playerClr.Enabled = true;
          
        }
        
        private void button2_Click(object sender, EventArgs e)
        {
            string message = txtMsg.Text;
            send_query("message=" + message + "=id=" + id.ToString());
            txtReceivedMsg.Text += $"{userName} : {message}{Environment.NewLine}";
            txtMsg.Clear();
        }
        //exit
        private void button3_Click(object sender, EventArgs e)
        {
            
            this.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {  
        }


        private void playbtn_Click(object sender, EventArgs e)
        {
            if (RoomTxtBox.Text == "") MessageBox.Show("please enter room to play");
            else
            {
                playerClr.Enabled = true;
                send_query("play=" + "id=" + id.ToString() + "=room=" + RoomTxtBox.Text);
            }
        }

        private void watchbtn_Click(object sender, EventArgs e)
        {
            if (RoomTxtBox.Text == "") MessageBox.Show("please enter room to watch");
            else
            {
                send_query("watch=" + "id=" + id.ToString() + "=room=" + RoomTxtBox.Text);
                playerClr.Enabled = false;
            }
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            send_query("clientOut=" + "id=" + id.ToString());

        }

        private void setBoardBtn_Click(object sender, EventArgs e)
        {
            selectSize();
            send_query("board=" + "id=" + id.ToString() + "=col=" + colTxtBox.Text + "=row=" + rowTxtBox.Text + "=color=" + Player1_Clr.Name + "=BackColor=" + RectColor.Name + "=toid=" + toId);
            DrawBoard(colTxtBox.Text, rowTxtBox.Text, RectColor);
            disabledControl();
            colTxtBox.Enabled = false;
            rowTxtBox.Enabled = false;
            setBoardBtn.Enabled = false;
            
        }

        private void BoardClr_Click(object sender, EventArgs e)
        {
            DialogResult Dlg_Result;
            Dlg_Result = BoardcolorDialog.ShowDialog();
            if (Dlg_Result == DialogResult.OK)
            {
                RectColor = BoardcolorDialog.Color;
            }
        }
        public static Color ToColor(string colorString,string alterColor)
        {
            colorString = ExtractHexDigits(colorString);

            Color color = Color.White;

            if (colorString.Length == 6)
            {
                var r = colorString.Substring(0, 2);
                var g = colorString.Substring(2, 2);
                var b = colorString.Substring(4, 2);

                try
                {
                    byte rc = Byte.Parse(r, NumberStyles.HexNumber);
                    byte gc = Byte.Parse(g, NumberStyles.HexNumber);
                    byte bc = Byte.Parse(b, NumberStyles.HexNumber);
                    color = Color.FromArgb(rc, gc, bc);
                    return color;
                    
                }
                catch (Exception)
                {
                    return Color.FromName(alterColor);
                    throw;
                }
            }
           else if (colorString.Length == 8)
            {
                var a = colorString.Substring(0, 2);
                var r = colorString.Substring(2, 2);
                var g = colorString.Substring(4, 2);
                var b = colorString.Substring(6, 2);

                try
                {
                    byte ac = Byte.Parse(a, NumberStyles.HexNumber);
                    byte rc = Byte.Parse(r, NumberStyles.HexNumber);
                    byte gc = Byte.Parse(g, NumberStyles.HexNumber);
                    byte bc = Byte.Parse(b, NumberStyles.HexNumber);
                    color = Color.FromArgb(ac, rc, gc, bc);
                    return color;
                }
                catch (Exception)
                {
                    return Color.FromName(alterColor) ;
                    throw;
                }
            }
            else
            {
                return Color.FromName(alterColor);
            }
        }
        private static string ExtractHexDigits(string colorString)
        {
            Regex HexDigits = new Regex(@"[abcdefABCDEF\d]+", RegexOptions.Compiled);

            var hexnum = new StringBuilder();
            foreach (char c in colorString)
                if (HexDigits.IsMatch(c.ToString()))
                    hexnum.Append(c.ToString());

            return hexnum.ToString();
        }

        private void playerClr_Click(object sender, EventArgs e)
        {
            DialogResult Dlg_Result;
            Dlg_Result = PlayerColorDialog.ShowDialog();
            if (Dlg_Result == DialogResult.OK)
            {
                send_query("playerColor=" + PlayerColorDialog.Color.Name + "=id=" + id.ToString());

            }
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void send_query(string query)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(query);
            int length = bytes.Length;
            writer.Write(length);
            writer.Write(bytes);
            writer.Flush();
        }

        private void send_img_Click(object sender, EventArgs e)
        {
            string message = txtMsg.Text;
            send_query("message=" + $"{userName} : {message}" + "=id=" + id.ToString());
            txtReceivedMsg.Text += $"{userName} : {message}{Environment.NewLine}";
            txtMsg.Clear();
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x128) return;
            base.WndProc(ref m);
        }
    }
}
