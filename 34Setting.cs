using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Globalization;
using System.Resources;
using System.IO;
using System.Text.RegularExpressions;
using NativeWifi;

namespace Pexo16
{
    public partial class _34Setting : Form
    {
        string wifi_pass = "";
        string ssid = "";
        string ip = "";

        string cloud_port = "";
        string cloud_ip = "";
        string password = "";
        string cloud_email = "";

        string staticip = "";
        string staticnetmask = "";
        string staticgateway = "";

        bool enableDHCP;

        Int16 reportID = 0x01;
        Int16 cmdPass = 0x02;
        Int16 cmdName = 0x01;
        Int16 cmdIp = 0x03;
        Int16 cmdPort = 0x04;
        Int16 cmdUser = 0x07;
        Int16 cmdDHCP = 0x10;
        Int16 cmdStaticIP = 0x11;
        Int16 cmdStaticNetmask = 0x12;
        Int16 cmdStaticGateway = 0x13;
        Int16 cmdEndSet = 0x45;
        Int16 cmdEnd = 0x0a;
        Int16 Mode = 0x01;
        Int16 l = 0x24;


        string hostPort;
        private string strStatus;

        Device dev34 = null;
        byte[] buf = new byte[100];

        ResourceManager res_man = new ResourceManager("Pexo16.Lang.Resources", typeof(_34Setting).Assembly);
        CultureInfo cul;

        public _34Setting(string hostport)
        {
            InitializeComponent();
            hostPort = hostport;
        }

