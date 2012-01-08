<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ApplicationScopingExample.Default"  %>
<%@ Import Namespace="ApplicationScopingExample" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    Client Session: <%=Global.RpcClient.Session %>
        <br />
        <asp:Button ID="Button1" runat="server" Text="Fetch Headlines" onclick="Button1_Click" />
        <br />
        Headlines:<br />
        <asp:ListBox ID="ListBox1" runat="server" Height="94px" Width="695px"></asp:ListBox>
        <br />
        Log: 
        <asp:Button ID="Button2" runat="server" Text="Clear Log" 
            onclick="Button2_Click" /><br />
        <asp:ListBox ID="ListBox2" runat="server" Height="491px" Width="690px"></asp:ListBox>
    </div>
    </form>
</body>
</html>
