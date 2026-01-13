using System.ComponentModel;

namespace SeCoGEST.Logic.Sicurezza
{
    /// <summary>
    /// Definisce una serie di valori relativi ai possibili risultati di un tentativo di login
    /// </summary>
    public enum LoginResponseEnum
    {
        [Description("Accesso Consentito")]
        AccessoConsentito = 0,

        [Description("Utente non riconosciuto")]
        UtenteSconosciuto = 1,

        [Description("Utente Bloccato")]
        UtenteBloccato = 2,

        [Description("Password Scaduta")]
        PasswordScaduta = 3
    }
}
