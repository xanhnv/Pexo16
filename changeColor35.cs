using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Resources;
using System.Globalization;

namespace Pexo16
{
    public partial class formC : Form
    {
        Device35 dv35 = null;
        Graph graph35 = null;
        Label[] lblCH = null;
        Label[] lblColor = null;
        Button[] btnChange = null;

        ResourceManager res_man = new ResourceManager("Pexo16.Lang.Resources", typeof(formC).Assembly);
        CultureInfo cul;

        public formC()
        {
            InitializeComponent();
        }

        private void changeColor35_Load(object sender, EventArgs e)
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

            btnOK.Text = res_man.GetString("Ok", cul);
            
            dv35 = Device35.Instance;
            graph35 = Graph.Instance;

            int dem = 0;
            for (int i = 0; i < dv35.numOfChannel; i++)
            {
                if(dv35.Channels[i].Unit != 0)
                {
                    dem += 1;
                }
            }

            lblCH = new Label[dv35.numOfChannel];
            lblColor = new Label[dv35.numOfChannel];
            btnChange = new Button[dem];
            int tmp = 0;

            int channel = 1;
            int d = 0;
            for (int i = 0; i < dv35.numOfChannel; i++)
            {
                if(dv35.Channels[i].Unit != 0)
                {
                    lblCH[i] = new Label();

                    if (dv35.Channels[i].Unit == 3)
                    {
                        switch(d)
                        { 
                            case 0:
                                lblCH[i].Text = res_man.GetString("Chanel", cul) + " " + (channel) + " X";
                                break;
                            case 1 :
                                lblCH[i].Text = res_man.GetString("Chanel", cul) + " " + (channel) + " Y";
                                break;
                            case 2:
                                lblCH[i].Text = res_man.GetString("Chanel", cul) + " " + (channel) + " Z";
                                channel += 1;
                                d = -1;
                                break;
                        }
                        d += 1;
                    }
                    else
                    {
                        lblCH[i].Text = res_man.GetString("Chanel", cul) +" "+ (channel);
                        channel += 1;
                    }
               
                    lblCH[i].Left = 36;
                    lblCH[i].Top = 30 + 55 * tmp;
                    lblCH[i].Visible = true;
                    this.Controls.Add(lblCH[i]);

                    lblColor[i] = new Label();
                    lblColor[i].Name = "Channel " + (channel) + ":";
                    lblColor[i].BackColor = graph35.chart1.Series[tmp].Color;
                    lblColor[i].Width = 80;
                    lblColor[i].Height = 25;
                    lblColor[i].Left = 150;
                    lblColor[i].Top = 30 + 55 * tmp;
                    lblColor[i].Visible = true;
                    this.Controls.Add(lblColor[i]);

                    Button btn = new Button();
                    btn.Text = res_man.GetString("Change", cul);
                    btn.Top = 29 + 55 * tmp;
                    btn.Left = 300;
                    btn.Name = "btnChange" + channel;
                    btn.Tag = i;
                    btn.Click += new EventHandler(this.Buttons_Click);
                    this.Controls.Add(btn);

                    this.Height =  200 + 55 * tmp;
                    btnOK.Top = 100 + 55 * tmp;

                    tmp += 1;
                }
                else
                {
                    channel += 1;
                }
            }
        }

        private void Buttons_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            colorDialog1.AllowFullOpen = true;
            colorDialog1.AnyColor = true;
            colorDialog1.SolidColorOnly = false;

            if (colorDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                lblColor[(int)button.Tag].BackColor = colorDialog1.Color;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            mGlobal.ColorChanged = true;
            int tmp = 0;
            for (int i = 0; i < dv35.numOfChannel; i++)
            {
                if (dv35.Channels[i].Unit != 0)
                {
                    dv35.Channels[i].LineColor = lblColor[i].BackColor;
                    graph35.chart1.Series[i - tmp].Color = dv35.Channels[i].LineColor;
                }
                else
                {
                    tmp += 1;
                }
            }
            this.Close();
        }
    }
}
