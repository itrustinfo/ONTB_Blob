using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProjectManager.DAL;

namespace ProjectManagementTool._modal_pages
{
    public partial class view_documentReplaced : System.Web.UI.Page
    {
        DBGetData getdata = new DBGetData();
        TaskUpdate TKUpdate = new TaskUpdate();
        protected void Page_Load(object sender, EventArgs e)
        {
            this.UnobtrusiveValidationMode = System.Web.UI.UnobtrusiveValidationMode.None;
            if (Session["Username"] == null)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
            }
            else
            {
                if (!IsPostBack)
                {
                    if (Request.QueryString["DocID"] != null)
                    {
                        BindDocument();
                    }
                }
            }
        }

        protected void BindDocument()
        {
           
            DataSet ds = getdata.getActualDocumentStatusList(new Guid(Request.QueryString["DocID"].ToString()));

            DataSet ds1 = getdata.ActualDocuments_SelectBy_ActualDocumentUID(new Guid(Request.QueryString["DocID"].ToString()));
             
            if (ds1.Tables[0].Rows.Count > 0)
            {
                lblDocumentName.Text = GetSubmittalName(ds1.Tables[0].Rows[0]["DocumentUID"].ToString());

                txtONTBRefNo.Text = ds1.Tables[0].Rows[0]["ProjectRef_Number"].ToString();
                txtDesc.Text = ds1.Tables[0].Rows[0]["Description"].ToString();

                // if (ds.Tables[0].Rows[0]["Doc_Type"].ToString() == "Cover Letter")
                // {
                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["CoverLetterFile"].ToString()))
                        lblOldCoverLetter.Text = ds.Tables[0].Rows[0]["CoverLetterFile"].ToString().Split('/')[ds.Tables[0].Rows[0]["CoverLetterFile"].ToString().Split('/').Length-1].Replace("_1","");
               // }
               // else if (ds.Tables[0].Rows[0]["Doc_Type"].ToString() == "Document")
               // {
                   if (!string.IsNullOrEmpty(ds1.Tables[0].Rows[0]["ActualDocument_Name"].ToString()))
                        lblOldDocument.Text = ds1.Tables[0].Rows[0]["ActualDocument_Name"].ToString() + ds1.Tables[0].Rows[0]["ActualDocument_Type"].ToString();
              //  }

