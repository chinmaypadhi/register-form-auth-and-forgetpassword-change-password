<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LockedAcount.aspx.cs" Inherits="WebApplication76.LockedAcount" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .auto-style1 {
            text-align: center;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
          <div>
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" OnRowCommand="GridView1_RowCommand">
            <Columns>
                <asp:BoundField DataField="userName" HeaderText="userName" />
                <asp:BoundField DataField="Email" HeaderText="Email" />
                <asp:BoundField DataField="LockedDateTime" HeaderText="LockedDateTime" />
                <asp:BoundField DataField="HoursElapsed" HeaderText="HoursElapsed" />
                <asp:TemplateField HeaderText="Enable">
                    <ItemTemplate>
                <asp:Button ID="Button1" runat="server"  CommandArgument='<%# Eval("email") %>' Enabled='<%#Convert.ToInt32(Eval("HoursElapsed")) > 24%>' Text="Enable" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>     
        </div>
    </form>
</body>
</html>
