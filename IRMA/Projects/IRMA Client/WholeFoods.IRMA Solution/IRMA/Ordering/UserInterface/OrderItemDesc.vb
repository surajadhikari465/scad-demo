Option Strict Off
Option Explicit On
Imports log4net
Friend Class frmOrdersItemDesc
    Inherits System.Windows.Forms.Form

    Private Property iTextFieldLength As Integer = 0

    ' Define the log4net logger for this class.
    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

	'UPGRADE_WARNING: Arrays in structure rsComment may need to be initialized before they can be used. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="814DF224-76BD-4BB4-BFFB-EA359CB9FC48"'
	Dim rsComment As dao.Recordset
	Dim pbDataChanged As Boolean
	
	Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click

        logger.Debug("cmdExit_Click Entry")
        Me.Close()
        logger.Debug("cmdExit_Click Exit")
		
	End Sub
	
	Private Sub frmOrdersItemDesc_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load

        logger.Debug("frmOrdersItemDesc_Load Entry")
		CenterForm(Me)
		
		SQLOpenRS(rsComment, "EXEC GetOrderItemComments " & glOrderItemID, dao.RecordsetTypeEnum.dbOpenSnapshot, dao.RecordsetOptionEnum.dbSQLPassThrough)
        txtField.Text = rsComment.Fields("Comments").Value & ""

        ' Unselect the text. RS. 7/6/2011
        txtField.Select(0, 0)
		rsComment.Close()
		
        pbDataChanged = False

        logger.Debug("frmOrdersItemDesc_Load Exit")
		
	End Sub
	
	Private Sub frmOrdersItemDesc_FormClosed(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed

        logger.Debug("frmOrdersItemDesc_FormClosed Entry")
		If pbDataChanged Then
			SQLExecute("EXEC UpdateOrderItemComments " & glOrderItemID & ", '" & txtField.Text & "'", dao.RecordsetOptionEnum.dbSQLPassThrough)
        End If
        logger.Debug("frmOrdersItemDesc_FormClosed Exit")
		
	End Sub
	
	'UPGRADE_WARNING: Event txtField.TextChanged may fire when form is initialized. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"'
	Private Sub txtField_TextChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles txtField.TextChanged
        logger.Debug("txtField_TextChanged Entry")

        Dim txtFieldValidated As String = ""
        Dim shortElementAscii As Short

        ' More than 1 character was entered, so check characters for special characters as copy/paste could have occurred
        If (Len(txtField.Text) - iTextFieldLength) > 1 Then
            For Each element As Char In txtField.Text
                shortElementAscii = ValidateKeyPressEvent(Asc(element), "LIMITEDSTRING", txtField, 0, 0, 0)
                If shortElementAscii > 0 Then
                    txtFieldValidated = txtFieldValidated & Chr(shortElementAscii)
                End If
            Next

            ' Stop handling TextChanged events.
            RemoveHandler txtField.TextChanged, AddressOf txtField_TextChanged
            ' This event will not be handled.
            txtField.Text = txtFieldValidated
            ' Associate the TextChanged event handler to the txtField
            AddHandler txtField.TextChanged, AddressOf txtField_TextChanged
        End If

        iTextFieldLength = Len(txtField.Text)

        pbDataChanged = True
        logger.Debug("txtField_TextChanged Exit")
    End Sub
	
	Private Sub txtField_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles txtField.KeyPress

        logger.Debug("txtField_KeyPress Entry")
        Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
		
        KeyAscii = ValidateKeyPressEvent(KeyAscii, "LIMITEDSTRING", txtField, 0, 0, 0)
		
		eventArgs.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			eventArgs.Handled = True
        End If
        logger.Debug("txtField_KeyPress Exit")

	End Sub
End Class