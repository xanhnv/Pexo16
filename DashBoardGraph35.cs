using System;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Pexo16
{
    public partial class DashBoardGraph35 : Form
    {
        DashBoardSelect frmSelect;
        public int max1;
        public int min1;
        private Thread addDataRunner;
        public delegate void AddDataDelegate();
        public AddDataDelegate addDataDel;
        private Random rand = new Random();
        int chartArea = 0;

        Device35 device_dashboard = Device35.DelInstance();

        ResourceManager res_man = new ResourceManager("Pexo16.Lang.Resources", typeof(formC).Assembly);
        CultureInfo cul;

        private string host;
        private int[] unit = new int[12];

        public DashBoardGraph35(DashBoardSelect select)
        {
            InitializeComponent();
            frmSelect = select;
        }

        private void DashBoardGraph35_Load(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Maximized;
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

            //chkArea1.Text = res_man.GetString("Show chart area", cul) + " 1";
            //chkArea2.Text = res_man.GetString("Show chart area", cul) + " 2";


            device_dashboard = Device35.Instance;
            for (int i = 0; i < getDeviceInfo.activeDeviceListAl.Count; i++)
            {
                if (frmSelect.selectedLogger == getDeviceInfo.activeDeviceListAl[i].ToString())
                {
                    host = getDeviceInfo.activeDeviceListAl[i].ToString();
                    break;
                }
            }

            bool check = device_dashboard.USBOpen(host);

            if (check)
            {
                if (device_dashboard.readSettingDevice())
                {
                    if (device_dashboard.byteLogging != 68)
                    {
                        MessageBox.Show(res_man.GetString("Please start logging to view data with realtime", cul));
                        ThreadStart addDataThreadStart1 = new ThreadStart(AddDataThreadLoop);
                        addDataRunner = new Thread(addDataThreadStart1);
                        btnStop.Text = res_man.GetString("Stop", cul);
                        device_dashboard.Close();
                        this.Close();
                        return;
                    }
                }
                else
                {
                    //this.Close();
                    return;
                }
            }
            else
            {
                MessageBox.Show(res_man.GetString("Connect to device fail", cul));
                return;
            }

            device_dashboard.Channels = new Channel[4];
            for (int i = 0; i < 4; i++)
            {
                device_dashboard.Channels[i] = new Channel();
            }
            device_dashboard.readSettingChannel();
            int numOfChannel = 0;
            for (int i = 0; i < 4; i++)
            {
                if(device_dashboard.Channels[i].Sensor == 3)
                {
                    numOfChannel += 3;
                }
                else
                {
                    numOfChannel += 1;
                }
            }

            int[] unit = new int[4];
            for (int i = 0; i < 4; i++)
            {
                if (device_dashboard.Channels[i].Sensor == 1)
                {
                    unit[i] = device_dashboard.Channels[i].Unit;
                }
                else
                {
                    unit[i] = device_dashboard.Channels[i].Sensor;
                }
            }
            //device_dashboard = Device35.DelInstance();
            //device_dashboard = Device35.Instance;

            //device_dashboard.Channels = new Channel[numOfChannel];
            ////for (int i = 0; i < numOfChannel; i++)
            ////{
            ////    device_dashboard.Channels[i] = new Channel();
            ////}

            //int count = 0;
            //for (int i = 0; i < 4; i++)
            //{
            //    if(unit[i] == 3)
            //    {
            //        device_dashboard.Channels[count] = new Channel();
            //        device_dashboard.Channels[count].Unit = 3;
            //        device_dashboard.Channels[count + 1] = new Channel();
            //        device_dashboard.Channels[count + 1].Unit = 3;
            //        device_dashboard.Channels[count + 2] = new Channel();
            //        device_dashboard.Channels[count + 2].Unit = 3;

            //        count += 3;
            //    }
            //    else //if(device_dashboard.Channels[i].Sensor == 0)
            //    {
            //        device_dashboard.Channels[count] = new Channel();
            //        //if (unit[i] == 1)
            //            device_dashboard.Channels[count].Unit = byte.Parse(unit[i].ToString());
            //        //else
            //        //{
            //        //    device_dashboard.Channels[i].Unit = device_dashboard.Channels[i].Sensor;
            //        //}
            //        count += 1;
            //    }
            //}


            int count = 0;
            for (int i = 0; i < 4; i++)
            {
                if(device_dashboard.Channels[i].Sensor == 3)
                {
                    count += 3;
                }
                else
                {
                    count += 1;
                }
            }
            device_dashboard.numOfChannel = count;

            for (int i = 0; i < 4; i++)
            {
                if (device_dashboard.Channels[i].Sensor != 1 && device_dashboard.Channels[i].Sensor != 3)
                {
                    device_dashboard.Channels[i].Unit = device_dashboard.Channels[i].Sensor;
                }
            }

            device_dashboard.Close();
            //device_dashboard.USBOpen(host);


            ThreadStart addDataThreadStart = new ThreadStart(AddDataThreadLoop);
            addDataRunner = new Thread(addDataThreadStart);

            // create a delegate for adding Data
            addDataDel += new AddDataDelegate(AddData);
            btnStop.Text = res_man.GetString("Run", cul);

             //device_dashboard.Close();
        }

        private void AddData()
        {
            DateTime timeStamp = DateTime.Now;
            int channel = 0;

            int dem = 0;
            double[] data = new double[12];
            byte[] buf = new byte[64];

            string StrDevs = host;
            try
            {
                device_dashboard.Close();
                device_dashboard.USBOpen(host);
                //if (device_dashboard.USBOpen(StrDevs) == true)
                //{
                    device_dashboard.readSettingDevice();
                    if (device_dashboard.byteLogging == 0xaa)
                    {
                        if (addDataRunner.IsAlive == true)
                        {
                            addDataRunner.Suspend();
                        }
                        this.Close();
                    }
                    //Thread.Sleep(500);
                   
                    //HIDFunction.hid_SetNonBlocking(device_dashboard.dev, 1);
                    device_dashboard.readDataProbe(ref buf);

                    if (buf[1] != 28)
                    {
                        device_dashboard.Close();
                        HIDFunction.hid_Exit();
                        device_dashboard.USBOpen(StrDevs);
                        //MessageBox.Show(res_man.GetString("Read data fail", cul));
                        return;
                    }

                    int count = 0;
                    //for (int i = 0; i < 4; i++)
                    //{
                    //    string hexSensor = buf[2 + i * 7].ToString("X");
                    //    if (hexSensor != "0")
                    //    {
                    //        if (hexSensor.Substring(1, 1) == "3")
                    //        {
                    //            for (int j = 0; j < 3; j++)
                    //            {
                    //                data[count] = mGlobal.get_temp(buf[3 + i * 7 + 2 * j], buf[3 + i * 7 + 2 * j + 1]) / 1000;
                    //                count += 1;
                    //            }
                    //        }
                    //        else if (hexSensor.Substring(1, 1) == "1" || hexSensor.Substring(1, 1) == "2")
                    //        {
                    //            data[count] = mGlobal.get_temp(buf[3 + i * 7], buf[3 + i * 7 + 1]) / 10.0;
                    //            count += 1;
                    //        }
                    //        else if (hexSensor.Substring(1, 1) == "0")
                    //        {
                    //            continue;
                    //        }
                    //        else
                    //        {
                    //            data[count] = mGlobal.get_temp(buf[3 + i * 7], buf[3 + i * 7 + 1]);
                    //            count += 1;
                    //        }
                    //    }

                    //    //for (int j = 0; j < 6; j++)
                    //    //{
                    //    //    if (buf[3 + i * 7 + j] != 0)
                    //    //    {
                    //    //        data[i * j] = buf[3 + i * 7 + j];
                    //    //    }
                    //    //}
                    //}

                    for (int i = 0; i < 4; i++)
                    {
                        string hexSensor = buf[2 + i * 7].ToString("X");
                        if (hexSensor != "0")
                        {
                            if (hexSensor.Substring(1, 1) == "3")
                            {
                                for (int j = 0; j < 3; j++)
                                {
                                    data[i * 3 + j] = mGlobal.get_temp(buf[3 + i * 7 + 2 * j], buf[3 + i * 7 + 2 * j + 1]) / 1000.0;
                                    //data[i * 3 + j] = mGlobal.get_temp(buf[3 + i * 7 + 2 * j], buf[3 + i * 7 + 2 * j + 1]) / 1000.0;
                                    count += 1;
                                }
                            }
                            else if (hexSensor.Substring(1, 1) == "1" || hexSensor.Substring(1, 1) == "2")
                            {
                                data[i * 3] = mGlobal.get_temp(buf[3 + i * 7], buf[3 + i * 7 + 1]) / 10.0;
                                count += 1;
                            }
                            else if (hexSensor.Substring(1, 1) == "0")
                            {
                                continue;
                            }
                            else
                            {
                                data[i * 3] = mGlobal.get_temp(buf[3 + i * 7], buf[3 + i * 7 + 1]);
                                count += 1;
                            }
                        }

                        //for (int j = 0; j < 6; j++)
                        //{
                        //    if (buf[3 + i * 7 + j] != 0)
                        //    {
                        //        data[i * j] = buf[3 + i * 7 + j];
                        //    }
                        //}
                    }
                    //device_dashboard.Close();
                //}
            }
            catch 
            {
                MessageBox.Show(device_dashboard.dev.ToString());
                device_dashboard.Close();
            }
            //device_dashboard.Close();


            int c = 0;

            foreach (Series ptSeries in chart1.Series)
            {
                //channel += 1;
                string num = "";
                num = ptSeries.Name.ToString().Substring(8, 1);
                string name = "";
                name = ptSeries.Name.ToString().Substring(10, 1);
              
                if(name == "(" || name == "X")
                {
                    dem = (Int32.Parse(num) - 1) * 3;
                    AddNewPoint(timeStamp, ptSeries, dem, data, c);
                    c = c + 1;
                }
                else if(name == "Y")
                {
                    dem = (Int32.Parse(num) - 1) * 3 + 1;
                    AddNewPoint(timeStamp, ptSeries, dem, data, c);
                    c = c + 1;
                }
                else
                {
                    dem = (Int32.Parse(num) - 1) * 3 + 2;
                    AddNewPoint(timeStamp, ptSeries, dem, data, c);
                    c = c + 1;
                }

                //channel = Int32.Parse(name.Substring(8, 1));

                //AddNewPoint(timeStamp, ptSeries, channel);
                

                //ptSeries.Points.AddXY(timeStamp.ToOADate(), );
                //dem += 1;
                //AddNewPoint(timeStamp, ptSeries, dem, data);

            }
        }

        private void AddNewPoint(DateTime timeStamp, Series ptSeries, int channel, double[] data, int dem)
        {
            //string StrDevs = host;
            //if (device_dashboard.USBOpen(StrDevs) == true)
            //{
            //    double[] data = new double[12];
            //    byte[] buf = new byte[64];


            //    device_dashboard.readDataProbe(ref buf);


            //    int count = 0;
            //    for (int i = 0; i < 4; i++)
            //    {
            //        string hexSensor = buf[2 + i * 7].ToString("X");

            //        if(hexSensor.Substring(1,1) == "3")
            //        {
            //            for (int j = 0; j < 3; j++)
            //            {
            //                data[count] = mGlobal.get_temp(buf[3 + i * 7 + 2 * j] , buf[3 + i * 7 + 2 * j + 1])/1000;
            //                count += 1;
            //            }
            //        }
            //        else if(hexSensor.Substring(1,1) == "1" || hexSensor.Substring(1,1) == "2")
            //        {
            //            data[count] = (buf[3 + i * 7] * 256 + buf[3 + i * 7 + 1]) / 10.0;
            //            count += 1;
            //        }
            //        else if (hexSensor.Substring(1, 1) == "0")
            //        {
            //            continue;
            //        }
            //        else
            //        {
            //                data[count] = buf[3 + i * 7] * 256 + buf[3 + i * 7 + 1];
            //                count += 1;
            //        }

            //        //for (int j = 0; j < 6; j++)
            //        //{
            //        //    if (buf[3 + i * 7 + j] != 0)
            //        //    {
            //        //        data[i * j] = buf[3 + i * 7 + j];
            //        //    }
            //        //}
            //    }
              
                //for (int i = 0; i < 4; i++)
                //{
                //    string hexTamp =  buf[2 + i*7].ToString("X");
                //    int unit = Int32.Parse(hexTamp.Substring(1,1));
                //    if(unit == 3)
                //    {
                //        if (device_dashboard.Channels[i].Unit == 3)
                //        {
                //            newSer[count] = new Series("Channel " + (i + 1) + " X (" + mGlobal.IntToUnit35(byte.Parse(unit.ToString())) + ")");
                //            double dataX = buf[3 + i*7] * 256 + buf[4 + i *7];
                //            if(dataX > 32768)
                //            {
                //                dataX = dataX - 65536;
                //            }
                //            newSer[count].Points.AddXY(timeStamp.ToOADate(), dataX);
                //            count += 1;
                //            newSer[count] = new Series("Channel " + (i + 1) + " Y (" + mGlobal.IntToUnit35(byte.Parse(unit.ToString())) + ")");
                //            double dataY = buf[5 + i * 7] * 256 + buf[6 + i * 7];
                //            if (dataY > 32768)
                //            {
                //                dataY = dataY - 65536;
                //            }
                //            newSer[count].Points.AddXY(timeStamp.ToOADate(), dataY);
                //            count += 1;
                //            newSer[count] = new Series("Channel " + (i + 1) + " Z (" + mGlobal.IntToUnit35(byte.Parse(unit.ToString())) + ")");
                //            double dataZ = buf[3 + i * 7] * 256 + buf[4 + i * 7];
                //            if (dataZ > 32768)
                //            {
                //                dataZ = dataZ - 65536;
                //            }
                //            newSer[count].Points.AddXY(timeStamp.ToOADate(), dataZ);
                //            count += 1;
                //        }
                //        else
                //        {
                //            newSer[count] = new Series("Channel " + (i + 1) + " (" + mGlobal.IntToUnit35(byte.Parse(unit.ToString())) + ")");
                //        }
                //    }
                //}

                // Add new Data point to its series.
                switch (channel)
                {
                    case 0:
                        ptSeries.Points.AddXY(timeStamp.ToOADate(), data[0]);
                        chart1.Series[dem].LegendText = chart1.Series[dem].Name.Substring(0, chart1.Series[dem].Name.IndexOf(")") + 1);
                        chart1.Series[dem].LegendText += "  " + data[0].ToString("0.0##");
                        break;
                    case 1:// set value to Channel 2
                        ptSeries.Points.AddXY(timeStamp.ToOADate(), data[1]);
                        chart1.Series[dem].LegendText = chart1.Series[dem].Name.Substring(0, chart1.Series[dem].Name.IndexOf(")") + 1);
                        chart1.Series[dem].LegendText += "  " + data[1].ToString("0.0##");
                        break;
                    case 2:
                        ptSeries.Points.AddXY(timeStamp.ToOADate(), data[2]);
                        chart1.Series[dem].LegendText = chart1.Series[dem].Name.Substring(0, chart1.Series[dem].Name.IndexOf(")") + 1);
                        chart1.Series[dem].LegendText += "  " + data[2].ToString("0.0##");
                        break;
                    case 3:
                        ptSeries.Points.AddXY(timeStamp.ToOADate(), data[3]);
                        chart1.Series[dem].LegendText = chart1.Series[dem].Name.Substring(0, chart1.Series[dem].Name.IndexOf(")") + 1);
                        chart1.Series[dem].LegendText += "  " + data[3].ToString("0.0##");
                        break;
                    case 4:
                        ptSeries.Points.AddXY(timeStamp.ToOADate(), data[4]);
                        chart1.Series[dem].LegendText = chart1.Series[dem].Name.Substring(0, chart1.Series[dem].Name.IndexOf(")") + 1);
                        chart1.Series[dem].LegendText += "  " + data[4].ToString("0.0##");
                        break;
                    case 5:
                        ptSeries.Points.AddXY(timeStamp.ToOADate(), data[5]);
                        chart1.Series[dem].LegendText = chart1.Series[dem].Name.Substring(0, chart1.Series[dem].Name.IndexOf(")") + 1);
                        chart1.Series[dem].LegendText += "  " + data[5].ToString("0.0##");
                        break;
                    case 6:
                        ptSeries.Points.AddXY(timeStamp.ToOADate(), data[6]);
                        chart1.Series[dem].LegendText = chart1.Series[dem].Name.Substring(0, chart1.Series[dem].Name.IndexOf(")") + 1);
                        chart1.Series[dem].LegendText += "  " + data[6].ToString("0.0##");
                        break;
                    case 7:
                        ptSeries.Points.AddXY(timeStamp.ToOADate(), data[7]);
                        chart1.Series[dem].LegendText = chart1.Series[dem].Name.Substring(0, chart1.Series[dem].Name.IndexOf(")") + 1);
                        chart1.Series[dem].LegendText += "  " + data[7].ToString("0.0##");
                        break;
                    case 8:
                        ptSeries.Points.AddXY(timeStamp.ToOADate(), data[8]);
                        chart1.Series[dem].LegendText = chart1.Series[dem].Name.Substring(0, chart1.Series[dem].Name.IndexOf(")") + 1);
                        chart1.Series[dem].LegendText += "  " + data[8].ToString("0.0##");
                        break;
                    case 9:
                        ptSeries.Points.AddXY(timeStamp.ToOADate(), data[9]);
                        chart1.Series[dem].LegendText = chart1.Series[dem].Name.Substring(0, chart1.Series[dem].Name.IndexOf(")") + 1);
                        chart1.Series[dem].LegendText += "  " + data[9].ToString("0.0##");
                        break;
                    case 10:
                        ptSeries.Points.AddXY(timeStamp.ToOADate(), data[10]);
                        chart1.Series[dem].LegendText = chart1.Series[dem].Name.Substring(0, chart1.Series[dem].Name.IndexOf(")") + 1);
                        chart1.Series[dem].LegendText += "  " + data[10].ToString("0.0##");
                        break;
                    case 11:
                        ptSeries.Points.AddXY(timeStamp.ToOADate(), data[11]);
                        chart1.Series[dem].LegendText = chart1.Series[dem].Name.Substring(0, chart1.Series[dem].Name.IndexOf(")") + 1);
                        chart1.Series[dem].LegendText += "  " + data[11].ToString("0.0##");
                        break;

                    default:
                        break;
                }

                
                // remove all points from the source series older than 1.5 minutes.
                double removeBefore = timeStamp.AddSeconds((double)(90) * (-1)).ToOADate();

                //remove oldest values to maintain a constant number of Data points
                while (ptSeries.Points[0].XValue < removeBefore)
                {
                    ptSeries.Points.RemoveAt(0);
                }

                //chart1.ChartAreas[0].AxisX.Minimum = ptSeries.Points[0].XValue;
                //chart1.ChartAreas[0].AxisX.Maximum = DateTime.FromOADate(ptSeries.Points[0].XValue).AddMinutes(2).ToOADate();
                //chart1.ChartAreas[0].RecalculateAxesScale();
                for (int i = 0; i < chart1.ChartAreas.Count; i++)
                {
                    chart1.ChartAreas[i].AxisX.Minimum = ptSeries.Points[0].XValue;
                    chart1.ChartAreas[i].AxisX.Maximum = DateTime.FromOADate(ptSeries.Points[0].XValue).AddMinutes(2).ToOADate();
                }

                chart1.Invalidate();
            //}
            //device_dashboard.Close();
        }

        private void AddDataThreadLoop()
        {
            while (true)
            {
                chart1.Invoke(addDataDel);

                Thread.Sleep(900);
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (btnStop.Text == res_man.GetString("Run", cul))
            {
                //ThreadStart addDataThreadStart = new ThreadStart(AddDataThreadLoop);
                //addDataRunner = new Thread(addDataThreadStart);

                //// create a delegate for adding Data
                //addDataDel += new AddDataDelegate(AddData);

                //startTrending.Enabled = false;
                //// and only Enable the Stop button
                //stopTrending.Enabled = true;

                // Predefine the viewing chart1.ChartAreas[0] of the Chartrun

                int c1 = 0;
                int numVib = 0;
                for (int i = 0; i < 4; i++)
                {
                    if (device_dashboard.Channels[i].Sensor == 3)
                        numVib += 1;
                    else if(device_dashboard.Channels[i].Sensor != 0)
                    {
                        c1 = 1;
                    }
                }

                DateTime minValue = DateTime.Now;
                DateTime maxValue = minValue.AddSeconds(240);

                chart1.ChartAreas.Clear();
                if (c1 != 0)
                {
                    ChartArea first = new ChartArea();

                    first.AxisY2.Enabled = AxisEnabled.False;
                    first.AxisY2.LabelStyle.Enabled = false;
                    first.AxisX.Minimum = minValue.ToLocalTime().ToOADate();
                    first.AxisX.Maximum = maxValue.ToLocalTime().ToOADate();
                    first.AxisY2.Title = "CO2.(PPM)";
                    first.AxisY.TitleFont = new Font("Arial", 9.0F, FontStyle.Bold);
                    first.AxisY2.TitleFont = new Font("Arial", 9.0F, FontStyle.Bold);
                    //chart1.Titles[0].Text = frmSelect.cbbSelected.Substring(0, frmSelect.cbbSelected.Length - 17);
                    chart1.Titles[0].Text = "Marathon Electronic Data Logger";
                    first.AxisY.LineWidth = 2;
                    first.AxisY2.LineWidth = 2;
                    first.AxisY.LineColor = Color.SeaGreen;
                    first.AxisY2.LineColor = Color.SeaGreen;
                    first.BorderDashStyle = ChartDashStyle.Dash;
                    first.AxisY.MinorGrid.Enabled = false;
                    first.AxisY2.MinorGrid.Enabled = false;
                    first.AxisY2.MajorGrid.Enabled = false;
                    first.AxisY2.IntervalAutoMode = IntervalAutoMode.FixedCount;

                    first.AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dot;
                    first.AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dot;
                    first.AxisY2.MajorGrid.LineDashStyle = ChartDashStyle.Dot;
                    first.BorderDashStyle = ChartDashStyle.Solid;
                    first.AxisX.LabelStyle.Format = "HH:mm:ss";
                    //first.AxisY2.Minimum = 500;
                    first.AxisY2.IntervalAutoMode = IntervalAutoMode.FixedCount;
                    first.AxisY2.Interval = first.AxisY.Interval;
                    first.AxisY2.IsLabelAutoFit = true;
                    first.AxisY2.LabelAutoFitStyle = LabelAutoFitStyles.StaggeredLabels;

                    first.CursorX.IsUserEnabled = true;
                    first.CursorX.IsUserSelectionEnabled = true;
                    first.AxisX.ScaleView.Zoomable = true;
                    first.AxisX.ScrollBar.IsPositionedInside = true;

                    first.CursorY.IsUserEnabled = true;
                    first.CursorY.IsUserSelectionEnabled = true;
                    first.AxisY.ScaleView.Zoomable = true;
                    first.AxisY.ScrollBar.IsPositionedInside = true;

                   
                    first.AxisY2.ScaleView.Zoomable = true;
                    first.AxisY2.ScrollBar.IsPositionedInside = true;
                    first.AxisY2.ScrollBar.Enabled = true;

                    first.BackColor = Color.Ivory;

                    first.AxisY2.Enabled = AxisEnabled.False;
                    first.AxisY2.LabelStyle.Enabled = false;
                    first.AxisY.LabelStyle.Format = "00.0";
                    first.AlignmentOrientation = AreaAlignmentOrientations.Vertical;
                    first.AlignmentStyle = AreaAlignmentStyles.PlotPosition;
                    first.Position.Auto = true;
                    first.InnerPlotPosition.Auto = true;

                    chart1.ChartAreas.Add(first);

                }

                for (int i = 1; i <= numVib; i++)
                {
                    ChartArea a = new ChartArea();
                    a.AxisY2.Enabled = AxisEnabled.False;
                    a.AxisY2.LabelStyle.Enabled = false;

                    a.AxisX.Minimum = minValue.ToLocalTime().ToOADate();
                    a.AxisX.Maximum = maxValue.ToLocalTime().ToOADate();
                    a.AxisY.Title = "Acceleration (G)" + i.ToString();
                    a.AxisY.TitleFont = new Font("Arial", 9.0F, FontStyle.Bold);
                    a.AxisY.LineWidth = 2;
                    a.AxisY.LineColor = Color.SeaGreen;
                    a.BorderDashStyle = ChartDashStyle.Dash;
                    a.AxisY.MinorGrid.Enabled = false;
                    a.AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dot;
                    a.AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dot;
                    a.BorderDashStyle = ChartDashStyle.Solid;
                    a.AxisX.LabelStyle.Format = "HH:mm:ss";

                    a.AxisY.ScaleView.SmallScrollMinSizeType = DateTimeIntervalType.Milliseconds;
                    a.AxisY.LabelStyle.Format = "0.##";

                    a.CursorX.IsUserEnabled = true;
                    a.CursorX.IsUserSelectionEnabled = true;
                    a.AxisX.ScaleView.Zoomable = true;
                    a.AxisX.ScrollBar.IsPositionedInside = true;

                    a.CursorY.IsUserEnabled = true;
                    a.CursorY.IsUserSelectionEnabled = true;
                    a.AxisY.ScaleView.Zoomable = true;
                    a.AxisY.ScrollBar.IsPositionedInside = true;
                    a.CursorY.Interval = 0.05;

                    a.BackColor = Color.Ivory;
                    a.AxisY.LabelStyle.Format = "0.00";

                    a.AlignmentOrientation = AreaAlignmentOrientations.Vertical;
                    a.AlignmentStyle = AreaAlignmentStyles.PlotPosition;

                    chart1.ChartAreas.Add(a);
                }

                chart1.Legends[0].BorderColor = Color.Aquamarine;
                chart1.Legends[0].BorderWidth = 2;
                chart1.Legends[0].BorderDashStyle = ChartDashStyle.Solid;
                chart1.Legends[0].ShadowOffset = 2;

                CheckBox[] chkArea = new CheckBox[chart1.ChartAreas.Count];
                chartArea = chart1.ChartAreas.Count;
                int tmpH = 0;
                chart1.Controls.Clear();
                for (int i = 0; i < chartArea; i++)
                {
                    tmpH += 50;
                    chkArea[i] = new CheckBox();
                    chkArea[i].Name = "Check " + i + ":";
                    chkArea[i].Text = "Show Area " + (i + 1).ToString();
                    chkArea[i].Font = new Font("Arial", 10.0F, FontStyle.Italic);
                    chkArea[i].Left = Int32.Parse((chart1.Width - 180).ToString());
                    chkArea[i].Top = 300 + tmpH;
                    chkArea[i].Width = 150;
                    chkArea[i].Visible = true;
                    chkArea[i].Checked = true;
                    chart1.Controls.Add(chkArea[i]);
                    chkArea[i].CheckedChanged += new EventHandler(chkArea_CheckedChanged);
                }

                chart1.AntiAliasing = AntiAliasingStyles.All;

                chart1.Series.Clear();

                Series[] series = new Series[device_dashboard.numOfChannel];

                int count = 0;
                int countVIB = 0;
                for (int i = 0; i < 4; i++)
                {
                    if (device_dashboard.Channels[i].Sensor != 0)
                    {
                        if (device_dashboard.Channels[i].Sensor == 3)
                        {
                            string hexUnitVib = device_dashboard.Channels[i].Unit.ToString("X");
                            int limit = 0;
                            int tmp = Int32.Parse(hexUnitVib.Substring(0, 1));
                            switch (tmp)
                            {
                                case 0:
                                    limit = 2;
                                    break;
                                case 1:
                                    limit = 4;
                                    break;
                                case 2:
                                    limit = 8;
                                    break;
                                case 3:
                                    limit = 16;
                                    break;
                            }

                            chart1.ChartAreas[countVIB + c1].AxisY.Minimum = -(limit + 0.5);
                            chart1.ChartAreas[countVIB + c1].AxisY.Maximum = limit + 0.5;

                            series[count] = new Series(string.Format("{0} {1} X ({2})", res_man.GetString("Channel", cul), (i + 1), mGlobal.IntToUnit35(device_dashboard.Channels[i].Sensor)));
                            series[count].ChartType = SeriesChartType.Line;
                            series[count].BorderWidth = 2;
                            series[count].Color = device_dashboard.Channels[i].LineColor;
                            series[count].XValueType = ChartValueType.Time;
                            series[count].ChartArea = chart1.ChartAreas[countVIB + c1].Name;
                            chart1.Series.Add(series[count]);
                            chart1.Series[count].ToolTip = string.Format("{0} {1}\r\n #VALX \r\n [#VALY]", res_man.GetString("Channel:", cul), (i + 1));
                            chart1.Series[count].LabelToolTip = string.Format("{0} {1}\r\n #VALX \r\n [#VALY]", res_man.GetString("Channel:", cul), (i + 1));
                            count += 1;

                            series[count] = new Series(string.Format("{0} {1} Y ({2})", res_man.GetString("Channel", cul), (i + 1), mGlobal.IntToUnit35(device_dashboard.Channels[i].Sensor)));
                            series[count].ChartType = SeriesChartType.Line;
                            series[count].BorderWidth = 2;
                            series[count].Color = device_dashboard.Channels[i].LineColor;
                            series[count].XValueType = ChartValueType.Time;
                            series[count].ChartArea = chart1.ChartAreas[countVIB + c1].Name;
                            chart1.Series.Add(series[count]);
                            chart1.Series[count].ToolTip = string.Format("{0} {1}\r\n #VALX \r\n [#VALY]", res_man.GetString("Channel:", cul), (i + 1));
                            chart1.Series[count].LabelToolTip = string.Format("{0} {1}\r\n #VALX \r\n [#VALY]", res_man.GetString("Channel:", cul), (i + 1));
                            count += 1;

                            series[count] = new Series(string.Format("{0} {1} Z ({2})", res_man.GetString("Channel", cul), (i + 1), mGlobal.IntToUnit35(device_dashboard.Channels[i].Sensor)));
                            series[count].ChartType = SeriesChartType.Line;
                            series[count].BorderWidth = 2;
                            series[count].Color = device_dashboard.Channels[i].LineColor;
                            series[count].XValueType = ChartValueType.Time;
                            series[count].ChartArea = chart1.ChartAreas[countVIB + c1].Name;
                            chart1.Series.Add(series[count]);
                            chart1.Series[count].ToolTip = string.Format("{0} {1}\r\n #VALX \r\n [#VALY]", res_man.GetString("Channel:", cul), (i + 1));
                            chart1.Series[count].LabelToolTip = string.Format("{0} {1}\r\n #VALX \r\n [#VALY]", res_man.GetString("Channel:", cul), (i + 1));
                            count += 1;
                            countVIB += 1;
                        }
                        else
                        {
                            series[count] = new Series(string.Format("{0} {1} ({2})", res_man.GetString("Channel", cul), (i + 1), mGlobal.IntToUnit35(device_dashboard.Channels[i].Unit)));
                            string title = mGlobal.IntToUnit35_RealTime(device_dashboard.Channels[i].Unit);
                            if (!chart1.ChartAreas[0].AxisY.Title.Contains(title))
                            {
                                if (chart1.ChartAreas[0].AxisY.Title != "")
                                    chart1.ChartAreas[0].AxisY.Title += " _ ";
                                chart1.ChartAreas[0].AxisY.Title += title;
                            }
                            series[count].ChartType = SeriesChartType.Line;
                            series[count].BorderWidth = 2;
                            series[count].Color = device_dashboard.Channels[i].LineColor;
                            series[count].XValueType = ChartValueType.Time;
                            series[count].ChartArea = chart1.ChartAreas[0].Name;
                            if (device_dashboard.Channels[i].Unit == 4)
                            {
                                series[count].YAxisType = AxisType.Secondary;
                                chart1.ChartAreas[0].AxisY2.Enabled = AxisEnabled.True;
                                chart1.ChartAreas[0].AxisY2.LabelStyle.Enabled = true;
                                chart1.ChartAreas[0].AxisY2.Minimum = 500;
                            }
                            chart1.Series.Add(series[count]);
                            chart1.Series[count].ToolTip = string.Format("{0} {1}\r\n #VALX \r\n [#VALY{{##.##}}]", res_man.GetString("Channel:", cul), (i + 1));
                            count += 1;
                        }
                    }
                }

                int demArea2 = 0;
                int demArea1 = 0;
                for (int j = 0; j < 4; j++)
                {
                    if (device_dashboard.Channels[j].Sensor == 3)
                    {
                        demArea2 += 1;
                    }
                    else if (device_dashboard.Channels[j].Unit == 175 || device_dashboard.Channels[j].Unit == 172 || device_dashboard.Channels[j].Sensor == 2 || device_dashboard.Channels[j].Sensor == 4)
                    {
                        demArea1 += 1;
                    }
                }


                device_dashboard.USBOpen(host);
                if (addDataRunner.IsAlive == true)
                {
                    addDataRunner.Resume();
                }
                else
                {
                    addDataRunner.Start();
                }
                
                btnStop.Text = res_man.GetString("Stop", cul);
            }
            else
            {
                
                if (addDataRunner.IsAlive == true)
                {
                    addDataRunner.Suspend();
                }
                device_dashboard.Close();
                // Enable all controls on the form
                //startTrending.Enabled = true;
                btnStop.Text = res_man.GetString("Run", cul);
                // and only Disable the Stop button
                //stopTrending.Enabled = false;
            }
        }

        private void chkArea_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chkA = (CheckBox)sender;
            if (chkA.Checked == true)
            {
                chart1.ChartAreas[Int32.Parse(chkA.Name.Substring(6, 1))].Visible = true;
                
            }
            else
            {
                chart1.ChartAreas[Int32.Parse(chkA.Name.Substring(6, 1))].Visible = false;
            }

        }

        //private void chkArea1_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (chkArea1.Checked == true)
        //    {
        //        chart1.ChartAreas[0].Visible = true;
                
        //    }
        //    else
        //    {
        //        chart1.ChartAreas[0].Visible = false;
        //    }
        //}

        //private void chkArea2_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (chkArea2.Checked == true)
        //    {
        //        chart1.ChartAreas[1].Visible = true;
        //    }
        //    else
        //    {
        //        chart1.ChartAreas[1].Visible = false;
        //    }
        //}

        private void DashBoardGraph35_Resize(object sender, EventArgs e)
        {
            chart1.Height = this.Height - 100;
            chart1.Width = this.Width - 10;
            chart1.Left = 10;
            //chkArea1.Left = this.Width - 150;
            //chkArea2.Left = this.Width - 150;

            //chkArea1.Top = this.Height - 150;
            //chkArea2.Top = this.Height - 100;
        }

        private void DashBoardGraph35_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (addDataRunner.IsAlive == true)
            {
                addDataRunner.Suspend();
            }
            device_dashboard.Close();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
