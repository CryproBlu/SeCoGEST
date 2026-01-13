CREATE TABLE [SeCoGEST].[AnalisiCostoRaggruppamento] (
    [ID]                        UNIQUEIDENTIFIER NOT NULL,
    [IDAnalisiCosto]            UNIQUEIDENTIFIER NOT NULL,
    [IDRaggruppamentoPadre]     UNIQUEIDENTIFIER NULL,
    [Ordine]                    INT              NOT NULL,
    [Denominazione]             NVARCHAR (250)   NOT NULL,
    [TotaleCosto]               MONEY            NULL,
    [TotaleVenditaCalcolato]    MONEY            NULL,
    [TotaleRicaricoValuta]      MONEY            NULL,
    [TotaleRicaricoPercentuale] DECIMAL (18)     NULL,
    [TotaleVendita]             MONEY            NULL,
    CONSTRAINT [PK_AnalisiCostoRaggruppamento] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_AnalisiCostoRaggruppamento_AnalisiCosto] FOREIGN KEY ([IDAnalisiCosto]) REFERENCES [SeCoGEST].[AnalisiCosto] ([ID]),
    CONSTRAINT [FK_AnalisiCostoRaggruppamento_AnalisiCostoRaggruppamento] FOREIGN KEY ([IDRaggruppamentoPadre]) REFERENCES [SeCoGEST].[AnalisiCostoRaggruppamento] ([ID])
);




GO
CREATE NONCLUSTERED INDEX [IX_AnalisiCostoRaggruppamento_1]
    ON [SeCoGEST].[AnalisiCostoRaggruppamento]([IDRaggruppamentoPadre] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_AnalisiCostoRaggruppamento]
    ON [SeCoGEST].[AnalisiCostoRaggruppamento]([IDAnalisiCosto] ASC);

