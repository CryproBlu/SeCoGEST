CREATE TABLE [SeCoGEST].[AnalisiVenditaConfigurazioneArticoloAggiuntivo] (
    [ID]                           UNIQUEIDENTIFIER NOT NULL,
    [Tipologia]                    TINYINT          NOT NULL,
    [CodiceArticoloIn]             VARCHAR (50)     NULL,
    [CodiceGruppoIn]               DECIMAL (5)      NULL,
    [CodiceCategoriaIn]            DECIMAL (5)      NULL,
    [CodiceCategoriaStatisticaIn]  DECIMAL (5)      NULL,
    [GruppoIn]                     NVARCHAR (50)    NULL,
    [CategoriaIn]                  NVARCHAR (50)    NULL,
    [CategoriaStatisticaIn]        NVARCHAR (50)    NULL,
    [TestoAvviso]                  NVARCHAR (MAX)   NULL,
    [CodiceArticoloOut]            VARCHAR (50)     NULL,
    [ClausoleVessatorieAggiuntive] NVARCHAR (MAX)   NULL,
    [TestoDescrizionePreventivo]   NVARCHAR (MAX)   NULL,
    [TestoDescrizioneContratto]    NVARCHAR (MAX)   NULL,
    [UnitaMisura]                  NVARCHAR (10)    NULL,
    [Costo]                        MONEY            NULL,
    [Vendita]                      MONEY            NULL,
    [Quantita]                     DECIMAL (18, 2)  NULL,
    [TotaleCosto]                  MONEY            NULL,
    [RicaricoValuta]               MONEY            NULL,
    [RicaricoPercentuale]          DECIMAL (18, 2)  NULL,
    [TotaleVendita]                MONEY            NULL,
    CONSTRAINT [PK_AnalisiVenditaConfigurazioneArticoloAggiuntivo] PRIMARY KEY CLUSTERED ([ID] ASC)
);





