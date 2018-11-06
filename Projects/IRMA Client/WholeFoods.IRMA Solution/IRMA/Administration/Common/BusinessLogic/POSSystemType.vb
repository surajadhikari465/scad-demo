Namespace WholeFoods.IRMA.Administration.Common.BusinessLogic

    Public Class POSSystemType

        Private _posSystemID As Integer
        Private _posSystemType As String

        Public Property POSSystemID() As Integer
            Get
                Return _posSystemID
            End Get
            Set(ByVal value As Integer)
                _posSystemID = value
            End Set
        End Property

        Public Property POSSystemType() As String
            Get
                Return _posSystemType
            End Get
            Set(ByVal value As String)
                _posSystemType = value
            End Set
        End Property

    End Class

End Namespace