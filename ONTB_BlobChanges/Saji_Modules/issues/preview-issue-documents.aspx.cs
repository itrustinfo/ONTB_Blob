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
using System.Text.RegularExpressions;
using System.Web.Configuration;

namespace ProjectManagementTool._modal_pages
{
    public partial class preview_issue_documents : System.Web.UI.Page
    {
        DBGetData getdata = new DBGetData();
        static int img_count = 0;
        static List<string> image_list = new List<string>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Username"] == null)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
            }
            else
            {
                Session["issue_status_preview"] = 1;
                if (!Page.IsPostBack)
                {
                    if (Request.QueryString["IssueUID"] != null)
                    {
                        DataSet ds = getdata.GetUploadedIssueImages(Request.QueryString["IssueUID"]);

                        image_list.Clear();
                        img_count = 0;

                        if (ds.Tables[0].Rows.Count > 0)
                        {
                           
                            string image_source = "";
                            
                            foreach(DataRow dr in ds.Tables[0].Rows)
                            {

                                string Extension = Path.GetExtension(dr.ItemArray[1].ToString());

                                if (Extension == ".jpg" || Extension == ".png" || Extension == ".jpeg" || Extension == ".bmp")
                                {
                                    img_count = img_count + 1;

                                    string filename = "";
                                    byte[] bytes = null;

                                    bytes = getdata.DownloadIssueDocument(Convert.ToInt32(dr.ItemArray[0].ToString()), out filename);

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
                                    getdata.DecryptFile(filepath, outPath);

                                    image_source = "~/_PreviewLoad/" + Path.GetFileName(outPath);

                                    if (img_count == 1)
                                        image.Src = image_source;

                                    image_list.Add(image_source);
                                }
                            }

                            img_count = 1;

                            
                        }

                        img_count = 0;

                        if (image_list.Count == 1)
                        {
                            btnNext.Visible = false;
                            btnPrv.Visible = false;
                        }
                        else
                        {
                            btnNext.Visible = true;
                            btnPrv.Visible = true;
                        }
                    }

                }
            }
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {

            //if (image_list.Count ==1)
                
            img_count = img_count + 1;

            if (img_count < image_list.Count )
            {
                image.Src = image_list[img_count];
            }
            else
            {
                img_count = 0;
                image.Src = image_list[img_count];
            }
               
        }
        protected void btnPrevious_Click(object sender, EventArgs e)
        {
            img_count = img_count - 1;

            if (img_count > -1)
            {
                image.Src = image_list[img_count];
            }
            else
            {
                img_count = image_list.Count-1;
                image.Src = image_list[img_count];
            }

        }
    }
}