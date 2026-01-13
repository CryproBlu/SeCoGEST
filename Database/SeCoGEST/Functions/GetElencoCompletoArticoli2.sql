-- =============================================
-- Author:		Maurizio Coffinardi
-- Create date: 29/12/2015
-- Description:	Restituisce l'elenco completo degli articoli gestiti da metodo compresi quelli specificati nei Contratti attivi nella data specificata
-- =============================================
CREATE FUNCTION [SeCoGEST].[GetElencoCompletoArticoli2]
(	
	@dataValidita	datetime
)
RETURNS TABLE 
AS
RETURN 
(
	SELECT		'0' + CODICE AS ID, 0 AS TipologiaArticolo, 
				'Articoli Magazzino' AS DescrizioneTipologia, 
				NULL AS Progressivo, '' AS CodiceContratto, '' AS CodiceCliente, CODICE AS CodiceArticolo, 
				DESCRIZIONE AS Descrizione, NULL AS OreFatte, NULL AS OreIncluse, NULL AS OreDaFare, NULL AS Note, 
				CONVERT(DATETIME, '1900-01-01') as DataAttivazione, 
				CONVERT(DATETIME, '2099-01-01') as DataChiusura
	FROM		dbo.ANAGRAFICAARTICOLI

	UNION ALL


	--SELECT		'1' + CONVERT(VARCHAR(50), Progressivo) AS ID, 1 AS TipologiaArticolo, 
	--			'Articoli di Contratti' AS DescrizioneTipologia, 
	--			Progressivo, Contratto AS CodiceContratto, CodConto AS CodiceCliente, Codart AS CodiceArticolo, 
	--			Descrizione AS Descrizione, NULL AS OreFatte, NULL AS OreIncluse, NULL AS OreDaFare, Note, 
	--			ISNULL(DataAttivazione, CONVERT(DATETIME, '1900-01-01')) as DataAttivazione, 
	--			[SeCoGEST].[GetDataMinoreScadenzaContratto](DecorrenzaDisdetta, Scadenza, CONVERT(datetime, '2099-01-01')) as DataChiusura
	--FROM        [dbo]._T_ExtraContratti
	SELECT		'1' + CONVERT(VARCHAR(50), Progressivo) AS ID, 1 AS TipologiaArticolo, 
				'Articoli di Contratti' AS DescrizioneTipologia, 
				Progressivo, Contratto AS CodiceContratto, CodConto AS CodiceCliente, Codart AS CodiceArticolo, 
				Descrizione AS Descrizione, NULL AS OreFatte, NULL AS OreIncluse, NULL AS OreDaFare, Note, 
				ISNULL(DataAttivazione, CONVERT(DATETIME, '1900-01-01')) as DataAttivazione, 
				[SeCoGEST].[GetDataMinoreScadenzaContratto](DecorrenzaDisdetta, Scadenza, CONVERT(datetime, '2099-01-01')) as DataChiusura
	FROM        [dbo]._T_ExtraContratti
	WHERE       [SeCoGEST].[IsContrattoAttivo](Contratto, '2017-01-01') = 1 AND
				Codart <> 'CONTRATTO' 
				AND Disdetta = 0 
				AND Sostituzione = 0

	UNION ALL

	--SELECT		'2' + CONVERT(VARCHAR(50), Progressivo) AS ID, 2 AS TipologiaArticolo, 
	--			'Contratti - Articoli Prepagati' AS DescrizioneTipologia, Progressivo, Contratto AS CodiceContratto, CodConto AS CodiceCliente, Codart AS CodiceArticolo, 
	--			Descrizione AS Descrizione, OreFatte, OreIncluse, OreDaFare, Note,
	--			ISNULL(DataAttivazione, CONVERT(DATETIME, '1900-01-01')) as DataAttivazione, 
	--			isnull(DataChiusura, CONVERT(DATETIME, '2099-01-01')) as DataChiusura
	--FROM		[dbo]._T_ExtraPrepagati 
	SELECT		'2' + CONVERT(VARCHAR(50), Progressivo) AS ID, 2 AS TipologiaArticolo, 
				'Contratti - Articoli Prepagati' AS DescrizioneTipologia, Progressivo, Contratto AS CodiceContratto, CodConto AS CodiceCliente, Codart AS CodiceArticolo, 
				Descrizione AS Descrizione, OreFatte, OreIncluse, OreDaFare, Note,
				ISNULL(DataAttivazione, CONVERT(DATETIME, '1900-01-01')) as DataAttivazione, 
				isnull(DataChiusura, CONVERT(DATETIME, '2099-01-01')) as DataChiusura
	FROM		[dbo]._T_ExtraPrepagati 
	WHERE       [SeCoGEST].[IsContrattoAttivo](Contratto, @dataValidita) = 1 AND
				Codart <> 'CONTRATTO' AND 
				FlagChiuso = 0

	UNION ALL

	--SELECT		'3' + CONVERT(VARCHAR(50), Progressivo) AS ID, 3 AS TipologiaArticolo, 
	--			'Contratti - Servizi' AS DescrizioneTipologia, Progressivo, Contratto AS CodiceContratto, CodConto AS CodiceCliente, Codart AS CodiceArticolo, 
	--			Descrizione AS Descrizione, NULL AS OreFatte, NULL AS OreIncluse, NULL AS OreDaFare, Note,
	--			ISNULL(DataAttivazione, CONVERT(DATETIME, '1900-01-01')) as DataAttivazione, 
	--			CONVERT(DATETIME, '2099-01-01') as DataChiusura
	--FROM        [dbo]._T_ExtraServizi
	SELECT		'3' + CONVERT(VARCHAR(50), Progressivo) AS ID, 3 AS TipologiaArticolo, 
				'Contratti - Servizi' AS DescrizioneTipologia, Progressivo, Contratto AS CodiceContratto, CodConto AS CodiceCliente, Codart AS CodiceArticolo, 
				Descrizione AS Descrizione, NULL AS OreFatte, NULL AS OreIncluse, NULL AS OreDaFare, Note,
				ISNULL(DataAttivazione, CONVERT(DATETIME, '1900-01-01')) as DataAttivazione, 
				CONVERT(DATETIME, '2099-01-01') as DataChiusura
	FROM        [dbo]._T_ExtraServizi
	WHERE       [SeCoGEST].[IsContrattoAttivo](Contratto, @dataValidita) = 1 AND
				Codart <> 'CONTRATTO'

	UNION ALL

	--SELECT		'4' + CONVERT(VARCHAR(50), Progressivo) AS ID, 4 AS TipologiaArticolo, 
	--			'Contratti - Addebiti' AS DescrizioneTipologia, Progressivo, Contratto AS CodiceContratto, CodConto AS CodiceCliente, Codart AS CodiceArticolo, 
	--			Descrizione AS Descrizione, NULL AS OreFatte, NULL AS OreIncluse, NULL AS OreDaFare, Note, 
	--			ISNULL(DataAttivazione, CONVERT(DATETIME, '1900-01-01')) as DataAttivazione,
	--			CONVERT(DATETIME, '2099-01-01') as DataChiusura
	--FROM       	[dbo]._T_ExtraAddebito
	SELECT		'4' + CONVERT(VARCHAR(50), Progressivo) AS ID, 4 AS TipologiaArticolo, 
				'Contratti - Addebiti' AS DescrizioneTipologia, Progressivo, Contratto AS CodiceContratto, CodConto AS CodiceCliente, Codart AS CodiceArticolo, 
				Descrizione AS Descrizione, NULL AS OreFatte, NULL AS OreIncluse, NULL AS OreDaFare, Note, 
				ISNULL(DataAttivazione, CONVERT(DATETIME, '1900-01-01')) as DataAttivazione,
				CONVERT(DATETIME, '2099-01-01') as DataChiusura
	FROM       	[dbo]._T_ExtraAddebito
	WHERE		[SeCoGEST].[IsContrattoAttivo](Contratto, @dataValidita) = 1
)