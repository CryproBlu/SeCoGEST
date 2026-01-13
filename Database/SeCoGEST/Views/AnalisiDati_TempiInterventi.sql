CREATE VIEW SeCoGEST.AnalisiDati_TempiInterventi
AS
SELECT        SeCoGEST.Intervento.Numero, SeCoGEST.Intervento.DataRedazione, SeCoGEST.Intervento.DataPrevistaIntervento, SeCoGEST.Intervento.Oggetto, SeCoGEST.Intervento.Chiamata, SeCoGEST.Intervento.CodiceCliente, 
                         SeCoGEST.Intervento.RagioneSociale, SeCoGEST.Intervento.DescrizioneStato, SeCoGEST.Intervento.Chiuso, SeCoGEST.Operatore.Area, SeCoGEST.Operatore.CognomeNome AS Operatore, 
                         SeCoGEST.ModalitaRisoluzioneIntervento.Descrizione AS ModalitàRisoluzione, SeCoGEST.Intervento_Operatore.TotaleMinuti AS DurataMinuti
FROM            SeCoGEST.Intervento LEFT OUTER JOIN
                         SeCoGEST.Intervento_Operatore ON SeCoGEST.Intervento.ID = SeCoGEST.Intervento_Operatore.IDIntervento LEFT OUTER JOIN
                         SeCoGEST.Operatore ON SeCoGEST.Intervento_Operatore.IDOperatore = SeCoGEST.Operatore.ID LEFT OUTER JOIN
                         SeCoGEST.ModalitaRisoluzioneIntervento ON SeCoGEST.Intervento_Operatore.IDModalitaRisoluzione = SeCoGEST.ModalitaRisoluzioneIntervento.ID
GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 2, @level0type = N'SCHEMA', @level0name = N'SeCoGEST', @level1type = N'VIEW', @level1name = N'AnalisiDati_TempiInterventi';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane2', @value = N'     Output = 720
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
', @level0type = N'SCHEMA', @level0name = N'SeCoGEST', @level1type = N'VIEW', @level1name = N'AnalisiDati_TempiInterventi';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane1', @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[37] 4[24] 2[10] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1[50] 4[14] 3) )"
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
         Configuration = "(H (1[56] 3) )"
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
         Configuration = "(H (1[36] 4) )"
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
      ActivePaneConfig = 9
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "Intervento (SeCoGEST)"
            Begin Extent = 
               Top = 8
               Left = 20
               Bottom = 419
               Right = 292
            End
            DisplayFlags = 280
            TopColumn = 7
         End
         Begin Table = "Intervento_Operatore (SeCoGEST)"
            Begin Extent = 
               Top = 55
               Left = 394
               Bottom = 308
               Right = 680
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Operatore (SeCoGEST)"
            Begin Extent = 
               Top = 5
               Left = 820
               Bottom = 205
               Right = 1147
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "ModalitaRisoluzioneIntervento (SeCoGEST)"
            Begin Extent = 
               Top = 251
               Left = 849
               Bottom = 347
               Right = 1019
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
      PaneHidden = 
   End
   Begin DataPane = 
      PaneHidden = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 14
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1995
         Width = 4005
         Width = 1500
         Width = 1050
         Width = 2670
         Width = 1500
         Width = 2325
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 2190
         Alias = 1995
         Table = 4740
    ', @level0type = N'SCHEMA', @level0name = N'SeCoGEST', @level1type = N'VIEW', @level1name = N'AnalisiDati_TempiInterventi';



