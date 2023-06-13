using ProjectManager.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManagementTool._content_pages.Convert_to_blob
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
                    BindProject();
                    LblProgress.Visible = false;
                }

            }
        }

        private void BindProject()
        {
            DataTable ds = new DataTable();
            if (Session["TypeOfUser"].ToString() == "U" || Session["TypeOfUser"].ToString() == "MD" || Session["TypeOfUser"].ToString() == "VP")
            {
                ds = gettk.GetProjects();
            }
            else if (Session["TypeOfUser"].ToString() == "PA")
            {
                //ds = gettk.GetProjects_by_UserUID(new Guid(Session["UserUID"].ToString()));
                ds = gettk.GetAssignedProjects_by_UserUID(new Guid(Session["UserUID"].ToString()));
            }
            else
            {
                //ds = gettk.GetProjects();
                ds = gettk.GetAssignedProjects_by_UserUID(new Guid(Session["UserUID"].ToString()));
            }
            DDlProject.DataTextField = "ProjectName";
            DDlProject.DataValueField = "ProjectUID";
            DDlProject.DataSource = ds;
            DDlProject.DataBind();

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            LblProgress.Visible = true;
            LblMessage.Visible = false;
            int TotalDocuments = 0, SuccessFullyConverted = 0, Errored = 0, StatusInsert = 0, StatusError = 0, VersionSuccess = 0, VersionError = 0, FileNotFound = 0;
            //for blob
            string Connectionstring = getdt.getProjectConnectionString(new Guid(DDlProject.SelectedValue));
            if (RBList.SelectedValue == "Actual Documents")
            {
                DataSet ds = getdt.GetAllDocumentsby_ProjectUID(new Guid(DDlProject.SelectedValue));


                if (ds.Tables[0].Rows.Count > 0)
                {
                    TotalDocuments = ds.Tables[0].Rows.Count;
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Guid ActualDocumentUID = new Guid(ds.Tables[0].Rows[i]["ActualDocumentUID"].ToString());
                        string path = ""; //Server.MapPath(ds.Tables[0].Rows[i]["ActualDocument_Path"].ToString());
                        try
                        {
                            path = Server.MapPath(ds.Tables[0].Rows[i]["ActualDocument_Path"].ToString());
                            if (File.Exists(path))
                            {
                                byte[] filetobytes = getdt.FileToByteArray(path);

                                int aDoc = getdt.ActualDocumentBlobInsertorUpdate(Guid.NewGuid(), ActualDocumentUID, filetobytes, Connectionstring);
                                if (aDoc > 0)
                                {
                                    //Insert into Logs table
                                    SuccessFullyConverted += 1;
                                    int alog = getdt.DocumenttoBlobLog_Insert(Guid.NewGuid(), ActualDocumentUID, "ActualDocuments", "Success", path);
                                    //Insert into DocumentStatus Blob Table
                                    DataSet dsstatus = getdt.getActualDocumentStatusList(ActualDocumentUID);
                                    if (dsstatus.Tables[0].Rows.Count > 0)
                                    {
                                        for (int j = 0; j < dsstatus.Tables[0].Rows.Count; j++)
                                        {
                                            try
                                            {
                                                byte[] Reviewfiletobytes = null;
                                                byte[] Coverfilebytes = null;
                                                string coverLetterPath = dsstatus.Tables[0].Rows[j]["CoverLetterFile"].ToString();
                                                string RevieFilePath = "~/_modal_pages/" + dsstatus.Tables[0].Rows[j]["LinkToReviewFile"].ToString();

                                                if (!string.IsNullOrEmpty(dsstatus.Tables[0].Rows[j]["LinkToReviewFile"].ToString()) && dsstatus.Tables[0].Rows[j]["LinkToReviewFile"] != DBNull.Value)
                                                {
                                                    Reviewfiletobytes = getdt.FileToByteArray(Server.MapPath(RevieFilePath));
                                                }
                                                if (!string.IsNullOrEmpty(dsstatus.Tables[0].Rows[j]["CoverLetterFile"].ToString()))
                                                {
                                                    Coverfilebytes = getdt.FileToByteArray(Server.MapPath(coverLetterPath));
                                                }
                                                int statuccount = getdt.DocumentStatusBlob_InsertorUpdate(Guid.NewGuid(), new Guid(dsstatus.Tables[0].Rows[j]["StatusUID"].ToString()), new Guid(dsstatus.Tables[0].Rows[j]["DocumentUID"].ToString()), Coverfilebytes, Reviewfiletobytes, Connectionstring);
                                                if (statuccount > 0)
                                                {
                                                    StatusInsert += 1;
                                                    int log = getdt.DocumenttoBlobLog_Insert(Guid.NewGuid(), new Guid(dsstatus.Tables[0].Rows[j]["StatusUID"].ToString()), "DocumentStatus", "Success", coverLetterPath);
                                                }
                                                else
                                                {
                                                    StatusError += 1;
                                                    int log = getdt.DocumenttoBlobLog_Insert(Guid.NewGuid(), new Guid(dsstatus.Tables[0].Rows[j]["StatusUID"].ToString()), "DocumentStatus", "Error", coverLetterPath);
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                StatusError += 1;
                                            }

                                            //Insert into DocumentVersionBlob Table
                                            byte[] VersionDoc = null;
                                            byte[] CoverFileDoc = null;
                                            DataSet dsversion = getdt.getDocumentVersions_by_StatusUID(new Guid(dsstatus.Tables[0].Rows[j]["StatusUID"].ToString()));
                                            for (int k = 0; k < dsversion.Tables[0].Rows.Count; k++)
                                            {
                                                try
                                                {

                                                    string Versonpath = "";
                                                    if (!string.IsNullOrEmpty(dsversion.Tables[0].Rows[k]["Doc_FileName"].ToString()))
                                                    {
                                                        VersionDoc = getdt.FileToByteArray(Server.MapPath(dsversion.Tables[0].Rows[k]["Doc_FileName"].ToString()));
                                                    }

                                                    if (!string.IsNullOrEmpty(dsversion.Tables[0].Rows[k]["Doc_CoverLetter"].ToString()))
                                                    {
                                                        CoverFileDoc = getdt.FileToByteArray(Server.MapPath(dsversion.Tables[0].Rows[k]["Doc_CoverLetter"].ToString()));
                                                    }

                                                    int versionCnt = getdt.DocumentVersionBlob_insertorUpdate(Guid.NewGuid(), new Guid(dsversion.Tables[0].Rows[k]["DocVersion_UID"].ToString()), ActualDocumentUID, CoverFileDoc, VersionDoc, Connectionstring);
                                                    if (versionCnt > 0)
                                                    {
                                                        VersionSuccess += 1;
                                                        int log = getdt.DocumenttoBlobLog_Insert(Guid.NewGuid(), new Guid(dsversion.Tables[0].Rows[k]["DocVersion_UID"].ToString()), "DocumentVesrion", "Success", dsversion.Tables[0].Rows[k]["Doc_FileName"].ToString());
                                                    }
                                                    else
                                                    {
                                                        VersionError += 1;
                                                        int log = getdt.DocumenttoBlobLog_Insert(Guid.NewGuid(), new Guid(dsversion.Tables[0].Rows[k]["DocVersion_UID"].ToString()), "DocumentVesrion", "Error", dsversion.Tables[0].Rows[k]["Doc_FileName"].ToString());
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    VersionError += 1;
                                                    int log = getdt.DocumenttoBlobLog_Insert(Guid.NewGuid(), new Guid(dsversion.Tables[0].Rows[k]["DocVersion_UID"].ToString()), "DocumentVesrion", "Error", dsversion.Tables[0].Rows[k]["Doc_FileName"].ToString());
                                                }
                                            }
                                        }
                                    }


                                }
                                else
                                {
                                    Errored += 1;
                                    int elog = getdt.DocumenttoBlobLog_Insert(Guid.NewGuid(), ActualDocumentUID, "ActualDocuments", "Failure", path);
                                }
                            }
                            else
                            {
                                FileNotFound += 1;
                            }
                        }
                        catch (Exception ex)
                        {
                            Errored += 1;

                            int log = getdt.DocumenttoBlobLog_Insert(Guid.NewGuid(), ActualDocumentUID, "ActualDocuments", "Failure", path);
                        }
                    }


                }
                LblMessage.Text = "Total Documents : " + TotalDocuments + ", FileNotFound : " + FileNotFound + " Converted Documents : " + SuccessFullyConverted + ", Errored Documents : " + Errored;
                LblProgress.Visible = false;
                LblMessage.Visible = true;
            }
            else if (RBList.SelectedValue == "Document Attachments")
            {
                DataSet ds = getdt.GetAllDocumentsAtatchmentsby_ProjectUID(new Guid(DDlProject.SelectedValue));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    TotalDocuments = ds.Tables[0].Rows.Count;
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Guid AttachmentUID = new Guid(ds.Tables[0].Rows[i]["AttachmentUID"].ToString());
                        try
                        {
                            string path = Server.MapPath(ds.Tables[0].Rows[i]["AttachmentFile"].ToString());
                            if (File.Exists(path))
                            {
                                byte[] filetobytes = getdt.FileToByteArray(path);
                                int gDoc = getdt.InsertDocumentsAttachmentsBlob(AttachmentUID, new Guid(ds.Tables[0].Rows[i]["AttachmentUID"].ToString()), new Guid(ds.Tables[0].Rows[i]["StatusUID"].ToString()), filetobytes, Connectionstring);
                                if (gDoc != 0)
                                {
                                    SuccessFullyConverted += 1;
                                    int alog = getdt.DocumenttoBlobLog_Insert(Guid.NewGuid(), AttachmentUID, "DocumentsAttachments", "Success", path);
                                }
                                else
                                {
                                    Errored += 1;
                                    int elog = getdt.DocumenttoBlobLog_Insert(Guid.NewGuid(), AttachmentUID, "DocumentsAttachments", "Failure", path);
                                }
                            }
                            else
                            {
                                FileNotFound += 1;
                                int elog = getdt.DocumenttoBlobLog_Insert(Guid.NewGuid(), AttachmentUID, "DocumentsAttachments", "Failure", path);
                            }

                        }
                        catch (Exception ex)
                        {
                            Errored += 1;
                        }
                    }
                }
                LblMessage.Text = "Total Documents : " + TotalDocuments + ", FileNotFound : " + FileNotFound + " Converted Documents : " + SuccessFullyConverted + ", Errored Documents : " + Errored;
                LblProgress.Visible = false;
                LblMessage.Visible = true;
            }

            else if (RBList.SelectedValue == "General Documents")
            {
                DataSet ds = getdt.GetAllGeneralDocuments();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    TotalDocuments = ds.Tables[0].Rows.Count;
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Guid GeneralDocumentUID = new Guid(ds.Tables[0].Rows[i]["GeneralDocumentUID"].ToString());
                        try
                        {
                            string path = Server.MapPath(ds.Tables[0].Rows[i]["GeneralDocument_Path"].ToString());
                            if (File.Exists(path))
                            {
                                byte[] filetobytes = getdt.FileToByteArray(path);
                                int gDoc = getdt.GeneralDocumentBlobInsertorUpdate(Guid.NewGuid(), GeneralDocumentUID, filetobytes);
                                if (gDoc > 0)
                                {
                                    SuccessFullyConverted += 1;
                                    int alog = getdt.DocumenttoBlobLog_Insert(Guid.NewGuid(), GeneralDocumentUID, "GeneralDocuments", "Success", path);
                                }
                                else
                                {
                                    Errored += 1;
                                    int elog = getdt.DocumenttoBlobLog_Insert(Guid.NewGuid(), GeneralDocumentUID, "GeneralDocuments", "Failure", path);
                                }
                            }
                            else
                            {
                                FileNotFound += 1;
                                int elog = getdt.DocumenttoBlobLog_Insert(Guid.NewGuid(), GeneralDocumentUID, "GeneralDocuments", "Failure", path);
                            }

                        }
                        catch (Exception ex)
                        {
                            Errored += 1;
                        }
                    }
                }

                LblMessage.Text = "Total Documents : " + TotalDocuments + ", FileNotFound : " + FileNotFound + " Converted Documents : " + SuccessFullyConverted + ", Errored Documents : " + Errored;
                LblProgress.Visible = false;
                LblMessage.Visible = true;
            }
            else if (RBList.SelectedValue == "Document Status")
            {
                //added by zuber
                //Insert into DocumentStatus Blob Table
                DataSet dsstatus = getdt.blob_GetDocumentStatusPending();
                TotalDocuments = dsstatus.Tables[0].Rows.Count;
                string coverLetterPath = string.Empty;
                string RevieFilePath = string.Empty;
                if (dsstatus.Tables[0].Rows.Count > 0)
                {
                    for (int j = 0; j < dsstatus.Tables[0].Rows.Count; j++)
                    {
                        coverLetterPath = string.Empty;
                        RevieFilePath = string.Empty;

                        try
                        {
                            byte[] Reviewfiletobytes = null;
                            byte[] Coverfilebytes = null;
                            coverLetterPath = dsstatus.Tables[0].Rows[j]["CoverLetterFile"].ToString();
                            RevieFilePath = "~/_modal_pages/" + dsstatus.Tables[0].Rows[j]["LinkToReviewFile"].ToString();

                            if (!string.IsNullOrEmpty(dsstatus.Tables[0].Rows[j]["LinkToReviewFile"].ToString()) && dsstatus.Tables[0].Rows[j]["LinkToReviewFile"] != DBNull.Value)
                            {
                                Reviewfiletobytes = getdt.FileToByteArray(Server.MapPath(RevieFilePath));
                            }
                            if (!string.IsNullOrEmpty(dsstatus.Tables[0].Rows[j]["CoverLetterFile"].ToString()))
                            {
                                Coverfilebytes = getdt.FileToByteArray(Server.MapPath(coverLetterPath));
                            }
                            int statuccount = getdt.DocumentStatusBlob_InsertorUpdate(Guid.NewGuid(), new Guid(dsstatus.Tables[0].Rows[j]["StatusUID"].ToString()), new Guid(dsstatus.Tables[0].Rows[j]["DocumentUID"].ToString()), Coverfilebytes, Reviewfiletobytes, Connectionstring);
                            if (statuccount > 0)
                            {
                                StatusInsert += 1;
                                int log = getdt.DocumenttoBlobLog_Insert(Guid.NewGuid(), new Guid(dsstatus.Tables[0].Rows[j]["StatusUID"].ToString()), "DocumentStatus", "Success", coverLetterPath);
                            }
                            else
                            {
                                StatusError += 1;
                                int log = getdt.DocumenttoBlobLog_Insert(Guid.NewGuid(), new Guid(dsstatus.Tables[0].Rows[j]["StatusUID"].ToString()), "DocumentStatus", "Error", coverLetterPath);
                            }
                        }
                        catch (Exception ex)
                        {
                            StatusError += 1;
                            int log = getdt.DocumenttoBlobLog_Insert(Guid.NewGuid(), new Guid(dsstatus.Tables[0].Rows[j]["StatusUID"].ToString()), "DocumentStatus", "Error" + ex.Message, coverLetterPath + " link path" + RevieFilePath);
                        }

                    }
                }
                LblProgress.Visible = false;
                LblMessage.Visible = true;
                LblMessage.Text = "Total Documents : " + TotalDocuments + ", FileNotFound : " + FileNotFound + " Converted Documents : " + StatusInsert + ", Errored Documents : " + StatusError;
            }
            else if (RBList.SelectedValue == "Issues")
            {
                DataSet ds = getdt.GetAllIssueDocs_by_ProjectUID(new Guid(DDlProject.SelectedValue));

                if (ds.Tables[0].Rows.Count > 0)
                {
                    TotalDocuments = ds.Tables[0].Rows.Count;

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Guid Issue_Uid = new Guid(ds.Tables[0].Rows[i]["Issue_Uid"].ToString());

                        try
                        {
                            string path = Server.MapPath(ds.Tables[0].Rows[i]["doc_path"].ToString() + "/" + ds.Tables[0].Rows[i]["doc_name"].ToString());

                            if (File.Exists(path))
                            {
                                byte[] filetobytes = getdt.FileToByteArray(path);

                                int gDoc = getdt.IssueDocBlobUpdate(Convert.ToInt32(ds.Tables[0].Rows[i]["doc_id"].ToString()), filetobytes);

                                if (gDoc > 0)
                                {
                                    SuccessFullyConverted += 1;
                                    int alog = getdt.DocumenttoBlobLog_Insert(Guid.NewGuid(), Issue_Uid, "Issues", "Success", path);

                                    DataSet iRemarks = getdt.GetAllIssueStatusDocs_by_IssueUID(Issue_Uid);

                                    if (iRemarks.Tables[0].Rows.Count > 0)
                                    {
                                        for (int j = 0; j < iRemarks.Tables[0].Rows.Count; j++)
                                        {
                                            try
                                            {
                                                byte[] Remarkfilebytes = null;
                                                string remarkspath = Server.MapPath(ds.Tables[0].Rows[i]["doc_path"].ToString() + "/" + ds.Tables[0].Rows[i]["doc_name"].ToString());
                                                string RemarkFilePath = ds.Tables[0].Rows[i]["doc_path"].ToString() + "/" + ds.Tables[0].Rows[i]["doc_name"].ToString();

                                                if (File.Exists(remarkspath))
                                                {
                                                    Remarkfilebytes = getdt.FileToByteArray(remarkspath);

                                                    int cnt = getdt.IssueStatusDocBlobUpdate(Convert.ToInt32(iRemarks.Tables[0].Rows[j]["uploaded_doc_id"].ToString()), Remarkfilebytes);

                                                    if (cnt > 0)
                                                    {
                                                        StatusInsert += 1;
                                                        int log = getdt.DocumenttoBlobLog_Insert(Guid.NewGuid(), new Guid(iRemarks.Tables[0].Rows[j]["IssueRemarksUID"].ToString()), "IssueRemarks", "Success", RemarkFilePath);
                                                    }
                                                    else
                                                    {
                                                        StatusError += 1;
                                                        int log = getdt.DocumenttoBlobLog_Insert(Guid.NewGuid(), new Guid(iRemarks.Tables[0].Rows[j]["IssueRemarksUID"].ToString()), "IssueRemarks", "Error", RemarkFilePath);
                                                    }
                                                }

                                            }
                                            catch (Exception ex)
                                            {
                                                StatusError += 1;
                                            }
                                        }
                                    }

                                }
                                else
                                {
                                    Errored += 1;
                                    int elog = getdt.DocumenttoBlobLog_Insert(Guid.NewGuid(), Issue_Uid, "Issues", "Failure", path);
                                }
                            }
                            else
                            {
                                FileNotFound += 1;
                            }

                        }
                        catch (Exception ex)
                        {
                            Errored += 1;

                            int log = getdt.DocumenttoBlobLog_Insert(Guid.NewGuid(), Issue_Uid, "Issues", "Failure", "");
                        }
                    }
                }

                LblMessage.Text = "Total Documents : " + TotalDocuments + ", FileNotFound : " + FileNotFound + " Converted Documents : " + SuccessFullyConverted + ", Errored Documents : " + Errored;
                LblProgress.Visible = false;
                LblMessage.Visible = true;
            }
            else if (RBList.SelectedValue == "Bank")
            {

                DataSet ds = getdt.GetAllBankDocuments_by_ProjectUID(new Guid(DDlProject.SelectedValue));

                if (ds.Tables[0].Rows.Count > 0)
                {
                    TotalDocuments = ds.Tables[0].Rows.Count;

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        try
                        {
                            string path = Server.MapPath(ds.Tables[0].Rows[i]["Document_File"].ToString());
                            string fileName = Path.GetFileName(ds.Tables[0].Rows[i]["Document_File"].ToString());

                            if (File.Exists(path))
                            {
                                byte[] filetobytes = getdt.FileToByteArray(path);

                                int gDoc = getdt.BankDocBlobInsertorUpdate(new Guid(ds.Tables[0].Rows[i]["BankDoc_UID"].ToString()), filetobytes, fileName);

                                if (gDoc > 0)
                                {
                                    SuccessFullyConverted += 1;
                                    int alog = getdt.DocumenttoBlobLog_Insert(Guid.NewGuid(), new Guid(ds.Tables[0].Rows[i]["BankDoc_UID"].ToString()), "BankDocuments", "Success", path);
                                }
                                else
                                {
                                    Errored += 1;
                                    int elog = getdt.DocumenttoBlobLog_Insert(Guid.NewGuid(), new Guid(ds.Tables[0].Rows[i]["BankDoc_UID"].ToString()), "BankDocuments", "Failure", path);
                                }

                            }
                            else
                            {
                                FileNotFound += 1;
                            }
                        }
                        catch
                        {
                            Errored += 1;

                            int log = getdt.DocumenttoBlobLog_Insert(Guid.NewGuid(), new Guid(ds.Tables[0].Rows[i]["BankDoc_UID"].ToString()), "BankDocuments", "Failure", "");
                        }
                    }
                }

                LblMessage.Text = "Total Documents : " + TotalDocuments + ", FileNotFound : " + FileNotFound + " Converted Documents : " + SuccessFullyConverted + ", Errored Documents : " + Errored;
                LblProgress.Visible = false;
                LblMessage.Visible = true;
            }
            else if (RBList.SelectedValue == "Insurance")
            {
                DataSet ds = getdt.GetAllInsuranceDocuments_by_ProjectUID(new Guid(DDlProject.SelectedValue));

                if (ds.Tables[0].Rows.Count > 0)
                {
                    TotalDocuments = ds.Tables[0].Rows.Count;

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        try
                        {

                            string path = Server.MapPath(ds.Tables[0].Rows[i]["InsuranceDoc_FilePath"].ToString());
                            string fileName = Path.GetFileName(ds.Tables[0].Rows[i]["InsuranceDoc_FilePath"].ToString());

                            if (File.Exists(path))
                            {
                                byte[] filetobytes = getdt.FileToByteArray(path);

                                int gDoc = getdt.InsuranceDocBlobInsertorUpdate(new Guid(ds.Tables[0].Rows[i]["InsuranceDoc_UID"].ToString()), filetobytes, fileName);

                                if (gDoc > 0)
                                {
                                    SuccessFullyConverted += 1;
                                    int alog = getdt.DocumenttoBlobLog_Insert(Guid.NewGuid(), new Guid(ds.Tables[0].Rows[i]["InsuranceDoc_UID"].ToString()), "InsuranceDocuments", "Success", path);
                                }
                                else
                                {
                                    Errored += 1;
                                    int elog = getdt.DocumenttoBlobLog_Insert(Guid.NewGuid(), new Guid(ds.Tables[0].Rows[i]["InsuranceDoc_UID"].ToString()), "InsuranceDocuments", "Failure", path);
                                }

                            }
                            else
                            {
                                FileNotFound += 1;
                            }
                        }
                        catch
                        {
                            Errored += 1;

                            int log = getdt.DocumenttoBlobLog_Insert(Guid.NewGuid(), new Guid(ds.Tables[0].Rows[i]["InsuranceDoc_UID"].ToString()), "InsuranceDocuments", "Failure", "");
                        }
                    }
                }

                LblMessage.Text = "Total Documents : " + TotalDocuments + ", FileNotFound : " + FileNotFound + " Converted Documents : " + SuccessFullyConverted + ", Errored Documents : " + Errored;
                LblProgress.Visible = false;
                LblMessage.Visible = true;
            }
            else if (RBList.SelectedValue == "Insurance Premium")
            {
                DataSet ds = getdt.GetAllInsurancePremiums_by_ProjectUID(new Guid(DDlProject.SelectedValue));

                if (ds.Tables[0].Rows.Count > 0)
                {
                    TotalDocuments = ds.Tables[0].Rows.Count;

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        try
                        {
                            string path = Server.MapPath(ds.Tables[0].Rows[i]["Premium_Receipt"].ToString());
                            string fileName = Path.GetFileName(ds.Tables[0].Rows[i]["Premium_Receipt"].ToString());

                            if (File.Exists(path))
                            {
                                byte[] filetobytes = getdt.FileToByteArray(path);

                                int gDoc = getdt.InsurancePremiumBlobInsertorUpdate(new Guid(ds.Tables[0].Rows[i]["PremiumUID"].ToString()), filetobytes);

                                if (gDoc > 0)
                                {
                                    SuccessFullyConverted += 1;
                                    int alog = getdt.DocumenttoBlobLog_Insert(Guid.NewGuid(), new Guid(ds.Tables[0].Rows[i]["PremiumUID"].ToString()), "InsurancePremiumBlob", "Success", path);
                                }
                                else
                                {
                                    Errored += 1;
                                    int elog = getdt.DocumenttoBlobLog_Insert(Guid.NewGuid(), new Guid(ds.Tables[0].Rows[i]["PremiumUID"].ToString()), "InsurancePremiumBlob", "Failure", path);
                                }

                            }
                            else
                            {
                                FileNotFound += 1;
                            }
                        }
                        catch
                        {
                            Errored += 1;

                            int log = getdt.DocumenttoBlobLog_Insert(Guid.NewGuid(), new Guid(ds.Tables[0].Rows[i]["PremiumUID"].ToString()), "InsurancePremiumBlob", "Failure", "");
                        }
                    }
                }

                LblMessage.Text = "Total Documents : " + TotalDocuments + ", FileNotFound : " + FileNotFound + " Converted Documents : " + SuccessFullyConverted + ", Errored Documents : " + Errored;
                LblProgress.Visible = false;
                LblMessage.Visible = true;
            }
            else if (RBList.SelectedValue == "RABill")
            {
                DataSet ds = getdt.GetAllRABillDocuments_by_ProjectUID(new Guid(DDlProject.SelectedValue));

                if (ds.Tables[0].Rows.Count > 0)
                {
                    TotalDocuments = ds.Tables[0].Rows.Count;

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        try
                        {
                            string path = Server.MapPath(ds.Tables[0].Rows[i]["FilePath"].ToString());
                            string fileName = Path.GetFileName(ds.Tables[0].Rows[i]["FilePath"].ToString());

                            string getExtension = Path.GetExtension(path);
                            string outPath = path.Replace(getExtension, "") + "_DE" + getExtension;

                            if (File.Exists(outPath))
                            {

                                byte[] filetobytes = getdt.FileToByteArray(outPath);


                                int gDoc = getdt.RABillBlobUpdate(new Guid(ds.Tables[0].Rows[i]["RABillUid"].ToString()), filetobytes);

                                if (gDoc > 0)
                                {
                                    SuccessFullyConverted += 1;
                                    int alog = getdt.DocumenttoBlobLog_Insert(Guid.NewGuid(), new Guid(ds.Tables[0].Rows[i]["RABillUid"].ToString()), "RABillDocuments", "Success", path);
                                }
                                else
                                {
                                    Errored += 1;
                                    int elog = getdt.DocumenttoBlobLog_Insert(Guid.NewGuid(), new Guid(ds.Tables[0].Rows[i]["RABillUid"].ToString()), "RABillDocuments", "Failure", path);
                                }

                            }
                            else
                            {
                                FileNotFound += 1;
                            }
                        }
                        catch
                        {
                            Errored += 1;

                            int log = getdt.DocumenttoBlobLog_Insert(Guid.NewGuid(), new Guid(ds.Tables[0].Rows[i]["RABillUid"].ToString()), "RABillDocuments", "Failure", "");
                        }
                    }

                }

                LblMessage.Text = "Total Documents : " + TotalDocuments + ", FileNotFound : " + FileNotFound + " Converted Documents : " + SuccessFullyConverted + ", Errored Documents : " + Errored;
                LblProgress.Visible = false;
                LblMessage.Visible = true;
            }
            else if (RBList.SelectedValue == "Photograph")
            {

                DataSet ds = getdt.GetSiteLatestPhotograph_by_ProjectUID(new Guid(DDlProject.SelectedValue));

                if (ds.Tables[0].Rows.Count > 0)
                {
                    TotalDocuments = ds.Tables[0].Rows.Count;

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        try
                        {
                            string path = Server.MapPath(ds.Tables[0].Rows[i]["Site_Image"].ToString());
                            string fileName = Path.GetFileName(ds.Tables[0].Rows[i]["Site_Image"].ToString());

                            if (File.Exists(path))
                            {
                                byte[] filetobytes = getdt.FileToByteArray(path);

                                int gDoc = getdt.InsertOrUpdateSitePhotographBlob(ds.Tables[0].Rows[i]["SitePhotograph_UID"].ToString(), filetobytes, fileName, ds.Tables[0].Rows[i]["Site_Image"].ToString());

                                if (gDoc == -1)
                                {
                                    SuccessFullyConverted += 1;
                                    int alog = getdt.DocumenttoBlobLog_Insert(Guid.NewGuid(), new Guid(ds.Tables[0].Rows[i]["SitePhotograph_UID"].ToString()), "SitePhotographs", "Success", path);
                                }
                                else
                                {
                                    Errored += 1;
                                    int elog = getdt.DocumenttoBlobLog_Insert(Guid.NewGuid(), new Guid(ds.Tables[0].Rows[i]["SitePhotograph_UID"].ToString()), "SitePhotographs", "Failure", path);
                                }

                            }
                            else
                            {
                                FileNotFound += 1;
                            }
                        }
                        catch
                        {
                            Errored += 1;

                            int log = getdt.DocumenttoBlobLog_Insert(Guid.NewGuid(), new Guid(ds.Tables[0].Rows[i]["SitePhotograph_UID"].ToString()), "SitePhotographs", "Failure", "");
                        }
                    }
                }

                LblMessage.Text = "Total Documents : " + TotalDocuments + ", FileNotFound : " + FileNotFound + " Converted Documents : " + SuccessFullyConverted + ", Errored Documents : " + Errored;
                LblProgress.Visible = false;
                LblMessage.Visible = true;
            }

        }

        protected void RBList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RBList.SelectedValue == "General Documents" || RBList.SelectedValue == "Document Status")
            {
                DDlProject.Enabled = false;
            }
            else
            {
                DDlProject.Enabled = true;
            }
        }
    }
}