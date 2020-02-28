using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Resources;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace Pexo16
{
    public partial class EmailSetting : Form
    {
        //Device emailDevice = Device.Instance;
        //string _str = "";
        //string str_device = "";
        //string FilePath;

        string host = "";
        Device35 emailDevice = null;

        ResourceManager res_man = new ResourceManager("Pexo16.Lang.Resources", typeof(EmailSetting).Assembly);
        CultureInfo cul;

        public EmailSetting(string hostPort)
        {
            host = hostPort;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //string a = Send_Email("huytq0601@yahoo.com.vn", "tranquanghuy1906@gmail.com", "abcdefgh", "hgedcba");
            //MessageBox.Show(a);
            //timer1.Enabled = true;

            emailDevice.USBOpen(host);

            if (emailDevice.readSettingDevice() == false)
            {
                emailDevice.Close();
                return;
            }

            emailDevice.Close();

            if (emailDevice.byteLogging == 0x44)
            {
                MessageBox.Show("Logger is recording. Cannot write setting email");
                return;
            }

            emailDevice.USBOpen(host);

            string cc = "";
            int lan = 0;

            cc = richTextBox1.Text.Replace("\n", ";");
            //string dsEmail = textBox1.Text + ";" + cc;
            string dsEmail = cc;
            if (dsEmail.Substring(dsEmail.Length - 1, 1) == ";")
            {
                dsEmail = dsEmail.Substring(0, dsEmail.Length - 1);
            }
            //byte[] arrEmail = new byte[dsEmail.Length];
            byte[] arrEmail = new byte[600];

            int tmp = 0;
            if (arrEmail.Length <= 60)
                tmp = arrEmail.Length;
            else tmp = 60;

            for (int i = 0; i < dsEmail.Length; i++)
            {
                arrEmail[i] = Encoding.ASCII.GetBytes(dsEmail.Substring(i, 1))[0];
            }
            for (int i = dsEmail.Length; i < 600; i++)
            {
                arrEmail[i] = 0xff;
            }
            if (arrEmail.Length > 600)
            {
                MessageBox.Show(res_man.GetString("Emails are too long to save", cul));
                emailDevice.Close();
                return;
            }
            else
            {
                if (arrEmail.Length % 60 > 0)
                {
                    lan = arrEmail.Length / 60 + 1;
                }
                else
                {
                    lan = arrEmail.Length / 60;
                }
                for (int i = 0; i < 10; i++)
                {
                    byte[] buf = new byte[64];
                    buf[0] = 0x01;
                    buf[1] = 0xb6;
                    buf[2] = 0x00;
                    buf[3] = byte.Parse((i + 1).ToString());
                    //if (i == lan - 1)
                    //{
                    //    tmp = arrEmail.Length % 60;
                    //}
                    for (int j = 0; j < tmp; j++)
                    {
                        buf[j + 4] = arrEmail[60 * i + j];
                    }
                    Thread.Sleep(200);
                    if (!emailDevice.writeEmailSetting(buf))
                    {
                        emailDevice.Close();
                        MessageBox.Show(res_man.GetString("Write Setting fail", cul));
                        return;
                    }
                }
                saveEmail(dsEmail);
                btnRead.Enabled = true;
                emailDevice.Close();
                MessageBox.Show(res_man.GetString("Write setting successfully", cul));
            }

        }

        public static bool isValidEmail(string inputEmail)
        {
            string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                  @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                  @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            Regex re = new Regex(strRegex);
            if (re.IsMatch(inputEmail))
                return (true);
            else
                return (false);
        }

        public string Send_Email(string SendFrom, string SendTo, string Subject, string Body)
        {
            try
            {
                System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");

                bool result = regex.IsMatch(SendTo);
                if (result == false)
                {
                    return "Địa chỉ email không hợp lệ.";
                }
                else
                {
                    //System.Net.Mail.SmtpClient smtp = new SmtpClient();
                    SmtpClient smtp = new SmtpClient("localhost");
                    System.Net.Mail.MailMessage msg = new MailMessage(SendFrom, SendTo, Subject, Body);
                    msg.IsBodyHtml = true;
                    smtp.EnableSsl = true;
                    smtp.Port = 587;
                    smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                    smtp.Host = "smtp.gmail.com"; 
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential("huytq0601@yahoo.com.vn", "190601");
                    smtp.Send(msg);
                    return "Email đã được gửi đến: " + SendTo + ".";
                }
            }
            catch
            {
                return "";
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //DateTime timetamp = DateTime.Now;

            //emailDevice.USBOpen(str_device);
            //emailDevice.Read_Sensor();

            //bool hasOver = false;
            //string nextData = "";
            //string tempData = "";
            //string tempTime = "";

            //for (int i = 0; i < emailDevice.numOfChannel; i++)
            //{
            //    //nextData += ",";
            //    if (emailDevice.channels[i].Val > emailDevice.channels[i].AlarmMax || emailDevice.channels[i].Val < emailDevice.channels[i].AlarmMin)
            //    {
            //        timetamp = DateTime.Now;
            //        //tempTime += timetamp.ToString() + ",";
            //        //nextData += timetamp.ToString() + ",";
            //        tempData += emailDevice.channels[i].Val + ",";
            //        //nextData += emailDevice.Channels[i].Val + ",";
            //        hasOver = true;
            //    }
            //    else
            //    {
            //        //nextData += ",";
            //        tempData += ",";
            //    }
            //    //tempTime += timetamp.ToString() + ",";
            //}
            ////nextData += Environment.NewLine;
            //if (hasOver)
            //{
            //    nextData += timetamp.ToString() + "," + ",";
            //    nextData += tempData;
            //    using (StreamWriter sw = new StreamWriter(FilePath, true))
            //    {
            //        sw.WriteLine(nextData);
            //    }  
            //    //System.IO.File.WriteAllText(FilePath, nextData);
            //}
            //nextData += Environment.NewLine;
            //emailDevice.Close();
        }

        private void EmailSetting_Load(object sender, EventArgs e)
        {
            CenterToScreen();
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


            emailDevice = Device35.DelInstance();
            emailDevice = Device35.Instance;
            byte[] buf = new byte[600];
            string emails = "";
            string[] email;

            //emailDevice.USBOpen(host);
            for (int i = 0; i < 10; i++)
            {
                emailDevice.USBOpen(host);
                byte[] data = new byte[64];
                if (!emailDevice.readEmailSetting(ref data, i + 1))
                {
                    MessageBox.Show(res_man.GetString("Read Email Setting fail", cul));
                    emailDevice.Close();
                    return;
                }
                else
                {
                    for (int j = 0; j < 60; j++)
                    {
                        buf[60 * i + j] = data[j + 2];
                    }
                }
                emailDevice.Close();
            }

            //emailDevice.Close();
            int count = 0;
            for (int i = 0; i < buf.Length; i++)
            {
                if (buf[i] != 255 && buf[i] != 0)
                {
                    count += 1;
                }
            }

            emails = mGlobal.ArrayToStr(ref buf, 0, count);
            email = emails.Split(';');
            //textBox1.Text = email[0];
            richTextBox1.Clear();
            for (int i = 0; i < email.Length; i++)
            {
                richTextBox1.Text += email[i] + Environment.NewLine;
            }

            if (richTextBox1.Text != "")
            {
                richTextBox1.Text = richTextBox1.Text.Substring(0, richTextBox1.Text.Length - 1);
            }


            //Read SMS
            byte[] SMSlist = new byte[200];
            for (int i = 0; i < 5; i++)
            {
                emailDevice.USBOpen(host);
                byte[] data = new byte[44];
                if (!emailDevice.readSMSSetting(ref data, i + 1))
                {
                    MessageBox.Show("Read SMS Setting fail");
                    emailDevice.Close();
                    return;
                }
                else
                {
                    for (int j = 0; j < 40; j++)
                    {
                        SMSlist[40 * i + j] = data[j + 2]; //bo report + byte length
                    }
                }
                emailDevice.Close();
            }
            rtxtSMS.Clear();

            byte[] singleSMS = new byte[20];
            for (int i = 0; i < 10; i++)
            {
                Array.Copy(SMSlist, i*20, singleSMS, 0, 20);
                string singleText = Encoding.ASCII.GetString(singleSMS).Replace('\0', ' ').Trim();
                if (singleText.Length > 0)
                {
                    rtxtSMS.Text += singleText + Environment.NewLine;
                }
            }

            string FileName = "";
            FileName = mGlobal.app_patch(FileName);

            FileName += "\\dataEmail.txt";

            if (System.IO.File.Exists(FileName))
            {
                btnRead.Enabled = true;
            }
            else
            {
                btnRead.Enabled = false;
            }
        }

        private void richTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            int count = 0;

            if (e.KeyCode == Keys.Enter)
            {
                if (richTextBox1.Lines.Count() >= 10)
                {
                    MessageBox.Show(res_man.GetString("Maximum: 10 emails", cul));
                    e.Handled = true;
                    return;
                }
                count += 1;
                if (richTextBox1.Text == "\n")
                {
                    richTextBox1.Text = "";
                }
                if (richTextBox1.Text == "")
                {
                    e.Handled = true;
                    return;
                }
                int tamp = 0;
                string[] lines = richTextBox1.Text.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
                foreach (string item in lines)
                {
                    if (!isValidEmail(item))
                    {
                        MessageBox.Show(res_man.GetString("Email address not formatted correctly", cul));
                        if (tamp == 0)
                        {
                            richTextBox1.Text = richTextBox1.Text.Replace(item, "");
                            richTextBox1.SelectionStart = richTextBox1.Text.Length;
                            e.Handled = true;
                        }
                        else
                        {
                            if (item != "")
                            {
                                richTextBox1.Text = richTextBox1.Text.Replace("\n" + item, "");
                            }
                            else
                            {
                                richTextBox1.Text = richTextBox1.Text.Substring(0, richTextBox1.Text.Length - 1);
                            }
                            richTextBox1.SelectionStart = richTextBox1.Text.Length;
                        }
                    }
                    tamp += 1;
                }
            }
        }

        //private void textBox1_Leave(object sender, EventArgs e)
        //{
        //    if (!isValidEmail(textBox1.Text))
        //    {
        //        MessageBox.Show(res_man.GetString("Email address not formatted correctly", cul));
        //        textBox1.Text = "";
        //        textBox1.Focus();
        //    }
        //}

        //private void textBox1_KeyDown(object sender, KeyEventArgs e)
        //{
        //    if (e.KeyCode == Keys.Enter && textBox1.Text != "")
        //    {
        //        richTextBox1.Focus();
        //    }
        //}

        private void btnRead_Click(object sender, EventArgs e)
        {
            string FileName = "";
            FileName = mGlobal.app_patch(FileName);

            FileName += "\\dataEmail.txt";

            string emails = "";
            string[] email;

            if (System.IO.File.Exists(FileName))
            {
                FileStream fs = new FileStream(FileName, FileMode.Open, FileAccess.Read);

                using (StreamReader sw = new StreamReader(fs))
                {
                    emails += sw.ReadToEnd();
                }

                email = Regex.Split(emails, Environment.NewLine);
                //textBox1.Text = email[0];
                richTextBox1.Clear();
                for (int i = 0; i < email.Length; i++)
                {
                    richTextBox1.Text += email[i] + Environment.NewLine;
                }

                if (richTextBox1.Text != "")
                {
                    richTextBox1.Text = richTextBox1.Text.Substring(0, richTextBox1.Text.Length - 1);
                }
            }
        }

        private void saveEmail(string strEmail)
        {
            strEmail = strEmail.Replace(";", Environment.NewLine);

            string FileName = "";
            FileName = mGlobal.app_patch(FileName);

            FileName += "\\dataEmail.txt";


            FileStream fs = null;

            if (System.IO.File.Exists(FileName))
            {
                fs = new FileStream(FileName, FileMode.Truncate, FileAccess.Write);
            }
            else
            {
                fs = new FileStream(FileName, FileMode.OpenOrCreate, FileAccess.Write);
            }

            using (StreamWriter sw = new StreamWriter(fs))
            {
                sw.Write(strEmail);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSaveSMS_Click(object sender, EventArgs e)
        {
            emailDevice.USBOpen(host);

            if (emailDevice.readSettingDevice() == false)
            {
                emailDevice.Close();
                return;
            }

            emailDevice.Close();

            if (emailDevice.byteLogging == 0x44)
            {
                MessageBox.Show("Logger is recording. Cannot write setting email");
                return;
            }

            emailDevice.USBOpen(host);

            string mySMS = rtxtSMS.Text.Trim();

            if (mySMS.Length > 210)
            {
                //MessageBox.Show(res_man.GetString("Too many SMS recipients. Only 10 SMS recipients are allowed", cul));
                MessageBox.Show("Too many SMS recipients. Only 10 SMS recipients are allowed");
                emailDevice.Close();
                return;
            }

            string[] SMSlist = mySMS.Split('\n');
            string[] fullSMS = new string[10];
            for (int i = 0; i < 10; i++)
            {
                fullSMS[i] = String.Empty;   
            }

            for (int i = 0; i < SMSlist.Length; i++)
            {
                if (SMSlist[i].Length > 20)
                {
                    MessageBox.Show("SMS number cannot be over 20 characters");
                    emailDevice.Close();
                    return;
                }

                fullSMS[i] = SMSlist[i];
            }

            byte[] writeSMS = new byte[44];
            for (int i = 0; i < 5; i++)
            {
                Array.Clear(writeSMS, 0, writeSMS.Length);

                writeSMS[0] = 0x01;
                writeSMS[1] = 0xb7;
                writeSMS[2] = 0x00;
                writeSMS[3] = Convert.ToByte(i + 1);
                byte[] byteSMS1 = Encoding.ASCII.GetBytes(fullSMS[2 * i]);
                byte[] byteSMS2 = Encoding.ASCII.GetBytes(fullSMS[2 * i + 1]);
                for (int j = 0; j < byteSMS1.Length; j++)
                {
                    writeSMS[j + 4] = byteSMS1[j];
                }

                for (int j = 0; j < byteSMS2.Length; j++)
                {
                    writeSMS[j + 24] = byteSMS2[j];
                }

                if (!emailDevice.writeSMSSetting(writeSMS))
                {
                    emailDevice.Close();
                    MessageBox.Show("Write SMS fail");
                    return;
                }
            }

            emailDevice.Close();
            MessageBox.Show("Write SMS successfully");
        }


        //private void richTextBox1_Enter(object sender, EventArgs e)
        //{
        //    string[] lines = richTextBox1.Text.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
        //    foreach (string item in lines)
        //    {
        //        if (!isValidEmail(item))
        //        {
        //            MessageBox.Show("email ko hop le");
        //            richTextBox1.Text = richTextBox1.Text.Replace(item, "");
        //        }
        //    }
        //}


    }
}
