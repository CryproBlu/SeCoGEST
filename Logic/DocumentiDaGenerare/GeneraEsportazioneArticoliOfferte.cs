using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;
using SeCoGes.Utilities;
using SeCoGEST.Entities;

namespace SeCoGEST.Logic.DocumentiDaGenerare
{
    public class GeneraEsportazioneArticoliOfferte : GeneratoreDocumentiExcel
    {
        #region Variabili

        private EntityId<Entities.Offerta> IdOfferta = null;
        private Entities.Offerta Offerta = null;

        #endregion

        #region Costruttori e DAL interno

        /// <summary>
        /// Data utilizzato da tutte le operazioni su database fatte tramite questa istanza
        /// </summary>
        private Offerte ll;

        /// <summary>
        /// Crea l'istanza della classe necessaria per creare l'istanza del logic layer nei metodi statici
        /// </summary>
        private GeneraEsportazioneArticoliOfferte() : this(null) { }

        /// <summary>
        /// Crea l'istanza della classe utilizzando il DataContext globale condiviso
        /// </summary>
        /// <param name="idOfferta"></param>
        public GeneraEsportazioneArticoliOfferte(EntityId<Entities.Offerta> idOfferta)
            : base(false)
        {
            this.IdOfferta = idOfferta;
            CreateLogic();
        }

