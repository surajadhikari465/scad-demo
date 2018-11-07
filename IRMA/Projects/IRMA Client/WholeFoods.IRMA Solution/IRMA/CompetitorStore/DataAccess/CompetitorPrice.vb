Imports System.Data.SqlClient
Imports WholeFoods.Utility.DataAccess
Imports WholeFoods.IRMA.CompetitorStore.BusinessLogic

Namespace WholeFoods.IRMA.CompetitorStore.DataAccess
    Public Class CompetitorPrice

#Region "Helper Methods"

        Private Function CreateInsertCompetitorPriceCommand(ByVal factory As DataFactory, ByVal transaction As SqlTransaction) As SqlCommand
            Dim command As SqlCommand = factory.GetDataCommand("InsertCompetitorPrice", transaction, True)

            With command.Parameters
                .Add(New SqlParameter("Item_Key", SqlDbType.Int, 0, "Item_Key"))
                .Add(New SqlParameter("FiscalYear", SqlDbType.SmallInt, 0, "FiscalYear"))
                .Add(New SqlParameter("FiscalPeriod", SqlDbType.SmallInt, 0, "FiscalPeriod"))
                .Add(New SqlParameter("PeriodWeek", SqlDbType.SmallInt, 0, "PeriodWeek"))
                .Add(New SqlParameter("UPCCode", SqlDbType.VarChar, 50, "UPCCode"))
                .Add(New SqlParameter("Description", SqlDbType.VarChar, 50, "Description"))
                .Add(New SqlParameter("Size", SqlDbType.Decimal, 5, "Size"))
                .Add(New SqlParameter("Unit_ID", SqlDbType.Int, 0, "Unit_ID"))
                .Add(New SqlParameter("PriceMultiple", SqlDbType.Int, 0, "PriceMultiple"))
                .Add(New SqlParameter("Price", SqlDbType.Money, 0, "Price"))
                .Add(New SqlParameter("SaleMultiple", SqlDbType.Int, 0, "SaleMultiple"))
                .Add(New SqlParameter("Sale", SqlDbType.Money, 0, "Sale"))
                .Add(New SqlParameter("UpdateUserID", SqlDbType.Int, 0, "UpdateUserID"))
                .Add(New SqlParameter("UpdateDateTime", SqlDbType.DateTime, 0, "UpdateDateTime"))
                .Add(New SqlParameter("Competitor", SqlDbType.VarChar, 50, "Competitor"))
                .Add(New SqlParameter("Location", SqlDbType.VarChar, 50, "Location"))
                .Add(New SqlParameter("CompetitorStore", SqlDbType.VarChar, 50, "CompetitorStore"))
                .Add(New SqlParameter("ItemIdentifier", SqlDbType.VarChar, 13, "WFMIdentifier"))
            End With

            Dim currentParameter As SqlParameter = New SqlParameter("CompetitorID", SqlDbType.Int, 0, "CompetitorID")

            currentParameter.Direction = ParameterDirection.InputOutput
            command.Parameters.Add(currentParameter)

            currentParameter = New SqlParameter("CompetitorLocationID", SqlDbType.Int, 0, "CompetitorLocationID")
            currentParameter.Direction = ParameterDirection.InputOutput
            command.Parameters.Add(currentParameter)

            currentParameter = New SqlParameter("CompetitorStoreID", SqlDbType.Int, 0, "CompetitorStoreID")
            currentParameter.Direction = ParameterDirection.InputOutput
            command.Parameters.Add(currentParameter)

            Return command
        End Function

        Private Function CreateUpdateCompetitorPriceCommand(ByVal factory As DataFactory, ByVal transaction As SqlTransaction) As SqlCommand
            Dim command As SqlCommand = factory.GetDataCommand("UpdateCompetitorPrice", transaction, True)

            With command.Parameters
                .Add(New SqlParameter("Item_Key", SqlDbType.Int, 0, "Item_Key"))
                .Add(New SqlParameter("CompetitorStoreID", SqlDbType.Int, 0, "CompetitorStoreID"))
                .Add(New SqlParameter("FiscalYear", SqlDbType.SmallInt, 0, "FiscalYear"))
                .Add(New SqlParameter("FiscalPeriod", SqlDbType.SmallInt, 0, "FiscalPeriod"))
                .Add(New SqlParameter("PeriodWeek", SqlDbType.SmallInt, 0, "PeriodWeek"))
                .Add(New SqlParameter("UPCCode", SqlDbType.VarChar, 50, "UPCCode"))
                .Add(New SqlParameter("Description", SqlDbType.VarChar, 50, "Description"))
                .Add(New SqlParameter("Size", SqlDbType.Decimal, 5, "Size"))
                .Add(New SqlParameter("Unit_ID", SqlDbType.Int, 0, "Unit_ID"))
                .Add(New SqlParameter("PriceMultiple", SqlDbType.Int, 0, "PriceMultiple"))
                .Add(New SqlParameter("Price", SqlDbType.Money, 0, "Price"))
                .Add(New SqlParameter("SaleMultiple", SqlDbType.Int, 0, "SaleMultiple"))
                .Add(New SqlParameter("Sale", SqlDbType.Money, 0, "Sale"))
                .Add(New SqlParameter("UpdateUserID", SqlDbType.Int, 0, "UpdateUserID"))
                .Add(New SqlParameter("UpdateDateTime", SqlDbType.DateTime, 0, "UpdateDateTime"))
            End With

            Return command
        End Function

        Private Function CreateDeleteCompetitorPriceCommand(ByVal factory As DataFactory, ByVal transaction As SqlTransaction) As SqlCommand
            Dim command As SqlCommand = factory.GetDataCommand("DeleteCompetitorPrice", transaction, True)

            With command.Parameters
                .Add(New SqlParameter("Item_Key", SqlDbType.Int, 0, "Item_Key"))
                .Add(New SqlParameter("CompetitorStoreID", SqlDbType.Int, 0, "CompetitorStoreID"))
                .Add(New SqlParameter("FiscalYear", SqlDbType.SmallInt, 0, "FiscalYear"))
                .Add(New SqlParameter("FiscalPeriod", SqlDbType.SmallInt, 0, "FiscalPeriod"))
                .Add(New SqlParameter("PeriodWeek", SqlDbType.SmallInt, 0, "PeriodWeek"))
            End With

            Return command
        End Function

        Private Shared Sub SetOptionalIntegerParameter(ByVal parameterValue As Nullable(Of Integer), ByVal parameterName As String, ByVal command As SqlCommand)
            Dim param As SqlParameter
            If parameterValue.HasValue AndAlso parameterValue.Value > 0 Then
                param = New SqlParameter(parameterName, SqlDbType.Int)
                param.Value = parameterValue.Value
                command.Parameters.Add(param)
            End If
        End Sub

        Private Shared Sub AddValueParameter(ByVal parameterValue As Object, ByVal parameterType As SqlDbType, ByVal parameterName As String, ByVal command As SqlCommand)
            Dim param As New SqlParameter(parameterName, parameterType)
            param.Value = parameterValue
            command.Parameters.Add(param)
        End Sub

