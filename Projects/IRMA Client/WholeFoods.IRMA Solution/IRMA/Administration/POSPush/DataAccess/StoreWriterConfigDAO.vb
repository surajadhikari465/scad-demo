Imports System.Data.SqlClient
Imports WholeFoods.IRMA.Administration.POSPush.BusinessLogic
Imports WholeFoods.Utility
Imports WholeFoods.Utility.DataAccess

Namespace WholeFoods.IRMA.Administration.POSPush.DataAccess
    Public Class StoreWriterConfigDAO
#Region "Read Methods"
        ''' <summary>
        ''' Selects the list of Store records that do not have an entry in the StorePOSConfig table.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared Function GetStoresNotConfigured() As DataSet
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            ' Execute the stored procedure 
            Return factory.GetStoredProcedureDataSet("Administration_POSPush_GetStoresAvailableForAdd")
        End Function

        ''' <summary>
        ''' Read the complete list of StorePOSConfig objects.
        ''' </summary>
        ''' <exception cref="DataFactoryException" />
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetStorePOSConfigurations() As DataSet
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            ' setup parameters for stored proc
            currentParam = New DBParam
            currentParam.Name = "Store_No"
            currentParam.Value = DBNull.Value
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            ' Execute the stored procedure 
            Return factory.GetStoredProcedureDataSet("Administration_POSPush_GetStorePOSWriterConfigurations", paramList)
        End Function

        ''' <summary>
        ''' Read the Store POS Writer configuration data for a single store.
        ''' </summary>
        ''' <param name="storeNo"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetStorePOSConfiguration(ByVal storeNo As Integer) As StorePOSConfigBO
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As SqlDataReader = Nothing
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim storePOS As StorePOSConfigBO = Nothing

            ' setup parameters for stored proc
            currentParam = New DBParam
            currentParam.Name = "Store_No"
            currentParam.Value = storeNo
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            'currentParam = New DBParam
            'currentParam.Name = "Writer_Type"
            'currentParam.Value = FileWriterType
            'currentParam.Type = DBParamType.String
            ' paramList.Add(currentParam)

            Try
                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataReader("Administration_POSPush_GetStorePOSWriterConfigurations", paramList)
                While results.Read
                    storePOS = New StorePOSConfigBO(results)
                End While
            Catch ex As Exception
                Throw ex
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

            Return storePOS
        End Function

        ''' <summary>
        ''' Read the Store Writer configuration data for a single store for a given writer type.
        ''' </summary>
        ''' <param name="storeNo"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetStoreShelfTagWriterConfiguration(ByVal storeNo As Integer, ByVal writerType As String) As StorePOSConfigBO
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As SqlDataReader = Nothing
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim storePOS As StorePOSConfigBO = Nothing

            ' setup parameters for stored proc
            currentParam = New DBParam
            currentParam.Name = "Store_No"
            currentParam.Value = storeNo
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Writer_Type"
            currentParam.Value = writerType
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            Try
                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataReader("Administration_TagPush_GetStoreShelfTagWriterConfigurations", paramList)

                While results.Read
                    storePOS = New StorePOSConfigBO(results)
                End While
            Catch ex As Exception
                Throw ex
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

            Return storePOS
        End Function

        ''' <summary>
        ''' Read the Store Writer configuration data for a single store for a given writer type.
        ''' </summary>
        ''' <param name="storeNo"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetStoreElectronicShelfTagWriterConfiguration(ByVal storeNo As Integer, ByVal writerType As String) As StorePOSConfigBO
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As SqlDataReader = Nothing
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim storePOS As StorePOSConfigBO = Nothing

            ' setup parameters for stored proc
            currentParam = New DBParam
            currentParam.Name = "Store_No"
            currentParam.Value = storeNo
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Writer_Type"
            currentParam.Value = writerType
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            Try
                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataReader("Administration_TagPush_GetStoreElectronicShelfTagWriterConfigurations", paramList)

                While results.Read
                    storePOS = New StorePOSConfigBO(results)
                End While
            Catch ex As Exception
                Throw ex
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

            Return storePOS
        End Function

        ''' <summary>
        ''' Read the complete list of StoreScaleConfig objects.
        ''' </summary>
        ''' <exception cref="DataFactoryException" />
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetStoreScaleConfigurations() As DataSet
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            ' setup parameters for stored proc
            currentParam = New DBParam
            currentParam.Name = "Store_No"
            currentParam.Value = DBNull.Value
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            ' Execute the stored procedure 
            Return factory.GetStoredProcedureDataSet("Administration_POSPush_GetStoreScaleWriterConfigurations", paramList)
        End Function

        ''' <summary>
        ''' Read the Store Scale Writer configuration data for a single store.
        ''' </summary>
        ''' <param name="storeNo"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetStoreScaleConfiguration(ByVal storeNo As Integer, ByVal scaleType As String) As StoreScaleConfigBO
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As SqlDataReader = Nothing
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim storeScalse As New StoreScaleConfigBO

            ' setup parameters for stored proc
            currentParam = New DBParam
            currentParam.Name = "Store_No"
            currentParam.Value = storeNo
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "ScaleType"
            currentParam.Value = scaleType
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            Try
                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataReader("Administration_POSPush_GetStoreScaleWriterConfigurations", paramList)

                While results.Read
                    storeScalse = New StoreScaleConfigBO(results)
                End While
            Catch ex As Exception
                Throw ex
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

            Return storeScalse
        End Function

