Imports WholeFoods.IRMA.Replenishment.Jobs
Public Class Form_PLUMHostJobController

    Private Sub Form_PLUMHostJobController_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ' TODO: Any way to check to see if PLUM Process is already running?
    End Sub

    Private Sub ProcessPLUMRequest(ByVal FTPOnly As Boolean)
        Try
            ' Disable the button while the process is running and update the status on the UI
            Button_StartJob.Enabled = False
            Button_StartStoreFTP.Enabled = False
            Label_ExceptionText.Visible = False
            Me.Refresh()

            If FTPOnly Then
                Label_JobStatus.Text = "PLUM Store FTP process is executing."
                Me.Refresh()
                ' Start the extract job - but only run the FTP part
                Dim storeExtract As New PlumStoreExtractJob
                storeExtract.PlumAppDirectory = TextBox_PlumDir.Text
                storeExtract.RunExtractProcess = False
                Dim storeSuccess As Boolean = storeExtract.Main()
                ' The job finished executing - update the status and enable the button
                If storeSuccess Then
                    Label_JobStatus.Text = "PLUM Store FTP process completed successfully. "
                Else
                    ' Display the error status, including the error message if there is one.
                    If storeExtract.ErrorMessage IsNot Nothing Then
                        Label_JobStatus.Text = "Error during Store FTP process: " & storeExtract.ErrorMessage()
                    Else
                        Label_JobStatus.Text = "Error during Store FTP process."
                    End If

                    ' Show the exception stack trace if it's available.
                    If storeExtract.ErrorException IsNot Nothing Then
                        Label_ExceptionText.Text = storeExtract.ErrorException.ToString()
                        Label_ExceptionText.Visible = True
                    End If
                End If
            Else
                Label_JobStatus.Text = "PLUM Host process is executing."
                Me.Refresh()
                ' Start the import job - which will call the extract job if the user selected that UI option
                Dim plumJob As PlumHostImportJob = New PlumHostImportJob
                plumJob.IncludeStoreExtract = CheckBox_IncludeExport.Checked
                plumJob.PlumAppDirectory = TextBox_PlumDir.Text
                Dim jobStatus As Boolean = plumJob.Main()

                ' The job finished executing - update the status and enable the button
                If jobStatus Then
                    Label_JobStatus.Text = "PLUM Host process completed successfully. "
                Else
                    ' Display the error status, including the error message if there is one.
                    If plumJob.ErrorMessage IsNot Nothing Then
                        Label_JobStatus.Text = "Error during PLUM Host process: " & plumJob.ErrorMessage()
                    Else
                        Label_JobStatus.Text = "Error during PLUM Host process."
                    End If

                    ' Show the exception stack trace if it's available.
                    If plumJob.ErrorException IsNot Nothing Then
                        Label_ExceptionText.Text = plumJob.ErrorException.ToString()
                        Label_ExceptionText.Visible = True
                    End If
                End If
            End If

            Button_StartJob.Enabled = True
            Button_StartStoreFTP.Enabled = True
            Me.Refresh()
        Catch e1 As Exception
            ' An error occurred during processing - display a message and enable the button
            Label_JobStatus.Text = "Error during PLUM Host process: " & e1.Message()
            Label_ExceptionText.Text = e1.ToString()
            Label_ExceptionText.Visible = True
            Button_StartJob.Enabled = True
            Button_StartStoreFTP.Enabled = True
            Me.Refresh()
        End Try
    End Sub

    ''' <summary>
    ''' The user selected the button to kick-off the process.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Button_StartJob_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_StartJob.Click
        ProcessPLUMRequest(False)
    End Sub

    ''' <summary>
    ''' The user clicked on the button to select a PLUM install directory
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Button_PlumInstallDir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_PlumInstallDir.Click
        Me.FolderBrowserDialog_PlumInstall.ShowDialog()
        Me.TextBox_PlumDir.Text = Me.FolderBrowserDialog_PlumInstall.SelectedPath
    End Sub

    Private Sub Button_StartStoreFTP_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_StartStoreFTP.Click
        ProcessPLUMRequest(True)
    End Sub
End Class