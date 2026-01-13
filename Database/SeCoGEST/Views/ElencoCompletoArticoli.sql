CREATE VIEW SeCoGEST.ElencoCompletoArticoli
AS
SELECT        '0' + CODICE AS ID, 0 AS TipologiaArticolo, 'Articoli Magazzino' AS DescrizioneTipologia, NULL AS Progressivo, '' AS CodiceContratto, '' AS CodiceCliente, CODICE AS CodiceArticolo, 
                         DESCRIZIONE AS Descrizione, NULL AS OreFatte, NULL AS OreIncluse, NULL AS OreDaFare, NULL AS Note, CONVERT(DATETIME, '1900-01-01') as DataAttivazione, CONVERT(DATETIME, '2099-01-01') as DataChiusura
FROM            dbo.ANAGRAFICAARTICOLI
UNION ALL
SELECT        '1' + CONVERT(VARCHAR(50), Progressivo) AS ID, 1 AS TipologiaArticolo, 'Articoli di Contratti' AS DescrizioneTipologia, Progressivo, Contratto AS CodiceContratto, CodConto AS CodiceCliente, 
                         Codart AS CodiceArticolo, Descrizione AS Descrizione, NULL AS OreFatte, NULL AS OreIncluse, NULL AS OreDaFare, Note, ISNULL(DataAttivazione, CONVERT(DATETIME, '1900-01-01')) as DataAttivazione, isnull(DecorrenzaDisdetta, CONVERT(DATETIME, '2099-01-01')) as DataChiusura
FROM            [dbo]._T_ExtraContratti
WHERE        Codart <> 'CONTRATTO' AND Disdetta = 0 AND Sostituzione = 0
UNION ALL
SELECT        '2' + CONVERT(VARCHAR(50), Progressivo) AS ID, 2 AS TipologiaArticolo, 'Contratti - Articoli Prepagati' AS DescrizioneTipologia, Progressivo, Contratto AS CodiceContratto, CodConto AS CodiceCliente, 
                         Codart AS CodiceArticolo, Descrizione AS Descrizione, OreFatte, OreIncluse, OreDaFare, Note,  ISNULL(DataAttivazione, CONVERT(DATETIME, '1900-01-01')) as DataAttivazione, isnull(DataChiusura, CONVERT(DATETIME, '2099-01-01')) as DataChiusura
FROM            [dbo]._T_ExtraPrepagati 
WHERE        Codart <> 'CONTRATTO' AND FlagChiuso = 0
UNION ALL
SELECT        '3' + CONVERT(VARCHAR(50), Progressivo) AS ID, 3 AS TipologiaArticolo, 'Contratti - Servizi' AS DescrizioneTipologia, Progressivo, Contratto AS CodiceContratto, CodConto AS CodiceCliente, 
                         Codart AS CodiceArticolo, Descrizione AS Descrizione, NULL AS OreFatte, NULL AS OreIncluse, NULL AS OreDaFare, Note,  ISNULL(DataAttivazione, CONVERT(DATETIME, '1900-01-01')) as DataAttivazione, CONVERT(DATETIME, '2099-01-01') as DataChiusura
FROM            [dbo]._T_ExtraServizi
WHERE        Codart <> 'CONTRATTO'
UNION ALL
SELECT        '4' + CONVERT(VARCHAR(50), Progressivo) AS ID, 4 AS TipologiaArticolo, 'Contratti - Addebiti' AS DescrizioneTipologia, Progressivo, Contratto AS CodiceContratto, CodConto AS CodiceCliente, 
                         Codart AS CodiceArticolo, Descrizione AS Descrizione, NULL AS OreFatte, NULL AS OreIncluse, NULL AS OreDaFare, Note, ISNULL(DataAttivazione, CONVERT(DATETIME, '1900-01-01')) as DataAttivazione,CONVERT(DATETIME, '2099-01-01') as DataChiusura
FROM            [dbo]._T_ExtraAddebito
GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 1, @level0type = N'SCHEMA', @level0name = N'SeCoGEST', @level1type = N'VIEW', @level1name = N'ElencoCompletoArticoli';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane1', @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[19] 4[9] 2[29] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4[30] 2[40] 3) )"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2[40] 3) )"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4[38] 3) )"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[41] 4[33] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1[75] 4) )"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4) )"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 5
   End
   Begin DiagramPane = 
      PaneHidden = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 15
         Width = 284
         Width = 1770
         Width = 2490
         Width = 2205
         Width = 2910
         Width = 1965
         Width = 2355
         Width = 2025
         Width = 3330
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      PaneHidden = 
      Begin ColumnWidths = 11
         Column = 3285
         Alias = 2265
         Table = 2955
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
', @level0type = N'SCHEMA', @level0name = N'SeCoGEST', @level1type = N'VIEW', @level1name = N'ElencoCompletoArticoli';

