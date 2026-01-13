-- =============================================
-- Author:		Maurizio Coffinardi
-- Create date: 24/02/2017
-- Description:	Restituisce un campo bit che indica se AnalisiCostoArticolo ha degli AnalisiCostoArticoloCampoAggiuntivo associati
-- =============================================
CREATE FUNCTION [SeCoGEST].[IsArticoloConCampiAggiuntivi]
(
	@IDAnalisiCostoArticolo	uniqueidentifier
)
RETURNS bit
AS
BEGIN
	DECLARE @resultVar bit = 0
	
	SELECT @resultVar = CASE WHEN EXISTS 
		(SELECT TOP 1 1 
		 FROM SeCoGEST.AnalisiCostoArticoloCampoAggiuntivo 
		 WHERE IDAnalisiCostoArticolo = @IDAnalisiCostoArticolo
		 ) 
         THEN CAST (1 AS BIT) 
         ELSE CAST (0 AS BIT) END

	RETURN @resultVar
END