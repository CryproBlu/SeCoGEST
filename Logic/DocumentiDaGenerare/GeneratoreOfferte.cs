using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Word;
using SeCoGes.Utilities;
using SeCoGEST.Entities;

namespace SeCoGEST.Logic.DocumentiDaGenerare
{
    public class GeneratoreOfferte : GeneratoreDocumenti
    {
        #region Variabili

        private EntityId<Entities.Offerta> IdOfferta = null;
        private Entities.Offerta Offerta = null;

        #endregion

        #region Bookmarks

        public const string PREFISSO_BOOKMARK_DOCUMENTO_NUMERO_OFFERTA = "NUMERO_OFFERTA_";
        public const string PREFISSO_BOOKMARK_DOCUMENTO_NUMERO_REVISIONE_OFFERTA = "NUMERO_REVISIONE_OFFERTA_";
        public const string PREFISSO_BOOKMARK_DOCUMENTO_DATA_OFFERTA = "DATA_OFFERTA_";
        public const string PREFISSO_BOOKMARK_DOCUMENTO_TITOLO_OFFERTA = "TITOLO_OFFERTA_";
        public const string PREFISSO_BOOKMARK_DOCUMENTO_CODICE_CLIENTE = "CODICE_CLIENTE_";
        public const string PREFISSO_BOOKMARK_DOCUMENTO_RAGIONE_SOCIALE_CLIENTE = "RAGIONE_SOCIALE_CLIENTE_";
        public const string PREFISSO_BOOKMARK_DOCUMENTO_DESTINAZIONE_MERCE = "DESTINAZIONE_MERCE_";
        public const string PREFISSO_BOOKMARK_DOCUMENTO_INDIRIZZO_CLIENTE = "INDIRIZZO_";
        public const string PREFISSO_BOOKMARK_DOCUMENTO_CAP_CLIENTE = "CAP_";
        public const string PREFISSO_BOOKMARK_DOCUMENTO_LOCALITA_CLIENTE = "LOCALITA_";
        public const string PREFISSO_BOOKMARK_DOCUMENTO_PROVINCIA_CLIENTE = "PROVINCIA_";
        public const string PREFISSO_BOOKMARK_DOCUMENTO_TELEFONO_CLIENTE = "TELEFONO_";
        public const string PREFISSO_BOOKMARK_DOCUMENTO_TOTALE_COSTO = "TOTALE_COSTO_";
        public const string PREFISSO_BOOKMARK_DOCUMENTO_TOTALE_VENDITA_CALCOLATO = "TOTALE_VENDITA_CALCOLATO_";
        public const string PREFISSO_BOOKMARK_DOCUMENTO_TOTALE_RICARICO_VALUTA = "TOTALE_RICARICO_VALUTA_";
        public const string PREFISSO_BOOKMARK_DOCUMENTO_TOTALE_RICARICO_PERCENTUALE = "TOTALE_RICARICO_PERCENTUALE_";
        public const string PREFISSO_BOOKMARK_DOCUMENTO_TOTALE_VENDITA = "TOTALE_VENDITA_";
        public const string PREFISSO_BOOKMARK_DOCUMENTO_CODICE_COMMESSA = "CODICE_COMMESSA_";
        public const string PREFISSO_BOOKMARK_DOCUMENTO_GIORNI_VALIDITA = "GIORNI_VALIDITA_";
        public const string PREFISSO_BOOKMARK_DOCUMENTO_TEMPI_DI_CONSEGNA = "TEMPI_DI_CONSEGNA_";
        public const string PREFISSO_BOOKMARK_DOCUMENTO_NOTE_INTERNE = "NOTE_INTERNE_";
        public const string PREFISSO_BOOKMARK_DOCUMENTO_NOTE_RIFIUTO = "NOTE_RIFIUTO_";
        public const string PREFISSO_BOOKMARK_DOCUMENTO_TESTO_PIE_DI_PAGINA = "TESTO_PIE_DI_PAGINA_";
        public const string PREFISSO_BOOKMARK_DOCUMENTO_TESTO_INTESTAZIONE = "TESTO_INTESTAZIONE_";
        public const string PREFISSO_BOOKMARK_DOCUMENTO_TESTO_VALIDITA_OFFERTA = "TESTO_VALIDITA_OFFERTA_";
        public const string PREFISSO_BOOKMARK_DOCUMENTO_TESTO_EMAIL_INVIATA_AL_CLIENTE = "TESTO_EMAIL_INVIATA_AL_CLIENTE_";
        public const string PREFISSO_BOOKMARK_DOCUMENTO_CODICE_PAGAMENTO = "CODICE_PAGAMENTO_";
        public const string PREFISSO_BOOKMARK_DOCUMENTO_DESCRIZIONE_PAGAMENTO = "DESCRIZIONE_PAGAMENTO_";
        public const string PREFISSO_BOOKMARK_DOCUMENTO_CODICE_IBAN = "CODICE_IBAN_";
        public const string PREFISSO_BOOKMARK_DOCUMENTO_SE_BONIFICO_CODICE_IBAN = "SE_BONIFICO_CODICE_IBAN_";
        public const string PREFISSO_BOOKMARK_DOCUMENTO_TOTALE_COSTO_CALCOLATO = "TOTALE_COSTO_CALCOLATO_";
        public const string PREFISSO_BOOKMARK_DOCUMENTO_TOTALE_RICARICO_VALUTA_CALCOLATO = "TOTALE_RICARICO_VALUTA_CALCOLATO_";
        public const string PREFISSO_BOOKMARK_DOCUMENTO_TOTALE_RICARICO_PERCENTUALE_CALCOLATO = "TOTALE_RICARICO_PERCENTUALE_CALCOLATO_";
        public const string PREFISSO_BOOKMARK_DOCUMENTO_TESTO_SEZIONE_PAGAMENTI = "TESTO_SEZIONE_PAGAMENTI_";
        public const string PREFISSO_BOOKMARK_DOCUMENTO_SERVIZI = "SERVIZI_";
        public const string PREFISSO_BOOKMARK_DOCUMENTO_ARTICOLI = "ARTICOLI_";
        public const string PREFISSO_BOOKMARK_DOCUMENTO_CONTRATTI_SENZA_CLAUSOLE_VESSATORIE = "CONTRATTI_SENZA_CLAUSOLE_VESSATORIE_";
        public const string PREFISSO_BOOKMARK_DOCUMENTO_CONTRATTI_CON_CLAUSOLE_VESSATORIE = "CONTRATTI_CON_CLAUSOLE_VESSATORIE_";
        public const string PREFISSO_BOOKMARK_DOCUMENTO_ELENCO_CLAUSOLE_VESSATORIE_FISSE = "ELENCO_CLAUSOLE_VESSATORIE_FISSE_";
        public const string PREFISSO_BOOKMARK_DOCUMENTO_ELENCO_CLAUSOLE_VESSATORIE_VARIABILI = "ELENCO_CLAUSOLE_VESSATORIE_VARIABILI_";

        #endregion

        #region Costruttori e DAL interno

        /// <summary>
        /// Data utilizzato da tutte le operazioni su database fatte tramite questa istanza
        /// </summary>
        private Offerte ll;

        /// <summary>
        /// Crea l'istanza della classe necessaria per creare l'istanza del logic layer nei metodi statici
        /// </summary>
        private GeneratoreOfferte() : this(null) { }

        /// <summary>
        /// Crea l'istanza della classe utilizzando il DataContext globale condiviso
        /// </summary>
        /// <param name="idOfferta"></param>
        public GeneratoreOfferte(EntityId<Entities.Offerta> idOfferta)
            : base(false)
        {
            this.IdOfferta = idOfferta;
            CreateLogic();
        }

        /// <summary>
        /// Crea l'istanza della classe permettendo al chiamante di specificare se utilizzare un DataContext privato o quello globale condiviso
        /// </summary>
        /// <param name="createStandaloneContext"></param>
        public GeneratoreOfferte(bool createStandaloneContext, EntityId<Entities.Offerta> idOfferta)
            : base(createStandaloneContext)
        {
            this.IdOfferta = idOfferta;
            CreateLogic();
        }

        /// <summary>
        /// Crea l'istanza della classe permettendo al chiamante di specificare il LogicLayer collegato.
        /// L'istanza creata utilizzerà il DataContext interno al LogicLayer passato per effettuare le operazioni sulla base dati
        /// </summary>
        /// <param name="logicLayer"></param>
        public GeneratoreOfferte(Base.LogicLayerBase logicLayer, EntityId<Entities.Offerta> idOfferta)
            : base(logicLayer)
        {
            this.IdOfferta = idOfferta;
            CreateLogic();
        }

        /// <summary>
        /// Crea un Data che utilizza il DataContext specificato nella classe base LogicLayerBase
        /// </summary>
        protected override void CreateLogic()
        {
            ll = new Offerte(this);
        }

        #endregion

        #region Metodi Pubblici
       
        /// <summary>
        /// Effettua il rilascio delle risorse occupate dall'istanza corrente
        /// </summary>
        public override void Dispose()
        {
            Offerta = null;
            base.Dispose();
        }

        /// <summary>
        /// Effettua la generazione del file di offerta da generare
        /// </summary>
        /// <param name="nomeModelloDaUtilizzare"></param>
        /// <param name="estensione"></param>
        /// <returns></returns>
        public string GeneraNomeFile(string nomeModelloDaUtilizzare, string estensione)
        {
            if (String.IsNullOrWhiteSpace(estensione)) estensione = ".docx";

            if (Offerta != null)
            {
                return $"{DateTime.Now.ToString("yyyyMMddHHmmss")}_{Path.GetFileNameWithoutExtension(nomeModelloDaUtilizzare)}_{Offerta.Numero}{estensione}";
            }
            else
            {
                return $"{DateTime.Now.ToString("yyyyMMddHHmmss")}_{Path.GetFileNameWithoutExtension(nomeModelloDaUtilizzare)}{estensione}";
            }
        }

        #endregion

        #region Funzioni Accessorie

