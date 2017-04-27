Imports WholeFoods.IRMA.ItemHosting.DataAccess
Imports WholeFoods.IRMA.ItemHosting.BusinessLogic
Imports WholeFoods.IRMA.Common.DataAccess
Imports log4net
Imports Infragistics.Win
Imports Infragistics.Win.UltraWinGrid

Public Class ManageDefaultAttributes
    ' define logger
    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

#Region "Constant, Field, Enum, and Property Definitions"

    Private Const TEXTBOX_WIDTH As Integer = 150
    Private Const DROPDOWN_WIDTH As Integer = 150
    Private Const CONTROL_X As Integer = 127
    Private Const CONTROL_SPACER As Integer = 26
    Private Const CONTROL_HEIGHT As Integer = 20
    Private Const CHECKBOX_WIDTH As Integer = 81
    Private Const CHECKBOX_SPACER As Integer = 23


    Public Enum SaveOrDeleteActions
        Save
        Delete
    End Enum

    Private _attributeControls As Hashtable = New Hashtable
    Private _allItemDefaultAttributes As DataTable

    Private _attributeValueChanged As Boolean = False

    Public Property AttributeValueChanged() As Boolean
        Get
            Return _attributeValueChanged
        End Get
        Set(ByVal value As Boolean)
            _attributeValueChanged = value

            Me.cmdApplyChanges.Enabled = value

        End Set
    End Property

#End Region


#Region "Private Methods"

    Private Sub ManageDefaultAttributes_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        logger.Debug("ManageDefaultAttributes_Load Entry")

        BuildAttributeControls()

        logger.Debug("ManageDefaultAttributes_Load Exit")
    End Sub

    Private Sub BuildAttributeControls()
        logger.Debug("BuildAttributeControls Entry")

        Try

            ' this is reset to default in the Finally block below
            Me.Cursor = Cursors.WaitCursor

            _allItemDefaultAttributes = ItemDefaultAttributeDAO.GetAllItemDefaultAttributes()

            '    (Me.HierarchySelector1.SelectedLevel4Id, Me.HierarchySelector1.SelectedCategoryId)

            'Dim itemDefaultAttribute As ItemDefaultAttributeManagerBO

            Dim typeString As String = "Text Field"

            'For Each itemDefaultAttribute In _allItemDefaultAttributes
            '    Select Case itemDefaultAttribute.ControlType
            '        Case 1 ' Text field
            '            typeString = "Text Field"
            '        Case 2 ' Dropdown
            '            typeString = "Dropdown List"
            '        Case 3 ' Checkbox
            '            typeString = "Checkbox"
            '    End Select



            'Next

        Catch ex As Exception
            Dim message As String = ex.Message

            If Not IsNothing(ex.InnerException) Then
                message = ex.InnerException.Message
            End If
            MessageBox.Show("The following error has occured. Please report this to the IRMA support team." + _
                ControlChars.NewLine + ControlChars.NewLine + message)

            logger.Info("The following error has occured. Please report this to the IRMA support team." + _
                ControlChars.NewLine + ControlChars.NewLine + message)

        Finally

            Me.Cursor = Cursors.Default

        End Try

        Me.ugridAttributes.DataSource = _allItemDefaultAttributes

        logger.Debug("BuildAttributeControls Exit")
    End Sub

    Private Sub HandleAttributeValueChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        ' AttributeValueChanged = True

    End Sub

    Private Function CheckForChange() As DialogResult

        logger.Debug("CheckForChange Entry")


        Dim aDialogResult As DialogResult = Windows.Forms.DialogResult.None

        If AttributeValueChanged() Then

            aDialogResult = MessageBox.Show("Save your Changes?", "Default Attribute Values", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question)

        End If

        logger.Debug("CheckForChange Exit")

        Return aDialogResult

    End Function


    Private Sub SaveChanges()

        logger.Debug("SaveChanges entry")

        Dim row As UltraGridRow = Nothing
        Dim rowTypes As GridRowType = GridRowType.DataRow
        Dim band As UltraGridBand = ugridAttributes.DisplayLayout.Bands(0)
        Dim enumerator As IEnumerable = band.GetRowEnumerator(rowTypes)
        Dim order As Integer = 1

        Dim attributeId As Integer = Nothing
        Dim attributeName As String = Nothing
        Dim active As Boolean = False


        For Each row In enumerator
            ' Get the appropriate values
            attributeId = row.Cells(0).Value
            attributeName = row.Cells(1).Value
            active = row.Cells(3).Value
            Try
                'call update
                ItemDefaultAttributeDAO.UpdateItemDefaultAttribute(attributeId, attributeName, active, order)

            Catch ex As Exception
                Dim message As String = ex.Message

                If Not IsNothing(ex.InnerException) Then
                    message = ex.InnerException.Message
                End If
                MessageBox.Show("The following error has occured. Please report this to the IRMA support team." + _
                    ControlChars.NewLine + ControlChars.NewLine + message)

                logger.Info("The following error has occured. Please report this to the IRMA support team." + _
                    ControlChars.NewLine + ControlChars.NewLine + message)

            Finally
                ' reset our vars
                attributeId = Nothing
                attributeName = Nothing
                active = False
                order = order + 1
            End Try

        Next

        AttributeValueChanged = False

        logger.Debug("SaveChanges exit")

    End Sub

