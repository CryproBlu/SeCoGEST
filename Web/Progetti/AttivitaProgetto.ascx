<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AttivitaProgetto.ascx.cs" Inherits="SeCoGEST.Web.Progetti.AttivitaProgetto" %>
<%@ Register Src="~/UI/PageMessage.ascx" TagPrefix="uc1" TagName="PageMessage" %>

<style type="text/css">
    .noStandardStyle {
        border: none !important;
        background-color: transparent !important;
    }
</style>

<telerik:RadGrid ID="rgAttivita" runat="server" Skin="Windows7" Height="500px"
    GridLines="None" ClientSettings-Selecting-AllowRowSelect="true"
    AllowPaging="False" AllowSorting="False" AutoGenerateColumns="False" ShowStatusBar="False"
    AllowAutomaticDeletes="False" AllowAutomaticInserts="False" AllowAutomaticUpdates="False"
    OnNeedDataSource="rgAttivita_NeedDataSource"
    OnItemDataBound="rgAttivita_ItemDataBound"
    OnItemCreated="rgAttivita_ItemCreated"
    OnItemCommand="rgAttivita_ItemCommand"
    OnUpdateCommand="rgAttivita_UpdateCommand"
    OnDeleteCommand="rgAttivita_DeleteCommand">
    <ClientSettings>
        <ClientEvents OnCommand="RadGridCommand" OnPopUpShowing="rgAttivita_PopUpShowing" />
        <Scrolling AllowScroll="true" UseStaticHeaders="false" SaveScrollPosition="true" />
    </ClientSettings>

    <MasterTableView CommandItemDisplay="Top" DataKeyNames="ID" AllowFilteringByColumn="true" EditMode="PopUp" 
        NoMasterRecordsText="<div style='padding: 15px; background-color: #EFCF02; width: 100%'><strong>Nessuna Attività da visualizzare per questo Progetto.</strong></div>">
        
        <Columns>
            <telerik:GridEditCommandColumn HeaderStyle-Width="50px"></telerik:GridEditCommandColumn>

            <telerik:GridDateTimeColumn UniqueName="dataInserimento" 
                HeaderText="Inserimento" DataField="DataInserimento" DataFormatString="{0:dd/MM/yyyy}"
                HeaderStyle-Width="160px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                CurrentFilterFunction="EqualTo" AutoPostBackOnFilter="true">
            </telerik:GridDateTimeColumn>

            <telerik:GridBoundColumn UniqueName="Descrizione" 
                HeaderText="Descrizione" DataField="Descrizione" 
                HeaderStyle-Width="400px"
                CurrentFilterFunction="Contains" FilterControlWidth="350px" AutoPostBackOnFilter="true">
            </telerik:GridBoundColumn>

            <telerik:GridDateTimeColumn UniqueName="Scadenza" 
                HeaderText="Scadenza" DataField="Scadenza"
                HeaderStyle-Width="160px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                CurrentFilterFunction="EqualTo" AutoPostBackOnFilter="true">
            </telerik:GridDateTimeColumn>

            <telerik:GridBoundColumn UniqueName="AssegnatoA" 
                HeaderText="Operat. Assegnato" DataField="OperatoreAssegnatoCognomeNome" 
                HeaderStyle-Width="300px"
                CurrentFilterFunction="Contains" FilterControlWidth="250px" AutoPostBackOnFilter="true">
            </telerik:GridBoundColumn>

            <telerik:GridBoundColumn UniqueName="Esecutore"
                HeaderText="Operat. Esecutore" DataField="OperatoreEsecutoreCognomeNome" 
                HeaderStyle-Width="300px"
                CurrentFilterFunction="Contains" FilterControlWidth="250px" AutoPostBackOnFilter="true">
            </telerik:GridBoundColumn>

            <telerik:GridTemplateColumn UniqueName="Ticket" 
                HeaderText="N.Ticket" DataField="NumeroTicket"
                HeaderStyle-Width="200px"
                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                 CurrentFilterFunction="EqualTo" FilterControlWidth="150px" AutoPostBackOnFilter="true">
                <ItemTemplate>
                    <asp:HyperLink runat="server" ID="hlOpenTicket"></asp:HyperLink>
                </ItemTemplate>
            </telerik:GridTemplateColumn>

            <telerik:GridBoundColumn UniqueName="NoteContratto" 
                HeaderStyle-Width="400px"
                HeaderText="Note Contratto" DataField="NoteContratto" 
                CurrentFilterFunction="Contains" FilterControlWidth="350px" AutoPostBackOnFilter="true">
            </telerik:GridBoundColumn>

            <telerik:GridBoundColumn UniqueName="NoteOperatore" 
                HeaderStyle-Width="200px"
                HeaderText="Note Operatore" DataField="NoteOperatore" 
                CurrentFilterFunction="Contains" FilterControlWidth="150px" AutoPostBackOnFilter="true">
            </telerik:GridBoundColumn>

            <telerik:GridBoundColumn UniqueName="Allegati" 
                HeaderStyle-Width="300px"
                HeaderText="Allegati" DataField="NomiAllegati" 
                CurrentFilterFunction="Contains" FilterControlWidth="250px" AutoPostBackOnFilter="true">
            </telerik:GridBoundColumn>
            
            <telerik:GridBoundColumn UniqueName="Stato" 
                HeaderStyle-Width="200px"
                HeaderText="Stato" DataField="StatoString" 
                CurrentFilterFunction="Contains" FilterControlWidth="150px" AutoPostBackOnFilter="true">
            </telerik:GridBoundColumn>


            <telerik:GridButtonColumn HeaderStyle-Width="50px" UniqueName="ColonnaOrdinamentoSpostaSopra"
                Resizable="false"
                HeaderStyle-HorizontalAlign="Center"
                ItemStyle-Wrap="true"
                ItemStyle-HorizontalAlign="Center"
                ButtonType="ImageButton"
                ImageUrl="/UI/Images/Toolbar/16x16/ArrowUp.png"
                Text="Sposta verso l'alto"
                ShowInEditForm="false"
                CommandName="MoveUp" />

            <telerik:GridButtonColumn HeaderStyle-Width="50px" UniqueName="ColonnaOrdinamentoSpostaSotto"
                Resizable="false"
                HeaderStyle-HorizontalAlign="Center"
                ItemStyle-Wrap="true"
                ItemStyle-HorizontalAlign="Center"
                ButtonType="ImageButton"
                ImageUrl="/UI/Images/Toolbar/16x16/ArrowDown.png"
                Text="Sposta verso il basso"
                ShowInEditForm="false"
                CommandName="MoveDown" />

            <telerik:GridButtonColumn CommandName="Delete" Text="Elimina" UniqueName="columnDelete" 
                HeaderStyle-Width="50px" ItemStyle-Width="50px"
                ConfirmDialogType="RadWindow" ConfirmTitle="Conferma eliminazione"
                ConfirmText="Eliminare l'attività selezionata?">
            </telerik:GridButtonColumn>
        </Columns>

        <CommandItemSettings AddNewRecordText="Aggiungi nuova attività" RefreshText="Aggiorna" />

        <EditFormSettings EditFormType="Template" EditColumn-ItemStyle-BackColor="#bfcfe0"
            EditColumn-CancelText="Annulla" EditColumn-InsertText="Aggiungi" EditColumn-UpdateText="Salva" >

            <PopUpSettings 
                Modal="true"
                OverflowPosition="Center"
                ShowCaptionInEditForm="true"
                Width="1300px"
                Height="535px"
                ScrollBars="Auto" />

            <FormTemplate>

                <!-- Edit template -->
                <div style="margin: 0px; padding: 10px; background-color: #bfcfe0;">

                    <!-- Page Messages -->
                    <uc1:PageMessage runat="server" id="ArchiveMessages" visible="false" />
                    <!-- /Page Messages -->

                    <table border="0" style="width: 100%; border: none;">
                        <tr>
                            <td colspan="5" style="width: 100%;">
                                <Table border="0" style="width: 100%;">
                                    <tr>
                                        <td style="width: 110px;">
                                            Data inserimento<br />
                                            <telerik:RadDatePicker ID="rdpDataInserimento" runat="server" SelectedDate='<%# Bind("DataInserimento")%>'
                                                DatePopupButton-ToolTip="Seleziona una data dal calendario"
                                                DateInput-DisabledStyle-BackColor="#F5F5F5">
                                                <Calendar ID="Calendar1" runat="server">
                                                    <SpecialDays>
                                                        <telerik:RadCalendarDay ItemStyle-CssClass="rcToday" Repeatable="Today" />
                                                    </SpecialDays>
                                                </Calendar>
                                            </telerik:RadDatePicker>
                                        </td>
                                        <td>&nbsp;</td>
                                        <td style="width: 110px;">
                                            Data inizio<br />
                                            <telerik:RadDatePicker ID="rdpDataInizio" runat="server" SelectedDate='<%# Bind("DataInizio")%>'
                                                DatePopupButton-ToolTip="Seleziona una data dal calendario"
                                                DateInput-DisabledStyle-BackColor="#F5F5F5">
                                                <Calendar ID="Calendar2" runat="server">
                                                    <SpecialDays>
                                                        <telerik:RadCalendarDay ItemStyle-CssClass="rcToday" Repeatable="Today" />
                                                    </SpecialDays>
                                                </Calendar>
                                            </telerik:RadDatePicker>
                                        </td>
                                        <td style="width: 110px;">
                                            Orario inizio<br />
                                            <telerik:RadTimePicker runat="server" ID="rtpOraInizio" SelectedTime='<%# Bind("OraInizio")%>'> 
                                                <TimeView runat="server" Interval="00:15:00" Columns="4"></TimeView>
                                            </telerik:RadTimePicker>
                                        </td>
                                        <td style="width: 50px;">&nbsp;</td>
                                        <td style="width: 110px;">
                                            Data fine<br />
                                            <telerik:RadDatePicker ID="rdpDataFine" runat="server" SelectedDate='<%# Bind("DataFine")%>'
                                                DatePopupButton-ToolTip="Seleziona una data dal calendario"
                                                DateInput-DisabledStyle-BackColor="#F5F5F5">
                                                <Calendar ID="Calendar3" runat="server">
                                                    <SpecialDays>
                                                        <telerik:RadCalendarDay ItemStyle-CssClass="rcToday" Repeatable="Today" />
                                                    </SpecialDays>
                                                </Calendar>
                                            </telerik:RadDatePicker>
                                        </td>
                                        <td style="width: 80px;">
                                            Orario fine<br />
                                            <telerik:RadTimePicker runat="server" ID="rtpOraFine" SelectedTime='<%# Bind("OraFine")%>'>
                                                <TimeView runat="server" Interval="00:15:00" Columns="4"></TimeView>
                                            </telerik:RadTimePicker>
                                        </td>
                                    </tr>
                                </Table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">Descrizione Attività<br />
                                <telerik:RadTextBox runat="server" ID="rtbNome" MaxLength="100" Height="60px" Width="100%" TextMode="MultiLine" Text='<%# Bind("Descrizione") %>' TabIndex="0"></telerik:RadTextBox><br />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 180px; padding-top: 10px;">
                                Scadenza<br />
                                <telerik:RadDateTimePicker RenderMode="Lightweight" runat="server" ID="rdtpScadenza" Width="100%" SelectedDate='<%# Bind("Scadenza")%>'></telerik:RadDateTimePicker>
                            </td>
                            <td style="width: 335px; padding-top: 10px;">
                                Operatore Assegnato<br />
                                <telerik:RadComboBox ID="rcbOperatoreAssegnato" runat="server" Width="100%"
                                    AllowCustomText="false" 
                                    DataValueField="ID"
                                    DataTextField="CognomeNome" />
                            </td>
                            <td style="width: 335px; padding-top: 10px;">
                                Operatore Esecutore<br />
                                <telerik:RadComboBox ID="rcbOperatoreEsecutore" runat="server" Width="100%"
                                    AllowCustomText="false"
                                    DataValueField="ID"
                                    DataTextField="CognomeNome" />
                            </td>
                            <td style="padding-top: 10px;">
                                Ticket<br />
                                <telerik:RadComboBox ID="rcbTicket" runat="server" ShowWhileLoading="true"
                                    Width="100%" DropDownAutoWidth="Disabled" DropDownWidth="1000"
                                    MarkFirstMatch="false"
                                    HighlightTemplatedItems="true"
                                    EmptyMessage="Selezionare un Ticket"
                                    ItemsPerRequest="20"
                                    ShowMoreResultsBox="true"
                                    EnableLoadOnDemand="true"
                                    EnableItemCaching="false"
                                    AllowCustomText="true"
                                    LoadingMessage="Caricamento tickets in corso..."
                                    DataValueField="ID"
                                    DataTextField="Numero"
                                    OnItemsRequested="rcbTicket_ItemsRequested">
                                    <ItemTemplate>
                                        <telerik:RadPageLayout runat="server" Width="100%" HtmlTag="None" GridType="Fluid">
                                            <Rows>
                                                <telerik:LayoutRow RowType="Region">
                                                    <Columns>
                                                        <telerik:LayoutColumn Span="1">
                                                            <%# Eval("Numero") %>
                                                        </telerik:LayoutColumn>
                                                        <telerik:LayoutColumn Span="5">
                                                            <%# Eval("RagioneSociale") %>
                                                        </telerik:LayoutColumn>
                                                        <telerik:LayoutColumn Span="6">
                                                            <%# Eval("Oggetto") %>
                                                        </telerik:LayoutColumn>
                                                    </Columns>
                                                </telerik:LayoutRow>
                                            </Rows>
                                        </telerik:RadPageLayout>
                                    </ItemTemplate>
                                </telerik:RadComboBox>
                            </td>
                            <td style="padding-top: 10px;">
                                Stato<br />
                                <telerik:RadComboBox ID="rcbStato" runat="server" Width="100%" AllowCustomText="false" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5" style="padding-top: 10px;">Note Attività<br />
                                <telerik:RadTextBox runat="server" ID="rtbNoteContratto" Height="80px" Width="100%" TextMode="MultiLine" Text='<%# Bind("NoteContratto") %>' TabIndex="0"></telerik:RadTextBox><br />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5" style="padding-top: 10px;">Note Operatore<br />
                                <telerik:RadTextBox runat="server" ID="rtbNoteOperatore" Height="60px" Width="100%" TextMode="MultiLine" Text='<%# Bind("NoteOperatore") %>' TabIndex="0"></telerik:RadTextBox><br />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5" style="padding-top: 10px;">
                                Allegati
                                <table style="width: 100%; text-align: right;">
                                    <tr>
                                        <td style="vertical-align: top;">
                                            <telerik:RadAsyncUpload
                                                ID="rauAllegati"
                                                runat="server"
                                                MultipleFileSelection="Automatic"
                                                PostbackTriggers="btUpdate"
                                                MaxFileInputsCount="10"
                                                TemporaryFolder="~\App_Data\RadUploadTemp"
                                                Localization-Remove="Rimuovi" 
                                                OverwriteExistingFiles="false">
                                            </telerik:RadAsyncUpload>
                                        </td>
                                        <td style="vertical-align: top;">
                                            <telerik:RadAjaxPanel ID="rapAllegatiAttivita" runat="server" LoadingPanelID="RadAjaxLoadingPanelMaster">
                                                <asp:Repeater ID="rptAllegatiAttivita" runat="server" 
                                                    OnItemDataBound="rptAllegatiAttivita_ItemDataBound"
                                                    OnItemCommand="rptAllegatiAttivita_ItemCommand">
                                                    <ItemTemplate>
                                                        <asp:HiddenField runat="server" ID="hfIdAllegato" Value='<%# Eval("ID") %>' />
                                                        <asp:HyperLink runat="server" ID="hlDownload" Text='<%# Eval("NomeFile") %>' Target="_blank" ToolTip="Click per scaricre l'allegato" CssClass="noStandardStyle"></asp:HyperLink>
                                                        <asp:LinkButton runat="server" ID="lbRemoveItem" CssClass="noStandardStyle"
                                                            CommandName="Remove"
                                                            CommandArgument='<%# Eval("ID") %>'
                                                            OnClientClick="return confirm('Vuoi davvero rimuovere questo allegato?');"
                                                            ForeColor="Red"
                                                            Text="❌" />
                                                        <br />
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </telerik:RadAjaxPanel>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>


                    <div style="margin-top: 15px;">
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
    <GroupingSettings CaseSensitive="false" />
