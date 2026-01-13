using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeCoGEST.Entities;
using SeCoGEST.Helper;
using msWord = Microsoft.Office.Interop.Word;

namespace SeCoGEST.Logic
{
    public class Offerte : Base.LogicLayerBase
    {
        #region Costruttori e DAL interno

        /// <summary>
        /// Data utilizzato da tutte le operazioni su database fatte tramite questa istanza
        /// </summary>
        private Data.Offerte dal;

        /// <summary>
        /// Crea l'istanza della classe utilizzando il DataContext globale condiviso
        /// </summary>
        public Offerte()
            : base(false)
        {
            CreateDalAndLogic();
        }

        /// <summary>
        /// Crea l'istanza della classe permettendo al chiamante di specificare se utilizzare un DataContext privato o quello globale condiviso
        /// </summary>
        /// <param name="createStandaloneContext"></param>
        public Offerte(bool createStandaloneContext)
            : base(createStandaloneContext)
        {
            CreateDalAndLogic();
        }

        /// <summary>
        /// Crea l'istanza della classe permettendo al chiamante di specificare il LogicLayer collegato.
        /// L'istanza creata utilizzerà il DataContext interno al LogicLayer passato per effettuare le operazioni sulla base dati
        /// </summary>
        /// <param name="logicLayer"></param>
        public Offerte(Base.LogicLayerBase logicLayer)
            : base(logicLayer)
        {
            CreateDalAndLogic();
        }



        /// <summary>
        /// Crea un Data che utilizza il DataContext specificato nella classe base LogicLayerBase
        /// </summary>
        private void CreateDalAndLogic()
        {
            dal = new Data.Offerte(this.context);
        }

        #endregion

        #region CRUD

        /// <summary>
        /// Aggiunge una nuova entity
        /// </summary>
        /// <param name="entityToCreate"></param>
        /// <param name="submitChanges"></param>
        public void Create(Entities.Offerta entityToCreate, bool submitChanges)
        {
            if (entityToCreate != null)
            {
                if (entityToCreate.ID.Equals(Guid.Empty))
                {
                    entityToCreate.ID = Guid.NewGuid();
                    entityToCreate.Numero = GetNuovoNumero();
                }

                // Salvataggio nel database
                dal.Create(entityToCreate, submitChanges);
            }
            else
            {
                throw new ArgumentNullException("Errore durante la creazione dell'entity 'Offerta': parametro nullo!");
            }
        }

        /// <summary>
        /// Restituisce tutte le entities
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entities.Offerta> Read()
        {
            return from u in dal.Read() orderby u.Data descending select u;
        }

        /// <summary>
        /// Restituisce l'entity relativa all'IDentificativo passato e null se non trova l'entity
        /// </summary>
        /// <param name="idToFind"></param>
        /// <returns></returns>
        public Entities.Offerta Find(EntityId<Offerta> identificativoOfferta)
        {
            return Find(identificativoOfferta.Value);
        }

        /// <summary>
        /// Restituisce l'entity relativa all'ID passato e null se non trova l'entity
        /// </summary>
        /// <param name="idToFind"></param>
        /// <returns></returns>
        private Entities.Offerta Find(Guid idToFind)
        {
            return dal.Find(idToFind);
        }

        /// <summary>
        /// Elimina l'entity passata
        /// </summary>
        /// <param name="entityToDelete"></param>
        /// <param name="submitChanges"></param>
        public void Delete(Entities.Offerta entityToDelete, bool submitChanges)
        {
            if (entityToDelete != null)
            {
                dal.Delete(entityToDelete, submitChanges);
            }
            else
            {
                throw new ArgumentNullException("Errore durante l'eliminazione dell'entity 'Offerta': parametro nullo!");
            }
        }

        /// <summary>
        /// Elimina le entity passate
        /// </summary>
        /// <param name="entitiesToDelete"></param>
        /// <param name="checkAllegati"></param>
        /// <param name="submitChanges"></param>
        public void Delete(IEnumerable<Entities.Offerta> entitiesToDelete, bool submitChanges)
        {
            if (entitiesToDelete != null)
            {
                if (entitiesToDelete.Count() > 0)
                {
                    entitiesToDelete.ToList().ForEach(x => Delete(x, submitChanges));
                }
            }
            else
            {
                throw new ArgumentNullException("Errore durante l'eliminazione delle entities 'Offerta': parametro nullo!");
            }
        }

        #endregion

        #region Custom

        #region Clonazione e Revisione

