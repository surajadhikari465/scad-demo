Imports System.Collections
Imports System.Data.SqlClient
Imports System.Text
Imports WholeFoods.IRMA.Replenishment.Common.Writers
Imports WholeFoods.IRMA.Replenishment.POSPush.BusinessLogic
Imports WholeFoods.IRMA.Replenishment.POSPush.DataAccess
Imports WholeFoods.Utility

Namespace WholeFoods.IRMA.Replenishment.POSPush.Controller

    Public Enum StoreDataRowType
        None = 0
        POSPriceChangeHeaders= 1
        POSDeleteItemHeaders = 2
        POSPromoOfferHeaders = 3
    End Enum
    ''' <summary>
    ''' Subclasses POSProcessor, providing the details for the ItemDataDelete ChangeType.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class POSBatchDataForStoresProcessor
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
        ''' Executes the Replenishment_POSPush_GetBatchDataForStores stored procedure to retrieve the data
        ''' for the stores associated with the batches.
        ''' </summary>
        ''' <param name="StoreUpdatesBO"></param>
        ''' <remarks></remarks>
        Overrides Sub RetrieveChangeRecordsFromIRMA(ByRef StoreUpdatesBO As Hashtable, ByVal isAuditReport As Boolean, Optional ByVal storeNo As String = Nothing)
            Logger.LogDebug("RetrieveChangeRecordsFromIRMA entry", Me.GetType())
            Dim result As SqlDataReader = Nothing
            Dim currentStoreNum As Integer
            Dim currentStoreUpdate As StoreUpdatesBO
            Dim posPushJobTimer As Stopwatch
            Dim stepTimeInSeconds As Long = 0

            Try
                posPushJobTimer = Stopwatch.StartNew()

                ' Read the price batch deletes that are ready to be sent to the stores.
                result = POSWriterDAO.GetBatchStores(dStart)

                posPushJobTimer.Stop()      
                stepTimeInSeconds = CLng((posPushJobTimer.ElapsedMilliseconds/1000L))
                Console.WriteLine("Time taken for processing stores in batches DB Step in POS Push: " + stepTimeInSeconds.ToString())
                Logger.LogInfo("Time taken for processing stores in batches DB Step in POS Push: " + stepTimeInSeconds.ToString(), Me.GetType())

                posPushJobTimer = Stopwatch.StartNew()

                Logger.LogDebug("processing first result set - Stores data", Me.GetType())
                While result.Read()
                    ' get the store # for this record
                    currentStoreNum = result.GetInt32(result.GetOrdinal("Store_No"))

                    ' verify this store is configured in StorePOSConfig
                    If (StoreUpdatesBO.ContainsKey(currentStoreNum)) Then
                        currentStoreUpdate = CType(StoreUpdatesBO.Item(currentStoreNum), StoreUpdatesBO)
                        PopulateApplyChangesDataForStoresInBatches(currentStoreUpdate, result)
                    Else
                        'ERROR PROCESSING ... all stores should be found in the hash
                        Logger.LogWarning("Error processing a record returned by Replenishment_POSPush_GetBatchDataForStores.  Store # not configured in StorePOSConfig table: " & currentStoreNum, Me.GetType())

                        'send message about exception
                        Dim args(1) As String
                        args(0) = currentStoreNum.ToString
                        ErrorHandler.ProcessError(ErrorType.POSPush_StoreNotFound, args, SeverityLevel.Warning)

                        Continue While
                    End If
                End While

                posPushJobTimer.Stop()
                stepTimeInSeconds = CLng((posPushJobTimer.ElapsedMilliseconds/1000L))
                Console.WriteLine("Time taken for processing stores in batches Application Step in POS Push: " + stepTimeInSeconds.ToString())
                Logger.LogInfo("Time taken for processing stores in batches Application Step in POS Push: " + stepTimeInSeconds.ToString(), Me.GetType())

                Logger.LogDebug("Done processing all results", Me.GetType())
            Catch e As Exception
                'common exceptions would be DataFactory or File I/O exceptions
                Throw
            Finally
                ' Close the result set and the connection
                If (result IsNot Nothing) Then
                    result.Close()
                End If
            End Try
            Logger.LogDebug("RetrieveChangeRecordsFromIRMA exit", Me.GetType())
        End Sub
        ''' <summary>
        ''' Each record that is added to the POS Push file by WriteResultsToFile will contain key data,
        ''' such as database IDs, that need to be stored for ApplyChangesInIRMA.  
        ''' Parse this data from the result record and add it to the StoreUpdatesBO object.
        ''' 
        ''' This function takes in each row returned from the Replenishment_POSPush_GetBatchDataForStores procedure
        ''' and then writes into the POSPriceChangeHeaders or POSDeleteItemHeaders or POSPromoOfferHeaders objects 
        ''' based on the value in the RowType column field
        ''' </summary>
        ''' <param name="currentStore"></param>
        ''' <param name="result"></param>
        ''' <remarks></remarks>
        Private  Sub PopulateApplyChangesDataForStoresInBatches(ByRef currentStore As StoreUpdatesBO, ByRef result As SqlDataReader)
            Dim headerInfo As New POSBatchHeaderBO
            Dim batchID As Int32
            Dim batchDate As Date
            Dim storeDataRowType As StoreDataRowType = Controller.StoreDataRowType.None
            Dim batchDesc As String = Nothing

            If (Not result.IsDBNull(result.GetOrdinal("PriceBatchHeaderID"))) Then
                batchID = result.GetInt32(result.GetOrdinal("PriceBatchHeaderID"))
            End If

            If (Not result.IsDBNull(result.GetOrdinal("AutoApplyFlag"))) Then
                headerInfo.AutoApply = result.GetBoolean(result.GetOrdinal("AutoApplyFlag"))
            End If

            If (Not result.IsDBNull(result.GetOrdinal("StartDate"))) Then
                batchDate = result.GetDateTime(result.GetOrdinal("StartDate"))
            End If

            If (Not result.IsDBNull(result.GetOrdinal("ApplyDate"))) Then
                headerInfo.ApplyDate = result.GetDateTime(result.GetOrdinal("ApplyDate"))
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

            If batchDesc IsNot Nothing AndAlso Not batchDesc.Trim.Equals("") Then
                headerInfo.BatchDesc = batchDesc
            End If

            headerInfo.StoreNo = result.GetInt32(result.GetOrdinal("Store_No"))
            'the rowType column contains the type of batch detail record for the row
            storeDataRowType = CType([Enum].Parse(GetType(StoreDataRowType), result.GetString(result.GetOrdinal("RowType"))), StoreDataRowType)

            Select Case storeDataRowType
                Case StoreDataRowType.POSDeleteItemHeaders:
                    If (String.IsNullOrEmpty(headerInfo.BatchDesc) = True)
                         headerInfo.BatchDesc = "ITEM DELETE " + batchDate.ToString("MM/dd")
                    End If
                    currentStore.POSDeleteItemHeaders.Add(batchID, headerInfo)
                Case StoreDataRowType.POSPriceChangeHeaders:
                    If (String.IsNullOrEmpty(headerInfo.BatchDesc) = True)
                        headerInfo.BatchDesc = "ITEM CHANGE " + batchDate.ToString("MM/dd")
                    End If
                    currentStore.POSPriceChangeHeaders.Add(batchID, headerInfo)
                Case StoreDataRowType.POSPromoOfferHeaders:
                    If (String.IsNullOrEmpty(headerInfo.BatchDesc) = True)
                         headerInfo.BatchDesc = "PROMO OFFER " + batchDate.ToString("MM/dd")
                    End If
                    
                    currentStore.POSPromoOfferHeaders.Add(batchID, headerInfo)                 
            End Select
        End Sub

        Public Overrides Sub PopulateApplyChangesData(ByRef currentStore As StoreUpdatesBO, ByRef result As SqlDataReader)
            Throw New NotImplementedException()
        End Sub

        Public Overrides Sub ApplyChangesInIRMA(ByRef StoreUpdatesBO As Hashtable)
            Throw New NotImplementedException()
        End Sub
    End Class
End Namespace
