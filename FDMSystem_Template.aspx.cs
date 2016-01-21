using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using DevExpress.Web;
using DevExpress.Web.ASPxGridView;
using DevExpress.Web.ASPxGridView.Export;
using DevExpress.Web.ASPxEditors;
using DevExpress.Web.ASPxFormLayout;
using DevExpress.Web.ASPxClasses;
using DevExpress.XtraPrinting;
using System.Collections;
using DevExpress.Web.Data;
using System.Collections;

namespace EDISON
{
    public partial class FDMSystem : System.Web.UI.Page
    {
        FGMModule fgm_moduler;
        string sys_name = "";
        DataSet System_Effect = new DataSet();
        DataTable dt_Effect = new DataTable();
        DataTable dt_System = new DataTable();
        Boolean wantload = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            fgm_moduler = new FGMModule("FDM", SessionInfo.ProjectName  , "TagGroupDetector");
            System_Effect.Tables.AddRange(new DataTable[] { dt_System, dt_Effect });

            if (!IsPostBack)
            {
                Setup_Sys();
                CauseGrid.DataSource = null;
                CauseGrid.DataBind();

            }          
            sys_name = ddl_System.SelectedValue;
            makeCauseGrid(sys_name);
            makeEffectDataset(sys_name);
            Session["Dataset"] = System_Effect;
        }
        protected void Setup_Sys()
        {
            DataSet ds_Sys_name = new DataSet();
            SqlDataAdapter data_adapter;
            string load_qry = "SELECT SYS_NAME FROM dbo.TBLSYSTEM_TEMPLATE where PRJ_NAME = '" + fgm_moduler.func_Class.project_Name + "'  AND CLASS_NAME = 'NAME' ";
            load_qry = load_qry + " AND (N1TERMINATIONDATE = '' OR N1TERMINATIONDATE IS NULL)  AND (N1TERMINATIONDATE = '' OR N1TERMINATIONDATE IS NULL)  ";
            data_adapter = new SqlDataAdapter(load_qry, fgm_moduler.func_Class.connString);
            data_adapter.Fill(ds_Sys_name);

            ddl_System.DataSource = ds_Sys_name.Tables[0];
            ddl_System.DataTextField = "SYS_NAME";
            ddl_System.DataValueField = "SYS_NAME";
            ddl_System.DataBind();           
        }

        protected void makeCauseGrid(string sys_name)
        {
            SqlDataAdapter data_adapter;
            string load_qry = "SELECT OBID, DESCRIPTION, SEQ FROM dbo.TBLSYSTEM_TEMPLATE where PRJ_NAME = '" + fgm_moduler.func_Class.project_Name + "' AND CLASS_NAME = 'Cause' ";
            load_qry = load_qry + "AND SYS_NAME ='" + sys_name + "'  AND (N1TERMINATIONDATE = '' OR N1TERMINATIONDATE IS NULL)  ORDER BY SEQ";
            data_adapter = new SqlDataAdapter(load_qry, fgm_moduler.func_Class.connString);
            data_adapter.Fill(dt_System);

            CauseGrid.DataSource = dt_System;
            CauseGrid.DataBind();
            
        }

        protected void makeEffectDataset(string sys_name)
        {
            dt_Effect.Clear();
            SqlDataAdapter data_adapter;
            string load_qry = "SELECT DESCRIPTION, PARENT_OBID, OBID, LOC1, LOC2 FROM dbo.TBLSYSTEM_TEMPLATE where PRJ_NAME = '" + fgm_moduler.func_Class.project_Name + "' AND CLASS_NAME = 'Effect' ";
            load_qry = load_qry + "AND SYS_NAME ='" + sys_name + "' AND (N1TERMINATIONDATE = '' OR N1TERMINATIONDATE IS NULL) ";
            data_adapter = new SqlDataAdapter(load_qry, fgm_moduler.func_Class.connString);
            data_adapter.Fill(dt_Effect);    
        }

