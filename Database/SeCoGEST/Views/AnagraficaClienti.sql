CREATE VIEW SeCoGEST.AnagraficaClienti
AS
SELECT        A.TIPOCONTO, A.CODCONTO, A.DSCCONTO1, A.DSCCONTO2, A.CODMASTRO, A.INDIRIZZO, A.CAP, A.LOCALITA, A.PROVINCIA, A.TELEFONO, A.FAX, A.TELEX, A.CODFISCALE, A.PARTITAIVA, A.CODNAZIONE, A.CODICEISO, 
                         A.CODLINGUA, A.TIPOPROFESSIONISTA, A.LUOGONASCITA, A.DATANASCITA, A.TIPODOCRTACC, A.VERSPRESSO, A.NOTE, A.UTENTEMODIFICA, A.DATAMODIFICA, A.INDIRIZZOINTERNET, A.INDIRIZZORITACC, 
                         A.LOCALITARITACC, A.PROVINCIARITACC, A.CAPRITACC, A.PARTITAIVARITACC, A.CODFISCALERITACC, A.ZonaSped, A.TipoClienteBudget, A.ConsideraBudget, A.CODREGIONE, A.PERCORSODOCUMENTI, A.FLGBLACKLIST, 
                         A.FLGPERSFISICA, A.DATANASCITAPERSFIS, A.COMNASCITAPERSFIS, A.PROVNASCITAPERSFIS, A.CODSTATOESTERO, A.STATOPROVCONTEA, A.INDIRIZZOESTERO, A.COGNOMEPERSFIS, A.NOMEPERSFIS, A.FLGELENCOCF, 
                         A.FLGSISTRI, A.CODDEST_EDI, A.FLGDOGANA, A.FLGQUALITY, A.StatoIndicatore, A.CODDESTINATARIOPA, A.CAUSALEPAGAMENTOPA, A.TIPOCASSAPA, A.RIFAMMINISTRAZIONEPA, A.SESSOPERSFIS, 
                         120 AS MINUTIMAXPRESAINCARICO, 960 AS MINUTIMAXCHIUSURA, R.STATOALTRO, R.NOTE1
FROM            dbo.ANAGRAFICACF AS A LEFT OUTER JOIN
                         dbo.ANAGRAFICARISERVATICF AS R ON A.CODCONTO = R.CODCONTO
WHERE        (A.TIPOCONTO = 'C') AND (R.ESERCIZIO = YEAR(GETDATE()))
GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 2, @level0type = N'SCHEMA', @level0name = N'SeCoGEST', @level1type = N'VIEW', @level1name = N'AnagraficaClienti';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane2', @value = N'Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
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
         Column = 3390
         Alias = 3465
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
', @level0type = N'SCHEMA', @level0name = N'SeCoGEST', @level1type = N'VIEW', @level1name = N'AnagraficaClienti';




GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane1', @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[19] 4[47] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1[50] 4[25] 3) )"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4[56] 2[14] 3) )"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1[56] 3) )"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4[50] 3) )"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3) )"
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
         Begin Table = "A"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 354
               Right = 265
            End
            DisplayFlags = 344
            TopColumn = 0
         End
         Begin Table = "R"
            Begin Extent = 
               Top = 6
               Left = 303
               Bottom = 307
               Right = 537
            End
            DisplayFlags = 280
            TopColumn = 48
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 63
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1245
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         ', @level0type = N'SCHEMA', @level0name = N'SeCoGEST', @level1type = N'VIEW', @level1name = N'AnagraficaClienti';



