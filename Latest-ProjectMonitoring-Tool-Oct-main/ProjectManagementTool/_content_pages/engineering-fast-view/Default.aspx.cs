using ProjectManager.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManager._content_pages.engineering_fast_view
{
    public partial class Default : System.Web.UI.Page
    {
        DBGetData dbgetdata = new DBGetData();
        TaskUpdate gettk = new TaskUpdate();
       // private static string SelectedTaskUID = string.Empty;
        private static string SelectedTaskName = string.Empty;
        public int SearchResultCount = 0;
        private static string[] TaskIDArray = new string[8];
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Username"] == null)
            {
                Response.Redirect("~/Login.aspx");
            }
            else
            {
                ScriptManager.RegisterStartupScript(
                        UpdatePanel2,
                        this.GetType(),
                        "MyAction",
                        "BindEvents();",
                        true);
                if (!IsPostBack)
                {
                    BindProject();
                    SelectedProject();
                    DDlProject_SelectedIndexChanged(sender, e);

                    if (TaskIDArray[0] == null & Session["TaskIDArray"] == null)
                    {
                        InitTaskArray();
                        Session["TaskIDArray"] = FillTaskArray();
                        Session["CurrentSelection"] = "";
                    }
                     
                    Session["BOQData"] = null;
                    //
                    if (Session["TypeOfUser"].ToString() == "NJSD")
                    {
                        UploadSitePhotograph.Visible = false;
                        ViewSitePhotograph.Visible = false;
                        CopyActivityData.Visible = false;
                        btnAddSubTask.Visible = false;
                        btnAddTask.Visible = false;
                        btnaddtaskschedule.Visible = false;
                        btnDependency.Visible = false;
                        btnMilestoneAdd.Visible = false;
                        btncopy.Visible = false;
                        btncopy.Visible = false;
                        //
                        GrdTreeView.Columns[4].Visible = false;
                        GrdTreeView.Columns[5].Visible = false;
                        //
                        grdMileStones.Columns[5].Visible = false;
                        grdMileStones.Columns[6].Visible = false;
                        //
                        grdResourceAllocated.Columns[8].Visible = false;
                        grdResourceAllocated.Columns[9].Visible = false;
                        //
                        GrdResourceDeployment.Columns[3].Visible = false;
                    }
                    //
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

        private void OptionBind(string WorkpackgeUID)
        {
            DataSet dsoption = dbgetdata.GetSelectedOption_By_WorkpackageUID(new Guid(WorkpackgeUID));
            RBOptionList.DataSource = dsoption;
            RBOptionList.DataBind();
        }

        private void BindMainTask(string WorkpackageUID)
        {
            DataSet ds = dbgetdata.GetTasksForWorkPackages(WorkpackageUID);
            DDLMainTask.DataTextField = "Name";
            DDLMainTask.DataValueField = "TaskUID";
            DDLMainTask.DataSource = ds;
            DDLMainTask.DataBind();
            DDLMainTask.Items.Insert(0, "--Select--");
        }

        protected void DDlProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDlProject.SelectedValue != "")
            {
                DataSet ds = new DataSet();
                if (Session["TypeOfUser"].ToString() == "U" || Session["TypeOfUser"].ToString() == "MD" || Session["TypeOfUser"].ToString() == "VP" || Session["TypeOfUser"].ToString() == "NJSD")
                {
                    ds = dbgetdata.GetWorkPackages_By_ProjectUID(new Guid(DDlProject.SelectedValue));
                }
                else if (Session["TypeOfUser"].ToString() == "PA")
                {
                    ds = dbgetdata.GetWorkPackages_ForUser_by_ProjectUID(new Guid(Session["UserUID"].ToString()), new Guid(DDlProject.SelectedValue));
                }
                else
                {
                    ds = dbgetdata.GetWorkPackages_ForUser_by_ProjectUID(new Guid(Session["UserUID"].ToString()), new Guid(DDlProject.SelectedValue));
                }

                               
                HideButtons();
                Session["Project_Workpackage"] = DDlProject.SelectedValue;

                DDLWorkPackage.DataTextField = "Name";
                DDLWorkPackage.DataValueField = "WorkPackageUID";
                DDLWorkPackage.DataSource = ds;
                DDLWorkPackage.DataBind();
               // SelectedProjectWorkpackage("Workpackage");
                Session["Project_Workpackage"] = DDlProject.SelectedValue + "_" + DDLWorkPackage.SelectedValue;
               // BindMainTask(DDLWorkPackage.SelectedValue);
                OptionBind(DDLWorkPackage.SelectedValue);
                ClearAllSubTasksForward(0);
                BindActivities3();
                              

               // DDLWorkPackage.SelectedIndex = 0;
               // DDLWorkPackage_SelectedIndexChanged(sender, e);
                    
                    if (Session["SelectedActivity"] != null)
                    {
                        if (Session["SelectedActivity"].ToString() != "")
                        {
                                if (Session["CurrentSelection"].ToString() == "WorkPackage" )
                                {
                                   RBOptionList.SelectedIndex = Convert.ToInt32(Session["OptionIndex"].ToString());
                                   RBOptionList_SelectedIndexChanged(sender, e);

                                   DDLMainTask.SelectedValue = Session["SelectedActivity"].ToString();

                                       if (Session["edited"].ToString() == "edited" )
                                        {
                                            DisplayTasks();
                                           // SelectedTaskUID = Session["SelectedActivity"].ToString();
                                            ViewState["SelectedTaskUID"] = Session["SelectedActivity"].ToString();
                                            ViewState["Show"] = "false";
                                            BindActivities2();
                                            Session["edited"] = "";
                                        }
                                }
                                else if (Session["CurrentSelection"].ToString() == "Task")
                                {
                                        RBOptionList.SelectedIndex = Convert.ToInt32(Session["OptionIndex"].ToString());
                                        RBOptionList_SelectedIndexChanged(sender, e);

                                        DisplayTasks();

                            // SelectedTaskUID = Session["SelectedActivity"].ToString();
                                    ViewState["SelectedTaskUID"] = Session["SelectedActivity"].ToString();
                                    ViewState["Show"] = "true";
                                        BindActivities2();
                                }
                        }

                        Session["SelectedActivity"] = "";
                    }
            }
        }

        protected void DDLWorkPackage_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            Session["Project_Workpackage"] = DDlProject.SelectedValue + "_" + DDLWorkPackage.SelectedValue;

            //  SelectedTaskUID = DDLWorkPackage.SelectedValue;
            ViewState["SelectedTaskUID"] = Session["SelectedActivity"].ToString();

            BindMainTask(DDLWorkPackage.SelectedValue);
            OptionBind(DDLWorkPackage.SelectedValue);

            MainTask.Visible = false;
            Task1.Visible = false;
            Task2.Visible = false;
            Task3.Visible = false;
            Task4.Visible = false;
            Task5.Visible = false;
            Task6.Visible = false;
            Task7.Visible = false;

            BindActivities3();


        }

        private void ClearAllSubTasksForward(int task_count)
        {
            if (task_count ==0)
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

                // SelectedTaskUID = "";
                ViewState["SelectedTaskUID"] = "";

                SelectedTaskName = "";
            }
            else if ( task_count ==1)
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
      

        protected void DDLMainTask_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDLMainTask.SelectedValue != "--Select--")
            {
                DataSet ds = dbgetdata.GetSubTasksForWorkPackages(DDLMainTask.SelectedValue);
                DDLSubTask.DataTextField = "Name";
                DDLSubTask.DataValueField = "TaskUID";
                DDLSubTask.DataSource = ds;
                DDLSubTask.DataBind();
                DDLSubTask.Items.Insert(0, "--Select--");
                // SelectedTaskUID = DDLMainTask.SelectedValue;
                ViewState["SelectedTaskUID"] = DDLMainTask.SelectedValue;

                SelectedTaskName = DDLMainTask.SelectedItem.Text;

                ClearAllSubTasksForward(1);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    Task1.Visible = true;
                }

            }
            else
            {
                ClearAllSubTasksForward(0);
            }
        }

        protected void DDLSubTask_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDLSubTask.SelectedValue != "--Select--")
            {
                DataSet ds = dbgetdata.GetSubtoSubTasksForWorkPackages(DDLSubTask.SelectedValue);
                DDLSubTask1.DataTextField = "Name";
                DDLSubTask1.DataValueField = "TaskUID";
                DDLSubTask1.DataSource = ds;
                DDLSubTask1.DataBind();
                DDLSubTask1.Items.Insert(0, "--Select--");
                // SelectedTaskUID = DDLSubTask.SelectedValue;

                ViewState["SelectedTaskUID"] = DDLSubTask.SelectedValue;

                SelectedTaskName = DDLSubTask.SelectedItem.Text;

                ClearAllSubTasksForward(2);

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
                DataSet ds = dbgetdata.GetSubtoSubtoSubTasksForWorkPackages(DDLSubTask1.SelectedValue);
                DDLSubTask2.DataTextField = "Name";
                DDLSubTask2.DataValueField = "TaskUID";
                DDLSubTask2.DataSource = ds;
                DDLSubTask2.DataBind();
                DDLSubTask2.Items.Insert(0, "--Select--");
               // SelectedTaskUID = DDLSubTask1.SelectedValue;

                ViewState["SelectedTaskUID"] = DDLSubTask1.SelectedValue;
                SelectedTaskName = DDLSubTask1.SelectedItem.Text;

                ClearAllSubTasksForward(3);

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
                // SelectedTaskUID = DDLSubTask2.SelectedValue;
                ViewState["SelectedTaskUID"] = DDLSubTask2.SelectedValue;

                SelectedTaskName = DDLSubTask2.SelectedItem.Text;
                DataSet ds = dbgetdata.GetSubtoSubtoSubtoSubTasksForWorkPackages(DDLSubTask2.SelectedValue);
                DDLSubTask3.DataTextField = "Name";
                DDLSubTask3.DataValueField = "TaskUID";
                DDLSubTask3.DataSource = ds;
                DDLSubTask3.DataBind();
                DDLSubTask3.Items.Insert(0, "--Select--");

                ClearAllSubTasksForward(4);

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
               // SelectedTaskUID = DDLSubTask3.SelectedValue;
                ViewState["SelectedTaskUID"] = DDLSubTask3.SelectedValue;

                SelectedTaskName = DDLSubTask3.SelectedItem.Text;
                DataSet ds = dbgetdata.GetSubtoSubtoSubtoSubtoSubTasksForWorkPackages(DDLSubTask3.SelectedValue);
                DDLSubTask4.DataTextField = "Name";
                DDLSubTask4.DataValueField = "TaskUID";
                DDLSubTask4.DataSource = ds;
                DDLSubTask4.DataBind();
                DDLSubTask4.Items.Insert(0, "--Select--");

                ClearAllSubTasksForward(5);

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

               // SelectedTaskUID = DDLSubTask4.SelectedValue;
                ViewState["SelectedTaskUID"] = DDLSubTask4.SelectedValue;

                SelectedTaskName = DDLSubTask4.SelectedItem.Text;
                DataSet ds = dbgetdata.GetSubTask_By_ParentTask_Level(DDLSubTask4.SelectedValue, 7);
                DDLSubTask5.DataTextField = "Name";
                DDLSubTask5.DataValueField = "TaskUID";
                DDLSubTask5.DataSource = ds;
                DDLSubTask5.DataBind();
                DDLSubTask5.Items.Insert(0, "--Select--");

                ClearAllSubTasksForward(6);

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
               // SelectedTaskUID = DDLSubTask5.SelectedValue;
                ViewState["SelectedTaskUID"] = DDLSubTask5.SelectedValue;
                SelectedTaskName = DDLSubTask5.SelectedItem.Text;
                DataSet ds = dbgetdata.GetSubTask_By_ParentTask_Level(DDLSubTask5.SelectedValue, 8);
                DDLSubTask6.DataTextField = "Name";
                DDLSubTask6.DataValueField = "TaskUID";
                DDLSubTask6.DataSource = ds;
                DDLSubTask6.DataBind();
                DDLSubTask6.Items.Insert(0, "--Select--");

                ClearAllSubTasksForward(7);

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
               // SelectedTaskUID = DDLSubTask6.SelectedValue;
                ViewState["SelectedTaskUID"] = DDLSubTask6.SelectedValue;
                SelectedTaskName = DDLSubTask6.SelectedItem.Text;
            }
        }

        protected void RBOptionList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RBOptionList.SelectedValue != "")
            {
                ClearAllSubTasksForward(1);
               
                DataSet ds = dbgetdata.GetTasks_by_WorkpackageOptionUID(new Guid(DDLWorkPackage.SelectedValue), new Guid(RBOptionList.SelectedValue));
                DDLMainTask.DataTextField = "Name";
                DDLMainTask.DataValueField = "TaskUID";
                DDLMainTask.DataSource = ds;
                DDLMainTask.DataBind();
                DDLMainTask.Items.Insert(0, "--Select--");

                DDLMainTask.Visible = true;

                BindActivities1();

                if (Session["CurrentSelection"].ToString() == "")
                         Session["CurrentSelection"] = "WorkPackage";

                Session["OptionIndex"] = RBOptionList.SelectedIndex;

                
              //  Session["TaskIDArray"] = FillTaskArray();
            }
           
            
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            LblMessage.Text = string.Empty;

            if (ViewState["SelectedTaskUID"] != null)
            {
                if (!string.IsNullOrEmpty(ViewState["SelectedTaskUID"].ToString()))
                {
                    Session["TaskIDArray"] = FillTaskArray();
                    ViewState["IsFolder"] = "No";
                    BindActivities2();
                    Session["CurrentSelection"] = "Task";
                }
                else
                {
                    LblMessage.Text = "Select any Task";
                    LblMessage.ForeColor = System.Drawing.Color.Red;
                    Session["CurrentSelection"] = "Folder";
                }
            }
            else
            {
                LblMessage.Text = "Select any Task";
                LblMessage.ForeColor = System.Drawing.Color.Red;
                Session["CurrentSelection"] = "Folder";
            }
        }
        protected void btnClear_Click(object sender, EventArgs e)
        {
            Session["CurrentSelection"] = "";
            Response.Redirect("default.aspx");
        }

        private void InitTaskArray()
        {
            TaskIDArray[0] = "--Select--";

            TaskIDArray[1] = "";

            TaskIDArray[2] = "";

            TaskIDArray[3] = "";

            TaskIDArray[4] = "";

            TaskIDArray[5] = "";

            TaskIDArray[6] = "";

            TaskIDArray[7] = "";

            return;
        }

        private string[] FillTaskArray()
        {
            if (DDLMainTask.Visible)
                TaskIDArray[0] = DDLMainTask.SelectedValue != "--Select--" ? DDLMainTask.SelectedValue : "--Select--";
            else 
                TaskIDArray[0] = "";

            if (DDLSubTask.Visible)
                TaskIDArray[1] = DDLSubTask.SelectedValue != "--Select--" ? DDLSubTask.SelectedValue : "--Select--";
            else
                TaskIDArray[1] = "";

            if (DDLSubTask1.Visible)
                TaskIDArray[2] = DDLSubTask1.SelectedValue != "--Select--" ? DDLSubTask1.SelectedValue : "--Select--";
            else
                TaskIDArray[2] = "";

            if (DDLSubTask2.Visible)
                TaskIDArray[3] = DDLSubTask2.SelectedValue != "--Select--" ? DDLSubTask2.SelectedValue : "--Select--";
            else
                TaskIDArray[3] = "";

            if (DDLSubTask3.Visible)
                TaskIDArray[4] = DDLSubTask3.SelectedValue != "--Select--" ? DDLSubTask3.SelectedValue : "--Select--";
            else
                TaskIDArray[4] = "";

            if (DDLSubTask4.Visible)
                TaskIDArray[5] = DDLSubTask4.SelectedValue != "--Select--" ? DDLSubTask4.SelectedValue : "--Select--";
            else
                TaskIDArray[5] = "";

            if (DDLSubTask5.Visible)
                TaskIDArray[6] = DDLSubTask5.SelectedValue != "--Select--" ? DDLSubTask5.SelectedValue : "--Select--";
            else
                TaskIDArray[6] = "";

            if (DDLSubTask6.Visible)
                TaskIDArray[7] = DDLSubTask6.SelectedValue != "--Select--" ? DDLSubTask6.SelectedValue : "--Select--";
            else
                TaskIDArray[7] = "";

            return TaskIDArray;
        }

        private void DisplayTasks()
        {
            try
            {
                TaskIDArray = (string[])Session["TaskIDArray"];

                if (!string.IsNullOrEmpty(TaskIDArray[0]))
                {
                    DDLMainTask.SelectedValue = TaskIDArray[0];
                    DDLMainTask.Visible = true;

                    DataSet ds = dbgetdata.GetSubTasksForWorkPackages(DDLMainTask.SelectedValue);
                    DDLSubTask.DataTextField = "Name";
                    DDLSubTask.DataValueField = "TaskUID";
                    DDLSubTask.DataSource = ds;
                    DDLSubTask.DataBind();
                    DDLSubTask.Items.Insert(0, "--Select--");

                    if (TaskIDArray[0] == "--Select--")
                    {
                        if (Session["SelectedActivity"] != null)
                            DDLMainTask.SelectedValue = Session["SelectedActivity"].ToString();
                    }
                }

                if (!string.IsNullOrEmpty(TaskIDArray[1]))
                {

                    DDLSubTask.SelectedValue = TaskIDArray[1];
                    Task1.Visible = true;

                    DataSet ds = dbgetdata.GetSubtoSubTasksForWorkPackages(DDLSubTask.SelectedValue);
                    DDLSubTask1.DataTextField = "Name";
                    DDLSubTask1.DataValueField = "TaskUID";
                    DDLSubTask1.DataSource = ds;
                    DDLSubTask1.DataBind();
                    DDLSubTask1.Items.Insert(0, "--Select--");

                    if (TaskIDArray[1] == "--Select--")
                    {
                        if (Session["SelectedActivity"] != null)
                            DDLSubTask.SelectedValue = Session["SelectedActivity"].ToString();
                    }
                }

                if (!string.IsNullOrEmpty(TaskIDArray[2]))
                {
                    DDLSubTask1.SelectedValue = TaskIDArray[2];
                    Task2.Visible = true;

                    DataSet ds = dbgetdata.GetSubtoSubtoSubTasksForWorkPackages(DDLSubTask1.SelectedValue);
                    DDLSubTask2.DataTextField = "Name";
                    DDLSubTask2.DataValueField = "TaskUID";
                    DDLSubTask2.DataSource = ds;
                    DDLSubTask2.DataBind();
                    DDLSubTask2.Items.Insert(0, "--Select--");

                    if (TaskIDArray[2] == "--Select--")
                    {
                        if (Session["SelectedActivity"] != null)
                            DDLSubTask1.SelectedValue = Session["SelectedActivity"].ToString();
                    }
                }

                if (!string.IsNullOrEmpty(TaskIDArray[3]))
                {
                    DDLSubTask2.SelectedValue = TaskIDArray[3];
                    Task3.Visible = true;

                    DataSet ds = dbgetdata.GetSubtoSubtoSubtoSubTasksForWorkPackages(DDLSubTask2.SelectedValue);
                    DDLSubTask3.DataTextField = "Name";
                    DDLSubTask3.DataValueField = "TaskUID";
                    DDLSubTask3.DataSource = ds;
                    DDLSubTask3.DataBind();
                    DDLSubTask3.Items.Insert(0, "--Select--");

                    if (TaskIDArray[3] == "--Select--")
                    {
                        if (Session["SelectedActivity"] != null)
                            DDLSubTask2.SelectedValue = Session["SelectedActivity"].ToString();
                    }
                }

                if (!string.IsNullOrEmpty(TaskIDArray[4]))
                {
                    DDLSubTask3.SelectedValue = TaskIDArray[4];
                    Task4.Visible = true;

                    DataSet ds = dbgetdata.GetSubtoSubtoSubtoSubtoSubTasksForWorkPackages(DDLSubTask3.SelectedValue);
                    DDLSubTask4.DataTextField = "Name";
                    DDLSubTask4.DataValueField = "TaskUID";
                    DDLSubTask4.DataSource = ds;
                    DDLSubTask4.DataBind();
                    DDLSubTask4.Items.Insert(0, "--Select--");

                    if (TaskIDArray[4] == "--Select--")
                    {
                        if (Session["SelectedActivity"] != null)
                            DDLSubTask3.SelectedValue = Session["SelectedActivity"].ToString();
                    }

                }

                if (!string.IsNullOrEmpty(TaskIDArray[5]))
                {
                    DDLSubTask4.SelectedValue = TaskIDArray[5];
                    Task5.Visible = true;

                    DataSet ds = dbgetdata.GetSubTask_By_ParentTask_Level(DDLSubTask4.SelectedValue, 7);
                    DDLSubTask5.DataTextField = "Name";
                    DDLSubTask5.DataValueField = "TaskUID";
                    DDLSubTask5.DataSource = ds;
                    DDLSubTask5.DataBind();
                    DDLSubTask5.Items.Insert(0, "--Select--");

                    if (TaskIDArray[5] == "--Select--")
                    {
                        if (Session["SelectedActivity"] != null)
                            DDLSubTask4.SelectedValue = Session["SelectedActivity"].ToString();
                    }
                }

                if (!string.IsNullOrEmpty(TaskIDArray[6]))
                {
                    DDLSubTask5.SelectedValue = TaskIDArray[6];
                    Task6.Visible = true;

                    DataSet ds = dbgetdata.GetSubTask_By_ParentTask_Level(DDLSubTask5.SelectedValue, 8);
                    DDLSubTask6.DataTextField = "Name";
                    DDLSubTask6.DataValueField = "TaskUID";
                    DDLSubTask6.DataSource = ds;
                    DDLSubTask6.DataBind();
                    DDLSubTask6.Items.Insert(0, "--Select--");

                    if (TaskIDArray[6] == "--Select--")
                    {
                        if (Session["SelectedActivity"] != null)
                            DDLSubTask5.SelectedValue = Session["SelectedActivity"].ToString();
                    }
                }

                if (!string.IsNullOrEmpty(TaskIDArray[7]))
                {
                    DDLSubTask6.SelectedValue = TaskIDArray[7];
                    Task7.Visible = true;

                    if (TaskIDArray[7] == "--Select--")
                    {
                        if (Session["SelectedActivity"] != null)
                            DDLSubTask6.SelectedValue = Session["SelectedActivity"].ToString();
                    }
                }
            }
            catch(Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert(" + ex.Message + ");</script>");
            }
           
        }



        internal void SelectedProject()
        {
            if (!IsPostBack)
            {
                if (Session["Project_Workpackage"] != null)
                {
                    string[] selectedValue = Session["Project_Workpackage"].ToString().Split('_');
                    if (selectedValue.Length > 1)
                    {
                        DDlProject.SelectedValue = selectedValue[0];
                    }
                    else
                    {
                        DDlProject.SelectedValue = Session["Project_Workpackage"].ToString();
                    }

                }
            }
            
        }

        private void HideButtons()
        {
            // added on 18/11/2020
            DataSet dscheck = new DataSet();
            dscheck = dbgetdata.GetUsertypeFunctionality_Mapping(Session["TypeOfUser"].ToString());
            AddTask.Visible = false;
            AddWorkPackage.Visible = false;
            AddSubTask.Visible = false;
            AddTaskMileStone.Visible = false;
            AddTaskResources.Visible = false;
            AddDependency.Visible = false;
            ViewState["Show"] = "false";
            if (dscheck.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dscheck.Tables[0].Rows)
                {

                    if (dr["Code"].ToString() == "FX") // PROJECT PROGRESS TRACKING
                    {
                        AddTask.Visible = true;
                        AddWorkPackage.Visible = true;
                        AddSubTask.Visible = true;
                        AddTaskMileStone.Visible = true;
                        AddTaskResources.Visible = true;
                        AddDependency.Visible = true;
                        ViewState["Show"] = "true";
                    }


                }
            }
            //
        }

       
        private void LoadProjectDetails(string ProjectUID)
        {
            DataSet ds = new DataSet();
            ds = dbgetdata.GetProject_by_ProjectUID(new Guid(ProjectUID));
            if (ds.Tables[0].Rows.Count > 0)
            {
                lblPrjName.Text = ds.Tables[0].Rows[0]["ProjectName"].ToString();
                //lblPrjOwner.Text = ds.Tables[0].Rows[0]["OwnerName"].ToString();
                lblStartDate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["StartDate"].ToString()).ToString("dd MMM yyyy");
                lblPlannedEndDate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["PlannedEndDate"].ToString()).ToString("dd MMM yyyy");
                lblPrjctedEndDate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["ProjectedEndDate"].ToString()).ToString("dd MMM yyyy");
                //lblPrjBudget.Text = ds.Tables[0].Rows[0]["Budget"].ToString();
                //lblPrjActaulExpenditure.Text = ds.Tables[0].Rows[0]["ActualExpenditure"].ToString();
                lblPrjStatus.Text = GetStatus(ds.Tables[0].Rows[0]["Status"].ToString());
                LblFundingAgency.Text = ds.Tables[0].Rows[0]["Funding_Agency"].ToString();
                LblCumulativeProgressProject.Text = dbgetdata.GetCumulativeProgress_Project(new Guid(ProjectUID)).ToString("0.###") + " %";
            }
        }

        protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
        {
        }

      
        public void BindActivities3()
        {
            HideButtons();

            SelectedTaskName = "";
          //  SelectedTaskUID = "";
            ViewState["SelectedTaskUID"] = "";

            DDLMainTask.Items.Clear();
            ClearAllSubTasksForward(0);

            AddTask.HRef = "/_modal_pages/add-task.aspx?type=Add&PrjUID=" + DDlProject.SelectedValue + "&wUID=" + DDLWorkPackage.SelectedValue;
            UploadSitePhotograph.HRef = "/_modal_pages/upload-sitephotograph.aspx?PrjUID=" + DDlProject.SelectedValue + "&WorkPackage=" + DDLWorkPackage.SelectedValue;
            ViewSitePhotograph.HRef = "/_modal_pages/view-sitephotographs.aspx?PrjUID=" + DDlProject.SelectedValue + "&WorkPackage=" + DDLWorkPackage.SelectedValue;

            //ActivityHeading.Text = "Option";
            //DataSet ds = gettk.GetTasks_by_WorkPackage(TreeView1.SelectedNode.Value);
            //GrdTreeView.DataSource = ds;
            //GrdTreeView.DataBind();
            GrdOptions.Visible = false;

            DataSet ds = dbgetdata.GetSelectedOption_By_WorkpackageUID(new Guid(DDLWorkPackage.SelectedValue));
            //DataSet ds = dbgetdata.GetWorkPackages_By_WorkPackageUID(new Guid(TreeView1.SelectedNode.Value));
            GrdOptions.DataSource = ds;
            GrdOptions.DataBind();

            GrdTreeView.Visible = false;
            ProjectDetails.Visible = false;
            WorkPackageDetils.Visible = true;
            GetWorkPackage_by_UID(new Guid(DDLWorkPackage.SelectedValue));
            AddTask.Visible = false;
            TaskDetails.Visible = false;
            Contractors.Visible = true;
            AddWorkPackage.Visible = false;
            AddSubTask.Visible = false;
            MileStoneList.Visible = true;
            ResourceAllocatedList.Visible = false;
            Dependencies.Visible = false;
            LoadTaskMileStones(DDLWorkPackage.SelectedValue);
            EnableOption.Visible = false;
            AddTaskMileStone.HRef = "/_modal_pages/add-milestone.aspx?TaskUID=" + DDLWorkPackage.SelectedValue;
            SortActivity.Visible = false;
            ResourceDeployment.Visible = true;
            LoadResourceDeployment(DDLWorkPackage.SelectedValue);
            //if (ds.Tables[0].Rows.Count > 0)
            //{
            //    SortActivity.Visible = true;
            //    SortActivity.HRef = "/_modal_pages/change-activityorder.aspx?WorkPackage=" + TreeView1.SelectedNode.Value;
            //}
            //else
            //{
            //    SortActivity.Visible = false;
            //}
            //if (ds.Tables[0].Rows.Count == 0)
            //{
            //    CopyActivityData.Visible = true;
            //    CopyActivityData.HRef = "/_modal_pages/copy-activitydata.aspx?WorkPackage=" + TreeView1.SelectedNode.Value;

            //}
            //else
            //{
            //    CopyActivityData.Visible = false;

            //}
        }

        public void BindActivities0()
        {
            HideButtons();

            SelectedTaskName = "";
            // SelectedTaskUID = "";

            ViewState["SelectedTaskUID"] = "";


            DDLMainTask.Items.Clear();
            ClearAllSubTasksForward(0);

            LoadProjectDetails(DDlProject.SelectedValue);
            ActivityHeading.Text = "Work Packages";
            DataSet ds = dbgetdata.GetWorkPackage_By_ProjectUID(new Guid(DDlProject.SelectedValue));
            GrdTreeView.DataSource = ds;
            GrdTreeView.DataBind();
            ProjectDetails.Visible = true;
            WorkPackageDetils.Visible = false;
            AddTask.Visible = false;
            TaskDetails.Visible = false;
            Contractors.Visible = false;
            if (ViewState["Show"].ToString() == "true")
            {
                AddWorkPackage.Visible = true;
            }
            AddWorkPackage.HRef = "/_modal_pages/add-workpackage.aspx?ProjectUID=" + DDlProject.SelectedValue;
            MileStoneList.Visible = true;
            AddSubTask.Visible = false;
            ResourceAllocatedList.Visible = false;
            LoadTaskMileStones(DDLWorkPackage.SelectedValue);
            AddTaskMileStone.HRef = "/_modal_pages/add-milestone.aspx?TaskUID=" + DDLWorkPackage.SelectedValue;
            AddSubTask.Visible = false;
            SortActivity.Visible = false;
            Dependencies.Visible = false;
            EnableOption.Visible = false;
            GrdOptions.Visible = false;
            GrdTreeView.Visible = true;
            ResourceDeployment.Visible = false;
            if (ds.Tables[0].Rows.Count == 0)
            {
                CopyProjectData.Visible = true;
                CopyProjectData.HRef = "/_modal_pages/copy-projectdata.aspx?ProjectUID=" + DDlProject.SelectedValue;
            }
            else
            {
                CopyProjectData.Visible = false;
            }
        }

        public void BindActivities1()
        {
            HideButtons();
            GrdOptions.Visible = false;
            GrdTreeView.Visible = true;
            AddTask.HRef = "/_modal_pages/add-task.aspx?type=Add&PrjUID=" + DDlProject.SelectedValue + "&wUID=" + DDLWorkPackage.SelectedValue + "&OptionUID=" + RBOptionList.SelectedValue;
            ActivityHeading.Text = "Activities";
            //DataSet ds = gettk.GetTasks_by_WorkPackage(TreeView1.SelectedNode.Parent.Value);
            DataSet ds = dbgetdata.GetTasks_by_WorkpackageOptionUID(new Guid(DDLWorkPackage.SelectedValue), new Guid(RBOptionList.SelectedValue));
            GrdTreeView.DataSource = ds;
            GrdTreeView.DataBind();
            ProjectDetails.Visible = false;
            WorkPackageDetils.Visible = false;
            //GetWorkPackage_by_UID(new Guid(TreeView1.SelectedNode.Value));

            if (ViewState["Show"].ToString() == "true")
            {
                AddTask.Visible = true;
            }

            TaskDetails.Visible = false;
            Contractors.Visible = false;
            AddWorkPackage.Visible = false;
            AddSubTask.Visible = false;
            MileStoneList.Visible = false;
            ResourceAllocatedList.Visible = false;
            Dependencies.Visible = false;
            LoadTaskMileStones(DDLWorkPackage.SelectedValue);
            EnableOption.Visible = false;
            AddTaskMileStone.HRef = "/_modal_pages/add-milestone.aspx?TaskUID=" + RBOptionList.SelectedValue;
            CopyActivityData.Visible = false;
            ResourceDeployment.Visible = false;
            if (ds.Tables[0].Rows.Count > 0)
            {
                SortActivity.Visible = true;
                SortActivity.HRef = "/_modal_pages/change-activityorder.aspx?WorkPackage=" + DDLWorkPackage.SelectedValue + "&OptionUID=" + RBOptionList.SelectedValue;
            }
            else
            {
                SortActivity.Visible = false;
            }
        }

        public void BindActivities2()
        {
            GrdOptions.Visible = false;
            ActivityHeading.Text = "Sub Activities";
            if (ViewState["SelectedTaskUID"] == null) return;
            DataSet ds = dbgetdata.GetTask_by_ParentTaskUID(new Guid(ViewState["SelectedTaskUID"].ToString()));
            GrdTreeView.DataSource = ds;
            GrdTreeView.DataBind();
            ProjectDetails.Visible = false;
            WorkPackageDetils.Visible = false;
            AddTask.Visible = false;
            AddWorkPackage.Visible = false;
            TaskDetails.Visible = true;
            GetTask_by_UID(ViewState["SelectedTaskUID"].ToString());
            Contractors.Visible = false;
            if (ViewState["Show"].ToString() == "true")
            {
                AddSubTask.Visible = true;
            }
            MileStoneList.Visible = true;
            ResourceAllocatedList.Visible = true;
            Dependencies.Visible = true;
            CopyActivityData.Visible = false;
            CopyProjectData.Visible = false;
            EnableOption.Visible = false;
            GrdTreeView.Visible = true;
            ResourceDeployment.Visible = false;
            if (ds.Tables[0].Rows.Count > 0)
            {
                SortActivity.Visible = true;
                SortActivity.HRef = "/_modal_pages/change-activityorder.aspx?TaskUID=" + ViewState["SelectedTaskUID"].ToString();
            }
            else
            {
                SortActivity.Visible = false;
            }
        }

        protected void GrdTreeView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string UID = e.CommandArgument.ToString();

            if (e.CommandName == "view")
            {
                //for (int i = 0; i < TreeView1.Nodes.Count; i++)
                //{
                   // retrieveNodes(TreeView1.Nodes[i], UID);
                //}
            }

            if (e.CommandName == "delete")
            {
                //if (TreeView1.SelectedNode.Target == "Class")
                //{
                //    //Delete whole project data
                //}
                //else if (TreeView1.SelectedNode.Target == "Project")
                //{
                //    int cnt = dbgetdata.Workpackage_Delete(new Guid(UID), new Guid(Session["UserUID"].ToString()));
                //    if (cnt > 0)
                //    {
                //        BindActivities();
                //    }
                //}
                //else
                //{
                //    int cnt = dbgetdata.Tasks_Delete_by_TaskUID(new Guid(UID), new Guid(Session["UserUID"].ToString()));
                //    if (cnt > 0)
                //    {
                //        BindActivities();
                //    }
                //}

                int cnt = dbgetdata.Tasks_Delete_by_TaskUID(new Guid(UID), new Guid(Session["UserUID"].ToString()));
               
                if (cnt > 0)
                {
                    // BindActivities();

                    if (Session["CurrentSelection"].ToString() == "WorkPackage")
                    {
                        RBOptionList_SelectedIndexChanged(sender, e);
                        BindActivities1();
                    }
                    else
                    {
                        
                        BindActivities2();
                        DisplayTasks();
                    }

                   
                }

            }
            if (e.CommandName == "up")
            {
                //if (TreeView1.SelectedNode.Target == "Project")
                //{
                //    int Cnt = dbgetdata.Workpackge_Order_Update(new Guid(UID), "Up");
                //    if (Cnt > 0)
                //    {
                //        string TreeSelectedValue = TreeView1.SelectedNode.Value;
                //        BindTreeview();
                //        //for (int i = 0; i < TreeView1.Nodes.Count; i++)
                //        //{
                //        //    retrieveNodes(TreeView1.Nodes[i], TreeSelectedValue);
                //        //}
                //        BindActivities();
                //    }
                //}
                //else
                //{
                //    int Cnt = dbgetdata.Task_Order_Update(new Guid(UID), "Up");
                //    if (Cnt > 0)
                //    {
                //        string TreeSelectedValue = TreeView1.SelectedNode.Value;
                //        BindTreeview();
                //        for (int i = 0; i < TreeView1.Nodes.Count; i++)
                //        {
                //            retrieveNodes(TreeView1.Nodes[i], TreeSelectedValue);
                //        }
                //        BindActivities();
                //    }
                //}
            }
            if (e.CommandName == "down")
            {
                //if (TreeView1.SelectedNode.Target == "Project")
                //{
                //    int Cnt = dbgetdata.Workpackge_Order_Update(new Guid(UID), "Down");
                //    if (Cnt > 0)
                //    {
                //        string TreeSelectedValue = TreeView1.SelectedNode.Value;
                //        BindTreeview();
                //        //for (int i = 0; i < TreeView1.Nodes.Count; i++)
                //        //{
                //        //    retrieveNodes(TreeView1.Nodes[i], TreeSelectedValue);
                //        //}
                //        BindActivities();
                //    }
                //}
                //else
                //{
                //    int Cnt = dbgetdata.Task_Order_Update(new Guid(UID), "Down");
                //    if (Cnt > 0)
                //    {
                //        string TreeSelectedValue = TreeView1.SelectedNode.Value;
                //        BindTreeview();
                //        for (int i = 0; i < TreeView1.Nodes.Count; i++)
                //        {
                //            retrieveNodes(TreeView1.Nodes[i], TreeSelectedValue);
                //        }
                //        BindActivities();
                //    }
                //}
            }
        }

        protected void GrdTreeView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdTreeView.PageIndex = e.NewPageIndex;

            if (Session["CurrentSelection"].ToString() == "WorkPackage")
            {
                BindActivities1();
            }
            else
            {
                BindActivities2();
            }

           

            //DataSet ds = dbgetdata.GetTasks_by_ProjectUID(new Guid(TreeView1.SelectedNode.Value));
            //GrdTreeView.DataSource = ds;
            //GrdTreeView.DataBind();

        }

        protected void GrdTreeView_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //if (TreeView1.SelectedNode.Target == "Class")
                //{
                //    e.Row.Cells[4].Visible = false;
                //    GrdTreeView.HeaderRow.Cells[4].Visible = false;
                //    e.Row.Cells[5].Visible = false;
                //    GrdTreeView.HeaderRow.Cells[5].Visible = false;
                //}
                //else if (TreeView1.SelectedNode.Target == "Project")
                //{
                //    e.Row.Cells[4].Visible = false;
                //    //e.Row.Cells[5].Visible = false;
                //    //e.Row.Cells[6].Visible = false;
                //    GrdTreeView.HeaderRow.Cells[4].Visible = false;
                //    //GrdTreeView.HeaderRow.Cells[5].Visible = false;
                //    //GrdTreeView.HeaderRow.Cells[6].Visible = false;
                //}
                //if (TreeView1.SelectedNode.Target == "Tasks")
                //{
                //    //e.Row.Cells[6].Visible = false;
                //    //GrdTreeView.HeaderRow.Cells[6].Visible = false;
                //}
            }
        }

        protected void GrdTreeView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        public void GetWorkPackage_by_UID(Guid WorkPackageID)
        {
            DataSet ds = dbgetdata.GetWorkPackages_By_WorkPackageUID(WorkPackageID);
            if (ds.Tables[0].Rows.Count > 0)
            {
                LblWorkPackageName.Text = ds.Tables[0].Rows[0]["Name"].ToString();
                LblWorkPackageLocation.Text = ds.Tables[0].Rows[0]["WorkPackage_Location"].ToString();
                LblWorkPackageClient.Text = ds.Tables[0].Rows[0]["WorkPackage_Client"].ToString();
                LblWorkPackageStatus.Text = GetStatus(ds.Tables[0].Rows[0]["Status"].ToString());
                LblCumulativeProgressWorkpackage.Text = dbgetdata.GetCumulativeProgress_Workpackage(WorkPackageID).ToString("0.###") + " %";
                //LblWorkpackageCurrency.Text = ds.Tables[0].Rows[0]["Currency"].ToString();
                //LblWorkPackageBudget.Text =  Convert.ToDouble(ds.Tables[0].Rows[0]["Budget"].ToString()).ToString("#,##.00", CultureInfo.CreateSpecificCulture(ds.Tables[0].Rows[0]["Currency_CultureInfo"].ToString()));
                lblWkpgStartDate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["StartDate"].ToString()).ToString("dd MMM yyyy");
                lblWkpgEndDate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["PlannedEndDate"].ToString()).ToString("dd MMM yyyy");
                if (ds.Tables[0].Rows[0]["Contractor_UID"].ToString() != "")
                {
                    GetContractors(new Guid(ds.Tables[0].Rows[0]["Contractor_UID"].ToString()));
                }

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Workpackage_OptionUID"].ToString()))
                {
                    if (dbgetdata.GetWorkpackageOption_Order(new Guid(ds.Tables[0].Rows[0]["Workpackage_OptionUID"].ToString())) == 4)
                    {
                        CopyActivityData.Visible = true;
                      //  CopyActivityData.HRef = "/_modal_pages/copy-activitydata.aspx?WorkPackage=" + TreeView1.SelectedNode.Value;
                    }
                    else
                    {
                        CopyActivityData.Visible = false;
                    }
                }
            }
        }

        public void GetContractors(Guid ContractUID)
        {
            DataSet ds = dbgetdata.GetContractors_by_ContractorUID(ContractUID);
            if (ds.Tables[0].Rows.Count > 0)
            {
                LblContractor.Text = ds.Tables[0].Rows[0]["Contractor_Name"].ToString();
                LblContract_Type.Text = ds.Tables[0].Rows[0]["Type_of_Contract"].ToString();
                //LblContract_Representatives.Text = ds.Tables[0].Rows[0]["Contractor_Representatives"].ToString();
                LblContract_Duration.Text = GetStatus(ds.Tables[0].Rows[0]["Contract_Duration"].ToString()) + " Months";

                LBLProjectNumber.Text = WebConfigurationManager.AppSettings["Domain"] + " Project Number";

                LblNJSEIProjectNumber.Text = ds.Tables[0].Rows[0]["NJSEI_Number"].ToString();
                LblProjectSpecificPackageNumber.Text = ds.Tables[0].Rows[0]["ProjectSpecific_Number"].ToString();
                if (ds.Tables[0].Rows[0]["Contract_StartDate"].ToString() != "")
                {
                    LblContract_StartDate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["Contract_StartDate"].ToString()).ToString("dd MMM yyyy");
                }
                if (ds.Tables[0].Rows[0]["Contract_Completion_Date"].ToString() != "")
                {
                    LblContract_CompletionDate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["Contract_Completion_Date"].ToString()).ToString("dd MMM yyyy");
                }

            }
        }

        public void GetTask_by_UID(string TaskUID)
        {
            DataTable ds = dbgetdata.GetTaskDetails_TaskUID(TaskUID);
         
            if (ds.Rows.Count > 0)
            {
                LblTaskName.Text = ds.Rows[0]["Name"].ToString();
                LblTaskDescription.Text = ds.Rows[0]["Description"].ToString();
                if (ds.Rows[0]["StartDate"].ToString() != "")
                {
                    LblTaskStartDate.Text = Convert.ToDateTime(ds.Rows[0]["StartDate"].ToString()).ToString("dd MMM yyyy");
                }
                else
                {
                    LblTaskStartDate.Text = "";
                }
                if (ds.Rows[0]["PlannedEndDate"].ToString() != "")
                {
                    LblTaskPlannedEndDate.Text = Convert.ToDateTime(ds.Rows[0]["PlannedEndDate"].ToString()).ToString("dd MMM yyyy");
                }
                else
                {
                    LblTaskPlannedEndDate.Text = "";
                }
                //LblTaskCurrency.Text = ds.Rows[0]["Currency"].ToString();
                //LblTaskBudget.Text = Convert.ToDouble(ds.Rows[0]["Total_Budget"].ToString()).ToString("#,##.00", CultureInfo.CreateSpecificCulture(ds.Rows[0]["Currency_CultureInfo"].ToString()));
                LblTaskStatus.Text = GetStatus(ds.Rows[0]["Status"].ToString());

                if (ds.Rows[0]["UnitforProgress"].ToString() != "")
                {
                    MeasurementDetails.Visible = true;
                    LblMeasurementUnit.Text = ds.Rows[0]["UnitforProgress"].ToString();
                    LblMeasurementTotalQuantity.Text= ds.Rows[0]["UnitQuantity"].ToString();
                    //double CumulativeQuan = dbgetdata.GetMeasurementCumulativeQuantity(new Guid(TaskUID));
                    if (ds.Rows[0]["CumulativeAchvQuantity"] != DBNull.Value)
                    {
                        if (ds.Rows[0]["ParentTaskID"] != DBNull.Value)
                        {
                            if (dbgetdata.GetTaskCount_by_ParentTaskID(new Guid(ds.Rows[0]["ParentTaskID"].ToString())) > 0)
                            {
                                if (ds.Rows[0]["UnitforProgress"].ToString().ToLower() == "percentage")
                                {
                                    LblMeasurementCumulativeQuantity.Text = ds.Rows[0]["Task_CulumativePercentage"].ToString() == "" ? "0 %" : Convert.ToDecimal(ds.Rows[0]["Task_CulumativePercentage"].ToString()).ToString("0.###");
                                }
                                else
                                {
                                    LblMeasurementCumulativeQuantity.Text = ds.Rows[0]["CumulativeAchvQuantity"].ToString();
                                }
                            }
                            else
                            {
                                LblMeasurementCumulativeQuantity.Text = ds.Rows[0]["CumulativeAchvQuantity"].ToString();
                            }
                        }
                        else
                        {
                            LblMeasurementCumulativeQuantity.Text = ds.Rows[0]["CumulativeAchvQuantity"].ToString();
                        }
                    }
                    else
                    {
                        LblMeasurementCumulativeQuantity.Text = "0";
                    }
                }
                else
                {
                    MeasurementDetails.Visible = false;
                }
                LblCumulativePercentage.Text = ds.Rows[0]["Task_CulumativePercentage"].ToString() == "" ? "0 %" : Convert.ToDecimal(ds.Rows[0]["Task_CulumativePercentage"].ToString()).ToString("0.###") + " %";
                LoadTaskMileStones(TaskUID);
                BindDependencies(TaskUID);
                AddTaskMileStone.HRef = "/_modal_pages/add-milestone.aspx?TaskUID=" + TaskUID;
                AddTaskResources.HRef = "/_modal_pages/add-resourceallocated.aspx?TaskUID=" + TaskUID + "&ProjectUID=" + ds.Rows[0]["ProjectUID"].ToString() + "&WorkPackageUID=" + ds.Rows[0]["WorkPackageUID"].ToString();
                AddDependency.HRef = "/_modal_pages/add-dependency.aspx?type=add&TaskUID=" + TaskUID + "&WorkUID=" + ds.Rows[0]["WorkPackageUID"].ToString();
                LoadResourceAllocated(TaskUID);
               AddSubTask.HRef = "/_modal_pages/add-subtask.aspx?type=add&ParentTaskUID=" + TaskUID + "&PrjUID=" + ds.Rows[0]["ProjectUID"].ToString() + "&WrkUID=" + ds.Rows[0]["WorkPackageUID"].ToString() + "&OptionUID="+ ds.Rows[0]["Workpackage_Option"].ToString();
                TaskSchedule.HRef= "/_modal_pages/addtask-targetschedule.aspx?type=add&TaskUID=" + TaskUID + "&WorkUID=" + ds.Rows[0]["WorkPackageUID"].ToString();
            }
        }

        private void LoadTaskMileStones(string TaskUID)
        {
            grdMileStones.DataSource = dbgetdata.getTaskMileStones(new Guid(TaskUID));
            grdMileStones.DataBind();
        }

        private void LoadResourceAllocated(string TaskUID)
        {
           grdResourceAllocated.DataSource = dbgetdata.getTaskResourceAllocated(new Guid(TaskUID));
           grdResourceAllocated.DataBind();
        }

        protected void grdResourceAllocated_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.Cells[5].Text != "&nbsp;")
                {
                    DataSet ds = dbgetdata.GetUserResource_by_TaskUID_AllocationUID(new Guid(e.Row.Cells[7].Text), new Guid(e.Row.Cells[5].Text));
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        e.Row.Cells[5].Text = ds.Tables[0].Rows[0]["Usage"].ToString();
                    }
                    else
                    {
                        e.Row.Cells[5].Text = "0";
                    }
                    //Label lblgst = (Label)e.Row.FindControl("LblGST");
                    //DataSet res = dbgetdata.getResourceMasterDetails(new Guid(e.Row.Cells[7].Text));
                    //DataSet res= dbgetdata.GetBOQDetails_by_BOQDetailsUID(new Guid(DDLBOQDetails.SelectedValue));
                    //if (res.Tables[0].Rows.Count > 0)
                    //{
                    //    lblgst.Text = res.Tables[0].Rows[0]["GST"].ToString() + "%";
                    //}
                    //else
                    //{
                    //    lblgst.Text = "0";
                    //}
                }
            }
        }

        protected void grdMileStones_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.Cells[6].Text != "&nbsp;")
                {
                    DateTime dt = Convert.ToDateTime(e.Row.Cells[6].Text);
                    if (dt.Date < DateTime.Now.Date && e.Row.Cells[4].Text == "Not Completed")
                    {
                        e.Row.Cells[3].BackColor = System.Drawing.Color.Red;
                        e.Row.Cells[3].ForeColor = System.Drawing.Color.White;
                    }
                }
                if (e.Row.Cells[4].Text == "Completed")
                {
                    e.Row.Cells[4].ForeColor = System.Drawing.Color.Green;
                }
            }
        }

        public string LimitCharts(string Desc)
        {
            if (Desc.Length > 120)
            {
                return Desc.Substring(0, 120) + "  . . .";
            }
            else
            {
                return Desc;
            }
        }

        public string ShoworHide(string Desc)
        {
            if (Desc.Length > 45)
            {
                return "More";
            }
            else
            {
                return string.Empty;
            }
        }
        

        public string GetStatus(string StatusCode)
        {
            string retStatus = StatusCode;
            if (StatusCode == "I")
            {
                retStatus = "In-Progress";
            }
            else if (StatusCode == "P")
            {
                retStatus = "Not Started";
            }
            else if (StatusCode == "C")
            {
                retStatus = "Completed";

            }
            return retStatus;
        }

       
        TreeNode SearchNode(TreeNode node, string searchText)
        {
            if (node.Text.Contains(searchText)) return node;

            TreeNode tn = null;
            foreach (TreeNode childNode in node.ChildNodes)
            {
                tn = SearchNode(childNode, searchText);
                if (tn != null) break;
            }

            if (tn != null) node.Expand();
            return tn;
        }

        public string getTaskName(string TaskUID)
        {
            return dbgetdata.getTaskNameby_TaskUID(new Guid(TaskUID));
        }

        protected void BindDependencies(string TaskUID)
        {
            DataSet ds = dbgetdata.getDependencies(new Guid(TaskUID));
            GrdDependency.DataSource = ds;
            GrdDependency.DataBind();
        }
        public string DependencyType(string dCode)
        {
            if (dCode == "FS")
            {
                return "Finish to Start";
            }
            else if (dCode == "SF")
            {
                return "Start to Finish";
            }
            else if (dCode == "SS")
            {
                return "Start to Start";
            }
            else
            {
                return "Finish to Finish";
            }
        }

        protected void btnEnableOption_Click(object sender, EventArgs e)
        {
            //DataSet ds = dbgetdata.WorkpackageSelectedOptions_by_UID(new Guid(TreeView1.SelectedNode.Value));
            //if (ds.Tables[0].Rows.Count > 0)
            //{
            //    CopyMasterActivityData(new Guid(ds.Tables[0].Rows[0]["WorkPackageUID"].ToString()), new Guid(ds.Tables[0].Rows[0]["Workpackage_OptionUID"].ToString()), new Guid(DDlProject.SelectedValue), new Guid(TreeView1.SelectedNode.Value));
            //    int cnt = dbgetdata.WorkpackageSelectedOption_Enabled(new Guid(TreeView1.SelectedNode.Value));
            //    if (cnt > 0)
            //    {
            //        // Session["SelectedActivity"] = TreeView1.SelectedNode.Value;
            //        Response.Redirect(Request.Url.AbsoluteUri);
            //    }
            //}
        }

        private void CopyMasterActivityData(Guid WorkpackageUID, Guid OptionUID, Guid ProjectUID,Guid SelectedOptionUID)
        {
            DataSet ds = dbgetdata.WorkpackageMainActivityMaster_SelectBy_OptionUID(OptionUID);
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Guid TaskUID = Guid.NewGuid();
                    try
                    {

                        bool result = dbgetdata.InsertorUpdateMainTask_From_Master(TaskUID, WorkpackageUID, ProjectUID, SelectedOptionUID, Session["UserUID"].ToString(), ds.Tables[0].Rows[i]["WorkpakageActivity_Name"].ToString(), ds.Tables[0].Rows[i]["WorkpakageActivity_Name"].ToString(), "P", 0, 0, 1, 0, 0, 0, "&#x20B9;", "en-IN", Convert.ToInt32(ds.Tables[0].Rows[i]["WorkpakageActivity_Order"].ToString()));
                        if (result)
                        {
                            TaskDataInsert(WorkpackageUID, new Guid(ds.Tables[0].Rows[i]["WorkpakageActivity_MasterUID"].ToString()), TaskUID, ProjectUID, SelectedOptionUID);
                        }
                        else
                        {
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code AWPM-01 there is a problem with this feature. Please contact system admin.');</script>");
                        }
                    }
                    catch (Exception ex)
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code AWPM-01 there is a problem with this feature. Please contact system admin.');</script>");
                    }

                }
            }
        }
        private void TaskDataInsert(Guid WorkpackageUID, Guid FromParentTaskUID, Guid ToParentTaskUID,Guid ProjectUID,Guid SelectedOptionUID)
        {
            DataSet ds = dbgetdata.WorkpackageActivityMaster_SelectBy_ParentUID(FromParentTaskUID);
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Guid sSubTaskUID = Guid.NewGuid();
                    try
                    {

                        bool result = dbgetdata.InsertorUpdateSubTask_From_Master(sSubTaskUID, WorkpackageUID, ProjectUID, SelectedOptionUID, Session["UserUID"].ToString(), ds.Tables[0].Rows[i]["WorkpakageActivity_Name"].ToString(), ds.Tables[0].Rows[i]["WorkpakageActivity_Name"].ToString(), "P", 0, 0, Convert.ToInt32(ds.Tables[0].Rows[i]["WorkpakageActivity_Level"].ToString()), 0, 0, 0, "&#x20B9;", "en-IN", Convert.ToInt32(ds.Tables[0].Rows[i]["WorkpakageActivity_Order"].ToString()), ToParentTaskUID.ToString());
                        if (result)
                        {
                            TaskDataInsert(WorkpackageUID, new Guid(ds.Tables[0].Rows[i]["WorkpakageActivity_MasterUID"].ToString()), sSubTaskUID, ProjectUID, SelectedOptionUID);
                        }
                    }
                    catch (Exception ex)
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code SAWPM-01 there is a problem with this feature. Please contact system admin.');</script>");
                    }

                }
            }
        }

        public string GetWorkpackageOptionName(string Workpackage_OptionUID)
        {
            return dbgetdata.WorkpackageoptionName_SelectBy_UID(new Guid(Workpackage_OptionUID));
        }

        protected void GrdOptions_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string UID = e.CommandArgument.ToString();
            if (e.CommandName == "view")
            {
                //TreeView1.CollapseAll();
                //DataSet ds = dbgetdata.GetWorkpackage_SelectOption_by_UID(new Guid(UID));
                //if (ds.Tables[0].Rows.Count > 0)
                //{
                //    for (int i = 0; i < TreeView1.Nodes.Count; i++)
                //    {
                //       // retrieveNodes_Options(TreeView1.Nodes[i], ds.Tables[0].Rows[0]["Workpackage_OptionUID"].ToString(), ds.Tables[0].Rows[0]["WorkPackageUID"].ToString());
                //    }
                //}
                
                //DataSet ds = dbgetdata.GetWorkPackages_By_WorkPackageUID(new Guid(UID));
                //if (ds.Tables[0].Rows.Count > 0)
                //{
                //    for (int i = 0; i < TreeView1.Nodes.Count; i++)
                //    {
                //        retrieveNodes_by_Parent(TreeView1.Nodes[i], ds.Tables[0].Rows[0]["Workpackage_OptionUID"].ToString(), UID);
                //    }
                //}

            }
        }

       
        protected void grdMileStones_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string UID = e.CommandArgument.ToString();
            if (e.CommandName == "delete")
            {
                int cnt = dbgetdata.MileStone_Delete(new Guid(UID), new Guid(Session["UserUID"].ToString()));

                if (cnt > 0)
                {

                    if (Session["CurrentSelection"].ToString() == "Task")
                        BindActivities3();
                    else
                    {
                        Session["CurrentSelection"] = "";
                        Response.Redirect("default.aspx");
                    }
                }
            }
        }

        protected void grdMileStones_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        protected void grdResourceAllocated_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string UID = e.CommandArgument.ToString();
            if (e.CommandName == "delete")
            {
                int cnt = dbgetdata.ResourceAllocation_Delete(new Guid(UID), new Guid(Session["UserUID"].ToString()));
                if (cnt > 0)
                {
                   // LoadResourceAllocated(TreeView1.SelectedNode.Value);
                }
            }
        }

        protected void grdResourceAllocated_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        protected void GrdDependency_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string dUID = e.CommandArgument.ToString();
            if (e.CommandName == "delete")
            {
                int cnt = dbgetdata.Dependency_Delete(new Guid(dUID), new Guid(Session["UserUID"].ToString()));
                if (cnt > 0)
                {

                    if (Session["CurrentSelection"].ToString() == "Task")
                        BindActivities3();
                    else
                    {
                        Session["CurrentSelection"] = "";
                        Response.Redirect("default.aspx");
                    }
                        
                }
            }
        }

        protected void GrdDependency_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        private void LoadResourceDeployment(string WorkpackageUID)
        {
            GrdResourceDeployment.DataSource = dbgetdata.getResourceMaster(new Guid(WorkpackageUID));
            GrdResourceDeployment.DataBind();
        }
    }
}