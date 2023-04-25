<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/modal.Master" AutoEventWireup="true" CodeBehind="add-Design_and_drawing_works_dwg_issue.aspx.cs" Inherits="ProjectManagementTool._modal_pages.add_Design_and_drawing_works_dwg_issue" %>

<asp:Content ID="Content1" ContentPlaceHolderID="modal_master_head" runat="server">
    <script type="text/javascript">
       
            $(function () {
             var date = new Date();
    $("input[id$='txtSubmittedDate']").datepicker({
                changeMonth: true,
        changeYear: true,
        dateFormat: 'dd/mm/yy',
        maxDate: date,
    });
                $("input[id$='txtapproveddate']").datepicker({
                    changeMonth: true,
                    changeYear: true,
                    dateFormat: 'dd/mm/yy',
                    maxDate: date,
                });

                $("input[id$='txtGFCApprovedDate']").datepicker({
                    changeMonth: true,
                    changeYear: true,
                    dateFormat: 'dd/mm/yy',
                    maxDate: date,
                });
    });
  
    </script>
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
                    <div class="form-group" id="Div2" runat="server">
                        <label class="lblCss" for="txtactivityname">Zone Name</label>
                        <asp:DropDownList ID="ddlZoneName" runat="server" CssClass="form-control" >
                            <asp:ListItem Selected="True" Value="Bommanahalli">Bommanahalli</asp:ListItem>
                            <asp:ListItem  Value="Mahadevapura">Mahadevapura</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="form-group" id="AcvitityName" runat="server">
                        <label class="lblCss" for="txtactivityname">Location</label>
                        <asp:TextBox ID="txtLocation" runat="server" CssClass="form-control" required ></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="txtIssue_Description">Submitted By Contractor </label>
                        &nbsp;<span style="color: red; font-size: 1.1rem;">*</span>
                        <asp:TextBox ID="txtSubmittedByContractor" runat="server"  CssClass="form-control" required></asp:TextBox>
                    </div>
                               
                  
                </div>
                <div class="col-sm-6">
                      <div class="form-group">
                        <label class="lblCss" for="txtSubmittedDate">Submitted Date</label>
                        &nbsp;<span style="color: red; font-size: 1.1rem;">*</span>
                        <asp:TextBox ID="txtSubmittedDate" runat="server"  CssClass="form-control" required></asp:TextBox>
                    </div>
                      <div class="form-group">
                        <label class="lblCss" for="txtapprovedbyontb">GFC Released by ONTB</label>&nbsp;<span style="color: red; font-size: 1.1rem;">*</span>
                        <asp:TextBox ID="txtapprovedbyontb" runat="server"  CssClass="form-control" required></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="txtapproveddate">Date Approved by ONTB</label>&nbsp;<span style="color: red; font-size: 1.1rem;">*</span>
                        <asp:TextBox ID="txtapproveddate" runat="server"  CssClass="form-control"  required></asp:TextBox>
                    </div>
                  <div class="form-group">
                        <label class="lblCss" for="txtGFCApprovedByBWSSB">GFC Approved by BWSSB</label>&nbsp;<span style="color: red; font-size: 1.1rem;">*</span>
                        <asp:TextBox ID="txtGFCApprovedByBWSSB" runat="server"  CssClass="form-control"  required></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="">Date of Approval by BWSSB</label>&nbsp;<span style="color: red; font-size: 1.1rem;">*</span>
                        <asp:TextBox ID="txtGFCApprovedDate" runat="server"  CssClass="form-control"  required></asp:TextBox>
                    </div>

                </div>
            </div>
        </div>
        <div class="modal-footer">
            <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-primary" OnClick="btnSubmit_Click" />
        </div>
    </form>
</asp:Content>


