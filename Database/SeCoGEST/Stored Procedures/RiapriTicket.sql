-- =============================================
-- Author:		Maurizio Coffinardi
-- Create date: 30/12/2021
-- Description:	Riapre il ticket relativo al numero passato
-- =============================================
CREATE PROCEDURE [SeCoGEST].[RiapriTicket]
	-- Add the parameters for the stored procedure here
	@numeroTicket	int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    UPDATE SeCoGEST.Intervento 
	SET Chiuso = 0
	WHERE Numero = @numeroTicket

END