        public void Detail_BeforePerformDataSelect(object sender, EventArgs e)
        {
            System_Effect = (DataSet)Session["DataSet"];
            DataTable detailTable = System_Effect.Tables[1];
            DataView dv = new DataView(detailTable);
            ASPxGridView detailGridView = (ASPxGridView)sender;
            dv.RowFilter = "PARENT_OBID = '" + detailGridView.GetMasterRowKeyValue().ToString() + "'";
            detailGridView.DataSource = dv;

            //detailGridView.Templates.EditForm = new EditFormTemplate_DV();
        }

        protected void btn_delete_Click(object sender, EventArgs e)
        {
            string qry = " UPDATE TBLSYSTEM_TEMPLATE SET N1TERMINATIONDATE = REPLACE(REPLACE(CONVERT(varchar(23), GETDATE(), 121), '-','/'),'  ', '-'),  ";
            qry = qry + "N1TERMINATIONID = '" + SessionInfo.UserID + "'  ";
            qry = qry + "WHERE SYS_NAME = '" + sys_name + "' AND PRJ_NAME = '" + fgm_moduler.func_Class.project_Name + "' ";
            SqlCommand cmd;
            cmd = new SqlCommand(qry, fgm_moduler.func_Class.conn);
            cmd.ExecuteNonQuery();

            Setup_Sys();
            sys_name = ddl_System.SelectedValue;
        }

        protected void btn_Add_Click(object sender, EventArgs e)
        {

            if (tb_sys_name.Text == "")
                return;

            string qry = " INSERT INTO TBLSYSTEM_TEMPLATE(OBID, SYS_NAME, CLASS_NAME, SEQ, PARENT_OBID, DESCRIPTION, PRJ_NAME) ";
            qry = qry + "VALUES(NEWID(), '" + tb_sys_name.Text + "', 'NAME', '0', '', '', '" + fgm_moduler.func_Class.project_Name + "')  ";
            SqlCommand cmd;
            cmd = new SqlCommand(qry, fgm_moduler.func_Class.conn);
            cmd.ExecuteNonQuery();

            tb_sys_name.Text = "";
            Setup_Sys();
            sys_name = ddl_System.SelectedValue;
        }
        public void Cause_Deleting(object sender, ASPxDataDeletingEventArgs e)
        {
            SqlCommand cmd;
            ASPxGridView detailGridView = (ASPxGridView)sender;
            string Selected_OBID = e.Keys[CauseGrid.KeyFieldName].ToString();
            string qry = " UPDATE TBLSYSTEM_TEMPLATE SET N1TERMINATIONDATE = REPLACE(REPLACE(CONVERT(varchar(23), GETDATE(), 121), '-','/'),'  ', '-'),  ";
            qry = qry + "N1TERMINATIONID = '" + SessionInfo.UserID + "'  ";
            qry = qry + "WHERE OBID = '"+Selected_OBID+"' ";
            cmd = new SqlCommand(qry, fgm_moduler.func_Class.conn);
            cmd.ExecuteNonQuery();

            string qry2 = " UPDATE TBLSYSTEM_TEMPLATE SET N1TERMINATIONDATE = REPLACE(REPLACE(CONVERT(varchar(23), GETDATE(), 121), '-','/'),'  ', '-'),  ";
            qry2 = qry2 + "N1TERMINATIONID = '" + SessionInfo.UserID + "'  ";
            qry2 = qry2 + "WHERE PARENT_OBID = '" + Selected_OBID + "' ";
            cmd = new SqlCommand(qry2, fgm_moduler.func_Class.conn);
            cmd.ExecuteNonQuery();

            e.Cancel = true;
            CauseGrid.CancelEdit();
        }
        protected void Cause_Inserting(object sender, ASPxDataInsertingEventArgs e)
        {
            string desc = e.NewValues[0].ToString();
            string seq = e.NewValues[1].ToString();

            string qry = "INSERT INTO TBLSYSTEM_TEMPLATE(OBID, SYS_NAME, CLASS_NAME, SEQ, PARENT_OBID, DESCRIPTION, PRJ_NAME) ";
            qry = qry + "VALUES(NEWID(), '" + sys_name + "', 'Cause', '" + seq + "', '" + sys_name + "', '" + desc + "', '" + fgm_moduler.func_Class.project_Name + "')";
            SqlCommand cmd;
            cmd = new SqlCommand(qry, fgm_moduler.func_Class.conn);
            cmd.ExecuteNonQuery();

            e.Cancel = true;
            CauseGrid.CancelEdit();
        }
        protected void GridGroupDetail_BatchUpdate(object sender, ASPxDataBatchUpdateEventArgs e)
        {
            DataTable dt = new DataTable();


            foreach (var args in e.UpdateValues)
            {
                string OBID = args.Keys[0].ToString();
                string desc_new = args.NewValues[0].ToString();
                string seq_new = args.NewValues[1].ToString();

                string qry = "  UPDATE TBLSYSTEM_TEMPLATE SET DESCRIPTION = '"+desc_new+"', SEQ = '"+seq_new+"' ";
                qry = qry + " WHERE OBID = '" + OBID + "'  ";
                SqlCommand cmd;
                cmd = new SqlCommand(qry, fgm_moduler.func_Class.conn);
                cmd.ExecuteNonQuery();
            }
        }
                

