using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Pexo16
{
    public partial class Setting35 : Form
    {
        public Device35 device35 = null;
        Graph saveGraph = null;

        private byte g_channel;
        private byte Logging;
        private DateTime LoggerTime;
        private byte g_unit;
        private byte g_sensor;
        Int16 firstHex;
        Int16 lastHex;
        public string defaultFolder = "";
        public byte[] datalogger;
        public DateTime _logger_date;
        long[] len = new long[9];
        private byte tmpChannel;


        public Setting35()
        {
            InitializeComponent();

            device35 = Device35.DelInstance();
            device35 = Device35.Instance;

            device35.Channels = new Channel[4];
            for (int i = 0; i < 4; i++)
            {
                device35.Channels[i] = new Channel();
            }

            saveGraph = Graph.DelInstance();
            saveGraph = Graph.Instance;

            mGlobal.unitFromFile = false;
        }

        private void Setting35_Load(object sender, EventArgs e)
        {
            SystemEvents.TimeChanged += new EventHandler(SystemEvent_TimeChanged);

            lbl_LoggerDate.Text = DateTime.Now.ToShortDateString();
            lbl_LoggerTime.Text = DateTime.Now.ToShortTimeString();

            cbbTimeZone.Text = TimeZoneInfo.Local.ToString();

            cbbTimeZone.Items.Clear();

            ReadOnlyCollection<TimeZoneInfo> timeZones = TimeZoneInfo.GetSystemTimeZones();
            foreach (TimeZoneInfo timeZoneInfo in timeZones)
            {
                cbbTimeZone.Items.Add(timeZoneInfo.DisplayName);
            }

            TimeZone zone = TimeZone.CurrentTimeZone;
            DateTime local = zone.ToLocalTime(DateTime.Now);
            Tim_UpdateClock.Enabled = true;

            byte[] bufSetting = new byte[360];

            string FileName = "";
            FileName = mGlobal.app_patch(FileName);


            if (System.IO.File.Exists(FileName + "\\dataSetting.txt") == true)
            {
                bufSetting = System.IO.File.ReadAllBytes(FileName + "\\dataSetting.txt");
                device35.openSetting(bufSetting);
                txtSerial.Text = device35.Serial;
                txtDescription.Text = device35.Description;
                txtLocation.Text = device35.Location;
                if (device35.Duration != 65535)
                {
                    int sec1 = 0;
                    int min2 = 0;

                    if (Convert.ToInt32(device35.Duration) > 60)
                    {
                        min2 = Convert.ToInt32(device35.Duration) / 60;
                        sec1 = Convert.ToInt32(device35.Duration) % 60;
                        lb_interval.Text = string.Format("{0}: {1} min {2} sec.", "Sample interval", min2, sec1);
                    }
                    else
                    {
                        lb_interval.Text = string.Format("{0}: {1} sec.", "Sample interval", Convert.ToInt32(device35.Duration));
                    }
                }
                else
                {
                    lb_interval.Text = 0.ToString();
                }

                if (device35.Delay == 255)
                {
                    cbbStartDelay.Text = cbbStartDelay.Items[0].ToString();
                }
                else
                {
                    cbbStartDelay.Text = device35.Delay.ToString();
                }

                if (device35.Duration == 65535)
                {
                    cbbDuration.Text = cbbDuration.Items[0].ToString();
                }
                else
                {
                    cbbDuration.Text = device35.Duration.ToString();
                }

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
                btnReadSetting_Click(sender, e);
            }

            cbbChannel.Items.Clear();
            for (int i = 1; i <= 4; i++)
            {
                cbbChannel.Items.Add(i);
            }

            cbbChannel.Text = cbbChannel.Items[0].ToString();
            cbbChannel.SelectedIndex = 0;

            if (device35.numOfChannel != 8)
            {
                cbbUnit.Items.Clear();
                cbbUnit.Items.AddRange(new string[] { "Not Use", "Celsius", "Fahrenheit", "CO2", "%RH", "G" });
            }
            cbbChannel_SelectedIndexChanged(sender, e);
            cbbUnit_SelectedIndexChanged(sender, e);

            cbbDuration.DropDownStyle = ComboBoxStyle.DropDownList;
            cbbStartDelay.DropDownStyle = ComboBoxStyle.DropDownList;
            cbbUnit.DropDownStyle = ComboBoxStyle.DropDownList;

            chkRecord.Checked = false;
            chkDes.Checked = false;
            chkLoc.Checked = false;
            chkChannel.Checked = false;

            mGlobal.len = 64;
        }

        private void SystemEvent_TimeChanged(object sender, EventArgs e)
        {
            CultureInfo.CurrentCulture.ClearCachedData();

            cbbTimeZone.Text = TimeZoneInfo.Local.ToString();

            lbl_LoggerDate.Text = DateTime.Now.ToShortDateString();
            lbl_LoggerTime.Text = DateTime.Now.ToShortTimeString();
        }

        private void cbbChannel_SelectedIndexChanged(object sender, EventArgs e)
        {
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
            cbbUnit.Items.AddRange(new string[] { "Not Use", "Celsius", "Fahrenheit", "CO2", "%RH", "G" });

            g_channel = byte.Parse((int.Parse(cbbChannel.SelectedItem.ToString()) - 1).ToString());
            g_unit = device35.Channels[g_channel].Unit;
            g_sensor = device35.Channels[g_channel].Sensor;

            txtChlDesc.Text = device35.Channels[g_channel].Desc;

            cbbUnit.Text = mGlobal.toUnit35(g_unit, g_sensor);
            if (cbbUnit.Text == "CO2")
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
                int thphan1 = device35.Channels[g_channel].AlarmMax % 10;
                txtMaxAlarm.Text = nguyen1.ToString() + "," + thphan1.ToString();

                int nguyen2 = device35.Channels[g_channel].AlarmMin / 10;
                int thphan2 = device35.Channels[g_channel].AlarmMin % 10;
                txtMinAlarm.Text = nguyen2.ToString() + "," + thphan2.ToString();
            }
        }

        private void cbbUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
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
                        txtMinAlarm.Text = 0.ToString();
                        txtMinAlarm.Enabled = false;
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
                        if (device35.Channels[g_channel].Unit == 0xAC || device35.Channels[g_channel].Unit == 0xAF || device35.Channels[g_channel].Unit == 0xff)
                        {
                            device35.Channels[g_channel].Unit = 0x00;
                        }
                        string hex = String.Format("0x{0:X}", device35.Channels[g_channel].Unit);
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

                        txtMaxAlarm.Text = 0.ToString();
                        txtMaxAlarm.Enabled = false;
                        txtMinAlarm.Text = 0.ToString();
                        txtMinAlarm.Enabled = false;
                        groupBox6.Enabled = false;
                        chbNoAlarm.Checked = true;
                        chbNoAlarm.Enabled = false;

                        break;
                    }
                case "CO2":
                    {
                        device35.Channels[g_channel].DivNum = 1;
                        device35.Channels[g_channel].Sensor = 4;
                        device35.Channels[g_channel].Unit = 0;
                        cbbDuration_SelectedIndexChanged(sender, e);
                        lb_unitmax.Text = "ppm";
                        lb_unitmin.Text = "ppm";
                        enableVib(false);

                        txtMaxAlarm.Text = 0.ToString();
                        txtMaxAlarm.Enabled = false;

                        txtMinAlarm.Text = 0.ToString();
                        txtMinAlarm.Enabled = false;
                        groupBox6.Enabled = false;

                        chbNoAlarm.Checked = true;
                        chbNoAlarm.Enabled = false;

                        break;
                    }
                default:
                    {
                        break;
                    }
            }
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
                x = 127 * 1024;
            }
            if (cbbDuration.Text.Length == 0)
            {
                MessageBox.Show("Please set record time");
                return;
            }
            tg = mGlobal.duration35(Convert.ToInt32(cbbDuration.Text), x);
            if (tg > 60)
            {
                min = tg / 60;
                sec = tg % 60;
                lb_interval.Text = "Sample interval: " + min + " min " + sec + " sec.";
            }
            else
            {
                lb_interval.Text = "Sample interval: "  + tg + " sec.";
            }
            device35.Duration = int.Parse(cbbDuration.Text);    
        
        }

        private void btnReadSetting_Click(object sender, EventArgs e)
        {
            getDeviceInfo.getActiveDevice();
            if (getDeviceInfo.activeDeviceListAl.Count == 0)
            {
                MessageBox.Show("Troi oi! Chua cam logger kia thim 2!");
                return;
            }
            device35.hostport = getDeviceInfo.activeDeviceListAl[0].ToString();

            lblStatus.Text = "Reading.....Please wait";
            lblStatus.Update();

            string StrDevs = device35.hostport;
            //Open Device
            if (device35.USBOpen(StrDevs) == false)
            {
                MessageBox.Show("Open USB fail. Please try again.");
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
            rtxtEventLog.Clear();
            rtxtEventLog.Text += "Hardware: " + device35.HardVer + " Firmware: " + device35.FirmVer;
            device35.Close();


            //read Location
            device35.USBOpen(StrDevs);
            if (device35.readLocation() == false)
            {
                MessageBox.Show("Read Location fail");
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
                MessageBox.Show("Read description fail");
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
                MessageBox.Show("Read Setting fail");
                lockButton(false);
                device35.Close();
                return;
            }


            string TimeZone = mGlobal.FindSystemTimeZoneFromString(device35.Timezone.ToString()).ToString();
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

                if (Convert.ToInt32(device35.Duration) > 60)
                {
                    min2 = Convert.ToInt32(device35.Duration) / 60;
                    sec1 = Convert.ToInt32(device35.Duration) % 60;
                    lb_interval.Text = "Sample interval: " + min2 + " min " + sec1 + " sec.";
                }
                else
                {
                    lb_interval.Text = "Sample interval: " + Convert.ToInt32(device35.Duration) + " sec.";
                }
            }
            else
            {
                lb_interval.Text = 0.ToString();
            }

            if (device35.Delay == 255)
            {
                cbbStartDelay.Text = cbbStartDelay.Items[0].ToString();
            }
            else
            {
                cbbStartDelay.Text = device35.Delay.ToString();
            }

            if (device35.Duration == 65535)
            {
                cbbDuration.Text = cbbDuration.Items[0].ToString();
            }
            else
            {
                cbbDuration.Text = device35.Duration.ToString();
            }

            device35.Close();

            for (int i = 0; i < 4; i++)
            {
                device35.USBOpen(StrDevs);
                if (device35.readSettingChannel1(i) == false)
                {
                    MessageBox.Show("Read Setting fail");
                    lockButton(false);
                    device35.Close();
                    return;
                }
                device35.Close();
            }

            cbbChannel_SelectedIndexChanged(sender, e);

            lblStatus.Text = ".....";
        }

        private void lockButton(bool p)
        {
            if (p == true)
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

        private void btnWriteSetting_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "Writing";
            lblStatus.Refresh();
            getDeviceInfo.getActiveDevice();
            if (getDeviceInfo.activeDeviceListAl.Count == 0)
            {
                MessageBox.Show("Troi oi! Chua cam logger kia thim 2!");
                return;
            }
            device35.hostport = getDeviceInfo.activeDeviceListAl[0].ToString();

            if (cbbStartDelay.Text.Length == 0)
            {
                MessageBox.Show("Please set delay time to start recording");
                return;
            }

            if (cbbDuration.Text.Length == 0)
            {
                MessageBox.Show("Please set record time");
                return;
            }

            cbbChannel_SelectedIndexChanged(null, null);
            int dldura = Convert.ToInt32(cbbDuration.Text);
            string StrDevs = device35.hostport;

            if (string.IsNullOrEmpty(StrDevs))
            {
                MessageBox.Show("No Device");
                return;
            }

            if (txtDescription.Text.Length > 40 && chkDes.Checked == true)
            {
                MessageBox.Show("Description can not be over 40 characters");
                return;
            }
            else if (txtDescription.Text.Length == 0 && chkDes.Checked == true)
            {
                MessageBox.Show("No Description Input");
                return;
            }

            if (txtLocation.Text.Length > 40 && chkLoc.Checked)
            {
                MessageBox.Show("Location can not be over 40 characters");
                return;
            }
            else if (txtLocation.Text.Length == 0 && chkLoc.Checked)
            {
                MessageBox.Show("No Location Input");
                return;
            }

            if (txtSerial.Text.Length == 0)
            {
                MessageBox.Show("Serial can't be empty");
                return;
            }

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
                    return;
                }
            }

            lblStatus.Visible = true;
            lblStatus.Text = "Writing....Please wait";
            lblStatus.Update();
            //lockButton(true);

            if (device35.USBOpen(StrDevs) == false)
            {
                lblStatus.Visible = false;
                MessageBox.Show("Open USB fail");
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
            if (chkLoc.Checked)
            {
                device35.Location = txtLocation.Text;
                if (device35.writeLocation() == false)
                {
                    lblStatus.Visible = false;
                    MessageBox.Show("Write Setting fail");
                    rtxtEventLog.Text += "Write Setting fail" + Environment.NewLine;
                    lockButton(false);
                    device35.Close();
                    return;
                }
                else
                {
                    rtxtEventLog.Text += "Write Location successfully" + Environment.NewLine;
                }
            }

            //------write Description
            if (chkDes.Checked)
            {
                device35.Description = txtDescription.Text;
                Thread.Sleep(100);
                if (device35.writeDescription() == false)
                {
                    lblStatus.Visible = false;
                    MessageBox.Show("Write Setting fail");
                    rtxtEventLog.Text += "Write Setting fail" + Environment.NewLine;
                    lockButton(false);
                    device35.Close();
                    return;
                }
                else
                {
                    rtxtEventLog.Text += "Write Description successfully" + Environment.NewLine;
                }
            }

            //-----write Serial
            Thread.Sleep(100);
            device35.Serial = txtSerial.Text;
            if (!device35.writeSerial())
            {
                lblStatus.Visible = false;
                MessageBox.Show("Write Setting fail");
                lockButton(false);
                device35.Close();
                return;
            }


            //-------------------write Setting (Time, TimeZone, Delay, Duration).
            if (chkRecord.Checked)
            {
                //Thread.Sleep(500);
                string TimeZone = null;
                try
                {
                    TimeZone = mGlobal.FindSystemTimeZoneFromDisplayName(cbbTimeZone.Text).Id.ToString();
                    //get 15 first char and 3 last char
                    TimeZone = TimeZone.Substring(0, 15) + TimeZone.Substring(Math.Max(TimeZone.Length - 2, 1) - 1).Substring(TimeZone.Substring(Math.Max(TimeZone.Length - 2, 1) - 1).Length - 3, 3);
                }
                catch (Exception)
                {
                    TimeZone = "local";
                }

                device35.Duration = int.Parse(cbbDuration.Text);
                device35.Startrec = int.Parse(cbbStartDelay.Text);
                Thread.Sleep(100);
                if (device35.writeSettingDevice(LoggerTime, TimeZone) == false)
                {
                    lblStatus.Visible = false;
                    MessageBox.Show("Write setting fail");
                    rtxtEventLog.Text += "Write setting fail" + Environment.NewLine;
                    lockButton(false);
                    device35.Close();
                    return;
                }
                else
                {
                    rtxtEventLog.Text += "Write setting successfully" + Environment.NewLine;
                }
            }

            //-----------------write setting Channel (Unit, sensor, description, alarm)
            if (chkChannel.Checked)
            {
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
                    MessageBox.Show("Write setting Channel fail");
                    rtxtEventLog.Text += "Write setting Channel fail" + Environment.NewLine;
                    lockButton(false);
                    device35.Close();
                    return;
                }
                else
                {
                    rtxtEventLog.Text += "Write setting Channel successfully" + Environment.NewLine;
                }
            }

            saveSetting();
            lblStatus.Text = "....";
            rtxtEventLog.Text += "Writing to " + device35.Serial + "has done!" + Environment.NewLine;

            string tampSerial = (Int32.Parse(device35.Serial.Substring(6, device35.Serial.Length - 6)) + 1).ToString();
            string seri = "";
            for (int i = 0; i < (4 - tampSerial.Length); i++)
            {
                seri += "0";
            }
            seri = device35.Serial.Substring(0, 6) + seri + tampSerial;
            txtSerial.Text = seri;

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

        private void btnEraseAllData_Click(object sender, EventArgs e)
        {
            lblStatus.Visible = true;
            lblStatus.Text = "Erasing...Please wait";
            lblStatus.Update();

            string StrDevs = device35.hostport;
            if (device35.USBOpen(StrDevs) == false)
            {
                lblStatus.Visible = false;
                MessageBox.Show("Open USB fail");
                lockButton(false);
                device35.Close();
                return;
            }

            if (!device35.eraseSetting())
            {
                lblStatus.Text = "Erase fail. Logger is recording";
                rtxtEventLog.Text += "Erase fail. Logger is recording" + Environment.NewLine;
                device35.Close();
                return;
            }
            else
            {
                lblStatus.Text = "...";
                rtxtEventLog.Text += "Erase All Data Successfully" + Environment.NewLine;
                device35.Close();
                btnReadSetting_Click(sender, e);
                cbbChannel_SelectedIndexChanged(sender, e);
                cbbUnit_SelectedIndexChanged(sender, e);
                return;
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
                try
                {
                    int ph = 0;
                    if (txtMaxAlarm.Text.IndexOf(".") > 0)
                    {
                        ph = txtMaxAlarm.Text.IndexOf(".");
                    }
                    else
                    {
                        ph = txtMaxAlarm.Text.Length;
                    }

                    int nguyen = 0;
                    int thPhan = 0;
                    if (ph == 0)
                    {
                        nguyen = 0;
                    }
                    else
                    {
                        nguyen = Int32.Parse(txtMaxAlarm.Text.Substring(0, ph));
                    }
                    if (ph < txtMaxAlarm.Text.Length)
                    {
                        thPhan = Int32.Parse(txtMaxAlarm.Text.Substring(ph + 1, 1));
                    }
                    else
                    {
                        thPhan = 0;
                    }
                    device35.Channels[g_channel].AlarmMax = nguyen * 10 + thPhan;//chuyen ve double rui nhan 10 la dc rui. khung ghe noi ak.
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
                try
                {
                    int ph = 0;
                    if (txtMinAlarm.Text.IndexOf(".") > 0)
                    {
                        ph = txtMinAlarm.Text.IndexOf(".");
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

                    device35.Channels[g_channel].AlarmMin = nguyen * 10 + thPhan;
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

        private void txtLocation_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8)
            {
                if (txtLocation.Text.Length >= 40)
                {
                    e.Handled = true;
                    ToolTip t = new ToolTip();
                    t.SetToolTip(txtLocation, "Location can not be over 40 characters");
                }
                else if (((e.KeyChar == (char)60) || (e.KeyChar == (char)62)) || (e.KeyChar == (char)63) || (e.KeyChar == (char)47) || (e.KeyChar == (char)58) || (e.KeyChar == (char)124) || (e.KeyChar == (char)92) || (e.KeyChar == (char)42))
                {
                    e.Handled = true;
                    ToolTip t = new ToolTip();
                    t.SetToolTip(txtLocation, "Location can't contain any of the following characters" + Environment.NewLine + "\\ / < > ? : \" | *");
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
                    e.Handled = true;
                    ToolTip t = new ToolTip();
                    t.SetToolTip(txtDescription, "Description can not be over 40 characters");
                }
                else if (((e.KeyChar == (char)60) || (e.KeyChar == (char)62)) || (e.KeyChar == (char)63) || (e.KeyChar == (char)47) || (e.KeyChar == (char)58) || (e.KeyChar == (char)124) || (e.KeyChar == (char)92) || (e.KeyChar == (char)42))
                {
                    e.Handled = true;
                    ToolTip t = new ToolTip();
                    t.SetToolTip(txtDescription, "Description can't contain any of the following characters" + Environment.NewLine + "\\ / < > ? : \" | *");
                }
                else
                {
                    e.Handled = false;
                }
            }
        }

        private void Setting35_FormClosed(object sender, FormClosedEventArgs e)
        {
            device35.Close();
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

        private void txtSerial_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8)
            {
                if (txtSerial.Text.Length >= 10)
                {
                    e.Handled = true;
                    ToolTip t = new ToolTip();
                    t.SetToolTip(txtSerial, "Serial can not be over 10 characters");
                }
                else if ((e.KeyChar == (char)60) || (e.KeyChar == (char)62) || (e.KeyChar == (char)63) || (e.KeyChar == (char)47) || (e.KeyChar == (char)58) || (e.KeyChar == (char)124) || (e.KeyChar == (char)92) || (e.KeyChar == (char)42))
                {
                    e.Handled = true;
                    ToolTip t = new ToolTip();
                    t.SetToolTip(txtSerial, "Serial can't contain any of the following characters" + Environment.NewLine + "\\ / < > ? : \" | *");
                }
                else
                {
                    e.Handled = false;
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DateTime CurrentTime = DateTime.Now;
            //lblCurrentDate.Text = CurrentTime.ToShortDateString();
            //lblCurrentTime.Text = CurrentTime.ToString("HH:mm:ss");

            //tbCurrentTimeZone.Text = TimeZoneInfo.Local.DisplayName;

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
            //if (this.chbDayLight.Checked == true)
            //{
            //    ReadOnlyCollection<TimeZoneInfo> timeZones = TimeZoneInfo.GetSystemTimeZones();
            //    // Get each Time zone 
            //    foreach (TimeZoneInfo timeZone in timeZones)
            //    {
            //        if (timeZone.DisplayName == this.cbbTimeZone.Text)
            //        {

            //            TimeZoneInfo.AdjustmentRule[] adjustments = timeZone.GetAdjustmentRules();

            //            if (adjustments.Length == 0)
            //            {
            //                this.lbl_LoggerTime.Text = LoggerTime.ToString("HH:mm:ss");
            //                this.lbl_LoggerDate.Text = LoggerTime.ToLongDateString();
            //            }

            //            foreach (TimeZoneInfo.AdjustmentRule daylight in adjustments)
            //            {
            //                if (timeZone.IsDaylightSavingTime(LoggerTime) == true)
            //                {
            //                    theUTCTime1 = TimeZoneInfo.ConvertTimeToUtc(CurrentTime.AddHours(daylight.DaylightDelta.Hours).AddMinutes(daylight.DaylightDelta.Minutes).AddSeconds(daylight.DaylightDelta.Seconds), localZone);

            //                    try
            //                    {
            //                        OffsetHour = Convert.ToDouble(this.cbbTimeZone.Text.Substring(4, 3));
            //                        OffsetMin = Convert.ToDouble(this.cbbTimeZone.Text.Substring(8, 2));
            //                    }
            //                    catch (Exception)
            //                    {
            //                        OffsetHour = 0;
            //                        OffsetMin = 0;
            //                    }
            //                    LoggerTime = theUTCTime1.AddHours(OffsetHour).AddMinutes(OffsetMin);

            //                    this.lbl_LoggerDate.Text = LoggerTime.ToShortDateString();
            //                    this.lbl_LoggerTime.Text = LoggerTime.ToString("HH:mm:ss");
            //                }
            //                else
            //                {
            //                    this.lbl_LoggerTime.Text = LoggerTime.ToString("HH:mm:ss");
            //                    this.lbl_LoggerDate.Text = LoggerTime.ToString("MM/dd/yyyy");
            //                }
            //            }
            //        }
            //    }
            //}
            //else
            //{
                this.lbl_LoggerTime.Text = LoggerTime.ToString("HH:mm:ss");
                this.lbl_LoggerDate.Text = LoggerTime.ToShortDateString();
            //}
        }

        private void chkDes_CheckStateChanged(object sender, EventArgs e)
        {
            if (chkDes.Checked)
            {
                txtDescription.Enabled = true;
            }
            else
            {
                //txtDescription.Text = "";
                txtDescription.Enabled = false;
            }
        }

        private void chkChannel_CheckStateChanged(object sender, EventArgs e)
        {
            if (chkChannel.Checked)
            {
                groupBox4.Enabled = true;
            }
            else
            {
                groupBox4.Enabled = false;
            }
        }

        private void chkLoc_CheckStateChanged(object sender, EventArgs e)
        {
            if (chkLoc.Checked)
            {
                txtLocation.Enabled = true;
            }
            else
            {
                //txtLocation.Text = "";
                txtLocation.Enabled = false;
            }
        }

        private void chkRecord_CheckStateChanged(object sender, EventArgs e)
        {
            if (chkRecord.Checked)
            {
                groupBox2.Enabled = true;
                groupBox5.Enabled = true;
            }
            else
            {
                groupBox2.Enabled = false;
                groupBox5.Enabled = false;
            }
        }

    }
}
