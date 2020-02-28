using System;
using System.Collections;
using System.Windows.Forms;
using System.Globalization;
using System.Resources;

namespace Pexo16
{
    public partial class GeneralInfo35 : Form
    {
        ResourceManager res_man = new ResourceManager("Pexo16.Lang.Resources", typeof(GeneralInfo35).Assembly);
        CultureInfo cul;

        string hostport;
        public static string interval;
        public ArrayList chDesAl = new ArrayList();
        string StrDev = "";
        Device35 dev_general = null;

        byte[] buf = new byte[64];

        public static string[] currentval_print = new string[8];
        public static string[] unit_print = new string[8];
        public static string[] max_print = new string[8];
        public static string[] min_print = new string[8];
        public static string[] data = new string[8];


        TextBox[] txtChannel;
        TextBox[] txtCurrentValue;
        TextBox[] txtUnit;
        TextBox[] txtHighAlarm;
        TextBox[] txtLowAlarm;
        TextBox[] txtDescription;

        public GeneralInfo35()
        {
            InitializeComponent();
            dev_general = Device35.DelInstance();
            dev_general = Device35.Instance;
        }

        public GeneralInfo35(string s)
        {
            hostport = s;
            InitializeComponent();
            dev_general = Device35.DelInstance();
            dev_general = Device35.Instance;
            dev_general.Channels = new Channel[4];
            for (int i = 0; i < 4; i++)
            {
                dev_general.Channels[i] = new Channel();
            }
        }

        private void GeneralInfo35_Load(object sender, EventArgs e)
        {
            CenterToScreen();
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

            label1.Text = res_man.GetString("Serial number", cul);
            label2.Text = res_man.GetString("Duration", cul);
            label3.Text = res_man.GetString("Logger setting Time", cul);
            label4.Text = res_man.GetString("Location", cul);
            label5.Text = res_man.GetString("Start Delay", cul);
            label6.Text = res_man.GetString("Computer Time", cul);
            label7.Text = res_man.GetString("Description", cul);
            label8.Text = res_man.GetString("Measurement Interval", cul);
            label9.Text = res_man.GetString("Chanel", cul);
            label10.Text = res_man.GetString("Current Value", cul);
            label11.Text = res_man.GetString("Unit", cul);
            label12.Text = res_man.GetString("High alarm limit", cul);
            label13.Text = res_man.GetString("Low alarm limit", cul);
            label14.Text = res_man.GetString("Channel Description", cul);
            btnRead.Text = res_man.GetString("Read", cul);
            btnPrint.Text = res_man.GetString("Print", cul);
            btnExit.Text = res_man.GetString("Exit", cul);
            label15.Text = res_man.GetString("Firmware version", cul);
            //label16.Text = res_man.GetString("Hardware version", cul);

            txtHostPort.Text = hostport;
            timer1.Enabled = true;
            btnPrint.Enabled = false;
        }

