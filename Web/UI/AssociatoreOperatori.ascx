<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AssociatoreOperatori.ascx.cs" Inherits="SeCoGEST.Web.UI.AssociatoreOperatori" %>

<telerik:RadAjaxPanel runat="server" LoadingPanelID="RadAjaxLoadingPanelMaster">

	<telerik:RadGrid RenderMode="Lightweight" ID="rgItems" runat="server"
		GridLines="None" ClientSettings-Selecting-AllowRowSelect="true"
		AllowPaging="False" AllowSorting="False" AutoGenerateColumns="False" ShowStatusBar="False"
		AllowAutomaticDeletes="False" AllowAutomaticInserts="False" AllowAutomaticUpdates="False"
		OnNeedDataSource="rgItems_NeedDataSource"
		OnDeleteCommand="rgItems_DeleteCommand">
		<MasterTableView CommandItemDisplay="None" NoMasterRecordsText="Nessun Operatore associato da visualizzare."
			DataKeyNames="ID">
			<Columns>
				<telerik:GridBoundColumn UniqueName="Nome" HeaderText="Nome" DataField="CognomeNome"></telerik:GridBoundColumn>
				<telerik:GridButtonColumn CommandName="Delete" Text="Elimina" UniqueName="columnDelete" HeaderStyle-Width="50px"
					ConfirmDialogType="RadWindow" ConfirmTitle="Conferma rimozione"
					ConfirmText="Rimuovere l'operatore selezionato?"></telerik:GridButtonColumn>
			</Columns>
		</MasterTableView>
	</telerik:RadGrid>

	<table style="margin-top: 4px; width: 100%; border: 0px;" cellpadding="0" cellapscing="0" runat="server" id="tbAddNewOperator">
		<tr>
			<td>
				<div id="divNewOperator" style="visibility: hidden;">
					<telerik:RadComboBox ID="rcbNuovoOperatore" runat="server" Width="100%"
						AllowCustomText="false"
						DataValueField="ID"
						DataTextField="CognomeNome" />
				</div>
			</td>
			<td style="width: 50px; text-align: center;">
				<img id="imgShowDivNewOperator" src="/UI/Images/Toolbar/24x24/add.png" style="cursor: pointer;" onclick="showDivNewOperator()" title="Mostra Operatori da aggiungere" />
			
				<asp:ImageButton ID="imgButtonNewOperator" runat="server" ImageUrl="/UI/Images/Toolbar/24x24/add.png" ToolTip="Aggiungi Operatore selezionato" OnClick="imgButtonNewOperator_Click" style="visibility: hidden;"/>
			</td>
		</tr>
	</table>

</telerik:RadAjaxPanel>

<telerik:RadScriptBlock runat="server">
	<script type="text/javascript">
		function showDivNewOperator() {
			var div = document.getElementById("divNewOperator");
			var add = document.getElementById("imgShowDivNewOperator");
			var btn = document.getElementById('<%=imgButtonNewOperator.ClientID%>');
			if (div != null) {
				div.style.visibility = "visible";
			}
			if (add != null) {
				add.style.visibility = "hidden";
				add.style.display = "none";
			}
			if (btn != null) {
				btn.style.visibility = "visible";
			}
		}
	</script>
</telerik:RadScriptBlock>
