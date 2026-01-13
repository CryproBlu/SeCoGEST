
namespace SeCoGEST.Entities
{
    public class AnagraficaDestinazioneMerce
    {
        public AnagraficaDestinazioneMerce(Entities.AnagraficaClienti cliente)
        {
            this.CodiceCliente = cliente.CODCONTO;
            this.CodiceDestinazione = 0;
            this.RagioneSociale = cliente.DSCCONTO1 + cliente.DSCCONTO2;
            this.Indirizzo = cliente.INDIRIZZO;
            this.CAP = cliente.CAP;
            this.Localita = cliente.LOCALITA;
            this.Provincia = cliente.PROVINCIA;
            this.Telefono = cliente.TELEFONO;
        }
        public AnagraficaDestinazioneMerce(Entities.DESTINAZIONIDIVERSE destinazioneDiversa)
        {
            this.CodiceCliente = destinazioneDiversa.CODCONTO;
            this.CodiceDestinazione = destinazioneDiversa.CODICE;
            this.RagioneSociale = destinazioneDiversa.RAGIONESOCIALE;
            this.Indirizzo = destinazioneDiversa.INDIRIZZO;
            this.CAP = destinazioneDiversa.CAP;
            this.Localita = destinazioneDiversa.LOCALITA;
            this.Provincia = destinazioneDiversa.PROVINCIA;
            this.Telefono = destinazioneDiversa.TELEFONO;
        }

        public string CodiceCliente { get; set; }
        public decimal CodiceDestinazione { get; set; }
        public string RagioneSociale { get; set; }
        public string Indirizzo { get; set; }
        public string CAP { get; set; }
        public string Localita { get; set; }
        public string Provincia { get; set; }
        public string Telefono { get; set; }
    }
}
