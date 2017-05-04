
Namespace WholeFoods.IRMA.ItemChaining.UserInterface
    Public Class ListSelect
        Inherits System.Windows.Forms.UserControl

#Region "Member Variables"

        Private _initialized As Boolean = False

        Protected WithEvents _addButton As Button
        Protected WithEvents _addAllButton As Button
        Protected WithEvents _removeButton As Button
        Protected WithEvents _removeAllButton As Button
        Protected WithEvents _availableListView As ListView
        Protected WithEvents _selectedListView As ListView

#End Region

#Region "Events"

        Public Event ButtonStateChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Public Event SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

        ''' <summary>
        ''' Fired after an item has left one list and before it is added to the other
        ''' </summary>
        Public Event ItemListChanging(ByVal sender As System.Object, ByVal e As ItemListChangeEventArgs)

        ''' <summary>
        ''' Fired after all items have been moved
        ''' </summary>
        Public Event ListsChanged(ByVal sender As Object, ByVal toSelectedList As Boolean)

#End Region

#Region "Properties"

        Public ReadOnly Property SelectedListCount() As Integer
            Get
                Return _selectedListView.Items.Count
            End Get
        End Property

#End Region

#Region "Helper Classes"

        Public Class ItemListChangeEventArgs : Inherits System.EventArgs
            Public Item As ListViewItem
            Public ToSelectedList As Boolean

            Public Sub New(ByVal myItem As ListViewItem, ByVal myToSelectedList As Boolean)
                Item = myItem
                ToSelectedList = myToSelectedList
            End Sub
        End Class

#End Region

#Region "Helper Methods"

        Protected Sub Initialize(ByVal add As Button, ByVal addAll As Button, ByVal remove As Button, ByVal removeAll As Button, ByVal available As ListView, ByVal selected As ListView)
            _addButton = add
            _addAllButton = addAll
            _removeButton = remove
            _removeAllButton = removeAll
            _availableListView = available
            _selectedListView = selected
            _initialized = True
        End Sub

        ''' <summary>
        ''' Sets the state of the buttons based on the list content and selection
        ''' </summary>
        ''' <remarks></remarks>
        Protected Sub SetButtons()
            If _initialized Then
                _addButton.Enabled = _availableListView.SelectedItems.Count > 0
                _addAllButton.Enabled = _availableListView.Items.Count > 0
                _removeButton.Enabled = _selectedListView.SelectedItems.Count > 0
                _removeAllButton.Enabled = _selectedListView.Items.Count > 0

                RaiseEvent ButtonStateChanged(Me, Nothing)
            End If
        End Sub

        Protected Sub BeforeMoveListItems()
            Windows.Forms.Cursor.Current = Cursors.WaitCursor

            _availableListView.BeginUpdate()
            _selectedListView.BeginUpdate()
        End Sub

        Protected Sub AfterMoveListItems()
            _availableListView.EndUpdate()
            _selectedListView.EndUpdate()

            SetButtons()

            Windows.Forms.Cursor.Current = Cursors.Default
        End Sub
#End Region

#Region "Event Handlers"

#Region "Button"

        ''' <summary>
        ''' Moves selected items from the found list to selected list
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Private Sub AddButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _addButton.Click, _availableListView.DoubleClick
            BeforeMoveListItems()

            Dim selected As ListView.SelectedListViewItemCollection = _availableListView.SelectedItems

            For Each item As ListViewItem In selected
                _availableListView.Items.Remove(item)
                RaiseEvent ItemListChanging(Me, New ItemListChangeEventArgs(item, True))
                _selectedListView.Items.Add(item)
            Next

            RaiseEvent ListsChanged(Me, True)

            AfterMoveListItems()
        End Sub

        ''' <summary>
        ''' Moves all items from found items list to selected items list.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Private Sub _addAllButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _addAllButton.Click
            BeforeMoveListItems()

            For Each item As ListViewItem In _availableListView.Items
                _availableListView.Items.Remove(item)
                RaiseEvent ItemListChanging(Me, New ItemListChangeEventArgs(item, True))
                _selectedListView.Items.Add(item)
            Next

            RaiseEvent ListsChanged(Me, True)

            AfterMoveListItems()
        End Sub

        ''' <summary>
        ''' Removes selected items from the selected items list
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Private Sub _removeButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _removeButton.Click, _selectedListView.DoubleClick
            BeforeMoveListItems()

            Dim selected As ListView.SelectedListViewItemCollection = _selectedListView.SelectedItems

            For Each item As ListViewItem In selected
                _selectedListView.Items.Remove(item)
                RaiseEvent ItemListChanging(Me, New ItemListChangeEventArgs(item, False))
                _availableListView.Items.Add(item)
            Next

            RaiseEvent ListsChanged(Me, False)

            AfterMoveListItems()
        End Sub

        ''' <summary>
        ''' Moves all items from selected items list back to found items list
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Private Sub _removeAllButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _removeAllButton.Click
            BeforeMoveListItems()

            For Each item As ListViewItem In _selectedListView.Items
                _selectedListView.Items.Remove(item)
                RaiseEvent ItemListChanging(Me, New ItemListChangeEventArgs(item, False))
                _availableListView.Items.Add(item)
            Next

            RaiseEvent ListsChanged(Me, False)

            AfterMoveListItems()
        End Sub

#End Region

#Region "Drag and drop"

        ''' <summary>
        ''' User started dragging found items
        ''' </summary>
        Private Sub _availableListView_ItemDrag(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ItemDragEventArgs) Handles _availableListView.ItemDrag
            _availableListView.AllowDrop = False
            _selectedListView.AllowDrop = True
            _availableListView.DoDragDrop(_availableListView.SelectedItems, DragDropEffects.Move)

            SetButtons()
        End Sub

        ''' <summary>
        ''' User drops items on the selected items list
        ''' </summary>
        Private Sub _selectedListView_DragDrop(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles _selectedListView.DragDrop
            AddButton_Click(sender, e)

            _selectedListView.AllowDrop = False

            SetButtons()
        End Sub

        ''' <summary>
        ''' User hovers with dragged items above the selected items list
        ''' </summary>
        Private Sub _selectedListView_DragEnter(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles _selectedListView.DragEnter
            e.Effect = DragDropEffects.Move
        End Sub

        ''' <summary>
        ''' Users drag items from the selected items list back to the found items list
        ''' </summary>
        Private Sub _selectedListView_ItemDrag(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ItemDragEventArgs) Handles _selectedListView.ItemDrag
            _selectedListView.AllowDrop = False
            _availableListView.AllowDrop = True
            _selectedListView.DoDragDrop(_selectedListView.SelectedItems, DragDropEffects.Move)
        End Sub

        ''' <summary>
        ''' User hovers with dragged items above the found items list
        ''' </summary>
        Private Sub _availableListView_DragEnter(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles _availableListView.DragEnter
            e.Effect = DragDropEffects.Move
        End Sub

        ''' <summary>
        ''' Users drops items from the selected items list back on the found items list
        ''' </summary>
        Private Sub _availableListView_DragDrop(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles _availableListView.DragDrop
            _removeButton_Click(sender, e)

            _availableListView.AllowDrop = False

            SetButtons()
        End Sub

#End Region

        ''' <summary>
        ''' Selection in the items list changed
        ''' </summary>
        Private Sub listView_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _availableListView.SelectedIndexChanged, _selectedListView.SelectedIndexChanged
            SetButtons()

            RaiseEvent SelectedIndexChanged(sender, e)
        End Sub

#End Region

    End Class
End Namespace