Imports System.Text
Imports WholeFoods.IRMA.ItemHosting.BusinessLogic
Imports WholeFoods.IRMA.ItemHosting.DataAccess
Imports WholeFoods.Utility


Public Class MarginInfo
    Inherits System.Windows.Forms.Form

    Private _itemBO As ItemBO
    Private UseAverageCostforCostandMargin As Boolean = False

    Public Property ItemBO() As ItemBO
        Get
            Return _itemBO
        End Get
        Set(ByVal value As ItemBO)
            _itemBO = value
        End Set
    End Property

    Private Sub MarginInfo_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.CenterToParent()

        CheckConfigurationSettings()
        LoadData()
        BindData()
    End Sub

    Private Sub CheckConfigurationSettings()
        Try
            UseAverageCostforCostandMargin = ConfigurationServices.AppSettings("UseAvgCostforCostandMargin")
        Catch ex As Exception
            ' UseAverageCostforCostandMargin was not found. This key needs to be created. Default to false and display a warning.
            UseAverageCostforCostandMargin = False
            Label_AvgCostConfigMessage.Visible = True
        End Try
    End Sub

    Private Sub Button_Exit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Exit.Click
        Me.Close()
    End Sub

    Private Sub LoadData()
        'set item description label
        Me.Label_ItemDescValue.Text = _itemBO.ItemDescription

        'get item unit data
        ItemUnitDAO.GetItemUnitInfo(_itemBO)

        Dim packDesc As New StringBuilder
        packDesc.Append(Format(_itemBO.PackageDesc1, ResourcesIRMA.GetString("NumberFormatBigInteger")))
        packDesc.Append(" / ")
        packDesc.Append(Format(_itemBO.PackageDesc2, ResourcesIRMA.GetString("NumberFormatBigDecimal")))
        packDesc.Append(" ")
        packDesc.Append(_itemBO.PackageUnitName)

        Me.Label_PkgDescValue.Text = packDesc.ToString
    End Sub

    Private Sub BindData()
        'bind data to margin grid
        Me.UltraGrid_MarginInfo.DataSource = ItemDAO.GetMarginInfo(_itemBO.Item_Key)

        If UltraGrid_MarginInfo.DisplayLayout.Bands(0).Columns.Count > 0 Then
            'hide columns
            UltraGrid_MarginInfo.DisplayLayout.Bands(0).Columns("PackageDesc1").Hidden = True

            'sort columns in correct order
            UltraGrid_MarginInfo.DisplayLayout.Bands(0).Columns("StoreName").Header.VisiblePosition = 0
            UltraGrid_MarginInfo.DisplayLayout.Bands(0).Columns("CompanyName").Header.VisiblePosition = 1
            UltraGrid_MarginInfo.DisplayLayout.Bands(0).Columns("RegularUnitCost").Header.VisiblePosition = 2
            UltraGrid_MarginInfo.DisplayLayout.Bands(0).Columns("NetUnitCost").Header.VisiblePosition = 3
            UltraGrid_MarginInfo.DisplayLayout.Bands(0).Columns("CurrentPrice").Header.VisiblePosition = 4
            UltraGrid_MarginInfo.DisplayLayout.Bands(0).Columns("RegularMarginCurrentCost").Header.VisiblePosition = 5
            UltraGrid_MarginInfo.DisplayLayout.Bands(0).Columns("CurrenMarginCurrentCost").Header.VisiblePosition = 6

            'set col headers 
            UltraGrid_MarginInfo.DisplayLayout.Bands(0).Columns("CurrenMarginCurrentCost").Header.Caption = "Cur Margin% (CurCost)"
            UltraGrid_MarginInfo.DisplayLayout.Bands(0).Columns("RegularMarginCurrentCost").Header.Caption = "Reg Margin% (CurCost)"
            UltraGrid_MarginInfo.DisplayLayout.Bands(0).Columns("RegularUnitCost").Header.Caption = "Reg Unit Cost"
            UltraGrid_MarginInfo.DisplayLayout.Bands(0).Columns("NetUnitCost").Header.Caption = "Net Unit Cost"
            UltraGrid_MarginInfo.DisplayLayout.Bands(0).Columns("AvgCost").Header.Caption = "Avg Cost"
            UltraGrid_MarginInfo.DisplayLayout.Bands(0).Columns("CurrentMarginAvgCost").Header.Caption = "Cur Margin% (AvgCost)"
            UltraGrid_MarginInfo.DisplayLayout.Bands(0).Columns("RegularMarginAvgCost").Header.Caption = "Reg Margin% (AvgCost)"


            'set col tooltips
            UltraGrid_MarginInfo.DisplayLayout.Bands(0).Columns("CurrenMarginCurrentCost").Header.ToolTipText = "Cur Margin% (CurCost) = 100*(Current Retail - Current Net Unit Cost from Primary Vendor)/Current Retail"
            UltraGrid_MarginInfo.DisplayLayout.Bands(0).Columns("RegularMarginCurrentCost").Header.ToolTipText = "Reg Margin% (CurCost) = 100*(Current Reg Retail - Current Net Unit Cost from Primary Vendor)/Current Reg Retail"
            UltraGrid_MarginInfo.DisplayLayout.Bands(0).Columns("RegularUnitCost").Header.ToolTipText = "Reg Unit Cost = Current Reg Cost from Primary Vendor/Current Vendor Pack"
            UltraGrid_MarginInfo.DisplayLayout.Bands(0).Columns("NetUnitCost").Header.ToolTipText = "Net Unit Cost = Current Net Cost from Primary Vendor/Current Vendor Pack"
            UltraGrid_MarginInfo.DisplayLayout.Bands(0).Columns("AvgCost").Header.ToolTipText = "(yesterday’s ending inventory x yesterday’s average cost) + (today’s receipts x today’s received cost) all divided by today’s ending inventory (i.e. yesterday’s ending inventory + today’s receipts – today’s sales)"
            UltraGrid_MarginInfo.DisplayLayout.Bands(0).Columns("CurrentMarginAvgCost").Header.ToolTipText = "Cur Margin% (AvgCost) = (Current Retail - Current Average Cost)/Current Retail"
            UltraGrid_MarginInfo.DisplayLayout.Bands(0).Columns("RegularMarginAvgCost").Header.ToolTipText = "Reg Margin (AvgCost) = (Current Reg Retail - Current Average Cost)/Current Reg Retail"


            'right justify cost cols
            UltraGrid_MarginInfo.DisplayLayout.Bands(0).Columns("RegularUnitCost").CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            UltraGrid_MarginInfo.DisplayLayout.Bands(0).Columns("NetUnitCost").CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            UltraGrid_MarginInfo.DisplayLayout.Bands(0).Columns("CurrentPrice").CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right

            'prevent cols from being edited
            UltraGrid_MarginInfo.DisplayLayout.Bands(0).Columns("RegularUnitCost").CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
            UltraGrid_MarginInfo.DisplayLayout.Bands(0).Columns("RegularUnitCost").CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
            UltraGrid_MarginInfo.DisplayLayout.Bands(0).Columns("NetUnitCost").CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
            UltraGrid_MarginInfo.DisplayLayout.Bands(0).Columns("NetUnitCost").CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect

            UltraGrid_MarginInfo.DisplayLayout.Bands(0).Columns("CurrentPrice").Format = "#0.00"


            With UltraGrid_MarginInfo.DisplayLayout.Bands(0)
                .Columns("CurrentMarginAvgCost").Hidden = Not UseAverageCostforCostandMargin
                .Columns("RegularMarginAvgCost").Hidden = Not UseAverageCostforCostandMargin
                .Columns("AvgCost").Hidden = Not UseAverageCostforCostandMargin

                .Columns("CurrenMarginCurrentCost").Hidden = UseAverageCostforCostandMargin
                .Columns("RegularMarginCurrentCost").Hidden = UseAverageCostforCostandMargin
                .Columns("RegularUnitCost").Hidden = UseAverageCostforCostandMargin
                .Columns("NetUnitCost").Hidden = UseAverageCostforCostandMargin

                'set autosize flags.
                .Columns("StoreName").AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.None
                .Columns("CompanyName").AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.None
                .Columns("CurrentPrice").AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.VisibleRows
                .Columns("CurrenMarginCurrentCost").AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.VisibleRows
                .Columns("RegularMarginCurrentCost").AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.VisibleRows
                .Columns("RegularUnitCost").AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.VisibleRows
                .Columns("NetUnitCost").AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.VisibleRows
            End With

            'autosize.
            UltraGrid_MarginInfo.DisplayLayout.PerformAutoResizeColumns(False, Infragistics.Win.UltraWinGrid.PerformAutoSizeType.None, True)
        End If
    End Sub

    Private Sub UltraGrid_MarginInfo_AfterRowActivate(sender As System.Object, e As System.EventArgs) Handles UltraGrid_MarginInfo.AfterRowActivate

        ' Get the store number of the currently selected row.
        Dim storeNumber As Integer = CInt(UltraGrid_MarginInfo.ActiveRow.GetCellValue("StoreNo").ToString())

        ' Update the Item object with the values relevant to that store's jurisdiction.
        ItemUnitDAO.GetItemUnitInfo(_itemBO, storeNumber)

        ' Display the updated values in the UI.
        Me.Label_ItemDescValue.Text = _itemBO.ItemDescription

        Dim packDesc As New StringBuilder
        packDesc.Append(Format(_itemBO.PackageDesc1, ResourcesIRMA.GetString("NumberFormatBigInteger")))
        packDesc.Append(" / ")
        packDesc.Append(Format(_itemBO.PackageDesc2, ResourcesIRMA.GetString("NumberFormatBigDecimal")))
        packDesc.Append(" ")
        packDesc.Append(_itemBO.PackageUnitName)

        Me.Label_PkgDescValue.Text = packDesc.ToString

    End Sub

End Class