Imports System.Text
Imports WholeFoods.IRMA.Replenishment.Common.Writers
Imports WholeFoods.IRMA.Replenishment.POSPush.BusinessLogic
Imports WholeFoods.IRMA.Replenishment.ScalePush.Controller
Imports WholeFoods.IRMA.Replenishment.ScalePush.DataAccess
Imports WholeFoods.Utility

Namespace WholeFoods.IRMA.Replenishment.Jobs

    ''' <summary>
    ''' Job that performs the SCALE Push process.
    ''' </summary>
    ''' <remarks>
    ''' ASSUMPTION: this is NOT designed to be multi-threaded.  If process must become multi-threaded then 
    ''' file management in StoreUpdatesBO.vb should no longer remove file if temp file exists upon
    ''' process startup.
    ''' </remarks>
    Public Class ScalePushJob
        Inherits ScheduledJob

#Region "Events raised by this job"
        ' These events are raised during key steps of the process so the UI can let the user know
        ' where in the process things are.
        Public Event ScalePushStarted(ByVal IsRegional As Boolean)
        Public Event ScaleReadStoreConfigurationData(ByVal NumStores As Integer)
        Public Event ScaleReadItemIdAdds()
        Public Event ScaleReadItemIdDeletes()
        Public Event ScaleReadItemPriceChanges()
        Public Event ScaleReadNutriFacts()
        Public Event ScaleReadExtraText()
        Public Event ScaleReadZoneDeletes()
        Public Event ScaleReadZonePriceChanges()
        Public Event ScaleTransferFiles(ByVal FileStatus As String)
        Public Event ScaleCorpTempQueueCleared()
        Public Event ScaleNutriFactsTempQueueCleared()
        Public Event ScaleExtraTextTempQueueCleared()
        Public Event ScaleAuthorizationsReset()
        Public Event ScaleDeAuthorizationsReset()
        Public Event ScaleCompleteSuccess()
        Public Event ScaleCompleteError()
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
        ''' Kicks off the SCALE Push Job, processing the Scale changes for each region/store defined in the
        ''' StoreScaleConfig table.
        ''' </summary>
        ''' <returns>True if it executes successfully; False otherwise</returns>
        ''' <remarks></remarks>
        Protected Overrides Function RunProcess() As Boolean
            Logger.LogDebug("RunProcess entry", Me.GetType())

            _hasTriggeredAlert = False

            Dim jobSuccess As Boolean = False

            jobSuccess = ProcessScaleData(_jobRunDate)

            Logger.LogDebug("RunProcess exit: " & jobSuccess.ToString(), Me.GetType())

            Return jobSuccess
        End Function

        ''' <summary>
        ''' creates SCALE writer files;  returns true or false based on success of run
        ''' this method is public because the SCALE writer can be fired off independently of the POS writer
        ''' </summary>
        ''' <returns>true or false based on success</returns>
        ''' <remarks></remarks>
        Public Function ProcessScaleData(ByVal startDate As Date) As Boolean
            Dim success As Boolean = False
            Dim itemIdAddProcessor As CorpScaleItemIdAddsProcessor = Nothing
            Dim itemIdDeleteProcessor As CorpScaleItemIdDeletesProcessor = Nothing
            Dim itemChangeProcessor As CorpScaleItemDataChangeProcessor = Nothing
            Dim nutriFactProcessor As NutriFactsProcessor = Nothing
            Dim extraTextProcessor As ScaleExtraTextProcessor = Nothing
            Dim zoneItemDeleteProcessor As ZoneScaleItemDeleteProcessor = Nothing
            Dim zonePriceChangeProcessor As ZoneScalePriceChangeProcessor = Nothing
            Dim bUseBiztalk As Boolean = CType(ConfigurationServices.AppSettings("BTUseForPush").ToString, Boolean)
            Dim processMonitor As New ProcessMonitorDAO
            Dim currProcess As System.Diagnostics.Process = System.Diagnostics.Process.GetCurrentProcess()
            Dim scalePushjobMainTimer As Stopwatch
            Dim scalePushJobTimer As Stopwatch
            Dim stepTimeInSeconds As Long = 0

            scalePushJobTimer = Stopwatch.StartNew()
            scalePushjobMainTimer = Stopwatch.StartNew()


            Try
                'foer testing only --next line will throw an exception which will cause pager alert to raqise event
                '''''''''Throw New System.Exception("An exception has occurred.")

                processMonitor.UpdateProcessMonitor("ScalePushJob", "RUNNING", Now.ToShortTimeString & " Scale Push starting", "The job is running as Process ID " & currProcess.Id.ToString, False)

                RaiseEvent ScalePushStarted(IsRegional:=False)

                ' get STORE configurations
                ' Initialize the storeUpdates hashtable, adding an entry for each store

                _storeUpdates = StoreScaleConfigDAO.GetStoreConfigurations(Constants.FileWriterType_SCALE, False)
                scalePushJobTimer.Stop()
                stepTimeInSeconds = CLng((scalePushJobTimer.ElapsedMilliseconds / 1000L))
                Console.WriteLine("Time taken for Step GetStoreConfigurations: " + stepTimeInSeconds.ToString())

                RaiseEvent ScaleReadStoreConfigurationData(_storeUpdates.Count)

                ' check if any stores require SCALE hosting
                If _storeUpdates.Keys.Count > 0 Then
                    ' Initialize a change processor for each type of change
                    itemIdAddProcessor = New CorpScaleItemIdAddsProcessor(startDate)
                    itemIdDeleteProcessor = New CorpScaleItemIdDeletesProcessor(startDate)
                    itemChangeProcessor = New CorpScaleItemDataChangeProcessor(startDate)
                    nutriFactProcessor = New NutriFactsProcessor(startDate)
                    extraTextProcessor = New ScaleExtraTextProcessor(startDate)

                    ' ZONE changes
                    zoneItemDeleteProcessor = New ZoneScaleItemDeleteProcessor(startDate)
                    zonePriceChangeProcessor = New ZoneScalePriceChangeProcessor(startDate)

                    ' Grab the details for each change and add them to the POS Push file for the associated store
                    processMonitor.UpdateProcessMonitor("ScalePushJob", "RUNNING", Now.ToShortTimeString & " Processing Item ID Adds", "The job is running as Process ID " & currProcess.Id.ToString, False)

                    scalePushJobTimer = Stopwatch.StartNew()
                    itemIdAddProcessor.RetrieveChangeRecordsFromIRMA(_storeUpdates, False)
                    scalePushJobTimer.Stop()
                    stepTimeInSeconds = CLng((scalePushJobTimer.ElapsedMilliseconds / 1000L))
                    Console.WriteLine("Time taken for Processing Item ID Adds: " + stepTimeInSeconds.ToString())
                    Logger.LogInfo("Time taken for Processing Item ID Adds: " + stepTimeInSeconds.ToString(), Me.GetType())

                    RaiseEvent ScaleReadItemIdAdds()

                    processMonitor.UpdateProcessMonitor("ScalePushJob", "RUNNING", Now.ToShortTimeString & " Processing Item ID Deletes", "The job is running as Process ID " & currProcess.Id.ToString, False)

                    scalePushJobTimer = Stopwatch.StartNew()
                    itemIdDeleteProcessor.RetrieveChangeRecordsFromIRMA(_storeUpdates, False)
                    scalePushJobTimer.Stop()
                    stepTimeInSeconds = CLng((scalePushJobTimer.ElapsedMilliseconds / 1000L))
                    Console.WriteLine("Time taken for Processing Item ID Deletes: " + stepTimeInSeconds.ToString())
                    Logger.LogInfo("Time taken for Processing Item ID Deletes: " + stepTimeInSeconds.ToString(), Me.GetType())

                    RaiseEvent ScaleReadItemIdDeletes()

                    processMonitor.UpdateProcessMonitor("ScalePushJob", "RUNNING", Now.ToShortTimeString & " Processing Item Changes", "The job is running as Process ID " & currProcess.Id.ToString, False)

                    scalePushJobTimer = Stopwatch.StartNew()

                    itemChangeProcessor.RetrieveChangeRecordsFromIRMA(_storeUpdates, False)

                    scalePushJobTimer.Stop()
                    stepTimeInSeconds = CLng((scalePushJobTimer.ElapsedMilliseconds / 1000L))
                    Console.WriteLine("Time taken for Processing Item Changes: " + stepTimeInSeconds.ToString())
                    Logger.LogInfo("Time taken for Processing Item Changes: " + stepTimeInSeconds.ToString(), Me.GetType())

                    RaiseEvent ScaleReadItemPriceChanges()

                    processMonitor.UpdateProcessMonitor("ScalePushJob", "RUNNING", Now.ToShortTimeString & " Processing Nutrifacts", "The job is running as Process ID " & currProcess.Id.ToString, False)

                    scalePushJobTimer = Stopwatch.StartNew()

                    nutriFactProcessor.RetrieveChangeRecordsFromIRMA(_storeUpdates, False)

                    scalePushJobTimer.Stop()
                    stepTimeInSeconds = CLng((scalePushJobTimer.ElapsedMilliseconds / 1000L))
                    Console.WriteLine("Time taken for Processing Nutrifacts: " + stepTimeInSeconds.ToString())
                    Logger.LogInfo("Time taken for Processing Nutrifacts: " + stepTimeInSeconds.ToString(), Me.GetType())

                    RaiseEvent ScaleReadNutriFacts()

                    processMonitor.UpdateProcessMonitor("ScalePushJob", "RUNNING", Now.ToShortTimeString & " Processing Ingredients", "The job is running as Process ID " & currProcess.Id.ToString, False)
                    scalePushJobTimer = Stopwatch.StartNew()

                    extraTextProcessor.RetrieveChangeRecordsFromIRMA(_storeUpdates, False)

                    scalePushJobTimer.Stop()
                    stepTimeInSeconds = CLng((scalePushJobTimer.ElapsedMilliseconds / 1000L))
                    Console.WriteLine("Time taken for Processing Extra Text: " + stepTimeInSeconds.ToString())
                    Logger.LogInfo("Time taken for Processing Extra Text: " + stepTimeInSeconds.ToString(), Me.GetType())

                    RaiseEvent ScaleReadExtraText()

                    ' ZONE changes
                    processMonitor.UpdateProcessMonitor("ScalePushJob", "RUNNING", Now.ToShortTimeString & " Processing Zone Item Deletes", "The job is running as Process ID " & currProcess.Id.ToString, False)

                    scalePushJobTimer = Stopwatch.StartNew()

                    zoneItemDeleteProcessor.RetrieveChangeRecordsFromIRMA(_storeUpdates, False)

                    scalePushJobTimer.Stop()
                    stepTimeInSeconds = CLng((scalePushJobTimer.ElapsedMilliseconds / 1000L))
                    Console.WriteLine("Time taken for Processing Zone Item Deletes: " + stepTimeInSeconds.ToString())
                    Logger.LogInfo("Time taken for Processing Zone Item Deletes: " + stepTimeInSeconds.ToString(), Me.GetType())

                    RaiseEvent ScaleReadZoneDeletes()

                    processMonitor.UpdateProcessMonitor("ScalePushJob", "RUNNING", Now.ToShortTimeString & " Processing Zone Price Changes", "The job is running as Process ID " & currProcess.Id.ToString, False)

                    scalePushJobTimer = Stopwatch.StartNew()

                    zonePriceChangeProcessor.RetrieveChangeRecordsFromIRMA(_storeUpdates, False)

                    scalePushJobTimer.Stop()
                    stepTimeInSeconds = CLng((scalePushJobTimer.ElapsedMilliseconds / 1000L))
                    Console.WriteLine("Time taken for Processing Zone Price Change Adds: " + stepTimeInSeconds.ToString())
                    Logger.LogInfo("Time taken for Processing Zone Price Change Adds: " + stepTimeInSeconds.ToString(), Me.GetType())

                    RaiseEvent ScaleReadZonePriceChanges()
                End If

                ' check if any stores require SCALE hosting
                If _storeUpdates.Keys.Count > 0 Then
                    ' Deliver the file to each of the stores
                    Dim transfer As New TransferWriterFiles
                    Dim transferSuccess As Boolean
                    Dim transferStores As String    ' the FTP class includes status for each store
                    transfer.StopAlerts = StopAlerts
                    scalePushJobTimer = Stopwatch.StartNew()

                    If bUseBiztalk Then
                        processMonitor.UpdateProcessMonitor("ScalePushJob", "RUNNING", Now.ToShortTimeString & " FTPing files to BizTalk", "The job is running as Process ID " & currProcess.Id.ToString, False)
                        transferSuccess = transfer.TransferStoreFilesBT(_storeUpdates)
                        _hasTriggeredAlert = Not transferSuccess
                    Else
                        processMonitor.UpdateProcessMonitor("ScalePushJob", "RUNNING", Now.ToShortTimeString & " FTPing files to the scales", "The job is running as Process ID " & currProcess.Id.ToString, False)
                        transferSuccess = transfer.TransferStoreFiles(_storeUpdates)
                        _hasTriggeredAlert = Not transferSuccess
                    End If

                    transferStores = transfer.StoreList()

                    scalePushJobTimer.Stop()
                    stepTimeInSeconds = CLng((scalePushJobTimer.ElapsedMilliseconds / 1000L))
                    Console.WriteLine("Time taken for sending files to FTP: " + stepTimeInSeconds.ToString())
                    Logger.LogInfo("Time taken for sending files to FTP: " + stepTimeInSeconds.ToString(), Me.GetType())

                    RaiseEvent ScaleTransferFiles(transferStores)

                    'Regional and Store specific scale push uses corporate data so it needs to be cleared
                    'If this is a store scale system, only clear the data from the pending queues if all stores that are
                    'authorized for selling the item have received the update. This will result in duplicate records being
                    'sent to stores that were successful the first attempt, but it prevents stores from missing out on an
                    'update.
                    If transferSuccess Then
                        'for store scale systems, build a string of all the stores that successfully received the file
                        Dim storeList As New StringBuilder
                        Dim currentStore As StoreUpdatesBO
                        Dim storeEnum As IEnumerator = _storeUpdates.Values.GetEnumerator()

                        While (storeEnum.MoveNext())
                            currentStore = CType(storeEnum.Current, StoreUpdatesBO)
                            If currentStore.ChangesDelivered Then
                                storeList.Append(currentStore.StoreNum)
                                storeList.Append("|")
                            End If
                        End While

                        'update IRMA to remove temp items
                        processMonitor.UpdateProcessMonitor("ScalePushJob", "RUNNING", Now.ToShortTimeString & " Deleting temp corp scale changes", "The job is running as Process ID " & currProcess.Id.ToString, False)

                        scalePushJobTimer = Stopwatch.StartNew()

                        PLUMCorpChgDAO.DeleteTempCorporateDataChanges(storeList.ToString)

                        scalePushJobTimer.Stop()
                        stepTimeInSeconds = CLng((scalePushJobTimer.ElapsedMilliseconds / 1000L))
                        Console.WriteLine("Time taken for temp corp scale changes: " + stepTimeInSeconds.ToString())
                        Logger.LogInfo("Time taken for temp corp scale changes: " + stepTimeInSeconds.ToString(), Me.GetType())

                        RaiseEvent ScaleCorpTempQueueCleared()

                        processMonitor.UpdateProcessMonitor("ScalePushJob", "RUNNING", Now.ToShortTimeString & " Deleting temp nutrifact changes", "The job is running as Process ID " & currProcess.Id.ToString, False)

                        scalePushJobTimer = Stopwatch.StartNew()
                        ScaleWriterDAO.DeleteTempNutriFactChanges()

                        scalePushJobTimer.Stop()
                        stepTimeInSeconds = CLng((scalePushJobTimer.ElapsedMilliseconds / 1000L))
                        Console.WriteLine("Time taken for deleting temp nutrition fact changes: " + stepTimeInSeconds.ToString())
                        Logger.LogInfo("Time taken for deleting temp nutrition fact changes: " + stepTimeInSeconds.ToString(), Me.GetType())

                        RaiseEvent ScaleNutriFactsTempQueueCleared()

                        processMonitor.UpdateProcessMonitor("ScalePushJob", "RUNNING", Now.ToShortTimeString & " Deleting temp extra text changes", "The job is running as Process ID " & currProcess.Id.ToString, False)

                        scalePushJobTimer = Stopwatch.StartNew()
                        ScaleWriterDAO.DeleteTempExtraTextChanges()

                        scalePushJobTimer.Stop()
                        stepTimeInSeconds = CLng((scalePushJobTimer.ElapsedMilliseconds / 1000L))
                        Console.WriteLine("Time taken for deleting temp ingredient changes: " + stepTimeInSeconds.ToString())
                        Logger.LogInfo("Time taken for deleting temp ingredient changes: " + stepTimeInSeconds.ToString(), Me.GetType())

                        RaiseEvent ScaleExtraTextTempQueueCleared()

                        'update IRMA to reset auth/deauth scale flags
                        processMonitor.UpdateProcessMonitor("ScalePushJob", "RUNNING", Now.ToShortTimeString & " Applying zone price change batches in IRMA", "The job is running as Process ID " & currProcess.Id.ToString, False)

                        scalePushJobTimer = Stopwatch.StartNew()
                        zonePriceChangeProcessor.ApplyChangesInIRMA(_storeUpdates)

                        scalePushJobTimer.Stop()
                        stepTimeInSeconds = CLng((scalePushJobTimer.ElapsedMilliseconds / 1000L))
                        Console.WriteLine("Time taken for zone price change batches in IRMA: " + stepTimeInSeconds.ToString())
                        Logger.LogInfo("Time taken for zone price change batches in IRMA: " + stepTimeInSeconds.ToString(), Me.GetType())

                        processMonitor.UpdateProcessMonitor("ScalePushJob", "RUNNING", Now.ToShortTimeString & " Applying item ID add batches in IRMA", "The job is running as Process ID " & currProcess.Id.ToString, False)

                        scalePushJobTimer = Stopwatch.StartNew()
                        itemIdAddProcessor.ApplyChangesInIRMA(_storeUpdates)
                        scalePushJobTimer.Stop()
                        stepTimeInSeconds = CLng((scalePushJobTimer.ElapsedMilliseconds / 1000L))
                        Console.WriteLine("Time taken for aplying item id add batches in IRMA: " + stepTimeInSeconds.ToString())
                        Logger.LogInfo("Time taken for aplying item id add batches in IRMA: " + stepTimeInSeconds.ToString(), Me.GetType())

                        RaiseEvent ScaleAuthorizationsReset()

                        processMonitor.UpdateProcessMonitor("ScalePushJob", "RUNNING", Now.ToShortTimeString & " Applying zone item delete batches in IRMA", "The job is running as Process ID " & currProcess.Id.ToString, False)

                        scalePushJobTimer = Stopwatch.StartNew()
                        zoneItemDeleteProcessor.ApplyChangesInIRMA(_storeUpdates)

                        scalePushJobTimer.Stop()
                        stepTimeInSeconds = CLng((scalePushJobTimer.ElapsedMilliseconds / 1000L))
                        Console.WriteLine("Time taken for applying zone item delete batches in IRMA: " + stepTimeInSeconds.ToString())
                        Logger.LogInfo("Time taken for applying zone item delete batches in IRMA: " + stepTimeInSeconds.ToString(), Me.GetType())

                        RaiseEvent ScaleDeAuthorizationsReset()
                        success = True
                    ElseIf Not transferSuccess Then
                        ' update the error message so the user knows what happened
                        _jobExecutionMessage = "Transfer of Scale Hosting files did not succeed.  Updates were not applied in IRMA."
                    End If
                Else
                    ' there were not any stores to update, so scale push is considered a success
                    RaiseEvent ScaleTransferFiles("No stores to update.")
                    success = True
                End If
            Catch ex As Exception
                Logger.LogError("Exception: ", Me.GetType(), ex)
                ErrorHandler.ProcessError(ErrorType.GeneralApplicationError, SeverityLevel.Fatal, ex)
                success = False
                Dim msg As New StringBuilder
                msg.Append("Error during processing of Scale Push job.")
                msg.Append(Environment.NewLine)
                msg.Append(ex.Message)
                msg.Append(Environment.NewLine)
                msg.Append(ex.StackTrace)
                _jobExecutionMessage = msg.ToString()

                If (Not _hasTriggeredAlert And Not StopAlerts) Then
                    PagerDutyUtility.TriggerPagerDutyAlert("IRMA Scale Push", "Scale Push Failure", ex.ToString())
                    _hasTriggeredAlert = True
                End If
            End Try

            If success Then
                processMonitor.UpdateProcessMonitor("ScalePushJob", "RUNNING", Now.ToShortTimeString & " Scale push completed successfully!", "", False)
                RaiseEvent ScaleCompleteSuccess()
            Else
                processMonitor.UpdateProcessMonitor("ScalePushJob", "RUNNING", Now.ToShortTimeString & " Scale push failed", "", False)
                RaiseEvent ScaleCompleteError()
            End If

            scalePushjobMainTimer.Stop()
            stepTimeInSeconds = CLng((scalePushjobMainTimer.ElapsedMilliseconds/1000L))
            Console.WriteLine("Time taken for the Scale Push: " + stepTimeInSeconds.ToString())
            Logger.LogInfo("Time taken for the Scale Push: " + stepTimeInSeconds.ToString(), Me.GetType())  
            
            Return success
        End Function
    End Class
End Namespace
