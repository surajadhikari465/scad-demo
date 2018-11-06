Imports System.Text
Imports WholeFoods.IRMA.CompetitorStore.BusinessLogic
Imports WholeFoods.IRMA.CompetitorStore.DataAccess

Namespace WholeFoods.IRMA.CompetitorStore.UserInterface
    Public Class CompetitorStoreListSelect

#Region "Member Variables"

        Private _competitorStoreDataObject As New DataAccess.CompetitorStore
        Private _dataSet As CompetitorStoreDataSet
        Private _competitorStoreView As DataView
        Private _store_No As New Nullable(Of Integer)
        Private _showPriority As Boolean = True

#End Region

#Region "Properties"

        Public Property ShowFilter() As Boolean
            Get
                Return pnlFilter.Visible
            End Get
            Set(ByVal value As Boolean)
                pnlFilter.Visible = value
            End Set
        End Property

        Public WriteOnly Property ShowPriority() As Boolean
            Set(ByVal value As Boolean)
                _showPriority = value

                If Not value Then
                    btnIncreasePriority.Visible = False
                    btnDecreasePriority.Visible = False

                    lstSelected.Columns.Remove(colPriority)
                End If
            End Set
        End Property

        Public WriteOnly Property DataSet() As CompetitorStoreDataSet
            Set(ByVal value As CompetitorStoreDataSet)
                _dataSet = value
            End Set
        End Property

        Public WriteOnly Property WFMStore_No() As Nullable(Of Integer)
            Set(ByVal value As Nullable(Of Integer))
                BeforeMoveListItems()

                _store_No = value

                ' Clear selected stores
                lstSelected.Items.Clear()
                _dataSet.StoreCompetitorStore.Rows.Clear()
                
                If value.HasValue Then
                    ' Get StoreCompetitorStores
                    StoreCompetitorStore.GetByStore_No(_dataSet, value.Value)

                    ' Add the related stores to the selected list
                    For Each row As CompetitorStoreDataSet.StoreCompetitorStoreRow In _dataSet.StoreCompetitorStore
                        lstSelected.Items.Add(CreateCompetitorStoreListViewItem(row))
                    Next
                End If

                FilterAvailableCompetitorStores()
                BindAvailableCompetitorStores()

                AfterMoveListItems()
            End Set
        End Property

        Public ReadOnly Property SelectedCompetitorStores() As CompetitorStoreDataSet.CompetitorStoreRow()
            Get
                Dim vbOffsetCount As Integer = lstSelected.Items.Count - 1
                Dim rows(vbOffsetCount) As CompetitorStoreDataSet.CompetitorStoreRow

                For i As Integer = 0 To vbOffsetCount
                    rows(i) = _dataSet.CompetitorStore.FindByCompetitorStoreID(CInt(lstSelected.Items(i).SubItems(4).Text))
                Next

                Return rows
            End Get
        End Property

#End Region

#Region "Helper Methods"

#Region "ListViewItem <=> StoreCompetitorStore"

        Private Function CreateCompetitorStoreListViewItem(ByVal storeCompetitorStoreRow As CompetitorStoreDataSet.StoreCompetitorStoreRow) As ListViewItem
            Return CreateCompetitorStoreListViewItem(storeCompetitorStoreRow.Priority, storeCompetitorStoreRow.CompetitorStoreRow)
        End Function

        Private Function CreateCompetitorStoreListViewItem(ByVal priority As Integer, ByVal storeRow As CompetitorStoreDataSet.CompetitorStoreRow) As ListViewItem
            Return New ListViewItem(New String() {priority.ToString(), storeRow.CompetitorRow.Name, storeRow.Name, storeRow.CompetitorLocationRow.Name, storeRow.CompetitorStoreID.ToString()})
        End Function

        Private Function CreateCompetitorStoreListViewItem(ByVal storeRow As CompetitorStoreDataSet.CompetitorStoreRow) As ListViewItem
            Return New ListViewItem(New String() {storeRow.CompetitorRow.Name, storeRow.Name, storeRow.CompetitorLocationRow.Name, storeRow.CompetitorStoreID.ToString()})
        End Function

        Private Sub UpdateStoreCompetitorStore(ByVal item As ListViewItem)
            Dim priority As Byte = Convert.ToByte(item.SubItems(0).Text)
            Dim competitorStoreId As Integer = Convert.ToInt32(item.SubItems(4).Text)

            Try
                _dataSet.StoreCompetitorStore.AddStoreCompetitorStoreRow(_store_No.Value, _dataSet.CompetitorStore.FindByCompetitorStoreID(competitorStoreId), priority)
            Catch ex As Data.ConstraintException
                ' A row with this key already exists
                Dim row As CompetitorStoreDataSet.StoreCompetitorStoreRow = _
                    _dataSet.StoreCompetitorStore.FindByStore_NoCompetitorStoreID(_store_No.Value, competitorStoreId)

                ' Reject the deletion from earlier
                row.RejectChanges()
                row.Priority = priority
            End Try
        End Sub

