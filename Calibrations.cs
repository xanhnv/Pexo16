using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pexo16
{
    public partial class Calibrations : Form
    {
        private Device35 dv35;
        string host;
        enum Sensor
        {
            No_sensor,
            Humid_sensor,
            Temp_sensor
        }
        ResourceManager res_man = new ResourceManager("Pexo16.Lang.Resources", typeof(Calib35).Assembly);
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
        List<DataCalibs> channels = new List<DataCalibs>();
        List<GroupBox> listGrbTem = new List<GroupBox>();
        List<GroupBox> listGrbHum = new List<GroupBox>();
        List<Button> listButtonRead = new List<Button>();
        List<Button> listButtonCalib = new List<Button>();
       // List<TextBox> listTextboxCalTemp = new List<TextBox>();

        List<RadioButton> rdbGroup1 = new List<RadioButton>();
        List<RadioButton> rdbGroup2 = new List<RadioButton>();
        List<RadioButton> rdbGroup3 = new List<RadioButton>();
        List<RadioButton> rdbGroup4 = new List<RadioButton>();

        public Calibrations(string hostPost)
        {
            host = hostPost;
            InitializeComponent();
        }

        private void Calibrations_Load_1(object sender, EventArgs e)
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
           // MessageBox.Show(res_man.GetString("Data offset cannot over 25.5", cul));
            cbbNumCalPoint1.SelectedIndex = 1;
            cbbNumCalPoint2.SelectedIndex = 1;
            cbbNumCalPoint3.SelectedIndex = 1;
            cbbNumCalPoint4.SelectedIndex = 1;
            listGrbTem.Clear();
            listGrbTem.Add(grbCalTemp1);
            listGrbTem.Add(grbCalTemp2);
            listGrbTem.Add(grbCalTemp3);
            listGrbTem.Add(grbCalTemp4);
            listGrbHum.Clear();
            listGrbHum.Add(grbCalHum1);
            listGrbHum.Add(grbCalHum2);
            listGrbHum.Add(grbCalHum3);
            listGrbHum.Add(grbCalHum4);
            listButtonCalib.Clear();
            listButtonCalib.Add(btnCalProb1);
            listButtonCalib.Add(btnCalProb2);
            listButtonCalib.Add(btnCalProb3);
            listButtonCalib.Add(btnCalProb4);
            listButtonRead.Clear();
            listButtonRead.Add(btnReadProb1);
            listButtonRead.Add(btnReadProb2);
            listButtonRead.Add(btnReadProb3);
            listButtonRead.Add(btnReadProb4);
            //sensor Temp
            //listTextboxCalTemp.Add(textBox49);
            //listTextboxCalTemp.Add(textBox53);
            //listTextboxCalTemp.Add(textBox56);
            //listTextboxCalTemp.Add(textBox59);

            //Radio button group 1
            rdbGroup1.Clear();
            rdbGroup1.Add(radioButton1);
            rdbGroup1.Add(radioButton2);
            rdbGroup1.Add(radioButton3);
            rdbGroup1.Add(radioButton4);
            //Radio button group 2
            rdbGroup2.Clear();
            rdbGroup2.Add(radioButton9);
            rdbGroup2.Add(radioButton10);
            rdbGroup2.Add(radioButton11);
            rdbGroup2.Add(radioButton12);
            //Radio button group 3
            rdbGroup3.Clear();
            rdbGroup3.Add(radioButton5);
            rdbGroup3.Add(radioButton6);
            rdbGroup3.Add(radioButton7);
            rdbGroup3.Add(radioButton8);
            //Radio button group 4
            rdbGroup4.Clear();
            rdbGroup4.Add(radioButton13);
            rdbGroup4.Add(radioButton14);
            rdbGroup4.Add(radioButton15);
            rdbGroup4.Add(radioButton16);

            ReadAndFill();


        }
        private void ReadAndFill()
        {
            if (ReadInfoDatacalib())
            {

                for (int i = 0; i < channels.Count; i++)
                {
                    if (channels[i].sensor == Sensor.No_sensor)
                    {
                        listGrbHum[i].BringToFront();
                        listGrbHum[i].Enabled = false;
                        listButtonRead[i].Enabled = false;
                        listButtonCalib[i].Enabled = false;
                    }
                    else
                    {
                        if (channels[i].sensor == Sensor.Temp_sensor)
                        {
                            listGrbTem[i].BringToFront();
                            listGrbTem[i].Enabled = true;
                            listGrbTem[i].Visible = true;

                            listGrbHum[i].Enabled = true;
                            listGrbHum[i].Visible = true;
                            //fill value
                            //listTextboxCalTemp[i].Text = channels[i].temp.ToString();
                            listButtonRead[i].Visible = false;
                            listButtonCalib[i].Enabled = true;
                        }
                        else if (channels[i].sensor == Sensor.Humid_sensor)
                        {
                            listButtonRead[i].Visible = true;
                            listButtonRead[i].Enabled = true;
                            listButtonCalib[i].Enabled = true;

                            listGrbHum[i].BringToFront();
                            listGrbHum[i].Visible = true;
                            listGrbHum[i].Enabled = true;
                            listGrbTem[i].Visible = false;
                            //Fill value
                            switch (i)
                            {
                                case 0:
                                    textBox8.Text = channels[i].temp.ToString();
                                    textBox7.Text = channels[i].rHNoOffset.ToString();
                                    break;
                                case 1:
                                    textBox24.Text = channels[i].temp.ToString();
                                    textBox23.Text = channels[i].rHNoOffset.ToString();
                                    break;
                                case 2:
                                    textBox36.Text = channels[0].temp.ToString();
                                    textBox35.Text = channels[0].rHNoOffset.ToString();
                                    break;
                                case 3:
                                    textBox48.Text = channels[i].temp.ToString();
                                    textBox47.Text = channels[i].rHNoOffset.ToString();
                                    break;
                            }

                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Logger is not in calib mode");
                Close();
            }
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
                dv35.Open(host);
                //HIDFunction.hid_SetNonBlocking(dv35.dev, 1);

                Thread.Sleep(200);

                byte[] buf = new byte[64];
                dv35.readSettingDevice();
                if (dv35.byteLogging == 68)
                {
                    MessageBox.Show("Logger is recording. Please stop to calibrate");
                    dv35.Close();
                    this.Close();
                }
                byte[] buf2 = new byte[64];
                buf2[0] = 0x01;
                buf2[1] = 0xa9;
                dv35.Write(buf2);
                dv35.Read(ref buf2);

                Thread.Sleep(100);
                buf[0] = 0x01;
                buf[1] = 0xa7;
                // buf[1] = 0x92;
                buf[2] = 0x01;
                buf[3] = 0x01;
                dv35.Write(buf);
                dv35.Read(ref buf);
                string mess = Encoding.ASCII.GetString(buf);
                if (mess.Contains("NotCalib"))
                {
                    return false;
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
                    string dev = Encoding.ASCII.GetString(buf2,i,7);
                    switch (dev)
                    {
                        case "nodevic":
                            channel.sensor = Sensor.No_sensor;
                            break;
                        case "PEXO-37":
                            channel.sensor = Sensor.Temp_sensor;
                            break;
                        case "PEXO-40":
                            channel.sensor = Sensor.Humid_sensor;
                            break;
                        default:
                            channel.sensor = Sensor.No_sensor;
                            break;
                    }
                    //switch (firstByte.Substring(4, 4))
                    //{
                    //    case "0000":
                    //        channel.sensor = Sensor.No_sensor;
                    //        break;
                    //    case "0001":
                    //        channel.sensor = Sensor.Temp_sensor;
                    //        break;
                    //    case "0010":
                    //        channel.sensor = Sensor.Humid_sensor;
                    //        break;
                    //    default:
                    //        channel.sensor = Sensor.No_sensor;
                    //        break;
                    //}

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
                return true;
            }
            catch
            {
                MessageBox.Show("Cannot open device");
                return false;
            }
            finally { dv35.Close(); }

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

        private void txActT1_Enter(object sender, EventArgs e)
        {
            TextBox TB = (TextBox)sender;
            int VisibleTime = 1000;  //in milliseconds
            ToolTip tt = new ToolTip();
            tt.Show("Double click to get new value!", TB, 0, 0, VisibleTime);
        }

        private void txActT1_DoubleClick(object sender, EventArgs e)
        {
            Random a = new Random(30);
            var val = a.NextDouble();
        }

        private void cbbNumCalPoint2_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cbbNumCalPoint2.SelectedIndex)
            {
                case 0:
                    panel1.Enabled = false;
                    panel2.Enabled = false;
                    panel3.Enabled = false;
                    radioButton9.Checked = true;
                    radioButton10.Checked = false;
                    radioButton11.Checked = false;
                    radioButton12.Checked = false;
                    textBox14.Text = "";
                    textBox15.Text = "";
                    textBox17.Text = "";
                    textBox18.Text = "";
                    textBox20.Text = "";
                    textBox21.Text = "";
                    txtRefHum2Prob2.Text = "";
                    txtRefHum3Prob2.Text = "";
                    txtRefHum4Prob2.Text = "";
                    break;
                case 1:
                    panel1.Enabled = false;
                    panel2.Enabled = false;
                    panel3.Enabled = true;
                    radioButton9.Checked = false;
                    radioButton10.Checked = true;
                    radioButton11.Checked = false;
                    radioButton12.Checked = false;
                    
                    textBox17.Text = "";
                    textBox18.Text = "";
                    textBox20.Text = "";
                    textBox21.Text = "";
                    txtRefHum2Prob2.Text = "";
                    txtRefHum3Prob2.Text = "";
                    break;
                case 2:
                    panel1.Enabled = false;
                    panel2.Enabled = true;
                    panel3.Enabled = true;
                    radioButton9.Checked = false;
                    radioButton10.Checked = false;
                    radioButton11.Checked = true;
                    radioButton12.Checked = false;
                    textBox20.Text = "";
                    textBox21.Text = "";
                    txtRefHum2Prob2.Text = "";
                   
                    break;
                case 3:
                    panel1.Enabled = true;
                    panel2.Enabled = true;
                    panel3.Enabled = true;
                    radioButton9.Checked = false;
                    radioButton10.Checked = false;
                    radioButton11.Checked = false;
                    radioButton12.Checked = true;
                    break;
            }
        }
        private void cbbNumCalPoint3_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cbbNumCalPoint3.SelectedIndex)
            {
                case 0:
                    panel9.Enabled = false;
                    panel10.Enabled = false;
                    panel11.Enabled = false;
                    radioButton5.Checked = true;
                    radioButton6.Checked = false;
                    radioButton7.Checked = false;
                    radioButton8.Checked = false;
                    textBox26.Text = "";
                    textBox27.Text = "";
                    textBox29.Text = "";
                    textBox30.Text = "";
                    textBox32.Text = "";
                    textBox33.Text = "";
                    txtRefHum2Prob3.Text = "";
                    txtRefHum3Prob3.Text = "";
                    txtRefHum4Prob3.Text = "";
                    break;
                case 1:

                    panel9.Enabled = false;
                    panel10.Enabled = false;
                    panel11.Enabled = true;
                    radioButton5.Checked = false;
                    radioButton6.Checked = true;
                    radioButton7.Checked = false;
                    radioButton8.Checked = false;
                    textBox26.Text = "";
                    textBox27.Text = "";
                    textBox29.Text = "";
                    textBox30.Text = "";
                    //textBox32.Text = "";
                    //textBox33.Text = "";
                    txtRefHum2Prob3.Text = "";
                    txtRefHum3Prob3.Text = "";
                    //txtRefHum4Prob3.Text = "";

                    break;
                case 2:

                    panel9.Enabled = false;
                    panel10.Enabled = true;
                    panel11.Enabled = true;
                    radioButton5.Checked = false;
                    radioButton6.Checked = false;
                    radioButton7.Checked = true;
                    radioButton8.Checked = false;
                    textBox26.Text = "";
                    textBox27.Text = "";
                    txtRefHum2Prob3.Text = "";
                   
                    break;
                case 3:
                    panel9.Enabled = true;
                    panel10.Enabled = true;
                    panel11.Enabled = true;
                    radioButton5.Checked = false;
                    radioButton6.Checked = false;
                    radioButton7.Checked = false;
                    radioButton8.Checked = true;
                  
                    break;
            }
        }

        private void cbbNumCalPoint4_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cbbNumCalPoint4.SelectedIndex)
            {
                case 0:

                    panel13.Enabled = false;
                    panel14.Enabled = false;
                    panel15.Enabled = false;
                    radioButton13.Checked = true;
                    radioButton14.Checked = false;
                    radioButton15.Checked = false;
                    radioButton16.Checked = false;
                    textBox38.Text = "";
                    textBox39.Text = "";
                    textBox41.Text = "";
                    textBox42.Text = "";
                    textBox44.Text = "";
                    textBox45.Text = "";
                    txtRefHum2Prob4.Text = "";
                    txtRefHum3Prob4.Text = "";
                    txtRefHum4Prob4.Text = "";

                    break;
                case 1:
                   
                    textBox41.Text = "";
                    textBox42.Text = "";
                    textBox44.Text = "";
                    textBox45.Text = "";
                    txtRefHum2Prob4.Text = "";
                    txtRefHum3Prob4.Text = "";
                    panel13.Enabled = false;
                    panel14.Enabled = false;
                    panel15.Enabled = true;
                    radioButton13.Checked = false;
                    radioButton14.Checked = true;
                    radioButton15.Checked = false;
                    radioButton16.Checked = false;
                    break;
                case 2:
                  
                    textBox44.Text = "";
                    textBox45.Text = "";
                    txtRefHum2Prob4.Text = "";
                    panel13.Enabled = false;
                    panel14.Enabled = true;
                    panel15.Enabled = true;
                    radioButton13.Checked = false;
                    radioButton14.Checked = false;
                    radioButton15.Checked = true;
                    radioButton16.Checked = false;
                    break;
                case 3:

                    panel13.Enabled = true;
                    panel14.Enabled = true;
                    panel15.Enabled = true;
                    radioButton13.Checked = false;
                    radioButton14.Checked = false;
                    radioButton15.Checked = false;
                    radioButton16.Checked = true;

                    break;
            }
        }

        private void grbCalTemp4_Enter(object sender, EventArgs e)
        {

        }
        //byte[] dataCalib = new byte[8];
        double a, b, c, d;
        double detA, detB, detC, detD;
        double DetT ;
        //double[,] matrix;// = new double[4, 4];
        //double[,] matrixa;//= new double[4, 4];
        //double[,] matrixb;//= new double[4, 4];
        //double[,] matrixc;//= new double[4, 4];
        //double[,] matrixd;//= new double[4, 4];
        double T1, T2, T3, T4; //Temp sensor
        double X1, X2, X3, X4;// sensor rH
        double Y1, Y2, Y3, Y4;//reference rH
        private CultureInfo cul;

        /// <summary>
        /// Calculator a,b,c,d with 2point
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <param name="x1"></param>
        /// <param name="x2"></param>
        /// <param name="y1"></param>
        /// <param name="y2"></param>
        void Calculator2point(double t1, double t2, double x1,double x2, double y1, double y2)
        {
            int n = 2;
            double[,] matrix= new double[n, n];
            double[,] matrixa= new double[n, n];
            double[,] matrixb= new double[n, n];
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
            c = d = 0;
        }
        void Calculator3Point(double t1, double t2, double t3, double x1, double x2,double x3, double y1, double y2, double y3)
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

            DetT = Det(matrix, 3);
            detA = Det(matrixa, 3);
            detB = Det(matrixb, 3);
            detC = Det(matrixc, 3);
            a = detA / DetT;
            b = detB / DetT;
            c = detC / DetT;
            d = 0;
        }

        void Calculator4Point(double t1, double t2, double t3,double t4, double x1, double x2, double x3, double x4, double y1, 
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
        void ValidatePoint1()
        {
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

        }
        void ValidatePoint2()
        {
            ValidatePoint1();
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
        }
        void ValidatePoint3()
        {
            ValidatePoint2();
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
        }
        void ValidatePoint4()
        {
            ValidatePoint3();
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
        }
        private void button1_Click(object sender, EventArgs e)//Calib channel 1
        {
            bool success = false;
            byte selectedChannel = 0;
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
                        if (String.IsNullOrEmpty(textBox3.Text.Trim()))
                        {
                            MessageBox.Show("Enter or Click Read value of humidity in Point 3");
                            textBox3.Focus();
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
                        if (String.IsNullOrEmpty(textBox3.Text.Trim()))
                        {
                            MessageBox.Show("Enter or Click Read value of humidity in Point 3");
                            textBox3.Focus();
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
                        a = 1;
                        b = double.Parse(txtRefHum1Prob1.Text, CultureInfo.InvariantCulture) - double.Parse(textBox7.Text, CultureInfo.InvariantCulture);
                        c = d = 0;
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

                success = WriteDataCalib(selectedChannel);
                if (success)
                {
                    //if (a > 32000)
                    //{
                    //    a = a - 65536;
                    //}
                    //if (b > 32000)
                    //{
                    //    b = b - 65536;
                    //}
                    //if (c > 32000)
                    //{
                    //    c = c - 65536;
                    //}
                    //if (d > 32000)
                    //{
                    //    d = d - 65536;
                    //}
                    MessageBox.Show("Calib humidity successful!");
                    return;
                }
                else
                {
                    MessageBox.Show("Calib Fail! Try again");
                    return;
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
                byte bufDt = (byte)(Math.Abs(data) * 10);
                byte dau = 0;
                if (textBox51.Text.Contains("-"))
                {
                    dau = 1;
                }

                if (bufDt >= 255)
                {
                    MessageBox.Show("Data offset cannot over 25.5");
                    textBox51.Text = "";
                    return;
                }
                try
                {
                    dv35.Open(host);
                    Thread.Sleep(500);
                    if (!dv35.writeCalibOffset((byte)(selectedChannel + 1), bufDt, dau))
                    {
                        MessageBox.Show("Setting Calib fail");
                        
                        return;
                    }
                    else
                    {
                        MessageBox.Show("Setting Calib successful");
                        
                    }
                }
                catch 
                {
                    MessageBox.Show("Setting Calib successful");
                }
                finally { }
               
            }
            btnCalProb1.Enabled = false;
            btnCalProb1.Refresh();
            Thread.Sleep(2000);
            btnCalProb1.Enabled = true;

        }
        double[] TimPhuongTrinh(List<double> X, List<double> Y, List<double> T)
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
            double[,] ata = new double[2, 2] { { ata1, ata2 }, { ata3, ata4 } };
            double[] atb = new double[2] { atb1, atb2 };
            detA = Det(ata, 2);
            c = d = 0;
            if (detA != 0)
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
        //double Corrected_rH_at_30(double RH, double T)
        //{
        //    return RH + 0.05 * (T - 30);
        //}
        double Corrected_rH_at_30(double RH, double T)
        {
            return RH + 0.05 * (T - 30);
        }
        private void btnReadProb1_Click(object sender, EventArgs e)
        {
            if (!ReadInfoDatacalib())
            {
                MessageBox.Show("Logger is not in calib mode");
                Close();
                return;
            }

            //this.Refresh();
            //grbCalTemp1.Refresh();
            //grbCalHum1.Refresh();
            if (channels[0].sensor == Sensor.No_sensor)
            {
                listGrbTem[0].Enabled = false;
                listGrbTem[0].BringToFront();
                listGrbTem[0].Visible = false;
                listGrbHum[0].Enabled = false;
                listButtonRead[0].Visible = false;
                listButtonCalib[0].Enabled = false;
            }
            else
            {
                if (channels[0].sensor == Sensor.Temp_sensor)
                {
                    //listButtonRead[0].Visible = false;
                    //listButtonCalib[0].Enabled = true;
                    //listGrbHum[0].SendToBack();
                    //listGrbHum[0].Visible = true;
                    //listGrbTem[0].BringToFront();
                    //listGrbTem[0].Enabled = true;
                    //listGrbTem[0].Visible = true;
                    grbCalHum1.SendToBack();
                    grbCalHum1.Visible = true;

                    grbCalTemp1.Select();
                    grbCalTemp1.BringToFront();
                    grbCalTemp1.Refresh();
                    grbCalTemp1.Enabled = true;

                    grbCalTemp1.Visible = true;
                    grbCalTemp1.Enabled = true;
                    textBox51.Enabled = true;
                    btnReadProb1.Visible = false;
                    btnCalProb1.Enabled = true;
                    ////fill value
                    //listTextboxCalTemp[0].Text = channels[0].temp.ToString();

                }
                else if (channels[0].sensor == Sensor.Humid_sensor)
                {
                  
                    //cbbNumCalPoint1.SelectedIndex = 1;
                    btnReadProb1.Visible = true;
                    btnReadProb1.Enabled = true;
                    btnCalProb1.Enabled = true;
                    grbCalHum1.BringToFront();
                    grbCalHum1.Visible = true;
                    grbCalHum1.Enabled = true;
                    grbCalTemp1.SendToBack();
                    grbCalTemp1.Visible = false;
                   
                    //Fill value to point row
                    int rdg1Checked = 0;
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
                            textBox8.Text = channels[0].temp.ToString();
                            textBox7.Text = channels[0].rHNoOffset.ToString();
                            break;
                        case 1:
                            textBox6.Text = channels[0].temp.ToString();
                            textBox5.Text = channels[0].rHNoOffset.ToString();
                            break;
                        case 2:
                            textBox4.Text = channels[0].temp.ToString();
                            textBox3.Text = channels[0].rHNoOffset.ToString();
                            break;
                        case 3:
                            textBox2.Text = channels[0].temp.ToString();
                            textBox1.Text = channels[0].rHNoOffset.ToString();
                            break;
                    }
                }
            }

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

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            if (rb.Checked)
            {
                foreach (RadioButton other in rdbGroup3)
                {
                    if (other == rb)
                    {
                        continue;
                    }
                    other.Checked = false;
                }
            }
        }

        private void radioButton9_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            if (rb.Checked)
            {
                foreach (RadioButton other in rdbGroup2)
                {
                    if (other == rb)
                    {
                        continue;
                    }
                    other.Checked = false;
                }
            }
        }

        private void radioButton13_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            if (rb.Checked)
            {
                foreach (RadioButton other in rdbGroup4)
                {
                    if (other == rb)
                    {
                        continue;
                    }
                    other.Checked = false;
                }
            }
        }

        private void btnReadProb2_Click(object sender, EventArgs e)
        {
            if (!ReadInfoDatacalib())
            {
                MessageBox.Show("Logger is not in calib mode");
                Close();
                return;
            }
            if (channels[1].sensor == Sensor.No_sensor)
            {
                listGrbTem[1].Enabled = false;
                listGrbTem[1].BringToFront();
                listGrbTem[1].Visible = false;
                listGrbHum[1].Enabled = false;
                listButtonRead[1].Visible = false;
                listButtonCalib[1].Enabled = false;
            }
            else
            {
               
                if (channels[1].sensor == Sensor.Temp_sensor)
                {
                    listButtonRead[1].Visible = false;
                    listButtonCalib[1].Enabled = true;
                    listGrbTem[1].BringToFront();
                    listGrbTem[1].Enabled = true;
                    listGrbTem[1].Visible = true;
                    listGrbHum[1].SendToBack();
                    listGrbHum[1].Visible = true;
                    //fill value
                    //listTextboxCalTemp[1].Text = channels[1].temp.ToString();

                }
                else if (channels[1].sensor == Sensor.Humid_sensor)
                {
                    listButtonRead[1].Visible = true;
                    listButtonCalib[1].Enabled = true;
                    listButtonRead[1].Enabled = true;
                    listGrbHum[1].BringToFront();
                    listGrbHum[1].Visible = true;
                    listGrbHum[1].Enabled = true;
                    listGrbTem[1].SendToBack();
                    listGrbTem[1].Visible = false;
                    //Fill value to point row
                    int rdg1Checked = 0;
                    for (int r = 0; r < 4; r++)
                    {
                        if (rdbGroup2[r].Checked)
                        {
                            rdg1Checked = r;
                            break;
                        }
                    }

                    switch (rdg1Checked)
                    {
                        case 0:
                            textBox24.Text = channels[1].temp.ToString();
                            textBox23.Text = channels[1].rHNoOffset.ToString();
                            break;
                        case 1:
                            textBox21.Text = channels[1].temp.ToString();
                            textBox20.Text = channels[1].rHNoOffset.ToString();
                            break;
                        case 2:
                            textBox18.Text = channels[1].temp.ToString();
                            textBox17.Text = channels[1].rHNoOffset.ToString();
                            break;
                        case 3:
                            textBox15.Text = channels[1].temp.ToString();
                            textBox14.Text = channels[1].rHNoOffset.ToString();
                            break;
                    }
                }
            }
        }

        private void btnReadProb3_Click(object sender, EventArgs e)
        {
            if (!ReadInfoDatacalib())
            {
                MessageBox.Show("Logger is not in calib mode");
                Close();
                return;
            }
            if (channels[2].sensor == Sensor.No_sensor)
            {
                listGrbTem[2].Enabled = false;
                listGrbTem[2].BringToFront();
                listGrbTem[2].Visible = false;
                listGrbHum[2].Enabled = false;
                listButtonRead[2].Visible = false;
                listButtonCalib[2].Enabled = false;
            }
            else
            {
               
                if (channels[2].sensor == Sensor.Temp_sensor)
                {
                    listButtonRead[2].Visible = false;
                    listButtonCalib[2].Enabled = true;
                    listGrbTem[2].BringToFront();
                    listGrbTem[2].Enabled = true;
                    listGrbTem[2].Visible = true;
                    listGrbHum[2].SendToBack();
                    listGrbHum[2].Visible = true;
                    //fill value
                    //listTextboxCalTemp[2].Text = channels[2].temp.ToString();

                }
                else if (channels[2].sensor == Sensor.Humid_sensor)
                {
                    listButtonRead[2].Visible = true;
                    listButtonCalib[2].Enabled = true;
                    listButtonRead[2].Enabled = true;
                    listGrbHum[2].BringToFront();
                    listGrbHum[2].Visible = true;
                    listGrbHum[2].Enabled = true;
                    listGrbTem[2].SendToBack();
                    listGrbTem[2].Visible = false;
                    //Fill value to point row
                    int rdg1Checked = 0;
                    for (int r = 0; r < 4; r++)
                    {
                        if (rdbGroup3[r].Checked)
                        {
                            rdg1Checked = r;
                            break;
                        }
                    }

                    switch (rdg1Checked)
                    {
                        case 0:
                            textBox36.Text = channels[2].temp.ToString();
                            textBox35.Text = channels[2].rHNoOffset.ToString();
                            break;
                        case 1:
                            textBox33.Text = channels[2].temp.ToString();
                            textBox32.Text = channels[2].rHNoOffset.ToString();
                            break;
                        case 2:
                            textBox30.Text = channels[2].temp.ToString();
                            textBox29.Text = channels[2].rHNoOffset.ToString();
                            break;
                        case 3:
                            textBox27.Text = channels[2].temp.ToString();
                            textBox26.Text = channels[2].rHNoOffset.ToString();
                            break;
                    }
                }
            }
        }

        private void btnReadProb4_Click(object sender, EventArgs e)
        {
            if (!ReadInfoDatacalib())
            {
                MessageBox.Show("Logger is not in calib mode");
                Close(); return;
            }
            if (channels[3].sensor == Sensor.No_sensor)
            {
                listGrbTem[3].Enabled = false;
                listGrbTem[3].BringToFront();
                listGrbTem[3].Visible = false;
                listGrbHum[3].Enabled = false;
                listButtonRead[3].Visible = false;
                listButtonCalib[3].Enabled = false;
            }
            else
            {
                
                if (channels[3].sensor == Sensor.Temp_sensor)
                {
                    listButtonRead[3].Visible = false;
                    listButtonCalib[3].Enabled = true;
                    listGrbTem[3].BringToFront();
                    listGrbTem[3].Enabled = true;
                    listGrbTem[3].Visible = true;
                    listGrbHum[3].SendToBack();
                    listGrbHum[3].Visible = true;
                    //fill value
                    //listTextboxCalTemp[3].Text = channels[3].temp.ToString();
                }
                else if (channels[3].sensor == Sensor.Humid_sensor)
                {
                    listButtonRead[3].Visible = true;
                    listButtonCalib[3].Enabled = true;
                    listButtonRead[3].Enabled = true;
                    listGrbHum[3].BringToFront();
                    listGrbHum[3].Visible = true;
                    listGrbHum[3].Enabled = true;
                    listGrbTem[3].SendToBack();
                    listGrbTem[3].Visible = false;
                    //Fill value to point row
                    int rdg1Checked = 0;
                    for (int r = 0; r < 4; r++)
                    {
                        if (rdbGroup4[r].Checked)
                        {
                            rdg1Checked = r;
                            break;
                        }
                    }

                    switch (rdg1Checked)
                    {
                        case 0:
                            textBox48.Text = channels[3].temp.ToString();
                            textBox47.Text = channels[3].rHNoOffset.ToString();
                            break;
                        case 1:
                            textBox45.Text = channels[3].temp.ToString();
                            textBox44.Text = channels[3].rHNoOffset.ToString();
                            break;
                        case 2:
                            textBox42.Text = channels[3].temp.ToString();
                            textBox41.Text = channels[3].rHNoOffset.ToString();
                            break;
                        case 3:
                            textBox39.Text = channels[3].temp.ToString();
                            textBox38.Text = channels[3].rHNoOffset.ToString();
                            break;
                    }
                }
            }
        }

        private void btnCalProb2_Click(object sender, EventArgs e)
        {
            bool success = false;
            byte selectedChannel = 1;
            if (channels[selectedChannel].sensor == Sensor.Humid_sensor)
            {
                //Calib Humid
                //1 point 
                if (String.IsNullOrEmpty(textBox24.Text.Trim()))
                {
                    MessageBox.Show("Enter or Click Read value of temperature in Point 1");
                    textBox24.Focus();
                    return;
                }
                if (String.IsNullOrEmpty(textBox23.Text.Trim()))
                {
                    MessageBox.Show("Enter or Click Read value of humiddity in point 1");
                    textBox23.Focus();
                    return;
                }
                if (String.IsNullOrEmpty(txtRefHum1Prob2.Text.Trim()))
                {
                    MessageBox.Show("Referece rH can not be empty");
                    txtRefHum1Prob2.Focus();
                    return;
                }
                switch (cbbNumCalPoint2.SelectedIndex)
                {
                    case 1:
                       
                        //2 point 
                        if (String.IsNullOrEmpty(textBox21.Text.Trim()))
                        {
                            MessageBox.Show("Enter or Click Read value of temperature in Point 2");
                            textBox21.Focus();
                            return;
                        }
                        if (String.IsNullOrEmpty(textBox20.Text.Trim()))
                        {
                            MessageBox.Show("Enter or Click Read value of humiddity int Point 2");
                            textBox20.Focus();
                            return;
                        }
                        if (String.IsNullOrEmpty(txtRefHum2Prob2.Text.Trim()))
                        {
                            MessageBox.Show("Referece rH can not be empty");
                            txtRefHum2Prob2.Focus();
                            return;
                        }
                        break;
                    case 2:

                        //2 point 
                        if (String.IsNullOrEmpty(textBox21.Text.Trim()))
                        {
                            MessageBox.Show("Enter or Click Read value of temperature in Point 2");
                            textBox21.Focus();
                            return;
                        }
                        if (String.IsNullOrEmpty(textBox20.Text.Trim()))
                        {
                            MessageBox.Show("Enter or Click Read value of humiddity int Point 2");
                            textBox20.Focus();
                            return;
                        }
                        if (String.IsNullOrEmpty(txtRefHum2Prob2.Text.Trim()))
                        {
                            MessageBox.Show("Referece rH can not be empty");
                            txtRefHum2Prob2.Focus();
                            return;
                        }
                       
                        //ValidatePoint3();
                        if (String.IsNullOrEmpty(textBox18.Text.Trim()))
                        {
                            MessageBox.Show("Enter or Click Read value of temperature in Point 3");
                            textBox18.Focus();
                            return;
                        }
                        if (String.IsNullOrEmpty(textBox17.Text.Trim()))
                        {
                            MessageBox.Show("Enter or Click Read value of humidity in Point 3");
                            textBox17.Focus();
                            return;
                        }
                        if (String.IsNullOrEmpty(txtRefHum3Prob2.Text.Trim()))
                        {
                            MessageBox.Show("Referece rH can not be empty");
                            txtRefHum3Prob3.Focus();
                            return;
                        }
                        break;
                    case 3:
                        //2 point 
                        if (String.IsNullOrEmpty(textBox21.Text.Trim()))
                        {
                            MessageBox.Show("Enter or Click Read value of temperature in Point 2");
                            textBox21.Focus();
                            return;
                        }
                        if (String.IsNullOrEmpty(textBox20.Text.Trim()))
                        {
                            MessageBox.Show("Enter or Click Read value of humiddity int Point 2");
                            textBox20.Focus();
                            return;
                        }
                        if (String.IsNullOrEmpty(txtRefHum2Prob2.Text.Trim()))
                        {
                            MessageBox.Show("Referece rH can not be empty");
                            txtRefHum2Prob2.Focus();
                            return;
                        }

                        //ValidatePoint3();
                        if (String.IsNullOrEmpty(textBox18.Text.Trim()))
                        {
                            MessageBox.Show("Enter or Click Read value of temperature in Point 3");
                            textBox18.Focus();
                            return;
                        }
                        if (String.IsNullOrEmpty(textBox17.Text.Trim()))
                        {
                            MessageBox.Show("Enter or Click Read value of humidity in Point 3");
                            textBox17.Focus();
                            return;
                        }
                        if (String.IsNullOrEmpty(txtRefHum3Prob2.Text.Trim()))
                        {
                            MessageBox.Show("Referece rH can not be empty");
                            txtRefHum3Prob3.Focus();
                            return;
                        }
                        //point 4
                        if (String.IsNullOrEmpty(textBox15.Text.Trim()))
                        {
                            MessageBox.Show("Enter or Click Read value of temperature in Point 3");
                            textBox15.Focus();
                            return;
                        }
                        if (String.IsNullOrEmpty(textBox14.Text.Trim()))
                        {
                            MessageBox.Show("Enter or Click Read value of humidity in Point 3");
                            textBox14.Focus();
                            return;
                        }
                        if (String.IsNullOrEmpty(txtRefHum4Prob2.Text.Trim()))
                        {
                            MessageBox.Show("Referece rH can not be empty");
                            txtRefHum4Prob2.Focus();
                            return;
                        }
                        break;
                }
                //start Calib humid

                switch (cbbNumCalPoint2.SelectedIndex)
                {
                    case 0:
                        a = 1;
                        d = c = 0;
                        b = double.Parse(txtRefHum1Prob2.Text, CultureInfo.InvariantCulture) - double.Parse(textBox23.Text, CultureInfo.InvariantCulture);

                        break;
                    case 1:
                        // 1 Tinh X1,X2, Y1, Y2 chuyen qua 30oC =>Next
                        // 2: thanh lap 3 ma tran, tim ra 3 dinh thuc
                        // 3: giai he 2 phuong trinh tim ra he so ab
                        //Pt: AX1+B=Y1
                        //    AX2+B=Y2

                        T1 = double.Parse(textBox24.Text, CultureInfo.InvariantCulture);
                        T2 = double.Parse(textBox21.Text, CultureInfo.InvariantCulture);
                        X1 = double.Parse(textBox23.Text, CultureInfo.InvariantCulture);
                        X2 = double.Parse(textBox20.Text, CultureInfo.InvariantCulture);
                        Y1 = double.Parse(txtRefHum1Prob2.Text, CultureInfo.InvariantCulture);
                        Y2 = double.Parse(txtRefHum2Prob2.Text, CultureInfo.InvariantCulture);

                        //Calculator ab,c,d when Point =2
                        Calculator2point(T1, T2, X1, X2, Y1, Y2);

                        break;
                    case 2:

                        T1 = double.Parse(textBox24.Text, CultureInfo.InvariantCulture);
                        T2 = double.Parse(textBox21.Text, CultureInfo.InvariantCulture);
                        T3 = double.Parse(textBox18.Text, CultureInfo.InvariantCulture);

                        X1 = double.Parse(textBox23.Text, CultureInfo.InvariantCulture);
                        X2 = double.Parse(textBox20.Text, CultureInfo.InvariantCulture);
                        X3 = double.Parse(textBox17.Text, CultureInfo.InvariantCulture);

                        Y1 = double.Parse(txtRefHum1Prob2.Text, CultureInfo.InvariantCulture);
                        Y2 = double.Parse(txtRefHum2Prob2.Text, CultureInfo.InvariantCulture);
                        Y3 = double.Parse(txtRefHum3Prob2.Text, CultureInfo.InvariantCulture);

                        List<double> X = new List<double>() { X1, X2, X3 };
                        List<double> Y = new List<double>() { Y1, Y2, Y3 };
                        List<double> T = new List<double>() { T1, T2, T3 };
                        TimPhuongTrinh(X, Y, T);
                        //Calculator3Point(T1, T2, T3, X1, X2, X3, Y1, Y2, Y3);//Giai he phuong trinh bac 2
                        break;
                    case 3:
                        //giai he 4 phuong trinh tim ra he so abcd
                        T1 = double.Parse(textBox24.Text, CultureInfo.InvariantCulture);
                        T2 = double.Parse(textBox21.Text, CultureInfo.InvariantCulture);
                        T3 = double.Parse(textBox18.Text, CultureInfo.InvariantCulture);
                        T4 = double.Parse(textBox15.Text, CultureInfo.InvariantCulture);

                        X1 = double.Parse(textBox23.Text, CultureInfo.InvariantCulture);
                        X2 = double.Parse(textBox20.Text, CultureInfo.InvariantCulture);
                        X3 = double.Parse(textBox17.Text, CultureInfo.InvariantCulture);
                        X4 = double.Parse(textBox14.Text, CultureInfo.InvariantCulture);

                        Y1 = double.Parse(txtRefHum1Prob2.Text, CultureInfo.InvariantCulture);
                        Y2 = double.Parse(txtRefHum2Prob2.Text, CultureInfo.InvariantCulture);
                        Y3 = double.Parse(txtRefHum3Prob2.Text, CultureInfo.InvariantCulture);
                        Y4 = double.Parse(txtRefHum4Prob2.Text, CultureInfo.InvariantCulture);

                        //Calculator4Point(T1, T2, T3, T4, X1, X2, X3, X4, Y1, Y2, Y3, Y4);
                        X = new List<double>() { X1, X2, X3, X4 };
                        Y = new List<double>() { Y1, Y2, Y3, Y4 };
                        T = new List<double>() { T1, T2, T3, T4 };
                        TimPhuongTrinh(X, Y, T);
                        break;
                }

                success = WriteDataCalib(selectedChannel);
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

                    MessageBox.Show("Calib humidity successful!");
                    return;
                }
                else
                {
                    MessageBox.Show("Calib Fail! Try again");
                    return;
                }
            }
            else if (channels[selectedChannel].sensor == Sensor.Temp_sensor)
            {
                //Calib Temp
                if (String.IsNullOrEmpty(txtReferenceC21.Text.Trim()))
                {
                    MessageBox.Show("Offset can not be empty");
                    txtReferenceC21.Focus();
                    return;
                }

                double data = double.Parse(textBox52.Text, CultureInfo.InvariantCulture);
                byte bufDt = (byte)(Math.Abs(data) * 10);
                byte dau = 0;
                if (textBox52.Text.Contains("-"))
                {
                    dau = 1;
                }

                if (bufDt >= 255)
                {
                    MessageBox.Show("Data offset cannot over 25.5");
                    textBox52.Text = "";
                    return;
                }
                try
                {
                    dv35.Open(host);
                    //HIDFunction.hid_SetNonBlocking(dv35.dev, 1);
                    Thread.Sleep(500);
                    if (!dv35.writeCalibOffset((byte)(selectedChannel + 1), bufDt, dau))
                    {
                        MessageBox.Show("Setting Calib fail");
                        return;
                    }
                    else
                    {
                        MessageBox.Show("Setting Calib successful");
                    }

                }
                catch
                {
                    MessageBox.Show("Setting Calib successful");
                }
                finally { dv35.Close(); }
               
            }
            btnCalProb2.Enabled = false;
            btnCalProb2.Refresh();
            Thread.Sleep(2000);
            btnCalProb2.Enabled = true;


        }

        private void btnCalProb3_Click(object sender, EventArgs e)
        {
            bool success = false;
            byte selectedChannel = 2;//Probe 3
            if (channels[selectedChannel].sensor == Sensor.Humid_sensor)
            {
                //Calib Humid
                //1 point 
                if (String.IsNullOrEmpty(textBox36.Text.Trim()))
                {
                    MessageBox.Show("Enter or Click Read value of temperature in Point 1");
                    textBox36.Focus();
                    return;
                }
                if (String.IsNullOrEmpty(textBox35.Text.Trim()))
                {
                    MessageBox.Show("Enter or Click Read value of humiddity in point 1");
                    textBox35.Focus();
                    return;
                }
                if (String.IsNullOrEmpty(txtRefHum1Prob3.Text.Trim()))
                {
                    MessageBox.Show("Referece rH can not be empty");
                    txtRefHum1Prob3.Focus();
                    return;
                }
                switch (cbbNumCalPoint3.SelectedIndex)
                {
                    case 1:
                        //1 point

                        if (String.IsNullOrEmpty(textBox36.Text.Trim()))
                        {
                            MessageBox.Show("Enter or Click Read value of temperature in Point 1");
                            textBox36.Focus();
                            return;
                        }
                        if (String.IsNullOrEmpty(textBox35.Text.Trim()))
                        {
                            MessageBox.Show("Enter or Click Read value of humiddity in point 1");
                            textBox35.Focus();
                            return;
                        }
                        if (String.IsNullOrEmpty(txtRefHum1Prob3.Text.Trim()))
                        {
                            MessageBox.Show("Referece rH can not be empty");
                            txtRefHum1Prob3.Focus();
                            return;
                        }
                        //2 point 
                        //ValidatePoint2();
                        if (String.IsNullOrEmpty(textBox33.Text.Trim()))
                        {
                            MessageBox.Show("Enter or Click Read value of temperature in Point 2");
                            textBox33.Focus();
                            return;
                        }
                        if (String.IsNullOrEmpty(textBox32.Text.Trim()))
                        {
                            MessageBox.Show("Enter or Click Read value of humiddity int Point 2");
                            textBox32.Focus();
                            return;
                        }
                        if (String.IsNullOrEmpty(txtRefHum2Prob3.Text.Trim()))
                        {
                            MessageBox.Show("Referece rH can not be empty");
                            txtRefHum2Prob3.Focus();
                            return;
                        }
                        break;
                    case 2:
                        if (String.IsNullOrEmpty(textBox36.Text.Trim()))
                        {
                            MessageBox.Show("Enter or Click Read value of temperature in Point 1");
                            textBox36.Focus();
                            return;
                        }
                        if (String.IsNullOrEmpty(textBox35.Text.Trim()))
                        {
                            MessageBox.Show("Enter or Click Read value of humiddity in point 1");
                            textBox35.Focus();
                            return;
                        }
                        if (String.IsNullOrEmpty(txtRefHum1Prob3.Text.Trim()))
                        {
                            MessageBox.Show("Referece rH can not be empty");
                            txtRefHum1Prob3.Focus();
                            return;
                        }
                        //2 point 
                        //ValidatePoint2();
                        if (String.IsNullOrEmpty(textBox33.Text.Trim()))
                        {
                            MessageBox.Show("Enter or Click Read value of temperature in Point 2");
                            textBox33.Focus();
                            return;
                        }
                        if (String.IsNullOrEmpty(textBox32.Text.Trim()))
                        {
                            MessageBox.Show("Enter or Click Read value of humiddity int Point 2");
                            textBox32.Focus();
                            return;
                        }
                        if (String.IsNullOrEmpty(txtRefHum2Prob3.Text.Trim()))
                        {
                            MessageBox.Show("Referece rH can not be empty");
                            txtRefHum2Prob3.Focus();
                            return;
                        }
                        //ValidatePoint3();
                        if (String.IsNullOrEmpty(textBox30.Text.Trim()))
                        {
                            MessageBox.Show("Enter or Click Read value of temperature in Point 3");
                            textBox30.Focus();
                            return;
                        }
                        if (String.IsNullOrEmpty(textBox29.Text.Trim()))
                        {
                            MessageBox.Show("Enter or Click Read value of humidity in Point 3");
                            textBox29.Focus();
                            return;
                        }
                        if (String.IsNullOrEmpty(txtRefHum3Prob3.Text.Trim()))
                        {
                            MessageBox.Show("Referece rH can not be empty");
                            txtRefHum3Prob3.Focus();
                            return;
                        }
                        break;
                    case 3:
                        if (String.IsNullOrEmpty(textBox36.Text.Trim()))
                        {
                            MessageBox.Show("Enter or Click Read value of temperature in Point 1");
                            textBox36.Focus();
                            return;
                        }
                        if (String.IsNullOrEmpty(textBox35.Text.Trim()))
                        {
                            MessageBox.Show("Enter or Click Read value of humiddity in point 1");
                            textBox35.Focus();
                            return;
                        }
                        if (String.IsNullOrEmpty(txtRefHum1Prob3.Text.Trim()))
                        {
                            MessageBox.Show("Referece rH can not be empty");
                            txtRefHum1Prob3.Focus();
                            return;
                        }
                        //2 point 
                        //ValidatePoint2();
                        if (String.IsNullOrEmpty(textBox33.Text.Trim()))
                        {
                            MessageBox.Show("Enter or Click Read value of temperature in Point 2");
                            textBox33.Focus();
                            return;
                        }
                        if (String.IsNullOrEmpty(textBox32.Text.Trim()))
                        {
                            MessageBox.Show("Enter or Click Read value of humiddity int Point 2");
                            textBox32.Focus();
                            return;
                        }
                        if (String.IsNullOrEmpty(txtRefHum2Prob3.Text.Trim()))
                        {
                            MessageBox.Show("Referece rH can not be empty");
                            txtRefHum2Prob3.Focus();
                            return;
                        }
                        //ValidatePoint3();
                        if (String.IsNullOrEmpty(textBox30.Text.Trim()))
                        {
                            MessageBox.Show("Enter or Click Read value of temperature in Point 3");
                            textBox30.Focus();
                            return;
                        }
                        if (String.IsNullOrEmpty(textBox29.Text.Trim()))
                        {
                            MessageBox.Show("Enter or Click Read value of humidity in Point 3");
                            textBox29.Focus();
                            return;
                        }
                        if (String.IsNullOrEmpty(txtRefHum3Prob3.Text.Trim()))
                        {
                            MessageBox.Show("Referece rH can not be empty");
                            txtRefHum3Prob3.Focus();
                            return;
                        }
                        //point 4
                        if (String.IsNullOrEmpty(textBox27.Text.Trim()))
                        {
                            MessageBox.Show("Enter or Click Read value of temperature in Point 3");
                            textBox27.Focus();
                            return;
                        }
                        if (String.IsNullOrEmpty(textBox26.Text.Trim()))
                        {
                            MessageBox.Show("Enter or Click Read value of humidity in Point 3");
                            textBox26.Focus();
                            return;
                        }
                        if (String.IsNullOrEmpty(txtRefHum4Prob3.Text.Trim()))
                        {
                            MessageBox.Show("Referece rH can not be empty");
                            txtRefHum4Prob3.Focus();
                            return;
                        }
                        break;
                }
                //start Calib humid

                switch (cbbNumCalPoint3.SelectedIndex)
                {
                    case 0:
                        a = 1;
                        d = c = 0;
                        b = double.Parse(txtRefHum1Prob3.Text, CultureInfo.InvariantCulture) - double.Parse(textBox35.Text, CultureInfo.InvariantCulture);

                        break;
                    case 1:
                        // 1 Tinh X1,X2, Y1, Y2 chuyen qua 30oC =>Next
                        // 2: thanh lap 3 ma tran, tim ra 3 dinh thuc
                        // 3: giai he 2 phuong trinh tim ra he so ab
                        //Pt: AX1+B=Y1
                        //    AX2+B=Y2

                        T1 = double.Parse(textBox36.Text, CultureInfo.InvariantCulture);
                        T2 = double.Parse(textBox33.Text, CultureInfo.InvariantCulture);
                        X1 = double.Parse(textBox35.Text, CultureInfo.InvariantCulture);
                        X2 = double.Parse(textBox32.Text, CultureInfo.InvariantCulture);
                        Y1 = double.Parse(txtRefHum1Prob3.Text, CultureInfo.InvariantCulture);
                        Y2 = double.Parse(txtRefHum2Prob3.Text, CultureInfo.InvariantCulture);

                        //Calculator ab,c,d when Point =2
                        Calculator2point(T1, T2, X1, X2, Y1, Y2);

                        break;
                    case 2:

                        T1 = double.Parse(textBox36.Text, CultureInfo.InvariantCulture);
                        T2 = double.Parse(textBox33.Text, CultureInfo.InvariantCulture);
                        T3 = double.Parse(textBox30.Text, CultureInfo.InvariantCulture);

                        X1 = double.Parse(textBox35.Text, CultureInfo.InvariantCulture);
                        X2 = double.Parse(textBox32.Text, CultureInfo.InvariantCulture);
                        X3 = double.Parse(textBox29.Text, CultureInfo.InvariantCulture);

                        Y1 = double.Parse(txtRefHum1Prob3.Text, CultureInfo.InvariantCulture);
                        Y2 = double.Parse(txtRefHum2Prob3.Text, CultureInfo.InvariantCulture);
                        Y3 = double.Parse(txtRefHum3Prob3.Text, CultureInfo.InvariantCulture);

                        List<double> X = new List<double>() { X1, X2, X3 };
                        List<double> Y = new List<double>() { Y1, Y2, Y3 };
                        List<double> T = new List<double>() { T1, T2, T3 };
                        TimPhuongTrinh(X, Y, T);
                        //Calculator3Point(T1, T2, T3, X1, X2, X3, Y1, Y2, Y3);//Giai he phuong trinh bac 2
                        break;
                    case 3:
                        //giai he 4 phuong trinh tim ra he so abcd
                        T1 = double.Parse(textBox36.Text, CultureInfo.InvariantCulture);
                        T2 = double.Parse(textBox33.Text, CultureInfo.InvariantCulture);
                        T3 = double.Parse(textBox30.Text, CultureInfo.InvariantCulture);
                        T4 = double.Parse(textBox27.Text, CultureInfo.InvariantCulture);

                        X1 = double.Parse(textBox35.Text, CultureInfo.InvariantCulture);
                        X2 = double.Parse(textBox32.Text, CultureInfo.InvariantCulture);
                        X3 = double.Parse(textBox29.Text, CultureInfo.InvariantCulture);
                        X4 = double.Parse(textBox26.Text, CultureInfo.InvariantCulture);

                        Y1 = double.Parse(txtRefHum1Prob3.Text, CultureInfo.InvariantCulture);
                        Y2 = double.Parse(txtRefHum2Prob3.Text, CultureInfo.InvariantCulture);
                        Y3 = double.Parse(txtRefHum3Prob3.Text, CultureInfo.InvariantCulture);
                        Y4 = double.Parse(txtRefHum4Prob3.Text, CultureInfo.InvariantCulture);

                        //Calculator4Point(T1, T2, T3, T4, X1, X2, X3, X4, Y1, Y2, Y3, Y4);
                        X = new List<double>() { X1, X2, X3, X4 };
                        Y = new List<double>() { Y1, Y2, Y3, Y4 };
                        T = new List<double>() { T1, T2, T3, T4 };
                        TimPhuongTrinh(X, Y, T);
                        break;
                }

                success = WriteDataCalib(selectedChannel);
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

                    MessageBox.Show("Calib humidity successful!");
                    return;
                }
                else
                {
                    MessageBox.Show("Calib Fail! Try again");
                    return;
                }
            }
            else if (channels[selectedChannel].sensor == Sensor.Temp_sensor)
            {
                //Calib Temp
                if (String.IsNullOrEmpty(textBox55.Text.Trim()))
                {
                    MessageBox.Show("Offset can not be empty");
                    textBox55.Focus();
                    return;
                }

                double data = double.Parse(textBox55.Text, CultureInfo.InvariantCulture);
                byte bufDt = (byte)(Math.Abs(data) * 10);
                byte dau = 0;
                if (textBox55.Text.Contains("-"))
                {
                    dau = 1;
                }

                if (bufDt >= 255)
                {
                    MessageBox.Show("Data offset cannot over 25.5");
                    textBox55.Text = "";
                    return;
                }

                try
                {
                    dv35.Open(host);
                    //HIDFunction.hid_SetNonBlocking(dv35.dev, 1);
                    Thread.Sleep(500);
                    if (!dv35.writeCalibOffset((byte)(selectedChannel + 1), bufDt, dau))
                    {
                        MessageBox.Show("Setting Calib fail");
                        return;
                    }
                    else
                    {
                        MessageBox.Show("Setting Calib successful");
                    }

                }
                catch
                {
                    MessageBox.Show("Setting Calib successful");
                }
                finally { dv35.Close(); }

            }
            btnCalProb1.Enabled = false;
            btnCalProb1.Refresh();
            Thread.Sleep(2000);
            btnCalProb1.Enabled = true;
        }

        private void btnCalProb4_Click(object sender, EventArgs e)
        {
            bool success = false;
            byte selectedChannel = 3;
            if (channels[selectedChannel].sensor == Sensor.Humid_sensor)
            {
                //Calib Humid
                //1 point 
                if (String.IsNullOrEmpty(textBox48.Text.Trim()))
                {
                    MessageBox.Show("Enter or Click Read value of temperature in Point 1");
                    textBox48.Focus();
                    return;
                }
                if (String.IsNullOrEmpty(textBox47.Text.Trim()))
                {
                    MessageBox.Show("Enter or Click Read value of humiddity in point 1");
                    textBox47.Focus();
                    return;
                }
                if (String.IsNullOrEmpty(txtRefHum1Prob4.Text.Trim()))
                {
                    MessageBox.Show("Referece rH can not be empty");
                    txtRefHum1Prob4.Focus();
                    return;
                }
                switch (cbbNumCalPoint4.SelectedIndex)
                {
                    case 1:
                        //1 point

                        if (String.IsNullOrEmpty(textBox48.Text.Trim()))
                        {
                            MessageBox.Show("Enter or Click Read value of temperature in Point 1");
                            textBox48.Focus();
                            return;
                        }
                        if (String.IsNullOrEmpty(textBox47.Text.Trim()))
                        {
                            MessageBox.Show("Enter or Click Read value of humiddity in point 1");
                            textBox47.Focus();
                            return;
                        }
                        if (String.IsNullOrEmpty(txtRefHum1Prob4.Text.Trim()))
                        {
                            MessageBox.Show("Referece rH can not be empty");
                            txtRefHum1Prob4.Focus();
                            return;
                        }
                        //2 point 
                        //ValidatePoint2();
                        if (String.IsNullOrEmpty(textBox45.Text.Trim()))
                        {
                            MessageBox.Show("Enter or Click Read value of temperature in Point 2");
                            textBox45.Focus();
                            return;
                        }
                        if (String.IsNullOrEmpty(textBox44.Text.Trim()))
                        {
                            MessageBox.Show("Enter or Click Read value of humiddity int Point 2");
                            textBox44.Focus();
                            return;
                        }
                        if (String.IsNullOrEmpty(txtRefHum2Prob4.Text.Trim()))
                        {
                            MessageBox.Show("Referece rH can not be empty");
                            txtRefHum2Prob4.Focus();
                            return;
                        }
                        break;
                    case 2:
                        if (String.IsNullOrEmpty(textBox48.Text.Trim()))
                        {
                            MessageBox.Show("Enter or Click Read value of temperature in Point 1");
                            textBox48.Focus();
                            return;
                        }
                        if (String.IsNullOrEmpty(textBox47.Text.Trim()))
                        {
                            MessageBox.Show("Enter or Click Read value of humiddity in point 1");
                            textBox47.Focus();
                            return;
                        }
                        if (String.IsNullOrEmpty(txtRefHum1Prob4.Text.Trim()))
                        {
                            MessageBox.Show("Referece rH can not be empty");
                            txtRefHum1Prob4.Focus();
                            return;
                        }
                        //2 point 
                        //ValidatePoint2();
                        if (String.IsNullOrEmpty(textBox45.Text.Trim()))
                        {
                            MessageBox.Show("Enter or Click Read value of temperature in Point 2");
                            textBox45.Focus();
                            return;
                        }
                        if (String.IsNullOrEmpty(textBox44.Text.Trim()))
                        {
                            MessageBox.Show("Enter or Click Read value of humiddity int Point 2");
                            textBox44.Focus();
                            return;
                        }
                        if (String.IsNullOrEmpty(txtRefHum2Prob4.Text.Trim()))
                        {
                            MessageBox.Show("Referece rH can not be empty");
                            txtRefHum2Prob4.Focus();
                            return;
                        }
                        //ValidatePoint3();
                        if (String.IsNullOrEmpty(textBox42.Text.Trim()))
                        {
                            MessageBox.Show("Enter or Click Read value of temperature in Point 3");
                            textBox42.Focus();
                            return;
                        }
                        if (String.IsNullOrEmpty(textBox41.Text.Trim()))
                        {
                            MessageBox.Show("Enter or Click Read value of humidity in Point 3");
                            textBox41.Focus();
                            return;
                        }
                        if (String.IsNullOrEmpty(txtRefHum3Prob4.Text.Trim()))
                        {
                            MessageBox.Show("Referece rH can not be empty");
                            txtRefHum3Prob4.Focus();
                            return;
                        }
                        break;
                    case 3:
                        if (String.IsNullOrEmpty(textBox48.Text.Trim()))
                        {
                            MessageBox.Show("Enter or Click Read value of temperature in Point 1");
                            textBox48.Focus();
                            return;
                        }
                        if (String.IsNullOrEmpty(textBox47.Text.Trim()))
                        {
                            MessageBox.Show("Enter or Click Read value of humiddity in point 1");
                            textBox47.Focus();
                            return;
                        }
                        if (String.IsNullOrEmpty(txtRefHum1Prob4.Text.Trim()))
                        {
                            MessageBox.Show("Referece rH can not be empty");
                            txtRefHum1Prob4.Focus();
                            return;
                        }
                        //2 point 
                        //ValidatePoint2();
                        if (String.IsNullOrEmpty(textBox45.Text.Trim()))
                        {
                            MessageBox.Show("Enter or Click Read value of temperature in Point 2");
                            textBox45.Focus();
                            return;
                        }
                        if (String.IsNullOrEmpty(textBox44.Text.Trim()))
                        {
                            MessageBox.Show("Enter or Click Read value of humiddity int Point 2");
                            textBox44.Focus();
                            return;
                        }
                        if (String.IsNullOrEmpty(txtRefHum2Prob4.Text.Trim()))
                        {
                            MessageBox.Show("Referece rH can not be empty");
                            txtRefHum2Prob4.Focus();
                            return;
                        }
                        //ValidatePoint3();
                        if (String.IsNullOrEmpty(textBox42.Text.Trim()))
                        {
                            MessageBox.Show("Enter or Click Read value of temperature in Point 3");
                            textBox42.Focus();
                            return;
                        }
                        if (String.IsNullOrEmpty(textBox41.Text.Trim()))
                        {
                            MessageBox.Show("Enter or Click Read value of humidity in Point 3");
                            textBox41.Focus();
                            return;
                        }
                        if (String.IsNullOrEmpty(txtRefHum3Prob4.Text.Trim()))
                        {
                            MessageBox.Show("Referece rH can not be empty");
                            txtRefHum3Prob4.Focus();
                            return;
                        }
                        //point 4
                        if (String.IsNullOrEmpty(textBox39.Text.Trim()))
                        {
                            MessageBox.Show("Enter or Click Read value of temperature in Point 3");
                            textBox39.Focus();
                            return;
                        }
                        if (String.IsNullOrEmpty(textBox38.Text.Trim()))
                        {
                            MessageBox.Show("Enter or Click Read value of humidity in Point 3");
                            textBox38.Focus();
                            return;
                        }
                        if (String.IsNullOrEmpty(txtRefHum4Prob4.Text.Trim()))
                        {
                            MessageBox.Show("Referece rH can not be empty");
                            txtRefHum4Prob4.Focus();
                            return;
                        }
                        break;
                }
                //start Calib humid

                switch (cbbNumCalPoint4.SelectedIndex)
                {
                    case 0:
                        a = 1;
                        d = c = 0;
                        b = double.Parse(txtRefHum1Prob4.Text, CultureInfo.InvariantCulture) - double.Parse(textBox47.Text, CultureInfo.InvariantCulture);

                        break;
                    case 1:
                        // 1 Tinh X1,X2, Y1, Y2 chuyen qua 30oC =>Next
                        // 2: thanh lap 3 ma tran, tim ra 3 dinh thuc
                        // 3: giai he 2 phuong trinh tim ra he so ab
                        //Pt: AX1+B=Y1
                        //    AX2+B=Y2

                        T1 = double.Parse(textBox48.Text, CultureInfo.InvariantCulture);
                        T2 = double.Parse(textBox45.Text, CultureInfo.InvariantCulture);
                        X1 = double.Parse(textBox47.Text, CultureInfo.InvariantCulture);
                        X2 = double.Parse(textBox44.Text, CultureInfo.InvariantCulture);
                        Y1 = double.Parse(txtRefHum1Prob4.Text, CultureInfo.InvariantCulture);
                        Y2 = double.Parse(txtRefHum2Prob4.Text, CultureInfo.InvariantCulture);

                        //Calculator ab,c,d when Point =2
                        Calculator2point(T1, T2, X1, X2, Y1, Y2);

                        break;
                    case 2:

                        T1 = double.Parse(textBox48.Text, CultureInfo.InvariantCulture);
                        T2 = double.Parse(textBox45.Text, CultureInfo.InvariantCulture);
                        T3 = double.Parse(textBox42.Text, CultureInfo.InvariantCulture);

                        X1 = double.Parse(textBox47.Text, CultureInfo.InvariantCulture);
                        X2 = double.Parse(textBox44.Text, CultureInfo.InvariantCulture);
                        X3 = double.Parse(textBox41.Text, CultureInfo.InvariantCulture);

                        Y1 = double.Parse(txtRefHum1Prob4.Text, CultureInfo.InvariantCulture);
                        Y2 = double.Parse(txtRefHum2Prob4.Text, CultureInfo.InvariantCulture);
                        Y3 = double.Parse(txtRefHum3Prob4.Text, CultureInfo.InvariantCulture);

                        List<double> X = new List<double>() { X1, X2, X3 };
                        List<double> Y = new List<double>() { Y1, Y2, Y3 };
                        List<double> T = new List<double>() { T1, T2, T3 };
                        TimPhuongTrinh(X, Y, T);
                        //Calculator3Point(T1, T2, T3, X1, X2, X3, Y1, Y2, Y3);//Giai he phuong trinh bac 2
                        break;
                    case 3:
                        //giai he 4 phuong trinh tim ra he so abcd
                        T1 = double.Parse(textBox48.Text, CultureInfo.InvariantCulture);
                        T2 = double.Parse(textBox45.Text, CultureInfo.InvariantCulture);
                        T3 = double.Parse(textBox42.Text, CultureInfo.InvariantCulture);
                        T4 = double.Parse(textBox39.Text, CultureInfo.InvariantCulture);

                        X1 = double.Parse(textBox47.Text, CultureInfo.InvariantCulture);
                        X2 = double.Parse(textBox44.Text, CultureInfo.InvariantCulture);
                        X3 = double.Parse(textBox41.Text, CultureInfo.InvariantCulture);
                        X4 = double.Parse(textBox38.Text, CultureInfo.InvariantCulture);

                        Y1 = double.Parse(txtRefHum1Prob4.Text, CultureInfo.InvariantCulture);
                        Y2 = double.Parse(txtRefHum2Prob4.Text, CultureInfo.InvariantCulture);
                        Y3 = double.Parse(txtRefHum3Prob4.Text, CultureInfo.InvariantCulture);
                        Y4 = double.Parse(txtRefHum4Prob4.Text, CultureInfo.InvariantCulture);

                        //Calculator4Point(T1, T2, T3, T4, X1, X2, X3, X4, Y1, Y2, Y3, Y4);
                        X = new List<double>() { X1, X2, X3, X4 };
                        Y = new List<double>() { Y1, Y2, Y3, Y4 };
                        T = new List<double>() { T1, T2, T3, T4 };
                        TimPhuongTrinh(X, Y, T);
                        break;
                }

                success = WriteDataCalib(selectedChannel);
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

                    MessageBox.Show("Calib humidity successful!");
                    return;
                }
                else
                {
                    MessageBox.Show("Calib Fail! Try again");
                    return;
                }
            }
            else if (channels[selectedChannel].sensor == Sensor.Temp_sensor)
            {
                //Calib Temp
                if (String.IsNullOrEmpty(textBox58.Text.Trim()))
                {
                    MessageBox.Show("Offset can not be empty");
                    textBox58.Focus();
                    return;
                }

                double data = double.Parse(textBox58.Text, CultureInfo.InvariantCulture);
                byte bufDt = (byte)(Math.Abs(data) * 10);
                byte dau = 0;
                if (textBox58.Text.Contains("-"))
                {
                    dau = 1;
                }

                if (bufDt >= 255)
                {
                    MessageBox.Show("Data offset cannot over 25.5");
                    textBox58.Text = "";
                    return;
                }

                try
                {
                    dv35.Open(host);
                    //HIDFunction.hid_SetNonBlocking(dv35.dev, 1);
                    Thread.Sleep(500);
                    if (!dv35.writeCalibOffset((byte)(selectedChannel + 1), bufDt, dau))
                    {
                        MessageBox.Show("Setting Calib fail");
                        return;
                    }
                    else
                    {
                        MessageBox.Show("Setting Calib successful");
                    }

                }
                catch
                {
                    MessageBox.Show("Setting Calib successful");
                }
                finally { dv35.Close(); }

            }
            btnCalProb1.Enabled = false;
            btnCalProb1.Refresh();
            Thread.Sleep(2000);
            btnCalProb1.Enabled = true;
        }
        /// <summary>
        /// Read Data Calib of 4 channel
        /// </summary>
        /// <returns></returns>
        byte[] ReadDataCalib(byte channel)
        {
            byte[] buf = new byte[64];
            buf[0] = 0x01;
            buf[1] = 0x92;
            buf[2] = 0x01;//Read or Write
            buf[3] = channel;//Channel
            dv35.Write(buf);
            dv35.Read(ref buf);
            byte[] dataCalibs = new byte[8];
            for (int i = 0; i < 8; i++)
            {
                dataCalibs[i] = buf[i + 2];
            }
            return dataCalibs;
        }

        private void textBox51_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar) || Char.IsControl(e.KeyChar)) { }

            else if (e.KeyChar == '.' && textBox51.Text.IndexOf('.'.ToString()) == -1) { }

            else if (e.KeyChar == '-' && textBox51.Text == "") { }

            else
            {
                e.Handled = true;
            }

            if (textBox51.Text.IndexOf('.') != -1 && textBox51.Text.IndexOf('.') == textBox51.Text.Length - 2)
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

        private void button1_Click_1(object sender, EventArgs e)
        {
            ReadAndFill();
        }

        private void textBox51_TextChanged(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            textBox.Text = textBox.Text.Replace(',', '.');
        }

        bool WriteDataCalib(byte channel)
        {
            channel = (byte)(channel + 1);
            try
            {
                dv35.Open(host);
                byte[] buf = new byte[64];
                buf[0] = 0x01;
                buf[1] = 0x92;
                buf[2] = 0x00;//Read or Write
                buf[3] = (byte)(channel);//Channel

                if (a < 0)
                {
                    a = a + 65536;
                }

                if (b < 0)
                {
                    b = b + 65536;
                }

                //if (c < 0)
                //{
                //    c = c + 65536;
                //}

                //if (d < 0)
                //{
                //    d = d + 65536;
                //}


                buf[4] = (byte)(a * 1000 / 256);
                buf[5] = (byte)(a * 1000 % 256);
                buf[6] = (byte)(b * 100 / 256);
                buf[7] = (byte)(b * 100 % 256);
                //buf[8] = (byte)(c / 256);
                //buf[9] = (byte)(c % 256);
                //buf[10] = (byte)(d / 256);
                //buf[11] = (byte)(d % 256);
                byte[] dataCalib = new byte[8];
                for (int i = 0; i < 8; i++)
                {
                    dataCalib[i] = buf[i + 4];
                }
                dv35.Write(buf);
                Thread.Sleep(100);
                dv35.Read(ref buf);
                Thread.Sleep(1000);
                if (buf[3] == 0x92 && buf[2] == 0x92)
                {
                    byte[] re = ReadDataCalib((byte)channel);
                    if (re == null)
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
            catch
            {
                return false;
            }
            finally { dv35.Close(); }
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

        private void txtRefHum1Prob3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar) || Char.IsControl(e.KeyChar)) { }

            else if (e.KeyChar == '.' && txtRefHum1Prob3.Text.IndexOf('.'.ToString()) == -1) { }

            else if (e.KeyChar == '-' && txtRefHum1Prob3.Text == "") { }

            else
            {
                e.Handled = true;
            }

            if (txtRefHum1Prob3.Text.IndexOf('.') != -1 && txtRefHum1Prob3.Text.IndexOf('.') == txtRefHum1Prob3.Text.Length - 2)
            {
                if (!Char.IsControl(e.KeyChar))
                    e.Handled = true;
            }
        }

        private void txtRefHum2Prob3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar) || Char.IsControl(e.KeyChar)) { }

            else if (e.KeyChar == '.' && txtRefHum2Prob3.Text.IndexOf('.'.ToString()) == -1) { }

            else if (e.KeyChar == '-' && txtRefHum2Prob3.Text == "") { }

            else
            {
                e.Handled = true;
            }

            if (txtRefHum2Prob3.Text.IndexOf('.') != -1 && txtRefHum2Prob3.Text.IndexOf('.') == txtRefHum2Prob3.Text.Length - 2)
            {
                if (!Char.IsControl(e.KeyChar))
                    e.Handled = true;
            }
        }

        private void txtRefHum3Prob3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar) || Char.IsControl(e.KeyChar)) { }

            else if (e.KeyChar == '.' && txtRefHum3Prob3.Text.IndexOf('.'.ToString()) == -1) { }

            else if (e.KeyChar == '-' && txtRefHum3Prob3.Text == "") { }

            else
            {
                e.Handled = true;
            }

            if (txtRefHum3Prob3.Text.IndexOf('.') != -1 && txtRefHum3Prob3.Text.IndexOf('.') == txtRefHum3Prob3.Text.Length - 2)
            {
                if (!Char.IsControl(e.KeyChar))
                    e.Handled = true;
            }
        }

        private void txtRefHum4Prob3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar) || Char.IsControl(e.KeyChar)) { }

            else if (e.KeyChar == '.' && txtRefHum4Prob3.Text.IndexOf('.'.ToString()) == -1) { }

            else if (e.KeyChar == '-' && txtRefHum4Prob3.Text == "") { }

            else
            {
                e.Handled = true;
            }

            if (txtRefHum4Prob3.Text.IndexOf('.') != -1 && txtRefHum4Prob3.Text.IndexOf('.') == txtRefHum4Prob3.Text.Length - 2)
            {
                if (!Char.IsControl(e.KeyChar))
                    e.Handled = true;
            }
        }

        private void txtRefHum4Prob4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar) || Char.IsControl(e.KeyChar)) { }

            else if (e.KeyChar == '.' && txtRefHum4Prob4.Text.IndexOf('.'.ToString()) == -1) { }

            else if (e.KeyChar == '-' && txtRefHum4Prob4.Text == "") { }

            else
            {
                e.Handled = true;
            }

            if (txtRefHum4Prob4.Text.IndexOf('.') != -1 && txtRefHum4Prob4.Text.IndexOf('.') == txtRefHum4Prob4.Text.Length - 2)
            {
                if (!Char.IsControl(e.KeyChar))
                    e.Handled = true;
            }
        }

        private void txtRefHum3Prob4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar) || Char.IsControl(e.KeyChar)) { }

            else if (e.KeyChar == '.' && txtRefHum3Prob4.Text.IndexOf('.'.ToString()) == -1) { }

            else if (e.KeyChar == '-' && txtRefHum3Prob4.Text == "") { }

            else
            {
                e.Handled = true;
            }

            if (txtRefHum3Prob4.Text.IndexOf('.') != -1 && txtRefHum3Prob4.Text.IndexOf('.') == txtRefHum3Prob4.Text.Length - 2)
            {
                if (!Char.IsControl(e.KeyChar))
                    e.Handled = true;
            }
        }

        private void txtRefHum2Prob4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar) || Char.IsControl(e.KeyChar)) { }

            else if (e.KeyChar == '.' && txtRefHum2Prob4.Text.IndexOf('.'.ToString()) == -1) { }

            else if (e.KeyChar == '-' && txtRefHum2Prob4.Text == "") { }

            else
            {
                e.Handled = true;
            }

            if (txtRefHum2Prob4.Text.IndexOf('.') != -1 && txtRefHum2Prob4.Text.IndexOf('.') == txtRefHum2Prob4.Text.Length - 2)
            {
                if (!Char.IsControl(e.KeyChar))
                    e.Handled = true;
            }
        }

        private void txtRefHum1Prob4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar) || Char.IsControl(e.KeyChar)) { }

            else if (e.KeyChar == '.' && txtRefHum2Prob4.Text.IndexOf('.'.ToString()) == -1) { }

            else if (e.KeyChar == '-' && txtRefHum2Prob4.Text == "") { }

            else
            {
                e.Handled = true;
            }

            if (txtRefHum2Prob4.Text.IndexOf('.') != -1 && txtRefHum2Prob4.Text.IndexOf('.') == txtRefHum2Prob4.Text.Length - 2)
                if (!Char.IsControl(e.KeyChar))
                    e.Handled = true;
        }
    
        
        private void textBox49_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }
    }
}
