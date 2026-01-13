CREATE TABLE [SeCoGEST].[MappaturaGruppoCategoriaCategoriaStatistica] (
    [ID]                             INT           IDENTITY (1, 1) NOT NULL,
    [CodiceGruppo]                   DECIMAL (5)   NOT NULL,
    [DescrizioneGruppo]              NVARCHAR (80) NOT NULL,
    [CodiceCategoria]                DECIMAL (5)   NOT NULL,
    [DescrizioneCategoria]           NVARCHAR (80) NOT NULL,
    [CodiceCategoriaStatistica]      DECIMAL (5)   NOT NULL,
    [DescrizioneCategoriaStatistica] NVARCHAR (80) NOT NULL,
    CONSTRAINT [PK_MappaturaGruppoCategoriaCategoriaStatistica] PRIMARY KEY CLUSTERED ([ID] ASC)
);

