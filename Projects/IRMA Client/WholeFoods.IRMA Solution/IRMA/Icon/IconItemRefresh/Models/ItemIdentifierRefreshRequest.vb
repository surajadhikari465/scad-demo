Public Class ItemIdentifierRefreshRequest
    Property ItemRefreshModels As List(Of ItemRefreshModel)
    Property ItemRefreshType As String

    Sub New(ByVal itemRefreshModels As List(Of ItemRefreshModel), ByVal itemRefreshType As String)
        Me.ItemRefreshModels = itemRefreshModels
        Me.ItemRefreshType = itemRefreshType
    End Sub
End Class
