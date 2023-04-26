using ProjectManager.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManagementTool._content_pages.submittal_documents
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
                    
                    BindDocuments();
                }
            }
        }

        private void BindDocuments()
        {
                LblDocumentHeading.Text = "";
                GrdClientApprovedDocuments.Visible = true;
                DataSet ds = getdt.GetDocumentsBySubmittal(new Guid(Request.QueryString["ProjectUID"]),new Guid(Request.QueryString["WorkPackageUID"]));
                GrdClientApprovedDocuments.DataSource = ds;
                GrdClientApprovedDocuments.DataBind();
        }

        public string GetSubmittalName(string DocumentID)
        {
            return getdt.getDocumentName_by_DocumentUID(new Guid(DocumentID));
        }
        public string GetDocumentTypeIcon(string DocumentExtn)
        {
            return getdt.GetDocumentTypeMasterIcon_by_Extension(DocumentExtn);
        }

        public string GetTaskHierarchy_By_DocumentUID(string DocumentUID)
        {
            return getdt.GetTaskHierarchy_By_DocumentUID(new Guid(DocumentUID));
        }
        public string GetDocumentName(string DocumentExtn)
        {
            string retval = getdt.GetDocumentMasterType_by_Extension(DocumentExtn);
            if (retval == null || retval == "")
            {
                return "N/A";
            }
            else
            {
                return retval;
            }
        }
  
        protected void GrdClientApprovedDocuments_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdClientApprovedDocuments.PageIndex = e.NewPageIndex;
           // string[] DocType = Request.QueryString["DocumentType"].ToString().Split('_');
            BindDocuments();
        }

        protected void GrdClientApprovedDocuments_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                //if (e.Row.Cells[4+3].Text != "&nbsp;")
                //{
                //    DateTime dt = getdt.GetDocumentReviewedDateByID(new Guid(e.Row.Cells[4+3].Text), "Client Approved");
                //    e.Row.Cells[4+3].Text = dt.ToString("dd MMM yyyy");
                //}

                //DateTime? acceptedRejDate = getdt.GetDocumentAcceptedRecejtedDate(new Guid(e.Row.Cells[e.Row.Cells.Count - 1].Text));
                //if (acceptedRejDate != null)
                //{
                //    if (acceptedRejDate.ToString().Contains("1/1/0001"))
                //    {
                //        e.Row.Cells[7].Text = "N/A";
                //    }
                //    else
                //    {
                //        e.Row.Cells[7].Text = Convert.ToDateTime(acceptedRejDate).ToString("dd/MM/yyyy");
                //    }
                //}
            }
        }

       
        protected void GrdClientApprovedDocuments_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string UID = e.CommandArgument.ToString();
            if (e.CommandName == "download")
            {
                string path = string.Empty;
                string filename = string.Empty;
                DataSet ds1 = null;
                DataSet ds = getdt.getLatest_DocumentVerisonSelect(new Guid(UID));
                string docversion = string.Empty;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    path = Server.MapPath(ds.Tables[0].Rows[0]["Doc_FileName"].ToString());
                    //File.Decrypt(path);
                    docversion = ds.Tables[0].Rows[0]["Doc_Version"].ToString();
                }
                else
                {
                    ds1 = getdt.ActualDocuments_SelectBy_ActualDocumentUID(new Guid(UID));
                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        if (ds1.Tables[0].Rows[0]["ActualDocument_Path"].ToString() != "")
                        {
                            path = Server.MapPath(ds1.Tables[0].Rows[0]["ActualDocument_Path"].ToString());
                        }
                    }
                }
                // added on  20/10/2020
                ds.Clear();
                ds = getdt.ActualDocuments_SelectBy_ActualDocumentUID(new Guid(UID));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["ActualDocument_Name"].ToString() != "")
                    {
                        filename = ds.Tables[0].Rows[0]["ActualDocument_Name"].ToString() + ds.Tables[0].Rows[0]["ActualDocument_Type"].ToString();
                    }
                }
                //
                byte[] bytes;

                string filepath = Server.MapPath("~/_PreviewLoad/" + Path.GetFileName(path));
                //
                //for blob
                string Connectionstring = getdt.getProjectConnectionString(new Guid(ds.Tables[0].Rows[0]["ProjectUID"].ToString()));
                //
                if (docversion == "1")
                {
                    DataSet docBlob = getdt.GetActualDocumentBlob_by_ActualDocumentUID(new Guid(UID), Connectionstring);
                    if (docBlob.Tables[0].Rows.Count > 0)
                    {
                        bytes = (byte[])docBlob.Tables[0].Rows[0]["Blob_Data"];

                        BinaryWriter Writer = null;
                        Writer = new BinaryWriter(File.OpenWrite(filepath));

                        // Writer raw data                
                        Writer.Write(bytes);
                        Writer.Flush();
                        Writer.Close();
                    }
                }
                else
                {
                    DataSet docBlob = getdt.GetDocumentVersionBlob_by_DocVersion_UID(new Guid(ds.Tables[0].Rows[0]["DocVersion_UID"].ToString()), Connectionstring);
                    if (docBlob.Tables[0].Rows.Count > 0)
                    {
                        bytes = (byte[])docBlob.Tables[0].Rows[0]["ResubmitFile_Blob"];

                        BinaryWriter Writer = null;
                        Writer = new BinaryWriter(File.OpenWrite(filepath));

                        // Writer raw data                
                        Writer.Write(bytes);
                        Writer.Flush();
                        Writer.Close();
                    }
                }
                string getExtension = System.IO.Path.GetExtension(filepath);
                string outPath = path.Replace(getExtension, "") + "_download" + getExtension;
                getdt.DecryptFile(filepath, outPath);
                System.IO.FileInfo file = new System.IO.FileInfo(outPath);

                if (file.Exists)
                {
                    int Cnt = getdt.DocumentHistroy_InsertorUpdate(Guid.NewGuid(), new Guid(UID), new Guid(Session["UserUID"].ToString()), "Downloaded", "Documents");
                    if (Cnt <= 0)
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code: DDH-01. there is a problem with updating histroy. Please contact system admin.');</script>");
                    }
                    Response.Clear();

                    // Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + filename);
                    Response.AddHeader("Content-Length", file.Length.ToString());

                    Response.ContentType = "application/octet-stream";

                    Response.WriteFile(file.FullName);

                    Response.Flush();

                    try
                    {
                        if (File.Exists(outPath))
                        {
                            File.Delete(outPath);
                        }
                    }
                    catch (Exception ex)
                    {
                        //throw
                    }

                    Response.End();

                }
                else
                {

                    //Response.Write("This file does not exist.");
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('File does not exists.');</script>");

                }
            }
        }

        
    }
}