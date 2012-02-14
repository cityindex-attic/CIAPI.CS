<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true"
    CodeBehind="Index.aspx.cs" Inherits="ClientScopingExample.Index" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<asp:Button ID="ChangePasswordButton" runat="server" Text="Change Password" 
            onclick="ChangePasswordButtonClick" Visible="false"/><br />
    <asp:Panel ID="LogInPanel" Visible="false" runat="server" GroupingText="Login">
        Username:
        <asp:TextBox ID="UserNameTextBox" runat="server" Text="xx189949"></asp:TextBox><br />
        Password:
        <asp:TextBox ID="PasswordTextBox" runat="server" Text="password"></asp:TextBox><br />
        <asp:Button ID="LoginButton" runat="server" Text="Login" 
            onclick="LoginButtonClick" /><br />
    </asp:Panel>
    <asp:Panel ID="ChangePasswordPanel" Visible="false" runat="server" GroupingText="Change Password">
        Old Password:
        <asp:TextBox ID="ChangePasswordOldPasswordTextBox" runat="server"></asp:TextBox><br />
        New Password:
        <asp:TextBox ID="ChangePasswordNewPasswordTextBox" runat="server"></asp:TextBox><br />
        
    </asp:Panel>
    <asp:Panel ID="LoggedInPanel" Visible="false" runat="server">
            <div id="MarketInfoContainer" style="border: 1px solid green">
                Market ID: <asp:TextBox ID="MarketId" Text="71442" runat="server"></asp:TextBox><br />
                <asp:Button ID="GetMarketInfoButton" runat="server" Text="Get Market Info" 
                    onclick="GetMarketInfoButtonClick" /><br />
                


            <asp:Literal ID="MarketInfo" Mode="PassThrough" runat="server"></asp:Literal>
            
        </div>
    </asp:Panel>
    <asp:TextBox ID="ErrorMessage" runat="server" Width="100%" ForeColor="Red" BorderColor="Red" BorderWidth="1px"
        Visible="false"></asp:TextBox>
</asp:Content>
