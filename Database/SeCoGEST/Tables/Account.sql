CREATE TABLE [SeCoGEST].[Account] (
    [ID]                UNIQUEIDENTIFIER NOT NULL,
    [UserName]          NVARCHAR (50)    NOT NULL,
    [Password]          NVARCHAR (250)   NULL,
    [ScadenzaPassword]  DATE             NULL,
    [Nominativo]        NVARCHAR (50)    NOT NULL,
    [Email]             NVARCHAR (500)   NULL,
    [Amministratore]    BIT              NULL,
    [SolaLettura]       BIT              NULL,
    [Bloccato]          BIT              NULL,
    [UltimoAccesso]     SMALLDATETIME    NULL,
    [CodiceCliente]     VARCHAR (7)      NULL,
    [RagioneSociale]    VARCHAR (150)    NULL,
    [Tipologia]         TINYINT          CONSTRAINT [DF_Account_Tipologia] DEFAULT ((0)) NOT NULL,
    [IdDestinazione]    VARCHAR (7)      NULL,
    [DestinazioneMerce] VARCHAR (150)    NULL,
    [ValidatoreOfferta] BIT              CONSTRAINT [DF_Account_Validatore] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Account] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [IX_Account_1] UNIQUE NONCLUSTERED ([UserName] ASC)
);



