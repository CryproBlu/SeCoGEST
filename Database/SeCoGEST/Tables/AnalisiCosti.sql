CREATE TABLE [SeCoGEST].[AnalisiCosti] (
    [ID]                        UNIQUEIDENTIFIER NOT NULL,
    [NumeroRevisione]           INT              NOT NULL,
    [Data]                      DATE             NOT NULL,
    [Titolo]                    NVARCHAR (50)    NOT NULL,
    [TotaleCosto]               MONEY            NULL,
    [TotaleVenditaCalcolato]    MONEY            NULL,
    [TotaleRicaricoValuta]      MONEY            NULL,
    [TotaleRicaricoPercentuale] DECIMAL (18)     NULL,
    [TotaleVendita]             MONEY            NULL,
    CONSTRAINT [PK_AnalisiCosti] PRIMARY KEY CLUSTERED ([ID] ASC)
);

