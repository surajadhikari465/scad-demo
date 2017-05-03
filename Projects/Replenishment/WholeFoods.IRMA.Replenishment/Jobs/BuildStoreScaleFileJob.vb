Imports System.Configuration.ConfigurationSettings
Imports System.IO
Imports System.Text
Imports WholeFoods.IRMA.Replenishment.Common.BusinessLogic
Imports WholeFoods.IRMA.Replenishment.Common.DataAccess
Imports WholeFoods.IRMA.Replenishment.Common.Writers
Imports WholeFoods.IRMA.Replenishment.POSPush.BusinessLogic
Imports WholeFoods.IRMA.Replenishment.ScalePush.Controller
Imports WholeFoods.IRMA.Replenishment.ScalePush.DataAccess
Imports WholeFoods.Utility
Imports WholeFoods.Utility.DataAccess
Imports WholeFoods.Utility.FTP

Namespace WholeFoods.IRMA.Replenishment.Jobs

    ''' <summary>
    ''' Job that performs the POS Audit Report generation.
    ''' </summary>
    ''' <remarks>
    ''' ASSUMPTION: this is NOT designed to be multi-threaded.  If process must become multi-threaded then 
    ''' file management in StoreUpdatesBO.vb should no longer remove file if temp file exists upon
    ''' process startup.
    ''' </remarks>
    Public Class BuildStoreScaleFileJob
        ''' <summary>
        ''' Collection of StoreUpdatesBO objects to store the Scale Push configuration for each store.
        ''' </summary>
        ''' <remarks></remarks>
        Private _storeUpdates As Hashtable = New Hashtable

        ''' <summary>
        ''' Contains a message describing the error condition if Main does not execute successfully.
        ''' </summary>
        ''' <remarks></remarks>
        Private _errorMessage As String

        ''' <summary>
        ''' Contains any exception caught during processing if Main does not execute successfully.
        ''' </summary>
        ''' <remarks></remarks>
        Private _errorException As Exception

        ''' <summary>
        ''' The date should be the same for all POS Push and Scale Push stored procedure calls to keep the
        ''' process consistent.  This is important if the job starts before midnight and completes after midnight.
        ''' </summary>
        ''' <remarks></remarks>
        Private _jobRunDate As Date = Now

        ''' <summary>
        ''' store to generate a full scale file for
        ''' </summary>
        ''' <remarks></remarks>
        Private _storeNo As Integer

#Region "Events raised by this job"
        ' These events are raised during key steps of the process so the U.I. can let the user know
        ' where in the process things are.
        Public Event ScalePushStarted(ByVal IsRegional As Boolean)
        Public Event ScaleReadStoreConfigurationData(ByVal NumStores As Integer)
        Public Event ScaleReadScaleItemsForStore()
        Public Event ScaleTransferFiles(ByVal FileStatus As String)
        Public Event ScaleCompleteSuccess()
        Public Event ScaleCompleteError()
#End Region

        ''' <summary>
        ''' Generates Scale Push files for all stores configured in the admin application.
        ''' The files contain data for ALL items available in the store, not just items that were
        ''' added/deleted/batched.
        ''' </summary>
        ''' <returns>True if it executes successfully; False otherwise</returns>
        ''' <remarks></remarks>
        Public Function Main() As Boolean
            Logger.LogDebug("Main entry", Me.GetType())
            ' Set the return flag
            Dim returnStatus As Boolean = True

            ' Define the change processors being used for the full store file
            Dim itemIdAddProcessor As CorpScaleItemIdAddsProcessor

            Try
                'determine if scale push is at regional or store level
                Dim useRegionalScaleFile As Boolean = InstanceDataDAO.IsFlagActive("UseRegionalScaleFile")
                RaiseEvent ScalePushStarted(useRegionalScaleFile)

                If useRegionalScaleFile Then
                    'get REGIONAL level scale configuration
                    ' Initialize the storeUpdates hashtable, adding an entry for each ScaleWriterType for the regional Store entry
                    _storeUpdates = StoreScaleConfigDAO.GetStoreConfigurations(Constants.FileWriterType_SCALE, True)
                    RaiseEvent ScaleReadStoreConfigurationData(_storeUpdates.Count)

                    ' The corporate scale writers include pricing exception records, and the output file format is defined in the
                    ' zone scale writer definition using the admin UI.  Copy the zone definition to the corporate definition.
                    Dim corpScaleWriter As StoreUpdatesBO = CType(_storeUpdates.Item(Constants.ScaleWriterType_Corporate), StoreUpdatesBO)
                    Dim zoneScaleWriter As StoreUpdatesBO
                    If (Not InstanceDataDAO.IsFlagActive("UseSmartXPriceData")) Then
                        zoneScaleWriter = CType(_storeUpdates.Item(Constants.ScaleWriterType_Zone), StoreUpdatesBO)
                        corpScaleWriter.FileWriter.CorpScalePriceExceptionConfig = zoneScaleWriter.FileWriter.ZoneScalePriceChangeConfig
                    Else
                        ' The SmartX writer works differently than other scale writers.  The same writer from IRMA is used,
                        ' but the filename sent to the scales for the Price changes is different from all other filenames.
                        zoneScaleWriter = CType(_storeUpdates.Item(Constants.ScaleWriterType_SmartXZone), StoreUpdatesBO)
                        zoneScaleWriter.FileWriter.WriterFilename(Nothing) = "Text1.txt"
                    End If

                    ' Initialize a change processor for each type of change being added to the full store file
                    ' --- CORP changes
                    itemIdAddProcessor = New CorpScaleItemIdAddsProcessor(_jobRunDate)

                    ' Grab the details for each change and add them to the Regional Scale Push file
                    ' --- CORP changes
                    itemIdAddProcessor.RetrieveChangeRecordsFromIRMA(_storeUpdates, True, _storeNo.ToString)
                    RaiseEvent ScaleReadScaleItemsForStore()

                Else
                    'get STORE configurations
                    '' Initialize the storeUpdates hashtable, adding an entry for each store
                    _storeUpdates = StoreScaleConfigDAO.GetStoreConfigurations(Constants.FileWriterType_SCALE, False)
                    RaiseEvent ScaleReadStoreConfigurationData(_storeUpdates.Count)

                    '' check if any stores require SCALE hosting
                    If _storeUpdates.ContainsKey(_storeNo) Then
                        ' Initialize a change processor for each type of change
                        itemIdAddProcessor = New CorpScaleItemIdAddsProcessor(_jobRunDate)

                        ' Grab the details for each change and add them to the POS Push file for the associated store
                        itemIdAddProcessor.RetrieveChangeRecordsFromIRMA(_storeUpdates, False, _storeNo.ToString)
                        RaiseEvent ScaleReadScaleItemsForStore()
                    Else
                        ' ERROR - the store does not contain scale writer configuration data
                        returnStatus = False
                        _errorMessage = "The selected store (" + _storeNo.ToString + ") has not been assigned a scale writer so processing cannot continue."
                    End If
                End If

                ' check if any stores require SCALE hosting
                If _storeUpdates.Keys.Count > 0 AndAlso returnStatus Then
                    ' Deliver the file to each of the stores
                    Dim transfer As New TransferWriterFiles
                    Dim transferSuccess As Boolean
                    Dim transferStores As String    ' the FTP class includes status for each store
                    If useRegionalScaleFile Then
                        ' It is important that the corporate file is sent before the zone file so that the
                        ' changes arrive in the correct order at PLUM Store.  Because the hashtable is not
                        ' sorted, this must be handled in the code.
                        Dim corpHash As New Hashtable
                        Dim transferCorpSuccess As Boolean = False
                        corpHash.Add(Constants.ScaleWriterType_Corporate, _storeUpdates.Item(Constants.ScaleWriterType_Corporate))
                        transferCorpSuccess = transfer.TransferStoreFiles(corpHash)
                        transferStores = transfer.StoreList()

                        If (Not InstanceDataDAO.IsFlagActive("UseSmartXPriceData")) Then
                            Dim zoneHash As New Hashtable
                            Dim transferZoneSuccess As Boolean = False
                            zoneHash.Add(Constants.ScaleWriterType_Zone, _storeUpdates.Item(Constants.ScaleWriterType_Zone))
                            transferZoneSuccess = transfer.TransferStoreFiles(zoneHash)
                            transferStores += transfer.StoreList()

                            transferSuccess = transferCorpSuccess AndAlso transferZoneSuccess
                        Else
                            Dim zoneSmartxHash As New Hashtable
                            Dim transferZoneSmartxSuccess As Boolean = False
                            zoneSmartxHash.Add(Constants.ScaleWriterType_SmartXZone, _storeUpdates.Item(Constants.ScaleWriterType_SmartXZone))
                            transferZoneSmartxSuccess = transfer.TransferStoreFiles(zoneSmartxHash)
                            transferStores += transfer.StoreList()

                            transferSuccess = transferCorpSuccess AndAlso transferZoneSmartxSuccess
                        End If
                    Else
                        transferSuccess = transfer.TransferStoreFiles(_storeUpdates)
                        transferStores = transfer.StoreList()
                    End If

                    RaiseEvent ScaleTransferFiles(transferStores)
                ElseIf returnStatus Then
                    ' there were not any stores to update, so scale push is considered a success
                    RaiseEvent ScaleTransferFiles("no stores to update")
                    returnStatus = True
                End If
            Catch e As DataFactoryException
                Logger.LogError("Exception: ", Me.GetType(), e)
                ErrorHandler.ProcessError(ErrorType.DataFactoryException, SeverityLevel.Fatal, e)
                returnStatus = False
                _errorMessage = e.Message()
                _errorException = e
            Catch e1 As Exception
                Logger.LogError("Exception: ", Me.GetType(), e1)
                ErrorHandler.ProcessError(ErrorType.GeneralApplicationError, SeverityLevel.Fatal, e1)
                _errorMessage = e1.Message()
                returnStatus = False
                _errorException = e1
            End Try

            If returnStatus Then
                RaiseEvent ScaleCompleteSuccess()
            Else
                RaiseEvent ScaleCompleteError()
            End If

            Logger.LogDebug("Main exit", Me.GetType())
            Return returnStatus
        End Function

        Public Property ErrorMessage() As String
            Get
                Return _errorMessage
            End Get
            Set(ByVal Value As String)
                _errorMessage = Value
            End Set
        End Property

        Public Property ErrorException() As Exception
            Get
                Return _errorException
            End Get
            Set(ByVal Value As Exception)
                _errorException = Value
            End Set
        End Property

        Public Property StoreNo() As Integer
            Get
                Return _storeNo
            End Get
            Set(ByVal value As Integer)
                _storeNo = value
            End Set
        End Property

    End Class
End Namespace
