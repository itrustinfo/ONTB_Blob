<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/modal.Master" AutoEventWireup="true" CodeBehind="add-Design_and_drawing_works_b_tt.aspx.cs" Inherits="ProjectManagementTool._modal_pages.add_Design_and_drawing_works_b_tt" %>

<asp:Content ID="Content1" ContentPlaceHolderID="modal_master_head" runat="server">
    <script type="text/javascript">
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
                        <label class="lblCss" for="txtactivityname">Vilage Name</label>
                        <asp:TextBox ID="txtVillageName" runat="server" CssClass="form-control" required ></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="txtIssue_Description">Crossing </label>
                        &nbsp;<span style="color: red; font-size: 1.1rem;">*</span>
                        <asp:TextBox ID="txtCrossing" runat="server"  CssClass="form-control" required></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="txtIssue_Description">GFC Status </label>
                        &nbsp;<span style="color: red; font-size: 1.1rem;">*</span>
                        <asp:TextBox ID="txtgfcstatus" runat="server"  CssClass="form-control" required></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="txtIssue_Description">Length  m</label>
                        &nbsp;<span style="color: red; font-size: 1.1rem;">*</span>
                        <asp:TextBox ID="txtLength" runat="server"  CssClass="form-control" required></asp:TextBox>
                    </div>
                  
                </div>
                <div class="col-sm-6">
                      <div class="form-group">
                        <label class="lblCss" for="txtIssue_Description">Diameter mm </label>&nbsp;<span style="color: red; font-size: 1.1rem;">*</span>
                        <asp:TextBox ID="txtDiameter" runat="server"  CssClass="form-control" required></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="txtIssue_Description">Start Depth m</label>&nbsp;<span style="color: red; font-size: 1.1rem;">*</span>
                        <asp:TextBox ID="txtStartDepth" runat="server"  CssClass="form-control"  required></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="txtIssue_Description">Stop Depth m</label>&nbsp;<span style="color: red; font-size: 1.1rem;">*</span>
                        <asp:TextBox ID="txtStopDepth" runat="server"  CssClass="form-control"   AutoPostBack="true" required></asp:TextBox>
                    </div>
                   
                    <div class="form-group">
                        <label class="lblCss" for="txtRefNumber">Remarks</label>
                        <asp:TextBox ID="txtRemarks" CssClass="form-control" TextMode="MultiLine" runat="server" ClientIDMode="Static"></asp:TextBox>
                    </div>

                </div>
            </div>
        </div>
        <div class="modal-footer">
            <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-primary" OnClick="btnSubmit_Click" />
        </div>
    </form>
</asp:Content>


