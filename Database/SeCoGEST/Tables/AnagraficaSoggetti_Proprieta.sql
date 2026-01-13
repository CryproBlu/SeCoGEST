CREATE TABLE [SeCoGEST].[AnagraficaSoggetti_Proprieta] (
    [CodiceCliente] VARCHAR (7)    NOT NULL,
    [Proprietà]     NVARCHAR (50)  NOT NULL,
    [Valore]        NVARCHAR (255) NOT NULL,
    CONSTRAINT [PK_AnagraficaSoggetti_Proprieta] PRIMARY KEY CLUSTERED ([CodiceCliente] ASC, [Proprietà] ASC)
);

