CREATE TABLE [SeCoGEST].[ModelloConfigurazioneTicketCliente] (
    [Id]   UNIQUEIDENTIFIER NOT NULL,
    [Nome] NVARCHAR (150)   NOT NULL,
    CONSTRAINT [PK_ModelloConfigurazioneTicketCliente] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [IX_ModelloConfigurazioneTicketCliente] UNIQUE NONCLUSTERED ([Nome] ASC)
);