#End Region

#Region "Priority"

        Private Sub UpdatePriority(ByVal item As ListViewItem)
            item.SubItems(0).Text = (item.Index + 1).ToString()
        End Sub

        Private Sub UpdatePriorities()
            For Each item As ListViewItem In lstSelected.Items
                UpdatePriority(item)
            Next
        End Sub

        Private Sub PriorityChange(ByVal increase As Boolean)
            If lstSelected.SelectedItems.Count = 1 Then
                Dim selectedItem As ListViewItem = lstSelected.SelectedItems(0)
                Dim selectedListCount As Integer = lstSelected.Items.Count

                ' If an item is selected and it's able to move in the direction specified
                If selectedListCount > 1 AndAlso ((increase AndAlso selectedItem.Index > 0) _
                    OrElse (Not increase AndAlso selectedItem.Index < (selectedListCount - 1))) Then

                    Dim swapItem As ListViewItem = lstSelected.Items(selectedItem.Index + CInt(IIf(increase, -1, 1)))
                    Dim oldSwapIndex As Integer = swapItem.Index

                    BeforeMoveListItems()

                    selectedItem.Remove()

                    lstSelected.Items.Insert(oldSwapIndex, selectedItem)

                    UpdatePriority(selectedItem)
                    UpdatePriority(swapItem)

                    AfterMoveListItems()
                End If
            Else
                MessageBox.Show(My.Resources.CompetitorStore.PriorityChangeSingleStore, My.Resources.CompetitorStore.SelectOneStore)
            End If
        End Sub

#End Region

        Private Sub SetupFilter()
            _dataSet.Competitor.AddCompetitorRow(My.Resources.CompetitorStore.AllItems)
            _dataSet.CompetitorLocation.AddCompetitorLocationRow(My.Resources.CompetitorStore.AllItems)

            Competitor.List(_dataSet)
            CompetitorLocation.List(_dataSet)

            cmbCompetitor.DisplayMember = _dataSet.Competitor.NameColumn.ColumnName
            cmbCompetitor.ValueMember = _dataSet.Competitor.CompetitorIDColumn.ColumnName
            cmbCompetitor.DataSource = _dataSet.Competitor

            cmbLocation.DisplayMember = _dataSet.CompetitorLocation.NameColumn.ColumnName
            cmbLocation.ValueMember = _dataSet.CompetitorLocation.CompetitorLocationIDColumn.ColumnName
            cmbLocation.DataSource = _dataSet.CompetitorLocation

            ' Don't add these handlers until the lists have been configured
            AddHandler cmbCompetitor.SelectedIndexChanged, New EventHandler(AddressOf ComboBox_SelectedIndexChanged)
            AddHandler cmbLocation.SelectedIndexChanged, New EventHandler(AddressOf ComboBox_SelectedIndexChanged)
        End Sub

        Private Sub BindAvailableCompetitorStores()
            For Each rowView As DataRowView In _competitorStoreView
                lstAvailable.Items.Add(CreateCompetitorStoreListViewItem(CType(rowView.Row, CompetitorStoreDataSet.CompetitorStoreRow)))
            Next
        End Sub

        Private Sub LoadAvailableStores()
            _competitorStoreDataObject.Search(_dataSet, Nothing, Nothing)

            _competitorStoreView = New DataView(_dataSet.CompetitorStore)

            BindAvailableCompetitorStores()
        End Sub

        Private Sub FilterAvailableCompetitorStores()
            Dim filterBuilder As New StringBuilder()

            lstAvailable.Items.Clear()

            If cmbCompetitor.SelectedIndex > 0 Then
                filterBuilder.AppendFormat("CompetitorID = {0} AND ", CInt(cmbCompetitor.SelectedValue))
            End If

            If cmbLocation.SelectedIndex > 0 Then
                filterBuilder.AppendFormat("CompetitorLocationID = {0} AND ", CInt(cmbLocation.SelectedValue))
            End If

            For Each item As ListViewItem In lstSelected.Items
                filterBuilder.AppendFormat("CompetitorStoreID <> {0} AND ", CInt(item.SubItems(4).Text))
            Next

            If filterBuilder.Length > 0 Then
                ' "AND ".length
                filterBuilder.Remove(filterBuilder.Length - 4, 4)
            End If

            _competitorStoreView.RowFilter = filterBuilder.ToString()
        End Sub

