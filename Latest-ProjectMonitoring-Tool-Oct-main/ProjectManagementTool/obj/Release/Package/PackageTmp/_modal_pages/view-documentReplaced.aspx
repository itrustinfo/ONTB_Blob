<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/modal.Master" AutoEventWireup="true" CodeBehind="view-documentReplaced.aspx.cs" Inherits="ProjectManagementTool._modal_pages.view_documentReplaced" %>
<asp:Content ID="Content1" ContentPlaceHolderID="modal_master_head" runat="server">
    <style type="text/css">
        .label {
  min-width: 200px !important;
  display: inline-block !important
}
    </style>

    <script type="text/javascript">
 $( function() {
    $("input[id$='dtIncomingRevDate']").datepicker({
      changeMonth: true,
        changeYear: true,
      dateFormat:'dd/mm/yy'
      });

      $("input[id$='dtDocumentDate']").datepicker({
      changeMonth: true,
        changeYear: true,
      dateFormat:'dd/mm/yy'
      });
    });
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="modal_master_body" runat="server">
     <form id="frmViewDocumentReplacedModal" runat="server">
        <div class="container-fluid" style="max-height:85vh; overflow-y:auto;">
            <div class="row">
                <div class="col-sm-6">
                    <div class="form-group">
                        <label class="lblCss" for="txtDocumentName">Submittal Name</label>
                         <asp:Label ID="lblDocumentName" runat="server" Text="Label" CssClass="form-control"></asp:Label>
                        <%--<asp:TextBox ID="txtDocumentName" runat="server" CssClass="form-control" BackColor="White" Enabled="false" ></asp:TextBox>--%>
                    </div>
                    
                   
                    <div class="form-group">
                        <label class="lblCss" for="txtdesc">ONTB Ref.No</label> 
                        <%--<asp:Label ID="lblDescription" runat="server" Text="Label"  CssClass="form-control"></asp:Label>--%>
                        <asp:TextBox ID="txtONTBRefNo" runat="server" CssClass="form-control" BackColor="White" Enabled="false" ></asp:TextBox>
                    </div>
                   
                    <div class="form-group">
                        <label class="lblCss" for="txtprefnumber">Old Document</label>
                        <asp:Label ID="lblOldDocument" runat="server" Text="Label" CssClass="form-control"></asp:Label>
                    </div>
                   
                    <div class="form-group">
                        <label class="lblCss" for="txtprefnumber">Old Cover Letter</label>
                        <asp:Label ID="lblOldCoverLetter" runat="server" Text="Label" CssClass="form-control"></asp:Label>
                    </div>
                    
                     <div class="form-group" id="IncomingReceDate" runat="server">
                        <label class="lblCss" for="DatIncomingRevisionDate">Incoming Received Date<asp:Label ID="LblDateText" runat="server"></asp:Label>

                        </label> &nbsp;<span style="color:red; font-size:1.2rem;">*</span>
                        <asp:TextBox ID="dtIncomingRevDate" CssClass="form-control" required runat="server" placeholder="dd/mm/yyyy" autocomplete="off" onchange="CheckDate()" ClientIDMode="Static"></asp:TextBox>
                    </div>
                     <div class="form-group" id="OriginatorNumber" runat="server">
                        <label class="lblCss" for="txtRefNumber">Originator Ref.Number</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                        <asp:TextBox ID="txtRefNumber" CssClass="form-control" autocomplete="off" runat="server" required ClientIDMode="Static"></asp:TextBox>
                        
                    </div>
                    
                </div>

                <div class="col-sm-6">
                    
                     <div style="height:87px"></div>
                    <div class="form-group">
                        <label class="lblCss" for="txtdesc">Description</label> 
                        <%--<asp:Label ID="lblDescription" runat="server" Text="Label"  CssClass="form-control"></asp:Label>--%>
                        <asp:TextBox ID="txtDesc" runat="server" CssClass="form-control" BackColor="White" Enabled="false" ></asp:TextBox>
                    </div>
                     <div class="form-group">
                        <label class="lblCss" for="FileUploadDoc">New Document</label>
                        <div class="custom-file">
                            <asp:FileUpload ID="FileUploadDoc" runat="server" CssClass="custom-file-input" AllowMultiple="false"/>
                            <label class="custom-file-label" for="FileUploadDoc">Choose document</label>
                        </div>
                    </div>
                    
                     <div class="form-group">
                        <label class="lblCss" for="FileUploadCover">New Cover Letter</label>
                        <div class="custom-file">
                            <asp:FileUpload ID="FileUploadCover" runat="server" CssClass="custom-file-input" AllowMultiple="false"/>
                            <label class="custom-file-label" for="FileUploadCover">Choose Cover Letter</label>
                        </div>
                    </div>
                   
                   
                    <div class="form-group" id="DocumentDate" runat="server">
                        <label class="lblCss" for="dtDocumentDate">Document Date</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                        <asp:TextBox ID="dtDocumentDate" CssClass="form-control" required runat="server" placeholder="dd/mm/yyyy" autocomplete="off" onchange="CheckDate()" ClientIDMode="Static"></asp:TextBox>
                    </div>
                </div>
              
            </div> 
        </div>
            <div class="modal-footer">
                    <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-primary" OnClick="btnSubmit_Click"/>
            </div>
    </form>
</asp:Content>
