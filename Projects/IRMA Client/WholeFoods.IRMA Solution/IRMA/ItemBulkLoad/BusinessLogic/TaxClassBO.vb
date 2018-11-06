Namespace WholeFoods.IRMA.ItemBulkLoad.BusinessLogic

    Public Class TaxClassBO
        Private _taxClassID As Integer
        Private _taxClassDesc As String

#Region "Property Access Methods"

        Public Property TaxClassID() As Integer
            Get
                Return _taxClassID
            End Get
            Set(ByVal value As Integer)
                _taxClassID = value
            End Set
        End Property

        Public Property TaxClassDesc() As String
            Get
                Return _taxClassDesc
            End Get
            Set(ByVal value As String)
                _taxClassDesc = value
            End Set
        End Property

#End Region

    End Class
End Namespace
