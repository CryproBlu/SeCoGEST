using System.Data.Linq;
//using SeCoGEST.Entities.Applicazione;
//using SeCoGes.Hdemia.Helper;

namespace SeCoGEST.Data.Base
{
    public class Database
    {
        private DatabaseDataContext context;

        /// <summary>
        /// Memorizza nel database le modifiche applicate alle Entities del DataContext in uso
        /// </summary>
        public Database(DatabaseDataContext currentContext)
        {
            context = currentContext;
        }


        private enum TipologiaCambiamenti
        {
            Creazione,
            Modifica,
            Eliminazione
        }


        /// <summary>
        /// Memorizza nel database le modifiche applicate alle Entities del DataContext in uso
        /// </summary>
        public void SubmitChanges()
        {
            //// Recupero tutti i cambiamenti effettuati nel DataContext
            //ChangeSet changes = this.context.GetChangeSet();

            //// Effettuo un ciclo su tutte le entità create
            //foreach (object campo in changes.Inserts)
            //{
            //    InfoOperazioneTabellaEnum? tabellaEnum = GetTabellaEnum(campo);
            //    if (tabellaEnum.HasValue) CreaInfoOperazioneRecord(campo, tabellaEnum.Value, TipologiaCambiamenti.Creazione);
            //}

            //// Effettuo un ciclo su tutte le entità modificate
            //foreach (object campo in changes.Updates)
            //{
            //    InfoOperazioneTabellaEnum? tabellaEnum = GetTabellaEnum(campo);
            //    if (tabellaEnum.HasValue)
            //    {
            //        ValidazioneEntityInModifica(campo, tabellaEnum.Value);
            //        CreaInfoOperazioneRecord(campo, tabellaEnum.Value, TipologiaCambiamenti.Modifica);
            //    }
            //}

            //// Effettuo un ciclo su tutte le entità modificate
            //foreach (object campo in changes.Deletes)
            //{
            //    InfoOperazioneTabellaEnum? tabellaEnum = GetTabellaEnum(campo);
            //    if (tabellaEnum.HasValue) CreaInfoOperazioneRecord(campo, tabellaEnum.Value, TipologiaCambiamenti.Eliminazione);
            //}

            this.context.SubmitChanges();
        }

        //private InfoOperazioneTabellaEnum? GetTabellaEnum(object campo)
        //{
        //    try
        //    {

        //        // Recupera il corretto valore dell'enumeratore che si riferisce alla tabella relativa al campo modificato
        //        return EnumHelper.GetValueFromDescription<InfoOperazioneTabellaEnum>(campo.GetType().ToString());
        //    }
        //    catch
        //    {
        //        return null;
        //    }
        //}

        /// <summary>
        /// Effettua una validazione sulle modifiche applicate alle entity controllando che rispettino alcune regole basilari
        /// </summary>
        /// <param name="campo"></param>
        //private void ValidazioneEntityInModifica(object campo, InfoOperazioneTabellaEnum tabellaEnum)
        //{
        //    //// Se le modifiche sono state applicate a entitis di tipo Soggetto
        //    //// allora controllo se è stata cambiata la Funzione.
        //    //// Ci sono 2 motivi per questo controllo:
        //    //// 1) Le entities di tipo AttrezzaturaLudica hanno un riferimento con i soggetti di funzione Produttore.
        //    ////    Se ad un soggetto Produttore viene cambiata la funzione, il sistema controlla che questo soggetto non sia indicato in qualche Attrezzatura Ludica.
        //    //// 2) Le entities di tipo Manutenzione_Soggetto hanno un riferimento con i soggetti di funzione Manutentore.
        //    ////    Se ad un soggetto Manutentore viene cambiata la funzione, il sistema controlla che questo soggetto non sia indicato come manutentore in qualche Manutenzione.
        //    //// Se così è allora annulla la modifica.
        //    //if (tabellaEnum == InfoOperazioneTabellaEnum.Soggetto)
        //    //{
        //    //    // Recupera l'entity con i valori originali (prima che venissero modificati)
        //    //    EntityLayer.Soggetto soggettoOriginale = (EntityLayer.Soggetto)this.context.GetTable(campo.GetType()).GetOriginalEntityState(campo);                

