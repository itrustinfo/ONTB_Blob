<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/default.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="ProjectManagementTool._content_pages.document_view._default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="default_master_head" runat="server">
     <style type="text/css">
    
  .black_overlay {
  display: none;
  position: absolute;
  top: 0%;
  left: 0%;
  width: 100%; 
  height: 100%;
  background-color: black;
  z-index: 1001;
  -moz-opacity: 0.8;
  opacity: .80;
  filter: alpha(opacity=80);
}
.white_content {
  display: none;
  position: absolute;
  top:auto;
  left: 25%;
  width: 35%;
  padding: 10px;
  border: 8px solid #3498db;
  background-color: white;
  z-index: 1002;
  overflow: auto;
  
  text-align:justify;
  line-height:20px;
  box-shadow: 5px 10px #888888;
  font-weight:normal;
  font-size:large;
}
     .hideItem {
         display:none;
         
     }

      .showItem {
         display:block;
         
     }
  
    </style>
<script type="text/javascript">
    function expand1() {
            $("[id*=imgProductsShow]").each(function () {
                if ($(this)[0].src.indexOf("minus") != -1) {
                    $(this).closest("tr").after("<tr><td></td><td colspan = '999'>" + $(this).next().html() + "</td></tr>");
                    $(this).next().remove();
                }
        });

        
    }
    function expand2() {
        $("[id*=imgProductsShow1]").each(function () {
                if ($(this)[0].src.indexOf("minus") != -1) {
                    $(this).closest("tr").after("<tr><td></td><td colspan = '999'>" + $(this).next().html() + "</td></tr>");
                    $(this).next().remove();
                }
            });
    }
