Imports System.Text
Imports WholeFoods.IRMA.CompetitorStore.DataAccess
Imports WholeFoods.IRMA.CompetitorStore.BusinessLogic

Namespace WholeFoods.IRMA.CompetitorStore.UserInterface
    Public Class CompetitorDataManagement

#Region "Member Variables"

        Private _itemSearchForm As ItemSearch = Nothing
        Private _storeSearchForm As CompetitorStoreSearchForm = Nothing
        Private _competitorStoreDataObject As DataAccess.CompetitorStore
        Private _competitorPriceDataObject As DataAccess.CompetitorPrice
        Private _dataSet As CompetitorStoreDataSet

#End Region

#Region "Properties"

        Private ReadOnly Property ItemSearchForm() As ItemSearch
            Get
                If _itemSearchForm Is Nothing OrElse _itemSearchForm.IsDisposed Then
                    _itemSearchForm = New ItemSearch()
                End If

                Return _itemSearchForm
            End Get
        End Property

        Private ReadOnly Property CompetitorStoreSearchForm() As CompetitorStoreSearchForm
            Get
                If _storeSearchForm Is Nothing OrElse _storeSearchForm.IsDisposed Then
                    _storeSearchForm = New CompetitorStoreSearchForm
                End If

                Return _storeSearchForm
            End Get
        End Property

        Private ReadOnly Property CompetitorStoreDataObject() As DataAccess.CompetitorStore
            Get
                If _competitorStoreDataObject Is Nothing Then
                    _competitorStoreDataObject = New DataAccess.CompetitorStore
                End If

                Return _competitorStoreDataObject
            End Get
        End Property

        Private ReadOnly Property CompetitorPriceDataObject() As DataAccess.CompetitorPrice
            Get
                If _competitorPriceDataObject Is Nothing Then
                    _competitorPriceDataObject = New DataAccess.CompetitorPrice
                End If

                Return _competitorPriceDataObject
            End Get
        End Property

#End Region

#Region "Helper Methods"

        Private Sub Search()
            Dim competitorID As Nullable(Of Integer) = CType(cmbCompetitor.SelectedValue, Nullable(Of Integer))
            Dim locationID As Nullable(Of Integer) = CType(cmbLocation.SelectedValue, Nullable(Of Integer))
            Dim storeID As Nullable(Of Integer) = CType(cmbStore.SelectedValue, Nullable(Of Integer))
            Dim fiscalWeek As CompetitorStoreDataSet.FiscalWeekRow = Nothing

            If cmbFiscalWeek.SelectedValue IsNot Nothing Then
                fiscalWeek = CType(CType(cmbFiscalWeek.SelectedValue, DataRowView).Row, CompetitorStoreDataSet.FiscalWeekRow)
            End If

            _dataSet.CompetitorPrice.Rows.Clear()

            CompetitorPriceDataObject.Search(_dataSet, competitorID, locationID, storeID, fiscalWeek, txtWFMIdentifier.Text)

            If _dataSet.CompetitorPrice.Count = 0 Then
                MessageBox.Show(My.Resources.CompetitorStore.NoPricesFound, My.Resources.CompetitorStore.CompetitorPriceSearch)
            End If

            cpgGrid.DataSourceRefreshed()
        End Sub

        Private Sub Save()
            Windows.Forms.Cursor.Current = Cursors.WaitCursor

            CompetitorPriceDataObject.Save(_dataSet)

            Windows.Forms.Cursor.Current = Cursors.Default
        End Sub

        Private Sub LoadAndBind()
            _dataSet = New CompetitorStoreDataSet()

            _dataSet.Competitor.AddCompetitorRow(My.Resources.CompetitorStore.AllItems).AcceptChanges()
            _dataSet.CompetitorLocation.AddCompetitorLocationRow(My.Resources.CompetitorStore.AllItems).AcceptChanges()

            FiscalWeek.List(_dataSet)
            Competitor.List(_dataSet)
            CompetitorLocation.List(_dataSet)
            ItemUnit.List(_dataSet)

            FiscalWeekBindingSource.DataSource = _dataSet
            CompetitorStoreBindingSource.DataSource = _dataSet
            CompetitorLocationBindingSource.DataSource = _dataSet
            CompetitorBindingSource.DataSource = _dataSet
        End Sub

#End Region

