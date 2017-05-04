Imports WholeFoods.IRMA.Administration.StoreAdmin.DataAccess
Imports WholeFoods.IRMA.Replenishment.Jobs

Public Class Form_BuildFullESTFileForStore
    Dim WithEvents estJob As BuildStoreESTFileJob

    Private Sub Form_BuildFullESTFileForStore_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        BindStoreData()
    End Sub

#Region "Update UI based on scheduled job events"
    Private Sub estJob_ESTPushStarted() Handles estJob.ESTPushStarted
        CheckBox_ESTStarted.Checked = True
        Me.Refresh()
    End Sub

    Private Sub estJob_ScaleReadStoreConfigurationData(ByVal NumStores As Integer) Handles ESTJob.ESTReadStoreConfigurationData
        CheckBox_ESTStores.Checked = True
        Label_ESTStoreCount.Text = Label_ESTStoreCount.Text & NumStores.ToString
        Me.Refresh()
    End Sub

    Private Sub estJob_ScaleReadScaleItemsForStore() Handles ESTJob.ESTReadScaleItemsForStore
        CheckBox_FullESTData.Checked = True
        Me.Refresh()
    End Sub

    Private Sub estJob_ScaleTransferFiles(ByVal FileStatus As String) Handles ESTJob.ESTTransferFiles
        CheckBox_ESTFTP.Checked = True
        TextBox_ESTFTPStatus.AppendText(FileStatus)
        Me.Refresh()
    End Sub

    Private Sub estJob_ScaleCompleteSuccess() Handles ESTJob.ESTCompleteSuccess
        CheckBox_ESTSuccess.Checked = True
        Me.Refresh()
    End Sub
#End Region

    Private Sub ResetUIStatus()
        Button_StartJob.Enabled = False
        Label_JobStatus.Text = "Build Store Electronic Shelf Tag File process is executing."
        Label_ExceptionText.Visible = False
        CheckBox_ESTStarted.Checked = False
        CheckBox_ESTStores.Checked = False
        Label_ESTStoreCount.Text = "# Stores Read?  "
        CheckBox_FullESTData.Checked = False
        CheckBox_ESTFTP.Checked = False
        TextBox_ESTFTPStatus.Clear()
        CheckBox_ESTSuccess.Checked = False

        Me.Refresh()
    End Sub

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
                'Dim TagPush As New TagPriceBatchProcessor(0)
                'Dim jobStatus As Boolean = TagPush.BuildFullElectronicShelfTagFile(CType(Me.ComboBox_StoreList.SelectedValue, Integer))
                estJob = New BuildStoreESTFileJob
                Dim jobStatus As Boolean = estJob.Main((CType(Me.ComboBox_StoreList.SelectedValue, Integer)))

                ' The job finished executing - update the status and enable the button
                If jobStatus Then
                    Label_JobStatus.Text = "Build Store File process completed successfully. "
                Else
                    ' Display the error status, including the error message if there is one.
                    If estJob.ErrorException IsNot Nothing Then
                        Label_JobStatus.Text = "Error during Build Store File process: " & estJob.ErrorException.Message
                        Label_ExceptionText.Text = estJob.ErrorException.ToString()
                        Label_ExceptionText.Visible = True
                    Else
                        Label_JobStatus.Text = "Error during Build Store File process."
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
