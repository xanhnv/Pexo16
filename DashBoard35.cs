using System;
using System.Drawing;
using System.Windows.Forms;

namespace Pexo16
{
    public partial class DashBoard35 : Form
    {

        Device35 dev_dashboard35 = null;
        string[,] arrUnit = new string[100, 4];
        double[,] arrMax = new double[100, 4];
        double[,] arrMin = new double[100, 4];
        private bool viewGraph = false;
        private int pre_index;
        //double max;
        //double min;

        public DashBoard35()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            string str_device = "";
            int numofChannel = 0;
            int count = 0;
            
                for (int i = 0; i < getDeviceInfo.activeDeviceListAl.Count; i++)
                {
                    dev_dashboard35 = Device35.DelInstance();
                    dev_dashboard35 = Device35.Instance;
                    str_device = getDeviceInfo.activeDeviceListAl[i].ToString();
                    if (getDeviceInfo.activeDeviceListAl[i].ToString().Contains("Pexo35 Custm HID"))
                    {
                    
                        dev_dashboard35.USBOpen(str_device);

                        dev_dashboard35.readLocation();
                        dev_dashboard35.readDescription();
                        dev_dashboard35.readSerial();
                        dev_dashboard35.readSettingDevice();

                        dev_dashboard35.Channels = new Channel[4];
                        for (int k = 0; k < 4; k++)
                        {
                            dev_dashboard35.Channels[k] = new Channel();
                        }
                        dev_dashboard35.readSettingChannel();


                        for (int j = 0; j < 4; j++)
                        {
                            if (dev_dashboard35.Channels[j].Sensor == 3)
                            {
                                numofChannel += 3;
                            }
                            else if (dev_dashboard35.Channels[j].Sensor != 0)
                            {
                                numofChannel += 1;
                            }
                        }

                    
                        for (int j = 0; j < 4; j++)
                        {
                            if (dev_dashboard35.byteLogging == 0x44)
                            {
                                arrUnit[i, j] = mGlobal.IntToUnit_Dashboard35(dev_dashboard35.Channels[j].Unit, dev_dashboard35.Channels[j].Sensor);
                            }
                            else
                            {
                                arrUnit[i, j] = "--";
                            }
                            if (!dev_dashboard35.Channels[j].NoAlarm)
                            {
                                arrMax[i, j] = (double)dev_dashboard35.Channels[j].AlarmMax / 10.0;
                                arrMin[i, j] = (double)dev_dashboard35.Channels[j].AlarmMin / 10.0;
                            }
                            else
                            {
                                arrMax[i, j] = 65535;
                                arrMin[i, j] = 65535;
                            }
                        }

                   
                        dataGridView1.Rows.Add("", "");
                        dataGridView1.Rows[count].Cells[0].Value = count.ToString();
                        dataGridView1.Rows[count].Cells[1].Value = getDeviceInfo.activeDeviceListAl[i].ToString();
                        dataGridView1.Rows[count].Cells[2].Value = dev_dashboard35.Location;
                        dataGridView1.Rows[count].Cells[3].Value = dev_dashboard35.Description;
                        dataGridView1.Rows[count].Cells[4].Value = dev_dashboard35.Serial;

                        count += 1;

                        dev_dashboard35.Close();
                    }
                }
            if (btnStart.Text == "Run")
            {
                btnStart.Text = "Stop";
                timer1.Enabled = true;
            }
            else
            {
                btnStart.Text = "Run";
                timer1.Enabled = false;
            }

            if (count == 0)
            {
                btnGraph.Enabled = false;
                btnStart.Enabled = false;
            }

            
        }

        private void btnGraph_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;

            if (dev_dashboard35 != null)
            {
                dev_dashboard35.Close();
            }
           
            getDeviceInfo.getActiveDevice();
            DashBoardSelect realTimeSelect = new DashBoardSelect(1);
            realTimeSelect.FormClosed += new FormClosedEventHandler(close);
            this.Hide();
            realTimeSelect.ShowDialog();