        /// <summary>
        /// Crea l'istanza della classe permettendo al chiamante di specificare se utilizzare un DataContext privato o quello globale condiviso
        /// </summary>
        /// <param name="createStandaloneContext"></param>
        public GeneraEsportazioneArticoliOfferte(bool createStandaloneContext, EntityId<Entities.Offerta> idOfferta)
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
        public GeneraEsportazioneArticoliOfferte(Base.LogicLayerBase logicLayer, EntityId<Entities.Offerta> idOfferta)
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
        protected override void PopolaDocumento(Application application, Workbook workbook)
        {
            if (((workbook?.Worksheets?.Count) ?? 0) <= 0) return;

            Offerta = ll.Find(IdOfferta);
            if (Offerta == null) throw new Exception($"L'Offerta con ID \"{Offerta}\" non esiste");

            //OffertaArticolo[] elencoArticoliUnivoci = GetArticoliUnivociPerServiziContrattiOfferta(Offerta);

            Worksheet foglioPrincipale = workbook.Worksheets[1];
            foglioPrincipale.Name = "Esportazione Articoli";

            foglioPrincipale.Cells[1, 1] = "Titolo Offerta";
            foglioPrincipale.Cells[1, 2] = Offerta.Titolo;

            foglioPrincipale.Cells[2, 1] = "Numero Offerta";
            foglioPrincipale.Cells[2, 2] = Offerta.Numero;

            foglioPrincipale.Cells[3, 1] = "Data Offerta";
            foglioPrincipale.Cells[3, 2] = Offerta.Data;

            if ((Offerta?.OffertaRaggruppamentos?.Count ?? 0) > 0)
            {
                int indiceColonnaOpzione = 1;
                int indiceColonnaGruppo = 2;
                int indiceColonnaCategoria = 3;
                int indiceColonnaCategoriaStatistica = 4;
                int indiceColonnaDescrizione = 5;
                int indiceColonnaUM = 6;
                int indiceColonnaQta = 7;
                int indiceColonnaTotCosto = 8;
                int indiceColonnaTotSpesa = 9;
                int indiceColonnaTotRicavo = 10;
                int indiceColonnaPercentualeRicarico = 11;
                int indiceColonnaTotVendita = 12;

                int rigaAttuale = 3;

                rigaAttuale++;

                foglioPrincipale.Cells[rigaAttuale, indiceColonnaOpzione] = "Opzione";
                SettaBordo(foglioPrincipale, rigaAttuale, indiceColonnaOpzione);

                foglioPrincipale.Cells[rigaAttuale, indiceColonnaGruppo] = "Gruppo";
                SettaBordo(foglioPrincipale, rigaAttuale, indiceColonnaGruppo);

                foglioPrincipale.Cells[rigaAttuale, indiceColonnaCategoria] = "Categoria";
                SettaBordo(foglioPrincipale, rigaAttuale, indiceColonnaCategoria);

                foglioPrincipale.Cells[rigaAttuale, indiceColonnaCategoriaStatistica] = "Categoria Statistica";
                SettaBordo(foglioPrincipale, rigaAttuale, indiceColonnaCategoriaStatistica);

                foglioPrincipale.Cells[rigaAttuale, indiceColonnaDescrizione] = "Descrizione Art.";
                SettaBordo(foglioPrincipale, rigaAttuale, indiceColonnaDescrizione);

                foglioPrincipale.Cells[rigaAttuale, indiceColonnaUM] = "Unità di Misura";
                SettaBordo(foglioPrincipale, rigaAttuale, indiceColonnaUM);

                foglioPrincipale.Cells[rigaAttuale, indiceColonnaQta] = "Quantità";
                SettaBordo(foglioPrincipale, rigaAttuale, indiceColonnaQta);

                foglioPrincipale.Cells[rigaAttuale, indiceColonnaTotCosto] = "Tot. Costo";
                SettaBordo(foglioPrincipale, rigaAttuale, indiceColonnaTotCosto);

                foglioPrincipale.Cells[rigaAttuale, indiceColonnaTotSpesa] = "Tot. Spesa";
                SettaBordo(foglioPrincipale, rigaAttuale, indiceColonnaTotSpesa);

                foglioPrincipale.Cells[rigaAttuale, indiceColonnaTotRicavo] = "Tot. Ricavo";
                SettaBordo(foglioPrincipale, rigaAttuale, indiceColonnaTotRicavo);

                foglioPrincipale.Cells[rigaAttuale, indiceColonnaPercentualeRicarico] = "% Ricarico";
                SettaBordo(foglioPrincipale, rigaAttuale, indiceColonnaPercentualeRicarico);

                foglioPrincipale.Cells[rigaAttuale, indiceColonnaTotVendita] = "Tot. Vendita";
                SettaBordo(foglioPrincipale, rigaAttuale, indiceColonnaTotVendita);

                foreach (OffertaRaggruppamento offertaRaggruppamento in Offerta.OffertaRaggruppamentos.OrderBy(x => x.Ordine))
                {
                    if ((offertaRaggruppamento?.OffertaArticolos?.Count ?? 0) > 0)
                    {
                        foreach (OffertaArticolo offertaArticolo in offertaRaggruppamento.OffertaArticolos.OrderBy(x => x.Ordine))
                        {
                            rigaAttuale++;

                            foglioPrincipale.Cells[rigaAttuale, indiceColonnaOpzione] = offertaRaggruppamento.Denominazione;
                            SettaBordo(foglioPrincipale, rigaAttuale, indiceColonnaOpzione);

                            foglioPrincipale.Cells[rigaAttuale, indiceColonnaGruppo] = offertaArticolo.TABGRUPPI.DESCRIZIONE;
                            SettaBordo(foglioPrincipale, rigaAttuale, indiceColonnaGruppo);

                            foglioPrincipale.Cells[rigaAttuale, indiceColonnaCategoria] = offertaArticolo.TABCATEGORIE.DESCRIZIONE;
                            SettaBordo(foglioPrincipale, rigaAttuale, indiceColonnaCategoria);

                            foglioPrincipale.Cells[rigaAttuale, indiceColonnaCategoriaStatistica] = offertaArticolo.TABCATEGORIESTAT.DESCRIZIONE;
                            SettaBordo(foglioPrincipale, rigaAttuale, indiceColonnaCategoriaStatistica);

                            foglioPrincipale.Cells[rigaAttuale, indiceColonnaDescrizione] = offertaArticolo.Descrizione;
                            SettaBordo(foglioPrincipale, rigaAttuale, indiceColonnaDescrizione);

                            foglioPrincipale.Cells[rigaAttuale, indiceColonnaUM] = offertaArticolo.UnitaMisura;
                            SettaBordo(foglioPrincipale, rigaAttuale, indiceColonnaUM);

                            foglioPrincipale.Cells[rigaAttuale, indiceColonnaQta] = offertaArticolo.Quantita;
                            SettaBordo(foglioPrincipale, rigaAttuale, indiceColonnaQta);

                            foglioPrincipale.Cells[rigaAttuale, indiceColonnaTotCosto] = offertaArticolo.TotaleCosto;
                            SettaBordo(foglioPrincipale, rigaAttuale, indiceColonnaTotCosto);

                            foglioPrincipale.Cells[rigaAttuale, indiceColonnaTotSpesa] = offertaArticolo.TotaleSpesa;
                            SettaBordo(foglioPrincipale, rigaAttuale, indiceColonnaTotSpesa);

                            foglioPrincipale.Cells[rigaAttuale, indiceColonnaTotRicavo] = offertaArticolo.TotaleRicarico;
                            SettaBordo(foglioPrincipale, rigaAttuale, indiceColonnaTotRicavo);

                            foglioPrincipale.Cells[rigaAttuale, indiceColonnaPercentualeRicarico] = offertaArticolo.RicaricoPercentuale;
                            SettaBordo(foglioPrincipale, rigaAttuale, indiceColonnaPercentualeRicarico);

                            foglioPrincipale.Cells[rigaAttuale, indiceColonnaTotVendita] = offertaArticolo.TotaleVenditaConSpesa;
                            SettaBordo(foglioPrincipale, rigaAttuale, indiceColonnaTotVendita);
                        }
                    }

                    rigaAttuale++;
                    foglioPrincipale.Cells[rigaAttuale, indiceColonnaQta] = "TOT OPZIONE";
                    SettaGrassetto(foglioPrincipale, rigaAttuale, indiceColonnaQta);
                    SettaGrassetto(foglioPrincipale, rigaAttuale, indiceColonnaQta);

                    foglioPrincipale.Cells[rigaAttuale, indiceColonnaTotCosto] = offertaRaggruppamento.TotaleCosto;
                    SettaBordo(foglioPrincipale, rigaAttuale, indiceColonnaTotCosto);

                    foglioPrincipale.Cells[rigaAttuale, indiceColonnaTotSpesa] = offertaRaggruppamento.TotaleSpesa;
                    SettaBordo(foglioPrincipale, rigaAttuale, indiceColonnaTotSpesa);

                    foglioPrincipale.Cells[rigaAttuale, indiceColonnaTotRicavo] = offertaRaggruppamento.TotaleRicaricoValuta;
                    SettaBordo(foglioPrincipale, rigaAttuale, indiceColonnaTotRicavo);

                    foglioPrincipale.Cells[rigaAttuale, indiceColonnaPercentualeRicarico] = offertaRaggruppamento.TotaleRicaricoPercentuale;
                    SettaBordo(foglioPrincipale, rigaAttuale, indiceColonnaPercentualeRicarico);

                    foglioPrincipale.Cells[rigaAttuale, indiceColonnaTotVendita] = offertaRaggruppamento.TotaleVenditaConSpesa;
                    SettaBordo(foglioPrincipale, rigaAttuale, indiceColonnaTotVendita);
                }

                rigaAttuale++;
                foglioPrincipale.Cells[rigaAttuale, indiceColonnaQta] = "TOT OFFERTA";
                SettaGrassetto(foglioPrincipale, rigaAttuale, indiceColonnaQta);
                SettaBordo(foglioPrincipale, rigaAttuale, indiceColonnaQta);

                foglioPrincipale.Cells[rigaAttuale, indiceColonnaTotCosto] = Offerta.TotaleCosto;
                SettaBordo(foglioPrincipale, rigaAttuale, indiceColonnaTotCosto);

                foglioPrincipale.Cells[rigaAttuale, indiceColonnaTotSpesa] = Offerta.TotaleSpesa;
                SettaBordo(foglioPrincipale, rigaAttuale, indiceColonnaTotSpesa);

                foglioPrincipale.Cells[rigaAttuale, indiceColonnaTotRicavo] = Offerta.TotaleRicaricoValuta;
                SettaBordo(foglioPrincipale, rigaAttuale, indiceColonnaTotRicavo);

                foglioPrincipale.Cells[rigaAttuale, indiceColonnaPercentualeRicarico] = Offerta.TotaleRicaricoPercentuale;
                SettaBordo(foglioPrincipale, rigaAttuale, indiceColonnaPercentualeRicarico);

                foglioPrincipale.Cells[rigaAttuale, indiceColonnaTotVendita] = Offerta.TotaleVenditaConSpesa;
                SettaBordo(foglioPrincipale, rigaAttuale, indiceColonnaTotVendita);
            }
        }


