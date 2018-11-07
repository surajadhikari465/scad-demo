Imports WholeFoods.IRMA.ItemHosting.BusinessLogic
Imports WholeFoods.IRMA.ItemHosting.DataAccess

Public Class Form_Tare
    Private hasChanged As Boolean
    Private FormClosingNoSave As Boolean

    Private Sub Form_Tare_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If hasChanged Then
            'prompt the user to save changes
            Dim result As DialogResult = MessageBox.Show(ResourcesCommon.GetString("msg_confirmSaveData"), Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question)

            If result = Windows.Forms.DialogResult.Yes Then
                If Not ApplyChanges() Then
                    e.Cancel = True
                End If
            Else
                FormClosingNoSave = True
            End If
        End If
    End Sub

    Private Sub Form_Tare_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        LoadCombo()
        hasChanged = False
        FormClosingNoSave = False
    End Sub

    Private Sub LoadCombo()
        DescriptionCombo.DataSource = ScaleTareDAO.GetComboList()
        If DescriptionCombo.Items.Count > 0 Then
            DescriptionCombo.DisplayMember = "Description"
            DescriptionCombo.ValueMember = "ID"
            DescriptionCombo.SelectedIndex = 0
        End If
    End Sub

    Private Sub DescriptionCombo_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DescriptionCombo.SelectedIndexChanged
        If DescriptionCombo.SelectedIndex > -1 Then
            Dim scaleTare As New ScaleTareBO()
            scaleTare = CType(DescriptionCombo.SelectedItem, ScaleTareBO)

            DescriptionTextbox.Text = scaleTare.Description
            Zone1NumericEditor.Value = scaleTare.Zone1
            Zone2NumericEditor.Value = scaleTare.Zone2
            Zone3NumericEditor.Value = scaleTare.Zone3
            Zone4NumericEditor.Value = scaleTare.Zone4
            Zone5NumericEditor.Value = scaleTare.Zone5
            Zone6NumericEditor.Value = scaleTare.Zone6
            Zone7NumericEditor.Value = scaleTare.Zone7
            Zone8NumericEditor.Value = scaleTare.Zone8
            Zone9NumericEditor.Value = scaleTare.Zone9
            Zone10NumericEditor.Value = scaleTare.Zone10

            hasChanged = False
        End If
    End Sub

    Private Sub Clear()
        DescriptionCombo.SelectedIndex = -1
        DescriptionTextbox.Text = String.Empty
        Zone1NumericEditor.Value = 0
        Zone2NumericEditor.Value = 0
        Zone3NumericEditor.Value = 0
        Zone4NumericEditor.Value = 0
        Zone5NumericEditor.Value = 0
        Zone6NumericEditor.Value = 0
        Zone7NumericEditor.Value = 0
        Zone8NumericEditor.Value = 0
        Zone9NumericEditor.Value = 0
        Zone10NumericEditor.Value = 0
        DescriptionTextbox.Focus()
        hasChanged = False
    End Sub
    Private Sub ApplyButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ApplyButton.Click
        ApplyChanges()
    End Sub

    Private Sub DeleteButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DeleteButton.Click
        If DescriptionCombo.SelectedIndex > -1 Then
            ScaleTareDAO.Delete(CInt(DescriptionCombo.SelectedValue.ToString()))
            ' reload the combo
            LoadCombo()
        Else
            ' simply clear the controls if the last item selected has not been saved
            Clear()
        End If
        hasChanged = False
    End Sub

    Private Sub AddButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddButton.Click
        If hasChanged Then
            'prompt the user to save changes
            Dim result As DialogResult = MessageBox.Show(ResourcesCommon.GetString("msg_confirmSaveData"), Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question)

            If result = Windows.Forms.DialogResult.Yes Then
                If ApplyChanges() Then
                    Clear()
                End If
            End If
        Else
            Clear()
        End If
    End Sub

    Private Sub DescriptionTextbox_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DescriptionTextbox.TextChanged
        hasChanged = True
    End Sub

    
    Private Function ApplyChanges() As Boolean
        ApplyChanges = True

        If DescriptionTextbox.Text.Length = 0 Then
            MsgBox(String.Format(ResourcesIRMA.GetString("Required"), DescriptionLabel.Text.Replace(":", "")), MsgBoxStyle.Critical, Me.Text)
            ApplyChanges = False
            Exit Function
        End If

        Dim scaleTare As New ScaleTareBO()

        scaleTare.Description = DescriptionTextbox.Text
        If DescriptionCombo.SelectedIndex > -1 Then scaleTare.ID = CInt(DescriptionCombo.SelectedValue.ToString())

        If Not ValidateTareValues() Then
            ApplyChanges = False
            Exit Function
        End If

        scaleTare.Zone1 = CDec(Zone1NumericEditor.Value.ToString())
        scaleTare.Zone2 = CDec(Zone2NumericEditor.Value.ToString())
        scaleTare.Zone3 = CDec(Zone3NumericEditor.Value.ToString())
        scaleTare.Zone4 = CDec(Zone4NumericEditor.Value.ToString())
        scaleTare.Zone5 = CDec(Zone5NumericEditor.Value.ToString())
        scaleTare.Zone6 = CDec(Zone6NumericEditor.Value.ToString())
        scaleTare.Zone7 = CDec(Zone7NumericEditor.Value.ToString())
        scaleTare.Zone8 = CDec(Zone8NumericEditor.Value.ToString())
        scaleTare.Zone9 = CDec(Zone9NumericEditor.Value.ToString())
        scaleTare.Zone10 = CDec(Zone10NumericEditor.Value.ToString())

        If ScaleTareDAO.Save(scaleTare) Then
            ' reload the combo
            LoadCombo()
            ' select the most recently added/edited item
            If DescriptionCombo.Items.Count > 0 Then
                Dim index As Integer
                For Each item As ScaleTareBO In DescriptionCombo.Items
                    If item.Description = scaleTare.Description Then
                        DescriptionCombo.SelectedIndex = index
                        Exit For
                    End If
                    index = index + 1
                Next
            End If
        Else
            ' Indicate that this is a duplicate
            MsgBox(String.Format(ResourcesIRMA.GetString("Duplicate"), DescriptionTextbox.Text), MsgBoxStyle.Critical, Me.Text)
            Clear()
            ApplyChanges = False
        End If

        scaleTare = Nothing
        hasChanged = False

    End Function

    Private Sub CloseButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CloseButton.Click
        Me.Close()
    End Sub

    Private Sub NumericEditor_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Zone10NumericEditor.ValueChanged, Zone9NumericEditor.ValueChanged, Zone8NumericEditor.ValueChanged, Zone7NumericEditor.ValueChanged, Zone6NumericEditor.ValueChanged, Zone5NumericEditor.ValueChanged, Zone4NumericEditor.ValueChanged, Zone3NumericEditor.ValueChanged, Zone2NumericEditor.ValueChanged, Zone1NumericEditor.ValueChanged
        hasChanged = True
    End Sub
    Private Sub NumericEditor_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Zone10NumericEditor.Click, Zone9NumericEditor.Click, Zone8NumericEditor.Click, Zone7NumericEditor.Click, Zone6NumericEditor.Click, Zone5NumericEditor.Click, Zone4NumericEditor.Click, Zone3NumericEditor.Click, Zone2NumericEditor.Click, Zone1NumericEditor.Click
        CType(sender, Infragistics.Win.UltraWinEditors.UltraNumericEditor).SelectAll()
    End Sub
    Private Sub NumericEditor_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles Zone10NumericEditor.Enter, Zone9NumericEditor.Enter, Zone8NumericEditor.Enter, Zone7NumericEditor.Enter, Zone6NumericEditor.Enter, Zone5NumericEditor.Enter, Zone4NumericEditor.Enter, Zone3NumericEditor.Enter, Zone2NumericEditor.Enter, Zone1NumericEditor.Enter
        CType(sender, Infragistics.Win.UltraWinEditors.UltraNumericEditor).SelectAll()
    End Sub
    Private Function ValidateTareValues() As Boolean
        ValidateTareValues = True

        If Not ValidateTare(Zone1NumericEditor.Value.ToString()) Then
            ValidateTareValues = False
            Zone1NumericEditor.Focus()
            Exit Function
        End If
        If Not ValidateTare(Zone2NumericEditor.Value.ToString()) Then
            ValidateTareValues = False
            Zone2NumericEditor.Focus()
            Exit Function
        End If
        If Not ValidateTare(Zone3NumericEditor.Value.ToString()) Then
            ValidateTareValues = False
            Zone3NumericEditor.Focus()
            Exit Function
        End If
        If Not ValidateTare(Zone4NumericEditor.Value.ToString()) Then
            ValidateTareValues = False
            Zone4NumericEditor.Focus()
            Exit Function
        End If
        If Not ValidateTare(Zone5NumericEditor.Value.ToString()) Then
            ValidateTareValues = False
            Zone5NumericEditor.Focus()
            Exit Function
        End If
        If Not ValidateTare(Zone6NumericEditor.Value.ToString()) Then
            ValidateTareValues = False
            Zone6NumericEditor.Focus()
            Exit Function
        End If
        If Not ValidateTare(Zone7NumericEditor.Value.ToString()) Then
            ValidateTareValues = False
            Zone7NumericEditor.Focus()
            Exit Function
        End If
        If Not ValidateTare(Zone8NumericEditor.Value.ToString()) Then
            ValidateTareValues = False
            Zone8NumericEditor.Focus()
            Exit Function
        End If
        If Not ValidateTare(Zone9NumericEditor.Value.ToString()) Then
            ValidateTareValues = False
            Zone9NumericEditor.Focus()
            Exit Function
        End If
        If Not ValidateTare(Zone10NumericEditor.Value.ToString()) Then
            ValidateTareValues = False
            Zone10NumericEditor.Focus()
            Exit Function
        End If
    End Function
    Private Function ValidateTare(ByVal TareValue As String) As Boolean
        Dim DecTareValue As Decimal
        ValidateTare = True

        If Not FormClosingNoSave Then
            DecTareValue = CDec(TareValue)

            If DecTareValue > 1.0 AndAlso DecTareValue <> 9.999 Then
                MsgBox(String.Format(ResourcesIRMA.GetString("msg_error_InvalidTareValue"), DescriptionLabel.Text.Replace(":", "")), MsgBoxStyle.Critical, Me.Text)
                ValidateTare = False
            End If
        End If
    End Function
End Class