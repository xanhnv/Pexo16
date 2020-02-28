using Microsoft.Win32;
using System;
using System.Drawing;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Pexo16
{
    public partial class DashBoardGraph : Form
    {
        DashBoardSelect frmSelect;
        public int max1;
        public int min1;
        private Thread addDataRunner;
        public delegate void AddDataDelegate();
        public AddDataDelegate addDataDel;
        private Random rand = new Random();

        Device device_dashboard = Device.Instance;

        private string host;
        private int[] unit = new int[8];


        public DashBoardGraph(DashBoardSelect select)
        {
            InitializeComponent();
            frmSelect = select;
        }

        private void SystemEvent_TimeChanged(object sender, EventArgs e)
        {
            CultureInfo.CurrentCulture.ClearCachedData();
        }

        private void frmDashBoardGraph_Load(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Maximized;
            SystemEvents.TimeChanged += new EventHandler(SystemEvent_TimeChanged);

            for (int i = 0; i < getDeviceInfo.activeDeviceListAl.Count; i++)
            {
                if (frmSelect.selectedLogger == getDeviceInfo.activeDeviceListAl[i].ToString())
                {
                    host = getDeviceInfo.activeDeviceListAl[i].ToString();
                    break;
                }
            }

            bool check = device_dashboard.USBOpen(host);
            device_dashboard.channels = new Channel[8];
            for (int i = 0; i < 8; i++)
            {
                device_dashboard.channels[i] = new Channel();
            }
            device_dashboard.Read_setting();

            for (int i = 0; i < device_dashboard.numOfChannel; i++)
            {
                unit[i] = device_dashboard.channels[i].Unit; //Channel 1
            }

            device_dashboard.Close();
   
            ThreadStart addDataThreadStart = new ThreadStart(AddDataThreadLoop);
            addDataRunner = new Thread(addDataThreadStart);

            // create a delegate for adding Data
            addDataDel += new AddDataDelegate(AddData);
            btnStop.Text = "Run";
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            SystemEvents.TimeChanged += new EventHandler(SystemEvent_TimeChanged);

            if(btnStop.Text == "Run")
            {
                //startTrending.Enabled = false;
                //// and only Enable the Stop button
                //stopTrending.Enabled = true;

                // Predefine the viewing chart1.ChartAreas[0] of the Chart
                DateTime minValue = DateTime.Now;
                DateTime maxValue = minValue.AddSeconds(240);

                chart1.ChartAreas[0].AxisX.Minimum = minValue.ToLocalTime().ToOADate();
                chart1.ChartAreas[0].AxisX.Maximum = maxValue.ToLocalTime().ToOADate();
                //chart1.ChartAreas[0].AxisY.Minimum = Int32.Parse(device_dashboard.Channels[0].ave_frm_suminfo.ToString()) - 2;
                //chart1.ChartAreas[0].AxisY.Minimum = 25;
                //chart1.ChartAreas[0].AxisY.Maximum = 50;
                //chart1.ChartAreas[0].AxisY2.Minimum = 1000;
                //chart1.ChartAreas[0].AxisY2.Maximum = 1500;
                chart1.ChartAreas[0].AxisY.Title = "Temp.(C) _ Temp(F) _ Humid.(%RH)";
                chart1.ChartAreas[0].AxisY2.Title = "CO2.(PPM)";
                chart1.ChartAreas[0].AxisY.TitleFont = new Font("Arial", 9.0F, FontStyle.Bold);
                chart1.ChartAreas[0].AxisY2.TitleFont = new Font("Arial", 9.0F, FontStyle.Bold);
                chart1.Titles[0].Text = frmSelect.cbbSelected;
                chart1.ChartAreas[0].AxisY.LineWidth = 2;
                chart1.ChartAreas[0].AxisY2.LineWidth = 2;
                chart1.ChartAreas[0].AxisY.LineColor = Color.SeaGreen;
                chart1.ChartAreas[0].AxisY2.LineColor = Color.SeaGreen;
                chart1.ChartAreas[0].BorderDashStyle = ChartDashStyle.Dash;
                chart1.ChartAreas[0].AxisY.MinorGrid.Enabled = false;
                chart1.ChartAreas[0].AxisY2.MinorGrid.Enabled = false;
                chart1.ChartAreas[0].AxisY2.MajorGrid.Enabled = false;
                chart1.ChartAreas[0].AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dot;
                chart1.ChartAreas[0].AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dot;
                chart1.ChartAreas[0].AxisY2.MajorGrid.LineDashStyle = ChartDashStyle.Dot;
                chart1.ChartAreas[0].BorderDashStyle = ChartDashStyle.Solid;
                chart1.ChartAreas[0].AxisX.LabelStyle.Format = "HH:mm:ss";

                chart1.ChartAreas[0].CursorX.IsUserEnabled = true;
                chart1.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
                chart1.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
                chart1.ChartAreas[0].AxisX.ScrollBar.IsPositionedInside = true;

                chart1.ChartAreas[0].CursorY.IsUserEnabled = true;
                chart1.ChartAreas[0].CursorY.IsUserSelectionEnabled = true;
                chart1.ChartAreas[0].AxisY.ScaleView.Zoomable = true;
                chart1.ChartAreas[0].AxisY.ScrollBar.IsPositionedInside = true;
                //chart1.ChartAreas[0].AxisY.ScrollBar.BackColor = Color.Khaki;

                chart1.ChartAreas[0].AxisY2.ScaleView.ZoomReset(0);
                chart1.ChartAreas[0].AxisY2.ScaleView.Zoomable = true;
                chart1.ChartAreas[0].AxisY2.ScrollBar.IsPositionedInside = true;
                chart1.ChartAreas[0].AxisY2.ScrollBar.Enabled = true;

                // Reset number of series in the Chart.

                chart1.Legends[0].BorderColor = Color.Aquamarine;
                chart1.Legends[0].BorderWidth = 2;
                chart1.Legends[0].BorderDashStyle = ChartDashStyle.Solid;
                chart1.Legends[0].ShadowOffset = 2;


                chart1.Series.Clear();

                Series[] series = new Series[device_dashboard.numOfChannel];

                for (int i = 0; i < device_dashboard.numOfChannel; i++)
                {
                    series[i] = new Series("Channel " + (i + 1) + " ("+ mGlobal.IntToUnit(byte.Parse(unit[i].ToString())) + ")");
                    series[i].ChartType = SeriesChartType.Line;
                    series[i].BorderWidth = 2;
                    series[i].Color = device_dashboard.channels[i].LineColor;
                    series[i].XValueType = ChartValueType.Time;
                    if (device_dashboard.channels[i].Unit != 0)
                    {
                        chart1.Series.Add(series[i]);
                    }
                    if (device_dashboard.channels[i].Unit == 3)
                    {
                        //chart1.ChartAreas[0].AxisY2.Minimum = 1000;
                        //chart1.ChartAreas[0].AxisY2.Maximum = 2000;
                        series[i].YAxisType = AxisType.Secondary;
                    }
                    series[i].ToolTip = "Channel: " + (i + 1) + "\r\n" + " #VALX \r\n [#VALY]";
                    series[i].LabelToolTip = "Channel: " + (i + 1) + "\r\n" + " #VALX \r\n [#VALY]";
                }

                if (addDataRunner.IsAlive == true)
                {
                    addDataRunner.Resume();
                }
                else
                {
                    addDataRunner.Start();
                }
                btnStop.Text = "Stop";
            }
            else
            {
                if (addDataRunner.IsAlive == true)
                {
                    addDataRunner.Suspend();
                }

                // Enable all controls on the form
                //startTrending.Enabled = true;
                btnStop.Text = "Run";
                // and only Disable the Stop button
                //stopTrending.Enabled = false;
            }

        }

        private void AddDataThreadLoop()
        {
            while (true)
            {
                chart1.Invoke(addDataDel);

                Thread.Sleep(1000);
            }
        }

        public void AddData()
        {
            DateTime timeStamp = DateTime.Now;
            int channel = 0;

            foreach (Series ptSeries in chart1.Series)
            {
                //channel += 1;
                channel = Int32.Parse(ptSeries.Name.Substring(8, 1));
                AddNewPoint(timeStamp, ptSeries, channel);
            }
        }

        public void AddNewPoint(DateTime timeStamp, System.Windows.Forms.DataVisualization.Charting.Series ptSeries, int channel)
        {
            string StrDevs = host;
            if (device_dashboard.USBOpen(StrDevs) == true)
            {
                device_dashboard.Read_Sensor();

                double newVal = 0;

                if (ptSeries.Points.Count > 0)
                {
                    newVal = ptSeries.Points[ptSeries.Points.Count - 1].YValues[0] + ((rand.NextDouble() * 2) - 1);
                }

                if (newVal < 0)
                    newVal = 0;

                // Add new Data point to its series.
                switch (channel)
                {
                    case 1:
                        ptSeries.Points.AddXY(timeStamp.ToOADate(), device_dashboard.channels[0].Val.ToString("0.0"));
                        break;
                    case 2:// set value to Channel 2
                        ptSeries.Points.AddXY(timeStamp.ToOADate(), device_dashboard.channels[1].Val.ToString("0.0"));
                        break;
                    case 3:
                        ptSeries.Points.AddXY(timeStamp.ToOADate(), device_dashboard.channels[2].Val.ToString("0.0"));
                        break;
                    case 4:
                        ptSeries.Points.AddXY(timeStamp.ToOADate(), device_dashboard.channels[3].Val.ToString("0.0"));
                        break;
                    case 5:
                        ptSeries.Points.AddXY(timeStamp.ToOADate(), device_dashboard.channels[4].Val.ToString("0.0"));
                        break;
                    case 6:// set virtual value to Channel 6
                        //ptSeries.Points.AddXY(timeStamp.ToOADate(), rand.NextDouble() + 26);
                        ptSeries.Points.AddXY(timeStamp.ToOADate(), device_dashboard.channels[5].Val.ToString());
                        break;
                    case 7:
                        ptSeries.Points.AddXY(timeStamp.ToOADate(), device_dashboard.channels[6].Val.ToString("0.0"));
                        break;
                    case 8:
                        ptSeries.Points.AddXY(timeStamp.ToOADate(), device_dashboard.channels[7].Val.ToString("0.0"));
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

                //chart1.ChartAreas[0].AxisY.Minimum = ptSeries.Points[0].YValues[0] - 10;
                //chart1.ChartAreas[0].AxisY.Minimum = ptSeries.Points[0].YValues[0] + 10;

                //chart1.ChartAreas[0].AxisY2.Minimum = ptSeries.Points[0].YValues[0] - 10;
                //chart1.ChartAreas[0].AxisY2.Minimum = ptSeries.Points[0].YValues[0] + 10;


                chart1.ChartAreas[0].RecalculateAxesScale();
                chart1.ChartAreas[0].AxisX.Minimum = ptSeries.Points[0].XValue;
                chart1.ChartAreas[0].AxisX.Maximum = DateTime.FromOADate(ptSeries.Points[0].XValue).AddMinutes(2).ToOADate();

                chart1.Invalidate();
            }
            device_dashboard.Close();
        }

        private void frmDashBoardGraph_FormClosed(object sender, FormClosedEventArgs e)
        {
            btnStop_Click(sender, e);
        }

        private void DashBoardGraph_Resize(object sender, EventArgs e)
        {
            chart1.Height = this.Height - 100;
            chart1.Width = this.Width - 50;
        }

        
    }
}
