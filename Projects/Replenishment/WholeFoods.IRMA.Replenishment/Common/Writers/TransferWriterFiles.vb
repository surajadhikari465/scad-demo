Imports System.IO
Imports System.Text
Imports log4net
Imports WholeFoods.IRMA.Replenishment.POSPush.BusinessLogic
Imports WholeFoods.IRMA.Replenishment.POSPush.DataAccess
Imports WholeFoods.Utility
Imports WholeFoods.Utility.FTP
Imports WholeFoods.Utility.wodSFTP

Namespace WholeFoods.IRMA.Replenishment.Common.Writers

    Public Class TransferWriterFiles
        Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
        Private _storeList As New StringBuilder
        Private _deleteFilesOnFailure As Boolean = False
        Private _hasTriggeredAlert As Boolean = False
        Public StopAlerts As Boolean = False

#Region "Constructors"
        ''' <summary>
        ''' Use default settings
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
            _deleteFilesOnFailure = True     'set to True so previous default behavior remains the same
        End Sub

        ''' <summary>
        ''' Allows optional deletion of files upon failure
        ''' </summary>
        ''' <param name="deleteFilesOnFailure">When True, deletes the generated files upon failure.  Otherwise the files are kept.</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal deleteFilesOnFailure As Boolean)
            _deleteFilesOnFailure = deleteFilesOnFailure
        End Sub
#End Region

#Region "Property Access Methods"
        Public ReadOnly Property StoreList() As String
            Get
                Return _storeList.ToString
            End Get
        End Property

        Friend Property DeleteFilesOnFailure() As Boolean
            Get
                Return _deleteFilesOnFailure
            End Get
            Set(ByVal value As Boolean)
                _deleteFilesOnFailure = value
            End Set
        End Property
