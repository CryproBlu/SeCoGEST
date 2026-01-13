CREATE TABLE [SeCoGEST].[InfoOperazioneRecord] (
    [ID]                  INT            IDENTITY (1, 1) NOT NULL,
    [IDLegame]            NVARCHAR (150) NOT NULL,
    [IDTabellaLegame]     TINYINT        NOT NULL,
    [TipoOperazione]      TINYINT        NOT NULL,
    [UserName]            NVARCHAR (50)  NOT NULL,
    [DataOperazione]      DATETIME       NOT NULL,
    [DescrizioneModifica] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_ApplicazioneInfoOperazioneRecord_ID] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [CK_InfoOperazioneRecord_TipoOperazione] CHECK ([TipoOperazione]>=(0) AND [TipoOperazione]<=(2)),
    CONSTRAINT [FK_InfoOperazioneRecord_InfoOperazioneRecord_Tabella] FOREIGN KEY ([IDTabellaLegame]) REFERENCES [SeCoGEST].[InfoOperazioneRecord_Tabella] ([ID])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'0 = Creazione; 
1 = Modifica; 
2 = Eliminazione
', @level0type = N'SCHEMA', @level0name = N'SeCoGEST', @level1type = N'TABLE', @level1name = N'InfoOperazioneRecord', @level2type = N'COLUMN', @level2name = N'TipoOperazione';

