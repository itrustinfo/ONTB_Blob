using ProjectManager.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManagementTool._modal_pages
{
    public partial class add_issuestatus : System.Web.UI.Page
    {
        DBGetData getdata = new DBGetData();
        TaskUpdate TKUpdate = new TaskUpdate();
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
                    if (Request.QueryString["Issue_Uid"] != null)
                    {
                        IssueBind();
                    }
                    if (Request.QueryString["IssueRemarksUID"] != null)
                    {
                        IssueStatusDataBind();
                    }
                }
            }
        }

        private void IssueStatusDataBind()
        {
            DataSet ds = getdata.GetIssueStatus_by_IssueRemarksUID(new Guid(Request.QueryString["IssueRemarksUID"]));
            if (ds.Tables[0].Rows.Count > 0)
            {
                DDLStatus.SelectedValue = ds.Tables[0].Rows[0]["Issue_Status"].ToString();
                txtremarks.Text = ds.Tables[0].Rows[0]["Issue_Remarks"].ToString();
                ViewState["Document"] = ds.Tables[0].Rows[0]["Issue_Document"].ToString();
                //DDLStatus.Enabled = false;
            }
        }

        private void IssueBind()
        {
            DataSet ds = getdata.getIssuesList_by_UID(new Guid(Request.QueryString["Issue_Uid"]));
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["Issue_Status"].ToString() == "Close")
                {
                    DDLStatus.SelectedValue = "Close";
                    DDLStatus.Enabled = false;
                    btnSubmit.Visible = false;
                }
                else
                {
                    DDLStatus.Enabled = true;
                    btnSubmit.Visible = true;
                }

                if (ds.Tables[0].Rows[0]["TaskUID"].ToString() == "00000000-0000-0000-0000-000000000000")
                {
                    HiddenActivity.Value = ds.Tables[0].Rows[0]["WorkPackagesUID"].ToString();
                }
                else
                {
                    HiddenActivity.Value = ds.Tables[0].Rows[0]["TaskUID"].ToString();
                }

                //added on 22/02/2023
                if(WebConfigurationManager.AppSettings["Domain"] == "LNT" || WebConfigurationManager.AppSettings["Domain"] =="Suez")
                {
                    DDLStatus.Items.Remove("Close");
                }
            }
        }
       

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string DocPath = "";

            var issue_uid = new Guid(Request.QueryString["Issue_Uid"]);

            var issue_remarks_uid = (Request.QueryString["IssueRemarksUID"] == null) ? Guid.NewGuid() : new Guid(Request.QueryString["IssueRemarksUID"]);

            int cnt = getdata.Issues_Status_Remarks_Insert(issue_remarks_uid, issue_uid, DDLStatus.SelectedValue, txtremarks.Text, DocPath, DateTime.Today.Date);

            if (cnt > 0)
            {
                if (FileUploadDoc.HasFiles)
                {
                    string FileDirectory = "~/Documents/IssueRemarks/";

                    byte[] filetobytes = null;

                    if (!Directory.Exists(Server.MapPath(FileDirectory)))
                    {
                        Directory.CreateDirectory(Server.MapPath(FileDirectory));
                    }

                    foreach (HttpPostedFile postedFile in FileUploadDoc.PostedFiles)
                    {
                        string fileName = Path.GetFileName(postedFile.FileName);
                        postedFile.SaveAs(Server.MapPath(FileDirectory) + fileName);

                        string sFileName = Path.GetFileNameWithoutExtension(postedFile.FileName);
                        string Extn = Path.GetExtension(postedFile.FileName);

                        string savedPath = FileDirectory + fileName;

                        DocPath = FileDirectory + sFileName + "_DE" + Extn;

                        getdata.EncryptFile(Server.MapPath(savedPath), Server.MapPath(DocPath));

                        filetobytes = DBGetData.FileToByteArray(Server.MapPath(DocPath));

                        getdata.InsertIssueRemarksBlob(fileName, savedPath, filetobytes, issue_remarks_uid.ToString());
                    }
                }
            }

            Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
        }
    }
}