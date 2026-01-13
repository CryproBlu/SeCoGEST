CREATE TABLE [SeCoGEST].[Progetto] (
    [ID]                 UNIQUEIDENTIFIER NOT NULL,
    [IDReferenteCliente] UNIQUEIDENTIFIER NOT NULL,
    [IDDPO]              UNIQUEIDENTIFIER NULL,
    [Numero]             INT              NOT NULL,
    [DataRedazione]      DATETIME         NOT NULL,
    [CodiceCliente]      VARCHAR (7)      NOT NULL,
    [RagioneSociale]     NVARCHAR (150)   NOT NULL,
    [IdDestinazione]     VARCHAR (7)      NULL,
    [DestinazioneMerce]  VARCHAR (150)    NULL,
    [Indirizzo]          VARCHAR (80)     NULL,
    [CAP]                VARCHAR (8)      NULL,
    [Localita]           VARCHAR (80)     NULL,
    [Provincia]          VARCHAR (4)      NULL,
    [Telefono]           VARCHAR (25)     NULL,
    [Titolo]             NVARCHAR (250)   NOT NULL,
    [Descrizione]        NVARCHAR (3000)  NULL,
    [NumeroCommessa]     NVARCHAR (20)    NULL,
    [CodiceContratto]    NVARCHAR (20)    NULL,
    [Stato]              TINYINT          NOT NULL,
    [Chiuso]             BIT              NOT NULL,
    [Note]               NVARCHAR (MAX)   NULL,
    CONSTRAINT [PK_Progetto] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Progetto_Operatore] FOREIGN KEY ([IDDPO]) REFERENCES [SeCoGEST].[Operatore] ([ID]),
    CONSTRAINT [FK_Progetto_Operatore1] FOREIGN KEY ([IDReferenteCliente]) REFERENCES [SeCoGEST].[Operatore] ([ID])
);


GO
CREATE NONCLUSTERED INDEX [IX_Progetto_1]
    ON [SeCoGEST].[Progetto]([IDReferenteCliente] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Progetto]
    ON [SeCoGEST].[Progetto]([IDDPO] ASC);

