<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OperatoriIntervento.ascx.cs" Inherits="SeCoGEST.Web.Interventi.OperatoriIntervento" %>

<telerik:RadScriptBlock runat="server">

    <script type="text/javascript">
        function DateSelected(data1ClientID, data2ClientID, txtClientID) {
            var timeFrom = $find(data1ClientID);
            var varFrom = timeFrom.get_selectedDate();

            var timeTo = $find(data2ClientID);
            var varTo = timeTo.get_selectedDate();

            var txtTo = $find(txtClientID);

            if (varFrom != null && varTo != null) {
                txtTo.set_value(daysBetween(varFrom, varTo));
            }
        }

        function daysBetween(date1, date2) {
            //Get 1 day in milliseconds
            var one_day = 1000 * 60 * 60 * 24;

            // Convert both dates to milliseconds
            var date1_ms = date1.getTime();
            var date2_ms = date2.getTime();
            var varSeconds = ((date2_ms - date1_ms) / 1000);
            var varMinutes = varSeconds / 60;
            return varMinutes;
        }


        function ImpostaInizioIntervento(chkPresaInCarico, rdtpInizioIntervento) {
            if (chkPresaInCarico.checked) {
                var rdtpInizioIntervento = $find(rdtpInizioIntervento);
                if (rdtpInizioIntervento) {
                    if (rdtpInizioIntervento.isEmpty()) {
                        rdtpInizioIntervento.set_selectedDate(new Date())
                    }
                }
            }
        }

    </script>

</telerik:RadScriptBlock>

