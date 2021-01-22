using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Mail;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using MYBLL;

namespace WebApplication76.registration
{
    public partial class forgetpass : System.Web.UI.Page
    {
        static mybll ob = new mybll(globalConnection.str);


        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void Button1_Click(object sender, EventArgs e)
        {
                        List<SqlParameter> paramList = new List<SqlParameter>()
                {
                    new SqlParameter()
                    {
                        ParameterName = "@email",
                        Value =forgetpassEmail.Text
                    }
                };
            SqlDataReader rdr = ob.ExecuteReaderWithParamiterAndReturntype("SpResetPassword", paramList);
                while(rdr.Read())
                {
                    if(Convert.ToBoolean(rdr["returncode"]))
                    {
                        ob.sendPasswordResetEmail(forgetpassEmail.Text, rdr["userName"].ToString(),rdr["uniqueid"].ToString());
                        Label1.Text = "an Email with instruction to reset your password is sent to your registered Email";
                    }
                    else
                    {
                        Label1.ForeColor = System.Drawing.Color.Green;
                        Label1.Text = "Email not found !";
                    }
                }
            }
        }
    }
