<%@ Page Title="Analisi Situazione Progetti per Attività" Language="C#" MasterPageFile="~/UI/Main.Master" AutoEventWireup="true" CodeBehind="AnalisiSituazioneAttivita.aspx.cs" Inherits="SeCoGEST.Web.Progetti.AnalisiSituazioneAttivita" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .parametriProgetto {
            width: 100%;
            border-spacing: 10px;
            background-color: #bfcfe0;
        }
        .parametriAttivita {
            width: 100%;
            border-spacing: 10px;
            background-color: #a1bec4;
            border-top: solid 1px gray;
            border-bottom: solid 1px gray;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="title" runat="server">
    <asp:Label ID="lblTitolo" runat="server" Text="Analisi Situazione Progetti per Attività" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="main" runat="server">

    <div id="divParametriRicerca" style="width: 100%;">
        <table width="100%" class="parametriProgetto">
            <tr>
                <td>
                    Numero progetto
                    <telerik:RadNumericTextBox runat="server" ID="rntbNumeroProgetto" Type="Number" NumberFormat-DecimalDigits="0" NumberFormat-GroupSeparator="" Width="100%"></telerik:RadNumericTextBox>
                </td>
                <td>
                    Data redazione (dal)
                    <telerik:RadDateTimePicker ID="rdtpDataRedazioneDal" runat="server" Width="100%">
                        <TimeView ID="TimeView1" runat="server"
                            Columns="4"
                            EndTime="19:00:00"
                            Interval="00:15:00"
                            ShowHeader="false"
                            StartTime="07:00:00">
                        </TimeView>
                    </telerik:RadDateTimePicker>
                </td>
                <td>
                    Data redazione (al)
                    <telerik:RadDateTimePicker ID="rdtpDataRedazioneAl" Width="100%" runat="server">
                        <TimeView runat="server"
                            ShowHeader="false"
                            StartTime="07:00:00"
                            Interval="00:15:00"
                            EndTime="19:00:00"
                            Columns="4">
                        </TimeView>
                    </telerik:RadDateTimePicker>
                </td>
                <td>
                    Numero Commessa
                    <telerik:RadTextBox runat="server" ID="rtbNumeroCommessa" Width="100%"></telerik:RadTextBox>
                </td>
                <td>
                    Codice Contratto
                    <telerik:RadTextBox ID="rtbCodiceContratto" runat="server" Width="100%"></telerik:RadTextBox>
                </td>
                <td>
                    Stato Progetto
                    <telerik:RadComboBox ID="rcbStatoProgetto" runat="server" RenderMode="Lightweight" Width="100%" AllowCustomText="false"></telerik:RadComboBox>
                </td>
                <td>
                    Progetto Chiuso
                    <telerik:RadComboBox ID="rcbChiuso" runat="server" RenderMode="Lightweight" Width="100%" AllowCustomText="false">
                        <Items>
                            <telerik:RadComboBoxItem Value="10" Text="Indifferente" />
                            <telerik:RadComboBoxItem Value="1" Text="Si" />
                            <telerik:RadComboBoxItem Value="2" Text="No" />
                        </Items>
                    </telerik:RadComboBox>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    Ragione Sociale
                    <telerik:RadTextBox runat="server" ID="rtbRagioneSociale" Width="100%"></telerik:RadTextBox>
                </td>
                <td colspan="4">
                    Titolo Progetto
                    <telerik:RadTextBox runat="server" ID="rtbTitoloProgetto" Width="100%"></telerik:RadTextBox>
                </td>
            </tr>

            <tr>
                <td colspan="4">
                    Referente cliente
                    <telerik:RadTextBox runat="server" ID="rtbReferenteCliente" Width="100%"></telerik:RadTextBox>
                </td>
                <td colspan="3">
                    DPO
                    <telerik:RadTextBox runat="server" ID="rtbDPO" Width="100%"></telerik:RadTextBox>
                </td>
            </tr>
        </table>
        <table width="100%" class="parametriAttivita">
            <tr>
                <td>
                    Data inserimento (dal)
                    <telerik:RadDateTimePicker ID="rdtpDataInserimentoDal" Width="100%" runat="server">
                        <TimeView runat="server"
                            ShowHeader="false"
                            StartTime="07:00:00"
                            Interval="00:15:00"
                            EndTime="19:00:00"
                            Columns="4">
                        </TimeView>
                    </telerik:RadDateTimePicker>
                </td>
                <td>
                    Data inserimento (aal)
                    <telerik:RadDateTimePicker ID="rdtpDataInserimentoAl" Width="100%" runat="server">
                        <TimeView runat="server"
                            ShowHeader="false"
                            StartTime="07:00:00"
                            Interval="00:15:00"
                            EndTime="19:00:00"
                            Columns="4">
                        </TimeView>
                    </telerik:RadDateTimePicker>
                </td>
                <td colspan="5">
                    Allegati
                    <telerik:RadTextBox runat="server" ID="rtbAllegati" Width="100%"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    Descrizione Attività
                    <telerik:RadTextBox runat="server" ID="rtbDescrizioneAttivita" Width="100%"></telerik:RadTextBox>
                </td>
                <td colspan="2">
                    Note Attività
                    <telerik:RadTextBox runat="server" ID="rtbNoteContratto" Width="100%"></telerik:RadTextBox>
                </td>
                <td colspan="2">
                    Note Operatore
                    <telerik:RadTextBox runat="server" ID="rtbNoteOperatore" Width="100%"></telerik:RadTextBox>
                </td>
                <td>
                    Numero Ticket
                    <telerik:RadNumericTextBox runat="server" ID="rntbTicket" Width="100%" NumberFormat-DecimalDigits="0" NumberFormat-AllowRounding="false" MinValue="1"></telerik:RadNumericTextBox>
                </td>        
            </tr>
            <tr>
                <td>
                    Scadenza (dal)
                    <telerik:RadDatePicker ID="rdtpDataNotificaDAL" Width="100%" runat="server"></telerik:RadDatePicker>
                </td>
                <td>
                    Scadenza (al)
                    <telerik:RadDatePicker ID="rdtpDataNotificaAL" Width="100%" runat="server"></telerik:RadDatePicker>
                </td>
                <td colspan="2">
                    Operatore Assegnato
                    <telerik:RadTextBox runat="server" ID="rtbOperatoreAssegnato" Width="100%"></telerik:RadTextBox>
                </td>
                <td colspan="2">
                    Operatore Esecutore
                    <telerik:RadTextBox runat="server" ID="rtbOperatoreEsecutore" Width="100%"></telerik:RadTextBox>
                </td>
                <td>
                    Stato Attività
                    <telerik:RadComboBox ID="rcbStatoAttivita" runat="server" RenderMode="Lightweight" Width="100%" AllowCustomText="false"></telerik:RadComboBox>
                </td>        
            </tr>
        </table>
        <table width="100%" style="padding: 12px;">
            <tr>
                <td>
                    <asp:Button runat="server" ID="btSearch" Text="Esegui Analisi" OnClick="btSearch_Click" />
                </td>
            </tr>
        </table>
    </div>

    <telerik:RadGrid runat="server"
        ID="rgGriglia"
        CssClass="GridResponsiveColumns"
        AllowFilteringByColumn="false"
        AllowPaging="true"
        PageSize="20"
        GridLines="None"
        SortingSettings-SortToolTip="Clicca qui per ordinare i dati in base a questa colonna"
        AutoGenerateColumns="false"
        OnNeedDataSource="rgGriglia_NeedDataSource"
        PagerStyle-FirstPageToolTip="Prima Pagina"
        PagerStyle-LastPageToolTip="Ultima Pagina"
        PagerStyle-PrevPageToolTip="Pagina Precedente"
        PagerStyle-NextPageToolTip="Pagina Successiva"
        PagerStyle-PagerTextFormat="{4} Elementi da {2} a {3} su {5}, Pagina {0} di {1}">

        <MasterTableView DataKeyNames="ID" TableLayout="Fixed"
            Width="100%"
            AllowFilteringByColumn="false"
            AllowSorting="true"
            AllowMultiColumnSorting="true"
            GridLines="Both"
            NoMasterRecordsText="Nessun dato da visualizzare"
            CommandItemDisplay="Top">

            <Columns>
                <telerik:GridHyperLinkColumn
                    HeaderStyle-Width="50"
                    ItemStyle-Width="50"
                    HeaderStyle-HorizontalAlign="Center"
                    ItemStyle-HorizontalAlign="Center"
                    ImageUrl="/UI/Images/open_folder.png"
                    Text="Apri Progetto..."
                    Target="_blank"
                    Resizable="false"
                    DataNavigateUrlFields="ID,IDProgetto"
                    DataNavigateUrlFormatString="/Progetti/DettagliProgetto.aspx?ID={1}&IDAttivita={0}"
                    AllowFiltering="false"
                    AllowSorting="false"
                    Exportable="false" />

<%--                <telerik:GridBoundColumn HeaderText="IDAttivita" DataField="ID" ItemStyle-Wrap="true"/>
                <telerik:GridBoundColumn HeaderText="IDProgetto" DataField="IDProgetto" ItemStyle-Wrap="true"/>--%>

                <telerik:GridBoundColumn HeaderText="Descrizione Attività" DataField="Descrizione" ItemStyle-Wrap="true"/>

                <telerik:GridBoundColumn HeaderText="Note Attività" DataField="NoteContratto" ItemStyle-Wrap="true"></telerik:GridBoundColumn>
                
                <telerik:GridBoundColumn HeaderText="Ragione Sociale" DataField="Progetto.DenominazioneCliente" ItemStyle-Wrap="true"></telerik:GridBoundColumn>

                <telerik:GridBoundColumn HeaderText="Operat. Esecutore" DataField="Esecutore.CognomeNome" ItemStyle-Wrap="true"></telerik:GridBoundColumn>

                <telerik:GridBoundColumn HeaderText="Note Operatore" DataField="NoteOperatore" ItemStyle-Wrap="true" DataType="System.String" DataFormatString="{0:@}" ></telerik:GridBoundColumn>

                <telerik:GridBoundColumn HeaderText="Stato" DataField="StatoString" HeaderStyle-Width="110px"></telerik:GridBoundColumn>

<%--                <telerik:GridDateTimeColumn HeaderText="Scadenza" DataField="Scadenza" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"></telerik:GridDateTimeColumn>--%>

<%--                <telerik:GridBoundColumn HeaderText="Operat. Assegnato" DataField="Operatore.CognomeNome" ItemStyle-Wrap="true"></telerik:GridBoundColumn>--%>

<%--                <telerik:GridBoundColumn HeaderText="Ticket" DataField="Ticket.Numero"></telerik:GridBoundColumn>--%>

<%--                <telerik:GridBoundColumn HeaderText="Progetto" DataField="Progetto.Numero"></telerik:GridBoundColumn>--%>

<%--                <telerik:GridBoundColumn  HeaderText="Titolo Progetto" DataField="Progetto.Titolo" ItemStyle-Wrap="true"></telerik:GridBoundColumn>--%>

            </Columns>

        </MasterTableView>
        
        <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" />
        <GroupingSettings CaseSensitive="false" />

        <ClientSettings EnableRowHoverStyle="true">
            <Selecting AllowRowSelect="True" />
            <Resizing AllowColumnResize="true" EnableRealTimeResize="true" AllowResizeToFit="false" />
        </ClientSettings>
    </telerik:RadGrid>
    <br />
    <br />

    <div style="display: none;">
        <telerik:RadAjaxManagerProxy runat="server" ID="ramProgetti">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="btSearch">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="divParametriRicerca" LoadingPanelID="RadAjaxLoadingPanelMaster" />
                        <telerik:AjaxUpdatedControl ControlID="rgGriglia" LoadingPanelID="RadAjaxLoadingPanelMaster" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManagerProxy>
    </div>

</asp:Content>