        /// <summary>
        /// Effettua il popolamento del documento da generare
        /// </summary>
        /// <param name="application"></param>
        /// <param name="document"></param>
        protected override void PopolaDocumento(Application application, Document document)
        {
            Offerta = ll.Find(IdOfferta);
            if (Offerta == null) throw new Exception($"L'Offerta con ID \"{Offerta}\" non esiste");

            Logic.AnalisiVenditeConfigurazioneArticoliAggiuntivi llConfigurazioneArticoliAggiuntivi = new AnalisiVenditeConfigurazioneArticoliAggiuntivi(ll);
            OffertaArticolo[] elencoArticoliUnivoci = GetArticoliUnivociPerServiziContrattiOfferta(Offerta);

            Bookmarks bookmarks = document.Bookmarks;
            if (bookmarks != null && bookmarks.Count > 0)
            {
                foreach(Bookmark bookmark in bookmarks)
                {
                    if (bookmark.Name.StartsWith(PREFISSO_BOOKMARK_DOCUMENTO_NUMERO_OFFERTA, StringComparison.InvariantCultureIgnoreCase))
                    {
                        bookmark.Range.Text = Offerta.Numero.ToString();
                    }
                    else if (bookmark.Name.StartsWith(PREFISSO_BOOKMARK_DOCUMENTO_NUMERO_REVISIONE_OFFERTA, StringComparison.InvariantCultureIgnoreCase))
                    {
                        bookmark.Range.Text = Offerta.NumeroRevisione.ToString();
                    }
                    else if (bookmark.Name.StartsWith(PREFISSO_BOOKMARK_DOCUMENTO_DATA_OFFERTA, StringComparison.InvariantCultureIgnoreCase))
                    {
                        bookmark.Range.Text = Offerta.Data.ToString("dd/MM/yyyy");
                    }
                    else if (bookmark.Name.StartsWith(PREFISSO_BOOKMARK_DOCUMENTO_TITOLO_OFFERTA, StringComparison.InvariantCultureIgnoreCase))
                    {
                        bookmark.Range.Text = Offerta.Titolo?.ToString() ?? String.Empty;
                    }
                    else if (bookmark.Name.StartsWith(PREFISSO_BOOKMARK_DOCUMENTO_CODICE_CLIENTE, StringComparison.InvariantCultureIgnoreCase))
                    {
                        bookmark.Range.Text = Offerta.CodiceCliente?.ToString() ?? String.Empty;
                    }
                    else if (bookmark.Name.StartsWith(PREFISSO_BOOKMARK_DOCUMENTO_RAGIONE_SOCIALE_CLIENTE, StringComparison.InvariantCultureIgnoreCase))
                    {
                        bookmark.Range.Text = Offerta.RagioneSociale?.ToString() ?? String.Empty;
                    }
                    else if (bookmark.Name.StartsWith(PREFISSO_BOOKMARK_DOCUMENTO_DESTINAZIONE_MERCE, StringComparison.InvariantCultureIgnoreCase))
                    {
                        bookmark.Range.Text = Offerta.DestinazioneMerce?.ToString() ?? String.Empty;
                    }
                    else if (bookmark.Name.StartsWith(PREFISSO_BOOKMARK_DOCUMENTO_INDIRIZZO_CLIENTE, StringComparison.InvariantCultureIgnoreCase))
                    {
                        bookmark.Range.Text = Offerta.Indirizzo?.ToString() ?? String.Empty;
                    }
                    else if (bookmark.Name.StartsWith(PREFISSO_BOOKMARK_DOCUMENTO_CAP_CLIENTE, StringComparison.InvariantCultureIgnoreCase))
                    {
                        bookmark.Range.Text = Offerta.CAP?.ToString() ?? String.Empty;
                    }
                    else if (bookmark.Name.StartsWith(PREFISSO_BOOKMARK_DOCUMENTO_LOCALITA_CLIENTE, StringComparison.InvariantCultureIgnoreCase))
                    {
                        bookmark.Range.Text = Offerta.Localita?.ToString() ?? String.Empty;
                    }
                    else if (bookmark.Name.StartsWith(PREFISSO_BOOKMARK_DOCUMENTO_PROVINCIA_CLIENTE, StringComparison.InvariantCultureIgnoreCase))
                    {
                        bookmark.Range.Text = Offerta.Provincia?.ToString() ?? String.Empty;
                    }
                    else if (bookmark.Name.StartsWith(PREFISSO_BOOKMARK_DOCUMENTO_TELEFONO_CLIENTE, StringComparison.InvariantCultureIgnoreCase))
                    {
                        bookmark.Range.Text = Offerta.Telefono?.ToString() ?? String.Empty;
                    }
                    else if (bookmark.Name.StartsWith(PREFISSO_BOOKMARK_DOCUMENTO_TOTALE_COSTO, StringComparison.InvariantCultureIgnoreCase))
                    {
                        bookmark.Range.Text = Offerta.TotaleCosto?.ToString("F2") ?? String.Empty;
                    }
                    else if (bookmark.Name.StartsWith(PREFISSO_BOOKMARK_DOCUMENTO_TOTALE_VENDITA_CALCOLATO, StringComparison.InvariantCultureIgnoreCase))
                    {
                        bookmark.Range.Text = Offerta.TotaleVenditaConSpesaCacolato?.ToString("F2") ?? String.Empty;
                    }
                    else if (bookmark.Name.StartsWith(PREFISSO_BOOKMARK_DOCUMENTO_TOTALE_RICARICO_VALUTA, StringComparison.InvariantCultureIgnoreCase))
                    {
                        bookmark.Range.Text = Offerta.TotaleRicaricoValuta?.ToString("F2") ?? String.Empty;
                    }
                    else if (bookmark.Name.StartsWith(PREFISSO_BOOKMARK_DOCUMENTO_TOTALE_RICARICO_PERCENTUALE, StringComparison.InvariantCultureIgnoreCase))
                    {
                        bookmark.Range.Text = Offerta.TotaleRicaricoPercentuale?.ToString("F2") ?? String.Empty;
                    }
                    else if (bookmark.Name.StartsWith(PREFISSO_BOOKMARK_DOCUMENTO_TOTALE_VENDITA, StringComparison.InvariantCultureIgnoreCase))
                    {
                        bookmark.Range.Text = Offerta.TotaleVenditaConSpesa?.ToString("F2") ?? String.Empty;
                    }
                    else if (bookmark.Name.StartsWith(PREFISSO_BOOKMARK_DOCUMENTO_CODICE_COMMESSA, StringComparison.InvariantCultureIgnoreCase))
                    {
                        bookmark.Range.Text = Offerta.CodiceCommessa?.ToString() ?? String.Empty;
                    }
                    else if (bookmark.Name.StartsWith(PREFISSO_BOOKMARK_DOCUMENTO_GIORNI_VALIDITA, StringComparison.InvariantCultureIgnoreCase))
                    {
                        string tipologiaGiorni = Offerta.TipologiaGiorniValiditaEnum.HasValue ? " " + Offerta.TipologiaGiorniValiditaEnum.Value.GetDescription().ToLower() : String.Empty;
                        bookmark.Range.Text = (Offerta.GiorniValidita?.ToString() + tipologiaGiorni) ?? String.Empty;
                    }
                    else if (bookmark.Name.StartsWith(PREFISSO_BOOKMARK_DOCUMENTO_TEMPI_DI_CONSEGNA, StringComparison.InvariantCultureIgnoreCase))
                    {
                        string tipologiaGiorni = Offerta.TipologiaTempiDiConsegnaEnum.HasValue ? " " + Offerta.TipologiaTempiDiConsegnaEnum.Value.GetDescription().ToLower() : String.Empty;
                        bookmark.Range.Text = (Offerta.TempiDiConsegna?.ToString() + tipologiaGiorni) ?? String.Empty;
                    }
                    else if (bookmark.Name.StartsWith(PREFISSO_BOOKMARK_DOCUMENTO_NOTE_INTERNE, StringComparison.InvariantCultureIgnoreCase))
                    {
                        InsertHtmlOnBokmark(bookmark, Offerta.NoteInterne ?? String.Empty);
                    }
                    else if (bookmark.Name.StartsWith(PREFISSO_BOOKMARK_DOCUMENTO_NOTE_RIFIUTO, StringComparison.InvariantCultureIgnoreCase))
                    {
                        InsertHtmlOnBokmark(bookmark, Offerta.NoteRifiuto ?? String.Empty);
                    }
                    else if (bookmark.Name.StartsWith(PREFISSO_BOOKMARK_DOCUMENTO_TESTO_PIE_DI_PAGINA, StringComparison.InvariantCultureIgnoreCase))
                    {
                        InsertHtmlOnBokmark(bookmark, Offerta.TestoPieDiPagina ?? String.Empty);
                    }
                    else if (bookmark.Name.StartsWith(PREFISSO_BOOKMARK_DOCUMENTO_TESTO_INTESTAZIONE, StringComparison.InvariantCultureIgnoreCase))
                    {
                        InsertHtmlOnBokmark(bookmark, Offerta.TestoIntestazione ?? String.Empty);
                    }
                    else if (bookmark.Name.StartsWith(PREFISSO_BOOKMARK_DOCUMENTO_TESTO_VALIDITA_OFFERTA, StringComparison.InvariantCultureIgnoreCase))
                    {
                        bookmark.Range.Text = Offerta.TestoValiditaOfferta?.ToString() ?? String.Empty;
                    }
                    else if (bookmark.Name.StartsWith(PREFISSO_BOOKMARK_DOCUMENTO_TESTO_EMAIL_INVIATA_AL_CLIENTE, StringComparison.InvariantCultureIgnoreCase))
                    {
                        InsertHtmlOnBokmark(bookmark, Offerta.TestoEmailInviataAlCliente?.ToString() ?? String.Empty);
                    }
                    else if (bookmark.Name.StartsWith(PREFISSO_BOOKMARK_DOCUMENTO_CODICE_PAGAMENTO, StringComparison.InvariantCultureIgnoreCase))
                    {
                        bookmark.Range.Text = Offerta.CodicePagamento?.ToString() ?? String.Empty;
                    }
                    else if (bookmark.Name.StartsWith(PREFISSO_BOOKMARK_DOCUMENTO_DESCRIZIONE_PAGAMENTO, StringComparison.InvariantCultureIgnoreCase))
                    {
                        bookmark.Range.Text = Offerta.DescrizionePagamento?.ToString() ?? String.Empty;
                    }
                    else if (bookmark.Name.StartsWith(PREFISSO_BOOKMARK_DOCUMENTO_CODICE_IBAN, StringComparison.InvariantCultureIgnoreCase))
                    {
                        bookmark.Range.Text = Offerta.CodiceIBAN?.ToString() ?? String.Empty;
                    }
                    else if (bookmark.Name.StartsWith(PREFISSO_BOOKMARK_DOCUMENTO_SE_BONIFICO_CODICE_IBAN, StringComparison.InvariantCultureIgnoreCase))
                    {
                        if (Offerta.CodicePagamento == "B")
                        {
                            bookmark.Range.Text = $"IBAN: {Offerta.CodiceIBAN?.ToString() ?? String.Empty}";
                        }
                        else
                        {
                            bookmark.Range.Text = String.Empty;
                        }
                    }
                    else if (bookmark.Name.StartsWith(PREFISSO_BOOKMARK_DOCUMENTO_TOTALE_COSTO_CALCOLATO, StringComparison.InvariantCultureIgnoreCase))
                    {
                        bookmark.Range.Text = Offerta.TotaleCostoCalcolato?.ToString("F2") ?? String.Empty;
                    }
                    else if (bookmark.Name.StartsWith(PREFISSO_BOOKMARK_DOCUMENTO_TOTALE_RICARICO_VALUTA_CALCOLATO, StringComparison.InvariantCultureIgnoreCase))
                    {
                        bookmark.Range.Text = Offerta.TotaleRicaricoValutaCalcolato?.ToString("F2") ?? String.Empty;
                    }
                    else if (bookmark.Name.StartsWith(PREFISSO_BOOKMARK_DOCUMENTO_TOTALE_RICARICO_PERCENTUALE_CALCOLATO, StringComparison.InvariantCultureIgnoreCase))
                    {
                        bookmark.Range.Text = Offerta.TotaleRicaricoPercentualeCalcolato?.ToString("F2") ?? String.Empty;
                    }
                    else if (bookmark.Name.StartsWith(PREFISSO_BOOKMARK_DOCUMENTO_TESTO_SEZIONE_PAGAMENTI, StringComparison.InvariantCultureIgnoreCase))
                    {
                        InsertHtmlOnBokmark(bookmark, Offerta.TestoSezionePagamenti?.ToString() ?? String.Empty);
                    }
                    else if (bookmark.Name.StartsWith(PREFISSO_BOOKMARK_DOCUMENTO_SERVIZI, StringComparison.InvariantCultureIgnoreCase))
                    {
                        InsertServizi(application, document, bookmark, llConfigurazioneArticoliAggiuntivi, elencoArticoliUnivoci);
                    }
                    else if (bookmark.Name.StartsWith(PREFISSO_BOOKMARK_DOCUMENTO_ARTICOLI, StringComparison.InvariantCultureIgnoreCase))
                    {
                        InsertTableArticoli(application, document, bookmark, llConfigurazioneArticoliAggiuntivi);
                    }
                    else if (bookmark.Name.StartsWith(PREFISSO_BOOKMARK_DOCUMENTO_CONTRATTI_SENZA_CLAUSOLE_VESSATORIE, StringComparison.InvariantCultureIgnoreCase))
                    {
                        InsertTableContrattiClausoleVessatorie(application, document, bookmark, llConfigurazioneArticoliAggiuntivi, elencoArticoliUnivoci, true);
                    }
                    else if (bookmark.Name.StartsWith(PREFISSO_BOOKMARK_DOCUMENTO_CONTRATTI_CON_CLAUSOLE_VESSATORIE, StringComparison.InvariantCultureIgnoreCase))
                    {
                        InsertTableContrattiClausoleVessatorie(application, document, bookmark, llConfigurazioneArticoliAggiuntivi, elencoArticoliUnivoci, false);
                    }
                    else if (bookmark.Name.StartsWith(PREFISSO_BOOKMARK_DOCUMENTO_ELENCO_CLAUSOLE_VESSATORIE_FISSE, StringComparison.InvariantCultureIgnoreCase))
                    {
                        InsertElencoClausoleVessatorieFisse(application, document, bookmark, llConfigurazioneArticoliAggiuntivi, elencoArticoliUnivoci);
                    }
                    else if (bookmark.Name.StartsWith(PREFISSO_BOOKMARK_DOCUMENTO_ELENCO_CLAUSOLE_VESSATORIE_VARIABILI, StringComparison.InvariantCultureIgnoreCase))
                    {
                        InsertElencoClausoleVessatorieVariabili(application, document, bookmark, llConfigurazioneArticoliAggiuntivi, elencoArticoliUnivoci);
                    }
                }
            }
        }

