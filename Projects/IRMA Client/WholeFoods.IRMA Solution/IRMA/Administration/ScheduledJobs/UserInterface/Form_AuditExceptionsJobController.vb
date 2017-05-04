Option Strict Off
Option Explicit On
Imports System.Configuration
Imports WholeFoods.Utility
Imports WholeFoods.Utility.FTP
Imports WholeFoods.Utility.DataAccess

Public Class Form_AuditExceptionsJobController

    Private Const PROCESS_NAME As String = "Audit Exceptions Report"

    Private Sub Form_AuditExceptionsJobController_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ' TODO: Any way to check to see if Audit Exceptions Report is already running?
    End Sub

    Private Sub Button_StartJob_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_StartJob.Click
        Try
            ' Disable the button while the process is running and update the status on the UI
            Button_StartJob.Enabled = False
            Label_JobStatus.Text = PROCESS_NAME & " process is executing."
            Label_ExceptionText.Visible = False
            Me.Refresh()

            ' Start the job
            Call RunReport()

            Button_StartJob.Enabled = True
            Label_JobStatus.Text = PROCESS_NAME & " process has been run."
            Me.Refresh()
        Catch e1 As Exception
            ' An error occurred during processing - display a message and enable the button
            Label_JobStatus.Text = "Error during " & PROCESS_NAME & " process: " & e1.Message()
            Label_ExceptionText.Text = e1.ToString()
            Label_ExceptionText.Visible = True
            Button_StartJob.Enabled = True
            Me.Refresh()
        End Try
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks>Code copied from $\WholeFoods\IRMA\IRMA Client\WholeFoods.IRMA Solution\IRMA\Reporting\UserInterface\VendorReports.vb</remarks>
    Private Function RunReport() As Boolean

        'Dim oRow As Infragistics.Win.UltraWinGrid.UltraGridRow
        Dim sReportURL As New System.Text.StringBuilder
        'Dim sStoreNoList As String = String.Empty

        'Const DELIM As Char = ","   'list separator

        ''validate input
        'If glVendorID = 0 Then
        '    MsgBox("No vendor was selected.", MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, Me.Text)
        '    Exit Function
        'End If

        '--------------------------
        ' Setup Report Parameters.
        '--------------------------
        'report name
        sReportURL.Append("PirisAudit")

        'report display
        sReportURL.Append("&rs:Command=Render")
        sReportURL.Append("&rc:Parameters=False")

        'sReportURL.Append("&Vendor_ID=" & glVendorID)

        'Select Case True
        '    Case optRegionSingleRpt.Checked
        '        sReportURL.Append("&IsRegional=True")

        '    Case optZoneSingleRpt.Checked
        '        sReportURL.Append("&Zone_ID=" & VB6.GetItemData(cmbZoneSingleRpt, cmbZoneSingleRpt.SelectedIndex))

        '    Case optEachStore.Checked
        '        'Create list of stores for report from those selected in the grid
        '        For Each oRow In ugrdStoreList.Selected.Rows
        '            sStoreNoList += DELIM & oRow.Cells("Store_No").Value.ToString
        '        Next
        '        If Len(sStoreNoList) = 0 Then
        '            MsgBox("At least one store must be selected.", MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, Me.Text)
        '            Exit Function
        '        Else
        '            'remove the leading delimiter and append the parameter
        '            sReportURL.Append("&Store_No_List=" & Mid(sStoreNoList, Len(DELIM)))
        '        End If

        'End Select

        'If cmbTeam.SelectedIndex > -1 Then
        '    sReportURL.Append("&Team_No=" & VB6.GetItemData(cmbTeam, cmbTeam.SelectedIndex))
        'End If

        'If cmbSubTeam.SelectedIndex > -1 Then
        '    sReportURL.Append("&SubTeam_No=" & VB6.GetItemData(cmbSubTeam, cmbSubTeam.SelectedIndex))
        'End If

        'If cmbCategory.SelectedIndex > -1 Then
        '    sReportURL.Append("&Category_ID=" & VB6.GetItemData(cmbCategory, cmbCategory.SelectedIndex))
        'End If

        'If cmbBrand.SelectedIndex > -1 Then
        '    sReportURL.Append("&Brand_ID=" & VB6.GetItemData(cmbBrand, cmbBrand.SelectedIndex))
        'End If

        'If Len(txtUPC.Text) <> 0 Then
        '    If Not (IsNumeric(txtUPC.Text)) Or InStr(1, txtUPC.Text, ".") <> 0 Or InStr(1, txtUPC.Text, "$") <> 0 Then
        '        MsgBox("UPC may only contain numbers.", MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, Me.Text)
        '        Exit Function
        '    Else
        '        sReportURL.Append("&Identifier=" & txtUPC.Text)
        '    End If
        'End If

        '--------------
        ' Display Report.
        '--------------
        Call ReportingServicesReport(sReportURL.ToString)

        Return True

    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sReportURL"></param>
    ''' <remarks>Candidate for reuse in global module; Code copied from </remarks>
    Private Sub ReportingServicesReport(ByRef sReportURL As String)
        '-------------------------------------------
        ' Purpose: Runs a Reporting Services report.
        '-------------------------------------------

        ' WRONG: WebBrowser is a control. Start a new process instead.
        'Dim Browser As New WebBrowser

        'Doesn't work... fix later...
        'Browser.Width = Screen.PrimaryScreen.Bounds.Width
        'Browser.Height = Screen.PrimaryScreen.Bounds.Height

        'Browser.Navigate(sReportingServicesURL & sReportURL, True)
        'Browser.Visible = True

        Dim sReportingServicesURL As String = ConfigurationServices.AppSettings("reportingServicesURL")
        System.Diagnostics.Process.Start(sReportingServicesURL)

    End Sub


#Region "Close button"
    ''' <summary>
    ''' The close button returns the user to the calling form.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Button_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Close.Click
        Me.Close()
    End Sub

    ''' <summary>
    ''' The user clicked the 'X' button in top-right of window to close the form.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Form_AuditExceptionsJobController_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        ' No additional processing is needed because data can't be changed on this form
    End Sub
#End Region

    Private Sub btnUploadFiles_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUploadFiles.Click

        ''Dim ftpUtil As FTPclient = New FTP.FTPclient("")
        'Dim fdl As New OpenFileDialog
        'fdl.InitialDirectory = "c:\"
        'fdl.Multiselect = False
        'fdl.ShowDialog()

        'If fdl.FileName = String.Empty Then
        '    MsgBox("no file selected")
        '    Exit Sub
        'End If

        ''1. select directory rather than individual file?
        ''2. then read directory to see which stores are represented (include file dates?)
        ''3. put stores/filenames into a grid for selected stores to be processed?


        ''When creating and FTPClient instance, there is a new Boolean Flag to use SecureFTP (TLS) or not.
        'Dim ftp As New FTP.FTPclient("cen-eudyr", "robin", "eudy", True)

        ''This is how the object creation would look for NON secure ftp. 
        ''If a boolean flag is not supplied it will default to False.
        ''Dim ftp As New FTP.FTPclient("cen-eudyr", "robin", "eudy")
        'If ftp.Upload(fdl.FileName) Then
        '    MessageBox.Show("File Upload succeeded.")
        'Else
        '    MessageBox.Show("File Upload Failed.")
        'End If

    End Sub

    Private Sub btnImportFiles_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImportFiles.Click

        Dim fdl As New OpenFileDialog
        Dim sFileList As String = String.Empty
        Dim i As Int16
        Dim iImported As Int16
        Dim iResult As MsgBoxResult

        'set file dialog defaults
        fdl.InitialDirectory = "C:\"
        fdl.Multiselect = True
        fdl.Title = "Select files to import into the database"
        fdl.Filter = "SAL files (*.sal)|*.sal|Text files (*.txt)|*.txt|All files (*.*)|*.*"
        fdl.FilterIndex = 1
        fdl.ShowDialog()

        If fdl.FileName = String.Empty Then
            MsgBox("No files were selected to import!", MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, "Import PIRIS Files")
        Else
            For i = LBound(fdl.FileNames) To UBound(fdl.FileNames)
                sFileList = sFileList & vbCrLf & " " & Chr(149) & " " & fdl.FileNames(i)
            Next i

            If LBound(fdl.FileNames) = UBound(fdl.FileNames) Then
                'only 1 file selected
                sFileList = "The following file was selected to import:" & vbTab & vbCrLf _
                            & sFileList & vbCrLf & vbCrLf & "Import the selected file?"
            Else
                sFileList = "The following files were selected to import:" & vbTab & vbCrLf _
                            & sFileList & vbCrLf & vbCrLf & "Import the selected files?"
            End If

            iResult = MsgBox(sFileList, MsgBoxStyle.Information + MsgBoxStyle.OkCancel, "Import PIRIS Files")

            If iResult = MsgBoxResult.Ok Then
                'import the selected files
                Windows.Forms.Cursor.Current = Cursors.WaitCursor
                For i = LBound(fdl.FileNames) To UBound(fdl.FileNames)
                    If ImportFile(fdl.FileNames(i)) Then
                        iImported = iImported + 1
                    End If
                Next i
                Windows.Forms.Cursor.Current = Cursors.Default

                sFileList = "Successfully imported " & iImported.ToString & " of " & i.ToString & " files."

                MsgBox(sFileList, MsgBoxStyle.Information + MsgBoxStyle.OkOnly, "Import PIRIS Files")
            End If
        End If

        fdl.Dispose()
    End Sub

    Private Function ImportFile(ByRef FullFileName As String) As Boolean
        '-----------------------------------------------------
        ' Purpose: Bulk load file into the database.
        '-----------------------------------------------------
        Dim factory As DataAccess.DataFactory
        Dim currentParam As DataAccess.DBParam
        Dim paramList As ArrayList = New ArrayList

        Dim sFileNameParts() As String
        Dim sFilePath As String = String.Empty
        Dim sFileName As String = String.Empty

        sFileNameParts = Split(FullFileName, "\")

        sFileName = sFileNameParts(UBound(sFileNameParts))

        sFilePath = Replace(FullFileName, sFileName, String.Empty)

        'add input parameters
        currentParam = New DBParam
        currentParam.Name = "@FilePath"
        currentParam.Type = DBParamType.String
        'TSP 08/26/2006: supply the file path as null until the db server can grab remote files; default is to use local share folder on db server
        currentParam.Value = System.DBNull.Value 'sFilePath
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "@FileName"
        currentParam.Type = DBParamType.String
        currentParam.Value = sFileName
        paramList.Add(currentParam)

        Try
            'set the database connection
            factory = New DataAccess.DataFactory(DataFactory.ItemCatalog)

            factory.ExecuteStoredProcedure("Reporting_PIRIS_ImportFile", paramList)

            ImportFile = True
        Catch e As Exception
            Debug.WriteLine("ImportFile failed: " & e.InnerException.Message & vbCrLf & "File: " & FullFileName)
            MsgBox("ImportFile failed: " & e.InnerException.Message & vbCrLf & "File: " & FullFileName, MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, MyBase.Name & ".ImportFile")
        Finally
            currentParam = Nothing
            paramList.Clear()
            paramList = Nothing
        End Try

    End Function
End Class