#Region "Constructors"

        ''' <summary>
        ''' Provide filter settings for a search to perform before showing the user the form
        ''' </summary>
        Public Sub New(ByVal competitorID As Integer, ByVal competitorLocationID As Integer, ByVal competitorStoreID As Integer, ByVal fiscalWeekDescription As String)
            Dim store As CompetitorStoreDataSet.CompetitorStoreRow

            InitializeComponent()

            LoadAndBind()

            cmbFiscalWeek.SelectedIndex = cmbFiscalWeek.FindStringExact(fiscalWeekDescription)
            cmbCompetitor.SelectedValue = competitorID
            cmbLocation.SelectedValue = competitorLocationID

            ' On change of the selected value, the competitor store row will automatically be found
            ' and added to the combo box
            store = _dataSet.CompetitorStore.FindByCompetitorStoreID(competitorStoreID)

            If store IsNot Nothing Then
                cmbStore.SelectedValue = store.CompetitorStoreID
            End If

            cpgGrid.Initialize(_dataSet, Nothing)

            If store IsNot Nothing Then
                Search()
            End If
        End Sub

        ''' <summary>
        ''' Default behavior; pre-selects the current fiscal week but does not run the search
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
            Dim currentWeek As CompetitorStoreDataSet.FiscalWeekRow

            InitializeComponent()

            LoadAndBind()

            currentWeek = FiscalWeek.FindCurrent(_dataSet)

            If currentWeek IsNot Nothing Then
                cmbFiscalWeek.SelectedIndex = cmbFiscalWeek.FindStringExact(currentWeek.FiscalWeekDescription)
            End If

            cpgGrid.Initialize(_dataSet, currentWeek)
        End Sub

#End Region

#Region "Form Event Handlers"

        Private Sub CompetitorDataManagement_FormClosing(ByVal sender As Object, ByVal e As FormClosingEventArgs) Handles Me.FormClosing
            If _dataSet.HasChanges() AndAlso MessageBox.Show(My.Resources.CompetitorStore.UnsavedChangesBeforeExit, My.Resources.CompetitorStore.UnsavedChangesTitle, MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.No Then
                e.Cancel = True
            End If
        End Sub

#End Region

#Region "Combo Box Event Handlers"

        Private Sub cmbCompetitor_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cmbCompetitor.SelectedIndexChanged, cmbLocation.SelectedIndexChanged
            _dataSet.CompetitorStore.Rows.Clear()

            If cmbCompetitor.SelectedIndex > 0 AndAlso cmbLocation.SelectedIndex > 0 Then
                CompetitorStoreDataObject.Search(_dataSet, _
                    CType(cmbCompetitor.SelectedValue, Nullable(Of Integer)), _
                    CType(cmbLocation.SelectedValue, Nullable(Of Integer)))

                cmbStore.Enabled = True
            Else
                cmbStore.Enabled = False
            End If
        End Sub

#End Region

#Region "Button Click Event Handlers"

#Region "Matching"

        Private Sub btnItemSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnItemSearch.Click
            If cpgGrid.SelectedRowsAvailableForMatching Then
                If ItemSearchForm.ShowDialog() = Windows.Forms.DialogResult.OK Then
                    cpgGrid.UpdateSelectedRowsWithItem(_itemSearchForm.SelectedItem_Key.Value, _itemSearchForm.SelectedIdentifier)
                End If
            End If
        End Sub

        Private Sub btnStoreSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStoreSearch.Click
            If cpgGrid.SelectedRowsAvailableForMatching Then
                If CompetitorStoreSearchForm.ShowDialog() = Windows.Forms.DialogResult.OK Then
                    cpgGrid.UpdateSelectedRowsWithStore(_storeSearchForm.SelectedCompetitorStore)
                End If
            End If
        End Sub

#End Region

        Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
            ' Saving logic in Closing event handler
            Me.Close()
        End Sub

        Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
            If cpgGrid.ShouldSaveChanges() Then
                Save()
            End If
        End Sub

        Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
            If cpgGrid.ShouldSaveChanges() Then

                If _dataSet.HasChanges() Then
                    Select Case MessageBox.Show(My.Resources.CompetitorStore.UnsavedChangesBeforeSearch, My.Resources.CompetitorStore.UnsavedChangesTitle, MessageBoxButtons.YesNoCancel)
                        Case Windows.Forms.DialogResult.Yes
                            Save()
                        Case Windows.Forms.DialogResult.No
                            _dataSet.RejectChanges()
                        Case Windows.Forms.DialogResult.Cancel
                            Return
                    End Select
                End If

                Search()
            End If
        End Sub

#End Region

    End Class
End Namespace