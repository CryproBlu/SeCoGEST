<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ArticoliIntervento.ascx.cs" Inherits="SeCoGEST.Web.Interventi.ArticoliIntervento" %>
<%@ Register Src="~/Interventi/ArticoliInterventoEdit.ascx" TagPrefix="uc1" TagName="ArticoliInterventoEdit" %>

<telerik:RadScriptBlock runat="server">

    <script type="text/javascript">
        
        function PopUpShowing(sender, eventArgs) {            
            eventArgs.set_cancel(true);
        }

        function <%=NOME_FUNZIONE_MOSTRA_FINESTRA_DETTGALIO%>() {
            var rwArticoliInterventoEdit = $find('<%=rwArticoliInterventoEdit.ClientID%>');
            if (rwArticoliInterventoEdit != null && rwArticoliInterventoEdit.show) {
                rwArticoliInterventoEdit.show();
            }
        }

        function <%=NOME_FUNZIONE_CHIUSURA_FINESTRA_DETTGALIO_REBIND_GRIGLIA%>() {

            var rwArticoliInterventoEdit = $find('<%=rwArticoliInterventoEdit.ClientID%>');
            if (rwArticoliInterventoEdit != null && rwArticoliInterventoEdit.close) {
                rwArticoliInterventoEdit.close();
            }


            var rgGriglia = $find('<%=rgGriglia.ClientID%>');
            if (rgGriglia != null && rgGriglia.get_masterTableView) {
                var masterTableView = rgGriglia.get_masterTableView().rebind();
                if (masterTableView != null && masterTableView.rebind) {
                    masterTableView.rebind();
                }
            }
        }

    </script>

</telerik:RadScriptBlock>

<telerik:RadGrid runat="server"
    ID="rgGriglia"
    AllowPaging="false"
    AutoGenerateColumns="false"
    OnNeedDataSource="rgGriglia_NeedDataSource"
    OnItemCommand="rgGriglia_ItemCommand">
    <MasterTableView DataKeyNames="ID" TableLayout="Fixed" EditMode="PopUp" 
        Width="100%"
        GridLines="Both"
        NoMasterRecordsText="Nessun articolo registrato."
        AllowAutomaticInserts="false"
        AllowFilteringByColumn="false"
        AllowSorting="false"
        AllowMultiColumnSorting="false"
        CommandItemDisplay="Top">
        <Columns>
            <telerik:GridButtonColumn ButtonType="ImageButton"
                UniqueName="ColonnaModifica"
                HeaderStyle-Width="40"
                HeaderStyle-HorizontalAlign="Center"
                ItemStyle-HorizontalAlign="Center"
                ImageUrl="/UI/Images/Toolbar/edit.png"
                Text="Apri..."
                Resizable="false"
                CommandName="ModificaCommand" />

            <telerik:GridBoundColumn ItemStyle-Wrap="true"
                DataField="DescrizioneProvenienzaArticolo"
                HeaderText="Tipologia" />

            <telerik:GridBoundColumn ItemStyle-Wrap="true"
                DataField="CodiceContratto"
                HeaderText="Contratto" />

            <telerik:GridBoundColumn ItemStyle-Wrap="true"
                DataField="CodiceArticolo"
                HeaderText="Codice Articolo" />

            <telerik:GridBoundColumn ItemStyle-Wrap="true"
                DataField="Descrizione"
                HeaderText="Articolo" />

            <telerik:GridBoundColumn ItemStyle-Wrap="true"
                DataField="Note"
                HeaderText="Note" />
            
            <telerik:GridBoundColumn ItemStyle-Wrap="true"
                HeaderStyle-Width="120px"
                HeaderStyle-HorizontalAlign="Center"
                ItemStyle-HorizontalAlign="Center"
                DataField="Tempo_Quantita"
                HeaderText="Tempo/Q.tà" />
            
            <telerik:GridCheckBoxColumn
                HeaderStyle-Width="50px"
                HeaderStyle-HorizontalAlign="Center"
                ItemStyle-HorizontalAlign="Center"
                DataField="DaFatturare"
                HeaderText="Fatt." />

            <telerik:GridButtonColumn HeaderStyle-Width="40px" UniqueName="ColonnaElimina"
                Resizable="false"
                HeaderStyle-HorizontalAlign="Center"
                ItemStyle-Wrap="true"
                ItemStyle-HorizontalAlign="Center"
                ConfirmText="Eliminare l'Articolo selezionato?"
                ConfirmTextFields="CodiceArticolo,Descrizione"
                ConfirmTextFormatString="Eliminare l'Articolo '{0}' {1}?"
                ConfirmDialogType="RadWindow"
                ConfirmTitle="Elimina"
                ButtonType="ImageButton"
                ImageUrl="/UI/Images/Toolbar/delete.png"
                Text="Elimina..."
                CommandName="Delete" />
        </Columns>
        <CommandItemSettings AddNewRecordText="Aggiungi articolo..." CancelChangesText="Annulla"  SaveChangesText="Salva" ShowAddNewRecordButton="true" ShowCancelChangesButton="false" ShowSaveChangesButton="false" />
    </MasterTableView>
    
    <ClientSettings EnableRowHoverStyle="true">
        <Selecting AllowRowSelect="True" />
        <ClientEvents OnPopUpShowing="PopUpShowing"/>
    </ClientSettings>

</telerik:RadGrid>

<telerik:RadWindow ID="rwArticoliInterventoEdit" runat="server" Modal="true" CenterIfModal="true" VisibleOnPageLoad="false" Width="1200px" MinWidth="400px" Height="580px" MinHeight="520px" Behaviors="Close,Move,Maximize,Resize" Overlay="true" Style="z-index: 7001" DestroyOnClose="true">
    <ContentTemplate>        
        <uc1:ArticoliInterventoEdit runat="server" ID="ucArticoliInterventoEdit" OnSaved="ucArticoliInterventoEdit_OnSaved"  OnCanceled="ucArticoliInterventoEdit_OnCanceled"/>
    </ContentTemplate>
</telerik:RadWindow>

<div style="display: none;">
    <telerik:RadAjaxManagerProxy runat="server" ID="rampArticoliIntervento">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="rgGriglia">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rgGriglia" LoadingPanelID="RadAjaxLoadingPanelMaster" />
                    <telerik:AjaxUpdatedControl ControlID="RadWindowManagerMaster" />
                    <telerik:AjaxUpdatedControl ControlID="rwArticoliInterventoEdit" />
                    <telerik:AjaxUpdatedControl ControlID="ucArticoliInterventoEdit" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="rwArticoliInterventoEdit">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rgGriglia" LoadingPanelID="RadAjaxLoadingPanelMaster" />
                    <telerik:AjaxUpdatedControl ControlID="rwArticoliInterventoEdit" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManagerProxy>
</div>