        /// <summary>
        /// Effettua l'inserimento dei servizi presenti nell'offerta
        /// </summary>
        /// <param name="application"></param>
        /// <param name="document"></param>
        /// <param name="bookmark"></param>
        /// <param name="ll"></param>
        /// <param name="elencoArticoliUnivoci"></param>
        private void InsertServizi(Application application, Document document, Bookmark bookmark, Logic.AnalisiVenditeConfigurazioneArticoliAggiuntivi ll, OffertaArticolo[] elencoArticoliUnivoci)
        {
            if (elencoArticoliUnivoci != null && elencoArticoliUnivoci.Length > 0)
            {
                Servizio[] elencoServizi = GetElencoServiziByArticoliOfferta(ll, elencoArticoliUnivoci);
                List<OffertaArticolo> elencoArticoliAnalizzati = new List<OffertaArticolo>();
                List<AnalisiVenditaConfigurazioneArticoloAggiuntivo> elencoConfigurazioniArticoliAggiuntiviUtilizzati = new List<AnalisiVenditaConfigurazioneArticoloAggiuntivo>();
                
                StringBuilder sb = new StringBuilder();

                if (elencoServizi != null && elencoServizi.Length > 0)
                {
                    foreach(Servizio servizio in elencoServizi)
                    {
                        if (!String.IsNullOrWhiteSpace(servizio.Descrizione))
                        {
                            sb.AppendLine(servizio.Descrizione);
                        }

                        if (servizio.ServizioArticolos.Any())
                        {
                            foreach (ServizioArticolo articoloServizio in servizio.ServizioArticolos.OrderBy(x => x.CodiceAnagraficaArticolo))
                            {
                                OffertaArticolo[] articoliOfferteByCodiceArticolo = elencoArticoliUnivoci.Where(x => x.CodiceArticolo.ToLower().Trim() == articoloServizio.CodiceAnagraficaArticolo.ToLower().Trim()).Distinct().ToArray();
                                if (articoliOfferteByCodiceArticolo != null && articoliOfferteByCodiceArticolo.Length > 0)
                                {
                                    foreach(OffertaArticolo offertaArticolo in articoliOfferteByCodiceArticolo)
                                    {
                                        elencoArticoliAnalizzati.Add(offertaArticolo);
                                        GestioneTestoDocumentoDaOffertaArticolo(ll, offertaArticolo, elencoConfigurazioniArticoliAggiuntiviUtilizzati, sb);
                                    }
                                }
                            }
                        }
                    }
                }

                foreach (OffertaArticolo offertaArticolo in elencoArticoliUnivoci)
                {
                    if (!elencoArticoliAnalizzati.Any(x => x.ID == offertaArticolo.ID))
                    {
                        elencoArticoliAnalizzati.Add(offertaArticolo);

                        GestioneTestoDocumentoDaOffertaArticolo(ll, offertaArticolo, elencoConfigurazioniArticoliAggiuntiviUtilizzati, sb);
                    }
                }

                Range range = bookmark.Range;
                InsertHtmlOnRange(range, sb.ToString(), wdRecoveryType: WdRecoveryType.wdFormatOriginalFormatting);
            }
            else
            {
                bookmark.Range.Text = String.Empty;
            }
        }

