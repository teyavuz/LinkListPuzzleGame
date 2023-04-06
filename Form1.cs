using System;
using System.Windows.Forms;

namespace LinkListPuzzleGame
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            loginBtn.Enabled = false;
        }
        private void usernameTxt_TextChanged(object sender, EventArgs e)
        {

            if (usernameTxt.Text == "")
            {
                loginBtn.Enabled = false;
            }
            else
            {
                loginBtn.Enabled = true;
            }
        }

        private void loginBtn_Click(object sender, EventArgs e)
        {

            if (usernameTxt.Text != "")
            {
                MainForm mainform = new MainForm(usernameTxt.Text.ToUpper());
                this.Hide();
                mainform.Show();
            }
        }
    }
}