        private void btnRead_Click(object sender, EventArgs e)
        {
            chDesAl.Clear();
            btnPrint.Enabled = true;
            StrDev = txtHostPort.Text;

            dev_general.USBOpen(StrDev);

            if(!dev_general.readSerial())
            {
                MessageBox.Show(res_man.GetString("Read setting fail", cul) + "1");
                dev_general.Close();
                return;
            }
            //if (dev_general.FirmVer > 128)
            //{
            //    txtFirmWare.Text = "No Wifi _ " + (dev_general.FirmVer - 128).ToString();
            //}
            //else
            //{
            //    txtFirmWare.Text = "Wifi _ " + dev_general.FirmVer.ToString();
            //}
            //txtHardWare.Text = dev_general.HardVer.ToString();
            txtFirmWare.Text = dev_general.HardVer + "." + dev_general.FirmVer;
            txtSerialNum.Text = dev_general.Serial;

            dev_general.Close();
            dev_general.USBOpen(StrDev);

            if(!dev_general.readDescription())
            {
                MessageBox.Show(res_man.GetString("Read setting fail", cul) + "2");
                dev_general.Close();
                return;
            }
            textBox1.Text = dev_general.Description;

            dev_general.Close();
            dev_general.USBOpen(StrDev);

            if(!dev_general.readLocation())
            {
                MessageBox.Show(res_man.GetString("Read setting fail", cul) + "3");
                dev_general.Close();
                return;
            }
            txtLocation.Text = dev_general.Location;

            dev_general.Close();
            dev_general.USBOpen(StrDev);

            if(!dev_general.readSettingDevice())
            {
                MessageBox.Show(res_man.GetString("Read setting fail", cul) + "4");
                dev_general.Close();
                return;
            }
            txtTimeSetting.Text = dev_general.SettingTime.ToLongTimeString();
            txtDelay.Text = dev_general.Delay.ToString() + " Min(s)";
            txtDuration.Text = dev_general.Duration.ToString() + " Day(s)";

            if(!dev_general.readSettingChannel())
            {
                MessageBox.Show(res_man.GetString("Read setting fail", cul) + "5");
                dev_general.Close();
                return;
            }

            this.Refresh();

            if (!dev_general.readDataProbe(ref buf))
            {
                //MessageBox.Show(res_man.GetString("Read data fail", cul));
                dev_general.Close();
                //return;
            }

            dev_general.Close();

            int num = 0;
            for (int i = 0; i < 4; i++)
            {
                if (dev_general.Channels[i].Sensor == 0)
                {
                    num += 0;
                }
                else if (dev_general.Channels[i].Sensor == 3)
                {
                    num += 3;
                }
                else
                {
                    num += 1;
                }
            }

            int sec = 0;
            int min = 0;
            int tg = 0;
            int x = 0;
            if (num != 0)
            {
                x = (128 * 1024) / num;
            }
            else
            {
                x = 128 * 1024;
            }

            tg = mGlobal.duration35(Convert.ToInt32(dev_general.Duration), x);
            dev_general.Interval = tg.ToString();
            if (tg > 60)
            {
                min = tg / 60;
                sec = tg % 60;
                txtMeasurenment.Text =  min + " min(s) " + sec + " sec(s).";
            }
            else
            {
                txtMeasurenment.Text =  tg + " sec(s).";
            }

            txtChannel = new TextBox[num];
            txtCurrentValue = new TextBox[num];
            txtUnit = new TextBox[num];
            txtHighAlarm = new TextBox[num];
            txtLowAlarm = new TextBox[num];
            txtDescription = new TextBox[num];

            //declare
            for (int i = 0; i < num; i++)
            {
                txtChannel[i] = new TextBox();
                txtChannel[i].Name = "txtChannel" + i.ToString();
                txtChannel[i].Width = 80;
                txtChannel[i].Top = (int)(220 +  1.5 * i * txtChannel[i].Height);
                txtChannel[i].Left = 20;
                txtChannel[i].TextAlign = HorizontalAlignment.Center;
                this.groupBox1.Controls.Add(txtChannel[i]);

                txtUnit[i] = new TextBox();
                txtUnit[i].Name = "txtUnit" + i.ToString();
                txtUnit[i].Width = 90;
                txtUnit[i].Top = (int)(220 + 1.5 * i * txtChannel[i].Height);
                txtUnit[i].Left = 140;
                txtUnit[i].TextAlign = HorizontalAlignment.Center;
                this.groupBox1.Controls.Add(txtUnit[i]);

                txtCurrentValue[i] = new TextBox();
                txtCurrentValue[i].Name = "txtValue" + i.ToString();
                txtCurrentValue[i].Width = 110;
                txtCurrentValue[i].Top = (int)(220 + 1.5 * i * txtChannel[i].Height);
                txtCurrentValue[i].Left = 260;
                txtCurrentValue[i].TextAlign = HorizontalAlignment.Center;
                this.groupBox1.Controls.Add(txtCurrentValue[i]);

                txtHighAlarm[i] = new TextBox();
                txtHighAlarm[i].Name = "txtHigh" + i.ToString();
                txtHighAlarm[i].Width = 130;
                txtHighAlarm[i].Top = (int)(220 + 1.5 * i * txtChannel[i].Height);
                txtHighAlarm[i].Left = 410;
                txtHighAlarm[i].TextAlign = HorizontalAlignment.Center;
                this.groupBox1.Controls.Add(txtHighAlarm[i]);

                txtLowAlarm[i] = new TextBox();
                txtLowAlarm[i].Name = "txtLow" + i.ToString();
                txtLowAlarm[i].Width = 130;
                txtLowAlarm[i].Top = (int)(220 + 1.5 * i * txtChannel[i].Height);
                txtLowAlarm[i].Left = 580;
                txtLowAlarm[i].TextAlign = HorizontalAlignment.Center;
                this.groupBox1.Controls.Add(txtLowAlarm[i]);


                txtDescription[i] = new TextBox();
                txtDescription[i].Name = "txtDescription" + i.ToString();
                txtDescription[i].Width = 200;
                txtDescription[i].Top = (int)(220 + 1.5 * i * txtChannel[i].Height);
                txtDescription[i].Left = 760;
                txtDescription[i].TextAlign = HorizontalAlignment.Center;
                this.groupBox1.Controls.Add(txtDescription[i]);
            }


            addData();
            ////add data
            //int channel = 0;
           
            //for (int i = 0; i < 4; i++)
            //{
            //    int tmpDiv;
            //    if (dev_general.Channels[i].Unit == 175 || dev_general.Channels[i].Unit == 172 || dev_general.Channels[i].Sensor == 2) 
            //    {
            //        tmpDiv = 10;
            //    }
            //    else if (dev_general.Channels[i].Sensor == 3)
            //    {
            //        tmpDiv = 1000;
            //    }
            //    else
            //    {
            //        tmpDiv = 1;
            //    }
            //    if (dev_general.Channels[i].Sensor == 3)
            //    {
            //        int dem = 0;
            //        for (int j = 0; j < 3; j++)
            //        {
            //            switch (dem)
            //            {
            //                case 0:
            //                    txtChannel[channel].Text = (i + 1).ToString() + "(X)";
            //                    txtUnit[channel].Text = mGlobal.IntToUnit35(dev_general.Channels[i].Sensor);
            //                    txtHighAlarm[channel].Text = res_man.GetString("No Alarm",cul);
            //                    txtLowAlarm[channel].Text = res_man.GetString("No Alarm", cul);
            //                    txtDescription[channel].Text = dev_general.Channels[i].Desc;
            //                    txtCurrentValue[channel].Text = (mGlobal.get_temp(buf[3 + 7 * i], buf[4 + 7 * i])/tmpDiv).ToString();
            //                    channel += 1;
            //                    dem += 1;
            //                    break;

            //                case 1:
            //                    txtChannel[channel].Text = (i + 1).ToString() + "(Y)";
            //                    txtUnit[channel].Text = mGlobal.IntToUnit35(dev_general.Channels[i].Sensor);
            //                    txtHighAlarm[channel].Text = res_man.GetString("No Alarm", cul);
            //                    txtLowAlarm[channel].Text = res_man.GetString("No Alarm", cul);
            //                    txtDescription[channel].Text = dev_general.Channels[i].Desc;
            //                    txtCurrentValue[channel].Text = (mGlobal.get_temp(buf[5 + 7 * i], buf[6 + 7 * i]) / tmpDiv).ToString();
            //                    channel += 1;
            //                    dem += 1;
            //                    break;

            //                case 2:
            //                    txtChannel[channel].Text = (i + 1).ToString() + "(Z)";
            //                    txtUnit[channel].Text = mGlobal.IntToUnit35(dev_general.Channels[i].Sensor);
            //                    txtHighAlarm[channel].Text = res_man.GetString("No Alarm", cul);
            //                    txtLowAlarm[channel].Text = res_man.GetString("No Alarm", cul);
            //                    txtDescription[channel].Text = dev_general.Channels[i].Desc;
            //                    txtCurrentValue[channel].Text = (mGlobal.get_temp(buf[7 + 7 * i], buf[8 + 7 * i]) / tmpDiv).ToString();
            //                    channel += 1;
            //                    dem += 1;
            //                    break;
            //            }
            //        }
            //    }
            //    else if (dev_general.Channels[i].Sensor == 1)
            //    {
            //        txtChannel[channel].Text = (i + 1).ToString();
            //        txtUnit[channel].Text = mGlobal.IntToUnit35(dev_general.Channels[i].Unit);
            //        if (dev_general.Channels[i].NoAlarm)
            //        {
            //            txtHighAlarm[channel].Text = res_man.GetString("No Alarm", cul);
            //            txtLowAlarm[channel].Text = res_man.GetString("No Alarm", cul);
            //        }
            //        else
            //        {
            //            txtHighAlarm[channel].Text = (dev_general.Channels[i].AlarmMax / 10).ToString();
            //            txtLowAlarm[channel].Text = (dev_general.Channels[i].AlarmMin / 10).ToString();
            //        }
            //        txtDescription[channel].Text = dev_general.Channels[i].Desc;
            //        txtCurrentValue[channel].Text = (mGlobal.get_temp(buf[3 + 7 * i], buf[4 + 7 * i]) / tmpDiv).ToString();
            //        channel += 1;
            //    }
            //    else if (dev_general.Channels[i].Sensor == 2)
            //    {
            //        txtChannel[channel].Text = (i + 1).ToString();
            //        txtUnit[channel].Text = mGlobal.IntToUnit35(dev_general.Channels[i].Sensor);
            //        if (dev_general.Channels[i].NoAlarm)
            //        {
            //            txtHighAlarm[channel].Text = res_man.GetString("No Alarm", cul);
            //            txtLowAlarm[channel].Text = res_man.GetString("No Alarm", cul);
            //        }
            //        else
            //        {
            //            txtHighAlarm[channel].Text = (dev_general.Channels[i].AlarmMax / 10).ToString();
            //            txtLowAlarm[channel].Text = (dev_general.Channels[i].AlarmMin / 10).ToString();
            //        }
            //        txtDescription[channel].Text = dev_general.Channels[i].Desc;
            //        txtCurrentValue[channel].Text = (mGlobal.get_temp(buf[3 + 7 * i], buf[4 + 7 * i]) / tmpDiv).ToString();
            //        channel += 1;
            //    }
            //    else if(dev_general.Channels[i].Sensor == 4)
            //    {
            //        txtChannel[channel].Text = (i + 1).ToString();
            //        txtUnit[channel].Text = mGlobal.IntToUnit35(dev_general.Channels[i].Sensor);
            //        txtHighAlarm[channel].Text = res_man.GetString("No Alarm", cul);
            //        txtLowAlarm[channel].Text = res_man.GetString("No Alarm", cul);
            //        txtDescription[channel].Text = dev_general.Channels[i].Desc;
            //        txtCurrentValue[channel].Text = (mGlobal.get_temp(buf[3 + 7 * i], buf[4 + 7 * i]) / tmpDiv).ToString();
            //        channel += 1;
            //    }
            //}
        }

