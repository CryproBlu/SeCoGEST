CREATE TABLE [SeCoGEST].[OrarioRepartoUfficio] (
    [Id]               UNIQUEIDENTIFIER NOT NULL,
    [IdRepartoUfficio] UNIQUEIDENTIFIER NOT NULL,
    [Giorno]           TINYINT          NOT NULL,
    [OrarioDalle]      TIME (7)         NOT NULL,
    [OrarioAlle]       TIME (7)         NOT NULL,
    CONSTRAINT [PK_OrarioRepartoUfficio] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_OrarioRepartoUfficio_RepartoUfficio] FOREIGN KEY ([IdRepartoUfficio]) REFERENCES [SeCoGEST].[RepartoUfficio] ([Id]) ON DELETE CASCADE
);




GO
CREATE NONCLUSTERED INDEX [IX_OrarioRepartoUfficio]
    ON [SeCoGEST].[OrarioRepartoUfficio]([IdRepartoUfficio] ASC);

