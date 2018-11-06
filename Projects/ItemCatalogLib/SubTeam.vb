Public Class SubTeam
    Dim m_subteam_no As Integer
    Dim m_subteam_name As String
    Dim m_subteam_abbr As String
    Dim m_restricted_ordering As Boolean
    Dim m_expense As Boolean
    Public Property Number() As Integer
        Get
            Return Me.m_subteam_no
        End Get
        Set(ByVal Value As Integer)
            Me.m_subteam_no = Value
        End Set
    End Property
    Public Property Name() As String
        Get
            Return Me.m_subteam_name
        End Get
        Set(ByVal Value As String)
            Me.m_subteam_name = Value
        End Set
    End Property
    Public Property Abbreviation() As String
        Get
            Return Me.m_subteam_abbr
        End Get
        Set(ByVal Value As String)
            Me.m_subteam_abbr = Value
        End Set
    End Property
    Public Property IsOrderingRestricted() As Boolean
        Get
            Return Me.m_restricted_ordering
        End Get
        Set(ByVal Value As Boolean)
            Me.m_restricted_ordering = Value
        End Set
    End Property
    Public Property IsExpense() As Boolean
        Get
            Return Me.m_expense
        End Get
        Set(ByVal Value As Boolean)
            Me.m_expense = Value
        End Set
    End Property
    Public Sub New()
    End Sub
    Public Sub New(ByVal SubTeam_No As Integer)
        Dim cmd As System.Data.SqlClient.SqlCommand = Nothing
        Dim dr As System.Data.SqlClient.SqlDataReader = Nothing
        Try
            cmd = New System.Data.SqlClient.SqlCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "GetSubTeam"
            Dim prm As New System.Data.SqlClient.SqlParameter
            prm.Direction = ParameterDirection.Input
            prm.DbType = DbType.Int32
            prm.ParameterName = "@SubTeam_No"
            prm.Value = SubTeam_No
            cmd.Parameters.Add(prm)
            dr = ItemCatalog.DataAccess.GetSqlDataReader(cmd, DataAccess.enuDBList.ItemCatalog)
            If dr.HasRows Then
                dr.Read()
                Me.m_subteam_no = dr!SubTeam_No
                Me.m_subteam_name = dr!SubTeam_Name
                Me.m_subteam_abbr = IIf(IsDBNull(dr!SubTeam_Abbreviation), String.Empty, dr!SubTeam_Abbreviation)
                Me.m_restricted_ordering = Not dr!SubTeam_Unrestricted
                Me.m_expense = dr!IsExpense
            Else
                Throw New System.ApplicationException("SubTeam construct failed:  SubTeam_No not found")
            End If
        Finally
            ItemCatalog.DataAccess.ReleaseDataObject(dr, DataAccess.enuDBList.ItemCatalog)
            ItemCatalog.DataAccess.ReleaseDataObject(cmd, DataAccess.enuDBList.ItemCatalog)
        End Try
    End Sub
    Public Sub New(ByVal Number As Integer, ByVal Name As String, ByVal Abbreviation As String, ByVal RestrictedOrdering As Boolean, ByVal Expense As Boolean)
        Me.m_subteam_no = Number
        Me.m_subteam_name = Name
        Me.m_subteam_abbr = Abbreviation
        Me.m_restricted_ordering = RestrictedOrdering
        Me.m_expense = Expense
    End Sub
End Class
