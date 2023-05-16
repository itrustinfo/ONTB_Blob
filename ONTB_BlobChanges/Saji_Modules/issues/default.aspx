<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/default.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="ProjectManagementTool._content_pages.issues._default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="default_master_head" runat="server">
     <style type="text/css">
        .hiddencol { display: none; }
    </style>
         <script type="text/javascript">

             //function showImgModal(s) {

             //    $("#ModIssueImage img").attr("src", s);
             //    $("#ModIssueImage img").attr("width", "100%")
             //    $("#ModIssueImage img").attr("height", "auto")
                                  
             //    $("#ModIssueImage").modal('show');
             //}

             function showImgModal(s) {

                 $("#ModIssueImage img").attr("src", s);
                 $("#ModIssueImage img").attr("width", "100%")
                 $("#ModIssueImage img").attr("height", "700px")

                 $("#ModIssueImage").modal('show');
             }


             //function showPdfModal(s) {

             //    $("#ModIssuePdf iframe").attr("src", s);
             //    $("#ModIssuePdf iframe").attr("width", "100%")
             //    $("#ModIssuePdf iframe").attr("height", "700px")

             //    $("#ModIssuePdf").modal('show');
             //}

             function showPdfModal(s) {

                 $("#ModIssuePdf iframe").attr("src", s);
                 $("#ModIssuePdf iframe").attr("width", "100%")
                 $("#ModIssuePdf iframe").attr("height", "700px")

                 $("#ModIssuePdf").modal('show');
             }


             function BindEvents() {
                 $(".showIssueModal").click(function(e) {
                     e.preventDefault();                 
                var url = $(this).attr("href");
                $("#ModAddIssue iframe").attr("src", url);
                $("#ModAddIssue").modal("show");
                 });

                 $(".EditIssues").click(function(e) {
                     e.preventDefault();                 
                var url = $(this).attr("href");
                $("#ModEditIssue iframe").attr("src", url);
                $("#ModEditIssue").modal("show");
                 });

                 $(".EditAssignUser").click(function (e) {
                     e.preventDefault();
                     var url = $(this).attr("href");
                     $("#ModAssignUser iframe").attr("src", url);
                     $("#ModAssignUser").modal("show");
                 });

                 $(".UpdateIssueStatus").click(function(e) {
                    e.preventDefault();                 
                var url = $(this).attr("href");
                $("#ModIssueStatus iframe").attr("src", url);
                $("#ModIssueStatus").modal("show");
                 });

                 $(".IssueImagePreview").click(function (e) {
                     e.preventDefault();
                    // $("#ModIssuePreview iframe").html("");

                     var url = $(this).attr("href");

                     $("#ModIssuePreview iframe").attr("src", url);
                     $("#ModIssuePreview").modal("show");
                 });
             }
            $(document).ready(function () {
                BindEvents();
               });
         </script>

    <script type="text/javascript">
        function DeleteItem() {
            if (confirm("Are you sure you want to delete ...?")) {
                return true;
            }
            return false;
        }
 
        function PrintDiv(id) {
            var data = document.getElementById(id).innerHTML;
            var myWindow = window.open('', 'my div', 'height=600,width=800');
            myWindow.document.write('<html><head><title></title>');
            /*optional stylesheet*/ myWindow.document.write('<link rel="stylesheet" href="Css/style.css" type="text/css" />');
            myWindow.document.write('</head><body >');
            myWindow.document.write(data);
            myWindow.document.write('</body></html>');
            myWindow.document.close(); // necessary for IE >= 10

            myWindow.onload = function () { // necessary if the div contain images

                myWindow.focus(); // necessary for IE >= 10
                myWindow.print();
                myWindow.close();
            };
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="default_master_body" runat="server">
     <asp:ScriptManager ID="ScriptManager1" runat="server">
   </asp:ScriptManager>
    <%--project selection dropdowns--%>
        <div class="container-fluid">
            <div class="row">
                <div class="col-md-12 col-lg-8 form-group">Issues</div>
                <div class="col-md-6 col-lg-4 form-group">
                    <label class="sr-only" for="DDLProject">Project</label>
                    <div class="input-group">
                        <div class="input-group-prepend">
                            <span class="input-group-text">Project</span>
                        </div>
                        <asp:DropDownList ID="DDlProject" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="DDlProject_SelectedIndexChanged"></asp:DropDownList>
                    </div>
                </div>
            </div>
        </div>
    <div class="container-fluid">
<%--        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>--%>
                    <div class="row">
                        <div class="col-lg-6 col-xl-2 form-group">
                            <div class="card h-100" style="max-height:700px; overflow-y:auto;">
                                <div class="card-body">
                                    <h6 class="card-title text-muted text-uppercase font-weight-bold">WorkPackage Directory</h6>
                                    <div class="form-group">
                                        <label class="sr-only" for="TxtSearchDocuments">Search</label>
                                        <div class="input-group">
                                            <input id="TxtSearchDocuments" class="form-control" type="text" placeholder="Activity name" />
                                            <div class="input-group-append">
                                                <asp:Button ID="BtnSearchDocuments" CssClass="btn btn-primary" Text="Search" runat="server" />
                                            </div>
                                        </div>
                                    </div>
                                    <asp:TreeView runat="server" CssClass="it_tree_view" ID="TreeView1" ImageSet="XPFileExplorer" OnSelectedNodeChanged="TreeView1_SelectedNodeChanged" NodeIndent="15" EnableTheming="True" NodeWrap="True">                               
                                        <NodeStyle CssClass="it_tree_view__node" NodeSpacing="2px" />
                                        <ParentNodeStyle Font-Bold="False" />
                                        <SelectedNodeStyle CssClass="it_tree_view__node__selected" HorizontalPadding="4px" VerticalPadding="2px" />
                                    </asp:TreeView>    
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-6 col-xl-10 form-group">
                            <div class="row" style="text-align:center;">
                                 <div class="col-md-6 col-xl-2 mb-2">
                                    <div class="card">
                                        <div class="card-body" id="divTotal" runat="server">
                                            <b style="font-size:large;"><a href="/_content_pages/issues?status=">Total No. of Issues</a></b>
                                            <br />
                                            <b style="color:#444444; font-size:xx-large;">
                                                <asp:Label ID="LblTotalIssues" runat="server">0</asp:Label>
                                            </b>
                                            </div>
                                        </div>
                                    </div>
                                <div class="col-md-6 col-xl-2 mb-2">
                                    <div class="card">
                                        <div class="card-body" id="divOpen" runat="server">
                                            <b style="font-size:large;"><a href="/_content_pages/issues?status=Open">Open Issues </a></b>
                                            <br />
                                            <b style="color:#3366CC; font-size:xx-large;">
                                                <asp:Label ID="LblOpenIssues" runat="server">0</asp:Label>
                                            </b>                
                                            </div>
                                        </div>
                                    </div>
                                 <div class="col-md-6 col-xl-2 mb-2">
                                    <div class="card">
                                            <div class="card-body" id="divProgress" runat="server">
                                                <b style="font-size:large;"><a href="/_content_pages/issues?status=In-Progress">In-Progress Issues </a></b>
                                                <br />
                                                <b style="color:#FF9900; font-size:xx-large;">
                                                    <asp:Label ID="LblInProgressIssues" runat="server">0</asp:Label>
                                                </b>                
                                            </div>
                                        </div>
                                    </div>
                                <div class="col-md-6 col-xl-2 mb-2">
                                    <div class="card">
                                        <div class="card-body" id="divClosed" runat="server">
                                            <b style="font-size:large;"><a href="/_content_pages/issues?status=Close">Closed Issues</a></b>
                                            <br />
                                            <b style="color:#109618; font-size:xx-large;">
                                                <asp:Label ID="LblClosedIssues" runat="server">0</asp:Label>
                                            </b>    
                                        </div>
                                    </div>
                                </div>

                                <div class="col-md-6 col-xl-2 mb-2">
                                    <div class="card">
                                        <div class="card-body" id="divRejected" runat="server">
                                            <b style="font-size:large;"><a href="/_content_pages/issues?status=Rejected">Rejected Issues</a></b>
                                            <br />
                                            <b style="color:#DC3912; font-size:xx-large;">
                                                <asp:Label ID="LblRejectedIssues" runat="server">0</asp:Label>
                                            </b>    
                                         </div>
                                     </div>
                                 </div>
                            </div>
                            <div class="card mb-4">
                                    <div class="card-body">
                                        <div class="card-title">
                                            <div class="d-flex justify-content-between">
                                                 <div class="col-md-6 col-lg-4 form-group" style="margin-left:0px;padding-left:0px">
                                                   <%-- <label class="sr-only" for="DDLProject">Project</label>--%>
                                                    <div class="input-group">
                                                        <div class="input-group-prepend" style="margin-left:0px">
                                                            <span class="input-group-text">Reporting User</span>
                                                        </div>
                                                        <asp:DropDownList ID="DDLUser" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="DDLUser_SelectedIndexChanged"></asp:DropDownList>
                                                    </div>
                                                </div>
                                                <h6 class="text-muted">
                                                    <asp:Label ID="ActivityHeading" CssClass="text-uppercase font-weight-bold" runat="server" style="text-align:center;padding:10px;font-weight:900 !important;color:maroon" />
                                                    <%--<asp:Label Text="Foo bar" CssClass="pl-1" runat="server" />--%>
                                                </h6>
                                                <div>
                                                   <a id="AddIssues" runat="server" href="/_modal_pages/add-issues.aspx" class="showIssueModal"><asp:Button ID="Button2" runat="server" Text="+ Add Issues" CssClass="btn btn-primary"></asp:Button></a>
                                                </div>
                                            </div>
                                        </div>
                                        
                                                                                 
                                                    <div class="table-responsive">
                                                        <asp:Label ID="lblTotal" runat="server" Text="0" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                                <asp:GridView ID="GrdIssues" runat="server"  AutoGenerateColumns="False" EmptyDataText="No Data Found" Width="100%" CssClass="table table-bordered" OnRowDataBound="GrdIssues_RowDataBound" OnRowCommand="GrdIssues_RowCommand" OnRowDeleting="GrdIssues_RowDeleting" OnPageIndexChanging="OnPageIndexChanging" AllowPaging="true" PageSize="10" >
                                                    <Columns>
                                                        <%--<asp:BoundField DataField="Issue_Uid" ItemStyle-CssClass="hiddencol"  HeaderStyle-CssClass="hiddencol" HeaderText="LinkToReviewFile" >
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>--%>
                                                        <asp:TemplateField HeaderText="S.No" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <%# Container.DataItemIndex + 1 %>
                                                            </ItemTemplate>

                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="Issue_Description" HeaderText="Issue Description&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" HeaderStyle-Width="40%" ItemStyle-Width="40%" >
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                           
<ItemStyle Width="30%"></ItemStyle>
                                                           
                                                        </asp:BoundField>
                                                          <asp:BoundField DataField="Issue_Remarks" HeaderText="Remarks&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" HeaderStyle-Width="40%" ItemStyle-Width="40%" >
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                           
<ItemStyle Width="20%"></ItemStyle>
                                                           
                                                        </asp:BoundField>
                                                        <%-- <asp:BoundField DataField="TaskUID" HeaderText="Task">
                                                <HeaderStyle HorizontalAlign="Left" />
                                                </asp:BoundField>--%>
                                                <asp:BoundField DataField="Issued_User" HeaderText="Reporting User" >
                                                <HeaderStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Issue_Date" HeaderText="Date of Issue Occurrence" DataFormatString="{0:dd/MM/yyyy}">
                                                <HeaderStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Actual_Closer_Date" HeaderText="Approving Date"  DataFormatString="{0:dd/MM/yyyy}">
                                                <HeaderStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                
                                              
                                                <asp:BoundField DataField="Issue_Status" HeaderText="Status" >
                                                <HeaderStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <%-- <asp:BoundField DataField="Issue_Remarks" HeaderText="Issue Remarks" >
                                                <HeaderStyle HorizontalAlign="Left" />
                                                </asp:BoundField>--%>
                                                        
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <a href="/_modal_pages/View_UpdateIssueStatus.aspx?Issue_Uid=<%#Eval("Issue_Uid")%>"  class="UpdateIssueStatus">View/Update Status</a>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <a href="/_modal_pages/preview-issue-documents.aspx?IssueUid=<%#Eval("Issue_Uid")%>"  class="IssueImagePreview">All Images Preview</a>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                      
                                                        <asp:TemplateField HeaderText="Attachment/s">
                                                                 <ItemTemplate>
                                                                         <%--<asp:LinkButton ID="lnkdown" runat="server" CommandArgument='<%#Eval("Issue_Uid")%>' CausesValidation="false" CommandName="download">Download</asp:LinkButton>--%>
                                                                 </ItemTemplate>
                                                        </asp:TemplateField>   
                                                        <asp:BoundField DataField="Issue_Document" ItemStyle-CssClass="hiddencol"  HeaderStyle-CssClass="hiddencol" HeaderText="IssueDocument" >
                                                    <HeaderStyle HorizontalAlign="Left" />

                                                    <ItemStyle CssClass="hiddencol"></ItemStyle>

                                                    </asp:BoundField>
                                                          <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <a id="EditIssues" href="/_modal_pages/add-issues.aspx?Issue_Uid=<%#Eval("Issue_Uid")%>&PrjID=<%#Eval("ProjectUID")%>"  class="EditIssues"><span title="Edit" class="fas fa-edit"></span></a>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <a id="EditAssignUser" href="/_modal_pages/add-issues-users.aspx?Issue_Uid=<%#Eval("Issue_Uid")%>&PrjID=<%#Eval("ProjectUID")%>"  class="EditAssignUser"><span title="Assign User" class="fas fa-users"></span></a>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                         <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lnkactualdelete" runat="server" OnClientClick="return DeleteItem()" CausesValidation="false" CommandArgument='<%#Eval("Issue_Uid")%>' CommandName="delete"><span title="Delete" class="fas fa-trash"></span></asp:LinkButton>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                         <asp:BoundField DataField="Issue_Uid" HeaderText="Issue_Uid" HeaderStyle-CssClass="hiddencol" ItemStyle-CssClass="hiddencol" >
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemStyle CssClass="hiddencol"></ItemStyle>
                                                         </asp:BoundField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <a id="printDocs" target="_blank"  href="/_content_pages/doctobrowser/default.aspx?Issue_Uid=<%#Eval("Issue_Uid")%>&PrjID=<%#Eval("ProjectUID")%>"  hidden="hidden" ><span title="print" class="fas fa-print"></span></a>
                                                                <asp:LinkButton ID="lnkdown" runat="server" CommandArgument='<%#Eval("Issue_Uid")%>' CausesValidation="false" CommandName="download"><span title="download all" class="fas fa-download"></span></asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <PagerSettings Position="TopAndBottom" />
                                                </asp:GridView>
                                                    </div>
                                     
                                        <div id="printreport" runat="server" visible="false" style="text-align:right;padding-top:10px">
                                         <input type="button" value="Print Report" class="btn btn-primary" onclick="PrintDiv('myDiv')" />
                                        </div>
                                        </div>
                                    </div>

                           
                            </div>
                     </div>
                        <%--</ContentTemplate>  
                        <Triggers>
                         <asp:AsyncPostBackTrigger ControlID="TreeView1" EventName="SelectedNodeChanged" />
                            <asp:PostBackTrigger ControlID="AddIssues" />
                         </Triggers>
                 </asp:UpdatePanel>--%>

        <div class="row" id="myDiv" style="background-color:#ffffff; display:none;">
            <div class="card">
                <div class="card-body">
                    <b style="font-size:x-large;">PROJECT MONITORING TOOL</b> <br />
                    <b style="font-size:large;"> <%= DateTime.Now.ToString("dd MMM yyyy") %> </b>
                    </div>
                </div>

             <div class="col-lg-12 grid-margin stretch-card">
              <div class="card">
                <div class="card-body"><h1 style="text-align:center; font-weight:bold; color:#006699;">ISSUES</h1></div></div>
                 </div>
                     <div class="col-lg-12 grid-margin stretch-card">
                          <div class="card">
                              <asp:GridView ID="GrdPrint" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered" Width="100%"  OnRowDataBound="GrdPrint_RowDataBound">
                                <Columns>
                                    <asp:TemplateField HeaderText="S.No" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <%# Container.DataItemIndex + 1 %>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Issue_Description" HeaderText="Issue Description">
                            <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                                   
                            <asp:BoundField DataField="Issued_User" HeaderText="Reporting User" >
                            <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Issue_Date" HeaderText="Date of Issue Occurrence" DataFormatString="{0:dd/MM/yyyy}">
                            <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                          
                            <asp:BoundField DataField="Actual_Closer_Date" HeaderText="Actual Closer Date"  DataFormatString="{0:dd/MM/yyyy}">
                            <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Issue_Status" HeaderText="Status" >
                            <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                             <asp:BoundField DataField="Issue_Remarks" HeaderText="Issue Remarks" >
                            <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                           <%-- <asp:BoundField DataField="Issue_Uid" HeaderText="Issue ID" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" >
                            <HeaderStyle HorizontalAlign="Left" />
                             <ItemStyle CssClass="hiddencol"></ItemStyle>
                            </asp:BoundField>--%>
<%--<asp:TemplateField HeaderText="Uploaded Documents" ItemStyle-HorizontalAlign="Center">
               <ItemTemplate>
                  <asp:Panel ID="pnlUploadedDocs" runat="server" Visible="true" Style="position: relative">
                    <asp:GridView ID="gvUploadedDocs" runat="server" AutoGenerateColumns="false" 
                        AllowPaging="false" CssClass="ChildGrid"
                        DataKeyNames="doc_id">
                        <Columns>
                            <asp:BoundField ItemStyle-Width="150px" DataField="doc_name" HeaderText="File Name" />
                        </Columns>
                    </asp:GridView>
                </asp:Panel>
            </ItemTemplate>
        </asp:TemplateField>
                                  <asp:TemplateField HeaderText="Issue Status Details" ItemStyle-HorizontalAlign="Center">
               <ItemTemplate>
                  <asp:Panel ID="pnlIssueRemarks" runat="server" Visible="true" Style="position: relative">
                    <asp:GridView ID="gvIssueRemarks" runat="server" AutoGenerateColumns="false" 
                        AllowPaging="false" CssClass="ChildGrid"
                        DataKeyNames="IssueRemarksUID">
                        <Columns>
                            <asp:BoundField ItemStyle-Width="150px" DataField="Issue_Status" HeaderText="Status" />
                            <asp:BoundField ItemStyle-Width="150px" DataField="Issue_Remarks" HeaderText="Remarks" />
                            <asp:BoundField ItemStyle-Width="150px" DataField="IssueRemark_Date" HeaderText="Date" />
                            <asp:BoundField ItemStyle-Width="150px" DataField="IssueRemarksUID" HeaderText="ID" />
                            <asp:TemplateField HeaderText="Uploaded Documents" ItemStyle-HorizontalAlign="Center">
                               <ItemTemplate>
                                  <asp:Panel ID="pnlUploadedRemDocs" runat="server" Visible="true" Style="position: relative">
                                    <asp:GridView ID="gvUploadedRemDocs" runat="server" AutoGenerateColumns="false" 
                                        AllowPaging="false" CssClass="ChildGrid"
                                        DataKeyNames="uploaded_doc_id">
                                        <Columns>
                                            <asp:BoundField ItemStyle-Width="150px" DataField="doc_name" HeaderText="File Name" />
                                        </Columns>
                                    </asp:GridView>
                                </asp:Panel>
                            </ItemTemplate>
        </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </asp:Panel>
            </ItemTemplate>
        </asp:TemplateField>--%>
                                </Columns>                        
                            </asp:GridView>
                          </div>
                         </div>
                <br />    
                 </div>


        <%--<asp:GridView ID="gvCustomers" runat="server" AutoGenerateColumns="false" CssClass="Grid" DataKeyNames="CustomerID">
        <Columns>
            <asp:TemplateField>
               <ItemTemplate>
                <asp:ImageButton ID="imgOrdersShow" runat="server" OnClick="Show_Hide_OrdersGrid" ImageUrl="~/images/plus.png"
                    CommandArgument="Show" />
                <asp:Panel ID="pnlOrders" runat="server" Visible="false" Style="position: relative">
                    <asp:GridView ID="gvOrders" runat="server" AutoGenerateColumns="false" PageSize="5"
                        AllowPaging="true" OnPageIndexChanging="OnOrdersGrid_PageIndexChanging" CssClass="ChildGrid"
                        DataKeyNames="OrderId">
                        <Columns>
                            <asp:BoundField ItemStyle-Width="150px" DataField="OrderId" HeaderText="Order Id" />
                            <asp:BoundField ItemStyle-Width="150px" DataField="OrderDate" HeaderText="Date" />
                        </Columns>
                    </asp:GridView>
                </asp:Panel>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField ItemStyle-Width="150px" DataField="ContactName" HeaderText="Contact Name" />
        <asp:BoundField ItemStyle-Width="150px" DataField="City" HeaderText="City" />
    </Columns>
        </asp:GridView>--%>
      </div>

    <%--add resource modal--%>
    <div id="ModAddIssue" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-xl">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Add Issue</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height: 460px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>

    <%--adEditd resource modal--%>
    <div id="ModEditIssue" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-xl">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Edit Issue</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height: 460px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>
    <div id="ModAssignUser" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-xl">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Assign Users</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height: 460px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>

    

    <%--Add Issue status resource modal--%>
    <div id="ModIssueStatus" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-xl">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">View Issue Status</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button" onclick="javascript:closepopup();"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:750px;width:750px" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>
     
     <%--Add Issue status image modal--%>
    <div id="ModIssueImage" class="modal it-modal fade">
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
    </div>

     <%--Add Issue status image modal--%>
    <div id="ModIssuePdf" class="modal it-modal fade">
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
    </div>

     <%--Add Issue status resource modal--%>
    <div id="ModIssuePreview" class="modal it-modal-xl fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog modal-xl">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Preview</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button" onclick="javascript:closepopup();" ><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:750px;width:750px" loading="lazy"></iframe>
			    </div>
               <div class="modal-footer" style="padding:5px;background-color:black">
                    <div class="row" style="padding:10px;background-color:black;text-align:center"><button type="button" class="btn btn-secondary" data-dismiss="modal" style="color:white">Close</button></div>
              </div>
		    </div>
	    </div>
    </div>

</asp:Content>
