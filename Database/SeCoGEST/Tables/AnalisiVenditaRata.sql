CREATE TABLE [SeCoGEST].[AnalisiVenditaRata] (
    [ID]               UNIQUEIDENTIFIER NOT NULL,
    [IDAnalisiVendita] UNIQUEIDENTIFIER NOT NULL,
    [Progressivo]      INT              NOT NULL,
    [Data]             DATE             NULL,
    [ImportoProposto]  MONEY            NULL,
    [ImportoDefinito]  MONEY            NULL,
    CONSTRAINT [PK_AnalisiVenditaRata] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_AnalisiVenditaRata_AnalisiVendita] FOREIGN KEY ([IDAnalisiVendita]) REFERENCES [SeCoGEST].[AnalisiVendita] ([ID])
);


GO
CREATE NONCLUSTERED INDEX [IX_AnalisiVenditaRata]
    ON [SeCoGEST].[AnalisiVenditaRata]([IDAnalisiVendita] ASC);

