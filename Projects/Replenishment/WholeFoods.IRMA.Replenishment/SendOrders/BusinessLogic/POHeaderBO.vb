Namespace WholeFoods.IRMA.Replenishment.SendOrders.BusinessLogic

    Public Class POHeaderBO : Inherits ItemCatalog.Order

        ''' <summary>
        ''' POHeader
        ''' </summary>
        ''' <remarks></remarks>
        Private _business_unit As Integer
        Private _fax_order As Boolean
        Private _email_order As Boolean
        Private _printer As String
        Private _peopleSoftNumber As String
        Private _electronic_transfer As Boolean
        Private _FTP_Addr As String
        Private _FTP_Path As String
        Private _FTP_User As String
        Private _FTP_Password As String
        Private _sender_email As String
        Private _cover_page As String
        Private _full_name As String
        Private _qtyDiscount As Double
        Private _discountType As Integer
        Private _user_phone As String
        Private _user_fax_number As String
        Private _user_title As String
        Private _podescription As String
        Private _vendor_fax As String
        Private _rec_vendor_fax As String
        Private _pur_vendor_fax As String
        Private _unfi_store As String
        Private _podetailsList As ArrayList
        Private _fileType As String 'added for check in again
        Public vendor As ItemCatalog.Vendor = New ItemCatalog.Vendor
        Public receiving_vendor As ItemCatalog.Vendor = New ItemCatalog.Vendor
        Public purchase_vendor As ItemCatalog.Vendor = New ItemCatalog.Vendor

        Private _vendorEmail As String
        Private _overrideRransmissionMethod As Boolean
        Private _overrideTransmissionTarget As String

        Private _storeNo As Integer

        Public Property BusinessUnit() As Integer
            Get
                Return _business_unit
            End Get
            Set(ByVal Value As Integer)
                _business_unit = Value
            End Set
        End Property

        Public Property Fax_Order() As Boolean
            Get
                Return _fax_order
            End Get
            Set(ByVal Value As Boolean)
                _fax_order = Value
            End Set
        End Property


        Public Property Email_Order() As Boolean
            Get
                Return _email_order
            End Get
            Set(ByVal Value As Boolean)
                _email_order = Value
            End Set
        End Property


        Public Property Printer() As String
            Get
                Return _printer
            End Get
            Set(ByVal Value As String)
                _printer = Value
            End Set
        End Property

        Public Property PeopleSoftNumber() As String
            Get
                Return _peopleSoftNumber
            End Get
            Set(ByVal Value As String)
                _peopleSoftNumber = Value
            End Set
        End Property
        Public Property Electronic_Transfer() As Boolean
            Get
                Return _electronic_transfer
            End Get
            Set(ByVal Value As Boolean)
                _electronic_transfer = Value
            End Set
        End Property

        Public Property FTP_Addr() As String
            Get
                Return _FTP_Addr
            End Get
            Set(ByVal Value As String)
                _FTP_Addr = Value
            End Set
        End Property

        Public Property FTP_Path() As String
            Get
                Return _FTP_Path
            End Get
            Set(ByVal Value As String)
                _FTP_Path = Value
            End Set
        End Property

        Public Property FTP_User() As String
            Get
                Return _FTP_User
            End Get
            Set(ByVal Value As String)
                _FTP_User = Value
            End Set
        End Property

        Public Property FTP_Password() As String
            Get
                Return _FTP_Password
            End Get
            Set(ByVal Value As String)
                _FTP_Password = Value
            End Set
        End Property
        Public Property Sender_Email() As String
            Get
                Return _sender_email
            End Get
            Set(ByVal Value As String)
                _sender_email = Value
            End Set
        End Property

        Public Property Cover_Page() As String
            Get
                Return _cover_page
            End Get
            Set(ByVal Value As String)
                _cover_page = Value
            End Set
        End Property

        Public Property Full_Name() As String
            Get
                Return _full_name
            End Get
            Set(ByVal Value As String)
                _full_name = Value
            End Set
        End Property

        Public Property QtyDiscount() As Double
            Get
                Return _qtyDiscount
            End Get
            Set(ByVal Value As Double)
                _qtyDiscount = Value
            End Set
        End Property
        Public Property DiscountType() As Integer
            Get
                Return _discountType
            End Get
            Set(ByVal Value As Integer)
                _discountType = Value
            End Set
        End Property
        Public Property UserPhone() As String
            Get
                Return _user_phone
            End Get
            Set(ByVal Value As String)
                _user_phone = Value
            End Set
        End Property
        Public Property UserFaxNumber() As String
            Get
                Return _user_fax_number
            End Get
            Set(ByVal Value As String)
                _user_fax_number = Value
            End Set
        End Property
        Public Property UserTitle() As String
            Get
                Return _user_title
            End Get
            Set(ByVal Value As String)
                _user_title = Value
            End Set
        End Property
        Public Property PODescription() As String
            Get
                Return _podescription
            End Get
            Set(ByVal Value As String)
                _podescription = Value
            End Set
        End Property
        Public Property Vendor_Fax() As String
            Get
                Return _vendor_fax
            End Get
            Set(ByVal Value As String)
                _vendor_fax = Value
            End Set
        End Property
        Public Property Receiving_Vendor_Fax() As String
            Get
                Return _rec_vendor_fax
            End Get
            Set(ByVal Value As String)
                _rec_vendor_fax = Value
            End Set
        End Property
        Public Property Purchase_Vendor_Fax() As String
            Get
                Return _pur_vendor_fax
            End Get
            Set(ByVal Value As String)
                _pur_vendor_fax = Value
            End Set
        End Property
        Public Property PODetailsList() As ArrayList
            Get
                Return _podetailsList
            End Get
            Set(ByVal Value As ArrayList)
                _podetailsList = Value
            End Set
        End Property
        Public Property UNFIStore() As String
            Get
                Return _unfi_store
            End Get
            Set(ByVal Value As String)
                _unfi_store = Value
            End Set
        End Property
        Public Property FileType() As String
            Get
                Return _fileType
            End Get
            Set(ByVal Value As String)
                _fileType = Value
            End Set
        End Property

        Public Property OverrideTransmissionMethod() As Boolean
            Get
                Return _overrideRransmissionMethod
            End Get
            Set(ByVal value As Boolean)
                _overrideRransmissionMethod = value
            End Set
        End Property

        Public Property OverrideTransmissionTarget() As String
            Get
                Return _overrideTransmissionTarget
            End Get
            Set(ByVal value As String)
                _overrideTransmissionTarget = value
            End Set
        End Property

        Public Property Vendor_Email() As String
            Get
                Return _vendorEmail
            End Get
            Set(ByVal value As String)
                _vendorEmail = value
            End Set
        End Property

        Public Property StoreNo() As Integer
            Get
                Return _storeNo
            End Get
            Set(ByVal value As Integer)
                _storeNo = value
            End Set
        End Property

    End Class


End Namespace