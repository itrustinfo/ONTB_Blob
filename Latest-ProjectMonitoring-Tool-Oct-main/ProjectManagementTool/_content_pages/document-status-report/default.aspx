<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/default.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="ProjectManagementTool._content_pages.document_status_report._default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="default_master_head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="default_master_body" runat="server">
        <div class="container-fluid">
            <div class="row">
                <div class="col-md-12 col-lg-4 form-group">Document Status Report
                </div>
                <div class="col-md-6 col-lg-4 form-group">
                    <label class="sr-only" for="DDLProject">Project</label>
                    <div class="input-group">
                        <div class="input-group-prepend">
                            <span class="input-group-text">Project</span>
                        </div>
                        <asp:DropDownList ID="DDlProject" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="DDlProject_SelectedIndexChanged"></asp:DropDownList>
                    </div>
                </div>
                <div class="col-md-6 col-lg-4 form-group">
                    <label class="sr-only" for="DDLWorkPackage">Work Package</label>
                    <div class="input-group">
                        <div class="input-group-prepend">
                            <span class="input-group-text">Work Package</span>
                        </div>
                        <asp:DropDownList ID="DDLWorkPackage" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="DDLWorkPackage_SelectedIndexChanged"></asp:DropDownList>
                    </div>
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
                              <h6 class="text-muted">
                                   <asp:Label ID="ActivityHeading" CssClass="text-uppercase font-weight-bold" runat="server" Text="Document Status Report" />
                                       
                               </h6>
                              <div>
                                    <asp:Button ID="btnDocumentSummaryExportExcel" runat="server" Text="Export Excel" Visible="true" CssClass="btn btn-primary" OnClick="btnDocumentSummaryExportExcel_Click"/>
                                    <asp:Button ID="btnDocumentSummaryExportPDF" runat="server" Text="Export PDF" Visible="false" CssClass="btn btn-primary" />
                                    <asp:Button ID="btnDocumentSummaryPrint" runat="server" Text="Print" Visible="false" CssClass="btn btn-primary"/>
                                </div>
                          </div>
                            </div>
                        <div class="table-responsive" style="height:700px;overflow:scroll">
                                <asp:GridView ID="GrdDocumentStatusReport" runat="server" CssClass="table table-bordered" EmptyDataText="No Data" AutoGenerateColumns="False" Width="100%" OnDataBound="GrdDocumentStatusReport_DataBound">
                                <Columns>
                                      <asp:BoundField DataField="SlNo" HeaderText="Sl.No" >
                                    <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                      <%--<asp:BoundField DataField="TaskName" HeaderText="Drawing/Documents" >
                                    <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>--%>
                                    <asp:BoundField DataField="SubmittalName" HeaderText="Submittal/Task Name" >
                                    <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="DocumentName" HeaderText="Document Name" >
                                    <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <%--<asp:BoundField DataField="DocumentName" HeaderText="Document Name" >
                                    <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>--%>
                                    <asp:BoundField DataField="DateSubmission" HeaderText="Date of Submission" >
                                    <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="CodeStatus" HeaderText="Code" >
                                    <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="DateApprovedbyPMC" HeaderText="Date - Approved by PMC" >
                                    <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="DateApprovedbyBWSSB" HeaderText="Date - Approved by BWSSB" >
                                    <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>

                                    <asp:BoundField DataField="DateSubmissionB" HeaderText="Date of Submission" >
                                    <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="CodeStatusB" HeaderText="Code" >
                                    <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="DateApprovedbyPMCB" HeaderText="Date - Approved by PMC" >
                                    <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="DateApprovedbyBWSSBB" HeaderText="Date - Approved by BWSSB" >
                                    <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>

                                    <asp:BoundField DataField="DateSubmissionC" HeaderText="Date of Submission" >
                                    <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="CodeStatusC" HeaderText="Code" >
                                    <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="DateApprovedbyPMCC" HeaderText="Date - Approved by PMC" >
                                    <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="DateApprovedbyBWSSBC" HeaderText="Date - Approved by BWSSB" >
                                    <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>

                                    <asp:BoundField DataField="DateSubmissionD" HeaderText="Date of Submission" >
                                    <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="CodeStatusD" HeaderText="Code" >
                                    <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="DateApprovedbyPMCD" HeaderText="Date - Approved by PMC" >
                                    <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="DateApprovedbyBWSSBD" HeaderText="Date - Approved by BWSSB" >
                                    <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>

                                    <asp:BoundField DataField="DateSubmissionE" HeaderText="Date of Submission" >
                                    <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="CodeStatusE" HeaderText="Code" >
                                    <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="DateApprovedbyPMCE" HeaderText="Date - Approved by PMC" >
                                    <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="DateApprovedbyBWSSBE" HeaderText="Date - Approved by BWSSB" >
                                    <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>

                                    <asp:BoundField DataField="DateSubmissionF" HeaderText="Date of Submission" >
                                    <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="CodeStatusF" HeaderText="Code" >
                                    <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="DateApprovedbyPMCF" HeaderText="Date - Approved by PMC" >
                                    <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="DateApprovedbyBWSSBF" HeaderText="Date - Approved by BWSSB" >
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
</asp:Content>
