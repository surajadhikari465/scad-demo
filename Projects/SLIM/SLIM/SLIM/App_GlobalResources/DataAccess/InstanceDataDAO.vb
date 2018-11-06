Imports System.Data.SqlClient
Imports SLIM.WholeFoods.IRMA.Common.BusinessLogic
Imports SLIM.WholeFoods.Utility.DataAccess

Namespace WholeFoods.IRMA.Common.DataAccess

    Public Class InstanceDataDAO

        ''' <summary>
        ''' gets data in InstanceData table describing the current region's settings
        ''' </summary>
        ''' <returns>InstanceDataBO</returns>
        ''' <remarks></remarks>
        Public Shared Function GetInstanceData() As InstanceDataBO
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
                End While

                Return instance
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
        ''' Determines if an InstanceDataFlag is active - takes in a store value (or nothing) and determines
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
            Dim active As Boolean

            If Not InstanceDataDAO.IsFlagActiveCache.ContainsKey(flagKey) Then
                active = InstanceDataDAO.IsFlagActive(flagKey, store)
                InstanceDataDAO.IsFlagActiveCache.Add(flagKey, active)
            Else
                active = CBool(InstanceDataDAO.IsFlagActiveCache.Item(flagKey))
            End If

            Return active
        End Function

    End Class

End Namespace
