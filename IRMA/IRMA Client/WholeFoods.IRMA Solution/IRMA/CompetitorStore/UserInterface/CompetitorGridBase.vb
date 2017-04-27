Imports System.ComponentModel
Imports WholeFoods.IRMA.CompetitorStore.DataAccess
Imports WholeFoods.IRMA.CompetitorStore.BusinessLogic
Imports Infragistics.Win
Imports Infragistics.Win.UltraWinGrid

Namespace WholeFoods.IRMA.CompetitorStore.UserInterface
    Public Class CompetitorGridBase
        Inherits System.Windows.Forms.UserControl

#Region "Member Variables"

        Protected _disablePrimaryKeyColumnEdit As Boolean = False

        ' UltraGrid column references
        Protected _competitorColumn As UltraGridColumn
        Protected _competitorIDColumn As UltraGridColumn
        Protected _competitorStoreIDColumn As UltraGridColumn
        Protected _competitorStoreNameColumn As UltraGridColumn
        Protected _fiscalWeekColumn As UltraGridColumn
        Protected _itemKeyColumn As UltraGridColumn
        Protected _locationColumn As UltraGridColumn
        Protected _locationIDColumn As UltraGridColumn
        Protected _sizeColumn As UltraGridColumn
        Protected _itemUnitColumn As UltraGridColumn
        Protected _upcCodeColumn As UltraGridColumn
        Protected _wfmIdentifierColumn As UltraGridColumn

        Private _columnsSet As Boolean = False
        Private _unmatchedValueColor As Color = Color.LightGray
        Private _preEditCellValue As Object
        Private _dataSet As CompetitorStoreDataSet
        Private _captureNewIdentifiers As Boolean = True
        Private _currentFiscalWeek As CompetitorStoreDataSet.FiscalWeekRow

        Private WithEvents ugPreview As UltraGrid

#End Region

#Region "Properties"

        Public WriteOnly Property Filter() As String
            Set(ByVal value As String)
                CType(ugPreview.DataSource, DataView).RowFilter = value

                SetFiscalWeekSelections()
            End Set
        End Property

        ''' <summary>
        ''' Gets the count of rows selected in the grid
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property SelectedRowCount() As Integer
            Get
                If ugPreview.Selected.Rows.Count = 0 AndAlso ugPreview.ActiveCell IsNot Nothing Then
                    ugPreview.ActiveCell.Row.Selected = True
                End If

                Return ugPreview.Selected.Rows.Count
            End Get
        End Property

        Public ReadOnly Property SelectedRowsAvailableForMatching() As Boolean
            Get
                Dim available As Boolean = True

                If Me.SelectedRowCount > 0 Then
                    If _disablePrimaryKeyColumnEdit Then
                        For Each gridRow As UltraGridRow In ugPreview.Selected.Rows
                            If GetPriceRowFromGridRow(gridRow).IsExistingRow Then
                                available = False
                                Exit For
                            End If
                        Next

                        If Not available Then
                            MessageBox.Show(My.Resources.CompetitorStore.CannotRematchItem, My.Resources.CompetitorStore.UnavailableRows)
                        End If
                    End If
                Else
                    available = False
                    MessageBox.Show(My.Resources.CompetitorStore.SelectRows, My.Resources.CompetitorStore.SelectRowsTitle)
                End If

                Return available
            End Get
        End Property

#End Region

#Region "Helper Methods"

#Region "RevertCellColor overloads"

        Private Sub RevertCellColor(ByVal cell As UltraGridCell)
            cell.Appearance.BackColor = cell.Row.CellAppearance.BackColor
        End Sub

        Private Sub RevertCellColor(ByVal gridRow As UltraGridRow, ByVal columnName As String)
            RevertCellColor(gridRow.Cells(columnName))
        End Sub

        Private Sub RevertCellColor(ByVal gridRow As UltraGridRow, ByVal column As UltraGridColumn)
            RevertCellColor(gridRow.Cells(column))
        End Sub

