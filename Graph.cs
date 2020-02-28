using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Pexo16
{
    public partial class Graph : Form
    {
        public TimeZoneInfo _timezone;
        public TimeZoneInfo writeTimeZone;
        public DateTime StartTime;
        public string _starttime;
        public DateTime StopTime;
        public string _stoptime;
        public DateTime BaseTime;
        public string _eclapsetime;
        int chartArea = 0;

        Device deviceOpen;
        Device35 device35;

        //Point? prevPosition = null;
        ToolTip tooltip = new ToolTip();
        int count = 0;

        ResourceManager res_man = new ResourceManager("Pexo16.Lang.Resources", typeof(Graph).Assembly);
        CultureInfo cul;

        private static Graph instance;

        public static Graph Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Graph();
                }
                return instance;
            }
        }

        public static Graph DelInstance()
        {
            instance = null;
            return instance;
        }

        public Graph()
        {
            InitializeComponent();
            deviceOpen = Device.Instance;
            device35 = Device35.Instance;
            mGlobal.unitFromFile = true;

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

        }

        private void DeviceFromFile_Load(object sender, EventArgs e)
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

            //chkShowArea1.Text = res_man.GetString("Show chart area", cul) + " 1";
            //chkShowArea2.Text = res_man.GetString("Show chart area", cul) + " 2";
            this.Width = ParentForm.Width;
            this.Height = ParentForm.Height;
        }

        public void draw_graph()
        {
            //chkShowArea1.Visible = false;
            //chkShowArea2.Visible = false;
            float[] sum = new float[deviceOpen.numOfChannel];

            int[] len = new int[8];
            Single GraphMaxLeft, GrapMinLeft;
            Single GrapMaxRight, GrapMinRight;

            if (!mGlobal.open_file)
            {
                deviceOpen.offset = 128;

                int[] temp_unit = new int[deviceOpen.numOfChannel];
                mGlobal.get_unit(ref temp_unit, deviceOpen.data_open, deviceOpen.numOfChannel);
                for (int i = 0; i < deviceOpen.numOfChannel; i++)
                {
                    deviceOpen.channels[i].Unit = byte.Parse(temp_unit[i].ToString());
                }

                mGlobal.get_duration(deviceOpen.Duration, deviceOpen.data_open);
                mGlobal.get_delay(deviceOpen.Delay, deviceOpen.data_open);
            }

            StartTime = deviceOpen._logger_date;
            _starttime = deviceOpen._logger_date.ToString("MM/dd/yyyy HH:mm:ss");
            int sec1;
            int min2;

            if (Convert.ToInt32(deviceOpen.Duration) > 60)
            {
                min2 = Convert.ToInt32(deviceOpen.Duration) / 60;
                sec1 = Convert.ToInt32(deviceOpen.Duration) % 60;
                deviceOpen.Interval = min2 + " Min " + sec1 + " Sec";
            }
            else
            {
                deviceOpen.Interval = deviceOpen.Duration + " Sec";
            }

            BaseTime = deviceOpen._logger_date.AddMinutes(Convert.ToInt32(deviceOpen.Delay));

            mGlobal.num_measure_suminfo = (deviceOpen.data_open.Length - deviceOpen.offset) / 16;
            _eclapsetime = mGlobal.Sec2Day((mGlobal.num_measure_suminfo) * Convert.ToInt32(deviceOpen.Duration));

            string temp = _eclapsetime;

            string d = temp.Substring(0, temp.IndexOf(" "));
            temp = temp.Substring((temp.IndexOf(":") + 1 - 2) - 1);
            string h = temp.Substring((temp.IndexOf(":") + 1 - 2) - 1, 2);
            temp = temp.Substring(temp.IndexOf(":") + 1);
            string m = temp.Substring(0, temp.IndexOf(":"));
            temp = temp.Substring(temp.IndexOf(":") + 1);
            string s = temp;

            _stoptime = deviceOpen._logger_date.AddDays(double.Parse(d)).AddHours(double.Parse(h)).AddMinutes(double.Parse(deviceOpen.Delay.ToString()) + double.Parse(m)).AddSeconds(double.Parse(s)).ToString("MM/dd/yyyy HH:mm:ss");
            StopTime = deviceOpen._logger_date.AddDays(double.Parse(d)).AddHours(double.Parse(h)).AddMinutes(double.Parse(deviceOpen.Delay.ToString()) + double.Parse(m)).AddSeconds(double.Parse(s));

            for (int i = 0; i < deviceOpen.numOfChannel; i++)
            {
                deviceOpen.channels[i].Data = new float[(deviceOpen.data_open.Length - deviceOpen.offset) / 16]; //+1];
            }

            TimeSpan a = StopTime - StartTime;
            double secElapse = a.TotalSeconds;

            byte ch = 0;
            float val = 0;

            if (mGlobal.C2F)//C degree
            {
                for (int i = deviceOpen.offset; i < deviceOpen.data_open.Length; i += 2)
                {
                    byte tmpDiv;
                    if (deviceOpen.channels[((i - 6) % 16) / 2].Unit == 3)
                    {
                        tmpDiv = 10;
                    }
                    else tmpDiv = 10;

                    if (i < deviceOpen.data_open.Length - 1)
                    {
                        val = mGlobal.get_temp(deviceOpen.data_open[i + 1], deviceOpen.data_open[i]) / tmpDiv;
                    }

                    switch (ch)
                    {
                        case 0:
                            if (deviceOpen.channels[0].Unit == 2) //F
                                deviceOpen.channels[0].Data[len[0]] = mGlobal.ExchangeF2C(ref val);
                            else
                                deviceOpen.channels[0].Data[len[0]] = val;
                            len[0] += 1;
                            break;
                        case 1:
                            if (deviceOpen.channels[1].Unit == 2)
                                deviceOpen.channels[1].Data[len[1]] = mGlobal.ExchangeF2C(ref val);
                            else
                                deviceOpen.channels[1].Data[len[1]] = val;
                            len[1] += 1;
                            break;
                        case 2:
                            if (deviceOpen.channels[2].Unit == 2)
                                deviceOpen.channels[2].Data[len[2]] = mGlobal.ExchangeF2C(ref val);
                            else
                                deviceOpen.channels[2].Data[len[2]] = val;
                            len[2] += 1;
                            break;
                        case 3:
                            if (deviceOpen.channels[3].Unit == 2)
                                deviceOpen.channels[3].Data[len[3]] = mGlobal.ExchangeF2C(ref val);
                            else
                                deviceOpen.channels[3].Data[len[3]] = val;
                            len[3] += 1;
                            break;
                        case 4:
                            if (deviceOpen.channels[4].Unit == 2)
                                deviceOpen.channels[4].Data[len[4]] = mGlobal.ExchangeF2C(ref val);
                            else
                                deviceOpen.channels[4].Data[len[4]] = val;
                            len[4] += 1;
                            break;
                        case 5:
                            if (deviceOpen.channels[5].Unit == 2)
                                deviceOpen.channels[5].Data[len[5]] = mGlobal.ExchangeF2C(ref val);
                            else
                                deviceOpen.channels[5].Data[len[5]] = val;
                            len[5] += 1;
                            break;
                        case 6:
                            if (deviceOpen.channels[6].Unit == 2)
                                deviceOpen.channels[6].Data[len[6]] = mGlobal.ExchangeF2C(ref val);
                            else
                                deviceOpen.channels[6].Data[len[6]] = val;
                            len[6] += 1;
                            break;
                        case 7:
                            if (deviceOpen.channels[7].Unit == 2)
                                deviceOpen.channels[7].Data[len[7]] = mGlobal.ExchangeF2C(ref val);
                            else
                                deviceOpen.channels[7].Data[len[7]] = val;
                            len[7] += 1;
                            break;
                    }
                    ch += 1;
                    if (ch == deviceOpen.numOfChannel) ch = 0;
                }
                for (int j = 0; j < deviceOpen.numOfChannel; j++)
                {
                    if (count == 0)
                    {
                        mGlobal.unitTemp[j] = deviceOpen.channels[j].Unit;
                        if (mGlobal.unitTemp[j] == 2)
                        {
                            mGlobal.unitTemp[j] = 1;
                        }
                    }
                    else if(count == 1)
                    {
                        mGlobal.unitTemp[j] = deviceOpen.channels[j].Unit;
                        if (mGlobal.unitTemp[j] == 2)
                        {
                            mGlobal.unitTemp[j] = 1;
                            deviceOpen.channels[j].AlarmMax = (int)Math.Round((deviceOpen.channels[j].AlarmMax - 32) / 1.8);
                            deviceOpen.channels[j].AlarmMin = (int)Math.Round((deviceOpen.channels[j].AlarmMin - 32) / 1.8);
                        }
                    }
                    else
                    {
                        if (mGlobal.unitTemp[j] == 2)
                        {
                            mGlobal.unitTemp[j] = 1;
                            deviceOpen.channels[j].AlarmMax = (int)Math.Round((deviceOpen.channels[j].AlarmMax - 32) / 1.8);
                            deviceOpen.channels[j].AlarmMin = (int)Math.Round((deviceOpen.channels[j].AlarmMin - 32) / 1.8);
                        }
                    }
                }
                count += 1;
            }
            else//F degree
            {
                for (int i = deviceOpen.offset; i < deviceOpen.data_open.Length; i += 2)
                {
                    byte tmpDiv;
                    if (deviceOpen.channels[((i - 6) % 16) / 2].Unit == 3)
                    {
                        tmpDiv = 10;
                    }
                    else
                    {
                        tmpDiv = 10;
                    }
                    if (i < deviceOpen.data_open.Length - 1)
                    {
                        val = mGlobal.get_temp(deviceOpen.data_open[i + 1], deviceOpen.data_open[i]) / tmpDiv;
                    }

                    switch (ch)
                    {
                        case 0:
                            if (deviceOpen.channels[0].Unit == 1)// C
                                deviceOpen.channels[0].Data[len[0]] = mGlobal.ExchangeC2F(ref val);
                            else
                                deviceOpen.channels[0].Data[len[0]] = val;
                            len[0] += 1;
                            break;
                        case 1:
                            if (deviceOpen.channels[1].Unit == 1)
                                deviceOpen.channels[1].Data[len[1]] = mGlobal.ExchangeC2F(ref val);
                            else
                                deviceOpen.channels[1].Data[len[1]] = val;
                            len[1] += 1;
                            break;
                        case 2:
                            if (deviceOpen.channels[2].Unit == 1)
                                deviceOpen.channels[2].Data[len[2]] = mGlobal.ExchangeC2F(ref val);
                            else
                                deviceOpen.channels[2].Data[len[2]] = val;
                            len[2] += 1;
                            break;
                        case 3:
                            if (deviceOpen.channels[3].Unit == 1)
                                deviceOpen.channels[3].Data[len[3]] = mGlobal.ExchangeC2F(ref val);
                            else
                                deviceOpen.channels[3].Data[len[3]] = val;
                            len[3] += 1;
                            break;
                        case 4:
                            if (deviceOpen.channels[4].Unit == 1)
                                deviceOpen.channels[4].Data[len[4]] = mGlobal.ExchangeC2F(ref val);
                            else
                                deviceOpen.channels[4].Data[len[4]] = val;
                            len[4] += 1;
                            break;
                        case 5:
                            if (deviceOpen.channels[5].Unit == 1)
                                deviceOpen.channels[5].Data[len[5]] = mGlobal.ExchangeC2F(ref val);
                            else
                                deviceOpen.channels[5].Data[len[5]] = val;
                            len[5] += 1;
                            break;
                        case 6:
                            if (deviceOpen.channels[6].Unit == 1)
                                deviceOpen.channels[6].Data[len[6]] = mGlobal.ExchangeC2F(ref val);
                            else
                                deviceOpen.channels[6].Data[len[6]] = val;
                            len[6] += 1;
                            break;
                        case 7:
                            if (deviceOpen.channels[7].Unit == 1)
                                deviceOpen.channels[7].Data[len[7]] = mGlobal.ExchangeC2F(ref val);
                            else
                                deviceOpen.channels[7].Data[len[7]] = val;
                            len[7] += 1;
                            break;
                    }
                    ch += 1;
                    if (ch == deviceOpen.numOfChannel) ch = 0;
                }

                for (int j = 0; j < deviceOpen.numOfChannel; j++)
                {
                    if (count == 0)
                    {
                        mGlobal.unitTemp[j] = deviceOpen.channels[j].Unit;
                        if (mGlobal.unitTemp[j] == 1)
                        {
                            mGlobal.unitTemp[j] = 2;//Unit[5,6,7] = {ppm = 3; %RH = 4}
                        }
                    }
                    else if(count == 1)
                    {
                        mGlobal.unitTemp[j] = deviceOpen.channels[j].Unit;
                        if (mGlobal.unitTemp[j] == 1)
                        {
                            deviceOpen.channels[j].AlarmMax = (int)Math.Round(deviceOpen.channels[j].AlarmMax * 1.8) + 32;
                            deviceOpen.channels[j].AlarmMin = (int)Math.Round(deviceOpen.channels[j].AlarmMin * 1.8) + 32;
                            mGlobal.unitTemp[j] = 2;//Unit[5,6,7] = {ppm = 3; %RH = 4}
                        }
                    }
                    else
                    {
                        if (mGlobal.unitTemp[j] == 1)
                        {
                            deviceOpen.channels[j].AlarmMax = (int)Math.Round(deviceOpen.channels[j].AlarmMax * 1.8) + 32;
                            deviceOpen.channels[j].AlarmMin = (int)Math.Round(deviceOpen.channels[j].AlarmMin * 1.8) + 32;
                            mGlobal.unitTemp[j] = 2;//Unit[5,6,7] = {ppm = 3; %RH = 4}
                        }
                    }
                }
                count += 1;
            }

            // Draw Graph
            chart1.Width = this.Width;
            chart1.Height = 600;
            chart1.Left = this.Width / 1362;

            chart1.ChartAreas.Clear();
            chart1.ChartAreas.Add("area1");
            //chart1.ChartAreas.Add("area2");

            chart1.ChartAreas[0].AxisY.LineWidth = 2;
            chart1.ChartAreas[0].AxisY2.LineWidth = 2;
            chart1.ChartAreas[0].AxisY.LineColor = Color.SeaGreen;
            chart1.ChartAreas[0].AxisY2.LineColor = Color.SeaGreen;
            chart1.ChartAreas[0].BorderDashStyle = ChartDashStyle.Dash;
            chart1.ChartAreas[0].AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dot;
            chart1.ChartAreas[0].AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dot;
            
            chart1.ChartAreas[0].AxisY2.MajorGrid.Enabled = false;
            chart1.ChartAreas[0].BorderDashStyle = ChartDashStyle.Solid;

            chart1.ChartAreas[0].AxisY2.Enabled = AxisEnabled.True;

            //to zoom chart.
            // Enable range selection and zooming end user interface
            chart1.ChartAreas[0].CursorX.IsUserEnabled = true;
            chart1.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
            //chart1.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
            //chart1.ChartAreas[0].AxisX.ScrollBar.IsPositionedInside = true;

            chart1.ChartAreas[0].CursorY.IsUserEnabled = true;
            chart1.ChartAreas[0].CursorY.IsUserSelectionEnabled = true;
            chart1.ChartAreas[0].AxisY.ScaleView.Zoomable = true;
            chart1.ChartAreas[0].AxisY.ScrollBar.IsPositionedInside = true;
            //chart1.ChartAreas[0].AxisY.ScrollBar.BackColor = Color.Khaki;

            chart1.ChartAreas[0].AxisY2.ScaleView.ZoomReset(0);
            chart1.ChartAreas[0].AxisY2.ScaleView.Zoomable = true;
            chart1.ChartAreas[0].AxisY2.ScrollBar.IsPositionedInside = true;
            chart1.ChartAreas[0].AxisY2.ScrollBar.Enabled = true;

            chart1.ChartAreas[0].CursorX.SetCursorPosition(double.NaN);
            chart1.ChartAreas[0].CursorY.SetCursorPosition(double.NaN);
            chart1.ChartAreas[0].CursorX.LineWidth = 0;
            chart1.ChartAreas[0].CursorY.LineWidth = 0;
            chart1.ChartAreas[0].AxisX.LabelStyle.IntervalOffset = 0;
            chart1.ChartAreas[0].AxisX.LabelStyle.IsEndLabelVisible = true;
            chart1.ChartAreas[0].AxisX.MinorGrid.Enabled = true;
            chart1.ChartAreas[0].AxisX.MinorGrid.LineDashStyle = ChartDashStyle.Dot;

            chart1.ChartAreas[0].AxisX.IsLabelAutoFit = true;
            chart1.ChartAreas[0].AxisX.LabelAutoFitStyle = LabelAutoFitStyles.StaggeredLabels;
            chart1.ChartAreas[0].AxisX.IntervalAutoMode = IntervalAutoMode.FixedCount;
            chart1.ChartAreas[0].AxisX.IsStartedFromZero = true;
            chart1.ChartAreas[0].AxisY2.Enabled = AxisEnabled.False;
            chart1.ChartAreas[0].AxisX.IsStartedFromZero = true;


            chart1.Legends[0].BorderColor = Color.Aquamarine;
            chart1.Legends[0].BorderWidth = 2;
            chart1.Legends[0].BorderDashStyle = ChartDashStyle.Solid;
            chart1.Legends[0].ShadowOffset = 2;


            //chart1.ChartAreas[0].BackColor = Color.Ivory;

            
            //chart1.ChartAreas[0].AxisX.MinorGrid.Interval = 20;
            //chart1.ChartAreas[0].AxisX.MajorGrid.Interval = 10;
            //chart1.ChartAreas[0].AxisX.MajorGrid.Interval = 20;
            //chart1.ChartAreas[0].AxisX.MinorTickMark.Interval = 15;
            //chart1.ChartAreas[0].AxisX.MinorGrid.Interval = 10;
            
            //set title chart
            if (mGlobal.TitleChanged)
            {
                chart1.Titles[0].Text = mGlobal.TitleGraph;
            }
            else
            {
                if (deviceOpen.titlegraph == "")
                {
                    deviceOpen.titlegraph = "Marathon Electronic Data Logger";
                }
                chart1.Titles[0].Text = deviceOpen.titlegraph;
            }

            //set footer chart
            string footer = "Desc: " + deviceOpen.Description + " - Loc: " + deviceOpen.Location + " " +
                    "Time: " + secElapse.ToString() + ", " + StartTime.ToString()
                    + " to " + StopTime.ToString() + ", " + _timezone.ToString();
            chart1.Titles[1].Text = footer;
            Title footertitle = chart1.Titles[1];
            footertitle.Docking = Docking.Bottom;
            footertitle.Alignment = ContentAlignment.BottomCenter;

            // Reset number of series in the Chart.
            chart1.Series.Clear();
            Series[] newSer = new Series[3 * deviceOpen.numOfChannel];
            for (int i = 0; i < deviceOpen.numOfChannel; i++)
            {
                if (deviceOpen.channels[i].Unit != 0)
                {
                    newSer[i] = new Series("Channel " + (i + 1) + " (" + mGlobal.IntToUnit(byte.Parse(mGlobal.unitTemp[i].ToString())) + ")");
                    newSer[i].ChartType = SeriesChartType.Line;
                    newSer[i].BorderWidth = 2;
                    if (deviceOpen.channels[i].Unit == 3)
                    {
                        newSer[i].YAxisType = AxisType.Secondary;
                    }
                    newSer[i].ChartArea = "area1";
                }
            }
            //for (int i = 0; i < 2 * deviceOpen.numOfChannel; i+=2)
            //{
            //    newSer[i + deviceOpen.numOfChannel] = new Series("Channel " + (i/2 + 1) + " : MaxAlarm");
            //    newSer[i + deviceOpen.numOfChannel].ChartType = SeriesChartType.Line;
            //    newSer[i + 1 + deviceOpen.numOfChannel] = new Series("Channel " + (i/2 + 1) + " : MinAlarm");
            //    newSer[i + 1 + deviceOpen.numOfChannel].ChartType = SeriesChartType.Line;
            //    if (deviceOpen.channels[i/2].Unit == 3)
            //    {
            //        newSer[i + deviceOpen.numOfChannel].YAxisType = AxisType.Secondary;
            //        newSer[i + 1 + deviceOpen.numOfChannel].YAxisType = AxisType.Secondary;
            //    }
            //}

            DateTime GraphMinTime, GraphMaxTime;

            if (!mGlobal.tlb_eclapse)
            {
                TimeZoneInfo localZone;
                TimeZoneInfo TargetZone;
                mGlobal._get_timezone_date(ref writeTimeZone, ref deviceOpen._logger_date, deviceOpen.data_open);

                localZone = writeTimeZone;
                TargetZone = mGlobal.FindSystemTimeZoneFromDisplayName(_timezone.ToString());

                double OffsetHour, OffsetMin;
                try
                {
                    OffsetHour = Convert.ToDouble(_timezone.ToString().Substring(4, 3));
                    OffsetMin = Convert.ToDouble(_timezone.ToString().Substring(8, 2));
                }
                catch (Exception)
                {
                    OffsetHour = 0;
                    OffsetMin = 0;
                }

                DateTime theUTCTime;
                theUTCTime = TimeZoneInfo.ConvertTimeToUtc(StartTime, localZone);
                GraphMinTime = theUTCTime.AddHours(OffsetHour).AddMinutes(OffsetMin);

                theUTCTime = TimeZoneInfo.ConvertTimeToUtc(StopTime, localZone);
                GraphMaxTime = theUTCTime.AddHours(OffsetHour).AddMinutes(OffsetMin);

                ReadOnlyCollection<TimeZoneInfo> timeZones = TimeZoneInfo.GetSystemTimeZones();

                foreach (TimeZoneInfo timeZone in timeZones)
                {
                    if (timeZone.DisplayName == _timezone.ToString())
                    {
                        TimeZoneInfo.AdjustmentRule[] adjustments = timeZone.GetAdjustmentRules();

                        if (adjustments.Length == 0)
                        {
                            theUTCTime = TimeZoneInfo.ConvertTimeToUtc(StartTime, localZone);
                            GraphMinTime = theUTCTime.AddHours(OffsetHour).AddMinutes(OffsetMin);

                            theUTCTime = TimeZoneInfo.ConvertTimeToUtc(StopTime, localZone);
                            GraphMaxTime = theUTCTime.AddHours(OffsetHour).AddMinutes(OffsetMin);
                        }

                        foreach (TimeZoneInfo.AdjustmentRule daylight in adjustments)
                        {
                            if (timeZone.IsDaylightSavingTime(GraphMinTime))
                            {
                                theUTCTime = TimeZoneInfo.ConvertTimeToUtc(StartTime.AddHours(daylight.DaylightDelta.Hours).AddMinutes(daylight.DaylightDelta.Minutes).AddSeconds(daylight.DaylightDelta.Seconds), localZone);
                                GraphMinTime = theUTCTime.AddHours(OffsetHour).AddMinutes(OffsetMin);

                                theUTCTime = TimeZoneInfo.ConvertTimeToUtc(StopTime.AddHours(daylight.DaylightDelta.Hours).AddMinutes(daylight.DaylightDelta.Minutes).AddSeconds(daylight.DaylightDelta.Seconds), localZone);
                                GraphMaxTime = theUTCTime.AddHours(OffsetHour).AddMinutes(OffsetMin);
                            }
                            else
                            {
                                theUTCTime = TimeZoneInfo.ConvertTimeToUtc(StartTime, localZone);
                                GraphMinTime = theUTCTime.AddHours(OffsetHour).AddMinutes(OffsetMin);

                                theUTCTime = TimeZoneInfo.ConvertTimeToUtc(StopTime, localZone);
                                GraphMaxTime = theUTCTime.AddHours(OffsetHour).AddMinutes(OffsetMin);
                            }
                        }
                    }
                }

                //BaseTime = GraphMinTime;
                BaseTime = GraphMinTime.AddMinutes(Convert.ToInt32(deviceOpen.Delay));

                chart1.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
                chart1.ChartAreas[0].AxisY.ScaleView.Zoomable = true;
                chart1.ChartAreas[0].AxisY2.ScaleView.Zoomable = true;
                chart1.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
                chart1.ChartAreas[0].AxisX.MajorTickMark.Enabled = false;
                if (secElapse < 50)
                {
                    chart1.ChartAreas[0].AxisX.MinorGrid.Interval = 2;
                    chart1.ChartAreas[0].AxisX.LabelStyle.Interval = 2;
                }
                else
                {
                    chart1.ChartAreas[0].AxisX.MinorGrid.Interval = (int)(secElapse / 50);
                    chart1.ChartAreas[0].AxisX.LabelStyle.Interval = (int)(secElapse / 50);
                }
                
                //frmViewData
                for (int i = 0; i < deviceOpen.channels[0].Data.Length; i++)
                {
                    for (int j = 0; j < deviceOpen.numOfChannel; j++)
                    {
                        if (deviceOpen.channels[j].Unit != 0)
                        {
                            newSer[j].Points.AddXY(BaseTime.AddSeconds(deviceOpen.Duration * i).ToString("HH:mm:ss \n dd/MM/yyyy"), deviceOpen.channels[j].Data[i]);
                            //newSer[j].Points.AddXY(BaseTime.AddSeconds(deviceOpen.Duration * i).ToLongDateString(), deviceOpen.channels[j].Data[i]);
                            sum[j] += deviceOpen.channels[j].Data[i];
                        }
                        //newSer[j + deviceOpen.numOfChannel].Points.AddXY(BaseTime.AddSeconds(deviceOpen.Duration * i).ToString("HH:mm:ss \n dd/MM/yyyy"), deviceOpen.Channels[j].AlarmMax);
                        //newSer[j + 1 + deviceOpen.numOfChannel].Points.AddXY(BaseTime.AddSeconds(deviceOpen.Duration * i).ToString("HH:mm:ss \n dd/MM/yyyy"), deviceOpen.Channels[j].AlarmMin);
                    }
                //    newSer[8].Points.AddXY(BaseTime.AddSeconds(deviceOpen.Duration * i).ToString("HH:mm:ss \n dd/MM/yyyy"), deviceOpen.channels[0].AlarmMax);
                //    newSer[9].Points.AddXY(BaseTime.AddSeconds(deviceOpen.Duration * i).ToString("HH:mm:ss \n dd/MM/yyyy"), deviceOpen.channels[0].AlarmMin);
                //    newSer[10].Points.AddXY(BaseTime.AddSeconds(deviceOpen.Duration * i).ToString("HH:mm:ss \n dd/MM/yyyy"), deviceOpen.channels[1].AlarmMax);
                //    newSer[11].Points.AddXY(BaseTime.AddSeconds(deviceOpen.Duration * i).ToString("HH:mm:ss \n dd/MM/yyyy"), deviceOpen.channels[1].AlarmMin);
                //    newSer[12].Points.AddXY(BaseTime.AddSeconds(deviceOpen.Duration * i).ToString("HH:mm:ss \n dd/MM/yyyy"), deviceOpen.channels[2].AlarmMax);
                //    newSer[13].Points.AddXY(BaseTime.AddSeconds(deviceOpen.Duration * i).ToString("HH:mm:ss \n dd/MM/yyyy"), deviceOpen.channels[2].AlarmMin);
                //    newSer[14].Points.AddXY(BaseTime.AddSeconds(deviceOpen.Duration * i).ToString("HH:mm:ss \n dd/MM/yyyy"), deviceOpen.channels[3].AlarmMax);
                //    newSer[15].Points.AddXY(BaseTime.AddSeconds(deviceOpen.Duration * i).ToString("HH:mm:ss \n dd/MM/yyyy"), deviceOpen.channels[3].AlarmMin);
                //    newSer[16].Points.AddXY(BaseTime.AddSeconds(deviceOpen.Duration * i).ToString("HH:mm:ss \n dd/MM/yyyy"), deviceOpen.channels[4].AlarmMax);
                //    newSer[17].Points.AddXY(BaseTime.AddSeconds(deviceOpen.Duration * i).ToString("HH:mm:ss \n dd/MM/yyyy"), deviceOpen.channels[4].AlarmMin);
                //    newSer[18].Points.AddXY(BaseTime.AddSeconds(deviceOpen.Duration * i).ToString("HH:mm:ss \n dd/MM/yyyy"), deviceOpen.channels[5].AlarmMax);
                //    newSer[19].Points.AddXY(BaseTime.AddSeconds(deviceOpen.Duration * i).ToString("HH:mm:ss \n dd/MM/yyyy"), deviceOpen.channels[5].AlarmMin);
                //    newSer[20].Points.AddXY(BaseTime.AddSeconds(deviceOpen.Duration * i).ToString("HH:mm:ss \n dd/MM/yyyy"), deviceOpen.channels[6].AlarmMax);
                //    newSer[21].Points.AddXY(BaseTime.AddSeconds(deviceOpen.Duration * i).ToString("HH:mm:ss \n dd/MM/yyyy"), deviceOpen.channels[6].AlarmMin);
                //    newSer[22].Points.AddXY(BaseTime.AddSeconds(deviceOpen.Duration * i).ToString("HH:mm:ss \n dd/MM/yyyy"), deviceOpen.channels[7].AlarmMax);
                //    newSer[23].Points.AddXY(BaseTime.AddSeconds(deviceOpen.Duration * i).ToString("HH:mm:ss \n dd/MM/yyyy"), deviceOpen.channels[7].AlarmMin);
                }
            }
            else //eclapse Time
            {
                Int32 day_no = 0;
                string str;
                BaseTime = new DateTime(2000, 1, 1, 0, 0, 0);
                GraphMinTime = BaseTime;
                BaseTime = GraphMinTime.AddMinutes(Convert.ToInt32(0));
                GraphMaxTime = BaseTime.AddSeconds(deviceOpen.Duration * (deviceOpen.channels[0].Data.Length - 1));
                chart1.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
                chart1.ChartAreas[0].AxisY.ScaleView.Zoomable = true;
                chart1.ChartAreas[0].AxisY2.ScaleView.Zoomable = true;
                chart1.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
                chart1.ChartAreas[0].AxisX.LabelAutoFitStyle = LabelAutoFitStyles.DecreaseFont;
                chart1.ChartAreas[0].AxisX.IsLabelAutoFit = true;
                chart1.ChartAreas[0].AxisX.LabelStyle.Format = "HH:mm:ss";

                if (secElapse < 50)
                {
                    if (chart1.ChartAreas[0].AxisX.ScaleView.IsZoomed == true)
                    {
                        chart1.ChartAreas[0].AxisX.IsLabelAutoFit = true;
                        chart1.ChartAreas[0].AxisX.IntervalAutoMode = IntervalAutoMode.FixedCount;
                    }
                    else
                    {
                        chart1.ChartAreas[0].AxisX.MinorGrid.Interval = 2;
                        chart1.ChartAreas[0].AxisX.LabelStyle.Interval = 2;
                    }
                }
                else
                {
                    if (chart1.ChartAreas[0].AxisX.ScaleView.IsZoomed == true)
                    {
                        chart1.ChartAreas[0].AxisX.IsLabelAutoFit = true;
                        chart1.ChartAreas[0].AxisX.IntervalAutoMode = IntervalAutoMode.FixedCount;
                    }
                    else
                    {
                        chart1.ChartAreas[0].AxisX.LabelStyle.Interval = (int)((int)(secElapse / 50));
                        chart1.ChartAreas[0].AxisX.MinorGrid.Interval = (int)((int)(secElapse / 50));
                    }
                }

                for (int i = 0; i < deviceOpen.channels[0].Data.Length; i++)
                {
                    day_no = BaseTime.AddSeconds(deviceOpen.Duration * i).DayOfYear - 1;
                    str = Environment.NewLine + " day " + BaseTime.AddSeconds(deviceOpen.Duration * i).DayOfYear.ToString();

                    //str = (BaseTime.AddSeconds(deviceOpen.Duration * i).Day - 1).ToString() + " days" + Environment.NewLine + BaseTime.AddSeconds(deviceOpen.Duration * i).ToString("HH:mm:ss");

                    Object XValue = BaseTime.AddSeconds(deviceOpen.Duration * i).ToString("HH:mm:ss");

                    for (int j = 0; j < deviceOpen.numOfChannel; j++)
                    {
                        if (deviceOpen.channels[j].Unit != 0)
                        {
                            //newSer[j].Points.AddXY(str, deviceOpen.channels[j].Data[i]);
                            newSer[j].Points.AddXY(BaseTime.AddSeconds(deviceOpen.Duration * i).ToString("HH:mm:ss"), deviceOpen.channels[j].Data[i]);
                            sum[j] += deviceOpen.channels[j].Data[i];
                        }
                    }
                }
            }

            for (int i = 0; i < deviceOpen.numOfChannel; i++)
            {
                deviceOpen.channels[i].high_suminfo = mGlobal.find_max(deviceOpen.channels[i].Data);
                deviceOpen.channels[i].low_suminfo = mGlobal.find_min(deviceOpen.channels[i].Data);
            }

            GraphMaxLeft = -1000;
            GrapMaxRight = -1000;
            GrapMinLeft = 1000;
            GrapMinRight = 1000;

            for (int i = 0; i < deviceOpen.numOfChannel; i++)
            {
                if (deviceOpen.channels[i].Unit != 3)
                {
                    GraphMaxLeft = Math.Max(deviceOpen.channels[i].high_suminfo, GraphMaxLeft);
                    GrapMinLeft = Math.Min(deviceOpen.channels[i].low_suminfo, GrapMinLeft);
                }
                else
                {
                    GrapMaxRight = Math.Max(deviceOpen.channels[i].high_suminfo, GrapMaxRight);
                    GrapMinRight = Math.Min(deviceOpen.channels[i].low_suminfo, GrapMinRight);
                }
            }

            try
            {
                //if (deviceOpen.channels[0].LineColor == Color.Empty || deviceOpen.channels[0].LineColor == Color.White) deviceOpen.channels[0].LineColor = Color.Red;
                //if (deviceOpen.channels[1].LineColor == Color.Empty || deviceOpen.channels[1].LineColor == Color.White) deviceOpen.channels[1].LineColor = Color.Black;
                //if (deviceOpen.channels[2].LineColor == Color.Empty || deviceOpen.channels[2].LineColor == Color.White) deviceOpen.channels[2].LineColor = Color.Yellow;
                //if (deviceOpen.channels[3].LineColor == Color.Empty || deviceOpen.channels[3].LineColor == Color.White) deviceOpen.channels[3].LineColor = Color.Green;
                //if (deviceOpen.channels[4].LineColor == Color.Empty || deviceOpen.channels[4].LineColor == Color.White) deviceOpen.channels[4].LineColor = Color.Blue;
                //if (deviceOpen.channels[5].LineColor == Color.Empty || deviceOpen.channels[5].LineColor == Color.White) deviceOpen.channels[5].LineColor = Color.Brown;
                //if (deviceOpen.channels[6].LineColor == Color.Empty || deviceOpen.channels[6].LineColor == Color.White) deviceOpen.channels[6].LineColor = Color.Cyan;
                //if (deviceOpen.channels[7].LineColor == Color.Empty || deviceOpen.channels[7].LineColor == Color.White) deviceOpen.channels[7].LineColor = Color.Orange;


                 deviceOpen.channels[0].LineColor = Color.Red;
                 deviceOpen.channels[1].LineColor = Color.Black;
                 deviceOpen.channels[2].LineColor = Color.Yellow;
                 deviceOpen.channels[3].LineColor = Color.Green;
                 deviceOpen.channels[4].LineColor = Color.Blue;
                 deviceOpen.channels[5].LineColor = Color.Brown;
                 deviceOpen.channels[6].LineColor = Color.Cyan;
                 deviceOpen.channels[7].LineColor = Color.Orange;

                for (int i = 0; i < deviceOpen.numOfChannel; i++)
                {
                    if (deviceOpen.channels[i].Unit != 0)
                    {
                        newSer[i].Color = deviceOpen.channels[i].LineColor;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            for (int i = 0; i < deviceOpen.numOfChannel; i++)
            {
                if (mGlobal.C2F)//show C
                {
                    if (mGlobal.unitTemp[i] == 2)
                        mGlobal.unitTemp[i] = 1;
                }
                else//show F
                {
                    if (mGlobal.unitTemp[i] == 1)
                        mGlobal.unitTemp[i] = 2;
                }
            }

            for (int i = 0; i < deviceOpen.numOfChannel; i++)
            {
                deviceOpen.channels[i].MaxCount = 0;
                deviceOpen.channels[i].MinCount = 0;

                if (!deviceOpen.channels[i].NoAlarm && deviceOpen.channels[i].Unit != 0)
                {
                    for (int j = 0; j < deviceOpen.channels[i].Data.Length; j++)
                    {
                        if (Convert.ToSingle(deviceOpen.channels[i].Data[j]) >= Convert.ToSingle(deviceOpen.channels[i].AlarmMax))
                        {
                            deviceOpen.channels[i].MaxCount = deviceOpen.channels[i].MaxCount + 1;
                        }
                        else if (Convert.ToSingle(deviceOpen.channels[i].Data[j]) <= Convert.ToSingle(deviceOpen.channels[i].AlarmMin))
                        {
                            deviceOpen.channels[i].MinCount = deviceOpen.channels[i].MinCount + 1;
                        }
                    }
                }
                deviceOpen.channels[i].ave_frm_suminfo = Math.Round(sum[i] / ((deviceOpen.data_open.Length - deviceOpen.offset) / 16), 2);
            }

            //title axis Y wiht series
            if (mGlobal.C2F)
            {
                chart1.ChartAreas[0].AxisY.Title = "Temp.(C) - Humid.(%RH)";
                chart1.ChartAreas[0].AxisY.TitleFont = new Font("Arial", 9.0F, FontStyle.Bold);
            }
            else
            {
                chart1.ChartAreas[0].AxisY.Title = "Temp.(F) - Humid.(%RH)";
                chart1.ChartAreas[0].AxisY.TitleFont = new Font("Arial", 9.0F, FontStyle.Bold); 
            }
            chart1.ChartAreas[0].AxisY2.Title = "PPM";
            chart1.ChartAreas[0].AxisY2.TitleFont = new Font("Arial", 9.0F, FontStyle.Bold);


            //set min and max in AxisY, AxisY2
            double lmin = Math.Round(GrapMinLeft, 0);
            double lmax = Math.Round(GraphMaxLeft, 0);
            double rmin = Math.Round(GrapMinRight, 0);
            double rmax = Math.Round(GrapMaxRight, 0);

            //chart1.ChartAreas[0].AxisY2.Maximum = rmax + 10;
            //chart1.ChartAreas[0].AxisY2.Minimum = 0;

            bool checkY2 = false;
            for (int i = 0; i < deviceOpen.numOfChannel; i++)
            {
                if (deviceOpen.channels[i].Unit == 3)
                {
                    checkY2 = true;
                }
            }

            if (checkY2)
            {
                chart1.ChartAreas[0].AxisY2.Maximum = rmax + 10;
                chart1.ChartAreas[0].AxisY2.Minimum = 0;
            }
            else
            {
                chart1.ChartAreas[0].AxisY2.Enabled = AxisEnabled.False;
            }

            chart1.ChartAreas[0].AxisY.Maximum = lmax + 5;
            chart1.ChartAreas[0].AxisY.Minimum = lmin - 5;
            chart1.AlignDataPointsByAxisLabel();
            //chart1.ChartAreas[0].AxisX.Minimum = Double.NaN;
            //chart1.ChartAreas[0].AxisY.Minimum = Double.NaN;
            
            //Add series to chart
            for (int i = 0; i < deviceOpen.numOfChannel; i++)
            {
                if (deviceOpen.channels[i].Unit != 0)
                {
                    chart1.Series.Add(newSer[i]);
                   
                }
            }

            for (int i = 0; i < chart1.Series.Count; i++)
            {
                 //set tooltip
                if (mGlobal.tlb_value)
                {
                    chart1.Series[i].ToolTip = chart1.Series[i].Name + "\r\n #VALX \r\n [#VALY{##.##}]";
                    //chart1.Series[i].ToolTip = "Channel: " + (i+1) + "\r\n #VALX \r\n [#VALY]"; // set tooltip.
                    //chart1.Series[i].LabelToolTip = "Channel: " + (i+1);
                }
            }
            //chkShowArea1.Visible = false;
            //chkShowArea2.Visible = false;
        }

        public void set_graph(byte[] record)
        {
            int offset = 0;

            float[] sum = new float[deviceOpen.numOfChannel];

            int[] len = new int[deviceOpen.numOfChannel];

            //xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx offset Data xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
            // offset: offset Data, offset=200 (v00), offset: offset when open file, offset=128: offset when read Data from MCU

            //xxxxxxxxxxxxxxxxxxx get Unit, Delay, Duration, max, min from Data xxxxxxxxxxxxxx
            if (mGlobal.open_file == false)
            {
                offset = 128;

                int[] temp_unit = new int[deviceOpen.numOfChannel];
                mGlobal.get_unit(ref temp_unit, record, deviceOpen.numOfChannel);
                for (int i = 0; i < deviceOpen.numOfChannel; i++)
                {
                    deviceOpen.channels[i].Unit = Convert.ToByte(temp_unit[i]);
                }
            }

            mGlobal._get_timezone_date(ref _timezone, ref deviceOpen._logger_date, record);
            _starttime = deviceOpen._logger_date.ToString("MM/dd/yyyy HH:mm:ss");

            int sec1 = 0;
            int min2 = 0;

            if (Convert.ToInt32(deviceOpen.Duration) > 60)
            {
                min2 = Convert.ToInt32(deviceOpen.Duration) / 60;
                sec1 = Convert.ToInt32(deviceOpen.Duration) % 60;
                deviceOpen.Interval = min2 + " Min " + sec1 + " Sec";
            }
            else
            {
                deviceOpen.Interval = Convert.ToInt32(deviceOpen.Duration) + " Sec";
            }

            //num of measure
            mGlobal.num_measure_suminfo = (record.Length - offset) / 16;

            //stoptime
            for (int i = 0; i < deviceOpen.numOfChannel; i++)
            {
                deviceOpen.channels[i].Data = new float[(record.Length - offset) / 16];
            }

            byte ch = 0;
            float Val = 0F;

            if (mGlobal.C2F == true) // show C degree
            {
                for (int i = offset; i < record.Length; i += 2)
                {
                    byte tmpDiv = 0;
                    if (deviceOpen.channels[(i % 16) / 2].Unit == 3)
                    {
                        tmpDiv = 1;
                    }
                    else
                    {
                        tmpDiv = 10;
                    }

                    Val = mGlobal.get_temp(record[i + 1], record[i]) / tmpDiv;

                    switch (ch)
                    {
                        case 0:
                            if (deviceOpen.channels[0].Unit == 2)
                            {
                                deviceOpen.channels[0].Data[len[0]] = mGlobal.ExchangeF2C(ref Val);
                            }
                            else
                            {
                                deviceOpen.channels[0].Data[len[0]] = Val;
                            }
                            len[0] += 1;
                            break;
                        case 1:
                            if (deviceOpen.channels[1].Unit == 2)
                            {
                                deviceOpen.channels[1].Data[len[1]] = mGlobal.ExchangeF2C(ref Val);
                            }
                            else
                            {
                                deviceOpen.channels[1].Data[len[1]] = Val;
                            }
                            len[1] += 1;
                            break;
                        case 2:
                            if (deviceOpen.channels[2].Unit == 2)
                            {
                                deviceOpen.channels[2].Data[len[2]] = mGlobal.ExchangeF2C(ref Val);
                            }
                            else
                            {
                                deviceOpen.channels[2].Data[len[2]] = Val;
                            }
                            len[2] += 1;
                            break;
                        case 3:
                            if (deviceOpen.channels[3].Unit == 2)
                            {
                                deviceOpen.channels[3].Data[len[3]] = mGlobal.ExchangeF2C(ref Val);
                            }
                            else
                            {
                                deviceOpen.channels[3].Data[len[3]] = Val;
                            }
                            len[3] += 1;
                            break;
                        case 4:
                            if (deviceOpen.channels[4].Unit == 2)
                            {
                                deviceOpen.channels[4].Data[len[4]] = mGlobal.ExchangeF2C(ref Val);
                            }
                            else
                            {
                                deviceOpen.channels[4].Data[len[4]] = Val;
                            }
                            len[4] += 1;
                            break;
                        case 5:
                            if (deviceOpen.channels[5].Unit == 2)
                            {
                                deviceOpen.channels[5].Data[len[5]] = mGlobal.ExchangeF2C(ref Val);
                            }
                            else
                            {
                                deviceOpen.channels[5].Data[len[5]] = Val;
                            }
                            len[5] += 1;
                            break;
                        case 6:
                            if (deviceOpen.channels[6].Unit == 2)
                            {
                                deviceOpen.channels[6].Data[len[6]] = mGlobal.ExchangeF2C(ref Val);
                            }
                            else
                            {
                                deviceOpen.channels[6].Data[len[6]] = Val;
                            }
                            len[6] += 1;
                            break;
                        case 7:
                            if (deviceOpen.channels[7].Unit == 2)
                            {
                                deviceOpen.channels[7].Data[len[7]] = mGlobal.ExchangeF2C(ref Val);
                            }
                            else
                            {
                                deviceOpen.channels[7].Data[len[7]] = Val;
                            }
                            len[7] += 1;
                            break;
                    }
                    ch += 1;
                    if (ch == deviceOpen.numOfChannel)
                    {
                        ch = 0;
                    }
                }

                for (int i = 0; i < deviceOpen.numOfChannel; i++)
                {
                    if (mGlobal.unitTemp[i] == 2)
                    {
                        mGlobal.unitTemp[i] = 1;
                    }
                }
            }
            else //show F degree
            {
                for (int i = offset; i < record.Length; i += 2)
                {
                    byte tmpDiv = 0;
                    if (deviceOpen.channels[(i % 16) / 2].Unit == 3)
                    {
                        tmpDiv = 1;
                    }
                    else
                    {
                        tmpDiv = 10;
                    }
                    Val = mGlobal.get_temp(record[i + 1], record[i]) / tmpDiv;

                    switch (ch)
                    {
                        case 0:
                            if (deviceOpen.channels[0].Unit == 1)
                            {
                                deviceOpen.channels[0].Data[len[1]] = mGlobal.ExchangeC2F(ref Val);
                            }
                            else
                            {
                                deviceOpen.channels[0].Data[len[1]] = Val;
                            }
                            len[1] += 1;
                            break;
                        case 1:
                            if (deviceOpen.channels[1].Unit == 1)
                            {
                                deviceOpen.channels[1].Data[len[2]] = mGlobal.ExchangeC2F(ref Val);
                            }
                            else
                            {
                                deviceOpen.channels[1].Data[len[2]] = Val;
                            }
                            len[2] += 1;
                            break;
                        case 2:
                            if (deviceOpen.channels[2].Unit == 1)
                            {
                                deviceOpen.channels[2].Data[len[3]] = mGlobal.ExchangeC2F(ref Val);
                            }
                            else
                            {
                                deviceOpen.channels[2].Data[len[3]] = Val;
                            }
                            len[3] += 1;
                            break;
                        case 3:
                            if (deviceOpen.channels[3].Unit == 1)
                            {
                                deviceOpen.channels[3].Data[len[4]] = mGlobal.ExchangeC2F(ref Val);
                            }
                            else
                            {
                                deviceOpen.channels[3].Data[len[4]] = Val;
                            }
                            len[4] += 1;
                            break;
                        case 4:
                            if (deviceOpen.channels[4].Unit == 1)
                            {
                                deviceOpen.channels[4].Data[len[5]] = mGlobal.ExchangeC2F(ref Val);
                            }
                            else
                            {
                                deviceOpen.channels[4].Data[len[5]] = Val;
                            }
                            len[5] += 1;
                            break;
                        case 5:
                            if (deviceOpen.channels[5].Unit == 1)
                            {
                                deviceOpen.channels[5].Data[len[6]] = mGlobal.ExchangeC2F(ref Val);
                            }
                            else
                            {
                                deviceOpen.channels[5].Data[len[6]] = Val;
                            }
                            len[6] += 1;
                            break;
                        case 6:
                            if (deviceOpen.channels[6].Unit == 1)
                            {
                                deviceOpen.channels[6].Data[len[7]] = mGlobal.ExchangeC2F(ref Val);
                            }
                            else
                            {
                                deviceOpen.channels[6].Data[len[7]] = Val;
                            }
                            len[7] += 1;
                            break;
                        case 7:
                            if (deviceOpen.channels[7].Unit == 1)
                            {
                                deviceOpen.channels[7].Data[len[8]] = mGlobal.ExchangeC2F(ref Val);
                            }
                            else
                            {
                                deviceOpen.channels[7].Data[len[8]] = Val;
                            }
                            len[8] += 1;
                            break;
                    }
                    ch += 1;
                    if (ch == deviceOpen.numOfChannel)
                    {
                        ch = 0;
                    }
                }
                for (int i = 0; i < deviceOpen.numOfChannel; i++)
                {
                    if (mGlobal.unitTemp[i] == 1)
                    {
                        mGlobal.unitTemp[i] = 2;
                    }
                }
            }

            for (int i = 0; i < deviceOpen.numOfChannel; i++)
            {
                for (int j = 0; j < deviceOpen.channels[0].Data.Length; j++)
                {
                    sum[i] = sum[i] + deviceOpen.channels[i].Data[j];
                }
            }

            try
            {
                for (int i = 0; i < deviceOpen.numOfChannel; i++)
                {
                    Random randomGen = new Random();
                    KnownColor[] names = (KnownColor[])Enum.GetValues(typeof(KnownColor));
                    KnownColor randomColorName = names[randomGen.Next(names.Length)];
                    Color randomColor = Color.FromKnownColor(randomColorName);
                    if (deviceOpen.channels[i].LineColor == null || deviceOpen.channels[i].LineColor == Color.Empty)
                    {
                        deviceOpen.channels[i].LineColor = randomColor;
                    }
                }
            }
            catch (Exception)
            { }

            // set Unit to show 'C or 'F
            for (int i = 0; i < deviceOpen.numOfChannel; i++)
            {
                if (mGlobal.C2F == true) // show 'C
                {
                    if (deviceOpen.channels[i].Unit == 2)
                    {
                        deviceOpen.channels[i].AlarmMax = (int)Math.Round((deviceOpen.channels[i].AlarmMax - 32)/1.8);
                        deviceOpen.channels[i].AlarmMin = (int)Math.Round((deviceOpen.channels[i].AlarmMin - 32)/1.8);
                        deviceOpen.channels[i].Unit = 1;
                    }
                }
                else // show 'F
                {
                    if (deviceOpen.channels[i].Unit == 1)
                    {
                        deviceOpen.channels[i].AlarmMax = (int)Math.Round((deviceOpen.channels[i].AlarmMax * 1.8) + 32);
                        deviceOpen.channels[i].AlarmMin = (int)Math.Round((deviceOpen.channels[i].AlarmMin  * 1.8) + 32);
                        deviceOpen.channels[i].Unit = 2;
                    }
                }
            }
            //xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
            mGlobal.readlog_opened = true;
        }


        //public delegate void toolBar(object se, FormClosedEventArgs e);
        //public toolBar visibale;

        private void DeviceFromFile_FormClosed(object sender, FormClosedEventArgs e)
        {
            //visibale(sender, e);
            mGlobal.activeDevice = true;
            deviceOpen = Device.DelInstance();
            device35 = Device35.DelInstance();
            mGlobal.tlb_eclapse = false;
        }

        private void chart1_AxisViewChanged(object sender, ViewEventArgs e)
        {
            //chart1.ChartAreas[0].AxisX.MajorGrid.Interval = chart1.ChartAreas[0].AxisX.LabelStyle.Interval * 5;
            chart1.ChartAreas[0].AxisX.MajorGrid.IntervalOffset = 0;
            //chart1.ChartAreas[1].AxisX.MajorGrid.IntervalOffset = 0;


            chart1.ChartAreas[0].AxisY2.ScaleView.Zoomable = true;
            chart1.ChartAreas[0].AxisY2.ScrollBar.Enabled = true;
            chart1.ChartAreas[0].AxisY2.ScrollBar.IsPositionedInside = true;
            chart1.ChartAreas[0].AxisY2.MajorGrid.Enabled = false;
            chart1.ChartAreas[0].AxisY2.MinorGrid.Enabled = false;

            chart1.ChartAreas[0].AxisY2.Interval = (chart1.ChartAreas[0].AxisY2.Maximum - chart1.ChartAreas[0].AxisY2.Minimum)*chart1.ChartAreas[0].AxisY.Interval/(chart1.ChartAreas[0].AxisY.Maximum - chart1.ChartAreas[0].AxisY.Minimum);
            //chart1.ChartAreas[0].AxisX.LabelAutoFitStyle = LabelAutoFitStyles.StaggeredLabels;
            //chart1.ChartAreas[0].AxisX.IsLabelAutoFit = true;
            //chart1.ChartAreas[0].AxisX.IntervalAutoMode = IntervalAutoMode.VariableCount;
        }


        public void set_graph35(byte[] record)
        {
            double[] sum = new double[20];

            int[] len = new int[20];

            if (mGlobal.open_file == false)
            {
                int[] temp_unit = new int[deviceOpen.numOfChannel];
                mGlobal.get_unit35(ref temp_unit, record);
                for (int i = 0; i < 4; i++)
                {
                    deviceOpen.channels[i].Unit = Convert.ToByte(temp_unit[i]);
                }
            }

            mGlobal._get_timezone_date(ref _timezone, ref deviceOpen._logger_date, record);
            _starttime = deviceOpen._logger_date.ToString("MM/dd/yyyy HH:mm:ss");

            int sec1 = 0;
            int min2 = 0;

            if (Convert.ToInt32(deviceOpen.Duration) > 60)
            {
                min2 = Convert.ToInt32(deviceOpen.Duration) / 60;
                sec1 = Convert.ToInt32(deviceOpen.Duration) % 60;
                deviceOpen.Interval = min2 + " Min " + sec1 + " Sec";
            }
            else
            {
                deviceOpen.Interval = Convert.ToInt32(deviceOpen.Duration) + " Sec";
            }

            //num of measure
            //mGlobal.num_measure_suminfo = (record.Length - offset) / 16;

            //stoptime


            for (int i = 0; i < 4; i++)
            {
                //deviceOpen.Channels[i].Data = new float[(record.Length - offset) / 16];
                deviceOpen.channels[i].Data = new float[deviceOpen.channels[i].DataLenght];
            }
            float Val = 0F;
            int minDataLenght = deviceOpen.channels[0].DataLenght;
            for (int i = 0; i < 4; i++)
			{
			    if(deviceOpen.channels[i].DataLenght < minDataLenght)
                {
                    minDataLenght = deviceOpen.channels[i].DataLenght;
                }
			}

            

            if (mGlobal.C2F == true) //show C degree
            {
                for (int i = 0; i < 4; i++)
                {
                    int dataLenght = 0;
                    int tmpDivNum = 0;
                    if(record[i + 30] == 1 || record[i+30] == 2)
                    {
                        tmpDivNum = 10;
                    }
                    else if(record[30 + i] == 3)
                    {
                        dataLenght = minDataLenght * 3;
                    }
                    else
                    {
                        tmpDivNum = 1;
                        dataLenght = minDataLenght;
                    }

                    deviceOpen.channels[i].Sensor = record[30 + i];
                    deviceOpen.channels[i].Unit = record[34 + i];
                    for (int j = 0; j < dataLenght; j++)
                    {
                        Val = mGlobal.get_temp(record[j + 1], record[j]) / tmpDivNum;
                        deviceOpen.channels[i].Data[i] = mGlobal.ExchangeC2F(ref Val);
                    }
                }

                for (int i = 0; i < deviceOpen.numOfChannel; i++)
                {
                    if (mGlobal.unitTemp[i] == 175)
                    {
                        mGlobal.unitTemp[i] = 172;
                    }
                }
            }
            else //show F degree
            {
                for (int i = 0; i < 4; i++)
                {
                    int dataLenght = 0;
                    int tmpDivNum = 0;
                    if (record[i + 30] == 1 || record[i + 30] == 2)
                    {
                        tmpDivNum = 10;
                    }
                    else if (record[30 + i] == 3)
                    {
                        dataLenght = minDataLenght * 3;
                    }
                    else
                    {
                        tmpDivNum = 1;
                        dataLenght = minDataLenght;
                    }


                    deviceOpen.channels[i].Sensor = record[30 + i];
                    deviceOpen.channels[i].Unit = record[34 + i];
                    for (int j = 0; j < dataLenght; j++)
                    {
                        Val = mGlobal.get_temp(record[j + 1], record[j]) / tmpDivNum;
                        deviceOpen.channels[i].Data[i] = mGlobal.ExchangeF2C(ref Val);
                    }
                }

                for (int i = 0; i < 4; i++)
                {
                    if (mGlobal.unitTemp[i] == 172)
                    {
                        mGlobal.unitTemp[i] = 175;
                    }
                }
            }

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < deviceOpen.channels[0].Data.Length; j++)
                {
                    sum[i] = sum[i] + deviceOpen.channels[i].Data[j];
                }
            }

            try
            {
                for (int i = 0; i < 4; i++)
                {
                    if(record[30 + i] == 3)
                    {
                        if(deviceOpen.channels[i].LineColor == Color.Empty)
                        {

                        }
                        if(deviceOpen.channels[i].LineColorY == Color.Empty)
                        {

                        }
                        if(deviceOpen.channels[i].LineColorZ == Color.Empty)
                        {

                        }
                    }
                    if(deviceOpen.channels[i].LineColor == Color.Empty)
                    {
                         // nhu vay set trung het rui. -> ko on
                    }
                }
                if (deviceOpen.channels[0].LineColor == null || deviceOpen.channels[0].LineColor == Color.Empty) deviceOpen.channels[0].LineColor = Color.Red;
                if (deviceOpen.channels[1].LineColor == null || deviceOpen.channels[1].LineColor == Color.Empty) deviceOpen.channels[1].LineColor = Color.Black;
                if (deviceOpen.channels[2].LineColor == null || deviceOpen.channels[2].LineColor == Color.Empty) deviceOpen.channels[2].LineColor = Color.Yellow;
                if (deviceOpen.channels[3].LineColor == null || deviceOpen.channels[3].LineColor == Color.Empty) deviceOpen.channels[3].LineColor = Color.Green;
            }
            catch (Exception)
            { }//set color line in chart

            // set Unit to show 'C or 'F
            for (int i = 0; i < 4; i++)
            {
                if (mGlobal.C2F == true) // show 'C
                {
                    if (deviceOpen.channels[i].Unit == 175)
                    {
                        deviceOpen.channels[i].Unit = 172;
                    }
                }
                else // show 'F
                {
                    if (deviceOpen.channels[i].Unit == 172)
                    {
                        deviceOpen.channels[i].Unit = 175;
                    }
                }
            }
            //xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
            mGlobal.readlog_opened = true;
        }

        public void set_graph35_2(byte[] record)
        {
            //int offset = 0;
            double[] sum = new double[20];

            int[] len = new int[20];

            int numSeries = 0;

            for (int i = 0; i < 4; i++)
            {
                if (record[i + 30] == 3)
                {
                    numSeries += 3;
                }
                else if (record[i + 30] == 0)
                {
                    numSeries += 1;
                }
                else
                {
                    numSeries += 1;
                }
            }

            if(numSeries <= 4)
            {
                numSeries = 4;
            }

            device35.Channels = new Channel[numSeries];
            device35.numOfChannel = numSeries;


            //tim min lenght Data (not vibr_or vibr if all of Channels are vibr)
            int dem = 0;
            int minDataLenght = (record[38] * 256 * 256 + record[39] * 256 + record[40]); //device35.Channels[0].DataLenght;
            int du = 0;

            for (int i = 0; i < 4; i++)
            {
                if ((record[38 + i * 3] * 256 * 256 + record[38 + 3 * i + 1] * 256 + record[38 + 3 * i + 2]) < minDataLenght && (record[38 + i * 3] * 256 * 256 + record[38 + 3 * i + 1] * 256 + record[38 + 3 * i + 2]) != 0 || minDataLenght == 0)
                {
                    minDataLenght = (record[38 + i * 3] * 256 * 256 + record[38 + 3 * i + 1] * 256 + record[38 + 3 * i + 2]);
                    dem = i;
                }
            }

            if (record[30 + dem] != 3)
            {
                minDataLenght = minDataLenght;
            }
            else
            {
                du = minDataLenght % 3;
                minDataLenght = minDataLenght / 3;
            } 
            

            if (mGlobal.open_file == false)
            {
                //offset = 65;

                int[] temp_unit = new int[device35.numOfChannel];
                mGlobal.get_unit35(ref temp_unit, record);

                int tamp = 0;
                for (int k = 0; k < 4; k++)
                {
                    if (record[k + 30] == 3)
                    {
                        device35.Channels[tamp] = new Channel();
                        device35.Channels[tamp].Unit = Convert.ToByte(temp_unit[k]);

                        device35.Channels[tamp].DataLenght = (record[38 + k * 3] * 256 * 256 + record[38 + 3 * k + 1] * 256 + record[38 + 3 * k + 2]) / 3;
                       
                        //device35.Channels[tamp].DataLenght = minDataLenght;
                        if (record[50 + k] == 0xf0)
                        {
                            device35.Channels[tamp].NoAlarm = true;
                        }
                        else
                        {
                            device35.Channels[tamp].NoAlarm = false;
                        }
                        device35.Channels[tamp].AlarmMax = record[2 * k + 54] * 256 + record[2 * k + 54 + 1];
                        device35.Channels[tamp].AlarmMin = record[2 * k + 62] * 256 + record[2 * k + 62 + 1];

                        device35.Channels[tamp + 1] = new Channel();
                        device35.Channels[tamp + 1].Unit = Convert.ToByte(temp_unit[k]);

                        device35.Channels[tamp + 1].DataLenght = (record[38 + k * 3] * 256 * 256 + record[38 + 3 * k + 1] * 256 + record[38 + 3 * k + 2]) / 3;
                       
                        //device35.Channels[tamp + 1].DataLenght = minDataLenght;
                        if (record[50 + k] == 0xf0)
                        {
                            device35.Channels[tamp + 1].NoAlarm = true;
                        }
                        else
                        {
                            device35.Channels[tamp + 1].NoAlarm = false;
                        }
                        device35.Channels[tamp + 1].AlarmMax = record[2 * k + 54] * 256 + record[2 * k + 54 + 1];
                        device35.Channels[tamp + 1].AlarmMin = record[2 * k + 62] * 256 + record[2 * k + 62 + 1];

                        device35.Channels[tamp + 2] = new Channel();
                        device35.Channels[tamp + 2].Unit = Convert.ToByte(temp_unit[k]);

                        device35.Channels[tamp + 2].DataLenght = (record[38 + k * 3] * 256 * 256 + record[38 + 3 * k + 1] * 256 + record[38 + 3 * k + 2]) / 3;
                       
                        //device35.Channels[tamp + 2].DataLenght = minDataLenght;
                        if (record[50 + k] == 0xf0)
                        {
                            device35.Channels[tamp + 2].NoAlarm = true;
                        }
                        else
                        {
                            device35.Channels[tamp + 2].NoAlarm = false;
                        }
                        device35.Channels[tamp + 2].AlarmMax = record[2 * k + 54] * 256 + record[2 * k + 54 + 1];
                        device35.Channels[tamp + 2].AlarmMin = record[2 * k + 62] * 256 + record[2 * k + 62 + 1];
                        tamp += 3;

                        du = (record[38 + k * 3] * 256 * 256 + record[38 + 3 * k + 1] * 256 + record[38 + 3 * k + 2]) % 3;
                    }
                    else if(record[k + 30] == 0)
                    {
                        device35.Channels[tamp] = new Channel();
                        device35.Channels[tamp].Unit = Convert.ToByte(temp_unit[k]);
                        tamp += 1;
                    }
                    else
                    {
                        device35.Channels[tamp] = new Channel();
                        device35.Channels[tamp].Unit = Convert.ToByte(temp_unit[k]);
                        device35.Channels[tamp].DataLenght = (record[38 + k * 3] * 256 * 256 + record[38 + 3 * k + 1] * 256 + record[38 + 3 * k + 2]);
                        //device35.Channels[tamp].DataLenght = minDataLenght;

                        if (record[50 + k] == 0xf0)
                        {
                            device35.Channels[tamp].NoAlarm = true;
                        }
                        else
                        {
                            device35.Channels[tamp].NoAlarm = false;
                        }
                        device35.Channels[tamp].AlarmMax = record[2 * k + 54] * 256 + record[2 * k + 54 + 1];
                        device35.Channels[tamp].AlarmMin = record[2 * k + 62] * 256 + record[2 * k + 62 + 1];

                        tamp += 1;
                    }
                    //if (tamp >= numSeries)
                    //    break;
                }
            }

            mGlobal._get_timezone_date35(ref _timezone, ref device35._logger_date, record);
            _starttime = device35._logger_date.ToString("HH:mm:ss") + " " + device35._logger_date.ToShortDateString();

            int sec1 = 0;
            int min2 = 0;

            int dur = mGlobal.duration(device35.Duration);
            if (dur > 60)
            {
                min2 = dur / 60;
                sec1 = dur % 60;
                device35.Interval = min2 + " Min " + sec1 + " Sec";
            }
            else
            {
                device35.Interval = dur + " Sec";
            }

            float Val = 0F;

            
            ////tim min lenght Data (not vibr_or vibr if all of Channels are vibr)
            //int dem = 0;
            //int minDataLenght = (record[38]*256 + record[39]); //device35.Channels[0].DataLenght;

            //for (int i = 0; i < 4; i++)
            //{
            //    if ((record[38 + i * 2] * 256 + record[38 + 2 * i + 1]) < minDataLenght && (record[38 + i * 2] * 256 + record[38 + 2 * i + 1]) != 0 || minDataLenght == 0)
            //    {
            //        minDataLenght = (record[38 + i * 2] * 256 + record[38 + 2 * i + 1]);
            //        dem = i;
            //    }
            //}

            //if (record[30 + dem] != 3)
            //{
            //    minDataLenght = minDataLenght;
            //}
            //else
            //{
            //    minDataLenght = minDataLenght / 3;
            //} 

            //for (int i = 0; i < numSeries; i++)
            //{

            //    device35.Channels[i].Data = new float[minDataLenght];
            //}

            if (mGlobal.C2F == true) //show C degree
            {
                int countSeries = 0;
                int countByteRead = 70;
                for (int i = 0; i < 4; i++)
                {
                    //if (countSeries > numSeries - 1)
                    //    break;
                    int tmpDivNum = 0;
                    //if (record[i + 30] == 1 || record[i + 30] == 2)
                    //{
                    //    tmpDivNum = 10;
                    //}
                    //else if(record[i + 30] == 3)
                    //{
                    //    tmpDivNum = 1000;
                    //}
                    //else
                    //{
                    //    tmpDivNum = 1;
                    //}


                    //add Data
                    if (device35.Channels[countSeries].Unit == 3)
                    {
                        tmpDivNum = 1000;
                        for (int k = 0; k < 3; k++)
                        {
                            device35.Channels[countSeries].Data = new float[minDataLenght];
                           // for (int j = 0; j < 2 * device35.Channels[countSeries].DataLenght; j+=2)
                            for (int j = 0; j < 2 * minDataLenght; j += 2)
                            {
                                Val = mGlobal.get_temp(record[(countByteRead + 2 * k) + (3 * j)], record[countByteRead + (2 * k) + 3 * j + 1]) / tmpDivNum;
                               
                                device35.Channels[countSeries].Data[j/2] = Val;
                            }
                            countSeries += 1;
                        }
                        countByteRead += (3 * device35.Channels[countSeries - 1].DataLenght + du) * 2;
                    }
                    else if(device35.Channels[countSeries].Unit == 0)
                    {
                        device35.Channels[countSeries].Data = new float[0];
                        countSeries += 1;
                        countByteRead += 0;
                    }
                    else
                    {
                        if (device35.Channels[countSeries].Unit == 175 || device35.Channels[countSeries].Unit == 172 || device35.Channels[countSeries].Unit == 2)
                        {
                            tmpDivNum = 10;
                        }
                        else
                        {
                            tmpDivNum = 1;
                        }
                        device35.Channels[countSeries].Data = new float[minDataLenght];
                        //for (int j = 0; j < 2 * device35.Channels[countSeries].DataLenght; j+=2)
                        for (int j = 0; j < 2 * minDataLenght; j += 2)
                        {
                            Val = mGlobal.get_temp(record[countByteRead + j], record[countByteRead + 1 + j]) / tmpDivNum;
                            if (device35.Channels[countSeries].Unit == 175)
                            {
                                device35.Channels[countSeries].Data[j / 2] = mGlobal.ExchangeF2C(ref Val);
                            }
                            device35.Channels[countSeries].Data[j / 2] = Val;
                        }
                        countSeries += 1;
                        countByteRead += device35.Channels[countSeries- 1].DataLenght * 2;
                    }
                    //if (countSeries >= numSeries)
                    //    break;
                }

                device35.numOfChannel = countSeries;
                for (int i = 0; i < device35.numOfChannel; i++)
                {
                    if (mGlobal.unitTemp[i] == 175)
                    {
                        mGlobal.unitTemp[i] = 172;
                    }
                }
            }

            else //show F degree
            {
                int countSeries = 0;
                int countByteRead = 64;
                for (int i = 0; i < 4; i++)
                {
                    if (countSeries > numSeries - 1)
                        break;
                    int tmpDivNum = 0;
                    if (record[i + 30] == 1 || record[i + 30] == 2 || record[i + 30] == 3)
                    {
                        tmpDivNum = 10;
                    }
                    else if (record[i + 30] == 3)
                    {
                        tmpDivNum = 1000;
                    }
                    else
                    {
                        tmpDivNum = 1;
                    }

                    //add Data
                    if (device35.Channels[countSeries].Unit == 3)
                    {
                        for (int k = 0; k < 3; k++)
                        {
                            device35.Channels[countSeries].Data = new float[minDataLenght];
                            //for (int j = 0; j < 2 * device35.Channels[countSeries].DataLenght; j+=2)
                            for (int j = 0; j < 2 * minDataLenght; j += 2)
                            {
                                Val = mGlobal.get_temp(record[countByteRead + (2 * k) + (3 * j)], record[countByteRead + (2 * k) + (3 * j) + 1]) / tmpDivNum;
                                //if (record[(countByteRead + 2 * k) + (3 * j)] == 255)
                                //{
                                //    Val = Val - 65536;
                                //}
                                device35.Channels[countSeries].Data[j/2] =  Val;
                            }
                            countSeries += 1;
                        }
                        countByteRead += 3 * device35.Channels[countSeries -1].DataLenght * 2;
                    }
                    else if(device35.Channels[countSeries].Unit == 0)
                    {
                        device35.Channels[countSeries].Data = new float[0];
                        countSeries += 1;
                        countByteRead += 0;
                    }
                    else
                    {
                        device35.Channels[countSeries].Data = new float[minDataLenght];
                        //for (int j = 0; j < 2 * device35.Channels[countSeries].DataLenght; j+=2)
                        for (int j = 0; j < 2 * minDataLenght; j += 2)
                        {
                            Val = mGlobal.get_temp(record[countByteRead + j], record[countByteRead + 1 + j]) / tmpDivNum;
                            //Val = (record[countByteRead + j] * 10 + record[countByteRead + 1 + j]) / tmpDivNum;
                            if (device35.Channels[countSeries].Unit == 172)
                            {
                                device35.Channels[countSeries].Data[j/2] = mGlobal.ExchangeC2F(ref Val);
                            }
                            else
                            {
                                device35.Channels[countSeries].Data[j/2] = Val;
                            }
                        }
                        countSeries += 1;
                        countByteRead += device35.Channels[countSeries - 1].DataLenght * 2;
                    }

                    if (countSeries >= numSeries)
                        break;
                }

                for (int i = 0; i < device35.numOfChannel; i++)
                {
                    if (mGlobal.unitTemp[i] == 172)
                    {
                        mGlobal.unitTemp[i] = 175;
                    }
                }
            }

            //set sum
            for (int i = 0; i < device35.numOfChannel; i++)
            {
                for (int j = 0; j < device35.Channels[i].Data.Length; j++)
                {
                    sum[i] = sum[i] + device35.Channels[i].Data[j];
                }
            }

            try
            {
                for (int i = 0; i < numSeries; i++)
                {
                    Random randomGen = new Random();
                    KnownColor[] names = (KnownColor[]) Enum.GetValues(typeof(KnownColor));
                    KnownColor randomColorName = names[randomGen.Next(names.Length)];
                    Color randomColor = Color.FromKnownColor(randomColorName);
                    if (device35.Channels[i].LineColor == null || device35.Channels[i].LineColor == Color.Empty)
                    {
                        device35.Channels[i].LineColor = randomColor;
                    }
                }
            }
            catch (Exception)
            { }

            // set Unit to show 'C or 'F
            for (int i = 0; i < numSeries; i++)
            {
                if (mGlobal.C2F == true) // show 'C
                {
                    if (device35.Channels[i].Unit == 175)
                    {
                        int tmp = device35.Channels[i].AlarmMax;
                        device35.Channels[i].AlarmMax = Int32.Parse(Math.Round((tmp - 320) / 1.8, 0).ToString());
                        int tmp2 = device35.Channels[i].AlarmMin;
                        device35.Channels[i].AlarmMin = Int32.Parse(Math.Round((tmp2 - 320) / 1.8, 0).ToString());
                        device35.Channels[i].Unit = 172;
                    }
                }
                else // show 'F
                {
                    if (device35.Channels[i].Unit == 172)
                    {
                        int tmp = device35.Channels[i].AlarmMax;
                        device35.Channels[i].AlarmMax = (int)(tmp * 1.8) + 320;
                        int tmp2 = device35.Channels[i].AlarmMin;
                        device35.Channels[i].AlarmMin = (int)(tmp2 * 1.8) + 320;
                        device35.Channels[i].Unit = 175;
                    }
                }
            }
            //xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
            mGlobal.readlog_opened = true;
        }

        #region draw_graph35
        #region draw_graph35
        public void draw_graph35()
        {
            DateTime GraphMinTime, GraphMaxTime;
            mGlobal.drawGraph35 = true;
            float[] sum = new float[device35.numOfChannel];

            int[] len = new int[12];
            Single GraphMaxLeft, GrapMinLeft;
            Single GrapMaxRight, GrapMinRight;

            Single GraphMaxLeft2, GrapMinLeft2;

            if (!mGlobal.open_file)
            {
                device35.offset = 128;//theo file luu

                int[] temp_unit = new int[device35.numOfChannel];
                mGlobal.get_unit35(ref temp_unit, device35.data_open);
                for (int i = 0; i < device35.numOfChannel; i++)
                {
                    device35.Channels[i].Unit = byte.Parse(temp_unit[i].ToString());
                }

                mGlobal.get_duration(device35.Duration, device35.data_open);
                mGlobal.get_delay(device35.Delay, device35.data_open);
            } //open file(not use)

            StartTime = device35._logger_date;
            //_starttime = device35._logger_date.ToString("MM/dd/yyyy HH:mm:ss");
            _starttime = string.Format("{0}  {1:HH:mm:ss}", device35._logger_date.ToShortDateString(), device35._logger_date);
            int sec1;
            int min2;

            //int tg = mGlobal.Duration(device35.Duration);//06/04/2015: Duration nhan dc la ngay?
            //if (tg > 60)
            //{
            //    min2 = tg / 60;
            //    sec1 = tg % 60;
            //    device35.Interval = min2 + " Min " + sec1 + " Sec";
            //}
            //else
            //{
            //    device35.Interval = tg + " Sec";
            //}

            mGlobal.numChan = 0;
            for (int i = 0; i < device35.numOfChannel; i++)
            {
                if (device35.Channels[i].Unit == 0)
                {
                    mGlobal.numChan += 0;
                }
                //else if (device35.Channels[i].Unit == 3)
                //{
                //    mGlobal.numChan += 3;
                //}
                else
                {
                    mGlobal.numChan += 1;
                }
            }
            int sec = 0;
            int min = 0;
            int tg = 0;
            int x = (127 * 1024) / mGlobal.numChan;
            tg = mGlobal.duration35(Convert.ToInt32(device35.Duration), x);
            //tg = mGlobal.Duration(Convert.ToInt32(cbbDuration.Text));
            if (tg > 60)
            {
                min = tg / 60;
                sec = tg % 60;
                device35.Interval = string.Format("Sample Interval: {0} min {1} sec.", min, sec);
            }
            else
            {
                device35.Interval = string.Format("Sample Interval: {0} sec.", tg);
            }

            //BaseTime = device35._logger_date.AddMinutes(Convert.ToInt32(device35.Delay));
            BaseTime = device35._logger_date;

            mGlobal.num_measure_suminfo = (device35.data_open.Length - device35.offset) / (2 * device35.numOfChannel) - 1;
            //mGlobal.num_measure_suminfo = (device35.data_open.Length - device35.offset) / mGlobal.numChan;
            //_eclapsetime = mGlobal.Sec2Day((mGlobal.num_measure_suminfo) * Convert.ToInt32(device35.Duration));
            _eclapsetime = mGlobal.Sec2Day((mGlobal.num_measure_suminfo) * tg);//stop Time

            string temp = _eclapsetime;

            string d = temp.Substring(0, temp.IndexOf(" "));
            temp = temp.Substring((temp.IndexOf(":") + 1 - 2) - 1);
            string h = temp.Substring((temp.IndexOf(":") + 1 - 2) - 1, 2);
            temp = temp.Substring(temp.IndexOf(":") + 1);
            string m = temp.Substring(0, temp.IndexOf(":"));
            temp = temp.Substring(temp.IndexOf(":") + 1);
            string s = temp;

            _stoptime = string.Format("{0}  {1:HH:mm:ss}", device35._logger_date.AddDays(double.Parse(d)).ToShortDateString(), device35._logger_date.AddHours(double.Parse(h)).AddMinutes(double.Parse(deviceOpen.Delay.ToString()) + double.Parse(m)).AddSeconds(double.Parse(s)));
            StopTime = device35._logger_date.AddDays(double.Parse(d)).AddHours(double.Parse(h)).AddMinutes(double.Parse(deviceOpen.Delay.ToString()) + double.Parse(m)).AddSeconds(double.Parse(s));

            for (int i = 0; i < device35.numOfChannel; i++)
            {
                device35.Channels[i].Data = new float[(device35.data_open.Length - device35.offset) / (device35.numOfChannel *2)]; //+1];
            }

            TimeSpan a = StopTime - StartTime;
            double secElapse = a.TotalSeconds;

            byte ch = 0;
            float val = 0;

            if (mGlobal.C2F)//C degree
            {
                for (int i = device35.offset; i < device35.data_open.Length; i += 2)
                {
                    int tmpDiv;
                    if (device35.Channels[((i - (510 % (2 * device35.numOfChannel))) % (2 * device35.numOfChannel)) / 2].Unit == 175 || device35.Channels[((i - (510 % (2 * device35.numOfChannel))) % (2 * device35.numOfChannel)) / 2].Unit == 172 || device35.Channels[((i - (510 % (2 * device35.numOfChannel))) % (2 * device35.numOfChannel)) / 2].Unit == 2) // offsset = 500; phai tru 6 thi khi % 24 moi về 0; trong soft truoc là 128
                    {
                        tmpDiv = 10;
                    }
                    else if(device35.Channels[((i - (510 % (2 * device35.numOfChannel))) % (2 * device35.numOfChannel)) / 2].Unit == 3)
                    {
                        tmpDiv = 1000;
                    }
                    else
                    {
                        tmpDiv = 1;
                    }
                    if (i < device35.data_open.Length - 1)
                    {
                        val = mGlobal.get_temp(device35.data_open[i], device35.data_open[i + 1]) / tmpDiv;
                    }

                    switch (ch)
                    {
                        case 0:
                            if (device35.Channels[0].Unit == 175) //F
                            {
                                device35.Channels[0].Data[len[0]] = mGlobal.ExchangeF2C(ref val);
                            }
                            else
                                device35.Channels[0].Data[len[0]] = val;
                            len[0] += 1;
                            break;
                        case 1:
                            if (device35.Channels[1].Unit == 175)
                            {
                                device35.Channels[1].Data[len[1]] = mGlobal.ExchangeF2C(ref val);
                            }
                            else
                                device35.Channels[1].Data[len[1]] = val;
                            len[1] += 1;
                            break;
                        case 2:
                            if (device35.Channels[2].Unit == 175)
                            {
                                device35.Channels[2].Data[len[2]] = mGlobal.ExchangeF2C(ref val);
                            }
                            else
                                device35.Channels[2].Data[len[2]] = val;
                            len[2] += 1;
                            break;
                        case 3:
                            if (device35.Channels[3].Unit == 175)
                            {
                                device35.Channels[3].Data[len[3]] = mGlobal.ExchangeF2C(ref val);
                            }
                            else
                                device35.Channels[3].Data[len[3]] = val;
                            len[3] += 1;
                            break;
                        case 4:
                            if (device35.Channels[4].Unit == 175)
                            {
                                device35.Channels[4].Data[len[4]] = mGlobal.ExchangeF2C(ref val);
                            }
                            else
                                device35.Channels[4].Data[len[4]] = val;
                            len[4] += 1;
                            break;
                        case 5:
                            if (device35.Channels[5].Unit == 175)
                            {
                                device35.Channels[5].Data[len[5]] = mGlobal.ExchangeF2C(ref val);
                            }
                            else
                                device35.Channels[5].Data[len[5]] = val;
                            len[5] += 1;
                            break;
                        case 6:
                            if (device35.Channels[6].Unit == 175)
                            {
                                device35.Channels[6].Data[len[6]] = mGlobal.ExchangeF2C(ref val);
                            }
                            else
                                device35.Channels[6].Data[len[6]] = val;
                            len[6] += 1;
                            break;
                        case 7:
                            if (device35.Channels[7].Unit == 175)
                            {
                                device35.Channels[7].Data[len[7]] = mGlobal.ExchangeF2C(ref val);
                            }
                            else
                                device35.Channels[7].Data[len[7]] = val;
                            len[7] += 1;
                            break;
                        case 8:
                            if (device35.Channels[8].Unit == 175)
                            {
                                device35.Channels[8].Data[len[8]] = mGlobal.ExchangeF2C(ref val);
                            }
                            else
                                device35.Channels[8].Data[len[8]] = val;
                            len[8] += 1;
                            break;
                        case 9:
                            if (device35.Channels[9].Unit == 175)
                            {
                                device35.Channels[9].Data[len[9]] = mGlobal.ExchangeF2C(ref val);
                            }
                            else
                                device35.Channels[9].Data[len[9]] = val;
                            len[9] += 1;
                            break;
                        case 10:
                            if (device35.Channels[10].Unit == 175)
                            {
                                device35.Channels[10].Data[len[10]] = mGlobal.ExchangeF2C(ref val);
                            }
                            else
                                device35.Channels[10].Data[len[10]] = val;
                            len[10] += 1;
                            break;
                        case 11:
                            if (device35.Channels[11].Unit == 175)
                            {
                                device35.Channels[11].Data[len[11]] = mGlobal.ExchangeF2C(ref val);
                            }
                            else
                                device35.Channels[11].Data[len[11]] = val;
                            len[11] += 1;
                            break;
                    }
                    ch += 1;
                    if (ch == device35.numOfChannel) ch = 0;
                }
                for (int j = 0; j < device35.numOfChannel; j++)
                {
                    if (count == 0)
                    {
                        mGlobal.unitTemp[j] = device35.Channels[j].Unit;

                        if (mGlobal.unitTemp[j] == 175)
                        {
                            mGlobal.unitTemp[j] = 172;
                        }
                    }
                    else if(count == 1)
                    {
                        mGlobal.unitTemp[j] = device35.Channels[j].Unit;
                        if (mGlobal.unitTemp[j] == 175)
                        {
                            int tmp = device35.Channels[j].AlarmMax;
                            device35.Channels[j].AlarmMax = Int32.Parse(Math.Round((tmp - 320) / 1.8, 0).ToString());
                            int tmp2 = device35.Channels[j].AlarmMin;
                            device35.Channels[j].AlarmMin = Int32.Parse(Math.Round((tmp2 - 320) / 1.8, 0).ToString());
                            mGlobal.unitTemp[j] = 172;
                        }
                    }
                    else
                    {
                        if (mGlobal.unitTemp[j] == 175)
                        {
                            int tmp = device35.Channels[j].AlarmMax;
                            device35.Channels[j].AlarmMax = Int32.Parse(Math.Round((tmp - 320) / 1.8, 0).ToString());
                            int tmp2 = device35.Channels[j].AlarmMin;
                            device35.Channels[j].AlarmMin = Int32.Parse(Math.Round((tmp2 - 320) / 1.8, 0).ToString());
                            mGlobal.unitTemp[j] = 172;
                        }
                    }
                }
                count += 1;
            }
            else//F degree
            {
                for (int i = device35.offset; i < device35.data_open.Length; i += 2)
                {
                    int tmpDiv;
                    if (device35.Channels[((i - (510 % (2 * device35.numOfChannel))) % (2 * device35.numOfChannel)) / 2].Unit == 175 || device35.Channels[((i - (510 % (2 * device35.numOfChannel))) % (2 * device35.numOfChannel)) / 2].Unit == 172 || device35.Channels[((i - (510 % (2 * device35.numOfChannel))) % (2 * device35.numOfChannel)) / 2].Unit == 2) // offsset = 500; phai tru 6 thi khi % 24 moi về 0; trong soft truoc là 128
                    {
                        tmpDiv = 10;
                    }
                    else if (device35.Channels[((i - (510 % (2 * device35.numOfChannel))) % (2 * device35.numOfChannel)) / 2].Unit == 3)
                    {
                        tmpDiv = 1000;
                    }
                    else
                    {
                        tmpDiv = 1;
                    }
                    if (i < device35.data_open.Length - 1)
                    {
                        val = mGlobal.get_temp(device35.data_open[i], device35.data_open[i + 1]) / tmpDiv;
                        //if (device35.data_open[i] == 255)
                        //{
                        //    val = val - 65536;
                        //}
                    }

                    switch (ch)
                    {
                        case 0:
                            if (device35.Channels[0].Unit == 172)// C
                            {
                                device35.Channels[0].Data[len[0]] = mGlobal.ExchangeC2F(ref val);
                            }
                            //else if (device35.Channels[0].Unit == 0)
                            //{
                            //    ch += 1;
                            //    goto case 1;
                            //}
                            else
                                device35.Channels[0].Data[len[0]] = val;
                            len[0] += 1;
                            break;
                        case 1:
                            if (device35.Channels[1].Unit == 172)
                            {
                                device35.Channels[1].Data[len[1]] = mGlobal.ExchangeC2F(ref val);
                            }
                            else
                                device35.Channels[1].Data[len[1]] = val;
                            len[1] += 1;
                            break;
                        case 2:
                            if (device35.Channels[2].Unit == 172)
                            {
                                device35.Channels[2].Data[len[2]] = mGlobal.ExchangeC2F(ref val);
                            }
                            else
                                device35.Channels[2].Data[len[2]] = val;
                            len[2] += 1;
                            break;
                        case 3:
                            if (device35.Channels[3].Unit == 172)
                            {
                                device35.Channels[3].Data[len[3]] = mGlobal.ExchangeC2F(ref val);
                            }
                            else
                                device35.Channels[3].Data[len[3]] = val;
                            len[3] += 1;
                            break;
                        case 4:
                            if (device35.Channels[4].Unit == 172)
                            {
                                device35.Channels[4].Data[len[4]] = mGlobal.ExchangeC2F(ref val);
                            }
                            else
                                device35.Channels[4].Data[len[4]] = val;
                            len[4] += 1;
                            break;
                        case 5:
                            if (device35.Channels[5].Unit == 172)
                            {
                                device35.Channels[5].Data[len[5]] = mGlobal.ExchangeC2F(ref val);
                            }
                            else
                                device35.Channels[5].Data[len[5]] = val;
                            len[5] += 1;
                            break;
                        case 6:
                            if (device35.Channels[6].Unit == 172)
                            {
                                device35.Channels[6].Data[len[6]] = mGlobal.ExchangeC2F(ref val);
                            }
                            else
                                device35.Channels[6].Data[len[6]] = val;
                            len[6] += 1;
                            break;
                        case 7:
                            if (device35.Channels[7].Unit == 172)
                            {
                                device35.Channels[7].Data[len[7]] = mGlobal.ExchangeC2F(ref val);
                            }
                            else
                                device35.Channels[7].Data[len[7]] = val;
                            len[7] += 1;
                            break;
                        case 8:
                            if (device35.Channels[8].Unit == 172)
                            {
                                device35.Channels[8].Data[len[8]] = mGlobal.ExchangeC2F(ref val);
                            }
                            else
                                device35.Channels[8].Data[len[8]] = val;
                            len[8] += 1;
                            break;
                        case 9:
                            if (device35.Channels[9].Unit == 172)
                            {
                                device35.Channels[9].Data[len[9]] = mGlobal.ExchangeC2F(ref val);
                            }
                            else
                                device35.Channels[9].Data[len[9]] = val;
                            len[9] += 1;
                            break;
                        case 10:
                            if (device35.Channels[10].Unit == 172)
                            {
                                device35.Channels[10].Data[len[10]] = mGlobal.ExchangeC2F(ref val);
                            }
                            else
                                device35.Channels[10].Data[len[10]] = val;
                            len[10] += 1;
                            break;
                        case 11:
                            if (device35.Channels[11].Unit == 172)
                            {
                                device35.Channels[11].Data[len[11]] = mGlobal.ExchangeC2F(ref val);
                            }
                            else
                                device35.Channels[11].Data[len[11]] = val;
                            len[11] += 1;
                            break;
                    }
                    ch += 1;
                    if (ch == device35.numOfChannel) ch = 0;
                }

                
                for (int j = 0; j < device35.numOfChannel; j++)
                {
                    if (count == 0)
                    {
                        mGlobal.unitTemp[j] = device35.Channels[j].Unit;
                        if (mGlobal.unitTemp[j] == 172)
                        {
                            mGlobal.unitTemp[j] = 175;//Unit[5,6,7] = {ppm = 3; %RH = 4}
                        }
                    }
                    else if(count == 1)
                    {
                        mGlobal.unitTemp[j] = device35.Channels[j].Unit;
                        if (mGlobal.unitTemp[j] == 172)
                        {
                            int tmp = device35.Channels[j].AlarmMax;
                            device35.Channels[j].AlarmMax = (int)(tmp * 1.8) + 320;
                            int tmp2 = device35.Channels[j].AlarmMin;
                            device35.Channels[j].AlarmMin = (int)(tmp2 * 1.8) + 320;
                            mGlobal.unitTemp[j] = 175;//Unit[5,6,7] = {ppm = 3; %RH = 4}
                        }
                    }
                    else
                    {
                        if (mGlobal.unitTemp[j] == 172)
                        {
                            int tmp = device35.Channels[j].AlarmMax;
                            device35.Channels[j].AlarmMax = (int)(tmp * 1.8) + 320;
                            int tmp2 = device35.Channels[j].AlarmMin;
                            device35.Channels[j].AlarmMin = (int)(tmp2 * 1.8) + 320;
                            mGlobal.unitTemp[j] = 175;//Unit[5,6,7] = {ppm = 3; %RH = 4}
                        }
                    }
                }
                count += 1;
            }

            // Draw Graph

            chart1.Width = this.Width;
            chart1.Height = 600;
            chart1.Left = this.Width / 1362;

            chart1.ChartAreas.Clear();

            int c1 = 0;
            int numVib = 0;
            for (int i = 0; i < device35.numOfChannel; i++)
            {
                if (device35.Channels[i].Unit == 3)
                    numVib += 1;
                else if (device35.Channels[i].Unit != 0)
                {
                    c1 = 1;
                }
            }
            numVib = numVib / 3;

            if (c1 != 0)
            {
                ChartArea first = new ChartArea();
                ////first.AxisY2.Enabled = AxisEnabled.False;
                //first.AxisY.LineWidth = 2;
                ////first.AxisY2.LineWidth = 2;
                //first.AxisY.LineColor = Color.SeaGreen;
                ////first.AxisY2.LineColor = Color.SeaGreen;
                //first.BorderDashStyle = ChartDashStyle.Dash;

                //first.AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dot;
                //first.AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dot;
                ////first.AxisY2.MajorGrid.Enabled = true;
                ////first.AxisY2.MajorGrid.LineDashStyle = ChartDashStyle.Dot;
                //first.BorderDashStyle = ChartDashStyle.Solid;

                //// to zoom chart.
                //// Enable range selection and zooming end user interface
                //first.CursorX.IsUserEnabled = true;
                //first.CursorX.IsUserSelectionEnabled = true;
                //first.AxisX.ScaleView.Zoomable = true;
                //first.AxisX.ScrollBar.IsPositionedInside = true;

                //first.CursorY.IsUserEnabled = true;
                //first.CursorY.IsUserSelectionEnabled = true;
                //first.AxisY.ScaleView.Zoomable = true;
                //first.AxisY.ScrollBar.IsPositionedInside = true;

                ////first.AxisY2.ScaleView.ZoomReset(0);
                ////first.AxisY2.ScaleView.Zoomable = true;
                ////first.AxisY2.ScrollBar.IsPositionedInside = true;
                ////first.AxisY2.ScrollBar.Enabled = true;

                //first.CursorY.SetCursorPosition(double.NaN);
                //first.CursorX.LineWidth = 0;
                //first.CursorY.LineWidth = 0;
                //first.AxisX.LabelStyle.IntervalOffset = 0;
                //first.AxisX.IsStartedFromZero = true;

                //first.BackColor = Color.Ivory;

                ////first.AxisY2.Enabled = AxisEnabled.False;
                ////first.AxisY2.LabelStyle.Enabled = false;
                //first.AxisX.IntervalAutoMode = IntervalAutoMode.VariableCount;
                //first.AxisY.LabelStyle.Format = "00.0";

                //first.AxisX.ScaleView.Zoomable = true;
                //first.AxisY.ScaleView.Zoomable = true;
                //first.AxisX.IsLabelAutoFit = true;

                //// Set X axis automatic fitting style
                //first.AxisX.LabelAutoFitStyle = LabelAutoFitStyles.DecreaseFont | LabelAutoFitStyles.IncreaseFont | LabelAutoFitStyles.WordWrap;
                //first.AxisX.MinorGrid.Enabled = false;


                first.AxisY2.Title = "CO2.(PPM)";
                first.AxisY.TitleFont = new Font("Arial", 9.0F, FontStyle.Bold);
                first.AxisY2.TitleFont = new Font("Arial", 9.0F, FontStyle.Bold);
                chart1.Titles[0].Text = "sjvhsvjksvls";
                first.AxisY.LineWidth = 2;
                first.AxisY2.LineWidth = 2;
                first.AxisY.LineColor = Color.SeaGreen;
                first.AxisY2.LineColor = Color.SeaGreen;
                first.BorderDashStyle = ChartDashStyle.Dash;
                first.AxisY.MinorGrid.Enabled = false;
                first.AxisY2.MinorGrid.Enabled = false;
                first.AxisY2.MajorGrid.Enabled = true;
                first.AxisY2.IntervalAutoMode = IntervalAutoMode.FixedCount;

                first.AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dot;
                first.AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dot;
                first.AxisY2.MajorGrid.LineDashStyle = ChartDashStyle.Dot;
                first.BorderDashStyle = ChartDashStyle.Solid;
                first.AxisX.LabelStyle.Format = "HH:mm:ss";

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
                first.AxisX.IsLabelAutoFit = true;
                first.AxisX.LabelStyle.IntervalOffset = 0;
                first.AxisX.IsStartedFromZero = true;

                chart1.ChartAreas.Add(first);
            }

            for (int i = 1; i <= numVib; i++)
            {
                ChartArea many = new ChartArea();
                ////many.AxisY2.Enabled = AxisEnabled.False;
                ////many.AxisY2.LabelStyle.Enabled = false;


                //many.AxisY.LineWidth = 2;
                //many.AxisY.LineColor = Color.SeaGreen;
                //many.BorderDashStyle = ChartDashStyle.Dash;

                //many.AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dot;
                //many.AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dot;
                //many.BorderDashStyle = ChartDashStyle.Solid;

                ////to zoom chart.
                //// Enable range selection and zooming end user interface
                //many.CursorX.IsUserEnabled = true;
                //many.CursorX.IsUserSelectionEnabled = true;
                //many.AxisX.ScaleView.Zoomable = true;
                //many.AxisX.ScrollBar.IsPositionedInside = true;

                //many.CursorY.IsUserEnabled = true;
                //many.CursorY.IsUserSelectionEnabled = true;
                //many.AxisY.ScaleView.Zoomable = true;
                //many.AxisY.ScrollBar.IsPositionedInside = true;

                //many.AxisY.ScaleView.SmallScrollMinSizeType = DateTimeIntervalType.Milliseconds;
                //many.AxisY.LabelStyle.Format = "0.##";

                //many.AxisY.ScaleView.SmallScrollSize = 0.001;
                //many.AxisY.ScaleView.MinSize = 0.001;
                //many.AxisY.IsMarginVisible = true;
                //many.AxisY.ScrollBar.IsPositionedInside = true;
                //many.AxisY.ScaleView.Position = 0.001;
                //many.CursorY.Interval = 0.005;


                //many.CursorX.SetCursorPosition(double.NaN);
                //many.CursorY.SetCursorPosition(double.NaN);
                //many.CursorX.LineWidth = 0;
                //many.CursorY.LineWidth = 0;
                //many.AxisX.LabelStyle.IntervalOffset = 0;
                //many.AxisX.IsStartedFromZero = true;

                //many.BackColor = Color.Ivory;


                //many.ShadowOffset = 5;
                //many.BorderWidth = 2;
                //many.BorderColor = Color.CadetBlue;

                //many.AxisX.IntervalAutoMode = IntervalAutoMode.VariableCount;

                //many.AxisY.LabelStyle.Format = "0.00";

                //many.AxisX.IsLabelAutoFit = true;

                //// Set X axis automatic fitting style
                //many.AxisX.LabelAutoFitStyle = LabelAutoFitStyles.DecreaseFont | LabelAutoFitStyles.IncreaseFont | LabelAutoFitStyles.WordWrap;

                //many.AxisX.ScaleView.Zoomable = true;
                //many.AxisY.ScaleView.Zoomable = true;
                //many.AxisX.MinorGrid.Enabled = false;

                //many.AlignmentOrientation = AreaAlignmentOrientations.Vertical;
                //many.AlignWithChartArea = "area1";
                //many.AlignmentStyle = AreaAlignmentStyles.PlotPosition;

                many.AxisY2.Enabled = AxisEnabled.False;
                many.AxisY2.LabelStyle.Enabled = false;

                many.AxisY.Title = "Acceleration (G)" + i.ToString();
                many.AxisY.TitleFont = new Font("Arial", 9.0F, FontStyle.Bold);
                many.AxisY.LineWidth = 2;
                many.AxisY.LineColor = Color.SeaGreen;
                many.BorderDashStyle = ChartDashStyle.Dash;
                many.AxisY.MinorGrid.Enabled = false;
                many.AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dot;
                many.AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dot;
                many.BorderDashStyle = ChartDashStyle.Solid;
                many.AxisX.LabelStyle.Format = "HH:mm:ss";

                many.AxisY.ScaleView.SmallScrollMinSizeType = DateTimeIntervalType.Milliseconds;
                many.AxisY.LabelStyle.Format = "0.##";

                many.CursorX.IsUserEnabled = true;
                many.CursorX.IsUserSelectionEnabled = true;
                many.AxisX.ScaleView.Zoomable = true;
                many.AxisX.ScrollBar.IsPositionedInside = true;

                many.CursorY.IsUserEnabled = true;
                many.CursorY.IsUserSelectionEnabled = true;
                many.AxisY.ScaleView.Zoomable = true;
                many.AxisY.ScrollBar.IsPositionedInside = true;
                many.CursorY.Interval = 0.05;

                many.BackColor = Color.Ivory;
                many.AxisY.LabelStyle.Format = "0.00";

                many.AlignmentOrientation = AreaAlignmentOrientations.Vertical;
                many.AlignmentStyle = AreaAlignmentStyles.PlotPosition;
                chart1.ChartAreas.Add(many);

                //if (chart1.ChartAreas.Count > 1)
                //{
                //    many.AlignWithChartArea = chart1.ChartAreas[chart1.ChartAreas.Count - 2].Name;
                //}
            }

            chart1.Legends[0].BorderColor = Color.Gray;
            chart1.Legends[0].BorderWidth = 2;
            chart1.Legends[0].BorderDashStyle = ChartDashStyle.Solid;
            chart1.Legends[0].ShadowOffset = 2;

            chart1.Series.ResumeUpdates();
            chart1.Series.Invalidate();
            chart1.Series.SuspendUpdates();

            CheckBox[] chkArea = new CheckBox[chart1.ChartAreas.Count];
            chartArea = chart1.ChartAreas.Count;
            int tmpH = 0;
            chart1.Controls.Clear();
            for (int i = 0; i < chartArea; i++)
            {
                tmpH += 50;
                chkArea[i] = new CheckBox();
                chkArea[i].Name = "Check " + i;
                chkArea[i].Text = "Show Area " + (i + 1).ToString();
                chkArea[i].Font = new Font("Arial", 10.0F, FontStyle.Italic);
                chkArea[i].Left = Int32.Parse((chart1.Width - 160).ToString());
                chkArea[i].Top = 300 + tmpH;
                chkArea[i].Width = 150;
                chkArea[i].Visible = true;
                chkArea[i].Checked = true;
                chart1.Controls.Add(chkArea[i]);
                chkArea[i].CheckedChanged += new EventHandler(chkArea_CheckedChanged);
            }

            //set title chart

            if (mGlobal.TitleChanged)
            {
                chart1.Titles[0].Text = mGlobal.TitleGraph;
            }
            else
            {
                if (device35.titlegraph == "")
                {
                    device35.titlegraph = "Marathon Electronic Data Logger";
                }
                chart1.Titles[0].Text = device35.titlegraph;
            }

            ////set footer chart
            //string footer = "Desc: " + device35.Description + " - Loc: " + device35.Location + " " +
            //        "Time: " + secElapse.ToString() + ", " + StartTime.ToString()
            //        + " to " + StopTime.ToString() + ", " + _timezone.ToString();
            //chart1.Titles[1].Text = footer;
            //Title footertitle = chart1.Titles[1];
            //footertitle.Docking = Docking.Bottom;
            //footertitle.Alignment = ContentAlignment.BottomCenter;

            // Reset number of series in the Chart.
            chart1.Series.Clear();
            Series[] newSer = new Series[device35.numOfChannel];
            int channel = 1;
            int dem = 0;
            int countVIB = 0;
            int num = 0;
            for (int i = 0; i < device35.numOfChannel; i++)
            {
                if (device35.Channels[i].Unit == 3)
                {
                    switch (dem)
                    {
                        case 0:
                            //newSer[i] = new Series(string.Format(res_man.GetString("Chanel", cul) + " {0} X ({1})", (channel), mGlobal.IntToUnit35(byte.Parse(mGlobal.unitTemp[i].ToString()))));
                            newSer[i] = new Series(res_man.GetString("Chanel", cul) + " " + channel.ToString() + " X (" + mGlobal.IntToUnit35(byte.Parse(mGlobal.unitTemp[i].ToString())) + ")");
                            newSer[i].ChartType = SeriesChartType.FastLine;
                            newSer[i].BorderWidth = 2;
                            newSer[i].ChartArea = chart1.ChartAreas[countVIB - num  + c1].Name;
                            newSer[i].Points.SuspendUpdates();
                            break;
                        case 1:
                            //countVIB = countVIB - 1;
                            newSer[i] = new Series(string.Format(res_man.GetString("Chanel", cul) + " {0} Y ({1})", (channel), mGlobal.IntToUnit35(byte.Parse(mGlobal.unitTemp[i].ToString()))));
                            newSer[i].ChartType = SeriesChartType.FastLine;
                            newSer[i].BorderWidth = 2;
                            newSer[i].ChartArea = chart1.ChartAreas[countVIB - num + c1].Name;
                            newSer[i].Points.SuspendUpdates();
                           
                            break;
                        case 2:
                            //countVIB = countVIB - 2;
                            newSer[i] = new Series(string.Format(res_man.GetString("Chanel", cul) + " {0} Z ({1})", (channel), mGlobal.IntToUnit35(byte.Parse(mGlobal.unitTemp[i].ToString()))));
                            newSer[i].ChartType = SeriesChartType.FastLine;
                            newSer[i].BorderWidth = 2;
                            newSer[i].ChartArea = chart1.ChartAreas[countVIB - num + c1].Name;
                            newSer[i].Points.SuspendUpdates();
                            num -= 1;
                            channel += 1;
                            dem = -1;
                            break;
                    }
                    dem += 1;
                    countVIB += 1;
                    num += 1;
                }
                else
                {
                    newSer[i] = new Series(string.Format(res_man.GetString("Chanel", cul) + " {0} ({1})", (channel), mGlobal.IntToUnit35(byte.Parse(mGlobal.unitTemp[i].ToString()))));
                    newSer[i].ChartType = SeriesChartType.FastLine;
                    newSer[i].BorderWidth = 2;
                    newSer[i].ChartArea = chart1.ChartAreas[0].Name;
                    newSer[i].Points.SuspendUpdates();
                    channel += 1;
                    if (device35.Channels[i].Unit == 4)
                    {
                        newSer[i].YAxisType = AxisType.Secondary;
                        chart1.ChartAreas[0].AxisY2.Enabled = AxisEnabled.True;
                        chart1.ChartAreas[0].AxisY2.LabelStyle.Enabled = true;
                    }
                }
                
            }

            

            if (!mGlobal.tlb_eclapse)
            {
                TimeZoneInfo localZone;
                TimeZoneInfo TargetZone;
                mGlobal._get_timezone_date(ref writeTimeZone, ref device35._logger_date, device35.data_open);

                localZone = writeTimeZone;
                TargetZone = mGlobal.FindSystemTimeZoneFromDisplayName(_timezone.ToString());

                double OffsetHour, OffsetMin;
                try
                {
                    OffsetHour = Convert.ToDouble(_timezone.ToString().Substring(4, 3));
                    OffsetMin = Convert.ToDouble(_timezone.ToString().Substring(8, 2));
                }
                catch (Exception)
                {
                    OffsetHour = 0;
                    OffsetMin = 0;
                }

                DateTime theUTCTime;
                theUTCTime = TimeZoneInfo.ConvertTimeToUtc(StartTime, localZone);
                GraphMinTime = theUTCTime.AddHours(OffsetHour).AddMinutes(OffsetMin);

                theUTCTime = TimeZoneInfo.ConvertTimeToUtc(StopTime, localZone);
                GraphMaxTime = theUTCTime.AddHours(OffsetHour).AddMinutes(OffsetMin);

                ReadOnlyCollection<TimeZoneInfo> timeZones = TimeZoneInfo.GetSystemTimeZones();

                foreach (TimeZoneInfo timeZone in timeZones)
                {
                    if (timeZone.DisplayName == _timezone.ToString())
                    {
                        TimeZoneInfo.AdjustmentRule[] adjustments = timeZone.GetAdjustmentRules();

                        if (adjustments.Length == 0)
                        {
                            theUTCTime = TimeZoneInfo.ConvertTimeToUtc(StartTime, localZone);
                            GraphMinTime = theUTCTime.AddHours(OffsetHour).AddMinutes(OffsetMin);

                            theUTCTime = TimeZoneInfo.ConvertTimeToUtc(StopTime, localZone);
                            GraphMaxTime = theUTCTime.AddHours(OffsetHour).AddMinutes(OffsetMin);
                        }

                        foreach (TimeZoneInfo.AdjustmentRule daylight in adjustments)
                        {
                            if (timeZone.IsDaylightSavingTime(GraphMinTime))
                            {
                                theUTCTime = TimeZoneInfo.ConvertTimeToUtc(StartTime.AddHours(daylight.DaylightDelta.Hours).AddMinutes(daylight.DaylightDelta.Minutes).AddSeconds(daylight.DaylightDelta.Seconds), localZone);
                                GraphMinTime = theUTCTime.AddHours(OffsetHour).AddMinutes(OffsetMin);

                                theUTCTime = TimeZoneInfo.ConvertTimeToUtc(StopTime.AddHours(daylight.DaylightDelta.Hours).AddMinutes(daylight.DaylightDelta.Minutes).AddSeconds(daylight.DaylightDelta.Seconds), localZone);
                                GraphMaxTime = theUTCTime.AddHours(OffsetHour).AddMinutes(OffsetMin);
                            }
                            else
                            {
                                theUTCTime = TimeZoneInfo.ConvertTimeToUtc(StartTime, localZone);
                                GraphMinTime = theUTCTime.AddHours(OffsetHour).AddMinutes(OffsetMin);

                                theUTCTime = TimeZoneInfo.ConvertTimeToUtc(StopTime, localZone);
                                GraphMaxTime = theUTCTime.AddHours(OffsetHour).AddMinutes(OffsetMin);
                            }
                        }
                    }
                }

                //BaseTime = GraphMinTime.AddMinutes(Convert.ToInt32(device35.Delay));
                BaseTime = GraphMinTime;
                // Set X axis automatic fitting style
                chart1.AntiAliasing = AntiAliasingStyles.All;

                for (int j = 0; j < device35.numOfChannel; j++)
                {
                    for (int i = 0; i < device35.Channels[j].Data.Length; i++)
                    {
                        newSer[j].Points.AddXY(string.Format("{0:HH:mm:ss}\n{1}", BaseTime.AddSeconds(tg * i), BaseTime.AddSeconds(tg * i).ToShortDateString()), device35.Channels[j].Data[i]);

                        sum[j] += device35.Channels[j].Data[i];
                    }
                }


                //set footer chart
                string footer = "Desc: " + device35.Description + " - Loc: " + device35.Location + " " +
                        "Time: " + secElapse.ToString() + ", " + GraphMinTime.ToString()
                        + " to " + GraphMaxTime.ToString() + ", " + _timezone.ToString();
                chart1.Titles[1].Text = footer;
                Title footertitle = chart1.Titles[1];
                footertitle.Docking = Docking.Bottom;
                footertitle.Alignment = ContentAlignment.BottomCenter;
            }

           //Eclapse Time
            else //eclapse Time
            {
                Int32 day_no = 0;
                string str;
                BaseTime = new DateTime(2000, 1, 1, 0, 0, 0);
                GraphMinTime = BaseTime;
                BaseTime = GraphMinTime.AddMinutes(Convert.ToInt32(0));
                GraphMaxTime = BaseTime.AddSeconds(tg * (device35.Channels[0].Data.Length - 1));

                chart1.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
                chart1.ChartAreas[0].AxisY.ScaleView.Zoomable = true;
                chart1.ChartAreas[0].AxisX.MinorGrid.Enabled = false;
                chart1.ChartAreas[0].AxisX.LabelAutoFitStyle = LabelAutoFitStyles.IncreaseFont;
                chart1.ChartAreas[0].AxisX.IsLabelAutoFit = true;
                chart1.ChartAreas[0].AxisX.LabelStyle.Format = "dd:HH:mm:ss";

                if (chart1.ChartAreas.Count > 1)
                {
                    chart1.ChartAreas[1].AxisX.MinorGrid.Enabled = false;
                    chart1.ChartAreas[1].AxisX.ScaleView.Zoomable = true;
                    chart1.ChartAreas[1].AxisY.ScaleView.Zoomable = true;
                    chart1.ChartAreas[1].AxisX.IsLabelAutoFit = true;
                    chart1.ChartAreas[1].AxisX.LabelStyle.Format = "dd:HH:mm:ss";
                    chart1.ChartAreas[1].AxisX.LabelAutoFitStyle = LabelAutoFitStyles.IncreaseFont;
                }

                chart1.Series.ResumeUpdates();
                chart1.Series.Invalidate();
                chart1.Series.SuspendUpdates();

                for (int j = 0; j < device35.numOfChannel; j++)
                {
                    for (int i = 0; i < device35.Channels[j].Data.Length; i++)
                    {
                        day_no = BaseTime.AddSeconds(device35.Duration * i).DayOfYear - 1;
                        //str = Environment.NewLine + " day " + BaseTime.AddSeconds(device35.Duration * i).DayOfYear.ToString();

                        str = (BaseTime.AddSeconds(tg * i).Day - 1).ToString() + " days" + Environment.NewLine + BaseTime.AddSeconds(tg * i).ToString("HH:mm:ss");

                        Object XValue = BaseTime.AddSeconds(device35.Duration * i).ToString("HH:mm:ss");

                        newSer[j].Points.AddXY(str, device35.Channels[j].Data[i]);
                        //newSer[j].Points.AddXY(BaseTime.AddSeconds(tg * i).ToString("HH:mm:ss"), device35.Channels[j].Data[i]);
                        sum[j] += device35.Channels[j].Data[i];
                    }
                }

                //set footer chart
                string footer = "Desc: " + device35.Description + "  - Loc: " + device35.Location + " " +
                        "  Time: " + secElapse.ToString() + ",   " + (BaseTime.Day - 1).ToString() +":" + BaseTime.ToString("HH:mm:ss")
                        + "  to  " + (GraphMaxTime.Day - 1).ToString() + ":" + GraphMaxTime.ToString("HH:mm:ss") + ", " + _timezone.ToString();
                chart1.Titles[1].Text = footer;
                Title footertitle = chart1.Titles[1];
                footertitle.Docking = Docking.Bottom;
                footertitle.Alignment = ContentAlignment.BottomCenter;
            }

            ////set footer chart
            //string footer = "Desc: " + device35.Description + " - Loc: " + device35.Location + " " +
            //        "Time: " + secElapse.ToString() + ", " + GraphMinTime.ToString()
            //        + " to " + GraphMaxTime.ToString() + ", " + _timezone.ToString();
            //chart1.Titles[1].Text = footer;
            //Title footertitle = chart1.Titles[1];
            //footertitle.Docking = Docking.Bottom;
            //footertitle.Alignment = ContentAlignment.BottomCenter;



            _starttime = GraphMinTime.ToString("MM/dd/yyyy HH:mm:ss");
            _stoptime = GraphMaxTime.ToString("MM/dd/yyyy HH:mm:ss");

            float high1 = 0;
            float low1 = 0;

            float high2 = 0;
            float low2 = 0;

            for (int i = 0; i < device35.numOfChannel; i++)
            {
                if(device35.Channels[i].Unit == 3)
                {
                    high2 = mGlobal.find_max(device35.Channels[i].Data);
                    low2 = mGlobal.find_min(device35.Channels[i].Data);
                }
                else
                {
                    high1 = mGlobal.find_max(device35.Channels[i].Data);
                    low1 = mGlobal.find_min(device35.Channels[i].Data);
                }
                device35.Channels[i].high_suminfo = mGlobal.find_max(device35.Channels[i].Data);
                device35.Channels[i].low_suminfo = mGlobal.find_min(device35.Channels[i].Data);
            }

            GraphMaxLeft = -1000;
            GrapMaxRight = -1000;
            GrapMinLeft = 1000;
            GrapMinRight = 1000;

            GrapMinLeft2 = 1000;
            GraphMaxLeft2 = -1000;

            for (int i = 0; i < device35.numOfChannel; i++)
            {
                if (device35.Channels[i].Unit != 4 )
                {
                    if(device35.Channels[i].Unit == 3)
                    {
                        GraphMaxLeft2 = Math.Max(device35.Channels[i].high_suminfo, GraphMaxLeft2);
                        GrapMinLeft2 = Math.Min(device35.Channels[i].low_suminfo, GrapMinLeft2);
                    }
                    else
                    {
                        GraphMaxLeft = Math.Max(device35.Channels[i].high_suminfo, GraphMaxLeft);
                        GrapMinLeft = Math.Min(device35.Channels[i].low_suminfo, GrapMinLeft);
                    }
                }
                else
                {
                    GrapMaxRight = Math.Max(device35.Channels[i].high_suminfo, GrapMaxRight);
                    GrapMinRight = Math.Min(device35.Channels[i].low_suminfo, GrapMinRight);
                }
            }

            Color[] colors = new Color[12];
            colors[0] = Color.Red;
            colors[1] = Color.Green;
            colors[2] = Color.Cyan;
            colors[3] = Color.Blue;
            colors[4] = Color.Yellow;
            colors[5] = Color.Peru;
            colors[6] = Color.Brown;
            colors[7] = Color.MediumAquamarine;
            colors[8] = Color.Orange;
            colors[9] = Color.Salmon;
            colors[10] = Color.Teal;
            colors[11] = Color.Violet;

            for (int i = 0; i < device35.numOfChannel; i++)
            {
                device35.Channels[i].LineColor = colors[i];
            }

            for (int i = 0; i < device35.numOfChannel; i++)
            {
                if (mGlobal.C2F)//show C
                {
                    if (mGlobal.unitTemp[i] == 175)
                    {
                        mGlobal.unitTemp[i] = 172;
                    }
                }
                else//show F
                {
                    if (mGlobal.unitTemp[i] == 172)
                    {
                        mGlobal.unitTemp[i] = 175;
                    }
                }
            }

            for (int i = 0; i < device35.numOfChannel; i++)
            {
                device35.Channels[i].MaxCount = 0;
                device35.Channels[i].MinCount = 0;

                if (!device35.Channels[i].NoAlarm && device35.Channels[i].Unit != 0)
                {
                    for (int j = 0; j < device35.Channels[i].Data.Length - 1; j++)
                    {
                        if (Convert.ToSingle(device35.Channels[i].Data[j]) >= Convert.ToSingle(device35.Channels[i].AlarmMax) / 10)
                        {
                            device35.Channels[i].MaxCount = device35.Channels[i].MaxCount + 1;
                        }
                        else if (Convert.ToSingle(device35.Channels[i].Data[j]) <= Convert.ToSingle(device35.Channels[i].AlarmMin) / 10)
                        {
                            device35.Channels[i].MinCount = device35.Channels[i].MinCount + 1;
                        }
                    }
                    //device35.Channels[i].MaxCount -= 1;
                    //device35.Channels[i].MinCount -= 1;
                }
                device35.Channels[i].ave_frm_suminfo = Math.Round(sum[i] / ((device35.data_open.Length - device35.offset) / (2 * device35.numOfChannel)), 2);
            }

            bool Hum = false;
            for (int i = 0; i < device35.Channels.Count(); i++)
            {
                if (device35.Channels[i].Unit == 2)
                    Hum = true;
            }

            //title axis Y wiht series
            if (mGlobal.C2F)
            {
                if (c1 != 0)
                {
                    if (Hum)
                    {
                        chart1.ChartAreas[0].AxisY.Title = "Temp.(C) - Humid.(%)";
                    }
                    else
                    {
                        chart1.ChartAreas[0].AxisY.Title = "Temp.(C)";
                    }
                    chart1.ChartAreas[0].AxisY.TitleFont = new Font("Arial", 9.0F, FontStyle.Bold);
                    for (int i = 1; i < chart1.ChartAreas.Count; i++)
                    {
                        chart1.ChartAreas[i].AxisY.Title = "Acc.(G)";
                        chart1.ChartAreas[i].AxisY.TitleFont = new Font("Arial", 9.0F, FontStyle.Bold);
                    }
                }
                else
                {
                    for (int i = 0; i < chart1.ChartAreas.Count; i++)
                    {
                        chart1.ChartAreas[i].AxisY.Title = "Acc.(G)";
                        chart1.ChartAreas[i].AxisY.TitleFont = new Font("Arial", 9.0F, FontStyle.Bold);
                    }
                }
            }
            else
            {
                if (c1 != 0)
                {
                    if (Hum)
                    {
                        chart1.ChartAreas[0].AxisY.Title = "Temp.(F) - Humid.(%)";
                    }
                    else
                    {
                        chart1.ChartAreas[0].AxisY.Title = "Temp.(F)";
                    }
                    chart1.ChartAreas[0].AxisY.TitleFont = new Font("Arial", 9.0F, FontStyle.Bold);
                    //chart1.ChartAreas[0].AxisY2.Title = "PPM";
                    //chart1.ChartAreas[0].AxisY2.TitleFont = new Font("Arial", 9.0F, FontStyle.Bold);
                    for (int i = 1; i < chart1.ChartAreas.Count; i++)
                    {
                        chart1.ChartAreas[i].AxisY.Title = "Acc.(G)";
                        chart1.ChartAreas[i].AxisY.TitleFont = new Font("Arial", 9.0F, FontStyle.Bold);
                    }
                }
                else
                {
                    for (int i = 0; i < chart1.ChartAreas.Count; i++)
                    {
                        chart1.ChartAreas[i].AxisY.Title = "Acc.(G)";
                        chart1.ChartAreas[i].AxisY.TitleFont = new Font("Arial", 9.0F, FontStyle.Bold);
                    }
                }
            }


            //set min and max in AxisY, AxisY2
            double lmin = Math.Round(GrapMinLeft, 0);
            double lmax = Math.Round(GraphMaxLeft, 0);
            double rmin = Math.Round(GrapMinRight, 0);
            double rmax = Math.Round(GrapMaxRight, 0);

            double lmin2 = Math.Round(GrapMinLeft2, 0);
            double lmax2 = Math.Round(GraphMaxLeft2, 0);

            if (c1 != 0)
            {
                chart1.ChartAreas[0].AxisY.Maximum = lmax + 10;
                chart1.ChartAreas[0].AxisY.Minimum = lmin - 10;
                for (int i = 1; i < chart1.ChartAreas.Count; i++)
                {
                    chart1.ChartAreas[i].AxisY.Maximum = lmax2 + 1;
                    chart1.ChartAreas[i].AxisY.Minimum = lmin2 - 1;
                }
            }
            else
            {
                for (int i = 0; i < chart1.ChartAreas.Count; i++)
                {
                    chart1.ChartAreas[i].AxisY.Maximum = lmax2 + 1;
                    chart1.ChartAreas[i].AxisY.Minimum = lmin2 - 1;
                }
            }

            bool checkY2 = false;
            for (int i = 0; i < device35.numOfChannel; i++)
            {
                if (device35.Channels[i].Unit == 4)
                {
                    checkY2 = true;
                }
            }

            if (checkY2)
            {
                chart1.ChartAreas[0].AxisY2.Maximum = rmax + 100;
                chart1.ChartAreas[0].AxisY2.Minimum = 500;
            }
            //chart1.ChartAreas[0].AxisY2.Enabled = AxisEnabled.False;
            //chart1.ChartAreas[0].AxisY2.LabelStyle.Enabled = false;
           
            //Add series to chart
            for (int i = 0; i < device35.numOfChannel; i++)
            {
                if (device35.Channels[i].Unit != 0)
                {
                    if (device35.Channels[i].LineColor != null && device35.Channels[i].LineColor != Color.Empty && device35.Channels[i].LineColor.Name != "ff000000")
                    {
                        newSer[i].Color = device35.Channels[i].LineColor;
                    }
                    chart1.Series.Add(newSer[i]);
                }
            }
            for (int i = 0; i < chart1.Series.Count; i++)
            {
                if (mGlobal.tlb_value)
                {
                    //chart1.Series[i].LabelToolTip = chart1.Series[i].Name + "\r\n #VALX \r\n [#VALY{0.##}]";
                    chart1.Series[i].ToolTip = chart1.Series[i].Name + "\r\n #VALX \r\n [#VALY{0.000}]";
                }
            }            

          

            //chkShowArea1.Left = Int32.Parse((chart1.Width - 180).ToString());
            //chkShowArea2.Left = Int32.Parse((chart1.Width - 180).ToString());

            //chkShowArea1.Checked = true;
            //chkShowArea2.Checked = true;

            int demArea2 = 0;
            int demArea1 = 0;
            for (int i = 0; i < device35.numOfChannel; i++)
            {
                if(device35.Channels[i].Unit == 3)
                {
                    demArea2 += 1;
                }
                else if (device35.Channels[i].Unit == 175 || device35.Channels[i].Unit == 172 || device35.Channels[i].Unit == 2 || device35.Channels[i].Unit == 4)
                {
                    demArea1 += 1;
                }
            }

            //if(demArea2 == 0)
            //{
            //    chkShowArea2.Checked = false;
            //}
            //else if(demArea1 == 0)
            //{
            //    chkShowArea1.Checked = false;
            //}
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
        #endregion
        #endregion



        //private void chart1_SelectionRangeChanged(object sender, CursorEventArgs e)
        //{
        //    double startX, endX, startY, endY;

        //    if (chart1.ChartAreas[0].CursorX.SelectionStart > chart1.ChartAreas[0].CursorX.SelectionEnd)
        //    {
        //        startX = chart1.ChartAreas[0].CursorX.SelectionEnd;
        //        endX = chart1.ChartAreas[0].CursorX.SelectionStart;
        //    }
        //    else
        //    {
        //        startX = chart1.ChartAreas[0].CursorX.SelectionStart;
        //        endX = chart1.ChartAreas[0].CursorX.SelectionEnd;
        //    }
        //    if (chart1.ChartAreas[0].CursorY.SelectionStart > chart1.ChartAreas[0].CursorY.SelectionEnd)
        //    {
        //        endY = chart1.ChartAreas[0].CursorY.SelectionStart;
        //        startY = chart1.ChartAreas[0].CursorY.SelectionEnd;
        //    }
        //    else
        //    {
        //        startY = chart1.ChartAreas[0].CursorY.SelectionStart;
        //        endY = chart1.ChartAreas[0].CursorY.SelectionEnd;
        //    }

        //    if (startX == endX && startY == endY)
        //    {
        //        return;
        //    }

        //    chart1.ChartAreas[0].AxisX.ScaleView.Zoom(startX, (endX - startX), DateTimeIntervalType.Auto, true);
        //    chart1.ChartAreas[0].AxisY.ScaleView.Zoom(startY, (endY - startY), DateTimeIntervalType.Auto, true);

        //}

       

        private void chart1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Control && e.KeyCode == Keys.U)
            {
                if (mGlobal.drawGraph35)
                {
                    if (chart1.ChartAreas[0].AxisX.ScaleView.IsZoomed)
                    {
                        chart1.ChartAreas[0].AxisX.ScaleView.ZoomReset(1);
                        chart1.ChartAreas[0].AxisY.ScaleView.ZoomReset(1);
                    }
                    if (chart1.ChartAreas[1].AxisX.ScaleView.IsZoomed)
                    {
                        chart1.ChartAreas[1].AxisX.ScaleView.ZoomReset(1);
                        chart1.ChartAreas[1].AxisY.ScaleView.ZoomReset(1);
                    }
                }
                else
                {
                    if (chart1.ChartAreas[0].AxisX.ScaleView.IsZoomed)
                    {
                        chart1.ChartAreas[0].AxisX.ScaleView.ZoomReset(1);
                        chart1.ChartAreas[0].AxisY.ScaleView.ZoomReset(1);
                    }
                }
            }


            if (e.KeyCode == Keys.Right)
            {
                chart1.ChartAreas[1].AxisX.ScaleView.Scroll(ScrollType.SmallIncrement);
            }
            else if (e.KeyCode == Keys.Left)
            {
                chart1.ChartAreas[1].AxisX.ScaleView.Scroll(ScrollType.SmallDecrement);
            }
            else if (e.KeyCode == Keys.PageDown)
            {
                chart1.ChartAreas[1].AxisX.ScaleView.Scroll(ScrollType.LargeIncrement);
            }
            else if (e.KeyCode == Keys.PageUp)
            {
                chart1.ChartAreas[1].AxisX.ScaleView.Scroll(ScrollType.LargeDecrement);
            }
            else if (e.KeyCode == Keys.Home)
            {
                chart1.ChartAreas[1].AxisX.ScaleView.Scroll(ScrollType.First);
            }
            else if (e.KeyCode == Keys.End)
            {
                chart1.ChartAreas[1].AxisX.ScaleView.Scroll(ScrollType.Last);
            }

        }

        private void chart1_MouseDown(object sender, MouseEventArgs e)
        {

            if(e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                if (mGlobal.drawGraph35)
                {
                    for (int i = 0; i < chart1.ChartAreas.Count; i++)
                    {
                        if (chart1.ChartAreas[i].AxisX.ScaleView.IsZoomed)
                        {
                            chart1.ChartAreas[i].AxisX.ScaleView.ZoomReset(1);
                            chart1.ChartAreas[i].AxisY.ScaleView.ZoomReset(1);
                        }
                    }
                   
                }
                else
                {
                    if (chart1.ChartAreas[0].AxisX.ScaleView.IsZoomed)
                    {
                        chart1.ChartAreas[0].AxisX.ScaleView.ZoomReset(1);
                        chart1.ChartAreas[0].AxisY.ScaleView.ZoomReset(1);
                    }
                }
            }
        }

        private void Graph_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Right)
            {
                chart1.ChartAreas[1].AxisX.ScaleView.Scroll(ScrollType.SmallIncrement);
            }
            else if (e.KeyCode == Keys.Left)
            {
                chart1.ChartAreas[1].AxisX.ScaleView.Scroll(ScrollType.SmallDecrement);
            }
            else if (e.KeyCode == Keys.PageDown)
            {
                chart1.ChartAreas[1].AxisX.ScaleView.Scroll(ScrollType.LargeIncrement);
            }
            else if (e.KeyCode == Keys.PageUp)
            {
                chart1.ChartAreas[1].AxisX.ScaleView.Scroll(ScrollType.LargeDecrement);
            }
            else if (e.KeyCode == Keys.Home)
            {
                chart1.ChartAreas[1].AxisX.ScaleView.Scroll(ScrollType.First);
            }
            else if (e.KeyCode == Keys.End)
            {
                chart1.ChartAreas[1].AxisX.ScaleView.Scroll(ScrollType.Last);
            }
        }

       
    }
}