        private void _34Setting_Load(object sender, EventArgs e)
        {
            switch (mGlobal.language)
            {
                case "Spanish":
                    cul = CultureInfo.CreateSpecificCulture("es-ES");
                    break;
                case "Korean":
                    cul = CultureInfo.CreateSpecificCulture("ko_KR");
                    break;
                case "Japanese":
                    cul = CultureInfo.CreateSpecificCulture("ja-JP");
                    break;
                default:
                    cul = CultureInfo.CreateSpecificCulture("en-US");
                    break;
            }


            //label7.Text = res_man.GetString("Network Setting", cul);
            label2.Text = res_man.GetString("Password", cul);
            label3.Text = res_man.GetString("Name", cul);
            btnSend.Text = res_man.GetString("Send", cul);
            btnReset.Text = res_man.GetString("Reset", cul);
            chkShowPass.Text = res_man.GetString("Show password", cul);

            txtPass.Focus();
            chkShowPass.Checked = true;
            try
            {
                WlanClient client = new WlanClient();
                if (client != null)
                {
                    if (client.Interfaces.Length > 0)
                    {
                        foreach (WlanClient.WlanInterface wlanIface in client.Interfaces)
                        {
                            // Lists all available networks
                            //Wlan.WlanAccess net;
                            //wlanIface.
                            Wlan.WlanProfileInfo[] b = wlanIface.GetProfiles();
                            Wlan.WlanBssEntry[] a = wlanIface.GetNetworkBssList();
                            Wlan.WlanAvailableNetwork[] networks = wlanIface.GetAvailableNetworkList(Wlan.WlanGetAvailableNetworkFlags.IncludeAllAdhocProfiles);
                            foreach (Wlan.WlanAvailableNetwork network in networks)
                            {
                                string name = System.Text.Encoding.UTF8.GetString(network.dot11Ssid.SSID);
                                bool add = true;

                                foreach (string item in cbbNetWork.Items)
                                {
                                    if (item == name)
                                    {
                                        add = false;
                                    }
                                }

                                if (add)
                                {
                                    cbbNetWork.Items.Add(name);
                                }
                                //Console.WriteLine("Found network with SSID {0}.", GetStringForSSID(network.dot11Ssid));
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                //throw;
                Close();
            }


            //foreach (Wlan.WlanProfileInfo profileInfo in wlanIface.GetProfiles())
            //{
            //    cbbNetWork.Items.Add(profileInfo.profileName);
            //}

            //foreach(Wlan.)



            string FileName = "";
            FileName = mGlobal.app_patch(FileName);

            FileName += "\\dataSetting34.txt";

            string setting = "";

            if (System.IO.File.Exists(FileName))
            {
                FileStream fs = new FileStream(FileName, FileMode.Open, FileAccess.ReadWrite);

                using (StreamReader sw = new StreamReader(fs))
                {
                    setting += sw.ReadToEnd();
                }


                readTextSetting(setting);

                txtPass.Text = wifi_pass;
                //txtName.Text = name;
                cbbNetWork.Text = ssid;

                if (ip != "")
                {
                    txtIP.Text = ip;
                    //string[] ipTamp = ip.Split('.');

                    //txtIP.Text = ipTamp[0];
                    //txtIP2.Text = ipTamp[1];
                    //txtIP3.Text = ipTamp[2];
                    //txtIP4.Text = ipTamp[3];
                }
                else
                {
                    txtIP.Text = "";
                    //txtIP2.Text = "";
                    //txtIP3.Text = "";
                    //txtIP4.Text = "";
                }
                txtPort.Text = cloud_port;
                txtStaticIP.Text = staticip;
                txtNetmask.Text = staticnetmask;
                txtGateway.Text = staticgateway;
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            dev34 = Device.DelInstance();
            dev34 = Device.Instance;
            
            
            string pass = txtPass.Text;
            //string name = txtName.Text;
            //string ip = txtIP.Text + "." + txtIP2.Text + "." + txtIP3.Text + "." + txtIP4.Text; 
            string ip = txtIP.Text;
            string port = txtPort.Text;
            string sta_ip = txtStaticIP.Text;
            string netmask = txtNetmask.Text;
            string gateway = txtGateway.Text;

            string msg = "";

            if (cbbNetWork.Text == "")
            {
                msg += res_man.GetString("Wifi name cannot be empty", cul) + Environment.NewLine;
            }
            if(txtPass.Text == "")
            {
                msg += res_man.GetString("Wifi password cannot be empty", cul) + Environment.NewLine;
            }
            if(txtIP.Text == "")
            {
                msg += res_man.GetString("IP cannot be empty", cul) + Environment.NewLine;
            }
            if(txtPort.Text == "")
            {
                msg += res_man.GetString("Port cannot be empty", cul)+ Environment.NewLine; 
            }
            if (txtUsername.Text == "")
            {
                msg += res_man.GetString("Email cannot be empty", cul) ; 
            }
            if (rdostaticip.Checked)
            {
                if (txtStaticIP.Text == "")
                {
                    msg += res_man.GetString("Static IP cannot be empty", cul) + Environment.NewLine;
                }
                if (txtNetmask.Text == "")
                {
                    msg += res_man.GetString("Netmask cannot be empty", cul) + Environment.NewLine;
                }
                if (txtGateway.Text == "")
                {
                    msg += res_man.GetString("Gateway cannot be empty", cul);
                }
            }
           
            if (msg == "")
            {
                if (dev34.USBOpen(hostPort) != true)
                {
                    MessageBox.Show(res_man.GetString("Open", cul) + " USB " + res_man.GetString("fail", cul) + " . " + res_man.GetString("Please try again", cul));
                    dev34.Close();
                    this.Close();
                    return;
                }
                //bool a = test();
                //bool a = pexo16();
                //string a = nhanDang();
                if (Write_Pass() == false)
                {
                    MessageBox.Show(res_man.GetString("Write pass fail",cul));
                    dev34.Close();
                    return;
                }
                Thread.Sleep(200);
                if (Write_Name() == false)
                {
                    MessageBox.Show(res_man.GetString("Write Name fail", cul));
                    dev34.Close();
                    return;
                }
                Thread.Sleep(200);
                if (Write_DHCP() == false)
                {
                    MessageBox.Show(res_man.GetString("Write DHCP fail", cul));
                    dev34.Close();
                    return;
                }

                if (Write_StaticIp() == false)
                {
                    MessageBox.Show(res_man.GetString("Write Static IP fail", cul));
                    dev34.Close();
                    return;
                }


                Thread.Sleep(200);
                if (Write_StaticNetmask() == false)
                {
                    MessageBox.Show(res_man.GetString("Write Netmask fail", cul));
                    dev34.Close();
                    return;
                }
                Thread.Sleep(200);
                if (Write_StaticGateway() == false)
                {
                    MessageBox.Show(res_man.GetString("Write Gateway fail", cul));
                    dev34.Close();
                    return;
                }
                Thread.Sleep(200);
                if (Write_Ip() == false)
                {
                    MessageBox.Show(res_man.GetString("Write IP fail", cul));
                    dev34.Close();
                    return;
                }
                Thread.Sleep(200);
                if (Write_Port() == false)
                {
                    MessageBox.Show(res_man.GetString("Write Port fail", cul));
                    dev34.Close();
                    return;
                }

                Thread.Sleep(200);
                if (Write_ModeSetting() == false)
                {
                    MessageBox.Show(res_man.GetString("End setting fail", cul));
                    dev34.Close();
                    return;
                }
                Thread.Sleep(200);
                if (Write_Interval()==false)
                {
                    MessageBox.Show("Write interval fail!");
                    dev34.Close();
                    return;
                }

                Thread.Sleep(200);
                if (Write_User() == false)
                {
                    MessageBox.Show("Write user account fail!");
                    dev34.Close();
                    return;
                }
                Thread.Sleep(700);
                if (Write_EndSetting() == false)
                {
                    MessageBox.Show(res_man.GetString("End setting fail", cul));
                    dev34.Close();
                    return;
                }
                var str = res_man.GetString("Setting successful", cul);
                MessageBox.Show(res_man.GetString("Setting successful", cul));
                dev34.Close();
            }
            else
            {
                MessageBox.Show(msg);
                return;
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            dev34 = Device.DelInstance();
            dev34 = Device.Instance;
            if (dev34.USBOpen(hostPort) != true)
            {
                MessageBox.Show(res_man.GetString("Open", cul) +" USB " + res_man.GetString("fail", cul) + ". " +  res_man.GetString("Please try again", cul));
                dev34.Close();
                this.Close();
                return;
            }

            //txtPass.Text = "";
            //txtName.Text = "";
            //txtIP1.Text = "";
            //txtIP2.Text = "";
            //txtIP3.Text = "";
            //txtIP4.Text = "";
            //txtPort.Text = "";


            bool reset = Write_Reset();
            if (reset != true)
            {
                MessageBox.Show(res_man.GetString("Reset fail", cul));
            }
            else
            {
                dev34.Close();
                this.Close();
            }
        }

        private bool test()
        {
            byte[] abuf = new byte[500];
            byte[] msg = new byte[100];
            short res = 0;

            //abuf[0] = 0x0;
            abuf[0] = 0x0a;
            //abuf[0] = byte.Parse(0x2.ToString());
            //abuf[1] = byte.Parse(3.ToString());

            //res = HIDFunction.hid_SendFeatureReport(dev34.dev, ref abuf[0], 64);
            //res = HIDFunction.hid_Write(dev34.dev, ref abuf[0], 64);

            //abuf[0] = 0x0;
            res = HIDFunction.hid_Read(dev34.dev, ref abuf[0], 6);
            //res = HIDFunction.hid_GetFeatureReport(dev34.dev, ref abuf[0], 500);
            if (res <= 0)
            {
                HIDFunction.hid_Error(dev34.dev, ref msg[0]);
                string error = "";
                for (int i = 0; i < msg.Length; i++)
                {
                    char a = (char)msg[i];
                    error += a.ToString();
                }
                MessageBox.Show(error);
            }

            string e = "";
            for (int i = 0; i < abuf.Length; i++)
            {
                char a = (char)abuf[i];
                e += a.ToString();
            }

            return true;
        }

        private bool pexo16()
        {
            byte[] abuf = new byte[501];
            short res = 0;

            abuf[0] = byte.Parse(2.ToString());
            abuf[1] = byte.Parse(3.ToString());

            res = HIDFunction.hid_SendFeatureReport(dev34.dev, ref abuf[0], 128 + 5);
                
            res = HIDFunction.hid_GetFeatureReport(dev34.dev, ref abuf[0], 254);

            string e = "";
            for (int i = 0; i < abuf.Length; i++)
            {
                char a = (char)abuf[i];
                e += a.ToString();
            }

            return true;
        }

        public string nhanDang()
        {
            short res = 0;
            string name = "";
            byte[] buf = new byte[64];
            byte[] msg = new byte[100];

            Int16 m = 0x0;
            Int16 n = 0x01;
            Int16 b = 0xa8;
            Int16 v = 0x33;
            Int16 c = 0x34;
            buf[0] = byte.Parse(m.ToString());
            buf[1] = byte.Parse(n.ToString());
            buf[2] = byte.Parse(b.ToString());
            buf[3] = byte.Parse(v.ToString());
            buf[4] = byte.Parse(c.ToString());

            //buf[0] = 0x0;
            //buf[1] = 0x01;
            //buf[2] = 0xa8;
            ////buf[3] = 0x50;
            ////buf[4] = 0x43;
            //buf[3] = 0x33;
            //buf[4] = 0x34;

            byte[] buf3 = new byte[100];
            byte[] buf2 = new byte[100];
            byte s;
            buf2[0] = byte.Parse(0x01.ToString());
            buf2[1] = byte.Parse(0xa8.ToString());
            buf2[2] = byte.Parse(0x33.ToString());
            buf2[3] = byte.Parse(0x34.ToString());

            //HIDFunction.hid_DeviceSerialNum(dev34.dev, ref buf3[0]);
            //HIDFunction.hid_GetSerialNumberString(dev34.dev, ref buf3[0], 10);
            //res = HIDFunction.hid_SetNonBlocking(dev34.dev, 0);
            res = HIDFunction.hid_Write(dev34.dev, ref  buf[0], 64);
            //res = HIDFunction.hid_SendFeatureReport(dev34.dev, ref buf2[0], 63);
            //res = HIDFunction.hid_SendFeatureReport(dev34.dev,ref buf[0], 64);
            //buf[0] = byte.Parse(m.ToString());
            //res = HIDFunction.hid_Read(dev34.dev, ref buf[0], 64);
            //res = HIDFunction.hid_ReadTimeOut(dev34.dev, ref buf[0], 7, 500);
            //buf2[0] = 0x2;
            Thread.Sleep(5000);
            res = HIDFunction.hid_Read(dev34.dev,ref buf[0], 64);
            //res = HIDFunction.hid_GetFeatureReport(dev34.dev, ref buf[0], 64);
            //res = HIDFunction.hid_Get_Feature_Report(dev34.dev, ref buf[0], 64);
            //sau khi check device
            //res = HIDFunction.hidRead(dev34.dev, ref buf2[0], 64);
            string ten = "";
            for (int i = 0; i < buf2[1]; i++)
            {
                ten += (char)buf2[i + 2];
            }

            if(res > 0)
            {
                buf[0] = 0x02;
                //res = HIDFunction.hid_Read(dev34.dev, ref buf[0], 7);
                res = HIDFunction.hid_GetFeatureReport(dev34.dev, ref buf[0], 255);
                if(res < 0)
                {
                    HIDFunction.hid_Error(dev34.dev, ref msg[0]);
                    string error = "";
                    for (int i = 0; i < msg.Length; i++)
                    {
                        char a = (char)msg[i];
                        error += a.ToString();
                    }
                    MessageBox.Show(error);
                }
            }
            else
            {
                HIDFunction.hid_Error(dev34.dev, ref msg[0]);
                string error = "";
                for (int i = 0; i < msg.Length; i++)
                {
                    char a = (char)msg[i];
                    error += a.ToString();
                }
                MessageBox.Show(error);
            }
            return name;
        }

        private bool Write_Pass()
        {
            wifi_pass = txtPass.Text;
           
            byte[] bufPass = new byte[100];
            byte[] msg = new byte[100];
            short res = 0;
            int lenght = wifi_pass.Length + 4;

            bufPass[0] = byte.Parse(reportID.ToString());
            bufPass[1] = byte.Parse(cmdPass.ToString());

            //string stringhexLenght = String.Format("{0:X}", lenght);
            //Int16 hexLenght = Convert.ToInt16(stringhexLenght, 16);
            bufPass[2] = byte.Parse(l.ToString());

            for (int i = 0; i < wifi_pass.Length; i++)
            {
                //char item = Convert.ToChar(pass.Substring(i, 1));
                //byte dec = (byte)item;
                //string hexOutput = String.Format("{0:X}", dec);
                //Int16 v = Convert.ToInt16(hexOutput, 16);
                bufPass[i + 3] = Encoding.ASCII.GetBytes(wifi_pass.Substring(i, 1))[0];
                //string temp = pass.Substring(i, 1);
                //int value = Convert.ToInt32(temp);
                //string hexOutput = String.Format("{0:X}", value);
                //UInt16 Hextemp = UInt16.Parse(temp, NumberStyles.AllowHexSpecifier);
                //Convert.ToInt32(temp).ToString("X");
                //bufPass[i + 3] = Encoding.ASCII.GetBytes(Hextemp.ToString())[0];
                //string a = String.Format("{0:x2}", (uint)System.Convert.ToUInt32(temp.ToString()));
                //Int16 b = Int16.Parse(temp);
                //bufPass[i + 3] = byte.Parse(b.ToString());
            }
            //bufPass[pass.Length + 3] = byte.Parse(cmdEnd.ToString());
            for (int i = wifi_pass.Length + 3; i < 64; i++)
            {
                bufPass[i] = 0x00;
            }

            //byte[] tamp = new byte[bufPass.Length];
            //for (int i = 0; i < lenght; i++)
            //{
            //    string k = String.Format("{0:X}", bufPass[i]);
            //    tamp[i] = byte.Parse(k);
            //}


            //strStatus = "send_feature(CMD_WRITE_PASS)";
            //ushort len = ushort.Parse(bufPass.Length.ToString());
            //res = HIDFunction.hid_SendFeatureReport(dev34.dev, ref bufPass[0], 65);
            Thread.Sleep(20);
            res = HIDFunction.hid_Write(dev34.dev, ref  bufPass[0], 65);
            if(res < 0)
            {
                strStatus += "error (res = " + res +")"+ Environment.NewLine ;
                HIDFunction.hid_Error(dev34.dev, ref msg[0]);
                string b = "";
                for (int i = 0; i < msg.Length; i++)
                {
                    char a = (char)msg[i];
                    b += a.ToString();
                }
                MessageBox.Show(b);
                strStatus += "Error = " + msg.ToString() + Environment.NewLine;
                return false;
            }
            //bufPass[0] = byte.Parse(m.ToString());
            //res = HIDFunction.hid_GetFeatureReport(dev34.dev, ref bufPass[0], 255);
            //string Serial = mGlobal.ArrayToStr(ref bufPass, 2, 10);
            return true;
        }

        private bool Write_Name()
        {
            ssid = cbbNetWork.Text;
            byte[] bufName = new byte[100];
            byte[] msg = new byte[100];
            short res = 0;
            int lenght = ssid.Length + 4;

            bufName[0] = byte.Parse(reportID.ToString());
            bufName[1] = byte.Parse(cmdName.ToString());
            bufName[2] = byte.Parse(l.ToString());
            
            for (int i = 0; i < ssid.Length; i++)
            {
                bufName[i + 3] = Encoding.ASCII.GetBytes(ssid.Substring(i,1))[0];
            }
            //bufName[name.Length + 3] = byte.Parse(cmdEnd.ToString());

            for (int i = ssid.Length + 3; i < 64; i++)
            {
                bufName[i] = 0x00;
            }

            Thread.Sleep(20);
            res = HIDFunction.hid_Write(dev34.dev, ref  bufName[0], 65);

            if(res < 0)
            {
                HIDFunction.hid_Error(dev34.dev, ref msg[0]);
                string b = "";
                for (int i = 0; i < msg.Length; i++)
                {
                    char a = (char)msg[i];
                    b += a.ToString();
                }
                return false;
            }
            return true;
        }

        private bool Write_User()
        {
            cloud_ip = txtUsername.Text;
            //password = txtPassword.Text;
            byte[] bufUser = new byte[100];
            byte[] msg = new byte[100];
            short res = 0;

            bufUser[0] = byte.Parse(reportID.ToString());
            bufUser[1] = byte.Parse(cmdUser.ToString());
            bufUser[2] = 0x2C;

            for (int i = 0; i < cloud_ip.Length; i++)
            {
                bufUser[i + 3] = Encoding.ASCII.GetBytes(cloud_ip.Substring(i, 1))[0];
            }

            //for (int i = 0; i < password.Length; i++)
            //{
            //    bufUser[i + 23] = Encoding.ASCII.GetBytes(password.Substring(i, 1))[0];
            //}
            Thread.Sleep(20);
            res = HIDFunction.hid_Write(dev34.dev, ref  bufUser[0], 65);

            if (res < 0)
            {
                HIDFunction.hid_Error(dev34.dev, ref msg[0]);
                string b = "";
                for (int i = 0; i < msg.Length; i++)
                {
                    char a = (char)msg[i];
                    b += a.ToString();
                }
                return false;
            }
            return true;
        }

        private bool Write_Ip()
        {
            //ip = txtIP.Text + "." + txtIP2.Text + "." + txtIP3.Text + "." + txtIP4.Text;
            ip = txtIP.Text;
            byte[] bufIP = new byte[100];
            byte[] msg = new byte[100];
            short res = 0;
            int lenght = ip.Length + 4;

            bufIP[0] = byte.Parse(reportID.ToString());
            bufIP[1] = byte.Parse(cmdIp.ToString());
            bufIP[2] = 0x13;

            for (int i = 0; i < ip.Length; i++)
            {
                bufIP[i + 3] = Encoding.ASCII.GetBytes(ip.Substring(i, 1))[0];
            }
            //bufIP[ip.Length + 3] = byte.Parse(cmdEnd.ToString());

            for (int i = ip.Length + 3; i < 64; i++)
            {
                bufIP[i] = 0x00;
            }

            //res = HIDFunction.hid_SendFeatureReport(dev34.dev, ref bufIP[0], 65);
            Thread.Sleep(20);
            res = HIDFunction.hid_Write(dev34.dev, ref  bufIP[0], 65);
            if(res < 0)
            {
                HIDFunction.hid_Error(dev34.dev, ref msg[0]);
                string b = "";
                for (int i = 0; i < msg.Length; i++)
                {
                    char a = (char)msg[i];
                    b += a.ToString();
                }
                MessageBox.Show(b);
                return false;
            }
            return true;
        }

        private void ReadAllData()
        {
            byte [] buf = new byte[64];
            byte[] data = new byte[64];
            int res;
            int length = 0;
            //Read SSID
            buf[0] = 0x01;
            buf[1] = 0x14;
            res = HIDFunction.hid_Write(dev34.dev, ref buf[0], 65);
            if (res >0)
            {
                HIDFunction.hid_Read(dev34.dev, ref data[0], 64);
                length = data[1];
                ssid = Encoding.ASCII.GetString(data, 2, length).Replace("\0","");
                
            }
            else
            {
                HIDFunction.hid_Error(dev34.dev, ref data[0]);
                string b = "";
                b = Encoding.ASCII.GetString(data, 2, data.Length);
                MessageBox.Show(b);
                return;
            }
            Thread.Sleep(100);
            //Read Wifi Password
            buf[0] = 0x01;
            buf[1] = 0x15;
            res = HIDFunction.hid_Write(dev34.dev, ref buf[0], 65);
            if (res > 0)
            {
                HIDFunction.hid_Read(dev34.dev, ref data[0], 64);
                length = data[1];
                wifi_pass = Encoding.ASCII.GetString(data, 2, length).Replace("\0", "");
            }
            else
            {
                HIDFunction.hid_Error(dev34.dev, ref data[0]);
                string b = "";
                b = Encoding.ASCII.GetString(data, 2, data.Length);
                MessageBox.Show(b);
                return;
            }
            Thread.Sleep(100);
            //Read IP DHCP Or Static
            buf[0] = 0x01;
            buf[1] = 0x16;
            res = HIDFunction.hid_Write(dev34.dev, ref buf[0], 65);
            if (res > 0)
            {
                HIDFunction.hid_Read(dev34.dev, ref data[0], 64);
                length = data[1];
                if (length==16)
                {
                    enableDHCP = true;
                    staticip = Encoding.ASCII.GetString(data, 3, 15).Replace("\0", "");
                    staticgateway = "";
                    staticnetmask = "";
                }
                else if (length == 46)
                {
                    enableDHCP = false;
                    staticip = Encoding.ASCII.GetString(data, 3, 15).Replace("\0", "");
                    staticnetmask = Encoding.ASCII.GetString(data, 18, 15).Replace("\0", "");
                    staticgateway = Encoding.ASCII.GetString(data, 33, 15).Replace("\0", "");
                }
            }
            else
            {
                HIDFunction.hid_Error(dev34.dev, ref data[0]);
                string b = "";
                b = Encoding.ASCII.GetString(data, 2, data.Length);
                MessageBox.Show(b);
                return;
            }
            Thread.Sleep(100);
            //Read cloud setting
            buf[0] = 0x01;
            buf[1] = 0x17;
            res = HIDFunction.hid_Write(dev34.dev, ref buf[0], 65);
            if (res > 0)
            {
                HIDFunction.hid_Read(dev34.dev, ref data[0], 64);
                length = data[1];
                cloud_ip = Encoding.ASCII.GetString(data, 2, 15).Replace("\0","");
                cloud_port = Encoding.ASCII.GetString(data, 17, 5).Replace("\0", "");

            }
            else
            {
                HIDFunction.hid_Error(dev34.dev, ref data[0]);
                string b = "";
                b = Encoding.ASCII.GetString(data, 2, data.Length);
                MessageBox.Show(b);
                return;
            }


        }

        private bool Write_Port()
        {
            cloud_port = txtPort.Text;
            byte[] bufPort = new byte[100];
            byte[] msg = new byte[100];
            short res = 0;
            int lenght = cloud_port.Length + 4;

            bufPort[0] = byte.Parse(reportID.ToString());
            bufPort[1] = byte.Parse(cmdPort.ToString());
            bufPort[2] = 0x09;

            for (int i = 0; i < cloud_port.Length; i++)
            {
                bufPort[i + 3] = Encoding.ASCII.GetBytes(cloud_port.Substring(i, 1))[0];
            }
            //bufPort[port.Length + 3] = byte.Parse(cmdEnd.ToString());
            for (int i = cloud_port.Length + 3; i < 64; i++)
            {
                bufPort[i] = 0x00;
            }

            //res = HIDFunction.hid_SendFeatureReport(dev34.dev, ref bufPort[0], 65);
            Thread.Sleep(20);
            res = HIDFunction.hid_Write(dev34.dev, ref  bufPort[0], 65);
            if(res < 0)
            {
                HIDFunction.hid_Error(dev34.dev, ref msg[10]);
                string b = "";
                for (int i = 0; i < msg.Length; i++)
                {
                    char a = (char)msg[i];
                    b += a.ToString();
                }
                return false;
            }
            return true;
        }

        private bool Write_Interval()
        {
            byte[] bufPort = new byte[100];
            byte[] msg = new byte[100];
            short res = 0;
            int lenght = cloud_port.Length + 4;

            bufPort[0] = 0x01;
            bufPort[1] = 0x0A;
            TimeSpan interval = TimeSpan.FromSeconds((double)txtInterval.Value);
            bufPort[2] = (byte)interval.Seconds;
            bufPort[3] = (byte)interval.Minutes;
            bufPort[4] = (byte)interval.Hours;
            res = HIDFunction.hid_Write(dev34.dev, ref bufPort[0], 65);
            if (res < 0)
            {
                HIDFunction.hid_Error(dev34.dev, ref msg[10]);
                string b = "";
                for (int i = 0; i < msg.Length; i++)
                {
                    char a = (char)msg[i];
                    b += a.ToString();
                }
                return false;
            }
            return true;
        }
        private bool Write_DHCP()
        {
            byte[] bufdhcp = new byte[100];
            byte[] msg = new byte[100];
            short res = 0;
            //int lenght = 5;

            bufdhcp[0] = byte.Parse(reportID.ToString());
            bufdhcp[1] = byte.Parse(cmdDHCP.ToString());
            bufdhcp[2] = 0x05;
            if (rdodhcp.Checked == true)
            {
                bufdhcp[3] = 0x31;
            }
            if (rdostaticip.Checked == true)
            {
                bufdhcp[3] = 0x30;
            }
            
            //bufPort[port.Length + 3] = byte.Parse(cmdEnd.ToString());
            for (int i = 4; i < 64; i++)
            {
                bufdhcp[i] = 0x00;
            }

            //res = HIDFunction.hid_SendFeatureReport(dev34.dev, ref bufPort[0], 65);
            Thread.Sleep(20);
            res = HIDFunction.hid_Write(dev34.dev, ref bufdhcp[0], 65);
            if (res < 0)
            {
                HIDFunction.hid_Error(dev34.dev, ref msg[10]);
                string b = "";
                for (int i = 0; i < msg.Length; i++)
                {
                    char a = (char)msg[i];
                    b += a.ToString();
                }
                return false;
            }
            return true;
        }

        private bool Write_StaticIp()
        {
            //ip = txtIP.Text + "." + txtIP2.Text + "." + txtIP3.Text + "." + txtIP4.Text;
            if (rdodhcp.Checked)
            {
                staticip = "0.0.0.0";
            }
            else
            {
                staticip = txtStaticIP.Text;
            }
            byte[] bufSTATICIP = new byte[100];
            byte[] msg = new byte[100];
            short res = 0;
            int lenght = staticip.Length + 4;

            bufSTATICIP[0] = byte.Parse(reportID.ToString());
            bufSTATICIP[1] = byte.Parse(cmdStaticIP.ToString());
            bufSTATICIP[2] = 0x13;

            for (int i = 0; i < staticip.Length; i++)
            {
                bufSTATICIP[i + 3] = Encoding.ASCII.GetBytes(staticip.Substring(i, 1))[0];
            }
            //bufIP[ip.Length + 3] = byte.Parse(cmdEnd.ToString());

            for (int i = staticip.Length + 3; i < 64; i++)
            {
                bufSTATICIP[i] = 0x00;
            }

            //res = HIDFunction.hid_SendFeatureReport(dev34.dev, ref bufIP[0], 65);
            Thread.Sleep(20);
            res = HIDFunction.hid_Write(dev34.dev, ref bufSTATICIP[0], 65);
            if (res < 0)
            {
                HIDFunction.hid_Error(dev34.dev, ref msg[0]);
                string b = "";
                for (int i = 0; i < msg.Length; i++)
                {
                    char a = (char)msg[i];
                    b += a.ToString();
                }
                MessageBox.Show(b);
                return false;
            }
            return true;
        }

        private bool Write_StaticNetmask()
        {
            //ip = txtIP.Text + "." + txtIP2.Text + "." + txtIP3.Text + "." + txtIP4.Text;
            if (rdodhcp.Checked)
            {
                staticnetmask = "0.0.0.0";
            }
            else
            {
                staticnetmask = txtNetmask.Text;
            }
            
            byte[] bufNETMASK = new byte[100];
            byte[] msg = new byte[100];
            short res = 0;
            int lenght = staticnetmask.Length + 4;

            bufNETMASK[0] = byte.Parse(reportID.ToString());
            bufNETMASK[1] = byte.Parse(cmdStaticNetmask.ToString());
            bufNETMASK[2] = 0x13;

            for (int i = 0; i < staticnetmask.Length; i++)
            {
                bufNETMASK[i + 3] = Encoding.ASCII.GetBytes(staticnetmask.Substring(i, 1))[0];
            }
            //bufIP[ip.Length + 3] = byte.Parse(cmdEnd.ToString());

            for (int i = staticnetmask.Length + 3; i < 64; i++)
            {
                bufNETMASK[i] = 0x00;
            }

            //res = HIDFunction.hid_SendFeatureReport(dev34.dev, ref bufIP[0], 65);
            Thread.Sleep(20);
            res = HIDFunction.hid_Write(dev34.dev, ref bufNETMASK[0], 65);
            if (res < 0)
            {
                HIDFunction.hid_Error(dev34.dev, ref msg[0]);
                string b = "";
                for (int i = 0; i < msg.Length; i++)
                {
                    char a = (char)msg[i];
                    b += a.ToString();
                }
                MessageBox.Show(b);
                return false;
            }
            return true;
        }

        private bool Write_StaticGateway()
        {
            //ip = txtIP.Text + "." + txtIP2.Text + "." + txtIP3.Text + "." + txtIP4.Text;
            if (rdodhcp.Checked)
            {
                staticgateway = "0.0.0.0";
            }
            else
            {
                staticgateway = txtGateway.Text;
            }
            
            byte[] bufGateway = new byte[100];
            byte[] msg = new byte[100];
            short res = 0;
            int lenght = staticgateway.Length + 4;

            bufGateway[0] = byte.Parse(reportID.ToString());
            bufGateway[1] = byte.Parse(cmdStaticGateway.ToString());
            bufGateway[2] = 0x13;

            for (int i = 0; i < staticgateway.Length; i++)
            {
                bufGateway[i + 3] = Encoding.ASCII.GetBytes(staticgateway.Substring(i, 1))[0];
            }
            //bufIP[ip.Length + 3] = byte.Parse(cmdEnd.ToString());

            for (int i = staticgateway.Length + 3; i < 64; i++)
            {
                bufGateway[i] = 0x00;
            }

            //res = HIDFunction.hid_SendFeatureReport(dev34.dev, ref bufIP[0], 65);
            Thread.Sleep(20);
            res = HIDFunction.hid_Write(dev34.dev, ref bufGateway[0], 65);
            if (res < 0)
            {
                HIDFunction.hid_Error(dev34.dev, ref msg[0]);
                string b = "";
                for (int i = 0; i < msg.Length; i++)
                {
                    char a = (char)msg[i];
                    b += a.ToString();
                }
                MessageBox.Show(b);
                return false;
            }
            return true;
        }

        private bool Write_EndSetting()
        {
            byte[] bufEndSet = new byte[500];
            byte[] msg = new byte[100];
            short res = 0;

            bufEndSet[0] = 0x01;
            bufEndSet[1] = 0x45;
            bufEndSet[2] = 0x04;
            bufEndSet[3] = 0x0a;

            Thread.Sleep(20);
            res = HIDFunction.hid_Write(dev34.dev, ref  bufEndSet[0], 65);
            //res = HIDFunction.hid_SendFeatureReport(dev34.dev, ref bufEndSet[0], 64);
            
            //bufEndSet[0] = 0x2;
            //res = HIDFunction.hid_Read(dev34.dev, ref bufEndSet[0], 64);
            //res = HIDFunction.hid_GetFeatureReport(dev34.dev, ref bufEndSet[0], 11);
            if(res < 0)
            {
                HIDFunction.hid_Error(dev34.dev, ref msg[0]);
                string b = "";
                for (int i = 0; i < msg.Length; i++)
                {
                    char a = (char)msg[i];
                    b += a.ToString();
                }
                return false;
            }
            return true;
        }

        private bool Write_ModeSetting()
        {
            byte[] bufModeSet = new byte[500];
            byte[] msg = new byte[100];
            short res = 0;

            bufModeSet[0] = 0x01;
            bufModeSet[1] = 0x05;
            bufModeSet[2] = 0x05;
            bufModeSet[3] = byte.Parse(Mode.ToString());
            bufModeSet[4] = 0x0a;

            Thread.Sleep(20);
            res = HIDFunction.hid_Write(dev34.dev, ref  bufModeSet[0], 65);
            //res = HIDFunction.hid_SendFeatureReport(dev34.dev, ref bufEndSet[0], 64);

            //bufEndSet[0] = 0x2;
            //res = HIDFunction.hid_Read(dev34.dev, ref bufEndSet[0], 64);
            //res = HIDFunction.hid_GetFeatureReport(dev34.dev, ref bufEndSet[0], 11);
            if (res < 0)
            {
                HIDFunction.hid_Error(dev34.dev, ref msg[0]);
                string b = "";
                for (int i = 0; i < msg.Length; i++)
                {
                    char a = (char)msg[i];
                    b += a.ToString();
                }
                return false;
            }
            return true;
        }

        private bool Write_Reset()
        {
            byte[] bufReset = new byte[100];
            byte[] bufMsg = new byte[100];
            short res = 0;

            bufReset[0] = 0x01;
            bufReset[1] = 0x46;
            bufReset[2] = 0x04;
            bufReset[3] = 0x0a;

            Thread.Sleep(200);
            res = HIDFunction.hid_Write(dev34.dev, ref  bufReset[0], 65);
            if(res < 0)
            {
                HIDFunction.hid_Error(dev34.dev, ref bufMsg[0]);
                string b = "";
                for (int i = 0; i < bufMsg.Length; i++)
                {
                    char a = (char)bufMsg[i];
                    b += a.ToString();
                }
                return false;
            }
            return true;
        }

        private void txtPort_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
            else
            {
                if (e.KeyChar != 8)
                {
                    if (txtPort.Text.Length > 4)
                    {
                        e.Handled = true;
                        ToolTip t = new ToolTip();
                        t.SetToolTip(txtPort, res_man.GetString("Port cannot be over 5 digit", cul));
                    }
                }
                if (e.KeyChar == 13)
                {
                    if(txtPass.Text != "" && txtIP.Text != "" && txtPort.Text != "")
                    {
                        btnSend_Click(sender, e);
                    }
                    else
                    {
                        txtPass.Focus();
                    }
                }
            }
        }

        //private void txtIP1_KeyPress(object sender, KeyPressEventArgs e)
        //{
        //    if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
        //    {
        //        e.Handled = true;
        //    }
        //    else
        //    {
        //        if (e.KeyChar != 8)
        //        {
        //            if (txtIP.Text.Length > 2)
        //            {
        //                e.Handled = true;
        //            }
        //        }
        //        if (e.KeyChar == 13)
        //        {
        //            txtIP2.Focus();
        //        }
        //    }
        //}

        //private void txtIP2_KeyPress(object sender, KeyPressEventArgs e)
        //{
        //    if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
        //    {
        //        e.Handled = true;
        //    }
        //    else
        //    {
        //        if (e.KeyChar != 8)
        //        {
        //            if (txtIP2.Text.Length > 2)
        //            {
        //                e.Handled = true;
        //            }
        //            if (e.KeyChar == 13)
        //            {
        //                txtIP3.Focus();
        //            }
        //        }
        //    }
        //}

        //private void txtIP3_KeyPress(object sender, KeyPressEventArgs e)
        //{
        //    if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
        //    {
        //        e.Handled = true;
        //    }
        //    else
        //    {
        //        if (e.KeyChar != 8)
        //        {
        //            if (txtIP3.Text.Length > 2)
        //            {
        //                e.Handled = true;
        //            }
        //            if (e.KeyChar == 13)
        //            {
        //                txtIP4.Focus();
        //            }
        //        }
        //    }
        //}

        //private void txtIP4_KeyPress(object sender, KeyPressEventArgs e)
        //{
        //    if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
        //    {
        //        e.Handled = true;
        //    }
        //    else
        //    {
        //        if (e.KeyChar != 8)
        //        {
        //            if (txtIP4.Text.Length > 2)
        //            {
        //                e.Handled = true;
        //            }
        //            if (e.KeyChar == 13)
        //            {
        //                txtPort.Focus();
        //            }
        //        }
        //    }
        //}

        private void txtPass_KeyPress(object sender, KeyPressEventArgs e)
       {
            if (e.KeyChar != 8)
            {
                if(txtPass.Text.Length > 31)
                {
                    e.Handled = true;
                    ToolTip t = new ToolTip();
                    t.SetToolTip(txtPass, "Pass cannot be over 32 characters");
                }
            }
        }

        private void chkShowPass_CheckedChanged(object sender, EventArgs e)
        {
            txtPass.PasswordChar = chkShowPass.Checked ? '\0' : '*';
        }

        //private void cbbMode_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if(cbbMode.SelectedIndex == 0)
        //    {
        //        Mode = 0x01;

        //    }
        //    else if(cbbMode.SelectedIndex == 1)
        //    {
        //        Mode = 0x02;
        //    }
        //}

        public void saveSetting(ref byte[] data_save)
        {
            for (int i = 0; i < wifi_pass.Length; i++)
            {
                if (wifi_pass != "")
                {
                    data_save[i] = Encoding.ASCII.GetBytes(wifi_pass.Substring(i, 1))[0];
                }
            }


            for (int i = 0; i < ssid.Length; i++)
            {
               if(ssid != "")
               {
                   data_save[i + 32] = Encoding.ASCII.GetBytes(ssid.Substring(i, 1))[0];
               }
            }


            for (int i = 0; i < ip.Length; i++)
            {
                if(ip != "")
                {
                    data_save[i + 64] = Encoding.ASCII.GetBytes(ip.Substring(i, 1))[0];
                }
            }


            for (int i = 0; i < cloud_port.Length; i++)
            {
                if(cloud_port != "")
                {
                    data_save[i + 83] = Encoding.ASCII.GetBytes(cloud_port.Substring(i, 1))[0];
                }
            }

            for (int i = 0; i < cloud_ip.Length; i++)
            {
                if (cloud_port != "")
                {
                    data_save[i + 100] = Encoding.ASCII.GetBytes(cloud_ip.Substring(i, 1))[0];
                }
            }

            for (int i = 0; i < password.Length; i++)
            {
                if (cloud_port != "")
                {
                    data_save[i + 120] = Encoding.ASCII.GetBytes(password.Substring(i, 1))[0];
                }
            }


            data_save[88] = byte.Parse(Mode.ToString());
        }

        public void saveTextSetting(ref string setting)
        {
            setting += string.Format("Pass: {0}{1}", wifi_pass, Environment.NewLine);
            setting += string.Format("Name: {0}{1}", ssid, Environment.NewLine);
            setting += string.Format("TcpIP: {0}{1}", ip, Environment.NewLine);
            setting += string.Format("Port: {0}{1}", cloud_port, Environment.NewLine);
            if (Mode == 0x01)
            {
                setting += "Mode: 1" + Environment.NewLine;
            }
            else
            {
                setting += "Mode: 2" + Environment.NewLine;
            }
            setting += string.Format("UserName: {0}{1}", cloud_ip, Environment.NewLine);
            setting += string.Format("Password: {0}{1}", password, Environment.NewLine);
            setting += string.Format("StaticIP: {0}{1}", staticip, Environment.NewLine);
            setting += string.Format("Netmask: {0}{1}", staticnetmask, Environment.NewLine);
            setting += string.Format("Gateway: {0}{1}", staticgateway, Environment.NewLine);
        }

        public void readTextSetting(string setting)
        {
            string[] lines = Regex.Split(setting, Environment.NewLine);

            wifi_pass = lines[0].Substring(6, lines[0].Length - 6);
            ssid = lines[1].Substring(6, lines[1].Length - 6);
            ip = lines[2].Substring(7, lines[2].Length - 7);
            cloud_port = lines[3].Substring(6, lines[3].Length - 6);
            if(lines[4].Substring(6, 1) == "1")
            {
                Mode = 0x01;
            }
            else
            {
                Mode = 0x02;
            }
            cloud_ip = lines[5].Substring(10, lines[5].Length - 10);
            password = lines[6].Substring(10, lines[6].Length - 10);
            staticip = lines[7].Substring(10, lines[7].Length - 10);
            staticnetmask = lines[8].Substring(9, lines[8].Length - 9);
            staticgateway = lines[9].Substring(9, lines[9].Length - 9);
        }

        private void _34Setting_FormClosed(object sender, FormClosedEventArgs e)
        {
            string FileName = "";
            FileName = mGlobal.app_patch(FileName);

            FileName += "\\dataSetting34.txt";

            string setting = "";
            saveTextSetting(ref setting);

            FileStream fs = null;

            if (System.IO.File.Exists(FileName))
            {
                fs = new FileStream(FileName, FileMode.Truncate, FileAccess.Write);
            }
            else
            {
                fs = new FileStream(FileName, FileMode.OpenOrCreate, FileAccess.Write);
            }
           
            using (StreamWriter sw = new StreamWriter(fs))
            {
                sw.Write(setting);
            }
        }

        private void cbbNetWork_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtPass.Text = "";
        }

        private void txtUsername_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8)
            {
                if (txtPass.Text.Length > 60)
                {
                    e.Handled = true;
                    ToolTip t = new ToolTip();
                    t.SetToolTip(txtUsername, "Your email cannot be over 60 characters");
                }
            }
        }

        private void btnFirmware_Click(object sender, EventArgs e)
        {
            dev34 = Device.DelInstance();
            dev34 = Device.Instance;
            if (dev34.USBOpen(hostPort) != true)
            {
                MessageBox.Show(res_man.GetString("Open", cul) + " USB " + res_man.GetString("fail", cul) + ". " + res_man.GetString("Please try again", cul));
                dev34.Close();
                this.Close();
                return;
            }

            bool readfirm = ReadFirmware();
            if (readfirm != true)
            {
                //MessageBox.Show(res_man.GetString("Fail to read firmware", cul));
                lblFirmware.Text = "";
                MessageBox.Show("Fail to read firmware");
            }
            else
            {
                //lblFirmware.Text = "";
            }
            dev34.Close();
        }

        private bool ReadFirmware()
        {
            byte[] bufFirmware = new byte[64];
            short res = 0;

            bufFirmware[0] = 0x01;
            bufFirmware[1] = 0x08;
            bufFirmware[2] = 0x01;

            Thread.Sleep(200);
            res = HIDFunction.hid_Write(dev34.dev, ref bufFirmware[0], 65);
            if (res < 0)
            {
                //HIDFunction.hid_Error(dev34.dev, ref bufMsg[0]);
                //string b = "";
                //for (int i = 0; i < bufMsg.Length; i++)
                //{
                //    char a = (char)bufMsg[i];
                //    b += a.ToString();
                //}
                return false;
            }
            Thread.Sleep(200);

            res = HIDFunction.hid_Read(dev34.dev, ref bufFirmware[0], 65);
            if (res < 0) return false;
            //dev34.
            lblFirmware.Text = "Firmware version: " + bufFirmware[2] + "." + bufFirmware[3];
            return true;
        }

        private void rdodhcp_CheckedChanged(object sender, EventArgs e)
        {
            //txtStaticIP.Text = "0.0.0.0";
            //txtGateway.Text = "0.0.0.0";
            VisibleDHCP(false);
        }
        void VisibleDHCP(bool visible)
        {
            label8.Enabled = visible;
            label10.Enabled = visible;
            label12.Enabled = visible;
            txtStaticIP.Enabled = visible;
            txtNetmask.Enabled = visible;
            txtGateway.Enabled = visible;
        }

        private void rdostaticip_CheckedChanged(object sender, EventArgs e)
        {
            VisibleDHCP(true);
        }

        private void btnRead_Click(object sender, EventArgs e)
        {
            dev34 = Device.DelInstance();
            dev34 = Device.Instance;
            try
            {
                if (dev34.USBOpen(hostPort) != true)
                {
                    MessageBox.Show(res_man.GetString("Open", cul) + " USB " + res_man.GetString("fail", cul) + " . " + res_man.GetString("Please try again", cul));
                    dev34.Close();
                    this.Close();
                    return;
                }
                ReadAllData();
                dev34.Close();
                cbbNetWork.Text = ssid;
                txtPass.Text = wifi_pass;
                chkShowPass.Checked = true;
                if (enableDHCP)
                {
                    rdodhcp.Checked = true;
                    txtStaticIP.Text = staticip;
                    txtNetmask.Text = "";
                    txtGateway.Text = "";
                }
                else
                {
                    rdostaticip.Checked = true;
                    txtStaticIP.Text = staticip;
                    txtNetmask.Text = staticnetmask;
                    txtGateway.Text = staticgateway;
                }
                txtIP.Text = cloud_ip;
                txtPort.Text = cloud_port;
            }
            catch
            {
                MessageBox.Show("Read Fail");
                return;
            }
            finally
            {
                dev34.Close();
            }
           
        }
        //private void txtPassword_KeyPress(object sender, KeyPressEventArgs e)
        //{
        //    if (e.KeyChar != 8)
        //    {
        //        if (txtPass.Text.Length > 19)
        //        {
        //            e.Handled = true;
        //            ToolTip t = new ToolTip();
        //            t.SetToolTip(txtPassword, "Pass cannot be over 20 characters");
        //        }
        //    }
        //}

    }
}
