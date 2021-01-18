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

namespace WebApplication76.registration
{
    public partial class forgetpass : System.Web.UI.Page
    {

        private void sendPasswordResetEmail(string ToEmployee,string userName,string  uniqueId )
        {
            MailMessage MailMessage = new MailMessage("padhichinmay108@gmail.com", ToEmployee);
            //string builder is present inside system.text namespace
            StringBuilder sbEmailBody = new StringBuilder();
            sbEmailBody.Append("dear" + userName +"<br/><br/>");
            sbEmailBody.Append("please click on the following link to reset your pasword");
            sbEmailBody.Append("<br/>");
            sbEmailBody.Append("http://localhost/WebApplication76/Registration/changepassword.aspx?uid=" + uniqueId);
            sbEmailBody.Append("<b>chinmay technologies</b>");
            MailMessage.IsBodyHtml = true;
            MailMessage.Body = sbEmailBody.ToString();
            MailMessage.Subject = "reset your pasword";
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com",587);
            smtpClient.Credentials = new System.Net.NetworkCredential() {
                UserName = "padhichinmay108@gmail.com",
            Password = "9778346572"
            };
            smtpClient.EnableSsl = true;
            smtpClient.Send(MailMessage);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string str = ConfigurationManager.ConnectionStrings["DBCS"].ToString();
            using (SqlConnection con = new SqlConnection(str))
            {
                SqlCommand cmd = new SqlCommand("SpResetPassword",con);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter paramiterName = new SqlParameter("@email", forgetpassEmail.Text);
                cmd.Parameters.Add(paramiterName);
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while(rdr.Read())
                {
                    if(Convert.ToBoolean(rdr["returncode"]))
                    {
                        sendPasswordResetEmail(forgetpassEmail.Text, rdr["userName"].ToString(),rdr["uniqueid"].ToString());
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
}