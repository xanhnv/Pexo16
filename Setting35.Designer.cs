namespace Pexo16
{
    partial class Setting35
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
            this.components = new System.ComponentModel.Container();
            this.cbbFS = new System.Windows.Forms.ComboBox();
            this.lblFS = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.rtxtEventLog = new System.Windows.Forms.RichTextBox();
            this.btnEraseAllData = new System.Windows.Forms.Button();
            this.btnReadSetting = new System.Windows.Forms.Button();
            this.btnWriteSetting = new System.Windows.Forms.Button();
            this.lblHZ = new System.Windows.Forms.Label();
            this.lblG = new System.Windows.Forms.Label();
            this.cbbBR = new System.Windows.Forms.ComboBox();
            this.chbNoAlarm = new System.Windows.Forms.CheckBox();
            this.txtMaxAlarm = new System.Windows.Forms.TextBox();
            this.txtMinAlarm = new System.Windows.Forms.TextBox();
            this.lb_unitmax = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.lbl_LoggerTime = new System.Windows.Forms.Label();
            this.lbl_LoggerDate = new System.Windows.Forms.Label();
            this.cbbTimeZone = new System.Windows.Forms.ComboBox();
            this.lbl_min = new System.Windows.Forms.Label();
            this.lbl_max = new System.Windows.Forms.Label();
            this.lblSR = new System.Windows.Forms.Label();
            this.Tim_UpdateClock = new System.Windows.Forms.Timer(this.components);
            this.lb_unitmin = new System.Windows.Forms.Label();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.txtChlDesc = new System.Windows.Forms.TextBox();
            this.txtSerial = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtLocation = new System.Windows.Forms.TextBox();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lb_interval = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cbbDuration = new System.Windows.Forms.ComboBox();
            this.cbbStartDelay = new System.Windows.Forms.ComboBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.cbbChannel = new System.Windows.Forms.ComboBox();
            this.cbbUnit = new System.Windows.Forms.ComboBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.chkChannel = new System.Windows.Forms.CheckBox();
            this.chkRecord = new System.Windows.Forms.CheckBox();
            this.chkDes = new System.Windows.Forms.CheckBox();
            this.chkLoc = new System.Windows.Forms.CheckBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.groupBox5.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // cbbFS
            // 
            this.cbbFS.FormattingEnabled = true;
            this.cbbFS.Items.AddRange(new object[] {
            "2",
            "4",
            "8",
            "16"});
            this.cbbFS.Location = new System.Drawing.Point(120, 121);
            this.cbbFS.Name = "cbbFS";
            this.cbbFS.Size = new System.Drawing.Size(102, 21);
            this.cbbFS.TabIndex = 10;
            this.cbbFS.SelectedIndexChanged += new System.EventHandler(this.cbbFS_SelectedIndexChanged);
            // 
            // lblFS
            // 
            this.lblFS.AutoSize = true;
            this.lblFS.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFS.Location = new System.Drawing.Point(26, 124);
            this.lblFS.Name = "lblFS";
            this.lblFS.Size = new System.Drawing.Size(61, 13);
            this.lblFS.TabIndex = 0;
            this.lblFS.Text = "Full scale";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.ForeColor = System.Drawing.Color.Red;
            this.lblStatus.Location = new System.Drawing.Point(211, 439);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(50, 18);
            this.lblStatus.TabIndex = 19;
            this.lblStatus.Text = "Status";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.Location = new System.Drawing.Point(7, 437);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(70, 15);
            this.label17.TabIndex = 20;
            this.label17.Text = "Event Log";
            // 
            // label16
            // 
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.Location = new System.Drawing.Point(183, 483);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(59, 23);
            this.label16.TabIndex = 21;
            // 
            // rtxtEventLog
            // 
            this.rtxtEventLog.BackColor = System.Drawing.SystemColors.Control;
            this.rtxtEventLog.Location = new System.Drawing.Point(3, 465);
            this.rtxtEventLog.Name = "rtxtEventLog";
            this.rtxtEventLog.Size = new System.Drawing.Size(413, 139);
            this.rtxtEventLog.TabIndex = 22;
            this.rtxtEventLog.TabStop = false;
            this.rtxtEventLog.Text = "";
            // 
            // btnEraseAllData
            // 
            this.btnEraseAllData.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEraseAllData.Location = new System.Drawing.Point(487, 565);
            this.btnEraseAllData.Name = "btnEraseAllData";
            this.btnEraseAllData.Size = new System.Drawing.Size(106, 23);
            this.btnEraseAllData.TabIndex = 30;
            this.btnEraseAllData.Text = "Erase All Data ";
            this.btnEraseAllData.UseVisualStyleBackColor = true;
            this.btnEraseAllData.Click += new System.EventHandler(this.btnEraseAllData_Click);
            // 
            // btnReadSetting
            // 
            this.btnReadSetting.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReadSetting.Location = new System.Drawing.Point(487, 516);
            this.btnReadSetting.Name = "btnReadSetting";
            this.btnReadSetting.Size = new System.Drawing.Size(106, 23);
            this.btnReadSetting.TabIndex = 29;
            this.btnReadSetting.Text = "Read Setting";
            this.btnReadSetting.UseVisualStyleBackColor = true;
            this.btnReadSetting.Click += new System.EventHandler(this.btnReadSetting_Click);
            // 
            // btnWriteSetting
            // 
            this.btnWriteSetting.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnWriteSetting.Location = new System.Drawing.Point(487, 466);
            this.btnWriteSetting.Name = "btnWriteSetting";
            this.btnWriteSetting.Size = new System.Drawing.Size(106, 23);
            this.btnWriteSetting.TabIndex = 28;
            this.btnWriteSetting.Text = "Write Setting";
            this.btnWriteSetting.UseVisualStyleBackColor = true;
            this.btnWriteSetting.Click += new System.EventHandler(this.btnWriteSetting_Click);
            // 
            // lblHZ
            // 
            this.lblHZ.AutoSize = true;
            this.lblHZ.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHZ.Location = new System.Drawing.Point(242, 156);
            this.lblHZ.Name = "lblHZ";
            this.lblHZ.Size = new System.Drawing.Size(22, 13);
            this.lblHZ.TabIndex = 0;
            this.lblHZ.Text = "Hz";
            // 
            // lblG
            // 
            this.lblG.AutoSize = true;
            this.lblG.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblG.Location = new System.Drawing.Point(242, 124);
            this.lblG.Name = "lblG";
            this.lblG.Size = new System.Drawing.Size(16, 13);
            this.lblG.TabIndex = 0;
            this.lblG.Text = "G";
            // 
            // cbbBR
            // 
            this.cbbBR.FormattingEnabled = true;
            this.cbbBR.Items.AddRange(new object[] {
            "10",
            "50",
            "100",
            "200",
            "400"});
            this.cbbBR.Location = new System.Drawing.Point(120, 153);
            this.cbbBR.Name = "cbbBR";
            this.cbbBR.Size = new System.Drawing.Size(102, 21);
            this.cbbBR.TabIndex = 11;
            this.cbbBR.SelectedIndexChanged += new System.EventHandler(this.cbbBR_SelectedIndexChanged);
            // 
            // chbNoAlarm
            // 
            this.chbNoAlarm.AutoSize = true;
            this.chbNoAlarm.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chbNoAlarm.ForeColor = System.Drawing.SystemColors.Highlight;
            this.chbNoAlarm.Location = new System.Drawing.Point(11, 84);
            this.chbNoAlarm.Name = "chbNoAlarm";
            this.chbNoAlarm.Size = new System.Drawing.Size(77, 17);
            this.chbNoAlarm.TabIndex = 14;
            this.chbNoAlarm.Text = "No Alarm";
            this.chbNoAlarm.UseVisualStyleBackColor = true;
            this.chbNoAlarm.CheckedChanged += new System.EventHandler(this.chbNoAlarm_CheckedChanged);
            // 
            // txtMaxAlarm
            // 
            this.txtMaxAlarm.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMaxAlarm.Location = new System.Drawing.Point(77, 20);
            this.txtMaxAlarm.Name = "txtMaxAlarm";
            this.txtMaxAlarm.Size = new System.Drawing.Size(82, 21);
            this.txtMaxAlarm.TabIndex = 12;
            this.txtMaxAlarm.TextChanged += new System.EventHandler(this.txtMaxAlarm_TextChanged);
            // 
            // txtMinAlarm
            // 
            this.txtMinAlarm.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMinAlarm.Location = new System.Drawing.Point(77, 57);
            this.txtMinAlarm.Name = "txtMinAlarm";
            this.txtMinAlarm.Size = new System.Drawing.Size(82, 21);
            this.txtMinAlarm.TabIndex = 13;
            this.txtMinAlarm.TextChanged += new System.EventHandler(this.txtMinAlarm_TextChanged);
            // 
            // lb_unitmax
            // 
            this.lb_unitmax.AutoSize = true;
            this.lb_unitmax.Location = new System.Drawing.Point(187, 21);
            this.lb_unitmax.Name = "lb_unitmax";
            this.lb_unitmax.Size = new System.Drawing.Size(44, 15);
            this.lb_unitmax.TabIndex = 0;
            this.lb_unitmax.Text = "deg C";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.lbl_LoggerTime);
            this.groupBox5.Controls.Add(this.lbl_LoggerDate);
            this.groupBox5.Controls.Add(this.cbbTimeZone);
            this.groupBox5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox5.Location = new System.Drawing.Point(4, 315);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(339, 114);
            this.groupBox5.TabIndex = 23;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Logger Clock";
            // 
            // lbl_LoggerTime
            // 
            this.lbl_LoggerTime.AutoSize = true;
            this.lbl_LoggerTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_LoggerTime.Location = new System.Drawing.Point(205, 71);
            this.lbl_LoggerTime.Name = "lbl_LoggerTime";
            this.lbl_LoggerTime.Size = new System.Drawing.Size(76, 25);
            this.lbl_LoggerTime.TabIndex = 0;
            this.lbl_LoggerTime.Text = "label2";
            // 
            // lbl_LoggerDate
            // 
            this.lbl_LoggerDate.AutoSize = true;
            this.lbl_LoggerDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_LoggerDate.Location = new System.Drawing.Point(54, 77);
            this.lbl_LoggerDate.Name = "lbl_LoggerDate";
            this.lbl_LoggerDate.Size = new System.Drawing.Size(41, 13);
            this.lbl_LoggerDate.TabIndex = 0;
            this.lbl_LoggerDate.Text = "label1";
            // 
            // cbbTimeZone
            // 
            this.cbbTimeZone.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbbTimeZone.FormattingEnabled = true;
            this.cbbTimeZone.Location = new System.Drawing.Point(57, 30);
            this.cbbTimeZone.Name = "cbbTimeZone";
            this.cbbTimeZone.Size = new System.Drawing.Size(243, 23);
            this.cbbTimeZone.TabIndex = 3;
            this.cbbTimeZone.SelectedValueChanged += new System.EventHandler(this.cbbTimeZone_SelectedValueChanged);
            // 
            // lbl_min
            // 
            this.lbl_min.AutoSize = true;
            this.lbl_min.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_min.Location = new System.Drawing.Point(4, 59);
            this.lbl_min.Name = "lbl_min";
            this.lbl_min.Size = new System.Drawing.Size(55, 13);
            this.lbl_min.TabIndex = 0;
            this.lbl_min.Text = "Minimum";
            // 
            // lbl_max
            // 
            this.lbl_max.AutoSize = true;
            this.lbl_max.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_max.Location = new System.Drawing.Point(4, 27);
            this.lbl_max.Name = "lbl_max";
            this.lbl_max.Size = new System.Drawing.Size(58, 13);
            this.lbl_max.TabIndex = 0;
            this.lbl_max.Text = "Maximum";
            // 
            // lblSR
            // 
            this.lblSR.AutoSize = true;
            this.lblSR.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSR.Location = new System.Drawing.Point(21, 156);
            this.lblSR.Name = "lblSR";
            this.lblSR.Size = new System.Drawing.Size(84, 13);
            this.lblSR.TabIndex = 0;
            this.lblSR.Text = "Sampling rate";
            // 
            // lb_unitmin
            // 
            this.lb_unitmin.AutoSize = true;
            this.lb_unitmin.Location = new System.Drawing.Point(190, 57);
            this.lb_unitmin.Name = "lb_unitmin";
            this.lb_unitmin.Size = new System.Drawing.Size(44, 15);
            this.lb_unitmin.TabIndex = 0;
            this.lb_unitmin.Text = "deg C";
            // 
            // txtChlDesc
            // 
            this.txtChlDesc.Location = new System.Drawing.Point(118, 45);
            this.txtChlDesc.Name = "txtChlDesc";
            this.txtChlDesc.Size = new System.Drawing.Size(159, 20);
            this.txtChlDesc.TabIndex = 8;
            // 
            // txtSerial
            // 
            this.txtSerial.ForeColor = System.Drawing.SystemColors.Highlight;
            this.txtSerial.Location = new System.Drawing.Point(128, 25);
            this.txtSerial.Name = "txtSerial";
            this.txtSerial.Size = new System.Drawing.Size(171, 21);
            this.txtSerial.TabIndex = 6;
            this.txtSerial.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSerial_KeyPress);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtSerial);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.txtLocation);
            this.groupBox1.Controls.Add(this.txtDescription);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(5, 7);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(339, 157);
            this.groupBox1.TabIndex = 24;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Description of Recording";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.Red;
            this.label8.Location = new System.Drawing.Point(32, 122);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(217, 14);
            this.label8.TabIndex = 0;
            this.label8.Text = "(Max. 40 charaters and can\'t have <>?/:\"|\\*)";
            // 
            // txtLocation
            // 
            this.txtLocation.Location = new System.Drawing.Point(127, 89);
            this.txtLocation.Name = "txtLocation";
            this.txtLocation.Size = new System.Drawing.Size(172, 21);
            this.txtLocation.TabIndex = 2;
            this.txtLocation.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtLocation_KeyPress);
            // 
            // txtDescription
            // 
            this.txtDescription.Location = new System.Drawing.Point(127, 57);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(172, 21);
            this.txtDescription.TabIndex = 1;
            this.txtDescription.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtDescription_KeyPress);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(23, 92);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Location";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(23, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Description";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(23, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Model Number";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.lb_interval);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.cbbDuration);
            this.groupBox2.Controls.Add(this.cbbStartDelay);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(5, 169);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(339, 134);
            this.groupBox2.TabIndex = 25;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Recording";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(245, 33);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(31, 13);
            this.label7.TabIndex = 0;
            this.label7.Text = "Min.";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(245, 70);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(33, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "Day.";
            // 
            // lb_interval
            // 
            this.lb_interval.AutoSize = true;
            this.lb_interval.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_interval.ForeColor = System.Drawing.Color.Red;
            this.lb_interval.Location = new System.Drawing.Point(18, 106);
            this.lb_interval.Name = "lb_interval";
            this.lb_interval.Size = new System.Drawing.Size(70, 14);
            this.lb_interval.TabIndex = 0;
            this.lb_interval.Text = "Max 10 digits";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(6, 70);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(55, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "Duration";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(6, 31);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(70, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Start Delay";
            // 
            // cbbDuration
            // 
            this.cbbDuration.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.cbbDuration.FormattingEnabled = true;
            this.cbbDuration.Items.AddRange(new object[] {
            "1",
            "4",
            "7",
            "16",
            "30",
            "60",
            "90",
            "365"});
            this.cbbDuration.Location = new System.Drawing.Point(105, 65);
            this.cbbDuration.Name = "cbbDuration";
            this.cbbDuration.Size = new System.Drawing.Size(107, 23);
            this.cbbDuration.TabIndex = 6;
            this.cbbDuration.SelectedIndexChanged += new System.EventHandler(this.cbbDuration_SelectedIndexChanged);
            // 
            // cbbStartDelay
            // 
            this.cbbStartDelay.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.cbbStartDelay.FormattingEnabled = true;
            this.cbbStartDelay.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "5",
            "10",
            "20"});
            this.cbbStartDelay.Location = new System.Drawing.Point(105, 28);
            this.cbbStartDelay.Name = "cbbStartDelay";
            this.cbbStartDelay.Size = new System.Drawing.Size(107, 23);
            this.cbbStartDelay.TabIndex = 5;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.lblHZ);
            this.groupBox4.Controls.Add(this.lblG);
            this.groupBox4.Controls.Add(this.cbbBR);
            this.groupBox4.Controls.Add(this.cbbFS);
            this.groupBox4.Controls.Add(this.lblSR);
            this.groupBox4.Controls.Add(this.lblFS);
            this.groupBox4.Controls.Add(this.txtChlDesc);
            this.groupBox4.Controls.Add(this.cbbChannel);
            this.groupBox4.Controls.Add(this.cbbUnit);
            this.groupBox4.Controls.Add(this.label14);
            this.groupBox4.Controls.Add(this.label15);
            this.groupBox4.Controls.Add(this.label13);
            this.groupBox4.Controls.Add(this.groupBox6);
            this.groupBox4.Location = new System.Drawing.Point(349, 127);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(288, 305);
            this.groupBox4.TabIndex = 27;
            this.groupBox4.TabStop = false;
            // 
            // cbbChannel
            // 
            this.cbbChannel.FormattingEnabled = true;
            this.cbbChannel.Location = new System.Drawing.Point(21, 44);
            this.cbbChannel.Name = "cbbChannel";
            this.cbbChannel.Size = new System.Drawing.Size(63, 21);
            this.cbbChannel.TabIndex = 7;
            this.cbbChannel.SelectedIndexChanged += new System.EventHandler(this.cbbChannel_SelectedIndexChanged);
            // 
            // cbbUnit
            // 
            this.cbbUnit.FormattingEnabled = true;
            this.cbbUnit.Items.AddRange(new object[] {
            "Not Use",
            "Celsius",
            "Fahrenheit",
            "G",
            "CO2",
            "%RH"});
            this.cbbUnit.Location = new System.Drawing.Point(120, 80);
            this.cbbUnit.Name = "cbbUnit";
            this.cbbUnit.Size = new System.Drawing.Size(102, 21);
            this.cbbUnit.TabIndex = 9;
            this.cbbUnit.SelectedIndexChanged += new System.EventHandler(this.cbbUnit_SelectedIndexChanged);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(115, 20);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(82, 13);
            this.label14.TabIndex = 0;
            this.label14.Text = "Description1 ";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(41, 83);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(30, 13);
            this.label15.TabIndex = 0;
            this.label15.Text = "Unit";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(21, 20);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(50, 13);
            this.label13.TabIndex = 0;
            this.label13.Text = "Chanel ";
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.chbNoAlarm);
            this.groupBox6.Controls.Add(this.txtMaxAlarm);
            this.groupBox6.Controls.Add(this.txtMinAlarm);
            this.groupBox6.Controls.Add(this.lb_unitmax);
            this.groupBox6.Controls.Add(this.lb_unitmin);
            this.groupBox6.Controls.Add(this.lbl_min);
            this.groupBox6.Controls.Add(this.lbl_max);
            this.groupBox6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox6.Location = new System.Drawing.Point(10, 186);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(267, 106);
            this.groupBox6.TabIndex = 0;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Alarm";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.chkChannel);
            this.groupBox3.Controls.Add(this.chkRecord);
            this.groupBox3.Controls.Add(this.chkDes);
            this.groupBox3.Controls.Add(this.chkLoc);
            this.groupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(349, 7);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(287, 117);
            this.groupBox3.TabIndex = 31;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Multi Choice";
            // 
            // chkChannel
            // 
            this.chkChannel.AutoSize = true;
            this.chkChannel.Checked = true;
            this.chkChannel.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkChannel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkChannel.Location = new System.Drawing.Point(135, 30);
            this.chkChannel.Name = "chkChannel";
            this.chkChannel.Size = new System.Drawing.Size(153, 19);
            this.chkChannel.TabIndex = 0;
            this.chkChannel.Text = "Write Channel\'s Setting";
            this.chkChannel.UseVisualStyleBackColor = true;
            this.chkChannel.CheckStateChanged += new System.EventHandler(this.chkChannel_CheckStateChanged);
            // 
            // chkRecord
            // 
            this.chkRecord.AutoSize = true;
            this.chkRecord.Checked = true;
            this.chkRecord.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkRecord.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkRecord.Location = new System.Drawing.Point(138, 74);
            this.chkRecord.Name = "chkRecord";
            this.chkRecord.Size = new System.Drawing.Size(138, 19);
            this.chkRecord.TabIndex = 0;
            this.chkRecord.Text = "Write Record Setting";
            this.chkRecord.UseVisualStyleBackColor = true;
            this.chkRecord.CheckStateChanged += new System.EventHandler(this.chkRecord_CheckStateChanged);
            // 
            // chkDes
            // 
            this.chkDes.AutoSize = true;
            this.chkDes.Checked = true;
            this.chkDes.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDes.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkDes.Location = new System.Drawing.Point(29, 32);
            this.chkDes.Name = "chkDes";
            this.chkDes.Size = new System.Drawing.Size(79, 19);
            this.chkDes.TabIndex = 0;
            this.chkDes.Text = "Write Des";
            this.chkDes.UseVisualStyleBackColor = true;
            this.chkDes.CheckStateChanged += new System.EventHandler(this.chkDes_CheckStateChanged);
            // 
            // chkLoc
            // 
            this.chkLoc.AutoSize = true;
            this.chkLoc.Checked = true;
            this.chkLoc.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkLoc.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkLoc.Location = new System.Drawing.Point(29, 73);
            this.chkLoc.Name = "chkLoc";
            this.chkLoc.Size = new System.Drawing.Size(77, 19);
            this.chkLoc.TabIndex = 0;
            this.chkLoc.Text = "Write Loc";
            this.chkLoc.UseVisualStyleBackColor = true;
            this.chkLoc.CheckStateChanged += new System.EventHandler(this.chkLoc_CheckStateChanged);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // Setting35
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(644, 612);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.rtxtEventLog);
            this.Controls.Add(this.btnEraseAllData);
            this.Controls.Add(this.btnReadSetting);
            this.Controls.Add(this.btnWriteSetting);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox4);
            this.Name = "Setting35";
            this.Text = "Setting35";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Setting35_FormClosed);
            this.Load += new System.EventHandler(this.Setting35_Load);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbbFS;
        private System.Windows.Forms.Label lblFS;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.RichTextBox rtxtEventLog;
        private System.Windows.Forms.Button btnEraseAllData;
        private System.Windows.Forms.Button btnReadSetting;
        private System.Windows.Forms.Button btnWriteSetting;
        private System.Windows.Forms.Label lblHZ;
        private System.Windows.Forms.Label lblG;
        private System.Windows.Forms.ComboBox cbbBR;
        private System.Windows.Forms.CheckBox chbNoAlarm;
        private System.Windows.Forms.TextBox txtMaxAlarm;
        private System.Windows.Forms.TextBox txtMinAlarm;
        private System.Windows.Forms.Label lb_unitmax;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label lbl_LoggerTime;
        private System.Windows.Forms.Label lbl_LoggerDate;
        private System.Windows.Forms.ComboBox cbbTimeZone;
        private System.Windows.Forms.Label lbl_min;
        private System.Windows.Forms.Label lbl_max;
        private System.Windows.Forms.Label lblSR;
        private System.Windows.Forms.Timer Tim_UpdateClock;
        private System.Windows.Forms.Label lb_unitmin;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.TextBox txtChlDesc;
        private System.Windows.Forms.TextBox txtSerial;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtLocation;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lb_interval;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cbbDuration;
        private System.Windows.Forms.ComboBox cbbStartDelay;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.ComboBox cbbChannel;
        private System.Windows.Forms.ComboBox cbbUnit;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox chkChannel;
        private System.Windows.Forms.CheckBox chkRecord;
        private System.Windows.Forms.CheckBox chkDes;
        private System.Windows.Forms.CheckBox chkLoc;
        private System.Windows.Forms.Timer timer1;
    }
}