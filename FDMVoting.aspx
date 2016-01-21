<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FDMVoting.aspx.cs" Inherits="EDISON.FDMVoting" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">

td {
	font-family:"Arial";
	font-size: 12px;
	font-style: normal;
	line-height: 120%;
	font-weight: normal;
	font-variant: normal;
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
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <table>
        <tr>
        <td style="width:500px; height:25px;" align="left">
            <asp:DropDownList ID="ddl_zone" runat="server" Width="100px"  Height="21px" 
             Align="Middle" BackColor="White" AutoPostBack="True"   >
            </asp:DropDownList>
            <asp:HiddenField ID="tagType_HiddenField" runat="server" />
         </td>
        </tr>
        </table>
    </div>  

 
    <dx:ASPxCallbackPanel runat="server" ID="CallbackPanel1" ClientInstanceName="CallbackPanel1" Height="185px">
    <PanelCollection>
        <dx:PanelContent>



       <div runat="server" id="taglist" style="border-style: solid; border-width: 1px; float:left; width:50%; height:688px;">
           <dx:ASPxGridView ID="Detector_Grid" runat="server" Width="100%" 
            ClientInstanceName="Detector_Grid" OnRowInserting = "DetectorVoting_Inserting" OnRowDeleting= "DetectorVoting_Deleting" 
               RowDblClick="GridDblClick" KeyFieldName="OBID"  >
                <ClientSideEvents EndCallback="OnEndCallback" RowDblClick="GridDblClick" />
                <SettingsBehavior AllowSort="false"  />
                <ClientSideEvents RowDblClick="GridDblClick" />
                <Columns>
                    <dx:GridViewDataTextColumn FieldName = "TYPE1" Caption="First Type"  VisibleIndex="0" >
                        <EditItemTemplate>                    
                            <asp:DropDownList ID="TYPE1_COMBO" runat="server"  Width="100px" Height="21px" 
                                  Align="Middle" OnLoad = "Setup_Type" >
                             </asp:DropDownList>
                        </EditItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName = "Voting1" Caption="First Voting" VisibleIndex="1">
                        <EditItemTemplate>
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
                        </EditItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName = "TYPE2" Caption="Second Type" VisibleIndex="2">
                            <EditItemTemplate>                                
                                     <asp:DropDownList ID="TYPE2_COMBO" runat="server"  Width="100px" Height="21px" Align="Middle" OnLoad = "Setup_Type" >
                            </asp:DropDownList>
                                    </EditItemTemplate>

                             </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName = "Voting2" Caption="Second Voting" VisibleIndex="3">
                      <EditItemTemplate>
                            <dx:ASPxComboBox ID="combo_voting2_cnt" runat="server" Width="90px">
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
                        </EditItemTemplate>
                    </dx:GridViewDataTextColumn>  
                    <dx:GridViewCommandColumn VisibleIndex="4" ShowNewButtonInHeader="true" Width="3%" ButtonType="Image" ShowDeleteButton="True" Name="Command">
                    <CellStyle Font-Size="Smaller"></CellStyle>
                    </dx:GridViewCommandColumn>          
                   
                 </Columns>
                 <SettingsBehavior AllowSort="false" ConfirmDelete="true" />
                 <SettingsText PopupEditFormCaption="Add Effect" ConfirmDelete="Delete?" />           
                 <SettingsEditing EditFormColumnCount="2" />
                 <SettingsCommandButton>
                    <DeleteButton><Image Url="../img/Collapse.gif" /></DeleteButton>
                    <NewButton><Image Url="../img/imageExpand.gif" ToolTip="Add"  /></NewButton>
                    <EditButton><Image Url="../img/imageExpand.gif" /></EditButton>
                    <UpdateButton><Image Url="../img/btn_add.gif" ToolTip="Add" /></UpdateButton> 
                    <CancelButton><Image Url="../img/btn_cancel.gif" ToolTip="Cancle"/></CancelButton>                                           
                 </SettingsCommandButton>
                 <Templates>
                    <DetailRow>
                   


                         <dx:ASPxGridView ID="Effect_Grid" runat="server" Width="100%" KeyFieldName="OBID"  OnRowInserting = "Effect_Inserting"                         
                            ClientInstanceName="Effect_Grid" OnRowDeleting ="Effect_Deleting"  OnBeforePerformDataSelect="gvDetail_BeforePerformDataSelect">                             
                                <ClientSideEvents EndCallback="OnEndCallback" RowDblClick="GridDblClick" />
                                <Columns>
                                    <dx:GridViewDataTextColumn FieldName = "TAG" Caption="Tag No"  VisibleIndex="0" >
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName = "DESC_NAME" Caption="Description" VisibleIndex="1">
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName = "ACTION_STATUS" Caption="Action" VisibleIndex="3">
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName = "SYS_INTERFACE" Caption="Sys. Interface" VisibleIndex="2">
                                    </dx:GridViewDataTextColumn>                                    
                                    <dx:GridViewCommandColumn VisibleIndex="4" ShowNewButtonInHeader="true" Width="3%" ButtonType="Image" ShowDeleteButton="True" Name="Command">
                                    <CellStyle Font-Size="Smaller"></CellStyle>
                                    </dx:GridViewCommandColumn>
                                    </Columns>
                                    <SettingsBehavior AllowSort="false" ConfirmDelete="true" />
                                    <SettingsText PopupEditFormCaption="Add Effect" ConfirmDelete="Delete?" />
                                    <SettingsCommandButton>
                                        <DeleteButton><Image Url="../img/Collapse.gif" ToolTip = "Delete" /></DeleteButton>
                                        <NewButton><Image Url="../img/imageExpand.gif" ToolTip="Add" /></NewButton>
                                        <EditButton><Image Url="../img/imageExpand.gif" /></EditButton>
                                        <UpdateButton><Image Url="../img/btn_add.gif" ToolTip="Add" /></UpdateButton>
                                    </SettingsCommandButton> 
                                    <SettingsText Title="Effect List" />
                                    <SettingsEditing Mode="PopupEditForm" />
                                    <SettingsPopup>
                                        <EditForm Width="550" Height ="400" />
                                    </SettingsPopup>
                                    <Styles>
                                        <Header BackColor ="#E6E6FA"/>
                                        <TitlePanel BackColor = "#B0C4DE" HorizontalAlign="Left" Paddings-PaddingLeft="10px" ForeColor="Black" Font-Bold="True" >
                                            <Paddings PaddingLeft="10px"></Paddings>
                                        </TitlePanel>
                                        <CommandColumn Spacing="2px" Wrap="False" />
                                    </Styles>
                                    <SettingsDetail IsDetailGrid="True" />            
                        </dx:ASPxGridView>



                     </DetailRow>
                 </Templates>
                 <SettingsDetail ShowDetailRow="true" />
           </dx:ASPxGridView>  
      

          </div>
  
    

             </dx:PanelContent>
    </PanelCollection>
    </dx:ASPxCallbackPanel>
    </form>
</body>
</html>