#End Region

        Public Sub Search(ByRef dataSet As CompetitorStoreDataSet, ByVal competitorID As Nullable(Of Integer), _
            ByVal competitorLocationID As Nullable(Of Integer), ByVal competitorStoreID As Nullable(Of Integer), _
            ByVal fiscalWeek As CompetitorStoreDataSet.FiscalWeekRow, ByVal itemIdentifier As String)

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim command As SqlCommand = factory.GetDataCommand("GetCompetitorPriceSearch", Nothing, True)

            SetOptionalIntegerParameter(competitorID, "CompetitorID", command)
            SetOptionalIntegerParameter(competitorLocationID, "CompetitorLocationID", command)
            SetOptionalIntegerParameter(competitorStoreID, "CompetitorStoreID", command)

            If fiscalWeek IsNot Nothing Then
                AddValueParameter(fiscalWeek.FiscalYear, SqlDbType.SmallInt, "FiscalYear", command)
                AddValueParameter(fiscalWeek.FiscalPeriod, SqlDbType.SmallInt, "FiscalPeriod", command)
                AddValueParameter(fiscalWeek.PeriodWeek, SqlDbType.SmallInt, "PeriodWeek", command)
            End If

            If Not String.IsNullOrEmpty(itemIdentifier) Then
                AddValueParameter(itemIdentifier, SqlDbType.VarChar, "ItemIdentifier", command)
            End If

            Using adapter As SqlDataAdapter = factory.GetDataAdapter(command, Nothing)
                adapter.Fill(dataSet.CompetitorPrice)
            End Using
        End Sub

        Public Sub Save(ByRef dataSet As CompetitorStoreDataSet)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim command As SqlCommand = Nothing
            Dim transaction As SqlTransaction = Nothing

            Using adapter As SqlDataAdapter = factory.GetDataAdapter(command, transaction)
                With adapter
                    .InsertCommand = CreateInsertCompetitorPriceCommand(factory, transaction)
                    .UpdateCommand = CreateUpdateCompetitorPriceCommand(factory, transaction)
                    .DeleteCommand = CreateDeleteCompetitorPriceCommand(factory, transaction)
                    .Update(dataSet.CompetitorPrice)
                End With
            End Using
        End Sub
    End Class
End Namespace