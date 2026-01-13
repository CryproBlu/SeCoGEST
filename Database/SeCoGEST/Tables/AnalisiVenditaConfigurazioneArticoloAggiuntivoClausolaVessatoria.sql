CREATE TABLE [SeCoGEST].[AnalisiVenditaConfigurazioneArticoloAggiuntivoClausolaVessatoria] (
    [IDAnalisiVenditaConfigurazioneArticoloAggiuntivo] UNIQUEIDENTIFIER NOT NULL,
    [IDClausolaVessatoria]                             UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_AnalisiVenditaConfigurazioneArticoloAggiuntivoClausolaVessatoria] PRIMARY KEY CLUSTERED ([IDAnalisiVenditaConfigurazioneArticoloAggiuntivo] ASC, [IDClausolaVessatoria] ASC),
    CONSTRAINT [FK_AnalisiVenditaConfigurazioneArticoloAggiuntivoClausolaVessatoria_AnalisiVenditaConfigurazioneArticoloAggiuntivo] FOREIGN KEY ([IDAnalisiVenditaConfigurazioneArticoloAggiuntivo]) REFERENCES [SeCoGEST].[AnalisiVenditaConfigurazioneArticoloAggiuntivo] ([ID]),
    CONSTRAINT [FK_AnalisiVenditaConfigurazioneArticoloAggiuntivoClausolaVessatoria_ClausolaVessatoria] FOREIGN KEY ([IDClausolaVessatoria]) REFERENCES [SeCoGEST].[ClausolaVessatoria] ([ID])
);

