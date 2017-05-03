Imports System.Data.SqlClient
Imports System.Text
Imports WholeFoods.IRMA.Replenishment.Common.DataAccess
Imports WholeFoods.IRMA.Replenishment.Common.Writers
Imports WholeFoods.IRMA.Replenishment.POSPush.BusinessLogic
Imports WholeFoods.IRMA.Replenishment.ScalePush.DataAccess
Imports WholeFoods.Utility

Namespace WholeFoods.IRMA.Replenishment.ScalePush.Controller

    ''' <summary>
    ''' Subclasses ScaleProcessor, providing the details for the ZoneScalePriceChange ChangeType.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class ZoneScalePriceChangeProcessor
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
        ''' Reads the database records from IRMA, adding the results to the POS Push file for each store.
        ''' Executes the GetPriceBatchSent stored procedure to retrieve the change records.
        ''' </summary>
        ''' <param name="storeUpdates"></param>
        ''' <remarks></remarks>
        ''' <exception cref="WholeFoods.Utility.DataAccess.DataFactory" />
        Overrides Sub RetrieveChangeRecordsFromIRMA(ByRef storeUpdates As Hashtable, ByVal isRegionalScaleFile As Boolean, Optional ByVal storeNo As String = Nothing)
            Logger.LogDebug("RetrieveChangeRecordsFromIRMA entry", Me.GetType())

            'if regional scale file then data comes from 2 places, vs just 1
            If isRegionalScaleFile Then
                'REGIONAL FILE --> get ZONE data - price changes from GetPriceBatchSent
                RetrieveZoneChangeRecordsFromIRMA(storeUpdates)
            Else
                'STORE SPECIFIC FILES --> get ITEM & PRICE changes from GetPriceBatchSent
                RetrieveStoreChangeRecordsFromIRMA(storeUpdates)
            End If

            Logger.LogDebug("RetrieveChangeRecordsFromIRMA exit", Me.GetType())
        End Sub

        ''' <summary>
        ''' gets the price changes for a STORE scale file;
        ''' </summary>
        ''' <param name="storeUpdates"></param>
        ''' <remarks></remarks>
        Overrides Sub RetrieveStoreChangeRecordsFromIRMA(ByRef storeUpdates As Hashtable)
            Logger.LogDebug("RetrieveStoreChangeRecordsFromIRMA entry", Me.GetType())

            Dim results As SqlDataReader = Nothing
            Dim scalePushJobTimer As Stopwatch
            Dim stepTimeInSeconds As Long = 0

            Try
                scalePushJobTimer = Stopwatch.StartNew()

                ' Read the price batch changes that are ready to be sent to the stores.
                ' results are limited to scale items & price changes only
                results = ScaleWriterDAO.GetItemDataChanges(dStart)

                scalePushJobTimer.Stop()
                stepTimeInSeconds = CLng((scalePushJobTimer.ElapsedMilliseconds/1000L))
                Console.WriteLine("Time taken for Zone Price Change for Stores DB Step : " + stepTimeInSeconds.ToString())
                Logger.LogInfo("Time taken for Zone Price Change for Stores DB Step : " + stepTimeInSeconds.ToString(), Me.GetType())
                scalePushJobTimer = Stopwatch.StartNew()

                ' -- First resultset - list of price changes and their details
                Logger.LogDebug("processing first result set - item/price details", Me.GetType())
                ' Process the result set
                WriteResultsToFile(storeUpdates, results, ChangeType.ZoneScalePriceChange, False, Constants.ScaleWriterType_Zone)

                scalePushJobTimer.Stop()
                stepTimeInSeconds = CLng((scalePushJobTimer.ElapsedMilliseconds/1000L))
                Console.WriteLine("Time taken for Zone Price Change for Stores Application Step : " + stepTimeInSeconds.ToString())
                Logger.LogInfo("Time taken for Zone Price Change for Stores Application Step : " + stepTimeInSeconds.ToString(), Me.GetType())
            Finally
                ' Close the result set and the connection
                If (results IsNot Nothing) Then
                    results.Close()
                End If
            End Try

            Logger.LogDebug("RetrieveStoreChangeRecordsFromIRMA exit", Me.GetType())
        End Sub

        ''' <summary>
        ''' gets the ZONE price changes for a regional scale file;
        ''' gets data from GetPriceBatchSent where change type = PRICE
        ''' </summary>
        ''' <param name="storeUpdates"></param>
        ''' <remarks></remarks>
        Overrides Sub RetrieveZoneChangeRecordsFromIRMA(ByRef storeUpdates As Hashtable)
            Logger.LogDebug("RetrieveZoneChangeRecordsFromIRMA entry", Me.GetType())
            Dim results As SqlDataReader = Nothing
            Dim scalePushJobTimer As Stopwatch
            Dim stepTimeInSeconds As Long = 0

            Try
                ' The price changes are in a different format if the store is using the SmartX writer
                ' instead of the other Scale writers.
                If (Not InstanceDataDAO.IsFlagActive("UseSmartXPriceData")) Then
                    ' Use the default writer
                    ' Read the price batch changes that are ready to be sent to the stores.
                    ' results are limited to scale items & price changes only
                    scalePushJobTimer = Stopwatch.StartNew()

                    results = ScaleWriterDAO.GetItemDataChanges(dStart)

                    scalePushJobTimer.Stop()
                    stepTimeInSeconds = CLng((scalePushJobTimer.ElapsedMilliseconds/1000L))
                    Console.WriteLine("Time taken for Zone Scale Price Change Records DB Step : " + stepTimeInSeconds.ToString())
                    Logger.LogInfo("Time taken for Zone Scale Price Change Records DB Step : " + stepTimeInSeconds.ToString(), Me.GetType())
                    scalePushJobTimer = Stopwatch.StartNew()

                    ' -- First resultset - list of price changes and their details
                     WriteResultsToFile(storeUpdates, results, ChangeType.ZoneScalePriceChange, True, Constants.ScaleWriterType_Zone)

                    ' -- Second resultset - newly de-authorized price changes
                    ' -- (sent to the corporate scale systems as a $0 price change)
                    Logger.LogDebug("processing fourth result set - item/price details for auto de-auth batches", Me.GetType())
                    If results.NextResult() Then
                        ' Process the result set
                        WriteResultsToFile(storeUpdates, results, ChangeType.ZoneScaleItemDeAuthPriceChange, True, Constants.ScaleWriterType_Zone)
                    End If

                    scalePushJobTimer.Stop()
                    stepTimeInSeconds = CLng((scalePushJobTimer.ElapsedMilliseconds/1000L))
                    Console.WriteLine("Time taken for Zone Scale Price Change Records Application Step : " + stepTimeInSeconds.ToString())
                    Logger.LogInfo("Time taken for Zone Scale Price Change Records Application Step : " + stepTimeInSeconds.ToString(), Me.GetType())
                Else
                    scalePushJobTimer = Stopwatch.StartNew()

                    ' Use the Smart X Writer
                    ' Read the price batch changes that are ready to be sent to the stores.
                    ' results are limited to scale items & price changes only
                    results = ScaleWriterDAO.GetSmartXPriceChanges(dStart)

                    scalePushJobTimer.Stop()
                    stepTimeInSeconds = CLng((scalePushJobTimer.ElapsedMilliseconds/1000L))
                    Console.WriteLine("Time taken for Zone Scale Price Change Records DB Step : " + stepTimeInSeconds.ToString())
                    scalePushJobTimer = Stopwatch.StartNew()

                    ' -- First resultset - list of price changes and their details
                    Logger.LogDebug("processing first result set - item/price details", Me.GetType())
                    ' Process the result set
                    WriteResultsToFile(storeUpdates, results, ChangeType.ZoneScaleSmartXPriceChange, True, Constants.ScaleWriterType_SmartXZone)

                    ' -- Second resultset - list of auth/de-auth records being processed
                    Dim currentStoreUpdate As StoreUpdatesBO = CType(storeUpdates.Item(Constants.ScaleWriterType_SmartXZone), StoreUpdatesBO)
                    If results.NextResult() Then
                        ' Populate the array lists so that the auth/de-auth flags can be turned off
                        While results.Read()
                            If Not results.IsDBNull(results.GetOrdinal("ScaleAuth")) Then
                                If results.GetBoolean(results.GetOrdinal("ScaleAuth")) Then
                                    currentStoreUpdate.ScaleAuthorizations.Add(results.GetInt32(results.GetOrdinal("StoreItemAuthorizationID")))
                                End If
                            End If
                            If Not results.IsDBNull(results.GetOrdinal("ScaleDeAuth")) Then
                                If results.GetBoolean(results.GetOrdinal("ScaleDeAuth")) Then
                                    currentStoreUpdate.ScaleDeAuthorizations.Add(results.GetInt32(results.GetOrdinal("StoreItemAuthorizationID")))
                                End If
                            End If
                        End While
                    End If
                    scalePushJobTimer.Stop()
                    stepTimeInSeconds = CLng((scalePushJobTimer.ElapsedMilliseconds/1000L))
                    Console.WriteLine("Time taken for Zone Scale Price Change Records Application Step : " + stepTimeInSeconds.ToString())

                End If
            Catch ex As Exception
                'common exceptions would be DataFactory or File I/O exceptions
                Throw
            Finally
                ' Close the result set and the connection
                If (results IsNot Nothing) Then
                    results.Close()
                End If
            End Try
            Logger.LogDebug("RetrieveZoneChangeRecordsFromIRMA exit", Me.GetType())
        End Sub

        ''' <summary>
        ''' Reset the authorization and de-authorization flags for the scale auth/de-auths that were communicated to the
        ''' scales by this run of scale push.
        ''' </summary>
        ''' <param name="storeUpdates"></param>
        ''' <remarks></remarks>
        Public Overrides Sub ApplyChangesInIRMA(ByRef storeUpdates As Hashtable)
            Logger.LogDebug("ApplyChangesInIRMA entry", Me.GetType())
            Dim authErrors As New StringBuilder
            Dim deAuthErrors As New StringBuilder

            Try
                ' Process the changes for the "zone" store
                Dim currentStore As StoreUpdatesBO
                If storeUpdates.ContainsKey(Constants.ScaleWriterType_Zone) Then
                    ' This was a corporate configuration, so the auth and de-auth records should be reset during
                    ' the processing of price changes.
                    currentStore = CType(storeUpdates.Item(Constants.ScaleWriterType_Zone), StoreUpdatesBO)
                    Dim changesEnum As IEnumerator

                    ' Process the changes for the store
                    If currentStore.ChangesDelivered AndAlso currentStore.ScaleAuthorizations.Count > 0 Then
                        changesEnum = currentStore.ScaleAuthorizations.GetEnumerator()

                        While changesEnum.MoveNext()
                            Try
                                ' Execute the stored procedure to update the StoreItem record
                                ScaleWriterDAO.UpdateAuthorizedScaleChanges(CType(changesEnum.Current, Integer))
                            Catch ex As Exception
                                Logger.LogError(" Exception: ", Me.GetType(), ex)

                                'track list of ids (w/ store) that cause error and report errors
                                authErrors.Append("Store: ")
                                authErrors.Append(currentStore.StoreNum)
                                authErrors.Append("   StoreItemAuthorizationID: ")
                                authErrors.Append(changesEnum.Current)
                                authErrors.Append(Environment.NewLine)
                            End Try
                        End While
                    End If

                    If currentStore.ChangesDelivered AndAlso currentStore.ScaleDeAuthorizations.Count > 0 Then
                        changesEnum = currentStore.ScaleDeAuthorizations.GetEnumerator()

                        While changesEnum.MoveNext()
                            Try
                                ' Execute the stored procedure to update the StoreItem record
                                ScaleWriterDAO.UpdateDeAuthorizedScaleChanges(CType(changesEnum.Current, Integer))
                            Catch ex As Exception
                                Logger.LogError(" Exception: ", Me.GetType(), ex)

                                'track list of ids (w/ store) that cause error and report errors
                                deAuthErrors.Append("Store: ")
                                deAuthErrors.Append(currentStore.StoreNum)
                                deAuthErrors.Append("   StoreItemAuthorizationID: ")
                                deAuthErrors.Append(changesEnum.Current)
                                deAuthErrors.Append(Environment.NewLine)
                            End Try
                        End While
                    End If
                End If
            Catch e1 As Exception
                Logger.LogError(" Exception: ", Me.GetType(), e1)

                Dim args(2) As String
                args(0) = "ZoneScalePriceChangeProcessor"
                args(1) = "There were errors processing the following authorization StoreItemAuthorizationID values: " + authErrors.ToString + " and/or de-authorization StoreItemAuthorizationID values: " + deAuthErrors.ToString
                ErrorHandler.ProcessError(ErrorType.ScalePush_ApplyDeAuthChangesInIRMA, args, SeverityLevel.Fatal, e1)
            Finally
                'handle errors if any in errors StringBuilder
                If authErrors.Length > 0 Then
                    Dim args(2) As String
                    args(0) = "ZoneScalePriceChangeProcessor"
                    args(1) = "There were errors processing the following StoreItemAuthorizationID values: " + authErrors.ToString
                    ErrorHandler.ProcessError(ErrorType.ScalePush_ApplyAuthChangesInIRMA, args, SeverityLevel.Fatal)
                End If

                If deAuthErrors.Length > 0 Then
                    Dim args(2) As String
                    args(0) = "ZoneScalePriceChangeProcessor"
                    args(1) = "There were errors processing the following StoreItemAuthorizationID values: " + deAuthErrors.ToString
                    ErrorHandler.ProcessError(ErrorType.ScalePush_ApplyDeAuthChangesInIRMA, args, SeverityLevel.Fatal)
                End If
            End Try

            Logger.LogDebug("ApplyChangesInIRMA exit", Me.GetType())
        End Sub
    End Class
End Namespace