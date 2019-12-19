CREATE VIEW dbo.ViewGetCustomerReportData
AS
SELECT dbo.REPORT_DETAIL.PS_SUBTEAM, dbo.REPORT_DETAIL.UPC, dbo.ITEM_MASTER.BRAND, dbo.ITEM_MASTER.LONG_DESCRIPTION, 
               dbo.ITEM_MASTER.ITEM_SIZE, dbo.ITEM_MASTER.ITEM_UOM, dbo.REPORT_DETAIL.VENDOR_KEY, dbo.REPORT_DETAIL.VIN, 
               dbo.REPORT_DETAIL.MOVEMENT, 'computed' AS sales, 'get_times_scanned' AS times_scanned, dbo.REPORT_DETAIL.NOTES, 'computed' AS cost, 
               'computed' AS margin, dbo.REPORT_DETAIL.EFF_PRICE, dbo.REPORT_DETAIL.EFF_PRICETYPE, dbo.ITEM_MASTER.CATEGORY_NAME, 
               dbo.ITEM_MASTER.CLASS_NAME, 'computed' AS Avg_daily_Units, 'computed' AS Avg_Mov_Sales, 
               dbo.REASON.REASON_DESCRIPTION AS Product_Status
FROM  dbo.ITEM_MASTER LEFT OUTER JOIN
               dbo.REASON ON dbo.ITEM_MASTER.ID = dbo.REASON.ID INNER JOIN
               dbo.REPORT_DETAIL ON dbo.REASON.ID = dbo.REPORT_DETAIL.REASON_ID INNER JOIN
               dbo.REPORT_HEADER ON dbo.REPORT_DETAIL.REPORT_HEADER_ID = dbo.REPORT_HEADER.ID

GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane1', @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
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
         Begin Table = "ITEM_MASTER"
            Begin Extent = 
               Top = 7
               Left = 48
               Bottom = 135
               Right = 265
            End
            DisplayFlags = 280
            TopColumn = 8
         End
         Begin Table = "REASON"
            Begin Extent = 
               Top = 7
               Left = 313
               Bottom = 135
               Right = 531
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "REPORT_DETAIL"
            Begin Extent = 
               Top = 140
               Left = 48
               Bottom = 268
               Right = 261
            End
            DisplayFlags = 280
            TopColumn = 13
         End
         Begin Table = "REPORT_HEADER"
            Begin Extent = 
               Top = 140
               Left = 309
               Bottom = 268
               Right = 522
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
      Begin ColumnWidths = 22
         Width = 284
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
 ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'ViewGetCustomerReportData';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane2', @value = N'        Column = 1440
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
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'ViewGetCustomerReportData';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 2, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'ViewGetCustomerReportData';

