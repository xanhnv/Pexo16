using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Windows.Forms;

namespace Pexo16
{
    public static class mGlobal
    {
        //color changed
        public static bool ColorChanged;
        //title Graph changed
        public static bool TitleChanged;
        //comment changed
        public static bool CommentChanged;
        //path file open

        public static string usb_id;//getData.getActiveDevice
        public static bool usb_search;
        public static bool open_file;
        public static byte[] ibuf_read = new byte[1024 * 1024];
        public static bool readlog_opened;
        public static int num_measure_suminfo;
        public static int numOfClipboard;

        //toolbar_open
        public static string defaultFolder = "";
        public static string expireDate = "";
        public static string updateURL = "";
        public static byte[] data_byte;
        public static bool openedChart = false;
        public static string checkName = "";
        public static string SavingPath = "";

        //draw_graph
        public static string TitleGraph = "";
        public static bool C2F;// true: C-->F, false F-->C
        public static byte[] unitTemp = new byte[12];
        public static bool unitFromFile = false;

        //
        public static bool activeDevice = true;//luc mo file thì = false
        //public static ArrayList perfectItemAL = new ArrayList();
        

        //GeneralInfomation
        public static bool PrintPDF;
        public static bool tlb_eclapse;
        public static bool tlb_value;
        public static string PathFile;

        public static bool drawGraph35;

        public static int numChan;



        //for multi language
        public static string language;


        public static int numProgress;

        public static ushort len;


        // multi Setting
        //public static ArrayList checkedDevice = new ArrayList();

        public static string FindString1(string MainStr, string FindStr, string end_str)
        {
            //Returned string = between FindStr and ENTER
            //if ERROR --> Return Empty string
            string _retStr = null;
            Int32 _start = MainStr.IndexOf(FindStr);
            if (_start == 0)
                return "";
            _start += FindStr.Length;
            try
            {
                _retStr = MainStr.Substring(_start);
            }
            catch (Exception)
            {
                return "";
            }
            Int32 _end = _retStr.IndexOf(end_str) - 1;
            try
            {
               _retStr = MainStr.Substring(_start, _end);
            }
            catch (Exception)
            {
                return "";
            }
            return _retStr;
        }


        public static string app_patch(string patch)
        {
            string dd = Application.StartupPath.ToString();
            string username = WindowsIdentity.GetCurrent().Name;

            string username1 = username.Substring(username.LastIndexOf("\\") + 1);
            //int pos4 = dd.IndexOf(":");
            //dd = dd.Substring(0, dd.IndexOf("Documents") + 9);

            dd = "C:\\Users\\" + username1 + "\\Documents";

            if (System.IO.Directory.Exists(dd + "\\MaxWifi\\" + username1) == false)
            {
                System.IO.Directory.CreateDirectory(dd + "\\MaxWifi\\" + username1);
                //System.IO.File.WriteAllText(dd & "\MaxWifi_" & username1 & "\MenuData2.bin", "MenuData2.bin" & vbCrLf & "0" & vbCrLf)
            }
            patch = dd + "\\MaxWifi\\" + username1;

            //if (System.IO.Directory.Exists(dd + "\\MaxWifi\\" + username1) == false)
            //{
            //    System.IO.Directory.CreateDirectory(dd + "\\MaxWifi\\" + username1);
            //    //System.IO.File.WriteAllText(dd & "\MaxWifi_" & username1 & "\MenuData2.bin", "MenuData2.bin" & vbCrLf & "0" & vbCrLf)
            //}
            //patch = dd + "\\MaxWifi\\" + username1;

            return patch;
        }

        public static string app_patch_G(string patch)
        {
            string dd = Application.StartupPath.ToString();
            string username = WindowsIdentity.GetCurrent().Name;

            string username1 = username.Substring(username.LastIndexOf("\\") + 1);
            int pos4 = dd.IndexOf(":");
            dd = dd.Substring(0, dd.IndexOf(":") + 1);

            if (System.IO.Directory.Exists(dd + "\\MaxWifi\\" + username1) == false)
            {
                System.IO.Directory.CreateDirectory(dd + "\\MaxWifi\\" + username1);
                //System.IO.File.WriteAllText(dd & "\MaxWifi_" & username1 & "\MenuData2.bin", "MenuData2.bin" & vbCrLf & "0" & vbCrLf)
            }
            patch = dd + "\\MaxWifi\\" + username1;

            return patch;
        }


