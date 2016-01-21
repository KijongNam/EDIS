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
//using DevExpress.XtraPrinting.Export;
using System.Collections;




using DevExpress.Web.Data;
using System.Collections;

namespace EDISON
{
    public partial class FDMVoting : System.Web.UI.Page
    {
        FGMModule fgm_moduler;
        DataSet DetectorVoting = new DataSet();
        DataTable detector = new DataTable();        
        
        DataTable Effect = new DataTable();
        protected DataTable ds_Type = new DataTable();
      
        string zone = "";
        string tagID = "";
       

        protected void Page_Load(object sender, EventArgs e)
        {          
            fgm_moduler = new FGMModule("FDM", "TT2", "TagGroupDetector");
            Detector_Grid.SettingsDetail.ShowDetailRow = true;
            detector.Columns.Add("Voting1");
            detector.Columns.Add("Voting2");
            DetectorVoting.Tables.AddRange(new DataTable[] { detector, Effect });

            if (!IsPostBack)
            {
                Setup_Zone();
                DetectorVoting.Clear();   
            }
            zone = ddl_zone.SelectedValue;
            tagType_HiddenField.Value = zone;


            makeDetectorGridView();
            makeEffectGridView();
           
            Session["Dataset"] = DetectorVoting;

            Detector_Grid.DataSource = DetectorVoting;
            Detector_Grid.DataBind();
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
        protected void Setup_Type(object sender, EventArgs e )
        {
            SqlDataAdapter data_adapter;
            string load_qry = "SELECT LOOKUPDETNAME FROM LOOKUPDET WHERE LOOKUPNAME = 'DETECTOR_TYPE' AND DEPENDENTPROJECT = '";
            load_qry = load_qry + fgm_moduler.func_Class.project_Name + "'AND TERMINATIONDATE = ''";
            data_adapter = new SqlDataAdapter(load_qry, fgm_moduler.func_Class.admin_connString);
            data_adapter.Fill(ds_Type);

            DropDownList type1 = (DropDownList)sender;
            type1.DataSource = ds_Type;
            type1.DataTextField = "LOOKUPDETNAME";
            type1.DataValueField = "LOOKUPDETNAME";
            type1.DataTextFormatString = "{0}";
            type1.DataBind();
            return;
        }

        private void makeDetectorGridView()
        {
           // DetectorVoting.Clear();
            SqlDataAdapter data_adapter;
            string load_qry = "SELECT OBID ,TYPE1, VOTING_CNT1, ALARM_LEVEL1, TYPE2,  VOTING_CNT2, ALARM_LEVEL2 ";
            load_qry = load_qry + "FROM TBLDETECTOR_VOTING ";
            load_qry = load_qry + "WHERE N1PROJECTNAME = '" + fgm_moduler.func_Class.project_Name + "' and Zone = '" + zone + "' AND N1TERMINATIONDATE IS NULL ";
            data_adapter = new SqlDataAdapter(load_qry, fgm_moduler.func_Class.conn);
            data_adapter.Fill(detector);

            for (int i = 0; i < detector.Rows.Count; i++)
            {
                string voting1 = ""; string voting2 = "";
                voting1 = detector.Rows[i]["VOTING_CNT1"].ToString() + "ooN";
                detector.Rows[i]["voting1"] = voting1;
                voting2 = detector.Rows[i]["VOTING_CNT2"].ToString() + "ooN";
                detector.Rows[i]["voting2"] = voting2;
    }
        }

        private void makeEffectGridView()
        {
            SqlDataAdapter data_adapter;
            string load_qry = "SELECT B.OBID ,B.LEFTREL, C.OBID AS EffectOBID, C.N1NAME AS TAG, C.DESC_NAME, C.SYS_INTERFACE, C.ACTION_STATUS ";
            load_qry = load_qry + "FROM TBLDETECTOR_VOTING A INNER JOIN TB_DE_LINK B ON A.OBID = B.LEFTREL INNER JOIN TBLEFFECT_SETUP C ON B.RIGHTREL = C.OBID ";
            load_qry = load_qry + "WHERE A.N1PROJECTNAME = '" + fgm_moduler.func_Class.project_Name + "' ";
            load_qry = load_qry + "and Zone = '" + zone + "' AND  B.N1TERMINATIONDATE IS NULL ";
            data_adapter = new SqlDataAdapter(load_qry, fgm_moduler.func_Class.conn);
            data_adapter.Fill(Effect);         
        }

        public void gvDetail_BeforePerformDataSelect(object sender, EventArgs e)
        {
            DetectorVoting = (DataSet)Session["DataSet"];
            DataTable detailTable = DetectorVoting.Tables[1];
            DataView dv = new DataView(detailTable);
            ASPxGridView detailGridView = (ASPxGridView)sender;
            dv.RowFilter = "LEFTREL = '" + detailGridView.GetMasterRowKeyValue().ToString() + "'";
            detailGridView.DataSource = dv;

            detailGridView.Templates.EditForm = new EditFormTemplate_DV();


        }

        public void Effect_Inserting(object sender, ASPxDataInsertingEventArgs e)
        {
                     

            ASPxGridView detailGridView = (ASPxGridView)sender;

            ASPxGridView editform_gv = (ASPxGridView)detailGridView.FindEditFormTemplateControl("cnr").FindControl("gv1");
            ASPxFormLayout fl = (ASPxFormLayout)detailGridView.FindEditFormTemplateControl("cnr").FindControl("Effect_Layout_Form");

            
            for (int i = 0; i < editform_gv.Selection.Count; i++)
            {
                SqlCommand cmd;   
                string effect_obid = editform_gv.GetSelectedFieldValues("OBID")[i].ToString();
                string detector_obid = detailGridView.GetMasterRowKeyValue().ToString();
                string load_qry = "insert into TB_DE_LINK(OBID, LEFTREL, RIGHTREL, N1CREATIONDATE, N1CREATIONSESID)  ";
                load_qry = load_qry + "VALUES (NEWID(), '" + detector_obid + "', '"+effect_obid+"',  ";
                load_qry = load_qry + "REPLACE(REPLACE(CONVERT(varchar(23), GETDATE(), 121), '-','/'),'  ', '-') , '" + SessionInfo.UserName + "')  ";
                cmd = new SqlCommand(load_qry, fgm_moduler.func_Class.conn);
                cmd.ExecuteNonQuery();
            }
            
            e.Cancel = true;
            Detector_Grid.CancelEdit();

            //makeDetectorGridView();
            //makeEffectGridView();
            
            //Session["Dataset"] = DetectorVoting;

            //Detector_Grid.DataSource = DetectorVoting;
            //Detector_Grid.DataBind();
        
        }

        public void Effect_Deleting(object sender, ASPxDataDeletingEventArgs e)
        {
            SqlCommand cmd;
           
            ASPxGridView detailGridView = (ASPxGridView)sender;
            string Selected_OBID = e.Keys[detailGridView.KeyFieldName].ToString(); //detailGridView.GetRowValues(detailGridView.FocusedRowIndex, "OBID").ToString();
            
            string qry = " UPDATE TB_DE_LINK SET N1TERMINATIONDATE = REPLACE(REPLACE(CONVERT(varchar(23), GETDATE(), 121), '-','/'),'  ', '-'),  ";
            qry = qry + "N1TERMINATIONSESID = '" + SessionInfo.UserName + "'  ";
            qry = qry + "WHERE OBID = '" + Selected_OBID + "' ";
            cmd = new SqlCommand(qry, fgm_moduler.func_Class.conn);
            cmd.ExecuteNonQuery();

            e.Cancel = true;
            Detector_Grid.CancelEdit();
        }

   

        public void DetectorVoting_Deleting(object sender, ASPxDataDeletingEventArgs e)
        {
            SqlCommand cmd;
            ASPxGridView detailGridView = (ASPxGridView)sender;



            string Selected_OBID = e.Keys[Detector_Grid.KeyFieldName].ToString();
            
            string qry = " UPDATE TBLDETECTOR_VOTING SET N1TERMINATIONDATE = REPLACE(REPLACE(CONVERT(varchar(23), GETDATE(), 121), '-','/'),'  ', '-'),  ";
            qry = qry + "N1TERMINATIONSESID = '" + SessionInfo.UserID + "'  ";
            qry = qry + "WHERE OBID = '" + Selected_OBID + "' ";
            cmd = new SqlCommand(qry, fgm_moduler.func_Class.conn);
            cmd.ExecuteNonQuery();

            e.Cancel = true;
            Detector_Grid.CancelEdit();
            detailGridView.CancelEdit();
            //Detector_Grid.CancelEdit();
        }

        protected void DetectorVoting_Inserting(object sender, ASPxDataInsertingEventArgs e)
        {
            SqlCommand cmd;        
          
            ASPxComboBox combo_voting_cnt = (ASPxComboBox)Detector_Grid.FindEditRowCellTemplateControl((GridViewDataColumn)Detector_Grid.Columns[1], "combo_voting_cnt");
            ASPxComboBox combo_voting2_cnt = (ASPxComboBox)Detector_Grid.FindEditRowCellTemplateControl((GridViewDataColumn)Detector_Grid.Columns[3], "combo_voting2_cnt");  
            DropDownList combo_type1 = (DropDownList)Detector_Grid.FindEditRowCellTemplateControl((GridViewDataColumn)Detector_Grid.Columns[0], "TYPE1_COMBO");
            DropDownList combo_type2 = (DropDownList)Detector_Grid.FindEditRowCellTemplateControl((GridViewDataColumn)Detector_Grid.Columns[2], "TYPE2_COMBO");
           
            string voting1 = combo_voting_cnt.Text;
            string voting2 = combo_voting2_cnt.Text;
            string type1 = combo_type1.Text;
            string type2 = combo_type2.Text;
            
            string qry = "insert into TBLDETECTOR_VOTING(OBID, TYPE1, VOTING_CNT1, TYPE2, VOTING_CNT2, N1CREATIONDATE, N1CREATIONSESID, N1PROJECTNAME, Zone) ";
            qry = qry + "values (NEWID(), '"+type1+"','"+voting1+"','"+type2+"', '"+voting2+"',  REPLACE(REPLACE(CONVERT(varchar(23), GETDATE(), 121), '-','/'),'  ', '-'), ";
            qry = qry + "'" + SessionInfo.UserName + "', '" + fgm_moduler.func_Class.project_Name + "', '" + zone + "')";
                           
            cmd = new SqlCommand(qry, fgm_moduler.func_Class.conn);
            cmd.ExecuteNonQuery();

            e.Cancel = true;
            Detector_Grid.CancelEdit();
        }      

}







