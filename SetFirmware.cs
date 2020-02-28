using System;
using System.Windows.Forms;

namespace Pexo16
{
    public partial class SetFirmware : Form
    {
        public Device35 device35 = null;

        public SetFirmware()
        {
            InitializeComponent();

            device35 = Device35.DelInstance();
            device35 = Device35.Instance;

            device35.Channels = new Channel[4];
            for (int i = 0; i < 4; i++)
            {
                device35.Channels[i] = new Channel();
            }

        }

        private void SetFirmware_Load(object sender, EventArgs e)
        {
            comboBox1.Text = comboBox1.Items[0].ToString();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            getDeviceInfo.getActiveDevice();
            if (getDeviceInfo.activeDeviceListAl.Count == 0)
            {
                MessageBox.Show("Troi oi! Chua cam logger kia thim 2!");
                return;
            }
            device35.hostport = getDeviceInfo.activeDeviceListAl[0].ToString();

            if (device35.USBOpen(device35.hostport) == false)
            {
                MessageBox.Show("Open USB fail");
                device35.Close();
                return;
            }

          
            if(comboBox1.Text == "With WifiReader")
            {
                device35.version = 1;
            }
            else
            {
                device35.version = 0;
            }

            mGlobal.len = 64;

            if(device35.writeFirmVer())
            {
                MessageBox.Show("OK!!!!!!!!!!");
                this.Close();
            }
            else
            {
                MessageBox.Show("Fail! Please try again!!!");
            }
            
            device35.Close();

        }
    }
}
