Namespace IRMA

    <DataContract()> _
    Public Class GetItem

        Private _itemKey As Integer
        Private _itemDescription As String
        Private _itemSubteamNo As Integer
        Private _itemSubteamName As String
        Private _packageDesc1 As Integer
        Private _packageDesc2 As Decimal
        Private _packageUnitAbbr As String
        Private _soldByWeight As Boolean
        Private _CostedByWeight As Boolean
        Private _packageUnitID As Integer

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
        Public Property ItemSubteamNo() As Integer
            Get
                Return _itemSubteamNo
            End Get
            Set(ByVal value As Integer)
                _itemSubteamNo = value
            End Set
        End Property

        <DataMember()> _
        Public Property ItemSubteamName() As String
            Get
                Return _itemSubteamName
            End Get
            Set(ByVal value As String)
                _itemSubteamName = value
            End Set
        End Property

        <DataMember()> _
        Public Property PackageDesc1() As Integer
            Get
                Return _packageDesc1
            End Get
            Set(ByVal value As Integer)
                _packageDesc1 = value
            End Set
        End Property

        <DataMember()> _
        Public Property PackageDesc2() As Decimal
            Get
                Return _packageDesc2
            End Get
            Set(ByVal value As Decimal)
                _packageDesc2 = value
            End Set
        End Property

        <DataMember()> _
        Public Property PackageUnitAbbr() As String
            Get
                Return _packageUnitAbbr
            End Get
            Set(ByVal value As String)
                _packageUnitAbbr = value
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

        <DataMember()>
        Public Property CostedByWeight As Boolean
            Get
                Return _CostedByWeight
            End Get
            Set(ByVal value As Boolean)
                _CostedByWeight = value
            End Set
        End Property

        <DataMember()>
        Public Property RetailUnit As String

        'ItemKey*
        'Item_Description*
        'POS_Description
        'Item.Subteam_No*
        'Package_Desc1*
        'Package_Desc2*
        'Package_Unit_Abbr*
        'Not_Available
        'Sold_By_Weight*
        'Retail_Sale
        'Vendor_Unit_ID
        'Vendor_Unit_Name

        <DataMember()> _
        Public Property PackageUnitID() As Integer
            Get
                Return _packageUnitID
            End Get
            Set(ByVal value As Integer)
                _packageUnitID = value
            End Set
        End Property

    End Class

End Namespace