            //this.Close();
        }

        private void close(object sender, FormClosedEventArgs e)
        {
            this.Close();
        }

        private void DashBoard35_Load(object sender, EventArgs e)
        {
            getDeviceInfo.getActiveDevice();

            timer1.Enabled = false;

            btnStart.Text = "Run";

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            int count = 0;
            if (viewGraph == true)
            {
                btnStart.Enabled = false;
                btnGraph.Enabled = false;
                return;
            }

            for (int i = 0; i < getDeviceInfo.activeDeviceListAl.Count; i++)
            {
                if (getDeviceInfo.activeDeviceListAl[i].ToString().Contains("Pexo35 Custm HID"))
                {        
                    string[] show = new string[4];
                    float[] CH = new float[dev_dashboard35.numOfChannel];
                    string StrDevs = getDeviceInfo.activeDeviceListAl[i].ToString();

                    if (dev_dashboard35.USBOpen(StrDevs))
                    {
                        byte[] buf = new byte[64];

                        dev_dashboard35.readDataProbe(ref buf);
                        //dev_dashboard35.readSettingChannel();

                        dev_dashboard35.Close();

                        DateTime _time = DateTime.Now;
                       
                        string tmpStr = "";
                        for (int j = 0; j < 4; j++)
                        {
                             string hexSensor = buf[2 + j * 7].ToString("X");
                             if (hexSensor != "0")
                             {
                                 if (hexSensor.Substring(1, 1) == "3")
                                 {
                                     for (int k = 0; k < 3; k++)
                                     {
                                         show[j] += mGlobal.format_numDB35(mGlobal.get_temp(buf[3 + j * 7 + 2 * k], buf[3 + j * 7 + 2 * k + 1]) / 1000.0).ToString() + "  |  ";
                                     }

                                     show[j] = show[j].Substring(0, show[j].Length - 3) + "  (" + arrUnit[i, j] + ")";
                                 }
                                 else if (hexSensor.Substring(1, 1) == "1" || hexSensor.Substring(1, 1) == "2")
                                 {
                                     show[j] = (mGlobal.get_temp(buf[3 + j * 7], buf[3 + j * 7 + 1]) / 10.0).ToString() + "  (" + arrUnit[i, j] + ")";
                                 }
                                 else
                                 {
                                     show[j] = mGlobal.get_temp(buf[3 + j * 7], buf[3 + j * 7 + 1]).ToString() + "  (" + arrUnit[i, j] + ")";
                                 }
                             }
                            
                            //show[j] = mGlobal.format_num(dev_dashboard35.Channels[j].Val) + " " + arrUnit[i, j];
                            tmpStr += show[j];
                            if (arrUnit[i, j] == "--")
                            {
                                show[j] = "---";
                            }
                        }

                        for (int k = 5; k < 5 + 4; k++)
                        {
                            dataGridView1.Rows[count].Cells[k].Value = show[k - 5];
                            if (show[k - 5] != "---"  && show[k - 5] != null)
                            {
                                //if (dev_dashboard35.Channels[k - 5].Sensor == 1 || dev_dashboard35.Channels[k - 5].Sensor == 2)
                                //{
                                    if (arrMin[i, k-5] != 65535 && arrMax[i, k -5] != 65535)
                                    {
                                        //double tampMax = (double)dev_dashboard35.Channels[k - 5].AlarmMax / 10.0;
                                        //double tampMin = (double)dev_dashboard35.Channels[k - 5].AlarmMin / 10.0;
                                        if (double.Parse(show[k - 5].Substring(0, show[k - 5].IndexOf(" "))) >= arrMax[i, k-5])
                                        {
                                            dataGridView1.Rows[count].Cells[k].Style.ForeColor = Color.Red;
                                        }
                                        else if (double.Parse(show[k - 5].Substring(0, show[k - 5].IndexOf(" "))) <= arrMin[i, k- 5])
                                        {
                                            dataGridView1.Rows[count].Cells[k].Style.ForeColor = Color.Blue;
                                        }
                                        else
                                        {
                                            dataGridView1.Rows[count].Cells[k].Style.ForeColor = Color.Black;
                                        }
                                    }
                                //}
                            }
                            
                        }
                        
                        
                    }
                    else
                    {
                        dataGridView1.Rows[count].Cells[4].Value = "Connecting";
                        dataGridView1.Rows[count].Cells[1].Value = getDeviceInfo.activeDeviceListAl[i].ToString();
                    }
                    count += 1;
                }
            }
        }

        private void DashBoard35_Resize(object sender, EventArgs e)
        {
            dataGridView1.Width = this.Width - 50;
            dataGridView1.Height = this.Height - 170;
            //dataGridView1.AutoResizeColumn(5);
            //dataGridView1.AutoSize = true;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }

       
    }
}