#End Region

        Private Function GetPriceRowFromGridRow(ByVal gridRow As UltraGridRow) As ICompetitorPriceRow
            Return CType(CType(gridRow.ListObject, DataRowView).Row, ICompetitorPriceRow)
        End Function

        Private Sub SetColumnError(ByVal competitorPriceRow As ICompetitorPriceRow, ByVal columnKey As String, ByVal isError As Boolean, ByVal errorMessage As String)
            competitorPriceRow.SetColumnError(columnKey, CStr(IIf(isError, errorMessage, String.Empty)))
        End Sub

        ''' <summary>
        ''' An attempt to avoid magic numbers and strings throughout the code related to accessing columns
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub GetGridColumnReferences()
            If Not _columnsSet Then
                Dim gridColumns As UltraWinGrid.ColumnsCollection = ugPreview.DisplayLayout.Bands(0).Columns

                _competitorColumn = gridColumns("Competitor")
                _locationColumn = gridColumns("Location")
                _competitorStoreNameColumn = gridColumns("CompetitorStore")
                _wfmIdentifierColumn = gridColumns("WFMIdentifier")
                _upcCodeColumn = gridColumns("UPCCode")
                _sizeColumn = gridColumns("Size")
                _itemUnitColumn = gridColumns("Unit_ID")
                _fiscalWeekColumn = gridColumns("FiscalWeek")

                ' Hidden columns
                _itemKeyColumn = gridColumns("Item_Key")
                _competitorStoreIDColumn = gridColumns("CompetitorStoreID")
                _competitorIDColumn = gridColumns("CompetitorID")
                _locationIDColumn = gridColumns("CompetitorLocationID")

                _columnsSet = True
            End If
        End Sub

        Private Sub SetFiscalWeekSelections()
            Dim competitorPriceRow As ICompetitorPriceRow

            ' Set the correct selection for each row
            For Each gridRow As UltraGridRow In ugPreview.Rows
                competitorPriceRow = GetPriceRowFromGridRow(gridRow)

                If (Not (competitorPriceRow.IsFiscalYearNull OrElse competitorPriceRow.IsFiscalPeriodNull OrElse competitorPriceRow.IsPeriodWeekNull)) Then
                    gridRow.Cells(_fiscalWeekColumn).Value = competitorPriceRow.FiscalWeekRowParent
                End If
            Next

            ' Update the data source so the edited flag doesn't show up from the manipulation above
            ugPreview.UpdateData()
        End Sub

        Private Sub ConfigureFiscalWeekValueList()
            Dim fiscalWeeks As ValueList = ugPreview.DisplayLayout.ValueLists.Add("FiscalWeeks")

            ' First create the value list
            For Each week As CompetitorStoreDataSet.FiscalWeekRow In _dataSet.FiscalWeek
                fiscalWeeks.ValueListItems.Add(week, week.FiscalWeekDescription)
            Next

            fiscalWeeks.DisplayStyle = ValueListDisplayStyle.DisplayText

            ' Associate the value list with the fiscal week column
            _fiscalWeekColumn.ValueList = fiscalWeeks
            _fiscalWeekColumn.Style = ColumnStyle.DropDownList

            SetFiscalWeekSelections()
        End Sub

        Private Sub ConfigureItemUnitValueList()
            Dim itemUnits As ValueList = ugPreview.DisplayLayout.ValueLists.Add("ItemUnits")

            For Each unit As CompetitorStoreDataSet.ItemUnitRow In _dataSet.ItemUnit
                itemUnits.ValueListItems.Add(unit.Unit_ID, unit.Unit_Name)
            Next

            itemUnits.DisplayStyle = ValueListDisplayStyle.DisplayText

            _itemUnitColumn.ValueList = itemUnits
            _itemUnitColumn.Style = ColumnStyle.DropDownValidate
        End Sub

        Private Sub ConfigureValueLists()
            ConfigureFiscalWeekValueList()

            ConfigureItemUnitValueList()

            ' This handler isn't added until now because the changes thus far haven't changed the data source
            AddHandler ugPreview.BeforeRowUpdate, New CancelableRowEventHandler(AddressOf ugPreview_BeforeRowUpdate)
        End Sub

#End Region

#Region "Public Methods"

#Region "Selected Row(s) Manipulation"

        Public Sub UpdateSelectedRowsWithItem(ByVal item_key As Integer, ByVal identifier As String)
            For Each gridRow As UltraGridRow In ugPreview.Selected.Rows
                gridRow.Cells(_itemKeyColumn).Value = item_key
                gridRow.Cells(_wfmIdentifierColumn).Value = identifier

                ' Update color to indicate that this value has been matched
                RevertCellColor(gridRow, _wfmIdentifierColumn)
            Next
        End Sub

        Public Sub UpdateSelectedRowsWithStore(ByVal competitorStore As CompetitorStoreDataSet.CompetitorStoreRow)
            Dim cell As UltraGridCell

            For Each gridRow As UltraGridRow In ugPreview.Selected.Rows
                gridRow.Cells(_competitorIDColumn).Value = competitorStore.CompetitorID
                gridRow.Cells(_locationIDColumn).Value = competitorStore.CompetitorLocationID
                gridRow.Cells(_competitorStoreIDColumn).Value = competitorStore.CompetitorStoreID

                cell = gridRow.Cells(_competitorColumn)
                cell.Value = competitorStore.CompetitorRow.Name
                RevertCellColor(cell)

                cell = gridRow.Cells(_locationColumn)
                cell.Value = competitorStore.CompetitorLocationRow.Name
                RevertCellColor(cell)

                cell = gridRow.Cells(_competitorStoreNameColumn)
                cell.Value = competitorStore.Name
                RevertCellColor(cell)
            Next
        End Sub

