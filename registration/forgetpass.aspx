<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="forgetpass.aspx.cs" Inherits="WebApplication76.registration.forgetpass" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../static/css/chin1.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div>
           <label style="color:green">enter the email id</label>
            <asp:TextBox ID="forgetpassEmail"  runat="server" CssClass="text" placeholder="Enter Email here.."></asp:TextBox>
            <asp:Button ID="Button1" runat="server" Text="send" OnClick="Button1_Click" />
            <asp:Label ID="Label1" runat="server" ></asp:Label>
        </div>
        <div>
        </div>
    </form>
</body>
</html>
