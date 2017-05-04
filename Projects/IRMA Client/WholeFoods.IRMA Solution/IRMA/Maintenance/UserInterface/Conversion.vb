Option Strict Off
Option Explicit On

Imports log4net
Friend Class frmConversion
    Inherits System.Windows.Forms.Form

    Private IsInitializing As Boolean
	Dim rsConversion As dao.Recordset
	Dim pbDataChanged As Boolean
	Dim plFromUnitID As Integer
    Dim plToUnitID As Integer

    ' Define the log4net logger for this class.
    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)


    Private Sub cmbField_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmbField.SelectedIndexChanged, _cmbField_2.SelectedIndexChanged, _cmbField_1.SelectedIndexChanged, _cmbField_0.SelectedIndexChanged
        If Me.IsInitializing Then Exit Sub

        pbDataChanged = True
    End Sub
	
    Private Sub cmbField_KeyPress(ByVal eventSender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbField.KeyPress

        logger.Debug("cmbField_KeyPress Entry")

        Dim KeyAscii As Short = Asc(e.KeyChar)
        Dim Index As Short = cmbField.GetIndex(eventSender)

        If KeyAscii = 8 Then
            cmbField(Index).SelectedIndex = -1
        End If

        e.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            e.Handled = True
        End If

        logger.Debug("cmbField_KeyPress Exit")

    End Sub
	
	Private Sub cmdAdd_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdAdd.Click

        logger.Debug("cmdAdd_Click Entry")

		'-- if its not the first then update the data
		If plFromUnitID > -1 Then
			If Not SaveData Then Exit Sub
		End If
		
		glFromUnitID = 0
		glToUnitID = 0
		
        '-- Call the adding form
        Dim fConversionAdd As New frmConversionAdd
        fConversionAdd.ShowDialog()
        fConversionAdd.Dispose()
		
		'-- a new Brand was added
		If glFromUnitID <> 0 Then
			'-- Put the new data in
			RefreshData(glFromUnitID, glToUnitID)
			'-- go to enter the other new data
			cmbField(iConversionConversionSymbol).Focus()
		ElseIf plFromUnitID <> -1 Then 
			RefreshData(plFromUnitID, plToUnitID)
        End If

        logger.Debug("cmdAdd_Click Exit")

		
	End Sub
	
	Private Sub cmdBrandSearch_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdBrandSearch.Click


        logger.Debug("cmdBrandSearch_Click Entry")

        Dim Index As Short = cmdBrandSearch.GetIndex(eventSender)
		
		'-- if its not the first then update the data
		If Not SaveData Then Exit Sub
		
		'-- Set glOriginid to none found
		glFromUnitID = 0
		glToUnitID = 0
		
		'-- Set the search type
		giSearchType = Index
        Dim fSearch As New frmSearch
		'-- Open the search form
		Select Case Index
            Case isearchFromUnit : fSearch.Text = "Search for Unit Conversion by From Unit"
            Case iSearchToUnit : fSearch.Text = "Search for Unit Conversion by To Unit"
        End Select
        fSearch.ShowDialog()
        fSearch.Dispose()
		
		'-- a new Brand was added
		If glFromUnitID <> 0 Then
			'-- Put the new data in
			RefreshData(glFromUnitID, glToUnitID)
			'-- go to enter the other new data
			cmbField(iConversionConversionSymbol).Focus()
		ElseIf plFromUnitID <> -1 Then 
			RefreshData(plFromUnitID, plToUnitID)
        End If

        logger.Debug("cmdBrandSearch_Click Exit")

		
	End Sub
	
	Private Sub cmdDelete_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdDelete.Click

        logger.Debug("cmdDelete_Click Entry")

		'-- Make sure they really want to delete that Brand
		If MsgBox("Really delete this conversion?", MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton2, "Warning!") = MsgBoxResult.No Then
            logger.Info("Really delete this conversion?--NO")
            logger.Debug("cmdDelete_Click Exit")
            Exit Sub
		End If
		
		'-- Delete the Brand from the database
		SQLExecute("EXEC DeleteConversion " & plFromUnitID & ", " & plToUnitID, dao.RecordsetOptionEnum.dbSQLPassThrough)
		
		'-- Refresh the grid and seek the new one of its place
        RefreshData(-1, -1)

        logger.Debug("cmdDelete_Click Exit")
		
	End Sub
	
	Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click
		
		'-- Quit this form
		Me.Close()
		
	End Sub
	
    Function SaveData() As Boolean

        logger.Debug("SaveData Entry")


        SaveData = True

        If plToUnitID = -1 Then Exit Function

        If pbDataChanged Then

            '-- Make sure Category name is not null
            If CDec(txtField(iConversionConversionFactor).Text) = 0 Then
                If MsgBox("Changes will NOT be made if Conversion Factor is left blank or zero value. " & Chr(13) & "Enter Conversion Factor?", MsgBoxStyle.Exclamation + MsgBoxStyle.YesNo, "Factor Missing!") = MsgBoxResult.Yes Then
                    SaveData = False
                    txtField(iConversionConversionFactor).Focus()
                End If
                logger.Info("Changes will NOT be made if Conversion Factor is left blank - SaveData False")
                logger.Debug("SaveData Exit")
                Exit Function
            End If

            '-- Make sure the losers don't make unwanted changes.
            If Not bProfessional Then
                If MsgBox("Save changes to unit conversion?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, "Data Changed") = MsgBoxResult.No Then
                    logger.Debug("Save changes to unit conversion?--No")
                    logger.Debug("SaveData Exit")
                    Exit Function
                End If
            End If

            SQLExecute("EXEC UpdateConversionInfo " & plFromUnitID & ", " & plToUnitID & ", '" & cmbField(iConversionConversionSymbol).Text & "', " & txtField(iConversionConversionFactor).Text, DAO.RecordsetOptionEnum.dbSQLPassThrough)

        End If

        logger.Debug("SaveData Exit")


    End Function
	
	Private Sub frmConversion_Activated(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Activated

        '-- Check to see if there is any data
        ' CheckNoConversions()

    End Sub
	
	Private Sub frmConversion_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load

        logger.Debug("frmConversion_Load Entry")
		'-- Center the form
        CenterForm(Me)

		'-- Put units into the combo fields
		LoadUnit(cmbField(iConversionFromUnit_ID))
		ReplicateCombo(cmbField(iConversionFromUnit_ID), cmbField(iConversionToUnit_ID))
		
        RefreshData(-1, -1)

        logger.Debug("frmConversion_Load Exit")

		
	End Sub
	
    Sub CheckNoConversions()

        logger.Debug("CheckNoConversions Entry")


        '-- Make sure there is data for them to be in this form
        If plFromUnitID = -1 Then
            If MsgBox("No conversion tables found in the database." & vbCrLf & "Would you like to add one?", MsgBoxStyle.YesNo + MsgBoxStyle.Question, "Notice!") = MsgBoxResult.Yes Then
                cmdAdd_Click(cmdAdd, New System.EventArgs())
                If plFromUnitID = -1 Then
                    Me.Close()
                End If
            Else
                Me.Close()
            End If
        End If

        logger.Debug("CheckNoConversions Exit")

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
		
		If KeyAscii = 13 Then
			KeyAscii = 0
			System.Windows.Forms.SendKeys.Send("{TAB}")
		Else
			KeyAscii = ValidateKeyPressEvent(KeyAscii, txtField(Index).Tag, txtField(Index), 0, 0, 0)
		End If
		
		eventArgs.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			eventArgs.Handled = True
        End If

        logger.Debug("txtField_KeyPress Exit")

	End Sub
	
	Sub RefreshData(ByRef lFromUnitID As Integer, ByRef lToUnitID As Integer)

        logger.Debug("RefreshData Entry")
		Dim nCount As Short
		
		If lFromUnitID = -1 Then
			SQLOpenRS(rsConversion, "EXEC GetConversionInfoFirst", dao.RecordsetTypeEnum.dbOpenSnapshot, dao.RecordsetOptionEnum.dbSQLPassThrough)
		Else
			SQLOpenRS(rsConversion, "EXEC GetConversionInfo " & lFromUnitID & ", " & lToUnitID, dao.RecordsetTypeEnum.dbOpenSnapshot, dao.RecordsetOptionEnum.dbSQLPassThrough)
		End If
		
		If rsConversion.EOF Then
			plFromUnitID = -1
			plToUnitID = -1
		Else
			plFromUnitID = rsConversion.Fields("FromUnit_ID").Value
			plToUnitID = rsConversion.Fields("ToUnit_ID").Value
            SetCombo(cmbField(iConversionFromUnit_ID), CInt(rsConversion.Fields("FromUnit_ID").Value))
            SetCombo(cmbField(iConversionToUnit_ID), CInt(rsConversion.Fields("ToUnit_ID").Value))
            SetActive(cmbField(iConversionFromUnit_ID), False)
            SetActive(cmbField(iConversionToUnit_ID), False)

            txtField(iConversionConversionFactor).Text = rsConversion.Fields("ConversionFactor").Value.ToString
			'-- Enter the data
			For nCount = 0 To cmbField(iConversionConversionSymbol).Items.Count - 1
				If VB6.GetItemString(cmbField(iConversionConversionSymbol), nCount) = rsConversion.Fields("ConversionSymbol").Value Then
                    cmbField(iConversionConversionSymbol).SelectedIndex = nCount
                    Exit For
				End If
			Next nCount
		End If
        rsConversion.Close()
        rsConversion = Nothing
		
		pbDataChanged = False
		
		If lFromUnitID <> -1 And plFromUnitID = -1 Then
			RefreshData(-1, -1)
            CheckNoConversions()
        ElseIf lFromUnitID = -1 And plFromUnitID = -1 Then
            CheckNoConversions()
        End If

        logger.Debug("RefreshData Exit")
		
	End Sub

    Private Sub frmConversion_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        e.Cancel = Not SaveData()

    End Sub
End Class