using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeCoGEST.Entities
{
    public partial class AnagraficaSoggetti_Proprieta
    {
        public AnagraficaSoggetti_ProprietaEnums? ValoreProprieta
        {
            get
            {
                //if (Enum.TryParse<AnagraficaSoggetti_ProprietaEnums>(value, out AnagraficaSoggetti_ProprietaEnums Proprieta))
                //{
                //    //Here you can use age
                //}
                if (Enum.IsDefined(typeof(AnagraficaSoggetti_ProprietaEnums), this.Valore))
                {
                    return (AnagraficaSoggetti_ProprietaEnums)Enum.Parse(typeof(AnagraficaSoggetti_ProprietaEnums), this.Valore);
                }
                else
                {
                    return null;
                }
            }
            set
            {
                this.Valore = value.ToString();
            }
        }
    }

    public enum AnagraficaSoggetti_ProprietaEnums
    {
        DefaultVisibilitaTicketCliente
    }
}
