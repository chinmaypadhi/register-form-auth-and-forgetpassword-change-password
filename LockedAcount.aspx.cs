using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace WebApplication76
{
    public partial class LockedAcount : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (User.Identity.Name.ToLower() == "padhichinmay108@gmail.com")
            {
                if (!IsPostBack)
                {
                    GetData();
                }
            }
            else
            {
                Response.Redirect("~/AccessDenied.aspx");
            }
        }
        private void GetData()
        {
            string CS = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("spGetAllLocakedUserAccounts", con);
                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();
                GridView1.DataSource = cmd.ExecuteReader();
                GridView1.DataBind();
            }
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            EnableUserAccount(e.CommandArgument.ToString());
            GetData();
        }
        private void EnableUserAccount(string email)
        {
            string CS = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("spEnableUserAccount", con);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter paramUserName = new SqlParameter()
                {
                    ParameterName = "@email",
                    Value = email
                };

                cmd.Parameters.Add(paramUserName);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

    }
}