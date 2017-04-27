Imports WholeFoods.IRMA.ItemHosting.DataAccess
Imports WholeFoods.IRMA.ItemHosting.BusinessLogic
Imports WholeFoods.IRMA.ModelLayer.DataAccess

Public Class Form_ManagePriceChgType

    Private _NEW_TYPE_ID As Integer = -1
    Private _MODIFIED_ROWS As List(Of Integer) = New List(Of Integer)

    Private Sub Form_ManagePriceChgType_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        FillGrid()
    End Sub

    Private Sub FillGrid()
        UltraGrid_PriceChgType.DataSource = WholeFoods.IRMA.ItemHosting.DataAccess.PriceChgTypeDAO.GetPriceChgTypeList(True, False)
        UltraGrid_PriceChgType.DataBind()

        'Disable the read-only fields
        UltraGrid_PriceChgType.DisplayLayout.Bands(0).Columns("PriceChgTypeID").CellActivation = Infragistics.Win.UltraWinGrid.Activation.Disabled
        UltraGrid_PriceChgType.DisplayLayout.Bands(0).Columns("LastUpdateTimestamp").CellActivation = Infragistics.Win.UltraWinGrid.Activation.Disabled

        _MODIFIED_ROWS.Clear()
    End Sub

    Private Sub Button_Insert_Click(sender As System.Object, e As System.EventArgs) Handles Button_Insert.Click
        Dim newType As PriceChgTypeBO = New PriceChgTypeBO()
        newType.PriceChgTypeID = _NEW_TYPE_ID
        newType.PriceChgTypeDesc = ""
        newType.Priority = 0
        newType.IsOnSale = True
        newType.IsMSRPRequired = False
        newType.IsLineDrive = False
        'newType.IsCompetetive = False
        'newType.LastUpdate = Today

        Dim _types As ArrayList = CType(UltraGrid_PriceChgType.DataSource, ArrayList)
        Dim newRow As Integer = _types.Add(newType)
        _MODIFIED_ROWS.Add(newRow)

        UltraGrid_PriceChgType.DataSource = _types
        UltraGrid_PriceChgType.DataBind()
    End Sub

    Private Sub Button_Save_Click(sender As System.Object, e As System.EventArgs) Handles Button_Save.Click
        Dim _types As ArrayList = CType(UltraGrid_PriceChgType.DataSource, ArrayList)
        Dim dao As WholeFoods.IRMA.ModelLayer.DataAccess.PriceChgTypeDAO = New WholeFoods.IRMA.ModelLayer.DataAccess.PriceChgTypeDAO()
        Dim currentType As WholeFoods.IRMA.ModelLayer.BusinessLogic.PriceChgType

        Try
            _MODIFIED_ROWS.Sort()
            For Each gridRow As Infragistics.Win.UltraWinGrid.UltraGridRow In UltraGrid_PriceChgType.Rows
                If _MODIFIED_ROWS.Contains(gridRow.Index) Then
                    'Get the current row's Business Object
                    currentType = New WholeFoods.IRMA.ModelLayer.BusinessLogic.PriceChgType()
                    currentType.PriceChgTypeDesc = gridRow.Cells("PriceChgTypeDesc").Value
                    currentType.Priority = gridRow.Cells("Priority").Value
                    currentType.OnSale = gridRow.Cells("IsOnSale").Value
                    currentType.MSRPRequired = gridRow.Cells("IsMSRPRequired").Value
                    currentType.LineDrive = gridRow.Cells("IsLineDrive").Value
                    currentType.Competetive = gridRow.Cells("IsCompetitive").Value

                    If gridRow.Cells("PriceChgTypeID").Value = _NEW_TYPE_ID Then
                        dao.InsertPriceChgType(currentType)
                    Else
                        currentType.PriceChgTypeID = gridRow.Cells("PriceChgTypeID").Value
                        dao.UpdatePriceChgType(currentType)
                    End If
                End If
            Next
            _MODIFIED_ROWS.Clear()
            FillGrid()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub UltraGrid_PriceChgType_CellChange(sender As System.Object, e As Infragistics.Win.UltraWinGrid.CellEventArgs) Handles UltraGrid_PriceChgType.CellChange
        If (Not e.Cell.OriginalValue = e.Cell.Value Or e.Cell.DataChanged) And Not _MODIFIED_ROWS.Contains(e.Cell.Row.Index) Then
            _MODIFIED_ROWS.Add(e.Cell.Row.Index)
        End If
    End Sub
End Class