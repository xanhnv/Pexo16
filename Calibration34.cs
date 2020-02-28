using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pexo16
{
    public partial class Calibration34 : Form
    {
        public Calibration34(string hostPost)
        {
            host = hostPost;
            InitializeComponent();
        }
        private Device35 dv35;
        string host;
        private List<DataCalibs> channels;
        double a, b, c, d;
        double detA, detB, detC, detD;
        double DetT;
        //double[,] matrix;// = new double[4, 4];
        //double[,] matrixa;//= new double[4, 4];
        //double[,] matrixb;//= new double[4, 4];
        //double[,] matrixc;//= new double[4, 4];
        //double[,] matrixd;//= new double[4, 4];
        double T1, T2, T3, T4; //Temp sensor
        double X1, X2, X3, X4;// sensor rH
        bool calibMode = false;
        List<RadioButton> rdbGroup1 = new List<RadioButton>();
        private void btnReload_Click(object sender, EventArgs e)
        {
            if (ReadInfoDatacalib())
            {
                cbbChannel.Items.Clear();
                for (int i = 0; i < channels.Count; i++)
                {
                    if (channels[i].sensor != Sensor.No_sensor)
                    {
                        cbbChannel.Items.Add(i + 1);
                    }
                }
                if (cbbChannel.Items.Count > 0)
                {
                    cbbChannel.SelectedIndex = 0;
                }
                else
                {
                    //grbCalTemp1.Enabled = false;
                    grbCalHum1.BringToFront();
                    grbCalHum1.Visible = false;
                    grbCalHum1.Enabled = false;
                    grbCalTemp1.Visible = false;
                    btnReadProb1.Enabled = false;
                    btnCalProb1.Enabled = false;
                    cbbChannel.Text = "";
                }
                btnReload.Enabled = false;
                btnReload.Refresh();
                Thread.Sleep(1000);
                btnReload.Enabled = true;
            }
            else
            {
                MessageBox.Show("Select calib mode to continue!");
                this.Close();
                return;
            }
           
        }

        private void cbbChannel_SelectedIndexChanged(object sender, EventArgs e)
        {
            //int i = cbbChannel.SelectedIndex;
            int i = (int)cbbChannel.SelectedItem-1;
            if (channels[i].sensor == Sensor.Temp_sensor)
            {
                grbCalTemp1.BringToFront();
                grbCalTemp1.Enabled = true;
                grbCalTemp1.Enabled = true;
                grbCalTemp1.Visible = true;
                grbCalTemp1.Text = grbCalTemp1.Text.Replace('1', (i + 1).ToString().ToCharArray()[0]);
                grbCalHum1.SendToBack();
                grbCalHum1.Visible = true;

                btnReadProb1.Visible = false;
                btnCalProb1.Enabled = true;
            }
            else if (channels[i].sensor == Sensor.Humid_sensor)
            {
                cbbNumCalPoint1.SelectedIndex = 1;
                btnReadProb1.Visible = true;
                btnReadProb1.Enabled = true;
                btnCalProb1.Enabled = true;
                grbCalHum1.BringToFront();
                grbCalHum1.Visible = true;
                grbCalHum1.Enabled = true;
                grbCalHum1.Text = grbCalHum1.Text.Replace('1', (i + 1).ToString().ToCharArray()[0]);
                grbCalTemp1.SendToBack();
                grbCalTemp1.Visible = false;
                //Fill value
                textBox8.Text = channels[i].temp.ToString();
                textBox7.Text = channels[i].rHNoOffset.ToString();
                textBox6.Text = "";
                textBox5.Text = "";
                textBox4.Text = "";
                textBox3.Text = "";
                textBox2.Text = "";
                textBox1.Text = "";
                txtRefHum1Prob1.Text = "";
                txtRefHum2Prob1.Text = "";
                txtRefHum3Prob1.Text = "";
                txtRefHum4Prob1.Text = "";
            }
        }
        private void cbbNumofCalPoint_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cbbNumCalPoint1.SelectedIndex)
            {
                case 0:
                    panel5.Enabled = false;
                    panel6.Enabled = false;
                    panel7.Enabled = false;
                    textBox1.Text = "";
                    textBox2.Text = "";
                    textBox3.Text = "";
                    textBox4.Text = "";
                    textBox5.Text = "";
                    textBox6.Text = "";
                    txtRefHum2Prob1.Text = "";
                    txtRefHum3Prob1.Text = "";
                    txtRefHum4Prob1.Text = "";
                    radioButton1.Checked = true;
                    radioButton2.Checked = false;
                    radioButton3.Checked = false;
                    radioButton4.Checked = false;
                    break;
                case 1:
                    panel5.Enabled = false;
                    panel6.Enabled = false;
                    panel7.Enabled = true;
                    radioButton1.Checked = false;
                    radioButton2.Checked = true;
                    radioButton3.Checked = false;
                    radioButton4.Checked = false;
                    textBox1.Text = "";
                    textBox2.Text = "";
                    textBox3.Text = "";
                    textBox4.Text = "";
                    txtRefHum3Prob1.Text = "";
                    txtRefHum4Prob1.Text = "";
                    break;
                case 2:
                    panel5.Enabled = false;
                    panel6.Enabled = true;
                    panel7.Enabled = true;
                    radioButton1.Checked = false;
                    radioButton2.Checked = false;
                    radioButton3.Checked = true;
                    radioButton4.Checked = false;
                    textBox1.Text = "";
                    textBox2.Text = "";
                    txtRefHum4Prob1.Text = "";
                
                    break;
                case 3:
                    panel5.Enabled = true;
                    panel6.Enabled = true;
                    panel7.Enabled = true;
                    radioButton1.Checked = false;
                    radioButton2.Checked = false;
                    radioButton3.Checked = false;
                    radioButton4.Checked = true;
                   
                    break;
                default:
                    panel5.Enabled = true;
                    panel6.Enabled = true;
                    panel7.Enabled = true;
                    radioButton1.Checked = true;
                    radioButton2.Checked = false;
                    radioButton3.Checked = false;
                    radioButton4.Checked = false;
                    break;
            }
        }

        private void btnReadProb1_Click(object sender, EventArgs e)
        {
            if (!ReadInfoDatacalib())
            {
                MessageBox.Show("Select calib mode to continue!");
                this.Close();
                return;
            };
            int nosensor = 0;
            for (int i = 0; i < channels.Count; i++)
            {
                if (channels[i].sensor == Sensor.No_sensor)
                {
                    nosensor++;
                }
            }
            if (nosensor==4)
            {
                cbbChannel.Items.Clear();

                MessageBox.Show("Not found any probe!");
                grbCalHum1.BringToFront();
                grbCalHum1.Visible = false;
                grbCalHum1.Enabled = false;
                grbCalTemp1.Visible = false;
                btnReadProb1.Enabled = false;
                btnCalProb1.Enabled = false;
                cbbChannel.Text = "";
                return;
            }
            int rdg1Checked = 0;
            int selectedChennel = (int)cbbChannel.SelectedItem-1;
            for (int r = 0; r < 4; r++)
            {
                if (rdbGroup1[r].Checked)
                {
                    rdg1Checked = r;
                    break;
                }
            }

            switch (rdg1Checked)
            {
                case 0:
                    textBox8.Text = channels[selectedChennel].temp.ToString();
                    textBox7.Text = channels[selectedChennel].rHNoOffset.ToString();
                    break;
                case 1:
                    textBox6.Text = channels[selectedChennel].temp.ToString();
                    textBox5.Text = channels[selectedChennel].rHNoOffset.ToString();
                    break;
                case 2:
                    textBox4.Text = channels[selectedChennel].temp.ToString();
                    textBox3.Text = channels[selectedChennel].rHNoOffset.ToString();
                    break;
                case 3:
                    textBox2.Text = channels[selectedChennel].temp.ToString();
                    textBox1.Text = channels[selectedChennel].rHNoOffset.ToString();
                    break;
            }
            //rdg1Checked++;
            //switch (cbbNumCalPoint1.SelectedIndex)
            //{
            //    case 0:
            //        if (rdg1Checked == 1)
            //        {
            //            rdg1Checked = 0;
            //        }
            //        break;
            //    case 1:
            //        if (rdg1Checked == 2)
            //        {
            //            rdg1Checked = 0;
            //        }
            //        break;
            //    case 2:
            //        if (rdg1Checked == 3)
            //        {
            //            rdg1Checked = 0;
            //        }
            //        break;
            //    case 3:
            //        if (rdg1Checked == 4)
            //        {
            //            rdg1Checked = 0;
            //        }
            //        break;
            //}


            //rdbGroup1[rdg1Checked].Checked = true;
            btnReadProb1.Enabled = false;
            btnCalProb1.Enabled = false;
            btnReadProb1.Refresh();
            btnCalProb1.Refresh();
            Thread.Sleep(1500);
            btnReadProb1.Enabled = true;
            btnCalProb1.Enabled = true;
        }
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            if (rb.Checked)
            {
                foreach (RadioButton other in rdbGroup1)
                {
                    if (other == rb)
                    {
                        continue;
                    }
                    other.Checked = false;
                }
            }

        }
        
        private void txtRefHum1Prob1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar) || Char.IsControl(e.KeyChar)) { }

            else if (e.KeyChar == '.' && txtRefHum1Prob1.Text.IndexOf('.'.ToString()) == -1) { }

            else if (e.KeyChar == '-' && txtRefHum1Prob1.Text == "") { }

            else
            {
                e.Handled = true;
            }

            if (txtRefHum1Prob1.Text.IndexOf('.') != -1 && txtRefHum1Prob1.Text.IndexOf('.') == txtRefHum1Prob1.Text.Length - 2)
            {
                if (!Char.IsControl(e.KeyChar))
                    e.Handled = true;
            }
        }

        private void txtRefHum2Prob1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar) || Char.IsControl(e.KeyChar)) { }

            else if (e.KeyChar == '.' && txtRefHum2Prob1.Text.IndexOf('.'.ToString()) == -1) { }

            else if (e.KeyChar == '-' && txtRefHum2Prob1.Text == "") { }

            else
            {
                e.Handled = true;
            }

            if (txtRefHum2Prob1.Text.IndexOf('.') != -1 && txtRefHum2Prob1.Text.IndexOf('.') == txtRefHum2Prob1.Text.Length - 2)
            {
                if (!Char.IsControl(e.KeyChar))
                    e.Handled = true;
            }
        }

        private void txtRefHum3Prob1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar) || Char.IsControl(e.KeyChar)) { }

            else if (e.KeyChar == '.' && txtRefHum3Prob1.Text.IndexOf('.'.ToString()) == -1) { }

            else if (e.KeyChar == '-' && txtRefHum3Prob1.Text == "") { }

            else
            {
                e.Handled = true;
            }

            if (txtRefHum3Prob1.Text.IndexOf('.') != -1 && txtRefHum3Prob1.Text.IndexOf('.') == txtRefHum3Prob1.Text.Length - 2)
            {
                if (!Char.IsControl(e.KeyChar))
                    e.Handled = true;
            }
        }

        private void txtRefHum4Prob1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar) || Char.IsControl(e.KeyChar)) { }

            else if (e.KeyChar == '.' && txtRefHum4Prob1.Text.IndexOf('.'.ToString()) == -1) { }

            else if (e.KeyChar == '-' && txtRefHum4Prob1.Text == "") { }

            else
            {
                e.Handled = true;
            }

            if (txtRefHum4Prob1.Text.IndexOf('.') != -1 && txtRefHum4Prob1.Text.IndexOf('.') == txtRefHum4Prob1.Text.Length - 2)
            {
                if (!Char.IsControl(e.KeyChar))
                    e.Handled = true;
            }
        }
        private void textBox8_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar) || Char.IsControl(e.KeyChar)) { }

            else if (e.KeyChar == '.' && textBox8.Text.IndexOf('.'.ToString()) == -1) { }

            else if (e.KeyChar == '-' && textBox8.Text == "") { }

            else
            {
                e.Handled = true;
            }

            if (textBox8.Text.IndexOf('.') != -1 && textBox8.Text.IndexOf('.') == textBox8.Text.Length - 2)
            {
                if (!Char.IsControl(e.KeyChar))
                    e.Handled = true;
            }
        }

        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar) || Char.IsControl(e.KeyChar)) { }

            else if (e.KeyChar == '.' && textBox6.Text.IndexOf('.'.ToString()) == -1) { }

            else if (e.KeyChar == '-' && textBox6.Text == "") { }

            else
            {
                e.Handled = true;
            }

            if (textBox6.Text.IndexOf('.') != -1 && textBox6.Text.IndexOf('.') == textBox6.Text.Length - 2)
            {
                if (!Char.IsControl(e.KeyChar))
                    e.Handled = true;
            }
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar) || Char.IsControl(e.KeyChar)) { }

            else if (e.KeyChar == '.' && textBox4.Text.IndexOf('.'.ToString()) == -1) { }

            else if (e.KeyChar == '-' && textBox4.Text == "") { }

            else
            {
                e.Handled = true;
            }

            if (textBox4.Text.IndexOf('.') != -1 && textBox4.Text.IndexOf('.') == textBox4.Text.Length - 2)
            {
                if (!Char.IsControl(e.KeyChar))
                    e.Handled = true;
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar) || Char.IsControl(e.KeyChar)) { }

            else if (e.KeyChar == '.' && textBox2.Text.IndexOf('.'.ToString()) == -1) { }

            else if (e.KeyChar == '-' && textBox2.Text == "") { }

            else
            {
                e.Handled = true;
            }

            if (textBox2.Text.IndexOf('.') != -1 && textBox2.Text.IndexOf('.') == textBox2.Text.Length - 2)
            {
                if (!Char.IsControl(e.KeyChar))
                    e.Handled = true;
            }
        }

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar) || Char.IsControl(e.KeyChar)) { }

            else if (e.KeyChar == '.' && textBox5.Text.IndexOf('.'.ToString()) == -1) { }

            else if (e.KeyChar == '-' && textBox5.Text == "") { }

            else
            {
                e.Handled = true;
            }

            if (textBox5.Text.IndexOf('.') != -1 && textBox5.Text.IndexOf('.') == textBox5.Text.Length - 2)
            {
                if (!Char.IsControl(e.KeyChar))
                    e.Handled = true;
            }
        }

        private void textBox7_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar) || Char.IsControl(e.KeyChar)) { }

            else if (e.KeyChar == '.' && textBox7.Text.IndexOf('.'.ToString()) == -1) { }

            else if (e.KeyChar == '-' && textBox7.Text == "") { }

            else
            {
                e.Handled = true;
            }

            if (textBox7.Text.IndexOf('.') != -1 && textBox7.Text.IndexOf('.') == textBox7.Text.Length - 2)
            {
                if (!Char.IsControl(e.KeyChar))
                    e.Handled = true;
            }
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar) || Char.IsControl(e.KeyChar)) { }

            else if (e.KeyChar == '.' && textBox3.Text.IndexOf('.'.ToString()) == -1) { }

            else if (e.KeyChar == '-' && textBox3.Text == "") { }

            else
            {
                e.Handled = true;
            }

            if (textBox3.Text.IndexOf('.') != -1 && textBox3.Text.IndexOf('.') == textBox3.Text.Length - 2)
            {
                if (!Char.IsControl(e.KeyChar))
                    e.Handled = true;
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar) || Char.IsControl(e.KeyChar)) { }

            else if (e.KeyChar == '.' && textBox1.Text.IndexOf('.'.ToString()) == -1) { }

            else if (e.KeyChar == '-' && textBox1.Text == "") { }

            else
            {
                e.Handled = true;
            }

            if (textBox1.Text.IndexOf('.') != -1 && textBox1.Text.IndexOf('.') == textBox1.Text.Length - 2)
            {
                if (!Char.IsControl(e.KeyChar))
                    e.Handled = true;
            }
        }
        private void textBox51_KeyPress(object sender, KeyPressEventArgs e)
        {

        }
        private void btnCalProb1_Click(object sender, EventArgs e)
        {
            bool success = false;
            int selectedChannel = (int)(cbbChannel.SelectedItem) - 1;
            if (channels[selectedChannel].sensor == Sensor.Humid_sensor)
            {
                //Calib Humid
                //1 point 
                if (String.IsNullOrEmpty(textBox8.Text.Trim()))
                {
                    MessageBox.Show("Enter or Click Read value of temperature in Point 1");
                    textBox8.Focus();
                    return;
                }
                if (String.IsNullOrEmpty(textBox7.Text.Trim()))
                {
                    MessageBox.Show("Enter or Click Read value of humiddity in point 1");
                    textBox7.Focus();
                    return;
                }
                if (String.IsNullOrEmpty(txtRefHum1Prob1.Text.Trim()))
                {
                    MessageBox.Show("Referece rH can not be empty");
                    txtRefHum1Prob1.Focus();
                    return;
                }
                switch (cbbNumCalPoint1.SelectedIndex)
                {
                    case 1:
                        //1 point

                        if (String.IsNullOrEmpty(textBox8.Text.Trim()))
                        {
                            MessageBox.Show("Enter or Click Read value of temperature in Point 1");
                            textBox8.Focus();
                            return;
                        }
                        if (String.IsNullOrEmpty(textBox7.Text.Trim()))
                        {
                            MessageBox.Show("Enter or Click Read value of humiddity in point 1");
                            textBox7.Focus();
                            return;
                        }
                        if (String.IsNullOrEmpty(txtRefHum1Prob1.Text.Trim()))
                        {
                            MessageBox.Show("Referece rH can not be empty");
                            txtRefHum1Prob1.Focus();
                            return;
                        }
                        //2 point 
                        //ValidatePoint2();
                        if (String.IsNullOrEmpty(textBox6.Text.Trim()))
                        {
                            MessageBox.Show("Enter or Click Read value of temperature in Point 2");
                            textBox6.Focus();
                            return;
                        }
                        if (String.IsNullOrEmpty(textBox5.Text.Trim()))
                        {
                            MessageBox.Show("Enter or Click Read value of humiddity int Point 2");
                            textBox5.Focus();
                            return;
                        }
                        if (String.IsNullOrEmpty(txtRefHum2Prob1.Text.Trim()))
                        {
                            MessageBox.Show("Referece rH can not be empty");
                            txtRefHum2Prob1.Focus();
                            return;
                        }
                        break;
                    case 2:
                        if (String.IsNullOrEmpty(textBox8.Text.Trim()))
                        {
                            MessageBox.Show("Enter or Click Read value of temperature in Point 1");
                            textBox8.Focus();
                            return;
                        }
                        if (String.IsNullOrEmpty(textBox7.Text.Trim()))
                        {
                            MessageBox.Show("Enter or Click Read value of humiddity in point 1");
                            textBox7.Focus();
                            return;
                        }
                        if (String.IsNullOrEmpty(txtRefHum1Prob1.Text.Trim()))
                        {
                            MessageBox.Show("Referece rH can not be empty");
                            txtRefHum1Prob1.Focus();
                            return;
                        }
                        //2 point 
                        //ValidatePoint2();
                        if (String.IsNullOrEmpty(textBox6.Text.Trim()))
                        {
                            MessageBox.Show("Enter or Click Read value of temperature in Point 2");
                            textBox6.Focus();
                            return;
                        }
                        if (String.IsNullOrEmpty(textBox5.Text.Trim()))
                        {
                            MessageBox.Show("Enter or Click Read value of humiddity int Point 2");
                            textBox5.Focus();
                            return;
                        }
                        if (String.IsNullOrEmpty(txtRefHum2Prob1.Text.Trim()))
                        {
                            MessageBox.Show("Referece rH can not be empty");
                            txtRefHum2Prob1.Focus();
                            return;
                        }
                        //ValidatePoint3();
                        if (String.IsNullOrEmpty(textBox4.Text.Trim()))
                        {
                            MessageBox.Show("Enter or Click Read value of temperature in Point 3");
                            textBox4.Focus();
                            return;
                        }
                        if (String.IsNullOrEmpty(textBox5.Text.Trim()))
                        {
                            MessageBox.Show("Enter or Click Read value of humidity in Point 3");
                            textBox5.Focus();
                            return;
                        }
                        if (String.IsNullOrEmpty(txtRefHum3Prob1.Text.Trim()))
                        {
                            MessageBox.Show("Referece rH can not be empty");
                            txtRefHum3Prob1.Focus();
                            return;
                        }
                        break;
                    case 3:
                        if (String.IsNullOrEmpty(textBox8.Text.Trim()))
                        {
                            MessageBox.Show("Enter or Click Read value of temperature in Point 1");
                            textBox8.Focus();
                            return;
                        }
                        if (String.IsNullOrEmpty(textBox7.Text.Trim()))
                        {
                            MessageBox.Show("Enter or Click Read value of humiddity in point 1");
                            textBox7.Focus();
                            return;
                        }
                        if (String.IsNullOrEmpty(txtRefHum1Prob1.Text.Trim()))
                        {
                            MessageBox.Show("Referece rH can not be empty");
                            txtRefHum1Prob1.Focus();
                            return;
                        }
                        //2 point 
                        //ValidatePoint2();
                        if (String.IsNullOrEmpty(textBox6.Text.Trim()))
                        {
                            MessageBox.Show("Enter or Click Read value of temperature in Point 2");
                            textBox6.Focus();
                            return;
                        }
                        if (String.IsNullOrEmpty(textBox5.Text.Trim()))
                        {
                            MessageBox.Show("Enter or Click Read value of humiddity int Point 2");
                            textBox5.Focus();
                            return;
                        }
                        if (String.IsNullOrEmpty(txtRefHum2Prob1.Text.Trim()))
                        {
                            MessageBox.Show("Referece rH can not be empty");
                            txtRefHum2Prob1.Focus();
                            return;
                        }
                        //ValidatePoint3();
                        if (String.IsNullOrEmpty(textBox4.Text.Trim()))
                        {
                            MessageBox.Show("Enter or Click Read value of temperature in Point 3");
                            textBox4.Focus();
                            return;
                        }
                        if (String.IsNullOrEmpty(textBox5.Text.Trim()))
                        {
                            MessageBox.Show("Enter or Click Read value of humidity in Point 3");
                            textBox5.Focus();
                            return;
                        }
                        if (String.IsNullOrEmpty(txtRefHum3Prob1.Text.Trim()))
                        {
                            MessageBox.Show("Referece rH can not be empty");
                            txtRefHum3Prob1.Focus();
                            return;
                        }
                        //point 4
                        if (String.IsNullOrEmpty(textBox2.Text.Trim()))
                        {
                            MessageBox.Show("Enter or Click Read value of temperature in Point 3");
                            textBox2.Focus();
                            return;
                        }
                        if (String.IsNullOrEmpty(textBox1.Text.Trim()))
                        {
                            MessageBox.Show("Enter or Click Read value of humidity in Point 3");
                            textBox1.Focus();
                            return;
                        }
                        if (String.IsNullOrEmpty(txtRefHum4Prob1.Text.Trim()))
                        {
                            MessageBox.Show("Referece rH can not be empty");
                            txtRefHum4Prob1.Focus();
                            return;
                        }
                        break;
                }
                //start Calib humid

                switch (cbbNumCalPoint1.SelectedIndex)
                {
                    case 0:
                        a = b = c = 0;
                        d = double.Parse(txtRefHum1Prob1.Text, CultureInfo.InvariantCulture) - double.Parse(textBox7.Text, CultureInfo.InvariantCulture);

                        break;
                    case 1:
                        // 1 Tinh X1,X2, Y1, Y2 chuyen qua 30oC =>Next
                        // 2: thanh lap 3 ma tran, tim ra 3 dinh thuc
                        // 3: giai he 2 phuong trinh tim ra he so ab
                        //Pt: AX1+B=Y1
                        //    AX2+B=Y2

                        T1 = double.Parse(textBox8.Text, CultureInfo.InvariantCulture);
                        T2 = double.Parse(textBox6.Text, CultureInfo.InvariantCulture);
                        X1 = double.Parse(textBox7.Text, CultureInfo.InvariantCulture);
                        X2 = double.Parse(textBox5.Text, CultureInfo.InvariantCulture);
                        Y1 = double.Parse(txtRefHum1Prob1.Text, CultureInfo.InvariantCulture);
                        Y2 = double.Parse(txtRefHum2Prob1.Text, CultureInfo.InvariantCulture);

                        //Calculator ab,c,d when Point =2
                        Calculator2point(T1, T2, X1, X2, Y1, Y2);

                        break;
                    case 2:

                        T1 = double.Parse(textBox8.Text, CultureInfo.InvariantCulture);
                        T2 = double.Parse(textBox6.Text, CultureInfo.InvariantCulture);
                        T3 = double.Parse(textBox4.Text, CultureInfo.InvariantCulture);

                        X1 = double.Parse(textBox7.Text, CultureInfo.InvariantCulture);
                        X2 = double.Parse(textBox5.Text, CultureInfo.InvariantCulture);
                        X3 = double.Parse(textBox3.Text, CultureInfo.InvariantCulture);

                        Y1 = double.Parse(txtRefHum1Prob1.Text, CultureInfo.InvariantCulture);
                        Y2 = double.Parse(txtRefHum2Prob1.Text, CultureInfo.InvariantCulture);
                        Y3 = double.Parse(txtRefHum3Prob1.Text, CultureInfo.InvariantCulture);

                        List<double> X = new List<double>() { X1, X2, X3 };
                        List<double> Y = new List<double>() { Y1, Y2, Y3 };
                        List<double> T = new List<double>() { T1, T2, T3 };
                        TimPhuongTrinh(X, Y, T);
                        //Calculator3Point(T1, T2, T3, X1, X2, X3, Y1, Y2, Y3);//Giai he phuong trinh bac 2
                        break;
                    case 3:
                        //giai he 4 phuong trinh tim ra he so abcd
                        T1 = double.Parse(textBox8.Text, CultureInfo.InvariantCulture);
                        T2 = double.Parse(textBox6.Text, CultureInfo.InvariantCulture);
                        T3 = double.Parse(textBox4.Text, CultureInfo.InvariantCulture);
                        T4 = double.Parse(textBox2.Text, CultureInfo.InvariantCulture);

                        X1 = double.Parse(textBox7.Text, CultureInfo.InvariantCulture);
                        X2 = double.Parse(textBox5.Text, CultureInfo.InvariantCulture);
                        X3 = double.Parse(textBox3.Text, CultureInfo.InvariantCulture);
                        X4 = double.Parse(textBox1.Text, CultureInfo.InvariantCulture);

                        Y1 = double.Parse(txtRefHum1Prob1.Text, CultureInfo.InvariantCulture);
                        Y2 = double.Parse(txtRefHum2Prob1.Text, CultureInfo.InvariantCulture);
                        Y3 = double.Parse(txtRefHum3Prob1.Text, CultureInfo.InvariantCulture);
                        Y4 = double.Parse(txtRefHum4Prob1.Text, CultureInfo.InvariantCulture);

                        //Calculator4Point(T1, T2, T3, T4, X1, X2, X3, X4, Y1, Y2, Y3, Y4);
                        X = new List<double>() { X1, X2, X3, X4 };
                        Y = new List<double>() { Y1, Y2, Y3, Y4 };
                        T = new List<double>() { T1, T2, T3, T4 };
                        TimPhuongTrinh(X, Y, T);
                        break;
                }

                success = WriteDataCalib(selectedChannel + 1);
                if (success)
                {
                    if (a > 32000)
                    {
                        a = a - 65536;
                    }
                    if (b > 32000)
                    {
                        b = b - 65536;
                    }
                    if (c > 32000)
                    {
                        c = c - 65536;
                    }
                    if (d > 32000)
                    {
                        d = d - 65536;
                    }

                    MessageBox.Show("Calib Success!\na= " + a + "\nb= " + b + "\nc= " + c + "\nd= " + d);
                }
                else
                {
                    MessageBox.Show("Calib Fail! Try again");
                }
            }
            else if (channels[selectedChannel].sensor == Sensor.Temp_sensor)
            {
                //Calib Temp
                if (String.IsNullOrEmpty(textBox51.Text.Trim()))
                {
                    MessageBox.Show("Offset can not be empty");
                    textBox51.Focus();
                    return;
                }

                double data = double.Parse(textBox51.Text, CultureInfo.InvariantCulture);
                byte bufDt = (byte)(data * 10);
                byte dau = 0;
                if (textBox51.Text.Contains("-"))
                {
                    dau = 1;
                }
                dv35.USBOpen(host);
                //HIDFunction.hid_SetNonBlocking(dv35.dev, 1);
                Thread.Sleep(500);
                if (!dv35.writeCalibOffset((byte)(selectedChannel + 1), bufDt, dau))
                {
                    MessageBox.Show("Setting Calib fail");
                    dv35.Close();
                    return;
                }
                else
                {
                    MessageBox.Show("Setting Calib successful");
                    dv35.Close();
                }


            }
            btnCalProb1.Enabled = false;
            btnCalProb1.Refresh();
            Thread.Sleep(2000);
            btnCalProb1.Enabled = true;

        }

        double Y1, Y2, Y3, Y4;//reference rH

        private void txtRefHum1Prob1_TextChanged(object sender, EventArgs e)
        {
            TextBox textBox=  (TextBox)sender;
            textBox.Text = textBox.Text.Replace(',', '.');
        }

        enum Sensor
        {
            No_sensor,
            Humid_sensor,
            Temp_sensor
        }
        struct DataCalibs
        {
            /// <summary>
            /// type of Sensor connect
            /// </summary>
            public Sensor sensor;
            /// <summary>
            /// num of channel= 1,2,3,4
            /// </summary>
            public int channel;
            /// <summary>
            /// yes or no alarm
            /// </summary>
            public bool alarm;
            /// <summary>
            /// Humidity with offset
            /// </summary>
            public double rHOffset;
            /// <summary>
            /// rH No Offset 
            /// </summary>
            public double rHNoOffset;
            /// <summary>
            /// temp data
            /// </summary>
            public double temp;
            /// <summary>
            /// data offset
            /// </summary>
            public double[] dataOffset;
        }
        double Corrected_rH_at_30(double RH, double T)
        {
            return RH + 0.05 * (T - 30);
        }
        /// <summary>
        /// Calculator a,b,c,d with 2point
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <param name="x1"></param>
        /// <param name="x2"></param>
        /// <param name="y1"></param>
        /// <param name="y2"></param>
        double [] Calculator2point(double t1, double t2, double x1, double x2, double y1, double y2)
        {
            int n = 2;
            double[,] matrix = new double[n, n];
            double[,] matrixa = new double[n, n];
            double[,] matrixb = new double[n, n];
            matrix[0, 0] = Corrected_rH_at_30(x1, t1);
            matrix[0, 1] = 1;
            matrix[1, 0] = Corrected_rH_at_30(x2, t2);
            matrix[1, 1] = 1;

            matrixa[0, 0] = Corrected_rH_at_30(y1, t1);
            matrixa[0, 1] = 1;
            matrixa[1, 0] = Corrected_rH_at_30(y2, t2);
            matrixa[1, 1] = 1;

            matrixb[0, 0] = Corrected_rH_at_30(x1, t1);
            matrixb[0, 1] = Corrected_rH_at_30(y1, t1);
            matrixb[1, 0] = Corrected_rH_at_30(x2, t2);
            matrixb[1, 1] = Corrected_rH_at_30(y2, t2);

            DetT = Det(matrix, n);
            detA = Det(matrixa, n);
            detB = Det(matrixb, n);
            a = detA / DetT;
            b = detB / DetT;
            return new double[2] { a, b };
        }
        void Calculator3Point(double t1, double t2, double t3, double x1, double x2, double x3, double y1, double y2, double y3)
        {
            int n = 3;
            double[,] matrix = new double[n, n];
            double[,] matrixa = new double[n, n];
            double[,] matrixb = new double[n, n];
            double[,] matrixc = new double[n, n];

            matrix[0, 0] = Math.Pow(Corrected_rH_at_30(X1, T1), 2);
            matrix[0, 1] = Corrected_rH_at_30(X1, T1);
            matrix[0, 2] = 1;

            matrix[1, 0] = Math.Pow(Corrected_rH_at_30(X2, T2), 2);
            matrix[1, 1] = Corrected_rH_at_30(X2, T2);
            matrix[1, 2] = 1;

            matrix[2, 0] = Math.Pow(Corrected_rH_at_30(X3, T3), 2);
            matrix[2, 1] = Corrected_rH_at_30(X3, T3);
            matrix[2, 2] = 1;

            matrixa[0, 0] = Corrected_rH_at_30(Y1, T1);
            matrixa[0, 1] = Corrected_rH_at_30(X1, T1);
            matrixa[0, 2] = 1;

            matrixa[1, 0] = Corrected_rH_at_30(Y2, T2);
            matrixa[1, 1] = Corrected_rH_at_30(X2, T2);
            matrixa[1, 2] = 1;

            matrixa[2, 0] = Corrected_rH_at_30(Y3, T3);
            matrixa[2, 1] = Corrected_rH_at_30(X3, T3);
            matrixa[2, 2] = 1;

            matrixb[0, 0] = Math.Pow(Corrected_rH_at_30(X1, T1), 2);
            matrixb[0, 1] = Corrected_rH_at_30(Y1, T1);
            matrixb[0, 2] = 1;

            matrixb[1, 0] = Math.Pow(Corrected_rH_at_30(X2, T2), 2);
            matrixb[1, 1] = Corrected_rH_at_30(Y2, T2);
            matrixb[1, 2] = 1;

            matrixb[2, 0] = Math.Pow(Corrected_rH_at_30(X3, T3), 2);
            matrixb[2, 1] = Corrected_rH_at_30(Y3, T3);
            matrixb[2, 2] = 1;

            matrixc[0, 0] = Math.Pow(Corrected_rH_at_30(X1, T1), 2);
            matrixc[0, 1] = Corrected_rH_at_30(X1, T1);
            matrixc[0, 2] = Corrected_rH_at_30(Y1, T1);
            matrixc[1, 0] = Math.Pow(Corrected_rH_at_30(X2, T2), 2);
            matrixc[1, 1] = Corrected_rH_at_30(X2, T2);
            matrixc[1, 2] = Corrected_rH_at_30(Y2, T2);
            matrixc[2, 0] = Math.Pow(Corrected_rH_at_30(X3, T3), 2);
            matrixc[2, 1] = Corrected_rH_at_30(X3, T3);
            matrixc[2, 2] = Corrected_rH_at_30(Y3, T3);

            double[,] test = new double[3, 3];
            test[0, 0] = -1;
            test[0, 1] = 2;
            test[0, 2] = 3;

            test[1, 0] = 2;
            test[1, 1] = -1;
            test[1, 2] = 4;

            test[2, 0] = 0;
            test[2, 1] = -3;
            test[2, 2] = 2;

            DetT = Det(test,2);// Det(matrix, 3);

            double dett1 = determinant(test);
            detA = Det(matrixa, 3);
            detB = Det(matrixb, 3);
            detC = Det(matrixc, 3);
            a = detA / DetT;
            b = detB / DetT;
            c = detC / DetT;
            d = 0;
        }

        void Calculator4Point(double t1, double t2, double t3, double t4, double x1, double x2, double x3, double x4, double y1,
            double y2, double y3, double y4)
        {
            int n = 4;
            double[,] matrix = new double[n, n];
            double[,] matrixa = new double[n, n];
            double[,] matrixb = new double[n, n];
            double[,] matrixc = new double[n, n];
            double[,] matrixd = new double[n, n];
            matrix[0, 0] = Math.Pow(Corrected_rH_at_30(X1, T1), 3);
            matrix[0, 1] = Math.Pow(Corrected_rH_at_30(X1, T1), 2);
            matrix[0, 2] = Corrected_rH_at_30(X1, T1);
            matrix[0, 3] = 1;

            matrix[1, 0] = Math.Pow(Corrected_rH_at_30(X2, T2), 3);
            matrix[1, 1] = Math.Pow(Corrected_rH_at_30(X2, T2), 2);
            matrix[1, 2] = Corrected_rH_at_30(X2, T2);
            matrix[1, 3] = 1;

            matrix[2, 0] = Math.Pow(Corrected_rH_at_30(X3, T3), 3);
            matrix[2, 1] = Math.Pow(Corrected_rH_at_30(X3, T3), 2);
            matrix[2, 2] = Corrected_rH_at_30(X3, T3); ;
            matrix[2, 3] = 1;

            matrix[3, 0] = Math.Pow(Corrected_rH_at_30(X4, T4), 3);
            matrix[3, 1] = Math.Pow(Corrected_rH_at_30(X4, T4), 2);
            matrix[3, 2] = Corrected_rH_at_30(X4, T4); ;
            matrix[3, 3] = 1;

            matrixa[0, 0] = Corrected_rH_at_30(Y1, T1);
            matrixa[0, 1] = Math.Pow(Corrected_rH_at_30(X1, T1), 2);
            matrixa[0, 2] = Corrected_rH_at_30(X1, T1);
            matrixa[0, 3] = 1;

            matrixa[1, 0] = Corrected_rH_at_30(Y2, T2);
            matrixa[1, 1] = Math.Pow(Corrected_rH_at_30(X2, T2), 2);
            matrixa[1, 2] = Corrected_rH_at_30(X2, T2);
            matrixa[1, 3] = 1;

            matrixa[2, 0] = Corrected_rH_at_30(Y3, T3);
            matrixa[2, 1] = Math.Pow(Corrected_rH_at_30(X3, T3), 2);
            matrixa[2, 2] = Corrected_rH_at_30(X3, T3);
            matrixa[2, 3] = 1;

            matrixa[3, 0] = Corrected_rH_at_30(Y4, T4);
            matrixa[3, 1] = Math.Pow(Corrected_rH_at_30(X4, T4), 2);
            matrixa[3, 2] = Corrected_rH_at_30(X4, T4);
            matrixa[3, 3] = 1;

            matrixb[0, 0] = Math.Pow(Corrected_rH_at_30(X1, T1), 3);
            matrixb[0, 1] = Corrected_rH_at_30(Y1, T1);
            matrixb[0, 2] = Corrected_rH_at_30(X1, T1);
            matrixb[0, 3] = 1;

            matrixb[1, 0] = Math.Pow(Corrected_rH_at_30(X2, T2), 3);
            matrixb[1, 1] = Corrected_rH_at_30(Y2, T2);
            matrixb[1, 2] = Corrected_rH_at_30(X2, T2);
            matrixb[1, 3] = 1;

            matrixb[2, 0] = Math.Pow(Corrected_rH_at_30(X3, T3), 3);
            matrixb[2, 1] = Corrected_rH_at_30(Y3, T3);
            matrixb[2, 2] = Corrected_rH_at_30(X3, T3);
            matrixb[2, 3] = 1;

            matrixb[3, 0] = Math.Pow(Corrected_rH_at_30(X4, T4), 3);
            matrixb[3, 1] = Corrected_rH_at_30(Y4, T4);
            matrixb[3, 2] = Corrected_rH_at_30(X4, T4);
            matrixb[3, 3] = 1;

            matrixc[0, 0] = Math.Pow(Corrected_rH_at_30(X1, T1), 3);
            matrixc[0, 1] = Math.Pow(Corrected_rH_at_30(X1, T1), 2);
            matrixc[0, 2] = Corrected_rH_at_30(Y1, T1);
            matrixc[0, 3] = 1;

            matrixc[1, 0] = Math.Pow(Corrected_rH_at_30(X2, T2), 3);
            matrixc[1, 1] = Math.Pow(Corrected_rH_at_30(X2, T2), 2);
            matrixc[1, 2] = Corrected_rH_at_30(Y2, T2);
            matrixc[1, 3] = 1;

            matrixc[2, 0] = Math.Pow(Corrected_rH_at_30(X3, T3), 3);
            matrixc[2, 1] = Math.Pow(Corrected_rH_at_30(X3, T3), 2);
            matrixc[2, 2] = Corrected_rH_at_30(Y3, T3);
            matrixc[2, 3] = 1;

            matrixc[3, 0] = Math.Pow(Corrected_rH_at_30(X4, T4), 3);
            matrixc[3, 1] = Math.Pow(Corrected_rH_at_30(X4, T4), 2);
            matrixc[3, 2] = Corrected_rH_at_30(Y4, T4);
            matrixc[3, 3] = 1;

            DetT = Det(matrix, 4);
            detA = Det(matrixa, 4);
            detB = Det(matrixb, 4);
            detC = Det(matrixc, 4);
            detD = Det(matrixd, 4);
            a = detA / DetT;
            b = detB / DetT;
            c = detC / DetT;
            d = detD / DetT;
        }
        private bool ReadInfoDatacalib()
        {
            dv35 = Device35.DelInstance();
            dv35 = Device35.Instance;
            channels = new List<DataCalibs>();
            dv35.Channels = new Channel[4];
            for (int i = 0; i < 4; i++)
            {
                dv35.Channels[i] = new Channel();
            }
            try
            {
                if (!dv35.Open(host))
                {
                    MessageBox.Show("Unable to open device!");
                    return false;
                }
                //dv35.USBOpen(host);
                byte[] buf = new byte[64];
                dv35.readSettingDevice();
                if (dv35.byteLogging == 68)
                {
                    MessageBox.Show("Logger is recording. Please stop to calibrate");
                    this.Close();
                }
                buf[0] = 0x01;
                buf[1] = 0xa7;
                // buf[1] = 0x92;
                buf[2] = 0x01;
                buf[3] = 0x01;
                dv35.Write(buf);
                dv35.Read(ref buf);
                if (buf[1]==8)
                {
                    string notcalib = Encoding.ASCII.GetString(buf, 2, 8);
                    if (notcalib.Contains("NotCalib"))
                    {
                        return false;
                    }
                }
                for (int i = 2; i < 28; i += 7)
                {
                    DataCalibs channel = new DataCalibs();
                    string firstByte = Convert.ToString(buf[i], 2).PadLeft(8, '0');
                    if (firstByte.Substring(0, 1) == "1")
                    {
                        channel.alarm = true;
                    }
                    channel.channel = Convert.ToByte(firstByte.Substring(1, 3), 2);
                    switch (firstByte.Substring(4, 4))
                    {
                        case "0000":
                            channel.sensor = Sensor.No_sensor;
                            break;
                        case "0001":
                            channel.sensor = Sensor.Temp_sensor;
                            break;
                        case "0010":
                            channel.sensor = Sensor.Humid_sensor;
                            break;
                        default:
                            channel.sensor = Sensor.No_sensor;
                            break;
                    }
                    if (channel.sensor == Sensor.Humid_sensor)
                    {
                        channel.rHOffset = mGlobal.get_temp(buf[i + 1], buf[i + 2]) / 10.00;
                        channel.rHNoOffset = mGlobal.get_temp(buf[i + 3], buf[i + 4]) / 10.00;

                        byte tem = Convert.ToByte(Convert.ToString(buf[i + 5], 2).PadLeft(8, '0').Remove(0, 1), 2);
                        channel.temp = mGlobal.get_temp(tem, buf[i + 6]) / 10.00;
                    }
                    else if (channel.sensor == Sensor.Temp_sensor)
                    {
                        channel.temp = mGlobal.get_temp(buf[i + 1], buf[i + 2]) / 10.00;
                    }
                    channels.Add(channel);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Cannot open device. Try again");
                this.Close();
            }
            finally
            { dv35.Close(); }
            return true;
            //HIDFunction.hid_SetNonBlocking(dv35.dev, 1);

          

        }
        bool WriteDataCalib(int channel)
        {
            if (dv35.Open(host))
            {
                byte[] buf = new byte[64];
                buf[0] = 0x01;
                buf[1] = 0x92;
                buf[2] = 0x00;//Read or Write
                buf[3] = (byte)channel;//Channel

                if (a < 0)
                {
                    a = a + 65536;
                }

                if (b < 0)
                {
                    b = b + 65536;
                }

                if (c < 0)
                {
                    c = c + 65536;
                }

                if (d < 0)
                {
                    d = d + 65536;
                }


                buf[4] = (byte)(a * 1000 / 256);
                buf[5] = (byte)(a * 1000 % 256);
                buf[6] = (byte)(b * 100 / 256);
                buf[7] = (byte)(b * 100 % 256);
                buf[8] = (byte)(c / 256);
                buf[9] = (byte)(c % 256);
                buf[10] = (byte)(d / 256);
                buf[11] = (byte)(d % 256);
                byte[] dataCalib = new byte[8];
                for (int i = 0; i < 8; i++)
                {
                    dataCalib[i] = buf[i + 4];
                }
                dv35.Write(buf);
                Thread.Sleep(100);
                dv35.Read(ref buf);
                dv35.Close();
                Thread.Sleep(1000);
                if (buf[3] == 0x92 && buf[2] == 0x92)
                {
                    byte[] re = ReadDataCalib((byte)channel);
                    if (re==null)
                    {
                        return false;
                    }
                    for (int i = 0; i < re.Length; i++)
                    {
                        if (re[i] != dataCalib[i])
                        {

                            return false;
                        }
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }
        /// <summary>
        /// Read Data Calib of 4 channel
        /// </summary>
        /// <returns></returns>
        byte[] ReadDataCalib(byte channel)
        {
            if (dv35.USBOpen(host))
            {
                byte[] buf = new byte[64];
                buf[0] = 0x01;
                buf[1] = 0x92;
                buf[2] = 0x01;//Read or Write
                buf[3] = channel;//Channel
                dv35.Write(buf);
                Thread.Sleep(100);
                dv35.Read(ref buf);
                dv35.Close();
                byte[] dataCalibs = new byte[8];
                for (int i = 0; i < 8; i++)
                {
                    dataCalibs[i] = buf[i + 2];
                }

                return dataCalibs;

            }
            return null;
        }
        /// <summary>
        /// Calculator Det A
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        public double Det(double[,] arr, int n)
        {
            double d = 0;
            if (n <= 0) return 0;
            if (n == 1) return arr[0, 0];
            if (n == 2) return (arr[0, 0] * arr[1, 1] - arr[0, 1] * arr[1, 0]);
            else
            {
                double[,] c = new double[n - 1, n - 1];
                for (int i = 0; i < n; i++)
                {
                    int p = 0; int m = 0;
                    for (int j = 1; j < n; j++)
                    {
                        for (int e = 0; e < n; e++)
                        {
                            if (e == i) continue;
                            c[p, m] = arr[j, e];
                            m++;
                            if (m == n - 1)
                            {
                                p++;
                                m = 0;
                            }
                        }
                    }
                    d += Math.Pow(-1, i) * arr[0, i] * Det(c, n - 1);
                }
                
                return d;

            }
        }
        // ham tinh dinh thuc
        double determinant(double [,] array)
        {
            double det = 0;
            double total = 0;
            double[,] tempArr = new double[array.GetLength(0) - 1, array.GetLength(1) - 1];

            if (array.GetLength(0) == 2)
            {
                det = array[0, 0] * array[1, 1] - array[0, 1] * array[1, 0];
            }

            else
            {

                for (int i = 0; i < 1; i++)
                {
                    for (int j = 0; j < array.GetLength(1); j++)
                    {
                        //if (j % 2 != 0) array[i, j] = array[i, j] * -1;
                        //tempArr = fillNewArr(array, i, j);
                        //det += determinant(tempArr);
                        //total = total + (det * array[i, j]);
                        double subdet = determinant(fillNewArr(array, i, j));
                        if (j % 2 != 0) subdet *= -1;
                        det += array[i, j] * subdet;
                    }
                }
            }
            return det;
        }
        public static double[,] fillNewArr(double[,] originalArr, int row, int col)
        {
            double[,] tempArray = new double[originalArr.GetLength(0) - 1, originalArr.GetLength(1) - 1];

            for (int i = 0, newRow = 0; i < originalArr.GetLength(0); i++)
            {
                if (i == row)
                    continue;
                for (int j = 0, newCol = 0; j < originalArr.GetLength(1); j++)
                {
                    if (j == col) continue;
                    tempArray[newRow, newCol] = originalArr[i, j];

                    newCol++;
                }
                newRow++;
            }
            return tempArray;

        }
        private void Calibration34_Load(object sender, EventArgs e)
        {

            btnReload_Click(sender, e);

            rdbGroup1.Add(radioButton1);
            rdbGroup1.Add(radioButton2);
            rdbGroup1.Add(radioButton3);
            rdbGroup1.Add(radioButton4);

            double [] ab= TimPhuongTrinh(new List<double>() {0,1,2,3,4 }, new List<double>() { 100,100.39,100.77,101.18,101.59}, new List<double>() { 30, 30, 30, 30, 30 });
            double[] xy = TimPhuongTrinh(new List<double>() { 69.6, 87.5, 93.7 }, new List<double>() {67, 85, 90}, new List<double>() { 28, 29, 30 });
        }

        double[] TimPhuongTrinh(List<double> X, List<double> Y, List<double>T)
        {
            //Chuyen qua 30 do C
            for (int i = 0; i < X.Count; i++)
            {
                X[i] = Corrected_rH_at_30(X[i], T[i]);
                Y[i] = Corrected_rH_at_30(Y[i], T[i]);
            }
            double ata1 = 0, ata2 = 0, ata3 = 0, ata4 = X.Count;
            double atb1 = 0, atb2 = 0;
            for (int i = 0; i < X.Count; i++)
            {
                ata1 += X[i] * X[i];
                ata2 += X[i];
                ata3 += X[i];
                
                atb1 += X[i] * Y[i];
                atb2 += Y[i];
            }
            double[,] ata = new double[2, 2] { {ata1, ata2},{ata3,ata4 } };
            double[] atb = new double[2] { atb1,atb2};
            detA = Det(ata, 2);
            if (detA!=0)
            {
                a = (ata4 * atb1 - ata3 * atb2) / detA;
                b = (-ata2 * atb1 + ata1 * atb2) / detA;
                return new double[2] { a, b };
            }
            else
            {
                return null;
            }
            
        }
    }
}
