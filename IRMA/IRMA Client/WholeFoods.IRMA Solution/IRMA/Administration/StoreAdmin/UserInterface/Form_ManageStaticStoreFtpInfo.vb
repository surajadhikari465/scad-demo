Imports Infragistics.Win.UltraWinGrid
Imports WholeFoods.IRMA.Administration.Common.BusinessLogic
Imports WholeFoods.IRMA.Administration.POSPush.BusinessLogic
Imports WholeFoods.IRMA.Administration.POSPush.DataAccess
Imports WholeFoods.IRMA.Replenishment.Common.BusinessLogic
Imports WholeFoods.IRMA.Replenishment.Common.DataAccess
Imports WholeFoods.Utility
Imports WholeFoods.Utility.DataAccess
Imports WholeFoods.IRMA.ItemHosting.DataAccess

Public Class Form_ManageStaticStoreFtpInfo

    Dim _loading As Boolean = False
    Private _Store_No As Integer = -1
    Dim WithEvents _editStoreFtpForm As Form_EditStaticStoreFTPInfo
    'list of writer types that the store can configure by clicking the 'Add' button
    Private _availWriterTypes As ArrayList

    Public Property Store_No() As Integer
        Get
            Return _Store_No
        End Get
        Set(ByVal value As Integer)
            _Store_No = value
        End Set
    End Property

#Region "form events"

    Private Sub Form_ManageStoreFtpInfo_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.CenterToScreen()
        FormatDataGrid()
        LoadStoreInformation()
    End Sub

    Private Sub LoadStoreInformation()
        Dim DAO As WholeFoods.IRMA.ItemHosting.DataAccess.StoreDAO = New WholeFoods.IRMA.ItemHosting.DataAccess.StoreDAO
        Dim DT As DataTable
        Dim DR As DataRow

        _loading = True
        'DT = StoreDAO.GetStoreList()
        DT = StoreDAO.GetStoreAndDCList()
        DR = DT.NewRow()
        DR("Store_no") = -1
        DR("store_name") = "< None >"
        DT.Rows.InsertAt(DR, 0)
        ComboBox_Store.DataSource = DT
        ComboBox_Store.DisplayMember = "Store_Name"
        ComboBox_Store.ValueMember = "Store_no"

        For Each item As DataRowView In ComboBox_Store.Items
            If Me.Store_No = CInt(item("Store_no")) Then
                ComboBox_Store.SelectedItem = item
            End If
        Next
        _loading = False
    End Sub
#Region "Button actions"

    Private Sub Button_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Close.Click
        Me.Close()
    End Sub

    Private Sub Button_Add_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Add.Click
        'add
        UpdateData(False)
    End Sub

    Private Sub Button_Edit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Edit.Click
        'edit
        UpdateData(True)
    End Sub

    Private Sub Button_Delete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Delete.Click
        'delete item
        Dim result As DialogResult = MessageBox.Show(ResourcesCommon.GetString("msg_confirmDeleteData"), Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question)

        If result = Windows.Forms.DialogResult.Yes Then
            'delete
            If DeleteData() Then
                'update grid
                BindData()
                'enable 'Add' button only if store has writer types available to configure
                GetAvailableWriterTypesForAdd()
            End If
        End If
    End Sub

#End Region