        //    //    // Se in origine il Soggetto era di funzione Produttore...
        //    //    if (soggettoOriginale.IDFunzioneSoggetto != null && soggettoOriginale.IDFunzioneSoggetto.Equals(ConfigurationKeys.ID_FUNZIONE_SOGGETTO_PRODUTTORE))
        //    //    {
        //    //        // ...ed ora la sua funzione è un altra...
        //    //        EntityLayer.Soggetto soggettoModificato = (EntityLayer.Soggetto)campo;
        //    //        if (soggettoModificato.IDFunzioneSoggetto != soggettoOriginale.IDFunzioneSoggetto)
        //    //        {
        //    //            // ...allora controllo che non sia indicato in una o più Attrezzature Ludiche
        //    //            AttrezzatureLudiche dalAL = new AttrezzatureLudiche(true);
        //    //            IQueryable<EntityLayer.AttrezzaturaLudica> attrezzatureDelSoggetto = dalAL.ReadBySoggetto(soggettoModificato);

        //    //            // Se è indicato in una o più Attrezzature Ludiche allora genero un errore di run-time con la spiegazione dell'errore rilevato
        //    //            int numAttrezzatureDelSoggetto = attrezzatureDelSoggetto.Count();
        //    //            if (numAttrezzatureDelSoggetto > 0)
        //    //            {
        //    //                string msg = string.Empty;
        //    //                if (numAttrezzatureDelSoggetto == 1)
        //    //                {
        //    //                    msg = "Il Soggetto che si sta modificando è indicato come Produttore per una Attrezzatura Ludica. Non è possibile modificare la sua funzione finche esiste questa associazione.\n\r\n\r";
        //    //                }
        //    //                else
        //    //                {
        //    //                    msg = string.Format("Il Soggetto che si sta modificando è indicato come Produttore per {0} Attrezzature Ludiche. Non è possibile modificare la sua funzione finche esistono queste associazioni.\n\r\n\r", numAttrezzatureDelSoggetto);
        //    //                }
        //    //                int numAttrezzatureDaMostrare = 5;
        //    //                if (numAttrezzatureDelSoggetto > numAttrezzatureDaMostrare)
        //    //                {
        //    //                    msg = string.Concat(msg, "Elenco parziale Attrezzature Ludiche:\n\r");
        //    //                }
        //    //                foreach (EntityLayer.AttrezzaturaLudica attLud in attrezzatureDelSoggetto.Take(numAttrezzatureDaMostrare))
        //    //                {
        //    //                    msg = string.Concat(msg, "- <a href='/Anagrafiche/AttrezzaturaLudica.aspx?ID=", attLud.ID.ToString(), "' target='_blank'>", attLud.Denominazione, "</a>\n\r");
        //    //                }
        //    //                msg = string.Concat(msg, "\n\rOperazione annullata.");

        //    //                throw new Exception(msg);
        //    //            }
        //    //        }
        //    //    }

        //    //    // Se in origine il Soggetto era di funzione Manutentore...
        //    //    else if (soggettoOriginale.IDFunzioneSoggetto != null && soggettoOriginale.IDFunzioneSoggetto.Equals(ConfigurationKeys.ID_FUNZIONE_SOGGETTO_MANUTENTORE))
        //    //    {
        //    //        // ...ed ora la sua funzione è un altra...
        //    //        EntityLayer.Soggetto soggettoModificato = (EntityLayer.Soggetto)campo;
        //    //        if (soggettoModificato.IDFunzioneSoggetto != soggettoOriginale.IDFunzioneSoggetto)
        //    //        {
        //    //            // ...allora controllo che non sia indicato in una o più Manutenzioni
        //    //            Manutenzioni_Soggetti dalMS = new Manutenzioni_Soggetti(true);
        //    //            IQueryable<EntityLayer.Manutenzione_Soggetto> manutenzioniDelSoggetto = dalMS.Read().Where(s => s.IDSoggetto == soggettoModificato.ID);

