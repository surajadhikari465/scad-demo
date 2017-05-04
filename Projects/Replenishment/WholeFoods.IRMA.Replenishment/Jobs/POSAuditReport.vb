Imports System.Configuration.ConfigurationSettings
Imports System.IO
Imports System.Text
Imports WholeFoods.IRMA.Replenishment.Common.Writers
Imports WholeFoods.IRMA.Replenishment.POSPush.BusinessLogic
Imports WholeFoods.IRMA.Replenishment.POSPush.Controller
Imports WholeFoods.IRMA.Replenishment.POSPush.DataAccess
Imports WholeFoods.IRMA.Replenishment.POSPush.POSException
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
    Public Class POSAuditReport
#Region "Events raised by this job"
        ' These events are raised during key steps of the process so the U.I. can let the user know
        ' where in the process things are.
        Public Event ScalePushStarted(ByVal IsRegional As Boolean)
        Public Event ScaleReadStoreConfigurationData(ByVal NumStores As Integer)
        Public Event ScaleReadItemIdAdds()
        Public Event ScaleTransferFiles(ByVal FileStatus As String)
        Public Event ScaleCorpTempQueueCleared()
        Public Event ScaleCompleteSuccess()
        Public Event ScaleCompleteError()

        Public Event POSPushStarted()
        Public Event POSReadStoreConfigurationData(ByVal NumStores As Integer)
        Public Event POSReadVendorAdds()
        Public Event POSReadItemIdAdds()
        Public Event POSGeneratedPOSControlFiles()
        Public Event POSTransferFiles(ByVal FileStatus As String)
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
        ''' store to generate a full POS file for
        ''' </summary>
        ''' <remarks></remarks>
        Private _storeNo As Integer

        ''' <summary>
        ''' Allows optional deletion of files upon failure
        ''' </summary>
        ''' <remarks></remarks>
        Private _deleteFilesOnFailure As Boolean = False

#Region "Constructors"
        ''' <summary>
        ''' Use default settings
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
        End Sub

        ''' <summary>
        ''' Allows optional deletion of files upon failure
        ''' </summary>
        ''' <param name="deleteFilesOnFailure">When True, deletes the generated files upon failure.  Otherwise the files are kept until the next attempt.</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal deleteFilesOnFailure As Boolean)
            _deleteFilesOnFailure = deleteFilesOnFailure
        End Sub
#End Region

        ''' <summary>
        ''' Generates POS Push files for all stores configured in the POS admin application.
        ''' The files contain data for ALL items available in the store, not just items that were
        ''' added/deleted/batched.
        ''' </summary>
        ''' <returns>True if it executes successfully; False otherwise</returns>
        ''' <remarks></remarks>
        Public Function Main() As Boolean
            Logger.LogDebug("Main entry", Me.GetType())
            ' Set the return flag
            Dim returnStatus As Boolean = True
            Dim processorWriteError As Boolean = False
            Dim processorDescription As String = "Not Defined" ' recorded for error handling purposes
            Dim currentProcessor As POSProcessor = Nothing ' recorded for error handling purposes

            Try
                RaiseEvent POSPushStarted()
                ' Initialize the storeUpdates hashtable, adding an entry for each store
                _storeUpdates = StorePOSConfigDAO.GetStoreConfigurations("POS", _storeNo.ToString)

                RaiseEvent POSReadStoreConfigurationData(_storeUpdates.Count)

                ' Process each type of change
                ' Use the same date for the changes to make sure they all operate the same
                Dim dStart As Date = Now

                ' Initialize a change processor for each type of change  
                Dim vendorIdAddProcessor As New POSVendorIdAddsProcessor(dStart)
                Dim itemIdAddProcessor As New POSItemIdAddsProcessor(dStart)

                Try
                    processorDescription = "Vendor ID Adds"
                    currentProcessor = vendorIdAddProcessor

                    currentProcessor.POSFileWriterList = POSWriterDAO.GetPOSFileWriterClass()
                    Dim currentPOSFileWriterClass As String = currentProcessor.GetPOSFileWriterClass(_storeNo)

                    ' For an R10 store build, custom logic will be executed to populate the IConPOSPushPublish table.
                    If currentPOSFileWriterClass.Contains("R10") Then
                        POSWriterDAO.BuildStorePosFileForR10(_storeNo)
                        RaiseEvent POSCompleteSuccess()
                        Logger.LogDebug("Main exit", Me.GetType())
                        Return returnStatus
                    Else
                        vendorIdAddProcessor.RetrieveChangeRecordsFromIRMA(_storeUpdates, True, _storeNo.ToString)
                        RaiseEvent POSReadVendorAdds()

                        ' Grab the details for each change and add them to the POS Push file for the associated store.
                        processorDescription = "Item ID Adds"
                        currentProcessor = itemIdAddProcessor
                        itemIdAddProcessor.RetrieveChangeRecordsFromIRMA(_storeUpdates, True, _storeNo.ToString)
                        RaiseEvent POSReadItemIdAdds()
                    End If
                Catch e As Exception
                    ' Send an error email with the details
                    Dim args(6) As String
                    args(0) = processorDescription
                    args(1) = currentProcessor.DebugStoreNo().ToString()
                    args(2) = currentProcessor.DebugSuccessLinesCount().ToString()
                    args(3) = currentProcessor.DebugErrorRowNum()
                    args(4) = currentProcessor.DebugErrorColNum()
                    args(5) = currentProcessor.DebugFieldId()
                    ErrorHandler.ProcessError(ErrorType.POSAudit_ProcessorRetrievalException, args, SeverityLevel.Fatal, e)
                    ' Add additional error message information, if it exists, to the error message
                    Dim message As New StringBuilder
                    message.Append("POS Push error when parsing the data and adding it to the IRMA output file.  This is typically caused by a data error.")
                    message.Append(Environment.NewLine)
                    message.AppendFormat("Change Type Being Processed: {0}", args(0))
                    message.Append(Environment.NewLine)
                    message.AppendFormat("Current Store Being Processed: {0}", args(1))
                    message.Append(Environment.NewLine)
                    message.AppendFormat("# Successful Lines Written to Store File: {0}", args(2))
                    message.Append(Environment.NewLine)
                    message.AppendFormat("Current Row # Being Processed: {0}", args(3))
                    message.Append(Environment.NewLine)
                    message.AppendFormat("Current Column # Being Processed: {0}", args(4))
                    message.Append(Environment.NewLine)
                    message.AppendFormat("Current Field ID Being Processed: {0}", args(5))
                    message.Append(Environment.NewLine)
                    ' Set the flag so that another email is not generated when the exception is caught
                    ' further down in the code; Set the error messsage text for the UI client
                    processorWriteError = True
                    _errorMessage = message.ToString
                    ' Throw the exception to allow error handling to continue
                    Throw e
                End Try

                ' Generate a POS Control file for each store (if needed)
                Dim storeEnum As IDictionaryEnumerator = _storeUpdates.GetEnumerator
                Dim currentStore As StoreUpdatesBO
                While storeEnum.MoveNext
                    currentStore = CType(storeEnum.Value, StoreUpdatesBO)
                    currentStore.FileWriter.CreateControlFile(currentStore)
                End While
                RaiseEvent POSGeneratedPOSControlFiles()

                ' Deliver the file to each of the stores
                Dim transfer As New TransferWriterFiles(_deleteFilesOnFailure)
                Dim transferSuccess As Boolean = transfer.TransferStoreFiles(_storeUpdates)
                RaiseEvent POSTransferFiles(transfer.StoreList)

                If transferSuccess Then
                    ' Update the IRMA database to indicate that the changes were applied to the stores
                    'vendorIdAddProcessor.ApplyChangesInIRMA(_storeUpdates)
                    'itemIdAddProcessor.ApplyChangesInIRMA(_storeUpdates)
                    'RaiseEvent POSApplyChangesToIRMA()

                    Dim filename As String = "ssh_remote_status.txt"

                    If File.Exists(filename) Then
                        File.Delete(filename)
                    End If

                    RaiseEvent POSStartedRemoteJobs()

                    'call remote process (if needed for writer)
                    storeEnum.Reset()
                    While storeEnum.MoveNext
                        currentStore = CType(storeEnum.Value, StoreUpdatesBO)

                        If (currentStore.FTPInfo.IsSecureTransfer) Then

                            Logger.WriteLog(filename, "SSH Remote execution started on store [Store# <{0}>, IP = <{1}>, UserId = <{2}>, Password = <{3}>] ...", _
                            currentStore.StoreNum, _
                            currentStore.FTPInfo.IPAddress, _
                            currentStore.FTPInfo.FTPUser, _
                            currentStore.FTPInfo.FTPPassword)

                            currentStore.FileWriter.CallSSHRemoteJobProcess(currentStore, filename)
                            RaiseEvent POSSSHRemoteExecution(currentStore.FileWriter.RemoteSSHStoreList)
                        Else
                            currentStore.FileWriter.CallRemoteJobProcess(currentStore)
                        End If

                    End While
                Else
                    ' update the error message so the user knows what happened
                    _errorMessage = "Transfer of POS files did not succeed.  Updates were not applied in IRMA."
                    Throw New Exception(_errorMessage)
                End If

            Catch e As DataFactoryException
                Logger.LogError("Exception: ", Me.GetType(), e)
                ErrorHandler.ProcessError(ErrorType.DataFactoryException, SeverityLevel.Fatal, e)
                returnStatus = False
                _errorMessage = e.Message()
                _errorException = e
            Catch e1 As Exception
                Logger.LogError("Exception: ", Me.GetType(), e1)
                If Not processorWriteError Then
                    ErrorHandler.ProcessError(ErrorType.GeneralApplicationError, SeverityLevel.Fatal, e1)
                    _errorMessage = e1.Message()
                End If
                returnStatus = False
                _errorException = e1
            End Try

            If returnStatus = True Then
                RaiseEvent POSCompleteSuccess()
            Else
                RaiseEvent POSCompleteError()
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

        ''' <summary>
        ''' Allows optional deletion of files upon failure
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks>When True, deletes the generated files upon failure.  Otherwise the files are kept until the next attempt.</remarks>
        Friend Property DeleteFilesOnFailure() As Boolean
            Get
                Return _deleteFilesOnFailure
            End Get
            Set(ByVal value As Boolean)
                _deleteFilesOnFailure = value
            End Set
        End Property

    End Class
End Namespace
