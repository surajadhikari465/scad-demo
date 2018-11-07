Imports WholeFoods.IRMA.Replenishment.Common.BusinessLogic
Imports System.Configuration
Imports System.Data.SqlClient
Imports WholeFoods.Utility
Imports WholeFoods.Utility.DataAccess

Namespace WholeFoods.IRMA.Replenishment.Common.DataAccess

    Public Class StoreFTPConfigDAO

        ' Set the class type for logging
        Private Shared CLASSTYPE As Type = System.Type.GetType("IRMA.Common.DataAccess.StoreFTPConfigDAO")

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

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
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
                results = factory.GetStoredProcedureDataReader("Replenishment_POSPush_GetFTPConfigForStoreAndWriterType", paramList)

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

        ''' <summary>
        ''' gets StoreFTPConfig data for all stores and a specific file writer type;
        ''' returns a Hashtable of StoreFTPConfigBO objects keyed on Store_No
        ''' </summary>
        ''' <param name="fileWriterType"></param>
        ''' <returns>Hashtable of StoreFTPConfigBO objects keyed on Store_No</returns>
        ''' <remarks></remarks>
        Public Function GetFTPConfigDataForWriterType(ByVal fileWriterType As String) As Hashtable
            Logger.LogDebug("GetFTPConfigDataForStore entry: fileWriterType=" + fileWriterType, CLASSTYPE)

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As SqlDataReader = Nothing
            Dim currentParam As DBParam
            Dim paramList As ArrayList
            Dim ftpConfigHash As New Hashtable
            Dim ftpConfigInfo As StoreFTPConfigBO = Nothing
            Dim dtXML As New DataTable

            Dim bUseBiztalk As Boolean
            bUseBiztalk = InstanceDataDAO.IsFlagActive("UseBiztalkPOSPush")

            ' setup parameters for stored proc
            paramList = New ArrayList

            currentParam = New DBParam
            currentParam.Name = "FileWriterType"
            currentParam.Value = fileWriterType
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            Try
                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataReader("Replenishment_POSPush_GetFTPConfigForWriterType", paramList)

                While results.Read
                    ftpConfigInfo = New StoreFTPConfigBO(results)

                    'add store ftp object to hash
                    ftpConfigHash.Add(ftpConfigInfo.StoreNo, ftpConfigInfo)
                End While
            Catch ex As Exception
                Throw ex
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

            Logger.LogDebug("GetFTPConfigDataForStore exit", CLASSTYPE)

            Return ftpConfigHash
        End Function

        ''' <summary>
        ''' gets StoreFTPConfig data for all stores and all file writer types;
        ''' returns a Hashtable of Hashtables keyed on file writer type; inner Hashtable contains StoreFTPConfigBO objects keyed on Store_No
        ''' </summary>
        ''' <returns>Hashtable of Hashtables keyed on file writer type; inner Hashtable contains StoreFTPConfigBO objects keyed on Store_No</returns>
        ''' <remarks></remarks>
        Public Function GetAllFTPConfigData() As Hashtable
            Logger.LogDebug("GetAllStoreFTPConfigData entry", CLASSTYPE)

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As SqlDataReader = Nothing
            Dim currentWriterType As String
            Dim previousWriterType As String = "-1"
            Dim ftpConfigHash As New Hashtable
            Dim ftpStoreConfigHash As New Hashtable
            Dim ftpConfigInfo As StoreFTPConfigBO = Nothing

            Try
                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataReader("Replenishment_POSPush_GetAllFTPConfigData")

                While results.Read
                    ftpConfigInfo = New StoreFTPConfigBO(results)

                    'get current record of data
                    currentWriterType = ftpConfigInfo.FileWriterType

                    'if this is a new writer type then create a new hash to house store objects for this writer type
                    If Not currentWriterType.Equals(previousWriterType) AndAlso Not previousWriterType.Equals("-1") Then
                        'add store hash to main hash
                        ftpConfigHash.Add(previousWriterType, ftpStoreConfigHash)

                        'create new inner store hash
                        ftpStoreConfigHash = New Hashtable
                    End If

                    'add store ftp object to store hash
                    ftpStoreConfigHash.Add(ftpConfigInfo.StoreNo, ftpConfigInfo)

                    previousWriterType = currentWriterType
                End While

                If Not previousWriterType.Equals("-1") Then
                    'add last writer type hash to main hash
                    ftpConfigHash.Add(previousWriterType, ftpStoreConfigHash)
                End If
            Catch ex As Exception
                Throw ex
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

            Logger.LogDebug("GetAllStoreFTPConfigData exit", CLASSTYPE)

            Return ftpConfigHash
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

        Public Sub UpdateFTPPassword(ByVal IPAddress As String, ByVal Password As String)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            Try
                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "IP_Address"
                currentParam.Value = IPAddress
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "FTP_Password"
                currentParam.Value = Password
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                ' Execute the stored procedure 
                factory.ExecuteStoredProcedure("Administration_POSPush_UpdateStoreFTPPassword", paramList)

            Catch ex As Exception
                Throw ex
            End Try

        End Sub
        ''' <summary>
        ''' build DBParams and call stored procedure to insert or update data based on flag
        ''' </summary>
        ''' <param name="isInsert"></param>
        ''' <param name="storeFTPConfig"></param>
        ''' <remarks></remarks>
        Private Sub InsertOrUpdateData(ByVal isInsert As Boolean, ByVal storeFTPConfig As StoreFTPConfigBO)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
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
                    factory.ExecuteStoredProcedure("Administration_POSPush_InsertStoreFTPConfig", paramList)
                Else
                    factory.ExecuteStoredProcedure("Administration_POSPush_UpdateStoreFTPConfig", paramList)
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
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
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
                factory.ExecuteStoredProcedure("Administration_POSPush_DeleteStoreFTPConfig", paramList)
            Catch ex As Exception
                Throw ex
            End Try
        End Sub

#End Region

    End Class

End Namespace
