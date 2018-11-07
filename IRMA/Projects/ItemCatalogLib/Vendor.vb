Imports WFM.DataAccess.Setup
Public Class Vendor
    Dim m_vendor_id As Long
    Dim m_key As String
    Dim m_company_name As String
    Dim m_address_line_1 As String
    Dim m_address_line_2 As String
    Dim m_city As String
    Dim m_state As String
    Dim m_zip_code As String
    Dim m_country As String
    Dim m_phone As String
    Dim m_ps_vendor_id As String
    Dim m_isa_qualifier As String
    Dim m_isa_id As String
    Dim m_edi_notification_email As String
    Dim m_potransmissiontypeid As String



    Public Property VendorID() As Long
        Get
            Return m_vendor_id
        End Get
        Set(ByVal Value As Long)
            m_vendor_id = Value
        End Set
    End Property

    Public Property PSVendorID() As String
        Get
            Return m_ps_vendor_id
        End Get
        Set(ByVal Value As String)
            m_ps_vendor_id = Value
        End Set
    End Property

    Public Property Key() As String
        Get
            Return Me.m_key
        End Get
        Set(ByVal Value As String)
            Me.m_key = Value
        End Set
    End Property
    Public Property CompanyName() As String
        Get
            Return m_company_name
        End Get
        Set(ByVal Value As String)
            m_company_name = Value
        End Set
    End Property
    Public Property AddressLine1() As String
        Get
            Return m_address_line_1
        End Get
        Set(ByVal Value As String)
            m_address_line_1 = Value
        End Set
    End Property
    Public Property AddressLine2() As String
        Get
            Return m_address_line_2
        End Get
        Set(ByVal Value As String)
            m_address_line_2 = Value
        End Set
    End Property
    Public Property City() As String
        Get
            Return m_city
        End Get
        Set(ByVal Value As String)
            m_city = Value
        End Set
    End Property
    Public Property State() As String
        Get
            Return m_state
        End Get
        Set(ByVal Value As String)
            m_state = Value
        End Set
    End Property
    Public Property ZipCode() As String
        Get
            Return m_zip_code
        End Get
        Set(ByVal Value As String)
            m_zip_code = Value
        End Set
    End Property
    Public Property Country() As String
        Get
            Return m_country
        End Get
        Set(ByVal Value As String)
            m_country = Value
        End Set
    End Property
    Public Property Phone() As String
        Get
            Return m_phone
        End Get
        Set(ByVal Value As String)
            m_phone = Value
        End Set
    End Property
    Public Property ISAQualifier() As String
        Get
            Return m_isa_qualifier
        End Get
        Set(ByVal Value As String)
            m_isa_qualifier = Value
        End Set
    End Property

    Public Property ISA_ID() As String
        Get
            Return m_isa_id
        End Get
        Set(ByVal Value As String)
            m_isa_id = Value
        End Set
    End Property

    Public Property EDINotificationEmail() As String
        Get
            Return m_edi_notification_email
        End Get
        Set(ByVal Value As String)
            m_edi_notification_email = Value
        End Set
    End Property

    Public Property POTransmissionTypeID() As String
        Get
            Return m_potransmissiontypeid
        End Get
        Set(ByVal Value As String)
            m_potransmissiontypeid = Value
        End Set
    End Property

    Public Sub New()
    End Sub

    Public Sub New(ByVal VendorID As Long)
        Me.m_vendor_id = VendorID
    End Sub

    Public Sub New(ByVal VendorID As Long, ByVal Key As String, ByVal CompanyName As String, ByVal AddressLine1 As String, ByVal AddressLine2 As String, ByVal City As String, ByVal State As String, ByVal ZipCode As String, ByVal Country As String, ByVal Phone As String)
        Me.m_vendor_id = VendorID
        Me.m_key = Key
        Me.m_company_name = CompanyName
        Me.m_address_line_1 = AddressLine1
        Me.m_address_line_2 = AddressLine2
        Me.m_city = City
        Me.m_state = State
        Me.m_zip_code = ZipCode
        Me.m_country = Country
        Me.m_phone = Phone
    End Sub

    Public Shared Function GetP2PVendorsForIntegrators(ByVal IntegratorID As Integer) As ArrayList
        Dim cmd As System.Data.SqlClient.SqlCommand = Nothing
        Dim dr As System.Data.SqlClient.SqlDataReader = Nothing
        Dim myVendor As Vendor

        ' Used in Procurement to Payment (P2P).  Returns a listing of values
        ' associated with a particular vendor.  
        ' Called from the "Order - Integrators - Send 850 EDI.dtsx" script.
        Try
            Dim results As New ArrayList
            cmd = New System.Data.SqlClient.SqlCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "GetP2PVendorsForIntegrator"

            cmd.Parameters.Clear()
            cmd.Parameters.Add(CreateParam("@INTEGRATOR_ID", SqlDbType.Int, ParameterDirection.Input, CObj(IntegratorID)))

            dr = ItemCatalog.DataAccess.GetSqlDataReader(cmd, DataAccess.enuDBList.ItemCatalog)

            If dr.HasRows Then
                While dr.Read
                    myVendor = New Vendor
                    myVendor.VendorID = dr!VendorID
                    myVendor.PSVendorID = dr!PSVendorID
                    myVendor.ISAQualifier = dr!ISAQualifier
                    myVendor.ISA_ID = dr!ISA_ID
                    myVendor.EDINotificationEmail = dr!EDINotificationEmail
                    results.Add(myVendor)
                End While
            End If

            Return results
        Finally
            ItemCatalog.DataAccess.ReleaseDataObject(dr, DataAccess.enuDBList.ItemCatalog)
            ItemCatalog.DataAccess.ReleaseDataObject(cmd, DataAccess.enuDBList.ItemCatalog)
        End Try
    End Function

    Public Function GetP2PUnsentOrders() As ArrayList
        Dim cmd As System.Data.SqlClient.SqlCommand = Nothing
        Dim dr As System.Data.SqlClient.SqlDataReader = Nothing

        ' Used in Procurement to Payment (P2P).  Returns a listing of vendor IDs
        ' associated with a particular vendor.  
        ' Called from the "Order - Integrators - Send 850 EDI.dtsx" script.
        Try
            Dim results As New ArrayList
            cmd = New System.Data.SqlClient.SqlCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "GetP2PUnsentOrdersForVendor"

            cmd.Parameters.Clear()
            cmd.Parameters.Add(CreateParam("@Vendor_ID", SqlDbType.Int, ParameterDirection.Input, CObj(Me.VendorID)))

            dr = ItemCatalog.DataAccess.GetSqlDataReader(cmd, DataAccess.enuDBList.ItemCatalog)

            If dr.HasRows Then
                While dr.Read
                    results.Add(New ItemCatalog.Order(dr!OrderHeader_ID))
                End While
            End If

            Return results
        Finally
            ItemCatalog.DataAccess.ReleaseDataObject(dr, DataAccess.enuDBList.ItemCatalog)
            ItemCatalog.DataAccess.ReleaseDataObject(cmd, DataAccess.enuDBList.ItemCatalog)
        End Try
    End Function

    Public Shared Function GetP2PVendorInterchangeControlNumber(ByVal VendorID As Integer) As String
        Dim cmd As System.Data.SqlClient.SqlCommand = Nothing
        Dim dr As System.Data.SqlClient.SqlDataReader = Nothing
        Dim strResults As String = "0"

        ' Used in Procurement to Payment (P2P).  Returns an interchange control number,
        ' which is a unique, per-vendor, incrementing number used to identify individual
        ' individual "envelopes" (i.e., ISA-blocks) sent to a vendor via the EDI integrator.
        ' So, for each EDI ISA-block we send to a vendor, we'll increment and retrieve this value.

        ' Called from the "Order - Integrators - Send 850 EDI.dtsx" script.
        Try
            Dim results As New ArrayList
            cmd = New System.Data.SqlClient.SqlCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "[GetP2PVendorInterchangeControlNumber]"

            cmd.Parameters.Clear()
            cmd.Parameters.Add(CreateParam("@Vendor_ID", SqlDbType.Int, ParameterDirection.Input, CObj(VendorID)))

            dr = ItemCatalog.DataAccess.GetSqlDataReader(cmd, DataAccess.enuDBList.ItemCatalog)

            If dr.HasRows Then

                While dr.Read
                    strResults = dr!P2PIntegrator_ISA_InterchangeControlNumber.ToString()
                End While
            End If

            Return strResults
        Finally
            ItemCatalog.DataAccess.ReleaseDataObject(dr, DataAccess.enuDBList.ItemCatalog)
            ItemCatalog.DataAccess.ReleaseDataObject(cmd, DataAccess.enuDBList.ItemCatalog)
        End Try
    End Function

End Class
