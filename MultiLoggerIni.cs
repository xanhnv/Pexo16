﻿using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
//using Microsoft.Win32;
using System.Resources;
using System.Text;
using System.Windows.Forms;

namespace Pexo16
{
    public partial class MultiLoggerIni : Form
    {       
        Device selectedDevice = null;
        Graph saveGraph = null;

        private byte g_channel;
        private byte Logging;
        private DateTime LoggerTime;
        private byte g_unit;
       // private string selectedHostPort;
        public string defaultFolder = "";
        private bool sttCF; // bien nay dung de khong che, k cho goi ham draw_graph 2 lan
        public byte[] datalogger;
        public DateTime _logger_date;
        private bool StopLogged;
        long[] len = new long[9];

        //ToolStripComboBox tscb_timezone;
        private byte tmpChannel;

        ArrayList checkedDevice = new ArrayList();

        ResourceManager res_man = new ResourceManager("Pexo16.Lang.Resources", typeof(LoggerIni_35).Assembly);
        CultureInfo cul;


        public MultiLoggerIni(ArrayList HostPort)
        {
            InitializeComponent();

            //one Device
            selectedDevice = Device.DelInstance();//xoa instance truoc do.
            selectedDevice = Device.Instance;// tao mot instance moi.
            saveGraph = Graph.DelInstance();//xoa instance da tao truoc do.
            saveGraph = Graph.Instance;//tao mot instance moi.
            //selectedHostPort = HostPort[0].ToString();
            //selectedDevice.hostport = HostPort[0].ToString();

            //multi Device
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

        private void frmLoggerIni_Load(object sender, EventArgs e)
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
            btnMultiWriteSetting.Text = res_man.GetString("Write Setting", cul);
            btnMultiReadSetting.Text = res_man.GetString("Read Setting", cul);
            btnMultiEraseAllData.Text = res_man.GetString("Erase All Data", cul);
            chboxSameDecAndLoc.Text = res_man.GetString("Same Desc. and Loc. for all device", cul);
            chboxSameDescChannel.Text = res_man.GetString("Same Channel's Desc for all device", cul);


            //CultureInfo.CurrentCulture.ClearCachedData();
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

            btnReadSetting_Click(sender, e);//numberOfChannel'value

            for (int i = 1; i <= selectedDevice.numOfChannel; i++)
            {
                cbbChannel.Items.Add(i);
            }

            lblSerial.Text = selectedDevice.Serial;
            cbbChannel.Text = cbbChannel.Items[0].ToString();

            if(selectedDevice.numOfChannel != 8)
            {
                cbbUnit.Items.Clear();
                cbbUnit.Items.AddRange(new string[]{"Not Use", "Celsius", "Farenheit", "CO2", "%RH"});
            }

            cbbDuration.DropDownStyle = ComboBoxStyle.DropDownList;
            cbbStartDelay.DropDownStyle = ComboBoxStyle.DropDownList;
            cbbUnit.DropDownStyle = ComboBoxStyle.DropDownList;
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

        public void btnReadSetting_Click(object sender, EventArgs e)
        {
            lblStatus.Text = res_man.GetString("Reading.....Please wait", cul);
            lockButton(true);
            lblStatus.Visible = false;


            for (int i = 0; i < checkedDevice.Count; i++)//to read logging status from all device.
            {
                string Devs = checkedDevice[i].ToString();
                //Read Logging Status
                if (selectedDevice.USBOpen(Devs) == false)
                {
                    MessageBox.Show(res_man.GetString("Open USB fail. Please try again.", cul));
                    lockButton(false);
                    this.Close();
                    return;
                }
                if (selectedDevice.Read_DataLength() == false)
                {
                    MessageBox.Show(res_man.GetString("Logging Status fail", cul));
                    lockButton(false);
                    return;
                }
                selectedDevice.Close();
                Logging += selectedDevice.byteLogging;
            }
            if (Logging != 0)
            {
                btnMultiEraseAllData.Text = res_man.GetString("Stop Logging", cul);
                btnMultiWriteSetting.Enabled = false;
                btnMultiReadSetting.Enabled = false;
            }
            else
            {
                btnMultiEraseAllData.Text = res_man.GetString("Erase All Data", cul);
            }
   
            //string StrDevs = selectedDevice.hostport;
            selectedDevice.hostport = checkedDevice[0].ToString();// checked device from bulk program (set multi device);
            string StrDevs = selectedDevice.hostport;
            //Read Logging Status
            if (selectedDevice.USBOpen(StrDevs) == false)
            {
                MessageBox.Show(res_man.GetString("Open USB fail. Please try again.", cul));
                lockButton(false);
                this.Close();
                return;
            }
            if (selectedDevice.Read_DataLength() == false)
            {
                MessageBox.Show(res_man.GetString("Logging Status fail", cul));
                lockButton(false);
                return;
            }
            selectedDevice.Close();


            //--------------------read setting
            selectedDevice.USBOpen(StrDevs);  ///nnnnn
            selectedDevice.channels = new Channel[selectedDevice.numOfChannel];
            for (int i = 0; i < selectedDevice.numOfChannel; i++)
            {
                selectedDevice.channels[i] = new Channel();
            }
 
            if (selectedDevice.Read_setting() == false)  
            {
                MessageBox.Show(res_man.GetString("Read setting fail", cul));
                lockButton(false);
                return;
            }
            selectedDevice.Close();
            string a;
            try
            {
                a = cbbChannel.SelectedItem.ToString();
            }
            catch
            {
                a = "1";
            }

            g_channel = byte.Parse((int.Parse(a) -1).ToString());
            g_unit = selectedDevice.channels[g_channel].Unit;

            if (mGlobal.IntToUnit(g_unit) == "ppm")
            {
                cbbUnit.Text = "CO2";
            }
            else
            {
                cbbUnit.Text = mGlobal.IntToUnit(g_unit);
            }

            lb_unitmax.Text = mGlobal.IntToUnit(g_unit);
            lb_unitmin.Text = mGlobal.IntToUnit(g_unit);

            for (byte i = 0; i < selectedDevice.numOfChannel; i++)
            {
                if (selectedDevice.channels[i].NoAlarm == true)
                {
                    getDefaultAlarm(selectedDevice.channels[i].AlarmMax, selectedDevice.channels[i].AlarmMin, selectedDevice.channels[i].Unit);
                }
            }

            if (selectedDevice.channels[int.Parse(g_channel.ToString())].NoAlarm)
            {
                txtMaxAlarm.Text = 0.ToString();
                txtMinAlarm.Text = 0.ToString();
            }
            else
            {
                txtMaxAlarm.Text = selectedDevice.channels[int.Parse(g_channel.ToString())].AlarmMax.ToString();
                txtMinAlarm.Text = selectedDevice.channels[int.Parse(g_channel.ToString())].AlarmMin.ToString();
            }


            //zzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzz
            if (selectedDevice.channels[g_channel].NoAlarm == true)
            {
                chbNoAlarm.Checked = true;
            }
            else
            {
                chbNoAlarm.Checked = false;
            }
            //zzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzz

            int sec1 = 0;
            int min2 = 0;

            if (Convert.ToInt32(selectedDevice.Duration) > 60)
            {
                min2 = Convert.ToInt32(selectedDevice.Duration) / 60;
                sec1 = Convert.ToInt32(selectedDevice.Duration) % 60;
                lb_interval.Text = res_man.GetString("Sample Interval:", cul) + " " + min2 + " " + res_man.GetString("Min", cul) + " " + sec1 + " " + res_man.GetString("sec", cul) + ".";
            }
            else
            {
                lb_interval.Text = res_man.GetString("Sample Interval:") + " " + Convert.ToInt32(selectedDevice.Duration) + " " + res_man.GetString("sec", cul) + ".";
            }

            if (selectedDevice.Duration == 2)
            {
                cbbDuration.Text = (string)cbbDuration.Items[0];
            }
            else if (selectedDevice.Duration == 6)
            {
                cbbDuration.Text = (string)cbbDuration.Items[1];
            }
            else if (selectedDevice.Duration == 10)
            {
                cbbDuration.Text = (string)cbbDuration.Items[2];
            }
            else if (selectedDevice.Duration == 22)
            {
                cbbDuration.Text = (string)cbbDuration.Items[3];
            }
            else if (selectedDevice.Duration == 40)
            {
                cbbDuration.Text = (string)cbbDuration.Items[4];
            }
            else if (selectedDevice.Duration == 80)
            {
                cbbDuration.Text = (string)cbbDuration.Items[5];
            }
            else if (selectedDevice.Duration == 119)
            {
                cbbDuration.Text = (string)cbbDuration.Items[6];
            }
            else if (selectedDevice.Duration == 482)
            {
                cbbDuration.Text = (string)cbbDuration.Items[7];
            }


            if (selectedDevice.Startrec == 0)
            {
                cbbStartDelay.Text = cbbStartDelay.Items[0].ToString();
            }
            else if (selectedDevice.Startrec == 1)
            {
                cbbStartDelay.Text = cbbStartDelay.Items[1].ToString();
            }
            else if (selectedDevice.Startrec == 2)
            {
                cbbStartDelay.Text = cbbStartDelay.Items[2].ToString();
            }
            else if (selectedDevice.Startrec == 5)
            {
                cbbStartDelay.Text = cbbStartDelay.Items[3].ToString();
            }
            else if (selectedDevice.Startrec == 10)
            {
                cbbStartDelay.Text = cbbStartDelay.Items[4].ToString();
            }
            else if (selectedDevice.Startrec == 20)
            {
                cbbStartDelay.Text = cbbStartDelay.Items[5].ToString();
            }

            //-------------------read Location, Serial, Description
            selectedDevice.USBOpen(selectedDevice.hostport);        //vvvvv
            if (selectedDevice.readLocationSerialDescription() == false)
            {
                MessageBox.Show(res_man.GetString("Read Location, Serial, Description Fail", cul) + selectedDevice.Status);
                lockButton(false);
                selectedDevice.Close();
                return;
            }
            selectedDevice.Close();
            txtDescription.Text = selectedDevice.Description;
            txtLocation.Text = selectedDevice.Location;
            if(chboxSameDecAndLoc.Checked == true)
            {
                txtDescription.Enabled = true;
                txtLocation.Enabled = true;
            }
            else
            {
                txtDescription.Enabled = false;
                txtLocation.Enabled = false;
            }


            //-------------------read Description1 Channel
            selectedDevice.USBOpen(selectedDevice.hostport);
            if (selectedDevice.readDescriptionChannel() == false)
            {
                MessageBox.Show(res_man.GetString("Read Description Channel", cul) + selectedDevice.Status);
                lockButton(false);
                selectedDevice.Close();
                return;
            }
            selectedDevice.Close();
            txtChlDesc.Text = selectedDevice.channels[g_channel].Desc;
            if(chboxSameDescChannel.Checked == true)
            {
                txtChlDesc.Enabled = true;
            }
            else
            {
                txtChlDesc.Enabled = false;
            }

            //-----------------------read Timezone
            selectedDevice.USBOpen(selectedDevice.hostport);
            if (selectedDevice.ReadDateTime() == false)
            {
                MessageBox.Show(res_man.GetString("Read TimeZone fail", cul));
                lockButton(false);
                return;
            }
            selectedDevice.Close();

            string TimeZone = mGlobal.FindSystemTimeZoneFromString(selectedDevice.Timezone.ToString().ToString()).ToString();
            for (int i = 0; i <= cbbTimeZone.Items.Count - 1; i++)
            {
                if (cbbTimeZone.Items[i].ToString() == TimeZone)
                {
                    cbbTimeZone.Text = cbbTimeZone.Items[i].ToString();
                }
            }

            lockButton(false);
        }

        public void lockButton(bool tmp)
        {
            if (tmp == true)
            {
                btnMultiWriteSetting.Enabled = false;
                btnMultiReadSetting.Enabled = false;
                btnMultiEraseAllData.Enabled = false;
            }
            else
            {
                btnMultiWriteSetting.Enabled = true;
                btnMultiReadSetting.Enabled = true;
                btnMultiEraseAllData.Enabled = true;
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

        public void btnMultiEraseAllData_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedDevice.Count; i++)
            {
                selectedDevice.hostport = checkedDevice[i].ToString();

                string StrDevs = selectedDevice.hostport;

                SaveFileDialog SaveFileDialog1 = new SaveFileDialog();
                string FilePath = null;

                string str1 = selectedDevice.Description;
                string str2 = selectedDevice.Serial;

                int length_str1 = 0;
                //
                for (int j = 1; j <= 20; j++)
                {
                    length_str1 = str1.Length;
                    if (str1.Substring(str1.Length - 1) == " ")
                    {
                        str1 = selectedDevice.Description.Substring(0, length_str1 - 1);
                    }
                }
                if (defaultFolder == "")
                {
                    try
                    {
                        SaveFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    }
                    catch (Exception)
                    {
                    }
                }
                else
                {
                    SaveFileDialog1.InitialDirectory = defaultFolder;
                }

                SaveFileDialog1.Filter = "Marathon Data Logger File (*.D8S)|*.D8S|Marathon Data Logger File (*.D4S)|*.D4S";

                SaveFileDialog1.FilterIndex = 1;
                SaveFileDialog1.RestoreDirectory = true;

                string check_name = null;

                SaveFileDialog1.FileName = DateTime.Now.ToString("ddMMyy") + "_" + selectedDevice.Location + "_" + str2 + "_" + str1 + ".D" + selectedDevice.numOfChannel + "S";
                check_name = DateTime.Now.ToString("ddMMyy") + "_" + selectedDevice.Location + "_" + str2 + "_";

                DialogResult checkResult = 0;

                lockButton(true);
                if (StrDevs.IndexOf("VID") + 1 != 0)
                {
                    if (selectedDevice.USBOpen(StrDevs) == false)
                    {
                        lblStatus.Visible = false;
                        MessageBox.Show(res_man.GetString("Open USB fail", cul));
                        MessageBox.Show("Open USB false. Setting of device "  + checkedDevice[i] + " will be skipped.");
                        rtxtEventLog.Text += "Setting of device " + checkedDevice[i] + " is skipped" + "\r\n";
                        lockButton(false);
                        selectedDevice.Close();
                        return;
                    }

                    //================================
                    //Check Logging Status
                    if (selectedDevice.Read_DataLength() == false)
                    {
                        lblStatus.Visible = false;
                        MessageBox.Show(res_man.GetString("Read setting fail", cul));
                        lockButton(false);
                        selectedDevice.Close();
                        return;
                    }
                    selectedDevice.Close();
                    Logging = selectedDevice.byteLogging;

                    long tempLength = selectedDevice.lDataLength;

                    if (btnMultiEraseAllData.Text == res_man.GetString("Stop Logging", cul))//
                    {
                        // Device is stop, exit now
                        if (Logging == 0)
                        {
                            //btnMultiEraseAllData.Text = "Erase All Data";
                            lblStatus.Visible = false;
                            lockButton(false);
                            //return;
                        }
                        else
                        {
                            if (MessageBox.Show(res_man.GetString("Do you want to stop logging Data from", cul) + " " + checkedDevice[i].ToString(), res_man.GetString("Stop Logging", cul), MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.No)
                            {
                                lblStatus.Visible = false;
                                lockButton(false);
                                return;
                            }
                            //Else Send Stop command
                            else
                            {
                                selectedDevice.USBOpen(StrDevs);
                                if (selectedDevice.StopLogging() == false)
                                {
                                    lblStatus.Visible = false;
                                    MessageBox.Show(res_man.GetString("Stop logging fail", cul));
                                    rtxtEventLog.Text += res_man.GetString("Stop logging fail", cul) + Environment.NewLine;
                                    lockButton(false);
                                    return;
                                }
                                rtxtEventLog.Text += res_man.GetString("Stop logging successfully", cul) + Environment.NewLine;
                                selectedDevice.Close();
                                lblStatus.Visible = false;
                                lockButton(false);
                                //btnMultiEraseAllData.Text = "Erase All Data";
                                StopLogged = true;
                            }
                        }
                    }

                    else //If Button1.Text = "Erase All Data" then
                    {
                        if (tempLength == 0)
                        {
                            rtxtEventLog.Text += checkedDevice[i] + " does not have Data." + "\r\n";
                            //checkResult = MessageBox.Show(checkedDevice[i] + " does not have Data. No Data will be saved. Do you want to continue?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);

                            //if (checkResult == System.Windows.Forms.DialogResult.No)
                            //{
                             //   lockButton(false);
                            //    return;
                            //}
                            if(i == checkedDevice.Count - 1)
                            {
                                lockButton(false);
                                return;
                            }
                            continue;
                        }
                        else
                        {
                            string s = res_man.GetString("All Data of",cul) + " " + checkedDevice[i] +" " + res_man.GetString("will be lost. Do you want to save Data before erasing?", cul);

                            checkResult = MessageBox.Show(s, res_man.GetString("Erase Data", cul), MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);

                            if (checkResult == System.Windows.Forms.DialogResult.Yes)
                            {
                                sttCF = true;
                                //set Unit default
                                mGlobal.C2F = true;
                                selectedDevice.USBOpen(StrDevs);
                                if (selectedDevice.readLocationSerialDescription() == false)
                                {
                                    MessageBox.Show(res_man.GetString("Read Location, Serial, Description Fail", cul));
                                    mGlobal.ColorChanged = false;
                                    mGlobal.TitleChanged = false;
                                    mGlobal.CommentChanged = false;
                                    this.Close();
                                    return;
                                }
                                selectedDevice.Close();

                                selectedDevice.titlegraph = "Marathon Electronic Data Logger";
                                selectedDevice.USBOpen(StrDevs);
                                selectedDevice.Read_DataLength();

                                selectedDevice.ReadData_Start();
                                label16.Visible = true;
                                do
                                {
                                    label16.Text = res_man.GetString("Downloading...", cul) + Math.Round(double.Parse(selectedDevice.lReceivedByteNum.ToString()) / selectedDevice.lDataLength * 100) + "%";
                                    System.Windows.Forms.Application.DoEvents();
                                }
                                while (!(selectedDevice.boolWaiting == false));

                                label16.Visible = false;
                                selectedDevice.Close();
                                selectedDevice.USBOpen(StrDevs);
                                System.IO.FileStream oFileStream = null;

                                string FileName = "";
                                FileName = mGlobal.app_patch(FileName);

                                oFileStream = new System.IO.FileStream(FileName + "\\Data.bin", System.IO.FileMode.Create);

                                oFileStream.Write(mGlobal.ibuf_read, 0, selectedDevice.lDataLength);
                                oFileStream.Close();
                                selectedDevice.Close();

                                if (System.IO.File.Exists(FileName + "\\Data.bin") == true)
                                {
                                    datalogger = System.IO.File.ReadAllBytes(FileName + "\\Data.bin");
                                }
                                else
                                {
                                    MessageBox.Show(res_man.GetString("File not found", cul));
                                    mGlobal.ColorChanged = false;
                                    mGlobal.TitleChanged = false;
                                    mGlobal.CommentChanged = false;
                                    this.Close();
                                    return;
                                }

                                byte[] data_byte = new byte[datalogger.Length];
                                for (int dd = 0; dd < datalogger.Length; dd++)
                                {
                                    data_byte[dd] = datalogger[dd];
                                }

                                saveGraph._timezone = mGlobal.FindSystemTimeZoneFromString(selectedDevice.Timezone);
                                saveGraph.set_graph(data_byte);

                                byte[] data_save = { 0 };

                                if (SaveFileDialog1.ShowDialog() == DialogResult.OK)
                                {
                                    string filename_koduoi = null;
                                    string filename_duoi = null;
                                    string filename_checknow = null;
                                    FilePath = SaveFileDialog1.FileName.ToString();

                                    filename_duoi = FilePath.Substring(FilePath.LastIndexOf("\\") + 1);
                                    filename_koduoi = filename_duoi.Substring(0, filename_duoi.LastIndexOf("."));
                                    filename_checknow = filename_koduoi.Substring(0, check_name.Length);

                                    if (FilePath.Substring(FilePath.Length - 4) != ".D" + selectedDevice.numOfChannel + "S")
                                    {
                                        FilePath = FilePath + ".D" + selectedDevice.numOfChannel + "S";
                                    }

                                    selectedDevice.SaveFile_MP_Lgr2(ref data_save);
                                    try
                                    {
                                        System.IO.File.WriteAllBytes(FilePath, data_save);
                                        rtxtEventLog.Text += checkedDevice[i] + ": " + res_man.GetString("File has been saved successfully", cul) + Environment.NewLine;
                                    }
                                    catch (Exception)
                                    {
                                        rtxtEventLog.Text += checkedDevice[i] + ": " + res_man.GetString("Fail to save file", cul) + Environment.NewLine;
                                        return;
                                    }
                                }
                                else
                                {
                                    MessageBox.Show(res_man.GetString("Save file is cancelled. Device will not be erased.", cul));
                                    lockButton(false);
                                    return;
                                }
                            }
                            else if (checkResult == System.Windows.Forms.DialogResult.No)
                            { }
                            else
                            {
                                lockButton(false);
                                return;
                            }
                        }
                        lblStatus.Visible = true;
                        lblStatus.Text = res_man.GetString("Erasing...Please wait", cul);
                        lblStatus.Update();
                        System.Windows.Forms.Application.DoEvents();

                        //=============Read Timezone======
                        selectedDevice.USBOpen(StrDevs);
                        if (selectedDevice.ReadDateTime() == false)
                        {
                            lblStatus.Visible = false;
                            MessageBox.Show(res_man.GetString("Read TimeZone fail", cul));
                            lockButton(false);
                            selectedDevice.Close();
                            return;
                        }

                        string TimeZone = mGlobal.FindSystemTimeZoneFromString(selectedDevice.Timezone.ToString()).ToString();
                        for (int j = 0; j < cbbTimeZone.Items.Count; j++)
                        {
                            if (cbbTimeZone.Items[j].ToString() == TimeZone)
                            {
                                cbbTimeZone.Text = cbbTimeZone.Items[j].ToString();
                            }
                        }
                        //=============Erase======
                        if (selectedDevice.EraseFLASH() == false)
                        {
                            lblStatus.Visible = false;
                            MessageBox.Show(res_man.GetString("Erase fail", cul));
                            rtxtEventLog.Text += res_man.GetString("Erase fail", cul) + Environment.NewLine;
                            lockButton(false);
                            selectedDevice.Close();
                            return;
                        }
                        else
                        {
                            System.Threading.Thread.Sleep(200);

                            //=============Write Timezone======
                            //Dim TimeZone As String
                            try
                            {
                                TimeZone = mGlobal.FindSystemTimeZoneFromDisplayName(cbbTimeZone.Text).Id.ToString();
                                //get 15 first char and 3 last char
                            }
                            catch (Exception)
                            {
                                TimeZone = "local";
                            }

                            if (selectedDevice.WriteDateTime(LoggerTime, TimeZone) == false)
                            {
                                lblStatus.Visible = false;
                                MessageBox.Show(res_man.GetString("Erase fail", cul));
                                lockButton(false);
                                rtxtEventLog.Text += res_man.GetString("Write date and Timezone fail", cul) + Environment.NewLine;
                                return;
                            }
                            else
                            {
                                rtxtEventLog.Text += res_man.GetString("Write date and Timezone successfully", cul) + Environment.NewLine;
                            }

                            //================================
                            lblStatus.Visible = false;
                            MessageBox.Show(res_man.GetString("Erase Successfully", cul));
                            rtxtEventLog.Text += res_man.GetString("Erase Successfully", cul) + Environment.NewLine;
                            selectedDevice.Close();
                            lockButton(false);
                        }
                    }
                }
            }
            if(StopLogged == true)
            {
                btnMultiEraseAllData.Text = res_man.GetString("Erase All Data", cul);
            }
            return;
        }

        private void btnWriteSetting_Click(object sender, EventArgs e)
        {
            if(cbbStartDelay.Text.Length == 0)
            {
                MessageBox.Show(res_man.GetString("Start delay cannot be empty", cul));
                return;
            }
            if(cbbDuration.Text.Length == 0)
            {
                MessageBox.Show(res_man.GetString("Duration cannot be empty", cul));
                return;
            }

            for (int i = 0; i < checkedDevice.Count; i++)
            {
                Device tempDev = Device.DelInstance();
                tempDev = Device.Instance;

                selectedDevice.hostport = checkedDevice[i].ToString();

                cbbChannel_SelectedIndexChanged(null, null);
                int dldura = Convert.ToInt32(cbbDuration.Text);
                string StrDevs = selectedDevice.hostport;

                lblStatus.Text = res_man.GetString("Sending...Please wait", cul);
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

                for (int j = 0; j < selectedDevice.numOfChannel; j++)
                {
                    if (selectedDevice.channels[j].AlarmMax <= selectedDevice.channels[j].AlarmMin)
                    {
                        MessageBox.Show(res_man.GetString("Max alarm of channel", cul) + " " + (j + 1) + " " + res_man.GetString("must higher than min alarm of channel", cul) + " " + (j + 1));
                        return;
                    }
                    if (!selectedDevice.channels[j].NoAlarm)
                    {
                        if (selectedDevice.channels[j].AlarmMax == 30000)
                        {
                            MessageBox.Show(res_man.GetString("Alarm max of channel", cul) + " " + (j + 1) + " " + res_man.GetString("can not be empty", cul));
                            return;
                        }
                        if(selectedDevice.channels[j].AlarmMin == -30000)
                        {
                            MessageBox.Show(res_man.GetString("Alarm min of channel") + " " + (j + 1) + " " + res_man.GetString("can not be empty", cul));
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

                lockButton(true);

                if (selectedDevice.USBOpen(StrDevs) == false)
                {
                    lblStatus.Visible = false;
                    MessageBox.Show(res_man.GetString("Open USB fail", cul));
                    lockButton(false);
                    selectedDevice.Close();
                    return;
                }

                if (selectedDevice.Read_DataLength() == false)
                {
                    lblStatus.Visible = false;
                    MessageBox.Show(res_man.GetString("Logging Status fail", cul));
                    lockButton(false);
                    selectedDevice.Close();
                    return;
                }

                if (selectedDevice.lDataLength != 0)
                {
                    lblStatus.Visible = false;
                    MessageBox.Show(res_man.GetString("Writting: Inhibit. Data was not erased", cul) + Environment.NewLine + res_man.GetString("Please stop recording first and erase Data logger before write setting", cul), res_man.GetString("Logger Initialization", cul), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    lockButton(false);
                    selectedDevice.Close();
                    return;
                }

                //--------------write logger Time, Timezone
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

                if (selectedDevice.WriteDateTime(LoggerTime, TimeZone) == false)
                {
                    lblStatus.Visible = false;
                    MessageBox.Show(res_man.GetString("Write date and Timezone fail", cul));
                    rtxtEventLog.Text += res_man.GetString("Write date and Timezone fail", cul) + Environment.NewLine;
                    lockButton(false);
                    selectedDevice.Close();
                }
                else
                {
                    rtxtEventLog.Text += res_man.GetString("Write date and Timezone successfully", cul) + Environment.NewLine;
                }
                System.Threading.Thread.Sleep(300);

                //---------------write Location1, Description1, Serial (Serial not be updated by hardware)
                if (chboxSameDecAndLoc.Checked == true)
                {
                    selectedDevice.Location = txtLocation.Text;
                    selectedDevice.Description = txtDescription.Text;
                }
                else
                {
                    if (selectedDevice.readLocationSerialDescription() == false)
                    {
                        lblStatus.Visible = false;
                        MessageBox.Show(res_man.GetString("Read Location, Serial, Description Fail", cul));
                        lockButton(false);
                        selectedDevice.Close();
                        return;
                    }
                }

                if (selectedDevice.Write_location_serial_description() == false)
                {
                    lblStatus.Visible = false;
                    MessageBox.Show(res_man.GetString("Write Location, Description fail", cul));
                    rtxtEventLog.Text += res_man.GetString("Write Location, Description fail", cul) + Environment.NewLine;
                    lockButton(false);
                    selectedDevice.Close();
                    return;
                }
                else
                {
                    rtxtEventLog.Text += res_man.GetString("Write Location, Description successfully", cul) + Environment.NewLine;
                }
                System.Threading.Thread.Sleep(300);

                //---------------write Description Channel --------------
                if (chboxSameDescChannel.Checked == true)
                {
                    selectedDevice.channels[g_channel].Desc = txtChlDesc.Text;
                }
                else 
                {
                    if(selectedDevice.readDescriptionChannel() == false)
                    {
                        lblStatus.Visible = false;
                        MessageBox.Show(res_man.GetString("Read Description of Channel in this device fail", cul));
                        lockButton(false);
                        selectedDevice.Close();
                        return;
                    }
                }

                if (selectedDevice.Write_Description_Channel() == false)
                {
                    lblStatus.Visible = false;
                    MessageBox.Show(res_man.GetString("Write Description of Channel fail", cul));
                    rtxtEventLog.Text += res_man.GetString("Write Description of Channel fail", cul) + Environment.NewLine;
                    lockButton(false);
                    selectedDevice.Close();
                    return;
                }
                else
                {
                    rtxtEventLog.Text += res_man.GetString("Write Description of Channel successfully", cul) + Environment.NewLine;
                }
                System.Threading.Thread.Sleep(300);

                //---------------write setting (Unit, max, min, alamr, Duration, Delay)
                selectedDevice.Duration = mGlobal.duration(dldura);
                selectedDevice.Startrec = int.Parse(cbbStartDelay.Text);

                if (selectedDevice.Write_setting() == false)
                {
                    lblStatus.Visible = false;
                    rtxtEventLog.Text += res_man.GetString("Write Setting fail", cul) + Environment.NewLine;
                    MessageBox.Show(res_man.GetString("Write Setting fail", cul));
                    lockButton(false);
                    selectedDevice.Close();
                    return;
                }
                else
                {
                    if (chbAutoStart.Checked == true)
                    {
                        if (selectedDevice.StartLogging() == false)
                        {
                            lblStatus.Visible = false;
                            MessageBox.Show(res_man.GetString("Start logging fail", cul));
                            lockButton(false);
                            return;
                        }

                        if (selectedDevice.Read_DataLength() == false)//sau write
                        {
                            MessageBox.Show(res_man.GetString("Logging Status fail", cul));
                            lockButton(false);
                            return;
                        }
                        Logging = selectedDevice.byteLogging;
                        if (Logging != 0)
                        {
                            btnMultiEraseAllData.Text = res_man.GetString("Stop Logging", cul);
                        }
                        else
                        {
                            btnMultiEraseAllData.Text = res_man.GetString("Erase All Data", cul);
                        }
                    }
                    rtxtEventLog.Text += res_man.GetString("Write setting successfully", cul) + Environment.NewLine;
                    lblStatus.Visible = false;
                    lockButton(false);
                    selectedDevice.Close();
                }
            }
        }

        private void cbbChannel_SelectedIndexChanged(object sender, EventArgs e)
        {
            //-------------------------------------------------------------------
            //tmpCheckedManual = False
            txtChlDesc.Text = "";
            if(cbbChannel.SelectedItem == null)
            {
                cbbChannel.SelectedItem = cbbChannel.Items[0];
            }
            byte tmpCH = byte.Parse(cbbChannel.SelectedItem.ToString());
            if (selectedDevice.numOfChannel == 8)
            {
                if (tmpCH == 6 || tmpCH == 7 || tmpCH == 8)
                {
                    cbbUnit.Items.Clear();
                    cbbUnit.Items.Add("Not Use");
                    cbbUnit.Items.Add("Celsius");
                    cbbUnit.Items.Add("Fahrenheit");
                    cbbUnit.Items.Add("CO2");
                    cbbUnit.Items.Add("%RH");
                }
                else
                {
                    cbbUnit.Items.Clear();
                    cbbUnit.Items.Add("Not Use");
                    cbbUnit.Items.Add("Celsius");
                    cbbUnit.Items.Add("Fahrenheit");
                }
            }
            else
            {
                cbbUnit.Items.Clear();
                cbbUnit.Items.AddRange(new string[] { "Not Use", "Celsius", "Fahrenheit", "CO2", "%RH" });
            }
           
            g_channel = byte.Parse((int.Parse(cbbChannel.SelectedItem.ToString()) -1).ToString());
            g_unit = selectedDevice.channels[g_channel].Unit;
            txtChlDesc.Text = selectedDevice.channels[g_channel].Desc;


            string a = mGlobal.IntToUnit(g_unit);
            if (a == "ppm")
            {
                cbbUnit.Text = "CO2";
            }
            else
            {
                cbbUnit.Text = mGlobal.IntToUnit(g_unit);
            }
            

            if (cbbUnit.Text == "CO2")
            {
                lb_unitmax.Text = "ppm";
                lb_unitmin.Text = "ppm";
            }
            else
            {
                lb_unitmax.Text = mGlobal.IntToUnit(g_unit);
                lb_unitmin.Text = mGlobal.IntToUnit(g_unit);
            }

            if (selectedDevice.channels[g_channel].NoAlarm)
            {
                txtMaxAlarm.Text = "";
                txtMinAlarm.Text = "";
            }
            else
            {
                //if (selectedDevice.channels[g_channel].AlarmMax != 30000 && selectedDevice.channels[g_channel].AlarmMin != -30000)
                //{
                //    txtMaxAlarm.Text = selectedDevice.channels[g_channel].AlarmMax.ToString();
                //    txtMinAlarm.Text = selectedDevice.channels[g_channel].AlarmMin.ToString();
                //}
                //else
                //{
                //    txtMaxAlarm.Text = "";
                //    txtMinAlarm.Text = "";
                //}
                if (selectedDevice.channels[g_channel].AlarmMax != 30000)
                {
                    txtMaxAlarm.Text = selectedDevice.channels[g_channel].AlarmMax.ToString();
                }
                else
                {
                    txtMaxAlarm.Text = "";
                }


                if (selectedDevice.channels[g_channel].AlarmMin != -30000)
                {
                    txtMinAlarm.Text = selectedDevice.channels[g_channel].AlarmMin.ToString();
                }
                else
                {
                    txtMinAlarm.Text = "";
                }
            }

            if (selectedDevice.channels[g_channel].NoAlarm == true)
            {
                chbNoAlarm.Checked = true;
            }
            else
            {
                chbNoAlarm.Checked = false;
            }
        }

        private void txtMaxAlarm_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if(txtMaxAlarm.Text != "")
                {
                    selectedDevice.channels[g_channel].AlarmMax = int.Parse(txtMaxAlarm.Text);
                }
                else
                {
                    selectedDevice.channels[g_channel].AlarmMax = 30000;
                    selectedDevice.channels[g_channel].AlarmMin = int.Parse(txtMinAlarm.Text);
                }
            }
            catch (Exception) { }
        }

        private void txtMinAlarm_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtMinAlarm.Text != "")
                {
                    selectedDevice.channels[g_channel].AlarmMin = int.Parse(txtMinAlarm.Text);
                }
                else
                {
                    selectedDevice.channels[g_channel].AlarmMin = -30000;
                    selectedDevice.channels[g_channel].AlarmMax = int.Parse(txtMaxAlarm.Text);
                }
            }
            catch (Exception){ }
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

            try
            {
                tmpChannel = Convert.ToByte(Int32.Parse(cbbChannel.SelectedItem.ToString()) - 1);
            }
            catch
            {
                tmpChannel = 0;
            }
            if (chbNoAlarm.Checked == true)
            {
                selectedDevice.channels[tmpChannel].NoAlarm = true;
            }
            else
            {
                selectedDevice.channels[tmpChannel].NoAlarm = false;
            }
        }

        private void txtMaxAlarm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtMinAlarm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void cbbDuration_SelectedIndexChanged(object sender, EventArgs e)
        {
            int sec = 0;
            int min = 0;
            int tg = 0;
            if(cbbDuration.Text.Length == 0)
            {
                lb_interval.Text = res_man.GetString("Sample Interval:", cul) + " ";
                return;
            }
            tg = mGlobal.duration(Convert.ToInt32(cbbDuration.Text));
            if (tg > 60)
            {
                min = tg / 60;
                sec = tg % 60;
                lb_interval.Text = res_man.GetString("Sample Interval:", cul) + " " + min + " " + res_man.GetString("Min", cul) + " " + sec + " " + res_man.GetString("sec", cul) + ".";
            }
            else
            {
                lb_interval.Text = res_man.GetString("Sample Interval:", cul) + " " + tg + " " + res_man.GetString("sec", cul) + ".";
            }
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
            g_unit = mGlobal.UnitToInt(cbbUnit.SelectedItem.ToString());
            selectedDevice.channels[g_channel].Unit = g_unit;

            switch (cbbUnit.Text)
            {
                case "CO2":
                    {
                        selectedDevice.channels[g_channel].DivNum = 1;
                        lb_unitmax.Text = "ppm";
                        lb_unitmin.Text = "ppm";
                        groupBox6.Enabled = true;
                        break;
                    }
                case "Not Use":
                    {
                        txtMaxAlarm.Text = "";
                        txtMinAlarm.Text = "";
                        chbNoAlarm.Checked = true;
                        groupBox6.Enabled = false;
                        break;
                    }
                default:
                    {
                        selectedDevice.channels[g_channel].DivNum = 10;
                        lb_unitmax.Text = mGlobal.IntToUnit(g_unit);
                        lb_unitmin.Text = mGlobal.IntToUnit(g_unit);
                        groupBox6.Enabled = true;
                        break;
                    }  
            }
        }

        private void LoggerIni_FormClosed(object sender, FormClosedEventArgs e)
        {
            selectedDevice.Close();
        }

        private void chboxSameDecAndLoc_CheckedChanged(object sender, EventArgs e)
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

        private void txtChlDesc_Validating(object sender, CancelEventArgs e)
        {
            selectedDevice.channels[g_channel].Desc = txtChlDesc.Text.Trim();
        }

        private void chboxSameDescChannel_CheckedChanged(object sender, EventArgs e)
        {
            if(chboxSameDescChannel.Checked == true)
            {
                txtChlDesc.Enabled = true;
            }
            else
            {
                txtChlDesc.Enabled = false;
            }
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
