CREATE TABLE [SeCoGEST].[AnalisiCostoArticoloCampoAggiuntivo] (
    [ID]                     UNIQUEIDENTIFIER NOT NULL,
    [IDAnalisiCostoArticolo] UNIQUEIDENTIFIER NOT NULL,
    [Ordine]                 INT              NOT NULL,
    [NomeCampo]              NVARCHAR (50)    NOT NULL,
    [TipoCampo]              NVARCHAR (50)    NOT NULL,
    [Valore]                 NVARCHAR (500)   NOT NULL,
    [FonteEsternaValori]     NVARCHAR (50)    NULL,
    CONSTRAINT [PK_AnalisiCostoArticoloCampoAggiuntivo] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_AnalisiCostoArticoloCampoAggiuntivo_AnalisiCostoArticolo] FOREIGN KEY ([IDAnalisiCostoArticolo]) REFERENCES [SeCoGEST].[AnalisiCostoArticolo] ([ID])
);




GO
CREATE NONCLUSTERED INDEX [IX_AnalisiCostoArticoloCampoAggiuntivo]
    ON [SeCoGEST].[AnalisiCostoArticoloCampoAggiuntivo]([IDAnalisiCostoArticolo] ASC);

