CREATE TABLE [SeCoGEST].[Servizio] (
    [ID]          UNIQUEIDENTIFIER NOT NULL,
    [Codice]      VARCHAR (20)     NOT NULL,
    [Nome]        VARCHAR (50)     NOT NULL,
    [Descrizione] VARCHAR (MAX)    NULL,
    CONSTRAINT [PK_Servizio] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [UK_Servizio_Codice] UNIQUE NONCLUSTERED ([Codice] ASC)
);

