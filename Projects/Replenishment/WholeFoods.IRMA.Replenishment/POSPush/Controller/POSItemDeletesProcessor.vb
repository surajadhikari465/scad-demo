Imports System.Collections
Imports System.Data.SqlClient
Imports System.Text
Imports WholeFoods.IRMA.Replenishment.Common.Writers
Imports WholeFoods.IRMA.Replenishment.POSPush.BusinessLogic
Imports WholeFoods.IRMA.Replenishment.POSPush.DataAccess
Imports WholeFoods.Utility

Namespace WholeFoods.IRMA.Replenishment.POSPush.Controller

    ''' <summary>
    ''' Subclasses POSProcessor, providing the details for the ItemDataDelete ChangeType.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class POSItemDeletesProcessor
        Inherits POSProcessor

        ''' <summary>
        ''' Constructor
        ''' </summary>
        ''' <param name="inDate"></param>
        ''' <remarks></remarks>
        Public Sub New(ByVal inDate As Date)
            MyBase.New(inDate)
        End Sub

        ''' <summary>
        ''' Reads the database records from IRMA, adding the results to the POS Push file for each store.
        ''' Executes the GetPriceBatchSent stored procedure to retrieve the change records.
        ''' </summary>
        ''' <param name="StoreUpdatesBO"></param>
        ''' <remarks></remarks>
        Overrides Sub RetrieveChangeRecordsFromIRMA(ByRef StoreUpdatesBO As Hashtable, ByVal isAuditReport As Boolean, Optional ByVal storeNo As String = Nothing)
            Logger.LogDebug("RetrieveChangeRecordsFromIRMA entry", Me.GetType())
            Dim results As SqlDataReader = Nothing
            Dim posPushJobTimer As Stopwatch
            Dim stepTimeInSeconds As Long = 0

            Try
                posPushJobTimer = Stopwatch.StartNew()

                ' Read the price batch deletes that are ready to be sent to the stores.
                results = POSWriterDAO.GetItemDeletes(dStart)

                posPushJobTimer.Stop()      
                stepTimeInSeconds = CLng((posPushJobTimer.ElapsedMilliseconds/1000L))
                Console.WriteLine("Time taken for processing Item Deletes DB Step in POS Push: " + stepTimeInSeconds.ToString())
                Logger.LogInfo("Time taken for processing Item Deletes DB Step in POS Push: " + stepTimeInSeconds.ToString(), Me.GetType())

                posPushJobTimer = Stopwatch.StartNew()
                ' -- First resultset - list of delete items and their details
                Logger.LogDebug("processing first result set - batched delete details", Me.GetType())
                ' Process the result set
                WriteResultsToFile(StoreUpdatesBO, results, ChangeType.ItemDataDelete)

                ' -- second resultset - list of de-auth items and their details
                Logger.LogDebug("processing fourth result set - auto de-auth details", Me.GetType())
                If results.NextResult() Then
                    ' Process the result set
                    WriteResultsToFile(StoreUpdatesBO, results, ChangeType.ItemDataDeAuth)
                End If

                posPushJobTimer.Stop()
                stepTimeInSeconds = CLng((posPushJobTimer.ElapsedMilliseconds/1000L))
                Console.WriteLine("Time taken for processing Item Deletes Application Step in POS Push: " + stepTimeInSeconds.ToString())
                Logger.LogInfo("Time taken for processing Item Deletes Application Step in POS Push: " + stepTimeInSeconds.ToString(), Me.GetType())

                Logger.LogDebug("done processing all results", Me.GetType())
            Catch e As Exception
                'common exceptions would be DataFactory or File I/O exceptions
                Throw
            Finally
                ' Close the result set and the connection
                If (results IsNot Nothing) Then
                    results.Close()
                End If
            End Try
            Logger.LogDebug("RetrieveChangeRecordsFromIRMA exit", Me.GetType())
        End Sub

        ''' <summary>
        ''' Updates the database records in IRMA.
        ''' Executes the UpdatePriceBatchProcessedDel stored procedure to update the change records.
        ''' </summary>
        ''' <param name="StoreUpdatesBO"></param>
        ''' <remarks></remarks>
        Overrides Sub ApplyChangesInIRMA(ByRef StoreUpdatesBO As Hashtable)
            Logger.LogDebug("ApplyChangesInIRMA entry", Me.GetType())
            Dim errors As New StringBuilder
            Dim deAuthErrors As New StringBuilder
            Dim HeaderIds As New DataTable
            Dim StoreItemAuthorizationIds As New DataTable

            With HeaderIds.Columns
                .Add("PriceBatchHeaderId", GetType(Integer))
                .Add("BatchId", GetType(Integer))
            End With

           
            With StoreItemAuthorizationIds.Columns
                .Add("Id", GetType(Integer))
            End With

            Try
                If (StoreUpdatesBO.Count > 0) Then
                    Dim storeEnum As IEnumerator = StoreUpdatesBO.Values.GetEnumerator()
                    Dim currentStore As StoreUpdatesBO
                    Dim sortedHeaders As ArrayList
                    Dim changesEnum As IEnumerator
                    Dim headerData As POSBatchHeaderBO

                    ' Process each of the stores that have changes
                    While storeEnum.MoveNext()
                        ' For each store, update the price batch header records associated with item deletes
                        currentStore = CType(storeEnum.Current, StoreUpdatesBO)

                        If currentStore.ChangesDelivered AndAlso currentStore.POSDeleteItemHeaders.Count > 0 Then
                            ' Sort the price batch header records based on the batch header id so that they db changes
                            ' are applied in the correct order.
                            sortedHeaders = New ArrayList(currentStore.POSDeleteItemHeaders().Keys)
                            sortedHeaders.Sort()
                            changesEnum = sortedHeaders.GetEnumerator()

                            While changesEnum.MoveNext()
                                headerData = CType(currentStore.POSDeleteItemHeaders().Item(changesEnum.Current), POSBatchHeaderBO)
                                HeaderIds.Rows.Add(CType(headerData.BatchID, Integer), CType(currentStore.BatchID, Integer))
                            End While
                        End If

                        Try
                            If (HeaderIds.Rows.Count > 0) Then 
                                ' Execute the stored procedure to update the PriceBatchHeader and delete the 
                                ' PriceBatchDetail records
                                POSWriterDAO.ApplyItemDeletes(HeaderIds)
                            End If

                        Catch ex As Exception
                            Logger.LogError(" Exception: ", Me.GetType(), ex)
                        End Try

                        If currentStore.ChangesDelivered AndAlso currentStore.POSItemDeAuthorizations.Count > 0 Then
                            ' For each store, update each of the records associated with de-auths
                            changesEnum = currentStore.POSItemDeAuthorizations.GetEnumerator()

                            While changesEnum.MoveNext()
                                StoreItemAuthorizationIds.Rows.Add(CType(changesEnum.Current, Integer))
                            End While

                            Try
                                If (StoreItemAuthorizationIds.Rows.Count > 0) Then
                                    ' Execute the stored procedure to update the StoreItem record
                                    POSWriterDAO.UpdateDeAuthorizedPOSChanges(StoreItemAuthorizationIds)
                                End If

                            Catch ex As Exception
                                Logger.LogError(" Exception: ", Me.GetType(), ex)
                            End Try

                        End If

                    End While
                End If
            Catch e1 As Exception
                Logger.LogError(" Exception: ", Me.GetType(), e1)

                Dim args(2) As String
                args(0) = "POSItemDeletes"
                args(1) = "Unexpected error has occurred"
                ErrorHandler.ProcessError(ErrorType.POSPush_ApplyChangesInIRMA, args, SeverityLevel.Fatal, e1)
            Finally
                'handle errors if any in errors StringBuilder
                If errors.Length > 0 Then
                    Dim args(2) As String
                    args(0) = "POSItemDeletes"
                    args(1) = "There were errors processing the following batches: " + errors.ToString
                    ErrorHandler.ProcessError(ErrorType.POSPush_ApplyChangesInIRMA, args, SeverityLevel.Fatal)
                End If

                If deAuthErrors.Length > 0 Then
                    Dim args(2) As String
                    args(0) = "POSItemDeletes"
                    args(1) = "There were errors processing the following StoreItemAuthorizationID values: " + errors.ToString
                    ErrorHandler.ProcessError(ErrorType.POSPush_ApplyDeAuthChangesInIRMA, args, SeverityLevel.Fatal)
                End If
            End Try
            Logger.LogDebug("ApplyChangesInIRMA exit", Me.GetType())
        End Sub

        ''' <summary>
        ''' Each record that is added to the POS Push file by WriteResultsToFile will contain key data,
        ''' such as database IDs, that need to be stored for ApplyChangesInIRMA.  
        ''' Parse this data from the result record and add it to the StoreUpdatesBO object.
        ''' 
        ''' There is logic based on the FromBatchHeader flag.  It is set to true for Item Deletes that are batched
        ''' and we are processing the header result set.  It is set to false for the details result set because 
        ''' the columns available for reading are different.
        ''' </summary>
        ''' <param name="currentStore"></param>
        ''' <param name="result"></param>
        ''' <remarks></remarks>
        Public Overloads Sub PopulateApplyChangesData(ByRef currentStore As StoreUpdatesBO, ByRef result As SqlDataReader, ByVal FromBatchHeader As Boolean)
            Dim headerInfo As New POSBatchHeaderBO
            Dim batchID As Int32
            Dim batchDate As Date
            Dim autoApply As Boolean
            Dim applyDate As Date = Nothing
            Dim batchDesc As String = Nothing

            If FromBatchHeader Then
                ' This is the header record for a batched change.  
                ' Add it to the POSDeleteItemHeaders collection.
                If (Not result.IsDBNull(result.GetOrdinal("PriceBatchHeaderID"))) Then
                    batchID = result.GetInt32(result.GetOrdinal("PriceBatchHeaderID"))
                End If

                If (Not result.IsDBNull(result.GetOrdinal("AutoApplyFlag"))) Then
                    autoApply = result.GetBoolean(result.GetOrdinal("AutoApplyFlag"))
                End If

                If (Not result.IsDBNull(result.GetOrdinal("StartDate"))) Then
                    batchDate = result.GetDateTime(result.GetOrdinal("StartDate"))
                End If

                If (Not result.IsDBNull(result.GetOrdinal("ApplyDate"))) Then
                    applyDate = result.GetDateTime(result.GetOrdinal("ApplyDate"))
                End If

                If (Not result.IsDBNull(result.GetOrdinal("BatchDescription"))) Then
                    batchDesc = result.GetString(result.GetOrdinal("BatchDescription"))
                End If

                If (Not result.IsDBNull(result.GetOrdinal("POSBatchId"))) Then
                    headerInfo.POSBatchId = result.GetInt32(result.GetOrdinal("POSBatchId")).ToString
                Else
                    headerInfo.POSBatchId = Nothing
                End If

                headerInfo.BatchID = batchID
                headerInfo.BatchDate = batchDate
                headerInfo.ApplyDate = applyDate
                headerInfo.AutoApply = autoApply

                If batchDesc IsNot Nothing AndAlso Not batchDesc.Trim.Equals("") Then
                    headerInfo.BatchDesc = batchDesc
                Else
                    headerInfo.BatchDesc = "ITEM DELETE " + batchDate.ToString("MM/dd")
                End If

                headerInfo.StoreNo = result.GetInt32(result.GetOrdinal("Store_No"))

                currentStore.POSDeleteItemHeaders.Add(batchID, headerInfo)
            Else
                ' Check to see if this detailed result set is associated with a de-authorization change.
                ' If it is, add it to the POSItemDeAuthorizations collection.
                If (Not result.IsDBNull(result.GetOrdinal("StoreItemAuthorizationID"))) Then
                    currentStore.POSItemDeAuthorizations.Add(result.GetInt32(result.GetOrdinal("StoreItemAuthorizationID")))
                End If
            End If
        End Sub

        ''' <summary>
        ''' Each record that is added to the POS Push file by WriteResultsToFile will contain key data,
        ''' such as database IDs, that need to be stored for ApplyChangesInIRMA.  
        ''' Parse this data from the result record and add it to the StoreUpdatesBO object.
        ''' </summary>
        ''' <param name="currentStore"></param>
        ''' <param name="result"></param>
        ''' <remarks></remarks>
        Public Overrides Sub PopulateApplyChangesData(ByRef currentStore As StoreUpdatesBO, ByRef result As SqlDataReader)
            ' Default the FromBatchHeader flag to FALSE
            PopulateApplyChangesData(currentStore, result, False)
        End Sub

    End Class

End Namespace
