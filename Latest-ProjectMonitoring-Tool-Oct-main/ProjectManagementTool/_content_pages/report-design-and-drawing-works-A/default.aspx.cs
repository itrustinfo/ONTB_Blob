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

namespace ProjectManagementTool._content_pages.report_design_and_drawing_works_A
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
                    BindDailyReportMasterMaster();
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

        protected void DDLDailyReportMaster_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindDocumentSummary();
        }

        private void BindDailyReportMasterMaster()
        {
            DataSet ds = getdt.GetdesignanddrawingAmaster();
            if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                DataTable dt = ds.Tables[0].AsEnumerable().OrderByDescending(r => r.Field<DateTime>("CreatedDate")).CopyToDataTable();

                DDLDailyReportMaster.DataTextField = "Description";
                DDLDailyReportMaster.DataValueField = "DPR_UID";
                DDLDailyReportMaster.DataSource = dt;
                DDLDailyReportMaster.DataBind();
            }
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
        private DataTable AddDataRow(DataTable dt, int slNo, string VillageName, string PipeDia, string QuantityAsPerBoq, string ReceivedQty, string PipesReceived, string PreviousQty
            , string TodaysQty, string TotalUpToDate, string Balance, string Remarks)
        {
            DataRow dr = dt.NewRow();
            dr["Sl_No"] = slNo;
            dr["VillageName"] = VillageName;
            dr["PipeDia"] = PipeDia;
            dr["QuantityAsPerBoq"] = QuantityAsPerBoq;
            dr["ReceivedQty"] = ReceivedQty;
            dr["PipesReceived"] = PipesReceived;
            dr["PreviousQty"] = PreviousQty;
            dr["TodaysQty"] = TodaysQty;
            dr["TotalUpToDate"] = TotalUpToDate;
            dr["Balance"] = Balance;
            dr["Remarks"] = Remarks;
            dt.Rows.Add(dr);
            return dt;
        }
        protected void BindDocumentSummary()
        {
            Guid projectUid = Guid.Empty, workPackageUid = Guid.Empty, DPR_UID = Guid.Empty;
            if (DDLDailyReportMaster.SelectedValue != "")
            {
                DPR_UID = new Guid(DDLDailyReportMaster.SelectedValue);
            }
            if (DDlProject.SelectedValue != "")
            {
                projectUid = new Guid(DDlProject.SelectedValue);
            }
            if (DDLWorkPackage.SelectedValue != "")
            {
                workPackageUid = new Guid(DDLWorkPackage.SelectedValue);
            }

            DataSet ds = new DataSet();
            ds = getdt.GetdesignanddrawingworksA(DPR_UID, projectUid, workPackageUid);
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
             
                btnDocumentSummaryExportExcel.Visible = true;
                DocumentSummaryReportName.InnerHtml = "BANGALORE WATER SUPPLY AND SEWERAGE PROJECT- PHASE III<br/>Sewerage System-Contract-CP 26-Works 'A'-Status of site works as on 15.05.2022";
                DocumentSummaryProjectName.InnerHtml = "Contract-" + DDlProject.SelectedItem.Text + " (" + DDLWorkPackage.SelectedItem.Text + ")-Works-A";

                ViewState["Export"] = "1";

                DataTable dt = ds.Tables[0].Copy();

                
                if (ds.Tables[0].Rows.Count > 0)
                {

                    string sHTMLString = " <div style = 'width:100%; float:left;  overflow-y:auto;'><table class='table table-bordered'>";
                   // sHTMLString += "<tr><td coslpan='4' style='text-align:center'><h2><b>BANGALORE WATER SUPPLY AND SEWERAGE PROJECT- PHASE III </b></h2> <br/><h3><b>Sewerage System-Contract-CP 26-Works 'A'-Status of site works as on 15.05.2022</b></h3> </td></tr>";
                  //  sHTMLString += "<tr><td coslpan='4' style='text-align:center'><h3><b>Contract-CP26(K&C Valley)-Works-A</b></h3></td></tr>";
                    sHTMLString += "<tr><td style='text-align:center;' colspan='2'>Name of the Contractor::</td><td colspan='2'>M/s LARSEN & TOUBRO</td></tr>";
                    sHTMLString += "<tr><td style='text-align:center;' colspan='2'>Name of the Work::</td><td colspan='2'>JICA Assisted Bengaluru Water Supply and Sewerage Project (Phase III)–CP 26: Sewerage Facilities to 110 Villages: <br/>" +
                                     "Design, Engineering, Construction and Commissioning of Sewerage Treatment Plants and Intermediate Sewerage Pumping <br/>"+
                                     "Stations with Operation & Maintenance thereof for Seven Years (Works-A) and procurement and constructions of main sewer <br/>" +
                                     "including Manholes in Bommanahalli & Mahadevapura Zones (K & C Valley Catchment) (Works B) under JICA Loan ID–P266 </ td></tr>";
                    sHTMLString += "<tr><td style='text-align:center;' colspan='2'>Contract awarded date ::</td><td colspan='2'>02.07.2021</td></tr>";
                    sHTMLString += "<tr><td style='text-align:center;' colspan='2'>Contract Completion date::</td><td colspan='2'>01.01.2024</td></tr>";
                    sHTMLString += "<tr><td style='text-align:center;' colspan='2'>Awarded amount::</td><td colspan='2'>INR 457,03,52,122 {Capital Cost INR 426,67,67,574 + Operation & Maintanance Cost INR 30,35,84,548}</td></tr>";
                    sHTMLString += "<tr><td style='width:8%'><b>Sl No</b></td>";
                    sHTMLString += "<td style='width:32%'><b>Location</b></td>";
                    sHTMLString += "<td style='width:15%'><b>Capacity</b> </td>";
                    sHTMLString += "<td style='width:45%'><b>Status Of Site Works</b></td></tr>";
                    
                    var headinglist= ds.Tables[0].AsEnumerable().Select(d=>d.Field<string>("Heading")).Distinct().ToArray();
                    char aChar = 'A';

                    foreach (var item in headinglist)
                    {
                        sHTMLString += "<tr><td>" + (aChar) + "</td>";
                        sHTMLString += "<td>" + item.ToString() + "</td>";
                        sHTMLString += "<td></td>";
                        sHTMLString += "<td></td>";

                        sHTMLString += "</tr>";
                        aChar++;


                        var bommanahalliList = ds.Tables[0].AsEnumerable().Where(r => r.Field<string>("Heading") == item).ToList();
                   
                        int i = 0;
                        foreach (var bommanahalli in bommanahalliList)
                        {
                            i += 1;
                            sHTMLString += "<tr><td>" + (i) + "</td>";
                            sHTMLString += "<td>" + bommanahalli["SubHeading"].ToString() + "</td>";
                            sHTMLString += "<td>" + bommanahalli["Units"].ToString() + "</td>";
                            sHTMLString += "<td>" + bommanahalli["SiteWorks"].ToString() + "</td>";

                            sHTMLString += "</tr>";
                        }
                    }
                    sHTMLString += "</table></div>";

                   
                    DivAcrossMonths.InnerHtml = sHTMLString;
                }

                else
                {
                    btnDocumentSummaryExportPDF.Visible = false;
                    btnDocumentSummaryPrint.Visible = false;
                    btnDocumentSummaryExportExcel.Visible = false;
                }

            }
        }


        protected void GrdDocumentSummary_DataBound(object sender, EventArgs e)
        {
            if (ViewState["Export"].ToString() == "1")
            {
                //Guid workPackageUid = Guid.Empty;
                //string name = string.Empty, nameOfWork = string.Empty, awardedDate = string.Empty, completionDate = string.Empty, awardedAmount = string.Empty;
                //if (DDLWorkPackage.SelectedValue != "")
                //{
                //    workPackageUid = new Guid(DDLWorkPackage.SelectedValue);
                //    DataTable dataTable = getdt.GetContractorDetails(workPackageUid);
                //    if(dataTable != null && dataTable.Rows.Count > 0)
                //    {
                //        name = dataTable.Rows[0]["Contractor_Name"].ToString();
                //        awardedDate = dataTable.Rows[0]["Contract_Agreement_Date"].ToString();
                //        if (!string.IsNullOrEmpty(awardedDate))
                //            awardedDate = Convert.ToDateTime(awardedDate).ToString("MM/dd/yyyy");
                //        completionDate = dataTable.Rows[0]["Contract_Completion_Date"].ToString();
                //        if (!string.IsNullOrEmpty(completionDate))
                //            completionDate = Convert.ToDateTime(completionDate).ToString("MM/dd/yyyy");
                //        awardedAmount = dataTable.Rows[0]["Contractor_Representatives"].ToString();
                //    }
                //}

                //if(Constants.NameOfWorkMapWithProjectNameForDailyReport.ContainsKey(DDlProject.SelectedItem.Text))
                //{
                //    nameOfWork = Constants.NameOfWorkMapWithProjectNameForDailyReport[DDlProject.SelectedItem.Text];
                //}

                //GrdDocumentSummary.HeaderRow.Parent.Controls.AddAt(0, GetHeaderGridViewRow("Name of the contractor", name));
                //GrdDocumentSummary.HeaderRow.Parent.Controls.AddAt(1, GetHeaderGridViewRow("Name of the Work:", nameOfWork));
                //GrdDocumentSummary.HeaderRow.Parent.Controls.AddAt(2, GetHeaderGridViewRow("Contract awarded date:", awardedDate));
                //GrdDocumentSummary.HeaderRow.Parent.Controls.AddAt(3, GetHeaderGridViewRow("Contract Completion date:", completionDate));
                //GrdDocumentSummary.HeaderRow.Parent.Controls.AddAt(4, GetHeaderGridViewRow("Awarded amount:", awardedAmount));

            }
        }

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

        protected void GrdDocumentSummary_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            
        }

        protected void btnDocumentSummaryExportExcel_Click(object sender, EventArgs e)
        {
            try
            {
                ViewState["Export"] = "2";
               
                StringBuilder StrExport = new StringBuilder();

                StringWriter stw = new StringWriter();
                HtmlTextWriter htextw = new HtmlTextWriter(stw);
                htextw.AddStyleAttribute("font-size", "11pt");
                htextw.AddStyleAttribute("color", "Black");

                DivAcrossMonths.RenderControl(htextw);

                //GrdDocumentSummary.RenderControl(htextw); //Name of the Panel

                //var sb1 = new StringBuilder();
                //GridDiv.RenderControl(new HtmlTextWriter(new StringWriter(sb1)));

                string s = htextw.InnerWriter.ToString();

                string HTMLstring = "<html><body>" +
                    "<div style='width:100%; margin:auto;'><div style='width:100%; float:left; line-height:25px; font-size:12pt;' align='center'>" +
                    "<asp:Label ID='Lbl1' runat='server' Font-Bold='true'>" + WebConfigurationManager.AppSettings["Domain"] + " Report Name: Status of Documents submitted by the Contractor</asp:Label><br />" +
                    "<asp:Label ID='Lbl4' runat='server'>Project Name : " + DDlProject.SelectedItem.Text + " (" + DDLWorkPackage.SelectedItem.Text + ")</asp:Label><br />" +
                    "</div> <div style='width:100%; float:left; height:10px;'>&nbsp;&nbsp;&nbsp;</div>" +
                    "<div style='width:100%;font-size:11pt float:left;' align='left'>" +
                    s +
                    "</div>" +
                    "</div></body></html>";

                string strFile = "Report_design-and-drawing-works-b-tt_" + DateTime.Now.Ticks + ".xls";
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
        protected void btnDocumentSummaryExportPDF_Click(object sender, EventArgs e)
        {
            ViewState["Export"] = "2";
            BindDocumentSummary();
           // ExporttoPDF(GrdDocumentSummary, 2, "No");
        }
        protected void btnDocumentSummaryPrint_Click(object sender, EventArgs e)
        {
            ViewState["Export"] = "2";
            BindDocumentSummary();
          //  ExporttoPDF(GrdDocumentSummary, 2, "Yes");
        }

        private void ExporttoPDF(GridView gd, int type, string isPrint)
        {
            try
            {

                DateTime CDate1 = DateTime.Now;
                GridView gdRp = new GridView();
                gdRp = gd;

                int noOfColumns = 0, noOfRows = 0;
                DataTable tbl = null;

                if (gdRp.AutoGenerateColumns)
                {
                    tbl = gdRp.DataSource as DataTable; // Gets the DataSource of the GridView Control.
                    noOfColumns = tbl.Columns.Count;
                    noOfRows = tbl.Rows.Count;
                }
                else
                {
                    noOfColumns = gdRp.Columns.Count;
                    noOfRows = gdRp.Rows.Count;
                }

                float HeaderTextSize = 9;
                float ReportNameSize = 9;
                float ReportTextSize = 9;
                float ApplicationNameSize = 13;
                string ProjectName = DDlProject.SelectedItem.ToString();

                //int Diff = 100 - ProjectName.Length;
                //if (ProjectName.Length > 30)
                //{
                //    ProjectName = ProjectName.Substring(0, 29) + "..";
                //}

                //if (Diff > 0)
                //{
                //    ProjectName = ProjectName + string.Concat(Enumerable.Repeat("&nbsp;", Diff));
                //}
                // Creates a PDF document

                Document document = null;
                //if (LandScape == true)
                //{
                // Sets the document to A4 size and rotates it so that the     orientation of the page is Landscape.
                document = new Document(PageSize.A4.Rotate(), 0, 0, 0, 0);
                //}
                //else
                //{
                //    document = new Document(PageSize.A4, 0, 0, 15, 5);
                //}

                // Creates a PdfPTable with column count of the table equal to no of columns of the gridview or gridview datasource.
                iTextSharp.text.pdf.PdfPTable mainTable = new iTextSharp.text.pdf.PdfPTable(noOfColumns);

                // Sets the first 4 rows of the table as the header rows which will be repeated in all the pages.
                mainTable.HeaderRows = 4;


                // Creates a PdfPTable with 2 columns to hold the header in the exported PDF.
                iTextSharp.text.pdf.PdfPTable headerTable = new iTextSharp.text.pdf.PdfPTable(1);

                // Creates a phrase to hold the application name at the left hand side of the header.
                Phrase phApplicationName = new Phrase();
                string ExportFileName = "";
                phApplicationName = new Phrase("Report Name: Daily Progress Report of " + DDLDailyReportMaster.SelectedItem.Text, FontFactory.GetFont("Arial", ApplicationNameSize, iTextSharp.text.Font.BOLD));
                ExportFileName = "Daily_Progress_Report_" + DateTime.Now.Ticks + ".pdf";
                mainTable.SetWidths(new float[] { 10, 30, 20, 20, 20, 10, 10, 10, 10, 10, 10 });


                // Creates a PdfPCell which accepts a phrase as a parameter.
                PdfPCell clApplicationName = new PdfPCell(phApplicationName);
                // Sets the border of the cell to zero.
                clApplicationName.Border = PdfPCell.NO_BORDER;
                // Sets the Horizontal Alignment of the PdfPCell to left.
                clApplicationName.HorizontalAlignment = Element.ALIGN_CENTER;

                // Creates a phrase to show the current date at the right hand side of the header.
                //Phrase phDate = new Phrase(DateTime.Now.Date.ToString("dd/MM/yyyy"), FontFactory.GetFont("Arial", ApplicationNameSize, iTextSharp.text.Font.NORMAL));

                //// Creates a PdfPCell which accepts the date phrase as a parameter.
                //PdfPCell clDate = new PdfPCell(phDate);
                //// Sets the Horizontal Alignment of the PdfPCell to right.
                //clDate.HorizontalAlignment = Element.ALIGN_RIGHT;
                //// Sets the border of the cell to zero.
                //clDate.Border = PdfPCell.NO_BORDER;

                // Adds the cell which holds the application name to the headerTable.
                headerTable.AddCell(clApplicationName);
                // Adds the cell which holds the date to the headerTable.
                //  headerTable.AddCell(clDate);
                // Sets the border of the headerTable to zero.
                headerTable.DefaultCell.Border = PdfPCell.NO_BORDER;

                // Creates a PdfPCell that accepts the headerTable as a parameter and then adds that cell to the main PdfPTable.
                PdfPCell cellHeader = new PdfPCell(headerTable);
                cellHeader.Border = PdfPCell.NO_BORDER;
                // Sets the column span of the header cell to noOfColumns.
                cellHeader.Colspan = noOfColumns;
                // Adds the above header cell to the table.
                mainTable.AddCell(cellHeader);

                // Creates a phrase which holds the file name.
                Phrase phHeader = new Phrase("Project Name : " + ProjectName + " (" + DDLWorkPackage.SelectedItem.Text + ")");
                PdfPCell clHeader = new PdfPCell(phHeader);
                clHeader.Colspan = noOfColumns;
                clHeader.Border = PdfPCell.NO_BORDER;
                clHeader.HorizontalAlignment = Element.ALIGN_CENTER;
                mainTable.AddCell(clHeader);



                // Creates a phrase for a new line.
                Phrase phSpace = new Phrase("\n");
                PdfPCell clSpace = new PdfPCell(phSpace);
                clSpace.Border = PdfPCell.NO_BORDER;
                clSpace.Colspan = noOfColumns;
                mainTable.AddCell(clSpace);



                Guid workPackageUid = Guid.Empty;
                string name = string.Empty, nameOfWork = string.Empty, awardedDate = string.Empty, completionDate = string.Empty, awardedAmount = string.Empty;
                if (DDLWorkPackage.SelectedValue != "")
                {
                    workPackageUid = new Guid(DDLWorkPackage.SelectedValue);
                    DataTable dataTable = getdt.GetContractorDetails(workPackageUid);
                    if (dataTable != null && dataTable.Rows.Count > 0)
                    {
                        name = dataTable.Rows[0]["Contractor_Name"].ToString();
                        awardedDate = dataTable.Rows[0]["Contract_Agreement_Date"].ToString();
                        if (!string.IsNullOrEmpty(awardedDate))
                            awardedDate = Convert.ToDateTime(awardedDate).ToString("MM/dd/yyyy");
                        completionDate = dataTable.Rows[0]["Contract_Completion_Date"].ToString();
                        if (!string.IsNullOrEmpty(completionDate))
                            completionDate = Convert.ToDateTime(completionDate).ToString("MM/dd/yyyy");
                        awardedAmount = dataTable.Rows[0]["Contractor_Representatives"].ToString();
                    }
                }

                if (Constants.NameOfWorkMapWithProjectNameForDailyReport.ContainsKey(DDlProject.SelectedItem.Text))
                {
                    nameOfWork = Constants.NameOfWorkMapWithProjectNameForDailyReport[DDlProject.SelectedItem.Text];
                }


                Phrase phTop1 = new Phrase("Name of the contractor", FontFactory.GetFont("Arial", HeaderTextSize, iTextSharp.text.Font.BOLD)); ;
                PdfPCell col_Top1_1 = new PdfPCell(phTop1);
                //col_Top1_1.Colspan = 1;
                mainTable.AddCell(col_Top1_1);
                Phrase phTop1_1 = new Phrase(name, FontFactory.GetFont("Arial", HeaderTextSize, iTextSharp.text.Font.BOLD)); ;
                PdfPCell col_Top1_2 = new PdfPCell(phTop1_1);
                col_Top1_2.Colspan = 10;
                mainTable.AddCell(col_Top1_2);


                Phrase phTop2 = new Phrase("Name of the Work", FontFactory.GetFont("Arial", HeaderTextSize, iTextSharp.text.Font.BOLD)); ;
                PdfPCell col_Top2_1 = new PdfPCell(phTop2);
                //col_Top2_1.Colspan = 1;
                mainTable.AddCell(col_Top2_1);
                Phrase phTop2_1 = new Phrase(nameOfWork, FontFactory.GetFont("Arial", HeaderTextSize, iTextSharp.text.Font.BOLD)); ;
                PdfPCell col_Top2_2 = new PdfPCell(phTop2_1);
                col_Top2_2.Colspan = 10;
                mainTable.AddCell(col_Top2_2);


                Phrase phTop3 = new Phrase("Contract awarded date", FontFactory.GetFont("Arial", HeaderTextSize, iTextSharp.text.Font.BOLD)); ;
                PdfPCell col_Top3_1 = new PdfPCell(phTop3);
                //col_Top3_1.Colspan = 1;
                mainTable.AddCell(col_Top3_1);
                Phrase phTop3_1 = new Phrase(awardedDate, FontFactory.GetFont("Arial", HeaderTextSize, iTextSharp.text.Font.BOLD)); ;
                PdfPCell col_Top3_2 = new PdfPCell(phTop3_1);
                col_Top3_2.Colspan = 10;
                mainTable.AddCell(col_Top3_2);


                Phrase phTop4 = new Phrase("Contract Completion date", FontFactory.GetFont("Arial", HeaderTextSize, iTextSharp.text.Font.BOLD)); ;
                PdfPCell col_Top4_1 = new PdfPCell(phTop4);
                //col_Top4_1.Colspan = 1;
                mainTable.AddCell(col_Top4_1);
                Phrase phTop4_1 = new Phrase(completionDate, FontFactory.GetFont("Arial", HeaderTextSize, iTextSharp.text.Font.BOLD)); ;
                PdfPCell col_Top4_2 = new PdfPCell(phTop4_1);
                col_Top4_2.Colspan = 10;
                mainTable.AddCell(col_Top4_2);


                Phrase phTop5 = new Phrase("Awarded amount", FontFactory.GetFont("Arial", HeaderTextSize, iTextSharp.text.Font.BOLD)); ;
                PdfPCell col_Top5_1 = new PdfPCell(phTop5);
                //col_Top5_1.Colspan = 1;
                mainTable.AddCell(col_Top5_1);
                Phrase phTop5_1 = new Phrase(awardedAmount, FontFactory.GetFont("Arial", HeaderTextSize, iTextSharp.text.Font.BOLD)); ;
                PdfPCell col_Top5_2 = new PdfPCell(phTop5_1);
                col_Top5_2.Colspan = 10;
                mainTable.AddCell(col_Top5_2);








                // Sets the gridview column names as table headers.
                for (int i = 0; i < noOfColumns; i++)
                {
                    Phrase ph = null;

                    if (gdRp.AutoGenerateColumns)
                    {
                        ph = new Phrase(tbl.Columns[i].ColumnName.Replace("<br/>", Environment.NewLine), FontFactory.GetFont("Arial", HeaderTextSize, iTextSharp.text.Font.BOLD));
                    }
                    else
                    {
                        ph = new Phrase(gdRp.Columns[i].HeaderText.Replace("<br/>", Environment.NewLine), FontFactory.GetFont("Arial", HeaderTextSize, iTextSharp.text.Font.BOLD));
                    }
                    PdfPCell cl = new PdfPCell(ph);
                    if (i == 0)
                    {
                        cl.HorizontalAlignment = Element.ALIGN_CENTER;
                        mainTable.AddCell(cl);
                    }
                    else if (i == 1)
                    {
                        cl.HorizontalAlignment = Element.ALIGN_LEFT;
                        mainTable.AddCell(cl);
                    }
                    else if (i == 2)
                    {
                        cl.HorizontalAlignment = Element.ALIGN_CENTER;
                        mainTable.AddCell(cl);
                    }
                    else if (i == 3)
                    {
                        cl.HorizontalAlignment = Element.ALIGN_CENTER;
                        mainTable.AddCell(cl);
                    }
                    //else if (i == 3)
                    //{
                    //    if (RBLReportFor.SelectedValue == "By Originator")
                    //    {
                    //        cl.HorizontalAlignment = Element.ALIGN_LEFT;
                    //        mainTable.AddCell(cl);
                    //    }
                    //    else
                    //    {
                    //        cl.HorizontalAlignment = Element.ALIGN_CENTER;
                    //        mainTable.AddCell(cl);
                    //    }

                    //}
                    else
                    {
                        cl.HorizontalAlignment = Element.ALIGN_CENTER;
                        mainTable.AddCell(cl);


                    }

                }

                bool isBold = false;
                // Reads the gridview rows and adds them to the mainTable
                for (int rowNo = 0; rowNo <= noOfRows; rowNo++)
                {
                    if (rowNo != noOfRows)
                    {
                        for (int columnNo = 0; columnNo < noOfColumns; columnNo++)
                        {
                            if (gdRp.AutoGenerateColumns)
                            {
                                string s = gdRp.Rows[rowNo].Cells[columnNo].Text.Trim();
                                Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
                                PdfPCell cl = new PdfPCell(ph);
                                cl.HorizontalAlignment = Element.ALIGN_CENTER;
                                mainTable.AddCell(cl);
                            }
                            else
                            {
                                if (columnNo == 0)
                                    isBold = false;
                                if (columnNo == 1)
                                {
                                    if (gdRp.Columns[columnNo] is TemplateField)
                                    {
                                        DataBoundLiteralControl lc = gdRp.Rows[rowNo].Cells[columnNo].Controls[0] as DataBoundLiteralControl;
                                        string s = lc.Text.Trim();
                                        s = s.Replace("<span style='font-weight: bold'>", "").Replace("<span style='font-weight: bold; font-size: 16px;'>", "").Replace("</span>", "");
                                        //Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
                                        Phrase ph = new Phrase();
                                        if (lc.Text.Trim() == s)
                                        {
                                            ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
                                        }
                                        else
                                        {
                                            ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.BOLD));
                                            isBold = true;
                                        }
                                        PdfPCell cl = new PdfPCell(ph);
                                        cl.HorizontalAlignment = Element.ALIGN_LEFT;
                                        mainTable.AddCell(cl);
                                    }
                                    else
                                    {
                                        string s = gdRp.Rows[rowNo].Cells[columnNo].Text.Trim();
                                        s = s.Replace("&nbsp;", "");
                                        //Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
                                        Phrase ph = new Phrase();
                                        if (isBold)
                                        {
                                            ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.BOLD));
                                        }
                                        else
                                        {
                                            ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
                                        }
                                        PdfPCell cl = new PdfPCell(ph);
                                        cl.HorizontalAlignment = Element.ALIGN_LEFT;
                                        mainTable.AddCell(cl);
                                    }
                                }
                                else
                                {
                                    if (gdRp.Columns[columnNo] is TemplateField)
                                    {
                                        DataBoundLiteralControl lc = gdRp.Rows[rowNo].Cells[columnNo].Controls[0] as DataBoundLiteralControl;
                                        string s = lc.Text.Trim();
                                        Phrase ph = new Phrase();
                                        if(isBold)
                                            ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.BOLD));
                                        else
                                            ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));

                                        PdfPCell cl = new PdfPCell(ph);
                                        cl.HorizontalAlignment = Element.ALIGN_CENTER;
                                        mainTable.AddCell(cl);
                                    }
                                    else
                                    {
                                        string s = gdRp.Rows[rowNo].Cells[columnNo].Text.Trim();
                                        s = s.Replace("&nbsp;", "");
                                        Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
                                        PdfPCell cl = new PdfPCell(ph);
                                        cl.HorizontalAlignment = Element.ALIGN_CENTER;
                                        mainTable.AddCell(cl);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (type == 1)
                        {
                            for (int columnNo = 0; columnNo < noOfColumns; columnNo++)
                            {
                                string s = "Grand Total";
                                if (columnNo == 1)
                                {
                                    Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.BOLD));
                                    PdfPCell cl = new PdfPCell(ph);
                                    cl.HorizontalAlignment = Element.ALIGN_CENTER;
                                    mainTable.AddCell(cl);
                                }
                                else if (columnNo == 2)
                                {
                                    s = ViewState["grandtotalcount"].ToString();// Session["AwardedValue"].ToString();
                                    Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.BOLD));
                                    PdfPCell cl = new PdfPCell(ph);
                                    cl.HorizontalAlignment = Element.ALIGN_CENTER;
                                    mainTable.AddCell(cl);
                                }
                                else if (columnNo == 3)
                                {
                                    s = ViewState["grandtotaldwnldcount"].ToString();// Session["ExpenditureOverallTotal"].ToString();
                                    Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.BOLD));
                                    PdfPCell cl = new PdfPCell(ph);
                                    cl.HorizontalAlignment = Element.ALIGN_CENTER;
                                    mainTable.AddCell(cl);
                                }
                                else if (columnNo == 4)
                                {
                                    s = ViewState["grandtotalviewcount"].ToString();// Session["TargetedOverallTotal"].ToString() + "%";
                                    Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.BOLD));
                                    PdfPCell cl = new PdfPCell(ph);
                                    cl.HorizontalAlignment = Element.ALIGN_CENTER;
                                    mainTable.AddCell(cl);
                                }
                                else if (columnNo == 5)
                                {
                                    s = ViewState["GrandTotalDocumentLinkSent"].ToString();// Session["AchievedOverallTotal"].ToString() + "%";
                                    Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.BOLD));
                                    PdfPCell cl = new PdfPCell(ph);
                                    cl.HorizontalAlignment = Element.ALIGN_CENTER;
                                    mainTable.AddCell(cl);
                                }
                                else
                                {
                                    Phrase ph = new Phrase("", FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.BOLD));
                                    PdfPCell cl = new PdfPCell(ph);
                                    cl.HorizontalAlignment = Element.ALIGN_CENTER;
                                    mainTable.AddCell(cl);
                                }
                            }
                        }

                    }

                    // Tells the mainTable to complete the row even if any cell is left incomplete.
                    mainTable.CompleteRow();
                }

                // Gets the instance of the document created and writes it to the output stream of the Response object.
                if (isPrint == "Yes")
                {
                    PdfWriter.GetInstance(document, new FileStream(Server.MapPath("~") + "/RegExcel/printpdf.pdf", FileMode.Create));
                }
                else
                {
                    PdfWriter.GetInstance(document, Response.OutputStream);
                }
                //PdfWriter.GetInstance(document, new FileStream(Server.MapPath("~") + "mypdf.pdf", FileMode.Create));
                // Creates a footer for the PDF document.
                int len = 174;
                System.Text.StringBuilder time = new System.Text.StringBuilder();
                time.Append(DateTime.Now.ToString("hh:mm tt"));
                time.Append("".PadLeft(len, ' ').Replace(" ", " "));

                iTextSharp.text.Font foot = new iTextSharp.text.Font();
                foot.Size = 10;
                HeaderFooter pdfFooter = new HeaderFooter(new Phrase("Date : " + DateTime.Now.ToString("dd/MM/yyyy") + " Time " + time + "  Page: ", foot), new Phrase(Environment.NewLine + " ", foot));
                pdfFooter.Alignment = Element.ALIGN_CENTER;
                document.Footer = pdfFooter;
                document.Open();
                // Adds the mainTable to the document.
                document.Add(mainTable);
                // Closes the document.
                document.Close();
                if (isPrint == "Yes")
                {
                    Session["Print"] = true;
                    Response.Write("<script>window.open ('/PDFPRint.aspx','_blank');</script>");
                }
                else
                {
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-disposition", "attachment;filename=" + ExportFileName + "");
                    Response.End();
                }
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