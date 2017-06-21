Imports System.Text.RegularExpressions
Imports System.IO

Imports Infragistics.Win
Imports Infragistics.Win.UltraWinGrid
Imports Infragistics.Win.UltraWinGrid.UltraGridAction

Imports WholeFoods.IRMA.ExtendedItemMaintenance
Imports WholeFoods.IRMA.ModelLayer
Imports WholeFoods.IRMA.ModelLayer.BusinessLogic
Imports WholeFoods.IRMA.ModelLayer.DataAccess
Imports WholeFoods.IRMA.ExtendedItemMaintenance.Logic
Imports WholeFoods.IRMA.ItemBulkLoad.DataAccess
Imports WholeFoods.IRMA.ItemBulkLoad.BusinessLogic
Imports WholeFoods.IRMA.Common
Imports WholeFoods.IRMA.ItemHosting.BusinessLogic
Imports WholeFoods.IRMA.ExtendedItemMaintenance.Logic.Utilites
Imports WholeFoods.IRMA.Common.DataAccess
Imports WholeFoods.IRMA.ItemHosting.DataAccess
Imports WholeFoods.Utility

'EIM Main Form
Public Class ExtendedItemMaintenanceForm

#Region "Fields and Properties"

    Private WithEvents _extendedItemMaintenanceManager As New ExtendedItemMaintenanceManager()
    Private _itemLoadDataSet As DataSet
    Private _currentSessionAction As SessionActions = SessionActions.SaveNew
    Private _hasChanged As Boolean = False
    Private _isUploadSuccesful As Boolean = False
    Private _anErrorHasOccurred As Boolean = False
    Private _allowUpload As Boolean = True
    Private _loadValueListDataProgressComplete As Boolean = True

    Private _calculatingCellValues As Boolean = False
    Private _isInitializingRow As Boolean = False
    Private _ignoreSessionTypeChange As Boolean = False
    Private _ignoreUploadToItemStoreChange As Boolean = False

    ''' <summary>
    ''' Is set to false if an error occurs during session saving or validating.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property AllowUpload() As Boolean
        Get
            Return _allowUpload
        End Get
        Set(ByVal value As Boolean)
            _allowUpload = value

        End Set
    End Property

    ''' <summary>
    ''' Is set to true if any error occurs during a manjor action.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property AnErrorHasOccurred() As Boolean
        Get
            Return _anErrorHasOccurred
        End Get
        Set(ByVal value As Boolean)
            _anErrorHasOccurred = value
        End Set
    End Property

    Public Property CalculatingCellValues() As Boolean
        Get
            Return _calculatingCellValues
        End Get
        Set(ByVal value As Boolean)
            _calculatingCellValues = value
        End Set
    End Property

    Public Property EIMManager() As ExtendedItemMaintenanceManager
        Get
            Return _extendedItemMaintenanceManager
        End Get
        Set(ByVal value As ExtendedItemMaintenanceManager)
            _extendedItemMaintenanceManager = value
        End Set
    End Property

    Public Property ItemLoadDataSet() As DataSet
        Get
            Return _itemLoadDataSet
        End Get
        Set(ByVal value As DataSet)
            _itemLoadDataSet = value
        End Set
    End Property

    Public Property CurrentSessionAction() As SessionActions
        Get
            Return _currentSessionAction
        End Get
        Set(ByVal value As SessionActions)
            _currentSessionAction = value
        End Set
    End Property

    Public Property HasChanged() As Boolean
        Get
            Return _hasChanged
        End Get
        Set(ByVal value As Boolean)
            _hasChanged = value

            ManageUIAppearance()
        End Set
    End Property

    Public Property LoadValueListDataProgressComplete() As Boolean
        Get
            Return _loadValueListDataProgressComplete
        End Get
        Set(ByVal value As Boolean)
            _loadValueListDataProgressComplete = value
        End Set
    End Property

#End Region

