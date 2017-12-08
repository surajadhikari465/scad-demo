Public Class StoreItemInfoModel

    Public Sub New()
    End Sub

    Public Sub New(ByVal store As String, ByVal itemKey As String,
                   ByVal errorDetails As String, ByVal identifier As String,
                   ByVal storeName As String)
        Me.Store = store
        Me.Identifier = identifier
        Me.ErrorDetails = errorDetails
        Me.ItemKey = itemKey
        Me.StoreName = storeName
    End Sub

    Public Property Store As String
    Public Property ItemKey As String
    Public Property ErrorDetails As String
    Public Property Identifier As String
    Public Property StoreName As String

End Class