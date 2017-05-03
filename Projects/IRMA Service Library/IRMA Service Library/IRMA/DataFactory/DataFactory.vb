Imports System.Collections.Specialized
Imports System.Configuration
Imports System.Data.Common
Imports System.Data.SqlClient
Imports System.Diagnostics
Imports System.IO
Imports System.Runtime.Serialization
Imports System.Text
Imports System.Text.RegularExpressions
Imports WholeFoods.ServiceLibrary.Encryption
Imports System.Web.Hosting
Imports log4net
Imports log4net.Config


Namespace DataAccess

    Public Enum SupportedDatabaseType
        SqlServer
    End Enum

    Public NotInheritable Class DataFactory

        Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

        Public Const IRMAServiceLibrary As String = "IRMAServiceLibrary_Conn"

        Private _connect, _backupConnect As String
        Private _connection, _backupConnection As SqlConnection
        Private _commandTimeout As Integer  ' Allow setting CommandTimeout for long running queries.  
        Private _dfSwitch As BooleanSwitch
        Private _dbCallStart As DateTime = Date.Now
        Private _logAndThrowNewExceptionOnError As Boolean = True

#Region "Constructors and Initialization"

        Public Sub New(ByVal connect As String, Optional ByVal useInputString As Boolean = False)

            initClass(connect, Nothing, useInputString, -1)

        End Sub

        Public Sub New(ByVal connect As String, ByVal backupConnect As String, Optional ByVal useInputString As Boolean = False)

            initClass(connect, backupConnect, useInputString, -1)

        End Sub

        Public Sub New(ByVal connect As String, ByVal manualTimeout As Integer, Optional ByVal useInputString As Boolean = False, Optional ByVal Encrypted As Boolean = False)

            initClass(connect, Nothing, useInputString, manualTimeout, Encrypted)

        End Sub

        Private Sub initClass(ByVal connect As String, ByVal backupConnect As String, ByVal useInputString As Boolean, ByVal manualTimeout As Integer, Optional ByVal Encrypted As Boolean = False)

            ' Is the connection string being set by the calling app instead of in a config file?
            If useInputString Then
                _connect = connect
            Else
                ' use the connection string specified in app.config
                _connect = ConfigurationManager.ConnectionStrings(connect).ConnectionString
            End If

            If Not Encrypted Then

                ' are connection string encrypted?
                Encrypted = CType(ConfigurationManager.AppSettings("encryptedConnectionStrings"), Boolean)

            End If

            If Encrypted Then

                Dim encryptor As New Encryptor()

                _connect = encryptor.Decrypt(_connect)

            End If

            'Set the backup connection string if one exists
            If Not backupConnect Is Nothing Then
                _backupConnect = backupConnect
            End If

            ' Create the switch and trace it
            Trace.AutoFlush = True
            _dfSwitch = New BooleanSwitch("dfSwitch", "Data Factory")
            If _dfSwitch.Enabled = True Then
                Trace.WriteLine(Now & ": DataFactory Instance created")
                Trace.Flush()
            End If

            ' Allow setting CommandTimeout for long running queries
            Dim defaultCommandTimeout As Integer = 120
            If manualTimeout = -1 Then

                Try

                    If _commandTimeout <= 0 Then
                        _commandTimeout = defaultCommandTimeout
                    End If

                Catch ex As Exception
                    _commandTimeout = defaultCommandTimeout

                End Try
            Else
                Try
                    _commandTimeout = manualTimeout

                    If _commandTimeout <= 0 Then
                        _commandTimeout = defaultCommandTimeout
                    End If

                Catch ex As Exception
                    _commandTimeout = defaultCommandTimeout

                End Try
            End If


        End Sub

        Private Sub createProviderTypes()
            ' Create an instance of the connection object
            Try
                _connection.ConnectionString = _connect

                'setup backup connection object if one exists
                If Not _backupConnect Is Nothing Then
                    _backupConnection.ConnectionString = _backupConnect
                End If
            Catch e As Exception
                throwException("Could not create connection object.", e)
            End Try
        End Sub

#End Region

