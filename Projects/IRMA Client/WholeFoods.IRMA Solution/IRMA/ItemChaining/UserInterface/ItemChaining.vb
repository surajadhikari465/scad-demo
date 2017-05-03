Namespace WholeFoods.IRMA.ItemChaining.UserInterface
    Public Class ItemChaining
        Private _chain As DataAccess.ItemChain = New DataAccess.ItemChain()
        Public Connection As Object

        Private Sub btnNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNext.Click
            If TabControl1.SelectedTab Is wpWelcome Then
                _chain.ID = ""
                ItemSelectingControl1.SelectedItems.Clear()
                txtChainName.Text = ""
                If radNewChain.Checked Then
                    TabControl1.SelectedTab = wpSearchItems
                Else
                    TabControl1.SelectedTab = wpSelectChain
                End If
            ElseIf TabControl1.SelectedTab Is wpSearchItems Then
                Windows.Forms.Cursor.Current = Cursors.WaitCursor
                lblWait.Visible = True
                Update()
                ItemSearchControl1.Search()
                ItemSelectingControl1.SelectFromDataSet = ItemSearchControl1.DataSet

                lblWait.Visible = False
                TabControl1.SelectedTab = wpSelectItems
                Windows.Forms.Cursor.Current = Cursors.Default
            ElseIf TabControl1.SelectedTab Is wpSelectChain Then

                Windows.Forms.Cursor.Current = Cursors.WaitCursor
                'Load items
                _chain.GetChain(True)

                ItemSelectingControl1.SelectedItems.Clear()
                For Each item As ListViewItem In _chain.ChainedItems
                    ItemSelectingControl1.SelectedItems.Add(item)
                Next
                TabControl1.SelectedTab = wpSearchItems
                Windows.Forms.Cursor.Current = Cursors.Default
            Else
                TabControl1.SelectedIndex = TabControl1.SelectedIndex + 1
                If TabControl1.SelectedIndex = TabControl1.TabCount - 1 Then
                    btnNext.Visible = False
                    btnFinish.Visible = True
                End If
            End If
            btnBack.Enabled = True
        End Sub

        Private Sub frmChaining2_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            pnlWorkArea.SendToBack()
            lblTitle.Text = TabControl1.SelectedTab.Text
            lblHeader.Text = TabControl1.SelectedTab.Tag.ToString()
            btnBack.Enabled = False

        End Sub

        Private Sub TabControl1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TabControl1.SelectedIndexChanged
            Dim dataSet As DataSet
            lblTitle.Text = TabControl1.SelectedTab.Text
            lblHeader.Text = TabControl1.SelectedTab.Tag.ToString()

            If TabControl1.SelectedTab Is wpSelectChain Then
                Update()
                Windows.Forms.Cursor.Current = Cursors.WaitCursor
                lstFoundChain.Items.Clear()

                dataSet = _chain.ListChains

                If dataSet IsNot Nothing AndAlso dataSet.Tables.Count > 0 Then
                    For Each row As Data.DataRow In _chain.ListChains.Tables(0).Rows
                        Dim item As ListViewItem = New ListViewItem
                        item.SubItems.Add(row("ID").ToString())
                        item.Text = row("Value").ToString()
                        lstFoundChain.Items.Add(item)
                    Next
                End If

                lstFoundChain.SelectedItems.Clear()
                If radDeleteChain.Checked Then
                    btnNext.Visible = False
                    btnFinish.Visible = True
                    lblTitle.Text = My.Resources.ItemChaining.SelectChainToDelete
                    lblHeader.Text = My.Resources.ItemChaining.SelectChainToDelete
                Else
                    btnNext.Visible = True
                    btnFinish.Visible = False
                    lblTitle.Text = My.Resources.ItemChaining.SelectChainToEdit
                    lblHeader.Text = My.Resources.ItemChaining.SelectChainToEdit
                    Windows.Forms.Cursor.Current = Cursors.Default
                End If
                btnNext.Enabled = False
                btnFinish.Enabled = False

            End If
        End Sub

        Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
            If TabControl1.SelectedTab Is wpSearchItems Then
                TabControl1.SelectedTab = wpWelcome
            Else
                TabControl1.SelectedIndex = TabControl1.SelectedIndex - 1
            End If
            btnBack.Enabled = TabControl1.SelectedIndex > 0
            btnNext.Visible = True
            btnFinish.Visible = False
        End Sub

        Private Sub wpSelectChain_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles wpSelectChain.Enter

        End Sub

        Private Sub btnFinish_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFinish.Click
            If TabControl1.SelectedTab Is wpSaveChain Then
                Windows.Forms.Cursor.Current = Cursors.WaitCursor
                _chain.ChainName = txtChainName.Text
                ' insert all items
                _chain.ChainedItems.Clear()
                For n As Integer = 0 To ItemSelectingControl1.SelectedItems.Count - 1
                    _chain.ChainedItems.Add(ItemSelectingControl1.SelectedItems(n).SubItems(2).Text)
                Next
                If _chain.CreateChain() Then
                    Windows.Forms.Cursor.Current = Cursors.Default
                    MsgBox(String.Format(My.Resources.ItemChaining.ChainSaved, txtChainName.Text), MsgBoxStyle.OkOnly, My.Resources.ItemChaining.ChainSavedTitle)
                    TabControl1.SelectedIndex = 0
                Else
                    Windows.Forms.Cursor.Current = Cursors.Default
                    MsgBox(String.Format(My.Resources.ItemChaining.ChainNotSaved, txtChainName.Text), MsgBoxStyle.OkOnly, My.Resources.ItemChaining.ChainNotSavedTitle)
                End If
            End If
            If radDeleteChain.Checked Then
                If MsgBox(String.Format(My.Resources.ItemChaining.ConfirmChainDelete, txtChainName.Text), MsgBoxStyle.OkCancel, My.Resources.ItemChaining.ConfirmChainDeleteTitle) = MsgBoxResult.Ok Then
                    'Delete chain
                    Windows.Forms.Cursor.Current = Cursors.WaitCursor
                    If _chain.DeleteChain Then
                        Windows.Forms.Cursor.Current = Cursors.Default
                        MsgBox(String.Format(My.Resources.ItemChaining.ChainDeleted, txtChainName.Text), MsgBoxStyle.OkOnly, My.Resources.ItemChaining.ChainDeletedTitle)
                        TabControl1.SelectedIndex = 0
                    Else
                        Windows.Forms.Cursor.Current = Cursors.Default
                        MsgBox(String.Format(My.Resources.ItemChaining.ChainNotDeleted, txtChainName.Text), MsgBoxStyle.OkOnly, My.Resources.ItemChaining.ChainNotDeletedTitle)
                    End If
                End If
            End If
        End Sub

        Private Sub wpWelcome_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles wpWelcome.Click

        End Sub

        Private Sub wpWelcome_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles wpWelcome.Enter
            btnNext.Visible = True
            btnFinish.Visible = False
            btnNext.Enabled = True
            btnFinish.Enabled = False
            btnBack.Enabled = False
        End Sub

        Private Sub lstFoundChain_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstFoundChain.SelectedIndexChanged
            btnNext.Enabled = lstFoundChain.SelectedItems.Count > 0
            If radDeleteChain.Checked Then
                btnFinish.Enabled = lstFoundChain.SelectedItems.Count > 0
            End If
            If lstFoundChain.SelectedItems.Count > 0 Then
                txtChainName.Text = lstFoundChain.SelectedItems(0).Text
                _chain.ID = lstFoundChain.SelectedItems(0).SubItems(1).Text
            End If
        End Sub

        Private Sub lstFoundChain_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstFoundChain.DoubleClick
            If lstFoundChain.SelectedItems.Count > 0 Then
                If radDeleteChain.Checked Then
                    btnFinish_Click(sender, e)
                Else
                    btnNext_Click(sender, e)
                End If
            End If
        End Sub

        Private Sub wpSaveChain_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles wpSaveChain.Enter
            btnNext.Visible = False
            btnFinish.Enabled = True
            btnFinish.Visible = True
        End Sub

        Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
            If MsgBox(My.Resources.ItemChaining.ConfirmChainingExit, MsgBoxStyle.OkCancel, My.Resources.ItemChaining.ConfirmChainingExitTitle) = MsgBoxResult.Ok Then
                Me.Close()
            End If
        End Sub

        Private Sub ItemSelectingControl1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ItemSelectingControl1.SelectedIndexChanged
            lblMessage.Text = ""
            If ItemSelectingControl1.FoundItemsListView.SelectedItems.Count > 0 Then
                If ItemSelectingControl1.FoundItemsListView.SelectedItems(0).StateImageIndex = 1 Then
                    Windows.Forms.Cursor.Current = Cursors.WaitCursor
                    _chain.ID = ItemSelectingControl1.FoundItemsListView.SelectedItems(0).SubItems(3).Text
                    Dim chains As String = ""
                    For Each row As Data.DataRow In _chain.ListItemChains(ItemSelectingControl1.FoundItemsListView.SelectedItems(0).SubItems(2).Text).Tables(0).Rows
                        If chains <> "" Then chains = chains + ", "
                        chains = chains + row("ItemChainDesc").ToString()
                    Next
                    lblMessage.Text = String.Format(My.Resources.ItemChaining.ItemInChainWarning, chains)
                    If chains.IndexOf(",") > -1 Then lblMessage.Text = lblMessage.Text + "s"
                    Windows.Forms.Cursor.Current = Cursors.Default
                End If
            End If
        End Sub
    End Class
End Namespace