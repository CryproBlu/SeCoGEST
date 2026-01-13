-- =============================================
-- Author:		Maurizio Coffinardi
-- Create date: 29/12/2015
-- Description:	Restituisce un campo bit che indica se il contratto specificato è attivo nella data indicata
-- =============================================
CREATE FUNCTION [SeCoGEST].[IsContrattoAttivo]
(
	@CodiceContratto	varchar(25),
	@Data				datetime
)
RETURNS bit
AS
BEGIN
	DECLARE @resultVar bit = 0
	DECLARE @numContratti int = 0;

	SELECT	@numContratti = Count(*) 
	FROM    [dbo]._T_ExtraContratti
	WHERE   Codart = 'CONTRATTO' AND 
			Contratto = @CodiceContratto AND
			ISNULL(Disdetta, 0) = 0 AND 
			ISNULL(Sostituzione, 0) = 0 AND
			@Data BETWEEN ISNULL(DataAttivazione, CONVERT(DATETIME, '1900-01-01')) AND [SeCoGEST].[GetDataMinoreScadenzaContratto](DecorrenzaDisdetta, Scadenza, CONVERT(datetime, '2099-01-01'))

	IF (@numContratti > 0) SET @resultVar = 1

	RETURN @resultVar
END