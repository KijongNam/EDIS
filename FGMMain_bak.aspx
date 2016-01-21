<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FGMMain.aspx.cs" Inherits="EDISON.FGMMain" %>

<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register assembly="DevExpress.Web.ASPxTreeList.v14.2, Version=14.2.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxTreeList" tagprefix="dx1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>:: HEC-EDISON - Detector Main ::</title>
    <link href='./css/edis_Setup.css' type='text/css' rel='stylesheet' />
<script type="text/javascript">

    var postponedCallbackRequired = false;
    function GridDblClick(s, e) {
        if (GridGroupDetail.InCallback())
            postponedCallbackRequired = true;
        else {
            GridGroupDetail.PerformCallback();
            GridDetector.PerformCallback();
            gv1temp.PerformCallback();
        }
    }
    function OnEndCallback(s, e) {
        if (postponedCallbackRequired) {
            GridGroupDetail.PerformCallback();
            GridDetector.PerformCallback();
            gv1temp.PerformCallback();
            postponedCallbackRequired = false;
        }
    }

    function ClearSelection() {
        GridEffectList.SetFocusedNodeKey("");
        UpdateControls(null, "");
    }
    function UpdateSelection() {
        var tokenname = "";
        var focusedNodeKey = GridEffectList.GetRowKey(GridEffectList.GetFocusedRowIndex())
        if (focusedNodeKey != "")
            tokenname = GridEffectList.cpgetTokenname[focusedNodeKey];
        UpdateControls(focusedNodeKey, tokenname);
    }
    function UpdateControls(key, text) {
        EffectDropDownList.SetText(text);
        EffectDropDownList.SetKeyValue(key);
        EffectDropDownList.HideDropDown();
        UpdateButtons();
    }
    function UpdateButtons() {
        clearButton.SetEnabled(EffectDropDownList.GetText() != "");
        selectButton.SetEnabled(GridEffectList.GetRowKey(GridEffectList.GetFocusedRowIndex()) != "");
    }
    function OnDropDown() {
        GridEffectList.SetFocusedNodeKey(EffectDropDownList.GetKeyValue());
        GridEffectList.MakeNodeVisible(GridEffectList.GetRowKey(GridEffectList.GetFocusedRowIndex()));
    }

    function OnSaveClick(s, e) {
        GroupDetail.UpdateEdit();
    };
    function EndBatchUpdate(s, e) {
        GroupDetail.PerformCallback();
    };
    function OnAddClick(s, e) {
        CallbackPanel1.PerformCallback("Insert_Effect");
    };

</script>
</head>

