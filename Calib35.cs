using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.Resources;
using System.Threading;

namespace Pexo16
{
    public partial class Calib35 : Form
    {
        Device35 dv35 = null;

        int g_channel = 0;

        string host = "";

        struct Chan
        {
            public int data;
            public int dau;
            public byte unit;
        }

        Chan[] channels = new Chan[4];


        ResourceManager res_man = new ResourceManager("Pexo16.Lang.Resources", typeof(Calib35).Assembly);
        CultureInfo cul;


        public Calib35(string hostPort)
        {
            host = hostPort;
            InitializeComponent();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar) || Char.IsControl(e.KeyChar)) { }

            else if (e.KeyChar == '.' && txtDataOffset.Text.IndexOf('.'.ToString()) == -1) { }

            else if (e.KeyChar == '-' && txtDataOffset.Text == "") { }

            else
            {
                e.Handled = true;
            }

            if (txtDataOffset.Text.IndexOf('.') != -1 && txtDataOffset.Text.IndexOf('.') == txtDataOffset.Text.Length - 2)
            {
                if (!Char.IsControl(e.KeyChar))
                    e.Handled = true;
            }
        }

        private void txtDataOffset_TextChanged(object sender, EventArgs e)
        {
            if (txtDataOffset.Text == "0" || txtDataOffset.Text == "")
            {
                channels[g_channel].data = 0;
            }
            else
            {
                try
                {
                    int ph = 0;
                    if (txtDataOffset.Text.IndexOf(".") > 0)
                    {
                        ph = txtDataOffset.Text.IndexOf(".");
                    }
                    else
                    {
                        ph = txtDataOffset.Text.Length;
                    }

                    int nguyen = 0;
                    int thPhan = 0;
                    if (ph == 0)
                    {
                        nguyen = 0;
                    }
                    else
                    {
                        nguyen = Int32.Parse(txtDataOffset.Text.Substring(0, ph));
                    }
                    if (ph < txtDataOffset.Text.Length)
                    {
                        thPhan = Int32.Parse(txtDataOffset.Text.Substring(ph + 1, 1));
                    }
                    else
                    {
                        thPhan = 0;
                    }

                    if (nguyen < 0)
                    {
                        thPhan = -thPhan;
                    }
                        
                    int tmp = nguyen * 10 + thPhan;

                    //if (channels[g_channel].unit == 175)
                    //{
                    //    float tamp = tmp / 10;
                    //    tamp = Convert.ToSingle((tamp / 1.8).ToString("0.0"));
                    //    tmp = Int32.Parse((tamp * 10).ToString());
                    //}

                    if (tmp >= 255)
                    {
                        MessageBox.Show(res_man.GetString("Data offset cannot over 25.5", cul));
                        txtDataOffset.Text = "";
                        return;
                    }
                    else 
                    {
                        if (tmp < 0)
                        {
                            channels[g_channel].dau = 1;
                            tmp = -tmp;
                            channels[g_channel].data = tmp;
                        }
                        else
                        {
                            channels[g_channel].dau = 0;
                            channels[g_channel].data = tmp;
                        }
                    }
                    //channels[g_channel].data = nguyen * 10 + thPhan;
                }
                catch (Exception) { }
            }
        }

        private void Calib35_Load(object sender, EventArgs e)
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

            label1.Text = res_man.GetString("Chanel", cul);
            label4.Text = res_man.GetString("Unit", cul);
            label2.Text = res_man.GetString("Data offset", cul);
            btnSetting.Text = res_man.GetString("Write Setting", cul);


            cbbChannel.Items.Clear();

            string channel1 = "";
            string channel2 = "";
            string channel3 = "";
            string channel4 = "";
 
            dv35 = Device35.DelInstance();
            dv35 = Device35.Instance;
            dv35.Channels = new Channel[4];
            for (int i = 0; i < 4; i++)
            {
                dv35.Channels[i] = new Channel();
            }

            dv35.USBOpen(host);

            //HIDFunction.hid_SetNonBlocking(dv35.dev, 1);

            Thread.Sleep(200);

            byte[] buf = new byte[64];
            dv35.readSettingDevice();
            if(dv35.byteLogging == 68)
            {
                MessageBox.Show("Logger is recording. Please stop to calibrate");
                dv35.Close();
                this.Close();
            }
            dv35.readInfo4Device(ref buf);
            Thread.Sleep(2000);
            dv35.readSettingChannel();

            dv35.Close();
           
            byte[] temp1 = new byte[7];
            for (int i = 0; i < 7; i++)
            {
                if (buf[2 + i] != 0)
                {
                    temp1[i] = buf[2 + i];
                }
            }
            channel1 += Encoding.UTF8.GetString(temp1);

            byte[] temp2 = new byte[7];
            for (int i = 0; i < 7; i++)
            {
                if (buf[9 + i] != 0)
                {
                    temp2[i] = buf[9 + i];
                }
            }
            channel2 += Encoding.UTF8.GetString(temp2);

            byte[] temp3 = new byte[7];
            for (int i = 0; i < 7; i++)
            {
                if (buf[16 + i] != 0)
                {
                    temp3[i] = buf[16 + i];
                }
            }
            channel3 += Encoding.UTF8.GetString(temp3);

            byte[] temp4 = new byte[7];
            for (int i = 0; i < 7; i++)
            {
                if (buf[23 + i] != 0)
                {
                    temp4[i] = buf[23 + i];
                }
            }
            channel4 += Encoding.UTF8.GetString(temp4);

            if(channel1 == "PEXO-37")
            {
                cbbChannel.Items.Add("1");
                channels[0].unit = dv35.Channels[0].Unit;
            }
            else if (channel1 == "PEXO-40")
            {
                cbbChannel.Items.Add("1");
                channels[0].unit = 2;
            }

            if (channel2 == "PEXO-37")
            {
                cbbChannel.Items.Add("2");
                channels[1].unit = dv35.Channels[1].Unit;
            }
            else if (channel2 == "PEXO-40")
            {
                cbbChannel.Items.Add("2");
                channels[1].unit = 2;
            }

            if (channel3 == "PEXO-37")
            {
                cbbChannel.Items.Add("3");
                channels[2].unit = dv35.Channels[2].Unit;
            }
            else if (channel3 == "PEXO-40")
            {
                cbbChannel.Items.Add("3");
                channels[2].unit = 2;
            }

            if (channel4 == "PEXO-37")
            {
                cbbChannel.Items.Add("4");
                channels[3].unit = dv35.Channels[3].Unit;
            }
            else if (channel4 == "PEXO-40")
            {
                cbbChannel.Items.Add("4");
                channels[3].unit = 2;
            }

            //cbbChannel.Text = cbbChannel.Items[0].ToString();
            //cbbSign.Text = cbbSign.Items[0].ToString();
            cbbUnit.DropDownStyle = ComboBoxStyle.DropDownList;

            if (cbbUnit.Items.Count > 0)
            {
                cbbUnit.Text = cbbUnit.Items[0].ToString();
            }
            if (cbbChannel.Items.Count > 0)
            {
                cbbChannel.Text = cbbChannel.Items[0].ToString();
            }
            
        }

        private void btnSetting_Click(object sender, EventArgs e)
        {
            if(cbbChannel.Text == "")
            {
                MessageBox.Show(res_man.GetString("Please choose one of channels", cul));
                return;
            }
            if(txtDataOffset.Text == "")
            {
                MessageBox.Show(res_man.GetString("Please set value to calib", cul));
                return;
            }

            
            int index;
            index = Int32.Parse(cbbChannel.SelectedItem.ToString()) - 1;
            byte channel = byte.Parse((index + 1).ToString());
            byte data = byte.Parse(channels[index].data.ToString());
            byte dau = byte.Parse(channels[index].dau.ToString());
            if(channels[index].unit == 175)
            {
                float tamp = data / 10;
                tamp = Convert.ToSingle((tamp / 1.8).ToString("0.0"));
                data = byte.Parse((tamp * 10).ToString());
            }

            dv35.USBOpen(host);
            //HIDFunction.hid_SetNonBlocking(dv35.dev, 1);
            Thread.Sleep(2000);
            if (!dv35.writeCalibOffset(channel, data, dau))
            {
                MessageBox.Show(res_man.GetString("Setting Calib fail", cul));
                dv35.Close();
                return;
            }
            else
            {
                MessageBox.Show(res_man.GetString("Setting successful", cul));
                dv35.Close();
            }

            btnSetting.Enabled = false;
            btnSetting.Refresh();
            Thread.Sleep(4000);
            btnSetting.Enabled = true;

            //for (int i = 0; i < cbbChannel.Items.Count; i++)
            //{
            //    index = Int32.Parse(cbbChannel.Items[i].ToString()) - 1;
            //    byte channel = byte.Parse((index + 1).ToString());
            //    byte data = byte.Parse(channels[index].data.ToString());
            //    byte dau = byte.Parse(channels[index].dau.ToString());
            //    if(!dv35.writeCalibOffset(channel, data, dau))
            //    {
            //        MessageBox.Show("Setting Calib fail!");
            //        dv35.Close();
            //        return;
            //    }
            //}
            //MessageBox.Show("Successfully!");
            //dv35.Close();
        }

        private void cbbChannel_SelectedIndexChanged(object sender, EventArgs e)
        {
            g_channel = Int32.Parse(cbbChannel.SelectedItem.ToString()) - 1;
            txtDataOffset.Text = "";
            if (channels[g_channel].unit == 175)
            {
                cbbUnit.Enabled = true;
                //lblUnit.Text = res_man.GetString("Temperature", cul) + " (F)";
                lblUnit.Text = " F";
                cbbUnit.SelectedIndex = 1;
                cbbUnit.Text = cbbUnit.Items[1].ToString();
            }
            else if(channels[g_channel].unit == 172)
            {
                cbbUnit.Enabled = true;
                //lblUnit.Text = res_man.GetString("Temperature",cul) + " (C)";
                lblUnit.Text = " C";
                cbbUnit.SelectedIndex = 0;
                cbbUnit.Text = cbbUnit.Items[0].ToString();
            } 
            else if(channels[g_channel].unit == 2)
            {
                //lblUnit.Text = res_man.GetString ("Humidity", cul);
                lblUnit.Text = "%RH";
                cbbUnit.Enabled = false;
                //cbbUnit.Visible = false;
            }
            //lblUnit.Text = mGlobal.IntToUnit35(channels[g_channel].unit);
        }

        private void cbbUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cbbUnit.SelectedIndex == 0)
            {
                if (channels[g_channel].unit == 1 || channels[g_channel].unit == 175)
                {
                    channels[g_channel].unit = 172;
                }
            }
            else if(cbbUnit.SelectedIndex == 1)
            {
                if (channels[g_channel].unit == 1 || channels[g_channel].unit == 172)
                {
                    channels[g_channel].unit = 175;
                }
            }
        }


        //private void cbbSign_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if(cbbSign.SelectedItem.ToString() == "+")
        //    {
        //        channels[g_channel].dau = 0;
        //    }
        //    else
        //    {
        //        channels[g_channel].dau = 1;
        //    }
        //}
    }
}
