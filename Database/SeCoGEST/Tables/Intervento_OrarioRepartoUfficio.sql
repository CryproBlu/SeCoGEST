CREATE TABLE [SeCoGEST].[Intervento_OrarioRepartoUfficio] (
    [Id]           UNIQUEIDENTIFIER NOT NULL,
    [IdIntervento] UNIQUEIDENTIFIER NOT NULL,
    [Giorno]       TINYINT          NOT NULL,
    [OrarioDalle]  TIME (7)         NOT NULL,
    [OrarioAlle]   TIME (7)         NOT NULL,
    CONSTRAINT [PK_Intervento_OrarioRepartoUfficio] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Intervento_OrarioRepartoUfficio_Intervento] FOREIGN KEY ([IdIntervento]) REFERENCES [SeCoGEST].[Intervento] ([ID])
);


GO
CREATE NONCLUSTERED INDEX [IX_Intervento_OrarioRepartoUfficio]
    ON [SeCoGEST].[Intervento_OrarioRepartoUfficio]([IdIntervento] ASC);