        //    //            // Se è indicato in una o più Manutenzioni allora genero un errore di run-time con la spiegazione dell'errore rilevato
        //    //            int numManutenzioni = manutenzioniDelSoggetto.Count();
        //    //            if (numManutenzioni > 0)
        //    //            {
        //    //                string msg = string.Empty;
        //    //                if (numManutenzioni == 1)
        //    //                {
        //    //                    msg = "Il Soggetto che si sta modificando è indicato come Manutentore di una Manutenzione. Non è possibile modificare la sua funzione finche esiste questa associazione.\n\r\n\r";
        //    //                }
        //    //                else
        //    //                {
        //    //                    msg = string.Format("Il Soggetto che si sta modificando è indicato come Manutentore per {0} Manutenzioni. Non è possibile modificare la sua funzione finche esistono queste associazioni.\n\r\n\r", numManutenzioni);
        //    //                }
        //    //                int numManutenzioniDaMostrare = 5;
        //    //                if (numManutenzioni > numManutenzioniDaMostrare)
        //    //                {
        //    //                    msg = string.Concat(msg, "Elenco parziale Manutenzioni:\n\r");
        //    //                }
        //    //                foreach (EntityLayer.Manutenzione_Soggetto manut in manutenzioniDelSoggetto.Take(numManutenzioniDaMostrare))
        //    //                {
        //    //                    msg = string.Concat(msg, "- <a href='/Manutenzione/Manutenzione.aspx?ID=", manut.IDManutenzione.ToString(), "' target='_blank'>", manut.Manutenzione.Descrizione, "</a>\n\r");
        //    //                }
        //    //                msg = string.Concat(msg, "\n\rOperazione annullata.");

        //    //                throw new Exception(msg);
        //    //            }
        //    //        }
        //    //    }

        //    //}
        //}


        /// <summary>
        /// Aggiunge le entity relative alle informazioni sulla creazione, modifica, eliminazione dei records
        /// </summary>
        /// <param name="campo"></param>
        //private void CreaInfoOperazioneRecord(object campo, InfoOperazioneTabellaEnum tabellaEnum, TipologiaCambiamenti tipoCambiamenti)
        //{
        //    //try
        //    //{
        //    //    // Nel caso venisse generato un errore di run-time questo viene ignorato e le informazioni di modifica del record non verranno inserire nel database
        //    //    // L'errore di run-time viene generato per il fallimento del recupero del valore dell'enumeratore in base al tipo dell'entity in gestione.
        //    //    // Visto che le informazioni di modifica dei records non vengono memorizzate per tutte le tipologie di entity 
        //    //    // è probabile che questo codice sia eseguito anche quando non serve. Non si è usato uno Switch per evitare di eseguire questo codice sui tipi non gestiti.
        //    //    // Semplicemente non si propaga l'errore. Questo avrà un costo in performances ma da la libertà di ignorare eventuali modifiche sulle entity da gestire.
        //    //    // Le uniche entities gestite sono quelle specificate nell'enumeratore InfoOperazioneTabellaEnum

        //    //    // Recupera il corretto valore dell'enumeratore che si riferisce alla tabella relativa al campo modificato
        //    //    //InfoOperazioneTabellaEnum tabellaEnum = EnumHelper.GetValueFromDescription<InfoOperazioneTabellaEnum>(campo.GetType().ToString());

        //    //    dynamic campoModificato = campo;

        //    //    string cambiamenti = null;
        //    //    try
        //    //    {
        //    //        if (ConfigurationKeys.LOG_AVANZATO_MODIFICHE)
        //    //        {
        //    //            cambiamenti = string.Empty;
        //    //            switch (tipoCambiamenti)
        //    //            {
        //    //                case TipologiaCambiamenti.Creazione:
        //    //                    cambiamenti = string.Concat("Creazione entità - ", GetInformazioneEntity(campo));
        //    //                    break;

