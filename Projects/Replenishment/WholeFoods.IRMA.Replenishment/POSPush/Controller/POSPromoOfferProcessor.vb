Imports System.Collections
Imports System.Data.SqlClient
Imports System.Text
Imports WholeFoods.IRMA.Replenishment.Common.Writers
Imports WholeFoods.IRMA.Replenishment.POSPush.BusinessLogic
Imports WholeFoods.IRMA.Replenishment.POSPush.DataAccess
Imports WholeFoods.Utility

Namespace WholeFoods.IRMA.Replenishment.POSPush.Controller

    ''' <summary>
    ''' Subclasses POSProcessor, providing the details for the Promotional Offer ChangeType.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class POSPromoOfferProcessor
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
        ''' Executes the Replenishment_POSPush_GetPriceBatchOffers stored procedure to retrieve the change records.
        ''' </summary>
        ''' <param name="StoreUpdatesBO"></param>
        ''' <remarks></remarks>
        ''' <exception cref="WholeFoods.Utility.DataAccess.DataFactory" />
        Overrides Sub RetrieveChangeRecordsFromIRMA(ByRef StoreUpdatesBO As Hashtable, ByVal isAuditReport As Boolean, Optional ByVal storeNo As String = Nothing)
            Logger.LogDebug("RetrieveChangeRecordsFromIRMA entry", Me.GetType())
            Dim results As SqlDataReader = Nothing
            Dim posPushJobTimer As Stopwatch
            Dim stepTimeInSeconds As Long = 0

            Try
                posPushJobTimer = Stopwatch.StartNew()

                ' Read the price batch changes that are ready to be sent to the stores.
                results = POSWriterDAO.GetPromoOfferDataChanges(dStart)

                posPushJobTimer.Stop()      
                stepTimeInSeconds = CLng((posPushJobTimer.ElapsedMilliseconds/1000L))
                Console.WriteLine("Time taken for processing Promo Offers DB Step in POS Push: " + stepTimeInSeconds.ToString())
                Logger.LogInfo("Time taken for processing Promo Offers DB Step in POS Push: " + stepTimeInSeconds.ToString(), Me.GetType())

                posPushJobTimer = Stopwatch.StartNew()

                ' -- First resultset - list of promo offers and their details
                Logger.LogDebug("processing first result set - promo offers", Me.GetType())
                ' Process the result set
                WriteResultsToFile(StoreUpdatesBO, results, ChangeType.PromoOffer)

                posPushJobTimer.Stop()
                stepTimeInSeconds = CLng((posPushJobTimer.ElapsedMilliseconds/1000L))
                Console.WriteLine("Time taken for processing Promo Offers Application Step in POS Push: " + stepTimeInSeconds.ToString())
                Logger.LogInfo("Time taken for processing Promo Offers Application Step in POS Push: " + stepTimeInSeconds.ToString(), Me.GetType())

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
        ''' Executes the UpdatePriceBatchProcessedChg stored procedure to update the change records.
        ''' </summary>
        ''' <param name="StoreUpdatesBO"></param>
        ''' <remarks></remarks>
        Overrides Sub ApplyChangesInIRMA(ByRef StoreUpdatesBO As Hashtable)
            Logger.LogDebug("ApplyChangesInIRMA entry", Me.GetType())
            Dim errors As New StringBuilder

            Try
                If (StoreUpdatesBO.Count > 0) Then
                    Dim storeEnum As IEnumerator = StoreUpdatesBO.Values.GetEnumerator()
                    Dim currentStore As StoreUpdatesBO
                    Dim sortedHeaders As ArrayList
                    Dim changesEnum As IEnumerator
                    Dim headerData As POSBatchHeaderBO

                    ' Process each of the stores that have changes
                    While storeEnum.MoveNext()
                        ' For each store, update the price batch header records associated with promo offers
                        currentStore = CType(storeEnum.Current, StoreUpdatesBO)

                        If currentStore.ChangesDelivered AndAlso currentStore.POSPromoOfferHeaders.Count > 0 Then
                            ' Sort the price batch header records based on the batch header id so that they db changes
                            ' are applied in the correct order.
                            sortedHeaders = New ArrayList(currentStore.POSPromoOfferHeaders().Keys)
                            sortedHeaders.Sort()
                            changesEnum = sortedHeaders.GetEnumerator()

                            While changesEnum.MoveNext()
                                headerData = CType(currentStore.POSPromoOfferHeaders().Item(changesEnum.Current), POSBatchHeaderBO)

                                Try
                                    ' Execute the stored procedure to update the PriceBatchHeader and PromotionalStoreOffer tables
                                    POSWriterDAO.ApplyPromoOffers(headerData, currentStore)
                                Catch ex As Exception
                                    Logger.LogError(" Exception: ", Me.GetType(), ex)

                                    'track list of batches (w/ store) that cause error and report errors
                                    errors.Append("Store: ")
                                    errors.Append(currentStore.StoreNum)
                                    errors.Append(" PriceBatchHeaderID: ")
                                    errors.Append(headerData.BatchID)
                                    errors.Append(Environment.NewLine)
                                End Try
                            End While
                        End If
                    End While
                End If
            Catch e1 As Exception
                Logger.LogError(" Exception: ", Me.GetType(), e1)

                Dim args(2) As String
                args(0) = "POSPromoOffer"
                args(1) = "Unexpected error has occurred"
                ErrorHandler.ProcessError(ErrorType.POSPush_ApplyChangesInIRMA, args, SeverityLevel.Fatal, e1)
            Finally
                'handle errors if any in errors StringBuilder
                If errors.Length > 0 Then
                    Dim args(2) As String
                    args(0) = "POSPromoOffer"
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
            Dim batchDate As Date

            If (Not result.IsDBNull(result.GetOrdinal("PriceBatchHeaderID"))) Then
                batchID = result.GetInt32(result.GetOrdinal("PriceBatchHeaderID"))
            End If

            If (Not result.IsDBNull(result.GetOrdinal("StartDate"))) Then
                batchDate = result.GetDateTime(result.GetOrdinal("StartDate"))
            End If

            headerInfo.BatchID = batchID
            headerInfo.BatchDate = batchDate
            headerInfo.BatchDesc = "PROMO OFFER " + batchDate.ToString("MM/dd")
            headerInfo.StoreNo = result.GetInt32(result.GetOrdinal("Store_No"))

            currentStore.POSPromoOfferHeaders.Add(batchID, headerInfo)
        End Sub

    End Class

End Namespace
