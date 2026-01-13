CREATE TABLE [SeCoGEST].[ModalitaRisoluzioneIntervento] (
    [ID]          INT           NOT NULL,
    [Descrizione] NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_ModalitaRisoluzioneIntervento] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [IX_ModalitaRisoluzioneIntervento] UNIQUE NONCLUSTERED ([Descrizione] ASC)
);

