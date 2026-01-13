using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SeCoGEST.Web
{
    internal static class GeneratoreDocumentiHelper
    {
        public const string NOME_PARAMETRO_IDENTIFICATORE_ENTITY = "ID";

        public const string NOME_PARAMETRO_TIPO_DOCUMENTO = "TD";

        public const string NOME_PARAMETRO_NOME_DOCUMENTO_PER_DOWNLOAD = "ND";

        /// <summary>
        /// Restituisce il percorso di generazione del documento in base ai parametri passati come parametro
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="identificatore"></param>
        /// <param name="tipoDocumento"></param>
        /// <param name="nomeDocumentoPerDownload">Nome con il quale dev'essere scaricato il documento</param>
        /// <returns></returns>
        public static string GetPercorsoGenerazioneDocumenti<T>(Entities.EntityId<T> identificatore, TipologiaDocumentiDaGenerareEnum tipoDocumento, string nomeDocumentoPerDownload)
            where T : class
        {
            if (identificatore == null) throw new ArgumentNullException("identificatore", "Parametro nullo");

            return String.Format("/{0}.aspx?{1}={2}&{3}={4}&{5}={6}", 
                                typeof(GeneratoreDocumenti).Name, 
                                NOME_PARAMETRO_IDENTIFICATORE_ENTITY,
                                identificatore.Value, 
                                NOME_PARAMETRO_TIPO_DOCUMENTO,
                                (int)tipoDocumento,
                                NOME_PARAMETRO_NOME_DOCUMENTO_PER_DOWNLOAD,
                                nomeDocumentoPerDownload);
        }
    }
}