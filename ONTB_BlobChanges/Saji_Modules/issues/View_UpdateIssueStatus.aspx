<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/modal.Master" AutoEventWireup="true" CodeBehind="View_UpdateIssueStatus.aspx.cs" Inherits="ProjectManagementTool._modal_pages.View_UpdateIssueStatus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="modal_master_head" runat="server">
     <script type="text/javascript">
        function DeleteItem() {
            if (confirm("Are you sure you want to delete ...?")) {
                return true;
            }
                return false;
            }
    </script>

    <style type="text/css">
        .hiddencol { display: none; }
    </style>
    <script type="text/javascript">

        //function showImgModal(s) {

        //    alert(s);
        //    e.preventDefault();
        //    jQuery.noConflict();
        //    $("#ModIssueStatusImage img").attr("src", s);
        //    $("#ModIssueStatusImage img").attr("width", "100%")
        //    $("#ModIssueStatusImage img").attr("height", "auto")
        //    $('#ModIssueStatusImage').modal({ backdrop: 'static', keyboard: false })
        //    $("#ModIssueStatusImage").modal('show');
        //}

        //function showPdfModal(s) {

        //    alert('reached here');
        //    e.preventDefault();
        //    jQuery.noConflict();
        //    $("#ModIssueStatusPdf iframe").attr("src", s);
        //    $("#ModIssueStatusPdf iframe").attr("width", "100%")
        //    $("#ModIssueStatusPdf iframe").attr("height", "auto")
        //    $('#ModIssueStatusPdf').modal({ backdrop: 'static', keyboard: false })
        //    $("#ModIssueStatusPdf").modal('show');
        //}

        $(document).ready(function () {
            $(".showStatusModal").click(function (e) {
                e.preventDefault();
                jQuery.noConflict();
                var url = $(this).attr("href");
                $("#ModAddIssueStatus iframe").attr("src", url);
                $('#ModAddIssueStatus').modal({ backdrop: 'static', keyboard: false })
                $("#ModAddIssueStatus").modal("show");
            });

            $(".EditStatusModal").click(function (e) {
                e.preventDefault();
                jQuery.noConflict();
                var url = $(this).attr("href");
                $("#ModEditIssueStatus iframe").attr("src", url);
                $('#ModEditIssueStatus').modal({ backdrop: 'static', keyboard: false })
                $("#ModEditIssueStatus").modal("show");
            });

            //$(".IssueStatusImagePreview").click(function (e) {

            //    e.preventDefault();
            //    jQuery.noConflict();

            //    $("#ModIssuePreview iframe").html("");

            //    var url = $(this).attr("href");

            //    $("#ModIssuePreview iframe").attr("src", url);
            //    $("#ModIssuePreview").modal("show");
            //});

            $(".IssueStatusImagePreview").click(function (e) {

                e.preventDefault();
                jQuery.noConflict();

                $("#ModIssuePreview iframe").html("");

                var url = $(this).attr("href");

                $("#ModIssuePreview iframe").attr("src", url);
                $("#ModIssuePreview iframe").attr("width", "100%")
                $("#ModIssuePreview iframe").attr("height", "600px")
                $("#ModIssuePreview").modal("show");
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="modal_master_body" runat="server">
    <form id="frmAddDocumentModal" runat="server">
    <div class="container-fluid" style="overflow-y:auto; min-height:83vh;">
        <div class="row">
           <div class="col-sm-6">
                <div class="form-group">
                       <div class="form-group" >
                        <label class="lblCss" for="lblWorkPackage">Issue</label>&nbsp;&nbsp;<b>:</b>&nbsp;&nbsp;
                         <asp:Label ID="LblIssue" class="lblCss" Font-Bold="true" runat="server"></asp:Label>
                    </div>
                 </div>
           </div>
            <div class="col-sm-6">
                 
                </div>
        </div>
        <div class="row">
            <div class="col-sm-12">
                <div class="table-responsive">
                        <asp:GridView ID="GrdIssueStatus" runat="server" Width="100%" PageSize="10" AllowPaging="true" CssClass="table table-bordered" DataKeyNames="IssueRemarksUID" EmptyDataText="No Status Found" AutoGenerateColumns="false" OnRowDataBound="GrdIssueStatus_RowDataBound" OnRowCommand="GrdIssueStatus_RowCommand" OnRowDeleting="GrdIssueStatus_RowDeleting" OnPageIndexChanging="GrdIssueStatus_PageIndexChanging">
                        <Columns>
                            <asp:BoundField DataField="Issue_Status" HeaderText="Status" >
                            <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Issue_Remarks" HeaderText="Remarks">
                            <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="IssueRemark_Date" HeaderText="Date" DataFormatString="{0:dd MMM yyyy}">
                            <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Issue_Document" ItemStyle-CssClass="hiddencol"  HeaderStyle-CssClass="hiddencol" HeaderText="LinkToReviewFile" >
                            <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                             <asp:BoundField DataField="IssueRemarksUID" ItemStyle-CssClass="hiddencol"  HeaderStyle-CssClass="hiddencol" HeaderText="LinkToReviewFile" >
                            <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Attachment/s">
                                <ItemTemplate>
                                      <%--<asp:LinkButton ID="lnkdown" runat="server" CommandArgument='<%#Eval("IssueRemarksUID")%>' CausesValidation="false" CommandName="download">Download</asp:LinkButton>--%>
                               </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField>
                                 <ItemTemplate>
                                    <%-- <span title="Preview" class="fas fa-eye">--%>
                                       <a id="Preview" href="/_modal_pages/preview-issue-status-documents.aspx?IssueRemarksUID=<%#Eval("IssueRemarksUID")%>" class="IssueStatusImagePreview">All Image Preview</span></a> 
                                 </ItemTemplate>
                             </asp:TemplateField>

                             <asp:TemplateField>
                                 <ItemTemplate>
                                       <a id="Edit" href="/_modal_pages/add-issuestatus.aspx?IssueRemarksUID=<%#Eval("IssueRemarksUID")%>&Issue_Uid=<%#Eval("Issue_Uid")%>" class="EditStatusModal"><span title="Edit" class="fas fa-edit"></span></a> 
                                 </ItemTemplate>
                             </asp:TemplateField>
                             <asp:TemplateField>
                                  <ItemTemplate>
                                        <asp:LinkButton ID="lnkdelete" runat="server" CausesValidation="false" OnClientClick="return DeleteItem()" CommandArgument='<%#Eval("IssueRemarksUID")%>' CommandName="delete"><span title="Delete" class="fas fa-trash"></span></asp:LinkButton>
                                  </ItemTemplate>
                             </asp:TemplateField>          
                        </Columns>
                    </asp:GridView>
                           </div>
                </div>
            </div>
    </div>
    <div class="modal-footer">
            <a id="AddStatus" runat="server" href="/_modal_pages/add-issuestatus.aspx" class="showStatusModal"><asp:Button ID="btnaddstatus" runat="server" Height="35px" Width="150px" Text="+ Add Status" CssClass="btn btn-primary"></asp:Button></a>
                </div>

    <%--View Issue status modal--%>
    <div id="ModAddIssueStatus" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-lg">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Add Status</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:280px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>
       <%--Add Issue status resource modal--%>
    <div id="ModIssuePreview" class="modal it-modal-xl fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog modal-xl">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Preview</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button" ><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:600px;width:700px" loading="lazy"></iframe>
			    </div>
               <div class="modal-footer" style="padding:5px;background-color:black">
                    <div class="row" style="padding:10px;background-color:black;text-align:center"><button type="button" class="btn btn-secondary" data-dismiss="modal" style="color:white">Close</button></div>
              </div>
		    </div>
	    </div>
    </div>
         <div id="ModEditIssueStatus" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-lg">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Edit Status</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:280px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>

         <%--<div id="ModIssueStatusImage" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-xl">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">View Issue Image</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button" ><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <img class="embed-responsive" src="" />
			    </div>
              <div class="modal-footer" style="padding:5px;background-color:black">
               <div style="padding:10px;background-color:black;text-align:center"><button type="button" class="btn btn-secondary" data-dismiss="modal" style="color:white">OK</button></div>
              </div>
		    </div>
	    </div>
    </div>--%>

      <%--  <div id="ModIssueStatusPdf" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-xl">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">View Issue pdf</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button" ><span aria-hidden="true">&times;</span></button>
			    </div>
			   <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:750px;" loading="lazy"></iframe>
			    </div>
              <div class="modal-footer" style="padding:5px;background-color:black">
               <div style="padding:10px;background-color:black;text-align:center"><button type="button" class="btn btn-secondary" data-dismiss="modal" style="color:white">OK</button></div>
              </div>
		    </div>
	    </div>
    </div>--%>
    </form>
</asp:Content>
