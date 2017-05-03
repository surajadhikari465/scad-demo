Imports WholeFoods.IRMA.Administration.StoreAdmin.DataAccess
Imports WholeFoods.IRMA.Replenishment.Jobs

Public Class Form_AuditReportJobController
    Private WithEvents auditReportJob As POSAuditReport

#Region "Update UI based on scheduled job events"
    Private Sub auditReportJob_POSCompleteError() Handles auditReportJob.POSCompleteError
        Me.Refresh()
    End Sub

    Private Sub auditReportJob_POSCompleteSuccess() Handles auditReportJob.POSCompleteSuccess
        CheckBox_POSSuccess.Checked = True
        Me.Refresh()
    End Sub

    Private Sub auditReportJob_POSGeneratedPOSControlFiles() Handles auditReportJob.POSGeneratedPOSControlFiles
        CheckBox_POSControlFiles.Checked = True
        Me.Refresh()
    End Sub

    Private Sub auditReportJob_POSPushStarted() Handles auditReportJob.POSPushStarted
        CheckBox_POSStarted.Checked = True
        Me.Refresh()
    End Sub

    Private Sub auditReportJob_POSReadItemIdAdds() Handles auditReportJob.POSReadItemIdAdds
        CheckBox_POSItemIdAdd.Checked = True
        Me.Refresh()
    End Sub

    Private Sub auditReportJob_POSReadStoreConfigurationData(ByVal NumStores As Integer) Handles auditReportJob.POSReadStoreConfigurationData
        CheckBox_POSReadStore.Checked = True
        Label_POSStoreCount.Text = Label_POSStoreCount.Text & NumStores.ToString
        Me.Refresh()
    End Sub

    Private Sub auditReportJob_POSReadVendorAdds() Handles auditReportJob.POSReadVendorAdds
        CheckBox_POSVendorAdd.Checked = True
        Me.Refresh()
    End Sub

    Private Sub auditReportJob_POSStartedRemoteJobs() Handles auditReportJob.POSStartedRemoteJobs
        CheckBox_POSRemoteJobs.Checked = True
        Me.Refresh()
    End Sub

    Private Sub auditReportJob_POSTransferFiles(ByVal FileStatus As String) Handles auditReportJob.POSTransferFiles
        CheckBox_POSFileTransfer.Checked = True
        TextBox_POSFTPStatus.AppendText(FileStatus)
        Me.Refresh()
    End Sub

    Private Sub auditReportJob_POSSSHRemoteExecution(ByVal FileStatus As String) Handles auditReportJob.POSSSHRemoteExecution
        TextBox_SSHStatus.AppendText(FileStatus)
        Me.Refresh()
    End Sub

    'Private Sub auditReportJob_ScaleCompleteError() Handles auditReportJob.ScaleCompleteError
    '    Me.Refresh()
    'End Sub

    'Private Sub auditReportJob_ScaleCompleteSuccess() Handles auditReportJob.ScaleCompleteSuccess
    '    CheckBox_ScaleSuccess.Checked = True
    '    Me.Refresh()
    'End Sub

    'Private Sub auditReportJob_ScaleCorpTempQueueCleared() Handles auditReportJob.ScaleCorpTempQueueCleared
    '    CheckBox_ScaleTempQueue.Checked = True
    '    Me.Refresh()
    'End Sub

    'Private Sub auditReportJob_ScalePushStarted(ByVal IsRegional As Boolean) Handles auditReportJob.ScalePushStarted
    '    CheckBox_ScaleStarted.Checked = True
    '    Label_ScaleRegional.Text = Label_ScaleRegional.Text & IsRegional.ToString
    '    Me.Refresh()
    'End Sub

    'Private Sub auditReportJob_ScaleReadItemIdAdds() Handles auditReportJob.ScaleReadItemIdAdds
    '    CheckBox_ScaleItemIdAdd.Checked = True
    '    Me.Refresh()
    'End Sub

    'Private Sub auditReportJob_ScaleReadStoreConfigurationData(ByVal NumStores As Integer) Handles auditReportJob.ScaleReadStoreConfigurationData
    '    CheckBox_ScaleStores.Checked = True
    '    Label_ScalesStoreCount.Text = Label_ScalesStoreCount.Text & NumStores.ToString
    '    Me.Refresh()
    'End Sub

    'Private Sub auditReportJob_ScaleTransferFiles(ByVal FileStatus As String) Handles auditReportJob.ScaleTransferFiles
    '    CheckBox_ScaleFTP.Checked = True
    '    TextBox_ScaleFTPStatus.AppendText(FileStatus)
    '    Me.Refresh()
    'End Sub