        //    //                case TipologiaCambiamenti.Modifica:
        //    //                    ModifiedMemberInfo[] campiModificati = this.context.GetTable(campo.GetType()).GetModifiedMembers(campo);
        //    //                    foreach (var cam in campiModificati)
        //    //                    {
        //    //                        string valoreOriginale = string.Empty;
        //    //                        if (cam.OriginalValue != null) valoreOriginale = cam.OriginalValue.ToString();

        //    //                        string valoreCorrente = string.Empty;
        //    //                        if (cam.CurrentValue != null) valoreCorrente = cam.CurrentValue.ToString();

        //    //                        cambiamenti += string.Format("Campo: '{0}'\nOriginale: '{1}'\nModificato: '{2}'\n", cam.Member.Name, valoreOriginale, valoreCorrente, Environment.NewLine);
        //    //                    }
        //    //                    break;

        //    //                case TipologiaCambiamenti.Eliminazione:
        //    //                    cambiamenti = string.Concat("Eliminazione entità - ", GetInformazioneEntity(campo));
        //    //                    break;

        //    //                default:
        //    //                    break;
        //    //            }
        //    //        }
        //    //    }
        //    //    catch
        //    //    { }


        //    //    // Aggiunge le informazioni sull'utente che ha salvato i dati

        //    //    DataLayer.InfoOperazioniRecord dal = new DataLayer.InfoOperazioniRecord(this.context);
        //    //    EntityLayer.InfoOperazioneRecord info = new EntityLayer.InfoOperazioneRecord();
        //    //    info.IDLegame = GetIdentificativoEntityPerLog(campoModificato);
        //    //    info.IDTabellaLegame = (byte)tabellaEnum;
        //    //    info.UserName = System.Web.HttpContext.Current.User.Identity.Name;
        //    //    info.DataOperazione = DateTime.Now;
        //    //    info.DescrizioneModifica = cambiamenti;
        //    //    this.context.InfoOperazioneRecords.InsertOnSubmit(info);
        //    //}
        //    //catch
        //    //{ }
        //}



        /// <summary>
        /// Restituisce un valore stringa relativo al valore identificativo usato la scrittura/lettura dei log delle operazioni per l'entity passata
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public string GetIdentificativoEntityPerLog(dynamic entity)
        {
            //if (entity is EntityLayer.Segnalazione_Utente)
            //{
            //    return ((EntityLayer.Segnalazione_Utente)entity).IDSegnalazione.ToString();
            //}

            //else if (entity is EntityLayer.Segnalazione_Utente_Nota)
            //{
            //    return ((EntityLayer.Segnalazione_Utente_Nota)entity).IDSegnalazione.ToString();
            //}

            //else if (entity is EntityLayer.Manutenzione_Segnalazione)
            //{
            //    return ((EntityLayer.Manutenzione_Segnalazione)entity).IDManutenzione.ToString();
            //}

            //else if (entity is EntityLayer.Manutenzione_AttrezzaturaLudica)
            //{
            //    return ((EntityLayer.Manutenzione_AttrezzaturaLudica)entity).IDManutenzione.ToString();
            //}

            //else if (entity is EntityLayer.Manutenzione_Soggetto)
            //{
            //    return ((EntityLayer.Manutenzione_Soggetto)entity).IDManutenzione.ToString();
            //}

            //else if (entity is EntityLayer.Sopralluogo)
            //{
            //    return ((EntityLayer.Sopralluogo)entity).IDAttrezzaturaLudica.ToString();
            //}

            //else
            //{
            return entity.ID.ToString();
            //}
        }


