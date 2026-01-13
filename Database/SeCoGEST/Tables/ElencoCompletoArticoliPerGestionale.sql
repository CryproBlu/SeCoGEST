CREATE TABLE [SeCoGEST].[ElencoCompletoArticoliPerGestionale] (
    [ID]                   VARCHAR (51)    NULL,
    [TipologiaArticolo]    INT             NOT NULL,
    [DescrizioneTipologia] VARCHAR (30)    NOT NULL,
    [Progressivo]          DECIMAL (18)    NULL,
    [CodiceContratto]      VARCHAR (25)    NULL,
    [CodiceCliente]        VARCHAR (7)     NOT NULL,
    [CodiceArticolo]       VARCHAR (50)    NOT NULL,
    [Descrizione]          VARCHAR (3000)  NULL,
    [OreFatte]             DECIMAL (19, 2) NULL,
    [OreIncluse]           DECIMAL (19, 2) NULL,
    [OreDaFare]            DECIMAL (19, 2) NULL,
    [Note]                 VARCHAR (3000)  NULL,
    [DataAttivazione]      DATETIME        NULL,
    [DataChiusura]         DATETIME        NULL
);

