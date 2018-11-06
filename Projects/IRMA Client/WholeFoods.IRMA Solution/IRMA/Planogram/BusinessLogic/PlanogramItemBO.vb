Namespace WholeFoods.IRMA.Planogram.BusinessLogic
    Public Class PlanogramItemBO
        Private _ItemKey As Integer = 1

        Private _Identifier As String
        Private _StoreNo As Integer
        Private _Group As String
        Private _Shelf As String
        Private _Placement As String
        Private _UPCNumber As String
        Private _MaxUnits As String
        Private _Facings As Integer
        Private _PlanNumber As String

#Region "Properties"
        Public Property ItemKey() As Integer
            Get
                Return _ItemKey
            End Get
            Set(ByVal value As Integer)
                _ItemKey = value
            End Set
        End Property
        Public Property Identifier() As String
            Get
                Return _Identifier
            End Get
            Set(ByVal value As String)
                _Identifier = value
            End Set
        End Property
        Public Property StoreNo() As Integer
            Get
                Return _StoreNo
            End Get
            Set(ByVal value As Integer)
                _StoreNo = value
            End Set
        End Property
        Public Property Group() As String
            Get
                Return _Group
            End Get
            Set(ByVal value As String)
                _Group = value
            End Set
        End Property

        Public Property Shelf() As String
            Get
                Return _Shelf
            End Get
            Set(ByVal value As String)
                _Shelf = value
            End Set
        End Property

        Public Property Placement() As String
            Get
                Return _Placement
            End Get
            Set(ByVal value As String)
                _Placement = value
            End Set
        End Property

        Public Property UPCNumber() As String
            Get
                Return _UPCNumber
            End Get
            Set(ByVal value As String)
                _UPCNumber = value
            End Set
        End Property

        Public Property MaxUnits() As String
            Get
                Return _MaxUnits
            End Get
            Set(ByVal value As String)
                _MaxUnits = value
            End Set
        End Property

        Public Property Facings() As Integer
            Get
                Return _Facings
            End Get
            Set(ByVal value As Integer)
                _Facings = value
            End Set
        End Property

        Public Property PlanNumber() As String
            Get
                Return _PlanNumber
            End Get
            Set(ByVal value As String)
                _PlanNumber = value
            End Set
        End Property

        Public ReadOnly Property ToPlanogramFileString(ByVal StoreNo As Integer) As String
            Get
                Return vbTab & StoreNo & vbTab & _
                Me.ItemKey & vbTab & _
                Me.Group & vbTab & _
                Me.Shelf & vbTab & _
                Me.Placement & vbTab & _
                Me.MaxUnits & vbTab & _
                Me.Facings & vbTab & _
                Me.PlanNumber & vbTab & vbTab

            End Get
        End Property
        Public ReadOnly Property ToMissingPlanogramFileString(ByVal StoreNo As Integer) As String
            Get
                Return vbTab & StoreNo & vbTab & _
                Me.Identifier & vbTab & _
                Me.Group & vbTab & _
                Me.Shelf & vbTab & _
                Me.Placement & vbTab & _
                Me.MaxUnits & vbTab & _
                Me.Facings & vbTab & _
                Me.PlanNumber & vbTab & vbTab

            End Get
        End Property
#End Region


    End Class

End Namespace