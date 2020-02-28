using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Text;
using System.Windows.Forms;

namespace Pexo16
{
    public partial class DashBoard : Form
    {
        int tyleDevice = 0;
        //Device dev_dashboard = Device.Instance;
        Device dev_dashboard = null;
        Device dev_dashboard_8S = null;
        Device dev_dashboard_4S = null;
        string[,] arrUnit = new string[100, 10];
        private bool viewGraph = false;
        private int pre_index;
        public bool timer_graph;

        ResourceManager res_man = new ResourceManager("Pexo16.Lang.Resources", typeof(DashBoard).Assembly);
        CultureInfo cul;

        // MainUI main = new MainUI();
        public DashBoard(MainUI frmMain)
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if(dev_dashboard == null)
            {
                return;
            }
            getDeviceInfo.getActiveDevice();
            if (timerDashBoard.Enabled == true)
            {
                btnStart.Text = "Run";

                dataGridView1.Rows.Clear();
                //if(dataGridView1.Rows.Count < main.perfectMenuAL.Count)
                if (dataGridView1.Rows.Count < getDeviceInfo.activeDeviceListAl.Count)
                {
                    for (int i = 0; i < getDeviceInfo.activeDeviceListAl.Count; i++)
                    {
                        dataGridView1.Rows.Add("", "");
                    }
                }
                timerDashBoard.Enabled = false;
                dev_dashboard.Close();
            }
            else
            {
                btnStart.Text = "Stop";
                timerDashBoard.Enabled = true;
            }
        }

        private void frmDashBoard_Load(object sender, EventArgs e)
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

            CenterToScreen();
            getDeviceInfo.getActiveDevice();

            int x = Screen.PrimaryScreen.Bounds.Width;
            int y = Screen.PrimaryScreen.Bounds.Height;

            this.Left = 20;
            this.Width = x - 50;
            this.Height = dataGridView1.Height + 200;
            dataGridView1.Left = 30;
            dataGridView1.Width = this.Width - 60;

            timerDashBoard.Enabled = false;
            btnStart.Text = "Run";

            string str_device = "";
            int count = 0;

            //for (int i = 0; i < mGlobal.perfectItemAL.Count; i++)
            for (int i = 0; i < getDeviceInfo.activeDeviceListAl.Count; i++)
            {
                dev_dashboard = Device.DelInstance();
                dev_dashboard = Device.Instance;
                str_device = getDeviceInfo.activeDeviceListAl[i].ToString();
                dev_dashboard.USBOpen(str_device);// numOfChannel(lay tu Serial)
                dev_dashboard.readLocationSerialDescription();//09/02/2015 (numOfChannel lay tu device)
                dev_dashboard.channels = new Channel[8];
                //dev_dashboard.Channels = new Channel[dev_dashboard.numOfChannel];
                //for (int j = 0; j < dev_dashboard.numOfChannel; j++)
                for (int j = 0; j < 8; j++)
                {
                    dev_dashboard.channels[j] = new Channel();
                }

                dev_dashboard.Read_setting();

                for (int j = 0; j < dev_dashboard.numOfChannel; j++)
                {
                    if (getDeviceInfo.nhanDang(dev_dashboard.dev) != "PEXO-35" && getDeviceInfo.nhanDang34(dev_dashboard.dev) != "PEXO-34")
                    {
                        count += 1;
                        arrUnit[i, j] = mGlobal.IntToUnit_Dashboard(dev_dashboard.channels[j].Unit);
                    }
                }
                dev_dashboard.Close();
            }

            //add row datagridview
            //if (dataGridView1.Rows.Count < mGlobal.perfectItemAL.Count)
            if (dataGridView1.Rows.Count < getDeviceInfo.activeDeviceListAl.Count)
            {
                //for (int i = 0; i < mGlobal.perfectItemAL.Count; i++)
                for (int i = 0; i < getDeviceInfo.activeDeviceListAl.Count; i++)
                {
                    dataGridView1.Rows.Add("", "");
                }
            }

