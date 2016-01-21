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

using DevExpress.Web.Data;
using System.Collections;

using Aspose.Cells;
using System.IO;
using System.Drawing;



namespace EDISON
{
    public partial class FGMMain : System.Web.UI.Page
    {
        FGMModule fgm_moduler;
        XmlDocument env_config;
        DataSet ds;
        DataView dv;
        DataTable dynamic_table;
        DataTable dynamic_table_detector;
        DataTable dynamic_table_effect;
        string module_Name = "FDM";
        string group_Name = "";
        string tag_type = "";
        string zone = "";
        string issue_state = "";
        string Detector_obid = "";
        string Detector_type = "";
        string Detector_unit = "";
        string Detector_loc = "";
        string Detector_cnt = "";
        //string Detector_desc = "";
        
        public string conn_string = "";
        protected void Page_Load(object sender, EventArgs e)
        {
                fgm_moduler = new FGMModule("FDM", SessionInfo.ProjectName, "TagGroupDetector");
                tag_type = Request.QueryString["tagType"];
                tagType_HiddenField.Value = tag_type;
                hidden_prj_name.Value = SessionInfo.ProjectName;
                GridGroupDetector.JSProperties["cpConfirmationMessage"] = "";    
            GridGroupDetail.JSProperties["cpConfirmationMessage"] = "";
                GridDetector.JSProperties["cpConfirmationMessage"] = "";

                env_config = new XmlDocument();
                env_config.Load(fgm_moduler.func_Class.get_EnvFilePath());
                if (!IsPostBack)
                {
                    Setup_Zone();
                }
                zone = ddl_zone.SelectedValue;
                Session["zone"] = zone;

            
                if (tag_type == "GroupDetector")
                {
                    btn_Issue.Visible = true;
                    btn_Import.Visible = true;
                    btn_add.Visible = true;
                    btn_delete.Visible = true;
                    btn_save.Visible = true;
                    btn_Export.Visible = true;
                    btn_Export1.Visible = false;
                    makeViewGridGroupDetector();
                    //if (Page.IsCallback || !Page.IsPostBack)
                    //{
                    if (GridGroupDetector.FocusedRowIndex > -1)
                    {
                        int focusedRowIndex = GridGroupDetector.FocusedRowIndex;
                        if (GridGroupDetector.GetRowValues(GridGroupDetector.FocusedRowIndex, "OBID") == null)
                        {
                            focusedRowIndex = 0;
                        }
                        Detector_obid = (string)GridGroupDetector.GetRowValues(focusedRowIndex, "OBID");
                        tagName.Value = (string)GridGroupDetector.GetRowValues(focusedRowIndex, "TagNo");
                        makeViewGridGroupDetail(Detector_obid);
                        Session["S_DB_NAME"] = GridGroupDetail.GetRowValues(GridGroupDetail.FocusedRowIndex, "DB_NAME");
                        makeViewGridEffect(Detector_obid);
                        makeViewGridDetectorDetail(Detector_obid);

                        //gv1temp.Templates.EditForm = new EditFormTemplate();
                        //EditFormTemplate editform_template = new EditFormTemplate();
                        //editform_template.dynamic_effect = dynamic_table_effect;
                        //gv1temp.Templates.EditForm = editform_template;
                    }
                    //}
                }
                else if (tag_type == "Detector")
                {
                    taglist.Style["width"] = "50%";
                    taglist2.Style["width"] = "47%";

                    btn_Issue.Visible = false;
                    btn_Import.Visible = false;
                    btn_add.Visible = false;
                    btn_delete.Visible = false;
                    btn_save.Visible = false;
                    btn_Export.Visible = false;
                    btn_Export1.Visible = true;

                    makeViewGridDetectorList();
                    if (GridGroupDetector.FocusedRowIndex > -1)
                    {
                        Detector_obid = GridGroupDetector.GetRowValues(GridGroupDetector.FocusedRowIndex, "GROUP_OBID").ToString();

                        makeViewGridGroupDetail(Detector_obid);
                        makeViewGridEffect(Detector_obid);
                        //makeViewGridDetectorDetail(Detector_obid);
                        GridDetector.Visible = false;
                    }
                    GridGroupDetail.SettingsEditing.Mode = GridViewEditingMode.Inline;
                    gv1temp.Columns["Command"].Visible = false;

                    //gv1temp.Templates.EditForm = new EditFormTemplate();
               }

        }
        protected void makeViewGridGroupDetector()
        {
            //ds = fgm_moduler.Load_GroupDetectorData(fgm_moduler.func_Class.conn, "TagGroupDetector", "GroupDetector");
            ds = fgm_moduler.Load_GroupDetectorData(fgm_moduler.func_Class.conn, "TagGroupDetector", zone);

            DataColumn row_index = new DataColumn();
            row_index.ColumnName = "No.";
            ds.Tables[0].Columns.Add(row_index);
            ds.Tables[0].Columns["No."].SetOrdinal(0);
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                ds.Tables[0].Rows[i]["No."] = i + 1;
            }
            GridGroupDetector.DataSource = ds;
            GridGroupDetector.DataBind();
            GridGroupDetector.Columns["OBID"].Visible = false;
            GridGroupDetector.Columns["Count"].Visible = false;
            GridGroupDetector.Columns["N1ISSUESTATE"].Visible = false;
            GridGroupDetector.Columns["No."].Width = 30;
            GridGroupDetector.Columns["No."].CellStyle.Font.Bold = true;
            //GridGroupDetector.Columns["No."].CellStyle.BackColor = Color.LightGray;
            GridGroupDetector.Columns["Rev"].Width = 30;
            GridGroupDetector.Columns["Type"].Width = 50;
            GridGroupDetector.Columns["Unit"].Width = 50;
            GridGroupDetector.KeyFieldName = "OBID";


