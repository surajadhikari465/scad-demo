
Public Class frmAvgCostAdjReason

#Region " Private Fields"

    Private IsLoading As Boolean

#End Region

#Region " Custom Events"

    Private Event ActiveFlagChanged()
    Private Event DescriptionChanged()

    Private Sub EnableReasonSave() Handles Me.DescriptionChanged

        If Me.txtDescription.Text.Length > 0 Then
            Me.cmdAdd.Enabled = True
        Else
            Me.cmdAdd.Enabled = False
        End If

    End Sub

    Private Sub EnableReasonEdit() Handles Me.ActiveFlagChanged

        If Me.cmbReason.SelectedIndex > -1 Then
            Me.cmdSave.Enabled = True
        Else
            Me.cmdSave.Enabled = False
        End If

    End Sub

#End Region

#Region " Form Events"

    Private Sub AvgCostAdjReason_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Me.RefreshData()

    End Sub

#End Region

#Region " Control Events"

    Private Sub chkActive_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkActive.CheckedChanged

        RaiseEvent ActiveFlagChanged()

    End Sub

    Private Sub txtDescription_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtDescription.TextChanged

        RaiseEvent DescriptionChanged()

    End Sub

    Private Sub cmbReason_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbReason.SelectedIndexChanged

        If Me.IsLoading Then Exit Sub

        If Me.cmbReason.SelectedIndex > -1 Then

            Try

                Me.chkActive.Checked = AvgCostAdjBO.IsAdjustmentReasonActive(CInt(Me.cmbReason.SelectedValue))

            Catch ex As Exception

                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error)
                Me.Close()

            End Try

        End If

    End Sub

    Private Sub cmdAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAdd.Click

        Try

            If ValidateData() Then

                AvgCostAdjBO.AddAdjustmentReason(Me.txtDescription.Text, True)

                Me.RefreshData()

                MessageBox.Show("The adjustment reason '" & Me.txtDescription.Text & "' has been added.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)

                Me.ClearForm()

            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click

        Try

            AvgCostAdjBO.SetReasonStatus(CInt(Me.cmbReason.SelectedValue), Me.chkActive.Checked)

            MessageBox.Show("'" & Me.cmbReason.Text & "' status changed.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)

            Me.ClearForm()

        Catch ex As Exception

            MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub txtDescription_Validated(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDescription.Validated

        Me.frmErrorProvider.SetError(Me.txtDescription, "")

    End Sub

#End Region

#Region " Private Methods"

    Private Function ValidateData() As Boolean

        ValidateData = True

        If Me.txtDescription.Text.Length = 0 Then
            Me.frmErrorProvider.SetError(Me.txtDescription, "Required")
            ValidateData = False
        ElseIf Me.cmbReason.FindStringExact(Me.txtDescription.Text) > -1 Then
            Me.frmErrorProvider.SetError(Me.txtDescription, "Reason already exists.")
            ValidateData = False
        End If

    End Function

    Private Sub RefreshData()

        Me.IsLoading = True

        Try

            Me.cmbReason.DataSource = AvgCostAdjBO.GetAdjustmentReasons(False, Nothing)
            Me.cmbReason.DisplayMember = "Description"
            Me.cmbReason.ValueMember = "ID"
            Me.cmbReason.SelectedIndex = -1

        Catch ex As Exception

            MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error)
            Me.Close()

        End Try

        Me.IsLoading = False

    End Sub

    Private Sub ClearForm()

        Me.cmbReason.SelectedIndex = -1
        Me.chkActive.Checked = False
        Me.txtDescription.Text = String.Empty
        Me.txtDescription.Focus()

    End Sub


#End Region

End Class