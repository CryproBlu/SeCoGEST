CREATE TABLE [SeCoGEST].[AnalisiCosto] (
    [ID]                        UNIQUEIDENTIFIER NOT NULL,
    [Numero]                    INT              NOT NULL,
    [NumeroRevisione]           INT              NOT NULL,
    [Data]                      DATE             NOT NULL,
    [Titolo]                    NVARCHAR (50)    NOT NULL,
    [TotaleCosto]               MONEY            NULL,
    [TotaleVenditaCalcolato]    MONEY            NULL,
    [TotaleRicaricoValuta]      MONEY            NULL,
    [TotaleRicaricoPercentuale] DECIMAL (18)     NULL,
    [TotaleVendita]             MONEY            NULL,
    [Chiuso]                    BIT              NULL,
    CONSTRAINT [PK_AnalisiCosto] PRIMARY KEY CLUSTERED ([ID] ASC)
);





