using ProjectManager.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ionic.Zip;
using ProjectManagementTool.Models;

namespace ProjectManagementTool._content_pages.issues
{
    public partial class _default : System.Web.UI.Page
    {
        DBGetData getdt = new DBGetData();
        TaskUpdate gettk = new TaskUpdate();
        DataSet ds = new DataSet();
        static string status = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Username"] == null)
            {
                Response.Redirect("~/Login.aspx");
            }
            else
            {
                //ScriptManager.RegisterStartupScript(
                //       UpdatePanel2,
                //       this.GetType(),
                //       "MyAction",
                //       "BindEvents();",
                //       true);
            
                //
                if (!IsPostBack)
                {
                    HideActionButtons();
                    BindProject();
                    SelectedProject();
                    status = Request.QueryString["status"];
                    DDlProject_SelectedIndexChanged(sender, e);
                    Session["ProjectUID"] = DDlProject.SelectedValue;
                    Session["ProjectName"] = DDlProject.SelectedItem.Text;
                    
                   
                    ColorSelected(status);

                   // BindIssues(Session["selected_item"].ToString(), TreeView1.SelectedNode.Value, "All");
                }
                else
                {
                   

                    if (Session["ProjectName"].ToString() != DDlProject.SelectedItem.Text)
                    {
                        status = "";
                        TreeView1.Nodes[0].Selected = true;
                        Session["selected_item"] = "WorkPackage";
                        Session["ProjectName"] = DDlProject.SelectedItem.Text;

                    }
                                     
                    BindIssues(Session["selected_item"].ToString(), TreeView1.SelectedNode.Value,DDLUser.SelectedValue);
                }

                
            }
        }


        private void ColorSelected(string status)
        {
            if (string.IsNullOrEmpty(status))
            {
                divTotal.Attributes.Add("style", "background-color:yellow !important");
                divOpen.Attributes.Add("style", "background-color:white !important");
                divProgress.Attributes.Add("style", "background-color:white !important");
                divClosed.Attributes.Add("style", "background-color:white !important");
                divRejected.Attributes.Add("style", "background-color:white !important");
            }
            else if (status == "Open")
            {
                divTotal.Attributes.Add("style", "background-color:white !important");
                divOpen.Attributes.Add("style", "background-color:yellow !important");
                divProgress.Attributes.Add("style", "background-color:white !important");
                divClosed.Attributes.Add("style", "background-color:white !important");
                divRejected.Attributes.Add("style", "background-color:white !important");
            }
            else if (status == "In-Progress")
            {
                divTotal.Attributes.Add("style", "background-color:white !important");
                divOpen.Attributes.Add("style", "background-color:white !important");
                divProgress.Attributes.Add("style", "background-color:yellow !important");
                divClosed.Attributes.Add("style", "background-color:white !important");
                divRejected.Attributes.Add("style", "background-color:white !important");
            }
            else if (status == "Close")
            {
                divTotal.Attributes.Add("style", "background-color:white !important");
                divOpen.Attributes.Add("style", "background-color:white !important");
                divProgress.Attributes.Add("style", "background-color:white !important");
                divClosed.Attributes.Add("style", "background-color:yellow !important");
                divRejected.Attributes.Add("style", "background-color:white !important");
            }
            else if (status == "Rejected")
            {
                divTotal.Attributes.Add("style", "background-color:white !important");
                divOpen.Attributes.Add("style", "background-color:white !important");
                divProgress.Attributes.Add("style", "background-color:white !important");
                divClosed.Attributes.Add("style", "background-color:white !important");
                divRejected.Attributes.Add("style", "background-color:yellow !important");
            }
        }


        private void BindUsers()
        {
            DataSet ds = getdt.getAllIssuedUsers(new Guid(TreeView1.Nodes[0].Value),status);

            DDLUser.Items.Clear();

            if (ds.Tables[0].Rows.Count > 0)
            {
                DDLUser.DataTextField = "userName";
                DDLUser.DataValueField = "userId";
                DDLUser.DataSource = ds;
                DDLUser.DataBind();
            }

            DDLUser.Items.Insert(0, new ListItem("All","All"));
          //  DDLUser.SelectedIndex = 0;

        }

