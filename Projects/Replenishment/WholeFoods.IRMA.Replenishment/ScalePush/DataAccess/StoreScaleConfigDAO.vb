Imports System.Data
Imports System.Data.Common
Imports System.Data.SqlClient
Imports System.Text
Imports WholeFoods.IRMA.Replenishment.Common.BusinessLogic
Imports WholeFoods.IRMA.Replenishment.Common.DataAccess
Imports WholeFoods.IRMA.Replenishment.POSPush.BusinessLogic
Imports WholeFoods.IRMA.Replenishment.ScalePush.ScaleException
Imports WholeFoods.Utility
Imports WholeFoods.Utility.DataAccess

Namespace WholeFoods.IRMA.Replenishment.ScalePush.DataAccess

    Public Class StoreScaleConfigDAO
        ' Set the class type for logging
        Private Shared CLASSTYPE As Type = System.Type.GetType("WholeFoods.IRMA.Replenishment.ScalePush.DataAccess.StoreScaleConfigDAO")

#Region "Read methods"

        ''' <summary>
        ''' Read the list of stores from the StoreScaleConfig table and initialze a StoreUpdatesBO
        ''' object in the _storeUpdates Hashatable for each store.
        ''' Executes the Replenishment_POSPush_GetStoreWriterConfigurations stored procedure.
        ''' </summary>
        ''' <remarks></remarks>
        ''' <exception cref="DataFactoryException" />
        Public Shared Function GetStoreConfigurations(ByVal fileWriterType As String, ByVal isRegionalScaleFile As Boolean) As Hashtable
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
                results = factory.GetStoredProcedureDataReader("Replenishment_ScalePush_GetStoreWriterConfigurations", paramList)

                ' Process each store in the result set
                While (results.Read())
                    Try
                        currentStore = New StoreUpdatesBO(results)

                        'set FTP info for writer type in store object
                        currentStore.FTPInfo = CType(ftpInfo.Item(currentStore.StoreNum), StoreFTPConfigBO)

                        If isRegionalScaleFile Then
                            'key writer definitions on ScaleWriterType because there will be multiple writers 
                            'for the regional Store entry
                            storeUpdates.Add(currentStore.FileWriter.ScaleWriterType, currentStore)
                        Else
                            'key writer definitions on Store_No
                            storeUpdates.Add(currentStore.StoreNum, currentStore)
                        End If
                    Catch e As ScaleStoreUpdateInitializationException
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

#End Region

    End Class

End Namespace