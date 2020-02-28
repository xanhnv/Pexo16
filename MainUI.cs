using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using System.Windows.Forms;

namespace Pexo16
{
    public partial class MainUI : Form
    {
        public ArrayList perfectLoggerAL = new ArrayList();
        public ArrayList allLoggerDescAL = new ArrayList();
        public ArrayList allLoggerValueAL = new ArrayList();
        public ArrayList perfectMenuAL = new ArrayList();
        public ArrayList activeDevicelistAL = new ArrayList();
        public ArrayList tempAL = new ArrayList();
        public ArrayList activeSerialAL = new ArrayList();


        //openfile
        public string bytenhandang;
        public string b;
        public int dem;

        Graph grapFromFile;
        Device openDevice = null;
        Device35 device35 = null;

        Device35 logDevice = null;
        LoggerIni_35 loggerIni35 = null;
        ToolStripItem menuSender = null;

        bool ngang;
        bool doc;


        ResourceManager res_man;
        ComponentResourceManager res;
        CultureInfo cul;


        public MainUI()
        {
            InitializeComponent();
            backgroundWorker1.WorkerSupportsCancellation = true;

        }

        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            for (int i = 1; (i <= 10); i++)
            {
                if ((worker.CancellationPending == true))
                {
                    e.Cancel = true;
                    break;
                }
                else
                {
                    // Perform a time consuming operation and report progress.
                    System.Threading.Thread.Sleep(500);
                    worker.ReportProgress((i * 10));
                }
            }
        }

        private void MainUI_Load(object sender, EventArgs e)
        {
            switch (mGlobal.language)
            {
                case "Spanish":
                    res_man = new ResourceManager("Pexo16.Lang.Resources", typeof(MainUI).Assembly);


                    cul = CultureInfo.CreateSpecificCulture("es-ES");
                    break;
                case "Korean":
                    res_man = new ResourceManager("Pexo16.Lang.Resources", typeof(MainUI).Assembly);
                    cul = CultureInfo.CreateSpecificCulture("ko-KR");
                    break;
                case "Japanese":
                    res_man = new ResourceManager("Pexo16.Lang.Resources", typeof(MainUI).Assembly);
                    cul = CultureInfo.CreateSpecificCulture("ja-JP");
                    break;
                default:
                    res_man = new ResourceManager("Pexo16.Lang.Resources", typeof(MainUI).Assembly);
                    cul = CultureInfo.CreateSpecificCulture("en-US");
                    break;
            }

            fileToolStripMenuItem.Text = res_man.GetString("File", cul);
            openToolStripMenuItem.Text = res_man.GetString("Open", cul);
            saveToolStripMenuItem.Text = res_man.GetString("Save", cul);
            saveAsToolStripMenuItem.Text = res_man.GetString("Save as", cul);
            closeToolStripMenuItem.Text = res_man.GetString("Close", cul);
            exportDataToolStripMenuItem.Text = res_man.GetString("Export data", cul);
            printToolStripMenuItem.Text = res_man.GetString("Print", cul);
            exitToolStripMenuItem.Text = res_man.GetString("Exit", cul);
            preferencesToolStripMenuItem.Text = res_man.GetString("General setting", cul);
            menu_logger.Text = res_man.GetString("Logger", cul);
            graphToolStripMenuItem.Text = res_man.GetString("Graph", cul);
            windowsToolStripMenuItem.Text = res_man.GetString("Windows", cul);
            helpToolStripMenuItem.Text = res_man.GetString("Help", cul);
            logger_alllogger.Text = res_man.GetString("Bulk programming", cul);
            viewSummaryInfomationToolStripMenuItem.Text = res_man.GetString("View Summary Infomation", cul);
            viewDataToolStripMenuItem.Text = res_man.GetString("View Data", cul);
            copyToClipboardToolStripMenuItem.Text = res_man.GetString("Copy To Clipboard", cul);
            editToolStripMenuItem.Text = res_man.GetString("Edit", cul);
            gridToolStripMenuItem.Text = res_man.GetString("Grid", cul);
            titleToolStripMenuItem.Text = res_man.GetString("Title", cul);
            colorToolStripMenuItem.Text = res_man.GetString("Color", cul);
            toolbarToolStripMenuItem.Text = res_man.GetString("Toolbar", cul);
            fullScreenToolStripMenuItem.Text = res_man.GetString("Full screen", cul);
            cascadToolStripMenuItem.Text = res_man.GetString("Cascade all", cul);
            websiteMarathonToolStripMenuItem.Text = res_man.GetString("Website Marathon", cul);
            aboutToolStripMenuItem.Text = res_man.GetString("About", cul);
            LoggerInitializeToolStripMenuItem.Text = res_man.GetString("Logger Initialization", cul);
            realtimeToolStripMenuItem.Text = res_man.GetString("Realtime", cul);


            tsbtnOpen.ToolTipText = res_man.GetString("Open", cul);
            tsbtnSave.ToolTipText = res_man.GetString("Save", cul);
            tsbtnSaveAs.ToolTipText = res_man.GetString("Save as", cul);
            tsbtnExportData.ToolTipText = res_man.GetString("Export data", cul);
            tsbtnPrintReview.ToolTipText = res_man.GetString("Print Preview", cul);
            tsbtnChangeColor.ToolTipText = res_man.GetString("Change Color", cul);
            tsbtnHorizontal.ToolTipText = res_man.GetString("Horizontal Grid", cul);
            tsbtnVertical.ToolTipText = res_man.GetString("Vertical Grid", cul);
            tsbtnEditText.ToolTipText = res_man.GetString("Edit Title", cul);
            tsbtnLegend.ToolTipText = res_man.GetString("Show/Hide Legend", cul);
            tsbtnInformation.ToolTipText = res_man.GetString("Summary Information", cul);
            tsbtnViewData.ToolTipText = res_man.GetString("View Data", cul);
            tsbtnEditComment.ToolTipText = res_man.GetString("Edit Comment", cul);
            tsbtnClipboard.ToolTipText = res_man.GetString("Copy To Clipboard", cul);
            tsbtnShowValue.ToolTipText = res_man.GetString("Show Value", cul);
            tsbtnTimeElapse.ToolTipText = res_man.GetString("Elapsed Time on X Axis", cul);
            tsbtnChangeUnit.ToolTipText = res_man.GetString("Change Unit", cul);


            ////foreach(Control c in this.Controls)
            ////{
            ////    res.ApplyResources(c, c.Name, cul);
            ////}
            //mGlobal.language = "spanish";

            //if (mGlobal.language == "spanish")
            //{
            //    //foreach (Control c in this.Controls)
            //    //{
            //    //    ComponentResourceManager resources = new ComponentResourceManager(typeof(MainUI));
            //    //    resources.ApplyResources(c, c.Name, new CultureInfo("es"));
            //    //}

            //    res_man = new ResourceManager("Pexo16.Lang.Res", typeof(MainUI).Assembly);
            //    ////res = new ComponentResourceManager(typeof(MainUI));
            //    cul = CultureInfo.CreateSpecificCulture("es");

            //    fileToolStripMenuItem.Text = res_man.GetString("File", cul);
            //    openToolStripMenuItem.Text = res_man.GetString("Open", cul);
            //    saveToolStripMenuItem.Text = res_man.GetString("Save", cul);
            //    saveAsToolStripMenuItem.Text = res_man.GetString("Save as", cul);
            //    closeToolStripMenuItem.Text = res_man.GetString("Close", cul);
            //    exportDataToolStripMenuItem.Text = res_man.GetString("Export data", cul);
            //    printToolStripMenuItem.Text = res_man.GetString("Print", cul);
            //    exitToolStripMenuItem.Text = res_man.GetString("Exit", cul);
            //    preferencesToolStripMenuItem.Text = res_man.GetString("General setting", cul);

            //    ////Thread.CurrentThread.CurrentUICulture = new CultureInfo("es");
            //    //foreach (Control c in this.Controls)
            //    //{
            //    //    try
            //    //    {
            //    //        c.Text = res_man.GetString(c.Text, cul);
            //    //    }
            //    //    catch
            //    //    { }
            //    //}
            //}

            //getDeviceInfo.getActiveDevice();
            //getData.getLogger();
            mGlobal.LoadTimeZoneFromSystem_ComboBox(ref tscb_TiZo);
            tscb_TiZo.Enabled = true;

            toolbarToolStripMenuItem.Checked = true;
            enableToolButton(false);

            //if (backgroundWorker1.IsBusy != true)
            //{
            //    backgroundWorker1.RunWorkerAsync();
            //}
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            //object a = new object();
            ToolStripItem menuSender = (ToolStripMenuItem)sender;
            ToolStripItem menuLogger = menuSender.OwnerItem;
            Device35 logDevice = null;

            mGlobal.open_file = false;
            mGlobal.activeDevice = false;

            for (int i = 0; i <= getDeviceInfo.activeSerialAL.Count - 1; i++)
            {
                if (perfectLoggerAL[i].ToString() == menuLogger.Name.ToString())
                {
                    LoggerIni_35 loggerIni35 = new LoggerIni_35(getDeviceInfo.activeDeviceListAl[i].ToString());

                    logDevice = loggerIni35.readLogger();

                    this.Cursor = Cursors.Default;

                    if (logDevice != null) logDevice.Close();
                    else
                    {
                        //MessageBox.Show(res_man.GetString("Read data fail! Please try to read logger again", cul));
                        //logDevice.Close();
                        mGlobal.activeDevice = true;
                        return;
                    }
                }
            }
        }

