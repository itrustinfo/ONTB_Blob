using ProjectManager.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManagementTool._modal_pages
{
    public partial class add_treenodemaster : System.Web.UI.Page
    {
        DBGetData getdt = new DBGetData();
        TaskUpdate gettk = new TaskUpdate();
        private static string SelectedTaskUID = string.Empty;
        private static string SelectedTaskName = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Username"] == null)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
            }
            else
            {
                if (!IsPostBack)
                {
                    BindProject();
                    DDlProject_SelectedIndexChanged(sender, e);
                }
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
                   
                    Session["Project_Workpackage"] = DDlProject.SelectedValue + "_" + DDLWorkPackage.SelectedValue;
                    SelectedTaskUID = string.Empty;
                    btnSubmit.Visible = false;
                    OptionBind(DDLWorkPackage.SelectedValue);

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
                    
                }
                else if (Session["TypeOfUser"].ToString() == "DDE" || Session["TypeOfUser"].ToString() == "DE")
                {
                    RBOptionList.Items.RemoveAt(2);
                }
                //
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
        protected void RBOptionList_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            SelectedTaskUID = string.Empty;
            btnSubmit.Visible = true;
            
            //added on 02/11/2022 for correspondence
            string option = RBOptionList.SelectedValue;
            if (RBOptionList.SelectedIndex == 2)
                option = RBOptionList.Items[1].Value;
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
            DDlProject.SelectedValue = Request.QueryString["projectuid"].ToString();

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
        

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                string taskNode = "";
                if(MainTask.Visible)
                {
                    taskNode += DDLMainTask.SelectedItem.ToString();
                }
                if (Task1.Visible)
                {
                    taskNode += "->"+ DDLSubTask.SelectedItem.ToString();
                }
                if (Task2.Visible)
                {
                    taskNode += "->" + DDLSubTask1.SelectedItem.ToString();
                }
                if (Task3.Visible)
                {
                    taskNode += "->" + DDLSubTask2.SelectedItem.ToString();
                }
                if (Task4.Visible)
                {
                    taskNode += "->" + DDLSubTask3.SelectedItem.ToString();
                }
                if (Task5.Visible)
                {
                    taskNode += "->" + DDLSubTask4.SelectedItem.ToString();
                }
                if (Task6.Visible)
                {
                    taskNode += "->" + DDLSubTask5.SelectedItem.ToString();
                }
                if (Task7.Visible)
                {
                    taskNode += "->" + DDLSubTask6.SelectedItem.ToString();
                }
               int cnt= getdt.InsertformTaskData(SelectedTaskUID, taskNode, DDlProject.SelectedValue.ToString(), DDLWorkPackage.SelectedValue.ToString());
                if (cnt > 0)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
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