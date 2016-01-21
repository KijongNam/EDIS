<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FDMSystem_Template.aspx.cs" Inherits="EDISON.FDMSystem" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
        <style type="text/css">
        .NoStyle
        {
            background-image: none !important;
            background-color: transparent !important;
            border: none !important;
            float: right !important;
        }
    </style>
    <script type="text/javascript">
        function GridDblClick(s, e) {
            CallbackPanel1.PerformCallback();
        }
        function OnEndCallback(s, e) {
            CallbackPanel1.PerformCallback();
        }
    </script>
    <style type="text/css">
        .style1
        {
            height: 25px;
            width: 164px;
        }
        </style>
</head>
<body>
    <form id="form1" runat="server">
    <div runat="server" id="dv" style="border-style: solid; border-width: 1px; float:left; width:700px; height:100%;">
    <table>
        <tr >
        <td align="left" valign="top" class="style1">
        <asp:DropDownList ID="ddl_System" runat="server" Width="100px"  Height="21px" 
            Align="Middle" BackColor="White" AutoPostBack="True">
        </asp:DropDownList>
        <asp:HiddenField ID="aa11" runat="server" />         

        </td>
        
        <td  align="left" valign="middle"  >
            <span id="Label3" style=" width: 300px;  vertical-align: middle; font-weight: bold; font-size: 9pt; color: black; font-family: Arial;">                   System Name : </span> 
             </td>
        <td  align="left" valign="middle"  >
            <dx:ASPxTextBox ID="tb_sys_name" runat="server" Width="100px">    </dx:ASPxTextBox>
             </td>
        <td  align="left" valign="middle" >
            <dx:ASPxButton ID="btn_add" runat="server" AutoPostBack="False" AllowFocus="False" OnClick="btn_Add_Click"
                        CssClass="NoStyle" EnableTheming="False" >
                    <ClientSideEvents Click="OnEndCallback" />
                    <Image Url="../img/btn_add.gif">
                    </Image>
                <FocusRectPaddings Padding="0px" />
            </dx:ASPxButton>
             </td>
        <td  align="left" valign="middle" " >
            <dx:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="False" AllowFocus="False" OnClick="btn_delete_Click"
                        CssClass="NoStyle" EnableTheming="False" >
                <ClientSideEvents Click="OnEndCallback" />
                    <Image Url="../img/btn_delete.gif">
                    </Image>
                <FocusRectPaddings Padding="0px" />
            </dx:ASPxButton>
        </td>
