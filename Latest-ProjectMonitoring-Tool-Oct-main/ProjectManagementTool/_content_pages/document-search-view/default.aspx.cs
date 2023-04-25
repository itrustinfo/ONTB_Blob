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
using ProjectManagementTool.DAL;

namespace ProjectManagementTool._content_pages.document_search_view
{
    public partial class _default : System.Web.UI.Page
    {
        DBGetData getdt = new DBGetData();
        TaskUpdate gettk = new TaskUpdate();
        private static string SelectedTaskUID = string.Empty;
        private static string SelectedTaskName = string.Empty;
        public int SearchResultCount = 0;
        Dictionary<string, string> folderNames = new Dictionary<string, string>();
        Dictionary<string, string> folderNamesAc = new Dictionary<string, string>();
        string folder = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Username"] == null)
            {
                Response.Redirect("~/Login.aspx");
            }
            if (!IsPostBack)
            {
                BindProject();
               // SelectedProjectWorkpackage("Project");
                DDlProject_SelectedIndexChanged(sender, e);
               // PageLoadItemsBind();
                SelectedTaskUID = string.Empty;
                              
                //
                //if (Session["IsClient"].ToString() == "Y" || Session["UserID"].ToString() == "ns.rao04@gmail.com")
                //{
                //    RBOptionList.Items[2].Selected = true;
                //    RBOptionList_SelectedIndexChanged(sender, e);
                //    btnSubmit_Click(sender, e);
                //    divoption.Visible = false;
                //    MainTask.Visible = false;
                //    divsubmit.Visible = false;
                //}
                //else if(Session["TypeOfUser"].ToString() == "DDE" || Session["TypeOfUser"].ToString() =="DE")
                //{
                //    if(RBOptionList.Items.Count == 3)
                //         RBOptionList.Items.RemoveAt(2);
                //}
                //
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
                    SelectedProjectWorkpackage("Workpackage");
                    Session["Project_Workpackage"] = DDlProject.SelectedValue + "_" + DDLWorkPackage.SelectedValue;
                    //BindMainTask(DDLWorkPackage.SelectedValue);
                    OptionBind(DDLWorkPackage.SelectedValue);
                    //
                    SelectedTaskUID = string.Empty;
                   
                    
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
                    divoption.Visible = false;
                    MainTask.Visible = false;
                   
                }
                else if (Session["TypeOfUser"].ToString() == "DDE" || Session["TypeOfUser"].ToString() == "DE")
                {
                    RBOptionList.Items.RemoveAt(2);
                }

                DDLWorkPackage_SelectedIndexChanged(sender, e);
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