<body>
    <form id="form1" runat="server">
    <div style="float:top; width:45%; height:90%">
    <asp:DropDownList ID="ddl_zone" runat="server" Width="10%"  Height="23px" 
            Align="Middle" BackColor="White" AutoPostBack="True">
        </asp:DropDownList>
        <asp:Panel runat="server" ID="Panel1" HorizontalAlign="Right" Font-Names="돋움">
        <asp:FileUpload ID="FileUpload1" runat="server" Width="250px" Height="22px" TabIndex="1" CssClass="btnstyle" />
        <asp:Button ID="btn_Import" runat="server" UseSubmitBehavior="False" Text="Import" 
                Width="70px" CssClass="btnstyle" TabIndex="2" onclick="btn_Import_Click" />
        <asp:Button ID="btn_Export" runat="server" 
            Text="Export" Width="70px" CssClass="btnstyle" TabIndex="3" 
                UseSubmitBehavior="False" onclick="btn_Export_Click" />
        <asp:Button ID="btn_Save" runat="server" CssClass="btnstyle" Text="Save" style="width: 64px" 
        UseSubmitBehavior="False" Width="70px" TabIndex="4" />
            <dx:ASPxButton ID="ASP_Save" runat="server" Text="ASP_Save" AutoPostBack="False">
                <ClientSideEvents Click="OnSaveClick" />
            </dx:ASPxButton>
            <dx:ASPxButton ID="Add_Effect" runat="server" Text="Add_Effect" AutoPostBack="False">
                <ClientSideEvents Click="OnAddClick" />
            </dx:ASPxButton>
        </asp:Panel>
    </div>
    <br />

    <div style="float:left; width:24%; height:90%;">
        <dx:ASPxGridView ID="GridGroupDetector" runat="server" Width="99%" 
            ClientInstanceName="GridGroupTag" 
            oncustomcallback="GridGroupDetector_CustomCallback" EnableRowsCache="False" 
            SettingsBehavior-AllowSelectByRowClick="True" 
            SettingsBehavior-AllowFocusedRow="True" SettingsPager-PageSize="30" >
            <ClientSideEvents EndCallback="OnEndCallback" RowDblClick="GridDblClick" />
            <styles FocusedRow-BackColor="#003399" Header-HorizontalAlign="Center">
                <header backcolor="#CCFF99">
                </header>
            </styles>
        </dx:ASPxGridView>


        <dx:ASPxGridView ID="gv1temp" runat="server" Width="99%" ClientInstanceName="gv1temp"
            AutoGenerateColumns="False" KeyFieldName="OBID" >
            <Columns>
                <dx:GridViewCommandColumn ShowNewButton="False" ShowEditButton="true" VisibleIndex="0" ShowNewButtonInHeader="True" />
                
            <dx:GridViewDataTextColumn FieldName = "SIGNAL_VOTING" GroupIndex="0">
                </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="TagNo" VisibleIndex="1">
                <EditFormSettings VisibleIndex="1" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Description" VisibleIndex="2">
                <EditFormSettings VisibleIndex="2" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Location"  VisibleIndex="3">
                <EditFormSettings VisibleIndex="3" />
            </dx:GridViewDataTextColumn>
            </Columns>
            <Settings ShowTitlePanel="true" GroupFormat="{1}"></Settings>
            <SettingsText Title="[Effect]" />
            <SettingsEditing EditFormColumnCount="3" Mode="PopupEditForm" />
            <SettingsPopup>
                <EditForm Width="300" />
            </SettingsPopup>
            <Templates>
                <EditForm>
                    <dx:ASPxFormLayout ID="formLayout" runat="server" ColCount="1">
                        <Items>
                            <dx:LayoutItem Caption=" ">
                                <LayoutItemNestedControlCollection>
                                    <dx:LayoutItemNestedControlContainer runat="server" >
                                        <dx:ASPxGridView ID="nested_grid" runat="server" Width="300px"></dx:ASPxGridView>
                                    </dx:LayoutItemNestedControlContainer>
                                </LayoutItemNestedControlCollection>
                            </dx:LayoutItem>
                            
                        </Items>
                    </dx:ASPxFormLayout>
                    <dx:ASPxGridViewTemplateReplacement ID="UpdateBtn" ReplacementType="EditFormUpdateButton" runat="server" />
                </EditForm>
            </Templates>
        </dx:ASPxGridView>

    </div>
    
    <div style="float:left; width:18%; height:90%; padding-right: 10px; padding-left: 10px;" id="grpddl">
        <dx:ASPxGridView ID="GridGroupDetail" runat="server" Width="99%" 
            ClientInstanceName="GridGroupDetail" 
            EnableRowsCache="False" 
            SettingsEditing-Mode="Batch" 
            Settings-ShowColumnHeaders="False" 
            SettingsPager-PageSize="30" onhtmlrowcreated="GridGroupDetail_HtmlRowCreated" 
            onhtmlrowprepared="GridGroupDetail_HtmlRowPrepared" 
            oncustomcallback="GridGroupDetail_CustomCallback" 
            onbatchupdate="GridGroupDetail_BatchUpdate" 
            oncustomcolumndisplaytext="GridGroupDetail_CustomColumnDisplayText">
            <Columns>
            <dx:GridViewDataTextColumn Caption="#"  FieldName="RowNumber" UnboundType ="Integer" VisibleIndex="0" ReadOnly="true">
                <EditFormSettings Visible="False" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName = "N3_OBID" VisibleIndex="1" Visible="false">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName = "DB_NAME" VisibleIndex="2" Visible="false" >
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName = "DESC_NAME" VisibleIndex="3" ReadOnly="true" Width="50%">
                <EditFormSettings Visible="False" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName = "N3_VALUE"  VisibleIndex="4" Width="50%">
            </dx:GridViewDataTextColumn>
            </Columns>
            <ClientSideEvents EndCallback="OnEndCallback" />
            <SettingsText Title="[Group Detector]" />
            <SettingsPager PageSize="30"></SettingsPager>
            <SettingsEditing Mode="Batch" BatchEditSettings-ShowConfirmOnLosingChanges="False"></SettingsEditing>
            <Settings ShowStatusBar="Hidden" ShowTitlePanel="true"></Settings>
            <SettingsBehavior AllowFocusedRow = "true" />
            <styles>
                <header backcolor="#CCFF99">
                </header>
            </styles>
        </dx:ASPxGridView>
        
        
        <dx:ASPxCallbackPanel ID="CallbackPanel1" runat="server" Width="200px" 
            ClientInstanceName="CallbackPanel1" oncallback="CallbackPanel1_Callback">
            <PanelCollection>
                <dx:PanelContent>
                        <dx:ASPxDropDownEdit ID="InsEffectDropDownList" runat="server" Width="99%" 
                                ClientInstanceName="InsEffectDropDownList">
                                
                        </dx:ASPxDropDownEdit>
                </dx:PanelContent>
            </PanelCollection>
        </dx:ASPxCallbackPanel>
        <dx:ASPxDropDownEdit ID="EffectDropDownList" runat="server" Width="99%" 
                ClientInstanceName="EffectDropDownList" >
            <DropDownWindowTemplate>
                <div>
                    <dx:ASPxGridView ID="GridEffectList" runat="server" Width="100%" KeyFieldName="OBID" ClientInstanceName="GridEffectList" OnCustomJSProperties="GridEffectList_CustomJSProperties">
                    <ClientSideEvents FocusedRowChanged="function(s,e){ selectButton.SetEnabled(true); }" />
                    <SettingsBehavior AllowFixedGroups="true" AutoExpandAllGroups="True" AllowFocusedRow = "true" AllowSelectByRowClick="True" AllowDragDrop="False" AllowSort="False" />
                    <SettingsPager Mode="ShowAllRecords"/>
                        <Columns>
                            <dx:GridViewDataTextColumn FieldName = "OBID"  Visible="false">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName = "SYS_INTERFACE" Caption="Sys. Interface" GroupIndex="0">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName = "TagNo" Caption="TagNo." VisibleIndex="2" ReadOnly="true" Width="30%">
                            <CellStyle Font-Size="Smaller"></CellStyle>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName = "Description" Caption="Description" VisibleIndex="6" Width="40%">
                                <CellStyle Font-Size="Smaller"></CellStyle>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName = "Location" Caption="Loc." VisibleIndex="7" Width="5%" >
                            <CellStyle Font-Size="Smaller"></CellStyle>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName = "ActionStatus" Caption="Action" VisibleIndex="8" Width="20%">
                            <CellStyle Font-Size="Smaller"></CellStyle>
                            </dx:GridViewDataTextColumn>
                        </Columns>
                    </dx:ASPxGridView>
                </div>
                <table style="background-color: White; width: 100%;">
                    <tr>
                        <td style="padding: 10px;">
                            <dx:ASPxButton ID="clearButton" ClientEnabled="false" ClientInstanceName="clearButton"
                                runat="server" AutoPostBack="false" Text="Clear">
                                <ClientSideEvents Click="ClearSelection" />
                            </dx:ASPxButton>
                        </td>
                        <td style="text-align: right; padding: 10px;">
                            <dx:ASPxButton ID="selectButton" ClientEnabled="false" ClientInstanceName="selectButton"
                                runat="server" AutoPostBack="false" Text="Select">
                                <ClientSideEvents Click="UpdateSelection" />
                            </dx:ASPxButton>
                            <dx:ASPxButton ID="closeButton" runat="server" AutoPostBack="false" Text="Close">
                                <ClientSideEvents Click="function(s,e) { EffectDropDownList.HideDropDown(); }" />
                            </dx:ASPxButton>
                        </td>
                    </tr>
                </table>
            </DropDownWindowTemplate>
        </dx:ASPxDropDownEdit>
        <dx:ASPxTokenBox ID="ASPxTokenBox1" runat="server" AllowMouseWheel="True" TextField="Token"
                Tokens="" Width="99%" NullText="Select Effect">
            <CaptionCellStyle>
            </CaptionCellStyle>
            <TokenStyle Width="97%">
            </TokenStyle>
        </dx:ASPxTokenBox>
    

    </div>
    
    <div style="float:left; width:15%; height:90%;">
        <dx:ASPxGridView ID="GridDetector" runat="server" Width="95%" 
            ClientInstanceName="GridDetector" 
            EnableRowsCache="False" 
            Settings-ShowColumnHeaders="False" 
            SettingsBehavior-AllowSelectByRowClick="True" 
            SettingsPager-PageSize="30" AutoGenerateColumns="False" 
            >
            <styles FocusedRow-BackColor="#003399" Header-HorizontalAlign="Center">
                <header backcolor="#CCFF99"></header>
                <FocusedRow BackColor="#003399"></FocusedRow>
            </styles>
            <Columns>
                <dx:GridViewDataTextColumn FieldName = "DETECTOR" GroupIndex="0">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName = "SEQ"  VisibleIndex="1" Visible="true">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName = "GROUPDETECTOR" VisibleIndex="2" ReadOnly="true" Width="50%">
                    <EditFormSettings Visible="False" />
                </dx:GridViewDataTextColumn>
                
            </Columns>
            <SettingsPager PageSize="30"></SettingsPager>
            <Settings ShowTitlePanel="true" GroupFormat="{1}"></Settings>
            <SettingsBehavior AllowFixedGroups="true" AutoExpandAllGroups="True" />
            <SettingsText Title="[Detector]" />
        </dx:ASPxGridView>
    </div>

    
    <asp:SqlDataSource  ID="TBL_LOOKUPDET" runat="server"         
        SelectCommand="SELECT LOOKUPDETNAME, LOOKUPDETDESC FROM LOOKUPDET WHERE DEPENDENTPROJECT = 'TT2' AND LOOKUPNAME = 'SYS_INTERFACE' AND TERMINATIONDATE = '' " 
        ConnectionString="<%$ ConnectionStrings:Admin_dataConnectionString %>" 
        ProviderName="<%$ ConnectionStrings:Admin_dataConnectionString.ProviderName %>">
    </asp:SqlDataSource>
    <dx:ASPxGridViewExporter ID="GridViewExporter" runat="server" 
        GridViewID="GridGroupDetector">
    </dx:ASPxGridViewExporter>
    <br />
    </form>
</body>
</html>
