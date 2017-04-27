Imports System.ComponentModel
Imports WholeFoods.IRMA.CompetitorStore.BusinessLogic
Imports WholeFoods.IRMA.CompetitorStore.DataAccess
Imports Infragistics.Win
Imports Infragistics.Win.UltraWinGrid

Namespace WholeFoods.IRMA.CompetitorStore.UserInterface
    Public Class ImportPreview

#Region "Member Variables"

        ' Child Forms
        Private _itemSearchForm As ItemSearch = Nothing
        Private _storeSearchForm As CompetitorStoreSearchForm = Nothing

        ' Data
        Private _currentSession As CompetitorStoreDataSet.CompetitorImportSessionRow
        Private _previewDataSet As CompetitorStoreDataSet

        ' Data Access
        Private _competitorImportSessionDataObject As CompetitorImportSession
        Private _competitorImportInfoDataObject As CompetitorImportInfo

        Private _importComplete As Boolean = False
        Private _handleChangeEvents As Boolean = False

#End Region

#Region "Properties"

#Region "Private"

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

        Private ReadOnly Property CompetitorImportSessionDataObject() As CompetitorImportSession
            Get
                If _competitorImportSessionDataObject Is Nothing Then
                    _competitorImportSessionDataObject = New CompetitorImportSession()
                End If

                Return _competitorImportSessionDataObject
            End Get
        End Property

        Private ReadOnly Property CompetitorImportInfoDataObject() As CompetitorImportInfo
            Get
                If (_competitorImportInfoDataObject Is Nothing) Then
                    _competitorImportInfoDataObject = New CompetitorImportInfo()
                End If

                Return _competitorImportInfoDataObject
            End Get
        End Property

#End Region

#Region "Public"

        Public ReadOnly Property SelectedCompetitorStore() As CompetitorStoreDataSet.CompetitorStoreRow
            Get
                Dim store As CompetitorStoreDataSet.CompetitorStoreRow = Nothing
                Dim storeID As Nullable(Of Integer) = Nothing
                Dim importInfoCount As Integer = _previewDataSet.CompetitorImportInfo.Count

                If importInfoCount > 0 Then
                    Dim view As New DataView(_previewDataSet.CompetitorImportInfo)
                    Dim firstInfo As CompetitorStoreDataSet.CompetitorImportInfoRow = _previewDataSet.CompetitorImportInfo(0)

                    ' Check if all rows have the same competitor store
                    view.RowFilter = String.Format("Competitor <> '{0}' OR Location <> '{1}' OR CompetitorStore <> '{2}'", _
                        firstInfo.Competitor, firstInfo.Location, firstInfo.CompetitorStore)

                    If view.Count = 0 Then
                        store = CompetitorStore.DataAccess.CompetitorStore.GetByName(_previewDataSet, firstInfo.Competitor, firstInfo.Location, firstInfo.CompetitorStore)
                    End If
                End If

                Return store
            End Get
        End Property

        Public ReadOnly Property SelectedFiscalWeekDescription() As String
            Get
                Dim fiscalWeek As CompetitorStoreDataSet.FiscalWeekRow = Nothing
                Dim description As String = Nothing
                Dim importInfoCount As Integer = _previewDataSet.CompetitorImportInfo.Count

                If importInfoCount > 0 Then
                    fiscalWeek = _previewDataSet.CompetitorImportInfo(0).FiscalWeekRowParent

                    If importInfoCount > 1 Then
                        Dim view As New DataView(_previewDataSet.CompetitorImportInfo)

                        view.RowFilter = String.Format("FiscalYear <> {0} OR FiscalPeriod <> {1} OR PeriodWeek <> {2}", _
                            fiscalWeek.FiscalYear, fiscalWeek.FiscalPeriod, fiscalWeek.PeriodWeek)

                        If view.Count = 0 Then
                            description = fiscalWeek.FiscalWeekDescription
                        End If
                    Else
                        description = fiscalWeek.FiscalWeekDescription
                    End If
                End If

                Return description
            End Get
        End Property

#End Region

#End Region

#Region "Helper Methods"

        Private Sub FilterRadioButtonChanged(ByVal sender As Object, ByVal rowFilter As String)
            If _handleChangeEvents AndAlso TypeOf sender Is RadioButton AndAlso CType(sender, RadioButton).Checked Then
                cgGrid.Filter = rowFilter
            End If
        End Sub

