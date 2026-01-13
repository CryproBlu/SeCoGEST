CREATE TABLE [SeCoGEST].[AnalisiVendita] (
    [ID]                        UNIQUEIDENTIFIER NOT NULL,
    [IDAnalisiCosto]            UNIQUEIDENTIFIER NULL,
    [IDRaggruppamento]          UNIQUEIDENTIFIER NULL,
    [IDArticolo]                UNIQUEIDENTIFIER NULL,
    [Progressivo]               INT              NOT NULL,
    [Data]                      DATETIME         NULL,
    [Tipologia]                 TINYINT          NOT NULL,
    [Descrizione]               NVARCHAR (50)    NULL,
    [TotaleCosto]               MONEY            NULL,
    [TotaleRicaricoValuta]      MONEY            NULL,
    [TotaleRicaricoPercentuale] DECIMAL (18, 2)  NULL,
    [TotaleVendita]             MONEY            NULL,
    CONSTRAINT [PK_AnalisiVendita] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_AnalisiVendita_AnalisiCosto] FOREIGN KEY ([IDAnalisiCosto]) REFERENCES [SeCoGEST].[AnalisiCosto] ([ID]),
    CONSTRAINT [FK_AnalisiVendita_AnalisiCostoArticolo] FOREIGN KEY ([IDArticolo]) REFERENCES [SeCoGEST].[AnalisiCostoArticolo] ([ID]),
    CONSTRAINT [FK_AnalisiVendita_AnalisiCostoRaggruppamento] FOREIGN KEY ([IDRaggruppamento]) REFERENCES [SeCoGEST].[AnalisiCostoRaggruppamento] ([ID])
);






GO
CREATE NONCLUSTERED INDEX [IX_AnalisiVendita_2]
    ON [SeCoGEST].[AnalisiVendita]([IDArticolo] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_AnalisiVendita_1]
    ON [SeCoGEST].[AnalisiVendita]([IDRaggruppamento] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_AnalisiVendita]
    ON [SeCoGEST].[AnalisiVendita]([IDAnalisiCosto] ASC);

