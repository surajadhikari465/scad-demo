Option Strict Off
Option Explicit On

Imports log4net
Friend Class frmUnitAdd
    Inherits System.Windows.Forms.Form
    ' Define the log4net logger for this class.
    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

	
	Private Sub cmdAdd_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdAdd.Click

        logger.Debug("cmdAdd_Click Entry")

        Dim rsUnit As DAO.Recordset = Nothing
		
		'-- Take out unwanted spaces
		txtUnit_Name.Text = Trim(txtUnit_Name.Text)
		
		'-- Check to see if anything was entered
		If txtUnit_Name.Text = vbNullString Then
            MsgBox("Unit name cannot be left blank.", MsgBoxStyle.Exclamation, "Error!")
            logger.Info("Unit name cannot be left blank.")
            txtUnit_Name.Focus()
            logger.Debug("cmdAdd_Click Exit")
			Exit Sub
		End If
		
        '-- Check to see if the name already exists
        Try
            rsUnit = SQLOpenRecordSet("EXEC CheckForDuplicateUnits 0, '" & txtUnit_Name.Text & "'", DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
            If rsUnit.Fields("UnitCount").Value > 0 Then
                rsUnit.Close()
                MsgBox("Unit name already exists.", MsgBoxStyle.Exclamation, "Error!")
                logger.Info("Unit name already exists.")
                txtUnit_Name.Focus()
                logger.Debug("cmdAdd_Click Exit")
                Exit Sub
            End If
        Finally
            If rsUnit IsNot Nothing Then
                rsUnit.Close()
            End If
        End Try

        '-- Add the new record
        SQLExecute("EXEC InsertItemUnit '" & txtUnit_Name.Text & "'", DAO.RecordsetOptionEnum.dbSQLPassThrough)
        glUnitID = -2

        '-- Go back to the previous form
        Me.Close()

        logger.Debug("cmdAdd_Click Exit")

    End Sub
	
	Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click
        logger.Debug("cmdExit_Click Entry")

        '-- Don't Add a vendor
        Me.Close()

        logger.Debug("cmdExit_Click Exit")

		
	End Sub
	
	Private Sub frmUnitAdd_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load

        logger.Debug("frmUnitAdd_Load Entry")
        '-- Center the form

        CenterForm(Me)

        logger.Debug("frmUnitAdd_Load Exit")

		
	End Sub
	
	Private Sub txtUnit_Name_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles txtUnit_Name.KeyPress

        logger.Debug("txtUnit_Name_KeyPress Entry")

        Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
		
		KeyAscii = ValidateKeyPressEvent(KeyAscii, "String", txtUnit_Name, 0, 0, 0)
		
		eventArgs.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			eventArgs.Handled = True
        End If

        logger.Debug("txtUnit_Name_KeyPress Exit")

	End Sub
End Class