Option Strict Off
Option Explicit On

Imports log4net
Friend Class Level3Add
    Inherits System.Windows.Forms.Form

    Dim IsInitializing As Boolean

    Public SubTeamId As Integer
    Public CategoryId As Integer
    ' Define the log4net logger for this class.
    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)


    Private Sub Level3Add_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load

        logger.Debug("Level3Add_Load Entry")

        Dim l As Integer

        LoadCategory(cmbCategory, SubTeamId)
        If Me.CategoryId > 0 Then
            For l = 0 To cmbCategory.Items.Count - 1
                If VB6.GetItemData(cmbCategory, l) = CategoryId Then
                    cmbCategory.SelectedIndex = l
                    SetActive(cmbCategory, False)
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
        txtLevel3_Name.Text = ConvertQuotes(Trim(txtLevel3_Name.Text))

        '-- Check to see if anything was entered
        If txtLevel3_Name.Text = "" Then
            MsgBox("Level 3 name cannot be left blank.", MsgBoxStyle.Critical, Me.Text)
            txtLevel3_Name.Focus()
            logger.Info("Level 3 name cannot be left blank.")
            logger.Debug("cmdAdd_Click Exit")
            Exit Sub
        End If

        If cmbCategory.SelectedIndex = -1 Then
            MsgBox("Category is required", MsgBoxStyle.Critical, Me.Text)
            cmbCategory.Focus()
            logger.Info("Category is required")
            logger.Debug("cmdAdd_Click Exit")
            Exit Sub
        End If

        '-- Check to see if the name already exists
        Try
            rsLevel3 = SQLOpenRecordSet("EXEC CheckForDuplicateProdHierarchyLevel3 0, '" & txtLevel3_Name.Text & "'," & ComboValue(cmbCategory), DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
            If rsLevel3.Fields("Level3Count").Value > 0 Then
                rsLevel3.Close()
                MsgBox("Level 3 name already exists.", MsgBoxStyle.Critical, Me.Text)
                txtLevel3_Name.Focus()
                logger.Info("Level 3 name already exists.")
                logger.Debug("cmdAdd_Click Exit")
                Exit Sub
            End If
        Finally
            If rsLevel3 IsNot Nothing Then
                rsLevel3.Close()
            End If
        End Try

        '-- Add the new record
        glLevel3IDCategoryID = CInt(ComboValue(cmbCategory))
        SQLExecute("EXEC InsertProdHierarchyLevel3 '" & txtLevel3_Name.Text & "', " & glLevel3IDCategoryID, DAO.RecordsetOptionEnum.dbSQLPassThrough)
        glLevel3ID = -2

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

    Private Sub txtCategory_Name_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles txtLevel3_Name.KeyPress, txtLevel3_Name.KeyPress

        logger.Debug("txtCategory_Name_KeyPress Entry")

        Dim KeyAscii As Short = Asc(eventArgs.KeyChar)

        KeyAscii = ValidateKeyPressEvent(KeyAscii, "String", txtLevel3_Name, 0, 0, 0)

        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If

        logger.Debug("txtCategory_Name_KeyPress Exit")

    End Sub

    Private Sub cmbCategory_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbCategory.KeyPress, cmbCategory.KeyPress

        logger.Debug("cmbCategory_KeyPress Entry")

        Dim KeyAscii As Short = Asc(e.KeyChar)

        If KeyAscii = 8 Then
            cmbCategory.SelectedIndex = -1
        End If

        e.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            e.Handled = True
        End If

        logger.Debug("cmbCategory_KeyPress Exit")

    End Sub

End Class