Option Strict Off
Option Explicit On
Friend Class frmContactAdd
	Inherits System.Windows.Forms.Form
	
	Private Sub cmdAdd_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdAdd.Click
        Dim sMsg As String = String.Empty
        Dim rsContact As DAO.Recordset = Nothing
		
		'-- Take out unwanted spaces
		txtContact_Name.Text = Trim(txtContact_Name.Text)
		
		'-- Check to see if anything was entered
        If txtContact_Name.Text = vbNullString Then
            sMsg = String.Format(ResourcesIRMA.GetString("Required"), lblContact.Text.Replace(":", ""))
            MsgBox(sMsg, MsgBoxStyle.Exclamation, Me.Text)
            txtContact_Name.Focus()
            Exit Sub
        End If

        Try
            '-- Check to see if the name already exists
            rsContact = SQLOpenRecordSet("EXEC CheckForDuplicateContacts " & glVendorID & ", 0, '" & txtContact_Name.Text & "'", DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
            If rsContact.Fields("ContactCount").Value > 0 Then
                sMsg = String.Format(ResourcesItemHosting.GetString("Exists"), lblContact.Text.Replace(":", ""), Chr(13))
                If MsgBox(sMsg, MsgBoxStyle.YesNo + MsgBoxStyle.Information, Me.Text) = MsgBoxResult.No Then
                    rsContact.Close()
                    txtContact_Name.Focus()
                    Exit Sub
                End If
            End If
        Finally
            If rsContact IsNot Nothing Then
                rsContact.Close()
                rsContact = Nothing
            End If
        End Try

        '-- Add the new record
        SQLExecute("EXEC InsertContact " & glVendorID & ", '" & txtContact_Name.Text & "'", DAO.RecordsetOptionEnum.dbSQLPassThrough)
        glContactID = -2

        '-- Go back to the previous form
        Me.Close()

    End Sub
	
	Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click
		
		'-- Don't Add a vendor
		Me.Close()
		
	End Sub
	
	Private Sub frmContactAdd_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
		
		'-- Center the form
		CenterForm(Me)
		
	End Sub
	
	Private Sub txtContact_Name_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles txtContact_Name.KeyPress
		Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
		
		KeyAscii = ValidateKeyPressEvent(KeyAscii, "String", txtContact_Name, 0, 0, 0)
		
		eventArgs.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			eventArgs.Handled = True
		End If
	End Sub
End Class