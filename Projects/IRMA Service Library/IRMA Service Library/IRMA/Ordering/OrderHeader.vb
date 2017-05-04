Namespace IRMA.Ordering
    <DataContract()> _
    Public Class OrderHeader
        Private _fromVendorId As Integer
        Private _orderTypeId As Byte
        Private _productTypeId As Byte
        Private _toVendorId As Integer 'This variable is used by orderheader's PurchaseLocation_ID and ReceiveLocation_ID. The two should alwasys be the same.
        Private _fromSubTeam As Integer
        Private _toSubTeam As Integer
        Private _expectedDate As Date
        Private _createdBy As Integer
 
        <DataMember()> _
        Public Property VendorId() As Integer
            Get
                Return _fromVendorId
            End Get
            Set(ByVal value As Integer)
                _fromVendorId = value
            End Set
        End Property

        <DataMember()> _
        Public Property ProductTypeId() As Byte
            Get
                Return _productTypeId
            End Get
            Set(ByVal value As Byte)
                _productTypeId = value
            End Set
        End Property

        <DataMember()> _
        Public Property OrderTypeId() As Byte
            Get
                Return _orderTypeId
            End Get
            Set(ByVal value As Byte)
                _orderTypeId = value
            End Set
        End Property

        <DataMember()> _
        Public Property PurchaseLocationId() As Integer
            Get
                Return _toVendorId
            End Get
            Set(ByVal value As Integer)
                _toVendorId = value
            End Set
        End Property

        <DataMember()> _
        Public Property TransferSubTeam() As Integer
            Get
                Return _fromSubTeam
            End Get
            Set(ByVal value As Integer)
                _fromSubTeam = value
            End Set
        End Property

        <DataMember()> _
        Public Property TransferToSubTeam() As Integer
            Get
                Return _toSubTeam
            End Get
            Set(ByVal value As Integer)
                _toSubTeam = value
            End Set
        End Property

        <DataMember()> _
        Public Property ExpectedDate() As Date
            Get
                Return _expectedDate
            End Get
            Set(ByVal value As Date)
                _expectedDate = value
            End Set
        End Property

        <DataMember()> _
        Public Property CreatedBy() As Integer
            Get
                Return _createdBy
            End Get
            Set(ByVal value As Integer)
                _createdBy = value
            End Set
        End Property
    End Class
End Namespace
