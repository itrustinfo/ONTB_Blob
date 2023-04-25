<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/default.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="ProjectManagementTool._content_pages.design_and_drawing_works_dwg_issue._default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="default_master_head" runat="server">
    <style type="text/css">
        .hideItem {
            display: none;
        }

        .ChkBoxClass input {width:22px; height:22px; margin-left: 20px;}

        .cbCopy label{width:200px;display:inline-block;vertical-align:top}
    </style>
    <script type="text/javascript">
        function CopyMessage() {
            if (confirm("Are you sure you want to Copy ...?")) {
                return true;
            }
            return false;
        }

        function DeleteItem() {
            if (confirm("Are you sure you want to delete ...?")) {
                return true;
            }
            return false;
        }
    </script>
    <script>
        $(document).ready(function () {
            $(".showModalAddPhysicalProgress").click(function (e) {
                e.preventDefault();
                var url = $(this).attr("href");
                $("#ModAddPhysicalProgress iframe").attr("src", url);
                $("#ModAddPhysicalProgress").modal("show");
            });
            $(".showModalEditPhysicalProgress").click(function (e) {
                e.preventDefault();
                var url = $(this).attr("href");
                $("#ModEditPhysicalProgress iframe").attr("src", url);
                $("#ModEditPhysicalProgress").modal("show");
            });
        });
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="default_master_body" runat="server">
    <div class="container-fluid">
        <div class="row">
            <div class="col-md-6 col-lg-4 form-group">Design and Drawing Works B TT</div>
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
                    <asp:DropDownList ID="DDLWorkPackage" runat="server" CssClass="form-control"></asp:DropDownList>
                </div>
            </div>

        </div>

    </div>
    <div class="container-fluid">
        <div class="row">
            <div class="col-lg-6 col-xl-12 form-group">
                <div class="card mb-4">
                    <div class="card-body">
                        <div class="card-title">
                            <div class="d-flex justify-content-between">
                                <h6 class="text-muted">
                                    <asp:Label Text="Design and Drawing Works DWG Issue" CssClass="text-uppercase font-weight-bold" runat="server" />
                                    <%--<asp:Label Text="Foo bar" CssClass="pl-1" runat="server" />--%>
                                </h6>
                                <div>
                                    <a href="/_modal_pages/add-Design_and_drawing_works_dwg_issue.aspx" id="addDailyProgress" runat="server" class="showModalAddPhysicalProgress">
                                        <asp:Button ID="btnadd" runat="server" Text="+ Add Design and Drawing Works DWG Issue" CssClass="btn btn-primary"></asp:Button></a>
                                </div>
                            </div>
                        </div>
                        <div class="table-responsive">
                            <label class="lblCss" for="txtxsummary">Select Design and Drawing Works DWG Issue Master</label>
                            <asp:DropDownList ID="DDLDailyReportMaster" runat="server" CssClass="form-control" Width="30%" AutoPostBack="true" OnSelectedIndexChanged="DDLDailyReportMaster_SelectedIndexChanged">
                            </asp:DropDownList>
                            <br />

                            <asp:Button ID="btncopy" runat="server" Text="Copy Data from Previous Report" CssClass="btn btn-primary" OnClientClick="return CopyMessage()" Visible="false" OnClick="btncopy_Click" />
                            <asp:CheckBox ID="cbCopy" CssClass="ChkBoxClass" Text="Do you want to copy the previous total up to date quantity to this previous qty" runat="server" />
                            <br />
                            <br />
                            <asp:GridView ID="GrdPhysicalProgress" runat="server" FooterStyle-Font-Bold="true" CssClass="table table-bordered" AutoGenerateColumns="False" EmptyDataText="No Data" Width="100%" OnRowDataBound="GrdPhysicalProgress_RowDataBound" onrowcommand="grdResult_RowCommand" OnRowDeleting="GrdDailyProgress_RowDeleting">
                                <Columns>
                                    <asp:BoundField DataField="UID" HeaderText="UID" ItemStyle-CssClass="hideItem" HeaderStyle-CssClass="hideItem">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Zone Name" FooterStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtZoneName" runat="server" CssClass="form-control" Text='<%# Bind("ZoneName") %>' required></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Location" FooterStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtLocation" runat="server" CssClass="form-control" Text='<%# Bind("Location") %>' required></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Submitted By contractor" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtsubmittedbycontractor" runat="server" CssClass="form-control" Text='<%# Bind("submittedbycontrator") %>' required></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Submitted Date(dd-mm-yyyy)">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtsubmitteddate" runat="server" CssClass="form-control" Text='<%# Bind("submitteddate") %>'  required></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                      <asp:TemplateField HeaderText="GFC Released by ONTB" FooterStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtapproverbyontb" runat="server" CssClass="form-control" Text='<%# Bind("approverbyontb") %>'  AutoPostBack="true" required></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                   
                                    <asp:TemplateField HeaderText="Date Approved By ONTB" FooterStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtapproverdate" runat="server" CssClass="form-control" Text='<%# Bind("approverdate") %>'  AutoPostBack="true" required></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="GFC Approved by bwssb" FooterStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtGFCApprovedByBWSSB" runat="server" CssClass="form-control" Text='<%# Bind("GFCApprovedByBWSSB") %>'  AutoPostBack="true" required></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                   
                                    <asp:TemplateField HeaderText="GFC Approved date" FooterStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtGFCApprovedDate" runat="server" CssClass="form-control" Text='<%# Bind("GFCApprovedDate") %>'  AutoPostBack="true" required></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                   
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkactualdelete" runat="server" OnClientClick="return DeleteItem()" CausesValidation="false" CommandArgument='<%#Eval("UID")%>' CommandName="delete"><span title="Delete" class="fas fa-trash"></span></asp:LinkButton>
                                            
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <div class="text-right">
                                <asp:Button ID="btnexport" runat="server" Visible="false" Text="Export to PDF" OnClick="btnexport_Click" CssClass="btn btn-primary" />
                            </div>
                            <div class="text-right">
                                <asp:Button ID="btnSubmit" runat="server" CssClass="btn btn-primary" Text="Submit" OnClick="btnAdd_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <%--Add Physical Progress--%>
    <div id="ModAddPhysicalProgress" class="modal it-modal fade">
        <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-xl">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">Add Design and Drawing Works DWG Issue</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
                </div>
                <div class="modal-body">
                    <iframe class="border-0 w-100" style="height: 480px;" loading="lazy"></iframe>
                </div>
            </div>
        </div>
    </div>

    <%--add work package category modal--%>
    <div id="ModEditPhysicalProgress" class="modal it-modal fade">
        <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-xl">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">Edit Project Physical Progess</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
                </div>
                <div class="modal-body">
                    <iframe class="border-0 w-100" style="height: 480px;" loading="lazy"></iframe>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

