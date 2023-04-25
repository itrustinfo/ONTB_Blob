<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/default.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="ProjectManagementTool._content_pages.report_suez_physical_progress._default" %>


<asp:Content ID="Content1" ContentPlaceHolderID="default_master_head" runat="server">
      <style type="text/css">
               .div {
  padding-top: 0px;
  padding-right: 200px;
  padding-bottom: 0px;
  padding-left: 10px;
  width:100% !important;
  overflow-x:scroll;
  overflow-y: hidden;
}

              

               .chklist{
                   padding-left :10px;
               }

              .container {
         /*  position:fixed;*/
         
   
            width:100%; 
            height:100%;
            overflow:hidden;
        }
        .container img {
          position:absolute;
             
    top:0; 
    left:0; 
    right:0; 
    bottom:0; 
           /* margin:auto; */
           /* min-width:100%;
            min-height:100%;*/
            overflow: hidden;
        }



            #chart_div .google-visualization-tooltip {  position:relative !important; top:0 !important;right:0 !important; z-index:+1;} 
        </style>
        <script type="text/javascript" src="https://www.gstatic.com/charts/loader.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="default_master_body" runat="server">
    <div class="container-fluid" id="divMonthly" runat="server">
       <div class="row">
            <div class="col-md-12 col-lg-4 form-group">Project Physical Progress Custom Report</div>
           </div>
        <div class="row">
            <div class="col-md-6 col-lg-3 form-group"> <asp:RadioButtonList CssClass="form-control" ID="RBLReport" BorderStyle="None" runat="server" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="RBLReport_SelectedIndexChanged">
                                <asp:ListItem Value="0" Selected="True">&nbsp;By Month</asp:ListItem>  
                                <asp:ListItem Value="1">&nbsp;OverAll</asp:ListItem>
                <asp:ListItem Value="2">&nbsp;OverAll Graph</asp:ListItem> 
                               
                        </asp:RadioButtonList></div>
            <div class="col-md-6 col-lg-3 form-group">
                <label class="sr-only" for="DDLProject">Project</label>
                <div class="input-group">
                    <div class="input-group-prepend">
                        <span class="input-group-text">Project</span>
                    </div>
                    <asp:DropDownList ID="DDlProject" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="DDlProject_SelectedIndexChanged"></asp:DropDownList>
                </div>
            </div>
            <div class="col-md-6 col-lg-3 form-group">
                <label class="sr-only" for="DDLWorkPackage">Work Package</label>
                <div class="input-group">
                    <div class="input-group-prepend">
                        <span class="input-group-text">Work Package</span>
                    </div>
                    <asp:DropDownList ID="DDLWorkPackage" runat="server" CssClass="form-control"></asp:DropDownList>
                </div>
            </div>
            <div class="col-md-6 col-lg-2 form-group" id="DivForMonthly" runat="server">
                <label class="sr-only" for="DDLWorkPackage">Select Month</label>
                <div class="input-group">
                    <div class="input-group-prepend">
                        <span class="input-group-text">Month</span>
                    </div>
                    <asp:DropDownList ID="ddlMonth" runat="server" CssClass="form-control"></asp:DropDownList>
                </div>
            </div>
            <div class="col-md-6 col-lg-1 form-group">
                <asp:Button ID="btnSubmit" runat="server" CssClass="btn btn-primary" Text="Submit" OnClick="btnSubmit_Click" />
            </div>
        </div>




        <div class="container-fluid" id="divTabular" runat="server" visible="false">
            <div class="row">
                <div class="col-lg-12 col-xl-12 form-group">
                    <div class="card mb-4">
                        <div class="card-body">
                            <div class="card-title">
                                <div class="d-flex justify-content-between">
                                    <h6 class="text-muted">
                                        <%--<asp:Label ID="ActivityHeading" CssClass="text-uppercase font-weight-bold" runat="server" Text="Contract Details" />--%>
                                    </h6>
                                    <div>
                                        <asp:Button ID="btnExcel" runat="server" Text="Export to Excel" CssClass="btn btn-primary" Visible="true" OnClick="btnExcel_Click" />&nbsp;
                                        <%--<asp:Button ID="btnPDF" runat="server" Text="Export to PDF" CssClass="btn btn-primary" Visible="true" OnClick="btnPDF_Click" />&nbsp;
                                        <asp:Button ID="tnPrint" runat="server" Text="Print" CssClass="btn btn-primary" Visible="true" OnClick="tnPrint_Click" />--%>
                                    </div>
                                </div>
                            </div>
                            <div class="table-responsive">
                                <div id="divsummary" runat="server" visible="true">
                                    <div style="overflow: hidden;" id="DivHeaderRowNew"></div>
                                    <div style="overflow: scroll;" onscroll="OnScrollDivNew(this)" id="DivMainContentNew">
                                        <asp:GridView ID="grdDataList" EmptyDataText="No Data Found." runat="server" Width="100%" CellPadding="6" CellSpacing="16" HeaderStyle-BackColor="#666666" HeaderStyle-ForeColor="White" AlternatingRowStyle-BackColor="#F5F5F5"
                                            CssClass="table table-bordered">
                                         
                                        </asp:GridView>
                                    </div>
                                    <div id="DivFooterRowNew" style="overflow: hidden"></div>
                                </div>
                                <div class="card h-100" id="divGraph" runat="server" visible="false">
                        <div class="card-body">
                            <asp:Literal ID="ltScript_deployment" runat="server" Visible="true"></asp:Literal>
                               <div id="chart_div" style="width:100%; height:325px; overflow-y: auto; overflow-x:scroll;"></div>
                               
                        </div>
                    </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>



    </div>
</asp:Content>
