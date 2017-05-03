Imports System.Data.SqlClient
Imports System.Text
Imports WholeFoods.IRMA.Replenishment.Common.DataAccess
Imports WholeFoods.IRMA.Replenishment.Common.Writers
Imports WholeFoods.IRMA.Replenishment.POSPush.BusinessLogic
Imports WholeFoods.IRMA.Replenishment.ScalePush.DataAccess
Imports WholeFoods.Utility

Namespace WholeFoods.IRMA.Replenishment.ScalePush.Controller

    ''' <summary>
    ''' Subclasses ScaleProcessor, providing the details for the ItemIdAdd ChangeType.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CorpScaleItemIdAddsProcessor
        Inherits ScaleProcessor

        ''' <summary>
        ''' Constructor
        ''' </summary>
        ''' <param name="inDate"></param>
        ''' <remarks></remarks>
        Public Sub New(ByVal inDate As Date)
            MyBase.New(inDate)
        End Sub

        ''' <summary>
        ''' Reads the database records from IRMA, adding the results to the SCALE Push file for each store.
        ''' 
        ''' If the store number parameter is set, this reads the full scale file for a single store instead of 
        ''' processing added scale items for all stores.
        ''' </summary>
        ''' <param name="storeUpdates"></param>
        ''' <param name="isRegionalScaleFile"></param>
        ''' <param name="storeNo"></param>
        ''' <remarks></remarks>
        Overrides Sub RetrieveChangeRecordsFromIRMA(ByRef storeUpdates As Hashtable, ByVal isRegionalScaleFile As Boolean, Optional ByVal storeNo As String = Nothing)
            Logger.LogDebug("RetrieveChangeRecordsFromIRMA entry", Me.GetType())

            If isRegionalScaleFile Then
                'REGIONAL FILE --> get CORPORATE data - item id adds from GetPLUMCorpChg
                RetrieveCorporateChangeRecordsFromIRMA(storeUpdates, storeNo)
            Else
                'STORE SPECIFIC FILES --> get item id adds
                RetrieveStoreChangeRecordsFromIRMA(storeUpdates, storeNo)
            End If

            Logger.LogDebug("RetrieveChangeRecordsFromIRMA exit", Me.GetType())
        End Sub

        ''' <summary>
        ''' gets the item id adds for a STORE scale file;
        ''' </summary>
        ''' <param name="storeUpdates"></param>
        ''' <remarks></remarks>
        Overrides Sub RetrieveStoreChangeRecordsFromIRMA(ByRef storeUpdates As Hashtable, ByVal storeNo As String)
            Logger.LogDebug("RetrieveStoreChangeRecordsFromIRMA entry", Me.GetType())

            Dim scalePushJobTimer As Stopwatch
            Dim stepTimeInSeconds As Long = 0

            Dim results As SqlDataReader = Nothing

            Try
                scalePushJobTimer = Stopwatch.StartNew()

                If storeNo IsNot Nothing Then
                    ' get the data for a full scale file for this store; ActionCode = 'F'
                    ' this result set will include all the authorized scale items for the specified store
                    results = PLUMCorpChgDAO.GetCorporateDataChanges(Constants.ActionCode_FullScaleFile, dStart, storeNo)
                Else
                    'get item data changes; ActionCode = 'A'
                    'this result set will also include the records for scale items that have been 
                    'authorized in IRMA (StoreItem.ScaleAuth = 1)
                    results = PLUMCorpChgDAO.GetCorporateDataChanges(Constants.ActionCode_ItemIdAdd, dStart, storeNo)
                End If

                scalePushJobTimer.Stop()
                stepTimeInSeconds = CLng((scalePushJobTimer.ElapsedMilliseconds/1000L))
                Console.WriteLine("Time taken for ItemID Adds Get Store Change Records DB Step : " + stepTimeInSeconds.ToString())
                Logger.LogInfo("Time taken for ItemID Adds Get Store Change Records DB Step : " + stepTimeInSeconds.ToString(), Me.GetType())

                scalePushJobTimer = Stopwatch.StartNew()

                ' -- First resultset - list of items to be sent to the scales
                Logger.LogDebug("processing first result set - change records plus authorized scale items", Me.GetType())
                WriteResultsToFile(storeUpdates, results, ChangeType.CorpScaleItemIdAdd, False)

                ' -- Second resultset - list of authorizations communicated by this run of scale push 
                Logger.LogDebug("processing second result set - list of scale authorizations processed by this request", Me.GetType())

                Dim currentStoreNum As Integer
                Dim currentStoreUpdate As StoreUpdatesBO

                If results.NextResult() Then
                    While results.Read()
                        ' get the store # for this record
                        currentStoreNum = results.GetInt32(results.GetOrdinal("Store_No"))

                        ' verify this store is configured in StorePOSConfig
                        If (storeUpdates.ContainsKey(currentStoreNum)) Then
                            currentStoreUpdate = CType(storeUpdates.Item(currentStoreNum), StoreUpdatesBO)

                            ' track ID data for updates to DB in ApplyChangesInIRMA
                            PopulateApplyChangesData(currentStoreUpdate, results)
                        Else
                            ' ERROR PROCESSING ... all stores should be found in the hash
                            Logger.LogWarning("Error processing in CorpScaleItemIdAddsProcessor.RetrieveStoreChangeRecordsFromIRMA.  Store # not configured in StoreScaleConfig table: " & currentStoreNum, Me.GetType())

                            'send message about exception
                            Dim args(1) As String
                            args(0) = currentStoreNum.ToString
                            ErrorHandler.ProcessError(ErrorType.ScalePush_StoreNotFound, args, SeverityLevel.Warning)

                            Continue While
                        End If
                    End While
                End If
                scalePushJobTimer.Stop()
                stepTimeInSeconds = CLng((scalePushJobTimer.ElapsedMilliseconds/1000L))
                Console.WriteLine("Time taken for ItemID Adds Get Store Change Records Application Step : " + stepTimeInSeconds.ToString())
                Logger.LogInfo("Time taken for ItemID Adds Get Store Change Records Application Step : " + stepTimeInSeconds.ToString(), Me.GetType())
            Finally
                ' Close the result set and the connection
                If (results IsNot Nothing) Then
                    results.Close()
                End If
            End Try

            Logger.LogDebug("RetrieveStoreChangeRecordsFromIRMA exit", Me.GetType())
        End Sub

        ''' <summary>
        ''' gets the CORPORATE data changes for a regional scale file;
        ''' gets data from GetPLUMCorpChg where ActionCode = 'A'
        ''' </summary>
        ''' <param name="storeUpdates"></param>
        ''' <remarks></remarks>
        Overrides Sub RetrieveCorporateChangeRecordsFromIRMA(ByRef storeUpdates As Hashtable, ByVal storeNo As String)
            Logger.LogDebug("RetrieveCorporateChangeRecordsFromIRMA entry", Me.GetType())

            Dim results As SqlDataReader = Nothing
            Dim scalePushJobTimer As Stopwatch
            Dim stepTimeInSeconds As Long = 0

            Try
                scalePushJobTimer = Stopwatch.StartNew()

                If storeNo IsNot Nothing Then
                    ' get the data for a full scale file for this store; ActionCode = 'F'
                    ' this result set will include all the authorized scale items for the specified store
                    results = PLUMCorpChgDAO.GetCorporateDataChanges(Constants.ActionCode_FullScaleFile, dStart, storeNo)
                Else
                    'get item data changes; ActionCode = 'A'
                    results = PLUMCorpChgDAO.GetCorporateDataChanges(Constants.ActionCode_ItemIdAdd, dStart, storeNo)
                End If

                scalePushJobTimer.Stop()
                stepTimeInSeconds = CLng((scalePushJobTimer.ElapsedMilliseconds/1000L))
                Console.WriteLine("Time taken for ItemID Adds Get Corp Change Records DB Step : " + stepTimeInSeconds.ToString())
                scalePushJobTimer = Stopwatch.StartNew()

                ' Process the result set
                ' -- First resultset - list of corporate change records 
                Logger.LogDebug("processing first result set - corporate records", Me.GetType())
                WriteResultsToFile(storeUpdates, results, ChangeType.CorpScaleItemIdAdd, True, Constants.ScaleWriterType_Corporate)

                ' -- Second resultset - list of price zone records to be included with the corporate changes
                If results.NextResult() Then
                    If (Not InstanceDataDAO.IsFlagActive("UseSmartXPriceData")) Then
                        Logger.LogDebug("processing second result set - price zones for corporate records", Me.GetType())
                        WriteResultsToFile(storeUpdates, results, ChangeType.CorpScalePriceExceptions, True, Constants.ScaleWriterType_Corporate)
                    End If
                End If

                scalePushJobTimer.Stop()
                stepTimeInSeconds = CLng((scalePushJobTimer.ElapsedMilliseconds/1000L))
                Console.WriteLine("Time taken for ItemID Adds Get Corp Change Records Application Step : " + stepTimeInSeconds.ToString())

                ' -- Third resultset - list of authorizations communicated by this run of scale push 
                ' not used by corporate scale systems; these are communicated as zone price changes
                Logger.LogDebug("skipping the third result set - scale authorizations are sent as zone price changes", Me.GetType())
            Catch ex As Exception
                'common exceptions would be DataFactory or File I/O exceptions
                Logger.LogError("RetrieveCorporateChangeRecordsFromIRMA error during processing", Me.GetType(), ex)
                Throw ex
            Finally
                ' Close the result set and the connection
                If (results IsNot Nothing) Then
                    results.Close()
                End If
            End Try

            Logger.LogDebug("RetrieveCorporateChangeRecordsFromIRMA exit", Me.GetType())
        End Sub

        ''' <summary>
        ''' Reset the authorization flag for the scale authorizations that were communicated to the
        ''' scales by this run of scale push.
        ''' </summary>
        ''' <param name="StoreUpdatesBO"></param>
        ''' <remarks></remarks>
        Public Overrides Sub ApplyChangesInIRMA(ByRef StoreUpdatesBO As Hashtable)
            Logger.LogDebug("ApplyChangesInIRMA entry", Me.GetType())

            Dim errors As New StringBuilder

            Try
                If (StoreUpdatesBO.Count > 0) Then
                    Dim storeEnum As IEnumerator = StoreUpdatesBO.Values.GetEnumerator()
                    Dim currentStore As StoreUpdatesBO
                    Dim changesEnum As IEnumerator

                    ' Process each of the stores that have changes
                    While storeEnum.MoveNext()
                        ' For each store, update each of the records associated with scale auths
                        currentStore = CType(storeEnum.Current, StoreUpdatesBO)

                        If currentStore.ChangesDelivered AndAlso currentStore.ScaleAuthorizations.Count > 0 Then
                            changesEnum = currentStore.ScaleAuthorizations.GetEnumerator()

                            While changesEnum.MoveNext()
                                Try
                                    ' Execute the stored procedure to update the StoreItem record
                                    ScaleWriterDAO.UpdateAuthorizedScaleChanges(CType(changesEnum.Current, Integer))
                                Catch ex As Exception
                                    Logger.LogError(" Exception: ", Me.GetType(), ex)

                                    'track list of ids (w/ store) that cause error and report errors
                                    errors.Append("Store: ")
                                    errors.Append(currentStore.StoreNum)
                                    errors.Append("   StoreItemAuthorizationID: ")
                                    errors.Append(changesEnum.Current)
                                    errors.Append(Environment.NewLine)
                                End Try
                            End While
                        End If
                    End While
                End If
            Catch e1 As Exception
                Logger.LogError(" Exception: ", Me.GetType(), e1)

                Dim args(2) As String
                args(0) = "CorpScaleItemIdAdd"
                args(1) = "There were errors processing the following StoreItemAuthorizationID values: " + errors.ToString
                ErrorHandler.ProcessError(ErrorType.ScalePush_ApplyAuthChangesInIRMA, args, SeverityLevel.Fatal, e1)
            Finally
                'handle errors if any in errors StringBuilder
                If errors.Length > 0 Then
                    Dim args(2) As String
                    args(0) = "CorpScaleItemIdAdd"
                    args(1) = "There were errors processing the following StoreItemAuthorizationID values: " + errors.ToString
                    ErrorHandler.ProcessError(ErrorType.ScalePush_ApplyAuthChangesInIRMA, args, SeverityLevel.Fatal)
                End If
            End Try

            Logger.LogDebug("ApplyChangesInIRMA exit", Me.GetType())
        End Sub

        ''' <summary>
        ''' Keep track of the StoreItem.StoreItemAuthorizationID values for the authorizations communicated to
        ''' the scales by this run of scale push.
        ''' </summary>
        ''' <param name="currentStore"></param>
        ''' <param name="result"></param>
        ''' <remarks></remarks>
        Public Overrides Sub PopulateApplyChangesData(ByRef currentStore As StoreUpdatesBO, ByRef result As SqlDataReader)
            currentStore.ScaleAuthorizations.Add(result.GetInt32(result.GetOrdinal("StoreItemAuthorizationID")))
        End Sub
    End Class
End Namespace