<%--        <td style="  float:right;" align="right" valign="top">
                <dx:ASPxButton ID="ASPxButton2" runat="server" AutoPostBack="False" AllowFocus="False" OnClick="btn_delete_Click"
                        CssClass="NoStyle" EnableTheming="False" >
                <ClientSideEvents Click="OnEndCallback" />
                    <Image Url="../img/btn_delete.gif">
                    </Image>
                <FocusRectPaddings Padding="0px" />
                </dx:ASPxButton>

        </td>--%>
        </tr>
    </table>
    </div>
    <dx:ASPxCallbackPanel runat="server" ID="CallbackPanel1" ClientInstanceName="CallbackPanel1"> 
    <PanelCollection>
        <dx:PanelContent>
        <div runat="server" id="taglist" style="border-style: solid; border-width: 1px; float:left; width:700px; height:100%;">
      

        <dx:ASPxGridView ID="CauseGrid" runat="server" Width="100%" 
            ClientInstanceName="Detector_Grid" KeyFieldName="OBID" OnRowDeleting = "Cause_Deleting" OnRowInserting = "Cause_Inserting"  
            SettingsEditing-Mode="Batch" SettingsPager-PageSize="30"  onbatchupdate="GridGroupDetail_BatchUpdate"  > 
                <ClientSideEvents EndCallback="OnEndCallback" RowDblClick="GridDblClick"  />
                <SettingsBehavior AllowSort="false"/>

                <ClientSideEvents RowDblClick="GridDblClick" />
                <Columns>
                    <dx:GridViewDataTextColumn FieldName = "DESCRIPTION" Caption="Cause Description" VisibleIndex="0"/>
                    <dx:GridViewDataTextColumn FieldName = "SEQ" Caption="Seq" VisibleIndex="1"/>
                    <dx:GridViewCommandColumn VisibleIndex="2" ShowNewButtonInHeader="true" Width="3%" ButtonType="Image"  ShowDeleteButton="True" Name="Command"/>
                                                   
                 </Columns>
                 <SettingsBehavior AllowSort="false" ConfirmDelete="true" />
                 <SettingsText PopupEditFormCaption="Add Effect" ConfirmDelete="Delete?" />           
                 <SettingsEditing EditFormColumnCount="2" />
                 <SettingsEditing Mode="Batch" BatchEditSettings-ShowConfirmOnLosingChanges="False" BatchEditSettings-StartEditAction="DblClick"></SettingsEditing>
           
                 <SettingsCommandButton>
                    <DeleteButton><Image Url="../img/Collapse.gif" /></DeleteButton>
                    <NewButton><Image Url="../img/imageExpand.gif" ToolTip="Add"  /></NewButton>
                    <EditButton><Image Url="../img/imageExpand.gif" /></EditButton>
                    <UpdateButton><Image Url="../img/btn_add.gif" ToolTip="Add" /></UpdateButton> 
                    <CancelButton><Image Url="../img/btn_cancel.gif" ToolTip="Cancle"/></CancelButton>                                           
                 </SettingsCommandButton>
                 <Templates>
                    <DetailRow>
                   
                         <dx:ASPxGridView ID="EffectGrid" runat="server" Width="100%" KeyFieldName="OBID" OnBeforePerformDataSelect="Detail_BeforePerformDataSelect" 
                         onRowInserting = "Effect_Inserting" OnRowDeleting ="Effect_Deleting" onbatchupdate="Detail_BatchUpdate" >           
                                <ClientSideEvents EndCallback="OnEndCallback" />
                                <Columns>
                                    <dx:GridViewDataTextColumn FieldName = "DESCRIPTION" Caption="Effect Description"  VisibleIndex="0" Width = "400px">
                                    </dx:GridViewDataTextColumn> 
                                    <dx:GridViewDataTextColumn FieldName = "LOC1" Caption="Location1"  VisibleIndex="1" Width = "400px">
                                    </dx:GridViewDataTextColumn>     
                                    <dx:GridViewDataTextColumn FieldName = "LOC2" Caption="Location2"  VisibleIndex="2" Width = "400px">
                                    </dx:GridViewDataTextColumn>                                     
                                    <dx:GridViewCommandColumn VisibleIndex="2" ShowNewButtonInHeader="true" Width="3%" ButtonType="Image" ShowDeleteButton="True" Name="Command">
                                    <CellStyle Font-Size="Smaller"></CellStyle>
                                    </dx:GridViewCommandColumn>
                                </Columns>
                                    <SettingsBehavior AllowSort="false" ConfirmDelete="true" />
                                    <SettingsText ConfirmDelete="Delete?" />
                                    <SettingsCommandButton>
                                        <DeleteButton><Image Url="../img/Collapse.gif" ToolTip = "Delete" /></DeleteButton>
                                        <NewButton><Image Url="../img/imageExpand.gif" ToolTip="Add" /></NewButton>
                                        <EditButton><Image Url="../img/imageExpand.gif" /></EditButton>
                                        <UpdateButton><Image Url="../img/btn_add.gif" ToolTip="Add" /></UpdateButton>
                                        <CancelButton><Image Url="../img/btn_cancel.gif" ToolTip="Cancle"/></CancelButton>   
                                    </SettingsCommandButton> 
                                    <SettingsText Title="Effect List" />
                                    <Styles>
                                        <Header BackColor ="#E6E6FA"/>
                                        <TitlePanel BackColor = "#B0C4DE" HorizontalAlign="Left" Paddings-PaddingLeft="10px" ForeColor="Black" Font-Bold="True" >
                                            <Paddings PaddingLeft="10px"></Paddings>
                                        </TitlePanel>
                                        <CommandColumn Spacing="2px" Wrap="False" />
                                    </Styles>
                                    <SettingsDetail IsDetailGrid="True" />            
                                     <SettingsEditing Mode="Batch" BatchEditSettings-ShowConfirmOnLosingChanges="False" BatchEditSettings-StartEditAction="DblClick"></SettingsEditing>
                        </dx:ASPxGridView>



                     </DetailRow>
                 </Templates>
                 <SettingsDetail ShowDetailRow="true" />
                 <Styles>
                     <DetailCell>
                         <Paddings Padding="5px" />
                     </DetailCell>
                 </Styles>
           </dx:ASPxGridView>  
      

        </div>
        </dx:PanelContent>
    </PanelCollection>
    </dx:ASPxCallbackPanel>


    </form>
</body>
</html>
