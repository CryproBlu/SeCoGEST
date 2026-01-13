CREATE TABLE [SeCoGEST].[Allegato] (
    [ID]                UNIQUEIDENTIFIER NOT NULL,
    [IDLegame]          UNIQUEIDENTIFIER NOT NULL,
    [TipologiaAllegato] TINYINT          NOT NULL,
    [NomeFile]          NVARCHAR (255)   NOT NULL,
    [UserName]          NVARCHAR (50)    NOT NULL,
    [DataArchiviazione] DATETIME         NOT NULL,
    [Compresso]         BIT              NOT NULL,
    [FileAllegato]      VARBINARY (MAX)  NOT NULL,
    [Note]              VARCHAR (MAX)    NULL,
    CONSTRAINT [PK_Allegato] PRIMARY KEY CLUSTERED ([ID] ASC)
);

