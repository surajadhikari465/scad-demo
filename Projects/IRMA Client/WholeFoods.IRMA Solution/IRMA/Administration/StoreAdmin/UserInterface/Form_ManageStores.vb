Imports WholeFoods.IRMA.Administration.Common.BusinessLogic
Imports WholeFoods.IRMA.Administration.Common.DataAccess
Imports WholeFoods.IRMA.Administration.POSPush.DataAccess
Imports WholeFoods.IRMA.Administration.StoreAdmin.DataAccess
Imports WholeFoods.IRMA.Replenishment.Common.DataAccess
Imports WholeFoods.Utility
Imports WholeFoods.Utility.DataAccess

Public Class Form_ManageStores
#Region "Class Level Vars and Property Definitions"
    ''' <summary>
    ''' Value of the current regional configuration settings
    ''' </summary>
    ''' <remarks></remarks>
    Private _regionalConfig As RegionBO
    Dim WithEvents editRegionalForm As Form_EditRegionalSettings
    Dim WithEvents editFtpConfigForm As Form_ManageStoreFtpInfo
    Dim WithEvents editStoreForm As Form_EditStore
#End Region

#Region "Events handled by this form"
#Region "Load Form"
    Private Sub Form_ManageStores_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.CenterToScreen()
        ' load the stores
        BindStoreData()
        ' load the regional settings
        BindRegionalData()
    End Sub

    Private Sub BindStoreData()
        Try
            ' Read the Store data
            DataGridView_ConfigItems.DataSource = StoreDAO.GetStores

            ' Format the view
            ' Make sure at least one entry was returned before configuring the columns
            If (DataGridView_ConfigItems.Columns.Count > 0) Then
                DataGridView_ConfigItems.Columns("RegionalOffice").Visible = False
                DataGridView_ConfigItems.Columns("PhoneNo").Visible = False
                DataGridView_ConfigItems.Columns("POSSystem").Visible = False

                DataGridView_ConfigItems.Columns("StoreNo").DisplayIndex = 0
                DataGridView_ConfigItems.Columns("StoreNo").HeaderText = ResourcesAdministration.GetString("label_storeNo")
                DataGridView_ConfigItems.Columns("StoreNo").ReadOnly = True

                DataGridView_ConfigItems.Columns("StoreName").DisplayIndex = 1
                DataGridView_ConfigItems.Columns("StoreName").HeaderText = ResourcesAdministration.GetString("label_storeName")
                DataGridView_ConfigItems.Columns("StoreName").ReadOnly = True

                DataGridView_ConfigItems.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            End If
        Catch e As DataFactoryException
            Logger.LogError("Exception: ", Me.GetType(), e)
            'display a message to the user
            DisplayErrorMessage(ERROR_DB)
            'send message about exception
            Dim args(1) As String
            args(0) = "Form_ManageStores form: BindData sub"
            ErrorHandler.ProcessError(ErrorType.POSPush_AdminError, args, SeverityLevel.Warning)
        End Try
    End Sub

    Private Sub BindRegionalData()
        _regionalConfig = New RegionBO()

        'check that valid regional Store entry exists
        If _regionalConfig.RegionalStore.StoreNo <= 0 Then
            'display warning message and disable all entry buttons
            Me.Label_RegionalStoreError.Visible = True
            Me.CheckBox_RegionalScale.Enabled = False
            Me.Button_EditRegionalSettings.Enabled = False
            Me.Button_FTPInfo.Enabled = False
        Else
            'enable all buttons and hide warning msg
            Me.Label_RegionalStoreError.Visible = False
            Me.CheckBox_RegionalScale.Enabled = True
            Me.Button_EditRegionalSettings.Enabled = True
            Me.Button_FTPInfo.Enabled = True

            'is regional scale file?
            Dim isRegionalScaleFile As Boolean = False
            If InstanceDataDAO.IsFlagActive("UseRegionalScaleFile") Then
                isRegionalScaleFile = True
            End If

            'setup entry screen
            CheckBox_RegionalScale.Checked = isRegionalScaleFile
        End If

        If CheckBox_RegionalScale.Checked Then
            ' Populate the scale writers
            Label_CorpWriterVal.Text = _regionalConfig.CorpScaleWriter.ScaleFileWriterCode()
            Label_ZoneWriterVal.Text = _regionalConfig.ZoneScaleWriter.ScaleFileWriterCode()
            ' Enable the FTP button
            Button_FTPInfo.Enabled = True
        Else
            ' The scale writers are not applicable - they are defined at the store level
            Label_CorpWriterVal.Text = "N/A"
            Label_ZoneWriterVal.Text = "N/A"
            ' Disable the FTP button
            Button_FTPInfo.Enabled = False
        End If
    End Sub
#End Region

#Region "Updates made to child form"
    ''' <summary>
    ''' Changes were made to the writer configurations.  Refresh the writer table.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub editRegionalForm_UpdateCallingForm() Handles editRegionalForm.UpdateCallingForm
        ' refresh the regional settings
        BindRegionalData()
    End Sub

    ''' <summary>
    ''' changes were made to store data.  refresh store list.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub editStoreForm_UpdateCallingForm() Handles editStoreForm.UpdateCallingForm
        BindStoreData()
    End Sub
#End Region

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

    Private Sub Button_Edit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Edit.Click
        editStoreForm = New Form_EditStore

        ' Get the selected row
        Dim selectedRow As DataGridViewRow = getSelectedRow(DataGridView_ConfigItems)
        If selectedRow IsNot Nothing Then
            ' Populate the edit form with the values from the selected row
            editStoreForm.StoreBO = New StoreBO(selectedRow)
            editStoreForm.RegionalScaleHosting = _regionalConfig.UseRegionalScaleFlag

            'open form
            editStoreForm.ShowDialog(Me)
            editStoreForm.Close()
            editStoreForm.Dispose()
        End If
    End Sub

    Private Sub Button_EditRegionalSettings_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_EditRegionalSettings.Click
        ' Populate the edit form with the values from the db
        editRegionalForm = New Form_EditRegionalSettings
        editRegionalForm.RegionalConfig = _regionalConfig

        'open form
        editRegionalForm.ShowDialog(Me)
        editRegionalForm.Close()
        editRegionalForm.Dispose()
    End Sub

    Private Sub Button_FTPInfo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_FTPInfo.Click
        ' Populate the edit form with the values from the db
        editFtpConfigForm = New Form_ManageStoreFtpInfo
        editFtpConfigForm.StoreConfig = _regionalConfig.RegionalStore

        'open form
        editFtpConfigForm.ShowDialog(Me)
        editFtpConfigForm.Close()
        editFtpConfigForm.Dispose()
    End Sub

    Private Sub Button_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Close.Click
        Me.Close()
    End Sub
#End Region

    Private Sub Button_Add_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Add.Click
        DisplayErrorMessage("This feature is not implemented.")
    End Sub

End Class