        ///// <summary>
        ///// Restituisce le entità relative ai servizi e ai contratti relativi alle offerte
        ///// </summary>
        ///// <returns></returns>
        //private OffertaArticolo[] GetArticoliUnivociPerServiziContrattiOfferta(Entities.Offerta offerta)
        //{
        //    List<OffertaArticolo> elencoArticoli = new List<OffertaArticolo>();

        //    if ((offerta?.OffertaRaggruppamentos?.Count ?? 0) > 0)
        //    {
        //        foreach (OffertaRaggruppamento offertaRaggruppamento in offerta.OffertaRaggruppamentos)
        //        {
        //            if ((offertaRaggruppamento?.OffertaArticolos?.Count ?? 0) > 0)
        //            {
        //                foreach (OffertaArticolo offertaArticolo in offertaRaggruppamento.OffertaArticolos)
        //                {
        //                    if (!elencoArticoli.Any(x => CompareDecimal(x.CodiceGruppo, offertaArticolo.CodiceGruppo) &&
        //                    CompareDecimal(x.CodiceCategoria, offertaArticolo.CodiceCategoria) &&
        //                    CompareDecimal(x.CodiceCategoriaStatistica, offertaArticolo.CodiceCategoriaStatistica)))
        //                    {
        //                        elencoArticoli.Add(offertaArticolo);
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    return elencoArticoli.ToArray();
        //}

        #endregion
    }
}
