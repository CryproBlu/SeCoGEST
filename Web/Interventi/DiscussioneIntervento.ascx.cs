using SeCoGEST.Entities;
using SeCoGEST.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SeCoGEST.Web.Interventi
{
    public partial class DiscussioneIntervento : System.Web.UI.UserControl
    {
        Logic.Allegati logicAllegati = null;
        Logic.Allegati LogicAllegati
        {
            get
            {
                if(logicAllegati == null) logicAllegati=new Logic.Allegati();
                return logicAllegati;
            }
        }

        public void ShowDiscussione(Entities.Intervento_Discussione discussione, bool primaDiscussione = false)
        {
            if(discussione.Account != null && discussione.Account.TipologiaEnum == Entities.TipologiaAccountEnum.SeCoGes)
            {
                imgOperator.AlternateText = "Supporto tecnico";
                imgOperator.ToolTip = "Supporto tecnico";
                imgOperator.ImageUrl = "/UI/Images/operator.png";
                lblEtichetta.Text = $"{discussione.DataCommento:f} - {discussione.Account.Nominativo}";
                rlDiscussione.BackColor = System.Drawing.ColorTranslator.FromHtml("#8FCDBE"); // System.Drawing.Color.LawnGreen;
            }
            else
            {
                imgOperator.AlternateText = "Utente";
                imgOperator.ToolTip = "Utente";
                imgOperator.ImageUrl = "/UI/Images/customer.png";
                lblEtichetta.Text = $"{discussione.DataCommento:f} - {discussione.Account.Nominativo}";
                rlDiscussione.BackColor = System.Drawing.ColorTranslator.FromHtml("#7799D1"); // System.Drawing.Color.Aquamarine;
            }

            if(primaDiscussione)
            {
                imgOperator.Visible = false;
                lblEtichetta.Text = $"<strong>Segnalazione</strong> - {discussione.DataCommento:f} - {discussione.Account.Nominativo}";
                rlDiscussione.BackColor = System.Drawing.Color.White;
                //rlDiscussione.Height = 250;
                rlDiscussione.Attributes.Add("style", "min-height: 100px;");
            }

            rlDiscussione.Text = discussione.Commento.Replace(Environment.NewLine, "<br/>");



            IQueryable<Entities.Allegato> allegatiDiscussione = LogicAllegati.Read(new EntityId<Entities.Intervento_Discussione>(discussione.ID));
            repAllegati.DataSource = allegatiDiscussione;
            repAllegati.DataBind();
            trAllegati.Visible = allegatiDiscussione.Any();
        }

        protected void repAllegati_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (Guid.TryParse(e.CommandArgument.ToString(), out Guid idAllegato))
            {
                Logic.Allegati llDocumenti = new Logic.Allegati();
                Entities.Allegato entityAllegato = llDocumenti.Find(idAllegato);
                if (entityAllegato != null)
                {
                    Helper.Web.DownloadAsFile(entityAllegato.NomeFile, entityAllegato.FileAllegato.ToArray());
                    //string filePath = Path.Combine(ConfigurationKeys.PERCORSO_TEMPORANEO, entityAllegato.NomeFile);
                    //File.WriteAllBytes(filePath, entityAllegato.FileAllegato.ToArray());
                    //if (File.Exists(filePath))
                    //{
                    //    Helper.Web.DownloadFile(filePath);
                    //}
                    //else
                    //{
                    //    throw new Exception(String.Format("Non è stato possibile recuperare il file '{0}'.", entityAllegato.NomeFile));
                    //}
                }
            }
        }
    }
}