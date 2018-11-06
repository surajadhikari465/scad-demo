Imports WholeFoods.IRMA.Replenishment.EST.DataAccess
Imports WholeFoods.Utility.FTP
Imports WholeFoods.Utility.SMTP
Imports WholeFoods.Utility
Imports System.Configuration
Imports System.IO
Imports log4net
Imports System.IO.Compression
Imports Ionic.Zip

Namespace WholeFoods.IRMA.Replenishment.EST.BusinessLogic
    Public Class ESTBO
        Private Shared CLASSTYPE As Type = System.Type.GetType("WholeFoods.IRMA.Replenishment.EST.BusinessLogic.ESTBO")
        Private log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

        Public Function DoESTImport() As String
            Dim ESTDAO As New ESTDAO
            Dim sRegionCode As String = ESTDAO.GetRegionCode
            Dim sFileDownloadPath As String = ConfigurationServices.AppSettings("FileDownloadPath")
            Dim sErrorEmailAddress As String = ConfigurationServices.AppSettings("ErrorEmailAddress").ToString
            Dim sSuccessEmailAddress As String = ConfigurationServices.AppSettings("SuccessEmailAddress").ToString
            Dim diServer As New DirectoryInfo(sFileDownloadPath)
            Dim aryFiles As FileInfo()
            Dim fi As FileInfo
            Dim strSuccessEmailText As String = ""

            Try
                ' ensure that the Local Path has a slash at the end
                If Mid(sFileDownloadPath, Len(sFileDownloadPath), 1) <> "\" Then sFileDownloadPath = sFileDownloadPath & "\"

                log.Info("Read all of the downloaded filenames")
                aryFiles = diServer.GetFiles()

                For Each fi In aryFiles
                    strSuccessEmailText = strSuccessEmailText + vbNewLine + fi.Name

                    Try
                        log.Info("Load the LoadESTFileUpdate table")
                        ESTDAO.LoadESTFileUpdate(sFileDownloadPath & fi.Name)
                    Catch fileLoadException As Exception

                        'Move the bad file to Error directory
                        log.Info("The EST Import Job failed for the following reason, bad file has been moved to Error directory: " & vbNewLine & vbNewLine & fileLoadException.Message & " - " & fileLoadException.InnerException.Message)
                        SendMail(sErrorEmailAddress, "EST Import Job", "The EST Import failed for the following reason, bad file has been moved to Error directory:  " & vbNewLine & vbNewLine & fileLoadException.Message & " - " & fileLoadException.InnerException.Message)
                        ArchiveFile(sFileDownloadPath & fi.Name, sFileDownloadPath & "Errors\" & fi.Name)

                        'Remove the bad file name from the success message
                        strSuccessEmailText = strSuccessEmailText.Remove((strSuccessEmailText.Length - fi.Name.Length - vbNewLine.Length))
                    End Try

                    'archive
                    log.Info("Finished processing file " & sFileDownloadPath & fi.Name & ".  File has been archived.")
                    ArchiveFile(sFileDownloadPath & fi.Name, sFileDownloadPath & "Archive\" & "\" & fi.Name)
                Next

                log.Info("The EST Import job has completed successfully")

                If UBound(aryFiles) <> -1 Then
                    SendMail(sSuccessEmailAddress, "EST Import Job", "The EST Import job has completed successfully and following file(s) has\have been imported: " + vbNewLine + strSuccessEmailText)
                End If

                Return "The EST Import Job has completed successfully."
            Catch ex As Exception
                log.Info("The EST Import Job failed for the following reason: " & ex.Message & " - " & ex.InnerException.Message)
                SendMail(sErrorEmailAddress, "EST Import Job", "The EST Import job did not complete due to the following error:  " & ex.Message & " - " & ex.InnerException.Message)
                Return "The EST Import failed for the following reason: " & ex.Message & " - " & ex.InnerException.Message
            End Try
        End Function

        Private Sub SendMail(ByVal sRecipient As String, ByVal sSubject As String, ByVal sMessage As String)
            Dim sSMTPHost As String = ConfigurationServices.AppSettings("SMTPHost")
            Dim sFromEmailAddress As String = ConfigurationServices.AppSettings("FromEmailAddress")
            Dim smtp As New SMTP(sSMTPHost)

            smtp.send(sMessage, sRecipient, Nothing, sFromEmailAddress, sSubject)
        End Sub

        Private Sub ArchiveFile(ByVal sFilePath As String, ByVal sArchivePath As String)
            If File.Exists(sArchivePath) Then File.Delete(sArchivePath)
            File.Copy(sFilePath, sArchivePath)
            File.Delete(sFilePath)
        End Sub
    End Class
End Namespace
