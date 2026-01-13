using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using SeCoGEST.Entities;

// base(System.Configuration.ConfigurationManager.ConnectionStrings["DatabaseConnectionString"].ConnectionString, mappingSource)

namespace SeCoGEST.Data.Base
{
    [global::System.Data.Linq.Mapping.DatabaseAttribute(Name = "GestGRPSCG")]
    public partial class DatabaseDataContext : System.Data.Linq.DataContext
    {

        private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();

        #region Definizioni metodo Extensibility
        partial void OnCreated();
        partial void InsertAllegato(Allegato instance);
        partial void UpdateAllegato(Allegato instance);
        partial void DeleteAllegato(Allegato instance);
        partial void InsertOperatore(Operatore instance);
        partial void UpdateOperatore(Operatore instance);
        partial void DeleteOperatore(Operatore instance);
        partial void InsertIntervento(Intervento instance);
        partial void UpdateIntervento(Intervento instance);
        partial void DeleteIntervento(Intervento instance);
        partial void InsertIntervento_Articolo(Intervento_Articolo instance);
        partial void UpdateIntervento_Articolo(Intervento_Articolo instance);
        partial void DeleteIntervento_Articolo(Intervento_Articolo instance);
        partial void InsertAccount(Account instance);
        partial void UpdateAccount(Account instance);
        partial void DeleteAccount(Account instance);
        partial void InsertIntervento_Operatore(Intervento_Operatore instance);
        partial void UpdateIntervento_Operatore(Intervento_Operatore instance);
        partial void DeleteIntervento_Operatore(Intervento_Operatore instance);
        partial void InsertModalitaRisoluzioneIntervento(ModalitaRisoluzioneIntervento instance);
        partial void UpdateModalitaRisoluzioneIntervento(ModalitaRisoluzioneIntervento instance);
        partial void DeleteModalitaRisoluzioneIntervento(ModalitaRisoluzioneIntervento instance);
        partial void InsertElencoCompletoArticoli(ElencoCompletoArticoli instance);
        partial void UpdateElencoCompletoArticoli(ElencoCompletoArticoli instance);
        partial void DeleteElencoCompletoArticoli(ElencoCompletoArticoli instance);
        partial void InsertIntervento_Stato(Intervento_Stato instance);
        partial void UpdateIntervento_Stato(Intervento_Stato instance);
        partial void DeleteIntervento_Stato(Intervento_Stato instance);
        partial void InsertInformazioniContratto(InformazioniContratto instance);
        partial void UpdateInformazioniContratto(InformazioniContratto instance);
        partial void DeleteInformazioniContratto(InformazioniContratto instance);
        partial void InsertVocePredefinitaIntervento(VocePredefinitaIntervento instance);
        partial void UpdateVocePredefinitaIntervento(VocePredefinitaIntervento instance);
        partial void DeleteVocePredefinitaIntervento(VocePredefinitaIntervento instance);
        partial void InsertDESTINAZIONIDIVERSE(DESTINAZIONIDIVERSE instance);
        partial void UpdateDESTINAZIONIDIVERSE(DESTINAZIONIDIVERSE instance);
        partial void DeleteDESTINAZIONIDIVERSE(DESTINAZIONIDIVERSE instance);
        partial void InsertARTICOLIUNITAMISURA(ARTICOLIUNITAMISURA instance);
        partial void UpdateARTICOLIUNITAMISURA(ARTICOLIUNITAMISURA instance);
        partial void DeleteARTICOLIUNITAMISURA(ARTICOLIUNITAMISURA instance);
        partial void InsertAccountOperatore(AccountOperatore instance);
        partial void UpdateAccountOperatore(AccountOperatore instance);
        partial void DeleteAccountOperatore(AccountOperatore instance);
        partial void InsertLogInvioNotifica(LogInvioNotifica instance);
        partial void UpdateLogInvioNotifica(LogInvioNotifica instance);
        partial void DeleteLogInvioNotifica(LogInvioNotifica instance);
        partial void InsertNotifica(Notifica instance);
        partial void UpdateNotifica(Notifica instance);
        partial void DeleteNotifica(Notifica instance);
        partial void InsertInfoOperazioneRecord(InfoOperazioneRecord instance);
        partial void UpdateInfoOperazioneRecord(InfoOperazioneRecord instance);
        partial void DeleteInfoOperazioneRecord(InfoOperazioneRecord instance);
        partial void InsertInfoOperazioneRecord_Tabella(InfoOperazioneRecord_Tabella instance);
        partial void UpdateInfoOperazioneRecord_Tabella(InfoOperazioneRecord_Tabella instance);
        partial void DeleteInfoOperazioneRecord_Tabella(InfoOperazioneRecord_Tabella instance);
        partial void InsertAnalisiVenditaRata(AnalisiVenditaRata instance);
        partial void UpdateAnalisiVenditaRata(AnalisiVenditaRata instance);
        partial void DeleteAnalisiVenditaRata(AnalisiVenditaRata instance);
        partial void InsertAnalisiCostoRaggruppamento(AnalisiCostoRaggruppamento instance);
        partial void UpdateAnalisiCostoRaggruppamento(AnalisiCostoRaggruppamento instance);
        partial void DeleteAnalisiCostoRaggruppamento(AnalisiCostoRaggruppamento instance);
        partial void InsertAnalisiCostoArticolo(AnalisiCostoArticolo instance);
        partial void UpdateAnalisiCostoArticolo(AnalisiCostoArticolo instance);
        partial void DeleteAnalisiCostoArticolo(AnalisiCostoArticolo instance);
        partial void InsertAnalisiCostoArchivioCampoAggiuntivo(AnalisiCostoArchivioCampoAggiuntivo instance);
        partial void UpdateAnalisiCostoArchivioCampoAggiuntivo(AnalisiCostoArchivioCampoAggiuntivo instance);
        partial void DeleteAnalisiCostoArchivioCampoAggiuntivo(AnalisiCostoArchivioCampoAggiuntivo instance);
        partial void InsertAnalisiCostoArticoloCampoAggiuntivo(AnalisiCostoArticoloCampoAggiuntivo instance);
        partial void UpdateAnalisiCostoArticoloCampoAggiuntivo(AnalisiCostoArticoloCampoAggiuntivo instance);
        partial void DeleteAnalisiCostoArticoloCampoAggiuntivo(AnalisiCostoArticoloCampoAggiuntivo instance);
        partial void InsertAnalisiCosto(AnalisiCosto instance);
        partial void UpdateAnalisiCosto(AnalisiCosto instance);
        partial void DeleteAnalisiCosto(AnalisiCosto instance);
        partial void InsertAnalisiVendita(AnalisiVendita instance);
        partial void UpdateAnalisiVendita(AnalisiVendita instance);
        partial void DeleteAnalisiVendita(AnalisiVendita instance);
        partial void InsertTABGRUPPI(TABGRUPPI instance);
        partial void UpdateTABGRUPPI(TABGRUPPI instance);
        partial void DeleteTABGRUPPI(TABGRUPPI instance);
        partial void InsertTABCATEGORIE(TABCATEGORIE instance);
        partial void UpdateTABCATEGORIE(TABCATEGORIE instance);
        partial void DeleteTABCATEGORIE(TABCATEGORIE instance);
        partial void InsertTABCATEGORIESTAT(TABCATEGORIESTAT instance);
        partial void UpdateTABCATEGORIESTAT(TABCATEGORIESTAT instance);
        partial void DeleteTABCATEGORIESTAT(TABCATEGORIESTAT instance);
        partial void InsertOffertaRaggruppamento(OffertaRaggruppamento instance);
        partial void UpdateOffertaRaggruppamento(OffertaRaggruppamento instance);
        partial void DeleteOffertaRaggruppamento(OffertaRaggruppamento instance);
        partial void InsertOffertaArticolo(OffertaArticolo instance);
        partial void UpdateOffertaArticolo(OffertaArticolo instance);
        partial void DeleteOffertaArticolo(OffertaArticolo instance);
        partial void InsertOffertaArchivioCampoAggiuntivo(OffertaArchivioCampoAggiuntivo instance);
        partial void UpdateOffertaArchivioCampoAggiuntivo(OffertaArchivioCampoAggiuntivo instance);
        partial void DeleteOffertaArchivioCampoAggiuntivo(OffertaArchivioCampoAggiuntivo instance);
        partial void InsertOffertaArticoloCampoAggiuntivo(OffertaArticoloCampoAggiuntivo instance);
        partial void UpdateOffertaArticoloCampoAggiuntivo(OffertaArticoloCampoAggiuntivo instance);
        partial void DeleteOffertaArticoloCampoAggiuntivo(OffertaArticoloCampoAggiuntivo instance);
        partial void InsertCaratteristicaIntervento(CaratteristicaIntervento instance);
        partial void UpdateCaratteristicaIntervento(CaratteristicaIntervento instance);
        partial void DeleteCaratteristicaIntervento(CaratteristicaIntervento instance);
        partial void InsertCaratteristicaTipologiaIntervento(CaratteristicaTipologiaIntervento instance);
        partial void UpdateCaratteristicaTipologiaIntervento(CaratteristicaTipologiaIntervento instance);
        partial void DeleteCaratteristicaTipologiaIntervento(CaratteristicaTipologiaIntervento instance);
        partial void InsertAnagraficaSoggetti_Proprieta(AnagraficaSoggetti_Proprieta instance);
        partial void UpdateAnagraficaSoggetti_Proprieta(AnagraficaSoggetti_Proprieta instance);
        partial void DeleteAnagraficaSoggetti_Proprieta(AnagraficaSoggetti_Proprieta instance);
        partial void InsertAnagraficaClienti(AnagraficaClienti instance);
        partial void UpdateAnagraficaClienti(AnagraficaClienti instance);
        partial void DeleteAnagraficaClienti(AnagraficaClienti instance);
        partial void InsertIntervento_Discussione(Intervento_Discussione instance);
        partial void UpdateIntervento_Discussione(Intervento_Discussione instance);
        partial void DeleteIntervento_Discussione(Intervento_Discussione instance);
        partial void InsertCondizioneIntervento(CondizioneIntervento instance);
        partial void UpdateCondizioneIntervento(CondizioneIntervento instance);
        partial void DeleteCondizioneIntervento(CondizioneIntervento instance);
        partial void InsertConfigurazioneTipologiaTicketCliente(ConfigurazioneTipologiaTicketCliente instance);
        partial void UpdateConfigurazioneTipologiaTicketCliente(ConfigurazioneTipologiaTicketCliente instance);
        partial void DeleteConfigurazioneTipologiaTicketCliente(ConfigurazioneTipologiaTicketCliente instance);
        partial void InsertIntervento_CaratteristicaTipologiaIntervento(Intervento_CaratteristicaTipologiaIntervento instance);
        partial void UpdateIntervento_CaratteristicaTipologiaIntervento(Intervento_CaratteristicaTipologiaIntervento instance);
        partial void DeleteIntervento_CaratteristicaTipologiaIntervento(Intervento_CaratteristicaTipologiaIntervento instance);
        partial void InsertIntervento_ConfigurazioneTipologiaTicketCliente(Intervento_ConfigurazioneTipologiaTicketCliente instance);
        partial void UpdateIntervento_ConfigurazioneTipologiaTicketCliente(Intervento_ConfigurazioneTipologiaTicketCliente instance);
        partial void DeleteIntervento_ConfigurazioneTipologiaTicketCliente(Intervento_ConfigurazioneTipologiaTicketCliente instance);
        partial void InsertIntervento_OrarioRepartoUfficio(Intervento_OrarioRepartoUfficio instance);
        partial void UpdateIntervento_OrarioRepartoUfficio(Intervento_OrarioRepartoUfficio instance);
        partial void DeleteIntervento_OrarioRepartoUfficio(Intervento_OrarioRepartoUfficio instance);
        partial void InsertModelloConfigurazioneCaratteristicheTicketCliente(ModelloConfigurazioneCaratteristicheTicketCliente instance);
        partial void UpdateModelloConfigurazioneCaratteristicheTicketCliente(ModelloConfigurazioneCaratteristicheTicketCliente instance);
        partial void DeleteModelloConfigurazioneCaratteristicheTicketCliente(ModelloConfigurazioneCaratteristicheTicketCliente instance);
        partial void InsertModelloConfigurazioneTicketCliente(ModelloConfigurazioneTicketCliente instance);
        partial void UpdateModelloConfigurazioneTicketCliente(ModelloConfigurazioneTicketCliente instance);
        partial void DeleteModelloConfigurazioneTicketCliente(ModelloConfigurazioneTicketCliente instance);
        partial void InsertOrarioRepartoUfficio(OrarioRepartoUfficio instance);
        partial void UpdateOrarioRepartoUfficio(OrarioRepartoUfficio instance);
        partial void DeleteOrarioRepartoUfficio(OrarioRepartoUfficio instance);
        partial void InsertRepartoUfficio(RepartoUfficio instance);
        partial void UpdateRepartoUfficio(RepartoUfficio instance);
        partial void DeleteRepartoUfficio(RepartoUfficio instance);
        partial void InsertTipologiaIntervento(TipologiaIntervento instance);
        partial void UpdateTipologiaIntervento(TipologiaIntervento instance);
        partial void DeleteTipologiaIntervento(TipologiaIntervento instance);
        partial void InsertAnalisiVenditaConfigurazioneArticoloAggiuntivo(AnalisiVenditaConfigurazioneArticoloAggiuntivo instance);
        partial void UpdateAnalisiVenditaConfigurazioneArticoloAggiuntivo(AnalisiVenditaConfigurazioneArticoloAggiuntivo instance);
        partial void DeleteAnalisiVenditaConfigurazioneArticoloAggiuntivo(AnalisiVenditaConfigurazioneArticoloAggiuntivo instance);
        partial void InsertPeriodoFestivita(PeriodoFestivita instance);
        partial void UpdatePeriodoFestivita(PeriodoFestivita instance);
        partial void DeletePeriodoFestivita(PeriodoFestivita instance);
        partial void InsertANAGRAFICAARTICOLI(ANAGRAFICAARTICOLI instance);
        partial void UpdateANAGRAFICAARTICOLI(ANAGRAFICAARTICOLI instance);
        partial void DeleteANAGRAFICAARTICOLI(ANAGRAFICAARTICOLI instance);
        partial void InsertOfferta(Offerta instance);
        partial void UpdateOfferta(Offerta instance);
        partial void DeleteOfferta(Offerta instance);
        partial void InsertOffertaAccountValidatore(OffertaAccountValidatore instance);
        partial void UpdateOffertaAccountValidatore(OffertaAccountValidatore instance);
        partial void DeleteOffertaAccountValidatore(OffertaAccountValidatore instance);
        partial void InsertTABPAGAMENTI(TABPAGAMENTI instance);
        partial void UpdateTABPAGAMENTI(TABPAGAMENTI instance);
        partial void DeleteTABPAGAMENTI(TABPAGAMENTI instance);
        partial void InsertBANCAAPPCF(BANCAAPPCF instance);
        partial void UpdateBANCAAPPCF(BANCAAPPCF instance);
        partial void DeleteBANCAAPPCF(BANCAAPPCF instance);
        partial void InsertMappaturaGruppoCategoriaCategoriaStatistica(MappaturaGruppoCategoriaCategoriaStatistica instance);
        partial void UpdateMappaturaGruppoCategoriaCategoriaStatistica(MappaturaGruppoCategoriaCategoriaStatistica instance);
        partial void DeleteMappaturaGruppoCategoriaCategoriaStatistica(MappaturaGruppoCategoriaCategoriaStatistica instance);
        partial void InsertAnalisiVenditaConfigurazioneArticoloAggiuntivoClausolaVessatoria(AnalisiVenditaConfigurazioneArticoloAggiuntivoClausolaVessatoria instance);
        partial void UpdateAnalisiVenditaConfigurazioneArticoloAggiuntivoClausolaVessatoria(AnalisiVenditaConfigurazioneArticoloAggiuntivoClausolaVessatoria instance);
        partial void DeleteAnalisiVenditaConfigurazioneArticoloAggiuntivoClausolaVessatoria(AnalisiVenditaConfigurazioneArticoloAggiuntivoClausolaVessatoria instance);
        partial void InsertClausolaVessatoria(ClausolaVessatoria instance);
        partial void UpdateClausolaVessatoria(ClausolaVessatoria instance);
        partial void DeleteClausolaVessatoria(ClausolaVessatoria instance);
        partial void InsertServizio(Servizio instance);
        partial void UpdateServizio(Servizio instance);
        partial void DeleteServizio(Servizio instance);
        partial void InsertServizioArticolo(ServizioArticolo instance);
        partial void UpdateServizioArticolo(ServizioArticolo instance);
        partial void DeleteServizioArticolo(ServizioArticolo instance);
        partial void InsertProgetto(Progetto instance);
        partial void UpdateProgetto(Progetto instance);
        partial void DeleteProgetto(Progetto instance);
        partial void InsertProgetto_Attivita(Progetto_Attivita instance);
        partial void UpdateProgetto_Attivita(Progetto_Attivita instance);
        partial void DeleteProgetto_Attivita(Progetto_Attivita instance);
        partial void InsertProgetto_Operatore(Progetto_Operatore instance);
        partial void UpdateProgetto_Operatore(Progetto_Operatore instance);
        partial void DeleteProgetto_Operatore(Progetto_Operatore instance);
        partial void InsertAttivita(Attivita instance);
        partial void UpdateAttivita(Attivita instance);
        partial void DeleteAttivita(Attivita instance);
        #endregion