        public static TimeZoneInfo FindSystemTimeZoneFromString(string TzName)
        {
            string str = null;
            if(TzName == null)
            {
                return System.TimeZoneInfo.Local;
            }
            ReadOnlyCollection<TimeZoneInfo> Tz = TimeZoneInfo.GetSystemTimeZones();
            foreach (TimeZoneInfo timeZone in Tz)
            {
                str = timeZone.Id;
                str = Simulate.LSet(str, 15) + Simulate.RSet(str.Substring(Math.Max(str.Length - 2, 1) - 1), 3);
                if (str == TzName)
                    return timeZone;
            }
            //Select Time zone
            return System.TimeZoneInfo.Local;
        }
   

        public static Array get_unit(ref int[] unit, byte[] data, int numOfChannel)
        {
            for (int i = 0; i < numOfChannel; i++)
            {
                unit[i] = data[32 + 10 * i];
            }
            return unit;
        }

        public static Array get_unit35(ref int[] unit, byte[] data)
        {
            unit = new int[4];
            for (int i = 0; i < 4; i++)
            {
                if (data[i + 30] == 1)
                {
                    unit[i] = data[i + 34];
                }
                else
                {
                    unit[i] = data[i + 30];
                }
            }
            return unit;
        }

        //get Delay from Data (second)
        public static int get_delay(int delay, byte[] data)
        {
            delay = data[32 + 80] + data[32 + 80 + 1] * 256;
            delay = delay / 60;
            return delay;
        }

        //get Duration from Data (second)
        public static int get_duration(int dur, byte[] data)
        {
            dur = data[32 + 80 + 2] + data[32 + 80 + 3] * 256;
            return dur;
        }

       

        //----get max from Data 
        public static List<int> get_max(List<int> max, byte[] data, int numOfChannel)
        {
            for (int i = 0; i < numOfChannel; i++)
            {
                max[i] = (data[39 + 10 * i] * 256 + data[39 - 1 + 10 * i]);
                if ((max[i] & 32768) == 32768)
                {
                    max[i] = max[i] - 65536;
                }
                max[i] = max[i] / 10;
            }
            return max;
        }

        //----get min from Data 
        public static List<int> get_min(List<int> min, byte[] data, int numOfChannel)
        {
            for (int i = 0; i <= 7; i++)
            {
                min[i] = (data[41 + 10 * i] * 256 + data[41 - 1 + 10 * i]);
                if ((min[i] & 32768) == 32768)
                {
                    min[i] = min[i] - 65536;
                }
                min[i] = min[i] / 10;
            }
            return min;
        }


