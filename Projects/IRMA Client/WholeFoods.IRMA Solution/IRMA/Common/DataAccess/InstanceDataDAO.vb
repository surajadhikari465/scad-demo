Imports System.Data.SqlClient
Imports WholeFoods.IRMA.Common.BusinessLogic
Imports WholeFoods.Utility.DataAccess
Imports log4net

Namespace WholeFoods.IRMA.Common.DataAccess

    Public Class InstanceDataDAO

        ' Define the log4net logger for this class.
        Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)


        ''' <summary>
        ''' gets data in InstanceData table describing the current region's settings
        ''' </summary>
        ''' <returns>InstanceDataBO</returns>
        ''' <remarks></remarks>
        Public Shared Function GetInstanceData() As InstanceDataBO

            logger.Debug("GetInstanceData Entry")

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As SqlDataReader = Nothing
            Dim instance As InstanceDataBO = Nothing

            Try
                results = factory.GetStoredProcedureDataReader("GetInstanceData")

                While results.Read
                    instance = New InstanceDataBO

                    If results.GetValue(results.GetOrdinal("PrimaryRegionName")).GetType IsNot GetType(DBNull) Then
                        instance.RegionName = results.GetString(results.GetOrdinal("PrimaryRegionName"))
                    End If

                    If results.GetValue(results.GetOrdinal("PrimaryRegionCode")).GetType IsNot GetType(DBNull) Then
                        instance.RegionCode = results.GetString(results.GetOrdinal("PrimaryRegionCode"))
                    End If

                    If results.GetValue(results.GetOrdinal("PluDigitsSentToScale")).GetType IsNot GetType(DBNull) Then
                        instance.PluDigitsSentToScale = results.GetString(results.GetOrdinal("PluDigitsSentToScale"))
                    End If

                    If results.GetValue(results.GetOrdinal("UG_Culture")).GetType IsNot GetType(DBNull) Then
                        instance.UG_Culture = results.GetString(results.GetOrdinal("UG_Culture"))
                    End If

                    If results.GetValue(results.GetOrdinal("UG_DateMask")).GetType IsNot GetType(DBNull) Then
                        instance.UG_DateMask = results.GetString(results.GetOrdinal("UG_DateMask"))
                    End If
                End While

                Return instance
            Catch ex As Exception
                logger.Error(ex.Message)
                Throw ex
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

            logger.Debug("GetInstanceData Exit")
        End Function

        ''' <summary>
        ''' gets data in InstanceDataFlags table describing the current region's settings
        ''' </summary>
        ''' <returns>Hashtable of key/value pairs that match the flag key/value data</returns>
        ''' <remarks></remarks>
        Public Shared Function GetInstanceDataFlags() As Hashtable

            logger.Debug("GetInstanceDataFlags Entry")

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
                logger.Error(ex.Message)
                Throw ex
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

            logger.Debug("GetInstanceDataFlags Exit")
        End Function

        ''' <summary>
        ''' Determines if an InstanceDataFlag is active - takes in a store value (or nothing) and determines
        ''' if that store is overridden, or if the default regional value should be used.
        ''' </summary>
        ''' <param name="flagKey">InstanceDataFlag.FlagKey</param>
        ''' <param name="store">passed as a String so that a value of 'Nothing' can be passed in for situations
        ''' where there is no store, and the regional value should be used</param>
        ''' <returns>true or false indicating if the flag is on/off</returns>
        ''' <remarks></remarks>        ''' 
        Public Shared Function IsFlagActive(ByVal flagKey As String, Optional ByVal store As String = Nothing) As Boolean

            logger.Debug("IsFlagActive Entry")

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
                logger.Error(ex.Message)
                Throw ex
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

            logger.Debug("IsFlagActive Exit")

            Return active
        End Function

        Private Shared IsFlagActiveCache As New Hashtable

        ''' <summary>
        ''' Just like IsFlagActive accept caches the value.
        ''' Determines if an InstanceDataFlag is active - takes in a store value (or nothing) and determines
        ''' if that store is overridden, or if the default regional value should be used.
        ''' </summary>
        ''' <param name="flagKey">InstanceDataFlag.FlagKey</param>
        ''' <param name="store">passed as a String so that a value of 'Nothing' can be passed in for situations
        ''' where there is no store, and the regional value should be used</param>
        ''' <returns>true or false indicating if the flag is on/off</returns>
        ''' <remarks></remarks>
        Public Shared Function IsFlagActiveCached(ByVal flagKey As String, Optional ByVal store As String = Nothing) As Boolean

            logger.Debug("IsFlagActiveCached Entry")

            Dim active As Boolean

            If Not InstanceDataDAO.IsFlagActiveCache.ContainsKey(flagKey) Then
                active = InstanceDataDAO.IsFlagActive(flagKey, store)
                InstanceDataDAO.IsFlagActiveCache.Add(flagKey, active)
            Else
                active = CBool(InstanceDataDAO.IsFlagActiveCache.Item(flagKey))
            End If

            logger.Debug("IsFlagActiveCached Exit")


            Return active
        End Function

        Public Shared Sub SetInstanceDataFlag(ByVal inKey As String, ByVal inValue As Boolean)

            logger.Debug("SetInstanceDataFlag Entry")


            Dim factory As New DataFactory(DataFactory.ItemCatalog)

            ' Update the InstanceDataFlags record
            ' setup parameters for stored proc
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            currentParam = New DBParam
            currentParam.Name = "FlagKey"
            currentParam.Value = inKey
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "FlagValue"
            currentParam.Value = inValue
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            ' Execute the stored procedure to update the Instance Data record.
            factory.ExecuteStoredProcedure("Administration_UpdateInstanceDataFlags", paramList)

            logger.Debug("SetInstanceDataFlag Exit")

        End Sub

        Public Shared Function FlagHasNoStoreOverrides(ByVal flagKey As String) As Boolean

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As SqlDataReader = Nothing
            Dim allStoresOnGPM As Boolean = False
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            ' setup parameters for stored proc
            currentParam = New DBParam
            currentParam.Name = "FlagKey"
            currentParam.Value = flagKey
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            ' Execute the stored procedure to query the Instance Data record.
            results = factory.GetStoredProcedureDataReader("FlagHasNoStoreOverrides", paramList)

            While results.Read
                allStoresOnGPM = results.GetBoolean(results.GetOrdinal("FlagHasNoStoreOverrides"))
            End While

            Return allStoresOnGPM
        End Function
    End Class

End Namespace