#End Region

#Region "Create, Update Methods"
        ''' <summary>
        ''' Insert a new record into the StorePOSConfig table.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared Sub AddStorePOSConfigRecord(ByRef currentStore As StorePOSConfigBO)
            Logger.LogDebug("AddStorePOSConfigRecord entry", Nothing)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)

            'setup parameters for stored proc
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            currentParam = New DBParam
            currentParam.Name = "Store_No"
            currentParam.Value = currentStore.StoreConfig.StoreNo
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "POSFileWriterKey"
            currentParam.Value = currentStore.POSFileWriterKey
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "ConfigType"
            currentParam.Value = currentStore.ConfigType
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            'currentParam = New DBParam
            'currentParam.Name = "ScaleFileWriterKey"
            'If currentStore.ScaleFileWriterKey < 0 Then
            '    currentParam.Value = DBNull.Value
            'Else
            '    currentParam.Value = currentStore.ScaleFileWriterKey
            'End If
            'currentParam.Type = DBParamType.Int
            'paramList.Add(currentParam)

            ' Execute the stored procedure to insert the new StorePOSConfig record.
            factory.ExecuteStoredProcedure("Administration_POSPush_InsertStorePOSConfig", paramList)
            Logger.LogDebug("AddStorePOSConfigRecord exit", Nothing)
        End Sub

        ''' <summary>
        ''' Update an existing record in the StorePOSConfig table.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared Sub UpdateStorePOSConfigRecord(ByRef currentStore As StorePOSConfigBO)
            Logger.LogDebug("UpdateStorePOSConfigRecord entry", Nothing)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)

            'setup parameters for stored proc
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            currentParam = New DBParam
            currentParam.Name = "Store_No"
            currentParam.Value = currentStore.StoreConfig.StoreNo
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "POSFileWriterKey"
            currentParam.Value = currentStore.POSFileWriterKey
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "ConfigType"
            currentParam.Value = currentStore.ConfigType
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            ' Execute the stored procedure to insert the new StorePOSConfig record.
            factory.ExecuteStoredProcedure("Administration_POSPush_UpdateStorePOSConfig", paramList)
            Logger.LogDebug("UpdateStorePOSConfigRecord exit", Nothing)
        End Sub

        ''' <summary>
        ''' Insert a new record into the StoreShelfTagConfig table.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared Sub AddStoreShelfTagConfigRecord(ByRef currentStore As StorePOSConfigBO)
            Logger.LogDebug("AddStoreShelfTagConfigRecord entry", Nothing)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)

            'setup parameters for stored proc
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            currentParam = New DBParam
            currentParam.Name = "Store_No"
            currentParam.Value = currentStore.StoreConfig.StoreNo
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "POSFileWriterKey"
            currentParam.Value = currentStore.POSFileWriterKey
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "ConfigType"
            currentParam.Value = currentStore.ConfigType
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            factory.ExecuteStoredProcedure("Administration_TagPush_InsertStoreShelfTagConfig", paramList)
            Logger.LogDebug("AddStoreShelfTagConfigRecord exit", Nothing)
        End Sub

        ''' <summary>
        ''' Insert a new record into the StoreElectronicShelfTagConfig table.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared Sub AddStoreElectronicShelfTagConfigRecord(ByRef currentStore As StorePOSConfigBO)
            Logger.LogDebug("AddStoreElectronicShelfTagConfigRecord entry", Nothing)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)

            'setup parameters for stored proc
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            currentParam = New DBParam
            currentParam.Name = "Store_No"
            currentParam.Value = currentStore.StoreConfig.StoreNo
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "POSFileWriterKey"
            currentParam.Value = currentStore.POSFileWriterKey
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "ConfigType"
            currentParam.Value = currentStore.ConfigType
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            factory.ExecuteStoredProcedure("Administration_TagPush_InsertStoreElectronicShelfTagConfig", paramList)
            Logger.LogDebug("AddStoreShelfTagConfigRecord exit", Nothing)
        End Sub

        ''' <summary>
        ''' Update an existing record in the StorePOSConfig table.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared Sub UpdateStoreShelfTagConfigRecord(ByRef currentStore As StorePOSConfigBO)
            Logger.LogDebug("UpdateStoreShelfTagConfigRecord entry", Nothing)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)

            'setup parameters for stored proc
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            currentParam = New DBParam
            currentParam.Name = "Store_No"
            currentParam.Value = currentStore.StoreConfig.StoreNo
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "POSFileWriterKey"
            currentParam.Value = currentStore.POSFileWriterKey
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Writer_Type"
            If currentStore.FileWriterType Is Nothing Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = currentStore.FileWriterType
            End If

            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            ' Execute the stored procedure to insert the new StorePOSConfig record.
            factory.ExecuteStoredProcedure("Administration_TagPush_UpdateStoreShelfTagWriterConfig", paramList)
            Logger.LogDebug("UpdateStoreShelfTagConfigRecord exit", Nothing)
        End Sub

        ''' <summary>
        ''' Update an existing record in the StorePOSConfig table.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared Sub UpdateStoreElectronicShelfTagConfigRecord(ByRef currentStore As StorePOSConfigBO)
            Logger.LogDebug("UpdateStoreElectronicShelfTagConfigRecord entry", Nothing)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)

            'setup parameters for stored proc
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            currentParam = New DBParam
            currentParam.Name = "Store_No"
            currentParam.Value = currentStore.StoreConfig.StoreNo
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "POSFileWriterKey"
            currentParam.Value = currentStore.POSFileWriterKey
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Writer_Type"
            If currentStore.FileWriterType Is Nothing Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = currentStore.FileWriterType
            End If

            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            ' Execute the stored procedure to insert the new StorePOSConfig record.
            factory.ExecuteStoredProcedure("Administration_TagPush_UpdateStoreElectronicShelfTagWriterConfig", paramList)
            Logger.LogDebug("UpdateStoreElectronicShelfTagConfigRecord exit", Nothing)
        End Sub

        ''' <summary>
        ''' Insert a new record into the StoreScaleConfig table.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared Sub AddStoreScaleConfigRecord(ByRef currentStore As StoreScaleConfigBO)
            Logger.LogDebug("AddStoreScaleConfigRecord entry", Nothing)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)

            'setup parameters for stored proc
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            currentParam = New DBParam
            currentParam.Name = "Store_No"
            currentParam.Value = currentStore.StoreConfig.StoreNo
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "ScaleFileWriterKey"
            currentParam.Value = CType(currentStore.ScaleFileWriterKey, Integer)
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            factory.ExecuteStoredProcedure("Administration_Scale_InsertStoreScaleConfig", paramList)
            Logger.LogDebug("AddStoreShelfTagConfigRecord exit", Nothing)
        End Sub

        ''' <summary>
        ''' Update an existing record in the StorePOSConfig table.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared Sub UpdateStoreScaleConfigRecord(ByRef currentStore As StoreScaleConfigBO)
            Logger.LogDebug("UpdateStoreScaleConfigRecord entry", Nothing)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)

            'setup parameters for stored proc
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            currentParam = New DBParam
            currentParam.Name = "Store_No"
            currentParam.Value = currentStore.StoreConfig.StoreNo
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "ScaleFileWriterKey"
            currentParam.Value = CType(currentStore.ScaleFileWriterKey, Integer)
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Writer_Type"
            currentParam.Value = currentStore.ScaleWriterType
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            ' Execute the stored procedure to insert the new StorePOSConfig record.
            factory.ExecuteStoredProcedure("Administration_Scale_UpdateStoreScaleWriterConfig", paramList)
            Logger.LogDebug("UpdateStoreScaleConfigRecord exit", Nothing)
        End Sub