<telerik:RadGrid runat="server"
    ID="rgGriglia"
    Width="100%"
    AllowPaging="false"
    AutoGenerateColumns="false"
    OnPreRender="rgGriglia_PreRender"
    OnNeedDataSource="rgGriglia_NeedDataSource"
    OnItemCreated="rgGriglia_ItemCreated"
    OnItemDataBound="rgGriglia_ItemDataBound"
    OnItemCommand="rgGriglia_ItemCommand"
    OnDeleteCommand="rgGriglia_DeleteCommand" >   

    <MasterTableView DataKeyNames="ID"
        TableLayout="Fixed"
        Width="100%"
        GridLines="Both"

        CommandItemDisplay="None" 
        CommandItemSettings-AddNewRecordText="Aggiungi operatore..."
        CommandItemSettings-ShowAddNewRecordButton="false"
        CommandItemSettings-ShowRefreshButton="false"
        
        EnableHeaderContextMenu="false" 
        EnableHeaderContextAggregatesMenu="false" 
        EnableHeaderContextFilterMenu="false" 
        
        AllowFilteringByColumn="false"
        AllowSorting="false"
        AllowMultiColumnSorting="false"

        NoMasterRecordsText="Nessun operatore da visualizzare"
        NoDetailRecordsText="Nessun operatore da visualizzare" >

        <Columns>
            <telerik:GridBoundColumn UniqueName="RowIndex" 
                ItemStyle-CssClass='<% if (!Enabled) { return "NessunBordo noBorder";} else { return "";} %>'
                HeaderStyle-Width="40"
                HeaderStyle-HorizontalAlign="Center"
                ItemStyle-Wrap="true"
                ItemStyle-HorizontalAlign ="Center"
                ItemStyle-VerticalAlign="Top"
                Resizable="false"
                Exportable="false"/>

            <telerik:GridTemplateColumn ItemStyle-Wrap="true"
                ItemStyle-CssClass='<% if (!Enabled) { return "NessunBordo noBorder";} else { return "";} %>'
                HeaderStyle-Width="200"
                ItemStyle-VerticalAlign="Top"
                DataField="IDOperatore" 
                HeaderText="Operatore">
                <ItemTemplate>
                    <% if (!Enabled) { %>
                        <span style="padding: 7px;"><%# Eval("Operatore.CognomeNome")%></span>
                    <% } else { %>
                        <asp:HiddenField runat="server" ID="hdIdOperatore" />
                        <asp:Label runat="server" ID="lblOperatore"></asp:Label>
                        <telerik:RadComboBox ID="rcbOperatori" runat="server" Width="100%"
                            AllowCustomText="false" 
                            DataValueField="ID"
                            DataTextField="CognomeNome"/>
                    <% } %>
                </ItemTemplate>
            </telerik:GridTemplateColumn>

            <telerik:GridTemplateColumn ItemStyle-Wrap="true"
                ItemStyle-CssClass='<% if (!Enabled) { return "NessunBordo noBorder";} else { return "";} %>'
                HeaderStyle-Width="180"
                ItemStyle-VerticalAlign="Top"
                DataField="IDModalitaRisoluzione" 
                HeaderText="Modalità Risoluzione">
                <ItemTemplate>
                    <% if (!Enabled) { %>
                        <span style="padding: 7px;"><%# Eval("ModalitaRisoluzioneIntervento.Descrizione")%></span>
                    <% } else { %>
                        <telerik:RadComboBox ID="rcbModalitaRisoluzione" runat="server" Width="100%"
                            AllowCustomText="false" 
                            DataValueField="ID"
                            DataTextField="Descrizione"/>
                    <% } %>
                </ItemTemplate>
            </telerik:GridTemplateColumn>

            <telerik:GridTemplateColumn ItemStyle-Wrap="true"
                ItemStyle-CssClass='<% if (!Enabled) { return "NessunBordo noBorder";} else { return "";} %>'
                HeaderStyle-Width="95"
                ItemStyle-Width="95"
                HeaderStyle-HorizontalAlign="Center"
                ItemStyle-HorizontalAlign="Center"
                ItemStyle-VerticalAlign="Top"
                DataField="PresaInCarico" 
                HeaderText="Presa In Carico">
                <ItemTemplate>
                    <% if (!Enabled) { %>
                        <span style="padding: 7px;"><%# Eval("PresaInCaricoString")%></span>
                    <% } else { %>
                        <asp:CheckBox ID="chkPresaInCarico" runat="server" Text="" />
                        <asp:Label runat="server" ID="lblDataPresaInCarico" Text='<%# Eval("DataPresaInCarico", "{0:dd/MM/yyyy HH:mm}")%>'></asp:Label>
                    <% } %>
                </ItemTemplate>
            </telerik:GridTemplateColumn>

            <telerik:GridTemplateColumn ItemStyle-Wrap="true"
                ItemStyle-CssClass='<% if (!Enabled) { return "NessunBordo noBorder";} else { return "";} %>'
                ItemStyle-VerticalAlign="Top"
                HeaderStyle-Width="230"
                DataField="InizioIntervento" 
                HeaderText="Inizio Intervento">
                <ItemTemplate>
                    <% if (!Enabled) { %>
                        <span style="padding: 7px;"><%# Eval("InizioIntervento", "{0:dd/MM/yyyy HH:mm}")%></span>
                    <% } else { %>
                        <telerik:RadDateTimePicker ID="rdtpInizioIntervento" Width="100%" runat="server">
                            <TimeView runat="server"
                                ShowHeader="false"
                                StartTime="07:00:00"
                                Interval="00:15:00"
                                EndTime="19:00:00"
                                Columns="4">
                            </TimeView>
                            <Calendar runat="server"> 
                                <SpecialDays> 
                                    <telerik:RadCalendarDay Repeatable="Today" ItemStyle-BackColor="LightGreen" />
                                </SpecialDays> 
                            </Calendar> 
                        </telerik:RadDateTimePicker>
                    <% } %>
                </ItemTemplate>
            </telerik:GridTemplateColumn>

            <telerik:GridTemplateColumn ItemStyle-Wrap="true"
                ItemStyle-CssClass='<% if (!Enabled) { return "NessunBordo noBorder";} else { return "";} %>'
                HeaderStyle-Width="230"
                ItemStyle-VerticalAlign="Top"
                DataField="FineIntervento" 
                HeaderText="Fine Intervento">
                <ItemTemplate>
                    <% if (!Enabled) { %>
                        <span style="padding-bottom: 25px; margin-bottom:10px;"><%# Eval("FineIntervento", "{0:dd/MM/yyyy HH:mm}")%></span>
                    <% } else { %>
                        <telerik:RadDateTimePicker ID="rdtpFineIntervento" Width="100%" runat="server">
                            <TimeView runat="server"
                                ShowHeader="false"
                                StartTime="07:00:00"
                                Interval="00:15:00"
                                EndTime="19:00:00"
                                Columns="4">
                            </TimeView>
                            <Calendar runat="server"> 
                                <SpecialDays> 
                                    <telerik:RadCalendarDay Repeatable="Today" ItemStyle-BackColor="LightGreen" />
                                </SpecialDays> 
                            </Calendar> 
                        </telerik:RadDateTimePicker>
                    <% } %>
                </ItemTemplate>
            </telerik:GridTemplateColumn>

                    
            <telerik:GridTemplateColumn
                ItemStyle-CssClass='<% if (!Enabled) { return "NessunBordo noBorder";} else { return "";} %>'
                HeaderStyle-HorizontalAlign="Center"
                HeaderStyle-Width="80"
                HeaderText="Durata Minuti" 
                DataField="DurataMinuti"
                ItemStyle-VerticalAlign="Top"
                ItemStyle-Wrap="true">
                <ItemTemplate>
                    <% if (!Enabled) { %>
                        <span style="padding: 7px;"><%#Eval("DurataMinuti") %></span>
                    <% } else { %>
                        <telerik:RadNumericTextBox runat="server" ID="rntbDurataMinuti" Text='<%# (int?)Eval("DurataMinuti") %>' Type="Number" NumberFormat-DecimalDigits="0" NumberFormat-GroupSeparator="" Width="100%"></telerik:RadNumericTextBox>
                    <% } %>
                </ItemTemplate>
            </telerik:GridTemplateColumn>

                    
            <telerik:GridTemplateColumn
                ItemStyle-CssClass='<% if (!Enabled) { return "NessunBordo noBorder";} else { return "";} %>'
                HeaderStyle-HorizontalAlign="Center"
                HeaderStyle-Width="80"
                HeaderText="Pausa Minuti" 
                DataField="PausaMinuti"
                ItemStyle-VerticalAlign="Top"
                ItemStyle-Wrap="true">
                <ItemTemplate>
                    <% if (!Enabled) { %>
                        <span style="padding: 7px;"><%#Eval("PausaMinuti") %></span>
                    <% } else { %>
                        <telerik:RadNumericTextBox runat="server" ID="rntbPausaMinuti" Text='<%# (int?)Eval("PausaMinuti") %>' Type="Number" NumberFormat-DecimalDigits="0" NumberFormat-GroupSeparator="" Width="100%"></telerik:RadNumericTextBox>
                    <% } %>
                </ItemTemplate>
            </telerik:GridTemplateColumn>

                                
            <telerik:GridBoundColumn
                ItemStyle-CssClass='<% if (!Enabled) { return "NessunBordo noBorder";} else { return "";} %>'
                HeaderStyle-Width="60"
                HeaderStyle-HorizontalAlign="Center"
                ItemStyle-Wrap="true"
                ItemStyle-HorizontalAlign ="Center"
                ItemStyle-VerticalAlign="Top"
                HeaderText="Totale Minuti" 
                DataField="TotaleMinuti"
                Resizable="false"
                ReadOnly="true" />


            <telerik:GridTemplateColumn UniqueName="ColonnaNote"
                ItemStyle-CssClass='<% if (!Enabled) { return "NessunBordo noBorder";} else { return "";} %>'
                DataField="Note"
                HeaderText="Note" 
                ItemStyle-Wrap="true"
                ItemStyle-VerticalAlign="Top">
                <ItemTemplate>
                    <% if (!Enabled) { %>
                        <span style="padding-bottom: 25px;"><%#Eval("Note") %></span>
                    <% } else { %>
                        <telerik:RadTextBox ID="rtbNote" runat="server" Text='<%# (string)Eval("Note") %>' Width="100%" Height="70px" TextMode="MultiLine"/>
                    <% } %>
                </ItemTemplate>
            </telerik:GridTemplateColumn>

            <telerik:GridButtonColumn UniqueName="ColonnaElimina"
                ItemStyle-VerticalAlign="Top"
                ItemStyle-CssClass='<% if (!Enabled) { return "NessunBordo noBorder";} else { return "";} %>'
                Resizable="false"
                HeaderStyle-Width="40" 
                HeaderStyle-HorizontalAlign="Center"
                ItemStyle-Wrap="true"
                ItemStyle-HorizontalAlign="Center"
                ConfirmText="Eliminare le informazioni di contatto selezionate? L'eliminazione definitiva verrà effettuata al salvataggio della pagina"
                ConfirmDialogType="RadWindow"
                ConfirmTitle="Elimina contatto"
                ButtonType="ImageButton"
                ImageUrl="/UI/Images/Toolbar/24x24/delete.png"
                Text="Elimina"
                CommandName="Delete" 
                Exportable="false"/>
        </Columns>
        
    </MasterTableView>

    <ClientSettings EnableRowHoverStyle="true" AllowColumnHide="false" EnableAlternatingItems="false">
        <Selecting AllowRowSelect="false" />
        <ClientEvents OnGridCreated="RadGrid_OnGridCreated" />
    </ClientSettings>

</telerik:RadGrid>

<div style="display: none;">
    <telerik:RadAjaxManagerProxy runat="server" ID="ramInterventiOperatori">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="rgGriglia">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rgGriglia" LoadingPanelID="RadAjaxLoadingPanelMaster" />
                    <telerik:AjaxUpdatedControl ControlID="RadWindowManagerMaster" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManagerProxy>
    <telerik:RadPersistenceManagerProxy ID="rpmpInterventiOperatori" runat="server" >
        <PersistenceSettings>
            <telerik:PersistenceSetting ControlID="rgGriglia" />
        </PersistenceSettings>
    </telerik:RadPersistenceManagerProxy>
</div>
