using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Pexo16
{
    public partial class DashBoardSelect : Form
    {
       
        public string selectedLogger;
        public string cbbSelected;
        int nhanDang = 0;
        public DashBoardSelect(int tyle)
        {
            InitializeComponent();
            nhanDang = tyle;
        }

        private void closeForm(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmDashBoardSelect_Load(object sender, EventArgs e)
        {
            cbbLogger.Items.Clear();

            for (int i = 0; i < getDeviceInfo.activeDeviceListAl.Count; i++)
            {
                Device tmpDev = Device.DelInstance();
                tmpDev = Device.Instance;
                string str_device = getDeviceInfo.activeDeviceListAl[i].ToString();
                tmpDev.USBOpen(str_device);
                if(nhanDang == 0)//pexo 16
                {
                    if (getDeviceInfo.nhanDang(tmpDev.dev) != "PEXO-35" && getDeviceInfo.nhanDang34(tmpDev.dev) != "PEXO-34")
                    {
                        cbbLogger.Items.Add(getDeviceInfo.activeDeviceListAl[i].ToString());
                    }
                }
                else if(nhanDang == 1)//pexo 35
                {
                    if (getDeviceInfo.nhanDang(tmpDev.dev) == "PEXO-35")
                    {
                        cbbLogger.Items.Add(getDeviceInfo.activeDeviceListAl[i].ToString());
                    }
                }
                tmpDev.Close();
                
            }
            if (cbbLogger.Items.Count == 0)
            {
                btnStart.Enabled = false;
            }
            else
            {
                cbbLogger.Text = cbbLogger.Items[0].ToString();
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {

            selectedLogger = cbbLogger.SelectedItem.ToString();
            cbbSelected = cbbLogger.SelectedItem.ToString();
            if(cbbSelected.Substring(17, 2) == "8S")
            {
                DashBoardGraph graph = new DashBoardGraph(this);
                this.Hide();
                graph.FormClosed += new FormClosedEventHandler(closeForm);
                graph.ShowDialog();      
            }
            else
            {
                DashBoardGraph35 graph = new DashBoardGraph35(this);
                this.Hide();
                graph.FormClosed += new FormClosedEventHandler(closeForm);
                graph.ShowDialog();
            }
        }

    }
}
