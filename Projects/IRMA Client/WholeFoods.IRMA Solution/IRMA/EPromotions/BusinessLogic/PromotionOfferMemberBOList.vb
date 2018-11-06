Imports System.ComponentModel

Namespace WholeFoods.IRMA.EPromotions.BusinessLogic
    Public Class PromotionOfferMemberBOList
        Inherits BindingList(Of PromotionOfferMemberBO)

        Protected Overrides ReadOnly Property SupportsSearchingCore() As Boolean
            Get
                Return True
            End Get
        End Property

        Protected Overrides Function FindCore(ByVal prop As PropertyDescriptor, _
            ByVal key As Object) As Integer

            ' Ignore the prop value and search by GroupID. The objects are assumed to be already filtered
            ' by OfferID
            Try
                Dim i As Integer
                While i < Count
                    If Items(i).GroupID = CInt(key) Then
                        Return i
                    End If
                    i += 1
                End While
            Catch
                Return -1
            End Try

        End Function
    End Class
End Namespace
