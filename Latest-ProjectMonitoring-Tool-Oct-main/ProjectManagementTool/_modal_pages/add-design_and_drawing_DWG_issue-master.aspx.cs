﻿using ProjectManager.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManagementTool._modal_pages
{
    public partial class add_design_and_drawing_dwg_issue_master : System.Web.UI.Page
    {
        DBGetData getdata = new DBGetData();
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

                    if (Request.QueryString["MeetigUID"] != null)
                    {
                        BindReviewMeeting(Request.QueryString["MeetigUID"]);
                    }
                }
            }
        }


        private void BindReviewMeeting(string Meeting_UID)
        {
            DataSet ds = getdata.Getdesignanddrawingdwgissuemaster_by_UID(new Guid(Meeting_UID));
            if (ds.Tables[0].Rows.Count > 0)
            {
                txtdesc.Text = ds.Tables[0].Rows[0]["Description"].ToString();
                if (ds.Tables[0].Rows[0]["CreatedDate"].ToString() != null && ds.Tables[0].Rows[0]["CreatedDate"].ToString() != "")
                {
                    dtMeetingDate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["CreatedDate"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

                }
            }

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                string sDate1 = "";
                DateTime CDate1 = DateTime.Now;
                sDate1 = dtMeetingDate.Text;
                //sDate1 = sDate1.Split('/')[1] + "/" + sDate1.Split('/')[0] + "/" + sDate1.Split('/')[2];
                sDate1 = getdata.ConvertDateFormat(sDate1);
                CDate1 = Convert.ToDateTime(sDate1);
                int cntDesc = getdata.Checkdesignanddrawingworksb_Issue(CDate1);
                if ((cntDesc == 1 && Request.QueryString["MeetigUID"] != null) || cntDesc == 0)                    
                {
                    Guid Meeting_UID = Guid.NewGuid();
                    if (Request.QueryString["MeetigUID"] != null)
                    {
                        Meeting_UID = new Guid(Request.QueryString["MeetigUID"]);
                    }
                   

                    int cnt = getdata.InsertorUpdatedesignanddrawingdwgissuemaster(Meeting_UID, txtdesc.Text, CDate1);
                    if (cnt > 0)
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
                    }
                }
                else
                {
                    Response.Write("<script>alert('Design and Drawing Works B TT is already created, Please try other date');</script>");
                    return;
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code : ARM-01 there is a problem with these feature. please contact system admin.');</script>");
            }
        }
    }
}