                dtIncomingRevDate.Text =Convert.ToDateTime(ds1.Tables[0].Rows[0]["IncomingRec_Date"]).ToString("dd/MM/yyyy");
                dtDocumentDate.Text = Convert.ToDateTime(ds1.Tables[0].Rows[0]["Document_Date"]).ToString("dd/MM/yyyy");
                txtRefNumber.Text = ds1.Tables[0].Rows[0]["Ref_Number"].ToString();
                ViewState["StatusUID"] = ds.Tables[0].Rows[0]["StatusUID"].ToString();
                txtONTBRefNo.Enabled = false;
                
            }
        }

        public string GetSubmittalName(string DocumentID)
        {
            return getdata.getDocumentName_by_DocumentUID(new Guid(DocumentID));
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                string sDocumentPath = string.Empty;
                string sCoverPagePath = string.Empty;
                string projectId = Session["Project_Workpackage"].ToString().Split('_')[0];
                if (dtIncomingRevDate.Text != "" && dtDocumentDate.Text != "")
                {

                    DateTime CheckIncomingRec_Date = Convert.ToDateTime(getdata.ConvertDateFormat(dtIncomingRevDate.Text));
                    DateTime CheckDocumentDate = Convert.ToDateTime(getdata.ConvertDateFormat(dtDocumentDate.Text));

                    if (CheckDocumentDate > CheckIncomingRec_Date)
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "Warning", "<script language='javascript'>alert('Document Date connot be greater than Incoming Receive date.');</script>");
                        return;
                    }
                }
                if (!FileUploadDoc.HasFile || !FileUploadCover.HasFile)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "Warning", "<script language='javascript'>alert('Please upload documents.');</script>");
                    return;
                }

                string sFileName = string.Empty;
                string sFileNameCover = string.Empty;
                string DocPagePath = string.Empty;
                string CoverPagePath = string.Empty;
                //
                byte[] coverfiletobytes = null;
                if (FileUploadCover.HasFile)
                {
                    sCoverPagePath = "~/" + projectId + "/" + ViewState["StatusUID"] + "/CoverLetter";
                    if (!Directory.Exists(Server.MapPath(sCoverPagePath)))
                    {
                        Directory.CreateDirectory(Server.MapPath(sCoverPagePath));
                    }

                    sFileNameCover = Path.GetFileNameWithoutExtension(FileUploadCover.FileName);
                    string Extn = System.IO.Path.GetExtension(FileUploadCover.FileName);
                    FileUploadCover.SaveAs(Server.MapPath(sCoverPagePath + "/" + sFileNameCover + "_1_copy" + Extn));

                    string savedPath = sCoverPagePath + "/" + sFileNameCover + "_1_copy" + Extn;
                    CoverPagePath = sCoverPagePath + "/" + sFileNameCover + "_1" + Extn;
                    EncryptFile(Server.MapPath(savedPath), Server.MapPath(CoverPagePath));
                    //for blob
                    coverfiletobytes = getdata.FileToByteArray(Server.MapPath(CoverPagePath));
                    //
                }
                //
                byte[] docfiletobytes = null;
                if (FileUploadDoc.HasFile)
                {
                    sDocumentPath = "~/" + projectId + "/" + ViewState["StatusUID"] + "/Documents";
                    if (!Directory.Exists(Server.MapPath(sDocumentPath)))
                    {
                        Directory.CreateDirectory(Server.MapPath(sDocumentPath));
                    }

                   sFileName = Path.GetFileNameWithoutExtension(FileUploadDoc.FileName);
                    string ExtnD = System.IO.Path.GetExtension(FileUploadDoc.FileName);
                    FileUploadDoc.SaveAs(Server.MapPath(sDocumentPath + "/" + sFileName + "_1_copy" + ExtnD));

                    string savedPath = sDocumentPath + "/" + sFileName + "_1_copy" + ExtnD;
                    DocPagePath = sDocumentPath + "/" + sFileName + "_1" + ExtnD;
                    EncryptFile(Server.MapPath(savedPath), Server.MapPath(DocPagePath));
                    //for blob
                    docfiletobytes = getdata.FileToByteArray(Server.MapPath(DocPagePath));
                    //
                }
                DateTime Document_Date = Convert.ToDateTime(getdata.ConvertDateFormat(dtDocumentDate.Text));
                DateTime IncominRcvdDate = Convert.ToDateTime(getdata.ConvertDateFormat(dtIncomingRevDate.Text));

                int result = getdata.ReplaceDocsFlow2old(new Guid(Request.QueryString["DocID"].ToString()),sFileName, DocPagePath, sFileNameCover, CoverPagePath, IncominRcvdDate, Document_Date,txtRefNumber.Text);
                if (result == -1)
                {
                    //for blob storage
                    DataSet dsUIDs = getdata.GetReplaceDocsFlow2old_UIDs_ForBlob(new Guid(Request.QueryString["DocID"].ToString()));
                    if(dsUIDs.Tables[0].Rows.Count > 0)
                    {
                        //for blob
                        string Connectionstring = getdata.getProjectConnectionString(new Guid(projectId));
                        getdata.ReplaceDocsFlow2old_Blob(new Guid(Request.QueryString["DocID"].ToString()),new Guid(dsUIDs.Tables[0].Rows[0]["CoverLetterUID"].ToString()), new Guid(dsUIDs.Tables[0].Rows[0]["StatusUID"].ToString()), new Guid(dsUIDs.Tables[0].Rows[0]["DocVersionUID"].ToString()), Connectionstring, docfiletobytes, coverfiletobytes);

                    }
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "Error", "<script language='javascript'>alert('Error:11,There is some problem while inserting document. Please contact administrator');</script>");
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        private void EncryptFile(string inputFile, string outputFile)
        {

            try
            {
                string password = @"myKey123"; // Your Key Here
                UnicodeEncoding UE = new UnicodeEncoding();
                byte[] key = UE.GetBytes(password);

                string cryptFile = outputFile;
                FileStream fsCrypt = new FileStream(cryptFile, FileMode.Create);

                RijndaelManaged RMCrypto = new RijndaelManaged();

                CryptoStream cs = new CryptoStream(fsCrypt,
                    RMCrypto.CreateEncryptor(key, key),
                    CryptoStreamMode.Write);

                FileStream fsIn = new FileStream(inputFile, FileMode.Open);

                int data;
                while ((data = fsIn.ReadByte()) != -1)
                    cs.WriteByte((byte)data);


                fsIn.Close();
                cs.Close();
                fsCrypt.Close();
            }
            catch (Exception ex)
            {
                string exmsg = ex.Message;
                //MessageBox.Show("Encryption failed!", "Error");
            }
        }

    } 
}