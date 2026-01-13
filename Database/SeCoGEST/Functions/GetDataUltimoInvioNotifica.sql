
-- =============================================
-- Author:		Gianluigi.Turla	
-- Create date: 14/07/2016
-- Description:	Restituisce la data dell'ultimo invio di una notifica in base ai parametri passati alla funzione
-- =============================================
CREATE FUNCTION [SeCoGEST].[GetDataUltimoInvioNotifica]
(
	@IDLegame uniqueidentifier,
	@IDTabellaLegame tinyint,
	@IDNotifica tinyint
)
RETURNS datetime
AS
BEGIN
	-- Vengono dichiarate le variabili
	DECLARE @ResultVar datetime

	-- Viene effettuata la query che recupera la data dell'ultimo invio di una notifica in base ai parametri passati
	SELECT TOP (1) @ResultVar = Data
	FROM [SeCoGEST].[LogInvioNotifica]
	WHERE IDLegame = @IDLegame AND IDTabellaLegame = @IDTabellaLegame AND IDNotifica = @IDNotifica
	ORDER BY Data DESC

	-- Viene restituita la data recuperata
	RETURN @ResultVar

END