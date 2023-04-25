<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/default.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="ProjectManagementTool._content_pages.document_correspondence._default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="default_master_head" runat="server">
      <link rel="stylesheet" href="//code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">
 <script src="https://code.jquery.com/jquery-1.12.4.js"></script>
 <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>
    
    <style type="text/css">
         .hideItem {
         display:none;
     }
  
    </style>
    <script type="text/javascript">
        function BindEvents() {

            $(".showModalPreviewDocument").click(function (e) {
                jQuery.noConflict();
                e.preventDefault();
              
                var url = $(this).attr("href");
                $("#ModDocumentPreview iframe").attr("src", url);
                $("#ModDocumentPreview").modal("show");
            });

            $(".showModalDocumentMail").click(function (e) {
                jQuery.noConflict();
                e.preventDefault();
                var url = $(this).attr("href");
                $("#ModDocumentMail iframe").attr("src", url);
                $("#ModDocumentMail").modal("show");
            });

            $(".showModalDocumentView").click(function (e) {
                jQuery.noConflict();
                e.preventDefault();
                var url = $(this).attr("href");
                $("#ModViewDocument iframe").attr("src", url);
                $("#ModViewDocument").modal("show");
            });
        }
   
        $(document).ready(function () {
            BindEvents();
        });
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="default_master_body" runat="server">
    <div class="container-fluid">
            <div class="row">
                <div class="col-md-12 col-lg-4 form-group">Documents-ONTB/BWSSB Correspondence</div>
                <div class="col-md-6 col-lg-4 form-group">
                 </div>
                <div class="col-md-6 col-lg-4 form-group text-right">
                    <asp:Button ID="btnback" runat="server" Text="Back" CssClass="btn btn-primary" PostBackUrl="/_content_pages/dashboard/"></asp:Button>
                </div>
            </div>
        </div>
    <div class="container-fluid">
        
        <div class="row">
            <div class="col-lg-12 col-xl-12 form-group">
                <div class="card mb-4">
                    <div class="card-body">
                        <div class="card-title">
                        <div class="d-flex justify-content-between">
                              <%--<h6 class="text-muted">
                                   <asp:Label ID="LblDocumentHeading" CssClass="text-uppercase font-weight-bold" runat="server" />
                                       <%--<asp:Label Text="Foo bar" CssClass="pl-1" runat="server" />--%>
                              <%-- </h6>--%>
                                <div>
                                   
                                </div>
                          </div>
                            </div>
                        <div class="row">
                <div class="col-sm-12 mb-4" runat="server">
                     <div class="card">
                        <div class="card-body" >   
                            <div class="table-responsive">
                            <table style="width:100%;">
                                <tr>
                                    <th style="width:20%"></th>
                                   
                                    <th style="width:20%">
                                        <h6 class="card-title text-muted text-uppercase font-weight-bold">Flow</h6>
                                    </th>
                                     <th style="width:20%">
                                        <h6 class="card-title text-muted text-uppercase font-weight-bold">Current Status</h6>
                                    </th>
                                     <th style="width:20%">
                                         <h6 class="card-title text-muted text-uppercase font-weight-bold">ONTB Ref.No</h6>
                                    </th>
                                    <th style="width:7%"></th>
                                     <th style="width:7%"></th>
                                </tr>
                             <tr>
                                 <td></td>
                                
                                
                                 <td style="width:20%">
                                     <asp:DropDownList ID="DDLFlow" runat="server" CssClass="form-control" OnSelectedIndexChanged="DDLFlow_SelectedIndexChanged" AutoPostBack="true" ></asp:DropDownList>
                                 </td>
                                  <td>
                                    <asp:DropDownList ID="DDLStatus" runat="server" CssClass="form-control" ></asp:DropDownList>
                                 </td>
                                  <td>
                                      <asp:TextBox ID="TxtOntbNo" runat="server" CssClass="form-control" ></asp:TextBox>
                                 </td>
                                 <td style="width:7%;">
                                     <asp:Button ID="btnsubmitfilter" runat="server" CssClass="btn btn-primary" Text="Submit" OnClick="btnsubmitfilter_Click"   />
                                 </td>
                                 <td style="width:7%;">
                                     <asp:Button ID="btnClear" runat="server" CssClass="btn btn-primary" Text="Clear"  OnClick ="btnClear_Click" />
                                 </td>

                             </tr>
                        </table>
                                </div>
                            </div>
                         </div>
                    </div>
                </div>
                        <div class="table-responsive">
                         <asp:GridView ID="GrdDocuments" EmptyDataText="No Documents Found." runat="server" Width="100%" AutoGenerateColumns="false" 
                                            AllowPaging="true" PageSize="15" DataKeyNames="ActualDocumentUID" CssClass="table table-bordered" OnRowDataBound="GrdDocuments_RowDataBound" OnPageIndexChanging="GrdDocuments_PageIndexChanging" OnRowCommand="GrdDocuments_RowCommand">
                                       <Columns>
                                          
                                           <asp:TemplateField HeaderText="Submittal Name">
                                            <ItemTemplate>
                                                <%#GetSubmittalName(Eval("DocumentUID").ToString())%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                           <asp:TemplateField HeaderText="Document Name" ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                       <img width="24" src='../../_assets/images/<%#GetDocumentTypeIcon(Eval("ActualDocument_Type").ToString())%>' alt='<%#Eval("ActualDocument_Type")%>' />  &nbsp;&nbsp;
                                                       <%--<asp:LinkButton ID="lnkviewpdf" class='<%#Convert.ToString(Eval("ActualDocument_Type")) == ".pdf" ? "view" : "hideItem" %>' runat="server" CausesValidation="false" CommandArgument='<%#Eval("ActualDocument_Path")%>' CommandName="ViewDoc"><%#Eval("ActualDocument_Name")%></asp:LinkButton>--%>
                                                       <a id="ShowFile" href='/_modal_pages/preview-document.aspx?ActualDocumentUID=<%#Eval("ActualDocumentUID")%>&previewpath=<%#Eval("ActualDocument_Path").ToString().Replace('&','!')%>' class='<%#Convert.ToString(Eval("ActualDocument_Type")) == ".pdf" ? "showModalPreviewDocument" : Convert.ToString(Eval("ActualDocument_Type")) == ".xls" ? "showModalPreviewDocument" : Convert.ToString(Eval("ActualDocument_Type")) == ".xlsx" ? "showModalPreviewDocument" : Convert.ToString(Eval("ActualDocument_Type")) == ".docx" ? "showModalPreviewDocument" : Convert.ToString(Eval("ActualDocument_Type")) == ".doc" ? "showModalPreviewDocument" : Convert.ToString(Eval("ActualDocument_Type")) == ".PDF" ? "showModalPreviewDocument" : "hideItem" %>'><%#Eval("ActualDocument_Name")%></a>
                                                       <asp:Label ID="lblName" runat="server" Text='<%#Eval("ActualDocument_Name")%>' Visible='<%#Convert.ToString(Eval("ActualDocument_Type")) == ".pdf" ? false : Convert.ToString(Eval("ActualDocument_Type")) == ".xls" ? false : Convert.ToString(Eval("ActualDocument_Type")) == ".xlsx" ? false : Convert.ToString(Eval("ActualDocument_Type")) == ".docx" ? false : Convert.ToString(Eval("ActualDocument_Type")) == ".doc" ? false : Convert.ToString(Eval("ActualDocument_Type")) == ".PDF" ? false : true %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                           <asp:TemplateField HeaderText="Document Type">
                                            <ItemTemplate>
                                                <%#GetDocumentName(Eval("ActualDocument_Type").ToString())%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                           <asp:BoundField DataField="ActualDocument_CurrentStatus" DataFormatString="{0:dd MMM yyyy}" HeaderText="Current Status">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                             <asp:BoundField DataField="ActualDocument_CreatedDate" HeaderText="Created Date">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                           <asp:TemplateField>
                                            <ItemTemplate>              
                                                      <asp:LinkButton ID="LnkDownloadnew" runat="server" CausesValidation="false" CommandArgument='<%#Eval("ActualDocumentUID")%>' CommandName="download">Download</asp:LinkButton>               
                                            </ItemTemplate>
                                            </asp:TemplateField>
                                       </Columns>
                                       </asp:GridView>
                         
                         <asp:GridView ID="GrdActualSubmittedDocuments" EmptyDataText="No Documents Found." runat="server" Width="100%" AutoGenerateColumns="false" CellPadding="6" CellSpacing="16" 
                                            AllowPaging="true" PageSize="15" DataKeyNames="ActualDocumentUID" CssClass="table table-bordered" OnRowDataBound="GrdActualSubmittedDocuments_RowDataBound" OnPageIndexChanging="GrdActualSubmittedDocuments_PageIndexChanging" OnRowCommand="GrdActualSubmittedDocuments_RowCommand">
                                       <Columns>
                                           <asp:TemplateField HeaderText="Submittal Name">
                                            <ItemTemplate>
                                                <%#GetSubmittalName(Eval("DocumentUID").ToString())%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                           <asp:TemplateField HeaderText="Document Name" ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                     <img width="24" src='../../_assets/images/<%#GetDocumentTypeIcon(Eval("ActualDocument_Type").ToString())%>' alt='<%#Eval("ActualDocument_Type")%>' />  &nbsp;&nbsp;
                                                       <%--<asp:LinkButton ID="lnkviewpdf" class='<%#Convert.ToString(Eval("ActualDocument_Type")) == ".pdf" ? "view" : "hideItem" %>' runat="server" CausesValidation="false" CommandArgument='<%#Eval("ActualDocument_Path")%>' CommandName="ViewDoc"><%#Eval("ActualDocument_Name")%></asp:LinkButton>--%>
                                                       <a id="ShowFile1" href='/_modal_pages/preview-document.aspx?ActualDocumentUID=<%#Eval("ActualDocumentUID")%>&previewpath=<%#Eval("ActualDocument_Path").ToString().Replace('&','!')%>' class='<%#Convert.ToString(Eval("ActualDocument_Type")) == ".pdf" ? "showModalPreviewDocument" : Convert.ToString(Eval("ActualDocument_Type")) == ".xls" ? "showModalPreviewDocument" : Convert.ToString(Eval("ActualDocument_Type")) == ".xlsx" ? "showModalPreviewDocument" : Convert.ToString(Eval("ActualDocument_Type")) == ".docx" ? "showModalPreviewDocument" : Convert.ToString(Eval("ActualDocument_Type")) == ".doc" ? "showModalPreviewDocument" : Convert.ToString(Eval("ActualDocument_Type")) == ".PDF" ? "showModalPreviewDocument" : "hideItem" %>'><%#Eval("ActualDocument_Name")%></a>
                                                       <asp:Label ID="lblName" runat="server" Text='<%#Eval("ActualDocument_Name")%>' Visible='<%#Convert.ToString(Eval("ActualDocument_Type")) == ".pdf" ? false : Convert.ToString(Eval("ActualDocument_Type")) == ".xls" ? false : Convert.ToString(Eval("ActualDocument_Type")) == ".xlsx" ? false : Convert.ToString(Eval("ActualDocument_Type")) == ".docx" ? false : Convert.ToString(Eval("ActualDocument_Type")) == ".doc" ? false : Convert.ToString(Eval("ActualDocument_Type")) == ".PDF" ? false : true %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                           <asp:TemplateField HeaderText="Document Type">
                                            <ItemTemplate>
                                                <%#GetDocumentName(Eval("ActualDocument_Type").ToString())%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                           <asp:BoundField DataField="ActualDocument_CurrentStatus" HeaderText="Current Status">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="ActualDocumentUID" HeaderText="Submitted Date">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                           <asp:TemplateField>
                                            <ItemTemplate>              
                                                      <asp:LinkButton ID="LnkDownloadnew" runat="server" CausesValidation="false" CommandArgument='<%#Eval("ActualDocumentUID")%>' CommandName="download">Download</asp:LinkButton>               
                                            </ItemTemplate>
                                            </asp:TemplateField>
                                       </Columns>
                                       </asp:GridView>

                            <asp:GridView ID="GrdReviewedDocuments" EmptyDataText="No Documents Found." runat="server" Width="100%" AutoGenerateColumns="false" 
                                            AllowPaging="true" PageSize="15" DataKeyNames="ActualDocumentUID" CssClass="table table-bordered" OnRowDataBound="GrdReviewedDocuments_RowDataBound" OnPageIndexChanging="GrdReviewedDocuments_PageIndexChanging" OnRowCommand="GrdReviewedDocuments_RowCommand">
                                       <Columns>
                                           <asp:TemplateField HeaderText="Submittal Name">
                                            <ItemTemplate>
                                                <%#GetSubmittalName(Eval("DocumentUID").ToString())%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                           <asp:TemplateField HeaderText="Document Name" ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                             <img width="24" src='../../_assets/images/<%#GetDocumentTypeIcon(Eval("ActualDocument_Type").ToString())%>' alt='<%#Eval("ActualDocument_Type")%>' />  &nbsp;&nbsp;
                                                       <%--<asp:LinkButton ID="lnkviewpdf" class='<%#Convert.ToString(Eval("ActualDocument_Type")) == ".pdf" ? "view" : "hideItem" %>' runat="server" CausesValidation="false" CommandArgument='<%#Eval("ActualDocument_Path")%>' CommandName="ViewDoc"><%#Eval("ActualDocument_Name")%></asp:LinkButton>--%>
                                                       <a id="ShowFile2" href='/_modal_pages/preview-document.aspx?ActualDocumentUID=<%#Eval("ActualDocumentUID")%>&previewpath=<%#Eval("ActualDocument_Path").ToString().Replace('&','!')%>' class='<%#Convert.ToString(Eval("ActualDocument_Type")) == ".pdf" ? "showModalPreviewDocument" : Convert.ToString(Eval("ActualDocument_Type")) == ".xls" ? "showModalPreviewDocument" : Convert.ToString(Eval("ActualDocument_Type")) == ".xlsx" ? "showModalPreviewDocument" : Convert.ToString(Eval("ActualDocument_Type")) == ".docx" ? "showModalPreviewDocument" : Convert.ToString(Eval("ActualDocument_Type")) == ".doc" ? "showModalPreviewDocument" : Convert.ToString(Eval("ActualDocument_Type")) == ".PDF" ? "showModalPreviewDocument" : "hideItem" %>'><%#Eval("ActualDocument_Name")%></a>
                                                       <asp:Label ID="lblName" runat="server" Text='<%#Eval("ActualDocument_Name")%>' Visible='<%#Convert.ToString(Eval("ActualDocument_Type")) == ".pdf" ? false : Convert.ToString(Eval("ActualDocument_Type")) == ".xls" ? false : Convert.ToString(Eval("ActualDocument_Type")) == ".xlsx" ? false : Convert.ToString(Eval("ActualDocument_Type")) == ".docx" ? false : Convert.ToString(Eval("ActualDocument_Type")) == ".doc" ? false : Convert.ToString(Eval("ActualDocument_Type")) == ".PDF" ? false : true %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                           <asp:TemplateField HeaderText="Document Type">
                                            <ItemTemplate>
                                                <%#GetDocumentName(Eval("ActualDocument_Type").ToString())%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                           <asp:BoundField DataField="ActualDocument_CurrentStatus" HeaderText="Current Status">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                           <asp:BoundField DataField="ActualDocumentUID" HeaderText="Reviewed Date">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                         <%--  <asp:BoundField DataField="ActualDocumentUID" HeaderText="Reviewed Days">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            </asp:BoundField>--%>
                                           <asp:TemplateField>
                                            <ItemTemplate>              
                                                      <asp:LinkButton ID="LnkDownloadnew" runat="server" CausesValidation="false" CommandArgument='<%#Eval("ActualDocumentUID")%>' CommandName="download">Download</asp:LinkButton>               
                                            </ItemTemplate>
                                            </asp:TemplateField>
                                           <%--<asp:TemplateField HeaderText="Reviewed Date">
                                               <ItemTemplate>
                                                   <%#Eval("ActualDocument_CreatedDate","{0:dd MMM yyyy}")%>
                                               </ItemTemplate>
                                           </asp:TemplateField>--%>
                                       </Columns>
                                       </asp:GridView>

                            <asp:GridView ID="GrdApprovedDocuments" EmptyDataText="No Documents Found." runat="server" Width="100%" AutoGenerateColumns="false" 
                                            AllowPaging="true" PageSize="15" DataKeyNames="ActualDocumentUID" CssClass="table table-bordered" OnRowDataBound="GrdApprovedDocuments_RowDataBound" OnPageIndexChanging="GrdApprovedDocuments_PageIndexChanging" OnRowCommand="GrdApprovedDocuments_RowCommand">
                                       <Columns>
                                            
                                           <asp:TemplateField HeaderText="Submittal Name">
                                            <ItemTemplate>
                                                <%#GetSubmittalName(Eval("DocumentUID").ToString())%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                           <asp:TemplateField HeaderText="Document Name" ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                            <img width="24" src='../../_assets/images/<%#GetDocumentTypeIcon(Eval("ActualDocument_Type").ToString())%>' alt='<%#Eval("ActualDocument_Type")%>' />  &nbsp;&nbsp;
                                                       <%--<asp:LinkButton ID="lnkviewpdf" class='<%#Convert.ToString(Eval("ActualDocument_Type")) == ".pdf" ? "view" : "hideItem" %>' runat="server" CausesValidation="false" CommandArgument='<%#Eval("ActualDocument_Path")%>' CommandName="ViewDoc"><%#Eval("ActualDocument_Name")%></asp:LinkButton>--%>
                                                       <a id="ShowFile3" href='/_modal_pages/preview-document.aspx?ActualDocumentUID=<%#Eval("ActualDocumentUID")%>&previewpath=<%#Eval("ActualDocument_Path").ToString().Replace('&','!')%>' class='<%#Convert.ToString(Eval("ActualDocument_Type")) == ".pdf" ? "showModalPreviewDocument" : Convert.ToString(Eval("ActualDocument_Type")) == ".xls" ? "showModalPreviewDocument" : Convert.ToString(Eval("ActualDocument_Type")) == ".xlsx" ? "showModalPreviewDocument" : Convert.ToString(Eval("ActualDocument_Type")) == ".docx" ? "showModalPreviewDocument" : Convert.ToString(Eval("ActualDocument_Type")) == ".doc" ? "showModalPreviewDocument" : Convert.ToString(Eval("ActualDocument_Type")) == ".PDF" ? "showModalPreviewDocument" : "hideItem" %>'><%#Eval("ActualDocument_Name")%></a>
                                                       <asp:Label ID="lblName" runat="server" Text='<%#Eval("ActualDocument_Name")%>' Visible='<%#Convert.ToString(Eval("ActualDocument_Type")) == ".pdf" ? false : Convert.ToString(Eval("ActualDocument_Type")) == ".xls" ? false : Convert.ToString(Eval("ActualDocument_Type")) == ".xlsx" ? false : Convert.ToString(Eval("ActualDocument_Type")) == ".docx" ? false : Convert.ToString(Eval("ActualDocument_Type")) == ".doc" ? false : Convert.ToString(Eval("ActualDocument_Type")) == ".PDF" ? false : true %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                           <asp:TemplateField HeaderText="Document Type">
                                            <ItemTemplate>
                                                <%#GetDocumentName(Eval("ActualDocument_Type").ToString())%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                           <asp:BoundField DataField="ActualDocument_CurrentStatus" HeaderText="Current Status">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                             <asp:BoundField DataField="ActualDocumentUID" HeaderText="Approved Date">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                           <%--<asp:BoundField DataField="ActualDocumentUID" HeaderText="Approved Days">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            </asp:BoundField>--%>
                                           <asp:TemplateField>
                                            <ItemTemplate>              
                                                      <asp:LinkButton ID="LnkDownloadnew" runat="server" CausesValidation="false" CommandArgument='<%#Eval("ActualDocumentUID")%>' CommandName="download">Download</asp:LinkButton>               
                                            </ItemTemplate>
                                            </asp:TemplateField>
                                       </Columns>
                                       </asp:GridView>

                            <asp:GridView ID="GrdClientApprovedDocuments" EmptyDataText="No Documents Found." runat="server" Width="100%" AutoGenerateColumns="false" 
                                            AllowPaging="true" PageSize="15" DataKeyNames="ActualDocumentUID" CssClass="table table-bordered" OnPageIndexChanging="GrdClientApprovedDocuments_PageIndexChanging" OnRowDataBound="GrdClientApprovedDocuments_RowDataBound" OnRowCommand="GrdClientApprovedDocuments_RowCommand">
                                       <Columns>
                                           <asp:BoundField DataField="SerialNo" HeaderText="Serial No">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                           <asp:BoundField DataField="ProjectName" HeaderText="Project Name">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                           <asp:BoundField DataField="FlowName" HeaderText="Flow Name">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                          <%-- <asp:TemplateField HeaderText="Submittal Name">
                                            <ItemTemplate>
                                                <%#GetSubmittalName(Eval("DocumentUID").ToString())%>
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>
                                            <asp:TemplateField HeaderText="Submittal Name" SortExpression="DocName">
                                            <ItemTemplate>
                                                <a href="#"  data-toggle="tooltip" data-placement="top" title='<%#GetTaskHierarchy_By_DocumentUID(Eval("DocumentUID").ToString())%>'><i class="fa fa-info-circle" style="color:black;" aria-hidden="true"></i></a>&nbsp;&nbsp; <a href="/_content_pages/documents/Default.aspx?SubmittalUID=<%#Eval("DocumentUID")%>"><%#GetSubmittalName(Eval("DocumentUID").ToString())%></a>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                          <asp:TemplateField ItemStyle-Width="35%" HeaderStyle-Width="35%" HeaderText="Document Name" ItemStyle-HorizontalAlign="Left" SortExpression="ActualDocument_Name">
                                            <ItemTemplate>
                                                       <img width="24" src='../../_assets/images/<%#GetDocumentTypeIcon(Eval("ActualDocument_Type").ToString())%>' alt='<%#Eval("ActualDocument_Type")%>' />  &nbsp;&nbsp;
                                                       <%--<asp:LinkButton ID="lnkviewpdf" class='<%#Convert.ToString(Eval("ActualDocument_Type")) == ".pdf" ? "view" : "hideItem" %>' runat="server" CausesValidation="false" CommandArgument='<%#Eval("ActualDocument_Path")%>' CommandName="ViewDoc"><%#Eval("ActualDocument_Name")%></asp:LinkButton>--%>
                                                       <a id="ShowFile" href='/_modal_pages/preview-document.aspx?ActualDocumentUID=<%#Eval("ActualDocumentUID")%>&previewpath=<%#Eval("ActualDocument_Path").ToString().Replace('&','!')%>' class= "showModalPreviewDocument '<%#Convert.ToString(Eval("ActualDocument_Type")) == ".pdf" ? "showModalPreviewDocument" : Convert.ToString(Eval("ActualDocument_Type")) == ".xls" ? "showModalPreviewDocument" : Convert.ToString(Eval("ActualDocument_Type")) == ".xlsx" ? "showModalPreviewDocument" : Convert.ToString(Eval("ActualDocument_Type")) == ".docx" ? "showModalPreviewDocument" : Convert.ToString(Eval("ActualDocument_Type")) == ".doc" ? "showModalPreviewDocument" : Convert.ToString(Eval("ActualDocument_Type")) == ".PDF" ? "showModalPreviewDocument" : "hideItem" %>'" >
                                                          <asp:Label ID="lblDocumentName" runat="server" Text='<%#Eval("ActualDocument_Name")%>'></asp:Label></a>
                                                       <asp:Label ID="lblName" runat="server" Text='<%#Eval("ActualDocument_Name")%>' Visible='<%#Convert.ToString(Eval("ActualDocument_Type")) == ".pdf" ? false : Convert.ToString(Eval("ActualDocument_Type")) == ".xls" ? false : Convert.ToString(Eval("ActualDocument_Type")) == ".xlsx" ? false : Convert.ToString(Eval("ActualDocument_Type")) == ".docx" ? false : Convert.ToString(Eval("ActualDocument_Type")) == ".doc" ? false : Convert.ToString(Eval("ActualDocument_Type")) == ".PDF" ? false : true %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:TemplateField>
                                           <asp:TemplateField HeaderText="Document Type">
                                            <ItemTemplate>
                                                <%#GetDocumentName(Eval("ActualDocument_Type").ToString())%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                           <asp:BoundField DataField="ActualDocument_CurrentStatus" HeaderText="Current Status">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                             <asp:BoundField DataField="ActualDocument_CurrentStatus" HeaderText="Next Level Action" SortExpression="ActualDocument_CurrentStatus">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                           <%--  <asp:BoundField DataField="ActualDocumentUID" HeaderText="Approved Date">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            </asp:BoundField>--%>
                                          <%-- <asp:BoundField DataField="Document_Date" HeaderText="Accepted/Rejected Date" SortExpression="Document_Date" DataFormatString="{0:dd/MM/yyyy}">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:BoundField>--%>
                                           
                                            <asp:BoundField DataField="ProjectRef_Number" HeaderText="ONTB Reference #" SortExpression="ProjectRef_Number">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                           <asp:BoundField DataField="Ref_Number" HeaderText="Originator Reference #" SortExpression="Ref_Number">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                             <asp:BoundField DataField="ActualDocument_CreatedDate" HeaderText="Uploaded Date" SortExpression="ActualDocument_CreatedDate" DataFormatString="{0:dd/MM/yyyy}" >
                                            <HeaderStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <a class="showModalDocumentMail" href="/_modal_pages/view-documenthistory.aspx?DocID=<%#Eval("ActualDocumentUID")%>&ProjectUID=<%#Eval("ProjectUID")%>">View History</a>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                            <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <a class="showModalDocumentView" href="/_modal_pages/view-documentdetails.aspx?DocID=<%#Eval("ActualDocumentUID")%>&ProjectUID=<%#Eval("ProjectUID")%>">View Details</a>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                            <asp:TemplateField>
                                            <ItemTemplate>              
                                                      <asp:LinkButton ID="LnkDownloadnew" runat="server" CausesValidation="false" CommandArgument='<%#Eval("ActualDocumentUID")%>' CommandName="download">Download</asp:LinkButton>               
                                            </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="ActualDocumentUID"  HeaderText="UID" ItemStyle-CssClass="hideItem" HeaderStyle-CssClass="hideItem">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                           <asp:BoundField DataField="FlowUID" HeaderText="Serial No" ItemStyle-CssClass="hideItem" HeaderStyle-CssClass="hideItem">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                       </Columns>
                                       </asp:GridView>

                            </div>




                        
                        </div>
                    </div>
                </div>
            </div>
        </div>
    <%--View document histroy modal--%>
    <div id="ModDocumentPreview" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-xl">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Document Preview</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:500px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>
     <%--View document History  modal--%>
    <div id="ModDocumentMail" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-xl">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Document History</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:500px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>
     <%--View document modal--%>
    <div id="ModViewDocument" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-xl">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">View Document Details</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:500px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>
</asp:Content>