#End Region

    Private Sub editStoreFtpForm_UpdateCallingForm() Handles _editStoreFtpForm.UpdateCallingForm
        'rebind grid
        BindData()
        'enable 'Add' button only if store has writer types available to configure
        GetAvailableWriterTypesForAdd()
    End Sub

    Private Sub GetAvailableWriterTypesForAdd()
        'enable 'Add' button only if store has writer types available to configure
        Dim writerDAO As New POSWriterDAO
        _availWriterTypes = writerDAO.GetAvailableFileWriterTypes(Me.Store_No, False)
        If _availWriterTypes.Count <= 0 Then
            Me.Button_Add.Enabled = False
        Else
            Me.Button_Add.Enabled = True
        End If
    End Sub

    Private Sub BindData()
        Dim staticStoreFtpConfigDAO As New StaticStoreFTPConfigDAO
        Dim storeConfigList As New ArrayList

        Try
            'get ftp data for store for all writer types
            Dim posFTPData As StoreFTPConfigBO = staticStoreFtpConfigDAO.GetFTPConfigDataForStoreAndWriterType(Me.Store_No, POSWriterBO.WRITER_TYPE_POS)
            Dim scaleFTPData As StoreFTPConfigBO = staticStoreFtpConfigDAO.GetFTPConfigDataForStoreAndWriterType(Me.Store_No, POSWriterBO.WRITER_TYPE_SCALE)
            Dim tlogFTPData As StoreFTPConfigBO = staticStoreFtpConfigDAO.GetFTPConfigDataForStoreAndWriterType(Me.Store_No, POSWriterBO.WRITER_TYPE_TLOG)
            Dim posPullFTPData As StoreFTPConfigBO = staticStoreFtpConfigDAO.GetFTPConfigDataForStoreAndWriterType(Me.Store_No, POSWriterBO.WRITER_TYPE_POSPULL)
            Dim plumStoreFTPData As StoreFTPConfigBO = staticStoreFtpConfigDAO.GetFTPConfigDataForStoreAndWriterType(Me.Store_No, POSWriterBO.WRITER_TYPE_PLUMSTORE)
            Dim reprintTagsFTPData As StoreFTPConfigBO = staticStoreFtpConfigDAO.GetFTPConfigDataForStoreAndWriterType(Me.Store_No, POSWriterBO.WRITER_TYPE_REPRINTTAG)
            Dim shelfTagFTPData As StoreFTPConfigBO = staticStoreFtpConfigDAO.GetFTPConfigDataForStoreAndWriterType(Me.Store_No, POSWriterBO.WRITER_TYPE_TAG)

            If posFTPData IsNot Nothing Then
                storeConfigList.Add(posFTPData)
            End If

            If scaleFTPData IsNot Nothing Then
                storeConfigList.Add(scaleFTPData)
            End If

            If tlogFTPData IsNot Nothing Then
                storeConfigList.Add(tlogFTPData)
            End If

            If posPullFTPData IsNot Nothing Then
                storeConfigList.Add(posPullFTPData)
            End If

            If reprintTagsFTPData IsNot Nothing Then
                storeConfigList.Add(reprintTagsFTPData)
            End If

            If plumStoreFTPData IsNot Nothing Then
                storeConfigList.Add(plumStoreFTPData)
            End If

            If shelfTagFTPData IsNot Nothing Then
                storeConfigList.Add(shelfTagFTPData)
            End If

            Me.UltraGrid_StoreFTPConfig.DataSource = storeConfigList

            If Me.UltraGrid_StoreFTPConfig.Rows.Count > 0 Then
                'first row appears to be defaulted because it is highlighted, but it's not actually selected.
                'so if user tried to edit or delete upon entering the screen they must still click the highlighted row.
                'below code is actually selecting row that is already highlighted.
                Me.UltraGrid_StoreFTPConfig.Rows(0).Selected = True
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub FormatDataGrid()
        If UltraGrid_StoreFTPConfig.DisplayLayout.Bands(0).Columns.Count > 0 Then
            'hide columns
            UltraGrid_StoreFTPConfig.DisplayLayout.Bands(0).Columns("StoreNo").Hidden = True

            'sort columns in correct order
            UltraGrid_StoreFTPConfig.DisplayLayout.Bands(0).Columns("FileWriterType").Header.VisiblePosition = 0
            UltraGrid_StoreFTPConfig.DisplayLayout.Bands(0).Columns("IPAddress").Header.VisiblePosition = 1
            UltraGrid_StoreFTPConfig.DisplayLayout.Bands(0).Columns("FTPUser").Header.VisiblePosition = 2
            UltraGrid_StoreFTPConfig.DisplayLayout.Bands(0).Columns("FTPPassword").Header.VisiblePosition = 3
            UltraGrid_StoreFTPConfig.DisplayLayout.Bands(0).Columns("ChangeDirectory").Header.VisiblePosition = 4
            UltraGrid_StoreFTPConfig.DisplayLayout.Bands(0).Columns("Port").Header.VisiblePosition = 5
            UltraGrid_StoreFTPConfig.DisplayLayout.Bands(0).Columns("IsSecureTransfer").Header.VisiblePosition = 6

            'set column names
            UltraGrid_StoreFTPConfig.DisplayLayout.Bands(0).Columns("FileWriterType").Header.Caption = ResourcesAdministration.GetString("label_header_fileWriterType")
            UltraGrid_StoreFTPConfig.DisplayLayout.Bands(0).Columns("IPAddress").Header.Caption = ResourcesAdministration.GetString("label_header_ipAddress")
            UltraGrid_StoreFTPConfig.DisplayLayout.Bands(0).Columns("FTPUser").Header.Caption = ResourcesAdministration.GetString("label_header_ftpUser")
            UltraGrid_StoreFTPConfig.DisplayLayout.Bands(0).Columns("FTPPassword").Header.Caption = ResourcesAdministration.GetString("label_header_ftpPassword")
            UltraGrid_StoreFTPConfig.DisplayLayout.Bands(0).Columns("ChangeDirectory").Header.Caption = ResourcesAdministration.GetString("label_header_changeDirectory")
            UltraGrid_StoreFTPConfig.DisplayLayout.Bands(0).Columns("Port").Header.Caption = ResourcesAdministration.GetString("label_header_port")
            UltraGrid_StoreFTPConfig.DisplayLayout.Bands(0).Columns("IsSecureTransfer").Header.Caption = ResourcesAdministration.GetString("label_header_isSecureTransfer")
        End If
    End Sub

    ''' <summary>
    ''' This function reads the selected row from a data grid.
    ''' The row can be selected by highlighting the entire row or a single cell
    ''' within the row.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function getSelectedRow(ByRef dataGrid As DataGridView) As DataGridViewRow
        ' Get the selected row
        Dim selectedRow As DataGridViewRow = Nothing
        If (dataGrid.SelectedRows.Count = 1) Then
            Dim rowEnum As IEnumerator = dataGrid.SelectedRows.GetEnumerator
            rowEnum.MoveNext()
            selectedRow = CType(rowEnum.Current, DataGridViewRow)
        ElseIf (dataGrid.SelectedCells.Count = 1) Then
            Dim cellEnum As IEnumerator = dataGrid.SelectedCells.GetEnumerator
            cellEnum.MoveNext()
            Dim selectedCell As DataGridViewCell = CType(cellEnum.Current, DataGridViewCell)
            selectedRow = selectedCell.OwningRow
        Else
            ' Error condition
            DisplayErrorMessage("A row must be selected to perform this action.")
        End If
        Return selectedRow
    End Function

    Private Sub UpdateData(ByVal isEdit As Boolean)
        _editStoreFtpForm = New Form_EditStaticStoreFTPInfo
        Dim selectedRowIndex As Integer
        Dim isRowSelected As Boolean = False

        Try
            'get selected row
            If Me.UltraGrid_StoreFTPConfig.Selected.Rows.Count = 0 Then
                selectedRowIndex = Me.UltraGrid_StoreFTPConfig.ActiveCell.Row.Index
            Else
                selectedRowIndex = Me.UltraGrid_StoreFTPConfig.ActiveRow.Index
            End If

            isRowSelected = True
        Catch ex As Exception
            isRowSelected = False
        End Try

        'verify that user has selected a valid row
        If (isEdit AndAlso isRowSelected) Or (Not isEdit) Then
            _editStoreFtpForm.IsEdit = isEdit

            If isEdit Then
                ' Populate the edit form with the values from the selected row
                Dim currentRow As UltraGridRow = Me.UltraGrid_StoreFTPConfig.Rows(selectedRowIndex)
                _editStoreFtpForm.StoreFTPConfig = New StoreFTPConfigBO(currentRow)
            Else
                Dim storeFtpConfig As New StoreFTPConfigBO
                storeFtpConfig.StoreNo = Me.Store_No
                _editStoreFtpForm.StoreFTPConfig = storeFtpConfig
                _editStoreFtpForm.AvailableWriterTypes = _availWriterTypes
            End If

            'open form
            _editStoreFtpForm.ShowDialog(Me)
            _editStoreFtpForm.Dispose()
        Else
            'prompt user to select a row
            MessageBox.Show(ResourcesCommon.GetString("msg_selectEditRow"), Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Hand)
        End If
    End Sub

    Private Function DeleteData() As Boolean
        Dim success As Boolean = False
        Dim selectedRowIndex As Integer
        Dim isRowSelected As Boolean = False
        Dim staticStoreFtpConfigDAO As New StaticStoreFTPConfigDAO

        Try
            'get selected row
            If Me.UltraGrid_StoreFTPConfig.Selected.Rows.Count = 0 Then
                selectedRowIndex = Me.UltraGrid_StoreFTPConfig.ActiveCell.Row.Index
            Else
                selectedRowIndex = Me.UltraGrid_StoreFTPConfig.ActiveRow.Index
            End If

            isRowSelected = True
        Catch ex As Exception
            isRowSelected = False
        End Try

        'verify that user has selected a valid row
        If isRowSelected Then
            Dim currentRow As UltraGridRow = Me.UltraGrid_StoreFTPConfig.Rows(selectedRowIndex)

            Try
                'perform delete
                staticStoreFtpConfigDAO.DeleteFTPInfo(New StoreFTPConfigBO(currentRow))

                success = True
            Catch ex As DataFactoryException
                Logger.LogError("Exception: ", Me.GetType(), ex)
                'display a message to the user
                DisplayErrorMessage(ERROR_DB)
                'send message about exception
                Dim args(1) As String
                args(0) = "Form_ManageStoreFtpInfo form: DeleteData sub"
                ErrorHandler.ProcessError(WholeFoods.Utility.ErrorType.POSPush_AdminError, args, SeverityLevel.Warning)
            End Try
        Else
            'prompt user to select a row
            MessageBox.Show(ResourcesCommon.GetString("msg_selectDeleteRow"), Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Hand)
        End If

        Return success
    End Function

    Private Sub ComboBox_Store_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles ComboBox_Store.SelectedIndexChanged
        Dim dr As DataRowView
        If Not _loading Then
            If Not ComboBox_Store.SelectedItem Is Nothing Then
                dr = CType(ComboBox_Store.SelectedItem, DataRowView)
                Me.Store_No = CInt(dr("store_no"))
                Me.editStoreFtpForm_UpdateCallingForm()
            End If
        End If
    End Sub
End Class