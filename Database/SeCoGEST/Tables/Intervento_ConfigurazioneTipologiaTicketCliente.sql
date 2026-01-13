CREATE TABLE [SeCoGEST].[Intervento_ConfigurazioneTipologiaTicketCliente] (
    [Id]                 UNIQUEIDENTIFIER NOT NULL,
    [TipologiaArticolo]  INT              NOT NULL,
    [Progressivo]        DECIMAL (18)     NULL,
    [CodiceCliente]      VARCHAR (7)      NOT NULL,
    [CodiceContratto]    VARCHAR (25)     NULL,
    [CodiceArticolo]     VARCHAR (50)     NULL,
    [Descrizione]        NVARCHAR (500)   NOT NULL,
    [IdTipologia]        UNIQUEIDENTIFIER NOT NULL,
    [IdRepartoUfficio]   UNIQUEIDENTIFIER NOT NULL,
    [ScadenzaContratto]  DATETIME         NULL,
    [VisibilePerCliente] BIT              NULL,
    CONSTRAINT [PK_Intervento_ConfigurazioneTipologiaTicketCliente] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Intervento_ConfigurazioneTipologiaTicketCliente_Intervento] FOREIGN KEY ([Id]) REFERENCES [SeCoGEST].[Intervento] ([ID])
);

