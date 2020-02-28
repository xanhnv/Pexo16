//using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Resources;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Pexo16
{
    public partial class LoggerIni_35 : Form
    {
        //Device selectedDevice = null;//X
        public Device35 device35 = null;
        Graph saveGraph = null;

        private byte g_channel;
        private byte Logging;
        private DateTime LoggerTime;
        private byte g_unit;
        private byte g_sensor;
        Int16 firstHex;
        Int16 lastHex;
        //private string ChlDesc;
        // private string selectedHostPort;
        public string defaultFolder = "";
        private bool sttCF; // bien nay dung de khong che, k cho goi ham draw_graph 2 lan
        public byte[] datalogger;
        public DateTime _logger_date;
        long[] len = new long[9];

        ResourceManager res_man = new ResourceManager("Pexo16.Lang.Resources", typeof(LoggerIni_35).Assembly);
        CultureInfo cul;


        private Progress _Progress;


        //ToolStripComboBox tscb_timezone;
        private byte tmpChannel;

        public LoggerIni_35(string HostPort)
        {
            switch (mGlobal.language)
            {
                case "Spanish":
                    cul = CultureInfo.CreateSpecificCulture("es-ES");
                    break;
                case "Korean":
                    cul = CultureInfo.CreateSpecificCulture("ko-KR");
                    break;
                case "Japanese":
                    cul = CultureInfo.CreateSpecificCulture("ja-JP");
                    break;
                default:
                    cul = CultureInfo.CreateSpecificCulture("en-US");
                    break;
            }


            InitializeComponent();

            //selectedDevice = Device.DelInstance();//xoa instance truoc do.//X
            //selectedDevice = Device.Instance;// tao mot instance moi.//X
            device35 = Device35.DelInstance();
            device35 = Device35.Instance;

            device35.Channels = new Channel[4];
            for (int i = 0; i < 4; i++)
            {
                device35.Channels[i] = new Channel();
            }

            saveGraph = Graph.DelInstance();//xoa instance da tao truoc do.
            saveGraph = Graph.Instance;//tao mot instance moi.
            //selectedDevice.hostport = HostPort;//X
            device35.hostport = HostPort;

            mGlobal.unitFromFile = false;

        }


        private void SystemEvent_TimeChanged(object sender, EventArgs e)
        {
            CultureInfo.CurrentCulture.ClearCachedData();

            tbCurrentTimeZone.Text = TimeZoneInfo.Local.ToString();
            cbbTimeZone.Text = TimeZoneInfo.Local.ToString();

            lblCurrentDate.Text = DateTime.Now.ToShortDateString();
            lblCurrentTime.Text = DateTime.Now.ToShortTimeString();
            lbl_LoggerDate.Text = DateTime.Now.ToShortDateString();
            lbl_LoggerTime.Text = DateTime.Now.ToShortTimeString();
        }

        private void LoggerIni_35_Load(object sender, EventArgs e)
        {
            CenterToScreen();
            switch (mGlobal.language)
            {
                case "Spanish":
                    cul = CultureInfo.CreateSpecificCulture("es-ES");
                    break;
                case "Korean":
                    cul = CultureInfo.CreateSpecificCulture("ko-KR");
                    break;
                case "Japanese":
                    cul = CultureInfo.CreateSpecificCulture("ja-JP");
                    break;
                default:
                    cul = CultureInfo.CreateSpecificCulture("en-US");
                    break;
            }

            groupBox1.Text = res_man.GetString("Description of Recording", cul);
            label1.Text = res_man.GetString("Model Number", cul);
            label2.Text = res_man.GetString("Description", cul);
            label3.Text = res_man.GetString("Location", cul);
            label8.Text = res_man.GetString("(Max. 40 charaters and can't have <>?/:\"|\\*)", cul);
            groupBox3.Text = res_man.GetString("Current Clock", cul);
            groupBox5.Text = res_man.GetString("Logger Clock", cul);
            chbDayLight.Text = res_man.GetString("Auto Adjust Daylight Time", cul);
            label17.Text = res_man.GetString("Event Log", cul);
            groupBox2.Text = res_man.GetString("Recording", cul);
            label4.Text = res_man.GetString("Start Delay", cul);
            label5.Text = res_man.GetString("Duration", cul);
            //lb_interval.Text = res_man.GetString("Max 10 digits", cul);
            label6.Text = res_man.GetString("Day", cul);
            label7.Text = res_man.GetString("Min", cul);
            label13.Text = res_man.GetString("Chanel", cul);
            label14.Text = res_man.GetString("Description", cul);
            label15.Text = res_man.GetString("Unit", cul);
            groupBox6.Text = res_man.GetString("Alarm", cul);
            lbl_max.Text = res_man.GetString("Maximum", cul);
            lbl_min.Text = res_man.GetString("Minimum", cul);
            chbNoAlarm.Text = res_man.GetString("No Alarm", cul);
            btnWriteSetting.Text = res_man.GetString("Write Setting", cul);
            btnReadSetting.Text = res_man.GetString("Read Setting", cul);
            btnEraseAllData.Text = res_man.GetString("Erase All Data", cul);
            btnEmail.Text = res_man.GetString("Cloud setting", cul);
            lblFS.Text = res_man.GetString("Full scale", cul);
            lblSR.Text = res_man.GetString("Sampling rate", cul);


            Microsoft.Win32.SystemEvents.TimeChanged += new EventHandler(SystemEvent_TimeChanged);

            lblCurrentDate.Text = DateTime.Now.ToShortDateString();
            lblCurrentTime.Text = DateTime.Now.ToShortTimeString();
            lbl_LoggerDate.Text = DateTime.Now.ToShortDateString();
            lbl_LoggerTime.Text = DateTime.Now.ToShortTimeString();
            tbCurrentTimeZone.Text = TimeZoneInfo.Local.ToString();

            cbbTimeZone.Text = TimeZoneInfo.Local.ToString();

            cbbTimeZone.Items.Clear();

            ReadOnlyCollection<TimeZoneInfo> timeZones = TimeZoneInfo.GetSystemTimeZones();
            foreach (TimeZoneInfo timeZoneInfo in timeZones)
            {
                cbbTimeZone.Items.Add(timeZoneInfo.DisplayName);
            }

            TimeZone zone = TimeZone.CurrentTimeZone;
            // Demonstrate ToLocalTime and ToUniversalTime.
            DateTime local = zone.ToLocalTime(DateTime.Now);
            Tim_UpdateClock.Enabled = true;

            //cbbDuration.Text = cbbDuration.Items[0].ToString();

            //tao file luu setting.

            byte[] bufSetting = new byte[360];

            string FileName = "";
            FileName = mGlobal.app_patch(FileName);


            if (System.IO.File.Exists(FileName + "\\dataSetting.txt") == true)
            {
                //read Serial
                device35.USBOpen(device35.hostport);
                txtSerial.Text = getDeviceInfo.readSerial(device35.dev);
                device35.Close();

                bufSetting = System.IO.File.ReadAllBytes(FileName + "\\dataSetting.txt");
                if (bufSetting!=null)
                {
                    if (bufSetting.Length>5)
                    {
                        device35.openSetting(bufSetting);
                    }
                    
                }
               
                txtSerial.Text = device35.Serial;


                txtDescription.Text = device35.Description;
                txtLocation.Text = device35.Location;
                if (device35.Duration != 65535)
                {
                    int sec1 = 0;
                    int min2 = 0;
                    int hour = 0;
                    hour = Convert.ToInt32(device35.Duration / 3600);
                    min2 = Convert.ToInt32(device35.Duration) / 60;
                    sec1 = Convert.ToInt32(device35.Duration) % 60;
                    nbrIntervalHour.Value = hour;
                    nbrIntervalMin.Value = min2;
                    nbrIntervalSec.Value = sec1;
                }
                else
                {
                    nbrIntervalHour.Value = 0;
                    nbrIntervalMin.Value = 0;
                    nbrIntervalSec.Value = 0;
                }

                if (device35.Delay == 255)
                {
                    cbbStartDelay.Text = cbbStartDelay.Items[0].ToString();
                }
                else
                {
                    cbbStartDelay.Text = device35.Delay.ToString();
                }

                //if (device35.Duration == 65535)
                //{
                //    cbbDuration.Text = cbbDuration.Items[0].ToString();
                //}
                //else
                //{
                //    cbbDuration.Text = device35.Duration.ToString();
                //}

                string Zone = mGlobal.FindSystemTimeZoneFromString(device35.Timezone.ToString()).ToString();
                for (int i = 0; i <= cbbTimeZone.Items.Count - 1; i++)
                {
                    if (cbbTimeZone.Items[i].ToString() == Zone)
                    {
                        cbbTimeZone.Text = cbbTimeZone.Items[i].ToString();
                    }
                }
            }
            else
            {
                //**************** Khoa tam ***********
                //sai o cbbChannel_SelectedIndexChanged - Byte.parse, chua xac dinh duoc
                btnReadSetting_Click(sender, e);
            }


            //btnReadSetting_Click(sender, e);
            //numberOfChannel'value

            cbbChannel.Items.Clear();
            for (int i = 1; i <= 4; i++)
            {
                cbbChannel.Items.Add(i);
            }

            //lblSerial.Text = device35.Serial;
            cbbChannel.Text = cbbChannel.Items[0].ToString();
            cbbChannel.SelectedIndex = 0;

            if (device35.numOfChannel != 8)
            {
                cbbUnit.Items.Clear();
                cbbUnit.Items.AddRange(new string[] { "Not Use", "Celsius", "Fahrenheit", "PPM", "%RH", "G" });
            }
            cbbChannel_SelectedIndexChanged(sender, e);
            cbbUnit_SelectedIndexChanged(sender, e);

            //cbbDuration.DropDownStyle = ComboBoxStyle.DropDownList;
            cbbStartDelay.DropDownStyle = ComboBoxStyle.DropDownList;
            cbbUnit.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        public void btnReadSetting_Click(object sender, EventArgs e)
        {
            lblStatus.Text = res_man.GetString("Reading.....Please wait", cul);
            lblStatus.Update();

            string StrDevs = device35.hostport;
            //Open Device
            if (device35.USBOpen(StrDevs) == false)
            {
                MessageBox.Show(res_man.GetString("Open USB fail. Please try again.", cul));
                lockButton(false);
                this.Close();
                return;
            }
            //read Serial
            if (device35.readSerial() == false)
            {
                MessageBox.Show("Read Eeprom fail. Please check USB caple!");
                lockButton(false);
                device35.Close();
                return;
            }
            txtSerial.Text = device35.Serial;
            //lblSerial.Text = device35.Serial;
            device35.Close();


            //read Location
            device35.USBOpen(StrDevs);
            if (device35.readLocation() == false)
            {
                MessageBox.Show(res_man.GetString("Read Location", cul) + " " + res_man.GetString("fail", cul));
                lockButton(false);
                device35.Close();
                return;
            }
            txtLocation.Text = device35.Location;
            device35.Close();


            //read Description
            device35.USBOpen(StrDevs);
            if (device35.readDescription() == false)
            {
                MessageBox.Show(res_man.GetString("Read description", cul) + " " + res_man.GetString("fail", cul));
                lockButton(false);
                device35.Close();
                return;
            }

            txtDescription.Text = device35.Description;
            device35.Close();


            //read Description
            device35.USBOpen(StrDevs);

            //--------------read Setting (Time, TimeZone, Delay, Duration)
            if (device35.readSettingDevice() == false)
            {
                MessageBox.Show(res_man.GetString("Read Setting", cul) + " " + res_man.GetString("fail", cul));
                lockButton(false);
                device35.Close();
                return;
            }


            //if (device35.byteLogging == 0x44)//running
            //{
            //    btnWriteSetting.Enabled = false;
            //    btnEraseAllData.Enabled = false;
            //}

            string TimeZone = mGlobal.FindTimeZoneByID(device35.Timezone.ToString()).ToString();
            //string TimeZone = mGlobal.FindSystemTimeZoneFromString(device35.Timezone.ToString()).ToString();
            for (int i = 0; i <= cbbTimeZone.Items.Count - 1; i++)
            {
                if (cbbTimeZone.Items[i].ToString() == TimeZone)
                {
                    cbbTimeZone.Text = cbbTimeZone.Items[i].ToString();
                }
            }
            if (device35.Duration != 65535)
            {
                int sec1 = 0;
                int min2 = 0;
                int hour = 0;
                hour = Convert.ToInt32(device35.Duration / 3600);
                min2 = Convert.ToInt32(device35.Duration) / 60;
                sec1 = Convert.ToInt32(device35.Duration) % 60;
                nbrIntervalHour.Value = hour;
                nbrIntervalMin.Value = min2;
                nbrIntervalSec.Value = sec1;
                
            }
            else
            {
                nbrIntervalHour.Value = 0;
                nbrIntervalMin.Value = 0;
                nbrIntervalSec.Value = 0;
            }

            if (device35.Delay == 255)
            {
                cbbStartDelay.Text = cbbStartDelay.Items[0].ToString();
            }
            else
            {
                cbbStartDelay.Text = device35.Delay.ToString();
            }

            //if (device35.Duration == 65535)
            //{
            //    cbbDuration.Text = cbbDuration.Items[0].ToString();
            //}
            //else
            //{
            //    cbbDuration.Text = device35.Duration.ToString();
            //}

            device35.Close();
            //device35.USBOpen(StrDevs);
            ////-----------read Setting Channel
            //if (device35.readSettingChannel() == false)
            //{
            //    MessageBox.Show(res_man.GetString("Read Setting", cul) +" "+ res_man.GetString("fail", cul));
            //    lockButton(false);
            //    device35.Close();
            //    return;
            //}
            //device35.Close();

            for (int i = 0; i < 4; i++)
            {
                device35.USBOpen(StrDevs);
                if (device35.readSettingChannel1(i) == false)
                {
                    MessageBox.Show(res_man.GetString("Read Setting", cul) + " " + res_man.GetString("fail", cul));
                    lockButton(false);
                    device35.Close();
                    return;
                }
                device35.Close();
            }

            //cbbChannel.SelectedIndex = 0;

            cbbChannel_SelectedIndexChanged(sender, e);
            //device35.Close();

            //cbbChannel.Refresh();
            //cbbUnit.Refresh();

            lblStatus.Text = ".....";
        }

        public void lockButton(bool tmp)
        {
            if (tmp == true)
            {
                btnWriteSetting.Enabled = false;
                btnReadSetting.Enabled = false;
                btnEraseAllData.Enabled = false;
            }
            else
            {
                btnWriteSetting.Enabled = true;
                btnReadSetting.Enabled = true;
                btnEraseAllData.Enabled = true;
            }
        }

        private void cbbTimeZone_SelectedValueChanged(object sender, EventArgs e)
        {
            DateTime theUTCTime = TimeZoneInfo.ConvertTimeToUtc(DateTime.Now, TimeZoneInfo.Local);

            ReadOnlyCollection<TimeZoneInfo> timeZones = TimeZoneInfo.GetSystemTimeZones();
            foreach (TimeZoneInfo timeZoneInfo in timeZones)
            {
                if (timeZoneInfo.DisplayName == cbbTimeZone.SelectedItem.ToString())
                {
                    lbl_LoggerDate.Text = TimeZoneInfo.ConvertTimeFromUtc(theUTCTime, timeZoneInfo).ToShortDateString();
                    lbl_LoggerTime.Text = TimeZoneInfo.ConvertTimeFromUtc(theUTCTime, timeZoneInfo).ToShortTimeString();
                }
            }
        }

        public void getDefaultAlarm(int max, int min, byte unit)
        {
            if (unit != 3)
            {
                max = 50;
                min = 10;
            }
            else
            {
                max = 1000;
                min = 300;
            }
        }

        private void Tim_UpdateClock_Tick(object sender, EventArgs e)
        {
            DateTime CurrentTime = DateTime.Now;
            lblCurrentDate.Text = CurrentTime.ToShortDateString();
            lblCurrentTime.Text = CurrentTime.ToString("HH:mm:ss");

            tbCurrentTimeZone.Text = TimeZoneInfo.Local.DisplayName;

            DateTime theUTCTime1 = default(DateTime);
            TimeZoneInfo localZone = TimeZoneInfo.Local;
            var theUTCTime = TimeZoneInfo.ConvertTimeToUtc(CurrentTime, localZone);
            double OffsetHour = 0;
            double OffsetMin = 0;

            try
            {
                OffsetHour = Convert.ToDouble(cbbTimeZone.Text.Substring(4, 3));
                OffsetMin = Convert.ToDouble(cbbTimeZone.Text.Substring(8, 2));
            }
            catch (Exception)
            {
                OffsetHour = 0;
                OffsetMin = 0;
            }
            LoggerTime = theUTCTime.AddHours(OffsetHour).AddMinutes(OffsetMin);
            if (this.chbDayLight.Checked == true)
            {
                ReadOnlyCollection<TimeZoneInfo> timeZones = TimeZoneInfo.GetSystemTimeZones();
                // Get each Time zone 
                foreach (TimeZoneInfo timeZone in timeZones)
                {
                    if (timeZone.DisplayName == this.cbbTimeZone.Text)
                    {

                        TimeZoneInfo.AdjustmentRule[] adjustments = timeZone.GetAdjustmentRules();

                        if (adjustments.Length == 0)
                        {
                            this.lbl_LoggerTime.Text = LoggerTime.ToString("HH:mm:ss");
                            this.lbl_LoggerDate.Text = LoggerTime.ToLongDateString();
                        }

                        foreach (TimeZoneInfo.AdjustmentRule daylight in adjustments)
                        {
                            if (timeZone.IsDaylightSavingTime(LoggerTime) == true)
                            {
                                theUTCTime1 = TimeZoneInfo.ConvertTimeToUtc(CurrentTime.AddHours(daylight.DaylightDelta.Hours).AddMinutes(daylight.DaylightDelta.Minutes).AddSeconds(daylight.DaylightDelta.Seconds), localZone);

                                try
                                {
                                    OffsetHour = Convert.ToDouble(this.cbbTimeZone.Text.Substring(4, 3));
                                    OffsetMin = Convert.ToDouble(this.cbbTimeZone.Text.Substring(8, 2));
                                }
                                catch (Exception)
                                {
                                    OffsetHour = 0;
                                    OffsetMin = 0;
                                }
                                LoggerTime = theUTCTime1.AddHours(OffsetHour).AddMinutes(OffsetMin);

                                this.lbl_LoggerDate.Text = LoggerTime.ToShortDateString();
                                this.lbl_LoggerTime.Text = LoggerTime.ToString("HH:mm:ss");
                            }
                            else
                            {
                                this.lbl_LoggerTime.Text = LoggerTime.ToString("HH:mm:ss");
                                this.lbl_LoggerDate.Text = LoggerTime.ToString("MM/dd/yyyy");
                            }
                        }
                    }
                }
            }
            else
            {
                this.lbl_LoggerTime.Text = LoggerTime.ToString("HH:mm:ss");
                this.lbl_LoggerDate.Text = LoggerTime.ToShortDateString();
            }
        }

        private void btnEraseAllData_Click(object sender, EventArgs e)
        {
            lblStatus.Visible = true;
            lblStatus.Text = res_man.GetString("Erasing...Please wait", cul);
            lblStatus.Update();

            string StrDevs = device35.hostport;
            if (device35.USBOpen(StrDevs) == false)
            {
                lblStatus.Visible = false;
                MessageBox.Show(res_man.GetString("Open") + " USB " + res_man.GetString("fail", cul));
                lockButton(false);
                device35.Close();
                return;
            }

            if (!device35.eraseSetting())
            {
                //lblStatus.Text = "Erase fail!";
                lblStatus.Text = res_man.GetString("Erase fail. Logger is recording", cul);
                //rtxtEventLog.Text += res_man.GetString("Erase All Data", cul) +" "+ res_man.GetString("fail", cul) + Environment.NewLine;
                rtxtEventLog.Text += res_man.GetString("Erase fail. Logger is recording", cul) + Environment.NewLine;
                device35.Close();
                return;
            }
            else
            {
                lblStatus.Text = "...";
                rtxtEventLog.Text += res_man.GetString("Erase All Data", cul) + " " + res_man.GetString("Successfully") + Environment.NewLine;
                device35.Close();
                btnReadSetting_Click(sender, e);
                cbbChannel_SelectedIndexChanged(sender, e);
                cbbUnit_SelectedIndexChanged(sender, e);
                return;
            }
        }

        private void btnWriteSetting_Click(object sender, EventArgs e)
        {
            if (cbbStartDelay.Text.Length == 0)
            {
                MessageBox.Show(res_man.GetString("Please set delay time to start recording", cul));
                return;
            }

            //if (cbbDuration.Text.Length == 0)
            //{
            //    MessageBox.Show(res_man.GetString("Please set record time", cul));
            //    return;
            //}

            cbbChannel_SelectedIndexChanged(null, null);
            int dldura = Convert.ToInt32(nbrDurationHour.Value) + Convert.ToInt32(nbrDurationDay.Value) * 24;

            string StrDevs = device35.hostport;

            if (string.IsNullOrEmpty(StrDevs))
            {
                MessageBox.Show(res_man.GetString("No Device", cul));
                return;
            }

            if (txtDescription.Text.Length > 40)
            {
                MessageBox.Show(res_man.GetString("Description can not be over 40 characters", cul));
                return;
            }
            else if (txtDescription.Text.Length == 0)
            {
                MessageBox.Show(res_man.GetString("No Description Input", cul));
                return;
            }

            if (txtLocation.Text.Length > 40)
            {
                MessageBox.Show(res_man.GetString("Location can not be over 40 characters", cul));
                return;
            }
            else if (txtLocation.Text.Length == 0)
            {
                MessageBox.Show(res_man.GetString("No Location Input", cul));
                return;
            }

            if (txtSerial.Text.Length == 0)
            {
                MessageBox.Show(res_man.GetString("Serial can't be empty", cul));
                return;
            }
            //try
            //{
            //    double a1 = Convert.ToDouble(txtMaxAlarm.Text);
            //}
            //catch (Exception)
            //{
            //    MessageBox.Show(res_man.GetString("Max value is wrong", cul));
            //    return;
            //}

            //try
            //{
            //    double a2 = Convert.ToDouble(txtMinAlarm.Text);
            //}
            //catch (Exception)
            //{
            //    MessageBox.Show("Min value is wrong");
            //    return;
            //}

            //if (Convert.ToDouble(txtMaxAlarm.Text) <= Convert.ToDouble(txtMinAlarm.Text) && chbNoAlarm.Checked == false)
            //{
            //    MessageBox.Show("Max value must higher than Min value");
            //    return;
            //}

            for (int i = 0; i < 4; i++)
            {
                if (!device35.Channels[i].NoAlarm)
                {
                    if (device35.Channels[i].AlarmMax <= device35.Channels[i].AlarmMin)
                    {
                        MessageBox.Show("Max alarm of channel " + (i + 1) + " must higher than min alarm of channel " + (i + 1));
                        return;
                    }
                }
            }

            char[] characters = txtDescription.Text.ToCharArray();
            for (int j = 0; j < characters.Length; j++)
            {
                if (Encoding.UTF8.GetByteCount(characters[j].ToString()) == characters[j].ToString().Length)
                {
                    continue;
                }
                else
                {
                    MessageBox.Show("Description must be typed in English");
                    //Exit For
                    return;
                }
            }

            lblStatus.Visible = true;
            lblStatus.Text = res_man.GetString("Writing....Please wait", cul);
            lblStatus.Update();
            //lockButton(true);

            if (device35.USBOpen(StrDevs) == false)
            {
                lblStatus.Visible = false;
                MessageBox.Show(res_man.GetString("Open") + " USB " + res_man.GetString("fail", cul));
                lockButton(false);
                device35.Close();
                return;
            }

            if (device35.readSettingDevice() == false)
            {
                device35.Close();
            }

            if (device35.byteLogging == 0x44)
            {
                MessageBox.Show("Logger is recording. Cannot write setting");
                lblStatus.Text = ".....";
                device35.Close();
                return;
            }


            //---------------write Location, Description, Serial (Serial not be updated by hardware)
            device35.Location = txtLocation.Text;
            if (device35.writeLocation() == false)
            {
                lblStatus.Visible = false;
                MessageBox.Show(res_man.GetString("Write Setting fail", cul));
                rtxtEventLog.Text += res_man.GetString("Write Setting fail", cul) + Environment.NewLine;
                lockButton(false);
                device35.Close();
                return;
            }
            else
            {
                rtxtEventLog.Text += res_man.GetString("Write Location successfully") + Environment.NewLine;
            }

            //------write Description
            //Thread.Sleep(500);
            device35.Description = txtDescription.Text;
            Thread.Sleep(100);
            if (device35.writeDescription() == false)
            {
                lblStatus.Visible = false;
                MessageBox.Show(res_man.GetString("Write Setting fail", cul));
                rtxtEventLog.Text += res_man.GetString("Write Setting fail", cul) + Environment.NewLine;
                lockButton(false);
                device35.Close();
                return;
            }
            else
            {
                rtxtEventLog.Text += res_man.GetString("Write Description successfully", cul) + Environment.NewLine;
            }

            //-----write Serial
            //Thread.Sleep(100);
            //device35.Serial = txtSerial.Text;
            //if(!device35.writeSerial())
            //{
            //    lblStatus.Visible = false;
            //    MessageBox.Show(res_man.GetString("Write Setting fail", cul));
            //    lockButton(false);
            //    device35.Close();
            //    return;
            //}


            //-------------------write Setting (Time, TimeZone, Delay, Duration).
            //Thread.Sleep(500);
            string TimeZone = null;
            try
            {
                //TimeZone a = ()

                TimeZone = mGlobal.FindSystemTimeZoneFromDisplayName(cbbTimeZone.Text).Id.ToString();
                //get 15 first char and 3 last char
                //TimeZone = TimeZone.Substring(0, 15) + TimeZone.Substring(Math.Max(TimeZone.Length - 2, 1) - 1).Substring(TimeZone.Substring(Math.Max(TimeZone.Length - 2, 1) - 1).Length - 3, 3);
                if (TimeZone.Length > 22)
                {
                    TimeZone = TimeZone.Substring(0, 22);
                }
            }
            catch (Exception)
            {
                TimeZone = "local";
            }

            device35.Duration = dldura;
            device35.Startrec = int.Parse(cbbStartDelay.Text);
            Thread.Sleep(100);
            if (device35.writeSettingDevice(LoggerTime, TimeZone) == false)
            {
                lblStatus.Visible = false;
                MessageBox.Show(res_man.GetString("Write setting fail", cul));
                rtxtEventLog.Text += res_man.GetString("Write setting fail", cul) + Environment.NewLine;
                lockButton(false);
                device35.Close();
                return;
            }
            else
            {

                rtxtEventLog.Text += res_man.GetString("Write setting successfully", cul) + Environment.NewLine;
            }


            //-----------------write setting Channel (Unit, sensor, description, alarm)
            //Thread.Sleep(500);
            for (int i = 0; i < 4; i++)
            {
                if (device35.Channels[i].Sensor == 255)
                {
                    device35.Channels[i].Sensor = 0;
                }
            }

            Thread.Sleep(100);
            if (device35.writeSettingChannel() == false)
            {
                lblStatus.Visible = false;
                MessageBox.Show(res_man.GetString("Write setting Channel fail", cul));
                rtxtEventLog.Text += res_man.GetString("Write setting Channel fail", cul) + Environment.NewLine;
                lockButton(false);
                device35.Close();
                return;
            }
            else
            {
                rtxtEventLog.Text += res_man.GetString("Write setting Channel successfully", cul) + Environment.NewLine;
            }

            saveSetting();
            lblStatus.Text = res_man.GetString("Write done");

            device35.Close();
        }

        private void cbbChannel_SelectedIndexChanged(object sender, EventArgs e)
        {
            //-------------------------------------------------------------------
            //tmpCheckedManual = False
            txtChlDesc.Text = "";
            if (cbbChannel.SelectedItem == null)
            {
                for (int i = 1; i <= 4; i++)
                {
                    cbbChannel.Items.Add(i);
                }

                cbbChannel.SelectedItem = cbbChannel.Items[0];
            }
            byte tmpCH = byte.Parse(cbbChannel.SelectedItem.ToString());

            cbbUnit.Items.Clear();
            cbbUnit.Items.AddRange(new string[] { "Not Use", "Celsius", "Fahrenheit", "PPM", "%RH", "G" });

            g_channel = byte.Parse((int.Parse(cbbChannel.SelectedItem.ToString()) - 1).ToString());
            g_unit = device35.Channels[g_channel].Unit;
            g_sensor = device35.Channels[g_channel].Sensor;

            txtChlDesc.Text = device35.Channels[g_channel].Desc;

            //ChlDesc = txtChlDesc.Text;
            //selectedDevice.Channels[g_channel].Desc = ChlDesc;

            //cbbUnit.Text = mGlobal.IntToUnit(g_unit);
            cbbUnit.Text = mGlobal.toUnit35(g_unit, g_sensor);
            if (cbbUnit.Text == "PPM")
            {
                lb_unitmax.Text = "ppm";
                lb_unitmin.Text = "ppm";
            }
            else
            {
                lb_unitmax.Text = mGlobal.toUnit35(g_unit, g_sensor);
                lb_unitmin.Text = mGlobal.toUnit35(g_unit, g_sensor);
            }



            if (device35.Channels[g_channel].NoAlarm == true)
            {
                chbNoAlarm.Checked = true;
                txtMaxAlarm.Text = "";
                txtMinAlarm.Text = "";
            }
            else
            {
                chbNoAlarm.Checked = false;
                int nguyen1 = device35.Channels[g_channel].AlarmMax / 10;
                int thphan1 = Math.Abs(device35.Channels[g_channel].AlarmMax % 10);
                txtMaxAlarm.Text = nguyen1.ToString() + "," + thphan1.ToString();
                //txtMaxAlarm.Text = device35.Channels[g_channel].AlarmMax.ToString();

                int nguyen2 = device35.Channels[g_channel].AlarmMin / 10;
                int thphan2 = Math.Abs(device35.Channels[g_channel].AlarmMin % 10);
                txtMinAlarm.Text = nguyen2.ToString() + "," + thphan2.ToString();
                //txtMinAlarm.Text = device35.Channels[g_channel].AlarmMin.ToString();
            }
        }

        private void txtMaxAlarm_TextChanged(object sender, EventArgs e)
        {
            if (txtMaxAlarm.Text == "0" || txtMaxAlarm.Text == "")
            {
                device35.Channels[g_channel].AlarmMax = 0;
            }
            else
            {
                //double a = double.Parse(txtMaxAlarm.Text);
                //device35.Channels[g_channel].AlarmMax = (int)(a * 10.0);
                //device35.Channels[g_channel].AlarmMax = (int)(float.Parse(txtMaxAlarm.Text));
                try
                {
                    int ph = 0;
                    if (txtMaxAlarm.Text.IndexOf(".") > 0)
                    {
                        ph = txtMaxAlarm.Text.IndexOf(".");
                    }
                    else if (txtMaxAlarm.Text.IndexOf(",") > 0)
                    {
                        ph = txtMaxAlarm.Text.IndexOf(",");
                    }
                    else
                    {
                        ph = txtMaxAlarm.Text.Length;
                    }

                    float nguyen = 0;
                    float thPhan = 0;
                    if (ph == 0)
                    {
                        nguyen = 0;
                    }
                    else
                    {
                        nguyen = float.Parse(txtMaxAlarm.Text.Substring(0, ph));
                    }
                    if (ph < txtMaxAlarm.Text.Length)
                    {
                        thPhan = Int32.Parse(txtMaxAlarm.Text.Substring(ph + 1, 1));
                    }
                    else
                    {
                        thPhan = 0;
                    }

                    float tmp = 0;
                    if (nguyen > 0)
                    {
                        tmp = nguyen * 10 + thPhan;
                    }
                    else
                    {
                        tmp = nguyen * 10 - thPhan;
                    }


                    //if(tmp < 0)
                    //{
                    //    tmp = 65536 + tmp;
                    //}

                    device35.Channels[g_channel].AlarmMax = (int)tmp;
                    //device35.Channels[g_channel].AlarmMax = nguyen * 10 + thPhan;//chuyen ve double rui nhan 10 la dc rui. khung ghe noi ak.

                    //device35.Channels[g_channel].AlarmMax = int.Parse(txtMaxAlarm.Text);
                }
                catch (Exception) { }
            }
        }

        private void txtMinAlarm_TextChanged(object sender, EventArgs e)
        {
            if (txtMinAlarm.Text == "0" || txtMinAlarm.Text == "")
            {
                device35.Channels[g_channel].AlarmMin = 0;
            }
            else
            {
                //device35.Channels[g_channel].AlarmMax = (int)(float.Parse(txtMinAlarm.Text));
                //double a = double.Parse(txtMinAlarm.Text);
                //device35.Channels[g_channel].AlarmMin = (int)(a * 10.0);
                try
                {
                    int ph = 0;
                    if (txtMinAlarm.Text.IndexOf(".") > 0)
                    {
                        ph = txtMinAlarm.Text.IndexOf(".");
                    }
                    else if (txtMaxAlarm.Text.IndexOf(",") > 0)
                    {
                        ph = txtMaxAlarm.Text.IndexOf(",");
                    }
                    else
                    {
                        ph = txtMinAlarm.Text.Length;
                    }

                    int nguyen = 0;
                    int thPhan = 0;
                    if (ph == 0)
                    {
                        nguyen = 0;
                    }
                    else
                    {
                        nguyen = Int32.Parse(txtMinAlarm.Text.Substring(0, ph));
                    }
                    if (ph < txtMinAlarm.Text.Length)
                    {
                        thPhan = Int32.Parse(txtMinAlarm.Text.Substring(ph + 1, 1));
                    }
                    else
                    {
                        thPhan = 0;
                    }


                    float tmp = 0;
                    if (nguyen > 0)
                    {
                        tmp = nguyen * 10 + thPhan;
                    }
                    else
                    {
                        tmp = nguyen * 10 - thPhan;
                    }


                    //if (tmp < 0)
                    //{
                    //    tmp = 65536 + tmp;
                    //}

                    device35.Channels[g_channel].AlarmMin = (int)tmp;

                    //device35.Channels[g_channel].AlarmMin = int.Parse(txtMinAlarm.Text);
                }
                catch (Exception) { }
            }
        }

        private void chbNoAlarm_CheckedChanged(object sender, EventArgs e)
        {
            if (chbNoAlarm.Checked == true)
            {
                lbl_max.Enabled = false;
                lbl_min.Enabled = false;
                lb_unitmin.Enabled = false;
                lb_unitmax.Enabled = false;
                txtMaxAlarm.Text = "";
                txtMinAlarm.Text = "";
                txtMaxAlarm.Enabled = false;
                txtMinAlarm.Enabled = false;
            }
            else
            {
                lbl_max.Enabled = true;
                lbl_min.Enabled = true;
                lb_unitmax.Enabled = true;
                lb_unitmin.Enabled = true;
                txtMaxAlarm.Enabled = true;
                txtMinAlarm.Enabled = true;
            }

            tmpChannel = Convert.ToByte(Int32.Parse(cbbChannel.SelectedItem.ToString()) - 1);
            if (chbNoAlarm.Checked == true)
            {
                device35.Channels[tmpChannel].NoAlarm = true;
            }
            else
            {
                device35.Channels[tmpChannel].NoAlarm = false;
            }
        }

        private void txtMaxAlarm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar) || Char.IsControl(e.KeyChar)) { }

            else if (e.KeyChar == '.' && txtMaxAlarm.Text.IndexOf('.'.ToString()) == -1) { }

            else if (e.KeyChar == '-' && txtMaxAlarm.Text == "") { }

            else
            {
                e.Handled = true;
            }

            if (txtMaxAlarm.Text.IndexOf('.') != -1 && txtMaxAlarm.Text.IndexOf('.') == txtMaxAlarm.Text.Length - 2)
            {
                if (!Char.IsControl(e.KeyChar))
                    e.Handled = true;
            }
        }

        private void txtMinAlarm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar) || Char.IsControl(e.KeyChar)) { }

            else if (e.KeyChar == '.' && txtMinAlarm.Text.IndexOf('.'.ToString()) == -1) { }

            else if (e.KeyChar == '-' && txtMinAlarm.Text == "") { }

            else
            {
                e.Handled = true;
            }

            if (txtMinAlarm.Text.IndexOf('.') != -1 && txtMinAlarm.Text.IndexOf('.') == txtMinAlarm.Text.Length - 2)
            {
                if (!Char.IsControl(e.KeyChar))
                    e.Handled = true;
            }
        }

        private void cbbDuration_SelectedIndexChanged(object sender, EventArgs e)
        {

            mGlobal.numChan = 0;
            for (int i = 0; i < 4; i++)
            {
                if (device35.Channels[i].Sensor == 0)
                {
                    mGlobal.numChan += 0;
                }
                else if (device35.Channels[i].Sensor == 3)
                {
                    mGlobal.numChan += 3;
                }
                else
                {
                    mGlobal.numChan += 1;
                }
            }
            int sec = 0;
            int min = 0;
            int tg = 0;
            int x = 0;
            if (mGlobal.numChan != 0)
            {
                x = (127 * 1024) / mGlobal.numChan;
            }
            else
            {
                x = 127 * 1024; //127*1024 so data toi da moi kenh
            }
            if (nbrDurationDay.Text.Length == 0|| nbrDurationHour.Text.Length==0)
            {
                MessageBox.Show(res_man.GetString("Please set record time", cul));
                return;
            }
            tg = mGlobal.DurationToInterval((int)nbrDurationDay.Value * 24 + (int)nbrDurationHour.Value, x);
            //tg = mGlobal.Duration(Convert.ToInt32(cbbDuration.Text));
            int sec1 = 0;
            int min2 = 0;
            int hour = 0;
            hour = tg / 3600;
            min2 = tg / 60;
            sec1 = tg % 60;
            nbrIntervalHour.Value = hour;
            nbrIntervalMin.Value = min2;
            nbrIntervalSec.Value = sec1;
        }

        private void txtLocation_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8)
            {
                if (txtLocation.Text.Length >= 40)
                {
                    //MessageBox.Show("Location can not be over 40 characters.");
                    e.Handled = true;
                    ToolTip t = new ToolTip();
                    t.SetToolTip(txtLocation, res_man.GetString("Location can not be over 40 characters", cul));
                    //t.SetToolTip(txtLocation, "Location cannot be over 40 characters");
                }
                else if (((e.KeyChar == (char)60) || (e.KeyChar == (char)62)) || (e.KeyChar == (char)63) || (e.KeyChar == (char)47) || (e.KeyChar == (char)58) || (e.KeyChar == (char)124) || (e.KeyChar == (char)92) || (e.KeyChar == (char)42))
                {
                    //MessageBox.Show(e.KeyChar + "is not available");
                    e.Handled = true;
                    ToolTip t = new ToolTip();
                    //t.SetToolTip(txtLocation, e.KeyChar +" "+ res_man.GetString("is not available", cul));
                    t.SetToolTip(txtLocation, res_man.GetString("Location can't contain any of the following characters", cul) + Environment.NewLine + "\\ / < > ? : \" | *");
                }
                else
                {
                    e.Handled = false;
                }
            }
        }

        private void txtDescription_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8)
            {
                if (txtDescription.Text.Length >= 40)
                {
                    //MessageBox.Show("Description cannot be over 40 characters");
                    e.Handled = true;
                    ToolTip t = new ToolTip();
                    t.SetToolTip(txtDescription, res_man.GetString("Description can not be over 40 characters", cul));
                }
                else if (((e.KeyChar == (char)60) || (e.KeyChar == (char)62)) || (e.KeyChar == (char)63) || (e.KeyChar == (char)47) || (e.KeyChar == (char)58) || (e.KeyChar == (char)124) || (e.KeyChar == (char)92) || (e.KeyChar == (char)42))
                {
                    // MessageBox.Show("Cannot typed " + e.KeyChar + "character");
                    e.Handled = true;
                    ToolTip t = new ToolTip();
                    //t.SetToolTip(txtDescription, "Cannot enter " + e.KeyChar + "character");
                    t.SetToolTip(txtDescription, res_man.GetString("Description can't contain any of the following characters", cul) + Environment.NewLine + "\\ / < > ? : \" | *");
                }
                else
                {
                    e.Handled = false;
                }
            }
        }

        private void cbbUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            //g_unit = mGlobal.UnitToInt(cbbUnit.SelectedItem.ToString());
            //device35.Channels[g_channel].Unit = g_unit;
            switch (cbbUnit.Text)
            {
                case "Not Use":
                    {
                        device35.Channels[g_channel].Sensor = 0;
                        device35.Channels[g_channel].Unit = 0x00;
                        cbbDuration_SelectedIndexChanged(sender, e);
                        lb_unitmax.Text = "Not use";
                        lb_unitmin.Text = "Not use";
                        enableVib(false);
                        txtMaxAlarm.Text = 0.ToString();
                        txtMaxAlarm.Enabled = false;
                        //txtMaxAlarm.Visible = false;
                        txtMinAlarm.Text = 0.ToString();
                        txtMinAlarm.Enabled = false;
                        //txtMinAlarm.Visible = false;
                        chbNoAlarm.Checked = true;
                        chbNoAlarm.Enabled = false;
                        groupBox6.Enabled = false;
                        cbbBR.Text = "";
                        cbbFS.Text = "";
                        break;
                    }
                case "Celsius":
                    {
                        device35.Channels[g_channel].Sensor = 1;
                        cbbDuration_SelectedIndexChanged(sender, e);
                        device35.Channels[g_channel].Unit = 0xAC;
                        device35.Channels[g_channel].DivNum = 10;
                        groupBox6.Enabled = true;
                        lb_unitmax.Text = "Celsius";
                        lb_unitmin.Text = "Celsius";
                        enableVib(false);
                        chbNoAlarm.Enabled = true;
                        break;
                    }
                case "Fahrenheit":
                    {
                        device35.Channels[g_channel].Sensor = 1;
                        cbbDuration_SelectedIndexChanged(sender, e);
                        device35.Channels[g_channel].Unit = 0xAF;
                        device35.Channels[g_channel].DivNum = 10;
                        groupBox6.Enabled = true;
                        lb_unitmax.Text = "Fahrenheit";
                        lb_unitmin.Text = "Fahrenheit";
                        enableVib(false);
                        chbNoAlarm.Enabled = true;
                        break;
                    }
                case "%RH":
                    {
                        device35.Channels[g_channel].Sensor = 2;
                        cbbDuration_SelectedIndexChanged(sender, e);
                        device35.Channels[g_channel].Unit = 0x0;
                        device35.Channels[g_channel].DivNum = 10;
                        groupBox6.Enabled = true;
                        lb_unitmax.Text = "%RH";
                        lb_unitmin.Text = "%RH";
                        enableVib(false);
                        chbNoAlarm.Enabled = true;
                        break;
                    }
                case "G":
                    {
                        device35.Channels[g_channel].Sensor = 3;
                        //if (device35.Channels[g_channel].Sensor == 3)
                        //{
                        if (device35.Channels[g_channel].Unit == 0xAC || device35.Channels[g_channel].Unit == 0xAF || device35.Channels[g_channel].Unit == 0xff)
                        {
                            device35.Channels[g_channel].Unit = 0x00;
                        }
                        string hex = String.Format("0x{0:X}", device35.Channels[g_channel].Unit);
                        //string hex = device35.Channels[g_channel].Unit.ToString("X");
                        int indexsFS = 0;
                        int indexsBR = 0;
                        if (hex.Length == 4)
                        {
                            indexsFS = int.Parse(hex.Substring(2, 1));
                            indexsBR = int.Parse(hex.Substring(3, 1));
                        }
                        else if (hex.Length == 3)
                        {
                            indexsFS = 0;
                            indexsBR = int.Parse(hex.Substring(2, 1));
                        }
                        cbbFS.Text = cbbFS.Items[indexsFS].ToString();
                        cbbBR.Text = cbbBR.Items[indexsBR].ToString();
                        //}
                        device35.Channels[g_channel].DivNum = 1;
                        cbbDuration_SelectedIndexChanged(sender, e);
                        lb_unitmax.Text = "G";
                        lb_unitmin.Text = "G";
                        enableVib(true);
                        if (cbbFS.Text == "" && cbbBR.Text == "")
                        {
                            cbbFS.Text = cbbFS.Items[0].ToString();
                            cbbBR.Text = cbbBR.Items[0].ToString();
                        }

                        //do rung ko set alarm
                        txtMaxAlarm.Text = 0.ToString();
                        txtMaxAlarm.Enabled = false;
                        //txtMaxAlarm.Visible = false;
                        txtMinAlarm.Text = 0.ToString();
                        txtMinAlarm.Enabled = false;
                        groupBox6.Enabled = false;
                        //txtMinAlarm.Visible = false;
                        chbNoAlarm.Checked = true;
                        chbNoAlarm.Enabled = false;

                        break;
                    }
                case "PPM":
                    {
                        device35.Channels[g_channel].DivNum = 1;
                        device35.Channels[g_channel].Sensor = 4;
                        device35.Channels[g_channel].Unit = 0;
                        cbbDuration_SelectedIndexChanged(sender, e);
                        lb_unitmax.Text = "ppm";
                        lb_unitmin.Text = "ppm";
                        enableVib(false);

                        //Co2 ko set alarm.
                        txtMaxAlarm.Text = 0.ToString();
                        txtMaxAlarm.Enabled = false;
                        //txtMaxAlarm.Visible = false;

                        txtMinAlarm.Text = 0.ToString();
                        txtMinAlarm.Enabled = false;
                        groupBox6.Enabled = false;
                        //txtMinAlarm.Visible = false;

                        chbNoAlarm.Checked = true;
                        chbNoAlarm.Enabled = false;

                        break;
                    }
                default:
                    {
                        break;
                    }
            }
            //txtMaxAlarm.Text = device35.Channels[g_channel].AlarmMax.ToString();
        }

        public void enableVib(bool status)
        {
            lblSR.Enabled = status;
            lblFS.Enabled = status;
            cbbBR.Enabled = status;
            cbbFS.Enabled = status;
            lblG.Enabled = status;
            lblHZ.Enabled = status;
        }

        public Device35 readLogger()
        {

            string StrDevs = device35.hostport;
            if (device35.USBOpen(StrDevs) == false)
            {
                lblStatus.Visible = false;
                MessageBox.Show(res_man.GetString("Open") + " USB " + res_man.GetString("fail", cul));
                lockButton(false);
                device35.Close();
                return null;
            }

            if (!device35.readSettingChannel())
            {
                lblStatus.Visible = false;
                MessageBox.Show(res_man.GetString("Read Setting", cul) + " " + res_man.GetString("fail", cul));
                lockButton(false);
                device35.Close();
                return null;
            }

            device35.Close();

            //lockButton(true);
            if (device35.USBOpen(StrDevs) == false)
            {
                lblStatus.Visible = false;
                MessageBox.Show(res_man.GetString("Open USB fail", cul));
                lockButton(false);
                device35.Close();
                return null;
            }

            ////================================
            //Check Logging Status
            if (device35.readSettingDevice() == false)//De lay trang thai logging
            {
                lblStatus.Visible = false;
                MessageBox.Show(res_man.GetString("Read Setting", cul) + " " + res_man.GetString("fail", cul));
                lockButton(false);
                device35.Close();
                return null;
            }

            device35._logger_date = device35.StartTime;
            //device35.Close();
            Logging = device35.byteLogging;//chua biet nhan ve cai gi!!!!(07/04/2015)

            sttCF = true;
            //set Unit default
            mGlobal.C2F = true;
            //device35.USBOpen(StrDevs);

            device35.Close();
            device35.USBOpen(StrDevs);

            if (device35.readLocation() == false)
            {
                MessageBox.Show(res_man.GetString("Read Location", cul) + " " + res_man.GetString("fail", cul));
                mGlobal.ColorChanged = false;
                mGlobal.TitleChanged = false;
                mGlobal.CommentChanged = false;
                this.Close();
                return null;
            }

            device35.Close();
            device35.USBOpen(StrDevs);

            if (device35.readDescription() == false)
            {
                MessageBox.Show(res_man.GetString("Read description", cul) + " " + res_man.GetString("fail", cul));
                mGlobal.ColorChanged = false;
                mGlobal.TitleChanged = false;
                mGlobal.CommentChanged = false;
                this.Close();
                return null;
            }

            if (device35.readSerial() == false)
            {
                MessageBox.Show("Read Serial " + res_man.GetString("fail", cul));
                mGlobal.ColorChanged = false;
                mGlobal.TitleChanged = false;
                mGlobal.CommentChanged = false;
                this.Close();
                return null;
            }

            string str1 = device35.Description;//40 byte
            string str2 = device35.Serial;

            if (str1 == "")
            {
                MessageBox.Show("Data have been erased");
                device35.Close();
                return null;
            }


            int length_str1 = 0;
            //Lay description (20 byte ?????????) -> de luu ten file
            for (int i = 1; i <= 20; i++)
            {
                length_str1 = str1.Length;
                if (str1.Substring(str1.Length - 1) == " ")
                {
                    str1 = device35.Description.Substring(0, length_str1 - 1);
                }
            }

            SaveFileDialog SaveFileDialog1 = new SaveFileDialog();
            string FilePath = null;

            string path = "";
            path = mGlobal.app_patch(path);
            path += "\\Reference.bin";

            if (File.Exists(path))
            {
                FileStream fs = new FileStream(path, FileMode.OpenOrCreate);
                BinaryReader br = new BinaryReader(fs);
                defaultFolder = br.ReadString();
                fs.Close();
                br.Close();
            }

            if (defaultFolder == "")
            {
                try
                {
                    SaveFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                }
                catch (Exception)
                { }
            }
            else
            {
                SaveFileDialog1.InitialDirectory = defaultFolder;
            }

            SaveFileDialog1.Filter = "Marathon Data Logger File (*.D4S)|*.D4S";

            SaveFileDialog1.FilterIndex = 1;
            SaveFileDialog1.RestoreDirectory = true;

            string check_name = null;

            SaveFileDialog1.FileName = string.Format("{0:ddMMyy}_{1}_{2}_{3}.D{4}S", DateTime.Now, device35.Location, str2, str1, device35.numOfChannel);
            check_name = string.Format("{0:ddMMyy}_{1}_{2}_", DateTime.Now, device35.Location, str2);

            SaveFileDialog1.FileName = string.Format("{0:ddMMyy}_{1}_{2}_{3}", DateTime.Now, device35.Location, device35.Serial, device35.Description);
            check_name = string.Format("{0:ddMMyy}_{1}_{2}_", DateTime.Now, device35.Location, device35.Serial);

            DialogResult checkResult = 0;


            //device35.Close();

            device35.titlegraph = "Marathon Electronic Data Logger";

            //save TimeZone
            for (int i = 0; i <= 17; i++)
            {
                if (device35.Timezone != "")
                {
                    mGlobal.ibuf_read[i] = Encoding.ASCII.GetBytes(device35.Timezone.Substring(i, 1))[0];
                }
            }

            //save start Time
            mGlobal.ibuf_read[18] = byte.Parse(device35.StartTime.Year.ToString());
            mGlobal.ibuf_read[19] = byte.Parse(device35.StartTime.Month.ToString());
            mGlobal.ibuf_read[20] = byte.Parse(device35.StartTime.Day.ToString());
            mGlobal.ibuf_read[21] = byte.Parse(device35.StartTime.Hour.ToString());
            mGlobal.ibuf_read[22] = byte.Parse(device35.StartTime.Minute.ToString());
            mGlobal.ibuf_read[23] = byte.Parse(device35.StartTime.Second.ToString());

            //save stop Time
            mGlobal.ibuf_read[24] = byte.Parse(device35.StopTime.Year.ToString());
            mGlobal.ibuf_read[25] = byte.Parse(device35.StopTime.Month.ToString());
            mGlobal.ibuf_read[26] = byte.Parse(device35.StopTime.Day.ToString());
            mGlobal.ibuf_read[27] = byte.Parse(device35.StopTime.Hour.ToString());
            mGlobal.ibuf_read[28] = byte.Parse(device35.StopTime.Minute.ToString());
            mGlobal.ibuf_read[29] = byte.Parse(device35.StopTime.Second.ToString());

            //doc eeprom
            int runSec = (device35.RunTime.Day - 1) * 24 * 60 * 60 + device35.RunTime.Hour * 60 * 60 + device35.RunTime.Minute * 60 + device35.RunTime.Second;

            int offset = 70;
            int temp = 0;
            for (int i = 0; i < 4; i++)
            {
                if (device35.Channels[i].Sensor == 0)
                    temp += 0;
                else if (device35.Channels[i].Sensor == 3)
                    temp += 3;
                else
                    temp += 1;
            }
            int lenght = (127 * 1024) / temp;


            int interval = 0;
            if ((device35.Duration * 86400) % lenght != 0)
            {
                interval = (device35.Duration * 86400) / lenght + 1;
            }
            else
            {
                interval = (device35.Duration * 86400) / lenght;
            }
            int lenght1 = runSec / interval;

            //max in progress bar.
            int sumTime = 0;
            int sumProbe = 0;
            for (int i = 0; i < 4; i++)
            {
                if (device35.Channels[i].Sensor == 3)
                {
                    sumProbe += 3;
                }
                else if (device35.Channels[i].Sensor != 0 && device35.Channels[i].Sensor != 3)
                {
                    sumProbe += 1;
                }
            }

            int sec = (device35.RunTime.Day - 1) * 24 * 60 * 60 + device35.RunTime.Hour * 60 * 60 + device35.RunTime.Minute * 60 + device35.RunTime.Second;
            int x = 0;
            if (sumProbe != 0)
            {
                x = (127 * 1024) / sumProbe;
            }
            else
            {
                x = 127 * 1024;
            }
            int tg = mGlobal.duration35(device35.Duration, x);
            int data = sumProbe * sec / tg;

            if (data == 0)
            {
                MessageBox.Show("No Data");
                device35.Close();
                return null;
            }

            sumTime = data / 31;
            //if(data % 32 > 0)
            //{
            //    sumTime = sumTime + 1;
            //}

            mGlobal.numProgress = 0;
            Progress pro = new Progress(sumTime);
            pro.FormClosed += MyChanged;
            pro.Show();
            //pro.progressBar1.Maximum = sumTime;


            //device35.Close();
            //device35.USBOpen(StrDevs);
            for (int i = 0; i < 4; i++)
            {
                if (mGlobal.stop)
                {
                    device35.Close();
                    mGlobal.stop = false;
                    return null;
                }


                int lenghtCH = 0;
                if (device35.Channels[i].Sensor == 3)
                {
                    lenghtCH = lenght * 3;
                }
                else if (device35.Channels[i].Sensor == 0)
                {
                    lenghtCH = 0;
                }
                else
                {
                    lenghtCH = lenght;
                }
                int Time = lenghtCH / 31;
                int du = lenghtCH % 62;


                device35.Channels[i].Data = new float[lenghtCH];

                if (device35.Channels[i].Sensor != 0)
                {
                    device35.Close();
                    device35.USBOpen(StrDevs);

                    //HIDFunction.hid_SetNonBlocking(device35.dev, 1);

                    device35.readDataStart(i + 1);

                    //Thread.Sleep(3);
                    device35.Close();
                    //try
                    //{
                    device35.USBOpen(StrDevs);
                    //}
                    //finally
                    //{


                    //}

                    //if (!device35.readDataEeprom1(Time, i + 1, device35.Channels[i].Sensor, pro, StrDevs))
                    //{
                    //    device35.Close();
                    //    return null;
                    //}

                    if (!device35.readDataEeprom2(lenght1, i + 1, device35.Channels[i].Sensor, pro, StrDevs))
                    {
                        MessageBox.Show("Cannot connect to logger. Please check USB port and read again");
                        pro.Close();
                        mGlobal.stop = false;
                        device35.Close();
                        return null;
                    }
                    //device35.readDataStop(Time, du, i + 1);

                }

            }

            for (int i = 0; i < 4; i++)
            {
                int lenghtCH = 0;
                if (device35.Channels[i].Sensor == 3)
                {
                    lenghtCH = lenght * 3;
                }
                else if (device35.Channels[i].Sensor == 0)
                {
                    lenghtCH = 0;
                }
                else
                {
                    lenghtCH = lenght;
                }

                //save sensor and Unit
                mGlobal.ibuf_read[30 + i] = device35.Channels[i].Sensor;
                if (device35.Channels[i].Unit == 255)
                    device35.Channels[i].Unit = 0;
                mGlobal.ibuf_read[34 + i] = device35.Channels[i].Unit;

                //Lay divnum
                //if(device35.Channels[i].Sensor == 3 || device35.Channels[i].Sensor == 4)
                //{
                //    device35.Channels[i].DivNum = 1;
                //}
                //else
                //{
                //    device35.Channels[i].DivNum = 10;
                //}

                int dem = 0;
                //if (i == 0)
                //{
                for (int j = 0; j < lenghtCH; j++)
                {
                    //if (device35.Channels[i].Data[j] == 65535)
                    //{
                    //    MessageBox.Show(j.ToString());
                    //    MessageBox.Show(device35.Channels[i].Data[j - 1].ToString());
                    //}
                    if (device35.Channels[i].Sensor != 3)
                    {
                        if (device35.Channels[i].Data[j] == 25966 && device35.Channels[i].Data[j + 1] == 25697 && device35.Channels[i].Data[j + 2] == 29793)
                        {
                            break;
                        }
                        else
                        {
                            dem += 1;
                        }
                    }
                    else
                    {
                        //if (device35.Channels[i].Data[j] == 0 && device35.Channels[i].Data[j + 1] == 0 && device35.Channels[i].Data[j + 2] == 0 && device35.Channels[i].Data[j + 3] == 0 && device35.Channels[i].Data[j + 4] == 0 && device35.Channels[i].Data[j + 5] == 0)
                        if (device35.Channels[i].Data[j] == 25966 && device35.Channels[i].Data[j + 1] == 25697 && device35.Channels[i].Data[j + 2] == 29793)
                        {
                            break;
                        }
                        else
                        {
                            dem += 1;
                        }
                    }
                }
                //dem = dem;
                //}
                //if (device35.Channels[i].Sensor == 3)
                //{
                //    dem = (dem - 3);
                //}
                //else
                //    {
                //        dem = dem - 1;
                //    }

                if (dem < 0)
                {
                    //MessageBox.Show("Read data fail");
                    return null;
                }
                //else if(device35.Channels[i].Sensor == 3)
                //{
                //    dem = dem * 3;
                //}
                if (device35.Channels[i].Sensor != 0)
                {
                    mGlobal.ibuf_read[38 + 3 * i] = byte.Parse((((dem / 256) / 256) % 256).ToString());
                    mGlobal.ibuf_read[38 + 3 * i + 1] = byte.Parse(((dem / 256) % 256).ToString());
                    mGlobal.ibuf_read[38 + 3 * i + 2] = byte.Parse((dem % 256).ToString());
                    //mGlobal.ibuf_read[38 + 2 * i] = byte.Parse((dem / 256).ToString());
                    //mGlobal.ibuf_read[38 + 2 * i + 1] = byte.Parse((dem % 256).ToString());
                }

                if (device35.Channels[i].NoAlarm)
                {
                    //mGlobal.ibuf_read[46 + i] = 0xF0;
                    mGlobal.ibuf_read[50 + i] = 0xF0;
                }
                else
                {
                    //mGlobal.ibuf_read[46 + i] = 0x0;
                    //mGlobal.ibuf_read[50 + 2 * i] = byte.Parse((device35.Channels[i].AlarmMax / 256).ToString());
                    //mGlobal.ibuf_read[50 + 2 * i + 1] = byte.Parse((device35.Channels[i].AlarmMax % 256).ToString());

                    //mGlobal.ibuf_read[58 + 2 * i] = byte.Parse((device35.Channels[i].AlarmMin / 256).ToString());
                    //mGlobal.ibuf_read[58 + 2 * i + 1] = byte.Parse((device35.Channels[i].AlarmMin % 256).ToString());

                    mGlobal.ibuf_read[50 + i] = 0x0;
                    mGlobal.ibuf_read[54 + 2 * i] = byte.Parse((device35.Channels[i].AlarmMax / 256).ToString());
                    mGlobal.ibuf_read[54 + 2 * i + 1] = byte.Parse((device35.Channels[i].AlarmMax % 256).ToString());

                    mGlobal.ibuf_read[62 + 2 * i] = byte.Parse((device35.Channels[i].AlarmMin / 256).ToString());
                    mGlobal.ibuf_read[62 + 2 * i + 1] = byte.Parse((device35.Channels[i].AlarmMin % 256).ToString());
                }


                //save Data
                //    if (device35.Channels[i].Sensor != 0)
                //    {
                //        for (int j = 0; j < lenghtCH; j++)
                //        {
                //            mGlobal.ibuf_read[offset + j] = byte.Parse((device35.Channels[i].Data[j]).ToString());
                //        }
                //    }

                if (device35.Channels[i].Sensor != 0)
                {
                    for (int j = 0; j < 2 * dem; j += 2)
                    {
                        mGlobal.ibuf_read[offset + j] = byte.Parse(((int)device35.Channels[i].Data[j / 2] / 256).ToString());
                        mGlobal.ibuf_read[offset + j + 1] = byte.Parse(((int)device35.Channels[i].Data[j / 2] % 256).ToString());
                    }
                }
                offset = offset + 2 * dem;

                if (device35.Channels[i].Sensor == 3)
                {
                    dem = dem / 3;
                }
            }

            device35.Close();


            pro.Close();

            System.IO.FileStream oFileStream = null;

            string FileName = "";
            FileName = mGlobal.app_patch(FileName);

            oFileStream = new System.IO.FileStream(FileName + "\\Data.txt", System.IO.FileMode.Create);

            //for (int i = 0; i < mGlobal.ibuf_read.Length; i++)
            //{
            //    string tmp = mGlobal.ibuf_read[i].ToString();
            //    for (int j = 0; j < tmp.Length; j++)
            //    {
            //        byte a = byte.Parse(tmp.Substring(j, 1));
            //        oFileStream.WriteByte(a);
            //    }
            //    oFileStream.WriteByte(20);
            //}

            //for (int j = 65; j < 1000; j++)
            //{
            //    oFileStream.WriteByte(mGlobal.ibuf_read[j]);
            //    oFileStream.WriteByte(32);
            //}

            oFileStream.Write(mGlobal.ibuf_read, 0, mGlobal.ibuf_read.Length);//mglobal.ibuf_read dc write tu selectedDevice.ReadData_Start()_8S;
            oFileStream.Close();
            //device35.Close();

            //tao file Data.txt

            if (System.IO.File.Exists(FileName + "\\Data.txt") == true)
            {
                datalogger = System.IO.File.ReadAllBytes(FileName + "\\Data.txt");
            }
            else
            {
                MessageBox.Show(res_man.GetString("File not found", cul));
                mGlobal.ColorChanged = false;
                mGlobal.TitleChanged = false;
                mGlobal.CommentChanged = false;
                this.Close();
                return null;
            }

            saveGraph._timezone = mGlobal.FindSystemTimeZoneFromString(device35.Timezone);
            saveGraph.set_graph35_2(datalogger);
            // muc dich cua set_graph la dua du lieu vo Channel. roi sau do luu Channel xuong file.
            byte[] data_save = { 0 };


            if (SaveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                FilePath = SaveFileDialog1.FileName.ToString();

                if (FilePath.Substring(FilePath.Length - 4) != ".D4S")
                {
                    FilePath = FilePath + ".D4S";
                }

                mGlobal.PathFile = FilePath;

                device35.SaveFile_MP_Lgr2(ref data_save);

                try
                {
                    System.IO.File.WriteAllBytes(FilePath, data_save);
                    rtxtEventLog.Text += res_man.GetString("File is saved successfully", cul) + Environment.NewLine;
                    mGlobal.stop = false;
                }
                catch (Exception)
                {
                    rtxtEventLog.Text += res_man.GetString("Error to save file", cul) + Environment.NewLine;
                    return null;
                }

                //string setting = device35.SaveTextFile();

                //FileStream fs = null;

                //if (System.IO.File.Exists(FilePath))
                //{
                //    fs = new FileStream(FilePath, FileMode.Truncate, FileAccess.Write);
                //}
                //else
                //{
                //    fs = new FileStream(FilePath, FileMode.OpenOrCreate, FileAccess.Write);
                //}

                //using (StreamWriter sw = new StreamWriter(fs))
                //{
                //    sw.Write(setting);
                //}
            }
            else
            {
                MessageBox.Show(res_man.GetString("Saving file is cancelled. Cannot open chart", cul));
                device35.Close();
                mGlobal.activeDevice = true;
                mGlobal.stop = false;
                lockButton(false);
                return null;
            }
            //}

            ////=============Read Timezone======
            //device35.USBOpen(StrDevs);
            //if (device35.readSettingDevice() == false)
            //{
            //    lblStatus.Visible = false;
            //    MessageBox.Show("Read TimeZone fail");
            //    lockButton(false);
            //    device35.Close();
            //    return null;
            //}

            //string TimeZone = mGlobal.FindSystemTimeZoneFromString(device35.Timezone.ToString()).ToString();
            //for (int i = 0; i < cbbTimeZone.Items.Count; i++)
            //{
            //    if (cbbTimeZone.Items[i].ToString() == TimeZone)
            //    {
            //        cbbTimeZone.Text = cbbTimeZone.Items[i].ToString();
            //    }
            //}

            device35.OpenFile_MP_Lgr(FilePath.ToString());
            //device35.OpenTextFile(FilePath.ToString());
            return device35;
        }

        private void LoggerIni_35_FormClosed(object sender, FormClosedEventArgs e)
        {
            byte[] dataSetting = new byte[360];
            System.IO.FileStream oFileStream = null;

            string FileName = "";
            FileName = mGlobal.app_patch(FileName);

            oFileStream = new System.IO.FileStream(FileName + "\\dataSetting.txt", System.IO.FileMode.Create);

            device35.saveSetting(ref dataSetting);

            oFileStream.Write(dataSetting, 0, dataSetting.Length);
            oFileStream.Close();

            device35.Close();
        }


        private void saveSetting()
        {
            byte[] dataSetting = new byte[360];
            System.IO.FileStream oFileStream = null;

            string FileName = "";
            FileName = mGlobal.app_patch(FileName);

            oFileStream = new System.IO.FileStream(FileName + "\\dataSetting.txt", System.IO.FileMode.Create);

            device35.saveSetting(ref dataSetting);

            oFileStream.Write(dataSetting, 0, dataSetting.Length);
            oFileStream.Close();
        }

        private void txtChlDesc_Validating(object sender, CancelEventArgs e)
        {
            //if (txtChlDesc.Text.Trim().Length > 40)
            //{
            //    MessageBox.Show(res_man.GetString("Channel's description can not be over 40 characters", cul));
            //    e.Cancel = true;
            //    return;
            //}

            //char[] characters = txtChlDesc.Text.ToCharArray();
            //for (int i = 0; i < characters.Length; i++)
            //{
            //    if ((int)characters[i] != 0)
            //    {
            //        continue;
            //    }
            //    else
            //    {
            //        MessageBox.Show("Channel's description must be typed in English");
            //        //Exit For
            //        return;
            //    }
            //}
            device35.Channels[g_channel].Desc = txtChlDesc.Text.Trim();
        }

        private void cbbFS_SelectedIndexChanged(object sender, EventArgs e)
        {
            firstHex = Int16.Parse((cbbFS.SelectedIndex * 10).ToString());
            int decF = int.Parse(firstHex.ToString(), System.Globalization.NumberStyles.HexNumber);
            int decL = int.Parse(lastHex.ToString(), NumberStyles.HexNumber);
            int dec = decF + decL;
            device35.Channels[g_channel].Unit = (byte)dec;
        }

        private void cbbBR_SelectedIndexChanged(object sender, EventArgs e)
        {
            lastHex = Int16.Parse(cbbBR.SelectedIndex.ToString());
            int decF = int.Parse(firstHex.ToString(), System.Globalization.NumberStyles.HexNumber);
            int decL = int.Parse(lastHex.ToString(), NumberStyles.HexNumber);
            int dec = decF + decL;
            device35.Channels[g_channel].Unit = (byte)dec;
        }

        private void txtDescription_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                txtLocation.Focus();
            }
        }

        private void txtLocation_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                cbbTimeZone.Focus();
            }
        }

        private void btnEmail_Click(object sender, EventArgs e)
        {
            string StrDevs = device35.hostport;

            EmailSetting email = new EmailSetting(StrDevs);
            email.ShowDialog();
        }

        private void txtChlDesc_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8)
            {
                if (txtChlDesc.Text.Length >= 40)
                {
                    //MessageBox.Show("Description cannot be over 40 characters");
                    e.Handled = true;
                    ToolTip t = new ToolTip();
                    t.SetToolTip(txtChlDesc, "Description of this channel can not be over 40 characters");
                }
                else if (((e.KeyChar == (char)60) || (e.KeyChar == (char)62)) || (e.KeyChar == (char)63) || (e.KeyChar == (char)47) || (e.KeyChar == (char)58) || (e.KeyChar == (char)124) || (e.KeyChar == (char)92) || (e.KeyChar == (char)42))
                {
                    // MessageBox.Show("Cannot typed " + e.KeyChar + "character");
                    e.Handled = true;
                    ToolTip t = new ToolTip();
                    t.SetToolTip(txtChlDesc, res_man.GetString("Description can't contain any of the following characters", cul) + Environment.NewLine + "\\ / < > ? : \" | *");
                }
                else
                {
                    e.Handled = false;
                }
            }
        }

        private void txtSerial_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8)
            {
                if (txtSerial.Text.Length >= 10)
                {
                    //MessageBox.Show("Location can not be over 40 characters.");
                    e.Handled = true;
                    ToolTip t = new ToolTip();
                    t.SetToolTip(txtSerial, res_man.GetString("Serial can not be over 10 characters", cul));
                    //t.SetToolTip(txtLocation, "Location cannot be over 40 characters");
                }
                else if ((e.KeyChar == (char)60) || (e.KeyChar == (char)62) || (e.KeyChar == (char)63) || (e.KeyChar == (char)47) || (e.KeyChar == (char)58) || (e.KeyChar == (char)124) || (e.KeyChar == (char)92) || (e.KeyChar == (char)42))
                {
                    //MessageBox.Show(e.KeyChar + "is not available");
                    e.Handled = true;
                    ToolTip t = new ToolTip();
                    //t.SetToolTip(txtSerial, e.KeyChar + " " + res_man.GetString("is not available", cul));
                    t.SetToolTip(txtSerial, res_man.GetString("Serial can't contain any of the following characters", cul) + Environment.NewLine + "\\ / < > ? : \" | *");
                }
                else
                {
                    e.Handled = false;
                }
            }
        }


        //private void saveSetting(ref byte[] buf, Device35 dev35)
        //{
        //    dev35 = Device35.Instance;
        //    //save Serial
        //    for (int i = 0; i < 10; i++)
        //    {
        //        buf[i] = Encoding.ASCII.GetBytes(dev35.Serial.Substring(i, 1))[0];
        //    }

        //    //save Description
        //    for (int i = 0; i < 40; i++)
        //    {
        //        buf[10 + i] = Encoding.ASCII.GetBytes(dev35.Description.Substring(0, 1))[0];
        //    }

        //    //save Location
        //    for (int i = 0; i < ; i++)
        //    {

        //    }
        //}


        public void test()
        {
            Progress pro = new Progress(100);
            pro.Show();
            for (int i = 0; i < 100; i++)
            {

                Thread.Sleep(100);
                mGlobal.numProgress += 1;

                //pro.progressBar1.Value = mGlobal.numProgress;
                //pro.progressBar1.Refresh();
                //pro.lblProgress.Refresh();
                //pro.progressBar1.Update
                pro.Refresh();
                //pro.progressBar1.Refresh();
            }
        }

        void MyChanged(object source, EventArgs e)
        {
            mGlobal.stop = true;
        }


    }
}
