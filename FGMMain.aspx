<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FGMMain.aspx.cs" Inherits="EDISON.FGMMain" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>:: HEC-EDISON - Detector Main ::</title>
        <style type="text/css">
        .NoStyle
        {
            background-image: none !important;
            background-color: transparent !important;
            border: none !important;
            float: right !important;
        }
        .GroupCell
        {
            background-color: #FFCCCC !important;
            
        }
        .NoStyle2
        {
            background-image: none !important;
            background-color: transparent !important;
            border: none !important;
            border-style: none !important;
            font-size: 11px;
            float: left !important;
            vertical-align:text-bottom !important;
        }
    </style>
    <link href='./css/edis_Setup.css' type='text/css' rel='stylesheet' />
<script type="text/javascript">

    var postponedCallbackRequired = false;
    function GridDblClick(s, e) {
        var tag_type = document.getElementById("tagType_HiddenField");

        if (CallbackPanel1.InCallback()) {
            postponedCallbackRequired = true;
        }
        else {
            CallbackPanel1.PerformCallback();
        }
    }
    function OnEndCallback(s, e) {
        if (s.cpNewWindowUrl != null) {
            window.open(s.cpNewWindowUrl, '', 'Height=660px,Width=500px,scrolling=yes,status=yes,scrollbars=yes');
        }
        else {
            if (postponedCallbackRequired) {
                CallbackPanel1.PerformCallback();
                postponedCallbackRequired = false;
            }
            else {
                postponedCallbackRequired = true;
            }
        }
    }
    var updateCallbackRequire = false;
    function OnEndCallback_update(s, e) {
        if (s.cpNewWindowUrl != null) {
            window.open(s.cpNewWindowUrl, '', 'Height=660px,Width=830px,scrollbars=yes,status=yes,scrollbars=yes');
        }
    }

    function ClearSelection() {
        GridEffectList.SetFocusedNodeKey("");
        UpdateControls(null, "");
    }
    function UpdateSelection() {
        var tokenname = "";
        var focusedNodeKey = gv1.GetRowKey(gv1.GetFocusedRowIndex())
        if (focusedNodeKey != "")
            tokenname = gv1.cpgetTokenname[focusedNodeKey];
        gv1temp.UpdateEdit(tokenname);
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

    var panelCallbackRequired = false; 
    function EndBatchUpdate(s, e) {
        if (s.cpConfirmationMessage != "") {
            alert(s.cpConfirmationMessage);
        }
        else {
            if (s.cpNewWindowUrl != null)
                window.open(s.cpNewWindowUrl, '', 'Height=660px Width=550px scrollbars=yes status=yes');
            if (panelCallbackRequired) {
                CallbackPanel1.PerformCallback();
                panelCallbackRequired = false;
            }
            else {
                panelCallbackRequired = true;
            }
        }
    };
    function GridDetector_EndCallback(s, e) {
        if (s.cpConfirmationMessage != "") {
            alert(s.cpConfirmationMessage);
        }
        else {
            CallbackPanel1.PerformCallback();
        }
    }
    function OnSaveClick(s, e) {
        //if (GridGroupDetail.InCallback())
        if (CallbackPanel1.InCallback())
            panelCallbackRequired = true;
        else {
            panelCallbackRequired = true;
            GridGroupDetail.UpdateEdit();
            GridDetector.UpdateEdit();
        }
    };
    function OnAddClick(s, e) {
        if (GridGroupDetector.InCallback())
            panelCallbackRequired = true;
        else
            GridGroupDetector.AddNewRow();

    };
    function btn_delete_Click() {
        var confirm_value = document.createElement("INPUT");
        confirm_value.type = "hidden";
        confirm_value.name = "confirm_value";
        confirm_value.value = confirm("Delete?");
        document.forms[0].appendChild(confirm_value);
//        if (CallbackPanel1.InCallback()) {
//            panelCallbackRequired = true;
//        }
//        else {
//            CallbackPanel1.PerformCallback();
//        }
        CallbackPanel1.PerformCallback();
    };
//    function OnDelClick(s, e) {
//        GridGroupDetector.UpdateEdit();
//    };
    function OnEditUpdate(s, e) {
        gv1temp.UpdateEdit();
    };

    function open_Import(module_Name) {
        var url = "Setup_ImportList.aspx?MNAME=" + module_Name;
        window.open(url, "", "Height=660px,Width=988px,resizable=no,scrolling=no,status=yes");
        return false;
    };
    function open_Export(module_Name) {
        var url = "Setup_ExportList.aspx?MNAME=" + module_Name;
        window.open(url, "", "Height=350px,Width=570px,resizable=yes,scrolling=yes,status=yes");
        return false;
    };

    function CloseEditEffect(s, e) {
        gv1temp.CancelEdit();
    };

    //임시
    var lastDB_NAME=null;
    function GridGroupDetail_FocusedRowChanged(s, e) {
        var tag_type = document.getElementById("tagType_HiddenField").value;
        if (tag_type == "GroupDetector") {
            if (GridGroupDetail.GetEditor("N3_VALUE").InCallback()) {
                lastDB_NAME = true;
            }
            else {
                GridGroupDetail.GetEditor("N3_VALUE").PerformCallback();
            }
        }
    };
    function doIssue() {
        var b_check = confirm("Does issue?");

        if (b_check) {
            url = "FDMIssue.aspx?line_type=" + "" + "&mname=FDM";
            window.open(url, "", "Height=699px,Width=1140px,resizable=yes,scrolling=yes,status=yes");
        }

    };
    function doSysManage() {
        url = "FDMSystem.aspx?line_type=" +""+ "&mname=FDM";
        window.open(url, "", "Height=699px,Width=800px,resizable=yes,scrolling=yes,status=yes");

    };

    

    function rowClick(s, e) {
        s.SelectRowOnPage(e.visibleIndex, !s.IsRowSelectedOnPage(e.visibleIndex));
    }
</script>
</head>

<body>
    <form id="form1" runat="server">
    <div style="float:top; width:100%; height:20px">
    
        <asp:Panel runat="server" ID="Panel1" HorizontalAlign="Right" Font-Names="돋움">
        <table>
        <tr>
        <td style="width:600px; height:25px;" align="left" valign="top">
        <asp:DropDownList ID="ddl_zone" runat="server" Width="100px"  Height="21px" 
            Align="Middle" BackColor="White" AutoPostBack="True">
        </asp:DropDownList>
        <asp:HiddenField ID="tagName" runat="server" />
            <asp:ImageButton ID="btn_Issue" runat="server" 
                ImageUrl="../img/btn_issue.gif" ImageAlign="Middle"
                OnClientClick="javascript:doIssue();"  />
                <asp:ImageButton ID="btn_report" runat="server" Visible="false"
                ImageUrl="../img/btn_report.gif" ImageAlign="Middle" 
                />
                <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="../img/btn_system.gif" ImageAlign="Middle"
                 OnClientClick="javascript:doSysManage();"   />
                <asp:ImageButton ID="btn_Import" runat="server" ImageUrl="../img/btn_import.gif" ImageAlign="Middle"  
                    OnClientClick="javascript:open_Import('FDM');" />
                <asp:ImageButton ID="btn_Export1" runat="server" 
                ImageUrl="../img/btn_export.gif" ImageAlign="Middle"   />
                <asp:ImageButton ID="btn_Export" runat="server" ImageUrl="../img/btn_export.gif" ImageAlign="Middle" 
                    OnClientClick="javascript:open_Export('FDM');" />
        </td>
        <td style="width:40%; height:25px; float:right;" align="right" valign="top">
                    
                <dx:ASPxButton ID="btn_add" runat="server" AutoPostBack="False" AllowFocus="False" 
                        CssClass="NoStyle" EnableTheming="False" >
                    <ClientSideEvents Click="OnAddClick" />
                    <Image Url="../img/btn_add.gif">
                    </Image>
                <FocusRectPaddings Padding="0px" />
                </dx:ASPxButton>
                <dx:ASPxButton ID="btn_delete" runat="server" AutoPostBack="False" AllowFocus="False" OnClick="btn_delete_Click"
                        CssClass="NoStyle" EnableTheming="False" >
                <ClientSideEvents Click="btn_delete_Click" />
                    <Image Url="../img/btn_delete.gif">
                    </Image>
                <FocusRectPaddings Padding="0px" />
                </dx:ASPxButton>
                <dx:ASPxButton ID="btn_save" runat="server" AutoPostBack="False" AllowFocus="False" 
                        CssClass="NoStyle" EnableTheming="False" >
                    <ClientSideEvents Click="OnSaveClick" />
                    <Image Url="../img/btn_save.gif">
                    </Image>
                <FocusRectPaddings Padding="0px" />
                </dx:ASPxButton>
        </td>
            </tr>
        </table>
        </asp:Panel>
    </div>
    <br />
    
    <dx:ASPxCallbackPanel runat="server" ID="CallbackPanel1" ClientInstanceName="CallbackPanel1">
    <PanelCollection>
        <dx:PanelContent>
        <div runat="server" id="taglist" style="border-style: solid; border-width: 1px; float:left; width:41%; height:100%;">
        <dx:ASPxGridView ID="GridGroupDetector" runat="server" Width="100%" 
            ClientInstanceName="GridGroupDetector" OnHtmlRowCreated="GridGroupDetector_HtmlRowCreated"
            oncustomcallback="GridGroupDetector_CustomCallback" EnableRowsCache="False" OnRowInserting= "GridGroupDetector_OnRowInserting" >
            <%--<ClientSideEvents EndCallback="OnEndCallback" RowDblClick="GridDblClick" />--%>
            <ClientSideEvents EndCallback="EndBatchUpdate" RowDblClick="GridDblClick"/>
            <styles FocusedRow-BackColor="#003399"  FocusedRow-Font-Bold="true" Header-HorizontalAlign="Center">
                <header backcolor="#C1FFC1" Font-Size="11px" Font-Bold="true">
                </header>
                <Cell Font-Size="11px" HorizontalAlign="Center" VerticalAlign="Middle">
                </Cell>
            </styles>         
            <Settings  VerticalScrollBarMode =  "Visible"  VerticalScrollableHeight =  "650" ShowHeaderFilterButton="true" />
            <SettingsBehavior AllowSort="false" AllowSelectByRowClick="true" AllowFocusedRow="true" />
            <SettingsPager ShowEmptyDataRows="true" PageSize="31" Mode="ShowAllRecords"></SettingsPager>
            <SettingsEditing Mode="PopupEditForm" />
            <SettingsText PopupEditFormCaption="Add Detector" />
            <SettingsPopup>
                <EditForm Modal="true" VerticalAlign="TopSides" HorizontalAlign="LeftSides" Width="300px" />
            </SettingsPopup>
            <SettingsCommandButton>
                <UpdateButton ButtonType="Image">
                    <Image Url="../img/btn_add.gif" ToolTip="Add" />
                </UpdateButton>
                <CancelButton ButtonType="Image">
                    <Image Url="../img/btn_cancel.gif" ToolTip="Cancel" />
                </CancelButton>
            </SettingsCommandButton>
            <Images>
                <HeaderFilter Url="../img/2.gif" Width="8px"></HeaderFilter>
            </Images>
        </dx:ASPxGridView>

    </div>
    <div runat="server" id="Div1" style="border-style: solid; border-width: 1px; float:left; width:34%; height:50%; margin-right: 3px; margin-left: 2px;" >
    <dx:ASPxGridView ID="GridGroupDetail" runat="server" Width="100%" KeyFieldName="DB_NAME" 
            ClientInstanceName="GridGroupDetail" 
            EnableRowsCache="False" 
            SettingsEditing-Mode="Batch" 
            Settings-ShowColumnHeaders="False" 
            SettingsPager-PageSize="30" onhtmlrowcreated="GridGroupDetail_HtmlRowCreated" 
            onhtmlrowprepared="GridGroupDetail_HtmlRowPrepared" 
            onbatchupdate="GridGroupDetail_BatchUpdate" 
          
            >
            <Columns>
            <dx:GridViewDataTextColumn FieldName = "N3_OBID" VisibleIndex="1" Visible="false">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName = "DB_NAME" VisibleIndex="2" Visible="false" >
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName = "AUTH" Visible="false" >
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName = "DESC_NAME" VisibleIndex="3" ReadOnly="true" Width="35%" FixedStyle="Left">
                <EditFormSettings Visible="False" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataComboBoxColumn FieldName = "N3_VALUE"  VisibleIndex="4" Width="65%">
                <PropertiesComboBox DropDownStyle="DropDown" ValueType="System.String" DataSourceID="DDL_LOOKUPDET" ValueField="LOOKUPDETNAME" TextField="LOOKUPDETNAME" EnableSynchronization="False" IncrementalFilteringMode="StartsWith">
                <%--<ClientSideEvents EndCallback="Combo_EndCallback" />--%>
                </PropertiesComboBox>
            </dx:GridViewDataComboBoxColumn>
            </Columns>
            <ClientSideEvents FocusedRowChanged="GridGroupDetail_FocusedRowChanged" EndCallback="EndBatchUpdate" />
            <SettingsText Title="Detail of Detector" />
            <SettingsPager PageSize="30"></SettingsPager>
            <SettingsEditing Mode="Batch" BatchEditSettings-ShowConfirmOnLosingChanges="False" BatchEditSettings-StartEditAction="DblClick"></SettingsEditing>
            <Settings ShowStatusBar="Hidden" ShowTitlePanel="true" VerticalScrollableHeight="300" VerticalScrollBarMode="Auto" ></Settings>
            <SettingsBehavior AllowFocusedRow = "true" />
            <styles BatchEditModifiedCell-BackColor="White" BatchEditModifiedCell-ForeColor="Red">
                <header backcolor="#CCFF99"></header>
                <TitlePanel BackColor = "#B0C4DE" HorizontalAlign="Left" Paddings-PaddingLeft="10px" ForeColor="Black" Font-Bold="True"/>
                <FocusedRow BackColor = "#003399" />
            </styles>
        </dx:ASPxGridView>
    </div>
    <div style="float:left; width:22%; height:50%; border-style: solid; border-width: 1px;margin-left: 1px;">
        <dx:ASPxGridView ID="GridDetector" runat="server" Width="100%" KeyFieldName="KEY" 
            ClientInstanceName="GridDetector" 
            EnableRowsCache="False" 
            SettingsEditing-Mode="Batch" 
            Settings-ShowColumnHeaders="False" 
            SettingsBehavior-AllowSelectByRowClick="True" 
            onhtmlrowprepared="GridDetector_HtmlRowPrepared" OnHtmlRowCreated="GridDetector_HtmlRowCreated"
            OnBatchUpdate="GridDetector_BatchUpdate" 
            >
            <styles Header-HorizontalAlign="Center">
                <header backcolor="#CCFF99"></header>
                <TitlePanel BackColor ="#FFB6C1" Paddings-PaddingLeft="5px" ForeColor="Black" 
                    Font-Bold="True" HorizontalAlign="Left" >
                <Paddings PaddingLeft="5px"></Paddings>
                </TitlePanel>
                <GroupRow BackColor ="#FFCCCC"/>
                <Cell Font-Size="10px" />
                <BatchEditModifiedCell BackColor="White" ForeColor="Red" />
                <%--<PagerBottomPanel HorizontalAlign="Center" Paddings-Padding="0px" />--%>
            </styles>
            <Columns>
                <dx:GridViewDataTextColumn FieldName = "N1NAME" GroupIndex="0" 
                    Visible="False" VisibleIndex="0">
                </dx:GridViewDataTextColumn>
                <%--<dx:GridViewDataTextColumn FieldName = "SEQ"  VisibleIndex="4" Visible="False">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName = "RELDEFID" VisibleIndex="5" 
                    ReadOnly="true" Width="50%" Visible="False">
                    <EditFormSettings Visible="False" />
                </dx:GridViewDataTextColumn>--%>
                <dx:GridViewDataTextColumn FieldName = "KEY" VisibleIndex="1" Visible="false">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName = "OBID" VisibleIndex="2" Visible="false">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName = "N3_OBID" VisibleIndex="3" Visible="false">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName = "DB_NAME" VisibleIndex="4" Visible="false" >
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="DESC_NAME" VisibleIndex="5" ReadOnly="true">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="N3_VALUE" VisibleIndex="6">
                </dx:GridViewDataTextColumn>
                <dx:GridViewCommandColumn VisibleIndex="7" Width="2%" ButtonType="Image" ShowDeleteButton="True" Name="Command">
            <CellStyle Font-Size="Smaller"></CellStyle>
            </dx:GridViewCommandColumn>
            </Columns>
            <Templates>
                <GroupRowContent>
                    <%--<dx:ASPxLabel ID="lb_nested" runat="server" Text="<%#Container.GroupText%>" CssClass="NoStyle2"></dx:ASPxLabel>--%>
                    <dx:ASPxButton ID="btn_nested" HorizontalAlign="Left" runat="server" Image-Url="../img/btn_cancel1.gif" Image-Height="11px" OnClick="btn_nested_Click" Height="15.5px"
                    Image-Width="11px" ImagePosition="Right" CssClass="NoStyle2" Text="<%#Container.GroupText%>" Border-BorderStyle="None">
                    <ClientSideEvents Click="btn_delete_Click" /> 
                    </dx:ASPxButton>
                    
                </GroupRowContent>
            </Templates>
            <Settings ShowStatusBar="Hidden" ShowTitlePanel="true" GroupFormat="{1}" VerticalScrollableHeight="300" VerticalScrollBarMode="Auto" ></Settings>
            <SettingsBehavior AutoExpandAllGroups="True" ConfirmDelete="true"/>
            <SettingsEditing Mode="Batch" BatchEditSettings-ShowConfirmOnLosingChanges="False" BatchEditSettings-StartEditAction="DblClick"></SettingsEditing>
            <SettingsCommandButton>
                <DeleteButton>
                    <Image Url="../img/Collapse.gif" />
                </DeleteButton>
            </SettingsCommandButton>
            <%--<SettingsPager ShowEmptyDataRows="true" PageSize="31" Mode="ShowAllRecords">--%>
            <SettingsPager PageSize="25" Summary-Text="" Mode="ShowAllRecords"></SettingsPager>
            <SettingsText Title="Detector List" ConfirmDelete="Detector Delete?" CommandBatchEditUpdate="Save" CommandBatchEditCancel="Cancel"/>
            <ClientSideEvents EndCallback="EndBatchUpdate" />
            <%--<ClientSideEvents EndCallback="GridDetector_EndCallback" />--%>
            <Images ExpandedButton-Url="../img/2.gif" ExpandedButton-Width="10px" CollapsedButton-Url="../img/1.gif" CollapsedButton-Width="10px"/>
        </dx:ASPxGridView>
        
    </div>
        <div runat="server" id="taglist2" style="border-style: solid; border-width: 1px; float:left; width:57%; margin-right: 2px; margin-left: 2px;" >
        
        
        <dx:ASPxGridView ID="gv1temp" runat="server" Width="100%" ClientInstanceName="gv1temp" OnRowUpdating="gv1temp_RowUpdating"
           OnRowDeleting= "gv1temp_RowDeleting"  AutoGenerateColumns="False" KeyFieldName="OBID" OnInitNewRow= "gv1temp_InitNewRow">
            <%--<ClientSideEvents EndCallback="OnEndCallback_update" />--%>
            <ClientSideEvents EndCallback="OnEndCallback_update"  />
            <Columns>
            <%--<dx:GridViewDataTextColumn FieldName = "VOTING" GroupIndex="0">
                </dx:GridViewDataTextColumn>
             <dx:GridViewDataTextColumn FieldName = "ALARM_LEVEL" GroupIndex="0">
                </dx:GridViewDataTextColumn>--%>
            <dx:GridViewDataTextColumn FieldName = "EFFECT_GROUP" GroupIndex="0">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="TagNo" VisibleIndex="1" Width="17%">
            <CellStyle Font-Size="9px"></CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="DESC_NAME" Caption="Description" VisibleIndex="2" Width="30%">
            <CellStyle Font-Size="9px"></CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="LOC1" Caption="Loc1" VisibleIndex="3" Width="28%">
            <CellStyle Font-Size="9px"></CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="LOC2" Caption="Loc2" VisibleIndex="4" Width="18%">
            <CellStyle Font-Size="9px"></CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewCommandColumn VisibleIndex="5" ShowNewButtonInHeader="true" Width="4%" ButtonType="Image" ShowDeleteButton="True" Name="Command">
            <CellStyle Font-Size="Smaller"></CellStyle>
            </dx:GridViewCommandColumn>
            </Columns>
            
            <SettingsPager ShowEmptyDataRows="true" PageSize="31" Mode="ShowAllRecords"></SettingsPager>
            <Settings ShowTitlePanel="true" GroupFormat="{1}" VerticalScrollableHeight="292" VerticalScrollBarMode="Auto" ></Settings>
            <SettingsBehavior AllowSort="false" ConfirmDelete="true" AutoExpandAllGroups="true" />
            <SettingsText Title="Effect List" PopupEditFormCaption="Add Effect" ConfirmDelete="Delete?" />
            <SettingsCommandButton>
                <DeleteButton>
                    <Image Url="../img/Collapse.gif" />
                </DeleteButton>
                <NewButton>
                    <Image Url="../img/imageExpand.gif" ToolTip="Add" />
                </NewButton>
                <EditButton>
                <Image Url="../img/imageExpand.gif" />
                </EditButton>
                <UpdateButton>
                    <Image Url="../img/btn_add.gif" ToolTip="Add" />
                </UpdateButton>
            </SettingsCommandButton>
            <SettingsEditing Mode="PopupEditForm" />
            <SettingsPopup>
                <EditForm Width="600" Height ="400" VerticalAlign="TopSides" HorizontalAlign="LeftSides" VerticalOffset="-100" HorizontalOffset="-100" />
            </SettingsPopup>
            <Images ExpandedButton-Url="../img/2.gif" ExpandedButton-Width="7px" CollapsedButton-Url="../img/1.gif" CollapsedButton-Width="7px"/>
            <Styles>
                <Header BackColor ="#E6E6FA"/>
                <TitlePanel BackColor = "#B0C4DE" HorizontalAlign="Left" 
                    Paddings-PaddingLeft="10px" ForeColor="Black" Font-Bold="True" >
                <Paddings PaddingLeft="5px"></Paddings>
                </TitlePanel>
                <CommandColumn Spacing="2px" Wrap="False" />
                <BatchEditModifiedCell BackColor="White" ForeColor="Red" />
<%--                <GroupRow BackColor="Yellow" />--%>
            </Styles>
            <Templates>
            <EditForm>
            <dx:ASPxFormLayout runat="server" ID="Effect_Layout_Form" ColCount="3" SettingsItems-HorizontalAlign="Center">
            <Items>
                <dx:LayoutItem FieldName = "Alarm Level">
                    <LayoutItemNestedControlCollection>
                        <dx:LayoutItemNestedControlContainer>
                            <dx:ASPxComboBox ID="combo_alarmlevel" runat="server" Width="90px" >
                            <ClientSideEvents SelectedIndexChanged="OnSelectedIndexChanged" />
                                <Items>
                                    <dx:ListEditItem Text="" Value=" "/>
                                    <dx:ListEditItem Text="H" Value="H" />
                                    <dx:ListEditItem Text="HH" Value="HH" />
                                </Items>
                            </dx:ASPxComboBox>
                        </dx:LayoutItemNestedControlContainer>
                    </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                <dx:LayoutItem FieldName = "Voting">
                    <LayoutItemNestedControlCollection>
                        <dx:LayoutItemNestedControlContainer>
                            <dx:ASPxComboBox ID="combo_voting" runat="server" Width="90px" >
                                <Items>
                                    <dx:ListEditItem Text="Single" Value="S" Selected="true" />
                                    <dx:ListEditItem Text="Voting" Value="V" />
                                </Items>
                            </dx:ASPxComboBox>
                        </dx:LayoutItemNestedControlContainer>
                    </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                <dx:LayoutItem FieldName = "Voting Count">
                    <LayoutItemNestedControlCollection>
                        <dx:LayoutItemNestedControlContainer>
                            <dx:ASPxComboBox ID="combo_voting_cnt" runat="server" Width="90px">
                            <Items>
                                <dx:ListEditItem Text="1" Value="1" Selected="true"/>
                                <dx:ListEditItem Text="2" Value="2" />
                                <dx:ListEditItem Text="3" Value="3" />
                                <dx:ListEditItem Text="4" Value="4" />
                                <dx:ListEditItem Text="5" Value="5" />
                                <dx:ListEditItem Text="6" Value="6" />
                                <dx:ListEditItem Text="7" Value="7" />
                                <dx:ListEditItem Text="8" Value="8" />
                            </Items>
                            </dx:ASPxComboBox>
                        </dx:LayoutItemNestedControlContainer>
                    </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
            </Items>
          </dx:ASPxFormLayout>
  </EditForm>
            </Templates>
        </dx:ASPxGridView>

    </div>
        <%--<div style="float:left; width:18%; height:70%; margin-right:1px;">--%>
        
        </dx:PanelContent>
    </PanelCollection>
    </dx:ASPxCallbackPanel>

    <asp:SqlDataSource  ID="TBL_LOOKUPDET" runat="server"         
        SelectCommand="SELECT LOOKUPDETNAME, LOOKUPDETDESC FROM LOOKUPDET WHERE DEPENDENTPROJECT = @N1PROJECTNAME AND LOOKUPNAME = 'SYS_INTERFACE' AND TERMINATIONDATE = '' " 
        ConnectionString="<%$ ConnectionStrings:Admin_dataConnectionString %>" 
        ProviderName="<%$ ConnectionStrings:Admin_dataConnectionString.ProviderName %>">
        <SelectParameters>
            <asp:ControlParameter ControlID="hidden_prj_name" Name="N1PROJECTNAME" PropertyName="Value" Type="String"/>
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource  ID="DDL_LOOKUPDET" runat="server"         
        SelectCommand="SELECT LOOKUPDETNAME, LOOKUPDETDESC FROM LOOKUPDET WHERE DEPENDENTPROJECT = @N1PROJECTNAME AND LOOKUPNAME = @DB_NAME AND TERMINATIONDATE = '' ORDER BY LOOKUPDETNAME" 
        ConnectionString="<%$ ConnectionStrings:Admin_dataConnectionString %>" 
        ProviderName="<%$ ConnectionStrings:Admin_dataConnectionString.ProviderName %>">
        <SelectParameters>
            <asp:ControlParameter ControlID="hidden_prj_name" Name="N1PROJECTNAME" PropertyName="Value" Type="String"/>
            <asp:SessionParameter Name="DB_NAME" SessionField ="S_DB_NAME" Type="String" /> 
            <%--<asp:ControlParameter ControlID="CallbackPanel1$GridGroupDetail" DefaultValue="0" Name="DB_NAME" PropertyName="FocusedRowIndex" Type="String" />--%>
        </SelectParameters>
    </asp:SqlDataSource>
    <dx:ASPxGridViewExporter ID="GridViewExporter" runat="server" 
        GridViewID="GridGroupDetector">
    </dx:ASPxGridViewExporter>
    
    <br />
    <asp:HiddenField ID="tagType_HiddenField" runat="server" />
    <asp:HiddenField ID="Detector_type_HiddenField" runat="server" />
    <asp:HiddenField ID="hidden_prj_name" runat="server" />
    </form>
</body>
</html>
