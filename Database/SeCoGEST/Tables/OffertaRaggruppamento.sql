CREATE TABLE [SeCoGEST].[OffertaRaggruppamento] (
    [ID]                                 UNIQUEIDENTIFIER NOT NULL,
    [IDOfferta]                          UNIQUEIDENTIFIER NOT NULL,
    [IDRaggruppamentoPadre]              UNIQUEIDENTIFIER NULL,
    [Ordine]                             INT              NOT NULL,
    [Denominazione]                      NVARCHAR (250)   NOT NULL,
    [TotaleCosto]                        MONEY            NULL,
    [TotaleVenditaCalcolato]             MONEY            NULL,
    [TotaleRicaricoValuta]               MONEY            NULL,
    [TotaleRicaricoPercentuale]          DECIMAL (18, 2)  NULL,
    [TotaleVendita]                      MONEY            NULL,
    [TotaleCostoCalcolato]               MONEY            NULL,
    [TotaleRicaricoValutaCalcolato]      MONEY            NULL,
    [TotaleRicaricoPercentualeCalcolato] DECIMAL (18, 2)  NULL,
    [TotaliModificati]                   BIT              CONSTRAINT [DF_OffertaRaggruppamento_TotaliModificati] DEFAULT ((0)) NOT NULL,
    [OpzioneStampaOfferta]               INT              CONSTRAINT [DF_OffertaRaggruppamento_OpzioneStampaOfferta] DEFAULT ((2147483647)) NOT NULL,
    [TotaleSpesa]                        MONEY            NULL,
    [TotaleVenditaConSpesa]              MONEY            NULL,
    [TotaleSpesaCacolato]                DECIMAL (18, 2)  NULL,
    [TotaleVenditaConSpesaCacolato]      DECIMAL (18, 2)  NULL,
    CONSTRAINT [PK_OffertaRaggruppamento] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_OffertaRaggruppamento_Offerta] FOREIGN KEY ([IDOfferta]) REFERENCES [SeCoGEST].[Offerta] ([ID]) ON DELETE CASCADE,
    CONSTRAINT [FK_OffertaRaggruppamento_OffertaRaggruppamento] FOREIGN KEY ([IDRaggruppamentoPadre]) REFERENCES [SeCoGEST].[OffertaRaggruppamento] ([ID])
);




GO
CREATE NONCLUSTERED INDEX [IX_OffertaRaggruppamento_1]
    ON [SeCoGEST].[OffertaRaggruppamento]([IDRaggruppamentoPadre] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_OffertaRaggruppamento]
    ON [SeCoGEST].[OffertaRaggruppamento]([IDOfferta] ASC);

