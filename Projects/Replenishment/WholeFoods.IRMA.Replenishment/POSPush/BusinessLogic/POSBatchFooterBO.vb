Namespace WholeFoods.IRMA.Replenishment.POSPush.BusinessLogic

    Public Class POSBatchFooterBO

        ''' <summary>
        ''' PIRUS specific footer data - contains TOREX formatted Julian date from PriceBatchHeader.StartDate value
        ''' </summary>
        ''' <remarks></remarks>
        Private _pirusStartDate As Integer

        Public Property PIRUS_StartDate() As Integer
            Get
                Return _pirusStartDate
            End Get
            Set(ByVal Value As Integer)
                _pirusStartDate = Value
            End Set
        End Property

    End Class

End Namespace
