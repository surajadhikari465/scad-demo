Namespace WholeFoods.IRMA.ItemBulkLoad.BusinessLogic
    Public Class ImportTypeBO

        Private _itemUploadTypeID As Integer
        Private _description As String

        Property ItemUploadTypeID() As Integer
            Get
                Return _itemUploadTypeID
            End Get
            Set(ByVal value As Integer)
                _itemUploadTypeID = value
            End Set
        End Property

        Property Description() As String
            Get
                Return _description
            End Get
            Set(ByVal value As String)
                _description = value
            End Set
        End Property

    End Class

End Namespace
