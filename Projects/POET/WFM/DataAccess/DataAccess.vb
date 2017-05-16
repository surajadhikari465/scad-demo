Namespace SQLServer
    Public Class Connect
        Dim mConnectionString As String
        Dim mCommandTimeout As Integer
        Public Sub New(ByVal ConnectionString As String, ByVal CommandTimeout As Integer)
            mConnectionString = ConnectionString
            mCommandTimeout = CommandTimeout
        End Sub
        Public ReadOnly Property ConnectionString() As String
            Get
                Return mConnectionString
            End Get
        End Property
        Public ReadOnly Property CommandTimeout() As Integer
            Get
                Return mCommandTimeout
            End Get
        End Property
        Public Function GetSqlDataReader(ByRef cmd As System.Data.SqlClient.SqlCommand) As System.Data.SqlClient.SqlDataReader
            Dim rdr As System.Data.SqlClient.SqlDataReader
            If cmd.Connection Is Nothing Then
                cmd.Connection = New System.Data.SqlClient.SqlConnection(mConnectionString)
                cmd.Connection.Open()
            ElseIf cmd.Connection.State = ConnectionState.Closed Then
                cmd.Connection.ConnectionString = mConnectionString
                cmd.Connection.Open()
            End If
            cmd.CommandTimeout = mCommandTimeout
            rdr = cmd.ExecuteReader()
            If Not rdr.HasRows Then
                rdr.NextResult() 'Needed because deadlocks do not error - this throws the error - see MS Knowledgebase article #316667
            End If
            Return rdr
        End Function
        Public Function GetSqlDataSet(ByVal cmd As System.Data.SqlClient.SqlCommand) As System.Data.DataSet
            If cmd.Connection Is Nothing Then
                cmd.Connection = New System.Data.SqlClient.SqlConnection(mConnectionString)
                cmd.Connection.Open()
            ElseIf cmd.Connection.State = ConnectionState.Closed Then
                cmd.Connection.ConnectionString = mConnectionString
                cmd.Connection.Open()
            End If
            cmd.CommandTimeout = mCommandTimeout
            Dim da As New System.Data.SqlClient.SqlDataAdapter(cmd)
            Dim ds As New System.Data.DataSet
            da.Fill(ds, "ItemUnit")
            Return ds
        End Function
        Public Sub ExecuteSqlCommand(ByRef cmd As System.Data.SqlClient.SqlCommand)
            If cmd.Connection Is Nothing Then
                cmd.Connection = New System.Data.SqlClient.SqlConnection(mConnectionString)
                cmd.Connection.Open()
            ElseIf cmd.Connection.State = ConnectionState.Closed Then
                cmd.Connection.ConnectionString = mConnectionString
                cmd.Connection.Open()
            End If
            cmd.CommandTimeout = mCommandTimeout
            cmd.ExecuteNonQuery()
        End Sub
        Public Sub ConnectSqlCommand(ByRef cmd As System.Data.SqlClient.SqlCommand)
            If cmd.Connection Is Nothing Then
                cmd.Connection = New System.Data.SqlClient.SqlConnection(mConnectionString)
                cmd.Connection.Open()
            ElseIf cmd.Connection.State = ConnectionState.Closed Then
                cmd.Connection.ConnectionString = mConnectionString
                cmd.Connection.Open()
            End If
        End Sub
        Public Sub ReleaseDataObject(ByRef obj As System.Object)
            Dim cmd As System.Data.SqlClient.SqlCommand
            Dim dr As System.Data.SqlClient.SqlDataReader

            If obj Is Nothing Then Exit Sub

            If TypeOf obj Is System.Data.SqlClient.SqlDataReader Then
                dr = obj
                If Not (dr Is Nothing) Then
                    dr.Close()
                    dr = Nothing
                End If
            ElseIf TypeOf obj Is System.Data.SqlClient.SqlCommand Then
                cmd = obj
                If Not (cmd Is Nothing) Then
                    If Not (cmd.Connection Is Nothing) Then
                        If cmd.Connection.State = ConnectionState.Open Then cmd.Connection.Close()
                        cmd.Connection.Dispose()
                        cmd.Connection = Nothing
                    End If
                    cmd.Dispose()
                    cmd = Nothing
                End If
            Else
                Throw New System.ArgumentException("Unknown data object type")
            End If
        End Sub
    End Class
    Public Class Setup
        Shared Sub New()
        End Sub
        Private Shared Function CreateParam(ByVal prm As SqlClient.SqlParameter, ByVal ParamName As String, ByVal sqldbtype As System.Data.SqlDbType, ByVal Direction As System.Data.ParameterDirection) As SqlClient.SqlParameter
            prm.ParameterName = ParamName
            prm.SqlDbType = sqldbtype
            prm.Direction = Direction
            If sqldbtype = sqldbtype.Decimal And (prm.Scale = 0 Or prm.Precision = 0) Then
                Dim ex As New System.Data.SqlTypes.SqlTypeException("Parameters of type decimal require both a non-zero Scale and Percision value ")
                Throw ex
            End If
            Return prm
        End Function
        Public Shared Function CreateParam(ByVal ParamName As String, ByVal sqldbtype As System.Data.SqlDbType, ByVal Direction As System.Data.ParameterDirection) As System.Data.SqlClient.SqlParameter
            Dim prm As New System.Data.SqlClient.SqlParameter
            prm = CreateParam(prm, ParamName, sqldbtype, Direction)
            Return prm
        End Function
        Public Shared Function CreateParam(ByVal ParamName As String, ByVal sqlDBType As System.Data.SqlDbType, ByVal Direction As System.Data.ParameterDirection, ByVal Value As Object, ByVal Size As Long) As System.Data.sqlclient.SqlParameter
            Dim prm As New System.Data.SqlClient.SqlParameter
            prm.Size = Size
            prm.Value = Value
            prm = CreateParam(prm, ParamName, sqlDBType, Direction)
            Return prm
        End Function
        Public Shared Function CreateParam(ByVal ParamName As String, ByVal sqlDBType As System.Data.SqlDbType, ByVal Direction As System.Data.ParameterDirection, ByVal Size As Long) As System.Data.sqlclient.SqlParameter
            Dim prm As New System.Data.SqlClient.SqlParameter
            prm.Size = Size
            prm = CreateParam(prm, ParamName, sqlDBType, Direction)
            Return prm
        End Function
        Public Shared Function CreateParam(ByVal ParamName As String, ByVal sqlDBType As System.Data.SqlDbType, ByVal Direction As System.Data.ParameterDirection, ByVal Value As Object) As System.Data.sqlclient.SqlParameter
            Dim prm As New System.Data.SqlClient.SqlParameter
            prm.Value = Value
            prm = CreateParam(prm, ParamName, sqlDBType, Direction)
            Return prm
        End Function
        Public Shared Function CreateParam(ByVal ParamName As String, ByVal sqlDBType As System.Data.SqlDbType, ByVal Direction As System.Data.ParameterDirection, ByVal Precision As Long, ByVal scale As Long, ByVal Value As Object) As System.Data.sqlclient.SqlParameter
            Dim prm As New System.Data.SqlClient.SqlParameter
            prm.Precision = Precision
            prm.Scale = scale
            prm.Value = Value
            prm = CreateParam(prm, ParamName, sqlDBType, Direction)
            Return prm
        End Function
        Public Shared Function CreateParam(ByVal ParamName As String, ByVal sqlDBType As System.Data.SqlDbType, ByVal Direction As System.Data.ParameterDirection, ByVal Precision As Long, ByVal scale As Long) As System.Data.sqlclient.SqlParameter
            Dim prm As New System.Data.SqlClient.SqlParameter
            prm.Precision = Precision
            prm.Scale = scale
            prm = CreateParam(prm, ParamName, sqlDBType, Direction)
            Return prm
        End Function
    End Class
End Namespace
