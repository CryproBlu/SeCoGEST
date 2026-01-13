using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeCoGEST.Entities
{
    public enum FiltroInterventoEnum
    {
        [Description("Interventi con più di un Articolo")]
        InterventiConPiùDiUnArticolo = 1,

        [Description("Interventi con Modalità risoluzione diverse")]
        InterventiConModalitàRisoluzioneDiverse = 2,

        [Description("Interventi con Operatori di Aree Diverse")]
        InterventiConOperatoriAreeDiverse = 3,
    }
}

