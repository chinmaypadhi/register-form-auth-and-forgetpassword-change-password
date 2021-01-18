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
            if (AuthenticateUser(UserEmail.Text, Password.Text))
            {
                FormsAuthentication.RedirectFromLoginPage(UserEmail.Text, sndCheckbox.Checked);
            }
            else
            {
                errorLogin.ForeColor = System.Drawing.Color.Red;
                errorLogin.Text = "Invalid User Name and/or Password";
            }
        }

        private bool AuthenticateUser(string userEmail, string password)
        {
            string cs = ConfigurationManager.ConnectionStrings["DBCS"].ToString();
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("spAuthenticateUser", con);
                cmd.CommandType = CommandType.StoredProcedure;

                string encriptedPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(password, "SHA1");

                SqlParameter paramusername = new SqlParameter("@email", userEmail);
                SqlParameter parampassword = new SqlParameter("@password_", encriptedPassword);

                cmd.Parameters.Add(paramusername);
                cmd.Parameters.Add(parampassword);

                con.Open();
                int Returncode = (int)cmd.ExecuteScalar();
                return Returncode == 1;
               
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