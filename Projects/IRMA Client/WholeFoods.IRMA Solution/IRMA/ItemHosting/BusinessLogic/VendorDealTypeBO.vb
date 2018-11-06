Namespace WholeFoods.IRMA.ItemHosting.BusinessLogic

    Public Class VendorDealTypeBO

        Private _vendorDealTypeID As Integer
        Private _code As String
        Private _description As String
        Private _caseAmtType As String

        Public Property VendorDealTypeID() As Integer
            Get
                Return _vendorDealTypeID
            End Get
            Set(ByVal value As Integer)
                _vendorDealTypeID = value
            End Set
        End Property

        Public Property VendorDealTypeCode() As String
            Get
                Return _code
            End Get
            Set(ByVal value As String)
                _code = value
            End Set
        End Property

        Public Property VendorDealTypeDesc() As String
            Get
                Return _description
            End Get
            Set(ByVal value As String)
                _description = value
            End Set
        End Property

        Public Property CaseAmtType() As String
            Get
                Return _caseAmtType
            End Get
            Set(ByVal value As String)
                _caseAmtType = value
            End Set
        End Property

    End Class

End Namespace