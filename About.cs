using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Pexo16
{
    public partial class About : Form
    {
        public About()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void About_Load(object sender, EventArgs e)
        {
            DateTime Fdate = default(DateTime);
            Fdate = System.IO.File.GetLastWriteTime(System.Reflection.Assembly.GetExecutingAssembly().Location);
            lblBuild.Text = "Build: " + Fdate.ToString("F");
            lblApp.Text = "MaxiThermal Wifi" + DateTime.Now.Year.ToString();
        }
    }
}
