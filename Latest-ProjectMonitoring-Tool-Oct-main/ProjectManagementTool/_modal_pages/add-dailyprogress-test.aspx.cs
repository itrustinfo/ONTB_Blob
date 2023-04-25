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
    public partial class add_dailyprogress_test : System.Web.UI.Page
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
            DataSet ds = getdata.GetGFCStatusMasters();
            DDLDailyReportMaster.DataTextField = "Description";
            DDLDailyReportMaster.DataValueField = "GFC_UID";
            DDLDailyReportMaster.DataSource = ds;
            DDLDailyReportMaster.Items.Insert(0, "--Select--");
            DDLDailyReportMaster.DataBind();
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
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


                int cnt = getdata.InsertorUpdateGFCStatus(Guid.NewGuid(), new Guid(DDLDailyReportMaster.SelectedValue), projectUid, workPackageUid, txtLocation.Text,Convert.ToDecimal(txtLength.Text),Convert.ToDateTime(getdata.ConvertDateFormat(txtSubmittedDate.Text)),Convert.ToDecimal(txtONTBReleasedLength.Text),Convert.ToDateTime(getdata.ConvertDateFormat(txtONTBReleasedDate.Text)),Convert.ToDecimal(txtONTBReleasedBalance.Text )
                      ,Convert.ToDateTime(getdata.ConvertDateFormat(txtONTBReleasedBalanceDate.Text)), Convert.ToDecimal(txtGFCApproved.Text),txtRemarks.Text,txtEEOfficeApproval.Text,txtNineSets.Text, txtApproved.Text);
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

        protected void TxtId_TextChanged(object sender, EventArgs e)
        {
            //decimal todaysQty = 0, previousQty = 0, totalQty = 0;
            //if (!string.IsNullOrEmpty(txtPreviousQty.Text))
            //    Decimal.TryParse(txtPreviousQty.Text.Trim(), out previousQty);
            //if (!string.IsNullOrEmpty(txtTodaysQty.Text))
            //    Decimal.TryParse(txtTodaysQty.Text.Trim(), out todaysQty);
            //totalQty = todaysQty + previousQty;

            //txtTotalQty.Text = totalQty.ToString();
        }
    }
}