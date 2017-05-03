Option Strict Off
Option Explicit On

Imports log4net
Friend Class frmCategory
	Inherits System.Windows.Forms.Form
    Private IsInitializing As Boolean
	Dim rsCategory As dao.Recordset
	Dim pbDataChanged As Boolean
    Dim plCategoryID As Integer

    ' Define the log4net logger for this class.
    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

	
	Private Sub cmbSubTeam_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmbSubTeam.SelectedIndexChanged

        pbDataChanged = True

    End Sub
	
	Private Sub cmdAdd_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdAdd.Click

        logger.Debug("cmdAdd_Click Entry")

		'-- if its not the first then update the data
		If plCategoryID > -1 Then
			If Not lblReadOnly.Visible Then
				If Not SaveData Then Exit Sub
			End If
		End If
		
		glCategoryID = 0
		
        '-- Call the adding form
        frmCategoryAdd.ShowDialog()
        frmCategoryAdd.Dispose()
		
		'-- a new Category was added
		If glCategoryID = -2 Then
			'-- Put the new data in
			RefreshData(-2)
			txtField.Focus()
		ElseIf plCategoryID <> -1 Then 
			RefreshData(plCategoryID)
        End If

        logger.Debug("cmdAdd_Click Exit")

		
	End Sub
	
	Private Sub cmdCategorySearch_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdCategorySearch.Click


        logger.Debug("cmdCategorySearch_Click Entry")


		'-- Do a validation event
		If Not lblReadOnly.Visible Then
			If Not SaveData Then Exit Sub
		End If
		
		'-- Set glCategoryid to none found
		glCategoryID = 0
		
		'-- Set the search type
		giSearchType = iSearchCategory
		
        '-- Open the search form
        Dim fSearch As New frmSearch
        fSearch.Text = "Search for Name by Class Name"
        fSearch.ShowDialog()
        fSearch.Dispose()
		
		'-- if its not zero, then something was found
		If glCategoryID > 0 Then
			RefreshData(glCategoryID)
		Else
			RefreshData(plCategoryID)
        End If

        logger.Debug("cmdCategorySearch_Click Exit")

		
	End Sub
	
	Private Sub cmdDelete_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdDelete.Click

        logger.Debug("cmdDelete_Click Entry")

		'-- Make sure there is a record to delete
		If plCategoryID < 1 Then
            MsgBox("There is not a current record to delete.", MsgBoxStyle.Exclamation, "Error!")

            logger.Info("There is not a current record to delete.")
            logger.Debug("cmdDelete_Click Exit")
			Exit Sub
		End If
		
		'-- Make sure they really want to delete that Category
        If MsgBox("Really delete Class """ & Trim(txtField.Text) & """?", MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton2, "Warning!") = MsgBoxResult.No Then
            logger.Info(" Really delete Class -- No")
            logger.Debug("cmdDelete_Click Exit")
            Exit Sub
        End If
		
		'-- Delete the Category from the database
		SQLExecute("EXEC UnlockCategory " & plCategoryID, dao.RecordsetOptionEnum.dbSQLPassThrough)
		SQLExecute("EXEC DeleteCategory " & plCategoryID, dao.RecordsetOptionEnum.dbSQLPassThrough, True)
		
		glCategoryID = plCategoryID
		
		'-- Refresh the grid and seek the new one of its place
		RefreshData(plCategoryID)
		
		'-- Check to see if it was deleted
		If glCategoryID = plCategoryID Then
            MsgBox("Unable to delete Class.", MsgBoxStyle.Exclamation, "Error!")
            logger.Info("Unable to delete Class.")
        End If

        logger.Debug("cmdDelete_Click Exit")
		
	End Sub
	
	Private Sub cmdUnlock_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdUnlock.Click

        logger.Debug("cmdUnlock_Click Entry")

		If MsgBox("Really unlock this record?", MsgBoxStyle.YesNo + MsgBoxStyle.Question, "Warning!") = MsgBoxResult.Yes Then

            logger.Info("Really unlock this record?-Yes")
			SQLExecute("EXEC UnlockCategory " & plCategoryID, dao.RecordsetOptionEnum.dbSQLPassThrough)
			RefreshData(plCategoryID)
			
        End If

        logger.Debug("cmdUnlock_Click Exit")

		
	End Sub
	
    Function SaveData() As Boolean

        logger.Debug("SaveData Entry")


        SaveData = True

        If plCategoryID = -1 Then Exit Function

        If pbDataChanged Then

            '-- Make sure Category name is not null
            If Trim(txtField.Text) = "" Then
                If MsgBox("Changes will NOT be made if Class is left blank. " & Chr(13) & "Enter Class name?", MsgBoxStyle.Exclamation + MsgBoxStyle.YesNo, "Class Name Missing!") = MsgBoxResult.Yes Then
                    txtField.Focus()
                    SaveData = False
                Else
                    SQLExecute("EXEC UnlockCategory " & plCategoryID, DAO.RecordsetOptionEnum.dbSQLPassThrough)
                End If
                Exit Function
            End If

            '-- Make sure they haven't duped a name
            SQLOpenRS(rsCategory, "EXEC CheckForDuplicateCategories " & plCategoryID & ", '" & txtField.Text & "'," & ComboValue(cmbSubTeam), DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
            If rsCategory.Fields("CategoryCount").Value > 0 Then
                rsCategory.Close()
                If MsgBox("Duplicate Class Found, Changes will NOT be made if Class is a duplicate. " & Chr(13) & "Change Class Name?", MsgBoxStyle.Exclamation + MsgBoxStyle.YesNo, "Class Name Duplicate!") = MsgBoxResult.Yes Then
                    txtField.Focus()
                    SaveData = False
                Else
                    SQLExecute("EXEC UnlockCategory " & plCategoryID, DAO.RecordsetOptionEnum.dbSQLPassThrough)
                End If
                Exit Function
            End If
            rsCategory.Close()
            rsCategory = Nothing

            '-- Make sure the losers don't make unwanted changes.
            If Not bProfessional Then
                If MsgBox("Save changes to Class?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, "Data Changed") = MsgBoxResult.No Then
                    SQLExecute("EXEC UnlockCategory " & plCategoryID, DAO.RecordsetOptionEnum.dbSQLPassThrough)
                    Exit Function
                End If
            End If

        End If

        '-- Everything is ok
        If pbDataChanged Then
            SQLExecute("EXEC UpdateCategoryInfo " & plCategoryID & ", '" & ConvertQuotes(Trim(txtField.Text)) & "'," & ComboValue(cmbSubTeam), DAO.RecordsetOptionEnum.dbSQLPassThrough)
        ElseIf Not lblReadOnly.Visible Then
            SQLExecute("EXEC UnlockCategory " & plCategoryID, DAO.RecordsetOptionEnum.dbSQLPassThrough)
        End If

        logger.Debug("SaveData Exit")


    End Function
	
	Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click
        logger.Debug("cmdExit_Click Entry")
        Me.Close()
        logger.Debug("cmdExit_Click Exit")
		
	End Sub
	
	Private Sub frmCategory_Activated(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Activated

        '-- Make sure there is data in the database
        CheckNoCategories()

    End Sub

    Private Sub frmCategory_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        e.Cancel = Not SaveData()

    End Sub
	
	Private Sub frmCategory_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load

        logger.Debug("frmCategory_Load Entry")

		'-- Center the form
		CenterForm(Me)
		
        SetActive(cmdAdd, gbSuperUser Or gbItemAdministrator)
		
		LoadAllSubTeams(cmbSubTeam)
		
        RefreshData(-1)

        logger.Debug("frmCategory_Load Exit")

		
	End Sub
	

	Private Sub txtField_TextChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles txtField.TextChanged
        If Me.IsInitializing Then Exit Sub
        pbDataChanged = True

    End Sub
	
	Private Sub txtField_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles txtField.KeyPress

        logger.Debug("txtField_KeyPress Entry")

        Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
		
		If Not txtField.ReadOnly Then
			KeyAscii = ValidateKeyPressEvent(KeyAscii, (txtField.Tag), txtField, 0, 0, 0)
		End If
		
		eventArgs.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			eventArgs.Handled = True
        End If

        logger.Debug("txtField_KeyPress Exit")

	End Sub
	
	Sub CheckNoCategories()

        logger.Debug("CheckNoCategories Entry")
		'-- Make sure there is data for them to be in this form
		If plCategoryID = -1 Then
            If MsgBox("No Classes found in the database." & vbCrLf & "Would you like to add one?", MsgBoxStyle.YesNo + MsgBoxStyle.Question, "Notice!") = MsgBoxResult.Yes Then
                cmdAdd_Click(cmdAdd, New System.EventArgs())
                If plCategoryID = -1 Then
                    Me.Close()
                End If
            Else
                Me.Close()
            End If
        End If
        logger.Debug("CheckNoCategories Exit")
		
	End Sub
	
    Sub RefreshData(ByVal lRecord As Integer)
        logger.Debug("RefreshData Entry")

        Select Case lRecord
            Case -2 : SQLOpenRS(rsCategory, "EXEC GetCategoryInfoLast", DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
            Case -1 : SQLOpenRS(rsCategory, "EXEC GetCategoryInfoFirst", DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
            Case Else : SQLOpenRS(rsCategory, "EXEC GetCategoryInfo " & lRecord, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
        End Select

        If rsCategory.EOF Then
            plCategoryID = -1
            txtField.Text = ""
        Else
            plCategoryID = rsCategory.Fields("Category_ID").Value
            txtField.Text = rsCategory.Fields("Category_Name").Value & ""
            SetCombo(cmbSubTeam, rsCategory.Fields("SubTeam_No").Value)

            If IsDBNull(rsCategory.Fields("User_ID").Value) Then
                lblReadOnly.Visible = False
                cmdUnlock.Enabled = False
                SQLExecute("EXEC LockCategory " & plCategoryID & ", " & giUserID, DAO.RecordsetOptionEnum.dbSQLPassThrough)
            Else
                lblReadOnly.Text = "Read Only (" & GetInvUserName(rsCategory.Fields("User_ID").Value) & ")"
                lblReadOnly.Visible = True
                cmdUnlock.Enabled = (gbLockAdministrator Or gbItemAdministrator Or giUserID = rsCategory.Fields("User_ID").Value)
            End If

        End If
        rsCategory.Close()
        rsCategory = Nothing

        SetActive(txtField, (Not lblReadOnly.Visible) And (gbSuperUser Or gbItemAdministrator))
        SetActive(cmdAdd, gbSuperUser Or gbItemAdministrator)
        SetActive(cmdDelete, (Not lblReadOnly.Visible) And (gbSuperUser Or gbItemAdministrator))
        SetActive(cmbSubTeam, (Not lblReadOnly.Visible) And (gbSuperUser Or gbItemAdministrator))

        pbDataChanged = False

        If lRecord <> -1 And plCategoryID = -1 Then
            RefreshData(-1)
            CheckNoCategories()
        End If

        logger.Debug("RefreshData Exit")

    End Sub


End Class