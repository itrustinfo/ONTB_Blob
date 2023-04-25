<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/default.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="ProjectManagementTool._content_pages.Convert_to_blob._default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="default_master_head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="default_master_body" runat="server">
            <div class="container-fluid">
            <div class="row">
                <div class="col-md-12 col-lg-2 form-group">Covert Documents to Blob</div>
                <div class="col-md-6 col-lg-4 form-group">
                    <label class="sr-only" for="DDLProject">Project</label>
                    <div class="input-group">
                        <div class="input-group-prepend">
                            <span class="input-group-text">Project</span>
                        </div>
                        <asp:DropDownList ID="DDlProject" runat="server" CssClass="form-control"></asp:DropDownList>
                    </div>
                </div>
                <div class="col-md-12 col-lg-2 form-group">
                    
                </div>
            </div>

        </div>
        <div class="container-fluid">
            <div class="row">
                <div class="col-lg-12 col-xl-12 form-group">
                    <asp:RadioButtonList ID="RBList" runat="server" OnSelectedIndexChanged="RBList_SelectedIndexChanged" AutoPostBack="true" RepeatDirection="Horizontal" Font-Bold="true">
                        <asp:ListItem Value="Actual Documents" Selected="True">Actual Documents  </asp:ListItem>
                        <asp:ListItem Value="General Documents">General Documents  </asp:ListItem>
                        <asp:ListItem Value="Issues">Issues</asp:ListItem>
                        <asp:ListItem Value="Document Status">Document Status Pending</asp:ListItem>
                        <asp:ListItem Value="Document Attachments">Document Attachments</asp:ListItem>
                    </asp:RadioButtonList>
                    <br />
                    <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-primary" OnClick="btnSubmit_Click" />
                    </div>
                <div class="col-lg-12 col-xl-12 form-group">
                    <asp:Label ID="LblMessage" runat="server" Font-Bold="true" ForeColor="Green"></asp:Label>
                    <asp:Label ID="LblProgress" runat="server" Font-Bold="true" ForeColor="Green" Text="Conversion is in Progress... Please wait..."></asp:Label>
                    </div>
                </div>
            </div>
</asp:Content>
