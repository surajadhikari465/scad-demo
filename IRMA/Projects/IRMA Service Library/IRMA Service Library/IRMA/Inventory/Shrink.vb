Namespace IRMA.Inventory

    <DataContract()> _
    Public Class Shrink

        Private _storeNo As Integer
        Private _itemKey As Integer
        Private _quantity As Decimal
        Private _weight As Decimal
        Private _extendedCost As Decimal
        Private _adjustmentID As Integer
        Private _adjustmentReason As String
        Private _createdByUserID As Integer
        Private _subteamNo As Integer
        Private _orderItemID As Integer
        Private _inventoryAdjustmentCodeAbbreviation As String
        Private _username As String
        Private _shrinkSubTypeId As String

        <DataMember()> _
        Public Property UserName() As String
            Get
                Return _username
            End Get
            Set(ByVal value As String)
                _username = value
            End Set
        End Property

        <DataMember()> _
        Public Property StoreNo() As Integer
            Get
                Return _storeNo
            End Get
            Set(ByVal value As Integer)
                _storeNo = value
            End Set
        End Property

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
        Public Property Quantity() As Decimal
            Get
                Return _quantity
            End Get
            Set(ByVal value As Decimal)
                _quantity = value
            End Set
        End Property

        <DataMember()>
        Public Property ShrinkSubTypeId() As Integer
            Get
                Return _shrinkSubTypeId
            End Get
            Set(ByVal value As Integer)
                _shrinkSubTypeId = value
            End Set
        End Property

        <DataMember()> _
        Public Property Weight() As Decimal
            Get
                Return _weight
            End Get
            Set(ByVal value As Decimal)
                _weight = value
            End Set
        End Property

        'This value is NULL
        <DataMember()> _
        Public Property ExtendedCost() As Decimal
            Get
                Return _extendedCost
            End Get
            Set(ByVal value As Decimal)
                _extendedCost = value
            End Set
        End Property

        'Adjustment ID for the type of Shrink/Waste from the ItemAdjustment table
        <DataMember()> _
        Public Property AdjustmentID() As Integer
            Get
                Return _adjustmentID
            End Get
            Set(ByVal value As Integer)
                _adjustmentID = value
            End Set
        End Property

        'Based on type of Shrink Chosen (Spoilage/Food Bank/Samples etc)
        <DataMember()> _
        Public Property AdjustmentReason() As String
            Get
                Return _adjustmentReason
            End Get
            Set(ByVal value As String)
                _adjustmentReason = value
            End Set
        End Property

        <DataMember()> _
        Public Property CreatedByUserID() As Integer
            Get
                Return _createdByUserID
            End Get
            Set(ByVal value As Integer)
                _createdByUserID = value
            End Set
        End Property

        <DataMember()> _
        Public Property SubteamNo() As Integer
            Get
                Return _subteamNo
            End Get
            Set(ByVal value As Integer)
                _subteamNo = value
            End Set
        End Property

        'This value will be NULL
        <DataMember()> _
        Public Property OrderItemID() As Integer
            Get
                Return _orderItemID
            End Get
            Set(ByVal value As Integer)
                _orderItemID = value
            End Set
        End Property

        'Abbreviation of the code i. e. 'FB' for Food Bank
        <DataMember()> _
        Public Property InventoryAdjustmentCodeAbbreviation() As String
            Get
                Return _inventoryAdjustmentCodeAbbreviation
            End Get
            Set(ByVal value As String)
                _inventoryAdjustmentCodeAbbreviation = value
            End Set
        End Property

    End Class

End Namespace