function DeleteItem() {
            if (confirm("Are you sure you want to delete ...?")) {
                return true;
            }
            return false;
    }

     function closepopupdoc() {
         if (confirm("Click OK to referesh the parent page OR Cancel to close without referesh !")) {
                 window.location.reload();
            }
           
    }

      function closepopupdocmsg() {
         
              if (confirm("Click OK to referesh the parent page OR Cancel to close without referesh !")) {
                 window.location.reload();
            }
            
           
        }
 </script>

    <script type="text/javascript">
        function BindEvents() {
        $(".showModal").click(function(e) {
            e.preventDefault();
        var url = $(this).attr("href");
        $("#ModAddsubmittal iframe").attr("src", url);
            $("#ModAddsubmittal").modal("show");
        });

        $(".showModalEdit").click(function(e) {
            e.preventDefault();
        var url = $(this).attr("href");
        $("#ModEditsubmittal iframe").attr("src", url);
        $("#ModEditsubmittal").modal("show");
            });

        $(".showModalDocument").click(function(e) {
        e.preventDefault();
        var url = $(this).attr("href");
        $("#ModAddDocument iframe").attr("src", url);
        $("#ModAddDocument").modal("show");
            });

        $(".showModalDocumentEdit").click(function(e) {
        e.preventDefault();
        var url = $(this).attr("href");
        $("#ModEditDocument iframe").attr("src", url);
        $("#ModEditDocument").modal("show");
            });

        $(".showModalDocumentHistory").click(function(e) {
        e.preventDefault();
        var url = $(this).attr("href");
        $("#ModViewDocumentHistory iframe").attr("src", url);
        $("#ModViewDocumentHistory").modal("show");
            });

            $(".showModalPreviewDocument").click(function(e) {
        e.preventDefault();
        var url = $(this).attr("href");
        $("#ModDocumentPreview iframe").attr("src", url);
        $("#ModDocumentPreview").modal("show");
            });

             $(".showModalDocumentMail").click(function(e) {
        e.preventDefault();
        var url = $(this).attr("href");
        $("#ModDocumentMail iframe").attr("src", url);
        $("#ModDocumentMail").modal("show");
             });

            $(".showModalDocumentMailGenDocs").click(function(e) {
        e.preventDefault();
        var url = $(this).attr("href");
        $("#ModDocumentMail iframe").attr("src", url);
        $("#ModDocumentMail").modal("show");
            });

            $(".showCopyModal").click(function(e) {
        e.preventDefault();
        var url = $(this).attr("href");
        $("#ModCopyDocument iframe").attr("src", url);
        $("#ModCopyDocument").modal("show");
        });

            $(".showAddGeneralDocumentStructureModal").click(function(e) {
        e.preventDefault();
        var url = $(this).attr("href");
        $("#ModAddGeneralDocumentStructure iframe").attr("src", url);
        $("#ModAddGeneralDocumentStructure").modal("show");
             });

            $(".showGeneralDocumentStructureEditModal").click(function(e) {
        e.preventDefault();
        var url = $(this).attr("href");
        $("#ModEditGeneralDocumentStructure iframe").attr("src", url);
        $("#ModEditGeneralDocumentStructure").modal("show");
            });

            $(".showAddGeneralDocumentModal").click(function(e) {
        e.preventDefault();
        var url = $(this).attr("href");
        $("#ModAddGeneralDocument iframe").attr("src", url);
        $("#ModAddGeneralDocument").modal("show");
             });

            $(".showGeneralDocumentEditModal").click(function(e) {
        e.preventDefault();
        var url = $(this).attr("href");
        $("#ModEditGeneralDocument iframe").attr("src", url);
        $("#ModEditGeneralDocument").modal("show");
            });
        }
        $(document).ready(function () {
            BindEvents();
            expand1();
            expand2();
            //$('#loader').fadeOut();
        });
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="default_master_body" runat="server">
    <asp:ScriptManager EnablePartialRendering="true"
            ID="ScriptManager1" runat="server"></asp:ScriptManager>
     <div class="container-fluid">
            <div class="row">
                <asp:HiddenField ID="HiddenPaging" runat="server" />
                <div class="col-md-12 col-lg-4 form-group">Documents View</div>
                <div class="col-md-6 col-lg-4 form-group">
                    <label class="sr-only" for="DDLProject">Project</label>
                    <div class="input-group">
                        <div class="input-group-prepend">
                            <span class="input-group-text">Project</span>
                        </div>
                        <asp:DropDownList ID="DDlProject" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="DDlProject_SelectedIndexChanged"></asp:DropDownList>
                        <%--View document modal--%>
                    </div>
                </div>
                <div class="col-md-6 col-lg-4 form-group" id="divWP"  runat="server">
                    <label class="sr-only" for="DDLWorkPackage">Work Package</label>
                    <div class="input-group">
                        <div class="input-group-prepend">
                            <span class="input-group-text">Work Package</span>
                        </div>
                        <asp:DropDownList ID="DDLWorkPackage" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="DDLWorkPackage_SelectedIndexChanged"></asp:DropDownList>

                        <%--edit document modal--%>
                    </div>
                </div>
            </div>
        </div>
        <div class="container-fluid">
        <div class="row">
            <div class="col-lg-12 col-xl-12 form-group">
                <div class="card mb-4">
                    <div class="card-body">
                        <div class="card-title" id="divoption" runat="server">
                        <div class="d-flex justify-content-between">
                              <h6 class="text-muted">
                                   <asp:Label ID="LblOption" Text="Option" CssClass="text-uppercase font-weight-bold" runat="server" />
                               </h6>
                                <div>
                                   
                                </div>
                          </div>
                            <asp:RadioButtonList ID="RBOptionList" runat="server" 
                                DataTextField="WorkpackageSelectedOption_Name" DataValueField="Workpackage_OptionUID" AutoPostBack="true"
                                CssClass="text-muted text-uppercase font-weight-bold" CellPadding="4" RepeatDirection="Horizontal" OnSelectedIndexChanged="RBOptionList_SelectedIndexChanged"></asp:RadioButtonList>
                            </div>


                        <%--<div class="card-title">
                        <div class="d-flex justify-content-between">
                              <h6 class="text-muted">
                                   <asp:Label ID="LblTaskSelect" Text="Choose Task" CssClass="text-uppercase font-weight-bold" runat="server" />
                               </h6>
                                <div>
                                   
                                </div>
                          </div>
                            </div>--%>
                        <asp:UpdatePanel ID="Update1" runat="server">
                                <ContentTemplate>
                        <div class="row">                            
                        <div class="col-md-12 col-lg-4 form-group" id="MainTask" runat="server">
                <label class="sr-only" for="DDLMainTask">Task</label>
                <div class="input-group">
                     <div class="input-group-prepend">
                            <span class="input-group-text">Task</span>
                     </div>
                    <asp:DropDownList ID="DDLMainTask" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="DDLMainTask_SelectedIndexChanged"></asp:DropDownList>
                </div>
            </div>
                        <div class="col-md-12 col-lg-4 form-group" id="Task1" runat="server">
                <label class="sr-only" for="DDLSubTask">Sub Task</label>
                <div class="input-group">
                     <div class="input-group-prepend">
                            <span class="input-group-text">Sub Task</span>
                     </div>
                    <asp:DropDownList ID="DDLSubTask" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="DDLSubTask_SelectedIndexChanged"></asp:DropDownList>
                </div>
            </div>
                        <div class="col-md-12 col-lg-4 form-group" id="Task2" runat="server">
                <label class="sr-only" for="DDLSubTask1">Sub Task</label>
                <div class="input-group">
                     <div class="input-group-prepend">
                            <span class="input-group-text">Sub Task</span>
                     </div>
                    <asp:DropDownList ID="DDLSubTask1" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="DDLSubTask1_SelectedIndexChanged"></asp:DropDownList>
                </div>
            </div>
                        <div class="col-md-12 col-lg-4 form-group" id="Task3" runat="server">
                <label class="sr-only" for="DDLSubTask2">Sub Task</label>
                <div class="input-group">
                     <div class="input-group-prepend">
                            <span class="input-group-text">Sub Task</span>
                     </div>
                    <asp:DropDownList ID="DDLSubTask2" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="DDLSubTask2_SelectedIndexChanged"></asp:DropDownList>
                </div>
            </div>
                        <div class="col-md-12 col-lg-4 form-group" id="Task4" runat="server">
                <label class="sr-only" for="DDLSubTask3">Sub Task</label>
                <div class="input-group">
                     <div class="input-group-prepend">
                            <span class="input-group-text">Sub Task</span>
                     </div>
                    <asp:DropDownList ID="DDLSubTask3" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="DDLSubTask3_SelectedIndexChanged"></asp:DropDownList>
                </div>
            </div>
                        <div class="col-md-12 col-lg-4 form-group" id="Task5" runat="server">
                <label class="sr-only" for="DDLSubTask4">Sub Task</label>
                <div class="input-group">
                     <div class="input-group-prepend">
                            <span class="input-group-text">Sub Task</span>
                     </div>
                    <asp:DropDownList ID="DDLSubTask4" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="DDLSubTask4_SelectedIndexChanged"></asp:DropDownList>
                </div>
            </div>
                        <div class="col-md-12 col-lg-4 form-group text-right" id="Task6" runat="server">
                                <label class="sr-only" for="DDLSubTask5">Sub Task</label>
                                <div class="input-group">
                                     <div class="input-group-prepend">
                                            <span class="input-group-text">Sub Task</span>
                                     </div>
                                    <asp:DropDownList ID="DDLSubTask5" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="DDLSubTask5_SelectedIndexChanged"></asp:DropDownList>
                                </div>
                            </div>
                        <div class="col-md-12 col-lg-4 form-group text-right" id="Task7" runat="server">
                                <div class="col-md-12 col-lg-4 form-group text-right" id="Div1" runat="server">
                                <label class="sr-only" for="DDLSubTask6">Sub Task</label>
                                <div class="input-group">
                                     <div class="input-group-prepend">
                                            <span class="input-group-text">Sub Task</span>
                                     </div>
                                    <asp:DropDownList ID="DDLSubTask6" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="DDLSubTask6_SelectedIndexChanged"></asp:DropDownList>
                                </div>
                            </div>
                            </div>  

                        </div>
                        <div class="row">
                            <div class="col-md-12 col-lg-4 form-group" id="divsubmit" runat="server">
                                
                                <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-primary" OnClick="btnSubmit_Click"/>
                                &nbsp;<asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="btn btn-primary" OnClick="btnClear_Click"/>
                                <br />
                                <asp:Label ID="LblMessage" runat="server" Font-Bold="true"></asp:Label>
                                </div>
                        </div>
                    </ContentTemplate>
                            <Triggers>
                                <asp:PostBackTrigger ControlID="btnSubmit" />
                            </Triggers>
                           </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
            
            </div>
        </div>
    <div class="container-fluid" id="DocumentDiv" runat="server">
        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
         <ContentTemplate>
        <div class="row">
            <div class="col-lg-6 col-xl-12 form-group">
                                    <asp:UpdateProgress ID="UpdateProgress1" runat="Server" AssociatedUpdatePanelID="UpdatePanel2" DisplayAfter="1"> 
                                        <ProgressTemplate > 
                                        <!-- <asp:Label ID="lblwait" runat="server" Text="Please wait while we fetch the data.." ForeColor="Red"></asp:Label>-->
                                            <div id="loader"></div>
                                        </ProgressTemplate> 
                                    </asp:UpdateProgress>
                                        <div class="card mb-4" >
                                            <div class="card-body">
                                                <div class="card-title">
                                                    <div class="d-flex justify-content-between">
                                                        <h6 class="text-muted">
                                                            <asp:Label ID="ActivityHeading" CssClass="text-uppercase font-weight-bold" runat="server" />
                                                            <%--<asp:Label Text="Foo bar" CssClass="pl-1" runat="server" />--%>
                                                        </h6>
                                                        
                                                        
                                                        <div>
                                                            
                                                            <a id="AddDocument" runat="server"  href="/_modal_pages/add-submittal.aspx" class="showModal"><asp:Button ID="btnSubmittal" runat="server" Text="+ Add Submittal" CssClass="btn btn-primary" align="end"></asp:Button></a>
                                                        </div>
                                                        
                                                    </div>
                                                    <div id="DivDocumentsSyncedCount" runat="server" visible="false" style="text-align:left; font-size:10pt; margin-top:10px;display:none">
                                                            
                                                            <b class="card-title text-muted text-uppercase">Last Synced Date : </b>
                                                            <asp:Label ID="LblLastSyncedDate" runat="server" Font-Bold="true" ForeColor="Green" Text="2021-09-03 14:34:05.847"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;
                                                            <b class="card-title text-muted text-uppercase"><asp:Label ID="LblSourceHeading" runat="server"></asp:Label> : </b>
                                                            <asp:Label ID="LblTotalSourceDocuments" runat="server" Font-Bold="true" ForeColor="Green" Text="500"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;
                                                            <b class="card-title text-muted text-uppercase"><asp:Label ID="LblDestinationHeading" runat="server"></asp:Label> : </b>
                                                            <asp:Label ID="LblTotalDestinationDocuments" runat="server" Font-Bold="true" ForeColor="Green" Text="500"></asp:Label>
                                                        </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="card" id="DocumentGrid" runat="server">
                                            <div class="card-body">
                                                <h6 class="card-title text-muted text-uppercase font-weight-bold">Submittals</h6>
                                                
                                                <asp:GridView ID="GrdNewDocument" class="table table-bordered" DataKeyNames="DocumentUID" PageSize="15" AllowSorting="true" runat="server" AllowPaging="true" EmptyDataText="No Submittal Found." AutoGenerateColumns="False" Width="100%" OnPageIndexChanging="GrdNewDocument_PageIndexChanging" OnRowCommand="GrdNewDocument_RowCommand" OnRowDeleting="GrdNewDocument_RowDeleting" OnRowDataBound="GrdNewDocument_RowDataBound" >
                                        <Columns>
                                            <asp:TemplateField>
                                                  <ItemTemplate>
                                                      <asp:ImageButton ID="imgProductsShow" runat="server" OnClick="Show_Hide_DocumentsGrid" ImageUrl="~/_assets/images/plus.png"
                                                                CommandArgument="Show" Height="25px" Width="25px"/>
                                                      <asp:Panel ID="pnlDocuments" runat="server" Visible="false" style="position:relative;">
                                                          <script type="text/javascript">
                                                              $(document).ready(function () {
                                                                  $(".showModalDocumentEdit").click(function (e) {
                                                                      e.preventDefault();
                                                                      var url = $(this).attr("href");
                                                                      $("#ModEditDocument iframe").attr("src", url);
                                                                      $("#ModEditDocument").modal("show");
                                                                  });
                                                                  $(".showModalDocumentHistory").click(function (e) {
                                                                      e.preventDefault();
                                                                      var url = $(this).attr("href");
                                                                      $("#ModViewDocumentHistory iframe").attr("src", url);
                                                                      $("#ModViewDocumentHistory").modal("show");
                                                                  });
                                                                  $(".showModalPreviewDocument").click(function (e) {
                                                                      e.preventDefault();
                                                                      var url = $(this).attr("href");
                                                                      $("#ModDocumentPreview iframe").attr("src", url);
                                                                      $("#ModDocumentPreview").modal("show");
                                                                  });
                                                                  $(".showModalDocumentMail").click(function(e) {
                                                                    e.preventDefault();
                                                                    var url = $(this).attr("href");
                                                                    $("#ModDocumentMail iframe").attr("src", url);
                                                                    $("#ModDocumentMail").modal("show");
                                                                   });
                                                              });
                                                        </script>
                                                          <div style="overflow: auto;">
                                                          <asp:GridView ID="GrdActualDocuments" runat="server" Width="100%" AutoGenerateColumns="false"  class="table table-bordered"
                                                                    AllowPaging="false" DataKeyNames="ActualDocumentUID" OnRowDataBound="GrdActualDocuments_RowDataBound" OnRowCommand="GrdActualDocuments_RowCommand" OnRowDeleting="GrdActualDocuments_RowDeleting" >
                                                               <Columns>
                                                                   <asp:TemplateField HeaderText="Name" ItemStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                      
                                                                       <%-- <asp:UpdatePanel runat="server" ID="UpdatePanelview" UpdateMode="Conditional" ChildrenAsTriggers="True">
                                                                        <ContentTemplate>--%>
                                                                        <div id="divPDF" runat="server">
                                                                                <img width="24" src='<%#GetDocumentTypeIcon(Eval("ActualDocument_Type").ToString(),Eval("ActualDocumentUID").ToString(),"Actual Document")%>' alt='<%#Eval("ActualDocument_Type")%>' />  &nbsp;&nbsp;
                                                                            <%--<asp:LinkButton ID="lnkviewpdf" class='<%#Convert.ToString(Eval("ActualDocument_Type")) == ".pdf" ? "view" : "hideItem" %>' runat="server" CausesValidation="false" CommandArgument='<%#Eval("ActualDocument_Path")%>' CommandName="ViewDoc"><%#Eval("ActualDocument_Name")%></asp:LinkButton>--%>
                                                                            <%--<a id="ShowFile" href='/_modal_pages/preview-document.aspx?ActualDocumentUID=<%#Eval("ActualDocumentUID")%>&previewpath=<%#Eval("ActualDocument_Path").ToString().Replace('&','!')%>' class='<%#Convert.ToString(Eval("ActualDocument_Type")) == ".pdf" ? "showModalPreviewDocument" : Convert.ToString(Eval("ActualDocument_Type")) == ".xls" ? "showModalPreviewDocument" : Convert.ToString(Eval("ActualDocument_Type")) == ".xlsx" ? "showModalPreviewDocument" : Convert.ToString(Eval("ActualDocument_Type")) == ".docx" ? "showModalPreviewDocument" : Convert.ToString(Eval("ActualDocument_Type")) == ".doc" ? "showModalPreviewDocument" : Convert.ToString(Eval("ActualDocument_Type")) == ".PDF" ? "showModalPreviewDocument" : "hideItem" %>'><%#Eval("ActualDocument_Name")%></a>--%>
                                                                            <a id="ShowFile" href='/_modal_pages/preview-document.aspx?ActualDocumentUID=<%#Eval("ActualDocumentUID")%>&previewpath=<%#Eval("ActualDocument_Path").ToString().Replace('&','!')%>' class='<%#Convert.ToString(Eval("ActualDocument_Type")) == ".pdf" ? "showModalPreviewDocument" : Convert.ToString(Eval("ActualDocument_Type")) == ".xls" ? "showModalPreviewDocument" : Convert.ToString(Eval("ActualDocument_Type")) == ".xlsx" ? "showModalPreviewDocument" : Convert.ToString(Eval("ActualDocument_Type")) == ".docx" ? "showModalPreviewDocument" : Convert.ToString(Eval("ActualDocument_Type")) == ".doc" ? "showModalPreviewDocument" : Convert.ToString(Eval("ActualDocument_Type")) == ".PDF" ? "showModalPreviewDocument" : "hideItem" %>'>
                                                                                <asp:Label ID="lblDocumentName" runat="server" Text='<%#Eval("ActualDocument_Name")%>'></asp:Label>
                                                                            </a>
                                                                            <asp:Label ID="lblName" runat="server" Text='<%#Eval("ActualDocument_Name")%>' Visible='<%#Convert.ToString(Eval("ActualDocument_Type")) == ".pdf" ? false : Convert.ToString(Eval("ActualDocument_Type")) == ".xls" ? false : Convert.ToString(Eval("ActualDocument_Type")) == ".xlsx" ? false : Convert.ToString(Eval("ActualDocument_Type")) == ".docx" ? false : Convert.ToString(Eval("ActualDocument_Type")) == ".doc" ? false : Convert.ToString(Eval("ActualDocument_Type")) == ".PDF" ? false : true %>'></asp:Label>
                                                                                <asp:Label ID="LblVersion" runat="server"></asp:Label>
                                                                            </div>
                                                                        <%--<a href='/_modal_pages/preview-document.aspx?previewpath=<%#Eval("ActualDocument_Path").ToString().Replace('&','!')%>&ActualDocumentUID=<%#Eval("ActualDocumentUID")%>' class='<%#Convert.ToString(Eval("ActualDocument_Type")) == ".pdf" ? "showModalPreviewDocument" : "hideItem" %>'><%#Eval("ActualDocument_Name")%></a>
                                                                            <asp:Label ID="lblName" runat="server" Text='<%#Eval("ActualDocument_Name")%>' Visible='<%#Convert.ToString(Eval("ActualDocument_Type")) == ".pdf" ? false : Convert.ToString(Eval("ActualDocument_Type")) == ".xls" ? false : true %>'></asp:Label>
                                                                                <asp:Label ID="LblVersion" runat="server"></asp:Label>--%>
                                                                         <img id="imgfolder" runat="server" src="/_assets/images/folder.jpg" height="15" width="20">
                                                                        
                                                                        <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandName="Folder_View" CommandArgument='<%#Eval("ActualDocumentUID")%>'
                                                                         Text="Edit"></asp:LinkButton>

                                                                         
                                                                            <%--</ContentTemplate>
                                                                          <Triggers>
                                                                            <asp:PostBackTrigger ControlID="lnkviewpdf" />
                                                                          </Triggers>
                                                                        </asp:UpdatePanel>--%>
                                                                                <%--<a class='<%#Convert.ToString(Eval("ActualDocument_Type")) == ".pdf" ? "view" : "hideItem" %>' target="_blank"  href='<%#Server.MapPath(Eval("ActualDocument_Path").ToString())%>'><%#Eval("ActualDocument_Name")%></a>--%>
                                                                                
                                                                             </ContentTemplate>
                                                                          
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                  <%-- <asp:BoundField DataField="ActualDocument_Version" HeaderText="Version">
                                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                                    </asp:BoundField>--%>
                                                                  <%-- <asp:BoundField DataField="Doc_Type" HeaderText="Document For">
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                    </asp:BoundField>--%>
                                                                   <asp:TemplateField HeaderText="Type">
                                                                    <ItemTemplate>
                                                                        <%#GetDocumentName(Eval("ActualDocument_Type").ToString())%>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                   <asp:BoundField DataField="ActualDocumentUID" HeaderText="Status">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                   <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                         <%--<a class="view" href="ViewDocument.aspx?DocID=<%#Eval("DocumentUID")%>">View</a>--%>
                                                                        <asp:UpdatePanel runat="server" ID="UpdatePanelDownload" UpdateMode="Conditional" ChildrenAsTriggers="True">
                                                                        <ContentTemplate>
                                                                            <asp:LinkButton ID="LnkDownloadnew" runat="server" CausesValidation="false" CommandArgument='<%#Eval("ActualDocumentUID")%>' CommandName="download">Download</asp:LinkButton>
                                                                            </ContentTemplate>
                                                                          <Triggers>
                                                                            <asp:PostBackTrigger ControlID="LnkDownloadnew" />
                                                                          </Triggers>
                                                                        </asp:UpdatePanel>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                   <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                            <a id="ViewByActivity" class="<%#Convert.ToString(Eval("Doc_Type")) == "Cover Letter" ? "hideItem" : Convert.ToString(Eval("Doc_Type")) =="General Document" ? "hideItem" : "showModalDocumentHistory" %>" 
                                                                                href="/_modal_pages/view-documenthistory.aspx?DocID=<%#Eval("ActualDocumentUID")%>&ProjectUID=<%#Eval("ProjectUID")%>&FlowUID=<%#Eval("FlowUID")%>">View/Update Status</a>

                                                                       <%-- <asp:HyperLink ID="anchorView" runat="server" CssClass='<%#Convert.ToString(Eval("Doc_Type")) == "Cover Letter" ? "hideItem" : Convert.ToString(Eval("Doc_Type")) =="General Document" ? "hideItem" : "showModalDocumentHistory" %>'
                                                                              NavigateUrl='/_modal_pages/view-documenthistory.aspx?DocID=<%#Eval("ActualDocumentUID")%>&ProjectUID=<%#Eval("ProjectUID")%>'
                                                                              Text="View/Update Status">
                                                                            </asp:HyperLink>--%>
                                                                        </ItemTemplate>
                                                                       </asp:TemplateField>
                                                                    <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <a class="showModalDocumentMail" href="/_modal_pages/mail-documentlink.aspx?DocID=<%#Eval("ActualDocumentUID")%>&pUID=<%#Eval("ProjectUID")%>&wUID=<%#Eval("WorkPackageUID")%>"><span title="Mail" class="fas fa-envelope"></span></a>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                   <%--<asp:TemplateField ItemStyle-CssClass="hideItem" HeaderStyle-CssClass="hideItem">--%>
                                                                   <asp:TemplateField ItemStyle-CssClass="hideItem" HeaderStyle-CssClass="hideItem">
                                                                    <ItemTemplate>
                                                                        <asp:UpdatePanel runat="server" ID="UpdatePanelCopy" UpdateMode="Conditional" ChildrenAsTriggers="True">
                                                                        <ContentTemplate>
                                                                        <asp:LinkButton ID="lnkcopy" runat="server"  CausesValidation="false" CommandArgument='<%#Eval("ActualDocumentUID")%>' CommandName="copyfile"><span title="Copy" class="fas fa-copy"></span></asp:LinkButton>
                                                                             </ContentTemplate>
                                                                            <Triggers>
                                                                                <asp:PostBackTrigger ControlID="lnkcopy" />
                                                                            </Triggers>
                                                                        </asp:UpdatePanel>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                   <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <a class="showModalDocumentEdit" href="/_modal_pages/edit-document.aspx?DocID=<%#Eval("ActualDocumentUID")%>&pUID=<%#Eval("ProjectUID")%>&wUID=<%#Eval("WorkPackageUID")%>"><span title="Edit" class="fas fa-edit"></span></a>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                   <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                       <%-- <asp:UpdatePanel runat="server" ID="UpdatePanelDelete" UpdateMode="Conditional" ChildrenAsTriggers="True">
                                                                        <ContentTemplate>--%>
                                                                        <asp:LinkButton ID="lnkactualdelete" runat="server" OnClientClick="return DeleteItem()" CausesValidation="false" CommandArgument='<%#Eval("ActualDocumentUID")%>' CommandName="delete"><span title="Delete" class="fas fa-trash"></span></asp:LinkButton>
                                                                            <%--</ContentTemplate>
                                                                            <Triggers>
                                                                                <asp:AsyncPostBackTrigger ControlID="lnkactualdelete" EventName="RowCommand" />
                                                                                
                                                                            </Triggers>
                                                                            </asp:UpdatePanel>--%>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                    <asp:BoundField DataField="ActualDocument_RelativePath" HeaderText="Path" ItemStyle-CssClass="hideItem" HeaderStyle-CssClass="hideItem">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                   
                                                               </Columns>
                                                               <EmptyDataTemplate>
                                                                    No Documents Found !
                                                                </EmptyDataTemplate>
                                                               </asp:GridView>
                                                              </div>
                                                       </asp:Panel>
                                                  </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Submittal Name" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <%#Eval("DocName")%>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Category" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <%#getCategoryName(Eval("Doc_Category").ToString())%>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                            </asp:TemplateField>
                                           <asp:TemplateField HeaderText="Created Date" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <%#Eval("CreatedDate","{0:dd MMM yyyy}")%>
                                                </ItemTemplate>
                                             <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                            </asp:TemplateField>
                                             <asp:TemplateField>
                                                <ItemTemplate>
                                                    <%--<button class="btn btn-primary" type="button" data-toggle="modal" data-target="#ModAddWorkpackages"><i class="fa fa-plus" aria-hidden="true"></i> Add WorkPackage</button>--%>
                                                    <a href="/_modal_pages/add-document.aspx?dUID=<%#Eval("DocumentUID")%>&pUID=<%#Eval("ProjectUID")%>&wUID=<%#Eval("WorkPackageUID")%>&tUID=<%#Eval("TaskUID")%>&sType=Activity&fID=<%#Eval("FlowUID")%>" class="showModalDocument"><asp:Label ID="lbl1" runat="server" Text="Add Documents"></asp:Label></a>
                                                    <%--<a data-toggle="modal" data-target="#ModAddDocuments" href="#">Add Documents</a>--%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <a class="showModalEdit" href="/_modal_pages/add-submittal.aspx?type=edit&DocID=<%#Eval("DocumentUID")%>&TaskUID=<%#Eval("TaskUID")%>&PrjUID=<%#Eval("ProjectUID")%>&WrkUID=<%#Eval("WorkPackageUID")%>&fID=<%#Eval("FlowUID")%>"><span title="Edit" class="fas fa-edit"></span></a>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkdelete" runat="server" CausesValidation="false" OnClientClick="return DeleteItem()" CommandArgument='<%#Eval("DocumentUID")%>' CommandName="delete"><span title="Delete" class="fas fa-trash"></span></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>             
                                        </Columns>
                                        <EmptyDataTemplate>
                                            No Submittal Found !
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                                <asp:GridView ID="GrdActualDocuments_new" runat="server" Width="100%" AutoGenerateColumns="false"  class="table table-bordered"
                                                                    AllowPaging="false" DataKeyNames="ActualDocumentUID" OnRowDataBound="GrdActualDocuments_new_RowDataBound" OnRowCommand="GrdActualDocuments_new_RowCommand" OnRowDeleting="GrdActualDocuments_new_RowDeleting" >
                                                               <Columns>
                                                                   <asp:TemplateField HeaderText="Name" ItemStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                      
                                                                       <%-- <asp:UpdatePanel runat="server" ID="UpdatePanelview" UpdateMode="Conditional" ChildrenAsTriggers="True">
                                                                        <ContentTemplate>--%>
                                                                        <div id="divPDF" runat="server">
                                                                                <img width="24" src='../../_assets/images/<%#GetDocumentTypeIcon(Eval("ActualDocument_Type").ToString(),Eval("ActualDocumentUID").ToString(),"Actual Document")%>' alt='<%#Eval("ActualDocument_Type")%>' />  &nbsp;&nbsp;
                                                                            <%--<asp:LinkButton ID="lnkviewpdf" class='<%#Convert.ToString(Eval("ActualDocument_Type")) == ".pdf" ? "view" : "hideItem" %>' runat="server" CausesValidation="false" CommandArgument='<%#Eval("ActualDocument_Path")%>' CommandName="ViewDoc"><%#Eval("ActualDocument_Name")%></asp:LinkButton>--%>
                                                                            <%--<a href='/_modal_pages/preview-document.aspx?ActualDocumentUID=<%#Eval("ActualDocumentUID")%>&previewpath=<%#Eval("ActualDocument_Path").ToString().Replace('&','!')%>' class='<%#Convert.ToString(Eval("ActualDocument_Type")) == ".pdf" ? "showModalPreviewDocument" : Convert.ToString(Eval("ActualDocument_Type")) == ".xls" ? "showModalPreviewDocument" : Convert.ToString(Eval("ActualDocument_Type")) == ".xlsx" ? "showModalPreviewDocument" : Convert.ToString(Eval("ActualDocument_Type")) == ".docx" ? "showModalPreviewDocument" : Convert.ToString(Eval("ActualDocument_Type")) == ".doc" ? "showModalPreviewDocument" : Convert.ToString(Eval("ActualDocument_Type")) == ".PDF" ? "showModalPreviewDocument" : "hideItem" %>'><%#Eval("ActualDocument_Name")%></a>--%>
                                                                            <a href='/_modal_pages/preview-document.aspx?ActualDocumentUID=<%#Eval("ActualDocumentUID")%>&previewpath=<%#Eval("ActualDocument_Path").ToString().Replace('&','!')%>' class='<%#Convert.ToString(Eval("ActualDocument_Type")) == ".pdf" ? "showModalPreviewDocument" : Convert.ToString(Eval("ActualDocument_Type")) == ".xls" ? "showModalPreviewDocument" : Convert.ToString(Eval("ActualDocument_Type")) == ".xlsx" ? "showModalPreviewDocument" : Convert.ToString(Eval("ActualDocument_Type")) == ".docx" ? "showModalPreviewDocument" : Convert.ToString(Eval("ActualDocument_Type")) == ".doc" ? "showModalPreviewDocument" : Convert.ToString(Eval("ActualDocument_Type")) == ".PDF" ? "showModalPreviewDocument" : "hideItem" %>'>
                                                                                <asp:Label ID="lblDocumentName" runat="server" Text='<%#Eval("ActualDocument_Name")%>'></asp:Label>

                                                                            </a>
                                                                            <asp:Label ID="lblName" runat="server" Text='<%#Eval("ActualDocument_Name")%>' Visible='<%#Convert.ToString(Eval("ActualDocument_Type")) == ".pdf" ? false : Convert.ToString(Eval("ActualDocument_Type")) == ".xls" ? false : Convert.ToString(Eval("ActualDocument_Type")) == ".xlsx" ? false : Convert.ToString(Eval("ActualDocument_Type")) == ".docx" ? false : Convert.ToString(Eval("ActualDocument_Type")) == ".doc" ? false : Convert.ToString(Eval("ActualDocument_Type")) == ".PDF" ? false : true %>'></asp:Label>
                                                                                <asp:Label ID="LblVersion" runat="server"></asp:Label>
                                                                        <%--<a href='/_modal_pages/preview-document.aspx?previewpath=<%#Eval("ActualDocument_Path").ToString().Replace('&','!')%>&ActualDocumentUID=<%#Eval("ActualDocumentUID")%>' class='<%#Convert.ToString(Eval("ActualDocument_Type")) == ".pdf" ? "showModalPreviewDocument" : "hideItem" %>'><%#Eval("ActualDocument_Name")%></a>
                                                                            <asp:Label ID="lblName" runat="server" Text='<%#Eval("ActualDocument_Name")%>' Visible='<%#Convert.ToString(Eval("ActualDocument_Type")) == ".pdf" ? false : Convert.ToString(Eval("ActualDocument_Type")) == ".xls" ? false : true %>'></asp:Label>
                                                                                <asp:Label ID="LblVersion" runat="server"></asp:Label>--%>
                                                                            </div>
                                                                        <img id="imgfolder" runat="server" src="/_assets/images/folder.jpg" height="15" width="20">
                                                                        
                                                                        <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandName="Folder_View" CommandArgument='<%# Container.DataItemIndex %>'
                                                                         Text="Edit"></asp:LinkButton>
                                                                            <%--</ContentTemplate>
                                                                          <Triggers>
                                                                            <asp:PostBackTrigger ControlID="lnkviewpdf" />
                                                                          </Triggers>
                                                                        </asp:UpdatePanel>--%>
                                                                                <%--<a class='<%#Convert.ToString(Eval("ActualDocument_Type")) == ".pdf" ? "view" : "hideItem" %>' target="_blank"  href='<%#Server.MapPath(Eval("ActualDocument_Path").ToString())%>'><%#Eval("ActualDocument_Name")%></a>--%>
                                                                                
                                                                             </ContentTemplate>
                                                                          
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                  <%-- <asp:BoundField DataField="ActualDocument_Version" HeaderText="Version">
                                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                                    </asp:BoundField>--%>
                                                                  <%-- <asp:BoundField DataField="Doc_Type" HeaderText="Document For">
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                    </asp:BoundField>--%>
                                                                   <asp:TemplateField HeaderText="Type">
                                                                    <ItemTemplate>
                                                                        <%#GetDocumentName(Eval("ActualDocument_Type").ToString())%>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                   <asp:BoundField DataField="ActualDocumentUID" HeaderText="Status">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                   <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                         <%--<a class="view" href="ViewDocument.aspx?DocID=<%#Eval("DocumentUID")%>">View</a>--%>
                                                                        <asp:UpdatePanel runat="server" ID="UpdatePanelDownload1" UpdateMode="Conditional" ChildrenAsTriggers="True">
                                                                        <ContentTemplate>
                                                                            <asp:LinkButton ID="LnkDownload_new" runat="server" CausesValidation="false" CommandArgument='<%#Eval("ActualDocumentUID")%>' CommandName="download">Download</asp:LinkButton>
                                                                            </ContentTemplate>
                                                                          <Triggers>
                                                                            <asp:PostBackTrigger ControlID="LnkDownload_new" />
                                                                          </Triggers>
                                                                        </asp:UpdatePanel>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                   <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                            <a class="<%#Convert.ToString(Eval("Doc_Type")) == "Cover Letter" ? "hideItem" : Convert.ToString(Eval("Doc_Type")) =="General Document" ? "hideItem" : "showModalDocumentHistory" %>" href="/_modal_pages/view-documenthistory.aspx?DocID=<%#Eval("ActualDocumentUID")%>&ProjectUID=<%#Eval("ProjectUID")%>&FlowUID=<%#Eval("FlowUID")%>">View/Update Status</a>
                                                                        </ItemTemplate>
                                                                       </asp:TemplateField>
                                                                    <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <a class="showModalDocumentMail" href="/_modal_pages/mail-documentlink.aspx?DocID=<%#Eval("ActualDocumentUID")%>&pUID=<%#Eval("ProjectUID")%>&wUID=<%#Eval("WorkPackageUID")%>"><span title="Mail" class="fas fa-envelope"></span></a>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                   <%--<asp:TemplateField ItemStyle-CssClass="hideItem" HeaderStyle-CssClass="hideItem">--%>
                                                                   <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <asp:UpdatePanel runat="server" ID="UpdatePanelCopy1" UpdateMode="Conditional" ChildrenAsTriggers="True">
                                                                        <ContentTemplate>
                                                                        <asp:LinkButton ID="lnkcopy1" runat="server" CausesValidation="false" CommandArgument='<%#Eval("ActualDocumentUID")%>' CommandName="copyfile"><span title="Copy" class="fas fa-copy"></span></asp:LinkButton>
                                                                             </ContentTemplate>
                                                                            <Triggers>
                                                                                <asp:PostBackTrigger ControlID="lnkcopy1" />
                                                                            </Triggers>
                                                                        </asp:UpdatePanel>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                   <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <a class="showModalDocumentEdit" href="/_modal_pages/edit-document.aspx?DocID=<%#Eval("ActualDocumentUID")%>&pUID=<%#Eval("ProjectUID")%>&wUID=<%#Eval("WorkPackageUID")%>"><span title="Edit" class="fas fa-edit"></span></a>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                   <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lnkactualdeletenew" runat="server" OnClientClick="return DeleteItem()" CausesValidation="false" CommandArgument='<%#Eval("ActualDocumentUID")%>' CommandName="delete"><span title="Delete" class="fas fa-trash"></span></asp:LinkButton>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                   <asp:BoundField DataField="ActualDocument_RelativePath" HeaderText="Path" ItemStyle-CssClass="hideItem" HeaderStyle-CssClass="hideItem">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="ActualDocument_DirectoryName" HeaderText="Path" ItemStyle-CssClass="hideItem" HeaderStyle-CssClass="hideItem">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="ActualDocumentUID" HeaderText="UID" ItemStyle-CssClass="hideItem" HeaderStyle-CssClass="hideItem">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="ActualDocument_Name" HeaderText="Name" ItemStyle-CssClass="hideItem" HeaderStyle-CssClass="hideItem">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                               </Columns>
                                                  <EmptyDataTemplate>
                                                        No Documents Found !
                                                 </EmptyDataTemplate>
                                       </asp:GridView>
                                </div>
                        </div>
               </div>
        </div>
        </ContentTemplate>  
       </asp:UpdatePanel>
    </div>

      <%--add Submittal modal--%>
    <div id="ModAddsubmittal" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-lg">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Add Submittal</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button" onclick="javascript:closepopupdocmsg();"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height: 480px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>

    <%--add Submittal modal--%>
    <div id="ModEditsubmittal" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-lg">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Edit Submittal</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button" onclick="javascript:closepopupdocmsg();"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height: 480px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>

       <%--add document modal--%>
    <div id="ModAddDocument" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-xl">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Add Document</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button" onclick="javascript:closepopupdoc();"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:500px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>

     <%--edit document modal--%>
    <div id="ModEditDocument" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-xl">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Edit Document</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:500px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>

    <%--View document histroy modal--%>
    <div id="ModViewDocumentHistory" class="modal it-modal fade">
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
    <%--Mail document  modal--%>
    <div id="ModDocumentMail" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-lg">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Send Document Link</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:180px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>
</asp:Content>
