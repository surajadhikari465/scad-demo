Option Explicit On
Option Strict On

Imports System.IO
Imports System.Data.SqlClient
Imports System.Text
Imports WholeFoods.IRMA.Replenishment.Common.Writers
Imports WholeFoods.IRMA.Replenishment.Common.BusinessLogic
Imports WholeFoods.IRMA.Replenishment.POSPush.Controller
Imports WholeFoods.IRMA.Replenishment.POSPush.DataAccess
Imports WholeFoods.IRMA.Replenishment.POSPush.BusinessLogic
Imports WholeFoods.Utility

Imports WholeFoods.IRMA.Replenishment.Common.DataAccess

Imports System.Threading
Imports WeOnlyDo.Client.SSH
Imports WeOnlyDo.Security.Cryptography.KeyManager
Imports System.Windows.Forms

' NOTE: If this Namespace is updated, the StoreUpdatesBO constructor should also be updated.
Namespace WholeFoods.IRMA.Replenishment.POSPush.Writers

    Public Class IBM_Writer
        Inherits POSWriter

        ' POSFilename is the name of the file placed on the FTP server
        Protected _POSFilename As String = "EAMDD" 'append XXX to filename, which is the currentStore.BatchID
        Protected _isPOSFilenameDynamic As Boolean = True
        Protected _RemoteJobCommand As String = "EAMDMSIL.286"
        Protected _RemoteJobDirectory As String = "ADX_IPGM"

        Protected Overrides_ControlFileName As String = "EAMDMCTL.DAT"
        Protected Overrides_RemoteSSHStoreList As New StringBuilder

        Protected _outputFileFormat As FileFormat = FileFormat.Binary
        ' Complete listing of code page values: http://msdn2.microsoft.com/en-us/library/system.text.encoding.aspx
        ' 37    IBM EBCDIC (US-Canada) 
        ' 500   IBM EBCDIC (International)
        ' 1140  IBM EBCDIC (US-Canada-Euro)
        ' 1146  IBM EBCDIC (UK-Euro)
        ' 1148  IBM EBCDIC (International-Euro)
        ' 20285 IBM EBCDIC (UK)
        Protected Overrides_fileEncoding As Encoding = Encoding.Default

        Private Const SOURCE_FILE As Integer = 1    'source file number

#Region "Writer Constructors"

        Public Sub New(ByVal FileWriterKey As Integer)
            MyBase.New(FileWriterKey)

            Overrides_RemoteSSHStoreList.Append(String.Empty)
        End Sub

#End Region

