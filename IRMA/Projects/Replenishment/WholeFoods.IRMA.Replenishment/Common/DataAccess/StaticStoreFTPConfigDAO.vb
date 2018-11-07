Imports WholeFoods.IRMA.Replenishment.Common.BusinessLogic
Imports System.Configuration
Imports System.Data.SqlClient
Imports WholeFoods.Utility
Imports WholeFoods.Utility.DataAccess

Namespace WholeFoods.IRMA.Replenishment.Common.DataAccess

    Public Class StaticStoreFTPConfigDAO

        ' Set the class type for logging
        Private Shared CLASSTYPE As Type = System.Type.GetType("IRMA.Common.DataAccess.StaticStoreFTPConfigDAO")

#Region "read methods"

        ''' <summary>
        ''' gets StoreFTPConfig data for 1 specific store and file writer type combination.
        ''' </summary>
        ''' <param name="storeNo"></param>
        ''' <param name="fileWriterType"></param>
        ''' <returns>StoreFTPConfigBO object</returns>
        ''' <remarks></remarks>
        Public Function GetFTPConfigDataForStoreAndWriterType(ByVal storeNo As Integer, ByVal fileWriterType As String) As StoreFTPConfigBO
            Logger.LogDebug("GetStoreFTPConfigDataForWriterType entry: storeNo=" + storeNo.ToString + "; fileWriterType=" + fileWriterType, CLASSTYPE)

            Dim factory As New DataFactory(DataFactory.StaticEnv)
            Dim results As SqlDataReader = Nothing
            Dim currentParam As DBParam
            Dim paramList As ArrayList
            Dim ftpConfigInfo As StoreFTPConfigBO = Nothing

            ' setup parameters for stored proc
            paramList = New ArrayList

            currentParam = New DBParam
            currentParam.Name = "Store_No"
            currentParam.Value = storeNo
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "FileWriterType"
            currentParam.Value = fileWriterType
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            Try
                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataReader("Administration_StaticEnv_GetFTPConfigForStoreAndWriterType", paramList)

                While results.Read
                    ftpConfigInfo = New StoreFTPConfigBO(results)
                End While
            Catch ex As Exception
                Throw ex
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

            Logger.LogDebug("GetStoreFTPConfigDataForWriterType exit", CLASSTYPE)

            Return ftpConfigInfo
        End Function
#End Region

#Region "write methods"

        ''' <summary>
        ''' Insert StoreFTPConfig data
        ''' </summary>
        ''' <param name="storeFTPConfig"></param>
        ''' <remarks></remarks>
        Public Sub InsertFTPInfo(ByVal storeFTPConfig As StoreFTPConfigBO)
            InsertOrUpdateData(True, storeFTPConfig)
        End Sub

        ''' <summary>
        ''' Update StoreFTPConfig data
        ''' </summary>
        ''' <param name="storeFTPConfig"></param>
        ''' <remarks></remarks>
        Public Sub UpdateFTPInfo(ByVal storeFTPConfig As StoreFTPConfigBO)
            InsertOrUpdateData(False, storeFTPConfig)
        End Sub

        ''' <summary>
        ''' build DBParams and call stored procedure to insert or update data based on flag
        ''' </summary>
        ''' <param name="isInsert"></param>
        ''' <param name="storeFTPConfig"></param>
        ''' <remarks></remarks>
        Private Sub InsertOrUpdateData(ByVal isInsert As Boolean, ByVal storeFTPConfig As StoreFTPConfigBO)
            Dim factory As New DataFactory(DataFactory.StaticEnv)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            Try
                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "Store_No"
                currentParam.Value = storeFTPConfig.StoreNo
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "FileWriterType"
                currentParam.Value = storeFTPConfig.FileWriterType
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "IP_Address"
                currentParam.Value = storeFTPConfig.IPAddress
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "FTP_User"
                currentParam.Value = storeFTPConfig.FTPUser
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "FTP_Password"
                currentParam.Value = storeFTPConfig.FTPPassword
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "ChangeDirectory"
                currentParam.Value = storeFTPConfig.ChangeDirectory
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Port"
                If storeFTPConfig.Port IsNot Nothing AndAlso Not storeFTPConfig.Port.Trim.Equals("") Then
                    currentParam.Value = CType(storeFTPConfig.Port, Integer)
                Else
                    currentParam.Value = DBNull.Value
                End If
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "IsSecureTransfer"
                currentParam.Value = storeFTPConfig.IsSecureTransfer
                currentParam.Type = DBParamType.Bit
                paramList.Add(currentParam)

                ' Execute the stored procedure 
                If isInsert Then
                    factory.ExecuteStoredProcedure("Administration_StaticEnv_InsertStoreFTPConfig", paramList)
                Else
                    factory.ExecuteStoredProcedure("Administration_StaticEnv_UpdateStoreFTPConfig", paramList)
                End If
            Catch ex As Exception
                Throw ex
            End Try
        End Sub


        ''' <summary>
        ''' build DBParams and call stored procedure to delete data based on flags
        ''' </summary>
        ''' <param name="storeFTPConfig"></param>
        ''' <remarks></remarks>
        Public Sub DeleteFTPInfo(ByVal storeFTPConfig As StoreFTPConfigBO)
            Dim factory As New DataFactory(DataFactory.StaticEnv)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            Try
                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "Store_No"
                currentParam.Value = storeFTPConfig.StoreNo
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "FileWriterType"
                currentParam.Value = storeFTPConfig.FileWriterType
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                ' Execute the stored procedure 
                factory.ExecuteStoredProcedure("Administration_StaticEnv_DeleteStoreFTPConfig", paramList)
            Catch ex As Exception
                Throw ex
            End Try
        End Sub

#End Region

    End Class

End Namespace
