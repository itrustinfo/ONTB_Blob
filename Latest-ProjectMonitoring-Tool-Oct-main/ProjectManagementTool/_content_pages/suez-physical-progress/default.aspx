<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/default.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="ProjectManagementTool._content_pages.suez_physical_progress._default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="default_master_head" runat="server">
      <style>
        .Hide
            {
                display : none;
            }
    </style>
  <link rel="stylesheet" href="//code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">
  <script src="https://code.jquery.com/jquery-1.12.4.js"></script>
  <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>
   
   
    <script type="text/javascript">
       
        
            $(function () {
                $("input[id$='dtMeetingDate']").datepicker({
                    changeMonth: true,
                    changeYear: true,
                    dateFormat: 'dd/mm/yy'
                });
        });
   


          function BindEvents() {
           

              $(".showModalDocumentView").click(function (e) {
                 // alert("test");
                 jQuery.noConflict();
        e.preventDefault();
        var url = $(this).attr("href");
        $("#ModViewDocument iframe").attr("src", url);
        $("#ModViewDocument").modal("show");
            });
              
              $(".showBankGuaranteeModal").click(function (e) {
                 jQuery.noConflict();
                e.preventDefault();
                var url = $(this).attr("href");
                $("#ModAddBankGuarantee iframe").attr("src", url);
                $("#ModAddBankGuarantee").modal("show");
              });

              $(".showMobilisationAdvanceModal").click(function (e) {
                  jQuery.noConflict();
                  e.preventDefault();
                  var url = $(this).attr("href");
                  $("#ModAddMobilisationAdvance iframe").attr("src", url);
                  $("#ModAddMobilisationAdvance").modal("show");
              });        
        }

        $(document).ready(function () {
            //$('#loader').fadeOut();
             BindEvents();
            DateText();

           
        });

        function isNumberKey(txt, evt) {
      var charCode = (evt.which) ? evt.which : evt.keyCode;
      if (charCode == 46) {
        //Check if the text already contains the . character
        if (txt.value.indexOf('.') === -1) {
          return true;
        } else {
          return false;
        }
      } else {
        if (charCode > 31 &&
          (charCode < 48 || charCode > 57))
          return false;
      }
      return true;
    }
        
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="default_master_body" runat="server">
     <%--project selection dropdowns--%>

<asp:ScriptManager ID="ScriptManager1" runat="server">
</asp:ScriptManager>
<%--<div id="loader"></div>--%>
<div class="container-fluid">
    <div class="row">
        <div class="col-md-12 col-lg-12 form-group">Suez Physical Progress</div>
        <div class="col-md-6 col-lg-4 form-group">
            <label class="sr-only" for="DDLProject">Project</label>
            <div class="input-group">
                <div class="input-group-prepend">
                    <span class="input-group-text">Project</span>
                </div>
                <asp:DropDownList ID="ddlProject" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlProject_SelectedIndexChanged"></asp:DropDownList>
            </div>
        </div>
        <div class="col-md-6 col-lg-4 form-group">
            <label class="sr-only" for="DDLWorkPackage">Work Package</label>
            <div class="input-group">
                <div class="input-group-prepend">
                    <span class="input-group-text">Work Package</span>
                </div>
                <asp:DropDownList ID="ddlworkpackage" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlworkpackage_SelectedIndexChanged"></asp:DropDownList>
            </div>
        </div>
         <div class="col-md-6 col-lg-4 form-group">
            <label class="sr-only" for="DDLWorkPackage">Select Achieved Date</label>
            <div class="input-group">
                <div class="input-group-prepend">
                    <span class="input-group-text">Select Achieved Date</span>
                </div>
                <asp:TextBox ID="dtMeetingDate" runat="server" placeholder="dd/mm/yyyy" autocomplete="off" CssClass="form-control" required AutoPostBack="True" OnTextChanged="dtMeetingDate_TextChanged"></asp:TextBox>
            </div>
        </div>
    </div>

<div class="container-fluid">
  
                    <div class="row">
                        <div class="col-lg-12 col-xl-12 form-group">                          
                                <div class="card mb-4">
                                    <div class="card-body">
                                   
                                        <div class="table-responsive">
                                             <div align="right">
                                   <a id="AddData" runat="server" href="~/_modal_pages/add-treenodemaster.aspx" class="showBankGuaranteeModal"><asp:Button ID="Button2" runat="server" Text="+ Add" CssClass="btn btn-primary"></asp:Button></a>
                                 
                                             </div><br />
                                      
                                            <div id="divStatusMonth" runat="server" visible="false">
                                                 <asp:GridView ID="GrdDocStatus" runat="server" Width="100%" CssClass="table table-bordered" DataKeyNames="TaskUid" EmptyDataText="No Status Found" AutoGenerateColumns="False"  >
                                <Columns>
                                    <asp:TemplateField>
                                        <HeaderTemplate>S.No</HeaderTemplate>
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex + 1%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>Task Name</HeaderTemplate>
                                      
                                        <ItemTemplate>
                                            <asp:Label ID="lblTaskName" runat="server" Text='<%# Bind("Taskname") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Achived Value">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtTaskValue" runat="server" Text='<%# Bind("Quantity") %>' ></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                                            </div>
                                            </div>
                                        </div>
                                  </div>
                         </div>
                        </div>
              <div class="row">
                                            <div class="col-md-12 col-lg-4 form-group" id="divsubmit" runat="server">

                                                <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-primary" OnClick="btnSubmit_Click" />

                                                <br />
                                                <asp:Label ID="LblMessage" runat="server" Font-Bold="true"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                   
    
</div>

<div id="ModAddBankGuarantee" class="modal it-modal fade">
    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-lg">
        <div class="modal-content">
            <div class="modal-header">
                <h5 class="modal-title">SUEZ PHYSICAL PROGRESS UPDATE</h5>
                <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
            </div>
            <div class="modal-body">
                <iframe class="border-0 w-100" style="height: 500px;" loading="lazy"></iframe>
            </div>

        </div>
    </div>
</div>

</asp:Content>
