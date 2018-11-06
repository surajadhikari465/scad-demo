Imports System.Collections
Imports System.Data.SqlClient
Imports System.Text
Imports WholeFoods.IRMA.Replenishment.Common.Writers
Imports WholeFoods.IRMA.Replenishment.POSPush.BusinessLogic
Imports WholeFoods.IRMA.Replenishment.POSPush.DataAccess
Imports WholeFoods.Utility

Namespace WholeFoods.IRMA.Replenishment.POSPush.Controller

    ''' <summary>
    ''' Subclasses POSProcessor, providing the details for the ItemDataDelete ChangeType.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class POSTaxFlagsProcessor
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
        ''' Executes the Replenishment_POSPush_GetAllTaxflagData stored procedure to retrieve the data
        ''' for the stores associated with the batches.
        ''' </summary>
        ''' <param name="StoreUpdatesBO"></param>
        ''' <remarks></remarks>
        Overrides Sub RetrieveChangeRecordsFromIRMA(ByRef StoreUpdatesBO As Hashtable, ByVal isAuditReport As Boolean, Optional ByVal storeNo As String = Nothing)
            Logger.LogDebug("RetrieveChangeRecordsFromIRMA entry", Me.GetType())
            Dim result As SqlDataReader = Nothing
            Dim posPushJobTimer As Stopwatch
            Dim stepTimeInSeconds As Long = 0
            Dim currentStoreUpdate As StoreUpdatesBO = Nothing
            Dim currentStoreNum As Integer = -1
            Dim currentItemKey As Integer = -1
            Dim currentTaxFlag As TaxFlagBO

            Try
                posPushJobTimer = Stopwatch.StartNew()

                ' Read the tax fags data
                result = POSWriterDAO.GetTaxFlags(dStart)

                posPushJobTimer.Stop()      
                stepTimeInSeconds = CLng((posPushJobTimer.ElapsedMilliseconds/1000L))
                Console.WriteLine("Time taken for processing Tax Flags Data DB Step in POS Push: " + stepTimeInSeconds.ToString())
                Logger.LogInfo("Time taken for processing Tax Flags Data DB Step in POS Push: " + stepTimeInSeconds.ToString(), Me.GetType())

                posPushJobTimer = Stopwatch.StartNew()

                Logger.LogDebug("Processing result set for Tax flags", Me.GetType())
                ' results contains an entry for each item being added to the POS Push file that 
                ' is configured in the tax hosting tables
                While result.Read()
                    ' get the store # for this record
                    currentStoreNum = result.GetInt32(result.GetOrdinal("Store_No"))
                    ' get the item key for the record
                    currentItemKey = result.GetInt32(result.GetOrdinal("Item_Key"))
                    ' verify this store is configured in StorePOSConfig
                    If (StoreUpdatesBO.ContainsKey(currentStoreNum)) Then
                        ' get the StoreUpdatesBO for the current store being processed
                        currentStoreUpdate = CType(StoreUpdatesBO(currentStoreNum), StoreUpdatesBO)
                        ' populate a TaxFlagBO for this store item
                        currentTaxFlag = New TaxFlagBO(result)
                        ' store the TaxFlagBO in the hashtable for the store
                        currentStoreUpdate.AddItemTaxFlagData(currentItemKey, currentTaxFlag)
                    Else
                        ' Ignore - an error notification does not need to be delivered if the store # is not configured
                        ' in the StorePOSConfig because the message is already being sent by the WriteResultsToFile method
                    End If
                End While

                posPushJobTimer.Stop()
                stepTimeInSeconds = CLng((posPushJobTimer.ElapsedMilliseconds/1000L))
                Console.WriteLine("Time taken for processing Tax Flags Data Application Step in POS Push: " + stepTimeInSeconds.ToString())
                Logger.LogInfo("Time taken for processing Tax Flags Data Application Step in POS Push: " + stepTimeInSeconds.ToString(), Me.GetType())

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

        Public Overrides Sub PopulateApplyChangesData(ByRef currentStore As StoreUpdatesBO, ByRef result As SqlDataReader)
            Throw New NotImplementedException()
        End Sub

        Public Overrides Sub ApplyChangesInIRMA(ByRef StoreUpdatesBO As Hashtable)
            Throw New NotImplementedException()
        End Sub
    End Class
End Namespace
