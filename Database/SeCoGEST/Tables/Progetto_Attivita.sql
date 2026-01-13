CREATE TABLE [SeCoGEST].[Progetto_Attivita] (
    [ID]                   UNIQUEIDENTIFIER NOT NULL,
    [IDProgetto]           UNIQUEIDENTIFIER NOT NULL,
    [Ordine]               SMALLINT         NOT NULL,
    [Descrizione]          NVARCHAR (4000)  NOT NULL,
    [Scadenza]             DATETIME         NULL,
    [IDOperatoreAssegnato] UNIQUEIDENTIFIER NULL,
    [IDOperatoreEsecutore] UNIQUEIDENTIFIER NULL,
    [IDTicket]             UNIQUEIDENTIFIER NULL,
    [Stato]                TINYINT          NOT NULL,
    [NoteContratto]        NVARCHAR (2000)  NULL,
    [NoteOperatore]        NVARCHAR (2000)  NULL,
    CONSTRAINT [PK_Progetto_Attivita] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Progetto_Attivita_Intervento] FOREIGN KEY ([IDTicket]) REFERENCES [SeCoGEST].[Intervento] ([ID]),
    CONSTRAINT [FK_Progetto_Attivita_Operatore] FOREIGN KEY ([IDOperatoreAssegnato]) REFERENCES [SeCoGEST].[Operatore] ([ID]),
    CONSTRAINT [FK_Progetto_Attivita_Operatore1] FOREIGN KEY ([IDOperatoreEsecutore]) REFERENCES [SeCoGEST].[Operatore] ([ID]),
    CONSTRAINT [FK_Progetto_Attivita_Progetto] FOREIGN KEY ([IDProgetto]) REFERENCES [SeCoGEST].[Progetto] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE
);




GO
CREATE NONCLUSTERED INDEX [IX_Progetto_Attivita_3]
    ON [SeCoGEST].[Progetto_Attivita]([IDTicket] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Progetto_Attivita_2]
    ON [SeCoGEST].[Progetto_Attivita]([IDOperatoreAssegnato] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Progetto_Attivita_1]
    ON [SeCoGEST].[Progetto_Attivita]([IDOperatoreEsecutore] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Progetto_Attivita]
    ON [SeCoGEST].[Progetto_Attivita]([IDProgetto] ASC);