#End Region

        Protected Sub Initialize(ByVal grid As UltraGrid, ByVal dataSet As CompetitorStoreDataSet, ByVal dataView As DataView)
            ugPreview = grid
            _dataSet = dataSet

            AddHandler ugPreview.InitializeRow, New InitializeRowEventHandler(AddressOf ugPreview_InitializeRow)

            ugPreview.DataSource = dataView

            GetGridColumnReferences()

            ConfigureValueLists()
        End Sub

        Protected Sub Initialize(ByVal grid As UltraGrid, ByVal dataSet As CompetitorStoreDataSet, ByVal dataView As DataView, ByVal captureNewIdentifiers As Boolean, ByVal currentWeek As CompetitorStoreDataSet.FiscalWeekRow)
            _captureNewIdentifiers = captureNewIdentifiers
            _currentFiscalWeek = currentWeek

            Me.Initialize(grid, dataSet, dataView)
        End Sub

        Public Sub DataSourceRefreshed()
            SetFiscalWeekSelections()
        End Sub

        Public Function ShouldSaveChanges() As Boolean
            Dim hasErrors As Boolean = False
            Dim table As DataTable = CType(ugPreview.DataSource, DataView).Table

            Windows.Forms.Cursor.Current = Cursors.WaitCursor

            ugPreview.PerformAction(UltraGridAction.ExitEditMode)
            ugPreview.UpdateData()

            Windows.Forms.Cursor.Current = Cursors.Default

            ' This is used instead of the built-in HasErrors method because that counts 
            ' deleted rows with errors, which we want to ignore
            For Each row As DataRow In table.Rows
                If row.RowState <> DataRowState.Deleted AndAlso row.HasErrors Then
                    hasErrors = True
                    Exit For
                End If
            Next

            If hasErrors Then
                MessageBox.Show(My.Resources.CompetitorStore.FixErrors, My.Resources.CompetitorStore.FixErrorsTitle)

                Return False
            Else
                Return True
            End If
        End Function

#End Region

#Region "UltraGrid Event Handlers"

#Region "Edit Mode Change"

        Private Sub ugPreview_BeforeEnterEditMode(ByVal sender As Object, ByVal e As CancelEventArgs) Handles ugPreview.BeforeEnterEditMode
            _preEditCellValue = ugPreview.ActiveCell.Value
        End Sub

        Private Sub ugPreview_AfterExitEditMode(ByVal sender As Object, ByVal e As EventArgs) Handles ugPreview.AfterExitEditMode
            Dim cell As UltraGridCell = ugPreview.ActiveCell
            Dim rowCells As CellsCollection = cell.Row.Cells
            Dim value As Object = cell.Value

            ' If the value has changed
            If (value Is Nothing Xor _preEditCellValue Is Nothing) _
                OrElse (value IsNot Nothing AndAlso Not (value.Equals(_preEditCellValue))) Then

                ' If the cell is in one of the columns that needs to be directly mapped to maintain an ID,
                ' reset the corresponding ID(s)
                Select Case cell.Column.Key
                    Case _competitorColumn.Key
                        rowCells(_competitorIDColumn).Value = DBNull.Value
                        rowCells(_competitorStoreIDColumn).Value = DBNull.Value
                    Case _locationColumn.Key
                        rowCells(_locationIDColumn).Value = DBNull.Value
                        rowCells(_competitorStoreIDColumn).Value = DBNull.Value
                    Case _competitorStoreNameColumn.Key
                        rowCells(_competitorStoreIDColumn).Value = DBNull.Value
                    Case Else
                        ' No action
                End Select
            End If
        End Sub

