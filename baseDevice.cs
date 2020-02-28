using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Pexo16
{
    public class baseDevice
    {
        //private static baseDevice instance;

        //public static baseDevice Instance
        //{
        //    get
        //    {
        //        if(instance == null)
        //        {
        //            if (instance == null)
        //            {
        //                instance = new baseDevice();
        //            }
        //        }
        //        return instance;
        //    }   
        //}
        //public static baseDevice DelInstance()
        //{
        //    instance = null;
        //    return instance;
        //}

        public baseDevice() {  }

        byte[] buf = new byte[500];
        public string hostport;
        public string strStatus;
        public Int64 dev;

        public IntPtr devG;

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

        private byte ERR_OK = 0;

        private Channel[] channels;
        public Channel[] Channels
        {
            get { return channels; }
            set { channels = value; }
        }

        private List<Channel> listCH = new List<Channel>();
        public List<Channel> ListCH
        {
            get { return listCH; }
            set { listCH = value; }
        }


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

        private int firmVer;
        public int FirmVer
        {
            get { return firmVer; }
            set { firmVer = value; }
        }

        private int hardVer;
        public int HardVer
        {
            get { return hardVer; }
            set { hardVer = value; }
        }

        private string timezone;
        public string Timezone
        {
            get { return timezone; }
            set { timezone = value; }
        }

        private DateTime startTime;
        public DateTime StartTime
        {
            get { return startTime; }
            set { startTime = value; }
        }

        private DateTime stopTime;
        public DateTime StopTime
        {
            get { return stopTime; }
            set { stopTime = value; }
        }

        private DateTime runTime;
        public DateTime RunTime
        {
            get { return runTime; }
            set { runTime = value; }
        }

        private DateTime settingTime;
        public DateTime SettingTime
        {
            get { return settingTime; }
            set { settingTime = value; }
        }

        public string Status;
        private bool boolOK = false;
        public long lReceivedByteNum;

        //From File///////////////////////////////////////////////////////////////////////////////////////////      
        public byte[] data_open;
        public int offset;
        public int version;
        public string bytenhandang;
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

        public bool Read(ref byte[] buf)
        {
            try
            {
                byte[] msg = new byte[mGlobal.len];
                short res = 0;

                if (dev == 0)
                {
                    strStatus = "device = 0" + "\r\n";
                    return false;
                }

                //buf[0] = byte.Parse(0.ToString());
                //for (int i = 0; i < 3; i++)
                //{
                //    buf[1 + i] = cmd[i];
                //}

                //strStatus = "send_feature(CMD_READ_SS_SETTING)";
                //res = HIDFunction.hid_Write(dev, ref buf[0], 64);
                //if (res < 0)
                //{
                //    strStatus += " error(res=" + res + ")" + "\r\n";
                //    return false;
                //}
                //strStatus += " ok" + "\r\n";
                //strStatus += "Get feature";

                //buf[0] = byte.Parse(0x02.ToString());
                //HIDFunction.hid_SetNonBlocking(dev, 1);
                res = HIDFunction.hid_Read(dev, ref buf[0], mGlobal.len);
                //res = HIDFunction.hidRead(dev, ref buf[0], 64);
                //res = HIDFunction.hid_ReadTimeOut(dev, ref buf[0], 64, 3);
                if (res <= 0)
                {
                    //strStatus += " error (res=" + res + ")" + "\r\n";
                    //HIDFunction.hid_Error(dev, ref msg[0]);
                    //strStatus += "Error= " + msg.ToString() + "\r\n";
                    return false;
                }

                //res = HIDFunction.hid_Exit();
                return true;
            }
            catch (Exception)
            {
                MessageBox.Show("Read_setting false. status=" + strStatus);
                return false;
            }
        }

        public bool Write(byte[] buf)
        {
            byte[] msg = new byte[100];
            short res = 0;

            if (dev == 0)
            {
                strStatus = "device = 0" + Environment.NewLine;
                return false;
            }
            //strStatus = "send_feature(CMD_WRITE_SERIAL_NUM)";
            res = HIDFunction.hid_Write(dev, ref  buf[0], mGlobal.len);

            if (res < 0)
            {
                //strStatus += "error (res=" + res + Environment.NewLine;
                //HIDFunction.hid_Error(dev, ref msg[0]);
                //MessageBox.Show(msg.ToString());
                //strStatus += "Error = " + msg.ToString() + Environment.NewLine;
                return false;
            }
            //res = HIDFunction.hid_Exit();
            return true;
        }


        public bool WriteG(byte[] buf)
        {
            byte[] msg = new byte[100];
            int res = 0;

            //if (dev == 0)
            //{
            //    strStatus = "device = 0" + Environment.NewLine;
            //    return false;
            //}

            //res = HIDFunction.hid_SendFeatureReport(dev, ref buf[0], 64);
            res = HIDFunction.hid_WriteG(devG, buf, 64);

            if (res < 0)
            {
                //HIDFunction.hid_Error(dev, ref msg[0]);
                //MessageBox.Show(msg.ToString());
                return false;
            }
            res = HIDFunction.hid_Exit();
            return true;
        }

        public bool ReadG(ref byte[] buf)
        {
            try
            {
                byte[] msg = new byte[64];
                int res = 0;

                //if (dev == 0)
                //{
                //    strStatus = "device = 0" + "\r\n";
                //    return false;
                //}

                //res = HIDFunction.hid_Get_Feature_Report(dev, ref buf[0], 64);
                res = HIDFunction.hid_ReadG(devG, buf, 64);

                if (res <= 0)
                {
                    strStatus += " error (res=" + res + ")" + "\r\n";
                    HIDFunction.hid_Error(dev, ref msg[0]);
                    strStatus += "Error= " + msg.ToString() + "\r\n";
                    return false;
                }

                res = HIDFunction.hid_Exit();
                return true;
            }
            catch (Exception)
            {
                MessageBox.Show("Read_setting false. status=" + strStatus);
                return false;
            }
        }


        public bool Get(ref byte[] buf)
        {
            try
            {
                byte[] msg = new byte[64];
                short res = 0;

                if (dev == 0)
                {
                    strStatus = "device = 0" + "\r\n";
                    return false;
                }

                res = HIDFunction.hid_GetFeatureReport(dev, ref buf[0], 64);
                if (res <= 0)
                {
                    strStatus += " error (res=" + res + ")" + "\r\n";
                    HIDFunction.hid_Error(dev, ref msg[0]);
                    strStatus += "Error= " + msg.ToString() + "\r\n";
                    return false;
                }
                return true;
            }
            catch (Exception)
            {
                MessageBox.Show("Read_setting false. status=" + strStatus);
                return false;
            }
        }

        public bool Send(byte[] buf)
        {
            byte[] msg = new byte[100];
            short res = 0;

            if (dev == 0)
            {
                strStatus = "device = 0" + Environment.NewLine;
                return false;
            }

            strStatus = "send_feature(CMD_WRITE_SERIAL_NUM)";
            res = HIDFunction.hid_SendFeatureReport(dev, ref buf[0], 3);

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


        public bool ReadTimeOut(ref byte[] buf)
        {
            try
            {
                byte[] msg = new byte[64];
                short res = 0;

                if (dev == 0)
                {
                    strStatus = "device = 0" + "\r\n";
                    return false;
                }

                res = HIDFunction.hid_ReadTimeOut(dev, ref buf[0], 64, 500);
                if (res <= 0)
                {
                    strStatus += " error (res=" + res + ")" + "\r\n";
                    HIDFunction.hid_Error(dev, ref msg[0]);
                    strStatus += "Error= " + msg.ToString() + "\r\n";
                    return false;
                }
                return true;
            }
            catch (Exception)
            {
                MessageBox.Show("Read_setting false. status=" + strStatus);
                return false;
            }
        }

    }
}