        /// <summary>
        /// Effettua l'inserimento della tabella relativa agli articoli
        /// </summary>
        /// <param name="application"></param>
        /// <param name="document"></param>
        /// <param name="bookmark"></param>
        /// <param name="ll"></param>
        private void InsertTableArticoli(Application application, Document document, Bookmark bookmark, Logic.AnalisiVenditeConfigurazioneArticoliAggiuntivi ll)
        {   
            if ((Offerta?.OffertaRaggruppamentos?.Count ?? 0) > 0)
            {
                int conteggioRighe = 0;
                if ((Offerta?.OffertaRaggruppamentos?.Count ?? 0) > 0)
                {
                    foreach (OffertaRaggruppamento offertaRaggruppamento in Offerta.OffertaRaggruppamentos)
                    {
                        if (!offertaRaggruppamento.OpzioneStampaOffertaEnum.HasFlag(OffertaRaggruppamentoOpzioneStampaOffertaEnum.MostraRaggruppamento))
                        {
                            continue;
                        }

                        if (offertaRaggruppamento.OpzioneStampaOffertaEnum.HasFlag(OffertaRaggruppamentoOpzioneStampaOffertaEnum.MostraIntestazioniRaggruppamento))
                        {
                            conteggioRighe++;
                        }

                        if ((offertaRaggruppamento?.OffertaArticolos?.Count ?? 0) > 0)
                        {
                            if (offertaRaggruppamento.OpzioneStampaOffertaEnum.HasFlag(OffertaRaggruppamentoOpzioneStampaOffertaEnum.MostraIntestazioniArticoli))
                            {
                                conteggioRighe++;
                            }

                            if (offertaRaggruppamento.OpzioneStampaOffertaEnum.HasFlag(OffertaRaggruppamentoOpzioneStampaOffertaEnum.MostraArticoli))
                            {
                                foreach (OffertaArticolo offertaArticolo in offertaRaggruppamento.OffertaArticolos)
                                {
                                    conteggioRighe++;
                                }
                            }                                
                        }

                        if (offertaRaggruppamento.OpzioneStampaOffertaEnum.HasFlag(OffertaRaggruppamentoOpzioneStampaOffertaEnum.MostraTotaliRaggruppamento))
                        {
                            conteggioRighe++;
                        }                            
                    }

                    if (!Offerta.OffertaRaggruppamentos.All(x => !x.OpzioneStampaOffertaEnum.HasFlag(OffertaRaggruppamentoOpzioneStampaOffertaEnum.MostraRaggruppamento)))
                    {
                        conteggioRighe++;
                    }
                    
                    conteggioRighe++;
                }

                Table table = bookmark.Range.Tables.Add(bookmark.Range, conteggioRighe, 7, null, WdAutoFitBehavior.wdAutoFitWindow);
                table.Borders.Enable = 1;

                application.Selection.Collapse();

                int indiceColonnaArticoliRichiesti = 1;
                int indiceColonnaCodiceArticolo = 2;
                int indiceColonnaDescrizioneArticolo = 3;
                int indiceColonnaUnitaDiMisura = 4;
                int indiceColonnaQuantita = 5;
                int indiceColonnaImportoUnitario = 6;
                int indiceColonnaTotaleImporto = 7;

                int incr = 0;

                foreach (OffertaRaggruppamento offertaRaggruppamento in Offerta.OffertaRaggruppamentos.OrderBy(x => x.Ordine))
                {
                    if (offertaRaggruppamento.OpzioneStampaOffertaEnum.HasFlag(OffertaRaggruppamentoOpzioneStampaOffertaEnum.NessunaOpzione) || 
                        !offertaRaggruppamento.OpzioneStampaOffertaEnum.HasFlag(OffertaRaggruppamentoOpzioneStampaOffertaEnum.MostraRaggruppamento))
                    {
                        continue;
                    }

                    if (offertaRaggruppamento.OpzioneStampaOffertaEnum.HasFlag(OffertaRaggruppamentoOpzioneStampaOffertaEnum.MostraIntestazioniRaggruppamento))
                    {
                        incr++;
                        Row rigaOpzione = table.Rows[incr];

                        rigaOpzione.Cells[indiceColonnaArticoliRichiesti].Range.Text = "OPZIONE";
                        rigaOpzione.Cells[indiceColonnaArticoliRichiesti].Range.Bold = 1;
                        rigaOpzione.Cells[indiceColonnaArticoliRichiesti].Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphRight;
                        rigaOpzione.Cells[indiceColonnaArticoliRichiesti].Range.Shading.BackgroundPatternColor = WdColor.wdColorGray05;

                        rigaOpzione.Cells[indiceColonnaCodiceArticolo].Range.Text = offertaRaggruppamento.Denominazione;
                        rigaOpzione.Cells[indiceColonnaCodiceArticolo].Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
                        rigaOpzione.Cells[indiceColonnaCodiceArticolo].Merge(rigaOpzione.Cells[indiceColonnaTotaleImporto]);
                    }

                    Row rigaIntestazioni = null;
                    if (offertaRaggruppamento.OpzioneStampaOffertaEnum.HasFlag(OffertaRaggruppamentoOpzioneStampaOffertaEnum.MostraIntestazioniArticoli))
                    {
                        incr++;
                        rigaIntestazioni = table.Rows[incr];

                        rigaIntestazioni.Cells[indiceColonnaArticoliRichiesti].Range.Text = "Articoli Richiesti";
                        rigaIntestazioni.Cells[indiceColonnaCodiceArticolo].Range.Text = "COD.";
                        rigaIntestazioni.Cells[indiceColonnaDescrizioneArticolo].Range.Text = "Descrizione";
                        rigaIntestazioni.Cells[indiceColonnaUnitaDiMisura].Range.Text = "UM";
                        rigaIntestazioni.Cells[indiceColonnaQuantita].Range.Text = "Quantità";
                        rigaIntestazioni.Cells[indiceColonnaImportoUnitario].Range.Text = "Importo Unitario in €";
                        rigaIntestazioni.Cells[indiceColonnaTotaleImporto].Range.Text = "Totale Importo in €";
                    }

                    if ((offertaRaggruppamento?.OffertaArticolos?.Count ?? 0) > 0)
                    {
                        if (offertaRaggruppamento.OpzioneStampaOffertaEnum.HasFlag(OffertaRaggruppamentoOpzioneStampaOffertaEnum.MostraArticoli))
                        {
                            foreach (OffertaArticolo offertaArticolo in offertaRaggruppamento.OffertaArticolos.OrderBy(x => x.Ordine))
                            {
                                if (!offertaArticolo.Quantita.HasValue) offertaArticolo.Quantita = 0;
                                if (!offertaArticolo.TotaleVenditaConSpesa.HasValue) offertaArticolo.TotaleVenditaConSpesa = 0;

                                incr++;
                                Row rigaArticolo = table.Rows[incr];

                                rigaArticolo.Cells[indiceColonnaArticoliRichiesti].Range.Font.Name = "Wingdings";
                                rigaArticolo.Cells[indiceColonnaArticoliRichiesti].Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                                rigaArticolo.Cells[indiceColonnaArticoliRichiesti].Range.Text = ((char)168).ToString();
                                rigaArticolo.Cells[indiceColonnaArticoliRichiesti].Range.Font.Size = 20;

                                rigaArticolo.Cells[indiceColonnaCodiceArticolo].Range.Text = offertaArticolo.CodiceArticolo;
                                rigaArticolo.Cells[indiceColonnaDescrizioneArticolo].Range.Text = offertaArticolo.Descrizione;
                                rigaArticolo.Cells[indiceColonnaUnitaDiMisura].Range.Text = offertaArticolo.UnitaMisura;
                                rigaArticolo.Cells[indiceColonnaUnitaDiMisura].Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                                rigaArticolo.Cells[indiceColonnaQuantita].Range.Text = ((int)offertaArticolo.Quantita.Value).ToString();
                                rigaArticolo.Cells[indiceColonnaQuantita].Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;

                                if (offertaRaggruppamento.OpzioneStampaOffertaEnum.HasFlag(OffertaRaggruppamentoOpzioneStampaOffertaEnum.MostraTotaliArticolo))
                                {
                                    decimal importoUnitario = 0;
                                    if (!(offertaArticolo.TotaleVenditaConSpesa.Value == 0 && offertaArticolo.Quantita.Value == 0))
                                    {
                                        importoUnitario = ((decimal)(offertaArticolo.TotaleVenditaConSpesa.Value / offertaArticolo.Quantita.Value));
                                    }

                                    rigaArticolo.Cells[indiceColonnaImportoUnitario].Range.Text = ((decimal)importoUnitario).ToString("F2");
                                    rigaArticolo.Cells[indiceColonnaImportoUnitario].Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                                    rigaArticolo.Cells[indiceColonnaTotaleImporto].Range.Text = offertaArticolo.TotaleVenditaConSpesa.Value.ToString("F2");
                                    rigaArticolo.Cells[indiceColonnaTotaleImporto].Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                                }                 
                                else
                                {
                                    rigaArticolo.Cells[indiceColonnaImportoUnitario].Range.Text = String.Empty;
                                    rigaArticolo.Cells[indiceColonnaImportoUnitario].Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                                    rigaArticolo.Cells[indiceColonnaTotaleImporto].Range.Text = String.Empty;
                                    rigaArticolo.Cells[indiceColonnaTotaleImporto].Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                                }
                            }
                        }

                        if (offertaRaggruppamento.OpzioneStampaOffertaEnum.HasFlag(OffertaRaggruppamentoOpzioneStampaOffertaEnum.MostraTotaliRaggruppamento))
                        {
                            incr++;
                            Row rigaTotaleOpzione = table.Rows[incr];

                            rigaTotaleOpzione.Cells[indiceColonnaQuantita].Range.Text = "TOTALE OPZIONE";
                            rigaTotaleOpzione.Cells[indiceColonnaQuantita].Range.Shading.BackgroundPatternColor = WdColor.wdColorGray05;
                            rigaTotaleOpzione.Cells[indiceColonnaQuantita].Range.Bold = 1;

                            rigaTotaleOpzione.Cells[indiceColonnaTotaleImporto].Range.Text = offertaRaggruppamento.TotaleVenditaConSpesa?.ToString("F2") ?? "0";
                            rigaTotaleOpzione.Cells[indiceColonnaTotaleImporto].Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;

                            rigaTotaleOpzione.Cells[indiceColonnaQuantita].Merge(rigaTotaleOpzione.Cells[indiceColonnaImportoUnitario]);
                            rigaTotaleOpzione.Cells[indiceColonnaQuantita].Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;

                            rigaTotaleOpzione.Cells[indiceColonnaArticoliRichiesti].Merge(rigaTotaleOpzione.Cells[indiceColonnaUnitaDiMisura]);
                        }                            
                    }

                    if (rigaIntestazioni != null)
                    {
                        rigaIntestazioni.Cells[indiceColonnaArticoliRichiesti].Range.Bold = 1;
                        rigaIntestazioni.Cells[indiceColonnaArticoliRichiesti].Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                        rigaIntestazioni.Cells[indiceColonnaArticoliRichiesti].Range.Shading.BackgroundPatternColor = WdColor.wdColorGray05;

                        rigaIntestazioni.Cells[indiceColonnaCodiceArticolo].Range.Bold = 1;
                        rigaIntestazioni.Cells[indiceColonnaCodiceArticolo].Range.Shading.BackgroundPatternColor = WdColor.wdColorGray05;

                        rigaIntestazioni.Cells[indiceColonnaDescrizioneArticolo].Range.Bold = 1;
                        rigaIntestazioni.Cells[indiceColonnaDescrizioneArticolo].Range.Shading.BackgroundPatternColor = WdColor.wdColorGray05;

                        rigaIntestazioni.Cells[indiceColonnaUnitaDiMisura].Range.Bold = 1;
                        rigaIntestazioni.Cells[indiceColonnaUnitaDiMisura].Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                        rigaIntestazioni.Cells[indiceColonnaUnitaDiMisura].Range.Shading.BackgroundPatternColor = WdColor.wdColorGray05;

                        rigaIntestazioni.Cells[indiceColonnaQuantita].Range.Bold = 1;
                        rigaIntestazioni.Cells[indiceColonnaQuantita].Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                        rigaIntestazioni.Cells[indiceColonnaQuantita].Range.Shading.BackgroundPatternColor = WdColor.wdColorGray05;

                        rigaIntestazioni.Cells[indiceColonnaImportoUnitario].Range.Bold = 1;
                        rigaIntestazioni.Cells[indiceColonnaImportoUnitario].Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                        rigaIntestazioni.Cells[indiceColonnaImportoUnitario].Range.Shading.BackgroundPatternColor = WdColor.wdColorGray05;

                        rigaIntestazioni.Cells[indiceColonnaTotaleImporto].Range.Bold = 1;
                        rigaIntestazioni.Cells[indiceColonnaTotaleImporto].Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                        rigaIntestazioni.Cells[indiceColonnaTotaleImporto].Range.Shading.BackgroundPatternColor = WdColor.wdColorGray05;
                    }                    
                }

                if (!Offerta.OffertaRaggruppamentos.All(x => !x.OpzioneStampaOffertaEnum.HasFlag(OffertaRaggruppamentoOpzioneStampaOffertaEnum.MostraRaggruppamento)))
                {
                    incr++;
                    Row rigaVuotaPrimaDelTotaleOfferta = table.Rows[incr];
                    rigaVuotaPrimaDelTotaleOfferta.Cells[indiceColonnaArticoliRichiesti].Merge(rigaVuotaPrimaDelTotaleOfferta.Cells[indiceColonnaTotaleImporto]);
                }

                incr++;
                Row rigaTotaleOfferta = table.Rows[incr];
                rigaTotaleOfferta.Cells[indiceColonnaArticoliRichiesti].Range.Text = "TOTALE OFFERTA";
                rigaTotaleOfferta.Cells[indiceColonnaArticoliRichiesti].Range.Bold = 1;
                rigaTotaleOfferta.Cells[indiceColonnaArticoliRichiesti].Range.Shading.BackgroundPatternColor = WdColor.wdColorGray10;

                rigaTotaleOfferta.Cells[indiceColonnaTotaleImporto].Range.Text = Offerta.TotaleVenditaConSpesa?.ToString("F2") ?? "0";
                rigaTotaleOfferta.Cells[indiceColonnaTotaleImporto].Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;

                rigaTotaleOfferta.Cells[indiceColonnaArticoliRichiesti].Merge(rigaTotaleOfferta.Cells[indiceColonnaImportoUnitario]);
                rigaTotaleOfferta.Cells[indiceColonnaArticoliRichiesti].Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphRight;
            }
            else
            {
                bookmark.Range.Text = String.Empty;
            }
        }

