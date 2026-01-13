CREATE TABLE [SeCoGEST].[LogInvioNotifica] (
    [ID]              INT              IDENTITY (1, 1) NOT NULL,
    [IDLegame]        UNIQUEIDENTIFIER NOT NULL,
    [IDTabellaLegame] TINYINT          NOT NULL,
    [IDNotifica]      TINYINT          NOT NULL,
    [Data]            DATETIME         NOT NULL,
    [Esito]           BIT              NOT NULL,
    [Note]            NVARCHAR (MAX)   NULL,
    CONSTRAINT [PK_LogInvioAlert] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_LogInvioNotifica_InfoOperazioneRecord_Tabella] FOREIGN KEY ([IDTabellaLegame]) REFERENCES [SeCoGEST].[InfoOperazioneRecord_Tabella] ([ID]),
    CONSTRAINT [FK_LogInvioNotifica_Notifica] FOREIGN KEY ([IDNotifica]) REFERENCES [SeCoGEST].[Notifica] ([ID])
);


GO
CREATE NONCLUSTERED INDEX [IX_LogInvioAlert_Data]
    ON [SeCoGEST].[LogInvioNotifica]([Data] DESC);