#End Region

        ''' <summary>
        ''' Transfer the POS/SCALE file for each store.
        ''' The type of transfer (direct or sent) for each store is determined by the StorePOSConfig 
        ''' configuration values.
        ''' </summary>
        ''' <param name="storeUpdates"></param>
        ''' <remarks>
        ''' - direct connections ftp the file to the pos host and wait to receive a response code before
        ''' the transfer is considered a success
        ''' - sent connections ftp the file to the pos host and consider the transfer a success if there are
        ''' not any ftp errors; no response is sent from the post host to this process
        ''' </remarks>
        Public Function TransferStoreFiles(ByRef storeUpdates As Hashtable, Optional ByVal overrideFilename As String = Nothing) As Boolean
            logger.Debug("TransferPOSFiles entry")

            _hasTriggeredAlert = False

            Dim success As Boolean = True 'reset to false if error occurs for any stores
            Dim currentStore As StoreUpdatesBO
            Dim ftpUtil As FTPclient = Nothing
            Dim sftpUtil As wodSFTP.SFTPClient = Nothing

            Dim msg As String
            msg = Nothing

            If (storeUpdates.Count > 0) Then
                ' Iterate through all of the Stores and FTP the change file for each store
                Dim storeEnum As IEnumerator = storeUpdates.Values.GetEnumerator()

                While (storeEnum.MoveNext())
                    currentStore = CType(storeEnum.Current, StoreUpdatesBO)
                    logger.Debug("currentStore.StoreNum=" + currentStore.StoreNum.ToString)
                    If (_storeList.Length <> 0) Then
                        _storeList.Append(", ")
                    End If
                    _storeList.Append(currentStore.StoreNum.ToString)
                    _storeList.Append(" = ")

                    ' make sure the store has at least one change before trying to send the file.
                    logger.Debug("currentStore.HasCurrentChanges=" + currentStore.HasCurrentChanges.ToString)

                    If currentStore.HasCurrentChanges Then
                        Try
                            If currentStore.FTPInfo Is Nothing Then
                                msg = "Nothing"
                            Else
                                msg = "Has FTP Info"
                            End If

                            logger.Debug("currentStore.FTPInfo=" + msg)

                            If currentStore.FTPInfo IsNot Nothing Then
                                If currentStore.FTPInfo.Port IsNot Nothing Then
                                    logger.Debug("currentStore.FTPInfo.Port=" + currentStore.FTPInfo.Port.ToString)
                                    logger.Debug("currentStore.FTPInfo.IPAddress=" + currentStore.FTPInfo.IPAddress.ToString)

                                    If (currentStore.FTPInfo.IsSecureTransfer) Then
                                        logger.Info("TransferStoreFiles - creating FTP object for secure file transfer: StoreNum=" + currentStore.StoreNum.ToString + ", IPAddress=" + currentStore.FTPInfo.IPAddress.ToString)
                                        sftpUtil = New SFTPClient(currentStore.FTPInfo.IPAddress, currentStore.FTPInfo.FTPUser, currentStore.FTPInfo.FTPPassword)
                                    Else
                                        logger.Info("TransferStoreFiles - creating FTP object for unsecure file transfer: StoreNum=" + currentStore.StoreNum.ToString + ", IPAddress=" + currentStore.FTPInfo.IPAddress.ToString)
                                        ftpUtil = New FTPclient(currentStore.FTPInfo.IPAddress, currentStore.FTPInfo.FTPUser, currentStore.FTPInfo.FTPPassword, currentStore.FTPInfo.IsSecureTransfer, CType(currentStore.FTPInfo.Port, Integer))
                                    End If
                                Else
                                    logger.Debug("currentStore.FTPInfo.IPAddress=" + currentStore.FTPInfo.IPAddress.ToString)
                                    logger.Debug("currentStore.FTPInfo.FTPUser=" + currentStore.FTPInfo.FTPUser.ToString)
                                    logger.Debug("currentStore.FTPInfo.FTPPassword=" + currentStore.FTPInfo.FTPPassword.ToString)

                                    If (currentStore.FTPInfo.IsSecureTransfer) Then
                                        logger.Info("TransferStoreFiles - creating FTP object for secure file transfer: StoreNum=" + currentStore.StoreNum.ToString + ", IPAddress=" + currentStore.FTPInfo.IPAddress.ToString)
                                        sftpUtil = New SFTPClient(currentStore.FTPInfo.IPAddress, currentStore.FTPInfo.FTPUser, currentStore.FTPInfo.FTPPassword)
                                    Else
                                        logger.Info("TransferStoreFiles - creating FTP object for unsecure file transfer: StoreNum=" + currentStore.StoreNum.ToString + ", IPAddress=" + currentStore.FTPInfo.IPAddress.ToString)
                                        ftpUtil = New FTPclient(currentStore.FTPInfo.IPAddress, currentStore.FTPInfo.FTPUser, currentStore.FTPInfo.FTPPassword, currentStore.FTPInfo.IsSecureTransfer)
                                    End If
                                End If

                                If overrideFilename IsNot Nothing Then
                                    If (currentStore.ShelfTagFiles.Count > 0) Then
                                        Dim str As String
                                        For Each str In currentStore.ShelfTagFiles
                                            logger.Info("TransferStoreFiles - sending shelf tag file: filename=" + str)
                                            If (currentStore.FTPInfo.IsSecureTransfer) Then
                                                sftpUtil.Upload(str, Path.Combine(currentStore.FTPInfo.ChangeDirectory, overrideFilename), currentStore.FileWriter.AppendToFile)
                                            Else
                                                ftpUtil.Upload(str, Path.Combine(currentStore.FTPInfo.ChangeDirectory, overrideFilename), currentStore.FileWriter.AppendToFile)
                                            End If
                                        Next
                                    Else
                                        If (currentStore.FTPInfo.IsSecureTransfer) Then
                                            logger.Info("TransferStoreFiles - sending batch file: filename=" + currentStore.BatchFileName)
                                            sftpUtil.Upload(currentStore.BatchFileName, Path.Combine(currentStore.FTPInfo.ChangeDirectory, overrideFilename), currentStore.FileWriter.AppendToFile)
                                        Else
                                            logger.Info("TransferStoreFiles - sending batch file: filename=" + currentStore.BatchFileName)
                                            ftpUtil.Upload(currentStore.BatchFileName, Path.Combine(currentStore.FTPInfo.ChangeDirectory, overrideFilename), currentStore.FileWriter.AppendToFile)
                                        End If
                                    End If

                                    If currentStore.ExemptTagFileName IsNot Nothing Then
                                        Dim str As String
                                        For Each str In currentStore.ExemptShelfTagFiles
                                            logger.Info("TransferStoreFiles - sending exempt shelf tag file: filename=" + str)
                                            If (currentStore.FTPInfo.IsSecureTransfer) Then
                                                sftpUtil.Upload(currentStore.ExemptTagFileName, Path.Combine(currentStore.FTPInfo.ChangeDirectory, overrideFilename), currentStore.ExemptFileWriter.AppendToFile)
                                            Else
                                                ftpUtil.Upload(currentStore.ExemptTagFileName, Path.Combine(currentStore.FTPInfo.ChangeDirectory, overrideFilename), currentStore.ExemptFileWriter.AppendToFile)
                                            End If
                                        Next
                                    End If
                                Else
                                    'DaveStacey 20080513 - Change routine so the property check doesn't wipe out the temp file name
                                    If (currentStore.ShelfTagFiles.Count > 0) Then
                                        Dim str As String
                                        For Each str In currentStore.ShelfTagFiles
                                            logger.Info("TransferStoreFiles - sending shelf tag file: filename=" + str)
                                            If (currentStore.FTPInfo.IsSecureTransfer) Then
                                                sftpUtil.Upload(str, Path.Combine(currentStore.FTPInfo.ChangeDirectory, str.Substring(str.LastIndexOf("\") + 1)), currentStore.FileWriter.AppendToFile)
                                            Else
                                                ftpUtil.Upload(str, Path.Combine(currentStore.FTPInfo.ChangeDirectory, str.Substring(str.LastIndexOf("\") + 1)), currentStore.FileWriter.AppendToFile)
                                            End If
                                        Next
                                        'DStacey - 20070706 - Added handling for situation when Regular Exempt is the only file sent
                                        'DStacey - 20070815 - Changed call to ExemptTagFileName to count = 0 due to the fact calling the name property changes the temp file name which kills archiving
                                    ElseIf (currentStore.ExemptShelfTagFiles.Count = 0) Then
                                        logger.Info("TransferStoreFiles - sending batch file: filename=" + currentStore.BatchFileName)
                                        If (currentStore.FTPInfo.IsSecureTransfer) Then
                                            sftpUtil.Upload(currentStore.BatchFileName, Path.Combine(currentStore.FTPInfo.ChangeDirectory, currentStore.FileWriter.WriterFilename(currentStore)), currentStore.FileWriter.AppendToFile)
                                        Else
                                            ftpUtil.Upload(currentStore.BatchFileName, Path.Combine(currentStore.FTPInfo.ChangeDirectory, currentStore.FileWriter.WriterFilename(currentStore)), currentStore.FileWriter.AppendToFile)
                                        End If
                                    End If

                                    If currentStore.ExemptShelfTagFiles.Count > 0 Then
                                        Dim str As String
                                        For Each str In currentStore.ExemptShelfTagFiles
                                            logger.Info("TransferStoreFiles - sending exempt shelf tag file: filename=" + str)
                                            If (currentStore.FTPInfo.IsSecureTransfer) Then
                                                sftpUtil.Upload(str, Path.Combine(currentStore.FTPInfo.ChangeDirectory, str.Substring(str.LastIndexOf("\") + 1)), currentStore.ExemptFileWriter.AppendToFile)
                                            Else
                                                ftpUtil.Upload(str, Path.Combine(currentStore.FTPInfo.ChangeDirectory, str.Substring(str.LastIndexOf("\") + 1)), currentStore.ExemptFileWriter.AppendToFile)
                                            End If
                                        Next
                                    End If
                                End If

                                'move current file to archive directory w/ timestamp appended to filename
                                currentStore.ArchiveProcessedBatchFile()

                                'some writers (such as the IBM Binary) require an additional control file to be sent to the POS
                                'along with the batch file - send this file 
                                If File.Exists(currentStore.ControlFileName) Then
                                    logger.Info("TransferStoreFiles - processing control file: ControlFileName=" + currentStore.ControlFileName)
                                    If (currentStore.FTPInfo.IsSecureTransfer) Then
                                        sftpUtil.Upload(currentStore.ControlFileName, Path.Combine(currentStore.FTPInfo.ChangeDirectory, currentStore.FileWriter.ControlFilename), currentStore.FileWriter.AppendToFile)
                                    Else
                                        ftpUtil.Upload(currentStore.ControlFileName, Path.Combine(currentStore.FTPInfo.ChangeDirectory, currentStore.FileWriter.ControlFilename), currentStore.FileWriter.AppendToFile)
                                    End If
                                    'move control file to archive directory w/ timestamp appended to filename
                                    currentStore.ArchiveProcessedControlFile()
                                End If

                                'indicate that this store's file(s) were sent successfully so IRMA changes can be applied
                                currentStore.ChangesDelivered = True
                                _storeList.Append("delivered")
                            Else
                                'FTP settings not setup for store - changes NOT delivered
                                currentStore.ChangesDelivered = False
                                _storeList.Append("** MISSING FTP CONFIG **")
                                logger.Info("TransferStoreFiles - missing FTP configuration: StoreNum=" + currentStore.StoreNum.ToString)

                                If _deleteFilesOnFailure Then
                                    'remove temporary files for current store. data will be picked up in next run of job
                                    logger.Info("TransferStoreFiles - deleting temporary file that was not delivered: BatchFileName=" + currentStore.BatchFileName)
                                    File.Delete(currentStore.BatchFileName)
                                    If currentStore.ExemptTagFileName IsNot Nothing Then
                                        logger.Info("TransferStoreFiles - deleting temporary file that was not delivered: ExemptTagFileName=" + currentStore.ExemptTagFileName)
                                        File.Delete(currentStore.ExemptTagFileName)
                                    End If
                                    logger.Info("TransferStoreFiles - deleting temporary file that was not delivered: ControlFileName=" + currentStore.ControlFileName)
                                    File.Delete(currentStore.ControlFileName)
                                End If

                                'send message about exception
                                Dim args(3) As String
                                args(0) = currentStore.StoreNum.ToString
                                args(1) = "NOT CONFIGURED!"
                                args(2) = "NOT CONFIGURED!"

                                Throw New Exception("Configure the FTP parameters for this store using the Admin Client.")

                                success = False
                            End If
                        Catch ex As Exception
                            currentStore.ChangesDelivered = False
                            _storeList.Append("** FTP ERROR **")
                            logger.Error("TransferStoreFiles - error during FTP processing", ex)

                            If _deleteFilesOnFailure Then
                                'remove temporary files for current store. data will be picked up in next run of job
                                logger.Info("TransferStoreFiles - deleting temporary file that was not delivered: BatchFileName=" + currentStore.BatchFileName)
                                File.Delete(currentStore.BatchFileName)
                                If currentStore.ExemptTagFileName IsNot Nothing Then
                                    logger.Info("TransferStoreFiles - deleting temporary file that was not delivered: ExemptTagFileName=" + currentStore.ExemptTagFileName)
                                    File.Delete(currentStore.ExemptTagFileName)
                                End If
                                logger.Info("TransferStoreFiles - deleting temporary file that was not delivered: ControlFileName=" + currentStore.ControlFileName)
                                File.Delete(currentStore.ControlFileName)
                            End If

                            'send message about exception
                            Dim args(3) As String
                            args(0) = currentStore.StoreNum.ToString
                            args(1) = currentStore.FTPInfo.IPAddress
                            args(2) = currentStore.FTPInfo.FTPUser
                            ErrorHandler.ProcessError(ErrorType.FTPException, args, SeverityLevel.Warning, ex)

                            success = False

                            Dim alertError As String = String.Format("FTP error occurred for store: {0} - {1}.  IP Address: {2}, FTP User: {3}, Exception: {4}",
                                                                     currentStore.StoreNum, currentStore.StoreName, currentStore.FTPInfo.IPAddress, currentStore.FTPInfo.FTPUser, ex.ToString())

                            If (Not _hasTriggeredAlert And Not StopAlerts) Then
                                PagerDutyUtility.TriggerPagerDutyAlert("IRMA Push Job", "IRMA Push FTP Failure", alertError)
                                _hasTriggeredAlert = True
                            End If
                        End Try
                    Else
                        logger.Info("TransferStoreFiles - no changes to deliver: StoreNum=" + currentStore.StoreNum.ToString)
                        _storeList.Append("no changes")
                    End If
                End While
            End If

            logger.Debug("TransferPOSFiles exit")

            Return success
        End Function

        Public Function TransferStoreFilesBT(ByRef storeUpdates As Hashtable) As Boolean
            logger.Debug("TransferPOSFiles entry")
            Dim success As Boolean = True 'reset to false if error occurs for any stores
            Dim currentStore As StoreUpdatesBO = Nothing

            Dim ftpUtil As FTPclient = Nothing
            Dim sftpUtil As wodSFTP.SFTPClient = Nothing

            Dim msg As String
            msg = Nothing

            'ftp variables
            Dim stFTPAddress As String = String.Empty
            Dim stPort As String = String.Empty
            Dim stChangeDir As String = String.Empty
            Dim stUsername As String = String.Empty
            Dim stPassword As String = String.Empty
            Dim stDropFilename As String = String.Empty
            Dim bIsSecure As Boolean = False
            Dim stDropControlFileName As String
            Dim stRegion As String = Nothing
            Dim dsXML As DataSet = CreatePOSPushXMLTable("POSPushEnvelope", "BizXML")
            Dim stBatchPath As String


            If (storeUpdates.Count > 0) Then
                ' Iterate through all of the Stores and FTP the change file for each store
                Dim storeEnum As IEnumerator = storeUpdates.Values.GetEnumerator()

                While (storeEnum.MoveNext())
                    currentStore = CType(storeEnum.Current, StoreUpdatesBO)
                    stBatchPath = currentStore.BatchFilePath
                    logger.Debug("currentStore.StoreNum=" + currentStore.StoreNum.ToString)
                    If (_storeList.Length <> 0) Then
                        _storeList.Append(", ")
                    End If
                    _storeList.Append(currentStore.StoreNum.ToString)
                    _storeList.Append(" = ")
                    ' make sure the store has at least one change before trying to send the file.
                    logger.Debug("currentStore.HasCurrentChanges=" + currentStore.HasCurrentChanges.ToString)
                    If currentStore.HasCurrentChanges Then
                        Try
                            If currentStore.FTPInfo Is Nothing Then
                                msg = "Nothing"
                            Else
                                msg = "Has FTP Info"
                            End If
                            logger.Debug("currentStore.FTPInfo=" + msg)

                            'set ftp variables using biztalk info
                            Dim results As DataTable = StorePOSConfigDAO.GetRegionFTPConfigInfo("BizTalk")

                            If results.Rows.Count > 0 Then
                                Dim dr As DataRow
                                dr = results.Rows(0)
                                stFTPAddress = dr.Item("FTPAddress").ToString
                                stPort = dr.Item("Port").ToString
                                stUsername = dr.Item("Username").ToString
                                stPassword = dr.Item("Password").ToString
                                stChangeDir = dr.Item("ChangeDir").ToString
                                stRegion = dr.Item("Region").ToString

                                'need filename stuff
                            End If

                            If currentStore.FTPInfo IsNot Nothing Then
                                logger.Debug("currentStore.FTPInfo.IPAddress=" + stFTPAddress)
                                logger.Debug("currentStore.FTPInfo.FTPUser=" + stUsername)
                                logger.Debug("currentStore.FTPInfo.FTPPassword=" + stPassword)

                                logger.Info("TransferStoreFiles - creating FTP object for unsecure file transfer: StoreNum=" + currentStore.StoreNum.ToString + ", IPAddress=" + stFTPAddress)
                                ftpUtil = New FTPclient(stFTPAddress, stUsername, stPassword, False)

                                logger.Info("TransferStoreFiles - sending batch file: filename=" + currentStore.BatchFileName)

                                If currentStore.FileWriter.AppendToFile Then
                                    stDropFilename = stRegion & currentStore.FTPInfo.BusinessUnitID.ToString & currentStore.FTPInfo.FileWriterType
                                Else
                                    stDropFilename = stRegion & currentStore.FTPInfo.BusinessUnitID.ToString & currentStore.FTPInfo.FileWriterType & Date.Now.ToString("yyyymmddhhmmss")
                                End If
                                ftpUtil.Upload(currentStore.BatchFileName, Path.Combine(stChangeDir, stDropFilename), currentStore.FileWriter.AppendToFile)

                                'Create row in data table for this file entry 
                                AddRowPOSPushXMLTable(currentStore, dsXML, stDropFilename)

                                'move current file to archive directory w/ timestamp appended to filename
                                currentStore.ArchiveProcessedBatchFile()

                                'some writers (such as the IBM Binary) require an additional control file to be sent to the POS
                                'along with the batch file - send this file 
                                If File.Exists(currentStore.ControlFileName) Then
                                    logger.Info("TransferStoreFiles - processing control file: ControlFileName=" + currentStore.ControlFileName)
                                    stDropControlFileName = stRegion & currentStore.FTPInfo.BusinessUnitID.ToString & currentStore.FileWriter.ToString & "_Con"
                                    ftpUtil.Upload(currentStore.ControlFileName, Path.Combine(stChangeDir, stDropControlFileName), currentStore.FileWriter.AppendToFile)

                                    ' Create row in data table for this file entry 
                                    AddRowPOSPushXMLTable(currentStore, dsXML, stDropControlFileName)

                                    'move control file to archive directory w/ timestamp appended to filename
                                    currentStore.ArchiveProcessedControlFile()
                                End If


                                'indicate that this store's file(s) were sent successfully so IRMA changes can be applied
                                currentStore.ChangesDelivered = True
                                _storeList.Append("delivered")
                            Else
                                'FTP settings not setup for store - changes NOT delivered
                                currentStore.ChangesDelivered = False
                                _storeList.Append("** MISSING FTP CONFIG **")
                                logger.Info("TransferStoreFiles - missing FTP configuration: StoreNum=" + currentStore.StoreNum.ToString)

                                If _deleteFilesOnFailure Then
                                    'remove temporary files for current store. data will be picked up in next run of job
                                    logger.Info("TransferStoreFiles - deleting temporary file that was not delivered: BatchFileName=" + currentStore.BatchFileName)
                                    File.Delete(currentStore.BatchFileName)
                                    If currentStore.ExemptTagFileName IsNot Nothing Then
                                        logger.Info("TransferStoreFiles - deleting temporary file that was not delivered: ExemptTagFileName=" + currentStore.ExemptTagFileName)
                                        File.Delete(currentStore.ExemptTagFileName)
                                    End If
                                    logger.Info("TransferStoreFiles - deleting temporary file that was not delivered: ControlFileName=" + currentStore.ControlFileName)
                                    File.Delete(currentStore.ControlFileName)
                                End If

                                'send message about exception
                                Dim args(3) As String
                                args(0) = currentStore.StoreNum.ToString
                                args(1) = "NOT CONFIGURED!"
                                args(2) = "NOT CONFIGURED!"

                                Throw New Exception("Configure the FTP parameters for this store using the Admin Client.")

                                success = False
                            End If
                        Catch ex As Exception
                            currentStore.ChangesDelivered = False
                            _storeList.Append("** FTP ERROR **")
                            logger.Error("TransferStoreFiles - error during FTP processing", ex)

                            If _deleteFilesOnFailure Then
                                'remove temporary files for current store. data will be picked up in next run of job
                                logger.Info("TransferStoreFiles - deleting temporary file that was not delivered: BatchFileName=" + currentStore.BatchFileName)
                                File.Delete(currentStore.BatchFileName)
                                If currentStore.ExemptTagFileName IsNot Nothing Then
                                    logger.Info("TransferStoreFiles - deleting temporary file that was not delivered: ExemptTagFileName=" + currentStore.ExemptTagFileName)
                                    File.Delete(currentStore.ExemptTagFileName)
                                End If
                                logger.Info("TransferStoreFiles - deleting temporary file that was not delivered: ControlFileName=" + currentStore.ControlFileName)
                                File.Delete(currentStore.ControlFileName)
                            End If

                            'send message about exception
                            Dim args(3) As String
                            args(0) = currentStore.StoreNum.ToString
                            args(1) = currentStore.FTPInfo.IPAddress
                            args(2) = currentStore.FTPInfo.FTPUser
                            ErrorHandler.ProcessError(ErrorType.FTPException, args, SeverityLevel.Warning, ex)

                            success = False
                        End Try
                    Else
                        logger.Info("TransferStoreFiles - no changes to deliver: StoreNum=" + currentStore.StoreNum.ToString)
                        _storeList.Append("no changes")
                    End If
                End While

                SendXMLtoBizTalk(dsXML, currentStore.BatchFilePath)

            End If
            logger.Debug("TransferPOSFiles exit")

            Return success

        End Function

        Public Shared Function SendXMLtoBizTalk(ByVal dsXML As DataSet, ByVal stBatchPath As String) As Boolean
            Dim stFTPAddress As String = ""
            Dim stPort As String = ""
            Dim stChangeDir As String = ""
            Dim stUsername As String = ""
            Dim stPassword As String = ""
            Dim stRegion As String = Nothing
            Dim ftpUtil As FTPclient = Nothing

            Try
                If dsXML.Tables(0).Rows.Count > 0 Then
                    Dim stListFileName As String
                    Dim results As DataTable = StorePOSConfigDAO.GetRegionFTPConfigInfo("BizTalkXML")

                    If results.Rows.Count > 0 Then
                        Dim dr As DataRow
                        dr = results.Rows(0)
                        stFTPAddress = dr.Item("FTPAddress").ToString
                        stPort = dr.Item("Port").ToString
                        stUsername = dr.Item("Username").ToString
                        stPassword = dr.Item("Password").ToString
                        stChangeDir = dr.Item("ChangeDir").ToString
                        stRegion = dr.Item("Region").ToString
                    End If

                    ftpUtil = New FTPclient(stFTPAddress, stUsername, stPassword, False)

                    stListFileName = stRegion & Date.Now.ToString("yyyymmddhhmmss") & ".xml"
                    dsXML.WriteXml(Path.Combine(stBatchPath, stListFileName))

                    ftpUtil.Upload(Path.Combine(stBatchPath, stListFileName), Path.Combine(stChangeDir, stListFileName), False)
                    If File.Exists(Path.Combine(stBatchPath, stListFileName)) Then
                        File.Move(Path.Combine(stBatchPath, stListFileName), Path.Combine(stBatchPath & "\Archive\", stListFileName))
                        File.Delete(Path.Combine(stBatchPath, stListFileName))
                    End If
                End If
                Return True
            Catch ex As Exception
                Throw
            End Try
        End Function

        Public Shared Function CreatePOSPushXMLTable(ByVal rootName As String, ByVal tableName As String) As DataSet
            Dim ds As New DataSet(rootName)
            Dim dt As New DataTable(tableName)
            Dim stEmail As String = ConfigurationServices.AppSettings("BTNotificationEmail")
            Dim stUNCPath As String = ConfigurationServices.AppSettings("BTUNCPath")
            Dim bSuccessEmail As String = ConfigurationServices.AppSettings("BTSendSuccessEmail")

            dt.Namespace = "http://POSPushJob.RoutingInfoDocument"
            ds.Namespace = "http://POSPushJob.RoutingInfoEnvelope"

            Dim column As New DataColumn("ID", GetType(System.Int32))
            column.AutoIncrement = True
            dt.Columns.Add(column)

            column = New DataColumn("FileName", GetType(System.String))
            dt.Columns.Add(column)

            column = New DataColumn("FTPAddress", GetType(System.String))
            dt.Columns.Add(column)

            column = New DataColumn("Port", GetType(System.String))
            column.DefaultValue = "0"
            dt.Columns.Add(column)

            column = New DataColumn("ChangeDir", GetType(System.String))
            dt.Columns.Add(column)

            column = New DataColumn("Username", GetType(System.String))
            dt.Columns.Add(column)

            column = New DataColumn("Password", GetType(System.String))
            dt.Columns.Add(column)

            column = New DataColumn("DropFileName", GetType(System.String))
            dt.Columns.Add(column)

            column = New DataColumn("Secure", GetType(System.Boolean))
            dt.Columns.Add(column)

            column = New DataColumn("Email", GetType(System.String))
            column.DefaultValue = stEmail
            dt.Columns.Add(column)

            If stUNCPath.Substring(Len(stUNCPath) - 1, 1) <> "\" Then
                stUNCPath = stUNCPath & "\"
            End If

            column = New DataColumn("UNCPath", GetType(System.String))
            column.DefaultValue = stUNCPath
            dt.Columns.Add(column)

            column = New DataColumn("SendSuccessEmail", GetType(System.Boolean))
            column.DefaultValue = bSuccessEmail
            dt.Columns.Add(column)

            column = New DataColumn("AppendToFile", GetType(System.Boolean))
            dt.Columns.Add(column)

            ds.Tables.Add(dt)

            Return ds
        End Function

        Public Shared Sub AddRowPOSPushXMLTable(ByVal suCurrent As StoreUpdatesBO, ByVal dsXML As DataSet, ByVal stRemoteFileName As String)
            Dim dr As DataRow = dsXML.Tables(0).NewRow
            Dim ob As Object = suCurrent.FTPInfo.Port
            Dim stChangeDir As String = suCurrent.FTPInfo.ChangeDirectory


            dr.Item("FileName") = stRemoteFileName
            dr.Item("FTPAddress") = suCurrent.FTPInfo.IPAddress
            If IsDBNull(ob) Or IsNothing(ob) Then
                If suCurrent.FTPInfo.IsSecureTransfer Then
                    suCurrent.FTPInfo.Port = "22"
                Else
                    suCurrent.FTPInfo.Port = "21"
                End If
            End If
            dr.Item("Port") = suCurrent.FTPInfo.Port

            If stChangeDir.Substring(Len(stChangeDir) - 1, 1) <> "/" Then
                stChangeDir = stChangeDir & "/"
            End If

            dr.Item("ChangeDir") = stChangeDir
            dr.Item("Username") = suCurrent.FTPInfo.FTPUser
            dr.Item("Password") = suCurrent.FTPInfo.FTPPassword
            dr.Item("DropFileName") = suCurrent.FileWriter.WriterFilename(suCurrent)
            dr.Item("Secure") = suCurrent.FTPInfo.IsSecureTransfer
            dr.Item("AppendToFile") = suCurrent.FileWriter.AppendToFile
            dsXML.Tables(0).Rows.Add(dr)

        End Sub

    End Class

End Namespace