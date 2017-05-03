Imports WholeFoods.IRMA.ItemHosting.BusinessLogic
Imports WholeFoods.IRMA.ItemHosting.DataAccess

Public Class Form_ManageLabelType

    Private _NEW_TYPE_ID As Integer = -1
    Private _MODIFIED_ROWS As List(Of Integer) = New List(Of Integer)

    Private Sub Form_ManageLabelType_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        FillGrid()
    End Sub

    Private Sub FillGrid()
        UltraGrid_LabelType.DataSource = LabelTypeDAO.GetLabelTypeList()
        UltraGrid_LabelType.DataBind()

        'Disable the read-only fields
        UltraGrid_LabelType.DisplayLayout.Bands(0).Columns("LabelTypeID").CellActivation = Infragistics.Win.UltraWinGrid.Activation.Disabled

        _MODIFIED_ROWS.Clear()
    End Sub

    Private Sub Button_Insert_Click(sender As System.Object, e As System.EventArgs) Handles Button_Insert.Click
        Dim newType As LabelTypeBO = New LabelTypeBO()
        newType.LabelTypeID = _NEW_TYPE_ID
        newType.LabelTypeDesc = ""

        Dim _types As ArrayList = CType(UltraGrid_LabelType.DataSource, ArrayList)
        Dim newRow As Integer = _types.Add(newType)
        _MODIFIED_ROWS.Add(newRow)

        UltraGrid_LabelType.DataSource = _types
        UltraGrid_LabelType.DataBind()
    End Sub


    Private Sub Button_Save_Click(sender As System.Object, e As System.EventArgs) Handles Button_Save.Click
        Dim _types As ArrayList = CType(UltraGrid_LabelType.DataSource, ArrayList)
        Dim currentType As LabelTypeBO

        Try
            _MODIFIED_ROWS.Sort()
            For Each gridRow As Infragistics.Win.UltraWinGrid.UltraGridRow In UltraGrid_LabelType.Rows
                If _MODIFIED_ROWS.Contains(gridRow.Index) Then
                    'Get the current row's Business Object
                    currentType = New LabelTypeBO()
                    currentType.LabelTypeDesc = gridRow.Cells("LabelTypeDesc").Value

                    If gridRow.Cells("LabelTypeID").Value = _NEW_TYPE_ID Then
                        LabelTypeDAO.AddLabelType(currentType)
                    Else
                        currentType.LabelTypeID = gridRow.Cells("LabelTypeID").Value
                        LabelTypeDAO.UpdateLabelType(currentType)
                    End If
                End If
            Next
            _MODIFIED_ROWS.Clear()
            FillGrid()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub UltraGrid_LabelType_CellChange(sender As System.Object, e As Infragistics.Win.UltraWinGrid.CellEventArgs) Handles UltraGrid_LabelType.CellChange
        If (Not e.Cell.OriginalValue = e.Cell.Value Or e.Cell.DataChanged) And Not _MODIFIED_ROWS.Contains(e.Cell.Row.Index) Then
            _MODIFIED_ROWS.Add(e.Cell.Row.Index)
        End If
    End Sub

    Private Sub UltraGrid_LabelType_ClickCellButton(sender As System.Object, e As Infragistics.Win.UltraWinGrid.CellEventArgs) Handles UltraGrid_LabelType.ClickCellButton
        '---------------------------------------------------------------------------
        'Delete is currently disabled due to incomplete requirements.
        '---------------------------------------------------------------------------
        'If (e.Cell.Row.Cells("LabelTypeID").Value = _NEW_TYPE_ID) Then
        '    e.Cell.Row.Delete()
        'Else
        '    Try
        '        Dim currentType As LabelTypeBO = New LabelTypeBO()
        '        currentType.LabelTypeID = e.Cell.Row.Cells("LabelTypeID").Value
        '        currentType.LabelTypeDesc = e.Cell.Row.Cells("LabelTypeDesc").Value
        '        LabelTypeDAO.DeleteLabelType(currentType)
        '        e.Cell.Row.Delete()
        '    Catch ex As Exception
        '        MessageBox.Show("This Label Type cannot be deleted due to items being associated to the Label Type.")
        '    End Try
        'End If
    End Sub
End Class