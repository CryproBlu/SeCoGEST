-- =============================================
-- Author:		Maurizio Coffinardi
-- Create date: 22/03/2018
-- Description:	Clona il contratto specificato di un cliente attribuendolo ad un altro cliente
-- =============================================
CREATE PROCEDURE [SeCoGEST].[CopiaContrattoCliente]
	@CodContoSorgente		varchar(7),
	@CodContoDestinazione	varchar(7),
	@CodContratto				varchar(25)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @NewProgressivo decimal(18, 0)
	
	BEGIN TRANSACTION

	SELECT @NewProgressivo = MAX(Progressivo) FROM _T_ExtraACorpo
	INSERT INTO _T_ExtraACorpo
	SELECT	@NewProgressivo + row_number() over (order by (select NULL)) as Progressivo, 
			Codart, Descrizione, Riferimento, DataAttivazione, ImportoPrevisto, TempoPrevistoH, TempoConsuntivoH, Note, UtenteModifica, DataModifica, @CodContoDestinazione as CodConto, FlagChiuso, TempoPrevistoM, TempoConsuntivoM, 
            TempoDaFareH, TempoDaFareM, ResiduoPerNuovoContratto, DataChiusura, Contratto
	FROM	dbo._T_ExtraACorpo
	WHERE	(CodConto = @CodContoSorgente
	AND		DataChiusura IS NULL
	AND		Contratto = @CodContratto)
	OR
			(CodConto = @CodContoSorgente
	AND		DataChiusura IS NOT NULL
	AND		DataChiusura >	GetDate()
	AND		Contratto = @CodContratto)
	

	SELECT @NewProgressivo = MAX(Progressivo) FROM _T_ExtraAddebito
	INSERT INTO _T_ExtraAddebito
	SELECT	@NewProgressivo + row_number() over (order by (select NULL)) as Progressivo, 
			Codart, Descrizione, DataAttivazione, AddebitoinSempre, AddebitoinPrepagato, Quantita, Costo, Note, UtenteModifica, DataModifica, @CodContoDestinazione as CodConto, Urgenza, OLD_TempoViaggio, Contratto
	FROM	dbo._T_ExtraAddebito
	WHERE	CodConto = @CodContoSorgente
	AND		Contratto = @CodContratto

	  
	
	SELECT @NewProgressivo = MAX(Progressivo) FROM _T_ExtraContratti
	INSERT INTO _T_ExtraContratti   
	SELECT	@NewProgressivo + row_number() over (order by (select NULL)) as Progressivo, 
			Codart, Descrizione, DataAttivazione, ImportoContratto, AnnoFatturazione, MeseFatturazione, ImportoFatturati, Note, UtenteModifica, DataModifica, @CodContoDestinazione as CodConto, dataDisdetta, DecorrenzaDisdetta, Mensilita, 
            DataUltFatturazione, DataNewFatturazione, TipoFatturazione, Contratto, AbilitaFatturazione, Scadenza, Tacitorinnovo, Sostituzione, Disdetta, CodPagamento, ContrattoSostitutivo, TipoRinnovo, TipoMensilita
	FROM	dbo._T_ExtraContratti      
	WHERE	CodConto = @CodContoSorgente
	AND		decorrenzadisdetta is null
	AND		Contratto = @CodContratto



	SELECT @NewProgressivo = MAX(Progressivo) FROM _T_ExtraPrepagati
	INSERT INTO _T_ExtraPrepagati   
	SELECT	@NewProgressivo + row_number() over (order by (select NULL)) as Progressivo, 
			Codart, Descrizione, DataAttivazione, ImportoPrepagato, MinutiInclusi, MinutiFatti, Note, UtenteModifica, DataModifica, @CodContoDestinazione as CodConto, Orefatte, OreIncluse, OreDaFare, MinutiDaFare, FlagChiuso, Tipologia, TariffaOraria, ResiduoPerNuovoContratto, DataChiusura, Contratto
	FROM    dbo._T_ExtraPrepagati
	WHERE	(CodConto = @CodContoSorgente
	AND		DataChiusura IS NULL
	AND		Contratto = @CodContratto)
	OR
			(CodConto = @CodContoSorgente
	AND		DataChiusura IS NOT NULL
	AND		DataChiusura >	GetDate()
	AND		Contratto = @CodContratto)



	                
	SELECT @NewProgressivo = MAX(Progressivo) FROM _T_ExtraServizi
	INSERT INTO _T_ExtraServizi   
	SELECT	@NewProgressivo + row_number() over (order by (select NULL)) as Progressivo, 
			Codart, Descrizione, DataAttivazione, ImportoServizi, Note, UtenteModifica, DataModifica, @CodContoDestinazione, TempoMinimoFatturazione, AddebitoinPrepagato, TipologiaConsulenti, Sel_Default, Contratto
	FROM    dbo._T_ExtraServizi
	WHERE	CodConto = @CodContoSorgente
	AND		Contratto = @CodContratto

	COMMIT TRANSACTION

END