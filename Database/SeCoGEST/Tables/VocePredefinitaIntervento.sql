CREATE TABLE [SeCoGEST].[VocePredefinitaIntervento] (
    [ID]                   INT           IDENTITY (1, 1) NOT NULL,
    [Categoria]            VARCHAR (250) NOT NULL,
    [SottoCategoria]       VARCHAR (250) NULL,
    [Abstract]             VARCHAR (250) NOT NULL,
    [Descrizione]          VARCHAR (MAX) NOT NULL,
    [OrdineCategoria]      INT           NOT NULL,
    [OrdineSottoCategoria] INT           NOT NULL,
    CONSTRAINT [PK_VociPredefiniteIntervento] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_VociPredefiniteIntervento_Descrizione]
    ON [SeCoGEST].[VocePredefinitaIntervento]([Abstract] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_VociPredefiniteIntervento_Categoria_SottoCategoria]
    ON [SeCoGEST].[VocePredefinitaIntervento]([Categoria] ASC, [SottoCategoria] ASC);

