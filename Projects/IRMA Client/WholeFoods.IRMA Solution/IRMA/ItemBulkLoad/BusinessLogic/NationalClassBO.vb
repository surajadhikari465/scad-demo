Namespace WholeFoods.IRMA.ItemBulkLoad.BusinessLogic
    Public Class NationalClassBO
        Private _classID As Integer
        Private _className As String

#Region "Property Access Methods"

        Public Property ClassID() As Integer
            Get
                Return _classID
            End Get
            Set(ByVal value As Integer)
                _classID = value
            End Set
        End Property

        Public Property ClassName() As String
            Get
                Return _className
            End Get
            Set(ByVal value As String)
                _className = value
            End Set
        End Property

#End Region

    End Class
End Namespace

