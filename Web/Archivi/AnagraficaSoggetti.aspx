<%@ Page Title="Anagrafica Soggetti" Language="C#" MasterPageFile="~/UI/Main.Master" AutoEventWireup="true" CodeBehind="AnagraficaSoggetti.aspx.cs" Inherits="SeCoGEST.Web.Archivi.AnagraficaSoggetti" %>
<%@ Register Src="~/UI/PageMessage.ascx" TagName="PageMessage" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="title" runat="server">
    AnagraficaSoggetti
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
		AllowPaging="True" PageSize="20" AllowSorting="True" AutoGenerateColumns="False" ShowStatusBar="false"
		AllowAutomaticDeletes="False" AllowAutomaticInserts="False" AllowAutomaticUpdates="False"
		OnNeedDataSource="rgArchiveItems_NeedDataSource"
		SortingSettings-SortToolTip="Clicca qui per ordinare i dati in base a questa colonna"
		PagerStyle-FirstPageToolTip="Prima Pagina"
		PagerStyle-LastPageToolTip="Ultima Pagina"
		PagerStyle-PrevPageToolTip="Pagina Precedente"
		PagerStyle-NextPageToolTip="Pagina Successiva"
		PagerStyle-PagerTextFormat="{4} Elementi da {2} a {3} su {5}, Pagina {0} di {1}">
		<MasterTableView CommandItemDisplay="None" NoMasterRecordsText="<div style='padding: 15px; background-color: #EFCF02; width: 100%'><strong>Nessuna anagrafica da visualizzare.</strong></div>"
			DataKeyNames="Codice" AllowFilteringByColumn="true">
			<Columns>
				<telerik:GridHyperLinkColumn UniqueName="Codice" 
					HeaderText="Codice" 
					DataTextField="Codice" 
					DataNavigateUrlFields="Codice" 
					DataNavigateUrlFormatString="/Archivi/AnagraficaSoggettiProprieta.aspx?cod={0}"
					CurrentFilterFunction="Contains"
                    AutoPostBackOnFilter="true">
				</telerik:GridHyperLinkColumn>

				<telerik:GridBoundColumn UniqueName="RagioneSociale" 
					HeaderText="Ragione Sociale" 
					DataField="RagioneSociale"
					CurrentFilterFunction="Contains"
                    AutoPostBackOnFilter="true">
				</telerik:GridBoundColumn>

				<telerik:GridBoundColumn UniqueName="Indirizzo" 
					HeaderText="Indirizzo" 
					DataField="Indirizzo"
					CurrentFilterFunction="Contains"
                    AutoPostBackOnFilter="true">
				</telerik:GridBoundColumn>

				<telerik:GridBoundColumn UniqueName="CAP" 
					HeaderText="CAP" 
					DataField="CAP"
					CurrentFilterFunction="Contains"
                    AutoPostBackOnFilter="true">
				</telerik:GridBoundColumn>

				<telerik:GridBoundColumn UniqueName="Località" 
					HeaderText="Località" 
					DataField="Località"
					CurrentFilterFunction="Contains"
                    AutoPostBackOnFilter="true">
				</telerik:GridBoundColumn>

				<telerik:GridBoundColumn UniqueName="Provincia" 
					HeaderText="Provincia" 
					DataField="Provincia"
					CurrentFilterFunction="Contains"
                    AutoPostBackOnFilter="true">
				</telerik:GridBoundColumn>

				<telerik:GridBoundColumn UniqueName="DefaultVisibilitaTicketCliente" 
					HeaderText="Visibilita Ticket Cliente" 
					DataField="DefaultVisibilitaTicketCliente"
					CurrentFilterFunction="Contains"
                    AutoPostBackOnFilter="true">
				</telerik:GridBoundColumn>

            </Columns>

            <CommandItemSettings RefreshText="Aggiorna" ShowAddNewRecordButton="false" ShowRefreshButton="true" />
            <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" />

        </MasterTableView>

        <GroupingSettings casesensitive="false" />

	</telerik:RadGrid>

</asp:Content>
