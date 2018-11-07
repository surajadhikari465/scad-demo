Imports System.Data.SqlClient
Imports System.Text
Imports WholeFoods.IRMA.Replenishment.Common.Writers
Imports WholeFoods.IRMA.Replenishment.POSPush.BusinessLogic
Imports WholeFoods.IRMA.Replenishment.POSPush.DataAccess
Imports WholeFoods.Utility

Namespace WholeFoods.IRMA.Replenishment.POSPush.Controller

    ''' <summary>
    ''' Subclasses POSProcessor, providing the details for the VendorIdAdd ChangeType.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class POSVendorIdAddsProcessor
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

            Dim posPushJobTimer As Stopwatch
            Dim stepTimeInSeconds As Long = 0

            posPushJobTimer = Stopwatch.StartNew()

            Dim results As SqlDataReader = Nothing

            Try
                posPushJobTimer = Stopwatch.StartNew()

                ' Execute the stored procedure to read the vendor id adds 
                results = POSWriterDAO.GetVendorAdds(isAuditReport, storeNo)

                posPushJobTimer.Stop()      
                stepTimeInSeconds = CLng((posPushJobTimer.ElapsedMilliseconds/1000L))
                Console.WriteLine("Time taken for processing vendor ID Adds DB Step in POS Push: " + stepTimeInSeconds.ToString())

                posPushJobTimer = Stopwatch.StartNew()

                ' -- First resultset - data of add vendors to write to file .
                Logger.LogDebug("processing first result set - item details", Me.GetType())
                If results.HasRows Then
                    WriteResultsToFile(StoreUpdatesBO, results, ChangeType.VendorIDAdd)
                End If

                posPushJobTimer.Stop()
                stepTimeInSeconds = CLng((posPushJobTimer.ElapsedMilliseconds/1000L))
                Console.WriteLine("Time taken for processing vendor ID Adds Application Step in POS Push: " + stepTimeInSeconds.ToString())

                Logger.LogDebug("done processing all results", Me.GetType())
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
        ''' Executes the POSAddVendor stored procedure to update the change records.
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
                    Dim changesEnum As IEnumerator

                    ' Process each of the stores that have changes
                    While storeEnum.MoveNext()
                        ' For each store, update each of the records associated with vendor id adds
                        currentStore = CType(storeEnum.Current, StoreUpdatesBO)

                        If currentStore.ChangesDelivered AndAlso currentStore.POSVendorIdAdds.Count > 0 Then
                            changesEnum = currentStore.POSVendorIdAdds.GetEnumerator()

                            While changesEnum.MoveNext()
                                Try
                                    ' Execute the stored procedure to update the vendor record
                                    ' for the Vendor_ID, setting AddVendor = 0 (false)
                                    POSWriterDAO.ApplyVendorIdAdds(CType(changesEnum.Current, Integer))
                                Catch ex As Exception
                                    Logger.LogError(" Exception: ", Me.GetType(), ex)

                                    'track list of vendor ids (w/ store) that cause error and report errors
                                    errors.Append("Store: ")
                                    errors.Append(currentStore.StoreNum)
                                    errors.Append(" Vendor_ID: ")
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
                args(0) = "POSVendorIdAdds"
                args(1) = "Unexpected error has occurred"
                ErrorHandler.ProcessError(ErrorType.POSPush_ApplyChangesInIRMA, args, SeverityLevel.Fatal, e1)
            Finally
                'handle errors if any in errors StringBuilder
                If errors.Length > 0 Then
                    Dim args(2) As String
                    args(0) = "POSVendorIdAdds"
                    args(1) = "There were errors processing the following vendor ids: " + errors.ToString
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
            currentStore.POSVendorIdAdds.Add(result.GetInt32(result.GetOrdinal("Vendor_ID")))
        End Sub

    End Class

End Namespace
