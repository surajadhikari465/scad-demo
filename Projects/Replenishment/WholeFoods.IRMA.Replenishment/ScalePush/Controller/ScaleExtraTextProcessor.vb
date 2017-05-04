Imports System.Data.SqlClient
Imports WholeFoods.IRMA.Replenishment.Common.Writers
Imports WholeFoods.IRMA.Replenishment.ScalePush.DataAccess
Imports WholeFoods.Utility

Namespace WholeFoods.IRMA.Replenishment.ScalePush.Controller

    ''' <summary>
    ''' Subclasses ScaleProcessor, providing the details for the ZoneScalePriceChange ChangeType.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class ScaleExtraTextProcessor
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
        ''' Reads the database records from for the NutriFact changes in IRMA, 
        ''' adding the results to the Scale Push file.
        ''' </summary>
        ''' <param name="storeUpdates"></param>
        ''' <remarks></remarks>
        ''' <exception cref="WholeFoods.Utility.DataAccess.DataFactory" />
        Overrides Sub RetrieveChangeRecordsFromIRMA(ByRef storeUpdates As Hashtable, ByVal isRegionalScaleFile As Boolean, Optional ByVal storeNo As String = Nothing)
            Logger.LogDebug("RetrieveChangeRecordsFromIRMA entry", Me.GetType())

            Dim scalePushJobTimer As Stopwatch
            Dim stepTimeInSeconds As Long = 0

            Dim results As SqlDataReader = Nothing

            Try
                scalePushJobTimer = Stopwatch.StartNew()

                ' Read the ExtraText changes that are ready to be sent to the stores.
                results = ScaleWriterDAO.GetExtraTextChanges()

                scalePushJobTimer.Stop()
                stepTimeInSeconds = CLng((scalePushJobTimer.ElapsedMilliseconds/1000L))
                Console.WriteLine("Time taken for Scale Extratext  Step DB Step : " + stepTimeInSeconds.ToString())
                Logger.LogInfo("Time taken for Scale Extratext  Step DB Step : " + stepTimeInSeconds.ToString(), Me.GetType())

                scalePushJobTimer = Stopwatch.StartNew()

                ' -- Process the resultset, which is a list of all items with ExtraText changes, ordered by the
                ' change types (Add, Change, Delete)
                Logger.LogDebug("processing result set - ExtraText records", Me.GetType())

                If isRegionalScaleFile Then
                    WriteResultsToFile(storeUpdates, results, ChangeType.ExtraText, isRegionalScaleFile, Constants.ScaleWriterType_Corporate)
                Else
                    WriteResultsToFile(storeUpdates, results, ChangeType.ExtraText, isRegionalScaleFile, Constants.ScaleWriterType_Store)
                End If

                scalePushJobTimer.Stop()
                stepTimeInSeconds = CLng((scalePushJobTimer.ElapsedMilliseconds/1000L))
                Console.WriteLine("Time taken for Scale Extratext Application Step : " + stepTimeInSeconds.ToString())
                Logger.LogInfo("Time taken for Scale Extratext Application Step : " + stepTimeInSeconds.ToString(), Me.GetType())
            Finally
                ' Close the result set and the connection
                If (results IsNot Nothing) Then
                    results.Close()
                End If
            End Try

            Logger.LogDebug("RetrieveChangeRecordsFromIRMA exit", Me.GetType())
        End Sub
    End Class
End Namespace