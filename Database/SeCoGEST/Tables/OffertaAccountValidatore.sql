CREATE TABLE [SeCoGEST].[OffertaAccountValidatore] (
    [IDOfferta]             UNIQUEIDENTIFIER NOT NULL,
    [IDAccount]             UNIQUEIDENTIFIER NOT NULL,
    [EffettuatoValidazione] BIT              CONSTRAINT [DF_OffertaAccountValidatore_EffettuatoValidazione] DEFAULT ((0)) NOT NULL,
    [DataOraValidazione]    DATETIME         NULL,
    CONSTRAINT [PK_OffertaAccountValidatore] PRIMARY KEY CLUSTERED ([IDOfferta] ASC, [IDAccount] ASC),
    CONSTRAINT [FK_OffertaAccountValidatore_Account] FOREIGN KEY ([IDAccount]) REFERENCES [SeCoGEST].[Account] ([ID]) ON DELETE CASCADE,
    CONSTRAINT [FK_OffertaAccountValidatore_Offerta] FOREIGN KEY ([IDOfferta]) REFERENCES [SeCoGEST].[Offerta] ([ID]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_OffertaAccountValidatore_IDAccount]
    ON [SeCoGEST].[OffertaAccountValidatore]([IDAccount] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_OffertaAccountValidatore_IDOfferta]
    ON [SeCoGEST].[OffertaAccountValidatore]([IDOfferta] ASC);

