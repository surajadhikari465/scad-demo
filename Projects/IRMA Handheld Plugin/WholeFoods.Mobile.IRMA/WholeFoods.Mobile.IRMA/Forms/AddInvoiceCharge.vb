Imports System.ServiceModel

Public Class AddInvoiceCharge

    Private session As Session
    Private serviceFault As ParsedCFFaultException = Nothing
    Private allocatedCharges As InvoiceCharge()
    Private nonAllocatedCharges As ListsSubteam()
    Private orderHeaderID As Integer
    Private notEmptyValidation As Boolean
    Private selectedChargeValidation As Boolean
    Private chargeValue As Decimal
    Private serviceCallSuccess As Boolean

    Public Sub New(ByVal session As Session, ByVal orderHeaderID As Integer)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.session = session
        Me.orderHeaderID = orderHeaderID
        AlignText()

    End Sub

    Private Sub AddInvoiceCharge_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Cursor.Current = Cursors.WaitCursor

        Try
            ' Attempt a service call to populate the lists of allocated and non-allocated charges.
            serviceCallSuccess = True

            allocatedCharges = session.WebProxyClient.GetAllocatedCharges()
            nonAllocatedCharges = session.WebProxyClient.GetGLAccountSubteams(orderHeaderID)


            ' Explicitly handle service faults, timeouts, and connection failures.  If this initialization block fails, the user will
            ' fall back to the last form she was on.
        Catch ex As Microsoft.Tools.ServiceModel.CFFaultException
            serviceFault = New ParsedCFFaultException(ex.FaultMessage)
            Dim err As New ErrorHandler(serviceFault)
            err.ShowErrorNotification()
            Me.DialogResult = Windows.Forms.DialogResult.Abort

        Catch ex As TimeoutException
            Dim err As New ErrorHandler()
            err.DisplayErrorMessage(Messages.TIMEOUT_EXCEPTION, "AddInvoiceCharge_Load")
            Me.DialogResult = Windows.Forms.DialogResult.Abort

        Catch ex As CommunicationException
            Dim err As New ErrorHandler()
            err.DisplayErrorMessage(Messages.COMM_EXCEPTION, "AddInvoiceCharge_Load")
            Me.DialogResult = Windows.Forms.DialogResult.Abort

        Catch ex As Exception
            Dim err As New ErrorHandler()
            err.DisplayErrorMessage(ex.Message, "AddInvoiceCharge_Load")
            Me.DialogResult = Windows.Forms.DialogResult.Abort

        Finally
            Cursor.Current = Cursors.Default

        End Try

    End Sub

    Private Sub RadioButtonNonAllocated_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButtonNonAllocated.CheckedChanged

        ' Load the combobox with the GL Account subteams.
        ComboBoxChargeDescription.Enabled = True
        TextBoxChargeValue.Enabled = False

        ComboBoxChargeDescription.Items.Clear()
        If RadioButtonNonAllocated.Checked = True Then
            For Each subteam As ListsSubteam In nonAllocatedCharges
                ComboBoxChargeDescription.Items.Add(subteam.SubteamName)
            Next
        End If

    End Sub

    Private Sub RadioButtonAllocatedCharge_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButtonAllocatedCharge.CheckedChanged

        ' Load the combobox with the list of allocated charges.
        ComboBoxChargeDescription.Enabled = True
        TextBoxChargeValue.Enabled = False

        ComboBoxChargeDescription.Items.Clear()
        If RadioButtonAllocatedCharge.Checked = True Then
            For Each charge As InvoiceCharge In allocatedCharges
                ComboBoxChargeDescription.Items.Add(charge.ElementName)
            Next
        End If

    End Sub

    Private Sub TextBoxChargeValue_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBoxChargeValue.TextChanged
        notEmptyValidation = False
        selectedChargeValidation = False

        If TextBoxChargeValue.Text.Length > 0 Then
            notEmptyValidation = True

            If ComboBoxChargeDescription.SelectedItem <> Nothing Then
                selectedChargeValidation = True
            End If

        End If

        If notEmptyValidation And selectedChargeValidation Then
            ButtonAddCharge.Enabled = True
        End If
    End Sub

    Private Sub ComboBoxChargeDescription_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBoxChargeDescription.SelectedIndexChanged

        If ComboBoxChargeDescription.SelectedItem <> Nothing Then
            TextBoxChargeValue.Enabled = True
            TextBoxChargeValue.Text = String.Empty
        End If

    End Sub

    Private Sub ButtonAddCharge_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAddCharge.Click
        AddCharge()
    End Sub

    Private Sub ButtonCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonCancel.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
    End Sub

    Private Sub AddInvoiceCharge_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown

        If (e.KeyCode = System.Windows.Forms.Keys.Enter) And ButtonAddCharge.Enabled Then
            AddCharge()
        End If

    End Sub

    Private Sub AddCharge()

        Dim allowanceIndicator As String
        Dim allowance As Boolean = False

        ' The charge value must be numeric.
        If Not IsNumeric(TextBoxChargeValue.Text) Then
            MsgBox("Please enter numeric values only.", MsgBoxStyle.Critical, Me.Text)
            TextBoxChargeValue.Text = String.Empty
            ButtonAddCharge.Enabled = False
            Exit Sub
        Else
            chargeValue = TextBoxChargeValue.Text
        End If

        ' Allocated charges must be positive.
        If RadioButtonAllocatedCharge.Checked = True Then
            If chargeValue < 0 Then
                MsgBox("Allocated charges cannot be negative.", MsgBoxStyle.Information, Me.Text)
                TextBoxChargeValue.Text = String.Empty
                ButtonAddCharge.Enabled = False
                Exit Sub
            End If
        End If

        ' If it's an allocated charge, determine if it's a charge or allowance.
        If RadioButtonAllocatedCharge.Checked = True Then

            allowanceIndicator = (From charge In allocatedCharges _
                        Where charge.ElementName = ComboBoxChargeDescription.SelectedItem _
                        Select charge.IsAllowance).Single

            If allowanceIndicator = "-1" Then
                ' The charge is an allowance (the hardcoded -1 is due to the way the db stores this value).  Reverse the sign.
                chargeValue = chargeValue * -1
                allowance = True
            End If
        End If

        Try
            ' Attempt a service call to add the invoice charge.
            serviceCallSuccess = True

            Dim success As Result
            Dim SACType As Integer = If(RadioButtonAllocatedCharge.Checked, 1, 2)
            Dim description As String = If(RadioButtonAllocatedCharge.Checked, ComboBoxChargeDescription.SelectedItem, String.Empty)
            Dim subteamGLAccount As Integer = If(RadioButtonNonAllocated.Checked, (From charge In nonAllocatedCharges _
                                                                                   Where charge.SubteamName = ComboBoxChargeDescription.SelectedItem _
                                                                                   Select charge.SubteamNo).Single, -1)

            success = session.WebProxyClient.AddInvoiceCharge(orderHeaderID, SACType, description, subteamGLAccount, allowance, chargeValue)

            If Not success.Status Then
                MsgBox("Error adding charge.", MsgBoxStyle.Exclamation, Me.Text)
            End If

            ' Explicitly handle service faults, timeouts, and connection failures.  If this call fails, allow the user to try adding the charge again.
        Catch ex As Microsoft.Tools.ServiceModel.CFFaultException
            serviceFault = New ParsedCFFaultException(ex.FaultMessage)
            Dim err As New ErrorHandler(serviceFault)
            err.ShowErrorNotification()
            serviceCallSuccess = False

        Catch ex As TimeoutException
            Dim err As New ErrorHandler()
            err.DisplayErrorMessage(Messages.TIMEOUT_EXCEPTION, "AddCharge")
            serviceCallSuccess = False

        Catch ex As CommunicationException
            Dim err As New ErrorHandler()
            err.DisplayErrorMessage(Messages.COMM_EXCEPTION, "AddCharge")
            serviceCallSuccess = False

        Catch ex As Exception
            Dim err As New ErrorHandler()
            err.DisplayErrorMessage(ex.Message, "AddCharge")
            serviceCallSuccess = False

        End Try

        If Not serviceCallSuccess Then
            ' The call to AddInvoiceCharge failed.  End the method and allow the user to retry.
            Exit Sub
        End If

        Me.DialogResult = Windows.Forms.DialogResult.OK

    End Sub

    Private Sub AlignText()
        LabelHeader.TextAlign = ContentAlignment.TopCenter
        LabelAmount.TextAlign = ContentAlignment.TopRight
    End Sub

End Class