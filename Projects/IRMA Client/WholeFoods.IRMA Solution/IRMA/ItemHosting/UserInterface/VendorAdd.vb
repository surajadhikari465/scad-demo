Option Strict Off
Option Explicit On
Imports log4net
Imports WholeFoods.IRMA.Common.BusinessLogic
Imports WholeFoods.IRMA.Common.DataAccess
Imports WholeFoods.IRMA.ItemHosting.DataAccess

Friend Class frmVendorAdd
    Inherits System.Windows.Forms.Form
    ' Define the log4net logger for this class.
    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

	
	Private Sub cmdAdd_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdAdd.Click
        logger.Debug("cmdAdd_Click Entry")

        Dim rsVendor As DAO.Recordset = Nothing
		
		'-- Take out unwanted spaces
		txtCompany_Name.Text = ConvertQuotes(Trim(txtCompany_Name.Text))
		
		'-- Check to see if anything was entered
		If txtCompany_Name.Text = vbNullString Then
            MsgBox(String.Format(ResourcesIRMA.GetString("Required"), lblCompany.Text.Replace(":", "")), MsgBoxStyle.Exclamation, Me.Text)
            logger.Info(String.Format(ResourcesIRMA.GetString("Required"), lblCompany.Text.Replace(":", "")))
            txtCompany_Name.Focus()
            logger.Debug("cmdAdd_Click Exit")
			Exit Sub
        End If

        ' TFS#6686 (v3.2) - New vendor records must be created with a Vendor Key if required.
        ' The Key is now required for some regions.
        If InstanceDataDAO.IsFlagActive("VendorKeyRequired") Then
            If Trim(txtVendorKey.Text) = vbNullString Then
                MsgBox(String.Format(ResourcesIRMA.GetString("Required"), lblKey.Text.Replace(":", "")), MsgBoxStyle.Exclamation, Me.Text)
                logger.Info(String.Format(ResourcesIRMA.GetString("Required"), lblKey.Text.Replace(":", "")))
                txtVendorKey.Focus()
                logger.Debug("cmdAdd_Click Exit")
                Exit Sub
            ElseIf VendorDAO.VendorKeyExists(Trim(txtVendorKey.Text)) Then
                MsgBox(String.Format(ResourcesIRMA.GetString("Duplicate"), lblKey.Text.Replace(":", "")), MsgBoxStyle.Exclamation, Me.Text)
                logger.Info(String.Format(ResourcesIRMA.GetString("Duplicate"), lblKey.Text.Replace(":", "")))
                txtVendorKey.Focus()
                txtVendorKey.SelectAll()
                logger.Debug("cmdAdd_Click Exit")
                Exit Sub
            End If
        End If

        'bug 5195 - don't ask for PS information if the vendor is not an external one
        If Me._checkExternal.Checked Then

            If Not IsNumeric(txtPSVendor.Text.Trim()) Then
                MsgBox(String.Format(ResourcesItemHosting.GetString("PSVendorNumericValue"), lblPSVendor.Text.Replace(":", "")))
                logger.Info(String.Format(ResourcesItemHosting.GetString("PSVendorNumericValue"), lblPSVendor.Text.Replace(":", "")))
                txtPSVendor.Focus()
                logger.Debug("cmdAdd_Click Exit")
                Exit Sub
            End If

            If Not IsNumeric(txtPSVendorExport.Text.Trim()) Then
                MsgBox(String.Format(ResourcesItemHosting.GetString("PSVendorNumericValue"), lblPSVendorExport.Text.Replace(":", "")))
                txtPSVendorExport.Focus()
                logger.Info(String.Format(ResourcesItemHosting.GetString("PSVendorNumericValue"), lblPSVendorExport.Text.Replace(":", "")))
                logger.Debug("cmdAdd_Click Exit")
                Exit Sub
            End If

        End If

        Try
            '-- Check to see if the name already exists
            '-- This check uses the "child" PS vendor name, not the parent
            rsVendor = SQLOpenRecordSet("EXEC CheckForDuplicateVendors 0, '" & txtCompany_Name.Text & "'," & TextValue("'" & txtPSVendor.Text & "'") & "," & TextValue("'" & txtPSAddr.Text & "'"), DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
            If rsVendor.Fields("VendorCount").Value > 0 Then
                ' rsVendor will be closed in the finally block. This call is not needed. (bug 5686,6515)
                ' rsVendor.Close()

                MsgBox(ResourcesItemHosting.GetString("DuplicateCompany"), MsgBoxStyle.Critical, Me.Text)
                logger.Info(ResourcesItemHosting.GetString("DuplicateCompany"))
                logger.Debug("cmdAdd_Click Exit")
                Exit Sub
            End If
        Finally
            If rsVendor IsNot Nothing Then

                rsVendor.Close()
                rsVendor = Nothing
            End If
        End Try

        '-- Add the new record
        SQLExecute("EXEC InsertVendor '" & txtCompany_Name.Text & "'," & TextValue("'" & txtPSVendor.Text & "'") & "," & TextValue("'" & txtPSVendorExport.Text & "'") & "," & TextValue("'" & txtPSAddr.Text & "'") & "," & TextValue("'" & txtVendorKey.Text & "'"), DAO.RecordsetOptionEnum.dbSQLPassThrough)
        glVendorID = -2

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
	
	Private Sub frmVendorAdd_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
        logger.Debug("frmVendorAdd_Load Entry")
		'-- Center the form
        CenterForm(Me)
        txtCompany_Name.Focus()
        txtCompany_Name.Select()
        logger.Debug("frmVendorAdd_Load Exit")
		
	End Sub
	
	Private Sub txtCompany_Name_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles txtCompany_Name.KeyPress
        logger.Debug("txtCompany_Name_KeyPress Entry")

        Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
		
		KeyAscii = ValidateKeyPressEvent(KeyAscii, "String", txtCompany_Name, 0, 0, 0)
		
		eventArgs.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			eventArgs.Handled = True
        End If

        logger.Debug("txtCompany_Name_KeyPress Exit")
	End Sub
	
	Private Sub txtPSAddr_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles txtPSAddr.KeyPress
        logger.Debug("txtPSAddr_KeyPress Entry")

        Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
		
		KeyAscii = ValidateKeyPressEvent(KeyAscii, (txtPSAddr.Tag), txtPSAddr, 0, 0, 0)
		
		eventArgs.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			eventArgs.Handled = True
        End If

        logger.Debug("txtPSAddr_KeyPress Exit")
	End Sub
	
	
	Private Sub txtPSVendor_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles txtPSVendor.KeyPress

        logger.Debug("txtPSVendor_KeyPress Entry")

        Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
		
		KeyAscii = ValidateKeyPressEvent(KeyAscii, (txtPSVendor.Tag), txtPSVendor, 0, 0, 0)
		
		eventArgs.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			eventArgs.Handled = True
        End If
        logger.Debug("txtPSVendor_KeyPress Exit")

	End Sub

    Private Sub _checkExternal_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _checkExternal.CheckedChanged
        Me._groupAccountingSetup.Enabled = Me._checkExternal.Checked
    End Sub

End Class
