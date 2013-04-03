<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddTape.aspx.cs" Inherits="Mocument.WebUI.Tapes.AddTape" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <p>
        Tape Name:
        <asp:TextBox ID="NameTextBox" runat="server"></asp:TextBox>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
            ControlToValidate="NameTextBox" ErrorMessage="RequiredFieldValidator">Field is required</asp:RequiredFieldValidator>
        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
            ControlToValidate="NameTextBox" ErrorMessage="RegularExpressionValidator" 
            ValidationExpression="^[0-9a-zA-Z_]+$">Only alpha-numeric and underscore are allowed</asp:RegularExpressionValidator>
    </p>
    <p>
        Description:<asp:TextBox ID="DescTextBox" runat="server"></asp:TextBox>
    </p>
    <p>
        Open For Recording:<asp:CheckBox ID="LockedCheckBox" runat="server" />
    </p>
    <p>
        Allowed IP:<asp:TextBox ID="IpTextBox" runat="server"></asp:TextBox>
    </p>
    <p>
        <asp:Button ID="AddButton" runat="server" onclick="AddButton_Click" 
            Text="Add Tape" />
&nbsp;<asp:Button ID="CancelButton" runat="server" onclick="CancelButton_Click" 
            Text="Cancel" CausesValidation="False" />
    </p>
    <p>
        &nbsp;</p>
</asp:Content>
