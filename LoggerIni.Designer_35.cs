namespace Pexo16
{
    partial class LoggerIni
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.lblSerial = new System.Windows.Forms.Label();
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
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.tbCurrentTimeZone = new System.Windows.Forms.TextBox();
            this.lblCurrentTime = new System.Windows.Forms.Label();
            this.lblCurrentDate = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.txtChlDesc = new System.Windows.Forms.TextBox();
            this.cbbChannel = new System.Windows.Forms.ComboBox();
            this.cbbUnit = new System.Windows.Forms.ComboBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.chbNoAlarm = new System.Windows.Forms.CheckBox();
            this.txtMaxAlarm = new System.Windows.Forms.TextBox();
            this.txtMinAlarm = new System.Windows.Forms.TextBox();
            this.lb_unitmax = new System.Windows.Forms.Label();
            this.lb_unitmin = new System.Windows.Forms.Label();
            this.lbl_min = new System.Windows.Forms.Label();
            this.lbl_max = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.chbDayLight = new System.Windows.Forms.CheckBox();
            this.lbl_LoggerTime = new System.Windows.Forms.Label();
            this.lbl_LoggerDate = new System.Windows.Forms.Label();
            this.cbbTimeZone = new System.Windows.Forms.ComboBox();
            this.btnWriteSetting = new System.Windows.Forms.Button();
            this.btnReadSetting = new System.Windows.Forms.Button();
            this.btnNetWorkSetting = new System.Windows.Forms.Button();
            this.btnEraseAllData = new System.Windows.Forms.Button();
            this.btnEmailSetting = new System.Windows.Forms.Button();
            this.chbAutoStart = new System.Windows.Forms.CheckBox();
            this.rtxtEventLog = new System.Windows.Forms.RichTextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.Tim_UpdateClock = new System.Windows.Forms.Timer(this.components);
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.lblSerial);
            this.groupBox1.Controls.Add(this.txtLocation);
            this.groupBox1.Controls.Add(this.txtDescription);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(13, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(339, 135);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Description of Recording";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.Red;
            this.label8.Location = new System.Drawing.Point(32, 111);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(217, 14);
            this.label8.TabIndex = 6;
            this.label8.Text = "(Max. 40 charaters and can\'t have <>?/:\"|\\*)";
            // 
            // lblSerial
            // 
            this.lblSerial.AutoSize = true;
            this.lblSerial.ForeColor = System.Drawing.SystemColors.Highlight;
            this.lblSerial.Location = new System.Drawing.Point(206, 21);
            this.lblSerial.Name = "lblSerial";
            this.lblSerial.Size = new System.Drawing.Size(47, 15);
            this.lblSerial.TabIndex = 5;
            this.lblSerial.Text = "label4";
            // 
            // txtLocation
            // 
            this.txtLocation.Location = new System.Drawing.Point(127, 84);
            this.txtLocation.Name = "txtLocation";
            this.txtLocation.Size = new System.Drawing.Size(172, 21);
            this.txtLocation.TabIndex = 4;
            this.txtLocation.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtLocation_KeyPress);
            // 
            // txtDescription
            // 
            this.txtDescription.Location = new System.Drawing.Point(127, 49);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(172, 21);
            this.txtDescription.TabIndex = 3;
            this.txtDescription.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtDescription_KeyPress);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(23, 84);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Location";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(23, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 13);
            this.label2.TabIndex = 1;
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
            this.groupBox2.Location = new System.Drawing.Point(358, 13);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(219, 114);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Recording";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(182, 33);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(31, 13);
            this.label7.TabIndex = 1;
            this.label7.Text = "Min.";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(180, 70);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(33, 13);
            this.label6.TabIndex = 1;
            this.label6.Text = "Day.";
            // 
            // lb_interval
            // 
            this.lb_interval.AutoSize = true;
            this.lb_interval.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_interval.ForeColor = System.Drawing.Color.Red;
            this.lb_interval.Location = new System.Drawing.Point(18, 96);
            this.lb_interval.Name = "lb_interval";
            this.lb_interval.Size = new System.Drawing.Size(70, 14);
            this.lb_interval.TabIndex = 1;
            this.lb_interval.Text = "Max 10 digits";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(6, 70);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(55, 13);
            this.label5.TabIndex = 1;
            this.label5.Text = "Duration";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(6, 31);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(70, 13);
            this.label4.TabIndex = 1;
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
            this.cbbDuration.Location = new System.Drawing.Point(91, 65);
            this.cbbDuration.Name = "cbbDuration";
            this.cbbDuration.Size = new System.Drawing.Size(85, 23);
            this.cbbDuration.TabIndex = 0;
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
            this.cbbStartDelay.Location = new System.Drawing.Point(91, 28);
            this.cbbStartDelay.Name = "cbbStartDelay";
            this.cbbStartDelay.Size = new System.Drawing.Size(85, 23);
            this.cbbStartDelay.TabIndex = 0;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.tbCurrentTimeZone);
            this.groupBox3.Controls.Add(this.lblCurrentTime);
            this.groupBox3.Controls.Add(this.lblCurrentDate);
            this.groupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(12, 150);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(339, 92);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Current Clock";
            // 
            // tbCurrentTimeZone
            // 
            this.tbCurrentTimeZone.BackColor = System.Drawing.SystemColors.ControlLight;
            this.tbCurrentTimeZone.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbCurrentTimeZone.Location = new System.Drawing.Point(47, 26);
            this.tbCurrentTimeZone.Name = "tbCurrentTimeZone";
            this.tbCurrentTimeZone.Size = new System.Drawing.Size(243, 21);
            this.tbCurrentTimeZone.TabIndex = 2;
            // 
            // lblCurrentTime
            // 
            this.lblCurrentTime.AutoSize = true;
            this.lblCurrentTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrentTime.Location = new System.Drawing.Point(204, 56);
            this.lblCurrentTime.Name = "lblCurrentTime";
            this.lblCurrentTime.Size = new System.Drawing.Size(76, 25);
            this.lblCurrentTime.TabIndex = 1;
            this.lblCurrentTime.Text = "label2";
            // 
            // lblCurrentDate
            // 
            this.lblCurrentDate.AutoSize = true;
            this.lblCurrentDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrentDate.Location = new System.Drawing.Point(53, 65);
            this.lblCurrentDate.Name = "lblCurrentDate";
            this.lblCurrentDate.Size = new System.Drawing.Size(41, 13);
            this.lblCurrentDate.TabIndex = 0;
            this.lblCurrentDate.Text = "label1";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.txtChlDesc);
            this.groupBox4.Controls.Add(this.cbbChannel);
            this.groupBox4.Controls.Add(this.cbbUnit);
            this.groupBox4.Controls.Add(this.label14);
            this.groupBox4.Controls.Add(this.label15);
            this.groupBox4.Controls.Add(this.label13);
            this.groupBox4.Controls.Add(this.groupBox6);
            this.groupBox4.Location = new System.Drawing.Point(357, 133);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(219, 237);
            this.groupBox4.TabIndex = 0;
            this.groupBox4.TabStop = false;
            // 
            // txtChlDesc
            // 
            this.txtChlDesc.Location = new System.Drawing.Point(92, 44);
            this.txtChlDesc.Name = "txtChlDesc";
            this.txtChlDesc.Size = new System.Drawing.Size(118, 20);
            this.txtChlDesc.TabIndex = 3;
            this.txtChlDesc.Validating += new System.ComponentModel.CancelEventHandler(this.txtChlDesc_Validating);
            // 
            // cbbChannel
            // 
            this.cbbChannel.FormattingEnabled = true;
            this.cbbChannel.Location = new System.Drawing.Point(21, 44);
            this.cbbChannel.Name = "cbbChannel";
            this.cbbChannel.Size = new System.Drawing.Size(63, 21);
            this.cbbChannel.TabIndex = 2;
            this.cbbChannel.SelectedIndexChanged += new System.EventHandler(this.cbbChannel_SelectedIndexChanged);
            // 
            // cbbUnit
            // 
            this.cbbUnit.FormattingEnabled = true;
            this.cbbUnit.Items.AddRange(new object[] {
            "Not Use",
            "Celsius",
            "Fahrenheit",
            "CO2",
            "%RH"});
            this.cbbUnit.Location = new System.Drawing.Point(92, 84);
            this.cbbUnit.Name = "cbbUnit";
            this.cbbUnit.Size = new System.Drawing.Size(121, 21);
            this.cbbUnit.TabIndex = 2;
            this.cbbUnit.SelectedIndexChanged += new System.EventHandler(this.cbbUnit_SelectedIndexChanged);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(115, 20);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(75, 13);
            this.label14.TabIndex = 1;
            this.label14.Text = "Description ";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(32, 87);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(30, 13);
            this.label15.TabIndex = 1;
            this.label15.Text = "Unit";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(21, 20);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(50, 13);
            this.label13.TabIndex = 1;
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
            this.groupBox6.Location = new System.Drawing.Point(10, 125);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(200, 106);
            this.groupBox6.TabIndex = 0;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Alarm";
            // 
            // chbNoAlarm
            // 
            this.chbNoAlarm.AutoSize = true;
            this.chbNoAlarm.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chbNoAlarm.ForeColor = System.Drawing.SystemColors.Highlight;
            this.chbNoAlarm.Location = new System.Drawing.Point(11, 84);
            this.chbNoAlarm.Name = "chbNoAlarm";
            this.chbNoAlarm.Size = new System.Drawing.Size(77, 17);
            this.chbNoAlarm.TabIndex = 2;
            this.chbNoAlarm.Text = "No Alarm";
            this.chbNoAlarm.UseVisualStyleBackColor = true;
            this.chbNoAlarm.CheckedChanged += new System.EventHandler(this.chbNoAlarm_CheckedChanged);
            // 
            // txtMaxAlarm
            // 
            this.txtMaxAlarm.Location = new System.Drawing.Point(77, 20);
            this.txtMaxAlarm.Name = "txtMaxAlarm";
            this.txtMaxAlarm.Size = new System.Drawing.Size(69, 21);
            this.txtMaxAlarm.TabIndex = 1;
            this.txtMaxAlarm.TextChanged += new System.EventHandler(this.txtMaxAlarm_TextChanged);
            this.txtMaxAlarm.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtMaxAlarm_KeyPress);
            // 
            // txtMinAlarm
            // 
            this.txtMinAlarm.Location = new System.Drawing.Point(77, 56);
            this.txtMinAlarm.Name = "txtMinAlarm";
            this.txtMinAlarm.Size = new System.Drawing.Size(69, 21);
            this.txtMinAlarm.TabIndex = 1;
            this.txtMinAlarm.TextChanged += new System.EventHandler(this.txtMinAlarm_TextChanged);
            this.txtMinAlarm.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtMinAlarm_KeyPress);
            // 
            // lb_unitmax
            // 
            this.lb_unitmax.AutoSize = true;
            this.lb_unitmax.Location = new System.Drawing.Point(152, 21);
            this.lb_unitmax.Name = "lb_unitmax";
            this.lb_unitmax.Size = new System.Drawing.Size(44, 15);
            this.lb_unitmax.TabIndex = 0;
            this.lb_unitmax.Text = "deg C";
            // 
            // lb_unitmin
            // 
            this.lb_unitmin.AutoSize = true;
            this.lb_unitmin.Location = new System.Drawing.Point(152, 57);
            this.lb_unitmin.Name = "lb_unitmin";
            this.lb_unitmin.Size = new System.Drawing.Size(44, 15);
            this.lb_unitmin.TabIndex = 0;
            this.lb_unitmin.Text = "deg C";
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
            this.lbl_max.Size = new System.Drawing.Size(65, 13);
            this.lbl_max.TabIndex = 0;
            this.lbl_max.Text = "Maxnimum";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.chbDayLight);
            this.groupBox5.Controls.Add(this.lbl_LoggerTime);
            this.groupBox5.Controls.Add(this.lbl_LoggerDate);
            this.groupBox5.Controls.Add(this.cbbTimeZone);
            this.groupBox5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox5.Location = new System.Drawing.Point(12, 251);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(339, 119);
            this.groupBox5.TabIndex = 0;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Logger Clock";
            // 
            // chbDayLight
            // 
            this.chbDayLight.AutoSize = true;
            this.chbDayLight.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chbDayLight.Location = new System.Drawing.Point(6, 62);
            this.chbDayLight.Name = "chbDayLight";
            this.chbDayLight.Size = new System.Drawing.Size(172, 17);
            this.chbDayLight.TabIndex = 3;
            this.chbDayLight.Text = "Auto Adjust Daylight Time";
            this.chbDayLight.UseVisualStyleBackColor = true;
            // 
            // lbl_LoggerTime
            // 
            this.lbl_LoggerTime.AutoSize = true;
            this.lbl_LoggerTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_LoggerTime.Location = new System.Drawing.Point(205, 82);
            this.lbl_LoggerTime.Name = "lbl_LoggerTime";
            this.lbl_LoggerTime.Size = new System.Drawing.Size(76, 25);
            this.lbl_LoggerTime.TabIndex = 2;
            this.lbl_LoggerTime.Text = "label2";
            // 
            // lbl_LoggerDate
            // 
            this.lbl_LoggerDate.AutoSize = true;
            this.lbl_LoggerDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_LoggerDate.Location = new System.Drawing.Point(54, 91);
            this.lbl_LoggerDate.Name = "lbl_LoggerDate";
            this.lbl_LoggerDate.Size = new System.Drawing.Size(41, 13);
            this.lbl_LoggerDate.TabIndex = 1;
            this.lbl_LoggerDate.Text = "label1";
            // 
            // cbbTimeZone
            // 
            this.cbbTimeZone.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbbTimeZone.FormattingEnabled = true;
            this.cbbTimeZone.Location = new System.Drawing.Point(57, 25);
            this.cbbTimeZone.Name = "cbbTimeZone";
            this.cbbTimeZone.Size = new System.Drawing.Size(243, 23);
            this.cbbTimeZone.TabIndex = 0;
            this.cbbTimeZone.SelectedValueChanged += new System.EventHandler(this.cbbTimeZone_SelectedValueChanged);
            // 
            // btnWriteSetting
            // 
            this.btnWriteSetting.Location = new System.Drawing.Point(461, 409);
            this.btnWriteSetting.Name = "btnWriteSetting";
            this.btnWriteSetting.Size = new System.Drawing.Size(106, 23);
            this.btnWriteSetting.TabIndex = 1;
            this.btnWriteSetting.Text = "Write Setting";
            this.btnWriteSetting.UseVisualStyleBackColor = true;
            this.btnWriteSetting.Click += new System.EventHandler(this.btnWriteSetting_Click);
            // 
            // btnReadSetting
            // 
            this.btnReadSetting.Location = new System.Drawing.Point(461, 438);
            this.btnReadSetting.Name = "btnReadSetting";
            this.btnReadSetting.Size = new System.Drawing.Size(106, 23);
            this.btnReadSetting.TabIndex = 1;
            this.btnReadSetting.Text = "Read Setting";
            this.btnReadSetting.UseVisualStyleBackColor = true;
            this.btnReadSetting.Click += new System.EventHandler(this.btnReadSetting_Click);
            // 
            // btnNetWorkSetting
            // 
            this.btnNetWorkSetting.Location = new System.Drawing.Point(461, 467);
            this.btnNetWorkSetting.Name = "btnNetWorkSetting";
            this.btnNetWorkSetting.Size = new System.Drawing.Size(106, 23);
            this.btnNetWorkSetting.TabIndex = 1;
            this.btnNetWorkSetting.Text = "Network Setting";
            this.btnNetWorkSetting.UseVisualStyleBackColor = true;
            // 
            // btnEraseAllData
            // 
            this.btnEraseAllData.Location = new System.Drawing.Point(461, 496);
            this.btnEraseAllData.Name = "btnEraseAllData";
            this.btnEraseAllData.Size = new System.Drawing.Size(106, 23);
            this.btnEraseAllData.TabIndex = 1;
            this.btnEraseAllData.Text = "Erase All Data ";
            this.btnEraseAllData.UseVisualStyleBackColor = true;
            this.btnEraseAllData.Click += new System.EventHandler(this.btnEraseAllData_Click);
            // 
            // btnEmailSetting
            // 
            this.btnEmailSetting.Location = new System.Drawing.Point(461, 525);
            this.btnEmailSetting.Name = "btnEmailSetting";
            this.btnEmailSetting.Size = new System.Drawing.Size(106, 23);
            this.btnEmailSetting.TabIndex = 1;
            this.btnEmailSetting.Text = "Email Setting";
            this.btnEmailSetting.UseVisualStyleBackColor = true;
            // 
            // chbAutoStart
            // 
            this.chbAutoStart.AutoSize = true;
            this.chbAutoStart.Checked = true;
            this.chbAutoStart.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbAutoStart.Location = new System.Drawing.Point(475, 385);
            this.chbAutoStart.Name = "chbAutoStart";
            this.chbAutoStart.Size = new System.Drawing.Size(73, 17);
            this.chbAutoStart.TabIndex = 2;
            this.chbAutoStart.Text = "Auto Start";
            this.chbAutoStart.UseVisualStyleBackColor = true;
            // 
            // rtxtEventLog
            // 
            this.rtxtEventLog.BackColor = System.Drawing.SystemColors.Control;
            this.rtxtEventLog.Location = new System.Drawing.Point(13, 409);
            this.rtxtEventLog.Name = "rtxtEventLog";
            this.rtxtEventLog.Size = new System.Drawing.Size(413, 139);
            this.rtxtEventLog.TabIndex = 3;
            this.rtxtEventLog.Text = "";
            // 
            // label16
            // 
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.Location = new System.Drawing.Point(191, 489);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(59, 23);
            this.label16.TabIndex = 4;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.Location = new System.Drawing.Point(15, 385);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(70, 15);
            this.label17.TabIndex = 4;
            this.label17.Text = "Event Log";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.ForeColor = System.Drawing.Color.Red;
            this.lblStatus.Location = new System.Drawing.Point(219, 382);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(50, 18);
            this.lblStatus.TabIndex = 6;
            this.lblStatus.Text = "Status";
            // 
            // Tim_UpdateClock
            // 
            this.Tim_UpdateClock.Tick += new System.EventHandler(this.Tim_UpdateClock_Tick);
            // 
            // LoggerIni
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(589, 554);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.rtxtEventLog);
            this.Controls.Add(this.chbAutoStart);
            this.Controls.Add(this.btnEmailSetting);
            this.Controls.Add(this.btnEraseAllData);
            this.Controls.Add(this.btnNetWorkSetting);
            this.Controls.Add(this.btnReadSetting);
            this.Controls.Add(this.btnWriteSetting);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "LoggerIni";
            this.Text = "frmLoggerIni";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.LoggerIni_FormClosed);
            this.Load += new System.EventHandler(this.frmLoggerIni_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label lblCurrentTime;
        private System.Windows.Forms.Label lblCurrentDate;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.TextBox tbCurrentTimeZone;
        private System.Windows.Forms.ComboBox cbbTimeZone;
        private System.Windows.Forms.Label lbl_LoggerTime;
        private System.Windows.Forms.Label lbl_LoggerDate;
        private System.Windows.Forms.Label lblSerial;
        private System.Windows.Forms.TextBox txtLocation;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lb_interval;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cbbDuration;
        private System.Windows.Forms.ComboBox cbbStartDelay;
        private System.Windows.Forms.TextBox txtChlDesc;
        private System.Windows.Forms.ComboBox cbbChannel;
        private System.Windows.Forms.ComboBox cbbUnit;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.CheckBox chbNoAlarm;
        private System.Windows.Forms.TextBox txtMaxAlarm;
        private System.Windows.Forms.TextBox txtMinAlarm;
        private System.Windows.Forms.Label lb_unitmax;
        private System.Windows.Forms.Label lb_unitmin;
        private System.Windows.Forms.Label lbl_min;
        private System.Windows.Forms.Label lbl_max;
        private System.Windows.Forms.CheckBox chbDayLight;
        private System.Windows.Forms.Button btnWriteSetting;
        private System.Windows.Forms.Button btnReadSetting;
        private System.Windows.Forms.Button btnNetWorkSetting;
        private System.Windows.Forms.Button btnEraseAllData;
        private System.Windows.Forms.Button btnEmailSetting;
        private System.Windows.Forms.CheckBox chbAutoStart;
        private System.Windows.Forms.RichTextBox rtxtEventLog;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Timer Tim_UpdateClock;
        private System.Windows.Forms.Label label8;
    }
}