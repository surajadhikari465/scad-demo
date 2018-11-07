Imports WholeFoods.IRMA.Replenishment.HART.DataAccess
Imports WholeFoods.Utility.FTP
Imports WholeFoods.Utility.SMTP
Imports WholeFoods.Utility
Imports System.Configuration
Imports System.IO
Imports log4net
Imports System.IO.Compression
Imports Ionic.Zip
Imports System.Data.SqlClient

Namespace WholeFoods.IRMA.Replenishment.HART.BusinessLogic
    Public Class HARTBO
        Private Shared CLASSTYPE As Type = System.Type.GetType("WholeFoods.IRMA.Replenishment.HART.BusinessLogic.HARTBO")
        Private log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
        Private errMessage As String

        Public Function DoHARTImport() As String
            Dim HARTDAO As New HARTDAO
            Dim sRegionCode As String = HARTDAO.GetRegionCode
            Dim aryFiles As FileInfo()
            Dim fi As FileInfo
            Dim iBusinessUnitID As Integer
            Dim strSuccessEmailText As String = String.Empty

            Dim sExtractFilePath As String = ConfigurationServices.AppSettings("DatabaseLocalPath")
            Dim sZipFileDownloadPath As String = ConfigurationServices.AppSettings("FileDownloadPath")
            Dim sErrorEmailAddress As String = ConfigurationServices.AppSettings("ErrorEmailAddress").ToString
            Dim sSuccessEmailAddress As String = ConfigurationServices.AppSettings("SuccessEmailAddress").ToString
            Dim bRunStoreOpsExport As Boolean = CBool(ConfigurationServices.AppSettings("RunStoreOpsExport"))
            Dim bUsePSSubTeamNoForImport As Boolean = CBool(ConfigurationServices.AppSettings("UsePSSubTeamNoForImport"))

            ' Use for Debug Mode
            '-------------------------------------------------------------
            'Dim sZipFileDownloadPath As String = "\\irmadevfile\Data\HART\SO\Import"
            'Dim sErrorEmailAddress As String = "amudha.sethuraman@wholefoods.com"
            'Dim sSuccessEmailAddress As String = "amudha.sethuraman@wholefoods.com"
            '-------------------------------------------------------------

            'Dim diDB As New DirectoryInfo(sDatabaseUNCPath)
            Dim diDB As New DirectoryInfo(sExtractFilePath) 'debug
            Dim diServer As New DirectoryInfo(sZipFileDownloadPath)

            Try
                ' ensure that the Database Local Path has a slash at the end
                If Mid(sExtractFilePath, Len(sExtractFilePath), 1) <> "\" Then sExtractFilePath = sExtractFilePath & "\"
                If Mid(sZipFileDownloadPath, Len(sZipFileDownloadPath), 1) <> "\" Then sZipFileDownloadPath = sZipFileDownloadPath & "\"

                'download zip files from HART to sZipFileDownloadPath and extract files to sExtractFilePath
                log.Info("Download all of the files from HART")
                DownloadFilesFromHART(sRegionCode)

                log.Info("Read all of the downloaded filenames")
                aryFiles = diDB.GetFiles()

                For Each fi In aryFiles
                    errMessage = ""
                    'DBS 20110110 - Add file names to email, remove email when no files are processed
                    strSuccessEmailText = strSuccessEmailText + vbNewLine + fi.Name
                    iBusinessUnitID = CInt(Mid(fi.Name, 1, InStr(fi.Name, "_") - 1))

                    'Task 8115, File reconciliation - check if line count matches header record on file
                    Try
                        log.Info("File reconciliation (" & fi.Name & ") - Check if line count and product count matches header record " & iBusinessUnitID.ToString)
                        If FileReconciled(sExtractFilePath & fi.Name) Then
                            log.Info("File reconciled  (" & fi.Name & ") - line count and product count matches header record " & iBusinessUnitID.ToString)

                            log.Info("Clear the InventoryServiceImportLoad table for business unit " & iBusinessUnitID.ToString)
                            HARTDAO.DeleteInventortServiceImportLoad(iBusinessUnitID)

                            'Task 1927 v 4.2.2, improve error handling during file load
                            Try
                                log.Info("Load the InventoryServiceImportLoad table for business unit " & iBusinessUnitID.ToString)
                                HARTDAO.LoadInventoryServiceImport(sExtractFilePath & fi.Name, bUsePSSubTeamNoForImport)

                            Catch fileLoadException As Exception
                                log.Info("The HART Import Job failed for the following reason, bad file has been moved to Error directory: " & vbNewLine & vbNewLine & fileLoadException.Message & " - " & fileLoadException.InnerException.Message)
                                SendMail(sErrorEmailAddress, "HART Import Job", "The HART Import failed for the following reason, bad file has been moved to Error directory:  " & vbNewLine & vbNewLine & fileLoadException.Message & " - " & fileLoadException.InnerException.Message)
                                'Move the bad file to Error directory
                                ArchiveFile(sExtractFilePath, sZipFileDownloadPath & "Error\", fi.Name, fi.Name)
                                'Remove the bad file name from the success message
                                strSuccessEmailText = strSuccessEmailText.Remove((strSuccessEmailText.Length - fi.Name.Length - vbNewLine.Length))
                            End Try

                            log.Info("Load the Cycle Count tables for business unit " & iBusinessUnitID.ToString)
                            HARTDAO.LoadCycleCountExternal(Now.Date, bUsePSSubTeamNoForImport)

                            log.Info("Finished processing file " & sExtractFilePath & fi.FullName & ".  File has been deleted.")

                            File.Delete(sExtractFilePath & fi.Name)

                        Else
                            Throw New IOException(errMessage)
                        End If

                    Catch ex As IOException
                        log.Info("HART Import Job failed for " & fi.Name & " due to the following reason: " & vbNewLine & ex.Message & vbNewLine & " This file has been moved to Error directory.")
                        SendMail(sErrorEmailAddress, "HART Import Job", "HART Import Job failed for " & fi.Name & " due to the following reason: " & vbNewLine & ex.Message & vbNewLine & " This file has been moved to Error directory.")
                        'Move the bad file to Error directory
                        ArchiveFile(sExtractFilePath, sZipFileDownloadPath & "Error\", fi.Name, fi.Name)
                        'Remove the bad file name from the success message
                        strSuccessEmailText = strSuccessEmailText.Remove((strSuccessEmailText.Length - fi.Name.Length - vbNewLine.Length))
                    End Try

                Next

                If bRunStoreOpsExport Then
                    log.Info("Generate StoreOps Inventory Export File")
                    DoStoreOpsExport()
                End If


                log.Info("Archive HART zip files")
                aryFiles = diServer.GetFiles()

                For Each fi In aryFiles
                    ArchiveFile(sZipFileDownloadPath, sZipFileDownloadPath & "Archive\" & sRegionCode & "\", fi.Name, fi.Name)
                Next

                log.Info("The HART Import job has completed successfully")

                If UBound(aryFiles) <> -1 Then
                    SendMail(sSuccessEmailAddress, "HART Import Job", "The HART Import job has completed successfully and following file(s) has\have been imported: " + vbNewLine + strSuccessEmailText)
                End If

                Return "The HART Import Job has completed successfully."
            Catch ex As Exception
                log.Info("The HART Import Job failed for the following reason: " & ex.Message)
                SendMail(sErrorEmailAddress, "HART Import Job", "The HART Import job did not complete due to the following error:  " & ex.Message)
                Return "The HART Import failed for the following reason: " & ex.Message
            End Try

        End Function

        Public Function DoHARTExport() As String
            Dim HARTDAO As New HARTDAO
            Dim dtHART As DataTable = Nothing
            Dim dtStores As DataTable = Nothing
            Dim iHARTId As Integer = 0
            Dim sRegionCode As String = ""
            Dim sPath As String = ConfigurationServices.AppSettings("ExportFileLocation").ToString
            Dim sErrorEmailAddress As String = ConfigurationServices.AppSettings("ErrorEmailAddress").ToString
            Dim sSuccessEmailAddress As String = ConfigurationServices.AppSettings("SuccessEmailAddress").ToString
            Dim blnFileExists As Boolean = False

            Dim sFileName As String = ""
            Dim sZipFileName As String = ""

            Dim sPeriod As String = ""
            Dim _dt As DataTable = Nothing
            Dim _dr As DataRow = Nothing
            _dt = HARTDAO.GetFiscalCalendarInfo(DateTime.Today)
            If _dt.Rows.Count > 0 Then
                _dr = _dt.Rows(0)
                sPeriod = _dr("FiscalPeriod").ToString.PadLeft(2, "0"c)
            End If

            Try
                If Mid(sPath, Len(sPath), 1) <> "\" Then sPath = sPath & "\"

                log.Info("Loading the InventoryServiceExportLoad table")

                If HARTDAO.LoadInventoryServiceExport() Then
                    blnFileExists = True
                    sRegionCode = HARTDAO.GetRegionCode
                    dtStores = HARTDAO.GetStoreBusinessUnits


                    For Each row As DataRow In dtStores.Rows
                        log.Info("Getting data from the InventoryServiceExportLoad table for business unit " & row("BusinessUnit_ID").ToString)
                        dtHART = HARTDAO.GetInventoryServiceExport(iHARTId, CInt(row("BusinessUnit_ID")))

                        If dtHART.Rows.Count > 0 Then

                            sFileName = sRegionCode & row("BusinessUnit_ID").ToString & "_" & Now.ToString("yyyy") & "fp" & sPeriod & "master.txt"
                            sZipFileName = sRegionCode & row("BusinessUnit_ID").ToString & "_" & Now.ToString("yyyy") & "fp" & sPeriod & "master.zip"

                            log.Info("Creating the text file " & sPath & sFileName & " for business unit " & row("BusinessUnit_ID").ToString)
                            CreateExportFile(dtHART, sPath, sFileName)

                            log.Info("Zipping " & sPath & sFileName & " to " & sPath & sZipFileName & " for business unit " & row("BusinessUnit_ID").ToString)
                            ZipHARTFile(sPath & sFileName, sPath & sZipFileName)

                            log.Info("FTPing the file for business unit " & row("BusinessUnit_ID").ToString)
                            FTPFile(sPath & sFileName)

                            log.Info("Archiving " & sZipFileName & " for business unit " & row("BusinessUnit_ID").ToString)
                            ArchiveFile(sPath, sPath & "Archive\" & Now.ToString("MMddyyyy") & "\" & sRegionCode & "\", sZipFileName, sFileName)

                        End If
                    Next

                    SendMail(sSuccessEmailAddress, "HART Export Job", "The HART Export job has completed successfully and all files have " & _
                                                                       "been delivered.")
                End If

                log.Info("The HART Job finished successfully.")
                If Not blnFileExists Then SendMail(sSuccessEmailAddress, "HART Export Job", "The HART Export job has completed " & _
                                                                                            "successfully, but no files were ready to be sent.")
                Return "The HART Export finished successfully."
            Catch ex As Exception
                log.Info("The HART Job failed for the following reason: " & ex.Message & " - " & ex.InnerException.Message)
                SendMail(sErrorEmailAddress, "HART Export Job", "The HART Export job did not complete due to the following error:  " & ex.Message & " - " & ex.InnerException.Message)
                Return "The HART Export failed for the following reason: " & ex.Message & " - " & ex.InnerException.Message
            End Try
        End Function
        Public Function DoHARTExportLoad() As String
            Dim HARTDAO As New HARTDAO
            Dim dtHART As DataTable = Nothing
            Dim dtStores As DataTable = Nothing
            Dim iHARTId As Integer = 0
            Dim sRegionCode As String = ""
            Dim sPath As String = ConfigurationServices.AppSettings("ExportFileLocation").ToString
            Dim sErrorEmailAddress As String = ConfigurationServices.AppSettings("ErrorEmailAddress").ToString
            Dim sSuccessEmailAddress As String = ConfigurationServices.AppSettings("SuccessEmailAddress").ToString
            Dim blnFileExists As Boolean = False

            Try
                If Mid(sPath, Len(sPath), 1) <> "\" Then sPath = sPath & "\"

                log.Info("Loading the InventoryServiceExportLoad table")

                If HARTDAO.LoadInventoryServiceExport() Then
                    blnFileExists = True

                    SendMail(sSuccessEmailAddress, "HART Export Job", "The HART Export Load job has completed successfully.")
                End If

                log.Info("The HART Export Load Job finished successfully.")
                If Not blnFileExists Then SendMail(sSuccessEmailAddress, "HART Export Load Job", "The HART Export Load job has completed " & _
                                                                                            "successfully, but no files were ready to be sent.")
                Return "The HART Export Load Job finished successfully."
            Catch ex As Exception
                log.Info("The HART Export Load Job failed for the following reason: " & ex.Message & " - " & ex.InnerException.Message)
                SendMail(sErrorEmailAddress, "HART Export Load Job", "The HART Export job did not complete due to the following error:  " & ex.Message & " - " & ex.InnerException.Message)
                Return "The HART Export Load Job failed for the following reason: " & ex.Message & " - " & ex.InnerException.Message
            End Try
        End Function

        Public Function DoHARTExportTransfer() As String
            Dim HARTDAO As New HARTDAO
            Dim dtHART As DataTable = Nothing
            Dim dtStores As DataTable = Nothing
            Dim iHARTId As Integer = 0
            Dim sRegionCode As String = ""
            Dim sPath As String = ConfigurationServices.AppSettings("ExportFileLocation").ToString
            Dim sErrorEmailAddress As String = ConfigurationServices.AppSettings("ErrorEmailAddress").ToString
            Dim sSuccessEmailAddress As String = ConfigurationServices.AppSettings("SuccessEmailAddress").ToString
            Dim blnFileExists As Boolean = False

            Dim sFileName As String = ""
            Dim sZipFileName As String = ""

            Dim sPeriod As String = ""
            Dim _dt As DataTable = Nothing
            Dim _dr As DataRow = Nothing
            _dt = HARTDAO.GetFiscalCalendarInfo(DateTime.Today)
            If _dt.Rows.Count > 0 Then
                _dr = _dt.Rows(0)
                sPeriod = _dr("FiscalPeriod").ToString.PadLeft(2, "0"c)
            End If

            Try
                If Mid(sPath, Len(sPath), 1) <> "\" Then sPath = sPath & "\"

                log.Info("Loading the InventoryServiceExportLoad table")

                blnFileExists = True
                sRegionCode = HARTDAO.GetRegionCode
                dtStores = HARTDAO.GetStoreBusinessUnits

                For Each row As DataRow In dtStores.Rows
                    log.Info("Getting data from the InventoryServiceExportLoad table for business unit " & row("BusinessUnit_ID").ToString)
                    dtHART = HARTDAO.GetInventoryServiceExport(iHARTId, CInt(row("BusinessUnit_ID")))

                    If dtHART.Rows.Count > 0 Then

                        sFileName = sRegionCode & row("BusinessUnit_ID").ToString & "_" & Now.ToString("yyyy") & "fp" & sPeriod & "master.txt"
                        sZipFileName = sRegionCode & row("BusinessUnit_ID").ToString & "_" & Now.ToString("yyyy") & "fp" & sPeriod & "master.zip"

                        log.Info("STARTED Creating the text file " & sPath & sFileName & " for business unit " & row("BusinessUnit_ID").ToString)
                        CreateExportFile(dtHART, sPath, sFileName)
                        log.Info("COMPLETED Creating the text file " & sPath & sFileName & " for business unit " & row("BusinessUnit_ID").ToString)


                        log.Info("STARTED Zipping " & sPath & sFileName & " to " & sPath & sZipFileName & " for business unit " & row("BusinessUnit_ID").ToString)
                        ZipHARTFile(sPath & sFileName, sPath & sZipFileName)
                        log.Info("COMPLETED Zipping " & sPath & sFileName & " to " & sPath & sZipFileName & " for business unit " & row("BusinessUnit_ID").ToString)

                        log.Info("STARTED FTPing the file for business unit " & row("BusinessUnit_ID").ToString)
                        FTPFile(sPath & sFileName)
                        log.Info("COMPLETED FTPing the file for business unit " & row("BusinessUnit_ID").ToString)

                        log.Info("STARTED Archiving " & sZipFileName & " for business unit " & row("BusinessUnit_ID").ToString)
                        ArchiveFile(sPath, sPath & "Archive\" & Now.ToString("MMddyyyy") & "\" & sRegionCode & "\", sZipFileName, sFileName)
                        log.Info("COMPLETED Archiving " & sZipFileName & " for business unit " & row("BusinessUnit_ID").ToString)
                    End If
                Next

                SendMail(sSuccessEmailAddress, "HART Export Job", "The HART Transfer Export job has completed successfully and all files have " & _
                                                                   "been delivered.")

                log.Info("The HART Job finished successfully.")
                If Not blnFileExists Then SendMail(sSuccessEmailAddress, "HART Export Job", "The HART Export job has completed " & _
                                                                                            "successfully, but no files were ready to be sent.")
                Return "The HART Export finished successfully."
            Catch ex As Exception
                log.Info("The HART Job failed for the following reason: " & ex.Message & " - " & ex.InnerException.Message)
                SendMail(sErrorEmailAddress, "HART Export Job", "The HART Export job did not complete due to the following error:  " & ex.Message & " - " & ex.InnerException.Message)
                Return "The HART Export failed for the following reason: " & ex.Message & " - " & ex.InnerException.Message
            End Try
        End Function
        Private Sub CreateExportFile(ByVal dt As DataTable, ByVal sFilePath As String, ByVal sFileName As String)
            Dim sw As StreamWriter

            If File.Exists(sFilePath & sFileName) Then File.Delete(sFilePath & sFileName)

            If Not Directory.Exists(sFilePath) Then
                Directory.CreateDirectory(sFilePath)
            End If

            sw = New StreamWriter(sFilePath & sFileName)

            For Each row As DataRow In dt.Rows
                sw.WriteLine(row("REGION").ToString & vbTab & row("STORE_NAME").ToString & vbTab & row("PS_BU").ToString & vbTab & _
                             row("PS_PROD_SUBTEAM").ToString & vbTab & row("PS_PROD_DESCRIPTION").ToString & vbTab & _
                             row("UPC").ToString & vbTab & row("DESCRIPTION").ToString & vbTab & row("EFF_PRICE").ToString & vbTab & _
                             row("AVG_COST").ToString & vbTab & row("SKU").ToString & vbTab & row("REG_VEND_NUM_CZ").ToString & vbTab & _
                             row("LONG_DESCRIPTION").ToString & vbTab & row("CASE_SIZE").ToString & vbTab & row("CASE_UOM").ToString)
            Next

            sw.Close()
            sw.Dispose()
        End Sub

        Private Sub FTPFile(ByVal sFileName As String)
            Dim ftp As FTPclient
            Dim fileInfo As FileInfo
            Dim sFTPAddress As String = ConfigurationServices.AppSettings("FTPHost")
            Dim sFTPUsername As String = ConfigurationServices.AppSettings("FTPUsername")
            Dim sFTPPassword As String = ConfigurationServices.AppSettings("FTPPassword")
            Dim sErrorEmailAddress As String = ConfigurationServices.AppSettings("ErrorEmailAddress")
            Dim sSuccessEmailAddress As String = ConfigurationServices.AppSettings("SuccessEmailAddress")
            Dim sRemoteFilePath As String = ConfigurationServices.AppSettings("RemoteFilePath")

            'ensure that the Remote File Path has a slash at the beginning and end
            If Mid(sRemoteFilePath, 1, 1) <> "/" Then sRemoteFilePath = "/" & sRemoteFilePath
            If Mid(sRemoteFilePath, Len(sRemoteFilePath), 1) <> "/" Then sRemoteFilePath = sRemoteFilePath & "/"

            If Len(sFileName) > 0 Then
                If File.Exists(sFileName) Then
                    fileInfo = New FileInfo(sFileName)
                    ftp = New FTPclient(sFTPAddress, sFTPUsername, sFTPPassword)
                    ftp.Upload(fileInfo, sRemoteFilePath & fileInfo.Name)
                End If
            End If
        End Sub

        Private Sub SendMail(ByVal sRecipient As String, ByVal sSubject As String, ByVal sMessage As String)
            Dim sSMTPHost As String = ConfigurationServices.AppSettings("SMTPHost")
            Dim sFromEmailAddress As String = ConfigurationServices.AppSettings("FromEmailAddress")
            Dim smtp As New SMTP(sSMTPHost)

            smtp.send(sMessage, sRecipient, Nothing, sFromEmailAddress, sSubject)
        End Sub

        Private Sub ArchiveFile(ByVal sFilePath As String, ByVal sArchivePath As String, ByVal sZipFilename As String, ByVal sOrigFileName As String)
            If File.Exists(sFilePath & sZipFilename) Then
                If Not Directory.Exists(sArchivePath) Then
                    Directory.CreateDirectory(sArchivePath)
                End If

                If File.Exists(sArchivePath & sZipFilename) Then File.Delete(sArchivePath & sZipFilename)
                File.Move(sFilePath & sZipFilename, sArchivePath & sZipFilename)
                File.Delete(sFilePath & sOrigFileName)
            End If
        End Sub

        Private Sub ZipHARTFile(ByVal sSourceFile As String, ByVal sDestFile As String)
            Using zip As ZipFile = New ZipFile()
                zip.AddFile(sSourceFile, "")
                zip.Save(sDestFile)
            End Using
        End Sub

        Private Sub DownloadFilesFromHART(ByVal sRegionCode As String)
            Dim ftp As FTPclient
            Dim lsFiles As List(Of String)
            Dim x As Integer = 0
            Dim sFTPAddress As String = ConfigurationServices.AppSettings("FTPHost")
            Dim sFTPUsername As String = ConfigurationServices.AppSettings("FTPUsername")
            Dim sFTPPassword As String = ConfigurationServices.AppSettings("FTPPassword")
            Dim sErrorEmailAddress As String = ConfigurationServices.AppSettings("ErrorEmailAddress")
            Dim sSuccessEmailAddress As String = ConfigurationServices.AppSettings("SuccessEmailAddress")
            Dim sRemoteFilePath As String = ConfigurationServices.AppSettings("RemoteFilePath")
            Dim sZipFileDownloadPath As String = ConfigurationServices.AppSettings("FileDownloadPath")
            'Dim sZipFileDownloadPath As String = "\\irmadevfile\Data\HART\SO\Import" 'For Debug

            ' ensure that the Remote File Path has a slash at the beginning and end
            If Mid(sRemoteFilePath, 1, 1) <> "/" Then sRemoteFilePath = "/" & sRemoteFilePath
            If Mid(sRemoteFilePath, Len(sRemoteFilePath), 1) <> "/" Then sRemoteFilePath = sRemoteFilePath & "/"

            ' ensure that the File Download Directory has a slash at the end
            If Mid(sZipFileDownloadPath, Len(sZipFileDownloadPath), 1) <> "\" Then sZipFileDownloadPath = sZipFileDownloadPath & "\"

            ftp = New FTPclient(sFTPAddress, sFTPUsername, sFTPPassword)
            lsFiles = ftp.ListDirectory(sRemoteFilePath)

            ' Download and Delete files that match the format of <Region Code>#######.zip (ex: SO12345A6.zip)
            For x = 0 To lsFiles.Count - 1
                If lsFiles.Item(x) <> "" Then
                    If Mid(lsFiles.Item(x), 1, 2) = sRegionCode And Mid(lsFiles.Item(x).ToString, Len(lsFiles.Item(x).ToString) - 3).ToLower = ".zip" Then
                        ftp.Download(sRemoteFilePath & lsFiles.Item(x), sZipFileDownloadPath & lsFiles.Item(x), True)
                        ftp.FtpDelete(sRemoteFilePath & lsFiles.Item(x))
                        UnZipHARTFile(sZipFileDownloadPath & lsFiles.Item(x))
                    End If
                End If
            Next

            ftp = Nothing
        End Sub

        Private Sub UnZipHARTFile(ByVal sSourceFile As String)
            Dim sExtractFilePath As String = ConfigurationServices.AppSettings("DatabaseLocalPath")
            Dim zipEntry As ZipEntry

            ' ensure that the File Download Directory has a slash at the end
            If Mid(sExtractFilePath, Len(sExtractFilePath), 1) <> "\" Then sExtractFilePath = sExtractFilePath & "\"

            Using zipFiles As ZipFile = ZipFile.Read(sSourceFile)
                For Each zipEntry In zipFiles
                    zipFiles.ExtractAll(sExtractFilePath, ExtractExistingFileAction.OverwriteSilently)
                Next
            End Using
        End Sub

        Private Sub DoStoreOpsExport()
            Dim HARTDAO As New HARTDAO
            Dim dt As New DataTable
            Dim sFilePath As String = ConfigurationServices.AppSettings("StoreOpsExportFileLocation").ToString
            Dim sFileName As String = ConfigurationServices.AppSettings("StoreOpsExportFilePrefix").ToString & Now.ToString("yyyyMMddHHmmss") & ".txt"

            'load values
            log.Info("Load the aggregate average cost values for the StoreOps Inventory export.")
            dt = HARTDAO.GetInventoryStoreOpsExport(Now.Date)

            'directory checker/creator
            If Not Directory.Exists(sFilePath) Then
                Directory.CreateDirectory(sFilePath)
            End If

            If dt.Rows.Count > 0 Then
                Dim sw As StreamWriter
                sw = New StreamWriter(sFilePath & sFileName)

                For Each row As DataRow In dt.Rows
                    sw.WriteLine(row("FP").ToString & "|" & row("FY").ToString & "|" & row("BU").ToString & "|" & row("subteam").ToString & "|" & row("AvgCost").ToString)
                Next

                sw.Close()
                sw.Dispose()
            End If
        End Sub

        Private Function FileReconciled(ByVal sFilename As String) As Boolean
            Dim result As Boolean = False
            Dim fileReader As StreamReader
            Dim headerRowCount As Integer
            Dim headerProductCount As Decimal
            Dim row As String
            Dim rowCount As Integer = 0
            Dim productCount As Decimal = 0
            Dim columnArray() As String

            If File.Exists(sFilename) Then
                fileReader = New StreamReader(sFilename)

                While Not fileReader.EndOfStream
                    row = fileReader.ReadLine
                    columnArray = row.Split(Chr(9))
                    rowCount = rowCount + 1

                    If rowCount = 1 And columnArray(0) = "HD" Then 'First row - Header record
                        headerRowCount = CInt(columnArray(1))
                        headerProductCount = CDec(columnArray(2))
                    ElseIf rowCount = 1 And columnArray(0) <> "HD" Then 'Header record missing
                        errMessage = "Missing header record."
                        Return result
                    Else 'Not first row 
                        productCount = productCount + CDec(columnArray(6))
                    End If

                End While

                fileReader.Close()

                If (headerRowCount = rowCount - 1) And (headerProductCount = productCount) Then 'rowCount - 1 to exclude the header row.
                    result = True
                ElseIf headerRowCount <> (rowCount - 1) Then
                    errMessage = "Header row count does not match file."
                ElseIf headerProductCount <> productCount Then
                    errMessage = errMessage & vbNewLine & "Header product count does not match file."
                End If
            Else
                errMessage = "File not found: " & sFilename
            End If


            Return result

        End Function

    End Class

End Namespace
