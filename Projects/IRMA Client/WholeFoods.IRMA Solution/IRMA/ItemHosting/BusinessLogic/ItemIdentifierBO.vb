Namespace WholeFoods.IRMA.ItemHosting.BusinessLogic
    Public Module ItemIdentifierBO
        Public Function IsPosPluIdentifier(text As String) As Boolean
            If Not String.IsNullOrWhiteSpace(text) AndAlso text.Length < 7 Then
                Return True
            Else
                Return False
            End If
        End Function
    End Module
End Namespace