#End Region

#Region "Public Methods"

        Public Sub SaveSelections()
            If _store_No.HasValue AndAlso _showPriority Then
                ' Anything still in the list will be updated instead
                For Each row As CompetitorStoreDataSet.StoreCompetitorStoreRow In _dataSet.StoreCompetitorStore
                    row.Delete()
                Next

                For Each item As ListViewItem In lstSelected.Items
                    UpdateStoreCompetitorStore(item)
                Next

                StoreCompetitorStore.Save(_dataSet)
            End If
        End Sub

#End Region

#Region "Event Handlers"

        Private Sub CompetitorStoreListSelect_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
            'Sets transparency
            Me.SetStyle(System.Windows.Forms.ControlStyles.DoubleBuffer, True)
            Me.SetStyle(System.Windows.Forms.ControlStyles.AllPaintingInWmPaint, False)
            Me.SetStyle(System.Windows.Forms.ControlStyles.ResizeRedraw, True)
            Me.SetStyle(System.Windows.Forms.ControlStyles.UserPaint, True)
            Me.SetStyle(System.Windows.Forms.ControlStyles.SupportsTransparentBackColor, True)
            Me.BackColor = System.Drawing.Color.Transparent

            Me.Initialize(btnAdd, btnAddAll, btnRemove, btnRemoveAll, lstAvailable, lstSelected)

            If Not Me.DesignMode Then
                If _dataSet Is Nothing Then
                    _dataSet = New CompetitorStoreDataSet()
                End If

                SetupFilter()

                LoadAvailableStores()
            End If

            If _showPriority Then
                AddHandler Me.ItemListChanging, AddressOf MyItemListChanging
                AddHandler Me.ListsChanged, AddressOf Me.MyListsChanged
            End If

            SetButtons()
        End Sub

        Private Sub ComboBox_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
            BeforeMoveListItems()

            FilterAvailableCompetitorStores()

            BindAvailableCompetitorStores()

            AfterMoveListItems()
        End Sub

        Private Sub MyItemListChanging(ByVal sender As System.Object, ByVal e As WholeFoods.IRMA.ItemChaining.UserInterface.ListSelect.ItemListChangeEventArgs)
            If e.ToSelectedList Then
                e.Item.SubItems.Insert(0, New ListViewItem.ListViewSubItem(e.Item, (lstSelected.Items.Count + 1).ToString()))
            Else
                e.Item.SubItems.RemoveAt(0)
            End If
        End Sub

        Private Sub MyListsChanged(ByVal sender As Object, ByVal toSelectedList As Boolean)
            If Not toSelectedList Then
                UpdatePriorities()
            End If
        End Sub

        Private Sub btnIncreasePriority_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnIncreasePriority.Click
            PriorityChange(True)
        End Sub

        Private Sub btnDecreasePriority_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDecreasePriority.Click
            PriorityChange(False)
        End Sub

#End Region

    End Class
End Namespace