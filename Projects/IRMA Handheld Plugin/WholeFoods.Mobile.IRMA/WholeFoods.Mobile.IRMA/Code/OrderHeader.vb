Public Class OrderHeader
    Private _fromVendorId As Integer
    Private _productTypeId As Byte
    Private _toVendorId As Integer 'This variable is used by orderheader's PurchaseLocation_ID and ReceiveLocation_ID. The two should alwasys be the same.
    Private _fromSubTeam As Integer
    Private _toSubTeam As Integer
    Private _expectedDate As Date
    Private _createdBy As Integer
    Private _fromVendorName As String
    Private _fromSubTeamName As String
    Private _toVendorName As String
    Private _toSubTeamname As String

#Region " Public Properties"

    Public Property VendorId() As Integer
        Get
            Return _fromVendorId
        End Get
        Set(ByVal value As Integer)
            _fromVendorId = value
        End Set
    End Property

    Public Property ProductTypeId() As Byte
        Get
            Return _productTypeId
        End Get
        Set(ByVal value As Byte)
            _productTypeId = value
        End Set
    End Property

    Public Property PurchaseLocationId() As Integer
        Get
            Return _toVendorId
        End Get
        Set(ByVal value As Integer)
            _toVendorId = value
        End Set
    End Property

    Public Property TransferSubTeam() As Integer
        Get
            Return _fromSubTeam
        End Get
        Set(ByVal value As Integer)
            _fromSubTeam = value
        End Set
    End Property

    Public Property TransferToSubTeam() As Integer
        Get
            Return _toSubTeam
        End Get
        Set(ByVal value As Integer)
            _toSubTeam = value
        End Set
    End Property

    Public Property ExpectedDate() As Date
        Get
            Return _expectedDate
        End Get
        Set(ByVal value As Date)
            _expectedDate = value
        End Set
    End Property

    Public Property CreatedBy() As Integer
        Get
            Return _createdBy
        End Get
        Set(ByVal value As Integer)
            _createdBy = value
        End Set
    End Property

    Public Property FromVendorName() As String
        Get
            Return _fromVendorName
        End Get
        Set(ByVal value As String)
            _fromVendorName = value
        End Set
    End Property

    Public Property FromSubTeamName() As String
        Get
            Return _fromSubTeamName
        End Get
        Set(ByVal value As String)
            _fromSubTeamName = value
        End Set
    End Property

    Public Property ToVendorName() As String
        Get
            Return _toVendorName
        End Get
        Set(ByVal value As String)
            _toVendorName = value
        End Set
    End Property

    Public Property ToSubTeamName() As String
        Get
            Return _toSubTeamname
        End Get
        Set(ByVal value As String)
            _toSubTeamname = value
        End Set
    End Property
#End Region
End Class
