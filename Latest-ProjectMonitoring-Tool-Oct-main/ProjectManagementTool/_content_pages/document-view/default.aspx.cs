using ProjectManager.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Configuration;
using System.IO;
using System.Net;
using System.Web.UI.HtmlControls;

namespace ProjectManagementTool._content_pages.document_view
{
    public partial class _default : System.Web.UI.Page
    {
        DBGetData getdt = new DBGetData();
        TaskUpdate gettk = new TaskUpdate();
      //  private static string SelectedTaskUID = string.Empty;
        
       // private static string SelectedTaskName = string.Empty;
        public int SearchResultCount = 0;
        Dictionary<string, string> folderNames = new Dictionary<string, string>();
        Dictionary<string, string> folderNamesAc = new Dictionary<string, string>();
        string folder = "";
        protected void Page_Load(object sender, EventArgs e)
        {
           
            ScriptManager.RegisterStartupScript(
                        UpdatePanel2,
                        this.GetType(),
                        "MyAction",
                        "BindEvents();",
                        true);
            if (Session["Username"] == null)
            {
                Response.Redirect("~/Login.aspx");
            }
            if (!IsPostBack)
            {
                BindProject();
                SelectedProjectWorkpackage("Project");
                DDlProject_SelectedIndexChanged(sender, e);
                PageLoadItemsBind();
                ViewState["SelectedTaskUID"] = string.Empty;
                btnSubmit.Visible = false;
                btnClear.Visible = false;
                DocumentDiv.Visible = false;
                //
                if (Session["IsClient"].ToString() == "Y" || Session["UserID"].ToString() == "ns.rao04@gmail.com")
                {
                    RBOptionList.Items[2].Selected = true;
                    RBOptionList_SelectedIndexChanged(sender, e);
                    btnSubmit_Click(sender, e);
                    divoption.Visible = false;
                    MainTask.Visible = false;
                    divsubmit.Visible = false;
                }
                else if (Session["TypeOfUser"].ToString() == "DDE" || Session["TypeOfUser"].ToString() == "DE")
                {
                    if (RBOptionList.Items.Count == 3)
                        RBOptionList.Items.RemoveAt(2);
                }
                //
                //
                if (Session["TypeOfUser"].ToString() == "NJSD")
                {
                    btnSubmittal.Visible = false;
                }
            }
           
        }
        private void PageLoadItemsBind()
        {
            DataSet dscheck = new DataSet();
            dscheck = getdt.GetUsertypeFunctionality_Mapping(Session["TypeOfUser"].ToString());
            ViewState["isDelete"] = "false";
            ViewState["isUpload"] = "false";
            ViewState["isUpdateStatus"] = "false";
            ViewState["isView"] = "false";
            ViewState["isDownloadClient"] = "false";
            ViewState["isDownloadNJSE"] = "false";
            Session["isDownloadClient"] = "false";
            Session["isDownloadNJSE"] = "false";
            GrdActualDocuments_new.Columns[7].Visible = false;

            GrdNewDocument.Columns[6].Visible = false;
            //
            GrdActualDocuments_new.Columns[6].Visible = false;
            GrdNewDocument.Columns[5].Visible = false;
            //
            GrdNewDocument.Columns[4].Visible = false;
            //
            btnSubmittal.Visible = false;
            if (dscheck.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dscheck.Tables[0].Rows)
                {
                    if (dr["Code"].ToString() == "FP" || dr["Code"].ToString() == "FO") // Delete submittal and documents
                    {

                        //
                        if (Session["IsClient"].ToString() != "Y")
                        {
                            ViewState["isDelete"] = "true";
                            GrdActualDocuments_new.Columns[7].Visible = true;
                            GrdNewDocument.Columns[6].Visible = true;
                        }
                    }
                    if (dr["Code"].ToString() == "FH") // upload documents/ edit / add submittal
                    {
                        ViewState["isUpload"] = "true";
                        btnSubmittal.Visible = true;
                        //
                        GrdActualDocuments_new.Columns[6].Visible = true;
                        GrdNewDocument.Columns[5].Visible = true;
                        GrdNewDocument.Columns[4].Visible = true;
                    }
                    if (dr["Code"].ToString() == "FU") // upload documents/ edit 
                    {
                        ViewState["isUpdateStatus"] = "true";
                    }
                    if (dr["Code"].ToString() == "FI") // View Documents
                    {
                        ViewState["isView"] = "true";
                    }
                    if (dr["Code"].ToString() == "FN") // DOWNLOAD APPROVED DOCUMENTS (FINAL APPROVED BY CLIENT)
                    {
                        ViewState["isDownloadClient"] = "true";
                        Session["isDownloadClient"] = "true";
                    }
                    if (dr["Code"].ToString() == "FM") // DOWNLOAD APPROVED DOCUMENTS (FINAL APPROVED BY NJSEI)
                    {
                        ViewState["isDownloadNJSE"] = "true";
                        Session["isDownloadNJSE"] = "true";
                    }
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

        }

        protected void DDlProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDlProject.SelectedValue != "")
            {
                //DataSet ds = getdt.GetWorkPackages_By_ProjectUID(new Guid(DDlProject.SelectedValue));
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
                    SelectedProjectWorkpackage("Workpackage");
                    Session["Project_Workpackage"] = DDlProject.SelectedValue + "_" + DDLWorkPackage.SelectedValue;
                    //BindMainTask(DDLWorkPackage.SelectedValue);
                    OptionBind(DDLWorkPackage.SelectedValue);
                    //
                     ViewState["SelectedTaskUID"] = string.Empty;
                    btnSubmit.Visible = false;
                    btnClear.Visible = false;
                    DocumentDiv.Visible = false;
                }
                MainTask.Visible = false;
                Task1.Visible = false;
                Task2.Visible = false;
                Task3.Visible = false;
                Task4.Visible = false;
                Task5.Visible = false;
                Task6.Visible = false;
                Task7.Visible = false;
                //
                //
                if (Session["IsClient"].ToString() == "Y" || Session["UserID"].ToString() == "ns.rao04@gmail.com")
                {
                    RBOptionList.Items[2].Selected = true;
                    RBOptionList_SelectedIndexChanged(sender, e);
                    btnSubmit_Click(sender, e);
                    divoption.Visible = false;
                    MainTask.Visible = false;
                    divsubmit.Visible = false;
                }
                else if (Session["TypeOfUser"].ToString() == "DDE" || Session["TypeOfUser"].ToString() == "DE")
                {
                    RBOptionList.Items.RemoveAt(2);
                }
                //
            }
        }

        protected void DDLWorkPackage_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["Project_Workpackage"] = DDlProject.SelectedValue + "_" + DDLWorkPackage.SelectedValue;
            //BindMainTask(DDLWorkPackage.SelectedValue);
            OptionBind(DDLWorkPackage.SelectedValue);
            MainTask.Visible = false;
            Task1.Visible = false;
            Task2.Visible = false;
            Task3.Visible = false;
            Task4.Visible = false;
            Task5.Visible = false;
            Task6.Visible = false;
            Task7.Visible = false;
        }

        internal void SelectedProjectWorkpackage(string pType)
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

        private void OptionBind(string WorkpackgeUID)
        {
            DataSet dsoption = getdt.GetSelectedOption_By_WorkpackageUID(new Guid(WorkpackgeUID));
            RBOptionList.DataSource = dsoption;
            RBOptionList.DataBind();
            if (dsoption.Tables[0].Rows.Count > 0)
            {
                RBOptionList.Items.Insert(2, "Correspondence");
            }
        }
        private void BindMainTask(string WorkpackageUID)
        {
            DataSet ds = getdt.GetTasksForWorkPackages(WorkpackageUID);
            DDLMainTask.DataTextField = "Name";
            DDLMainTask.DataValueField = "TaskUID";
            DDLMainTask.DataSource = ds;
            DDLMainTask.DataBind();
            DDLMainTask.Items.Insert(0, "--Select--");
        }

        protected void DDLMainTask_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDLMainTask.SelectedValue != "--Select--")
            {
                DataSet ds = getdt.GetSubTasksForWorkPackages(DDLMainTask.SelectedValue);
                DDLSubTask.DataTextField = "Name";
                DDLSubTask.DataValueField = "TaskUID";
                DDLSubTask.DataSource = ds;
                DDLSubTask.DataBind();
                DDLSubTask.Items.Insert(0, "--Select--");
                 ViewState["SelectedTaskUID"] = DDLMainTask.SelectedValue;
                 ViewState["SelectedTaskName"] = DDLMainTask.SelectedItem.Text;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    Task1.Visible = true;
                    
                }
            }
        }

        protected void DDLSubTask_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDLSubTask.SelectedValue != "--Select--")
            {
                DataSet ds = getdt.GetSubtoSubTasksForWorkPackages(DDLSubTask.SelectedValue);
                DDLSubTask1.DataTextField = "Name";
                DDLSubTask1.DataValueField = "TaskUID";
                DDLSubTask1.DataSource = ds;
                DDLSubTask1.DataBind();
                DDLSubTask1.Items.Insert(0, "--Select--");
                 ViewState["SelectedTaskUID"] = DDLSubTask.SelectedValue;
                 ViewState["SelectedTaskName"] = DDLSubTask.SelectedItem.Text;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    Task2.Visible = true;
                    
                }
            }
        }

        protected void DDLSubTask1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDLSubTask1.SelectedValue != "--Select--")
            {
                DataSet ds = getdt.GetSubtoSubtoSubTasksForWorkPackages(DDLSubTask1.SelectedValue);
                DDLSubTask2.DataTextField = "Name";
                DDLSubTask2.DataValueField = "TaskUID";
                DDLSubTask2.DataSource = ds;
                DDLSubTask2.DataBind();
                DDLSubTask2.Items.Insert(0, "--Select--");
                 ViewState["SelectedTaskUID"] = DDLSubTask1.SelectedValue;
                 ViewState["SelectedTaskName"] = DDLSubTask1.SelectedItem.Text;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    Task3.Visible = true;
                    
                }
                
            }
        }

        //protected void DDLSubTask2_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (DDLSubTask2.SelectedValue != "--Select--")
        //    {
        //        DataSet ds = getdt.GetSubtoSubtoSubtoSubTasksForWorkPackages(DDLSubTask2.SelectedValue);
        //        DDLSubTask3.DataTextField = "Name";
        //        DDLSubTask3.DataValueField = "TaskUID";
        //        DDLSubTask3.DataSource = ds;
        //        DDLSubTask3.DataBind();
        //        DDLSubTask3.Items.Insert(0, "--Select--");
        //         ViewState["SelectedTaskUID"] = DDLSubTask2.SelectedValue;
        //         ViewState["SelectedTaskName"] = DDLSubTask2.SelectedItem.Text;
        //        if (ds.Tables[0].Rows.Count > 0)
        //        {
        //            Task4.Visible = true;

        //        }

        //    }
        //}

        //protected void DDLSubTask3_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (DDLSubTask3.SelectedValue != "--Select--")
        //    {
        //        DataSet ds = getdt.GetSubtoSubtoSubtoSubtoSubTasksForWorkPackages(DDLSubTask3.SelectedValue);
        //        DDLSubTask4.DataTextField = "Name";
        //        DDLSubTask4.DataValueField = "TaskUID";
        //        DDLSubTask4.DataSource = ds;
        //        DDLSubTask4.DataBind();
        //        DDLSubTask4.Items.Insert(0, "--Select--");
        //         ViewState["SelectedTaskUID"] = DDLSubTask4.SelectedValue;
        //         ViewState["SelectedTaskName"] = DDLSubTask3.SelectedItem.Text;
        //        if (ds.Tables[0].Rows.Count > 0)
        //        {
        //            Task5.Visible = true;

        //        }
        //    }
        //}

        //protected void DDLSubTask4_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (DDLSubTask4.SelectedValue != "--Select--")
        //    {
        //         ViewState["SelectedTaskUID"] = DDLSubTask4.SelectedValue;
        //        DataSet ds = getdt.GetSubTask_By_ParentTask_Level(DDLSubTask4.SelectedValue,7);
        //        DDLSubTask5.DataTextField = "Name";
        //        DDLSubTask5.DataValueField = "TaskUID";
        //        DDLSubTask5.DataSource = ds;
        //        DDLSubTask5.DataBind();
        //        DDLSubTask5.Items.Insert(0, "--Select--");
        //         ViewState["SelectedTaskName"] = DDLSubTask4.SelectedItem.Text;
        //        if (ds.Tables[0].Rows.Count > 0)
        //        {
        //            Task6.Visible = true;

        //        }
        //    }
        //}

        //protected void DDLSubTask5_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (DDLSubTask5.SelectedValue != "--Select--")
        //    {
        //         ViewState["SelectedTaskUID"] = DDLSubTask5.SelectedValue;
        //        DataSet ds = getdt.GetSubTask_By_ParentTask_Level(DDLSubTask5.SelectedValue, 8);
        //        DDLSubTask6.DataTextField = "Name";
        //        DDLSubTask6.DataValueField = "TaskUID";
        //        DDLSubTask6.DataSource = ds;
        //        DDLSubTask6.DataBind();
        //        DDLSubTask6.Items.Insert(0, "--Select--");
        //         ViewState["SelectedTaskName"] = DDLMainTask.SelectedItem.Text;
        //        if (ds.Tables[0].Rows.Count > 0)
        //        {
        //            Task7.Visible = true;

        //        }
        //    }
        //}

        protected void DDLSubTask2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDLSubTask2.SelectedValue != "--Select--")
            {
                 ViewState["SelectedTaskUID"] = DDLSubTask2.SelectedValue;
                 ViewState["SelectedTaskName"] = DDLSubTask2.SelectedItem.Text;
                DataSet ds = getdt.GetSubtoSubtoSubtoSubTasksForWorkPackages(DDLSubTask2.SelectedValue);
                DDLSubTask3.DataTextField = "Name";
                DDLSubTask3.DataValueField = "TaskUID";
                DDLSubTask3.DataSource = ds;
                DDLSubTask3.DataBind();
                DDLSubTask3.Items.Insert(0, "--Select--");

                if (ds.Tables[0].Rows.Count > 0)
                {
                    Task4.Visible = true;

                }

            }
        }

        protected void DDLSubTask3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDLSubTask3.SelectedValue != "--Select--")
            {
                 ViewState["SelectedTaskUID"] = DDLSubTask3.SelectedValue;
                 ViewState["SelectedTaskName"] = DDLSubTask3.SelectedItem.Text;
                DataSet ds = getdt.GetSubtoSubtoSubtoSubtoSubTasksForWorkPackages(DDLSubTask3.SelectedValue);
                DDLSubTask4.DataTextField = "Name";
                DDLSubTask4.DataValueField = "TaskUID";
                DDLSubTask4.DataSource = ds;
                DDLSubTask4.DataBind();
                DDLSubTask4.Items.Insert(0, "--Select--");


                if (ds.Tables[0].Rows.Count > 0)
                {
                    Task5.Visible = true;

                }
            }
        }

        protected void DDLSubTask4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDLSubTask4.SelectedValue != "--Select--")
            {

                 ViewState["SelectedTaskUID"] = DDLSubTask4.SelectedValue;
                 ViewState["SelectedTaskName"] = DDLSubTask4.SelectedItem.Text;
                DataSet ds = getdt.GetSubTask_By_ParentTask_Level(DDLSubTask4.SelectedValue, 7);
                DDLSubTask5.DataTextField = "Name";
                DDLSubTask5.DataValueField = "TaskUID";
                DDLSubTask5.DataSource = ds;
                DDLSubTask5.DataBind();
                DDLSubTask5.Items.Insert(0, "--Select--");

                if (ds.Tables[0].Rows.Count > 0)
                {
                    Task6.Visible = true;

                }
            }
        }

        protected void DDLSubTask5_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDLSubTask5.SelectedValue != "--Select--")
            {
                 ViewState["SelectedTaskUID"] = DDLSubTask5.SelectedValue;
                 ViewState["SelectedTaskName"] = DDLSubTask5.SelectedItem.Text;
                DataSet ds = getdt.GetSubTask_By_ParentTask_Level(DDLSubTask5.SelectedValue, 8);
                DDLSubTask6.DataTextField = "Name";
                DDLSubTask6.DataValueField = "TaskUID";
                DDLSubTask6.DataSource = ds;
                DDLSubTask6.DataBind();
                DDLSubTask6.Items.Insert(0, "--Select--");

                if (ds.Tables[0].Rows.Count > 0)
                {
                    Task7.Visible = true;

                }
            }
        }

        protected void DDLSubTask6_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDLSubTask6.SelectedValue != "--Select--")
            {
                 ViewState["SelectedTaskUID"] = DDLSubTask6.SelectedValue;
                 ViewState["SelectedTaskName"] = DDLSubTask6.SelectedItem.Text;
            }
        }

        protected void RBOptionList_SelectedIndexChanged(object sender, EventArgs e)
        {
            LblMessage.Text = string.Empty;
             ViewState["SelectedTaskUID"] = string.Empty;
            btnSubmit.Visible = true;
            btnClear.Visible = true;
            //added on 02/11/2022 for correspondence
            string option = RBOptionList.SelectedValue;
            if (RBOptionList.SelectedIndex == 2)
                option =RBOptionList.Items[1].Value;
            DataSet ds = getdt.GetTasks_by_WorkpackageOptionUID(new Guid(DDLWorkPackage.SelectedValue), new Guid(option));
            DDLMainTask.DataTextField = "Name";
            DDLMainTask.DataValueField = "TaskUID";
            DDLMainTask.DataSource = ds;
            DDLMainTask.DataBind();
            DDLMainTask.Items.Insert(0, "--Select--");
            DocumentGrid.Visible = false;
            if (ds.Tables[0].Rows.Count > 0)
            {
                MainTask.Visible = true;
            }
            //
            if (RBOptionList.SelectedIndex == 2)
            {
                if (!string.IsNullOrEmpty(getdt.GetTaskUID_By_WorkPackageID_TName(new Guid(DDLWorkPackage.SelectedValue), "ONTB/BWSSB Correspondence")))
                {
                    DDLMainTask.SelectedValue = getdt.GetTaskUID_By_WorkPackageID_TName(new Guid(DDLWorkPackage.SelectedValue), "ONTB/BWSSB Correspondence").ToLower();
                    DDLMainTask_SelectedIndexChanged(sender, e);
                    DDLMainTask.Enabled = false;
                }
                
            }
            else
            {
                DDLMainTask.Enabled = true;
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            LblMessage.Text = string.Empty;
            if (!string.IsNullOrEmpty( ViewState["SelectedTaskUID"].ToString()))
            {
                ViewState["IsFolder"] = "No";
                BindActivities();
                BindDocuments( ViewState["SelectedTaskUID"].ToString());
                btnSubmittal.Visible = true;
                DocumentGrid.Visible = true;
            }
            else
            {
                LblMessage.Text = "Select any Task";
                LblMessage.ForeColor = System.Drawing.Color.Red;
            }
            //
            //
            if (Session["TypeOfUser"].ToString() == "NJSD")
            {
                btnSubmittal.Visible = false;
            }
        }

        private void BindActivities()
        {
            AddDocument.Visible = true;

            DataTable dtwkpg = getdt.GetTaskDetails_TaskUID( ViewState["SelectedTaskUID"].ToString());
            if (dtwkpg.Rows.Count > 0)
            {
                ViewState["WkpgUID"] = dtwkpg.Rows[0]["WorkPackageUID"].ToString();
            }

            AddDocument.HRef = "/_modal_pages/add-submittal.aspx?type=add&TaskUID=" +  ViewState["SelectedTaskUID"] + "&ViewDocumentBy=Activity&PrjUID=" + DDlProject.SelectedValue + "&WorkPackageUID=" + ViewState["WkpgUID"].ToString();
            ActivityHeading.Text = "Activity : " +  ViewState["SelectedTaskName"];
            DocumentGrid.Visible = true;
            GrdNewDocument.Visible = true;
            GrdActualDocuments_new.Visible = false;
        }

        private void DbSyncStatusCount(string WorkpackageUID)
        {
            if (WebConfigurationManager.AppSettings["Dbsync"] == "Yes")
            {
                DataSet ds = getdt.GetDbsync_Status_Count_by_WorkPackageUID(new Guid(WorkpackageUID));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DivDocumentsSyncedCount.Visible = true;
                    LblLastSyncedDate.Text = ds.Tables[0].Rows[0]["CreatedDate"].ToString() != "" ? Convert.ToDateTime(ds.Tables[0].Rows[0]["CreatedDate"].ToString()).ToString("dd MMM yyyy hh:mm tt") : "NA";
                    LblTotalSourceDocuments.Text = ds.Tables[0].Rows[0]["DestDocCount"].ToString() != "" ? ds.Tables[0].Rows[0]["DestDocCount"].ToString() : "NA";
                    LblTotalDestinationDocuments.Text = ds.Tables[0].Rows[0]["SourceDocCount"].ToString() != "" ? ds.Tables[0].Rows[0]["SourceDocCount"].ToString() : "NA";

                    //  LblSourceHeading.Text = TreeView1.SelectedNode.Parent.Text + "(" + TreeView1.SelectedNode.Text + ")" + " :- " + WebConfigurationManager.AppSettings["SourceSite"];
                    LblSourceHeading.Text =  ViewState["SelectedTaskName"] + " :- " + WebConfigurationManager.AppSettings["SourceSite"];
                    LblDestinationHeading.Text = WebConfigurationManager.AppSettings["DestinationSite"];
                }
                else
                {
                    DivDocumentsSyncedCount.Visible = false;
                }
            }
            else
            {
                DivDocumentsSyncedCount.Visible = false;
            }
        }

        public void BindDocuments(string UID)
        {
            DataSet ds = new DataSet();
            Session["CopiedActivity"] = UID;
            if (ViewState["IsFolder"].ToString() == "Yes")
            {
                folderNamesAc = ViewState["FoldernameAc"] as Dictionary<string, string>;
                ViewState["folder"] = folderNamesAc[UID].ToString().Replace("&amp;", "&");
                string CheckFile = folderNamesAc[UID].ToString().Replace("&amp;", "&");
                folderNames = Session["Foldername"] as Dictionary<string,string>;
                string NodeParentText = folderNames[UID].ToString().Replace("&amp;", "&");// ViewState["Foldername"].ToString().Replace("&amp;","&");
                //if (!string.IsNullOrEmpty(NodeParentText))
                //{
                //    NodeParentText = NodeParentText.Split('/')[0];
                //}
                ActivityHeading.Text = "Document : " + ViewState["folder"].ToString();
                folderNamesAc.Clear();
                folderNames.Clear();
                ds = new DataSet();
                //if (CheckFile.EndsWith(".pdf") || CheckFile.EndsWith(".PDF") || CheckFile.EndsWith(".doc") || CheckFile.EndsWith(".docx") || CheckFile.EndsWith(".dwg") || CheckFile.EndsWith(".xlsx") || CheckFile.EndsWith(".xls") || CheckFile.EndsWith(".txt") || CheckFile.EndsWith(".pptx") || CheckFile.EndsWith(".log") || CheckFile.EndsWith(".zip"))
                //{
                //    ActivityHeading.Text = "Document : " + TreeView1.SelectedNode.Text;
                //    //ds = getdata.ActualDocuments_SelectBy_DocID_FileName(new Guid(UID), TreeView2.SelectedNode.Text.Split('.')[0]);
                //    ds = getdata.ActualDocuments_SelectBy_ActualDocumentUID(new Guid(UID));
                //}
                //else
                //{
                    ds = getdt.ActualDocuments_SelectBy_DirectoryName_New(new Guid(UID), NodeParentText);

                    //}
                    //ds = getdata.ActualDocuments_SelectBy_DocumentUID(new Guid(UID));
                    GrdActualDocuments_new.DataSource = ds;
                    GrdActualDocuments_new.DataBind();
                    GrdNewDocument.Visible = false;
                    GrdActualDocuments_new.Visible = true;
                    btnSubmittal.Visible = false;


            }
            else
            {
                ds = getdt.getDocumentsForTasks(new Guid(UID));
                GrdNewDocument.DataSource = ds;
                GrdNewDocument.DataBind();
                DocumentDiv.Visible = true;
                DivDocumentsSyncedCount.Visible = false;
                DocumentGrid.Visible = true;
                GrdNewDocument.Visible = true;
            }
            
        }

        protected void GrdNewDocument_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string DocumentUID = GrdNewDocument.DataKeys[e.Row.RowIndex].Values[0].ToString();
                //GridView grdActualDocuments = (GridView)e.Row.FindControl("GrdActualDocuments");

                //DataSet ds = getdata.ActualDocuments_SelectBy_DocumentUID(new Guid(DocumentUID));
                //grdActualDocuments.DataSource = ds;
                //grdActualDocuments.DataBind();

                string Submitter = getdt.GetSubmittal_Submitter_By_DocumentUID(new Guid(DocumentUID));
                if (Submitter != "")
                {

                    if (Session["UserUID"].ToString().ToUpper() != Submitter.ToUpper())
                    {
                        //e.Row.Cells[4].Controls[0].Visible = false;
                        //e.Row.Cells[4].Text = "--";
                        //e.Row.Cells[4].Enabled = false;

                        e.Row.Cells[4].CssClass = "hideItem";
                        GrdNewDocument.HeaderRow.Cells[4].Visible = false;
                        //HtmlAnchor HA = (HtmlAnchor)e.Row.FindControl("AddDoc");
                        //HA.InnerText = "NA";

                    }
                    //added on 16/11/2022 for DTL correspondence/ Project co-ordinator
                    
                   
                        string FlowName = getdt.GetFlowName_by_SubmittalID(new Guid(DocumentUID));
                        if (FlowName == "DTL Correspondence")
                        {
                            DataSet dsMUSers = getdt.GetNextUser_By_SubmittalUID(new Guid(DocumentUID), 7);
                            if (dsMUSers.Tables[0].Rows.Count > 0)
                            {
                                foreach (DataRow druser in dsMUSers.Tables[0].Rows)
                                {
                                    if (Session["UserUID"].ToString().ToUpper() == druser["Approver"].ToString().ToUpper())
                                    {
                                    //e.Row.Cells[4].Controls[0].Visible = false;
                                    //e.Row.Cells[4].Text = "--";
                                    //e.Row.Cells[4].Enabled = false;

                                    e.Row.Cells[4].CssClass = "showItem";
                                    GrdNewDocument.HeaderRow.Cells[4].Visible = true;
                                    //HtmlAnchor HA = (HtmlAnchor)e.Row.FindControl("AddDoc");
                                    //HA.InnerText = "NA";

                                }
                                }
                            }
                        }
                    
                }
                // for db sync checking
                if (WebConfigurationManager.AppSettings["Dbsync"] == "Yes")
                {
                    bool isset = getdt.checkdbsyncflag(new Guid(DocumentUID), "Documents", "DocumentUID");
                    if (isset == true)
                    {
                        e.Row.BackColor = System.Drawing.Color.LightYellow;
                    }
                    else
                    {
                        //  e.Row.BackColor = System.Drawing.Color.Green;
                        // e.Row.ForeColor = System.Drawing.Color.White;
                    }
                }
            }
        }

        protected void GrdNewDocument_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string UID = e.CommandArgument.ToString();
            if (e.CommandName == "download")
            {
                string path = string.Empty;
                DataSet ds = getdt.getLatest_DocumentVerisonSelect(new Guid(UID));
                string docversion = string.Empty;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    path = Server.MapPath(ds.Tables[0].Rows[0]["Doc_FileName"].ToString());
                    docversion = ds.Tables[0].Rows[0]["Doc_Version"].ToString();
                }
                else
                {
                    DataSet ds1 = getdt.getDocumentsbyDocID(new Guid(UID));
                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        if (ds1.Tables[0].Rows[0]["DocPath"].ToString() != "")
                        {
                            path = Server.MapPath(ds1.Tables[0].Rows[0]["DocPath"].ToString());
                        }
                    }
                }
                try
                {
                    byte[] bytes;
                    //
                    //System.IO.FileInfo filenew = new System.IO.FileInfo(path);

                    //if (!filenew.Exists)
                    //{
                    //    using (FileStream fs = File.Create(path))

                    //    { }
                    //}
                    //
                    string filepath = Server.MapPath("~/_PreviewLoad/" + Path.GetFileName(path));
                    //FileInfo fi = new FileInfo(filepath);
                    //string filepath = path;
                    //for blob
                    string Connectionstring = getdt.getProjectConnectionString(new Guid(ds.Tables[0].Rows[0]["ProjectUID"].ToString()));
                   
                    //
                    if (docversion == "1")
                    {
                        DataSet docBlob = getdt.GetActualDocumentBlob_by_ActualDocumentUID(new Guid(UID),Connectionstring);
                        if (docBlob.Tables[0].Rows.Count > 0)
                        {
                            bytes = (byte[])docBlob.Tables[0].Rows[0]["Blob_Data"];

                            BinaryWriter Writer = null;
                            Writer = new BinaryWriter(File.OpenWrite(filepath));

                            // Writer raw data                
                            Writer.Write(bytes);
                            Writer.Flush();
                            Writer.Close();
                        }
                    }
                    else
                    {
                        DataSet docBlob = getdt.GetDocumentVersionBlob_by_DocVersion_UID(new Guid(ds.Tables[0].Rows[0]["DocVersion_UID"].ToString()), Connectionstring);
                        if (docBlob.Tables[0].Rows.Count > 0)
                        {
                            bytes = (byte[])docBlob.Tables[0].Rows[0]["ResubmitFile_Blob"];

                            BinaryWriter Writer = null;
                            Writer = new BinaryWriter(File.OpenWrite(filepath));

                            // Writer raw data                
                            Writer.Write(bytes);
                            Writer.Flush();
                            Writer.Close();
                        }
                    }
                    string getExtension = System.IO.Path.GetExtension(filepath);
                    string outPath = path.Replace(getExtension, "") + "_download" + getExtension;
                    getdt.DecryptFile(filepath, outPath);
                    System.IO.FileInfo file = new System.IO.FileInfo(outPath);

                    if (file.Exists)
                    {

                        Response.Clear();

                        Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);

                        Response.AddHeader("Content-Length", file.Length.ToString());

                        Response.ContentType = "application/octet-stream";

                        Response.WriteFile(file.FullName);

                        Response.Flush();

                        try
                        {
                            if (File.Exists(outPath))
                            {
                                File.Delete(outPath);
                            }
                        }
                        catch (Exception ex)
                        {
                            //throw
                        }

                        Response.End();



                    }
                    else
                    {

                        //Response.Write("This file does not exist.");
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('File does not exists.');</script>");

                    }
                }
                catch (Exception ex)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Document not uploaded for this flow.Please Contact Document controller!');</script>");
                }
            }

            if (e.CommandName == "delete")
            {
                //added on  20/07/2022 for delet submittal ...cechk if any documents are under that
                DataSet dsgr = getdt.ActualDocuments_SelectBy_DocumentUID(new Guid(UID));
                bool Delete = true;
                if (dsgr.Tables[0].Rows.Count > 0)
                {
                    Delete = false;
                }

                if (Delete)
                {
                    int cnt = getdt.Documents_Delete_by_DocID(new Guid(UID), new Guid(Session["UserUID"].ToString()));
                    Response.Redirect("~/_content_pages/documents/?TaskUID=" +  ViewState["SelectedTaskUID"]);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowStartStatus", "javascript:alert('Submittal cannot be deleted as there are documenst under that.Please delete all documnets and try !');", true);

                }
            }
        }

        protected void GrdNewDocument_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdNewDocument.PageIndex = e.NewPageIndex;
            BindDocuments( ViewState["SelectedTaskUID"].ToString());
        }

        protected void GrdNewDocument_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        public string GetDocumentName(string DocumentExtn)
        {
            string retval = getdt.GetDocumentMasterType_by_Extension(DocumentExtn);
            if (retval == null || retval == "")
            {
                if (DocumentExtn.ToLower() == ".jpeg" || DocumentExtn.ToLower() == ".jpg" || DocumentExtn.ToLower() == ".png" || DocumentExtn.ToLower() == ".gif" || DocumentExtn.ToLower() == ".bmp")
                {
                    return "Image";
                }
                else
                {
                    return "N/A";
                }

            }
            else
            {
                return retval;
            }
        }

        public string GetDocumentTypeIcon(string DocumentExtn, string ActualDocumentUID, string dType)
        {
            if (DocumentExtn.ToLower() == ".jpeg" || DocumentExtn.ToLower() == ".jpg" || DocumentExtn.ToLower() == ".png" || DocumentExtn.ToLower() == ".gif" || DocumentExtn.ToLower() == ".bmp")
            {
                string pPath = string.Empty;
                DataSet ds = getdt.ActualDocuments_SelectBy_ActualDocumentUID(new Guid(ActualDocumentUID));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    pPath = ds.Tables[0].Rows[0]["ActualDocument_Path"].ToString().Replace("~", "");
                }
                return pPath;
            }
            else
            {
                return "../../_assets/images/" + getdt.GetDocumentTypeMasterIcon_by_Extension(DocumentExtn);
            }

        }

        protected void Show_Hide_DocumentsGrid(object sender, EventArgs e)
        {
            try
            {
                Page currentPage = (Page)HttpContext.Current.Handler;
                ImageButton imgShowHide = (sender as ImageButton);
                GridViewRow row = (imgShowHide.NamingContainer as GridViewRow);
                if (imgShowHide.CommandArgument == "Show")
                {
                    row.FindControl("pnlDocuments").Visible = true;
                    imgShowHide.CommandArgument = "Hide";
                    imgShowHide.ImageUrl = "~/_assets/images/minus.png";
                    string orderId = (row.NamingContainer as GridView).DataKeys[row.RowIndex].Value.ToString();
                    GridView GrdActualDocuments = row.FindControl("GrdActualDocuments") as GridView;
                    BindActualDocuments(orderId, GrdActualDocuments);
                    ScriptManager.RegisterStartupScript(currentPage, typeof(string), "Script" + Guid.NewGuid(), "expand1();", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(currentPage, typeof(string), "Script" + Guid.NewGuid(), "expand1();", true);
                    row.FindControl("pnlDocuments").Visible = false;
                    imgShowHide.CommandArgument = "Show";
                    imgShowHide.ImageUrl = "~/_assets/images/plus.png";
                }
                //string DocumentUID = GrdNewDocument.DataKeys[row.RowIndex].Values[0].ToString();
                string DocumentUID = (row.NamingContainer as GridView).DataKeys[row.RowIndex].Value.ToString();
                string Submitter = getdt.GetSubmittal_Submitter_By_DocumentUID(new Guid(DocumentUID));
                if (Submitter != "")
                {
                    string sessionuser = Session["UserUID"].ToString();
                    if (sessionuser.ToUpper() != Submitter.ToUpper())
                    {
                        //TableCell cell = row.Cells[4];
                        //row.Cells[4].Enabled = false;
                        row.Cells[4].CssClass = "hideItem";
                    }
                }
            }
            catch (Exception ex)
            {

            }

        }

        public void BindActualDocuments(string DocumentID, GridView GrdActualDocumentsCategory)
        {
            try
            {
                DataSet ds = getdt.ActualDocuments_SelectBy_DocumentUID(new Guid(DocumentID));
                GrdActualDocumentsCategory.DataSource = ds;
                GrdActualDocumentsCategory.DataBind();
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        protected void GrdActualDocuments_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                if (ViewState["isDelete"].ToString() == "false")
                {
                    e.Row.Cells[8].Visible = false;
                }
                if (ViewState["isUpload"].ToString() == "false")
                {
                    e.Row.Cells[6].Visible = false;
                }
                if (Session["TypeOfUser"].ToString() == "NJSD")
                {
                    e.Row.Cells[5].Visible = false;
                    e.Row.Cells[7].Visible = false;
                }

            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // added on 05/11/2020
                try
                {
                    if (Session["TypeOfUser"].ToString() == "NJSD")
                    {
                        e.Row.Cells[5].Visible = false;
                        e.Row.Cells[7].Visible = false;
                    }

                        string ActualDocumentUID = "";
                    if (ViewState["isDelete"].ToString() == "false")
                    {
                        e.Row.Cells[8].Visible = false;
                    }
                    if (ViewState["isUpload"].ToString() == "false")
                    {
                        e.Row.Cells[6].Visible = false;
                    }
                    //-------------------------------------------
                    if (e.Row.Cells[9].Text == "&nbsp;") // for files 
                    {
                        HtmlImage lnkVoucher = e.Row.FindControl("imgfolder") as HtmlImage;
                        lnkVoucher.Visible = false;
                        LinkButton lnkfolder = (LinkButton)e.Row.FindControl("LinkButton2");
                        lnkfolder.Visible = false;
                        if (e.Row.Cells[2].Text != "&nbsp;")
                        {
                            ActualDocumentUID = e.Row.Cells[2].Text;
                            DataSet ds = getdt.getTop1_DocumentStatusSelect(new Guid(e.Row.Cells[2].Text));
                            if (ds != null)
                            {
                                int delay = 0;
                                Label lblVer = (Label)e.Row.FindControl("LblVersion");
                                Label lblDocumentName = (Label)e.Row.FindControl("lblDocumentName");
                                if (ds.Tables[0].Rows[0]["DocumentType"].ToString() == "General Document")
                                {

                                    lblVer.Text = "[ Ver. 1 ]";
                                    e.Row.Cells[2].Text = "Submitted";
                                    e.Row.Cells[4].Text = "No History";
                                }
                                else
                                {
                                    delay = getdt.GetDelayed_Actual_Documents(new Guid(ActualDocumentUID), ds.Tables[0].Rows[0]["ActivityType"].ToString());
                                    e.Row.Cells[2].Text = ds.Tables[0].Rows[0]["ActivityType"].ToString();
                                    if (ds.Tables[0].Rows[0]["ActivityType"].ToString() != "" && ds.Tables[0].Rows[0]["TopVersion"].ToString() != "")
                                    {
                                        //e.Row.Cells[1].Text = ds.Tables[0].Rows[0]["TopVersion"].ToString();

                                        lblVer.Text = "[ Ver. " + ds.Tables[0].Rows[0]["TopVersion"].ToString() + " ]";
                                        string newVersionFileName = Path.GetFileNameWithoutExtension(Server.MapPath(ds.Tables[0].Rows[0]["Doc_Path"].ToString()));
                                        lblDocumentName.Text = newVersionFileName.Substring(0, (newVersionFileName.Length - 2));
                                    }
                                    else
                                    {
                                        lblVer.Text = "[ Ver. 1 ]";
                                    }
                                    //
                                    if (ViewState["isUpdateStatus"].ToString() == "false")
                                    {
                                        e.Row.Cells[4].Text = "";
                                    }
                                }
                                //
                                if (ViewState["isView"].ToString() == "false")
                                {
                                    Label lblName = (Label)e.Row.FindControl("lblName");
                                    e.Row.Cells[0].Text = lblName.Text + " " + lblVer.Text;
                                }
                                if (e.Row.Cells[2].Text == "Code A")
                                {
                                    if (ViewState["isDownloadNJSE"].ToString() == "false")
                                    {
                                        e.Row.Cells[3].Text = "";
                                    }
                                }

                                //
                                if (e.Row.Cells[2].Text == "Client Approved" || e.Row.Cells[2].Text == "Approved" || e.Row.Cells[2].Text == "Client Approve")
                                {
                                    //e.Row.Cells[13].BackColor = System.Drawing.Color.Green;
                                    e.Row.Cells[2].ForeColor = System.Drawing.Color.Green;
                                    e.Row.Cells[2].Font.Bold = true;
                                    if (ViewState["isDownloadClient"].ToString() == "false")
                                    {
                                        e.Row.Cells[3].Text = "";
                                    }
                                }
                                if (delay == 1)
                                {
                                    e.Row.Cells[2].BackColor = System.Drawing.Color.Red;
                                    e.Row.Cells[2].ForeColor = System.Drawing.Color.White;
                                }
                            }
                            //for db sync check
                            if (WebConfigurationManager.AppSettings["Dbsync"] == "Yes")
                            {
                                if (!string.IsNullOrEmpty(ActualDocumentUID))
                                {
                                    if (getdt.checkDocumentsFlagdbsync(new Guid(ActualDocumentUID)) == 1)
                                    {
                                        e.Row.BackColor = System.Drawing.Color.LightYellow;
                                    }
                                    else
                                    {
                                        //  e.Row.BackColor = System.Drawing.Color.Green;
                                        // e.Row.ForeColor = System.Drawing.Color.White;
                                    }
                                }
                            }
                            //--------------------------------------------
                            string SubmittalUID = getdt.GetSubmittalUID_By_ActualDocumentUID(new Guid(ActualDocumentUID));
                            string Flowtype = getdt.GetFlowTypeBySubmittalUID(new Guid(SubmittalUID));
                            string FlowUID = getdt.GetFlowUIDBySubmittalUID(new Guid(SubmittalUID));
                            if (Session["IsContractor"].ToString() == "Y")
                            {
                                
                                if (Flowtype == "STP")
                                {

                                    string phase = getdt.GetPhaseforStatus(new Guid(getdt.GetFlowUIDBySubmittalUID(new Guid(SubmittalUID))), e.Row.Cells[2].Text);
                                    if (string.IsNullOrEmpty(phase))
                                    {
                                        //if (e.Row.Cells[2].Text == "Code A-CE Approval" || e.Row.Cells[2].Text == "Client CE GFC Approval")
                                        //{
                                        //    e.Row.Cells[2].Text = "Approved";

                                        //}
                                        //if (e.Row.Cells[2].Text == "Code B-CE Approval" || e.Row.Cells[3].Text == "Code C-CE Approval")
                                        //{
                                        //    e.Row.Cells[2].Text = "Under Client Approval Process";
                                        //}
                                        //
                                        if (e.Row.Cells[2].Text == "Code A-CE Approval")
                                        {
                                            e.Row.Cells[2].Text = "Approved By BWSSB Under Code A";

                                        }
                                        else if (e.Row.Cells[2].Text == "Code B-CE Approval")
                                        {
                                            e.Row.Cells[2].Text = "Approved By BWSSB Under Code B";
                                        }
                                        else if (e.Row.Cells[2].Text == "Code C-CE Approval")
                                        {
                                            e.Row.Cells[2].Text = "Under Client Approval Process";

                                        }
                                        else if (e.Row.Cells[2].Text == "Client CE GFC Approval")
                                        {
                                            e.Row.Cells[2].Text = "Approved GFC by BWSSB";
                                        }
                                    }
                                    else
                                    {
                                        e.Row.Cells[2].Text = phase;
                                    }
                                }
                                //added on 31/10/2022
                                if (Flowtype == "STP-OB")// ONTB/BWSSB Correspondence show/hide some columns for contractor
                                {
                                    DataSet documentSTatusList = getdt.getActualDocumentStatusList(new Guid(ActualDocumentUID));
                                    if (documentSTatusList.Tables[0].Rows.Count > 0)
                                    {
                                        if (getdt.CheckCCtoUser(new Guid(documentSTatusList.Tables[0].Rows[0]["StatusUID"].ToString()), "Contractor"))
                                        {
                                            e.Row.Cells[0].Enabled = true;
                                            e.Row.Cells[3].Enabled = true;
                                        }
                                        else
                                        {
                                            e.Row.Cells[0].Text = "Access Denied";
                                            e.Row.Cells[3].Text = "Access Denied";

                                        }
                                    }
                                }

                            }
                            else if (Session["IsONTB"].ToString() == "Y")//for DTL and PC
                            {
                                if (Flowtype == "STP-OB")// ONTB/BWSSB Correspondence show/hide some columns for contractor
                                {
                                    if (FlowUID.ToUpper() == "2B8F32F2-3B3A-4F55-837E-D08F8657E945") // DTL Correspondence
                                    {
                                        e.Row.Cells[0].Enabled = true;
                                        e.Row.Cells[3].Enabled = true;
                                    }
                                    else // other  than DTL Correspondence
                                    {
                                        DataSet documentSTatusList = getdt.getActualDocumentStatusList(new Guid(ActualDocumentUID));
                                        if (getdt.GetUserClientType(new Guid(DDLWorkPackage.SelectedValue), Session["UserUID"].ToString()) == "DTL" || getdt.GetUserClientType(new Guid(DDLWorkPackage.SelectedValue), Session["UserUID"].ToString()) == "PC")
                                        {
                                            if (getdt.CheckCCtoUser(new Guid(documentSTatusList.Tables[0].Rows[0]["StatusUID"].ToString()), "DTL"))
                                            {
                                                e.Row.Cells[0].Enabled = true;
                                                e.Row.Cells[3].Enabled = true;
                                            }
                                            else
                                            {
                                                e.Row.Cells[0].Text = "Access Denied";
                                                e.Row.Cells[3].Text = "Access Denied";
                                            }
                                        }
                                        else
                                        {
                                            e.Row.Cells[0].Text = "Access Denied";
                                            e.Row.Cells[3].Text = "Access Denied";
                                        }
                                    }
                                }
                            }
                            else if (Session["IsClient"].ToString() == "Y")//for BWSSB Enginers
                            {
                                if (Flowtype == "STP-OB")// ONTB/BWSSB Correspondence show/hide some columns for contractor
                                {
                                    if (getdt.GetUserClientType(new Guid(DDLWorkPackage.SelectedValue), Session["UserUID"].ToString()) == "EE")
                                    {
                                        if (FlowUID.ToLower() == "267fb2a3-0f45-44ec-aeac-46e7bcaff2ca") // EE Correspondence
                                        {
                                            e.Row.Cells[0].Enabled = true;
                                            e.Row.Cells[3].Enabled = true;
                                        }
                                        else
                                        {
                                            DataSet documentSTatusList = getdt.getActualDocumentStatusList(new Guid(ActualDocumentUID));
                                            if (getdt.CheckCCtoUser(new Guid(documentSTatusList.Tables[0].Rows[0]["StatusUID"].ToString()), "EE"))
                                            {
                                                e.Row.Cells[0].Enabled = true;
                                                e.Row.Cells[3].Enabled = true;
                                            }
                                            else
                                            {
                                                e.Row.Cells[0].Text = "Access Denied";
                                                e.Row.Cells[3].Text = "Access Denied";
                                            }

                                        }
                                    }
                                    else if (getdt.GetUserClientType(new Guid(DDLWorkPackage.SelectedValue), Session["UserUID"].ToString()) == "ACE")
                                    {
                                        if (FlowUID.ToLower() == "abae0151-40a2-43f9-9577-d5211011f479") // ACE Correspondence
                                        {
                                            e.Row.Cells[0].Enabled = true;
                                            e.Row.Cells[3].Enabled = true;
                                        }
                                        else
                                        {
                                            DataSet documentSTatusList = getdt.getActualDocumentStatusList(new Guid(ActualDocumentUID));
                                            if (getdt.CheckCCtoUser(new Guid(documentSTatusList.Tables[0].Rows[0]["StatusUID"].ToString()), "ACE"))
                                            {
                                                e.Row.Cells[0].Enabled = true;
                                                e.Row.Cells[3].Enabled = true;
                                            }
                                            else
                                            {
                                                e.Row.Cells[0].Text = "Access Denied";
                                                e.Row.Cells[3].Text = "Access Denied";
                                            }

                                        }
                                    }
                                    else if (getdt.GetUserClientType(new Guid(DDLWorkPackage.SelectedValue), Session["UserUID"].ToString()) == "CE")
                                    {
                                        if (FlowUID.ToLower() == "c0e51552-35c2-4fbc-8fde-d11ab0fbdf39") // ACE Correspondence
                                        {
                                            e.Row.Cells[0].Enabled = true;
                                            e.Row.Cells[3].Enabled = true;
                                        }
                                        else
                                        {
                                            DataSet documentSTatusList = getdt.getActualDocumentStatusList(new Guid(ActualDocumentUID));
                                            if (getdt.CheckCCtoUser(new Guid(documentSTatusList.Tables[0].Rows[0]["StatusUID"].ToString()), "ACE"))
                                            {
                                                e.Row.Cells[0].Enabled = true;
                                                e.Row.Cells[3].Enabled = true;
                                            }
                                            else
                                            {
                                                e.Row.Cells[0].Text = "Access Denied";
                                                e.Row.Cells[3].Text = "Access Denied";
                                            }

                                        }
                                    }
                                    else if (getdt.GetUserClientType(new Guid(DDLWorkPackage.SelectedValue), Session["UserUID"].ToString()) == "AEE")
                                    {

                                        DataSet documentSTatusList = getdt.getActualDocumentStatusList(new Guid(ActualDocumentUID));
                                        if (getdt.CheckCCtoUser(new Guid(documentSTatusList.Tables[0].Rows[0]["StatusUID"].ToString()), "AEE"))
                                        {
                                            e.Row.Cells[0].Enabled = true;
                                            e.Row.Cells[3].Enabled = true;
                                        }
                                        else
                                        {
                                            e.Row.Cells[0].Text = "Access Denied";
                                            e.Row.Cells[3].Text = "Access Denied";
                                        }


                                    }
                                }
                                //added on 05/12/2022
                                if (e.Row.Cells[6].Text == "Submitted to DTL for ACE" || e.Row.Cells[6].Text == "Submitted to DTL for CE" || e.Row.Cells[6].Text == "Submitted to DTL for EE")
                                {
                                    e.Row.Visible = false;
                                }


                            }


                            //

                        }
                    }
                    else // for folder view
                    {
                        if (folder != e.Row.Cells[9].Text.Split('/')[0])
                        {
                            Label lblVer = (Label)e.Row.FindControl("LblVersion");
                            lblVer.Visible = false;
                            //  HtmlImage lnkVoucher = e.Row.FindControl("imgpdf") as HtmlImage;
                            // lnkVoucher.Visible = false;
                            HtmlGenericControl htmlDivControl = e.Row.FindControl("divPDF") as HtmlGenericControl;
                            htmlDivControl.Visible = false;
                            LinkButton lnkfolder = (LinkButton)e.Row.FindControl("LinkButton2");
                            lnkfolder.Text = e.Row.Cells[9].Text.Split('/')[0];
                            e.Row.Cells[3].Text = "";
                            // e.Row.Cells[0].Text = "<a href='#' onclick='someFunction()'><img src='/_assets/images/folder.jpg' height='15px' width='20px'>  " + e.Row.Cells[7].Text.Split('/')[0] + "</a>";
                            //e.Row.Cells[0].Text = "<asp:LinkButton ID = \"LinkButton3\" runat = \"server\" CausesValidation = \"False\" CommandName = \"Edit\" Text = \"Edit\" ></ asp:LinkButton >";
                            folderNames.Add(e.Row.Cells[2].Text, e.Row.Cells[9].Text.Split('/')[0]);
                            folderNamesAc.Add(e.Row.Cells[2].Text, e.Row.Cells[9].Text.Split('/')[0]);
                            e.Row.Cells[1].Text = "";
                            e.Row.Cells[2].Text = "";
                            e.Row.Cells[4].Text = "";
                            e.Row.Cells[5].Text = "";
                            e.Row.Cells[6].Text = "";
                            e.Row.Cells[7].Text = "";
                            e.Row.Cells[8].Text = "";
                            folder = e.Row.Cells[9].Text.Split('/')[0];
                            ViewState["IsFolder"] = "Yes";
                            Session["Foldername"] =folderNames;
                            ViewState["FoldernameAc"] = folderNamesAc;
                           
                        }
                        else
                        {
                            e.Row.Attributes["style"] = "display:none";
                        }
                    }

                    if (ActualDocumentUID != "")
                    {
                        if (Session["copydocument"] != null)
                        {
                            getdt.sfinallist = (List<CopyDocumentFile>)Session["copydocument"];
                            if (getdt.sfinallist.Any(x => x.DocumentUID == new Guid(ActualDocumentUID)))
                            {
                                LinkButton lnkcopy = (LinkButton)e.Row.FindControl("lnkcopy");
                                lnkcopy.Enabled = false;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        protected void GrdActualDocuments_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string UID = e.CommandArgument.ToString();
            if (e.CommandName == "download")
            {
                string path = string.Empty;
                string filename = string.Empty;
                DataSet ds1 = null;
                DataSet ds = getdt.getLatest_DocumentVerisonSelect(new Guid(UID));
                string docversion = string.Empty;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    path = Server.MapPath(ds.Tables[0].Rows[0]["Doc_FileName"].ToString());
                    //File.Decrypt(path);
                    filename = Path.GetFileName(ds.Tables[0].Rows[0]["Doc_FileName"].ToString());
                    docversion = ds.Tables[0].Rows[0]["Doc_Version"].ToString();
                }
                else
                {
                    ds1 = getdt.ActualDocuments_SelectBy_ActualDocumentUID(new Guid(UID));
                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        if (ds1.Tables[0].Rows[0]["ActualDocument_Path"].ToString() != "")
                        {
                            path = Server.MapPath(ds1.Tables[0].Rows[0]["ActualDocument_Path"].ToString());
                            filename = ds1.Tables[0].Rows[0]["ActualDocument_Name"].ToString() + ds1.Tables[0].Rows[0]["ActualDocument_Type"].ToString();
                        }
                    }
                }
                // added on  20/10/2020
                //ds.Clear();
                //ds = getdata.ActualDocuments_SelectBy_ActualDocumentUID(new Guid(UID));
                //if (ds.Tables[0].Rows.Count > 0)
                //{
                //    if (ds.Tables[0].Rows[0]["ActualDocument_Name"].ToString() != "")
                //    {
                //        filename = ds.Tables[0].Rows[0]["ActualDocument_Name"].ToString() + ds.Tables[0].Rows[0]["ActualDocument_Type"].ToString();
                //    }
                //}
                //
                try
                {
                    byte[] bytes;
                    //
                    //System.IO.FileInfo filenew = new System.IO.FileInfo(path);

                    //if (!filenew.Exists)
                    //{
                    //    using (FileStream fs = File.Create(path))

                    //    { }
                    //}
                    //
                    string filepath = Server.MapPath("~/_PreviewLoad/" + Path.GetFileName(path));
                    //
                    //for blob
                    string Connectionstring = getdt.getProjectConnectionString(new Guid(ds.Tables[0].Rows[0]["ProjectUID"].ToString()));
                    //
                    if (docversion == "1")
                    {
                        DataSet docBlob = getdt.GetActualDocumentBlob_by_ActualDocumentUID(new Guid(UID), Connectionstring);
                        if (docBlob.Tables[0].Rows.Count > 0)
                        {
                            bytes = (byte[])docBlob.Tables[0].Rows[0]["Blob_Data"];

                            BinaryWriter Writer = null;
                            Writer = new BinaryWriter(File.OpenWrite(filepath));

                            // Writer raw data                
                            Writer.Write(bytes);
                            Writer.Flush();
                            Writer.Close();
                        }
                    }
                    else
                    {
                        DataSet docBlob = getdt.GetDocumentVersionBlob_by_DocVersion_UID(new Guid(ds.Tables[0].Rows[0]["DocVersion_UID"].ToString()), Connectionstring);
                        if (docBlob.Tables[0].Rows.Count > 0)
                        {
                            bytes = (byte[])docBlob.Tables[0].Rows[0]["ResubmitFile_Blob"];

                            BinaryWriter Writer = null;
                            Writer = new BinaryWriter(File.OpenWrite(filepath));

                            // Writer raw data                
                            Writer.Write(bytes);
                            Writer.Flush();
                            Writer.Close();
                        }
                    }

                    string getExtension = System.IO.Path.GetExtension(filepath);
                    string outPath = path.Replace(getExtension, "") + "_download" + getExtension;
                    getdt.DecryptFile(filepath, outPath);
                    System.IO.FileInfo file = new System.IO.FileInfo(outPath);

                    if (file.Exists)
                    {
                        int Cnt = getdt.DocumentHistroy_InsertorUpdate(Guid.NewGuid(), new Guid(UID), new Guid(Session["UserUID"].ToString()), "Downloaded", "Documents");
                        if (Cnt <= 0)
                        {
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code: DDH-01. there is a problem with updating histroy. Please contact system admin.');</script>");
                        }
                        Response.Clear();

                        // Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
                        Response.AddHeader("Content-Disposition", "attachment; filename=" + filename);
                        Response.AddHeader("Content-Length", file.Length.ToString());

                        Response.ContentType = "application/octet-stream";

                        Response.WriteFile(file.FullName);

                        Response.Flush();

                        try
                        {
                            if (File.Exists(outPath))
                            {
                                File.Delete(outPath);
                            }
                        }
                        catch (Exception ex)
                        {
                            //throw
                        }

                        Response.End();


                    }
                    else
                    {

                        //Response.Write("This file does not exist.");
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('File does not exists.');</script>");

                    }
                }
                catch (Exception ex)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Document not uploaded for this flow.Please Contact Document controller!');</script>");
                }
            }

            if (e.CommandName == "delete")
            {
                //
                DataSet dsstatus = getdt.getTop1_DocumentStatusSelect(new Guid(UID));
                bool Delete = true;
                if (dsstatus.Tables[0].Rows.Count > 0)
                {
                    if (Session["TypeOfUser"].ToString() != "U")
                    {
                        if (dsstatus.Tables[0].Rows[0]["ActivityType"].ToString() == "Submitted" || dsstatus.Tables[0].Rows[0]["ActivityType"].ToString() == "Reconciliation")
                        {
                            Delete = true;
                        }
                        else
                        {
                            Delete = false;
                        }
                    }

                }
                //
                if (Delete)
                {
                    int cnt = getdt.ActualDocuments_Delete_by_DocID(new Guid(UID), new Guid(Session["UserUID"].ToString()));
                    if (cnt > 0)
                    {
                        GridViewRow gvRowParent = (((GridView)sender)).DataItemContainer as GridViewRow;
                        string orderId = (gvRowParent.NamingContainer as GridView).DataKeys[gvRowParent.RowIndex].Value.ToString();
                        GridView gvrdChild = ((GridView)sender);
                        BindActualDocuments(orderId, gvrdChild);
                        //
                        //GridView gvwChild = ((GridView)sender);
                        //BindDocuments(TreeView1.SelectedNode.Value);
                        //gvRowParent.FindControl("pnlDocuments").Visible = true;
                        //imgShowHide.CommandArgument = "Hide";
                        //imgShowHide.ImageUrl = "~/_assets/images/minus.png";

                        //ImageButton imgbtn = gvRowParent.FindControl("pnlDocuments") as ImageButton;
                        //GridViewRow row = (sender as LinkButton).NamingContainer as GridViewRow;
                        // string id = "imgProductsShow";
                        //string script = @"$(document).ready(function () {$('#" + id + "').trigger('click');});";
                        ScriptManager.RegisterClientScriptBlock(sender as Control, this.GetType(), "alert", "expand1()", true);


                    }
                }
                else
                {
                    GridViewRow gvRowParent = (((GridView)sender)).DataItemContainer as GridViewRow;
                    string orderId = (gvRowParent.NamingContainer as GridView).DataKeys[gvRowParent.RowIndex].Value.ToString();
                    GridView gvrdChild = ((GridView)sender);
                    BindActualDocuments(orderId, gvrdChild);
                    
                    ScriptManager.RegisterClientScriptBlock(sender as Control, this.GetType(), "alert", "expand1()", true);
                    //ScriptManager.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('File does not exists.');</script>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowStartStatus", "javascript:alert('File cannot be deleted ! It is already in action');", true);

                }
            }
            if (e.CommandName == "ViewDoc")
            {
                string FilePath = Server.MapPath(UID);
                if (File.Exists(FilePath))
                {
                    string getExtension = System.IO.Path.GetExtension(FilePath);
                    string outPath = FilePath.Replace(getExtension, "") + "_download" + getExtension;
                    getdt.DecryptFile(FilePath, outPath);
                    //Response.Write("<script>window.open('" + outPath + "','_blank')</script>");
                    WebClient User = new WebClient();
                    Byte[] FileBuffer = User.DownloadData(outPath);
                    if (FileBuffer != null)
                    {
                        Response.ContentType = "application/pdf";
                        Response.AddHeader("content-length", FileBuffer.Length.ToString());
                        Response.BinaryWrite(FileBuffer);
                    }
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('File not found');</script>");
                }

            }
            else if (e.CommandName == "Folder_View") // this is for maintaining the folder structure....
            {
                BindDocuments(UID);
            }

            if (e.CommandName == "copyfile")
            {
                LinkButton ctrl = e.CommandSource as LinkButton;
                if (ctrl != null)
                {
                    GridViewRow row = ctrl.Parent.NamingContainer as GridViewRow;
                    LinkButton lnkcopy = (LinkButton)row.FindControl("lnkcopy");
                    lnkcopy.Enabled = false;
                }
            }
        }

        protected void GrdActualDocuments_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
        }

        protected void GrdActualDocuments_new_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (Session["TypeOfUser"].ToString() == "NJSD")
                    {
                      
                       e.Row.Cells[5].Visible = false;
                       e.Row.Cells[7].Visible = false;
                        e.Row.Cells[8].Visible = false;
                    }

                    string ActualDocumentUID = "";
                    if (ViewState["folder"].ToString().EndsWith(".pdf") || ViewState["folder"].ToString().EndsWith(".PDF") || ViewState["folder"].ToString().EndsWith(".doc") || ViewState["folder"].ToString().EndsWith(".docx") || ViewState["folder"].ToString().EndsWith(".dwg") || ViewState["folder"].ToString().EndsWith(".xlsx") || ViewState["folder"].ToString().EndsWith(".xls") || ViewState["folder"].ToString().EndsWith(".txt") || ViewState["folder"].ToString().EndsWith(".pptx") || ViewState["folder"].ToString().EndsWith(".log") || ViewState["folder"].ToString().EndsWith(".zip") || ViewState["folder"].ToString().EndsWith(".bak"))
                    {
                        if (e.Row.Cells[2].Text != "&nbsp;")
                        {
                            ActualDocumentUID = e.Row.Cells[2].Text;
                            DataSet ds = getdt.getTop1_DocumentStatusSelect(new Guid(e.Row.Cells[2].Text));
                            if (ds != null)
                            {
                                int delay = 0;
                                Label lblVer = (Label)e.Row.FindControl("LblVersion");
                                Label lblDocumentName = (Label)e.Row.FindControl("lblDocumentName");
                                if (ds.Tables[0].Rows[0]["DocumentType"].ToString() == "General Document")
                                {

                                    lblVer.Text = "[ Ver. 1 ]";
                                    e.Row.Cells[2].Text = "Submitted";
                                    e.Row.Cells[4].Text = "No History";
                                }
                                else
                                {
                                    delay = getdt.GetDelayed_Actual_Documents(new Guid(ActualDocumentUID), ds.Tables[0].Rows[0]["ActivityType"].ToString());
                                    e.Row.Cells[2].Text = ds.Tables[0].Rows[0]["ActivityType"].ToString();
                                    if (ds.Tables[0].Rows[0]["ActivityType"].ToString() != "" && ds.Tables[0].Rows[0]["TopVersion"].ToString() != "")
                                    {
                                        //e.Row.Cells[1].Text = ds.Tables[0].Rows[0]["TopVersion"].ToString();

                                        lblVer.Text = "[ Ver. " + ds.Tables[0].Rows[0]["TopVersion"].ToString() + " ]";
                                        string newVersionFileName = Path.GetFileNameWithoutExtension(Server.MapPath(ds.Tables[0].Rows[0]["Doc_Path"].ToString()));
                                        lblDocumentName.Text = newVersionFileName.Substring(0, (newVersionFileName.Length - 2));
                                    }
                                    else
                                    {
                                        lblVer.Text = "[ Ver. 1 ]";
                                    }
                                    if (ViewState["isUpdateStatus"].ToString() == "false")
                                    {
                                        e.Row.Cells[4].Text = "";
                                    }
                                }
                                //
                                if (ViewState["isView"].ToString() == "false")
                                {
                                    Label lblName = (Label)e.Row.FindControl("lblName");
                                    e.Row.Cells[0].Text = lblName.Text + " " + lblVer.Text;
                                }
                                if (e.Row.Cells[2].Text == "Code A")
                                {
                                    if (ViewState["isDownloadNJSE"].ToString() == "false")
                                    {
                                        e.Row.Cells[3].Text = "";
                                    }
                                }
                                //
                                if (e.Row.Cells[2].Text == "Client Approved" || e.Row.Cells[2].Text == "Approved" || e.Row.Cells[2].Text == "Client Approve")
                                {
                                    //e.Row.Cells[13].BackColor = System.Drawing.Color.Green;
                                    e.Row.Cells[2].ForeColor = System.Drawing.Color.Green;
                                    e.Row.Cells[2].Font.Bold = true;
                                    if (ViewState["isDownloadClient"].ToString() == "false")
                                    {
                                        e.Row.Cells[3].Text = "";
                                    }
                                }
                                if (delay == 1)
                                {
                                    e.Row.Cells[2].BackColor = System.Drawing.Color.Red;
                                    e.Row.Cells[2].ForeColor = System.Drawing.Color.White;
                                }
                            }

                        }

                        HtmlImage lnkVoucher = e.Row.FindControl("imgfolder") as HtmlImage;
                        lnkVoucher.Visible = false;
                        LinkButton lnkfolder = (LinkButton)e.Row.FindControl("LinkButton2");
                        lnkfolder.Visible = false;



                    }
                    else
                    {// added for folder structure on 22/10/2020



                        string[] foldernames = e.Row.Cells[9].Text.Split('/');
                        int index = 0;
                        int correctindex = 0;
                        string foldername = "";
                        foreach (string str in foldernames)
                        {
                            foldername = str.Trim().Replace("amp;", "");
                            if (foldername == ViewState["folder"].ToString().Trim())
                            {
                                correctindex = index + 1;
                            }
                            index = index + 1;
                        }
                        if (folder != e.Row.Cells[9].Text.Split('/')[correctindex].Trim().Replace("&amp;", "&"))
                        {
                            if (e.Row.Cells[10].Text.Trim().Replace("amp;", "") == ViewState["folder"].ToString())
                            {
                                if (e.Row.Cells[2].Text != "&nbsp;")
                                {
                                    ActualDocumentUID = e.Row.Cells[2].Text;
                                    DataSet ds = getdt.getTop1_DocumentStatusSelect(new Guid(e.Row.Cells[2].Text));
                                    if (ds != null)
                                    {
                                        int delay = 0;
                                        Label lblVer = (Label)e.Row.FindControl("LblVersion");
                                        Label lblDocumentName = (Label)e.Row.FindControl("lblDocumentName");
                                        if (ds.Tables[0].Rows[0]["DocumentType"].ToString() == "General Document")
                                        {

                                            lblVer.Text = "[ Ver. 1 ]";
                                            e.Row.Cells[2].Text = "Submitted";
                                            e.Row.Cells[4].Text = "No History";
                                        }
                                        else
                                        {
                                            delay = getdt.GetDelayed_Actual_Documents(new Guid(ActualDocumentUID), ds.Tables[0].Rows[0]["ActivityType"].ToString());
                                            e.Row.Cells[2].Text = ds.Tables[0].Rows[0]["ActivityType"].ToString();
                                            if (ds.Tables[0].Rows[0]["ActivityType"].ToString() != "" && ds.Tables[0].Rows[0]["TopVersion"].ToString() != "")
                                            {
                                                //e.Row.Cells[1].Text = ds.Tables[0].Rows[0]["TopVersion"].ToString();

                                                lblVer.Text = "[ Ver. " + ds.Tables[0].Rows[0]["TopVersion"].ToString() + " ]";
                                                string newVersionFileName = Path.GetFileNameWithoutExtension(Server.MapPath(ds.Tables[0].Rows[0]["Doc_Path"].ToString()));
                                                lblDocumentName.Text = newVersionFileName.Substring(0, (newVersionFileName.Length - 2));
                                            }
                                            else
                                            {
                                                lblVer.Text = "[ Ver. 1 ]";
                                            }
                                            if (ViewState["isUpdateStatus"].ToString() == "false")
                                            {
                                                e.Row.Cells[4].Text = "";
                                            }
                                        }
                                        //
                                        if (ViewState["isView"].ToString() == "false")
                                        {
                                            Label lblName = (Label)e.Row.FindControl("lblName");
                                            e.Row.Cells[0].Text = lblName.Text + " " + lblVer.Text;
                                        }
                                        if (e.Row.Cells[2].Text == "Code A")
                                        {
                                            if (ViewState["isDownloadNJSE"].ToString() == "false")
                                            {
                                                e.Row.Cells[3].Text = "";
                                            }
                                        }
                                        //
                                        if (e.Row.Cells[2].Text == "BWSSB Approved" || e.Row.Cells[2].Text == "Approved" || e.Row.Cells[2].Text == "BWSSB Approve")
                                        {
                                            //e.Row.Cells[13].BackColor = System.Drawing.Color.Green;
                                            e.Row.Cells[2].ForeColor = System.Drawing.Color.Green;
                                            e.Row.Cells[2].Font.Bold = true;
                                            if (ViewState["isDownloadClient"].ToString() == "false")
                                            {
                                                e.Row.Cells[3].Text = "";
                                            }
                                        }
                                        if (delay == 1)
                                        {
                                            e.Row.Cells[2].BackColor = System.Drawing.Color.Red;
                                            e.Row.Cells[2].ForeColor = System.Drawing.Color.White;
                                        }
                                    }

                                }

                                HtmlImage lnkVoucher = e.Row.FindControl("imgfolder") as HtmlImage;
                                lnkVoucher.Visible = false;
                                LinkButton lnkfolder = (LinkButton)e.Row.FindControl("LinkButton2");
                                lnkfolder.Visible = false;

                                //for db sync check
                                if (WebConfigurationManager.AppSettings["Dbsync"] == "Yes")
                                {
                                    if (!string.IsNullOrEmpty(ActualDocumentUID))
                                    {
                                        if (getdt.checkDocumentsFlagdbsync(new Guid(ActualDocumentUID)) == 1)
                                        {
                                            e.Row.BackColor = System.Drawing.Color.LightYellow;
                                        }
                                        else
                                        {
                                            //  e.Row.BackColor = System.Drawing.Color.Green;
                                            // e.Row.ForeColor = System.Drawing.Color.White;
                                        }
                                    }
                                }
                                //--------------------------------------------
                            }
                            else
                            {
                                //e.Row.Attributes["style"] = "display:none";
                                if (correctindex != 0)
                                {
                                    if ((e.Row.Cells[9].Text.Trim().Replace("amp;", "").Contains(ViewState["folder"].ToString())))
                                    {
                                        HtmlGenericControl htmlDivControl = e.Row.FindControl("divPDF") as HtmlGenericControl;
                                        htmlDivControl.Visible = false;
                                        LinkButton lnkfolder = (LinkButton)e.Row.FindControl("LinkButton2");
                                        lnkfolder.Text = e.Row.Cells[9].Text.Split('/')[correctindex];
                                        e.Row.Cells[3].Text = "";
                                        // e.Row.Cells[0].Text = "<a href='#' onclick='someFunction()'><img src='/_assets/images/folder.jpg' height='15px' width='20px'>  " + e.Row.Cells[7].Text.Split('/')[0] + "</a>";
                                        //e.Row.Cells[0].Text = "<asp:LinkButton ID = \"LinkButton3\" runat = \"server\" CausesValidation = \"False\" CommandName = \"Edit\" Text = \"Edit\" ></ asp:LinkButton >";
                                        folderNamesAc.Add(e.Row.Cells[2].Text, e.Row.Cells[9].Text.Trim().Split('/')[correctindex].Replace("amp;", ""));
                                        folderNames.Add(e.Row.Cells[2].Text, e.Row.Cells[9].Text.Split('/')[0]);
                                        e.Row.Cells[1].Text = "";
                                        e.Row.Cells[2].Text = "";
                                        e.Row.Cells[4].Text = "";
                                        e.Row.Cells[5].Text = "";
                                        e.Row.Cells[6].Text = "";
                                        e.Row.Cells[7].Text = "";
                                        e.Row.Cells[8].Text = "";
                                        folder = e.Row.Cells[9].Text.Trim().Split('/')[correctindex].Replace("amp;", "");
                                        ViewState["IsFolder"] = "Yes";
                                        ViewState["FoldernameAc"] = folderNamesAc;
                                        Session["Foldername"] = folderNames;
                                    }
                                    else
                                    {
                                        // e.Row.Attributes["style"] = "display:none";
                                        e.Row.Visible = false;
                                    }
                                }
                                else
                                {
                                    e.Row.Visible = false;
                                }
                            }
                        }
                        else
                        {
                            e.Row.Attributes["style"] = "display:none";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message;
            }
        }

        protected void GrdActualDocuments_new_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string filename = string.Empty;
            DataSet ds1 = null;
            string UID = e.CommandArgument.ToString();
            // e.CommandArgument.ToString();
            if (e.CommandName == "download")
            {
                string path = string.Empty;
                DataSet ds = getdt.getLatest_DocumentVerisonSelect(new Guid(UID));
                string docversion = string.Empty;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    path = Server.MapPath(ds.Tables[0].Rows[0]["Doc_FileName"].ToString());
                    //File.Decrypt(path);
                    filename = Path.GetFileName(ds.Tables[0].Rows[0]["Doc_FileName"].ToString());
                    docversion = ds.Tables[0].Rows[0]["Doc_Version"].ToString();
                }
                else
                {
                    ds1 = getdt.ActualDocuments_SelectBy_ActualDocumentUID(new Guid(UID));
                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        if (ds1.Tables[0].Rows[0]["ActualDocument_Path"].ToString() != "")
                        {
                            path = Server.MapPath(ds1.Tables[0].Rows[0]["ActualDocument_Path"].ToString());
                            filename = ds1.Tables[0].Rows[0]["ActualDocument_Name"].ToString() + ds1.Tables[0].Rows[0]["ActualDocument_Type"].ToString();
                        }
                    }
                }
                
                try
                {
                    byte[] bytes;
                   
                    string filepath = Server.MapPath("~/_PreviewLoad/" + filename);
                    //for blob
                    string Connectionstring = getdt.getProjectConnectionString(new Guid(ds.Tables[0].Rows[0]["ProjectUID"].ToString()));
                    //
                    if (docversion == "1")
                    {
                        DataSet docBlob = getdt.GetActualDocumentBlob_by_ActualDocumentUID(new Guid(UID), Connectionstring);
                        if (docBlob.Tables[0].Rows.Count > 0)
                        {
                            bytes = (byte[])docBlob.Tables[0].Rows[0]["Blob_Data"];

                            BinaryWriter Writer = null;
                            Writer = new BinaryWriter(File.OpenWrite(filepath));

                            // Writer raw data                
                            Writer.Write(bytes);
                            Writer.Flush();
                            Writer.Close();
                        }
                    }
                    else
                    {
                        DataSet docBlob = getdt.GetDocumentVersionBlob_by_DocVersion_UID(new Guid(ds.Tables[0].Rows[0]["DocVersion_UID"].ToString()), Connectionstring);
                        if (docBlob.Tables[0].Rows.Count > 0)
                        {
                            bytes = (byte[])docBlob.Tables[0].Rows[0]["ResubmitFile_Blob"];

                            BinaryWriter Writer = null;
                            Writer = new BinaryWriter(File.OpenWrite(filepath));

                            // Writer raw data                
                            Writer.Write(bytes);
                            Writer.Flush();
                            Writer.Close();
                        }
                    }

                    string getExtension = System.IO.Path.GetExtension(filepath);
                    string outPath = path.Replace(getExtension, "") + "_download" + getExtension;
                    getdt.DecryptFile(path, outPath);
                    System.IO.FileInfo file = new System.IO.FileInfo(outPath);

                    if (file.Exists)
                    {
                        int Cnt = getdt.DocumentHistroy_InsertorUpdate(Guid.NewGuid(), new Guid(UID), new Guid(Session["UserUID"].ToString()), "Downloaded", "Documents");
                        if (Cnt <= 0)
                        {
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code: DDH-01. there is a problem with updating histroy. Please contact system admin.');</script>");
                        }
                        Response.Clear();

                        Response.AddHeader("Content-Disposition", "attachment; filename=" + filename);

                        Response.AddHeader("Content-Length", file.Length.ToString());

                        Response.ContentType = "application/octet-stream";

                        Response.WriteFile(file.FullName);

                        Response.Flush();

                        try
                        {
                            if (File.Exists(outPath))
                            {
                                File.Delete(outPath);
                            }
                        }
                        catch (Exception ex)
                        {
                            //throw
                        }

                        Response.End();



                    }
                    else
                    {

                        //Response.Write("This file does not exist.");
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('File does not exists.');</script>");

                    }
                }
                catch (Exception ex)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Document not uploaded for this flow.Please Contact Document controller!');</script>");
                }
            }

            if (e.CommandName == "delete")
            {
                DataSet dsstatus = getdt.getTop1_DocumentStatusSelect(new Guid(UID));
                bool Delete = true;
                if (dsstatus.Tables[0].Rows.Count > 0)
                {
                    if (Session["TypeOfUser"].ToString() != "U")
                    {
                        if (dsstatus.Tables[0].Rows[0]["ActivityType"].ToString() == "Submitted" || dsstatus.Tables[0].Rows[0]["ActivityType"].ToString() == "Reconciliation")
                        {
                            Delete = true;
                        }
                        else
                        {
                            Delete = false;
                        }
                    }

                }
                //
                if (Delete)
                {
                    int cnt = getdt.ActualDocuments_Delete_by_DocID(new Guid(UID), new Guid(Session["UserUID"].ToString()));
                    if (cnt > 0)
                    {
                        BindDocuments( ViewState["SelectedTaskUID"].ToString());

                        //GridViewRow gvRowParent = (((GridView)sender)).DataItemContainer as GridViewRow;
                        //string orderId = (gvRowParent.NamingContainer as GridView).DataKeys[gvRowParent.RowIndex].Value.ToString();
                        //GridView gvrdChild = ((GridView)sender);
                        //BindActualDocuments(TreeView1.SelectedNode.Value, gvrdChild);

                    }
                }
                else
                {
                    BindDocuments( ViewState["SelectedTaskUID"].ToString());
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowStartStatus", "javascript:alert('File cannot be deleted ! It is already in action');", true);

                }
            }
            if (e.CommandName == "ViewDoc")
            {
                string FilePath = Server.MapPath(UID);
                if (File.Exists(FilePath))
                {
                    string getExtension = System.IO.Path.GetExtension(FilePath);
                    string outPath = FilePath.Replace(getExtension, "") + "_download" + getExtension;
                    getdt.DecryptFile(FilePath, outPath);
                    //Response.Write("<script>window.open('" + outPath + "','_blank')</script>");
                    WebClient User = new WebClient();
                    Byte[] FileBuffer = User.DownloadData(outPath);
                    if (FileBuffer != null)
                    {
                        Response.ContentType = "application/pdf";
                        Response.AddHeader("content-length", FileBuffer.Length.ToString());
                        Response.BinaryWrite(FileBuffer);
                    }
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('File not found');</script>");
                }

            }
            else if (e.CommandName == "Folder_View") // this is for maintaining the folder structure....
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                // string name = GrdActualDocuments_new.Rows[rowIndex].Cells[0].Text;
                GridViewRow row = GrdActualDocuments_new.Rows[rowIndex];
                LinkButton lnkfolder = (LinkButton)row.FindControl("LinkButton2");
                string name = lnkfolder.Text.Trim().Replace("amp;", "");
                UID = row.Cells[11].Text;
                
                BindDocuments(UID);
                //string folder = 
            }

            if (e.CommandName == "copyfile")
            {
                LinkButton ctrl = e.CommandSource as LinkButton;
                if (ctrl != null)
                {
                    GridViewRow row = ctrl.Parent.NamingContainer as GridViewRow;
                    LinkButton lnkcopy = (LinkButton)row.FindControl("lnkcopy1");
                    lnkcopy.Enabled = false;
                }
            }
        }

        protected void GrdActualDocuments_new_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        public string GetStatus(string sStatus)
        {
            if (sStatus == "P")
            {
                return "Not Started";
            }
            else if (sStatus == "I")
            {
                return "In-Progress";
            }
            else
            {
                return "Completed";
            }
        }


        public string LimitCharts(string Desc)
        {
            if (Desc.Length > 100)
            {
                return Desc.Substring(0, 100) + "  . . .";
            }
            else
            {
                return Desc;
            }
        }

        public string ShoworHide(string Desc)
        {
            if (Desc.Length > 50)
            {
                return "More";
            }
            else
            {
                return string.Empty;
            }
        }
        public string getCategoryName(string CategoryUID)
        {
            return getdt.GetWorkpackageCategory_By_UID(new Guid(CategoryUID));
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            Response.Redirect("default.aspx");
        }
    }
}