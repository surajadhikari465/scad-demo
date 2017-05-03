Option Strict Off
Option Explicit On

Imports log4net
Friend Class frmConversionAdd
    Inherits System.Windows.Forms.Form

    ' Define the log4net logger for this class.
    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)


    Dim bComplete As Boolean
	
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

        Dim rsConversion As DAO.Recordset = Nothing
        Dim temp As String = String.Empty

        '-- Check to see if anything was entered
        temp = txtField(iConversionConversionFactor).Text
        If (temp = Nothing Or temp.Trim() = "") Then
            temp = "0"
        End If
        If CDec(temp) = 0 Then
            MsgBox("Conversion factor cannot be left blank or zero value.", MsgBoxStyle.Exclamation, "Error!")
            txtField(iConversionConversionFactor).Focus()
            logger.Info("Conversion factor cannot be left blank or zero value.")
            logger.Debug("cmdAdd_Click Exit")
            Exit Sub
        End If

        If cmbField(iConversionFromUnit_ID).SelectedIndex = -1 Then
            MsgBox("From Unit cannot be left blank.", MsgBoxStyle.Exclamation, "Error!")
            cmbField(iConversionFromUnit_ID).Focus()
            logger.Info("From Unit cannot be left blank.")
            logger.Debug("cmdAdd_Click Exit")
            Exit Sub
        End If

        If cmbField(iConversionToUnit_ID).SelectedIndex = -1 Then
            MsgBox("To Unit cannot be left blank.", MsgBoxStyle.Exclamation, "Error!")
            cmbField(iConversionToUnit_ID).Focus()
            logger.Info("To Unit cannot be left blank.")
            logger.Debug("cmdAdd_Click Exit")
            Exit Sub
        End If

        If cmbField(iConversionToUnit_ID).SelectedIndex = cmbField(iConversionFromUnit_ID).SelectedIndex Then
            MsgBox("A Unit cannot be converted to itself.", MsgBoxStyle.Exclamation, "Error!")
            logger.Info("A Unit cannot be converted to itself.")
            logger.Debug("cmdAdd_Click Exit")
            cmbField(iConversionToUnit_ID).Focus()
            Exit Sub
        End If

        '-- Check to see if the name already exists
        Try
            rsConversion = SQLOpenRecordSet("EXEC CheckForDuplicateConversions " & VB6.GetItemData(cmbField(iConversionFromUnit_ID), cmbField(iConversionFromUnit_ID).SelectedIndex) & ", " & VB6.GetItemData(cmbField(iConversionToUnit_ID), cmbField(iConversionToUnit_ID).SelectedIndex), DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
            If rsConversion.Fields("ConversionCount").Value > 0 Then
                rsConversion.Close()
                MsgBox("Unit conversion already exists.", MsgBoxStyle.Exclamation, "Error!")

                cmbField(iConversionToUnit_ID).Focus()
                logger.Info("Unit conversion already exists.")
                logger.Debug("cmdAdd_Click Exit")
                Exit Sub
            End If
        Finally
            If rsConversion IsNot Nothing Then
                rsConversion.Close()
            End If
        End Try

        '-- Set back the current pointers
        glFromUnitID = VB6.GetItemData(cmbField(iConversionFromUnit_ID), cmbField(iConversionFromUnit_ID).SelectedIndex)
        glToUnitID = VB6.GetItemData(cmbField(iConversionToUnit_ID), cmbField(iConversionToUnit_ID).SelectedIndex)

        '-- Add the new record
        SQLExecute("EXEC InsertConversion " & glFromUnitID & ", " & glToUnitID & ", '" & cmbField(iConversionConversionSymbol).Text & "', " & txtField(iConversionConversionFactor).Text, DAO.RecordsetOptionEnum.dbSQLPassThrough)
        bComplete = True

        '-- Go back to the previous form
        Me.Close()

        logger.Debug("cmdAdd_Click Exit")

    End Sub
	
	Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click
        logger.Debug("cmdExit_Click Entry")
        Me.Close()
        logger.Debug("cmdExit_Click Exit")
		
	End Sub
	
	Private Sub frmConversionAdd_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
        logger.Debug("frmConversionAdd_Load Entry")

        Dim iIndex As Integer
        '-- Center the form
		CenterForm(Me)

		'-- Put units into the combo fields
		LoadUnit(cmbField(iConversionFromUnit_ID))
		LoadUnit(cmbField(iConversionToUnit_ID))
		
		If glToUnitID <> 0 Then
			For iIndex = 0 To cmbField(iConversionFromUnit_ID).Items.Count - 1
				If VB6.GetItemData(cmbField(iConversionFromUnit_ID), iIndex) = glFromUnitID Then
					cmbField(iConversionFromUnit_ID).SelectedIndex = iIndex
					Exit For
				End If
			Next iIndex
            SetActive(cmbField(iConversionFromUnit_ID), False)

			For iIndex = 0 To cmbField(iConversionToUnit_ID).Items.Count - 1
				If VB6.GetItemData(cmbField(iConversionToUnit_ID), iIndex) = glToUnitID Then
					cmbField(iConversionToUnit_ID).SelectedIndex = iIndex
					Exit For
				End If
            Next iIndex
            SetActive(cmbField(iConversionToUnit_ID), False)

		Else
			cmbField(iConversionFromUnit_ID).SelectedIndex = 0
			cmbField(iConversionToUnit_ID).SelectedIndex = 0
		End If
		
        cmbField(iConversionConversionSymbol).SelectedIndex = 0

        logger.Debug("frmConversionAdd_Load Exit")
		
	End Sub
	
	Private Sub frmConversionAdd_FormClosed(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        logger.Debug("frmConversionAdd_FormClosed Entry")

		If Not bComplete Then
			glToUnitID = 0
			glFromUnitID = 0
        End If

        logger.Debug("frmConversionAdd_FormClosed Exit")
		
	End Sub
	
	Private Sub txtField_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles txtField.KeyPress
        logger.Debug("txtField_KeyPress Entry")

        Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
		Dim Index As Short = txtField.GetIndex(eventSender)
		
		If KeyAscii = 13 Then
			KeyAscii = 0
			System.Windows.Forms.SendKeys.Send("{TAB}")
		End If
		
		If Not txtField(Index).ReadOnly Then
			
			KeyAscii = ValidateKeyPressEvent(KeyAscii, txtField(Index).Tag, txtField(Index), 0, 0, 0)
			
		End If
		
		eventArgs.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			eventArgs.Handled = True
        End If

        logger.Debug("txtField_KeyPress Exit")
    End Sub

End Class