        public void updateActiveMenu()
        {
            int tmp = 0;

            menu4S.DropDownItems.Clear();
            if (mGlobal.activeDevice)
            {
            Line0: getDeviceInfo.getActiveDevice();

            Device35 dev35 = null;
            dev35 = Device35.DelInstance();
            dev35 = Device35.Instance;

            perfectLoggerAL.Clear();

            for (int i = 0; i < getDeviceInfo.activeDeviceListAl.Count; i++)
            {
                if (getDeviceInfo.activeDeviceListAl[i] != null)
                {
                    perfectLoggerAL.Add(getDeviceInfo.activeSerialAL[i]);
                }
            }

            for (int i = this.menu8S.DropDownItems.Count -1; i >= 0; i--)
            {
                this.menu8S.DropDownItems.Remove(this.menu8S.DropDown.Items[i]);
            }

            for (int i = this.menu4S.DropDownItems.Count - 1; i >= 0; i--)
            {
                this.menu4S.DropDownItems.Remove(this.menu4S.DropDown.Items[i]);
            }

            for (int i = this.menuPexo34.DropDownItems.Count - 1; i >= 0; i--)
            {
                this.menuPexo34.DropDownItems.Remove(this.menuPexo34.DropDown.Items[i]);
            }

            //----------------add toolstrip
            if (perfectLoggerAL.Count == 0 && tmp <= 2)
            {
                tmp += 1;
                goto Line0;
            }

                for (int i = 0; i <= perfectLoggerAL.Count - 1; i++)
                {
                    ToolStripMenuItem[] menuLogger = new ToolStripMenuItem[perfectLoggerAL.Count + 1];
                    menuLogger[i] = new ToolStripMenuItem();

                    try
                    {
                        dev35.USBOpen(getDeviceInfo.activeDeviceListAl[i].ToString());
                    }
                    finally
                    {
                        //
                        menuLogger[i].Text = perfectLoggerAL[i].ToString();
                        menuLogger[i].Name = perfectLoggerAL[i].ToString();
                        string tempSerial = perfectLoggerAL[i].ToString();


                        if (tempSerial != "")
                        {
                            if (tempSerial.Substring(0, 1) == "8")
                            {
                                menu8S.DropDownItems.Add(menuLogger[i]);
                            }
                            else if (dev35.dev != 0)
                            {
                                string name = getDeviceInfo.nhanDang(dev35.dev);
                                if (name == "PEXO-35")
                                {
                                    menuLogger[i].Text = getDeviceInfo.readSerial(dev35.dev);
                                    menu4S.DropDownItems.Add(menuLogger[i]);
                                }
                                else if (getDeviceInfo.nhanDang34(dev35.dev) == "PEXO-34")
                                {
                                    menuPexo34.DropDownItems.Add(menuLogger[i]);
                                }
                            }
                        }

                        ToolStripMenuItem menuLoggerInit = new ToolStripMenuItem();
                        menuLoggerInit = new ToolStripMenuItem();
                        menuLoggerInit.Text = res_man.GetString("Logger Initialization", cul);
                        menuLoggerInit.Name = menuLogger[i].Name + "_" + menuLoggerInit.Text;

                        ToolStripMenuItem menuReadLogger = new ToolStripMenuItem();
                        menuReadLogger = new ToolStripMenuItem();
                        menuReadLogger.Text = res_man.GetString("Read Logger", cul);
                        menuReadLogger.Name = menuLogger[i].Name + "_" + menuReadLogger.Text;

                        ToolStripMenuItem menuCalib35 = new ToolStripMenuItem();
                        menuCalib35 = new ToolStripMenuItem();
                        menuCalib35.Text = res_man.GetString("Calib", cul);
                        menuCalib35.Name = menuLogger[i].Name + "_" + menuCalib35.Text;

                        ToolStripMenuItem menuGenInfo = new ToolStripMenuItem();
                        menuGenInfo = new ToolStripMenuItem();
                        ToolStripMenuItem menuitem14 = new ToolStripMenuItem();
                        menuGenInfo.Text = res_man.GetString("General Information", cul);
                        menuGenInfo.Name = menuLogger[i].Name + "_" + menuGenInfo.Text;

                        ToolStripMenuItem menuSetting34 = new ToolStripMenuItem();
                        menuSetting34.Text = res_man.GetString("Setting", cul);
                        menuSetting34.Name = menuLogger[i].Name + "_" + menuSetting34.Text;

                        if (tempSerial != "")
                        {
                            if (tempSerial.Substring(0, 1) == "8")
                            {
                                mGlobal.len = 64;

                                menuLogger[i].DropDownItems.Add(menuLoggerInit);
                                menuLoggerInit.Click += this.menuLoggerInit_Click;

                                menuLogger[i].DropDownItems.Add(menuReadLogger);
                                menuReadLogger.Click += this.menuReadLogger_Click;

                                menuLogger[i].DropDownItems.Add(menuGenInfo);
                                menuGenInfo.Click += this.menuGenInfo_Click;
                            }
                            else if (dev35.dev != 0)
                            {
                                if (getDeviceInfo.nhanDang(dev35.dev) == "PEXO-35")
                                {
                                    mGlobal.len = 64;

                                    //dev35.Close();
                                    menuLogger[i].DropDownItems.Add(menuLoggerInit);
                                    //dev35.Close();
                                    menuLoggerInit.Click += this.menuLoggerInit35_Click;

                                    menuLogger[i].DropDownItems.Add(menuReadLogger);
                                    menuReadLogger.Click += this.menuReadLogger35_Click;

                                    menuLogger[i].DropDownItems.Add(menuCalib35);
                                    menuCalib35.Click += this.menuCalib35_Click;

                                    menuLogger[i].DropDownItems.Add(menuGenInfo);
                                    menuGenInfo.Click += this.menuGenInfo35_Click;
                                }
                                else if (getDeviceInfo.nhanDang34(dev35.dev) == "PEXO-34")//setting pexo34
                                {
                                    mGlobal.len = 65;

                                    menuLogger[i].DropDownItems.Add(menuSetting34);
                                    menuSetting34.Click += this.menuSetting34_Click;

                                    menuLogger[i].DropDownItems.Add(menuLoggerInit);
                                    menuLoggerInit.Click += this.menuLoggerInit35_Click;
                                }
                            }
                        }
                    }
                    dev35.Close();
                }
            }
            else
            {
                    MessageBox.Show(res_man.GetString("One file is opening!", cul));
            }
        }

        private void menuCalib35_Click(object sender, EventArgs e)
        {
            ToolStripItem menuSender = (ToolStripMenuItem)sender;
            ToolStripItem menuLogger = menuSender.OwnerItem;

            for (int i = 0; i <= getDeviceInfo.activeSerialAL.Count - 1; i++)
            {
                if (perfectLoggerAL[i].ToString() == menuLogger.Name.ToString())
                {
                   //Calib35 calib = new Calib35(getDeviceInfo.activeDeviceListAl[i].ToString());
                   Calibrations calib = new Calibrations(getDeviceInfo.activeDeviceListAl[i].ToString());
                  //Calibration34 calib = new Calibration34(getDeviceInfo.activeDeviceListAl[i].ToString());
                    calib.ShowDialog();
                }
            }
        }

        private void menuSetting34_Click(object sender, EventArgs e)
        {
            ToolStripItem menuSender = (ToolStripMenuItem)sender;
            ToolStripItem menuLogger = menuSender.OwnerItem;

            mGlobal.open_file = false;

            for (int i = 0; i <= getDeviceInfo.activeSerialAL.Count - 1; i++)
            {
                if (perfectLoggerAL[i].ToString() == menuLogger.Name.ToString())
                {
                    _34Setting set34 = new _34Setting(getDeviceInfo.activeDeviceListAl[i].ToString());
                    set34.ShowDialog();
                    //LoggerIni loggerIni = new LoggerIni(getDeviceInfo.activeDeviceListAl[i].ToString());
                    //loggerIni.btnReadSetting_Click(sender, e);
                }
            }
        }

        //private void menuSumInfo_Click(object sender, EventArgs e)
        //{        }

        private void menuReadLogger_Click(object sender, EventArgs e)
        {
            ToolStripItem menuSender = (ToolStripMenuItem)sender;
            ToolStripItem menuLogger = menuSender.OwnerItem;
            Device logDevice = null;

            mGlobal.open_file = false;
            mGlobal.activeDevice = false;

            for (int i = 0; i <= getDeviceInfo.activeSerialAL.Count - 1; i++)
            {
                if (perfectLoggerAL[i].ToString() == menuLogger.Name.ToString())
                {
                    //ArrayList hostPort = new ArrayList();
                    //hostPort.Add(mGlobal.perfectItemAL[i].ToString());
                    //LoggerIni loggerIni = new LoggerIni(hostPort);
                    LoggerIni loggerIni = new LoggerIni(getDeviceInfo.activeDeviceListAl[i].ToString());
                    loggerIni.btnReadSetting_Click(sender, e);
                    logDevice =  loggerIni.readLogger();
                    if (logDevice != null) logDevice.Close();                      
                }
            }

            if (logDevice != null)
            {
                grapFromFile = Graph.DelInstance();
                grapFromFile = Graph.Instance;
                mGlobal._get_timezone_date(ref grapFromFile._timezone, ref logDevice._logger_date, logDevice.data_open);
                grapFromFile.FormClosed += new FormClosedEventHandler(childformClose);
                grapFromFile.MdiParent = this;
                grapFromFile.WindowState = FormWindowState.Maximized;
                grapFromFile.draw_graph();
                grapFromFile.Show();
                enableToolButton(true);
                tscb_TiZo.Text = grapFromFile._timezone.ToString();
            }
            else
            {
                return;
            }
        }

        private void menuReadLogger35_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            ToolStripItem menuSender = (ToolStripMenuItem)sender;
            ToolStripItem menuLogger = menuSender.OwnerItem;
            Device35 logDevice = null;

            mGlobal.open_file = false;
            mGlobal.activeDevice = false;

