Imports WholeFoods.IRMA.ItemHosting.BusinessLogic
Imports WholeFoods.IRMA.ItemHosting.DataAccess

Public Class Form_LabelType
    Private hasChanged As Boolean

    Private Sub Form_LabelType_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If hasChanged Then
            'prompt the user to save changes
            Dim result As DialogResult = MessageBox.Show(ResourcesCommon.GetString("msg_confirmSaveData"), Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question)

            If result = Windows.Forms.DialogResult.Yes Then
                ApplyChanges()
            End If
        End If


    End Sub

    Private Sub Form_LabelType_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        LoadCombo()
        hasChanged = False

    End Sub
    Private Sub LoadCombo()
        DescriptionCombo.DataSource = ScaleLabelTypeDAO.GetComboList()
        If DescriptionCombo.Items.Count > 0 Then

            Dim scaleLabelType As New ScaleLabelTypeBO()
            scaleLabelType = CType(DescriptionCombo.SelectedItem, ScaleLabelTypeBO)

            DescriptionCombo.DisplayMember = "Description"
            DescriptionCombo.ValueMember = "ID"
            DescriptionCombo.SelectedIndex = 0
            DescriptionTextbox.Text = scaleLabelType.Description
            CharsPerLineNumericEditor.Value = scaleLabelType.CharsPerLine
            LinesPerLabelNumericEditor.Value = scaleLabelType.LinesPerLabel
        Else
            Clear()
        End If

    End Sub

    Private Sub DescriptionCombo_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DescriptionCombo.SelectedIndexChanged
        If DescriptionCombo.SelectedIndex > -1 Then
            Dim scaleLabelType As New ScaleLabelTypeBO()
            scaleLabelType = CType(DescriptionCombo.SelectedItem, ScaleLabelTypeBO)
            DescriptionTextbox.Text = scaleLabelType.Description
            CharsPerLineNumericEditor.Value = scaleLabelType.CharsPerLine
            LinesPerLabelNumericEditor.Value = scaleLabelType.LinesPerLabel
            hasChanged = False
        End If

    End Sub

    Private Sub ApplyButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ApplyButton.Click
        ApplyChanges()

    End Sub

    Private Sub DeleteButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DeleteButton.Click
        If DescriptionCombo.SelectedIndex > -1 Then
            ScaleLabelTypeDAO.Delete(CInt(DescriptionCombo.SelectedValue.ToString()))
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
                ApplyChanges()
            End If
        End If
        Clear()

    End Sub
    Private Sub Clear()
        DescriptionCombo.SelectedIndex = -1
        DescriptionTextbox.Text = String.Empty
        CharsPerLineNumericEditor.Value = 1
        LinesPerLabelNumericEditor.Value = 1
        DescriptionTextbox.Focus()
        hasChanged = False

    End Sub

    Private Sub DescriptionTextbox_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DescriptionTextbox.TextChanged
        hasChanged = True

    End Sub

  
    Private Sub ApplyChanges()
        If DescriptionTextbox.Text.Length = 0 Then
            MsgBox(String.Format(ResourcesIRMA.GetString("Required"), DescriptionLabel.Text.Replace(":", "")), MsgBoxStyle.Critical, Me.Text)
            Exit Sub
        End If

        Dim scaleLabelType As New ScaleLabelTypeBO()
        scaleLabelType.Description = DescriptionTextbox.Text
        scaleLabelType.CharsPerLine = CInt(CharsPerLineNumericEditor.Value.ToString())
        scaleLabelType.LinesPerLabel = CInt(LinesPerLabelNumericEditor.Value.ToString())
        If DescriptionCombo.SelectedIndex > -1 Then scaleLabelType.ID = CInt(DescriptionCombo.SelectedValue.ToString())
        If ScaleLabelTypeDAO.Save(scaleLabelType) Then
            ' reload the combo
            LoadCombo()
            ' select the most recently added/edited item
            If DescriptionCombo.Items.Count > 0 Then
                Dim index As Integer
                For Each item As ScaleLabelTypeBO In DescriptionCombo.Items
                    If item.Description = scaleLabelType.Description Then
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
        End If

        scaleLabelType = Nothing
        hasChanged = False

    End Sub

    Private Sub CloseButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CloseButton.Click
        Me.Close()

    End Sub

    Private Sub NumericEditor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LinesPerLabelNumericEditor.Click, CharsPerLineNumericEditor.Click
        CType(sender, Infragistics.Win.UltraWinEditors.UltraNumericEditor).SelectAll()
    End Sub
    Private Sub NumericEditor_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CharsPerLineNumericEditor.ValueChanged, LinesPerLabelNumericEditor.ValueChanged
        hasChanged = True
    End Sub
End Class