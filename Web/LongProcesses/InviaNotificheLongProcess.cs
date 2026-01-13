using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SeCoGEST.Web.LongProcesses
{
    public class InviaNotificheLongProcess
    {
        /// <summary>
        /// Effettua l'invio dell'elenco di tutte le notifiche richieste dall'amministrazione
        /// </summary>
        /// <param name="parameters"></param>
        public void EffettuaInvioGlobale(object parameters)
        {
            Logic.MotoreInvioNotifiche llMotoreInvioNotifiche = new Logic.MotoreInvioNotifiche();
            llMotoreInvioNotifiche.InviaNotifichePerTicketsConConfigurazione();
            llMotoreInvioNotifiche.InviaNotificheSpecificatamenteRichieste();

            //Logic.MotoreInvioNotifiche2 llMotoreInvioNotifiche = new Logic.MotoreInvioNotifiche2(true);
            //llMotoreInvioNotifiche.AnalizzaNotificaDaInviare(Entities.TipologiaNotificaEnum.InterventoPresoInCaricoNonChiuso);
            //llMotoreInvioNotifiche.AnalizzaNotificaDaInviare(Entities.TipologiaNotificaEnum.InterventoApertoNonPresoInCarico);
            //llMotoreInvioNotifiche.AnalizzaNotificaDaInviare(Entities.TipologiaNotificaEnum.InterventoSenzaAlcunOperatoreAssegnato);
            //llMotoreInvioNotifiche.AnalizzaNotificaDaInviare(Entities.TipologiaNotificaEnum.InterventoDaNotificarePerSLA);
            //llMotoreInvioNotifiche.InviaNotificheTramiteEmail(true, true);
            //llMotoreInvioNotifiche.PulisciElencoNotificaDaInviare();

            //llMotoreInvioNotifiche.AnalizzaNotificaDaInviare(Entities.TipologiaNotificaEnum.InterventoDaNotificareInBaseSLA);
            //llMotoreInvioNotifiche.InviaNotificheTramiteEmail(true, true);
            //llMotoreInvioNotifiche.PulisciElencoNotificaDaInviare();
        }
    }
}