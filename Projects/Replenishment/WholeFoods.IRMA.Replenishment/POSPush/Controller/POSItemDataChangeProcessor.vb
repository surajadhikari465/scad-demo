Imports System.Data.SqlClient
Imports System.Text
Imports WholeFoods.IRMA.Replenishment.Common.DataAccess
Imports WholeFoods.IRMA.Replenishment.Common.Writers
Imports WholeFoods.IRMA.Replenishment.POSPush.BusinessLogic
Imports WholeFoods.IRMA.Replenishment.POSPush.DataAccess
Imports WholeFoods.Utility

Namespace WholeFoods.IRMA.Replenishment.POSPush.Controller

    ''' <summary>
    ''' Subclasses POSProcessor, providing the details for the ItemDataChange ChangeType.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class POSItemDataChangeProcessor
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
        ''' <exception cref="WholeFoods.Utility.DataAccess.DataFactory" />
        Overrides Sub RetrieveChangeRecordsFromIRMA(ByRef StoreUpdatesBO As Hashtable, ByVal isAuditReport As Boolean, Optional ByVal storeNo As String = Nothing)
            Logger.LogDebug("RetrieveChangeRecordsFromIRMA entry", Me.GetType())

            Dim results As SqlDataReader = Nothing
            Dim resultTable As New DataTable
            Dim writeToFileOK As Boolean = True
            Dim posPushJobTimer As Stopwatch
            Dim stepTimeInSeconds As Long = 0

            Try
                ' Off Promo Cost records are sent to the stores as part of the POS Push process.  These records can be created
                ' and batched ahead of time.  Between the time they are in a "Sent" batch and the POS Push process runs, IRMA will
                ' allow users to make updates to the Item data.  
                ' To prevent old Item information from being sent to the POS, all Off Promo Cost PDB records need to be updated
                ' with current Item info before POS picks them up.
                POSWriterDAO.UpdatePromoOffCostDetails(dStart)

                Dim enableIconItemNonBatchableChanges As Boolean = InstanceDataDAO.IsFlagActive("EnableIconItemNonBatchableChanges")

                posPushJobTimer = Stopwatch.StartNew()
                ' Read the price batch changes that are ready to be sent to the stores.
                results = POSWriterDAO.GetItemDataChanges(dStart, enableIconItemNonBatchableChanges)

                posPushJobTimer.Stop()      
                stepTimeInSeconds = CLng((posPushJobTimer.ElapsedMilliseconds/1000L))
                Console.WriteLine("Time taken for processing Item and Price Changes DB Step in POS Push: " + stepTimeInSeconds.ToString())
                Logger.LogInfo("Time taken for processing Item and Price Changes DB Step in POS Push: " + stepTimeInSeconds.ToString(), Me.GetType())

                posPushJobTimer = Stopwatch.StartNew()

                ' -- First resultset - list of change items and their details
                ' -- The first resultset contains the data only for the legacy stores.
                ' -- The data for the R10 stores is written directly to the IconPOSPushStaging table
                Logger.LogDebug("processing first result set - item details", Me.GetType())
                ' Process the result set
                WriteResultsToFile(StoreUpdatesBO, results, ChangeType.ItemDataChange)

                ' -- Second resultset - list of non batchable item changes
                If enableIconItemNonBatchableChanges Then
                    Logger.LogDebug("processing second result set - non batchable item changes", Me.GetType())

                    If results.NextResult() Then
                        WriteResultsToFile(StoreUpdatesBO, results, ChangeType.ItemDataChange, True)
                    End If
                End If

                posPushJobTimer.Stop()
                stepTimeInSeconds = CLng((posPushJobTimer.ElapsedMilliseconds/1000L))
                Console.WriteLine("Time taken for processing Item and Price Changes Application Step in POS Push: " + stepTimeInSeconds.ToString())
                Logger.LogInfo("Time taken for processing Item and Price Changes Application Step in POS Push: " + stepTimeInSeconds.ToString(), Me.GetType())

                Logger.LogDebug("done processing all results", Me.GetType())
            Catch e As Exception
                'Do not write to the denorm table if there's an erro when writing to the file.
                writeToFileOK = False

                'common exceptions would be DataFactory or File I/O exceptions
                Logger.LogError("RetrieveChangeRecordsFromIRMA - exception during processing", Me.GetType(), e)
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
        ''' Executes the UpdatePriceBatchProcessedChg stored procedure to update the change records.
        ''' </summary>
        ''' <param name="StoreUpdatesBO"></param>
        ''' <remarks></remarks>
        Overrides Sub ApplyChangesInIRMA(ByRef StoreUpdatesBO As Hashtable)
            Logger.LogDebug("ApplyChangesInIRMA entry", Me.GetType())
            Dim errors As New StringBuilder
            Dim HeaderIds As New DataTable
            With HeaderIds.Columns
                .Add("PriceBatchHeaderId", GetType(Integer))
                .Add("BatchId", GetType(Integer))
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
                        ' For each store, update the price batch header records associated with
                        ' item changes
                        currentStore = CType(storeEnum.Current, StoreUpdatesBO)

                        If currentStore.ChangesDelivered AndAlso currentStore.POSPriceChangeHeaders.Count > 0 Then
                            ' Sort the price batch header records based on the batch header id so that they db changes
                            ' are applied in the correct order.
                            sortedHeaders = New ArrayList(currentStore.POSPriceChangeHeaders().Keys)
                            sortedHeaders.Sort()
                            changesEnum = sortedHeaders.GetEnumerator()

                            'Gather the PriceBatchHeaderids and the POSBatchIds to be updated from the procedure Replenishment_POSPush_UpdatePriceBatchProcessedChg
                            While changesEnum.MoveNext()
                                headerData = CType(currentStore.POSPriceChangeHeaders().Item(changesEnum.Current), POSBatchHeaderBO)
                                HeaderIds.Rows.Add(CType(headerData.BatchID, Integer), CType(currentStore.BatchID, Integer))
                            End While
                        End If
                    End While

                    Try
                        If (HeaderIds.Rows.Count > 0) Then
                            ' Execute the stored procedure to update the PriceBatchHeader and update the 
                            ' Price and SignQueue records
                            POSWriterDAO.ApplyItemDataChanges(HeaderIds)
                        End If
                    Catch ex As Exception
                        Logger.LogError(" Exception: ", Me.GetType(), ex)
                    End Try
                End If

                If InstanceDataDAO.IsFlagActive("EnableIconItemNonBatchableChanges") Then
                    POSWriterDAO.DeleteItemNonBatchableChanges(dStart)
                End If

            Catch e1 As Exception
                Logger.LogError(" Exception: ", Me.GetType(), e1)

                Dim args(2) As String
                args(0) = "POSItemDataChange"
                args(1) = "Unexpected error has occurred"
                ErrorHandler.ProcessError(ErrorType.POSPush_ApplyChangesInIRMA, args, SeverityLevel.Fatal, e1)
            Finally
                'handle errors if any in errors StringBuilder
                If errors.Length > 0 Then
                    Dim args(2) As String
                    args(0) = "POSItemDataChange"
                    args(1) = "There were errors processing the following batches: " + errors.ToString
                    ErrorHandler.ProcessError(ErrorType.POSPush_ApplyChangesInIRMA, args, SeverityLevel.Fatal)
                End If
            End Try
            Logger.LogDebug("ApplyChangesInIRMA exit", Me.GetType())
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
            Dim headerInfo As New POSBatchHeaderBO
            Dim batchID As Int32
            Dim autoApply As Boolean
            Dim batchDate As Date
            Dim applyDate As Date = Nothing
            Dim batchDesc As String = Nothing

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
                headerInfo.BatchDesc = "ITEM CHANGE " + batchDate.ToString("MM/dd")
            End If

            headerInfo.StoreNo = result.GetInt32(result.GetOrdinal("Store_No"))

            currentStore.POSPriceChangeHeaders.Add(batchID, headerInfo)
        End Sub

    End Class

End Namespace
