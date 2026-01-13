CREATE TABLE [SeCoGEST].[Operatore] (
    [ID]                UNIQUEIDENTIFIER NOT NULL,
    [Cognome]           NVARCHAR (50)    NOT NULL,
    [Nome]              NVARCHAR (50)    NOT NULL,
    [CognomeNome]       AS               (([Cognome]+' ')+[Nome]),
    [Area]              BIT              CONSTRAINT [DF_Operatore_Area] DEFAULT ((0)) NOT NULL,
    [Attivo]            BIT              NOT NULL,
    [EmailResponsabile] NVARCHAR (500)   NULL,
    CONSTRAINT [PK_Operatore] PRIMARY KEY CLUSTERED ([ID] ASC)
);






















GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Operatore]
    ON [SeCoGEST].[Operatore]([Cognome] ASC, [Nome] ASC);

