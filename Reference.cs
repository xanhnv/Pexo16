using System;
using System.Globalization;
using System.IO;
using System.Resources;
using System.Windows.Forms;

namespace Pexo16
{
    public partial class Reference : Form
    {
        ResourceManager res_man;
        CultureInfo cul;

        public Reference()
        {
            InitializeComponent();
        }

        private void Reference_Load(object sender, EventArgs e)
        {
            CenterToScreen();

            //tab 1
            string FileName = "";
            FileName = mGlobal.app_patch(FileName);
            FileName += "\\Reference.bin";

            string str = "";
            if(File.Exists(FileName))
            {
                FileStream fs = new FileStream(FileName, FileMode.OpenOrCreate);
                BinaryReader br = new BinaryReader(fs);
                //str = File.ReadAllText(FileName);
                str = br.ReadString();
                txtPath.Text = str;
                fs.Close();
                br.Close();
            }
            else
            {
                txtPath.Text = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            }

            //tab 2
            if (mGlobal.language != null)
            {
                cbbLanguage.Text = mGlobal.language;
            }
            else
            {
                cbbLanguage.Text = cbbLanguage.Items[0].ToString();
            }

            switch (mGlobal.language)
            {
                case "Spanish":
                    res_man = new ResourceManager("Pexo16.Lang.Resources", typeof(MainUI).Assembly);
                    cul = CultureInfo.CreateSpecificCulture("es-ES");
                    break;
                case "Japanese":
                    res_man = new ResourceManager("Pexo16.Lang.Resources", typeof(MainUI).Assembly);
                    cul = CultureInfo.CreateSpecificCulture("ja-JP");
                    break;
                case "Korean":
                    res_man = new ResourceManager("Pexo16.Lang.Resources", typeof(MainUI).Assembly);
                    cul = CultureInfo.CreateSpecificCulture("ko-KR");
                    break;
                default:
                    res_man = new ResourceManager("Pexo16.Lang.Resources", typeof(MainUI).Assembly);
                    cul = CultureInfo.CreateSpecificCulture("en-US");
                    break;
            }

            tabPage1.Text = res_man.GetString("Location setting", cul);
            tabPage2.Text = res_man.GetString("Language setting", cul);
            btnChoose.Text = res_man.GetString("Browse", cul);
            btnSave.Text = res_man.GetString("Save", cul);
            btnClose.Text = res_man.GetString("Close", cul);
            btnSaveL.Text = res_man.GetString("Save", cul);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            mGlobal.defaultFolder = txtPath.Text;
            MessageBox.Show("Your setting has been saved!");

            string FileName = "";
            FileName = mGlobal.app_patch(FileName);
            FileName += "\\Reference.bin";

            FileStream fs = new FileStream(FileName, FileMode.Create);
            BinaryWriter bw = new BinaryWriter(fs);
            bw.Write(txtPath.Text);
            fs.Close();
            bw.Close();
        }

        private void btnChoose_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog browse = new FolderBrowserDialog();
            browse.RootFolder = Environment.SpecialFolder.Desktop;
            browse.Description = "Select a Default directory to Opening and Saving Data Logger";
            if (browse.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtPath.Text = browse.SelectedPath;
            }
        }

        private void btnSaveL_Click(object sender, EventArgs e)
        {
            switch(cbbLanguage.SelectedItem.ToString())
            {
                case "Spanish":
                    mGlobal.language = "Spanish";
                    break;
                case "Japanese":
                    mGlobal.language = "Japanese";
                    break;
                case "Korean":
                    mGlobal.language = "Korean";
                    break;
                default:
                    mGlobal.language = "English";
                    break;
            }
            this.Close();
        }

        private void btnCloseL_Click(object sender, EventArgs e)
        {
            //this.Dispose();
            this.Close();
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

    }
}