            GroupDetectorEditFormTemplate edit_template = new GroupDetectorEditFormTemplate();
            GridGroupDetector.Templates.EditForm = edit_template;
            edit_template.datafield = Srch_DataField("FDM");
        }
        protected void makeViewGridDetectorList()
        {
            ds = fgm_moduler.Load_DetectorData(fgm_moduler.func_Class.conn, "TagDetector", zone);

            GridGroupDetector.DataSource = ds;
            GridGroupDetector.DataBind();
            GridGroupDetector.Columns["OBID"].Visible = false;
            GridGroupDetector.Columns["GROUP_OBID"].Visible = false;
            GridGroupDetector.Columns["N1ISSUESTATE"].Visible = false;
            GridGroupDetector.KeyFieldName = "OBID";
        }
        protected void makeViewGridGroupDetail(string obid)
        {
            dynamic_table = Srch_DataField("FDM");
            Fill_Record(ref dynamic_table, obid);
            //DataView dv = ds.Tables[0].DefaultView;
            //dv.RowFilter = "OBID = '" + GridGroupDetector.GetRowValues(GridGroupDetector.FocusedRowIndex, "OBID").ToString() + "'";
            //GridGroupDetail.DataSource = dv;
            //GridGroupDetail.DataBind();
        }
        protected void makeViewGridDetectorDetail(string obid)
        {
            //string load_qry;
            //SqlDataAdapter data_adapter;
            //DataSet data_DataSet = new DataSet();
            //data_DataSet.Tables.Add(Srch_DataField("FDMDetector"));

            dynamic_table_detector = Srch_DataField_Detector("FDMDetector");
            Fill_Record_Detector(ref dynamic_table_detector, obid);


            //load_qry = " SELECT D.OBID, C.N1NAME AS [DETECTOR], C.N1LIBRARYNO AS [SEQ], RELDEFID, E.N1STRVALUE, G.DB_NAME,G.DESC_NAME FROM N2REVGRP C ";
            //load_qry = load_qry + " INNER JOIN N3RGRPRGRP R ON R.LEFTREL = C.OBID ";
            //load_qry = load_qry + " INNER JOIN N3OBJATTR D ON C.OBID = D.LEFTREL ";
            //load_qry = load_qry + " INNER JOIN N2SPCATTR E ON D.RIGHTREL = E.OBID ";
            //load_qry = load_qry + " INNER JOIN N2CLSATTR F ON E.N1ATTRCLASSOBID = F.OBID  ";
            //load_qry = load_qry + " INNER JOIN N3SECTION G ON G.DB_NAME = F.N1ATTRCLASSNAME ";
            //load_qry = load_qry + " WHERE C.N1TERMINATIONDATE = '' AND C.N1PROJECTNAME = '" + fgm_moduler.func_Class.project_Name + "' ";
            //load_qry = load_qry + " AND R.N1TERMINATIONDATE = '' AND R.RIGHTREL = '" + obid + "' AND R.N1TERMINATIONDATE = '' ";
            //load_qry = load_qry + " AND C.N1CLASS = 'TagDetector' AND D.N1TERMINATIONDATE = '' AND E.N1TERMINATIONDATE = '' ";
            //load_qry = load_qry + " AND G.MODULE_NAME IN ('FDMDetector') and G.PRJ_NAME='" + fgm_moduler.func_Class.project_Name + "' ";
            //load_qry = load_qry + " AND G.TERMINATIONDATE = ''";
            //load_qry = load_qry + " ORDER BY DETECTOR, VIEW_SEQ ";

            //data_adapter = new SqlDataAdapter(load_qry, fgm_moduler.func_Class.connString);
            //data_adapter.Fill(data_DataSet, "DataField");

            //GridDetector.DataSource = data_DataSet;
            //GridDetector.DataBind();
            //GridDetector.KeyFieldName = "OBID";
            //GridDetector.ExpandAll();
            
        }
        protected DataTable Srch_DataField(string m_name)
        {
            string load_qry;
            SqlDataAdapter data_adapter;
            DataSet ds_datafield = new DataSet();
            DataTable DetectorList = new DataTable();
            //string []enum_role;
            //enum_role = fgm_moduler.load_RoleList(module_Name);

            load_qry = "SELECT VIEW_INDEX, DB_NAME, ISFIXED, isnull(RTRIM(DESC_NAME),'') AS DESC_NAME ,VIEWER, SEL_TYPE, AUTH, VIEW_SEQ, isnull(UOM,'') AS UOM,REF_FROM ";
            load_qry = load_qry + ", GROUP_NAME, '' AS N3_VALUE, '' AS N3_OBID FROM N3SECTION ";
            load_qry = load_qry + " WHERE MODULE_NAME = '" + m_name + "' AND PRJ_NAME='" + SessionInfo.ProjectName + "'";
            load_qry = load_qry + " AND TERMINATIONDATE = '' ";
            load_qry = load_qry + " AND VIEWER =1  and DB_NAME != 'START_SEQ' AND DB_NAME != 'START_SUFFIX'  AND DB_NAME != 'ID_ZONE'  ";
            load_qry = load_qry + " ORDER BY AUTH DESC, VIEW_SEQ, SYSTEM_SEQ";

            data_adapter = new SqlDataAdapter(load_qry, fgm_moduler.func_Class.connString);
            data_adapter.Fill(ds_datafield, "DataField");


           return ds_datafield.Tables["DataField"];
        }
        protected DataTable Srch_DataField_Detector(string m_name)
        {
            string load_qry;
            SqlDataAdapter data_adapter;
            DataSet ds_datafield = new DataSet();
            DataTable DetectorList = new DataTable();

            load_qry = "SELECT C.OBID, C.N1NAME AS [N1NAME], C.OBID + '@' + DB_NAME+'@'+N1NAME AS [KEY], DB_NAME, isnull(RTRIM(DESC_NAME),'') AS DESC_NAME, '' AS N3_VALUE, '' AS N3_OBID ";
            load_qry = load_qry + " ,AUTH, VIEW_SEQ ";
            //load_qry = load_qry + " VIEWER, SEL_TYPE, AUTH, VIEW_SEQ, isnull(UOM,'') AS UOM, REF_FROM, GROUP_NAME ";
            load_qry = load_qry + " FROM N2REVGRP C ";
            load_qry = load_qry + " INNER JOIN N3RGRPRGRP R ON R.LEFTREL = C.OBID ";
            load_qry = load_qry + " CROSS JOIN N3SECTION G ";
            load_qry = load_qry + " WHERE C.N1TERMINATIONDATE = '' AND C.N1PROJECTNAME = '" + SessionInfo.ProjectName + "'  AND R.N1TERMINATIONDATE = '' ";
            load_qry = load_qry + " AND R.RIGHTREL = '" + Detector_obid + "' AND R.N1TERMINATIONDATE = ''  AND C.N1CLASS = 'TagDetector'  ";
            load_qry = load_qry + " AND G.MODULE_NAME = '" + m_name + "' AND G.PRJ_NAME = '"+ SessionInfo.ProjectName + "' AND G.TERMINATIONDATE ='' AND VIEWER=1";
            load_qry = load_qry + " ORDER BY N1NAME, VIEW_SEQ";


            data_adapter = new SqlDataAdapter(load_qry, fgm_moduler.func_Class.connString);
            data_adapter.Fill(ds_datafield, "DataField");

            return ds_datafield.Tables["DataField"];
        }
        string Detector_Group_cnt;
        protected void Fill_Record(ref DataTable dynamic_table, string obid)
        {
            string load_qry;
            SqlDataAdapter data_adapter;
            DataSet data_DataSet = new DataSet();
            
            string rev;
            string TagNo;
            string revdate;

            load_qry = "SELECT C.N1MAJORREVISION + C.N1MINORREVISION as Rev, C.OBID as TagID, isnull(C.N1NAME,'') as N1NAME, F.N1ATTRCLASSNAME, E.N1STRVALUE, C.N1CREATIONDATE, ";
            load_qry = load_qry + " C.N1CLASS,C.N1TERMINATIONDATE, RTrim(C.N1ORIGCLASSDEFUID) as N1ORIGCLASSDEFUID, D.OBID, isnull(G.ISFIXED,0) as FIXED, G.AUTH, C.N1LASTUPDATED, C.N1ISSUESTATE, G.SEL_TYPE,G.VIEWER ";
            load_qry = load_qry + " FROM N2REVGRP C ";
            load_qry = load_qry + " INNER JOIN N3OBJATTR D ON C.OBID = D.LEFTREL ";
            load_qry = load_qry + " INNER JOIN N2SPCATTR E ON D.RIGHTREL = E.OBID ";
            load_qry = load_qry + " INNER JOIN N2CLSATTR F ON E.N1ATTRCLASSOBID = F.OBID ";
            load_qry = load_qry + " INNER JOIN N3SECTION G ON G.DB_NAME = F.N1ATTRCLASSNAME ";
            load_qry = load_qry + " WHERE (C.OBID='" + obid + "') ";
            //load_qry = load_qry + " AND (C.N1PROJECTNAME='" + fgm_moduler.func_Class.project_Name + "') ";
            load_qry = load_qry + " AND (C.N1PROJECTNAME='"+ SessionInfo.ProjectName + "') AND C.N1TERMINATIONDATE = '' AND N1CLASS = 'TagGroupDetector' ";
            load_qry = load_qry + " AND D.N1TERMINATIONDATE = '' ";
            //load_qry = load_qry + " AND G.MODULE_NAME IN ('" + module_Name + "') and G.PRJ_NAME='" + fgm_moduler.func_Class.project_Name + "'";
            load_qry = load_qry + " AND G.MODULE_NAME IN ('FDM') and G.PRJ_NAME='" + SessionInfo.ProjectName + "'";
            load_qry = load_qry + " AND G.TERMINATIONDATE = '' ";
            //REPORT 조건 추가 (15.08.10)
            load_qry = load_qry + " AND G.REPORT = '0' ";

            string update_qry = "SELECT TOP 1 * FROM N2REVGRP";

            data_adapter = new SqlDataAdapter(load_qry, fgm_moduler.func_Class.connString);
            data_adapter.Fill(data_DataSet, "DataField");
            
            if (data_DataSet.Tables["DataField"].Rows.Count > 0)
            {
                rev = data_DataSet.Tables["DataField"].Rows[0]["Rev"].ToString();
                TagNo = data_DataSet.Tables["DataField"].Rows[0]["N1NAME"].ToString();
                revdate = data_DataSet.Tables["DataField"].Rows[0]["N1LASTUPDATED"].ToString();
                issue_state = data_DataSet.Tables["DataField"].Rows[0]["N1ISSUESTATE"].ToString();

                foreach (DataRow datafield_DataRow in dynamic_table.Select("AUTH = 'System'"))
                {
                    if (datafield_DataRow["DB_NAME"].ToString() == "REV")
                        datafield_DataRow["N3_VALUE"] = rev;
                    else if (datafield_DataRow["DB_NAME"].ToString() == "N1NAME")
                        datafield_DataRow["N3_VALUE"] = TagNo;
                    else if (datafield_DataRow["DB_NAME"].ToString() == "N1LASTUPDATED")
                        datafield_DataRow["N3_VALUE"] = revdate;
                }
            }
            foreach (DataRow data_DataRow in data_DataSet.Tables["DataField"].Rows)
            {
                if (data_DataRow["N1ATTRCLASSNAME"].ToString() == "CNT")
                    Detector_Group_cnt = data_DataRow["N1STRVALUE"].ToString();
                if (data_DataRow["N1ATTRCLASSNAME"].ToString() == "ID_UNIT")
                    Detector_unit = data_DataRow["N1STRVALUE"].ToString();
                if (data_DataRow["N1ATTRCLASSNAME"].ToString() == "DETECTOR_TYPE")
                {
                    Detector_type = data_DataRow["N1STRVALUE"].ToString();
                    Detector_type_HiddenField.Value = data_DataRow["N1STRVALUE"].ToString();
                }
                if (data_DataRow["N1ATTRCLASSNAME"].ToString() == "LOC")
                    Detector_loc = data_DataRow["N1STRVALUE"].ToString();
                //if (data_DataRow["N1ATTRCLASSNAME"].ToString() == "DESC_NAME")
                //    Detector_desc = data_DataRow["N1STRVALUE"].ToString();

                foreach (DataRow datafield_DataRow in dynamic_table.Select("DB_NAME = '" + data_DataRow["N1ATTRCLASSNAME"].ToString() + "'"))
                {
                    datafield_DataRow["N3_VALUE"] = data_DataRow["N1STRVALUE"];
                    datafield_DataRow["N3_OBID"] = data_DataRow["OBID"];
                }
            }
            GridGroupDetail.DataSource = dynamic_table;
            GridGroupDetail.DataBind();

            for (int i = 0; i < dynamic_table.Rows.Count; i++)
            {
                if (dynamic_table.Rows[i]["DB_NAME"].ToString() == "LOC")
                    Session["loc_cause"] = dynamic_table.Rows[i]["N3_VALUE"].ToString();

            }

            //Grid Disable (Issued)
            if (issue_state == "")
                GridGroupDetail.Enabled = true;
            else
                GridGroupDetail.Enabled = false;
            //GridGroupDetail.KeyFieldName = "DB_NAME";

            //Detector_Group_cnt = dynamic_table.Rows[11][11].ToString();
        }
        protected void Fill_Record_Detector(ref DataTable dynamic_table_detector, string obid)
        {
            string load_qry;
            SqlDataAdapter data_adapter;
            DataSet data_DataSet = new DataSet();

            //load_qry = "SELECT C.N1MAJORREVISION + C.N1MINORREVISION as Rev, C.OBID as TagID, isnull(C.N1NAME,'') as N1NAME, F.N1ATTRCLASSNAME, E.N1STRVALUE, C.N1CREATIONDATE, ";
            //load_qry = load_qry + " C.N1CLASS,C.N1TERMINATIONDATE, RTrim(C.N1ORIGCLASSDEFUID) as N1ORIGCLASSDEFUID, D.OBID, isnull(G.ISFIXED,0) as FIXED, G.AUTH, C.N1LASTUPDATED, C.N1ISSUESTATE, G.SEL_TYPE,G.VIEWER ";

            load_qry = " SELECT D.OBID, C.N1NAME AS [N1NAME], C.N1LIBRARYNO AS [SEQ], RELDEFID, E.N1STRVALUE, G.DB_NAME,G.DESC_NAME, F.N1ATTRCLASSNAME FROM N2REVGRP C ";
            load_qry = load_qry + " INNER JOIN N3RGRPRGRP R ON R.LEFTREL = C.OBID ";
            load_qry = load_qry + " INNER JOIN N3OBJATTR D ON C.OBID = D.LEFTREL ";
            load_qry = load_qry + " INNER JOIN N2SPCATTR E ON D.RIGHTREL = E.OBID ";
            load_qry = load_qry + " INNER JOIN N2CLSATTR F ON E.N1ATTRCLASSOBID = F.OBID  ";
            load_qry = load_qry + " INNER JOIN N3SECTION G ON G.DB_NAME = F.N1ATTRCLASSNAME ";
            load_qry = load_qry + " WHERE C.N1TERMINATIONDATE = '' AND C.N1PROJECTNAME = '" + fgm_moduler.func_Class.project_Name + "' ";
            load_qry = load_qry + " AND R.N1TERMINATIONDATE = '' AND R.RIGHTREL = '" + obid + "' AND R.N1TERMINATIONDATE = '' ";
            load_qry = load_qry + " AND C.N1CLASS = 'TagDetector' AND D.N1TERMINATIONDATE = '' AND E.N1TERMINATIONDATE = '' ";
            load_qry = load_qry + " AND G.MODULE_NAME IN ('FDMDetector') and G.PRJ_NAME='" + fgm_moduler.func_Class.project_Name + "' ";
            load_qry = load_qry + " AND G.TERMINATIONDATE = ''";
            load_qry = load_qry + " ORDER BY N1NAME, VIEW_SEQ ";


            data_adapter = new SqlDataAdapter(load_qry, fgm_moduler.func_Class.connString);
            data_adapter.Fill(data_DataSet, "DataField");

            foreach (DataRow data_DataRow in data_DataSet.Tables["DataField"].Rows)
            {
                foreach (DataRow datafield_DataRow in dynamic_table_detector.Select("N1NAME = '"+ data_DataRow["N1NAME"].ToString()+ "' AND DB_NAME = '" + data_DataRow["N1ATTRCLASSNAME"].ToString() + " '"))
                {
                    datafield_DataRow["N3_VALUE"] = data_DataRow["N1STRVALUE"];
                    datafield_DataRow["N3_OBID"] = data_DataRow["OBID"];
                }
            }

            GridDetector.Columns.Clear();
            GridDetector.AutoGenerateColumns = true;
            GridDetector.DataSource = dynamic_table_detector;
            GridDetector.DataBind();

            //Grid Disable (Issued)
            if (issue_state == "")
                GridDetector.Enabled = true;
            else
                GridDetector.Enabled = false;

            //Grid Column Setting
            GridDetector.Columns["KEY"].Visible = false;
            GridDetector.Columns["OBID"].Visible = false;
            GridDetector.Columns["N3_OBID"].Visible = false;
            GridDetector.Columns["DB_NAME"].Visible = false;
            GridDetector.GroupBy(GridDetector.Columns["N1NAME"]);
            GridViewDataColumn c1 = GridDetector.Columns["DESC_NAME"] as GridViewDataColumn;
            c1.ReadOnly = true;
            c1.EditFormSettings.Visible = DevExpress.Utils.DefaultBoolean.False;

            GridDetector.Columns["AUTH"].Visible = false;
            GridDetector.Columns["VIEW_SEQ"].Visible = false;
        }
        protected void Setup_Zone()
        {
            DataSet ds_zone = new DataSet();
            SqlDataAdapter data_adapter;
            string load_qry = "SELECT LOOKUPDETNAME, LOOKUPDETDESC FROM LOOKUPDET WHERE DEPENDENTPROJECT = '" + fgm_moduler.func_Class.project_Name + "' ";
            load_qry = load_qry + " AND LOOKUPNAME = 'ID_ZONE' AND TERMINATIONDATE = '' AND LOOKUPDETNAME <> 'COMMON' ORDER BY LOOKUPDETNAME";
            data_adapter = new SqlDataAdapter(load_qry, fgm_moduler.func_Class.admin_connString);
            data_adapter.Fill(ds_zone);

            ddl_zone.DataSource = ds_zone.Tables[0];
            ddl_zone.DataTextField = "LOOKUPDETNAME";
            ddl_zone.DataValueField = "LOOKUPDETNAME";
            ddl_zone.DataTextFormatString = "Zone {0}";
            ddl_zone.DataBind();
        }


        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="obid"></param>
        protected void makeViewGridEffect(string obid)
        {
            dynamic_table_effect = new DataTable();
            //DataTable dt_interface = new DataTable();
            SqlDataAdapter data_adapter;
            //string load_qry = "SELECT OBID, SYS_INTERFACE, N1NAME AS [TagNo], DESC_NAME as [Description],  ";
            //load_qry = load_qry + " LOC as [Location], ACTION_STATUS as [ActionStatus], SIGNAL_VOTING FROM TBLEFFECT ";
            //load_qry = load_qry + " WHERE N1TERMINATIONDATE = '' AND PARENT_REL = '" + obid + "' ";
            //load_qry = load_qry + "  ORDER BY N1NAME";
            
            //string load_qry = "SELECT D.OBID, (CASE WHEN E.VOTING='V' THEN 'Voting' WHEN E.VOTING='S' THEN 'Single' ELSE E.VOTING END) AS VOTING, E.VOTING_CNT, ('Effect ' + E.EFFECTTYPE) AS EFFECTTYPE, F.SYS_INTERFACE, F.N1NAME AS [TagNo], F.DESC_NAME AS [Description], F.LOC AS [Location], ";
            //load_qry = load_qry + " F.ACTION_STATUS as [ActionStatus] FROM N2REVGRP C ";
            //load_qry = load_qry + " INNER JOIN N3OBJATTR D ON D.LEFTREL = C.OBID ";
            //load_qry = load_qry + " INNER JOIN TBLEFFECT E ON E.OBID = D.RIGHTREL ";
            //load_qry = load_qry + " INNER JOIN TBLEFFECT_SETUP F ON F.OBID = E.RIGHTREL ";
            //load_qry = load_qry + " WHERE C.N1PROJECTNAME = '" + fgm_moduler.func_Class.project_Name + "' AND C.N1TERMINATIONDATE = '' ";
            //load_qry = load_qry + " AND C.N1CLASS = 'TagGroupDetector' AND D.N1TERMINATIONDATE = '' AND E.N1TERMINATIONDATE = '' AND F.N1TERMINATIONDATE = '' ";
            //load_qry = load_qry + " AND C.OBID = '" + obid + "' ORDER BY [TagNo] ";

            string load_qry = "SELECT D.OBID, F.VOTING, F.VOTING_CNT, F.ALARM_LEVEL, ";
            load_qry = load_qry + " CASE WHEN F.VOTING = 'Voting' THEN F.VOTING + ' : ' + F.VOTING_CNT + ' oo ' + '" + Detector_Group_cnt + "' + ' (' + F.ALARM_LEVEL + ') '";
            load_qry = load_qry + " ELSE F.VOTING + ' (' + F.ALARM_LEVEL + ') ' END AS EFFECT_GROUP, "; 
            load_qry = load_qry + " E.N1NAME AS [TagNo], F.DESC_NAME, F.LOC1, F.LOC2, F.OBID AS OBID_TEMPLATE";
            load_qry = load_qry + " FROM N2REVGRP C ";
            load_qry = load_qry + " INNER JOIN N3OBJATTR D ON D.LEFTREL = C.OBID ";
            load_qry = load_qry + " INNER JOIN TBLEFFECT_NEW E ON E.OBID = D.RIGHTREL ";
            load_qry = load_qry + " INNER JOIN TBLEFFECT_SETUP_NEW F ON F.OBID = E.RIGHTREL ";
            load_qry = load_qry + " WHERE C.N1PROJECTNAME = '" + fgm_moduler.func_Class.project_Name + "' AND C.N1TERMINATIONDATE = '' ";
            load_qry = load_qry + " AND C.N1CLASS = 'TagGroupDetector' AND D.N1TERMINATIONDATE = '' AND E.N1TERMINATIONDATE = '' AND F.N1TERMINATIONDATE = '' ";
            load_qry = load_qry + " AND C.OBID = '" + obid + "' ORDER BY [TagNo] ";

            data_adapter = new SqlDataAdapter(load_qry, fgm_moduler.func_Class.connString);
            data_adapter.Fill(dynamic_table_effect);

            //for (int i = 0; i < dynamic_table_effect.Rows.Count; i++)
            //{
            //    if (dynamic_table_effect.Rows[i]["VOTING"].ToString() == "Voting")
            //        dynamic_table_effect.Rows[i]["VOTING"] = "Voting : " + dynamic_table_effect.Rows[i]["VOTING_CNT"] + "oo" + Detector_Group_cnt;
            //}

            gv1temp.DataSource = dynamic_table_effect;
            gv1temp.DataBind();

            //Grid Enable/Disable (Issued)
            if (issue_state == "")
                gv1temp.Enabled = true;
            else
                gv1temp.Enabled = false;


            gv1temp.ExpandAll();
        }