        /// <summary>
        /// Effettua l'inserimento dei contratti con o senza le clausole vessatorie
        /// </summary>
        /// <param name="application"></param>
        /// <param name="document"></param>
        /// <param name="bookmark"></param>
        /// <param name="ll"></param>
        /// <param name="elencoArticoliUnivoci"></param>
        /// <param name="includereClausoleVessatorie"></param>
        private void InsertTableContrattiClausoleVessatorie(Application application, Document document, Bookmark bookmark, Logic.AnalisiVenditeConfigurazioneArticoliAggiuntivi ll, OffertaArticolo[] elencoArticoliUnivoci, bool includereClausoleVessatorie)
        {
            if (elencoArticoliUnivoci != null && elencoArticoliUnivoci.Length > 0)
            {
                AnalisiVenditaConfigurazioneArticoloAggiuntivo[] configurazioniContratti = GetConfigurazioniContrattiPreventivi(ll, elencoArticoliUnivoci, TipologiaAnalisiVenditaConfigurazioneArticoloAggiuntivo.Template_Articolo_Contratto);

                if (configurazioniContratti != null && configurazioniContratti.Length > 0)
                {
                    Table table = bookmark.Range.Tables.Add(bookmark.Range, 1, 2, null, WdAutoFitBehavior.wdAutoFitWindow);
                    table.Borders.Enable = 1;

                    application.Selection.Collapse();

                    List<Row> righeDaUnire = new List<Row>();

                    bool primaRiga = true;

                    foreach (AnalisiVenditaConfigurazioneArticoloAggiuntivo offertaArticolo in configurazioniContratti)
                    {
                        Row rigaIniziale = (primaRiga) ? table.Rows[1] : table.Rows.Add();
                        primaRiga = false;
                        rigaIniziale.Cells[1].Range.Select();
                        application.Selection.ClearFormatting();
                        application.Selection.Collapse();

                        rigaIniziale.Cells[1].Range.Text = "Clausola Contratto".ToUpper();
                        rigaIniziale.Cells[1].Range.Bold = 1;
                        rigaIniziale.Cells[1].Range.ParagraphFormat.SpaceBefore = 0;
                        rigaIniziale.Cells[1].Range.ParagraphFormat.SpaceAfter = 0;
                        rigaIniziale.Cells[1].Range.Shading.BackgroundPatternColor = WdColor.wdColorGray05;
                        rigaIniziale.Cells[2].Range.Shading.BackgroundPatternColor = WdColor.wdColorGray05;
                        rigaIniziale.HeightRule = WdRowHeightRule.wdRowHeightAuto;

                        righeDaUnire.Add(rigaIniziale);

                        Row rigaContratto = table.Rows.Add();
                        InsertHtmlOnRange(rigaContratto.Cells[1].Range, offertaArticolo.TestoDescrizioneContratto, wdRecoveryType:WdRecoveryType.wdFormatOriginalFormatting);

                        rigaContratto.Cells[1].Range.ParagraphFormat.SpaceBefore = 0;
                        rigaContratto.Cells[1].Range.ParagraphFormat.SpaceAfter = 0;
                        rigaContratto.HeightRule = WdRowHeightRule.wdRowHeightAuto;
                        rigaContratto.Cells[1].Range.Shading.BackgroundPatternColor = WdColor.wdColorWhite;
                        rigaContratto.Cells[2].Range.Shading.BackgroundPatternColor = WdColor.wdColorWhite;


                        righeDaUnire.Add(rigaContratto);

                        if (includereClausoleVessatorie)
                        {
                            if (offertaArticolo.AnalisiVenditaConfigurazioneArticoloAggiuntivoClausolaVessatorias != null && offertaArticolo.AnalisiVenditaConfigurazioneArticoloAggiuntivoClausolaVessatorias.Count > 0)
                            {
                                Row rigaIntestazioneClausoleVessatorie = table.Rows.Add();
                                rigaIntestazioneClausoleVessatorie.Cells[1].Range.Shading.BackgroundPatternColor = WdColor.wdColorGray05;
                                rigaIntestazioneClausoleVessatorie.Cells[2].Range.Shading.BackgroundPatternColor = WdColor.wdColorGray05;
                                rigaIntestazioneClausoleVessatorie.Cells[1].Range.Text = "Clausole Vessatorie";

                                rigaIntestazioneClausoleVessatorie.Cells[1].Range.ParagraphFormat.SpaceBefore = 0;
                                rigaIntestazioneClausoleVessatorie.Cells[1].Range.ParagraphFormat.SpaceAfter = 0;
                                rigaIntestazioneClausoleVessatorie.HeightRule = WdRowHeightRule.wdRowHeightAuto;

                                righeDaUnire.Add(rigaIntestazioneClausoleVessatorie);

                                Row rigaIntestazioneClausoleVessatorieIntestazioniColonne = table.Rows.Add();
                                rigaIntestazioneClausoleVessatorieIntestazioniColonne.Cells[1].Range.Text = "Codice";
                                rigaIntestazioneClausoleVessatorieIntestazioniColonne.Cells[2].Range.Text = "Descrizione";
                                rigaIntestazioneClausoleVessatorieIntestazioniColonne.Cells[1].Range.Shading.BackgroundPatternColor = WdColor.wdColorGray05;
                                rigaIntestazioneClausoleVessatorieIntestazioniColonne.Cells[2].Range.Shading.BackgroundPatternColor = WdColor.wdColorGray05;


                                foreach (AnalisiVenditaConfigurazioneArticoloAggiuntivoClausolaVessatoria articoloClausolaVessatoria in offertaArticolo.AnalisiVenditaConfigurazioneArticoloAggiuntivoClausolaVessatorias.OrderBy(x => x.ClausolaVessatoria.Codice))
                                {
                                    Row rigaClausolaVessatoria = table.Rows.Add();
                                    rigaClausolaVessatoria.Cells[1].Range.Text = articoloClausolaVessatoria.ClausolaVessatoria.Codice;
                                    rigaClausolaVessatoria.Cells[1].Range.ParagraphFormat.SpaceBefore = 0;
                                    rigaClausolaVessatoria.Cells[1].Range.ParagraphFormat.SpaceAfter = 0;
                                    rigaClausolaVessatoria.HeightRule = WdRowHeightRule.wdRowHeightAuto;
                                    rigaClausolaVessatoria.Cells[1].Range.Shading.BackgroundPatternColor = WdColor.wdColorWhite;
                                    rigaClausolaVessatoria.Cells[2].Range.Shading.BackgroundPatternColor = WdColor.wdColorWhite;

                                    InsertHtmlOnRange(rigaClausolaVessatoria.Cells[2].Range, articoloClausolaVessatoria.ClausolaVessatoria.Descrizione, wdRecoveryType: WdRecoveryType.wdFormatOriginalFormatting);
                                }
                            }

                            if (!String.IsNullOrWhiteSpace(offertaArticolo.ClausoleVessatorieAggiuntive))
                            {
                                Row rigaIntestazioneClausoleVessatorieAggiuntive = table.Rows.Add();
                                rigaIntestazioneClausoleVessatorieAggiuntive.Cells[1].Range.Shading.BackgroundPatternColor = WdColor.wdColorGray05;
                                rigaIntestazioneClausoleVessatorieAggiuntive.Cells[2].Range.Shading.BackgroundPatternColor = WdColor.wdColorGray05;

                                rigaIntestazioneClausoleVessatorieAggiuntive.Cells[1].Range.Text = "Clausole Vessatorie Aggiuntive";
                                rigaIntestazioneClausoleVessatorieAggiuntive.Cells[1].Range.ParagraphFormat.SpaceBefore = 0;
                                rigaIntestazioneClausoleVessatorieAggiuntive.Cells[1].Range.ParagraphFormat.SpaceAfter = 0;
                                rigaIntestazioneClausoleVessatorieAggiuntive.HeightRule = WdRowHeightRule.wdRowHeightAuto;

                                righeDaUnire.Add(rigaIntestazioneClausoleVessatorieAggiuntive);

                                Row rigaClausoleVessatorieAggiuntive = table.Rows.Add();
                                InsertHtmlOnRange(rigaClausoleVessatorieAggiuntive.Cells[1].Range, offertaArticolo.ClausoleVessatorieAggiuntive, wdRecoveryType: WdRecoveryType.wdFormatOriginalFormatting);
                                rigaClausoleVessatorieAggiuntive.Cells[1].Range.ParagraphFormat.SpaceBefore = 0;
                                rigaClausoleVessatorieAggiuntive.Cells[1].Range.ParagraphFormat.SpaceAfter = 0;
                                rigaClausoleVessatorieAggiuntive.HeightRule = WdRowHeightRule.wdRowHeightAuto;
                                rigaClausoleVessatorieAggiuntive.Cells[1].Range.Shading.BackgroundPatternColor = WdColor.wdColorWhite;
                                rigaClausoleVessatorieAggiuntive.Cells[2].Range.Shading.BackgroundPatternColor = WdColor.wdColorWhite;

                                righeDaUnire.Add(rigaClausoleVessatorieAggiuntive);
                            }
                        }
                    }

                    table.Columns[1].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPoints;
                    table.Columns[1].SetWidth(50, WdRulerStyle.wdAdjustSameWidth);

                    table.Columns[2].PreferredWidthType = WdPreferredWidthType.wdPreferredWidthAuto;

                    if (righeDaUnire.Count > 0)
                    {
                        foreach(Row riga in righeDaUnire)
                        {
                            riga.Cells.Merge();
                        }
                    }
                }
                else
                {
                    bookmark.Range.Text = String.Empty;
                }
            }
            else
            {
                bookmark.Range.Text = String.Empty;
            }
        }

