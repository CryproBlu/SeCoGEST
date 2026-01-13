<%@ Page Title="Configurazione Proprietà Clienti" Language="C#" MasterPageFile="~/UI/Main.Master" AutoEventWireup="true" CodeBehind="AnagraficaSoggettiProprieta.aspx.cs" Inherits="SeCoGEST.Web.Archivi.AnagraficaSoggettiProprieta" %>
<%@ Register Src="~/UI/PageMessage.ascx" TagName="PageMessage" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">    
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="title" runat="server">
    Configurazione Proprietà Clienti
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
    
	<div style="width:100%; padding: 15px; box-sizing: border-box;">
		<asp:Label ID="Label1" runat="server" Text="Cliente" AssociatedControlID="rcbCliente" /><br />
		<telerik:RadComboBox ID="rcbCliente" runat="server" ShowWhileLoading="true"
			Width="100%" DropDownAutoWidth="Disabled"
			MarkFirstMatch="false"
			HighlightTemplatedItems="true"
			EmptyMessage="Selezionare un Soggetto"
			ItemsPerRequest="20"
			ShowMoreResultsBox="true"
			EnableLoadOnDemand="true"
			EnableItemCaching="false"
			AllowCustomText="true"
			LoadingMessage="Caricamento in corso..."
			DataValueField="CODCONTO"
			DataTextField="DSCCONTO1"
			OnItemsRequested="rcbCliente_ItemsRequested"
			OnSelectedIndexChanged="rcbCliente_SelectedIndexChanged" AutoPostBack="true">
			<ItemTemplate>
				<telerik:RadPageLayout runat="server" Width="100%" HtmlTag="None" GridType="Fluid">
					<Rows>
						<telerik:LayoutRow RowType="Region">
							<Columns>
								<telerik:LayoutColumn Span="1">
									<%# Eval("CODCONTO") %>
								</telerik:LayoutColumn>
								<telerik:LayoutColumn Span="11">
									<%# Eval("DSCCONTO1") %>
								</telerik:LayoutColumn>
							</Columns>
						</telerik:LayoutRow>
					</Rows>
				</telerik:RadPageLayout>
			</ItemTemplate>
		</telerik:RadComboBox>
	</div>

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
		<MasterTableView CommandItemDisplay="TopAndBottom" NoMasterRecordsText="<div style='padding: 15px; background-color: #EFCF02; width: 100%'><strong>Nessuna Proprietà da visualizzare per il soggetto selezionato.</strong></div>"
			DataKeyNames="CodiceCliente,Proprietà">
			<Columns>
				<telerik:GridEditCommandColumn HeaderStyle-Width="60px"></telerik:GridEditCommandColumn>
				<telerik:GridBoundColumn UniqueName="columnProprietà" HeaderText="Proprietà" DataField="Proprietà"></telerik:GridBoundColumn>
				<telerik:GridBoundColumn UniqueName="columnValore" HeaderText="Valore" DataField="Valore"></telerik:GridBoundColumn>
				<telerik:GridButtonColumn CommandName="Delete" Text="Elimina" UniqueName="columnDelete" HeaderStyle-Width="60px"
					ConfirmDialogType="RadWindow" ConfirmTitle="Conferma eliminazione"
					ConfirmText="Eliminare il valore della proprietà selezionata per questo soggetto?"></telerik:GridButtonColumn>
			</Columns>
			<CommandItemSettings AddNewRecordText="Aggiungi nuovo valore" RefreshText="Aggiorna" />
			<EditFormSettings EditFormType="Template" EditColumn-CancelText="Annulla" EditColumn-InsertText="Aggiungi" EditColumn-UpdateText="Salva" >
				<FormTemplate>

					<!-- Edit template -->
					<div style="margin: 15px;">

						<!-- Page Messages -->
						<uc1:PageMessage runat="server" ID="ArchiveMessages" Visible="false" />
						<!-- /Page Messages -->

						<telerik:RadAjaxPanel runat="server" ID="ajaxPanel">
							Proprietà:<br />
							<telerik:RadDropDownList runat="server" ID="rddlProprieta" TabIndex="0" Width="500px"></telerik:RadDropDownList><br />
							<br /><br />
							<telerik:RadCheckBox runat="server" ID="rchkValore" Text="Valore" Value='<%# Bind("Valore") %>' TabIndex="1" AutoPostBack="false"></telerik:RadCheckBox>
<%--							<telerik:RadTextBox runat="server" ID="rtbValore" MaxLength="255" Width="100%" Text='<%# Bind("Valore") %>' TabIndex="1"></telerik:RadTextBox>--%>
						</telerik:RadAjaxPanel>

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
	</telerik:RadGrid>

</asp:Content>