</telerik:RadGrid>


<telerik:RadAjaxPanel runat="server" id="rapWindow" OnAjaxRequest="rapWindow_AjaxRequest">
    <telerik:RadWindow RenderMode="Lightweight" runat="server" ID="SelezioneAttivitaWindow" DestroyOnClose="true" Modal="true" CenterIfModal="true" Behaviors="Maximize,Move,Resize,Close" Width="800px" >
        <ContentTemplate>
            <telerik:RadToolBar runat="server" ID="SelezioneAttivitaToolbar" Width="100%" OnButtonClick="SelezioneAttivitaToolbar_ButtonClick">
                <Items>
                    <telerik:RadToolBarButton runat="server" Text="Importa Attività Selezionate" CommandName="Import"></telerik:RadToolBarButton>
                    <telerik:RadToolBarButton runat="server" IsSeparator="True" />
                    <telerik:RadToolBarButton runat="server" Text="Annulla" CommandName="Cancel"></telerik:RadToolBarButton>
                </Items>
            </telerik:RadToolBar>
            <asp:Repeater runat="server" ID="repAttivita">
                <ItemTemplate>
                    <asp:CheckBox runat="server" ID="chkAttivita" Text='<%#Eval("DescrizioneAttivita") %>'/><br />
                </ItemTemplate>
            </asp:Repeater>
        </ContentTemplate>
    </telerik:RadWindow>
