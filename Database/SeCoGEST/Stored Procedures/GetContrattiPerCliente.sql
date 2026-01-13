-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [SeCoGEST].[GetContrattiPerCliente]
	@CodiceCliente	varchar(7),
	@DataIntervento	datetime
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT	TipologiaArticolo, CodiceContratto, DescrizioneContratto
	FROM	[SeCoGEST].[GetElencoCompletoArticoli](@DataIntervento)
	WHERE	codicecliente = @CodiceCliente
	GROUP BY TipologiaArticolo, CodiceContratto, DescrizioneContratto

END