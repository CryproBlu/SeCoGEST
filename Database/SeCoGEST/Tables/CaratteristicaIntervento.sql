CREATE TABLE [SeCoGEST].[CaratteristicaIntervento] (
    [Id]   INT            NOT NULL,
    [Nome] NVARCHAR (100) NOT NULL,
    CONSTRAINT [PK_CaratteristicaIntervento] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_CaratteristicaIntervento]
    ON [SeCoGEST].[CaratteristicaIntervento]([Nome] ASC);