#Region "Properties (Connection, Provider)"

        Public ReadOnly Property ConnectString() As String
            ' Returns a reference to the connection
            Get
                Return _connect
            End Get
        End Property

        Public ReadOnly Property Connection() As IDbConnection
            ' Returns a reference to the connection
            Get
                Return _connection
            End Get
        End Property

        ' Allow setting CommandTimeout for long running queries.  -Karl Dirck - 4/8/2004
        Public Property CommandTimeout() As Integer
            Get
                Return _commandTimeout
            End Get
            Set(ByVal Value As Integer)
                _commandTimeout = Value
            End Set
        End Property

#End Region

#Region "GetDataReader"

        'DATA READER - when getting data and displaying it without any calculation/manipulation
        '                          - connection remains open until you close the DataReader
        Public Function GetDataReader(ByVal strSql As String) As SqlDataReader
            Return _getDataReader(strSql, Nothing)
        End Function

        Public Function GetDataReader(ByVal strSql As String, ByVal transaction As SqlTransaction) As SqlDataReader
            Return _getDataReader(strSql, transaction)
        End Function

        Private Function _getDataReader(ByVal strSql As String, ByVal transaction As SqlTransaction) As SqlDataReader

            Dim command As SqlCommand
            Dim dataReader As SqlDataReader

            ' Get the command
            command = GetDataCommand(strSql, transaction, False)

            Try
                'open connection
                If command.Connection.State = ConnectionState.Closed Then
                    OpenCommandConnection(command)
                End If

                'get data
                If transaction IsNot Nothing Then
                    'keep connection open for use in transaction
                    dataReader = command.ExecuteReader()
                Else
                    'connection will close when reader closes
                    dataReader = command.ExecuteReader(CommandBehavior.CloseConnection)
                End If

                Return dataReader
            Catch e As Exception
                throwException("Could not create DataReader for statement " & strSql, e)
                Return Nothing
            End Try
        End Function

#End Region

#Region "ExecuteNonQuery"

        'EXECUTE NON QUERY - perform an INSERT/UPDATE/DELETE
        Public Function ExecuteNonQuery(ByVal strSql As String) As Integer
            Return _executeNonQuery(strSql, Nothing, Nothing)
        End Function

        Public Function ExecuteNonQuery(ByVal strSql As String, ByVal transaction As SqlTransaction) As Integer
            Return _executeNonQuery(strSql, Nothing, transaction)
        End Function

        Public Function ExecuteNonQuery(ByVal strSql As String, ByVal paramList As ArrayList) As Integer
            Return _executeNonQuery(strSql, paramList, Nothing)
        End Function

        Public Function ExecuteNonQuery(ByVal strSql As String, ByVal paramList As DBParamList) As Integer
            Return _executeNonQuery(strSql, paramList.CopyToArrayList, Nothing)
        End Function

        Public Function ExecuteNonQuery(ByVal strSql As String, ByVal paramList As ArrayList, ByVal transaction As SqlTransaction) As Integer
            Return _executeNonQuery(strSql, paramList, transaction)
        End Function

        Public Function ExecuteNonQuery(ByVal strSql As String, ByVal paramList As DBParamList, ByVal transaction As SqlTransaction) As Integer
            Return _executeNonQuery(strSql, paramList.CopyToArrayList, transaction)
        End Function

        Private Function _executeNonQuery(ByVal strSql As String, _
                                                        ByVal paramList As ArrayList, _
                                                        ByVal transaction As SqlTransaction) As Integer
            ' Execute the INSERT/UPDATE/DELETE and return number of rows affected
            Dim command As SqlCommand
            Dim rowsAffected As Integer
            Dim leaveOpen As Boolean = False
            Dim sCommandString As String = String.Empty     'used to log param values to error table in event of error executing query

            ' Get the command
            command = GetDataCommand(strSql, transaction, False)

            Try
                'open connection
                If command.Connection.State = ConnectionState.Closed Then
                    OpenCommandConnection(command)
                ElseIf transaction IsNot Nothing Then
                    leaveOpen = True
                End If

                'add parameters (if any are specified)
                sCommandString = AddParameters(command, paramList)

                rowsAffected = command.ExecuteNonQuery()

                'Return rowsAffected
                Return rowsAffected
            Catch e As Exception
                If _logAndThrowNewExceptionOnError Then
                    throwException(String.Format("Could not invoke ExecuteNonQuery for the following statement: {0}", sCommandString), e)
                Else
                    Throw e
                End If
            Finally
                If Not leaveOpen Then
                    command.Connection.Close()
                End If
            End Try
        End Function

#End Region

