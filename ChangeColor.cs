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
    public partial class ChangeColor : Form
    {
        Graph graph = Graph.Instance;
        Device deviceFromFile = Device.Instance;

        public ChangeColor()
        {
            InitializeComponent();
        }

        private void btnCh1_Click(object sender, EventArgs e)
        {
            colorDialog1.AllowFullOpen = true;
            colorDialog1.AnyColor = true;
            colorDialog1.SolidColorOnly = false;

            if (colorDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                lbl_Ch1.BackColor = colorDialog1.Color;
            }
        }

        private void btnCh2_Click(object sender, EventArgs e)
        {
            colorDialog2.AllowFullOpen = true;
            colorDialog2.AnyColor = true;
            colorDialog2.SolidColorOnly = false;

            if (colorDialog2.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                lbl_Ch2.BackColor = colorDialog2.Color;
            }
        }

        private void btnCh3_Click(object sender, EventArgs e)
        {
            colorDialog3.AllowFullOpen = true;
            colorDialog3.AnyColor = true;
            colorDialog3.SolidColorOnly = false;

            if (colorDialog3.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                lbl_Ch3.BackColor = colorDialog3.Color;
            }
        }

        private void btnCh4_Click(object sender, EventArgs e)
        {
            colorDialog4.AllowFullOpen = true;
            colorDialog4.AnyColor = true;
            colorDialog4.SolidColorOnly = false;

            if (colorDialog4.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                lbl_Ch4.BackColor = colorDialog4.Color;
            }
        }

        private void btnCh5_Click(object sender, EventArgs e)
        {
            colorDialog5.AllowFullOpen = true;
            colorDialog5.AnyColor = true;
            colorDialog5.SolidColorOnly = false;

            if (colorDialog5.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                lbl_Ch5.BackColor = colorDialog5.Color;
            }
        }

        private void btnCh6_Click(object sender, EventArgs e)
        {
            colorDialog6.AllowFullOpen = true;
            colorDialog6.AnyColor = true;
            colorDialog6.SolidColorOnly = false;

            if (colorDialog6.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                lbl_Ch6.BackColor = colorDialog6.Color;                
            }
        }

        private void btnCh7_Click(object sender, EventArgs e)
        {
            colorDialog7.AllowFullOpen = true;
            colorDialog7.AnyColor = true;
            colorDialog7.SolidColorOnly = false;

            if (colorDialog7.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                lbl_Ch7.BackColor = colorDialog7.Color;
            }
        }

        private void btnCh8_Click(object sender, EventArgs e)
        {
            colorDialog8.AllowFullOpen = true;
            colorDialog8.AnyColor = true;
            colorDialog8.SolidColorOnly = false;

            if (colorDialog8.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                lbl_Ch8.BackColor = colorDialog8.Color;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            deviceFromFile.channels[0].LineColor = lbl_Ch1.BackColor;
            deviceFromFile.channels[1].LineColor = lbl_Ch2.BackColor;
            deviceFromFile.channels[2].LineColor = lbl_Ch3.BackColor;
            deviceFromFile.channels[3].LineColor = lbl_Ch4.BackColor;
            deviceFromFile.channels[4].LineColor = lbl_Ch5.BackColor;
            deviceFromFile.channels[5].LineColor = lbl_Ch6.BackColor;
            deviceFromFile.channels[6].LineColor = lbl_Ch7.BackColor;
            deviceFromFile.channels[7].LineColor = lbl_Ch8.BackColor;

            mGlobal.ColorChanged = true;
            //for (int i = 0; i < deviceFromFile.numOfChannel; i++)
            //{
            //    graph.chart1.Series[i].Color = deviceFromFile.channels[i].LineColor;
            //}
            for (int i = 0; i < graph.chart1.Series.Count; i++)
            {
                int index = Int32.Parse(graph.chart1.Series[i].Name.Substring(8, 1));
                graph.chart1.Series[i].Color = deviceFromFile.channels[index - 1].LineColor;
            }
            this.Close();
        }

        private void frmChangeColor_Load(object sender, EventArgs e)
        {
            lbl_Ch1.BackColor = deviceFromFile.channels[0].LineColor;
            lbl_Ch2.BackColor = deviceFromFile.channels[1].LineColor;
            lbl_Ch3.BackColor = deviceFromFile.channels[2].LineColor;
            lbl_Ch4.BackColor = deviceFromFile.channels[3].LineColor;
            lbl_Ch5.BackColor = deviceFromFile.channels[4].LineColor;
            lbl_Ch6.BackColor = deviceFromFile.channels[5].LineColor;
            lbl_Ch7.BackColor = deviceFromFile.channels[6].LineColor;
            lbl_Ch8.BackColor = deviceFromFile.channels[7].LineColor;

            if (deviceFromFile.channels[0].Unit != 0)
                btnCh1.Enabled = true;
            else
            {
                btnCh1.Enabled = false;
                lbl_Ch1.BackColor = Control.DefaultBackColor;
                lbl_Ch1.Text = "Not use";
            }

            if (deviceFromFile.channels[1].Unit != 0)
                btnCh2.Enabled = true;
            else
            {
                btnCh2.Enabled = false;
                lbl_Ch2.BackColor = Control.DefaultBackColor;
                lbl_Ch2.Text = "Not use";
            }

            if (deviceFromFile.channels[2].Unit != 0)
                btnCh3.Enabled = true;
            else
            {
                btnCh3.Enabled = false;
                lbl_Ch3.BackColor = Control.DefaultBackColor;
                lbl_Ch3.Text = "Not use";
            }

            if (deviceFromFile.channels[3].Unit != 0)
                btnCh4.Enabled = true;
            else
            {
                btnCh4.Enabled = false;
                lbl_Ch4.BackColor = Control.DefaultBackColor;
                lbl_Ch4.Text = "Not use";
            }

            if (deviceFromFile.channels[4].Unit != 0)
                btnCh5.Enabled = true;
            else
            {
                btnCh5.Enabled = false;
                lbl_Ch5.BackColor = Control.DefaultBackColor;
                lbl_Ch5.Text = "Not use";
            }

            if (deviceFromFile.channels[5].Unit != 0)
                btnCh6.Enabled = true;
            else
            {
                btnCh6.Enabled = false;
                lbl_Ch6.BackColor = Control.DefaultBackColor;
                lbl_Ch6.Text = "Not use";
            }

            if (deviceFromFile.channels[6].Unit != 0)
                btnCh7.Enabled = true;
            else
            {
                btnCh7.Enabled = false;
                lbl_Ch7.BackColor = Control.DefaultBackColor;
                lbl_Ch7.Text = "Not use";
            }

            if (deviceFromFile.channels[7].Unit != 0)
                btnCh8.Enabled = true;
            else
            {
                btnCh8.Enabled = false;
                lbl_Ch8.BackColor = Control.DefaultBackColor;
                lbl_Ch8.Text = "Not use";
            }
        }

    }
}
