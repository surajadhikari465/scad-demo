Imports WholeFoods.IRMA.Administration.EInvoicing.BusinessLogic
Imports WholeFoods.IRMA.Administration.EInvoicing.DataAccess
Imports WholeFoods.IRMA.Replenishment.EInvoicing.BusinessLogic
Imports WholeFoods.IRMA.Replenishment.EInvoicing.DataAccess
Imports Infragistics.Win





Public Class EInvoicing_SAC_Edit

    Private Sub EInvoicing_SAC_Edit_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        RefreshData()
    End Sub

    Private Sub _RefreshData()

        Dim BO As EInvoicingConfigBO = New EInvoicingConfigBO


        With UltraGrid_EInvoicing
            .DataSource = Nothing
            .ResetDisplayLayout()
            .DataSource = BO.getConfigInfo

        End With


        Formatdata()

    End Sub

    Private Sub Formatdata()
        With UltraGrid_EInvoicing
            .DisplayLayout.Bands(0).Override.AllowAddNew = UltraWinGrid.AllowAddNew.No
            .DisplayLayout.Bands(0).Override.AllowUpdate = DefaultableBoolean.False
            .DisplayLayout.Bands(0).Override.CellClickAction = UltraWinGrid.CellClickAction.RowSelect
            .DisplayLayout.Bands(0).Override.ColumnAutoSizeMode = UltraWinGrid.ColumnAutoSizeMode.AllRowsInBand

            .DisplayLayout.Bands(0).Columns("IsHeaderElement").Hidden = True
            .DisplayLayout.Bands(0).Columns("IsItemElement").Hidden = True
            .DisplayLayout.Bands(0).Columns("IsAllowance").Hidden = True
            .DisplayLayout.Bands(0).Columns("SubTeam_No").Hidden = True
            .DisplayLayout.Bands(0).Columns("SacCodeType").Hidden = True
            '.DisplayLayout.Bands(0).Columns("Disabled").Hidden = True
            .DisplayLayout.Bands(0).Columns("NeedsConfig").Hidden = True



        End With
    End Sub


    Private Sub UltraGrid1_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles UltraGrid_EInvoicing.DoubleClick

        'Cast the sender into an UltraGrid
        Dim grid As UltraWinGrid.UltraGrid = DirectCast(sender, UltraWinGrid.UltraGrid)

        'Get the last element that the mouse entered
        Dim lastElementEntered As UIElement = grid.DisplayLayout.UIElement.LastElementEntered

        'See if there's a RowUIElement in the chain.
        Dim rowElement As UltraWinGrid.RowUIElement
        If TypeOf lastElementEntered Is UltraWinGrid.RowUIElement Then
            rowElement = DirectCast(lastElementEntered, UltraWinGrid.RowUIElement)
        Else
            rowElement = DirectCast(lastElementEntered.GetAncestor(GetType(UltraWinGrid.RowUIElement)), UltraWinGrid.RowUIElement)
        End If

        If rowElement Is Nothing Then Return

        'Try to get a row from the element
        Dim row As UltraWinGrid.UltraGridRow = DirectCast(rowElement.GetContext(GetType(UltraWinGrid.UltraGridRow)), UltraWinGrid.UltraGridRow)

        'If no row was returned, then the mouse is not over a row. 
        If (row Is Nothing) Then Return

        'The mouse is over a row. 

        'This part is optional, but if the user double-clicks the line 
        'between Row selectors, the row is AutoSized by 
        'default. In that case, we probably don't want to do 
        'the double-click code.

        'Get the current mouse pointer location and convert it
        'to grid coords. 
        Dim MousePosition As Point = grid.PointToClient(Control.MousePosition)

        'See if the Point is on an AdjustableElement - meaning that
        'the user is clicking the line on the row selector
        If Not lastElementEntered.AdjustableElementFromPoint(MousePosition) Is Nothing Then Return

        'Everthing looks go, so display a message based on the row. 

        Dim _ConfigValue As EInvoicingConfigBO = New EInvoicingConfigBO
        With _ConfigValue
            .ElementName = row.Cells("ElementName").Value.ToString()
            .IsAllowance = DirectCast(IIf(row.Cells("IsAllowance").Value.ToString().Equals("1"), True, False), Boolean)
            .IsSAC = DirectCast(IIf(row.Cells("IsSACCode").Value.ToString().Equals("True"), True, False), Boolean)
            .Label = row.Cells("Label").Value.ToString()
            If row.Cells("IsHeaderElement").Value.ToString().Equals("True") Then
                .IsHeaderElement = True
            End If

            .Disabled = row.Cells("Disabled").Value.ToString().Equals("True")

            If row.Cells("IsItemElement").Value.ToString().Equals("True") Then
                .IsItemElement = True
            End If

            If Not row.Cells("SubTeam_No").Value Is DBNull.Value Then
                .SubTeam_NO = DirectCast(row.Cells("SubTeam_No").Value, Integer)
            Else
                .SubTeam_NO = -1
            End If
            If Not row.Cells("SacCodeType").Value Is DBNull.Value Then
                .SACType = Integer.Parse(row.Cells("SacCodeType").Value.ToString())
            Else
                .SACType = -1
            End If
        End With

        Dim frm As EInvoicing_SAC_Modify = New EInvoicing_SAC_Modify(_ConfigValue, Me)

        frm.ShowDialog()
        frm.Dispose()
        '_RefreshData()
        '        MessageBox.Show(Me, "Edit:" + row.Cells("ElementName").Value.ToString())
    End Sub


    Private Sub Button_Add_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Add.Click
        Dim frm As EInvoicing_SAC_Modify = New EInvoicing_SAC_Modify(Me)
        frm.ShowDialog()
        frm.Dispose()


    End Sub

    Private Sub Button_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Close.Click
        Me.Close()
        Me.Dispose()
    End Sub

    Public Sub RefreshData()
        _RefreshData()
    End Sub

    Private Sub Button_Remove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Remove.Click

        Dim dao As EinvoicingConfigDAO = New EinvoicingConfigDAO


        With UltraGrid_EInvoicing
            If .Selected.Rows.Count > 0 Then
                For Each row As UltraWinGrid.UltraGridRow In .Selected.Rows
                    dao.RemoveConfigvalue(row.Cells("ElementName").Value.ToString())
                Next
            Else
                MsgBox("You must select 1 or more rows to remove a configuration value.:", MsgBoxStyle.Exclamation, "Error")
            End If
        End With

        RefreshData()

    End Sub
End Class