        public static void _get_timezone_date(ref TimeZoneInfo _timezone, ref DateTime _date, byte[] _in)
        {
            string _str = null;
            _str = "";

            if (open_file == true)
            {
                for (int i = 0; i < 22; i++)
                {
                    _str += (char)_in[100 + i];
                }

                _str = _str.TrimEnd('\0');
                _timezone = FindTimeZoneByID(_str);

                try
                {
                    _date = new DateTime(Int32.Parse(_in[125].ToString()) * 256 + Convert.ToInt32(_in[124]), Convert.ToInt32(_in[123]), Convert.ToInt32(_in[122]), Convert.ToInt32(_in[126]), Convert.ToInt32(_in[127]), Convert.ToInt32(_in[128]));
                }
                catch (Exception)
                {
                    MessageBox.Show("Datetime was fail");
                    _date = DateTime.Now;
                }
            }
            else
            {
                //lay chuoi Timezone tu offset  0-17
                for (var i = 0; i <= 17; i++)
                {
                    _str += ((char)_in[i]).ToString();
                }
                if (_str.IndexOf("@") + 1 > 0)
                {
                    _str = _str.Substring(0, _str.IndexOf("@"));
                }

                _timezone = FindSystemTimeZoneFromString(_str);

                try
                {
                    //New(ByVal year As Integer, ByVal month As Integer, ByVal day As Integer, ByVal hour As Integer, ByVal minute As Integer, ByVal second As Integer)
                    _date = Convert.ToDateTime((new DateTime(2000 + Convert.ToInt32(_in[22]), Convert.ToInt32(_in[21]), Convert.ToInt32(_in[20]), Convert.ToInt32(_in[23]), Convert.ToInt32(_in[24]), Convert.ToInt32(_in[25]))).ToShortDateString() + " " + (new DateTime(2000 + Convert.ToInt32(_in[22]), Convert.ToInt32(_in[21]), Convert.ToInt32(_in[20]), Convert.ToInt32(_in[23]), Convert.ToInt32(_in[24]), Convert.ToInt32(_in[25]))).ToString("HH:mm:ss"));
                }
                catch (Exception)
                {
                    _date = DateTime.Now;
                }
            }
        }


        public static void _get_timezone_date35(ref TimeZoneInfo _timezone, ref DateTime _date, byte[] _in)
        {
            string _str = null;
            _str = "";

            if (open_file == true)
            {
                for (int i = 0; i <= 17; i++)
                {
                    _str += (char)_in[i];
                }
                _timezone = FindSystemTimeZoneFromString(_str);

                try
                {
                    _date = new DateTime(2000 + Convert.ToInt32(_in[18]), Convert.ToInt32(_in[19]), Convert.ToInt32(_in[20]), Convert.ToInt32(_in[21]), Convert.ToInt32(_in[22]), Convert.ToInt32(_in[23]));
                }
                catch (Exception)
                {
                    MessageBox.Show("Datetime was fail");
                    _date = DateTime.Now;
                }
            }
            else
            {
                //lay chuoi Timezone tu offset  0-17
                for (var i = 0; i <= 17; i++)
                {
                    _str += ((char)_in[i]).ToString();
                }
                if (_str.IndexOf("@") + 1 > 0)
                {
                    _str = _str.Substring(0, _str.IndexOf("@"));
                }

                _timezone = FindSystemTimeZoneFromString(_str);

                try
                {
                    //New(ByVal year As Integer, ByVal month As Integer, ByVal day As Integer, ByVal hour As Integer, ByVal minute As Integer, ByVal second As Integer)
                    _date = Convert.ToDateTime((new DateTime(2000 + Convert.ToInt32(_in[18]), Convert.ToInt32(_in[19]), Convert.ToInt32(_in[20]), Convert.ToInt32(_in[21]), Convert.ToInt32(_in[22]), Convert.ToInt32(_in[23]))).ToShortDateString() + " " + (new DateTime(2000 + Convert.ToInt32(_in[18]), Convert.ToInt32(_in[19]), Convert.ToInt32(_in[20]), Convert.ToInt32(_in[21]), Convert.ToInt32(_in[22]), Convert.ToInt32(_in[23]))).ToString("HH:mm:ss"));
                }
                catch (Exception)
                {
                    _date = DateTime.Now;
                }
            }
        }


        public static string Sec2Day(Int64 sec)
        {
            string day_string = "";
            int day = Convert.ToInt32(sec) / (24 * 60 * 60);
            int hour = int.Parse(Math.Truncate((decimal)((sec % (24 * 60 * 60)) / 3600)).ToString());
            int min = int.Parse(Math.Truncate((decimal)((sec % (24 * 60 * 60)) % 3600) / 60).ToString());
            int sec_1 = int.Parse((sec % 60).ToString());
            day_string = day + " Day " + hour.ToString("00") + ":" + min.ToString("00") + ":" + sec_1.ToString("00");
            return day_string;
        }