#Region "ExecuteScalar"

        'EXECUTE SCALAR - returns only one value (ie. COUNT(*)); much faster than a DataReader
        Public Function ExecuteScalar(ByVal strSql As String) As Object
            Return _executeScalar(strSql, Nothing)
        End Function

        Public Function ExecuteScalar(ByVal strSql As String, ByVal transaction As SqlTransaction) As Object
            Return _executeScalar(strSql, transaction)
        End Function

        Private Function _executeScalar(ByVal strSql As String, ByVal transaction As SqlTransaction) As Object
            'logger.Debug("_executeScalar entry")
            ' Return a single value using ExecuteScalar
            Dim command As SqlCommand
            Dim returnValue As Object
            Dim leaveOpen As Boolean = False

            ' Get the command
            command = GetDataCommand(strSql, transaction, False)

            Try
                'open connection
                If command.Connection.State = ConnectionState.Closed Then
                    OpenCommandConnection(command)
                ElseIf transaction IsNot Nothing Then
                    leaveOpen = True
                End If

                returnValue = command.ExecuteScalar()
                Return returnValue
            Catch e As Exception
                throwException("Failed to execute ExecuteScalar method for statement " & strSql, e)
                Return Nothing
            Finally
                If Not leaveOpen Then
                    command.Connection.Close()
                End If
            End Try
            'logger.Debug("_executeScalar exit")
        End Function

#End Region

#Region "ExecuteStoredProcedure"

#Region "       StoredProc.ArrayList"

        Public Function ExecuteStoredProcedure(ByVal strProcedureName As String) As ArrayList
            Return _executeStoredProcedure(strProcedureName, Nothing, Nothing, False)
        End Function

        Public Function ExecuteStoredProcedure(ByVal strProcedureName As String, ByVal paramList As ArrayList) As ArrayList
            Return _executeStoredProcedure(strProcedureName, paramList, Nothing, False)
        End Function

        Public Function ExecuteStoredProcedure(ByVal strProcedureName As String, ByVal paramList As ArrayList, ByVal isErrorLog As Boolean) As ArrayList
            Return _executeStoredProcedure(strProcedureName, paramList, Nothing, isErrorLog)
        End Function

        Public Function ExecuteStoredProcedure(ByVal strProcedureName As String, ByVal transaction As SqlTransaction) As ArrayList
            Return _executeStoredProcedure(strProcedureName, Nothing, transaction, False)
        End Function

        Public Function ExecuteStoredProcedure(ByVal strProcedureName As String, ByVal paramList As ArrayList, ByVal transaction As SqlTransaction) As ArrayList
            Return _executeStoredProcedure(strProcedureName, paramList, transaction, False)
        End Function

        Private Function _executeStoredProcedure(ByVal strProcedureName As String, _
                                                ByVal paramList As ArrayList, _
                                                ByVal transaction As SqlTransaction, _
                                                ByVal isErrorLog As Boolean) As ArrayList
            'Call stored procedure with passed in parameter values
            Dim command As SqlCommand
            Dim leaveOpen As Boolean = False
            Dim outputParamLocations As New ArrayList
            Dim outputParamValues As New ArrayList
            Dim outParm As SqlParameter
            Dim sCommandString As String = String.Empty     'used to log param values to error table in event of error executing query

            ' Get the command
            Dim i As Integer
            If Not paramList Is Nothing Then i = paramList.Count Else i = 0
            command = GetDataCommand(strProcedureName, transaction, True)

            Try
                'open connection
                If command.Connection.State = ConnectionState.Closed Then
                    OpenCommandConnection(command)
                ElseIf transaction IsNot Nothing Then
                    'leave connection open because it is coming from the transaction, which will be used on subsequent database calls
                    leaveOpen = True
                End If

                'add parameters (if any are specified)
                sCommandString = AddParameters(command, paramList, outputParamLocations)

                'execute stored procedure call
                command.ExecuteNonQuery()

                'add output param values
                Dim j As Integer
                For j = 0 To outputParamLocations.Count - 1
                    outParm = command.Parameters.Item(CType(outputParamLocations(j), String))
                    outputParamValues.Add(outParm.Value)
                Next

                'return arrayList of output param values
                Return outputParamValues
            Catch e As Exception
                'throw exceptions only for non-error logging issues.  this is to prevent an infinite loop when an error occurs.
                If isErrorLog = False Then
                    throwException(String.Format("Could not invoke ExecuteNonQuery for the following statement: {0}", sCommandString), e)
                End If
                Return Nothing
            Finally
                If Not leaveOpen Then
                    command.Connection.Close()
                    command.Connection.Dispose()
                    command.Dispose()
                End If

            End Try
            'logger.Debug("_executeStoredProcedure exit")
        End Function