#Region "Event Handlers"

    Private Sub ExtendedItemMaintenanceForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim oldCursor As Cursor = Me.Cursor
        Me.Cursor = Cursors.WaitCursor

        Try
            Me.Cursor = Cursors.WaitCursor

            Me.EIMManager.EIMForm = Me

            Me.StoreSelectorPriceUpload.CurrentUploadTypeCode = EIM_Constants.PRICE_UPLOAD_CODE
            Me.StoreSelectorPriceUpload.OtherStoreSelector = Me.StoreSelectorCostUpload

            Me.StoreSelectorCostUpload.CurrentUploadTypeCode = EIM_Constants.COST_UPLOAD_CODE
            Me.StoreSelectorCostUpload.OtherStoreSelector = Me.StoreSelectorPriceUpload

            BindAndPopulateControls()

            ' position and size various Item Load Search controls
            ' appropriate to the instance data flag values
            Dim useFourLevelHierarchy As Boolean = InstanceDataDAO.IsFlagActive("FourLevelHierarchy")
            Dim useStoreJurisdictions As Boolean = InstanceDataDAO.IsFlagActive("UseStoreJurisdictions")

            If useFourLevelHierarchy Then
                Me.HierarchySelectorItemLoad.Height = 117
                Me.PanelSearchButtons.Top = 139
            Else
                Me.HierarchySelectorItemLoad.Height = 70

                If Not useStoreJurisdictions Then
                    Me.PanelSearchButtons.Top = 115
                End If
            End If

            Me.PanelJurisdiction.Visible = useStoreJurisdictions

            Me.HasChanged = False

        Catch ex As Exception
            ShowErrorDialog(ex, "An EIM Error Has Occurred", "An error occurred while openning the EIM form. EIM may not be correctly configured in the database.")
        Finally
            Me.Cursor = oldCursor
        End Try

    End Sub

    Private Sub ExtendedItemMaintenanceForm_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing

        e.Cancel = AskToSaveOrCancel()

        If Not e.Cancel Then

            ' clean up the big memory users
            Me.EIMManager.CurrentUploadSession = Nothing
            Me.EIMManager.CurrentUploadRowHolderCollecton = Nothing

        End If

    End Sub

    Private Sub ButtonImport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonImport.Click

        If Not AskToSaveOrCancel() Then
            SpreadsheetImport()
        End If

    End Sub

    Private Sub ButtonExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonExport.Click

        SpreadsheetExport()

    End Sub

    Private Sub ButtonSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonSave.Click

        SessionSave(False, False)

    End Sub

    Private Sub ButtonSessionSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonSessionSearch.Click
        Dim oldCursor As Cursor = Me.Cursor
        Me.Cursor = Cursors.WaitCursor

        Try
            SessionSearch(True)
        Finally
            Me.Cursor = oldCursor
        End Try

    End Sub

    Private Sub ButtonSessionLoad_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonSessionLoad.Click

        SessionLoad()

    End Sub

    Private Sub ButtonSessionDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonSessionDelete.Click

        If UltraGridUploadSessions.Selected.Rows.Count > 0 Then
            If MessageBox.Show("Delete the selected " + UltraGridUploadSessions.Selected.Rows.Count.ToString() + " sessions and all related data?" + _
                ControlChars.NewLine + ControlChars.NewLine + "Uploaded Sessions will not be deleted.", _
                "EIM - Delete Session", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.OK Then

                For Each theGridRow As UltraGridRow In UltraGridUploadSessions.Selected.Rows
                    SessionCascadeDelete(Integer.Parse(theGridRow.Cells(0).Value.ToString()))
                Next

                SessionSearch(False)
            End If
        End If

    End Sub

    Private Sub ButtonItemSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonItemSearch.Click

        ItemSearch()

    End Sub

    Private Sub ButtonItemLoad_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonItemLoad.Click

        If Me.UltraGridItems.Rows.Count > 0 Then
            If Not AskToSaveOrCancel() Then
                ItemLoad()
            End If
        End If

    End Sub

    Private Sub ButtonUpload_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonUpload.Click

        SessionUpload()

    End Sub

    Private Sub UltraGridSearchResults_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UltraGridUploadSessions.DoubleClick
        'Cast the sender into an UltraGrid
        Dim grid As UltraGrid = DirectCast(sender, UltraGrid)

        'Get the last element that the mouse entered
        Dim lastElementEntered As UIElement = grid.DisplayLayout.UIElement.LastElementEntered

        'See if there's a RowUIElement in the chain.
        Dim rowElement As RowUIElement
        If TypeOf lastElementEntered Is RowUIElement Then
            rowElement = DirectCast(lastElementEntered, RowUIElement)
        Else
            rowElement = DirectCast(lastElementEntered.GetAncestor(GetType(RowUIElement)), RowUIElement)
        End If

        If rowElement Is Nothing Then Return

        'Try to get a row from the element
        Dim row As UltraGridRow = DirectCast(rowElement.GetContext(GetType(UltraGridRow)), UltraGridRow)

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

        SessionLoad()

    End Sub

    ''' <summary>
    ''' Mark the UploadValues and UploadRows corresponding to the deleted grid rows
    ''' for delete.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub UltraGridUpload_BeforeRowsDeleted(ByVal sender As System.Object, _
            ByVal e As Infragistics.Win.UltraWinGrid.BeforeRowsDeletedEventArgs) _
                Handles UltraGridItemMaintenance.BeforeRowsDeleted, _
                UltraGridPriceUpload.BeforeRowsDeleted, _
                UltraGridCostUpload.BeforeRowsDeleted

        If CType(sender, UltraGrid) Is Me.UltraGridItemMaintenance AndAlso _
                    (Me.UltraGridPriceUpload.Rows.Count > 0 Or Me.UltraGridCostUpload.Rows.Count > 0) Then

            MessageBox.Show("If there are rows in the Prices and Costs grids you must delete rows from there.", _
                            "EIM - Delete Upload Row", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Else

            If MessageBox.Show("This will delete the " + e.Rows.Length.ToString() + " selected row(s) in all upload grids." + _
                ControlChars.NewLine + "An item in the Items grid will not be deleted " + ControlChars.NewLine + _
                "until all its rows in the Price and Cost grids are." + ControlChars.NewLine + ControlChars.NewLine + "Continue?", _
                "EIM - Delete Upload Row", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.OK Then

                Dim theRowIdToDelete As Integer = 0
                Dim theUploadRow As UploadRow = Nothing

                ' loop through the selected rows
                For Each theUltraGridRow As UltraGridRow In e.Rows

                    ' find the UploadRow for the grid row
                    theRowIdToDelete = GetUploadRowIdForGridRow(theUltraGridRow)

                    ' remove corresponding item row from the * ALL * the grids
                    ' and cancel the grid-based delete below
                    ' this works better
                    DeleteRowFromAllGrids(theRowIdToDelete)
                Next

            End If

            Me.EIMManager.Validate(ValidationTypes.GridCell, False)

        End If

        ' don't let the grid do the delete itself or
        ' display the default message
        e.Cancel = True
        e.DisplayPromptMsg = False

        Me.HasChanged = True

    End Sub

    Private Sub CheckBoxHideMultipleItemMaintRows_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBoxHideMultipleItemMaintRows.CheckedChanged


        If Not IsNothing(Me.EIMManager.CurrentUploadSession.DataSet. _
            Tables(EIM_Constants.ITEM_MAINTENANCE_CODE)) Then

            ' refresh all the rows of the Item Maintenance grid
            ' to show or hide multiple rows for the same item as
            ' appropriate
            For Each theGridRow As UltraGridRow In Me.UltraGridItemMaintenance.Rows

                theGridRow.Refresh(RefreshRow.FireInitializeRow)
            Next

            ' commit the changes to the DataTable when hiding and showing rows
            Me.EIMManager.CurrentUploadSession.DataSet. _
                        Tables(EIM_Constants.ITEM_MAINTENANCE_CODE).AcceptChanges()

            ManageUIAppearance()
        End If


    End Sub

    ''' <summary>
    ''' Make all rows with invalid item keys look disabled
    ''' and hide all but one row per item. However, the one not hidden should have the lowest validation level,
    ''' or if the validation levels are all equal then do not hide the one not marked as identical.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub UltraGridItemMaintenance_InitializeRow(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.InitializeRowEventArgs) _
            Handles UltraGridItemMaintenance.InitializeRow

        Try
            Me._isInitializingRow = True

            Dim theUploadRowHolder As UploadRowHolder = GetUploadRowForGridRow(e.Row)

            e.Row.Hidden = False
            e.Row.Cells(EIM_Constants.IS_HIDDEN_COLUMN_NAME).Value = False

            If Not IsNothing(theUploadRowHolder) Then

                Dim isRowIdenticalToAnother As Boolean = CBool(e.Row.Cells(EIM_Constants.IS_IDENTICAL_ITEM_ROW_COLUMN_NAME).Value)

                Dim areAllEqual As Boolean = True
                Dim hasLowestValidationLevel As Boolean = Me.EIMManager.CurrentUploadRowHolderCollecton.HideGridRowInItemMaintenance(theUploadRowHolder, areAllEqual)

                Dim doNotHideRow As Boolean = (Not areAllEqual And hasLowestValidationLevel) Or (areAllEqual And Not isRowIdenticalToAnother)

                If Not doNotHideRow Then

                    If CheckBoxHideMultipleItemMaintRows.Checked Then

                        ' do not hide the row with the lowest validation level
                        e.Row.Hidden = True
                        e.Row.Cells(EIM_Constants.IS_HIDDEN_COLUMN_NAME).Value = True
                    Else
                        ' e.Row.Appearance.AlphaLevel = 75
                        e.Row.ToolTipText = "This row is for the same item as another."
                    End If
                Else
                    e.Row.Appearance.BackColor = Color.White
                    e.Row.Appearance.AlphaLevel = 0
                    e.Row.ToolTipText = ""
                End If

            End If
        Finally
            Me._isInitializingRow = False
        End Try

    End Sub

    Private Sub TabControlUpoadPages_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TabControlUpoadPages.SelectedIndexChanged

        ' set the focus on the correct control according to the tab that's
        ' on top
        If Me.TabControlUpoadPages.SelectedTab Is Me.TabPageHistory Then
            Me.NumericEditorSessionID.Focus()
        ElseIf Me.TabControlUpoadPages.SelectedTab Is Me.TabPageLoadItems Then
            Me.UltraTextEditorIdentifier.Focus()
            ' configure the hierarchy control
            Me.HierarchySelectorItemLoad.SetAddHierarchLevelsActive(False)
        End If

    End Sub

    ''' <summary>
    ''' *** The is a critical event handler in EIM.
    ''' It executes whenever the user changes a value of a cell in one of the
    ''' session data grids to perform various functions, including validation.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub UltraGridUpload_AfterCellUpdate(ByVal sender As System.Object, ByVal e As Infragistics.Win.UltraWinGrid.CellEventArgs) _
            Handles UltraGridItemMaintenance.AfterCellUpdate, UltraGridPriceUpload.AfterCellUpdate, UltraGridCostUpload.AfterCellUpdate


        '************ If upload exclusion check box for row with validation level "warning" was checked, we need to remember that

        Dim theAttributeKey As String = e.Cell.Column.Key
        Dim theCurrentGridRow As UltraGridRow = e.Cell.Row
        Dim theUploadRowHolder As UploadRowHolder = GetUploadRowForGridRow(theCurrentGridRow)

        ' We cannot go to handleCellUpdate if upload_exclusion was checked by a user after ValidateForUpload 
        ' because we will lose warnings generated by the ValidateForUpload since
        ' HandleCellUpdate will recalculate the grid row and wipe out validations left from ValidateForUpload.

        If theAttributeKey.Equals(EIM_Constants.UPLOAD_EXCLUSION_COLUMN) Then

            If theUploadRowHolder.ValidationWarnings.Count > 0 Then
                If e.Cell.Value = True Then
                    If Not theUploadRowHolder.ValidationErrors.Count > 0 Then
                        theUploadRowHolder.UploadRow.isUploadExclusionWarningChecked = True
                    End If
                Else
                    theUploadRowHolder.UploadRow.isUploadExclusionWarningChecked = False
                End If

            End If

            '************
        Else

            HandleCellUpdate(e.Cell, True)

        End If

    End Sub

    Private Sub UltraGrid_AfterEnterEditMode(ByVal sender As System.Object, ByVal e As System.EventArgs) _
            Handles UltraGridItemMaintenance.AfterEnterEditMode, UltraGridPriceUpload.AfterEnterEditMode, _
            UltraGridCostUpload.AfterEnterEditMode

        Dim theGrid As UltraGrid = CType(sender, UltraGrid)
        Dim theControl As EmbeddableEditorBase = theGrid.ActiveCell.EditorResolved

        ' default null start dates to today and end dates to tomorrow
        If theGrid.ActiveCell.Column.Key.Equals(EIM_Constants.PRICE_SALE_START_DATE_ATTR_KEY) OrElse _
                theGrid.ActiveCell.Column.Key.Equals(EIM_Constants.PRICE_START_DATE_ATTR_KEY) OrElse _
                theGrid.ActiveCell.Column.Key.Equals(EIM_Constants.COST_START_DATE_ATTR_KEY) Then
            If CType(theControl, DateTimeEditor).Value.Equals(DBNull.Value) Then
                CType(theControl, DateTimeEditor).Value = Date.Now
            End If
        ElseIf theGrid.ActiveCell.Column.Key.Equals(EIM_Constants.PRICE_SALE_END_DATE_ATTR_KEY) OrElse _
                theGrid.ActiveCell.Column.Key.Equals(EIM_Constants.COST_END_DATE_ATTR_KEY) Then
            If CType(theControl, DateTimeEditor).Value.Equals(DBNull.Value) Then
                CType(theControl, DateTimeEditor).Value = Date.Now.AddDays(1)
            End If
        End If
    End Sub

    Private Sub UltraGrid_DoubleClickCell(ByVal sender As System.Object, ByVal e As DoubleClickCellEventArgs) _
            Handles UltraGridItemMaintenance.DoubleClickCell, UltraGridCostUpload.DoubleClickCell

        HandlePopUpDataEntryForms(CType(sender, UltraGrid))
    End Sub

    Private Sub ButtonSaveAs_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonSaveAs.Click

        If MessageBox.Show("Save the current session as a new session?" + _
            ControlChars.NewLine + ControlChars.NewLine + "The original session will not be changed.", _
            "EIM - Save as new Session", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.OK Then

            SessionSave(True, False)
            Me.HasChanged = False

        End If

    End Sub

    Private Sub StoreSelector_SelectionChanged() Handles _
            StoreSelectorPriceUpload.SelectionChanged, StoreSelectorCostUpload.SelectionChanged

        Me.HasChanged = True

    End Sub

    Private Sub CheckBoxUploadToItemStore_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBoxUploadToItemStore.CheckedChanged

        If Not Me._ignoreUploadToItemStoreChange Then

            Dim continueChange As Boolean = True

            If Not Me.CheckBoxUploadToItemStore.Checked And _
                    Me.EIMManager.CurrentUploadSession.IsFromSLIM Then

                continueChange = MessageBox.Show("This session was loaded from SLIM." + ControlChars.NewLine + _
                    "Unchecking this will allow you to upload " + ControlChars.NewLine + _
                    "these items to stores other than those requesting them." + ControlChars.NewLine + ControlChars.NewLine + _
                    "Continue?", "EIM - Change Upload Method", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.OK
            End If

            If Not continueChange Then
                Me._ignoreUploadToItemStoreChange = True
                Me.CheckBoxUploadToItemStore.Checked = Not Me.CheckBoxUploadToItemStore.Checked
                Me._ignoreUploadToItemStoreChange = False
            Else

                Me.StoreSelectorPriceUpload.AllowStoreSelection = Not CheckBoxUploadToItemStore.Checked
                Me.StoreSelectorCostUpload.AllowStoreSelection = Not CheckBoxUploadToItemStore.Checked

                ' enable or disable the store cells as is appropriate in the price grid
                For Each theGridRow As UltraGridRow In Me.UltraGridPriceUpload.Rows
                    If Me.CheckBoxUploadToItemStore.Checked Then
                        GridUtilities.EnableCell(theGridRow, EIM_Constants.STORE_NO_ATTR_KEY)
                    Else
                        GridUtilities.DisableCell(theGridRow, EIM_Constants.STORE_NO_ATTR_KEY)
                    End If
                Next

                ' enable or disable the store cells as is appropriate in the cost grid
                For Each theGridRow As UltraGridRow In Me.UltraGridCostUpload.Rows
                    If Me.CheckBoxUploadToItemStore.Checked Then
                        GridUtilities.EnableCell(theGridRow, EIM_Constants.STORE_NO_ATTR_KEY)
                    Else
                        GridUtilities.DisableCell(theGridRow, EIM_Constants.STORE_NO_ATTR_KEY)
                    End If
                Next

                ' got to revalidate
                ' but only the store column!
                Me._isChangingUploadToStoresCheckBox = True
                ValidateSessionData(ValidationTypes.GridCell)
                Me._isChangingUploadToStoresCheckBox = False


                '02/26/2011 set upload_exclusion checkboxes
                SetUploadExclusionCheckBox()

            End If
        End If

    End Sub

    Private Sub ButtonInvertSelection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonInvertSelection.Click

        Try
            Me.UltraGridItems.BeginUpdate()
            Me.Cursor = Cursors.WaitCursor

            For Each theRow As UltraGridRow In Me.UltraGridItems.Rows

                theRow.Selected = Not theRow.Selected
            Next

        Finally
            Me.UltraGridItems.EndUpdate()
            Me.Cursor = Cursors.Default
        End Try

    End Sub

    Private Sub ButtonStickySelection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonStickSelection.Click

        Try
            Me.UltraGridItems.BeginUpdate()
            Me.Cursor = Cursors.WaitCursor

            For Each theSelectedRow As UltraGridRow In Me.UltraGridItems.Selected.Rows

                theSelectedRow.Cells("STICKY").Value = True
            Next

        Finally
            Me.UltraGridItems.EndUpdate()
            Me.Cursor = Cursors.Default
        End Try

    End Sub

    Private Sub ButtonUnstickySelection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonUnstickSelection.Click

        Try
            Me.UltraGridItems.BeginUpdate()
            Me.Cursor = Cursors.WaitCursor

            For Each theSelectedRow As UltraGridRow In Me.UltraGridItems.Selected.Rows

                theSelectedRow.Cells("STICKY").Value = False
            Next

        Finally
            Me.UltraGridItems.EndUpdate()
            Me.Cursor = Cursors.Default
        End Try

    End Sub

    Private Sub ButtonUnstickyAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonUnstickAll.Click

        Try
            Me.UltraGridItems.BeginUpdate()
            Me.Cursor = Cursors.WaitCursor

            For Each theRow As UltraGridRow In Me.UltraGridItems.Rows

                theRow.Cells("STICKY").Value = False
            Next

        Finally
            Me.UltraGridItems.EndUpdate()
            Me.Cursor = Cursors.Default
        End Try

    End Sub

    Private Sub ButtonSelectSticky_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonSelectSticky.Click

        Try
            Me.UltraGridItems.BeginUpdate()
            Me.Cursor = Cursors.WaitCursor

            For Each theRow As UltraGridRow In Me.UltraGridItems.Rows

                theRow.Selected = CBool(theRow.Cells("STICKY").Value)
            Next

        Finally
            Me.UltraGridItems.EndUpdate()
            Me.Cursor = Cursors.Default
        End Try

    End Sub

    Private Sub ButtonDeleteSelected_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonDeleteSelected.Click

        Try
            Me.UltraGridItems.BeginUpdate()
            Me.Cursor = Cursors.WaitCursor

            Dim theRowsToDelete As New ArrayList
            For Each theSelectedRow As UltraGridRow In Me.UltraGridItems.Selected.Rows

                theRowsToDelete.Add(theSelectedRow)
            Next

            For Each theRow As UltraGridRow In theRowsToDelete

                theRow.Delete(False)
            Next

            Me.ItemLoadDataSet.AcceptChanges()

        Finally
            Me.UltraGridItems.EndUpdate()
            ManageUIAppearance()
            Me.Cursor = Cursors.Default
        End Try

    End Sub

    ''' <summary>
    ''' Allows enter key to open popup data entry forms.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub UltraGrid_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) _
            Handles UltraGridItemMaintenance.KeyPress, UltraGridPriceUpload.KeyPress, UltraGridCostUpload.KeyPress

        ' only for the enter key
        If e.KeyChar = ChrW(13) Then
            HandlePopUpDataEntryForms(CType(sender, UltraGrid))
        End If
    End Sub

    Private Sub ButtonDeleteInvalid_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonDeleteInvalid.Click

        If MessageBox.Show("Delete all invalid rows?", "EIM - Session Data", _
                MessageBoxButtons.OKCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.OK Then

            Try
                Me.UltraGridItemMaintenance.BeginUpdate()
                Me.UltraGridPriceUpload.BeginUpdate()
                Me.UltraGridCostUpload.BeginUpdate()

                Me.Cursor = Cursors.WaitCursor

                Dim theRowsToDelete As New ArrayList
                For Each theUploadRowHolder As UploadRowHolder In Me.EIMManager.CurrentUploadRowHolderCollecton.UploadRowHolderList

                    If theUploadRowHolder.UploadRow.ValidationLevel = EIM_Constants.ValidationLevels.Invalid Then
                        theRowsToDelete.Add(theUploadRowHolder)
                    End If
                Next

                For Each theUploadRowHolder As UploadRowHolder In theRowsToDelete

                    If theUploadRowHolder.UploadRow.ValidationLevel = EIM_Constants.ValidationLevels.Invalid Then
                        DeleteRowFromAllGrids(theUploadRowHolder.UploadRow.UploadRowID)
                    End If
                Next

                If theRowsToDelete.Count > 0 Then
                    Me.HasChanged = True
                End If

                Me.EIMManager.CurrentUploadSession.DataSet.AcceptChanges()
            Finally
                Me.UltraGridItemMaintenance.EndUpdate()
                Me.UltraGridPriceUpload.EndUpdate()
                Me.UltraGridCostUpload.EndUpdate()

                Me.Cursor = Cursors.Default
            End Try
        End If
    End Sub

    Private Sub btnClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClear.Click

        Me.UltraTextEditorIdentifier.Value = ""
        Me.UltraTextEditorDescription.Value = ""
        SetCombo(Me.ComboBoxBrand, 0)
        SetCombo(Me.ComboBoxVendor, 0)
        Me.StoreSelectorItemLoad.ClearSelection()
        Me.HierarchySelectorItemLoad.ClearSelection()

        Me.UltraTextEditorPSVendorID.Text = ""
        Me.UltraTextEditorVendorItemID.Text = ""
        SetCombo(Me.ComboBoxDistSubteam, 0)
        SetCombo(Me.ComboBoxItemChain, 0)
        Me.chkDiscontinued.Checked = False
        Me.chkNotAvailable.Checked = False
        Me.chkIncludeDeletedItems.Checked = False

        SetCombo(Me.ComboBoxIsDefaultJurisdiction, -1)
        SetCombo(Me.ComboBoxJurisdiction, 0)

    End Sub

    Private Sub CheckBoxFromSLIM_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBoxLoadFromSLIM.CheckedChanged

        ' if a SLIM load then disable the search criteria that don't make sense for
        ' SLIM items
        Me.ComboBoxItemChain.Enabled = Not CheckBoxLoadFromSLIM.Checked
        Me.ComboBoxDistSubteam.Enabled = Not CheckBoxLoadFromSLIM.Checked
        Me.chkDiscontinued.Enabled = Not CheckBoxLoadFromSLIM.Checked
        Me.chkIncludeDeletedItems.Enabled = Not CheckBoxLoadFromSLIM.Checked
        Me.chkNotAvailable.Enabled = Not CheckBoxLoadFromSLIM.Checked

        ' if a slim session select all stores as a convenience
        If CheckBoxLoadFromSLIM.Checked Then
            Me.StoreSelectorItemLoad.AllRadioButton.Checked = True
        End If

    End Sub

    Private Sub StoreSelectorItemLoad_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles StoreSelectorItemLoad.Click
        Me.StoreSelectorItemLoad.SettingSelection = False
        Me.StoreSelectorCostUpload.SettingSelection = False
        Me.StoreSelectorPriceUpload.SettingSelection = False
    End Sub

    Private Sub ButtonSelectAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonSelectAll.Click

        For Each theRow As UltraGridRow In Me.UltraGridItems.Rows

            theRow.Selected = True
        Next
    End Sub

    Private Sub ExtendedItemMaintenanceForm_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.ControlKey Then
            Me.StoreSelectorItemLoad.SettingSelection = False
            Me.StoreSelectorCostUpload.SettingSelection = False
            Me.StoreSelectorPriceUpload.SettingSelection = False
        End If
    End Sub

    Private Sub ExtendedItemMaintenanceForm_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown

        Me.StoreSelectorItemLoad.SettingSelection = False

        If e.KeyCode = Keys.D And e.Control Then

            Dim doEIMTraceLogging As Boolean = InstanceDataDAO.IsFlagActive("DoEIMTraceLogging")

            If doEIMTraceLogging Then
                If MessageBox.Show("Turn off EIM Trace Logging?", "EIM Logging", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.OK Then
                    doEIMTraceLogging = False
                End If
            Else
                If MessageBox.Show("Turn on EIM Trace Logging?", "EIM Logging", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.OK Then
                    doEIMTraceLogging = True
                End If
            End If

            InstanceDataDAO.SetInstanceDataFlag("DoEIMTraceLogging", doEIMTraceLogging)

        ElseIf e.KeyCode = Keys.Enter Then

            ItemSearch()

        ElseIf e.Control Then

            Me.StoreSelectorItemLoad.SettingSelection = True
            Me.StoreSelectorCostUpload.SettingSelection = True
            Me.StoreSelectorPriceUpload.SettingSelection = True
            Me.StoreSelectorItemLoad.ManualRadioButton.Checked = True
            Me.StoreSelectorCostUpload.ManualRadioButton.Checked = True
            Me.StoreSelectorPriceUpload.ManualRadioButton.Checked = True


        End If

    End Sub

    Private Sub UltraGridItems_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles UltraGridItems.KeyDown

        ' toggle the sticky checkbox in the selected Item Load grid rows
        ' when the user presses the space bar
        If e.KeyCode = Keys.Space Then
            For Each theRow As UltraGridRow In UltraGridItems.Selected.Rows
                Dim isSticky As Integer = Integer.Parse(theRow.Cells("STICKY").Value.ToString())

                If isSticky = 1 Then
                    theRow.Cells("STICKY").Value = 0
                Else
                    theRow.Cells("STICKY").Value = 1
                End If
            Next

            e.Handled = True
        End If

    End Sub

    Private Sub UploadGrid_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles _
            UltraGridItemMaintenance.KeyDown, UltraGridPriceUpload.KeyDown, UltraGridCostUpload.KeyDown

        Dim theGrid As UltraGrid = CType(sender, UltraGrid)

        ' allow the user to navigate the grids using the arrow keys
        Select Case e.KeyValue
            Case Keys.Up
                theGrid.PerformAction(ExitEditMode, False, False)
                theGrid.PerformAction(AboveCell, False, False)
                e.Handled = True
                theGrid.PerformAction(EnterEditMode, False, False)
            Case Keys.Down
                theGrid.PerformAction(ExitEditMode, False, False)
                theGrid.PerformAction(BelowCell, False, False)
                e.Handled = True
                theGrid.PerformAction(EnterEditMode, False, False)
            Case Keys.Right
                theGrid.PerformAction(ExitEditMode, False, False)
                theGrid.PerformAction(NextCellByTab, False, False)
                e.Handled = True
                theGrid.PerformAction(EnterEditMode, False, False)
            Case Keys.Left
                theGrid.PerformAction(ExitEditMode, False, False)
                theGrid.PerformAction(PrevCellByTab, False, False)
                e.Handled = True
                theGrid.PerformAction(EnterEditMode, False, False)
        End Select

    End Sub

#End Region

#Region "Private Methods"

#Region "Main Action Methods"

    Private Sub SpreadsheetImport()

        Dim fileName As String = Nothing

        If GetImportSpreadsheetFileName(fileName) Then

            If Not String.IsNullOrEmpty(fileName) Then
                Dim oldCursor As Cursor = Me.Cursor

                Try
                    Me.Cursor = Cursors.WaitCursor
                    Me.UltraGridItemMaintenance.BeginUpdate()
                    Me.UltraGridPriceUpload.BeginUpdate()
                    Me.UltraGridCostUpload.BeginUpdate()

                    Me.EIMManager.BackUpCurrentUploadSession()

                    Me.EIMManager.ProgressCounter = 0
                    Me.EIMManager.ProgressComplete = False

                    Me.EIMManager.CurrentFileName = fileName

                    ' this will cause the creation a new session
                    Me.EIMManager.CurrentUploadSession = Nothing

                    Dim theUploadTypeCollection As BusinessObjectCollection = _
                            Me.EIMManager.GetUploadTypesFromSpreadsheet(True)

                    If theUploadTypeCollection.Count > 0 Then

                        Me.EIMManager.CurrentUploadSession.SetUploadTypes(theUploadTypeCollection)

                        Dim theGridCollection As SortableHashlist = GetGridsByUploadTypeCollection()

                        If GetSelectedUploadTypesFromUser(SessionActions.Import) Then
                            Me.Cursor = Cursors.WaitCursor

                            ClearAllGrids()

                            SetTopTab()

                            ' spin off the import on another thread so this UI thread
                            ' can poll the progress
                            ' this approach was chosen over a singly threaded event-based approach
                            ' because intermittent polling will add less overhead than
                            ' handling a synchronous event that updates the progress bar
                            Dim theImportThread As New Thread(AddressOf Me.SpreadsheetImportThreaded)
                            theImportThread.Start()
                            Me.Cursor = Cursors.WaitCursor

                            ' show the progress dialog but first wait a bit first
                            ' and then check to see if the action is complete to avoid
                            ' flashing the dialog when the progress is too quick
                            Thread.Sleep(500)
                            If Not Me.EIMManager.ProgressComplete And Not Me.AnErrorHasOccurred Then
                                Dim theProgressDialog As ProgressDialog = ProgressDialog.OpenProgressDialog(Me, "Importing a Spreadsheet into a new Session", _
                                      "Validating Identifiers. Please Stand By...", "rows", Me.EIMManager.SpreadSheetRowCount - 2, 0)

                                ' poll for the collection's progress
                                While Not Me.EIMManager.ProgressComplete And Not Me.AnErrorHasOccurred
                                    Thread.Sleep(300)
                                    If Not IsNothing(theProgressDialog) Then
                                        theProgressDialog.UpdateProgressDialogValue(Me.EIMManager.ProgressCounter)
                                    End If
                                End While
                                If Not IsNothing(theProgressDialog) Then
                                    theProgressDialog.CloseProgressDialog()
                                End If
                            End If
                            Me.Cursor = Cursors.WaitCursor

                            If Not Me.AnErrorHasOccurred Then

                                Me.EIMManager.AddAllRequiredUploadValuesToUploadRows()

                                LoadValueListData(True)

                                Me.EIMManager.BindUploadSessionDataToGrids(GetGridsByUploadTypeCollection(), True, True)

                                ' validate data
                                ValidateSessionData(ValidationTypes.UploadValue)

                                ' setup the selections in the store selector controls
                                Me.StoreSelectorPriceUpload.CurrentUploadSession = Me.EIMManager.CurrentUploadSession
                                Me.StoreSelectorCostUpload.CurrentUploadSession = Me.EIMManager.CurrentUploadSession

                                Me.HasChanged = True

                            End If
                        Else
                            Me.EIMManager.RestorePreviousUploadSession()
                            ManageUIAppearance()
                        End If
                    End If
                Catch ex As IOException

                    MessageBox.Show("The spreadsheet is currently open in another program. " + _
                            Environment.NewLine + Environment.NewLine + "Please close the spreadsheet and try again.", "Open Spreadsheet Error", _
                            MessageBoxButtons.OK, MessageBoxIcon.Error)
                Finally
                    Me.UltraGridItemMaintenance.EndUpdate()
                    Me.UltraGridPriceUpload.EndUpdate()
                    Me.UltraGridCostUpload.EndUpdate()

                    Me.Cursor = oldCursor
                    Me.Enabled = True

                    ManageUploadToItemStoreCheckBox()

                    Me.AnErrorHasOccurred = False
                End Try
            End If
        End If
    End Sub

    Private Sub SpreadsheetImportThreaded()

        Try
            Me.EIMManager.SpreadsheetImport()
        Catch ex As IOException

            MessageBox.Show("The spreadsheet is currently open in another program. " + _
                    Environment.NewLine + Environment.NewLine + "Please close the spreadsheet and try again.", "Open Spreadsheet Error", _
                    MessageBoxButtons.OK, MessageBoxIcon.Error)

            Me.AnErrorHasOccurred = True

        End Try
    End Sub

    Private Sub SpreadsheetExport()

        Dim oldCursor As Cursor = Me.Cursor
        Dim theOriginalUploadSessionUploadTypeCollection As BusinessObjectCollection = _
            Me.EIMManager.CurrentUploadSession.UploadSessionUploadTypeCollection

        Try
            Me.Cursor = Cursors.WaitCursor

            ' make a copy of the session's UploadSessionUploadTypeCollection to restore
            ' after the export because the GetSelectedUploadTypesFromUser function changes it
            theOriginalUploadSessionUploadTypeCollection = _
                Me.EIMManager.CurrentUploadSession.CopyUploadSessionUploadTypeCollection()

            If GetSelectedUploadTypesFromUser(SessionActions.Export) Then
                Dim gridsByUploadTypeCollection As SortableHashlist = GetGridsByUploadTypeCollection()

                Dim fileName As String = Nothing

                If GetExportSpreadsheetFileName(fileName) Then

                    If Not String.IsNullOrEmpty(fileName) Then

                        Me.EIMManager.CurrentFileName = fileName

                        Me.Cursor = Cursors.WaitCursor
                        Dim exportRowCount As Integer = Me.EIMManager.CurrentUploadSession.RowsNotMarkedForDeleteCount

                        ' this is critical
                        Me.EIMManager.ProgressComplete = False
                        Me.EIMManager.ProgressCounter = 0

                        ' spin off the export on another thread so this UI thread
                        ' can poll the progress
                        ' this approach was chosen over a singly threaded event-based approach
                        ' because intermittent polling will add less overhead than
                        ' handling a synchronous event that updates the progress bar
                        Dim theExportThread As New Thread(AddressOf Me.SpreadsheetExportThreaded)
                        theExportThread.Start()

                        ' show the progress dialog but first wait a bit first
                        ' and then check to see if the action is complete to avoid
                        ' flashing the dialog when the progress is too quick
                        Thread.Sleep(500)
                        If Not Me.EIMManager.ProgressComplete And Not Me.AnErrorHasOccurred Then
                            Dim theProgressDialog As ProgressDialog = ProgressDialog.OpenProgressDialog(Me, "Exporting to a new Spreadsheet", _
                                  "Please Stand By...", "rows", exportRowCount, 0)

                            ' poll for the collection's progress
                            While Not Me.EIMManager.ProgressComplete And Not Me.AnErrorHasOccurred
                                Thread.Sleep(50)
                                If Not IsNothing(theProgressDialog) Then
                                    theProgressDialog.UpdateProgressDialogValue(Me.EIMManager.ProgressCounter)
                                End If
                            End While

                            If Not IsNothing(theProgressDialog) Then
                                theProgressDialog.CloseProgressDialog()
                            End If
                            Me.Cursor = Cursors.WaitCursor

                        End If
                    End If
                End If
            End If
        Finally
            Me.Cursor = oldCursor
            Me.Enabled = True
            Me.AnErrorHasOccurred = False

            ' now restore the session's UploadSessionUploadTypeCollection
            Me.EIMManager.CurrentUploadSession.UploadSessionUploadTypeCollection = _
                theOriginalUploadSessionUploadTypeCollection

        End Try

    End Sub

    Private Sub SpreadsheetExportThreaded()

        Try
            Dim gridsByUploadTypeCollection As SortableHashlist = GetGridsByUploadTypeCollection()
            Me.EIMManager.SpreadsheetExport(gridsByUploadTypeCollection, Me.EIMManager.CurrentUploadSession.UploadTypeCollection, Me.EIMManager.CurrentUploadRowHolderCollecton.UploadRowHolderList)
        Catch ex As IOException

            MessageBox.Show("The spreadsheet you are overwriting is currently open in another program. " + _
                    Environment.NewLine + Environment.NewLine + "Please close the spreadsheet and try again.", "Export Spreadsheet Error", _
                    MessageBoxButtons.OK, MessageBoxIcon.Error)

            Me.AnErrorHasOccurred = True

        End Try

    End Sub

    Private Sub SessionUpload()

        If Me.AllowUpload Then
            Dim oldCursor As Cursor = Me.Cursor

            Try
                Dim doUpload As Boolean = True
                Dim theStoreSelectionMessage As String = ""
                Dim theDialogResult As DialogResult

                ' if this is a new item session only allow upload if there is at least
                ' item maintenance data
                If Me.EIMManager.CurrentUploadSession.IsNewItemSessionFlag AndAlso _
                    Me.UltraGridItemMaintenance.Rows.Count = 0 Then

                    MessageBox.Show("You must at least upload item data " + _
                                ControlChars.NewLine + "when uploading a new item session.", "EIM - Upload Session", MessageBoxButtons.OK, MessageBoxIcon.Hand)
                Else

                    If Me.CheckBoxUploadToItemStore.Checked Then

                        If Not IsNothing(Me.EIMManager.CurrentUploadSession.FindUploadSessionUploadType(EIM_Constants.PRICE_UPLOAD_CODE, True)) OrElse _
                            Not IsNothing(Me.EIMManager.CurrentUploadSession.FindUploadSessionUploadType(EIM_Constants.COST_UPLOAD_CODE, True)) Then

                            doUpload = MessageBox.Show("You have chosen to upload Prices and Costs Back to Item's Store. " + _
                                    ControlChars.NewLine + ControlChars.NewLine + _
                                    "Continue?", "EIM - Upload Session", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.OK
                        End If
                    Else

                        If Not IsNothing(Me.EIMManager.CurrentUploadSession.FindUploadSessionUploadType(EIM_Constants.PRICE_UPLOAD_CODE, True)) And _
                                Not Me.StoreSelectorPriceUpload.HasStoreSelection Then

                            theStoreSelectionMessage = "You must select stores for the Price Upload or " + _
                                ControlChars.NewLine + "check the checkbox to upload Prices and Costs Back to Item's Store."

                            doUpload = False

                        End If

                        If Not IsNothing(Me.EIMManager.CurrentUploadSession.FindUploadSessionUploadType(EIM_Constants.COST_UPLOAD_CODE, True)) And _
                                Not Me.StoreSelectorCostUpload.HasStoreSelection Then

                            If theStoreSelectionMessage.Length > 0 Then
                                theStoreSelectionMessage = "You must select stores for the Price and Cost Uploads or " + _
                                ControlChars.NewLine + "check the checkbox to upload Prices and Costs Back to Item's Store."
                            Else
                                theStoreSelectionMessage = "You must select stores for the Cost Upload or " + _
                                ControlChars.NewLine + "check the checkbox to upload Prices and Costs Back to Item's Store."
                            End If

                            doUpload = False

                        End If

                        If Not doUpload Then

                            MessageBox.Show(theStoreSelectionMessage, "EIM - Upload Session", _
                                                                    MessageBoxButtons.OK, MessageBoxIcon.Warning)

                        Else
                            theDialogResult = MessageBox.Show("Upload this session?", "EIM - Upload Session", _
                                                                    MessageBoxButtons.YesNo, MessageBoxIcon.Question)

                            doUpload = theDialogResult = Windows.Forms.DialogResult.Yes

                        End If
                    End If

                    If doUpload Then

                        Me.EIMManager.CurrentUploadSession.LoadFromDataSet(False)

                        ' this is important!
                        Me.EIMManager.CalcValidateManager.ResetIdentifierValidationFlags()

                        ' only fully validate the session data if we have rolled over to a
                        ' new day since the last time full validation ran
                        ' TFS 6398 - remove this condition and always re-validate session data.
                        'If Me.EIMManager.CalcValidateManager.LastValidatedDateTime.Date.CompareTo(DateTime.Now.Date) < 0 Then
                        ValidateSessionData(ValidationTypes.UploadValue)
                        'End If

                        ValidateForUpload()

                        ManageUIAppearance()
                        Me.SetUploadExclusionCheckBox()
                        Dim theUploadMessage As String = ""

                        If Me.EIMManager.CurrentUploadSession.ItemsProcessedCount = 0 Then
                            MessageBox.Show("There are no rows without validation errors or warnings not flagged for exclusion to upload." + ControlChars.NewLine + ControlChars.NewLine + _
                                    "Please correct these errors/warnings as desired and try the upload again.", "EIM - Session Upload", MessageBoxButtons.OK, MessageBoxIcon.Information)

                            doUpload = False
                        Else
                            If Me.EIMManager.CurrentUploadSession.ErrorsCount > 0 Or Me.EIMManager.CurrentUploadSession.WarningCount > 0 Then

                                ' warn the user about any validation warnings and errors
                                If Me.EIMManager.CurrentUploadSession.ErrorsCount > 0 Then
                                    theUploadMessage = "There are " + Me.EIMManager.CurrentUploadSession.ErrorsCount.ToString() + " rows with validation errors"
                                End If

                                If Me.EIMManager.CurrentUploadSession.WarningCount > 0 Then

                                    If theUploadMessage.Length > 0 Then
                                        theUploadMessage = theUploadMessage + ControlChars.NewLine + _
                                            "and " + Me.EIMManager.CurrentUploadSession.WarningCount.ToString() + " rows with validation warnings"
                                    Else

                                        If Me.EIMManager.CurrentUploadSession.WarningCount = 1 Then
                                            theUploadMessage = "There is " + Me.EIMManager.CurrentUploadSession.WarningCount.ToString() + " row with validation warning"

                                        Else
                                            theUploadMessage = "There are " + Me.EIMManager.CurrentUploadSession.WarningCount.ToString() + " rows with validation warnings"
                                        End If
                                      

                                    End If
                                End If


                                If Me.EIMManager.CurrentUploadSession.WarningCountItemDeauthForAllStores = 1 Then
                                    theUploadMessage = theUploadMessage + "." + ControlChars.NewLine + ControlChars.NewLine + "ITEM WILL BE DEAUTHORIZED FOR ALL STORES"
                                ElseIf Me.EIMManager.CurrentUploadSession.WarningCountItemDeauthForAllStores > 1 Then
                                    theUploadMessage = theUploadMessage + "." + ControlChars.NewLine + ControlChars.NewLine + "ITEMS WILL BE DEAUTHORIZED FOR ALL STORES"
                                End If


                                If Me.EIMManager.CurrentUploadSession.WarningCountItemDeauthForStore = 1 Then
                                    theUploadMessage = theUploadMessage + "." + ControlChars.NewLine + ControlChars.NewLine + "ITEM WILL BE DEAUTHORIZED FOR STORE"
                                ElseIf Me.EIMManager.CurrentUploadSession.WarningCountItemDeauthForStore > 1 Then
                                    theUploadMessage = theUploadMessage + "." + ControlChars.NewLine + ControlChars.NewLine + "ITEMS WILL BE DEAUTHORIZED FOR STORES"
                                End If


                                If Me.EIMManager.CurrentUploadSession.WarningCountPrimaryVendorSwap Then
                                    theUploadMessage = theUploadMessage + "." + ControlChars.NewLine + ControlChars.NewLine + "PRIMARY VENDOR WILL BE SWAPPED"
                                End If


                                theUploadMessage = theUploadMessage + "." + ControlChars.NewLine + ControlChars.NewLine + _
                                        "The rows with warnings not flagged for exclusion will upload but those " + ControlChars.NewLine + _
                                        "with errors and warnings flagged for exclusion will not upload and will remain in the grids as a new session."

                            End If
                        End If

                        If Me.EIMManager.PriceChangeErrorCount > 0 Then
                            If theUploadMessage.Length > 0 Then
                                theUploadMessage = theUploadMessage + ControlChars.NewLine + ControlChars.NewLine
                            End If

                            theUploadMessage = theUploadMessage + _
                              "At least one of these errors is for price changes that " + ControlChars.NewLine + _
                              "either would duplicate existing changes or for which " + ControlChars.NewLine + _
                              "there are not primary vendors for all stores uploaded to."
                        End If

                        If Me.EIMManager.UploadedDuplicatePriceChangesCount > 0 Then
                            If theUploadMessage.Length > 0 Then
                                theUploadMessage = theUploadMessage + ControlChars.NewLine + ControlChars.NewLine
                            End If

                            theUploadMessage = theUploadMessage + _
                            "At least one of these errors is for price changes that " + ControlChars.NewLine + _
                            "are duplicates of other data in the prices grid."
                        End If

                        If Me.EIMManager.UploadedPrimaryVendorCollisionCount > 0 Then
                            If theUploadMessage.Length > 0 Then
                                theUploadMessage = theUploadMessage + ControlChars.NewLine + ControlChars.NewLine
                            End If

                            theUploadMessage = theUploadMessage + _
                            "At least one of these errors is for an item/store with more than one vendor " + ControlChars.NewLine + _
                            "set as primary in the costs grid."
                        End If


                        If Me.EIMManager.UploadedSecondaryVendorCollisionCount > 0 Then
                            If theUploadMessage.Length > 0 Then
                                theUploadMessage = theUploadMessage + ControlChars.NewLine + ControlChars.NewLine
                            End If

                            theUploadMessage = theUploadMessage + _
                            "At least one of these errors is for an item/store(s) with more than one secondary vendor available when primary item vendor " + ControlChars.NewLine + _
                            "is being deleted or deauthorized."
                        End If


                        If Me.EIMManager.StoreJurisdictionErrorCount > 0 Then
                            If theUploadMessage.Length > 0 Then
                                theUploadMessage = theUploadMessage + ControlChars.NewLine + ControlChars.NewLine
                            End If

                            theUploadMessage = theUploadMessage + _
                            "At least one of these errors are store jurisdiction errors."
                        End If

                        If theUploadMessage.Length > 0 Then
                            If doUpload Then
                                theUploadMessage = theUploadMessage + ControlChars.NewLine + ControlChars.NewLine

                                theUploadMessage = theUploadMessage + "Continue and Upload?"

                                theDialogResult = MessageBox.Show(theUploadMessage, "EIM - Upload Session", _
                                                        MessageBoxButtons.YesNo, MessageBoxIcon.Question)

                                doUpload = theDialogResult = Windows.Forms.DialogResult.Yes
                            Else
                                theDialogResult = MessageBox.Show(theUploadMessage, "EIM - Upload Session", _
                                                       MessageBoxButtons.OK, MessageBoxIcon.Information)

                            End If
                        End If
                    End If

                    If doUpload Then
                        Me.Cursor = Cursors.WaitCursor

                        ' hold on to the rows that are not invalid
                        ' as they will be successfully uploaded and
                        ' will have to be removed from the session,
                        ' which is made new, after the upload
                        Dim theUploadRowsToRemove As New ArrayList()

                        For Each theUploadRow As UploadRow In Me.EIMManager.CurrentUploadSession.UploadRowCollection


                            Dim upload_exclusion As Integer = 0
                            Dim theUploadValueForUploadExclusionFlag As UploadValue

                            theUploadValueForUploadExclusionFlag = theUploadRow.FindUploadValueByAttributeKey(EIM_Constants.UPLOAD_EXCLUSION_COLUMN)

                            If Not IsNothing(theUploadValueForUploadExclusionFlag) AndAlso CBool(theUploadValueForUploadExclusionFlag.Value) Then
                                upload_exclusion = 1
                            End If

                            'If theUploadRow.ValidationLevel <> EIM_Constants.ValidationLevels.Invalid And upload_exclusion = 0 Then
                            If theUploadRow.ValidationLevel = EIM_Constants.ValidationLevels.Valid Or (theUploadRow.ValidationLevel = EIM_Constants.ValidationLevels.Warning And upload_exclusion = 0) Then

                                theUploadRowsToRemove.Add(theUploadRow)

                            End If
                        Next

                        doUpload = SessionSave(False, False)

                        If doUpload Then

                            ' for the session load we are tracking the progress of the UploadSession.BuildDataSet()
                            ' method as it triggers the just-in-time instantiation of UploadRows and their values
                            ' from the database
                            Me.EIMManager.CurrentUploadSession.ProgressComplete = False

                            ' spin off the load on another thread so this UI thread
                            ' can poll the progress
                            ' this approach was chosen over a singly threaded event-based approach
                            ' because intermittent polling will add less overhead than
                            ' handling a synchronous event that updates the progress bar
                            Dim theSessionUploadThread As New Thread(AddressOf Me.SessionUploadThreaded)
                            theSessionUploadThread.Start()

                            ' show the progress dialog but first wait a bit first
                            ' and then check to see if the action is complete to avoid
                            ' flashing the dialog when the progress is too quick
                            Thread.Sleep(500)
                            If Not Me.EIMManager.CurrentUploadSession.ProgressComplete And Not Me.AnErrorHasOccurred Then
                                Dim theProgressDialog As ProgressDialog = ProgressDialog.OpenProgressDialog(Me, "Uploading your Session Data.", _
                                      "This may take a while. Please Stand By...", "rows", ProgressBarStyle.Marquee)

                                ' poll for the collection's progress
                                While Not Me.EIMManager.CurrentUploadSession.ProgressComplete And Not Me.AnErrorHasOccurred
                                    Thread.Sleep(100)
                                    If Not IsNothing(theProgressDialog) Then
                                        theProgressDialog.UpdateProgressDialogValue(Me.EIMManager.CurrentUploadSession.ProgressCounter)
                                    End If
                                End While

                                If Not IsNothing(theProgressDialog) Then
                                    theProgressDialog.CloseProgressDialog()
                                End If

                                Me.Cursor = Cursors.WaitCursor
                            End If

                            If Not Me.AnErrorHasOccurred Then

                                If Not Me._isUploadSuccesful Then

                                    ShowErrorDialog("The error has been logged for session ID " + Me.EIMManager.CurrentUploadSession.UploadSessionID.ToString() + ".", _
                                        "An error occurred while an EIM Session was being uploaded. Your session was not uploaded", _
                                        "The detail for this error is in the ODBC Error Log in the database.")

                                Else

                                    Me.EIMManager.EmailUploadConfirmation()

                                    ' clear out all uploaded rows
                                    For Each theUploadRowToRemove As UploadRow In theUploadRowsToRemove
                                        Me.EIMManager.CurrentUploadSession.UploadRowCollection.Remove(theUploadRowToRemove)
                                    Next

                                    If Me.EIMManager.CurrentUploadSession.UploadRowCollection.Count > 0 Then
                                        ' make into a new session
                                        Me.EIMManager.CurrentUploadSession.MakeNew()

                                        ' update the grids
                                        ClearAllGrids()
                                        Me.EIMManager.BindUploadSessionDataToGrids(GetGridsByUploadTypeCollection(), False)
                                        Me.EIMManager.Validate(ValidationTypes.GridCell, False)
                                        ValidateForUploadNonThreaded()

                                        Me.HasChanged = True

                                    Else
                                        ' if there are no more rows then go to the search tab

                                        ' set the search to bring up the newly uploaded UploadSession
                                        Me.RadioButtonSavedAndUploaded.Checked = True
                                        Me.DateTimeEditorCreatedDate.DateTime = DateTime.Now
                                        SetAdminUser(Me.ComboBoxAdmins, giUserID)
                                        SessionSearch(False)

                                        ' clear out the session
                                        Me.EIMManager.CurrentUploadSession = Nothing
                                        ClearAllGrids()

                                        Me.HasChanged = False

                                        ' bring the session search tab to the top
                                        Me.TabControlUpoadPages.SelectedIndex = 1

                                    End If
                                End If
                            End If
                        End If
                    End If
                End If
            Finally
                Me.Cursor = oldCursor
                Me.AnErrorHasOccurred = False
                Me.Enabled = True
            End Try
        End If

    End Sub

    Private Sub SessionUploadThreaded()

        Try
            Me._isUploadSuccesful = UploadSessionDAO.Instance.UploadSession(Me.EIMManager.CurrentUploadSession, Me.EIMManager.CurrentUploadSession.IsFromSLIM)
        Catch ex As Exception
            UIThreadSafeShowUploadErrorDialog(ex, "An EIM Upload Error Has Occurred", "An error occurred while an EIM Session was being uploaded. Your session was not uploaded.")
        End Try

    End Sub

    Private Function SessionSave(ByVal asNewSession As Boolean, ByVal forUpload As Boolean) As Boolean

        Dim oldCursor As Cursor = Me.Cursor
        Dim saveSuccessful As Boolean = True
        Dim isSessionNew As Boolean = Me.EIMManager.CurrentUploadSession.IsNew

        Try
            ' load the store selections, if any
            Me.StoreSelectorPriceUpload.LoadDataFromUI()
            Me.StoreSelectorCostUpload.LoadDataFromUI()

            Dim theSessionAction As SessionActions

            If asNewSession Then
                Me.EIMManager.CurrentUploadSession.MakeNew()
            End If

            If Me.EIMManager.CurrentUploadSession.UploadSessionID < 0 Then
                theSessionAction = SessionActions.SaveNew
            Else
                theSessionAction = SessionActions.SaveExisting
            End If

            ' only save the session if it is new or it is existing and has changes
            If theSessionAction = SessionActions.SaveNew Or _
                (theSessionAction = SessionActions.SaveExisting And Me.HasChanged) Then

                saveSuccessful = False

                If forUpload OrElse GetSelectedUploadTypesFromUser(theSessionAction) Then
                    Me.Cursor = Cursors.WaitCursor

                    ' load the data from the grid-bound dataset back into the UploadSession data structure
                    Me.EIMManager.CurrentUploadSession.LoadFromDataSet(False)

                    ' if the load into items store checkbox is checked then
                    ' we must mark all the for delete
                    If Me.CheckBoxUploadToItemStore.Checked Then
                        Dim theUploadSessionUploadType As UploadSessionUploadType = _
                            Me.EIMManager.CurrentUploadSession.FindUploadSessionUploadType(EIM_Constants.PRICE_UPLOAD_CODE, True)

                        If Not IsNothing(theUploadSessionUploadType) Then
                            For Each theUploadSessionUploadTypeStore As UploadSessionUploadTypeStore In _
                                    theUploadSessionUploadType.UploadSessionUploadTypeStoreCollection

                                theUploadSessionUploadTypeStore.IsMarkedForDelete = True
                            Next
                        End If

                        theUploadSessionUploadType = _
                            Me.EIMManager.CurrentUploadSession.FindUploadSessionUploadType(EIM_Constants.COST_UPLOAD_CODE, True)

                        If Not IsNothing(theUploadSessionUploadType) Then
                            For Each theUploadSessionUploadTypeStore As UploadSessionUploadTypeStore In _
                                     theUploadSessionUploadType.UploadSessionUploadTypeStoreCollection

                                theUploadSessionUploadTypeStore.IsMarkedForDelete = True
                            Next
                        End If
                    End If

                    ' first show the progress for any deletes
                    Me.EIMManager.CurrentUploadSession.UploadRowCollection.DeleteProgressComplete = False

                    ' spin off the save on another thread so this UI thread
                    ' can poll the progress
                    ' this approach was chosen over a singly threaded event-based approach
                    ' because intermittent polling will add less overhead than
                    ' handling a synchronous event that updates the progress bar
                    Dim theSaveMarkedForDeleteThread As New Thread(AddressOf Me.SessionSaveMarkedForDeleteThreaded)
                    theSaveMarkedForDeleteThread.Start()

                    ' show the progress dialog but first wait a bit first
                    ' and then check to see if the action is complete to avoid
                    ' flashing the dialog when the progress is too quick
                    Thread.Sleep(1000)
                    If Not Me.EIMManager.CurrentUploadSession.UploadRowCollection.DeleteProgressComplete And Not Me.AnErrorHasOccurred Then
                        Dim theProgressDialog As ProgressDialog = ProgressDialog.OpenProgressDialog(Me, "Removing Deleted Rows", _
                              "Please Stand By...", "rows", Me.EIMManager.CurrentUploadSession.RowsMarkedForDeleteCount, 20)

                        ' poll for the collection's progress
                        While Not Me.EIMManager.CurrentUploadSession.UploadRowCollection.DeleteProgressComplete And Not Me.AnErrorHasOccurred
                            Thread.Sleep(300)
                            If Not IsNothing(theProgressDialog) Then
                                theProgressDialog.UpdateProgressDialogValue(Me.EIMManager.CurrentUploadSession.UploadRowCollection.DeleteProgressCounter)
                            End If
                        End While

                        If Not IsNothing(theProgressDialog) Then
                            theProgressDialog.CloseProgressDialog()
                        End If
                        Me.Cursor = Cursors.WaitCursor
                    End If

                    If Me.AnErrorHasOccurred Then

                        ManageUIAppearance()
                    Else

                        ' then show the progress for any saves
                        Me.EIMManager.CurrentUploadSession.UploadRowCollection.SaveProgressComplete = False

                        ' spin off the save on another thread so this UI thread
                        ' can poll the progress
                        ' this approach was chosen over a singly threaded event-based approach
                        ' because intermittent polling will add less overhead than
                        ' handling a synchronous event that updates the progress bar
                        Dim theSaveThread As New Thread(AddressOf Me.SessionSaveThreaded)
                        theSaveThread.Start()

                        ' show the progress dialog but first wait a bit first
                        ' and then check to see if the action is complete to avoid
                        ' flashing the dialog when the progress is too quick
                        Thread.Sleep(500)
                        If Not Me.EIMManager.CurrentUploadSession.UploadRowCollection.SaveProgressComplete And Not Me.AnErrorHasOccurred Then
                            Dim theProgressDialog As ProgressDialog = ProgressDialog.OpenProgressDialog(Me, "Saving Session", _
                                  "Please Stand By...", "rows", Me.EIMManager.CurrentUploadSession.ItemsLoadedCount, 10)

                            ' poll for the collection's progress
                            While Not Me.EIMManager.CurrentUploadSession.UploadRowCollection.SaveProgressComplete And Not Me.AnErrorHasOccurred
                                Thread.Sleep(300)
                                If Not IsNothing(theProgressDialog) Then
                                    theProgressDialog.UpdateProgressDialogValue(Me.EIMManager.CurrentUploadSession.UploadRowCollection.SaveProgressCounter)
                                End If
                            End While

                            If Not IsNothing(theProgressDialog) Then
                                theProgressDialog.CloseProgressDialog()
                            End If
                            Me.Cursor = Cursors.WaitCursor
                        End If

                        If Not Me.AnErrorHasOccurred Then

                            If isSessionNew Then
                                ' update with the new UploadRowIds
                                Me.EIMManager.CurrentUploadRowHolderCollecton.UpdateForNewUploadRowIDs()
                            End If

                            Me.HasChanged = False
                            saveSuccessful = True

                        End If
                    End If
                End If
            End If

        Finally
            Me.Cursor = oldCursor
            Me.AnErrorHasOccurred = False
            Me.Enabled = True
        End Try

        Return saveSuccessful

    End Function

    Private Sub SessionSaveMarkedForDeleteThreaded()

        Try
            ' Throw New Exception("Test")
            Me.EIMManager.SessionSaveMarkedForDelete(Me.EIMManager.CurrentUploadSession.UploadTypeCollection)
        Catch ex As Exception

            Me.AllowUpload = False

            UIThreadSafeShowErrorDialog(ex, "An EIM Saving Error Has Occurred", _
                "An error occurred while an EIM Session was being Saved. Your session was not saved. " + _
                "You will not be able to upload your data. Try to export your data to save your work.")
        End Try

    End Sub

    Private Sub SessionSaveThreaded()

        Try
            ' Throw New Exception("Test")
            Me.EIMManager.SessionSave(Me.EIMManager.CurrentUploadSession.UploadTypeCollection)
        Catch ex As Exception

            Me.AllowUpload = False

            UIThreadSafeShowErrorDialog(ex, "An EIM Saving Error Has Occurred", _
                "An error occurred while an EIM Session was being Saved. Your session was not saved. " + _
                "You will not be able to upload your data. Try to export your data to save your work.")
        End Try

    End Sub

    Private Sub SessionLoad()

        If UltraGridUploadSessions.Selected.Rows.Count > 0 Then

            If MessageBox.Show("Open the selected session?" + _
                    ControlChars.NewLine + ControlChars.NewLine + "This will replace any existing session.", _
                    "EIM - Open Session", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.OK Then

                If Not AskToSaveOrCancel() Then

                    Dim oldCursor As Cursor = Me.Cursor
                    Me.Cursor = Cursors.WaitCursor

                    Try
                        ClearAllGrids()

                        Me.Cursor = Cursors.WaitCursor

                        ' for the session load we are tracking the progress of the UploadSession.BuildDataSet()
                        ' method as it triggers the just-in-time instantiation of UploadRows and their values
                        ' from the database
                        Me.EIMManager.CurrentUploadSession.ProgressComplete = False

                        ' spin off the load on another thread so this UI thread
                        ' can poll the progress
                        ' this approach was chosen over a singly threaded event-based approach
                        ' because intermittent polling will add less overhead than
                        ' handling a synchronous event that updates the progress bar
                        Dim theSessionLoadThread As New Thread(AddressOf Me.SessionLoadThreaded)
                        theSessionLoadThread.Start()

                        ' show the progress dialog but first wait a bit first
                        ' and then check to see if the action is complete to avoid
                        ' flashing the dialog when the progress is too quick
                        Thread.Sleep(500)
                        If Not Me.EIMManager.CurrentUploadSession.ProgressComplete And Not Me.AnErrorHasOccurred Then
                            Dim theProgressDialog As ProgressDialog = ProgressDialog.OpenProgressDialog(Me, "Loading an Existing Session", _
                                  "Please Stand By...", "rows", Me.EIMManager.CurrentUploadSession.ItemsLoadedCount, 10)

                            ' poll for the collection's progress
                            While Not Me.EIMManager.CurrentUploadSession.ProgressComplete And Not Me.AnErrorHasOccurred
                                Thread.Sleep(100)
                                If Not IsNothing(theProgressDialog) Then
                                    theProgressDialog.UpdateProgressDialogValue(Me.EIMManager.CurrentUploadSession.ProgressCounter)
                                End If
                            End While

                            If Not IsNothing(theProgressDialog) Then
                                theProgressDialog.CloseProgressDialog()
                            End If
                            Me.Cursor = Cursors.WaitCursor
                        End If

                        If Not Me.AnErrorHasOccurred Then

                            LoadValueListData(False)

                            Me.EIMManager.BindUploadSessionDataToGrids(GetGridsByUploadTypeCollection(), False, False)

                            Me.Cursor = Cursors.WaitCursor

                            ' set the enabled state of the store selectors
                            ' must do this before making the session new
                            Dim theUploadSessionUploadType As UploadSessionUploadType = _
                                Me.EIMManager.CurrentUploadSession.FindUploadSessionUploadType(EIM_Constants.PRICE_UPLOAD_CODE, True)

                            If Not IsNothing(theUploadSessionUploadType) Then

                                If theUploadSessionUploadType.UploadSessionUploadTypeStoreCollection.Count > 0 Then
                                    Me.CheckBoxUploadToItemStore.Checked = False
                                    Me.StoreSelectorPriceUpload.AllowStoreSelection = True
                                Else
                                    Me.CheckBoxUploadToItemStore.Checked = True
                                    Me.StoreSelectorPriceUpload.AllowStoreSelection = False
                                End If
                            End If

                            theUploadSessionUploadType = _
                               Me.EIMManager.CurrentUploadSession.FindUploadSessionUploadType(EIM_Constants.COST_UPLOAD_CODE, True)

                            If Not IsNothing(theUploadSessionUploadType) Then

                                If theUploadSessionUploadType.UploadSessionUploadTypeStoreCollection.Count > 0 Then
                                    Me.CheckBoxUploadToItemStore.Checked = False
                                    Me.StoreSelectorCostUpload.Enabled = True
                                Else
                                    Me.CheckBoxUploadToItemStore.Checked = True
                                    Me.StoreSelectorCostUpload.Enabled = False
                                End If
                            End If

                            ' if the loaded UploadSession has already been uploaded
                            ' then we have to make it into a new session
                            ' so we can't overwrite the uploaded one in the db
                            ' also clear the IsFromSLIM and IsNewItemSessionFlag flags
                            If Me.EIMManager.CurrentUploadSession.IsUploaded Then

                                Me.EIMManager.CurrentUploadSession.MakeNew()
                                Me.EIMManager.CurrentUploadSession.IsUploaded = False
                                Me.EIMManager.CurrentUploadSession.IsFromSLIM = False
                                Me.EIMManager.CurrentUploadSession.IsNewItemSessionFlag = False
                                Me.HasChanged = True

                                ' update the UploadRowHolderList with the new temporary UploadRowIds
                                Me.EIMManager.CurrentUploadRowHolderCollecton.UpdateForNewUploadRowIDs()

                            End If

                            ' validate data
                            ValidateSessionData(ValidationTypes.UploadValue)
                            Me.Cursor = Cursors.WaitCursor

                            SetTopTab()
                            ManageUIAppearance()
                            Me.Cursor = Cursors.WaitCursor

                            ' setup the selections in the store selector controls
                            Me.StoreSelectorPriceUpload.CurrentUploadSession = Me.EIMManager.CurrentUploadSession
                            Me.StoreSelectorCostUpload.CurrentUploadSession = Me.EIMManager.CurrentUploadSession

                            If Me.EIMManager.CurrentUploadSession.IsUploaded Then
                                MessageBox.Show("This session data has been uploaded." + _
                                    ControlChars.NewLine + ControlChars.NewLine + "If you change and save it, it will save as a new session.", _
                                    "EIM - Open Session", MessageBoxButtons.OK, MessageBoxIcon.Question)
                            End If
                        End If

                    Finally
                        Me.Cursor = oldCursor
                        Me.AnErrorHasOccurred = False
                        Me.Enabled = True
                    End Try
                End If
            End If
        End If

    End Sub

    Private Sub SessionLoadThreaded()

        Try
            ' Throw New Exception("Test")
            Dim uploadSessionId As Integer = Integer.Parse(UltraGridUploadSessions.Selected.Rows(0).Cells(0).Value.ToString())
            Me.EIMManager.SessionLoad(uploadSessionId)
        Catch ex As Exception
            UIThreadSafeShowErrorDialog(ex, "An EIM Session Loading Error Has Occurred", "An error occurred while an EIM Session was being Loaded.")
        End Try
    End Sub

    Private Sub SessionCascadeDelete(ByVal uploadSessionId As Integer)
        Dim oldCursor As Cursor = Me.Cursor

        Try
            Me.Cursor = Cursors.WaitCursor
            Me.EIMManager.SessionCascadeDelete(uploadSessionId)

            If Me.EIMManager.CurrentUploadSession. _
                    UploadSessionID = uploadSessionId Then
                ClearAllGrids()
                Me.EIMManager.CurrentUploadSession = Nothing
            End If
        Catch ex As Exception
            ShowErrorDialog(ex, "An EIM Session Deleting Error Has Occurred", "An error occurred while an EIM Session(s) was being deleted.")
        Finally
            Me.Cursor = oldCursor
            Me.AnErrorHasOccurred = False
        End Try
    End Sub

    Private Sub SessionSearch(ByVal showNotFoundMessage As Boolean)

        Try
            Dim theUserID As Integer = -1
            If Not IsNothing(ComboBoxAdmins.SelectedItem) Then
                theUserID = CType(ComboBoxAdmins.SelectedItem, ItemAdminUserBO).UserID
            End If

            Dim theUploadType As String = "ALL"
            If Not IsNothing(Me.ComboBoxUploadType.SelectedValue) Then
                theUploadType = Me.ComboBoxUploadType.SelectedValue.ToString()
            End If

            Dim theSessionId As Integer = 0

            If Not Me.NumericEditorSessionID.Value Is DBNull.Value Then
                theSessionId = CInt(Me.NumericEditorSessionID.Value)
            End If

            Dim theDataSet As DataSet = UploadSessionDAO.Instance.SessionSearch(theSessionId, theUploadType, _
                Me.TextBoxSessionName.Text, theUserID, Me.DateTimeEditorCreatedDate.DateTime, _
                Me.RadioButtonSavedOnly.Checked, Me.RadioButtonUploadedOnly.Checked, Me.RadioButtonSavedAndUploaded.Checked)

            'Setup a column that you would like to sort on initially.
            theDataSet.AcceptChanges()
            Dim mdv As DataView = New DataView(theDataSet.Tables(0))

            mdv.Sort = "UploadSession_ID desc"
            Me.UltraGridUploadSessions.SetDataBinding(mdv, "", True)

            If mdv.Table.Rows.Count > 0 Then
                'Set the first session to selected.
                Me.UltraGridUploadSessions.Rows(0).Selected = True
                Me.EIMManager.AutoSizeAllGridColumns(Me.UltraGridUploadSessions)
            ElseIf showNotFoundMessage Then
                MessageBox.Show(ResourcesIRMA.GetString("NoneFound"), "EIM - Session Search", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If

            ManageUIAppearance()
        Catch ex As Exception
            ShowErrorDialog(ex, "An EIM Search Session Error Has Occurred", "An error occurred while EIM Session were being searched.")
        End Try

    End Sub

    Private Sub ItemSearch()

        If Me.CheckBoxLoadFromSLIM.Checked And Not Me.StoreSelectorItemLoad.HasStoreSelection Then

            MessageBox.Show("You must select at least one store when loading items from SLIM.", _
                    "EIM - Load from SLIM", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Else

            Dim oldCursor As Cursor = Me.Cursor

            Try
                Me.Cursor = Cursors.WaitCursor

                Dim theIdentifier As String = CStr(Me.UltraTextEditorIdentifier.Value)
                Dim theDescription As String = CStr(Me.UltraTextEditorDescription.Value)
                Dim theBrandID As Integer = UIUtilities.GetComboBoxIntegerValue(Me.ComboBoxBrand)
                Dim theVendorID As Integer = UIUtilities.GetComboBoxIntegerValue(Me.ComboBoxVendor)
                Dim theStoreIDs As String = Me.StoreSelectorItemLoad.SelectedStoreIdString
                Dim theSubteamID As Integer = Me.HierarchySelectorItemLoad.SelectedSubTeamId
                Dim theCategoryID As Integer = Me.HierarchySelectorItemLoad.SelectedCategoryId
                Dim theLevel3ID As Integer = Me.HierarchySelectorItemLoad.SelectedLevel3Id
                Dim theLevel4ID As Integer = Me.HierarchySelectorItemLoad.SelectedLevel4Id

                Dim thePSVendorID As String = Me.UltraTextEditorPSVendorID.Text.Trim()
                Dim theVendorItemID As String = Me.UltraTextEditorVendorItemID.Text.Trim()
                Dim theDistSubteamID As Integer = UIUtilities.GetComboBoxIntegerValue(Me.ComboBoxDistSubteam)
                Dim theItemChainID As Integer = UIUtilities.GetComboBoxIntegerValue(Me.ComboBoxItemChain)
                Dim isDiscontinued As Boolean = Me.chkDiscontinued.Checked
                Dim isNotAvailable As Boolean = Me.chkNotAvailable.Checked
                Dim includeDeletedItems As Boolean = Me.chkIncludeDeletedItems.Checked
                Dim fromSLIM As Boolean = Me.CheckBoxLoadFromSLIM.Checked

                Dim theIsDefaultJurisdiction As Integer = UIUtilities.GetComboBoxIntegerValue(Me.ComboBoxIsDefaultJurisdiction)
                Dim theStoreJurisdictionId As Integer = UIUtilities.GetComboBoxIntegerValue(Me.ComboBoxJurisdiction)

                Me.ItemLoadDataSet = UploadSessionDAO.Instance.ItemSearch(theIdentifier, theDescription, _
                    theVendorID, theBrandID, theStoreIDs, theSubteamID, theCategoryID, theLevel3ID, theLevel4ID, _
                    thePSVendorID, theVendorItemID, theDistSubteamID, theItemChainID, isDiscontinued, isNotAvailable, _
                    includeDeletedItems, fromSLIM, theIsDefaultJurisdiction, theStoreJurisdictionId)

                Me.ItemLoadDataSet.AcceptChanges()

                Dim currentDataView As DataView = CType(Me.UltraGridItems.DataSource, DataView)

                ' handle the sticky rows
                If Not IsNothing(currentDataView) Then

                    ' commit any pending changes
                    currentDataView.Table.AcceptChanges()

                    ' if there is an existing items dataset then remove all the 
                    ' non-sticky rows and merge in the new items
                    For Each theDataRow As DataRow In currentDataView.Table.Rows

                        If Not CBool(theDataRow.Item("STICKY")) Then

                            theDataRow.Delete()
                        End If
                    Next

                    ' commit the deletes
                    currentDataView.Table.AcceptChanges()

                    ' merge the new items with the previous sticky items
                    currentDataView.Table.Merge(Me.ItemLoadDataSet.Tables(0))

                    currentDataView.Table.AcceptChanges()
                Else

                    ' if there is no existing items dataset then use the newly
                    ' loaded one
                    currentDataView = New DataView(Me.ItemLoadDataSet.Tables(0))

                End If

                'Setup a column that you would like to sort on initially.
                currentDataView.Sort = EIM_Constants.ITEM_ITEM_DESCRIPTION_ATTR_KEY
                Me.UltraGridItems.SetDataBinding(currentDataView, Nothing, True)

                If Me.ItemLoadDataSet.Tables(0).Rows.Count > 0 Then

                    ' force the sticky column to be a checkbox
                    Dim theStickyColumn As UltraGridColumn = Me.UltraGridItems.DisplayLayout.Bands(0).Columns("STICKY")
                    theStickyColumn.Style = ColumnStyle.CheckBox
                    theStickyColumn.CellAppearance.TextHAlign = HAlign.Center

                    'Set the first item to selected.
                    Me.UltraGridItems.Rows(0).Selected = True
                    Me.EIMManager.AutoSizeAllGridColumns(Me.UltraGridItems)

                    ManageUIAppearance()

                    If currentDataView.Table.Rows.Count >= 1000 Then
                        MessageBox.Show("Your item search has returned 1,000 or more rows." + _
                            ControlChars.NewLine + ControlChars.NewLine + "You may want to narrow your search to improve the performance of EIM.", "EIM - Item Search", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If

                Else
                    MessageBox.Show(ResourcesIRMA.GetString("NoneFound"), "EIM - Item Search", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If

            Catch ex As Exception
                ShowErrorDialog(ex, "An EIM Search Items Error Has Occurred", "An error occurred while items were being searched to load into EIM.")
            Finally
                Me.Cursor = oldCursor
                Me.AnErrorHasOccurred = False
            End Try
        End If
    End Sub

    Private Sub ItemLoad()

        If Not IsNothing(Me.UltraGridItems.DataSource) Then
            Dim oldCursor As Cursor = Me.Cursor

            Try
                Me.Cursor = Cursors.WaitCursor

                ' hold on to the previous session in case the user cancels
                Me.EIMManager.BackUpCurrentUploadSession()

                ' this will cause a new session to be created
                Me.EIMManager.CurrentUploadSession = Nothing

                Dim continueItemLoad As Boolean = GetSelectedUploadTypesFromUser(SessionActions.LoadItems)

                If continueItemLoad Then

                    ' set the flag if this is a "From SLIM" session
                    If Me.CheckBoxLoadFromSLIM.Checked Then

                        Me.EIMManager.CurrentUploadSession.IsFromSLIM = True

                        ' all "From SLIM" sessions must be marked as new
                        Me.EIMManager.CurrentUploadSession.IsNewItemSessionFlag = True

                        Me._ignoreSessionTypeChange = True
                        Me.CheckBoxNewItemSession.Checked = True
                        Me._ignoreSessionTypeChange = False

                        ' also, they must upload to the store on the row for the item
                        Me.CheckBoxUploadToItemStore.Checked = True

                    End If

                    ClearAllGrids()
                    Me.UltraGridItemMaintenance.BeginUpdate()
                    Me.UltraGridPriceUpload.BeginUpdate()
                    Me.UltraGridCostUpload.BeginUpdate()

                    Me.Cursor = Cursors.WaitCursor
                    Dim gridsByUploadTypeCollection As SortableHashlist = GetGridsByUploadTypeCollection()

                    Dim itemLoadRowCount As Integer = Me.ItemLoadDataSet.Tables(0).Rows.Count

                    ' reset the progress properties
                    Me.EIMManager.ProgressComplete = False
                    Me.EIMManager.ProgressCounter = 0

                    ' spin off the item load on another thread so this UI thread
                    ' can poll the progress
                    ' this approach was chosen over a singly threaded event-based approach
                    ' because intermittent polling will add less overhead than
                    ' handling a synchronous event that updates the progress bar
                    Dim theItemLoadThread As New Thread(AddressOf Me.ItemLoadThreaded)
                    theItemLoadThread.Start()

                    ' show the progress dialog but first wait a bit first
                    ' and then check to see if the action is complete to avoid
                    ' flashing the dialog when the progress is too quick
                    Thread.Sleep(500)
                    If Not Me.EIMManager.ProgressComplete And Not Me.AnErrorHasOccurred Then
                        Dim theProgressDialog As ProgressDialog = ProgressDialog.OpenProgressDialog(Me, "Loading Items into a new Session", _
                              "Please Stand By...", "rows", itemLoadRowCount, 0)

                        ' poll for the collection's progress
                        While Not Me.EIMManager.ProgressComplete And Not Me.AnErrorHasOccurred
                            Thread.Sleep(50)
                            If Not IsNothing(theProgressDialog) Then
                                theProgressDialog.UpdateProgressDialogValue(Me.EIMManager.ProgressCounter)
                            End If
                        End While
                        If Not IsNothing(theProgressDialog) Then
                            theProgressDialog.CloseProgressDialog()
                        End If

                        Me.Cursor = Cursors.WaitCursor

                    End If

                    If Not Me.AnErrorHasOccurred Then
                        ' clear the items from the items grid when done
                        'Me.UltraGridItems.SetDataBinding(Nothing, Nothing, True)
                        'Me.ItemLoadDataSet.Tables(0).Rows.Clear()
                        'Me.ItemLoadDataSet.Tables(0).AcceptChanges()
                        'Me.UltraGridItems.DataBind()

                        Me.UltraGridItems.BeginUpdate()
                        Dim theRow As UltraGridRow

                        For theRowIndex As Integer = 0 To Me.UltraGridItems.Rows.Count - 1

                            theRow = Me.UltraGridItems.Rows(0)
                            theRow.Delete(False)
                        Next
                        Me.UltraGridItems.EndUpdate()


                        LoadValueListData(False)

                        Me.EIMManager.BindUploadSessionDataToGrids(GetGridsByUploadTypeCollection(), False, True)

                        ValidateSessionData(ValidationTypes.UploadValue)
                        SetTopTab()

                        Me.HasChanged = True

                        Me.UltraGridItemMaintenance.EndUpdate()
                        Me.UltraGridPriceUpload.EndUpdate()
                        Me.UltraGridCostUpload.EndUpdate()

                        ' setup the selections in the store selector controls
                        Me.StoreSelectorPriceUpload.CurrentUploadSession = Me.EIMManager.CurrentUploadSession
                        Me.StoreSelectorCostUpload.CurrentUploadSession = Me.EIMManager.CurrentUploadSession

                    Else
                        ' the user cancelled so restore the previous session
                        Me.EIMManager.RestorePreviousUploadSession()
                        Me.HasChanged = False
                    End If
                End If

            Finally
                Me.Cursor = oldCursor
                Me.Enabled = True

                ManageUploadToItemStoreCheckBox()


                Me.AnErrorHasOccurred = False
            End Try

        End If
    End Sub

    Private Sub ItemLoadThreaded()

        Try
            Me.EIMManager.ItemLoad(CType(Me.UltraGridItems.DataSource, DataView).Table)
        Catch ex As Exception
            UIThreadSafeShowErrorDialog(ex, "An EIM Item Loading Error Has Occurred", "An error occurred while items were being loaded in an EIM Session.")
        End Try

    End Sub

    Private _isImporting As Boolean

    Private Sub LoadValueListData(ByVal inIsImporting As Boolean)

        Dim oldCursor As Cursor = Me.Cursor
        Me.Cursor = Cursors.WaitCursor

        Try
            Me.Cursor = Cursors.WaitCursor

            Me._isImporting = inIsImporting
            Me.LoadValueListDataProgressComplete = False

            ' spin off the load on another thread so this UI thread
            ' can poll the progress
            ' this approach was chosen over a singly threaded event-based approach
            ' because intermittent polling will add less overhead than
            ' handling a synchronous event that updates the progress bar
            Dim theLoadValueListDataThread As New Thread(AddressOf Me.LoadValueListDataThreaded)
            theLoadValueListDataThread.Start()

            ' show the progress dialog but first wait a bit first
            ' and then check to see if the action is complete to avoid
            ' flashing the dialog when the progress is too quick
            Thread.Sleep(1000)
            If Not Me.LoadValueListDataProgressComplete Then
                Dim theProgressDialog As ProgressDialog = ProgressDialog.OpenProgressDialog(Me, "Loading Value List Data", _
                      "Please Stand By...", "grid rows", ProgressBarStyle.Marquee, 1, 0)

                ' poll for the collection's progress
                While Not Me.LoadValueListDataProgressComplete And Not Me.AnErrorHasOccurred
                    Thread.Sleep(100)
                    If Not IsNothing(theProgressDialog) Then
                        ' this simply refreshes the dialog
                        theProgressDialog.UpdateProgressDialogValue(1)
                    End If
                End While

                If Not IsNothing(theProgressDialog) Then
                    theProgressDialog.CloseProgressDialog()
                End If

                Me.Cursor = Cursors.WaitCursor
            End If

            If Me.AnErrorHasOccurred Then
                ManageUIAppearance()
            End If

        Finally
            Me.Cursor = oldCursor
            Me.AnErrorHasOccurred = False
            Me.Enabled = True
        End Try

    End Sub


    Private Sub LoadValueListDataThreaded()

        Try

            Me.EIMManager.LoadValueListData(Me._isImporting)

        Catch ex As Exception
            UIThreadSafeShowErrorDialog(ex, "An Error Has Occurred", "An error occurred while an EIM Session data was being bound to the grids. " + _
                "You will not be able to load or import data into the grids.")

            Me.AnErrorHasOccurred = True
            Me.AllowUpload = False
        Finally
            Me.LoadValueListDataProgressComplete = True
        End Try

    End Sub

    Private Sub ManageUploadToItemStoreCheckBox()
        ' only if the user has selected the price and/or cost upload(s)
        If Not IsNothing(Me.EIMManager.CurrentUploadSession.FindUploadSessionUploadType(EIM_Constants.PRICE_UPLOAD_CODE, True)) Or _
                Not IsNothing(Me.EIMManager.CurrentUploadSession.FindUploadSessionUploadType(EIM_Constants.COST_UPLOAD_CODE, True)) Then

            ' make sure there is a store no column for both the
            ' price and cost grids if they have rows
            Dim hasRequiredStoreAttributeForPrice As Boolean = _
                Me.UltraGridPriceUpload.Rows.Count = 0 OrElse Me.UltraGridPriceUpload.DisplayLayout.Bands(0).Columns.Exists(EIM_Constants.STORE_NO_ATTR_KEY)

            Dim hasRequiredStoreAttributeForCost As Boolean = _
                Me.UltraGridCostUpload.Rows.Count = 0 OrElse Me.UltraGridCostUpload.DisplayLayout.Bands(0).Columns.Exists(EIM_Constants.STORE_NO_ATTR_KEY)

            If Not (hasRequiredStoreAttributeForPrice And hasRequiredStoreAttributeForCost) Then
                Me.CheckBoxUploadToItemStore.Checked = False
            End If

            Me.CheckBoxUploadToItemStore.Enabled = hasRequiredStoreAttributeForPrice And hasRequiredStoreAttributeForCost

        End If
    End Sub

    Public Sub SetUploadExclusionCheckBox()
        ' if validation level is invalid, check and disable upload exclusion check box
        ' if validation level is valid, disable check box
        ' If validation level is warning and  user checked checkbox for warning for upload exclusion before, set it checked. Otherwise clear it.

        '02/15/2011
        'there are three types of validation in EIM: for grid row, for whole session, for database: 
        'handleCellUpdate, ValidateSessionData, ValidateforUpload
        'if user flagged checkbox with warning and maybe made row changes, then uploaded session again, 
        'it is possible that grid row or session validation doesn't have a warning or error 
        'so checkbox will be cleared. 
        'After that database validation can generate warning again. 
        'We need to remember if warning checkbox was flagged by a user before the upload.
        'If yes, cleared warning checkbox has to be flagged again after all validations.
        'See UltraGridUpload_AfterCellUpdate, HandleCellUpdate.

        Try
            Me.UltraGridItems.BeginUpdate()
            Me.Cursor = Cursors.WaitCursor

            For Each theUploadRowHolder As UploadRowHolder In Me.EIMManager.CurrentUploadRowHolderCollecton.UploadRowHolderList

                For Each theGridAnddataRowHolder As GridAndDataRowHolder In theUploadRowHolder.GridAndDataRowList

                    Dim upload_exclusionGridCell As UltraGridCell = theGridAnddataRowHolder.GridRow.Cells(EIM_Constants.UPLOAD_EXCLUSION_COLUMN)

                    If theUploadRowHolder.ValidationErrors.Count > 0 Then

                        theUploadRowHolder.UploadRow.isUploadExclusion = True
                        upload_exclusionGridCell.Value = True
                        upload_exclusionGridCell.Activation = Activation.Disabled

                    ElseIf theUploadRowHolder.ValidationWarnings.Count > 0 Then

                        If theUploadRowHolder.UploadRow.isUploadExclusionWarningChecked = True Then
                            theUploadRowHolder.UploadRow.isUploadExclusion = True
                            upload_exclusionGridCell.Value = True
                        Else
                            theUploadRowHolder.UploadRow.isUploadExclusion = False
                            upload_exclusionGridCell.Value = False
                        End If
                        upload_exclusionGridCell.Activation = Activation.AllowEdit

                    ElseIf theUploadRowHolder.ValidationErrors.Count = 0 And theUploadRowHolder.ValidationWarnings.Count = 0 Then
                        theUploadRowHolder.UploadRow.isUploadExclusion = False
                        upload_exclusionGridCell.Value = False
                        upload_exclusionGridCell.Activation = Activation.Disabled
                    End If
                Next
            Next


            For Each theUploadRow As UploadRow In Me.EIMManager.CurrentUploadSession.UploadRowCollection
                For Each theUploadValue As UploadValue In theUploadRow.UploadValueCollection
                    If theUploadValue.Key.Equals(EIM_Constants.UPLOAD_EXCLUSION_COLUMN) Then
                        theUploadValue.Value = theUploadRow.isUploadExclusion
                        Exit For
                    End If
                Next
            Next


        Catch ex As Exception

        Finally
            Me.UltraGridItems.EndUpdate()
            ManageUIAppearance()
            Me.Cursor = Cursors.Default

        End Try

    End Sub

    ''' <summary>
    ''' Set the enabled state of the action buttons and the boldness of the 
    ''' upload tabs according to whether there is and the type of session present.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ManageUIAppearance()

        ' manage the session search tab
        If Me.UltraGridUploadSessions.Rows.Count > 0 Then
            Me.TabPageLoadItems.Text = "Saved Sessions - " + Me.UltraGridUploadSessions.Rows.Count.ToString("###,###,###") + " found"
            Me.TabPageLoadItems.ToolTipText = "There are " + Me.UltraGridUploadSessions.Rows.Count.ToString("###,###,###") + _
                UIUtilities.MakeSimpleSingularOrPlural(" session", Me.UltraGridUploadSessions.Rows.Count)

            Me.ButtonSessionLoad.Enabled = True
            Me.ButtonSessionDelete.Enabled = True
        Else
            Me.TabPageLoadItems.Text = "Saved Sessions"
            Me.TabPageLoadItems.ToolTipText = "There are no sessions."

            Me.ButtonSessionLoad.Enabled = False
            Me.ButtonSessionDelete.Enabled = False
        End If

        ' manage the item load tab
        If Me.UltraGridItems.Rows.Count > 0 Then
            Me.TabPageLoadItems.Text = "Item Load - " + Me.UltraGridItems.Rows.Count.ToString("###,###,###") + _
                UIUtilities.MakeSimpleSingularOrPlural(" row", Me.UltraGridItems.Rows.Count)

            Me.TabPageLoadItems.ToolTipText = "There are " + Me.UltraGridItems.Rows.Count.ToString("###,###,###") + _
                UIUtilities.MakeSimpleSingularOrPlural(" row", Me.UltraGridItems.Rows.Count)

            Me.ButtonItemLoad.Enabled = True
        Else
            Me.TabPageLoadItems.Text = "Item Load"
            Me.TabPageLoadItems.ToolTipText = "There are no items."

            Me.ButtonItemLoad.Enabled = False
        End If

        ' manage the upload tabs and the session action buttons
        Dim hasSessionWithData As Boolean = False
        If Not IsNothing(Me.EIMManager.CurrentUploadSession) Then
            If Me.EIMManager.CurrentUploadSession.ItemsLoadedCount > 0 Then
                hasSessionWithData = True
            End If
        End If

        Me.Text = "IRMA Extended Item Maintenance"

        If hasSessionWithData Then

            Me._ignoreSessionTypeChange = True
            Me.CheckBoxNewItemSession.Checked = Me.EIMManager.CurrentUploadSession.IsNewItemSessionFlag
            Me._ignoreSessionTypeChange = False

            If Me.EIMManager.CurrentUploadSession.IsNewItemSessionFlag Then
                If Me.CheckBoxLoadFromSLIM.Checked Then
                    Me.Text = Me.Text + " - SLIM"
                Else
                    Me.Text = Me.Text + " - Existing"
                End If
            Else
                Me.Text = Me.Text + " - Existing"
            End If

            Me.Text = Me.Text + " Item Session"

            Dim theDataTable As DataTable
            Dim theItemMaintenanceCount As Integer = 0
            Dim thePriceUploadCount As Integer = 0
            Dim theCostUploadCount As Integer = 0
            Dim itemMaintenanceErrorCount As Integer = 0
            Dim thePriceUploadErrorCount As Integer = 0
            Dim theCostUploadErrorCount As Integer = 0
            Dim itemMaintenanceWarningCount As Integer = 0
            Dim thePriceUploadWarningCount As Integer = 0
            Dim theCostUploadWarningCount As Integer = 0

            theDataTable = Me.EIMManager.CurrentUploadSession.DataSet.Tables(EIM_Constants.ITEM_MAINTENANCE_CODE)
            If Not IsNothing(theDataTable) Then
                theItemMaintenanceCount = theDataTable.Select(EIM_Constants.IS_HIDDEN_COLUMN_NAME + " = False").Length
                itemMaintenanceErrorCount = theDataTable.Select(EIM_Constants.VALIDATION_LEVEL_COLUMN_NAME + " = 2 and " + _
                        EIM_Constants.IS_HIDDEN_COLUMN_NAME + " = False", "", DataViewRowState.CurrentRows).Length
                itemMaintenanceWarningCount = theDataTable.Select(EIM_Constants.VALIDATION_LEVEL_COLUMN_NAME + " = 1 and " + _
                        EIM_Constants.IS_HIDDEN_COLUMN_NAME + " = False", "", DataViewRowState.CurrentRows).Length
            End If

            theDataTable = Me.EIMManager.CurrentUploadSession.DataSet.Tables(EIM_Constants.PRICE_UPLOAD_CODE)
            If Not IsNothing(theDataTable) Then
                thePriceUploadCount = theDataTable.Rows.Count
                thePriceUploadErrorCount = theDataTable.Select(EIM_Constants.VALIDATION_LEVEL_COLUMN_NAME + " = 2", "", DataViewRowState.CurrentRows).Length
                thePriceUploadWarningCount = theDataTable.Select(EIM_Constants.VALIDATION_LEVEL_COLUMN_NAME + " = 1", "", DataViewRowState.CurrentRows).Length
            End If

            theDataTable = Me.EIMManager.CurrentUploadSession.DataSet.Tables(EIM_Constants.COST_UPLOAD_CODE)
            If Not IsNothing(theDataTable) Then
                theCostUploadCount = theDataTable.Rows.Count
                theCostUploadErrorCount = theDataTable.Select(EIM_Constants.VALIDATION_LEVEL_COLUMN_NAME + " = 2", "", DataViewRowState.CurrentRows).Length
                theCostUploadWarningCount = theDataTable.Select(EIM_Constants.VALIDATION_LEVEL_COLUMN_NAME + " = 1", "", DataViewRowState.CurrentRows).Length
            End If

            If theItemMaintenanceCount > 0 Then
                Me.TabPageItems.Text = "Items - " + theItemMaintenanceCount.ToString("###,###,###") + _
                    UIUtilities.MakeSimpleSingularOrPlural(" row", theItemMaintenanceCount)

                Me.TabPageItems.ToolTipText = "There are " + theItemMaintenanceCount.ToString("###,###,###") + " Items."
            Else
                Me.TabPageItems.Text = "Items"
                Me.TabPageItems.ToolTipText = "There are no Items."
            End If

            If thePriceUploadCount > 0 Then
                Me.TabPagePrices.Text = "Prices - " + thePriceUploadCount.ToString("###,###,###") + _
                    UIUtilities.MakeSimpleSingularOrPlural(" row", thePriceUploadCount)

                Me.TabPageItems.ToolTipText = "There are " + thePriceUploadCount.ToString("###,###,###") + " Price Changes."
            Else
                Me.TabPagePrices.Text = "Prices"
                Me.TabPageItems.ToolTipText = "There are no Price Changes."
            End If

            If theCostUploadCount > 0 Then
                Me.TabPageCosts.Text = "Costs - " + theCostUploadCount.ToString("###,###,###") + _
                    UIUtilities.MakeSimpleSingularOrPlural(" row", theCostUploadCount)

                Me.TabPageItems.ToolTipText = "There are " + theCostUploadCount.ToString("###,###,###") + " Cost Changes."
            Else
                Me.TabPageCosts.Text = "Costs"
                Me.TabPageItems.ToolTipText = "There are no Cost Changes."
            End If


            If itemMaintenanceWarningCount > 0 Then
                Me.TabPageItems.Text = Me.TabPageItems.Text + _
                        " - " + itemMaintenanceWarningCount.ToString("###,###,###") + _
                        UIUtilities.MakeSimpleSingularOrPlural(" warning", itemMaintenanceWarningCount)
            End If

            If thePriceUploadWarningCount > 0 Then
                Me.TabPagePrices.Text = Me.TabPagePrices.Text + _
                    " - " + thePriceUploadWarningCount.ToString("###,###,###") + _
                    UIUtilities.MakeSimpleSingularOrPlural(" warning", thePriceUploadWarningCount)
            End If

            If theCostUploadWarningCount > 0 Then
                Me.TabPageCosts.Text = Me.TabPageCosts.Text + _
                    " - " + theCostUploadWarningCount.ToString("###,###,###") + _
                    UIUtilities.MakeSimpleSingularOrPlural(" warning", theCostUploadWarningCount)
            End If

            If itemMaintenanceErrorCount > 0 Then
                Me.TabPageItems.Text = Me.TabPageItems.Text + _
                        " " + itemMaintenanceErrorCount.ToString("###,###,###") + _
                        UIUtilities.MakeSimpleSingularOrPlural(" error", itemMaintenanceErrorCount)
            End If

            If thePriceUploadErrorCount > 0 Then
                Me.TabPagePrices.Text = Me.TabPagePrices.Text + _
                    " " + thePriceUploadErrorCount.ToString("###,###,###") + _
                    UIUtilities.MakeSimpleSingularOrPlural(" error", thePriceUploadErrorCount)
            End If

            If theCostUploadErrorCount > 0 Then
                Me.TabPageCosts.Text = Me.TabPageCosts.Text + _
                    " " + theCostUploadErrorCount.ToString("###,###,###") + _
                    UIUtilities.MakeSimpleSingularOrPlural(" error", theCostUploadErrorCount)
            End If

            Me.ButtonExport.Enabled = True

            Me.ButtonUpload.Enabled = Me.AllowUpload

            ManageUploadToItemStoreCheckBox()

            If Me.HasChanged Then
                Me.ButtonSave.Enabled = True
            Else
                Me.ButtonSave.Enabled = False
            End If

            If Not Me.EIMManager.CurrentUploadSession.IsNew Then
                Me.ButtonSaveAs.Enabled = True
            Else
                Me.ButtonSaveAs.Enabled = False
            End If

            If Me.EIMManager.CurrentUploadSession.IsNewItemSessionFlag Then
                Me._ignoreSessionTypeChange = True
                Me.CheckBoxNewItemSession.Checked = True
                Me._ignoreSessionTypeChange = False

                Me.CheckBoxNewItemSession.Enabled = False
            End If

        Else
            Me.TabPageItems.Text = "Item Maintenance"
            Me.TabPageItems.ToolTipText = "There are no Item Maintenance items."
            Me.TabPageItems.ToolTipText = "There are no Price Upload items."
            Me.TabPagePrices.Text = "Price Upload"
            Me.TabPageItems.ToolTipText = "There are no Cost Upload items."
            Me.TabPageCosts.Text = "Cost Upload"
            Me.TabPageItems.ToolTipText = "There are no Cost Upload items."

            Me.ButtonExport.Enabled = False
            Me.ButtonSave.Enabled = False
            Me.ButtonSaveAs.Enabled = False
            Me.ButtonUpload.Enabled = False
            Me.CheckBoxUploadToItemStore.Enabled = False
            Me._ignoreSessionTypeChange = True
            Me.CheckBoxNewItemSession.Checked = False
            Me._ignoreSessionTypeChange = False
            Me.CheckBoxNewItemSession.Enabled = False
        End If

    End Sub

#End Region

#Region "Validation Methods"

    Private Sub ValidateSessionData(ByVal inValidationType As ValidationTypes)

        Dim oldCursor As Cursor = Me.Cursor
        Me.Cursor = Cursors.WaitCursor

        _validationType = inValidationType

        Try
            Me.Cursor = Cursors.WaitCursor

            ' for the session load we are tracking the progress of the UploadSession.BuildDataSet()
            ' method as it triggers the just-in-time instantiation of UploadRows and their values
            ' from the database
            Me.EIMManager.CalcValidateManager.ProgressComplete = False

            Dim gridCollection As SortableHashlist = GetGridsByUploadTypeCollection()

            ' suspend the grid update during validation
            Me.UltraGridItemMaintenance.BeginUpdate()
            Me.UltraGridPriceUpload.BeginUpdate()
            Me.UltraGridCostUpload.BeginUpdate()

            Dim gridRowsToValidateCount As Integer = CType(gridCollection.Item(0), UltraGrid).Rows.Count * gridCollection.Count

            ' spin off the load on another thread so this UI thread
            ' can poll the progress
            ' this approach was chosen over a singly threaded event-based approach
            ' because intermittent polling will add less overhead than
            ' handling a synchronous event that updates the progress bar
            Dim theValidateSessionDataThread As New Thread(AddressOf Me.ValidateSessionDataThreaded)
            theValidateSessionDataThread.Start()

            ' show the progress dialog but first wait a bit first
            ' and then check to see if the action is complete to avoid
            ' flashing the dialog when the progress is too quick
            Thread.Sleep(1000)
            If Not Me.EIMManager.CalcValidateManager.ProgressComplete Then
                Dim theProgressDialog As ProgressDialog = ProgressDialog.OpenProgressDialog(Me, "Validating your Session Data", _
                      "Please Stand By...", Nothing, ProgressBarStyle.Blocks, gridRowsToValidateCount, 0)

                ' poll for the collection's progress
                While Not Me.EIMManager.CalcValidateManager.ProgressComplete And Not Me.AnErrorHasOccurred
                    Thread.Sleep(100)
                    If Not IsNothing(theProgressDialog) Then
                        theProgressDialog.UpdateProgressDialogValue(Me.EIMManager.CalcValidateManager.ProgressCounter * gridCollection.Count)
                    End If
                End While

                If Not IsNothing(theProgressDialog) Then
                    theProgressDialog.CloseProgressDialog()
                End If

                Me.Cursor = Cursors.WaitCursor
            End If

            If Not Me.AnErrorHasOccurred Then
                Me.HasChanged = True
            Else
                ManageUIAppearance()
            End If

        Finally
            ' resume the grid update
            Me.UltraGridItemMaintenance.EndUpdate()
            Me.UltraGridPriceUpload.EndUpdate()
            Me.UltraGridCostUpload.EndUpdate()

            Me.EIMManager.CurrentUploadSession.DataSet.AcceptChanges()

            Me.Cursor = oldCursor
            Me.AnErrorHasOccurred = False
            Me.Enabled = True

            If inValidationType = 1 Then
                SetUploadExclusionCheckBox()
            End If

        End Try

    End Sub

    Private _validationType As ValidationTypes = ValidationTypes.UploadValue
    Private _isChangingUploadToStoresCheckBox As Boolean = False

    Private Sub ValidateSessionDataThreaded()

        Try

            Me.EIMManager.Validate(Me._validationType, Me._isChangingUploadToStoresCheckBox)
        Catch ex As Exception
            UIThreadSafeShowErrorDialog(ex, "An EIM Validation Error Has Occurred", "An error occurred while an EIM Session was being validated. " + _
                "You will not be able to upload your session, but you can save or export it to upload later.")

            Me.AllowUpload = False
        End Try

    End Sub

    Private Sub ValidateForUpload()

        Dim oldCursor As Cursor = Me.Cursor
        Me.Cursor = Cursors.WaitCursor

        Try
            Me.Cursor = Cursors.WaitCursor

            ' for the session load we are tracking the progress of the UploadSession.BuildDataSet()
            ' method as it triggers the just-in-time instantiation of UploadRows and their values
            ' from the database
            Me.EIMManager.CalcValidateManager.ProgressComplete = False
            Me.EIMManager.CalcValidateManager.ProgressCounter = 0

            Dim gridCollection As SortableHashlist = GetGridsByUploadTypeCollection()

            ' suspend the grid update during validation
            Me.UltraGridItemMaintenance.BeginUpdate()
            Me.UltraGridPriceUpload.BeginUpdate()
            Me.UltraGridCostUpload.BeginUpdate()

            Dim gridRowsToValidateCount As Integer = CType(gridCollection.Item(0), UltraGrid).Rows.Count * gridCollection.Count

            ' spin off the load on another thread so this UI thread
            ' can poll the progress
            ' this approach was chosen over a singly threaded event-based approach
            ' because intermittent polling will add less overhead than
            ' handling a synchronous event that updates the progress bar
            Dim theValidateForUploadThread As New Thread(AddressOf Me.ValidateForUploadThreaded)
            theValidateForUploadThread.Start()

            ' show the progress dialog but first wait a bit first
            ' and then check to see if the action is complete to avoid
            ' flashing the dialog when the progress is too quick
            Thread.Sleep(500)
            If Not Me.EIMManager.CalcValidateManager.ProgressComplete Then
                Dim theProgressDialog As ProgressDialog = ProgressDialog.OpenProgressDialog(Me, "Validating your Price Changes", _
                      "Please Stand By...", "grid rows", ProgressBarStyle.Marquee, gridRowsToValidateCount, 0)

                ' poll for the collection's progress
                While Not Me.EIMManager.CalcValidateManager.ProgressComplete And Not Me.AnErrorHasOccurred
                    Thread.Sleep(100)
                    If Not IsNothing(theProgressDialog) Then
                        theProgressDialog.UpdateProgressDialogValue(Me.EIMManager.CalcValidateManager.ProgressCounter)
                    End If
                End While

                If Not IsNothing(theProgressDialog) Then
                    theProgressDialog.CloseProgressDialog()
                End If

                Me.Cursor = Cursors.WaitCursor
            End If

            If Me.AnErrorHasOccurred Then
                ManageUIAppearance()
            End If

        Finally
            ' resume the grid update
            Me.UltraGridItemMaintenance.EndUpdate()
            Me.UltraGridPriceUpload.EndUpdate()
            Me.UltraGridCostUpload.EndUpdate()

            Me.Cursor = oldCursor
            Me.AnErrorHasOccurred = False
            Me.Enabled = True
        End Try

    End Sub

    Private Sub ValidateForUploadThreaded()

        ValidateForUploadNonThreaded()

    End Sub

    Private Sub ValidateForUploadNonThreaded()

        Try

            Dim theStoreList As String = Nothing

            If Me.StoreSelectorPriceUpload.AllowStoreSelection Then
                theStoreList = Me.StoreSelectorPriceUpload.SelectedStoreIdString
            End If

            Me.EIMManager.ValidateForUpload(theStoreList, Me.CheckBoxUploadToItemStore.Checked)

        Catch ex As Exception
            UIThreadSafeShowErrorDialog(ex, "An EIM Validation Error Has Occurred", "An error occurred while an EIM Session was being validated for duplicate price changes. " + _
                "You will not be able to upload your session, but you can save or export it to upload later.")

            Me.AllowUpload = False
        End Try

    End Sub

#End Region

#Region "Support Methods"

    Private Function GetItemKeyForGridRow(ByVal inRow As UltraGridRow) As Integer
        Return CInt(inRow.Cells(EIM_Constants.ITEM_KEY_ATTR_KEY).Value)
    End Function

    Private Function GetItemIdentifierForGridRow(ByVal inRow As UltraGridRow) As String
        Return CStr(inRow.Cells(EIM_Constants.ITEMIDENTIFIER_IDENTIFIER_ATTR_KEY).Value)
    End Function

    Private Function GetUploadRowIdForGridRow(ByVal inRow As UltraGridRow) As Integer
        Return CInt(inRow.Cells(EIM_Constants.UPLOADROW_ID_COLUMN_NAME).Value)
    End Function

    Private Function GetUploadRowForGridRow(ByVal inRow As UltraGridRow) As UploadRowHolder
        Return Me.EIMManager.CurrentUploadRowHolderCollecton.GetUploadRowHolderForUploadRowId(GetUploadRowIdForGridRow(inRow))
    End Function

    Private Sub SetCellValue(ByVal inRow As UltraGridRow, ByVal inKey As String, _
            ByVal inValue As String)

        Dim theCell As UltraGridCell = Nothing

        Me.CalculatingCellValues = True
        If GridUtilities.TryGetGridCell(inKey, inRow, theCell) Then
            If String.IsNullOrEmpty(inValue) Or IsNothing(inValue) Then
                theCell.Value = DBNull.Value
            Else
                theCell.Value = inValue
            End If
        End If
        Me.CalculatingCellValues = False

    End Sub

    Private Function GetCellValue(ByVal inRow As UltraGridRow, ByVal inKey As String) As String

        Dim theValue As String
        Dim theCell As UltraGridCell
        Dim theUploadRowHolder As UploadRowHolder = GetUploadRowForGridRow(inRow)
        Dim theUploadValue As UploadValue = theUploadRowHolder.UploadRow.FindUploadValueByAttributeKey(inKey)

        theValue = Nothing
        theCell = Nothing

        If GridUtilities.TryGetGridCell(inKey, inRow, theCell) Then

            If Not Object.Equals(theCell.Value, DBNull.Value) Then
                theValue = CStr(theCell.Value)
            End If

            If String.IsNullOrEmpty(theValue) Or IsNothing(theValue) Then
                If Not IsNothing(theUploadValue) Then
                    If theUploadValue.DbDataType.ToLower() = "bit" Then
                        theValue = "False"
                    ElseIf theUploadValue.UploadAttribute.IsNumeric() Then
                        theValue = "0"
                    Else
                        theValue = ""
                    End If
                Else
                    theValue = ""
                End If
            End If
        End If

        Return theValue
    End Function

    Private Function GetIntegerCellValue(ByVal inRow As UltraGridRow, ByVal inKey As String) As Integer

        Dim theIntegerValue As Integer
        Dim theIntegerStringValue As String = GetCellValue(inRow, inKey)

        If Not Integer.TryParse(theIntegerStringValue, theIntegerValue) Then
            theIntegerValue = -1
        End If

        Return theIntegerValue

    End Function

    Private Function GetBooleanCellValue(ByVal inRow As UltraGridRow, ByVal inKey As String) As Boolean

        Dim theBooleanValue As Boolean = False
        Dim theBooleanStringValue As String = GetCellValue(inRow, inKey)

        If Not Boolean.TryParse(theBooleanStringValue, theBooleanValue) Then
            theBooleanValue = False
        End If

        Return theBooleanValue

    End Function

    Private Function GetUploadValueByKeyForGridRow(ByVal inRow As UltraGridRow, ByVal inKey As String) As UploadValue

        Dim theUploadRowHolder As UploadRowHolder = GetUploadRowForGridRow(inRow)
        Dim theUploadValue As UploadValue = theUploadRowHolder.UploadRow.FindUploadValueByAttributeKey(inKey)

        Return theUploadValue
    End Function

    Private Function GetSelectedUploadTypesFromUser(ByVal sessionAction As SessionActions) As Boolean

        Dim theSelectUploadTypesForm As New SelectUploadTypesForm()

        theSelectUploadTypesForm.SessionAction = sessionAction
        theSelectUploadTypesForm.CurrentUploadSession = Me.EIMManager.CurrentUploadSession

        Dim theDialogResult As DialogResult = theSelectUploadTypesForm.ShowDialog(Me)

        If theDialogResult = Windows.Forms.DialogResult.OK Then
            Me.EIMManager.CurrentUploadSession.Name = theSelectUploadTypesForm.SessionName
        End If

        theSelectUploadTypesForm.Dispose()

        ManageUIAppearance()

        Return theDialogResult = Windows.Forms.DialogResult.OK

    End Function

    Private Function GetGridsByUploadTypeCollection() As SortableHashlist
        Dim gridsByUploadTypeCollection As New SortableHashlist()

        gridsByUploadTypeCollection.Add(EIM_Constants.ITEM_MAINTENANCE_CODE, Me.UltraGridItemMaintenance)
        gridsByUploadTypeCollection.Add(EIM_Constants.PRICE_UPLOAD_CODE, Me.UltraGridPriceUpload)
        gridsByUploadTypeCollection.Add(EIM_Constants.COST_UPLOAD_CODE, Me.UltraGridCostUpload)

        Return gridsByUploadTypeCollection
    End Function

    ''' <summary>
    ''' *** This is where data entry is handled.
    ''' </summary>
    ''' <param name="inGridCell"></param>
    ''' <param name="inResizeColumn"></param>
    ''' <remarks></remarks>
    Private Sub HandleCellUpdate(ByRef inGridCell As UltraGridCell, ByVal inResizeColumn As Boolean)

        Dim theAttributeKey As String = inGridCell.Column.Key

        If Not CalculatingCellValues And Not Me._isInitializingRow Then

            Try
                Me.Cursor = Cursors.WaitCursor

                CalculatingCellValues = True

                Me.UltraGridItemMaintenance.BeginUpdate()
                Me.UltraGridPriceUpload.BeginUpdate()
                Me.UltraGridCostUpload.BeginUpdate()

                Dim theCurrentGridRow As UltraGridRow = inGridCell.Row
                Dim theCurrentItemKey As Integer = GetItemKeyForGridRow(theCurrentGridRow)
                Dim theCurrentIdentifier As String = GetItemIdentifierForGridRow(theCurrentGridRow)
                'Dim theAttributeKey As String = inGridCell.Column.Key

                Dim theUploadRowHolder As UploadRowHolder = GetUploadRowForGridRow(theCurrentGridRow)

                Dim theUploadValue As UploadValue = _
                        theUploadRowHolder.UploadRow.FindUploadValueByAttributeKey(theAttributeKey)

                Dim theCurrentCellValue As Object = inGridCell.Value
                Dim theGridCell As UltraGridCell = Nothing
                Dim theGrid As UltraGrid = CType(inGridCell.Band.Layout.Grid, UltraGrid)

                ' clear any existing validation errors for the row
                ' theUploadRowHolder.ClearAllValidationLevels()

                ' copy the value to all corresponding grid cells ACROSS the grids
                For Each theGridAndDataRowHolder As GridAndDataRowHolder In theUploadRowHolder.GridAndDataRowList

                    If GridUtilities.TryGetGridCell(theAttributeKey, theGridAndDataRowHolder.GridRow, theGridCell) Then

                        '02/14/2011 delete_vendor and deauth_store checkbox values are nothing when spreadsheet for COST_UPLOAD is loaded
                        If IsNothing(theUploadValue) And (theAttributeKey = "calculated_delete_vendor" Or theAttributeKey = "calculated_deauth_store") Then
                            theGridCell.Value = theCurrentCellValue
                        Else

                            If theUploadValue.Size > EIM_Constants.LONG_TEXT_SIZE Then
                                ' fix the log text columns' width
                                theGridCell.Column.Width = EIM_Constants.LONG_TEXT_COLUMN_WIDTH
                            Else

                                theGridCell.Value = theCurrentCellValue

                                ' resize the column to fit the data
                                theGridCell.Column.PerformAutoResize()
                            End If


                        End If

                    End If
                Next

                ' mark the row for identifier validation if the changed cell
                ' is the identifier
                If theAttributeKey.Equals(EIM_Constants.ITEMIDENTIFIER_IDENTIFIER_ATTR_KEY) Then
                    theUploadRowHolder.UploadRow.HasValidatedIdentifier = False

                    ' also update the UploadRow.Identifier property
                    theUploadRowHolder.UploadRow.Identifier = CStr(theGridCell.Value)
                End If

                ' mark the row for linked identifier validation if the changed cell
                ' is the linked identifier
                If theAttributeKey.Equals(EIM_Constants.PRICE_LINKED_ITEM_IDENTIFIER_ATTR_KEY) Then
                    theUploadRowHolder.UploadRow.HasValidatedLinkedIdentifier = False
                End If

                ' recalculate other cell values in this and
                ' related grid rows as needed
                Me.EIMManager.RecalculateRowValues(inGridCell)

                Me.EIMManager.CurrentUploadSession.DataSet.AcceptChanges()
                Me.EIMManager.ValidateGridRowsForUploadRow(theUploadRowHolder, ValidationTypes.GridCell)


                If Not IsNothing(theUploadValue) AndAlso Not IsNothing(theUploadValue.UploadAttribute) Then

                    ' only copy cell values to other rows in the current and other grids
                    ' for the same item if the value is at the item level, i.e. the same for the item
                    ' regardless of the store
                    If Me.EIMManager.CopyToOtherRowsForItem(theUploadValue) Then

                        ' loop over all rows in the grids for the same item
                        Dim theUploadRowHolderListForItem As ArrayList = _
                                Me.EIMManager.CurrentUploadRowHolderCollecton.GetUploadRowHolderListForIdentifier(theCurrentIdentifier)

                        Dim usesStoreJurisdictions As Boolean = InstanceDataDAO.IsFlagActiveCached("UseStoreJurisdictions")

                        Dim theCurrentIsDefaultJurisdictionFlag As Boolean = GridUtilities.GetGridCellBooleanValue(EIM_Constants.ITEM_IS_DEFAULT_JURISDICTION_ATTR_KEY, theCurrentGridRow)
                        Dim theCurrentJurisdictionId As Integer = GridUtilities.GetGridCellIntegerValue(EIM_Constants.ITEM_JURISDICTION_ID_ATTR_KEY, theCurrentGridRow)

                        Dim theOtherIsDefaultJurisdictionFlag As Boolean
                        Dim theOtherJurisdictionId As Integer

                        For Each theOtherUploadRowHolder As UploadRowHolder In theUploadRowHolderListForItem

                            For Each theGridAndDataRowHolder As GridAndDataRowHolder In theOtherUploadRowHolder.GridAndDataRowList

                                ' only if the column is in the grid
                                If GridUtilities.TryGetGridCell( _
                                        inGridCell.Column.Key, theGridAndDataRowHolder.GridRow, theGridCell) Then

                                    theOtherIsDefaultJurisdictionFlag = GridUtilities.GetGridCellBooleanValue(EIM_Constants.ITEM_IS_DEFAULT_JURISDICTION_ATTR_KEY, theGridAndDataRowHolder.GridRow)
                                    theOtherJurisdictionId = GridUtilities.GetGridCellIntegerValue(EIM_Constants.ITEM_JURISDICTION_ID_ATTR_KEY, theGridAndDataRowHolder.GridRow)

                                    If Not usesStoreJurisdictions OrElse Not theUploadValue.UploadAttribute.IsJurisdictionAttribute OrElse _
                                               (usesStoreJurisdictions And theCurrentIsDefaultJurisdictionFlag = theOtherIsDefaultJurisdictionFlag And _
                                                   theCurrentJurisdictionId = theOtherJurisdictionId) Then

                                        theGridCell.Value = inGridCell.Value

                                        If theUploadValue.Size > EIM_Constants.LONG_TEXT_SIZE Then
                                            ' fix the log text columns' width
                                            theGridCell.Column.Width = EIM_Constants.LONG_TEXT_COLUMN_WIDTH
                                        ElseIf inResizeColumn Then

                                            ' resize the column to fit the data
                                            theGridCell.Column.PerformAutoResize()
                                        End If
                                    End If
                                End If
                            Next
                            Me.EIMManager.ValidateGridRowsForUploadRow(theOtherUploadRowHolder, ValidationTypes.GridCell)
                        Next
                    End If
                End If


                ' 02/24/2011 update uploadExclusion checkboxes here

                SetUploadExclusionCheckBox()

                'If Not theAttributeKey.Equals(EIM_Constants.UPLOAD_EXCLUSION_COLUMN) Then

                '    For Each theGridAndDataRowHolder As GridAndDataRowHolder In theUploadRowHolder.GridAndDataRowList
                '        Dim upload_exclusionGridCell As UltraGridCell = theGridAndDataRowHolder.GridRow.Cells(EIM_Constants.UPLOAD_EXCLUSION_COLUMN)

                '        If theUploadRowHolder.ValidationErrors.Count > 0 Then
                '            upload_exclusionGridCell.Value = True
                '            upload_exclusionGridCell.Activation = Activation.Disabled

                '        ElseIf theUploadRowHolder.ValidationWarnings.Count > 0 Then
                '            'upload_exclusionGridCell.Value = False
                '            upload_exclusionGridCell.Activation = Activation.AllowEdit

                '        ElseIf theUploadRowHolder.ValidationErrors.Count = 0 And theUploadRowHolder.ValidationWarnings.Count = 0 Then
                '            upload_exclusionGridCell.Value = False
                '            upload_exclusionGridCell.Activation = Activation.Disabled
                '        End If

                '    Next
                'End If


            Finally

                Me.UltraGridItemMaintenance.EndUpdate()
                Me.UltraGridPriceUpload.EndUpdate()
                Me.UltraGridCostUpload.EndUpdate()

                CalculatingCellValues = False

                Me.Cursor = Cursors.Default

            End Try

            Me.HasChanged = True

        End If

    End Sub

    ''' <summary>
    ''' Ask the user for the import file name.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetImportSpreadsheetFileName(ByRef fileName As String) As Boolean

        fileName = Me.TextBoxImportFilePath.Text

        ' show the open file dialog
        Me.OpenFileDialogImport.InitialDirectory = My.Application.Info.DirectoryPath & ConfigurationServices.AppSettings("localItemBulkLoadDirectory")
        Me.SaveFileDialogExport.FileName = fileName
        Dim theDialogResult As DialogResult = Me.OpenFileDialogImport.ShowDialog()
        fileName = Me.OpenFileDialogImport.FileName()

        Me.TextBoxImportFilePath.Text = fileName

        Return Not String.IsNullOrEmpty(fileName) And theDialogResult <> Windows.Forms.DialogResult.Cancel

    End Function

    ''' <summary>
    ''' Ask the user for the export file name.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetExportSpreadsheetFileName(ByRef fileName As String) As Boolean

        fileName = Me.TextBoxImportFilePath.Text

        ' show the save file dialog
        Me.SaveFileDialogExport.CheckFileExists = False ' the excel.SaveAs does this
        Me.SaveFileDialogExport.InitialDirectory = My.Application.Info.DirectoryPath & ConfigurationServices.AppSettings("localItemBulkLoadDirectory")
        Me.SaveFileDialogExport.FileName = fileName
        Me.SaveFileDialogExport.CheckFileExists = False
        Me.SaveFileDialogExport.ShowDialog()
        fileName = Me.SaveFileDialogExport.FileName

        Return Not String.IsNullOrEmpty(fileName)

    End Function

    Private Sub SetTopTab()

        ' select the session data tab
        Me.TabControlUpoadPages.SelectedIndex = 2

        ' set the top tab page based on the provided  upload types
        ' give priority to the tabs from left to right
        If Me.EIMManager.CurrentUploadSession.UploadTypeCollection.ContainsKey(EIM_Constants.ITEM_MAINTENANCE_CODE) Then
            Me.TabControlUploadData.SelectedIndex = 0
        ElseIf Me.EIMManager.CurrentUploadSession.UploadTypeCollection.ContainsKey(EIM_Constants.PRICE_UPLOAD_CODE) Then
            Me.TabControlUploadData.SelectedIndex = 1
        ElseIf Me.EIMManager.CurrentUploadSession.UploadTypeCollection.ContainsKey(EIM_Constants.COST_UPLOAD_CODE) Then
            Me.TabControlUploadData.SelectedIndex = 2
        End If
    End Sub

    Private Sub BindItemLoadTabControls()

        Me.Cursor = Cursors.WaitCursor

        ' set created date to null
        Me.DateTimeEditorCreatedDate.Value = Nothing

        ' bind brand combo
        LoadBrand(Me.ComboBoxBrand)
        Me.ComboBoxBrand.Items.Insert(0, New VB6.ListBoxItem("- All -", 0))
        Me.ComboBoxBrand.SelectedIndex = 0

        ' bind vendor combo
        LoadVendors(Me.ComboBoxVendor)
        Me.ComboBoxVendor.Items.Insert(0, New VB6.ListBoxItem("- All -", 0))
        Me.ComboBoxVendor.SelectedIndex = 0

        ' bind dist subteam combo
        LoadDistSubTeam(Me.ComboBoxDistSubteam)
        Me.ComboBoxDistSubteam.Items.Insert(0, New VB6.ListBoxItem("- All -", 0))
        Me.ComboBoxDistSubteam.SelectedIndex = 0

        ' bind item chain combo
        LoadItemChains(Me.ComboBoxItemChain)
        Me.ComboBoxItemChain.Items.Insert(0, New VB6.ListBoxItem("- All -", 0))
        Me.ComboBoxItemChain.SelectedIndex = 0

        ' bind store jurisdiction combo
        UIUtilities.LoadDefautJuisdictionCombo(ComboBoxIsDefaultJurisdiction)

        ' bind store jurisdiction combo
        LoadStoreJurisdictions(ComboBoxJurisdiction)
        Me.ComboBoxJurisdiction.Items.Insert(0, New VB6.ListBoxItem("- All -", 0))
        Me.ComboBoxJurisdiction.SelectedIndex = 0

        Me.Cursor = Cursors.Default
    End Sub

    Private Sub BindSessionLoadControls()
        ' bind upload type combo
        Dim alluploadTypes As BusinessObjectCollection = UploadTypeDAO.Instance.GetAllUploadTypes()
        Dim fauxAllUploadType As New UploadType()
        fauxAllUploadType.UploadTypeCode = "ALL"
        fauxAllUploadType.Name = "- All -"
        alluploadTypes.Add(fauxAllUploadType.UploadTypeCode, fauxAllUploadType)
        alluploadTypes.SortByPropertyValue("UploadTypeCode")

        Me.ComboBoxUploadType.DataSource = alluploadTypes
        Me.ComboBoxUploadType.DisplayMember = "Name"
        Me.ComboBoxUploadType.ValueMember = "UploadTypeCode"
        ' set to Item and lock down
        Me.ComboBoxUploadType.SelectedIndex = 0

        ' bind user combo
        Dim adminList As ArrayList = New ItemMaintenanceBulkLoadDAO().GetItemAdminUserList()
        Dim fauxAllAdminUser As New ItemAdminUserBO()
        fauxAllAdminUser.UserID = -1
        fauxAllAdminUser.UserName = "- All -"
        adminList.Insert(0, fauxAllAdminUser)
        Me.ComboBoxAdmins.DataSource = adminList
        Me.ComboBoxAdmins.DisplayMember = "UserName"
        Me.ComboBoxAdmins.ValueMember = "UserID"
        ' set to Item
        SetAdminUser(Me.ComboBoxAdmins, giUserID)
    End Sub

    ''' <summary>
    ''' Create data sources and bind to this form's controls.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub BindAndPopulateControls()

        BindSessionLoadControls()
        BindItemLoadTabControls()

    End Sub

    ''' <summary>
    ''' Select the admin user in the admin combo corresponding to the
    ''' given user id.
    ''' </summary>
    ''' <param name="comboBoxAdmins"></param>
    ''' <param name="userId"></param>
    ''' <remarks></remarks>
    Private Sub SetAdminUser(ByRef comboBoxAdmins As System.Windows.Forms.ComboBox, ByRef userId As Integer)

        If userId < 1 Then
            comboBoxAdmins.SelectedIndex = -1
        Else
            Dim iLoop As Integer

            For iLoop = 0 To comboBoxAdmins.Items.Count - 1
                '-- See if its the right data
                If CType(comboBoxAdmins.Items(iLoop), ItemAdminUserBO).UserID = userId Then
                    '-- if so then set and exit
                    comboBoxAdmins.SelectedIndex = iLoop
                    Exit For
                End If
            Next iLoop
        End If
    End Sub

    ''' <summary>
    ''' Delete the row for the given item from the provided grid.
    ''' </summary>
    ''' <param name="inRowIdToDelete"></param>
    ''' <remarks></remarks>
    Private Sub DeleteRowFromAllGrids(ByVal inRowIdToDelete As Integer)

        Dim theUploadRowHolder As UploadRowHolder = _
                Me.EIMManager.CurrentUploadRowHolderCollecton.GetUploadRowHolderForUploadRowId(inRowIdToDelete)

        ' mark the UploadRow for delete
        theUploadRowHolder.UploadRow.IsMarkedForDelete = True
        ' mark the UploadRow's values for delete
        For Each theUploadValue As UploadValue In theUploadRowHolder.UploadRow.UploadValueCollection
            theUploadValue.IsMarkedForDelete = True
        Next

        ' delete all the DataRows for the UploadRow
        For Each theGridAndDataRowHolder As GridAndDataRowHolder In _
                 theUploadRowHolder.GridAndDataRowList

            theGridAndDataRowHolder.DataRow.Delete()
        Next

        ' now remove the UploadRowHolder
        Me.EIMManager.CurrentUploadRowHolderCollecton.RemoveUploadRowHolderForUploadRowId(inRowIdToDelete)

    End Sub

    ''' <summary>
    ''' Remove all data from the all grids.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ClearAllGrids()

        ClearGrid(Me.UltraGridItemMaintenance)
        ClearGrid(Me.UltraGridPriceUpload)
        ClearGrid(Me.UltraGridCostUpload)

        Me.StoreSelectorPriceUpload.ClearSelection()
        Me.StoreSelectorCostUpload.ClearSelection()

    End Sub

    ''' <summary>
    ''' Remove all data from the provided grid.
    ''' </summary>
    ''' <param name="grid"></param>
    ''' <remarks></remarks>
    Private Sub ClearGrid(ByRef grid As UltraGrid)

        If Not IsNothing(grid.DataSource) Then
            CType(CType(grid.DataSource, BindingSource).DataSource, DataSet).Clear()
        End If

    End Sub

    ''' <summary>
    ''' Let the user choose to save or not or to cancel the pending
    ''' action.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function AskToSaveOrCancel() As Boolean

        Dim cancel As Boolean = False

        If Me.HasChanged Then
            Dim theDialogResult As DialogResult = MessageBox.Show("Save your changes first?", _
                    "EIM - Open Session", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question)

            If theDialogResult = Windows.Forms.DialogResult.Yes Then
                SessionSave(False, False)
            End If

            cancel = theDialogResult = Windows.Forms.DialogResult.Cancel

        End If

        Return cancel

    End Function

    Private Function GetValueListValueFromKey(ByVal inValueListKey As Object, ByVal inValueListItemKey As Object) As String

        Dim theValue As String = Nothing
        Dim theValueListData As BusinessObjectCollection = _
                CType(Me.EIMManager.ValueListDataByKeyCollection.Item(inValueListKey), BusinessObjectCollection)

        If Not IsNothing(theValueListData) Then
            Dim theKeyedListItem As KeyedListItem = CType(theValueListData.ItemByKey(inValueListItemKey), KeyedListItem)

            If Not IsNothing(theKeyedListItem) Then
                theValue = CStr(theKeyedListItem.Value)
            End If
        End If

        Return theValue
    End Function

    Private Sub HandlePopUpDataEntryForms(ByRef inGrid As UltraGrid)

        If Not IsNothing(inGrid.ActiveCell) AndAlso inGrid.ActiveCell.Appearance.BackColor <> EIM_Constants.GRID_CELL_BACKGROUND_COLOR_DISABLED Then
            Dim theCurrentColumnGroup As String = inGrid.ActiveCell.Column.Group.Key
            Dim theCurrentCellKey As String = inGrid.ActiveCell.Column.Key
            Dim theUploadValue As UploadValue = GetUploadValueByKeyForGridRow(inGrid.ActiveRow, theCurrentCellKey)

            ' get the current row item identifier and Item Key values
            Dim theItemIdentifier As String = CStr(GetCellValue(inGrid.ActiveRow, EIM_Constants.ITEMIDENTIFIER_IDENTIFIER_ATTR_KEY))
            Dim theItemKey As Integer = GetUploadRowForGridRow(inGrid.ActiveRow).UploadRow.ItemKey

            ' let's make sure they aren't double clicking on the validation column
            If Not IsNothing(theUploadValue) Then

                If theCurrentColumnGroup.Equals(EIM_Constants.GROUP_HIERARCHY_DATA_KEY) Then

                    ' clear the current row selection and select
                    ' the active row
                    Me.UltraGridItemMaintenance.Selected.Rows.Clear()
                    Me.UltraGridItemMaintenance.ActiveCell.Row.Selected = True

                    ' get the current row item hierarchy values
                    Dim theItemSubteamNo As Integer = GetIntegerCellValue(inGrid.ActiveRow, EIM_Constants.ITEM_SUBTEAM_NO_ATTR_KEY)
                    Dim theItemCategoryId As Integer = GetIntegerCellValue(inGrid.ActiveRow, EIM_Constants.ITEM_CATEGORY_ID_ATTR_KEY)
                    Dim theItemLevel3Id As Integer = GetIntegerCellValue(inGrid.ActiveRow, EIM_Constants.ITEM_LEVEL_3_ATTR_KEY)
                    Dim theItemLevel4Id As Integer = GetIntegerCellValue(inGrid.ActiveRow, EIM_Constants.ITEM_LEVEL_4_ATTR_KEY)
                    Dim retailSale As Boolean = GetBooleanCellValue(inGrid.ActiveRow, EIM_Constants.ITEM_RETAIL_SALE)

                    Dim theSetHierarchyPositionForm As New SetHierarchyPositionForm

                    ' initialize the form and set the hierarchy values
                    theSetHierarchyPositionForm.Initialize()
                    theSetHierarchyPositionForm.ItemIdentifier = theItemIdentifier
                    theSetHierarchyPositionForm.SubTeamNo = theItemSubteamNo
                    theSetHierarchyPositionForm.CategoryId = theItemCategoryId
                    theSetHierarchyPositionForm.Level3 = theItemLevel3Id
                    theSetHierarchyPositionForm.Level4 = theItemLevel4Id
                    theSetHierarchyPositionForm.IsRetailSale = retailSale

                    If theSetHierarchyPositionForm.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then

                        ' get the hierarchy values back from the form
                        theItemSubteamNo = theSetHierarchyPositionForm.SubTeamNo
                        theItemCategoryId = theSetHierarchyPositionForm.CategoryId
                        theItemLevel3Id = theSetHierarchyPositionForm.Level3
                        theItemLevel4Id = theSetHierarchyPositionForm.Level4

                        ' set the cell values for the active row
                        SetCellValue(inGrid.ActiveRow, EIM_Constants.ITEM_SUBTEAM_NO_ATTR_KEY, IIf(theItemSubteamNo <= 0, "", CStr(theItemSubteamNo)).ToString())
                        SetCellValue(inGrid.ActiveRow, EIM_Constants.ITEM_CATEGORY_ID_ATTR_KEY, IIf(theItemCategoryId <= 0, "", CStr(theItemCategoryId)).ToString())
                        SetCellValue(inGrid.ActiveRow, EIM_Constants.ITEM_LEVEL_3_ATTR_KEY, IIf(theItemLevel3Id <= 0, "", CStr(theItemLevel3Id)).ToString())
                        SetCellValue(inGrid.ActiveRow, EIM_Constants.ITEM_LEVEL_4_ATTR_KEY, IIf(theItemLevel4Id <= 0, "", CStr(theItemLevel4Id)).ToString())

                        HandleCellUpdate(inGrid.ActiveCell, True)

                        ' validate the changes
                        Me.EIMManager.ValidateGridRowsForUploadRow( _
                            Me.EIMManager.CurrentUploadRowHolderCollecton.GetUploadRowHolderForUploadRowId(theUploadValue.UploadRowID), ValidationTypes.GridCell)

                    End If
                ElseIf theCurrentCellKey.Equals(EIM_Constants.ITEM_CHAINS_ATTR_KEY) Then
                    ' provide a custom entry form for the item's chain names

                    ' clear the current row selection and select
                    ' the active row
                    inGrid.Selected.Rows.Clear()
                    inGrid.ActiveCell.Row.Selected = True

                    ' get the chain name list value
                    Dim theChainNameListText As String = CStr(GetCellValue(inGrid.ActiveRow, theCurrentCellKey))

                    Dim theChainingForm As New ChainingForm

                    ' set the form title
                    theChainingForm.Text = "EIM - " + theUploadValue.Name

                    ' set the identifier and long text value
                    theChainingForm.LabelItemIdentifier.Text = theItemIdentifier
                    theChainingForm.ChainNameList = theChainNameListText

                    If theChainingForm.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then

                        ' get the long text value
                        theChainNameListText = theChainingForm.ChainNameList

                        ' set the long text value for the active row
                        SetCellValue(inGrid.ActiveRow, theCurrentCellKey, theChainNameListText)

                        HandleCellUpdate(inGrid.ActiveCell, False)

                        ' validate the changes
                        Me.EIMManager.ValidateGridRowsForUploadRow( _
                            Me.EIMManager.CurrentUploadRowHolderCollecton.GetUploadRowHolderForUploadRowId(theUploadValue.UploadRowID), ValidationTypes.GridCell)
                    End If
                ElseIf theCurrentCellKey.Equals(EIM_Constants.COST_DISCOUNT_ATTR_KEY) Then

                    If Me.EIMManager.CurrentUploadSession.IsNewItemSessionFlag Then

                        MessageBox.Show("The Cost Promotion Detail screen cannot be displayed for new items before they are uploaded.", _
                                "EIM - Cost Promotion History Drilldown", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Else
                        ' display the Cost Promotions Detail screen if this is a new item session

                        ' clear the current row selection and select
                        ' the active row
                        inGrid.Selected.Rows.Clear()
                        inGrid.ActiveCell.Row.Selected = True

                        ' display the form for the item's vendor deal history

                        Dim theCostPromotionForm As New CostPromotion
                        Dim itemDescription As String = CStr(GetCellValue(inGrid.ActiveRow, EIM_Constants.ITEM_ITEM_DESCRIPTION_ATTR_KEY))

                        Dim storeNo As Integer = CInt(GetCellValue(inGrid.ActiveRow, EIM_Constants.STORE_NO_ATTR_KEY))
                        Dim storeName As String = GetValueListValueFromKey(EIM_Constants.STORE_NO_ATTR_KEY, storeNo)

                        Dim vendorId As Integer = CInt(GetCellValue(inGrid.ActiveRow, EIM_Constants.COST_VENDOR_ATTR_KEY))
                        Dim vendorName As String = GetValueListValueFromKey(EIM_Constants.COST_VENDOR_ATTR_KEY, vendorId)

                        If IsNothing(storeName) Or IsNothing(vendorName) Then

                            MessageBox.Show("The Cost Promotion Detail screen cannot be displayed because sufficient information is not available in the cost grid.", _
                                    "EIM - Cost Promotion History Drilldown", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Else
                            'pass item info to child screen
                            Dim poItemInfo As New ItemBO
                            poItemInfo.Item_Key = theItemKey
                            poItemInfo.ItemDescription = itemDescription

                            Dim storeInfo As New StoreBO
                            storeInfo.StoreNo = storeNo
                            storeInfo.StoreName = storeName

                            Dim poVendorInfo As New VendorBO
                            poVendorInfo.VendorID = vendorId
                            poVendorInfo.VendorName = vendorName

                            theCostPromotionForm.ItemBO = poItemInfo
                            theCostPromotionForm.VendorBO = poVendorInfo
                            theCostPromotionForm.StoreBO = storeInfo

                            theCostPromotionForm.ShowDialog()
                        End If
                    End If
                ElseIf theUploadValue.Size > EIM_Constants.LONG_TEXT_SIZE Then
                    ' provide a custom entry form for text attributes above a certain length

                    ' clear the current row selection and select
                    ' the active row
                    inGrid.Selected.Rows.Clear()
                    inGrid.ActiveCell.Row.Selected = True

                    ' get the text value
                    Dim theLongText As String = CStr(GetCellValue(inGrid.ActiveRow, theCurrentCellKey))

                    Dim theLongTextForm As New LongTextForm

                    ' set the form title
                    theLongTextForm.Text = "EIM - " + theUploadValue.Name

                    ' set the identifier and long text value
                    theLongTextForm.LabelItemIdentifier.Text = theItemIdentifier
                    theLongTextForm.TextBoxLongText.Text = theLongText

                    If theLongTextForm.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then

                        ' get the long text value
                        theLongText = theLongTextForm.TextBoxLongText.Text

                        ' set the long text value for the active row
                        SetCellValue(inGrid.ActiveRow, theCurrentCellKey, theLongText)

                        HandleCellUpdate(inGrid.ActiveCell, False)

                        ' validate the changes
                        Me.EIMManager.ValidateGridRowsForUploadRow( _
                            Me.EIMManager.CurrentUploadRowHolderCollecton.GetUploadRowHolderForUploadRowId(theUploadValue.UploadRowID), ValidationTypes.GridCell)
                    End If
                End If
            End If
        End If
    End Sub

#End Region

#Region "Error Dialog Methods"

    Private Delegate Sub ShowErrorDialogDelegate(ByVal inSubject As String, _
            ByVal inShortMessage As String, ByVal inFullMessage As String, ByVal inNotificationType As ErrorDialog.NotificationTypes, _
            ByVal inErrorCategoryCode As String)

    Private Delegate Sub ShowUploadErrorDialogDelegate(ByVal ex As Exception, ByVal inSubject As String, ByVal inShortMessage As String)

    Private Sub UIThreadSafeShowErrorDialog(ByVal ex As Exception, ByVal inSubject As String, ByVal inShortMessage As String)
        UIThreadSafeShowErrorDialog(inSubject, inShortMessage, ex.ToString())
    End Sub

    Private Sub UIThreadSafeShowErrorDialog(ByVal inSubject As String, ByVal inShortMessage As String, ByVal inFullMessage As String)

        Me.AnErrorHasOccurred = True
        Me.Invoke(CType(AddressOf ErrorDialog.HandleError, ShowErrorDialogDelegate), _
            New Object() {inSubject, inShortMessage, inFullMessage, ErrorDialog.NotificationTypes.DialogAndEmail, "EIM_"})

    End Sub

    Private Sub UIThreadSafeShowUploadErrorDialog(ByVal ex As Exception, ByVal inSubject As String, ByVal inShortMessage As String)

        Me.AnErrorHasOccurred = True
        Me.Invoke(CType(AddressOf Me.ShowUploadErrorDialog, ShowUploadErrorDialogDelegate), _
            New Object() {ex, inSubject, inShortMessage})

    End Sub

    Private Sub ShowErrorDialog(ByVal ex As Exception, ByVal inSubject As String, ByVal inShortMessage As String)
        UIThreadSafeShowErrorDialog(inSubject, inShortMessage, ex.ToString())
    End Sub

    Private Sub ShowErrorDialog(ByVal inSubject As String, ByVal inShortMessage As String, ByVal inFullMessage As String)

        Me.AnErrorHasOccurred = True
        ErrorDialog.HandleError(inSubject, inShortMessage, inFullMessage, ErrorDialog.NotificationTypes.DialogAndEmail, "EIM_")

    End Sub

    Private Sub ShowUploadErrorDialog(ByVal ex As Exception, ByVal inSubject As String, ByVal inShortMessage As String)

        Dim showMissingAttributeDialog As Boolean = False
        Dim theFullExceptionMessage As String = ex.ToString()
        Dim theMissingAttributes As New ArrayList
        Dim theMissingUploadAttribute As UploadAttribute
        Dim theRegPriceIsZeroRegex As New Regex("regular price must be greater than zero")
        Dim thePriceChangeCollisionRegex As New Regex("Pending Price Change Collision:")
        Dim theMissingAttributesRegex As New Regex("Cannot insert the value NULL into column '(\w+)'")
        Dim theNullColumnName As String

        Me.EIMManager.CurrentUploadSession.ProgressComplete = True

        Dim theRegPriceIsZeroMatch As Match = theRegPriceIsZeroRegex.Match(theFullExceptionMessage)
        Dim thePriceChangeCollisionMatch As Match = thePriceChangeCollisionRegex.Match(theFullExceptionMessage)

        Dim theItemStoreText As String = ""
        Dim theItemStoreRegex As New Regex("for item/store ([\w,/]+):")
        Dim theItemStoreMatch As Match = theItemStoreRegex.Match(theFullExceptionMessage)

        If theRegPriceIsZeroMatch.Success Then

            If theItemStoreMatch.Success Then
                theItemStoreText = theItemStoreMatch.Groups(1).Value
            End If

            MessageBox.Show("A promo price change has failed for the identifier/store no (" + theItemStoreText + _
                ") because the regular price for the item is zero. " + Environment.NewLine + Environment.NewLine + _
                "This happens when the regular price column is not part of the upload, such as when" + Environment.NewLine + _
                "the Promo Price template is used. In such a case, EIM will use the current regular price for the item and" + Environment.NewLine + _
                " you will see this error when that value is zero. " + Environment.NewLine + Environment.NewLine + _
                "You can do the following to resolve this issue: " + Environment.NewLine + _
                "----------------------------------------------- " + Environment.NewLine + _
                "1. Turn off the price change for the item." + Environment.NewLine + _
                "2. Or include the Reg Price column in your upload." + Environment.NewLine + _
                "   Note: There may be more than one item/store with a zero price." + Environment.NewLine + _
                "3. Upload again.", "Upload Price Change Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Else

            If thePriceChangeCollisionMatch.Success Then

                theItemStoreRegex = New Regex("for item/store [\w,/\s-]+")
                theItemStoreMatch = theItemStoreRegex.Match(theFullExceptionMessage)

                If theItemStoreMatch.Success Then
                    theItemStoreText = theItemStoreMatch.Groups(0).Value
                End If

                MessageBox.Show("A collision has occured between an uploaded and an existing pending price change " + theItemStoreText + "." + Environment.NewLine + Environment.NewLine + _
                    "This occurs when pending price changes occur after the session has been validated but " + _
                    "before the new price change is uploaded.", _
                    "Upload Price Change Error", MessageBoxButtons.OK, MessageBoxIcon.Error)

            Else
                Dim theMissingAttributesMatches As MatchCollection = theMissingAttributesRegex.Matches(theFullExceptionMessage)

                For Each theMissingAttributesMatch As Match In theMissingAttributesMatches
                    If theMissingAttributesMatch.Success Then

                        showMissingAttributeDialog = True

                        theNullColumnName = theMissingAttributesMatch.Groups(1).Value

                        theMissingUploadAttribute = CType(UploadAttributeDAO.Instance.GetAllUploadAttributes(). _
                                FindOneByPropertyValue("ColumnNameOrKey", theNullColumnName.ToLower()), UploadAttribute)

                        If Not IsNothing(theMissingUploadAttribute) Then
                            If Not theMissingAttributes.Contains(theMissingUploadAttribute.Name) Then
                                theMissingAttributes.Add(theMissingUploadAttribute.Name)
                            End If
                        Else
                            Dim theColumnNameMessage As String = theNullColumnName + " - this database column is not configured as an EIM attribute"

                            If Not theMissingAttributes.Contains(theColumnNameMessage) Then
                                theMissingAttributes.Add(theColumnNameMessage)
                            End If
                        End If
                    End If
                Next

                If showMissingAttributeDialog Then
                    ShowUploadMissingAttributesErrorForm(theMissingAttributes)
                Else
                    ShowErrorDialog(inSubject, inShortMessage, ex.ToString())
                End If
            End If
        End If

        Me.AnErrorHasOccurred = True

    End Sub

    Private Sub ShowUploadMissingAttributesErrorForm(ByRef inMissingAttributes As ArrayList)

        Dim theUploadMissingAttributesErrorForm As New UploadMissingAttributesErrorForm

        theUploadMissingAttributesErrorForm.MissingAttributes = inMissingAttributes
        theUploadMissingAttributesErrorForm.ShowDialog()

    End Sub

#End Region

#End Region

End Class
