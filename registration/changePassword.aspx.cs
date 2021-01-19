using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Security;

namespace WebApplication76.registration
{
    public partial class changePassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!IsPasswordResetLinkValid())
                {
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    lblMessage.Text = "Password Reset link has expired or is invalid";
                }
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (ChangeUserPassword())
            {
                lblMessage.Text = "Password Changed Successfully!";
            }
            else
            {
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = "Password Reset link has expired or is invalid";
            }
        }
       

        private bool IsPasswordResetLinkValid()
        {
            List<SqlParameter> paramList = new List<SqlParameter>()
    {
        new SqlParameter()
        {
            ParameterName = "@GUID",
            Value = Request.QueryString["uid"]
        }
    };

            return ExecuteSP("spIsPasswordResetLinkValid", paramList);
        }
        private bool ExecuteSP(string SPName, List<SqlParameter> SPParameters)
        {
            string CS = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand(SPName, con);
                cmd.CommandType = CommandType.StoredProcedure;

                foreach (SqlParameter parameter in SPParameters)
                {
                    cmd.Parameters.Add(parameter);
                    Response.Write(parameter);
                }

                con.Open();
                Response.Write(cmd.ExecuteScalar());
                Response.Write(Convert.ToBoolean(cmd.ExecuteScalar()));
                return Convert.ToBoolean(cmd.ExecuteScalar());
            }
        }
//for change thr password
        private bool ChangeUserPassword()
        {
            List<SqlParameter> paramList = new List<SqlParameter>()
    {
        new SqlParameter()
        {
            ParameterName = "@GUID",
            Value = Request.QueryString["uid"]
        },
        new SqlParameter()
        {
            ParameterName = "@Password",
            Value = FormsAuthentication.HashPasswordForStoringInConfigFile(txtNewPassword.Text, "SHA1")
        }
    };

            return ExecuteSP("spChangePassword", paramList);
        }
    }
}