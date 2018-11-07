Option Strict Off
Option Explicit On
Imports System.Linq

Friend Class frmLocationEdit
    Inherits System.Windows.Forms.Form

    Private mbChanged As Boolean
    Private mbLoading As Boolean
    Private mlLocID As Integer
    Private mlStoreID As Integer 'Used for setting the default Store.
    Private mlSubTeamID As Integer 'Used for setting the default Sub-Team.
    Private mbIsManufacturing As Boolean 'Indicates that the sub-team for this location is a manufacturing sub-team.
    Private mbUserUpdates As Boolean 'Indicates that the user can update (save) the form.

    Private mlSubTeamList() As Boolean

    Private IsInitializing As Boolean


    Private Sub frmLocationEdit_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load

        'Test... UPGRADE_WARNING: Arrays in structure rsLocation may need to be initialized before they can be used. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="814DF224-76BD-4BB4-BFFB-EA359CB9FC48"'
        Dim rsLocation As DAO.Recordset = Nothing

        mbChanged = False
        mbLoading = True

        '-- Center form
        CenterForm(Me)

        '-- Load Store combo.
        LoadInventoryStore(cboStore)
        If glStore_Limit > 0 Then
            Call SetActive(cboStore, False)
            Call SetCombo(cboStore, glStore_Limit)
        End If

        '-- Load Inventory Location data if we have a record id.
        If mlLocID <> 0 Then
            Try
                rsLocation = SQLOpenRecordSet("EXEC GetInventoryLocation " & mlLocID, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)

                Call SetCombo(cboStore, Convert.ToInt64(rsLocation.Fields("Store_No").Value))
                Call SetActive(cboStore, False)

                Call LoadSubTeams()

                Call SetCombo(cboSubTeam, Convert.ToInt64(rsLocation.Fields("SubTeam_No").Value))
                Call SetActive(cboSubTeam, False)
                mlSubTeamID = rsLocation.Fields("SubTeam_No").Value

                txtLocName.Text = rsLocation.Fields("InvLoc_Name").Value
                txtLocDesc.Text = rsLocation.Fields("InvLoc_Desc").Value
                'Test... UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"'
                txtNotes.Text = IIf(IsDBNull(rsLocation.Fields("Notes").Value), Nothing, rsLocation.Fields("Notes").Value)

            Finally
                If rsLocation IsNot Nothing Then
                    rsLocation.Close()
                    rsLocation = Nothing
                End If
            End Try

        End If

        Call SetFormPermissions()

        mbLoading = False

    End Sub

    Public Sub Load_Form(Optional ByRef lInvLocID As Integer = 0, Optional ByRef lSubTeamID As Integer = 0, Optional ByRef bIsManufacturing As Boolean = False, Optional ByRef lStoreID As Integer = 0)

        mlLocID = lInvLocID
        mlSubTeamID = lSubTeamID
        mlStoreID = lStoreID
        mbIsManufacturing = bIsManufacturing

        Me.ShowDialog()

    End Sub

    Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click

        Dim iAns As Short

        If mbUserUpdates Then
            If mbChanged Then
                iAns = MsgBox("Save changes before closing?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, "Data changed!")
                Select Case iAns
                    Case MsgBoxResult.Yes
                        If Not Save() Then Exit Sub
                    Case MsgBoxResult.Cancel
                        Exit Sub
                End Select
            End If
        End If

        Me.Close()

    End Sub

    Private Sub cmdLocItems_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdLocItems.Click

        If mlLocID = 0 Then

            If Not ValidateData(False) Then
                'Show msg box and tell them they need to correct info and save before editing items.
                Call MsgBox("Location must saved before adding items, but cannot be saved until all necessary fields are filled out." & Chr(13) & "Location, sub-team, and location name are required.", MsgBoxStyle.Exclamation, "Cannot add items")
                Exit Sub
            Else
                If Not Save() Then Exit Sub
            End If

        End If

        Call frmLocationItemList.LoadForm(mlLocID, mlSubTeamID, mlStoreID, Trim(txtLocName.Text), mbIsManufacturing)
        frmLocationItemList.Dispose()

    End Sub

    Private Sub SetFormPermissions()

        '-- Disable functionality based on user privileges.

        mbUserUpdates = True

        If Not gbInventoryAdministrator Then
            Call SetActive(cboStore, False)
            Call SetActive(cboSubTeam, False)

            Call SetActive(cmdApply, False)
            Call SetActive((Me.txtLocDesc), False)
            Call SetActive((Me.txtLocName), False)
            Call SetActive((Me.txtNotes), False)

            mbUserUpdates = False
        End If

    End Sub

    Private Sub LoadSubTeams()

        '-- Limit sub-teams user-assigned and only those related to the selected location.
        If cboStore.SelectedIndex > -1 Then
            'Load the user's sub-teams restricted to the selected store.
            Call LoadSubTeamByType(Global_Renamed.enumSubTeamType.StoreUser, cboSubTeam, mlSubTeamList, VB6.GetItemData(cboStore, cboStore.SelectedIndex), 0)
        Else
            cboSubTeam.Items.Clear()
        End If

    End Sub

    Private Sub cboStore_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles cboStore.KeyPress
        Dim KeyAscii As Short = Asc(eventArgs.KeyChar)

        If KeyAscii = 8 Then
            cboStore.SelectedIndex = -1
        End If

        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
    End Sub

    Private Sub cboSubTeam_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles cboSubTeam.KeyPress
        Dim KeyAscii As Short = Asc(eventArgs.KeyChar)

        If KeyAscii = 8 Then
            cboSubTeam.SelectedIndex = -1
        End If

        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
    End Sub

    Private Sub cboStore_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cboStore.SelectedIndexChanged

        If IsInitializing Then Exit Sub

        mlStoreID = VB6.GetItemData(cboStore, cboStore.SelectedIndex).ToString

        mbIsManufacturing = False

        Call LoadSubTeams()
        Call Changed()

    End Sub

    Private Sub cmdApply_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdApply.Click

        Call Save()

    End Sub

    Private Function Save() As Boolean

        'Test... UPGRADE_WARNING: Arrays in structure rsLocation may need to be initialized before they can be used. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="814DF224-76BD-4BB4-BFFB-EA359CB9FC48"'
        Dim rsLocation As DAO.Recordset = Nothing

        Save = False

        If mbChanged Then

            If Not ValidateData() Then Exit Function

            Try
                '-- Check to see if the location already exists.
                rsLocation = SQLOpenRecordSet("EXEC CheckForDuplicateInvLocation " & mlLocID & ", " & VB6.GetItemData(cboStore, cboStore.SelectedIndex) & ", " & VB6.GetItemData(cboSubTeam, cboSubTeam.SelectedIndex) & ", '" & Trim(txtLocName.Text) & "'", DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
                If rsLocation.Fields("Found").Value > 0 Then
                    Call MsgBox("A location already exists for this store, sub-team, and location name.", MsgBoxStyle.Exclamation, "Cannot save location!")
                    rsLocation.Close()
                    cboStore.Focus()
                    Exit Function
                End If
            Finally
                If rsLocation IsNot Nothing Then
                    rsLocation.Close()
                    rsLocation = Nothing
                End If
            End Try

            '--Update the record.
            SQLOpenRS(rsLocation, "EXEC UpdateInventoryLocation " & mlLocID & ", " & VB6.GetItemData(cboStore, cboStore.SelectedIndex) & ", " & VB6.GetItemData(cboSubTeam, cboSubTeam.SelectedIndex) & ", '" & Trim(txtLocName.Text) & "', '" & Trim(txtLocDesc.Text) & "', '" & Trim(txtNotes.Text) & "'", DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)

            mlLocID = rsLocation.Fields(0).Value

            Save = True

            'Lock the store and sub-team after a save is performed. They can't be changing this after entering items.
            Call SetActive(cboStore, False)
            Call SetActive(cboSubTeam, False)

            Call Changed(False)

        End If

    End Function

    Private Function GetSubTeamRestrictedFlag() As Boolean

        On Error GoTo ErrExit

        'Test... UPGRADE_WARNING: Couldn't resolve default property of object mvSubTeamList(). Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        GetSubTeamRestrictedFlag = IIf(mlSubTeamList(cboSubTeam.SelectedIndex) = 0, False, True) '1 indicates Un-Restricted.
        Exit Function

ErrExit:

        'If anything fails better safe to send back restricted.
        GetSubTeamRestrictedFlag = True

    End Function

    Private Function ValidateData(Optional ByRef bMsg As Boolean = True) As Boolean
        ValidateData = True
        If cboStore.SelectedIndex = -1 Or Trim(txtLocName.Text) = "" Or cboSubTeam.SelectedIndex = -1 Then
            ValidateData = False
            If bMsg Then
                MsgBox("Store, Sub-Team and Location Name are required.", MsgBoxStyle.Exclamation, "Missing Info!")
                If cboStore.SelectedIndex = -1 Then
                    cboStore.Focus()
                Else
                    txtLocName.Focus()
                End If
            End If
        ElseIf Not txtLocName.Text.All(Function(c) Char.IsLetter(c) OrElse Char.IsNumber(c) OrElse Char.IsWhiteSpace(c)) Then
            ValidateData = False
            MsgBox("Location Name must contain only letters, numbers, or whitespace.", MsgBoxStyle.Exclamation, "Invalid Location Name!")
            txtLocName.Focus()
        ElseIf Not txtLocDesc.Text.All(Function(c) Char.IsLetter(c) OrElse Char.IsNumber(c) OrElse Char.IsWhiteSpace(c)) Then
            ValidateData = False
            MsgBox("Location Desc must contain only letters, numbers, or whitespace.", MsgBoxStyle.Exclamation, "Invalid Location Desc!")
            txtLocDesc.Focus()
        End If
    End Function

    Private Sub cboSubTeam_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cboSubTeam.SelectedIndexChanged

        If IsInitializing Then Exit Sub

        mlSubTeamID = VB6.GetItemData(cboSubTeam, cboSubTeam.SelectedIndex)

        mbIsManufacturing = GetSubTeamRestrictedFlag()

        Call Changed()

    End Sub

    Private Sub txtLocDesc_TextChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles txtLocDesc.TextChanged

        If IsInitializing Then Exit Sub

        Call Changed()

    End Sub

    Private Sub txtLocDesc_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles txtLocDesc.KeyPress
        Dim KeyAscii As Short = Asc(eventArgs.KeyChar)

        KeyAscii = ValidateKeyPressEvent(KeyAscii, (txtLocDesc.Tag), txtLocDesc, 0, 0, 0)

        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
    End Sub

    Private Sub txtLocName_TextChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles txtLocName.TextChanged

        If IsInitializing Then Exit Sub

        Call Changed()

    End Sub

    Private Sub txtLocName_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles txtLocName.KeyPress
        Dim KeyAscii As Short = Asc(eventArgs.KeyChar)

        KeyAscii = ValidateKeyPressEvent(KeyAscii, (txtLocName.Tag), txtLocName, 0, 0, 0)

        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
    End Sub

    Private Sub txtNotes_TextChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles txtNotes.TextChanged

        If IsInitializing Then Exit Sub

        Call Changed()

    End Sub

    Private Sub Changed(Optional ByRef bChanged As Boolean = True)

        If mbLoading Or Not mbUserUpdates Then Exit Sub

        If bChanged = True Then
            cmdApply.Enabled = True
            mbChanged = True
        Else
            cmdApply.Enabled = False
            mbChanged = False
        End If

    End Sub

    Private Sub frmLocationEdit_FormClosed(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed

        mbChanged = False
        mbLoading = False
        mlLocID = 0
        mlStoreID = 0
        mlSubTeamID = 0
        mbIsManufacturing = False
        mbUserUpdates = False

    End Sub

    Private Sub txtNotes_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles txtNotes.KeyPress
        Dim KeyAscii As Short = Asc(eventArgs.KeyChar)

        KeyAscii = ValidateKeyPressEvent(KeyAscii, (txtNotes.Tag), txtNotes, 0, 0, 0)

        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
    End Sub
End Class