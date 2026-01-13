CREATE TABLE [SeCoGEST].[AnalisiCostoArticolo] (
    [ID]                        UNIQUEIDENTIFIER NOT NULL,
    [IDRaggruppamento]          UNIQUEIDENTIFIER NULL,
    [IDAnalisiVendita]          UNIQUEIDENTIFIER NULL,
    [Ordine]                    INT              NOT NULL,
    [CodiceGruppo]              DECIMAL (5)      NULL,
    [CodiceCategoria]           DECIMAL (5)      NULL,
    [CodiceCategoriaStatistica] DECIMAL (5)      NULL,
    [Gruppo]                    NVARCHAR (50)    NULL,
    [Categoria]                 NVARCHAR (50)    NULL,
    [CategoriaStatistica]       NVARCHAR (50)    NULL,
    [CodiceArticolo]            VARCHAR (50)     NULL,
    [Descrizione]               NVARCHAR (250)   NOT NULL,
    [UnitaMisura]               NVARCHAR (10)    NULL,
    [Costo]                     MONEY            NULL,
    [Vendita]                   MONEY            NULL,
    [Quantita]                  DECIMAL (18, 2)  NULL,
    [TotaleCosto]               MONEY            NULL,
    [RicaricoValuta]            MONEY            NULL,
    [RicaricoPercentuale]       DECIMAL (18)     NULL,
    [TotaleVendita]             MONEY            NULL,
    [ContieneCampiAggiuntivi]   AS               ([SeCoGEST].[IsArticoloConCampiAggiuntivi]([ID])),
    CONSTRAINT [PK_AnalisiCostoArticolo] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_AnalisiCostoArticolo_AnalisiCostoRaggruppamento] FOREIGN KEY ([IDRaggruppamento]) REFERENCES [SeCoGEST].[AnalisiCostoRaggruppamento] ([ID]),
    CONSTRAINT [FK_AnalisiCostoArticolo_AnalisiVendita] FOREIGN KEY ([IDAnalisiVendita]) REFERENCES [SeCoGEST].[AnalisiVendita] ([ID])
);






GO
CREATE NONCLUSTERED INDEX [IX_AnalisiCostoArticolo_1]
    ON [SeCoGEST].[AnalisiCostoArticolo]([IDAnalisiVendita] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_AnalisiCostoArticolo]
    ON [SeCoGEST].[AnalisiCostoArticolo]([IDRaggruppamento] ASC);

