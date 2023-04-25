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

namespace ProjectManagementTool._content_pages.document_status_report
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
                    SelectedProjectWorkpackage("Project");
                    
                    DDlProject_SelectedIndexChanged(sender, e);

                    BindDocumentStatusReport();
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

        }

        protected void DDlProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDlProject.SelectedValue != "")
            {
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
                    SelectedProjectWorkpackage("Workpackage");
                    
                    Session["Project_Workpackage"] = DDlProject.SelectedValue + "_" + DDLWorkPackage.SelectedValue;
                    DDLWorkPackage_SelectedIndexChanged(sender, e);
                }
            }

        }

        protected void DDLWorkPackage_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["Project_Workpackage"] = DDlProject.SelectedValue + "_" + DDLWorkPackage.SelectedValue;
            BindDocumentStatusReport();
        }

        private void SelectedProjectWorkpackage(string pType)
        {
            if (!IsPostBack && Session["Project_Workpackage"] != null)
            {
                string[] selectedValue = Session["Project_Workpackage"].ToString().Split('_');
                if (selectedValue.Length > 1)
                {
                    if (pType == "Project")
                    {
                        DDlProject.SelectedValue = selectedValue[0];
                    }
                    else
                    {
                        DDLWorkPackage.SelectedValue = selectedValue[1];
                    }
                }
                else
                {
                    if (pType == "Project")
                    {
                        DDlProject.SelectedValue = Session["Project_Workpackage"].ToString();
                    }
                }
            }

        }

        private void BindDocumentStatusReport()
        {
            DataTable dt = new DataTable();
            DataRow dr;

            
            dt.Columns.Add(new DataColumn("SlNo"));
            dt.Columns.Add(new DataColumn("SubmittalName"));
            dt.Columns.Add(new DataColumn("DocumentName"));
            dt.Columns.Add(new DataColumn("DateSubmission"));
            dt.Columns.Add(new DataColumn("CodeStatus"));
            dt.Columns.Add(new DataColumn("DateApprovedbyPMC"));
            dt.Columns.Add(new DataColumn("DateApprovedbyBWSSB"));

            dt.Columns.Add(new DataColumn("DateSubmissionB"));
            dt.Columns.Add(new DataColumn("CodeStatusB"));
            dt.Columns.Add(new DataColumn("DateApprovedbyPMCB"));
            dt.Columns.Add(new DataColumn("DateApprovedbyBWSSBB"));

            dt.Columns.Add(new DataColumn("DateSubmissionC"));
            dt.Columns.Add(new DataColumn("CodeStatusC"));
            dt.Columns.Add(new DataColumn("DateApprovedbyPMCC"));
            dt.Columns.Add(new DataColumn("DateApprovedbyBWSSBC"));

            dt.Columns.Add(new DataColumn("DateSubmissionD"));
            dt.Columns.Add(new DataColumn("CodeStatusD"));
            dt.Columns.Add(new DataColumn("DateApprovedbyPMCD"));
            dt.Columns.Add(new DataColumn("DateApprovedbyBWSSBD"));

            dt.Columns.Add(new DataColumn("DateSubmissionE"));
            dt.Columns.Add(new DataColumn("CodeStatusE"));
            dt.Columns.Add(new DataColumn("DateApprovedbyPMCE"));
            dt.Columns.Add(new DataColumn("DateApprovedbyBWSSBE"));

            dt.Columns.Add(new DataColumn("DateSubmissionF"));
            dt.Columns.Add(new DataColumn("CodeStatusF"));
            dt.Columns.Add(new DataColumn("DateApprovedbyPMCF"));
            dt.Columns.Add(new DataColumn("DateApprovedbyBWSSBF"));

            DataSet ds = getdt.GetSubmittal_by_WorkpackageUID(new Guid(DDLWorkPackage.SelectedValue));
            DataSet dsData = null;
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                dr = dt.NewRow();
                dr["SlNo"] = i + 1;
                dr["SubmittalName"] = getdt.getTaskNameby_TaskUID(new Guid(ds.Tables[0].Rows[i]["TaskUID"].ToString())) + "->" + ds.Tables[0].Rows[i]["DocName"].ToString();
                dt.Rows.Add(dr);

                DataSet dsdocuments = getdt.ActualDocuments_SelectBy_DocumentUID(new Guid(ds.Tables[0].Rows[i]["DocumentUID"].ToString()));
                for (int j = 0; j < dsdocuments.Tables[0].Rows.Count; j++)
                {
                    dr = dt.NewRow();
                    dr["SlNo"] = (i + 1) + "." + (j + 1);
                    dr["DocumentName"] = dsdocuments.Tables[0].Rows[j]["ActualDocument_Name"].ToString();
                    dr["DateSubmission"] = Convert.ToDateTime(dsdocuments.Tables[0].Rows[j]["ActualDocument_CreatedDate"].ToString()).ToString("dd MMM yyyy");
                    DataSet dsStatus = getdt.GetDocumentVersion_by_DocumentUID(new Guid(dsdocuments.Tables[0].Rows[j]["ActualDocumentUID"].ToString()));
                    for (int k = 0; k < dsStatus.Tables[0].Rows.Count; k++)
                    {
                        if (k == 0)
                        {
                            // dr["CodeStatus"] = dsStatus.Tables[0].Rows[k]["Code"].ToString();
                            // dr["DateApprovedbyPMC"] = string.IsNullOrEmpty(dsStatus.Tables[0].Rows[k]["PMCDate"].ToString()) ? "" : Convert.ToDateTime(dsStatus.Tables[0].Rows[k]["PMCDate"].ToString()).ToString("dd MMM yyyy");
                            // dr["DateApprovedbyBWSSB"] = dsStatus.Tables[0].Rows[k]["Code"].ToString() == "Code A" ? string.IsNullOrEmpty(dsStatus.Tables[0].Rows[k]["PMCDate"].ToString()) ? "" : Convert.ToDateTime(dsStatus.Tables[0].Rows[k]["PMCDate"].ToString()).ToString("dd MMM yyyy") : "";
                            dsData = getdt.GetDocStatusandDateForReport(new Guid(dsdocuments.Tables[0].Rows[j]["ActualDocumentUID"].ToString()), 1, "DTL");
                            if (dsData.Tables[0].Rows.Count > 0)
                            {
                                dr["CodeStatus"] = dsData.Tables[0].Rows[k]["ActivityType"].ToString();
                                dr["DateApprovedbyPMC"] = string.IsNullOrEmpty(dsData.Tables[0].Rows[0]["CreatedDate"].ToString()) ? "" : Convert.ToDateTime(dsData.Tables[0].Rows[0]["CreatedDate"].ToString()).ToString("dd MMM yyyy");
                            }
                            //
                            dsData = getdt.GetDocStatusandDateForReport(new Guid(dsdocuments.Tables[0].Rows[j]["ActualDocumentUID"].ToString()), 1, "BWSSB");
                            if (dsData.Tables[0].Rows.Count > 0)
                            {
                                dr["DateApprovedbyBWSSB"] = string.IsNullOrEmpty(dsData.Tables[0].Rows[0]["CreatedDate"].ToString()) ? "" : Convert.ToDateTime(dsData.Tables[0].Rows[0]["CreatedDate"].ToString()).ToString("dd MMM yyyy");
                            }
                        }
                        else if (k == 1)
                        {
                            dr["DateSubmissionB"] = Convert.ToDateTime(dsStatus.Tables[0].Rows[k]["Doc_StatusDate"].ToString()).ToString("dd MMM yyyy");
                            dsData = getdt.GetDocStatusandDateForReport(new Guid(dsdocuments.Tables[0].Rows[j]["ActualDocumentUID"].ToString()), 2, "DTL");
                            if(dsData.Tables[0].Rows.Count > 0)
                            {
                                dr["CodeStatusB"] = dsData.Tables[0].Rows[0]["ActivityType"].ToString();
                                dr["DateApprovedbyPMCB"] = string.IsNullOrEmpty(dsData.Tables[0].Rows[0]["CreatedDate"].ToString()) ? "" : Convert.ToDateTime(dsData.Tables[0].Rows[0]["CreatedDate"].ToString()).ToString("dd MMM yyyy");

                            }
                            //
                            dsData = getdt.GetDocStatusandDateForReport(new Guid(dsdocuments.Tables[0].Rows[j]["ActualDocumentUID"].ToString()), 1, "BWSSB");
                            if (dsData.Tables[0].Rows.Count > 0)
                            {
                                dr["DateApprovedbyBWSSBB"] = string.IsNullOrEmpty(dsData.Tables[0].Rows[0]["CreatedDate"].ToString()) ? "" : Convert.ToDateTime(dsData.Tables[0].Rows[0]["CreatedDate"].ToString()).ToString("dd MMM yyyy");
                            }
                        }
                        else if (k == 2)
                        {
                            dr["DateSubmissionC"] = Convert.ToDateTime(dsStatus.Tables[0].Rows[k]["Doc_StatusDate"].ToString()).ToString("dd MMM yyyy");
                            dsData = getdt.GetDocStatusandDateForReport(new Guid(dsdocuments.Tables[0].Rows[j]["ActualDocumentUID"].ToString()), 3, "DTL");
                            if (dsData.Tables[0].Rows.Count > 0)
                            {
                                dr["CodeStatusC"] = dsData.Tables[0].Rows[0]["ActivityType"].ToString();
                                dr["DateApprovedbyPMCC"] = string.IsNullOrEmpty(dsData.Tables[0].Rows[0]["CreatedDate"].ToString()) ? "" : Convert.ToDateTime(dsData.Tables[0].Rows[0]["CreatedDate"].ToString()).ToString("dd MMM yyyy");

                            }
                            //
                            dsData = getdt.GetDocStatusandDateForReport(new Guid(dsdocuments.Tables[0].Rows[j]["ActualDocumentUID"].ToString()), 3, "BWSSB");
                            if (dsData.Tables[0].Rows.Count > 0)
                            {
                                dr["DateApprovedbyBWSSBC"] = string.IsNullOrEmpty(dsData.Tables[0].Rows[0]["CreatedDate"].ToString()) ? "" : Convert.ToDateTime(dsData.Tables[0].Rows[0]["CreatedDate"].ToString()).ToString("dd MMM yyyy");
                            }
                        }
                        else if (k == 3)
                        {
                            dr["DateSubmissionD"] = Convert.ToDateTime(dsStatus.Tables[0].Rows[k]["Doc_StatusDate"].ToString()).ToString("dd MMM yyyy");
                            dsData = getdt.GetDocStatusandDateForReport(new Guid(dsdocuments.Tables[0].Rows[j]["ActualDocumentUID"].ToString()), 4, "DTL");
                            if (dsData.Tables[0].Rows.Count > 0)
                            {
                                dr["CodeStatusD"] = dsData.Tables[0].Rows[0]["ActivityType"].ToString();
                                dr["DateApprovedbyPMCD"] = string.IsNullOrEmpty(dsData.Tables[0].Rows[0]["CreatedDate"].ToString()) ? "" : Convert.ToDateTime(dsData.Tables[0].Rows[0]["CreatedDate"].ToString()).ToString("dd MMM yyyy");

                            }
                            //
                            dsData = getdt.GetDocStatusandDateForReport(new Guid(dsdocuments.Tables[0].Rows[j]["ActualDocumentUID"].ToString()), 4, "BWSSB");
                            if (dsData.Tables[0].Rows.Count > 0)
                            {
                                dr["DateApprovedbyBWSSBD"] = string.IsNullOrEmpty(dsData.Tables[0].Rows[0]["CreatedDate"].ToString()) ? "" : Convert.ToDateTime(dsData.Tables[0].Rows[0]["CreatedDate"].ToString()).ToString("dd MMM yyyy");
                            }
                        }
                        else if (k == 4)
                        {
                            dr["DateSubmissionE"] = Convert.ToDateTime(dsStatus.Tables[0].Rows[k]["Doc_StatusDate"].ToString()).ToString("dd MMM yyyy");
                            dsData = getdt.GetDocStatusandDateForReport(new Guid(dsdocuments.Tables[0].Rows[j]["ActualDocumentUID"].ToString()), 5, "DTL");
                            if (dsData.Tables[0].Rows.Count > 0)
                            {
                                dr["CodeStatusE"] = dsData.Tables[0].Rows[0]["ActivityType"].ToString();
                                dr["DateApprovedbyPMCE"] = string.IsNullOrEmpty(dsData.Tables[0].Rows[0]["CreatedDate"].ToString()) ? "" : Convert.ToDateTime(dsData.Tables[0].Rows[0]["CreatedDate"].ToString()).ToString("dd MMM yyyy");

                            }
                            //
                            dsData = getdt.GetDocStatusandDateForReport(new Guid(dsdocuments.Tables[0].Rows[j]["ActualDocumentUID"].ToString()), 5, "BWSSB");
                            if (dsData.Tables[0].Rows.Count > 0)
                            {
                                dr["DateApprovedbyBWSSBE"] = string.IsNullOrEmpty(dsData.Tables[0].Rows[0]["CreatedDate"].ToString()) ? "" : Convert.ToDateTime(dsData.Tables[0].Rows[0]["CreatedDate"].ToString()).ToString("dd MMM yyyy");
                            }
                        }
                        else
                        {
                            dr["DateSubmissionF"] = Convert.ToDateTime(dsStatus.Tables[0].Rows[k]["Doc_StatusDate"].ToString()).ToString("dd MMM yyyy");
                            dsData = getdt.GetDocStatusandDateForReport(new Guid(dsdocuments.Tables[0].Rows[j]["ActualDocumentUID"].ToString()), 6, "DTL");
                            if (dsData.Tables[0].Rows.Count > 0)
                            {
                                dr["CodeStatusF"] = dsData.Tables[0].Rows[0]["ActivityType"].ToString();
                                dr["DateApprovedbyPMCF"] = string.IsNullOrEmpty(dsData.Tables[0].Rows[0]["CreatedDate"].ToString()) ? "" : Convert.ToDateTime(dsData.Tables[0].Rows[0]["CreatedDate"].ToString()).ToString("dd MMM yyyy");

                            }
                            //
                            dsData = getdt.GetDocStatusandDateForReport(new Guid(dsdocuments.Tables[0].Rows[j]["ActualDocumentUID"].ToString()), 6, "BWSSB");
                            if (dsData.Tables[0].Rows.Count > 0)
                            {
                                dr["DateApprovedbyBWSSBF"] = string.IsNullOrEmpty(dsData.Tables[0].Rows[0]["CreatedDate"].ToString()) ? "" : Convert.ToDateTime(dsData.Tables[0].Rows[0]["CreatedDate"].ToString()).ToString("dd MMM yyyy");
                            }
                        }




                    }

                    dt.Rows.Add(dr);
                }
                
            }
            
            GrdDocumentStatusReport.DataSource = dt;
            GrdDocumentStatusReport.DataBind();
        }

        protected void GrdDocumentStatusReport_DataBound(object sender, EventArgs e)
        {
            GridViewRow row = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);
            TableHeaderCell cell = new TableHeaderCell();
            cell.Text = "";
            cell.ColumnSpan = 3;
            row.Controls.Add(cell);

            cell = new TableHeaderCell();
            cell.ColumnSpan = 4;
            cell.Text = "Rev. A";
            row.Controls.Add(cell);

            cell = new TableHeaderCell();
            cell.ColumnSpan = 4;
            cell.Text = "Rev. B";
            row.Controls.Add(cell);

            cell = new TableHeaderCell();
            cell.ColumnSpan = 4;
            cell.Text = "Rev. C";
            row.Controls.Add(cell);

            cell = new TableHeaderCell();
            cell.ColumnSpan = 4;
            cell.Text = "Rev. D";
            row.Controls.Add(cell);

            cell = new TableHeaderCell();
            cell.ColumnSpan = 4;
            cell.Text = "Rev. E";
            row.Controls.Add(cell);

            cell = new TableHeaderCell();
            cell.ColumnSpan = 4;
            cell.Text = "Rev. F";
            row.Controls.Add(cell);
            if (GrdDocumentStatusReport.Rows.Count > 0)
            {
                GrdDocumentStatusReport.HeaderRow.Parent.Controls.AddAt(0, row);
            }
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            //required to avoid the runtime error "  
            //Control 'GridView1' of type 'GridView' must be placed inside a form tag with runat=server."  
        }

        protected void btnDocumentSummaryExportExcel_Click(object sender, EventArgs e)
        {
            try
            {
                StringBuilder StrExport = new StringBuilder();
                BindDocumentStatusReport();
                StringWriter stw = new StringWriter();
            HtmlTextWriter htextw = new HtmlTextWriter(stw);
            htextw.AddStyleAttribute("font-size", "11pt");
            htextw.AddStyleAttribute("color", "Black");


            GrdDocumentStatusReport.RenderControl(htextw); //Name of the Panel

            //var sb1 = new StringBuilder();
            //GridDiv.RenderControl(new HtmlTextWriter(new StringWriter(sb1)));

            string s = htextw.InnerWriter.ToString();

            string HTMLstring = "<html><body>" +
                "<div style='width:100%; margin:auto;'><div style='width:100%; float:left; line-height:25px; font-size:12pt;' align='center'>" +
                "<asp:Label ID='Lbl1' runat='server' Font-Bold='true'>" + WebConfigurationManager.AppSettings["Domain"] + " Report Name: Design & Drawings Works A STP Report</asp:Label><br />" +
                "<asp:Label ID='Lbl4' runat='server'>Project Name : " + DDlProject.SelectedItem.Text + " (" + DDLWorkPackage.SelectedItem.Text + ")</asp:Label><br />" +
                "</div> <div style='width:100%; float:left; height:10px;'>&nbsp;&nbsp;&nbsp;</div>" +
                "<div style='width:100%;font-size:11pt float:left;' align='left'>" +
                s +
                "</div>" +
                "</div></body></html>";

            string strFile = "Report_Design_&_Drawings Works A_" + DateTime.Now.Ticks + ".xls";
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
    }
}