        /// <summary>
        /// Effettua la clonazione dell'offerta passata come parametro
        /// </summary>
        /// <param name="offertaDaClonare"></param>
        /// <param name="submitToDatabase"></param>
        public Entities.Offerta Clone(Entities.Offerta offertaDaClonare, bool submitToDatabase)
        {
            if (offertaDaClonare == null) throw new ArgumentNullException(nameof(offertaDaClonare), "Parametro nullo");

            return Duplicate(offertaDaClonare, false, submitToDatabase);
        }

        /// <summary>
        /// Effettua la creazione della revisione dell'offerta passata come parametro
        /// </summary>
        /// <param name="offerta"></param>
        /// <param name="submitToDatabase"></param>
        public Entities.Offerta CreateRevision(Entities.Offerta offerta, bool submitToDatabase)
        {
            if (offerta == null) throw new ArgumentNullException(nameof(offerta), "Parametro nullo");

            return Duplicate(offerta, true, submitToDatabase);
        }

        /// <summary>
        /// Effettua la fuplicazione dell'offerta passata come parametro
        /// </summary>
        /// <param name="entityToClone"></param>
        /// <param name="creaRevisione">True per creare una revisione dall'offerta passata come parametro</param>
        private Entities.Offerta Duplicate(Entities.Offerta entityToClone, bool creaRevisione, bool submitToDatabase)
        {
            if (entityToClone == null) throw new ArgumentNullException(nameof(entityToClone), "Parametro nullo");

            Entities.Offerta entity = new Offerta();

            entity.ID = Guid.NewGuid();
            if (creaRevisione)
            {
                entity.Numero = entityToClone.Numero;
                entity.NumeroRevisione = ReadByNumero(entityToClone.Numero).Max(x => x.NumeroRevisione) + 1;
                entity.Titolo = entityToClone.Titolo;
                //entity.Chiuso = false;
                //entity.Stato = (byte)StatoOffertaEnum.Aperta;
                //entity.NoteRifiuto = String.Empty;
            }
            else
            {
                entity.Numero = GetNuovoNumero();
                entity.NumeroRevisione = 0;
                entity.Titolo = $"{entityToClone.Titolo} (copia)";
                //entity.Chiuso = entityToClone.Chiuso;
                //entity.Stato = entityToClone.Stato;
                //entity.NoteRifiuto = entityToClone.NoteRifiuto;
            }

            entity.Chiuso = false;
            entity.Stato = (byte)StatoOffertaEnum.Aperta;
            entity.NoteRifiuto = String.Empty;

            entity.Data = entityToClone.Data;            
            entity.CodiceCliente = entityToClone.CodiceCliente;
            entity.RagioneSociale = entityToClone.RagioneSociale;
            entity.DestinazioneMerce = entityToClone.DestinazioneMerce;
            entity.Indirizzo = entityToClone.Indirizzo;
            entity.CAP = entityToClone.CAP;
            entity.Localita = entityToClone.Localita;
            entity.Provincia = entityToClone.Provincia;
            entity.Telefono = entityToClone.Telefono;
            entity.TotaleCosto = entityToClone.TotaleCosto;
            entity.TotaleVenditaCalcolato = entityToClone.TotaleVenditaCalcolato;
            entity.TotaleRicaricoValuta = entityToClone.TotaleRicaricoValuta;
            entity.TotaleRicaricoPercentuale = entityToClone.TotaleRicaricoPercentuale;
            entity.TotaleVendita = entityToClone.TotaleVendita;            
            entity.CodiceCommessa = entityToClone.CodiceCommessa;
            entity.TempiDiConsegna = entityToClone.TempiDiConsegna;
            entity.NoteInterne = entityToClone.NoteInterne;            
            entity.TestoPieDiPagina = entityToClone.TestoPieDiPagina;
            entity.TestoIntestazione = entityToClone.TestoIntestazione;
            entity.TestoValiditaOfferta = entityToClone.TestoValiditaOfferta;
            entity.TestoEmailInviataAlCliente = entityToClone.TestoEmailInviataAlCliente;
            entity.CodicePagamento = entityToClone.CodicePagamento;
            entity.DescrizionePagamento = entityToClone.DescrizionePagamento;
            entity.CodiceIBAN = entityToClone.CodiceIBAN;
            entity.TotaleCostoCalcolato = entityToClone.TotaleCostoCalcolato;
            entity.TotaleRicaricoValutaCalcolato = entityToClone.TotaleRicaricoValutaCalcolato;
            entity.TotaleRicaricoPercentualeCalcolato = entityToClone.TotaleRicaricoPercentualeCalcolato;
            entity.TotaliModificati = entityToClone.TotaliModificati;

            Create(entity, submitToDatabase);

            if (entityToClone.OffertaRaggruppamentos?.Count > 0)
            {
                OfferteRaggruppamenti llOfferteRaggruppamenti = new OfferteRaggruppamenti(this);

                foreach (OffertaRaggruppamento gruppo in entityToClone.OffertaRaggruppamentos.OrderBy(x => x.Ordine))
                {
                    OffertaRaggruppamento nuovoGruppo = llOfferteRaggruppamenti.Clone(gruppo, true, submitToDatabase);                    
                    nuovoGruppo.IDOfferta = entity.ID;
                }
            }
            
            //if (!creaRevisione && entity.OffertaAccountValidatores?.Count > 0)
            //{
            //    OfferteAccountValidatori llOfferteAccountValidatori = new OfferteAccountValidatori(this);

            //    foreach (OffertaAccountValidatore offertaAccountValidatore in entityToClone.OffertaAccountValidatores)
            //    {
            //        OffertaAccountValidatore nuovoValidatoreOfferta = llOfferteAccountValidatori.Clone(offertaAccountValidatore);
            //        nuovoValidatoreOfferta.IDOfferta = entity.ID;

            //        llOfferteAccountValidatori.Create(nuovoValidatoreOfferta, submitToDatabase);
            //    }
            //}

            if (submitToDatabase) SubmitToDatabase();

            return entity;
        }

