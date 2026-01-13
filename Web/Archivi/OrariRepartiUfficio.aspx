<%@ Page Title="Orari Reparto" Language="C#" MasterPageFile="~/UI/Main.Master" AutoEventWireup="true" CodeBehind="OrariRepartiUfficio.aspx.cs" Inherits="SeCoGEST.Web.Archivi.OrariRepartiUfficio" %>
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
	<asp:Label runat="server" ID="lblPageTitle" Text="Orari Reparto"></asp:Label><br />
	<asp:HyperLink runat="server" ID="hlMostraReparto" Text="Torna ai reparti..." NavigateUrl="/Archivi/RepartiUfficio.aspx" Visible="false" style="margin-top: 10px; color: white; font-size:small;"></asp:HyperLink>
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
		OnItemDataBound="rgArchiveItems_ItemDataBound"
		OnInsertCommand="rgArchiveItems_InsertCommand"
		OnUpdateCommand="rgArchiveItems_UpdateCommand"
		OnDeleteCommand="rgArchiveItems_DeleteCommand"
		SortingSettings-SortToolTip="Clicca qui per ordinare i dati in base a questa colonna"
		PagerStyle-FirstPageToolTip="Prima Pagina"
		PagerStyle-LastPageToolTip="Ultima Pagina"
		PagerStyle-PrevPageToolTip="Pagina Precedente"
		PagerStyle-NextPageToolTip="Pagina Successiva"
		PagerStyle-PagerTextFormat="{4} Elementi da {2} a {3} su {5}, Pagina {0} di {1}">
		<MasterTableView CommandItemDisplay="TopAndBottom" NoMasterRecordsText="<div style='padding: 15px; background-color: #EFCF02; width: 100%'><strong>Nessun orario configurato da visualizzare.</strong></div>"
			DataKeyNames="Id">
			<Columns>
				<telerik:GridEditCommandColumn HeaderStyle-Width="60px"></telerik:GridEditCommandColumn>
				<telerik:GridBoundColumn UniqueName="Giorno" HeaderText="Giorno" DataField="NomeDelGiorno"></telerik:GridBoundColumn>
				<telerik:GridDateTimeColumn UniqueName="OrarioDalle" HeaderStyle-Width="100px" HeaderText="Dalle" DataField="OrarioDalle" ItemStyle-HorizontalAlign="Center" DataFormatString="{0:hh\:mm}"></telerik:GridDateTimeColumn>
				<telerik:GridDateTimeColumn UniqueName="OrarioAlle" HeaderStyle-Width="100px" HeaderText="Alle" DataField="OrarioAlle" ItemStyle-HorizontalAlign="Center" DataFormatString="{0:hh\:mm}"></telerik:GridDateTimeColumn>
				<telerik:GridButtonColumn CommandName="Delete" Text="Elimina" UniqueName="columnDelete" HeaderStyle-Width="60px"
					ConfirmDialogType="RadWindow" ConfirmTitle="Conferma eliminazione"
					ConfirmText="Eliminare l'orario selezionato?"></telerik:GridButtonColumn>
			</Columns>
			<CommandItemSettings AddNewRecordText="Aggiungi nuovo valore" RefreshText="Aggiorna" />
			<EditFormSettings EditFormType="Template" EditColumn-CancelText="Annulla" EditColumn-InsertText="Aggiungi" EditColumn-UpdateText="Salva" >
				<FormTemplate>

					<!-- Edit template -->
					<div style="margin: 15px;">

						<!-- Page Messages -->
                        <uc1:PageMessage runat="server" ID="ArchiveMessages" Visible="false" />
						<!-- /Page Messages -->

<%--                        <telerik:RadTextBox runat="server" ID="rtbNome" MaxLength="100" Width="100%" Text='<%# Bind("Giorno") %>' TabIndex="0"></telerik:RadTextBox>--%>
						<telerik:RadDropDownList runat="server" ID="rddlGiorni"></telerik:RadDropDownList>
						<telerik:RadTimePicker runat="server" ID="rtpDalle" TimeView-Interval="00:15:00" TimeView-Columns="4"></telerik:RadTimePicker>
						<telerik:RadTimePicker runat="server" ID="rtpAlle" TimeView-Interval="00:15:00" TimeView-Columns="4"></telerik:RadTimePicker>

                        <div style="margin-top: 15px; padding-top: 15px; border-top: solid 1px silver;">
                            <telerik:RadButton runat="server" ID="btUpdate" ButtonType="SkinnedButton"
                                Text='<%# (Container is GridEditFormInsertItem) ? "Aggiungi" : "Salva" %>'
                                CommandName='<%# (Container is GridEditFormInsertItem) ? "PerformInsert" : "Update" %>'>
                            </telerik:RadButton>
                            &nbsp;

							<telerik:RadButton ID="bnCancel" Text="Annulla" runat="server"
								CausesValidation="False"
								CommandName="Cancel">
							</telerik:RadButton>
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
