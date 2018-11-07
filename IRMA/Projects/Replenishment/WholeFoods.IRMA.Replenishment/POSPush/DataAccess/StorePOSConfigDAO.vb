Imports System.Data
Imports System.Data.Common
Imports System.Data.SqlClient
Imports System.Text
Imports WholeFoods.IRMA.Replenishment.Common.BusinessLogic
Imports WholeFoods.IRMA.Replenishment.Common.DataAccess
Imports WholeFoods.IRMA.Replenishment.POSPush.BusinessLogic
Imports WholeFoods.IRMA.Replenishment.POSPush.POSException
Imports WholeFoods.Utility
Imports WholeFoods.Utility.DataAccess

Namespace WholeFoods.IRMA.Replenishment.POSPush.DataAccess
    Public Class StorePOSConfigDAO
        ' Set the class type for logging
        Private Shared CLASSTYPE As Type = System.Type.GetType("WholeFoods.IRMA.Replenishment.POSPush.DataAccess.StorePOSConfigDAO")

#Region "Update methods"
        ''' <summary>
        ''' Update the batch record counter for the Store.  This is used by the POS Push process for stores that
        ''' send the next batch record count in the POS header information or control file.
        ''' </summary>
        ''' <param name="currentStore"></param>
        ''' <remarks></remarks>
        Public Shared Sub UpdateBatchRecordCount(ByRef currentStore As StoreUpdatesBO)
            Logger.LogDebug("UpdateBatchRecordCount entry: storeNo=" + currentStore.StoreNum.ToString, CLASSTYPE)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim currentParam As DBParam
            Dim paramList As New ArrayList
            Try
                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "Store_No"
                currentParam.Value = currentStore.StoreNum
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                ' Execute the stored procedure 
                factory.ExecuteStoredProcedure("UpdatePOSBatchInfo", paramList)
            Catch e As Exception
                Throw e
            End Try
        End Sub
#End Region