#End Region

#Region "Delete methods"
        ''' <summary>
        ''' Delete a record in the StorePOSConfig table.
        ''' Executes the DeleteStorePOSConfig stored procedure.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared Sub DeleteStorePOSConfigRecord(ByRef currentStore As StorePOSConfigBO)
            Logger.LogDebug("DeleteStorePOSConfigRecord entry", Nothing)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)

            'setup parameters for stored proc
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            currentParam = New DBParam
            currentParam.Name = "Store_No"
            currentParam.Value = currentStore.StoreConfig.StoreNo
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            ' Execute the stored procedure to delete the StorePOSConfig record.
            factory.ExecuteStoredProcedure("Administration_POSPush_DeleteStorePOSConfig", paramList)
            Logger.LogDebug("DeleteStorePOSConfigRecord exit", Nothing)
        End Sub

        ''' <summary>
        ''' Delete a record in the StoreShelfTagConfig table.
        ''' Executes the DeleteStoreShelfTagConfig stored procedure.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared Sub DeleteStoreShelfTagConfigRecord(ByRef currentStore As StorePOSConfigBO)
            Logger.LogDebug("DeleteStoreShelfTagConfigRecord entry", Nothing)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)

            'setup parameters for stored proc
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            currentParam = New DBParam
            currentParam.Name = "Store_No"
            currentParam.Value = currentStore.StoreConfig.StoreNo
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            ' Execute the stored procedure to delete the StoreShelfTagConfig record.
            factory.ExecuteStoredProcedure("Administration_TagPush_DeleteStoreShelfTagConfig", paramList)
            Logger.LogDebug("DeleteStoreShelfTagConfigRecord exit", Nothing)
        End Sub

        ''' <summary>
        ''' Delete a record in the StoreShelfTagConfig table.
        ''' Executes the DeleteStoreShelfTagConfig stored procedure.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared Sub DeleteStoreElectronicShelfTagConfigRecord(ByRef currentStore As StorePOSConfigBO)
            Logger.LogDebug("DeleteStoreElectronicShelfTagConfigRecord entry", Nothing)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)

            'setup parameters for stored proc
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            currentParam = New DBParam
            currentParam.Name = "Store_No"
            currentParam.Value = currentStore.StoreConfig.StoreNo
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            ' Execute the stored procedure to delete the StoreShelfTagConfig record.
            factory.ExecuteStoredProcedure("Administration_TagPush_DeleteStoreElectronicShelfTagConfig", paramList)
            Logger.LogDebug("DeleteStoreElectronicShelfTagConfigRecord exit", Nothing)
        End Sub

        ''' <summary>
        ''' Delete a record in the StoreScaleConfig table.
        ''' Executes the DeleteStoreScaleConfig stored procedure.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared Sub DeleteStoreScaleConfigRecord(ByRef currentStore As StoreScaleConfigBO)
            Logger.LogDebug("DeleteStoreScaleConfigRecord entry", Nothing)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)

            'setup parameters for stored proc
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            currentParam = New DBParam
            currentParam.Name = "Store_No"
            currentParam.Value = currentStore.StoreConfig.StoreNo
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            ' Execute the stored procedure to delete the StoreScaleConfig record.
            factory.ExecuteStoredProcedure("Administration_Scale_DeleteStoreScaleConfig", paramList)
            Logger.LogDebug("DeleteStoreScaleConfigRecord exit", Nothing)
        End Sub
#End Region

    End Class
End Namespace

