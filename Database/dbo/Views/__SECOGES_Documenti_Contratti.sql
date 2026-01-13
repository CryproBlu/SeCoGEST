CREATE VIEW dbo.__SECOGES_Documenti_Contratti
AS
SELECT        dbo.TESTEDOCUMENTI.PROGRESSIVO, dbo.TESTEDOCUMENTI.TIPODOC, dbo.TESTEDOCUMENTI.NUMERODOC, dbo.TESTEDOCUMENTI.DATADOC, dbo.TESTEDOCUMENTI.NUMRIFDOC, 
                         dbo.TESTEDOCUMENTI.CODCLIFOR, dbo.RIGHEDOCUMENTI.TIPORIGA, dbo.RIGHEDOCUMENTI.CODART, dbo.RIGHEDOCUMENTI.DESCRIZIONEART, dbo.EXTRARIGHEDOC.POSIZIONE, 
                         dbo.EXTRARIGHEDOC.UTENTEMODIFICA, dbo.EXTRARIGHEDOC.DATAMODIFICA, dbo.EXTRARIGHEDOC.Contratto
FROM            dbo.TESTEDOCUMENTI INNER JOIN
                         dbo.RIGHEDOCUMENTI ON dbo.TESTEDOCUMENTI.PROGRESSIVO = dbo.RIGHEDOCUMENTI.IDTESTA INNER JOIN
                         dbo.EXTRARIGHEDOC ON dbo.RIGHEDOCUMENTI.IDTESTA = dbo.EXTRARIGHEDOC.IDTESTA AND dbo.RIGHEDOCUMENTI.IDRIGA = dbo.EXTRARIGHEDOC.IDRIGA
WHERE        (dbo.TESTEDOCUMENTI.NUMERODOC = 1363)
GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 1, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'__SECOGES_Documenti_Contratti';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane1', @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[21] 2[11] 3) )"
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
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
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
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "TESTEDOCUMENTI"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 311
               Right = 304
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "RIGHEDOCUMENTI"
            Begin Extent = 
               Top = 6
               Left = 342
               Bottom = 292
               Right = 593
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "EXTRARIGHEDOC"
            Begin Extent = 
               Top = 43
               Left = 705
               Bottom = 239
               Right = 1004
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 14
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 2580
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
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
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'__SECOGES_Documenti_Contratti';

