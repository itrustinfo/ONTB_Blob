using ProjectManagementTool.Models;
using ProjectManager.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManagementTool._modal_pages
{
    public partial class upload_sitephotograph : System.Web.UI.Page
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
                var username = Session["Username"];

                if (Request.QueryString["PrjUID"] != null && Request.QueryString["WorkPackage"] != null)
                {
                    if (!IsPostBack)
                    {
                        BindSitePhotographs(null);
                    }
                }
            }
        }
        private void BindSitePhotographs(List<UploadSitePhoto> lstUploadNewPhoto)
        {
            DataSet ds = new DataSet();
            if (ViewState["sitephoto"] == null)
            {
                //DataSet ds = getdata.GetSitePhotograph_by_WorkpackageUID_Date(new Guid(Request.QueryString["WorkPackage"]), DateTime.Now);
                ds = getdata.GetSitePhotographs_by_WorkpackageUID(new Guid(Request.QueryString["WorkPackage"]));
                
            }
            else
            {
                ds = (DataSet)ViewState["sitephoto"];
            }

            if (lstUploadNewPhoto != null && lstUploadNewPhoto.Count > 0)
            {
                foreach (var eachNewPhoto in lstUploadNewPhoto)
                {
                    DataTable dt = ds.Tables[0];

                    DataRow dr;
                    dr = dt.NewRow();
                    dr[0] = new Guid();
                    dr[1] = eachNewPhoto.PrjUID;
                    dr[2] = eachNewPhoto.WorkPackage;
                    dr[3] = eachNewPhoto.Image;
                    dr[4] = eachNewPhoto.Description;

                    dt.Rows.InsertAt(dr, 0);
                }
            }

           GrdSitePhotograph.DataSource = ds;
           GrdSitePhotograph.DataBind();

            ViewState["sitephoto"] = ds;
        }

        protected void Item_Bound(Object sender, DataListItemEventArgs e)
        {

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {

                Label l = (Label)e.Item.FindControl("Label1");
                string img_uid = l.Text;
                string file_name = "";

                byte[] img_blob = getdata.DownloadSitePhotographByUID(img_uid,out file_name);
                Image  photo = (Image)e.Item.FindControl("imgEmp");
              
                String st = Server.MapPath(file_name); 
                FileStream fs = new FileStream(st, FileMode.Create, FileAccess.Write);
                fs.Write(img_blob, 0, img_blob.Length);
                fs.Close();
                photo.ImageUrl = file_name;

            }

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                string sFileDirectory = "~/SitePhotographs";

                if (!Directory.Exists(Server.MapPath(sFileDirectory)))
                {
                    Directory.CreateDirectory(Server.MapPath(sFileDirectory));
                }

                int NotSupportedImageCount = 0;
                List<UploadSitePhoto> lstUploadNewPhoto = new List<UploadSitePhoto>();
                foreach (HttpPostedFile uploadedFile in ImageUpload.PostedFiles)
                {
                    if (uploadedFile.ContentLength > 0 && !String.IsNullOrEmpty(uploadedFile.FileName))
                    {
                        string fileNameWIthoutExtentsion = Path.GetFileNameWithoutExtension(uploadedFile.FileName) + "-" + DateTime.Now.ToString("yyMMddhhmmss");
                        string sFileName = Path.GetFileName(uploadedFile.FileName);
                        string FileExtn = Path.GetExtension(uploadedFile.FileName);
                        if (FileExtn.ToUpper() == ".JPG" || FileExtn.ToUpper() == ".JPEG" || FileExtn.ToUpper() == ".PNG" || FileExtn.ToUpper() == ".GIF" || FileExtn.ToUpper() == ".TIFF")
                        {
                            sFileName = fileNameWIthoutExtentsion + "." + FileExtn;
                            uploadedFile.SaveAs(Server.MapPath(sFileDirectory + "/" + sFileName));

                            lstUploadNewPhoto.Add(new UploadSitePhoto
                            {
                                PrjUID = new Guid(Request.QueryString["PrjUID"]),
                                WorkPackage = new Guid(Request.QueryString["WorkPackage"]),
                                Image = (sFileDirectory + "/" + sFileName),
                                Description = string.Empty
                            });

                            Guid new_id = Guid.NewGuid();

                            int Cnt = getdata.SitePhotograph_InsertorUpdate(new_id, new Guid(Request.QueryString["PrjUID"]), new Guid(Request.QueryString["WorkPackage"]), (sFileDirectory + "/" + sFileName), "", DateTime.Now, Session["UserUID"].ToString());
                            
                            if (Cnt>0)
                            {
                                    byte[] filetobytes = null;

                                    //string fileName = Path.GetFileName(uploadedFile.FileName);
                                    //uploadedFile.SaveAs(Server.MapPath("~/Documents/") + fileName);

                                    //sFileName = Path.GetFileNameWithoutExtension(uploadedFile.FileName);
                                    //string Extn = Path.GetExtension(uploadedFile.FileName);

                                    string savedPath = sFileDirectory + "/" + sFileName;

                                   // string DocPath = "~/Documents/" + sFileName + "_DE" + Extn;

                                  //  getdata.EncryptFile(Server.MapPath(savedPath), Server.MapPath(DocPath));

                                    filetobytes = getdata.FileToByteArray(Server.MapPath(savedPath));

                                    Guid new_guid = Guid.NewGuid();
                                    getdata.InsertUploadedSitePhotographBlob(new_guid, new_id.ToString(), filetobytes, sFileName, savedPath);
                            }

                            if (Cnt <= 0)
                            {
                                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code ADDSP-01 there is a problem with this feature. Please contact system admin.');</script>");
                            }
                        }
                        else
                        {
                            NotSupportedImageCount += 1;
                        }
                    }
                }

                ViewState["sitephoto"] = null;
                BindSitePhotographs(null);

                if (NotSupportedImageCount > 0)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Some of the uploded image formats are not supported. Please contact system admin.');</script>");
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code : ADD_SP-02 There is a problem with these feature. Please contact system admin.');</script>");
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < GrdSitePhotograph.Items.Count; i++)
                {
                    TextBox txtdesc = (TextBox)GrdSitePhotograph.Items[i].FindControl("txtdesc");
                    Label SitePhotoGraph_UID = (Label)GrdSitePhotograph.Items[i].FindControl("LblSitePhotoGraph_UID");

                    if (SitePhotoGraph_UID.Text == "00000000-0000-0000-0000-000000000000")
                    {
                        var imageUrl = ((Image)GrdSitePhotograph.Items[i].FindControl("imgEmp")).ImageUrl;

                        int Cnt = getdata.SitePhotograph_InsertorUpdate(Guid.NewGuid(), new Guid(Request.QueryString["PrjUID"]), new Guid(Request.QueryString["WorkPackage"]), imageUrl, txtdesc.Text, DateTime.Now, Session["UserUID"].ToString());
                        if (Cnt <= 0)
                        {
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code ADDSP-01 there is a problem with this feature. Please contact system admin.');</script>");
                        }
                    }
                    else
                    {
                        int cnt = getdata.SitePhotograph_Desc_Update(new Guid(SitePhotoGraph_UID.Text), txtdesc.Text);
                        if (cnt <= 0)
                        {
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code UpdateDesc-01 there is a problem with this feature. Please contact system admin.');</script>");
                        }
                    }
                }
                Session["SelectedActivity"] = Request.QueryString["WorkPackage"].ToString();
                ViewState["sitephoto"] = null;
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code : UploadImage-01 There is a problem with these feature. Please contact system admin.');</script>");
            }
            
        }
    }
}