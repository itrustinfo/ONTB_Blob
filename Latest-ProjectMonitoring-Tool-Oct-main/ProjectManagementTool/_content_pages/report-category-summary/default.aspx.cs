using iTextSharp.text;
using iTextSharp.text.pdf;
using ProjectManagementTool.DAL;
using ProjectManager.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManagementTool._content_pages.report_category_summary
{
    public partial class _default : System.Web.UI.Page
    {
        DBGetData getdt = new DBGetData();
        TaskUpdate gettk = new TaskUpdate();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Username"] == null)
            {
                Response.Redirect("~/Login.aspx");
            }
            else
            {
                if (!IsPostBack)
                {
                    BindProject();                   
                }
            }
        }

        private void BindProject()
        {
            DataTable ds = new DataTable();
            if (Session["TypeOfUser"].ToString() == "U" || Session["TypeOfUser"].ToString() == "MD" || Session["TypeOfUser"].ToString() == "VP")
            {
                ds = gettk.GetProjects();
            }
            else if (Session["TypeOfUser"].ToString() == "PA")
            {
                ds = gettk.GetAssignedProjects_by_UserUID(new Guid(Session["UserUID"].ToString()));
            }
            else
            {
                ds = gettk.GetAssignedProjects_by_UserUID(new Guid(Session["UserUID"].ToString()));
            }

            DDlProject.DataTextField = "ProjectName";
            DDlProject.DataValueField = "ProjectUID";
            DDlProject.DataSource = ds;
            DDlProject.DataBind();

            DDlProject.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", ""));
            DDLWorkPackage.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", ""));
        }

        protected void DDlProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDlProject.SelectedValue != "")
            {
                //DataSet ds = getdt.GetWorkPackages_By_ProjectUID(new Guid(DDlProject.SelectedValue));
                DataSet ds = new DataSet();
                if (Session["TypeOfUser"].ToString() == "U" || Session["TypeOfUser"].ToString() == "MD" || Session["TypeOfUser"].ToString() == "VP")
                {
                    ds = getdt.GetWorkPackages_By_ProjectUID(new Guid(DDlProject.SelectedValue));
                }
                else if (Session["TypeOfUser"].ToString() == "PA")
                {
                    ds = getdt.GetWorkPackages_ForUser_by_ProjectUID(new Guid(Session["UserUID"].ToString()), new Guid(DDlProject.SelectedValue));
                }
                else
                {
                    ds = getdt.GetWorkPackages_ForUser_by_ProjectUID(new Guid(Session["UserUID"].ToString()), new Guid(DDlProject.SelectedValue));
                }
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DDLWorkPackage.DataTextField = "Name";
                    DDLWorkPackage.DataValueField = "WorkPackageUID";
                    DDLWorkPackage.DataSource = ds;
                    DDLWorkPackage.DataBind();

                    BindDocumentSummary();
                }
            }

        }

        protected void DDLWorkPackage_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDLWorkPackage.SelectedValue != "")
            {
                BindDocumentSummary();
            }
        }
       
        protected void BindDocumentSummary()
        {
            Guid projectUid = Guid.Empty, workPackageUid = Guid.Empty;
            if (DDlProject.SelectedValue != "")
            {
                projectUid = new Guid(DDlProject.SelectedValue);
            }
            if (DDLWorkPackage.SelectedValue != "")
            {
                workPackageUid = new Guid(DDLWorkPackage.SelectedValue);
            }

            DataTable dt = new DataTable();
            dt = getdt.Getdocument_Category_Summary(projectUid);
            if(dt!= null && dt.Rows.Count > 0)
            {
               
                btnDocumentSummaryExportExcel.Visible = true;
                DocumentSummaryReportName.InnerHtml = "Report Name : Document Category Summary Report";
                DocumentSummaryProjectName.InnerHtml = "Project Name : " + DDlProject.SelectedItem.Text + " (" + DDLWorkPackage.SelectedItem.Text + ")";

                ViewState["Export"] = "1";

               

                dt.Columns.Add("SNo", typeof(int)).SetOrdinal(0);
                dt.Columns.Add("Total Submitted").SetOrdinal(2); ;
                DataRow dr = dt.NewRow();
                dr[1] = "Total";
              
                int columnTotal = 0;
                int rowTotal = 0;
                int Grantotal = 0;
                for (int i= 0;i < dt.Rows.Count;i++)
                {
                    try
                    {
                        dt.Rows[i][0] = i + 1;
                        columnTotal = 0;
                        for (int cnt = 3; cnt < dt.Columns.Count; cnt++)
                        {

                            columnTotal += Convert.ToInt32(dt.Rows[i][cnt]);
                        }
                        rowTotal += columnTotal;
                       // dr[8] = rowTotal;
                        dt.Rows[i]["Total Submitted"] = columnTotal;
                        Grantotal += columnTotal;
                    }
                    catch(Exception ex)
                    {

                    }
                    
                }
                for (int cnt = 3; cnt < dt.Columns.Count; cnt++)
                {
                    dr[cnt] = dt.AsEnumerable().Sum(row => row.Field<Int32>(dt.Columns[cnt].ToString()));

                }
                dr[2] = Grantotal.ToString();
                dt.Rows.Add(dr);
                GrdInvoiceRaBillsDeductions.DataSource = dt;
                GrdInvoiceRaBillsDeductions.DataBind();
            }
            else
            {
                GrdInvoiceRaBillsDeductions.DataSource = null;
                GrdInvoiceRaBillsDeductions.DataBind();
                btnDocumentSummaryExportExcel.Visible = false;
            }

        }

        //protected void BindDocumentSummary()
        //{
        //    Guid projectUid = Guid.Empty, workPackageUid = Guid.Empty;
        //    if (DDlProject.SelectedValue != "")
        //    {
        //        projectUid = new Guid(DDlProject.SelectedValue);
        //    }
        //    if (DDLWorkPackage.SelectedValue != "")
        //    {
        //        workPackageUid = new Guid(DDLWorkPackage.SelectedValue);
        //    }

        //    DataTable dt = new DataTable();
        //    dt = getdt.Getdocument_Category_Summary(projectUid);
        //    if (dt != null && dt.Rows.Count > 0)
        //    {

        //        //btnDocumentSummaryExportExcel.Visible = true;
        //        //DocumentSummaryReportName.InnerHtml = "Report Name : Document Category Summary Report";
        //        //DocumentSummaryProjectName.InnerHtml = "Project Name : " + DDlProject.SelectedItem.Text + " (" + DDLWorkPackage.SelectedItem.Text + ")";

        //        ViewState["Export"] = "1";



        //        dt.Columns.Add("SNo", typeof(int)).SetOrdinal(0);
        //        dt.Columns.Add("Total Submitted");
        //        DataRow dr = dt.NewRow();
        //        dr[1] = "Total";

        //        int columnTotal = 0;
        //        int rowTotal = 0;
        //        for (int i = 0; i < dt.Rows.Count; i++)
        //        {
        //            try
        //            {
        //                dt.Rows[i][0] = i + 1;
        //                columnTotal = 0;
        //                for (int cnt = 2; cnt < dt.Columns.Count - 1; cnt++)
        //                {

        //                    columnTotal += Convert.ToInt32(dt.Rows[i][cnt]);
        //                }
        //                rowTotal += columnTotal;
        //                dr[8] = rowTotal;
        //                dt.Rows[i]["Total Submitted"] = columnTotal;

        //            }
        //            catch (Exception ex)
        //            {

        //            }

        //        }
        //        for (int cnt = 2; cnt < dt.Columns.Count - 1; cnt++)
        //        {
        //            dr[cnt] = dt.AsEnumerable().Sum(row => row.Field<Int32>(dt.Columns[cnt].ToString()));

        //        }
        //        dt.Rows.Add(dr);
        //        GrdInvoiceRaBillsDeductions.DataSource = dt;
        //        GrdInvoiceRaBillsDeductions.DataBind();
        //    }
        //    else
        //    {
        //        GrdInvoiceRaBillsDeductions.DataSource = null;
        //        GrdInvoiceRaBillsDeductions.DataBind();
        //        //btnDocumentSummaryExportExcel.Visible = false;
        //    }

        //}

        private GridViewRow GetHeaderGridViewRow(string header, string value)
        {
            GridViewRow row = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);
            TableHeaderCell cell = new TableHeaderCell();
            cell.Text = header;
            cell.ColumnSpan = 1;
            row.Controls.Add(cell);

            cell = new TableHeaderCell();
            cell.Text = value;
            cell.ColumnSpan = 10;
            cell.HorizontalAlign = HorizontalAlign.Left;
            row.Controls.Add(cell);
            return row;
        }

        protected void GrdInvoiceRaBillsDeductions_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            
        }

        protected void btnDocumentSummaryExportExcel_Click(object sender, EventArgs e)
        {
            try
            {
                ViewState["Export"] = "2";
                BindDocumentSummary();
                StringBuilder StrExport = new StringBuilder();

                StringWriter stw = new StringWriter();
                HtmlTextWriter htextw = new HtmlTextWriter(stw);
                htextw.AddStyleAttribute("font-size", "11pt");
                htextw.AddStyleAttribute("color", "Black");



                GrdInvoiceRaBillsDeductions.RenderControl(htextw); //Name of the Panel

                //var sb1 = new StringBuilder();
                //GridDiv.RenderControl(new HtmlTextWriter(new StringWriter(sb1)));

                string s = htextw.InnerWriter.ToString();

                string HTMLstring = "<html><body>" +
                    "<div style='width:100%; margin:auto;'><div style='width:100%; float:left; line-height:25px; font-size:12pt;' align='center'>" +
                    "<asp:Label ID='Lbl1' runat='server' Font-Bold='true'>" + WebConfigurationManager.AppSettings["Domain"] + " Report Name: Documet Category Summary</asp:Label><br />" +
                    "<asp:Label ID='Lbl4' runat='server'>Project Name : " + DDlProject.SelectedItem.Text + " (" + DDLWorkPackage.SelectedItem.Text + ")</asp:Label><br />" +
                    "</div> <div style='width:100%; float:left; height:10px;'>&nbsp;&nbsp;&nbsp;</div>" +
                    "<div style='width:100%;font-size:11pt float:left;' align='left'>" +
                    s +
                    "</div>" +
                    "</div></body></html>";

                string strFile = "Report_Document_Category_Status_" + DateTime.Now.Ticks + ".xls";
                string strcontentType = "application/excel";
                Response.ClearContent();
                Response.ClearHeaders();
                Response.BufferOutput = true;
                Response.ContentType = strcontentType;
                Response.AddHeader("Content-Disposition", "attachment; filename=" + strFile);
                Response.Write(HTMLstring);
                Response.Flush();
                Response.Close();
                Response.End();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
         


        public override void VerifyRenderingInServerForm(Control control)
        {
            //required to avoid the runtime error "  
            //Control 'GridView1' of type 'GridView' must be placed inside a form tag with runat=server."  
        }


    }
}