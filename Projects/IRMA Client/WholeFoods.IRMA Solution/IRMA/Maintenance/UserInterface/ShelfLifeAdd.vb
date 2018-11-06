Option Strict Off
Option Explicit On
Imports log4net
Friend Class frmShelfLifeAdd
    Inherits System.Windows.Forms.Form

    ' Define the log4net logger for this class.
    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

	
	Private Sub cmdAdd_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdAdd.Click

        logger.Debug("cmdAdd_Click Entry")

        Dim rsShelfLife As DAO.Recordset = Nothing
		
		'-- Take out unwanted spaces
		txtShelfLife_Name.Text = Trim(txtShelfLife_Name.Text)
		
		'-- Check to see if anything was entered
		If txtShelfLife_Name.Text = vbNullString Then
			MsgBox("Shelf Life name cannot be left blank.", MsgBoxStyle.Exclamation, "Error!")
            txtShelfLife_Name.Focus()
            logger.Info("Shelf Life name cannot be left blank.")
            logger.Debug("cmdAdd_Click Exit")
			Exit Sub
		End If
		
        '-- Check to see if the name already exists
        Try
            rsShelfLife = SQLOpenRecordSet("EXEC CheckForDuplicateShelfLives 0, '" & txtShelfLife_Name.Text & "'", DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
            If rsShelfLife.Fields("ShelfLifeCount").Value > 0 Then
                rsShelfLife.Close()
                MsgBox("Shelf Life already exists.", MsgBoxStyle.Exclamation, "Error!")
                logger.Info("Shelf Life already exists.")
                txtShelfLife_Name.Focus()
                Exit Sub
            End If
        Finally
            If rsShelfLife IsNot Nothing Then
                rsShelfLife.Close()
            End If
        End Try

        '-- Add the new record
        SQLExecute("EXEC InsertItemShelfLife '" & txtShelfLife_Name.Text & "'", DAO.RecordsetOptionEnum.dbSQLPassThrough)
        glShelfLifeID = -2

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
	
	Private Sub frmShelfLifeAdd_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load

        logger.Debug("frmShelfLifeAdd_Load Entry")

		'-- Center the form
		CenterForm(Me)

        logger.Debug("frmShelfLifeAdd_Load Exit")

	End Sub
	
	Private Sub txtShelfLife_Name_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles txtShelfLife_Name.KeyPress

        logger.Debug("txtShelfLife_Name_KeyPress Entry")
        Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
		
		KeyAscii = ValidateKeyPressEvent(KeyAscii, "String", txtShelfLife_Name, 0, 0, 0)
		
		eventArgs.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			eventArgs.Handled = True
        End If

        logger.Debug("txtShelfLife_Name_KeyPress Exit")
	End Sub
End Class