Imports WholeFoods.ServiceLibrary.DataAccess
Imports WholeFoods.ServiceLibrary.IRMA.Common

Namespace IRMA
    <DataContract()>
    Public Class Item
        Private _itemKey As Integer
        Private _itemDescription As String
        Private _soldByWeight As Boolean
        Private _itemIdentifier As String
        Private _vendorCost As Decimal
        Private _vendorPack As String
        Private _vendorUnitName As String
        Private _retailUnitName As String

        <DataMember()> _
        Public Property ItemKey() As Integer
            Get
                Return _itemKey
            End Get
            Set(ByVal value As Integer)
                _itemKey = value
            End Set
        End Property

        <DataMember()> _
        Public Property ItemDescription() As String
            Get
                Return _itemDescription
            End Get
            Set(ByVal value As String)
                _itemDescription = value
            End Set
        End Property

        <DataMember()> _
        Public Property SoldByWeight() As Boolean
            Get
                Return _soldByWeight
            End Get
            Set(ByVal value As Boolean)
                _soldByWeight = value
            End Set
        End Property

        <DataMember()> _
        Public Property Identifier() As String
            Get
                Return _itemIdentifier
            End Get
            Set(ByVal value As String)
                _itemIdentifier = value
            End Set
        End Property

        <DataMember()> _
        Public Property vendorCost() As Decimal
            Get
                Return _vendorCost
            End Get
            Set(ByVal value As Decimal)
                _vendorCost = value
            End Set
        End Property

        <DataMember()> _
        Public Property vendorPack() As String
            Get
                Return _vendorPack
            End Get
            Set(ByVal value As String)
                _vendorPack = value
            End Set
        End Property

        <DataMember()> _
        Public Property vendorUnitName() As String
            Get
                Return _vendorUnitName
            End Get
            Set(ByVal value As String)
                _vendorUnitName = value
            End Set
        End Property

        <DataMember()> _
        Public Property retailUnitName() As String
            Get
                Return _retailUnitName
            End Get
            Set(ByVal value As String)
                _retailUnitName = value
            End Set
        End Property

        Public Sub New()

        End Sub

        Public Sub New(ByVal dt As DataTable)
            If dt.Rows.Count > 0 Then
                Me.ItemKey = dt.Rows(0).Item("Item_Key")
                Me.Identifier = dt.Rows(0).Item("Identifier")
                Me.ItemDescription = dt.Rows(0).Item("Item_Description")
                Me.vendorCost = dt.Rows(0).Item("Vendor_Cost")
                Me.vendorPack = dt.Rows(0).Item("VendorPack")
                Me.SoldByWeight = dt.Rows(0).Item("Sold_By_Weight")
                Me.vendorUnitName = dt.Rows(0).Item("Vendor_Unit_Name")
                Me.retailUnitName = dt.Rows(0).Item("Retail_Unit_Name")
            End If
        End Sub

        Public Function GetTransferItem(ByVal iItem_Key As Integer, _
                             ByVal sIdentifier As String, _
                             ByVal iProductType_ID As Integer, _
                             ByVal iVendorStore_No As Integer, _
                             ByVal iTransfer_SubTeam As Integer, _
                             ByVal iSupplySubTeam_No As String) As Item

            Dim factory As New DataFactory(DataFactory.IRMAServiceLibrary)
            Try


                Dim dtResult As New DataTable
                Dim paramList As New ArrayList
                Dim currentParam As DBParam

                currentParam = New DBParam
                currentParam.Name = "Item_Key"
                If iItem_Key = Nothing Then
                    currentParam.Value = DBNull.Value
                Else
                    currentParam.Value = iItem_Key
                End If
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Identifier"
                If sIdentifier = Nothing Then
                    currentParam.Value = DBNull.Value
                Else
                    currentParam.Value = sIdentifier
                End If
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "ProductType_ID"
                If iProductType_ID = Nothing Then
                    currentParam.Value = DBNull.Value
                Else
                    currentParam.Value = iProductType_ID
                End If
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "VendStore_No"
                If iVendorStore_No = Nothing Then
                    currentParam.Value = DBNull.Value
                Else
                    currentParam.Value = iVendorStore_No
                End If
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Transfer_SubTeam"
                If iTransfer_SubTeam = Nothing Then
                    currentParam.Value = DBNull.Value
                Else
                    currentParam.Value = iTransfer_SubTeam
                End If
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "SupplySubTeam_No"
                If iSupplySubTeam_No = Nothing Then
                    currentParam.Value = DBNull.Value
                Else
                    currentParam.Value = iSupplySubTeam_No
                End If
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                dtResult = factory.GetStoredProcedureDataTable("WFMM_GetTransferItem", paramList)

                Dim item As New Item(dtResult)

                Return item
            Catch ex As Exception
                Throw
            Finally
                connectionCleanup(factory)
            End Try

        End Function
    End Class
End Namespace