#End Region

#Region "       StoredProc.DataReader"

        Public Function GetStoredProcedureDataReader(ByVal strProcedureName As String) As SqlDataReader
            Return _executeStoredProcedure_DataReader(strProcedureName, Nothing, Nothing)
        End Function

        Public Function GetStoredProcedureDataReader(ByVal strProcedureName As String, ByVal paramList As ArrayList) As SqlDataReader
            Return _executeStoredProcedure_DataReader(strProcedureName, paramList, Nothing)
        End Function

        Public Function GetStoredProcedureDataReader(ByVal strProcedureName As String, ByVal paramList As DBParamList) As SqlDataReader
            Return _executeStoredProcedure_DataReader(strProcedureName, paramList.CopyToArrayList, Nothing)
        End Function

        Public Function GetStoredProcedureDataReader(ByVal strProcedureName As String, ByVal transaction As SqlTransaction) As SqlDataReader
            Return _executeStoredProcedure_DataReader(strProcedureName, Nothing, transaction)
        End Function

        Public Function GetStoredProcedureDataReader(ByVal strProcedureName As String, ByVal paramList As ArrayList, ByVal transaction As SqlTransaction) As SqlDataReader
            Return _executeStoredProcedure_DataReader(strProcedureName, paramList, transaction)
        End Function

        Public Function GetStoredProcedureDataReader(ByVal strProcedureName As String, ByVal paramList As DBParamList, ByVal transaction As SqlTransaction) As SqlDataReader
            Return _executeStoredProcedure_DataReader(strProcedureName, paramList.CopyToArrayList, transaction)
        End Function

        Private Function _executeStoredProcedure_DataReader(ByVal strProcedureName As String, _
                                                                ByVal paramList As ArrayList, _
                                                                ByVal transaction As SqlTransaction) As SqlDataReader

            'Call stored procedure with passed in parameter values and returns a DataReader
            Dim command As SqlCommand
            Dim dataReader As SqlDataReader
            Dim sCommandString As String = String.Empty     'used to log param values to error table in event of error executing query

            ' Get the command
            command = GetDataCommand(strProcedureName, transaction, True)

            Try
                'open connection
                If command.Connection.State = ConnectionState.Closed Then
                    OpenCommandConnection(command)
                End If

                'add parameters (if any are specified)
                sCommandString = AddParameters(command, paramList)

                'execute stored proc - get data 
                If transaction IsNot Nothing Then
                    'keep connection open for use in transaction
                    dataReader = command.ExecuteReader()
                Else
                    'connection will close when reader closes
                    dataReader = command.ExecuteReader(CommandBehavior.CloseConnection)
                End If

                Return dataReader
            Catch e As Exception
                throwException(String.Format("Could not invoke ExecuteReader for the following statement: {0}", sCommandString), e)
                Return Nothing
            End Try
        End Function

#End Region

