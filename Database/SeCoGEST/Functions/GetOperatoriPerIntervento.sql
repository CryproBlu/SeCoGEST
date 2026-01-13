-- =============================================
-- Author:		Maurizio Coffinardi
-- Create date: 29/09/2017
-- Description:	Restituisce l'elenco dei nomi completi degli operatori dell'intervento indicato
-- =============================================
CREATE FUNCTION [SeCoGEST].[GetOperatoriPerIntervento]
(
	@IdIntervento		uniqueidentifier
)
RETURNS VARCHAR(8000)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @Names VARCHAR(8000)

	SELECT		@Names = COALESCE(@Names + ', ', '') + SQ.CognomeNome
	FROM		(SELECT DISTINCT TOP 100 PERCENT O.CognomeNome 
				 FROM		SeCoGEST.Intervento_Operatore I INNER JOIN 
							SeCoGEST.Operatore O on I.IDOperatore = O.ID 
				 WHERE		I.IDIntervento = @IdIntervento AND
							O.CognomeNome IS NOT NULL
				 ORDER BY	O.CognomeNome) SQ

	-- Return the result of the function
	RETURN @Names

END