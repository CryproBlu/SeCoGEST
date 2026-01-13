CREATE TABLE [SeCoGEST].[TipologiaIntervento] (
    [Id]                          UNIQUEIDENTIFIER NOT NULL,
    [Nome]                        NVARCHAR (100)   NOT NULL,
    [OreNotifica]                 INT              NULL,
    [VersioneModelloEsportazione] INT              NULL,
    CONSTRAINT [PK_TipologiaIntervento] PRIMARY KEY CLUSTERED ([Id] ASC)
);






GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_TipologiaIntervento]
    ON [SeCoGEST].[TipologiaIntervento]([Nome] ASC);