        /// <summary>
        /// Effettua l'inserimento l'elenco delle clausole vessatorie aggiuntive (fisse)
        /// </summary>
        /// <param name="application"></param>
        /// <param name="document"></param>
        /// <param name="bookmark"></param>
        /// <param name="ll"></param>
        /// <param name="elencoArticoliUnivoci"></param>
        /// <param name="includereClausoleVessatorie"></param>
        private void InsertElencoClausoleVessatorieFisse(Application application, Document document, Bookmark bookmark, Logic.AnalisiVenditeConfigurazioneArticoliAggiuntivi ll, OffertaArticolo[] elencoArticoliUnivoci)
        {
            Dictionary<string, string> elencoClausoleVessatorie = new Dictionary<string, string>();

            if (elencoArticoliUnivoci != null && elencoArticoliUnivoci.Length > 0)
            {
                AnalisiVenditaConfigurazioneArticoloAggiuntivo[] configurazioniContratti = GetConfigurazioniContrattiPreventivi(ll, elencoArticoliUnivoci, TipologiaAnalisiVenditaConfigurazioneArticoloAggiuntivo.Template_Articolo_Contratto);

                if (configurazioniContratti != null && configurazioniContratti.Length > 0)
                {
                    foreach (AnalisiVenditaConfigurazioneArticoloAggiuntivo offertaArticolo in configurazioniContratti)
                    {
                        if (offertaArticolo.AnalisiVenditaConfigurazioneArticoloAggiuntivoClausolaVessatorias != null && offertaArticolo.AnalisiVenditaConfigurazioneArticoloAggiuntivoClausolaVessatorias.Count > 0)
                        {
                            foreach (AnalisiVenditaConfigurazioneArticoloAggiuntivoClausolaVessatoria articoloClausolaVessatoria in offertaArticolo.AnalisiVenditaConfigurazioneArticoloAggiuntivoClausolaVessatorias.OrderBy(x => x.ClausolaVessatoria.Codice))
                            {
                                if (articoloClausolaVessatoria.ClausolaVessatoria != null && !elencoClausoleVessatorie.ContainsKey(articoloClausolaVessatoria.ClausolaVessatoria.Codice))
                                {
                                    string descrizione = articoloClausolaVessatoria.ClausolaVessatoria.Descrizione.Replace("<p>", String.Empty).Replace("</p>", String.Empty);
                                    elencoClausoleVessatorie.Add(articoloClausolaVessatoria.ClausolaVessatoria.Codice, descrizione);
                                }
                            }
                        }
                    }
                }
            }

            if (elencoClausoleVessatorie.Count > 0)
            {
                string htmlDaInserire = String.Join(", ", elencoClausoleVessatorie.Select(x => x.Value));
                InsertHtmlOnBokmark(bookmark, htmlDaInserire, true);                    
            }
            else
            {
                bookmark.Range.Text = String.Empty;
            }
        }

        /// <summary>
        /// Effettua l'inserimento l'elenco delle clausole vessatorie aggiuntive (variabili)
        /// </summary>
        /// <param name="application"></param>
        /// <param name="document"></param>
        /// <param name="bookmark"></param>
        /// <param name="ll"></param>
        /// <param name="elencoArticoliUnivoci"></param>
        /// <param name="includereClausoleVessatorie"></param>
        private void InsertElencoClausoleVessatorieVariabili(Application application, Document document, Bookmark bookmark, Logic.AnalisiVenditeConfigurazioneArticoliAggiuntivi ll, OffertaArticolo[] elencoArticoliUnivoci)
        {
            List<string> elencoClausoleVessatorieAggiuntive = new List<string>();

            if (elencoArticoliUnivoci != null && elencoArticoliUnivoci.Length > 0)
            {
                AnalisiVenditaConfigurazioneArticoloAggiuntivo[] configurazioniContratti = GetConfigurazioniContrattiPreventivi(ll, elencoArticoliUnivoci, TipologiaAnalisiVenditaConfigurazioneArticoloAggiuntivo.Template_Articolo_Contratto);

                if (configurazioniContratti != null && configurazioniContratti.Length > 0)
                {
                    foreach (AnalisiVenditaConfigurazioneArticoloAggiuntivo offertaArticolo in configurazioniContratti)
                    {
                        if (!String.IsNullOrWhiteSpace(offertaArticolo.ClausoleVessatorieAggiuntive))
                        {
                            string descrizione = offertaArticolo.ClausoleVessatorieAggiuntive.Replace("<p>", String.Empty).Replace("</p>", String.Empty);

                            if (!elencoClausoleVessatorieAggiuntive.Contains(descrizione))
                            {
                                elencoClausoleVessatorieAggiuntive.Add(descrizione);
                            }
                        }
                    }
                }
            }

            if (elencoClausoleVessatorieAggiuntive.Count > 0)
            {
                string htmlDaInserire = String.Join(", ", elencoClausoleVessatorieAggiuntive);
                InsertHtmlOnBokmark(bookmark, htmlDaInserire, true);
            }
            else
            {
                bookmark.Range.Text = String.Empty;
            }
        }

        /// <summary>
        /// Effettua l'inserimento di html nel bookmark passato come parametro
        /// </summary>
        /// <param name="bookmark"></param>
        /// <param name="html"></param>
        /// <param name="removeLastEndLineCharacterIfExists"></param>
        /// <param name="wdRecoveryType"></param>
        private void InsertHtmlOnBokmark(Bookmark bookmark, string html, bool removeLastEndLineCharacterIfExists = false, WdRecoveryType wdRecoveryType = WdRecoveryType.wdFormatSurroundingFormattingWithEmphasis)
        {
            InsertHtmlOnRange(bookmark.Range, html, removeLastEndLineCharacterIfExists, wdRecoveryType);
        }

        /// <summary>
        /// Effettua l'inserimento dell'html nel range passato come parametro
        /// </summary>
        /// <param name="range"></param>
        /// <param name="html"></param>
        /// <param name="removeLastEndLineCharacterIfExists"></param>
        /// <param name="wdRecoveryType"></param>
        private void InsertHtmlOnRange(Range range, string html, bool removeLastEndLineCharacterIfExists = false, WdRecoveryType wdRecoveryType = WdRecoveryType.wdFormatSurroundingFormattingWithEmphasis)
        {
            if (String.IsNullOrWhiteSpace(html))
            {
                range.Text = String.Empty;
            }
            else
            {
                string fullPath = Path.Combine(Infrastructure.ConfigurationKeys.PERCORSO_DIRECTORY_FILE_TEMPORANEI, $"{Guid.NewGuid()}.html");
                File.WriteAllText(fullPath, $"<html><body>{html}</body></html>");

                //range.InsertFile(fullPath, ConfirmConversions: false, Link: false);

                Document docuemntoHtmlAperto = application.Documents.Add(fullPath);
                docuemntoHtmlAperto.Range().Select();
                application.Selection.Copy();

                range.Select();
                application.Selection.PasteAndFormat(wdRecoveryType);
                application.Selection.Collapse();

                docuemntoHtmlAperto.Close(false);
                Helper.WordHelper.RilascioOggettiCOM(docuemntoHtmlAperto);
                docuemntoHtmlAperto = null;

                File.Delete(fullPath);

                if (removeLastEndLineCharacterIfExists && range.Paragraphs.Count > 0 && range.Paragraphs.Last.Range.Text.EndsWith("\r"))
                {
                    range.Paragraphs.Last.Range.Text = range.Paragraphs.Last.Range.Text.Substring(0, range.Paragraphs.Last.Range.Text.Length - 1);
                }

                //System.Windows.Forms.DataObject dataObject = new System.Windows.Forms.DataObject();
                //dataObject.PutInClipboard(html);
                //application.Selection.PasteAndFormat(WdRecoveryType.wdUseDestinationStylesRecovery);
            }            
        }

