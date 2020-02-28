using System;
using System.Windows.Forms;
using System.Globalization;
using System.Resources;

namespace Pexo16
{
    public partial class EditComment : Form
    {
        Device deviceOpen;
        Device35 dv35;

        ResourceManager res_man = new ResourceManager("Pexo16.Lang.Resources", typeof(EditComment).Assembly);
        CultureInfo cul;


        public EditComment()
        {
            if (mGlobal.drawGraph35)
            {
                dv35 = Device35.Instance;
            }
            else
            {
                deviceOpen = Device.Instance;
            }
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length > 200)
            {
                MessageBox.Show("Maximum 200 characters");
            }
            else
            {
                if (mGlobal.drawGraph35)
                {
                    dv35.comment = textBox1.Text;
                }
                else
                {
                    deviceOpen.comment = textBox1.Text;
                }
                mGlobal.CommentChanged = true;
                this.Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void EditComment_Load(object sender, EventArgs e)
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

            btnOK.Text = res_man.GetString("Ok", cul);
            btnCancel.Text = res_man.GetString("Cancel", cul);

            if (mGlobal.drawGraph35)
            {
                textBox1.Text = dv35.comment;
            }
            else
            {
                textBox1.Text = deviceOpen.comment;
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8)
            {
                if (textBox1.Text.Length >= 200)
                {
                    //MessageBox.Show("Description cannot be over 40 characters");
                    e.Handled = true;
                    ToolTip t = new ToolTip();
                    t.SetToolTip(textBox1, res_man.GetString("Comment cannot be over 200 characters", cul));
                }
                else
                {
                    e.Handled = false;
                }
            }
        }

    }
}
