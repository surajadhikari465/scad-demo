Option Strict Off
Option Explicit On
Imports log4net
Friend Class frmBrandAdd
    Inherits System.Windows.Forms.Form
    ' Define the log4net logger for this class.
    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

	
	Private Sub cmdAdd_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdAdd.Click
        logger.Debug("cmdAdd_Click Entry")

        Dim rsBrand As DAO.Recordset = Nothing
		
		txtBrand_Name.Text = ConvertQuotes(Trim(txtBrand_Name.Text))
		
		'-- Check to see if anything was entered
		If txtBrand_Name.Text = vbNullString Then
            MsgBox("Brand name cannot be left blank.", MsgBoxStyle.Exclamation, "Error!")
            logger.Info("Brand name cannot be left blank.")
            logger.Debug("cmdAdd_Click Exit")
			txtBrand_Name.Focus()
			Exit Sub
		End If
		
        '-- Check to see if the name already exists
        Try
            rsBrand = SQLOpenRecordSet("EXEC CheckForDuplicateBrands 0, '" & txtBrand_Name.Text & "'", DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbForwardOnly + DAO.RecordsetOptionEnum.dbSQLPassThrough)
            If rsBrand.Fields("BrandCount").Value > 0 Then
                rsBrand.Close()
                MsgBox("Brand name already exists.", MsgBoxStyle.Exclamation, "Error!")
                txtBrand_Name.Focus()
                logger.Info("Brand name already exists.")
                logger.Debug("cmdAdd_Click Exit")
                Exit Sub
            End If
        Finally
            If rsBrand IsNot Nothing Then
                rsBrand.Close()
            End If
        End Try

        '-- Add the new record
        SQLExecute("EXEC InsertItemBrand '" & txtBrand_Name.Text & "'", DAO.RecordsetOptionEnum.dbSQLPassThrough)
        glBrandID = -2

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
	
	Private Sub frmBrandAdd_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
        logger.Debug("frmBrandAdd_Load Entry")
		'-- Center the form
        CenterForm(Me)
        logger.Debug("frmBrandAdd_Load Exit")
		
	End Sub
	
	Private Sub txtBrand_Name_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles txtBrand_Name.KeyPress

        logger.Debug("txtBrand_Name_KeyPress Entry")

        Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
		
		KeyAscii = ValidateKeyPressEvent(KeyAscii, "String", txtBrand_Name, 0, 0, 0)
		
		eventArgs.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			eventArgs.Handled = True
        End If

        logger.Debug("txtBrand_Name_KeyPress Exit")
	End Sub
End Class