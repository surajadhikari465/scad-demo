Imports WholeFoods.IRMA.EPromotions.BusinessLogic.Common

Namespace WholeFoods.IRMA.EPromotions.BusinessLogic
    Public Class ItemGroupMemberBO
        Inherits BO_IRMABase


        Private _GroupId As Integer
        Private _ItemKey As Integer
        Private _ItemIdentifier As String
        Private _ItemDesc As String
        Private _ModifiedDate As DateTime = Nothing
        Private _UserId As Integer
        Private _Status As OfferChangeType


        Public Property GroupId() As Integer
            Get
                Return _GroupId
            End Get
            Set(ByVal value As Integer)
                _GroupId = value
            End Set
        End Property
        Public Property ItemKey() As Integer
            Get
                Return _ItemKey
            End Get
            Set(ByVal value As Integer)
                _ItemKey = value
            End Set
        End Property
        Public Property ItemIdentifier() As String
            Get
                Return _ItemIdentifier
            End Get
            Set(ByVal value As String)
                _ItemIdentifier = value
            End Set
        End Property
        Public Property ModifiedDate() As DateTime
            Get
                Return _ModifiedDate
            End Get
            Set(ByVal value As DateTime)
                _ModifiedDate = value
            End Set
        End Property

        Public Property Userid() As Integer
            Get
                Return _UserId
            End Get
            Set(ByVal value As Integer)
                _UserId = value
            End Set
        End Property

        Public Property ItemDesc() As String
            Get
                Return _ItemDesc
            End Get
            Set(ByVal value As String)
                _ItemDesc = value
            End Set
        End Property
        Public Property Status() As OfferChangeType
            Get
                Return _Status
            End Get
            Set(ByVal value As OfferChangeType)
                _Status = value
            End Set
        End Property



        Sub New()

        End Sub

        Sub New(ByVal GroupId As Integer, ByVal ItemKey As Integer, ByVal ModifiedDate As DateTime, ByVal UserId As Integer, ByVal ItemDesc As String)
            _GroupId = GroupId
            _ItemKey = ItemKey
            _ModifiedDate = ModifiedDate
            _UserId = UserId
            _ItemDesc = ItemDesc
        End Sub
    End Class
End Namespace
