Imports WholeFoods.IRMA.ItemHosting.BusinessLogic
Imports WholeFoods.IRMA.ItemHosting.DataAccess
Imports System.Text
Imports WholeFoods.IRMA.Common.DataAccess

Public Class ExtraTextNew

    Private _newScaleBO As ScaleExtraTextBO
    Private _enableReturnsInExtraText As Boolean

    Public Property NewExtraTextBO() As ScaleExtraTextBO
        Get
            Return Me._newScaleBO
        End Get
        Set(ByVal value As ScaleExtraTextBO)
            Me._newScaleBO = value
        End Set
    End Property

    Public Property LinkBackToIdentifier() As Boolean
        Get
            Return Me._chkLinkBack.Checked
        End Get
        Set(ByVal value As Boolean)
            Me._chkLinkBack.Checked = value
        End Set
    End Property

    Public WriteOnly Property ShowLinkCheckBox() As Boolean
        Set(ByVal value As Boolean)
            Me._chkLinkBack.Visible = value
        End Set
    End Property

    Private Sub ExtraTextNew_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ' load the label type drop-down
        Me.cmbLabelType.DataSource = ScaleLabelTypeDAO.GetComboList()

        If Me.cmbLabelType.Items.Count > 0 Then
            Me.cmbLabelType.DisplayMember = "Description"
            Me.cmbLabelType.ValueMember = "ID"
            Me.cmbLabelType.SelectedIndex = -1
        End If

        Me._enableReturnsInExtraText = InstanceDataDAO.IsFlagActive("EnableReturnsInExtraTextAndStorageData")
        Me.txtExtraText.AcceptsReturn = Me._enableReturnsInExtraText

        Me.txtDescription.Focus()

    End Sub

    Private Sub Clear()

        ' clear out all the existing values
        Me.cmbLabelType.SelectedIndex = -1
        Me.txtExtraText.Text = String.Empty
        Me.txtDescription.Text = String.Empty
        Me.txtDescription.Focus()

    End Sub

    Private Sub btnClearForm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClearForm.Click

        Me.Clear()

    End Sub

    Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub CloseButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddRecord.Click

        If ValidateOnSave() Then

            Dim newID As Integer = 0

            Dim _newExtraTextBO As New ScaleExtraTextBO

            _newExtraTextBO.Scale_LabelType_ID = CInt(Me.cmbLabelType.SelectedValue)
            _newExtraTextBO.Description = Me.txtDescription.Text
            _newExtraTextBO.ExtraText = Me.txtExtraText.Text

            Me.NewExtraTextBO = _newExtraTextBO

            newID = ScaleExtraTextDAO.Add(Me.NewExtraTextBO)

            If newID > 0 Then

                If Not Me._chkLinkBack.Checked Then
                    Me.DialogResult = Windows.Forms.DialogResult.Ignore
                Else
                    Me.NewExtraTextBO.ID = newID
                    Me.DialogResult = Windows.Forms.DialogResult.OK
                End If

            Else
                ' Indicate that this is a duplicate
                MsgBox(String.Format(ResourcesIRMA.GetString("Duplicate"), Me.txtDescription), MsgBoxStyle.Critical, Me.Text)
                Clear()
            End If

        End If

    End Sub

    Private Function ValidateOnSave() As Boolean

        Me.frmErrorProvider.Clear()

        Dim _isValid As Boolean = True

        If Me.cmbLabelType.SelectedIndex = -1 Then
            Me.frmErrorProvider.SetError(Me.cmbLabelType, "Label Type selection required.")
            _isValid = False
        End If

        If Me.txtExtraText.Text.Length = 0 Then
            Me.frmErrorProvider.SetError(Me.txtExtraText, "Enter the Extra Text in the box provided.")
            _isValid = False
        End If

        If Me.txtDescription.Text.Length = 0 Then
            Me.frmErrorProvider.SetError(Me.txtDescription, "Enter the Extra Text Description in the box provided.")
            _isValid = False
        End If

        Return _isValid

    End Function

    Private Sub txtExtraText_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtExtraText.TextChanged
        If Not Me._enableReturnsInExtraText Then
            'Prevent users from pasting in TABs and CR/LF.
            txtExtraText.Text = txtExtraText.Text.Replace(Convert.ToChar(9), "")
            txtExtraText.Text = txtExtraText.Text.Replace(Convert.ToChar(10), "")
            txtExtraText.Text = txtExtraText.Text.Replace(Convert.ToChar(13), "")
        End If
    End Sub

End Class