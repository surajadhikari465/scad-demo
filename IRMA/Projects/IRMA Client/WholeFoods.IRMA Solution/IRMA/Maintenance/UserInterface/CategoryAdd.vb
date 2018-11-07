Option Strict Off
Option Explicit On

Imports log4net
Friend Class frmCategoryAdd
    Inherits System.Windows.Forms.Form

    Dim IsInitializing As Boolean

    ' Define the log4net logger for this class.
    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

	Public plSubTeam_No As Integer

    Private Sub frmCategoryAdd_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load

        logger.Debug("frmCategoryAdd_Load Entry")

        Dim l As Integer

        LoadAllSubTeams(cmbSubTeam)
        If Me.plSubTeam_No > 0 Then
            For l = 0 To cmbSubTeam.Items.Count - 1
                If VB6.GetItemData(cmbSubTeam, l) = plSubTeam_No Then
                    cmbSubTeam.SelectedIndex = l
                    SetActive(cmbSubTeam, False)
                    Exit For
                End If
            Next l
        End If

        '-- Center the form
        CenterForm(Me)

        logger.Debug("frmCategoryAdd_Load Exit")
    End Sub

    Private Sub cmdAdd_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdAdd.Click

        logger.Debug("cmdAdd_Click Entry")

        Dim rsCategory As DAO.Recordset = Nothing

        '-- Take out unwanted spaces
        txtCategory_Name.Text = ConvertQuotes(Trim(txtCategory_Name.Text))

        '-- Check to see if anything was entered
        If txtCategory_Name.Text = "" Then
            MsgBox("Category name cannot be left blank.", MsgBoxStyle.Critical, Me.Text)
            txtCategory_Name.Focus()
            logger.Info("Category name cannot be left blank.")
            logger.Debug("cmdAdd_Click Exit")
            Exit Sub
        End If

        If cmbSubTeam.SelectedIndex = -1 Then
            MsgBox("Sub-Team is required", MsgBoxStyle.Critical, Me.Text)
            logger.Info("Sub-Team is required.")
            logger.Debug("cmdAdd_Click Exit")
            cmbSubTeam.Focus()
            Exit Sub
        End If

        '-- Check to see if the name already exists
        Try
            rsCategory = SQLOpenRecordSet("EXEC CheckForDuplicateCategories 0, '" & txtCategory_Name.Text & "'," & ComboValue(cmbSubTeam), DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
            If rsCategory.Fields("CategoryCount").Value > 0 Then
                rsCategory.Close()
                MsgBox("Category name already exists.", MsgBoxStyle.Critical, Me.Text)
                txtCategory_Name.Focus()
                logger.Info("Category name already exists.")
                logger.Debug("cmdAdd_Click Exit")
                Exit Sub
            End If
        Finally
            If rsCategory IsNot Nothing Then
                rsCategory.Close()
            End If
        End Try

        '-- Add the new record
        glCategoryIDSubTeam_No = CInt(ComboValue(cmbSubTeam))
        SQLExecute("EXEC InsertItemCategory '" & txtCategory_Name.Text & "', " & glCategoryIDSubTeam_No, DAO.RecordsetOptionEnum.dbSQLPassThrough)
        glCategoryID = -2

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
	
    Private Sub txtCategory_Name_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles txtCategory_Name.KeyPress
        logger.Debug("txtCategory_Name_KeyPress Entry")

        Dim KeyAscii As Short = Asc(eventArgs.KeyChar)

        KeyAscii = ValidateKeyPressEvent(KeyAscii, "String", txtCategory_Name, 0, 0, 0)

        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If

        logger.Debug("txtCategory_Name_KeyPress Exit")

    End Sub

    Private Sub cmbSubTeam_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbSubTeam.KeyPress

        logger.Debug("cmbSubTeam_KeyPress Entry")
        Dim KeyAscii As Short = Asc(e.KeyChar)

        If KeyAscii = 8 Then
            cmbSubTeam.SelectedIndex = -1
        End If

        e.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            e.Handled = True
        End If
        logger.Debug("cmbSubTeam_KeyPress Exit")

    End Sub

End Class