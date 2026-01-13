CREATE TABLE [SeCoGEST].[PeriodoFestivita] (
    [Id]        INT            IDENTITY (1, 1) NOT NULL,
    [Festivita] NVARCHAR (150) NULL,
    [Giorno]    INT            NULL,
    [Mese]      INT            NULL,
    [Anno]      INT            NULL,
    CONSTRAINT [PK_PeriodoFestivita] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [IX_PeriodoFestivita] UNIQUE NONCLUSTERED ([Giorno] ASC, [Mese] ASC, [Anno] ASC)
);

