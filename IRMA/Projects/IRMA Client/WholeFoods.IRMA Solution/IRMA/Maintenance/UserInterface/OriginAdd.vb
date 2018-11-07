Option Strict Off
Option Explicit On
Imports log4net

Friend Class frmOriginAdd
    Inherits System.Windows.Forms.Form

    ' Define the log4net logger for this class.
    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

	
	Private Sub cmdAdd_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdAdd.Click

        logger.Debug("cmdAdd_Click Entry")

        Dim rsOrigin As DAO.Recordset = Nothing
		
		'-- Take out unwanted spaces
		txtOrigin_Name.Text = Trim(txtOrigin_Name.Text)
		
		'-- Check to see if anything was entered
		If txtOrigin_Name.Text = vbNullString Then
            MsgBox("Origin name cannot be left blank.", MsgBoxStyle.Exclamation, "Error!")
            logger.Info("Origin name cannot be left blank.")
            txtOrigin_Name.Focus()
            logger.Debug("cmdAdd_Click Exit")
			Exit Sub
		End If
		
        '-- Check to see if the name already exists
        try
            rsOrigin = SQLOpenRecordSet("EXEC CheckForDuplicateOrigins 0, '" & txtOrigin_Name.Text & "'", dao.RecordsetTypeEnum.dbOpenSnapshot, dao.RecordsetOptionEnum.dbSQLPassThrough)
		    If rsOrigin.Fields("OriginCount").Value > 0 Then
			    rsOrigin.Close()
			    MsgBox("Origin name already exists.", MsgBoxStyle.Exclamation, "Error!")
                txtOrigin_Name.Focus()
                logger.Info("Origin name already exists.")
                logger.Debug("cmdAdd_Click Exit")
			    Exit Sub
		    End If
        Finally
            If rsOrigin IsNot Nothing Then
                rsOrigin.Close()
            End If
        End Try
		
		'-- Add the new record
		SQLExecute("EXEC InsertItemOrigin '" & txtOrigin_Name.Text & "'", dao.RecordsetOptionEnum.dbSQLPassThrough)
		glOriginID = -2
		
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
	
	Private Sub frmOriginAdd_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load

        logger.Debug("frmOriginAdd_Load Entry")
		'-- Center the form
        CenterForm(Me)

        logger.Debug("frmOriginAdd_Load Exit")

		
	End Sub
	
	Private Sub txtOrigin_Name_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles txtOrigin_Name.KeyPress

        logger.Debug("txtOrigin_Name_KeyPress Entry")

        Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
		
		KeyAscii = ValidateKeyPressEvent(KeyAscii, "String", txtOrigin_Name, 0, 0, 0)
		
		eventArgs.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			eventArgs.Handled = True
        End If

        logger.Debug("txtOrigin_Name_KeyPress Exit")

	End Sub
End Class