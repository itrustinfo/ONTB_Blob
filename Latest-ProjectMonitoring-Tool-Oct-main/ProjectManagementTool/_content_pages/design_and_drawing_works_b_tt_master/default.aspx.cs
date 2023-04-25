﻿using ProjectManager.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManagementTool._content_pages.design_and_drawing_works_b_tt_master
{
    public partial class _default : System.Web.UI.Page
    {
        DBGetData getdt = new DBGetData();
        TaskUpdate gettk = new TaskUpdate();
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
                    BindDailyProgressReportMaster();
                }
            }
        }

        private void BindDailyProgressReportMaster()
        {
            DataSet ds = getdt.Getdesignanddrawingworksbttmaster();
            GrdReviewMeeting.DataSource = ds;
            GrdReviewMeeting.DataBind();
        }

        protected void GrdReviewMeeting_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdReviewMeeting.PageIndex = e.NewPageIndex;
            BindDailyProgressReportMaster();
        }
    }
}