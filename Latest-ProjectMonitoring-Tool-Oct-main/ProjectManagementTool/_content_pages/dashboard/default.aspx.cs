using ProjectManagementTool.DAL;
using ProjectManager.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProjectManagementTool.Models;

namespace ProjectManager._content_pages
{
    public partial class dashboard : System.Web.UI.Page
    {
        DBGetData getdt = new DBGetData();
        TaskUpdate gettk = new TaskUpdate();
        Invoice invoice = new Invoice();
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
                    if (WebConfigurationManager.AppSettings["IsContractorPopUp"] == "Yes")
                    {
                        if (Session["IsContractor"].ToString() == "Y" & Session["MsgShown"].ToString() == "N")
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal();", true);
                            Session["MsgShown"] = "Y";
                        }
                    }
                    if (Session["TypeOfUser"].ToString() == "DDE" && WebConfigurationManager.AppSettings["Domain"] == "ONTB" && Session["MsgGeneralDocs"].ToString() == "N")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModalGD();", true);
                        Session["MsgGeneralDocs"] = "Y";
                    }

                    Session["ActivityUID"] = null;
                    BindProject();
                    SelectedProjectWorkpackage("Project");
                    DDlProject_SelectedIndexChanged(sender, e);

                    // added on 17/11/2020
                    DataSet dscheck = new DataSet();
                    dscheck = getdt.GetUsertypeFunctionality_Mapping(Session["TypeOfUser"].ToString());
                    // RdList.Items[1].Attributes.CssStyle.Add("display", "none");
                    //   rdSelect.Items[1].Attributes.CssStyle.Add("display", "none");
                    // rdSelect.Items[2].Attributes.CssStyle.Add("display", "none");
                    // RdList.Items[1].Enabled = false;
                    rdSelect.Items[1].Enabled = false;
                    rdSelect.Items[2].Enabled = false;
                    divCamera.Visible = false;
                    if (dscheck.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dscheck.Tables[0].Rows)
                        {

                            if (dr["Code"].ToString() == "FS" || dr["Code"].ToString() == "FT" || dr["Code"].ToString() == "FZ") // VIEW FINANCIAL PROGRESS OF PROJECT-INDIVIDUAL REGIONS // ALL // INDIVIDUAL PROJECT //
                            {
                                // RdList.Items[1].Attributes.CssStyle.Add("display", "block");
                                //  rdSelect.Items[2].Attributes.CssStyle.Add("display", "block");
                                //  RdList.Items[1].Enabled = true;
                                rdSelect.Items[2].Enabled = true;

                            }
                            if (dr["Code"].ToString() == "FX" || Session["TypeOfUser"].ToString() == "U") //Project progress tracking
                            {

                                // rdSelect.Items[1].Attributes.CssStyle.Add("display", "block");
                                rdSelect.Items[1].Enabled = true;
                            }
                            if (Session["TypeOfUser"].ToString() != "U")
                            {
                                if (dr["Code"].ToString() == "DC") //Project progress tracking
                                {

                                    divCamera.Visible = true;
                                }

                            }
                            else
                            {
                                divCamera.Visible = true;
                            }

                        }
                    }
                    //added on 15/07/2022 for slahuddins new requirements
                    if (Session["TypeOfUser"].ToString() == "DDE")
                    {
                        rdSelect.Items[1].Enabled = false;
                        divAlerts.Visible = false;
                        divIssues.Visible = false;
                        divCostChart.Visible = false;
                        divPhotographs.Visible = false;
                    }
                    //
                    if (WebConfigurationManager.AppSettings["Domain"] == "Suez")
                    {
                        divUsersdocs.Visible = false;
                    }
                    else if (WebConfigurationManager.AppSettings["Domain"] == "ONTB")
                    {
                        RdList3.Visible = false;
                        trSuezblock.Visible = false;
                        if(DDlProject.SelectedItem.ToString() == "CP-02" || DDlProject.SelectedItem.ToString() == "CP-03")
                        {
                            //chnaged here
                            RdList3.Visible = false;
                            trSuezblock.Visible = false;
                           
                        }
                    }
                    else if (WebConfigurationManager.AppSettings["Domain"] == "LNT")
                    {
                        RdList3.Visible = false;
                        trSuezblock.Visible = false;
                    }
                    //
                    if (Session["TypeOfUser"].ToString() == "NJSD")
                    {
                        UploadSitePhotograph.Visible = false;
                        rdSelect.Items[1].Enabled = true;
                    }
                }
            }
        }

        private void DbSyncStatusCount(string WorkpackageUID)
        {
            if (WebConfigurationManager.AppSettings["Dbsync"] == "Yes")
            {
                DataSet ds = getdt.GetDbsync_Status_Count_by_WorkPackageUID(new Guid(WorkpackageUID));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DivSyncedData.Visible = true;
                    LblLastSyncedDate.Text = ds.Tables[0].Rows[0]["CreatedDate"].ToString() != "" ? Convert.ToDateTime(ds.Tables[0].Rows[0]["CreatedDate"].ToString()).ToString("dd MMM yyyy hh:mm tt") : "NA";
                    // LblTotalSourceDocuments.Text = ds.Tables[0].Rows[0]["SourceDocCount"].ToString() != "" ? ds.Tables[0].Rows[0]["DestDocCount"].ToString() : "NA";
                    // LblTotalDestinationDocuments.Text = ds.Tables[0].Rows[0]["DestDocCount"].ToString() != "" ? ds.Tables[0].Rows[0]["SourceDocCount"].ToString() : "NA";
                    //string package = DDlProject.SelectedItem.ToString();
                    //if (package =="CP-04" || package == "CP-07" || package == "CP-08" || package == "CP-10" || package == "CP-12" || package == "CP-25" || package == "CP-27")
                    //{
                    //    LblLastSyncedDate.Text = DateTime.Now.ToString("dd MMM yyyy hh:mm tt");
                    //}

                    LblSourceHeading.Text = DDlProject.SelectedItem.Text + "(" + DDLWorkPackage.SelectedItem.Text + ")";// +" :- "+ WebConfigurationManager.AppSettings["SourceSite"];
                   // LblDestinationHeading.Text = WebConfigurationManager.AppSettings["DestinationSite"];
                    divsyncdetails.Visible = true;
                    //if (WebConfigurationManager.AppSettings["Domain"] == "LNT")
                    //{
                    //    lblONTBTo_No.Text = ds.Tables[0].Rows[0]["DestDocCount"].ToString() != "" ? ds.Tables[0].Rows[0]["DestDocCount"].ToString() : "0";
                    //}
                    //else
                    //{
                    //    lblONTBTo_No.Text = ds.Tables[0].Rows[0]["SourceDocCount"].ToString() != "" ? ds.Tables[0].Rows[0]["SourceDocCount"].ToString() : "0";

                    //}
                    lblReconDocsNo.Text = getdt.GetDashboardReconciliationDocs(new Guid(DDlProject.SelectedValue)).ToString();
                    lblContractorToNo.Text = getdt.GetDashboardContractotDocsSubmitted(new Guid(DDlProject.SelectedValue)).ToString();
                    lblONTBTo_No.Text = getdt.GetDashboardONTBtoContractorDocs(new Guid(DDlProject.SelectedValue)).ToString();
                    //lblONTBTo_No.Text = (int.Parse(lblONTBTo_No.Text) - int.Parse(lblContractorToNo.Text)) > 0  ? (int.Parse(lblONTBTo_No.Text) - int.Parse(lblContractorToNo.Text)).ToString() : "0";
                    lblRABills.Text = getdt.GetInvoiceDetails_by_WorkpackageUID(new Guid(WorkpackageUID)).Rows.Count.ToString();
                    lblInvoices.Text = invoice.GetInvoiceMaster_by_WorkpackageUID(new Guid(WorkpackageUID)).Tables[0].Rows.Count.ToString();
                    lblBankG.Text = getdt.GetBankGuarantee_by_Bank_WorkPackageUID(new Guid(WorkpackageUID)).Tables[0].Rows.Count.ToString();
                    lblInsurance.Text = getdt.GetInsuranceSelect_by_WorkPackageUID(new Guid(WorkpackageUID)).Tables[0].Rows.Count.ToString();
                    lblMeasurements.Text = getdt.GetTaskMeasurementBookForDashboard(new Guid(WorkpackageUID)).Tables[0].Rows.Count.ToString();

                    // make hyper links
                    hlRABills.HRef = "~/_content_pages/rabill-summary/?&PrjUID=" + DDlProject.SelectedValue;
                    hlInvoices.HRef = "~/_content_pages/invoice/?&PrjUID=" + DDlProject.SelectedValue;
                    hlBankGuarantee.HRef = "~/_content_pages/bank-guarantee/?&PrjUID=" + DDlProject.SelectedValue;
                    hlInsurance.HRef = "~/_content_pages/insurance/?&PrjUID=" + DDlProject.SelectedValue;
                    hlContractor.HRef = "~/_content_pages/documents-contractor/?&type=Contractor&PrjUID=" + DDlProject.SelectedValue;
                    hlReconciliationdocs.HRef = "~/_content_pages/documents-contractor/?&type=Recon&PrjUID=" + DDlProject.SelectedValue;
                    hlONTB.HRef = "~/_content_pages/documents-contractor/?&type=Ontb&PrjUID=" + DDlProject.SelectedValue;
                    hlMeasurement.HRef = "~/_content_pages/dashboard-measurment/?&WorkPackageUID=" + DDLWorkPackage.SelectedValue;

                    UploadSitePhotograph.HRef = "/_modal_pages/upload-sitephotograph.aspx?PrjUID=" + DDlProject.SelectedValue + "&WorkPackage=" + DDLWorkPackage.SelectedValue;
                    ViewSitePhotograph.HRef = "/_modal_pages/view-sitephotographs.aspx?PrjUID=" + DDlProject.SelectedValue + "&WorkPackage=" + DDLWorkPackage.SelectedValue;
                    A1.HRef = "~/_content_pages/document-correspondence?ProjectUID=" + DDlProject.SelectedValue + "&WorkPackageUID=" + DDLWorkPackage.SelectedValue;
                    A2.HRef = "~/_content_pages/documents-contractor-replaced/?&type=Ontb&PrjUID=" + DDlProject.SelectedValue;
                    //
                    A3.HRef = "/_content_pages/submittal_documents/default.aspx?ProjectUID=" + DDlProject.SelectedValue + "&WorkPackageUID=" + DDLWorkPackage.SelectedValue;

                    DataSet ds1 = getdt.GetDocumentsBySubmittal(new Guid(DDlProject.SelectedValue), new Guid(DDLWorkPackage.SelectedValue));

                    if (ds1.Tables[0].Rows.Count == 0)
                        A3.Visible = false;
                    else
                        A3.Visible = true;

                    //added on 27/01/2023
                    if ((DDlProject.SelectedItem.ToString() == "CP-02" || DDlProject.SelectedItem.ToString() == "CP-03") && (WebConfigurationManager.AppSettings["Domain"] == "Suez" || WebConfigurationManager.AppSettings["Domain"] == "Suez"))
                    {
                        //
                        ContractorDrawings.Visible = true;
                        lblContractorDrawingsToNo.Text = getdt.GetDashboardContractotDocsSubmitted_Type(new Guid(DDlProject.SelectedValue),"Drawings","Flow 2").ToString();
                        hlContractorDrawings.HRef = "~/_content_pages/documents-contractor/?&type=Drawings&PrjUID=" + DDlProject.SelectedValue + "&FlowName=Flow 2";

                        VendorCredentials.Visible = true;
                        lblVendorCredentialsToNo.Text = getdt.GetDashboardContractotDocsSubmitted_Type(new Guid(DDlProject.SelectedValue), "Vendor Credentials", "Flow 2").ToString();
                        hlVendorCredentails.HRef = "~/_content_pages/documents-contractor/?&type=Vendor Credentials&PrjUID=" + DDlProject.SelectedValue + "&FlowName=Flow 2";

                        IRQA.Visible = true;
                        IRQAToNo.Text = getdt.GetDashboardContractotDocsSubmitted_Type(new Guid(DDlProject.SelectedValue), "Inspection Reports/Quality Assurance", "Flow 2").ToString();
                        hlIRQA.HRef = "~/_content_pages/documents-contractor/?&type=Inspection Reports/Quality Assurance&PrjUID=" + DDlProject.SelectedValue + "&FlowName=Flow 2";

                        OthersF1.Visible = true;
                        OthersF1ToNo.Text = getdt.GetDashboardContractotDocsSubmitted_Type(new Guid(DDlProject.SelectedValue), "Others", "Flow 1").ToString();
                        hlOthersF1.HRef = "~/_content_pages/documents-contractor/?&type=Others&PrjUID=" + DDlProject.SelectedValue + "&FlowName=Flow 1";

                        OthersF2.Visible = true;
                        OthersF2ToNo.Text = getdt.GetDashboardContractotDocsSubmitted_Type(new Guid(DDlProject.SelectedValue), "Others", "Flow 2").ToString();
                        hlOthersF2.HRef = "~/_content_pages/documents-contractor/?&type=Others&PrjUID=" + DDlProject.SelectedValue + "&FlowName=Flow 2";

                        //
                        ReconDocs.Visible = false;
                       // ReconTotal.Visible = true;
                       // ReconExec.Visible = true;
                       // ReconBal.Visible = true;
                        //
                        divMontlyweeklyReport.Visible = true;
                        getTotalConcrete();
                        Bind_ConcreteChart();
                       
                        //
                       // ReconLabour.Visible = true;
                        DataTable dslabour = new DataTable();
                        dslabour = getdt.GetLabourDashboardCount(new Guid(DDLWorkPackage.SelectedValue), "Labour");
                        if(dslabour.Rows.Count > 0)
                        {
                            hllabour.InnerHtml = "Labour (" +Convert.ToDateTime(dslabour.Rows[0]["DeployedDate"].ToString()).ToString("dd/MM/yyyy") + ")";
                            ViewState["LabourMonth"] = Convert.ToDateTime(dslabour.Rows[0]["DeployedDate"].ToString()).ToString("MMM yyyy");
                            ViewState["YearMonth"] = Convert.ToDateTime(dslabour.Rows[0]["DeployedDate"].ToString()).Year.ToString() + "," + Convert.ToDateTime(dslabour.Rows[0]["DeployedDate"].ToString()).Month.ToString();
                           
                            lblLabour.Text = decimal.Truncate(decimal.Parse(dslabour.Rows[0]["Deployed"].ToString())).ToString();
                        }
                        else
                        {
                            ViewState["LabourMonth"] = "N/A";
                            hllabour.InnerHtml = "Labour";
                            lblLabour.Text = "0";
                        }
                        Bind_LabourChart();
                        //
                        divIssues.Visible = false;
                        divAlerts.Visible = false;
                        //
                        //
                        divConcrete.Visible = true;
                        divLabour.Visible = true;
                        //
                        RdList3.Visible = true;
                        trSuezblock.Visible = true;
                    }
                    else
                    {
                        ReconDocs.Visible = true;
                        ReconTotal.Visible = false;
                        ReconExec.Visible = false;
                        ReconBal.Visible = false;
                        ReconLabour.Visible = false;
                        divMontlyweeklyReport.Visible = false;
                        //
                        divConcrete.Visible = false;
                        divLabour.Visible = false;
                        //
                        ContractorDrawings.Visible = false;
                        VendorCredentials.Visible = false;
                        IRQA.Visible = false;
                        OthersF1.Visible = false;
                        OthersF2.Visible = false;
                        //
                        divIssues.Visible = true;
                        divAlerts.Visible = true;
                        //
                        RdList3.Visible = false;
                        trSuezblock.Visible = false;
                    }
                }
                else
                {
                    DivSyncedData.Visible = false;
                    divsyncdetails.Visible = false;
                    RdList3.Visible = false;
                    trSuezblock.Visible = false;
                    //LblLastSyncedDate.Text = "NA";
                    //LblTotalSourceDocuments.Text = "NA";
                    //LblTotalDestinationDocuments.Text = "NA";
                }
                
            }
            else
            {
                DivSyncedData.Visible = false;
            }
        }

        private void getDashboardImages(string WorkpackageUID)
        {
            if (DDLWorkPackage.SelectedValue != "--Select--")
            {
                string ImageName = getdt.GetDashboardImages(new Guid(WorkpackageUID));
                if (!string.IsNullOrEmpty(ImageName))
                {
                    divdashboardimage.Attributes.Add("style", "background-image:url('/_assets/images/" + ImageName + "');width:100% !important; height:100% !important;background-size:100% 100%;background-position: center center;margin: 0px 0px 0px 0px; opacity: 0.9 !important");
                }
                else
                {
                    divdashboardimage.Attributes.Remove("style");
                }
            }
        }
        private void BindProject()
        {
            DataTable ds = new DataTable();
            if (Session["TypeOfUser"].ToString() == "U" || Session["TypeOfUser"].ToString() =="MD" || Session["TypeOfUser"].ToString() == "VP" || Session["TypeOfUser"].ToString() == "NJSD")
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
            DDlProject.Items.Insert(0, "--Select--");
            DDLWorkPackage.Items.Insert(0, "--Select--");

        }

        protected void DDlProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDlProject.SelectedValue != "--Select--")
            {
                //chnage here
                if ((DDlProject.SelectedItem.ToString() == "CP-02" || DDlProject.SelectedItem.ToString() == "CP-03") && WebConfigurationManager.AppSettings["Domain"] == "Suez")
                {
                    RdList3.SelectedIndex = 1;
                    ViewState["vChart"] = null;
                }
                else
                {
                    ViewState["vChart"] = 1;
                }
                //
                divdashboardimage.Visible = true;
                dummyNJSEIdashboard.Visible = false;
                divdummydashboard.Visible = false;
                dummyONTBdashboard.Visible = false;
                DataSet ds = new DataSet();
                //ds = getdt.GetWorkPackages_By_ProjectUID(new Guid(DDlProject.SelectedValue));
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
                //ds = getdt.GetWorkPackages_By_ProjectUID(new Guid(DDlProject.SelectedValue));
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DDLWorkPackage.DataTextField = "Name";
                        DDLWorkPackage.DataValueField = "WorkPackageUID";
                        DDLWorkPackage.DataSource = ds;
                        DDLWorkPackage.DataBind();
                    SelectedProjectWorkpackage("Workpackage");
                    //  DDLWorkPackage.Items.Insert(0, "--Select--");
                    BindResourceMaster();

                    if (ViewState["vChart"] != null)
                    {
                        if (ViewState["vChart"].ToString() != "")
                        {
                            Bind_DocumentsChart();
                            RdList3.SelectedIndex = 0;
                            ViewState["vChart"] = 1;
                        }
                    }
                    else
                    if (Request.QueryString["Option"] != null)
                    {
                        if (Request.QueryString["selection"] == "1")
                        {
                            Bind_DocumentsChart1();
                            RdList3.SelectedIndex = 0;
                            ViewState["vChart"] = 2;
                        }
                        else
                        {
                            //if (Request.QueryString["back"] == "1")
                            //{
                            //    Bind_DocumentsChart4();
                            //    RdList3.SelectedIndex = 1;
                            //    ViewState["vChart"] = 1;
                            //}
                            //else
                            //{
                                Bind_DocumentsChart5();
                                RdList3.SelectedIndex = 1;
                                ViewState["vChart"] = 2;
                            //}
                        }
                    }
                    else
                    {
                        if (RdList3.SelectedValue == "Total")
                        {
                            Bind_DocumentsChart();
                            RdList3.SelectedIndex = 0;
                            ViewState["vChart"] = 1;
                        }
                        else
                        {
                            //Bind_DocumentsChart4();
                            Bind_DocumentsChart5();
                            RdList3.SelectedIndex = 1;
                            ViewState["vChart"] = 2;
                        }
                    }
                    // Bind_DocumentsChart();
                    BindAlerts("WorkPackage");
                        BindActivityPie_Chart("Work Package", DDLWorkPackage.SelectedValue);
                        //Bind_ResourceChart("Work Package", DDLWorkPackage.SelectedValue);
                    // Bind_ProgressChart("Work Package", DDLWorkPackage.SelectedValue);
                    Bind_CostChart("Work Package", DDLWorkPackage.SelectedValue);
                    if (rdSelect.SelectedValue == "1")
                    {
                        LoadGraph(); //Physical progress chart
                    }
                    else if (rdSelect.SelectedValue == "2")
                    {
                        LoadFinancialGraph();
                    }
                        BindCamera(DDLWorkPackage.SelectedValue);
                        heading.InnerHtml = "Physical Progress Chart - " + DDlProject.SelectedItem.Text + " (" + DDLWorkPackage.SelectedItem.Text + ")";
                        headingF.InnerHtml = "Financial Progress Chart - " + DDlProject.SelectedItem.Text + " (" + DDLWorkPackage.SelectedItem.Text + ")";

                    DbSyncStatusCount(DDLWorkPackage.SelectedValue);
                    getDashboardImages(DDLWorkPackage.SelectedValue);
                    //added on 23/03/2023
                    if ((DDlProject.SelectedItem.ToString() == "CP-02" || DDlProject.SelectedItem.ToString() == "CP-03") && WebConfigurationManager.AppSettings["Domain"] == "Suez")
                    {
                        BindDocumentSummary();
                    }
                    Session["Project_Workpackage"] = DDlProject.SelectedValue + "_" + DDLWorkPackage.SelectedValue;
                    // added on 10/01/2022 for docs to act on for the user

                    if (Session["TypeOfUser"].ToString() != "U" && Session["TypeOfUser"].ToString() != "VP" && Session["TypeOfUser"].ToString() != "MD" && Session["TypeOfUser"].ToString() != "NJSD")
                    {
                        divUsersdocs.Visible = true;
                        //if (getUserDocsNo() == 0)
                        //{
                        //    Hluserdocs.HRef = "#";
                        //    Hluserdocs.InnerText = "no documents";
                        //}
                        //else
                        //{
                        //    Hluserdocs.InnerText = getUserDocsNo() + " documents";
                        //    Hluserdocs.HRef = "~/_content_pages/documents-contractor/?&type=Ontb&PrjUID=" + DDlProject.SelectedValue + "&UserUID=" + Session["UserUID"].ToString() + "&WkpgUID=" + DDLWorkPackage.SelectedValue;
                        //}
                    }
                    else
                    {
                        divUsersdocs.Visible = false;
                    }
                }
                    else
                    {
                        DDLWorkPackage.DataSource = null;
                        DDLWorkPackage.DataBind();
                        ltScripts_piechart.Text = "<h4>No data</h4>";
                        ltScript_Progress.Text = "<h4>No data</h4>";
                        ltScript_Document.Text = "<h4>No data</h4>";
                        ltScript_Resource.Text = "<h4>No data</h4>";
                        ltScript_PhysicalProgress.Text= "<h4>No data</h4>";
                        ltScript_FinProgress.Text = "<h4>No data</h4>";
                        divtable.InnerHtml = "";
                        btnPrint.Visible = false;
                }
                //}
            }
            else
            {
                ltScripts_piechart.Text = "<h4>No data</h4>";
                ltScript_Progress.Text = "<h4>No data</h4>";
                ltScript_Document.Text = "<h4>No data</h4>";
                ltScript_Resource.Text = "<h4>No data</h4>";
                ltScript_PhysicalProgress.Text = "<h4>No data</h4>";
                ltScript_FinProgress.Text = "<h4>No data</h4>";
                divtable.InnerHtml = ""; 
                btnPrint.Visible = false;
                DDLWorkPackage.Items.Clear();
                DDLWorkPackage.Items.Insert(0, "--Select--");
                divdashboardimage.Visible = false;
                divUsersdocs.Visible = false;
                if (WebConfigurationManager.AppSettings["Domain"] == "NJSEI")
                {
                    divdummydashboard.Visible = false;
                    dummyNJSEIdashboard.Visible = true;
                    divNJSEIMIS.Visible = false;
                    dummyONTBdashboard.Visible = false;
                }
                else
                {
                    divdummydashboard.Visible = false;
                    dummyONTBdashboard.Visible = true;
                    dummyNJSEIdashboard.Visible = false;
                    divNJSEIMIS.Visible = false;
                   // rdSelect.Items[3].Text = "";
                   // rdSelect.Items[3].Enabled = false;
                }
            }

            //
            if(DDlProject.SelectedItem.ToString() == "CP-25" || DDlProject.SelectedItem.ToString() == "CP-26" || DDlProject.SelectedItem.ToString() == "CP-27")
            {
                alinksummarySTP.Visible = true;
                divContractorCPDocs.Visible = true;
                DataSet ds = getdt.GetDashboardContractotDocsSubmitted_Details(new Guid(DDlProject.SelectedValue));
                if (ds.Tables[0].AsEnumerable()
         .OrderByDescending(r => r.Field<DateTime>("IncomingRec_Date"))
                        .Where(r => r.Field<Guid>("FlowUID").ToString().ToLower().Equals("f76821a4-289b-468f-92d5-f4965481546c")).Count() > 0)
                {
                    DataTable dt = ds.Tables[0].AsEnumerable()
             .OrderByDescending(r => r.Field<DateTime>("IncomingRec_Date"))
                            .Where(r => r.Field<Guid>("FlowUID").ToString().ToLower().Equals("f76821a4-289b-468f-92d5-f4965481546c")).CopyToDataTable();
                    

                    hlContractorCPDocs.HRef = "~/_content_pages/documents-contractor/?&type=Contractor&Flow=Contractor Correspondence&&PrjUID=" + DDlProject.SelectedValue;
                    hlContractorCPDocs.InnerText = "Contractor Correspondence (" + dt.Rows.Count + ")";

                    DataSet ds1 = getdt.ClientDocumentsONTB_BWSSB_Correspondence(new Guid(DDlProject.SelectedValue), new Guid(DDLWorkPackage.SelectedValue), "All","All","");


                    A1.HRef = "~/_content_pages/document-correspondence?ProjectUID=" + DDlProject.SelectedValue + "&WorkPackageUID=" + DDLWorkPackage.SelectedValue;
                    if (ds1 != null)
                    {
                        A1.InnerText = "ONTB/BWSSB Correspondance (" + ds1.Tables[0].Rows.Count + ")";
                    }
                }
                else
                {
                    hlContractorCPDocs.InnerText = "Contractor Correspondence (0)";
                    hlContractorCPDocs.HRef = "#";

                }


            }
            else
            {
                alinksummarySTP.Visible = false;
                divContractorCPDocs.Visible = false;
            }
            //
            if (WebConfigurationManager.AppSettings["Domain"] == "Suez")
            {
                divUsersdocs.Visible = false;
            }
           
        }

        protected void DDLWorkPackage_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDLWorkPackage.SelectedValue != "--Select--")
            {
                getDashboardImages(DDLWorkPackage.SelectedValue);
                BindResourceMaster();
                Bind_DocumentsChart();
                BindAlerts("WorkPackage");
                BindActivityPie_Chart("Work Package", DDLWorkPackage.SelectedValue);
              //  Bind_ResourceChart("Work Package", DDLWorkPackage.SelectedValue);
                //Bind_ProgressChart("Work Package", DDLWorkPackage.SelectedValue);
                Bind_CostChart("Work Package", DDLWorkPackage.SelectedValue);
                BindCamera(DDLWorkPackage.SelectedValue);
                //added on 23/03/2023 for suez
                if ((DDlProject.SelectedItem.ToString() == "CP-02" || DDlProject.SelectedItem.ToString() == "CP-03") && WebConfigurationManager.AppSettings["Domain"] == "Suez")
                {
                    BindDocumentSummary();
                }
                //
                heading.InnerHtml = "Physical Progress Chart - " + DDlProject.SelectedItem.Text + " (" + DDLWorkPackage.SelectedItem.Text + ")";
                headingF.InnerHtml = "Financial Progress Chart - " + DDlProject.SelectedItem.Text + " (" + DDLWorkPackage.SelectedItem.Text + ")";
                if (rdSelect.SelectedValue == "1")
                {
                    LoadGraph(); //Physical progress chart
                }
                else if (rdSelect.SelectedValue == "2")
                {
                    LoadFinancialGraph();
                }
                Session["Project_Workpackage"] = DDlProject.SelectedValue + "_" + DDLWorkPackage.SelectedValue;
                //----------------------
                if (Session["TypeOfUser"].ToString() != "U" && Session["TypeOfUser"].ToString() != "VP" && Session["TypeOfUser"].ToString() != "MD" && Session["TypeOfUser"].ToString() != "NJSD")
                {
                    divUsersdocs.Visible = true;
                    //if (getUserDocsNo() == 0)
                    //{
                    //    Hluserdocs.HRef = "#";
                    //    Hluserdocs.InnerText = "no documents";
                    //}
                    //else
                    //{
                    //    Hluserdocs.InnerText = getUserDocsNo() + " documents";
                    //    Hluserdocs.HRef = "~/_content_pages/documents-contractor/?&type=Ontb&PrjUID=" + DDlProject.SelectedValue + "&UserUID=" + Session["UserUID"].ToString() + "&WkpgUID=" + DDLWorkPackage.SelectedValue;
                    //}
                }
                else
                {
                    divUsersdocs.Visible = false;
                }
                //-----------------------

            }

            if (WebConfigurationManager.AppSettings["Domain"] == "Suez")
            {
                divUsersdocs.Visible = false;
            }
            
        }

        private void SelectedProjectWorkpackage(string pType)
        {
            if (!IsPostBack)
            {
                if (Session["Project_Workpackage"] != null)
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

        }

        private void BindResourceMaster()
        {
            if (DDLWorkPackage.SelectedValue != "--Select--")
            {
                DataSet ds = getdt.getResourceMaster(new Guid(DDLWorkPackage.SelectedValue));
                DDlResource.DataTextField = "ResourceName";
                DDlResource.DataValueField = "ResourceUID";
                DDlResource.DataSource = ds;
                DDlResource.DataBind();
            }
        }

        private void BindCamera(string WorkpackageUID)
        {
            //if (DDLWorkPackage.SelectedValue != "--Select--")
            //{
            //    DataSet ds = getdt.Camera_Selectby_WorkpackageUID(new Guid(WorkpackageUID));
            //    DDLCamera.DataTextField = "Camera_Name";
            //    DDLCamera.DataValueField = "Camera_UID";
            //    DDLCamera.DataSource = ds;
            //    DDLCamera.DataBind();
            //    DDLCamera.Items.Insert(0, "--Select--");
            //}
            DataSet ds = getdt.Camera_Selectby_WorkpackageUID_Dashboard(new Guid(WorkpackageUID));
            string list = string.Empty;
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    list += "<div><a href='" + dr["Camera_IPAddress"].ToString() + "' target=\"_blank\">" + dr["Camera_Name"].ToString() + "</a></div><br/>";
                }
            }
            else
            {
                list = "<div>No Camera added</div>";
            }
            divCameralist.InnerHtml = list;                            
        }

        // old commented out for saji
        //private void Bind_DocumentsChart()
        //{
        //    if (DDLWorkPackage.SelectedValue != "--Select--")
        //    {
        //        DataSet ds = getdt.getDocumentCount_by_ProjectUID_WorkPackageUID(new Guid(DDlProject.SelectedValue), new Guid(DDLWorkPackage.SelectedValue));
        //        if (ds.Tables[0].Rows.Count > 0)
        //        {
        //            StringBuilder strScript = new StringBuilder();
        //            strScript.Append(@"<script type='text/javascript'>
        //        google.charts.load('current', { packages: ['corechart', 'bar'] });
        //        google.charts.setOnLoadCallback(drawBasic);

        //        function drawBasic() {
        //            var data = google.visualization.arrayToDataTable([
        //              ['Document', 'Ontime','Delayed', { role: 'annotation' }],");
        //            strScript.Append("['Tot. Documents', " + ds.Tables[0].Rows[0]["DocCount"].ToString() + ", 0,'" + ds.Tables[0].Rows[0]["DocCount"].ToString() + "'],");
        //            strScript.Append("['Submitted', " + (Convert.ToInt32(ds.Tables[0].Rows[0]["Status1"].ToString()) - Convert.ToInt32(ds.Tables[0].Rows[0]["Status1Delay"].ToString())) + ", " + ds.Tables[0].Rows[0]["Status1Delay"].ToString() + ",'" + ds.Tables[0].Rows[0]["Status1"].ToString() + "'],");
        //            strScript.Append("['Code A', " + (Convert.ToInt32(ds.Tables[0].Rows[0]["Status3"].ToString()) - Convert.ToInt32(ds.Tables[0].Rows[0]["Status3Delay"].ToString())) + ", " + ds.Tables[0].Rows[0]["Status3Delay"].ToString() + ",'" + ds.Tables[0].Rows[0]["Status3"].ToString() + "'],");
        //            strScript.Append("['Code B', " + (Convert.ToInt32(ds.Tables[0].Rows[0]["Status2"].ToString()) - Convert.ToInt32(ds.Tables[0].Rows[0]["Status2Delay"].ToString())) + ", " + ds.Tables[0].Rows[0]["Status2Delay"].ToString() + ",'" + ds.Tables[0].Rows[0]["Status2"].ToString() + "'],");
        //            strScript.Append("['Code C', " + (Convert.ToInt32(ds.Tables[0].Rows[0]["CodeC"].ToString()) - Convert.ToInt32(ds.Tables[0].Rows[0]["CodeCDelay"].ToString())) + ", " + ds.Tables[0].Rows[0]["CodeCDelay"].ToString() + ",'" + ds.Tables[0].Rows[0]["CodeC"].ToString() + "'],");
        //            strScript.Append("['Code D', " + (Convert.ToInt32(ds.Tables[0].Rows[0]["CodeD"].ToString()) - Convert.ToInt32(ds.Tables[0].Rows[0]["CodeDDelay"].ToString())) + ", " + ds.Tables[0].Rows[0]["CodeDDelay"].ToString() + ",'" + ds.Tables[0].Rows[0]["CodeD"].ToString() + "'],");
        //            strScript.Append("['Code E', " + (Convert.ToInt32(ds.Tables[0].Rows[0]["CodeE"].ToString()) - Convert.ToInt32(ds.Tables[0].Rows[0]["CodeEDelay"].ToString())) + ", " + ds.Tables[0].Rows[0]["CodeEDelay"].ToString() + ",'" + ds.Tables[0].Rows[0]["CodeE"].ToString() + "'],");
        //            strScript.Append("['Code F', " + (Convert.ToInt32(ds.Tables[0].Rows[0]["CodeF"].ToString()) - Convert.ToInt32(ds.Tables[0].Rows[0]["CodeFDelay"].ToString())) + ", " + ds.Tables[0].Rows[0]["CodeFDelay"].ToString() + ",'" + ds.Tables[0].Rows[0]["CodeF"].ToString() + "'],");
        //            strScript.Append("['Code G', " + (Convert.ToInt32(ds.Tables[0].Rows[0]["CodeG"].ToString()) - Convert.ToInt32(ds.Tables[0].Rows[0]["CodeGDelay"].ToString())) + ", " + ds.Tables[0].Rows[0]["CodeGDelay"].ToString() + ",'" + ds.Tables[0].Rows[0]["CodeG"].ToString() + "'],");
        //            strScript.Append("['Code H', " + (Convert.ToInt32(ds.Tables[0].Rows[0]["CodeH"].ToString()) - Convert.ToInt32(ds.Tables[0].Rows[0]["CodeHDelay"].ToString())) + ", " + ds.Tables[0].Rows[0]["CodeHDelay"].ToString() + ",'" + ds.Tables[0].Rows[0]["CodeH"].ToString() + "'],");
        //            strScript.Append("['Client Approved', " + (Convert.ToInt32(ds.Tables[0].Rows[0]["Status4"].ToString()) - Convert.ToInt32(ds.Tables[0].Rows[0]["Status4Delay"].ToString())) + "," + ds.Tables[0].Rows[0]["Status4Delay"].ToString() + ",'" + ds.Tables[0].Rows[0]["Status4"].ToString() + "'],");
        //            strScript.Remove(strScript.Length - 1, 1);
        //            strScript.Append("]);");
        //            strScript.Append(@"var options = {
        //                is3D: true,
        //                legend: { position: 'none' },
        //                fontSize: 13,
        //                isStacked: true,
        //                chartArea: {
        //                    left: '25%',
        //                    top: '5%',
        //                    height: '88%',
        //                    width: '61%'
        //                },
        //                bars: 'horizontal',
        //                annotations: {
        //                alwaysOutside:true,
        //                },
        //                axes: {
        //                    x: {
        //                        0: { side: 'top', label: 'Percentage' } // Top x-axis.
        //                    }
        //                },
        //                hAxis: {
        //                    minValue: 0
        //                }
        //            };
        //            function selectHandler()
        //            {
        //                var selection = chart.getSelection();
        //                if (selection.length > 0)
        //                {
        //                    var colLabel = data.getColumnLabel(selection[0].column);
        //                    var mydata = data.getValue(selection[0].row,0);
        //                    ");
        //            strScript.Append("window.open('/_content_pages/document-drilldown/default.aspx?DocumentType=' + (colLabel + '_' + mydata) + '&ProjectUID=" + DDlProject.SelectedValue + "&WorkPackageUID=" + DDLWorkPackage.SelectedValue + "', '_self', true);");
        //            //alert('The user selected ' + topping);
        //            strScript.Append(@"}
        //            }

        //            var chart = new google.visualization.BarChart(document.getElementById('DocChart_Div'));
        //            google.visualization.events.addListener(chart, 'select', selectHandler);
        //            chart.draw(data, options);
        //        }
        //    </script>");
        //            ltScript_Document.Text = strScript.ToString();
        //        }
        //        else
        //        {
        //            ltScript_Document.Text = "<h4>No data</h4>";
        //        }
        //    }
        //}

        //private void Bind_DocumentsChart()
        //{

        //    DataSet ds = getdt.getDocumentCount_by_ProjectUID_WorkPackageUID(new Guid(DDlProject.SelectedValue), new Guid(DDLWorkPackage.SelectedValue));
        //    BtoF.Visible = false;
        //    if (ds == null)
        //    {
        //        ltScript_Document.Text = "<h4>No data</h4>";
        //    }
        //    else
        //    {
        //        if (ds.Tables[0].Rows.Count > 0)
        //        {
        //            StringBuilder strScript = new StringBuilder();

        //            strScript.Append(@"<script type='text/javascript'>
        //                        google.charts.load('current', { packages: ['corechart', 'bar'] });
        //                        google.charts.setOnLoadCallback(drawBasic);

        //                    function drawBasic() {
        //                        var data = google.visualization.arrayToDataTable([
        //                            ['Document','FlowAll','Delayed', { role: 'annotation' }],");

        //            string total_docs = ds.Tables[0].Rows[0][1].ToString();

        //            foreach (DataRow rw in ds.Tables[0].Rows)
        //            {
        //                strScript.Append("['" + rw[2].ToString() + "', " + Convert.ToInt32(rw[1].ToString()) + ", " + "0" + ",'" + Convert.ToInt32(rw[1].ToString()) + "'],");
        //            }

        //            strScript.Remove(strScript.Length - 1, 1);
        //            strScript.Append("]);");
        //            strScript.Append(@"var options = {
        //                        is3D: true,
        //                        legend: { position: 'none' },
        //                        fontSize: 11,
        //                        isStacked: true,
        //                        height : 300,
        //                        chartArea: {
        //                            left: '25%',
        //                            top: '5%',
        //                            height: '100%',
        //                            width: '61%'
        //                        },
        //                        bars: 'horizontal',
        //                        annotations: {
        //                        alwaysOutside:false,
        //                        },
        //                        axes: {
        //                            x: {
        //                                0: { side: 'top', label: 'Percentage' } // Top x-axis.
        //                            }
        //                        },
        //                        hAxis: {
        //                            minValue: 0
        //                        }
        //                    };

        //                    function selectHandler()
        //                    {
        //                        var selection = chart.getSelection();
        //                        if (selection.length > 0)
        //                        {
        //                            var colLabel = data.getColumnLabel(selection[0].column);
        //                            var mydata = data.getValue(selection[0].row,0);
        //                            ");
        //            strScript.Append("window.open('/_content_pages/document-drilldown/default.aspx?DocumentType=' + (colLabel + '_' + mydata) + '&ProjectUID=" + DDlProject.SelectedValue + "&WorkPackageUID=" + DDLWorkPackage.SelectedValue + "', '_self', true);");

        //            strScript.Append(@"}
        //                    }

        //                    var chart = new google.visualization.BarChart(document.getElementById('DocChart_Div'));
        //                    google.visualization.events.addListener(chart, 'select', selectHandler);
        //                    chart.draw(data, options);
        //                }
        //            </script>");
        //            ltScript_Document.Text = strScript.ToString();
        //        }
        //        else
        //        {
        //            ltScript_Document.Text = "<h4>No data</h4>";
        //        }
        //    }
        //}

        private void BindAlerts(string By)
        {
            if (DDLWorkPackage.SelectedValue != "--Select--")
            {
                DataSet ds = new DataSet();
                if (By == "Project")
                {
                    ds = getdt.getAlerts_by_ProjectUID(new Guid(DDlProject.SelectedValue));
                }
                else if (By == "WorkPackage")
                {
                    ds = getdt.getAlerts_by_WorkPackageUID(new Guid(DDlProject.SelectedValue), new Guid(DDLWorkPackage.SelectedValue));
                }
                //else
                //{
                //    ds = getdt.getAlerts_by_TaskUID(new Guid(DDLTask.SelectedValue));
                //}
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string s1;
                    s1 = "<table class='table table-borderless'>";

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        //s1 += "<tr><td style='width:80%; color:#006699; font-size:larger;'>" + ds.Tables[0].Rows[i]["Alert_Text"].ToString() + "</td>" + "<td style='width:20%; text-align:right;'>" + Convert.ToDateTime(ds.Tables[0].Rows[i]["Alert_Date"].ToString()).ToString("dd/MM/yyyy") + "</td></tr>";
                        s1 += "<tr style='border-bottom:1px dotted Gray; margin-left:0px;'><td>" + ds.Tables[0].Rows[i]["Alert_Text"].ToString() + "</td></tr>";
                    }
                    s1 += "</table>";
                    lt1.Text = s1.ToString();
                }
                else
                {
                    lt1.Text = "<h4>No Alerts Found</h4>";
                }
            }
        }

        private void BindActivityPie_Chart(string ActivityType, string Activity_ID)
        {
            if (DDLWorkPackage.SelectedValue != "--Select--")
            {
                DataSet ds = getdt.Get_Open_Closed_Rejected_Issues_by_WorkPackageUID(new Guid(Activity_ID));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    StringBuilder strScript = new StringBuilder();
                    strScript.Append(@"<script type='text/javascript'>        
                google.charts.load('current', { packages: ['corechart'] });
                google.charts.setOnLoadCallback(drawChart);

                 function drawChart() {
                     var data = google.visualization.arrayToDataTable([
                       ['Issues', 'Count'],");
                    strScript.Append("['Open Issues', " + ds.Tables[0].Rows[0]["OpenIssues"].ToString() + "], ['In-Progress Issues', " + ds.Tables[0].Rows[0]["InProgressIssues"].ToString() + "], ['Closed Issues', " + ds.Tables[0].Rows[0]["ClosedIssues"].ToString() + "], ['Rejected Issues', " + ds.Tables[0].Rows[0]["RejectedIssues"].ToString() + "]]);");
                    strScript.Append(@"var options = {
                         is3D: true,
                         legend: { position: 'labeled', textStyle: { color: 'black', fontSize: 13 } },
                         colors: ['#3366CC', '#FF9900', '#109618', '#DC3912'],
                         pieSliceText: 'value',
                         pieSliceTextStyle: { bold: true, fontSize: 13 },
                         chartArea: {                        
                             height: '92%',
                             width: '92%'
                         }
                     };
                        function selectHandler() {
                        var selection = chart.getSelection(); 
                        if (selection.length > 0) {
                        var selectedItem = chart.getSelection()[0];
                        if (selectedItem) {
                           window.open('/_content_pages/issues', '_self', true);
                         }
                        }
                     }
                     var chart = new google.visualization.PieChart(document.getElementById('piechart_3d'));
                      google.visualization.events.addListener(chart, 'select', selectHandler);
                     chart.draw(data, options);}
                    </script>");
                    ltScripts_piechart.Text = strScript.ToString();
                }
                else
                {
                    ltScripts_piechart.Text = "<h4>No data</h4>";
                }

                //DataSet ds = getdt.GetTask_Status_Count(Session["UserUID"].ToString(), ActivityType, Activity_ID);
                //if (ds.Tables[0].Rows.Count > 0)
                //{

                //    StringBuilder strScript = new StringBuilder();
                //    strScript.Append(@"<script type='text/javascript'>        
                //google.charts.load('current', { packages: ['corechart'] });
                //google.charts.setOnLoadCallback(drawChart);

                // function drawChart() {
                //     var data = google.visualization.arrayToDataTable([
                //       ['Task', 'Hours per Day'],");
                //    strScript.Append("['Not Started', " + ds.Tables[0].Rows[0]["Pending"].ToString() + "], ['Completed', " + ds.Tables[0].Rows[0]["Completed"].ToString() + "], ['In Progress', " + ds.Tables[0].Rows[0]["Inprogress"].ToString() + "]]);");
                //    strScript.Append(@"var options = {
                //         is3D: true,
                //         legend: { position: 'labeled', textStyle: { color: 'black', fontSize: 13 } },
                //         pieSliceText: 'value',
                //         pieSliceTextStyle: { bold: true, fontSize: 13 },
                //         chartArea: {                        
                //             height: '92%',
                //             width: '92%'
                //         }
                //     };
                //     var chart = new google.visualization.PieChart(document.getElementById('piechart_3d'));  
                //     chart.draw(data, options);}
                //    </script>");
                //    ltScripts_piechart.Text = strScript.ToString();


                //    //function selectHandler()
                //    //{
                //    //    var selectedItem = chart.getSelection()[0];
                //    //    if (selectedItem)
                //    //    {
                //    //        var topping = data.getValue(selectedItem.row, 0); ");
                //    //  strScript.Append("window.open('WorkPackages.aspx?TaskType=' + topping + '&ProjectUID=" + DDlProject.SelectedValue + "&WorkPackageUID=" + DDLWorkPackage.SelectedValue + "', '_self', true);");
                //    //        //alert('The user selected ' + topping);
                //    //        strScript.Append(@"}
                //    //}
                //    //google.visualization.events.addListener(chart, 'select', selectHandler);

                //    //}
                //    //else
                //    //{
                //    //    ltScripts_piechart.Text = " < h4>No data</h4>";
                //    //}
                //}
                //else
                //{
                //    ltScripts_piechart.Text = "<h4>No data</h4>";
                //}
            }

        }

        private void Bind_ProgressChart(string ActivityType, string Activity_ID)
        {
            if (DDLWorkPackage.SelectedValue != "--Select--")
            {
                DataSet ds = getdt.GetConstructionProgramme_Tasks(new Guid(Activity_ID));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    StringBuilder strScript = new StringBuilder();
                    strScript.Append(@"<script type='text/javascript'>
                google.charts.load('current', { packages: ['corechart', 'bar'] });
                google.charts.setOnLoadCallback(drawBasic);
                function drawBasic() {
                var data = google.visualization.arrayToDataTable([
                ['Task', 'Target', 'Cumulative achieved'],");
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        string tName = ds.Tables[0].Rows[i]["Name"].ToString();
                        if (tName.Length > 20)
                        {
                            tName = ds.Tables[0].Rows[i]["Name"].ToString().Substring(0, 20) + "..";
                        }
                        else
                        {
                            tName = ds.Tables[0].Rows[i]["Name"].ToString();
                        }


                        string Target = "0";
                        string Cumulative = "0";
                        DataSet dsVal = getdt.GetTask_Target_Cumulative(new Guid(ds.Tables[0].Rows[i]["TaskUID"].ToString()));
                        if (dsVal.Tables[0].Rows.Count > 0)
                        {
                            Target = dsVal.Tables[0].Rows[0]["TargetValue"].ToString();
                            Cumulative = dsVal.Tables[0].Rows[0]["Culumative"].ToString();
                        }
                        strScript.Append("['" + tName + "', " + Target + ", " + Cumulative + "],");
                    }
                    strScript.Remove(strScript.Length - 1, 1);
                    strScript.Append("]);");
                    strScript.Append(@"var options = {
                            is3D: true,
                            legend: { position: 'none' },
                            fontSize: 13,
                            bar: { groupWidth:'50%' },
                            chartArea: {
                                left: '35%',
                                top: '10%',
                                height: '80%',
                                width: '60%'
                            },
                            height: 300,
                            bars: 'horizontal',
                            vAxis: { 
                             gridlines: { count: 10 } 
                              },
                            hAxis: {
                                minValue: 0,
                                gridlines: {count: 10}
                            }
                        };
                var chart = new google.visualization.BarChart(document.getElementById('chart_div'));
                chart.draw(data, options);
                }
                </script>");

                    ltScript_Progress.Text = strScript.ToString();
                }
                else
                {
                    ltScript_Progress.Text = "<h4>No data</h4>";
                }
                //DataSet ds = getdt.GetTask_Status_Percentage(Session["UserUID"].ToString(), ActivityType, Activity_ID);
                //if (ds.Tables[0].Rows.Count > 0)
                //{
                //    StringBuilder strScript = new StringBuilder();
                //    strScript.Append(@"<script type='text/javascript'>
                //    google.charts.load('current', { packages: ['corechart', 'bar'] });
                //    google.charts.setOnLoadCallback(drawBasic);

                //    function drawBasic() {
                //        var data = google.visualization.arrayToDataTable([
                //          ['Task', 'Completion(in %)', { role: 'style' }, { role: 'annotation' }],");
                //    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                //    {
                //        string name = ds.Tables[0].Rows[i]["Name"].ToString();
                //        if (name.Length > 10)
                //        {
                //            name= ds.Tables[0].Rows[i]["Name"].ToString().Substring(0, 10) + "..";
                //        }
                //        else
                //        {
                //            name = ds.Tables[0].Rows[i]["Name"].ToString();
                //        }
                //        if (ds.Tables[0].Rows[i]["StatusPer"].ToString() != "")
                //        {
                //            string colorCode = string.Empty;
                //            if (i == 0)
                //            {
                //                colorCode = "#3366CC";
                //            }
                //            else if (i == 1)
                //            {
                //                colorCode = "#990099";
                //            }
                //            else if (i == 2)
                //            {
                //                colorCode = "#109618";
                //            }
                //            else if (i == 3)
                //            {
                //                colorCode = "#DC3912";
                //            }
                //            else if (i == 4)
                //            {
                //                colorCode = "#DC3912";
                //            }
                //            else
                //            {
                //                colorCode = "#3366CC";
                //            }
                //            strScript.Append("['" + name + "', " + ds.Tables[0].Rows[i]["StatusPer"].ToString() + ", '" + colorCode + "', '" + ds.Tables[0].Rows[i]["StatusPer"].ToString() + "%'],");
                //        }
                //    }
                //    strScript.Remove(strScript.Length - 1, 1);
                //    strScript.Append("]);");
                //    strScript.Append(@"var options = {
                //            is3D: true,
                //            legend: { position: 'none' },
                //            fontSize: 13,
                //            bar: { groupWidth:'50%' },
                //            chartArea: {
                //                left: '25%',
                //                top: '10%',
                //                height: '80%',
                //                width: '50%'
                //            },
                //            height: 300,
                //            bars: 'horizontal',
                //            axes: {
                //                x: {
                //                    0: { side: 'top', label: 'Percentage' } // Top x-axis.
                //                }
                //            },
                //            hAxis: {
                //                minValue: 0,
                //                format: '#\'%\''
                //            }
                //        };

                //      function selectHandler() {
                //      var selectedItem = chart.getSelection()[0];
                //      if (selectedItem) {
                //        var topping = data.getValue(selectedItem.row, 0);");
                //    strScript.Append("window.open('WorkPackages.aspx?TaskName=' + topping + '&ProjectUID=" + DDlProject.SelectedValue + "&WorkPackageUID=" + DDLWorkPackage.SelectedValue + "', '_self', true);");
                //    //alert('The user selected ' + topping);
                //    strScript.Append(@"}
                //    }

                //        var chart = new google.visualization.BarChart(document.getElementById('chart_div'));

                //        chart.draw(data, options);
                //    }
                //</script>");
                //    ltScript_Progress.Text = strScript.ToString();
                //    //google.visualization.events.addListener(chart, 'select', selectHandler);
                //}
                //else
                //{
                //    ltScript_Progress.Text = "<h4>No data</h4>";
                //}
            }
        }

        //private void Bind_CostChart(string ActivityType, string Activity_ID)
        //{
        //    if (DDLWorkPackage.SelectedValue != "--Select--")
        //    {
        //        ltScript_Progress.Text = string.Empty;
        //        DataSet ds = getdt.Get_WorkPackage_Budget(Session["UserUID"].ToString(), ActivityType, Activity_ID);
        //        if (ds.Tables[0].Rows.Count > 0)
        //        {
        //            StringBuilder strScript = new StringBuilder();
        //            strScript.Append(@" <script type='text/javascript'>

        //        google.charts.load('current', { packages: ['corechart', 'bar'] });
        //        google.charts.setOnLoadCallback(drawBasic);

        //        function drawBasic() {

        //        var data = google.visualization.arrayToDataTable([
        //             ['Element', 'Cost', { role: 'style' }, { role: 'annotation' }],");
        //            string CurrencySymbol = "";
        //            if (ds.Tables[0].Rows[0]["Currency"].ToString() == "&#x20B9;")
        //            {
        //                CurrencySymbol = "₹";
        //            }
        //            else if (ds.Tables[0].Rows[0]["Currency"].ToString() == "&#36;")
        //            {
        //                CurrencySymbol = "$";
        //            }
        //            else
        //            {

        //                CurrencySymbol = "¥";
        //            }
        //            strScript.Append("['Actual', " + ds.Tables[0].Rows[0]["Actual"].ToString() + ", '#3366CC', '" + CurrencySymbol + ' ' + ds.Tables[0].Rows[0]["Actual"].ToString() + "'],['Planned', " + ds.Tables[0].Rows[0]["Planned"].ToString() + ", '#DC3912', '" + CurrencySymbol + ' ' + ds.Tables[0].Rows[0]["Planned"].ToString() + "'],['Budget', " + ds.Tables[0].Rows[0]["Budget"].ToString() + ", '#109618', '" + CurrencySymbol + ' ' + ds.Tables[0].Rows[0]["Budget"].ToString() + "']]);");
        //            strScript.Append(@"var options = {
        //            is3D: true,
        //            legend: { position: 'none' },
        //            fontSize: 14,
        //            chartArea: {
        //                left: '10%',
        //                top: '10%',
        //                height: '75%',
        //                width: '80%'
        //            },
        //            height: 300
        //        };

        //        var chart = new google.visualization.ColumnChart(
        //          document.getElementById('chart_div'));
        //         chart.draw(data, options);

        //    }</script>");
        //            //ltScript_Cost.Text = strScript.ToString();
        //            ltScript_Progress.Text = strScript.ToString();
        //        }
        //        else
        //        {
        //            //ltScript_Cost.Text = "<h3>No data</h3>";
        //            ltScript_Progress.Text = "<h3>No data</h3>";

        //        }
        //    }
        //}


        private void Bind_CostChart(string ActivityType, string Activity_ID)
        {
            if (DDLWorkPackage.SelectedValue != "--Select--")
            {
                ltScript_Progress.Text = string.Empty;
                // DataSet ds = getdt.Get_WorkPackage_Budget(Session["UserUID"].ToString(), ActivityType, Activity_ID);

                DataSet ds = getdt.GetCostGraphData(Activity_ID); // changed by saji augustin dated 13 may 2022

                if (ds.Tables[0].Rows.Count > 0)
                {
                    StringBuilder strScript = new StringBuilder();
                    strScript.Append(@" <script type='text/javascript'>
                
                google.charts.load('current', { packages: ['corechart', 'bar'] });
                google.charts.setOnLoadCallback(drawBasic);

                function drawBasic() {

                var data = google.visualization.arrayToDataTable([
                     ['Element', 'Cost', { role: 'style' }, { role: 'annotation' }],");
                    string CurrencySymbol = "";
                    if (ds.Tables[0].Rows[0]["Currency"].ToString() == "&#x20B9;")
                    {
                        CurrencySymbol = "₹";
                    }
                    else if (ds.Tables[0].Rows[0]["Currency"].ToString() == "&#36;")
                    {
                        CurrencySymbol = "$";
                    }
                    else
                    {

                        CurrencySymbol = "¥";
                    }
                    if (WebConfigurationManager.AppSettings["Domain"] != "Suez")
                    {
                        strScript.Append("['Actual', " + ds.Tables[0].Rows[0]["Actual"].ToString() + ", '#3366CC', '" + CurrencySymbol + ' ' + ds.Tables[0].Rows[0]["Actual"].ToString() + "'],['Planned', " + ds.Tables[0].Rows[0]["Planned"].ToString() + ", '#DC3912', '" + CurrencySymbol + ' ' + ds.Tables[0].Rows[0]["Planned"].ToString() + "'],['Budget', " + ds.Tables[0].Rows[0]["Budget"].ToString() + ", '#109618', '" + CurrencySymbol + ' ' + ds.Tables[0].Rows[0]["Budget"].ToString() + "']]);");
                    }
                    else
                    {
                        strScript.Append("['Planned', " + ds.Tables[0].Rows[0]["Planned"].ToString() + ", '#DC3912', '" + CurrencySymbol + ' ' + ds.Tables[0].Rows[0]["Planned"].ToString() + "'],['Actual', " + ds.Tables[0].Rows[0]["Actual"].ToString() + ", '#3366CC', '" + CurrencySymbol + ' ' + ds.Tables[0].Rows[0]["Actual"].ToString() + "']]);");

                    }
                    strScript.Append(@"var options = {
                    title : 'Cost in Crores of Rupees',
                    is3D: true, 
                    legend: { position: 'none' },
                    fontSize: 14,
                    chartArea: {
                        left: '10%',
                        top: '10%',
                        height: '75%',
                        width: '80%'
                    },
                    height: 300
                };

                var chart = new google.visualization.ColumnChart(
                  document.getElementById('chart_div'));
                 chart.draw(data, options);
                
            }</script>");
                    //ltScript_Cost.Text = strScript.ToString();
                    ltScript_Progress.Text = strScript.ToString();
                }
                else
                {
                    //ltScript_Cost.Text = "<h3>No data</h3>";
                    ltScript_Progress.Text = "<h3>No data</h3>";

                }
            }
        }

        private void Bind_ResourceChart(string ActivityType, string Activity_ID)
        {
            if (DDLWorkPackage.SelectedValue != "--Select--")
            {

                if (DDlResource.Items.Count > 0)
                {
                    DataSet ds = getdt.Get_WorkPackage_WorkLoad(Session["UserUID"].ToString(), ActivityType, Activity_ID, DDlResource.SelectedValue);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        StringBuilder strScript = new StringBuilder();
                        strScript.Append(@"<script type='text/javascript'>
                google.charts.load('current', { packages: ['corechart', 'bar'] });
                google.charts.setOnLoadCallback(drawStacked);

                function drawStacked() {
                    var data = google.visualization.arrayToDataTable([
                      ['WorkLoad', 'Used', 'Remaining' , { role: 'annotation' }],");
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            strScript.Append("['" + getdt.getTaskNameby_TaskUID(new Guid(ds.Tables[0].Rows[i]["TName"].ToString())) + "', " + ds.Tables[0].Rows[i]["Completed"].ToString() + ", " + ds.Tables[0].Rows[i]["Remaining"].ToString() + "," + ds.Tables[0].Rows[i]["AllocatedUnits"].ToString() + "],");
                        }
                        strScript.Remove(strScript.Length - 1, 1);
                        strScript.Append("]);");

                        strScript.Append(@"var options = {
                        legend:'none',
                        annotations: {alwaysOutside: true},
                        fontSize: 13,
                        height: 300,
                        chartArea: {
                            left: '20%',
                            top: '10%',
                            height: '75%',
                            width: '70%'
                        },
                        isStacked: true
                    };
                    var chart = new google.visualization.BarChart(document.getElementById('Resource_div'));
                    chart.draw(data, options);
                }
            </script>");
                        //LblTitle.Text = DDlResource.SelectedItem.Text;
                        ltScript_Resource.Text = strScript.ToString();
                    }
                    else
                    {
                        ltScript_Resource.Text = "<h4>No data</h4>";
                    }
                }
                else
                {
                    ltScript_Resource.Text = "<h4>No data</h4>";
                }
            }
        }

        protected void DDlResource_SelectedIndexChanged(object sender, EventArgs e)
        {
            Bind_ResourceChart("Work Package", DDLWorkPackage.SelectedValue);
        }

        protected void RdList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDLWorkPackage.SelectedValue != "")
            {
                if (RdList.SelectedValue == "Progress")
                {
                    Bind_ProgressChart("Work Package", DDLWorkPackage.SelectedValue);
                }
                else
                {
                    Bind_CostChart("Work Package", DDLWorkPackage.SelectedValue);
                }
            }
           
        }

        //        private void LoadGraph()
        //        {
        //            try
        //            {
        //                if (DDLWorkPackage.SelectedValue != "--Select--")
        //                {
        //                    ltScript_PhysicalProgress.Text = string.Empty;

        //                    DataSet ds = getdt.GetTaskScheduleDatesforGraph(new Guid(DDLWorkPackage.SelectedValue));
        //                    if (ds.Tables[0].Rows.Count > 0)
        //                    {
        //                        StringBuilder strScript = new StringBuilder();
        //                        string tablemonths = "<td>&nbsp;</td>";
        //                        string tmonthlyplan = "<td>Monthly Plan</td>";
        //                        string tmonthlyactual = "<td>Monthly Actual</td>";
        //                        string tcumulativeplan = "<td>Cumulative Plan</td>";
        //                        string tcumulativeactual = "<td>Cumulative Actual</td>";
        //                        strScript.Append(@" <script type='text/javascript'>

        //                google.charts.load('current', { packages: ['corechart', 'bar'] });
        //                google.charts.setOnLoadCallback(drawBasic);

        //                function drawBasic() {

        //                var data = google.visualization.arrayToDataTable([
        //          ['Month', 'Monthly Plan', 'Monthly Actual', 'Cumulative Plan', 'Cumulative Actual'],");
        //                        int count = 1;
        //                        DataSet dsvalues = new DataSet();
        //                        decimal planvalue = 0;
        //                        decimal actualvalue = 0;
        //                        decimal cumplanvalue = 0;
        //                        decimal cumactualvalue = 0;
        //                        foreach (DataRow dr in ds.Tables[0].Rows)
        //                        {
        //                            //get the actual and planned values....
        //                            dsvalues.Clear();
        //                            dsvalues = getdt.GetTaskScheduleValuesForGraph(new Guid(DDLWorkPackage.SelectedValue), Convert.ToDateTime(dr["StartDate"].ToString()), Convert.ToDateTime(dr["StartDate"].ToString()).AddMonths(1));
        //                            if (dsvalues.Tables[0].Rows.Count > 0)
        //                            {
        //                                planvalue = decimal.Parse(dsvalues.Tables[0].Rows[0]["TotalSchValue"].ToString());
        //                                actualvalue = decimal.Parse(dsvalues.Tables[0].Rows[0]["TotalAchValue"].ToString());
        //                                cumplanvalue += planvalue;
        //                                cumactualvalue += actualvalue;
        //                            }
        //                            if (count < ds.Tables[0].Rows.Count)
        //                            {

        //                                strScript.Append("['" + Convert.ToDateTime(dr["StartDate"].ToString()).ToString("MMMyy") + "'," + planvalue + "," + actualvalue + "," + cumplanvalue + "," + cumactualvalue + "],");
        //                            }
        //                            else
        //                            {
        //                                strScript.Append("['" + Convert.ToDateTime(dr["StartDate"].ToString()).ToString("MMMyy") + "'," + planvalue + "," + actualvalue + "," + cumplanvalue + "," + cumactualvalue + "]]);");
        //                            }
        //                            //
        //                            tablemonths += "<td>" + Convert.ToDateTime(dr["StartDate"].ToString()).ToString("MMM-yy") + "</td>";
        //                            tmonthlyplan += "<td>" + decimal.Round(planvalue, 2) + "</td>";
        //                            tmonthlyactual += "<td>" + decimal.Round(actualvalue, 2) + "</td>";

        //                            tcumulativeplan += "<td>" + decimal.Round(cumplanvalue, 2) + "</td>";
        //                            tcumulativeactual += "<td>" + decimal.Round(cumactualvalue, 2) + "</td>";


        //                            //
        //                            count++;
        //                        }

        //                        strScript.Append(@"var options = {
        //          title : 'Plan vs Achieved Progress Curve',

        //          hAxis: {title: 'MONTH',titleTextStyle: {
        //        bold:'true',
        //      }},
        //          seriesType: 'bars',
        //          series: {2: {type: 'line',targetAxisIndex: 1},3: {type: 'line',targetAxisIndex: 1}},
        //vAxes: {
        //            // Adds titles to each axis.

        //            0: {title: 'Monthly Plan (%)',titleTextStyle: {
        //        bold:'true',
        //      }},
        //            1: {title: 'Cumulative Plan (%)',titleTextStyle: {
        //        bold:'true',
        //      }}
        //          }
        //        };
        //                var chart = new google.visualization.ComboChart(
        //                  document.getElementById('chart_divProgress'));
        //                 chart.draw(data, options);

        //            }</script>");
        //                        //ltScript_Cost.Text = strScript.ToString();
        //                        ltScript_PhysicalProgress.Text = strScript.ToString();
        //                        divtable.InnerHtml = "<table border=\"1\" style=\"text-align:center;font-size:11px;padding-left:0px;\">" +
        //                                          "<tr> " + tablemonths + "</tr>" +
        //                                           "<tr> " + tmonthlyplan + "</tr>" +
        //                                            "<tr> " + tmonthlyactual + "</tr>" +
        //                                             "<tr> " + tcumulativeplan + "</tr>" +
        //                                              "<tr> " + tcumulativeactual + "</tr>" +
        //                                                  "</table>";
        //                        btnPrint.Visible = true;
        //                    }
        //                    else
        //                    {
        //                        ltScript_PhysicalProgress.Text = "<h3>No data</h3>";
        //                        divtable.InnerHtml = "";
        //                        btnPrint.Visible = false;
        //                    }
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                throw ex;
        //            }
        //        }


        //        private void LoadGraph()
        //        {
        //            try
        //            {
        //                if (DDLWorkPackage.SelectedValue != "--Select--")
        //                {
        //                    ltScript_PhysicalProgress.Text = string.Empty;

        //                    DataSet ds = getdt.GetTaskScheduleDatesforGraph(new Guid(DDLWorkPackage.SelectedValue));
        //                    if (ds.Tables[0].Rows.Count > 0)
        //                    {
        //                        StringBuilder strScript = new StringBuilder();
        //                        string tablemonths = "<td>&nbsp;</td>";
        //                        string tmonthlyplan = "<td>Monthly Plan</td>";
        //                        string tmonthlyactual = "<td>Monthly Actual</td>";
        //                        string tcumulativeplan = "<td>Cumulative Plan</td>";
        //                        string tcumulativeactual = "<td>Cumulative Actual</td>";
        //                        strScript.Append(@" <script type='text/javascript'>

        //                google.charts.load('current', { packages: ['corechart', 'bar'] });
        //                google.charts.setOnLoadCallback(drawBasic);

        //                function drawBasic() {

        //                var data = google.visualization.arrayToDataTable([
        //          ['Month', 'Monthly Plan', 'Monthly Actual', 'Cumulative Plan', 'Cumulative Actual'],");
        //                        int count = 1;
        //                        DataSet dsvalues1 = new DataSet();
        //                        DataSet dsvalues2 = new DataSet();

        //                        decimal planvalue = 0;
        //                        decimal actualvalue = 0;
        //                        decimal cumplanvalue = 0;
        //                        decimal cumactualvalue = 0;
        //                        foreach (DataRow dr in ds.Tables[0].Rows)
        //                        {
        //                            //get the actual and planned values....
        //                            dsvalues1.Clear();
        //                            dsvalues2.Clear();
        //                            dsvalues1 = getdt.GetTaskScheduleValuesForGraph(new Guid(DDLWorkPackage.SelectedValue), Convert.ToDateTime(dr["StartDate"].ToString()), Convert.ToDateTime(dr["StartDate"].ToString()).AddMonths(1));
        //                            if (dsvalues1.Tables[0].Rows.Count > 0)
        //                            {
        //                                planvalue = decimal.Parse(dsvalues1.Tables[0].Rows[0]["TotalSchValue"].ToString());
        //                                cumplanvalue += planvalue;
        //                            }

        //                            dsvalues2 = getdt.GetTaskActualValuesForGraph(new Guid(DDLWorkPackage.SelectedValue), Convert.ToDateTime(dr["StartDate"].ToString()), Convert.ToDateTime(dr["StartDate"].ToString()).AddMonths(1));
        //                            if (dsvalues2.Tables[0].Rows.Count > 0)
        //                            {
        //                                actualvalue = decimal.Parse(dsvalues2.Tables[0].Rows[0]["TotalAchValue"].ToString());
        //                                cumactualvalue += actualvalue;
        //                            }

        //                            if (count < ds.Tables[0].Rows.Count)
        //                            {
        //                                strScript.Append("['" + Convert.ToDateTime(dr["StartDate"].ToString()).ToString("MMMyy") + "'," + planvalue + "," + actualvalue + "," + cumplanvalue + "," + cumactualvalue + "],");
        //                            }
        //                            else
        //                            {
        //                                strScript.Append("['" + Convert.ToDateTime(dr["StartDate"].ToString()).ToString("MMMyy") + "'," + planvalue + "," + actualvalue + "," + cumplanvalue + "," + cumactualvalue + "]]);");
        //                            }
        //                            //
        //                            tablemonths += "<td>" + Convert.ToDateTime(dr["StartDate"].ToString()).ToString("MMM-yy") + "</td>";
        //                            tmonthlyplan += "<td>" + decimal.Round(planvalue, 2) + "</td>";
        //                            tmonthlyactual += "<td>" + decimal.Round(actualvalue, 2) + "</td>";

        //                            tcumulativeplan += "<td>" + decimal.Round(cumplanvalue, 2) + "</td>";
        //                            tcumulativeactual += "<td>" + decimal.Round(cumactualvalue, 2) + "</td>";


        //                            //
        //                            count++;
        //                        }

        //                        strScript.Append(@"var options = {
        //          title : 'Plan vs Achieved Progress Curve',

        //          hAxis: {title: 'MONTH',titleTextStyle: {
        //        bold:'true',
        //      }},
        //          seriesType: 'bars',
        //          series: {2: {type: 'line',targetAxisIndex: 1},3: {type: 'line',targetAxisIndex: 1}},
        //vAxes: {
        //            // Adds titles to each axis.

        //            0: {title: 'Monthly Plan (%)',titleTextStyle: {
        //        bold:'true',
        //      }},
        //            1: {title: 'Cumulative Plan (%)',titleTextStyle: {
        //        bold:'true',
        //      }}
        //          }
        //        };
        //                var chart = new google.visualization.ComboChart(
        //                  document.getElementById('chart_divProgress'));
        //                 chart.draw(data, options);

        //            }</script>");
        //                        //ltScript_Cost.Text = strScript.ToString();
        //                        ltScript_PhysicalProgress.Text = strScript.ToString();
        //                        divtable.InnerHtml = "<table border=\"1\" style=\"text-align:center;font-size:11px;padding-left:0px;\">" +
        //                                          "<tr> " + tablemonths + "</tr>" +
        //                                           "<tr> " + tmonthlyplan + "</tr>" +
        //                                            "<tr> " + tmonthlyactual + "</tr>" +
        //                                             "<tr> " + tcumulativeplan + "</tr>" +
        //                                              "<tr> " + tcumulativeactual + "</tr>" +
        //                                                  "</table>";
        //                        btnPrint.Visible = true;
        //                    }
        //                    else
        //                    {
        //                        ltScript_PhysicalProgress.Text = "<h3>No data</h3>";
        //                        divtable.InnerHtml = "";
        //                        btnPrint.Visible = false;
        //                    }
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                throw ex;
        //            }
        //        }




        //added on 24/12/2022 for saji
        //        private void LoadGraph()
        //        {
        //            try
        //            {
        //                //  DateTime t1 = DateTime.Now;

        //                if (DDLWorkPackage.SelectedValue != "--Select--")
        //                {
        //                    ltScript_PhysicalProgress.Text = string.Empty;

        //                    DataSet ds = getdt.GetGraphPhysicalProgressValues1(DDlProject.SelectedValue, DDLWorkPackage.SelectedValue);

        //                    if (ds.Tables[0].Rows.Count > 0)
        //                    {
        //                        StringBuilder strScript = new StringBuilder();
        //                        string tablemonths = "<td>&nbsp;</td>";
        //                        string tmonthlyplan = "<td>Monthly Plan</td>";
        //                        string tmonthlyactual = "<td>Monthly Actual</td>";
        //                        string tmonthlytest = "<td>Monthly Revised Plan</td>";
        //                        string tcumulativeplan = "<td>Cumulative Plan</td>";
        //                        string tcumulativeactual = "<td>Cumulative Actual</td>";
        //                        string tcumulativetest = "<td>Cumulative Revised Plan</td>";
        //                        strScript.Append(@" <script type='text/javascript'>

        //                google.charts.load('current', { packages: ['corechart', 'bar'] });
        //                google.charts.setOnLoadCallback(drawBasic);

        //                function drawBasic() {

        //                var data = google.visualization.arrayToDataTable([
        //          ['Month', 'Monthly Plan', 'Monthly Actual','Monthly Revised Plan', 'Cumulative Plan', 'Cumulative Actual','Cumulative Revised Plan'],");
        //                        int count = 1;

        //                        decimal planvalue = 0;
        //                        decimal actualvalue = 0;
        //                        decimal testvalue = 0;
        //                        decimal cumplanvalue = 0;
        //                        decimal cumactualvalue = 0;
        //                        decimal cumtestvalue = 0;

        //                        string[] graphValues = ds.Tables[0].Rows[0].ItemArray[0].ToString().Split(';');


        //                        foreach (string dr in graphValues)
        //                        {
        //                            //get the actual and planned values....

        //                            string[] fields = dr.Split(',');

        //                            if (fields.Length == 4)
        //                            {
        //                                planvalue = decimal.Parse(fields[1]);
        //                                cumplanvalue += planvalue;

        //                                actualvalue = decimal.Parse(fields[2]);
        //                                cumactualvalue += actualvalue;

        //                                testvalue = decimal.Parse(fields[3]);
        //                                cumtestvalue += testvalue;

        //                                if (count < graphValues.Length)
        //                                {
        //                                    strScript.Append("['" + Convert.ToDateTime(fields[0]).ToString("MMMyy") + "'," + planvalue + "," + actualvalue + "," + testvalue + "," + cumplanvalue + "," + cumactualvalue + "," + cumtestvalue + "],");
        //                                }
        //                                else
        //                                {
        //                                    strScript.Append("['" + Convert.ToDateTime(fields[0]).ToString("MMMyy") + "'," + planvalue + "," + actualvalue + "," + testvalue + "," + cumplanvalue + "," + cumactualvalue + "," + cumtestvalue + "]]);");
        //                                }


        //                                tablemonths += "<td>" + Convert.ToDateTime(fields[0]).ToString("MMM-yy") + "</td>";
        //                                tmonthlyplan += "<td>" + decimal.Round(planvalue, 2) + "</td>";
        //                                tmonthlyactual += "<td>" + decimal.Round(actualvalue, 2) + "</td>";
        //                                tmonthlytest += "<td>" + decimal.Round(testvalue, 2) + "</td>";

        //                                tcumulativeplan += "<td>" + decimal.Round(cumplanvalue, 2) + "</td>";
        //                                tcumulativeactual += "<td>" + decimal.Round(cumactualvalue, 2) + "</td>";
        //                                tcumulativetest += "<td>" + decimal.Round(cumtestvalue, 2) + "</td>";
        //                            }
        //                            count++;
        //                        }

        //                        strScript.Append(@"var options = {
        //          title : 'Plan vs Achieved Progress Curve',

        //          hAxis: {title: 'MONTH',titleTextStyle: {
        //        bold:'true',
        //      }},
        //          seriesType: 'bars',
        //          series: {3: {type: 'line',targetAxisIndex: 1},4: {type: 'line',targetAxisIndex: 1},5: {type: 'line',targetAxisIndex: 1}},
        //vAxes: {
        //            // Adds titles to each axis.

        //            0: {title: 'Monthly Plan (%)',titleTextStyle: {
        //        bold:'true',
        //      }},
        //            1: {title: 'Cumulative Plan (%)',titleTextStyle: {
        //        bold:'true',
        //      }}
        //          }
        //        };
        //                var chart = new google.visualization.ComboChart(
        //                  document.getElementById('chart_divProgress'));
        //                 chart.draw(data, options);

        //            }</script>");
        //                        //ltScript_Cost.Text = strScript.ToString();
        //                        ltScript_PhysicalProgress.Text = strScript.ToString();
        //                        divtable.InnerHtml = "<table border=\"1\" style=\"text-align:center;font-size:11px;padding-left:0px;\">" +
        //                                          "<tr> " + tablemonths + "</tr>" +
        //                                           "<tr> " + tmonthlyplan + "</tr>" +
        //                                            "<tr> " + tmonthlyactual + "</tr>" +
        //                                             "<tr> " + tmonthlytest + "</tr>" +
        //                                             "<tr> " + tcumulativeplan + "</tr>" +
        //                                              "<tr> " + tcumulativeactual + "</tr>" +
        //                                              "<tr> " + tcumulativetest + "</tr>" +
        //                                                  "</table>";
        //                        btnPrint.Visible = true;
        //                    }
        //                    else
        //                    {
        //                        ltScript_PhysicalProgress.Text = "<h3>No data</h3>";
        //                        divtable.InnerHtml = "";
        //                        btnPrint.Visible = false;
        //                    }
        //                }

        //                //  DateTime t2 = DateTime.Now;
        //                //  TimeSpan t3 = t2.Subtract(t1);

        //                //  WriteNewTimeTaken("Physical Progress Chart",DDlProject.SelectedItem.Text,t3);
        //            }
        //            catch (Exception ex)
        //            {
        //                throw ex;
        //            }
        //        }


        //added on 02/01/2023
        private void LoadGraph()
        {
            try
            {
                //  DateTime t1 = DateTime.Now;

                if (DDLWorkPackage.SelectedValue != "--Select--")
                {
                    ltScript_PhysicalProgress.Text = string.Empty;

                    DataSet ds = getdt.GetGraphPhysicalProgressValues1(DDlProject.SelectedValue, DDLWorkPackage.SelectedValue);

                    Boolean IsRevisedPlan = getdt.IsRevisedPlan(new Guid(DDLWorkPackage.SelectedValue)) == "Y" ? true : false;

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        StringBuilder strScript = new StringBuilder();

                        string tablemonths = "";
                        string tmonthlyplan = "";
                        string tmonthlyactual = "";
                        string tmonthlytest = "";
                        string tcumulativeplan = "";
                        string tcumulativeactual = "";
                        string tcumulativetest = "";

                        tablemonths = "<td>&nbsp;</td>";
                        tmonthlyplan = "<td>Monthly Plan</td>";
                        tmonthlyactual = "<td>Monthly Actual</td>";

                        if (IsRevisedPlan)
                            tmonthlytest = "<td>Monthly Revised Plan</td>";
                        tcumulativeplan = "<td>Cumulative Plan</td>";

                        tcumulativeactual = "<td>Cumulative Actual</td>";

                        if (IsRevisedPlan)
                            tcumulativetest = "<td>Cumulative Revised Plan</td>";

                        if (IsRevisedPlan)
                        {
                            strScript.Append(@" <script type='text/javascript'>
                
                google.charts.load('current', { packages: ['corechart', 'bar'] });
                google.charts.setOnLoadCallback(drawBasic);

                function drawBasic() {

                var data = google.visualization.arrayToDataTable([
          ['Month', 'Monthly Plan', 'Monthly Actual','Monthly Revised Plan', 'Cumulative Plan', 'Cumulative Actual','Cumulative Revised Plan'],");
                        }
                        else
                        {
                            strScript.Append(@" <script type='text/javascript'>
                
                google.charts.load('current', { packages: ['corechart', 'bar'] });
                google.charts.setOnLoadCallback(drawBasic);

                function drawBasic() {

                var data = google.visualization.arrayToDataTable([
          ['Month', 'Monthly Plan', 'Monthly Actual','Cumulative Plan', 'Cumulative Actual'],");
                        }


                        int count = 1;

                        decimal planvalue = 0;
                        decimal actualvalue = 0;
                        decimal testvalue = 0;
                        decimal cumplanvalue = 0;
                        decimal cumactualvalue = 0;
                        decimal cumtestvalue = 0;

                        string[] graphValues = ds.Tables[0].Rows[0].ItemArray[0].ToString().Split(';');


                        foreach (string dr in graphValues)
                        {
                            //get the actual and planned values....

                            string[] fields = dr.Split(',');

                            if (fields.Length == 4)
                            {
                                planvalue = decimal.Parse(fields[1]);
                                cumplanvalue += planvalue;

                                actualvalue = decimal.Parse(fields[2]);
                                cumactualvalue += actualvalue;

                                testvalue = decimal.Parse(fields[3]);
                                cumtestvalue += testvalue;

                                if (count < graphValues.Length)
                                {
                                    if (IsRevisedPlan)
                                        strScript.Append("['" + Convert.ToDateTime(fields[0]).ToString("MMMyy") + "'," + planvalue + "," + actualvalue + "," + testvalue + "," + cumplanvalue + "," + cumactualvalue + "," + cumtestvalue + "],");
                                    else
                                        strScript.Append("['" + Convert.ToDateTime(fields[0]).ToString("MMMyy") + "'," + planvalue + "," + actualvalue + "," + cumplanvalue + "," + cumactualvalue + "],");
                                }
                                else
                                {
                                    if (IsRevisedPlan)
                                        strScript.Append("['" + Convert.ToDateTime(fields[0]).ToString("MMMyy") + "'," + planvalue + "," + actualvalue + "," + testvalue + "," + cumplanvalue + "," + cumactualvalue + "," + cumtestvalue + "]]);");
                                    else
                                        strScript.Append("['" + Convert.ToDateTime(fields[0]).ToString("MMMyy") + "'," + planvalue + "," + actualvalue + "," + cumplanvalue + "," + cumactualvalue + "]]);");
                                }


                                tablemonths += "<td>" + Convert.ToDateTime(fields[0]).ToString("MMM-yy") + "</td>";
                                tmonthlyplan += "<td>" + decimal.Round(planvalue, 2) + "</td>";
                                tmonthlyactual += "<td>" + decimal.Round(actualvalue, 2) + "</td>";

                                if (IsRevisedPlan)
                                    tmonthlytest += "<td>" + decimal.Round(testvalue, 2) + "</td>";

                                tcumulativeplan += "<td>" + decimal.Round(cumplanvalue, 2) + "</td>";
                                tcumulativeactual += "<td>" + decimal.Round(cumactualvalue, 2) + "</td>";

                                if (IsRevisedPlan)
                                    tcumulativetest += "<td>" + decimal.Round(cumtestvalue, 2) + "</td>";
                            }
                            count++;
                        }

                        if (IsRevisedPlan)
                        {
                            strScript.Append(@"var options = {
          title : 'Plan vs Achieved Progress Curve',
          
          hAxis: {title: 'MONTH',titleTextStyle: {
        bold:'true',
      }},
          seriesType: 'bars',
          series: {3: {type: 'line',targetAxisIndex: 1},4: {type: 'line',targetAxisIndex: 1},5: {type: 'line',targetAxisIndex: 1}},
vAxes: {
            // Adds titles to each axis.
          
            0: {title: 'Monthly Plan (%)',titleTextStyle: {
        bold:'true',
      }},
            1: {title: 'Cumulative Plan (%)',titleTextStyle: {
        bold:'true',
      }}
          }
        };
                var chart = new google.visualization.ComboChart(
                  document.getElementById('chart_divProgress'));
                 chart.draw(data, options);
                
            }</script>");
                        }
                        else
                        {
                            strScript.Append(@"var options = {
          title : 'Plan vs Achieved Progress Curve',
          
          hAxis: {title: 'MONTH',titleTextStyle: {
        bold:'true',
      }},
          seriesType: 'bars',
          series: {2: {type: 'line',targetAxisIndex: 1},3: {type: 'line',targetAxisIndex: 1},4: {type: 'line',targetAxisIndex: 1}},
vAxes: {
            // Adds titles to each axis.
          
            0: {title: 'Monthly Plan (%)',titleTextStyle: {
        bold:'true',
      }},
            1: {title: 'Cumulative Plan (%)',titleTextStyle: {
        bold:'true',
      }}
          }
        };
                var chart = new google.visualization.ComboChart(
                  document.getElementById('chart_divProgress'));
                 chart.draw(data, options);
                
            }</script>");
                        }



                        //ltScript_Cost.Text = strScript.ToString();
                        ltScript_PhysicalProgress.Text = strScript.ToString();

                        if (IsRevisedPlan)
                        {
                            divtable.InnerHtml = "<table border=\"1\" style=\"text-align:center;font-size:11px;padding-left:0px;\">" +
                                                                      "<tr> " + tablemonths + "</tr>" +
                                                                       "<tr> " + tmonthlyplan + "</tr>" +
                                                                        "<tr> " + tmonthlyactual + "</tr>" +
                                                                        "<tr> " + tmonthlytest + "</tr>" +
                                                                         "<tr> " + tcumulativeplan + "</tr>" +
                                                                          "<tr> " + tcumulativeactual + "</tr>" +
                                                                          "<tr> " + tcumulativetest + "</tr>" +
                                                                              "</table>";
                        }
                        else
                        {
                            divtable.InnerHtml = "<table border=\"1\" style=\"text-align:center;font-size:11px;padding-left:0px;\">" +
                                                                     "<tr> " + tablemonths + "</tr>" +
                                                                      "<tr> " + tmonthlyplan + "</tr>" +
                                                                      "<tr> " + tmonthlyactual + "</tr>" +
                                                                      "<tr> " + tcumulativeplan + "</tr>" +
                                                                      "<tr> " + tcumulativeactual + "</tr>" +
                                                                      "</table>";
                        }

                        btnPrint.Visible = true;
                    }
                    else
                    {
                        ltScript_PhysicalProgress.Text = "<h3>No data</h3>";
                        divtable.InnerHtml = "";
                        btnPrint.Visible = false;
                    }
                }

                //  DateTime t2 = DateTime.Now;
                //  TimeSpan t3 = t2.Subtract(t1);

                //  WriteNewTimeTaken("Physical Progress Chart",DDlProject.SelectedItem.Text,t3);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void rdSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rdSelect.SelectedIndex == 0)
            {
                divProgresschart.Visible = false;
                divFinProgressChart.Visible = false;
                divNJSEIMIS.Visible = false;
                divdashboardimage.Visible = true;
                divMainblocks.Visible = true;
            }
            else if (rdSelect.SelectedIndex == 1)
            {
                divProgresschart.Visible = true;
                divFinProgressChart.Visible = false;
                divNJSEIMIS.Visible = false;
                divdashboardimage.Visible = true;
                divMainblocks.Visible = true;
                LoadGraph();
            }
            else if (rdSelect.SelectedIndex == 2)
            {
                divProgresschart.Visible = false;
                divFinProgressChart.Visible = true;
                divNJSEIMIS.Visible = false;
                divdashboardimage.Visible = true;
                divMainblocks.Visible = true;
                LoadFinancialGraph();
            }
            //else if (rdSelect.SelectedIndex == 3)
            //{
            //    divProgresschart.Visible = false;
            //    divFinProgressChart.Visible = false;
            //    divNJSEIMIS.Visible = true;
            //    divdashboardimage.Visible = true;
            //    divMainblocks.Visible = false;
            //}
        }
        protected void RBLPhotographs_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void LoadFinancialGraph()
        {
            try
            {
                if (DDLWorkPackage.SelectedValue != "--Select--")
                {
                    ltScript_FinProgress.Text = string.Empty;

                    DataSet ds = getdt.GetFinancialScheduleDatesforGraph(new Guid(DDLWorkPackage.SelectedValue));
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        StringBuilder strScript = new StringBuilder();
                        string tablemonths = "<td style=\"width:200px\">&nbsp;</td>";
                        string tmonthlyplan = "<td style=\"padding:3px\">Monthly Plan</td>";
                        string tmonthlyactual = "<td style=\"padding:3px\">Monthly Actual</td>";
                        string tcumulativeplan = "<td style=\"padding:3px\">Cumulative Plan</td>";
                        string tcumulativeactual = "<td style=\"padding:3px\">Cumulative Actual</td>";
                        strScript.Append(@" <script type='text/javascript'>
                
                google.charts.load('current', { packages: ['corechart', 'bar'] });
                google.charts.setOnLoadCallback(drawBasic);

                function drawBasic() {

                var data = google.visualization.arrayToDataTable([
          ['Month', 'Monthly Plan', 'Monthly Actual', 'Cumulative Plan', 'Cumulative Actual'],");
                        int count = 1;
                        DataSet dsvalues = new DataSet();
                        decimal planvalue = 0;
                        decimal actualvalue = 0;
                        decimal cumplanvalue = 0;
                        decimal cumactualvalue = 0;
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            //get the actual and planned values....
                            //dsvalues.Clear();
                            //dsvalues = getdt.GetTaskScheduleValuesForGraph(new Guid(DDLWorkPackage.SelectedValue), Convert.ToDateTime(dr["StartDate"].ToString()), Convert.ToDateTime(dr["StartDate"].ToString()).AddMonths(1));
                            //if (dsvalues.Tables[0].Rows.Count > 0)
                            //{
                            //    planvalue = decimal.Parse(dsvalues.Tables[0].Rows[0]["TotalSchValue"].ToString());
                            //    actualvalue = decimal.Parse(dsvalues.Tables[0].Rows[0]["TotalAchValue"].ToString());
                            //    cumplanvalue += planvalue;
                            //    cumactualvalue += actualvalue;
                            //}
                            dsvalues = getdt.GetFinMonthsPaymentTotal(new Guid(dr["FinMileStoneMonthUID"].ToString()));
                            planvalue = decimal.Parse(dr["AllowedPayment"].ToString());
                            actualvalue = 0;
                            if (dsvalues.Tables[0].Rows.Count > 0)
                            {
                                if (dsvalues.Tables[0].Rows[0]["TotalAmnt"].ToString() != "&nbsp;" && dsvalues.Tables[0].Rows[0]["TotalAmnt"].ToString() != "")
                                {
                                    // e.Row.Cells[2].Text = (decimal.Parse(ds.Tables[0].Rows[0]["TotalAmnt"].ToString()) / 10000000).ToString("n2");
                                    actualvalue = (decimal.Parse(dsvalues.Tables[0].Rows[0]["TotalAmnt"].ToString()) / 10000000);
                                }
                            }
                            // comment this code..used only for demo since actual values are not available....1
                            //Random random = new Random();
                            //if (planvalue > 0)
                            //{
                            //    System.Threading.Thread.Sleep(1000);
                            //    actualvalue = planvalue - random.Next(2,5);
                            //}

                            //
                            cumplanvalue += planvalue;
                            cumactualvalue += actualvalue;
                            if (count < ds.Tables[0].Rows.Count)
                            {

                                strScript.Append("['" + dr["MonthYear"].ToString() + "'," + planvalue + "," + actualvalue + "," + cumplanvalue + "," + cumactualvalue + "],");
                            }
                            else
                            {
                                strScript.Append("['" + dr["MonthYear"].ToString() + "'," + planvalue + "," + actualvalue + "," + cumplanvalue + "," + cumactualvalue + "]]);");
                            }
                            //
                            tablemonths += "<td style=\"padding:3px\">" + dr["MonthYear"].ToString() + "</td>";
                            tmonthlyplan += "<td style=\"padding:3px\">" + decimal.Round(planvalue, 2) + "</td>";
                            tmonthlyactual += "<td style=\"padding:3px\">" + decimal.Round(actualvalue, 2) + "</td>";

                            tcumulativeplan += "<td style=\"padding:3px\">" + decimal.Round(cumplanvalue, 2) + "</td>";
                            tcumulativeactual += "<td style=\"padding:3px\">" + decimal.Round(cumactualvalue, 2) + "</td>";


                            //
                            count++;
                        }

                        strScript.Append(@"var options = {
          title : 'Plan vs Achieved Progress Curve',
          
          hAxis: {title: 'MONTH',titleTextStyle: {
        bold:'true',
      }},
          seriesType: 'bars',
          series: {2: {type: 'line',targetAxisIndex: 1},3: {type: 'line',targetAxisIndex: 1}},
vAxes: {
            // Adds titles to each axis.
          
            0: {title: 'Monthly Plan (Crores.)',titleTextStyle: {
        bold:'true',
      }},
            1: {title: 'Cumulative Plan (Crores.)',titleTextStyle: {
        bold:'true',
      }}
          }
        };
                var chart = new google.visualization.ComboChart(
                  document.getElementById('chart_divProgressFin'));
                 chart.draw(data, options);
                
            }</script>");
                        //ltScript_Cost.Text = strScript.ToString();
                        ltScript_FinProgress.Text = strScript.ToString();
                        divtableFin.InnerHtml = "<table border=\"1\" style=\"text-align:center;font-size:11px;padding-left:10px;\">" +
                                          "<tr> " + tablemonths + "</tr>" +
                                           "<tr> " + tmonthlyplan + "</tr>" +
                                            "<tr> " + tmonthlyactual + "</tr>" +
                                             "<tr> " + tcumulativeplan + "</tr>" +
                                              "<tr> " + tcumulativeactual + "</tr>" +
                                                  "</table>";
                        btnPrint.Visible = true;
                    }
                    else
                    {
                        ltScript_FinProgress.Text = "<h3>No data</h3>";
                        divtableFin.InnerHtml = "";
                        btnPrint.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void RadioButtonList2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(RadioButtonList2.SelectedIndex == 0)
            {
                divdummyblock1.Visible = true;
                divdummyblock1_1.Visible = false;
            }
            else
            {
                divdummyblock1_1.Visible = true;
                divdummyblock1.Visible = false;
            }
        }

        private static int getUserDocsNo()// this is for getting no of docs user has to act on
        {
            DBGetData dbget = new DBGetData();
            int docscount = 0;
            string[] selectedValue = HttpContext.Current.Session["Project_Workpackage"].ToString().Split('_');

            DataSet ds = dbget.GetNextUserDocuments(new Guid(selectedValue[0]), new Guid(selectedValue[1]));
            DataSet dsNxtUser = new DataSet();
            HttpContext.Current.Session["docuids"] = string.Empty;
           // bool backtouser = false;
            foreach (DataRow drnext in ds.Tables[0].Rows)
            {
                // DataSet dsTop = dbget.getTop1_DocumentStatusSelect(new Guid(drnext["ActualDocumentUID"].ToString()));ActualDocuments.ActualDocument_CurrentStatus
                // dsTop.Tables[0].Rows[0]["ActivityType"].ToString()
                DataSet dsNext = dbget.GetNextStep_By_DocumentUID(new Guid(drnext["ActualDocumentUID"].ToString()), drnext["ActualDocument_CurrentStatus"].ToString());

                foreach (DataRow dr in dsNext.Tables[0].Rows)
                {
                    dsNxtUser = new DataSet();
                    dsNxtUser = dbget.GetNextUser_By_DocumentUID(new Guid(drnext["ActualDocumentUID"].ToString()), int.Parse(dr["ForFlow_Step"].ToString()));
                    if (dsNxtUser.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow druser in dsNxtUser.Tables[0].Rows)
                        {
                            if (HttpContext.Current.Session["UserUID"].ToString().ToUpper() == druser["Approver"].ToString().ToUpper())
                            {
                                if (drnext["ActualDocument_CurrentStatus"].ToString() == "Accepted")
                                {
                                    if (dbget.checkUserAddedDocumentstatus(new Guid(drnext["ActualDocumentUID"].ToString()), new Guid(HttpContext.Current.Session["UserUID"].ToString()), drnext["ActualDocument_CurrentStatus"].ToString()) == 0)
                                    {
                                        docscount = docscount + 1;
                                        //
                                        HttpContext.Current.Session["docuids"] += drnext["ActualDocumentUID"].ToString() + ",";
                                    }
                                }
                                else if (drnext["ActualDocument_CurrentStatus"].ToString().ToLower().Contains("back to pmc"))
                                {
                                    DataSet dsbackUser = dbget.GetBacktoPMCUsers(new Guid(drnext["ActualDocumentUID"].ToString()));
                                    foreach (DataRow druserb in dsbackUser.Tables[0].Rows)
                                    {
                                        if (druserb["PMCUser"].ToString().ToUpper() == HttpContext.Current.Session["UserUID"].ToString().ToUpper())
                                        {
                                            if (dbget.checkUserAddedDocumentstatus(new Guid(drnext["ActualDocumentUID"].ToString()), new Guid(HttpContext.Current.Session["UserUID"].ToString()), drnext["ActualDocument_CurrentStatus"].ToString()) == 0)
                                            {
                                                docscount = docscount + 1;
                                                //
                                                HttpContext.Current.Session["docuids"] += drnext["ActualDocumentUID"].ToString() + ",";
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    docscount = docscount + 1;
                                    //
                                    HttpContext.Current.Session["docuids"] += drnext["ActualDocumentUID"].ToString() + ",";
                                }
                                goto afterloop;
                            }
                            else
                            {


                            }
                        }
                    }

                }

            afterloop:
                Console.WriteLine("/Done");
            }
            return docscount;
        }

        [WebMethod(EnableSession = true)]
        public static string  GetDetails(string Id)
        {
            if (WebConfigurationManager.AppSettings["Domain"] != "Suez")
            {
                if (HttpContext.Current.Session["TypeOfUser"].ToString() != "U" && HttpContext.Current.Session["TypeOfUser"].ToString() != "NJSD")
                {
                    string[] selectedValue = HttpContext.Current.Session["Project_Workpackage"].ToString().Split('_');
                    return getUserDocsNo().ToString() + "$" + selectedValue[1];
                }
                else
                {
                  
                    string[] selectedValue = HttpContext.Current.Session["Project_Workpackage"].ToString().Split('_');
                    return "0$" + selectedValue[1];
                }
            }
            else
            {
                string[] selectedValue = HttpContext.Current.Session["Project_Workpackage"].ToString().Split('_');
                return "0$" + selectedValue[1];
            }
        }

        protected void DDLCamera_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDLCamera.SelectedValue != "--Select--")
            {
                DataSet dscamera = getdt.GetCameraDetails(new Guid(DDLCamera.SelectedValue));
                if(dscamera.Tables[0].Rows.Count > 0)
                {
                    //iframe set up
                    string url = dscamera.Tables[0].Rows[0]["Camera_IPAddress"].ToString();
                    ContentIframe.Attributes["src"] = url;
                    StringBuilder sb = new StringBuilder();
                    sb.Append("<script type = 'text/javascript'>");
                    sb.Append("window.open('");
                    sb.Append(url);
                    sb.Append("');");
                    sb.Append("</script>");
                    ClientScript.RegisterStartupScript(this.GetType(),
                            "script", sb.ToString());
                }
            }
            else
            {
                ContentIframe.Attributes["src"] = "";
            }
        }

        //added on 27/01/2023
        private void getTotalConcrete()
        {
            
            if (DDlProject.SelectedValue != "")
            {
                DataTable dtTasks = getdt.getTaskFormupdateData(new Guid(DDlProject.SelectedValue.ToString()));
                if (dtTasks.Rows.Count > 0)
                {
                    dtTasks.Columns.Add("SNo");
                    dtTasks.Columns[2].SetOrdinal(0);
                    //  DateTime dtStartDate = Convert.ToDateTime(ddlMonth.SelectedValue.ToString());
                    // DateTime selectedDate = Convert.ToDateTime( ddlMonth.SelectedValue.ToString());

                  

                    dtTasks.Columns.Add("Unit");
                    dtTasks.Columns.Add("Total Scope");
                    dtTasks.Columns.Add("Achieved till " + DateTime.Now.ToString("MMM dd yyyy"));
                    dtTasks.Columns.Add("Balance");

                    double columnTotal = 0;
                    double columnTotalPerDay = 0;
                    DataTable dstasks = new DataTable();
                    DataSet dsMsr = new DataSet();
                    decimal totalachieved = 0;
                    decimal balance = 0;
                    foreach (DataRow dr in dtTasks.Rows)
                    {
                        totalachieved = 0;

                        dstasks = getdt.GetTaskDetails_TaskUID(dr["taskuid"].ToString());
                        if (dstasks.Rows.Count > 0)
                        {
                            dr["Unit"] = dstasks.Rows[0]["UnitforProgress"].ToString();
                            dr["Total Scope"] = dstasks.Rows[0]["UnitQuantity"].ToString();
                        }
                        totalachieved = getdt.GetTaskMeasurementBook(new Guid(dr["taskuid"].ToString())).Tables[0].AsEnumerable().Sum(x => decimal.Parse(x.Field<string>("Quantity")));
                        dr["Achieved till " + DateTime.Now.ToString("MMM dd yyyy")] = totalachieved.ToString();
                        dr["Sno"] = dtTasks.Rows.IndexOf(dr) + 1;
                        balance = decimal.Parse(dr["Total Scope"].ToString()) - totalachieved;

                        dr["Balance"] = (balance < 0 ? 0 : balance).ToString();
                    }
                    dtTasks.Columns.Remove("taskuid");

                    DataRow drTotal = dtTasks.NewRow();
                    drTotal[1] = "Total";
                    dtTasks.Rows.Add(drTotal);

                    for (int cnt = 3; cnt < dtTasks.Columns.Count; cnt++)
                    {
                        columnTotal = 0;
                        for (int i = 0; i < dtTasks.Rows.Count - 1; i++)
                        {
                            if (double.TryParse(dtTasks.Rows[i][cnt].ToString(), out columnTotalPerDay))
                            {
                                columnTotal += columnTotalPerDay;
                            }
                        }
                        dtTasks.Rows[dtTasks.Rows.Count - 1][cnt] = columnTotal;
                        if (cnt == 3)
                        {
                            lblTotalM.Text =Math.Round(columnTotal,0).ToString();
                        }
                        else if (cnt == 4)
                        {
                            lblExecutedM.Text = Math.Round(columnTotal, 0).ToString();
                        }
                        else if (cnt == 5)
                        {
                            lblBalanceM.Text = Math.Round(columnTotal, 0).ToString();
                        }
                    }
                   

                }
                else
                {
                    lblTotalM.Text = "0";
                    lblExecutedM.Text = "0";
                    lblBalanceM.Text = "0";
                }
            }
        }

        ////added on 10/02/2023
        //private void Bind_ConcreteChart()
        //{
        //    if (DDLWorkPackage.SelectedValue != "--Select--")
        //    {
        //        ltScript_Concrete.Text = string.Empty;

        //            StringBuilder strScript = new StringBuilder();
        //            strScript.Append(@" <script type='text/javascript'>

        //        google.charts.load('current', { packages: ['corechart', 'bar'] });
        //        google.charts.setOnLoadCallback(drawBasic);

        //        function drawBasic() {



        //        var data = google.visualization.arrayToDataTable([
        //             ['Element', 'Concrete in Cum', { role: 'style' }, { role: 'annotation' }],");
        //        string CurrencySymbol = "";

        //        strScript.Append("['Balance', " + lblBalanceM.Text + ", '#3366CC', '" + CurrencySymbol + ' ' + lblBalanceM.Text + "'],['Executed', " + lblExecutedM.Text + ", '#DC3912', '" + CurrencySymbol + ' ' + lblExecutedM.Text + "'],['Total(PCC+RCC)', " + lblTotalM.Text + ", '#109618', '" + CurrencySymbol + ' ' + lblTotalM.Text + "']]);");
        //        strScript.Append(@"var options = {
        //            title : 'Concrete in Cum',
        //            is3D: true, 
        //            legend: { position: 'none' },
        //            fontSize: 14,
        //            chartArea: {
        //                left: '10%',
        //                top: '10%',
        //                height: '75%',
        //                width: '80%'
        //            },
        //            height: 300
        //        };


        //        var chart = new google.visualization.ColumnChart(
        //          document.getElementById('Concrete_div'));
        //         chart.draw(data, options);

        //    }</script>");
        //        //ltScript_Cost.Text = strScript.ToString();
        //        ltScript_Concrete.Text = strScript.ToString();
        //        }
        //        else
        //        {
        //        //ltScript_Cost.Text = "<h3>No data</h3>";
        //        ltScript_Concrete.Text = "<h3>No data</h3>";

        //        }

        //}

        //private void Bind_ConcreteChart()
        //{
        //    if (DDLWorkPackage.SelectedValue != "--Select--")
        //    {
        //        ltScript_Concrete.Text = string.Empty;

        //        StringBuilder strScript = new StringBuilder();
        //        strScript.Append(@" <script type='text/javascript'>
                
        //        google.charts.load('current', { packages: ['corechart', 'bar'] });
        //        google.charts.setOnLoadCallback(drawBasic);

        //        function drawBasic() {
                     

        //                var data = google.visualization.arrayToDataTable([
        //            ['Concrete','Balance', 'Executed', 'Total(PCC+RCC)', { role: 'annotation' } ],
        //            ['Concrete'," + lblBalanceM.Text + "," + lblExecutedM.Text + "," + lblTotalM.Text + ",' '" + "]]);");

        //        strScript.Append(@"
        //          var options = {
        //            width: 400,
        //            height: 280,
        //            legend: { position: 'top', maxLines: 3 },
        //            bar: { groupWidth: '100%' },
        //            isStacked: true
        //          };


        //        var chart = new google.visualization.ColumnChart(
        //          document.getElementById('Concrete_div'));
        //         chart.draw(data, options);
                
        //    }</script>");
        //        //ltScript_Cost.Text = strScript.ToString();
        //        ltScript_Concrete.Text = strScript.ToString();
        //    }
        //    else
        //    {
        //        //ltScript_Cost.Text = "<h3>No data</h3>";
        //        ltScript_Concrete.Text = "<h3>No data</h3>";

        //    }

        //}
        private int SundaysInMonth(int month, int year)
        {
            DateTime today = new DateTime(year, month, 1);
            DateTime endOfMonth = new DateTime(year, month, DateTime.DaysInMonth(year, month));
            //get only last day of month
            int day = endOfMonth.Day;

            DateTime now = DateTime.Now;
            int count;
            count = 0;
            for (int i = 0; i < day; ++i)
            {
                DateTime d = new DateTime(year, month, i + 1);
                //Compare date with sunday
                if (d.DayOfWeek == DayOfWeek.Sunday & d <= DateTime.Now.Date)
                {
                    count = count + 1;
                }
            }

            return count;
        }

        //added on 10/02/2023
        private void Bind_LabourChart()
        {
            if (DDLWorkPackage.SelectedValue != "--Select--")
            {
                getLabourChartData();
                ltScript_Labour.Text = string.Empty;

               //decimal TotalLabour = getdt.GetLabourDashboardGraph(new Guid(DDLWorkPackage.SelectedValue), "Labour");

                string YearMonth = "";
                string YearMonthPrev ="";
                decimal AverageLabour = 0;
                decimal TotalLabour = 0;
                //
                decimal AverageLabourPrev = 0;
                decimal TotalLabourPrev = 0;
                //  ViewState["LastMonthYear"] = item.MonthYear;
                //   ViewState["LastMonthYear"] = average;
                if (ViewState["LastMonthYear"] != null)
                {
                    YearMonth = ViewState["LastMonthYear"].ToString();
                    TotalLabour = decimal.Parse(ViewState["LastMonthAvg"].ToString()); 
                    AverageLabour = Math.Round(TotalLabour, 0);
                    //
                    YearMonthPrev = ViewState["PrevMonthYear"].ToString();
                    TotalLabourPrev = decimal.Parse(ViewState["PrevMonthAvg"].ToString());
                    AverageLabourPrev = Math.Round(TotalLabourPrev, 0);
                }

                // DateTime LastDate = getdt.GetLastDateInResourceDeployment(new Guid(DDLWorkPackage.SelectedValue), "Labour");

                // if (LastDate.Month == DateTime.Now.Month & LastDate.Year == DateTime.Now.Year)
                // AverageLabour = Math.Round(TotalLabour / (DateTime.Now.Day  - SundaysInMonth(Convert.ToInt32(YearMonth[1]), Convert.ToInt32(YearMonth[0]))), 1);
                //  else
                // AverageLabour = Math.Round(TotalLabour / (DateTime.DaysInMonth(Convert.ToInt32(YearMonth[0]), Convert.ToInt32(YearMonth[1])) - SundaysInMonth(Convert.ToInt32(YearMonth[1]), Convert.ToInt32(YearMonth[0]))),1);

                StringBuilder strScript = new StringBuilder();
                strScript.Append(@" <script type='text/javascript'>

                google.charts.load('current', { packages: ['corechart', 'bar'] });
                google.charts.setOnLoadCallback(drawBasic);

                function drawBasic() {



                var data = google.visualization.arrayToDataTable([
                     ['Element', 'Manpower', { role: 'style' }, { role: 'annotation' }],");
                string CurrencySymbol = "";

                strScript.Append("['" + YearMonthPrev + "', " + AverageLabourPrev + ", '#3366CC', '" + CurrencySymbol + ' ' + AverageLabourPrev + "'],['" + YearMonth + "', " + AverageLabour + ", '#DC3912', '" + CurrencySymbol + ' ' + AverageLabour + "']]);");
                strScript.Append(@"var options = {
                    title : 'Average Manpower',
                    is3D: true, 
                    legend: 'none',
                    fontSize: 12,
                    colors:['blue','green'],
                    chartArea: {
                        left: '20%',
                        top: '10%',
                        height: '71%',
                        width: '80%'
                    },
               vAxis: {minValue: 200},
                    height: 370
                };


                var chart = new google.visualization.ColumnChart(
                  document.getElementById('labour_div'));
                 chart.draw(data, options);

            }</script>");
                //ltScript_Cost.Text = strScript.ToString();
                ltScript_Labour.Text = strScript.ToString();
            }
            else
            {
                //ltScript_Cost.Text = "<h3>No data</h3>";
                ltScript_Labour.Text = "<h3>No data</h3>";

            }

        }


        //added on 21/03/2023
        private void getLabourChartData()
        {
            try
            {

                if (DDLWorkPackage.SelectedValue != "--Select--")
                {
                    string[] months = new string[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
                    string resourceUID = Guid.NewGuid().ToString();
                    
                    resourceUID = "91888a63-5a7c-41dc-94df-9a3a53837e21";
                    
                    DataSet ds = getdt.GetResourceDeployment_by_OverallGraph(new Guid(DDLWorkPackage.SelectedValue), new Guid(resourceUID));

                    DataTable dt = ds.Tables[0];
                    var query = (from row in dt.AsEnumerable()
                                 select new tClass1
                                 {
                                     MonthYear = months[Convert.ToInt32(row[0].ToString().Split('/')[1]) - 1] + " " + row[0].ToString().Split('/')[0],
                                     dQuantity = Convert.ToDecimal(row[1].ToString()),
                                     dAverage = Convert.ToDecimal(row[2].ToString())
                                 });
                    //Average =  Convert.ToDecimal(row[1].ToString())/((DateTime.DaysInMonth(Convert.ToInt32(row[0].ToString().Split('/')[0]), Convert.ToInt32(row[0].ToString().Split('/')[1]))) - SundaysInMonth(Convert.ToInt32(row[0].ToString().Split('/')[1]), Convert.ToInt32(row[0].ToString().Split('/')[0])))
                    // tClass1 lastItem = query.Last();

                    // DateTime  LastDate = getdt.GetLastDateInResourceDeployment(new Guid(DDLWorkPackage.SelectedValue), "Labour");

                    //  if (LastDate.Month == DateTime.Now.Month & LastDate.Year == DateTime.Now.Year)
                    // lastItem.dAverage = lastItem.dQuantity / (DateTime.Now.Day  - SundaysInMonth(LastDate.Month ,LastDate.Year ));




                    ltScript_Labour.Text = string.Empty;

                    if (ds.Tables[0].Rows.Count > 0)
                    {
          //              StringBuilder strScript = new StringBuilder();


          //              strScript.Append(@" <script type='text/javascript'>
                
          //      google.charts.load('current', { packages: ['corechart', 'bar'] });
          //      google.charts.setOnLoadCallback(drawBasic);

          //      function drawBasic() {

          //      var data = google.visualization.arrayToDataTable([
          //['Month', 'Deployed Quantity','Deployed Average'],");


                        decimal planvalue = 0;
                        decimal average = 0;

                        int last = query.Count();

                        int cnt = 0;
                        int noOfDays = 0;
                        int MonthNo = 0;
                        foreach (var item in query.ToList())
                        {

                            planvalue = Math.Round(item.dQuantity, 0);
                           // average = item.dAverage;
                            MonthNo = getMonthNo(item.MonthYear.Split(' ')[0]);
                            noOfDays = getdt.GetResouceAveargeForMonth(new Guid(DDLWorkPackage.SelectedValue), new Guid(resourceUID), MonthNo, int.Parse(item.MonthYear.Split(' ')[1]));
                            average = Math.Round((item.dQuantity / item.dAverage), 0);
                            cnt = cnt + 1;

                            if (cnt == last)
                            {
                                //strScript.Append("['" + item.MonthYear + "'," + planvalue + "," + average + "],");
                                ViewState["LastMonthYear"] = item.MonthYear;
                                ViewState["LastMonthAvg"] = average;
                            }
                            else if (cnt == (last - 1))
                            {
                                ViewState["PrevMonthYear"] = item.MonthYear;
                                ViewState["PrevMonthAvg"] = average;
                                //strScript.Append("['" + item.MonthYear + "'," + planvalue + "," + average + "]]);");
                            }
                        }



//                        strScript.Append(@"var options = {
//          title : 'Time vs Resource Deployed',
          
//          hAxis: {title: 'Deployed Months',titleTextStyle: {
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
//                  document.getElementById('labour_div'));
//                 chart.draw(data, options);
                
//            }</script>");

//                        ltScript_Labour.Text = strScript.ToString();
                    }
                    else
                    {
                      //  ltScript_Labour.Text = "<h3>No data</h3>";
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
            switch (Month)
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
        protected void RdList3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RdList3.SelectedValue == "Total")
            {
                Session["select"] = "Total";
                Bind_DocumentsChart();
            }
            else
            {
                Session["select"] = "Contractor";
                Bind_DocumentsChart5();
            }

        }

        private void Bind_DocumentsChart5()
        {
            if (DDLWorkPackage.SelectedValue != "--Select--")
            {
                //DataSet ds = getdt.getDocumentCount_by_ProjectUID_WorkPackageUID2(new Guid(DDlProject.SelectedValue), new Guid(DDLWorkPackage.SelectedValue), Request.QueryString["Option"].ToString());
                // DataSet ds = getdt.getDocumentCount_by_ProjectUID_WorkPackageUID2(new Guid(DDlProject.SelectedValue), new Guid(DDLWorkPackage.SelectedValue), "Flow 2");
                DataSet ds = getdt.getDocumentCount_by_ProjectUID_WorkPackageUID2(new Guid(DDlProject.SelectedValue), new Guid(DDLWorkPackage.SelectedValue), "Flow 2");

                if (ds.Tables[0].Rows.Count > 0)
                {
                    StringBuilder strScript = new StringBuilder();
                    strScript.Append(@"<script type='text/javascript'>
                google.charts.load('current', { packages: ['corechart', 'bar'] });
                google.charts.setOnLoadCallback(drawBasic);

                function drawBasic() {
                    var data = google.visualization.arrayToDataTable([
                      ['Document', 'Ontime','Delayed', { role: 'annotation' }],");

                    foreach (DataRow r in ds.Tables[0].Rows)
                    {
                        string status_name = r.ItemArray[0].ToString();
                        int status_count = Convert.ToInt32(r.ItemArray[1].ToString());
                        int status_delay_count = 0; // Convert.ToInt32(r.ItemArray[2].ToString());

                        if (status_name == "Submitted")
                        {
                            status_name = "Under Review";
                        }

                        if (status_name != "ONTB Review" && status_name != "Code G" && status_name != "Code H" && status_name != "Client Approved")
                           strScript.Append("['" + status_name + "'," + Math.Abs(status_count - status_delay_count).ToString() + "," + status_delay_count.ToString() + ",'" + status_count.ToString() + "'],");
                    }


                    strScript.Remove(strScript.Length - 1, 1);
                    strScript.Append("]);");
                    strScript.Append(@"var options = {
                        is3D: true,
                        legend: { position: 'none' },
                        fontSize: 13,
                        isStacked: true,
                        chartArea: {
                            left: '25%',
                            top: '5%',
                            height: '88%',
                            width: '61%'
                        },
                        bars: 'horizontal',
                        annotations: {
                        alwaysOutside:true,
                        },
                        axes: {
                            x: {
                                0: { side: 'top', label: 'Percentage' } // Top x-axis.
                            }
                        },
                        hAxis: {
                            minValue: 0
                        }
                    };
                    function selectHandler()
                    {
                        var selection = chart.getSelection();
                        if (selection.length > 0)
                        {
                            var colLabel = data.getColumnLabel(selection[0].column);
                            var mydata = data.getValue(selection[0].row,0);
                            ");
                    strScript.Append("window.open('/_content_pages/document-drilldown/default.aspx?DocumentType=' + (colLabel + '_' + mydata) + '&selection=2' + '&Flow=Flow 2&ProjectUID=" + DDlProject.SelectedValue + "&WorkPackageUID=" + DDLWorkPackage.SelectedValue + "', '_self', true);");
                    //alert('The user selected ' + topping);
                    strScript.Append(@"}
                    }
                    
                    var chart = new google.visualization.BarChart(document.getElementById('DocChart_Div'));
                    google.visualization.events.addListener(chart, 'select', selectHandler);
                    chart.draw(data, options);
                }
            </script>");
                    ltScript_Document.Text = strScript.ToString();

                    BtoF.HRef = "/_content_pages/dashboard/default.aspx?option=Flow 2&selection=2&back=1";
                    BtoF.Visible = false;
                }
                else
                {
                    ltScript_Document.Text = "<h4>No data</h4>";
                }
            }
        }

        //private void Bind_DocumentsChart4()
        //{

        //    DataSet ds = getdt.getContractorDocumentCount_by_ProjectUID_WorkPackageUID(new Guid(DDlProject.SelectedValue), new Guid(DDLWorkPackage.SelectedValue));

        //    BtoF.Visible = false;

        //    if (ds == null)
        //    {
        //        ltScript_Document.Text = "<h4>No data</h4>";
        //    }
        //    else
        //    {
        //        if (ds.Tables[0].Rows.Count > 0)
        //        {
        //            StringBuilder strScript = new StringBuilder();

        //            strScript.Append(@"<script type='text/javascript'>
        //                        google.charts.load('current', { packages: ['corechart', 'bar'] });
        //                        google.charts.setOnLoadCallback(drawBasic);

        //                    function drawBasic() {
        //                        var data = google.visualization.arrayToDataTable([
        //                            ['Document','FlowAll','Delayed', { role: 'annotation' }],");

        //            string total_docs = ds.Tables[0].Rows[0][1].ToString();


        //            foreach (DataRow rw in ds.Tables[0].Rows)
        //            {
        //                strScript.Append("['" + rw[2].ToString() + "', " + Convert.ToInt32(rw[1].ToString()) + ", " + "0" + ",'" + Convert.ToInt32(rw[1].ToString()) + "'],");
        //            }

        //            strScript.Remove(strScript.Length - 1, 1);
        //            strScript.Append("]);");
        //            strScript.Append(@"var options = {
        //                        is3D: true,
        //                        legend: { position: 'none' },
        //                        fontSize: 11,
        //                        isStacked: true,
        //                        height : 300,
        //                        chartArea: {
        //                            left: '25%',
        //                            top: '5%',
        //                            height: '100%',
        //                            width: '61%'
        //                        },
        //                        bars: 'horizontal',
        //                        annotations: {
        //                        alwaysOutside:false,
        //                        },
        //                        axes: {
        //                            x: {
        //                                0: { side: 'top', label: 'Percentage' } // Top x-axis.
        //                            }
        //                        },
        //                        hAxis: {
        //                            minValue: 0
        //                        }
        //                    };

        //                    function selectHandler()
        //                    {
        //                        var selection = chart.getSelection();
        //                        if (selection.length > 0)
        //                        {
        //                            var colLabel = data.getColumnLabel(selection[0].column);
        //                            var mydata = data.getValue(selection[0].row,0);
        //                            ");
        //            strScript.Append("window.open('/_content_pages/dashboard/default.aspx?option=' + mydata +'&selection=2' ,'_self', true);");

        //            strScript.Append(@"}
        //                    }

        //                    var chart = new google.visualization.BarChart(document.getElementById('DocChart_Div'));
        //                    google.visualization.events.addListener(chart, 'select', selectHandler);
        //                    chart.draw(data, options);
        //                }
        //            </script>");
        //            ltScript_Document.Text = strScript.ToString();
        //        }
        //        else
        //        {
        //            ltScript_Document.Text = "<h4>No data</h4>";
        //        }
        //    }
        //}

        private void Bind_DocumentsChart1()
        {
            if (DDLWorkPackage.SelectedValue != "--Select--")
            {
                DataSet ds = getdt.getDocumentCount_by_ProjectUID_WorkPackageUID1(new Guid(DDlProject.SelectedValue), new Guid(DDLWorkPackage.SelectedValue), Request.QueryString["Option"].ToString());

                if (ds.Tables[0].Rows.Count > 0)
                {
                    StringBuilder strScript = new StringBuilder();
                    strScript.Append(@"<script type='text/javascript'>
                google.charts.load('current', { packages: ['corechart', 'bar'] });
                google.charts.setOnLoadCallback(drawBasic);

                function drawBasic() {
                    var data = google.visualization.arrayToDataTable([
                      ['Document', 'Ontime','Delayed', { role: 'annotation' }],");

                    foreach (DataRow r in ds.Tables[0].Rows)
                    {
                        string status_name = r.ItemArray[0].ToString();
                        int status_count = Convert.ToInt32(r.ItemArray[1].ToString());
                        int status_delay_count = 0; // Convert.ToInt32(r.ItemArray[2].ToString());

                        strScript.Append("['" + status_name + "'," + Math.Abs(status_count - status_delay_count).ToString() + "," + status_delay_count.ToString() + ",'" + status_count.ToString() + "'],");
                    }


                    strScript.Remove(strScript.Length - 1, 1);
                    strScript.Append("]);");
                    strScript.Append(@"var options = {
                        is3D: true,
                        legend: { position: 'none' },
                        fontSize: 13,
                        isStacked: true,
                        chartArea: {
                            left: '25%',
                            top: '5%',
                            height: '88%',
                            width: '61%'
                        },
                        bars: 'horizontal',
                        annotations: {
                        alwaysOutside:true,
                        },
                        axes: {
                            x: {
                                0: { side: 'top', label: 'Percentage' } // Top x-axis.
                            }
                        },
                        hAxis: {
                            minValue: 0
                        }
                    };
                    function selectHandler()
                    {
                        var selection = chart.getSelection();
                        if (selection.length > 0)
                        {
                            var colLabel = data.getColumnLabel(selection[0].column);
                            var mydata = data.getValue(selection[0].row,0);
                            ");
                    //  strScript.Append("window.open('/_content_pages/document-drilldown/default.aspx?DocumentType=' + (colLabel + '_' + mydata) + '&ProjectUID=" + DDlProject.SelectedValue + "&WorkPackageUID=" + DDLWorkPackage.SelectedValue + "', '_self', true);");

                    strScript.Append("window.open('/_content_pages/document-drilldown/default.aspx?DocumentType=' + (colLabel + '_' + mydata) + '&selection=1' + '&Flow=" + Request.QueryString["Option"].ToString() + "&ProjectUID=" + DDlProject.SelectedValue + "&WorkPackageUID=" + DDLWorkPackage.SelectedValue + "', '_self', true);");

                    //alert('The user selected ' + topping);
                    strScript.Append(@"}
                    }
                    
                    var chart = new google.visualization.BarChart(document.getElementById('DocChart_Div'));
                    google.visualization.events.addListener(chart, 'select', selectHandler);
                    chart.draw(data, options);
                }
            </script>");
                    ltScript_Document.Text = strScript.ToString();

                    BtoF.HRef = "/_content_pages/dashboard/default.aspx";
                    BtoF.Visible = true;
                }
                else
                {
                    ltScript_Document.Text = "<h4>No data</h4>";
                }
            }
        }

        //private void Bind_DocumentsChart()
        //{

        //    DataSet ds = getdt.getDocumentCount_by_ProjectUID_WorkPackageUID(new Guid(DDlProject.SelectedValue), new Guid(DDLWorkPackage.SelectedValue));

        //    BtoF.Visible = false;

        //    if (ds == null)
        //    {
        //        ltScript_Document.Text = "<h4>No data</h4>";
        //    }
        //    else
        //    {
        //        if (ds.Tables[0].Rows.Count > 0)
        //        {
        //            StringBuilder strScript = new StringBuilder();

        //            strScript.Append(@"<script type='text/javascript'>
        //                        google.charts.load('current', { packages: ['corechart', 'bar'] });
        //                        google.charts.setOnLoadCallback(drawBasic);

        //                    function drawBasic() {
        //                        var data = google.visualization.arrayToDataTable([
        //                            ['Document','FlowAll','Delayed', { role: 'annotation' }],");

        //            string total_docs = ds.Tables[0].Rows[0][1].ToString();


        //            foreach (DataRow rw in ds.Tables[0].Rows)
        //            {
        //                strScript.Append("['" + rw[2].ToString() + "', " + Convert.ToInt32(rw[1].ToString()) + ", " + "0" + ",'" + Convert.ToInt32(rw[1].ToString()) + "'],");
        //            }

        //            strScript.Remove(strScript.Length - 1, 1);
        //            strScript.Append("]);");
        //            strScript.Append(@"var options = {
        //                        is3D: true,
        //                        legend: { position: 'none' },
        //                        fontSize: 11,
        //                        isStacked: true,
        //                        height : 300,
        //                        chartArea: {
        //                            left: '25%',
        //                            top: '5%',
        //                            height: '100%',
        //                            width: '61%'
        //                        },
        //                        bars: 'horizontal',
        //                        annotations: {
        //                        alwaysOutside:false,
        //                        },
        //                        axes: {
        //                            x: {
        //                                0: { side: 'top', label: 'Percentage' } // Top x-axis.
        //                            }
        //                        },
        //                        hAxis: {
        //                            minValue: 0
        //                        }
        //                    };

        //                    function selectHandler()
        //                    {
        //                        var selection = chart.getSelection();
        //                        if (selection.length > 0)
        //                        {
        //                            var colLabel = data.getColumnLabel(selection[0].column);
        //                            var mydata = data.getValue(selection[0].row,0);
        //                            ");
        //            strScript.Append("window.open('/_content_pages/dashboard/default.aspx?option=' + mydata + '&selection=1' ,'_self', true);");

        //            strScript.Append(@"}
        //                    }

        //                    var chart = new google.visualization.BarChart(document.getElementById('DocChart_Div'));
        //                    google.visualization.events.addListener(chart, 'select', selectHandler);
        //                    chart.draw(data, options);
        //                }
        //            </script>");
        //            ltScript_Document.Text = strScript.ToString();
        //        }
        //        else
        //        {
        //            ltScript_Document.Text = "<h4>No data</h4>";
        //        }
        //    }
        //}

        private void Bind_DocumentsChart()
        {

            DataSet ds = getdt.getDocumentCount_by_ProjectUID_WorkPackageUID(new Guid(DDlProject.SelectedValue), new Guid(DDLWorkPackage.SelectedValue));

            BtoF.Visible = false;

            if (ds == null)
            {
                ltScript_Document.Text = "<h4>No data</h4>";
            }
            else
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    StringBuilder strScript = new StringBuilder();

                    strScript.Append(@"<script type='text/javascript'>
                                google.charts.load('current', { packages: ['corechart', 'bar'] });
                                google.charts.setOnLoadCallback(drawBasic);

                            function drawBasic() {
                                var data = google.visualization.arrayToDataTable([
                                    ['Document','FlowAll','Delayed', { role: 'annotation' }],");

                    string total_docs = ds.Tables[0].Rows[0][1].ToString();


                    foreach (DataRow rw in ds.Tables[0].Rows)
                    {
                        strScript.Append("['" + rw[2].ToString() + "', " + Convert.ToInt32(rw[1].ToString()) + ", " + "0" + ",'" + Convert.ToInt32(rw[1].ToString()) + "'],");
                    }

                    strScript.Remove(strScript.Length - 1, 1);
                    strScript.Append("]);");
                    strScript.Append(@"var options = {
                                is3D: true,
                                legend: { position: 'none' },
                                fontSize: 11,
                                isStacked: true,
                                height : 300,
                                chartArea: {
                                    left: '25%',
                                    top: '5%',
                                    height: '100%',
                                    width: '61%'
                                },
                                bars: 'horizontal',
                                annotations: {
                                alwaysOutside:false,
                                },
                                axes: {
                                    x: {
                                        0: { side: 'top', label: 'Percentage' } // Top x-axis.
                                    }
                                },
                                hAxis: {
                                    minValue: 0
                                }
                            };

                            function selectHandler()
                            {
                                var selection = chart.getSelection();
                                if (selection.length > 0)
                                {
                                    var colLabel = data.getColumnLabel(selection[0].column);
                                    var mydata = data.getValue(selection[0].row,0);
                                    ");
                    if (WebConfigurationManager.AppSettings["domain"] == "Suez")
                    {
                        strScript.Append("window.open('/_content_pages/dashboard/default.aspx?option=' + mydata + '&selection=1' ,'_self', true);");
                    }
                    //else if (WebConfigurationManager.AppSettings["domain"] == "ONTB" && (DDlProject.SelectedItem.ToString() == "CP-02" || DDlProject.SelectedItem.ToString() == "CP-03"))
                    //{
                    //    strScript.Append("window.open('/_content_pages/dashboard/default.aspx?option=' + mydata + '&selection=1' ,'_self', true);");
                    //}
                    else
                    {
                        // strScript.Append("window.open('/_content_pages/document-drilldown/default.aspx?DocumentType=' + (colLabel + '_' + mydata) + '&selection=1' + '&Flow=" + Request.QueryString["Option"].ToString() + "&ProjectUID=" + DDlProject.SelectedValue + "&WorkPackageUID=" + DDLWorkPackage.SelectedValue + "', '_self', true);");
                        strScript.Append("window.open('/_content_pages/document-drilldown/default.aspx?DocumentType=' + (colLabel + '_' + mydata) + '&selection=3&ProjectUID=" + DDlProject.SelectedValue + "&WorkPackageUID=" + DDLWorkPackage.SelectedValue + "', '_self', true);");

                    }

                    strScript.Append(@"}
                            }

                            var chart = new google.visualization.BarChart(document.getElementById('DocChart_Div'));
                            google.visualization.events.addListener(chart, 'select', selectHandler);
                            chart.draw(data, options);
                        }
                    </script>");
                    ltScript_Document.Text = strScript.ToString();
                }
                else
                {
                    ltScript_Document.Text = "<h4>No data</h4>";
                }
            }
        }

        private void Bind_DocumentsChart4()
        {

            DataSet ds = getdt.getContractorDocumentCount_by_ProjectUID_WorkPackageUID(new Guid(DDlProject.SelectedValue), new Guid(DDLWorkPackage.SelectedValue));

            BtoF.Visible = false;

            if (ds == null)
            {
                ltScript_Document.Text = "<h4>No data</h4>";
            }
            else
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    StringBuilder strScript = new StringBuilder();

                    strScript.Append(@"<script type='text/javascript'>
                                google.charts.load('current', { packages: ['corechart', 'bar'] });
                                google.charts.setOnLoadCallback(drawBasic);

                            function drawBasic() {
                                var data = google.visualization.arrayToDataTable([
                                    ['Document','FlowAll','Delayed', { role: 'annotation' }],");

                    string total_docs = ds.Tables[0].Rows[0][1].ToString();


                    foreach (DataRow rw in ds.Tables[0].Rows)
                    {
                        strScript.Append("['" + rw[2].ToString() + "', " + Convert.ToInt32(rw[1].ToString()) + ", " + "0" + ",'" + Convert.ToInt32(rw[1].ToString()) + "'],");
                    }

                    strScript.Remove(strScript.Length - 1, 1);
                    strScript.Append("]);");
                    strScript.Append(@"var options = {
                                is3D: true,
                                legend: { position: 'none' },
                                fontSize: 11,
                                isStacked: true,
                                height : 300,
                                chartArea: {
                                    left: '25%',
                                    top: '5%',
                                    height: '100%',
                                    width: '61%'
                                },
                                bars: 'horizontal',
                                annotations: {
                                alwaysOutside:false,
                                },
                                axes: {
                                    x: {
                                        0: { side: 'top', label: 'Percentage' } // Top x-axis.
                                    }
                                },
                                hAxis: {
                                    minValue: 0
                                }
                            };

                            function selectHandler()
                            {
                                var selection = chart.getSelection();
                                if (selection.length > 0)
                                {
                                    var colLabel = data.getColumnLabel(selection[0].column);
                                    var mydata = data.getValue(selection[0].row,0);
                                    ");
                    if (WebConfigurationManager.AppSettings["domain"] == "Suez")
                    {
                        strScript.Append("window.open('/_content_pages/dashboard/default.aspx?option=' + mydata +'&selection=2' ,'_self', true);");
                    }
                    //else if (WebConfigurationManager.AppSettings["domain"] == "ONTB" && (DDlProject.SelectedItem.ToString() == "CP-02" || DDlProject.SelectedItem.ToString() == "CP-03"))
                    //{
                    //    strScript.Append("window.open('/_content_pages/dashboard/default.aspx?option=' + mydata +'&selection=2' ,'_self', true);");
                    //}
                    else
                        //strScript.Append("window.open('/_content_pages/document-drilldown/default.aspx?DocumentType=' + (colLabel + '_' + mydata) + '&selection=2' + '&Flow=" + Request.QueryString["Option"].ToString() + "&ProjectUID=" + DDlProject.SelectedValue + "&WorkPackageUID=" + DDLWorkPackage.SelectedValue + "', '_self', true);");
                        strScript.Append("window.open('/_content_pages/document-drilldown/default.aspx?DocumentType=' + (colLabel + '_' + mydata) + '&selection=4&ProjectUID=" + DDlProject.SelectedValue + "&WorkPackageUID=" + DDLWorkPackage.SelectedValue + "', '_self', true);");

                    strScript.Append(@"}
                            }

                            var chart = new google.visualization.BarChart(document.getElementById('DocChart_Div'));
                            google.visualization.events.addListener(chart, 'select', selectHandler);
                            chart.draw(data, options);
                        }
                    </script>");
                    ltScript_Document.Text = strScript.ToString();
                }
                else
                {
                    ltScript_Document.Text = "<h4>No data</h4>";
                }
            }
        }

        private void Bind_ConcreteChart()
        {
            if (DDLWorkPackage.SelectedValue != "--Select--")
            {
                ltScript_Concrete.Text = string.Empty;

                string total = (Math.Round((Convert.ToDecimal(lblBalanceM.Text) + Convert.ToDecimal(lblExecutedM.Text)), 2)).ToString();

                StringBuilder strScript = new StringBuilder();
                strScript.Append(@" <script type='text/javascript'>
                
                google.charts.load('current', { packages: ['corechart', 'bar'] });
                google.charts.setOnLoadCallback(drawBasic);

                function drawBasic() {
                         var data = google.visualization.arrayToDataTable([
                    ['Concrete','Executed', 'Balance', { role: 'annotation' } ],
                    ['Concrete'," + lblExecutedM.Text + "," + lblBalanceM.Text + "," + "' '" + "]]);");

                strScript.Append(@"
                  var options = {
                    title : 'Total Concrete : " + total + @"' ,
                    titleTextStyle: {
       
        fontSize: 12,
        bold: true   
       
    },
colors:['green','blue'],
fontSize: 12,
                    width: 330,
                    height: 370,
                    legend: { position: 'top'},
                    bar: { groupWidth: '70%' },
                    isStacked: true
                  };


                var chart = new google.visualization.ColumnChart(
                  document.getElementById('Concrete_div'));
                 chart.draw(data, options);
                
            }</script>");
                //ltScript_Cost.Text = strScript.ToString();
                ltScript_Concrete.Text = strScript.ToString();
            }
            else
            {
                //ltScript_Cost.Text = "<h3>No data</h3>";
                ltScript_Concrete.Text = "<h3>No data</h3>";

            }

        }

        //added on 23/03/2023
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

        protected void BindDocumentSummary()
        {
            try {
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
                
                if (dt != null && dt.Rows.Count > 0)
                {

                    // btnDocumentSummaryExportExcel.Visible = true;
                    // DocumentSummaryReportName.InnerHtml = "Report Name : Document Category Summary Report";
                    // DocumentSummaryProjectName.InnerHtml = "Project Name : " + DDlProject.SelectedItem.Text + " (" + DDLWorkPackage.SelectedItem.Text + ")";

                    ViewState["Export"] = "1";



                    dt.Columns.Add("SNo", typeof(int)).SetOrdinal(0);
                    dt.Columns.Add("Total Submitted").SetOrdinal(2); ;
                    DataRow dr = dt.NewRow();
                    dr[1] = "Total";

                    int columnTotal = 0;
                    int rowTotal = 0;
                    int Grantotal = 0;
                    for (int i = 0; i < dt.Rows.Count; i++)
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
                        catch (Exception ex)
                        {

                        }

                    }
                    for (int cnt = 3; cnt < dt.Columns.Count; cnt++)
                    {
                        dr[cnt] = dt.AsEnumerable().Sum(row => row.Field<Int32>(dt.Columns[cnt].ToString()));

                    }
                    dr[2] = Grantotal.ToString();
                    dt.Rows.Add(dr);
                    //added on 24/03/2023 for merging 2 Civil Categories
                    if (dt.Rows.Count >= 2)
                    {
                        if(dt.Rows[0][1].ToString().Contains("Civil") && dt.Rows[1][1].ToString().Contains("Civil"))
                        {
                            dt.Rows[1][1] = "Civil & Structural/Civil Drawings";
                            dt.Rows[1][2] = int.Parse(dt.Rows[1][2].ToString()) + int.Parse(dt.Rows[0][2].ToString());
                            dt.Rows[1][3] = int.Parse(dt.Rows[1][3].ToString()) + int.Parse(dt.Rows[0][3].ToString());
                            dt.Rows[1][4] = int.Parse(dt.Rows[1][4].ToString()) + int.Parse(dt.Rows[0][4].ToString());
                            dt.Rows[1][5] = int.Parse(dt.Rows[1][5].ToString()) + int.Parse(dt.Rows[0][5].ToString());
                            dt.Rows[1][6] = int.Parse(dt.Rows[1][6].ToString()) + int.Parse(dt.Rows[0][6].ToString());
                            dt.Rows[1][7] = int.Parse(dt.Rows[1][7].ToString()) + int.Parse(dt.Rows[0][7].ToString());
                           // dt.Rows[1][8] = int.Parse(dt.Rows[1][8].ToString()) + int.Parse(dt.Rows[0][8].ToString());
                           //
                           for(int K=1;K < dt.Rows.Count - 1; K++)
                            {
                                dt.Rows[K][0] = K;
                            }
                            ViewState["Hide"] = "Yes";
                        }
                        else
                        {
                            for (int K = 0; K < dt.Rows.Count - 1; K++)
                            {
                                dt.Rows[K][0] = K+1;
                            }
                            ViewState["Hide"] = "No";
                        }

                    }
                    
                    //
                        GrdInvoiceRaBillsDeductions.DataSource = dt;
                    GrdInvoiceRaBillsDeductions.DataBind();
                    //
                   // GrdInvoiceRaBillsDeductions.Columns[8].Visible = false;
                }
                else
                {
                    GrdInvoiceRaBillsDeductions.DataSource = null;
                    GrdInvoiceRaBillsDeductions.DataBind();
                    // btnDocumentSummaryExportExcel.Visible = false;
                }

            }
            catch(Exception ex)
            {
                throw ex;
            }
         }

        protected void GrdInvoiceRaBillsDeductions_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
               e.Row.Cells[8].Text = "";
            }
                if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if(e.Row.Cells[1].Text == "Civil and Structural" && ViewState["Hide"].ToString() == "Yes")
                {
                    e.Row.Visible = false;
                }
                e.Row.Cells[7].Visible = false;
            }
        }
    }
}