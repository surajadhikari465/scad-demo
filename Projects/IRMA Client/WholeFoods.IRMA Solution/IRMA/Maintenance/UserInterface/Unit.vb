Option Strict Off
Option Explicit On

Imports log4net

Friend Class frmUnit
	Inherits System.Windows.Forms.Form
    Private IsInitializing As Boolean

	Dim rsUnit As dao.Recordset
	Dim pbDataChanged As Boolean
    Dim plUnitID As Integer

    ' Define the log4net logger for this class.
    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

	
	Private Sub chkField_CheckStateChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles chkField.CheckStateChanged
        logger.Debug("chkField_CheckStateChanged Entry")

        If IsInitializing = True Then Exit Sub
        Dim Index As Short = chkField.GetIndex(eventSender)

        pbDataChanged = True

        logger.Debug("chkField_CheckStateChanged Exit")


    End Sub
	
	Private Sub cmdAdd_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdAdd.Click

        logger.Debug("cmdAdd_Click Entry")

		'-- if its not the first then update the data
		If plUnitID > -1 Then
			If Not lblReadOnly.Visible Then
				If Not SaveData Then Exit Sub
			End If
		End If
		
		glUnitID = 0
		
        '-- Call the adding form
        Dim fUnitAdd As New frmUnitAdd
        fUnitAdd.ShowDialog()
        fUnitAdd.Dispose()
		
		'-- a new Unit was added
		If glUnitID = -2 Then
			'-- Put the new data in
			RefreshData(-2)
			txtField(iUnitUnit_Name).Focus()
		ElseIf plUnitID > -1 Then 
			RefreshData(plUnitID)
        End If

        logger.Debug("cmdAdd_Click Exit")

		
	End Sub
	
	Private Sub cmdUnitSearch_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdUnitSearch.Click
        logger.Debug("cmdUnitSearch_Click Entry")

		'-- Do a validation event
		If Not lblReadOnly.Visible Then
			If Not SaveData Then Exit Sub
		End If
		
		'-- Set glUnitid to none found
		glUnitID = 0
		
		'-- Set the search type
		giSearchType = iSearchUnit
		
        '-- Open the search form
        Dim fSearch As New frmSearch
        fSearch.Text = "Search for Unit by Unit Name"
        fSearch.ShowDialog()
        fSearch.Dispose()
		
		'-- if its not zero, then something was found
		If glUnitID > 0 Then
			RefreshData(glUnitID)
		Else
			RefreshData(plUnitID)
        End If

        logger.Debug("cmdUnitSearch_Click Exit")

		
	End Sub
	
	Private Sub cmdDelete_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdDelete.Click

        logger.Debug("cmdDelete_Click Entry")

		'-- Make sure they really want to delete that Unit
		If MsgBox("Really delete Unit """ & Trim(txtField(iUnitUnit_Name).Text) & """?", MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton2, "Warning!") = MsgBoxResult.No Then
			Exit Sub
		End If
		
		'-- Delete the Unit from the database
		On Error Resume Next
		SQLExecute("EXEC UnlockUnit " & plUnitID, dao.RecordsetOptionEnum.dbSQLPassThrough)
		
		Dim lIgnoreErrNum(0) As Integer
		lIgnoreErrNum(0) = 547 'Can't be deleted - it would violate referential integrity.
		SQLExecute3("EXEC DeleteUnit " & plUnitID, dao.RecordsetOptionEnum.dbSQLPassThrough, lIgnoreErrNum)
		
		If Err.Number = 547 Then
			On Error GoTo 0
			'Lock it back.
			SQLExecute("EXEC LockUnit " & plUnitID & ", " & giUserID, dao.RecordsetOptionEnum.dbSQLPassThrough)
			Call MsgBox("This Unit is in use and cannot be deleted.", MsgBoxStyle.Exclamation, "Cannot delete Unit...")
		Else
			On Error GoTo 0
			glUnitID = plUnitID
			
			'-- Refresh the grid and seek the new one of its place
			RefreshData(plUnitID)
        End If

        logger.Debug("cmdDelete_Click Exit")

		
	End Sub
	
	Private Sub cmdUnlock_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdUnlock.Click

        logger.Debug("cmdUnlock_Click Entry")

		If MsgBox("Really unlock this record?", MsgBoxStyle.YesNo + MsgBoxStyle.Question, "Warning!") = MsgBoxResult.Yes Then
			
			SQLExecute("EXEC UnlockUnit " & plUnitID, dao.RecordsetOptionEnum.dbSQLPassThrough)
			RefreshData(plUnitID)
			
        End If

        logger.Debug("cmdUnlock_Click Exit")

		
	End Sub
	
    Function SaveData() As Boolean

        logger.Debug("SaveData Entry")

        SaveData = True

        If plUnitID = -1 Then Exit Function

        If pbDataChanged Then

            '-- Make sure Unit name is not null
            If Trim(txtField(iUnitUnit_Name).Text) = vbNullString Then
                If MsgBox("Changes will NOT be made if Unit is left blank. " & Chr(13) & "Enter Unit name?", MsgBoxStyle.Exclamation + MsgBoxStyle.YesNo, "Unit Name Missing!") = MsgBoxResult.Yes Then
                    txtField(iUnitUnit_Name).Focus()
                    SaveData = False
                Else
                    SQLExecute("EXEC UnlockUnit " & plUnitID, DAO.RecordsetOptionEnum.dbSQLPassThrough)
                End If

                logger.Info("Changes will NOT be made if Unit is left blank.-False")
                logger.Debug("SaveData Exit")
                Exit Function
            End If

            '-- Make sure they haven't duped a name
            SQLOpenRS(rsUnit, "EXEC CheckForDuplicateUnits " & plUnitID & ", '" & txtField(iUnitUnit_Name).Text & "'", DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
            If rsUnit.Fields("UnitCount").Value > 0 Then
                rsUnit.Close()
                If MsgBox("Duplicate Unit Found, Changes will NOT be made if Unit is a duplicate. " & Chr(13) & "Change Unit Name?", MsgBoxStyle.Exclamation + MsgBoxStyle.YesNo, "Unit Name Duplicate!") = MsgBoxResult.Yes Then
                    txtField(iUnitUnit_Name).Focus()
                    SaveData = False
                Else
                    SQLExecute("EXEC UnlockUnit " & plUnitID, DAO.RecordsetOptionEnum.dbSQLPassThrough)
                End If
                logger.Info("Duplicate Unit Found, Changes will NOT be made if Unit is a duplicate.--False")
                logger.Debug("SaveData Exit")
                Exit Function
            End If
            rsUnit.Close()

            '-- Make sure the losers don't make unwanted changes.
            If Not bProfessional Then
                If MsgBox("Save changes to Unit?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, "Data Changed") = MsgBoxResult.No Then
                    SQLExecute("EXEC UnlockUnit " & plUnitID, DAO.RecordsetOptionEnum.dbSQLPassThrough)
                    logger.Info("Save changes to Unit?--No")
                    logger.Debug("SaveData Exit")
                    Exit Function
                End If
            End If

        End If

        '-- Everything is ok
        If pbDataChanged Then
            SQLExecute("EXEC UpdateUnitInfo " & plUnitID & ", '" & Trim(txtField(iUnitUnit_Name).Text) & "', -" & System.Math.Abs(chkField(iUnitWeight_Unit).CheckState) & ", '" & Trim(txtField(iUnitUnit_Abbreviation).Text) & "'", DAO.RecordsetOptionEnum.dbSQLPassThrough)
        ElseIf Not lblReadOnly.Visible Then
            SQLExecute("EXEC UnlockUnit " & plUnitID, DAO.RecordsetOptionEnum.dbSQLPassThrough)
        End If

        logger.Debug("SaveData Exit")

    End Function
	
	Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click
        logger.Debug("cmdExit_Click Entry")
        Me.Close()
        logger.Debug("cmdExit_Click Exit")
		
	End Sub
	
	Private Sub frmUnit_Activated(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Activated

        logger.Debug("frmUnit_Activated Entry")

        '-- Make sure there is data in the database
        CheckNoUnits()

        logger.Debug("frmUnit_Activated Exit")


    End Sub
	
	Private Sub frmUnit_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load

        logger.Debug("frmUnit_Load Entry")
		'-- Center the form
		CenterForm(Me)
		
		RefreshData(-1)
        logger.Debug("frmUnit_Load Exit")

		
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
	
    Sub CheckNoUnits()

        logger.Debug("CheckNoUnits Entry")


        '-- Make sure there is data for them to be in this form
        If plUnitID = -1 Then
            If MsgBox("No Units found in the database." & vbCrLf & "Would you like to add one?", MsgBoxStyle.YesNo + MsgBoxStyle.Question, "Notice!") = MsgBoxResult.Yes Then
                cmdAdd_Click(cmdAdd, New System.EventArgs())
                If plUnitID = -1 Then
                    Me.Close()
                End If
            Else
                Me.Close()
            End If
        End If

        logger.Debug("CheckNoUnits Exit")


    End Sub
	
    Sub RefreshData(ByVal lRecord As Integer)

        logger.Debug("RefreshData Entry")


        Select Case lRecord
            Case -2 : SQLOpenRS(rsUnit, "EXEC GetUnitInfoLast", DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
            Case -1 : SQLOpenRS(rsUnit, "EXEC GetUnitInfoFirst", DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
            Case Else : SQLOpenRS(rsUnit, "EXEC GetUnitInfo " & lRecord, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
        End Select

        If rsUnit.EOF Then
            plUnitID = -1
            txtField(iUnitUnit_Name).Text = ""
        Else
            plUnitID = rsUnit.Fields("Unit_ID").Value
            txtField(iUnitUnit_Name).Text = rsUnit.Fields("Unit_Name").Value & ""
            txtField(iUnitUnit_Abbreviation).Text = rsUnit.Fields("Unit_Abbreviation").Value & ""
            If rsUnit.Fields("Weight_Unit").Value Then
                chkField(iUnitWeight_Unit).CheckState = CheckState.Checked
            Else
                chkField(iUnitWeight_Unit).CheckState = CheckState.Unchecked
            End If

            If IsDBNull(rsUnit.Fields("User_ID").Value) Then
                lblReadOnly.Visible = False
                cmdUnlock.Enabled = False
                SQLExecute("EXEC LockUnit " & plUnitID & ", " & giUserID, DAO.RecordsetOptionEnum.dbSQLPassThrough)
            Else
                lblReadOnly.Text = "Read Only (" & GetInvUserName(rsUnit.Fields("User_ID").Value) & ")"
                lblReadOnly.Visible = True
                cmdUnlock.Enabled = (gbLockAdministrator Or giUserID = rsUnit.Fields("User_ID").Value)
            End If

        End If
        rsUnit.Close()

        SetActive(txtField(iUnitUnit_Name), Not lblReadOnly.Visible)
        SetActive(cmdDelete, Not lblReadOnly.Visible)

        pbDataChanged = False

        If lRecord <> -1 And plUnitID = -1 Then
            RefreshData(-1)
            CheckNoUnits()
        End If

        logger.Debug("RefreshData Exit")


    End Sub

    Private Sub frmUnit_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        e.Cancel = Not SaveData()

    End Sub
End Class