        /// <summary>
        /// Restituisce un valore stringa contenente i dati informativi rappresentativi dell'entity passata
        /// quindi una stringa che permetta all'utente di capire che entità si sta trattando
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public string GetInformazioneEntity(object entity)
        {
            //if (entity is EntityLayer.Segnalazione_Utente)
            //{
            //    EntityLayer.Segnalazione_Utente segUte = (EntityLayer.Segnalazione_Utente)entity;
            //    string nomeUtente = "";
            //    if (segUte.Utente != null)
            //    {
            //        nomeUtente = segUte.Utente.Nome_Cognome;
            //    }
            //    else
            //    {
            //        Utenti llU = new Utenti(this.context);
            //        EntityLayer.Utente utente = llU.Find(segUte.IDUtente);
            //        if (utente != null)
            //        {
            //            nomeUtente = utente.Nome_Cognome;
            //        }
            //        else
            //        {
            //            nomeUtente = segUte.IDUtente.ToString();
            //        }
            //    }
            //    return string.Concat("Utente della Segnalazione: ", nomeUtente);
            //}



            //else if (entity is EntityLayer.Segnalazione_Utente_Nota)
            //{
            //    return string.Concat("Nota Utente: ", ((EntityLayer.Segnalazione_Utente_Nota)entity).Note);
            //}



            //else if (entity is EntityLayer.Manutenzione_AttrezzaturaLudica)
            //{
            //    EntityLayer.Manutenzione_AttrezzaturaLudica manAtt = (EntityLayer.Manutenzione_AttrezzaturaLudica)entity;
            //    string descAttrezzatura = "";
            //    if (manAtt.AttrezzaturaLudica != null)
            //    {
            //        descAttrezzatura = manAtt.AttrezzaturaLudica.DescrizioneCompleta;
            //    }
            //    else
            //    {
            //        AttrezzatureLudiche llA = new AttrezzatureLudiche(this.context);
            //        EntityLayer.AttrezzaturaLudica attrezzatura = llA.Find(manAtt.IDAttrezzaturaLudica);
            //        if (attrezzatura != null)
            //        {
            //            descAttrezzatura = attrezzatura.DescrizioneCompleta;
            //        }
            //        else
            //        {
            //            descAttrezzatura = manAtt.IDAttrezzaturaLudica.ToString();
            //        }
            //    }
            //    return string.Concat("Riferimento Attrezzatura Ludica: ", descAttrezzatura);
            //}



            //else if (entity is EntityLayer.Manutenzione_Segnalazione)
            //{
            //    EntityLayer.Manutenzione_Segnalazione manSeg = (EntityLayer.Manutenzione_Segnalazione)entity;
            //    string descSegnalazione = "";
            //    if (manSeg.Segnalazione != null)
            //    {
            //        descSegnalazione = manSeg.Segnalazione.DescrizioneCompleta;
            //    }
            //    else
            //    {
            //        Segnalazioni llS = new Segnalazioni(this.context);
            //        EntityLayer.Segnalazione segnalazione = llS.Find(manSeg.IDSegnalazione);
            //        if (segnalazione != null)
            //        {
            //            descSegnalazione = segnalazione.DescrizioneCompleta;
            //        }
            //        else
            //        {
            //            descSegnalazione = manSeg.IDSegnalazione.ToString();
            //        }
            //    }
            //    return string.Concat("Riferimento Segnalazione: ", descSegnalazione);
            //}



            //else if (entity is EntityLayer.Manutenzione_Soggetto)
            //{
            //    EntityLayer.Manutenzione_Soggetto manSog = (EntityLayer.Manutenzione_Soggetto)entity;
            //    string descSoggetto = "";
            //    if (manSog.Soggetto != null)
            //    {
            //        descSoggetto = manSog.Soggetto.Denominazione;
            //    }
            //    else
            //    {
            //        Soggetti llS = new Soggetti(this.context);
            //        EntityLayer.Soggetto soggetto = llS.Find(manSog.IDSoggetto);
            //        if (soggetto != null)
            //        {
            //            descSoggetto = soggetto.Denominazione;
            //        }
            //        else
            //        {
            //            descSoggetto = manSog.IDSoggetto.ToString();
            //        }
            //    }
            //    return string.Concat("Riferimento Manutentore: ", descSoggetto);
            //}



            //else if (entity is EntityLayer.Indirizzo)
            //{
            //    return string.Concat("Indirizzo: ", ((EntityLayer.Indirizzo)entity).IndirizzoCompleto);
            //}



            //else if (entity is EntityLayer.Sopralluogo)
            //{
            //    return string.Concat("Sopralluogo: ", ((EntityLayer.Sopralluogo)entity).DescrizioneCompleta);
            //}

            //else
            //{
            return string.Empty;
            //}
        }
    }
}
