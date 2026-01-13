CREATE TABLE [SeCoGEST].[Intervento] (
    [ID]                                UNIQUEIDENTIFIER NOT NULL,
    [Numero]                            INT              NOT NULL,
    [DataRedazione]                     DATETIME         NOT NULL,
    [DataPrevistaIntervento]            DATETIME         NULL,
    [CodiceCliente]                     VARCHAR (7)      NOT NULL,
    [RagioneSociale]                    VARCHAR (150)    NOT NULL,
    [IdDestinazione]                    VARCHAR (7)      NULL,
    [DestinazioneMerce]                 VARCHAR (150)    NULL,
    [Indirizzo]                         VARCHAR (80)     NULL,
    [CAP]                               VARCHAR (8)      NULL,
    [Localita]                          VARCHAR (80)     NULL,
    [Provincia]                         VARCHAR (4)      NULL,
    [Telefono]                          VARCHAR (25)     NULL,
    [Oggetto]                           NVARCHAR (250)   NOT NULL,
    [RichiesteProblematicheRiscontrate] NVARCHAR (MAX)   NULL,
    [Definizione]                       NVARCHAR (MAX)   NULL,
    [NumeroCommessa]                    NVARCHAR (20)    NULL,
    [IdTipologia]                       UNIQUEIDENTIFIER NULL,
    [Chiamata]                          BIT              NULL,
    [ReferenteChiamata]                 NVARCHAR (200)   NULL,
    [Urgente]                           BIT              NULL,
    [Interno]                           BIT              NULL,
    [VisibileAlCliente]                 BIT              CONSTRAINT [DF_Intervento_VisibileAlCliente] DEFAULT ((0)) NOT NULL,
    [Stato]                             AS               ([SeCoGEST].[GetStatoIntervento]([ID])),
    [DescrizioneStato]                  AS               ([SeCoGEST].[GetDescrizioneStatoIntervento]([ID])),
    [Note]                              NVARCHAR (2000)  NULL,
    [Chiuso]                            BIT              NULL,
    [TotaleMinuti]                      INT              NULL,
    [IDAccountUtenteRiferimento]        UNIQUEIDENTIFIER NULL,
    [IDInterventoSostitutivo]           UNIQUEIDENTIFIER NULL,
    [MotivazioneSostituzione]           NVARCHAR (500)   NULL,
    [IDConfigurazioneTipologiaTicket]   UNIQUEIDENTIFIER NULL,
    [DataNotifica]                      SMALLDATETIME    NULL,
    [TestoNotifica]                     NVARCHAR (500)   NULL,
    [Notificata]                        BIT              NULL,
    CONSTRAINT [PK_Intervento] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Intervento_ConfigurazioneTipologiaTicketCliente] FOREIGN KEY ([IDConfigurazioneTipologiaTicket]) REFERENCES [SeCoGEST].[ConfigurazioneTipologiaTicketCliente] ([Id]),
    CONSTRAINT [FK_Intervento_Intervento] FOREIGN KEY ([IDInterventoSostitutivo]) REFERENCES [SeCoGEST].[Intervento] ([ID]),
    CONSTRAINT [IX_Intervento] UNIQUE NONCLUSTERED ([Numero] ASC)
);












GO
CREATE NONCLUSTERED INDEX [IX_Intervento_1]
    ON [SeCoGEST].[Intervento]([IDInterventoSostitutivo] ASC);

