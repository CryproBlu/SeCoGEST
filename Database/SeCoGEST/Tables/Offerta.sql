CREATE TABLE [SeCoGEST].[Offerta] (
    [ID]                                 UNIQUEIDENTIFIER NOT NULL,
    [Numero]                             INT              NOT NULL,
    [NumeroRevisione]                    INT              NOT NULL,
    [Data]                               DATETIME         NOT NULL,
    [Titolo]                             NVARCHAR (50)    NOT NULL,
    [CodiceCliente]                      VARCHAR (7)      NOT NULL,
    [RagioneSociale]                     VARCHAR (150)    NOT NULL,
    [DestinazioneMerce]                  VARCHAR (150)    NULL,
    [Indirizzo]                          VARCHAR (80)     NULL,
    [CAP]                                VARCHAR (8)      NULL,
    [Localita]                           VARCHAR (80)     NULL,
    [Provincia]                          VARCHAR (4)      NULL,
    [Telefono]                           VARCHAR (25)     NULL,
    [TotaleCosto]                        MONEY            NULL,
    [TotaleVenditaCalcolato]             MONEY            NULL,
    [TotaleRicaricoValuta]               MONEY            NULL,
    [TotaleRicaricoPercentuale]          DECIMAL (18, 2)  NULL,
    [TotaleVendita]                      MONEY            NULL,
    [Chiuso]                             BIT              NULL,
    [Stato]                              TINYINT          CONSTRAINT [DF_Offerta_Stato] DEFAULT ((0)) NOT NULL,
    [CodiceCommessa]                     NVARCHAR (50)    NULL,
    [TempiDiConsegna]                    INT              NULL,
    [NoteInterne]                        NVARCHAR (MAX)   NULL,
    [NoteRifiuto]                        NVARCHAR (MAX)   NULL,
    [TestoPieDiPagina]                   NVARCHAR (MAX)   NULL,
    [TestoIntestazione]                  NVARCHAR (MAX)   NULL,
    [TestoValiditaOfferta]               NVARCHAR (MAX)   NULL,
    [TestoEmailInviataAlCliente]         NVARCHAR (MAX)   NULL,
    [CodicePagamento]                    VARCHAR (4)      NULL,
    [DescrizionePagamento]               VARCHAR (80)     NULL,
    [CodiceIBAN]                         VARCHAR (34)     NULL,
    [TotaleCostoCalcolato]               MONEY            NULL,
    [TotaleRicaricoValutaCalcolato]      MONEY            NULL,
    [TotaleRicaricoPercentualeCalcolato] DECIMAL (18, 2)  NULL,
    [TotaliModificati]                   BIT              CONSTRAINT [DF_Offerta_TotaliModificati] DEFAULT ((0)) NOT NULL,
    [TestoSezionePagamenti]              NVARCHAR (MAX)   NULL,
    [TipologiaTempiDiConsegna]           TINYINT          NULL,
    [GiorniValidita]                     INT              NULL,
    [TipologiaGiorniValidita]            TINYINT          NULL,
    [TotaleSpesa]                        MONEY            NULL,
    [TotaleVenditaConSpesa]              MONEY            NULL,
    [TotaleSpesaCacolato]                DECIMAL (18, 2)  NULL,
    [TotaleVenditaConSpesaCacolato]      DECIMAL (18, 2)  NULL,
    CONSTRAINT [PK_Offerta] PRIMARY KEY CLUSTERED ([ID] ASC)
);




GO
CREATE NONCLUSTERED INDEX [IX_Offerta_1]
    ON [SeCoGEST].[Offerta]([CodiceCliente] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Offerta]
    ON [SeCoGEST].[Offerta]([Numero] ASC);

