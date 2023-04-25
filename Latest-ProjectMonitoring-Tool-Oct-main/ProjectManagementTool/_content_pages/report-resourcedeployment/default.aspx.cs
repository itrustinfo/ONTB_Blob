using ProjectManager.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Web.Configuration;
using System.IO;
using System.Text;
using ProjectManagementTool.Models;

namespace ProjectManagementTool._content_pages.report_resourcedeployment
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
                    DDLResourceType.Visible = false;
                    BindProject();
                    BindResourceType();
                    ByMonth.Visible = false;
                    ResourceDeployment.Visible = false;
                  //  RBLReportFor.Items[0].Enabled = false;
                  //  RBLReportFor.SelectedIndex = 1;
                }
            }
        }

        private void BindProject()
        {
            DataTable ds = new DataTable();
            if (Session["TypeOfUser"].ToString() == "U" || Session["TypeOfUser"].ToString() == "MD" || Session["TypeOfUser"].ToString() == "VP" || Session["TypeOfUser"].ToString() == "NJSD")
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
                DataSet ds = new DataSet();
                if (Session["TypeOfUser"].ToString() == "U" || Session["TypeOfUser"].ToString() == "MD" || Session["TypeOfUser"].ToString() == "VP" || Session["TypeOfUser"].ToString() == "NJSD")
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

                    DDLWorkPackage_SelectedIndexChanged(sender, e);
                    //
                    BindResourceType();
                }
            }

        }

        protected void DDLWorkPackage_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDLWorkPackage.SelectedValue != "")
            {
                ByMonth.Visible = true;
                Bind_Year(DDLWorkPackage.SelectedValue);
                ResourceDeployment.Visible = false;
                lbl3.Visible = false;
            }
        }

        private void Bind_Year(string WorkpackageUID)
        {
            DataSet ds = getdt.GetWorkPackages_By_WorkPackageUID(new Guid(WorkpackageUID));
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["StartDate"].ToString() != "" && ds.Tables[0].Rows[0]["PlannedEndDate"].ToString() != "")
                {
                    try
                    {
                        DDLYear.Items.Clear();
                        //DDLMonth.Items.Clear();
                        DDLYear.Items.Add(new System.Web.UI.WebControls.ListItem("--Select Year--", ""));
                        DDLMonth.Items.Add(new System.Web.UI.WebControls.ListItem("--Select Month--", ""));

                        int StartYear = Convert.ToDateTime(ds.Tables[0].Rows[0]["StartDate"].ToString()).Year;
                        int EndDateYear = Convert.ToDateTime(ds.Tables[0].Rows[0]["PlannedEndDate"].ToString()).Year;

                        for (int i = StartYear; i <= EndDateYear; i++)
                        {
                            DDLYear.Items.Add(new System.Web.UI.WebControls.ListItem(i.ToString(), i.ToString()));
                        }
                    }
                    catch (Exception ex)
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error : " + ex.Message + "');</script>");
                    }
                }
            }

        }

        private int SundaysInMonth(int month,int year)
        {
            DateTime today = new DateTime(year,month,1);
            DateTime endOfMonth = new DateTime(year, month, DateTime.DaysInMonth(year, month));
            //get only last day of month
            int day = endOfMonth.Day;

            DateTime now = DateTime.Now;
            int count;
            count = 0;
            for (int i = 0; i < day; ++i)
            {
                DateTime d = new DateTime(year, month , i + 1);
                //Compare date with sunday
                if (d.DayOfWeek == DayOfWeek.Sunday & d <= DateTime.Now.Date)
                {
                    count = count + 1;
                }
            }

            return count;
        }

        protected void BntSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (DDLYear.SelectedValue != "" && DDLMonth.SelectedValue != "")
                {
                    headingReport.InnerHtml = "Report Name : Status of Resource Deployment in the Month of " + DDLMonth.SelectedItem.Text + " " + DDLYear.SelectedValue;
                    headingProject.InnerHtml = "Project Name : " + DDlProject.SelectedItem.Text + " (" + DDLWorkPackage.SelectedItem.Text + ")";

                    tDeployed.Visible = true;
                    aDeployed.Visible = true;
                    divTotal.Visible = true;

                    int month = Convert.ToInt32(DDLMonth.SelectedValue);
                    int year = Convert.ToInt32(DDLYear.SelectedValue);

                    int MonthDays = 0;

                    //DateTime LastDate = getdt.GetLastDateInResourceDeployment(new Guid(DDLWorkPackage.SelectedValue), "Labour");

                    //if (LastDate.Month == DateTime.Now.Month & LastDate.Year == DateTime.Now.Year)
                    //    MonthDays = (DateTime.Now.Day - SundaysInMonth(LastDate.Month, LastDate.Year));
                    //else
                    //    MonthDays = DateTime.DaysInMonth(year, month) - SundaysInMonth(month, year);

                    ResourceDeployment.Visible = true;
                    //ActivityHeading.Text = "Status of Resource Deployment in the Month of " + DDLMonth.SelectedItem.Text + " " + DDLYear.SelectedValue;
                    string sDate1 = "";
                    DateTime CDate1 = DateTime.Now;

                    sDate1 = "01/" + DDLMonth.SelectedValue + "/" + DDLYear.SelectedValue;
                    sDate1 = getdt.ConvertDateFormat(sDate1);
                    CDate1 = Convert.ToDateTime(sDate1);

                    DataSet ds1 = getdt.GetResourceDeployment_by_DayWiseforMonthGraph(new Guid(DDLWorkPackage.SelectedValue), CDate1, new Guid(DDLResourceType.SelectedValue));

                    DataTable dt = ds1.Tables[0];
                    var query = from row in dt.AsEnumerable()
                                select new tClass
                                {
                                    dDate = Convert.ToDateTime(row[0].ToString()),
                                    dQuantity = Convert.ToDecimal(row[1].ToString())
                                };

                    tDeployed.Text = Math.Round(query.Sum(a => a.dQuantity),0).ToString();

                    if (query.Count() > 0)
                        aDeployed.Text = Math.Round( (query.Sum(a => a.dQuantity) / ds1.Tables[0].Rows.Count),0).ToString();
                    else
                        aDeployed.Text = "0";

                    if (RBLReportFor.SelectedIndex == 0)
                    {
                        ltScript_deployment.Visible = false;
                        GrdResourceDeployment.Visible = true;

                        DataSet ds = getdt.GetResourceDeployment_by_WorkpackageUID_Month(new Guid(DDLWorkPackage.SelectedValue), CDate1);

                        GrdResourceDeployment.DataSource = ds;
                        GrdResourceDeployment.DataBind();
                        GrdResourceDeployment.Columns[4].Visible = true;
                        GrdResourceDeployment.Columns[1].Visible = false;
                        divTotal.Visible = false;


                    }
                    else if(RBLReportFor.SelectedIndex == 1)
                    {
                        ltScript_deployment.Visible = false;
                        GrdResourceDeployment.Visible = true;

                        DataSet ds = getdt.GetResourceDeployment_by_DayWiseforMonth(new Guid(DDLWorkPackage.SelectedValue), CDate1,new Guid(DDLResourceType.SelectedValue));

                        GrdResourceDeployment.DataSource = ds;
                        GrdResourceDeployment.DataBind();
                        GrdResourceDeployment.Columns[4].Visible = true;
          
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            // btnExportReportPDF.Visible = true;
                            // btnPrintPDF.Visible = true;
                            btnexcelexport.Visible = true;
                            GrdResourceDeployment.Columns[4].Visible = false;

                            GrdResourceDeployment.Columns[1].Visible = true;
                           
                        }
                        else
                        {
                            btnExportReportPDF.Visible = false;
                            btnPrintPDF.Visible = false;
                            btnexcelexport.Visible = false;
                        }
                        divTotal.Visible = true;
                    }
                    else if (RBLReportFor.SelectedIndex == 2)
                    {
                        headingReport.InnerHtml = "Resource Deployment Graph";
                        headingProject.InnerHtml = "Project Name : " + DDlProject.SelectedItem.Text + " (" + DDLWorkPackage.SelectedItem.Text + ")";

                        GrdResourceDeployment.Visible = false;
                        ltScript_deployment.Visible = true;
                        btnExportReportPDF.Visible = false;
                        btnPrintPDF.Visible = false;
                        btnexcelexport.Visible = false;
                        ResourceDeploymentGraph();
                    }
                    
                }
                else
                {
                    if (RBLReportFor.SelectedIndex == 3)
                    {
                        headingReport.InnerHtml = "Resource Deployment overall Graph";
                        headingProject.InnerHtml = "Project Name : " + DDlProject.SelectedItem.Text + " (" + DDLWorkPackage.SelectedItem.Text + ")";

                        GrdResourceDeployment.Visible = false;
                        ltScript_deployment.Visible = true;
                        btnExportReportPDF.Visible = false;
                        btnPrintPDF.Visible = false;
                        btnexcelexport.Visible = false;
                        ResourceDeployment.Visible = true;
                        
                        ResourceDeploymentOverallGraph();
                    }
                    else
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Please select Month or Year');</script>");
                    }
                }

                
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('There is a problem with this feature. Please contact system admin.');</script>");
            }
        }

        private void BindResourceType()
        {
            DDLResourceType.DataTextField = "ResourceType_Name";
            DDLResourceType.DataValueField = "ResourceType_UID";
            DDLResourceType.DataSource = getdt.getResourceTypeMaster_List();
            DDLResourceType.DataBind();
            if(DDlProject.SelectedItem.ToString() =="CP-02" || DDlProject.SelectedItem.ToString() == "CP-03" || DDlProject.SelectedItem.ToString() == "CP-09")
            {
                DDLResourceType.SelectedValue = "91888a63-5a7c-41dc-94df-9a3a53837e21"; //for Labour;
            }

        }

        protected void btnExportReportPDF_Click(object sender, EventArgs e)
        {
            ExporttoPDF(GrdResourceDeployment, 2, "No");
        }
        private void ExporttoPDF(GridView gd, int type,string isPrint)
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

                //if (ProjectName.Length > 30)
                //{
                //    ProjectName = ProjectName.Substring(0, 29) + "..";
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
                Phrase phApplicationName = new Phrase(Environment.NewLine + WebConfigurationManager.AppSettings["Domain"] + " Report Name: Status of Resource Deployment in the Month of " + DDLMonth.SelectedItem.Text + " " + DDLYear.SelectedValue, FontFactory.GetFont("Arial", ApplicationNameSize, iTextSharp.text.Font.BOLD));

                mainTable.SetWidths(new float[] { 8, 35, 15, 10, 10, 22 });

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
                    if (i == 1)
                    {
                        cl.HorizontalAlignment = Element.ALIGN_LEFT;
                        mainTable.AddCell(cl);
                    }
                    else if (i == 2)
                    {
                        cl.HorizontalAlignment = Element.ALIGN_LEFT;
                        mainTable.AddCell(cl);
                    }
                    else if (i == 5)
                    {
                        cl.HorizontalAlignment = Element.ALIGN_LEFT;
                        mainTable.AddCell(cl);
                    }
                    else
                    {
                        cl.HorizontalAlignment = Element.ALIGN_CENTER;
                        mainTable.AddCell(cl);
                    }
                   
                }

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
                                if (columnNo == 1)
                                {
                                    if (gdRp.Columns[columnNo] is TemplateField)
                                    {
                                        DataBoundLiteralControl lc = gdRp.Rows[rowNo].Cells[columnNo].Controls[0] as DataBoundLiteralControl;
                                        string s = lc.Text.Trim();
                                        Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
                                        PdfPCell cl = new PdfPCell(ph);
                                        cl.HorizontalAlignment = Element.ALIGN_LEFT;
                                        mainTable.AddCell(cl);
                                    }
                                    else
                                    {
                                        string s = gdRp.Rows[rowNo].Cells[columnNo].Text.Trim();
                                        s = s.Replace("&nbsp;", "");
                                        Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
                                        PdfPCell cl = new PdfPCell(ph);
                                        cl.HorizontalAlignment = Element.ALIGN_LEFT;
                                        mainTable.AddCell(cl);
                                    }
                                }
                                else if (columnNo == 2)
                                {
                                    if (gdRp.Columns[columnNo] is TemplateField)
                                    {
                                        DataBoundLiteralControl lc = gdRp.Rows[rowNo].Cells[columnNo].Controls[0] as DataBoundLiteralControl;
                                        string s = lc.Text.Trim();
                                        Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
                                        PdfPCell cl = new PdfPCell(ph);
                                        cl.HorizontalAlignment = Element.ALIGN_LEFT;
                                        mainTable.AddCell(cl);
                                    }
                                    else
                                    {
                                        string s = gdRp.Rows[rowNo].Cells[columnNo].Text.Trim();
                                        s = s.Replace("&nbsp;", "");
                                        Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
                                        PdfPCell cl = new PdfPCell(ph);
                                        cl.HorizontalAlignment = Element.ALIGN_LEFT;
                                        mainTable.AddCell(cl);
                                    }
                                }
                                else if (columnNo == 3)
                                {
                                    if (gdRp.Columns[columnNo] is TemplateField)
                                    {
                                        DataBoundLiteralControl lc = gdRp.Rows[rowNo].Cells[columnNo].Controls[0] as DataBoundLiteralControl;
                                        string s = lc.Text.Trim();
                                        Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
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
                                else if (columnNo == 5)
                                {
                                    if (gdRp.Columns[columnNo] is TemplateField)
                                    {
                                        DataBoundLiteralControl lc = gdRp.Rows[rowNo].Cells[columnNo].Controls[0] as DataBoundLiteralControl;
                                        string s = lc.Text.Trim();
                                        Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
                                        PdfPCell cl = new PdfPCell(ph);
                                        cl.HorizontalAlignment = Element.ALIGN_LEFT;
                                        mainTable.AddCell(cl);
                                    }
                                    else
                                    {
                                        string s = gdRp.Rows[rowNo].Cells[columnNo].Text.Trim();
                                        s = s.Replace("&nbsp;", "");
                                        Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
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
                                        Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
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
                    Response.AddHeader("content-disposition", "attachment;filename=Report_ResourceDeployment_Month(" + DDLMonth.SelectedItem.Text + " " + DDLYear.SelectedValue + ")" + DateTime.Now.Ticks + ".pdf");
                    Response.End();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnPrintPDF_Click(object sender, EventArgs e)
        {
            ExporttoPDF(GrdResourceDeployment, 2, "Yes");
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            //required to avoid the runtime error "  
            //Control 'GridView1' of type 'GridView' must be placed inside a form tag with runat=server."  
        }

        protected void btnexcelexport_Click(object sender, EventArgs e)
        {
            try
            {
                StringBuilder StrExport = new StringBuilder();

                StringWriter stw = new StringWriter();
                HtmlTextWriter htextw = new HtmlTextWriter(stw);
                htextw.AddStyleAttribute("font-size", "9pt");
                htextw.AddStyleAttribute("color", "Black");
                
               

                GrdResourceDeployment.RenderControl(htextw); //Name of the Panel

                //var sb1 = new StringBuilder();
                //GridDiv.RenderControl(new HtmlTextWriter(new StringWriter(sb1)));

                string s = htextw.InnerWriter.ToString();

                string HTMLstring = "<html><body>" +
                    "<div style='width:100%; margin:auto;'><div style='width:100%; float:left; line-height:25px; font-size:12pt;' align='center'>" +
                    "<asp:Label ID='Lbl1' runat='server' Font-Bold='true'>" + WebConfigurationManager.AppSettings["Domain"] + "Report Name : Status of Resource Deployment in the Month of " + DDLMonth.SelectedItem.Text + " " + DDLYear.SelectedValue + "</asp:Label><br />" +
                    "<asp:Label ID='Lbl4' runat='server'>Project Name : " + DDlProject.SelectedItem.Text + " (" + DDLWorkPackage.SelectedItem.Text + ")</asp:Label><br />" +
                    "</div> <div style='width:100%; float:left; height:10px;'>&nbsp;&nbsp;&nbsp;</div>" +
                    "<div style='width:100%; float:left;'>" +
                    s +
                    "</div>" +
                    "</div></body></html>";

                string strFile = "Report__ResourceDeployment_" + DateTime.Now.Ticks + ".xls";
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

        protected void RBLReportFor_SelectedIndexChanged(object sender, EventArgs e)
        {
            ResourceDeployment.Visible = false;
            if (RBLReportFor.SelectedIndex == 0)
            {
                DDLResourceType.Visible = false;
                DDLYear.Visible = true;
                DDLMonth.Visible = true;
                lbl1.Visible = true;
                lbl2.Visible = true;
                lbl3.Visible = false;
                ltScript_deployment.Visible = false;
            }
            else if (RBLReportFor.SelectedIndex == 3)
            {
                DDLYear.SelectedIndex = -1;
                DDLMonth.SelectedIndex = -1;
                DDLYear.Visible = false;
                DDLMonth.Visible = false;
                DDLResourceType.Visible = true;
                lbl1.Visible = false;
                lbl2.Visible = false;
                lbl3.Visible = true;
            }
            else
            {
                DDLYear.Visible = true;
                DDLMonth.Visible = true;
                DDLResourceType.Visible = true;
                lbl1.Visible = true;
                lbl2.Visible = true;
                lbl3.Visible = true;
                ltScript_deployment.Visible = false;
            }
        }


        //        private void ResourceDeploymentGraph()
        //        {
        //            try
        //            {
        //                //  DateTime t1 = DateTime.Now;

        //                if (DDLWorkPackage.SelectedValue != "--Select--")
        //                {
        //                    string sDate1 = "";
        //                    DateTime CDate1 = DateTime.Now;

        //                    sDate1 = "01/" + DDLMonth.SelectedValue + "/" + DDLYear.SelectedValue;
        //                    sDate1 = getdt.ConvertDateFormat(sDate1);
        //                    CDate1 = Convert.ToDateTime(sDate1);

        //                    ltScript_deployment.Text = string.Empty;

        //                    // DataSet ds = getdt.GetGraphPhysicalProgressValues1(DDlProject.SelectedValue, DDLWorkPackage.SelectedValue);
        //                    DataSet ds = getdt.GetResourceDeployment_by_DayWiseforMonthGraph(new Guid(DDLWorkPackage.SelectedValue), CDate1, new Guid(DDLResourceType.SelectedValue));


        //                    if (ds.Tables[0].Rows.Count > 0)
        //                    {
        //                        StringBuilder strScript = new StringBuilder();


        //                            strScript.Append(@" <script type='text/javascript'>

        //                google.charts.load('current', { packages: ['corechart', 'bar'] });
        //                google.charts.setOnLoadCallback(drawBasic);

        //                function drawBasic() {

        //                var data = google.visualization.arrayToDataTable([
        //          ['Date', 'Deployed Quantity'],");


        //                      //  int count = 1;

        //                        decimal planvalue = 0;


        //                        // string[] graphValues = ds.Tables[0].Rows[0].ItemArray[0].ToString().Split(';');

        //                        DateTime  d = Convert.ToDateTime(ds.Tables[0].Rows[0].ItemArray[0].ToString());

        //                        int month = d.Month;
        //                        int next_month = month;
        //                        bool cont = true;

        //                        DataTable dt = ds.Tables[0];
        //                        var query = from row in dt.AsEnumerable()
        //                        select new tClass
        //                        {
        //                             dDate = Convert.ToDateTime( row[0].ToString()),
        //                             dQuantity = Convert.ToDecimal(row[1].ToString())
        //                        };

        //                        DateTime d1 = new DateTime(d.Year, d.Month, 1);

        //                        while (cont)
        //                        {

        //                            next_month = d1.Month;
        //                            if (next_month > month)
        //                            {
        //                                cont = false;
        //                            }

        //                            if (query.Where(a=>a.dDate == d1).Count() > 0)
        //                            {
        //                                planvalue = decimal.Parse(query.Single(a => a.dDate == d1).dQuantity.ToString());


        //                                if (cont)
        //                                {
        //                                    strScript.Append("['" + d1.Day.ToString() + "'," + planvalue + "],");
        //                                }
        //                                else
        //                                {
        //                                    strScript.Append("['" + d1.Day.ToString() + "'," + planvalue + "]]);");
        //                                }

        //                            }
        //                            else
        //                            {

        //                                planvalue = 0;

        //                                if (cont)
        //                                {
        //                                    strScript.Append("['" + d1.Day.ToString() + "'," + planvalue + "],");
        //                                }
        //                                else
        //                                {
        //                                    strScript.Append("['" + d1.Day.ToString() + "'," + planvalue + "]]);");
        //                                }
        //                            }

        //                            d1 = d1.AddDays(1);


        //                        }

        //                            strScript.Append(@"var options = {
        //          title : 'Time vs Resource Deployed',

        //          hAxis: {title: 'Deployed Dates',titleTextStyle: {
        //        bold:'true',
        //      }},
        //          seriesType: 'bars',
        //          //series: {3: {type: 'line',targetAxisIndex: 1},4: {type: 'line',targetAxisIndex: 1},5: {type: 'line',targetAxisIndex: 1}},//
        //vAxes: {
        //            // Adds titles to each axis.

        //            0: {title: 'Deployed Quantities',titleTextStyle: {
        //        bold:'true',
        //      }},
        //            1: {title: 'Cumulative Plan (%)',titleTextStyle: {
        //        bold:'true',
        //      }}
        //          }
        //        };
        //                var chart = new google.visualization.ComboChart(
        //                  document.getElementById('chart_div'));
        //                 chart.draw(data, options);

        //            }</script>");





        //                        //ltScript_Cost.Text = strScript.ToString();
        //                        ltScript_deployment.Text = strScript.ToString();


        //                    }
        //                    else
        //                    {
        //                        ltScript_deployment.Text = "<h3>No data</h3>";

        //                    }
        //                }

        //            }
        //            catch (Exception ex)
        //            {
        //                throw ex;
        //            }
        //        }


        private void ResourceDeploymentGraph()
        {
            try
            {
                //  DateTime t1 = DateTime.Now;

                if (DDLWorkPackage.SelectedValue != "--Select--")
                {
                    string sDate1 = "";
                    DateTime CDate1 = DateTime.Now;

                    sDate1 = "01/" + DDLMonth.SelectedValue + "/" + DDLYear.SelectedValue;
                    sDate1 = getdt.ConvertDateFormat(sDate1);
                    CDate1 = Convert.ToDateTime(sDate1);

                    ltScript_deployment.Text = string.Empty;

                    // DataSet ds = getdt.GetGraphPhysicalProgressValues1(DDlProject.SelectedValue, DDLWorkPackage.SelectedValue);
                    DataSet ds = getdt.GetResourceDeployment_by_DayWiseforMonthGraph(new Guid(DDLWorkPackage.SelectedValue), CDate1, new Guid(DDLResourceType.SelectedValue));


                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        StringBuilder strScript = new StringBuilder();


                        strScript.Append(@" <script type='text/javascript'>
                
                google.charts.load('current', { packages: ['corechart', 'bar'] });
                google.charts.setOnLoadCallback(drawBasic);

                function drawBasic() {

                var data = google.visualization.arrayToDataTable([
          ['Date', 'Deployed Quantity'],");


                        //  int count = 1;

                        decimal planvalue = 0;


                        // string[] graphValues = ds.Tables[0].Rows[0].ItemArray[0].ToString().Split(';');

                        DateTime d = Convert.ToDateTime(ds.Tables[0].Rows[0].ItemArray[0].ToString());

                        int month = d.Month;
                        int next_month = month;
                        int year = d.Year;
                        int next_year = d.Year;
                        bool cont = true;

                        DataTable dt = ds.Tables[0];
                        var query = (from row in dt.AsEnumerable()
                                     select new tClass
                                     {
                                         dDate = Convert.ToDateTime(row[0].ToString()),
                                         dQuantity = Convert.ToDecimal(row[1].ToString())
                                     }).ToList();

                        DateTime d1 = new DateTime(d.Year, d.Month, 1);

                        while (cont)
                        {

                            next_month = d1.Month;
                            next_year = d1.Year;


                            if (next_month > month || next_year > year)
                            {
                                cont = false;
                            }

                            if (query.Where(a => a.dDate == d1).Count() > 0)
                            {
                                planvalue = decimal.Parse(query.Single(a => a.dDate == d1).dQuantity.ToString());


                                if (cont)
                                {
                                    strScript.Append("['" + d1.Day.ToString() + "'," + planvalue + "],");
                                }
                                else
                                {
                                    strScript.Append("['" + d1.Day.ToString() + "'," + planvalue + "]]);");
                                }

                            }
                            else
                            {

                                planvalue = 0;

                                if (cont)
                                {
                                    strScript.Append("['" + d1.Day.ToString() + "'," + planvalue + "],");
                                }
                                else
                                {
                                    strScript.Append("['" + d1.Day.ToString() + "'," + planvalue + "]]);");
                                }
                            }

                            d1 = d1.AddDays(1);


                        }

                        strScript.Append(@"var options = {
          title : 'Time vs Resource Deployed',
          
          hAxis: {title: 'Deployed Dates',titleTextStyle: {
        bold:'true',
      }},
          seriesType: 'bars',
          //series: {3: {type: 'line',targetAxisIndex: 1},4: {type: 'line',targetAxisIndex: 1},5: {type: 'line',targetAxisIndex: 1}},//
vAxes: {
            // Adds titles to each axis.
          
            0: {title: 'Deployed Quantities',titleTextStyle: {
        bold:'true',
      }},
            1: {title: 'Cumulative Plan (%)',titleTextStyle: {
        bold:'true',
      }}
          }
        };
                var chart = new google.visualization.ComboChart(
                  document.getElementById('chart_div'));
                 chart.draw(data, options);
                
            }</script>");





                        //ltScript_Cost.Text = strScript.ToString();
                        ltScript_deployment.Text = strScript.ToString();


                    }
                    else
                    {
                        ltScript_deployment.Text = "<h3>No data</h3>";

                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private void ResourceDeploymentOverallGraph()
        {
            try
            {
                
                if (DDLWorkPackage.SelectedValue != "--Select--")
                {
                    string[] months = new string[] { "Jan", "Feb","Mar","Apr","May","Jun","Jul","Aug","Sep","Oct","Nov","Dec" };

                    DataSet ds = getdt.GetResourceDeployment_by_OverallGraph(new Guid(DDLWorkPackage.SelectedValue), new Guid(DDLResourceType.SelectedValue));

                    DataTable dt = ds.Tables[0];
                    var query = (from row in dt.AsEnumerable()
                                select new tClass1
                                {
                                    MonthYear = months[Convert.ToInt32(row[0].ToString().Split('/')[1])-1] + " " + row[0].ToString().Split('/')[0],
                                    dQuantity = Convert.ToDecimal(row[1].ToString()),
                                    dAverage =  Convert.ToDecimal(row[2].ToString())
                                });
                    //Average =  Convert.ToDecimal(row[1].ToString())/((DateTime.DaysInMonth(Convert.ToInt32(row[0].ToString().Split('/')[0]), Convert.ToInt32(row[0].ToString().Split('/')[1]))) - SundaysInMonth(Convert.ToInt32(row[0].ToString().Split('/')[1]), Convert.ToInt32(row[0].ToString().Split('/')[0])))
                    // tClass1 lastItem = query.Last();

                    // DateTime  LastDate = getdt.GetLastDateInResourceDeployment(new Guid(DDLWorkPackage.SelectedValue), "Labour");

                    //  if (LastDate.Month == DateTime.Now.Month & LastDate.Year == DateTime.Now.Year)
                    // lastItem.dAverage = lastItem.dQuantity / (DateTime.Now.Day  - SundaysInMonth(LastDate.Month ,LastDate.Year ));


                    tDeployed.Visible = false;
                    aDeployed.Visible = false;
                    divTotal.Visible = false;
                    

                    ltScript_deployment.Text = string.Empty;

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        StringBuilder strScript = new StringBuilder();


                        strScript.Append(@" <script type='text/javascript'>
                
                google.charts.load('current', { packages: ['corechart', 'bar'] });
                google.charts.setOnLoadCallback(drawBasic);

                function drawBasic() {

                var data = google.visualization.arrayToDataTable([
          ['Month', 'Deployed Quantity','Deployed Average'],");


                        decimal planvalue = 0;
                        decimal average = 0;

                        int last = query.Count();

                        int cnt = 0;
                        int noOfDays = 0;
                        int MonthNo = 0;
                        foreach(var item in query.ToList())
                        {

                            planvalue = Math.Round(item.dQuantity,0);
                           // average = item.dAverage;
                            MonthNo = getMonthNo(item.MonthYear.Split(' ')[0]);
                           // noOfDays = getdt.GetResouceAveargeForMonth(new Guid(DDLWorkPackage.SelectedValue), new Guid(DDLResourceType.SelectedValue), MonthNo, int.Parse(item.MonthYear.Split(' ')[1]));
                            average = Math.Round((item.dQuantity / item.dAverage),0);
                            cnt = cnt + 1;

                            if (cnt < last)
                            {
                                strScript.Append("['" + item.MonthYear + "'," + planvalue + "," + average + "],");
                            }
                            else
                            {
                                strScript.Append("['" + item.MonthYear + "'," + planvalue + "," + average  + "]]);");
                            }
                        }

                        

                        strScript.Append(@"var options = {
          title : 'Time vs Resource Deployed',
          
          hAxis: {title: 'Deployed Months',titleTextStyle: {
        bold:'true',
      }},
          seriesType: 'bars',
          //series: {3: {type: 'line',targetAxisIndex: 1},4: {type: 'line',targetAxisIndex: 1},5: {type: 'line',targetAxisIndex: 1}},//
vAxes: {
            // Adds titles to each axis.
          
            0: {title: 'Deployed Quantities',titleTextStyle: {
        bold:'true',
      }},
            1: {title: 'Cumulative Plan (%)',titleTextStyle: {
        bold:'true',
      }}
          }
        };
                var chart = new google.visualization.ComboChart(
                  document.getElementById('chart_div'));
                 chart.draw(data, options);
                
            }</script>");

                       ltScript_deployment.Text = strScript.ToString();
                    }
                    else
                    {
                        ltScript_deployment.Text = "<h3>No data</h3>";
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private int getMonthNo(string Month)
        {
            switch(Month)
            {
                case "Jan": return 1;
                case "Feb": return 2;
                case "Mar": return 3;
                case "Apr": return 4;
                case "May": return 5;
                case "Jun": return 6;
                case "Jul": return 7;
                case "Aug": return 8;
                case "Sep": return 9;
                case "Oct": return 10;
                case "Nov": return 11;
                case "Dec": return 12;
            }

            return 1;
        }

    }


}