Imports System.Data
Imports System.Data.Common
Imports System.Data.SqlClient
Imports WholeFoods.IRMA.Administration.Common.BusinessLogic
Imports WholeFoods.Utility
Imports WholeFoods.Utility.DataAccess

Namespace WholeFoods.IRMA.Administration.Common.DataAccess
    Public Class RegionDAO
#Region "Read Methods"
        ''' <summary>
        ''' Populates the RegionBO with the instance data settings for this region.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared Sub GetRegionalInstanceData(ByRef region As RegionBO)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As SqlDataReader = Nothing

            Try
                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataReader("GetInstanceData")

                ' Process the result record
                While (results.Read())
                    ' Initialize a new RegionBO from the db record
                    If (Not results.IsDBNull(results.GetOrdinal("PrimaryRegionName"))) Then
                        region.PrimaryRegionName = results.GetString(results.GetOrdinal("PrimaryRegionName"))
                    End If
                    If (Not results.IsDBNull(results.GetOrdinal("PrimaryRegionCode"))) Then
                        region.PrimaryRegionCode = results.GetString(results.GetOrdinal("PrimaryRegionCode"))
                    End If
                End While
            Catch e As DataFactoryException
                Logger.LogError("Exception: ", Nothing, e)
                'send message about exception
                ErrorHandler.ProcessError(ErrorType.DataFactoryException, SeverityLevel.Warning, e)
            Finally
                ' Close the result set and the connection
                If (results IsNot Nothing) Then
                    results.Close()
                End If
            End Try
        End Sub

        ''' <summary>
        ''' Retrieve the store information for the regional office
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetRegionalOfficeStore() As StoreBO
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As SqlDataReader = Nothing
            Dim store As New StoreBO
            Try
                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataReader("Administration_GetRegionalStore")

                ' Process the result record
                While (results.Read())
                    ' Parse the results into the StoreBO
                    If (Not results.IsDBNull(results.GetOrdinal("Store_No"))) Then
                        store.StoreNo = results.GetInt32(results.GetOrdinal("Store_No"))
                    End If
                    If (Not results.IsDBNull(results.GetOrdinal("Store_Name"))) Then
                        store.StoreName = results.GetString(results.GetOrdinal("Store_Name"))
                    End If
                    If (Not results.IsDBNull(results.GetOrdinal("Regional"))) Then
                        store.RegionalOffice = results.GetBoolean(results.GetOrdinal("Regional"))
                    End If
                    If (Not results.IsDBNull(results.GetOrdinal("Phone_Number"))) Then
                        store.PhoneNo = results.GetString(results.GetOrdinal("Phone_Number"))
                    End If
                End While

            Catch e As DataFactoryException
                Logger.LogError("Exception: ", Nothing, e)
                'send message about exception
                ErrorHandler.ProcessError(ErrorType.DataFactoryException, SeverityLevel.Warning, e)
            Finally
                ' Close the result set and the connection
                If (results IsNot Nothing) Then
                    results.Close()
                End If
            End Try
            Return store
        End Function

        Public Shared Function GetRegionList() As DataTable
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As DataSet = Nothing

            Try
                Return factory.GetStoredProcedureDataTable("GetRegions")
            Catch e As DataFactoryException
                Logger.LogError("Exception: ", Nothing, e)
                ErrorHandler.ProcessError(ErrorType.DataFactoryException, SeverityLevel.Warning, e)
                Throw
            End Try
        End Function
#End Region

#Region "Create, Update Methods"
        ''' <summary>
        ''' Update an existing record in the Users table.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared Sub UpdateRegionalScaleSettings(ByRef currentRegion As RegionBO)
            Logger.LogDebug("UpdateRegionalScaleSettings entry", Nothing)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)

            ' Update the InstanceDataFlags record
            ' setup parameters for stored proc
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            currentParam = New DBParam
            currentParam.Name = "FlagKey"
            currentParam.Value = "UseRegionalScaleFile"
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "FlagValue"
            currentParam.Value = currentRegion.UseRegionalScaleFlag
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            ' Execute the stored procedure to update the Instance Data record.
            factory.ExecuteStoredProcedure("Administration_UpdateInstanceDataFlags", paramList)

            ' Update the store scale config settings
            paramList = New ArrayList
            currentParam = New DBParam
            currentParam.Name = "IsRegionalUpdate"
            currentParam.Value = True
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "CorpScaleFileWriterKey"
            If currentRegion.UseRegionalScaleFlag Then
                currentParam.Value = currentRegion.CorpScaleWriter.ScaleFileWriterKey
            Else
                currentParam.Value = DBNull.Value
            End If
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "ZoneScaleFileWriterKey"
            If currentRegion.UseRegionalScaleFlag Then
                currentParam.Value = currentRegion.ZoneScaleWriter.ScaleFileWriterKey
            Else
                currentParam.Value = DBNull.Value
            End If
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            ' set the store parameters to nothing
            currentParam = New DBParam
            currentParam.Name = "StoreScalefileWriterKey"
            currentParam.Value = DBNull.Value
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Store_No"
            currentParam.Value = DBNull.Value
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            ' Execute the stored procedure to update the Store record.
            factory.ExecuteStoredProcedure("Administration_POSPush_UpdateStoreScaleConfig", paramList)

            Logger.LogDebug("UpdateRegionalScaleSettings exit", Nothing)
        End Sub
#End Region

    End Class
End Namespace
