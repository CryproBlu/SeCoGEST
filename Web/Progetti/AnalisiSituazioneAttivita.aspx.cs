using SeCoGes.Utilities;
using SeCoGEST.Entities;
using SeCoGEST.Helper;
using System;
using System.Linq;
using Telerik.Web.UI;

namespace SeCoGEST.Web.Progetti
{
    public partial class AnalisiSituazioneAttivita : System.Web.UI.Page
    {
        bool eseguiRicerca = false;

        /// <summary>
        /// Metodo di gestione dell'evento Load della pagina
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //Traduce le voci del menu di filtro della griglia. Questa operazione deve essere fatta ad ogni post
                TelerikHelper.TraduciMenuFiltro(rgGriglia.FilterMenu);
                TelerikRadGridHelper.ManageExelExportSettings(rgGriglia, "Analisi", "Esporta in Excel", false, false);

                this.Form.DefaultButton = this.btSearch.UniqueID;

                if (!Helper.Web.IsPostOrCallBack(this))
                {
                    if (rcbStatoProgetto.Items.Count() == 0)
                    {
                        RadComboBoxItem defaultItem = new RadComboBoxItem("Indifferente", "10");
                        rcbStatoProgetto.Items.Add(defaultItem);
                        rcbStatoProgetto.Items.Add(new RadComboBoxItem("Da Eseguire", StatoProgettoEnum.DaEseguire.GetHashCode().ToString()));
                        rcbStatoProgetto.Items.Add(new RadComboBoxItem("In Gestione", StatoProgettoEnum.InGestione.GetHashCode().ToString()));
                        rcbStatoProgetto.Items.Add(new RadComboBoxItem("Eseguito", StatoProgettoEnum.Eseguito.GetHashCode().ToString()));
                        defaultItem.Selected = true;
                    }

                    if (rcbStatoAttivita.Items.Count() == 0)
                    {
                        RadComboBoxItem defaultItem = new RadComboBoxItem("Indifferente", "10");
                        rcbStatoAttivita.Items.Add(defaultItem);
                        rcbStatoAttivita.Items.Add(new RadComboBoxItem("Da Eseguire", StatoAttivitaProgettoEnum.DaEseguire.GetHashCode().ToString()));
                        rcbStatoAttivita.Items.Add(new RadComboBoxItem("In Gestione", StatoAttivitaProgettoEnum.InGestione.GetHashCode().ToString()));
                        rcbStatoAttivita.Items.Add(new RadComboBoxItem("Eseguito", StatoAttivitaProgettoEnum.Eseguito.GetHashCode().ToString()));
                        rcbStatoAttivita.Items.Add(new RadComboBoxItem("Modificato rispetto a contratto", StatoAttivitaProgettoEnum.Modificato.GetHashCode().ToString()));
                        defaultItem.Selected = true;
                    }

                }
            }
            catch (Exception ex)
            {
                MessageHelper.ShowErrorMessage(this, ex);
            }
        }

        protected void rgGriglia_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Helper.Web.IsPostOrCallBack(this))
            {
                Logic.Progetto_Attività llPA = new Logic.Progetto_Attività();
                var baseQuery = llPA.ReadAll();

                // *****************************************************************************************************
                // CONTROLLO RICERCA IN BASE AI CAMPI DEL PROGETTO

                // Numero progetto
                if (rntbNumeroProgetto.Value.HasValue)
                {
                    baseQuery = baseQuery.Where(a => a.Progetto.Numero.Equals(rntbNumeroProgetto.Value));
                }

                // Data redazione (al)
                if (rdtpDataRedazioneAl.SelectedDate.HasValue)
                {
                    baseQuery = baseQuery.Where(a => a.Progetto.DataRedazione >= rdtpDataRedazioneAl.SelectedDate.Value);
                }
                // Data redazione (al)
                if (rdtpDataRedazioneAl.SelectedDate.HasValue)
                {
                    baseQuery = baseQuery.Where(a => a.Progetto.DataRedazione <= rdtpDataRedazioneAl.SelectedDate.Value);
                }

                // Numero Commessa
                if (!string.IsNullOrEmpty(rtbNumeroCommessa.Text))
                {
                    baseQuery = baseQuery.Where(a => a.Progetto.NumeroCommessa.Contains(rtbNumeroCommessa.Text.Trim()));
                }

                // Codice Contratto
                if (!string.IsNullOrEmpty(rtbCodiceContratto.Text))
                {
                    baseQuery = baseQuery.Where(a => a.Progetto.CodiceContratto.Contains(rtbCodiceContratto.Text.Trim()));
                }

                // Stato Progetto
                if (rcbStatoProgetto.SelectedValue != string.Empty && rcbStatoProgetto.SelectedValue != "10")
                {
                    baseQuery = baseQuery.Where(a => a.Progetto.StatoString.ToLower().Contains(rcbStatoProgetto.Text.Trim().ToLower()));
                }

                // Progetto Chiuso
                if (rcbChiuso.SelectedValue != string.Empty && rcbChiuso.SelectedValue != "10")
                {
                    baseQuery = baseQuery.Where(a => a.Progetto.ChiusoString.ToLower().Contains(rcbChiuso.Text.Trim().ToLower()));
                }

                // Ragione Sociale
                if (!string.IsNullOrEmpty(rtbRagioneSociale.Text.Trim()))
                {
                    baseQuery = baseQuery.Where(a => a.Progetto.RagioneSociale.Contains(rtbRagioneSociale.Text.Trim()));
                }

                // Titolo Progetto
                if (!string.IsNullOrEmpty(rtbTitoloProgetto.Text.Trim()))
                {
                    baseQuery = baseQuery.Where(a => a.Progetto.Titolo.Contains(rtbTitoloProgetto.Text.Trim()));
                }

                // Referente cliente
                if (!string.IsNullOrEmpty(rtbReferenteCliente.Text.Trim()))
                {
                    baseQuery = baseQuery.Where(a => 
                        a.Progetto.ReferenteCliente != null &&
                        a.Progetto.ReferenteCliente.Nome.Contains(rtbReferenteCliente.Text.Trim()));
                }

                // DPO
                if (!string.IsNullOrEmpty(rtbDPO.Text.Trim()))
                {
                    baseQuery = baseQuery.Where(a =>
                        a.Progetto.NomeCompletoDPO != null &&
                        a.Progetto.NomeCompletoDPO.Contains(rtbDPO.Text.Trim()));
                }





                // *****************************************************************************************************
                // CONTROLLO RICERCA IN BASE AI CAMPI DELLE ATTIVITA'

                // Data inserimento (dal)
                if (rdtpDataInserimentoDal.SelectedDate.HasValue)
                {
                    baseQuery = baseQuery.Where(a => a.DataInserimento >= rdtpDataInserimentoDal.SelectedDate.Value);
                }
                // Data inserimento (al)
                if (rdtpDataInserimentoAl.SelectedDate.HasValue)
                {
                    baseQuery = baseQuery.Where(a => a.DataInserimento <= rdtpDataInserimentoAl.SelectedDate.Value);
                }

                // Allegati
                if (!string.IsNullOrEmpty(rtbAllegati.Text.Trim()))
                {
                    var allegati = new Logic.Allegati(llPA).Read();
                    baseQuery = baseQuery.Where(a =>
                        allegati.Any(al =>
                            al.TipologiaAllegato == TipologiaAllegatoEnum.AttivitaProgetto.GetHashCode() &&
                            al.IDLegame == a.ID &&
                            al.NomeFile.Contains(rtbAllegati.Text.Trim())));
                }

                // Descrizione Attività
                if (!string.IsNullOrEmpty(rtbDescrizioneAttivita.Text.Trim()))
                {
                    baseQuery = baseQuery.Where(a => a.Descrizione.Contains(rtbDescrizioneAttivita.Text.Trim()));
                }

                // Note Contratto
                if (!string.IsNullOrEmpty(rtbNoteContratto.Text.Trim()))
                {
                    baseQuery = baseQuery.Where(a => a.NoteContratto.Contains(rtbNoteContratto.Text.Trim()));
                }

                // Note Operatore
                if (!string.IsNullOrEmpty(rtbNoteOperatore.Text.Trim()))
                {
                    baseQuery = baseQuery.Where(a => a.NoteOperatore.Contains(rtbNoteOperatore.Text.Trim()));
                }

                // Numero Ticket
                if (rntbTicket.Value.HasValue)
                {
                    baseQuery = baseQuery.Where(a => a.Ticket.Numero == rntbTicket.Value);
                }

                // Scadenza (dal)
                if (rdtpDataNotificaDAL.SelectedDate.HasValue)
                {
                    baseQuery = baseQuery.Where(a => a.Scadenza.Value.Date >= rdtpDataNotificaDAL.SelectedDate);
                }
                //  Scadenza (al)
                if (rdtpDataNotificaAL.SelectedDate.HasValue)
                {
                    baseQuery = baseQuery.Where(a => a.Scadenza.Value.Date <= rdtpDataNotificaAL.SelectedDate);
                }

                // Operatore Assegnato
                if (!string.IsNullOrEmpty(rtbOperatoreAssegnato.Text.Trim()))
                {
                    baseQuery = baseQuery.Where(a => a.Operatore.CognomeNome.Contains(rtbOperatoreAssegnato.Text.Trim()));
                }

                // Operatore Esecutore
                if (!string.IsNullOrEmpty(rtbOperatoreEsecutore.Text.Trim()))
                {
                    baseQuery = baseQuery.Where(a => a.Esecutore.CognomeNome.Contains(rtbOperatoreEsecutore.Text.Trim()));
                }

                // Stato Attività
                if (rcbStatoAttivita.SelectedValue != string.Empty && rcbStatoAttivita.SelectedValue != "10")
                {
                    baseQuery = baseQuery.Where(a => a.Stato == byte.Parse(rcbStatoAttivita.SelectedValue));
                }




                baseQuery = baseQuery.OrderByDescending(a => a.Progetto.DataRedazione).ThenBy(a => a.Ordine);
                rgGriglia.DataSource = baseQuery;
            }
        }

        protected void btSearch_Click(object sender, EventArgs e)
        {
            eseguiRicerca = true;
            rgGriglia.Rebind();
        }
    }
}