using System;
using System.Collections;
using System.Globalization;
using System.Resources;
using System.Windows.Forms;

namespace Pexo16
{
    public partial class MultiSetting : Form
    {
        ArrayList checkedDevice = new ArrayList();
        string typeDev = "";
        Device35 dev35 = null;

        ResourceManager res_nam = new ResourceManager("Pexo16.Lang.Resources", typeof(MultiSetting).Assembly);
        CultureInfo cul;

        public MultiSetting(string tmp)
        {
            InitializeComponent();
            typeDev = tmp;
        }

        private void btnScanDevice_Click(object sender, EventArgs e)
        {
            checkedListBox1.Items.Clear();
            string px35 = "";
            string px16 = "";

            getDeviceInfo.getActiveDevice();

            for (int i = 0; i < getDeviceInfo.activeDeviceListAl.Count; i++)
            {
                dev35.USBOpen(getDeviceInfo.activeDeviceListAl[i].ToString());
                px35 = getDeviceInfo.nhanDang(dev35.dev);
                px16 = getDeviceInfo.activeSerialAL[i].ToString().Substring(0, 1);
                
            
                if(px16 == typeDev)
                    checkedListBox1.Items.Add(getDeviceInfo.activeDeviceListAl[i]);
                  
                        
                else if(px35 == typeDev)
                    checkedListBox1.Items.Add(getDeviceInfo.activeDeviceListAl[i]);
            
                        
                else if(px16 != "8" && px35 != "PEXO-35" && typeDev == "PEXO-34")
                    checkedListBox1.Items.Add(getDeviceInfo.activeDeviceListAl[i]);

                dev35.Close();
            }
        }

        private void MultiSetting_Load(object sender, EventArgs e)
        {
            switch(mGlobal.language)
            {
                case "Spanish":
                    cul = CultureInfo.CreateSpecificCulture("es");
                    break;
                case "Korean":
                    cul = CultureInfo.CreateSpecificCulture("ko-KR");
                    break;
                case "Japanese":
                    cul = CultureInfo.CreateSpecificCulture("ja-JP");
                    break;
                default:
                    cul = CultureInfo.CreateSpecificCulture("en");
                    break;
            }

            this.Text = res_nam.GetString("MultiSetting", cul);
            btnCheckAll.Text = res_nam.GetString("Check All", cul);
            btnScanDevice.Text = res_nam.GetString("Scan Device", cul);
            btnOk.Text = res_nam.GetString("Ok", cul);


            CenterToScreen();
            dev35 = Device35.DelInstance();
            dev35 = Device35.Instance;
            btnScanDevice_Click(sender, e);
        }

        private void btnCheckAll_Click(object sender, EventArgs e)
        {
            ArrayList temAl = new ArrayList();
            temAl.Clear();
            if (btnCheckAll.Text == res_nam.GetString("Check All", cul))
            {
                for (int i = 0; i < checkedListBox1.Items.Count; i++)
                {
                    checkedListBox1.SetItemChecked(i, true);
                    temAl.Add(checkedListBox1.Items[i].ToString().Substring(checkedListBox1.Items[i].ToString().Length - 23).Substring(0, 10));
                }
                btnCheckAll.Text = res_nam.GetString("Uncheck All", cul);
            }
            else
            {
                for (int i = 0; i < checkedListBox1.Items.Count; i++)
                {
                    checkedListBox1.SetItemCheckState(i, CheckState.Unchecked);
                }
                btnCheckAll.Text = res_nam.GetString("Check All", cul);
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (checkedListBox1.CheckedItems.Count == 0)
            {
                MessageBox.Show(res_nam.GetString("Please select at least one device to continue", cul));
            }
            else
            {
                for (int i = 0; i < checkedListBox1.Items.Count; i++)
                {
                    if (checkedListBox1.GetItemChecked(i) == true && checkedListBox1.GetItemCheckState(i) != CheckState.Indeterminate)
                    {
                        checkedDevice.Add(checkedListBox1.Items[i].ToString());
                    }
                }

                if (typeDev == "8")
                {
                    MultiLoggerIni logger = new MultiLoggerIni(checkedDevice);
                    logger.ShowDialog();
                }
                else if(typeDev == "PEXO-35")
                {
                    MultiLogginIni35 logger = new MultiLogginIni35(checkedDevice);
                    logger.ShowDialog();
                }
             
                this.Close();
            }
        }

        private void checkedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            string itemName = checkedListBox1.Items[e.Index].ToString();
            //string Serial = "";
            //string type = "";
        }

    }
}
