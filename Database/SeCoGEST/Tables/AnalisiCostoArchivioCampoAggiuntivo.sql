CREATE TABLE [SeCoGEST].[AnalisiCostoArchivioCampoAggiuntivo] (
    [ID]                        UNIQUEIDENTIFIER NOT NULL,
    [CodiceGruppo]              DECIMAL (5)      NULL,
    [CodiceCategoria]           DECIMAL (5)      NULL,
    [CodiceCategoriaStatistica] DECIMAL (5)      NULL,
    [Gruppo]                    NVARCHAR (50)    NULL,
    [Categoria]                 NVARCHAR (50)    NULL,
    [CategoriaStatistica]       NVARCHAR (50)    NULL,
    [Ordine]                    INT              NOT NULL,
    [NomeCampo]                 NVARCHAR (50)    NOT NULL,
    [TipoCampo]                 NVARCHAR (50)    NOT NULL,
    [Valore]                    NVARCHAR (500)   NULL,
    [ValorePredefinito]         NVARCHAR (500)   NULL,
    [FonteEsternaValori]        NVARCHAR (50)    NULL,
    CONSTRAINT [PK_AnalisiCostoArchivioCampoAggiuntivo] PRIMARY KEY CLUSTERED ([ID] ASC)
);





