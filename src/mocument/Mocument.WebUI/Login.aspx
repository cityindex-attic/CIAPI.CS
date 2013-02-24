<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Mocument.WebUI.Login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
          <asp:Button ID="btnGoogleLogin" runat="server" CommandArgument="https://www.google.com/accounts/o8/id" 
        OnCommand="BtnGoogleLoginClick" Text="Sign In with Google" />
</asp:Content>
