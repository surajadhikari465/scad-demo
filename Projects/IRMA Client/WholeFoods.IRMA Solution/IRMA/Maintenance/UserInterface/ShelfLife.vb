Option Strict Off
Option Explicit On
Imports log4net

Friend Class frmShelfLife
	Inherits System.Windows.Forms.Form
    Private IsInitializing As Boolean

	Dim rsShelfLife As dao.Recordset
	Dim pbDataChanged As Boolean
    Dim plShelfLifeID As Integer

    ' Define the log4net logger for this class.
    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

	
	Private Sub cmdAdd_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdAdd.Click

        logger.Debug("cmdAdd_Click Entry")

		'-- if its not the first then update the data
		If plShelfLifeID > -1 Then
			If Not lblReadOnly.Visible Then
				If Not SaveData Then Exit Sub
			End If
		End If
		
		glShelfLifeID = 0
		
        '-- Call the adding form
        frmShelfLifeAdd.ShowDialog()
        frmShelfLifeAdd.Close()
		
		'-- a new ShelfLife was added
		If glShelfLifeID = -2 Then
			'-- Put the new data in
			RefreshData(-2)
			txtField(iShelfLifeShelfLife_Name).Focus()
		ElseIf plShelfLifeID > -1 Then 
			RefreshData(plShelfLifeID)
        End If

        logger.Debug("cmdAdd_Click Exit")
		
	End Sub
	
	Private Sub cmdShelfLifesearch_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdShelfLifesearch.Click

        logger.Debug("cmdAdd_Click Entry")

		'-- Do a validation event
		If Not lblReadOnly.Visible Then
			If Not SaveData Then Exit Sub
		End If
		
		'-- Set glShelfLifeid to none found
		glShelfLifeID = 0
		
		'-- Set the search type
		giSearchType = iSearchShelfLife
		
        '-- Open the search form
        Dim fSearch As New frmSearch
        fSearch.Text = "Search for ShelfLife by ShelfLife Name"
        fSearch.ShowDialog()
        fSearch.Dispose()
		
		'-- if its not zero, then something was found
		If glShelfLifeID > 0 Then
			RefreshData(glShelfLifeID)
		Else
			RefreshData(plShelfLifeID)
		End If

        logger.Debug("cmdAdd_Click Exit")
	End Sub
	
	Private Sub cmdDelete_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdDelete.Click

        logger.Debug("cmdDelete_Click Entry")

		'-- Make sure they really want to delete that ShelfLife
		If MsgBox("Really delete ShelfLife """ & Trim(txtField(iShelfLifeShelfLife_Name).Text) & """?", MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton2, "Warning!") = MsgBoxResult.No Then
            logger.Info("Really delete ShelfLife - No")
            logger.Debug("cmdDelete_Click Exit")
            Exit Sub
		End If
		
		'-- Delete the ShelfLife from the database
		SQLExecute("EXEC UnlockShelfLife " & plShelfLifeID, dao.RecordsetOptionEnum.dbSQLPassThrough)
		SQLExecute("EXEC DeleteShelfLife " & plShelfLifeID, dao.RecordsetOptionEnum.dbSQLPassThrough, True)
		
		glShelfLifeID = plShelfLifeID
		
		'-- Refresh the grid and seek the new one of its place
		RefreshData(plShelfLifeID)
		
		'-- Check to see if it was deleted
		If glShelfLifeID = plShelfLifeID Then
            MsgBox("Unable to delete ShelfLife.", MsgBoxStyle.Exclamation, "Error!")
            logger.Info("Unable to delete ShelfLife.")
		End If

        logger.Debug("cmdDelete_Click Exit")
	End Sub
	
	Private Sub cmdUnlock_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdUnlock.Click

        logger.Debug("cmdUnlock_Click Entry")

		If MsgBox("Really unlock this record?", MsgBoxStyle.YesNo + MsgBoxStyle.Question, "Warning!") = MsgBoxResult.Yes Then

            logger.Info("Really unlock this record?-Yes")
			SQLExecute("EXEC UnlockShelfLife " & plShelfLifeID, dao.RecordsetOptionEnum.dbSQLPassThrough)
			RefreshData(plShelfLifeID)
			
        End If

        logger.Debug("cmdUnlock_Click Exit")
		
	End Sub
	
    Function SaveData() As Boolean

        logger.Debug("SaveData Entry")

        SaveData = True

        If plShelfLifeID = -1 Then Exit Function

        If pbDataChanged Then

            '-- Make sure ShelfLife name is not null
            If Trim(txtField(iShelfLifeShelfLife_Name).Text) = vbNullString Then
                If MsgBox("Changes will NOT be made if Shelf Life is left blank. " & Chr(13) & "Enter ShelfLife name?", MsgBoxStyle.Exclamation + MsgBoxStyle.YesNo, "ShelfLife Name Missing!") = MsgBoxResult.Yes Then
                    txtField(iShelfLifeShelfLife_Name).Focus()
                    SaveData = False
                Else
                    SQLExecute("EXEC UnlockShelfLife " & plShelfLifeID, DAO.RecordsetOptionEnum.dbSQLPassThrough)
                End If
                logger.Info("Changes will NOT be made if Shelf Life is left blank.--  SaveData = False")
                logger.Debug("SaveData Exit")
                Exit Function
            End If

            '-- Make sure they haven't duped a name
            SQLOpenRS(rsShelfLife, "EXEC CheckForDuplicateShelfLives " & plShelfLifeID & ", '" & txtField(iShelfLifeShelfLife_Name).Text & "'", DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
            If rsShelfLife.Fields("ShelfLifeCount").Value > 0 Then
                rsShelfLife.Close()
                If MsgBox("Duplicate ShelfLife Found, Changes will NOT be made if Shelf Life is a duplicate. " & Chr(13) & "Change ShelfLife Name?", MsgBoxStyle.Exclamation + MsgBoxStyle.YesNo, "ShelfLife Name Duplicate!") = MsgBoxResult.Yes Then
                    txtField(iShelfLifeShelfLife_Name).Focus()
                    SaveData = False
                Else
                    SQLExecute("EXEC UnlockShelfLife " & plShelfLifeID, DAO.RecordsetOptionEnum.dbSQLPassThrough)
                End If
                logger.Info("Duplicate ShelfLife Found, Changes will NOT be made if Shelf Life is a duplicate.--  SaveData = False")
                logger.Debug("SaveData Exit")
                Exit Function
            End If
            rsShelfLife.Close()

            '-- Make sure the losers don't make unwanted changes.
            If Not bProfessional Then
                If MsgBox("Save changes to Shelf Life?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, "Data Changed") = MsgBoxResult.No Then
                    SQLExecute("EXEC UnlockShelfLife " & plShelfLifeID, DAO.RecordsetOptionEnum.dbSQLPassThrough)
                    logger.Debug("SaveData Exit")
                    Exit Function
                End If
            End If

        End If

        '-- Everything is ok
        If pbDataChanged Then
            SQLExecute("EXEC UpdateShelfLifeInfo " & plShelfLifeID & ", '" & Trim(txtField(iShelfLifeShelfLife_Name).Text) & "'", DAO.RecordsetOptionEnum.dbSQLPassThrough)
        ElseIf Not lblReadOnly.Visible Then
            SQLExecute("EXEC UnlockShelfLife " & plShelfLifeID, DAO.RecordsetOptionEnum.dbSQLPassThrough)
        End If

        logger.Debug("SaveData Exit")

    End Function
	
	Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click
        logger.Debug("cmdExit_Click Entry")
        Me.Close()
        logger.Debug("cmdExit_Click Exit")
		
	End Sub
	
	Private Sub frmShelfLife_Activated(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Activated

        '-- Make sure there is data in the database
        'CheckNoShelfLives()

    End Sub
	
	Private Sub frmShelfLife_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
        logger.Debug("frmShelfLife_Load Entry")
		'-- Center the form
		CenterForm(Me)
		
        RefreshData(-1)

        logger.Debug("frmShelfLife_Load Exit")
		
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
	
    Sub CheckNoShelfLives()

        logger.Debug("CheckNoShelfLives Entry")

        '-- Make sure there is data for them to be in this form
        If plShelfLifeID = -1 Then
            If MsgBox("No Shelf Lives found in the database." & vbCrLf & "Would you like to add one?", MsgBoxStyle.YesNo + MsgBoxStyle.Question, "Notice!") = MsgBoxResult.Yes Then
                cmdAdd_Click(cmdAdd, New System.EventArgs())
                If plShelfLifeID = -1 Then
                    Me.Close()
                End If
                logger.Info("No Shelf Lives found in the database.")
            Else
                Me.Close()
            End If
        End If

        logger.Debug("CheckNoShelfLives Exit")


    End Sub
	
    Sub RefreshData(ByVal lRecord As Integer)

        logger.Debug("RefreshData Entry")

        Select Case lRecord
            Case -2 : SQLOpenRS(rsShelfLife, "EXEC GetShelfLifeInfoLast", DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
            Case -1 : SQLOpenRS(rsShelfLife, "EXEC GetShelfLifeInfoFirst", DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
            Case Else : SQLOpenRS(rsShelfLife, "EXEC GetShelfLifeInfo " & lRecord, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
        End Select

        If rsShelfLife.EOF Then
            plShelfLifeID = -1
            txtField(iShelfLifeShelfLife_Name).Text = ""
        Else
            plShelfLifeID = rsShelfLife.Fields("ShelfLife_ID").Value
            txtField(iShelfLifeShelfLife_Name).Text = rsShelfLife.Fields("ShelfLife_Name").Value & ""

            If IsDBNull(rsShelfLife.Fields("User_ID").Value) Then
                lblReadOnly.Visible = False
                cmdUnlock.Enabled = False
                SQLExecute("EXEC LockShelfLife " & plShelfLifeID & ", " & giUserID, DAO.RecordsetOptionEnum.dbSQLPassThrough)
            Else
                lblReadOnly.Text = "Read Only (" & GetInvUserName(rsShelfLife.Fields("User_ID").Value) & ")"
                lblReadOnly.Visible = True
                cmdUnlock.Enabled = (gbLockAdministrator Or giUserID = rsShelfLife.Fields("User_ID").Value)
            End If

        End If
        rsShelfLife.Close()

        SetActive(txtField(iShelfLifeShelfLife_Name), Not lblReadOnly.Visible)
        SetActive(cmdDelete, Not lblReadOnly.Visible)

        pbDataChanged = False

        If lRecord <> -1 And plShelfLifeID = -1 Then
            RefreshData(-1)
            'CheckNoShelfLives()
        ElseIf lRecord = -1 And plShelfLifeID = -1 Then
            CheckNoShelfLives()
        End If

        logger.Debug("RefreshData Exit")

    End Sub

    Private Sub frmShelfLife_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        e.Cancel = Not SaveData()

    End Sub
End Class