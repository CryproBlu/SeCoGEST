-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [SeCoGEST].[GetDescrizioneStatoIntervento]
(
	@IDIntervento	uniqueidentifier
)
RETURNS varchar(100)
AS
BEGIN
	DECLARE @stato varchar(100)

	SELECT TOP(1) @stato = DescrizioneStato + ' - ' + NomeUtente + ' - ' + CONVERT(VARCHAR(20), CONVERT(varchar, Data, 113))
	from SeCoGEST.Intervento_Stato 
	WHERE IDIntervento = @IDIntervento 
	order by Data DESC

	return @stato
END