using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Diagnostics;
using System.Configuration;
using System.IO;
using System.Security.AccessControl;
namespace iMgmt
{
    public partial class frmLogin : Form
    {
         //String cs = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source="+ConfigurationManager.AppSettings["DatabasePath"]+";";
        String cs = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source="+ConfigurationManager.AppSettings["DatabasePath"]+";";

        public frmLogin()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (txtUserName.Text == "")
            {
                MessageBox.Show("Please enter user name", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtUserName.Focus();
                return;
            }
            if (txtPassword.Text == "")
            {
                MessageBox.Show("Please enter password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPassword.Focus();
                return;
            }
            try
            {
                if (!Directory.Exists(ConfigurationManager.AppSettings["DatabaseFolder"]))
                {
                    DirectoryInfo di = Directory.CreateDirectory(ConfigurationManager.AppSettings["DatabaseFolder"]);
                    DirectorySecurity ds = di.GetAccessControl();
                    ds.AddAccessRule(new FileSystemAccessRule("Users",FileSystemRights.Delete,AccessControlType.Deny));
                    ds.AddAccessRule(new FileSystemAccessRule("Administrators", FileSystemRights.Delete, AccessControlType.Deny));
                    di.SetAccessControl(ds);
                    di.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
                    if (!File.Exists(ConfigurationManager.AppSettings["DatabasePath"]))
                    {
                        File.Copy("SIS_DB.accdb", ConfigurationManager.AppSettings["DatabasePath"]);
                        //OleDbConnection newConnection = default(OleDbConnection);
                        //newConnection = new OleDbConnection(cs);

                        //OleDbCommand newCommand = default(OleDbCommand);
                        //newCommand = new OleDbCommand("INSERT INTO [Registration] ([NameOfUser], [UserName], [User_Password], [ContactNo], [Email], [JoiningDate]) VALUES (\"bala\", \"admin\", \"12345\", NULL, NULL, NULL);", newConnection);
                        //newCommand.Connection.Open();
                        //newCommand.ExecuteNonQuery();
                        //newCommand.Connection.Close();
                        //newCommand = new OleDbCommand("INSERT INTO [Users] ([Username], [User_Password]) VALUES (\"admin\", \"12345\");", newConnection);
                        //newCommand.Connection.Open();
                        //newCommand.ExecuteNonQuery();
                        //newCommand.Connection.Close();
                    }
                }
                
                OleDbConnection myConnection = default(OleDbConnection);
                myConnection = new OleDbConnection(cs);

                OleDbCommand myCommand = default(OleDbCommand);

                myCommand = new OleDbCommand("SELECT Username,User_password FROM Users WHERE Username = @username AND User_password = @UserPassword", myConnection);
                OleDbParameter uName = new OleDbParameter("@username", OleDbType.VarChar);
                OleDbParameter uPassword = new OleDbParameter("@UserPassword", OleDbType.VarChar);
                uName.Value = txtUserName.Text;
                uPassword.Value = txtPassword.Text;
                myCommand.Parameters.Add(uName);
                myCommand.Parameters.Add(uPassword);

                myCommand.Connection.Open();

                OleDbDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection);

                if (myReader.Read() == true)
                {
                        int i;
                        ProgressBar1.Visible = true;
                        ProgressBar1.Maximum = 5000;
                        ProgressBar1.Minimum = 0;
                        ProgressBar1.Value = 4;
                        ProgressBar1.Step = 1;

                        for (i = 0; i <= 5000; i++)
                        {
                            ProgressBar1.PerformStep();
                        }
                        this.Hide();
                        frmMainMenu frm = new frmMainMenu();
                        frm.Show();
                        frm.lblUser.Text = txtUserName.Text;
                    

                    }
                

                else
                {
                    MessageBox.Show("Login is Failed...Try again !", "Login Denied", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    txtUserName.Clear();
                    txtPassword.Clear();
                    txtUserName.Focus();

                }
                if (myConnection.State == ConnectionState.Open)
                {
                    myConnection.Dispose();
                }

              

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
      
        private void Form1_Load(object sender, EventArgs e)
        {
            ProgressBar1.Visible = false;
            txtUserName.Focus();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
           // Application.Exit();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            frmChangePassword frm = new frmChangePassword();
            frm.Show();
            frm.txtUserName.Text = "";
            frm.txtNewPassword.Text = "";
            frm.txtOldPassword.Text = "";
            frm.txtConfirmPassword.Text = "";
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            frmRecoveryPassword frm = new frmRecoveryPassword();
            frm.txtEmail.Focus();
            frm.Show();
        }

        private void frmLogin_FormClosed(object sender, FormClosedEventArgs e)
        {
            Process.GetCurrentProcess().Kill();
        }
    }
}
