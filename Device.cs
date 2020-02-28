using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Pexo16
{
    public class Device 
    {
        private static Device instance;

        public static  Device Instance
        {
            get
            {
                if(instance == null)
                {
                    if (instance == null)
                    {
                        instance = new Device();
                    }
                }
                return instance;
            }   
        }
        public static Device DelInstance()
        {
            instance = null;
            return instance;
        }

        public Device() {  }

        public int numOfChannel = 0;

        byte[] buf = new byte[500];
        public string hostport;
        private string strStatus;
        public Int64 dev;

        private int lCapFlash;
        public int lDataLength;
        public byte byteLogging;
        private int lRecordNum;

        private string product;
        public string Product
        {
            get { return product; }
            set { product = value; }
        }

        private int delay;
        public int Delay
        {
            get { return delay; }
            set { delay = value; }
        }

        private int duration;
        public int Duration
        {
            get { return duration; }
            set { duration = value; }
        }

        private string ThreadRunning;
        public bool boolWaiting;
        Thread thReadData;
      
        // const CMD
        public int reportID = 2;
        public int CMD_INFO = 9;
        private int CMD_READ_VAL = 1;
        private int CMD_READ_SS_SETTING = 2;
        private int CMD_READ_DATE_SETTING = 3;
        private int CMD_READ_DESC_CH = 23;
        public int CMD_READ = 8;
        private int CMD_READ_SERIAL_NUM = 15;
    

        private int CMD_WRITE_SS_SETTING = 5;
        private int CMD_WRITE_DATE_SETTING = 6;
        private int CMD_WRITE_SERIAL_NUM = 16;
        private int CMD_WRITE_DESC_CH = 24;

        private byte CMD_WAIT = 10;
        private int CMD_ERASELOG = 18;
        private int CMD_STOP_LOGGING = 21;
        private int CMD_START_LOGGING = 22;     

        private byte ERR_OK = 0;

        public Channel[] channels;

        public byte[] ibuf = new byte[1024 * 1024];

        public int ss_index;

        private int startrec;
        public int Startrec
        {
            get { return startrec; }
            set { startrec = value; }
        }

        private string description = "";
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        private string location = "";
        public string Location
        {
            get { return location; }
            set { location = value; }
        }

        private string serial = "";
        public string Serial
        {
            get { return serial; }
            set { serial = value; }
        }

        private string timezone;
        public string Timezone
        {
            get { return timezone; }
            set { timezone = value; }
        }      

        public string Status;
        private bool boolOK = false;
        public long lReceivedByteNum;
   
        //From File///////////////////////////////////////////////////////////////////////////////////////////      
        public byte[] data_open;
        public int offset;
        private int version;
        private string bytenhandang;
        public string titlegraph;
        public string comment;

        public DateTime _logger_date;

        private string interval;
        public string Interval
        {
            get { return interval; }
            set { interval = value; }
        }  

        public float[] da;
        ArrayList listData = new ArrayList();

        //public string getHostPort(ushort vid, ushort pid)
        public string getHostPort(long dev)
        {

            byte[] bufS = new byte[100];
            byte[] bufP = new byte[100];
            byte[] bufA = new byte[100];
            byte[] bufB = new byte[100];
            //dev = HIDFunction.hid_Enumerate(vid, pid);
            if (dev != 0)
            {
                //HIDFunction.hid_GetSerialNumberString(dev, ref buf[0], 10);
                HIDFunction.hid_DeviceSerialNum(dev, ref bufS[0]);
                //if (bufS[0] == 0x00 && bufS[1] == 0x00 && bufS[2] == 0x00)
                //{
                //    Serial = "00000000000";
                //}
                //else
                //{
                    Serial = Encoding.UTF8.GetString(bufS);
                    Serial = Serial.Substring(0, Serial.IndexOf("\0"));
                //}
                    //MessageBox.Show(Serial);



                //HIDFunction.hid_GetSerialNumberString(dev, ref bufA[0], 20);
                //string a = Encoding.UTF8.GetString(bufA);
                //a = a.Substring(0, Serial.IndexOf("\0"));


                //ushort m = HIDFunction.hid_DeviceProductID(dev);


                HIDFunction.hid_DeviceProductString(dev, ref bufP[0]);
                Product = Encoding.UTF8.GetString(bufP);
                Product = Product.Substring(0, Product.IndexOf("\0"));
                //ushort temp = HIDFunction.hid_DeviceVendorID(dev);

                string vid = String.Format("{0:x2}", HIDFunction.hid_DeviceVendorID(dev)).ToUpper();
                string pid = String.Format("{0:x2}", HIDFunction.hid_DeviceProductID(dev)).ToUpper();


                if ((vid == "483" && pid == "5750" && Serial != "(null)" && Product == "(null)") || (vid == "483" && pid == "5750" && Serial == "(null)" && Product != "(null)") || (vid == "483" && pid == "5750" && Serial == "(null)" && Product == "(null)"))
                {
                    HIDFunction.hid_Exit();
                    return "er";
                }
                    hostport = "VID_" + vid;
                    hostport = hostport + "_PID_" + pid;
                    hostport = hostport + "_" + Serial;
                    hostport = hostport + "_" + Product;
            }
            //HIDFunction.hid_FreeEnumeration(dev);
            HIDFunction.hid_Exit();
            return hostport;
        }


        public bool USBOpen(string StrDevs)
        {
            string StrSerial = "";
            string StrVID = "";
            string StrPID = "";
            byte[] buf = new byte[50];

            try
            {
                StrSerial = StrDevs.Substring(17, StrDevs.Substring(17, StrDevs.Length - 17).IndexOf("_"));

                numOfChannel = Int32.Parse(StrSerial.Substring(0, 1));

                StrVID = StrDevs.Substring(4, StrDevs.IndexOf("_PID") - 4);
                string StrDevs1 = StrDevs.Substring(StrDevs.IndexOf("_PID") + 5, StrDevs.Length - StrDevs.IndexOf("_PID") - 5);
                StrPID = StrDevs1.Substring(0, StrDevs1.IndexOf("_"));
            }
            catch (Exception)
            {
                strStatus = "Get string device is error" + "\r\n";
                return false;
            }

            strStatus = "Open device: VID_" + StrVID + "_PID_" + StrPID + " Serial=" + StrSerial + "\r\n";
            buf = Encoding.ASCII.GetBytes(StrSerial);
            try
            {
                dev = HIDFunction.hid_Open(UInt16.Parse(StrVID, NumberStyles.AllowHexSpecifier), UInt16.Parse(StrPID, NumberStyles.AllowHexSpecifier), ref buf[0]);
            }
            catch
            {
                dev = 0;
            }

            if ((dev == 0))
            {
                byte[] msg = new byte[100];

                strStatus += "Unable to open device" + "\r\n";
                return false;
            }
            HIDFunction.hid_SetNonBlocking(dev, 0);
            return true;
        }


        public void Close()
        {
            if (ThreadRunning == "ReadData" & boolWaiting == true)
            {
                thReadData.Abort();
                ThreadRunning = "";
            }

            if (dev != 0)
            {
                HIDFunction.hid_Close(dev);
                dev = 0;
                HIDFunction.hid_Exit();
            }
        }


        public void ReadData_Start()
        {
            if (ThreadRunning == "ReadData" && boolWaiting == true)
            {
                thReadData.Abort();
            }
            ThreadRunning = "ReadData";
            lReceivedByteNum = 0;
            thReadData = new Thread(ReadData_Thread);
            thReadData.IsBackground = true;
            boolOK = false;
            boolWaiting = true;
            thReadData.Start();
        }


        public void ReadData_Thread()
        {
            byte[] buf = new byte[501];
            byte[] msg = new byte[501];
            bool res = false;
            long address = 0;
            long index = 0;

            strStatus = "";
            if (lDataLength == 0)
            {
                strStatus += "lDataLength=0";
                boolOK = false;
                boolWaiting = false;
                return;
            }

            if (dev != 0)
            {
                address = 0;
                do
                {
                    strStatus += "ReadBlockFLASH(" + address + ")" + Environment.NewLine;
                    res = ReadBlockFLASH(address, index);

                    //Dim s As String = "Downloading..." & Math.Round(i / ProgressBar1.Maximum * 100) & "%"
                    lReceivedByteNum = lReceivedByteNum + 128;
                    if (lReceivedByteNum > lDataLength)
                    {
                        lReceivedByteNum = lDataLength;
                    }
                    address = address + 128;
                } while (!((address >= lDataLength) || (res == false))); //Or USB_Stop = True

                boolOK = res;
                boolWaiting = false;
            }
            else
            {
                strStatus = "device = 0" + Environment.NewLine;
                boolOK = false;
                boolWaiting = false;
            }
        }


        private bool ReadBlockFLASH(long address, long offset)
        {
            byte[] buf = new byte[501];
            byte[] msg = new byte[501];
            short res = 0;
            int i = 0;

            long adr2 = 0;
            bool WaitRead = false;

            //==================================================
            // READ 128 Byte Data
            //==================================================
            //'--------------- Send code Read Data--------------
            //Set address -> buf(2..4)
            adr2 = address;
            buf[4] = byte.Parse((adr2 / 65536).ToString());
            adr2 = adr2 % 65536;
            buf[3] = byte.Parse((adr2 / 256).ToString());
            adr2 = adr2 % 256;
            buf[2] = Convert.ToByte(adr2);

            strStatus += "Send_feature(CMD_READ)";
            buf[0] = byte.Parse(reportID.ToString());
            buf[1] = byte.Parse(CMD_READ.ToString()); // code Read_serial +Location1

            if (dev == 0)
            {
                strStatus = "device = 0" + Environment.NewLine;
                return false;
            }
            res = HIDFunction.hid_SendFeatureReport(dev, ref buf[0], 128 + 5);
            if (res < 0)
            {
                strStatus += " error(res=" + res + ")" + "\r\n";
                return false;
            }
            strStatus += " ok" + Environment.NewLine;

            do
            {
                //Delay
                System.Threading.Thread.Sleep(5);

                //'--------------- Get feature - Read_Serial--------------
                strStatus += "Get feature";
                //Read Data in
                buf[0] = byte.Parse(reportID.ToString());

                if (dev == 0)
                {
                    strStatus = "device = 0" + Environment.NewLine;
                    return false;
                }
                res = HIDFunction.hid_GetFeatureReport(dev, ref buf[0], 255); //res = 7
                if (res < 0)
                {
                    strStatus += " error (res=" + res + ")" + "\r\n";
                    HIDFunction.hid_Error(dev, ref msg[0]);
                    strStatus += "Error= " + msg.ToString() + "\r\n";
                    //WaitRead = False
                    return false;
                }
                else
                {
                    strStatus += " ok" + Environment.NewLine;
                    if (buf[1] == CMD_READ)
                    {
                        WaitRead = false;
                        try
                        {
                            // Print out the returned buffer.
                            for (i = 0; i < 128; i++)
                            {
                                ibuf[i + address] = buf[i + 5];
                                mGlobal.ibuf_read[i + address] = buf[i + 5];
                                
                            }
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("a");
                        }
                    }
                    else
                    {
                        if (buf[1] != CMD_WAIT)
                        {
                            strStatus += " error(res=" + res + ")" + "\r\n";
                            return false;
                        }
                        else
                        {
                            WaitRead = true;
                        }
                    }
                }
                //==================================================
                // END READ 128 Byte Data
                // WaitRead
                //==================================================
            } while (!(WaitRead == false));

            return true;
        }


        public bool Read_DataLength()
       {
            byte[] buf = new byte[500];
            byte[] msg = new byte[500];
            short res;

            if (dev != 0)
            {
                //--------------- Send code Read_Serial--------------
                strStatus = "Send_feature(CMD_INFO)";
                buf[0] = byte.Parse(reportID.ToString());
                buf[1] = byte.Parse(CMD_INFO.ToString());
                // code Read_serial +Location1
                res = HIDFunction.hid_SendFeatureReport(dev, ref buf[0], 128 + 5);
                //res = 133;
                if (res < 0)
                {
                    strStatus += " error(res=" + res + ")" + "\r\n";
                    return false;
                }
                strStatus += " ok" + "\r\n";

                //--------------- Get feature - Read_Serial--------------
                strStatus += "Get feature";
                //Read Data in
                buf[0] = byte.Parse(reportID.ToString());
                res = HIDFunction.hid_GetFeatureReport(dev, ref buf[0], 255);
                //res = 7; // res = 25;
                if ((res < 0))
                {
                    strStatus += " error (res=" + res + ")" + "\r\n";
                    HIDFunction.hid_Error(dev, ref msg[0]);
                    strStatus += "Error= " + msg.ToString() + "\r\n";
                    return false;
                }
                strStatus += " ok" + "\r\n";

                lCapFlash = buf[7] * 65536;
                lCapFlash = lCapFlash + buf[6] * 256;
                lCapFlash = lCapFlash + buf[5];

                lDataLength = buf[10] * 65536;
                lDataLength = lDataLength + buf[9] * 256;
                lDataLength = lDataLength + buf[8];
                byteLogging = buf[11];

                lRecordNum = (lDataLength - 128) / 16;

                return true;
            }
            else
            {
                strStatus = "device = 0" + "\r\n";
                return false;
            }
        }


        public bool Read_setting()
        {
            try
            {
                byte[] buf = new byte[501];
                byte[] msg = new byte[501];
                short res = 0;
                short i = 0;

                if (dev == 0)
                {
                    strStatus = "device = 0" + "\r\n";
                    return false;
                }

                //--------------- Send code Read_Serial--------------
                buf[0] = byte.Parse(reportID.ToString());
                buf[1] = byte.Parse(CMD_READ_SS_SETTING.ToString());
                // code Read_SS
                strStatus = "send_feature(CMD_READ_SS_SETTING)";
                res = HIDFunction.hid_SendFeatureReport(dev, ref buf[0], 128 + 5);
                if (res < 0)
                {
                    strStatus += " error(res=" + res + ")" + "\r\n";
                    return false;
                }
                strStatus += " ok" + "\r\n";

                //--------------- Get feature - Read_Serial--------------
                //// u8 ID, u8 CMD,	
                //// u8	signal[2], u8 eth_addr[6], u8 enable_dhcp;
                //// u8 ip_addr[4], u8 net_mask[4], u8 gateway[4]
                //// u8 security_type, u8 ssid[32+1], u8 security_passphrase[64+1]
                strStatus += "Get feature";
                //Read Sensor
                buf[0] = byte.Parse(reportID.ToString());
                res = HIDFunction.hid_GetFeatureReport(dev, ref buf[0], 255);
                //res = 7

                if ((res < 0))
                {
                    strStatus += " error (res=" + res + ")" + "\r\n";
                    HIDFunction.hid_Error(dev, ref msg[0]);
                    strStatus += "Error= " + msg.ToString() + "\r\n";
                    return false;
                }
                else
                {
                    //// u8 ID, u8 CMD,	
                    //// u8 Unit, u8 DivNum, i16 Val, i8 Alarm, i8 __nop, i16 MAX, i16 MIN
                    //// X 8
                    for (i = 0; i < numOfChannel; i++)
                    {
                        //channels[i] = new Channel();
                        channels[i].Unit = buf[2 + i * 10];
                        channels[i].DivNum = buf[2 + i * 10 + 1];
                        if(channels[i].DivNum == 0)
                        {
                            channels[i].DivNum = 1;
                        }
                        channels[i].AlarmMax = buf[2 + i * 10 + 6] + buf[2 + i * 10 + 7] * 256;

                        //xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
                        if ((channels[i].AlarmMax & 32768) == 32768)
                            channels[i].AlarmMax = channels[i].AlarmMax - 65536;
                        if (channels[i].AlarmMax == 30000)
                        {
                            channels[i].NoAlarm = true;
                        }
                        else
                        {
                            channels[i].NoAlarm = false;
                            channels[i].AlarmMax = channels[i].AlarmMax / channels[i].DivNum; //10
                        }
                        //xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                        //xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
                        channels[i].AlarmMin = buf[2 + i * 10 + 8] + buf[2 + i * 10 + 9] * 256;

                        if ((channels[i].AlarmMin & 32768) == 32768)
                            channels[i].AlarmMin = channels[i].AlarmMin - 65536;
                        if (channels[i].AlarmMin == -30000)
                        {
                            channels[i].NoAlarm = true;
                        }
                        else
                        {
                            channels[i].NoAlarm = false;
                            channels[i].AlarmMin = channels[i].AlarmMin / channels[i].DivNum;
                            ////10
                        }
                    }
                    Startrec = buf[83] * 256 + buf[82];
                    if (Startrec != 0)
                        Startrec = Startrec / 60;
                    Duration = buf[85] * 256 + buf[84];
                }
                return true;
            }
            catch (Exception)
            {
                MessageBox.Show("Read_setting false. status=" + strStatus);
                return false;
            }
        }


        public bool readLocationSerialDescription()
        {
            byte[] buf = new byte[501];
            byte[] msg = new byte[501];
            short res = 0;

            if (dev != 0)
            {
                //--------------- Send code Read_Serial--------------
                strStatus = "Send_feature(CMD_READ_SERIAL_NUM)";
                buf[0] = byte.Parse(reportID.ToString());
                buf[1] = byte.Parse(CMD_READ_SERIAL_NUM.ToString());
                // code Read_serial +Location1
                res = HIDFunction.hid_SendFeatureReport(dev, ref buf[0], 128 + 5);
                if (res < 0)
                {
                    strStatus += " error(res=" + res + ")" + Environment.NewLine;
                    return false;
                }
                strStatus += " ok" + Environment.NewLine;

                //--------------- Get feature - Read_Serial--------------
                strStatus += "Get feature";
                //Read Data in
                buf[0] = byte.Parse(reportID.ToString());
                res = HIDFunction.hid_GetFeatureReport(dev, ref buf[0], 255);
                //res = 7
                if ((res < 0))
                {
                    strStatus += " error (res=" + res + ")" + "\r\n";
                    HIDFunction.hid_Error(dev, ref msg[0]);
                    strStatus += "Error= " + msg.ToString() + "\r\n";
                    return false;
                }
                strStatus += " ok" + Environment.NewLine;
                //// u8 ID, u8 CMD,	
                //// u10 Serial, another is Location1
                //// X 8
                Serial = mGlobal.ArrayToStr(ref buf, 2, 10);
                numOfChannel = Int32.Parse(Serial.Substring(0, 1));
                Location = mGlobal.ArrayToStr(ref buf, 12, 40);
                Description = mGlobal.ArrayToStr(ref buf, 52, 40);

                return true;
            }
            else
            {
                strStatus = "device = 0" + Environment.NewLine;
                return false;
            }
        }


        public bool readDescriptionChannel()
        {
            byte[] buf = new byte[501];
            byte[] msg = new byte[501];
            short res = 0;
            byte i = 0;

            //u8 ID, u8 CMD
            //u8 channel_num (0-7)
            //u8 DescCH[50]

            if (dev != 0)
            {
                for (i = 0; i < numOfChannel; i++)
                {
                    //--------------- Send code Read_Description Channel--------------

                    strStatus = "Send_feature(CMD_READ_DESC_CH)";
                    buf[0] = byte.Parse(reportID.ToString());
                    buf[1] = byte.Parse(CMD_READ_DESC_CH.ToString());//23
                    buf[2] = i;
                    // code Read_serial +Location1
                    res = HIDFunction.hid_SendFeatureReport(dev, ref buf[0], 128 + 5);
                   // strStatus = "Send_feature(CMD_READ_DESC_CH)";
                    // code Read Description1 Channel
                    
                    // Channel number
                    if (res < 0)
                    {
                        strStatus += " error(res=" + res + ")" + Environment.NewLine;
                        return false;
                    }
                    strStatus += " ok" + Environment.NewLine;

                    //--------------- Get feature - Read_Serial--------------
                    strStatus += "Get feature";
                    //Read Data in
                    buf[0] = byte.Parse(reportID.ToString());
                    res = HIDFunction.hid_GetFeatureReport(dev, ref buf[0], 255);
                    //res = 7
                    if ((res < 0))
                    {
                        strStatus += " error (res=" + res + ")" + "\r\n";
                        HIDFunction.hid_Error(dev, ref msg[0]);
                        strStatus += "Error= " + msg.ToString() + "\r\n";
                        return false;
                    }
                    strStatus += " ok" + Environment.NewLine;
                    //u8 ID, u8 CMD
                    //u8 channel_num (0-7)
                    //u8 DescCH[50]
                    string DescTmp = null;
                    DescTmp = mGlobal.ArrayToStr(ref buf, 3, 50);
                    channels[i].Desc = DescTmp;
                }
                return true;
            }
            else
            {
                strStatus = "device = 0" + Environment.NewLine;
                return false;
            }
        }


        public bool ReadDateTime()
        {
            byte[] buf = new byte[501];
            byte[] msg = new byte[501];
            short res = 0;

            if (dev != 0)
            {
                buf[0] = byte.Parse(reportID.ToString());
                buf[1] = byte.Parse(CMD_READ_DATE_SETTING.ToString());

                strStatus = "Send_feature(CMD_READ_DATE_SETTING)";
                res = HIDFunction.hid_SendFeatureReport(dev, ref buf[0], 128 + 5);
                if (res < 0)
                {
                    strStatus += "error (res=" + res + "\r\n";
                    return false;
                }
                strStatus += " Ok" + "\r\n";

                //-----------------
                strStatus += "Get Feature";
                buf[0] = byte.Parse(reportID.ToString());
                res = HIDFunction.hid_GetFeatureReport(dev, ref buf[0], 255);

                if (res < 0)
                {
                    strStatus += " error (res=" + res + ")" + "\r\n";
                    HIDFunction.hid_Error(dev, ref msg[0]);
                    strStatus += "Error= " + msg.ToString() + "\r\n";
                    return false;
                }

                Timezone = mGlobal.ArrayToStr(ref buf, 10, 22);
                Timezone = Timezone.Trim();
                strStatus = " Ok" + "\r\n";
                return true;
            }
            else
            {
                strStatus = "device = 0" + "\r\n";
                return false;
            }
        }


        public bool Read_Sensor()
        {
            byte[] buf = new byte[501];
            byte[] msg = new byte[501];
            short res = 0;
            byte i = 0;
            int j = 0;

            if (dev != 0)
            {
                //--------------- Send code Read_Serial--------------
                strStatus = "Send_feature(CMD_READ_VAL)";
                buf[0] = byte.Parse(reportID.ToString());
                buf[1] = byte.Parse(CMD_READ_VAL.ToString()); // code CMD_READ_VAL
                res = HIDFunction.hid_SendFeatureReport(dev, ref buf[0], 128 + 5);
                if (res < 0)
                {
                    strStatus += " error(res=" + res + ")" + Environment.NewLine;
                    return false;
                }
                strStatus += " ok" + Environment.NewLine;

                //--------------- Get feature - Read_Serial--------------
                strStatus += "Get feature";
                //Read Data in
                //Thread.Sleep(100);
                buf[0] = byte.Parse(reportID.ToString());
                res = HIDFunction.hid_GetFeatureReport(dev, ref buf[0], 255); //res = 7
                if (res < 0)
                {
                    strStatus += " error (res=" + res + ")" + Environment.NewLine;
                    HIDFunction.hid_Error(dev, ref msg[0]);
                    strStatus += "Error= " + msg.ToString() + Environment.NewLine;
                    return false;
                }
                strStatus += " ok" + Environment.NewLine;

                //// u8 ID, u8 CMD,	
                j = buf[2] + buf[3] * 256 + buf[4] * 65536 + buf[5] * 16777216;
                ss_index = j;

                // Note : Number -/+
                for (i = 0; i < numOfChannel; i++)
                {
                    j = buf[6 + i * 2] + buf[7 + i * 2] * 256;
                    if ((j & 32768) == 32768)
                    {
                        j = j - 65536;
                    }
                    //channels[i].Val = (double)j;
                    channels[i].Val = (double)j / channels[i].DivNum;
                }
                return true;
            }
            else
            {
                strStatus = "device = 0" + Environment.NewLine;
                return false;
            }
        }


        public bool WriteDateTime(DateTime LoggerTime, string ZoneName)
        {
            //Write Date
            byte[] buf = new byte[501];
            byte[] msg = new byte[501];
            short res = 0;

            byte day = 0;
            byte month = 0;
            int year = 0;
            byte hour = 0;
            byte min = 0;
            byte sec = 0;

            if (dev != 0)
            {
                day = Convert.ToByte(LoggerTime.ToString("dd"));
                month = Convert.ToByte(LoggerTime.ToString("MM"));
                year = Convert.ToInt32(LoggerTime.ToString("yyyy"));
                hour = Convert.ToByte(LoggerTime.ToString("HH"));
                min = Convert.ToByte(LoggerTime.ToString("mm"));
                sec = Convert.ToByte(LoggerTime.ToString("ss"));


                // Send code Write_Date
                //// u8 ID, u8 CMD,	
                //// u8 day, u8 month, u16 year
                //// u8 hour, u8 min, u16 sec
                //// u8 TimeZone[22]
                buf[0] = byte.Parse(reportID.ToString());
                buf[1] = byte.Parse(CMD_WRITE_DATE_SETTING.ToString()); // code Write_Date
                buf[2] = day;
                buf[3] = month;
                buf[4] = byte.Parse((year % 256).ToString());

                buf[5] = byte.Parse((year / 256).ToString());
                buf[6] = hour;
                buf[7] = min;
                buf[9] = 0;
                buf[8] = sec;

                if (ZoneName.Length > 39)
                {
                    ZoneName = ZoneName.Substring(0, 39);
                }
                mGlobal.StrToArray(ZoneName, ref buf, 10);

                strStatus = "send_feature(CMD_WRITE_DATE_SETTING)";
                res = HIDFunction.hid_SendFeatureReport(dev, ref buf[0], 128 + 5);
                if (res < 0)
                {
                    strStatus += "error (res=" + res + Environment.NewLine;
                    HIDFunction.hid_Error(dev, ref  msg[0]);
                    MessageBox.Show(msg.ToString());
                    strStatus += "Error = " + msg.ToString() + Environment.NewLine;
                    return false;
                }
                strStatus += " ok" + Environment.NewLine;
                boolOK = true;
                return true;
            }
            else
            {
                strStatus = "device = 0" + Environment.NewLine;
                return false;
            }
        }


        public bool Write_location_serial_description()
        {
            byte[] buf = new byte[500];
            byte[] msg = new byte[500];
            short res = 0;
            int i = 0;

            if (dev == 0)
            {
                strStatus = "device = 0" + Environment.NewLine;
                return false;
            }

            buf[0] = byte.Parse(reportID.ToString());
            buf[1] = byte.Parse(CMD_WRITE_SERIAL_NUM.ToString());
            //--------------Serial 10 bytes-----
            for (i = 0; i < Serial.Length; i++)
            {
                if (i < 10)
                {
                    buf[i + 2] = Encoding.ASCII.GetBytes(Serial.Substring(i, 1))[0];
                }
            }
            //------Location1 40 bytes--------
            for (i = 0; i < Location.Length; i++)
            {
                if (i < 40)
                {
                    buf[i + 12] = Encoding.ASCII.GetBytes(Location.Substring(i, 1))[0];
                }
            }
            //------Description1 40 bytes--------
            for (i = 0; i < Description.Length; i++)
            {
                if (i < 40)
                {
                    buf[i + 12 + 40] = Encoding.ASCII.GetBytes(Description.Substring(i, 1))[0];
                }
            }

            strStatus = "send_feature(CMD_WRITE_SERIAL_NUM)";
            res = HIDFunction.hid_SendFeatureReport(dev, ref buf[0], 128 + 5);

            if (res < 0)
            {
                strStatus += "error (res=" + res + Environment.NewLine;
                HIDFunction.hid_Error(dev, ref msg[0]);
                MessageBox.Show(msg.ToString());
                strStatus += "Error = " + msg.ToString() + Environment.NewLine;
                return false;
            }
            return true;
        }


        public bool Write_Description_Channel()
        {
            byte[] buf = new byte[501];
            byte[] msg = new byte[501];
            short res = 0;
            int i = 0;
            int channel = 0;

            if (dev == 0)
            {
                strStatus = "device = 0" + Environment.NewLine;
                return false;
            }

            for (channel = 0; channel < numOfChannel; channel++)
            {
                buf[0] = byte.Parse(reportID.ToString());
                buf[1] = byte.Parse(CMD_WRITE_DESC_CH.ToString());
                buf[2] = Convert.ToByte(channel);

                //--------------Desc Channel 50 bytes-----
                string desctmp = null;
                if(channels[channel].Desc == null)
                {
                    channels[channel].Desc = "";
                }
                desctmp = channels[channel].Desc;
                for (i = 0; i < desctmp.Length; i++)
                {
                    if (i < 50)
                    {
                        buf[i + 3] = Encoding.ASCII.GetBytes(desctmp.Substring(i, 1))[0];
                    }
                }
                buf[desctmp.Length + 3] = 0;

                strStatus = "send_feature(CMD_WRITE_DESC_CH)";
                res = HIDFunction.hid_SendFeatureReport(dev, ref buf[0], 128 + 5);

                if (res < 0)
                {
                    strStatus += "error (res=" + res + Environment.NewLine;
                    HIDFunction.hid_Error(dev, ref msg[0]);
                    MessageBox.Show(msg.ToString());
                    strStatus += "Error = " + msg.ToString() + Environment.NewLine;
                    return false;
                }
            }
            return true;
        }


        public bool Write_setting()
        {
            byte[] buf = new byte[500];
            byte[] msg = new byte[500];
            short res = 0;
            int tmp = 0;
            int i = 0;

            if (dev == 0)
            {
                strStatus = "device = 0" + Environment.NewLine;
                return false;
            }

            // Send code Write_SS
            //// u8 ID, u8 CMD,	
            //// u8 Unit, u8 DivNum, i16 Val, i8 Alarm, i8 __nop, i16 MAX, i16 MIN
            //// X 8
            for (i = 0; i < numOfChannel; i++)
            {
                buf[2 + i * 10] = channels[i].Unit;
                buf[2 + i * 10 + 1] = channels[i].DivNum; // Added 08/07/2013

                //xxxxxxxxxxxxxxxxxxxxxxxxxx
                if (channels[i].NoAlarm == true)
                {
                    tmp = 30000;
                }
                else
                {
                    tmp = channels[i].AlarmMax * channels[i].DivNum; //10
                }
                //xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                if (tmp < 0)
                {
                    tmp = (65536 + tmp);
                }
                buf[2 + i * 10 + 6] = (byte)(tmp % 256);
                buf[2 + i * 10 + 7] = byte.Parse((tmp / 256).ToString());

                //xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
                if (channels[i].NoAlarm == true)
                {
                    tmp = -30000;
                }
                else
                {
                    tmp = channels[i].AlarmMin * channels[i].DivNum; //10
                }
                //xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                if (tmp < 0)
                {
                    tmp = (65536 + tmp);
                }
                buf[2 + i * 10 + 8] = (byte)(tmp % 256);
                buf[2 + i * 10 + 9] = byte.Parse((tmp / 256).ToString());
            }
            //ghi them start va Duration 
            Startrec = Startrec * 60;
            buf[82] = (byte)(Startrec % 256);
            buf[83] = byte.Parse(Math.Truncate((double)Startrec / 256).ToString());
            buf[84] = (byte)(Duration % 256);
            buf[85] = byte.Parse(Math.Truncate((double)Duration / 256).ToString());

            buf[0] = byte.Parse(reportID.ToString()); //reportID
            buf[1] = byte.Parse(CMD_WRITE_SS_SETTING.ToString()); // code Write_SS
            strStatus = "send_feature(CMD_WRITE_SS_SETTING)";
            res = HIDFunction.hid_SendFeatureReport(dev, ref buf[0], 128 + 5);
            if (res < 0)
            {
                strStatus += "error (res=" + res + "\r\n";
                HIDFunction.hid_Error(dev, ref msg[0]);
                MessageBox.Show(msg.ToString());
                strStatus += "Error = " + msg.ToString() + "\r\n";
                return false;
            }
            strStatus += " ok" + "\r\n";
            return true;
        }


        public bool StartLogging()
        {
            byte[] buf = new byte[500];
            byte[] msg = new byte[500];
            short res = 0;

            if (dev == 0)
            {
                return false;
            }

            strStatus += "Send_feature(CMD_START_LOGGING)";
            buf[0] = byte.Parse(reportID.ToString());
            buf[1] = byte.Parse(CMD_START_LOGGING.ToString()); // code Read_serial +Location1
            res = HIDFunction.hid_SendFeatureReport(dev, ref buf[0], 128 + 5);
            if (res < 0)
            {
                strStatus += " error(res=" + res + ")" + Environment.NewLine;
                return false;
            }
            strStatus += " ok" + Environment.NewLine;


            //Delay
            System.Threading.Thread.Sleep(5);
            //--------------- Get feature - Read_Serial--------------
            strStatus += "Get feature";
            //Read Data in
            buf[0] = byte.Parse(reportID.ToString());
            res = HIDFunction.hid_GetFeatureReport(dev, ref buf[0], 255);//res = 133
            if (res < 0)
            {
                strStatus += " error (res=" + res + ")" + Environment.NewLine;
                HIDFunction.hid_Error(dev, ref msg[0]);
                strStatus += "Error= " + msg.ToString() + Environment.NewLine;
                //WaitRead = False
                return false;
            }
            else
            {
                strStatus += " ok" + Environment.NewLine;
                if (buf[1] == CMD_START_LOGGING && buf[2] == ERR_OK) //ERR_OK Then
                {
                    return true;
                }
                else
                {
                    strStatus += " error(res=" + res + ")" + Environment.NewLine;
                    return false;
                }
            }
        }


        public bool StopLogging()
        {
            byte[] buf = new byte[501];
            byte[] msg = new byte[501];
            short res = 0;

            if (dev == 0)
            {
                return false;
            }

            strStatus += "Send_feature(CMD_STOP_LOGGING)";
            buf[0] = byte.Parse(reportID.ToString());
            buf[1] = byte.Parse(CMD_STOP_LOGGING.ToString()); // code Read_serial +Location1 byte.Parse(CMD_STOP_LOGGING.ToString())
            res = HIDFunction.hid_SendFeatureReport(dev, ref buf[0], 128 + 5);
            if (res < 0)
            {
                strStatus += " error(res=" + res + ")" + Environment.NewLine;
                return false;
            }
            strStatus += " ok" + Environment.NewLine;


            //Delay
            System.Threading.Thread.Sleep(5);
            //'--------------- Get feature - Read_Serial--------------
            strStatus += "Get feature";
            //Read Data in
            buf[0] = byte.Parse(reportID.ToString());
            res = HIDFunction.hid_GetFeatureReport(dev, ref buf[0], 255); //res = 7
            if (res < 0)
            {
                strStatus += " error (res=" + res + ")" + "\r\n";
                HIDFunction.hid_Error(dev, ref msg[0]);
                strStatus += "Error= " + msg.ToString() + "\r\n";
                return false;
            }
            else
            {
                strStatus += " ok" + Environment.NewLine;
                if (buf[1] == CMD_STOP_LOGGING && buf[2] == ERR_OK) //ERR_OK Then
                {
                    return true;
                }
                else
                {
                    strStatus += " error(res=" + res + ")" + "\r\n";
                    return false;
                }
            }
        }


        public bool EraseFLASH()
        {
            byte[] buf = new byte[501];
            byte[] msg = new byte[501];
            short res = 0;

            bool WaitRead = false;

            if (dev == 0)
            {
                //strStatus = "device = 0" & vbCrLf
                return false;
            }

            strStatus += "Send_feature(CMD_ERASELOG)";
            buf[0] = byte.Parse(reportID.ToString());
            buf[1] = byte.Parse(CMD_ERASELOG.ToString()); // code Read_serial +Location1
            res = HIDFunction.hid_SendFeatureReport(dev, ref buf[0], 128 + 5);
            if (res < 0)
            {
                strStatus += " error(res=" + res + ")" + Environment.NewLine;
                return false;
            }
            strStatus += " ok" + Environment.NewLine;

            do
            {
                //Delay
                System.Threading.Thread.Sleep(5);

                //--------------- Get feature - Read_Serial--------------
                strStatus += "Get feature";
                //Read Data in
                buf[0] = byte.Parse(reportID.ToString());
                res = HIDFunction.hid_GetFeatureReport(dev, ref buf[0], 255); //res = 7
                if (res < 0)
                {
                    strStatus += " error (res=" + res + ")" + Environment.NewLine;
                    HIDFunction.hid_Error(dev, ref msg[0]);
                    strStatus += "Error= " + msg.ToString() + Environment.NewLine;
                    return false;
                }
                else
                {
                    strStatus += " ok" + Environment.NewLine;
                    if (buf[2] == 1) //ERR_OK Then
                    {
                        if (buf[1] == CMD_ERASELOG)
                        {
                            WaitRead = true;
                        }
                        else
                        {
                            strStatus += " error(res = " + res + ")" + Environment.NewLine;
                            return false;
                        }
                    }
                    else
                    {
                        WaitRead = false;
                    }
                }

            } while (!(WaitRead == false));

            return true;
        }


        public bool OpenFile_MP_Lgr(string path)
        {
            try { data_open = File.ReadAllBytes(path); }
            catch (Exception)
            {
                MessageBox.Show("Open file fail");
                return false;
            }
            if (data_open.Count() <= 1)
            {
                MessageBox.Show("Du lieu rong");
                return false;
            }

            //enableToolButton(true);

            offset = data_open[0] + data_open[1] * 256;
            version = data_open[2];

            bytenhandang = "";
            byte[] temp = new byte[6];

            for (int i = 0; i <= 5; i++)
            {
                temp[i] = data_open[i + 3];
            }
            bytenhandang += Encoding.UTF8.GetString(temp);
            if (bytenhandang != "pexo16")
            {
                MessageBox.Show("file incorrect");
                return false;
            }

            serial = "";
            byte[] temp1 = new byte[10];
            for (int i = 0; i <= 9; i++)
            {
                temp1[i] = data_open[10 + i];
            }
            serial += Encoding.UTF8.GetString(temp1);
            numOfChannel = Int32.Parse(serial.Substring(0, 1));
            channels = new Channel[numOfChannel];
            for (int i = 0; i < numOfChannel; i++)
            {
                channels[i] = new Channel();
            }

            location = "";
            byte[] temp2 = new byte[40];
            for (int i = 0; i <= 39; i++)
            {
                if (data_open[20 + i] != 0)
                    temp2[i] = data_open[20 + i];
            }
            location += Encoding.UTF8.GetString(temp2);
            location = location.Trim();
            if (location.IndexOf("\0") != -1)
            {
                location = location.Substring(0, location.IndexOf("\0"));
            }

            description = "";
            byte[] temp3 = new byte[40];
            for (int i = 0; i <= 39; i++)
            {
                if (data_open[60 + i] != 0)
                {
                    temp3[i] = data_open[60 + i];
                }
            }
            description += Encoding.UTF8.GetString(temp3);
            description = description.Trim();
            if (description.IndexOf("\0") != -1)
            {
                description = description.Substring(0, description.IndexOf("\0"));
            }

            delay = data_open[131];
            duration = data_open[132] + data_open[133] * 256;

            for (int i = 0; i < numOfChannel; i++)
            {
                channels[i].Unit = data_open[140 + i];
            }


            for (int i = 0; i < numOfChannel * 2; i += 2)
            {
                int a1 = data_open[150 + i] + data_open[150 + 1 + i] * 256;
                if ((a1 & 32768) == 32768)
                {
                    a1 = a1 - 65536;
                }
                channels[i / 2].AlarmMax = a1;
            }

            //----min 
            for (int i = 0; i < numOfChannel * 2; i += 2)
            {
                int a1 = data_open[170 + i] + data_open[170 + 1 + i] * 256;
                if ((a1 & 32768) == 32768)
                {
                    a1 = a1 - 65536;
                }
                channels[i / 2].AlarmMin = a1;
            }

            //---color read logger
            for (int i = 0; i < numOfChannel * 3; i += 3)
            {
                channels[i / 3].LineColor = Color.FromArgb(data_open[440 + i], data_open[440 + 1 + i], data_open[440 + 2 + i]);
            }

            mGlobal.data_byte = new byte[data_open.Length - 1];
            for (int i = 0; i <= data_open.Length - 2; i++)
            {
                mGlobal.data_byte[i] = data_open[i];
            }

            byte[] temp4 = new byte[50];
            titlegraph = "";
            int dem = 0;
            for (int i = 0; i <= 49; i++)
            {
                if (mGlobal.data_byte[190 + i] != 0)
                {
                    temp4[i] = mGlobal.data_byte[190 + i];
                    dem = i + 1;
                }
            }
            titlegraph += Encoding.UTF8.GetString(temp4);
            titlegraph = titlegraph.Trim();
            titlegraph = titlegraph.Substring(0, dem);

            comment = "";
            dem = 0;
            byte[] temp5 = new byte[200];
            for (int i = 0; i <= 199; i++)
            {
                if (mGlobal.data_byte[240 + i] != 0)
                {
                    temp5[i] = mGlobal.data_byte[240 + i];
                    dem = i + 1;
                }
            }
            comment = Encoding.UTF8.GetString(temp5).Trim();
            comment = comment.Substring(0, dem);

            mGlobal.open_file = true;

            if (offset == 0)
            {
                offset = 200;
                titlegraph = "";
                comment = "";
                for (int i = 0; i < numOfChannel; i++)
                {
                    channels[i].LineColor = Color.Empty;
                }
            }
            return true;
        }


        public void SaveFile_MP_Lgr2(ref byte[] data_save)
        {
            // save Data version v01
            int offset = 470;
            int version = 1; //v01
            data_save = new byte[offset + channels[0].Data.Length * 16];

            //---------- 2byte Data offset and 1 byte version Data
            data_save[0] = (byte)(offset % 256); 
            data_save[1] = Convert.ToByte(offset / 256);

            data_save[2] = Convert.ToByte(version);

            //pexo16 - nhan dang file (byte 3-8)
            for (int i = 0; i <= 5; i++)
            {
                data_save[i + 3] = Encoding.ASCII.GetBytes("pexo16".Substring(i, 1))[0];
            }

            //-----------10 byte Serial
            for (int i = 0; i <= 9; i++)
            {
                data_save[10 + i] = Encoding.ASCII.GetBytes(Serial.Substring(i, 1))[0];
            }
            //-----------40 byte Location
            for (int i = 0; i < Location.Length; i++)
            {
                data_save[20 + i] = Encoding.ASCII.GetBytes(Location.Substring(i, 1))[0];
            }

            //-----------40 byte Description1
            for (int i = 0; i < Description.Length; i++)
            {
                data_save[60 + i] = Encoding.ASCII.GetBytes(Description.Substring(i, 1))[0];           
            }

            //-----------20 byte Timezone record
            string a = "";
            a = mGlobal.FindTimeZoneByID(Timezone.ToString()).Id.ToString();
            a = Simulate.LSet(a, 15) + Simulate.RSet(a.Substring(Math.Max(a.Length - 2, 1) - 1), 3);
            for (int i = 0; i < a.Length; i++)
            {
                data_save[100 + i] = Encoding.ASCII.GetBytes(a.Substring(i, 1))[0];
            }
          
          
            //-----------7 byte datatime record
            DateTime b = _logger_date;
            byte day = Convert.ToByte(b.ToString("dd"));
            byte month = Convert.ToByte(b.ToString("MM"));
            int year = Convert.ToInt32(b.ToString("yyyy"));
            byte hour = Convert.ToByte(b.ToString("HH"));
            byte min = Convert.ToByte(b.ToString("mm"));
            byte sec = Convert.ToByte(b.ToString("ss"));

            try
            {
                data_save[120] = day;
                data_save[121] = month;
                data_save[122] = (byte)(year % 256);
                data_save[123] = Convert.ToByte(year / 256);
                data_save[124] = hour;
                data_save[125] = min;
                data_save[126] = sec;
            }
            catch (Exception)
            {
                MessageBox.Show("Save DateTime fail");
                return;
            }

            //-----------1 byte Delay
            data_save[131] = Convert.ToByte(Delay);

            //-----------2 byte Duration
            data_save[132] = (byte)(Convert.ToInt32(Duration) % 256);
            data_save[133] = Convert.ToByte(Convert.ToInt32(Duration) / 256);

            //-----------8 byte Unit
            if (mGlobal.unitFromFile == true)
            {
                for (int i = 0; i < numOfChannel; i++)
                {
                    data_save[140 + i] = mGlobal.unitTemp[i];
                }
            }
            else//Unit of device
            {
                for (int i = 0; i < numOfChannel; i++)
                {
                    data_save[140 + i] = channels[i].Unit;
                }
            }           

            //-----------16 byte max
            for (int i = 0; i < 2*numOfChannel; i += 2)
            {
                int tmp = channels[i / 2].AlarmMax;
                if (tmp < 0)
                {
                    tmp = (65536 + tmp);
                }
                data_save[150 + i] = (byte)(tmp % 256);
                data_save[150 + 1 + i] = Convert.ToByte(tmp / 256);
            }

            //-----------16 byte min
            for (int i = 0; i < 2*numOfChannel; i += 2)
            {
                int tmp = channels[i / 2].AlarmMin;
                if (tmp < 0)
                {
                    tmp = (65536 + tmp);
                }
                data_save[170 + i] = (byte)(tmp % 256);
                data_save[170 + i + 1] = Convert.ToByte(tmp / 256);
            }


            //----------- 50 byte title Graph
            for (int i = 0; i < mGlobal.TitleGraph.Length; i++)
            {
                data_save[190 + i] = Encoding.ASCII.GetBytes(mGlobal.TitleGraph.Substring(i, 1))[0];
            }

            //-----------200 byte comment
            if (comment != null)
            {
                for (int i = 0; i < comment.Length; i++)
                {
                    data_save[240 + i] = Encoding.ASCII.GetBytes(comment.Substring(i, 1))[0];
                }
            }

            //----------- 24 byte color line
            //line 1
            if (numOfChannel > 0)
            {
                for (int i = 0; i < 3 * numOfChannel; i += 3)
                {
                    data_save[440 + i] = Convert.ToByte(channels[i / 3].LineColor.R.ToString());
                    data_save[440 + 1 + i] = Convert.ToByte(channels[i / 3].LineColor.G.ToString());
                    data_save[440 + 2 + i] = Convert.ToByte(channels[i / 3].LineColor.B.ToString());
                }
            }

            //--------------Data (offset Data = 430)

            for (int k = 0; k < numOfChannel; k++)
            {
                for (int i = 0; i < channels[k].Data.Length; i++)
                {
                    double temp = 0;
                    temp = Convert.ToDouble(channels[k].Data[i]) * 10;
                    if (temp < 0)
                    {
                        temp = temp + 1 + 65535;
                    }
                    data_save[offset + 2 * k + i * 16] = (byte)(temp % 256);
                    data_save[offset + 2 * k + i * 16 + 1] = Convert.ToByte((int)(temp / 256));
                }
            }
        }



        internal void saveSetting(ref byte[] data_save)
        {
            //for (int i = 0; i < Serial.Length; i++)
            //{
            //    if (Serial != "")
            //    {
            //        data_save[i] = Encoding.ASCII.GetBytes(Serial.Substring(i, 1))[0];
            //    }
            //}
            //-----------40 byte Location
            for (int i = 0; i < Location.Length; i++)
            {
                if (Location != "")
                {
                    data_save[10 + i] = Encoding.ASCII.GetBytes(Location.Substring(i, 1))[0];
                }
            }

            //-----------40 byte Description1
            for (int i = 0; i < Description.Length; i++)
            {
                if (Description != "")
                {
                    data_save[50 + i] = Encoding.ASCII.GetBytes(Description.Substring(i, 1))[0];
                }
            }

            //-----------20 byte Timezone record
            string a = "";
            a = mGlobal.FindSystemTimeZoneFromString(Timezone.ToString()).Id.ToString();
            a = Simulate.LSet(a, 15) + Simulate.RSet(a.Substring(Math.Max(a.Length - 2, 1) - 1), 3);
            for (int i = 0; i < a.Length; i++)
            {
                if (Timezone != "")
                {
                    data_save[90 + i] = Encoding.ASCII.GetBytes(a.Substring(i, 1))[0];
                }
            }


            //-----------7 byte datatime record
            DateTime b = _logger_date;
            byte day = Convert.ToByte(b.ToString("dd"));
            byte month = Convert.ToByte(b.ToString("MM"));
            int year = Convert.ToInt32(b.ToString("yyyy"));
            byte hour = Convert.ToByte(b.ToString("HH"));
            byte min = Convert.ToByte(b.ToString("mm"));
            byte sec = Convert.ToByte(b.ToString("ss"));

            try
            {
                data_save[110] = day;
                data_save[111] = month;
                data_save[112] = (byte)(year % 256);
                data_save[113] = Convert.ToByte(year / 256);
                data_save[114] = hour;
                data_save[115] = min;
                data_save[116] = sec;
            }
            catch (Exception)
            {
                MessageBox.Show("Save DateTime fail");
                return;
            }

            //-----------1 byte Delay
            data_save[117] = Convert.ToByte(Delay);

            //-----------2 byte Duration
            data_save[118] = (byte)(Convert.ToInt32(Duration) % 256);
            data_save[119] = Convert.ToByte(Convert.ToInt32(Duration) / 256);

            //-----------8 byte Unit // 4 byte Unit (numSeries byte). max la 16.
            if (mGlobal.unitFromFile == true)
            {
                for (int i = 0; i < 4; i++)
                {
                    data_save[120 + i] = mGlobal.unitTemp[i];
                }
            }
            else//Unit of device
            {
                for (int i = 0; i < 4; i++)
                {
                    data_save[120 + i] = channels[i].Sensor;
                }
            }

            //
            for (int i = 0; i < 4; i++)
            {
                data_save[124 + i] = channels[i].Unit;
            }

            // 1 byte NoAlarm
            for (int i = 0; i < 4; i++)
            {
                if (channels[i].NoAlarm)
                {
                    data_save[136 + i] = 0xf0;
                }
                else
                {
                    data_save[136 + i] = 0x00;
                }
            }

            //-----------16 byte max // 4S chi co 8 byte max (max la 16)
            for (int i = 0; i < 2 * 4; i += 2)
            {
                int tmp = channels[i / 2].AlarmMax;
                if (tmp < 0)
                {
                    tmp = (65536 + tmp);
                }
                data_save[160 + i] = (byte)(tmp % 256);
                data_save[160 + 1 + i] = Convert.ToByte(tmp / 256);
            }

            //-----------16 byte min // 8 byte min (min la 16)
            for (int i = 0; i < 2 * 4; i += 2)
            {
                int tmp = channels[i / 2].AlarmMin;
                if (tmp < 0)
                {
                    tmp = (65536 + tmp);
                }
                data_save[184 + i] = (byte)(tmp % 256);
                data_save[184 + i + 1] = Convert.ToByte(tmp / 256);
            }


            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < channels[i].Desc.Length; j++)
                {
                    if (channels[i].Desc != "")
                    {
                        data_save[200 + 40 * i + j] = Encoding.ASCII.GetBytes(channels[i].Desc.Substring(j, 1))[0];
                    }
                }
            }
        }


        internal void openSetting(byte[] bufSetting)
        {
            //-------Location1
            Location = "";
            byte[] temp2 = new byte[40];
            for (int i = 0; i <= 39; i++)
            {
                if (bufSetting[10 + i] != 0)
                    temp2[i] = bufSetting[10 + i];
            }
            Location += Encoding.UTF8.GetString(temp2);
            Location = Location.Trim();

            //int a = Location.IndexOf("\0");

            if (Location.IndexOf("\0") != -1)
            {
                Location = Location.Substring(0, Location.IndexOf("\0"));
            }


            //---Description
            Description = "";
            byte[] temp3 = new byte[40];
            for (int i = 0; i <= 39; i++)
            {
                if (bufSetting[50 + i] != 0)
                {
                    temp3[i] = bufSetting[50 + i];
                }
            }

            Description += Encoding.UTF8.GetString(temp3);
            Description = Description.Trim();
            if (Description.IndexOf("\0") != -1)
            {
                Description = Description.Substring(0, Description.IndexOf("\0"));
            }



            Timezone = "";
            byte[] tempZone = new byte[20];
            for (int i = 0; i <= 19; i++)
            {
                if (bufSetting[90 + i] != 0)
                {
                    tempZone[i] = bufSetting[90 + i];
                }
            }

            Timezone += Encoding.UTF8.GetString(tempZone);
            Timezone = Description.Trim();
            if (Timezone.IndexOf("\0") != -1)
            {
                Timezone = Description.Substring(0, Timezone.IndexOf("\0"));
            }


            Delay = bufSetting[117];
            Duration = bufSetting[118] + bufSetting[119] * 256;

            //Sensor
            for (int i = 0; i < 8; i++)
            {
                channels[i].Sensor = bufSetting[120 + i];
            }


            //Unit
            for (int i = 0; i < 8; i++)
            {
                channels[i].Unit = bufSetting[124 + i];
            }



            for (int i = 0; i < 8; i++)
            {
                if (bufSetting[136 + i] == 0xf0)
                {
                    channels[i].NoAlarm = true;
                }
                else
                {
                    channels[i].NoAlarm = false;
                }
            }
            //----max
            for (int i = 0; i < 8 * 2; i += 2)
            {
                int a1 = bufSetting[160 + i] + bufSetting[160 + 1 + i] * 256;
                if ((a1 & 32768) == 32768)
                {
                    a1 = a1 - 65536;
                }
                channels[i / 2].AlarmMax = a1;
            }

            //----min 
            for (int i = 0; i < 8 * 2; i += 2)
            {
                int a1 = bufSetting[184 + i] + bufSetting[184 + 1 + i] * 256;
                if ((a1 & 32768) == 32768)
                {
                    a1 = a1 - 65536;
                }
                channels[i / 2].AlarmMin = a1;
            }


            for (int i = 0; i < 8; i++)
            {
                channels[i].Desc = "";
                byte[] tempDes = new byte[40];
                for (int j = 0; j <= 39; j++)
                {
                    if (bufSetting[200 + 40 * i + j] != 0)
                    {
                        tempDes[j] = bufSetting[200 + 40 * i + j];
                    }
                }

                channels[i].Desc += Encoding.UTF8.GetString(tempDes);
                channels[i].Desc = channels[i].Desc.Trim();
                if (channels[i].Desc.IndexOf("\0") != -1)
                {
                    channels[i].Desc = channels[i].Desc.Substring(0, channels[i].Desc.IndexOf("\0"));
                }
            }
        }


    }
}
