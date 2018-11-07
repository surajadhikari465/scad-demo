Option Strict Off
Option Explicit On
Imports log4net
Imports WholeFoods.Utility

Friend Class frmBrand
	Inherits System.Windows.Forms.Form
    Private IsInitializing As Boolean
	Dim rsBrand As dao.Recordset
	Dim pbDataChanged As Boolean
    Dim plBrandID As Integer
    Dim iconBrandId As Integer?

    ' Define the log4net logger for this class.
    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

	
	Private Sub cmdAdd_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdAdd.Click
        logger.Debug("cmdAdd_Click Entry")

		'-- if its not the first then update the data
		If plBrandID > -1 Then
			If Not lblReadOnly.Visible Then
				If Not SaveData Then Exit Sub
			End If
		End If
		
		glBrandID = 0
		
        '-- Call the adding form
        Dim fBrandAdd As New frmBrandAdd
        fBrandAdd.ShowDialog()
        fBrandAdd.Dispose()
		
		'-- a new Brand was added
		If glBrandID = -2 Then
			'-- Put the new data in
			RefreshData(-2)
			txtField(iBrandBrand_Name).Focus()
		ElseIf glBrandID > -1 Then 
			RefreshData(plBrandID)
        End If

        logger.Debug("cmdAdd_Click Exit")
	End Sub
	
	Private Sub cmdBrandSearch_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdBrandSearch.Click
        logger.Debug("cmdBrandSearch_Click Entry")

		'-- Do a validation event
		If Not lblReadOnly.Visible Then
			If Not SaveData Then Exit Sub
		End If
		
		'-- Set glBrandid to none found
		glBrandID = 0
		
		'-- Set the search type
		giSearchType = iSearchBrand
		
        '-- Open the search form
        Dim fSearch As New frmSearch
        fSearch.Text = "Search for Brand by Brand Name"
        fSearch.ShowDialog()
        fSearch.Dispose()
		
		'-- if its not zero, then something was found
		If glBrandID > 0 Then
			RefreshData(glBrandID)
		Else
			RefreshData(plBrandID)
        End If

        logger.Debug("cmdBrandSearch_Click Exit")
	End Sub
	
	Private Sub cmdDelete_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdDelete.Click
        logger.Debug("cmdDelete_Click Entry")

        ' Do not allow brand deletion if the brand has an IconBrandId.
        If iconBrandId.HasValue Then
            MessageBox.Show("This brand has an association to a global brand in Icon.  It cannot be deleted.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)
            logger.Info(String.Format("Detected an attempt to delete a brand with an Icon brand ID.  Brand_ID: {0}", plBrandID))
            Exit Sub
        End If

		'-- Make sure they really want to delete that Brand
		If MsgBox("Really delete Brand """ & Trim(txtField(iBrandBrand_Name).Text) & """?", MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton2, "Warning!") = MsgBoxResult.No Then
            logger.Info("Really delete Brand - No")
            logger.Debug(" cmdDelete_Click Exit")
            Exit Sub
		End If
		
		'-- Delete the Brand from the database
		On Error Resume Next
		SQLExecute("EXEC UnlockBrand " & plBrandID, dao.RecordsetOptionEnum.dbSQLPassThrough)
		SQLExecute("EXEC DeleteBrand " & plBrandID, dao.RecordsetOptionEnum.dbSQLPassThrough, True)
		On Error GoTo 0
		
		glBrandID = plBrandID
		
		'-- Refresh the grid and seek the new one of its place
		RefreshData(plBrandID)
		
		'-- Check to see if it was deleted
		If glBrandID = plBrandID Then
            MsgBox("Unable to delete Brand.", MsgBoxStyle.Exclamation, "Error!")
            logger.Error("Unable to delete Brand.")
        End If

        logger.Debug(" cmdDelete_Click Exit")
	End Sub
	
	Private Sub cmdUnlock_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdUnlock.Click
        logger.Debug("cmdUnlock_Click Entry")

        If MsgBox("Really unlock this record?", MsgBoxStyle.YesNo + MsgBoxStyle.Question, "Warning!") = MsgBoxResult.Yes Then
            logger.Info("Really unlock this record? - Yes")
            SQLExecute("EXEC UnlockBrand " & plBrandID, DAO.RecordsetOptionEnum.dbSQLPassThrough)
            RefreshData(plBrandID)
        End If

        logger.Debug("cmdUnlock_Click Exit")
	End Sub
	
    Function SaveData() As Boolean
        logger.Debug("SaveData Entry")

        SaveData = True

        If plBrandID = -1 Then Exit Function

        If pbDataChanged Then

            txtField(iBrandBrand_Name).Text = ConvertQuotes(Trim(txtField(iBrandBrand_Name).Text))

            '-- Make sure Brand name is not null
            If txtField(iBrandBrand_Name).Text = vbNullString Then
                If MsgBox("Changes will not be made if brand is left blank. " & Chr(13) & "Enter brand name?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, "Brand Name Missing!") = MsgBoxResult.Yes Then
                    txtField(iBrandBrand_Name).Focus()
                    SaveData = False
                Else
                    SQLExecute("EXEC UnlockBrand " & plBrandID, DAO.RecordsetOptionEnum.dbSQLPassThrough)
                End If
                logger.Info("Brand Name is blank")
                logger.Debug("SaveData Exit")
                Exit Function
            End If

            ' Do not allow brand name to be edited if this brand has an association in Icon.
            If iconBrandId.HasValue Then
                If MessageBox.Show("This brand has an association to a global brand in Icon.  It cannot be modified." + Environment.NewLine + Environment.NewLine +
                                                    "Continue without saving changes?", "Error", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) = Windows.Forms.DialogResult.Yes Then
                    logger.Info(String.Format("Detected an attempt to modify a brand with an Icon brand ID.  Brand_ID: {0}", plBrandID))
                    SQLExecute("EXEC UnlockBrand " & plBrandID, DAO.RecordsetOptionEnum.dbSQLPassThrough)
                    Return True
                Else
                    logger.Info(String.Format("Detected an attempt to modify a brand with an Icon brand ID.  Brand_ID: {0}", plBrandID))
                    Return False
                End If
            End If

            '-- Make sure they haven't duped a name
            SQLOpenRS(rsBrand, "EXEC CheckForDuplicateBrands " & plBrandID & ", '" & txtField(iBrandBrand_Name).Text & "'", DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
            If rsBrand.Fields("BrandCount").Value > 0 Then
                rsBrand.Close()
                rsBrand = Nothing
                If MsgBox("Duplicate Brand Found, Changes will NOT be made if Brand is a duplicate. " & Chr(13) & "Change Brand Name?", MsgBoxStyle.Exclamation + MsgBoxStyle.YesNo, "Brand Name Duplicate!") = MsgBoxResult.Yes Then
                    txtField(iBrandBrand_Name).Focus()
                    SaveData = False
                Else
                    SQLExecute("EXEC UnlockBrand " & plBrandID, DAO.RecordsetOptionEnum.dbSQLPassThrough)
                End If
                logger.Info("Duplicate Brand Names.")
                logger.Debug("SaveData Exit")
                Exit Function
            End If
            rsBrand.Close()
            rsBrand = Nothing

            '-- Make sure the losers don't make unwanted changes.
            If Not bProfessional Then
                If MsgBox("Save changes to Brand?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, "Data Changed") = MsgBoxResult.No Then
                    SQLExecute("EXEC UnlockBrand " & plBrandID, DAO.RecordsetOptionEnum.dbSQLPassThrough)
                    logger.Info("Save changes to Brand?-No")
                    logger.Debug("SaveData Exit")
                    Exit Function
                End If
            End If

        End If

        '-- Everything is ok
        If pbDataChanged Then
            SQLExecute("EXEC UpdateBrandInfo " & plBrandID & ", '" & txtField(iBrandBrand_Name).Text & "'", DAO.RecordsetOptionEnum.dbSQLPassThrough)
        ElseIf Not lblReadOnly.Visible Then
            SQLExecute("EXEC UnlockBrand " & plBrandID, DAO.RecordsetOptionEnum.dbSQLPassThrough)
        End If

        logger.Debug("SaveData Exit")

    End Function
	
	Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click
        logger.Debug("cmdExit_Click Entry")
		Me.Close()
        logger.Debug("cmdExit_Click Exit")
	End Sub
	
	Private Sub frmBrand_Activated(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Activated
        RemoveHandler Me.Activated, AddressOf frmBrand_Activated

		'-- Make sure there is data in the database
        CheckNoBrands()
	End Sub

    Private Sub frmBrand_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
        logger.Debug("frmBrand_Load Entry")

        CenterForm(Me)
        Dim bDisableBrandAdditions As Boolean = ConfigurationServices.AppSettings("DisableBrandAdditions")
        SetActive(cmdAdd, (gbSuperUser Or gbItemAdministrator), bDisableBrandAdditions)

        RefreshData(-1)

        logger.Debug("frmBrand_Load Exit")
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
	
    Sub CheckNoBrands()
        logger.Debug("CheckNoBrands Entry")

        '-- Make sure there is data for them to be in this form
        If plBrandID = -1 Then
            If MsgBox("No Brands found in the database." & vbCrLf & "Would you like to add one?", MsgBoxStyle.YesNo + MsgBoxStyle.Question, "Notice!") = MsgBoxResult.Yes Then
                cmdAdd_Click(cmdAdd, New System.EventArgs())
                If plBrandID = -1 Then
                    Me.Close()
                End If
            Else
                Me.Close()
            End If
        End If

        logger.Debug("CheckNoBrands Exit")
    End Sub
	
    Sub RefreshData(ByVal lRecord As Integer)
        logger.Debug("RefreshData Entry")

        Select Case lRecord
            Case -2 : SQLOpenRS(rsBrand, "EXEC GetBrandInfoLast " & giUserID, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
            Case -1 : SQLOpenRS(rsBrand, "EXEC GetBrandInfoFirst " & giUserID, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
            Case Else : SQLOpenRS(rsBrand, "EXEC GetBrandInfo " & lRecord & ", " & giUserID, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
        End Select

        If rsBrand.EOF Then
            plBrandID = -1
            txtField(iBrandBrand_Name).Text = String.Empty
        Else
            plBrandID = rsBrand.Fields("Brand_ID").Value
            iconBrandId = If(IsDBNull(rsBrand.Fields("IconBrandId").Value), Nothing, rsBrand.Fields("IconBrandId").Value)
            txtField(iBrandBrand_Name).Text = rsBrand.Fields("Brand_Name").Value

            If IsDBNull(rsBrand.Fields("User_ID").Value) Then
                lblReadOnly.Visible = False
                cmdUnlock.Enabled = False
            Else
                lblReadOnly.Text = "Read Only (" & rsBrand.Fields("UserName").Value & ")"
                lblReadOnly.Visible = True
                cmdUnlock.Enabled = (gbLockAdministrator Or giUserID = rsBrand.Fields("User_ID").Value)
            End If

        End If
        rsBrand.Close()
        rsBrand = Nothing

        SetActive(txtField(iBrandBrand_Name), (Not lblReadOnly.Visible) And (gbSuperUser Or gbItemAdministrator))
        SetActive(cmdDelete, (Not lblReadOnly.Visible) And (gbSuperUser Or gbItemAdministrator))

        pbDataChanged = False

        If lRecord <> -1 And plBrandID = -1 Then
            RefreshData(-1)
            CheckNoBrands()
        End If

        logger.Debug("RefreshData Exit")
    End Sub

    Private Sub frmBrand_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        logger.Debug("frmBrand_FormClosing Entry")
        e.Cancel = Not SaveData()
        logger.Debug("frmBrand_FormClosing Exit")
    End Sub
End Class