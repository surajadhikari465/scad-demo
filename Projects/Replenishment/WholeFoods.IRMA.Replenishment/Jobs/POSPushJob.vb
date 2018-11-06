Imports System.IO
Imports System.Text
Imports WholeFoods.IRMA.Replenishment.Common.BusinessLogic
Imports WholeFoods.IRMA.Replenishment.Common.Writers
Imports WholeFoods.IRMA.Replenishment.POSPush.BusinessLogic
Imports WholeFoods.IRMA.Replenishment.POSPush.Controller
Imports WholeFoods.IRMA.Replenishment.POSPush.DataAccess
Imports WholeFoods.IRMA.Replenishment.TagPush.Controller
Imports WholeFoods.Utility
Imports WholeFoods.Utility.SMTP

Namespace WholeFoods.IRMA.Replenishment.Jobs

    ''' <summary>
    ''' Job that performs the POS Push process.
    ''' </summary>
    ''' <remarks>
    ''' ASSUMPTION: this is NOT designed to be multi-threaded.  If process must become multi-threaded then 
    ''' file management in StoreUpdatesBO.vb should no longer remove file if temp file exists upon
    ''' process startup.
    ''' </remarks>
    Public Class POSPushJob
        Inherits ScheduledJob

#Region "Events raised by this job"
        ' These events are raised during key steps of the process so the U.I. can let the user know
        ' where in the process things are.
        Public Event ScalePushStarted(ByVal IsRegional As Boolean)
        Public Event ScaleReadStoreConfigurationData(ByVal NumStores As Integer)
        Public Event ScaleReadItemIdAdds()
        Public Event ScaleReadItemIdDeletes()
        Public Event ScaleReadItemPriceChanges()
        Public Event ScaleReadZoneDeletes()
        Public Event ScaleReadZonePriceChanges()
        Public Event ScaleTransferFiles(ByVal FileStatus As String)
        Public Event ScaleCorpTempQueueCleared()
        Public Event ScaleCompleteSuccess()
        Public Event ScaleCompleteError()

        Public Event POSPushStarted()
        Public Event POSReadStoreConfigurationData(ByVal NumStores As Integer)
        Public Event POSReadStoreBatchesAdds()
        Public Event POSReadTaxflagData()
        Public Event POSReadVendorAdds()
        Public Event POSReadItemDeletes()
        Public Event POSReadItemPriceChanges()
        Public Event POSReadItemIdAdds()
        Public Event POSReadItemRefreshes()
        Public Event POSReadItemIdDeletes()
        Public Event POSReadPromoOffers()
        Public Event POSGeneratedPOSControlFiles()
        Public Event POSTransferFiles(ByVal FileStatus As String)
        Public Event POSApplyChangesToIRMA()
        Public Event POSStartedRemoteJobs()
        Public Event POSSSHRemoteExecution(ByVal FileStatus As String)
        Public Event POSCompleteSuccess()
        Public Event POSCompleteError()
