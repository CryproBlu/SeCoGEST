CREATE TABLE [SeCoGEST].[AccountOperatore] (
    [IDAccount]      UNIQUEIDENTIFIER NOT NULL,
    [IDOperatore]    UNIQUEIDENTIFIER NOT NULL,
    [InviaNotifiche] BIT              NULL,
    CONSTRAINT [PK_AccountOperatore] PRIMARY KEY CLUSTERED ([IDAccount] ASC, [IDOperatore] ASC),
    CONSTRAINT [FK_AccountOperatore_Account] FOREIGN KEY ([IDAccount]) REFERENCES [SeCoGEST].[Account] ([ID]),
    CONSTRAINT [FK_AccountOperatore_Operatore] FOREIGN KEY ([IDOperatore]) REFERENCES [SeCoGEST].[Operatore] ([ID])
);