#End Region

        Private Sub ugPreview_AfterRowInsert(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles ugPreview.AfterRowInsert
            If _currentFiscalWeek IsNot Nothing Then
                e.Row.Cells(_fiscalWeekColumn).Value = _currentFiscalWeek
            End If
        End Sub

        Private Sub ugPreview_InitializeRow(ByVal sender As System.Object, ByVal e As Infragistics.Win.UltraWinGrid.InitializeRowEventArgs)
            Dim competitorPriceRow As ICompetitorPriceRow = GetPriceRowFromGridRow(e.Row)
            Dim cells As CellsCollection = e.Row.Cells

            GetGridColumnReferences()

            If _disablePrimaryKeyColumnEdit AndAlso competitorPriceRow.IsExistingRow Then
                cells(_competitorColumn).Activation = Activation.ActivateOnly
                cells(_locationColumn).Activation = Activation.ActivateOnly
                cells(_competitorStoreNameColumn).Activation = Activation.ActivateOnly
                cells(_fiscalWeekColumn).Activation = Activation.ActivateOnly
            End If

            ' Unmatched values
            If competitorPriceRow.IsCompetitorIDNull Then
                cells(_competitorColumn).Appearance.BackColor = _unmatchedValueColor
            Else
                RevertCellColor(cells(_competitorColumn))
            End If

            If competitorPriceRow.IsCompetitorLocationIDNull Then
                cells(_locationColumn).Appearance.BackColor = _unmatchedValueColor
            Else
                RevertCellColor(cells(_locationColumn))
            End If

            If competitorPriceRow.IsCompetitorStoreIDNull Then
                cells(_competitorStoreNameColumn).Appearance.BackColor = _unmatchedValueColor
            Else
                RevertCellColor(cells(_competitorStoreNameColumn))
            End If

            ' Missing required values
            SetColumnError(competitorPriceRow, _wfmIdentifierColumn.Key, competitorPriceRow.IsItem_KeyNull, My.Resources.CompetitorStore.MatchItem)
            SetColumnError(competitorPriceRow, "Price", competitorPriceRow.IsPriceNull(), My.Resources.CompetitorStore.RequirePrice)
            SetColumnError(competitorPriceRow, "PriceMultiple", competitorPriceRow.IsPriceMultipleNull(), My.Resources.CompetitorStore.RequirePriceMultiple)
            SetColumnError(competitorPriceRow, _sizeColumn.Key, competitorPriceRow.IsSizeNull, My.Resources.CompetitorStore.RequireSize)
            SetColumnError(competitorPriceRow, _competitorColumn.Key, competitorPriceRow.IsCompetitorNull(), My.Resources.CompetitorStore.RequireCompetitor)
            SetColumnError(competitorPriceRow, _locationColumn.Key, competitorPriceRow.IsCompetitorLocationNull(), My.Resources.CompetitorStore.RequireCompetitorLocation)
            SetColumnError(competitorPriceRow, _competitorStoreNameColumn.Key, competitorPriceRow.IsCompetitorStoreNull(), My.Resources.CompetitorStore.RequireCompetitorStore)

            If competitorPriceRow.IsFiscalYearNull() Then
                cells(_fiscalWeekColumn).Appearance.BackColor = ugPreview.DisplayLayout.Override.DataErrorCellAppearance.BackColor
                competitorPriceRow.SetColumnError("FiscalYear", My.Resources.CompetitorStore.RequireFiscalYear)
            Else
                RevertCellColor(cells(_fiscalWeekColumn))
                competitorPriceRow.SetColumnError("FiscalYear", String.Empty)
            End If
        End Sub

        Private Sub ugPreview_BeforeRowUpdate(ByVal sender As Object, ByVal e As CancelableRowEventArgs)
            Dim cells As CellsCollection = e.Row.Cells
            Dim fiscalWeekCell As UltraGridCell = cells(_fiscalWeekColumn)
            Dim competitorStoreNameCell As UltraGridCell = cells(_competitorStoreNameColumn)
            Dim competitorStoreIDCell As UltraGridCell = cells(_competitorStoreIDColumn)
            Dim itemKeyCell As UltraGridCell = cells(_itemKeyColumn)
            Dim wfmIdentifierCell As UltraGridCell = cells(_wfmIdentifierColumn)
            Dim competitorPriceRow As ICompetitorPriceRow = GetPriceRowFromGridRow(e.Row)

            If fiscalWeekCell.DataChanged Then
                ' New or different fiscal week selected
                competitorPriceRow.FiscalWeekRowParent = CType(fiscalWeekCell.Value, CompetitorStoreDataSet.FiscalWeekRow)
            End If

            ' If the the competitor store name cell has changed and the competitor store ID cell is not null
            ' the user created a new identifier
            If _captureNewIdentifiers AndAlso competitorStoreNameCell.DataChanged AndAlso competitorStoreIDCell.Value IsNot Nothing Then
                Try
                    _dataSet.CompetitorStoreIdentifier.AddCompetitorStoreIdentifierRow(CInt(competitorStoreIDCell.Value), CStr(competitorStoreNameCell.Value))
                Catch ex As Exception
                    ' Identifier already exists
                End Try
            End If
        End Sub

#End Region

    End Class
End Namespace