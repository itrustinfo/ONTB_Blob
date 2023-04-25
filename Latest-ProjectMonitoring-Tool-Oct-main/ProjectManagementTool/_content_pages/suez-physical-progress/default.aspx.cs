using ProjectManager.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManagementTool._content_pages.suez_physical_progress
{
    public partial class _default : System.Web.UI.Page
    {
        TaskUpdate TKUpdate = new TaskUpdate();
        DBGetData getdata = new DBGetData();
        DataSet dsPayment = new DataSet();
        InventoryCS invobj = new InventoryCS();

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
                    
                    LoadProjects();
                    SelectedProjectWorkpackage("Project");
                    ddlProject_SelectedIndexChanged(sender, e);

                    //dtMeetingDate.Text = DateTime.Now.ToString();
                    BindTaskScheduleData(ddlProject.SelectedValue.ToString());
                    divStatusMonth.Visible = true;
                    AddData.HRef = "~/_modal_pages/add-treenodemaster.aspx?projectuid=" + ddlProject.SelectedValue;
                }


            }
            
        }
        private void BindTaskScheduleData(string projectuid)
        {
            if(dtMeetingDate.Text == string.Empty)
            {
                dtMeetingDate.Text= DateTime.Now.ToString("dd/MM/yyyy");
            }
            string sDate1 = "";
            DateTime selectedDate = DateTime.Now;
            sDate1 = getdata.ConvertDateFormat(dtMeetingDate.Text);
            selectedDate = Convert.ToDateTime(sDate1);
            //dtMeetingDate.Text = selectedDate.ToString();
            try
            {
                DataTable ds = getdata.GetTaskScheduleData(new Guid(projectuid), selectedDate).Tables[0].AsEnumerable().Where(r => r.Field<string>("projectuid").ToString().Equals(projectuid)).CopyToDataTable();
                GrdDocStatus.DataSource = ds;
                GrdDocStatus.DataBind();
            }
            catch(Exception ex)
            {
                DataTable ds = null;
                GrdDocStatus.DataSource = ds;
                GrdDocStatus.DataBind();
            }

        }

        private void LoadProjects()
        {
            try
            {
                DataTable ds = new DataTable();
                if (Session["TypeOfUser"].ToString() == "U" || Session["TypeOfUser"].ToString() =="MD" || Session["TypeOfUser"].ToString() == "VP")
                {
                    ds = TKUpdate.GetProjects();
                }
                else if (Session["TypeOfUser"].ToString() == "PA")
                {
                    //ds = TKUpdate.GetProjects_by_UserUID(new Guid(Session["UserUID"].ToString()));
                    ds = TKUpdate.GetAssignedProjects_by_UserUID(new Guid(Session["UserUID"].ToString()));
                }
                else
                {
                    //ds = TKUpdate.GetProjects();
                    ds = TKUpdate.GetAssignedProjects_by_UserUID(new Guid(Session["UserUID"].ToString()));
                }
                ddlProject.DataSource = ds;
                ddlProject.DataTextField = "ProjectName";
                ddlProject.DataValueField = "ProjectUID";
                ddlProject.DataBind();
            }
            catch (Exception ex)
            {
            }
        }
        protected void ddlProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlProject.SelectedValue != "")
            {
                
                //divStatus.Visible = false;
                divStatusMonth.Visible = true;
                DataSet ds = new DataSet();
                if (Session["TypeOfUser"].ToString() == "U" || Session["TypeOfUser"].ToString() == "MD" || Session["TypeOfUser"].ToString() == "VP")
                {
                    ds = getdata.GetWorkPackages_By_ProjectUID(new Guid(ddlProject.SelectedValue));
                }
                else if (Session["TypeOfUser"].ToString() == "PA")
                {
                    ds = getdata.GetWorkPackages_ForUser_by_ProjectUID(new Guid(Session["UserUID"].ToString()), new Guid(ddlProject.SelectedValue));
                }
                else
                {
                    ds = getdata.GetWorkPackages_ForUser_by_ProjectUID(new Guid(Session["UserUID"].ToString()), new Guid(ddlProject.SelectedValue));
                }
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddlworkpackage.DataTextField = "Name";
                    ddlworkpackage.DataValueField = "WorkPackageUID";
                    ddlworkpackage.DataSource = ds;
                    ddlworkpackage.DataBind();
                    SelectedProjectWorkpackage("Workpackage");

                    AddData.HRef = "~/_modal_pages/add-suez_physical_progress.aspx?projectuid=" + ddlProject.SelectedValue;



                    Session["Project_Workpackage"] = ddlProject.SelectedValue + "_" + ddlworkpackage.SelectedValue;

                    Session["ProjectUID"] = ddlProject.SelectedValue;
                    Session["WorkPackageUID"] = ddlworkpackage.SelectedValue;
                    //
                    BindTaskScheduleData(ddlProject.SelectedValue.ToString());
                }
            }
            
        }

  
        protected void ddlworkpackage_SelectedIndexChanged(object sender, EventArgs e)
        {
          
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
                        ddlProject.SelectedValue = selectedValue[0];
                    }
                    else
                    {
                        ddlworkpackage.SelectedValue = selectedValue[1];
                    }
                }
                else
                {
                    if (pType == "Project")
                    {
                        ddlProject.SelectedValue = Session["Project_Workpackage"].ToString();
                    }
                }
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



        protected void dtMeetingDate_TextChanged(object sender, EventArgs e)
        {

            
                BindTaskScheduleData(ddlProject.SelectedValue.ToString());
            
        }

        //protected void btnSubmit_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        string taskuid = "";
        //        DateTime selectedDate = DateTime.Now;
        //      string  sDate1 = getdata.ConvertDateFormat(dtMeetingDate.Text);
        //        selectedDate = Convert.ToDateTime(sDate1);
        //        int cnt = 0;
        //        for (int i=0;i<GrdDocStatus.Rows.Count;i++)
        //        {
        //            taskuid=GrdDocStatus.DataKeys[i].Value.ToString();
        //            TextBox taskAchivedValue = GrdDocStatus.Rows[i].FindControl("txtTaskValue") as TextBox;
        //            if (Double.TryParse(taskAchivedValue.Text, out double value))
        //            {
        //               cnt+= getdata.Insert_Task_Achived_Value(taskuid, value, ddlProject.SelectedValue.ToString(), ddlworkpackage.SelectedValue.ToString(),selectedDate);
        //             }
        //            LblMessage.Text = cnt + " Rows updated";

        //        }
        //    }
        //    catch(Exception ex)
        //    {

        //    }
        //}


        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                string taskuid = "";
                DateTime selectedDate = DateTime.Now;
                string sDate1 = getdata.ConvertDateFormat(dtMeetingDate.Text);
                selectedDate = Convert.ToDateTime(sDate1);
                int cnt = 0;
                string sUnit = "";
                LblMessage.Text = "";
                for (int i = 0; i < GrdDocStatus.Rows.Count; i++)
                {
                    try
                    {
                        taskuid = GrdDocStatus.DataKeys[i].Value.ToString();
                        TextBox taskAchivedValue = GrdDocStatus.Rows[i].FindControl("txtTaskValue") as TextBox;
                        Label lblTaskName = GrdDocStatus.Rows[i].FindControl("lblTaskName") as Label;
                        if (Double.TryParse(taskAchivedValue.Text, out double achived_value))
                        {
                            DataSet dsTaskDetails = getdata.GetTaskDetails(taskuid);
                            if (Double.TryParse(dsTaskDetails.Tables[0].Rows[0]["UnitQuantity"].ToString(), out double totalQuantity))
                            {
                                if (achived_value < 0)
                                {
                                    LblMessage.Text += lblTaskName.Text + "(Negative),";
                                }
                                else if (achived_value == 0)
                                {
                                    LblMessage.Text += lblTaskName.Text + "(Zero),";
                                }
                                else if (achived_value > totalQuantity)
                                {
                                    LblMessage.Text += lblTaskName.Text + "(More than total quantity),";
                                }
                                else
                                {
                                    if (dsTaskDetails.Tables[0].Rows.Count > 0)
                                    {
                                        sUnit = dsTaskDetails.Tables[0].Rows[0]["UnitforProgress"].ToString();

                                        if (sUnit == null)
                                        {
                                            sUnit = "RM";
                                        }

                                       //cnt += getdata.InsertorUpdateTaskMeasurementBook(Guid.NewGuid(), new Guid(taskuid), sUnit, achived_value.ToString(), "", DateTime.Now, "", new Guid(Session["UserUID"].ToString()), "", selectedDate);
                                       //changed by zuber ...calling now NOn BOQ update
                                        cnt += getdata.InsertMeasurementBookWithoutTaskGrouping_formUpdate(Guid.NewGuid(), new Guid(taskuid), sUnit, achived_value.ToString(), "", selectedDate, "", new Guid(Session["UserUID"].ToString()), "", DateTime.Now);
                                    }
                                }
                            }
                            else
                            {
                                LblMessage.Text += lblTaskName.Text + "(Total Quantity),";
                            }

                        }
                        else
                        {
                            LblMessage.Text += lblTaskName.Text + "(No value),";
                        }
                    }
                    catch (Exception ex)
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code : ARM-01 there is a problem with these feature. please contact system admin.');</script>");
                    }

                }
                LblMessage.Text = LblMessage.Text + cnt + " Rows updated";
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code : ARM-01 there is a problem with these feature. please contact system admin.');</script>");
            }
        }
        public override void VerifyRenderingInServerForm(Control control)
        {
            //required to avoid the runtime error "  
            //Control 'GridView1' of type 'GridView' must be placed inside a form tag with runat=server."  
        }
    }
}