using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace Pexo16
{
    public class Device35 : baseDevice
    {

        private static Device35 instance;
        public static Device35 Instance
        {
            get
            {
                if (instance == null)
                {
                    if (instance == null)
                    {
                        instance = new Device35();
                    }
                }
                return instance;
            }
        }
        public static Device35 DelInstance()
        {
            instance = null;
            return instance;
        }

        private string ThreadRunning;
        Thread thReadData;
        public int numOfChannel { get; set; }
        public int desca;


        public List<Channel> getChannels()
        {
            ListCH.Clear();
            for (int i = 0; i < numOfChannel; i++)
            {
                ListCH.Add(Channels[i]);
            }
            return base.ListCH;
        }

        public bool USBOpenG(string StrDevs)
        {
            string StrSerial = "";
            string StrVID = "";
            string StrPID = "";
            byte[] buf = new byte[50];

            try
            {
                StrSerial = StrDevs.Substring(17, StrDevs.Substring(17, StrDevs.Length - 17).IndexOf("_"));
                StrVID = StrDevs.Substring(4, StrDevs.IndexOf("_PID") - 4);
                string StrDevs1 = StrDevs.Substring(StrDevs.IndexOf("_PID") + 5, StrDevs.Length - StrDevs.IndexOf("_PID") - 5);
                StrPID = StrDevs1.Substring(0, StrDevs1.IndexOf("_"));
            }
            catch (Exception)
            {
                strStatus = "Get string device is error" + "\r\n";
                return false;
            }

            buf = Encoding.ASCII.GetBytes(StrSerial);
            try
            {
                devG = HIDFunction.hid_OpenG(UInt16.Parse(StrVID, NumberStyles.AllowHexSpecifier), UInt16.Parse(StrPID, NumberStyles.AllowHexSpecifier), null);
            }
            catch (Exception)
            {
                //devG = 0;
                return false;
            }

            //if ((devG == 0))
            //{
            //    return false;
            //}

            HIDFunction.hid_SetNonBlockingG(devG, 1);
            return true;
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

                //numOfChannel = Int32.Parse(StrSerial.Substring(0, 1));

                StrVID = StrDevs.Substring(4, StrDevs.IndexOf("_PID") - 4);
                string StrDevs1 = StrDevs.Substring(StrDevs.IndexOf("_PID") + 5, StrDevs.Length - StrDevs.IndexOf("_PID") - 5);
                StrPID = StrDevs1.Substring(0, StrDevs1.IndexOf("_"));
            }
            catch (Exception)
            {
                strStatus = "Get string device is error" + "\r\n";
                return false;
            }

            strStatus = string.Format("Open device: VID_{0}_PID_{1} Serial={2}\r\n", StrVID, StrPID, StrSerial);
           
            //if (StrSerial == "")
            //{
            //    for (int i = 0; i < 12; i++)
            //    {
            //        buf[i] = 0x00;
            //    }
            //}
            //else
            //{
                buf = Encoding.ASCII.GetBytes(StrSerial);
            //}

            try
            {
                dev = HIDFunction.hid_Open(ushort.Parse(StrVID, NumberStyles.AllowHexSpecifier), ushort.Parse(StrPID, NumberStyles.AllowHexSpecifier), ref buf[0]);
            }
            catch
            {
                dev = 0;
            }

            if ((dev == 0))
            {
                byte[] msg = new byte[100];

                //MessageBox.Show("ccccccccc");
                strStatus += "Unable to open device" + "\r\n";
                return false;
            }
            //HIDFunction.hid_SetNonBlocking(dev, 0);
            return true;
        }
        public bool Open(string StrDevs)
        {
            string StrSerial = "";
            string StrVID = "";
            string StrPID = "";
            try
            {
                StrSerial = StrDevs.Substring(17, StrDevs.Substring(17, StrDevs.Length - 17).IndexOf("_"));
                StrVID = StrDevs.Substring(4, StrDevs.IndexOf("_PID") - 4);
                string StrDevs1 = StrDevs.Substring(StrDevs.IndexOf("_PID") + 5, StrDevs.Length - StrDevs.IndexOf("_PID") - 5);
                StrPID = StrDevs1.Substring(0, StrDevs1.IndexOf("_"));
            }
            catch (Exception)
            {
                strStatus = "Get string device is error" + "\r\n";
                return false;
            }


            try
            {
                dev = HIDFunction.hidOpen(ushort.Parse(StrVID, NumberStyles.AllowHexSpecifier), ushort.Parse(StrPID, NumberStyles.AllowHexSpecifier), StrSerial);
            }
            catch 
            {
                return false;
            }


            if ((dev==0))
            {
                strStatus += "Unable to open device" + "\r\n";
                return false;
            }
            //HIDFunction.hid_SetNonBlocking(dev, 0);
          
            return true;
        }

        public void Close()
        {
            if (ThreadRunning == "ReadData" & boolWaiting == true)
            {
                thReadData.Abort();
                ThreadRunning = "";
            }

            HIDFunction.hid_Close(dev);
            dev = 0;
            HIDFunction.hid_Exit();

            //if (dev != 0)
            //{
            //    HIDFunction.hid_Close(dev);
            //    dev = 0;
            //    HIDFunction.hid_Exit();
            //}
        }

        public bool writeFirmVer()
        {
            byte[] bufFirm = new byte[65];
            bufFirm[0] = 0x01;
            bufFirm[1] = 0x45;
            if(version == 1)
            {
                bufFirm[2] = 0xff;
            }
            else
            {
                bufFirm[3] = 0x00;
            }

          
            if (base.Write(bufFirm) == false)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool writeSerial()
        {
            byte[] bufSerial = new byte[65];
            bufSerial[0] = 0x01;
            bufSerial[1] = 0xa1;
            bufSerial[2] = 0x00;

            //------Serial 10 bytes--------
            for (int i = 0; i < Serial.Length; i++)
            {
                if (i < 10)
                {
                    bufSerial[i + 3] = Encoding.ASCII.GetBytes(Serial.Substring(i, 1))[0];
                }
            }
            if (base.Write(bufSerial) == false)
            {
                return false;
            }
            else
            {
                //Thread.Sleep(100);
                //bufSerial[0] = 0x02;
                //if (!base.Read(ref bufSerial))
                //{
                //    return false;
                //}
                //else
                //{
                //    return true;
                //}
                return true;
            }
        }

        public bool writeLocation()
        {
            byte[] bufLocation = new byte[65];
            bufLocation[0] = 0x01;
            bufLocation[1] = 0xb3;
            bufLocation[2] = 0x00;

            //------Location1 40 bytes--------
            for (int i = 0; i < Location.Length; i++)
            {
                if (i < 40)
                {
                    bufLocation[i + 3] = Encoding.ASCII.GetBytes(Location.Substring(i, 1))[0];
                }
            }
            if(base.Write(bufLocation) == false)
            {
                return false;
            }
            else
            {
                //Thread.Sleep(300);
                //bufLocation[0] = 0x02;
                //if (!base.Read(ref bufLocation))
                //{
                //    return false;
                //}
                //else
                //{
                //    return true;
                //}
                return true;
            }
        }

        public bool writeDescription()
        {
            byte[] bufDescription = new byte[65];
            bufDescription[0] = 0x01;
            bufDescription[1] = 0xb2;
            bufDescription[2] = 0x00;

            for (int i = 0; i < Description.Length; i++)
            {
                if (i < 40)
                {
                    bufDescription[i + 3] = Encoding.ASCII.GetBytes(Description.Substring(i, 1))[0];
                }
            }
            if (base.Write(bufDescription) == false)
            {
                return false;
            }
            else
            {
                //Thread.Sleep(300);
                //bufDescription[0] = 0x02;
                //if (!base.Read(ref bufDescription))
                //{
                //    return false;
                //}
                //else
                //{
                //    return true;
                //}
                return true;
            }
        }

        public bool writeSettingDevice(DateTime LoggerTime, string ZoneName)
        {
            byte[] buf = new byte[65];
            buf[0] = 0x01;
            buf[1] = 0xb1;
            buf[2] = 0x00;

            //Write TimeZone
            if (ZoneName.Length > 22)
            {
                ZoneName = ZoneName.Substring(0, 22);
            }
            mGlobal.StrToArray(ZoneName, ref buf, 3);

            //Write DateTime
            byte day = 0;
            byte month = 0;
            byte year = 0;
            byte hour = 0;
            byte min = 0;
            byte sec = 0;

            day = Convert.ToByte(LoggerTime.ToString("dd"));
            month = Convert.ToByte(LoggerTime.ToString("MM"));
            year = Convert.ToByte(LoggerTime.ToString("yy"));
            hour = Convert.ToByte(LoggerTime.ToString("HH"));
            min = Convert.ToByte(LoggerTime.ToString("mm"));
            sec = Convert.ToByte(LoggerTime.ToString("ss"));

            buf[25] = sec;
            buf[26] = min;
            buf[27] = hour;
            buf[28] = day;
            buf[29] = month;
            buf[30] = year;

            buf[31] = (byte)(Startrec);
            buf[32] = (byte)(Duration / 256);
            buf[33] = byte.Parse(Math.Truncate((double)Duration % 256).ToString());

            if (base.Write(buf) == false)
            {
                return false;
            }
            else
            {
                //Thread.Sleep(300);
                //buf[0] = 0x02;
                //if (!base.Read(ref buf))
                //{
                //    return false;
                //}
                //else
                //{
                //    return true;
                //}
                return true;
            }
        }

        public bool writeSettingChannel()
        {
            for (int i = 0; i < 4; i++)
            {
                Thread.Sleep(100);
                byte[] buf = new byte[100];
                buf[0] = 0x01;
                switch (i)
                {
                    case 0: buf[1] = 0xd1; break;
                    case 1: buf[1] = 0xd2; break;
                    case 2: buf[1] = 0xd3; break;
                    case 3: buf[1] = 0xd4; break;
                }
                buf[2] = 0x0;

                buf[3] = Channels[i].Sensor;
                if (Channels[i].Sensor == 0)
                {
                    buf[4] = 0xff;
                    buf[5] = 0xff;

                    // 2 byte alarm max
                    buf[6] = 0xff;
                    buf[7] = 0xff;
                    //2 byte alarm min
                    buf[8] = 0xff;
                    buf[9] = 0xff;
                }
                else
                {
                    buf[4] = Channels[i].Unit;
                    if (Channels[i].NoAlarm)
                    {
                        buf[5] = 0xF0;
                    }
                    else
                    {
                        buf[5] = 0x0;
                    }

                    if (Channels[i].AlarmMax < 0)
                    {
                        Channels[i].AlarmMax = 65536 + Channels[i].AlarmMax;
                    }

                    if (Channels[i].AlarmMin < 0)
                    {
                        Channels[i].AlarmMin = 65536 + Channels[i].AlarmMin;
                    }

                    // 2 byte alarm max
                    buf[6] = byte.Parse((Channels[i].AlarmMax / 256).ToString());
                    buf[7] = byte.Parse((Channels[i].AlarmMax % 256).ToString());
                    //2 byte alarm min
                    buf[8] = byte.Parse((Channels[i].AlarmMin / 256).ToString());
                    buf[9] = byte.Parse((Channels[i].AlarmMin % 256).ToString());
                }

                //40 byte description Channel
                mGlobal.StrToArray(Channels[i].Desc, ref buf, 10);

                if (!base.Write(buf))
                {
                    return false;
                }
                //Thread.Sleep(500);
                //buf[0] = 0x02;
                //if (!base.Read(ref buf))
                //{
                //    return false;
                //}
                //Thread.Sleep(500);
            }
            return true;
        }

        public bool writeSettingChannel1()
        {
                byte[] buf = new byte[100];
                buf[0] = 0x01;
                buf[1] = 0xd1; 
                buf[1] = 0xd1; 
               
                buf[2] = 0x0;

                buf[3] = Channels[0].Sensor;
                buf[4] = Channels[0].Unit;
                if (Channels[0].NoAlarm)
                {
                    buf[5] = 0xF0;
                }
                else
                {
                    buf[5] = 0x0;
                }

                //2 byte logger memory

                // 2 byte alarm max
            
                buf[6] = byte.Parse((Channels[0].AlarmMax / 256).ToString());
                buf[7] = byte.Parse((Channels[0].AlarmMax % 256).ToString());
                //2 byte alarm min
                buf[8] = byte.Parse((Channels[0].AlarmMin / 256).ToString());
                buf[9] = byte.Parse((Channels[0].AlarmMin % 256).ToString());
                //4 byte alarm Time

                //4 byte alarm timer up

                //4 byte alarm timer down

                //40 byte description Channel
                mGlobal.StrToArray(Channels[0].Desc, ref buf, 10);

                base.Write(buf);
                Thread.Sleep(500);
        
            return true;
        }

        public bool writeCalibOffset(byte channel, byte data, byte sign)
        {
            byte[] buf = new byte[64];

            buf[0] = 0x01;
            buf[1] = 0x91;
            buf[2] = channel;
            buf[3] = data;
            buf[4] = sign;

            if(!Write(buf))
            {
                return false;
            }
            else
            {
                //Thread.Sleep(200);
                //buf[0] = 0x02;
                if(!Read(ref buf))
                {
                    return false;
                }
                else
                {
                    if(buf[2] == 0xee && buf[3] == 0xee)
                    {
                        return false;
                    }
                    else if(buf[2] != 0x91 && buf[3] != 0x91)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
        }

        public bool writeEmailSetting(byte[] bufEmail)
        {
            if (!base.Write(bufEmail))
            {
                return false;
            }
            else
            {
                //bufEmail[0] = 0x02;
                //base.Read(ref bufEmail);
                return true;
            }
        }

        public bool writeSMSSetting(byte[] bufSMS)
        {
            if (!base.Write(bufSMS))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool eraseSetting()
        {
            byte[] buf = new byte[100];
            buf[0] = 0x01;
            buf[1] = 0xe1;

            base.Write(buf);
            Thread.Sleep(3000);
            HIDFunction.hid_SetNonBlocking(dev, 1);

            base.Read(ref buf);
            if(buf[2] == 225 && buf[3] == 225)
            { 
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool readSerial()
        {
            byte[] bufSerial = new byte[65];
            bufSerial[0] = 0x01;
            bufSerial[1] = 0xa1;
            bufSerial[2] = 0x01;

            if (base.Write(bufSerial) == false)
            {
                return false;
            }
            else
            {
                //bufSerial[0] = 0x02;
                Thread.Sleep(5);
                if (base.Read(ref bufSerial) == false)
                {
                    return false;
                }
                else
                {
                    if (bufSerial[2] == 0xee && bufSerial[3] == 0xee)
                    {
                        return false;
                    }
                    else
                    {
                        Serial = mGlobal.ArrayToStr(ref bufSerial, 2, 10);
                        HardVer = bufSerial[12];
                        FirmVer = bufSerial[13];
                    }
                }
            }
            return true;
        }

        public bool readLocation()
        {
            while (true)
            {
                byte[] bufLocation = new byte[65];
                bufLocation[0] = 0x01;
                bufLocation[1] = 0xb3;
                bufLocation[2] = 0x01;

                if (base.Write(bufLocation) == false)
                {
                    return false;
                }
                else
                {
                    //bufLocation[0] = 0x02;
                    Thread.Sleep(5);
                    if (base.Read(ref bufLocation) == false)
                    {
                        //break;
                        continue;
                        //return false;
                    }
                    else
                    {
                        if (bufLocation[2] == 238 && bufLocation[3] == 238)
                        {
                            continue;
                        }
                        else if (bufLocation[2] == 255 && bufLocation[3] == 255)
                        {
                            Location = "";
                        }
                        else
                        {
                            Location = mGlobal.ArrayToStr(ref bufLocation, 2, 40);
                        }
                        break;
                    }
                  
                }
            
            }
            return true;
        }

        public bool readDescription()
        {
            while (true)
            {
                byte[] buDecription = new byte[65];
                buDecription[0] = 0x01;
                buDecription[1] = 0xb2;
                buDecription[2] = 0x01;

                if (base.Write(buDecription) == false)
                {
                    return false;
                }
                else
                {
                    //buDecription[0] = 0x02;
                    Thread.Sleep(5);
                    if (base.Read(ref buDecription) == false)
                    {
                        continue;
                        //break;
                        //return false;
                    }
                    else
                    {
                        if (buDecription[2] == 238 && buDecription[3] == 238)
                        {
                            continue;
                        }
                        else if (buDecription[2] == 255 && buDecription[3] == 255)
                        {
                            Description = "";
                        }
                        else
                        {
                            Description = mGlobal.ArrayToStr(ref buDecription, 2, 40);
                        }
                        break;
                    }
                }
            }
            return true;
        }

        public bool readSettingDevice()
        {
            while(true)
            {
                byte[] buf = new byte[65];

                buf[0] = 0x01;
                buf[1] = 0xb1;
                buf[2] = 0x01;

                if (base.Write(buf) == false)
                {
                    return false;
                }
                else
                {
                    //buf[0] = 0x02;
                    Thread.Sleep(5);
                    if (base.Read(ref buf) == false)
                    {
                        continue;
                    }
                    else
                    {
                        if (buf[2] == 238 && buf[3] == 238)
                        {
                            continue;
                        }
                        else if (buf[1] != 48)
                        {
                            return false;
                        }
                        else
                        {
                            StartTime = new DateTime();
                           
                            //6 byte Start Time
                            double a = Convert.ToInt32(buf[2].ToString());
                            StartTime = StartTime.AddSeconds(Convert.ToInt32(buf[2].ToString()));
                            StartTime = StartTime.AddMinutes(buf[3]);
                            StartTime = StartTime.AddHours(buf[4]);
                            StartTime = StartTime.AddDays(buf[5] - 1);
                            StartTime = StartTime.AddMonths(buf[6] - 1);
                            if (buf[7] != 0)
                            {
                                StartTime = StartTime.AddYears(buf[7] - 1);
                            }
                            else
                            {
                                StartTime = StartTime.AddYears(buf[7]);
                            }

                            //6 byte Stop Time
                            StopTime.AddSeconds(buf[8]);
                            StopTime.AddMinutes(buf[9]);
                            StopTime.AddHours(buf[10]);
                            StopTime.AddDays(buf[11]);
                            StopTime.AddMonths(buf[12]);
                            StopTime.AddYears(buf[13]);

                            //4 byte Run Time
                            RunTime = new DateTime();

                            RunTime =  RunTime.AddSeconds(buf[14]);
                            RunTime =  RunTime.AddMinutes(buf[15]);
                            RunTime =  RunTime.AddHours(buf[16]);
                            RunTime =  RunTime.AddDays(buf[17]);

                            //1 byte Setting
                            byteLogging = buf[18];


                            //22 byte TimeZone
                            Timezone = mGlobal.ArrayToStr(ref buf, 19, 22);

                            SettingTime = new DateTime();
                            SettingTime = SettingTime.AddSeconds(buf[41]);
                            SettingTime = SettingTime.AddMinutes(buf[42]);
                            SettingTime = SettingTime.AddHours(buf[43]);
                            SettingTime = SettingTime.AddDays(buf[44] - 1);
                            SettingTime = SettingTime.AddMonths(buf[45] - 1);
                            SettingTime = SettingTime.AddYears(buf[46] - 1);


                            //1 byte Start Delay
                            Delay = buf[47];// min

                            //2 byte Duration
                            Duration = buf[48] * 256 + buf[49];// day

                            break;
                        }
                    }
                }
            }

            //bool dem = true;

            //Line0: byte[] buf = new byte[64];

            //buf[0] = 0x01;
            //buf[1] = 0xb1;
            //buf[2] = 0x01;

            //if (base.Write(buf) == false)
            //{
            //    return false;
            //}
            //else
            //{
            //    //buf[0] = 0x02;
            //    Thread.Sleep(20);
            //    if (base.Read(ref buf) == false)
            //    {
            //        return false;
            //    }
            //    else
            //    {
            //        if (buf[2] == 238 && buf[3] == 238 && dem)
            //        {
            //            dem = false; 
            //            goto Line0;
            //        }
            //        else if(buf[1] != 48)
            //        {
            //            return false;
            //        }
            //        else
            //        {
            //            StartTime = new DateTime();
            //            //6 byte Start Time
            //            double a = Convert.ToInt32(buf[2].ToString());
            //            StartTime = StartTime.AddSeconds(Convert.ToInt32(buf[2].ToString()));
            //            StartTime = StartTime.AddMinutes(buf[3]);
            //            StartTime = StartTime.AddHours(buf[4]);
            //            StartTime = StartTime.AddDays(buf[5] - 1);
            //            StartTime = StartTime.AddMonths(buf[6] - 1);
            //            if (buf[7] != 0)
            //            {
            //                StartTime = StartTime.AddYears(buf[7] - 1);
            //            }
            //            else
            //            {
            //                StartTime = StartTime.AddYears(buf[7]);
            //            }

            //            //6 byte Stop Time
            //            StopTime.AddSeconds(buf[8]);
            //            StopTime.AddMinutes(buf[9]);
            //            StopTime.AddHours(buf[10]);
            //            StopTime.AddDays(buf[11]);
            //            StopTime.AddMonths(buf[12]);
            //            StopTime.AddYears(buf[13]);

            //            //4 byte Run Time
            //            RunTime.AddSeconds(buf[14]);
            //            RunTime.AddMinutes(buf[15]);
            //            RunTime.AddHours(buf[16]);
            //            RunTime.AddDays(buf[17]);

            //            //1 byte Setting
            //            byteLogging = buf[18];

            //            //22 byte TimeZone
            //            Timezone = mGlobal.ArrayToStr(ref buf, 19, 22);

            //            //1 byte Start Delay
            //            Delay = buf[47];// min

            //            //2 byte Duration
            //            Duration = buf[48] * 256 + buf[49];// day
            //        }
            //    }
            //}
            return true;
        }

        public bool readSettingChannel()
        {
            for (int i = 0; i < 4; i++)
            {
                bool dem = true;
                while (true)
                {
                    byte[] buf = new byte[mGlobal.len];
                    buf[0] = 0x01;
                    switch (i)
                    {
                        case 0: buf[1] = 0xd1; break;
                        case 1: buf[1] = 0xd2; break;
                        case 2: buf[1] = 0xd3; break;
                        case 3: buf[1] = 0xd4; break;
                    }
                    buf[2] = 0x01;

                    if (base.Write(buf) == false)
                    {
                        return false;
                    }
                    else
                    {
                        //buf[0] = 0x02;
                        Thread.Sleep(5);
                        if (base.Read(ref buf) == false)
                        {
                            //return false;
                            continue;
                        }
                        else
                        {
                            if (buf[2] == 238 && buf[3] == 238)
                            {
                                //dem = false;
                                //goto Line0;
                                continue;
                            }
                            else
                            {
                                //1 byte sensor
                                Channels[i].Sensor = buf[16];

                                //1 byte Unit (sensor tem)
                                Channels[i].Unit = buf[17];

                                //1 byte no alarm?
                                if (buf[18] == 0)
                                {
                                    Channels[i].NoAlarm = false;
                                }
                                else
                                {
                                    Channels[i].NoAlarm = true;
                                }

                                //2 byte max alarm
                                Channels[i].AlarmMax = buf[19] * 256 + buf[20];

                                //2 byte min alarm
                                Channels[i].AlarmMin = buf[21] * 256 + buf[22];

                                //4 byte Time alarm (byte 11 -> 14)

                                //4 byte alarm timer up (byte 15 -> 18)

                                //4 byte alarm timer down (byte 19 -> 22)

                                //40 byte description
                                if (buf[23] == 255 && buf[24] == 255)
                                {
                                    Channels[i].Desc = "";
                                }
                                else
                                {
                                    Channels[i].Desc = mGlobal.ArrayToStr(ref buf, 23, 40);
                                }
                                break;
                            }
                        }
                    }
                }
                //Thread.Sleep(3000);
            }
            return true;
        }

        public bool readSettingChannel1(int i)
        {
                bool dem = true;
                while (true)
                {
                    byte[] buf = new byte[64];
                    buf[0] = 0x01;
                    switch (i)
                    {
                        case 0: buf[1] = 0xd1; break;
                        case 1: buf[1] = 0xd2; break;
                        case 2: buf[1] = 0xd3; break;
                        case 3: buf[1] = 0xd4; break;
                    }
                    buf[2] = 0x01;

                    if (base.Write(buf) == false)
                    {
                        return false;
                    }
                    else
                    {
                        //buf[0] = 0x02;
                        Thread.Sleep(5);
                        if (base.Read(ref buf) == false)
                        {
                            //return false;
                            continue;
                        }
                        else
                        {
                            if (buf[2] == 238 && buf[3] == 238)
                            {
                                //dem = false;
                                //goto Line0;
                                continue;
                            }
                            else
                            {
                                //1 byte sensor
                                Channels[i].Sensor = buf[16];

                                //1 byte Unit (sensor tem)
                                Channels[i].Unit = buf[17];

                                //1 byte no alarm?
                                if (buf[18] == 0)
                                {
                                    Channels[i].NoAlarm = false;
                                }
                                else
                                {
                                    Channels[i].NoAlarm = true;
                                }

                                //2 byte max alarm
                                Channels[i].AlarmMax = (int)mGlobal.get_temp(buf[19], buf[20]);
                                //Channels[i].AlarmMax = buf[19] * 256 + buf[20];
                                //if (Channels[i].AlarmMax > 32768)

                                //2 byte min alarm
                                Channels[i].AlarmMin = (int)mGlobal.get_temp(buf[21], buf[22]);
                                //Channels[i].AlarmMin = buf[21] * 256 + buf[22];

                                //4 byte Time alarm (byte 11 -> 14)

                                //4 byte alarm timer up (byte 15 -> 18)

                                //4 byte alarm timer down (byte 19 -> 22)

                                //40 byte description
                                if (buf[23] == 255 && buf[24] == 255)
                                {
                                    Channels[i].Desc = "";
                                }
                                else
                                {
                                    Channels[i].Desc = mGlobal.ArrayToStr(ref buf, 23, 40);
                                }
                                break;
                            }
                        }
                    }
                }
            return true;
        }

        public bool readDataStart(int channel)
        {
            bool dem = true;
            while (true)
            {
                byte[] buf = new byte[mGlobal.len];
                buf[0] = 0x01;
                buf[1] = 0xc1;
                buf[2] = 0x3E;
                buf[3] = byte.Parse(channel.ToString());

                buf[4] = 0x01;


                //HIDFunction.hid_SetNonBlocking(dev, 1);
                if (!base.Write(buf))
                {
                    return false;
                }
                //buf[0] = 0x02;
                //Thread.Sleep(5);
            
                if (!base.Read(ref buf))
                {
                    continue;
                    MessageBox.Show("dbbdbfb");
                    //return false;
                }
                else
                {
                    if (buf[2] == 238 && buf[3] == 238)
                    {
                        continue;
                        //dem = false;
                        //Thread.Sleep(100);
                        
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return true;
        }

       // public bool readDataEeprom(int times, int channel, int unit)
        //{
        //    if(unit == 3)
        //        times = 10;
        //    for (int i = 0; i < times; i++)
        //    {
        //        byte[] buf = new byte[64];
        //        buf[0] = 0x01;
        //        buf[1] = 0xc1;
        //        buf[2] = 0x3E;
        //        buf[3] = byte.Parse(channel.ToString());
        //        buf[4] = 0x00;

        //        if(!base.Write(buf))
        //        {
        //            return false;
        //        }

        //        Thread.Sleep(100);
        //        buf[0] = 0x02;
        //        if (!base.Read(ref buf))
        //        {
        //            return false;
        //        }
        //        else
        //        {
        //            for (int k = 0; k < 62; k += 2)//moi Data la 2 byte
        //            {
        //                if (buf[k + 2] == 0xff && buf[k + 3] == 0xff && unit != 3)
        //                {
        //                    return true;
        //                }
        //                Channels[channel - 1].Data[31 * i + k / 2] = buf[k + 2] * 256 + buf[k + 3];// Nen dua vao mang tam.
        //                //if (buf[k + 2] < 80)
        //                //{
        //                //    Channels[channel - 1].Data[31 * i + k / 2] = buf[k + 2] * 256 + buf[k + 3];// Nen dua vao mang tam.
        //                //}
        //                //else
        //                //{
        //                //    Channels[channel - 1].Data[31 * i + k / 2] = - buf[k + 3];
        //                //}
                       
        //            }
        //        }
        //    }
        //    return true;
        //}

        public bool readDataEeprom(int times, int channel, int unit)
        {
            int countByteRead = 0;
            
            for (int i = 0; i < times; i++)
            {
                int count = 0;

                while (true)
                {
                    byte[] buf = new byte[mGlobal.len];
                    buf[0] = 0x01;
                    buf[1] = 0xc1;
                    buf[2] = 0x3E;
                    buf[3] = byte.Parse(channel.ToString());
                    buf[4] = 0x00;

                    if (!base.Write(buf))
                    {
                        return false;
                    }
                    
                    Thread.Sleep(5);

                    if (base.Read(ref buf) == false)
                    {
                        continue;
                    }
                    //if(HIDFunction.hid_Read(dev, ref buf[0], 64) == 0)
                    //{
                    //    return false;
                    //}
                    else
                    {
                        if (buf[2] != 238 && buf[3] != 238)
                        {
                            int tmp = 0;
                            if (buf[2] == 255 && buf[3] == 255)
                            {
                                tmp = 4;
                            }
                            for (int k = tmp; k < 62; k += 2)//moi Data la 2 byte
                            {
                                if (buf[k + 2] == 0xff && buf[k + 3] == 0xff && unit != 3)
                                {
                                    return true;
                                }
                                //Channels[channel - 1].Data[31 * i + (k - tmp) / 2] = buf[k + 2] * 256 + buf[k + 3];// Nen dua vao mang tam.
                                Channels[channel - 1].Data[countByteRead] = buf[k + 2] * 256 + buf[k + 3];
                                //if (buf[k + 2] < 80)
                                //{
                                //    Channels[channel - 1].Data[31 * i + k / 2] = buf[k + 2] * 256 + buf[k + 3];// Nen dua vao mang tam.
                                //}
                                //else
                                //{
                                //    Channels[channel - 1].Data[31 * i + k / 2] = - buf[k + 3];
                                //}
                                if (unit == 3)
                                {
                                    //if (Channels[channel - 1].Data[31 * i + (k - tmp) / 2] == 65535 && Channels[channel - 1].Data[31 * i + (k - tmp) / 2 - 1] == 65535 && Channels[channel - 1].Data[31 * i + (k - tmp) / 2 - 2] == 65535)
                                    if (Channels[channel - 1].Data[countByteRead] == 65535 && Channels[channel - 1].Data[countByteRead - 1] == 65535 && Channels[channel - 1].Data[countByteRead - 2] == 65535)
                                    {
                                        return true;
                                    }
                                }
                                countByteRead += 1;
                            }
                            break;
                        }
                       
                    }
                }

            //Line0: byte[] buf = new byte[64];
            //    buf[0] = 0x01;
            //    buf[1] = 0xc1;
            //    buf[2] = 0x3E;
            //    buf[3] = byte.Parse(channel.ToString());
            //    buf[4] = 0x00;

            //    if (!base.Write(buf))
            //    {
            //        return false;
            //    }

            //    Thread.Sleep(500);
            //    buf[0] = 0x02;
            //    if (!base.Read(ref buf))
            //    {
            //        return false;
            //    }
            //    else
            //    {
            //        if (buf[2] == 238 && buf[3] == 238 && count != 2)
            //        {
            //            count += 1;
            //            //Thread.Sleep(1000);
            //            goto Line0;
            //        }
            //        else if(buf[1] != 62)
            //        {
            //            return false;
            //        }
            //        else
            //        { 
            //            int tmp = 0;
            //            if(buf[2] == 255 && buf[3] == 255)
            //            {
            //                tmp = 4;
            //            }
            //            for (int k = tmp; k < 62; k += 2)//moi Data la 2 byte
            //            {
            //                if (buf[k + 2] == 0xff && buf[k + 3] == 0xff && unit != 3)
            //                {
            //                    return true;
            //                }
            //                //Channels[channel - 1].Data[31 * i + (k - tmp) / 2] = buf[k + 2] * 256 + buf[k + 3];// Nen dua vao mang tam.
            //                Channels[channel - 1].Data[countByteRead] = buf[k + 2] * 256 + buf[k + 3];
            //                //if (buf[k + 2] < 80)
            //                //{
            //                //    Channels[channel - 1].Data[31 * i + k / 2] = buf[k + 2] * 256 + buf[k + 3];// Nen dua vao mang tam.
            //                //}
            //                //else
            //                //{
            //                //    Channels[channel - 1].Data[31 * i + k / 2] = - buf[k + 3];
            //                //}
            //                if (unit == 3)
            //                {
            //                    //if (Channels[channel - 1].Data[31 * i + (k - tmp) / 2] == 65535 && Channels[channel - 1].Data[31 * i + (k - tmp) / 2 - 1] == 65535 && Channels[channel - 1].Data[31 * i + (k - tmp) / 2 - 2] == 65535)
            //                    if (Channels[channel - 1].Data[countByteRead] == 65535 && Channels[channel - 1].Data[countByteRead - 1] == 65535 && Channels[channel - 1].Data[countByteRead - 2] == 65535)
            //                    {
            //                        return true;
            //                    }
            //                }
            //                countByteRead += 1;
            //            }
            //        }
            //    }

                //Thread.Sleep(5);
            }
            return true;
        }


        public bool readDataEeprom1(int times, int channel, int unit, Progress pro, string a)
        {
            int countByteRead = 0;
            int count = 0;

            for (int i = 0; i <= times; i++)
            {
                while (true)
                {
                    byte[] buf = new byte[mGlobal.len];
                    buf[0] = 0x01;
                    buf[1] = 0xc1;
                    buf[2] = 0x3E;
                    buf[3] = byte.Parse(channel.ToString());
                    buf[4] = 0x00;


                    if (!base.Write(buf))
                    {
                        return false;
                    }

                    if (base.Read(ref buf) == false)
                    {
                        continue;
                    }
                
                    else
                    {
                        if(buf[1] == 3)
                        {
                            continue;
                        }
                        if (buf[6] == 0x3e && buf[5] == 0xc1)
                        {
                            continue;
                        }
                        if (buf[2] != 238 && buf[3] != 238)
                        {
                            mGlobal.numProgress += 1;

                            if (mGlobal.numProgress <= pro.progressBar1.Maximum)
                            {
                                pro.progressBar1.Value = mGlobal.numProgress;
                                if (mGlobal.numProgress % 5 == 0)
                                {
                                    pro.lblProgress.Text = string.Format("Reading......{0:00.00} %", ((double)mGlobal.numProgress / (double)pro.progressBar1.Maximum) * 100);
                                }
                                pro.Refresh();
                                Application.DoEvents();
                                pro.progressBar1.Update();
                            }

                            int tmp = 0;
                            
                            for (int k = tmp; k < 62; k += 2)//moi Data la 2 byte
                            {
                                    Channels[channel - 1].Data[countByteRead] = buf[k + 2] * 256 + buf[k + 3];
                                    if (unit != 3)
                                    {
                                        if (Channels[channel - 1].Data[countByteRead] == 65535 && Channels[channel - 1].Data[countByteRead - 1] == 65535)
                                        {
                                            return true;
                                        }
                                    }

                                    else // unit = 3 (vibration)
                                    {
                                        if (countByteRead >= 2)
                                        {
                                            if (Channels[channel - 1].Data[countByteRead] == 65535 && Channels[channel - 1].Data[countByteRead - 1] == 65535 && Channels[channel - 1].Data[countByteRead - 2] == 65535)
                                            {
                                                return true;
                                            }
                                        }
                                    }
                                    countByteRead += 1;
                                //}
                            }
                            break;
                        }

                    }

                }
            }
            return true;
         
        }

        public bool readDataStop(int times, int lenght, int channel)
        {
            bool dem = true;
            //byte[] buf = new byte[64];
            //buf[0] = 0x01;
            //buf[1] = 0xc1;
            //buf[2] = byte.Parse(lenght.ToString());
            //buf[3] = byte.Parse(channel.ToString());
            //buf[4] = 0x00;

            //if(!base.Write(buf))
            //{
            //    return false;
            //}
            //else
            //{
            //    buf[0] = 0x02;
            //    if(!base.Read(ref buf))
            //    {
            //        return false;
            //    }
            //    else
            //    {
            //        //if (buf[2] == 238 && buf[3] == 238 && dem)
            //        //{
            //        //    dem = false;
            //        //    goto Line0;
            //        //}
            //        //else
            //        //{
            //            for (int i = 0; i < lenght; i++)
            //            {
            //                Channels[channel - 1].Data[times * 31 + i] = buf[i + 2] * 256 + buf[i + 3];
            //            }
            //        //}
            //    }
            //}

            Line0: byte[] buf2 = new byte[64];
            buf2[0] = 0x01;
            buf2[1] = 0xc1;
            buf2[2] = 0x3e;
            buf2[3] = byte.Parse(channel.ToString());
            buf2[4] = 0x02;

            if(!base.Write(buf2))
            {
                return false;
            }
            else
            {
                buf2[0] = 0x02;
                Thread.Sleep(5);
                if (!base.Read(ref buf2))
                {
                    return false;
                }
                else
                {
                    if(buf2[2] == 238 && buf2[3] == 238 && dem)
                    {
                        dem = false;
                        Thread.Sleep(100);
                        goto Line0;
                    }
                }
                return true;
            }
        }

        public bool readDataProbe(ref byte[] buf)
        {
            while(true)
            {
                buf[0] = 0x01;
                buf[1] = 0xc5;

                if (!Write(buf))
                {
                    return false;
                }
                else
                {
                    Thread.Sleep(100);
                    HIDFunction.hid_SetNonBlocking(dev, 1);
                    //buf[0] = 0x02;
                    if (!Read(ref buf))
                    {
                        //continue;
                        return false;
                    }
                    else
                    {
                        if (buf[2] == 238 && buf[3] == 238 || buf[1] != 28)
                        {
                            continue;
                            //check = false;
                            //dem += 1;
                            //Thread.Sleep(100);
                            //goto Line0;
                        }
                        else
                        {
                            return true;
                        }
                    }
                }
            }


            //bool check = true;
            //int dem = 0;
            ////byte[] buf = new byte[64];
            //Line0: buf[0] = 0x01;
            //buf[1] = 0xc5;

            //if(!Write(buf))
            //{
            //    return false;
            //}
            //else
            //{
            //    Thread.Sleep(500);
            //    //buf[0] = 0x02;
            //    if(!Read(ref buf))
            //    {
            //        return false;
            //    }
            //    else
            //    {
            //        if (buf[2] == 238 && buf[3] == 238 && dem != 2)
            //        {
            //            check = false;
            //            dem += 1;
            //            Thread.Sleep(100);
            //            goto Line0;
            //        }
            //        else
            //        {
            //            return true;
            //        }
            //    }
            //}
        }

        public bool readInfo4Device(ref byte[] buf)
        {
           // byte[] buf = new byte[64];
            buf[0] = 0x01;
            buf[1] = 0xa9;

            if (!base.Write(buf))
            {
                return false;
            }

            //Thread.Sleep(200);
            //buf[0] = 0x02;
            if (!base.Read(ref buf))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool readEmailSetting(ref byte[] buf, int lan)
        {
            buf[0] = 0x01;
            buf[1] = 0xb6;
            buf[2] = 0x01;
            buf[3] = byte.Parse(lan.ToString());

            if (!base.Write(buf))
            {
                return false;
            }

            Thread.Sleep(5);
            buf[0] = 0x02;
            if (!base.Read(ref buf))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool readSMSSetting(ref byte[] buf, int lan)
        {
            buf[0] = 0x01;
            buf[1] = 0xb7;
            buf[2] = 0x01;
            buf[3] = byte.Parse(lan.ToString());

            if (!base.Write(buf))
            {
                return false;
            }

            Thread.Sleep(5);
            buf[0] = 0x02;
            if (!base.Read(ref buf))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool readDataStart(int channel, long dev)
        {
            byte[] buf = new byte[64];
            buf[0] = 0x01;
            buf[1] = 0xc1;
            buf[2] = 0x3E;
            buf[3] = byte.Parse(channel.ToString());
            buf[4] = 0x01;

            if (HIDFunction.hid_Write(dev, ref  buf[0], 64) <= 0)
            {
                return false;
            }
            if (HIDFunction.hid_Read(dev, ref buf[0], 64) <= 0)
            {
                return false;
            }
            else
            {
                if (buf[2] == 238 && buf[3] == 238)
                {
                    MessageBox.Show("238");
                }
            }
            return true;
        }


        public bool readDataEeprom2(int bytes, int channel, int unit, Progress pro, string a)
        {
            int countByteRead = 0;
            int tamp = 1;
            int[] pos = new int[1000];
            int lan = 0;

            byte[] buf = new byte[mGlobal.len];
            buf[0] = 0x01;
            buf[1] = 0xc1;
            buf[2] = 0x3E;
            buf[3] = byte.Parse(channel.ToString());
            buf[4] = 0x00;
            buf[5] = 0x01;

            while (true)
            {
                if (!base.Write(buf))
                {
                    return false;
                }

                if (base.Read(ref buf) == false)
                {
                    continue;
                }
                else
                {
                    if (buf[1] == 3)
                    {
                        buf = new byte[mGlobal.len];
                        buf[0] = 0x01;
                        buf[1] = 0xc1;
                        buf[2] = 0x3E;
                        buf[3] = byte.Parse(channel.ToString());
                        buf[4] = 0x00;
                        buf[5] = 0x01;
                        continue;
                    }
                    if (buf[6] == 0x3e && buf[5] == 0xc1)
                    {
                        buf = new byte[mGlobal.len];
                        buf[0] = 0x01;
                        buf[1] = 0xc1;
                        buf[2] = 0x3E;
                        buf[3] = byte.Parse(channel.ToString());
                        buf[4] = 0x00;
                        buf[5] = 0x01;
                        continue;
                    }
                    if (buf[2] != 238 || buf[3] != 238)
                    {
                        mGlobal.numProgress += 1;

                        if (mGlobal.numProgress <= pro.progressBar1.Maximum)
                        {
                            pro.progressBar1.Value = mGlobal.numProgress;
                            if (mGlobal.numProgress % 5 == 0)
                            {
                                pro.lblProgress.Text = string.Format("Reading......{0:00.00} %", ((double)mGlobal.numProgress / (double)pro.progressBar1.Maximum) * 100);
                            }
                            pro.Refresh();
                            Application.DoEvents();
                            pro.progressBar1.Update();
                        }

                        int tmp = 0;
                        for (int k = tmp; k < 62; k += 2)
                        {
                            Channels[channel - 1].Data[countByteRead] = buf[k + 2] * 256 + buf[k + 3];
                            //Channels[channel - 1].Data[countByteRead] = mGlobal.get_temp(buf[k + 2], buf[k + 3]);

                            //if (countByteRead > 1)
                            //{
                            //    if (Channels[channel - 1].Data[countByteRead] - Channels[channel - 1].Data[countByteRead - 1] > 500)
                            //    {
                            //        Channels[channel - 1].Data[countByteRead] = Channels[channel - 1].Data[countByteRead - 1];
                            //    }
                            //}

                            if (unit != 3)
                            {
                                //Channels[channel - 1].Data[countByteRead] = mGlobal.get_temp(buf[k + 2], buf[k + 3]);

                                if (countByteRead > 1)
                                {
                                    if (Channels[channel - 1].Data[countByteRead] == 29793 && Channels[channel - 1].Data[countByteRead - 1] == 25697 && Channels[channel - 1].Data[countByteRead - 2] == 25966)
                                    {
                                        pos[lan] = countByteRead - 2;
                                        lan += 1;
                                    }
                                    else
                                    {
                                        if (Channels[channel - 1].Data[countByteRead] - Channels[channel - 1].Data[countByteRead - 1] > 500 && Channels[channel - 1].Data[countByteRead] - Channels[channel - 1].Data[countByteRead - 1] < 32768)
                                        {
                                            Channels[channel - 1].Data[countByteRead] = Channels[channel - 1].Data[countByteRead - 1];
                                        }
                                    }
                                }
                                if (countByteRead >= (bytes - 1) || mGlobal.stop)
                                {
                                    for (int i = 0; i < lan; i++)
                                    {
                                        if (i == lan - 1)
                                        {
                                            for (int j = 0; j <= bytes - pos[i]; j++)
                                            {
                                                Channels[channel - 1].Data[pos[i] + j] = Channels[channel - 1].Data[pos[i] - (1 + j)];
                                            }
                                        }
                                        else
                                        {
                                            for (int j = 0; j < 3; j++)
                                            {
                                                Channels[channel - 1].Data[pos[i] + j] = Channels[channel - 1].Data[pos[i] - (1 + j)];
                                            }
                                        }
                                    }

                                    if (Channels[channel - 1].Data[countByteRead] == 25966)
                                    {
                                        Channels[channel - 1].Data[countByteRead] = Channels[channel - 1].Data[countByteRead - 1];
                                    }
                                    if (Channels[channel - 1].Data[countByteRead] == 25697)
                                    {
                                        Channels[channel - 1].Data[countByteRead] = Channels[channel - 1].Data[countByteRead - 2];
                                        Channels[channel - 1].Data[countByteRead - 1] = Channels[channel - 1].Data[countByteRead - 3];
                                    }
                                    Channels[channel - 1].Data[countByteRead + 1] = 25966;
                                    Channels[channel - 1].Data[countByteRead + 2] = 25697;
                                    Channels[channel - 1].Data[countByteRead + 3] = 29793;
                                    return true;
                                }
                            }

                            else // unit = 3 (vibration)
                            {
                                //Channels[channel - 1].Data[countByteRead] = buf[k + 2] * 256 + buf[k + 3];
                                //Channels[channel - 1].Data[countByteRead] = mGlobal.get_temp(buf[k + 2], buf[k + 3]);

                                if (Channels[channel - 1].Data[countByteRead] == 65535 && Channels[channel - 1].Data[countByteRead - 1] == 65535)
                                {
                                    Channels[channel - 1].Data[countByteRead] = Channels[channel - 1].Data[countByteRead - 3];
                                }

                                //if (Channels[channel - 1].Data[countByteRead] == 25965)
                                //{
                                //    MessageBox.Show("aaaaaaaaaaaaaaaaaa");
                                //}
                                //if (Channels[channel - 1].Data[countByteRead] == 25966)
                                //{
                                //    MessageBox.Show("aaaaaaaaaaaaaaaaaa");
                                //}

                                if (countByteRead >= (3 * bytes - 1) || mGlobal.stop)
                                {
                                    Channels[channel - 1].Data[countByteRead + 1] = 25966;
                                    Channels[channel - 1].Data[countByteRead + 2] = 25697;
                                    Channels[channel - 1].Data[countByteRead + 3] = 29793;
                                    return true;
                                }
                            }
                            countByteRead += 1;
                        }
                        buf[0] = 0x01;
                        buf[1] = 0xc1;
                        buf[2] = 0x3E;
                        buf[3] = byte.Parse(channel.ToString());
                        buf[4] = 0x00;
                        buf[5] = 0x00;
                    }
                }
            }

        }



        public string SaveTextFile()
        {
            string dataSave = "";
            int offset = 510;
            int len = 0;

            dataSave += "Offset: " + offset.ToString() + Environment.NewLine;

            dataSave += string.Format("Number of channel: {0}{1}", numOfChannel, Environment.NewLine);

            dataSave += "Pexo35" + Environment.NewLine;

            dataSave += string.Format("Serial: {0}{1}", Serial, Environment.NewLine);

            dataSave += string.Format("Location: {0}{1}", Location, Environment.NewLine);

            dataSave += string.Format("Description: {0}{1}", Description, Environment.NewLine);

            string a = "";
            a = mGlobal.FindSystemTimeZoneFromString(Timezone.ToString()).Id.ToString();
            a = Simulate.LSet(a, 15) + Simulate.RSet(a.Substring(Math.Max(a.Length - 2, 1) - 1), 3);
            dataSave += string.Format("Timezone: {0}{1}", a, Environment.NewLine);

            dataSave += string.Format("StartTime: {0}{1}", _logger_date, Environment.NewLine);

            dataSave += string.Format("Delay: {0}{1}", Delay, Environment.NewLine);

            dataSave += string.Format("Duration: {0}{1}", Duration, Environment.NewLine);

            if (mGlobal.unitFromFile == true)
            {
                for (int i = 0; i < numOfChannel; i++)
                {
                    dataSave += mGlobal.unitTemp[i].ToString() + Environment.NewLine;
                }
            }
            else//Unit of device
            {
                for (int i = 0; i < numOfChannel; i++)
                {
                    if(Channels[i].NoAlarm)
                    {
                        dataSave += string.Format("{0}    No Alarm    {1}", Channels[i].Unit, Environment.NewLine);
                    }
                    else 
                    {
                        dataSave += string.Format("{0}{1} {2}{3}", Channels[i].Unit, Channels[i].AlarmMax, Channels[i].AlarmMin, Environment.NewLine);
                    }
                }
            }

            dataSave += string.Format("Title of graph: {0}{1}", mGlobal.TitleGraph, Environment.NewLine);


            if (comment != null)
            {
                dataSave += string.Format("Comment: {0}{1}", comment, Environment.NewLine);
            }

            for (int k = 0; k < numOfChannel; k++)
            {
                for (int i = 0; i < Channels[k].Data.Length; i++)
                {
                    if (Channels[k].Unit == 175 || Channels[k].Unit == 172 || Channels[k].Unit == 2)
                    {
                        dataSave += (Channels[k].Data[i] * 10).ToString() + "  ";
                    }
                    else if (Channels[k].Unit == 3)
                    {
                        dataSave += (Channels[k].Data[i] * 1000).ToString() + "  ";
                    }
                    else
                    {
                        dataSave += Channels[k].Data[i].ToString() + "  ";
                    }
                }
                dataSave += Environment.NewLine;
            }



            return dataSave;
        }


        public void SaveFile_MP_Lgr2(ref byte[] data_save)
        {
            // save Data version v01
            int offset = 510;
            //int version = 1; //v01

            int len = 0;
            for (int i = 0; i < numOfChannel; i++)
            {
                if(Channels[i].Data.Length != 0)
                    len = Channels[i].Data.Length;
            }

            data_save = new byte[offset + len * 2 * numOfChannel];

            //---------- 2byte Data offset and 1 byte version Data
            data_save[0] = (byte)(offset % 256);
            data_save[1] = Convert.ToByte(offset / 256);

            //data_save[2] = Convert.ToByte(version);
            data_save[2] = (byte)numOfChannel;

            //pexo16 - nhan dang file (byte 3-8)
            for (int i = 0; i <= 5; i++)
            {
                data_save[i + 3] = Encoding.ASCII.GetBytes("pexo35".Substring(i, 1))[0];
            }

            //-----------10 byte Serial
            for (int i = 0; i <= 9; i++)
            {
                if (Serial != "")
                {
                    data_save[10 + i] = Encoding.ASCII.GetBytes(Serial.Substring(i, 1))[0];
                }
            }
            //-----------40 byte Location
            for (int i = 0; i < Location.Length; i++)
            {
                if (Location != "")
                {
                    data_save[20 + i] = Encoding.ASCII.GetBytes(Location.Substring(i, 1))[0];
                }
            }

            //-----------40 byte Description
            for (int i = 0; i < Description.Length; i++)
            {
                if (Description != "")
                {
                    data_save[60 + i] = Encoding.ASCII.GetBytes(Description.Substring(i, 1))[0];
                }
            }

            //-----------20 byte Timezone record
            string a = "";
            //a = mGlobal.FindTimeZoneByID(Timezone.ToString()).Id.ToString();
            //a = Simulate.LSet(a, 15) + Simulate.RSet(a.Substring(Math.Max(a.Length - 2, 1) - 1), 3);
            TimeZoneInfo t = mGlobal.FindSystemTimeZoneFromDisplayName(Timezone);
            a = t.Id;
            if(a.Length > 22)
            {
                a = a.Substring(0, 22);
            }
            for (int i = 0; i < a.Length; i++)
            {
                if (Timezone != "")
                {
                    data_save[100 + i] = Encoding.ASCII.GetBytes(a.Substring(i, 1))[0];
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
                data_save[122] = day;
                data_save[123] = month;
                data_save[124] = (byte)(year % 256);
                data_save[125] = Convert.ToByte(year / 256);
                data_save[126] = hour;
                data_save[127] = min;
                data_save[128] = sec;
            }
            catch (Exception)
            {
                MessageBox.Show("Save DateTime fail");
                return;
            }

            //-----------1 byte Delay
            data_save[129] = Convert.ToByte(Delay);

            //-----------2 byte Duration
            data_save[130] = (byte)(Convert.ToInt32(Duration) % 256);
            data_save[131] = Convert.ToByte(Convert.ToInt32(Duration) / 256);

            //-----------8 byte Unit
            if (mGlobal.unitFromFile == true)
            {
                for (int i = 0; i < numOfChannel; i++)
                {
                    data_save[132 + i] = mGlobal.unitTemp[i];
                }
            }
            else//Unit of device
            {
                for (int i = 0; i < numOfChannel; i++)
                {
                    data_save[132 + i] = Channels[i].Unit;
                }
            }

            // 1 byte NoAlarm
            for (int i = 0; i < numOfChannel; i++)
            {
                if(Channels[i].NoAlarm)
                {
                    data_save[146 + i] = 0xf0;
                }
                else
                {
                    data_save[146 + i] = 0x00;
                }
            }

            //-----------16 byte max
            for (int i = 0; i < 2 * numOfChannel; i += 2)
            {
                int tmp = Channels[i / 2].AlarmMax;
                if (tmp < 0)
                {
                    tmp = (65536 + tmp);
                }
                data_save[160 + i] = (byte)(tmp % 256);
                data_save[160 + 1 + i] = Convert.ToByte(tmp / 256);
            }

            //-----------16 byte min // 8 byte min (min la 16)
            for (int i = 0; i < 2 * numOfChannel; i += 2)
            {
                int tmp = Channels[i / 2].AlarmMin;
                if (tmp < 0)
                {
                    tmp = (65536 + tmp);
                }
                data_save[184 + i] = (byte)(tmp % 256);
                data_save[184 + i + 1] = Convert.ToByte(tmp / 256);
            }


            //----------- 50 byte title Graph
            for (int i = 0; i < mGlobal.TitleGraph.Length; i++)
            {
                data_save[210 + i] = Encoding.ASCII.GetBytes(mGlobal.TitleGraph.Substring(i, 1))[0];
            }

            //-----------200 byte comment
            if (comment != null)
            {
                for (int i = 0; i < comment.Length; i++)
                {
                    data_save[260 + i] = Encoding.ASCII.GetBytes(comment.Substring(i, 1))[0];
                }
            }

            //----------- 24 byte color line // 48 byte color
            //line 1
            if (numOfChannel > 0)
            {
                for (int i = 0; i < 3 * numOfChannel; i += 3)
                {
                    data_save[460 + i] = Convert.ToByte(Channels[i / 3].LineColor.R.ToString());
                    data_save[460 + 1 + i] = Convert.ToByte(Channels[i / 3].LineColor.G.ToString());
                    data_save[460 + 2 + i] = Convert.ToByte(Channels[i / 3].LineColor.B.ToString());
                }
            }

            //--------------Data (offset Data = 510)

            for (int k = 0; k < numOfChannel; k++)
            {
                for (int i = 0; i < Channels[k].Data.Length; i++)
                {
                    //if (Channels[k].Data[i] != 0xff)
                    //{
                        double temp = 0;
                        if (Channels[k].Unit == 175 || Channels[k].Unit == 172 || Channels[k].Unit == 2)
                        {
                            temp = Convert.ToDouble(Channels[k].Data[i]) * 10;
                        }
                        else if(Channels[k].Unit == 3)
                        {
                            temp = Convert.ToDouble(Channels[k].Data[i]) * 1000;
                        }
                        else
                        {
                            temp = Convert.ToDouble(Channels[k].Data[i]);
                        }
                        if (temp < 0)
                        {
                            temp = temp + 1 + 65535;
                        }
                        data_save[offset + 2 * k + i * 2 * numOfChannel] = (byte)(temp / 256);
                        data_save[offset + 2 * k + i * 2 * numOfChannel + 1] = Convert.ToByte((int)(temp % 256));
                        //data_save[offset + i * 2 * numOfChannel] = (byte)(temp % 256);
                        //data_save[offset + i * 2 * numOfChannel + 1] = Convert.ToByte((int)(temp / 256));
                    //}
                }
                //offset = offset + Channels[k].Data.Length * 2;
            }
        }


        public bool OpenTextFile(string path)
        {
            string setting = "";
            if (System.IO.File.Exists(path))
            {
                FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);

                using (StreamReader sw = new StreamReader(fs))
                {
                    setting += sw.ReadToEnd();
                }
            }
            string[] lines = Regex.Split(setting, Environment.NewLine);

            offset = int.Parse(lines[0].Substring(8, 3));

            numOfChannel = int.Parse(lines[1].Substring(19, 1));

            Channels = new Channel[numOfChannel];
            for (int i = 0; i < numOfChannel; i++)
            {
                Channels[i] = new Channel();
            }


            Serial = lines[3].Substring(8, lines[3].Length - 8);

            Location = lines[4].Substring(10, lines[4].Length - 10);

            Description = lines[5].Substring(13, lines[5].Length - 13);

            Delay = int.Parse(lines[8].Substring(7, lines[8].Length - 7));

            Duration = int.Parse(lines[9].Substring(10, lines[9].Length - 10));

            Timezone = lines[6].Substring(10, lines[6].Length - 10);

            
            for (int i = 0; i < numOfChannel; i++)
            {
                Channels[i].Unit = byte.Parse(lines[10 + i].Substring(0, 3));
            }


            int mini = 65356;

            for (int i = 0; i < numOfChannel; i++)
            {
                string[] data2 = Regex.Split(lines[17 + i], "  ");
                if(data2.Length != 1 && data2.Length < mini)
                {
                    mini = data2.Length;
                }
            }
        
            data_open = new byte[2 * numOfChannel * (mini -1) + 510 ];

            int tmp = 0;

            for (int i = 0; i < numOfChannel; i++)
            {
                string[] data = Regex.Split(lines[17 + i], "  ");
                if (data.Length < mini)
                    tmp = data.Length - 1;
                else 
                    tmp = mini - 1; 
                for (int j = 0; j < tmp; j++)
                {
                    data_open[510 + 2 * i + 2 * j * numOfChannel] = (byte)(int.Parse(data[j]) / 256);
                    data_open[510 + 2 * i + 2 * j * numOfChannel + 1] = (byte)(int.Parse(data[j]) % 256);
                }
                //tmp = data.Length;
            }

            if (Timezone != "")
            {
                for (int i = 0; i < Timezone.Length; i++)
                {
                    data_open[100 + i] = Encoding.ASCII.GetBytes(Timezone.Substring(i, 1))[0];
                }
            }


            _logger_date = DateTime.Parse(lines[7].Substring(10, lines[7].Length - 10));


            DateTime b = _logger_date;
            byte day = Convert.ToByte(b.ToString("dd"));
            byte month = Convert.ToByte(b.ToString("MM"));
            int year = Convert.ToInt32(b.ToString("yyyy"));
            byte hour = Convert.ToByte(b.ToString("HH"));
            byte min = Convert.ToByte(b.ToString("mm"));
            byte sec = Convert.ToByte(b.ToString("ss"));

            try
            {
                data_open[120] = day;
                data_open[121] = month;
                data_open[122] = (byte)(year % 256);
                data_open[123] = Convert.ToByte(year / 256);
                data_open[124] = hour;
                data_open[125] = min;
                data_open[126] = sec;
            }
            catch (Exception)
            {
                MessageBox.Show("Save DateTime fail");
            }


            mGlobal.open_file = true;

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
            //version = data_open[2];
            numOfChannel = data_open[2];

            bytenhandang = "";
            byte[] temp = new byte[6];

            for (int i = 0; i <= 5; i++)
            {
                temp[i] = data_open[i + 3];
            }
            bytenhandang += Encoding.UTF8.GetString(temp);
            if (bytenhandang != "pexo35")
            {
                MessageBox.Show("file incorrect");
                return false;
            }
           

            Channels = new Channel[numOfChannel];
            for (int i = 0; i < numOfChannel; i++)
            {
                Channels[i] = new Channel();
            }


            //-------Serial
            Serial = "";
            byte[] tamp = new byte[10];
            for (int i = 0; i < 10; i++)
            {
                if (data_open[10 + i] != 0)
                    tamp[i] = data_open[10 + i];
            }

            Serial += Encoding.UTF8.GetString(tamp);
            Serial = Serial.Trim();
            if (Serial.IndexOf("\0") != -1)
            {
                Serial = Serial.Substring(0, Serial.IndexOf("\0"));
            }

            //-------Location1
            Location = "";
            byte[] temp2 = new byte[40];
            for (int i = 0; i <= 39; i++)
            {
                if (data_open[20 + i] != 0)
                    temp2[i] = data_open[20 + i];
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
                if (data_open[60 + i] != 0)
                {
                    temp3[i] = data_open[60 + i];
                }
            }

            Description += Encoding.UTF8.GetString(temp3);
            Description = Description.Trim();
            if (Description.IndexOf("\0") != -1)
            {
                Description = Description.Substring(0, Description.IndexOf("\0"));
            }

            Delay = data_open[129];
            Duration = data_open[130] + data_open[131] * 256;


            for (int i = 0; i < numOfChannel; i++)
            {
                Channels[i].Unit = data_open[132 + i];
            }


            for (int i = 0; i < numOfChannel; i++)
            {
                if (data_open[146 + i] == 0xf0)
                {
                    Channels[i].NoAlarm = true;
                }
                else
                {
                    Channels[i].NoAlarm = false;
                }
            }
            //----max
            for (int i = 0; i < numOfChannel * 2; i += 2)
            {
                int a1 = data_open[160 + i] + data_open[160 + 1 + i] * 256;
                if ((a1 & 32768) == 32768)
                {
                    a1 = a1 - 65536;
                }
                Channels[i / 2].AlarmMax = a1;
            }

            //----min 
            for (int i = 0; i < numOfChannel * 2; i += 2)
            {
                int a1 = data_open[184 + i] + data_open[184 + 1 + i] * 256;
                if ((a1 & 32768) == 32768)
                {
                    a1 = a1 - 65536;
                }
                Channels[i / 2].AlarmMin = a1;
            }

            //---color read logger
            for (int i = 0; i < numOfChannel * 3; i += 3)
            {
                Channels[i / 3].LineColor = Color.FromArgb(data_open[460 + i], data_open[460 + 1 + i], data_open[460 + 2 + i]);
            }

            // -----Data
            mGlobal.data_byte = new byte[data_open.Length - 1];
            for (int i = 0; i <= data_open.Length - 2; i++)
            {
                mGlobal.data_byte[i] = data_open[i];      
            }


            //--- title
            byte[] temp4 = new byte[50];
            titlegraph = "";
            int dem = 0;
            for (int i = 0; i <= 49; i++)
            {
                if (mGlobal.data_byte[210 + i] != 0)
                {
                    temp4[i] = mGlobal.data_byte[210 + i];
                    dem = i + 1;
                }
            }
            titlegraph += Encoding.UTF8.GetString(temp4);
            titlegraph = titlegraph.Trim();
            titlegraph = titlegraph.Substring(0, dem);


            //----comment
            comment = "";
            dem = 0;
            byte[] temp5 = new byte[200];
            for (int i = 0; i <= 199; i++)
            {
                if (mGlobal.data_byte[260 + i] != 0)
                {
                    temp5[i] = mGlobal.data_byte[260 + i];
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
                    Channels[i].LineColor = Color.Empty;
                }
            }


            return true;
        }



        public void saveSetting(ref byte[] data_save)
        {
            for (int i = 0; i < Serial.Length; i++)
            {
                if (Serial != "")
                {
                    data_save[i] = Encoding.ASCII.GetBytes(Serial.Substring(i, 1))[0];
                }
            }
            //-----------40 byte Location
            for (int i = 0; i < Location.Length; i++)
            {
                if (Location != "")
                {
                    data_save[10 + i] = Encoding.ASCII.GetBytes(Location.Substring(i, 1))[0];
                }
            }

            //-----------40 byte Description
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

            //-----------8 byte Unit
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
                    data_save[120 + i] = Channels[i].Sensor;
                }
            }

            //
            for (int i = 0; i < 4; i++)
            {
                data_save[124 + i] = Channels[i].Unit;
            }

            // 1 byte NoAlarm
            for (int i = 0; i < 4; i++)
            {
                if (Channels[i].NoAlarm)
                {
                    data_save[136 + i] = 0xf0;
                }
                else
                {
                    data_save[136 + i] = 0x00;
                }
            }

            //-----------16 byte max
            for (int i = 0; i < 2 * 4; i += 2)
            {
                int tmp = Channels[i / 2].AlarmMax;
                if (tmp < 0)
                {
                    tmp = (65536 + tmp);
                }
                data_save[160 + i] = (byte)(tmp % 256);
                data_save[160 + 1 + i] = Convert.ToByte(tmp / 256);
            }

            //-----------16 byte min
            for (int i = 0; i < 2 * 4; i += 2)
            {
                int tmp = Channels[i / 2].AlarmMin;
                if (tmp < 0)
                {
                    tmp = (65536 + tmp);
                }
                data_save[184 + i] = (byte)(tmp % 256);
                data_save[184 + i + 1] = Convert.ToByte(tmp / 256);
            }


            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < Channels[i].Desc.Length; j++)
                {
                    if (Channels[i].Desc != "")
                    {
                        data_save[200 + 40 * i + j] = Encoding.ASCII.GetBytes(Channels[i].Desc.Substring(j, 1))[0];
                    }
                }    
            }
        }

        public void openSetting(byte[] data_open)
        {
            Channels = new Channel[4];
            for (int i = 0; i < 4; i++)
            {
                Channels[i] = new Channel();
            }

            Serial = "";
            byte[] tamp = new byte[10];
            for (int i = 0; i < 10; i++)
            {
                if (data_open[i] != 0)
                {
                    tamp[i] = data_open[i];
                }
            }
            Serial += Encoding.UTF8.GetString(tamp);
            Serial = Serial.Trim();
            if (Serial.IndexOf("\0") != -1)
            {
                Serial = Serial.Substring(0, Serial.IndexOf("\0"));
            }
            

            //-------Location1
            Location = "";
            byte[] temp2 = new byte[40];
            for (int i = 0; i <= 39; i++)
            {
                if (data_open[10 + i] != 0)
                    temp2[i] = data_open[10 + i];
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
                if (data_open[50 + i] != 0)
                {
                    temp3[i] = data_open[50 + i];
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
                if (data_open[90 + i] != 0)
                {
                    tempZone[i] = data_open[90 + i];
                }
            }

            Timezone += Encoding.UTF8.GetString(tempZone);
            Timezone = Description.Trim();
            if (Timezone.IndexOf("\0") != -1)
            {
                Timezone = Description.Substring(0, Timezone.IndexOf("\0"));
            }


            Delay = data_open[117];
            Duration = data_open[118] + data_open[119] * 256;

            //Sensor
            for (int i = 0; i < 4; i++)
            {
                Channels[i].Sensor = data_open[120 + i];
            }


            //Unit
            for (int i = 0; i < 4; i++)
            {
                Channels[i].Unit = data_open[124 + i];
            }


            for (int i = 0; i < 4; i++)
            {
                if (data_open[136 + i] == 0xf0)
                {
                    Channels[i].NoAlarm = true;
                }
                else
                {
                    Channels[i].NoAlarm = false;
                }
            }
            //----max
            for (int i = 0; i < 4 * 2; i += 2)
            {
                int a1 = data_open[160 + i] + data_open[160 + 1 + i] * 256;
                if ((a1 & 32768) == 32768)
                {
                    a1 = a1 - 65536;
                }
                Channels[i / 2].AlarmMax = a1;
            }

            //----min 
            for (int i = 0; i < 4 * 2; i += 2)
            {
                int a1 = data_open[184 + i] + data_open[184 + 1 + i] * 256;
                if ((a1 & 32768) == 32768)
                {
                    a1 = a1 - 65536;
                }
                Channels[i / 2].AlarmMin = a1;
            }


            for (int i = 0; i < 4; i++)
            {
                Channels[i].Desc = "";
                byte[] tempDes = new byte[40];
                for (int j = 0; j <= 39; j++)
                {
                    if (data_open[200 + 40 * i + j] != 0)
                    {
                        tempDes[i] = data_open[200 + 40 * i + j];
                    }
                }

                Channels[i].Desc += Encoding.UTF8.GetString(tempDes);
                Channels[i].Desc = Channels[i].Desc.Trim();
                if (Channels[i].Desc.IndexOf("\0") != -1)
                {
                    Channels[i].Desc = Description.Substring(0, Channels[i].Desc.IndexOf("\0"));
                }
            }

        }
    }
}
