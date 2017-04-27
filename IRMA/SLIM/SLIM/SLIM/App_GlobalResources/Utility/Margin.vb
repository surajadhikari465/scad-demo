Imports Microsoft.VisualBasic

Public Class Margin

    Public Shared Function GetMargin(ByVal Price As Single, ByVal Cost As Single) As Single
        Dim margin As Single
        margin = CInt((((Price - Cost) / Price) * 100))
        Return margin
    End Function

    Public Shared Function GetPrice(ByVal Cost As Single, ByVal Margin As Single, ByVal Multi As Integer) As Single
        Dim price As Single
        price = ((Cost / (100 - Margin)) * 100)
        Return (price / Multi)
    End Function
End Class
