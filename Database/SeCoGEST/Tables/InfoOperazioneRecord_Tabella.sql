CREATE TABLE [SeCoGEST].[InfoOperazioneRecord_Tabella] (
    [ID]          TINYINT       NOT NULL,
    [NomeTabella] NVARCHAR (60) NOT NULL,
    CONSTRAINT [PK_InfoOperazioneRecord_Tabella_ID] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Il valore di ID deve essere lo stesso del valore dell''enumeratore InfoOperazioneTabellaEnum', @level0type = N'SCHEMA', @level0name = N'SeCoGEST', @level1type = N'TABLE', @level1name = N'InfoOperazioneRecord_Tabella', @level2type = N'COLUMN', @level2name = N'NomeTabella';

