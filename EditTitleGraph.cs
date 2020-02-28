using System;
using System.Windows.Forms;
using System.Resources;
using System.Globalization;

namespace Pexo16
{
    public partial class EditTitleGraph : Form
    {
        Graph graph = Graph.Instance;

        ResourceManager res_man = new ResourceManager("Pexo16.Lang.Resources", typeof(EditTitleGraph).Assembly);
        CultureInfo cul;

        public EditTitleGraph()
        {
            InitializeComponent();    
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (txtTitle.Text.Length > 50)
            {
                MessageBox.Show("Maximun 50 characters!");
                return;
            }
            else
            {
                graph.chart1.Titles[0].Text = txtTitle.Text;
                mGlobal.TitleGraph = txtTitle.Text;
                txtTitle.Text = "";
                mGlobal.TitleChanged = true;
                this.Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmEditTitleGraph_Load(object sender, EventArgs e)
        {
            switch(mGlobal.language)
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

            label1.Text = res_man.GetString("Graph Title", cul);
            btnOk.Text = res_man.GetString("Ok", cul);
            btnCancel.Text = res_man.GetString("Cancel", cul);
            txtTitle.Text = graph.chart1.Titles[0].Text;
        }

        private void txtTitle_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8)
            {
                if (txtTitle.Text.Length >= 50)
                {
                    //MessageBox.Show("Description cannot be over 40 characters");
                    e.Handled = true;
                    ToolTip t = new ToolTip();
                    t.SetToolTip(txtTitle, res_man.GetString("Title cannot be over 50 characters", cul));
                }
                else
                {
                    e.Handled = false;
                }
            }
        }

    }
}
