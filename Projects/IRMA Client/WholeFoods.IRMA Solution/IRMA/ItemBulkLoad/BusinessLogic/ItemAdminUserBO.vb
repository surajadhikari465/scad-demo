Namespace WholeFoods.IRMA.ItemBulkLoad.BusinessLogic
    Public Class ItemAdminUserBO

        Private _userID As Integer
        Private _userName As String

        Property UserID() As Integer
            Get
                Return _userID
            End Get
            Set(ByVal value As Integer)
                _userID = value
            End Set
        End Property

        Property UserName() As String
            Get
                Return _userName
            End Get
            Set(ByVal value As String)
                _userName = value
            End Set
        End Property

    End Class

End Namespace
