CREATE TABLE [SeCoGEST].[ServizioArticolo] (
    [IDServizio]               UNIQUEIDENTIFIER NOT NULL,
    [CodiceAnagraficaArticolo] VARCHAR (50)     NOT NULL,
    CONSTRAINT [PK_ServizioArticolo] PRIMARY KEY CLUSTERED ([IDServizio] ASC, [CodiceAnagraficaArticolo] ASC),
    CONSTRAINT [FK_ServizioArticolo_ANAGRAFICAARTICOLI] FOREIGN KEY ([CodiceAnagraficaArticolo]) REFERENCES [dbo].[ANAGRAFICAARTICOLI] ([CODICE]),
    CONSTRAINT [FK_ServizioArticolo_Servizio] FOREIGN KEY ([IDServizio]) REFERENCES [SeCoGEST].[Servizio] ([ID])
);





