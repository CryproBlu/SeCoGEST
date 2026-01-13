CREATE VIEW dbo.InterventiAnnuiPerOperatore
AS
SELECT        TOP (100) PERCENT SeCoGEST.Intervento.Numero, Format(SeCoGEST.Intervento.DataRedazione, 'dd/MM/yyyy hh:mm') AS RedattoIl, SeCoGEST.Intervento.CodiceCliente, SeCoGEST.Intervento.RagioneSociale, 
                         SeCoGEST.Intervento.DestinazioneMerce, SeCoGEST.Intervento.Indirizzo, SeCoGEST.Intervento.CAP, SeCoGEST.Intervento.Localita, SeCoGEST.Intervento.Provincia, SeCoGEST.Intervento.Telefono, 
                         SeCoGEST.Intervento.Oggetto, SeCoGEST.Intervento.Definizione, SeCoGEST.Intervento.NumeroCommessa, SeCoGEST.Intervento.DescrizioneStato, SeCoGEST.Operatore.CognomeNome AS Operatore, 
                         SeCoGEST.Intervento_Operatore.InizioIntervento, SeCoGEST.Intervento_Operatore.FineIntervento, SeCoGEST.Intervento.TotaleMinuti
FROM            SeCoGEST.Intervento INNER JOIN
                         SeCoGEST.Intervento_Operatore ON SeCoGEST.Intervento.ID = SeCoGEST.Intervento_Operatore.IDIntervento INNER JOIN
                         SeCoGEST.Operatore ON SeCoGEST.Intervento_Operatore.IDOperatore = SeCoGEST.Operatore.ID
WHERE        (YEAR(SeCoGEST.Intervento.DataRedazione) = 2021) AND (SeCoGEST.Operatore.Cognome = N'Acchiappati')
ORDER BY SeCoGEST.Intervento.DataRedazione
GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 2, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'InterventiAnnuiPerOperatore';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane2', @value = N'  Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'InterventiAnnuiPerOperatore';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane1', @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[11] 4[22] 2[21] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1[39] 4[29] 3) )"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1[50] 2[25] 3) )"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1[49] 3) )"
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
         Begin Table = "Intervento (SeCoGEST)"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 302
               Right = 310
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Intervento_Operatore (SeCoGEST)"
            Begin Extent = 
               Top = 33
               Left = 656
               Bottom = 318
               Right = 863
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Operatore (SeCoGEST)"
            Begin Extent = 
               Top = 6
               Left = 1061
               Bottom = 197
               Right = 1251
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
      Begin ColumnWidths = 21
         Width = 284
         Width = 2235
         Width = 1890
         Width = 2790
         Width = 2385
         Width = 3300
         Width = 2625
         Width = 2895
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 8895
         Width = 2940
         Width = 2700
         Width = 1740
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 2235
         Alias = 2280
         Table = 2850
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
       ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'InterventiAnnuiPerOperatore';