            if(count == 0)
            {
                btnGraph.Enabled = false;
                btnStart.Enabled = false;
            }
        }

        private void timerDashBoard_Tick(object sender, EventArgs e)
        {
            string[] show = new string[8];

            if (viewGraph == true)
            {
                btnStart.Enabled = false;
                btnGraph.Enabled = false;
                return;
            }

            for (int i = 0; i < getDeviceInfo.activeDeviceListAl.Count; i++)
            {
                if (getDeviceInfo.activeDeviceListAl[i].ToString().IndexOf("VID") >= 0)
                {
                    float[] CH = new float[dev_dashboard.numOfChannel];
                    string StrDevs = getDeviceInfo.activeDeviceListAl[i].ToString();

                    if (dev_dashboard.USBOpen(StrDevs))
                    {
                        dev_dashboard.Read_Sensor();
                        dev_dashboard.readLocationSerialDescription();
                        dev_dashboard.Read_setting();
                       //dev_dashboard.Read

                        DateTime _time = DateTime.Now;
                        if (dev_dashboard.ss_index != pre_index)//khong hieu
                        {
                            pre_index = dev_dashboard.ss_index;
                            string tmpStr = "";
                            for (int j = 0; j < dev_dashboard.numOfChannel; j++)
                            {
                                show[j] = mGlobal.format_num(dev_dashboard.channels[j].Val) + " " + arrUnit[i, j];
                                tmpStr += show[j];
                                if (arrUnit[i, j] == "-")
                                {
                                    show[j] = "---";
                                }
                            }

                            dataGridView1.Rows[i].Cells[0].Value = getDeviceInfo.activeDeviceListAl[i].ToString();
                            dataGridView1.Rows[i].Cells[1].Value = dev_dashboard.Location;
                            dataGridView1.Rows[i].Cells[2].Value = dev_dashboard.Description;
                            dataGridView1.Rows[i].Cells[3].Value = dev_dashboard.Serial;

                            for (int k = 4; k < 4 + dev_dashboard.numOfChannel; k++)
                            {
                                dataGridView1.Rows[i].Cells[k].Value = show[k - 4];
                            }
                        }
                        dev_dashboard.Close();
                    }
                    else
                    {
                        dataGridView1.Rows[i].Cells[4].Value = "Connecting";
                        dataGridView1.Rows[i].Cells[0].Value = getDeviceInfo.activeDeviceListAl[i].ToString();
                    }
                }
            }
        }

        private void frmDashBoard_SizeChanged(object sender, EventArgs e)
        {
            dataGridView1.Left = 30;
            dataGridView1.Width = this.Width - 60;
            dataGridView1.Height = this.Height - 200;

            groupBox1.Location = new Point((this.Width - groupBox1.Width) / 2, 25);
            dataGridView1.Location = new Point((this.Width - dataGridView1.Width) / 2, 150);
        }

        private void btnGraph_Click(object sender, EventArgs e)
        {
            if (btnStart.Text == "Stop")
            {
                btnStart_Click(null, null);
            }
            DashBoardSelect dbSelect = new DashBoardSelect(tyleDevice);
            dbSelect.ShowDialog();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            MessageBox.Show(dataGridView1.CurrentCell.Value.ToString());
        }

        private void frmDashBoard_FormClosed(object sender, FormClosedEventArgs e)
        {
            dataGridView1.Rows.Clear();
            timerDashBoard.Enabled = false;
            try
            {
                timer_graph = false;
            }
            catch (Exception)
            { }
        }


       // //Device dev_dashboard = Device.Instance;
       // Device dev_dashboard = null;
       // Device dev_dashboard_8S = null;
       // Device dev_dashboard_4S = null;
       // string[,] arrUnit = new string[100, 10];
       // private bool viewGraph = false;
       // private int pre_index;
       // public bool timer_graph;

       //// MainUI main = new MainUI();
       // public DashBoard(MainUI frmMain)
       // {
       //     InitializeComponent();
       //     //main = frmMain;
       // }

       // private void btnStart_Click(object sender, EventArgs e)
       // {
       //     //main.updateActiveMenu();
       //     getDeviceInfo.getActiveDevice();
       //     if(timerDashBoard.Enabled == true)
       //     {
       //         btnStart.Text = "Run";

       //         dataGridView1.Rows.Clear();
       //         //if(dataGridView1.Rows.Count < main.perfectMenuAL.Count)
       //         if (dataGridView1.Rows.Count < getDeviceInfo.activeDeviceListAl.Count)
       //         {
       //             for (int i = 0; i < getDeviceInfo.activeDeviceListAl.Count; i++)
       //             {
       //                 dataGridView1.Rows.Add("", "");
       //             }
       //         }
       //         timerDashBoard.Enabled = false;
       //         dev_dashboard.Close();
       //     }
       //     else
       //     {
       //         btnStart.Text = "Stop";
       //         timerDashBoard.Enabled = true;
       //     }
       // }

       // private void frmDashBoard_Load(object sender, EventArgs e)
       // {
       //     CenterToScreen();
       //     //main.updateActiveMenu();
       //     getDeviceInfo.getActiveDevice();

       //     int x = Screen.PrimaryScreen.Bounds.Width;
       //     int y = Screen.PrimaryScreen.Bounds.Height;

       //     this.Width = x - 50;
       //     this.Height = dataGridView1.Height + 200;
       //     dataGridView1.Left = 30;
       //     dataGridView1.Width = this.Width - 60;

       //     timerDashBoard.Enabled = false;
       //     btnStart.Text = "Run";

       //     string str_device = "";

       //     //for (int i = 0; i < mGlobal.perfectItemAL.Count; i++)
       //     for (int i = 0; i < getDeviceInfo.activeDeviceListAl.Count; i++)
       //     {
       //         dev_dashboard = Device.DelInstance();
       //         dev_dashboard = Device.Instance;
       //         str_device = getDeviceInfo.activeDeviceListAl[i].ToString();
       //         dev_dashboard.USBOpen(str_device);// numOfChannel(lay tu Serial)
       //         dev_dashboard.readLocationSerialDescription();//09/02/2015 (numOfChannel lay tu device)
       //         dev_dashboard.channels = new Channel[8];
       //         //dev_dashboard.Channels = new Channel[dev_dashboard.numOfChannel];
       //         //for (int j = 0; j < dev_dashboard.numOfChannel; j++)
       //         for (int j = 0; j < 8; j++)
       //         {
       //             dev_dashboard.channels[j] = new Channel();
       //         }

       //         dev_dashboard.Read_setting();

       //         for (int j = 0; j < dev_dashboard.numOfChannel; j++)
       //         {
       //             arrUnit[i, j] = mGlobal.IntToUnit_Dashboard(dev_dashboard.channels[j].Unit);
       //         }
       //         dev_dashboard.Close();
       //     }

       //     //add row datagridview
       //     //if (dataGridView1.Rows.Count < mGlobal.perfectItemAL.Count)
       //     if (dataGridView1.Rows.Count < getDeviceInfo.activeDeviceListAl.Count)
       //     {
       //         //for (int i = 0; i < mGlobal.perfectItemAL.Count; i++)
       //         for (int i = 0; i < getDeviceInfo.activeDeviceListAl.Count; i++)
       //         {
       //             dataGridView1.Rows.Add("", "");
       //         }
       //     }
       // }

       // private void timerDashBoard_Tick(object sender, EventArgs e)
       // {
       //     string[] show = new string[8];

       //     if(viewGraph == true)
       //     {
       //         btnStart.Enabled = false;
       //         btnGraph.Enabled = false;
       //         return;
       //     }

       //     for ( int i = 0; i < getDeviceInfo.activeDeviceListAl.Count; i++)
       //     {
       //         if (getDeviceInfo.activeDeviceListAl[i].ToString().IndexOf("VID") >= 0)
       //         {
       //             float[] CH = new float[dev_dashboard.numOfChannel];
       //             string StrDevs = getDeviceInfo.activeDeviceListAl[i].ToString();

       //             if (dev_dashboard.USBOpen(StrDevs))
       //             {

                       
       //                 dev_dashboard.Read_Sensor();
       //                 dev_dashboard.readLocationSerialDescription();
       //                 dev_dashboard.Read_setting();
                       

       //                 DateTime _time = DateTime.Now;
       //                 if (dev_dashboard.ss_index != pre_index)//khong hieu
       //                 {
       //                     pre_index = dev_dashboard.ss_index;
       //                     string tmpStr = "";
       //                     for (int j = 0; j < dev_dashboard.numOfChannel ; j++)
       //                     {
       //                         show[j] = mGlobal.format_num(dev_dashboard.channels[j].Val) + " " + arrUnit[i, j];
       //                         tmpStr += show[j];
       //                         if(arrUnit[i,j] == "-")
       //                         {
       //                             show[j] = "---";  
       //                         }
       //                     }

       //                     dataGridView1.Rows[i].Cells[0].Value = getDeviceInfo.activeDeviceListAl[i].ToString();
       //                     dataGridView1.Rows[i].Cells[1].Value = dev_dashboard.Location1;
       //                     dataGridView1.Rows[i].Cells[2].Value = dev_dashboard.Description1;
       //                     dataGridView1.Rows[i].Cells[3].Value = dev_dashboard.Serial;

       //                     for (int k = 4; k < 4 + dev_dashboard.numOfChannel; k++)
       //                     {
       //                         dataGridView1.Rows[i].Cells[k].Value = show[k - 4];
       //                     }
       //                 }
       //                 dev_dashboard.Close();
       //             }
       //             else
       //             {
       //                 dataGridView1.Rows[i].Cells[4].Value = "Connecting";
       //                 dataGridView1.Rows[i].Cells[0].Value = getDeviceInfo.activeDeviceListAl[i].ToString();
       //             }
       //         }
       //     }
       // }

       // private void frmDashBoard_SizeChanged(object sender, EventArgs e)
       // {
       //     dataGridView1.Left = 30;
       //     dataGridView1.Width = this.Width - 60;
       //     dataGridView1.Height = this.Height - 200;

       //     groupBox1.Location1 = new Point((this.Width - groupBox1.Width) / 2, 25);
       //     dataGridView1.Location1 = new Point((this.Width - dataGridView1.Width) / 2, 150);
       // }

       // private void btnGraph_Click(object sender, EventArgs e)
       // {
       //     if (btnStart.Text == "Stop")
       //     {
       //         btnStart_Click(null, null);
       //     }
       //     DashBoardSelect dbSelect = new DashBoardSelect();
       //     dbSelect.ShowDialog();
       // }

       // private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
       // {
       //     MessageBox.Show(dataGridView1.CurrentCell.Value.ToString());
       // }

       // private void frmDashBoard_FormClosed(object sender, FormClosedEventArgs e)
       // {
       //     dataGridView1.Rows.Clear();
       //     timerDashBoard.Enabled = false;
       //     try
       //     {
       //         timer_graph = false;
       //     }
       //     catch (Exception)
       //     {  }
        //}
    }
}
