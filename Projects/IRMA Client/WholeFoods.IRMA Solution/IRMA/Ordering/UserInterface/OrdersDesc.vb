Option Strict Off
Option Explicit On
Imports log4net
Imports WholeFoods.IRMA.Ordering.DataAccess

Friend Class frmOrdersDesc
    Inherits System.Windows.Forms.Form

    Public Property blnIsFromSuspendedPOTool As Boolean
    Public Property iOrderHeaderID As Integer
    Public Property iOrderItemID As Integer
    Private Property iTextFieldLength As Integer = 0

    ' Define the log4net logger for this class.
    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Dim IsInitializing As Boolean
    Dim rsComment As DAO.Recordset
    Dim pbDataChanged As Boolean

    Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click
        logger.Debug("cmdExit_Click Entry")
        Me.Close()
        logger.Debug("cmdExit_Click Exit")

    End Sub

    Private Sub frmOrdersDesc_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load

        logger.Debug("frmOrdersDesc_Load Entry")
        CenterForm(Me)

        iTextFieldLength = Len(txtField.Text)
        lblCharacterCount.Text = Convert.ToString(iTextFieldLength) & " of 5000 characters used."

        If blnIsFromSuspendedPOTool Then
            lblCharacterCount.Visible = True
            txtField.Text = OrderingDAO.GetSuspendedPONotes(iOrderHeaderID, iOrderItemID)
        Else
            lblCharacterCount.Visible = False
            SQLOpenRS(rsComment, "EXEC GetOrderHeaderDesc " & glOrderHeaderID, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
            txtField.Text = rsComment.Fields("OrderHeaderDesc").Value & ""
            txtField.Select(0, 0)
            txtField.Enabled = Convert.ToBoolean(rsComment.Fields("Deleted").Value)
            rsComment.Close()
        End If

        pbDataChanged = False
        logger.Debug("frmOrdersDesc_Load Exit")

    End Sub

    Private Sub frmOrdersDesc_FormClosed(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        logger.Debug("frmOrdersDesc_FormClosed Entry")
        If pbDataChanged Then
            If Not blnIsFromSuspendedPOTool Then
                SQLExecute("EXEC UpdateOrderHeaderDesc " & glOrderHeaderID & ", '" & txtField.Text & "'", DAO.RecordsetOptionEnum.dbSQLPassThrough)
            End If
        End If

        logger.Debug("frmOrdersDesc_FormClosed Exit")
    End Sub

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

        If Me.IsInitializing = True Then Exit Sub

        lblCharacterCount.Text = Len(txtField.Text) & " of 5000 characters used."
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
        Else
            eventArgs.Handled = False
        End If

        logger.Debug("txtField_KeyPress Exit")
    End Sub
End Class