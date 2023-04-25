using ProjectManagementTool.Models;
using ProjectManager.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManagementTool._modal_pages
{
    public partial class add_resourceplanned : System.Web.UI.Page
    {
        DBGetData getdata = new DBGetData();
        TaskUpdate TKUpdate = new TaskUpdate();
       
        public int StartYear = 0;
        public int EndYear = 0;
        
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
                   ResourcePlanningDetails();
                }
            }
        }

        protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Find the DropDownList in the Row.
                DropDownList ddlMonths = (e.Row.FindControl("ddlMonths") as DropDownList);
                ddlMonths.CssClass = "form-control";

                //Add Default Item in the DropDownList.
                ddlMonths.Items.Insert(0, new ListItem("Please select", "00"));
                ddlMonths.Items.Insert(1, new ListItem("Jan", "01"));
                ddlMonths.Items.Insert(2, new ListItem("Feb", "02"));
                ddlMonths.Items.Insert(3, new ListItem("Mar", "03"));
                ddlMonths.Items.Insert(4, new ListItem("Apr", "04"));
                ddlMonths.Items.Insert(5, new ListItem("May", "05"));
                ddlMonths.Items.Insert(6, new ListItem("Jun", "06"));
                ddlMonths.Items.Insert(7, new ListItem("Jul", "07"));
                ddlMonths.Items.Insert(8, new ListItem("Aug", "08"));
                ddlMonths.Items.Insert(9, new ListItem("Sep", "09"));
                ddlMonths.Items.Insert(10, new ListItem("Oct", "10"));
                ddlMonths.Items.Insert(11, new ListItem("Nov", "11"));
                ddlMonths.Items.Insert(12, new ListItem("Dec", "12"));

                //Select the Country of Customer in DropDownList.
                string month = (e.Row.FindControl("lblMonth") as Label).Text;
                ddlMonths.Items.FindByText(month).Selected = true;


                DropDownList ddlYears = (e.Row.FindControl("ddlYears") as DropDownList);
                ddlYears.CssClass = "form-control";

                //Add Default Item in the DropDownList.
                ddlYears.Items.Insert(0, new ListItem("Please select", "0000"));
                ddlYears.Items.Insert(1, new ListItem("2020", "2020"));
                ddlYears.Items.Insert(2, new ListItem("2021", "2021"));
                ddlYears.Items.Insert(3, new ListItem("2022", "2022"));
                ddlYears.Items.Insert(4, new ListItem("2023", "2023"));


                //Select the Country of Customer in DropDownList.
                string year = (e.Row.FindControl("lblYear") as Label).Text;
                ddlYears.Items.FindByText(year).Selected = true;

                TextBox txtPlanned = (e.Row.FindControl("txtPlanned") as TextBox);
                txtPlanned.CssClass = "form-control";


                string planned = (e.Row.FindControl("lblPlanned") as Label).Text;
                txtPlanned.Text = planned;
            }
        }


        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            DataSet ds = getdata.GetResourecDeployment_by_ResourceUID(new Guid(Request.QueryString["ResourceUID"]));

            ds.Tables[0].Rows.Add(Guid.NewGuid(), "Jan", "2020", 0);

            GridView1.DataSource = ds;
            GridView1.DataBind();

            btnAddNew.Enabled = false;
            btnCancel.Enabled = true;
            btnSaveNew.Enabled = true;
            btnSave.Enabled = false;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ResourcePlanningDetails();

            btnAddNew.Enabled = true;
            btnCancel.Enabled = false;
            btnSaveNew.Enabled = false;
            btnSave.Enabled = true;
        }

        protected void btnSaveNew_Click(object sender, EventArgs e)
        {
            try
            {

                GridViewRow NewRow = GridView1.Rows[GridView1.Rows.Count - 1];

                string ID = NewRow.Cells[0].Text;

                DropDownList ddlMonths = (NewRow.FindControl("ddlMonths") as DropDownList);

                int month = Convert.ToInt32(ddlMonths.SelectedValue);

                DropDownList ddlYears = (NewRow.FindControl("ddlYears") as DropDownList);

                int year = Convert.ToInt32(ddlYears.SelectedValue);

                TextBox txtPlanned = (NewRow.FindControl("txtPlanned") as TextBox);

                float Planned = Convert.ToSingle(txtPlanned.Text);

                DateTime startDate = new DateTime(year, month, 1);
                DateTime endDate = startDate.AddDays(DateTime.DaysInMonth(startDate.Year, startDate.Month));

                int sresult = getdata.InsertorUpdateResourceDeploymentPlanned(new Guid(ID), new Guid(Request.QueryString["WorkpackageUID"]), new Guid(Request.QueryString["ResourceUID"]), startDate, endDate, "Month", Planned, DateTime.Now);

                DataSet ds = getdata.GetResourecDeployment_by_ResourceUID(new Guid(Request.QueryString["ResourceUID"]));

                GridView1.DataSource = ds;
                GridView1.DataBind();

                btnAddNew.Enabled = true;
                btnCancel.Enabled = false;
                btnSaveNew.Enabled = false;
               

            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code ATS-01 there is a problem with this feature. Please contact system admin. Description : " + ex.Message + "');</script>");
            }

        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {

                foreach (GridViewRow grdrow in GridView1.Rows)
                {
                    string ID = grdrow.Cells[0].Text;

                    DropDownList ddlMonths = (grdrow.FindControl("ddlMonths") as DropDownList);
                    int month = Convert.ToInt32(ddlMonths.SelectedValue);

                    DropDownList ddlYears = (grdrow.FindControl("ddlYears") as DropDownList);
                    int year = Convert.ToInt32(ddlYears.SelectedValue);

                    TextBox txtPlanned = (grdrow.FindControl("txtPlanned") as TextBox);
                    float Planned = Convert.ToSingle(txtPlanned.Text);

                    DateTime startDate = new DateTime(year, month, 1);
                    DateTime endDate = startDate.AddDays(DateTime.DaysInMonth(startDate.Year, startDate.Month));

                    int sresult = getdata.InsertorUpdateResourceDeploymentPlanned(new Guid(ID), new Guid(Request.QueryString["WorkpackageUID"]), new Guid(Request.QueryString["ResourceUID"]), startDate, endDate, "Month", Planned, DateTime.Now);
                }

                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code ATS-01 there is a problem with this feature. Please contact system admin. Description : " + ex.Message + "');</script>");
            }
        }


        protected void OnRowCommand(object sender, GridViewCommandEventArgs e)
        {
            string UID = e.CommandArgument.ToString();

            if (e.CommandName == "delete")
            {
                int cnt = getdata.ResourceDeployment_Delete(new Guid(UID), new Guid(Session["UserUID"].ToString()));

                if (cnt > 0)
                {
                    //scrpManager.Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('It can not be deleted as there exists data');</script>");
                    //Page.ClientScript.RegisterStartupScript(Page.GetType(), "Warning", "<script language='javascript'>alert('It can not be deleted as there exists data.');</script>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Warning", "alert('This cannot be deleted as there exists data for this.');", true);
                }
                else if (cnt == 0)
                {
                    ResourcePlanningDetails();
                }
            }
        }
        
        protected void ResourcePlanningDetails()
        {
            GridView1.DataSource = null;
            GridView1.DataBind();
            DataSet ds = getdata.GetResourecDeployment_by_ResourceUID(new Guid(Request.QueryString["ResourceUID"]));
            GridView1.DataSource = ds;
            GridView1.DataBind();
        }

        protected void GrdView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            //Page.ClientScript.RegisterStartupScript(Page.GetType(), "Warning", "<script language='javascript'>alert('It can not be deleted as there exists data.');</script>");

        }
    }
}