        /// <summary>
        /// Restituisce le entità relative ai servizi e ai contratti relativi alle offerte
        /// </summary>
        /// <returns></returns>
        private OffertaArticolo[] GetArticoliUnivociPerServiziContrattiOfferta(Entities.Offerta offerta)
        {
            List<OffertaArticolo> elencoArticoli = new List<OffertaArticolo>();

            if ((offerta?.OffertaRaggruppamentos?.Count ?? 0) > 0)
            {
                foreach (OffertaRaggruppamento offertaRaggruppamento in offerta.OffertaRaggruppamentos.OrderBy(x => x.Ordine))
                {
                    if ((offertaRaggruppamento?.OffertaArticolos?.Count ?? 0) > 0)
                    {
                        foreach (OffertaArticolo offertaArticolo in offertaRaggruppamento.OffertaArticolos.OrderBy(x => x.Ordine))
                        {
                            if (!elencoArticoli.Any(x => CompareDecimal(x.CodiceGruppo, offertaArticolo.CodiceGruppo) &&
                            CompareDecimal(x.CodiceCategoria, offertaArticolo.CodiceCategoria) &&
                            CompareDecimal(x.CodiceCategoriaStatistica, offertaArticolo.CodiceCategoriaStatistica)))
                            {
                                elencoArticoli.Add(offertaArticolo);
                            }
                        }
                    }
                }
            }

            return elencoArticoli.ToArray();
        }

        /// <summary>
        /// Restituisce l'elenco delle configurazioni relative ai contratti oppure preventivi degli articoli passati come parametro
        /// </summary>
        /// <param name="ll"></param>
        /// <param name="elencoArticoliUnivoci"></param>
        /// <param name="tipologia"></param>
        /// <returns></returns>
        private AnalisiVenditaConfigurazioneArticoloAggiuntivo[] GetConfigurazioniContrattiPreventivi(Logic.AnalisiVenditeConfigurazioneArticoliAggiuntivi ll, OffertaArticolo[] elencoArticoliUnivoci, TipologiaAnalisiVenditaConfigurazioneArticoloAggiuntivo tipologia)
        {
            if (tipologia != TipologiaAnalisiVenditaConfigurazioneArticoloAggiuntivo.Template_Articolo_Contratto &&
                tipologia != TipologiaAnalisiVenditaConfigurazioneArticoloAggiuntivo.Template_Articolo_Descrizione_Preventivo &&
                tipologia != TipologiaAnalisiVenditaConfigurazioneArticoloAggiuntivo.Template_Articolo_Descrizione_Preventivo_Contratto)
            {
                throw new Exception("Questo metodo recupera le configurazioni solo per gli articoli relativi ai contratti o preventivi");
            }

            List<AnalisiVenditaConfigurazioneArticoloAggiuntivo> elencoElementi = new List<AnalisiVenditaConfigurazioneArticoloAggiuntivo>();

            AnalisiVenditaConfigurazioneArticoloAggiuntivo[] configurazioniTuttiGliArticoli = ll.ReadEmptyConfiguration(tipologia).ToArray();
            if (configurazioniTuttiGliArticoli != null && configurazioniTuttiGliArticoli.Length > 0) elencoElementi.AddRange(configurazioniTuttiGliArticoli);

            if (elencoArticoliUnivoci != null && elencoArticoliUnivoci.Length > 0)
            {
                List<Tuple<string, decimal?, decimal?, decimal?>> elencoQueryEffettuate = new List<Tuple<string, decimal?, decimal?, decimal?>>();

                foreach(OffertaArticolo offertaArticolo in elencoArticoliUnivoci)
                {
                    if (!String.IsNullOrEmpty(offertaArticolo.CodiceArticolo) ||
                        offertaArticolo.CodiceGruppo.HasValue ||
                        offertaArticolo.CodiceCategoria.HasValue ||
                        offertaArticolo.CodiceCategoriaStatistica.HasValue)
                    {
                        if (!elencoQueryEffettuate.Any(x => x.Item1 == offertaArticolo.CodiceArticolo &&
                        x.Item2 == offertaArticolo.CodiceGruppo &&
                        x.Item3 == offertaArticolo.CodiceCategoria &&
                        x.Item4 == offertaArticolo.CodiceCategoriaStatistica))
                        {
                            IQueryable<AnalisiVenditaConfigurazioneArticoloAggiuntivo> query = ll.Read(offertaArticolo, tipologia);
                            if (tipologia == TipologiaAnalisiVenditaConfigurazioneArticoloAggiuntivo.Template_Articolo_Contratto)
                            {
                                IQueryable<AnalisiVenditaConfigurazioneArticoloAggiuntivo> queryUnion = ll.Read(offertaArticolo, TipologiaAnalisiVenditaConfigurazioneArticoloAggiuntivo.Template_Articolo_Descrizione_Preventivo_Contratto).Where(x => x.TestoDescrizioneContratto != null && x.TestoDescrizioneContratto.Trim() != String.Empty);
                                query = query.Union(queryUnion);
                            }
                            else if (tipologia == TipologiaAnalisiVenditaConfigurazioneArticoloAggiuntivo.Template_Articolo_Descrizione_Preventivo)
                            {
                                IQueryable<AnalisiVenditaConfigurazioneArticoloAggiuntivo> queryUnion = ll.Read(offertaArticolo, TipologiaAnalisiVenditaConfigurazioneArticoloAggiuntivo.Template_Articolo_Descrizione_Preventivo_Contratto).Where(x => x.TestoDescrizionePreventivo != null && x.TestoDescrizionePreventivo.Trim() != String.Empty);
                                query = query.Union(queryUnion);
                            }

                            //AnalisiVenditaConfigurazioneArticoloAggiuntivo[] configurazioniArticolo = ll.Read(offertaArticolo, tipologia).ToArray();
                            AnalisiVenditaConfigurazioneArticoloAggiuntivo[] configurazioniArticolo = query.ToArray();
                            if (configurazioniArticolo != null && configurazioniArticolo.Length > 0)
                            {
                                foreach(AnalisiVenditaConfigurazioneArticoloAggiuntivo configurazioneArticolo in configurazioniArticolo)
                                {
                                    if (!elencoElementi.Any(x => x.ID == configurazioneArticolo.ID)) elencoElementi.Add(configurazioneArticolo);
                                }
                            }

                            Tuple<string, decimal?, decimal?, decimal?> queryParameters = new Tuple<string, decimal?, decimal?, decimal?>(offertaArticolo.CodiceArticolo, offertaArticolo.CodiceGruppo, offertaArticolo.CodiceCategoria, offertaArticolo.CodiceCategoriaStatistica);
                            elencoQueryEffettuate.Add(queryParameters);
                        }
                    }
                }
            }

            return elencoElementi.ToArray();
        }

        /// <summary>
        /// Effettua la comparazione tra due decimali
        /// </summary>
        /// <param name="decimal1"></param>
        /// <param name="decimal2"></param>
        /// <returns></returns>
        private bool CompareDecimal(decimal? decimal1, decimal? decimal2)
        {
            if (decimal1.HasValue && !decimal2.HasValue)
            {
                return false;
            }
            else if (!decimal1.HasValue && decimal2.HasValue)
            {
                return false;
            }
            else if (!decimal1.HasValue && !decimal2.HasValue)
            {
                return false;
            }
            else
            {
                return decimal1.Value.Equals(decimal2.Value);
            }
        }

