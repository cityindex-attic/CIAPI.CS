<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
         CodeBehind="MyTapes.aspx.cs" Inherits="Mocument.WebUI.Tapes.MyTapes" %>
<%@ Import Namespace="Mocument.WebUI.Code" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        
        .NameCell {
            background-color: #f5f5f5;
            border-right: 1px solid black;
            font-variant: small-caps;
            font-weight: bold;
            margin: 5px;
            padding: 5px;
            text-align: right;
            vertical-align: top;
            width: 5%;
        }

        .HeaderCell {
            background-color: #f5f5f5;
            font-variant: small-caps;
            font-weight: bold;
            margin: 5px;
            padding: 5px;
            text-align: center;
            vertical-align: bottom;
            width: 5%;
        }

        .subtable {
            border: 1px solid black;
            font-size: smaller;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <p>
        <asp:Button ID="AddButton" runat="server" OnClick="AddButton_Click" Text="ADD NEW TAPE" />
    </p>
    <p>
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="Id"
                      DataSourceID="ObjectDataSource1" AllowPaging="True" 
                      onselectedindexchanged="GridView1_SelectedIndexChanged" 
            onrowcommand="GridView1_RowCommand">
            <Columns>
                <asp:TemplateField ShowHeader="False">
                    <EditItemTemplate>
                        <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="True" 
                            CommandName="Update" Text="Update"></asp:LinkButton>
                        &nbsp;<asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" 
                            CommandName="Cancel" Text="Cancel"></asp:LinkButton>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" 
                            CommandName="Edit" Text="Edit"></asp:LinkButton>
                        &nbsp;<asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" 
                            CommandName="Select" Text="Details"></asp:LinkButton> &nbsp;<asp:LinkButton ID="LinkButton4" runat="server" CausesValidation="False" 
                            CommandName="Export" Text="Export" CommandArgument='<%# Eval("Id") %>'></asp:LinkButton>
                        &nbsp;<asp:LinkButton ID="LinkButton3" runat="server" CausesValidation="False" 
                            CommandName="Delete" Text="Delete" OnClientClick="return confirm('Are you sure you want to delete this tape?');"></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Id" SortExpression="Id">
                    <EditItemTemplate>
                        <asp:Label ID="TextBox1" runat="server" Text='<%# ProxySettings.GetTapeId(Eval("Id")) %>'></asp:Label>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%# ProxySettings.GetTapeId(Eval("Id")) %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Description" HeaderText="Description" 
                                SortExpression="Description" />
                <asp:CheckBoxField DataField="OpenForRecording" HeaderText="OpenForRecording" SortExpression="OpenForRecording" />
                <asp:BoundField DataField="AllowedIpAddress" HeaderText="AllowedIpAddress" SortExpression="AllowedIpAddress" />
                <asp:BoundField DataField="Comment" HeaderText="Comment" 
                                SortExpression="Comment" />
            </Columns>
        </asp:GridView>
    </p>
    <asp:Panel ID="Panel1" runat="server" GroupingText="Details" ScrollBars="Auto">
        <asp:Label ID="RecordLabel" runat="server"></asp:Label>
        <br />
        <asp:Label ID="PlayLabel" runat="server"></asp:Label>
        &nbsp;</asp:Panel>
    <p>
    </p>
    <p>
        &nbsp;</p>
    <p>
        &nbsp;</p>

    <p>
        <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="ListTapesForUser"
                              TypeName="Mocument.WebUI.Code.ContextDataSource" DataObjectTypeName="Mocument.Model.Tape"
                              DeleteMethod="Delete" InsertMethod="Insert" 
            UpdateMethod="Update" onupdating="ObjectDataSource1_Updating">
        </asp:ObjectDataSource>
    </p>
</asp:Content>