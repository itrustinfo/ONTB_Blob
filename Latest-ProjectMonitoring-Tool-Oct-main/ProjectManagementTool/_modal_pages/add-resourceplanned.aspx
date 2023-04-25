<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/modal.Master" AutoEventWireup="true" CodeBehind="add-resourceplanned.aspx.cs" Inherits="ProjectManagementTool._modal_pages.add_resourceplanned" %>
<asp:Content ID="Content1" ContentPlaceHolderID="modal_master_head" runat="server">
        <style type="text/css">
        .TheDateTimePicker{display:block;width:100%;height:calc(1.5em + .75rem + 2px);padding:.375rem .75rem;font-size:1rem;font-weight:400;line-height:1.5;color:#495057;background-color:#fff;background-clip:padding-box;border:1px solid #ced4da;border-radius:.25rem;transition:border-color .15s ease-in-out,box-shadow .15s ease-in-out;}
    </style>
      <script type="text/javascript">
        $(function () {
        bindDatePickers(); // bind date picker on first page load
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(bindDatePickers); // bind date picker on every UpdatePanel refresh
    });

          function bindDatePickers() {
              $('.TheDateTimePicker').datepicker({ dateFormat: 'dd/mm/yy',changeMonth: true,changeYear: true });
          }


          function ShowProgressBar(status) {
              
              if (status == "true") {
                  document.getElementById('dvProgressBar').style.visibility = 'visible';
              }
              else {
                  document.getElementById('dvProgressBar').style.visibility = 'hidden';
              }
          }

          function DeleteItem() {
              if (confirm("Are you sure you want to delete ...?")) {
                  return true;
              }
              return false;
          }

</script>
<style type="text/css">
  .hiddencol
  {
    display: none;
  }
</style>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="modal_master_body" runat="server">
    <form id="frmAddTaskSchedule" runat="server">
       <asp:ScriptManager ID="scrpManager" runat="server" />
        <div class="container-fluid" style="max-height:84vh; overflow-y:auto; min-height:84vh;">
            <asp:UpdatePanel runat="server" ID="myUpdtPanel" UpdateMode="Conditional">   
            <ContentTemplate>
                
                <asp:HiddenField ID="HiddenAction" runat="server" />
            <div class="row">
                 <div class="col-sm-6">
                     <div class="form-group">
                        <label class="lblCss" for="RBScheduleTye">Planned Type</label>
                          <asp:RadioButtonList ID="RBScheduleTye" runat="server" Width="100%" CssClass="lblCss" CellPadding="5" RepeatDirection="Horizontal" AutoPostBack="true"  RepeatColumns="4">
                                 <asp:ListItem Selected="True" Value="Month">&nbsp;&nbsp;By Month</asp:ListItem>
                                 <%--<asp:ListItem Value="Date">&nbsp;&nbsp;By Date</asp:ListItem>--%>
                             </asp:RadioButtonList>
                    </div>
                     </div>
            </div>
                <div class="container-fluid" style="width:100%">
                   
                   <asp:GridView ID="GridView1" CssClass="table table-bordered" runat="server" AutoGenerateColumns="false"  OnRowDataBound="OnRowDataBound" OnRowCommand="OnRowCommand" OnRowDeleting="GrdView1_RowDeleting" Width="100%" >
                     <Columns> 
                       
                        <asp:BoundField DataField="ReourceDeploymentUID"  HeaderText="DeploymentID" HeaderStyle-CssClass="hiddencol" ItemStyle-CssClass="hiddencol" >
                                                <HeaderStyle HorizontalAlign="Left" />

                        <ItemStyle CssClass="hiddencol" ></ItemStyle>
                                                </asp:BoundField>

                        <asp:TemplateField HeaderText = "Month">
                            <ItemTemplate>
                            <asp:Label ID="lblMonth" runat="server" Text='<%# Eval("s_month") %>' Visible = "false" />
                            <asp:DropDownList ID="ddlMonths" runat="server" Width="100%">
                            </asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText = "Year">
                            <ItemTemplate>
                            <asp:Label ID="lblYear" runat="server" Text='<%# Eval("s_year") %>' Visible = "false" />
                            <asp:DropDownList ID="ddlYears" runat="server" Width="100%">
                            </asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>

                          <asp:TemplateField HeaderText = "Planned">
                            <ItemTemplate>
                            <asp:Label ID="lblPlanned" runat="server" Text='<%# Eval("planned") %>' Visible = "false" />
                            <asp:TextBox ID="txtPlanned" runat="server" Width="100%">
                            </asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                          <asp:TemplateField HeaderText = "">
                            <ItemTemplate>
                               <asp:LinkButton ID="lnkdelete" runat="server" OnClientClick="return DeleteItem()"  CausesValidation="true" CommandArgument='<%# Eval("ReourceDeploymentUID") %>' CommandName="delete"><span title="x" class="fas fa-trash"></span></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    </asp:GridView>
                       
                </div>
               
                <div style="padding:15px">
                    <asp:Button ID="btnAddNew" runat="server" Text="Add New Row" CssClass="btn btn-primary" OnClick="btnAddNew_Click" Enabled="true" OnClientClick="javascript:disableButton('btnSave');" />
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-primary" OnClick="btnCancel_Click" Enabled="false" />
                    <asp:Button ID="btnSaveNew" runat="server" Text="Save New Row" CssClass="btn btn-primary" OnClick="btnSaveNew_Click" Enabled="false" />
                </div>
                </ContentTemplate>
               
                </asp:UpdatePanel>
            </div>
              <div id="dvProgressBar" style=" text-align:center; position:relative; visibility:hidden;" >
                     <img src="../_assets/images/progress.gif" width="40" alt="loading"  /> <span style="color:#006699; font-weight:bold;">Processing please wait...</span>
              </div> 
                <div class="modal-footer">
                   <asp:Button ID="btnSave" runat="server" Text="Update & Close" CssClass="btn btn-primary" OnClick="btnUpdate_Click" OnClientClick="ShowProgressBar('true')" />
                </div>
        </form>
</asp:Content>
