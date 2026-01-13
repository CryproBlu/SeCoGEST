CREATE TABLE [SeCoGEST].[Intervento_Stato] (
    [ID]               UNIQUEIDENTIFIER NOT NULL,
    [IDIntervento]     UNIQUEIDENTIFIER NOT NULL,
    [Stato]            TINYINT          NOT NULL,
    [DescrizioneStato] AS               (case when [Stato]=(0) then 'Aperto' when [Stato]=(10) then 'In Gestione' when [Stato]=(20) then 'Eseguito' when [Stato]=(30) then 'Chiuso' when [Stato]=(40) then 'Validato' when [Stato]=(50) then 'Sostituito'  end) PERSISTED,
    [NomeUtente]       VARCHAR (50)     NOT NULL,
    [Data]             DATETIME         NOT NULL,
    CONSTRAINT [PK_Intervento_Stato] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Intervento_Stato_Intervento] FOREIGN KEY ([IDIntervento]) REFERENCES [SeCoGEST].[Intervento] ([ID]) ON DELETE CASCADE
);




GO
CREATE NONCLUSTERED INDEX [IX_Intervento_Stato]
    ON [SeCoGEST].[Intervento_Stato]([IDIntervento] ASC);

