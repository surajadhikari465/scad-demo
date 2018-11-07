Imports System.IO
Imports System.Linq
Imports System.Net
Imports System.Net.Mail
Imports System.Text
Imports System.Web.Script.Serialization
Imports log4net
Imports WholeFoods.IRMA.InterfaceCommunication
Imports WholeFoods.IRMA.InterfaceCommunication.WebApiModel
Imports WholeFoods.IRMA.InterfaceCommunication.WebApiWrapper
Imports WholeFoods.IRMA.Planogram.DataAccess
Imports WholeFoods.IRMA.Pricing.BusinessLogic
Imports WholeFoods.Utility

Namespace WholeFoods.IRMA.Planogram.BusinessLogic

    Public Class PlanogramBO
        Private Const Target As Integer = 1
        Private Const MissingItems As Integer = 2
        Private Const Status As Integer = 3
        Private Const StoreNumberIndex As Integer = 3
        Private Const IdentifierIndex As Integer = 3
        Private Const SetNumberIndex As Integer = 6
        Private Const ExpectedNumberOfColumns As Integer = 7

        Private _localTargetFile As String
        Private _localMissingFile As String
        Private _copyTargetFile As String
        Private _DBServerSourceFile As String
        Private _missingFileName As String

        Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

        Public ReadOnly Property DBServerSourceFile() As String
            Get
                Return _DBServerSourceFile
            End Get
        End Property

        Public ReadOnly Property LocalTargetFile() As String
            Get
                Return _localTargetFile
            End Get
        End Property

        Public ReadOnly Property LocalMissingFile() As String
            Get
                Return _localMissingFile
            End Get
        End Property

        Public ReadOnly Property CopyTargetFile() As String
            Get
                Return _copyTargetFile
            End Get
        End Property

        Public ReadOnly Property MissingFileName() As String
            Get
                Return _missingFileName
            End Get
        End Property

        Private _storeCount As Integer
        Public ReadOnly Property StoreCount() As Integer
            Get
                Return _storeCount
            End Get
        End Property

        Private _headerCount As Integer
        Public ReadOnly Property HeaderCount() As Integer
            Get
                Return _headerCount
            End Get
        End Property

        Private _itemCount As Integer
        Public ReadOnly Property ItemCount() As Integer
            Get
                Return _itemCount
            End Get
        End Property

        Public Sub New()
            _missingFileName = "MISSING" + ConfigurationServices.AppSettings("PlanogramTargetFileName")

            _localTargetFile = String.Format("{0}{1}",
                ConfigurationServices.AppSettings("PlanogramLocalDir"),
                ConfigurationServices.AppSettings("PlanogramTargetFileName"))

            _copyTargetFile = String.Format("{0}{1}",
                ConfigurationServices.AppSettings("PlanogramTargetDir"),
                ConfigurationServices.AppSettings("PlanogramTargetFileName"))

            _DBServerSourceFile = String.Format("{0}{1}",
                ConfigurationServices.AppSettings("PlanogramDBServerSourceDir"),
                ConfigurationServices.AppSettings("PlanogramTargetFileName"))

            _localMissingFile = String.Format("{0}{1}",
                ConfigurationServices.AppSettings("PlanogramLocalDir"), _missingFileName)
        End Sub

        Public Sub ImportFile(ByVal selectedFile As String)
            Dim planogramItemBO As PlanogramItemBO = New PlanogramItemBO()
            Dim fileReader As System.IO.StreamReader = Nothing
            Dim stringReader As String
            Dim ItemDataset As DataSet = New DataSet
            Dim strSql As String = String.Empty
            Dim storeNo As Integer = 0
            Dim previousStoreNo As Integer = 0
            Dim storeCount As Integer = 0
            Dim itemCount As Integer = 0
            Dim missingCount As Integer = 0
            Dim duplicateCount As Integer = 0
            Dim missingFileName As String = String.Empty
            Dim targetDir As String
            Dim localDir As String
            Dim missingFileAttachment As String
            Dim statusFileAttachment As String
            Dim archiveDir As String
            Dim storeIdentifier As New Hashtable()

            Try
                ItemDataset = PlanogramDAO.GetItemDataset(ItemDataset, "tblItemDataset")
                targetDir = ConfigurationServices.AppSettings("PlanogramTargetDir")
                localDir = ConfigurationServices.AppSettings("PlanogramLocalDir")
                If Not (System.IO.Directory.Exists(targetDir)) Then System.IO.Directory.CreateDirectory(targetDir)
                If Not (System.IO.Directory.Exists(localDir)) Then System.IO.Directory.CreateDirectory(localDir)
                If Not (System.IO.Directory.Exists(targetDir & "Archive")) Then System.IO.Directory.CreateDirectory(targetDir + "\Archive")
                archiveDir = targetDir & "Archive\" & Now.ToString("yyyyMMdd") & "\"
                If Not (System.IO.Directory.Exists(archiveDir)) Then System.IO.Directory.CreateDirectory(archiveDir)

                missingFileName = Replace(Me.MissingFileName, ".dat", "") & "_" & Format(Now.ToString("hhmmss.bak"))
                missingFileAttachment = String.Format("{0}{1}", localDir, missingFileName)
                statusFileAttachment = String.Format("{0}{1}", localDir, "PlanogramStatus.txt")


                FileOpen(Target, Me.LocalTargetFile, OpenMode.Output)
                FileOpen(MissingItems, missingFileAttachment, OpenMode.Output)
                FileOpen(Status, statusFileAttachment, OpenMode.Output)

                fileReader = My.Computer.FileSystem.OpenTextFileReader(selectedFile)
                While Not fileReader.EndOfStream
                    stringReader = fileReader.ReadLine()
                    'skip blank lines in the file
                    If stringReader.Length = 0 Then Continue While
                    stringReader = stringReader.Replace("""", "")
                    Dim properties() As String = Split(stringReader, ",")

                    ' check for a header record
                    If properties(properties.Length - 1) = "HEADER" Then
                        ' this is a header record, pull off the store_no
                        storeNo = CInt(properties(3).Substring(properties(3).Length - 2, 2).Insert(0, ""))
                        'If storeNo = 214 Then storeNo = 2214 Check with team and uncomment later
                        storeCount = storeCount + 1

                        'if we have moved on to a different store, then we need to save the previous one
                        If storeNo <> previousStoreNo And previousStoreNo > 0 Then
                            If itemCount > 0 And itemCount - (duplicateCount + missingCount) > 0 Then
                                ' only do this process if we have items to process
                                'close target file only
                                FileClose(Target)
                                FileClose(MissingItems)
                                'copy to db server and insert into database
                                FileCopy(Me.LocalTargetFile, Me.CopyTargetFile)
                                PlanogramDAO.InsertPlanogramItems(previousStoreNo, Me.DBServerSourceFile)

                                ' construct the email message
                                PrintLine(Status, String.Format(ResourcesPlanogram.GetString("ImportResults"),
                                    vbCrLf, storeNo, itemCount, itemCount - (duplicateCount + missingCount), missingCount, duplicateCount))

                                ' delete missing file if empty
                                If missingCount = 0 Then
                                    My.Computer.FileSystem.DeleteFile(missingFileAttachment)
                                    missingFileAttachment = String.Empty
                                Else
                                    ' copy to the db server 
                                    FileCopy(missingFileAttachment, String.Format("{0}{1}", archiveDir, missingFileName))
                                    My.Computer.FileSystem.DeleteFile(missingFileAttachment)
                                End If

                                'reopen files for next store
                                FileOpen(Target, Me.LocalTargetFile, OpenMode.Output)
                                'reset counts for next store
                                itemCount = 0
                                missingCount = 0
                                duplicateCount = 0
                                ' add timestamp to filename
                                missingFileName = Replace(Me.MissingFileName, ".dat", "") & "_" & Format(Now.ToString("hhmmss.bak"))
                                missingFileAttachment = String.Format("{0}{1}", localDir, missingFileName)
                                FileOpen(MissingItems, missingFileAttachment, OpenMode.Output)

                            End If
                        End If
                        'update the previous store number
                        previousStoreNo = storeNo
                        ' force while to loop to read the next record
                        Continue While
                    End If  'End HEADER

                    '' check for an invalid row in the file, such as an end of file character
                    If properties.Length <> 7 Then Continue While

                    itemCount = itemCount + 1
                    'Group, Shelf, Placement, UPC Number, Max Units, Facings, Plan Number
                    ' iterate through the item and map to planogramItemBO properties
                    planogramItemBO.Group = properties(0)
                    planogramItemBO.Shelf = properties(1)
                    planogramItemBO.Placement = properties(2)
                    planogramItemBO.Identifier = properties(3)
                    planogramItemBO.MaxUnits = properties(4)
                    planogramItemBO.Facings = CInt(properties(5))
                    planogramItemBO.PlanNumber = properties(6)

                    ' planogram file is padding to 13, everything else pads to 12, so dropping left-most zero
                    strSql = "PaddedIdentifier = " + "'" + CStr(Trim(planogramItemBO.Identifier.Remove(0, 1))) + "'"
                    Dim foundRows As DataRow() = ItemDataset.Tables("tblItemDataset").Select(strSql)
                    If Not (foundRows.Length = 0) Then
                        Dim i As Integer
                        For i = 0 To foundRows.GetUpperBound(0)
                            planogramItemBO.ItemKey = CInt(foundRows(i)(0))
                            planogramItemBO.Identifier = CStr(foundRows(i)(1))
                        Next i

                        If Not (storeIdentifier.ContainsKey(String.Format("{0}{1}", storeNo, planogramItemBO.Identifier))) Then
                            storeIdentifier.Add(String.Format("{0}{1}", storeNo, planogramItemBO.Identifier), Nothing)
                        Else
                            duplicateCount = duplicateCount + 1
                        End If
                        ' write this planogramItem to the file for bulk insert later
                        ' DaveStacey - 20071003 - Removed this from the logic above related to not printing dupes
                        ' turns out that dupes are needed across planogram sets
                        PrintLine(Target, planogramItemBO.ToPlanogramFileString(storeNo))
                    Else
                        planogramItemBO.ItemKey = -1
                        missingCount = missingCount + 1
                        ' write this planogramItem to the file to record missing items
                        PrintLine(MissingItems, planogramItemBO.ToMissingPlanogramFileString(storeNo))
                    End If

                End While

                fileReader.Close()
                FileClose(Target)
                FileClose(MissingItems)

                If itemCount > 0 And itemCount - (duplicateCount + missingCount) > 0 Then
                    'copy to db server and insert into database
                    FileCopy(Me.LocalTargetFile, Me.CopyTargetFile)
                    PlanogramDAO.InsertPlanogramItems(storeNo, Me.DBServerSourceFile)
                End If

                ' construct the email message
                PrintLine(Status, String.Format(ResourcesPlanogram.GetString("ImportResults"),
                    vbCrLf, storeNo, itemCount, itemCount - (duplicateCount + missingCount), missingCount, duplicateCount))
                FileClose(Status)

                ' delete missing file if empty
                If missingCount = 0 Then
                    My.Computer.FileSystem.DeleteFile(missingFileAttachment)
                    missingFileAttachment = String.Empty
                Else
                    ' copy to the db server and add link to email
                    FileCopy(missingFileAttachment, String.Format("{0}{1}", archiveDir, missingFileName))
                    My.Computer.FileSystem.DeleteFile(missingFileAttachment)
                End If

                ' email the results
                EmailUploadConfirmation(statusFileAttachment, targetDir, missingCount > 0)

                ' copy to the db server 
                FileCopy(statusFileAttachment, String.Format("{0}{1}", archiveDir, "PlanogramStatus" & "_" & Format(Now.ToString("hhmmss.txt"))))
            Finally
                ' Let the CATCH bubble up - don't catch here, just clean up in the finally
                ItemDataset = Nothing
                If fileReader IsNot Nothing Then
                    fileReader.Close()
                End If

                fileReader = Nothing
                FileClose(Target)
            End Try
        End Sub

        Private Sub EmailUploadConfirmation(ByVal statusFileAttachment As String, ByVal targetDir As String, ByVal AnyMissing As Boolean)
            Dim emailFrom As String = ConfigurationServices.AppSettings("Planogram_FromEmailAddress")
            Dim emailTo As String = ConfigurationServices.AppSettings("Planogram_ToEmailAddress")
            Dim emailMessage As String
            Dim subject As String
            Dim sVersion As String
            Dim SendMailClient As SmtpClient
            Dim message As MailMessage
            Dim data As Attachment

            With My.Application.Info.Version
                sVersion = String.Format("{0}.{1}.{2}", .Major, .Minor, .Build)
            End With

            subject = ResourcesPlanogram.GetString("EmailSubject")

            emailMessage = String.Format(ResourcesPlanogram.GetString("EmailMessage"),
                gsUserName, vbCrLf, Now.ToString(ResourcesPlanogram.GetString("DateTime")), sVersion, emailTo, targetDir)

            Try
                SendMailClient = New SmtpClient(ConfigurationServices.AppSettings("EmailSMTPServer"))
                message = New MailMessage(emailFrom, emailTo, subject, emailMessage)
                If statusFileAttachment.Length > 0 Then
                    data = New Attachment(statusFileAttachment)
                    message.Attachments.Add(data)
                End If
                SendMailClient.Send(message)

            Catch ex As Exception
                ErrorHandler.ProcessError(ErrorType.GeneralApplicationError, SeverityLevel.Warning, ex)

                Dim warning As String = ResourcesPlanogram.GetString("WarningMessage")
                MsgBox(warning, CType(MsgBoxStyle.Information + MsgBoxStyle.OkOnly, MsgBoxStyle), ResourcesPlanogram.GetString("MessageTitle"))
            Finally
                SendMailClient = Nothing
                message = Nothing
                data = Nothing
            End Try
        End Sub

        Public Function ParseFile(selectedFile As String) As List(Of PlanogramItemBO)
            Dim planogramItems As List(Of PlanogramItemBO) = New List(Of PlanogramItemBO)
            Dim fileReader As System.IO.StreamReader = Nothing
            Dim textLine As String
            Dim storeNumber As Integer = 0
            Dim previousStoreNumber As Integer = 0
            Dim storeCount As Integer = 0
            Dim itemCount As Integer = 0
            Dim identifier As String
            Dim setNumber As String

            _headerCount = 0
            _storeCount = 0
            _itemCount = 0

            fileReader = My.Computer.FileSystem.OpenTextFileReader(selectedFile)

            While Not fileReader.EndOfStream
                textLine = fileReader.ReadLine()

                If textLine.Length = 0 Then Continue While

                textLine = textLine.Replace("""", "")

                Dim columns As String() = Split(textLine, ",")

                If columns.Length <> ExpectedNumberOfColumns Then
                    logger.Info(String.Format("Unexpected line length in planogram file.  Expected number of delimited values: {0}.  Actual number of values parsed: {1}.  File: {2}.  Text: {3}",
                                              ExpectedNumberOfColumns, columns.Length, selectedFile, textLine))
                    Continue While
                End If

                If columns(columns.Length - 1) = "HEADER" Then
                    _headerCount = _headerCount + 1

                    storeNumber = CInt(columns(StoreNumberIndex).Substring("STR#".Length))

                    If storeNumber <> previousStoreNumber Then
                        _storeCount = _storeCount + 1
                    End If

                    previousStoreNumber = storeNumber

                    Continue While
                Else
                    _itemCount = _itemCount + 1
                    identifier = columns(IdentifierIndex).TrimStart("0")
                    setNumber = columns(SetNumberIndex)
                    planogramItems.Add(New PlanogramItemBO With {.Identifier = identifier, .StoreNo = storeNumber, .PlanNumber = setNumber})
                End If
            End While

            Return planogramItems
        End Function

        Public Sub SendPlanogramPrintBatchRequests(
            items As List(Of PlanogramItemBO),
            setNumber As String,
            businessUnit As Integer,
            effectiveDate As Date)

            Dim slawPrintBatchModel = New SlawPrintBatchModel With
            {
                .Application = SlawConstants.PlanogramApplication,
                .BatchName = setNumber,
                .BusinessUnitId = businessUnit,
                .EffectiveDate = effectiveDate.Date,
                .IsAdHoc = False,
                .BatchEvent = SlawConstants.PlanogramEvent,
                .HasPriceChange = 0,
                .BatchChangeType = "ITM",
                .ItemCount = items.Count,
                .BatchItems = items.Select(Function(i) New SlawPrintBatchItemModel With {.Identifier = i.Identifier}).ToList()
            }

            Using slawWebApiWrapper As SlawWebApiWrapper = New SlawWebApiWrapper(String.Empty)
                slawWebApiWrapper.PostPrintHeader(slawPrintBatchModel)
            End Using
        End Sub
    End Class
End Namespace