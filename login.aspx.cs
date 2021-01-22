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
using MYBLL;

namespace WebApplication76
{
    public partial class WebForm1 : System.Web.UI.Page
    {

        static mybll ob = new mybll(globalConnection.str);
        protected void Page_Load(object sender, EventArgs e)
        {
           
        }
        protected void sndButton_Click(object sender, EventArgs e)
        {
            if (AuthenticateUser1(UserEmail.Text))
            {
                AuthenticateUser(UserEmail.Text, Password.Text);
            }
            else
            {
                errorLogin.ForeColor = System.Drawing.Color.Red;
                errorLogin.Text = "Invalid User Name and/or Password";
            }
        }
        private void AuthenticateUser(string username, string password)
        {
                //"spAuthenticateUser"

                string encryptedpassword = FormsAuthentication.HashPasswordForStoringInConfigFile(password, "SHA1");
                List<SqlParameter> paramList = new List<SqlParameter>()
            {
                new SqlParameter()
                {
                    ParameterName ="@email",
                    Value =username
                },
                new SqlParameter()
                {
                    ParameterName ="@Password",
                    Value =encryptedpassword
                },
            };
            
            SqlDataReader rdr = ob.ExecuteReaderWithParamiterAndReturntype("spAuthenticateUser", paramList);
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
        private bool AuthenticateUser1(string username)
        {
                    List<SqlParameter> paramList = new List<SqlParameter>()
            {
                new SqlParameter()
                {
                    ParameterName ="@email",
                    Value =username
                },
            };

                int ReturnCode = (int)ob.ExecuteScallerWithReturntype("spAuthenticateUser1",paramList);
                return ReturnCode == 1;
        }
        protected void signupButton_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                string cs = ConfigurationManager.ConnectionStrings["DBCS"].ToString();
                using (SqlConnection con = new SqlConnection(cs))
                {

                    //"spRegisterUser"

                    string encriptdPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(signupPass.Text, "SHA1");
                    List<SqlParameter> paramList = new List<SqlParameter>()
            {
                new SqlParameter()
                {
                    ParameterName ="@username",
                    Value =signupuser.Text
                },
                new SqlParameter()
                {
                    ParameterName ="@Password_",
                    Value =encriptdPassword
                },
                                new SqlParameter()
                {
                    ParameterName ="@Email",
                    Value =signupEmail.Text
                },
            };
                    int ReturnCode = (int)ob.ExecuteScallerWithReturntype("spRegisterUser", paramList);
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