#Region "Writer specific methods to add a single record to the POS Push file"

        ''' <summary>
        ''' This function allows writers to provide specific data formatting that is not handled by the 
        ''' writer configuration values.  This should only be used in rare instances.  In most cases, the 
        ''' wrtier configuration should be enhanced to capture this functionality.
        ''' 
        ''' Overridden to provide HEX ability, and MA specific rearranging of characters.  MA requires that a dynamic
        ''' data element (an integer value) be converted to HEX, and then rearranged to have the bytes reversed.
        ''' EX: value = 2001.  HEX = 07D1.  return value should be: D107
        ''' </summary>
        ''' <param name="dataContent"></param>
        ''' <param name="currentColumn"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Protected Overrides Function ApplyWriterFormatting(ByVal dataContent As String, ByRef currentColumn As POSDataElementBO) As String
            'perform specific action if column = "" ONLY
            If currentColumn.DataElement = "IBM_Offset42_Length2_MA" Then
                Dim newDataContent As New StringBuilder
                Dim hexDataContent As String

                '(1) convert value to HEX
                hexDataContent = Hex(dataContent)

                '(2) pad data with zeros until it is the appropriate length
                ' are we too short?
                If hexDataContent.Length < currentColumn.MaxFieldWidth Then
                    hexDataContent = PadData(hexDataContent, currentColumn)
                ElseIf hexDataContent.Length > currentColumn.MaxFieldWidth Then
                    ' we're too long
                    hexDataContent = TruncateData(hexDataContent, currentColumn)
                End If

                '(3) and flip order of data
                'NOTE: data will always be padded out to 4 characters, so the flip can always take the last 2
                'chars and put them before the first 2 chars
                'EX: value = 2001.  HEX = 07D1.  return value should be: D107
                newDataContent.Append(Right(hexDataContent, 2)) 'last 2 chars
                newDataContent.Append(Left(hexDataContent, 2)) 'first 2 chars

                dataContent = newDataContent.ToString
            End If

            Return dataContent
        End Function

#End Region

#Region "Common methods for additional business logic"
        ''' <summary>
        ''' Allow for the Writer to create an additional control file, separate from the POS or Scale Writer, that
        ''' can be sent to the store to kick off a job.
        ''' </summary>
        ''' <remarks>Create the control file that is sent to the IBM POS System.</remarks>
        Public Overrides Function CreateControlFile(ByRef currentStore As StoreUpdatesBO) As Boolean
            Logger.LogDebug("CreateControlFile entry", Me.GetType)

            Dim success As Boolean = True

            'currentStore.ControlFileName = Path.Combine(System.Configuration.ConfigurationServices.AppSettings("tempPOSFileDir"), currentStore.StoreNum.ToString) & "\" & Me.ControlFileName

            If RetrieveControlFile(currentStore) Then

                ' count the total number of records included in the file
                Dim recordCount As Integer = currentStore.GetTotalPOSRecordCount()

                'Tim (2007-12-18)
                ' - value greater than 65535 causes an error for Mid(sRecord, 38, 1) = Chr(recordCount \ 256)
                ' - this condition should only occur during a full item file push
                ' - it doesn't appear that this value is used for anything other than user information
                If recordCount > 65535 Then
                    recordCount = 65535
                End If

                ' Build the String for the control file
                '-- Insert Row Identifier
                Dim sRecord As New String(Chr(0), 48)  'NULL
                '-- DMCBID - Batch ID
                Mid(sRecord, 1, 3) = Pack(CStr(20000 + (currentStore.BatchID Mod 1000)), 3)
                '-- DMCHSN - Host Sequence Number
                Mid(sRecord, 4, 1) = Chr(&H0S)    'NULL
                '-- DMCREQ - Modify Data File
                Mid(sRecord, 5, 1) = Chr(&H40S)     '40 (HEX) => 64 (decimal) => bit 1 true (right to left)
                '-- DMCMT - Immediate
                Mid(sRecord, 6, 1) = Chr(&H11S)     '11 (HEX) => 17 (decimal) => bits 3 & 7 true (right to left)
                '-- DMCPO - Processing options
                Mid(sRecord, 7, 1) = Chr(&H10S)     '10 (HEX) => 16 (decimal) => bit 3 true (right to left)
                '-- DMCFN - Source file name
                Mid(sRecord, 13, 6) = "MD:" & (currentStore.BatchID Mod 1000).ToString("000")
                '-- DMCSFNA - Reserved (must be two ASCII periods, X'2E2E')
                Mid(sRecord, 19, 2) = ".."          ' . (Chr) => 46 (decimal) => 2E (HEX)
                '-- DMCTFN - Target file name
                Mid(sRecord, 21, 6) = "MITEMR"
                '-- DMCTFNA - Reserved (must be two ASCII periods, X'2E2E')
                Mid(sRecord, 27, 2) = ".."          ' . (Chr) => 46 (decimal) => 2E (HEX)
                '-- DMCRPN - Report file name
                Mid(sRecord, 29, 6) = Space(6)      ' space (Chr) => 32 (decimal) => 20 (HEX)
                '-- DMCRPNA - Reserved (must be two ASCII periods, X'2E2E')
                Mid(sRecord, 35, 2) = ".."          ' . (Chr) => 46 (decimal) => 2E (HEX)
                '-- DMCRCSF - Number of records in the source file
                Mid(sRecord, 37, 1) = Chr(recordCount Mod 256)      'integer remainder of division      =>  13 Mod 5 = 3
                Mid(sRecord, 38, 1) = Chr(recordCount \ 256)        'truncated integer from division    =>  13 \ 5 = 2
                '-- DMCERRCT - Number of errors logged for this batch
                Mid(sRecord, 39, 1) = Chr(&H0S)    'NULL
                Mid(sRecord, 40, 1) = Chr(&H0S)    'NULL
                '-- DMCERROF - Result of PTRRTN at first error message
                Mid(sRecord, 41, 4) = Space(4)
                '-- DMCESTAT - Execution status
                Mid(sRecord, 45, 1) = Chr(&H0S)   'NULL
                '-- DMCRSRVD - Reserved
                'Mid(sRecord, 46, 3) = New String(Chr(0), 3)    '3 NULL characters

                Try
                    Call InsertRecord(currentStore.ControlFileName, sRecord)
                Catch ex As Exception
                    Dim args(2) As String
                    Logger.LogError(String.Format("Unable to insert control record for store {0}", currentStore.StoreNum), Me.GetType)
                    args(0) = String.Format("Unable to insert control record for store {0}", currentStore.StoreNum)
                    args(1) = currentStore.FTPInfo.IPAddress
                    args(2) = currentStore.FTPInfo.FTPUser
                    ErrorHandler.ProcessError(ErrorType.FTPException, args, SeverityLevel.Fatal, ex)

                    Return False
                    Exit Function
                End Try

                ' Update the batch record count for this store
                StorePOSConfigDAO.UpdateBatchRecordCount(currentStore)
            Else
                MsgBox(String.Format("Unable to retrieve the Data Maintenance Control file for store {0}!", currentStore.StoreNum), CType(MsgBoxStyle.OkOnly + MsgBoxStyle.Critical, MsgBoxStyle), Me.GetType.ToString)

                success = False
            End If

            Logger.LogDebug("CreateControlFile exit", Me.GetType)

            Return success
        End Function
        
        ''' <summary>
        ''' calls a remote process on the IBM server after binary data has been transmitted
        ''' </summary>
        ''' <remarks></remarks>
        Public Overrides Function CallRemoteJobProcess(ByRef currentStore As StoreUpdatesBO) As Boolean

            Dim success As Boolean = True

            _currentStore = currentStore

            Overrides_RemoteSSHStoreList.Append(_currentStore.StoreNum.ToString)
            Overrides_RemoteSSHStoreList.Append(" = ")

            Dim ftpUtil As FTP.FTPclient
            Dim sCommand As String = String.Empty
            Dim sResponse As String = String.Empty

            With currentStore.FTPInfo
                ftpUtil = New FTP.FTPclient(.IPAddress, .FTPUser, .FTPPassword, .IsSecureTransfer, CType(.Port, Integer))
            End With

            Try
                'remotely issue command to begin processing the batch control file
                sCommand = "ADXSTART " + _RemoteJobDirectory + ":" + _RemoteJobCommand
                sResponse = ftpUtil.IssueCommand(sCommand)
                Logger.LogDebug(String.Format("Store {0} - ftpUtil.IssueCommand(""{1}"") response: '{2}'", currentStore.StoreNum, sCommand, sResponse), Me.GetType)

            Catch exFTP As FTP.FTPException
                Dim args(2) As String
                args(0) = currentStore.StoreNum.ToString
                args(1) = currentStore.FTPInfo.IPAddress
                args(2) = currentStore.FTPInfo.FTPUser
                ErrorHandler.ProcessError(ErrorType.FTPException, args, SeverityLevel.Warning, exFTP)
                Logger.LogError(String.Format("Store {0} - ftpUtil.IssueCommand(""{1}"") FTPException: '{2}'", currentStore.StoreNum, sCommand, exFTP.Message), Me.GetType)

                success = False

            Catch ex As Exception
                Dim args(2) As String
                args(0) = String.Format("ftpUtil.IssueCommand(""{0}"")", sCommand)
                args(1) = currentStore.StoreNum.ToString
                args(2) = sResponse
                ErrorHandler.ProcessError(ErrorType.POSPush_RunRemoteProcess, args, SeverityLevel.Warning, ex)
                Logger.LogError(String.Format("Store {0} - ftpUtil.IssueCommand(""{1}"") Exception: '{2}'", currentStore.StoreNum, sCommand, ex.Message), Me.GetType)

                success = False

            Finally
                If (success = True) Then
                    Overrides_RemoteSSHStoreList.Append("**Passed** ")
                Else
                    Overrides_RemoteSSHStoreList.Append("**FAILED** ")
                End If
                ftpUtil = Nothing
            End Try

            Return success
        End Function

        ''' <summary>
        ''' Write the contents of a data line to the file.  If a writer includes specific information before or 
        ''' after a line of data that is not covered by the common configuration, it should override this method.
        ''' </summary>
        ''' <param name="line"></param>
        ''' <remarks></remarks>
        Protected Overrides Sub WriteDataLine(ByVal line As String)
            WriteLine(line, True, False)
        End Sub

        ''' <summary>
        ''' Allow for the Writer to create an additional control file, separate from the POS or Scale Writer, that
        ''' can be sent to the store to kick off a job.
        ''' </summary>
        ''' <remarks>Create the control file that is sent to the IBM POS System.</remarks>
        Private Function RetrieveControlFile(ByRef currentStore As StoreUpdatesBO) As Boolean
            Logger.LogDebug("RetrieveControlFile entry", Me.GetType)

            Dim ftpClient As FTP.FTPclient = Nothing
            Dim sftpClient As wodSFTP.SFTPClient = Nothing

            Dim storeFTP As StoreFTPConfigBO = currentStore.FTPInfo
            Dim lErrors As Integer
            Dim sRemoteFile As String

            'build control file location - add directory separator if one is not already configured
            If storeFTP.ChangeDirectory.Length = 0 Then
                sRemoteFile = "/" & Me.ControlFileName
            ElseIf storeFTP.ChangeDirectory.Substring(storeFTP.ChangeDirectory.Length - 1).Equals("/") Then
                sRemoteFile = storeFTP.ChangeDirectory & Me.ControlFileName
            Else
                sRemoteFile = storeFTP.ChangeDirectory & "/" & Me.ControlFileName
            End If

            Dim sLocalFile As String = currentStore.ControlFileName     'includes full path
            Dim args(0) As String

            Try
                Try
                    If File.Exists(sLocalFile) Then
                        File.Delete(sLocalFile)
                    End If
                Catch ex As Exception
                    'keep going
                End Try

                '-- Get the Data Maintenance Control File
                Try
                    'create an FTP client for this store with its FTP config settings and download the file from each store 
                    If (storeFTP.IsSecureTransfer) Then
                        sftpClient = New wodSFTP.SFTPClient(storeFTP.IPAddress, storeFTP.FTPUser, storeFTP.FTPPassword)
                        sftpClient.Download(sRemoteFile, sLocalFile, True)
                    Else
                        ftpClient = New FTP.FTPclient(storeFTP.IPAddress, storeFTP.FTPUser, storeFTP.FTPPassword, storeFTP.IsSecureTransfer)
                        ftpClient.Download(sRemoteFile, sLocalFile, True)
                    End If

                Catch ex As Exception
                    Logger.LogError("ERROR: Could not connect to remote computer", Me.GetType, ex)
                    Logger.LogError(String.Format("Error retrieving POS control file from store {0}: " & ex.ToString, currentStore.StoreNum), Me.GetType)
                    args(0) = String.Format("Error retrieving POS control file from store {0}.", currentStore.StoreNum)
                    ErrorHandler.ProcessError(ErrorType.POSPush_AdminError, args, SeverityLevel.Fatal, ex)
                    Exit Function
                Finally
                    sftpClient = Nothing
                    ftpClient = Nothing
                End Try

                '-- Make sure we actually got a file back
                If Not File.Exists(sLocalFile) Then
                    args(0) = String.Format("Error retrieving POS control file from store {0}, but internet control did not error.", currentStore.StoreNum)
                    Logger.LogError(args(0), Me.GetType)
                    ErrorHandler.ProcessError(ErrorType.POSPush_AdminError, args, SeverityLevel.Fatal)
                    Exit Try
                End If

                If Not RemoveCompletedBatches(sLocalFile, lErrors) Then
                    args(0) = String.Format("Unable to insert data maintenance record; unable to clear completed control records for store {0}.", currentStore.StoreNum)
                    Logger.LogError(args(0), Me.GetType)
                    ErrorHandler.ProcessError(ErrorType.POSPush_AdminError, args, SeverityLevel.Fatal)
                    Exit Try
                End If

                'If mbVerifyControlOnly Then
                '    If lErrors > 0 Then LogError("Incomplete or erroneous batch count = " & CStr(lErrors) & " for store: " & glStoreNo)
                'Else
                If lErrors > 10 Then
                    args(0) = String.Format("More than ten errors or incomplete batches in DDM for store {0}.", currentStore.StoreNum)
                    Logger.LogError(args(0), Me.GetType)
                    ErrorHandler.ProcessError(ErrorType.POSPush_AdminError, args, SeverityLevel.Fatal)
                    Exit Try
                End If

                RetrieveControlFile = True

            Catch ex As Exception

            End Try

            Logger.LogDebug("RetrieveControlFile exit", Me.GetType)
        End Function

        Public Function RemoveCompletedBatches(ByVal sFile As String, ByRef lErrors As Integer) As Boolean

            Dim x As Integer
            Dim y As Integer
            Dim z As Integer

            Dim sRecord As New Microsoft.VisualBasic.Compatibility.VB6.FixedLengthString(512)
            'try sRecords declared at the top of the class with an 'attribute' of <VBFixedString(512)>
            Dim lBlocks As Integer
            Dim lRecordSize As Integer
            Dim lDivisor As Integer
            Dim lKeyLength As Integer
            Dim sTempRecord As String
            Dim sRecords() As String
            Dim args(0) As String

            Try
                ReDim sRecords(0)

                FileOpen(SOURCE_FILE, sFile, OpenMode.Random, , , 512)
                'fileStream = File.Open(sFile, FileMode.Open, FileAccess.ReadWrite, FileShare.None)

                FileGet(SOURCE_FILE, sRecord.Value, 1, True)

                lBlocks = Asc(Mid(sRecord.Value, 43, 1)) + (Asc(Mid(sRecord.Value, 44, 1)) * 256) + (Asc(Mid(sRecord.Value, 45, 1)) * 256 * 256) + (Asc(Mid(sRecord.Value, 46, 1)) * 256 * 256 * 256)
                lRecordSize = Asc(Mid(sRecord.Value, 47, 1)) + (Asc(Mid(sRecord.Value, 48, 1)) * 256)
                lDivisor = Asc(Mid(sRecord.Value, 49, 1)) + (Asc(Mid(sRecord.Value, 50, 1)) * 256) + (Asc(Mid(sRecord.Value, 51, 1)) * 256 * 256) + (Asc(Mid(sRecord.Value, 52, 1)) * 256 * 256 * 256)
                lKeyLength = Asc(Mid(sRecord.Value, 55, 1)) + (Asc(Mid(sRecord.Value, 56, 1)) * 256)

                y = 508 \ lRecordSize

                For x = 2 To lBlocks
                    FileGet(SOURCE_FILE, sRecord.Value, x)

                    For z = 1 To y

                        sTempRecord = Mid(sRecord.Value, 5 + (lRecordSize * (z - 1)), lRecordSize)
                        If Asc(Mid(sTempRecord, 45, 1)) <> 32 AndAlso CDbl(PDValue(Mid(sTempRecord, 1, 3))) <> 0 Then
                            ReDim Preserve sRecords(UBound(sRecords) + 1)
                            sRecords(UBound(sRecords)) = sTempRecord
                        End If

                    Next z
                Next x

                sRecord.Value = New String(Chr(0), 512)

                For x = 2 To lBlocks
                    FilePut(SOURCE_FILE, sRecord.Value, x, True)
                Next x

                FileClose(SOURCE_FILE)

                For x = 1 To UBound(sRecords)
                    If Not InsertRecord(sFile, sRecords(x)) Then
                        RemoveCompletedBatches = False
                        Exit Function
                    End If
                Next x

                lErrors = UBound(sRecords)

                RemoveCompletedBatches = True

            Catch ex As Exception
                args(0) = String.Format("Replenishment: RemoveCompletedBatches failed due to a system error: {0}.", ex.Message)
                Logger.LogError(args(0), Me.GetType)
                ErrorHandler.ProcessError(ErrorType.POSPush_AdminError, args, SeverityLevel.Fatal)

            Finally
                ' Let the CATCH bubble up - don't catch here, just clean up in the finally
                Erase sRecords
                'FileClose(SOURCE_FILE)

            End Try

        End Function

#End Region

#Region "Private Helper Methods"

        Private Function PDValue(ByVal sString As String) As String

            Dim sTemp As String = String.Empty
            Dim l As Integer

            For l = 1 To Len(sString)
                sTemp = sTemp & Right("0" & Hex(Asc(Mid(sString, l, 1))), 2)
            Next l

            Select Case UCase(Left(sTemp, 1))
                Case "D", "F"
                    sTemp = Mid(sTemp, 2)
                Case "A"
                    Mid(sTemp, 1, 1) = "-"
            End Select

            If Len(sTemp) = 0 Then
                PDValue = CStr(0)   'NULL
            Else
                If UCase(Left(sTemp, 1)) = "D" Then
                    sTemp = Mid(sTemp, 2)
                End If
                PDValue = CStr(CDec(sTemp))
            End If

        End Function

        Private Function PDString(ByVal sString As String) As String

            Dim sTemp As String = String.Empty
            Dim l As Integer

            For l = 1 To Len(sString)
                sTemp = sTemp & Right("0" & Hex(Asc(Mid(sString, l, 1))), 2)
            Next l

            Select Case UCase(Left(sTemp, 1))
                Case "F"
                    sTemp = Mid(sTemp, 2)
                Case "A"
                    Mid(sTemp, 1, 1) = "-"
            End Select

            If Len(sTemp) = 0 Then
                PDString = CStr(0)  'NULL
            Else
                PDString = sTemp
            End If

        End Function

        Private Function InsertRecord(ByRef sFile As String, ByRef sNewRecord As String) As Boolean

            'Dim x As Integer
            Dim y As Integer
            Dim z As Integer

            Dim sKey As String
            Dim lKey As Integer
            Dim lRecord As Integer

            Dim sRecord As New Microsoft.VisualBasic.Compatibility.VB6.FixedLengthString(512)
            Dim lBlocks As Integer
            Dim lRecordSize As Integer
            Dim lDivisor As Integer
            Dim lKeyLength As Integer
            Dim lHashingMethod As Integer

            Dim lTempKey As Integer
            Dim sTempKey As String
            Dim lTempRecord As Integer
            Dim sTempRecord As String
            Dim lHomeRecord As Integer

            Dim lPreviousRecord As Integer
            Dim lForwardRecord As Integer

            Dim bFound As Boolean

            Try
                FileOpen(SOURCE_FILE, sFile, OpenMode.Random, , , 512)

                FileGet(SOURCE_FILE, sRecord.Value, 1, True)

                lBlocks = Asc(Mid(sRecord.Value, 43, 1)) + (Asc(Mid(sRecord.Value, 44, 1)) * 256) + (Asc(Mid(sRecord.Value, 45, 1)) * 256 * 256) + (Asc(Mid(sRecord.Value, 46, 1)) * 256 * 256 * 256)
                lRecordSize = Asc(Mid(sRecord.Value, 47, 1)) + (Asc(Mid(sRecord.Value, 48, 1)) * 256)
                lDivisor = Asc(Mid(sRecord.Value, 49, 1)) + (Asc(Mid(sRecord.Value, 50, 1)) * 256) + (Asc(Mid(sRecord.Value, 51, 1)) * 256 * 256) + (Asc(Mid(sRecord.Value, 52, 1)) * 256 * 256 * 256)
                lKeyLength = Asc(Mid(sRecord.Value, 55, 1)) + (Asc(Mid(sRecord.Value, 56, 1)) * 256)
                lHashingMethod = Asc(Mid(sRecord.Value, 180, 1))

                y = 508 \ lRecordSize

                sKey = Mid(sNewRecord, 1, lKeyLength)
                lKey = GenerateKey(sKey, lHashingMethod)
                '-- Get the home block - this is the block where the hashing algorithm dictates this record should go
                ' See IBM POS Programming Guide p. 206-208 for a detailed explanation and examples that make this a lot clearer.
                lRecord = lKey Mod lDivisor
                If lRecord = 0 Then
                    lRecord = lDivisor
                End If
                lHomeRecord = lRecord

                While lKey <> -1

                    'Get the block that the insert record should go in based on the hashing algorithm
                    FileGet(SOURCE_FILE, sRecord.Value, lRecord + 1, True)
                    bFound = False

                    lForwardRecord = Asc(Mid(sRecord.Value, 1, 1)) + (Asc(Mid(sRecord.Value, 2, 1)) * 256)
                    lPreviousRecord = Asc(Mid(sRecord.Value, 3, 1)) + (Asc(Mid(sRecord.Value, 4, 1)) * 256)

                    For z = 1 To y
                        '-- Find out if its its an home, overflow or empty record
                        '
                        ' Calculate the block the current record should be in using the hashing algorithm.  This is done to determine
                        ' if this record is an overflow record from another block or if this is a home record.
                        '
                        sTempKey = Mid(sRecord.Value, 5 + (lRecordSize * (z - 1)), lKeyLength)
                        lTempKey = GenerateKey(sTempKey, lHashingMethod)
                        lTempRecord = lTempKey Mod lDivisor
                        If lTempRecord = 0 Then
                            lTempRecord = lDivisor 'This was "If lTempRecord = 0 Then lRecord = lDivisor" - changed on 11/19/02 by D. Lowe
                        End If

                        ' If this record is empty or if it is the record to be inserted, use it
                        If Val(PDValue(sTempKey)) = 0 OrElse Val(PDValue(sTempKey)) = Val(PDValue(Mid(sNewRecord, 1, lKeyLength))) Then
                            '-- ok to insert
                            Mid(sRecord.Value, 5 + (lRecordSize * (z - 1)), lRecordSize) = sNewRecord
                            sNewRecord = New String(Chr(0), lRecordSize)
                            FilePut(SOURCE_FILE, sRecord.Value, lRecord + 1, True)
                            bFound = True
                            Exit For
                            '
                            ' lTempRecord is the block where the current record belongs (its home block) and lRecord is the block we are in currently
                            ' If the current record is an overflow and it is part of another chain and this is the home block for the record being inserted
                            ' then this overflow record should be expelled - that is moved to another block - and replace by the record to be inserted
                        ElseIf (lTempRecord <> lRecord) AndAlso (lTempRecord <> lHomeRecord) AndAlso (lRecord = lHomeRecord) Then
                            '
                            ' If this is the last record in the block then the overflow record gets expelled and this block will now
                            ' be full of home records, so the overflow block pointers have to be fixed
                            If z = y Then
                                '-- This is the last record; must move the overload elsewhere and unlink record from previous overflow chian
                                sTempRecord = Mid(sRecord.Value, 5 + (lRecordSize * (z - 1)), lRecordSize)
                                Mid(sRecord.Value, 5 + (lRecordSize * (z - 1)), lRecordSize) = sNewRecord
                                ' The overflow record gets moved to sNewRecord so that when the we get to the end of the While loop
                                ' this overflow record will get processed and added back into the correct block
                                sNewRecord = sTempRecord
                                '-- Get rid of previous reference because there are no more overflows in this record
                                Mid(sRecord.Value, 3, 2) = New String(Chr(0), 2)
                                FilePut(SOURCE_FILE, sRecord.Value, lRecord + 1, True)
                                '-- Clear the previous record's forward record so it no longer points to this record
                                If lPreviousRecord > 0 Then
                                    FileGet(SOURCE_FILE, sRecord.Value, lPreviousRecord + 1, True)
                                    Mid(sRecord.Value, 1, 2) = New String(Chr(0), 2)
                                    FilePut(SOURCE_FILE, sRecord.Value, lPreviousRecord + 1, True)
                                End If
                                bFound = True
                                Exit For
                                '
                                ' Else this is not the last record so we can use the overflow record spot and move the overflow record
                                ' to another block without handling block overflow pointers.  If this block has overflow pointers and the is actually
                                ' the last record, then the overflow record we are going to replace will be put back here after the one we are
                                ' inserting now.  Even if this block ends up with no overflow records but still is part of the overflow chain, it does
                                ' not matter.
                            Else
                                '-- Remove row, add elsewhere
                                sTempRecord = Mid(sRecord.Value, 5 + (lRecordSize * (z - 1)), lRecordSize)
                                Mid(sRecord.Value, 5 + (lRecordSize * (z - 1)), lRecordSize) = sNewRecord
                                ' The overflow record gets moved to sNewRecord so that when the we get to the end of the While loop
                                ' this overflow record will get processed and added back into the correct block
                                sNewRecord = sTempRecord

                                'Get the key on the next record and see whether it is blank (it would be another overflow record if not)
                                sTempKey = Mid(sRecord.Value, 5 + (lRecordSize * z), lKeyLength)
                                'If the next record is blank then there are no more overflow recs in this block, so clear the backward pointer
                                If (lPreviousRecord > 0) AndAlso (Val(PDValue(sTempKey)) = 0) Then
                                    Mid(sRecord.Value, 3, 2) = New String(Chr(0), 2)
                                    FilePut(SOURCE_FILE, sRecord.Value, lRecord + 1, True)
                                    'Get the backward record and clear its forward pointer
                                    FileGet(SOURCE_FILE, sRecord.Value, lPreviousRecord + 1, True)
                                    Mid(sRecord.Value, 1, 2) = New String(Chr(0), 2)
                                    FilePut(SOURCE_FILE, sRecord.Value, lPreviousRecord + 1, True)
                                Else
                                    FilePut(SOURCE_FILE, sRecord.Value, lRecord + 1, True)
                                End If
                                ' The overflow record gets moved to sNewRecord so that when the we get to the end of the While loop
                                ' this overflow record will get processed and added back into the correct block
                                bFound = True
                                Exit For
                            End If
                        End If
                    Next z

                    ' If there was no place to put the record in the block it belongs in
                    If Not bFound Then
                        ' If this block already overflows, then follow the overflow chain to the next block and try it
                        If lForwardRecord <> 0 Then
                            '-- Go to the next record
                            lRecord = lForwardRecord
                        Else ' This block does not overflow yet and needs to
                            lTempRecord = lRecord
                            While Not bFound
                                '-- Look forward by three blocks to see if we can use it as an overflow block
                                lTempRecord = (lTempRecord + 3) Mod lDivisor
                                If lTempRecord = 0 Then lTempRecord = lDivisor

                                '-- Quit if nothing was found open - Prevent looping
                                If lTempRecord = lRecord Then
                                    FileClose(SOURCE_FILE)
                                    InsertRecord = False
                                    Exit Function
                                End If

                                FileGet(SOURCE_FILE, sRecord.Value, lTempRecord + 1, True)

                                '-- Make sure its not already an overflow
                                If Mid(sRecord.Value, 1, 4) = New String(Chr(0), 4) Then
                                    For z = 1 To y
                                        '-- Have to scan for the next record to add to the chain - find a record that is empty
                                        sTempKey = Mid(sRecord.Value, 5 + (lRecordSize * (z - 1)), lKeyLength)
                                        If Val(PDValue(sTempKey)) = 0 Then
                                            Mid(sRecord.Value, 5 + (lRecordSize * (z - 1)), lRecordSize) = sNewRecord
                                            sNewRecord = New String(Chr(0), lRecordSize)
                                            '-- Point to block we overflowed from
                                            Mid(sRecord.Value, 3, 1) = Chr(lRecord Mod 256)
                                            Mid(sRecord.Value, 4, 1) = Chr(lRecord \ 256)
                                            FilePut(SOURCE_FILE, sRecord.Value, lTempRecord + 1, True)
                                            '-- Get the previous block and update forward pointer to point to this overflow block
                                            FileGet(SOURCE_FILE, sRecord.Value, lRecord + 1, True)
                                            Mid(sRecord.Value, 1, 1) = Chr(lTempRecord Mod 256)
                                            Mid(sRecord.Value, 2, 1) = Chr(lTempRecord \ 256)
                                            FilePut(SOURCE_FILE, sRecord.Value, lRecord + 1, True)
                                            '-- Mark as found
                                            bFound = True
                                            lKey = -1
                                            Exit For
                                        End If
                                    Next z
                                End If
                            End While

                        End If
                    Else
                        ' If sNewRecord has been initialized to zeros, then the value of key will be 0, meaning we are done -
                        ' no more overflow records to move
                        If CDbl(PDValue(Mid(sNewRecord, 1, lKeyLength))) = 0 Then
                            lKey = -1
                        Else
                            ' Otherwise, calculate the block where the overflow record belongs and set it as the home block
                            ' and loop back around to process it
                            sKey = Mid(sNewRecord, 1, lKeyLength)
                            lKey = GenerateKey(sKey, lHashingMethod)
                            lRecord = lKey Mod lDivisor
                            If lRecord = 0 Then
                                lRecord = lDivisor
                            End If
                            lHomeRecord = lRecord
                        End If
                    End If

                End While

                InsertRecord = True
            Catch ex As Exception
                Dim args(0) As String
                args(0) = String.Format("WholeFoods.IRMA.Replenishment\POSPush\Writers\IBM_Writer.InsertRecord() failed due to a system error: {0}.", ex.Message)
                Logger.LogError(args(0), Me.GetType)
                ErrorHandler.ProcessError(ErrorType.POSPush_AdminError, args, SeverityLevel.Fatal, ex)

            Finally
                FileClose(SOURCE_FILE)

            End Try

        End Function

        Private Function GenerateKey(ByRef sKey As String, ByRef lMethod As Integer) As Integer

            Select Case lMethod
                Case 0 : GenerateKey = KeyFolding(sKey)
                Case 1 : GenerateKey = XorRotation(sKey)
                Case 2 : GenerateKey = CalculateCRC(sKey)
            End Select

        End Function

        Private Function KeyFolding(ByRef sKey As String) As Integer

            Dim l As Integer
            Dim LSB, MSB As Byte
            Dim bLow As Boolean

            bLow = True
            For l = Len(sKey) To 1 Step -1
                If bLow Then
                    LSB = LSB Xor CByte(Asc(Mid(sKey, l, 1)))
                Else
                    MSB = MSB Xor CByte(Asc(Mid(sKey, l, 1)))
                End If
                bLow = Not bLow
            Next l

            KeyFolding = (MSB * 256) + LSB

        End Function

        Private Function XorRotation(ByRef sKey As String) As Integer

            Dim l As Integer
            Dim LSB, MSB As Byte
            Dim RotatedByte As Integer
            Dim bLow As Boolean
            Dim shift As Integer

            bLow = True
            For l = Len(sKey) To 1 Step -1
                If bLow Then
                    LSB = LSB Xor CType(Asc(Mid(sKey, l, 1)), Byte)
                Else
                    MSB = MSB Xor CType(Asc(Mid(sKey, l, 1)), Byte)
                End If
                bLow = Not bLow
            Next l

            shift = (CInt(ReverseString(PDValue(sKey))) Mod 31) \ 2
            RotatedByte = (MSB * 256) + LSB
            RotatedByte = (CInt(RotatedByte * (2 ^ shift)) Or (RotatedByte \ CInt((2 ^ (16 - shift))))) And &HFFFF

            '-- Concatenate the bytes
            RotatedByte = (RotatedByte * 256 * 256) + (MSB * 256) + LSB

            '-- Rotate all 32 bits
            RotatedByte = (CInt(CShort(RotatedByte And &HFFFFFFF) * 2 ^ shift) Or (RotatedByte \ CInt((2 ^ (32 - shift))))) And &H7FFFFFFF

            XorRotation = RotatedByte

        End Function

        Private Function shl(ByRef w As Byte, ByRef c As Byte) As Byte
            Return CType((w * (2 ^ c)) Mod 256, Byte)
        End Function

        Private Function shr(ByRef w As Byte, ByRef c As Byte) As Byte
            Return CType((w \ CType((2 ^ c), Long)) Mod 256, Byte)
        End Function

        Private Function CalculateCRC(ByRef psString As String) As Integer

            Dim crcByte1, crcByte2 As Byte
            Dim crcByte3, crcByte4 As Byte
            Dim r1 As Byte
            Dim l As Integer

            crcByte1 = &HFFS
            crcByte2 = &HFFS

            For l = 1 To Len(psString)
                r1 = CByte(Asc(Mid(psString, l, 1))) Xor crcByte2
                r1 = shl(r1, 4) Xor r1

                crcByte2 = shl(r1, 4) Or shr(r1, 4)
                crcByte2 = CByte(crcByte2 And &HFS) Xor crcByte1
                crcByte1 = r1
                r1 = shl(r1, 3) Or shr(r1, 5)
                crcByte2 = crcByte2 Xor CByte(r1 And &HF8S)
                crcByte1 = crcByte1 Xor CByte(r1 And &H7S)

            Next l

            crcByte2 = Byte.MaxValue - crcByte2
            crcByte1 = Byte.MaxValue - crcByte1

            crcByte3 = &HFFS
            crcByte4 = &HFFS

            For l = Len(psString) To 1 Step -1
                r1 = CByte(Asc(Mid(psString, l, 1))) Xor crcByte4
                r1 = shl(r1, 4) Xor r1

                crcByte4 = shl(r1, 4) Or shr(r1, 4)
                crcByte4 = CByte(crcByte4 And &HFS) Xor crcByte3
                crcByte3 = r1
                r1 = shl(r1, 3) Or shr(r1, 5)
                crcByte4 = crcByte4 Xor CByte(r1 And &HF8S)
                crcByte3 = crcByte3 Xor CByte(r1 And &H7S)

            Next l

            crcByte4 = Byte.MaxValue - crcByte4
            crcByte3 = Byte.MaxValue - crcByte3

            CalculateCRC = (CShort(crcByte3 And &H7FS) * 256 * 256 * 256) + (crcByte4 * 256 * 256) + (crcByte1 * 256) + crcByte2

        End Function

        Private Function ReverseString(ByVal sString As String) As String

            Dim sTemp As String = String.Empty
            Dim l As Integer

            For l = Len(sString) To 1 Step -1
                sTemp = sTemp & Mid(sString, l, 1)
            Next l

            Return sTemp

        End Function

#End Region

#Region "Property Definitions"
        Overrides Property WriterFilename(ByVal currentStore As StoreUpdatesBO) As String
            Get
                Dim filename As New StringBuilder
                filename.Append(_POSFilename)
                filename.Append((currentStore.BatchID Mod 1000).ToString("000"))

                Return filename.ToString
            End Get
            Set(ByVal value As String)
                _POSFilename = value
            End Set
        End Property

        Overrides ReadOnly Property RemoteSSHStoreList() As String
            Get
                Return Overrides_RemoteSSHStoreList.ToString
            End Get
        End Property

        Overrides Property ControlFileName() As String
            Get
                Return Overrides_ControlFileName
            End Get
            Set(ByVal value As String)
                Overrides_ControlFileName = value
            End Set
        End Property

        Public Overrides Property OutputFileFormat() As FileFormat
            Get
                Return _outputFileFormat
            End Get
            Set(ByVal value As FileFormat)
                _outputFileFormat = value
            End Set
        End Property

        Overrides Property FileEncoding() As Encoding
            Get
                Return Overrides_fileEncoding
            End Get
            Set(ByVal value As Encoding)
                Overrides_fileEncoding = value
            End Set
        End Property

        Overrides ReadOnly Property SupportsChangeType(ByVal chgType As ChangeType) As Boolean
            Get
                'IBM writer supports all change types except Vendor ID Adds
                Select Case chgType
                    Case ChangeType.VendorIDAdd
                        Return False
                    Case Else
                        Return True
                End Select
            End Get
        End Property
#End Region

        Protected WithEvents Ssh As WeOnlyDo.Client.SSH

        Dim lcount As Integer = 0
        Private _currentStore As StoreUpdatesBO

        Dim bCompleted As Boolean = False
        Dim bFailed As Boolean = False
        Dim bTimedOut As Boolean = False

        Private Sub Ssh_ConnectedEvent(ByVal Sender As Object, ByVal Args As WeOnlyDo.Client.SSH.ConnectedArgs) Handles Ssh.ConnectedEvent

            If Not (Args.Error Is Nothing) Then
                bFailed = True
                Logger.LogError(String.Format("Store {0} - SSH_ConnectedEvent error: " & Args.Error.ToString, _currentStore.StoreNum), Me.GetType())
            Else
                lcount = 0
                bCompleted = False
            End If

        End Sub

        Private Sub Ssh_DisconnectedEvent(ByVal Sender As Object, ByVal Args As System.EventArgs) Handles Ssh.DisconnectedEvent

            If Not bCompleted Then 'Did not run command
                Logger.LogError(String.Format("Store {0} - Did not complete batch run for store ", _currentStore.StoreNum), Me.GetType)
                bFailed = True
            End If

        End Sub

        Protected Overridable Sub ssh1_DataReceivedEvent(ByVal Sender As Object, ByVal Args As WeOnlyDo.Client.SSH.DataReceivedArgs) Handles Ssh.DataReceivedEvent
            Dim sCommand As String
            Dim msTelnetPassword As String = String.Empty

            Dim storeFtpConfigDAO As New StoreFTPConfigDAO

            Try
                Dim sResponse As String

                sResponse = Ssh.Receive

                Select Case True
                    'Case InStr(1, sResponse, "New Password", CompareMethod.Binary) > 0
                    Case InStr(1, sResponse, "expired", CompareMethod.Binary) > 0
                        'Generate the new password
                        msTelnetPassword = RandomPassword.Generate(8)

                        'Update the store's new password in the StoreFTPConfig table 
                        storeFtpConfigDAO.UpdateFTPPassword(_currentStore.FTPInfo.IPAddress, msTelnetPassword)

                        'Apply to the POS
                        Ssh.Send(msTelnetPassword & vbCrLf) 'New Password
                        Ssh.Send(msTelnetPassword & vbCrLf) 'Verify

                    Case InStr(1, sResponse, "Operator", CompareMethod.Binary) > 0
                        Ssh.Send(_currentStore.FTPInfo.FTPUser & vbCrLf)

                    Case InStr(1, sResponse, "Password", CompareMethod.Binary) > 0
                        Ssh.Send(_currentStore.FTPInfo.FTPPassword & vbCrLf)

                    Case InStr(1, sResponse, "selection", CompareMethod.Binary) > 0
                        Ssh.Send("7" & vbCrLf)

                    Case InStr(1, sResponse, "C:>", CompareMethod.Binary) > 0
                        If lcount >= 3 Then
                            Ssh.Disconnect()
                        Else
                            lcount = lcount + 1
                            sCommand = _RemoteJobDirectory + IIf(Len(_RemoteJobDirectory) > 0, ":", "").ToString + _RemoteJobCommand
                            Logger.LogError(String.Format("Store {0} - Remote Command : " & sCommand, _currentStore.StoreNum), Me.GetType())

                            Ssh.Send(sCommand & vbCrLf)
                            'Ssh.Send(_RemoteJobCommand & vbCrLf)

                            bCompleted = True
                        End If
                End Select

            Catch ex As Exception
                bFailed = True
                ErrorHandler.ProcessError(ErrorType.SSHException, SeverityLevel.Warning, ex)
                Logger.LogError(String.Format("Store {0} - SSH_DataReceivedEvent failed with error: " & ex.ToString, _currentStore.StoreNum), Me.GetType())
                Ssh.Disconnect()
            End Try

        End Sub
        Private Sub ExisSSHSession(ByVal state As Object)
            bTimedOut = True
        End Sub

        Public Overrides Function CallSSHRemoteJobProcess(ByRef currentStore As StoreUpdatesBO, ByVal filename As String) As Boolean
            Logger.LogDebug("Call SSH Remote Job Process entry", Me.GetType)

            Dim success As Boolean = True

            _currentStore = currentStore

            Overrides_RemoteSSHStoreList.Append(_currentStore.StoreNum.ToString)
            Overrides_RemoteSSHStoreList.Append(" = ")

            Ssh = New WeOnlyDo.Client.SSH()

            Ssh.Authentication = WeOnlyDo.Client.SSH.Authentications.PublicKey
            Ssh.Login = currentStore.FTPInfo.FTPUser

            Ssh.DebugFile = "ssh_debug.txt"
            Ssh.LicenseKey = "PCWC-QC2P-F9XD-Q2GQ"

            Dim km As WeOnlyDo.Security.Cryptography.KeyManager = New WeOnlyDo.Security.Cryptography.KeyManager
            km.Load("IRMA Private Key", "Irma12#")
            Ssh.PrivateKey = km.PrivateKey(WeOnlyDo.Security.Cryptography.SSHKeyTypes.RSAKey)
            km.Dispose()
            Ssh.Protocol = WeOnlyDo.Client.SSH.SupportedProtocols.SSH2

            Dim pushTimer As System.Threading.Timer = _
                               New System.Threading.Timer( _
                                   New TimerCallback(AddressOf ExisSSHSession), _
                                   Nothing, _
                                   New TimeSpan(0, 0, 0, 60), _
                                   Nothing)

            Try
                'remotely issue command to begin processing the batch control file
                Ssh.Connect(currentStore.FTPInfo.IPAddress, 22)

                Do
                    Application.DoEvents()
                    System.Threading.Thread.Sleep(4000)
                Loop Until Me.bFailed OrElse Me.bCompleted OrElse Me.bTimedOut

                If (Not Ssh.State = WeOnlyDo.Client.SSH.States.Disconnected) Then
                    Ssh.Disconnect()
                End If

            Catch exSSH As WeOnlyDo.Exceptions.SSH.SSH
                Dim args(2) As String
                args(0) = currentStore.StoreNum.ToString
                args(1) = currentStore.FTPInfo.IPAddress
                args(2) = currentStore.FTPInfo.FTPUser
                ErrorHandler.ProcessError(ErrorType.SSHException, args, SeverityLevel.Warning, exSSH)
                Logger.LogError(String.Format("Store {0} - Remote SSH command execution error: ", currentStore.StoreNum, exSSH.Message), Me.GetType)

            Catch ex As Exception
                ErrorHandler.ProcessError(ErrorType.SSHException, SeverityLevel.Warning, ex)
                Logger.LogError(String.Format("Store {0} - Remote SSH command execution error: ", currentStore.StoreNum, ex.Message), Me.GetType)

            Finally
                pushTimer.dispose()

                Ssh = Nothing

                If (bCompleted = True) Then
                    Overrides_RemoteSSHStoreList.Append("**Passed** ")
                    Logger.WriteLog(filename, "PASSED")

                    success = True
                Else
                    Overrides_RemoteSSHStoreList.Append("**FAILED** ")
                    Logger.WriteLog(filename, "FAILED")

                    success = False
                End If
                Logger.WriteLog(filename, vbCrLf)

                bFailed = False
                bCompleted = False

                Logger.LogDebug("Call SSH Remote Job Process exit", Me.GetType)

            End Try

            Return success
        End Function
    End Class
End Namespace

