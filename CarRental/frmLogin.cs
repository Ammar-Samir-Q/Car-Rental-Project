using CarRental_BLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Odbc;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
namespace CarRental
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        private bool ReadUserNameAndPassowrd(ref string userName,ref string password)
        {
            try
            {
                string Path = Directory.GetCurrentDirectory();
                Path += "\\LoginData.txt";
                if (!File.Exists(Path))
                {
                    return false;
                }

                using (StreamReader reader = new StreamReader(Path))
                {
                    string Line = "";
                    while ((Line = reader.ReadLine()) != null)
                    {

                        string[] Result = Line.Split(new string[] { "#//#" }, StringSplitOptions.None);
                        userName = Result[0];
                        password = Result[1];
                        return true;
                    }


                }
            }
            catch(Exception ex) 
            { }
            return false;
        }

        private void _LoadUserData()
        {
            txtUserName.Text = "";
            txtPassword.Text = "";
            cbRememberMe.Checked = false;
            _HandelNullLogin();
            string Username="", Password = "";
            if(clsGlobalSettings.ReadUserInfoFromReg(ref Username,ref Password))
            {
                txtUserName.Text = Username;
                txtPassword.Text=Password;
                cbRememberMe.Checked = true;
                btnLogin.Focus();
            }
            else
            {
                txtUserName.Focus();
            }
        }

        private void _HandelNullLogin()
        {
            if(string.IsNullOrEmpty(txtUserName.Text)&&string.IsNullOrEmpty(txtPassword.Text))
            {
                txtUserName.Text = "";
                txtPassword.Text = "";
                btnLogin.Enabled = false;
                cbRememberMe.Enabled = false;
            }
            else
            {
                btnLogin.Enabled = true;
                cbRememberMe.Enabled = true;
            }

        }

        private void _SaveUserData(string Username,string Password)
        {
            try
            {
                string Path = Directory.GetCurrentDirectory();
                Path += "\\LoginData.txt";

           
                using (StreamWriter writer = new StreamWriter(Path))
                {
                    string LineToSave = Username + "#//#" + Password;
                    writer.WriteLine(LineToSave);
                    return;
                }
            }
            catch (Exception ex)
            { Console.WriteLine(ex.ToString());}

        }

        private void _HandelSavingData()
        {
            if (cbRememberMe.Checked)
                if(!clsGlobalSettings.SaveUserInfoToReg(txtUserName.Text, txtPassword.Text))
                {
                    MessageBox.Show("Faild To Save Login Data","Reg Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
        }

        private void _Login()
        {

            if (!clsUsers.IsUserExists(txtUserName.Text,txtPassword.Text))
            {
                MessageBox.Show("Wrong Username Or Password", " User Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            _HandelSavingData();

            clsGlobalSettings.UserID = clsUsers.GetUserIDByUsernameAndPassword(txtUserName.Text,txtPassword.Text);
            clsGlobalSettings.Role=clsUsers.GetUserByID(clsGlobalSettings.UserID).Role;
            Form frm = new MainMenu(this);
            this.Hide();
            frm.ShowDialog();
            Application.Exit();
        }


        private void btnLogin_Click(object sender, EventArgs e)
        {
            _Login();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void _HandelBackColor()
        {
            panel1.BackColor = Properties.Settings.Default.MainColor;
            panel2.BackColor = Properties.Settings.Default.MainColor;
            btnClose.BackColor = panel1.BackColor;
            btnLogin.BackColor = panel2.BackColor;
            btnRestoreDataBase.BackColor = panel1.BackColor;
            btnEnterAsCustomer.BackColor = panel2.BackColor;

            if (panel1.BackColor == Color.White)
            {
                lblTitle.ForeColor = Color.Black;
                lblSubTitle.ForeColor = Color.Black;
                lblVersion.ForeColor = Color.Black;
                btnLogin.ForeColor = Color.Black;
                btnClose.ForeColor = Color.Black;
                btnRestoreDataBase.ForeColor = Color.Black;
                btnEnterAsCustomer.ForeColor = Color.Black;
            }

        }
        private void frmLogin_Load(object sender, EventArgs e)
        {
            _HandelBackColor();
            if(!DataHelper.TestConnection())
            {
                btnLogin.Visible = false;
                btnEnterAsCustomer.Visible = false;
                btnRestoreDataBase.Visible = true;
                txtPassword.Enabled = false;
                txtUserName.Enabled = false;
                return;
            }
            else
            {
                txtPassword.Enabled = true;
                txtUserName.Enabled = true;
                btnLogin.Visible = true;
                btnEnterAsCustomer.Visible = true;
                btnRestoreDataBase.Visible = false;
            }

            _LoadUserData();
        }

        private void txtUserName_TextChanged(object sender, EventArgs e)
        {
            _HandelNullLogin();
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            _HandelNullLogin();
        }

        private void btnRestoreDataBase_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd=new OpenFileDialog();
            ofd.Title = "Select DataBase Path";
            if(ofd.ShowDialog() == DialogResult.OK)
            {
                if(!File.Exists(ofd.FileName))
                {
                    MessageBox.Show("DataBase Not Found", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if(DataHelper.RestoreDataBase(ofd.FileName))
                {
                    MessageBox.Show("DataBase Restored Successfully System Will Restart", "Restored", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Application.Restart();
                }
                else
                {
                    MessageBox.Show("Faild To Restore DataBase", "Faild", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
            }
        }

        private void btnEnterAsCustomer_Click(object sender, EventArgs e)
        {
            
            clsGlobalSettings.Role = clsUsers.enRole.Customer;
            clsGlobalSettings.UserID = 3;
            Form frm = new MainMenu();
            this.Hide();
            frm.ShowDialog();
            Application.Exit();
            //Testing----->
            //OpenFileDialog ofd=new OpenFileDialog();
            //if (ofd.ShowDialog() == DialogResult.OK)
            //{
            //    DataHelper.RestoreDataBase(ofd.FileName);
            //}
        }
    }
}
