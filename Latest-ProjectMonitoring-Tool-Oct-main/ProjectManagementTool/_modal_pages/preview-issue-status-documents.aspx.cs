using ProjectManager.DAL;
using System;
using System.Data;
using System.Data.OleDb;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Office.Interop.Word;
using System.Text.RegularExpressions;
using System.Web.Configuration;

namespace ProjectManagementTool._modal_pages
{
    public partial class preview_issue_status_documents : System.Web.UI.Page
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
                if (!Page.IsPostBack)
                {
                    if (Request.QueryString["IssueRemarksUID"] != null)
                    {
                        DataSet ds = getdata.GetUploadedIssueStatusImages(Request.QueryString["IssueRemarksUID"]);

                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            GrdIssueStatusImages.DataSource = null;
                            GrdIssueStatusImages.DataBind();

                            ImageField img = new ImageField();
                            img.HeaderText = "Issue Attached Images";
                            img.DataImageUrlField = "IssueImage";//Your Column Name Representing the image.
                            
                            GrdIssueStatusImages.Columns.Add(img);

                            GrdIssueStatusImages.DataSource = ds;
                            GrdIssueStatusImages.DataBind();
                            GrdIssueStatusImages.HeaderRow.Visible = false;
                        }
                    }
                }
               
            }
        }

        protected void GrdIssueStatusImages_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                
                Image issue_img = (Image) e.Row.Cells[0].Controls[0];

                string path = Server.MapPath(issue_img.ImageUrl);

                string Extension = Path.GetExtension(path);

                if (Extension == ".jpg" || Extension == ".png" || Extension == ".jpeg" || Extension == ".bmp")
                {
                    string outPath = path.Replace(Extension, "") + "_download" + Extension;
                    getdata.DecryptFile(path, outPath);
                    FileInfo file = new FileInfo(outPath);

                    string fname = "/Documents/Issues/" + file.Name;

                    issue_img.ImageUrl = fname;

                    issue_img.Attributes.Add("width", "100%");
                    issue_img.Attributes.Add("height", "620");
                    //issue_img.Attributes.Add("class", "img-fluid");
                }
                else
                {
                    issue_img.AlternateText = "Not an image file, it can not be displayed, go to next";
                }
                
            }
        }

        protected void OnPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdIssueStatusImages.PageIndex = e.NewPageIndex;
            DataSet ds = getdata.GetUploadedIssueStatusImages(Request.QueryString["IssueRemarksUID"]);
          
            GrdIssueStatusImages.DataSource = ds;
            GrdIssueStatusImages.DataBind();
            GrdIssueStatusImages.HeaderRow.Visible = false;
 
        }
    }
}