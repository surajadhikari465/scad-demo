Imports Infragistics.Win.UltraWinGrid
Imports log4net
Imports WholeFoods.IRMA.InvoiceControlGroup.BusinessLogic
Imports WholeFoods.IRMA.InvoiceControlGroup.DataAccess

Public Class InvoiceControlGroupSearch
    ' Define the log4net logger for this class.
    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

#Region "Events raised by this form"
    ''' <summary>
    ''' Notify the calling form of the control group id selected by the user.
    ''' </summary>
    ''' <param name="controlGroupID"></param>
    ''' <remarks></remarks>
    Public Event ControlGroupSelected(ByVal controlGroupID As Integer)
#End Region

    ''' <summary>
    ''' Search for control groups that match the criteria input by the user.
    ''' The data grid is updated with the search results.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Button_Search_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Search.Click
        logger.Debug("Button_Search_Click entry")

        ' Set the data for the search parameters
        Dim searchStatus As Integer = -1
        If RadioButton_AllStatus.Checked Then
            searchStatus = -1
        ElseIf RadioButton_StatusOpen.Checked Then
            searchStatus = 1
        ElseIf RadioButton_ClosedStatus.Checked Then
            searchStatus = 2
        End If
        Dim searchID As Integer = -1
        If (UltraNumericEditor_ControlGroupID.Value IsNot Nothing) AndAlso (UltraNumericEditor_ControlGroupID.Value IsNot DBNull.Value) Then
            searchID = CInt(UltraNumericEditor_ControlGroupID.Value)
        End If
        Dim searchResults As ArrayList = ControlGroupDAO.SearchForControlGroups(searchStatus, searchID)

        ' Refresh the data grid
        UltraGrid_ControlGroups.DataSource = searchResults

        ' Format the data grid
        If UltraGrid_ControlGroups.DisplayLayout.Bands(0).Columns.Count > 0 Then
            UltraGrid_ControlGroups.DisplayLayout.Bands(0).Columns("UpdateUserId").Hidden = True
            UltraGrid_ControlGroups.DisplayLayout.Bands(0).Columns("UpdateTime").Hidden = True
            UltraGrid_ControlGroups.DisplayLayout.Bands(0).Columns("Orders").Hidden = True
            UltraGrid_ControlGroups.DisplayLayout.Bands(0).Columns("OrdersInvoiceTotal").Hidden = True
            UltraGrid_ControlGroups.DisplayLayout.Bands(0).Columns("Status").Hidden = True
            UltraGrid_ControlGroups.DisplayLayout.Bands(0).Columns("CloseStatus").Hidden = True
            UltraGrid_ControlGroups.DisplayLayout.Bands(0).Columns("CloseErrorMsg").Hidden = True

            UltraGrid_ControlGroups.DisplayLayout.Bands(0).Columns("ControlGroupId").Header.VisiblePosition = 1
            UltraGrid_ControlGroups.DisplayLayout.Bands(0).Columns("ControlGroupId").Header.Caption = "ID"
            UltraGrid_ControlGroups.DisplayLayout.Bands(0).Columns("StatusDesc").Header.VisiblePosition = 2
            UltraGrid_ControlGroups.DisplayLayout.Bands(0).Columns("StatusDesc").Header.Caption = "Status"
            UltraGrid_ControlGroups.DisplayLayout.Bands(0).Columns("ExpectedGrossAmt").Header.VisiblePosition = 3
            UltraGrid_ControlGroups.DisplayLayout.Bands(0).Columns("ExpectedGrossAmt").Header.Caption = "Expect Gross Amt"
            UltraGrid_ControlGroups.DisplayLayout.Bands(0).Columns("ExpectedGrossAmt").Format = "$###,###,##0.00"
            UltraGrid_ControlGroups.DisplayLayout.Bands(0).Columns("ExpectedInvoiceCount").Header.VisiblePosition = 4
            UltraGrid_ControlGroups.DisplayLayout.Bands(0).Columns("ExpectedInvoiceCount").Header.Caption = "Expect # Inv"
            UltraGrid_ControlGroups.DisplayLayout.Bands(0).Columns("UpdateUserName").Header.VisiblePosition = 5
            UltraGrid_ControlGroups.DisplayLayout.Bands(0).Columns("UpdateUserName").Header.Caption = "Last Updated"

            ' Format the grid
            UltraGrid_ControlGroups.DisplayLayout.Bands(0).Override.ExpansionIndicator = ShowExpansionIndicator.Never
            UltraGrid_ControlGroups.DisplayLayout.Bands(0).Override.MaxSelectedRows = 1
            UltraGrid_ControlGroups.DisplayLayout.Bands(0).Override.CellClickAction = CellClickAction.RowSelect

            ' Default the first row to selected
            If UltraGrid_ControlGroups.Rows.Count > 0 Then
                UltraGrid_ControlGroups.Rows(0).Selected = True
            End If
        Else
            ' Let the user know that no matching results were found
            MsgBox("No control groups were found matching your search criteria.", MsgBoxStyle.OkOnly, Me.Text)
        End If
        logger.Debug("Button_Search_Click exit")
    End Sub

    ''' <summary>
    ''' The calling form is notified via the ControlGroupSelected event, and the
    ''' form is closed.    
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SelectRow()
        logger.Debug("SelectRow entry")
        '-- Make sure just one item was selected
        If UltraGrid_ControlGroups.Selected.Rows.Count = 1 Then
            ' Notify the calling form of the selected control group id.
            Dim selectedID As Integer = CInt(UltraGrid_ControlGroups.Selected.Rows(0).Cells("ControlGroupId").Value)
            logger.Info("A control group was selected from the search results: " + selectedID.ToString())
            RaiseEvent ControlGroupSelected(selectedID)
            ' Close the form.
            Me.Close()
        Else
            ' Alert the user that they must select at least one control group
            MsgBox("A control group from the list must be selected.", MsgBoxStyle.Exclamation, Me.Text)
        End If
        logger.Debug("SelectRow exit")
    End Sub

    ''' <summary>
    ''' The user selects a control group.  
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Button_Select_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Select.Click
        logger.Debug("Button_Select_Click entry")
        SelectRow()
        logger.Debug("Button_Select_Click exit")
    End Sub

    ''' <summary>
    ''' Process the exit action.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Button_Exit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Exit.Click
        ' Simply close the form.  If a control group was not selected during the search, the calling form is not updated.
        logger.Debug("Button_Exit_Click entry")
        logger.Debug("Button_Exit_Click exit")
    End Sub

    ''' <summary>
    ''' Process the form closing action.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub InvoiceControlGroupSearch_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        ' Simply close the form.  If a control group was not selected during the search, the calling form is not updated.
        logger.Debug("InvoiceControlGroupSearch_FormClosing entry")
        logger.Debug("InvoiceControlGroupSearch_FormClosing exit")
    End Sub

    ''' <summary>
    ''' The double click action for a row performs the same action as the select button.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub UltraGrid_ControlGroups_DoubleClickRow(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.DoubleClickRowEventArgs) Handles UltraGrid_ControlGroups.DoubleClickRow
        logger.Debug("UltraGrid_ControlGroups_DoubleClickRow entry")
        SelectRow()
        logger.Debug("UltraGrid_ControlGroups_DoubleClickRow exit")
    End Sub
End Class