        private void addData()
        {
            int channel = 0;

            for (int i = 0; i < 4; i++)
            {
                int tmpDiv;
                if (dev_general.Channels[i].Unit == 175 || dev_general.Channels[i].Unit == 172 || dev_general.Channels[i].Sensor == 2)
                {
                    tmpDiv = 10;
                }
                else if (dev_general.Channels[i].Sensor == 3)
                {
                    tmpDiv = 1000;
                }
                else
                {
                    tmpDiv = 1;
                }
                if (dev_general.Channels[i].Sensor == 3)
                {
                    int dem = 0;
                    for (int j = 0; j < 3; j++)
                    {
                        switch (dem)
                        {
                            case 0:
                                txtChannel[channel].Text = (i + 1).ToString() + "(X)";
                                txtUnit[channel].Text = mGlobal.IntToUnit35(dev_general.Channels[i].Sensor);
                                txtHighAlarm[channel].Text = res_man.GetString("No Alarm", cul);
                                txtLowAlarm[channel].Text = res_man.GetString("No Alarm", cul);
                                txtDescription[channel].Text = dev_general.Channels[i].Desc;
                                txtCurrentValue[channel].Text = (mGlobal.get_temp(buf[3 + 7 * i], buf[4 + 7 * i]) / tmpDiv).ToString();
                                txtCurrentValue[channel].Refresh();
                                channel += 1;
                                dem += 1;
                                break;

                            case 1:
                                txtChannel[channel].Text = (i + 1).ToString() + "(Y)";
                                txtUnit[channel].Text = mGlobal.IntToUnit35(dev_general.Channels[i].Sensor);
                                txtHighAlarm[channel].Text = res_man.GetString("No Alarm", cul);
                                txtLowAlarm[channel].Text = res_man.GetString("No Alarm", cul);
                                txtDescription[channel].Text = dev_general.Channels[i].Desc;
                                txtCurrentValue[channel].Text = (mGlobal.get_temp(buf[5 + 7 * i], buf[6 + 7 * i]) / tmpDiv).ToString();
                                channel += 1;
                                dem += 1;
                                break;

                            case 2:
                                txtChannel[channel].Text = (i + 1).ToString() + "(Z)";
                                txtUnit[channel].Text = mGlobal.IntToUnit35(dev_general.Channels[i].Sensor);
                                txtHighAlarm[channel].Text = res_man.GetString("No Alarm", cul);
                                txtLowAlarm[channel].Text = res_man.GetString("No Alarm", cul);
                                txtDescription[channel].Text = dev_general.Channels[i].Desc;
                                txtCurrentValue[channel].Text = (mGlobal.get_temp(buf[7 + 7 * i], buf[8 + 7 * i]) / tmpDiv).ToString();
                                channel += 1;
                                dem += 1;
                                break;
                        }
                    }
                }
                else if (dev_general.Channels[i].Sensor == 1)
                {
                    txtChannel[channel].Text = (i + 1).ToString();
                    txtUnit[channel].Text = mGlobal.IntToUnit35(dev_general.Channels[i].Unit);
                    if (dev_general.Channels[i].NoAlarm)
                    {
                        txtHighAlarm[channel].Text = res_man.GetString("No Alarm", cul);
                        txtLowAlarm[channel].Text = res_man.GetString("No Alarm", cul);
                    }
                    else
                    {
                        txtHighAlarm[channel].Text = (dev_general.Channels[i].AlarmMax / 10).ToString();
                        txtLowAlarm[channel].Text = (dev_general.Channels[i].AlarmMin / 10).ToString();
                    }
                    txtDescription[channel].Text = dev_general.Channels[i].Desc;
                    txtCurrentValue[channel].Text = (mGlobal.get_temp(buf[3 + 7 * i], buf[4 + 7 * i]) / tmpDiv).ToString();
                    channel += 1;
                }
                else if (dev_general.Channels[i].Sensor == 2)
                {
                    txtChannel[channel].Text = (i + 1).ToString();
                    txtUnit[channel].Text = mGlobal.IntToUnit35(dev_general.Channels[i].Sensor);
                    if (dev_general.Channels[i].NoAlarm)
                    {
                        txtHighAlarm[channel].Text = res_man.GetString("No Alarm", cul);
                        txtLowAlarm[channel].Text = res_man.GetString("No Alarm", cul);
                    }
                    else
                    {
                        txtHighAlarm[channel].Text = (dev_general.Channels[i].AlarmMax / 10).ToString();
                        txtLowAlarm[channel].Text = (dev_general.Channels[i].AlarmMin / 10).ToString();
                    }
                    txtDescription[channel].Text = dev_general.Channels[i].Desc;
                    txtCurrentValue[channel].Text = (mGlobal.get_temp(buf[3 + 7 * i], buf[4 + 7 * i]) / tmpDiv).ToString();
                    channel += 1;
                }
                else if (dev_general.Channels[i].Sensor == 4)
                {
                    txtChannel[channel].Text = (i + 1).ToString();
                    txtUnit[channel].Text = mGlobal.IntToUnit35(dev_general.Channels[i].Sensor);
                    txtHighAlarm[channel].Text = res_man.GetString("No Alarm", cul);
                    txtLowAlarm[channel].Text = res_man.GetString("No Alarm", cul);
                    txtDescription[channel].Text = dev_general.Channels[i].Desc;
                    txtCurrentValue[channel].Text = (mGlobal.get_temp(buf[3 + 7 * i], buf[4 + 7 * i]) / tmpDiv).ToString();
                    channel += 1;
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DateTime currenttime = DateTime.Now;
            txtComputerTime.Text = currenttime.ToString("MM/dd/yyyy") + "   " + currenttime.ToString("HH:mm:ss");

            dev_general.USBOpen(StrDev);

            if (!dev_general.readDataProbe(ref buf))
            {
                dev_general.Close();
            }

            dev_general.Close();
            addData();
            
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            ViewInfo viewData = new ViewInfo("viewGeneral35", buf);
            viewData.Show();
        }

        private void txtHostPort_TextChanged(object sender, EventArgs e)
        {

        }

    }
}
