Public Class SubTeamBO

    Private _SubTeamNo As Integer
    Public Property SubTeamNo() As Integer
        Get
            Return _SubTeamNo
        End Get
        Set(ByVal value As Integer)
            _SubTeamNo = value
        End Set
    End Property

    Private _SubTeamName As String
    Public Property SubTeamName() As String
        Get
            Return _SubTeamName
        End Get
        Set(ByVal value As String)
            _SubTeamName = value
        End Set
    End Property

    Private _Team_No As Integer
    Public Property TeamNo() As Integer
        Get
            Return _Team_No
        End Get
        Set(ByVal value As Integer)
            _Team_No = value
        End Set
    End Property

    Private _SubTeamAbbr As String
    Public Property SubTeamAbbreviation() As String
        Get
            Return _SubTeamAbbr
        End Get
        Set(ByVal value As String)
            _SubTeamAbbr = value
        End Set
    End Property

    Private _DeptNo As Integer
    Public Property DepartmentNo() As Integer
        Get
            Return _DeptNo
        End Get
        Set(ByVal value As Integer)
            _DeptNo = value
        End Set
    End Property

    Private _SubDeptNo As Integer
    Public Property SubDepartmentNo() As Integer
        Get
            Return _SubDeptNo
        End Get
        Set(ByVal value As Integer)
            _SubDeptNo = value
        End Set
    End Property

    Private _BuyerUserId As Integer
    Public Property BuyerUserId() As Integer
        Get
            Return _BuyerUserId
        End Get
        Set(ByVal value As Integer)
            _BuyerUserId = value
        End Set
    End Property

    Private _TargetMargin As Single
    Public Property TargetMargin() As Single
        Get
            Return _TargetMargin
        End Get
        Set(ByVal value As Single)
            _TargetMargin = value
        End Set
    End Property

    Private _JDA As Integer
    Public Property JDA() As Integer
        Get
            Return _JDA
        End Get
        Set(ByVal value As Integer)
            _JDA = value
        End Set
    End Property

    Private _GLPurchaseAcct As Integer
    Public Property GLPurchaseAcct() As Integer
        Get
            Return _GLPurchaseAcct
        End Get
        Set(ByVal value As Integer)
            _GLPurchaseAcct = value
        End Set
    End Property

    Private _GLDistributionAcct As Integer
    Public Property GLDistributionAcct() As Integer
        Get
            Return _GLDistributionAcct
        End Get
        Set(ByVal value As Integer)
            _GLDistributionAcct = value
        End Set
    End Property

    Private _GLTransferAcct As Integer
    Public Property GLTransferAcct() As Integer
        Get
            Return _GLTransferAcct
        End Get
        Set(ByVal value As Integer)
            _GLTransferAcct = value
        End Set
    End Property

    Private _GLSalesAcct As Integer
    Public Property GLSalesAcct() As Integer
        Get
            Return _GLSalesAcct
        End Get
        Set(ByVal value As Integer)
            _GLSalesAcct = value
        End Set
    End Property

    Private _GLPackagingAcct As Integer
    Public Property GLPackagingAcct() As Integer
        Get
            Return _GLPackagingAcct
        End Get
        Set(ByVal value As Integer)
            _GLPackagingAcct = value
        End Set
    End Property

    Private _GLSuppliesAcct As Integer
    Public Property GLSuppliesAcct() As Integer
        Get
            Return _GLSuppliesAcct
        End Get
        Set(ByVal value As Integer)
            _GLSuppliesAcct = value
        End Set
    End Property

    Private _TransferToMarkup As Single
    Public Property TransferToMarkup() As Single
        Get
            Return _TransferToMarkup
        End Get
        Set(ByVal value As Single)
            _TransferToMarkup = value
        End Set
    End Property

    Private _EXEWarehouseSent As Boolean
    Public Property EXEWarehouseSent() As Boolean
        Get
            Return _EXEWarehouseSent
        End Get
        Set(ByVal value As Boolean)
            _EXEWarehouseSent = value
        End Set
    End Property

    Private _Retail As Boolean
    Public Property Retail() As Boolean
        Get
            Return _Retail
        End Get
        Set(ByVal value As Boolean)
            _Retail = value
        End Set
    End Property

    Private _InventoryCountByCase As Boolean
    Public Property InventoryCountByCase() As Boolean
        Get
            Return _InventoryCountByCase
        End Get
        Set(ByVal value As Boolean)
            _InventoryCountByCase = value
        End Set
    End Property

    Private _EXEDistributed As Boolean
    Public Property EXEDistributed() As Boolean
        Get
            Return _EXEDistributed
        End Get
        Set(ByVal value As Boolean)
            _EXEDistributed = value
        End Set
    End Property

    Private _ScaleDepartment As Integer
    Public Property ScaleDepartment() As Integer
        Get
            Return _ScaleDepartment
        End Get
        Set(ByVal value As Integer)
            _ScaleDepartment = value
        End Set
    End Property

    Private _SubTeamTypeId As Integer
    Public Property SubTeamTypeId() As Integer
        Get
            Return _SubTeamTypeId
        End Get
        Set(ByVal value As Integer)
            _SubTeamTypeId = value
        End Set
    End Property

    Private _Distribution As Boolean
    Public Property Distribution() As Boolean
        Get
            Return _Distribution
        End Get
        Set(ByVal value As Boolean)
            _Distribution = value
        End Set
    End Property
    Private _FixedSpoilage As Integer
    Public Property FixedSpoilage() As Integer
        Get
            Return _FixedSpoilage
        End Get
        Set(ByVal value As Integer)
            _FixedSpoilage = value
        End Set
    End Property
    Private _Beverage As Integer
    Public Property Beverage() As Integer
        Get
            Return _Beverage
        End Get
        Set(ByVal value As Integer)
            _Beverage = value
        End Set
    End Property

    Private _AlignedSubTeam As Integer
    Public Property AlignedSubTeam() As Integer
        Get
            Return _AlignedSubTeam
        End Get
        Set(ByVal value As Integer)
            _AlignedSubTeam = value
        End Set
    End Property

    Sub New()

    End Sub

    Sub New(ByVal SubTeamNo As Integer, ByVal SubTeamName As String)
        Me._SubTeamNo = SubTeamNo
        Me._SubTeamName = SubTeamName
    End Sub

    Public Function Validate() As Boolean

    End Function

End Class
