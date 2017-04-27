Imports WholeFoods.ServiceLibrary.DataAccess
Imports WholeFoods.ServiceLibrary.IRMA.Common

Namespace IRMA
    <DataContract()>
    Public Class CycleCount
        <DataMember()>
        Public Property ID As Long
        <DataMember()>
        Public Property Closed As DateTime
        <DataMember()>
        Public Property EndScan As DateTime
        <DataMember()>
        Public Property IsEndOfPeriod As Boolean
        <DataMember()>
        Public Property StoreName As String
        <DataMember()>
        Public Property SubTeamName As String
        <DataMember()>
        Public Property InternalCycleHeaders As List(Of InternalCycleCountHeader)
        <DataMember()>
        Public Property ExternalCycleHeader As ExternalCycleCountHeader

        Public Sub New()


        End Sub

        Public Function GetCycleCount(ByVal lStoreNo As Long, ByVal lSubTeamNo As Long) As CycleCount

            Dim factory As New DataFactory(DataFactory.IRMAServiceLibrary)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            Try
                currentParam = New DBParam
                currentParam.Name = "Store_No"
                currentParam.Value = lStoreNo
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "SubTeam_No"
                currentParam.Value = lSubTeamNo
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Status"
                currentParam.Value = "OPEN"
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                Dim dtResult As New DataTable
                Dim cc As New CycleCount

                dtResult = factory.GetStoredProcedureDataTable("GetCycleCountMasterList", paramList)

                If dtResult.Rows.Count > 0 Then
                    Dim length As Integer = dtResult.Rows.Count
                    cc.ID = dtResult.Rows(length - 1).Item("MasterCountID")
                    cc.StoreName = dtResult.Rows(length - 1).Item("Store_Name")
                    cc.SubTeamName = dtResult.Rows(length - 1).Item("SubTeam_Name")
                    cc.EndScan = dtResult.Rows(length - 1).Item("EndScan")
                    cc.Closed = NotNull(dtResult.Rows(length - 1).Item("ClosedDate"), Nothing)
                    cc.IsEndOfPeriod = dtResult.Rows(length - 1).Item("EndofPeriod")

                    If (DateTime.Compare(DateTime.Now, cc.EndScan) > 0) Then
                        cc = Nothing
                        Return cc
                    End If

                    paramList.Clear()
                    currentParam = New DBParam
                    currentParam.Name = "MasterCountID"
                    currentParam.Value = cc.ID
                    currentParam.Type = DBParamType.Int
                    paramList.Add(currentParam)

                    currentParam = New DBParam
                    currentParam.Name = "Name"
                    currentParam.Value = DBNull.Value
                    currentParam.Type = DBParamType.String
                    paramList.Add(currentParam)

                    currentParam = New DBParam
                    currentParam.Name = "StartScan"
                    currentParam.Value = DBNull.Value
                    currentParam.Type = DBParamType.String
                    paramList.Add(currentParam)

                    currentParam = New DBParam
                    currentParam.Name = "Status"
                    currentParam.Value = "OPEN"
                    currentParam.Type = DBParamType.String
                    paramList.Add(currentParam)

                    Dim dt As New DataTable
                    dt = factory.GetStoredProcedureDataTable("GetCycleCountList", paramList)
                    If dt.Rows.Count > 0 Then
                        Dim ich As New List(Of InternalCycleCountHeader)
                        For Each dr As DataRow In dt.Rows
                            If Not CBool(dr("External")) Then
                                ich.Add(New InternalCycleCountHeader(dr("CycleCountID"), NotNull(dr("InvLoc_ID"), 0), dr("StartScan")))
                            Else
                                cc.ExternalCycleHeader = New ExternalCycleCountHeader(dr("CycleCountID"))
                            End If
                        Next
                        Dim sortQuery = From i In ich _
                                        Order By i.ID Descending _
                                        Select New InternalCycleCountHeader With {.ID = i.ID, _
                                                                                .InventoryLocationID = i.InventoryLocationID, _
                                                                                .StartScan = i.StartScan _
                                                                                }
                        cc.InternalCycleHeaders = sortQuery.ToList
                    End If
                Else
                    cc = Nothing
                End If
                Return cc
            Catch ex As Exception
                Throw
            Finally
                connectionCleanup(factory)
            End Try

        End Function

        Public Function CreateCycleCountHeader(ByVal lMasterCountID As Long, _
                                               ByVal dStartScan As DateTime, _
                                               ByVal lInventoryLocationId As Long, _
                                               ByVal bExternal As Boolean) As Object

            Dim factory As New DataFactory(DataFactory.IRMAServiceLibrary)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            Try
                currentParam = New DBParam
                currentParam.Name = "MasterCountID"
                currentParam.Value = lMasterCountID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "InvLocID"
                currentParam.Value = lInventoryLocationId
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "StartScan"
                currentParam.Value = dStartScan
                currentParam.Type = DBParamType.DateTime
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "External"
                currentParam.Value = bExternal
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                Dim dt As New DataTable
                dt = factory.GetStoredProcedureDataTable("InsertCycleCountHeader", paramList)

                If dt.Rows.Count > 0 Then
                    If bExternal Then
                        Me.ExternalCycleHeader = New ExternalCycleCountHeader(dt.Rows(0).Item("Added"))
                        Return Me.ExternalCycleHeader
                    Else
                        Dim ch As New InternalCycleCountHeader(dt.Rows(0).Item("Added"), lInventoryLocationId, dStartScan)
                        Return ch
                    End If
                Else
                    Return Nothing
                End If

            Catch ex As Exception
                Throw
            Finally
                connectionCleanup(factory)
            End Try

        End Function

        Public Sub AddCycleCountItem(ByVal lItemKey As Long, _
                                     ByVal dQuantity As Decimal, _
                                     ByVal dWeight As Decimal, _
                                     ByVal dPackSize As Decimal, _
                                     ByVal bIsCaseCnt As Boolean, _
                                     ByVal lCycleCountID As Long, _
                                     Optional ByVal lInvLocID As Long = Nothing)

            Dim factory As New DataFactory(DataFactory.IRMAServiceLibrary)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            Try
                currentParam = New DBParam
                currentParam.Name = "CycleCountID"
                currentParam.Value = lCycleCountID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Item_Key"
                currentParam.Value = lItemKey
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "InvLocID"
                currentParam.Value = IIf(lInvLocID = Nothing, DBNull.Value, lInvLocID)
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "ScanDateTime"
                currentParam.Value = DBNull.Value
                currentParam.Type = DBParamType.DateTime
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Count"
                currentParam.Value = dQuantity
                currentParam.Type = DBParamType.Decimal
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Weight"
                currentParam.Value = dWeight
                currentParam.Type = DBParamType.Decimal
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "PackSize"
                currentParam.Value = dPackSize
                currentParam.Type = DBParamType.Decimal
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "IsCaseCnt"
                currentParam.Value = bIsCaseCnt
                currentParam.Type = DBParamType.Bit
                paramList.Add(currentParam)

                factory.ExecuteStoredProcedure("InsertCycleCountItem2", paramList)

            Catch ex As Exception
                Throw
            Finally
                connectionCleanup(factory)
            End Try

        End Sub

        Public Function GetInventoryLocations(ByVal lStoreNo As Long, ByVal lSubTeamNo As Long) As List(Of InventoryLocation)
            Dim factory As New DataFactory(DataFactory.IRMAServiceLibrary)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            Try
                currentParam = New DBParam
                currentParam.Name = "StoreID"
                currentParam.Value = lStoreNo
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "SubTeamID"
                currentParam.Value = lSubTeamNo
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                Dim ilResult As New List(Of InventoryLocation)
                Dim dt As New DataTable

                dt = factory.GetStoredProcedureDataTable("GetInventoryLocations", paramList)
                For Each dr As DataRow In dt.Rows
                    Dim il As New InventoryLocation
                    il.InvLocID = dr("InvLoc_ID")
                    il.SubteamNo = dr("SubTeam_No")
                    il.Manufacturing = dr("Manufacturing")
                    il.StoreNo = dr("Store_No")
                    il.StoreName = dr("Store_Name")
                    il.SubTeamName = dr("SubTeam_Name")
                    il.InvLocName = dr("InvLoc_Name")
                    il.InvLocDesc = dr("InvLoc_Desc")
                    ilResult.Add(il)
                Next
                Return ilResult
            Catch ex As Exception
                Throw
            Finally
                connectionCleanup(factory)
            End Try
        End Function
    End Class
    <DataContract()>
    Public Class InventoryLocation
        <DataMember()>
        Public Property InvLocID As Long
        <DataMember()>
        Public Property SubteamNo As Integer
        <DataMember()>
        Public Property Manufacturing As Integer
        <DataMember()>
        Public Property StoreNo As Integer
        <DataMember()>
        Public Property StoreName As String
        <DataMember()>
        Public Property SubTeamName As String
        <DataMember()>
        Public Property InvLocName As String
        <DataMember()>
        Public Property InvLocDesc As String
    End Class

    <DataContract()>
    Public Class InternalCycleCountHeader
        <DataMember()>
        Public Property ID As Long
        <DataMember()>
        Public Property InventoryLocationID As Long
        <DataMember()>
        Public Property StartScan As DateTime

        Sub New()

        End Sub
        Sub New(ByVal ID As Long, ByVal InventoryLocationID As Long, ByVal StartScan As Date)
            Me.ID = ID
            Me.InventoryLocationID = InventoryLocationID
            Me.StartScan = StartScan
        End Sub


    End Class

    <DataContract()>
    Public Class ExternalCycleCountHeader
        <DataMember()>
        Public Property ID As Long
        Public Sub New(ByVal ID As Long)
            Me.ID = ID
        End Sub
    End Class

    Public Class CycleCountInfo
        Public Quantity As Decimal
        Public Weight As Decimal
    End Class
End Namespace