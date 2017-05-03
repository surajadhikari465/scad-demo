Option Strict Off
Option Explicit On
Imports log4net


Friend Class Level4Add
    Inherits System.Windows.Forms.Form

    Dim IsInitializing As Boolean

    Public CategoryId As Integer
    Public Level3Id As Integer

    ' Define the log4net logger for this class.
    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)


    Private Sub Level3Add_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load

        logger.Debug("Level3Add_Load Entry")

        Dim l As Integer

        LoadProdHierarchyLevel3s(cmbLevel3, CategoryId)
        If Me.CategoryId > 0 Then
            For l = 0 To cmbLevel3.Items.Count - 1
                If VB6.GetItemData(cmbLevel3, l) = Level3Id Then
                    cmbLevel3.SelectedIndex = l
                    SetActive(cmbLevel3, False)
                    Exit For
                End If
            Next l
        End If

        '-- Center the form
        CenterForm(Me)

        logger.Debug("Level3Add_Load Exit")

    End Sub

    Private Sub cmdAdd_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdAdd.Click

        logger.Debug("cmdAdd_Click Entry")

        Dim rsLevel3 As DAO.Recordset = Nothing

        '-- Take out unwanted spaces
        txtLevel4_Name.Text = ConvertQuotes(Trim(txtLevel4_Name.Text))

        '-- Check to see if anything was entered
        If txtLevel4_Name.Text = "" Then
            MsgBox("Level 4 name cannot be left blank.", MsgBoxStyle.Critical, Me.Text)
            txtLevel4_Name.Focus()
            logger.Info("Level 4 name cannot be left blank.")
            logger.DebugFormat("cmdAdd_Click Exit")
            Exit Sub
        End If

        If cmbLevel3.SelectedIndex = -1 Then
            MsgBox("Level 3 is required", MsgBoxStyle.Critical, Me.Text)
            cmbLevel3.Focus()
            logger.Info("Level 3 is required")
            logger.DebugFormat("cmdAdd_Click Exit")
            Exit Sub
        End If

        '-- Check to see if the name already exists
        Try
            rsLevel3 = SQLOpenRecordSet("EXEC CheckForDuplicateProdHierarchyLevel4 0, '" & txtLevel4_Name.Text & "'," & ComboValue(cmbLevel3), DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
            If rsLevel3.Fields("Level4Count").Value > 0 Then
                rsLevel3.Close()
                MsgBox("Level 3 name already exists.", MsgBoxStyle.Critical, Me.Text)
                txtLevel4_Name.Focus()
                logger.Info("Level 3 name already exists.")
                logger.DebugFormat("cmdAdd_Click Exit")
                Exit Sub
            End If
        Finally
            If rsLevel3 IsNot Nothing Then
                rsLevel3.Close()
            End If
        End Try

        '-- Add the new record
        glLevel4IDLevel3ID = CInt(ComboValue(cmbLevel3))
        SQLExecute("EXEC InsertProdHierarchyLevel4 '" & txtLevel4_Name.Text & "', " & glLevel4IDLevel3ID, DAO.RecordsetOptionEnum.dbSQLPassThrough)
        glLevel4ID = -2

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

    Private Sub txtCategory_Name_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles txtLevel4_Name.KeyPress, txtLevel4_Name.KeyPress, txtLevel4_Name.KeyPress

        logger.Debug("txtCategory_Name_KeyPress Entry")

        Dim KeyAscii As Short = Asc(eventArgs.KeyChar)

        KeyAscii = ValidateKeyPressEvent(KeyAscii, "String", txtLevel4_Name, 0, 0, 0)

        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        logger.Debug("txtCategory_Name_KeyPress Exit")

    End Sub

    Private Sub cmbLevel3_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbLevel3.KeyPress, cmbLevel3.KeyPress, cmbLevel3.KeyPress

        logger.Debug("cmbLevel3_KeyPress Entry")

        Dim KeyAscii As Short = Asc(e.KeyChar)

        If KeyAscii = 8 Then
            cmbLevel3.SelectedIndex = -1
        End If

        e.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            e.Handled = True
        End If

        logger.Debug("cmbLevel3_KeyPress Exit")
    End Sub

End Class