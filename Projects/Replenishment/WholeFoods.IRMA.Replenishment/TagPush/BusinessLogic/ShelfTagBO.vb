Namespace WholeFoods.IRMA.Replenishment.TagPush.BusinessLogic

    Public Class ShelfTagBO
#Region "Constructors"
        ''' <summary>
        ''' Constructor - initialize the object with the results from the Store and StorePOSConfig tables
        ''' </summary>
        ''' <param name="itemKey"></param>
        ''' <remarks></remarks>
        Public Sub New(ByVal pbdID As Integer, ByVal itemKey As Integer, ByVal tagNum As Integer, ByVal tagID2 As Integer)
            _itemKey = itemKey
            _tagID = tagNum
            _pbdID = pbdID
            _tagID2 = tagID2
        End Sub

#End Region

        ''' <summary>
        ''' ShelfTag Info Object to save shelfTag related Info
        ''' </summary>
        ''' <remarks></remarks>
        Private _pbdID As Integer
        Private _tagID As Integer
        Private _tagID2 As Integer
        Private _itemKey As Integer
        Private _pfacings As Integer



        Public ReadOnly Property ItemKey() As Integer
            Get
                Return _itemKey
            End Get
        End Property
        Public ReadOnly Property TagID() As Integer
            Get
                Return _tagID
            End Get
        End Property
        Public ReadOnly Property TagID2() As Integer
            Get
                Return _tagID2
            End Get
        End Property
        Public ReadOnly Property pbdID() As Integer
            Get
                Return (_pbdID)
            End Get
        End Property
    End Class

End Namespace
