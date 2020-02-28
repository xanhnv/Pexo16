namespace Pexo16
{
    partial class DashBoard
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param Name="disposing">true if managed resources should be disposed; otherwise, false.</param>
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
            this.components = new System.ComponentModel.Container();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnGraph = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.clmLogger = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmLocation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmSerial = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmCH1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmCH2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmCH3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmCH4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmCH5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmCH6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmCH7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmCH8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.timerDashBoard = new System.Windows.Forms.Timer(this.components);
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.groupBox1.Controls.Add(this.btnGraph);
            this.groupBox1.Controls.Add(this.btnStart);
            this.groupBox1.Location = new System.Drawing.Point(376, 41);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(350, 79);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // btnGraph
            // 
            this.btnGraph.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGraph.Location = new System.Drawing.Point(207, 27);
            this.btnGraph.Name = "btnGraph";
            this.btnGraph.Size = new System.Drawing.Size(101, 35);
            this.btnGraph.TabIndex = 0;
            this.btnGraph.Text = "Graph";
            this.btnGraph.UseVisualStyleBackColor = true;
            this.btnGraph.Click += new System.EventHandler(this.btnGraph_Click);
            // 
            // btnStart
            // 
            this.btnStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStart.Location = new System.Drawing.Point(42, 27);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(101, 35);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
            this.dataGridView1.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.clmLogger,
            this.clmLocation,
            this.clmDescription,
            this.clmSerial,
            this.clmCH1,
            this.clmCH2,
            this.clmCH3,
            this.clmCH4,
            this.clmCH5,
            this.clmCH6,
            this.clmCH7,
            this.clmCH8});
            this.dataGridView1.GridColor = System.Drawing.SystemColors.ControlDarkDark;
            this.dataGridView1.Location = new System.Drawing.Point(8, 126);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToDisplayedHeaders;
            this.dataGridView1.Size = new System.Drawing.Size(1028, 235);
            this.dataGridView1.TabIndex = 1;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // clmLogger
            // 
            this.clmLogger.HeaderText = "Logger";
            this.clmLogger.Name = "clmLogger";
            // 
            // clmLocation
            // 
            this.clmLocation.HeaderText = "Location";
            this.clmLocation.Name = "clmLocation";
            // 
            // clmDescription
            // 
            this.clmDescription.HeaderText = "Description";
            this.clmDescription.Name = "clmDescription";
            // 
            // clmSerial
            // 
            this.clmSerial.HeaderText = "Serial Number";
            this.clmSerial.Name = "clmSerial";
            // 
            // clmCH1
            // 
            this.clmCH1.HeaderText = "CH1";
            this.clmCH1.Name = "clmCH1";
            // 
            // clmCH2
            // 
            this.clmCH2.HeaderText = "CH2";
            this.clmCH2.Name = "clmCH2";
            // 
            // clmCH3
            // 
            this.clmCH3.HeaderText = "CH3";
            this.clmCH3.Name = "clmCH3";
            // 
            // clmCH4
            // 
            this.clmCH4.HeaderText = "CH4";
            this.clmCH4.Name = "clmCH4";
            // 
            // clmCH5
            // 
            this.clmCH5.HeaderText = "CH5";
            this.clmCH5.Name = "clmCH5";
            // 
            // clmCH6
            // 
            this.clmCH6.HeaderText = "CH6";
            this.clmCH6.Name = "clmCH6";
            // 
            // clmCH7
            // 
            this.clmCH7.HeaderText = "CH7";
            this.clmCH7.Name = "clmCH7";
            // 
            // clmCH8
            // 
            this.clmCH8.HeaderText = "CH8";
            this.clmCH8.Name = "clmCH8";
            // 
            // timerDashBoard
            // 
            this.timerDashBoard.Interval = 1000;
            this.timerDashBoard.Tick += new System.EventHandler(this.timerDashBoard_Tick);
            // 
            // DashBoard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1044, 373);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.groupBox1);
            this.Name = "DashBoard";
            this.Text = "DashBoard";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmDashBoard_FormClosed);
            this.Load += new System.EventHandler(this.frmDashBoard_Load);
            this.SizeChanged += new System.EventHandler(this.frmDashBoard_SizeChanged);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnGraph;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Timer timerDashBoard;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmLogger;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmLocation;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmDescription;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmSerial;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmCH1;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmCH2;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmCH3;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmCH4;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmCH5;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmCH6;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmCH7;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmCH8;
    }
}