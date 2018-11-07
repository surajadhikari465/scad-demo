Imports Infragistics.Win
Imports Infragistics.Win.UltraWinGrid
Imports WholeFoods.IRMA.CompetitorStore.BusinessLogic
Imports WholeFoods.IRMA.CompetitorStore.DataAccess

Namespace WholeFoods.IRMA.CompetitorStore.UserInterface
    Public Class frmNationalPurchasingValue

#Region "Member Variables"

        Private _dataSet As NPVDataSet
        Private _priceDAO As New Price

#End Region

#Region "Helper Methods"

        Private Function GetNullableIDFromComboBox(ByVal comboBox As ComboBox) As Nullable(Of Integer)
            Return CType(IIf(comboBox.SelectedIndex > 0, CInt(comboBox.SelectedValue), Nothing), Nullable(Of Integer))
        End Function

        Private Sub LoadWFMStores()
            Dim table As DataTable = ItemHosting.DataAccess.StoreDAO.CreateStoreDataTable()
            Dim row As DataRow = table.NewRow()

            row("Store_No") = -1
            row("Store_Name") = My.Resources.CompetitorStore.AllItems

            table.Rows.Add(row)

            ItemHosting.DataAccess.StoreDAO.GetStoreList(table)

            With cmbStore
                .DisplayMember = "Store_Name"
                .ValueMember = "Store_No"
                .DataSource = table
            End With
        End Sub

        Private Sub LoadCompetitivePriceTypes()
            _dataSet.CompetitivePriceType.AddCompetitivePriceTypeRow(-1, My.Resources.CompetitorStore.AllItems)

            DataAccess.CompetitivePriceType.List(_dataSet.CompetitivePriceType)

            With cmbCompetitivePriceType
                .DisplayMember = _dataSet.CompetitivePriceType.DescriptionColumn.ColumnName
                .ValueMember = _dataSet.CompetitivePriceType.CompetitivePriceTypeIDColumn.ColumnName
                .DataSource = _dataSet.CompetitivePriceType
            End With
        End Sub

        Private Sub ConfigureCompetitivePriceTypeValueList()
            Dim competitivePriceTypes As ValueList = ugNPV.DisplayLayout.ValueLists.Add("CompetitivePriceTypes")
            Dim cptColumn As UltraGridColumn = ugNPV.DisplayLayout.Bands(0).Columns("CompetitivePriceTypeID")

            For Each cptRow As NPVDataSet.CompetitivePriceTypeRow In _dataSet.CompetitivePriceType
                If cptRow.CompetitivePriceTypeID > 0 Then
                    competitivePriceTypes.ValueListItems.Add(cptRow.CompetitivePriceTypeID, cptRow.Description)
                Else
                    ' The "All Items" row was added to this table, but we want to display a "null value" item
                    competitivePriceTypes.ValueListItems.Add(DBNull.Value, My.Resources.CompetitorStore.NoItems)
                End If
            Next

            cptColumn.ValueList = competitivePriceTypes
            cptColumn.Style = ColumnStyle.DropDownValidate
        End Sub

        Private Function Save() As Boolean
            Dim success As Boolean = False

            Windows.Forms.Cursor.Current = Cursors.WaitCursor

            ugNPV.UpdateData()

            If _dataSet.HasErrors Then
                MessageBox.Show(My.Resources.CompetitorStore.FixErrors, My.Resources.CompetitorStore.FixErrorsTitle)
                success = False
            Else
                _priceDAO.Save(_dataSet)
                success = True
            End If

            Windows.Forms.Cursor.Current = Cursors.Default

            Return success
        End Function

        Private Sub Search()
            Windows.Forms.Cursor.Current = Cursors.WaitCursor

            With _priceDAO.SearchHelper
                .CompetitivePriceTypeID = GetNullableIDFromComboBox(cmbCompetitivePriceType)
                .Store_No = GetNullableIDFromComboBox(cmbStore)

                isSearch.AddNonDefaultSearchCriteria(.ItemSearchHelper)
                isoItemOptions.AddNonDefaultSearchCriteria(.ItemSearchHelper)
            End With

            _priceDAO.Search(_dataSet)

            For Each infoRow As NPVDataSet.CompetitivePriceInfoRow In _dataSet.CompetitivePriceInfo
                infoRow("PriceAndMultiple") = String.Format("{0} @ {1:#0.00}", infoRow.Multiple, infoRow.Price)
            Next

            _dataSet.AcceptChanges()

            Windows.Forms.Cursor.Current = Cursors.Default
        End Sub

        Private Shared Function GetPriceRow(ByVal row As UltraGridRow) As NPVDataSet.CompetitivePriceInfoRow
            Return CType(CType(row.ListObject, DataRowView).Row, NPVDataSet.CompetitivePriceInfoRow)
        End Function

        Private Shared Sub EnableBandwidthCells(ByVal row As UltraGridRow)
            Dim priceRow As NPVDataSet.CompetitivePriceInfoRow = GetPriceRow(row)
            Dim cells As CellsCollection = row.Cells
            Dim currentCell As UltraGridCell
            Dim selectedValue As Object

            currentCell = cells("CompetitivePriceTypeID")
            selectedValue = currentCell.ValueListResolved.GetValue(currentCell.ValueListResolved.SelectedItemIndex)

            If (selectedValue IsNot DBNull.Value AndAlso CInt(selectedValue) = enumCompetitivePriceType.BandwidthPricePercentage) _
                OrElse (Not priceRow.IsCompetitivePriceTypeIDNull AndAlso priceRow.CompetitivePriceTypeID = enumCompetitivePriceType.BandwidthPricePercentage) Then
                cells("BandwidthPercentageHigh").Activation = Activation.AllowEdit
                cells("BandwidthPercentageLow").Activation = Activation.AllowEdit

                priceRow.SetColumnError("BandwidthPercentageHigh", CStr(IIf(priceRow.IsBandwidthPercentageHighNull(), "Bandwidth Percentage High is required", String.Empty)))
                priceRow.SetColumnError("BandwidthPercentageLow", CStr(IIf(priceRow.IsBandwidthPercentageLowNull(), "Bandwidth Percentage Low is required", String.Empty)))
            Else
                currentCell = cells("BandwidthPercentageHigh")

                currentCell.Activation = Activation.NoEdit
                currentCell.Value = DBNull.Value

                currentCell = cells("BandwidthPercentageLow")
                currentCell.Activation = Activation.NoEdit
                currentCell.Value = DBNull.Value

                priceRow.SetColumnError("BandwidthPercentageHigh", String.Empty)
                priceRow.SetColumnError("BandwidthPercentageLow", String.Empty)
            End If
        End Sub

