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
    public partial class add_Design_and_drawing_works_dwg_issue : System.Web.UI.Page
    {
        TaskUpdate TKUpdate = new TaskUpdate();
        DBGetData getdata = new DBGetData();
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
                    BindDailyReportMasterMaster();

                    if (Session["DPR_UID"] != null)
                        DDLDailyReportMaster.SelectedValue = Session["DPR_UID"].ToString();

                }
            }
        }

        private void BindDailyReportMasterMaster()
        {
            DataSet ds = getdata.GetdesignanddrawingDWGIssuemaster();
            DDLDailyReportMaster.DataTextField = "Description";
            DDLDailyReportMaster.DataValueField = "DPR_UID";
            DDLDailyReportMaster.DataSource = ds;
            DDLDailyReportMaster.Items.Insert(0, "--Select--");
            DDLDailyReportMaster.DataBind();
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (!decimal.TryParse(txtSubmittedByContractor.Text, out decimal submittedbycontractor))
                {
                    Response.Write("<script>alert('Please enter correct submitted by contractor');</script>");
                    return;
                }
                if (!decimal.TryParse(txtapprovedbyontb.Text, out decimal approverbyconntractor))
                {
                    Response.Write("<script>alert('Please enter correct approved by Contractor');</script>");
                    return;
                }

                if (!decimal.TryParse(txtGFCApprovedByBWSSB.Text, out decimal GFCApprovedByBWSSB))
                {
                    Response.Write("<script>alert('Please enter correct value');</script>");
                    return;
                }

                if (submittedbycontractor < approverbyconntractor)
                {
                    Response.Write("<script>alert('Approved by contractor should be less than Submitted by contractor');</script>");
                    return;
                }

                Guid projectUid = Guid.Empty, workPackageUid = Guid.Empty;
                if (DDLDailyReportMaster.SelectedValue == "--Select--")
                {
                    Response.Write("<script>alert('Please select a Project');</script>");
                    return;
                }
                if (Request.QueryString["ProjectUID"] == null)
                {
                    Response.Write("<script>alert('Please select a project');</script>");
                    return;
                }
                else
                {
                    projectUid = new Guid(Request.QueryString["ProjectUID"].ToString());
                }
                if (Request.QueryString["WorkPackageUID"] == null)
                {
                    Response.Write("<script>alert('Please select a WorkPackageUID');</script>");
                    return;
                }
                else
                {
                    workPackageUid = new Guid(Request.QueryString["WorkPackageUID"].ToString());
                }

                string sDate1 = "";
                DateTime CDate1 = DateTime.Now,cdate2=DateTime.Now, cdate3=DateTime.Now;

                sDate1 = txtSubmittedDate.Text;
                sDate1 = getdata.ConvertDateFormat(sDate1);

                CDate1 = Convert.ToDateTime(sDate1);

                sDate1 = txtapproveddate.Text;
                sDate1 = getdata.ConvertDateFormat(sDate1);
                cdate2 = Convert.ToDateTime(sDate1);

                sDate1 = txtGFCApprovedDate.Text;
                sDate1 = getdata.ConvertDateFormat(sDate1);
                cdate3 = Convert.ToDateTime(sDate1);



                if (CDate1>cdate2)
                {
                    Response.Write("<script>alert('Approved Date should be greater than SubmittedDate');</script>");
                    return;
                }
                int cnt = getdata.InsertorUpdateDesignanddrawingworksdwgissue(Guid.NewGuid(), new Guid(DDLDailyReportMaster.SelectedValue), projectUid, workPackageUid, ddlZoneName.SelectedValue.ToString(), txtLocation.Text, Convert.ToDecimal(txtSubmittedByContractor.Text),
                     CDate1, Convert.ToDecimal(txtapprovedbyontb.Text), cdate2,Convert.ToDecimal(txtGFCApprovedByBWSSB.Text),cdate3, DateTime.Now);
                if (cnt > 0)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error while saving data.');</script>");
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code : ARM-01 there is a problem with these feature. please contact system admin.');</script>");
            }
        }

        //protected void TxtId_TextChanged(object sender, EventArgs e)
        //{
        //    decimal todaysQty = 0, previousQty = 0, totalQty = 0;
        //    if (!string.IsNullOrEmpty(txtPreviousQty.Text))
        //        Decimal.TryParse(txtPreviousQty.Text.Trim(), out previousQty);
        //    if (!string.IsNullOrEmpty(txtTodaysQty.Text))
        //        Decimal.TryParse(txtTodaysQty.Text.Trim(), out todaysQty);
        //    totalQty = todaysQty + previousQty;

        //    txtTotalQty.Text = totalQty.ToString();
        //}
    }
}