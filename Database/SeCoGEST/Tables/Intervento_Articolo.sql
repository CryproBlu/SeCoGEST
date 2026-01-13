CREATE TABLE [SeCoGEST].[Intervento_Articolo] (
    [ID]                UNIQUEIDENTIFIER NOT NULL,
    [IDIntervento]      UNIQUEIDENTIFIER NOT NULL,
    [TipologiaArticolo] INT              NOT NULL,
    [Progressivo]       DECIMAL (18)     NULL,
    [CodiceCliente]     VARCHAR (7)      NOT NULL,
    [CodiceContratto]   VARCHAR (25)     NULL,
    [CodiceArticolo]    VARCHAR (50)     NULL,
    [Descrizione]       VARCHAR (500)    NULL,
    [PrezzoUnitario]    DECIMAL (18, 3)  NULL,
    [Quantita]          INT              NULL,
    [OreTime]           NVARCHAR (10)    NULL,
    [Ore]               DECIMAL (20, 8)  NULL,
    [DaFatturare]       BIT              NULL,
    [Note]              NVARCHAR (500)   NULL,
    CONSTRAINT [PK_Intervento_Articolo] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Intervento_Articolo_Intervento] FOREIGN KEY ([IDIntervento]) REFERENCES [SeCoGEST].[Intervento] ([ID]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_Intervento_Articolo]
    ON [SeCoGEST].[Intervento_Articolo]([IDIntervento] ASC);