#End Region

#Region "Form Event Handlers"

        Private Sub frmNationalPurchasingValue_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
            _dataSet = New NPVDataSet()

            LoadWFMStores()
            LoadCompetitivePriceTypes()

            _dataSet.CompetitivePriceInfo.Columns.Add("PriceAndMultiple", GetType(String))
            _dataSet.AcceptChanges()

            ugNPV.DataSource = _dataSet.CompetitivePriceInfo

            ConfigureCompetitivePriceTypeValueList()
        End Sub

        Private Sub frmNationalPurchasingValue_FormClosing(ByVal sender As Object, ByVal e As FormClosingEventArgs) Handles Me.FormClosing
            If _dataSet.HasChanges() AndAlso MessageBox.Show(My.Resources.CompetitorStore.UnsavedChangesBeforeExit, My.Resources.CompetitorStore.UnsavedChangesTitle, MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.No Then
                e.Cancel = True
            End If
        End Sub

#End Region

#Region "UltraGrid Event Handlers"

        Private Sub ugNPV_InitializeRow(ByVal sender As System.Object, ByVal e As Infragistics.Win.UltraWinGrid.InitializeRowEventArgs) Handles ugNPV.InitializeRow
            EnableBandwidthCells(e.Row)
        End Sub

        Private Sub ugNPV_CellListSelect(ByVal sender As Object, ByVal e As UltraWinGrid.CellEventArgs) Handles ugNPV.CellListSelect
            EnableBandwidthCells(e.Cell.Row)
        End Sub

#End Region

#Region "Button Event Handlers"

        Private Sub btnClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClear.Click
            Me.isSearch.Clear()
            Me.isoItemOptions.Clear()

            cmbCompetitivePriceType.SelectedIndex = 0
            cmbStore.SelectedIndex = 0
        End Sub

        Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
            ugNPV.UpdateData()

            If _dataSet.HasChanges() Then
                Select Case MessageBox.Show(My.Resources.CompetitorStore.UnsavedChangesBeforeSearch, My.Resources.CompetitorStore.UnsavedChangesTitle, MessageBoxButtons.YesNoCancel)
                    Case Windows.Forms.DialogResult.Yes
                        If Not Save() Then
                            Return
                        End If
                    Case Windows.Forms.DialogResult.No
                        _dataSet.RejectChanges()
                    Case Windows.Forms.DialogResult.Cancel
                        Return
                End Select
            End If

            Search()
        End Sub

        Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
            Me.Close()
        End Sub

        Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
            Save()
        End Sub

#End Region

    End Class
End Namespace