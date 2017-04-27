Option Strict Off
Option Explicit On
Friend Class frmContact
	Inherits System.Windows.Forms.Form
    Private IsInitializing As Boolean
	Dim rsContact As dao.Recordset
	Dim pbDataChanged As Boolean
	Dim plContactID As Integer
	
	Private Sub cmdAdd_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdAdd.Click
		
		'-- Force the validation event
		If plContactID > -1 Then
			If Not SaveData Then Exit Sub
		End If
		
		glContactID = 0
		
        '-- Call the adding form
        Dim fContactAdd As New frmContactAdd
        fContactAdd.ShowDialog()
        fContactAdd.Dispose()
		
		'-- a new vendor was added
		If glContactID = -2 Then
			RefreshData(-2)
            txtPhone.Focus()
		ElseIf plContactID <> -1 Then 
			RefreshData(plContactID)
		End If
		
	End Sub
	
	Private Sub cmdCompanySearch_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdCompanySearch.Click
		
		'-- Force the validation event
		If Not SaveData Then Exit Sub
		
		'-- Set glvendorid to none found
		glContactID = 0
		
		'-- Set the search type
		giSearchType = iSearchContactContact
		
        '-- Open the search form
        Dim fSearch As New frmSearch
        fSearch.Text = "Search for Contact by Contact Name"
        fSearch.ShowDialog()
        fSearch.Dispose()
		
		'-- if its not zero, then something was found
		If glVendorID <> 0 Then
			RefreshData(glContactID)
		Else
			RefreshData(plContactID)
		End If
		
	End Sub
	
	Private Sub cmdDelete_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdDelete.Click
        Dim sMsg As String = String.Empty

        '-- Make sure they really want to delete that vendor
        sMsg = String.Format(ResourcesItemHosting.GetString("DeleteContact"), txtContactName.Text)
        If MsgBox(sMsg, MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton2, Me.Text) = MsgBoxResult.No Then
            Exit Sub
        End If
		
		'-- Delete all the contacts from the database
		SQLExecute("EXEC DeleteContact " & plContactID, dao.RecordsetOptionEnum.dbSQLPassThrough, True)
		
		glContactID = plContactID
		
		'-- Refresh the grid and seek the new one of its place
		RefreshData(plContactID)
		
		'-- Check to see if it was deleted
        If glContactID = plContactID Then
            sMsg = ResourcesItemHosting.GetString("DeleteContactError")
            MsgBox(sMsg, MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, Me.Text)
        End If
		
	End Sub
	
	Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click
		
		'-- Go back to the vendor form
		Me.Close()
		
	End Sub
	
	Function SaveData() As Boolean
        Dim sMsg As String = String.Empty

		SaveData = True
		
		If pbDataChanged = True Then
			
			'-- Contact name not allowed to be null
            If Trim(txtContactName.Text) = vbNullString Then
                sMsg = String.Format(ResourcesItemHosting.GetString("ChangesNotMade"), Chr(13), lblContact.Text.Replace(":", ""))
                If MsgBox(sMsg, MsgBoxStyle.Exclamation + MsgBoxStyle.YesNo, Me.Text) = MsgBoxResult.Yes Then
                    SaveData = False
                    txtContactName.Focus()
                End If
                Exit Function
            End If
			
			'-- Make sure the losers don't make unwanted changes.
            If Not bProfessional Then
                sMsg = String.Format(ResourcesIRMA.GetString("SaveChanges"))
                If MsgBox(sMsg, MsgBoxStyle.Question + MsgBoxStyle.YesNo, Me.Text) = MsgBoxResult.No Then
                    Exit Function
                End If
            End If
			
            SQLExecute("EXEC UpdateContactInfo " & plContactID & ", '" & txtContactName.Text & "', '" & txtPhone.Text & "', '" & txtExt.Text & "', '" & txtFax.Text & "', '" & txtEmail.Text & "'", DAO.RecordsetOptionEnum.dbSQLPassThrough)

        End If

    End Function

    Private Sub frmContact_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load

        '-- Center the screen
        CenterForm(Me)

        RefreshData(-1)

        SetActive(cmdAdd, gbCoordinator Or gbAccountant)
        SetActive(cmdDelete, gbCoordinator Or gbAccountant)

        '-- Make sure there is data
        CheckNoContacts()

    End Sub



    Private Sub txtField_TextChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles txtContactName.TextChanged, txtPhone.TextChanged, txtFax.TextChanged, txtExt.TextChanged, txtEmail.TextChanged
        If Me.IsInitializing Then Exit Sub

        pbDataChanged = True

    End Sub

    Private Sub txtField_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles txtContactName.Enter, txtPhone.Enter, txtFax.Enter, txtExt.Enter, txtEmail.Enter
        HighlightText(CType(eventSender, TextBox))
    End Sub

    Private Sub txtField_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles txtContactName.KeyPress, txtPhone.KeyPress, txtFax.KeyPress, txtExt.KeyPress, txtEmail.KeyPress
        Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
        Dim txtBox As TextBox = CType(eventSender, TextBox)

        If KeyAscii = 13 Then
            KeyAscii = 0
            System.Windows.Forms.SendKeys.Send("{TAB}")
        ElseIf Not txtBox.ReadOnly Then
            KeyAscii = ValidateKeyPressEvent(KeyAscii, txtBox.Tag, txtBox, 0, 0, 0)
        End If

        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
    End Sub

    Sub CheckNoContacts()
        Dim sMsg As String = String.Empty

        '-- Make sure there is data
        If plContactID = -1 Then
            sMsg = String.Format(ResourcesItemHosting.GetString("NoContactsFound"), Chr(13))
            If MsgBox(sMsg, MsgBoxStyle.YesNo + MsgBoxStyle.Question, Me.Text) = MsgBoxResult.Yes Then
                cmdAdd_Click(cmdAdd, New System.EventArgs())
                If plContactID = -1 Then
                    Me.Close()
                End If
            Else
                Me.Close()
            End If
        End If

    End Sub

    Sub RefreshData(ByVal lRecord As Integer)

        Select Case lRecord
            Case -2 : SQLOpenRS(rsContact, "EXEC GetContactInfoLast " & glVendorID, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
            Case -1 : SQLOpenRS(rsContact, "EXEC GetContactInfoFirst " & glVendorID, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
            Case Else : SQLOpenRS(rsContact, "EXEC GetContactInfo " & lRecord, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
        End Select

        If rsContact.EOF Then
            plContactID = -1
        Else
            plContactID = rsContact.Fields("Contact_ID").Value
            txtContactName.Text = rsContact.Fields("Contact_Name").Value & ""
            txtPhone.Text = rsContact.Fields("Phone").Value & ""
            txtExt.Text = rsContact.Fields("Phone_Ext").Value & ""
            txtFax.Text = rsContact.Fields("Fax").Value & ""
            txtEmail.Text = rsContact.Fields("Email").Value & ""
        End If
        rsContact.Close()
        rsContact = Nothing

        pbDataChanged = False

        If lRecord <> -1 And plContactID = -1 Then
            RefreshData(-1)
            CheckNoContacts()
        End If

    End Sub

    Private Sub frmContact_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        e.Cancel = Not SaveData()

    End Sub
End Class