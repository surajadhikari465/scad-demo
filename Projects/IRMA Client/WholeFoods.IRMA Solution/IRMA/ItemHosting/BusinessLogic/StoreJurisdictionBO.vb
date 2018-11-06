Namespace WholeFoods.IRMA.ItemHosting.BusinessLogic
    Public Class StoreJurisdictionBO
#Region "Property Definitions"
        Private _storeJurisdictionId As Integer
        Private _storeJurisdictionDesc As String
#End Region

#Region "Property Access Methods"
        Public Property StoreJurisdictionId() As Integer
            Get
                Return _storeJurisdictionId
            End Get
            Set(ByVal value As Integer)
                _storeJurisdictionId = value
            End Set
        End Property

        Public Property StoreJurisdictionDesc() As String
            Get
                Return _storeJurisdictionDesc
            End Get
            Set(ByVal value As String)
                _storeJurisdictionDesc = value
            End Set
        End Property
#End Region

#Region "Business Rules"
#End Region

    End Class
End Namespace

