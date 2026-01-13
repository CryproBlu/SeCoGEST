-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [SeCoGEST].[GetStatoIntervento]
(
	@IDIntervento	uniqueidentifier
)
RETURNS tinyint
AS
BEGIN
	DECLARE @stato varchar(20)

	SELECT TOP(1) @stato = Stato
	from SeCoGEST.Intervento_Stato 
	WHERE IDIntervento = @IDIntervento 
	order by Data DESC

	return @stato
END