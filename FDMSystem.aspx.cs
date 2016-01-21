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
        string sys_obid = "";
        DataSet System_Effect = new DataSet();
        DataTable dt_Effect = new DataTable();
        DataTable dt_System = new DataTable();
        Boolean wantload = false;
        string zone = "";
        string loc_cause = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            fgm_moduler = new FGMModule("FDM", SessionInfo.ProjectName, "TagGroupDetector");
            System_Effect.Tables.AddRange(new DataTable[] { dt_System, dt_Effect });
            //zone = Session["zone"].ToString();
            loc_cause = Session["loc_cause"].ToString();
            if (!IsPostBack)
            {
                Setup_Zone();
             
            }
            zone = ddl_zone.SelectedItem.Value.ToString();
            if (ddl_System.SelectedIndex < 1)
            {
                Setup_Sys();
            }  
          
            if (ddl_System.SelectedItem != null)
            {
                sys_obid = ddl_System.SelectedItem.Value.ToString();
                makeCauseGrid(sys_obid);
                makeEffectDataset(sys_obid);
            }
            else
            {
                makeCauseGrid("");
                makeEffectDataset("");
            }
            
            Session["Dataset"] = System_Effect;
           
        }
        protected void Setup_Zone()
        {
            DataSet ds_zone = new DataSet();
            SqlDataAdapter data_adapter;
            string load_qry = "SELECT LOOKUPDETNAME, LOOKUPDETDESC FROM LOOKUPDET WHERE DEPENDENTPROJECT = '" + fgm_moduler.func_Class.project_Name + "' ";
            load_qry = load_qry + " AND LOOKUPNAME = 'ID_ZONE' AND TERMINATIONDATE = '' AND LOOKUPDETNAME <> 'COMMON' ORDER BY LOOKUPDETDESC";
            data_adapter = new SqlDataAdapter(load_qry, fgm_moduler.func_Class.admin_connString);
            data_adapter.Fill(ds_zone);

            ddl_zone.DataSource = ds_zone.Tables[0];
            ddl_zone.DataTextField = "LOOKUPDETNAME";
            ddl_zone.DataValueField = "LOOKUPDETNAME";
            ddl_zone.DataTextFormatString = "Zone {0}";
            ddl_zone.DataBind();
            
        }
        protected void Setup_Sys()
        {
            DataSet ds_Sys_name = new DataSet();
            SqlDataAdapter data_adapter;
            string load_qry = "select A.N1NAME, C.OBID, C.SYS_NAME FROM N2REVGRP A INNER JOIN N3OBJATTR B ON A.OBID = B.LEFTREL INNER JOIN TBLEFFECT_NEW C ON B.RIGHTREL = C.OBID ";
            load_qry = load_qry + " WHERE A.N1CLASS = 'TagGroupDetector' AND A.N1TERMINATIONDATE = '' AND B.N1TERMINATIONDATE = '' AND (C.TEMPLATE_NAME = 'DELUGE' OR C.TEMPLATE_NAME = 'CLEAN AGENT')  ";
            load_qry = load_qry + " AND A.N1ORIGCLASSDEFUID= '" + zone + "' and A.N1PROJECTNAME = '" + SessionInfo.ProjectName + "' ";
            data_adapter = new SqlDataAdapter(load_qry, fgm_moduler.func_Class.connString);
            data_adapter.Fill(ds_Sys_name);

            for (int i = 0; i < ds_Sys_name.Tables[0].Rows.Count; i++)
            {
                ds_Sys_name.Tables[0].Rows[i]["SYS_NAME"] = ds_Sys_name.Tables[0].Rows[i]["SYS_NAME"] + "(" + ds_Sys_name.Tables[0].Rows[i]["N1NAME"].ToString() + ")";                
            }
            
            ddl_System.DataSource = ds_Sys_name.Tables[0];
            ddl_System.DataTextField = "SYS_NAME";
            ddl_System.DataValueField = "OBID";
            ddl_System.DataBind();
        }

        protected void makeCauseGrid(string sys_obid)
        {
            SqlDataAdapter data_adapter;
            string load_qry = "  SELECT A.TAGNO, A.RIGHTREL, A.LEFTREL, B.DESCRIPTION, B.SEQ, B.OBID, B.PARENT_OBID, B.LOC1, B.LOC2 ";
            load_qry = load_qry + " FROM TBLSYSTEM_TAGNO A   INNER JOIN TBLSYSTEM_TEMPLATE B ON A.LEFTREL = B.OBID WHERE B.CLASS_NAME = 'Cause' ";
            load_qry = load_qry + " AND  (B.N1TERMINATIONDATE = '' OR B.N1TERMINATIONDATE IS NULL) AND  A.RIGHTREL = '" + sys_obid + "' AND B.PRJ_NAME = '" + SessionInfo.ProjectName + "'  ORDER BY B.SEQ ";
                  
            data_adapter = new SqlDataAdapter(load_qry, fgm_moduler.func_Class.connString);
            data_adapter.Fill(dt_System);

            CauseGrid.DataSource = dt_System;
            CauseGrid.DataBind();
        }

        protected void makeEffectDataset(string sys_obid)
        {
            dt_Effect.Clear();
            SqlDataAdapter data_adapter;
            string load_qry = "  SELECT  A.TAGNO, A.RIGHTREL, A.LEFTREL, B.DESCRIPTION, B.SEQ, B.OBID, B.PARENT_OBID, B.LOC1, B.LOC2  ";
            load_qry = load_qry + "  FROM TBLSYSTEM_TAGNO A   INNER JOIN TBLSYSTEM_TEMPLATE B ON A.LEFTREL = B.OBID WHERE B.CLASS_NAME = 'Effect'";
            load_qry = load_qry + " AND  (B.N1TERMINATIONDATE = '' OR B.N1TERMINATIONDATE IS NULL) AND  A.RIGHTREL = '" + sys_obid + "' AND B.PRJ_NAME = '" + SessionInfo.ProjectName + "'  ORDER BY B.SEQ ";
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
        }

        protected void GridGroupDetail_BatchUpdate(object sender, ASPxDataBatchUpdateEventArgs e)
        {
            DataTable dt = new DataTable();
           
            foreach (var args in e.UpdateValues)
            {
                string OBID = args.Keys[0].ToString(); // leftrel
                string tag_no = args.NewValues[2].ToString();
                string right_rel = CauseGrid.GetRowValuesByKeyValue(OBID, "RIGHTREL").ToString();
                string qry = "  UPDATE TBLSYSTEM_TAGNO SET TAGNO = '" + tag_no + "' ";
                qry = qry + " WHERE LEFTREL = '" + OBID + "' AND RIGHTREL = '" + right_rel + "' ";
                SqlCommand cmd;
                cmd = new SqlCommand(qry, fgm_moduler.func_Class.conn);
                cmd.ExecuteNonQuery();
            }
        }

        protected void Detail_BatchUpdate(object sender, ASPxDataBatchUpdateEventArgs e)
        {
            DataTable dt = new DataTable();
            ASPxGridView EffectGrid = (ASPxGridView)sender;

            foreach (var args in e.UpdateValues)
            {
                string OBID = args.Keys[0].ToString();// leftrel
                string tag_no = args.NewValues[3].ToString();
                string right_rel = EffectGrid.GetRowValuesByKeyValue(OBID, "RIGHTREL").ToString();
                string qry = "  UPDATE TBLSYSTEM_TAGNO SET TAGNO = '" + tag_no + "' ";
                qry = qry + " WHERE LEFTREL = '" + OBID + "' AND RIGHTREL = '" + right_rel + "'  ";
                SqlCommand cmd;
                cmd = new SqlCommand(qry, fgm_moduler.func_Class.conn);
                cmd.ExecuteNonQuery();
            }
        }

   



    }
}