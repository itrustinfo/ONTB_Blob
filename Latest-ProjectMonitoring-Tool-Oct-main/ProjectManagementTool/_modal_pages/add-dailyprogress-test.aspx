<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/modal.Master" AutoEventWireup="true" CodeBehind="add-dailyprogress-test.aspx.cs" Inherits="ProjectManagementTool._modal_pages.add_dailyprogress_test" %>


<asp:Content ID="Content1" ContentPlaceHolderID="modal_master_head" runat="server">
<script type="text/javascript">

    $(function () {
        $("input[id$='txtSubmittedDate']").datepicker({
            changeMonth: true,
            changeYear: true,
            dateFormat: 'dd/mm/yy'
        });

        $("input[id$='txtONTBReleasedDate']").datepicker({
            changeMonth: true,
            changeYear: true,
            dateFormat: 'dd/mm/yy'
        });

        $("input[id$='txtONTBReleasedBalanceDate']").datepicker({
            changeMonth: true,
            changeYear: true,
            dateFormat: 'dd/mm/yy'
        });
    });

    $('#txtONTBReleasedLength').change(function () {

        alert('reached');
        var balance = $(this).val() - $("#txtLength").val();
        $("txtONTBReleasedBalance").val(balance);

    })

</script>

<%--<script type="text/javascript">
    $(document).ready(function () {
        $('#txtONTBReleasedLength').change(function () {

            alert('reached');
            var balance = $(this).val() - $("#txtLength").val();
            $("txtONTBReleasedBalance").val(balance);

        })
    });

</script>--%>
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="modal_master_body" runat="server">
    <form id="frmAddIssuesModal" runat="server">
        <asp:HiddenField ID="ProjectHiddenValue" runat="server" /> -
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
                        <label class="lblCss" for="txtLocation">Submitted Location</label>
                        <asp:TextBox ID="txtLocation" runat="server" CssClass="form-control" required ></asp:TextBox>
                    </div>
                    <div class="form-group" id="AcvitityName" runat="server">
                        <label class="lblCss" for="txtLength">Submitted Length</label>
                        <asp:TextBox ID="txtLength" runat="server" CssClass="form-control" required ></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="txtSubmittedDate">Submitted Date</label>
                        &nbsp;<span style="color: red; font-size: 1.1rem;">*</span>
                        <asp:TextBox ID="txtSubmittedDate" runat="server"  CssClass="form-control" required></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="txtONTBReleasedLength"> ONTB Released Length</label>
                        &nbsp;<span style="color: red; font-size: 1.1rem;">*</span>
                        <asp:TextBox ID="txtONTBReleasedLength" runat="server"  CssClass="form-control" required></asp:TextBox>
                    </div>
                     <div class="form-group">
                        <label class="lblCss" for="txtONTBReleasedDate">ONTB Released Date</label>
                        &nbsp;<span style="color: red; font-size: 1.1rem;">*</span>
                        <asp:TextBox ID="txtONTBReleasedDate" runat="server"  CssClass="form-control" required></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="txtONTBReleasedBalance">ONTB Released Balance</label>&nbsp;<span style="color: red; font-size: 1.1rem;">*</span>
                        <asp:TextBox ID="txtONTBReleasedBalance" runat="server"  CssClass="form-control" required></asp:TextBox>
                    </div>
                </div>
                <div class="col-sm-6">
                     <div class="form-group">
                        <label class="lblCss" for="txtONTBReleasedBalanceDate">ONTB Released Balance Date</label>
                        &nbsp;<span style="color: red; font-size: 1.1rem;">*</span>
                        <asp:TextBox ID="txtONTBReleasedBalanceDate" runat="server"  CssClass="form-control" required></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="txtGFCApproved">GFC Approved</label>&nbsp;<span style="color: red; font-size: 1.1rem;">*</span>
                        <asp:TextBox ID="txtGFCApproved" runat="server"  CssClass="form-control" OnTextChanged="TxtId_TextChanged"  AutoPostBack="true" required></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="txtRemarks">Remarks</label>&nbsp;<span style="color: red; font-size: 1.1rem;">*</span>
                        <asp:TextBox ID="txtRemarks" runat="server"  CssClass="form-control" required></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="txtEEOfficeApproval">EE Office Approval</label>&nbsp;<span style="color: red; font-size: 1.1rem;">*</span>
                        <asp:TextBox ID="txtEEOfficeApproval" runat="server"  CssClass="form-control" required></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="txtNineSets">Nine Sets Submitted</label>
                        <asp:TextBox ID="txtNineSets" CssClass="form-control" TextMode="MultiLine" runat="server" ClientIDMode="Static"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="txtApproved">Approved</label>
                        <asp:TextBox ID="txtApproved" CssClass="form-control" TextMode="MultiLine" runat="server" ClientIDMode="Static"></asp:TextBox>
                    </div>
                </div>
            </div>
        </div>
        <div class="modal-footer">
            <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-primary" OnClick="btnSubmit_Click" />
        </div>
    </form>
</asp:Content>