    public class EditFormTemplate_DV : ITemplate
    {
        FGMModule fgm_moduler;
        string _zone = "";
        string _DetectorGroupTag = "";

        public string DetectorGroupTag
        {
            get { return _DetectorGroupTag; }
            set { _DetectorGroupTag = value; }
        }
        public string zone
        {
            get { return _zone; }
            set { _zone = value; }
        }
        public void InstantiateIn(Control container)
        {

            Control control = (container as GridViewEditFormTemplateContainer).Grid.Page.LoadControl("./EffectGrid_DV.ascx");
            control.ID = "cnr" + container.ID;
            container.Controls.Add(control);

            ASPxGridViewTemplateReplacement btn_upd = new ASPxGridViewTemplateReplacement();
            btn_upd.ReplacementType = GridViewTemplateReplacementType.EditFormUpdateButton;
            btn_upd.ID = "btn_upd";
            container.Controls.Add(btn_upd);

            //ASPxGridViewTemplateReplacement btn_cancel = new ASPxGridViewTemplateReplacement();
            //btn_cancel.ReplacementType = GridViewTemplateReplacementType.EditFormCancelButton;
            //btn_cancel.ID = "btn_cancel";
            //container.Controls.Add(btn_cancel);


        }
        protected DataTable make_gv1_editForm()
        {
            fgm_moduler = new FGMModule("LDM", "TT2", "TagGroupDetector");
            //zone = Request.Form["ddl_zone"].ToString();
            //DetectorGroupTag = Request.Form["aa11"].ToString();

            DataTable dt_effect = new DataTable();
            DataTable dt_interface = new DataTable();
            SqlDataAdapter data_adapter;
            string load_qry = "SELECT OBID, SYS_INTERFACE, N1NAME as [TagNo], ID_ZONE as [Zone], ID_TYPE as [Type], EFFECT_NO as [Seq], DESC_NAME as [Description],  ";
            load_qry = load_qry + " N1NAME + ' | ' + DESC_NAME +' | ' + LOC +' | ' + ACTION_STATUS as [Token], ";
            load_qry = load_qry + " LOC as [Location], ACTION_STATUS as [ActionStatus] FROM TBLEFFECT_SETUP ";
            load_qry = load_qry + " WHERE N1PROJECTNAME = '" + SessionInfo.ProjectName + "' AND N1TERMINATIONDATE = '' ";
            load_qry = load_qry + " AND (ID_ZONE = '" + zone + "' OR ID_ZONE = 'COMMON') ";
            //load_qry = load_qry + " AND ID_ZONE = '" + ddl_zone.SelectedValue + "'" ;
            load_qry = load_qry + "  ORDER BY N1NAME";
            data_adapter = new SqlDataAdapter(load_qry, fgm_moduler.func_Class.connString);
            data_adapter.Fill(dt_effect);


            string interface_qry = "SELECT DISTINCT SYS_INTERFACE FROM TBLEFFECT_SETUP ";
            //interface_qry = interface_qry + " WHERE N1PROJECTNAME = '" + SessionInfo.ProjectName + "' AND N1TERMINATIONDATE = '' ";
            //interface_qry = interface_qry + " AND ID_ZONE = '" + ddl_zone.SelectedValue + "'" ;
            interface_qry = interface_qry + " ORDER BY SYS_INTERFACE ";
            data_adapter = new SqlDataAdapter(interface_qry, fgm_moduler.func_Class.connString);
            data_adapter.Fill(dt_interface);

            dt_effect.Columns.Add("check");
            return dt_effect;
            //gv1.DataSource = dt_effect;
            //gv1.DataBind();
        }
        protected void gv1_CustomJSProperties(object sender, CustomJSPropertiesEventArgs e)
        {
            ASPxGridView gv_effect = sender as ASPxGridView;
            Hashtable selectedRowTable = new Hashtable();
            for (int i = 0; i < gv_effect.VisibleRowCount; i++)
            {
                DataRow dr = gv_effect.GetDataRow(i);
                if (!selectedRowTable.ContainsKey(dr["OBID"].ToString()))
                    selectedRowTable.Add(dr["OBID"].ToString(), string.Format("{0} {1}", dr["TagNo"].ToString(), dr["Location"].ToString()));
            }
            e.Properties["cpgetTokenname"] = selectedRowTable;
        }
    }



}