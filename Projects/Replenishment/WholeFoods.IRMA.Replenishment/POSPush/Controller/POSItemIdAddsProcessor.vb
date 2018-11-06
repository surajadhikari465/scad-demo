Imports System.Collections
Imports System.Data.SqlClient
Imports System.Text
Imports WholeFoods.IRMA.Replenishment.Common.Writers
Imports WholeFoods.IRMA.Replenishment.POSPush.BusinessLogic
Imports WholeFoods.IRMA.Replenishment.POSPush.DataAccess
Imports WholeFoods.Utility

Namespace WholeFoods.IRMA.Replenishment.POSPush.Controller

    ''' <summary>
    ''' Subclasses POSProcessor, providing the details for the ItemIdAdd ChangeType.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class POSItemIdAddsProcessor
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
        ''' Executes the POSGetIdentifierAdds stored procedure to retrieve the change records.
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

                If isAuditReport Then
                    ' execute stored procedure to get all item data for a store
                    results = POSWriterDAO.GetAuditReportData(dStart, CType(storeNo, Integer))
                Else
                    ' Execute the stored procedure to read the item id adds 
                    results = POSWriterDAO.GetItemIdAdds(dStart)
                End If

                posPushJobTimer.Stop()      
                stepTimeInSeconds = CLng((posPushJobTimer.ElapsedMilliseconds/1000L))
                Console.WriteLine("Time taken for processing ItemID Adds DB Step in POS Push: " + stepTimeInSeconds.ToString())
                Logger.LogInfo("Time taken for processing ItemID Adds DB Step in POS Push: " + stepTimeInSeconds.ToString(), Me.GetType())

                posPushJobTimer = Stopwatch.StartNew()

                If isAuditReport Then
                    ' -- First resultset - list of tax hosting data for all added items
                    Logger.LogDebug("processing first result set - tax hosting", Me.GetType())
                    ' Process the result set
                    PopulateTaxHostingData(StoreUpdatesBO, results)

                    ' -- Second resultset - data of add items to write to file
                    Logger.LogDebug("processing second result set - item details", Me.GetType())
                    If results.NextResult() Then
                        ' Process the result set
                        WriteResultsToFile(StoreUpdatesBO, results, ChangeType.ItemIdAdd)
                    End If
                Else
                    ' -- First resultset - data of add items to write to file
                    Logger.LogDebug("processing first result set - item details", Me.GetType())
                    ' Process the result set
                    WriteResultsToFile(StoreUpdatesBO, results, ChangeType.ItemIdAdd)
                End If

                posPushJobTimer.Stop()
                stepTimeInSeconds = CLng((posPushJobTimer.ElapsedMilliseconds/1000L))
                Console.WriteLine("Time taken for processing ItemID Adds Application Step in POS Push: " + stepTimeInSeconds.ToString())
                Logger.LogInfo("Time taken for processing ItemID Adds Application Step in POS Push: " + stepTimeInSeconds.ToString(), Me.GetType())

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
        ''' Executes the POSAddIdentifier stored procedure to update the change records.
        ''' </summary>
        ''' <param name="StoreUpdatesBO"></param>
        ''' <remarks></remarks>
        Overrides Sub ApplyChangesInIRMA(ByRef StoreUpdatesBO As Hashtable)
            Logger.LogDebug("ApplyChangesInIRMA entry", Me.GetType())
            Dim errors As New StringBuilder
            Dim IdentifierIds As New DataTable
            With IdentifierIds.Columns
                .Add("IdentifierId", GetType(Integer))
            End With

            Try
                If (StoreUpdatesBO.Count > 0) Then
                    Dim storeEnum As IEnumerator = StoreUpdatesBO.Values.GetEnumerator()
                    Dim currentStore As StoreUpdatesBO
                    Dim changesEnum As IEnumerator

                    ' Process each of the stores that have changes
                    While storeEnum.MoveNext()
                        ' For each store, update each of the records associated with item id adds
                        currentStore = CType(storeEnum.Current, StoreUpdatesBO)

                        If currentStore.ChangesDelivered AndAlso currentStore.POSItemIdAdds.Count > 0 Then
                            changesEnum = currentStore.POSItemIdAdds.GetEnumerator()

                            While changesEnum.MoveNext()
                                IdentifierIds.Rows.Add(CType(changesEnum.Current, Integer))
                            End While
                        End If
                    End While

                    Try
                        If (IdentifierIds.Rows.Count  > 0)  Then
                            ' Execute the stored procedure to update the item identifier record
                            ' for the Identifier_ID, setting Add_Identifier = 0 (false)
                            POSWriterDAO.ApplyItemIdAdds(IdentifierIds)
                        End If
                    Catch ex As Exception
                        Logger.LogError(" Exception: ", Me.GetType(), ex)
                    End Try

                End If
            Catch e1 As Exception
                Logger.LogError(" Exception: ", Me.GetType(), e1)

                Dim args(2) As String
                args(0) = "POSItemIdAdds"
                args(1) = "Unexpected error has occurred"
                ErrorHandler.ProcessError(ErrorType.POSPush_ApplyChangesInIRMA, args, SeverityLevel.Fatal, e1)
            Finally
                'handle errors if any in errors StringBuilder
                If errors.Length > 0 Then
                    Dim args(2) As String
                    args(0) = "POSItemIdAdds"
                    args(1) = "There were errors processing the following identifiers: " + errors.ToString
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
            currentStore.POSItemIdAdds.Add(result.GetInt32(result.GetOrdinal("Identifier_ID")))
        End Sub

    End Class

End Namespace
