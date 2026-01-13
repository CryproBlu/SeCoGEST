CREATE TABLE [SeCoGEST].[Progetto_Operatore] (
    [ID]          UNIQUEIDENTIFIER NOT NULL,
    [IDProgetto]  UNIQUEIDENTIFIER NOT NULL,
    [IDOperatore] UNIQUEIDENTIFIER NOT NULL,
    [Ruolo]       SMALLINT         NOT NULL,
    CONSTRAINT [PK_Progetto_Operatore] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Progetto_Operatore_Operatore] FOREIGN KEY ([IDOperatore]) REFERENCES [SeCoGEST].[Operatore] ([ID]),
    CONSTRAINT [FK_Progetto_Operatore_Progetto] FOREIGN KEY ([IDProgetto]) REFERENCES [SeCoGEST].[Progetto] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_Progetto_Operatore_1]
    ON [SeCoGEST].[Progetto_Operatore]([IDOperatore] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Progetto_Operatore]
    ON [SeCoGEST].[Progetto_Operatore]([IDProgetto] ASC);