            //backgroundWorker1.RunWorkerAsync();

            for (int i = 0; i <= getDeviceInfo.activeSerialAL.Count - 1; i++)
            {
                if (perfectLoggerAL[i].ToString() == menuLogger.Name.ToString())
                {
                    LoggerIni_35 loggerIni35 = new LoggerIni_35(getDeviceInfo.activeDeviceListAl[i].ToString());

                    logDevice = loggerIni35.readLogger();

                    //loggerIni35.test();
                    this.Cursor = Cursors.Default;
                   
                    if (logDevice != null) logDevice.Close();
                    else
                    {
                        //MessageBox.Show(res_man.GetString("Read data fail! Please try to read logger again", cul));
                        //logDevice.Close();
                        mGlobal.activeDevice = true;
                        return;
                    }
                }
            }

            if (logDevice != null)
            {
                grapFromFile = Graph.DelInstance();
                grapFromFile = Graph.Instance;
                mGlobal._get_timezone_date(ref grapFromFile._timezone, ref logDevice._logger_date, logDevice.data_open);
                grapFromFile.FormClosed += new FormClosedEventHandler(childformClose);
                grapFromFile.MdiParent = this;
                grapFromFile.WindowState = FormWindowState.Maximized;

                //thu set mGlobal.C2F o day xem sao?
                grapFromFile.draw_graph35();
                grapFromFile.Show();
                enableToolButton(true);
                tscb_TiZo.Text = grapFromFile._timezone.ToString();
                logDevice.Close();
            }
            else
            {
                return;
            }
        }

        public void menuGenInfo_Click(object sender, EventArgs e)
        {
            ToolStripItem menuSender = (ToolStripMenuItem)sender;
            ToolStripItem menuLogger = menuSender.OwnerItem;

            mGlobal.open_file = false;

            for (int i = 0; i < getDeviceInfo.activeDeviceListAl.Count; i++)
            {
                if (perfectLoggerAL[i].ToString() == menuLogger.Name.ToString())
                {
                    string temp = getDeviceInfo.activeDeviceListAl[i].ToString();
                    GeneralInfo genInfo = new GeneralInfo(temp);
                    genInfo.ShowDialog();
                }
            }
        }

        public void menuGenInfo35_Click(object sender, EventArgs e)
        {
            ToolStripItem menuSender = (ToolStripMenuItem)sender;
            ToolStripItem menuLogger = menuSender.OwnerItem;

            mGlobal.open_file = false;

            for (int i = 0; i < getDeviceInfo.activeDeviceListAl.Count; i++)
            {
                if (perfectLoggerAL[i].ToString() == menuLogger.Name.ToString())
                {
                    string temp = getDeviceInfo.activeDeviceListAl[i].ToString();
                    GeneralInfo35 genInfo = new GeneralInfo35(temp);
                    genInfo.ShowDialog();
                }
            }
        }

        public void menuLoggerInit35_Click(object sender, EventArgs e)
        {

            ToolStripItem menuSender = (ToolStripMenuItem)sender;
            ToolStripItem menuLogger = menuSender.OwnerItem;

            mGlobal.open_file = false;

            for (int i = 0; i <= getDeviceInfo.activeSerialAL.Count - 1; i++)
            {
                if (perfectLoggerAL[i].ToString() == menuLogger.Name.ToString())
                {
                    //ArrayList hostPort = new ArrayList();
                    //hostPort.Add(mGlobal.perfectItemAL[i].ToString());
                    //LoggerIni loggerIni = new LoggerIni(hostPort);
                    LoggerIni_35 loggerIni35 = new LoggerIni_35(getDeviceInfo.activeDeviceListAl[i].ToString());
                    loggerIni35.Show();
                    //_34Setting set34 = new _34Setting(getDeviceInfo.activeDeviceListAl[i].ToString());
                    //set34.ShowDialog();
                }
            }
        }

        public void menuLoggerInit_Click(object sender, EventArgs e)
        {
            ToolStripItem menuSender = (ToolStripMenuItem)sender;
            ToolStripItem menuLogger = menuSender.OwnerItem;

            mGlobal.open_file = false;

            for (int i = 0; i <= getDeviceInfo.activeSerialAL.Count - 1; i++)
            {
                if (perfectLoggerAL[i].ToString() == menuLogger.Name.ToString())
                {
                    //ArrayList hostPort = new ArrayList();
                    //hostPort.Add(mGlobal.perfectItemAL[i].ToString());
                    //LoggerIni loggerIni = new LoggerIni(hostPort);
                    LoggerIni loggerIni = new LoggerIni(getDeviceInfo.activeDeviceListAl[i].ToString());
                    loggerIni.Show();
                    //_34Setting set34 = new _34Setting(getDeviceInfo.activeDeviceListAl[i].ToString());
                    //set34.ShowDialog();
                }
            }
        }

        public void getActiveDevice()
        {
            activeSerialAL.Clear();
            activeDevicelistAL.Clear();

            byte[] buf = new byte[500];
            string s = "";
            string product = "";
            string serialnumber = "";

            Int64 devs;

            devs = HIDFunction.hid_Enumerate(0, 0);

            HIDFunction a = new HIDFunction();
            while (devs != 0)
            {
                HIDFunction.hid_DeviceSerialNum(devs, ref buf[0]);
                serialnumber = Encoding.UTF8.GetString(buf);

                HIDFunction.hid_DeviceProductString(devs, ref buf[0]);
                product = Encoding.UTF8.GetString(buf);
                ushort temp = HIDFunction.hid_DeviceVendorID(devs);

                s = "VID_" + (HIDFunction.hid_DeviceVendorID(devs));
                s = s + "_PID_" + (HIDFunction.hid_DeviceProductID(devs));
                s = s + "_" + serialnumber;
                s = s + "_" + product;
                if (product == "Datalogger8S")
                {
                    activeDevicelistAL.Add(s);
                    activeSerialAL.Add(serialnumber);
                    mGlobal.usb_id = s;
                    mGlobal.usb_search = true;
                }

                devs = HIDFunction.hid_DeviceNext(devs);
            }
            HIDFunction.hid_FreeEnumeration(devs);
            HIDFunction.hid_Exit();
        }

        private void menu_logger_Click(object sender, EventArgs e)
        {
            //getDeviceInfo.getActiveDevice();

            updateActiveMenu();
        }

        private void btn_ToolbarOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            string defaultFolder = "";
            string path = "";
            path = mGlobal.app_patch(path);
            path += "\\Reference.bin";

            if (File.Exists(path))
            {
                FileStream fs = new FileStream(path, FileMode.OpenOrCreate);
                BinaryReader br = new BinaryReader(fs);
                //str = File.ReadAllText(FileName);
                defaultFolder = br.ReadString();
                fs.Close();
                br.Close();
            }
            if (defaultFolder == "")
            {
                openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            }
            else
            {
                openFileDialog.InitialDirectory = defaultFolder;
            }

            openFileDialog.Filter = "Marathon Logger (*.D8S)|*.D8S|Marathon Logger (*.D4S)|*.D4S|All File (*.*)|*.*";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;


            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                enableToolButton(true);
                //menu_logger.Enabled = false;
                mGlobal.activeDevice = false;

                //-------phai chi-------
                //baseDevice openDevice = null;
                //if(openFileDialog.FileName.Substring(openFileDialog.FileName.IndexOf(".") + 1, 3) == "D8S")
                //{
                //    openDevice = Device16.DelInstance();
                //    openDevice = Device16.Instance;
                //}
                //else // D4S
                //{
                //    openDevice = Device35.DelInstance();
                //    openDevice = Device35.Instance;
                //}
                //
                if (openFileDialog.FileName.Substring(openFileDialog.FileName.IndexOf(".") + 1, 3) == "D8S")
                {
                    openDevice = Device.DelInstance();
                    openDevice = Device.Instance;
                    if (openDevice.OpenFile_MP_Lgr(openFileDialog.FileName.ToString()))
                    {
                        grapFromFile = Graph.DelInstance();
                        grapFromFile = Graph.Instance;
                        mGlobal._get_timezone_date(ref grapFromFile._timezone, ref openDevice._logger_date, openDevice.data_open);
                        tscb_TiZo.Text = grapFromFile._timezone.ToString();
                        grapFromFile.FormClosed += new FormClosedEventHandler(childformClose);
                        //grapFromFile.visibale = new Graph.toolBar(childformClose);
                        grapFromFile.MdiParent = this;
                        grapFromFile.WindowState = FormWindowState.Maximized;
                        grapFromFile.draw_graph();
                        grapFromFile.Show();
                        tscb_TiZo.Text = grapFromFile._timezone.ToString();
                    }
                    else return;
                }
                else //D4S
                {
                    device35 = Device35.DelInstance();
                    device35 = Device35.Instance;
                    if (device35.OpenFile_MP_Lgr(openFileDialog.FileName.ToString()))
                    {
                        mGlobal.drawGraph35 = true;
                        grapFromFile = Graph.DelInstance();
                        grapFromFile = Graph.Instance;
                        mGlobal._get_timezone_date(ref grapFromFile._timezone, ref device35._logger_date, device35.data_open);
                        tscb_TiZo.Text = grapFromFile._timezone.ToString();
                        grapFromFile.FormClosed += new FormClosedEventHandler(childformClose);
                        grapFromFile.MdiParent = this;
                        grapFromFile.WindowState = FormWindowState.Maximized;
                        grapFromFile.draw_graph35();
                        grapFromFile.Show();
                       //tscb_TiZo.Text = grapFromFile._timezone.ToString();
                    }
                    else return;
                }
            }

            this.Text = "Maxithermal Wifi - Marathon Product, Inc - " + openFileDialog.FileName.ToString();
            mGlobal.PathFile = openFileDialog.FileName.ToString();
            mGlobal.ColorChanged = false;
            mGlobal.TitleChanged = false;
            mGlobal.CommentChanged = false;
        }

        private void childformClose(object sender, FormClosedEventArgs e)
        {
            enableToolButton(false);
        }

        private void dashBoardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //getDeviceInfo.getActiveDevice();
            DashBoard dashboard = new DashBoard(this);
            dashboard.ShowDialog();
        }

        public void enableToolStrip(bool state)
        {
            foreach (ToolStripItem item in graphToolStripMenuItem.DropDownItems)
            {
                item.Enabled = state;
            }
        }

        private void MainUI_Resize(object sender, EventArgs e)
        {
            
            //Graph_OpenFile Graph = new Graph_OpenFile(tscb_timezone);
            //Graph.Parent = this;

            //Graph.Width = this.Width - 20 * this.Width / 1382;

            //Graph.Top = 40 * this.Height / 744;

            //Graph.Height = this.Height - 81 * this.Height / 744;

            //Graph.Show();


            //Control control = (Control)sender;

            //// Ensure the Form remains square (Height = Width). 
            //if (control.Size.Height != control.Size.Width)
            //{
            //    control.Size = new Size(control.Size.Width, control.Size.Width);
            //}
        }

        private void tsbtnHorizontal_Click(object sender, EventArgs e)
        {
            if (mGlobal.drawGraph35)
            {
                if (ngang == true)
                {
                    //grapFromFile.chart1.ChartAreas[0].AxisY.MajorGrid.Enabled = ngang;
                    //grapFromFile.chart1.ChartAreas[0].AxisY2.MajorGrid.Enabled = ngang;
                    //grapFromFile.chart1.ChartAreas[1].AxisY.MajorGrid.Enabled = ngang;
                    for (int i = 0; i < grapFromFile.chart1.ChartAreas.Count; i++)
                    {
                        grapFromFile.chart1.ChartAreas[i].AxisY.MajorGrid.Enabled = ngang;
                    }
                    ngang = false;
                }
                else
                {
                    for (int i = 0; i < grapFromFile.chart1.ChartAreas.Count; i++)
                    {
                        grapFromFile.chart1.ChartAreas[i].AxisY.MajorGrid.Enabled = ngang;
                    }
                    //grapFromFile.chart1.ChartAreas[0].AxisY.MajorGrid.Enabled = ngang;
                    //grapFromFile.chart1.ChartAreas[0].AxisY2.MajorGrid.Enabled = ngang;
                    //grapFromFile.chart1.ChartAreas[1].AxisY.MajorGrid.Enabled = ngang;
                    ngang = true;
                }
            }
            else
            {
                if (ngang == true)
                {
                    grapFromFile.chart1.ChartAreas[0].AxisY.MajorGrid.Enabled = ngang;
                    grapFromFile.chart1.ChartAreas[0].AxisY2.MajorGrid.Enabled = ngang;
                    ngang = false;
                }
                else
                {
                    grapFromFile.chart1.ChartAreas[0].AxisY.MajorGrid.Enabled = ngang;
                    grapFromFile.chart1.ChartAreas[0].AxisY2.MajorGrid.Enabled = ngang;
                    ngang = true;
                }
            }
        }

        private void tsbtnVertical_Click(object sender, EventArgs e)
        {
            if (mGlobal.drawGraph35)
            {
                if (doc == true)
                {
                    for (int i = 0; i < grapFromFile.chart1.ChartAreas.Count; i++)
                    {
                        grapFromFile.chart1.ChartAreas[i].AxisX.MajorGrid.Enabled = doc;
                    }
                    //grapFromFile.chart1.ChartAreas[0].AxisX.MajorGrid.Enabled = doc;
                    //grapFromFile.chart1.ChartAreas[1].AxisX.MajorGrid.Enabled = doc;
                    doc = false;
                }
                else
                {
                    for (int i = 0; i < grapFromFile.chart1.ChartAreas.Count; i++)
                    {
                        grapFromFile.chart1.ChartAreas[i].AxisX.MajorGrid.Enabled = doc;
                    }
                    //grapFromFile.chart1.ChartAreas[0].AxisX.MajorGrid.Enabled = doc;
                    //grapFromFile.chart1.ChartAreas[1].AxisX.MajorGrid.Enabled = doc;
                    doc = true;
                }
            }
            else
            {
                if (doc == true)
                {
                    grapFromFile.chart1.ChartAreas[0].AxisX.MajorGrid.Enabled = doc;
                    doc = false;
                }
                else
                {
                    grapFromFile.chart1.ChartAreas[0].AxisX.MajorGrid.Enabled = doc;
                    doc = true;
                }
            }
        }

        private void tsbtnPrintReview_Click(object sender, EventArgs e)
        {
            //Graph.printChart();
            //ViewSumInfoAndChart view = new ViewSumInfoAndChart();
            //view.Show();
            if (mGlobal.drawGraph35)
            {
                //ViewInfo viewInfo = new ViewInfo("sumAndchart35"); // case 4: viewData35
                //viewInfo.Show();
                PreviewData35 pre = new PreviewData35();
                pre.Print_Preview(PreviewData35.Print_content.Print_Graph_and_Summary);
            }
            else
            {
                PreviewData pre = new PreviewData();
                pre.Print_Preview(PreviewData.Print_content.Print_Graph_and_Summary);
            }
        }

        public void enableToolButton(bool visible)
        {
            tsbtnSave.Enabled = visible;
            tsbtnSave.Enabled = visible;
            tsbtnSaveAs.Enabled = visible;
            tsbtnExportData.Enabled = visible;
            tsbtnPrintReview.Enabled = visible;
            //tsbtnInitialization.Enabled = !visible;
            //tsbtnReadLog.Enabled = !visible;
            tsbtnChangeColor.Enabled = visible;
            tsbtnVertical.Enabled = visible;
            tsbtnHorizontal.Enabled = visible;
            tsbtnEditText.Enabled = visible;
            tsbtnLegend.Enabled = visible;
            tsbtnInformation.Enabled = visible;
            tsbtnViewData.Enabled = visible;
            tsbtnEditComment.Enabled = visible;
            tsbtnClipboard.Enabled = visible;
            tsbtnShowValue.Enabled = visible;
            tsbtnTimeElapse.Enabled = visible;
            tsbtnChangeUnit.Enabled = visible;

            saveAsToolStripMenuItem.Enabled = visible;
            closeToolStripMenuItem.Enabled = visible;
            exportDataToolStripMenuItem.Enabled = visible;
            printToolStripMenuItem.Enabled = visible;
            graphToolStripMenuItem.Enabled = visible;
            windowsToolStripMenuItem.Enabled = visible;

            tscb_TiZo.Visible = visible;
            tscb_TiZo.Enabled = visible;

            tsbtnOpen.Enabled = !visible;
            openToolStripMenuItem.Enabled = !visible;
            preferencesToolStripMenuItem.Enabled = !visible;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btn_ToolbarOpen_Click(sender, e);
        }

        private void horizontalGridToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tsbtnHorizontal_Click(sender, e);
        }

        private void verticalGridToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tsbtnVertical_Click(sender, e);
        }

        public void file_save_Click(object sender, System.EventArgs e)
        {
            if (mGlobal.open_file == true)
            {
                byte[] save = { 0 };
                if (mGlobal.PathFile == null)
                {
                    //            file_saveas_Click(null, null);
                }
                else
                {
                    openDevice.SaveFile_MP_Lgr2(ref save);
                    System.IO.File.WriteAllBytes(mGlobal.PathFile.ToString(), save);

                    //ColorChanged = false;
                    //TitleChanged = false;
                    //CommentChanged = false;
                }
            }
            //    else
            //    {
            //        file_saveas_Click(null, null);
            //    }
        }

        private void tsbtnChangeUnit_Click(object sender, EventArgs e)
        {
            if (mGlobal.C2F)
            {
                mGlobal.C2F = false;
            }
            else
            {
                mGlobal.C2F = true;
            }
            if (mGlobal.drawGraph35)
            {
                grapFromFile.draw_graph35();
            }
            else
            {
                grapFromFile.draw_graph();
            }
        }

        private void tsbtnChangeColor_Click(object sender, EventArgs e)
        {
            if (mGlobal.drawGraph35)
            {
                formC changeColor35 = new formC();
                changeColor35.Show();
            }
            else
            {
                ChangeColor changeColor = new ChangeColor();
                changeColor.Show();
            }
        }

        private void tsbtnLegend_Click(object sender, EventArgs e)
        {
            if (grapFromFile.chart1.Legends[0].Enabled == true)
            {
                grapFromFile.chart1.Legends[0].Enabled = false;
                //grapFromFile.chkShowArea1.Visible = false;
                //grapFromFile.chkShowArea2.Visible = false;
                for (int i = 0; i < grapFromFile.chart1.Controls.Count; i++)
                {
                    grapFromFile.chart1.Controls[i].Visible = false;
                }
            }
            else
            {
                grapFromFile.chart1.Legends[0].Enabled = true;
                if(mGlobal.drawGraph35)
                {
                    for (int i = 0; i < grapFromFile.chart1.Controls.Count; i++)
                    {
                        grapFromFile.chart1.Controls[i].Visible = true;
                    }
                    //grapFromFile.chkShowArea1.Visible = true;
                    //grapFromFile.chkShowArea2.Visible = true;
                }
            }
        }

        private void tsbtnEditText_Click(object sender, EventArgs e)
        {
            EditTitleGraph changeTile = new EditTitleGraph();
            changeTile.Show();
        }

        private void tsbtnTimeElapse_Click(object sender, EventArgs e)
        {
            if (mGlobal.tlb_eclapse == true)
            {
                mGlobal.tlb_eclapse = false;
            }
            else
            {
                mGlobal.tlb_eclapse = true;
            }
            //Graph.draw_graph(mGlobal.data_byte);
            //mGlobal.C2F = true;
            if (mGlobal.drawGraph35)
            {
                grapFromFile.draw_graph35();
            }
            else
            {
                grapFromFile.draw_graph();
            }
        }

        private void tsbtnExportData_Click(object sender, EventArgs e)
        {
            device35 = Device35.Instance;
            openDevice = Device.Instance;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            string FilePath;
            string str = "";
            saveFileDialog1.Filter = "CSV File (*.csv)|*.csv";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                this.Cursor = Cursors.WaitCursor;
                FilePath = saveFileDialog1.FileName.ToString();
                if (FilePath.Substring(FilePath.Length - 4, 4) != ".csv")
                {
                    FilePath = FilePath + ".csv";
                }

                if (mGlobal.drawGraph35)
                {
                    str = createHeader35(ref str);
                    str += createData35(ref str);
                }
                else
                {
                    str = create_header(ref str);
                    str += create_data(ref str);
                }
                try
                {
                    System.IO.File.WriteAllText(FilePath, str);
                }
                catch
                {
                    MessageBox.Show(res_man.GetString("Save file fail", cul));
                    this.Cursor = Cursors.Default;
                    return;
                }
                this.Cursor = Cursors.Default;
                MessageBox.Show(res_man.GetString("File has been saved successfully", cul));
            }
        }

        private string createHeader35(ref string str_data)
        {
            int x = (128 * 1024) / mGlobal.numChan;
            int tg = mGlobal.duration35(Convert.ToInt32(device35.Duration), x);

            string _str = "";
            _str += "," + "," + res_man.GetString("Channel:", cul) + ",";

            int channel = 1;
            int dem = 0;
            for (int i = 0; i < device35.numOfChannel; i++)
            {
                if (device35.Channels[i].Unit == 3)
                {
                    switch (dem)
                    {
                        case 0:
                            _str += res_man.GetString("Channel", cul) + " " + channel + " X,";
                            break;

                        case 1:
                            _str += res_man.GetString("Channel", cul) + " " + channel + "Y,";
                            break;

                        case 2:
                            _str += res_man.GetString("Channel", cul) + " " + channel + "Z,";
                            channel += 1;
                            dem = -1;
                            break;
                    }
                    dem += 1;
                }
                else
                {
                    _str += res_man.GetString("Channel", cul) + " " + channel + ",";
                    channel += 1;
                }
            }

            _str += Environment.NewLine;

            //_str += "," + "," + "Channel:" + "," + "Channel 1" + "," + "Channel 2 " + "," + "Channel 3" + "," + "Channel 4 " + "," + "Channel 5" + "," + "Channel 6 " + "," + "Channel 7" + "," + "Channel 8 " + Environment.NewLine;

            _str += "," + "," + "  " + res_man.GetString("Unit:", cul) + ",";

            for (int i = 0; i < device35.numOfChannel; i++)
            {
                //_str += mGlobal.IntToUnit35(byte.Parse(device35.Channels[i].Unit.ToString())) + ",";
                _str += mGlobal.IntToUnit35(byte.Parse(mGlobal.unitTemp[i].ToString())) + ","; //   26/05_10.37
            }

            //_str += "," + "," + "  Unit:" + "," + mGlobal.IntToUnit(byte.Parse(openDevice.channels[0].Unit.ToString())) + "," + mGlobal.IntToUnit(byte.Parse(openDevice.channels[1].Unit.ToString())) + "," + mGlobal.IntToUnit(byte.Parse(openDevice.channels[2].Unit.ToString())) + "," + mGlobal.IntToUnit(byte.Parse(openDevice.channels[3].Unit.ToString())) + "," + mGlobal.IntToUnit(byte.Parse(openDevice.channels[4].Unit.ToString())) + "," + mGlobal.IntToUnit(byte.Parse(openDevice.channels[5].Unit.ToString())) + "," + mGlobal.IntToUnit(byte.Parse(openDevice.channels[6].Unit.ToString())) + "," + mGlobal.IntToUnit(byte.Parse(openDevice.channels[7].Unit.ToString())) + Environment.NewLine;

            _str += Environment.NewLine;

            _str += "," + "," + "  " + res_man.GetString("Highest:", cul);
            for (int i = 0; i < device35.numOfChannel; i++)
            {
                if (device35.Channels[i].Unit != 0)
                {
                    _str += "," + device35.Channels[i].high_suminfo;
                }
                else
                {
                    _str += "," + "---";
                }
            }

            _str += Environment.NewLine + "," + "," + "  " + res_man.GetString("Lowest:", cul);
            for (int i = 0; i < device35.numOfChannel; i++)
            {
                if (device35.Channels[i].Unit != 0)
                {
                    _str += "," + device35.Channels[i].low_suminfo;
                }
                else
                {
                    _str += "," + "---";
                }
            }

            _str += Environment.NewLine + "," + "," + "  "+ res_man.GetString("Average", cul);
            for (int i = 0; i < device35.numOfChannel; i++)
            {
                if (device35.Channels[i].Unit != 0)
                {
                    _str += "," + device35.Channels[i].ave_frm_suminfo;
                }
                else
                {
                    _str += "," + "---";
                }
            }

            _str += Environment.NewLine + "," + "," + "  " + "Standard deviation";
            for (int i = 0; i < device35.numOfChannel; i++)
            {
                if (device35.Channels[i].Unit != 0)
                {
                    _str += "," + Math.Round(mGlobal.StandartDeviation(device35.Channels[i].Data, 1), 2).ToString();
                }
                else
                {
                    _str += "," + "---";
                }
            }

            _str += Environment.NewLine + "," + "," + "  " + "Mean kinetic Temp.";
            for (int i = 0; i < device35.numOfChannel; i++)
            {
                if (device35.Channels[i].Unit == 172 || device35.Channels[i].Unit == 175)
                {
                    _str += "," + Math.Round(mGlobal.MKT(device35.Channels[i].Data, 1) - 273.16, 2).ToString();
                }
                else
                {
                    _str += "," + "---";
                }
            }

            _str += Environment.NewLine + "," + "," + "  " + "Median Value";
            for (int i = 0; i < device35.numOfChannel; i++)
            {
                if (device35.Channels[i].Unit != 0)
                {
                    if (device35.Channels[i].Data.Length % 2 != 0)
                    {
                        _str += "," + (device35.Channels[i].Data[device35.Channels[i].Data.Length / 2]).ToString();
                    }
                    else
                    {
                        _str += "," + ((device35.Channels[i].Data[device35.Channels[i].Data.Length / 2] + device35.Channels[i].Data[device35.Channels[i].Data.Length / 2 + 1]) / 2).ToString();
                    }
                }
                else
                {
                    _str += "," + "---";
                }
            }

            _str += Environment.NewLine + "," + "," + "  " + res_man.GetString("Max Alarm", cul);
            for (int i = 0; i < device35.numOfChannel; i++)
            {
                if (device35.Channels[i].Unit != 0)
                {
                    if (!device35.Channels[i].NoAlarm)
                    {
                        double tmp = (double)device35.Channels[i].AlarmMax / 10;
                        _str += "," + Math.Round(tmp, 1);
                    }
                    else
                    {
                        _str += "," + res_man.GetString("No Alarm", cul);
                    }
                }
                else
                {
                    _str += "," + "---";
                }

            }

            _str += Environment.NewLine + "," + "," + "  " + res_man.GetString("Min Alarm", cul);
            for (int i = 0; i < device35.numOfChannel; i++)
            {
                if (device35.Channels[i].Unit != 0)
                {
                    if (!device35.Channels[i].NoAlarm)
                    {
                        double tmp = (double)device35.Channels[i].AlarmMin / 10;
                        _str += "," + Math.Round(tmp, 1);
                    }
                    else
                    {
                        _str += "," + res_man.GetString("No Alarm", cul);
                    }
                }
                else
                {
                    _str += "," + "---";
                }
            }

            _str += Environment.NewLine;

            //===========Alarm
            int ch = 1;
            int d = 0;

            for (int i = 0; i < device35.numOfChannel; i++)
            {
                _str += res_man.GetString("Alarm Channel", cul) + " ";
                if (device35.Channels[i].Unit == 3)
                {
                    switch (d)
                    {
                        case 0:
                            _str += ch + " X,";
                            break;

                        case 1:
                            _str += ch + "Y,";
                            break;

                        case 2:
                            _str += ch + "Z,";
                            ch += 1;
                            d = -1;
                            break;
                    }
                    d += 1;
                }
                else
                {
                    _str += ch + ",";
                    ch += 1;
                }
                _str += Environment.NewLine;
               // _str += "Alarm Channel " + i + 1 + ": " + Environment.NewLine;
                _str += "  " + res_man.GetString("Number of measurements above high alarm limit:", cul) + "," + device35.Channels[i].MaxCount + Environment.NewLine;
                _str += "  " + res_man.GetString("Number of measurements below low alarm limit:", cul) + "," + device35.Channels[i].MinCount + Environment.NewLine;
                _str += "  " + res_man.GetString("Time over high alarm limit:", cul) + "," + mGlobal.Sec2Day(device35.Channels[i].MaxCount * Convert.ToInt32(tg)) + Environment.NewLine;
                _str += "  " + res_man.GetString("Time over low alarm limit:", cul) + "," + mGlobal.Sec2Day(device35.Channels[i].MinCount * Convert.ToInt32(tg)) + Environment.NewLine;
            }

            //===================== Setting Information
            _str += Environment.NewLine;
            _str += res_man.GetString("Setting Information:", cul) + Environment.NewLine;
            int temp = tg;
            _str += "  " + res_man.GetString("Duration:", cul) + "," + device35.Duration + " " + res_man.GetString("Day", cul) + Environment.NewLine;
            _str += "  " + res_man.GetString("Delay Start:", cul) + "," + device35.Delay + " " + res_man.GetString("Min", cul) + Environment.NewLine;


            //===================== Logger Information
            _str += Environment.NewLine;
            _str += res_man.GetString("Logger Information:", cul) + Environment.NewLine;
            _str += "  " + res_man.GetString("Serial Number:", cul) + "," + device35.Serial + Environment.NewLine;
            _str += "  " + res_man.GetString("Description:", cul) + "," + device35.Description + Environment.NewLine;
            _str += "  " + res_man.GetString("Location:", cul) + "," + device35.Location + Environment.NewLine;

            //===================== Time Information
            _str += Environment.NewLine;
            _str += res_man.GetString("Time Information:", cul) + Environment.NewLine;
            _str += "  " + res_man.GetString("Start Time:", cul) + "," + grapFromFile.StartTime + " " + grapFromFile._timezone.ToString().Replace(",", "-") + Environment.NewLine;
            _str += "  " + res_man.GetString("Stop Time:", cul) + "," + grapFromFile.StopTime + " " + grapFromFile._timezone.ToString().Replace(",", "-") + Environment.NewLine;
            _str += "  " + res_man.GetString("Elapse:", cul) + "," + grapFromFile._eclapsetime + Environment.NewLine;

            //===================== Measurement 
            _str += Environment.NewLine;
            _str += res_man.GetString("Measurements:", cul) + Environment.NewLine;
            _str += "  " + res_man.GetString("Sample Interval:", cul) + "," + tg + " " + res_man.GetString("sec", cul) + Environment.NewLine;
            _str += "  " + res_man.GetString("Number of Measurements:", cul) + "," + mGlobal.num_measure_suminfo + Environment.NewLine; //-----------'

            //===================== Edit comment
            _str += Environment.NewLine;
            _str += res_man.GetString("Comment:", cul) + Environment.NewLine;
            _str += device35.comment + Environment.NewLine;


            str_data = _str + Environment.NewLine;

            return str_data;

        }

        public string createData35(ref string str_data)
        {
            int x = (128 * 1024) / mGlobal.numChan;
            int tg = mGlobal.duration35(Convert.ToInt32(device35.Duration), x);

            string _str = "";
            DateTime[] time_print = new DateTime[device35.Channels[0].Data.Count()];
            _str += Environment.NewLine + Environment.NewLine;

            _str += "," + "          " + res_man.GetString("TIME", cul) + "          " + "," + "," + "             " + res_man.GetString("VALUE", cul) + "             "  + "," +   Environment.NewLine;
            //_str += "," + "," + "CH1" + "," + "CH2" + "," + "CH3" + "," + "CH4" + "," + "CH5" + "," + "CH6" + "," + "CH7" + "," + "CH8" + Environment.NewLine;
            _str += "," + "," + res_man.GetString("Channel:", cul) + ",";

            int channel = 1;
            int dem = 0;
            for (int i = 0; i < device35.numOfChannel; i++)
            {
                if (device35.Channels[i].Unit == 3)
                {
                    switch (dem)
                    {
                        case 0:
                            _str += "CH" + channel + " X,";
                            break;

                        case 1:
                            _str +=  "CH" + channel + " Y,";
                            break;

                        case 2:
                            _str += "CH" + channel + " Z,";
                            channel += 1;
                            dem = -1;
                            break;
                    }
                    dem += 1;
                }
                else
                {
                    _str += "CH" + channel + ",";
                    channel += 1;
                }
            }

            _str += Environment.NewLine;

            for (int i = 0; i < device35.Channels[0].Data.Count(); i++)
            {
                _str += "," + device35._logger_date.AddMinutes(Convert.ToInt32(device35.Delay)).AddSeconds(tg * i).ToString("MM/dd/yyyy HH:mm:ss ") + ",";

                for (int j = 0; j < device35.numOfChannel; j++)
                {
                    if (device35.Channels[j].Unit != 0)
                    {
                        _str += "," + mGlobal.format_num(Convert.ToSingle(device35.Channels[j].Data[i]));
                    }
                    else
                    {
                        _str += "," + "---";
                    }
                }

                _str += Environment.NewLine;
            }
            str_data = _str;

            return str_data;
        }

        private string create_header(ref string str_data)
        {
            string _str = "";

            _str += "," + "," + res_man.GetString("Channel:", cul) + "," + res_man.GetString("Channel", cul) + " 1" + "," + res_man.GetString("Channel", cul) + " 2 " + "," + res_man.GetString("Channel", cul) + " 3" + "," + res_man.GetString("Channel", cul) + " 4 " + "," + res_man.GetString("Channel", cul) + " 5" + "," + res_man.GetString("Channel", cul) + " 6 " + "," + res_man.GetString("Channel", cul) + " 7" + "," + res_man.GetString("Channel", cul) + " 8 " + Environment.NewLine;

            _str += "," + "," + "  " + res_man.GetString("Unit:", cul) + "," + mGlobal.IntToUnit(byte.Parse(mGlobal.unitTemp[0].ToString())) + "," + mGlobal.IntToUnit(byte.Parse(mGlobal.unitTemp[1].ToString())) + "," + mGlobal.IntToUnit(byte.Parse(mGlobal.unitTemp[2].ToString())) + "," + mGlobal.IntToUnit(byte.Parse(mGlobal.unitTemp[3].ToString())) + "," + mGlobal.IntToUnit(byte.Parse(mGlobal.unitTemp[4].ToString())) + "," + mGlobal.IntToUnit(byte.Parse(mGlobal.unitTemp[5].ToString())) + "," + mGlobal.IntToUnit(byte.Parse(mGlobal.unitTemp[6].ToString())) + "," + mGlobal.IntToUnit(byte.Parse(mGlobal.unitTemp[7].ToString())) + Environment.NewLine;

            _str += "," + "," + "  " + res_man.GetString("Highest:", cul);
            for (int i = 0; i < openDevice.numOfChannel; i++)
            {
                if (openDevice.channels[i].Unit != 0)
                {
                    //_str += "," + grapFromFile.high_suminfo[i];
                    _str += "," + openDevice.channels[i].high_suminfo;
                }
                else
                {
                    _str += "," + "---";
                }
            }

            _str += Environment.NewLine + "," + "," + "  " + res_man.GetString("Lowest:", cul);
            for (int i = 0; i < openDevice.numOfChannel; i++)
            {
                if (openDevice.channels[i].Unit != 0)
                {
                   // _str += "," + grapFromFile.low_suminfo[i];
                     _str += "," + openDevice.channels[i].low_suminfo;
                    
                }
                else
                {
                    _str += "," + "---";
                }
            }

            _str += Environment.NewLine + "," + "," + "  " + res_man.GetString("Average", cul);
            for (int i = 0; i < openDevice.numOfChannel; i++)
            {
                if (openDevice.channels[i].Unit != 0)
                {
                   // _str += "," +  grapFromFile.ave_frm_suminfo[i];
                    _str += "," + openDevice.channels[i].ave_frm_suminfo;
                }
                else
                {
                    _str += "," + "---";
                }
            }

            _str += Environment.NewLine + "," + "," + "  "+ res_man.GetString("Max Alarm", cul);
            for (int i = 0; i < openDevice.numOfChannel; i++)
            {
                if (openDevice.channels[i].Unit != 0)
                {
                    if (openDevice.channels[i].AlarmMax == 30000 || openDevice.channels[i].AlarmMax == 54032 || openDevice.channels[i].AlarmMin == 29999 || openDevice.channels[i].AlarmMin == 16649)
                    {
                        _str += "," + res_man.GetString("No Alarm", cul);
                    }
                    else
                    {
                        _str += "," + openDevice.channels[i].AlarmMax;
                    }
                }
                else
                {
                    _str += "," + "---";
                }
              
            }

            _str += Environment.NewLine + "," + "," + "  " + res_man.GetString("Min Alarm", cul);
            for (int i = 0; i < openDevice.numOfChannel; i++)
            {
                if (openDevice.channels[i].Unit != 0)
                {
                    if (openDevice.channels[i].AlarmMin == -30000 || openDevice.channels[i].AlarmMin == -53968 || openDevice.channels[i].AlarmMin == -29999 || openDevice.channels[i].AlarmMin == -16684)
                    {
                        _str += "," + res_man.GetString("No Alarm", cul);
                    }
                    else
                    {
                        _str += "," + openDevice.channels[i].AlarmMin;
                    }
                }
                else
                {
                    _str += "," + "---";
                }
            }

            _str += Environment.NewLine;

            //===========Alarm

            for (int i = 0; i < openDevice.numOfChannel; i++)
            {
                _str += Environment.NewLine;
                _str += res_man.GetString("Alarm Channel", cul) + " " + (i + 1).ToString() + ": " + Environment.NewLine;
                _str += "  " + res_man.GetString("Number of measurements above high alarm limit:", cul) + "," + openDevice.channels[i].MaxCount + Environment.NewLine;
                _str += "  " + res_man.GetString("Number of measurements below low alarm limit:", cul) + "," + openDevice.channels[i].MinCount + Environment.NewLine;
                _str += "  " + res_man.GetString("Time over high alarm limit:", cul) + "," + mGlobal.Sec2Day(openDevice.channels[i].MaxCount * Convert.ToInt32(openDevice.Duration)) + Environment.NewLine;
                _str += "  " + res_man.GetString("Time over low alarm limit:", cul) + "," + mGlobal.Sec2Day(openDevice.channels[i].MinCount * Convert.ToInt32(openDevice.Duration)) + Environment.NewLine;
            }

            //===================== Setting Information
            _str += Environment.NewLine;
            _str += res_man.GetString("Setting Information:", cul) + Environment.NewLine;
            int temp = openDevice.Duration;
            _str += "  " + res_man.GetString("Duration:", cul) + "," + mGlobal.interval2duration(temp) + " " + res_man.GetString("Day", cul) + Environment.NewLine;
            _str += "  " + res_man.GetString("Delay Start:", cul) + "," + openDevice.Delay + " " + res_man.GetString("Min", cul) + Environment.NewLine;


            //===================== Logger Information
            _str += Environment.NewLine;
            _str += res_man.GetString("Logger Information:", cul) + Environment.NewLine;
            _str += "  " + res_man.GetString("Serial Number:", cul) + "," + openDevice.Serial + Environment.NewLine;
            _str += "  " + res_man.GetString("Description:", cul) + "," + openDevice.Description + Environment.NewLine;
            _str += "  " + res_man.GetString("Location:", cul) + "," + openDevice.Location + Environment.NewLine;

            //===================== Time Information
            _str += Environment.NewLine;
            _str += res_man.GetString("Time Information:", cul) + Environment.NewLine;
            _str += "  " + res_man.GetString("Start Time:", cul) + "," + grapFromFile.StartTime + " " + grapFromFile._timezone.ToString().Replace(",", "-") + Environment.NewLine;
            _str += "  " + res_man.GetString("Stop Time:", cul) + "," + grapFromFile.StopTime + " " + grapFromFile._timezone.ToString().Replace(",", "-") + Environment.NewLine;
            _str += "  " + res_man.GetString("Elapse:", cul) + "," + grapFromFile._eclapsetime + Environment.NewLine;

            //===================== Measurement 
            _str += Environment.NewLine;
            _str += res_man.GetString("Measurements:", cul) + Environment.NewLine;
            _str += "  " + res_man.GetString("Sample Interval:", cul) + "," + openDevice.Interval  + Environment.NewLine;
            _str += "  " + res_man.GetString("Num of Measurements:", cul) + "," + mGlobal.num_measure_suminfo + Environment.NewLine; //-----------'

            //===================== Edit comment
            _str += Environment.NewLine;
            _str += res_man.GetString("Comment:", cul) + Environment.NewLine;
            _str += openDevice.comment + Environment.NewLine;


            str_data = _str + Environment.NewLine;

            return str_data;
        }

        public string create_data(ref string str_data)
        {
            string _str = "";
            DateTime[] time_print = new DateTime[openDevice.channels[7].Data.Count()];
            _str += Environment.NewLine + Environment.NewLine;

            _str += "," + "          " + res_man.GetString("TIME", cul) + "          " + "," + "             " + res_man.GetString("VALUE", cul) + "             " + Environment.NewLine;
            _str += "," + "," + "CH1" + "," + "CH2" + "," + "CH3" + "," + "CH4" + "," + "CH5" + "," + "CH6" + "," + "CH7" + "," + "CH8" + Environment.NewLine;

            for (int i = 0; i < openDevice.channels[0].Data.Count(); i++)
            {
                _str += "," + openDevice._logger_date.AddMinutes(Convert.ToInt32(openDevice.Delay)).AddSeconds(openDevice.Duration * i).ToString("MM/dd/yyyy HH:mm:ss ");

                for (int j = 0; j < openDevice.numOfChannel; j++)
                {
                    if (openDevice.channels[j].Unit != 0)
                    {
                        _str += "," + mGlobal.format_num(Convert.ToSingle(openDevice.channels[j].Data[i]));
                    }
                    else
                    {
                        _str += "," + "---";
                    }
                }

                _str += Environment.NewLine;
            }
            str_data = _str;

            return str_data;
        }

        private void tsbtnInformation_Click(object sender, EventArgs e)
        {
            if (mGlobal.drawGraph35)
            {
                ViewInfo viewSumInfo = new ViewInfo("viewSumInfo35");
                viewSumInfo.ShowDialog();
            }
            else
            {
                ViewInfo viewSumInfo = new ViewInfo("viewSum"); //2: SumInfo
                viewSumInfo.ShowDialog();
            }
        }

        private void tsbtnViewData_Click(object sender, EventArgs e)
        {
            if(mGlobal.drawGraph35)
            {
                ViewInfo viewInfo = new ViewInfo("data35"); // case 4: viewData35
                //viewInfo.MdiParent = this;
                //viewInfo.WindowState = FormWindowState.Maximized;
                viewInfo.Show();
            }
            else
            {
                ViewInfo viewInfo = new ViewInfo("viewData"); // case 3: viewData
                viewInfo.Show();
            }
        }

        private void tsbtnShowValue_Click(object sender, EventArgs e)
        {
            if(!mGlobal.tlb_value)
            {
                tsbtnShowValue.Image = Properties.Resources.focus;
                mGlobal.tlb_value = true;
                //if(mGlobal.drawGraph35)
                //{
                    for (int i = 0; i < grapFromFile.chart1.Series.Count; i++)
                    {
                        if (grapFromFile.chart1.Series[i].Name.Substring(10, 1) != "(")
                        {
                            grapFromFile.chart1.Series[i].LabelToolTip = grapFromFile.chart1.Series[i].Name + "\r\n #VALX \r\n [#VALY{0.000}]";
                            grapFromFile.chart1.Series[i].ToolTip = grapFromFile.chart1.Series[i].Name + "\r\n #VALX \r\n [#VALY{0.000}]";
                        }
                        else
                        {
                            grapFromFile.chart1.Series[i].LabelToolTip = grapFromFile.chart1.Series[i].Name + "\r\n #VALX \r\n [#VALY{0.0}]";
                            grapFromFile.chart1.Series[i].ToolTip = grapFromFile.chart1.Series[i].Name + "\r\n #VALX \r\n [#VALY{0.0}]";
                        }
                    }     
                //}
            }
            else
            {
                tsbtnShowValue.Image = Properties.Resources.focus_dis;
                mGlobal.tlb_value = false;
                for (int i = 0; i < grapFromFile.chart1.Series.Count; i++)
                {
                    grapFromFile.chart1.Series[i].ToolTip = "";
                }
            }
            //if (!mGlobal.tlb_value)
            //{
            //    tsbtnShowValue.Image = Properties.Resources.focus;
            //    mGlobal.tlb_value = true;
            //    if (mGlobal.drawGraph35)
            //    {
            //        grapFromFile.draw_graph35();
            //    }
            //    else
            //    {
            //        grapFromFile.draw_graph();
            //    }
            //}
            //else
            //{
            //    tsbtnShowValue.Image = Properties.Resources.focus_dis;
            //    mGlobal.tlb_value = false;
            //    if (mGlobal.drawGraph35)
            //    {
            //        grapFromFile.draw_graph35();
            //    }
            //    else
            //    {
            //        grapFromFile.draw_graph();
            //    }
            //}
        }

        private void tsbtnClipboard_Click(object sender, EventArgs e)
        {
            //grapFromFile.chart1.SaveImage("C:\\Users\\WIN7\\Documents\\Visual Studio 2013\\Projects\\Pexo16\\Pexo16\\clipboard\\mychart" + mGlobal.numOfClipboard + ".png", ChartImageFormat.Png);
            //mGlobal.numOfClipboard += 1;

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Images|*.png;*.bmp;*.jpg";
            if(mGlobal.drawGraph35)
            {
                device35 = Device35.Instance;
                sfd.FileName = DateTime.Now.ToString("ddMMyy") + device35.Serial + mGlobal.numOfClipboard;
            }
            else
            {
                openDevice = Device.Instance;
                sfd.FileName = DateTime.Now.ToString("ddMMyy") + openDevice.Serial + mGlobal.numOfClipboard;
            }
            ImageFormat format = ImageFormat.Png;
            if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string ext = System.IO.Path.GetExtension(sfd.FileName);
                switch (ext)
                {
                    case ".jpg":
                        format = ImageFormat.Jpeg;
                        break;
                    case ".bmp":
                        format = ImageFormat.Bmp;
                        break;
                }
                grapFromFile.chart1.SaveImage(sfd.FileName, format);
                MessageBox.Show("An image has been saved successfully");
            }
        }

        private void tsbtnEditComment_Click(object sender, EventArgs e)
        {
            EditComment comment = new EditComment();
            comment.ShowDialog();
        }

        private void tsbtnSave_Click(object sender, EventArgs e)
        {
            if (mGlobal.drawGraph35)
            {
                device35 = Device35.Instance;
                if (mGlobal.open_file == true)
                {
                    byte[] save = { 0 };

                    device35.Timezone = tscb_TiZo.Text;
                    device35.SaveFile_MP_Lgr2(ref save);

                    System.IO.File.WriteAllBytes(mGlobal.PathFile, save);

                    mGlobal.ColorChanged = false;
                    mGlobal.TitleChanged = false;
                    mGlobal.CommentChanged = false;
                    MessageBox.Show("This file has been saved");
                }
            }
            else
            {
                openDevice = Device.Instance;
                if (mGlobal.open_file == true)
                {
                    byte[] save = { 0 };

                    openDevice.Timezone = tscb_TiZo.Text;
                    openDevice.SaveFile_MP_Lgr2(ref save);

                    System.IO.File.WriteAllBytes(mGlobal.PathFile, save);

                    mGlobal.ColorChanged = false;
                    mGlobal.TitleChanged = false;
                    mGlobal.CommentChanged = false;
                    MessageBox.Show("This file has been saved");
                }
            }
        }

        private void tsbtnSaveAs_Click(object sender, EventArgs e)
        {
            if (mGlobal.drawGraph35)
            {
                device35 = Device35.Instance;
                byte[] data_save = { 0 };

                SaveFileDialog SaveFileDialog1 = new SaveFileDialog();
                string FilePath = null;
                string str1 = device35.Description;
                string str2 = device35.Serial;
                string str3 = device35.Location;

                OpenFileDialog OpenFileDialog1 = new OpenFileDialog();
                if (mGlobal.defaultFolder == "")
                {
                    try
                    {
                        SaveFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    }
                    catch (Exception)
                    {
                        return;
                    }
                }
                else
                {
                    SaveFileDialog1.InitialDirectory = mGlobal.defaultFolder;
                }

                SaveFileDialog1.Filter = "Marathon Data Logger File (*.D4S)|*.D4S";

                SaveFileDialog1.FilterIndex = 1;
                SaveFileDialog1.RestoreDirectory = true;

                string check_name = null;

                SaveFileDialog1.FileName = DateTime.Now.ToString("ddMMyy") + "_" + str3 + "_" + str2 + "_" + str1 + ".D4S";
                check_name = DateTime.Now.ToString("ddMMyy") + "_" + str3 + "_" + str2 + "_";

                if (SaveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string filename_koduoi = null;
                    string filename_duoi = null;
                    string filename_checknow = null;
                    FilePath = SaveFileDialog1.FileName.ToString();

                    filename_duoi = FilePath.Substring(FilePath.LastIndexOf("\\") + 1);
                    filename_koduoi = filename_duoi.Substring(0, filename_duoi.LastIndexOf("."));
                    filename_checknow = filename_koduoi.Substring(0, check_name.Length);

                    if (FilePath.Substring(FilePath.Length - 4) != ".D4S")
                    {
                        FilePath = FilePath + ".D4S";
                    }

                    device35.Timezone = tscb_TiZo.Text;
                    device35.SaveFile_MP_Lgr2(ref data_save);

                    try
                    {
                        System.IO.File.WriteAllBytes(FilePath, data_save);
                        MessageBox.Show("Save Successfully");
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Save file fail");
                    }


                    mGlobal.ColorChanged = false;
                    mGlobal.TitleChanged = false;
                    mGlobal.CommentChanged = false;
                }
            }
            else
            {
                openDevice = Device.Instance;
                byte[] data_save = { 0 };

                SaveFileDialog SaveFileDialog1 = new SaveFileDialog();
                string FilePath = null;
                //Dim check_save As Boolean
                //lay Description1
                string str1 = openDevice.Description;
                string str2 = openDevice.Serial;
                string str3 = openDevice.Location;


                //Try
                //    SaveFileDialog1.InitialDirectory = My.Computer.FileSystem.SpecialDirectories.Desktop
                //Catch ex As Exception
                //End Try

                OpenFileDialog OpenFileDialog1 = new OpenFileDialog();
                if (mGlobal.defaultFolder == "")
                {
                    try
                    {
                        SaveFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    }
                    catch (Exception)
                    {
                        return;
                    }
                }
                else
                {
                    SaveFileDialog1.InitialDirectory = mGlobal.defaultFolder;
                }

                SaveFileDialog1.Filter = "Marathon Data Logger File (*.D8S)|*.D8S|Marathon Data Logger File (*.D4S)|*.D4S";

                SaveFileDialog1.FilterIndex = 1;
                SaveFileDialog1.RestoreDirectory = true;

                string check_name = null;


                SaveFileDialog1.FileName = DateTime.Now.ToString("ddMMyy") + "_" + str3 + "_" + str2 + "_" + str1 + ".D" + openDevice.numOfChannel + "S";
                check_name = DateTime.Now.ToString("ddMMyy") + "_" + str3 + "_" + str2 + "_";



                if (SaveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string filename_koduoi = null;
                    string filename_duoi = null;
                    string filename_checknow = null;
                    FilePath = SaveFileDialog1.FileName.ToString();

                    filename_duoi = FilePath.Substring(FilePath.LastIndexOf("\\") + 1);
                    filename_koduoi = filename_duoi.Substring(0, filename_duoi.LastIndexOf("."));
                    filename_checknow = filename_koduoi.Substring(0, check_name.Length);

                    if (FilePath.Substring(FilePath.Length - 4) != ".D" + openDevice.numOfChannel + "S")
                    {
                        FilePath = FilePath + ".D" + openDevice.numOfChannel + "S";
                    }

                    openDevice.Timezone = tscb_TiZo.Text;
                    openDevice.SaveFile_MP_Lgr2(ref data_save);

                    try
                    {
                        System.IO.File.WriteAllBytes(FilePath, data_save);
                        MessageBox.Show("Save Successfully");
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Save file fail");
                    }

                    mGlobal.ColorChanged = false;
                    mGlobal.TitleChanged = false;
                    mGlobal.CommentChanged = false;
                }
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tsbtnSave_Click(sender, e);
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tsbtnSaveAs_Click(sender, e);
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form frm in this.MdiChildren)
                frm.Close();

            //foreach (Form f in Application.OpenForms)
            //{
            //    if (f is Graph)
            //    {
            //        //Graph is activated. Close it
            //        f.Close();
            //    }
            //}
        }

        private void exportDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tsbtnExportData_Click(sender, e);
        }

        private void viewSummaryInfomationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tsbtnInformation_Click(sender, e);
        }

        private void fullScreenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fullScreenToolStripMenuItem.Checked = true;
            cascadToolStripMenuItem.Checked = false;

            grapFromFile.WindowState = FormWindowState.Maximized;
        }

        private void cascadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cascadToolStripMenuItem.Checked = true;
            fullScreenToolStripMenuItem.Checked = false;

            grapFromFile.WindowState = FormWindowState.Normal;
        }

        private void copyToClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tsbtnClipboard_Click(sender, e);
        }

        private void titleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tsbtnEditText_Click(sender, e);
        }

        private void colorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tsbtnChangeColor_Click(sender, e);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About frmAbout = new About();
            frmAbout.ShowDialog();
        }

        private void viewDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (mGlobal.drawGraph35)
            {
                ViewInfo viewInfo = new ViewInfo("data35"); // case 4: viewData35
                viewInfo.Show();
            }
            else
            {
                ViewInfo viewInfo = new ViewInfo("viewData"); // case 3: viewData
                viewInfo.Show();
            }
        }

        private void LoggerInitializeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //MultiSetting multiSet = new MultiSetting();
            //multiSet.ShowDialog();
        }

        private void websiteMarathonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.marathonproducts.com");
        }

        private void toolbarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(toolStrip.Visible == false)
            {
                toolStrip.Visible = true;
                toolbarToolStripMenuItem.Checked = true;
            }
            else
            {
                toolStrip.Visible = false;
                toolbarToolStripMenuItem.Checked = false;
            }
        }

        private void tscb_TiZo_SelectedIndexChanged(object sender, EventArgs e)
        {
            //mGlobal.cb_tz = true;
            //if (mGlobal.sttCF == false)
            //{
            //frmReadLogger.changeChart = true;
            //frmReadLogger.draw_graph(data_byte);
            //}        
            grapFromFile._timezone = mGlobal.FindSystemTimeZoneFromDisplayName(tscb_TiZo.Text);
            if (mGlobal.drawGraph35 == true)
            {
                grapFromFile.draw_graph35();
            }
            else
            {
                grapFromFile.draw_graph();
            }
        }

        private void pexo16ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MultiSetting multiSet = new MultiSetting("8");
            multiSet.ShowDialog();
        }

        private void pexo35ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MultiSetting multiSet = new MultiSetting("PEXO-35");
            multiSet.ShowDialog();
        }

        private void pexo34ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MultiSetting multiSet = new MultiSetting("PEXO-34");
            multiSet.ShowDialog();
        }

        private void runWithRealtimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            getDeviceInfo.getActiveDevice();
            DashBoardSelect realTimeSelect = new DashBoardSelect(1);//1: pexo 35
            realTimeSelect.ShowDialog();
        }

        private void preferencesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Reference refe = new Reference();
            refe.FormClosed += new FormClosedEventHandler(setClose);
            refe.ShowDialog();
        }

        private void setClose(object sender, FormClosedEventArgs e)
        {
            MainUI_Load(sender, e);
            this.Refresh();
        }

        private void graphAndSumaryInfomationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tsbtnPrintReview_Click(sender, e);
        }

        private void genaralInformationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tsbtnInformation_Click(sender, e);
        }

        private void dataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tsbtnViewData_Click(sender, e);
        }

        private void pexo16RealtimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mGlobal.len = 64;
            DashBoard dashboard = new DashBoard(this);
            dashboard.ShowDialog();
        }

        private void pexo35RealTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mGlobal.len = 64;

            DashBoard35 dashboard = new DashBoard35();
            dashboard.ShowDialog();


            //getDeviceInfo.getActiveDevice();
            //DashBoardSelect realTimeSelect = new DashBoardSelect(1);
            //realTimeSelect.ShowDialog();
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (logDevice != null)
            {
                grapFromFile = Graph.DelInstance();
                grapFromFile = Graph.Instance;
                mGlobal._get_timezone_date(ref grapFromFile._timezone, ref logDevice._logger_date, logDevice.data_open);
                grapFromFile.FormClosed += new FormClosedEventHandler(childformClose);
                grapFromFile.MdiParent = this;
                grapFromFile.WindowState = FormWindowState.Maximized;

                grapFromFile.draw_graph35();
                grapFromFile.Show();
                enableToolButton(true);
                tscb_TiZo.Text = grapFromFile._timezone.ToString();
                logDevice.Close();
            }
            else
            {
                return;
            }
        }

        private void graphToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void settingVaCaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Setting35 set = new Setting35();
            set.Show();
        }

        private void setFirmwareVerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetFirmware setF = new SetFirmware();
            setF.Show();
        }

        private void Menu4S_Click(object sender, EventArgs e)
        {

        }

    }
}
