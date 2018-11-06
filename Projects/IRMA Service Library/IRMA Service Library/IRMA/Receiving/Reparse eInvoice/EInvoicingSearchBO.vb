Namespace WholeFoods.IRMA.Replenishment.EInvoicing.BusinessLogic

    Public Class EinvoicingSearchBO

        Private _InvoiceStartDate As DateTime?
        Private _ImportStartDate As DateTime?
        Private _ImportEndDate As DateTime?
        Private _ErrorCodeId As Integer?
        Private _PONumber As String
        Private _InvoiceNumber As String
        Private _PSVendorID As String
        Private _BusinessUnit As String
        Private _Status As String
        Private _Archived As Integer?

        Public Property InvoiceStartDate() As DateTime?
            Get
                Return _InvoiceStartDate
            End Get
            Set(ByVal value As DateTime?)
                _InvoiceStartDate = value
            End Set
        End Property


        Private _InvoiceEndDate As DateTime?
        Public Property InvoiceEndDate() As DateTime?
            Get
                Return _InvoiceEndDate
            End Get
            Set(ByVal value As DateTime?)
                _InvoiceEndDate = value
            End Set
        End Property

        Public Property ImportStartDate As DateTime?
            Get
                Return _ImportStartDate

            End Get
            Set(ByVal value As DateTime?)
                _ImportStartDate = value
            End Set
        End Property

        Public Property ImportEndDate As DateTime?
            Get
                Return _ImportEndDate
            End Get
            Set(ByVal value As DateTime?)
                _ImportEndDate = value
            End Set
        End Property


        Public Property InvoiceNumber As String
            Get
                Return _InvoiceNumber
            End Get
            Set(ByVal value As String)
                _InvoiceNumber = value
            End Set
        End Property

        Public Property BusinessUnit As String
            Get
                Return _BusinessUnit
            End Get
            Set(ByVal value As String)
                _BusinessUnit = value
            End Set
        End Property

        Public Property PSVendorId As String
            Get
                Return _PSVendorID
            End Get
            Set(ByVal value As String)
                _PSVendorID = value
            End Set
        End Property

        Public Property PONumber As String
            Get
                Return _PONumber
            End Get
            Set(ByVal value As String)
                _PONumber = value
            End Set
        End Property

        Public Property ErrorCodeId As Integer?
            Get
                Return _ErrorCodeId

            End Get
            Set(ByVal value As Integer?)
                _ErrorCodeId = value
            End Set
        End Property

        Public Property Status As String
            Get
                Return _Status
            End Get
            Set(ByVal value As String)
                _Status = value
            End Set
        End Property

        Public Property Archived As Integer?
            Get
                Return _Archived

            End Get
            Set(ByVal value As Integer?)
                _Archived = value
            End Set
        End Property

        Sub New()

        End Sub

    End Class
End Namespace