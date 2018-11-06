Public Class VendorContactEmail

    Private _emailAddress As String
    Public Property EmailAddess As String
        Set(ByVal value As String)
            _emailAddress = value
        End Set
        Get
            Return _emailAddress
        End Get
    End Property


    Public Sub EmailValidation()
        Dim Expression As New System.Text.RegularExpressions.Regex("\S+@\S+\.\S+")
        If Expression.IsMatch(Me.txtAccountingContactEmail.Text) Then
            'MsgBox("The email address is valid.")
            Me.cmdOK.Enabled = True
        Else
            'MsgBox("The email address is NOT valid.", MsgBoxStyle.Critical, "Invalid Mail ID")
            'Exit Sub
            Me.cmdOK.Enabled = False
        End If
    End Sub

    Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        Me.EmailAddess = ""
        Me.Close()
    End Sub

    Private Sub cmdOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOK.Click
        Me.EmailAddess = Me.txtAccountingContactEmail.Text
        frmVendor.txtAccountingContactEmail.Text = Me.txtAccountingContactEmail.Text
        frmVendor.CheckBox_EInvoicing.Checked = True
        frmVendor.SaveData()
        Me.Close()
    End Sub

    Private Sub txtAccountingContactEmail_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtAccountingContactEmail.TextChanged
        Me.EmailValidation()
    End Sub
End Class