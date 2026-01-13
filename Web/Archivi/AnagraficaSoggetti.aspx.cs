using SeCoGEST.Entities;
using SeCoGEST.Web.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SeCoGEST.Web.Archivi
{
    public partial class AnagraficaSoggetti : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Title = "Anagrafica Soggetti";
        }

        protected void rgArchiveItems_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            Logic.Metodo.AnagraficheClienti ll = new Logic.Metodo.AnagraficheClienti();
            Logic.AnagraficaSoggettiProprieta llT = new Logic.AnagraficaSoggettiProprieta(ll);

            var query = from clienti in ll.Read()
                        join props in llT.ReadByProperty(AnagraficaSoggetti_ProprietaEnums.DefaultVisibilitaTicketCliente) on clienti.CODCONTO equals props.CodiceCliente into gj
                        from subProps in gj.DefaultIfEmpty()
                        select new { 
                            Codice = clienti.CODCONTO,
                            RagioneSociale = string.Concat(clienti.DSCCONTO1, "", clienti.DSCCONTO2).Trim(),
                            Indirizzo = clienti.INDIRIZZO,
                            clienti.CAP,
                            Località = clienti.LOCALITA,
                            Provincia = clienti.PROVINCIA,
                            DefaultVisibilitaTicketCliente = subProps.Valore ?? String.Empty };

            rgArchiveItems.DataSource = query;
        }

        protected void rgArchiveItems_ItemCreated(object sender, GridItemEventArgs e)
        {
            //Traduce le voci del menu di filtro della griglia. Questa operazione deve essere fatta ad ogni post
            Helper.TelerikHelper.TraduciElementiGriglia(e);
        }

    }
}