#Region "       StoredProc.DataSet"

        Public Function GetStoredProcedureDataSet(ByVal strProcedureName As String) As DataSet
            Return _executeStoredProcedure_DataSet(strProcedureName, Nothing, Nothing)
        End Function

        Public Function GetStoredProcedureDataSet(ByVal strProcedureName As String, ByVal paramList As ArrayList) As DataSet
            Return _executeStoredProcedure_DataSet(strProcedureName, paramList, Nothing)
        End Function

        Public Function GetStoredProcedureDataSet(ByVal strProcedureName As String, ByVal paramList As DBParamList) As DataSet
            Return _executeStoredProcedure_DataSet(strProcedureName, paramList.CopyToArrayList, Nothing)
        End Function

        Public Function GetStoredProcedureDataSet(ByVal strProcedureName As String, ByVal transaction As SqlTransaction) As DataSet
            Return _executeStoredProcedure_DataSet(strProcedureName, Nothing, transaction)
        End Function

        Public Function GetStoredProcedureDataSet(ByVal strProcedureName As String, ByVal paramList As ArrayList, ByVal transaction As SqlTransaction) As DataSet
            Return _executeStoredProcedure_DataSet(strProcedureName, paramList, transaction)
        End Function

        Public Function GetStoredProcedureDataSet(ByVal strProcedureName As String, ByVal paramList As DBParamList, ByVal transaction As SqlTransaction) As DataSet
            Return _executeStoredProcedure_DataSet(strProcedureName, paramList.CopyToArrayList, transaction)
        End Function

        Private Function _executeStoredProcedure_DataSet(ByVal strProcedureName As String, _
                                                                ByVal paramList As ArrayList, _
                                                                ByVal transaction As SqlTransaction) As DataSet
            'Call stored procedure with passed in parameter values -- return DataSet
            Dim command As SqlCommand
            Dim sCommandString As String = String.Empty     'used to log param values to error table in event of error executing query

            ' Get the command
            command = GetDataCommand(strProcedureName, transaction, True)

            Try
                'open connection
                If command.Connection.State = ConnectionState.Closed Then
                    OpenCommandConnection(command)
                End If

                'add parameters (if any are specified)
                sCommandString = AddParameters(command, paramList)

                ' get the data adapter for the command
                Dim adapter As IDbDataAdapter = GetDataAdapter(command, transaction)

                ' Fill and return without specifying table name
                Dim dataSet As New DataSet
                adapter.Fill(dataSet)
                Return dataSet
            Catch e As Exception
                throwException(String.Format("Could not invoke DataAdapter.Fill for the following statement: {0}", sCommandString), e)
                Return Nothing
            End Try
        End Function

#End Region

#Region "       StoredProc.DataTable"

        Public Function GetStoredProcedureDataTable(ByVal strProcedureName As String) As DataTable
            Return _executeStoredProcedure_DataTable(strProcedureName, Nothing, Nothing)
        End Function

        Public Function GetStoredProcedureDataTable(ByVal strProcedureName As String, ByVal paramList As ArrayList) As DataTable
            Return _executeStoredProcedure_DataTable(strProcedureName, paramList, Nothing)
        End Function

        Public Function GetStoredProcedureDataTable(ByVal strProcedureName As String, ByVal paramList As DBParamList) As DataTable
            Return _executeStoredProcedure_DataTable(strProcedureName, paramList.CopyToArrayList, Nothing)
        End Function

        Public Function GetStoredProcedureDataTable(ByVal strProcedureName As String, ByVal transaction As SqlTransaction) As DataTable
            Return _executeStoredProcedure_DataTable(strProcedureName, Nothing, transaction)
        End Function

        Public Function GetStoredProcedureDataTable(ByVal strProcedureName As String, ByVal paramList As ArrayList, ByVal transaction As SqlTransaction) As DataTable
            Return _executeStoredProcedure_DataTable(strProcedureName, paramList, transaction)
        End Function

        Public Function GetStoredProcedureDataTable(ByVal strProcedureName As String, ByVal paramList As DBParamList, ByVal transaction As SqlTransaction) As DataTable
            Return _executeStoredProcedure_DataTable(strProcedureName, paramList.CopyToArrayList, transaction)
        End Function

        Private Function _executeStoredProcedure_DataTable(ByVal strProcedureName As String, _
                                                                ByVal paramList As ArrayList, _
                                                                ByVal transaction As SqlTransaction) As DataTable

            'Call stored procedure with passed in parameter values -- return DataTable
            Dim command As SqlCommand
            Dim sCommandString As String = String.Empty     'used to log param values to error table in event of error executing query

            ' Get the command
            command = GetDataCommand(strProcedureName, transaction, True)

            Try
                'open connection
                If command.Connection.State = ConnectionState.Closed Then
                    OpenCommandConnection(command)
                End If

                'add parameters (if any are specified)
                sCommandString = AddParameters(command, paramList)

                ' get the data adapter for the command
                Dim adapter As SqlDataAdapter = GetDataAdapter(command, transaction)

                ' Fill and return without specifying table name
                Dim DataTable As New DataTable
                adapter.Fill(DataTable)
                Return DataTable
            Catch e As Exception
                throwException(String.Format("Could not invoke DataAdapter.Fill for the following statement: {0}", sCommandString), e)
                Return Nothing
            End Try
        End Function

#End Region

#End Region