#End Region

#Region "Events"

    ' for handling the drag/drop sorting
    Private Sub ugridAttributes_SelectionDrag(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ugridAttributes.SelectionDrag, ugridAttributes.SelectionDrag
        ugridAttributes.DoDragDrop(ugridAttributes.Selected.Rows, DragDropEffects.Move)
    End Sub


    Private Sub ugridAttributes_DragOver(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles ugridAttributes.DragOver
        e.Effect = DragDropEffects.Move
        Dim grid As UltraGrid = TryCast(sender, UltraGrid)
        Dim pointInGridCoords As Point = grid.PointToClient(New Point(e.X, e.Y))

        If pointInGridCoords.Y < 20 Then
            'Scroll up
            Me.ugridAttributes.ActiveRowScrollRegion.Scroll(RowScrollAction.LineUp)
        ElseIf pointInGridCoords.Y > grid.Height - 20 Then
            'Scroll down
            Me.ugridAttributes.ActiveRowScrollRegion.Scroll(RowScrollAction.LineDown)
        End If
    End Sub

    Private Sub ugridAttributes_DragDrop(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles ugridAttributes.DragDrop
        Dim dropIndex As Integer

        'Get the position on the grid where the dragged row(s) are to be dropped. 
        'get the grid coordinates of the row (the drop zone) 
        Dim uieOver As UIElement = ugridAttributes.DisplayLayout.UIElement.ElementFromPoint(ugridAttributes.PointToClient(New Point(e.X, e.Y)))

        'get the row that is the drop zone/or where the dragged row is to be dropped 
        Dim ugrOver As UltraGridRow = TryCast(uieOver.GetContext(GetType(UltraGridRow), True), UltraGridRow)

        If ugrOver IsNot Nothing Then
            dropIndex = ugrOver.Index    'index/position of drop zone in grid 

            'get the dragged row(s)which are to be dragged to another position in the grid 
            Dim SelRows As SelectedRowsCollection = TryCast(DirectCast(e.Data.GetData(GetType(SelectedRowsCollection)), SelectedRowsCollection), SelectedRowsCollection)
            'get the count of selected rows and drop each starting at the dropIndex 
            For Each aRow As UltraGridRow In SelRows
                'move the selected row(s) to the drop zone 
                ugridAttributes.Rows.Move(aRow, dropIndex)
            Next
            ' we have changed something
            AttributeValueChanged = True

        End If
    End Sub



    Private Sub cmdExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdExit.Click

        logger.Debug("cmdExit_Click Entry")

        Dim checkForChangeDialogResult As DialogResult = CheckForChange()

        If checkForChangeDialogResult <> Windows.Forms.DialogResult.Cancel Then

            If checkForChangeDialogResult = Windows.Forms.DialogResult.Yes Then
                SaveChanges()
            End If

            Me.Close()

        End If
        logger.Debug("cmdExit_Click Exit")


    End Sub

    Private Sub cmdApplyChanges_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdApplyChanges.Click

        SaveChanges()

    End Sub

    Private Sub ugridAttributes_CellChange(ByVal sender As System.Object, ByVal e As Infragistics.Win.UltraWinGrid.CellEventArgs) Handles ugridAttributes.CellChange
        AttributeValueChanged = True
    End Sub

    Private Sub ugridAttributes_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles ugridAttributes.KeyPress
        If Not (Char.IsLetter(e.KeyChar) Or Char.IsWhiteSpace(e.KeyChar) Or Char.IsControl(e.KeyChar)) Then
            e.Handled = True
        End If
    End Sub

#End Region
End Class