Imports WholeFoods.IRMA.ItemHosting.BusinessLogic
Imports WholeFoods.IRMA.ItemHosting.DataAccess

Public Class Form_Grade
    Private hasChanged As Boolean

    Private Sub Form_Grade_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If hasChanged Then
            'prompt the user to save changes
            Dim result As DialogResult = MessageBox.Show(ResourcesCommon.GetString("msg_confirmSaveData"), Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question)

            If result = Windows.Forms.DialogResult.Yes Then
                ApplyChanges()
            End If
        End If

    End Sub

    Private Sub Form_Grade_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        LoadCombo()
        hasChanged = False


    End Sub
    Private Sub LoadCombo()
        DescriptionCombo.DataSource = ScaleGradeDAO.GetComboList()
        If DescriptionCombo.Items.Count > 0 Then
            DescriptionCombo.DisplayMember = "Description"
            DescriptionCombo.ValueMember = "ID"
            DescriptionCombo.SelectedIndex = 0
        End If

    End Sub

    Private Sub DescriptionCombo_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DescriptionCombo.SelectedIndexChanged
        If DescriptionCombo.SelectedIndex > -1 Then
            Dim scaleGrade As New ScaleGradeBO()
            scaleGrade = CType(DescriptionCombo.SelectedItem, ScaleGradeBO)

            DescriptionTextbox.Text = scaleGrade.Description
            Zone1NumericEditor.Value = scaleGrade.Zone1
            Zone2NumericEditor.Value = scaleGrade.Zone2
            Zone3NumericEditor.Value = scaleGrade.Zone3
            Zone4NumericEditor.Value = scaleGrade.Zone4
            Zone5NumericEditor.Value = scaleGrade.Zone5
            Zone6NumericEditor.Value = scaleGrade.Zone6
            Zone7NumericEditor.Value = scaleGrade.Zone7
            Zone8NumericEditor.Value = scaleGrade.Zone8
            Zone9NumericEditor.Value = scaleGrade.Zone9
            Zone10NumericEditor.Value = scaleGrade.Zone10

            hasChanged = False
        End If

    End Sub

    Private Sub DescriptionTextbox_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DescriptionTextbox.TextChanged
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
    Private Sub DeleteButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DeleteButton.Click
        If DescriptionCombo.SelectedIndex > -1 Then
            ScaleGradeDAO.Delete(CInt(DescriptionCombo.SelectedValue.ToString()))
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

        Dim scaleGrade As New ScaleGradeBO()
        scaleGrade.Description = DescriptionTextbox.Text
        If DescriptionCombo.SelectedIndex > -1 Then scaleGrade.ID = CInt(DescriptionCombo.SelectedValue.ToString())
        scaleGrade.Zone1 = CInt(Zone1NumericEditor.Value.ToString())
        scaleGrade.Zone2 = CInt(Zone2NumericEditor.Value.ToString())
        scaleGrade.Zone3 = CInt(Zone3NumericEditor.Value.ToString())
        scaleGrade.Zone4 = CInt(Zone4NumericEditor.Value.ToString())
        scaleGrade.Zone5 = CInt(Zone5NumericEditor.Value.ToString())
        scaleGrade.Zone6 = CInt(Zone6NumericEditor.Value.ToString())
        scaleGrade.Zone7 = CInt(Zone7NumericEditor.Value.ToString())
        scaleGrade.Zone8 = CInt(Zone8NumericEditor.Value.ToString())
        scaleGrade.Zone9 = CInt(Zone9NumericEditor.Value.ToString())
        scaleGrade.Zone10 = CInt(Zone10NumericEditor.Value.ToString())

        If ScaleGradeDAO.Save(scaleGrade) Then
            ' reload the combo
            LoadCombo()
            ' select the most recently added/edited item
            If DescriptionCombo.Items.Count > 0 Then
                Dim index As Integer
                For Each item As ScaleGradeBO In DescriptionCombo.Items
                    If item.Description = scaleGrade.Description Then
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

        scaleGrade = Nothing
        hasChanged = False

    End Sub

    Private Sub NumericEditor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Zone9NumericEditor.Click, Zone8NumericEditor.Click, Zone7NumericEditor.Click, Zone6NumericEditor.Click, Zone5NumericEditor.Click, Zone4NumericEditor.Click, Zone3NumericEditor.Click, Zone2NumericEditor.Click, Zone1NumericEditor.Click, Zone10NumericEditor.Click
        CType(sender, Infragistics.Win.UltraWinEditors.UltraNumericEditor).SelectAll()
    End Sub
    Private Sub NumericEditor_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Zone1NumericEditor.ValueChanged, Zone9NumericEditor.ValueChanged, Zone8NumericEditor.ValueChanged, Zone7NumericEditor.ValueChanged, Zone6NumericEditor.ValueChanged, Zone5NumericEditor.ValueChanged, Zone4NumericEditor.ValueChanged, Zone3NumericEditor.ValueChanged, Zone2NumericEditor.ValueChanged, Zone10NumericEditor.ValueChanged
        hasChanged = True
    End Sub
End Class