            if (DDLWorkPackage.SelectedValue != "-Select-")
            {
                if (Session["IsContractor"].ToString() == "Y")
                {
                    var IsProjFound = Constants.ProjectsForPhaseSearch.Where(r => r == DDlProject.SelectedItem.Text).FirstOrDefault();

                    if (!string.IsNullOrEmpty(IsProjFound))
                    {
                        ddlstatus.Attributes["style"] = "display: none;";
                        ddlPhase.Attributes["style"] = "";
                        status.Text = "Phase";
                        BindPhases(new Guid(DDLWorkPackage.SelectedValue));
                    }
                    else
                    {
                        ddlPhase.Attributes["style"] = "display: none;";
                        ddlstatus.Attributes["style"] = "";
                        status.Text = "Status";
                    }
                }
                else
                {
                    ddlPhase.Attributes["style"] = "display: none;";
                    ddlstatus.Attributes["style"] = "";
                    status.Text = "Status";
                }

                UpdatePanel2.Update();
                ddlstatus.DataSource = null;
                ddlstatus.DataBind();
                //
                ddlType.DataSource = null;
                ddlType.DataBind();
                lbldocNos.Text = "0";
                divMain.Visible = true;
                divGeneral.Visible = false;
                divWP.Visible = true;
                GrdDocuments.Visible = false;


                BindStatus();
                //bind phase

                BindType();
                BindFlow();


                if (Session["searchedit"] != null)
                {
                    ddlstatus.SelectedValue = Session["sStatus"].ToString();
                    ddlType.SelectedValue = Session["sType"].ToString();
                }
                Session["Project_Workpackage"] = DDlProject.SelectedValue + "_" + DDLWorkPackage.SelectedValue;
                // BindDocuments();
            }
            else
            {
                UpdatePanel2.Update();
                GrdDocuments.Visible = false;
                lbldocNos.Text = "0";
                ddlstatus.DataSource = null;
                ddlstatus.DataBind();
                //
                ddlType.DataSource = null;
                ddlType.DataBind();
            }
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
                SelectedTaskUID = DDLMainTask.SelectedValue;
                SelectedTaskName = DDLMainTask.SelectedItem.Text;
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
                SelectedTaskUID = DDLSubTask.SelectedValue;
                SelectedTaskName = DDLSubTask.SelectedItem.Text;
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
                SelectedTaskUID = DDLSubTask1.SelectedValue;
                SelectedTaskName = DDLSubTask1.SelectedItem.Text;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    Task3.Visible = true;
                    
                }
                
            }
        }

        protected void DDLSubTask2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDLSubTask2.SelectedValue != "--Select--")
            {
                SelectedTaskUID = DDLSubTask2.SelectedValue;
                SelectedTaskName = DDLSubTask2.SelectedItem.Text;
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
                SelectedTaskUID = DDLSubTask3.SelectedValue;
                SelectedTaskName = DDLSubTask3.SelectedItem.Text;
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

                SelectedTaskUID = DDLSubTask4.SelectedValue;
                SelectedTaskName = DDLSubTask4.SelectedItem.Text;
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
                SelectedTaskUID = DDLSubTask5.SelectedValue;
                SelectedTaskName = DDLSubTask5.SelectedItem.Text;
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
                SelectedTaskUID = DDLSubTask6.SelectedValue;
                SelectedTaskName = DDLSubTask6.SelectedItem.Text;
            }
        }

        private void ClearAllSubTasksForward(int task_count)
        {
            if (task_count == 0)
            {
                DDLSubTask.Items.Clear();
                DDLSubTask1.Items.Clear();
                DDLSubTask2.Items.Clear();
                DDLSubTask3.Items.Clear();
                DDLSubTask4.Items.Clear();
                DDLSubTask5.Items.Clear();
                DDLSubTask6.Items.Clear();

                Task1.Visible = false;
                Task2.Visible = false;
                Task3.Visible = false;
                Task4.Visible = false;
                Task5.Visible = false;
                Task6.Visible = false;
                Task7.Visible = false;

                SelectedTaskUID = "";
                SelectedTaskName = "";
            }
            else if (task_count == 1)
            {

                DDLSubTask1.Items.Clear();
                DDLSubTask2.Items.Clear();
                DDLSubTask3.Items.Clear();
                DDLSubTask4.Items.Clear();
                DDLSubTask5.Items.Clear();
                DDLSubTask6.Items.Clear();

                Task1.Visible = false;
                Task2.Visible = false;
                Task3.Visible = false;
                Task4.Visible = false;
                Task5.Visible = false;
                Task6.Visible = false;
                Task7.Visible = false;
            }
            else if (task_count == 2)
            {

                DDLSubTask2.Items.Clear();
                DDLSubTask3.Items.Clear();
                DDLSubTask4.Items.Clear();
                DDLSubTask5.Items.Clear();
                DDLSubTask6.Items.Clear();

                Task2.Visible = false;
                Task3.Visible = false;
                Task4.Visible = false;
                Task5.Visible = false;
                Task6.Visible = false;
                Task7.Visible = false;
            }
            else if (task_count == 3)
            {

                DDLSubTask3.Items.Clear();
                DDLSubTask4.Items.Clear();
                DDLSubTask5.Items.Clear();
                DDLSubTask6.Items.Clear();

                Task3.Visible = false;
                Task4.Visible = false;
                Task5.Visible = false;
                Task6.Visible = false;
                Task7.Visible = false;
            }
            else if (task_count == 4)
            {

                DDLSubTask4.Items.Clear();
                DDLSubTask5.Items.Clear();
                DDLSubTask6.Items.Clear();

                Task4.Visible = false;
                Task5.Visible = false;
                Task6.Visible = false;
                Task7.Visible = false;
            }
            else if (task_count == 5)
            {

                DDLSubTask5.Items.Clear();
                DDLSubTask6.Items.Clear();

                Task5.Visible = false;
                Task6.Visible = false;
                Task7.Visible = false;
            }
            else if (task_count == 6)
            {
                DDLSubTask6.Items.Clear();

                Task6.Visible = false;
                Task7.Visible = false;
            }

        }

        protected void RBOptionList_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearAllSubTasksForward(1);
            DDLMainTask.Visible = true;


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

        private void BindPhases(Guid WorkpackageUID)
        {
            DataSet dtStatus = getdt.GetStatusForSearch(new Guid(DDLWorkPackage.SelectedValue));
            DataTable dt = getdt.GetPhasesAndStatusForSearch(WorkpackageUID);
            if (dt != null)
            {
                ddlPhase.DataTextField = "Phase";
                ddlPhase.DataValueField = "Phase";
                ddlPhase.DataSource = dt;
                ddlPhase.DataBind();

            }

            if (dtStatus != null && dtStatus.Tables[0].Rows.Count > 0)
            {
                var isExist = dtStatus.Tables[0].AsEnumerable().Where(r => r.Field<string>("ActualDocument_CurrentStatus") == "Code A-CE Approval").FirstOrDefault();
                if (isExist != null)
                {
                    ddlPhase.Items.Add(new ListItem { Text = "Approved By BWSSB Under Code A", Value = "Code A-CE Approval" });
                }
                isExist = dtStatus.Tables[0].AsEnumerable().Where(r => r.Field<string>("ActualDocument_CurrentStatus") == "Code B-CE Approval").FirstOrDefault();
                if (isExist != null)
                {
                    ddlPhase.Items.Add(new ListItem { Text = "Approved By BWSSB Under Code B", Value = "Code B-CE Approval" });
                }
                isExist = dtStatus.Tables[0].AsEnumerable().Where(r => r.Field<string>("ActualDocument_CurrentStatus") == "Code C-CE Approval").FirstOrDefault();
                if (isExist != null)
                {
                    ddlPhase.Items.Add(new ListItem { Text = "Under Client Approval Process", Value = "Code C-CE Approval" });
                }
                isExist = dtStatus.Tables[0].AsEnumerable().Where(r => r.Field<string>("ActualDocument_CurrentStatus") == "Client CE GFC Approval").FirstOrDefault();
                if (isExist != null)
                {
                    ddlPhase.Items.Add(new ListItem { Text = "Approved GFC by BWSSB", Value = "Client CE GFC Approval" });
                }
            }

            ddlPhase.Items.Insert(0, "All");
        }

        private void BindStatus()
        {
            ddlstatus.DataSource = getdt.GetStatusForSearch(new Guid(DDLWorkPackage.SelectedValue));
            ddlstatus.DataTextField = "ActualDocument_CurrentStatus";
            ddlstatus.DataValueField = "ActualDocument_CurrentStatus";
            ddlstatus.DataBind();
            ddlstatus.Items.Insert(0, "All");
        }

        private void BindType()
        {
            DataSet ds = getdt.GetDoctypeForSearch(new Guid(DDLWorkPackage.SelectedValue));
            string name = "";
            ddlType.Items.Clear();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (dr["DocumentType"].ToString() == "Word")
                {
                    if (name != dr["DocumentType"].ToString())
                    {
                        name = dr["DocumentType"].ToString();
                        ddlType.Items.Add(new ListItem(dr["DocumentType"].ToString(), ".doc"));
                    }

                }
                else if (dr["DocumentType"].ToString() == "Excel")
                {
                    if (name != dr["DocumentType"].ToString())
                    {
                        name = dr["DocumentType"].ToString();
                        ddlType.Items.Add(new ListItem(dr["DocumentType"].ToString(), ".xls"));
                    }

                }
                else
                {
                    ddlType.Items.Add(new ListItem(dr["DocumentType"].ToString(), dr["DocumentExtension"].ToString()));
                }
            }

            //ddlType.DataSource = getdt.GetDoctypeForSearch(new Guid(DDLWorkPackage.SelectedValue));
            //ddlType.DataTextField = "DocumentType";
            //ddlType.DataValueField = "DocumentExtension";
            //ddlType.DataBind();
            ddlType.Items.Insert(0, "All");
        }

        void BindFlow()
        {
            DataTable ds = getdt.GetDocumentFlow();
            if (DDlProject.SelectedItem.ToString() == "CP-25" || DDlProject.SelectedItem.ToString() == "CP-26" || DDlProject.SelectedItem.ToString() == "CP-27")
            {
                ds = getdt.GetDocumentFlow().AsEnumerable().Where(r => !r.Field<string>("Flow_Name").Equals("Issued for Planning Purpose") && !r.Field<string>("Flow_Name").Equals("Design by Consultant") && !r.Field<string>("Flow_Name").Equals("Vendor Approval-QAP (old)") && !r.Field<string>("Flow_Name").Equals("Works B Design (old)") && !r.Field<string>("Flow_Name").Equals("LNT Flow 2 (old docs)") && !r.Field<string>("Flow_Name").Equals("Flow 3")).CopyToDataTable();

            }
            else if (DDlProject.SelectedItem.ToString() == "CP-26")
            {
                ds = getdt.GetDocumentFlow().AsEnumerable().Where(r => !r.Field<string>("Flow_Name").Equals("Works A") && !r.Field<string>("Flow_Name").Equals("Works B") && !r.Field<string>("Flow_Name").Equals("Vendor Approval") && !r.Field<string>("Flow_Name").Equals("STP-Correspondence") && !r.Field<string>("Flow_Name").Equals("STP & ISPS Doc R&A") && !r.Field<string>("Flow_Name").Equals("Vendor Approval-QAP (old)") && !r.Field<string>("Flow_Name").Equals("Works B Design (old)") && !r.Field<string>("Flow_Name").Equals("Flow 3")).CopyToDataTable();

            }
            else
            {
                ds = getdt.GetDocumentFlow().AsEnumerable().Where(r => !r.Field<string>("Flow_Name").Equals("Works A") && !r.Field<string>("Flow_Name").Equals("Works B") && !r.Field<string>("Flow_Name").Equals("Vendor Approval") && !r.Field<string>("Flow_Name").Equals("STP-Correspondence") && !r.Field<string>("Flow_Name").Equals("STP & ISPS Doc R&A") && !r.Field<string>("Flow_Name").Equals("Vendor Approval-QAP (old)") && !r.Field<string>("Flow_Name").Equals("Works B Design (old)") && !r.Field<string>("Flow_Name").Equals("LNT Flow 2 (old docs)")).CopyToDataTable();

            }
            if (ds != null && ds.Rows.Count > 0)
            {
                DDLFlow.DataTextField = "Flow_Name";
                DDLFlow.DataValueField = "FlowMasterUID";
                DDLFlow.DataSource = ds;
                DDLFlow.DataBind();
                DDLFlow.Items.Insert(0, "All");
                ViewState["Flow"] = ds;
            }
        }

        protected void btnSubmit_Click1(object sender, EventArgs e)
        {
            try
            {
                //Session["searchedit"] = null;
                //Session["sSubmittal"] = txtSubmittal.Text;
                //Session["sDocName"] = txtDocName.Text;
                //Session["sDate"] = dtInDate.Text;
                //Session["sType"] = ddlType.SelectedValue;
                //Session["sStatus"] = ddlstatus.SelectedValue;
                if (DDlProject.SelectedIndex != -1 && DDLWorkPackage.SelectedIndex != -1)
                {
                    GrdDocuments.Visible = true;
                    UpdatePanel3.Update();
                    GrdDocuments.PageSize = int.Parse(txtPageSize.Text);
                    if (HiddenPaging.Value != "true")
                    {
                        GrdDocuments.PageIndex = 0;
                    }
                    BindDocuments();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnClearSearch_Click(object sender, EventArgs e)
        {
            Session["searchedit"] = null;
            Response.Redirect("default.aspx");
        }

        private void BindDocuments()
        {
            if (Session["searchedit"] != null)
            {
                GrdDocuments.PageIndex = Convert.ToInt32(Session["PageIndex"]);
            }


            Session["searchedit"] = null;
            Session["sSubmittal"] = txtSubmittal.Text;
            Session["sDocName"] = txtDocName.Text;
            Session["sDate"] = dtInDate.Text;
            Session["sDocDate"] = dtDocDate.Text;
            Session["sType"] = ddlType.SelectedValue;
            Session["sStatus"] = ddlstatus.SelectedValue;
            Session["sDateTo"] = dtInToDate.Text;
            Session["sDocDateTo"] = dtDocToDate.Text;

            DateTime InDate = DateTime.Now;
            DateTime DocumentDate = DateTime.Now;
            DateTime InToDate = DateTime.Now;
            DateTime DocumentToDate = DateTime.Now;
            if (!string.IsNullOrEmpty(dtInDate.Text))
            {
                InDate = Convert.ToDateTime(getdt.ConvertDateFormat(dtInDate.Text));
            }
            if (string.IsNullOrEmpty(dtInToDate.Text))
            {
                InToDate = InDate;
            }

            if (!string.IsNullOrEmpty(dtDocDate.Text))
            {
                DocumentDate = Convert.ToDateTime(getdt.ConvertDateFormat(dtDocDate.Text));
            }
            if (string.IsNullOrEmpty(dtDocToDate.Text))
            {
                DocumentToDate = DocumentDate;
            }
            if (!string.IsNullOrEmpty(dtInToDate.Text))
            {
                InToDate = Convert.ToDateTime(getdt.ConvertDateFormat(dtInToDate.Text));
            }
            if (!string.IsNullOrEmpty(dtDocToDate.Text))
            {
                DocumentToDate = Convert.ToDateTime(getdt.ConvertDateFormat(dtDocToDate.Text));
            }


            // validations
            if (dtInDate.Text == "" && dtInToDate.Text != "")
            {
                //Page.ClientScript.RegisterStartupScript(Page.GetType(), "Alert", "<script language='javascript'>alert('Please enter Incoming Recv From Date');</script>");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "CLOSE", "alert('Please enter Incoming Recv From Date.');", true);
                return;
            }
            else if (dtInDate.Text != "" && dtInToDate.Text == "")
            {
                //Page.ClientScript.RegisterStartupScript(Page.GetType(), "Alert", "<script language='javascript'>alert('Please enter Incoming Recv To Date');</script>");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "CLOSE", "alert('Please enter Incoming Recv To Date.');", true);
                return;
            }
            else if (dtInDate.Text != "" && dtInToDate.Text != "")
            {
                if (InDate > InToDate)
                {
                    //Page.ClientScript.RegisterStartupScript(Page.GetType(), "Alert", "<script language='javascript'>alert('Incoming Recv From Date cannot be greater than Incoming Recv To Date');</script>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "CLOSE", "alert('Incoming Recv From Date cannot be greater than Incoming Recv To Date.');", true);
                    return;
                }

            }

            if (dtDocDate.Text == "" && dtDocToDate.Text != "")
            {
                //Page.ClientScript.RegisterStartupScript(Page.GetType(), "Alert", "<script language='javascript'>alert('Please enter Document From Date');</script>");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "CLOSE", "alert('Please enter Document From Date.');", true);
                return;
            }
            else if (dtDocDate.Text != "" && dtDocToDate.Text == "")
            {
                //Page.ClientScript.RegisterStartupScript(Page.GetType(), "Alert", "<script language='javascript'>alert('Please enter Document To Date');</script>");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "CLOSE", "alert('Please enter Document To Date');", true);
                return;
            }
            else if (dtDocDate.Text != "" && dtDocToDate.Text != "")
            {
                if (DocumentDate > DocumentToDate)
                {
                    //Page.ClientScript.RegisterStartupScript(Page.GetType(), "Alert", "<script language='javascript'>alert('Document From Date cannot be greater than Document To Date');</script>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "CLOSE", "alert('Document From Date cannot be greater than Document To Date');", true);
                    return;
                }
            }
            DataSet ds = new DataSet();
            string statusValue = "";

            bool IsPhaseSearch = false;
            if (status.Text == "Phase" && ddlPhase.SelectedItem.Text != "All")
            {
                IsPhaseSearch = true;
                if (Constants.DicFinalStatusAndPhase.ContainsKey(ddlPhase.SelectedValue))
                {
                    IsPhaseSearch = false;
                    statusValue = ddlPhase.SelectedValue;
                }
            }
            string flowUID = string.Empty;
            if (DDLFlow.SelectedValue.ToString() != "All")
                flowUID = DDLFlow.SelectedValue.ToString();
            if (IsPhaseSearch)
            {
                ds = getdt.ActualDocuments_SelectBy_WorkpackageUID_SearchPhase(new Guid(DDlProject.SelectedValue), new Guid(DDLWorkPackage.SelectedValue), txtDocName.Text, ddlType.SelectedValue, txtSubmittal.Text, ddlPhase.SelectedValue, InDate, DocumentDate, InToDate, DocumentToDate, 4, txtOntbRef.Text, txtOriginatorRef.Text, flowUID);

                if (dtDocDate.Text != "" && dtInDate.Text != "")
                {
                    ds = new DataSet();
                    ds = getdt.ActualDocuments_SelectBy_WorkpackageUID_SearchPhase(new Guid(DDlProject.SelectedValue), new Guid(DDLWorkPackage.SelectedValue), txtDocName.Text, ddlType.SelectedValue, txtSubmittal.Text, ddlPhase.SelectedValue, InDate, DocumentDate, InToDate, DocumentToDate, 3, txtOntbRef.Text, txtOriginatorRef.Text, flowUID);
                }
                else if (dtInDate.Text != "")
                {
                    ds = new DataSet();
                    ds = getdt.ActualDocuments_SelectBy_WorkpackageUID_SearchPhase(new Guid(DDlProject.SelectedValue), new Guid(DDLWorkPackage.SelectedValue), txtDocName.Text, ddlType.SelectedValue, txtSubmittal.Text, ddlPhase.SelectedValue, InDate, DocumentDate, InToDate, DocumentToDate, 1, txtOntbRef.Text, txtOriginatorRef.Text, flowUID);
                }
                else if (dtDocDate.Text != "")
                {
                    ds = new DataSet();
                    ds = getdt.ActualDocuments_SelectBy_WorkpackageUID_SearchPhase(new Guid(DDlProject.SelectedValue), new Guid(DDLWorkPackage.SelectedValue), txtDocName.Text, ddlType.SelectedValue, txtSubmittal.Text, ddlPhase.SelectedValue, InDate, DocumentDate, InToDate, DocumentToDate, 2, txtOntbRef.Text, txtOriginatorRef.Text, flowUID);
                }
            }
            else
            {
                if (string.IsNullOrEmpty(statusValue))
                    statusValue = ddlstatus.SelectedValue;
                ds = getdt.ActualDocuments_SelectBy_WorkpackageUID_Search(new Guid(DDlProject.SelectedValue), new Guid(DDLWorkPackage.SelectedValue), txtDocName.Text, ddlType.SelectedValue, txtSubmittal.Text, statusValue, InDate, DocumentDate, InToDate, DocumentToDate, 4, txtOntbRef.Text, txtOriginatorRef.Text, flowUID);
                if (dtDocDate.Text != "" && dtInDate.Text != "")
                {
                    ds = new DataSet();
                    ds = getdt.ActualDocuments_SelectBy_WorkpackageUID_Search(new Guid(DDlProject.SelectedValue), new Guid(DDLWorkPackage.SelectedValue), txtDocName.Text, ddlType.SelectedValue, txtSubmittal.Text, statusValue, InDate, DocumentDate, InToDate, DocumentToDate, 3, txtOntbRef.Text, txtOriginatorRef.Text, flowUID);
                }
                else if (dtInDate.Text != "")
                {
                    ds = new DataSet();
                    ds = getdt.ActualDocuments_SelectBy_WorkpackageUID_Search(new Guid(DDlProject.SelectedValue), new Guid(DDLWorkPackage.SelectedValue), txtDocName.Text, ddlType.SelectedValue, txtSubmittal.Text, statusValue, InDate, DocumentDate, InToDate, DocumentToDate, 1, txtOntbRef.Text, txtOriginatorRef.Text, flowUID);
                }
                else if (dtDocDate.Text != "")
                {
                    ds = new DataSet();
                    ds = getdt.ActualDocuments_SelectBy_WorkpackageUID_Search(new Guid(DDlProject.SelectedValue), new Guid(DDLWorkPackage.SelectedValue), txtDocName.Text, ddlType.SelectedValue, txtSubmittal.Text, statusValue, InDate, DocumentDate, InToDate, DocumentToDate, 2, txtOntbRef.Text, txtOriginatorRef.Text, flowUID);
                }
            }


            if (ds.Tables[0].Rows.Count > 0)
            {
                lbldocNos.Text = ds.Tables[0].Rows.Count.ToString();
            }
            else
            {
                lbldocNos.Text = "0";
            }

            //  DataSet ds = getdt.ActualDocuments_SelectBy_WorkpackageUID_Search(new Guid(DDLWorkPackage.SelectedValue),"","","","",DocDate,1);



            //if (txtDocName.Text != "" && ddlType.SelectedIndex ==0 && txtSubmittal.Text =="" && ddlstatus.SelectedIndex == 0  && dtDocumentDate.Text == "")
            //{
            //    ds = getdt.ActualDocuments_SelectBy_WorkpackageUID_Search(new Guid(DDLWorkPackage.SelectedValue), txtDocName.Text, "", "", "", DocDate, 2);
            //}
            //else if (txtDocName.Text == "" && ddlType.SelectedIndex != 0 && txtSubmittal.Text == "" && ddlstatus.SelectedIndex == 0 && dtDocumentDate.Text == "")
            //{
            //    ds = getdt.ActualDocuments_SelectBy_WorkpackageUID_Search(new Guid(DDLWorkPackage.SelectedValue), txtDocName.Text, ddlType.SelectedValue, "", "", DocDate, 3);
            //}
            //else if (txtDocName.Text == "" && ddlType.SelectedIndex == 0 && txtSubmittal.Text != "" && ddlstatus.SelectedIndex == 0 && dtDocumentDate.Text == "")
            //{
            //    ds = getdt.ActualDocuments_SelectBy_WorkpackageUID_Search(new Guid(DDLWorkPackage.SelectedValue), txtDocName.Text, ddlType.SelectedValue, txtSubmittal.Text, "", DocDate, 4);

            //}
            //else if (txtDocName.Text == "" && ddlType.SelectedIndex == 0 && txtSubmittal.Text == "" && ddlstatus.SelectedIndex != 0 && dtDocumentDate.Text == "")
            //{
            //    ds = getdt.ActualDocuments_SelectBy_WorkpackageUID_Search(new Guid(DDLWorkPackage.SelectedValue), txtDocName.Text, ddlType.SelectedValue, txtSubmittal.Text, ddlstatus.SelectedValue, DocDate, 5);
            //}
            //else if (txtDocName.Text != "" && ddlType.SelectedIndex != 0 && txtSubmittal.Text != "" && ddlstatus.SelectedIndex != 0 && dtDocumentDate.Text =="" )
            //{
            //    ds = getdt.ActualDocuments_SelectBy_WorkpackageUID_Search(new Guid(DDLWorkPackage.SelectedValue), txtDocName.Text, ddlType.SelectedValue, txtSubmittal.Text, ddlstatus.SelectedValue, DocDate, 6);
            //}
            //else if (txtDocName.Text != "" && ddlType.SelectedIndex != 0 && txtSubmittal.Text != "" && ddlstatus.SelectedIndex == 0 && dtDocumentDate.Text == "")
            //{
            //    ds = getdt.ActualDocuments_SelectBy_WorkpackageUID_Search(new Guid(DDLWorkPackage.SelectedValue), txtDocName.Text, ddlType.SelectedValue, txtSubmittal.Text, ddlstatus.SelectedValue, DocDate, 7);
            //}
            //else if (txtDocName.Text != "" && ddlType.SelectedIndex != 0 && txtSubmittal.Text == "" && ddlstatus.SelectedIndex == 0 && dtDocumentDate.Text == "")
            //{
            //    ds = getdt.ActualDocuments_SelectBy_WorkpackageUID_Search(new Guid(DDLWorkPackage.SelectedValue), txtDocName.Text, ddlType.SelectedValue, txtSubmittal.Text, ddlstatus.SelectedValue, DocDate, 8);
            //}
            //else if (txtDocName.Text != "" && ddlType.SelectedIndex == 0 && txtSubmittal.Text != "" && ddlstatus.SelectedIndex == 0 && dtDocumentDate.Text == "")
            //{
            //    ds = getdt.ActualDocuments_SelectBy_WorkpackageUID_Search(new Guid(DDLWorkPackage.SelectedValue), txtDocName.Text, ddlType.SelectedValue, txtSubmittal.Text, ddlstatus.SelectedValue, DocDate, 9);
            //}
            //else if (txtDocName.Text == "" && ddlType.SelectedIndex != 0 && txtSubmittal.Text != "" && ddlstatus.SelectedIndex == 0 && dtDocumentDate.Text == "")
            //{
            //    ds = getdt.ActualDocuments_SelectBy_WorkpackageUID_Search(new Guid(DDLWorkPackage.SelectedValue), txtDocName.Text, ddlType.SelectedValue, txtSubmittal.Text, ddlstatus.SelectedValue, DocDate, 10);
            //}
            //else if (txtDocName.Text != "" && ddlType.SelectedIndex == 0 && txtSubmittal.Text == "" && ddlstatus.SelectedIndex != 0 && dtDocumentDate.Text == "")
            //{
            //    ds = getdt.ActualDocuments_SelectBy_WorkpackageUID_Search(new Guid(DDLWorkPackage.SelectedValue), txtDocName.Text, ddlType.SelectedValue, txtSubmittal.Text, ddlstatus.SelectedValue, DocDate, 11);
            //}
            //else if (txtDocName.Text == "" && ddlType.SelectedIndex == 0 && txtSubmittal.Text != "" && ddlstatus.SelectedIndex != 0 && dtDocumentDate.Text == "")
            //{
            //    ds = getdt.ActualDocuments_SelectBy_WorkpackageUID_Search(new Guid(DDLWorkPackage.SelectedValue), txtDocName.Text, ddlType.SelectedValue, txtSubmittal.Text, ddlstatus.SelectedValue, DocDate, 12);
            //}
            //else if (txtDocName.Text == "" && ddlType.SelectedIndex != 0 && txtSubmittal.Text == "" && ddlstatus.SelectedIndex != 0 && dtDocumentDate.Text == "")
            //{
            //    ds = getdt.ActualDocuments_SelectBy_WorkpackageUID_Search(new Guid(DDLWorkPackage.SelectedValue), txtDocName.Text, ddlType.SelectedValue, txtSubmittal.Text, ddlstatus.SelectedValue, DocDate, 13);
            //}
            //else if (txtDocName.Text != "" && ddlType.SelectedIndex != 0 && txtSubmittal.Text == "" && ddlstatus.SelectedIndex != 0 && dtDocumentDate.Text == "")
            //{
            //    ds = getdt.ActualDocuments_SelectBy_WorkpackageUID_Search(new Guid(DDLWorkPackage.SelectedValue), txtDocName.Text, ddlType.SelectedValue, txtSubmittal.Text, ddlstatus.SelectedValue, DocDate, 14);
            //}
            //else if (txtDocName.Text != "" && ddlType.SelectedIndex == 0 && txtSubmittal.Text != "" && ddlstatus.SelectedIndex != 0 && dtDocumentDate.Text == "")
            //{
            //    ds = getdt.ActualDocuments_SelectBy_WorkpackageUID_Search(new Guid(DDLWorkPackage.SelectedValue), txtDocName.Text, ddlType.SelectedValue, txtSubmittal.Text, ddlstatus.SelectedValue, DocDate, 15);
            //}
            //else if (txtDocName.Text == "" && ddlType.SelectedIndex != 0 && txtSubmittal.Text != "" && ddlstatus.SelectedIndex != 0 && dtDocumentDate.Text == "")
            //{
            //    ds = getdt.ActualDocuments_SelectBy_WorkpackageUID_Search(new Guid(DDLWorkPackage.SelectedValue), txtDocName.Text, ddlType.SelectedValue, txtSubmittal.Text, ddlstatus.SelectedValue, DocDate, 16);
            //}
            //else if (txtDocName.Text == "" && ddlType.SelectedIndex == 0 && txtSubmittal.Text == "" && ddlstatus.SelectedIndex == 0 && dtDocumentDate.Text != "")
            //{
            //    ds = getdt.ActualDocuments_SelectBy_WorkpackageUID_Search(new Guid(DDLWorkPackage.SelectedValue), txtDocName.Text, ddlType.SelectedValue, txtSubmittal.Text, ddlstatus.SelectedValue, DocDate, 17);
            //}
            //else if (txtDocName.Text != "" && ddlType.SelectedIndex == 0 && txtSubmittal.Text == "" && ddlstatus.SelectedIndex == 0 && dtDocumentDate.Text != "")
            //{
            //    ds = getdt.ActualDocuments_SelectBy_WorkpackageUID_Search(new Guid(DDLWorkPackage.SelectedValue), txtDocName.Text, ddlType.SelectedValue, txtSubmittal.Text, ddlstatus.SelectedValue, DocDate, 18);
            //}
            //else if (txtDocName.Text == "" && ddlType.SelectedIndex != 0 && txtSubmittal.Text == "" && ddlstatus.SelectedIndex == 0 && dtDocumentDate.Text != "")
            //{
            //    ds = getdt.ActualDocuments_SelectBy_WorkpackageUID_Search(new Guid(DDLWorkPackage.SelectedValue), txtDocName.Text, ddlType.SelectedValue, txtSubmittal.Text, ddlstatus.SelectedValue, DocDate, 19);
            //}
            //else if (txtDocName.Text == "" && ddlType.SelectedIndex == 0 && txtSubmittal.Text != "" && ddlstatus.SelectedIndex == 0 && dtDocumentDate.Text != "")
            //{
            //    ds = getdt.ActualDocuments_SelectBy_WorkpackageUID_Search(new Guid(DDLWorkPackage.SelectedValue), txtDocName.Text, ddlType.SelectedValue, txtSubmittal.Text, ddlstatus.SelectedValue, DocDate, 20);
            //}
            //else if (txtDocName.Text == "" && ddlType.SelectedIndex == 0 && txtSubmittal.Text == "" && ddlstatus.SelectedIndex != 0 && dtDocumentDate.Text != "")
            //{
            //    ds = getdt.ActualDocuments_SelectBy_WorkpackageUID_Search(new Guid(DDLWorkPackage.SelectedValue), txtDocName.Text, ddlType.SelectedValue, txtSubmittal.Text, ddlstatus.SelectedValue, DocDate, 21);
            //}
            //


            GrdDocuments.DataSource = ds;
            GrdDocuments.DataBind();

            ViewState["datatable"] = ds.Tables[0];

            //
            if (ds.Tables[0].Rows.Count > 10 && int.Parse(txtPageSize.Text) > 10)
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "Key", "<script>MakeStaticHeader('" + GrdDocuments.ClientID + "', 600, 1300 , 55 ,true); </script>", false);
            }
        }

        protected void GrdDocuments_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdDocuments.PageIndex = e.NewPageIndex;
            HiddenPaging.Value = "true";
            Session["PageIndex"] = GrdDocuments.PageIndex;
            BindDocuments();

        }

        protected void GrdDocuments_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            //LinkButton lnkbtn;
            if (e.Row.RowType == DataControlRowType.Header)
            {
                if (WebConfigurationManager.AppSettings["Domain"].ToString().ToUpper() == "NJSEI")
                    e.Row.Cells[5].Text = "NJSEI Reference #";
                else
                    e.Row.Cells[5].Text = "ONTB Reference #";
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataSet ds = getdt.getTop1_DocumentStatusSelect(new Guid(e.Row.Cells[0].Text));
                Label lblDocumentName = (Label)e.Row.FindControl("lblDocumentName");

                if (ds != null)
                {
                    if (ds.Tables[0].Rows[0]["DocumentType"].ToString() == "General Document")
                    {
                        e.Row.Cells[8].Text = "No History";
                    }
                    //
                    if (ds.Tables[0].Rows[0]["ActivityType"].ToString() != "" && ds.Tables[0].Rows[0]["TopVersion"].ToString() != "")
                    {
                        //e.Row.Cells[1].Text = ds.Tables[0].Rows[0]["TopVersion"].ToString();

                        try
                        {
                            string newVersionFileName = Path.GetFileNameWithoutExtension(Server.MapPath(ds.Tables[0].Rows[0]["Doc_Path"].ToString()));
                            lblDocumentName.Text = newVersionFileName.Substring(0, (newVersionFileName.Length - 2)) + " [ Ver. " + ds.Tables[0].Rows[0]["TopVersion"].ToString() + " ]"; ;
                        }
                        catch
                        {

                        }

                    }
                }
                string SubmittalUID = getdt.GetSubmittalUID_By_ActualDocumentUID(new Guid(e.Row.Cells[0].Text));
                string Flowtype = getdt.GetFlowTypeBySubmittalUID(new Guid(SubmittalUID));
                string FlowUID = getdt.GetFlowUIDBySubmittalUID(new Guid(SubmittalUID));
                if (Session["IsContractor"].ToString() == "Y")
                {

                    string phase = getdt.GetPhaseforStatus(new Guid(getdt.GetFlowUIDBySubmittalUID(new Guid(SubmittalUID))), e.Row.Cells[4].Text);
                    //string phase = getdata.GetPhaseforStatus(new Guid(Request.QueryString["FlowUID"]), e.Row.Cells[3].Text);

                    if (Flowtype == "STP")
                    {
                        if (string.IsNullOrEmpty(phase))
                        {

                            //if (e.Row.Cells[4].Text == "Code A-CE Approval" || e.Row.Cells[4].Text == "Client CE GFC Approval")
                            //{
                            //    e.Row.Cells[4].Text = "Approved";

                            //}
                            //if (e.Row.Cells[4].Text == "Code B-CE Approval" || e.Row.Cells[3].Text == "Code C-CE Approval")
                            //{
                            //    e.Row.Cells[4].Text = "Client Approval";
                            //}
                            if (e.Row.Cells[4].Text == "Code A-CE Approval")
                            {
                                e.Row.Cells[4].Text = "Approved By BWSSB Under Code A";

                            }
                            else if (e.Row.Cells[4].Text == "Code B-CE Approval")
                            {
                                e.Row.Cells[4].Text = "Approved By BWSSB Under Code B";
                            }
                            else if (e.Row.Cells[4].Text == "Code C-CE Approval")
                            {
                                e.Row.Cells[4].Text = "Under Client Approval Process";

                            }
                            else if (e.Row.Cells[4].Text == "Client CE GFC Approval")
                            {
                                e.Row.Cells[4].Text = "Approved GFC by BWSSB";
                            }
                        }
                        else
                        {
                            e.Row.Cells[4].Text = phase;
                        }

                    }

                    if (Flowtype == "STP-OB")// ONTB/BWSSB Correspondence show/hide some columns for contractor
                    {
                        DataSet documentSTatusList = getdt.getActualDocumentStatusList(new Guid(e.Row.Cells[0].Text));
                        if (documentSTatusList.Tables[0].Rows.Count > 0)
                        {
                            if (getdt.CheckCCtoUser(new Guid(documentSTatusList.Tables[0].Rows[0]["StatusUID"].ToString()), "Contractor"))
                            {
                                e.Row.Cells[1].Enabled = true;
                                e.Row.Cells[2].Enabled = true;
                            }
                            else
                            {
                                e.Row.Cells[1].Text = "Access Denied";
                                e.Row.Cells[2].Text = "Access Denied to View";
                                e.Row.Cells[10].Text = "Access Denied";
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
                            e.Row.Cells[1].Enabled = true;
                            e.Row.Cells[2].Enabled = true;
                        }
                        else // other  than DTL Correspondence
                        {
                            DataSet documentSTatusList = getdt.getActualDocumentStatusList(new Guid(e.Row.Cells[0].Text));
                            if (getdt.GetUserClientType(new Guid(DDLWorkPackage.SelectedValue), Session["UserUID"].ToString()) == "DTL" || getdt.GetUserClientType(new Guid(DDLWorkPackage.SelectedValue), Session["UserUID"].ToString()) == "PC")
                            {
                                if (getdt.CheckCCtoUser(new Guid(documentSTatusList.Tables[0].Rows[0]["StatusUID"].ToString()), "DTL"))
                                {
                                    e.Row.Cells[1].Enabled = true;
                                    e.Row.Cells[2].Enabled = true;
                                }
                                else
                                {
                                    e.Row.Cells[1].Text = "Access Denied";
                                    e.Row.Cells[2].Text = "Access Denied to View";
                                    e.Row.Cells[10].Text = "Access Denied";
                                }
                            }
                            else
                            {
                                e.Row.Cells[1].Enabled = true;
                                e.Row.Cells[2].Enabled = true;
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
                                e.Row.Cells[1].Enabled = true;
                                e.Row.Cells[2].Enabled = true;
                            }
                            else
                            {
                                DataSet documentSTatusList = getdt.getActualDocumentStatusList(new Guid(e.Row.Cells[0].Text));
                                if (getdt.CheckCCtoUser(new Guid(documentSTatusList.Tables[0].Rows[0]["StatusUID"].ToString()), "EE"))
                                {
                                    e.Row.Cells[1].Enabled = true;
                                    e.Row.Cells[2].Enabled = true;
                                }
                                else
                                {
                                    e.Row.Cells[1].Text = "Access Denied";
                                    e.Row.Cells[2].Text = "Access Denied to View";
                                    e.Row.Cells[10].Text = "Access Denied";
                                }

                            }
                        }
                        else if (getdt.GetUserClientType(new Guid(DDLWorkPackage.SelectedValue), Session["UserUID"].ToString()) == "ACE")
                        {
                            if (FlowUID.ToLower() == "abae0151-40a2-43f9-9577-d5211011f479") // ACE Correspondence
                            {
                                e.Row.Cells[1].Enabled = true;
                                e.Row.Cells[2].Enabled = true;
                            }
                            else
                            {
                                DataSet documentSTatusList = getdt.getActualDocumentStatusList(new Guid(e.Row.Cells[0].Text));
                                if (getdt.CheckCCtoUser(new Guid(documentSTatusList.Tables[0].Rows[0]["StatusUID"].ToString()), "ACE"))
                                {
                                    e.Row.Cells[1].Enabled = true;
                                    e.Row.Cells[2].Enabled = true;
                                }
                                else
                                {
                                    e.Row.Cells[1].Text = "Access Denied";
                                    e.Row.Cells[2].Text = "Access Denied to View";
                                    e.Row.Cells[10].Text = "Access Denied";
                                }

                            }
                        }
                        else if (getdt.GetUserClientType(new Guid(DDLWorkPackage.SelectedValue), Session["UserUID"].ToString()) == "CE")
                        {
                            if (FlowUID.ToLower() == "c0e51552-35c2-4fbc-8fde-d11ab0fbdf39") // ACE Correspondence
                            {
                                e.Row.Cells[1].Enabled = true;
                                e.Row.Cells[2].Enabled = true;
                            }
                            else
                            {
                                DataSet documentSTatusList = getdt.getActualDocumentStatusList(new Guid(e.Row.Cells[0].Text));
                                if (getdt.CheckCCtoUser(new Guid(documentSTatusList.Tables[0].Rows[0]["StatusUID"].ToString()), "ACE"))
                                {
                                    e.Row.Cells[1].Enabled = true;
                                    e.Row.Cells[2].Enabled = true;
                                }
                                else
                                {
                                    e.Row.Cells[1].Text = "Access Denied";
                                    e.Row.Cells[2].Text = "Access Denied to View";
                                    e.Row.Cells[10].Text = "Access Denied";
                                }

                            }
                        }
                        else if (getdt.GetUserClientType(new Guid(DDLWorkPackage.SelectedValue), Session["UserUID"].ToString()) == "AEE")
                        {

                            DataSet documentSTatusList = getdt.getActualDocumentStatusList(new Guid(e.Row.Cells[0].Text));
                            if (getdt.CheckCCtoUser(new Guid(documentSTatusList.Tables[0].Rows[0]["StatusUID"].ToString()), "AEE"))
                            {
                                e.Row.Cells[1].Enabled = true;
                                e.Row.Cells[2].Enabled = true;
                            }
                            else
                            {
                                e.Row.Cells[1].Text = "Access Denied";
                                e.Row.Cells[2].Text = "Access Denied to View";
                                e.Row.Cells[10].Text = "Access Denied";
                            }


                        }
                    }
                    //added on 05/12/2022
                    if (e.Row.Cells[6].Text == "Submitted to DTL for ACE" || e.Row.Cells[6].Text == "Submitted to DTL for CE" || e.Row.Cells[6].Text == "Submitted to DTL for EE")
                    {
                        e.Row.Visible = false;
                    }


                }
            }
        }

        protected void GrdDocuments_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string UID = e.CommandArgument.ToString();
            if (e.CommandName == "download")
            {
                string path = string.Empty;
                string filename = string.Empty;
                DataSet ds1 = null;
                DataSet ds = getdt.getLatest_DocumentVerisonSelect(new Guid(UID));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    path = Server.MapPath(ds.Tables[0].Rows[0]["Doc_FileName"].ToString());
                    //File.Decrypt(path);
                    filename = Path.GetFileName(ds.Tables[0].Rows[0]["Doc_FileName"].ToString());
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
                //ds = getdt.ActualDocuments_SelectBy_ActualDocumentUID(new Guid(UID));
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
                    string getExtension = System.IO.Path.GetExtension(path);
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
                        // Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('File does not exists.');</script>");
                        //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "CLOSE", "<script language='javascript'>alert('File does not exists.');</script>",true);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "CLOSE", "alert('File does not exists.');", true);
                    }
                }
                catch (Exception ex)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Document not uploaded for this flow.Please Contact Document controller!');</script>");
                }
            }
        }

        protected void GrdDocuments_Sorting(object sender, GridViewSortEventArgs e)
        {

            DataTable dataTable = ViewState["datatable"] as DataTable;

            //if (dataTable != null)
            //{
            //    DataView dataView = new DataView(dataTable);
            //    dataView.Sort = e.SortExpression + " " + ConvertSortDirectionToSql(e.SortDirection);

            //    GrdDocuments.DataSource = dataView;
            //    GrdDocuments.DataBind();
            //}

            SetSortDirection(ViewState["SortDireaction"].ToString());

            if (dataTable != null)
            {
                //Sort the data.
                dataTable.DefaultView.Sort = e.SortExpression + " " + ViewState["_sortDirection"].ToString();
                GrdDocuments.DataSource = dataTable;
                GrdDocuments.DataBind();
                ViewState["SortDireaction"] = ViewState["_sortDirection"];
            }
        }

        protected void GrdDocuments_DataBound(object sender, EventArgs e)
        {
            //int sortedColumnPosition = 0;
            //LinkButton lnkbtn;

            //// Gets position of column whose header text matches SortExpression
            //// of the GridView when column is sorted
            //foreach (TableCell cell in GrdDocuments.HeaderRow.Cells)
            //{
            //    lnkbtn = (LinkButton)cell.Controls[0];
            //    if (lnkbtn.Text == GrdDocuments.SortExpression)
            //    {
            //        break;
            //    }
            //    sortedColumnPosition++;
            //}
            //if (!string.IsNullOrEmpty(GrdDocuments.SortExpression))
            //{
            //    foreach (GridViewRow row in GrdDocuments.Rows)
            //    {
            //        row.Cells[sortedColumnPosition].BackColor = System.Drawing.Color.LavenderBlush;
            //    }
            //}
        }

        protected void SetSortDirection(string sortDirection)
        {
            if (sortDirection == "ASC")
            {
                ViewState["_sortDirection"] = "DESC";
            }
            else
            {
                ViewState["_sortDirection"] = "ASC";
            }
        }

        public string GetDocumentTypeIcon(string DocumentExtn)
        {
            return getdt.GetDocumentTypeMasterIcon_by_Extension(DocumentExtn);
        }

        public string GetSubmittalName(string DocumentID)
        {
            return getdt.getDocumentName_by_DocumentUID(new Guid(DocumentID));
        }

        public string GetTaskHierarchy_By_DocumentUID(string DocumentUID)
        {
            return getdt.GetTaskHierarchy_By_DocumentUID(new Guid(DocumentUID));
        }
    }
}