<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/modal.Master" AutoEventWireup="true" CodeBehind="add-Design_and_drawing_works_A.aspx.cs" Inherits="ProjectManagementTool._modal_pages.add_Design_and_drawing_works_A" %>

<asp:Content ID="Content1" ContentPlaceHolderID="modal_master_head" runat="server">
   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="modal_master_body" runat="server">
    <form id="frmAddIssuesModal" runat="server">
        <asp:HiddenField ID="ProjectHiddenValue" runat="server" />
        <asp:HiddenField ID="WorkPackageHiddenValue" runat="server" />
        <asp:HiddenField ID="TaskHiddenValue" runat="server" />
        <div class="container-fluid" style="max-height: 80vh; overflow-y: auto;">
            <div class="row">
                <div class="col-sm-6">
                    <div class="form-group" id="Div1" runat="server">
                        <label class="lblCss" for="txtactivityname">Daily Progress Master</label>
                        <asp:DropDownList ID="DDLDailyReportMaster" CssClass="form-control"  runat="server" ></asp:DropDownList>
                    </div>
                   
                    <div class="form-group" id="AcvitityName" runat="server">
                        <label class="lblCss" for="txtactivityname">Heading</label>
                        <asp:TextBox ID="txtHeading" runat="server" CssClass="form-control" required ></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="txtIssue_Description">Sub Heading </label>
                        &nbsp;<span style="color: red; font-size: 1.1rem;">*</span>
                        <asp:TextBox ID="txtSubHeading" runat="server"  CssClass="form-control" required></asp:TextBox>
                    </div>
                               
                  
                </div>
                <div class="col-sm-6">
                      <div class="form-group">
                        <label class="lblCss" for="txtIssue_Description">Units</label>
                        &nbsp;<span style="color: red; font-size: 1.1rem;">*</span>
                        <asp:TextBox ID="txtUnits" runat="server"  CssClass="form-control"  required></asp:TextBox>
                    </div>
                      <div class="form-group">
                        <label class="lblCss" for="txtIssue_Description">Site Works</label>&nbsp;<span style="color: red; font-size: 1.1rem;">*</span>
                        <asp:TextBox ID="txtSiteWorks" runat="server"  TextMode="MultiLine" CssClass="form-control" required></asp:TextBox>
                    </div>
                   
                 

                </div>
            </div>
        </div>
        <div class="modal-footer">
            <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-primary" OnClick="btnSubmit_Click" />
        </div>
    </form>
</asp:Content>


