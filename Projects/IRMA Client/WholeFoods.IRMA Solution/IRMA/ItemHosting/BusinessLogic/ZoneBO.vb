Namespace WholeFoods.IRMA.ItemHosting.BusinessLogic
    Public Class ZoneBO

        Private _zoneID As Integer
        Private _zoneName As String

#Region "Property Access Methods"

        Public Property ZoneID() As Integer
            Get
                Return _zoneID
            End Get
            Set(ByVal value As Integer)
                _zoneID = value
            End Set
        End Property

        Public Property ZoneName() As String
            Get
                Return _zoneName
            End Get
            Set(ByVal value As String)
                _zoneName = value
            End Set
        End Property

#End Region

    End Class
End Namespace