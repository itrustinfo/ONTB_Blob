﻿using ProjectManager.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace ProjectManagementTool._modal_pages
{
    public partial class upload_rabill_document : System.Web.UI.Page
    {
        private DBGetData getData = new DBGetData();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Username"] == null)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
            }
            else
            {
                if (Request.QueryString["RABillUid"] != null && Request.QueryString["WorkpackageUID"] != null)
                {
                    BindDataforDocument_RABills(Request.QueryString["RABillUid"]);
                }
            }
        }
        private void BindDataforDocument_RABills(string RaBuildUID)
        {
            DataTable dt = getData.GetRaBillDocuement(new Guid(RaBuildUID));

            dt.Columns.Add("FileName");
            foreach(DataRow eachRow in dt.Rows)
            {
                string fileName = Path.GetFileName(eachRow.Field<string>("FilePath"));
                eachRow.SetField<string>("FileName", fileName);
            }

            GrdTreeView.DataSource = dt;
            GrdTreeView.DataBind();
            GrdTreeView.Columns[1].Visible = false;
        }
        protected void btnUpload_Click(object sender, EventArgs e)
        {
            string sFileDirectory = "~/Documents/RABills/"+ Request.QueryString["RABillUid"];

            if (!Directory.Exists(Server.MapPath(sFileDirectory)))
            {
                Directory.CreateDirectory(Server.MapPath(sFileDirectory));
            }
            string raBuilID = Request.QueryString["RABillUid"];

            byte[] filetobytes = null;

            foreach (HttpPostedFile uploadedFile in ImageUpload.PostedFiles)
            {
                if (uploadedFile.ContentLength > 0 && !String.IsNullOrEmpty(uploadedFile.FileName))
                {
                    //string sFileName = Path.GetFileName(uploadedFile.FileName);
                    //string FileExtn = Path.GetExtension(uploadedFile.FileName);
                    //uploadedFile.SaveAs(Server.MapPath(sFileDirectory + "/" + sFileName));
                    //string savedPath = sFileDirectory + "/" + sFileName;
                    //string DecryptPagePath = sFileDirectory + "/" + sFileName + "_DE" + FileExtn;
                    //getData.EncryptFile(Server.MapPath(savedPath), Server.MapPath(DecryptPagePath));

                    //string sFileName = Path.GetFileNameWithoutExtension(uploadedFile.FileName);
                    //string Extn = Path.GetExtension(uploadedFile.FileName);
                    //uploadedFile.SaveAs(Server.MapPath(sFileDirectory + "/" + sFileName + Extn));
                    //string savedPath = sFileDirectory + "/" + sFileName + Extn;
                    //string DecryptPagePath = sFileDirectory + "/" + sFileName + "_DE" + Extn;
                    //getData.EncryptFile(Server.MapPath(savedPath), Server.MapPath(DecryptPagePath));



                    string fileName = Path.GetFileName(uploadedFile.FileName);
                    uploadedFile.SaveAs(Server.MapPath("~/Documents/") + fileName);
                    string sFileName1 = Path.GetFileNameWithoutExtension(uploadedFile.FileName);
                    string Extn = Path.GetExtension(uploadedFile.FileName);
                    string savedPath1 = "~/Documents/" + fileName;
                    string DocPath = "~/Documents/" + sFileName1 + "_DE" + Extn;
                    getData.EncryptFile(Server.MapPath(savedPath1), Server.MapPath(DocPath));
                    filetobytes = getData.FileToByteArray(Server.MapPath(DocPath));


                    int Cnt = getData.RABill_Document_InsertUpdate(Guid.NewGuid(), new Guid(Request.QueryString["RABillUid"]), new Guid(Request.QueryString["WorkpackageUID"]), savedPath1, txtInvoiceNumber.Text, new Guid(Session["UserUID"].ToString()),filetobytes);
                    
                    if (Cnt <= 0)
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code ADDSP-01 there is a problem with this feature. Please contact system admin.');</script>");
                    }
                }
            }
            BindDataforDocument_RABills(raBuilID);
        }

        protected void GrdTreeView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            HiddenField hidRAbuilluid = GrdTreeView.Rows[e.RowIndex].FindControl("documentDeleteuid") as HiddenField;

            int Cnt = getData.RaBillDocuement_Delete( new Guid(hidRAbuilluid.Value), new Guid(Session["UserUID"].ToString()));
            
            string raBuilID = Request.QueryString["RABillUid"];
            BindDataforDocument_RABills(raBuilID);
        }

        protected void Download_Click(object sender, EventArgs e)
        {
            
            //string getExtension = Path.GetExtension(FilePath);
            //string outPath = FilePath.Replace(getExtension, "") + "_DE" + getExtension;
            //getData.DecryptFile(FilePath, outPath);
            //DownloadFile(FilePath);

            try
            {
                string doc_uid = ((HtmlAnchor)sender).HRef;
                byte[] bytes;
                string filename = "";
                bytes = getData.DownloadBill(doc_uid, out filename);

                string path = Server.MapPath(filename);

                string filepath = Server.MapPath("~/_PreviewLoad/" + Path.GetFileName(path));

                BinaryWriter Writer = null;
                Writer = new BinaryWriter(File.OpenWrite(filepath));

                // Writer raw data                
                Writer.Write(bytes);
                Writer.Flush();
                Writer.Close();

                string getExtension = System.IO.Path.GetExtension(filepath);
                string outPath = filepath.Replace(getExtension, "") + "_download" + getExtension;
                getData.DecryptFile(filepath, outPath);

                System.IO.FileInfo file = new System.IO.FileInfo(outPath);

                if (file.Exists)
                {

                    Response.Clear();

                    Response.AddHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(path));

                    Response.AddHeader("Content-Length", file.Length.ToString());

                    Response.ContentType = "application/octet-stream";

                    Response.WriteFile(file.FullName);
                    Response.Flush();
                    Response.SuppressContent = true;
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    //Response.End();
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('File not found.');</script>");
                }

                if (File.Exists(outPath))
                {
                    File.Delete(outPath);
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('There is a problem downloading file. Please contact system admin. Description: " + ex.Message + "');</script>");
            }
        }

        private void DownloadFile(string file)
        {
            var fi = new FileInfo(file);
            Response.Clear();
            Response.ContentType = "application/octet-stream";
            Response.AddHeader("Content-Disposition", "attachment; filename=" + fi.Name);
            Response.WriteFile(file);
            Response.End();
        }
    }
}