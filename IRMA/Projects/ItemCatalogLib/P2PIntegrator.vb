Public Class P2PIntegrator
    Dim mID As Integer
    Dim mName As String = String.Empty
    Dim mFTPInfo As Structures.FTPInfo

    Public Property ID() As Integer
        Get
            Return mID
        End Get
        Set(ByVal value As Integer)
            mID = value
        End Set
    End Property
    Public Property Name() As String
        Get
            Return mName
        End Get
        Set(ByVal value As String)
            mName = value
        End Set
    End Property
    Public Property FTPInfo() As Structures.FTPInfo
        Get
            Return mFTPInfo
        End Get
        Set(ByVal value As Structures.FTPInfo)
            mFTPInfo = value
        End Set
    End Property
    Private Sub New()
    End Sub
    Private Sub New(ByVal ID As Integer, ByVal Name As String, ByVal FTPAddr As String, ByVal FTPPort As Integer, ByVal FTPPathTransfer As String, ByVal FTPPathInbox As String, ByVal FTPPathOutbox As String, ByVal FTPUser As String, ByVal FTPPassword As String, ByVal FTPProtocolType As Integer, ByVal EDISuffixWithLineBreaks As Boolean)
        Me.ID = ID
        Me.Name = Name
        Dim f As Structures.FTPInfo
        f.Addr = FTPAddr
        f.port = FTPPort
        f.PathTransfer = FTPPathTransfer
        f.PathInbox = FTPPathInbox
        f.PathOutbox = FTPPathOutbox
        f.User = FTPUser
        f.Password = FTPPassword
        f.ProtocolType = FTPProtocolType
        f.EDISuffixWithLineBreaks = EDISuffixWithLineBreaks
        Me.FTPInfo = f
    End Sub
    Public Shared Function GetP2PIntegrators() As ArrayList
        Dim cmd As System.Data.SqlClient.SqlCommand = Nothing
        Dim dr As System.Data.SqlClient.SqlDataReader = Nothing
        Try
            Dim results As New ArrayList
            cmd = New System.Data.SqlClient.SqlCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "GetP2PIntegrators"
            dr = ItemCatalog.DataAccess.GetSqlDataReader(cmd, DataAccess.enuDBList.ItemCatalog)
            If dr.HasRows Then
                While dr.Read
                    results.Add(New ItemCatalog.P2PIntegrator(dr!ID, dr!Name, dr!FTPAddr, dr!FTPPort, dr!FTPPathTransfer, dr!FTPPathInbox, dr!FTPPathOutbox, dr!FTPUser, dr!FTPPassword, dr!FTPProtocolType, dr!EDISuffixWithLineBreaks))
                End While
            End If

            Return results
        Finally
            ItemCatalog.DataAccess.ReleaseDataObject(dr, DataAccess.enuDBList.ItemCatalog)
            ItemCatalog.DataAccess.ReleaseDataObject(cmd, DataAccess.enuDBList.ItemCatalog)
        End Try
    End Function

    
End Class