#End Region

#Region "Constructors"

        Public Sub New(ByVal previewDataSet As CompetitorStoreDataSet, ByVal currentSession As CompetitorStoreDataSet.CompetitorImportSessionRow)
            _previewDataSet = previewDataSet
            _currentSession = currentSession

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            cgGrid.Initialize(previewDataSet)

            _handleChangeEvents = True
        End Sub

#End Region

#Region "Form Event Handlers"

        Private Sub ImportPreview_FormClosing(ByVal sender As Object, ByVal e As FormClosingEventArgs) Handles Me.FormClosing
            If Not _importComplete Then
                If MessageBox.Show(My.Resources.CompetitorStore.ImportConfirmQuit, My.Resources.CompetitorStore.ConfirmQuitTitle, MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
                    If _currentSession IsNot Nothing Then
                        CompetitorImportSessionDataObject.Delete(_currentSession)
                    End If
                Else
                    e.Cancel = True
                End If
            End If
        End Sub

#End Region

#Region "Button Click Event Handlers"

#Region "Search"

        Private Sub btnStoreSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStoreSearch.Click
            If cgGrid.SelectedRowsAvailableForMatching Then
                If CompetitorStoreSearchForm.ShowDialog() = Windows.Forms.DialogResult.OK Then
                    cgGrid.UpdateSelectedRowsWithStore(_storeSearchForm.SelectedCompetitorStore)
                End If
            End If
        End Sub

        Private Sub btnItemSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnItemSearch.Click
            If cgGrid.SelectedRowsAvailableForMatching Then
                If ItemSearchForm.ShowDialog() = Windows.Forms.DialogResult.OK Then
                    cgGrid.UpdateSelectedRowsWithItem(_itemSearchForm.SelectedItem_Key.Value, _itemSearchForm.SelectedIdentifier)
                End If
            End If
        End Sub

#End Region

        Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
            Me.Close()
        End Sub

        Private Sub btnDone_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDone.Click
            If cgGrid.ShouldSaveChanges() Then
                Dim existingRowCount As Integer
                Dim overwriteExistingRows As Boolean = True
                Dim success As Boolean = False

                Windows.Forms.Cursor.Current = Cursors.WaitCursor

                CompetitorImportInfoDataObject.Save(_previewDataSet)

                ' Save new identifiers
                CompetitorStoreIdentifier.Save(_previewDataSet)

                ' Check for existing data
                existingRowCount = CompetitorImportInfoDataObject.GetExistingRowCount(_currentSession)

                If (existingRowCount > 0) Then
                    overwriteExistingRows = (MessageBox.Show(String.Format(My.Resources.CompetitorStore.ExistingCompetitorPrices, existingRowCount), _
                        My.Resources.CompetitorStore.ExistingCompetitorPricesTitle, MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes)
                End If

                ' Move data into CompetitorPrice
                success = CompetitorImportSessionDataObject.InsertCompetitorPriceFromImportSession(_currentSession, overwriteExistingRows)
                Windows.Forms.Cursor.Current = Cursors.Default

                If success Then
                    MessageBox.Show(My.Resources.CompetitorStore.ImportSuccessful)
                    CompetitorImportSessionDataObject.Delete(_currentSession)
                    Me.DialogResult = Windows.Forms.DialogResult.OK
                Else
                    MessageBox.Show(My.Resources.CompetitorStore.ImportFailed)
                End If

                _importComplete = True

                Me.Close()
            End If
        End Sub

#End Region

#Region "Radio Button Event Handlers"

        Private Sub rbAllItems_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbAllItems.CheckedChanged
            FilterRadioButtonChanged(sender, Nothing)
        End Sub

        Private Sub rbUnmatchedStores_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbUnmatchedStores.CheckedChanged
            FilterRadioButtonChanged(sender, "CompetitorStoreID IS NULL")
        End Sub

        Private Sub rbUnmatchedUPCs_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbUnmatchedUPCs.CheckedChanged
            FilterRadioButtonChanged(sender, "Item_Key IS NULL")
        End Sub

        Private Sub rbMissingData_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbMissingData.CheckedChanged
            FilterRadioButtonChanged(sender, "Price IS NULL OR Size IS NULL")
        End Sub

#End Region

    End Class
End Namespace