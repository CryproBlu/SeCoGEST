CREATE TABLE [SeCoGEST].[Intervento_CaratteristicaTipologiaIntervento] (
    [IdIntervento]     UNIQUEIDENTIFIER NOT NULL,
    [IdCaratteristica] INT              NOT NULL,
    [Parametri]        NVARCHAR (4000)  NULL,
    [DataLimite]       SMALLDATETIME    NULL,
    CONSTRAINT [PK_Intervento_CaratteristicaTipologiaIntervento] PRIMARY KEY CLUSTERED ([IdIntervento] ASC, [IdCaratteristica] ASC),
    CONSTRAINT [FK_Intervento_CaratteristicaTipologiaIntervento_CaratteristicaIntervento] FOREIGN KEY ([IdCaratteristica]) REFERENCES [SeCoGEST].[CaratteristicaIntervento] ([Id]),
    CONSTRAINT [FK_Intervento_CaratteristicaTipologiaIntervento_Intervento] FOREIGN KEY ([IdIntervento]) REFERENCES [SeCoGEST].[Intervento] ([ID])
);

