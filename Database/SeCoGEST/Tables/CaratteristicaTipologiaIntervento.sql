CREATE TABLE [SeCoGEST].[CaratteristicaTipologiaIntervento] (
    [IdConfigurazione] UNIQUEIDENTIFIER NOT NULL,
    [IdCaratteristica] INT              NOT NULL,
    [Parametri]        NVARCHAR (4000)  NULL,
    CONSTRAINT [PK_CaratteristicaTipologiaIntervento] PRIMARY KEY CLUSTERED ([IdConfigurazione] ASC, [IdCaratteristica] ASC),
    CONSTRAINT [FK_CaratteristicaTipologiaIntervento_CaratteristicaIntervento] FOREIGN KEY ([IdCaratteristica]) REFERENCES [SeCoGEST].[CaratteristicaIntervento] ([Id]),
    CONSTRAINT [FK_CaratteristicaTipologiaIntervento_ConfigurazioneTipologiaTicketCliente] FOREIGN KEY ([IdConfigurazione]) REFERENCES [SeCoGEST].[ConfigurazioneTipologiaTicketCliente] ([Id]) ON DELETE CASCADE
);



