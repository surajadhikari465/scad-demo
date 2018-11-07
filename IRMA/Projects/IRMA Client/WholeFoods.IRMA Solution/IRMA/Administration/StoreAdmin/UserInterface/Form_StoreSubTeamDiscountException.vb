Imports WholeFoods.IRMA.ItemHosting.DataAccess

Public Class Form_StoreSubTeamDiscountException

    Private _hasChanges As Boolean = False
    Private _lastStore As Integer = -1
    Private _ExceptionsList As List(Of DiscountExceptionsGridBO)

    Private Sub Form_StoreSubTeamDiscountException_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        LoadStore(ComboBox_Stores, True)
    End Sub

    Private Sub LoadDiscountExceptions()
        _hasChanges = False
        _lastStore = ComboBox_Stores.SelectedIndex
        Dim storeNo As Integer = CType(ComboBox_Stores.SelectedItem, VB6.ListBoxItem).ItemData
        Dim DAO As StoreSubTeamDiscountExceptionsDAO = New StoreSubTeamDiscountExceptionsDAO()
        Dim _dataset As DataSet
        _dataset = DAO.GetSubTeams(storeNo)

        _ExceptionsList = New List(Of DiscountExceptionsGridBO)
        For Each SubTeam As DataRow In _dataset.Tables(0).Rows
            _ExceptionsList.Add(New DiscountExceptionsGridBO(CInt(SubTeam("SubTeam_No").ToString()), SubTeam("SubTeam_Name").ToString(), CBool(SubTeam("IsException").ToString())))
        Next
        DataGridView_DiscountExceptions.DataSource = _ExceptionsList
        DataGridView_DiscountExceptions.Columns("SubTeam_Name").ReadOnly = True
        DataGridView_DiscountExceptions.Columns("SubTeam_Name").Width = 400
        DataGridView_DiscountExceptions.Columns("SubTeam_No").Visible = False
        DataGridView_DiscountExceptions.Columns("OriginalExceptionState").Visible = False
    End Sub

    Private Sub CheckForChanges()
        If Not _hasChanges And Not _ExceptionsList Is Nothing Then
            '_ExceptionsList = DataGridView_DiscountExceptions.DataSource
            For Each de As DiscountExceptionsGridBO In _ExceptionsList
                If de.DiscountException <> de.OriginalExceptionState Then
                    _hasChanges = True
                End If
            Next
        End If
    End Sub

    Private Sub Button_Close_Click(sender As System.Object, e As System.EventArgs) Handles Button_Close.Click
        Me.Close()
    End Sub

    Private Sub ComboBox_Stores_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles ComboBox_Stores.SelectedIndexChanged
        Dim reloadGrid As Boolean = True

        CheckForChanges()
        If _lastStore > -1 And _hasChanges And _lastStore <> ComboBox_Stores.SelectedIndex Then
            If MessageBox.Show("Changes pending! Do you wish to ignore the pending changes and continue?", Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = Windows.Forms.DialogResult.No Then
                ComboBox_Stores.SelectedIndex = _lastStore
                reloadGrid = False
            End If
        End If

        If reloadGrid And _lastStore <> ComboBox_Stores.SelectedIndex Then
            LoadDiscountExceptions()
        End If
    End Sub

    Private Sub Form_StoreSubTeamDiscountException_FormClosing(sender As System.Object, e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        CheckForChanges()
        If _hasChanges Then
            If MessageBox.Show("Changes pending! Do you wish to ignore the pending changes and close the window?", Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = Windows.Forms.DialogResult.No Then
                e.Cancel = True
            End If
        End If
    End Sub

    Private Sub Button_Save_Click(sender As System.Object, e As System.EventArgs) Handles Button_Save.Click
        Dim DAO As StoreSubTeamDiscountExceptionsDAO = New StoreSubTeamDiscountExceptionsDAO()
        Dim _dataset As DataSet
        Dim storeNo As Integer = CType(ComboBox_Stores.SelectedItem, VB6.ListBoxItem).ItemData

        CheckForChanges()
        If _hasChanges Then
            For Each de As DiscountExceptionsGridBO In _ExceptionsList
                If de.OriginalExceptionState <> de.DiscountException Then
                    If de.OriginalExceptionState Then
                        _dataset = DAO.RemoveDiscountException(storeNo, de.SubTeam_No)
                    Else
                        _dataset = DAO.AddDiscountException(storeNo, de.SubTeam_No)
                    End If
                End If
            Next
            'Reloading the details will reset the original state value to ensure that changes are properly checked.
            LoadDiscountExceptions()
        End If
    End Sub
End Class