</telerik:RadAjaxPanel>


<telerik:RadAjaxManagerProxy runat="server">
    <AjaxSettings>
        <telerik:AjaxSetting AjaxControlID="rapWindow">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="rgAttivita" LoadingPanelID="RadAjaxLoadingPanelMaster" />
            </UpdatedControls>
        </telerik:AjaxSetting>
    </AjaxSettings>
</telerik:RadAjaxManagerProxy>


<telerik:RadScriptBlock runat="server">
    <script type="text/javascript"> 
        function RadGridCommand(sender, args) { 
            if (args.get_commandName() == "InitInsert") {
                args.set_cancel(true);
                var rap = $find("<%=rapWindow.ClientID%>");
                rap.ajaxRequest();
            }
        }
    </script>
</telerik:RadScriptBlock>

<telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <script type="text/javascript">
        function rgAttivita_PopUpShowing(sender, args) {

            var popUp = args.get_popUp();

            // Posizione fissa:
            popUp.style.position = "fixed";
            popUp.style.top = "50px";   // distanza dall'alto
            popUp.style.left = "100px"; // distanza da sinistra
            popUp.top = "50px";
            popUp.left = "100px";
            
            // correzione per centrarlo meglio orizzontalmente:
        //    var width = popUp.offsetWidth || 0;
        //    if (width > 0) {
        //        popUp.style.marginLeft = "-" + (width / 2) + "px";
        //    }
        }
    </script>
</telerik:RadCodeBlock>
