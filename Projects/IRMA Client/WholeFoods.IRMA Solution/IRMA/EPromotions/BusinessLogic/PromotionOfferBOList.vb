Imports System.ComponentModel

Namespace WholeFoods.IRMA.EPromotions.BusinessLogic
    Public Class PromotionOfferBOList
        Inherits BindingList(Of PromotionOfferBO)

        Protected Overrides ReadOnly Property SupportsSearchingCore() As Boolean
            Get
                Return True
            End Get
        End Property

        Protected Overrides Function FindCore(ByVal prop As PropertyDescriptor, _
            ByVal key As Object) As Integer

            ' Ignore the prop value and search by PromotionOfferID
            Try
                Dim i As Integer
                While i < Count
                    If Items(i).PromotionOfferID = CInt(key) Then
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
