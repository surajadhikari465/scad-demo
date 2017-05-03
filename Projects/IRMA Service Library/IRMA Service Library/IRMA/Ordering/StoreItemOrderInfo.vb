Imports WholeFoods.ServiceLibrary.DataAccess
Imports WholeFoods.ServiceLibrary.IRMA.Common

Namespace IRMA
    <DataContract()>
    Public Class StoreItemOrderInfo
        <DataMember()>
        Public Property QtyOnOrder As Decimal
        <DataMember()>
        Public Property QtyOnQueue As Decimal
        <DataMember()>
        Public Property QtyOnQueueCredit As Decimal
        <DataMember()>
        Public Property QtyOnQueueTransfer As Decimal
        <DataMember()>
        Public Property PrimaryVendorKey As String
        <DataMember()>
        Public Property PrimaryVendorName As String
        <DataMember()>
        Public Property LastReceivedDate As DateTime
        <DataMember()>
        Public Property LastReceived As Decimal

        Public Sub New()

        End Sub

        Public Sub New(ByVal dt As DataTable)
            If dt.Rows.Count > 0 Then
                Me.QtyOnOrder = dt.Rows(0).Item("QtyOnOrder")
                Me.QtyOnQueue = dt.Rows(0).Item("QtyOnQueue")
                Me.QtyOnQueueCredit = dt.Rows(0).Item("QtyOnQueueCredit")
                Me.QtyOnQueueTransfer = dt.Rows(0).Item("QtyOnQueueTransfer")
                Me.PrimaryVendorKey = dt.Rows(0).Item("PrimaryVendorKey")
                Me.PrimaryVendorName = dt.Rows(0).Item("PrimaryVendorName")
                Me.LastReceivedDate = dt.Rows(0).Item("LastReceivedDate")
                Me.LastReceived = dt.Rows(0).Item("LastReceived")
            End If
        End Sub
        Public Function GetStoreItemOrderInfo(ByVal iStoreNo As Integer, _
                                               ByVal iTransferToSubTeamNo As Integer, _
                                               ByVal iItemKey As Integer) As StoreItemOrderInfo

            Dim factory As New DataFactory(DataFactory.IRMAServiceLibrary)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            Try
                currentParam = New DBParam
                currentParam.Name = "Store_No"
                currentParam.Value = iStoreNo
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "TransferToSubTeam_No"
                currentParam.Value = iTransferToSubTeamNo
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Item_Key"
                currentParam.Value = iItemKey
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                Dim dt As DataTable = factory.GetStoredProcedureDataTable("WFMM_GetStoreItemOrderInfo", paramList)
                Dim sio As New StoreItemOrderInfo(dt)

                Return sio
            Catch ex As Exception
                Throw ex
            Finally
                connectionCleanup(factory)
            End Try
        End Function

    End Class
End Namespace