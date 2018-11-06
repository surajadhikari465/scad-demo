Imports WholeFoods.IRMA.ItemHosting.BusinessLogic
Imports WholeFoods.IRMA.ItemHosting.DataAccess

Public Class Form_LabelFormat
    Private hasChanged As Boolean

    Private Sub Form_LabelFormat_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If hasChanged Then
            'prompt the user to save changes
            Dim result As DialogResult = MessageBox.Show(ResourcesCommon.GetString("msg_confirmSaveData"), Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question)

            If result = Windows.Forms.DialogResult.Yes Then
                ApplyChanges()
            End If
        End If

    End Sub

    Private Sub Form_LabelFormat_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        LoadCombo()
        hasChanged = False
    End Sub

    Private Sub LoadCombo()
        DescriptionCombo.DataSource = ScaleLabelFormatDAO.GetComboList()
        If DescriptionCombo.Items.Count > 0 Then
            DescriptionCombo.DisplayMember = "Description"
            DescriptionCombo.ValueMember = "ID"
            DescriptionCombo.SelectedIndex = 0
            DescriptionTextBox.Text = CType(DescriptionCombo.SelectedItem, ScaleLabelFormatBO).Description
        End If

    End Sub

    Private Sub DescriptionCombo_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DescriptionCombo.SelectedIndexChanged
        If DescriptionCombo.SelectedIndex > -1 Then
            DescriptionTextBox.Text = CType(DescriptionCombo.SelectedItem, ScaleLabelFormatBO).Description
            hasChanged = False
        End If

    End Sub

    Private Sub DescriptionTextBox_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DescriptionTextBox.TextChanged
        hasChanged = True

    End Sub

    Private Sub AddButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddButton.Click
        If hasChanged Then
            'prompt the user to save changes
            Dim result As DialogResult = MessageBox.Show(ResourcesCommon.GetString("msg_confirmSaveData"), Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question)

            If result = Windows.Forms.DialogResult.Yes Then
                ApplyChanges()
            End If
        End If
        Clear()

    End Sub

    Private Sub Clear()
        DescriptionCombo.SelectedIndex = -1
        DescriptionTextbox.Text = String.Empty
        DescriptionTextbox.Focus()
        hasChanged = False

    End Sub

    Private Sub DeleteButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DeleteButton.Click
        If DescriptionCombo.SelectedIndex > -1 Then
            ScaleLabelFormatDAO.Delete(CInt(DescriptionCombo.SelectedValue.ToString()))
            ' reload the combo
            LoadCombo()
        Else
            ' simply clear the controls if the last item selected has not been saved
            Clear()
        End If
        hasChanged = False


    End Sub

    Private Sub ApplyButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ApplyButton.Click
        ApplyChanges()

    End Sub

    Private Sub CloseButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CloseButton.Click
        Me.Close()

    End Sub
    Private Sub ApplyChanges()
        If DescriptionTextbox.Text.Length = 0 Then
            MsgBox(String.Format(ResourcesIRMA.GetString("Required"), DescriptionLabel.Text.Replace(":", "")), MsgBoxStyle.Critical, Me.Text)
            Exit Sub
        End If

        Dim scaleLabelFormat As New ScaleLabelFormatBO()
        scaleLabelFormat.Description = DescriptionTextBox.Text
        If DescriptionCombo.SelectedIndex > -1 Then scaleLabelFormat.ID = CInt(DescriptionCombo.SelectedValue.ToString())
        If ScaleLabelFormatDAO.Save(scaleLabelFormat) Then
            ' reload the combo
            LoadCombo()
            ' select the most recently added/edited item
            If DescriptionCombo.Items.Count > 0 Then
                Dim index As Integer
                For Each item As ScaleLabelFormatBO In DescriptionCombo.Items
                    If item.Description = scaleLabelFormat.Description Then
                        DescriptionCombo.SelectedIndex = index
                        Exit For
                    End If
                    index = index + 1
                Next
            End If
        Else
            ' Indicate that this is a duplicate
            MsgBox(String.Format(ResourcesIRMA.GetString("Duplicate"), DescriptionTextBox.Text), MsgBoxStyle.Critical, Me.Text)
            Clear()
        End If

        scaleLabelFormat = Nothing
        hasChanged = False

    End Sub
End Class