CREATE TABLE [SeCoGEST].[ModelloConfigurazioneCaratteristicheTicketCliente] (
    [IdModelloConfigurazioneTicketCliente] UNIQUEIDENTIFIER NOT NULL,
    [IdCaratteristica]                     INT              NOT NULL,
    [Parametri]                            NVARCHAR (4000)  NULL,
    CONSTRAINT [PK_ModelloConfigurazioneCaratteristicheTicketCliente_1] PRIMARY KEY CLUSTERED ([IdModelloConfigurazioneTicketCliente] ASC, [IdCaratteristica] ASC),
    CONSTRAINT [FK_ModelloConfigurazioneCaratteristicheTicketCliente_CaratteristicaIntervento] FOREIGN KEY ([IdCaratteristica]) REFERENCES [SeCoGEST].[CaratteristicaIntervento] ([Id]),
    CONSTRAINT [FK_ModelloConfigurazioneCaratteristicheTicketCliente_ModelloConfigurazioneTicketCliente] FOREIGN KEY ([IdModelloConfigurazioneTicketCliente]) REFERENCES [SeCoGEST].[ModelloConfigurazioneTicketCliente] ([Id]) ON DELETE CASCADE
);

