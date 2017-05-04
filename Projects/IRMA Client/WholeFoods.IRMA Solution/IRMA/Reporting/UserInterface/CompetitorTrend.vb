Imports System.Text
Imports WholeFoods.IRMA.CompetitorStore.UserInterface
Imports WholeFoods.IRMA.CompetitorStore.BusinessLogic
Imports WholeFoods.IRMA.CompetitorStore.DataAccess

Namespace WholeFoods.IRMA.Reporting.UserInterface
    Public Class frmCompetitiorTrend

#Region "Member Variables"

        Private _itemSearchForm As ItemSearch = Nothing
        Private _item_key As Nullable(Of Integer)
        Private _csDataSet As New CompetitorStoreDataSet

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

#End Region

#Region "Helper Methods"

        Private Sub LoadWFMStores()
            Dim storeDataSet As New DataSet()
            Dim storeTable As DataTable = ItemHosting.DataAccess.StoreDAO.GetStoreList()

            storeDataSet.Tables.Add(storeTable)

            isSelectStores.FoundItemsListView.StateImageList = Nothing
            isSelectStores.SelectedItemsListView.StateImageList = Nothing
            isSelectStores.SelectFromDataSet = storeDataSet
        End Sub

        Private Sub LoadFiscalWeeks()
            Dim currentWeek As CompetitorStoreDataSet.FiscalWeekRow

            FiscalWeek.List(_csDataSet)

            ' Different DataView for each combo box allows them to have different selected values
            cmbStartFiscalWeek.DisplayMember = _csDataSet.FiscalWeek.FiscalWeekDescriptionColumn.ColumnName
            cmbStartFiscalWeek.DataSource = New DataView(_csDataSet.FiscalWeek)

            cmbEndFiscalWeek.DisplayMember = _csDataSet.FiscalWeek.FiscalWeekDescriptionColumn.ColumnName
            cmbEndFiscalWeek.DataSource = New DataView(_csDataSet.FiscalWeek)

            currentWeek = FiscalWeek.FindCurrent(_csDataSet)

            If currentWeek IsNot Nothing Then
                Dim currentWeekIndex As Integer = cmbStartFiscalWeek.FindStringExact(currentWeek.FiscalWeekDescription)

                cmbStartFiscalWeek.SelectedIndex = currentWeekIndex
                cmbEndFiscalWeek.SelectedIndex = currentWeekIndex
            End If
        End Sub

        Private Sub LoadCompetitivePriceTypes()
            Dim dataSet As New NPVDataSet

            dataSet.CompetitivePriceType.AddCompetitivePriceTypeRow(-1, My.Resources.CompetitorStore.AllItems)

            CompetitivePriceType.List(dataSet.CompetitivePriceType)

            With cmbCompetitivePriceType
                .DisplayMember = dataSet.CompetitivePriceType.DescriptionColumn.ColumnName
                .ValueMember = dataSet.CompetitivePriceType.CompetitivePriceTypeIDColumn.ColumnName
                .DataSource = dataSet.CompetitivePriceType
            End With
        End Sub

        Private Function GetCompetitorStoreIDs() As String
            Dim list As String = Nothing

            If lsCompetitorStores.SelectedListCount > 0 Then
                Dim listBuilder As New StringBuilder()

                For Each store As CompetitorStoreDataSet.CompetitorStoreRow In lsCompetitorStores.SelectedCompetitorStores
                    listBuilder.AppendFormat("{0},", store.CompetitorStoreID)
                Next

                listBuilder.Remove(listBuilder.Length - 1, 1)

                list = listBuilder.ToString()
            End If

            Return list
        End Function

        Private Function GetStore_Nos() As String
            Dim list As String = Nothing

            If isSelectStores.SelectedListCount > 0 Then
                Dim listBuilder As New StringBuilder()

                For Each item As ListViewItem In isSelectStores.SelectedItems
                    listBuilder.AppendFormat("{0},", item.SubItems(2).Text)
                Next

                listBuilder.Remove(listBuilder.Length - 1, 1)

                list = listBuilder.ToString()
            End If

            Return list
        End Function

#End Region

#Region "Form Event Handlers"

        Private Sub frmCompetitorTrend_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            LoadWFMStores()
            LoadFiscalWeeks()
            LoadCompetitivePriceTypes()
        End Sub

#End Region

#Region "Button Event Handlers"

        Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
            Me.Close()
        End Sub

        Private Sub btnItemSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnItemSearch.Click
            If ItemSearchForm.ShowDialog() = Windows.Forms.DialogResult.OK Then
                _item_key = _itemSearchForm.SelectedItem_Key

                lblItemDescription.Text = String.Format(My.Resources.CompetitorStore.ItemDescription, _itemSearchForm.SelectedIdentifier, _itemSearchForm.SelectedItem_Description)
            End If
        End Sub

        Private Sub btnViewReport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnViewReport.Click
            If lsCompetitorStores.SelectedListCount > 0 OrElse isSelectStores.SelectedListCount > 0 Then
                If cmbStartFiscalWeek.SelectedIndex <= cmbEndFiscalWeek.SelectedIndex Then
                    If _item_key.HasValue Then
                        Dim results As New DataTable()
                        Dim report As CompetitorTrendReport

                        CompetitorStore.DataAccess.CompetitorTrend.RunReport(results, _
                            GetStore_Nos(), _
                            GetCompetitorStoreIDs(), _
                            _item_key.Value, _
                            rbRegular.Checked, _
                            CType(CType(cmbStartFiscalWeek.SelectedValue, DataRowView).Row, CompetitorStoreDataSet.FiscalWeekRow), _
                            CType(CType(cmbEndFiscalWeek.SelectedValue, DataRowView).Row, CompetitorStoreDataSet.FiscalWeekRow))

                        report = New CompetitorTrendReport(results, lblItemDescription.Text)
                        report.ShowDialog()
                    Else
                        MessageBox.Show(My.Resources.CompetitorStore.SelectItem, My.Resources.CompetitorStore.SelectItemTitle)
                    End If
                Else
                    MessageBox.Show(My.Resources.CompetitorStore.FiscalWeekRange, My.Resources.CompetitorStore.FiscalWeekRangeTitle)
                End If
            Else
                MessageBox.Show(My.Resources.CompetitorStore.SelectStore, My.Resources.CompetitorStore.SelectStoreTitle)
            End If
        End Sub

#End Region

    End Class
End Namespace