<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/default.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="ProjectManagementTool._content_pages.report_user_status1._default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="default_master_head" runat="server">
    <style type="text/css">
        .hideItem {
            display: none;
        }
    </style>

     <link rel="stylesheet" href="//code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">
  <script src="https://code.jquery.com/jquery-1.12.4.js"></script>
  <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>

     <script type="text/javascript">

         function checkMsg() {
             var IncomingReceDate = document.getElementById('<%=dtFrom.ClientID%>').value;
             var DocumentDate = document.getElementById('<%=dtTo.ClientID%>').value;
             if (IncomingReceDate != "" && DocumentDate != "") {
                 var msg = document.getElementById("default_master_body_divMsgdata");
                 msg.style.display = "block";
             }
         }

         $(function () {
             $("input[id$='dtFrom']").datepicker({
                 changeMonth: true,
                 changeYear: true,
                 dateFormat: 'dd/mm/yy'
             });
         });

         $(function () {
             $("input[id$='dtTo']").datepicker({
                 changeMonth: true,
                 changeYear: true,
                 dateFormat: 'dd/mm/yy'
             });
         });

     </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="default_master_body" runat="server">
    <div class="container-fluid">
          <div class="row">
                <div class="col-md-12 col-lg-4 form-group">User Status Report</div>
              
            </div>
    </div>

    <div class="container-fluid" id="divTabular" runat="server" >
        <div class="row">
            <div class="col-lg-12 col-xl-12 form-group">
                    <div class="card mb-4">
                        <div class="card-body">
                            <div class="col-lg-12 col-xl-12 form-group" align="center">
                                <h5 id="ReportName" style="font-weight: bold;" runat="server">User Status Report</h5>
                            </div>
                            <div class="card-title">
                                <div class="d-flex justify-content-between">
                                   <%-- <h6 class="text-muted">
                                    </h6>--%>
                                    <div class="col-3 form-group">
                                        <label class="sr-only" for="DDlUser">PMC User</label>
                    <div class="input-group">
                        <div class="input-group-prepend">
                            <span class="input-group-text">PMC User</span>
                        </div>
                        <asp:DropDownList ID="DDlUser" runat="server" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="DDlUser_SelectedIndexChanged"></asp:DropDownList>
                                  
                    </div>
                                        
                                      </div>
                                     <div class="col-3 form-group">
                                       <%-- <label class="sr-only" for="DDLWorkPackage">Select Achieved Date</label>--%>
                                        <div class="input-group">
                                            <div class="input-group-prepend">
                                                <span class="input-group-text">From Date</span>
                                            </div>
                                            <asp:TextBox ID="dtFrom" runat="server" placeholder="dd/mm/yyyy" autocomplete="off" CssClass="form-control" required AutoPostBack="false" OnTextChanged="dtFromDate_TextChanged"></asp:TextBox>
                                        </div>
                                    </div>
                                     <div class="col-3 form-group">
                                       <%-- <label class="sr-only" for="DDLWorkPackage">Select Achieved Date</label>--%>
                                        <div class="input-group">
                                            <div class="input-group-prepend">
                                                <span class="input-group-text">To Date</span>
                                            </div>
                                            <asp:TextBox ID="dtTo" runat="server" placeholder="dd/mm/yyyy" autocomplete="off" CssClass="form-control" required AutoPostBack="false" OnTextChanged="dtToDate_TextChanged"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div>
                                        <asp:Button ID="btnRefresh" runat="server" Text="Submit" CssClass="btn btn-primary" Visible="true" OnClick="btnRefresh_Click" OnClientClick="javascript:checkMsg();"></asp:Button>&nbsp;
                                        <asp:Button ID="btnExcel" runat="server" Text="Export to Excel" CssClass="btn btn-primary" Visible="true" OnClick="btnExcel_Click" />&nbsp;
                                        <asp:Button ID="btnPDF" runat="server" Text="Export to PDF" CssClass="btn btn-primary" Visible="false" OnClick="btnPDF_Click" />&nbsp;
                                        <asp:Button ID="tnPrint" runat="server" Text="Print" CssClass="btn btn-primary" Visible="false" OnClick="tnPrint_Click" />
                                    </div>
                                </div>
                            </div>
                            <div class="table-responsive">
                                <div id="divMsgdata" runat="server" style="color:red;display:none;text-align:center">Please wait ...processing data...this may take some time</div>
                                <div style="overflow: scroll;" onscroll="OnScrollDivNew(this)" id="DivMainContentNew">
                                        <asp:GridView ID="grdDataList" runat="server" AutoGenerateColumns="false" CellPadding="6" CellSpacing="16"  HeaderStyle-BackColor="#666666" HeaderStyle-ForeColor="White" 
                                            CssClass="table table-bordered" OnRowDataBound="grdDataList_RowDataBound" OnDataBound="GrdDataList_DataBound">
                                            <Columns>
                                                 <asp:BoundField DataField="ProjectName"  HeaderText="Project" ItemStyle-CssClass="hideItem" HeaderStyle-CssClass="hideItem">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="ProjectNameDisplay"  HeaderText="Project" >
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="DocumentType"  HeaderText="Document Type" >
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:TemplateField  HeaderText="Approved so far" >
                                                    <ItemTemplate>
                                                        <a class="showModalDocumentView" href="/_content_pages/report-mis-user-details?ProjectName=<%#Eval("ProjectName")%>&FlowName=<%#Eval("DocumentType")%>&type=ApprovedSofar&user_id=" target="_blank"><%#Eval("ApprovedSofar")%></a>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                 <asp:BoundField DataField="PendingAsOnDate"  HeaderText="Pending as on to date" >
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                 <asp:BoundField DataField="NoResponse"  HeaderText="Did not respond" >
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
    </div>

</asp:Content>