#Region "Database Helper Functions"

        Public Sub OpenCommandConnection(ByRef command As SqlCommand)
            'attempt to open new connection - if fails, look for backup connection
            Try
                If command.Connection.ConnectionString.Length = 0 Then
                    command.Connection.ConnectionString = Me.ConnectString
                End If

                command.Connection.Open()
            Catch e As Exception
                'if _backupConnection exists, reset the comman.Connection object, and attempt to open a connection
                If Not _backupConnection Is Nothing Then
                    command.Connection = _backupConnection

                    If command.Connection.ConnectionString.Length = 0 Then
                        command.Connection.ConnectionString = Me.ConnectString
                    End If

                    command.Connection.Open()
                Else
                    throwException("Could not establish a database connection.", e)
                End If
            End Try
        End Sub

        Public Sub OpenConnection()
            'attempt to open new connection - if fails, look for backup connection
            Try
                ' initialize the connection object
                If _connection Is Nothing Then
                    _connection = New SqlConnection()
                End If
                ' assign the connection string
                If _connection.ConnectionString.Length = 0 Then
                    _connection.ConnectionString = Me.ConnectString
                End If
                _connection.Open()
            Catch e As Exception
                'if _backupConnection exists, attempt to open a connection
                If Not _backupConnection Is Nothing Then
                    If _backupConnection.ConnectionString.Length = 0 Then
                        _backupConnection.ConnectionString = Me.ConnectString
                    End If

                    _backupConnection.Open()
                Else
                    throwException("Could not establish a database connection.", e)
                End If
            End Try
        End Sub

        Public Function BeginTransaction(ByVal iso As IsolationLevel) As SqlTransaction
            ' Start a new transaction. WARNING: This opens the connection
            Dim tran As SqlClient.SqlTransaction = Nothing
            Try
                ' Open the connection if it is close
                If (_connection Is Nothing) Then
                    OpenConnection()
                ElseIf _connection.State = ConnectionState.Closed Then
                    OpenConnection()
                End If

                'determine which connection (primary or backup) to start transaction from
                If _connection.State = ConnectionState.Open Then
                    tran = _connection.BeginTransaction(iso)
                ElseIf _backupConnection.State = ConnectionState.Open Then
                    tran = _backupConnection.BeginTransaction(iso)
                End If
            Catch e As Exception
                throwException("Could not begin the transaction", e)
                tran = Nothing
            End Try
            Return tran
        End Function

        Public Function GetDataCommand(ByVal strSql As String, ByVal transaction As SqlTransaction, ByVal isStoredProcCall As Boolean) As SqlCommand
            'gets the proper command object, and opens a connection with it
            Dim command As New SqlCommand

            _dbCallStart = Date.Now

            Try
                command.CommandText = strSql

                ' Allow setting CommandTimeout for long running queries. 
                command.CommandTimeout = _commandTimeout

                If isStoredProcCall Then
                    command.CommandType = CommandType.StoredProcedure
                End If

                If Not transaction Is Nothing Then
                    'set command's connection to current open connection of the transaction
                    'no need to open another connection
                    command.Connection = transaction.Connection
                    command.Transaction = transaction
                Else
                    If (_connection Is Nothing) Then
                        OpenConnection()
                    End If
                    command.Connection = _connection
                End If

                Return command
            Catch e As Exception
                throwException("Could not create Command object", e)
                Return Nothing
            End Try
        End Function

        Public Function GetDataAdapter(ByVal strSql As String, ByVal transaction As SqlTransaction) As SqlDataAdapter
            'gets the proper command object, and opens a connection with it
            Dim adapter As New SqlDataAdapter
            adapter.SelectCommand = GetDataCommand(strSql, transaction, False)

            Return adapter
        End Function

        Public Function GetDataAdapter(ByVal command As SqlCommand, ByVal transaction As SqlTransaction) As SqlDataAdapter
            'gets the proper command object, and opens a connection with it
            Dim adapter As New SqlDataAdapter
            _dbCallStart = Date.Now

            adapter.SelectCommand = command

            Return adapter
        End Function

        ''' <summary>
        ''' adds array of parameters to the SQL command object
        ''' </summary>
        ''' <param name="command"></param>
        ''' <param name="paramList"></param>
        ''' <param name="paramOutputList">optional array for output parameters</param>
        ''' <returns>Formatted list of parameters and values to be displayed in messages (i.e. @ParamName = ParamValue, ...)</returns>
        ''' <remarks></remarks>
        Private Function AddParameters(ByRef command As SqlCommand, ByVal paramList As ArrayList, Optional ByRef paramOutputList As ArrayList = Nothing) As String

            Dim sParamListString As New System.Text.StringBuilder

            If paramList IsNot Nothing AndAlso paramList.Count <> 0 Then
                'add parameters to command obj before execution
                Dim currentParam As DBParam
                Dim newParm As SqlClient.SqlParameter
                Dim paramEnum As System.Collections.IEnumerator = paramList.GetEnumerator()

                While paramEnum.MoveNext
                    currentParam = CType(paramEnum.Current, DBParam)

                    With currentParam
                        'setup name of parameter; SqlClient requires '@' as leading char                    
                        If Not .Name.StartsWith("@") Then
                            .Name = String.Format("@{0}", .Name)
                        End If

                        ' Create the parameter object using proper parameter type for given provider
                        newParm = New SqlParameter(.Name, .SqlDbType)

                        'OUTPUT params will not have values assigned
                        If Not .Value Is Nothing Then
                            'assign value to new provider parameter for input params
                            newParm.Value = .Value

                            Try
                                If .Value Is DBNull.Value Then
                                    sParamListString.AppendFormat(", {0} = {1}", .Name, "NULL")
                                Else
                                    Select Case .Type
                                        Case DBParamType.Binary, DBParamType.Bit, DBParamType.Decimal, DBParamType.Float, _
                                                DBParamType.Int, DBParamType.Money, DBParamType.SmallInt
                                            'numeric value
                                            sParamListString.AppendFormat(", {0} = {1}", .Name, .Value)
                                        Case Else
                                            sParamListString.AppendFormat(", {0} = '{1}'", .Name, .Value)
                                    End Select
                                End If
                            Catch ex As Exception
                                'unable to script param value in event of error
                            End Try
                        Else
                            'set direction as OUTPUT if no value
                            newParm.Direction = ParameterDirection.Output

                            If paramOutputList IsNot Nothing Then
                                If newParm.SqlDbType = SqlDbType.VarChar Then
                                    newParm.Value = String.Empty
                                    newParm.Size = -1
                                End If
                                'capture name of output parameter; used to send output param values back in array list from calling method
                                paramOutputList.Add(.Name)
                            End If
                        End If
                    End With

                    'add parameter to command object
                    command.Parameters.Add(newParm)
                End While

                Try
                    If sParamListString.Length <> 0 AndAlso sParamListString.Chars(0).Equals(","c) Then
                        'remove the leading comma
                        sParamListString.Remove(0, 1)
                    End If
                Catch ex As Exception
                    'unable to script param list in event of error
                End Try
            End If

            Try
                'add the name of the stored procedure or command statement
                sParamListString.Insert(0, command.CommandText)
            Catch ex As Exception
                'unable to script param list in event of error
            End Try

            Return sParamListString.ToString.Trim

        End Function

        ''' <summary>
        ''' adds array of parameters to the SQL command object
        ''' </summary>
        ''' <param name="SQLcommand"></param>
        ''' <param name="paramList"></param>
        ''' <returns>Formatted list of parameters and values to be displayed in messages (i.e. @ParamName = ParamValue, ...)</returns>
        ''' <remarks></remarks>
        Public Function GetSQLString(ByVal SQLcommand As String, ByVal paramList As ArrayList, Optional ByRef paramOutputList As ArrayList = Nothing) As String

            Dim sParamListString As New System.Text.StringBuilder

            If paramList IsNot Nothing AndAlso paramList.Count <> 0 Then
                'add parameters to command obj before execution
                Dim currentParam As DBParam
                Dim newParm As SqlClient.SqlParameter
                Dim paramEnum As System.Collections.IEnumerator = paramList.GetEnumerator()

                While paramEnum.MoveNext
                    currentParam = CType(paramEnum.Current, DBParam)

                    With currentParam
                        'setup name of parameter; SqlClient requires '@' as leading char                    
                        If Not .Name.StartsWith("@") Then
                            .Name = String.Format("@{0}", .Name)
                        End If

                        ' Create the parameter object using proper parameter type for given provider
                        newParm = New SqlParameter(.Name, .SqlDbType)

                        'OUTPUT params will not have values assigned
                        If Not .Value Is Nothing Then
                            'assign value to new provider parameter for input params
                            newParm.Value = .Value

                            Try
                                If .Value Is DBNull.Value Then
                                    sParamListString.AppendFormat(", {0} = {1}", .Name, "NULL")
                                Else
                                    Select Case .Type
                                        Case DBParamType.Binary, DBParamType.Bit, DBParamType.Decimal, DBParamType.Float, _
                                                DBParamType.Int, DBParamType.Money, DBParamType.SmallInt
                                            'numeric value
                                            sParamListString.AppendFormat(", {0} = {1}", .Name, .Value)
                                        Case Else
                                            sParamListString.AppendFormat(", {0} = '{1}'", .Name, .Value)
                                    End Select
                                End If
                            Catch ex As Exception
                                'unable to script param value in event of error
                            End Try
                        Else
                            'set direction as OUTPUT if no value
                            newParm.Direction = ParameterDirection.Output

                            If paramOutputList IsNot Nothing Then
                                If newParm.SqlDbType = SqlDbType.VarChar Then
                                    newParm.Value = String.Empty
                                    newParm.Size = -1
                                End If
                                'capture name of output parameter; used to send output param values back in array list from calling method
                                paramOutputList.Add(.Name)
                            End If
                        End If
                    End With

                End While

                Try
                    If sParamListString.Length <> 0 AndAlso sParamListString.Chars(0).Equals(","c) Then
                        'remove the leading comma
                        sParamListString.Remove(0, 1)
                    End If
                Catch ex As Exception
                    'unable to script param list in event of error
                End Try
            End If

            Try
                'add the name of the stored procedure or command statement
                sParamListString.Insert(0, SQLcommand)
            Catch ex As Exception
                'unable to script param list in event of error
            End Try

            Return sParamListString.ToString.Trim

        End Function

        Public Sub WriteToErrorLog(ByVal msg As String, ByVal Exception As Exception)

            Dim servername As String = System.Net.Dns.GetHostName()


            'Code for testing on local machine
            'Dim finfo As FileInfo = New FileInfo("Errorlog.txt")
            'If finfo.Exists = True Then
            '    If finfo.Length > 100 Then
            '        Dim oldfinfo As FileInfo = New FileInfo("Errorlogold.txt")
            '        If oldfinfo.Exists = True Then oldfinfo.Delete()
            '        finfo.MoveTo("Errorlogold.txt")
            '    End If
            'End If

            Dim finfo As FileInfo = New FileInfo(HostingEnvironment.MapPath("~/Errorlog.txt"))
            If finfo.Exists = True Then
                If finfo.Length > 1024000 Then
                    Dim oldfinfo As FileInfo = New FileInfo(HostingEnvironment.MapPath("~/Errorlogold.txt"))
                    If oldfinfo.Exists = True Then oldfinfo.Delete()
                    finfo.MoveTo(HostingEnvironment.MapPath("~/Errorlogold.txt"))
                End If
            End If

            'log it
            Dim fs1 As FileStream = New FileStream(HostingEnvironment.MapPath("~/Errorlog.txt"), FileMode.Append, FileAccess.Write)
            Dim s1 As StreamWriter = New StreamWriter(fs1)

            s1.Write(DateTime.Now.ToString() + " [" + servername + "] " + msg + vbCrLf)

            If Not Exception Is Nothing Then
                If Not Exception.Message = String.Empty Then
                    s1.Write(DateTime.Now.ToString() + " [" + servername + "]   -> Exception: " + Exception.Message + vbCrLf)
                End If

                If Not Exception.InnerException Is Nothing Then
                    If Not Exception.InnerException.Message = String.Empty Then
                        s1.Write(DateTime.Now.ToString() + " [" + servername + "]   ---> InnerException: " + Exception.InnerException.Message + vbCrLf)
                    End If
                End If
            End If

            s1.Close()
            fs1.Close()

        End Sub

        Private Sub throwException(ByVal message As String, ByVal Exception As Exception)
            'Dim newException As New DataFactoryException(message, innerException)

            'log exception to ODBCErrorLog table
            Dim odbcError As New ODBCErrorLog
            odbcError.ODBCStart = _dbCallStart
            odbcError.ODBCEnd = Date.Now
            odbcError.ErrorNumber = 0
            odbcError.ErrorDescription = Exception.Message
            odbcError.ODBCCall = message

            WriteToErrorLog(message, Exception)

            'logger.Error(message)

            ' odbcError.InsertODBCErrorLog()

            ' Throw
            'Throw newException
        End Sub

#End Region

    End Class

End Namespace
