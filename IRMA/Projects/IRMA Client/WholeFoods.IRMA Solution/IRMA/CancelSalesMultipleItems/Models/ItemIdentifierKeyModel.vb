Public Class ItemIdentifierKeyModel

    Public Sub New()
    End Sub
    Public Sub New(ByVal identifier As String, ByVal itemkey As String)
        Me.Identifier = identifier
        Me.ItemKey = itemkey
    End Sub

    Public Property Identifier As String
    Public Property ItemKey As String

End Class