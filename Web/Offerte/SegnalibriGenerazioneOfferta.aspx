<%@ Page Title="Segnalibri Generazione Offerta" Language="C#" MasterPageFile="~/UI/Main.Master" AutoEventWireup="true" CodeBehind="SegnalibriGenerazioneOfferta.aspx.cs" Inherits="SeCoGEST.Web.Offerte.SegnalibriGenerazioneOfferta" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .TabellaSegnalibri td, .TabellaSegnalibri th {
            border: 1px solid #aaa;
            width: 300px;
        }

        .TabellaSegnalibri th {
            background-color: #ccc;
        }

        .TabellaSegnalibri {
            border-collapse: collapse;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="title" runat="server">
    <asp:Label ID="lblTitolo" runat="server" Text="Dettagli Offerta" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="main" runat="server">
    <div class="pageBody">
        <telerik:RadPageLayout runat="server" Width="100%" HtmlTag="None" GridType="Fluid">
            <Rows>
                <telerik:LayoutRow RowType="Region">
                    <Columns>
                        <telerik:LayoutColumn Span="12" SpanMd="12" SpanSm="12" SpanXs="12" >
                            <h2>INFORMAZIONI SUI SEGNALIBRI PER LA CREAZIONE DI MODELLI PER LA GENERAZIONE DELLE OFFERTE</h2>

                            <p>Di seguito sono riportati i segnalibri gestiti dalla procedura di generazione del documento relativo a una offerta.</p>
                            <p>Mediante l'utilizzo di questi segnalibri, sarà possibile inserire nel documento informazioni specifiche dell'offerta oppure delle tabelle relative a informazioni correlate all'offerta.</p>
                            <p>L'inserimento di questi segnalibri è molto semplice, basterà:</p>
                            <ol>
                                <li>posizionare il mouse nella zona in cui si intende inserire una informazione all'interno del documento</li>
                                <li>cliccare l'etichetta "Inserisci" situata nella parte alta a sinistra della finestra di word</li>
                                <li>cliccare il pulsante "Segnalibro"</li>
                                <li>inserire, nella casella "Nome segnalibro", uno dei nomi riportati di seguito e premere aggiungi</li>
                            </ol>
                            <p>
                                <strong>ATTENZIONE !!!</strong><br />
                                Il segnalibro è univoco all'interno del documento, quindi non è possibile inserire la medesima informazione in più posizioni del documento.<br />
                                Per risolvere questa problematica basterà aggiungere al suffisso un numero che renda univoco uno specifico segnalibro dell'offerta.<br />
                                Un esempio prativo. Se volessimo mostrare il valore del segnalibro "<strong><%=SeCoGEST.Logic.DocumentiDaGenerare.GeneratoreOfferte.PREFISSO_BOOKMARK_DOCUMENTO_NUMERO_OFFERTA %></strong>" in due punti, basterà identificare il primo punto in cui si vuole inserire il valore e inserire come nome del segnalibro "<strong><%=SeCoGEST.Logic.DocumentiDaGenerare.GeneratoreOfferte.PREFISSO_BOOKMARK_DOCUMENTO_NUMERO_OFFERTA %><span style="color: red;">01</span></strong>", mentre nel secondo punto basterà inserire un segnalibro con il seguente nome "<strong><%=SeCoGEST.Logic.DocumentiDaGenerare.GeneratoreOfferte.PREFISSO_BOOKMARK_DOCUMENTO_NUMERO_OFFERTA %><span style="color: red;">02</span></strong>".
                            </p>
                            <p>
                                Successivamente alla creazione del modello da utilizzare per la generazione di una offerta, basterà caricarlo nella cartella che il gestionale monitora per permettere all'utente di selezionare il modello che più preferisce.
                            </p>

                            <asp:Repeater ID="rptTabellaSegnalibri" runat="server">
                                <HeaderTemplate>
                                    <table class="TabellaSegnalibri" cellspacing="0" cellpadding="5">
                                        <thead>
                                            <tr>
                                                <th><strong>PREFISSO SEGNAPOSTO</strong></th>
                                                <th><strong>DATO CHE VERR&Agrave; INSERITO</strong></th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                        <tr>
                                            <td><em><%# DataBinder.Eval(Container.DataItem, "Key") %></em></td>
                                            <td><%# DataBinder.Eval(Container.DataItem, "Value") %></td>
                                        </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                        </tbody>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>                            
                        </telerik:LayoutColumn>
                    </Columns>
                </telerik:LayoutRow>
            </Rows>
        </telerik:RadPageLayout>
    </div>

</asp:Content>
