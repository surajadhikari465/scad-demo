Imports WholeFoods.IRMA.Administration.StoreAdmin.DataAccess
Imports WholeFoods.IRMA.Replenishment.Jobs

Public Class Form_BuildFullScaleFileForStore
    Dim WithEvents scaleJob As BuildStoreScaleFileJob

    Private Sub Form_BuildFullScaleFileForStore_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        BindStoreData()
    End Sub

#Region "Update UI based on scheduled job events"
    Private Sub scaleJob_ScalePushStarted(ByVal IsRegional As Boolean) Handles scaleJob.ScalePushStarted
        CheckBox_ScaleStarted.Checked = True
        Label_ScaleRegional.Text = Label_ScaleRegional.Text & IsRegional.ToString
        Me.Refresh()
    End Sub

    Private Sub scaleJob_ScaleReadStoreConfigurationData(ByVal NumStores As Integer) Handles scaleJob.ScaleReadStoreConfigurationData
        CheckBox_ScaleStores.Checked = True
        Label_ScalesStoreCount.Text = Label_ScalesStoreCount.Text & NumStores.ToString
        Me.Refresh()
    End Sub

    Private Sub scaleJob_ScaleReadScaleItemsForStore() Handles scaleJob.ScaleReadScaleItemsForStore
        CheckBox_FullScaleData.Checked = True
        Me.Refresh()
    End Sub

    Private Sub scaleJob_ScaleTransferFiles(ByVal FileStatus As String) Handles scaleJob.ScaleTransferFiles
        CheckBox_ScaleFTP.Checked = True
        TextBox_ScaleFTPStatus.AppendText(FileStatus)
        Me.Refresh()
    End Sub

    Private Sub scaleJob_ScaleCompleteSuccess() Handles scaleJob.ScaleCompleteSuccess
        CheckBox_ScaleSuccess.Checked = True
        Me.Refresh()
    End Sub
#End Region

    Private Sub ResetUIStatus()
        Button_StartJob.Enabled = False
        Label_JobStatus.Text = "Build Store File process is executing."
        Label_ExceptionText.Visible = False
        CheckBox_ScaleStarted.Checked = False
        Label_ScaleRegional.Text = "Regional Scale Configuration?  "
        CheckBox_ScaleStores.Checked = False
        Label_ScalesStoreCount.Text = "# Stores Read?  "
        CheckBox_FullScaleData.Checked = False
        CheckBox_ScaleFTP.Checked = False
        TextBox_ScaleFTPStatus.Clear()
        CheckBox_ScaleSuccess.Checked = False

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
                scaleJob = New BuildStoreScaleFileJob

                'set the store number selected by the user
                scaleJob.StoreNo = CType(Me.ComboBox_StoreList.SelectedValue, Integer)

                Dim jobStatus As Boolean = scaleJob.Main()

                ' The job finished executing - update the status and enable the button
                If jobStatus Then
                    Label_JobStatus.Text = "Build Store File process completed successfully. "
                Else
                    ' Display the error status, including the error message if there is one.
                    If scaleJob.ErrorMessage IsNot Nothing Then
                        Label_JobStatus.Text = "Error during Build Store File process: " & scaleJob.ErrorMessage()
                    Else
                        Label_JobStatus.Text = "Error during Build Store File process."
                    End If

                    ' Show the exception stack trace if it's available.
                    If scaleJob.ErrorException IsNot Nothing Then
                        Label_ExceptionText.Text = scaleJob.ErrorException.ToString()
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
