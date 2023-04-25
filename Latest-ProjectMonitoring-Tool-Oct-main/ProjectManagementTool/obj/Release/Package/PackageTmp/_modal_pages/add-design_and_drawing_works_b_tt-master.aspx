
<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/modal.Master" AutoEventWireup="true" CodeBehind="add-design_and_drawing_works_b_tt-master.aspx.cs" Inherits="ProjectManagementTool._modal_pages.add_design_and_drawing_works_b_tt_master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="modal_master_head" runat="server">
     <script>
         $(function () {
             var date = new Date();
    $("input[id$='dtMeetingDate']").datepicker({
      changeMonth: true,
        changeYear: true,
        dateFormat: 'dd/mm/yy',
        maxDate: date,
      });
    });
     </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="modal_master_body" runat="server">
    <form id="AddReviewMeetingMaster" runat="server">
        <div class="container-fluid" style="overflow-y:auto; max-height:85vh; min-height:76vh;">
        <div class="row">
            <div class="col-sm-12">
                 <div class="form-group">
                         <label class="lblCss" for="txtdesc">Design and Drawing Works B TT Description</label> &nbsp;<span style="color:red; font-size:1.2rem;">*</span>
                         <asp:TextBox ID="txtdesc" runat="server" CssClass="form-control" autocomplete="off" required></asp:TextBox>
                   </div>
                            <div class="form-group">
                                <label class="lblCss" for="txtprojectclassdesc">Design and Drawing Works B TT Date</label> &nbsp;<span style="color:red; font-size:1.2rem;">*</span>
                                <asp:TextBox ID="dtMeetingDate" runat="server" placeholder="dd/mm/yyyy" autocomplete="off" CssClass="form-control" required></asp:TextBox>
                            </div>
               </div>
            </div>
        </div>

        <div class="modal-footer">
            <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-primary" OnClick="btnSubmit_Click" />
                </div>
    </form>
</asp:Content>