        /// <summary>
        /// Effettua il riempimento delle informazioni relative ai bookmark gestiti dal logic layer
        /// </summary>
        /// <param name="bookmarksInfo"></param>
        public static IDictionary<string, string> GetBookmarksInfo()
        {
            IDictionary<string, string> bookmarksInfo = new Dictionary<string, string>();

            bookmarksInfo.Add(PREFISSO_BOOKMARK_DOCUMENTO_NUMERO_OFFERTA, "Valore del campo \"Numero\"");
            bookmarksInfo.Add(PREFISSO_BOOKMARK_DOCUMENTO_NUMERO_REVISIONE_OFFERTA, "Valore del campo \"Revisione\"");
            bookmarksInfo.Add(PREFISSO_BOOKMARK_DOCUMENTO_DATA_OFFERTA, "Valore del campo \"Data Redazione\"");
            bookmarksInfo.Add(PREFISSO_BOOKMARK_DOCUMENTO_TITOLO_OFFERTA, "Valore del campo \"Titolo\"");
            bookmarksInfo.Add(PREFISSO_BOOKMARK_DOCUMENTO_CODICE_COMMESSA, "Valore del campo \"Codice Commessa\"");
            bookmarksInfo.Add(PREFISSO_BOOKMARK_DOCUMENTO_GIORNI_VALIDITA, "Valore del campo \"Giorni Validità\" e del campo \"Tipologia Giorni Validità\"");
            bookmarksInfo.Add(PREFISSO_BOOKMARK_DOCUMENTO_TEMPI_DI_CONSEGNA, "Valore del campo \"Tempi di consegna\" e del campo \"Tipologia Tempi di consegna\"");
            bookmarksInfo.Add(PREFISSO_BOOKMARK_DOCUMENTO_CODICE_CLIENTE, "Codice associato al cliente selezionato");
            bookmarksInfo.Add(PREFISSO_BOOKMARK_DOCUMENTO_RAGIONE_SOCIALE_CLIENTE, "Valore del campo \"Cliente\"");
            bookmarksInfo.Add(PREFISSO_BOOKMARK_DOCUMENTO_DESTINAZIONE_MERCE, "Valore del campo \"Destinazione Merce\"");
            bookmarksInfo.Add(PREFISSO_BOOKMARK_DOCUMENTO_INDIRIZZO_CLIENTE, "Valore del campo \"Indirizzo\"");
            bookmarksInfo.Add(PREFISSO_BOOKMARK_DOCUMENTO_CAP_CLIENTE, "Valore del campo \"CAP\"");
            bookmarksInfo.Add(PREFISSO_BOOKMARK_DOCUMENTO_LOCALITA_CLIENTE, "Valore del campo \"Localita\"");
            bookmarksInfo.Add(PREFISSO_BOOKMARK_DOCUMENTO_PROVINCIA_CLIENTE, "Valore del campo \"Prov.\"");
            bookmarksInfo.Add(PREFISSO_BOOKMARK_DOCUMENTO_TELEFONO_CLIENTE, "Valore del campo \"Telefono\"");
            bookmarksInfo.Add(PREFISSO_BOOKMARK_DOCUMENTO_NOTE_INTERNE, "Valore del campo \"Note Interne\"");
            bookmarksInfo.Add(PREFISSO_BOOKMARK_DOCUMENTO_NOTE_RIFIUTO, "Testo relativo alle note inserite per giustificare il rifiuto dell'offerta");
            bookmarksInfo.Add(PREFISSO_BOOKMARK_DOCUMENTO_TOTALE_COSTO, "Valore del campo \"Totale Costo\"");            
            bookmarksInfo.Add(PREFISSO_BOOKMARK_DOCUMENTO_TOTALE_RICARICO_VALUTA, "Valore del campo \"Valore del campo \"Totale Redditività\" sottoforma di cifra in &euro;\"");
            bookmarksInfo.Add(PREFISSO_BOOKMARK_DOCUMENTO_TOTALE_RICARICO_PERCENTUALE, "Valore del campo \"Totale Redditività\" sottoforma di cifra in percentuale");
            bookmarksInfo.Add(PREFISSO_BOOKMARK_DOCUMENTO_TOTALE_VENDITA, "Valore del campo \"Totale Vendita\"");
            bookmarksInfo.Add(PREFISSO_BOOKMARK_DOCUMENTO_TOTALE_VENDITA_CALCOLATO, "Valore del campo \"Totale Vendita Calcolato\"");
            bookmarksInfo.Add(PREFISSO_BOOKMARK_DOCUMENTO_TOTALE_COSTO_CALCOLATO, "Valore del campo \"Totale Costo Calcolato\"");
            bookmarksInfo.Add(PREFISSO_BOOKMARK_DOCUMENTO_TOTALE_RICARICO_VALUTA_CALCOLATO, "Valore del campo \"Totale Redditività Calcolato\" sottoforma di cifra in &euro;");
            bookmarksInfo.Add(PREFISSO_BOOKMARK_DOCUMENTO_TOTALE_RICARICO_PERCENTUALE_CALCOLATO, "Valore del campo \"Totale Redditività Calcolato\" sottoforma di cifra in percentuale");
            bookmarksInfo.Add(PREFISSO_BOOKMARK_DOCUMENTO_TESTO_INTESTAZIONE, "Valore del campo \"Testo Intestazioni\"");
            bookmarksInfo.Add(PREFISSO_BOOKMARK_DOCUMENTO_TESTO_PIE_DI_PAGINA, "Valore del campo \"Testo Pie di pagina\"");
            bookmarksInfo.Add(PREFISSO_BOOKMARK_DOCUMENTO_TESTO_SEZIONE_PAGAMENTI, "Valore del campo \"Testo Sezione Pagamenti\"");
            //bookmarksInfo.Add(PREFISSO_BOOKMARK_DOCUMENTO_TESTO_VALIDITA_OFFERTA, "Valore del campo \"\"");
            bookmarksInfo.Add(PREFISSO_BOOKMARK_DOCUMENTO_TESTO_EMAIL_INVIATA_AL_CLIENTE, "Testo dell'email, contenente l'offerta, inviata al cliente");
            
            bookmarksInfo.Add(PREFISSO_BOOKMARK_DOCUMENTO_CODICE_PAGAMENTO, "Codice associato alla metodologia di pagamento selezionata dall'utente");
            bookmarksInfo.Add(PREFISSO_BOOKMARK_DOCUMENTO_DESCRIZIONE_PAGAMENTO, "Valore del campo \"Metodologie di pagamento\"");
            bookmarksInfo.Add(PREFISSO_BOOKMARK_DOCUMENTO_CODICE_IBAN, "Valore del campo \"IBAN\"");
            bookmarksInfo.Add(PREFISSO_BOOKMARK_DOCUMENTO_SE_BONIFICO_CODICE_IBAN, "Valore del campo \"IBAN\" solo se la metodologia di pagamento selezionata è un bonifico");
            bookmarksInfo.Add(PREFISSO_BOOKMARK_DOCUMENTO_ARTICOLI, "Tabella contenente il riepilogo degli articoli inseriti nell'offerta");
            bookmarksInfo.Add(PREFISSO_BOOKMARK_DOCUMENTO_SERVIZI, $"Elenco delle descrizioni degli articoli associati all'offerta (configurazione articolo aggiuntivi con tipologia \"{Entities.TipologiaAnalisiVenditaConfigurazioneArticoloAggiuntivo.Template_Articolo_Descrizione_Preventivo.GetDescription()}\" oppure \"{Entities.TipologiaAnalisiVenditaConfigurazioneArticoloAggiuntivo.Template_Articolo_Descrizione_Preventivo_Contratto.GetDescription()}\")");            
            bookmarksInfo.Add(PREFISSO_BOOKMARK_DOCUMENTO_CONTRATTI_SENZA_CLAUSOLE_VESSATORIE, $"Elenco delle descrizioni dei contratti relativi agli articoli associati all'offerta (configurazione articolo aggiuntivi con tipologia \"{Entities.TipologiaAnalisiVenditaConfigurazioneArticoloAggiuntivo.Template_Articolo_Contratto.GetDescription()}\" oppure \"{Entities.TipologiaAnalisiVenditaConfigurazioneArticoloAggiuntivo.Template_Articolo_Descrizione_Preventivo_Contratto.GetDescription()}\"), senza mostrare le clausole vessatorie");
            bookmarksInfo.Add(PREFISSO_BOOKMARK_DOCUMENTO_CONTRATTI_CON_CLAUSOLE_VESSATORIE, $"Elenco delle descrizioni dei contratti relativi agli articoli associati all'offerta (configurazione articolo aggiuntivi con tipologia \"{Entities.TipologiaAnalisiVenditaConfigurazioneArticoloAggiuntivo.Template_Articolo_Contratto.GetDescription()}\" oppure \"{Entities.TipologiaAnalisiVenditaConfigurazioneArticoloAggiuntivo.Template_Articolo_Descrizione_Preventivo_Contratto.GetDescription()}\"), mostrando le clausole vessatorie");
            bookmarksInfo.Add(PREFISSO_BOOKMARK_DOCUMENTO_ELENCO_CLAUSOLE_VESSATORIE_FISSE, "Elenco concatenato delle clausole vessatorie fisse recuperate dalla configurazione degli articoli aggiuntivi in base agli articoli associati all'offerta");
            bookmarksInfo.Add(PREFISSO_BOOKMARK_DOCUMENTO_ELENCO_CLAUSOLE_VESSATORIE_VARIABILI, "Elenco concatenato delle clausole vessatorie variabili recuperate dalla configurazione degli articoli aggiuntivi in base agli articoli associati all'offerta");

            return bookmarksInfo;
        }

        /// <summary>
        /// Restituisce l'elenco dei servizi in base all'elenco delle configurazioni degli articoli aggiuntivi passate come parametro
        /// </summary>
        /// <param name="logic"></param>
        /// <param name="articoliOfferta"></param>
        /// <returns></returns>
        private Servizio[] GetElencoServiziByArticoliOfferta(Logic.Base.LogicLayerBase logic, OffertaArticolo[] articoliOfferta)
        {
            string[] elencoCodiceArticolo = articoliOfferta.Select(x => x.CodiceArticolo).Where(x => !String.IsNullOrWhiteSpace(x)).ToArray();
            if (elencoCodiceArticolo != null && elencoCodiceArticolo.Length > 0)
            {
                Logic.ServiziArticoli ll = new ServiziArticoli(logic);
                IQueryable<ServizioArticolo> query = ll.Read(elencoCodiceArticolo);

                Servizio[] servizi = query.Select(x => x.Servizio).Distinct().ToArray();
                if (servizi == null) servizi = new Servizio[] { };

                return servizi;
            }
            else
            {
                return new Servizio[] { };
            }
        }

        /// <summary>
        /// Effettua la gestione del testo da inserire nella sezione "Servizi" dell'offerta recuperandolo utilizzando l'associazione dell'articolo sull'offerta passata come parametro
        /// </summary>
        /// <param name="ll"></param>
        /// <param name="offertaArticolo"></param>
        /// <param name="elencoConfigurazioniArticoliAggiuntiviUtilizzati"></param>
        /// <param name="sb"></param>
        private void GestioneTestoDocumentoDaOffertaArticolo(Logic.AnalisiVenditeConfigurazioneArticoliAggiuntivi ll, OffertaArticolo offertaArticolo, List<AnalisiVenditaConfigurazioneArticoloAggiuntivo> elencoConfigurazioniArticoliAggiuntiviUtilizzati, StringBuilder sb)
        {
            AnalisiVenditaConfigurazioneArticoloAggiuntivo[] configurazioniServizi = GetConfigurazioniContrattiPreventivi(ll, new OffertaArticolo[] { offertaArticolo }, TipologiaAnalisiVenditaConfigurazioneArticoloAggiuntivo.Template_Articolo_Descrizione_Preventivo);
            if (configurazioniServizi != null && configurazioniServizi.Length > 0)
            {
                foreach (AnalisiVenditaConfigurazioneArticoloAggiuntivo configurazione in configurazioniServizi)
                {
                    if (!String.IsNullOrWhiteSpace(configurazione.TestoDescrizionePreventivo) && !elencoConfigurazioniArticoliAggiuntiviUtilizzati.Any(x => x.ID == configurazione.ID))
                    {
                        sb.AppendLine(configurazione.TestoDescrizionePreventivo);
                        elencoConfigurazioniArticoliAggiuntiviUtilizzati.Add(configurazione);
                    }
                }
            }
        }

        #endregion
    }
}
