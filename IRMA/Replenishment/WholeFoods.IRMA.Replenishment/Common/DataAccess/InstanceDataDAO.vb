Imports System.Data.SqlClient
Imports WholeFoods.IRMA.Replenishment.Common.BusinessLogic
Imports WholeFoods.Utility.DataAccess

Namespace WholeFoods.IRMA.Replenishment.Common.DataAccess

    Public Class InstanceDataDAO

        ''' <summary>
        ''' gets data in InstanceData table describing the current region's settings
        ''' </summary>
        ''' <returns>InstanceDataBO</returns>
        ''' <remarks></remarks>
        Public Shared Function GetInstanceData() As InstanceDataBO
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As SqlDataReader = Nothing
            Dim instanceData As InstanceDataBO = Nothing

            Try
                results = factory.GetStoredProcedureDataReader("GetInstanceData")

                While results.Read
                    instanceData = New InstanceDataBO

                    If results.GetValue(results.GetOrdinal("PrimaryRegionName")).GetType IsNot GetType(DBNull) Then
                        instanceData.RegionName = results.GetString(results.GetOrdinal("PrimaryRegionName"))
                    End If

                    If results.GetValue(results.GetOrdinal("PrimaryRegionCode")).GetType IsNot GetType(DBNull) Then
                        instanceData.RegionCode = results.GetString(results.GetOrdinal("PrimaryRegionCode"))
                    End If

                    If results.GetValue(results.GetOrdinal("PluDigitsSentToScale")).GetType IsNot GetType(DBNull) Then
                        instanceData.PluDigitsSentToScale = results.GetString(results.GetOrdinal("PluDigitsSentToScale"))
                    End If

                    If results.GetValue(results.GetOrdinal("UG_Culture")).GetType IsNot GetType(DBNull) Then
                        instanceData.UGCulture = results.GetString(results.GetOrdinal("UG_Culture"))
                    End If

                    If results.GetValue(results.GetOrdinal("UG_DateMask")).GetType IsNot GetType(DBNull) Then
                        instanceData.UGDateMask = results.GetString(results.GetOrdinal("UG_DateMask"))
                    End If
                End While

                Return instanceData
            Catch ex As Exception
                Throw ex
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try
        End Function

        ''' <summary>
        ''' determines if an InstanceDataFlag is active - takes in a store value (or nothing) and determines
        ''' if that store is overridden, or if the default regional value should be used.
        ''' </summary>
        ''' <param name="flagKey">InstanceDataFlag.FlagKey</param>
        ''' <param name="store">passed as a String so that a value of 'Nothing' can be passed in for situations
        ''' where there is no store, and the regional value should be used</param>
        ''' <returns>true or false indicating if the flag is on/off</returns>
        ''' <remarks></remarks>        ''' 
        Public Shared Function IsFlagActive(ByVal flagKey As String, Optional ByVal store As String = Nothing) As Boolean
            Dim active As Boolean
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As SqlDataReader = Nothing
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            Try
                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "FlagKey"
                currentParam.Value = flagKey
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Store_No"
                If store IsNot Nothing Then
                    currentParam.Value = Integer.Parse(store)
                Else
                    currentParam.Value = DBNull.Value
                End If
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                results = factory.GetStoredProcedureDataReader("GetInstanceDataFlagValue", paramList)

                While results.Read
                    active = results.GetBoolean(results.GetOrdinal("FlagValue"))
                End While
            Catch ex As Exception
                Throw ex
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

            Return active
        End Function

        ''' <summary>
        ''' gets data in InstanceDataFlags table describing the current region's settings
        ''' </summary>
        ''' <returns>Hashtable of key/value pairs that match the flag key/value data</returns>
        ''' <remarks></remarks>
        Public Shared Function GetInstanceDataFlags() As Hashtable
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As SqlDataReader = Nothing
            Dim flagMap As New Hashtable
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            Try
                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "LimitByStoreOverride"
                currentParam.Value = False
                currentParam.Type = DBParamType.Bit
                paramList.Add(currentParam)

                results = factory.GetStoredProcedureDataReader("GetInstanceDataFlags", paramList)

                While results.Read
                    flagMap.Add(results.GetString(results.GetOrdinal("FlagKey")), results.GetBoolean(results.GetOrdinal("FlagValue")))
                End While

                Return flagMap
            Catch ex As Exception
                Throw ex
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try
        End Function

        ''' <summary>
        ''' gets data in InstanceDataFlags table describing the current region's settings
        ''' </summary>
        ''' <returns>ArrayList of InstanceDataFlagsBO objects</returns>
        ''' <remarks></remarks>
        Public Shared Function GetInstanceDataFlagsBOList() As ArrayList
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As SqlDataReader = Nothing
            Dim flagList As New ArrayList
            Dim flagBO As InstanceDataFlagsBO
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            Try
                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "LimitByStoreOverride"
                currentParam.Value = False
                currentParam.Type = DBParamType.Bit
                paramList.Add(currentParam)

                results = factory.GetStoredProcedureDataReader("GetInstanceDataFlags", paramList)

                While results.Read
                    flagBO = New InstanceDataFlagsBO
                    flagBO.FlagKey = results.GetString(results.GetOrdinal("FlagKey"))
                    flagBO.FlagValue = results.GetBoolean(results.GetOrdinal("FlagValue"))

                    flagList.Add(flagBO)
                End While

                Return flagList
            Catch ex As Exception
                Throw ex
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try
        End Function

        Public Shared Function GetAllInstanceDataFlags() As DataTable
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As DataTable = Nothing
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            Try
                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "LimitByStoreOverride"
                currentParam.Value = False
                currentParam.Type = DBParamType.Bit
                paramList.Add(currentParam)

                results = factory.GetStoredProcedureDataTable("GetInstanceDataFlags", paramList)



                Return results
            Catch ex As Exception
                Throw ex
            Finally

            End Try
        End Function

        ''' <summary>
        ''' gets data in InstanceDataFlagsStoreOverride table for the given key;  does not include any key values
        ''' found in the ArrayList passed in (removeFlagList)
        ''' </summary>
        ''' <param name="flagKey"></param>
        ''' <param name="removeFlagList"></param>
        ''' <returns>ArrayList of InstanceDataFlagsBO objects that have override values</returns>
        ''' <remarks></remarks>
        Public Shared Function GetInstanceDataFlagsForKey(ByVal flagKey As String, ByVal removeFlagList As ArrayList) As ArrayList
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As SqlDataReader = Nothing
            Dim storeList As New ArrayList
            Dim flagBO As InstanceDataFlagsBO
            Dim currentParam As DBParam
            Dim paramList As ArrayList

            Dim currentRemoveFlag As InstanceDataFlagsBO
            Dim removeFlagEnum As IEnumerator = Nothing
            Dim isAddCurrentFlag As Boolean

            Try
                If removeFlagList IsNot Nothing Then
                    removeFlagEnum = removeFlagList.GetEnumerator
                End If

                ' setup parameters for stored proc
                paramList = New ArrayList

                currentParam = New DBParam
                currentParam.Name = "FlagKey"
                currentParam.Value = flagKey
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                results = factory.GetStoredProcedureDataReader("GetInstanceDataFlagsStoreOverrideList", paramList)

                While results.Read
                    flagBO = New InstanceDataFlagsBO
                    flagBO.StoreNo = results.GetInt32(results.GetOrdinal("Store_No"))
                    flagBO.StoreName = results.GetString(results.GetOrdinal("Store_Name"))
                    flagBO.FlagKey = flagKey
                    flagBO.FlagValue = results.GetBoolean(results.GetOrdinal("FlagValue"))

                    'do not add this override flag if it is contained in the removeFlagList
                    If removeFlagList IsNot Nothing AndAlso removeFlagList.Count > 0 Then
                        removeFlagEnum.Reset()
                        isAddCurrentFlag = False

                        While removeFlagEnum.MoveNext
                            currentRemoveFlag = CType(removeFlagEnum.Current, InstanceDataFlagsBO)

                            If currentRemoveFlag.FlagKey = flagBO.FlagKey AndAlso currentRemoveFlag.StoreNo = flagBO.StoreNo Then
                                'do not add this override flag
                                isAddCurrentFlag = False
                                Exit While
                            Else
                                isAddCurrentFlag = True
                            End If
                        End While

                        If isAddCurrentFlag Then
                            'add tax flag to list
                            storeList.Add(flagBO)
                        End If
                    Else
                        'add tax flag to list
                        storeList.Add(flagBO)
                    End If
                End While

                Return storeList
            Catch ex As Exception
                Throw ex
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try
        End Function

        ''' <summary>
        ''' gets list of stores that have not been overridden for the given FlagKey
        ''' </summary>
        ''' <param name="flagKey"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetAvailableStoreOverrides(ByVal flagKey As String, ByVal deletedFlags As ArrayList) As ArrayList
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As SqlDataReader = Nothing
            Dim flagList As New ArrayList
            Dim flagBO As InstanceDataFlagsBO
            Dim currentParam As DBParam
            Dim paramList As ArrayList
            Dim deletedFlagsEnum As IEnumerator
            Dim currentDeletedFlag As InstanceDataFlagsBO

            Try
                ' setup parameters for stored proc
                paramList = New ArrayList

                currentParam = New DBParam
                currentParam.Name = "FlagKey"
                currentParam.Value = flagKey
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                results = factory.GetStoredProcedureDataReader("GetInstanceDataAvailableStoreOverrides", paramList)

                While results.Read
                    flagBO = New InstanceDataFlagsBO
                    flagBO.FlagKey = flagKey
                    flagBO.StoreNo = results.GetInt32(results.GetOrdinal("Store_No"))
                    flagBO.StoreName = results.GetString(results.GetOrdinal("Store_Name"))

                    flagList.Add(flagBO)
                End While

                'add any deleted flags that pertain to the same flagKey value
                deletedFlagsEnum = deletedFlags.GetEnumerator
                While deletedFlagsEnum.MoveNext
                    currentDeletedFlag = CType(deletedFlagsEnum.Current, InstanceDataFlagsBO)

                    If currentDeletedFlag.FlagKey = flagKey Then
                        flagList.Add(currentDeletedFlag)
                    End If
                End While

                Return flagList
            Catch ex As Exception
                Throw ex
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try
        End Function

        Public Sub InsertInstanceDataFlagsStoreOverride(ByVal insertFlag As InstanceDataFlagsBO, ByVal transaction As SqlTransaction)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            Try
                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "Store_No"
                currentParam.Value = insertFlag.StoreNo
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "FlagKey"
                currentParam.Value = insertFlag.FlagKey
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "FlagValue"
                currentParam.Value = insertFlag.FlagValue
                currentParam.Type = DBParamType.Bit
                paramList.Add(currentParam)

                ' Execute the stored procedure 
                factory.ExecuteStoredProcedure("InsertInstanceDataFlagsStoreOverride", paramList, transaction)
            Catch ex As Exception
                Throw ex
            End Try
        End Sub

        ''' <summary>
        ''' Update InstanceDataFlag StoreOverride values
        ''' </summary>
        ''' <param name="updateList"></param>
        ''' <param name="transaction"></param>
        ''' <remarks></remarks>
        Public Sub UpdateInstanceDataFlagsStoreOverride(ByVal updateList As ArrayList, ByVal transaction As SqlTransaction)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim updateEnum As IEnumerator
            Dim currentUpdateFlag As InstanceDataFlagsBO

            Try
                updateEnum = updateList.GetEnumerator
                While updateEnum.MoveNext
                    paramList.Clear()
                    currentUpdateFlag = CType(updateEnum.Current, InstanceDataFlagsBO)
                    currentParam = New DBParam
                    currentParam.Name = "Store_No"
                    currentParam.Value = currentUpdateFlag.StoreNo
                    currentParam.Type = DBParamType.Int
                    paramList.Add(currentParam)

                    currentParam = New DBParam
                    currentParam.Name = "FlagKey"
                    currentParam.Value = currentUpdateFlag.FlagKey
                    currentParam.Type = DBParamType.String
                    paramList.Add(currentParam)

                    currentParam = New DBParam
                    currentParam.Name = "FlagValue"
                    currentParam.Value = currentUpdateFlag.FlagValue
                    currentParam.Type = DBParamType.Bit
                    paramList.Add(currentParam)

                    factory.ExecuteStoredProcedure("UpdateInstanceDataFlagsStoreOverride", paramList, transaction)
                End While
            Catch ex As Exception
                Throw ex
            End Try
        End Sub
        ''' <summary>
        ''' deletes store overrides for the given FlagKey
        ''' </summary>
        ''' <param name="removeFlag"></param>
        ''' <param name="transaction"></param>
        ''' <remarks></remarks>
        Public Sub DeleteInstanceDataFlagsStoreOverride(ByVal removeFlag As InstanceDataFlagsBO, ByVal transaction As SqlTransaction)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            Try
                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "Store_No"
                currentParam.Value = removeFlag.StoreNo
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "FlagKey"
                currentParam.Value = removeFlag.FlagKey
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                ' Execute the stored procedure 
                factory.ExecuteStoredProcedure("DeleteInstanceDataFlagsStoreOverride", paramList, transaction)
            Catch ex As Exception
                Throw ex
            End Try
        End Sub

        ''' <summary>
        ''' updates data in the InstanceData table; currently only the 'PluDigitsSentToScale' field is updatable via the UI
        ''' </summary>
        ''' <param name="data"></param>
        ''' <remarks></remarks>
        Public Sub UpdateInstanceData(ByVal data As InstanceDataBO)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            Try
                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "PluDigitsSentToScale"
                currentParam.Value = data.PluDigitsSentToScale
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "UGCulture"
                currentParam.Value = data.UGCulture
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "UGDateMask"
                currentParam.Value = data.UGDateMask
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                ' Execute the stored procedure 
                factory.ExecuteStoredProcedure("UpdateInstanceData", paramList)
            Catch ex As Exception
                Throw ex
            End Try
        End Sub

        ''' <summary>
        ''' Update InstanceDataFlag Values and CanStoreOverride
        ''' </summary>
        ''' <param name="updateList"></param>
        ''' <param name="transaction"></param>
        ''' <remarks></remarks>
        Public Sub UpdateInstanceDataFlagValues(ByVal updateList As ArrayList, ByVal transaction As SqlTransaction)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim updateEnum As IEnumerator
            Dim currentUpdateFlag As InstanceDataFlagsBO

            Try
                updateEnum = updateList.GetEnumerator
                While updateEnum.MoveNext
                    paramList.Clear()
                    currentUpdateFlag = CType(updateEnum.Current, InstanceDataFlagsBO)

                    currentParam = New DBParam
                    currentParam.Name = "FlagKey"
                    currentParam.Value = currentUpdateFlag.FlagKey
                    currentParam.Type = DBParamType.String
                    paramList.Add(currentParam)

                    currentParam = New DBParam
                    currentParam.Name = "FlagValue"
                    currentParam.Value = currentUpdateFlag.FlagValue
                    currentParam.Type = DBParamType.Bit
                    paramList.Add(currentParam)

                    currentParam = New DBParam
                    currentParam.Name = "CanStoreOverride"
                    currentParam.Value = currentUpdateFlag.CanStoreOverride
                    currentParam.Type = DBParamType.Int
                    paramList.Add(currentParam)

                    factory.ExecuteStoredProcedure("UpdateInstanceDataFlagsValues", paramList, transaction)
                End While
            Catch ex As Exception
                Throw ex
            End Try
        End Sub

        ''' <summary>
        ''' gets SqlTransaction object
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetTransaction() As SqlTransaction
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim transaction As SqlTransaction = factory.BeginTransaction(IsolationLevel.ReadCommitted)

            Return transaction
        End Function

    End Class

End Namespace
