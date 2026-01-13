-- =============================================
-- Author:		Maurizio Coffinardi
-- Create date: 05/04/2018
-- Description:	Sopsta il contratto specificato di un cliente attribuendolo ad un altro cliente. Sposta tutte le righe indipendentemente dal fatto che siano chiuse o aperte.
-- =============================================
CREATE PROCEDURE [SeCoGEST].[SpostaContrattoCliente_Tutto]
	@CodContoSorgente		varchar(7),
	@CodContoDestinazione	varchar(7),
	@CodContratto			varchar(25)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	BEGIN TRANSACTION

	-- *** Spostamento dati di Testata da cliente a cliente ***
	UPDATE	_T_TestaContratti
	SET		CodConto = @CodContoDestinazione
	WHERE	CodConto = @CodContoSorgente
		AND	Codice = @CodContratto

	-- ********************************************************

	-- Spostamento dati ExtraACorpo
	UPDATE	_T_ExtraACorpo
	SET		CodConto = @CodContoDestinazione
	WHERE	CodConto = @CodContoSorgente
		AND	Contratto = @CodContratto

	
	-- Spostamento dati ExtraAddebito
	UPDATE	_T_ExtraAddebito
	SET		CodConto = @CodContoDestinazione
	WHERE	CodConto = @CodContoSorgente
		AND	Contratto = @CodContratto

	
	-- Spostamento dati ExtraContratti
	UPDATE	_T_ExtraContratti
	SET		CodConto = @CodContoDestinazione
	WHERE	CodConto = @CodContoSorgente
		AND	Contratto = @CodContratto

	
	-- Spostamento dati ExtraPrepagati
	UPDATE	_T_ExtraPrepagati
	SET		CodConto = @CodContoDestinazione
	WHERE	CodConto = @CodContoSorgente
		AND	Contratto = @CodContratto

	
	-- Spostamento dati ExtraServizi
	UPDATE	_T_ExtraServizi
	SET		CodConto = @CodContoDestinazione
	WHERE	CodConto = @CodContoSorgente
		AND	Contratto = @CodContratto


	COMMIT TRANSACTION

END