        public static float get_temp(byte high_byte, byte low_byte)
        {
            int temp = 0;
            temp = 256 * high_byte + low_byte;
            if (temp > 32768)
            {
                temp = temp - 1 - 65535;
            }
            return temp;
        }


        public static float ExchangeF2C(ref float _F)
        {
            _F = Convert.ToSingle(((_F - 32) / 1.8).ToString("0.0"));
            return _F;
        }


        public static float ExchangeC2F(ref float _C)
        {
            _C = Convert.ToSingle((_C * 1.8 + 32).ToString("0.0"));
            return _C;
        }


        public static TimeZoneInfo FindSystemTimeZoneFromDisplayName(string TzName)
        {
            ReadOnlyCollection<TimeZoneInfo> Tz = TimeZoneInfo.GetSystemTimeZones();
            string str = TzName.Substring(0, 4);
            string str2 = null;
            foreach (TimeZoneInfo timeZone in Tz)
            {
                //replace GMT and UTC
                str2 = timeZone.DisplayName.Substring(0, 4);
                TzName = TzName.Replace(str, str2);
                if (timeZone.DisplayName == TzName)
                {
                    return timeZone;
                }
            }
            //Select Time zone
            return System.TimeZoneInfo.Local;
        }


        public static TimeZoneInfo FindTimeZoneByID(string id)
        {
            ReadOnlyCollection<TimeZoneInfo> Tz = TimeZoneInfo.GetSystemTimeZones();
            foreach (TimeZoneInfo timeZone in Tz)
            {
                if (timeZone.Id.ToString().Contains(id))
                {
                    return timeZone;
                }
            }
            return System.TimeZoneInfo.Local;
            //return TimeZoneInfo.FindSystemTimeZoneById(id);
        }

        public static float find_max(float[] _array)
        {
            float max = -1000F;
            for (var i = 0; i < _array.Length; i++)
            {
                if (_array[i] > max)
                {
                    max = _array[i];
                }
            }
            return max;
        }


        public static float find_min(float[] _array)
        {
            float min = 1000F;
            for (var i = 0; i < _array.Length; i++)
            {
                if (_array[i] < min)
                {
                    min = _array[i];
                }
                //if(_array[i] == 0)
                //{
                //    MessageBox.Show(i.ToString());
                //}
            }
            return min;
        }


        public static int duration(int dur)
        {
            if ((86400 * dur) % (65536 - 8 - 1) != 0)
            {
                dur = (86400 * dur) / (65536 - 8 - 1) + 1;
            }
            else
            {
                dur = (86400 * dur) / (65536 - 8 - 1);
            }
            return dur;
        }
        /// <summary>
        /// Tìm ra tổng số giây của interval ứng với Duration và số kênh 
        /// </summary>
        /// <param name="ngay"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        public static int duration35(int ngay, int x)
        {
            int ng = (ngay * 86400) / x;
            int du = 0;
            if ((ngay * 86400) % x != 0)
            {
                du = 1;
            }
            return ng + du;
            // voi x la datalenght moi Channel.
        }
        /// <summary>
        /// Calculator interval (second) from total duration hour, x= datalength each channel
        /// </summary>
        /// <param name="duraHour"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        public static int DurationToInterval(int duraHour, int x)
        {
            int interval = (duraHour * 3600) / x;
            int du = 0;
            if ((duraHour * 3600) % x != 0)
            {
                du = 1;
            }
            return interval + du;
        }
        public static int interval2duration(int dur)
        {
            dur = (dur * (65536 - 8 - 1)) / 86400;
            return Convert.ToInt32(dur);
        }

        public static string toUnit35(byte iUnit, byte sensor)
        {
            string sUnit = "";
            switch (sensor)
            {
                case 1:
                    {
                        if (iUnit == 172)
                            sUnit = "Celsius";
                        else if (iUnit == 175)
                            sUnit = "Fahrenheit";
                        break;
                    }
                case 2:
                    sUnit = "%RH";
                    break;
                case 3:
                    sUnit = "G";
                    break;
                case 4:
                    sUnit = "PPM";
                    break;
                default:
                    sUnit = "Not Use";
                    break;
            }
            return sUnit;
        }

