Imports log4net
Imports System.Data.SqlClient
Imports WholeFoods.IRMA.Administration.Common.BusinessLogic
Imports WholeFoods.Utility
Imports WholeFoods.Utility.DataAccess

Namespace WholeFoods.IRMA.Administration.StoreAdmin.DataAccess

    Public Class StoreDAO
        ' Define the log4net logger for this class.
        Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

        ' Set the class type for logging
        Private Shared CLASSTYPE As Type = System.Type.GetType("IRMA.Administration.StoreAdmin.DataAccess.StoreDAO")

#Region "Read Methods"

        ''' <summary>
        ''' Gets a list of all stores;
        ''' </summary>
        ''' <returns>ArrayList of StoreBO objects</returns>
        ''' <remarks></remarks>
        Public Shared Function GetStores() As ArrayList
            logger.Debug("GetStores entry")

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As SqlDataReader = Nothing
            Dim storeList As New ArrayList
            Dim store As StoreBO = Nothing

            Try
                results = factory.GetStoredProcedureDataReader("GetStores")

                While results.Read
                    store = New StoreBO(results)
                    storeList.Add(store)
                End While

            Catch ex As Exception
                Throw ex

            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

            logger.Debug("GetStores exit")

            Return storeList
        End Function

        ''' <summary>
        ''' Gets a list of all stores plus distribution and manufacturing facilities;
        ''' </summary>
        ''' <returns>ArrayList of StoreBO objects</returns>
        ''' <remarks></remarks>
        Public Shared Function GetStoresAndFacilities() As ArrayList
            logger.Debug("GetStoresAndFacilities entry")

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As SqlDataReader = Nothing
            Dim storeList As New ArrayList
            Dim store As StoreBO = Nothing

            Try
                results = factory.GetStoredProcedureDataReader("GetStoresAndDist")

                While results.Read
                    store = New StoreBO(results)
                    storeList.Add(store)
                End While

            Catch ex As Exception
                Throw ex

            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

            logger.Debug("GetStoresAndFacilities exit")

            Return storeList
        End Function

        ''' <summary>
        ''' gets list of all stores 
        ''' </summary>
        ''' <returns>ArrayList of StoreBO objects</returns>
        ''' <remarks></remarks>
        Public Shared Function GetRetailStores() As ArrayList()
            logger.Debug("GetRetailStores entry")

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As SqlDataReader = Nothing
            Dim storeList As New ArrayList
            Dim zoneList As New ArrayList
            Dim stateList As New ArrayList

            Dim store As StoreBO = Nothing
            Dim zone As ZoneBO = Nothing
            Dim state As StateBO = Nothing

            Try
                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataReader("GetRetailStores")
                Dim zoneHash As New Hashtable
                Dim zoneEnum As IEnumerator
                Dim stateHash As New Hashtable
                Dim stateEnum As IEnumerator

                While results.Read
                    ' Always add the store to the store list
                    store = New StoreBO(results, True)
                    storeList.Add(store)

                    ' Has this zone already been added to the zone list?
                    zone = New ZoneBO(results)
                    If Not zoneHash.ContainsKey(zone.ZoneId) Then
                        zoneHash.Add(zone.ZoneId, zone.ZoneName)
                        zoneList.Add(zone)
                    End If
                    ' Add the current store to the list of stores for the zone.
                    zoneEnum = zoneList.GetEnumerator()
                    While (zoneEnum.MoveNext())
                        zone = CType(zoneEnum.Current, ZoneBO)
                        If store.ZoneId.Equals(zone.ZoneId) Then
                            ' Add this store to the zone 
                            zone.StoreNoList.Add(store)
                            Exit While
                        End If
                    End While

                    ' Has this state already been added to the state list?
                    state = New StateBO(results)
                    If Not stateHash.ContainsKey(state.State) Then
                        stateHash.Add(state.State, state.State)
                        stateList.Add(state)
                    End If
                    ' Add the current store to the list of stores for the state.
                    stateEnum = stateList.GetEnumerator()
                    While (stateEnum.MoveNext())
                        state = CType(stateEnum.Current, StateBO)
                        If store.State.Equals(state.State) Then
                            ' Add this store to the zone 
                            state.StoreNoList.Add(store)
                            Exit While
                        End If
                    End While

                End While
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

            Dim returnList(3) As ArrayList
            returnList(0) = storeList
            returnList(1) = zoneList
            returnList(2) = stateList
            logger.Debug("GetRetailStores exit: store count=" + storeList.Count.ToString + ", zone count=" + zoneList.Count.ToString + ", state count=" + stateList.Count.ToString)
            Return returnList
        End Function

#End Region

#Region "Write Methods"


        Public Sub UpdateStore(ByVal store As StoreBO)
            logger.Debug("UpdateStore entry")

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            'setup parameters for stored proc
            currentParam = New DBParam
            currentParam.Name = "Store_No"
            currentParam.Value = store.StoreNo
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "POSSystemId"
            currentParam.Value = store.POSSystem.POSSystemID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            ' Execute the stored procedure 
            factory.ExecuteStoredProcedure("Administration_UpdateStore", paramList)

            logger.Debug("UpdateStore exit")
        End Sub

        Public Function QueueNewStoreItemMammothEvents(ByVal store As StoreBO) As DataTable
            logger.Debug("QueueNewStoreItemMammothEvents entry")

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            Dim outputParamValues As New DataTable

            'setup parameters for stored proc
            currentParam = New DBParam
            currentParam.Name = "Store_No"
            currentParam.Value = store.StoreNo
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            ' Execute the stored procedure 
            outputParamValues = factory.GetStoredProcedureDataTable("mammoth.InsertItemLocaleChangeQueueAndPriceChangeQueueByStore", paramList)

            logger.Debug("QueueNewStoreItemMammothEvents exit")
            Return outputParamValues
        End Function
#End Region

    End Class
End Namespace