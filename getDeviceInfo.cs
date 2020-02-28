using System;
using System.Collections;
using System.IO;
using System.Threading;

namespace Pexo16
{
    class getDeviceInfo
    {
        public static ArrayList listLoggerName = new ArrayList();
        public static ArrayList listHostPortName = new ArrayList();
        public static ArrayList activeDeviceListAl = new ArrayList(); //hostport
        public static ArrayList activeSerialAL = new ArrayList();     //Serial
        
        public static void getLogger()
        {
            string FileName = "";
            FileName = mGlobal.app_patch(FileName);
            FileName += "\\MenuData2.bin";
            if (File.Exists(FileName))
            {
                StreamReader reader = new StreamReader(FileName);
                string name = reader.ReadLine();
                string s = reader.ReadLine();
                int i = 0;
                int j = 0;
                string temp = null;
                string temp2 = "a";
                if (name.IndexOf("MenuData2.bin") >= 0 & s != "0")
                {
                    listLoggerName.Clear();
                    listHostPortName.Clear();
                    do
                    {
                        if (i == 0)
                        {
                            listLoggerName.Add(reader.ReadLine());
                            j = 1;
                            i = i + 1;
                            temp = reader.ReadLine();
                        }
                        else
                        {
                            if (j == 0)
                            {
                               
                                listLoggerName.Add(temp2);
                                j = 1;
                                i = i + 1;
                                temp = reader.ReadLine();
                            }
                            else
                            {
                                listHostPortName.Add(temp);
                                j = 0;
                                i = i + 1;
                                temp2 = reader.ReadLine();
                            }
                        }
                    }
                    while (temp != null & temp2 != null);
                }
            }
        }

        public static void getActiveDevice()
        {
          Li:  activeSerialAL.Clear();
            activeDeviceListAl.Clear();

            byte[] buf = new byte[500];
            string s = "";

           Int64 devs;
           Int64 curdev;

           //HIDFunction.hid_Init();

           devs = HIDFunction.hid_Enumerate(0, 0);
           //MessageBox.Show(devs.ToString());
           curdev = devs;

           while (curdev != 0)
           {
               //ushort vid = HIDFunction.hid_DeviceVendorID(curdev);
               //ushort pid = HIDFunction.hid_DeviceProductID(curdev);

               Device device = Device.DelInstance();
               device = Device.Instance;
            s = device.getHostPort(curdev);

            //MessageBox.Show(s);

               if(s == "er")
               {
                   //HIDFunction.hid_FreeEnumeration(curdev);
                   HIDFunction.hid_FreeEnumeration(devs);
                   goto Li;
               }

               //if (device.Product == "Datalogger8S" || device.Product == "Datalogger4S")
               if (device.Product != "(null)" && device.Serial != "(null)" && s.Contains("483"))
               {
                   //MessageBox.Show(device.Serial);
                   activeDeviceListAl.Add(s);
                   activeSerialAL.Add(device.Serial);

                   mGlobal.usb_id = s;
                   mGlobal.usb_search = true;
               }
               curdev = HIDFunction.hid_DeviceNext(curdev);
           }
           HIDFunction.hid_Exit();
           HIDFunction.hid_FreeEnumeration(curdev);
           HIDFunction.hid_FreeEnumeration(devs);
           //HIDFunction.hid_Exit();
        }
        
        public static string groupDevice(string hostport)
        {
            Device menuDevice = Device.DelInstance();
            menuDevice = Device.Instance;
            string name = "";
            if (menuDevice.USBOpen(hostport))
            {

            }
            return name;
        }


        public static string readSerial(long dev)
        {
            string name = "";
            short res = 0;

            byte[] bufSerial = new byte[65];
            bufSerial[0] = 0x01;
            bufSerial[1] = 0xa1;
            bufSerial[2] = 0x01;

            res = HIDFunction.hid_Write(dev, ref bufSerial[0], 64);

            if (res < 0)
            {
                return "Cannot connect to device.";
            }

            res = HIDFunction.hid_Read(dev, ref bufSerial[0], 64);

            if (bufSerial[2] == 238 && bufSerial[3] == 238)
            {
                return "Cannot connect to device.";
            }
            else
            {
                for (int i = 0; i < 10; i++)
                {
                    if (bufSerial[i + 2] == 0xff)
                    {
                        name += "FF";
                    }
                    else
                    {
                        name += (char)bufSerial[i + 2];
                    }
                }
            }
            return name;
        }

        public static string nhanDang(long dev)
        {
            if (dev != 0)
            {
                bool dem = true;
                string name = "";
                while (true)
                {
                    short res = 0;
                    byte[] buf = new byte[64];

                    buf[0] = byte.Parse(0x01.ToString());
                    buf[1] = byte.Parse(0xa8.ToString());
                    //buf[2] = byte.Parse(0x50.ToString());
                    //buf[3] = byte.Parse(0x43.ToString());

                    //HIDFunction.hid_SetNonBlocking(dev, 1);

                    res = HIDFunction.hid_Write(dev, ref buf[0], 64);
                    if (res < 0)
                    {
                        return "Cannot connect to device a8.";
                    }
                    //Thread.Sleep(5);
                    buf[0] = 0x02;
                    res = HIDFunction.hid_Read(dev, ref buf[0], 64);
                    if (res < 0)
                    {
                        return "Cannot connect to device a8.";
                    }

                    if (buf[2] == 238 && buf[3] == 238 && dem)
                    {
                        //dem = false;
                        Thread.Sleep(200);
                        continue;
                        //goto Line0;
                    }
                    else
                    {
                        for (int i = 0; i < 7; i++)
                        {
                            name += (char)buf[i + 2];
                        }
                        break;
                    }
                }
                return name;
            }
            else
            {
                return "er";
            }
        }


        public static string nhanDang34(long dev)
        {
            if (dev != 0)
            {
                bool dem = true;
            Line0: string name = "";
                short res = 0;
                byte[] buf = new byte[65];

                buf[0] = byte.Parse(0x01.ToString());
                buf[1] = byte.Parse(0xa8.ToString());

                res = HIDFunction.hid_Write(dev, ref  buf[0], 65);
                if (res < 0)
                {
                    HIDFunction.hid_Error(dev, ref buf[10]);
                    string b = "";
                    for (int i = 0; i < buf.Length; i++)
                    {
                        char a = (char)buf[i];
                        b += a.ToString();
                    }
                    return "Cannot connect to device.";
                }
                Thread.Sleep(50);

                res = HIDFunction.hid_Read(dev, ref buf[0], 65);
                if (res < 0)
                {
                    return "Cannot connect to device.";
                }

                if (buf[2] == 238 && buf[3] == 238 && dem)
                {
                    dem = false;
                    Thread.Sleep(200);
                    goto Line0;
                }
                else
                {
                    for (int i = 0; i < 7; i++)
                    {
                        name += (char)buf[i + 2];
                    }
                }
                return name;
            }
            else
            {
                return "er";
            }
        }
    }
}
