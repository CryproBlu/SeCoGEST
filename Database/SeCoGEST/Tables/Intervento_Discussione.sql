CREATE TABLE [SeCoGEST].[Intervento_Discussione] (
    [ID]           UNIQUEIDENTIFIER NOT NULL,
    [IDIntervento] UNIQUEIDENTIFIER NOT NULL,
    [IDAccount]    UNIQUEIDENTIFIER NULL,
    [DataCommento] DATETIME         NOT NULL,
    [Commento]     NVARCHAR (4000)  NOT NULL,
    CONSTRAINT [PK_Intervento_Discussione] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Intervento_Discussione_Account] FOREIGN KEY ([IDAccount]) REFERENCES [SeCoGEST].[Account] ([ID]),
    CONSTRAINT [FK_Intervento_Discussione_Intervento] FOREIGN KEY ([IDIntervento]) REFERENCES [SeCoGEST].[Intervento] ([ID])
);


GO
CREATE NONCLUSTERED INDEX [IX_Intervento_Discussione_2]
    ON [SeCoGEST].[Intervento_Discussione]([DataCommento] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Intervento_Discussione_1]
    ON [SeCoGEST].[Intervento_Discussione]([IDAccount] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Intervento_Discussione]
    ON [SeCoGEST].[Intervento_Discussione]([IDIntervento] ASC);

