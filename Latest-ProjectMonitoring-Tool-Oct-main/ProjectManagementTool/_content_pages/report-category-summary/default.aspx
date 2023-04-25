<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/default.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="ProjectManagementTool._content_pages.report_category_summary._default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="default_master_head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="default_master_body" runat="server">
    <div class="container-fluid">
        <div class="row">
            <div class="col-md-12 col-lg-3 form-group">Document Category Summary Report</div>
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
           
        </div>
    </div>



    
    <div class="container-fluid" id="DivDocumentSummary" runat="server">
        <div class="row">
            <div class="col-lg-12 col-xl-12 form-group">
                <div class="card mb-4">
                    <div class="card-body">
                        <div class="col-lg-12 col-xl-12 form-group" align="center">
                            <h5 id="DocumentSummaryReportName" style="font-weight: bold;" runat="server">Category Summary</h5>
                            <h5 id="DocumentSummaryProjectName" runat="server">Project Name</h5>
                        </div>
                        <div class="card-title">
                            <div class="d-flex justify-content-between">
                                <h6 class="text-muted">
                                    <%--<asp:Label ID="LblDocumentSummary" CssClass="text-uppercase font-weight-bold" runat="server" Text="Status of Documents submitted by the Contractor"  />--%>
                                      
                                </h6>
                                <div>
                                    <asp:Button ID="btnDocumentSummaryExportExcel" runat="server" Text="Export Excel" Visible="false" CssClass="btn btn-primary" OnClick="btnDocumentSummaryExportExcel_Click" />
                                 
                                </div>
                            </div>
                        </div>
                        <div class="table-responsive">
                            <asp:GridView ID="GrdInvoiceRaBillsDeductions" EmptyDataText="No Documents Found." HeaderStyle-HorizontalAlign="Center" runat="server" Width="100%" 
                                
                                 CssClass="table table-bordered">


<HeaderStyle HorizontalAlign="Left"></HeaderStyle>


                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>



</asp:Content>

