<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/modal.Master" AutoEventWireup="true" CodeBehind="add-treenodemaster.aspx.cs" Inherits="ProjectManagementTool._modal_pages.add_treenodemaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="modal_master_head" runat="server">
    <script type="text/javascript">
        $(function () {
            $("input[id$='dtPaymentDate']").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: 'dd/mm/yy'
            });
        });
</script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="modal_master_body" runat="server">

    <form id="AddMaster" runat="server">
        <asp:ScriptManager EnablePartialRendering="true"
            ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <div class="container-fluid" style="min-height: 82vh; overflow-y: auto; max-height: 82vh;">

            <div class="row">
                <asp:HiddenField ID="HiddenPaging" runat="server" />
                <%--<div class="col-md-12 col-lg-4 form-group">Documents View</div>--%>
                <div class="col-md-6 col-lg-4 form-group">
                    <label class="sr-only" for="DDLProject">Project</label>
                    <div class="input-group">
                        <div class="input-group-prepend">
                            <span class="input-group-text">Project</span>
                        </div>
                        <asp:DropDownList ID="DDlProject" runat="server" CssClass="form-control" AutoPostBack="true" Enabled="false"></asp:DropDownList>
                        <%--View document modal--%>
                    </div>
                </div>
                <div class="col-md-6 col-lg-4 form-group" id="divWP" runat="server">
                    <label class="sr-only" for="DDLWorkPackage">Work Package</label>
                    <div class="input-group">
                        <div class="input-group-prepend">
                            <span class="input-group-text">Work Package</span>
                        </div>
                        <asp:DropDownList ID="DDLWorkPackage" runat="server" CssClass="form-control" Enabled="false"></asp:DropDownList>

                        <%--edit document modal--%>
                    </div>
                </div>
            </div>

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
                                    CssClass="text-muted text-uppercase font-weight-bold" CellPadding="4" RepeatDirection="Horizontal" OnSelectedIndexChanged="RBOptionList_SelectedIndexChanged">
                                </asp:RadioButtonList>
                            </div>


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
                                        <div class="row">
                                            <div class="col-md-12 col-lg-4 form-group" id="divsubmit" runat="server">

                                                <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-primary" OnClick="btnSubmit_Click" />

                                                <br />
                                                <asp:Label ID="LblMessage" runat="server" Font-Bold="true"></asp:Label>
                                            </div>
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




    </form>

</asp:Content>
