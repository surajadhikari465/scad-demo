Imports System.Collections
Imports System.Data.SqlClient
Imports System.Text
Imports WholeFoods.IRMA.Replenishment.Common.Writers
Imports WholeFoods.IRMA.Replenishment.POSPush.BusinessLogic
Imports WholeFoods.IRMA.Replenishment.POSPush.DataAccess
Imports WholeFoods.Utility

Namespace WholeFoods.IRMA.Replenishment.POSPush.Controller

    ''' <summary>
    ''' Subclasses POSProcessor, providing the details for the ItemIdDelete ChangeType.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class POSItemIdDeletesProcessor
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
        ''' Executes the POSGetIdentifierDeletes stored procedure to retrieve the change records.
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

                ' Execute the stored procedure to read the item id deletes 
                results = POSWriterDAO.GetItemIdDeletes(dStart)

                posPushJobTimer.Stop()      
                stepTimeInSeconds = CLng((posPushJobTimer.ElapsedMilliseconds/1000L))
                Console.WriteLine("Time taken for processing ItemID Deletes DB Step in POS Push: " + stepTimeInSeconds.ToString())
                Logger.LogInfo("Time taken for processing ItemID Deletes DB Step in POS Push: " + stepTimeInSeconds.ToString(), Me.GetType())

                posPushJobTimer = Stopwatch.StartNew()

                ' -- First resultset - data of delete items to write to file
                Logger.LogDebug("processing first result set - item details", Me.GetType())
                WriteResultsToFile(StoreUpdatesBO, results, ChangeType.ItemIdDelete)

                posPushJobTimer.Stop()
                stepTimeInSeconds = CLng((posPushJobTimer.ElapsedMilliseconds/1000L))
                Console.WriteLine("Time taken for processing ItemID Deletes Application Step in POS Push: " + stepTimeInSeconds.ToString())
                Logger.LogInfo("Time taken for processing ItemID Deletes Application Step in POS Push: " + stepTimeInSeconds.ToString(), Me.GetType())

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
        ''' Executes the POSDeleteIdentifier stored procedure to update the change records.
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
                        ' For each store, update each of the records associated with item id deletes
                        currentStore = CType(storeEnum.Current, StoreUpdatesBO)

                        If currentStore.ChangesDelivered AndAlso currentStore.POSItemIdDeletes.Count > 0 Then
                            changesEnum = currentStore.POSItemIdDeletes.GetEnumerator()

                            While changesEnum.MoveNext()
                                IdentifierIds.Rows.Add(CType(changesEnum.Current, Integer))
                            End While
                        End If
                    End While

                    Try
                        If (IdentifierIds.Rows.Count > 0) Then
                            POSWriterDAO.ApplyItemIdDeletes(IdentifierIds)
                        End If
                    Catch ex As Exception
                        Logger.LogError(" Exception: ", Me.GetType(), ex)
                    End Try

                End If
            Catch e1 As Exception
                Logger.LogError(" Exception: ", Me.GetType(), e1)

                Dim args(2) As String
                args(0) = "POSItemIdDeletes"
                args(1) = "Unexpected error has occurred"
                ErrorHandler.ProcessError(ErrorType.POSPush_ApplyChangesInIRMA, args, SeverityLevel.Fatal, e1)
            Finally
                'handle errors if any in errors StringBuilder
                If errors.Length > 0 Then
                    Dim args(2) As String
                    args(0) = "POSItemIdDeletes"
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
            currentStore.POSItemIdDeletes.Add(result.GetInt32(result.GetOrdinal("Identifier_ID")))
        End Sub

    End Class

End Namespace
