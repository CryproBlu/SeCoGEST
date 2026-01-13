<%@ Page Title="Archivio Attività" Language="C#" MasterPageFile="~/UI/Main.Master" AutoEventWireup="true" CodeBehind="Attivita.aspx.cs" Inherits="SeCoGEST.Web.Archivi.Attivita" %>
<%@ Register Src="~/UI/PageMessage.ascx" TagName="PageMessage" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
	<telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
		<script type="text/javascript">
			function RowDblClick(sender, eventArgs) {
				sender.get_masterTableView().editItem(eventArgs.get_itemIndexHierarchical());
			}
		</script>
	</telerik:RadCodeBlock>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="title" runat="server">
    Archivio Attività
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="main" runat="server">

    <telerik:RadAjaxManagerProxy ID="RadAjaxProxy" runat="server">
		<AjaxSettings>
			<telerik:AjaxSetting AjaxControlID="rgArchiveItems">
				<UpdatedControls>
					<telerik:AjaxUpdatedControl ControlID="rgArchiveItems" LoadingPanelID="RadAjaxLoadingPanelMaster"></telerik:AjaxUpdatedControl>
				</UpdatedControls>
			</telerik:AjaxSetting>
		</AjaxSettings>
	</telerik:RadAjaxManagerProxy>

    <!-- Window manager -->
	<telerik:RadWindowManager RenderMode="Lightweight" ID="windowManager1" runat="server" EnableShadow="true" Style="border: solid 5px green; background-color:red; box-sizing: padding-box; z-index: 100001" Localization-Cancel="Annulla" ></telerik:RadWindowManager>
	<!-- /Window manager -->
    

	<telerik:RadGrid RenderMode="Lightweight" ID="rgArchiveItems" runat="server"
		GridLines="None" ClientSettings-Selecting-AllowRowSelect="true"
		AllowPaging="True" PageSize="10" AllowSorting="True" AutoGenerateColumns="False" ShowStatusBar="true"
		AllowAutomaticDeletes="False" AllowAutomaticInserts="False" AllowAutomaticUpdates="False"
		OnNeedDataSource="rgArchiveItems_NeedDataSource"
		OnItemCreated="rgArchiveItems_ItemCreated"
		OnItemCommand="rgArchiveItems_ItemCommand"
		OnInsertCommand="rgArchiveItems_InsertCommand"
		OnUpdateCommand="rgArchiveItems_UpdateCommand"
		OnDeleteCommand="rgArchiveItems_DeleteCommand"
		SortingSettings-SortToolTip="Clicca qui per ordinare i dati in base a questa colonna"
		PagerStyle-FirstPageToolTip="Prima Pagina"
		PagerStyle-LastPageToolTip="Ultima Pagina"
		PagerStyle-PrevPageToolTip="Pagina Precedente"
		PagerStyle-NextPageToolTip="Pagina Successiva"
		PagerStyle-PagerTextFormat="{4} Elementi da {2} a {3} su {5}, Pagina {0} di {1}">
		<MasterTableView CommandItemDisplay="TopAndBottom" NoMasterRecordsText="<div style='padding: 15px; background-color: #EFCF02; width: 100%'><strong>Nessuna Attività da visualizzare.</strong></div>"
			DataKeyNames="Id">
			<Columns>
				<telerik:GridEditCommandColumn HeaderStyle-Width="60px"></telerik:GridEditCommandColumn>

				<telerik:GridBoundColumn UniqueName="Descrizione" HeaderText="Descrizione" DataField="Descrizione"></telerik:GridBoundColumn>

				<telerik:GridButtonColumn CommandName="Delete" Text="Elimina" UniqueName="columnDelete" HeaderStyle-Width="60px"
					ConfirmDialogType="RadWindow" ConfirmTitle="Conferma eliminazione"
					ConfirmText="Eliminare la voce di archivio selezionata?"></telerik:GridButtonColumn>
			</Columns>
			<CommandItemSettings AddNewRecordText="Aggiungi nuovo valore" RefreshText="Aggiorna" />
			<EditFormSettings EditFormType="Template" EditColumn-CancelText="Annulla" EditColumn-InsertText="Aggiungi" EditColumn-UpdateText="Salva" >
				<FormTemplate>

					<!-- Edit template -->
					<div style="margin: 15px;">

						<!-- Page Messages -->
                        <uc1:PageMessage runat="server" ID="ArchiveMessages" Visible="false" />
						<!-- /Page Messages -->

						Descrizione:<br />
						<telerik:RadTextBox runat="server" ID="rtbNome" MaxLength="100" Width="100%" Text='<%# Bind("Descrizione") %>' TabIndex="0"></telerik:RadTextBox><br />
						
                        <div style="margin-top: 15px;">
							<telerik:RadButton runat="server" ID="btUpdate" ButtonType="SkinnedButton"
								Text='<%# (Container is GridEditFormInsertItem) ? "Aggiungi" : "Salva" %>'
								CommandName='<%# (Container is GridEditFormInsertItem) ? "PerformInsert" : "Update" %>'></telerik:RadButton>&nbsp;

							<telerik:RadButton ID="bnCancel" Text="Annulla" runat="server" 
								CausesValidation="False"
								CommandName="Cancel"></telerik:RadButton>
                        </div>
					</div>
					<!-- /Edit template -->

				</FormTemplate>
			</EditFormSettings>
		</MasterTableView>
		<ClientSettings>
			<ClientEvents OnRowDblClick="RowDblClick"></ClientEvents>
		</ClientSettings>
	</telerik:RadGrid>

</asp:Content>