#End Region

    Private Sub Form_AuditReportJobController_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ' TODO: Any way to check to see if Audit Report is already running?

        BindStoreData()
    End Sub

    Private Sub ResetUIStatus()
        Button_StartJob.Enabled = False
        Label_JobStatus.Text = "Build Store File process is executing."
        Label_ExceptionText.Visible = False
        CheckBox_POSControlFiles.Checked = False
        CheckBox_POSFileTransfer.Checked = False
        TextBox_POSFTPStatus.Clear()
        CheckBox_POSItemIdAdd.Checked = False
        CheckBox_POSReadStore.Checked = False
        Label_POSStoreCount.Text = "# Stores Read?  "
        CheckBox_POSRemoteJobs.Checked = False
        TextBox_SSHStatus.Clear()
        CheckBox_POSStarted.Checked = False
        CheckBox_POSSuccess.Checked = False
        CheckBox_POSVendorAdd.Checked = False
        'CheckBox_ScaleFTP.Checked = False
        'TextBox_ScaleFTPStatus.Clear()
        'CheckBox_ScaleItemIdAdd.Checked = False
        'CheckBox_ScaleStarted.Checked = False
        'Label_ScaleRegional.Text = "Regional Scale Configuration?  "
        'CheckBox_ScaleStores.Checked = False
        'Label_ScalesStoreCount.Text = "# Stores Read?  "
        'CheckBox_ScaleSuccess.Checked = False
        'CheckBox_ScaleTempQueue.Checked = False

        Me.Refresh()

    End Sub

    ''' <summary>
    ''' The user selected the button to kick-off the Audit Report process.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Button_StartJob_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_StartJob.Click
        'validate that user has selected a store
        If Me.ComboBox_StoreList.SelectedIndex < 0 Then
            'prompt user
            MessageBox.Show(ResourcesAdministration.GetString("msg_validation_selectStore"), Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Stop)
        Else
            Try
                ' Disable the button while the process is running and update the status on the UI
                Windows.Forms.Cursor.Current = Cursors.WaitCursor
                ' Reset the status messages
                ResetUIStatus()

                ' Start the job
                auditReportJob = New POSAuditReport(chkDeleteFiles.Checked)

                'set the store number selected by the user
                auditReportJob.StoreNo = CType(Me.ComboBox_StoreList.SelectedValue, Integer)

                Dim jobStatus As Boolean = auditReportJob.Main()

                ' The job finished executing - update the status and enable the button
                If jobStatus Then
                    Label_JobStatus.Text = "Build Store File process completed successfully. "
                Else
                    ' Display the error status, including the error message if there is one.
                    If auditReportJob.ErrorMessage IsNot Nothing Then
                        Label_JobStatus.Text = "Error during Build Store File process: " & auditReportJob.ErrorMessage()
                    Else
                        Label_JobStatus.Text = "Error during Build Store File process."
                    End If

                    ' Show the exception stack trace if it's available.
                    If auditReportJob.ErrorException IsNot Nothing Then
                        Label_ExceptionText.Text = auditReportJob.ErrorException.ToString()
                        Label_ExceptionText.Visible = True
                    End If
                End If
            Catch e1 As Exception
                ' An error occurred during processing - display a message and enable the button
                Label_JobStatus.Text = "Error during Build Store File process: " & e1.Message()
                Label_ExceptionText.Text = e1.ToString()
                Label_ExceptionText.Visible = True
            Finally
                Button_StartJob.Enabled = True
                Windows.Forms.Cursor.Current = Cursors.Default
                Me.Refresh()
            End Try
        End If
    End Sub

    Private Sub BindStoreData()
        'bind data to store list drop down
        Me.ComboBox_StoreList.DataSource = StoreDAO.GetStores()
        Me.ComboBox_StoreList.DisplayMember = "StoreName"
        Me.ComboBox_StoreList.ValueMember = "StoreNo"
        Me.ComboBox_StoreList.SelectedIndex = -1
    End Sub

End Class
