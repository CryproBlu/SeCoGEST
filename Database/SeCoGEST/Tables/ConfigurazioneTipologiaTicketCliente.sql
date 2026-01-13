CREATE TABLE [SeCoGEST].[ConfigurazioneTipologiaTicketCliente] (
    [Id]                 UNIQUEIDENTIFIER NOT NULL,
    [TipologiaArticolo]  INT              NOT NULL,
    [Progressivo]        DECIMAL (18)     NULL,
    [CodiceCliente]      VARCHAR (7)      NOT NULL,
    [CodiceContratto]    VARCHAR (25)     NULL,
    [CodiceArticolo]     VARCHAR (50)     NULL,
    [Descrizione]        NVARCHAR (500)   NOT NULL,
    [IdTipologia]        UNIQUEIDENTIFIER NOT NULL,
    [IdRepartoUfficio]   UNIQUEIDENTIFIER NOT NULL,
    [IdCondizione]       INT              NULL,
    [ScadenzaContratto]  DATETIME         NULL,
    [VisibilePerCliente] BIT              NULL,
    [IdOperatore]        UNIQUEIDENTIFIER NULL,
    CONSTRAINT [PK_ConfigurazioneTipologiaTicketCliente] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ConfigurazioneTipologiaTicketCliente_CondizioneIntervento] FOREIGN KEY ([IdCondizione]) REFERENCES [SeCoGEST].[CondizioneIntervento] ([Id]),
    CONSTRAINT [FK_ConfigurazioneTipologiaTicketCliente_Operatore] FOREIGN KEY ([IdOperatore]) REFERENCES [SeCoGEST].[Operatore] ([ID]),
    CONSTRAINT [FK_ConfigurazioneTipologiaTicketCliente_RepartoUfficio] FOREIGN KEY ([IdRepartoUfficio]) REFERENCES [SeCoGEST].[RepartoUfficio] ([Id]),
    CONSTRAINT [FK_ConfigurazioneTipologiaTicketCliente_TipologiaIntervento] FOREIGN KEY ([IdTipologia]) REFERENCES [SeCoGEST].[TipologiaIntervento] ([Id])
);




GO
CREATE NONCLUSTERED INDEX [IX_ConfigurazioneTipologiaTicketCliente]
    ON [SeCoGEST].[ConfigurazioneTipologiaTicketCliente]([IdTipologia] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ConfigurazioneTipologiaTicketCliente_1]
    ON [SeCoGEST].[ConfigurazioneTipologiaTicketCliente]([IdOperatore] ASC);

