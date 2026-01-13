CREATE TABLE [SeCoGEST].[Intervento_Operatore] (
    [ID]                    UNIQUEIDENTIFIER NOT NULL,
    [IDIntervento]          UNIQUEIDENTIFIER NOT NULL,
    [IDOperatore]           UNIQUEIDENTIFIER NOT NULL,
    [IDModalitaRisoluzione] INT              NULL,
    [InizioIntervento]      DATETIME         NULL,
    [FineIntervento]        DATETIME         NULL,
    [DurataMinuti]          INT              NULL,
    [PausaMinuti]           INT              NULL,
    [TotaleMinuti]          AS               (isnull([DurataMinuti],(0))-isnull([PausaMinuti],(0))) PERSISTED,
    [PresaInCarico]         BIT              NULL,
    [DataPresaInCarico]     DATETIME         NULL,
    [Note]                  NVARCHAR (4000)  NULL,
    CONSTRAINT [PK_Intervento_Operatore] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Intervento_Operatore_Intervento] FOREIGN KEY ([IDIntervento]) REFERENCES [SeCoGEST].[Intervento] ([ID]) ON DELETE CASCADE,
    CONSTRAINT [FK_Intervento_Operatore_ModalitaRisoluzioneIntervento] FOREIGN KEY ([IDModalitaRisoluzione]) REFERENCES [SeCoGEST].[ModalitaRisoluzioneIntervento] ([ID]),
    CONSTRAINT [FK_Intervento_Operatore_Operatori] FOREIGN KEY ([IDOperatore]) REFERENCES [SeCoGEST].[Operatore] ([ID])
);






















GO
CREATE NONCLUSTERED INDEX [IX_Intervento_Operatore_2]
    ON [SeCoGEST].[Intervento_Operatore]([IDModalitaRisoluzione] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Intervento_Operatore_1]
    ON [SeCoGEST].[Intervento_Operatore]([IDOperatore] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Intervento_Operatore]
    ON [SeCoGEST].[Intervento_Operatore]([IDIntervento] ASC);


GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE TRIGGER [SeCoGEST].[AggiornaTotaleMinutiIntervento]
   ON [SeCoGEST].[Intervento_Operatore]
   AFTER INSERT,DELETE,UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @IDIntervento uniqueidentifier = NULL
	SELECT @IDIntervento = IDIntervento FROM inserted

	IF @IDIntervento IS NULL 
		SELECT @IDIntervento = IDIntervento FROM deleted

    UPDATE [SeCoGEST].[Intervento]
	SET	TotaleMinuti = (SELECT SUM(TotaleMinuti) 
						FROM [SeCoGEST].[Intervento_Operatore] 
						WHERE [SeCoGEST].[Intervento_Operatore].IDIntervento = @IDIntervento)
	WHERE [SeCoGEST].[Intervento].ID = @IDIntervento

END