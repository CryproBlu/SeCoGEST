namespace SeCoGEST.Entities
{
    public partial class OrarioRepartoUfficio
    {
        public string NomeDelGiorno
        {
            get
            {
                return System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.DayNames[this.Giorno];
            }
        }
    }
}
