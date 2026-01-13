CREATE TABLE [SeCoGEST].[ClausolaVessatoria] (
    [ID]          UNIQUEIDENTIFIER NOT NULL,
    [Codice]      NVARCHAR (50)    NOT NULL,
    [Descrizione] NVARCHAR (MAX)   NOT NULL,
    CONSTRAINT [PK_ClausolaVessatoria] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [UK_ClausolaVessatoria_Codice]
    ON [SeCoGEST].[ClausolaVessatoria]([Codice] ASC);

