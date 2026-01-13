CREATE TABLE [SeCoGEST].[OffertaArticolo] (
    [ID]                        UNIQUEIDENTIFIER NOT NULL,
    [IDRaggruppamento]          UNIQUEIDENTIFIER NULL,
    [Ordine]                    INT              NOT NULL,
    [CodiceGruppo]              DECIMAL (5)      NULL,
    [CodiceCategoria]           DECIMAL (5)      NULL,
    [CodiceCategoriaStatistica] DECIMAL (5)      NULL,
    [CodiceArticolo]            VARCHAR (50)     NULL,
    [Descrizione]               NVARCHAR (250)   NOT NULL,
    [UnitaMisura]               NVARCHAR (10)    NULL,
    [Costo]                     MONEY            NULL,
    [Vendita]                   MONEY            NULL,
    [Quantita]                  DECIMAL (18, 2)  NULL,
    [TotaleCosto]               MONEY            NULL,
    [RicaricoValuta]            MONEY            NULL,
    [RicaricoPercentuale]       DECIMAL (18, 2)  NULL,
    [TotaleVendita]             MONEY            NULL,
    [ContieneCampiAggiuntivi]   BIT              NULL,
    [IDArticoloPadre]           UNIQUEIDENTIFIER NULL,
    [TotaleSpesa]               MONEY            NULL,
    [TotaleVenditaConSpesa]     MONEY            NULL,
    CONSTRAINT [PK_OffertaArticolo] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_OffertaArticolo_OffertaArticolo] FOREIGN KEY ([IDArticoloPadre]) REFERENCES [SeCoGEST].[OffertaArticolo] ([ID]),
    CONSTRAINT [FK_OffertaArticolo_OffertaRaggruppamento] FOREIGN KEY ([IDRaggruppamento]) REFERENCES [SeCoGEST].[OffertaRaggruppamento] ([ID]) ON DELETE CASCADE
);




GO
CREATE NONCLUSTERED INDEX [IX_OffertaArticolo]
    ON [SeCoGEST].[OffertaArticolo]([IDRaggruppamento] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_OffertaArticolo_IDArticoloPadre]
    ON [SeCoGEST].[OffertaArticolo]([IDArticoloPadre] ASC) WHERE ([IDArticoloPadre] IS NOT NULL);