        protected void Effect_Inserting(object sender, ASPxDataInsertingEventArgs e)
        {
            ASPxGridView detailGridView = (ASPxGridView)sender;
            string desc = "";
            string loc1 = "";
            string loc2 = "";
            try
            {
                loc1 = e.NewValues[0].ToString();
                loc2 = e.NewValues[1].ToString();
                desc = e.NewValues[2].ToString();
            }
            catch (Exception ex)
            {

            }
            finally 
            { 

            }
                
            string PARENT_OBID = detailGridView.GetMasterRowKeyValue().ToString();
            string qry = "INSERT INTO TBLSYSTEM_TEMPLATE(OBID, SYS_NAME, CLASS_NAME, SEQ, LOC1, LOC2, PARENT_OBID, DESCRIPTION, PRJ_NAME) ";
            qry = qry + "VALUES(NEWID(), '" + sys_name + "', 'Effect', '0', '" + loc1 + "','" + loc2 + "','" + PARENT_OBID + "', '" + desc + "', '" + fgm_moduler.func_Class.project_Name + "')";
            SqlCommand cmd;
            cmd = new SqlCommand(qry, fgm_moduler.func_Class.conn);
            cmd.ExecuteNonQuery();

            e.Cancel = true;
            detailGridView.CancelEdit();


        }

        public void Effect_Deleting(object sender, ASPxDataDeletingEventArgs e)
        {
            SqlCommand cmd;
            ASPxGridView EffectGrid = (ASPxGridView)sender;
            string obid = e.Keys[EffectGrid.KeyFieldName].ToString();
            string qry = " UPDATE TBLSYSTEM_TEMPLATE SET N1TERMINATIONDATE = REPLACE(REPLACE(CONVERT(varchar(23), GETDATE(), 121), '-','/'),'  ', '-'),  ";
            qry = qry + "N1TERMINATIONID = '" + SessionInfo.UserID + "'  ";
            qry = qry + "WHERE OBID = '" + obid + "' ";
            cmd = new SqlCommand(qry, fgm_moduler.func_Class.conn);
            cmd.ExecuteNonQuery();

            e.Cancel = true;
            EffectGrid.CancelEdit();
        }

        protected void Detail_BatchUpdate(object sender, ASPxDataBatchUpdateEventArgs e)
        {
            DataTable dt = new DataTable();


            foreach (var args in e.UpdateValues)
            {
                string OBID = args.Keys[0].ToString();
                string loc1_new  = args.NewValues[0].ToString();
                string loc2_new = args.NewValues[1].ToString();
                string desc_new = args.NewValues[2].ToString();

                string qry = "  UPDATE TBLSYSTEM_TEMPLATE SET DESCRIPTION = '" + desc_new + "', LOC1 = '" + loc1_new + "' , LOC2 = '" + loc2_new + "'";
                qry = qry + " WHERE OBID = '" + OBID + "'  ";
                SqlCommand cmd;
                cmd = new SqlCommand(qry, fgm_moduler.func_Class.conn);
                cmd.ExecuteNonQuery();
            }
        }

    }
}