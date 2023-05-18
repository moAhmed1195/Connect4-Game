namespace server
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.ClientsDataGridView = new System.Windows.Forms.DataGridView();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Client_Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Room = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Coord = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Client_Text = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PlayerStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.roomCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.roomRow = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.playerColor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.backColor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.playAgain = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Result = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.ClientsDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.ForeColor = System.Drawing.Color.Green;
            this.button2.Location = new System.Drawing.Point(30, 77);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(91, 45);
            this.button2.TabIndex = 3;
            this.button2.Text = "Start";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button3.ForeColor = System.Drawing.Color.Red;
            this.button3.Location = new System.Drawing.Point(30, 183);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(91, 46);
            this.button3.TabIndex = 4;
            this.button3.Text = "Exit";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // ClientsDataGridView
            // 
            this.ClientsDataGridView.AllowUserToAddRows = false;
            this.ClientsDataGridView.AllowUserToDeleteRows = false;
            this.ClientsDataGridView.AllowUserToResizeColumns = false;
            this.ClientsDataGridView.AllowUserToResizeRows = false;
            this.ClientsDataGridView.BackgroundColor = System.Drawing.Color.White;
            this.ClientsDataGridView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.ClientsDataGridView.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
            this.ClientsDataGridView.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Tahoma", 8F);
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.ClientsDataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.ClientsDataGridView.ColumnHeadersHeight = 24;
            this.ClientsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.ClientsDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ID,
            this.Client_Name,
            this.Room,
            this.Coord,
            this.Client_Text,
            this.PlayerStatus,
            this.roomCol,
            this.roomRow,
            this.playerColor,
            this.backColor,
            this.playAgain,
            this.Result,
            this.Date});
            this.ClientsDataGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.ClientsDataGridView.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.ClientsDataGridView.Location = new System.Drawing.Point(145, 12);
            this.ClientsDataGridView.MultiSelect = false;
            this.ClientsDataGridView.Name = "ClientsDataGridView";
            this.ClientsDataGridView.ReadOnly = true;
            this.ClientsDataGridView.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Tahoma", 8F);
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.Color.White;
            this.ClientsDataGridView.RowHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.ClientsDataGridView.RowHeadersVisible = false;
            this.ClientsDataGridView.RowHeadersWidth = 40;
            this.ClientsDataGridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.Color.Black;
            this.ClientsDataGridView.RowsDefaultCellStyle = dataGridViewCellStyle8;
            this.ClientsDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.ClientsDataGridView.ShowCellErrors = false;
            this.ClientsDataGridView.ShowCellToolTips = false;
            this.ClientsDataGridView.ShowEditingIcon = false;
            this.ClientsDataGridView.ShowRowErrors = false;
            this.ClientsDataGridView.Size = new System.Drawing.Size(1179, 492);
            this.ClientsDataGridView.TabIndex = 5;
            this.ClientsDataGridView.TabStop = false;
            // 
            // ID
            // 
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.ID.DefaultCellStyle = dataGridViewCellStyle6;
            this.ID.HeaderText = "ID";
            this.ID.Name = "ID";
            this.ID.ReadOnly = true;
            this.ID.Width = 50;
            // 
            // Client_Name
            // 
            this.Client_Name.HeaderText = "Name";
            this.Client_Name.Name = "Client_Name";
            this.Client_Name.ReadOnly = true;
            // 
            // Room
            // 
            this.Room.HeaderText = "Room";
            this.Room.Name = "Room";
            this.Room.ReadOnly = true;
            this.Room.Width = 50;
            // 
            // Coord
            // 
            this.Coord.HeaderText = "Coordination";
            this.Coord.Name = "Coord";
            this.Coord.ReadOnly = true;
            // 
            // Client_Text
            // 
            this.Client_Text.HeaderText = "Text";
            this.Client_Text.Name = "Client_Text";
            this.Client_Text.ReadOnly = true;
            this.Client_Text.Width = 200;
            // 
            // PlayerStatus
            // 
            this.PlayerStatus.HeaderText = "Status";
            this.PlayerStatus.Name = "PlayerStatus";
            this.PlayerStatus.ReadOnly = true;
            // 
            // roomCol
            // 
            this.roomCol.HeaderText = "Col";
            this.roomCol.Name = "roomCol";
            this.roomCol.ReadOnly = true;
            this.roomCol.Width = 50;
            // 
            // roomRow
            // 
            this.roomRow.HeaderText = "Row";
            this.roomRow.Name = "roomRow";
            this.roomRow.ReadOnly = true;
            this.roomRow.Width = 50;
            // 
            // playerColor
            // 
            this.playerColor.HeaderText = "Color";
            this.playerColor.Name = "playerColor";
            this.playerColor.ReadOnly = true;
            // 
            // backColor
            // 
            this.backColor.HeaderText = "BackColor";
            this.backColor.Name = "backColor";
            this.backColor.ReadOnly = true;
            // 
            // playAgain
            // 
            this.playAgain.HeaderText = "PlayAgain";
            this.playAgain.Name = "playAgain";
            this.playAgain.ReadOnly = true;
            // 
            // Result
            // 
            this.Result.HeaderText = "Result";
            this.Result.Name = "Result";
            this.Result.ReadOnly = true;
            this.Result.Width = 50;
            // 
            // Date
            // 
            this.Date.HeaderText = "Date";
            this.Date.Name = "Date";
            this.Date.ReadOnly = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1336, 559);
            this.Controls.Add(this.ClientsDataGridView);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Name = "Form1";
            this.Text = "Server";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)(this.ClientsDataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.DataGridView ClientsDataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Client_Name;
        private System.Windows.Forms.DataGridViewTextBoxColumn Room;
        private System.Windows.Forms.DataGridViewTextBoxColumn Coord;
        private System.Windows.Forms.DataGridViewTextBoxColumn Client_Text;
        private System.Windows.Forms.DataGridViewTextBoxColumn PlayerStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn roomCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn roomRow;
        private System.Windows.Forms.DataGridViewTextBoxColumn playerColor;
        private System.Windows.Forms.DataGridViewTextBoxColumn backColor;
        private System.Windows.Forms.DataGridViewTextBoxColumn playAgain;
        private System.Windows.Forms.DataGridViewTextBoxColumn Result;
        private System.Windows.Forms.DataGridViewTextBoxColumn Date;
    }
}

