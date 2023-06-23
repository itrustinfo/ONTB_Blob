using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using ProjectManager.DAL;

namespace ProjectManagementTool._content_pages.documents_contractor
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
                    //

                    FirstData("firstload");

                }
                  
            }
        }

        private void FirstData(string type)
        {
            //
            if (Request.QueryString["UserUID"] != null)
            {
                getPendingDocs();
                //
                //  lblTotalcount.Text = "Total Count : " + GrdDocuments.Rows.Count.ToString();
                //if (GrdDocuments.Rows.Count > 15)
                //{
                //    ScriptManager.RegisterStartupScript(Page, this.GetType(), "Key", "<script>MakeStaticHeader('" + GrdDocuments.ClientID + "', 700, 1300 , 45 ,true); </script>", false);
                //}
            }
            else if (Request.QueryString["PrjUID"] != null)
            {
                List<string> FlowUID = new List<string>();

                if (Request.QueryString["type"].ToString() == "Contractor" || Request.QueryString["FlowName"]!= null)
                {
                    ReportFilter.Visible = true;
                    //
                    if(getdt.getProjectNameby_ProjectUID(new Guid(Request.QueryString["PrjUID"])) == "CP-02" || getdt.getProjectNameby_ProjectUID(new Guid(Request.QueryString["PrjUID"])) == "CP-03")
                    {
                        tdOrglbl.Visible = true;
                        tdOrgtxtbox.Visible = true;
                        tdFileNumtxtbox.Visible = true;
                        tdFileReflbl.Visible = true;
                    }
                    //
                    DataSet ds = new DataSet();
                    if (Request.QueryString["FlowName"] != null)
                    {
                        ds = getdt.GetDashboardContractotDocsSubmitted_Details_Type(new Guid(Request.QueryString["PrjUID"]), Request.QueryString["type"].ToString(), Request.QueryString["FlowName"].ToString());
                    }
                    else
                    {
                        ds = getdt.GetDashboardContractotDocsSubmitted_Details(new Guid(Request.QueryString["PrjUID"]));
                    }

                    lblTotalcount.Text = "Total Count : " + ds.Tables[0].Rows.Count;

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        GrdDocuments.DataSource = ds.Tables[0].AsEnumerable()
         .OrderByDescending(r => r.Field<DateTime>("IncomingRec_Date"))
         .CopyToDataTable();

                        GrdDocuments.DataBind();

                        //
                        if (type == "firstload")
                        {
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                FlowUID.Add(dr["FlowUID"].ToString());
                            }
                            BindFlow(FlowUID);
                        }
                        //
                        if(Request.QueryString["Flow"] != null)
                        {
                            DDLFlow.SelectedValue = "f76821a4-289b-468f-92d5-f4965481546c";
                            DDLFlow.Enabled = false;
                            FilterData1();

                        }
                    }
                    else
                    {
                        GrdDocuments.DataSource = ds;
                        GrdDocuments.DataBind();
                    }
                    LblDocumentHeading.Text = "Document List for Contractor -> ONTB";
                    GrdDocuments.Columns[13 + 3].Visible = false;
                    GrdDocuments.Columns[14 + 3].Visible = false;
                    GrdDocuments.Columns[15 + 3].Visible = false;
                    //added on 13/06/2022 after inputs from saladin
                    GrdDocuments.Columns[7 + 3].Visible = true;
                    GrdDocuments.Columns[8 + 3].Visible = false;
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        GrdDocuments.HeaderRow.Cells[7 + 3].Text = "Contractor Uploaded date";
                    }
                    //
                   
                    if (GrdDocuments.Rows.Count > 14)
                    {
                        // ScriptManager.RegisterStartupScript(Page, this.GetType(), "Key", "<script>MakeStaticHeader('" + GrdDocuments.ClientID + "', 700, 1300 , 45 ,true); </script>", false);
                    }
                    ReportFilter.Visible = true;
                }
                else if (Request.QueryString["type"].ToString() == "Recon")
                {
                    DataSet ds = getdt.GetDashboardReconciliationDocs_Details(new Guid(Request.QueryString["PrjUID"]));
                    lblTotalcount.Text = "Total Count : " + ds.Tables[0].Rows.Count;
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        GrdDocuments.DataSource = ds.Tables[0].AsEnumerable()
             .OrderByDescending(r => r.Field<DateTime>("IncomingRec_Date"))
             .CopyToDataTable();
                        GrdDocuments.DataBind();
                    }
                    else
                    {
                        GrdDocuments.DataSource = ds;
                        GrdDocuments.DataBind();
                    }
                    LblDocumentHeading.Text = "Document List for Reconciliation Docs";

                    //added on 09/06/2022 after inputs from saladin
                    //added on 09/06/2022 after inputs from saladin
                    GrdDocuments.Columns[7 + 3].Visible = true;
                    GrdDocuments.Columns[8 + 3].Visible = false;
                    GrdDocuments.Columns[19].Visible = false;
                    GrdDocuments.Columns[20].Visible = false;
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        GrdDocuments.HeaderRow.Cells[7 + 3].Text = "Contractor Uploaded date";
                    }
                    //
                    if (Session["IsContractor"].ToString() == "Y" || Session["TypeOfUser"].ToString() == "NJSD")
                    {
                        GrdDocuments.Columns[13 + 3].Visible = false;
                        GrdDocuments.Columns[14 + 3].Visible = false;
                        GrdDocuments.Columns[15 + 3].Visible = false;

                    }
                    else
                    {
                        GrdDocuments.Columns[13 + 3].Visible = true;
                        GrdDocuments.Columns[14 + 3].Visible = true;
                        GrdDocuments.Columns[15 + 3].Visible = true;
                        btnSubmit.Visible = true;
                    }
                    if (GrdDocuments.Rows.Count > 14)
                    {
                      //  ScriptManager.RegisterStartupScript(Page, this.GetType(), "Key", "<script>MakeStaticHeader('" + GrdDocuments.ClientID + "', 700, 1300 , 45 ,true); </script>", false);
                    }
                }
                else
                {
                    ReportFilter.Visible = true;
                    //
                    if (getdt.getProjectNameby_ProjectUID(new Guid(Request.QueryString["PrjUID"])) == "CP-02" || getdt.getProjectNameby_ProjectUID(new Guid(Request.QueryString["PrjUID"])) == "CP-03")
                    {
                        tdOrglbl.Visible = true;
                        tdOrgtxtbox.Visible = true;
                        tdFileNumtxtbox.Visible = true;
                        tdFileReflbl.Visible = true;
                    }
                    //
                    DataSet ds = getdt.GetDashboardONTBtoContractorDocs_Details(new Guid(Request.QueryString["PrjUID"]));

                    lblTotalcount.Text = "Total Count : " + ds.Tables[0].Rows.Count;

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        GrdDocuments.DataSource = getdt.GetDashboardONTBtoContractorDocs_Details(new Guid(Request.QueryString["PrjUID"])).Tables[0].AsEnumerable()
         .OrderByDescending(r => r.Field<DateTime>("IncomingRec_Date"))
         .CopyToDataTable();
                        GrdDocuments.DataBind();
                    }
                    else
                    {
                        GrdDocuments.DataSource = ds;
                        GrdDocuments.DataBind();
                    }

                    if (type == "firstload")
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            FlowUID.Add(dr["FlowUID"].ToString());
                        }

                        BindFlow(FlowUID);
                    }

                    LblDocumentHeading.Text = "Document List for ONTB -> Contractor";

                    GrdDocuments.Columns[13 + 3].Visible = false;
                    GrdDocuments.Columns[14 + 3].Visible = false;
                    GrdDocuments.Columns[15 + 3].Visible = false;
                    GrdDocuments.Columns[19].Visible = false;
                    GrdDocuments.Columns[20].Visible = false;
                    if (GrdDocuments.Rows.Count > 14)
                    {
                        //ScriptManager.RegisterStartupScript(Page, this.GetType(), "Key", "<script>MakeStaticHeader('" + GrdDocuments.ClientID + "', 700, 1300 , 45 ,true); </script>", false);
                    }
                }
            }

            Session["isDownloadNJSE"] = false;
            DataSet dscheck = new DataSet();
            dscheck = getdt.GetUsertypeFunctionality_Mapping(Session["TypeOfUser"].ToString());
            if (dscheck.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dscheck.Tables[0].Rows)
                {

                    if (dr["Code"].ToString() == "FN") // DOWNLOAD APPROVED DOCUMENTS (FINAL APPROVED BY CLIENT)
                    {
                        ViewState["isDownloadClient"] = "true";
                        Session["isDownloadClient"] = "true";
                    }
                    if (dr["Code"].ToString() == "FM") // DOWNLOAD APPROVED DOCUMENTS (FINAL APPROVED BY NJSEI)
                    {
                        ViewState["isDownloadNJSE"] = "true";
                        Session["isDownloadNJSE"] = "true";
                    }
                }
            }
        
    }

        public string GetTaskHierarchy_By_DocumentUID(string DocumentUID)
        {
            return getdt.GetTaskHierarchy_By_DocumentUID(new Guid(DocumentUID));
        }

        public string GetSubmittalName(string DocumentID)
        {
            return getdt.getDocumentName_by_DocumentUID(new Guid(DocumentID));
        }
        public string GetDocumentTypeIcon(string DocumentExtn)
        {
            return getdt.GetDocumentTypeMasterIcon_by_Extension(DocumentExtn);
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

        //added on 09/06/2022
        public string GetFlowName(string SubmittalUID)
        {
            string retval = getdt.GetFlowName_by_SubmittalID(new Guid(SubmittalUID));
            if (retval == null || retval == "")
            {
                return "N/A";
            }
            else
            {
                return retval;
            }
        }

        protected void GrdDocuments_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            //LinkButton lnkbtn;
            //if (e.Row.RowType == DataControlRowType.Header)
            //{
            //    foreach (TableCell cell in e.Row.Cells)
            //    {
            //        lnkbtn = (LinkButton)cell.Controls[0];
            //        if (!string.IsNullOrEmpty(GrdDocuments.SortExpression))
            //        {
            //            if (GrdDocuments.SortExpression.Equals(lnkbtn.Text))
            //            {
            //                cell.BackColor = System.Drawing.Color.Crimson;
            //            }
            //        }
            //    }
            //}
            //----------


            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                DataSet ds = getdt.getTop1_DocumentStatusSelect(new Guid(e.Row.Cells[0 + 2].Text));
                Label lblDocumentName = (Label)e.Row.FindControl("lblDocumentName");
                //
                if (ds != null)
                {
                    if (ds.Tables[0].Rows[0]["DocumentType"].ToString() == "General Document")
                    {
                        e.Row.Cells[11 + 3].Text = "No History";
                    }
                }
                if (ds.Tables[0].Rows[0]["ActivityType"].ToString() != "" && ds.Tables[0].Rows[0]["TopVersion"].ToString() != "")
                {
                    //e.Row.Cells[1].Text = ds.Tables[0].Rows[0]["TopVersion"].ToString();

                    try { 
                    string newVersionFileName = Path.GetFileNameWithoutExtension(Server.MapPath(ds.Tables[0].Rows[0]["Doc_Path"].ToString()));
                    lblDocumentName.Text = newVersionFileName.Substring(0, (newVersionFileName.Length - 2)) + " [ Ver. " + ds.Tables[0].Rows[0]["TopVersion"].ToString() + " ]";
                    e.Row.Cells[4 + 2].Text = ds.Tables[0].Rows[0]["ActivityType"].ToString();
                      }   
                   catch
                        {
                        e.Row.Cells[4 + 2].Text = ds.Tables[0].Rows[0]["ActivityType"].ToString();
                    }
                }
                //
                //added on 27/06/2022 for next user action from current status
                string SubmittalUID_Next = e.Row.Cells[22].Text;  //getdt.GetSubmittalUID_By_ActualDocumentUID(new Guid(e.Row.Cells[0 + 2].Text));
                string FlowUID = e.Row.Cells[21].Text;  // getdt.GetFlowUIDBySubmittalUID(new Guid(SubmittalUID_Next));
                e.Row.Cells[7].Text = getdt.NextAction_ByCurrentStatus(new Guid(FlowUID), e.Row.Cells[6].Text);
                //
                if (Session["IsContractor"].ToString() == "Y")
                {
                    string SubmittalUID = getdt.GetSubmittalUID_By_ActualDocumentUID(new Guid(e.Row.Cells[0 + 2].Text));
                    string phase = getdt.GetPhaseforStatus(new Guid(getdt.GetFlowUIDBySubmittalUID(new Guid(SubmittalUID))), e.Row.Cells[4 + 2].Text);
                    //string phase = getdata.GetPhaseforStatus(new Guid(Request.QueryString["FlowUID"]), e.Row.Cells[3].Text);

                    string Flowtype = getdt.GetFlowTypeBySubmittalUID(new Guid(SubmittalUID));
                    if (Flowtype == "STP" || Flowtype == "STP-C")
                    {
                        if (string.IsNullOrEmpty(phase))
                        {

                            //if (e.Row.Cells[4].Text == "Code A-CE Approval" || e.Row.Cells[4].Text == "Client CE GFC Approval")
                            //{
                            //    e.Row.Cells[4].Text = "Approved";

                            //}
                            //if (e.Row.Cells[4].Text == "Code B-CE Approval" || e.Row.Cells[3].Text == "Code C-CE Approval")
                            //{
                            //    e.Row.Cells[4].Text = "Under Client Approval Process";
                            //}
                            //
                            if (e.Row.Cells[4 + 2].Text == "Code A-CE Approval")
                            {
                                e.Row.Cells[4 + 2].Text = "Approved By BWSSB Under Code A";

                            }
                            else if (e.Row.Cells[4 + 2].Text == "Code B-CE Approval")
                            {
                                e.Row.Cells[4 + 2].Text = "Approved By BWSSB Under Code B";
                            }
                            else if (e.Row.Cells[4 + 2].Text == "Code C-CE Approval")
                            {
                                e.Row.Cells[4 + 2].Text = "Under Client Approval Process";

                            }
                            else if (e.Row.Cells[4 + 2].Text == "Client CE GFC Approval")
                            {
                                e.Row.Cells[4 + 2].Text = "Approved GFC by BWSSB";
                            }
                        }
                        else
                        {
                            e.Row.Cells[4 + 2].Text = phase;
                        }
                    }

                }
                //
                if (e.Row.Cells[4 + 2].Text.Contains("Reconciliation"))
                {
                    e.Row.Cells[1 + 2].Text = GetSubmittalName(getdt.GetSubmittalUID_By_ActualDocumentUID(new Guid(e.Row.Cells[0 + 2].Text)));
                }
                //
                if (Request.QueryString["UserUID"] != null)
                {
                    e.Row.Visible = false;

                    DataSet dsNext = getdt.GetNextStep_By_DocumentUID(new Guid(e.Row.Cells[0 + 2].Text), ds.Tables[0].Rows[0]["ActivityType"].ToString());
                    DataSet dsNxtUser = new DataSet();
                    foreach (DataRow dr in dsNext.Tables[0].Rows)
                    {
                        dsNxtUser = getdt.GetNextUser_By_DocumentUID(new Guid(e.Row.Cells[0 + 2].Text), int.Parse(dr["ForFlow_Step"].ToString()));
                        if (dsNxtUser.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow druser in dsNxtUser.Tables[0].Rows)
                            {
                                if (Session["UserUID"].ToString().ToUpper() == druser["Approver"].ToString().ToUpper())
                                {
                                    if (ds.Tables[0].Rows[0]["ActivityType"].ToString() == "Accepted")
                                    {
                                        if (getdt.checkUserAddedDocumentstatus(new Guid(e.Row.Cells[0 + 2].Text), new Guid(HttpContext.Current.Session["UserUID"].ToString()), ds.Tables[0].Rows[0]["ActivityType"].ToString()) == 0)
                                        {
                                            e.Row.Visible = true;
                                            return;
                                        }
                                        else
                                        {
                                            e.Row.Visible = false;
                                            return;
                                        }
                                    }
                                    else if (ds.Tables[0].Rows[0]["ActivityType"].ToString().ToLower().Contains("back to pmc"))
                                    {
                                        DataSet dsbackUser = getdt.GetBacktoPMCUsers(new Guid(e.Row.Cells[0 + 2].Text));
                                        foreach (DataRow druserb in dsbackUser.Tables[0].Rows)
                                        {
                                            if (druserb["PMCUser"].ToString().ToUpper() == Session["UserUID"].ToString().ToUpper())
                                            {
                                                if (getdt.checkUserAddedDocumentstatus(new Guid(e.Row.Cells[0 + 2].Text), new Guid(HttpContext.Current.Session["UserUID"].ToString()), ds.Tables[0].Rows[0]["ActivityType"].ToString()) == 0)
                                                {
                                                    e.Row.Visible = true;
                                                    return;
                                                }
                                                else
                                                {
                                                    e.Row.Visible = false;
                                                    return;
                                                }
                                            }

                                        }
                                    }
                                    else
                                    {
                                        e.Row.Visible = true;
                                        return;
                                    }

                                }
                                else
                                {
                                    e.Row.Visible = false;

                                }
                            }
                        }
                        else
                        {
                            e.Row.Visible = false;
                        }
                    }
                }
                //

                //added on 26/06/2022 for salahuddins changes to be done

                e.Row.Cells[20].Text = getdt.GetUSP_DocumentAcceptedRejectedRemarks(new Guid(e.Row.Cells[0 + 2].Text));

                DateTime? acceptedRejDate = getdt.GetDocumentAcceptedRecejtedDate(new Guid(e.Row.Cells[0 + 2].Text));
                if (acceptedRejDate != null)
                {
                    if (acceptedRejDate.ToString().Contains("1/1/0001"))
                    {
                        e.Row.Cells[19].Text = "N/A";
                    }
                    else
                    {
                        e.Row.Cells[19].Text = Convert.ToDateTime(acceptedRejDate).ToString("dd/MM/yyyy");
                    }
                }



                if (e.Row.Cells[4 + 2].Text == "Accepted")
                {
                    e.Row.Cells[4 + 2].Text = "Accepted - (Under PMC Review)";
                }
                else if (e.Row.Cells[4 + 2].Text == "Accepted-PMC Comments")
                {
                    e.Row.Cells[4 + 2].Text = "Accepted" + "<br/>" + "(PMC Review Completed)"; ;
                }
                else if (e.Row.Cells[4 + 2].Text.ToLower().Contains("back to pmc"))
                {
                    e.Row.Cells[4 + 2].Text = e.Row.Cells[4 + 2].Text + "<br/>" + "(Under PMC Review)";

                }
            }
        }

        protected void GrdDocuments_DataBound(object sender, EventArgs e)
        {
            //int sortedColumnPosition = 0;
            //LinkButton lnkbtn;

            //// Gets position of column whose header text matches SortExpression
            //// of the GridView when column is sorted
            //foreach (TableCell cell in GrdDocuments.HeaderRow.Cells)
            //{
            //    lnkbtn = (LinkButton)cell.Controls[0];
            //    if (lnkbtn.Text == GrdDocuments.SortExpression)
            //    {
            //        break;
            //    }
            //    sortedColumnPosition++;
            //}
            //if (!string.IsNullOrEmpty(GrdDocuments.SortExpression))
            //{
            //    foreach (GridViewRow row in GrdDocuments.Rows)
            //    {
            //        row.Cells[sortedColumnPosition].BackColor = System.Drawing.Color.LavenderBlush;
            //    }
            //}
        }
        protected void GrdDocuments_RowCommand(object sender, GridViewCommandEventArgs e)
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
                    filename = Path.GetFileName(ds.Tables[0].Rows[0]["Doc_FileName"].ToString());
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
                            filename = ds1.Tables[0].Rows[0]["ActualDocument_Name"].ToString() + ds1.Tables[0].Rows[0]["ActualDocument_Type"].ToString();
                        }
                    }
                }
                // added on  20/10/2020
                //ds.Clear();
                //ds = getdt.ActualDocuments_SelectBy_ActualDocumentUID(new Guid(UID));
                //if (ds.Tables[0].Rows.Count > 0)
                //{
                //    if (ds.Tables[0].Rows[0]["ActualDocument_Name"].ToString() != "")
                //    {
                //        filename = ds.Tables[0].Rows[0]["ActualDocument_Name"].ToString() + ds.Tables[0].Rows[0]["ActualDocument_Type"].ToString();
                //    }
                //}
                //
                try
                {
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
                        Response.AddHeader("Content-Disposition", "attachment; filename=" + filename.Replace(',', '_'));
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
                catch (Exception ex)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Document not uploaded for this flow.Please Contact Document controller!');</script>");
                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e) // reconciliation submit
        {
            try
            {
                foreach (GridViewRow row in GrdDocuments.Rows)
                {
                    
                    RadioButtonList rbSelect = (RadioButtonList)row.FindControl("rdActionsList");
                    if(rbSelect.SelectedValue != "")
                    { 
                    TextBox txtremarks = (TextBox)row.FindControl("txtremarks");
                    TextBox txtrefNo = (TextBox)row.FindControl("txtONTBRefNo");
                        string status = rbSelect.SelectedValue;
                        string remarks = txtremarks.Text;
                        string EmailHeading = string.Empty;
                        string DocumentUID = row.Cells[0+2].Text;
                        string OriginatorRefNo = string.Empty;
                        string ProjectRefNo = string.Empty;
                        if (status == "Accept")
                        {
                            if (String.IsNullOrEmpty(txtrefNo.Text.Trim()))
                            {
                                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Please Enter Reference No !');</script>");
                                return;
                            }
                        }

                        if (!String.IsNullOrEmpty(txtrefNo.Text.Trim()))
                        {
                            if (getdt.checkPrjRefNoExists(new Guid(Request.QueryString["PrjUID"]), txtrefNo.Text.Trim()))
                            {
                                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('ONTB Reference No already Exists.Please choose another Ref No. !');</script>");
                                return;
                            }
                        }
                       
                   string documentname = getdt.GetActualDocumentName_by_ActualDocumentUID(new Guid(DocumentUID));
                        //
                        if (status =="Accept")
                    {
                            if (row.Cells[4+2].Text == "Contractor Submitted 9 Copies")
                            {
                                status = "Accepted 9 Copies";
                            }
                            else
                            {
                                status = "Accepted";
                            }
                            EmailHeading = "Under Review by ONTB";
                    }
                    else
                    {
                            if (row.Cells[4+2].Text == "Contractor Submitted 9 Copies")
                            {
                                status = "Rejected 9 Copies";
                            }
                            else
                            {
                                status = "Rejected";
                            }
                            EmailHeading = "Rejected by ONTB";
                        }
                        //
                       

                        string sDate1 = "";
                        DateTime CDate1 = DateTime.Now;
                        //
                        sDate1 = CDate1.ToString("dd/MM/yyyy");
                        //sDate1 = sDate1.Split('/')[1] + "/" + sDate1.Split('/')[0] + "/" + sDate1.Split('/')[2];
                        sDate1 = getdt.ConvertDateFormat(sDate1);
                        CDate1 = Convert.ToDateTime(sDate1);
                        string DocPath = "";
                    string Subject = Session["Username"].ToString() + " added a new Status.";
                    string sHtmlString = string.Empty;
                    Guid StatusUID = Guid.NewGuid();
                    string CoverPagePath = string.Empty;

                    int Cnt = getdt.InsertorUpdateDocumentStatus(StatusUID, new Guid(DocumentUID), 1, status, 0, CDate1, DocPath,
                        new Guid(Session["UserUID"].ToString()), remarks, status, "", CDate1, CoverPagePath, "Current", "ONTB");
                        if (Cnt > 0)
                        {
                           
                            //DataSet ds = getdata.getAllUsers();
                            DataSet ds = new DataSet();
                            //Update the targte dates 
                            getdt.StoreFreshTargetDatesforStatusChange(new Guid(DocumentUID), status);

                            //Update refno for ONTB ......
                            getdt.UpdateActualDocsRefNo(new Guid(DocumentUID), txtrefNo.Text, "", 1);
                            string RefUID = getdt.GetTopRefNoHistory(new Guid(DocumentUID));
                            if (!string.IsNullOrEmpty(RefUID))
                            {
                                getdt.UpdateRefNoHistory(new Guid(RefUID), txtrefNo.Text, "", 1);
                            }

                            ds = getdt.GetUsers_under_ProjectUID(new Guid(Request.QueryString["PrjUID"]));
                            //
                            DataSet dsTasks = getdt.ActualDocuments_SelectBy_ActualDocumentUID(new Guid(DocumentUID));
                            if (dsTasks.Tables[0].Rows.Count > 0)
                            {
                                OriginatorRefNo = dsTasks.Tables[0].Rows[0]["Ref_Number"].ToString();
                                ProjectRefNo = dsTasks.Tables[0].Rows[0]["ProjectRef_Number"].ToString();
                            }
                            //

                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                string CC = string.Empty;
                                string ToEmailID = "";
                                string sUserName = "";
                                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                                {
                                    if (ds.Tables[0].Rows[i]["UserUID"].ToString() == Session["UserUID"].ToString())
                                    {
                                        ToEmailID = ds.Tables[0].Rows[i]["EmailID"].ToString();
                                        sUserName = ds.Tables[0].Rows[i]["UserName"].ToString();
                                    }
                                    else
                                    {
                                        if (getdt.GetUserMailAccess(new Guid(ds.Tables[0].Rows[i]["UserUID"].ToString()), "documentmail") != 0)
                                        {
                                            CC += ds.Tables[0].Rows[i]["EmailID"].ToString() + ",";
                                        }

                                    }
                                }

                                //
                                CC = CC.TrimEnd(',');
                                sHtmlString = "<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>" + "<html xmlns='http://www.w3.org/1999/xhtml'>" +
                                  "<head>" + "<meta http-equiv='Content-Type' content='text/html; charset=utf-8' />" + "<style>table, th, td {border: 1px solid black; padding:6px;}</style></head>" +
                                     "<body style='font-family:Verdana, Arial, sans-serif; font-size:12px; font-style:normal;'>";
                                sHtmlString += "<div style='width:80%; float:left; padding:1%; border:2px solid #011496; border-radius:5px;'>" +
                                                   "<div style='float:left; width:100%; border-bottom:2px solid #011496;'>";
                                if (WebConfigurationManager.AppSettings["Domain"] == "NJSEI")
                                {
                                    sHtmlString += "<div style='float:left; width:7%;'><img src='https://dm.njsei.com/_assets/images/NJSEI%20Logo.jpg' width='50' /></div>";
                                }
                                else
                                {
                                    sHtmlString += "<div style='float:left; width:7%;'><h2>" + WebConfigurationManager.AppSettings["Domain"] + "</h2></div>";
                                }
                                sHtmlString += "<div style='float:left; width:70%;'><h2 style='margin-top:10px;'>Project Monitoring Tool</h2></div>" +
                                           "</div>";
                                sHtmlString += "<div style='width:100%; float:left;'><br/>Dear Users,<br/><br/><span style='font-weight:bold;'>" + sUserName + " has changed " + documentname + " status.</span> <br/><br/></div>";
                                sHtmlString += "<div style='width:100%; float:left;'><table style='width:100%;'>" +
                                                "<tr><td><b>Document Name </b></td><td style='text-align:center;'><b>:</b></td><td>" + documentname + "</td></tr>" +
                                                "<tr><td><b>Status </b></td><td style='text-align:center;'><b>:</b></td><td>" + status + "</td></tr>" +
                                               "<tr><td><b>Originator Ref. Number </b></td><td style='text-align:center;'><b>:</b></td><td>" + OriginatorRefNo + "</td></tr>" +
                                                    "<tr><td><b>ONTB Ref. Number</b></td><td style='text-align:center;'><b>:</b></td><td>" + ProjectRefNo + "</td></tr>" +
                                                      "<tr><td><b>Date </b></td><td style='text-align:center;'><b>:</b></td><td>" + DateTime.Now.ToString("dd MMM yyyy") + "</td></tr>" +
                                                "<tr><td><b>Comments </b></td><td style='text-align:center;'><b>:</b></td><td>" + remarks + "</td></tr>";
                                sHtmlString += "</table></div>";
                                sHtmlString += "<div style='width:100%; float:left;'><br/><br/>Sincerely, <br/> MIS System.</div></div></body></html>";

                                //sHtmlString = "<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>" + "<html xmlns='http://www.w3.org/1999/xhtml'>" +
                                //       "<head>" + "<meta http-equiv='Content-Type' content='text/html; charset=utf-8' />" + "</head>" +
                                //          "<body style='font-family:Verdana, Arial, sans-serif; font-size:12px; font-style:normal;'>";
                                //sHtmlString += "<div style='float:left; width:100%; height:30px;'>" +
                                //                   "Dear, " + "Users" +
                                //                   "<br/><br/></div>";
                                //sHtmlString += "<div style='width:100%; float:left;'><div style='width:100%; float:left;'>Below are the Status details. <br/><br/></div>";
                                //sHtmlString += "<div style='width:100%; float:left;'><div style='width:100%; float:left;'>Document Name : " + DDLDocument.SelectedItem.Text + "<br/></div>";
                                //sHtmlString += "<div style='width:100%; float:left;'><div style='width:100%; float:left;'>Status : " + DDlStatus.SelectedItem.Text + "<br/></div>";
                                //sHtmlString += "<div style='width:100%; float:left;'><div style='width:100%; float:left;'>Date : " + CDate1.ToShortDateString() + "<br/></div>";
                                //sHtmlString += "<div style='width:100%; float:left;'><br/><br/>Sincerely, <br/> Project Manager.</div></div></body></html>";
                                //string ret = getdata.SendMail(ds.Tables[0].Rows[0]["EmailID"].ToString(), Subject, sHtmlString, CC, Server.MapPath(DocPath));
                                // added on 02/11/2020
                                DataTable dtemailCred = getdt.GetEmailCredentials();
                                Guid MailUID = Guid.NewGuid();
                                getdt.StoreEmaildataToMailQueue(MailUID, new Guid(Session["UserUID"].ToString()), dtemailCred.Rows[0][0].ToString(), ToEmailID, Subject, sHtmlString, CC, "");
                                //
                                // added on 07/01/2022 for sending mail to next user in line to change status...
                                DataSet dsnew = getdt.getTop1_DocumentStatusSelect(new Guid(DocumentUID));
                                DataSet dsNext = getdt.GetNextStep_By_DocumentUID(new Guid(DocumentUID), dsnew.Tables[0].Rows[0]["ActivityType"].ToString());

                                sHtmlString = "<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>" + "<html xmlns='http://www.w3.org/1999/xhtml'>" +
                                "<head>" + "<meta http-equiv='Content-Type' content='text/html; charset=utf-8' />" + "<style>table, th, td {border: 1px solid black; padding:6px;}</style></head>" +
                                   "<body style='font-family:Verdana, Arial, sans-serif; font-size:12px; font-style:normal;'>";
                                sHtmlString += "<div style='width:80%; float:left; padding:1%; border:2px solid #011496; border-radius:5px;'>" +
                                                   "<div style='float:left; width:100%; border-bottom:2px solid #011496;'>";
                                if (WebConfigurationManager.AppSettings["Domain"] == "NJSEI")
                                {
                                    sHtmlString += "<div style='float:left; width:7%;'><img src='https://dm.njsei.com/_assets/images/NJSEI%20Logo.jpg' width='50' /></div>";
                                }
                                else
                                {
                                    sHtmlString += "<div style='float:left; width:7%;'><h2>" + WebConfigurationManager.AppSettings["Domain"] + "</h2></div>";
                                }
                                sHtmlString += "<div style='float:left; width:70%;'><h2 style='margin-top:10px;'>Project Monitoring Tool</h2></div>" +
                                           "</div>";
                                sHtmlString += "<div style='width:100%; float:left;'><br/>Dear Users,<br/><br/><span style='font-weight:bold;'>" + sUserName + " has changed " + documentname + " status.</span> <br/><br/></div>";
                                sHtmlString += "<div style='width:100%; float:left;'><table style='width:100%;'>" +
                                                "<tr><td><b>Document Name </b></td><td style='text-align:center;'><b>:</b></td><td>" + documentname + "</td></tr>" +
                                                "<tr><td><b>Status </b></td><td style='text-align:center;'><b>:</b></td><td>" + status + "</td></tr>" +
                                                 "<tr><td><b>Originator Ref. Number </b></td><td style='text-align:center;'><b>:</b></td><td>" + OriginatorRefNo + "</td></tr>" +
                                                    "<tr><td><b>ONTB Ref. Number</b></td><td style='text-align:center;'><b>:</b></td><td>" + ProjectRefNo + "</td></tr>" +

                                                "<tr><td><b>Date </b></td><td style='text-align:center;'><b>:</b></td><td>" + DateTime.Now.ToString("dd MMM yyyy") + "</td></tr>" +
                                                "<tr><td><b>Comments </b></td><td style='text-align:center;'><b>:</b></td><td>" + remarks + "</td></tr>";
                                sHtmlString += "</table><br /><br /><div style='color: red'>Kindly note that you are to act on this to complete the next step in document flow.</div></div>";
                                sHtmlString += "<div style='width:100%; float:left;'><br/><br/>Sincerely, <br/> MIS System.</div></div></body></html>";
                                Subject = Subject + ".Kindly complete the next step !";
                                string next = string.Empty;
                                DataSet dsNxtUser = new DataSet();
                                foreach (DataRow dr in dsNext.Tables[0].Rows)
                                {
                                    dsNxtUser = getdt.GetNextUser_By_DocumentUID(new Guid(DocumentUID), int.Parse(dr["ForFlow_Step"].ToString()));
                                    foreach (DataRow druser in dsNxtUser.Tables[0].Rows)
                                    {
                                        ToEmailID = getdt.GetUserEmail_By_UserUID_New(new Guid(druser["Approver"].ToString()));
                                        if (!next.Contains(ToEmailID))
                                        {
                                            getdt.StoreEmaildataToMailQueue(Guid.NewGuid(), new Guid(Session["UserUID"].ToString()), dtemailCred.Rows[0][0].ToString(), ToEmailID, Subject, sHtmlString, "", "");
                                            next += ToEmailID;
                                        }

                                    }
                                }
                                // mail to contractor
                                Subject = "Document Status Changed !" ;
                                DataSet dsMUSers = getdt.GetNextUser_By_DocumentUID(new Guid(DocumentUID), 1);
                                if (dsMUSers.Tables[0].Rows.Count > 0)
                                {
                                    foreach (DataRow druser in dsMUSers.Tables[0].Rows)
                                    {
                                        ToEmailID= getdt.GetUserEmail_By_UserUID_New(new Guid(druser["Approver"].ToString())) ;
                                    }
                                }
                                sHtmlString = "<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>" + "<html xmlns='http://www.w3.org/1999/xhtml'>" +
                                "<head>" + "<meta http-equiv='Content-Type' content='text/html; charset=utf-8' />" + "<style>table, th, td {border: 1px solid black; padding:6px;}</style></head>" +
                                   "<body style='font-family:Verdana, Arial, sans-serif; font-size:12px; font-style:normal;'>";
                                sHtmlString += "<div style='width:80%; float:left; padding:1%; border:2px solid #011496; border-radius:5px;'>" +
                                                   "<div style='float:left; width:100%; border-bottom:2px solid #011496;'>";
                                if (WebConfigurationManager.AppSettings["Domain"] == "NJSEI")
                                {
                                    sHtmlString += "<div style='float:left; width:7%;'><img src='https://dm.njsei.com/_assets/images/NJSEI%20Logo.jpg' width='50' /></div>";
                                }
                                else
                                {
                                    sHtmlString += "<div style='float:left; width:7%;'><h2>" + WebConfigurationManager.AppSettings["Domain"] + "</h2></div>";
                                }
                                sHtmlString += "<div style='float:left; width:70%;'><h2 style='margin-top:10px;'>Project Monitoring Tool</h2></div>" +
                                           "</div>";
                                sHtmlString += "<div style='width:100%; float:left;'><br/>Dear Contractor,<br/><br/><span style='font-weight:bold;'>Document " + documentname + " "  + EmailHeading +  ".</span> <br/><br/></div>";
                                sHtmlString += "<div style='width:100%; float:left;'><table style='width:100%;'>" +
                                                "<tr><td><b>Document Name </b></td><td style='text-align:center;'><b>:</b></td><td>" + documentname + "</td></tr>" +
                                                "<tr><td><b>Status </b></td><td style='text-align:center;'><b>:</b></td><td>" + status + "</td></tr>" +
                                               "<tr><td><b>Originator Ref. Number </b></td><td style='text-align:center;'><b>:</b></td><td>" + OriginatorRefNo + "</td></tr>" +
                                                    "<tr><td><b>ONTB Ref. Number</b></td><td style='text-align:center;'><b>:</b></td><td>" + ProjectRefNo + "</td></tr>" +

                                                "<tr><td><b>Date </b></td><td style='text-align:center;'><b>:</b></td><td>" + DateTime.Now.ToString("dd MMM yyyy") + "</td></tr>" +
                                                "<tr><td><b>Comments </b></td><td style='text-align:center;'><b>:</b></td><td>" + remarks + "</td></tr>";
                                sHtmlString += "</table></div>";
                                sHtmlString += "<div style='width:100%; float:left;'><br/><br/>Sincerely, <br/> MIS System.</div></div></body></html>";
                                getdt.StoreEmaildataToMailQueue(Guid.NewGuid(), new Guid(Session["UserUID"].ToString()), dtemailCred.Rows[0][0].ToString(), ToEmailID, Subject, sHtmlString, CC, "");

                                //
                                // mail to DTL and Project Co-ordinator after Accept/Reject
                                Subject = "Document Status -" + status + " under PMC Process";
                                DataSet dsMUSersPC = getdt.GetNextUser_By_DocumentUID(new Guid(DocumentUID), 3);
                                if (dsMUSers.Tables[0].Rows.Count > 0)
                                {
                                    ToEmailID = string.Empty;
                                    foreach (DataRow druser in dsMUSers.Tables[0].Rows)
                                    {
                                        ToEmailID = getdt.GetUserEmail_By_UserUID_New(new Guid(druser["Approver"].ToString())) + ",";
                                    }
                                }
                                ToEmailID += "ns.rao04@gmail.com";
                                ToEmailID = ToEmailID.TrimEnd(',');
                                sHtmlString = "<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>" + "<html xmlns='http://www.w3.org/1999/xhtml'>" +
                                "<head>" + "<meta http-equiv='Content-Type' content='text/html; charset=utf-8' />" + "<style>table, th, td {border: 1px solid black; padding:6px;}</style></head>" +
                                   "<body style='font-family:Verdana, Arial, sans-serif; font-size:12px; font-style:normal;'>";
                                sHtmlString += "<div style='width:80%; float:left; padding:1%; border:2px solid #011496; border-radius:5px;'>" +
                                                   "<div style='float:left; width:100%; border-bottom:2px solid #011496;'>";
                                if (WebConfigurationManager.AppSettings["Domain"] == "NJSEI")
                                {
                                    sHtmlString += "<div style='float:left; width:7%;'><img src='https://dm.njsei.com/_assets/images/NJSEI%20Logo.jpg' width='50' /></div>";
                                }
                                else
                                {
                                    sHtmlString += "<div style='float:left; width:7%;'><h2>" + WebConfigurationManager.AppSettings["Domain"] + "</h2></div>";
                                }
                                sHtmlString += "<div style='float:left; width:70%;'><h2 style='margin-top:10px;'>Project Monitoring Tool</h2></div>" +
                                           "</div>";
                                sHtmlString += "<div style='width:100%; float:left;'><br/>Dear User,<br/><br/><span style='font-weight:bold;'>Document " + documentname + " "  + status + " under PMC Process" + ".</span> <br/><br/></div>";
                                sHtmlString += "<div style='width:100%; float:left;'><table style='width:100%;'>" +
                                                "<tr><td><b>Document Name </b></td><td style='text-align:center;'><b>:</b></td><td>" + documentname + "</td></tr>" +
                                                "<tr><td><b>Status </b></td><td style='text-align:center;'><b>:</b></td><td>" + status + "</td></tr>" +
                                               "<tr><td><b>Originator Ref. Number </b></td><td style='text-align:center;'><b>:</b></td><td>" + OriginatorRefNo + "</td></tr>" +
                                                    "<tr><td><b>ONTB Ref. Number</b></td><td style='text-align:center;'><b>:</b></td><td>" + ProjectRefNo + "</td></tr>" +

                                                "<tr><td><b>Date </b></td><td style='text-align:center;'><b>:</b></td><td>" + DateTime.Now.ToString("dd MMM yyyy") + "</td></tr>" +
                                                "<tr><td><b>Comments </b></td><td style='text-align:center;'><b>:</b></td><td>" + remarks + "</td></tr>";
                                sHtmlString += "</table></div>";
                                sHtmlString += "<div style='width:100%; float:left;'><br/><br/>Sincerely, <br/> MIS System.</div></div></body></html>";
                                getdt.StoreEmaildataToMailQueue(Guid.NewGuid(), new Guid(Session["UserUID"].ToString()), dtemailCred.Rows[0][0].ToString(), ToEmailID, Subject, sHtmlString, CC, "");

                                //

                                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
                            }
                            else
                            {

                                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error: There is a problem with this feature. Please contact system admin');</script>");

                            }
                        }
                    }
                }

            }
            catch(Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error:Please Contact System Admin !');</script>");

            }
        }

        void BindFlow(List<string> Flow)
        {
            //string str[] = Flow.Distinct().ToList();
            //DataTable ds = getdt.GetDocumentFlow().AsEnumerable().Where(r => r.Field<string>("Flow_Name").Equals("Works A") || r.Field<string>("Flow_Name").Equals("Works B") || r.Field<string>("Flow_Name").Equals("Vendor Approval")).CopyToDataTable();
            DataTable ds = getdt.GetDocumentFlow();
            DataTable dt = new DataTable();
            dt.Columns.Add("Flow_Name", typeof(string));
            dt.Columns.Add("FlowMasterUID", typeof(string));

            if (ds != null && ds.Rows.Count > 0)
            {
                DataRow drs ;
                foreach (DataRow dr in ds.Rows)
                {
                    string result = Flow.FirstOrDefault(x => x == dr["FlowMasterUID"].ToString());
                    if (result != null)
                    {
                        drs = dt.NewRow();
                        drs["Flow_Name"] = dr["Flow_Name"]; // or dr[0]="Mohammad";
                        drs["FlowMasterUID"] = dr["FlowMasterUID"]; // or dr[1]=24;
                        dt.Rows.Add(drs);
                    }
                }
                DDLFlow.DataTextField = "Flow_Name";
                DDLFlow.DataValueField = "FlowMasterUID";
                DDLFlow.DataSource = dt;
                DDLFlow.DataBind();
                DDLFlow.Items.Insert(0, "All");
                //ViewState["Flow"] = ds;
            }
        }

        protected void btnsubmitfilter_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["type"].ToString() == "Contractor")
                FilterData1();
            else
                FilterData2();


        }

        private void FilterData1()
        {
            try
            {
                DateTime FromDate = DateTime.Now;
                DateTime ToDAte = DateTime.Now;

                if (!string.IsNullOrEmpty(dtInDate.Text))
                {
                    FromDate = Convert.ToDateTime(getdt.ConvertDateFormat(dtInDate.Text));
                }
                if (!string.IsNullOrEmpty(dtDocDate.Text))
                {
                    ToDAte = Convert.ToDateTime(getdt.ConvertDateFormat(dtDocDate.Text));

                }
                //
                //  DataSet ds = getdt.GetDashboardContractotDocsSubmitted_Details(new Guid(Request.QueryString["PrjUID"]));
                DataSet ds = new DataSet();
                if (Request.QueryString["FlowName"] != null)
                {
                    ds = getdt.GetDashboardContractotDocsSubmitted_Details_Type(new Guid(Request.QueryString["PrjUID"]), Request.QueryString["type"].ToString(), Request.QueryString["FlowName"].ToString());
                }
                else
                {
                    ds = getdt.GetDashboardContractotDocsSubmitted_Details(new Guid(Request.QueryString["PrjUID"]));
                }

                DataTable dt = new DataTable();

                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (dtInDate.Text != "" && dtDocDate.Text != "" && DDLFlow.SelectedValue == "All")
                    {
                        dt = ds.Tables[0].AsEnumerable()
         .OrderByDescending(r => r.Field<DateTime>("IncomingRec_Date")).Where(r => r.Field<DateTime>("IncomingRec_Date") >= FromDate && r.Field<DateTime>("IncomingRec_Date") <= ToDAte).CopyToDataTable();
                    }
                    else if (dtInDate.Text != "" && dtDocDate.Text != "" && DDLFlow.SelectedValue != "All")
                    {
                        dt = ds.Tables[0].AsEnumerable()
         .OrderByDescending(r => r.Field<DateTime>("IncomingRec_Date")).Where(r => r.Field<DateTime>("IncomingRec_Date") >= FromDate && r.Field<DateTime>("IncomingRec_Date") <= ToDAte).Where(r => r.Field<Guid>("FlowUID").ToString().Equals(DDLFlow.SelectedValue)).CopyToDataTable();
                    }
                    else if (dtInDate.Text != "" && dtDocDate.Text == "" && DDLFlow.SelectedValue == "All")
                    {
                        dt = ds.Tables[0].AsEnumerable()
         .OrderByDescending(r => r.Field<DateTime>("IncomingRec_Date")).Where(r => r.Field<DateTime>("IncomingRec_Date") >= FromDate).CopyToDataTable();
                    }
                    else if (dtInDate.Text != "" && dtDocDate.Text == "" && DDLFlow.SelectedValue != "All")
                    {
                        dt = ds.Tables[0].AsEnumerable()
         .OrderByDescending(r => r.Field<DateTime>("IncomingRec_Date")).Where(r => r.Field<DateTime>("IncomingRec_Date") >= FromDate).Where(r => r.Field<Guid>("FlowUID").ToString().Equals(DDLFlow.SelectedValue)).CopyToDataTable();
                    }
                    else if (dtInDate.Text == "" && dtDocDate.Text != "" && DDLFlow.SelectedValue == "All")
                    {
                        dt = ds.Tables[0].AsEnumerable()
         .OrderByDescending(r => r.Field<DateTime>("IncomingRec_Date")).Where(r => r.Field<DateTime>("IncomingRec_Date") <= ToDAte).CopyToDataTable();
                    }
                    else if (dtInDate.Text == "" && dtDocDate.Text != "" && DDLFlow.SelectedValue != "All")
                    {
                        dt = ds.Tables[0].AsEnumerable()
         .OrderByDescending(r => r.Field<DateTime>("IncomingRec_Date")).Where(r => r.Field<DateTime>("IncomingRec_Date") <= ToDAte).Where(r => r.Field<Guid>("FlowUID").ToString().Equals(DDLFlow.SelectedValue)).CopyToDataTable();
                    }
                    else if (DDLFlow.SelectedValue != "All")
                    {
                        dt = ds.Tables[0].AsEnumerable()
         .OrderByDescending(r => r.Field<DateTime>("IncomingRec_Date"))
                        .Where(r => r.Field<Guid>("FlowUID").ToString().Equals(DDLFlow.SelectedValue)).CopyToDataTable();
                    }
                    else
                    {
                        dt = ds.Tables[0].AsEnumerable()
         .OrderByDescending(r => r.Field<DateTime>("IncomingRec_Date")).CopyToDataTable();
                    }

                    if(txtOntbRefNo.Text != "")
                    {
                        dt = dt.AsEnumerable().Where(r => r.Field<string>("ProjectRef_Number").ToString().Contains(txtOntbRefNo.Text)).CopyToDataTable();
                    }

                    if (TxtSubmittal.Text != "")
                    {
                        dt = dt.AsEnumerable().Where(r => r.Field<string>("DocName").ToString().ToLower().Contains(TxtSubmittal.Text.ToLower())).CopyToDataTable();
                    }

                    if (TxtDocument.Text != "")
                    {
                        dt = dt.AsEnumerable().Where(r => r.Field<string>("ActualDocument_Name").ToString().ToLower().Contains(TxtDocument.Text.ToLower())).CopyToDataTable();
                    }

                    if (TxtOriginator.Text != "")
                    {
                        dt = dt.AsEnumerable().Where(r => r.Field<string>("Ref_Number").ToString().ToLower().Contains(TxtOriginator.Text.ToLower())).CopyToDataTable();
                    }

                    if (TxtFileRefNo.Text != "")
                    {
                        dt = dt.AsEnumerable().Where(r => r.Field<string>("FileRef_Number").ToString().ToLower().Contains(TxtFileRefNo.Text.ToLower())).CopyToDataTable();
                    }

                    GrdDocuments.DataSource = dt;

                    GrdDocuments.DataBind();

                    
                    //
                    LblDocumentHeading.Text = "Document List for Contractor -> ONTB";
                    GrdDocuments.Columns[13 + 3].Visible = false;
                    // GrdDocuments.Columns[14+3].Visible = false;
                    GrdDocuments.Columns[15 + 3].Visible = false;
                    //added on 13/06/2022 after inputs from saladin
                    GrdDocuments.Columns[7 + 3].Visible = true;
                    GrdDocuments.Columns[8 + 3].Visible = false;
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        GrdDocuments.HeaderRow.Cells[7 + 3].Text = "Contractor Uploaded date";
                    }
                }
                else
                {
                    GrdDocuments.DataSource = ds;
                    GrdDocuments.DataBind();
                    lblTotalcount.Text = "Total Count : 0";
                }

                lblTotalcount.Text = "Total Count : " + dt.Rows.Count;
            }
            catch (Exception ex)
            {
                GrdDocuments.DataSource = null;
                GrdDocuments.DataBind();
                lblTotalcount.Text = "Total Count : 0";
            }
        }

        private void FilterData2()
        {
            try
            {
                DateTime FromDate = DateTime.Now;
                DateTime ToDAte = DateTime.Now;

                if (!string.IsNullOrEmpty(dtInDate.Text))
                {
                    FromDate = Convert.ToDateTime(getdt.ConvertDateFormat(dtInDate.Text));
                }
                if (!string.IsNullOrEmpty(dtDocDate.Text))
                {
                    ToDAte = Convert.ToDateTime(getdt.ConvertDateFormat(dtDocDate.Text));

                }
                //
                DataSet ds = getdt.GetDashboardONTBtoContractorDocs_Details(new Guid(Request.QueryString["PrjUID"]));

                DataTable dt = new DataTable();

                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (dtInDate.Text != "" && dtDocDate.Text != "" && DDLFlow.SelectedValue == "All")
                    {
                        dt = ds.Tables[0].AsEnumerable()
         .OrderByDescending(r => r.Field<DateTime>("IncomingRec_Date")).Where(r => r.Field<DateTime>("IncomingRec_Date") >= FromDate && r.Field<DateTime>("IncomingRec_Date") <= ToDAte).CopyToDataTable();
                    }
                    else if (dtInDate.Text != "" && dtDocDate.Text != "" && DDLFlow.SelectedValue != "All")
                    {
                        dt = ds.Tables[0].AsEnumerable()
         .OrderByDescending(r => r.Field<DateTime>("IncomingRec_Date")).Where(r => r.Field<DateTime>("IncomingRec_Date") >= FromDate && r.Field<DateTime>("IncomingRec_Date") <= ToDAte).Where(r => r.Field<Guid>("FlowUID").ToString().Equals(DDLFlow.SelectedValue)).CopyToDataTable();
                    }
                    else if (dtInDate.Text != "" && dtDocDate.Text == "" && DDLFlow.SelectedValue == "All")
                    {
                        dt = ds.Tables[0].AsEnumerable()
         .OrderByDescending(r => r.Field<DateTime>("IncomingRec_Date")).Where(r => r.Field<DateTime>("IncomingRec_Date") >= FromDate).CopyToDataTable();
                    }
                    else if (dtInDate.Text != "" && dtDocDate.Text == "" && DDLFlow.SelectedValue != "All")
                    {
                        dt = ds.Tables[0].AsEnumerable()
         .OrderByDescending(r => r.Field<DateTime>("IncomingRec_Date")).Where(r => r.Field<DateTime>("IncomingRec_Date") >= FromDate).Where(r => r.Field<Guid>("FlowUID").ToString().Equals(DDLFlow.SelectedValue)).CopyToDataTable();
                    }
                    else if (dtInDate.Text == "" && dtDocDate.Text != "" && DDLFlow.SelectedValue == "All")
                    {
                        dt = ds.Tables[0].AsEnumerable()
         .OrderByDescending(r => r.Field<DateTime>("IncomingRec_Date")).Where(r => r.Field<DateTime>("IncomingRec_Date") <= ToDAte).CopyToDataTable();
                    }
                    else if (dtInDate.Text == "" && dtDocDate.Text != "" && DDLFlow.SelectedValue != "All")
                    {
                        dt = ds.Tables[0].AsEnumerable()
         .OrderByDescending(r => r.Field<DateTime>("IncomingRec_Date")).Where(r => r.Field<DateTime>("IncomingRec_Date") <= ToDAte).Where(r => r.Field<Guid>("FlowUID").ToString().Equals(DDLFlow.SelectedValue)).CopyToDataTable();
                    }
                    else if (DDLFlow.SelectedValue != "All")
                    {
                        dt = ds.Tables[0].AsEnumerable()
         .OrderByDescending(r => r.Field<DateTime>("IncomingRec_Date")).Where(r => r.Field<Guid>("FlowUID").ToString().Equals(DDLFlow.SelectedValue)).CopyToDataTable();
                    }
                    else
                    {
                        dt = ds.Tables[0].AsEnumerable()
         .OrderByDescending(r => r.Field<DateTime>("IncomingRec_Date")).CopyToDataTable();
                    }

                    if (txtOntbRefNo.Text != "")
                    {
                        dt = dt.AsEnumerable().Where(r => r.Field<string>("ProjectRef_Number").ToString().Contains(txtOntbRefNo.Text)).CopyToDataTable();
                    }

                    if (TxtSubmittal.Text != "")
                    {
                        dt = dt.AsEnumerable().Where(r => r.Field<string>("DocName").ToString().ToLower().Contains(TxtSubmittal.Text.ToLower())).CopyToDataTable();
                    }

                    if (TxtDocument.Text != "")
                    {
                        dt = dt.AsEnumerable().Where(r => r.Field<string>("ActualDocument_Name").ToString().ToLower().Contains(TxtDocument.Text.ToLower())).CopyToDataTable();
                    }

                    if (TxtOriginator.Text != "")
                    {
                        dt = dt.AsEnumerable().Where(r => r.Field<string>("Ref_Number").ToString().ToLower().Contains(TxtOriginator.Text.ToLower())).CopyToDataTable();
                    }

                    if (TxtFileRefNo.Text != "")
                    {
                        dt = dt.AsEnumerable().Where(r => r.Field<string>("FileRef_Number").ToString().ToLower().Contains(TxtFileRefNo.Text.ToLower())).CopyToDataTable();
                    }

                    GrdDocuments.DataSource = dt;

                    GrdDocuments.DataBind();
                    //
                    LblDocumentHeading.Text = "Document List for ONTB -> Contractor";
                    GrdDocuments.Columns[13 + 3].Visible = false;
                    // GrdDocuments.Columns[14+3].Visible = false;
                    GrdDocuments.Columns[15 + 3].Visible = false;
                    //added on 13/06/2022 after inputs from saladin
                    GrdDocuments.Columns[7 + 3].Visible = true;
                    GrdDocuments.Columns[8 + 3].Visible = false;
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        GrdDocuments.HeaderRow.Cells[7 + 3].Text = "Contractor Uploaded date";
                    }
                }
                else
                {
                    GrdDocuments.DataSource = ds;
                    GrdDocuments.DataBind();
                    lblTotalcount.Text = "Total Count : 0";
                }

                lblTotalcount.Text = "Total Count : " + dt.Rows.Count;
            }
            catch (Exception ex)
            {
                GrdDocuments.DataSource = null;
                GrdDocuments.DataBind();
                lblTotalcount.Text = "Total Count : 0";
            }
        }

        private void Reconsilation()
        {
            DataSet ds = getdt.GetDashboardReconciliationDocs_Details(new Guid(Request.QueryString["PrjUID"]));
            lblTotalcount.Text = "Total Count : " + ds.Tables[0].Rows.Count;
            if (ds.Tables[0].Rows.Count > 0)
            {
                GrdDocuments.DataSource = ds.Tables[0].AsEnumerable()
     .OrderByDescending(r => r.Field<DateTime>("IncomingRec_Date"))
     .CopyToDataTable();
                GrdDocuments.DataBind();
            }
            else
            {
                GrdDocuments.DataSource = ds;
                GrdDocuments.DataBind();
            }
            LblDocumentHeading.Text = "Document List for Reconciliation Docs";

            //added on 09/06/2022 after inputs from saladin
            //added on 09/06/2022 after inputs from saladin
            GrdDocuments.Columns[7 + 3].Visible = true;
            GrdDocuments.Columns[8 + 3].Visible = false;
            GrdDocuments.Columns[19].Visible = false;
            GrdDocuments.Columns[20].Visible = false;
            if (ds.Tables[0].Rows.Count > 0)
            {
                GrdDocuments.HeaderRow.Cells[7 + 3].Text = "Contractor Uploaded date";
            }
            //
            if (Session["IsContractor"].ToString() == "Y")
            {
                GrdDocuments.Columns[13 + 3].Visible = false;
                GrdDocuments.Columns[14 + 3].Visible = false;
                GrdDocuments.Columns[15 + 3].Visible = false;

            }
            else
            {
                GrdDocuments.Columns[13 + 3].Visible = true;
                GrdDocuments.Columns[14 + 3].Visible = true;
                GrdDocuments.Columns[15 + 3].Visible = true;
                btnSubmit.Visible = true;
            }
            if (GrdDocuments.Rows.Count > 14)
            {
                //  ScriptManager.RegisterStartupScript(Page, this.GetType(), "Key", "<script>MakeStaticHeader('" + GrdDocuments.ClientID + "', 700, 1300 , 45 ,true); </script>", false);
            }
        }

        private void no_user()
        {
            string str = HttpContext.Current.Session["docuids"].ToString();
            if (str.Contains(","))
            {
                str = str.Substring(0, str.Length - 1);
            }
            DataSet ds = getdt.GetNextUserDocuments_pending(new Guid(Request.QueryString["PrjUID"]), new Guid(Request.QueryString["WkpgUID"]), str);
            if (ds != null)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {

                    GrdDocuments.DataSource = getdt.GetNextUserDocuments_pending(new Guid(Request.QueryString["PrjUID"]), new Guid(Request.QueryString["WkpgUID"]), str).Tables[0].AsEnumerable()
             .OrderByDescending(r => r.Field<DateTime>("IncomingRec_Date"))
             .CopyToDataTable();
                    GrdDocuments.DataBind();
                }
                else
                {
                    GrdDocuments.DataSource = getdt.GetNextUserDocuments_pending(new Guid(Request.QueryString["PrjUID"]), new Guid(Request.QueryString["WkpgUID"]), str).Tables[0].AsEnumerable()
             .OrderByDescending(r => r.Field<DateTime>("IncomingRec_Date"))
             .CopyToDataTable();
                    GrdDocuments.DataBind();
                }
            }
            LblDocumentHeading.Text = "Document List";
            lblTotalcount.Visible = false;
            GrdDocuments.Columns[0].Visible = false;
            GrdDocuments.Columns[1].Visible = false;
            GrdDocuments.Columns[13 + 3].Visible = false;
            GrdDocuments.Columns[14 + 3].Visible = false;
            GrdDocuments.Columns[15 + 3].Visible = false;
            //added on 13/06/2022 after inputs from saladin
            GrdDocuments.Columns[7 + 3].Visible = true;
            GrdDocuments.Columns[8 + 3].Visible = false;
            GrdDocuments.Columns[19].Visible = false;
            GrdDocuments.Columns[20].Visible = false;
            if (ds != null)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    GrdDocuments.HeaderRow.Cells[7 + 3].Text = "Uploaded date";
                }
            }
            //
            //  lblTotalcount.Text = "Total Count : " + GrdDocuments.Rows.Count.ToString();
            //if (GrdDocuments.Rows.Count > 15)
            //{
            //    ScriptManager.RegisterStartupScript(Page, this.GetType(), "Key", "<script>MakeStaticHeader('" + GrdDocuments.ClientID + "', 700, 1300 , 45 ,true); </script>", false);
            //}
        }

        protected void GrdDocuments_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdDocuments.PageIndex = e.NewPageIndex;

            if (Request.QueryString["UserUID"] != null)
            {
                no_user();
            }
            else if (Request.QueryString["PrjUID"] != null)
            {

                if (Request.QueryString["type"].ToString() == "Contractor" || Request.QueryString["FlowName"] != null)
                { 
                    FilterData1();
                }
                else if (Request.QueryString["type"].ToString() == "Recon")
                {
                    Reconsilation();
                }
                else
                {
                    FilterData2();
                }
                   
            }
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            //required to avoid the runtime error "  
            //Control 'GridView1' of type 'GridView' must be placed inside a form tag with runat=server."  
        }

        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            try
            {
                StringBuilder StrExport = new StringBuilder();

                StringWriter stw = new StringWriter();
                HtmlTextWriter htextw = new HtmlTextWriter(stw);
                htextw.AddStyleAttribute("font-size", "11pt");
                htextw.AddStyleAttribute("color", "Black");


                GrdDocuments.AllowPaging = false;
               
                getPendingDocs();
                GrdDocuments.Columns[2].Visible = false;
                GrdDocuments.Columns[13].Visible = false;
                GrdDocuments.Columns[14].Visible = false;
                GrdDocuments.Columns[15].Visible = false;
                GrdDocuments.Columns[21].Visible = false;
                GrdDocuments.Columns[22].Visible = false;
                GrdDocuments.RenderControl(htextw); //Name of the Panel
                GrdDocuments.AllowPaging = true;
                GrdDocuments.Columns[2].Visible = true;
                GrdDocuments.Columns[13].Visible = true;
                GrdDocuments.Columns[14].Visible = true;
                GrdDocuments.Columns[15].Visible = true;
                GrdDocuments.Columns[21].Visible = true;
                GrdDocuments.Columns[22].Visible = true;
                //var sb1 = new StringBuilder();
                //GridDiv.RenderControl(new HtmlTextWriter(new StringWriter(sb1)));

                string s = htextw.InnerWriter.ToString();

                string HTMLstring = "<html><body>" +
                    "<div style='width:100%; margin:auto;'><div style='width:100%; float:left; line-height:25px; font-size:12pt;' align='center'>" +
                    "<asp:Label ID='Lbl1' runat='server' Font-Bold='true'>" + WebConfigurationManager.AppSettings["Domain"] + " Report Name: Pending Documents for " + Session["Username"].ToString() + " </asp:Label><br />" +
                    "<asp:Label ID='Lbl4' runat='server'>Project Name :" + getdt.getProjectNameby_ProjectUID(new Guid(Request.QueryString["PrjUID"])) + "</asp:Label><br />" +
                    "</div> <div style='width:100%; float:left; height:10px;'>&nbsp;&nbsp;&nbsp;</div>" +
                    "<div style='width:100%;font-size:11pt float:left;' align='left'>" +
                    s +
                    "</div>" +
                    "</div></body></html>";

                string strFile = "Report__PendingDocs_" + DateTime.Now.Ticks + ".xls";
                string strcontentType = "application/excel";
                Response.ClearContent();
                Response.ClearHeaders();
                Response.BufferOutput = true;
                Response.ContentType = strcontentType;
                Response.AddHeader("Content-Disposition", "attachment; filename=" + strFile);
                Response.Write(HTMLstring);
                Response.Flush();
                Response.Close();
                Response.End();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

       
        private void getPendingDocs()
        {
            string str = HttpContext.Current.Session["docuids"].ToString();
            if (str.Contains(","))
            {
                str = str.Substring(0, str.Length - 1);
            }
            DataSet ds = getdt.GetNextUserDocuments_pending(new Guid(Request.QueryString["PrjUID"]), new Guid(Request.QueryString["WkpgUID"]), str);
            if (ds != null)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {

                    GrdDocuments.DataSource = getdt.GetNextUserDocuments_pending(new Guid(Request.QueryString["PrjUID"]), new Guid(Request.QueryString["WkpgUID"]), str).Tables[0].AsEnumerable()
             .OrderByDescending(r => r.Field<DateTime>("IncomingRec_Date"))
             .CopyToDataTable();
                    GrdDocuments.DataBind();
                }
                else
                {
                    GrdDocuments.DataSource = getdt.GetNextUserDocuments_pending(new Guid(Request.QueryString["PrjUID"]), new Guid(Request.QueryString["WkpgUID"]), str).Tables[0].AsEnumerable()
             .OrderByDescending(r => r.Field<DateTime>("IncomingRec_Date"))
             .CopyToDataTable();
                    GrdDocuments.DataBind();
                }
            }
            LblDocumentHeading.Text = "Document List";
            lblTotalcount.Visible = false;
            GrdDocuments.Columns[0].Visible = false;
            GrdDocuments.Columns[1].Visible = false;
            GrdDocuments.Columns[13 + 3].Visible = false;
            GrdDocuments.Columns[14 + 3].Visible = false;
            GrdDocuments.Columns[15 + 3].Visible = false;
            //added on 13/06/2022 after inputs from saladin
            GrdDocuments.Columns[7 + 3].Visible = true;
            GrdDocuments.Columns[8 + 3].Visible = false;
            GrdDocuments.Columns[19].Visible = false;
            GrdDocuments.Columns[20].Visible = false;
            if (ds != null)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    GrdDocuments.HeaderRow.Cells[7 + 3].Text = "Uploaded date";
                }
            }
            btnExportExcel.Visible = true;
        }
    }
}