#Region "Read methods"
        ''' <summary>
        ''' Read the list of stores from the StorePOSConfig table and initialze a StoreUpdatesBO
        ''' object in the _storeUpdates Hashatable for each store.
        ''' Executes the Replenishment_POSPush_GetStoreWriterConfigurations stored procedure.
        ''' </summary>
        ''' <remarks></remarks>
        ''' <exception cref="DataFactoryException" />
        Public Shared Function GetStoreConfigurations(ByVal fileWriterType As String, Optional ByVal inStoreNo As String = Nothing) As Hashtable
            Logger.LogDebug("GetStoreConfigurations entry: fileWriterType=" + fileWriterType, CLASSTYPE)

            Dim storeUpdates As New Hashtable
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As SqlDataReader = Nothing
            Dim currentParam As DBParam
            Dim paramList As ArrayList
            Dim currentStore As StoreUpdatesBO

            'ftp information
            Dim storeFtpConfigDAO As New StoreFTPConfigDAO
            Dim ftpInfo As Hashtable = Nothing

            'manage store config errors
            Dim storeNo As String = ""
            Dim storeConfigErrorMsg As New StringBuilder
            Dim configErrorStores As New Hashtable

            Try
                'get store FTP information for multiple writer types
                ftpInfo = storeFtpConfigDAO.GetFTPConfigDataForWriterType(fileWriterType)

                ' setup parameters for stored proc
                paramList = New ArrayList

                currentParam = New DBParam
                currentParam.Name = "FileWriterType"
                currentParam.Value = fileWriterType
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataReader("Replenishment_POSPush_GetStoreWriterConfigurations", paramList)

                ' Process each store in the result set
                While (results.Read())
                    Try
                        currentStore = New StoreUpdatesBO(results)

                        'set FTP info for writer type in store object
                        currentStore.FTPInfo = CType(ftpInfo.Item(currentStore.StoreNum), StoreFTPConfigBO)

                        'if store no is passed in, then only return this store in the hashtable;
                        'otherwise return configurations for ALL stores
                        If inStoreNo Is Nothing Or (inStoreNo IsNot Nothing AndAlso inStoreNo = currentStore.StoreNum.ToString) Then
                            storeUpdates.Add(currentStore.StoreNum, currentStore)
                        End If
                    Catch e As POSStoreUpdateInitializationException
                        If (Not results.IsDBNull(results.GetOrdinal("Store_No"))) Then
                            storeNo = results.GetInt32(results.GetOrdinal("Store_No")).ToString
                        End If

                        'build config error message for stores so only 1 error message is sent for this POSPush run
                        'containing info for all stores that need to be configured
                        If Not configErrorStores.ContainsKey(storeNo) Then
                            configErrorStores.Add(storeNo, True)

                            storeConfigErrorMsg.Append("Store: ")
                            storeConfigErrorMsg.Append(storeNo)
                            storeConfigErrorMsg.Append(Environment.NewLine)
                        End If
                    End Try
                End While
            Catch e As Exception
                Throw e
            Finally
                ' Close the result set and the connection
                If (results IsNot Nothing) Then
                    results.Close()
                End If
            End Try

            'send config error msg
            If storeConfigErrorMsg.Length > 0 Then
                Dim args(1) As String
                args(0) = storeConfigErrorMsg.ToString
                ErrorHandler.ProcessError(ErrorType.POSPush_StoreConfig, args, SeverityLevel.Warning)
            End If

            Logger.LogDebug("GetStoreConfigurations exit: # stores=" + storeUpdates.Count.ToString, CLASSTYPE)
            Return storeUpdates
        End Function
        ''' <summary>
        ''' Read the list of stores from the StorePOSConfig table and initialze a StoreUpdatesBO
        ''' object in the _storeUpdates Hashatable for each store.
        ''' Executes the Administration_TagPush_GetStoreShelfTagWriterConfigurations stored procedure.
        ''' </summary>
        ''' <remarks></remarks>
        ''' <exception cref="DataFactoryException" />
        Public Shared Function GetShelfTagStoreConfigurations(ByVal fileWriterType As String, Optional ByVal inStoreNo As String = Nothing) As Hashtable
            Logger.LogDebug("GetShelfTagStoreConfigurations entry: fileWriterType=" + fileWriterType, CLASSTYPE)

            Dim storeUpdates As New Hashtable
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As SqlDataReader = Nothing
            Dim currentParam As DBParam
            Dim paramList As ArrayList
            Dim currentStore As StoreUpdatesBO

            'ftp information
            Dim storeFtpConfigDAO As New StoreFTPConfigDAO
            Dim storeFtpConfigBO As StoreFTPConfigBO = Nothing
            Dim ftpInfo As Hashtable = Nothing

            'manage store config errors
            Dim storeNo As String = ""
            Dim storeConfigErrorMsg As New StringBuilder
            Dim configErrorStores As New Hashtable

            Try
                'get store FTP information for multiple writer types
                If inStoreNo IsNot Nothing Then
                    'store-specific info
                    storeFtpConfigBO = storeFtpConfigDAO.GetFTPConfigDataForStoreAndWriterType(CType(inStoreNo, Integer), fileWriterType)
                Else
                    ftpInfo = storeFtpConfigDAO.GetFTPConfigDataForWriterType(fileWriterType)
                End If

                ' setup parameters for stored proc
                paramList = New ArrayList

                currentParam = New DBParam
                currentParam.Name = "FileWriterType"
                currentParam.Value = fileWriterType
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                If inStoreNo IsNot Nothing Then
                    paramList.Add(New DBParam("Store_No", DBParamType.Int, inStoreNo))
                End If

                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataReader("Replenishment_TagPush_GetStoreWriterConfigurations", paramList)

                ' Process each store in the result set
                While (results.Read())
                    Try
                        currentStore = New StoreUpdatesBO(results)

                        'set FTP info for writer type in store object
                        If inStoreNo IsNot Nothing Then
                            'store-specific info
                            currentStore.FTPInfo = storeFtpConfigBO
                        Else
                            currentStore.FTPInfo = CType(ftpInfo.Item(currentStore.StoreNum), StoreFTPConfigBO)
                        End If

                        'if store no is passed in, then only return this store in the hashtable;
                        'otherwise return configurations for ALL stores
                        If inStoreNo Is Nothing Or (inStoreNo IsNot Nothing AndAlso inStoreNo = currentStore.StoreNum.ToString) Then
                            storeUpdates.Add(currentStore.StoreNum, currentStore)
                        End If
                    Catch e As POSStoreUpdateInitializationException
                        If (Not results.IsDBNull(results.GetOrdinal("Store_No"))) Then
                            storeNo = results.GetInt32(results.GetOrdinal("Store_No")).ToString
                        End If

                        'build config error message for stores so only 1 error message is sent for this POSPush run
                        'containing info for all stores that need to be configured
                        If Not configErrorStores.ContainsKey(storeNo) Then
                            configErrorStores.Add(storeNo, True)

                            storeConfigErrorMsg.Append("Store: ")
                            storeConfigErrorMsg.Append(storeNo)
                            storeConfigErrorMsg.Append(Environment.NewLine)
                        End If
                    End Try
                End While
            Catch e As Exception
                Throw e
            Finally
                ' Close the result set and the connection
                If (results IsNot Nothing) Then
                    results.Close()
                    results = Nothing
                End If
                ftpInfo = Nothing
                storeFtpConfigDAO = Nothing
                storeFtpConfigBO = Nothing
            End Try

            'send config error msg
            If storeConfigErrorMsg.Length > 0 Then
                Dim args(1) As String
                args(0) = storeConfigErrorMsg.ToString
                ErrorHandler.ProcessError(ErrorType.POSPush_StoreConfig, args, SeverityLevel.Warning)
            End If

            Logger.LogDebug("GetStoreConfigurations exit: # stores=" + storeUpdates.Count.ToString, CLASSTYPE)
            Return storeUpdates
        End Function

        Public Shared Function GetElectronicShelfTagStoreConfigurations(ByVal fileWriterType As String, Optional ByVal inStoreNo As String = Nothing) As Hashtable
            Logger.LogDebug("GetElectronicShelfTagStoreConfigurations entry: fileWriterType=" + fileWriterType, CLASSTYPE)

            Dim storeUpdates As New Hashtable
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As SqlDataReader = Nothing
            Dim currentParam As DBParam
            Dim paramList As ArrayList
            Dim currentStore As StoreUpdatesBO

            'ftp information
            Dim storeFtpConfigDAO As New StoreFTPConfigDAO
            Dim storeFtpConfigBO As StoreFTPConfigBO = Nothing
            Dim ftpInfo As Hashtable = Nothing

            'manage store config errors
            Dim storeNo As String = ""
            Dim storeConfigErrorMsg As New StringBuilder
            Dim configErrorStores As New Hashtable

            Try
                'get store FTP information for multiple writer types
                If inStoreNo IsNot Nothing Then
                    'store-specific info
                    storeFtpConfigBO = storeFtpConfigDAO.GetFTPConfigDataForStoreAndWriterType(CType(inStoreNo, Integer), fileWriterType)
                Else
                    ftpInfo = storeFtpConfigDAO.GetFTPConfigDataForWriterType(fileWriterType)
                End If

                ' setup parameters for stored proc
                paramList = New ArrayList

                currentParam = New DBParam
                currentParam.Name = "Writer_Type"
                currentParam.Value = fileWriterType
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                If inStoreNo IsNot Nothing Then
                    paramList.Add(New DBParam("Store_No", DBParamType.Int, inStoreNo))
                End If

                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataReader("Administration_TagPush_GetStoreElectronicShelfTagWriterConfigurations", paramList)

                ' Process each store in the result set
                While (results.Read())
                    Try
                        currentStore = New StoreUpdatesBO(results)

                        'set FTP info for writer type in store object
                        If inStoreNo IsNot Nothing Then
                            'store-specific info
                            currentStore.FTPInfo = storeFtpConfigBO
                        Else
                            currentStore.FTPInfo = CType(ftpInfo.Item(currentStore.StoreNum), StoreFTPConfigBO)
                        End If

                        'if store no is passed in, then only return this store in the hashtable;
                        'otherwise return configurations for ALL stores
                        If inStoreNo Is Nothing Or (inStoreNo IsNot Nothing AndAlso inStoreNo = currentStore.StoreNum.ToString) Then
                            storeUpdates.Add(currentStore.StoreNum, currentStore)
                        End If
                    Catch e As POSStoreUpdateInitializationException
                        If (Not results.IsDBNull(results.GetOrdinal("Store_No"))) Then
                            storeNo = results.GetInt32(results.GetOrdinal("Store_No")).ToString
                        End If

                        'build config error message for stores so only 1 error message is sent for this POSPush run
                        'containing info for all stores that need to be configured
                        If Not configErrorStores.ContainsKey(storeNo) Then
                            configErrorStores.Add(storeNo, True)

                            storeConfigErrorMsg.Append("Store: ")
                            storeConfigErrorMsg.Append(storeNo)
                            storeConfigErrorMsg.Append(Environment.NewLine)
                        End If
                    End Try
                End While
            Catch e As Exception
                Throw e
            Finally
                ' Close the result set and the connection
                If (results IsNot Nothing) Then
                    results.Close()
                    results = Nothing
                End If
                ftpInfo = Nothing
                storeFtpConfigDAO = Nothing
                storeFtpConfigBO = Nothing
            End Try

            'send config error msg
            If storeConfigErrorMsg.Length > 0 Then
                Dim args(1) As String
                args(0) = storeConfigErrorMsg.ToString
                ErrorHandler.ProcessError(ErrorType.POSPush_StoreConfig, args, SeverityLevel.Warning)
            End If

            Logger.LogDebug("GetStoreConfigurations exit: # stores=" + storeUpdates.Count.ToString, CLASSTYPE)
            Return storeUpdates
        End Function

        ''' <summary>
        ''' Returns biztalk FTP information to be used in place of direct FTP information
        ''' </summary>
        ''' <param name="ConfigDescription"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetRegionFTPConfigInfo(ByVal ConfigDescription As String) As datatable
            Logger.LogDebug("GetBiztalkFTPConfigInfo entry", CLASSTYPE)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As DataTable = Nothing
            'Dim currentParam As DBParam
            'Dim paramList As ArrayList

            '' setup parameters for stored proc
            'paramList = New ArrayList

            'currentParam = New DBParam
            'currentParam.Name = "Description"
            'currentParam.Value = ConfigDescription
            'currentParam.Type = DBParamType.String
            'paramList.Add(currentParam)

            ' Execute the stored procedure 
            results = factory.GetDataTable("Replenishment_POSPush_GetRegionFTPConfig " & ConfigDescription)

            Logger.LogDebug("GetBiztalkFTPConfigInfo Exit", CLASSTYPE)

            Return results
        End Function
#End Region

    End Class
End Namespace

