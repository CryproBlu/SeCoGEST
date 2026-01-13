-- =============================================
-- Author:		Maurizio Coffinardi
-- Create date: 29/12/2015
-- Description:	Restituisce la data minore fra le due passate
-- =============================================
CREATE FUNCTION [SeCoGEST].[GetDataMinoreScadenzaContratto]
(
	@data1		datetime,
	@data2		datetime,
	@dataSeNull	datetime
)
RETURNS datetime
AS
BEGIN
	DECLARE @data	datetime

	SET @data1 = ISNULL(@data1, @dataSeNull)
	SET @data2 = ISNULL(@data2, @dataSeNull)
	
	IF (@data1 < @data2) SET @data = @data1 ELSE SET @data = @data2 

	RETURN @data
END