Namespace WholeFoods.IRMA.ItemHosting.BusinessLogic

    Public Class ItemMarginBO

        Private _storeNo As Integer
        Private _storeName As String
        Private _companyName As String 'vendor name
        Private _currentPrice As String
        Private _packageDesc1 As Decimal
        Private _RegularUnitCost As Decimal
        Private _NetUnitCost As Decimal
        Private _RegularMarginAvgCost As Decimal
        Private _RegularMarginCurrentCost As Decimal
        Private _CurrentMarginAvgCost As Decimal
        Private _CurrenMarginCurrentCost As Decimal
        Private _AvgCost As Decimal



        Public Property StoreNo() As Integer
            Get
                Return _storeNo
            End Get
            Set(ByVal value As Integer)
                _storeNo = value
            End Set
        End Property
        Public Property StoreName() As String
            Get
                Return _storeName
            End Get
            Set(ByVal value As String)
                _storeName = value
            End Set
        End Property
        Public Property CompanyName() As String
            Get
                Return _companyName
            End Get
            Set(ByVal value As String)
                _companyName = value
            End Set
        End Property
        Public Property CurrentPrice() As String
            Get
                Return _currentPrice
            End Get
            Set(ByVal value As String)
                _currentPrice = value
            End Set
        End Property
        Public Property PackageDesc1() As Decimal
            Get
                Return _packageDesc1
            End Get
            Set(ByVal value As Decimal)
                _packageDesc1 = value
            End Set
        End Property
        Public Property RegularUnitCost() As Decimal
            Get
                Return _RegularUnitCost
            End Get
            Set(ByVal value As Decimal)
                _RegularUnitCost = value
            End Set
        End Property
        Public Property NetUnitCost() As Decimal
            Get
                Return _NetUnitCost
            End Get
            Set(ByVal value As Decimal)
                _NetUnitCost = value
            End Set
        End Property
        Public Property RegularMarginCurrentCost() As Decimal
            Get
                Return _RegularMarginCurrentCost
            End Get
            Set(ByVal value As Decimal)
                _RegularMarginCurrentCost = value
            End Set
        End Property
        Public Property RegularMarginAvgCost() As Decimal
            Get
                Return _RegularMarginAvgCost
            End Get
            Set(ByVal value As Decimal)
                _RegularMarginAvgCost = value
            End Set
        End Property
        Public Property CurrentMarginAvgCost() As Decimal
            Get
                Return _CurrentMarginAvgCost
            End Get
            Set(ByVal value As Decimal)
                _CurrentMarginAvgCost = value
            End Set
        End Property
        Public Property CurrenMarginCurrentCost() As Decimal
            Get
                Return _CurrenMarginCurrentCost
            End Get
            Set(ByVal value As Decimal)
                _CurrenMarginCurrentCost = value
            End Set
        End Property
        Public Property AvgCost() As Decimal
            Get
                Return _AvgCost
            End Get
            Set(ByVal value As Decimal)
                _AvgCost = value
            End Set
        End Property

    End Class

End Namespace