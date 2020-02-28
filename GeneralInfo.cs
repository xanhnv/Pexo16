using System;
using System.Collections;
using System.Globalization;
using System.Resources;
using System.Windows.Forms;

namespace Pexo16
{
    public partial class GeneralInfo : Form
    {
        string hostport;
        public static string interval;
        public ArrayList chDesAl = new ArrayList();
        string StrDev = "";
        Device dev_general = null;
        
        public static string[] currentval_print = new string[8];
        public static string[] unit_print = new string[8];
        public static string[] max_print = new string[8];
        public static string[] min_print = new string[8];
        public static string[] data = new string[8];

        ResourceManager res_man = new ResourceManager("Pexo16.Lang.Resources", typeof(GeneralInfo).Assembly);
        CultureInfo cul;
   

        public GeneralInfo()
        {
            InitializeComponent();
            dev_general = Device.DelInstance();
            dev_general = Device.Instance;
        }

        public GeneralInfo(string s)
        {
            hostport = s;
            InitializeComponent();
            dev_general = Device.DelInstance();
            dev_general = Device.Instance;
            dev_general.channels = new Channel[dev_general.numOfChannel];
            for (int i = 0; i < dev_general.numOfChannel; i++)
            {
                dev_general.channels[i] = new Channel();
            }
        }

        private void frmGeneralInfo_Load(object sender, EventArgs e)
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

            txtHostPort.Text = hostport;

            timer1.Enabled = true;

            btnPrint.Enabled = false;
        }

        private void btnRead_Click(object sender, EventArgs e)
        {
            chDesAl.Clear();

            btnPrint.Enabled = true;
            StrDev = txtHostPort.Text;

            System.Threading.Thread.Sleep(500);

            dev_general.USBOpen(StrDev);
            if (dev_general.readLocationSerialDescription() == false)
            {
                MessageBox.Show(res_man.GetString("Read Location, Serial, Description Fail", cul));
                return;
            }
            dev_general.Close();
            txtSerialNum.Text = dev_general.Serial;
            txtLocation.Text = dev_general.Location;
            txtDescription.Text = dev_general.Description;
               
            dev_general.USBOpen(StrDev);
            dev_general.channels = new Channel[dev_general.numOfChannel];//add in 26/02/2015
            for (int i = 0; i < dev_general.numOfChannel; i++)
            {
                dev_general.channels[i] = new Channel();
            }
            if (dev_general.readDescriptionChannel() == false)
            {
                MessageBox.Show("Read Description of channels " + dev_general.Status);
                dev_general.Close();
                this.Close();
                return;
            }
            dev_general.Close();

            for (int i = 0; i < dev_general.numOfChannel; i++)
            {
                chDesAl.Add(dev_general.channels[i].Desc);
            }
            txtDescription1.Text = chDesAl[0].ToString();
            txtDescription2.Text = chDesAl[1].ToString();
            txtDescription3.Text = chDesAl[2].ToString();
            txtDescription4.Text = chDesAl[3].ToString();
            txtDescription5.Text = chDesAl[4].ToString();
            txtDescription6.Text = chDesAl[5].ToString();
            txtDescription7.Text = chDesAl[6].ToString();
            txtDescription8.Text = chDesAl[7].ToString();

            dev_general.USBOpen(StrDev);
            if (dev_general.ReadDateTime() == false)
            {
                MessageBox.Show(res_man.GetString("Read TimeZone fail", cul));
                this.Enabled = true;
                return;
            }
            dev_general.Close();
            txtTimeSetting.Text = mGlobal.FindSystemTimeZoneFromString(dev_general.Timezone).ToString();


            dev_general.USBOpen(StrDev);
            if (dev_general.Read_setting() == false)
            {
                MessageBox.Show(res_man.GetString("Read setting fail", cul));
                return;
            }
            dev_general.Close();
            System.Threading.Thread.Sleep(1000);

            dev_general.USBOpen(StrDev);
            if (dev_general.Read_Sensor() == false)
            {
                MessageBox.Show(res_man.GetString("Read data fail", cul));
                return;
            }
            dev_general.Close();

            for (int i = 0; i < dev_general.numOfChannel; i++)
            {
                if (dev_general.channels[i].Unit != 0)
                {
                    write_txt_currentval(i, dev_general.channels[i].Val.ToString());
                    currentval_print[i] = dev_general.channels[i].Val.ToString();
                }
                else
                {
                    write_txt_currentval(i, "");
                    currentval_print[i] = "";
                }
            }

            txtDelay.Text = dev_general.Startrec + " Min(s)";
            txtDuration.Text = mGlobal.interval2duration(dev_general.Duration) + " Day(s)";
            //txtDuration.Text = dev_general.Duration + " Day(s)";


            int sec = 0;
            int min = 0;
            int tg = 0;
            tg = mGlobal.duration(dev_general.Duration);
            if (tg > 60)
            {
                min = tg / 60;
                sec = tg % 60;
                txtMeasurenment.Text = min + " Min " + sec + " Sec";
            }
            else
            {
                txtMeasurenment.Text = dev_general.Duration + " Sec";
            }
            interval = txtMeasurenment.Text;

            for (int i = 0; i < dev_general.numOfChannel; i++)
            {
                W_txt_CH(i, (i + 1).ToString());
                write_txt_unit(i, (mGlobal.IntToUnit(dev_general.channels[i].Unit)));
                if (dev_general.channels[i].Unit != 0)
                {
                    if (dev_general.channels[i].AlarmMax == 30000)
                    {
                        write_txt_max(i, "No alarm");
                    }
                    else
                    {
                        write_txt_max(i, dev_general.channels[i].AlarmMax.ToString());
                    }

                    if (dev_general.channels[i].AlarmMin == -30000)
                    {
                        write_txt_min(i, "No alarm");
                    }
                    else
                    {
                        write_txt_min(i, dev_general.channels[i].AlarmMin.ToString());
                    }
                }
                else
                {
                    write_txt_max(i, "");
                    write_txt_min(i, "");
                }

                //-------send to general info print form
                unit_print[i] = mGlobal.IntToUnit(dev_general.channels[i].Unit);
                if (dev_general.channels[i].AlarmMax == 30000)
                {
                    max_print[i] = "No alarm";
                }
                else
                {
                    max_print[i] = dev_general.channels[i].AlarmMax.ToString();
                }

                if (dev_general.channels[i].AlarmMin == -30000)
                {
                    min_print[i] = "No alarm";
                }
                else
                {
                    min_print[i] = dev_general.channels[i].AlarmMin.ToString();
                }
            }
            data[0] = txtSerialNum.Text;
            data[1] = txtLocation.Text;
            data[2] = txtDescription.Text;
            data[3] = txtMeasurenment.Text;
            data[4] = txtDuration.Text;
            data[5] = txtComputerTime.Text;
            data[6] = txtTimeSetting.Text;
            data[7] = txtDelay.Text;
        }

