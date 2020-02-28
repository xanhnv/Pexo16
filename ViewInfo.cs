using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Pexo16
{
    public partial class ViewInfo : Form
    {
        private string Type = "";
        private string footer = "";
        private byte[] data;
       

        public ViewInfo(string infoType)
        {
            InitializeComponent();
            Type = infoType;
        }


        public ViewInfo(string infoType, byte[] buf)
        {
            InitializeComponent();
            Type = infoType;
            data = buf;
        }

        private void frmViewData_Load(object sender, EventArgs e)
        {
            switch (Type)
            {
                case "viewGeneral":
                    viewGeneralInfo();
                    break;
                case "viewSum":
                    viewSumInfo();
                    break;
                case "viewData":
                    //viewData();
                    viewData16();
                    break;
                case "data35":
                    data35();
                    break;
                case "viewSumInfo35":
                    viewSumInfo35();
                    break;
                case "sumAndchart35":
                    viewSumAndChart35();
                    break;
                case "viewGeneral35":
                    viewGeneralInfo35();
                    break;
                default:
                    break;
            }

        }

        private void viewGeneralInfo()
        {
            Device dv = Device.Instance;
            dv.Timezone = mGlobal.FindSystemTimeZoneFromString(dv.Timezone).ToString();

            List<Device> dev = new List<Device>();
            dev.Add(dv);
            reportViewer1.Height = this.Height;
            reportViewer1.Width = this.Width;


            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.ReportEmbeddedResource = "Pexo16.Report1.rdlc";
            Microsoft.Reporting.WinForms.ReportDataSource dataset = new Microsoft.Reporting.WinForms.ReportDataSource("DataSet1", dev);
            reportViewer1.LocalReport.DataSources.Add(dataset);
            dataset.Value = dev;

            ReportParameter[] para = new ReportParameter[6];

            para[0] = new ReportParameter("ReportParameter1");
            para[1] = new ReportParameter("ReportParameter2");
            para[2] = new ReportParameter("ReportParameter3");
            para[3] = new ReportParameter("ReportParameter4");
            para[4] = new ReportParameter("ReportParameter5");
            para[5] = new ReportParameter("ReportParameter6");
            for (int j = 0; j < dv.numOfChannel; j++)
            {
                string valueMax;
                string valueMin;
                if (dv.channels[j].AlarmMax == 30000)
                {
                    valueMax = "No Alarm";
                    valueMin = "No Alarm";
                }
                else
                {
                    if (dv.channels[j].Unit != 0)
                    {
                        valueMax = dv.channels[j].AlarmMax.ToString();
                        valueMin = dv.channels[j].AlarmMin.ToString();
                    }
                    else
                    {
                        valueMax = "";
                        valueMin = "";
                    }
                }
                para[0].Values.Add(valueMax);
                para[1].Values.Add(valueMin);
                if (dv.channels[j].Unit != 0)
                {
                    para[2].Values.Add(dv.channels[j].Val.ToString());
                }
                else
                {
                    para[2].Values.Add("");
                }
                para[3].Values.Add(mGlobal.IntToUnit(dv.channels[j].Unit));
            }
            DateTime currenttime = DateTime.Now;
            string Time = currenttime.ToString("MM/dd/yyyy") + "   " + currenttime.ToString("HH:mm:ss");
            para[4].Values.Add(Time);

            string dura = (Convert.ToInt32(dv.Duration * (65536 - 8 - 1) / 86400)).ToString();
            para[5].Values.Add(dura);

            reportViewer1.LocalReport.SetParameters(para[0]);
            reportViewer1.LocalReport.SetParameters(para[1]);
            reportViewer1.LocalReport.SetParameters(para[2]);
            reportViewer1.LocalReport.SetParameters(para[3]);
            reportViewer1.LocalReport.SetParameters(para[4]);
            reportViewer1.LocalReport.SetParameters(para[5]);

            reportViewer1.LocalReport.Refresh();
            reportViewer1.RefreshReport();
        }

        private void viewSumInfo()
        {
            Device dv = Device.Instance;
            Graph graph = Graph.Instance;
            dv.Timezone = mGlobal.FindSystemTimeZoneFromString(dv.Timezone).ToString();

            List<Device> dev = new List<Device>();
            dev.Add(dv);
            reportViewer1.Height = this.Height;
            reportViewer1.Width = this.Width;


            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.ReportEmbeddedResource = "Pexo16.reportSumInfo.rdlc";
            Microsoft.Reporting.WinForms.ReportDataSource dataset = new Microsoft.Reporting.WinForms.ReportDataSource("DataSet1", dev);
            reportViewer1.LocalReport.DataSources.Add(dataset);
            dataset.Value = dev;

            ReportParameter[] para = new ReportParameter[17];

            para[0] = new ReportParameter("ParaUnit");
            para[1] = new ReportParameter("ParaHigh");
            para[2] = new ReportParameter("ParaLow");
            para[3] = new ReportParameter("ParaAvg");
            para[4] = new ReportParameter("ParaSmax");
            para[5] = new ReportParameter("ParaSmin");
            para[6] = new ReportParameter("ParaTimeOverHigh");
            para[7] = new ReportParameter("ParaTimeOverLow");
            para[8] = new ReportParameter("ParaStartTime");
            para[9] = new ReportParameter("ParaStopTime");
            para[10] = new ReportParameter("ParaElapse");
            para[11] = new ReportParameter("ParaComment");
            para[12] = new ReportParameter("ParaMeasurement");
            para[13] = new ReportParameter("ParaNumHigh");
            para[14] = new ReportParameter("ParaNumLow");
            para[15] = new ReportParameter("ParaFooter");
            para[16] = new ReportParameter("paraDuration");

            for (int j = 0; j < dv.numOfChannel; j++)
            {
                para[0].Values.Add(mGlobal.IntToUnit(byte.Parse(mGlobal.unitTemp[j].ToString())));
                if (dv.channels[j].Unit == 0)
                {
                    para[1].Values.Add("");
                    para[2].Values.Add("");
                    para[3].Values.Add("");
                }
                else
                {
                    para[1].Values.Add(dv.channels[j].high_suminfo.ToString());
                    para[2].Values.Add(dv.channels[j].low_suminfo.ToString());
                    para[3].Values.Add(dv.channels[j].ave_frm_suminfo.ToString());
                }

                if (dv.channels[j].Unit != 0)
                {
                    if (dv.channels[j].AlarmMax == 30000 || dv.channels[j].AlarmMax == 54032 || dv.channels[j].AlarmMax == 16649)
                    {
                        para[4].Values.Add("No Alarm");
                    }
                    else
                    {
                        para[4].Values.Add(dv.channels[j].AlarmMax.ToString());
                    }
                }
                else
                {
                    para[4].Values.Add("");
                }

                if (dv.channels[j].Unit != 0)
                {
                    if (dv.channels[j].AlarmMin == -30000 || dv.channels[j].AlarmMin == -29999 || dv.channels[j].AlarmMin == -53968 || dv.channels[j].AlarmMin == -16684) //chua nghi ra dc cach giai quyet tot dep hon (27/07/2015);
                    {
                        para[5].Values.Add("No Alram");
                    }
                    else if(dv.channels[j].Unit == 0)
                    {
                        para[5].Values.Add("");
                    }
                    else
                    {
                        para[5].Values.Add(dv.channels[j].AlarmMin.ToString());
                    }
                }
                else
                {
                    para[5].Values.Add("");
                }


                if (dv.channels[j].Unit != 0)
                {
                    para[6].Values.Add(mGlobal.Sec2Day(dv.channels[j].MaxCount * Convert.ToInt32(dv.Duration)));
                    para[7].Values.Add(mGlobal.Sec2Day(dv.channels[j].MinCount * Convert.ToInt32(dv.Duration)));
                }
                else
                {
                    para[6].Values.Add("Not Use");
                    para[7].Values.Add("Not Use");
                }

                para[13].Values.Add((dv.channels[j].MaxCount).ToString());
                para[14].Values.Add((dv.channels[j].MinCount).ToString());
            }

            para[8].Values.Add(graph._starttime + graph._timezone);
            para[9].Values.Add(graph._stoptime + graph._timezone);
            para[10].Values.Add(graph._eclapsetime);
            para[11].Values.Add(dv.comment);
            para[12].Values.Add(mGlobal.num_measure_suminfo.ToString());

            //set footer
            footer = dv.ToString() + "  Desc: " + dv.Description + " - Loc: " + dv.Location + "\r\n" + "Marathon Products, Inc." + "\r\n" + "www.marathonproducts.com / info@marathonproducts.com";
            para[15].Values.Add(footer);
            para[16].Values.Add(mGlobal.interval2duration(dv.Duration).ToString());


            for (int i = 0; i < para.Length; i++)
            {
                reportViewer1.LocalReport.SetParameters(para[i]);
            }

            reportViewer1.LocalReport.Refresh();
            reportViewer1.RefreshReport();
            reportViewer1.SetDisplayMode(Microsoft.Reporting.WinForms.DisplayMode.PrintLayout);
        }       

        private void viewData()
        {
            Device dv = Device.Instance;
            Graph graph = Graph.Instance;
            dv.Timezone = mGlobal.FindSystemTimeZoneFromString(dv.Timezone).ToString();

            List<Device> dev = new List<Device>();
            dev.Add(dv);
            reportViewer1.Height = this.Height;
            reportViewer1.Width = this.Width;


            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.ReportEmbeddedResource = "Pexo16.reportData.rdlc";
            Microsoft.Reporting.WinForms.ReportDataSource dataset = new Microsoft.Reporting.WinForms.ReportDataSource("DataSet1", dev);
            reportViewer1.LocalReport.DataSources.Add(dataset);
            dataset.Value = dev;

            ReportParameter[] para = new ReportParameter[9];
            for (int i = 0; i < 8; i++)
            {
                //if (dv.channels[i].Unit != 0)
                //{
                    para[i] = new ReportParameter("paraDataChannel" + (i + 1).ToString());
                //}
            }
            para[8] = new ReportParameter("time");

            for (int i = 0; i < dv.numOfChannel; i++)
            {
                if (dv.channels[i].Unit != 0)
                {
                    string[] temp1 = new string[dv.channels[i].Data.Length];
                    for (int j = 0; j < dv.channels[i].Data.Length; j++)
                    {
                        temp1[j] = dv.channels[i].Data[j].ToString();
                    }
                    para[i].Values.AddRange(temp1);
                }
                else
                {
                    para[i].Values.Add("");
                }
            }

            string[] temp2 = new string[dv.channels[0].Data.Length];
            for (int j = 0; j < dv.channels[0].Data.Length; j++)
            {
                temp2[j] = graph.BaseTime.AddSeconds(dv.Duration * j).ToString("HH:mm:ss MM/dd/yyyy");
            }
            para[8].Values.AddRange(temp2);

            for (int i = 0; i < para.Length; i++)
            {
                reportViewer1.LocalReport.SetParameters(para[i]);
            }

            reportViewer1.LocalReport.Refresh();
            reportViewer1.RefreshReport();
        }

        private void ViewInfo_Resize(object sender, EventArgs e)
        {
            reportViewer1.Width = this.Width - 30;
            reportViewer1.Height = this.Height - 70;
        }
        
        private void viewData16()
        {
            Device dv = Device.Instance;
            Graph graph = Graph.Instance;
            
            bindingSource1.DataSource = dv.channels;

            //xet unit co khac rong ko.
            int ch = 0;
            for (int i = 0; i < dv.numOfChannel; i++)
            {
                if (dv.channels[i].Unit != 0)
                    ch += 1;
            }

            clsTables[] listCH = new clsTables[ch];

            int channel = 1;
            int list = 0;
            for (int i = 0; i < dv.numOfChannel; i++)
            {
                if (dv.channels[i].Unit != 0)
                {
                    clsTables tab = new clsTables();
                    tab.DataS = new string[dv.channels[i].Data.Length];

                    //int x = (128 * 1024) / mGlobal.numChan;
                    //int tg = mGlobal.duration35(Convert.ToInt32(dv.Duration), x);
                   
                    tab.Channel = channel;
                    tab.Data = dv.channels[i].Data;

                    for (int j = 0; j < dv.channels[i].Data.Length; j++)
                    {
                        tab.DataS[j] = dv.channels[i].Data[j].ToString();
                    }

                    channel += 1;

                    if (dv.channels[i].Unit != 0)
                    {
                        tab.Time = new string[dv.channels[i].Data.Length];
                        for (int j = 0; j < dv.channels[i].Data.Length; j++)
                        {
                            tab.Time[j] = graph.BaseTime.AddSeconds(dv.Duration * j).ToString("HH:mm:ss MM/dd/yyyy");
                        }
                    }

                    listCH[list] = tab;
                    list += 1;
                }
                else
                {
                    channel += 1;
                }
            }

            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.ReportEmbeddedResource = "Pexo16.reportData16.rdlc";
            Microsoft.Reporting.WinForms.ReportDataSource dataset = new Microsoft.Reporting.WinForms.ReportDataSource("DataSet1", listCH);
            reportViewer1.LocalReport.DataSources.Add(dataset);
            reportViewer1.RefreshReport();
            
        }

        private void data35()
        {
            Device35 dv = Device35.Instance;
            Graph graph = Graph.Instance;
            DateTime a = new DateTime();
            bindingSource1.DataSource = dv.Channels;

            //xet unit co khac rong ko.
            int ch = 0;
            for (int i = 0; i < dv.numOfChannel; i++)
            {
                if (dv.Channels[i].Unit != 0)
                    ch += 1;
            }

            clsTables[] listCH = new clsTables[ch];

            int channel = 1;
            int dem = 0;
            int list = 0;
            for (int i = 0; i < dv.numOfChannel; i++)
            {
                if (dv.Channels[i].Unit != 0)
                {
                    clsTables tab = new clsTables();
                    tab.DataS = new string[dv.Channels[i].Data.Length];

                    int x = (128 * 1024) / mGlobal.numChan;
                    int tg = mGlobal.duration35(Convert.ToInt32(dv.Duration), x);

                    if (dv.Channels[i].Unit == 3)
                    {
                        switch (dem)
                        {
                            case 0:
                                tab.Channel = channel;
                                tab.Name = "X(G)";
                                tab.Data = dv.Channels[i].Data;
                                for (int j = 0; j < dv.Channels[i].Data.Length; j++)
                                {
                                    tab.DataS[j] = dv.Channels[i].Data[j].ToString();
                                }
                                break;

                            case 1:
                                tab.Channel = channel;
                                tab.Name = "Y(G)";
                                tab.Data = dv.Channels[i].Data;
                                for (int j = 0; j < dv.Channels[i].Data.Length; j++)
                                {
                                    tab.DataS[j] = dv.Channels[i].Data[j].ToString();
                                }
                                break;

                            case 2:
                                tab.Channel = channel;
                                tab.Name = "Z(G)";
                                tab.Data = dv.Channels[i].Data;

                                for (int j = 0; j < dv.Channels[i].Data.Length; j++)
                                {
                                    tab.DataS[j] = dv.Channels[i].Data[j].ToString();
                                }

                                channel += 1;
                                dem = -1;
                                break;
                        }
                        dem += 1;
                    }
                    else
                    {
                        tab.Channel = channel;
                        tab.Name = mGlobal.IntToUnit35_RealTime(mGlobal.unitTemp[i]);
                        tab.Data = dv.Channels[i].Data;

                        for (int j = 0; j < dv.Channels[i].Data.Length; j++)
                        {
                            tab.DataS[j] = dv.Channels[i].Data[j].ToString();
                        }

                        channel += 1;
                    }

                    if (dv.Channels[i].Unit != 0)
                    {
                        tab.Time = new string[dv.Channels[i].Data.Length];
                        tab.Stt = new string[dv.Channels[i].Data.Length];
                        for (int j = 0; j < dv.Channels[i].Data.Length; j++)
                        {
                            //tab.Time[j] = graph.BaseTime.AddSeconds(tg * j).ToString("HH:mm:ss MM/dd/yyyy");
                            a = graph.BaseTime.AddMinutes(Convert.ToInt32(dv.Delay));
                            tab.Time[j] = a.AddSeconds(tg * j).ToString("HH:mm:ss MM/dd/yyyy");
                            tab.Stt[j] = (j).ToString();
                        }
                    }

                    listCH[list] = tab;
                    list += 1;
                }
                else
                {
                    channel += 1;
                }
            }

            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.ReportEmbeddedResource = "Pexo16.reportViewData35.rdlc";
            Microsoft.Reporting.WinForms.ReportDataSource dataset = new Microsoft.Reporting.WinForms.ReportDataSource("DataSet1", listCH);
            reportViewer1.LocalReport.DataSources.Add(dataset);
            //bindingSource1.DataSource = listCH;
            //clsTablesBindingSource.DataSource = listCH;
            
            reportViewer1.RefreshReport();
            reportViewer1.SetDisplayMode(Microsoft.Reporting.WinForms.DisplayMode.PrintLayout);
            //bindingSource1.DataSource = listCH;
            //clsTablesBindingSource.DataSource = listCH;
            //reportViewer1.RefreshReport();

            reportViewer1.ZoomMode = ZoomMode.Percent;
            reportViewer1.ZoomPercent = 100;
        }

        private void viewSumInfo35()
        {
            Device35 dv = Device35.Instance;
            Graph graph = Graph.Instance;
            //bindingSource1.DataSource = dv.Channels;

            clsTables[] listCH = new clsTables[dv.numOfChannel];

            int x = (128 * 1024) / mGlobal.numChan;
            int tg = mGlobal.duration35(Convert.ToInt32(dv.Duration), x);

            int channel = 1;
            int dem = 0;
            double div = 0;
            for (int i = 0; i < dv.numOfChannel; i++)
            {
                if(dv.Channels[i].Unit != 3)
                {
                    dv.Channels[i].ave_frm_suminfo = Math.Round(dv.Channels[i].ave_frm_suminfo, 1);
                    div = 10.0;
                }
                else
                {
                    div = 1000.0;
                }

                clsTables tab = new clsTables();
                tab.DataS = new string[dv.Channels[i].Data.Length];

                //int x = (128 * 1024) / mGlobal.numChan;
                //int tg = mGlobal.duration35(Convert.ToInt32(dv.Duration), x);

                if (dv.Channels[i].Unit == 3)
                {
                    switch (dem)
                    {
                        case 0:
                            tab.Channel = channel;
                            tab.Name = "X";
                            break;

                        case 1:
                            tab.Channel = channel;
                            tab.Name = "Y";
                            break;

                        case 2:
                            tab.Channel = channel;
                            tab.Name = "Z";
                            channel += 1;
                            dem = -1;
                            break;
                    }
                    dem += 1;
                }
                else
                {
                    tab.Channel = channel;
                    tab.Name = "";
                    channel += 1;
                }

                tab.Data = dv.Channels[i].Data;
                if (dv.Channels[i].Unit != 0)
                {
                    //tab.Unit = mGlobal.IntToUnit35(dv.Channels[i].Unit);
                    tab.Unit = mGlobal.IntToUnit35(mGlobal.unitTemp[i]);
                    tab.Highest = dv.Channels[i].high_suminfo.ToString();
                    tab.Lowest = dv.Channels[i].low_suminfo.ToString();
                    tab.Average = dv.Channels[i].ave_frm_suminfo.ToString();
                    if (mGlobal.unitTemp[i] == 172 || mGlobal.unitTemp[i] == 175)
                    {
                        tab.Sd = Math.Round(mGlobal.StandartDeviation(dv.Channels[i].Data, 1), 2).ToString();
                        tab.Mkt = Math.Round(mGlobal.MKT(dv.Channels[i].Data, 1) - 273.16, 2).ToString();
                    }
                    else
                    {
                        tab.Sd = "";
                        tab.Mkt = "";
                    }

                    if (dv.Channels[i].Data.Length % 2 != 0)
                    {
                        tab.Med = (dv.Channels[i].Data[dv.Channels[i].Data.Length / 2]).ToString();
                    }
                    else
                    {
                        tab.Med = ((dv.Channels[i].Data[dv.Channels[i].Data.Length / 2] + dv.Channels[i].Data[dv.Channels[i].Data.Length / 2 + 1]) / 2).ToString();
                    }


                    if (dv.Channels[i].NoAlarm)
                    {
                        tab.MaxAlarm = "No Alarm";
                        tab.MinAlarm = "No Alarm";
                    }
                    else
                    {
                        tab.MaxAlarm = (Convert.ToSingle(dv.Channels[i].AlarmMax) / 10).ToString();
                        tab.MinAlarm = (Convert.ToSingle(dv.Channels[i].AlarmMin) / 10).ToString();
                    }
                    tab.TimeHigh = mGlobal.Sec2Day(dv.Channels[i].MaxCount * tg);
                    tab.TimeLow = mGlobal.Sec2Day(dv.Channels[i].MinCount * tg);
                    tab.NumHigh = dv.Channels[i].MaxCount.ToString();
                    tab.NumLow = dv.Channels[i].MinCount.ToString();
                }
                else
                {
                    tab.Unit = "---";
                    tab.Highest = "---";
                    tab.Lowest = "---";
                    tab.Average = "---";
                    tab.MaxAlarm = "---";
                    tab.MinAlarm = "---";
                    tab.TimeHigh = "Not Use";
                    tab.TimeLow = "Not Use";
                    tab.NumHigh = "Not Use";
                    tab.NumLow = "Not Use";
                }

                listCH[i] = tab;
            }

            ReportParameter[] para = new ReportParameter[12];

            para[0] = new ReportParameter("paraDuration");
            para[1] = new ReportParameter("paraDelay");
            para[2] = new ReportParameter("paraSerial");
            para[3] = new ReportParameter("paraDescription");
            para[4] = new ReportParameter("paraLocation");
            para[5] = new ReportParameter("paraStart");
            para[6] = new ReportParameter("paraStop");
            para[7] = new ReportParameter("paraElapse");
            para[8] = new ReportParameter("paraInterval");
            para[9] = new ReportParameter("paraMeasurement");
            para[10] = new ReportParameter("paraComment");
            para[11] = new ReportParameter("paraFooter");


            para[0].Values.Add(dv.Duration.ToString());
            para[1].Values.Add(dv.Delay.ToString());
            para[2].Values.Add(dv.Serial.ToString());
            para[3].Values.Add(dv.Description.ToString());
            para[4].Values.Add(dv.Location.ToString());
            para[5].Values.Add(graph._starttime +" "+ graph._timezone);
            para[6].Values.Add(graph._stoptime +" "+ graph._timezone);
            para[7].Values.Add(graph._eclapsetime);
            para[8].Values.Add(dv.Interval.Substring(17, 6));
            para[9].Values.Add(mGlobal.num_measure_suminfo.ToString());
            if (dv.comment != "")
            {
                para[10].Values.Add(dv.comment);
            }
            else
            {
                para[10].Values.Add("No comment");
            }

            //set footer
            footer = dv.Serial.ToString() + "  Desc: " + dv.Description + " - Loc: " + dv.Location + "\r\n" + "Marathon Products, Inc." + "\r\n" + "www.marathonproducts.com / info@marathonproducts.com";
            para[11].Values.Add(footer);

            for (int i = 0; i < para.Length; i++)
            {
                reportViewer1.LocalReport.SetParameters(para[i]);
            }

            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.ReportEmbeddedResource = "Pexo16.reportSumInfo35.rdlc";
            Microsoft.Reporting.WinForms.ReportDataSource dataset = new Microsoft.Reporting.WinForms.ReportDataSource("DataSet1", listCH);
            reportViewer1.LocalReport.DataSources.Add(dataset);
            reportViewer1.LocalReport.Refresh();

            //bindingSource1.DataSource = listCH;
            //clsTablesBindingSource.DataSource = listCH;
            reportViewer1.RefreshReport();
            reportViewer1.SetDisplayMode(Microsoft.Reporting.WinForms.DisplayMode.PrintLayout);
            reportViewer1.ZoomMode = ZoomMode.Percent;
            reportViewer1.ZoomPercent = 100;
        }

        private void viewSumAndChart35() //xai hok dc(06/05/2015)
        {
            Image prn_Image;
            Graph graph = Graph.Instance;
            DataTable dt = new DataTable();
            DataColumn column = new DataColumn("chart");
            column.DataType = System.Type.GetType("System.Byte[]");
            dt.Columns.Add(column);
            DataRow dr = dt.NewRow();
            clsTables tab = new clsTables();

            //My_ImageProperties ExImg;
            //ExImg.Width = 1200;
            //ExImg.Height = 800;
            //ExImg.ImageType = My_ImgType._gif;

            string FileName = "";
            FileName = mGlobal.app_patch(FileName);

            if (System.IO.File.Exists(FileName + "\\~temp._t_") == false)
            {
                System.IO.File.WriteAllText(FileName + "\\~temp._t_", null);
            }

            graph.chart1.SaveImage(FileName + "\\~temp._t_", System.Drawing.Imaging.ImageFormat.Png);

            //Saving the Image of Graph into memory stream
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            //graph.chart1.SaveImage(ms, ChartImageFormat.Png);
            //Chart1.SaveImage(ms, ChartImageFormat.Png);

            byte[] ByteImage = new byte[ms.Length];
            ms.Position = 0;
            ms.Read(ByteImage, 0, (int)ms.Length);


            dr["chart"] = ByteImage;
            string s = dr[0].ToString();
            dt.Rows.Add(dr);
            //dt.Columns.Add(s);
            ReportParameter para = new ReportParameter("paraImage");
            para.Values.Add(FileName + "\\~temp._t_");

            reportViewer1.LocalReport.EnableExternalImages = true;
            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.SetParameters(para);
           
            reportViewer1.LocalReport.ReportPath = "Pexo16.reportGraphAndSum35.rdlc";

            reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", dt));
            reportViewer1.Visible = true;
            reportViewer1.LocalReport.Refresh();


            //reportViewer1.LocalReport.ReportPath = Microsoft.SqlServer.Server.MapPath("~/Report.rdlc");
            //reportViewer1.LocalReport.EnableExternalImages = true;
            //string imagePath = new Uri(Microsoft.SqlServer.Server.MapPath("~/images/Mudassar.jpg")).AbsoluteUri;
            //ReportParameter parameter = new ReportParameter("ImagePath", imagePath);
            //reportViewer1.LocalReport.SetParameters(parameter);
            //reportViewer1.LocalReport.Refresh();
        }

        private void viewGeneralInfo35()
        {
            Device35 dv = Device35.Instance;
            List<Device35> dev = new List<Device35>();
            dev.Add(dv);
            clsTables[] listCH = new clsTables[12];

            ReportParameter[] para = new ReportParameter[1];

            para[0] = new ReportParameter("paraTime");

            para[0].Values.Add(DateTime.Now.ToString());

            int count = 0;
            int dem = 0;
            for (int i = 0; i < 4; i++)
            {
                int tmpDiv;
                dem = 0;
                if (dv.Channels[i].Unit == 175 || dv.Channels[i].Unit == 172 || dv.Channels[i].Sensor == 2)
                {
                    tmpDiv = 10;
                }
                else if (dv.Channels[i].Sensor == 3)
                {
                    tmpDiv = 1000;
                }
                else
                {
                    tmpDiv = 1;
                }


                if (dv.Channels[i].Sensor == 3)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        clsTables tab = new clsTables();
                        switch (dem)
                        {
                            case 0:
                                tab.Channel = i + 1;
                                tab.Name = "X";
                                tab.Unit = mGlobal.IntToUnit35(dv.Channels[i].Sensor);
                                tab.CurentValue =  (mGlobal.get_temp(data[3 + 7 * i], data[4 + 7 * i]) / tmpDiv).ToString();
                                if (dv.Channels[i].NoAlarm)
                                {
                                    tab.MaxAlarm = "No Alarm";
                                    tab.MinAlarm = "No Alarm";
                                }
                                else
                                {
                                    tab.MaxAlarm = dv.Channels[i].AlarmMax.ToString();
                                    tab.MinAlarm = dv.Channels[i].AlarmMin.ToString();
                                }
                                break;  

                            case 1:
                                tab.Channel = i + 1;
                                tab.Name = "Y";
                                tab.Unit = mGlobal.IntToUnit35(dv.Channels[i].Sensor);
                                tab.CurentValue = (mGlobal.get_temp(data[5 + 7 * i], data[6 + 7 * i]) / tmpDiv).ToString();
                                if (dv.Channels[i].NoAlarm)
                                {
                                    tab.MaxAlarm = "No Alarm";
                                    tab.MinAlarm = "No Alarm";
                                }
                                else
                                {
                                    tab.MaxAlarm = dv.Channels[i].AlarmMax.ToString();
                                    tab.MinAlarm = dv.Channels[i].AlarmMin.ToString();
                                }
                                break;

                            case 2:
                                tab.Channel = i + 1;
                                tab.Name = "Z";
                                tab.Unit = mGlobal.IntToUnit35(dv.Channels[i].Sensor);
                                tab.CurentValue = (mGlobal.get_temp(data[7 + 7 * i], data[8 + 7 * i]) / tmpDiv).ToString();
                                if (dv.Channels[i].NoAlarm)
                                {
                                    tab.MaxAlarm = "No Alarm";
                                    tab.MinAlarm = "No Alarm";
                                }
                                else
                                {
                                    tab.MaxAlarm = dv.Channels[i].AlarmMax.ToString();
                                    tab.MinAlarm = dv.Channels[i].AlarmMin.ToString();
                                }
                                break;
                                
                        }
                        dem += 1;
                        listCH[count] = tab;
                        count += 1;
                    }
                }
                else
                {
                    clsTables tab = new clsTables();
                    tab.Channel = i + 1;
                    tab.Name = "";
                    if (dv.Channels[i].Sensor == 1)
                    {
                        tab.Unit = mGlobal.IntToUnit35(dv.Channels[i].Unit);
                    }
                    else
                    {
                        tab.Unit = mGlobal.IntToUnit35(dv.Channels[i].Sensor);
                    }
                    tab.CurentValue = (mGlobal.get_temp(data[3 + 7 * i], data[4 + 7 * i]) / tmpDiv).ToString();
                    if(dv.Channels[i].Sensor == 0)
                    {
                        tab.MaxAlarm = "";
                        tab.MinAlarm = "";
                        tab.CurentValue = "";
                    }
                    else if (dv.Channels[i].NoAlarm == true)
                    {
                        tab.MaxAlarm = "No Alarm";
                        tab.MinAlarm = "No Alarm";
                    }
                    else
                    {
                        tab.MaxAlarm = (dv.Channels[i].AlarmMax /10).ToString();
                        tab.MinAlarm = (dv.Channels[i].AlarmMin /10).ToString();
                    }
                    listCH[count] = tab;
                    count += 1;
                }
            }

            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.ReportEmbeddedResource = "Pexo16.reportGeneralInfo35.rdlc";
            Microsoft.Reporting.WinForms.ReportDataSource dataset = new Microsoft.Reporting.WinForms.ReportDataSource("DataSet1", dev);
            Microsoft.Reporting.WinForms.ReportDataSource dataset2 = new Microsoft.Reporting.WinForms.ReportDataSource("DataSet2", listCH);
            reportViewer1.LocalReport.DataSources.Add(dataset);
            reportViewer1.LocalReport.DataSources.Add(dataset2);


            for (int i = 0; i < para.Length; i++)
            {
                reportViewer1.LocalReport.SetParameters(para[i]);
            }

            reportViewer1.LocalReport.Refresh();
            reportViewer1.RefreshReport();

            reportViewer1.ZoomMode = ZoomMode.Percent;
            reportViewer1.ZoomPercent = 100;
        }


    }
    public class clsTables
    {
        private int channel;
        public int Channel
        {
            get { return channel; }
            set { channel = value; }
        }

        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private float[] data;
        public float[] Data
        {
            get { return data; }
            set { data = value; }
        }

        private string[] dataS;
        public string[] DataS
        {
            get { return dataS; }
            set { dataS = value; }
        }

        private string[] time;
        public string[] Time
        {
            get { return time; }
            set { time = value; }
        }

        private string[] stt;
        public string[] Stt
        {
            get { return stt; }
            set { stt = value; }
        }


        private string unit;
        public string Unit
        {
            get { return unit; }
            set { unit = value; }
        }

        private string highest;
        public string Highest
        {
            get { return highest; }
            set { highest = value; }
        }

        private string lowest;
        public string Lowest
        {
            get { return lowest; }
            set { lowest = value; }
        }

        private string average;
        public string Average
        {
            get { return average; }
            set { average = value; }
        }

        private string sd;
        public string Sd
        {
            get { return sd; }
            set { sd = value; }
        }

        private string mkt;
        public string Mkt
        {
            get { return mkt; }
            set { mkt = value; }
        }

        private string med;
        public string Med
        {
            get { return med; }
            set { med = value; }
        }

        private string maxAlarm;
        public string MaxAlarm
        {
            get { return maxAlarm; }
            set { maxAlarm = value; }
        }

        private string minAlarm;
        public string MinAlarm
        {
            get { return minAlarm; }
            set { minAlarm = value; }
        }

        private string numHigh;
        public string NumHigh
        {
            get { return numHigh; }
            set { numHigh = value; }
        }

        private string numLow;
        public string NumLow
        {
            get { return numLow; }
            set { numLow = value; }
        }

        private string timeHigh;
        public string TimeHigh
        {
            get { return timeHigh; }
            set { timeHigh = value; }
        }

        private string timeLow;
        public string TimeLow
        {
            get { return timeLow; }
            set { timeLow = value; }
        }

        private string curentValue;
        public string CurentValue
        {
            get { return curentValue; }
            set { curentValue = value; }
        }

        private Image ima;
        public Image Ima
        {
            get { return ima; }
            set { ima = value; }
        }

    }
}
