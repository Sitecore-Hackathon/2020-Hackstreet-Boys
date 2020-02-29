<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TestMe.aspx.cs" Inherits="Hackathon.Foundation.Teams.TestMe" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
        	<asp:Label ID="Label1" runat="server" Text="Enter your team name"></asp:Label>
&nbsp;:
			<asp:TextBox ID="teamName" runat="server"></asp:TextBox>
			<asp:Button ID="CreateTeamButton" runat="server" OnClick="CreateTeamButton_Click" Text="Do Magic" />
			<br />
			<br />
        </div>
    </form>
</body>
</html>
