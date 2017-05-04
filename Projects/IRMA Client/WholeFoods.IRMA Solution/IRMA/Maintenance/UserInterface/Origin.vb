Option Strict Off
Option Explicit On

Imports log4net

Friend Class frmOrigin
	Inherits System.Windows.Forms.Form
    Private IsInitializing As Boolean
    Dim rsOrigin As dao.Recordset
	Dim pbDataChanged As Boolean
    Dim plOriginID As Integer

    ' Define the log4net logger for this class.
    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)


    Private Sub frmOrigin_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        logger.Debug("frmOrigin_FormClosing Entry")
        e.Cancel = Not SaveData()
        logger.Debug("frmOrigin_FormClosing Exit")

    End Sub
	
	Private Sub frmOrigin_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load

        logger.Debug("frmOrigin_Load Entry")

		'-- Center the form
		CenterForm(Me)
		
		RefreshData(-1)
		
        SetActive(cmdAdd, gbSuperUser Or gbItemAdministrator)

        logger.Debug("frmOrigin_Load Exit")
		
	End Sub
	
	Private Sub cmdAdd_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdAdd.Click

        logger.Debug("cmdAdd_Click Entry")

		'-- if its not the first then update the data
		If plOriginID > -1 Then
			If Not lblReadOnly.Visible Then
				If Not SaveData Then Exit Sub
			End If
		End If
		
		glOriginID = 0
		
        '-- Call the adding form
        Dim fOriginAdd As New frmOriginAdd
        fOriginAdd.ShowDialog()
        fOriginAdd.Dispose()
		
		'-- a new Origin was added
		If glOriginID = -2 Then
			'-- Put the new data in
			RefreshData(-2)
			txtField(iOriginOrigin_Name).Focus()
		ElseIf plOriginID > -1 Then 
			RefreshData(plOriginID)
        End If

        logger.Debug("cmdAdd_Click Exit")

		
	End Sub
	
	Private Sub cmdOriginSearch_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdOriginSearch.Click

        logger.Debug("cmdOriginSearch_Click Entry")

		'-- Do a validation event
		If Not lblReadOnly.Visible Then
			If Not SaveData Then Exit Sub
		End If
		
		'-- Set glOriginid to none found
		glOriginID = 0
		
		'-- Set the search type
		giSearchType = iSearchOrigin
		
        '-- Open the search form
        Dim fSearch As New frmSearch
        fSearch.Text = "Search for Origin by Origin Name"
        fSearch.ShowDialog()
        fSearch.Dispose()
		
		'-- if its not zero, then something was found
		If glOriginID > 0 Then
			RefreshData(glOriginID)
		Else
			RefreshData(plOriginID)
        End If

        logger.Debug("cmdOriginSearch_Click Exit")
		
	End Sub
	
	Private Sub cmdDelete_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdDelete.Click

        logger.Debug("cmdDelete_Click Entry")

		'-- Make sure they really want to delete that Origin
		If MsgBox("Really delete Origin """ & Trim(txtField(iOriginOrigin_Name).Text) & """?", MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton2, "Warning!") = MsgBoxResult.No Then
            logger.Info("Really delete Origin=NO")
            logger.Debug("cmdDelete_Click Exit")

            Exit Sub
		End If
		
		'-- Delete the Origin from the database
		On Error Resume Next
		SQLExecute("EXEC UnlockOrigin " & plOriginID, dao.RecordsetOptionEnum.dbSQLPassThrough)
		
		Dim lIgnoreErrNum(0) As Integer
		lIgnoreErrNum(0) = 547 'Can't be deleted - it would violate referential integrity.
		SQLExecute3("EXEC DeleteOrigin " & plOriginID, dao.RecordsetOptionEnum.dbSQLPassThrough, lIgnoreErrNum)
		
		If Err.Number = 547 Then
			On Error GoTo 0
			'Lock it back.
			SQLExecute("EXEC LockOrigin " & plOriginID & ", " & giUserID, dao.RecordsetOptionEnum.dbSQLPassThrough)
            Call MsgBox("This Origin is in use and cannot be deleted.", MsgBoxStyle.Exclamation, "Cannot delete Origin...")
            logger.Info("This Origin is in use and cannot be deleted.")
		Else
			On Error GoTo 0
			
			glOriginID = plOriginID
			
			'-- Refresh the grid and seek the new one of its place
			RefreshData(plOriginID)
        End If

        logger.Debug("cmdDelete_Click Exit")

		
	End Sub
	
	Private Sub cmdUnlock_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdUnlock.Click
        logger.Debug("cmdUnlock_Click Entry")
		If MsgBox("Really unlock this record?", MsgBoxStyle.YesNo + MsgBoxStyle.Question, "Warning!") = MsgBoxResult.Yes Then
            logger.Info("Really unlock this record?-Yes")
			SQLExecute("EXEC UnlockOrigin " & plOriginID, dao.RecordsetOptionEnum.dbSQLPassThrough)
			RefreshData(plOriginID)
			
		End If
        logger.Debug("cmdUnlock_Click Exit")
	End Sub

	Function SaveData() As Boolean

        logger.Debug("SaveData Entry")

		SaveData = True
		
		If plOriginID = -1 Then Exit Function
		
		If pbDataChanged Then
			
			'-- Make sure Origin name is not null
			If Trim(txtField(iOriginOrigin_Name).Text) = vbNullString Then
				If MsgBox("Changes will NOT be made if Origin is left blank. " & Chr(13) & "Enter Origin name?", MsgBoxStyle.Exclamation + MsgBoxStyle.YesNo, "Origin Name Missing!") = MsgBoxResult.Yes Then
					txtField(iOriginOrigin_Name).Focus()
					SaveData = False
				Else
					SQLExecute("EXEC UnlockOrigin " & plOriginID, dao.RecordsetOptionEnum.dbSQLPassThrough)
                End If
                logger.Debug(" Changes will NOT be made if Origin is left blank. SaveData = False ")
                logger.Debug("SaveData Exit")

				Exit Function
			End If
			
			'-- Make sure they haven't duped a name
			SQLOpenRS(rsOrigin, "EXEC CheckForDuplicateOrigins " & plOriginID & ", '" & txtField(iOriginOrigin_Name).Text & "'", dao.RecordsetTypeEnum.dbOpenSnapshot, dao.RecordsetOptionEnum.dbSQLPassThrough)
			If rsOrigin.Fields("OriginCount").Value > 0 Then
				rsOrigin.Close()
				If MsgBox("Duplicate Origin Found, Changes will NOT be made if Origin is a duplicate. " & Chr(13) & "Change Origin Name?", MsgBoxStyle.Exclamation + MsgBoxStyle.YesNo, "Origin Name Duplicate!") = MsgBoxResult.Yes Then
					txtField(iOriginOrigin_Name).Focus()
					SaveData = False
				Else
					SQLExecute("EXEC UnlockOrigin " & plOriginID, dao.RecordsetOptionEnum.dbSQLPassThrough)
                End If

                logger.Debug(" Duplicate Origin Found, Changes will NOT be made if Origin is a duplicate.. SaveData = False ")
                logger.Debug("SaveData Exit")

				Exit Function
			End If
			rsOrigin.Close()
			
			'-- Make sure the losers don't make unwanted changes.
			If Not bProfessional Then
				If MsgBox("Save changes to Origin?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, "Data Changed") = MsgBoxResult.No Then
                    SQLExecute("EXEC UnlockOrigin " & plOriginID, DAO.RecordsetOptionEnum.dbSQLPassThrough)

                    logger.Debug("Save changes to Origin?.. No")
                    logger.Debug("SaveData Exit")
					Exit Function
				End If
			End If
			
		End If
		
		'-- Everything is ok
		If pbDataChanged Then
			SQLExecute("EXEC UpdateOriginInfo " & plOriginID & ", '" & Trim(txtField(iOriginOrigin_Name).Text) & "'", dao.RecordsetOptionEnum.dbSQLPassThrough)
		ElseIf Not lblReadOnly.Visible Then 
			SQLExecute("EXEC UnlockOrigin " & plOriginID, dao.RecordsetOptionEnum.dbSQLPassThrough)
        End If

        logger.Debug("SaveData Exit")
		
	End Function
	
	Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click
        logger.Debug("cmdExit_Click Entry")
        Me.Close()
        logger.Debug("cmdExit_Click Exit")
		
	End Sub
	
	Private Sub frmOrigin_Activated(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Activated

        '-- Make sure there is data in the database
        'CheckNoOrigins()

    End Sub
	
	Private Sub txtField_TextChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles txtField.TextChanged
        If Me.IsInitializing Then Exit Sub
        Dim Index As Short = txtField.GetIndex(eventSender)

        pbDataChanged = True

    End Sub
	
	Private Sub txtField_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles txtField.KeyPress

        logger.Debug("txtField_KeyPress Entry")

        Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
		Dim Index As Short = txtField.GetIndex(eventSender)
		
		If Not txtField(Index).ReadOnly Then
			KeyAscii = ValidateKeyPressEvent(KeyAscii, txtField(Index).Tag, txtField(Index), 0, 0, 0)
		End If
		
		eventArgs.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			eventArgs.Handled = True
        End If

        logger.Debug("txtField_KeyPress Exit")

	End Sub
	
    Sub CheckNoOrigins()

        logger.Debug("CheckNoOrigins Entry")


        '-- Make sure there is data for them to be in this form
        If plOriginID = -1 Then
            If MsgBox("No Origins found in the database." & vbCrLf & "Would you like to add one?", MsgBoxStyle.YesNo + MsgBoxStyle.Question, "Notice!") = MsgBoxResult.Yes Then
                cmdAdd_Click(cmdAdd, New System.EventArgs())
                If plOriginID = -1 Then
                    Me.Close()
                End If
            Else
                Me.Close()
            End If
        End If


        logger.Debug("CheckNoOrigins Exit")

    End Sub
	
    Sub RefreshData(ByVal lRecord As Integer)

        logger.Debug("RefreshData Entry")


        Select Case lRecord
            Case -2 : SQLOpenRS(rsOrigin, "EXEC GetOriginInfoLast", DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
            Case -1 : SQLOpenRS(rsOrigin, "EXEC GetOriginInfoFirst", DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
            Case Else : SQLOpenRS(rsOrigin, "EXEC GetOriginInfo " & lRecord, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
        End Select

        If rsOrigin.EOF Then
            plOriginID = -1
            txtField(iOriginOrigin_Name).Text = ""
        Else
            plOriginID = rsOrigin.Fields("Origin_ID").Value
            txtField(iOriginOrigin_Name).Text = rsOrigin.Fields("Origin_Name").Value & ""

            If IsDBNull(rsOrigin.Fields("User_ID").Value) Then
                lblReadOnly.Visible = False
                cmdUnlock.Enabled = False
                SQLExecute("EXEC LockOrigin " & plOriginID & ", " & giUserID, DAO.RecordsetOptionEnum.dbSQLPassThrough)
            Else
                lblReadOnly.Text = "Read Only (" & GetInvUserName(rsOrigin.Fields("User_ID").Value) & ")"
                lblReadOnly.Visible = True
                cmdUnlock.Enabled = (gbLockAdministrator Or giUserID = rsOrigin.Fields("User_ID").Value)
            End If

        End If
        rsOrigin.Close()

        SetActive(txtField(iOriginOrigin_Name), (Not lblReadOnly.Visible) And (gbSuperUser Or gbItemAdministrator))
        SetActive(cmdDelete, (Not lblReadOnly.Visible) And (gbSuperUser Or gbItemAdministrator))

        pbDataChanged = False

        If lRecord <> -1 And plOriginID = -1 Then
            RefreshData(-1)
            CheckNoOrigins()
        ElseIf lRecord = -1 And plOriginID = -1 Then
            CheckNoOrigins()
        End If

        logger.Debug("RefreshData Exit")


    End Sub
End Class