Imports System.Data.SqlClient
Imports System.Text
Imports WholeFoods.IRMA.Replenishment.Common.DataAccess
Imports WholeFoods.IRMA.Replenishment.Common.Writers
Imports WholeFoods.IRMA.Replenishment.POSPush.BusinessLogic
Imports WholeFoods.IRMA.Replenishment.ScalePush.DataAccess
Imports WholeFoods.Utility

Namespace WholeFoods.IRMA.Replenishment.ScalePush.Controller

    ''' <summary>
    ''' Subclasses ScaleProcessor, providing the details for the ZoneScaleItemDelete ChangeType.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class ZoneScaleItemDeleteProcessor
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
                'REGIONAL FILE --> get ZONE data - item deletes from GetPriceBatchSent
                RetrieveZoneChangeRecordsFromIRMA(storeUpdates)
            Else
                'STORE SPECIFIC FILES --> get item deletes from GetPriceBatchSent
                RetrieveStoreChangeRecordsFromIRMA(storeUpdates)
            End If

            Logger.LogDebug("RetrieveChangeRecordsFromIRMA exit", Me.GetType())
        End Sub

        ''' <summary>
        ''' gets the item deletes/changes for a STORE scale file;
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
                ' results are limited to scale items & deletes only
                results = ScaleWriterDAO.GetItemDeletes(dStart)

                scalePushJobTimer.Stop()
                stepTimeInSeconds = CLng((scalePushJobTimer.ElapsedMilliseconds/1000L))
                Console.WriteLine("Time taken for Zone Scale Item Deletes for Stores DB Step : " + stepTimeInSeconds.ToString())
                Logger.LogInfo("Time taken for Zone Scale Item Deletes for Stores DB Step : " + stepTimeInSeconds.ToString(), Me.GetType())
                scalePushJobTimer = Stopwatch.StartNew()

                ' -- First resultset - list of price changes and their details
                WriteResultsToFile(storeUpdates, results, ChangeType.ZoneScaleItemDelete, False, Constants.ScaleWriterType_Zone)

                ' -- Second resultset - list of de-auth items and their details
                Logger.LogDebug("processing second result set - auto de-auth records", Me.GetType())
                If results.NextResult() Then
                    ' Process the result set
                    WriteResultsToFile(storeUpdates, results, ChangeType.ZoneScaleItemDelete, False, Constants.ScaleWriterType_Zone)
                End If

                scalePushJobTimer.Stop()
                stepTimeInSeconds = CLng((scalePushJobTimer.ElapsedMilliseconds/1000L))
                Console.WriteLine("Time taken for Zone Scale Item Delete for Stores Application Step : " + stepTimeInSeconds.ToString())
                Logger.LogInfo("Time taken for Zone Scale Item Delete for Stores Application Step : " + stepTimeInSeconds.ToString(), Me.GetType())
            Finally
                ' Close the result set and the connection
                If (results IsNot Nothing) Then
                    results.Close()
                End If
            End Try
            Logger.LogDebug("RetrieveStoreChangeRecordsFromIRMA exit", Me.GetType())
        End Sub

        ''' <summary>
        ''' gets the ZONE item deletes for a regional scale file;
        ''' gets data from GetPriceBatchSent where
        ''' </summary>
        ''' <param name="storeUpdates"></param>
        ''' <remarks></remarks>
        Overrides Sub RetrieveZoneChangeRecordsFromIRMA(ByRef storeUpdates As Hashtable)
            Logger.LogDebug("RetrieveZoneChangeRecordsFromIRMA entry", Me.GetType())
            Dim results As SqlDataReader = Nothing
            Dim scalePushJobTimer As Stopwatch
            Dim stepTimeInSeconds As Long = 0

            Try
                scalePushJobTimer = Stopwatch.StartNew()

                ' Read the price batch changes that are ready to be sent to the stores.
                ' results are limited to scale items & deletes only
                results = ScaleWriterDAO.GetItemDeletes(dStart)

                scalePushJobTimer.Stop()
                stepTimeInSeconds = CLng((scalePushJobTimer.ElapsedMilliseconds/1000L))
                Console.WriteLine("Time taken for Zone Scale Item Deletes Change Records DB Step : " + stepTimeInSeconds.ToString())
                Logger.LogInfo("Time taken for Zone Scale Item Deletes Change Records DB Step : " + stepTimeInSeconds.ToString(), Me.GetType())
                scalePushJobTimer = Stopwatch.StartNew()

                ' -- First resultset - list of price changes and their details
                Logger.LogDebug("processing first result set - item/price details", Me.GetType())
                If results.NextResult() Then
                    ' Process the result set
                    If (Not InstanceDataDAO.IsFlagActive("UseSmartXPriceData")) Then
                        WriteResultsToFile(storeUpdates, results, ChangeType.ZoneScaleItemDelete, True, Constants.ScaleWriterType_Zone)
                    Else
                        WriteResultsToFile(storeUpdates, results, ChangeType.ZoneScaleItemDelete, True, Constants.ScaleWriterType_SmartXZone)
                    End If
                End If

                ' -- Fourth resultset of de-auth records is not used for corporate scale systems
                scalePushJobTimer.Stop()
                stepTimeInSeconds = CLng((scalePushJobTimer.ElapsedMilliseconds/1000L))
                Console.WriteLine("Time taken for Zone Scale Item Deletes Change Records Application Step : " + stepTimeInSeconds.ToString())
                Logger.LogInfo("Time taken for Zone Scale Item Deletes Change Records Application Step : " + stepTimeInSeconds.ToString(), Me.GetType())
            Catch ex As Exception
                'common exceptions would be DataFactory or File I/O exceptions
                Throw ex
            Finally
                ' Close the result set and the connection
                If (results IsNot Nothing) Then
                    results.Close()
                End If
            End Try
            Logger.LogDebug("RetrieveZoneChangeRecordsFromIRMA exit", Me.GetType())
        End Sub

        ''' <summary>
        ''' Reset the de-authorization flag for the scale de-authorizations that were communicated to the
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
                        ' For each store, update each of the records associated with scale de-auths
                        currentStore = CType(storeEnum.Current, StoreUpdatesBO)

                        If currentStore.ChangesDelivered AndAlso currentStore.ScaleDeAuthorizations.Count > 0 Then
                            changesEnum = currentStore.ScaleDeAuthorizations.GetEnumerator()

                            While changesEnum.MoveNext()
                                Try
                                    ' Execute the stored procedure to update the StoreItem record
                                    ScaleWriterDAO.UpdateDeAuthorizedScaleChanges(CType(changesEnum.Current, Integer))
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
                args(0) = "ZoneScaleItemDelete"
                args(1) = "There were errors processing the following StoreItemAuthorizationID values: " + errors.ToString
                ErrorHandler.ProcessError(ErrorType.ScalePush_ApplyDeAuthChangesInIRMA, args, SeverityLevel.Fatal, e1)
            Finally
                'handle errors if any in errors StringBuilder
                If errors.Length > 0 Then
                    Dim args(2) As String
                    args(0) = "ZoneScaleItemDelete"
                    args(1) = "There were errors processing the following StoreItemAuthorizationID values: " + errors.ToString
                    ErrorHandler.ProcessError(ErrorType.ScalePush_ApplyDeAuthChangesInIRMA, args, SeverityLevel.Fatal)
                End If
            End Try

            Logger.LogDebug("ApplyChangesInIRMA exit", Me.GetType())
        End Sub
    End Class
End Namespace