        public DatabaseDataContext() :
                base(System.Configuration.ConfigurationManager.ConnectionStrings["DatabaseConnectionString"].ConnectionString, mappingSource)
        {
            OnCreated();
        }

        public DatabaseDataContext(string connection) :
                base(connection, mappingSource)
        {
            OnCreated();
        }

        public DatabaseDataContext(System.Data.IDbConnection connection) :
                base(connection, mappingSource)
        {
            OnCreated();
        }

        public DatabaseDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) :
                base(connection, mappingSource)
        {
            OnCreated();
        }

        public DatabaseDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) :
                base(connection, mappingSource)
        {
            OnCreated();
        }

        public System.Data.Linq.Table<Allegato> Allegatoes
        {
            get
            {
                return this.GetTable<Allegato>();
            }
        }

        public System.Data.Linq.Table<Operatore> Operatores
        {
            get
            {
                return this.GetTable<Operatore>();
            }
        }

        public System.Data.Linq.Table<Intervento> Interventos
        {
            get
            {
                return this.GetTable<Intervento>();
            }
        }

        public System.Data.Linq.Table<Intervento_Articolo> Intervento_Articolos
        {
            get
            {
                return this.GetTable<Intervento_Articolo>();
            }
        }

        public System.Data.Linq.Table<Account> Accounts
        {
            get
            {
                return this.GetTable<Account>();
            }
        }

        public System.Data.Linq.Table<Intervento_Operatore> Intervento_Operatores
        {
            get
            {
                return this.GetTable<Intervento_Operatore>();
            }
        }

        public System.Data.Linq.Table<ModalitaRisoluzioneIntervento> ModalitaRisoluzioneInterventos
        {
            get
            {
                return this.GetTable<ModalitaRisoluzioneIntervento>();
            }
        }

        public System.Data.Linq.Table<ElencoCompletoArticoli> ElencoCompletoArticolis
        {
            get
            {
                return this.GetTable<ElencoCompletoArticoli>();
            }
        }

        public System.Data.Linq.Table<Intervento_Stato> Intervento_Statos
        {
            get
            {
                return this.GetTable<Intervento_Stato>();
            }
        }

        public System.Data.Linq.Table<InformazioniContratto> InformazioniContrattos
        {
            get
            {
                return this.GetTable<InformazioniContratto>();
            }
        }

        public System.Data.Linq.Table<VocePredefinitaIntervento> VocePredefinitaInterventos
        {
            get
            {
                return this.GetTable<VocePredefinitaIntervento>();
            }
        }

        public System.Data.Linq.Table<DESTINAZIONIDIVERSE> DESTINAZIONIDIVERSEs
        {
            get
            {
                return this.GetTable<DESTINAZIONIDIVERSE>();
            }
        }

        public System.Data.Linq.Table<ARTICOLIUNITAMISURA> ARTICOLIUNITAMISURAs
        {
            get
            {
                return this.GetTable<ARTICOLIUNITAMISURA>();
            }
        }

        public System.Data.Linq.Table<AccountOperatore> AccountOperatores
        {
            get
            {
                return this.GetTable<AccountOperatore>();
            }
        }

        public System.Data.Linq.Table<LogInvioNotifica> LogInvioNotificas
        {
            get
            {
                return this.GetTable<LogInvioNotifica>();
            }
        }

        public System.Data.Linq.Table<Notifica> Notificas
        {
            get
            {
                return this.GetTable<Notifica>();
            }
        }

        public System.Data.Linq.Table<InfoOperazioneRecord> InfoOperazioneRecords
        {
            get
            {
                return this.GetTable<InfoOperazioneRecord>();
            }
        }

        public System.Data.Linq.Table<InfoOperazioneRecord_Tabella> InfoOperazioneRecord_Tabellas
        {
            get
            {
                return this.GetTable<InfoOperazioneRecord_Tabella>();
            }
        }

        public System.Data.Linq.Table<AnalisiVenditaRata> AnalisiVenditaRatas
        {
            get
            {
                return this.GetTable<AnalisiVenditaRata>();
            }
        }

        public System.Data.Linq.Table<AnalisiCostoRaggruppamento> AnalisiCostoRaggruppamentos
        {
            get
            {
                return this.GetTable<AnalisiCostoRaggruppamento>();
            }
        }

        public System.Data.Linq.Table<AnalisiCostoArticolo> AnalisiCostoArticolos
        {
            get
            {
                return this.GetTable<AnalisiCostoArticolo>();
            }
        }

        public System.Data.Linq.Table<AnalisiCostoArchivioCampoAggiuntivo> AnalisiCostoArchivioCampoAggiuntivos
        {
            get
            {
                return this.GetTable<AnalisiCostoArchivioCampoAggiuntivo>();
            }
        }

        public System.Data.Linq.Table<AnalisiCostoArticoloCampoAggiuntivo> AnalisiCostoArticoloCampoAggiuntivos
        {
            get
            {
                return this.GetTable<AnalisiCostoArticoloCampoAggiuntivo>();
            }
        }

        public System.Data.Linq.Table<AnalisiCosto> AnalisiCostos
        {
            get
            {
                return this.GetTable<AnalisiCosto>();
            }
        }

        public System.Data.Linq.Table<AnalisiVendita> AnalisiVenditas
        {
            get
            {
                return this.GetTable<AnalisiVendita>();
            }
        }

        public System.Data.Linq.Table<TABGRUPPI> TABGRUPPIs
        {
            get
            {
                return this.GetTable<TABGRUPPI>();
            }
        }

        public System.Data.Linq.Table<TABCATEGORIE> TABCATEGORIEs
        {
            get
            {
                return this.GetTable<TABCATEGORIE>();
            }
        }

        public System.Data.Linq.Table<TABCATEGORIESTAT> TABCATEGORIESTATs
        {
            get
            {
                return this.GetTable<TABCATEGORIESTAT>();
            }
        }

        public System.Data.Linq.Table<OffertaRaggruppamento> OffertaRaggruppamentos
        {
            get
            {
                return this.GetTable<OffertaRaggruppamento>();
            }
        }

        public System.Data.Linq.Table<OffertaArticolo> OffertaArticolos
        {
            get
            {
                return this.GetTable<OffertaArticolo>();
            }
        }

        public System.Data.Linq.Table<OffertaArchivioCampoAggiuntivo> OffertaArchivioCampoAggiuntivos
        {
            get
            {
                return this.GetTable<OffertaArchivioCampoAggiuntivo>();
            }
        }

        public System.Data.Linq.Table<OffertaArticoloCampoAggiuntivo> OffertaArticoloCampoAggiuntivos
        {
            get
            {
                return this.GetTable<OffertaArticoloCampoAggiuntivo>();
            }
        }

        public System.Data.Linq.Table<CaratteristicaIntervento> CaratteristicaInterventos
        {
            get
            {
                return this.GetTable<CaratteristicaIntervento>();
            }
        }

        public System.Data.Linq.Table<CaratteristicaTipologiaIntervento> CaratteristicaTipologiaInterventos
        {
            get
            {
                return this.GetTable<CaratteristicaTipologiaIntervento>();
            }
        }

        public System.Data.Linq.Table<AnagraficaSoggetti_Proprieta> AnagraficaSoggetti_Proprietas
        {
            get
            {
                return this.GetTable<AnagraficaSoggetti_Proprieta>();
            }
        }

        public System.Data.Linq.Table<AnagraficaClienti> AnagraficaClientis
        {
            get
            {
                return this.GetTable<AnagraficaClienti>();
            }
        }

        public System.Data.Linq.Table<Intervento_Discussione> Intervento_Discussiones
        {
            get
            {
                return this.GetTable<Intervento_Discussione>();
            }
        }

        public System.Data.Linq.Table<CondizioneIntervento> CondizioneInterventos
        {
            get
            {
                return this.GetTable<CondizioneIntervento>();
            }
        }

        public System.Data.Linq.Table<ConfigurazioneTipologiaTicketCliente> ConfigurazioneTipologiaTicketClientes
        {
            get
            {
                return this.GetTable<ConfigurazioneTipologiaTicketCliente>();
            }
        }

        public System.Data.Linq.Table<Intervento_CaratteristicaTipologiaIntervento> Intervento_CaratteristicaTipologiaInterventos
        {
            get
            {
                return this.GetTable<Intervento_CaratteristicaTipologiaIntervento>();
            }
        }

        public System.Data.Linq.Table<Intervento_ConfigurazioneTipologiaTicketCliente> Intervento_ConfigurazioneTipologiaTicketClientes
        {
            get
            {
                return this.GetTable<Intervento_ConfigurazioneTipologiaTicketCliente>();
            }
        }

        public System.Data.Linq.Table<Intervento_OrarioRepartoUfficio> Intervento_OrarioRepartoUfficios
        {
            get
            {
                return this.GetTable<Intervento_OrarioRepartoUfficio>();
            }
        }

        public System.Data.Linq.Table<ModelloConfigurazioneCaratteristicheTicketCliente> ModelloConfigurazioneCaratteristicheTicketClientes
        {
            get
            {
                return this.GetTable<ModelloConfigurazioneCaratteristicheTicketCliente>();
            }
        }

        public System.Data.Linq.Table<ModelloConfigurazioneTicketCliente> ModelloConfigurazioneTicketClientes
        {
            get
            {
                return this.GetTable<ModelloConfigurazioneTicketCliente>();
            }
        }

        public System.Data.Linq.Table<OrarioRepartoUfficio> OrarioRepartoUfficios
        {
            get
            {
                return this.GetTable<OrarioRepartoUfficio>();
            }
        }

        public System.Data.Linq.Table<RepartoUfficio> RepartoUfficios
        {
            get
            {
                return this.GetTable<RepartoUfficio>();
            }
        }

        public System.Data.Linq.Table<TipologiaIntervento> TipologiaInterventos
        {
            get
            {
                return this.GetTable<TipologiaIntervento>();
            }
        }

        public System.Data.Linq.Table<AnalisiVenditaConfigurazioneArticoloAggiuntivo> AnalisiVenditaConfigurazioneArticoloAggiuntivos
        {
            get
            {
                return this.GetTable<AnalisiVenditaConfigurazioneArticoloAggiuntivo>();
            }
        }

        public System.Data.Linq.Table<PeriodoFestivita> PeriodoFestivitas
        {
            get
            {
                return this.GetTable<PeriodoFestivita>();
            }
        }

        public System.Data.Linq.Table<ANAGRAFICAARTICOLI> ANAGRAFICAARTICOLIs
        {
            get
            {
                return this.GetTable<ANAGRAFICAARTICOLI>();
            }
        }

        public System.Data.Linq.Table<Offerta> Offertas
        {
            get
            {
                return this.GetTable<Offerta>();
            }
        }

        public System.Data.Linq.Table<OffertaAccountValidatore> OffertaAccountValidatores
        {
            get
            {
                return this.GetTable<OffertaAccountValidatore>();
            }
        }

        public System.Data.Linq.Table<TABPAGAMENTI> TABPAGAMENTIs
        {
            get
            {
                return this.GetTable<TABPAGAMENTI>();
            }
        }

        public System.Data.Linq.Table<BANCAAPPCF> BANCAAPPCFs
        {
            get
            {
                return this.GetTable<BANCAAPPCF>();
            }
        }

        public System.Data.Linq.Table<MappaturaGruppoCategoriaCategoriaStatistica> MappaturaGruppoCategoriaCategoriaStatisticas
        {
            get
            {
                return this.GetTable<MappaturaGruppoCategoriaCategoriaStatistica>();
            }
        }

        public System.Data.Linq.Table<AnalisiVenditaConfigurazioneArticoloAggiuntivoClausolaVessatoria> AnalisiVenditaConfigurazioneArticoloAggiuntivoClausolaVessatorias
        {
            get
            {
                return this.GetTable<AnalisiVenditaConfigurazioneArticoloAggiuntivoClausolaVessatoria>();
            }
        }

        public System.Data.Linq.Table<ClausolaVessatoria> ClausolaVessatorias
        {
            get
            {
                return this.GetTable<ClausolaVessatoria>();
            }
        }

        public System.Data.Linq.Table<Servizio> Servizios
        {
            get
            {
                return this.GetTable<Servizio>();
            }
        }

        public System.Data.Linq.Table<ServizioArticolo> ServizioArticolos
        {
            get
            {
                return this.GetTable<ServizioArticolo>();
            }
        }

        public System.Data.Linq.Table<InterventiConAreeMultiple> InterventiConAreeMultiple
        {
            get
            {
                return this.GetTable<InterventiConAreeMultiple>();
            }
        }

        public System.Data.Linq.Table<IdInterventiConAreeMultiple> IdInterventiConAreeMultiple
        {
            get
            {
                return this.GetTable<IdInterventiConAreeMultiple>();
            }
        }

        public System.Data.Linq.Table<Progetto> Progettos
        {
            get
            {
                return this.GetTable<Progetto>();
            }
        }

        public System.Data.Linq.Table<Progetto_Attivita> Progetto_Attivitas
        {
            get
            {
                return this.GetTable<Progetto_Attivita>();
            }
        }

        public System.Data.Linq.Table<Progetto_Operatore> Progetto_Operatores
        {
            get
            {
                return this.GetTable<Progetto_Operatore>();
            }
        }

        public System.Data.Linq.Table<Attivita> Attivitas
        {
            get
            {
                return this.GetTable<Attivita>();
            }
        }

        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "SeCoGEST.CreaDocXMLInterventi")]
        public int CreaDocXMLInterventi([global::System.Data.Linq.Mapping.ParameterAttribute(DbType = "uniqueidentifier")] System.Nullable<System.Guid> IDTESTA)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), IDTESTA);
            return ((int)(result.ReturnValue));
        }

        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "SeCoGEST.GetContrattiPerCliente")]
        public ISingleResult<InformazioniContratto> GetContrattiPerCliente([global::System.Data.Linq.Mapping.ParameterAttribute(Name = "CodiceCliente", DbType = "VarChar(7)")] string codiceCliente, [global::System.Data.Linq.Mapping.ParameterAttribute(Name = "DataIntervento", DbType = "DateTime")] System.Nullable<System.DateTime> dataIntervento)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), codiceCliente, dataIntervento);
            return ((ISingleResult<InformazioniContratto>)(result.ReturnValue));
        }

        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "SeCoGEST.GetElencoCompletoArticoli", IsComposable = true)]
        public IQueryable<ElencoCompletoArticoli> GetElencoCompletoArticoli([global::System.Data.Linq.Mapping.ParameterAttribute(DbType = "DateTime")] System.Nullable<System.DateTime> dataValidita)
        {
            return this.CreateMethodCallQuery<ElencoCompletoArticoli>(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), dataValidita);
        }

        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "SeCoGEST.GetDataUltimoInvioNotifica", IsComposable = true)]
        public System.Nullable<System.DateTime> GetDataUltimoInvioNotifica([global::System.Data.Linq.Mapping.ParameterAttribute(Name = "IDLegame", DbType = "UniqueIdentifier")] System.Nullable<System.Guid> iDLegame, [global::System.Data.Linq.Mapping.ParameterAttribute(Name = "IDTabellaLegame", DbType = "TinyInt")] System.Nullable<byte> iDTabellaLegame, [global::System.Data.Linq.Mapping.ParameterAttribute(Name = "IDNotifica", DbType = "TinyInt")] System.Nullable<byte> iDNotifica)
        {
            return ((System.Nullable<System.DateTime>)(this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), iDLegame, iDTabellaLegame, iDNotifica).ReturnValue));
        }
    }








    //[global::System.Data.Linq.Mapping.DatabaseAttribute(Name = "test_metodo")]
    //public partial class DatabaseDataContext : System.Data.Linq.DataContext
    //{

    //    private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();

    //    #region Definizioni metodo Extensibility
    //    partial void OnCreated();
    //    partial void InsertAllegato(Allegato instance);
    //    partial void UpdateAllegato(Allegato instance);
    //    partial void DeleteAllegato(Allegato instance);
    //    partial void InsertOperatore(Operatore instance);
    //    partial void UpdateOperatore(Operatore instance);
    //    partial void DeleteOperatore(Operatore instance);
    //    partial void InsertIntervento(Intervento instance);
    //    partial void UpdateIntervento(Intervento instance);
    //    partial void DeleteIntervento(Intervento instance);
    //    partial void InsertIntervento_Articolo(Intervento_Articolo instance);
    //    partial void UpdateIntervento_Articolo(Intervento_Articolo instance);
    //    partial void DeleteIntervento_Articolo(Intervento_Articolo instance);
    //    partial void InsertAccount(Account instance);
    //    partial void UpdateAccount(Account instance);
    //    partial void DeleteAccount(Account instance);
    //    partial void InsertIntervento_Operatore(Intervento_Operatore instance);
    //    partial void UpdateIntervento_Operatore(Intervento_Operatore instance);
    //    partial void DeleteIntervento_Operatore(Intervento_Operatore instance);
    //    partial void InsertModalitaRisoluzioneIntervento(ModalitaRisoluzioneIntervento instance);
    //    partial void UpdateModalitaRisoluzioneIntervento(ModalitaRisoluzioneIntervento instance);
    //    partial void DeleteModalitaRisoluzioneIntervento(ModalitaRisoluzioneIntervento instance);
    //    partial void InsertElencoCompletoArticoli(ElencoCompletoArticoli instance);
    //    partial void UpdateElencoCompletoArticoli(ElencoCompletoArticoli instance);
    //    partial void DeleteElencoCompletoArticoli(ElencoCompletoArticoli instance);
    //    partial void InsertIntervento_Stato(Intervento_Stato instance);
    //    partial void UpdateIntervento_Stato(Intervento_Stato instance);
    //    partial void DeleteIntervento_Stato(Intervento_Stato instance);
    //    partial void InsertInformazioniContratto(InformazioniContratto instance);
    //    partial void UpdateInformazioniContratto(InformazioniContratto instance);
    //    partial void DeleteInformazioniContratto(InformazioniContratto instance);
    //    partial void InsertVocePredefinitaIntervento(VocePredefinitaIntervento instance);
    //    partial void UpdateVocePredefinitaIntervento(VocePredefinitaIntervento instance);
    //    partial void DeleteVocePredefinitaIntervento(VocePredefinitaIntervento instance);
    //    partial void InsertDESTINAZIONIDIVERSE(DESTINAZIONIDIVERSE instance);
    //    partial void UpdateDESTINAZIONIDIVERSE(DESTINAZIONIDIVERSE instance);
    //    partial void DeleteDESTINAZIONIDIVERSE(DESTINAZIONIDIVERSE instance);
    //    partial void InsertARTICOLIUNITAMISURA(ARTICOLIUNITAMISURA instance);
    //    partial void UpdateARTICOLIUNITAMISURA(ARTICOLIUNITAMISURA instance);
    //    partial void DeleteARTICOLIUNITAMISURA(ARTICOLIUNITAMISURA instance);
    //    partial void InsertAccountOperatore(AccountOperatore instance);
    //    partial void UpdateAccountOperatore(AccountOperatore instance);
    //    partial void DeleteAccountOperatore(AccountOperatore instance);
    //    partial void InsertLogInvioNotifica(LogInvioNotifica instance);
    //    partial void UpdateLogInvioNotifica(LogInvioNotifica instance);
    //    partial void DeleteLogInvioNotifica(LogInvioNotifica instance);
    //    partial void InsertNotifica(Notifica instance);
    //    partial void UpdateNotifica(Notifica instance);
    //    partial void DeleteNotifica(Notifica instance);
    //    partial void InsertInfoOperazioneRecord(InfoOperazioneRecord instance);
    //    partial void UpdateInfoOperazioneRecord(InfoOperazioneRecord instance);
    //    partial void DeleteInfoOperazioneRecord(InfoOperazioneRecord instance);
    //    partial void InsertInfoOperazioneRecord_Tabella(InfoOperazioneRecord_Tabella instance);
    //    partial void UpdateInfoOperazioneRecord_Tabella(InfoOperazioneRecord_Tabella instance);
    //    partial void DeleteInfoOperazioneRecord_Tabella(InfoOperazioneRecord_Tabella instance);
    //    partial void InsertTESTEDOCUMENTI(TESTEDOCUMENTI instance);
    //    partial void UpdateTESTEDOCUMENTI(TESTEDOCUMENTI instance);
    //    partial void DeleteTESTEDOCUMENTI(TESTEDOCUMENTI instance);
    //    partial void InsertRIGHEDOCUMENTI(RIGHEDOCUMENTI instance);
    //    partial void UpdateRIGHEDOCUMENTI(RIGHEDOCUMENTI instance);
    //    partial void DeleteRIGHEDOCUMENTI(RIGHEDOCUMENTI instance);
    //    partial void InsertAnalisiVenditaRata(AnalisiVenditaRata instance);
    //    partial void UpdateAnalisiVenditaRata(AnalisiVenditaRata instance);
    //    partial void DeleteAnalisiVenditaRata(AnalisiVenditaRata instance);
    //    partial void InsertAnalisiCostoRaggruppamento(AnalisiCostoRaggruppamento instance);
    //    partial void UpdateAnalisiCostoRaggruppamento(AnalisiCostoRaggruppamento instance);
    //    partial void DeleteAnalisiCostoRaggruppamento(AnalisiCostoRaggruppamento instance);
    //    partial void InsertAnalisiCostoArticolo(AnalisiCostoArticolo instance);
    //    partial void UpdateAnalisiCostoArticolo(AnalisiCostoArticolo instance);
    //    partial void DeleteAnalisiCostoArticolo(AnalisiCostoArticolo instance);
    //    partial void InsertAnalisiCostoArchivioCampoAggiuntivo(AnalisiCostoArchivioCampoAggiuntivo instance);
    //    partial void UpdateAnalisiCostoArchivioCampoAggiuntivo(AnalisiCostoArchivioCampoAggiuntivo instance);
    //    partial void DeleteAnalisiCostoArchivioCampoAggiuntivo(AnalisiCostoArchivioCampoAggiuntivo instance);
    //    partial void InsertAnalisiCostoArticoloCampoAggiuntivo(AnalisiCostoArticoloCampoAggiuntivo instance);
    //    partial void UpdateAnalisiCostoArticoloCampoAggiuntivo(AnalisiCostoArticoloCampoAggiuntivo instance);
    //    partial void DeleteAnalisiCostoArticoloCampoAggiuntivo(AnalisiCostoArticoloCampoAggiuntivo instance);
    //    partial void InsertAnalisiCosto(AnalisiCosto instance);
    //    partial void UpdateAnalisiCosto(AnalisiCosto instance);
    //    partial void DeleteAnalisiCosto(AnalisiCosto instance);
    //    partial void InsertAnalisiVendita(AnalisiVendita instance);
    //    partial void UpdateAnalisiVendita(AnalisiVendita instance);
    //    partial void DeleteAnalisiVendita(AnalisiVendita instance);
    //    partial void InsertTABGRUPPI(TABGRUPPI instance);
    //    partial void UpdateTABGRUPPI(TABGRUPPI instance);
    //    partial void DeleteTABGRUPPI(TABGRUPPI instance);
    //    partial void InsertTABCATEGORIE(TABCATEGORIE instance);
    //    partial void UpdateTABCATEGORIE(TABCATEGORIE instance);
    //    partial void DeleteTABCATEGORIE(TABCATEGORIE instance);
    //    partial void InsertTABCATEGORIESTAT(TABCATEGORIESTAT instance);
    //    partial void UpdateTABCATEGORIESTAT(TABCATEGORIESTAT instance);
    //    partial void DeleteTABCATEGORIESTAT(TABCATEGORIESTAT instance);
    //    partial void InsertOffertaRaggruppamento(OffertaRaggruppamento instance);
    //    partial void UpdateOffertaRaggruppamento(OffertaRaggruppamento instance);
    //    partial void DeleteOffertaRaggruppamento(OffertaRaggruppamento instance);
    //    partial void InsertOffertaArticolo(OffertaArticolo instance);
    //    partial void UpdateOffertaArticolo(OffertaArticolo instance);
    //    partial void DeleteOffertaArticolo(OffertaArticolo instance);
    //    partial void InsertOffertaArchivioCampoAggiuntivo(OffertaArchivioCampoAggiuntivo instance);
    //    partial void UpdateOffertaArchivioCampoAggiuntivo(OffertaArchivioCampoAggiuntivo instance);
    //    partial void DeleteOffertaArchivioCampoAggiuntivo(OffertaArchivioCampoAggiuntivo instance);
    //    partial void InsertOffertaArticoloCampoAggiuntivo(OffertaArticoloCampoAggiuntivo instance);
    //    partial void UpdateOffertaArticoloCampoAggiuntivo(OffertaArticoloCampoAggiuntivo instance);
    //    partial void DeleteOffertaArticoloCampoAggiuntivo(OffertaArticoloCampoAggiuntivo instance);
    //    partial void InsertCaratteristicaIntervento(CaratteristicaIntervento instance);
    //    partial void UpdateCaratteristicaIntervento(CaratteristicaIntervento instance);
    //    partial void DeleteCaratteristicaIntervento(CaratteristicaIntervento instance);
    //    partial void InsertCaratteristicaTipologiaIntervento(CaratteristicaTipologiaIntervento instance);
    //    partial void UpdateCaratteristicaTipologiaIntervento(CaratteristicaTipologiaIntervento instance);
    //    partial void DeleteCaratteristicaTipologiaIntervento(CaratteristicaTipologiaIntervento instance);
    //    partial void InsertAnagraficaSoggetti_Proprieta(AnagraficaSoggetti_Proprieta instance);
    //    partial void UpdateAnagraficaSoggetti_Proprieta(AnagraficaSoggetti_Proprieta instance);
    //    partial void DeleteAnagraficaSoggetti_Proprieta(AnagraficaSoggetti_Proprieta instance);
    //    partial void InsertAnagraficaClienti(AnagraficaClienti instance);
    //    partial void UpdateAnagraficaClienti(AnagraficaClienti instance);
    //    partial void DeleteAnagraficaClienti(AnagraficaClienti instance);
    //    partial void InsertIntervento_Discussione(Intervento_Discussione instance);
    //    partial void UpdateIntervento_Discussione(Intervento_Discussione instance);
    //    partial void DeleteIntervento_Discussione(Intervento_Discussione instance);
    //    partial void InsertCondizioneIntervento(CondizioneIntervento instance);
    //    partial void UpdateCondizioneIntervento(CondizioneIntervento instance);
    //    partial void DeleteCondizioneIntervento(CondizioneIntervento instance);
    //    partial void InsertConfigurazioneTipologiaTicketCliente(ConfigurazioneTipologiaTicketCliente instance);
    //    partial void UpdateConfigurazioneTipologiaTicketCliente(ConfigurazioneTipologiaTicketCliente instance);
    //    partial void DeleteConfigurazioneTipologiaTicketCliente(ConfigurazioneTipologiaTicketCliente instance);
    //    partial void InsertIntervento_CaratteristicaTipologiaIntervento(Intervento_CaratteristicaTipologiaIntervento instance);
    //    partial void UpdateIntervento_CaratteristicaTipologiaIntervento(Intervento_CaratteristicaTipologiaIntervento instance);
    //    partial void DeleteIntervento_CaratteristicaTipologiaIntervento(Intervento_CaratteristicaTipologiaIntervento instance);
    //    partial void InsertIntervento_ConfigurazioneTipologiaTicketCliente(Intervento_ConfigurazioneTipologiaTicketCliente instance);
    //    partial void UpdateIntervento_ConfigurazioneTipologiaTicketCliente(Intervento_ConfigurazioneTipologiaTicketCliente instance);
    //    partial void DeleteIntervento_ConfigurazioneTipologiaTicketCliente(Intervento_ConfigurazioneTipologiaTicketCliente instance);
    //    partial void InsertIntervento_OrarioRepartoUfficio(Intervento_OrarioRepartoUfficio instance);
    //    partial void UpdateIntervento_OrarioRepartoUfficio(Intervento_OrarioRepartoUfficio instance);
    //    partial void DeleteIntervento_OrarioRepartoUfficio(Intervento_OrarioRepartoUfficio instance);
    //    partial void InsertModelloConfigurazioneCaratteristicheTicketCliente(ModelloConfigurazioneCaratteristicheTicketCliente instance);
    //    partial void UpdateModelloConfigurazioneCaratteristicheTicketCliente(ModelloConfigurazioneCaratteristicheTicketCliente instance);
    //    partial void DeleteModelloConfigurazioneCaratteristicheTicketCliente(ModelloConfigurazioneCaratteristicheTicketCliente instance);
    //    partial void InsertModelloConfigurazioneTicketCliente(ModelloConfigurazioneTicketCliente instance);
    //    partial void UpdateModelloConfigurazioneTicketCliente(ModelloConfigurazioneTicketCliente instance);
    //    partial void DeleteModelloConfigurazioneTicketCliente(ModelloConfigurazioneTicketCliente instance);
    //    partial void InsertOrarioRepartoUfficio(OrarioRepartoUfficio instance);
    //    partial void UpdateOrarioRepartoUfficio(OrarioRepartoUfficio instance);
    //    partial void DeleteOrarioRepartoUfficio(OrarioRepartoUfficio instance);
    //    partial void InsertRepartoUfficio(RepartoUfficio instance);
    //    partial void UpdateRepartoUfficio(RepartoUfficio instance);
    //    partial void DeleteRepartoUfficio(RepartoUfficio instance);
    //    partial void InsertTipologiaIntervento(TipologiaIntervento instance);
    //    partial void UpdateTipologiaIntervento(TipologiaIntervento instance);
    //    partial void DeleteTipologiaIntervento(TipologiaIntervento instance);
    //    partial void InsertAnalisiVenditaConfigurazioneArticoloAggiuntivo(AnalisiVenditaConfigurazioneArticoloAggiuntivo instance);
    //    partial void UpdateAnalisiVenditaConfigurazioneArticoloAggiuntivo(AnalisiVenditaConfigurazioneArticoloAggiuntivo instance);
    //    partial void DeleteAnalisiVenditaConfigurazioneArticoloAggiuntivo(AnalisiVenditaConfigurazioneArticoloAggiuntivo instance);
    //    partial void InsertPeriodoFestivita(PeriodoFestivita instance);
    //    partial void UpdatePeriodoFestivita(PeriodoFestivita instance);
    //    partial void DeletePeriodoFestivita(PeriodoFestivita instance);
    //    partial void InsertANAGRAFICAARTICOLI(ANAGRAFICAARTICOLI instance);
    //    partial void UpdateANAGRAFICAARTICOLI(ANAGRAFICAARTICOLI instance);
    //    partial void DeleteANAGRAFICAARTICOLI(ANAGRAFICAARTICOLI instance);
    //    partial void InsertOfferta(Offerta instance);
    //    partial void UpdateOfferta(Offerta instance);
    //    partial void DeleteOfferta(Offerta instance);
    //    partial void InsertOffertaAccountValidatore(OffertaAccountValidatore instance);
    //    partial void UpdateOffertaAccountValidatore(OffertaAccountValidatore instance);
    //    partial void DeleteOffertaAccountValidatore(OffertaAccountValidatore instance);
    //    partial void InsertTABPAGAMENTI(TABPAGAMENTI instance);
    //    partial void UpdateTABPAGAMENTI(TABPAGAMENTI instance);
    //    partial void DeleteTABPAGAMENTI(TABPAGAMENTI instance);
    //    partial void InsertANAGRAFICARISERVATICF(ANAGRAFICARISERVATICF instance);
    //    partial void UpdateANAGRAFICARISERVATICF(ANAGRAFICARISERVATICF instance);
    //    partial void DeleteANAGRAFICARISERVATICF(ANAGRAFICARISERVATICF instance);
    //    partial void InsertBANCAAPPCF(BANCAAPPCF instance);
    //    partial void UpdateBANCAAPPCF(BANCAAPPCF instance);
    //    partial void DeleteBANCAAPPCF(BANCAAPPCF instance);
    //    partial void InsertMappaturaGruppoCategoriaCategoriaStatistica(MappaturaGruppoCategoriaCategoriaStatistica instance);
    //    partial void UpdateMappaturaGruppoCategoriaCategoriaStatistica(MappaturaGruppoCategoriaCategoriaStatistica instance);
    //    partial void DeleteMappaturaGruppoCategoriaCategoriaStatistica(MappaturaGruppoCategoriaCategoriaStatistica instance);
    //    partial void InsertAnalisiVenditaConfigurazioneArticoloAggiuntivoClausolaVessatoria(AnalisiVenditaConfigurazioneArticoloAggiuntivoClausolaVessatoria instance);
    //    partial void UpdateAnalisiVenditaConfigurazioneArticoloAggiuntivoClausolaVessatoria(AnalisiVenditaConfigurazioneArticoloAggiuntivoClausolaVessatoria instance);
    //    partial void DeleteAnalisiVenditaConfigurazioneArticoloAggiuntivoClausolaVessatoria(AnalisiVenditaConfigurazioneArticoloAggiuntivoClausolaVessatoria instance);
    //    partial void InsertClausolaVessatoria(ClausolaVessatoria instance);
    //    partial void UpdateClausolaVessatoria(ClausolaVessatoria instance);
    //    partial void DeleteClausolaVessatoria(ClausolaVessatoria instance);
    //    partial void InsertServizio(Servizio instance);
    //    partial void UpdateServizio(Servizio instance);
    //    partial void DeleteServizio(Servizio instance);
    //    partial void InsertServizioArticolo(ServizioArticolo instance);
    //    partial void UpdateServizioArticolo(ServizioArticolo instance);
    //    partial void DeleteServizioArticolo(ServizioArticolo instance);
    //    partial void InsertProgetto(Progetto instance);
    //    partial void UpdateProgetto(Progetto instance);
    //    partial void DeleteProgetto(Progetto instance);
    //    partial void InsertProgetto_Attivita(Progetto_Attivita instance);
    //    partial void UpdateProgetto_Attivita(Progetto_Attivita instance);
    //    partial void DeleteProgetto_Attivita(Progetto_Attivita instance);
    //    partial void InsertProgetto_Operatore(Progetto_Operatore instance);
    //    partial void UpdateProgetto_Operatore(Progetto_Operatore instance);
    //    partial void DeleteProgetto_Operatore(Progetto_Operatore instance);
    //    partial void InsertAttivita(Attivita instance);
    //    partial void UpdateAttivita(Attivita instance);
    //    partial void DeleteAttivita(Attivita instance);
    //    #endregion

    //    public DatabaseDataContext() :
    //            base(System.Configuration.ConfigurationManager.ConnectionStrings["DatabaseConnectionString"].ConnectionString, mappingSource)
    //    {
    //        OnCreated();
    //    }

    //    public DatabaseDataContext(string connection) :
    //            base(connection, mappingSource)
    //    {
    //        OnCreated();
    //    }

    //    public DatabaseDataContext(System.Data.IDbConnection connection) :
    //            base(connection, mappingSource)
    //    {
    //        OnCreated();
    //    }

    //    public DatabaseDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) :
    //            base(connection, mappingSource)
    //    {
    //        OnCreated();
    //    }

    //    public DatabaseDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) :
    //            base(connection, mappingSource)
    //    {
    //        OnCreated();
    //    }

    //    public System.Data.Linq.Table<Allegato> Allegatoes
    //    {
    //        get
    //        {
    //            return this.GetTable<Allegato>();
    //        }
    //    }

    //    public System.Data.Linq.Table<Operatore> Operatores
    //    {
    //        get
    //        {
    //            return this.GetTable<Operatore>();
    //        }
    //    }

    //    public System.Data.Linq.Table<Intervento> Interventos
    //    {
    //        get
    //        {
    //            return this.GetTable<Intervento>();
    //        }
    //    }

    //    public System.Data.Linq.Table<Intervento_Articolo> Intervento_Articolos
    //    {
    //        get
    //        {
    //            return this.GetTable<Intervento_Articolo>();
    //        }
    //    }

    //    public System.Data.Linq.Table<Account> Accounts
    //    {
    //        get
    //        {
    //            return this.GetTable<Account>();
    //        }
    //    }

    //    public System.Data.Linq.Table<Intervento_Operatore> Intervento_Operatores
    //    {
    //        get
    //        {
    //            return this.GetTable<Intervento_Operatore>();
    //        }
    //    }

    //    public System.Data.Linq.Table<ModalitaRisoluzioneIntervento> ModalitaRisoluzioneInterventos
    //    {
    //        get
    //        {
    //            return this.GetTable<ModalitaRisoluzioneIntervento>();
    //        }
    //    }

    //    public System.Data.Linq.Table<ElencoCompletoArticoli> ElencoCompletoArticolis
    //    {
    //        get
    //        {
    //            return this.GetTable<ElencoCompletoArticoli>();
    //        }
    //    }

    //    public System.Data.Linq.Table<Intervento_Stato> Intervento_Statos
    //    {
    //        get
    //        {
    //            return this.GetTable<Intervento_Stato>();
    //        }
    //    }

    //    public System.Data.Linq.Table<InformazioniContratto> InformazioniContrattos
    //    {
    //        get
    //        {
    //            return this.GetTable<InformazioniContratto>();
    //        }
    //    }

    //    public System.Data.Linq.Table<VocePredefinitaIntervento> VocePredefinitaInterventos
    //    {
    //        get
    //        {
    //            return this.GetTable<VocePredefinitaIntervento>();
    //        }
    //    }

    //    public System.Data.Linq.Table<DESTINAZIONIDIVERSE> DESTINAZIONIDIVERSEs
    //    {
    //        get
    //        {
    //            return this.GetTable<DESTINAZIONIDIVERSE>();
    //        }
    //    }

    //    public System.Data.Linq.Table<ARTICOLIUNITAMISURA> ARTICOLIUNITAMISURAs
    //    {
    //        get
    //        {
    //            return this.GetTable<ARTICOLIUNITAMISURA>();
    //        }
    //    }

    //    public System.Data.Linq.Table<AccountOperatore> AccountOperatores
    //    {
    //        get
    //        {
    //            return this.GetTable<AccountOperatore>();
    //        }
    //    }

    //    public System.Data.Linq.Table<LogInvioNotifica> LogInvioNotificas
    //    {
    //        get
    //        {
    //            return this.GetTable<LogInvioNotifica>();
    //        }
    //    }

    //    public System.Data.Linq.Table<Notifica> Notificas
    //    {
    //        get
    //        {
    //            return this.GetTable<Notifica>();
    //        }
    //    }

    //    public System.Data.Linq.Table<InfoOperazioneRecord> InfoOperazioneRecords
    //    {
    //        get
    //        {
    //            return this.GetTable<InfoOperazioneRecord>();
    //        }
    //    }

    //    public System.Data.Linq.Table<InfoOperazioneRecord_Tabella> InfoOperazioneRecord_Tabellas
    //    {
    //        get
    //        {
    //            return this.GetTable<InfoOperazioneRecord_Tabella>();
    //        }
    //    }

    //    public System.Data.Linq.Table<TESTEDOCUMENTI> TESTEDOCUMENTIs
    //    {
    //        get
    //        {
    //            return this.GetTable<TESTEDOCUMENTI>();
    //        }
    //    }

    //    public System.Data.Linq.Table<RIGHEDOCUMENTI> RIGHEDOCUMENTIs
    //    {
    //        get
    //        {
    //            return this.GetTable<RIGHEDOCUMENTI>();
    //        }
    //    }

    //    public System.Data.Linq.Table<AnalisiVenditaRata> AnalisiVenditaRatas
    //    {
    //        get
    //        {
    //            return this.GetTable<AnalisiVenditaRata>();
    //        }
    //    }

    //    public System.Data.Linq.Table<AnalisiCostoRaggruppamento> AnalisiCostoRaggruppamentos
    //    {
    //        get
    //        {
    //            return this.GetTable<AnalisiCostoRaggruppamento>();
    //        }
    //    }

    //    public System.Data.Linq.Table<AnalisiCostoArticolo> AnalisiCostoArticolos
    //    {
    //        get
    //        {
    //            return this.GetTable<AnalisiCostoArticolo>();
    //        }
    //    }

    //    public System.Data.Linq.Table<AnalisiCostoArchivioCampoAggiuntivo> AnalisiCostoArchivioCampoAggiuntivos
    //    {
    //        get
    //        {
    //            return this.GetTable<AnalisiCostoArchivioCampoAggiuntivo>();
    //        }
    //    }

    //    public System.Data.Linq.Table<AnalisiCostoArticoloCampoAggiuntivo> AnalisiCostoArticoloCampoAggiuntivos
    //    {
    //        get
    //        {
    //            return this.GetTable<AnalisiCostoArticoloCampoAggiuntivo>();
    //        }
    //    }

    //    public System.Data.Linq.Table<AnalisiCosto> AnalisiCostos
    //    {
    //        get
    //        {
    //            return this.GetTable<AnalisiCosto>();
    //        }
    //    }

    //    public System.Data.Linq.Table<AnalisiVendita> AnalisiVenditas
    //    {
    //        get
    //        {
    //            return this.GetTable<AnalisiVendita>();
    //        }
    //    }

    //    public System.Data.Linq.Table<TABGRUPPI> TABGRUPPIs
    //    {
    //        get
    //        {
    //            return this.GetTable<TABGRUPPI>();
    //        }
    //    }

    //    public System.Data.Linq.Table<TABCATEGORIE> TABCATEGORIEs
    //    {
    //        get
    //        {
    //            return this.GetTable<TABCATEGORIE>();
    //        }
    //    }

    //    public System.Data.Linq.Table<TABCATEGORIESTAT> TABCATEGORIESTATs
    //    {
    //        get
    //        {
    //            return this.GetTable<TABCATEGORIESTAT>();
    //        }
    //    }

    //    public System.Data.Linq.Table<OffertaRaggruppamento> OffertaRaggruppamentos
    //    {
    //        get
    //        {
    //            return this.GetTable<OffertaRaggruppamento>();
    //        }
    //    }

    //    public System.Data.Linq.Table<OffertaArticolo> OffertaArticolos
    //    {
    //        get
    //        {
    //            return this.GetTable<OffertaArticolo>();
    //        }
    //    }

    //    public System.Data.Linq.Table<OffertaArchivioCampoAggiuntivo> OffertaArchivioCampoAggiuntivos
    //    {
    //        get
    //        {
    //            return this.GetTable<OffertaArchivioCampoAggiuntivo>();
    //        }
    //    }

    //    public System.Data.Linq.Table<OffertaArticoloCampoAggiuntivo> OffertaArticoloCampoAggiuntivos
    //    {
    //        get
    //        {
    //            return this.GetTable<OffertaArticoloCampoAggiuntivo>();
    //        }
    //    }

    //    public System.Data.Linq.Table<CaratteristicaIntervento> CaratteristicaInterventos
    //    {
    //        get
    //        {
    //            return this.GetTable<CaratteristicaIntervento>();
    //        }
    //    }

    //    public System.Data.Linq.Table<CaratteristicaTipologiaIntervento> CaratteristicaTipologiaInterventos
    //    {
    //        get
    //        {
    //            return this.GetTable<CaratteristicaTipologiaIntervento>();
    //        }
    //    }

    //    public System.Data.Linq.Table<AnagraficaSoggetti_Proprieta> AnagraficaSoggetti_Proprietas
    //    {
    //        get
    //        {
    //            return this.GetTable<AnagraficaSoggetti_Proprieta>();
    //        }
    //    }

    //    public System.Data.Linq.Table<AnagraficaClienti> AnagraficaClientis
    //    {
    //        get
    //        {
    //            return this.GetTable<AnagraficaClienti>();
    //        }
    //    }

    //    public System.Data.Linq.Table<Intervento_Discussione> Intervento_Discussiones
    //    {
    //        get
    //        {
    //            return this.GetTable<Intervento_Discussione>();
    //        }
    //    }

    //    public System.Data.Linq.Table<CondizioneIntervento> CondizioneInterventos
    //    {
    //        get
    //        {
    //            return this.GetTable<CondizioneIntervento>();
    //        }
    //    }

    //    public System.Data.Linq.Table<ConfigurazioneTipologiaTicketCliente> ConfigurazioneTipologiaTicketClientes
    //    {
    //        get
    //        {
    //            return this.GetTable<ConfigurazioneTipologiaTicketCliente>();
    //        }
    //    }

    //    public System.Data.Linq.Table<Intervento_CaratteristicaTipologiaIntervento> Intervento_CaratteristicaTipologiaInterventos
    //    {
    //        get
    //        {
    //            return this.GetTable<Intervento_CaratteristicaTipologiaIntervento>();
    //        }
    //    }

    //    public System.Data.Linq.Table<Intervento_ConfigurazioneTipologiaTicketCliente> Intervento_ConfigurazioneTipologiaTicketClientes
    //    {
    //        get
    //        {
    //            return this.GetTable<Intervento_ConfigurazioneTipologiaTicketCliente>();
    //        }
    //    }

    //    public System.Data.Linq.Table<Intervento_OrarioRepartoUfficio> Intervento_OrarioRepartoUfficios
    //    {
    //        get
    //        {
    //            return this.GetTable<Intervento_OrarioRepartoUfficio>();
    //        }
    //    }

    //    public System.Data.Linq.Table<ModelloConfigurazioneCaratteristicheTicketCliente> ModelloConfigurazioneCaratteristicheTicketClientes
    //    {
    //        get
    //        {
    //            return this.GetTable<ModelloConfigurazioneCaratteristicheTicketCliente>();
    //        }
    //    }

    //    public System.Data.Linq.Table<ModelloConfigurazioneTicketCliente> ModelloConfigurazioneTicketClientes
    //    {
    //        get
    //        {
    //            return this.GetTable<ModelloConfigurazioneTicketCliente>();
    //        }
    //    }

    //    public System.Data.Linq.Table<OrarioRepartoUfficio> OrarioRepartoUfficios
    //    {
    //        get
    //        {
    //            return this.GetTable<OrarioRepartoUfficio>();
    //        }
    //    }

    //    public System.Data.Linq.Table<RepartoUfficio> RepartoUfficios
    //    {
    //        get
    //        {
    //            return this.GetTable<RepartoUfficio>();
    //        }
    //    }

    //    public System.Data.Linq.Table<TipologiaIntervento> TipologiaInterventos
    //    {
    //        get
    //        {
    //            return this.GetTable<TipologiaIntervento>();
    //        }
    //    }

    //    public System.Data.Linq.Table<AnalisiVenditaConfigurazioneArticoloAggiuntivo> AnalisiVenditaConfigurazioneArticoloAggiuntivos
    //    {
    //        get
    //        {
    //            return this.GetTable<AnalisiVenditaConfigurazioneArticoloAggiuntivo>();
    //        }
    //    }

    //    public System.Data.Linq.Table<PeriodoFestivita> PeriodoFestivitas
    //    {
    //        get
    //        {
    //            return this.GetTable<PeriodoFestivita>();
    //        }
    //    }

    //    public System.Data.Linq.Table<ANAGRAFICAARTICOLI> ANAGRAFICAARTICOLIs
    //    {
    //        get
    //        {
    //            return this.GetTable<ANAGRAFICAARTICOLI>();
    //        }
    //    }

    //    public System.Data.Linq.Table<Offerta> Offertas
    //    {
    //        get
    //        {
    //            return this.GetTable<Offerta>();
    //        }
    //    }

    //    public System.Data.Linq.Table<OffertaAccountValidatore> OffertaAccountValidatores
    //    {
    //        get
    //        {
    //            return this.GetTable<OffertaAccountValidatore>();
    //        }
    //    }

    //    public System.Data.Linq.Table<TABPAGAMENTI> TABPAGAMENTIs
    //    {
    //        get
    //        {
    //            return this.GetTable<TABPAGAMENTI>();
    //        }
    //    }

    //    public System.Data.Linq.Table<ANAGRAFICARISERVATICF> ANAGRAFICARISERVATICFs
    //    {
    //        get
    //        {
    //            return this.GetTable<ANAGRAFICARISERVATICF>();
    //        }
    //    }

    //    public System.Data.Linq.Table<BANCAAPPCF> BANCAAPPCFs
    //    {
    //        get
    //        {
    //            return this.GetTable<BANCAAPPCF>();
    //        }
    //    }

    //    public System.Data.Linq.Table<MappaturaGruppoCategoriaCategoriaStatistica> MappaturaGruppoCategoriaCategoriaStatisticas
    //    {
    //        get
    //        {
    //            return this.GetTable<MappaturaGruppoCategoriaCategoriaStatistica>();
    //        }
    //    }

    //    public System.Data.Linq.Table<AnalisiVenditaConfigurazioneArticoloAggiuntivoClausolaVessatoria> AnalisiVenditaConfigurazioneArticoloAggiuntivoClausolaVessatorias
    //    {
    //        get
    //        {
    //            return this.GetTable<AnalisiVenditaConfigurazioneArticoloAggiuntivoClausolaVessatoria>();
    //        }
    //    }

    //    public System.Data.Linq.Table<ClausolaVessatoria> ClausolaVessatorias
    //    {
    //        get
    //        {
    //            return this.GetTable<ClausolaVessatoria>();
    //        }
    //    }

    //    public System.Data.Linq.Table<StatisticaFatturato> StatisticaFatturatos
    //    {
    //        get
    //        {
    //            return this.GetTable<StatisticaFatturato>();
    //        }
    //    }

    //    public System.Data.Linq.Table<Servizio> Servizios
    //    {
    //        get
    //        {
    //            return this.GetTable<Servizio>();
    //        }
    //    }

    //    public System.Data.Linq.Table<ServizioArticolo> ServizioArticolos
    //    {
    //        get
    //        {
    //            return this.GetTable<ServizioArticolo>();
    //        }
    //    }

    //    public System.Data.Linq.Table<InterventiConAreeMultiple> InterventiConAreeMultiple
    //    {
    //        get
    //        {
    //            return this.GetTable<InterventiConAreeMultiple>();
    //        }
    //    }

    //    public System.Data.Linq.Table<IdInterventiConAreeMultiple> IdInterventiConAreeMultiple
    //    {
    //        get
    //        {
    //            return this.GetTable<IdInterventiConAreeMultiple>();
    //        }
    //    }

    //    public System.Data.Linq.Table<Progetto> Progettos
    //    {
    //        get
    //        {
    //            return this.GetTable<Progetto>();
    //        }
    //    }

    //    public System.Data.Linq.Table<Progetto_Attivita> Progetto_Attivitas
    //    {
    //        get
    //        {
    //            return this.GetTable<Progetto_Attivita>();
    //        }
    //    }

    //    public System.Data.Linq.Table<Progetto_Operatore> Progetto_Operatores
    //    {
    //        get
    //        {
    //            return this.GetTable<Progetto_Operatore>();
    //        }
    //    }

    //    public System.Data.Linq.Table<Attivita> Attivitas
    //    {
    //        get
    //        {
    //            return this.GetTable<Attivita>();
    //        }
    //    }

    //    [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "SeCoGEST.CreaDocXMLInterventi")]
    //    public int CreaDocXMLInterventi([global::System.Data.Linq.Mapping.ParameterAttribute(DbType = "uniqueidentifier")] System.Nullable<System.Guid> IDTESTA)
    //    {
    //        IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), IDTESTA);
    //        return ((int)(result.ReturnValue));
    //    }

    //    [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "SeCoGEST.GetContrattiPerCliente")]
    //    public ISingleResult<InformazioniContratto> GetContrattiPerCliente([global::System.Data.Linq.Mapping.ParameterAttribute(Name = "CodiceCliente", DbType = "VarChar(7)")] string codiceCliente, [global::System.Data.Linq.Mapping.ParameterAttribute(Name = "DataIntervento", DbType = "DateTime")] System.Nullable<System.DateTime> dataIntervento)
    //    {
    //        IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), codiceCliente, dataIntervento);
    //        return ((ISingleResult<InformazioniContratto>)(result.ReturnValue));
    //    }

    //    [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "SeCoGEST.GetElencoCompletoArticoli", IsComposable = true)]
    //    public IQueryable<ElencoCompletoArticoli> GetElencoCompletoArticoli([global::System.Data.Linq.Mapping.ParameterAttribute(DbType = "DateTime")] System.Nullable<System.DateTime> dataValidita)
    //    {
    //        return this.CreateMethodCallQuery<ElencoCompletoArticoli>(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), dataValidita);
    //    }

    //    [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "SeCoGEST.GetDataUltimoInvioNotifica", IsComposable = true)]
    //    public System.Nullable<System.DateTime> GetDataUltimoInvioNotifica([global::System.Data.Linq.Mapping.ParameterAttribute(Name = "IDLegame", DbType = "UniqueIdentifier")] System.Nullable<System.Guid> iDLegame, [global::System.Data.Linq.Mapping.ParameterAttribute(Name = "IDTabellaLegame", DbType = "TinyInt")] System.Nullable<byte> iDTabellaLegame, [global::System.Data.Linq.Mapping.ParameterAttribute(Name = "IDNotifica", DbType = "TinyInt")] System.Nullable<byte> iDNotifica)
    //    {
    //        return ((System.Nullable<System.DateTime>)(this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), iDLegame, iDTabellaLegame, iDNotifica).ReturnValue));
    //    }
    //}


}