            internal void HideActionButtons()
        {
            AddIssues.Visible = false;
            ViewState["isEdit"] = "false";
            ViewState["isAssignUser"] = "false";
            ViewState["isDelete"] = "false";
            GrdIssues.Columns[10].Visible = false;
            GrdIssues.Columns[11].Visible = false;
            GrdIssues.Columns[12].Visible = false;
            DataSet dscheck = new DataSet();
            dscheck = getdt.GetUsertypeFunctionality_Mapping(Session["TypeOfUser"].ToString());
            if (dscheck.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dscheck.Tables[0].Rows)
                {
                    if (dr["Code"].ToString() == "IA" || Session["IsClient"].ToString() == "Y")
                    {
                        AddIssues.Visible = true;
                    }

                    if (dr["Code"].ToString() == "IE" || Session["IsClient"].ToString() == "Y")
                    {
                        GrdIssues.Columns[10].Visible = true;
                        ViewState["isEdit"] = "true";
                    }
                    if (dr["Code"].ToString() == "IAU" || dr["Code"].ToString() == "ID" || Session["IsClient"].ToString() == "Y")
                    {
                        GrdIssues.Columns[11].Visible = true;
                        ViewState["isAssignUser"] = "true";
                    }
                    if (dr["Code"].ToString() == "ID" || Session["IsClient"].ToString() == "Y")
                    {
                        GrdIssues.Columns[12].Visible = true;
                        ViewState["isDelete"] = "true";
                    }
                    
                }
            }
        }

        private void BindProject()
        {
            DataSet ds = new DataSet();
            if (Session["TypeOfUser"].ToString() == "U" || Session["TypeOfUser"].ToString() =="MD" || Session["TypeOfUser"].ToString() == "VP" || Session["TypeOfUser"].ToString() == "NJSD")
            {
                ds = gettk.GetAllProjects();
            }
            else if (Session["TypeOfUser"].ToString() == "PA")
            {
                ds = getdt.GetAssignedProjects_by_UserUID(new Guid(Session["UserUID"].ToString()));
            }
            else
            {
                ds = getdt.GetAssignedProjects_by_UserUID(new Guid(Session["UserUID"].ToString()));
            }
            if (ds.Tables[0].Rows.Count > 0)
            {
                DDlProject.DataTextField = "ProjectName";
                DDlProject.DataValueField = "ProjectUID";
                DDlProject.DataSource = ds;
                DDlProject.DataBind();
            }
        }

        public void PopulateTreeView(DataSet dtParent, TreeNode treeNode, string ParentUID, int Level)
        {
            foreach (DataRow row in dtParent.Tables[0].Rows)
            {
                TreeNode child = new TreeNode
                {
                    Text = LimitCharts(row["Name"].ToString()),
                    Value = Level == 0 ? row["WorkPackageUID"].ToString() : row["TaskUID"].ToString(),
                    Target = Level == 0 ? "WorkPackage" : "Tasks",
                    ToolTip = row["Name"].ToString()
                };

                //if (ParentUID == "")
                //{
                //    TreeView1.Nodes.Add(child);
                //    DataSet dsworkPackage = getdt.GetWorkPackages_By_ProjectUID(new Guid(child.Value));
                //    if (dsworkPackage.Tables[0].Rows.Count > 0)
                //    {
                //        PopulateTreeView(dsworkPackage, child, child.Value, 1);
                //    }
                //}
                if (ParentUID == "")
                {
                    //treeNode.ChildNodes.Add(child);
                    TreeView1.Nodes.Add(child);
                    DataSet dschild = getdt.GetTasksForWorkPackages(child.Value);
                    //DataTable dtChild = TreeViewBAL.BL.TreeViewBL.GetData("Select ID,Name from Module where ProjID=" + child.Value);
                    if (dschild.Tables[0].Rows.Count > 0)
                    {
                        PopulateTreeView(dschild, child, child.Value, 1);
                    }

                }
                else if (Level == 1)
                {
                    treeNode.ChildNodes.Add(child);
                    DataSet dssubchild = getdt.GetSubTasksForWorkPackages(child.Value);
                    if (dssubchild.Tables[0].Rows.Count > 0)
                    {
                        PopulateTreeView(dssubchild, child, child.Value, 2);
                    }
                }
                else if (Level == 2)
                {
                    treeNode.ChildNodes.Add(child);
                    DataSet dssubtosubchild = getdt.GetSubtoSubTasksForWorkPackages(child.Value);
                    if (dssubtosubchild.Tables[0].Rows.Count > 0)
                    {
                        PopulateTreeView(dssubtosubchild, child, child.Value, 3);
                    }
                }
                //else if (Level == 3)
                //{
                //    treeNode.ChildNodes.Add(child);
                //    DataSet lastchild = getdt.GetSubtoSubtoSubTasksForWorkPackages(child.Value);
                //    if (lastchild.Tables[0].Rows.Count > 0)
                //    {
                //        PopulateTreeView(lastchild, child, child.Value, 4);
                //    }
                //}
                //else if (Level == 4)
                //{
                //    treeNode.ChildNodes.Add(child);
                //    DataSet lastchild = getdt.GetSubtoSubtoSubtoSubTasksForWorkPackages(child.Value);
                //    if (lastchild.Tables[0].Rows.Count > 0)
                //    {
                //        PopulateTreeView(lastchild, child, child.Value, 5);
                //    }
                //}
                //else
                //{
                //    treeNode.ChildNodes.Add(child);
                //    DataSet ds = getdt.GetTask_by_ParentTaskUID(new Guid(child.Value));
                //    if (ds.Tables[0].Rows.Count > 0)
                //    {
                //        PopulateTreeView(ds, child, child.Value, 6);
                //    }
                //}
            }

        }

        public string LimitCharts(string Desc)
        {
            if (Desc.Length > 100)
            {
                return Desc.Substring(0, 100) + "  . . .";
            }
            else
            {
                return Desc;
            }
        }

        public string ShoworHide(string Desc)
        {
            if (Desc.Length > 45)
            {
                return "More";
            }
            else
            {
                return string.Empty;
            }
        }
        protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
        {
            BindActivities();
        }

        protected void DDLUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindIssues(Session["selected_item"].ToString(), TreeView1.SelectedNode.Value,DDLUser.SelectedValue);
            ColorSelected(status);
        }

            protected void DDlProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDlProject.SelectedValue != "")
            {
                //AddIssues.Visible = true;
                //DataSet ds = getdt.GetWorkPackages_By_ProjectUID(new Guid(DDlProject.SelectedValue));
                DataSet ds = new DataSet();
                if (Session["TypeOfUser"].ToString() == "U" || Session["TypeOfUser"].ToString() == "MD" || Session["TypeOfUser"].ToString() == "VP" || Session["TypeOfUser"].ToString() == "NJSD")
                {
                    ds = getdt.GetWorkPackages_By_ProjectUID(new Guid(DDlProject.SelectedValue));
                }
                else if (Session["TypeOfUser"].ToString() == "PA")
                {
                    ds = getdt.GetWorkPackages_ForUser_by_ProjectUID(new Guid(Session["UserUID"].ToString()), new Guid(DDlProject.SelectedValue));
                }
                else
                {
                    ds = getdt.GetWorkPackages_ForUser_by_ProjectUID(new Guid(Session["UserUID"].ToString()), new Guid(DDlProject.SelectedValue));
                }
                if (ds.Tables[0].Rows.Count > 0)
                {
                    //DDLWorkPackage.DataTextField = "Name";
                    //DDLWorkPackage.DataValueField = "WorkPackageUID";
                    //DDLWorkPackage.DataSource = ds;
                    //DDLWorkPackage.DataBind();
                    TreeView1.Nodes.Clear();
                    PopulateTreeView(ds, null, "", 0);
                    TreeView1.CollapseAll();
                    TreeView1.Nodes[0].Expand();

                    if (Session["selected_item"] == null)
                        TreeView1.Nodes[0].Selected = true;
                    else
                    {
                        if (Session["selected_item"].ToString() == "WorkPackage")
                        {
                            TreeView1.Nodes[0].Selected = true;
                        }
                        else
                        {
                            
                            TreeviewTraverseAndFind(Session["ActivityName"].ToString(), TreeView1.Nodes[0]);
                        }
                    }
 
                  //  if (Session["ActivityName"] != null)
                   //     TreeviewTraverseAndFind(Session["ActivityName"].ToString(), TreeView1.Nodes[0]);
                  //  else
                   
                    //BindActivities();
                    TreeView1_SelectedNodeChanged(sender, e);
                    GrdIssues.Visible = true;
                    Session["WorkPackageUID"] = ds.Tables[0].Rows[0].ItemArray[0].ToString();
                }
                else
                {
                    GrdIssues.Visible = false;
                }

                Session["Project_Workpackage"] = DDlProject.SelectedValue;
               
                BindUsers();
              
                ColorSelected(status);

                if (DDLUser.Items.Count > 0) DDLUser.SelectedIndex = 0;

                
            }
            else
            {
                printreport.Visible = false;
                AddIssues.Visible = false;
            }
            
        }

        internal void SelectedProject()
        {
            if (!IsPostBack && Session["Project_Workpackage"] != null)
            {
                string[] selectedValue = Session["Project_Workpackage"].ToString().Split('_');
                if (selectedValue.Length > 1)
                {
                    DDlProject.SelectedValue = selectedValue[0];
                }
                else
                {
                    DDlProject.SelectedValue = Session["Project_Workpackage"].ToString();
                }
            }
        }
        public void BindActivities()
        {
            //if (TreeView1.SelectedNode == null)
            //    TreeView1.Nodes[0].Selected = true;

            //Session["selected_item"] = TreeView1.SelectedNode.Target;
            //Session["ActivityName"] = TreeView1.SelectedNode.Text;

            //if (string.IsNullOrEmpty(status))
            //    ActivityHeading.Text = TreeView1.SelectedNode.Text + " [Total]";
            //else
            //    ActivityHeading.Text = TreeView1.SelectedNode.Text + " [" + status + "]";


            //if (Session["selected_item"].ToString() == "WorkPackage")
            //{
            //    AddIssues.HRef = "/_modal_pages/add-issues.aspx?IssueFor=WorkPackage&ActivityID=" + TreeView1.SelectedNode.Value + "&AName=" + TreeView1.SelectedNode.Text.Replace('&',' ') + "&PrjID=" + DDlProject.SelectedValue;
            //}
            //else
            //{
            //    AddIssues.HRef = "/_modal_pages/add-issues.aspx?IssueFor=Taks&ActivityID=" + TreeView1.SelectedNode.Value + "&AName=" + TreeView1.SelectedNode.Text.Replace('&',' ') + "&PrjID=" + DDlProject.SelectedValue;
            //}

            //BindIssues(Session["selected_item"].ToString(), TreeView1.SelectedNode.Value);
            //IssueCountLoad("WorkPackage", TreeView1.SelectedNode.Value);

            if (TreeView1.SelectedNode == null)
            {
                if (Session["selected_item"] == null)
                {
                    TreeView1.Nodes[0].Selected = true;
                    Session["selected_item"] = TreeView1.SelectedNode.Target;
                    Session["ActivityName"] = TreeView1.SelectedNode.Text;
                    Session["NodeValue"] = TreeView1.SelectedNode.Value;
                }
            }
            else
            {
                Session["selected_item"] = TreeView1.SelectedNode.Target;
                Session["ActivityName"] = TreeView1.SelectedNode.Text;
                Session["NodeValue"] = TreeView1.SelectedNode.Value;
            }

            
            if (string.IsNullOrEmpty(status))
                ActivityHeading.Text = Session["ActivityName"].ToString() + " [Total]";
            else
                ActivityHeading.Text = Session["ActivityName"].ToString() + " [" + status + "]";


            if (Session["selected_item"].ToString() == "WorkPackage")
            {
                AddIssues.HRef = "/_modal_pages/add-issues.aspx?IssueFor=WorkPackage&ActivityID=" + Session["NodeValue"].ToString() + "&AName=" + Session["ActivityName"].ToString().Replace('&', ' ') + "&PrjID=" + DDlProject.SelectedValue;
            }
            else
            {
                AddIssues.HRef = "/_modal_pages/add-issues.aspx?IssueFor=Taks&ActivityID=" + Session["NodeValue"].ToString() + "&AName=" + Session["ActivityName"].ToString().Replace('&', ' ') + "&PrjID=" + DDlProject.SelectedValue;
            }

            BindUsers();

            BindIssues(Session["selected_item"].ToString(), Session["NodeValue"].ToString(),"All");
            ColorSelected("");

        
            if (Session["selected_item"].ToString() == "WorkPackage")
                IssueCountLoad("WorkPackage", Session["NodeValue"].ToString());
            else
                IssueCountLoad("Task", Session["NodeValue"].ToString());
        }

        protected void BindIssues(string IssuesFor, string ActivityUID,string user)
        {
            Session["ActivityUID"] = ActivityUID;

           

            if (IssuesFor == "WorkPackage")
            {

                DataSet ds = getdt.getIssuesList_by_WorkPackageUID_IssueStatus(new Guid(ActivityUID),status,DDLUser.SelectedValue );
                if (ds.Tables[0].Rows.Count > 0)
                {
                    printreport.Visible = true;
                }
                else
                {
                    printreport.Visible = false;
                }
                GrdIssues.DataSource = ds;
                GrdIssues.DataBind();
                GrdPrint.DataSource = ds;
                GrdPrint.DataBind();
               // LblTotalIssues.Text = ds.Tables[0].Rows.Count.ToString();

                Session["selected_item"] = "WorkPackage";
                lblTotal.Text = "Total :" + ds.Tables[0].Rows.Count;
            }
            else
            {
                DataSet ds = getdt.getIssuesList_by_TaskUID_IssueStatus(new Guid(ActivityUID), Request.QueryString["status"], DDLUser.SelectedValue);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    printreport.Visible = true;
                }
                else
                {
                    printreport.Visible = false;
                }
                GrdIssues.DataSource = ds;
                GrdIssues.DataBind();
                GrdPrint.DataSource = ds;
                GrdPrint.DataBind();
              // LblTotalIssues.Text = ds.Tables[0].Rows.Count.ToString();

                Session["selected_item"] = "Task";
                lblTotal.Text = "Total :" + ds.Tables[0].Rows.Count;
            }

           // BindUsers();
           // IssueCountLoad(IssuesFor, ActivityUID);
        }

        protected void IssueCountLoad(string IssuesFor, string ActivityID)
        {
            if (IssuesFor == "WorkPackage")
            {
                DataSet ds = getdt.Get_Open_Closed_Rejected_Issues_by_WorkPackageUID(new Guid(ActivityID));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    LblOpenIssues.Text = ds.Tables[0].Rows[0]["OpenIssues"].ToString();
                    LblClosedIssues.Text = ds.Tables[0].Rows[0]["ClosedIssues"].ToString();
                    LblRejectedIssues.Text = ds.Tables[0].Rows[0]["RejectedIssues"].ToString();
                    LblInProgressIssues.Text= ds.Tables[0].Rows[0]["InProgressIssues"].ToString();
                    LblTotalIssues.Text = (Convert.ToInt32(ds.Tables[0].Rows[0]["OpenIssues"].ToString()) + Convert.ToInt32(ds.Tables[0].Rows[0]["ClosedIssues"].ToString()) + Convert.ToInt32(ds.Tables[0].Rows[0]["RejectedIssues"].ToString()) + Convert.ToInt32(ds.Tables[0].Rows[0]["InProgressIssues"].ToString())).ToString();
                }
            }
            else
            {
                DataSet ds = getdt.Get_Open_Closed_Rejected_Issues_by_TaskUID(new Guid(ActivityID));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    LblOpenIssues.Text = ds.Tables[0].Rows[0]["OpenIssues"].ToString();
                    LblClosedIssues.Text = ds.Tables[0].Rows[0]["ClosedIssues"].ToString();
                    LblRejectedIssues.Text = ds.Tables[0].Rows[0]["RejectedIssues"].ToString();
                    LblInProgressIssues.Text = ds.Tables[0].Rows[0]["InProgressIssues"].ToString();

                    LblTotalIssues.Text = (Convert.ToInt32(ds.Tables[0].Rows[0]["OpenIssues"].ToString()) + Convert.ToInt32(ds.Tables[0].Rows[0]["ClosedIssues"].ToString()) + Convert.ToInt32(ds.Tables[0].Rows[0]["RejectedIssues"].ToString()) + Convert.ToInt32(ds.Tables[0].Rows[0]["InProgressIssues"].ToString())).ToString();
                }
            }
        }
           
        protected void GrdIssues_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                if (ViewState["isEdit"].ToString() == "false")
                {
                    e.Row.Cells[11].Visible = false;
                }
                if (ViewState["isAssignUser"].ToString() == "false")
                {
                    e.Row.Cells[12].Visible = false;
                }
                if (ViewState["isDelete"].ToString() == "false")
                {
                    e.Row.Cells[13].Visible = false;
                }
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ds.Clear();
                ds = getdt.getUserDetails(new Guid(e.Row.Cells[3].Text));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    e.Row.Cells[3].Text = ds.Tables[0].Rows[0]["UserName"].ToString();
                }
                ds.Clear();
                //ds = getdt.GetTasksby_TaskUiD(new Guid(e.Row.Cells[2].Text));
                //if (ds.Tables[0].Rows.Count > 0)
                //{
                //    e.Row.Cells[2].Text = ds.Tables[0].Rows[0]["Name"].ToString();
                //}

                

                if (e.Row.Cells[9].Controls.Count == 0)
                {
                    GridViewRow grd_row = e.Row;

                    GridView obj_grdview = new GridView();
                    obj_grdview.Visible = true;

                    DataSet ds1 = getdt.GetUploadedIssueDocuments(e.Row.Cells[14].Text);

                    ds1.Tables[0].Columns.Add("action-1");
                    ds1.Tables[0].Columns.Add("action-2");
                    ds1.Tables[0].Columns.Add("action-3");
                    ds1.Tables[0].Columns.Add("action-4");


                    int count = ds1.Tables[0].Rows.Count;

                    if (count == 0)
                    {
                        LinkButton new_link = new LinkButton();

                        new_link.ID = "No File";
                        new_link.Text = "No File";
                        new_link.Enabled = false;
                        grd_row.Cells[9].Controls.Add(new_link);

                        e.Row.Cells[8].Text = "";
                    }
                    else
                    {
                        obj_grdview.DataSource = ds1.Tables[0];
                        obj_grdview.DataBind();
                        obj_grdview.Attributes.Add("width", "100%");

                        Boolean preview = false;

                        foreach (GridViewRow grd_view_row in obj_grdview.Rows)
                        {

                            grd_view_row.Cells[0].Visible = false;
                            grd_view_row.Cells[1].Visible = false;
                            grd_view_row.Cells[2].Visible = false;

                            
                            LinkButton linkUpload = new LinkButton();

                            linkUpload.ID = "upload_" + grd_view_row.Cells[1].Text;
                            linkUpload.CommandName = "download";
                            linkUpload.CssClass = "fas fa-download";
                            linkUpload.ToolTip = "Download";
                           
                            linkUpload.CommandArgument = grd_view_row.Cells[2].Text + "/" + grd_view_row.Cells[1].Text;

                            linkUpload.Click += linkUpload_Click;

                            grd_view_row.Cells[3].Controls.Add(linkUpload);


                            //-------------------------------------------

                            LinkButton linkInfo = new LinkButton();

                            linkInfo.ID = "info_" + grd_view_row.Cells[1].Text;
                            linkInfo.CommandName = grd_view_row.Cells[0].Text;
                           
                            linkInfo.CssClass = "fas fa-info";
                            linkInfo.ToolTip = grd_view_row.Cells[1].Text;
                            linkInfo.CommandArgument = grd_view_row.Cells[2].Text + "/" + grd_view_row.Cells[1].Text;

                            linkInfo.Click += LinkInfo_Click;

                            grd_view_row.Cells[4].Controls.Add(linkInfo);

                            //-------------------------------------------------------
                            if (ViewState["isDelete"].ToString() == "true")
                            {
                                LinkButton linkDelete = new LinkButton();

                                linkDelete.ID = "delete_" + grd_view_row.Cells[1].Text;
                                linkDelete.CommandName = grd_view_row.Cells[0].Text;
                                // linkDelete.Text = "Delete";
                                linkDelete.CssClass = "fas fa-trash";
                                linkDelete.ToolTip = "Delete";
                                linkDelete.CommandArgument = grd_view_row.Cells[2].Text + "/" + grd_view_row.Cells[1].Text;

                                linkDelete.Click += linkDelete_Click;

                                grd_view_row.Cells[5].Controls.Add(linkDelete);
                            }

                            //-------------------------------------------------

                            string ext = linkUpload.CommandArgument.Substring(linkUpload.CommandArgument.Length - 3, 3);

                            if (ext == "jpg" || ext == "png" || ext == "pdf" || ext == "peg" || ext == "bmp")
                            {
                                LinkButton linkView = new LinkButton();

                                linkView.ID = "view_" + grd_view_row.Cells[1].Text;
                                linkView.CommandName = grd_view_row.Cells[0].Text;
                                //  linkView.Text = "View";
                                linkView.CssClass = "fas fa-eye";
                                linkView.ToolTip = "Preview";
                                linkView.CommandArgument = grd_view_row.Cells[2].Text + "/" + grd_view_row.Cells[1].Text;

                                linkView.Click += LinkView_Click;

                                grd_view_row.Cells[6].Controls.Add(linkView);

                                if (ext == "jpg" || ext == "png" || ext == "peg" || ext == "bmp") preview = true;
                            }
                        }

                        if (!preview)
                            e.Row.Cells[8].Text = "";

                        obj_grdview.HeaderRow.Cells[3].Text = "";
                        obj_grdview.HeaderRow.Cells[4].Text = "";
                        obj_grdview.HeaderRow.Cells[2].Text = "File Name";
                        obj_grdview.HeaderRow.Cells[2].Attributes.Add("width", "50%");

                        obj_grdview.HeaderRow.Cells[0].Visible = false;
                        obj_grdview.HeaderRow.Cells[2].Visible  = false;
                        obj_grdview.HeaderRow.Cells[1].Visible = false;
                        obj_grdview.HeaderRow.Cells[5].Visible = false;

                        obj_grdview.HeaderRow.Visible = false;

                        grd_row.Cells[9].Controls.Add(obj_grdview);
                    }

                   
                }
                else
                {
                    e.Row.Cells[8].Text  = "";
                }

                if (ViewState["isEdit"].ToString() == "false")
                {
                    e.Row.Cells[11].Visible = false;
                }
                if (ViewState["isAssignUser"].ToString() == "false")
                {
                    e.Row.Cells[12].Visible = false;
                }
                if (ViewState["isDelete"].ToString() == "false")
                {
                    e.Row.Cells[13].Visible = false;
                }
                //for db sync check
                if (WebConfigurationManager.AppSettings["Dbsync"] == "Yes")
                {
                    if (!string.IsNullOrEmpty(e.Row.Cells[13].Text))
                    {
                        if (getdt.checkIssuesSynced(new Guid(e.Row.Cells[13].Text)) > 0)
                        {
                            e.Row.BackColor = System.Drawing.Color.LightYellow;
                        }
                        else
                        {
                            //  e.Row.BackColor = System.Drawing.Color.Green;
                            // e.Row.ForeColor = System.Drawing.Color.White;
                        }
                    }
                }

               
            }
        }

        protected void OnPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdIssues.PageIndex = e.NewPageIndex;
            BindIssues(Session["selected_item"].ToString(), TreeView1.SelectedNode.Value,DDLUser.SelectedValue);
        }

        private void LinkInfo_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void LinkView_Click(object sender, EventArgs e)
        {
            if (Session["issue_status_preview"] != null)
            {
                Session["issue_status_preview"] = null;
                return;
            } 
         
            LinkButton new_link = (LinkButton)sender;

            string path = Server.MapPath(new_link.CommandArgument);

            string Extension = System.IO.Path.GetExtension(path);
            string outPath = path.Replace(Extension, "") + "_download" + Extension;
            getdt.DecryptFile(path, outPath);
            System.IO.FileInfo file = new System.IO.FileInfo(outPath);

            string fname = "/Documents/Issues/" + file.Name; // + "?status=" + status;

            if (file.Exists)
            {
                if (Extension == ".jpg" || Extension == ".png" || Extension == ".jpeg" || Extension == ".bmp")
                {
                   ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showImgModal('" + fname + "');", true);
                }
                else if (Extension == ".pdf")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showPdfModal('" + fname + "');", true);
                }
            }
        }

        private void linkDelete_Click(object sender, EventArgs e)
        {
            LinkButton new_link = (LinkButton)sender;

            int file_count = getdt.DeleteUploadedIssueDoc(Convert.ToInt32(new_link.CommandName));

            if (file_count != 0)
            {
                string fileName = Server.MapPath(new_link.CommandArgument);

                if (fileName != null || fileName != string.Empty)
                {
                    if ((System.IO.File.Exists(fileName)))
                    {
                        System.IO.File.Delete(fileName);
                    }
                }
            }

            BindIssues(Session["selected_item"].ToString(), TreeView1.SelectedNode.Value,DDLUser.SelectedValue);
        }

        private void linkUpload_Click(object sender, EventArgs e)
        {
            LinkButton new_link = (LinkButton)sender;

            string path = Server.MapPath(new_link.CommandArgument);

            string getExtension = System.IO.Path.GetExtension(path);
            string outPath = path.Replace(getExtension, "") + "_download" + getExtension;
            getdt.DecryptFile(path, outPath);
            System.IO.FileInfo file = new System.IO.FileInfo(outPath);

            if (file.Exists)
            {
                Response.Clear();

                Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);

                Response.AddHeader("Content-Length", file.Length.ToString());

                Response.ContentType = "application/octet-stream";

                Response.TransmitFile(outPath);

                Response.End();

            }
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('File does not exist.');</script>");
            }
        }

        protected void GrdPrint_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ds.Clear();
                ds = getdt.getUserDetails(new Guid(e.Row.Cells[2].Text));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    e.Row.Cells[2].Text = ds.Tables[0].Rows[0]["UserName"].ToString();
                }

                //string IssueUID = e.Row.Cells[7].Text;
                //GridView gvIssueRemarks = e.Row.FindControl("gvIssueRemarks") as GridView;
                //BindIssueStatus(IssueUID, gvIssueRemarks);


                //GridView gvDocs = e.Row.FindControl("gvUploadedDocs") as GridView;
                //BindUploadedDocuments(IssueUID, gvDocs);

                //foreach(GridViewRow r in gvIssueRemarks.Rows)
                //{
                //    GridView gvRemDocs = r.FindControl("gvUploadedRemDocs") as GridView;
                //    BindUploadedRemDocuments(r.Cells[3].Text, gvRemDocs);
                //}
                
            }
        }

        private void BindIssueStatus(string IssueId,GridView gvRemarks)
        {
            DataSet ds = getdt.GetIssueStatus_by_Issue_Uid(new Guid(IssueId));
            gvRemarks.DataSource = ds;
            gvRemarks.DataBind();
        }

        private void BindUploadedDocuments(string IssueId, GridView gvDocs)
        {
            DataSet ds1 = getdt.GetUploadedIssueDocuments(IssueId);
            gvDocs.DataSource = ds1;
            gvDocs.DataBind();
        }

        private void BindUploadedRemDocuments(string IssueRemId, GridView gvRemDocs)
        {
            DataSet ds = getdt.GetUploadedDocuments(IssueRemId);
            gvRemDocs.DataSource = ds;
            gvRemDocs.DataBind();
        }

        protected void GrdIssues_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string IssueUID = e.CommandArgument.ToString();
            if (e.CommandName == "download")
            {
                //DataSet ds = getdt.getIssuesList_by_UID(new Guid(IssueUID));
                //if (ds.Tables[0].Rows.Count > 0)
                //{
                //    string path = Server.MapPath(ds.Tables[0].Rows[0]["Issue_Document"].ToString());

                //    string getExtension = System.IO.Path.GetExtension(path);
                //    string outPath = path.Replace(getExtension, "") + "_download" + getExtension;
                //    getdt.DecryptFile(path, outPath);
                //    System.IO.FileInfo file = new System.IO.FileInfo(outPath);

                //    if (file.Exists)
                //    {
                //        Response.Clear();

                //        Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);

                //        Response.AddHeader("Content-Length", file.Length.ToString());

                //        Response.ContentType = "application/octet-stream";

                //        Response.WriteFile(file.FullName);

                //        Response.End();

                //    }

                //    else
                //    {

                //        //Response.Write("This file does not exist.");
                //        Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('File does not exist.');</script>");

                //    }
                //}


                //string path = Server.MapPath("~/Documents/Issues/Output/");//Location for inside Test Folder  
                //string[] Filenames = Directory.GetFiles(path);
                //using (Ionic.Zip.ZipFile zip = new Ionic.Zip.ZipFile())
                //{
                //    zip.AddFiles(Filenames, "Project");//Zip file inside filename  
                //    zip.Save(@"C:\Users\SAJI AUGUSTIN\Downloads\Projectzip.zip");//location and name for creating zip file  

                //}

                CreateAndDownloadZipFile(IssueUID);


      
            }

            if (e.CommandName == "delete")
            {
                int cnt = getdt.Issue_Delete(new Guid(IssueUID), new Guid(Session["UserUID"].ToString()));
                if (cnt > 0)
                {
                    BindIssues(TreeView1.SelectedNode.Target, TreeView1.SelectedNode.Value,DDLUser.SelectedValue);
                    IssueCountLoad(TreeView1.SelectedNode.Target, TreeView1.SelectedNode.Value);
                }
            }


        }


        protected void CreateAndDownloadZipFile(string issue_uid)
        {
                                  
            string zipFolder = Server.MapPath("/Documents/Issues/IssueDocs/");

            if (!Directory.Exists(zipFolder))
                Directory.CreateDirectory(zipFolder);
            else
            {
                    // Delete all files from the Directory  
                    foreach (string filename in Directory.GetFiles(zipFolder))
                    {
                        File.Delete(filename);
                    }
            }

            List<DocFile> files = new List<DocFile>();

            List<string> fileNames = new List<string>();

            DataSet ds1 = getdt.GetUploadedIssueDocuments(issue_uid);

            if (ds1.Tables[0].Rows.Count >0)
            {
                foreach(DataRow r in ds1.Tables[0].Rows)
                {
                   
                    string path = Server.MapPath(r.ItemArray[2].ToString() + r.ItemArray[1].ToString());

                    string outPath = zipFolder + "open_" + r.ItemArray[1].ToString();

                    string getExtension = System.IO.Path.GetExtension(path);

                    outPath = outPath.Replace(getExtension, "")  + getExtension;

                    getdt.DecryptFile(path, outPath);

                    if (files.Where(a => a.Name.Contains(outPath)).Count() == 0)
                        files.Add(new DocFile { Name = outPath, Position=0 });
                }
              
            }

            DataSet ds2 = getdt.GetUploadedAllIssueStatusDocuments(issue_uid);

            if (ds2.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in ds2.Tables[0].Rows)
                {
                    int p = 0;

                    string path = Server.MapPath(r.ItemArray[2].ToString() + r.ItemArray[1].ToString());

                    string outPath = zipFolder + r.ItemArray[3].ToString() + "_" + r.ItemArray[1].ToString();

                    string getExtension = Path.GetExtension(path);

                    outPath = outPath.Replace(getExtension, "") +  getExtension;

                    getdt.DecryptFile(path, outPath);

                    if (r.ItemArray[3].ToString() == "In-Progress") p = 1;

                    DocFile doc_file = new DocFile() { Name = outPath, Position = p };

                    if (files.Where(a=>a.Name.Contains(outPath)).Count() == 0)
                        files.Add(doc_file);
                }
            }

            // Save Zip file

            string zipFilePath = Server.MapPath("/Documents/Issues/IssueDocsZip.zip");

            foreach(DocFile item in files.OrderBy(a=>a.Position))
            {
                fileNames.Add(item.Name);
            }

            using (Ionic.Zip.ZipFile zip = new Ionic.Zip.ZipFile())
            {
                zip.AddFiles(fileNames, "IssueDocs");//Zip file inside filename  
                zip.Save(zipFilePath);//location and name for creating zip file  
            }

            FileInfo file = new FileInfo(zipFilePath);

            if (file.Exists)
            {
                Response.Clear();

                Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);

                Response.AddHeader("Content-Length", file.Length.ToString());

                Response.ContentType = "application/octet-stream";

                Response.WriteFile(file.FullName);

                Response.End();

            }
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('File does not exist.');</script>");
            }
        }

       
        protected void GrdIssues_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        //private void PopulateTreeView(IEnumerable<T> list, TreeNode parentNode)
        //{
        //    var nodes = list.Where(x => parentNode == null ? x.ParentId == null : x.ParentId == parentNode.Value).ToList();

        //    foreach (var node in nodes)
        //    {
        //        TreeNode newNode = new TreeNode(node.Name, node.Id.ToString());

        //        newNode.ShowCheckBox = true;
        //        newNode.Checked = node.TaskSelected;

        //        if (parentNode == null)
        //        {
        //            TreeView1.Nodes.Add(newNode);
        //        }
        //        else
        //        {
        //            parentNode.ChildNodes.Add(newNode);
        //        }

        //        PopulateTreeView(list, newNode);
        //    }
        //}

        private Boolean  TreeviewTraverseAndFind(string SearchId,TreeNode SearchingNode)
        {
            
            foreach(TreeNode Nd in SearchingNode.ChildNodes)
            {
                if (Nd.ChildNodes.Count >0)
                {
                    if (Nd.Text == SearchId)
                    {
                        Nd.Selected = true;
                        return true;
                    }

                    TreeviewTraverseAndFind(SearchId, Nd);
                }
                else
                {
                    if (Nd.Value == SearchId)
                    {
                        Nd.Selected = true;
                        return true;
                    }
                }
            }

            return false;
        }
    }
}