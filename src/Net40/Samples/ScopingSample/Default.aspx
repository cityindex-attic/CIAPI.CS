<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ScopingSample.Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Panel ID="SessionPanel" runat="server" Visible="true">
            Global Session:
            <asp:Label ID="lblGlobalSession" runat="server" Text=""></asp:Label><br />
            Client Session:
            <asp:Label ID="lblUserSession" runat="server" Text=""></asp:Label>
        </asp:Panel>
        <hr />
        <asp:Panel ID="LoginPanel" runat="server" Visible="true">
            Username:
            <asp:TextBox ID="txtUserName" runat="server" Text="XX070608"></asp:TextBox><br />
            Password:
            <asp:TextBox ID="txtPassword" runat="server" Text="password"></asp:TextBox><br />
            <asp:Button ID="btnLogin" runat="server" Text="Login" 
                onclick="btnLogin_Click" /><br />
            <asp:Label ID="lblLoginError" runat="server" Text="" ForeColor="Red"></asp:Label>
        </asp:Panel>
        <asp:Panel ID="pnlGlobalOps" runat="server">
            <asp:BulletedList ID="lstGlobalHeadlines" runat="server">
            </asp:BulletedList>
            <asp:GridView ID="GridView1" runat="server" 
                DataSourceID="NewsHeadlinesDataSource">
            </asp:GridView>
            <asp:ObjectDataSource ID="NewsHeadlinesDataSource" runat="server">
            </asp:ObjectDataSource>
        </asp:Panel>
        <asp:Panel ID="pnlSessionOps" runat="server">
        </asp:Panel>
    </div>
    </form>
</body>
</html>