        protected void GridGroupDetector_OnRowInserting(object sender, ASPxDataInsertingEventArgs e)
        {
            SqlCommand ins_cmd = new SqlCommand();
            XmlDocument tag_Doc = new XmlDocument();
            XmlElement tag_Ele;
            XmlElement data_Ele;
            int i;
            ASPxTextBox new_textbox;
            ASPxComboBox new_combobox;
            ASPxMemo new_memo;
            string grouptag_Name;
            string grouptag_ID;
            string tag_Name;
            string tag_Seq="";
            string tag_ID;
            int tag_Count=0;

            tag_Ele = tag_Doc.CreateElement("GroupDetector");
            tag_Doc.AppendChild(tag_Ele);

            //ASPxFormLayout editform_GroupDetector = new ASPxFormLayout();
            //editform_GroupDetector.ID = "editform_GroupDetector";
            ASPxFormLayout editform_GroupDetector = GridGroupDetector.FindEditFormTemplateControl("editform_GroupDetector") as ASPxFormLayout;
            int cnt = editform_GroupDetector.Items.Count;
            DataTable dt = Srch_DataField("FDM");

            string db_name = "";
            string auth = "";
            foreach (DataRow dr in dt.Select("AUTH <> 'System'", "VIEW_SEQ"))
            {
                string value="";
                db_name = dr["DB_NAME"].ToString();
                auth = dr["AUTH"].ToString();
                object item = editform_GroupDetector.FindNestedControlByFieldName(db_name);
                if (item.GetType() == typeof(ASPxTextBox))
                {
                    new_textbox = (ASPxTextBox)item;
                    if(new_textbox.Text != "")
                    {
                        data_Ele = tag_Doc.CreateElement("Data");
                        data_Ele.SetAttribute("Name", db_name);
                        value = new_textbox.Text;
                        data_Ele.InnerText = new_textbox.Text;
                        tag_Ele.AppendChild(data_Ele);
                    }
                }
                else if (item.GetType() == typeof(ASPxComboBox))
                {
                    new_combobox = (ASPxComboBox)item;
                    if (new_combobox.Text != "")
                    {
                        data_Ele = tag_Doc.CreateElement("Data");
                        data_Ele.SetAttribute("Name", db_name);
                        value = new_combobox.Text;
                        data_Ele.InnerText = new_combobox.Text;
                        tag_Ele.AppendChild(data_Ele);
                    }
                }
                else if (item.GetType() == typeof(ASPxMemo))
                {
                    new_memo = (ASPxMemo)item;
                    if (new_memo.Text != "")
                    {
                        data_Ele = tag_Doc.CreateElement("Data");
                        data_Ele.SetAttribute("Name", db_name);
                        value = new_memo.Text;
                        data_Ele.InnerText = new_memo.Text;
                        tag_Ele.AppendChild(data_Ele);
                    }
                }
                if (auth == "Manager" && value == "")
                {
                    (sender as ASPxGridView).JSProperties["cpConfirmationMessage"] = "필수값을 입력하세요";
                    e.Cancel = true;
                    return;
                }
                else if (db_name == "CNT")
                {
                    try
                    {
                        int chk_count = int.Parse(value);
                        if (chk_count < 1)
                            throw new Exception("");
                    }
                    catch
                    {
                        (sender as ASPxGridView).JSProperties["cpConfirmationMessage"] = "Count : Error";
                        e.Cancel = true;
                        return;
                    }
                }
            }


            grouptag_Name = fgm_moduler.Validate_tagName(ref tag_Ele, env_config, module_Name, "");
            if (grouptag_Name.Contains("Err"))
            {
                (sender as ASPxGridView).JSProperties["cpConfirmationMessage"] = "Tag No : Error";
                e.Cancel = true;
                return;
            }

            ins_cmd.Connection = fgm_moduler.func_Class.conn;
            try
            {
                //Group Detector Tag Insert
                grouptag_ID = fgm_moduler.Save_REVGRP(ref ins_cmd, env_config, "TagGroupDetector", grouptag_Name, tag_Ele, "*", 0, zone);
                Detector_obid = grouptag_ID;
                if (grouptag_ID.Contains("Err"))
                {
                    (sender as ASPxGridView).JSProperties["cpConfirmationMessage"] = "Tag No : Duplication";
                    e.Cancel = true;
                    //GridGroupDetector.CancelEdit();
                    return;
                }
                fgm_moduler.Add_Attr(ref ins_cmd, "ID_ZONE", "text", zone, grouptag_ID);
                foreach (XmlElement Ele in tag_Ele.ChildNodes)
                {
                    fgm_moduler.Add_Attr(ref ins_cmd, Ele.GetAttribute("Name"), "text", Ele.InnerText, grouptag_ID);
                    if (Ele.GetAttribute("Name").ToString() == "CNT")
                        tag_Count = Int32.Parse(Ele.InnerText.ToString());
                    if (Ele.GetAttribute("Name").ToString() == "ID_UNIT")
                        Detector_unit = Ele.InnerText.ToString();
                    if (Ele.GetAttribute("Name").ToString() == "DETECTOR_TYPE")
                        Detector_type = Ele.InnerText.ToString();
                    if (Ele.GetAttribute("Name").ToString() == "LOC")
                        Detector_loc = Ele.InnerText.ToString();
                    if (Ele.GetAttribute("Name").ToString() == "CNT")
                        Detector_cnt = Ele.InnerText.ToString();
                    //if (Ele.GetAttribute("Name").ToString() == "DESC_NAME")
                    //    Detector_desc = Ele.InnerText.ToString();
                }

                //Detector Tag Insert (Count 값으로 자동생성)
                //Group Detector Dfactor 삽입추가(15.10.04) ex) Unit-Type c
                for (i = 0; i < tag_Count; i++)
                {
                    Add_Detector(grouptag_ID, grouptag_Name, tag_Ele);
                }        
            }
            catch (Exception ex)
            {
                (sender as ASPxGridView).JSProperties["cpConfirmationMessage"] = ex.ToString();
                fgm_moduler.func_Class.conn.Close();
                e.Cancel = true;
                GridGroupDetector.CancelEdit();
                return;
            }
            e.Cancel = true;
            GridGroupDetector.CancelEdit();           
            GridGroupDetector.JSProperties["cpNewWindowUrl"] = "FDMQNA.aspx?TYPE=" + Detector_type + "&OBID=" + grouptag_ID + "&UNIT=" + Detector_unit + "&LOC=" + Detector_loc + "&ZONE=" + zone + "&CNT=" + tag_Count;
            
        }
        protected string get_DetectorName(XmlElement tag_Ele, XmlDocument env_config)
        {
            string names="";
            foreach (XmlElement xe in env_config.SelectNodes("//GENERAL/TAG_NO/GROUP_DETECTOR/FACTOR[@D_FACTOR='TRUE']"))
            {
                string db_name = xe.GetAttribute("NAME");
                string seperator = xe.GetAttribute("SEPERATOR");
                names = names + seperator;
                names = names + tag_Ele.SelectSingleNode("./Data[@Name ='" + db_name + "']").InnerText;
            }
            return names;
        }
        protected string get_DetectorSeq(string tag_Name, ref string len)
        {
            SqlDataAdapter data_adapter;
            DataTable dt = new DataTable();
            string seq="";
            string seq_qry = " SELECT TOP 1 N1LIBRARYNO AS SEQ, LEN(N1LIBRARYNO) AS LEN ";
            seq_qry = seq_qry + " FROM N2REVGRP WHERE N1PROJECTNAME = '" + SessionInfo.ProjectName + "' AND N1CLASS = 'TagDetector' ";
            seq_qry = seq_qry + " AND N1TERMINATIONDATE = '' AND N1NAME LIKE '" + tag_Name + "-%'";
            seq_qry = seq_qry + " ORDER BY N1NAME DESC ";

            data_adapter = new SqlDataAdapter(seq_qry, fgm_moduler.func_Class.connString);
            data_adapter.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                seq = dt.Rows[0]["SEQ"].ToString();
                for (int i = 0; i < Int32.Parse(dt.Rows[0]["LEN"].ToString()); i++)
                {
                    len = len + "0";
                }
                len = "{0:" + len + "}";
                return seq;
            }
            else
            {
                len = "{0:0000}";
                return "0";
            }
        }

        protected void GridGroupDetector_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            dv = ds.Tables[0].DefaultView;
            dv.RowFilter = "OBID = '" + GridGroupDetector.GetRowValues(GridGroupDetector.FocusedRowIndex, "OBID").ToString() + "'";

        }

        protected void GridGroupDetector_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (e.GetValue("N1ISSUESTATE") == null)
            {
                e.Row.Enabled = true;
            }
            else if (e.GetValue("N1ISSUESTATE").ToString() != "")
            {
                e.Row.Enabled = false;
            }
        }

        protected void GridGroupDetail_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (e.Row.Attributes["GROUP_NAME"] == group_Name)
                return;
            else
            {
                group_Name = e.Row.Attributes["GROUP_NAME"];          
            }
        }

        protected void GridGroupDetail_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (e.KeyValue != null)
            {
                if (e.GetValue("AUTH").ToString() == "System")
                {
                    e.Row.BackColor = Color.FromName("#E6E6FA");
                    e.Row.ForeColor = Color.Black;                    
                }
                else
                {

                }
            }
        }
        
        protected DataTable Srch_DropDown(string db_name)
        {
            string load_qry;

            FGMModule fgm_moduler;
            fgm_moduler = new FGMModule("FDM", SessionInfo.ProjectName, "TagGroupDetector");
            SqlDataAdapter data_adapter;
            DataSet ds_datafield = new DataSet();

            load_qry = "SELECT LOOKUPDETNAME, LOOKUPDETDESC FROM LOOKUPDET ";
            load_qry = load_qry + " WHERE LOOKUPNAME = '" + db_name + "' AND DEPENDENTPROJECT = '" + SessionInfo.ProjectName + "' AND TERMINATIONDATE = '' ";
            load_qry = load_qry + " ORDER BY LOOKUPDETDESC ";

            data_adapter = new SqlDataAdapter(load_qry, fgm_moduler.func_Class.admin_connString);
            data_adapter.Fill(ds_datafield, "DropDownItem");


            return ds_datafield.Tables["DropDownItem"];
        }

        protected void GridGroupDetail_RowUpdating(object sender, ASPxDataUpdatingEventArgs e)
        {
            string property = e.NewValues["N3_VALUE"].ToString();

            ASPxGridView gv = sender as ASPxGridView;
            e.Cancel = true;
            gv.CancelEdit();
        }
        protected void GridGroupDetail_BatchUpdate(object sender, ASPxDataBatchUpdateEventArgs e)
        {
            SqlCommand up_cmd = new SqlCommand();
            up_cmd.Connection = fgm_moduler.func_Class.conn;

            bool success = false;
            string grouptag_Name;
            
            XmlDocument tag_Doc = new XmlDocument();
            XmlElement tag_Ele;
            XmlElement data_Ele;

            tag_Ele = tag_Doc.CreateElement("Detector");
            tag_Ele.SetAttribute("Name", tagName.Value);
            tag_Ele.SetAttribute("OBID", Detector_obid);
            tag_Doc.AppendChild(tag_Ele);

            DataTable dt1 = Srch_DataField("FDM");
            DataTable dt = dynamic_table;
            dt.Columns.Add("v_state");
            dt.Columns.Add("old_value");

            //dt.Columns.Add("old_value");

            string db_name = "";
            string auth = "";
            foreach (var args in e.UpdateValues)
            { 
                db_name = args.Keys[0].ToString();
                DataRow dr = dt.Select("DB_NAME = '" + db_name + "'")[0];
                if(dr["AUTH"].ToString() != "System")
                {
                    string newValue = "";
                    if (args.NewValues["N3_VALUE"] != null)
                        newValue = args.NewValues["N3_VALUE"].ToString();
                    dr["old_value"] = args.OldValues["N3_VALUE"].ToString();
                    dr["N3_VALUE"] = newValue;
                    dr["v_state"] = "Updated";
                }
            }

            foreach (DataRow dr in dt.Select("AUTH <> 'System'", "VIEW_SEQ"))
            {
                //db_name = dr["DB_NAME"].ToString();
                //auth = dr["AUTH"].ToString();

                data_Ele = tag_Doc.CreateElement("Data");
                data_Ele.SetAttribute("Name", dr["DB_NAME"].ToString());
                data_Ele.InnerText = dr["N3_VALUE"].ToString();
                if (dr["v_state"].ToString() == "Updated")
                {
                    data_Ele.SetAttribute("v_state", "Updated");
                    data_Ele.SetAttribute("old_value", dr["old_value"].ToString());
                }
                tag_Ele.AppendChild(data_Ele);
            }
            grouptag_Name = fgm_moduler.Validate_tagName(ref tag_Ele, env_config, module_Name, "");
            if (grouptag_Name.Contains("Err"))
            {
                (sender as ASPxGridView).JSProperties["cpConfirmationMessage"] = "Tag No : Error - "+group_Name;
                return;
            }
            if (grouptag_Name != tag_Ele.GetAttribute("oldValue"))
            {
                fgm_moduler.Update_N2REVGRP(up_cmd, "N1NAME", grouptag_Name, Detector_obid);
                tagName.Value = grouptag_Name;
            }

            try
            {
                foreach (XmlElement Ele in tag_Ele.SelectNodes("./Data[@v_state = 'Updated']"))
                {
                    //Count Handling
                    if (Ele.GetAttribute("Name").ToString() == "CNT")
                    {                        
                        int prev_cnt; int cur_cnt;
                        try
                        {
                            prev_cnt = Convert.ToInt32(Ele.Attributes["old_value"].Value);
                            cur_cnt = Convert.ToInt32(Ele.InnerText);
                        }
                        catch (Exception ex)
                        {
                            (sender as ASPxGridView).JSProperties["cpConfirmationMessage"] = "CNT : Invalid value";
                            success = false;
                            continue;
                        }

                        if (prev_cnt < cur_cnt)
                        {
                            int add = cur_cnt - prev_cnt;

                            for (int i = 0; i < add; i++)
                            {
                                Add_Detector(Detector_obid, grouptag_Name,tag_Ele);
                            }

                        }
                        else if (prev_cnt == cur_cnt)
                        {
                            success = false;
                            continue;
                        }
                        else
                        {
                            int del = prev_cnt - cur_cnt;

                            for (int i = 0; i < del; i++)
                            {
                                Del_Detector(Detector_obid);
                            }
                        }
                    }
                    fgm_moduler.Add_Attr(ref up_cmd, Ele.GetAttribute("Name"), "text", Ele.InnerText, Detector_obid);
                    success = true;
                }
                if (e.UpdateValues.Count > 0)
                {
                    if (success)
                    {
                        fgm_moduler.Update_N2REVGRP(up_cmd, "N1LASTUPDATED", "date", Detector_obid);
                        fgm_moduler.Update_N2REVGRP(up_cmd, "", "version", Detector_obid);
                        
                    }
                }
                makeViewGridGroupDetail(Detector_obid);
            }
            catch (Exception ex)
            {
                (sender as ASPxGridView).JSProperties["cpConfirmationMessage"] = ex.ToString();
            }
            e.Handled = true;
        }        
        protected void Add_Detector(string grouptag_OBID, string grouptag_Name, XmlElement grouptag_Ele)
        {
            SqlCommand Add_cmd = new SqlCommand();
            Add_cmd.Connection = fgm_moduler.func_Class.conn;

            //string unit = ""; string type = ""; string group_name = ""; 
            string tag_Seq = "";
            string tag_Seq_Len = "";
            string tag_Name = "";
            string tag_ID = "";

            tag_Name = get_DetectorName(grouptag_Ele, env_config);
            tag_Seq = get_DetectorSeq(tag_Name, ref tag_Seq_Len);
            if (true)
            {
                tag_Seq = string.Format(tag_Seq_Len, Int32.Parse(tag_Seq) + 1);
            }
            else
            {
                
            }
            
            //tag_Seq = string.Format(tag_Seq_Len, Int32.Parse(tag_Seq) + 1);
            tag_ID = fgm_moduler.Create_GUID();
            if (SessionInfo.ProjectName == "INAS")
            {
             //   fgm_moduler.Add_N2REVGRP(Add_cmd, env_config, "TagDetector", tag_ID, tag_Name + tag_Seq, zone, tag_Seq);
            }
            else
                fgm_moduler.Add_N2REVGRP(Add_cmd, env_config, "TagDetector", tag_ID, tag_Name + "-" + tag_Seq, zone, tag_Seq);

            //Group Detector Dfactor 삽입추가(15.10.04) ex) Unit-Type c 
            foreach (XmlElement xe in env_config.SelectNodes("//GENERAL/TAG_NO/GROUP_DETECTOR/FACTOR[@D_FACTOR='TRUE']"))
            {
                string attr_name = xe.GetAttribute("NAME");
                string attr_value = grouptag_Ele.SelectSingleNode("./Data[@Name ='" + attr_name + "']").InnerText;
                fgm_moduler.Add_Attr(ref Add_cmd, attr_name, "text", attr_value, tag_ID);
            }

            fgm_moduler.Add_Attr(ref Add_cmd, "SEQ", "text", tag_Seq, tag_ID);
            fgm_moduler.Add_Attr(ref Add_cmd, "REF_N1NAME", "text", grouptag_Name, tag_ID);
            fgm_moduler.Save_N3RGRPRRP(ref Add_cmd, grouptag_OBID, "", tag_ID, "TagGroupDetector", "", grouptag_Name);


        }
        protected void Del_Detector(string groupOBID)
        {
            SqlDataAdapter data_adapter;
            DataTable dt = new DataTable();
            SqlCommand del_cmd = new SqlCommand();
            del_cmd.Connection = fgm_moduler.func_Class.conn;
          
            string qry = "SELECT TOP 1 C.OBID FROM N2REVGRP C ";
            qry = qry + " INNER JOIN N3RGRPRGRP R ON R.LEFTREL = C.OBID ";
            qry = qry + " WHERE C.N1PROJECTNAME = '" + fgm_moduler.func_Class.project_Name + "' AND R.RIGHTREL = '" + groupOBID + "' ";
            qry = qry + " AND C.N1CLASS = 'TagDetector' AND R.N1TERMINATIONDATE = '' AND C.N1TERMINATIONDATE ='' ";
            qry = qry + " ORDER BY N1LIBRARYNO DESC ";

            data_adapter = new SqlDataAdapter(qry, fgm_moduler.func_Class.connString);
            data_adapter.Fill(dt);
            string max_seq_obid = dt.Rows[0][0].ToString();

            string d_qry = "UPDATE N3RGRPRGRP SET N1TERMINATIONDATE = REPLACE(REPLACE(CONVERT(varchar(23), GETDATE(), 121), '-','/'),'  ', '-'), ";
            d_qry = d_qry + "N1TERMINATIONSESID = '" + SessionInfo.UserName + "' WHERE LEFTREL = '" + max_seq_obid + "' AND RIGHTREL = '" + groupOBID + "'";
            d_qry = d_qry + " AND N1TERMINATIONDATE = '' ";
            del_cmd.CommandText = d_qry;
            del_cmd.ExecuteNonQuery();

            fgm_moduler.Update_N2REVGRP(del_cmd, "N1TERMINATIONDATE", "date", max_seq_obid);
        }


        protected void GridEffectList_CustomJSProperties(object sender, CustomJSPropertiesEventArgs e)
        {
            ASPxGridView gv_effect = sender as ASPxGridView;
            Hashtable selectedRowTable = new Hashtable();
            for (int i = 0; i < gv_effect.VisibleRowCount; i++)
            {
                DataRow dr = gv_effect.GetDataRow(i);
                if(!selectedRowTable.ContainsKey(dr["OBID"].ToString()))
                    selectedRowTable.Add(dr["OBID"].ToString(), string.Format("{0} {1}", dr["TagNo"].ToString(), dr["Location"].ToString()));
            }

            e.Properties["cpgetTokenname"] = selectedRowTable;
        }

        public int effect_cnt = 0;

        protected void CallbackPanel1_Callback(object sender, CallbackEventArgsBase e)
        {
            if (e.Parameter == "Insert_Effect")
            {
                effect_cnt++;
                ASPxDropDownEdit ddl1 = new ASPxDropDownEdit();
                ddl1.ID = "ddlRunTime-" + effect_cnt.ToString();
                ddl1.Width = Unit.Percentage(99);
                ddl1.NullText = "Effect-" + effect_cnt.ToString();

                //DropDownTemplate ddeTemplate = new DropDownTemplate();

                Control cnr = LoadControl("./EffectGrid.ascx?zone=FZ-01");
                cnr.ID = "cnr-" + effect_cnt.ToString();
                cnr.Page.Response.Write("asdf");
                //ddl1.DropDownWindowTemplate = (ITemplate)cnr;

                gv1temp.Controls.Add(cnr);
                
            }
        }

        protected void GridDetector_BatchUpdate(object sender, ASPxDataBatchUpdateEventArgs e)
        {
            SqlCommand up_cmd = new SqlCommand();
            SqlCommand chk_cmd = new SqlCommand();
            SqlDataReader chk_reader;
            bool success = false;
            bool g_success = false;
            up_cmd.Connection = fgm_moduler.func_Class.conn;
            chk_cmd.Connection = fgm_moduler.func_Class.conn;



            XmlDocument tag_Doc = new XmlDocument();
            XmlElement root_Ele = tag_Doc.CreateElement("UpdateTags");
            tag_Doc.AppendChild(root_Ele);
            XmlElement tag_Ele = null ;
            XmlElement data_Ele;
            DataTable dt = dynamic_table_detector;
            dt.Columns.Add("v_state");
            dt.Columns.Add("old_value");
            
            foreach (var args in e.UpdateValues)
            {
                string[] split = args.Keys[0].ToString().Split('@');
                string detector_obid = split[0];
                string db_name = split[1];
                string detector_name = split[2];

                DataRow dr = dt.Select("OBID = '" + detector_obid + "' AND DB_NAME ='" + db_name + "'")[0];
                //if (dr["AUTH"].ToString() != "System")
                //{
                    string newValue = "";
                    if (args.NewValues["N3_VALUE"] != null)
                        newValue = args.NewValues["N3_VALUE"].ToString();
                    dr["old_value"] = args.OldValues["N3_VALUE"].ToString();
                    dr["N3_VALUE"] = newValue;
                    dr["v_state"] = "Updated";
                //}
            }

            string tag_obid = "";
            string tag_name = "";
            //XML Document 생성
            foreach (DataRow dr in dt.Select("", "N1NAME, VIEW_SEQ"))
            {
                if (tag_obid != dr["OBID"].ToString())
                {
                    tag_obid = dr["OBID"].ToString();
                    tag_name = dr["N1NAME"].ToString();
                    tag_Ele = tag_Doc.CreateElement("Detector");
                    tag_Ele.SetAttribute("Name", tag_name);
                    tag_Ele.SetAttribute("OBID", tag_obid);
                    root_Ele.AppendChild(tag_Ele);
                }

                data_Ele = tag_Doc.CreateElement("Data");
                data_Ele.SetAttribute("Name", dr["DB_NAME"].ToString());
                data_Ele.InnerText = dr["N3_VALUE"].ToString();
                if (dr["v_state"].ToString() == "Updated")
                {
                    data_Ele.SetAttribute("v_state", "Updated");
                    data_Ele.SetAttribute("old_value", dr["old_value"].ToString());
                }
                tag_Ele.AppendChild(data_Ele);
            }

            try
            {
                tag_obid = "";
                tag_name = "";
                foreach (XmlElement detector_Ele in tag_Doc.SelectNodes("//Detector"))
                {
                    success = false;
                    XmlElement Ele = detector_Ele;
                    tag_name = fgm_moduler.Validate_tagName(ref Ele, env_config, "FDMDetector", "");
                    tag_obid = detector_Ele.GetAttribute("OBID");
                    if (tag_name.Contains("Err"))
                    {
                        (sender as ASPxGridView).JSProperties["cpConfirmationMessage"] = "Tag No : Error - " + tag_name;
                        return;
                    }

                    if (Ele.GetAttribute("oldValue") != "")
                    {
                        string v_sql = "SELECT N1NAME FROM N2REVGRP WHERE N1NAME = '" + tag_name + "' AND N1TERMINATIONDATE = '' ";
                        v_sql = v_sql + "AND N1CLASS = 'TagDetector' AND N1PROJECTNAME = '" + SessionInfo.ProjectName + "' ";

                        chk_cmd.CommandText = v_sql;
                        chk_reader = chk_cmd.ExecuteReader();
                        if (chk_reader.Read())
                        {
                            //Tag No. 중복검사
                            (sender as ASPxGridView).JSProperties["cpConfirmationMessage"] = "SEQ : Duplicate";
                            //e.Handled = true;
                            chk_reader.Close();
                            break;
                        }
                        else
                        {
                            //TagNo Change
                            chk_reader.Close();
                            fgm_moduler.Update_N2REVGRP(up_cmd, "N1NAME", tag_name, tag_obid);
                        }
                    }

                    foreach (XmlElement detector_Data_Ele in detector_Ele.SelectNodes("./Data[@v_state = 'Updated']"))
                    {
                        fgm_moduler.Add_Attr(ref up_cmd, detector_Data_Ele.GetAttribute("Name"), "text", detector_Data_Ele.InnerText, tag_obid);

                        if (detector_Data_Ele.GetAttribute("Name") == "SEQ")
                        {
                            fgm_moduler.Update_N2REVGRP(up_cmd, "N1LIBRARYNO", detector_Data_Ele.InnerText, tag_obid);
                        }

                        success = true;
                        g_success = true;
                    }
                    if (success)
                    {
                        fgm_moduler.Update_N2REVGRP(up_cmd, "N1LASTUPDATED", "date", tag_obid);
                    }
                }

                //Group Detector 수정날짜 변경
                if (e.UpdateValues.Count > 0)
                {
                    if (g_success)
                    {
                        fgm_moduler.Update_N2REVGRP(up_cmd, "N1LASTUPDATED", "date", Detector_obid);
                        fgm_moduler.Update_N2REVGRP(up_cmd, "", "version", Detector_obid);
                        makeViewGridGroupDetail(Detector_obid);
                    }
                }
            }
            catch (Exception ex)
            {
                (sender as ASPxGridView).JSProperties["cpConfirmationMessage"] = ex.ToString();
            }
            e.Handled = true;

        }

        protected void btn_nested_Click(object sender, EventArgs e)
        {
            string confirmValue = Request.Form["confirm_value"];

            if (confirmValue == "true")
            {
                //Detector Termination
                SqlDataAdapter data_adapter;
                DataTable dt = new DataTable();

                ASPxButton ab = (ASPxButton)sender;
                string del_detector_name = ab.Text;
                string del_detector_obid = "";

                string qry = "SELECT OBID FROM N2REVGRP WHERE N1PROJECTNAME = '" + fgm_moduler.func_Class.project_Name + "' AND N1CLASS = 'TagDetector' AND ";
                qry = qry + " N1NAME = '" + del_detector_name + "' AND ";
                qry = qry + " N1TERMINATIONDATE = '' AND OBID IN (SELECT LEFTREL FROM N3RGRPRGRP WHERE RIGHTREL = '" + Detector_obid + "' AND N1TERMINATIONDATE = '') ";

                data_adapter = new SqlDataAdapter(qry, fgm_moduler.func_Class.connString);
                data_adapter.Fill(dt);
                if (dt.Rows.Count > 0)
                    del_detector_obid = dt.Rows[0]["OBID"].ToString();
                else
                {
                    Response.Write("<script language='javascript'>alert('Delete Error')</script>");
                    return;
                }

                try
                {
                    SqlCommand del_cmd = new SqlCommand();
                    del_cmd.Connection = fgm_moduler.func_Class.conn;

                    //N3OBJATTR Termination
                    qry = " UPDATE N3OBJATTR SET N1TERMINATIONDATE = REPLACE(REPLACE(CONVERT(varchar(23), GETDATE(), 121), '-','/'),'  ', '-'),";
                    qry = qry + "N1TERMINATIONSESID = '" + SessionInfo.UserID + "' ";
                    qry = qry + "WHERE N1PROJECTNAME = '" + fgm_moduler.func_Class.project_Name + "' AND N1TERMINATIONDATE = '' AND  ";
                    qry = qry + "LEFTREL = '" + del_detector_obid + "'";

                    del_cmd.CommandText = qry;
                    del_cmd.ExecuteNonQuery();

                    //N3RGRPRGRP Termination
                    qry = " UPDATE N3RGRPRGRP SET N1TERMINATIONDATE = REPLACE(REPLACE(CONVERT(varchar(23), GETDATE(), 121), '-','/'),'  ', '-'),";
                    qry = qry + "N1TERMINATIONSESID = '" + SessionInfo.UserID + "' ";
                    qry = qry + "WHERE LEFTREL = '" + del_detector_obid + "'";

                    del_cmd.CommandText = qry;
                    del_cmd.ExecuteNonQuery();

                    //N2REVGRP Termination
                    qry = " UPDATE N2REVGRP SET N1TERMINATIONDATE = REPLACE(REPLACE(CONVERT(varchar(23), GETDATE(), 121), '-','/'),'  ', '-'),";
                    qry = qry + "N1TERMINATIONSESID = '" + SessionInfo.UserID + "' ";
                    qry = qry + "WHERE N1PROJECTNAME = '" + fgm_moduler.func_Class.project_Name + "' AND ";
                    qry = qry + "OBID = '" + del_detector_obid + "'";

                    del_cmd.CommandText = qry;
                    del_cmd.ExecuteNonQuery();

                    //Group Detector Count 값 Update
                    SqlCommand chg_cmd = new SqlCommand();
                    chg_cmd.Connection = fgm_moduler.func_Class.conn;
                    int group_tag_cnt = Convert.ToInt32(GridGroupDetail.GetRowValuesByKeyValue("CNT", "N3_VALUE").ToString());
                    group_tag_cnt = group_tag_cnt - 1;

                    fgm_moduler.Add_Attr(ref chg_cmd, "CNT", "text", group_tag_cnt.ToString(), Detector_obid);
                    fgm_moduler.Update_N2REVGRP(chg_cmd, "N1LASTUPDATED", "date", Detector_obid);
                    fgm_moduler.Update_N2REVGRP(chg_cmd, "", "version", Detector_obid);


                    //Reload
                    makeViewGridGroupDetail(Detector_obid);
                    makeViewGridDetectorDetail(Detector_obid);
                }
                catch (Exception ex)
                {
                    Response.Write(ex.Message.ToString());
                }
                
            }
            else
            {
                return;
            }
        }
       
        protected void GridDetector_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            e.Row.Cells[0].CssClass = "GroupCell";
            e.Row.Height = Unit.Pixel(8);

        }
        protected void GridDetector_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (e.KeyValue != null)
            {
                if (e.GetValue("AUTH").ToString() == "System")
                {
                    e.Row.Visible = false;
                }  
            }
        }

        protected void gv1temp_RowUpdating(object sender, ASPxDataUpdatingEventArgs e)
        {
            ASPxGridView gv = sender as ASPxGridView;
            gv.AddNewRow();

            e.Cancel = true;
            gv.CancelEdit();
            
        }        
      
        protected void btn_delete_Click(object sender, EventArgs e)
        {
            string confirmValue = Request.Form["confirm_value"];

            if (confirmValue == "true")
            {
                SqlCommand del_cmd = new SqlCommand();
                del_cmd.Connection = fgm_moduler.func_Class.conn;

                //SqlDataAdapter data_adapter;
                Detector_obid = GridGroupDetector.GetRowValues(GridGroupDetector.FocusedRowIndex, "OBID").ToString();

                string qry = "UPDATE N3OBJATTR SET N1TERMINATIONDATE = REPLACE(REPLACE(CONVERT(varchar(23), GETDATE(), 121), '-','/'),'  ', '-'),";
                qry = qry + " N1TERMINATIONSESID = '" + SessionInfo.UserID + "' WHERE LEFTREL IN ";
                qry = qry + " (SELECT R.LEFTREL FROM N3RGRPRGRP R WHERE R.RIGHTREL = '" + Detector_obid + "' AND N1TERMINATIONDATE = '')";
                qry = qry + " AND N1TERMINATIONDATE = ''";

                del_cmd.CommandText = qry;
                del_cmd.ExecuteNonQuery();

                qry = "UPDATE N2REVGRP SET N1TERMINATIONDATE = REPLACE(REPLACE(CONVERT(varchar(23), GETDATE(), 121), '-','/'),'  ', '-'),";
                qry = qry + " N1TERMINATIONSESID = '" + SessionInfo.UserID + "' WHERE OBID IN ";
                qry = qry + " (SELECT R.LEFTREL FROM N3RGRPRGRP R WHERE R.RIGHTREL = '" + Detector_obid + "' AND N1TERMINATIONDATE = '')";
                qry = qry + " AND N1TERMINATIONDATE = ''";
                
                del_cmd.CommandText = qry;
                del_cmd.ExecuteNonQuery();

                //N3RGRPRGRP Termination
                qry = " UPDATE N3RGRPRGRP SET N1TERMINATIONDATE = REPLACE(REPLACE(CONVERT(varchar(23), GETDATE(), 121), '-','/'),'  ', '-'),";
                qry = qry + "N1TERMINATIONSESID = '" + SessionInfo.UserID + "' WHERE RIGHTREL = '" + Detector_obid + "' AND N1TERMINATIONDATE = '' ";

                del_cmd.CommandText = qry;
                del_cmd.ExecuteNonQuery();


                string qry2 = "UPDATE N3OBJATTR SET N1TERMINATIONDATE = REPLACE(REPLACE(CONVERT(varchar(23), GETDATE(), 121), '-','/'),'  ', '-'),";
                qry2 = qry2 + " N1TERMINATIONSESID = '" + SessionInfo.UserID + "' WHERE LEFTREL IN (SELECT OBID FROM N2REVGRP ";
                qry2 = qry2 + " WHERE OBID IN ('" + Detector_obid + "') AND N1TERMINATIONDATE='' AND N1PROJECTNAME ='"+ SessionInfo.ProjectName +"') AND N1TERMINATIONDATE = ''";

                del_cmd.CommandText = qry2;
                del_cmd.ExecuteNonQuery();

                qry2 = "UPDATE N2REVGRP SET N1TERMINATIONDATE = REPLACE(REPLACE(CONVERT(varchar(23), GETDATE(), 121), '-','/'),'  ', '-'),";
                qry2 = qry2 + " N1TERMINATIONSESID = '" + SessionInfo.UserID + "' ";
                qry2 = qry2 + " WHERE OBID IN ('" + Detector_obid + "') AND N1TERMINATIONDATE='' ";

                del_cmd.CommandText = qry2;
                del_cmd.ExecuteNonQuery();

                makeViewGridGroupDetector();
            }
            else
                return;
            //data_adapter = new SqlDataAdapter(qry, fgm_moduler.func_Class.connString);
            //data_adapter = new SqlDataAdapter(qry2, fgm_moduler.func_Class.connString);
            
            //multi row delete
        }
                
        protected void gv1temp_InitNewRow(object sender, EventArgs e)
        {
            ASPxGridView gv = sender as ASPxGridView;
            gv.CancelEdit();
            if (gv.VisibleRowCount > 0)
            {
                string Detector_type = "";
                string Detector_unit = "";
                string Detector_loc = "";
                string tag_Count = "";
                for (int i = 0; i < dynamic_table.Rows.Count; i++)
                {
                    if (dynamic_table.Rows[i]["DB_NAME"].ToString() == "DETECTOR_TYPE")
                    {
                        Detector_type = dynamic_table.Rows[i]["N3_VALUE"].ToString();

                    }
                    if (dynamic_table.Rows[i]["DB_NAME"].ToString() == "ID_UNIT")
                    {
                        Detector_unit = dynamic_table.Rows[i]["N3_VALUE"].ToString();

                    }
                    if (dynamic_table.Rows[i]["DB_NAME"].ToString() == "LOC")
                    {
                        Detector_loc = dynamic_table.Rows[i]["N3_VALUE"].ToString();
                    }
                    if (dynamic_table.Rows[i]["DB_NAME"].ToString() == "CNT")
                    {
                        tag_Count = dynamic_table.Rows[i]["N3_VALUE"].ToString();

                    }
                }
                gv1temp.JSProperties["cpNewWindowUrl"] = "FDMAddEffect.aspx?OBID_Detector=" + Detector_obid + "&Detector_unit=" + Detector_unit + "&Detector_loc=" + Detector_loc + "&ZONE=" + zone + "&Detector_type=" + Detector_type + "&tag_Count=" + tag_Count;
            }
            else
            {
                string tag_Count = "";
                for (int i = 0; i < dynamic_table.Rows.Count; i++)
                {
                    if (dynamic_table.Rows[i]["DB_NAME"].ToString() == "CNT")
                    {
                        tag_Count = dynamic_table.Rows[i]["N3_VALUE"].ToString();
                        break;
                    }
                }
                gv1temp.JSProperties["cpNewWindowUrl"] = "FDMQNA.aspx?TYPE=" + Detector_type + "&OBID=" + Detector_obid + "&UNIT=" + Detector_unit + "&LOC=" + Detector_loc + "&ZONE=" + zone + "&CNT=" + tag_Count;
            }

        }
        protected void gv1temp_RowDeleting(object sender, ASPxDataDeletingEventArgs e)
        {
            SqlCommand del_cmd = new SqlCommand();
            del_cmd.Connection = fgm_moduler.func_Class.conn;

            //SqlDataAdapter data_adapter;

            ASPxGridView gv = sender as ASPxGridView;
            string Selected_N3OBJATTR_OBID = e.Keys[gv1temp.KeyFieldName].ToString();
            string delet_Tag = e.Values["TagNo"].ToString();
            //string selected_tag = e.Values[""]
            //string loc2 = gv.GetRowValues(GridGroupDetector.FocusedRowIndex, "LOC2").ToString();
            string loc2 = e.Values["LOC2"].ToString();

            string qry = " UPDATE N3OBJATTR SET N1TERMINATIONDATE = REPLACE(REPLACE(CONVERT(varchar(23), GETDATE(), 121), '-','/'),'  ', '-'),";
            qry = qry + "N1TERMINATIONSESID = '" + SessionInfo.UserID + "' ";
            qry = qry + "WHERE OBID = '" + Selected_N3OBJATTR_OBID + "' AND N1TERMINATIONDATE =''";

            //data_adapter = new SqlDataAdapter(qry, fgm_moduler.func_Class.connString);
            del_cmd.CommandText = qry;
            del_cmd.ExecuteNonQuery();

            if (loc2 == "CLEAN AGENT" || loc2 == "DELUGE" || loc2 == "ESD" || loc2 == "FOAM")
            {
                string qry2 = " UPDATE TBLSYSTEM_INTERFACE SET N1TERMINATIONDATE = REPLACE(REPLACE(CONVERT(varchar(23), GETDATE(), 121), '-','/'),'  ', '-'),";
                qry2 = qry2 + "N1TERMINATIONID = '" + SessionInfo.UserID + "' ";
                qry2 = qry2 + "WHERE EFFECT_TAG = '" + delet_Tag + "' ";
                del_cmd.CommandText = qry2;
                del_cmd.ExecuteNonQuery();
            }

            e.Cancel = true;
            gv1temp.CancelEdit();

            gv1temp.JSProperties["cpNewWindowUrl"] = null;
        }
        
}
    //public class EditFormTemplate : ITemplate
    //{
    //    FGMModule fgm_moduler;
    //    DataTable _dynamic_effect;
    //    string _zone = "";
    //    string _DetectorGroupTag = "";
    //    string _detector_type = "";

    //    public string DetectorGroupTag
    //    {
    //        get { return _DetectorGroupTag; }
    //        set { _DetectorGroupTag = value; }
    //    }
    //    public string zone
    //    {
    //        get { return _zone; }
    //        set { _zone = value; }
    //    }
    //    public string detector_type
    //    {
    //        get { return _detector_type; }
    //        set { _detector_type = value; }
    //    }
    //    public DataTable dynamic_effect
    //    {
    //        get { return _dynamic_effect; }
    //        set { _dynamic_effect = value; }
    //    }
    //    public void InstantiateIn(Control container)
    //    {
    //        Control control = new Control();
    //        control.ID = "EditFormTemplate";
    //        container.Controls.Add(control);

    //        ASPxFormLayout btn_lo = new ASPxFormLayout();
    //        btn_lo.ID = "lo_effect";
    //        btn_lo.SettingsItems.HorizontalAlign = FormLayoutHorizontalAlign.Right;
    //        btn_lo.AlignItemCaptionsInAllGroups = true;


    //        LayoutItem gv_li = new LayoutItem("gv_li");
    //        gv_li.Height = Unit.Percentage(80);
    //        gv_li.ShowCaption = DevExpress.Utils.DefaultBoolean.False;
    //        gv_li.HorizontalAlign = FormLayoutHorizontalAlign.Right;
    //        btn_lo.Items.Add(gv_li);
    //        LayoutItemNestedControlContainer gv_li_container = gv_li.LayoutItemNestedControlContainer;

    //        ASPxGridView edit_form = new ASPxGridView();
    //        edit_form.ID = "gv1";
    //        GridViewDataTextColumn dc1 = new GridViewDataTextColumn();
    //        dc1.Caption = "OBID";
    //        dc1.FieldName = "OBID";
    //        dc1.Visible = false;
    //        edit_form.Columns.Add(dc1);
    //        GridViewDataTextColumn dc2 = new GridViewDataTextColumn();
    //        dc2.Caption = "Alarm Level";
    //        dc2.FieldName = "ALARM_LEVEL";
    //        dc2.GroupIndex = 0;
    //        edit_form.Columns.Add(dc2);
    //        GridViewDataTextColumn dc3 = new GridViewDataTextColumn();
    //        dc3.Caption = " ";
    //        dc3.FieldName = "VOTING";
    //        dc3.GroupIndex = 1;
    //        edit_form.Columns.Add(dc3);
    //        GridViewDataTextColumn dc4 = new GridViewDataTextColumn();
    //        dc4.Caption = "Description";
    //        dc4.FieldName = "DESC_NAME";
    //        edit_form.Columns.Add(dc4);
    //        dc4.CellStyle.Font.Size = FontUnit.Smaller;
    //        GridViewDataTextColumn dc5 = new GridViewDataTextColumn();
    //        dc5.Caption = "Location 1";
    //        dc5.FieldName = "LOC1";
    //        edit_form.Columns.Add(dc5);
    //        dc5.CellStyle.Font.Size = FontUnit.Smaller;
    //        GridViewDataTextColumn dc6 = new GridViewDataTextColumn();
    //        dc6.Caption = "Location 2";
    //        dc6.FieldName = "LOC2";
    //        edit_form.Columns.Add(dc6);
    //        dc6.CellStyle.Font.Size = FontUnit.Smaller;
    //        GridViewCommandColumn cc = new GridViewCommandColumn();
    //        cc.Caption = "v";

    //        cc.ShowSelectCheckbox = true;
    //        cc.VisibleIndex = 0;
    //        edit_form.Columns.Add(cc);

    //        edit_form.DataSource = make_gv1_editForm();
    //        edit_form.DataBind();

    //        edit_form.SettingsPopup.EditForm.ShowHeader = false;
    //        edit_form.KeyFieldName = "OBID";
    //        edit_form.Styles.Header.HorizontalAlign = HorizontalAlign.Center;
    //        edit_form.Settings.GroupFormat = "{0} {1}";
    //        edit_form.SettingsBehavior.AutoExpandAllGroups = true;
    //        edit_form.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;
    //        edit_form.SettingsPager.PageSize = 20;
    //        edit_form.Settings.ShowStatusBar = GridViewStatusBarMode.Visible;
    //        edit_form.Styles.Cell.Font.Name = "Arial";

    //        foreach (DataRow dr in _dynamic_effect.Rows)
    //        {
    //            edit_form.Selection.SelectRowByKey(dr["OBID_TEMPLATE"].ToString());

    //        }

    //        container.Controls.Add(edit_form);

    //        Control control = (container as GridViewEditFormTemplateContainer).Grid.Page.LoadControl("./EffectGrid.ascx");
    //        control.ID = "cnr" + container.ID;
    //        container.Controls.Add(control);
    //        gv_li_container.Controls.Add(edit_form);



    //        LayoutItem btn_li = new LayoutItem("btn_li");
    //        btn_li.Height = Unit.Percentage(10);
    //        btn_li.ShowCaption = DevExpress.Utils.DefaultBoolean.False;
    //        btn_li.HorizontalAlign = FormLayoutHorizontalAlign.Right;
    //        btn_li.Width = Unit.Percentage(100);
    //        btn_lo.Items.Add(btn_li);
    //        LayoutItemNestedControlContainer btn_li_container = btn_li.LayoutItemNestedControlContainer;

    //        ASPxGridViewTemplateReplacement btn_upd = new ASPxGridViewTemplateReplacement();
    //        btn_upd.ReplacementType = GridViewTemplateReplacementType.EditFormUpdateButton;
    //        btn_upd.ID = "btn_upd";
    //        btn_upd.ControlStyle.Width = Unit.Percentage(100);
    //        btn_li_container.Controls.Add(btn_upd);
    //        container.Controls.Add(btn_lo);

    //        container.Controls.Add(btn_lo);

    //    }
    //    protected DataTable make_gv1_editForm()
    //    {
    //        fgm_moduler = new FGMModule("FDM", SessionInfo.ProjectName, "TagGroupDetector");

    //        DataTable dt_effect = new DataTable();
    //        DataTable dt_interface = new DataTable();
    //        SqlDataAdapter data_adapter;

    //        string load_qry = "SELECT OBID, DESC_NAME, LOC1, LOC2, N1NAME + ' | ' + DESC_NAME +' | ' + LOC1 +' | ' + LOC2 + ' | ' + CODE as [Token]  ";
    //        load_qry = load_qry + " , ALARM_LEVEL, VOTING, VOTING_CNT ";
    //        load_qry = load_qry + " FROM TBLEFFECT_SETUP_NEW ";
    //        load_qry = load_qry + " WHERE N1PROJECTNAME = '" + SessionInfo.ProjectName + "' AND N1TERMINATIONDATE = '' ";
    //        load_qry = load_qry + " AND DETECTOR_TYPE LIKE '/%" + _detector_type + "/%' ";
    //        load_qry = load_qry + "  ORDER BY CODE";

    //        data_adapter = new SqlDataAdapter(load_qry, fgm_moduler.func_Class.connString);
    //        data_adapter.Fill(dt_effect);

    //        dt_effect.Columns.Add("CHECK");

    //        foreach (DataRow dr in _dynamic_effect.Rows)
    //        {
    //            dt_effect.Select("OBID = '" + dr["OBID_TEMPLATE"].ToString() + "'")[0]["CHECK"] = "1";
    //        }

    //        return dt_effect;
    //    }
    //    protected void gv1_CustomJSProperties(object sender, CustomJSPropertiesEventArgs e)
    //    {
    //        ASPxGridView gv_effect = sender as ASPxGridView;
    //        Hashtable selectedRowTable = new Hashtable();
    //        for (int i = 0; i < gv_effect.VisibleRowCount; i++)
    //        {
    //            DataRow dr = gv_effect.GetDataRow(i);
    //            if (!selectedRowTable.ContainsKey(dr["OBID"].ToString()))
    //                selectedRowTable.Add(dr["OBID"].ToString(), string.Format("{0} {1}", dr["TagNo"].ToString(), dr["Location"].ToString()));
    //        }
    //        e.Properties["cpgetTokenname"] = selectedRowTable;
    //    }
    //}
    public class GroupDetectorEditFormTemplate : ITemplate
    {
        DataTable _datafield;
        public DataTable datafield
        {
            get { return _datafield; }
            set { _datafield = value; }
        }
        
        public void InstantiateIn(Control container)
        {
            Control control = new Control();
            control.ID = "GroupDetectorEditForm";
            container.Controls.Add(control);
            
            DataTable dt = datafield;
            string db_name = "";
            string desc_name = "";
            string auth = "";
            string cell_type = "";

            ASPxFormLayout editform_GroupDetector = new ASPxFormLayout();
            editform_GroupDetector.ID = "editform_GroupDetector";
            //editform_GroupDetector.Border.BorderWidth = Unit.Pixel(1);
            //editform_GroupDetector.SettingsItems.VerticalAlign = FormLayoutVerticalAlign.Middle;
            //editform_GroupDetector.SettingsItems.HorizontalAlign = FormLayoutHorizontalAlign.Center;
            editform_GroupDetector.Paddings.Padding = Unit.Pixel(8);
            
            
            foreach (DataRow dr in dt.Select("AUTH <> 'System'", "VIEW_SEQ"))
            {
                db_name = dr["DB_NAME"].ToString();
                desc_name = dr["DESC_NAME"].ToString();
                auth = dr["AUTH"].ToString();
                cell_type = dr["SEL_TYPE"].ToString();
                

                LayoutItem li = new LayoutItem(desc_name);
                li.FieldName = db_name;
                
                editform_GroupDetector.Items.Add(li);
                LayoutItemNestedControlContainer li_container = li.LayoutItemNestedControlContainer;
                if (auth == "Manager")
                {
                    li.CaptionStyle.Font.Bold = true;
                    li.CaptionStyle.ForeColor = Color.Red;
                }


                if (cell_type == "DropDown List")
                {
                    ASPxDropDownEdit dropdownEdit = new ASPxDropDownEdit();
                    dropdownEdit.ID = db_name;

                    ASPxComboBox comboBox = new ASPxComboBox();
                    comboBox.DataSource = Srch_DropDownItem(db_name);
                    comboBox.TextField = "LOOKUPDETDESC";
                    comboBox.DataBind();
                    comboBox.IncrementalFilteringMode = IncrementalFilteringMode.StartsWith;
                    comboBox.DropDownStyle = DropDownStyle.DropDown;
                    comboBox.EnableSynchronization = DevExpress.Utils.DefaultBoolean.False;
                    li_container.Controls.Add(comboBox);
                }
                else if (cell_type == "Note")
                {
                    ASPxMemo textMemo = new ASPxMemo();
                    textMemo.ID = db_name;
                    textMemo.Width = Unit.Percentage(91);
                    li_container.Controls.Add(textMemo);
                }
                else
                {
                    
                    ASPxTextBox textBox = new ASPxTextBox();
                    textBox.ID = db_name;
                    li_container.Controls.Add(textBox);


                    //Suffix 증가 유무 체크 (15.10.16)
                    if (db_name == "CNT")
                    {
                        li = new LayoutItem("Suffix");
                        li.FieldName = "DETECTOR_SUFFIX";

                        editform_GroupDetector.Items.Add(li);
                        li_container = li.LayoutItemNestedControlContainer;

                        ASPxCheckBox chkBox = new ASPxCheckBox();
                        chkBox.ID = "DETECTOR_SUFFIX";
                        li_container.Controls.Add(chkBox);
                    }

                }
                
            }
            

            container.Controls.Add(editform_GroupDetector);
            //container.Controls.Add(editform_GroupDetector);
            
            //Button
            ASPxFormLayout btn_lo = new ASPxFormLayout();
            btn_lo.ID = "button_GroupDetector";
            btn_lo.ColCount = 1;
            btn_lo.Width = Unit.Percentage(90);
            btn_lo.SettingsItems.HorizontalAlign = FormLayoutHorizontalAlign.Right;
            btn_lo.Paddings.Padding = Unit.Pixel(1);

            LayoutItem btn_li = new LayoutItem("btn_li");
            btn_li.ShowCaption = DevExpress.Utils.DefaultBoolean.False;
            btn_lo.Items.Add(btn_li);
            LayoutItemNestedControlContainer btn_li_container = btn_li.LayoutItemNestedControlContainer;

            ASPxGridViewTemplateReplacement btn_add = new ASPxGridViewTemplateReplacement();
            btn_add.ReplacementType = GridViewTemplateReplacementType.EditFormUpdateButton;
            btn_add.ID = "btn_add";
            btn_li_container.Controls.Add(btn_add);

            //LayoutItem btn_li2 = new LayoutItem("btn_li2");
            //btn_li2.ShowCaption = DevExpress.Utils.DefaultBoolean.False;
            //btn_lo.Items.Add(btn_li2);
            //LayoutItemNestedControlContainer btn_li_container2 = btn_li2.LayoutItemNestedControlContainer;

            ASPxGridViewTemplateReplacement btn_cancel = new ASPxGridViewTemplateReplacement();
            btn_cancel.ReplacementType = GridViewTemplateReplacementType.EditFormCancelButton;
            btn_cancel.ID = "btn_cancel";
            btn_li_container.Controls.Add(btn_cancel);
            //btn_li.HorizontalAlign = FormLayoutHorizontalAlign.Right;

            container.Controls.Add(btn_lo);
        }
        protected DataTable Srch_DropDownItem(string db_name)
        {
            string load_qry;

            FGMModule fgm_moduler;
            fgm_moduler = new FGMModule("LDM", SessionInfo.ProjectName, "TagGroupDetector");
            SqlDataAdapter data_adapter;
            DataSet ds_datafield = new DataSet();

            load_qry = "SELECT LOOKUPDETNAME, LOOKUPDETDESC FROM LOOKUPDET ";
            load_qry = load_qry + " WHERE LOOKUPNAME = '" + db_name + "' AND DEPENDENTPROJECT = '" + SessionInfo.ProjectName + "' AND TERMINATIONDATE = '' ";
            load_qry = load_qry + " ORDER BY LOOKUPDETDESC ";

            data_adapter = new SqlDataAdapter(load_qry, fgm_moduler.func_Class.admin_connString);
            data_adapter.Fill(ds_datafield, "DropDownItem");


            return ds_datafield.Tables["DropDownItem"];
        }
    }

}