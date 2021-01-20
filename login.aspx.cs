using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace WebApplication76
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           
        }

        protected void sndButton_Click(object sender, EventArgs e)
        {
            AuthenticateUser(UserEmail.Text, Password.Text);
        }

        private void AuthenticateUser(string username, string password)
        {
            // ConfigurationManager class is in System.Configuration namespace
            string CS = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;

            // SqlConnection is in System.Data.SqlClient namespace
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("spAuthenticateUser", con);
                cmd.CommandType = CommandType.StoredProcedure;

                //Formsauthentication is in system.web.security
                string encryptedpassword = FormsAuthentication.HashPasswordForStoringInConfigFile(password, "SHA1");

                //sqlparameter is in System.Data namespace
                SqlParameter paramUsername = new SqlParameter("@email", username);
                SqlParameter paramPassword = new SqlParameter("@Password", encryptedpassword);

                cmd.Parameters.Add(paramUsername);
                cmd.Parameters.Add(paramPassword);

                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    int RetryAttempts = Convert.ToInt32(rdr["RetryAttempts"]);
                    if (Convert.ToBoolean(rdr["AccountLocked"]))
                    {
                        errorLogin.ForeColor = System.Drawing.Color.Red;
                        errorLogin.Text = "Account locked. Please contact administrator";
                    }
                    else if (RetryAttempts > 0)
                    {
                        int AttemptsLeft = (4 - RetryAttempts);
                        errorLogin.ForeColor = System.Drawing.Color.Yellow;
                        errorLogin.Text = "Invalid user name and/or password. " +
                            AttemptsLeft.ToString() + "attempt(s) left";
                    }
                    else if (Convert.ToBoolean(rdr["Authenticated"]))
                    {
                        FormsAuthentication.RedirectFromLoginPage(UserEmail.Text, sndCheckbox.Checked);
                    }
                }
            }
        }


        protected void signupButton_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                string cs = ConfigurationManager.ConnectionStrings["DBCS"].ToString();
                using (SqlConnection con = new SqlConnection(cs))
                {
                    SqlCommand cmd = new SqlCommand("spRegisterUser", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    string encriptdPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(signupPass.Text, "SHA1");

                    SqlParameter username = new SqlParameter("@username", signupuser.Text);
                    SqlParameter password = new SqlParameter("@password_", encriptdPassword);
                    SqlParameter email = new SqlParameter("@Email", signupEmail.Text);

                    cmd.Parameters.Add(username);
                    cmd.Parameters.Add(password);
                    cmd.Parameters.Add(email);
                    con.Open();
                    int ReturnCode = (int)cmd.ExecuteScalar();
                    if (ReturnCode == -1)
                    {
                    }
                    else
                    {
                        Response.Redirect("~/login.aspx");
                    }

                }
            }

        }
    }
}