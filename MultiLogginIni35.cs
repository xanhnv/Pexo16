using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Resources;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Pexo16
{
    public partial class MultiLogginIni35 : Form
    {
        ArrayList checkedDevice = new ArrayList();

          //Device selectedDevice = null;//X
        public Device35 device35 = null;

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

        //ToolStripComboBox tscb_timezone;
        private byte tmpChannel;


        ResourceManager res_man = new ResourceManager("Pexo16.Lang.Resources", typeof(LoggerIni_35).Assembly);
        CultureInfo cul;

        public MultiLogginIni35(ArrayList HostPort)
        {
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

            checkedDevice.Clear();
            checkedDevice = HostPort;

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

        public void btnReadSetting_Click(object sender, EventArgs e)
        {
            lblStatus.Text = res_man.GetString("Reading.....Please wait", cul);
            lblStatus.Update();
           // lblStatus.Visible = false;

            device35.hostport = checkedDevice[0].ToString();
            string StrDevs = device35.hostport;
            //Open Device
            if (device35.USBOpen(StrDevs) == false)
            {
                MessageBox.Show(res_man.GetString("Open USB fail. Please try again.", cul));
                lockButton(false);
                this.Close();
                return;
            }


            if (device35.readSerial() == false)
            {
                MessageBox.Show(res_man.GetString("Read Serial fail", cul));
                lockButton(false);
                device35.Close();
                return;
            }
            //read Location
            if (device35.readLocation() == false)
            {
                MessageBox.Show(res_man.GetString("Read Location fail", cul));
                lockButton(false);
                device35.Close();
                return;
            }
           
            //read Description
            if (device35.readDescription() == false)
            {
                MessageBox.Show(res_man.GetString("Read description fail", cul));
                lockButton(false);
                device35.Close();
                return;
            }
            //if (chboxSameDecAndLoc.Checked == true)
            //{
            //    txtDescription.Enabled = true;
            //    txtLocation.Enabled = true;
            //}
            //else
            //{
            //    txtDescription.Enabled = false;
            //    txtLocation.Enabled = false;
            //}

            lblSerial.Text = device35.Serial;
            txtLocation.Text = device35.Location;
            txtDescription.Text = device35.Description;

            //--------------read Setting (Time, TimeZone, Delay, Duration)
            if (device35.readSettingDevice() == false)
            {
                MessageBox.Show(res_man.GetString("Read Setting fail", cul));
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
                    lb_interval.Text = res_man.GetString("Sample interval", cul) + ": " + min2 + " "+res_man.GetString("Min", cul) + " " + sec1 + " " + res_man.GetString("sec", cul) + ".";
                }
                else
                {
                    lb_interval.Text = res_man.GetString("Sample interval", cul) + ": " + Convert.ToInt32(device35.Duration) + " " + res_man.GetString("sec", cul) + ".";
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
            

            //cbbDuration.Text = device35.Duration.ToString();

            //cbbStartDelay.Text = device35.Delay.ToString();

            //-----------read Setting Channel
            //if (device35.readSettingChannel() == false)
            //{
            //    MessageBox.Show(res_man.GetString("Read setting fail", cul));
            //    lockButton(false);
            //    device35.Close();
            //    return;
            //}


            for (int i = 0; i < 4; i++)
            {
                //device35.USBOpen(StrDevs);
                if (device35.readSettingChannel1(i) == false)
                {
                    MessageBox.Show(res_man.GetString("Read Setting", cul) + " " + res_man.GetString("fail", cul));
                    lockButton(false);
                    device35.Close();
                    return;
                }
                //device35.Close();
            }
            
            //for (int i = 0; i < 4; i++)
            //{
            //    //Read Data lenght
            //    if (!device35.readDataLenght(i))
            //    {
            //        MessageBox.Show("Read lenght Data fail");
            //        lockButton(false);
            //        device35.Close();
            //        return;
            //    }
            //    //sau khi readDataLenght se co dc device35.Channels[i].dataLenght va device35.Channels[i].numRecord
            //    int numRecord = device35.Channels[i].DataLenght / 64;
            //    int dataEnd = device35.Channels[i].DataLenght % 64;
            //    device35.readDataStart(i);
            //    device35.readDataEeprom(numRecord, i);
            //    device35.readDataStop(numRecord, dataEnd, i);
            //}
            if (device35.Duration == 65535)
            {
                cbbDuration.Text = cbbDuration.Items[0].ToString();
            }
            else
            {
                cbbDuration.Text = device35.Duration.ToString();
            }

            lblStatus.Text = ".....";
            device35.Close();

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
            lblCurrentDate.Text = CurrentTime.ToString("MM/dd/yyyy");
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
                            this.lbl_LoggerDate.Text = LoggerTime.ToString("MM/dd/yyyy");
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

                                this.lbl_LoggerDate.Text = LoggerTime.ToString("MM/dd/yyyy");
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
                this.lbl_LoggerDate.Text = LoggerTime.ToString("MM/dd/yyyy");
            }
        }

        private void btnEraseAllData_Click(object sender, EventArgs e)
        {
            lblStatus.Text = res_man.GetString("Erasing...Please wait", cul);
            lblStatus.Update();
            string StrDevs = device35.hostport;
            if (device35.USBOpen(StrDevs) == false)
            {
                lblStatus.Visible = false;
                MessageBox.Show(res_man.GetString("Open USB fail", cul));
                lockButton(false);
                device35.Close();
                return;
            }
            if(!device35.eraseSetting())
            {
                rtxtEventLog.Text += res_man.GetString("Erase fail", cul) + Environment.NewLine;
                device35.Close();
                return;
            }
            else
            {
                lblStatus.Text = "...";
                rtxtEventLog.Text += res_man.GetString("Erase Successfully", cul) + Environment.NewLine;
                device35.Close();
                btnReadSetting_Click(sender, e);
                cbbChannel_SelectedIndexChanged(sender, e);
                cbbUnit_SelectedIndexChanged(sender, e);
                return;
            }
        }

        private void btnWriteSetting_Click(object sender, EventArgs e)
        {
            if(cbbDuration.Text.Length == 0)
            {
                MessageBox.Show(res_man.GetString("Duration cannot be empty", cul));
                return;
            }
            if(cbbStartDelay.Text.Length == 0)
            {
                MessageBox.Show(res_man.GetString("Start delay cannot be empty", cul));
                return;
            }
            for (int i = 0; i < checkedDevice.Count; i++)
            {
                device35.hostport = checkedDevice[i].ToString();

                cbbChannel_SelectedIndexChanged(null, null);
                int dldura = Convert.ToInt32(cbbDuration.Text);
                string StrDevs = device35.hostport;

                //lblStatus.Text = "Sending...Please wait";
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

                //try
                //{
                //    int a1 = Convert.ToInt32(txtMaxAlarm.Text);
                //}
                //catch (Exception)
                //{
                //    MessageBox.Show("Max Input is not available");
                //    return;
                //}

                //try
                //{
                //    int a2 = Convert.ToInt32(txtMinAlarm.Text);
                //}
                //catch (Exception)
                //{
                //    MessageBox.Show("Min Input is not available");
                //    return;
                //}

                //if (Convert.ToInt32(txtMaxAlarm.Text) <= Convert.ToInt32(txtMinAlarm.Text) && chbNoAlarm.Checked == false)
                //{
                //    MessageBox.Show("Max value must higher than Min value");
                //    return;
                //}

                for (int j = 0; j < 4; j++)
                {
                    if (!device35.Channels[j].NoAlarm)
                    {
                        if (device35.Channels[j].AlarmMax <= device35.Channels[j].AlarmMin)
                        {
                            MessageBox.Show(res_man.GetString("Max alarm of channel", cul) + " " + (j + 1) + " " + res_man.GetString("must higher than min alarm of channel", cul) + " " + (j + 1));
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
                    MessageBox.Show(res_man.GetString("Open USB fail", cul));
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
                if (chboxSameDecAndLoc.Checked == true)
                {
                    device35.Location = txtLocation.Text;
                    device35.Description = txtDescription.Text;
                }
                else //neu khong thi read lai loc, des cu.
                {
                    device35.readDescription();
                    device35.readLocation();
                }
             
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
                    rtxtEventLog.Text += res_man.GetString("Write Location successfully", cul) + Environment.NewLine;
                }

                //------write Description
                Thread.Sleep(500);
                device35.Description = txtDescription.Text;
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


                //-------------------write Setting (Time, TimeZone, Delay, Duration).
                Thread.Sleep(500);
                string TimeZone = null;
                try
                {
                    TimeZone = mGlobal.FindSystemTimeZoneFromDisplayName(cbbTimeZone.Text).Id.ToString();
                    //get 15 first char and 3 last char
                    //TimeZone = TimeZone.Substring(0, 15) + TimeZone.Substring(Math.Max(TimeZone.Length - 2, 1) - 1).Substring(TimeZone.Substring(Math.Max(TimeZone.Length - 2, 1) - 1).Length - 3, 3);
                    if(TimeZone.Length > 22)
                    {
                        TimeZone = TimeZone.Substring(0, 22);
                    }
                }
                catch (Exception)
                {
                    TimeZone = "local";
                }

                device35.Duration = int.Parse(cbbDuration.Text);
                device35.Startrec = int.Parse(cbbStartDelay.Text);
                if (device35.writeSettingDevice(LoggerTime, TimeZone) == false)
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
                    rtxtEventLog.Text += res_man.GetString("Write setting successfully", cul) + Environment.NewLine;
                }


                //-----------------write setting Channel (Unit, sensor, description, alarm)
                Thread.Sleep(500);
                if (chboxSameDescChannel.Checked == true)
                {
                    device35.Channels[g_channel].Desc = txtChlDesc.Text;
                }

                if (device35.writeSettingChannel() == false)
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
                    lblStatus.Text = res_man.GetString("Write done", cul);
                    rtxtEventLog.Text += res_man.GetString("Write setting Channel successfully", cul) + Environment.NewLine;
                }
                device35.Close();
            }
        }

        private void cbbChannel_SelectedIndexChanged(object sender, EventArgs e)
        {
            //-------------------------------------------------------------------
            //tmpCheckedManual = False
            txtChlDesc.Text = "";
            if (cbbChannel.SelectedItem == null)
            {
                cbbChannel.SelectedItem = cbbChannel.Items[0];
            }
            byte tmpCH = byte.Parse(cbbChannel.SelectedItem.ToString());

            cbbUnit.Items.Clear();
            cbbUnit.Items.AddRange(new string[] { "Not Use", "Celsius", "Fahrenheit", "CO2", "%RH", "G" });

            g_channel = byte.Parse((int.Parse(cbbChannel.SelectedItem.ToString()) - 1).ToString());
            g_unit = device35.Channels[g_channel].Unit;
            g_sensor = device35.Channels[g_channel].Sensor;

            txtChlDesc.Text = device35.Channels[g_channel].Desc;

            //ChlDesc = txtChlDesc.Text;
            //selectedDevice.Channels[g_channel].Desc = ChlDesc;

            //cbbUnit.Text = mGlobal.IntToUnit(g_unit);
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
                //txtMaxAlarm.Text = device35.Channels[g_channel].AlarmMax.ToString();

                int nguyen2 = device35.Channels[g_channel].AlarmMin / 10;
                int thphan2 = device35.Channels[g_channel].AlarmMin % 10;
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
                //device35.Channels[g_channel].AlarmMax = (int)(float.Parse(txtMaxAlarm.Text));
                try
                {
                    int ph = 0;
                    if (txtMaxAlarm.Text.IndexOf(",") > 0)
                    {
                        ph = txtMaxAlarm.Text.IndexOf(",");
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

                try
                {
                    int ph = 0;
                    if (txtMinAlarm.Text.IndexOf(",") > 0)
                    {
                        ph = txtMinAlarm.Text.IndexOf(",");
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
                x = (128 * 1024) / mGlobal.numChan;
            }
            else
            {
                x = 128 * 1024;
            }
            if (cbbDuration.Text.Length == 0)
            {
                MessageBox.Show(res_man.GetString("Please set record time", cul));
                return;
            }
            tg = mGlobal.duration35(Convert.ToInt32(cbbDuration.Text), x);
            //tg = mGlobal.Duration(Convert.ToInt32(cbbDuration.Text));
            if (tg > 60)
            {
                min = tg / 60;
                sec = tg % 60;
                lb_interval.Text = res_man.GetString("Sample interval", cul) + ": " + min + " min " + sec + " sec.";
            }
            else
            {
                lb_interval.Text = res_man.GetString("Sample interval", cul) + ": " + tg + " sec.";
            }
            device35.Duration = int.Parse(cbbDuration.Text);    
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
                }
                else if (((e.KeyChar == (char)60) || (e.KeyChar == (char)62)) || (e.KeyChar == (char)63) || (e.KeyChar == (char)47) || (e.KeyChar == (char)58) || (e.KeyChar == (char)124) || (e.KeyChar == (char)92) || (e.KeyChar == (char)42))
                {
                    //MessageBox.Show(e.KeyChar + "is not available");
                    e.Handled = true;
                    ToolTip t = new ToolTip();
                    //t.SetToolTip(txtLocation, e.KeyChar + "is not available");
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
                    //t.SetToolTip(txtDescription, "Cannot typed " + e.KeyChar + "character");
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
                        device35.Channels[g_channel].Unit = 0;
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
                        if (device35.Channels[g_channel].Unit == 0xAC || device35.Channels[g_channel].Unit == 0xAF || device35.Channels[g_channel].Unit == 0xFF)
                        {
                            device35.Channels[g_channel].Unit = 0x00;
                        }
                        //if (device35.Channels[g_channel].Sensor == 3)
                       // {
                            //string hex = device35.Channels[g_channel].Unit.ToString("X");
                            //int indexs = int.Parse(hex.Substring(0, 1));
                            //cbbFS.Text = cbbFS.Items[indexs].ToString();
                            //cbbBR.Text = cbbBR.Items[indexs + 1].ToString();
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
                        device35.Channels[g_channel].Sensor = 3;
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
            lblBR.Enabled = status;
            lblFS.Enabled = status;
            cbbBR.Enabled = status;
            cbbFS.Enabled = status;
            lblG.Enabled = status;
            lblHZ.Enabled = status;
        }

        private void txtChlDesc_Validating(object sender, CancelEventArgs e)
        {
            //if (txtChlDesc.Text.Trim().Length > 40)
            //{
            //    MessageBox.Show("Channel description can not be over 40 characters.");
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
            //        MessageBox.Show("Channel description must be typed in English");
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

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (chboxSameDecAndLoc.Checked == true)
            {
                txtDescription.Enabled = true;
                txtLocation.Enabled = true;
            }
            else
            {
                txtDescription.Enabled = false;
                txtLocation.Enabled = false;
            }
        }

        private void chboxSameDescChannel_CheckedChanged(object sender, EventArgs e)
        {
            if (chboxSameDescChannel.Checked == true)
            {
                txtChlDesc.Enabled = true;
            }
            else
            {
                txtChlDesc.Enabled = false;
            }
        }

        private void MultiLogginIni35_Load(object sender, EventArgs e)
        {
            switch(mGlobal.language)
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
            lb_interval.Text = res_man.GetString("Max 10 digits", cul);
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
            chboxSameDecAndLoc.Text = res_man.GetString("Same Desc. and Loc. for all device", cul);
            chboxSameDescChannel.Text = res_man.GetString("Same Channel's Desc for all device", cul);


            SystemEvents.TimeChanged += new EventHandler(SystemEvent_TimeChanged);

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

            cbbDuration.Text = cbbDuration.Items[0].ToString();

            chboxSameDecAndLoc.Checked = false;
            chboxSameDescChannel.Checked = false;

            btnReadSetting_Click(sender, e);//numberOfChannel'value

            for (int i = 1; i <= 4; i++)
            {
                cbbChannel.Items.Add(i);
            }

            lblSerial.Text = device35.Serial;
            cbbChannel.Text = cbbChannel.Items[0].ToString();
            cbbChannel.SelectedIndex = 0;

            if (device35.numOfChannel != 8)
            {
                cbbUnit.Items.Clear();
                cbbUnit.Items.AddRange(new string[] { "Not Use", "Celsius", "Farenheit", "CO2", "%RH", "G" });
            }
            cbbUnit_SelectedIndexChanged(sender, e);
            cbbChannel_SelectedIndexChanged(sender, e);

            cbbDuration.DropDownStyle = ComboBoxStyle.DropDownList;
            cbbStartDelay.DropDownStyle = ComboBoxStyle.DropDownList;
            cbbUnit.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void MultiLogginIni35_FormClosed(object sender, FormClosedEventArgs e)
        {
            device35.Close();
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
                    t.SetToolTip(txtChlDesc, res_man.GetString("Description of this channel can not be over 40 characters", cul));
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

    }
}
