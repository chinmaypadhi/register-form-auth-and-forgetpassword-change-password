using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using MYBLL;
namespace WebApplication76
{
    public partial class LockedAcount : System.Web.UI.Page
    {
        static mybll ob = new mybll(globalConnection.str);
        protected void Page_Load(object sender, EventArgs e)
        {
            if (User.Identity.Name.ToLower() == "padhichinmay108@gmail.com")
            {
                if (!IsPostBack)
                {
                    GridView1.DataSource = ob.ExecuteReaderWithReturntype("spGetAllLocakedUserAccounts");
                    GridView1.DataBind();
                }
            }
            else
            {
                Response.Redirect("~/AccessDenied.aspx");
            }
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            EnableUserAccount(e.CommandArgument.ToString());
            GridView1.DataSource = ob.ExecuteReaderWithReturntype("spGetAllLocakedUserAccounts");
            GridView1.DataBind();
        }
        private void EnableUserAccount(string email)
        {
            List<SqlParameter> paramList = new List<SqlParameter>()
    {
        new SqlParameter()
        {
            ParameterName = "@email",
            Value =email
        }
    };
       ob.ExecteNonqueryWithoutReturn("spEnableUserAccount", paramList);
   
        }

    }
}