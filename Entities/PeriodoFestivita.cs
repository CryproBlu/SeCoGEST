namespace SeCoGEST.Entities
{
    public partial class PeriodoFestivita
    {
        public string Descrizione
        {
            get
            {
                string gg = string.Empty;
                string mm = string.Empty;
                string aa = string.Empty;

                if (this.Giorno.HasValue)
                {
                    gg = $"{this.Giorno:00}";
                }

                if (this.Mese.HasValue)
                {
                    switch (this.Mese)
                    {
                        case 1: mm = "Gennaio"; break;
                        case 2: mm = "Febbraio"; break;
                        case 3: mm = "Marzo"; break;
                        case 4: mm = "Aprile"; break;
                        case 5: mm = "Maggio"; break;
                        case 6: mm = "Giugno"; break;
                        case 7: mm = "Luglio"; break;
                        case 8: mm = "Agosto"; break;
                        case 9: mm = "Settembre"; break;
                        case 10: mm = "Ottobre"; break;
                        case 11: mm = "Novembre"; break;
                        case 12: mm = "Dicembre"; break;
                    }
                }

                if (this.Anno.HasValue)
                {
                    aa = $"{this.Anno:0000}";
                }





                if (this.Giorno.HasValue && !this.Mese.HasValue && !this.Anno.HasValue)
                {
                    gg = $"Ogni Giorno {this.Giorno:00}";
                }

                if (!this.Giorno.HasValue && !this.Mese.HasValue && this.Anno.HasValue)
                {
                    aa = $"Anno {this.Anno:0000}";
                }



                string festività = string.IsNullOrEmpty(this.Festivita) ? string.Empty : this.Festivita + ": ";

                return $"{festività} {gg} {mm} {aa}".Trim();
            }
        }
    }    
}