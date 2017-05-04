Imports WholeFoods.IRMA.ItemChaining.DataAccess

Namespace WholeFoods.IRMA.CompetitorStore.UserInterface
    Public Class ItemSearch

#Region "Properties"

        Public ReadOnly Property SelectedIdentifier() As String
            Get
                If lvResults.SelectedItems.Count = 1 Then
                    Return lvResults.SelectedItems(0).SubItems(0).Text
                Else
                    Return Nothing
                End If
            End Get
        End Property

        Public ReadOnly Property SelectedItem_Description() As String
            Get
                If lvResults.SelectedItems.Count = 1 Then
                    Return lvResults.SelectedItems(0).SubItems(1).Text
                Else
                    Return Nothing
                End If
            End Get
        End Property

        Public ReadOnly Property SelectedItem_Key() As Nullable(Of Integer)
            Get
                If lvResults.SelectedItems.Count = 1 Then
                    Return Convert.ToInt32(lvResults.SelectedItems(0).SubItems(2).Text)
                Else
                    Return Nothing
                End If
            End Get
        End Property

#End Region

#Region "Event Handlers"

        Private Sub ItemSearch_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            ItemSearchControl1.ShowHFM = False
            ItemSearchControl1.ShowWFM = False
        End Sub

        Private Sub SearchBegan() Handles ItemSearchControl1.Searching
            Windows.Forms.Cursor.Current = Cursors.WaitCursor
        End Sub

        Private Sub SearchFinished() Handles ItemSearchControl1.Searched
            Dim item As ListViewItem

            lvResults.BeginUpdate()
            lvResults.Items.Clear()

            For Each result As ItemSearchResults.ResultsRow In ItemSearchControl1.DataSet.Results
                item = New ListViewItem(result.Identifier)
                item.SubItems.Add(result.Item_Description)
                item.SubItems.Add(result.Item_Key)

                lvResults.Items.Add(item)
            Next

            lvResults.EndUpdate()

            Windows.Forms.Cursor.Current = Cursors.Default
        End Sub

#End Region

#Region "Button Click Event Handlers"

        Private Sub btnSelectItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSelectItem.Click
            If lvResults.SelectedItems.Count = 1 Then
                Me.DialogResult = Windows.Forms.DialogResult.OK
                Me.Close()
            Else
                MessageBox.Show(My.Resources.CompetitorStore.SelectItem, My.Resources.CompetitorStore.SelectItemTitle)
            End If
        End Sub

        Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
            Me.DialogResult = Windows.Forms.DialogResult.Cancel
            Me.Close()
        End Sub

#End Region

    End Class
End Namespace