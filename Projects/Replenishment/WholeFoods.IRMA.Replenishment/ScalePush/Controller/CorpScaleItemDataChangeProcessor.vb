Imports System.Data.SqlClient
Imports WholeFoods.IRMA.Replenishment.Common.DataAccess
Imports WholeFoods.IRMA.Replenishment.Common.Writers
Imports WholeFoods.IRMA.Replenishment.ScalePush.DataAccess
Imports WholeFoods.Utility

Namespace WholeFoods.IRMA.Replenishment.ScalePush.Controller

    ''' <summary>
    ''' Subclasses ScaleProcessor, providing the details for the ItemDataChange ChangeType.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CorpScaleItemDataChangeProcessor
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
                'REGIONAL FILE --> get CORPORATE data - item changes from GetPLUMCorpChg
                RetrieveCorporateChangeRecordsFromIRMA(storeUpdates)
            Else
                'STORE SPECIFIC FILES --> get ITEM & PRICE changes from GetPriceBatchSent
                RetrieveStoreChangeRecordsFromIRMA(storeUpdates)
            End If

            Logger.LogDebug("RetrieveChangeRecordsFromIRMA exit", Me.GetType())
        End Sub

        ''' <summary>
        ''' gets the item data changes for a STORE scale file;
        ''' </summary>
        ''' <param name="storeUpdates"></param>
        ''' <remarks></remarks>
        Overrides Sub RetrieveStoreChangeRecordsFromIRMA(ByRef storeUpdates As Hashtable)
            Logger.LogDebug("RetrieveStoreChangeRecordsFromIRMA entry", Me.GetType())

            Dim results As SqlDataReader = Nothing
            Dim tempStoreUpdates As New Hashtable
            Dim firstStoreNum As Integer = Nothing
            Dim scalePushJobTimer As Stopwatch
            Dim stepTimeInSeconds As Long = 0

            Try
                scalePushJobTimer = Stopwatch.StartNew()

                'get item data changes; ActionCode = 'C'
                results = PLUMCorpChgDAO.GetCorporateDataChanges(Constants.ActionCode_ItemChange, dStart, Nothing)

                scalePushJobTimer.Stop()
                stepTimeInSeconds = CLng((scalePushJobTimer.ElapsedMilliseconds/1000L))
                Console.WriteLine("Time taken for Get Scale Change Records DB Step : " + stepTimeInSeconds.ToString())
                Logger.LogInfo("Time taken for Get Scale Change Recordss DB Step : " + stepTimeInSeconds.ToString(), Me.GetType())

                scalePushJobTimer = Stopwatch.StartNew()

                'there is only one result set to process for store configurations
                Logger.LogDebug("processing plum result set", Me.GetType())
                WriteResultsToFile(storeUpdates, results, ChangeType.CorpScaleItemChange, False)

                scalePushJobTimer.Stop()
                stepTimeInSeconds = CLng((scalePushJobTimer.ElapsedMilliseconds/1000L))
                Console.WriteLine("Time taken for Get Scale Change Records Application Step : " + stepTimeInSeconds.ToString())
                Logger.LogInfo("Time taken for Get Scale Change Records Application Step : " + stepTimeInSeconds.ToString(), Me.GetType())

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
        ''' gets data from GetPLUMCorpChg where ActionCode = 'C'
        ''' </summary>
        ''' <param name="storeUpdates"></param>
        ''' <remarks></remarks>
        Overrides Sub RetrieveCorporateChangeRecordsFromIRMA(ByRef storeUpdates As Hashtable)
            Logger.LogDebug("RetrieveCorporateChangeRecordsFromIRMA entry", Me.GetType())
            Dim results As SqlDataReader = Nothing
            Dim scalePushJobTimer As Stopwatch
            Dim stepTimeInSeconds As Long = 0

            Try
                scalePushJobTimer = Stopwatch.StartNew()

                'get item data changes; ActionCode = 'C'
                results = PLUMCorpChgDAO.GetCorporateDataChanges(Constants.ActionCode_ItemChange, dStart, Nothing)

                scalePushJobTimer.Stop()
                stepTimeInSeconds = CLng((scalePushJobTimer.ElapsedMilliseconds/1000L))
                Console.WriteLine("Time taken for ItemChanges Get Corp Change Records DB Step : " + stepTimeInSeconds.ToString())
                Logger.LogInfo("Time taken for ItemChanges Get Corp Change Records DB Step : " + stepTimeInSeconds.ToString(), Me.GetType())

                scalePushJobTimer = Stopwatch.StartNew()

                ' Process the result set
                ' -- First resultset - list of corporate change records 
                Logger.LogDebug("processing first result set - corporate records", Me.GetType())
                WriteResultsToFile(storeUpdates, results, ChangeType.CorpScaleItemChange, True, Constants.ScaleWriterType_Corporate)

                ' -- Second resultset - list of price zone records to be included with the corporate changes
                If (Not InstanceDataDAO.IsFlagActive("UseSmartXPriceData")) Then
                    Logger.LogDebug("processing second result set - price zones for corporate records", Me.GetType())
                    If results.NextResult() Then
                        ' This is a corporate change, but the current prices are formatted based on the zone writer definitons for 
                        ' price changes so the Zone Writer is used here.
                        WriteResultsToFile(storeUpdates, results, ChangeType.CorpScalePriceExceptions, True, Constants.ScaleWriterType_Corporate)
                    End If
                End If

                scalePushJobTimer.Stop()
                stepTimeInSeconds = CLng((scalePushJobTimer.ElapsedMilliseconds/1000L))
                Console.WriteLine("Time taken for Item Changes Get Corp Change Records Application Step : " + stepTimeInSeconds.ToString())
                Logger.LogInfo("Time taken for Item Changes Get Corp Change Records Application Step : " + stepTimeInSeconds.ToString(), Me.GetType())
            Catch ex As Exception
                'common exceptions would be DataFactory or File I/O exceptions
                Throw
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