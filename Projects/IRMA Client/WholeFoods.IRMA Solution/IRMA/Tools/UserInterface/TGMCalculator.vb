Option Strict Off
Option Explicit On
Friend Class frmTGMCalculator
    Inherits System.Windows.Forms.Form
    Private IsInitializing As Boolean
	Private bChanging As Boolean
	
	Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click
		
		Me.Close()
		
    End Sub

    Private Sub frmTGMCalculator_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Select Case MsgBox("Save this as the new retail?", MsgBoxStyle.YesNoCancel + MsgBoxStyle.Question, "Keep Retail")
            Case MsgBoxResult.Yes : gtTGMCalculator.Cost = CDec(lblCost.Text)
                gtTGMCalculator.Retail = CDec(txtPrice.Text)
                gtTGMCalculator.Margin = CDec(txtMargin.Text)
            Case MsgBoxResult.No : gtTGMCalculator.Identifier = ""
            Case MsgBoxResult.Cancel
                e.Cancel = 1
        End Select

    End Sub
	
	Private Sub frmTGMCalculator_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
		
		'-- Center the screen
		CenterForm(Me)
		
		'-- Load the data
		lblIdentifier.Text = gtTGMCalculator.Identifier
		lblDescription.Text = gtTGMCalculator.Item_Description
		lblCost.Text = VB6.Format(gtTGMCalculator.Cost, "###0.00#")
		txtPrice.Text = VB6.Format(gtTGMCalculator.Retail, "##0.00")
		txtMargin.Text = VB6.Format(gtTGMCalculator.Margin, "##0.00#")
		
	End Sub
	
    Private Sub txtMargin_TextChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles txtMargin.TextChanged
        If Me.IsInitializing = True Then Exit Sub

        If Not bChanging Then

            bChanging = True

            If CDec(txtMargin.Text) = 0 Or CDec(lblCost.Text) = 0 Then
                txtPrice.Text = "0.00"
            Else
                txtPrice.Text = (VB6.Format(CDec(lblCost.Text) / (1 - CDec(Val(txtMargin.Text)) / 100), "##0.00"))
            End If


            bChanging = False

        End If

    End Sub
	
	Private Sub txtMargin_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles txtMargin.KeyPress
		Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
		
		KeyAscii = ValidateKeyPressEvent(KeyAscii, "-ExtCurrency", txtMargin, -99.999, 999.999, 3)
		
		eventArgs.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			eventArgs.Handled = True
		End If
	End Sub
	
	Private Sub txtMargin_Leave(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles txtMargin.Leave
		
		If txtMargin.Text = "" Then txtMargin.Text = "0"
		
	End Sub
	
    Private Sub txtPrice_TextChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles txtPrice.TextChanged
        If Me.IsInitializing = True Then Exit Sub

        If Not bChanging Then

            bChanging = True

            If CDec("0" & txtPrice.Text) = 0 Then
                txtMargin.Text = "0.00"
            Else
                txtMargin.Text = CStr((CDec(txtPrice.Text) - CDec(lblCost.Text)) / CDec(txtPrice.Text) * 100)
            End If

            bChanging = False

        End If

    End Sub
	
	Private Sub txtPrice_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles txtPrice.KeyPress
		Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
		
		KeyAscii = ValidateKeyPressEvent(KeyAscii, "ExtCurrency", txtPrice, 0, 999.99, 2)
		
		eventArgs.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			eventArgs.Handled = True
		End If
	End Sub
	
	Private Sub txtPrice_Leave(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles txtPrice.Leave
		
		If txtPrice.Text = "" Then txtPrice.Text = "0"
		
	End Sub
End Class