#End Region

        ''' <summary>
        ''' Collection of StoreUpdatesBO objects to store the POS Push configuration for each store.
        ''' </summary>
        ''' <remarks></remarks>
        Private _storeUpdates As Hashtable = New Hashtable

        ''' <summary>
        ''' The date should be the same for all POS Push and Scale Push stored procedure calls to keep the
        ''' process consistent.  This is important if the job starts before midnight and completes after midnight.
        ''' </summary>
        ''' <remarks></remarks>
        Private _jobRunDate As Date = Now

        Private _hasTriggeredAlert As Boolean = False

        Dim WithEvents scalePush As ScalePushJob

#Region "Property Access Methods"

        Public Property JobRunDate() As Date
            Get
                Return _jobRunDate
            End Get
            Set(ByVal value As Date)
                _jobRunDate = value
            End Set
        End Property

#End Region

        ''' <summary>
        ''' ShouldStopPageAlerts --return false if alerts needs to be send , otherwise True
        ''' </summary>
        ''' <param name="sToEmailAddress"></param>
        ''' <returns></returns>
        Private Function ShouldStopPageAlerts(ByVal sToEmailAddress As String) As Boolean

            Dim aRegionalAddress() As String
            Dim sRegionalEmailAddresses As String = String.Empty
            '' comma sperated list of regional email addresses
            sRegionalEmailAddresses = "FL.Data.Team@wholefoods.com,MA.IRMASupport@wholefoods.com,MW.IRMA.Support@wholefoods.com,na.irma.errors@wholefoods.com,nc.irma.support@wholefoods.com,NE.IRMA.Errors@wholefoods.com,PN.Data.Team@wholefoods.com,RMIRMAPOSPush@wholefoods.com,SOPOSPushAlerts@wholefoods.com,sp.irma.support@wholefoods.com,swirmasupport@wholefoods.com,uk.irma.support@wholefoods.com"
            If Not String.IsNullOrEmpty(sRegionalEmailAddresses) Then
                aRegionalAddress = Split(sRegionalEmailAddresses, ",")
                ' see if sToEmailAddress exists in list of regional email adddresses --if NO set push alert = false and return
                If (Array.IndexOf(aRegionalAddress, sToEmailAddress) >= 0) Then
                    Return False
                End If
            End If

            Return True
        End Function
        ''' <summary>
        ''' Kicks off the POS Push Job, processing the POS changes for each store defined in the
        ''' StorePOSConfig table.
        ''' </summary>
        ''' <returns>True if it executes successfully; False otherwise</returns>
        ''' <remarks></remarks>
        Protected Overrides Function RunProcess() As Boolean
            Logger.LogDebug("RunProcess entry", Me.GetType())

            _hasTriggeredAlert = False

            Dim posSuccess As Boolean = False
            Dim sRegion As String = String.Empty
            Dim sToEmailAddress As String = String.Empty
            Dim sFromEmailAddress As String = String.Empty
            Dim sDefaultEmailAddress As String = String.Empty
            Dim sSMTP As String = String.Empty
            Dim sEnvironment As String = String.Empty
            Dim sTime_Zone_Offset As String = String.Empty

            Dim processMonitor As New ProcessMonitorDAO

            Dim stopAlerts As Boolean = False

            If Environment.GetCommandLineArgs.Length > 1 Then
                sRegion = Environment.GetCommandLineArgs.GetValue(1).ToString
                sToEmailAddress = Environment.GetCommandLineArgs.GetValue(3).ToString
                ' pbi 19620
                If Not String.IsNullOrEmpty(sToEmailAddress) Then
                    stopAlerts = ShouldStopPageAlerts(sToEmailAddress)
                End If
            End If

            sDefaultEmailAddress = ConfigurationServices.AppSettings("PosPushNotificationEmailRecipients")

            If Not String.IsNullOrEmpty(sDefaultEmailAddress) Then
                If Not String.IsNullOrEmpty(sToEmailAddress) Then
                    sToEmailAddress = sToEmailAddress & ";" & sDefaultEmailAddress
                Else
                    sToEmailAddress = sDefaultEmailAddress
                End If
            End If

            Try
                sTime_Zone_Offset = ConfigurationServices.AppSettings("Time_Zone_Offset").ToString
            Catch ex As Exception
                sTime_Zone_Offset = "0"
            Finally
                If sTime_Zone_Offset = String.Empty Then
                    sTime_Zone_Offset = "0"
                End If
            End Try

            ' Delete all entries in the IConPOSPushStaging table
            Try
                POSWriterDAO.DeleteIConPOSPushStaging()
            Catch ex As Exception
                Logger.LogError("DeleteIConPOSPushStaging Error: " & ex.Message, Me.GetType())
                Throw
            End Try

            ' run SCALE push process -- not all regions will use scale hosting, but if Scale writers are setup, the data will be pushed
            scalePush = New ScalePushJob()
            ' set the job run date to be same for scale & pos push
            scalePush.JobRunDate = DateAdd(DateInterval.Hour, CInt(sTime_Zone_Offset), _jobRunDate)
            'Pbi 19620 --set stop alert property of the base class
            scalePush.StopAlerts = stopAlerts
            Me.StopAlerts = stopAlerts
            scalePush.Main()

            'only run POS process if Scale push was successful
            If scalePush.JobExecutionStatus Then
                'run POS push process with the same start date as the scale push process
                posSuccess = ProcessPOSData(DateAdd(DateInterval.Hour, CInt(sTime_Zone_Offset), _jobRunDate))
            Else
                ' Copy the scale error messages to this job so they can appear on the UI
                _jobExecutionMessage = ScheduledJobBO.GetJobCompletionStatusForUI(CType(scalePush, ScheduledJob))
            End If

            Try
                sSMTP = ConfigurationServices.AppSettings("SMTPHost").ToString
            Catch ex As Exception
                sSMTP = "smtp.wholefoods.com"
            End Try

            Try
                sEnvironment = ConfigurationServices.AppSettings("environment").ToString
            Catch ex As Exception
                sEnvironment = "NOENVIRONMENT"
            End Try

            If sRegion = String.Empty Then
                sFromEmailAddress = "POSPush." & sEnvironment & "@wholefoods.com"
            Else
                sFromEmailAddress = sRegion & ".POSPush." & sEnvironment & "@wholefoods.com"
            End If

            If sToEmailAddress <> String.Empty Then
                If posSuccess Then
                    SendMail(sToEmailAddress, sFromEmailAddress, "POS Push Success!", "The POS Push completed successfully!", sSMTP)
                Else
                    SendMail(sToEmailAddress, sFromEmailAddress, "POS Push Failure", _jobExecutionMessage, sSMTP)
                End If
            End If

            Logger.LogDebug("RunProcess exit: " & posSuccess.ToString(), Me.GetType())

            Return posSuccess
        End Function

        ''' <summary>
        ''' creates POS writer files;  returns true or false based on success of run
        ''' this method is private because the POS writer is dependent on the SCALE writer.  it must be executed via the Main method.
        ''' </summary>
        ''' <returns>true or false based on success</returns>
        ''' <remarks></remarks>
        Private Function ProcessPOSData(ByVal startDate As Date) As Boolean
            Dim success As Boolean = False
            Dim processorWriteError As Boolean = False
            Dim bUseBizTalk As Boolean = CType(ConfigurationServices.AppSettings("BTUseForPush").ToString, Boolean)
            Dim processMonitor As New ProcessMonitorDAO
            Dim currProcess As System.Diagnostics.Process = System.Diagnostics.Process.GetCurrentProcess()
            Dim posPushjobMainTimer As Stopwatch
            Dim posPushJobTimer As Stopwatch
            Dim stepTimeInSeconds As Long = 0

            posPushJobTimer = Stopwatch.StartNew()
            posPushjobMainTimer = Stopwatch.StartNew()


            Try
                RaiseEvent POSPushStarted()

                ' Initialize the storeUpdates hashtable, adding an entry for each store
                _storeUpdates = StorePOSConfigDAO.GetStoreConfigurations(Constants.FileWriterType_POS)

                posPushJobTimer.Stop()
                stepTimeInSeconds = CLng((posPushJobTimer.ElapsedMilliseconds / 1000L))
                Console.WriteLine("Time taken for Step GetStoreConfigurations in POS Push: " + stepTimeInSeconds.ToString())
                Logger.LogInfo("Time taken for Step GetStoreConfigurations in POS Push: " + stepTimeInSeconds.ToString(), Me.GetType())

                RaiseEvent POSReadStoreConfigurationData(_storeUpdates.Count)

                ' Initialize a change processor for each type of change
                Dim batchDataForStoresStoresProcessor As New POSBatchDataForStoresProcessor(startDate)
                Dim taxFlagsDataProcessor As New POSTaxFlagsProcessor(startDate)
                Dim vendorIdAddProcessor As New POSVendorIdAddsProcessor(startDate)
                Dim itemDeleteProcessor As New POSItemDeletesProcessor(startDate)
                Dim itemChangeProcessor As New POSItemDataChangeProcessor(startDate)
                Dim itemIdDeleteProcessor As New POSItemIdDeletesProcessor(startDate)
                Dim itemIdAddProcessor As New POSItemIdAddsProcessor(startDate)
                Dim promoOfferProcessor As New POSPromoOfferProcessor(startDate)
                Dim itemRefreshProcessor As New POSItemRefreshProcessor(startDate)

                ' Grab the details for each change and add them to the POS Push file for the associated store
                Dim processorDescription As String = "Not Defined" ' recorded for error handling purposes
                Dim currentProcessor As POSProcessor = Nothing ' recorded for error handling purposes

                Try
                    ' Add the information for the stores in the batches
                    processMonitor.UpdateProcessMonitor("POSPushJob", "RUNNING", Now.ToShortTimeString & " Adding the Stores present in batches", "The job is running as Process ID " & currProcess.Id.ToString, False)
                    processorDescription = "Stores in Batches Adds"
                    currentProcessor = batchDataForStoresStoresProcessor

                    posPushJobTimer = Stopwatch.StartNew()
                    batchDataForStoresStoresProcessor.RetrieveChangeRecordsFromIRMA(_storeUpdates, False)

                    posPushJobTimer.Stop()
                    stepTimeInSeconds = CLng((posPushJobTimer.ElapsedMilliseconds / 1000L))
                    Console.WriteLine("Time taken for processing Stores in Batches Adds in POS Push: " + stepTimeInSeconds.ToString())
                    Logger.LogInfo("Time taken for processing  Stores in Batches in POS Push: " + stepTimeInSeconds.ToString(), Me.GetType())
                    RaiseEvent POSReadStoreBatchesAdds()

                    ' Add the data for the Tax Flags needed in POS Push
                    processMonitor.UpdateProcessMonitor("POSPushJob", "RUNNING", Now.ToShortTimeString & " Adding the Tax flag data", "The job is running as Process ID " & currProcess.Id.ToString, False)
                    processorDescription = "Adding Tax Flags Data"
                    currentProcessor = taxFlagsDataProcessor

                    posPushJobTimer = Stopwatch.StartNew()
                    taxFlagsDataProcessor.RetrieveChangeRecordsFromIRMA(_storeUpdates, False)

                    posPushJobTimer.Stop()
                    stepTimeInSeconds = CLng((posPushJobTimer.ElapsedMilliseconds / 1000L))
                    Console.WriteLine("Time taken for processing Tax Flags data in POS Push: " + stepTimeInSeconds.ToString())
                    Logger.LogInfo("Time taken for processing Tax Flags data in POS Push: " + stepTimeInSeconds.ToString(), Me.GetType())
                    RaiseEvent POSReadTaxflagData()

                    ' Send vendor adds first because other types of changes often depend on the vendors.
                    processMonitor.UpdateProcessMonitor("POSPushJob", "RUNNING", Now.ToShortTimeString & " Processing Vendor ID Adds", "The job is running as Process ID " & currProcess.Id.ToString, False)
                    processorDescription = "Vendor ID Adds"
                    currentProcessor = vendorIdAddProcessor

                    posPushJobTimer = Stopwatch.StartNew()
                    vendorIdAddProcessor.RetrieveChangeRecordsFromIRMA(_storeUpdates, False)

                    posPushJobTimer.Stop()
                    stepTimeInSeconds = CLng((posPushJobTimer.ElapsedMilliseconds / 1000L))
                    Console.WriteLine("Time taken for processing vendor ID Adds in POS Push: " + stepTimeInSeconds.ToString())
                    Logger.LogInfo("Time taken for processing vendor ID Adds in POS Push: " + stepTimeInSeconds.ToString(), Me.GetType())
                    RaiseEvent POSReadVendorAdds()

                    processMonitor.UpdateProcessMonitor("POSPushJob", "RUNNING", Now.ToShortTimeString & " Processing Price Changes", "The job is running as Process ID " & currProcess.Id.ToString, False)
                    processorDescription = "Item/Price Changes"
                    currentProcessor = itemChangeProcessor

                    posPushJobTimer = Stopwatch.StartNew()
                    itemChangeProcessor.RetrieveChangeRecordsFromIRMA(_storeUpdates, False)

                    posPushJobTimer.Stop()
                    stepTimeInSeconds = CLng((posPushJobTimer.ElapsedMilliseconds / 1000L))
                    Console.WriteLine("Time taken for processing item price changes in POS Push: " + stepTimeInSeconds.ToString())
                    Logger.LogInfo("Time taken for processing item price changes in POS Push: " + stepTimeInSeconds.ToString(), Me.GetType())

                    RaiseEvent POSReadItemPriceChanges()

                    processMonitor.UpdateProcessMonitor("POSPushJob", "RUNNING", Now.ToShortTimeString & " Processing Price Refreshes", "The job is running as Process ID " & currProcess.Id.ToString, False)
                    processorDescription = "Item Refreshes"
                    currentProcessor = itemRefreshProcessor

                    posPushJobTimer = Stopwatch.StartNew()
                    itemRefreshProcessor.RetrieveChangeRecordsFromIRMA(_storeUpdates, False)

                    posPushJobTimer.Stop()
                    stepTimeInSeconds = CLng((posPushJobTimer.ElapsedMilliseconds / 1000L))
                    Console.WriteLine("Time taken for processing Price Refreshes in POS Push: " + stepTimeInSeconds.ToString())
                    Logger.LogInfo("Time taken for processing Price Refreshes in POS Push: " + stepTimeInSeconds.ToString(), Me.GetType())

                    RaiseEvent POSReadItemRefreshes()

                    processMonitor.UpdateProcessMonitor("POSPushJob", "RUNNING", Now.ToShortTimeString & " Processing Item ID Adds", "The job is running as Process ID " & currProcess.Id.ToString, False)
                    processorDescription = "Item ID Adds"
                    currentProcessor = itemIdAddProcessor

                    posPushJobTimer = Stopwatch.StartNew()
                    itemIdAddProcessor.RetrieveChangeRecordsFromIRMA(_storeUpdates, False)

                    posPushJobTimer.Stop()
                    stepTimeInSeconds = CLng((posPushJobTimer.ElapsedMilliseconds / 1000L))
                    Console.WriteLine("Time taken for processing Item ID adds in POS Push: " + stepTimeInSeconds.ToString())
                    Logger.LogInfo("Time taken for processing Item ID adds in POS Push: " + stepTimeInSeconds.ToString(), Me.GetType())

                    RaiseEvent POSReadItemIdAdds()

                    processMonitor.UpdateProcessMonitor("POSPushJob", "RUNNING", Now.ToShortTimeString & " Processing Promo Offers", "The job is running as Process ID " & currProcess.Id.ToString, False)
                    processorDescription = "Promo Offers"
                    currentProcessor = promoOfferProcessor

                    posPushJobTimer = Stopwatch.StartNew()
                    promoOfferProcessor.RetrieveChangeRecordsFromIRMA(_storeUpdates, False)

                    posPushJobTimer.Stop()
                    stepTimeInSeconds = CLng((posPushJobTimer.ElapsedMilliseconds / 1000L))
                    Console.WriteLine("Time taken for processing Promo Offers in POS Push: " + stepTimeInSeconds.ToString())
                    Logger.LogInfo("Time taken for processing Promo Offers in POS Push: " + stepTimeInSeconds.ToString(), Me.GetType())

                    RaiseEvent POSReadPromoOffers()

                    ' Include the item deletes after all other changes.  This is done so that de-auth records
                    ' don't conflict with item change or item add records if they also exist.
                    processMonitor.UpdateProcessMonitor("POSPushJob", "RUNNING", Now.ToShortTimeString & " Processing Item ID Deletes", "The job is running as Process ID " & currProcess.Id.ToString, False)
                    processorDescription = "Item ID Deletes"
                    currentProcessor = itemIdDeleteProcessor

                    posPushJobTimer = Stopwatch.StartNew
                    itemIdDeleteProcessor.RetrieveChangeRecordsFromIRMA(_storeUpdates, False)

                    posPushJobTimer.Stop()
                    stepTimeInSeconds = CLng((posPushJobTimer.ElapsedMilliseconds / 1000L))
                    Console.WriteLine("Time taken for processing item id deletes in POS Push: " + stepTimeInSeconds.ToString())
                    Logger.LogInfo("Time taken for processing item id in deletes in POS Push: " + stepTimeInSeconds.ToString(), Me.GetType())

                    RaiseEvent POSReadItemIdDeletes()

                    processMonitor.UpdateProcessMonitor("POSPushJob", "RUNNING", Now.ToShortTimeString & " Processing Item Deletes", "The job is running as Process ID " & currProcess.Id.ToString, False)
                    processorDescription = "Item Deletes"
                    currentProcessor = itemDeleteProcessor

                    posPushJobTimer = Stopwatch.StartNew()
                    itemDeleteProcessor.RetrieveChangeRecordsFromIRMA(_storeUpdates, False)

                    posPushJobTimer.Stop()
                    stepTimeInSeconds = CLng((posPushJobTimer.ElapsedMilliseconds / 1000L))
                    Console.WriteLine("Time taken for processing Item Deletes in POS Push: " + stepTimeInSeconds.ToString())
                    Logger.LogInfo("Time taken for processing Item Deletes in POS Push: " + stepTimeInSeconds.ToString(), Me.GetType())

                    RaiseEvent POSReadItemDeletes()
                Catch e As Exception
                    ' Send an error email with the details
                    Dim args(6) As String
                    args(0) = processorDescription
                    args(1) = currentProcessor.DebugStoreNo().ToString()
                    args(2) = currentProcessor.DebugSuccessLinesCount().ToString()
                    args(3) = currentProcessor.DebugErrorRowNum()
                    args(4) = currentProcessor.DebugErrorColNum()
                    args(5) = currentProcessor.DebugFieldId()
                    ErrorHandler.ProcessError(ErrorType.POSPush_ProcessorRetrievalException, args, SeverityLevel.Fatal, e)

                    ' Add additional error message information, if it exists, to the error message
                    Dim message As New StringBuilder
                    message.Append("POS Push error when parsing the data and adding it to the IRMA output file.  This is typically caused by a data error.")
                    message.Append(Environment.NewLine)
                    message.Append("Change Type Being Processed: ")
                    message.Append(args(0))
                    message.Append(Environment.NewLine)
                    message.Append("Current Store Being Processed:")
                    message.Append(args(1))
                    message.Append(Environment.NewLine)
                    message.Append("# Successful Lines Written to Store File: ")
                    message.Append(args(2))
                    message.Append(Environment.NewLine)
                    message.Append("Current Row # Being Processed:")
                    message.Append(args(3))
                    message.Append(Environment.NewLine)
                    message.Append("Current Column # Being Processed:")
                    message.Append(args(4))
                    message.Append(Environment.NewLine)
                    message.Append("Current Field ID Being Processed:")
                    message.Append(args(5))
                    message.Append(Environment.NewLine)

                    ' Set the flag so that another email is not generated when the exception is caught
                    ' further down in the code; Set the error messsage text for the UI client
                    processorWriteError = True
                    _jobExecutionMessage = message.ToString

                    Throw
                End Try

                ' Generate a POS Control file for each store
                Dim storeEnum As IDictionaryEnumerator = _storeUpdates.GetEnumerator
                Dim currentStore As StoreUpdatesBO
                Dim controlfileGenerationSuccess As Boolean = True

                While storeEnum.MoveNext
                    currentStore = CType(storeEnum.Value, StoreUpdatesBO)
                    If (currentStore.FileWriter.CreateControlFile(currentStore) = False) Then
                        controlfileGenerationSuccess = False
                    End If
                End While

                RaiseEvent POSGeneratedPOSControlFiles()

                ' Deliver the file to each of the stores
                Dim transfer As New TransferWriterFiles
                Dim transferSuccess As Boolean
                transfer.StopAlerts = StopAlerts
                If bUseBizTalk Then
                    processMonitor.UpdateProcessMonitor("POSPushJob", "RUNNING", Now.ToShortTimeString & " FTPing files to BizTalk", "The job is running as Process ID " & currProcess.Id.ToString, False)
                    transferSuccess = transfer.TransferStoreFilesBT(_storeUpdates)
                    _hasTriggeredAlert = Not transferSuccess
                Else
                    processMonitor.UpdateProcessMonitor("POSPushJob", "RUNNING", Now.ToShortTimeString & " FTPing files to the stores", "The job is running as Process ID " & currProcess.Id.ToString, False)
                    transferSuccess = transfer.TransferStoreFiles(_storeUpdates)
                    _hasTriggeredAlert = Not transferSuccess
                End If

                RaiseEvent POSTransferFiles(transfer.StoreList)

                'Execute remote process for each store
                Dim remoteExecutionSuccess As Boolean = True
                Dim filename As String = "ssh_remote_status.txt"

                If File.Exists(filename) Then
                    File.Delete(filename)
                End If

                If Not bUseBizTalk Then
                    storeEnum.Reset()

                    While storeEnum.MoveNext
                        processMonitor.UpdateProcessMonitor("POSPushJob", "RUNNING", Now.ToShortTimeString & " Executing the remote process on secure transfers", "The job is running as Process ID " & currProcess.Id.ToString, False)
                        currentStore = CType(storeEnum.Value, StoreUpdatesBO)

                        Try
                            posPushJobTimer = Stopwatch.StartNew()

                            If (currentStore.FTPInfo.IsSecureTransfer) Then
                                Logger.WriteLog(filename, "SSH Remote execution started on store [Store# <{0}>, IP = <{1}>, UserId = <{2}>, Password = <{3}>] ...",
                                currentStore.StoreNum,
                                currentStore.FTPInfo.IPAddress,
                                currentStore.FTPInfo.FTPUser,
                                currentStore.FTPInfo.FTPPassword)

                                If (currentStore.FileWriter.CallSSHRemoteJobProcess(currentStore, filename) = False) Then
                                    remoteExecutionSuccess = False
                                End If

                                RaiseEvent POSSSHRemoteExecution(currentStore.FileWriter.RemoteSSHStoreList)
                            Else
                                If (currentStore.FileWriter.CallRemoteJobProcess(currentStore) = False) Then
                                    remoteExecutionSuccess = False
                                End If

                                RaiseEvent POSSSHRemoteExecution(currentStore.FileWriter.RemoteSSHStoreList)
                            End If

                            posPushJobTimer.Stop()
                            stepTimeInSeconds = CLng((posPushJobTimer.ElapsedMilliseconds / 1000L))
                            Console.WriteLine("Time taken for processing secure transfers in POS Push: " + stepTimeInSeconds.ToString())
                            Logger.LogInfo("Time taken for processing secure transfers in POS Push: " + stepTimeInSeconds.ToString(), Me.GetType())
                        Catch ex As Exception
                            Dim errorMessage As String = String.Format("Failed during SSH remote execution for store: {0}.  Please verify FTP settings.", currentStore.StoreNum)
                            Logger.LogError(errorMessage, Me.GetType(), ex)
                            Throw New Exception(errorMessage, ex)
                        End Try
                    End While
                    RaiseEvent POSStartedRemoteJobs()
                End If

                Dim TagPush As New TagPriceBatchProcessor(0)
                Dim tagTransferSuccess As Boolean

                processMonitor.UpdateProcessMonitor("POSPushJob", "RUNNING", Now.ToShortTimeString & " Processing Electronic Shelf Tags", "The job is running as Process ID " & currProcess.Id.ToString, False)

                posPushJobTimer = Stopwatch.StartNew()
                tagTransferSuccess = TagPush.ProcessElectronicShelfTags()

                posPushJobTimer.Stop()
                stepTimeInSeconds = CLng((posPushJobTimer.ElapsedMilliseconds / 1000L))
                Console.WriteLine("Time taken for processing Electronic Shelf Tags in POS Push: " + stepTimeInSeconds.ToString())
                Logger.LogInfo("Time taken for processing Electronic Shelf Tags in POS Push: " + stepTimeInSeconds.ToString(), Me.GetType())

                ' Populate IconPOSPushStaging table with batch data
                processMonitor.UpdateProcessMonitor("POSPushJob", "RUNNING", Now.ToShortTimeString &
                    " Processing Populate IconPOSPushStaging table with batch data", "The job is running as Process ID " & currProcess.Id.ToString, False)
                Try
                    posPushJobTimer = Stopwatch.StartNew()
                    POSWriterDAO.PopulateIConPOSPushStagingWithBatchData(DateTime.Now)

                    posPushJobTimer.Stop()
                    stepTimeInSeconds = CLng((posPushJobTimer.ElapsedMilliseconds / 1000L))
                    Console.WriteLine("Time taken for populating IconPOSStaging table with Batch in POS Push: " + stepTimeInSeconds.ToString())
                    Logger.LogInfo("Time taken for populating IconPOSStaging table with Batch in POS Push: " + stepTimeInSeconds.ToString(), Me.GetType())
                Catch ex As Exception
                    Logger.LogError("PopulateIConPOSPushStagingWithBatchData failed: " & ex.ToString(), Me.GetType())
                    Throw
                End Try

                ' Populate IconPOSPushStaging table with non-batch data
                processMonitor.UpdateProcessMonitor("POSPushJob", "RUNNING", Now.ToShortTimeString &
                    " Processing Populate IconPOSPushStaging table with non-batch data", "The job is running as Process ID " & currProcess.Id.ToString, False)
                Try
                    posPushJobTimer = Stopwatch.StartNew()
                    POSWriterDAO.PopulateIConPOSPushStagingWithNonBatchData()

                    posPushJobTimer.Stop()
                    stepTimeInSeconds = CLng((posPushJobTimer.ElapsedMilliseconds / 1000L))
                    Console.WriteLine("Time taken for populating IconPOSStaging table with Non-Batch in POS Push: " + stepTimeInSeconds.ToString())
                    Logger.LogInfo("Time taken for populating IconPOSStaging table with Non-Batch in POS Push: " + stepTimeInSeconds.ToString(), Me.GetType())
                Catch ex As Exception
                    Logger.LogError("PopulateIConPOSPushStagingWithNonBatchData failed: " & ex.ToString(), Me.GetType())
                    Throw
                End Try

                ' Populate IconPOSPushPublish table
                ' Parameter value = 1, apply changes to IRMA for R10 stores.
                processMonitor.UpdateProcessMonitor("POSPushJob", "RUNNING", Now.ToShortTimeString & " Processing Populate IconPOSPushPublish table", "The job is running as Process ID " & currProcess.Id.ToString, False)
                Try
                    posPushJobTimer = Stopwatch.StartNew()
                    POSWriterDAO.PopulateIConPOSPushPublish(1)

                    posPushJobTimer.Stop()
                    stepTimeInSeconds = CLng((posPushJobTimer.ElapsedMilliseconds / 1000L))
                    Console.WriteLine("Time taken for populating IconPOSPUblish table in POS Push: " + stepTimeInSeconds.ToString())
                    Logger.LogInfo("Time taken for populating IconPOSPUblish table in POS Push: " + stepTimeInSeconds.ToString(), Me.GetType())
                Catch ex As Exception
                    Logger.LogError("PopulateIConPOSPushPublish failed: " & ex.ToString(), Me.GetType())
                    Throw
                End Try

                If controlfileGenerationSuccess And transferSuccess And tagTransferSuccess And remoteExecutionSuccess Then
                    ' Update the IRMA database to indicate that the changes were applied to the stores
                    processMonitor.UpdateProcessMonitor("POSPushJob", "RUNNING", Now.ToShortTimeString & " Applying Vendor ID Add batches in IRMA", "The job is running as Process ID " & currProcess.Id.ToString, False)

                    posPushJobTimer = Stopwatch.StartNew()
                    vendorIdAddProcessor.ApplyChangesInIRMA(_storeUpdates)

                    posPushJobTimer.Stop()
                    stepTimeInSeconds = CLng((posPushJobTimer.ElapsedMilliseconds / 1000L))
                    Console.WriteLine("Time taken for Applying Vendor ID Add Batches in POS Push: " + stepTimeInSeconds.ToString())
                    Logger.LogInfo("Time taken for Applying Vendor ID Add Batches in POS Push: " + stepTimeInSeconds.ToString(), Me.GetType())

                    processMonitor.UpdateProcessMonitor("POSPushJob", "RUNNING", Now.ToShortTimeString & " Applying Item Change batches in IRMA", "The job is running as Process ID " & currProcess.Id.ToString, False)

                    posPushJobTimer = Stopwatch.StartNew()
                    itemChangeProcessor.ApplyChangesInIRMA(_storeUpdates)
                    posPushJobTimer.Stop()
                    stepTimeInSeconds = CLng((posPushJobTimer.ElapsedMilliseconds / 1000L))
                    Console.WriteLine("Time taken for Applying Item Change Batches in POS Push: " + stepTimeInSeconds.ToString())
                    Logger.LogInfo("Time taken for Applying Item Change Batches in POS Push: " + stepTimeInSeconds.ToString(), Me.GetType())

                    processMonitor.UpdateProcessMonitor("POSPushJob", "RUNNING", Now.ToShortTimeString & " Applying Item ID Add batches in IRMA", "The job is running as Process ID " & currProcess.Id.ToString, False)
                    posPushJobTimer = Stopwatch.StartNew()
                    itemIdAddProcessor.ApplyChangesInIRMA(_storeUpdates)
                    posPushJobTimer.Stop()
                    stepTimeInSeconds = CLng((posPushJobTimer.ElapsedMilliseconds / 1000L))
                    Console.WriteLine("Time taken for Applying Item ID Add Batches in POS Push: " + stepTimeInSeconds.ToString())
                    Logger.LogInfo("Time taken for Applying Item ID Add Batches in POS Push: " + stepTimeInSeconds.ToString(), Me.GetType())

                    processMonitor.UpdateProcessMonitor("POSPushJob", "RUNNING", Now.ToShortTimeString & " Applying Promo Offer batches in IRMA", "The job is running as Process ID " & currProcess.Id.ToString, False)
                    posPushJobTimer = Stopwatch.StartNew()
                    promoOfferProcessor.ApplyChangesInIRMA(_storeUpdates)
                    posPushJobTimer.Stop()
                    stepTimeInSeconds = CLng((posPushJobTimer.ElapsedMilliseconds / 1000L))
                    Console.WriteLine("Time taken for Applying Promo Offer Batches Batches in POS Push: " + stepTimeInSeconds.ToString())
                    Logger.LogInfo("Time taken for Applying Promo Offer Batches Batches in POS Push: " + stepTimeInSeconds.ToString(), Me.GetType())

                    processMonitor.UpdateProcessMonitor("POSPushJob", "RUNNING", Now.ToShortTimeString & " Applying Item ID Delete batches in IRMA", "The job is running as Process ID " & currProcess.Id.ToString, False)
                    posPushJobTimer = Stopwatch.StartNew()
                    itemIdDeleteProcessor.ApplyChangesInIRMA(_storeUpdates)
                    posPushJobTimer.Stop()
                    stepTimeInSeconds = CLng((posPushJobTimer.ElapsedMilliseconds / 1000L))
                    Console.WriteLine("Time taken for Applying Item ID Delete Batches in POS Push: " + stepTimeInSeconds.ToString())
                    Logger.LogInfo("Time taken for Applying Item ID Delete Batches in POS Push: " + stepTimeInSeconds.ToString(), Me.GetType())

                    processMonitor.UpdateProcessMonitor("POSPushJob", "RUNNING", Now.ToShortTimeString & " Applying Item Delete batches in IRMA", "The job is running as Process ID " & currProcess.Id.ToString, False)
                    posPushJobTimer = Stopwatch.StartNew()
                    itemDeleteProcessor.ApplyChangesInIRMA(_storeUpdates)
                    posPushJobTimer.Stop()
                    stepTimeInSeconds = CLng((posPushJobTimer.ElapsedMilliseconds / 1000L))
                    Console.WriteLine("Time taken for Applying Item Delete Batches in POS Push: " + stepTimeInSeconds.ToString())
                    Logger.LogInfo("Time taken for Applying Item Delete Batches in POS Push: " + stepTimeInSeconds.ToString(), Me.GetType())

                    processMonitor.UpdateProcessMonitor("POSPushJob", "RUNNING", Now.ToShortTimeString & " Applying Item Refresh batches in IRMA", "The job is running as Process ID " & currProcess.Id.ToString, False)
                    posPushJobTimer = Stopwatch.StartNew
                    itemRefreshProcessor.ApplyChangesInIRMA(_storeUpdates)
                    posPushJobTimer.Stop()
                    stepTimeInSeconds = CLng((posPushJobTimer.ElapsedMilliseconds / 1000L))
                    Console.WriteLine("Time taken for Applying Item Refesh Batches in POS Push: " + stepTimeInSeconds.ToString())
                    Logger.LogInfo("Time taken for Applying Item Refesh Batches in POS Push: " + stepTimeInSeconds.ToString(), Me.GetType())

                    RaiseEvent POSApplyChangesToIRMA()

                    success = True

                    _jobExecutionMessage = "Scale/POS Push process completed successfully. Please see the log file ssh_remote_status.txt for any errors."
                Else
                    ' update the error message so the user knows what happened
                    If _jobExecutionMessage Is Nothing Then
                        _jobExecutionMessage = ""
                    End If

                    If (controlfileGenerationSuccess = False) Then
                        If _jobExecutionMessage IsNot Nothing AndAlso _jobExecutionMessage <> "" Then
                            _jobExecutionMessage = _jobExecutionMessage + Environment.NewLine
                        End If
                        _jobExecutionMessage = _jobExecutionMessage + "Generation of control file failed. "
                    End If

                    If (transferSuccess = False) Then
                        If _jobExecutionMessage IsNot Nothing AndAlso _jobExecutionMessage <> "" Then
                            _jobExecutionMessage = _jobExecutionMessage + Environment.NewLine
                        End If
                        _jobExecutionMessage = _jobExecutionMessage + "Transfer of POS Push files did not succeed. "
                    End If

                    If (tagTransferSuccess = False) Then
                        If _jobExecutionMessage IsNot Nothing AndAlso _jobExecutionMessage <> "" Then
                            _jobExecutionMessage = _jobExecutionMessage + Environment.NewLine
                        End If
                        _jobExecutionMessage = _jobExecutionMessage + "Transfer of Electronic Shelf Tag files did not succeed. "
                    End If

                    If (remoteExecutionSuccess = False) Then
                        If _jobExecutionMessage IsNot Nothing AndAlso _jobExecutionMessage <> "" Then
                            _jobExecutionMessage = _jobExecutionMessage + Environment.NewLine
                        End If
                        _jobExecutionMessage = _jobExecutionMessage + "Remote execution did not succeed. "
                    End If

                    If _jobExecutionMessage IsNot Nothing AndAlso _jobExecutionMessage <> "" Then
                        _jobExecutionMessage = _jobExecutionMessage + Environment.NewLine
                    End If
                    _jobExecutionMessage = _jobExecutionMessage + "Updates were not applied in IRMA."
                End If
            Catch ex As Exception
                Logger.LogError("Exception: ", Me.GetType(), ex)
                If Not processorWriteError Then
                    ErrorHandler.ProcessError(ErrorType.GeneralApplicationError, SeverityLevel.Fatal, ex)
                    Dim msg As New StringBuilder
                    msg.Append("Error during processing of POS Push job.")
                    msg.Append(Environment.NewLine)
                    msg.Append(ex.Message)
                    msg.Append(Environment.NewLine)
                    msg.Append(ex.StackTrace)
                    _jobExecutionMessage = msg.ToString()
                End If

                success = False

                _jobExecutionMessage = _jobExecutionMessage + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace
                'pbI 19620 --if push alert is false do not trigger pager alert
                If (Not _hasTriggeredAlert And Not StopAlerts) Then
                    PagerDutyUtility.TriggerPagerDutyAlert("IRMA POS Push", "POS Push Failure", ex.ToString())
                    _hasTriggeredAlert = True
                End If

            End Try

            If success Then
                ' If there are business units in the config key, publish the data to the publishing table, P_PriceBatchDenorm, from the denorm table.
                If Len(Trim(CStr(ConfigurationServices.AppSettings("BusinessUnits")))) > 1 Then
                    posPushJobTimer = Stopwatch.StartNew()
                    POSWriterDAO.PublishDenormTable()
                    posPushJobTimer.Stop()
                    stepTimeInSeconds = CLng((posPushJobTimer.ElapsedMilliseconds / 1000L))
                    Console.WriteLine("Time taken for Publishing data to the publishing table in POS Push: " + stepTimeInSeconds.ToString())

                End If

                processMonitor.UpdateProcessMonitor("POSPushJob", "RUNNING", Now.ToShortTimeString & " POS Push completed successfully!", "", False)
                RaiseEvent POSCompleteSuccess()
            Else
                processMonitor.UpdateProcessMonitor("POSPushJob", "RUNNING", Now.ToShortTimeString & " POS Push failed", "", False)
                RaiseEvent POSCompleteError()
            End If

            CheckForRecordsInStagingTable()
            CheckForBatchesInSentStatus(_jobRunDate)

            posPushjobMainTimer.Stop()
            stepTimeInSeconds = CLng((posPushjobMainTimer.ElapsedMilliseconds / 1000L))
            Console.WriteLine("Total Time taken for POS Push: " + stepTimeInSeconds.ToString())
            Logger.LogInfo("Total Time taken for POS Push: " + stepTimeInSeconds.ToString(), Me.GetType())
            Return success
        End Function

        Private Sub CheckForBatchesInSentStatus(jobRunDate As Date)
            Dim batchesInSentStatusCount As Integer = HealthCheckDAO.GetBatchesInSentStatus(jobRunDate)

            If CBool(ConfigurationServices.AppSettings("EnablePagerDutyAlertsForBatchesInSent")) And batchesInSentStatusCount > 0 Then
                Dim errorMessage As String = String.Format("The push has completed, but batches remain In Sent status With a StartDate Of {0}.", jobRunDate.ToString())

                Logger.LogError(errorMessage, Me.GetType())

                If (Not _hasTriggeredAlert And Not StopAlerts) Then
                    PagerDutyUtility.TriggerPagerDutyAlert("IRMA POS Push", "POS Push Failure", errorMessage)
                    _hasTriggeredAlert = True
                End If
            End If
        End Sub

        Private Sub CheckForRecordsInStagingTable()
            Dim iconStagingTableCount As Integer = HealthCheckDAO.GetIconStagingTableCount()

            If iconStagingTableCount > 0 Then
                Dim errorMessage As String = "The push has completed, but records remain in the IConPOSPushStaging table."

                Logger.LogError(errorMessage, Me.GetType())

                If (Not _hasTriggeredAlert And Not StopAlerts) Then
                    PagerDutyUtility.TriggerPagerDutyAlert("IRMA POS Push", "POS Push Failure", errorMessage)
                    _hasTriggeredAlert = True
                End If
            End If
        End Sub

        Public Sub SendMail(ByVal sRecipient As String, ByVal sFromAddress As String, ByVal sSubject As String, ByVal sMessage As String, ByVal sSMTP As String)
            Dim smtp As New SMTP(sSMTP)

            Try
                smtp.send(sMessage, sRecipient, Nothing, sFromAddress, sSubject)
            Catch ex As Exception
                Logger.LogDebug("SendMail failed:  " & ex.ToString(), Me.GetType())
            End Try
        End Sub

        Private Sub scalePush_ScaleCompleteError() Handles scalePush.ScaleCompleteError
            RaiseEvent ScaleCompleteError()
        End Sub

        Private Sub scalePush_ScaleCompleteSuccess() Handles scalePush.ScaleCompleteSuccess
            RaiseEvent ScaleCompleteSuccess()
        End Sub

        Private Sub scalePush_ScaleCorpTempQueueCleared() Handles scalePush.ScaleCorpTempQueueCleared
            RaiseEvent ScaleCorpTempQueueCleared()
        End Sub

        Private Sub scalePush_ScalePushStarted(ByVal IsRegional As Boolean) Handles scalePush.ScalePushStarted
            RaiseEvent ScalePushStarted(IsRegional)
        End Sub

        Private Sub scalePush_ScaleReadItemIdAdds() Handles scalePush.ScaleReadItemIdAdds
            RaiseEvent ScaleReadItemIdAdds()
        End Sub

        Private Sub scalePush_ScaleReadItemIdDeletes() Handles scalePush.ScaleReadItemIdDeletes
            RaiseEvent ScaleReadItemIdDeletes()
        End Sub

        Private Sub scalePush_ScaleReadItemPriceChanges() Handles scalePush.ScaleReadItemPriceChanges
            RaiseEvent ScaleReadItemPriceChanges()
        End Sub

        Private Sub scalePush_ScaleReadStoreConfigurationData(ByVal NumStores As Integer) Handles scalePush.ScaleReadStoreConfigurationData
            RaiseEvent ScaleReadStoreConfigurationData(NumStores)
        End Sub

        Private Sub scalePush_ScaleReadZoneDeletes() Handles scalePush.ScaleReadZoneDeletes
            RaiseEvent ScaleReadZoneDeletes()
        End Sub

        Private Sub scalePush_ScaleReadZonePriceChanges() Handles scalePush.ScaleReadZonePriceChanges
            RaiseEvent ScaleReadZonePriceChanges()
        End Sub

        Private Sub scalePush_ScaleTransferFiles(ByVal FileStatus As String) Handles scalePush.ScaleTransferFiles
            RaiseEvent ScaleTransferFiles(FileStatus)
        End Sub
    End Class

End Namespace

