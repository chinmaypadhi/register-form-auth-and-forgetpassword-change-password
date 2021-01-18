<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="WebApplication76.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="static/jquery/jquery-2.1.1.min.js"></script>
    <script src="static/jquery/jquery-1.11.1.min.js"></script>
    <link href="static/css/chin.css" rel="stylesheet" />

</head>
<body>
    <form id="form1" runat="server">
      <div class="login-wrap">
	<div class="login-html">
		<input id="tab-1" type="radio" name="tab" class="sign-in" checked><label for="tab-1" class="tab">Sign In</label>
		<input id="tab-2" type="radio" name="tab" class="sign-up"><label for="tab-2" class="tab">Sign Up</label>
		<div class="login-form">
			<div class="sign-in-htm">
				<div class="group">
					<label for="user" class="label">Email</label>
					<asp:TextBox ID="UserEmail" CssClass="input" runat="server"></asp:TextBox>
				</div>
				<div class="group">
					<label for="pass" class="label">Password</label>
					<asp:TextBox ID="Password" runat="server" CssClass="input"></asp:TextBox>
				</div>
				<div class="group">
                <asp:CheckBox ID="sndCheckbox" runat="server"  />Keep me Signed in<br />
					<asp:Label ID="errorLogin" runat="server" ></asp:Label>
<%--					<label for="check"><span class="icon"></span> Keep me Signed in</label>--%>
				</div>
				<div class="group">
					<asp:Button ID="sndButton" runat="server" Text="Sign In" CssClass="button" OnClick="sndButton_Click" />
				</div>
				<div class="hr"></div>
				<div class="foot-lnk">
<a href='#' onclick='window.open("registration/forgetpass.aspx", "FP", "scrollbars=yes, width=500,left=500,top=300,fullscreen=no,resizable=0,height=50"); return false;'>forgotpassword?</a>				</div>
			</div>
			<div class="sign-up-htm">
				<div class="group">
					<label for="user" class="label">Username</label>
					<asp:TextBox ID="signupuser" runat="server" CssClass="input"></asp:TextBox>
				</div>
				<div class="group">
					<label for="pass" class="label">Password</label>
				  <asp:TextBox ID="signupPass" CssClass="input" runat="server"></asp:TextBox>
				</div>
				<div class="group">
					<label for="pass" class="label">Repeat Password</label>
				<asp:TextBox ID="singupR_Pass" CssClass="input" runat="server"></asp:TextBox>
				</div>
				<div class="group">
					<label for="pass" class="label">Email Address</label>
                    <asp:TextBox ID="signupEmail" CssClass="input" runat="server"></asp:TextBox>
				</div>
				<div class="group">
			    <asp:Button ID="signupButton" CssClass="button" runat="server" Text="Sign up" OnClick="signupButton_Click" />
				</div>
				<div class="hr"></div>
				<div class="foot-lnk">
				</div>           
			</div>
		</div>
	</div>
		  <span><asp:Label ID="errorsignup" runat="server" Text="Label"></asp:Label>
</span>
</div>
    </form>
</body>
</html>
