Imports System.Text
Imports WholeFoods.IRMA.Planogram.BusinessLogic
Imports WholeFoods.IRMA.Planogram.DataAccess
Imports WholeFoods.Utility

Public Class Form_PlanogramImport
    Private selectedFile As String

    Private Sub CloseButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CloseButton.Click
        Me.Close()

    End Sub

    Private Sub btnSelectFile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSelectFile.Click

        txtFile.Text = Nothing

        selectFileDialog.InitialDirectory = ConfigurationServices.AppSettings("PlanogramLocalDir")
        selectFileDialog.ShowDialog()
        selectedFile = selectFileDialog.FileName()

        If Not String.IsNullOrEmpty(selectedFile) Then
            txtFile.Text = CStr(StripFile(selectedFile))
            Dim lastIndex As Integer = selectedFile.LastIndexOf(".")
            Dim extension As String = Mid$(selectedFile, lastIndex + 2, Len(selectedFile))
            If Not StrComp(extension, "dat", CompareMethod.Text) = 0 Then
                ToggleImportButtons(False)
                MsgBox("Only data files (.dat) can be uploaded!", MsgBoxStyle.OkOnly, Me.Text)
            Else
                ToggleImportButtons(True)
            End If
        Else
            ToggleImportButtons(False)
        End If

        Me.Enabled = True

    End Sub

    Private Sub ToggleImportButtons(ByVal canImport As Boolean)
        ImportButton.Enabled = canImport
        txtFile.Enabled = Not (canImport)
    End Sub

    Private Function StripFile(ByVal sFullFileName As String) As String
        Dim iLastPos As Integer, iPos As Integer
        iPos = InStr(1, sFullFileName, "\")
        Do Until iPos = 0
            iLastPos = iPos
            iPos = InStr(iPos + 1, sFullFileName, "\")
        Loop
        StripFile = Mid$(sFullFileName, iLastPos + 1, Len(sFullFileName) - iLastPos)
    End Function

    Private Sub ImportButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ImportButton.Click
        Dim planogramBO As PlanogramBO
        Dim oldCursor As Cursor = Me.Cursor

        Me.Enabled = False
        Me.Refresh()

        Me.Cursor = Cursors.WaitCursor
        planogramBO = New PlanogramBO
        planogramBO.ImportFile(selectedFile)
        MessageBox.Show(ResourcesPlanogram.GetString("Message"), Me.Text, MessageBoxButtons.OK, MessageBoxIcon.None)
        ToggleImportButtons(False)
        Me.Cursor = oldCursor

        Me.Enabled = True

    End Sub

    Private Sub CreateButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CreateButton.Click
        Dim lItemKey() As Integer
        Dim storeNo As Integer
        Dim subteamNo As Integer = 0
        Dim isPlanogramReg As Boolean

        If StoreComboBox.SelectedIndex = -1 Then
            ' notify the user that the store is required
            MessageBox.Show(String.Format(ResourcesIRMA.GetString("Required"), StoreLabel.Text.Replace(":", "")), Me.Text, MessageBoxButtons.OK)
            Exit Sub
        End If
        storeNo = VB6.GetItemData(StoreComboBox, StoreComboBox.SelectedIndex)
        If SubteamComboBox.SelectedIndex > -1 Then
            subteamNo = VB6.GetItemData(SubteamComboBox, SubteamComboBox.SelectedIndex)
        End If

        If Not (RegularRadioButton.Checked Or NonRegularRadioButton.Checked) Then
            ' notify the user that this is required
            MessageBox.Show(String.Format(ResourcesIRMA.GetString("Required"), RegularRadioButton.Text.Replace(":", "") + " OR " + NonRegularRadioButton.Text.Replace(":", "")), Me.Text, MessageBoxButtons.OK)
            Exit Sub
        End If
        If RegularRadioButton.Checked Then
            ' Print shelf tags for regular tag type for entire store
            isPlanogramReg = True
        ElseIf NonRegularRadioButton.Checked Then
            If dtpStartDate.Value Is Nothing Then
                MessageBox.Show(String.Format(ResourcesIRMA.GetString("Required"), DateLabel.Text.Replace(":", "")), Me.Text, MessageBoxButtons.OK)
                Exit Sub
            ElseIf CDate(dtpStartDate.Value) < Now Then
                ' can't have a date in the past
                MessageBox.Show(String.Format(ResourcesPlanogram.GetString("StartDatePast"), CDate(dtpStartDate.Value).ToString("M/dd/yyyy"), Me.Text, MessageBoxButtons.OK))
                Exit Sub
            End If
            ' Print shelf tags for non-regular tag types (sale, etc) for refresh
            isPlanogramReg = False
        End If

        lItemKey = PlanogramDAO.GetPlanogramItems(storeNo, subteamNo, GetSetList(), isPlanogramReg, CDate(dtpStartDate.Value))

        If lItemKey.Length > 0 Then
            frmPricingPrintSignInfo.SetPlanogramSignInfo(lItemKey, CType("|", Char), storeNo, isPlanogramReg, CDate(dtpStartDate.Value))
            frmPricingPrintSignInfo.ShowDialog()
            frmPricingPrintSignInfo.Dispose()
        Else
            MessageBox.Show(ResourcesIRMA.GetString("NoneFound"), Me.Text, MessageBoxButtons.OK)
        End If

    End Sub

    Private Function GetSetList() As String
        Dim list As New StringBuilder()
        Dim setBO As PlanogramSetBO

        If SetListBox.SelectedItems.Count > 0 Then
            For Each setBO In SetListBox.SelectedItems
                list.Append(setBO.Description).Append("|")
            Next
        End If

        Return list.ToString()
    End Function

    Private Sub Form_PlanogramImport_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ' Load stores combo box
        LoadRetailStore(StoreComboBox)
        LoadSubTeam(SubteamComboBox)
        StoreComboBox.SelectedIndex = -1
        SubteamComboBox.SelectedIndex = -1

    End Sub

    Private Sub StoreComboBox_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles StoreComboBox.SelectedIndexChanged
        If StoreComboBox.SelectedIndex > -1 Then
            LoadSetList()
        Else
            SetListBox.DataSource = Nothing
        End If

    End Sub

    Private Sub LoadSetList()
        Dim storeNo As Integer = 0
        Dim subTeamNo As Integer = 0

        ' Don't load this until they select a store
        If StoreComboBox.SelectedIndex = -1 Then Exit Sub

        If SubteamComboBox.SelectedIndex > -1 Then subTeamNo = VB6.GetItemData(SubteamComboBox, SubteamComboBox.SelectedIndex)
        storeNo = VB6.GetItemData(StoreComboBox, StoreComboBox.SelectedIndex)

        SetListBox.DataSource = PlanogramDAO.GetSetNumbersComboList(storeNo, subTeamNo)
        If SetListBox.Items.Count > 0 Then
            SetListBox.DisplayMember = "Description"
            SetListBox.ValueMember = "Description"
        End If
        SetListBox.SelectedIndex = -1
    End Sub

    Private Sub SubteamComboBox_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles SubteamComboBox.KeyPress
        Dim KeyAscii As Integer = Asc(e.KeyChar)

        If KeyAscii = 8 Then
            SubteamComboBox.SelectedIndex = -1
        End If

        e.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then e.Handled = True

    End Sub

    Private Sub SubteamComboBox_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles SubteamComboBox.SelectedIndexChanged
        LoadSetList()
    End Sub

    Private Sub NonRegularRadioButton_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NonRegularRadioButton.CheckedChanged
        If NonRegularRadioButton.Checked = True Then
            dtpStartDate.Enabled = True
        Else
            dtpStartDate.Enabled = False
        End If
    End Sub
End Class