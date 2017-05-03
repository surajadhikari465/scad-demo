Imports System.Linq
Imports System.Data.SqlClient
Imports log4net
Imports WholeFoods.Utility.DataAccess
Imports WholeFoods.IRMA.InterfaceCommunication.WebApiModel
Imports WholeFoods.IRMA.InterfaceCommunication
Imports WholeFoods.IRMA.InterfaceCommunication.WebApiWrapper
Imports WholeFoods.IRMA.Pricing.BusinessLogic

Public Class PricingPrintSignsBO

    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Public Function GetValidTagReprintIdentifiers(storeNumber As Integer, identifiers As List(Of String)) As List(Of String)
        Dim tableType As DataTable = New DataTable()

        With tableType.Columns
            .Add("Identifier", GetType(String))
        End With

        For Each identifier As String In identifiers
            tableType.Rows.Add({identifier})
        Next

        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim connection As SqlConnection = New SqlConnection()
        Dim results As SqlDataReader
        Dim validIdentifiers As List(Of String) = New List(Of String)

        Try
            connection.ConnectionString = factory.ConnectString
            connection.Open()

            Dim command As New SqlCommand("GetValidTagReprintIdentifiers", connection) With {.CommandType = CommandType.StoredProcedure, .CommandTimeout = factory.CommandTimeout}

            Dim identifiersParameter As SqlParameter = command.Parameters.Add("@Identifiers", SqlDbType.Structured)
            identifiersParameter.Value = tableType

            Dim storeNumberParameter As SqlParameter = command.Parameters.Add("@StoreNumber", SqlDbType.Int)
            storeNumberParameter.Value = storeNumber

            results = command.ExecuteReader()

            While results.Read
                validIdentifiers.Add(results.GetString(results.GetOrdinal("Identifier")))
            End While

            Dim invalidIdentifiers As List(Of String) = identifiers.Except(validIdentifiers).ToList()

            If invalidIdentifiers.Count > 0 Then
                logger.Info(String.Format("The following identifiers will be excluded from the print request for store {0}. Check authorization, validation, and deleted status: {1}",
                                              storeNumber, String.Join(", ", invalidIdentifiers)))
            End If
        Finally
            connection.Close()
            tableType.Clear()
        End Try

        Return validIdentifiers
    End Function

    Public Sub SendTagReprintPrintBatchRequests(businessUnit As Integer, printRequestBatchName As String, identifiers As List(Of String))
        Dim slawPrintBatchModel As SlawPrintBatchModel = New SlawPrintBatchModel()

        slawPrintBatchModel.Application = SlawConstants.IrmaApplication
        slawPrintBatchModel.BatchName = printRequestBatchName
        slawPrintBatchModel.BusinessUnitId = businessUnit
        slawPrintBatchModel.IsAdHoc = True
        slawPrintBatchModel.BatchEvent = SlawConstants.BatchReprintEvent
        slawPrintBatchModel.HasPriceChange = 0
        slawPrintBatchModel.BatchChangeType = "ITM"
        slawPrintBatchModel.ItemCount = identifiers.Count

        slawPrintBatchModel.BatchItems = New List(Of SlawPrintBatchItemModel)

        For Each identifier As String In identifiers
            Dim slawPrintBatchItemModel As SlawPrintBatchItemModel = New SlawPrintBatchItemModel()

            slawPrintBatchItemModel.Identifier = identifier

            slawPrintBatchModel.BatchItems.Add(slawPrintBatchItemModel)
        Next

        Using slawWebApiWrapper As SlawWebApiWrapper = New SlawWebApiWrapper(String.Empty)
            slawWebApiWrapper.PostPrintHeader(slawPrintBatchModel)
        End Using
    End Sub

    Public Function GetNoTagLogicExcludedItems(itemKeyIdentifiers As List(Of ItemKeyIdentifierModel), subteamNumber As Integer, subteamName As String, storeNumber As Integer) As List(Of Integer)
        Dim noTagDataAccess As New NoTagDataAccess()
        Dim noTagRules As New List(Of INoTagRule)
        Dim movementHistoryRule As New MovementHistoryRule(noTagDataAccess)
        Dim orderingHistoryRule As New OrderingHistoryRule(noTagDataAccess)
        Dim receivingHistoryRule As New ReceivingHistoryRule(noTagDataAccess)

        noTagRules.Add(movementHistoryRule)
        noTagRules.Add(orderingHistoryRule)
        noTagRules.Add(receivingHistoryRule)

        Dim noTagLogic As New AdHocReprintNoTagLogic(noTagRules, noTagDataAccess, itemKeyIdentifiers, subteamNumber, subteamName, storeNumber)
        noTagLogic.ApplyNoTagLogic()

        Return noTagLogic.ExcludedItems
    End Function
End Class