        public static string IntToUnit(byte iUnit)
        {
            string sUnit = null;

            switch (iUnit)
            {
                case 1:
                    sUnit = "Celsius";
                    break;
                case 2:
                    sUnit = "Fahrenheit";
                    break;
                case 3:
                    sUnit = "ppm";
                    break;
                case 4:
                    sUnit = "%RH";
                    break;
                default:
                    sUnit = "Not Use";
                    break;
            }
            return sUnit;
        }

        public static string IntToUnit35_RealTime(byte iUnit)
        {
            string sUnit = null;

            switch (iUnit)
            {
                case 1:
                    sUnit = "Temperature";
                    break;
                case 172:
                    sUnit = "Temp.(C)";
                    break;
                case 175:
                    sUnit = "Temp.(F)";
                    break;
                case 2:
                    sUnit = "Humid.(%)";
                    break;
                case 3:
                    sUnit = "ACC.(G)";
                    break;
                case 4:
                    sUnit = "";
                    //sUnit = "CO2.(PPM)";
                    break;
                default:
                    sUnit = "Not Use";
                    break;
            }
            return sUnit;
        }

        public static string IntToUnit35(byte iUnit)
        {
            string sUnit = null;

            switch (iUnit)
            {
                case 1:
                    sUnit = "Temperature";
                    break;
                case 172:
                    sUnit = "Celsius";
                    break;
                case 175:
                    sUnit = "Fahrenheit";
                    break;
                case 2:
                    sUnit = "%RH";
                    break;
                case 3:
                    sUnit = "G";
                    break;
                case 4:
                    sUnit = "ppm";
                    break;
                default:
                    sUnit = "Not Use";
                    break;
            }
            return sUnit;
        }


        public static string IntToSenSor35(byte iUnit)
        {
            string sUnit = null;

            switch (iUnit)
            {
                case 1:
                    sUnit = "Temp";
                    break;
                case 2:
                    sUnit = "%RH";
                    break;
                case 3:
                    sUnit = "Hum";
                    break;
                case 4:
                    sUnit = "ppm";
                    break;
                default:
                    sUnit = "No sensor";
                    break;
            }
            return sUnit;
        }


        public static byte UnitToInt(string sUnit)
        {
            byte iUnit = 0;

            switch (sUnit)
            {
                case "Celsius":
                    iUnit = 1;
                    break;
                case "Fahrenheit":
                    iUnit = 2;
                    break;
                case "CO2":
                    iUnit = 3;
                    break;
                case "%RH":
                    iUnit = 4;
                    break;
                case "ppm":
                    iUnit = 3;
                    break;
                default:
                    iUnit = 0;
                    break;
            }
            return iUnit;
        }


        public static string IntToUnit_Dashboard(byte iUnit)
        {
            string sUnit = null;

            switch (iUnit)
            {
                case 1:
                    sUnit = "'C";
                    break;
                case 2:
                    sUnit = "'F";
                    break;
                case 3:
                    sUnit = "ppm";
                    break;
                case 4:
                    sUnit = "%RH";
                    break;
                default:
                    sUnit = "-";
                    break;
            }
            return sUnit;
        }

        public static string IntToUnit_Dashboard35(byte iUnit, byte iSensor)
        {
            string sUnit = null;

            switch (iSensor)
            {
                case 1:
                    if(iUnit == 172)
                        sUnit = "'C";
                    else if(iUnit == 175)
                        sUnit = "'F";
                    break;
                case 2:
                    sUnit = "%RH";
                    break;
                case 3:
                    sUnit = "G";
                    break;
                case 4:
                    sUnit = "ppm";
                    break;
                default:
                    sUnit = "--";
                    break;
            }
            return sUnit;
        }

        public static string format_num(double num)
        {
            string _str = "";
            _str = num.ToString("0.0000", CultureInfo.InvariantCulture);
            return _str;
        }


