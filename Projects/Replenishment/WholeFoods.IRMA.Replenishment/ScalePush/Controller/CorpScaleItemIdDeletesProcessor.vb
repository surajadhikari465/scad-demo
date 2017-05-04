Imports System.Data.SqlClient
Imports WholeFoods.IRMA.Replenishment.Common.Writers
Imports WholeFoods.IRMA.Replenishment.ScalePush.DataAccess
Imports WholeFoods.Utility

Namespace WholeFoods.IRMA.Replenishment.ScalePush.Controller

    ''' <summary>
    ''' Subclasses ScaleProcessor, providing the details for the ItemIdDelete ChangeType.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CorpScaleItemIdDeletesProcessor
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
        ''' Executes the POSGetIdentifierDeletes stored procedure to retrieve the change records.
        ''' </summary>
        ''' <param name="storeUpdates"></param>
        ''' <remarks></remarks>
        Overrides Sub RetrieveChangeRecordsFromIRMA(ByRef storeUpdates As Hashtable, ByVal isRegionalScaleFile As Boolean, Optional ByVal storeNo As String = Nothing)
            Logger.LogDebug("RetrieveChangeRecordsFromIRMA entry", Me.GetType())

            If isRegionalScaleFile Then
                'REGIONAL FILE --> get CORPORATE data - item id deletes from GetPLUMCorpChg
                RetrieveCorporateChangeRecordsFromIRMA(storeUpdates)
            Else
                'STORE SPECIFIC FILES --> get item id deletes
                RetrieveStoreChangeRecordsFromIRMA(storeUpdates)
            End If

            Logger.LogDebug("RetrieveChangeRecordsFromIRMA exit", Me.GetType())
        End Sub

        ''' <summary>
        ''' gets the item id deletes for a STORE scale file;
        ''' </summary>
        ''' <param name="storeUpdates"></param>
        ''' <remarks></remarks>
        Overrides Sub RetrieveStoreChangeRecordsFromIRMA(ByRef storeUpdates As Hashtable)
            Logger.LogDebug("RetrieveStoreChangeRecordsFromIRMA entry", Me.GetType())

            Dim scalePushJobTimer As Stopwatch
            Dim stepTimeInSeconds As Long = 0
            Dim results As SqlDataReader = Nothing
            Dim tempStoreUpdates As New Hashtable

            Try
                scalePushJobTimer = Stopwatch.StartNew()
                'get item id deletes
                results = PLUMCorpChgDAO.GetCorporateItemIdDeletes(dStart)

                scalePushJobTimer.Stop()
                stepTimeInSeconds = CLng((scalePushJobTimer.ElapsedMilliseconds/1000L))
                Console.WriteLine("Time taken for ItemID Deletes Get Store Change Records DB Step : " + stepTimeInSeconds.ToString())
                Logger.LogInfo("Time taken for ItemID Deletes Get Store Change Records DB Step : " + stepTimeInSeconds.ToString(), Me.GetType())

                scalePushJobTimer = Stopwatch.StartNew()

                ' Process the result set
                ' -- First resultset - list of tax hosting data for all deleted items
                Logger.LogDebug("skipping first result set - tax hosting", Me.GetType())

                ' -- Second resultset - data of delete items to write to file
                Logger.LogDebug("processing second result set - item details", Me.GetType())

                If results.NextResult() Then
                    WriteResultsToFile(storeUpdates, results, ChangeType.CorpScaleItemIdDelete, False)
                End If

                scalePushJobTimer.Stop()
                stepTimeInSeconds = CLng((scalePushJobTimer.ElapsedMilliseconds/1000L))
                Console.WriteLine("Time taken for ItemID Deletes Records Application Step : " + stepTimeInSeconds.ToString())
                Logger.LogInfo("Time taken for ItemID Deletes Records Application Step : " + stepTimeInSeconds.ToString(), Me.GetType())
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
        ''' gets data from GetPLUMCorpChg where ActionCode = 'D'
        ''' </summary>
        ''' <param name="storeUpdates"></param>
        ''' <remarks></remarks>
        Overrides Sub RetrieveCorporateChangeRecordsFromIRMA(ByRef storeUpdates As Hashtable)
            Logger.LogDebug("RetrieveCorporateChangeRecordsFromIRMA entry", Me.GetType())

            Dim scalePushJobTimer As Stopwatch
            Dim stepTimeInSeconds As Long = 0
            Dim results As SqlDataReader = Nothing

            Try
                scalePushJobTimer = Stopwatch.StartNew()
                'get item id deletes
                results = PLUMCorpChgDAO.GetCorporateItemIdDeletes(dStart)

                scalePushJobTimer.Stop()
                stepTimeInSeconds = CLng((scalePushJobTimer.ElapsedMilliseconds/1000L))
                Console.WriteLine("Time taken for ItemID Deletes Get Corp Change Records DB Step : " + stepTimeInSeconds.ToString())
                Logger.LogInfo("Time taken for ItemID Deletes Get Corp Change Records DB Step : " + stepTimeInSeconds.ToString(), Me.GetType())

                scalePushJobTimer = Stopwatch.StartNew()

                ' Process the result set
                ' -- First resultset - list of tax hosting data for all deleted items
                Logger.LogDebug("skipping first result set - tax hosting", Me.GetType())

                ' -- Second resultset - data of delete items to write to file
                If results.NextResult() Then
                    Logger.LogDebug("processing second result set - item id deletes", Me.GetType())
                    WriteResultsToFile(storeUpdates, results, ChangeType.CorpScaleItemIdDelete, True, Constants.ScaleWriterType_Corporate)
                End If

                scalePushJobTimer.Stop()
                stepTimeInSeconds = CLng((scalePushJobTimer.ElapsedMilliseconds/1000L))
                Console.WriteLine("Time taken for ItemID Deletes Get Corp Change Records Application Step : " + stepTimeInSeconds.ToString())
                Logger.LogInfo("Time taken for ItemID Deletes Get Corp Change Records Application Step : " + stepTimeInSeconds.ToString(), Me.GetType())
            Finally
                ' Close the result set and the connection
                If (results IsNot Nothing) Then
                    results.Close()
                End If
            End Try

            Logger.LogDebug("RetrieveCorporateChangeRecordsFromIRMA exit", Me.GetType())
        End Sub
    End Class
End Namespace