        public void write_txt_currentval(int i, string val)
        {
            switch (i)
            {
                case 0:
                    txtValue1.Text = val;
                    break;
                case 1:
                    txtValue2.Text = val;
                    break;
                case 2:
                    txtValue3.Text = val;
                    break;
                case 3:
                    txtValue4.Text = val;
                    break;
                case 4:
                    txtValue5.Text = val;
                    break;
                case 5:
                    txtValue6.Text = val;
                    break;
                case 6:
                    txtValue7.Text = val;
                    break;
                case 7:
                    txtValue8.Text = val;
                    break;
            }

        }

        private void W_txt_CH(int i, string val)
        {
            switch (i)
            {
                case 0:
                    txtChannel1.Text = val;
                    break;
                case 1:
                    txtChannel2.Text = val;
                    break;
                case 2:
                    txtChannel3.Text = val;
                    break;
                case 3:
                    txtChannel4.Text = val;
                    break;
                case 4:
                    txtChannel5.Text = val;
                    break;
                case 5:
                    txtChannel6.Text = val;
                    break;
                case 6:
                    txtChannel7.Text = val;
                    break;
                case 7:
                    txtChannel8.Text = val;
                    break;
            }
        }

        private void write_txt_unit(int i, string val)
        {
            switch (i)
            {
                case 0:
                    txtUnit1.Text = val;
                    break;
                case 1:
                    txtUnit2.Text = val;
                    break;
                case 2:
                    txtUnit3.Text = val;
                    break;
                case 3:
                    txtUnit4.Text = val;
                    break;
                case 4:
                    txtUnit5.Text = val;
                    break;
                case 5:
                    txtUnit6.Text = val;
                    break;
                case 6:
                    txtUnit7.Text = val;
                    break;
                case 7:
                    txtUnit8.Text = val;
                    break;
            }

        }

        private void write_txt_max(int i, string val)
        {
            switch (i)
            {
                case 0:
                    txtHighAlarm1.Text = val;
                    break;
                case 1:
                    txtHighAlarm2.Text = val;
                    break;
                case 2:
                    txtHighAlarm3.Text = val;
                    break;
                case 3:
                    txtHighAlarm4.Text = val;
                    break;
                case 4:
                    txtHighAlarm5.Text = val;
                    break;
                case 5:
                    txtHighAlarm6.Text = val;
                    break;
                case 6:
                    txtHighAlarm7.Text = val;
                    break;
                case 7:
                    txtHighAlarm8.Text = val;
                    break;
            }
        }

        private void write_txt_min(int i, string val)
        {
            switch (i)
            {
                case 0:
                    txtLowAlarm1.Text = val;
                    break;
                case 1:
                    txtLowAlarm2.Text = val;
                    break;
                case 2:
                    txtLowAlarm3.Text = val;
                    break;
                case 3:
                    txtLowAlarm4.Text = val;
                    break;
                case 4:
                    txtLowAlarm5.Text = val;
                    break;
                case 5:
                    txtLowAlarm6.Text = val;
                    break;
                case 6:
                    txtLowAlarm7.Text = val;
                    break;
                case 7:
                    txtLowAlarm8.Text = val;
                    break;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DateTime currenttime = DateTime.Now;
            txtComputerTime.Text = currenttime.ToString("MM/dd/yyyy") + "   " + currenttime.ToString("HH:mm:ss");
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            ViewInfo viewData = new ViewInfo("viewGeneral");
            viewData.Show();
        }
    }
}