        public static string format_numDB35(double num)
        {
            string _str = "";
            _str = num.ToString("0.000", CultureInfo.InvariantCulture);
            return _str;
        }


        public static string GetDefaultPrinterName(ref string printerName)
        {
            System.Drawing.Printing.PrinterSettings oPS = new System.Drawing.Printing.PrinterSettings();
            try
            {
                printerName = oPS.PrinterName;
            }
            catch(SystemException)
            {
                printerName = "";
            }
            finally
            {
                oPS = null;
            }
            return printerName;
        }

        [DllImport("Winspool.drv")]
        public static extern bool SetDefaultPrinter(string printerName);
        public static void SetDefaultPrinterName(string printerName) //set doPDF default printer
        {
            Object wshnetwork = Activator.CreateInstance(Type.GetTypeFromProgID("WScript.Network"));
            //Object wshnetwork = CreateObject("");
            //wshnetwork.SetDefaultPrinter(printerName);
        }


        public static void LoadTimeZoneFromSystem_ComboBox(ref ToolStripComboBox cboBox)
        {
		    try
            {
		        cboBox.Items.Clear();
		    }
		    catch (Exception)
		    {
		    }
		    ReadOnlyCollection<TimeZoneInfo> Tz = TimeZoneInfo.GetSystemTimeZones();
		    foreach (TimeZoneInfo timeZone in Tz)
		    {
			    cboBox.Items.Add(timeZone.DisplayName);
		    }
	    }


        public static string ArrayToStr(ref byte[] a, int offset, int len)
        {
            string s = null;
            int i = 0;
            s = "";
            i = 0;
            while ((a[i + offset] != 0 & i < len))
            {
                s = s + (char)a[i + offset];
                i = i + 1;
            }
            return s;
        }


        public static void StrToArray(string s, ref byte[] a, short offset)
        {
            long i = 0;

            for (i = 1; i <= s.Length; i++)
            {
                a[offset + i - 1] = Encoding.ASCII.GetBytes(s.Substring(Convert.ToInt32(i - 1), 1))[0];
            }
            a[offset + s.Length] = 0;
        }


        public static void FormatString(ref System.Windows.Forms.RichTextBox rtext, string WordSearch, Color WordColor, float FontSize, FontStyle Style) // be used in frmSumInfo()
        {
            //format string in RichTextBox
            int FindPosistion = 0;
            System.Drawing.FontStyle newFontStyle = 0;
            System.Drawing.Font currentFont = rtext.SelectionFont;

            // Find WordSearch
            try
            {
                FindPosistion = rtext.Find(WordSearch, 0, RichTextBoxFinds.WholeWord);
            }
            catch (Exception)
            {
                return;
            }

            if (FindPosistion != -1)
            {
                // set color
                rtext.SelectionColor = WordColor;
                // set font style (bold, italic, etc,etc)
                newFontStyle = Style;
                // set text
                rtext.SelectionFont = new Font(currentFont.FontFamily, FontSize, newFontStyle);
            }
        }

        public static double MKT(float[] Data, double div)
        {
            double Tk = 0;
            double sum = 0;

            double e = Math.Exp(1);

            for (int i = 0; i < Data.Length; i++)
            {
                sum = sum + Math.Pow(e, -10000 / ((Data[i] / div) + 273.16));
            }

            Tk = 10000 / -Math.Log(sum / Data.Length);

            return Tk;
        }

        public static double StandartDeviation(float[] Data, double div)
        {
            double SD = 0;

            double AVG = 0;
            double Sum = 0;
            double SumSQRTDelta = 0;

            for (int i = 0; i < Data.Length; i++)
            {
                Sum = Sum + (Data[i] / div);
            }
            AVG = Sum / Data.Length;

            for (int i = 0; i < Data.Length; i++)
            {
                SumSQRTDelta = SumSQRTDelta + ((Data[i] / div) - AVG) * ((Data[i] / div) - AVG);
            }
            SD = Math.Sqrt(SumSQRTDelta / Data.Length);

            return SD;
        }

        public static bool stop { get; set; }
    }
}
