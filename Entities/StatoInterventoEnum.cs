using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeCoGEST.Entities
{
    public enum StatoInterventoEnum
    {
        [Description("Aperto")]
        Aperto = 0,

        [Description("In Gestione")]
        InGestione = 10,

        [Description("Eseguito")]
        Eseguito = 20, //1

        [Description("Chiuso")]
        Chiuso = 30, //2

        [Description("Validato")]
        Validato = 40, //3

        [Description("Sostituito")]
        Sostituito = 50 //4
    }


    // MODIFICHE DATABASE
    // TABELLA: Intervento_Stato, CAMPO DescrizioneStato
    //          Formula: (case when [Stato]=(0) then 'Aperto' when [Stato]=(10) then 'In Gestione' when [Stato]=(20) then 'Eseguito' when [Stato]=(30) then 'Chiuso' when [Stato]=(40) then 'Validato' when [Stato]=(50) then 'Sostituito' end)

    // AGGIORNAMENTO DATI
    // update SeCoGEST.Intervento_Stato set stato = 50 where stato = 4
    // update SeCoGEST.Intervento_Stato set stato = 40 where stato = 3
    // update SeCoGEST.Intervento_Stato set stato = 30 where stato = 2
    // update SeCoGEST.Intervento_Stato set stato = 20 where stato = 1

}

