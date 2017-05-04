Imports System.Data.SqlClient
Imports WholeFoods.Utility.DataAccess

Public Class NoTagDataAccess

    Public Overridable Sub WriteToNoTagExclusion(discardedItems As List(Of Integer), priceBatchHeaderId As Integer, storeNumber As Integer)
        Dim tableType As DataTable = New DataTable()

        With tableType.Columns
            .Add("ItemKey", GetType(Integer))
        End With

        For Each itemKey As Integer In discardedItems
            tableType.Rows.Add({itemKey})
        Next

        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim connection As SqlConnection = New SqlConnection()

        Try
            connection.ConnectionString = factory.ConnectString
            connection.Open()

            Dim command As New SqlCommand("InsertNoTagItemExclusion", connection) With {.CommandType = CommandType.StoredProcedure, .CommandTimeout = factory.CommandTimeout}

            Dim exclusionsParameter As SqlParameter = command.Parameters.Add("@ExcludedItemKeys", SqlDbType.Structured)
            exclusionsParameter.Value = tableType

            Dim priceBatchHeaderIdParameter As SqlParameter = command.Parameters.Add("@PriceBatchHeaderId", SqlDbType.Int)
            priceBatchHeaderIdParameter.Value = If(priceBatchHeaderId = 0, DBNull.Value, priceBatchHeaderId)

            Dim storeNumberParameter As SqlParameter = command.Parameters.Add("@StoreNumber", SqlDbType.Int)
            storeNumberParameter.Value = storeNumber

            command.ExecuteNonQuery()
        Finally
            connection.Close()
        End Try
    End Sub

    Public Overridable Sub WriteToNoTagExclusion(discardedIdentifiers As List(Of String), storeNumber As Integer)
        Dim tableType As DataTable = New DataTable()

        With tableType.Columns
            .Add("Identifier", GetType(String))
        End With

        For Each identifier As String In discardedIdentifiers
            tableType.Rows.Add({identifier})
        Next

        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim connection As SqlConnection = New SqlConnection()

        Try
            connection.ConnectionString = factory.ConnectString
            connection.Open()

            Dim command As New SqlCommand("InsertNoTagItemExclusion", connection) With {.CommandType = CommandType.StoredProcedure, .CommandTimeout = factory.CommandTimeout}

            Dim exclusionsParameter As SqlParameter = command.Parameters.Add("@ExcludedIdentifiers", SqlDbType.Structured)
            exclusionsParameter.Value = tableType

            Dim storeNumberParameter As SqlParameter = command.Parameters.Add("@StoreNumber", SqlDbType.Int)
            storeNumberParameter.Value = storeNumber

            command.ExecuteNonQuery()
        Finally
            connection.Close()
        End Try
    End Sub

    Public Overridable Function GetOffSaleExclusions(items As List(Of Integer), batchHeaderId As Integer) As List(Of Integer)
        Dim dataTable As DataTable = New DataTable()

        With dataTable.Columns
            .Add("Key", GetType(Integer))
        End With

        For Each itemKey As Integer In items
            dataTable.Rows.Add({itemKey})
        Next

        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim connection As SqlConnection = New SqlConnection()
        Dim results As SqlDataReader
        Dim excludedItemKeys As List(Of Integer) = New List(Of Integer)

        Try
            connection.ConnectionString = factory.ConnectString
            connection.Open()

            Dim command As New SqlCommand("GetNoTagOffSaleExclusions", connection) With {.CommandType = CommandType.StoredProcedure, .CommandTimeout = factory.CommandTimeout}

            Dim itemKeysParameter As SqlParameter = command.Parameters.Add("@ItemKeys", SqlDbType.Structured)
            itemKeysParameter.Value = dataTable

            Dim batchHeaderParameter As SqlParameter = command.Parameters.Add("@PriceBatchHeaderId", SqlDbType.Int)
            batchHeaderParameter.Value = batchHeaderId

            results = command.ExecuteReader()

            While results.Read()
                excludedItemKeys.Add(results.GetInt32(results.GetOrdinal("ItemKey")))
            End While
        Finally
            connection.Close()
        End Try

        Return excludedItemKeys
    End Function

    Public Function GetOrderingHistoryExclusions(items As List(Of Integer), storeNumber As Integer, days As Integer) As List(Of Integer)
        Dim dataTable As DataTable = New DataTable()

        With dataTable.Columns
            .Add("Key", GetType(Integer))
        End With

        For Each itemKey As Integer In items
            dataTable.Rows.Add({itemKey})
        Next

        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim connection As SqlConnection = New SqlConnection()
        Dim results As SqlDataReader
        Dim excludedItemKeys As List(Of Integer) = New List(Of Integer)

        Try
            connection.ConnectionString = factory.ConnectString
            connection.Open()

            Dim command As New SqlCommand("GetNoTagOrderingHistoryExclusions", connection) With {.CommandType = CommandType.StoredProcedure, .CommandTimeout = factory.CommandTimeout}

            Dim itemKeysParameter As SqlParameter = command.Parameters.Add("@ItemKeys", SqlDbType.Structured)
            itemKeysParameter.Value = dataTable

            Dim storeNumberParameter As SqlParameter = command.Parameters.Add("@StoreNumber", SqlDbType.Int)
            storeNumberParameter.Value = storeNumber

            Dim daysParameter As SqlParameter = command.Parameters.Add("@Days", SqlDbType.Int)
            daysParameter.Value = days

            results = command.ExecuteReader()

            While results.Read()
                excludedItemKeys.Add(results.GetInt32(results.GetOrdinal("ItemKey")))
            End While
        Finally
            connection.Close()
        End Try

        Return excludedItemKeys
    End Function

    Public Function GetReceivingHistoryExclusions(items As List(Of Integer), storeNumber As Integer, days As Integer) As List(Of Integer)
        Dim dataTable As DataTable = New DataTable()

        With dataTable.Columns
            .Add("Key", GetType(Integer))
        End With

        For Each itemKey As Integer In items
            dataTable.Rows.Add({itemKey})
        Next

        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim connection As SqlConnection = New SqlConnection()
        Dim results As SqlDataReader
        Dim excludedItemKeys As List(Of Integer) = New List(Of Integer)

        Try
            connection.ConnectionString = factory.ConnectString
            connection.Open()

            Dim command As New SqlCommand("GetNoTagReceivingHistoryExclusions", connection) With {.CommandType = CommandType.StoredProcedure, .CommandTimeout = factory.CommandTimeout}

            Dim itemKeysParameter As SqlParameter = command.Parameters.Add("@ItemKeys", SqlDbType.Structured)
            itemKeysParameter.Value = dataTable

            Dim storeNumberParameter As SqlParameter = command.Parameters.Add("@StoreNumber", SqlDbType.Int)
            storeNumberParameter.Value = storeNumber

            Dim daysParameter As SqlParameter = command.Parameters.Add("@Days", SqlDbType.Int)
            daysParameter.Value = days

            results = command.ExecuteReader()

            While results.Read()
                excludedItemKeys.Add(results.GetInt32(results.GetOrdinal("ItemKey")))
            End While
        Finally
            connection.Close()
        End Try

        Return excludedItemKeys
    End Function

    Public Function GetMovementHistoryExclusions(items As List(Of Integer), storeNumber As Integer, days As Integer) As List(Of Integer)
        Dim dataTable As DataTable = New DataTable()

        With dataTable.Columns
            .Add("Key", GetType(Integer))
        End With

        For Each itemKey As Integer In items
            dataTable.Rows.Add({itemKey})
        Next

        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim connection As SqlConnection = New SqlConnection()
        Dim results As SqlDataReader
        Dim excludedItemKeys As List(Of Integer) = New List(Of Integer)

        Try
            connection.ConnectionString = factory.ConnectString
            connection.Open()

            Dim command As New SqlCommand("GetNoTagMovementHistoryExclusions", connection) With {.CommandType = CommandType.StoredProcedure, .CommandTimeout = factory.CommandTimeout}

            Dim itemKeysParameter As SqlParameter = command.Parameters.Add("@ItemKeys", SqlDbType.Structured)
            itemKeysParameter.Value = dataTable

            Dim storeNumberParameter As SqlParameter = command.Parameters.Add("@StoreNumber", SqlDbType.Int)
            storeNumberParameter.Value = storeNumber

            Dim daysParameter As SqlParameter = command.Parameters.Add("@Days", SqlDbType.Int)
            daysParameter.Value = days

            results = command.ExecuteReader()

            While results.Read()
                excludedItemKeys.Add(results.GetInt32(results.GetOrdinal("ItemKey")))
            End While
        Finally
            connection.Close()
        End Try

        Return excludedItemKeys
    End Function

    Public Overridable Function GetSubteamOverride(subteamNumber As Integer) As Integer
        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim connection As SqlConnection = New SqlConnection()
        Dim results As SqlDataReader
        Dim subteamOverride As Integer? = Nothing

        Try
            connection.ConnectionString = factory.ConnectString
            connection.Open()

            Dim command As New SqlCommand("GetNoTagThresholdSubteamOverride", connection) With {.CommandType = CommandType.StoredProcedure, .CommandTimeout = factory.CommandTimeout}

            Dim subteamNumberParameter As SqlParameter = command.Parameters.Add("@SubteamNumber", SqlDbType.Int)
            subteamNumberParameter.Value = subteamNumber

            results = command.ExecuteReader()

            While results.Read()
                subteamOverride = results.GetInt32(results.GetOrdinal("ThresholdValueDays"))
            End While
        Finally
            connection.Close()
        End Try

        Return If(subteamOverride Is Nothing, 0, subteamOverride)
    End Function

    Public Function GetSubteamOverrides() As Dictionary(Of Integer, Integer)
        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim connection As SqlConnection = New SqlConnection()
        Dim results As SqlDataReader
        Dim subteamOverrides As Dictionary(Of Integer, Integer) = New Dictionary(Of Integer, Integer)

        Try
            connection.ConnectionString = factory.ConnectString
            connection.Open()

            Dim command As New SqlCommand("GetNoTagThresholdSubteamOverride", connection) With {.CommandType = CommandType.StoredProcedure, .CommandTimeout = factory.CommandTimeout}

            results = command.ExecuteReader()

            Dim subteamNumber As Integer
            Dim thresholdValue As Integer?

            While results.Read()
                subteamNumber = results.GetInt32(results.GetOrdinal("SubteamNumber"))
                thresholdValue = results.GetInt32(results.GetOrdinal("ThresholdValueDays"))

                subteamOverrides.Add(subteamNumber, If(thresholdValue Is Nothing, 0, thresholdValue))
            End While
        Finally
            connection.Close()
        End Try

        Return subteamOverrides
    End Function

    Public Overridable Function GetRuleDefaultThreshold(ruleName As String) As Integer
        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim connection As SqlConnection = New SqlConnection()
        Dim results As SqlDataReader
        Dim ruleThreshold As Integer? = Nothing

        Try
            connection.ConnectionString = factory.ConnectString
            connection.Open()

            Dim command As New SqlCommand("GetNoTagRuleThreshold", connection) With {.CommandType = CommandType.StoredProcedure, .CommandTimeout = factory.CommandTimeout}

            Dim ruleNameParameter As SqlParameter = command.Parameters.Add("@RuleName", SqlDbType.NVarChar)
            ruleNameParameter.Value = ruleName

            results = command.ExecuteReader()

            While results.Read()
                ruleThreshold = results.GetInt32(results.GetOrdinal("ThresholdValueDays"))
            End While
        Finally
            connection.Close()
        End Try

        Return If(ruleThreshold Is Nothing, 30, ruleThreshold)
    End Function

    Public Function GetRuleDefaultThresholds() As Dictionary(Of String, Integer)
        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim connection As SqlConnection = New SqlConnection()
        Dim results As SqlDataReader
        Dim ruleThresholds As Dictionary(Of String, Integer) = New Dictionary(Of String, Integer)

        Try
            connection.ConnectionString = factory.ConnectString
            connection.Open()

            Dim command As New SqlCommand("GetNoTagRuleThreshold", connection) With {.CommandType = CommandType.StoredProcedure, .CommandTimeout = factory.CommandTimeout}

            results = command.ExecuteReader()

            Dim ruleName As String
            Dim thresholdValue As Integer?

            While results.Read()
                ruleName = results.GetString(results.GetOrdinal("RuleName"))
                thresholdValue = results.GetInt32(results.GetOrdinal("ThresholdValueDays"))

                ruleThresholds.Add(ruleName, If(thresholdValue Is Nothing, 30, thresholdValue))
            End While
        Finally
            connection.Close()
        End Try

        Return ruleThresholds
    End Function

    Public Sub UpdateNoTagRuleThresholds(defaultRuleConfigurations As Dictionary(Of String, Integer))
        Dim tableType As DataTable = New DataTable()

        With tableType.Columns
            .Add("RuleName", GetType(String))
            .Add("ThresholdValue", GetType(Integer))
        End With

        For Each configuration As KeyValuePair(Of String, Integer) In defaultRuleConfigurations
            tableType.Rows.Add({configuration.Key, configuration.Value})
        Next

        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim connection As SqlConnection = New SqlConnection()

        Try
            connection.ConnectionString = factory.ConnectString
            connection.Open()

            Dim command As New SqlCommand("UpdateNoTagRuleThresholds", connection) With {.CommandType = CommandType.StoredProcedure, .CommandTimeout = factory.CommandTimeout}

            Dim exclusionsParameter As SqlParameter = command.Parameters.Add("@RuleThresholds", SqlDbType.Structured)
            exclusionsParameter.Value = tableType

            command.ExecuteNonQuery()
        Finally
            connection.Close()
        End Try
    End Sub

    Public Sub UpdateNoTagSubteamOverrides(subteamOverrides As Dictionary(Of Integer, Integer))
        Dim tableType As DataTable = New DataTable()

        With tableType.Columns
            .Add("SubteamNumber", GetType(Integer))
            .Add("ThresholdValue", GetType(Integer))
        End With

        For Each override As KeyValuePair(Of Integer, Integer) In subteamOverrides
            tableType.Rows.Add({override.Key, override.Value})
        Next

        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim connection As SqlConnection = New SqlConnection()

        Try
            connection.ConnectionString = factory.ConnectString
            connection.Open()

            Dim command As New SqlCommand("UpdateNoTagSubteamOverrides", connection) With {.CommandType = CommandType.StoredProcedure, .CommandTimeout = factory.CommandTimeout}

            Dim exclusionsParameter As SqlParameter = command.Parameters.Add("@SubteamOverrides", SqlDbType.Structured)
            exclusionsParameter.Value = tableType

            command.ExecuteNonQuery()
        Finally
            connection.Close()
        End Try
    End Sub

    Public Overridable Function GetLabelTypeExclusions(items As List(Of Integer)) As List(Of Integer)
        Dim dataTable As DataTable = New DataTable()

        With dataTable.Columns
            .Add("Key", GetType(Integer))
        End With

        For Each itemKey As Integer In items
            dataTable.Rows.Add({itemKey})
        Next

        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim connection As SqlConnection = New SqlConnection()
        Dim results As SqlDataReader
        Dim excludedItemKeys As List(Of Integer) = New List(Of Integer)

        Try
            connection.ConnectionString = factory.ConnectString
            connection.Open()

            Dim command As New SqlCommand("GetNoTagLabelTypeExclusions", connection) With {.CommandType = CommandType.StoredProcedure, .CommandTimeout = factory.CommandTimeout}

            Dim itemKeysParameter As SqlParameter = command.Parameters.Add("@ItemKeys", SqlDbType.Structured)
            itemKeysParameter.Value = dataTable

            results = command.ExecuteReader()

            While results.Read()
                excludedItemKeys.Add(results.GetInt32(results.GetOrdinal("ItemKey")))
            End While
        Finally
            connection.Close()
        End Try

        Return excludedItemKeys
    End Function
End Class