        #endregion

        /// <summary>
        /// Verifica se l'offerta passata riguarda la revisione più recente (ultima)
        /// </summary>
        /// <param name="offerta"></param>
        /// <returns></returns>
        public bool IsLastRevision(Entities.Offerta offerta)
        {
            if (offerta == null) throw new ArgumentNullException(nameof(offerta), "Parametro nullo");

            return offerta.NumeroRevisione == LastRevisionNumber(offerta.Numero);
        }

        /// <summary>
        /// Restituisce il numero dell'ultima revisione
        /// </summary>
        /// <param name="numeroOfferta"></param>
        /// <returns></returns>
        public int LastRevisionNumber(int numeroOfferta)
        {
            IQueryable<Entities.Offerta> query = ReadByNumero(numeroOfferta);
            if (query.Any())
            {
                return query.Max(x => x.NumeroRevisione);
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Effettua la lettura delle entità che hanno valorizzato il campo "Numero" con il valore passato come parametro
        /// </summary>
        /// <param name="numeroOfferta"></param>
        /// <returns></returns>
        public IQueryable<Entities.Offerta> ReadByNumero(int numeroOfferta)
        {
            return Read().Where(x => x.Numero == numeroOfferta).OrderBy(x => x.NumeroRevisione);
        }

        /// <summary>
        /// Restituisce il numero da utilizzare per il prossimo nuovo bollettino
        /// </summary>
        /// <returns></returns>
        public int GetNuovoNumero()
        {
            int? max = dal.Read().Select(x => (int?)x.Numero).Max();
            if (max.HasValue)
                return max.Value + 1;
            else
                return 1;
        }

        /// <summary>
        /// Effettua il calcolo di tutti i totali dell'offerta e dei relativi Raggruppamenti articoli
        /// </summary>
        /// <param name="offerta"></param>
        public void RicalcolaTotali(Guid idOfferta)
        {
            Entities.Offerta offerta = Find(idOfferta);
            if (offerta == null) return;

            offerta.TotaleCostoCalcolato = 0;
            offerta.TotaleVenditaCalcolato = 0;
            offerta.TotaleRicaricoValutaCalcolato = 0;
            offerta.TotaleRicaricoPercentualeCalcolato = 0;
            offerta.TotaleVenditaConSpesaCacolato = 0;

            decimal? totaleCosto = 0;
            decimal? totaleSpese = 0;
            decimal? totaleVendita = 0;

            Logic.OfferteRaggruppamenti llGruppi = new OfferteRaggruppamenti(this);

            // Esegue un ciclo per tutti i Raggruppamenti e ne ricalcola i totali
            foreach (OffertaRaggruppamento gruppo in llGruppi.Read(offerta))
            {
                decimal? totaleSpesaSottoArticoli = 0;
                decimal? totaleVenditaConSpesa = 0;
                foreach (OffertaArticolo articolo in gruppo.OffertaArticolos)
                {
                    decimal? totaleSpesa = articolo.TotaleSpesaSottoArticoli;

                    if (totaleSpesa.HasValue)
                    {
                        articolo.TotaleSpesa = totaleSpesa;
                        totaleSpesaSottoArticoli += articolo.TotaleSpesa;
                    }

                    if (articolo.TotaleVendita.HasValue && totaleSpesa.HasValue)
                    {
                        articolo.TotaleVenditaConSpesa = articolo.TotaleVendita + totaleSpesa;
                        totaleVenditaConSpesa += articolo.TotaleVenditaConSpesa;
                    }
                }

                gruppo.TotaleCostoCalcolato = gruppo.OffertaArticolos.Where(x => x.TotaleCosto.HasValue).Select(x => x.TotaleCosto.Value).Sum();
                gruppo.TotaleVenditaCalcolato = gruppo.OffertaArticolos.Where(x => x.TotaleVendita.HasValue).Select(x => x.TotaleVendita.Value).Sum();

                gruppo.TotaleVenditaConSpesaCacolato = gruppo.TotaleVenditaCalcolato + totaleSpesaSottoArticoli;

                gruppo.TotaleRicaricoValutaCalcolato = gruppo.TotaleVenditaConSpesaCacolato - gruppo.TotaleCostoCalcolato;
                gruppo.TotaleRicaricoPercentualeCalcolato = Math.Round(Helper.GenericHelper.GetPercentualeVariazione(gruppo.TotaleCostoCalcolato, gruppo.TotaleVenditaConSpesaCacolato), 2);

                if (!gruppo.TotaleCostoCalcolato.HasValue) gruppo.TotaleCostoCalcolato = 0;
                if (!gruppo.TotaleVenditaCalcolato.HasValue) gruppo.TotaleVenditaCalcolato = 0;                

                offerta.TotaleCostoCalcolato += gruppo.TotaleCostoCalcolato;
                offerta.TotaleVenditaCalcolato += gruppo.TotaleVenditaCalcolato;

                if (!gruppo.TotaliModificati)
                {
                    gruppo.TotaleCosto = gruppo.TotaleCostoCalcolato;
                    gruppo.TotaleRicaricoValuta = gruppo.TotaleRicaricoValutaCalcolato;
                    gruppo.TotaleRicaricoPercentuale = gruppo.TotaleRicaricoPercentualeCalcolato;
                    gruppo.TotaleVendita = gruppo.TotaleVenditaCalcolato;                    
                }

                gruppo.TotaleSpesa = totaleSpesaSottoArticoli;
                gruppo.TotaleSpesaCacolato = totaleSpesaSottoArticoli;

                gruppo.TotaleVenditaConSpesa = gruppo.TotaleVendita + totaleSpesaSottoArticoli;
                offerta.TotaleVenditaConSpesaCacolato += gruppo.TotaleVenditaConSpesaCacolato;

                totaleCosto += gruppo.TotaleCosto;
                totaleVendita += gruppo.TotaleVendita;
                totaleSpese += totaleSpesaSottoArticoli;
            }

            //offerta.TotaleVenditaCalcolato = offerta.TotaleVendita;
            offerta.TotaleRicaricoValutaCalcolato = offerta.TotaleVenditaConSpesaCacolato - offerta.TotaleCostoCalcolato;
            offerta.TotaleRicaricoPercentualeCalcolato = Math.Round(Helper.GenericHelper.GetPercentualeVariazione(offerta.TotaleCostoCalcolato, offerta.TotaleVenditaConSpesaCacolato), 2);

            if (!offerta.TotaliModificati)
            {
                offerta.TotaleCosto = totaleCosto;
                offerta.TotaleVendita = totaleVendita;

                offerta.TotaleRicaricoValuta = offerta.TotaleVenditaConSpesa - offerta.TotaleCosto;
                offerta.TotaleRicaricoPercentuale = Math.Round(Helper.GenericHelper.GetPercentualeVariazione(offerta.TotaleCosto, offerta.TotaleVenditaConSpesa), 2);                
            }

            offerta.TotaleSpesa = totaleSpese;
            offerta.TotaleSpesaCacolato = totaleSpese;
            offerta.TotaleVenditaConSpesa = offerta.TotaleVendita + offerta.TotaleSpesa;

            if (offerta.TotaleCosto == 0 && offerta.TotaleVendita == 0) offerta.TotaliModificati = false;

            SubmitToDatabase();
            //this.context.SubmitChanges();
        }

        #endregion
    }
}
