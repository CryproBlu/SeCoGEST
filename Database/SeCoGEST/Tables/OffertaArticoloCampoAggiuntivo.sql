CREATE TABLE [SeCoGEST].[OffertaArticoloCampoAggiuntivo] (
    [ID]                 UNIQUEIDENTIFIER NOT NULL,
    [IDOffertaArticolo]  UNIQUEIDENTIFIER NOT NULL,
    [Ordine]             INT              NOT NULL,
    [NomeCampo]          NVARCHAR (50)    NOT NULL,
    [TipoCampo]          NVARCHAR (50)    NOT NULL,
    [Valore]             NVARCHAR (500)   NOT NULL,
    [FonteEsternaValori] NVARCHAR (50)    NULL,
    CONSTRAINT [PK_OffertaArticoloCampoAggiuntivo] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_OffertaArticoloCampoAggiuntivo_OffertaArticolo] FOREIGN KEY ([IDOffertaArticolo]) REFERENCES [SeCoGEST].[OffertaArticolo] ([ID]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_OffertaArticoloCampoAggiuntivo]
    ON [SeCoGEST].[OffertaArticoloCampoAggiuntivo]([IDOffertaArticolo] ASC);

