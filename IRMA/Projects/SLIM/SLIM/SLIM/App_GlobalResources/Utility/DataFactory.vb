
Option Explicit On
Option Strict On

Imports System.Collections.Specialized
Imports System.Configuration
Imports System.Data.Common
Imports System.Data.SqlClient
Imports System.Diagnostics
Imports System.IO
Imports System.Runtime.Serialization
Imports System.Text
Imports SLIM.WholeFoods.Utility.Encryption

Namespace WholeFoods.Utility.DataAccess

    Public Enum SupportedDatabaseType
        SqlServer
    End Enum

    Public NotInheritable Class DataFactory
        Public Shared ItemCatalog As String = ConfigurationManager.AppSettings("ConnectionStringName")
        Private _connect, _backupConnect As String
        Private _connection, _backupConnection As SqlConnection
        Private _commandTimeout As Integer  ' Allow setting CommandTimeout for long running queries.  
        Private _dfSwitch As BooleanSwitch

#Region "Constructors and Initialization"

        Public Sub New(ByVal connect As String)
            initClass(connect, Nothing)
        End Sub

        Public Sub New(ByVal connect As String, ByVal backupConnect As String)
            initClass(connect, backupConnect)
        End Sub

        Private Sub initClass(ByVal connect As String, ByVal backupConnect As String)
                ' Set the connection string
            _connect = ConfigurationManager.ConnectionStrings(connect).ConnectionString

        
            ' Create the switch and trace it
            Trace.AutoFlush = True
            _dfSwitch = New BooleanSwitch("dfSwitch", "Data Factory")
            If _dfSwitch.Enabled = True Then
                Trace.WriteLine(Now & ": DataFactory Instance created")
                Trace.Flush()
            End If

            ' Allow setting CommandTimeout for long running queries
            _commandTimeout = 120
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
            End Try
        End Function

#End Region

#Region "GetDataSet, FillExistingDataSet, Update DataSet"

#Region "       GetDataSet, FillExistingDataSet"

        'DATA SET - when retrieving data you need to manipulate, perform calculations with
        '                  - DataSet has more overhead than a DataReader because it is stored in memory
        '                  - work done with DataSet is in disconnected state
        '
        '                  * GetDataSet returns a new dataset.  To fill an existing dataset (if you want to
        '                     add multiple tables to a dataset for use with relations) use the FillExistingDataSet method
        Public Function GetDataSet(ByVal strSql As String) As DataSet
            Return _getDataSet(strSql, Nothing, Nothing, Nothing)
        End Function

        Public Function GetDataSet(ByVal strSql As String, ByVal transaction As SqlTransaction) As DataSet
            Return _getDataSet(strSql, transaction, Nothing, Nothing)
        End Function

        Public Function GetDataSet(ByVal strSql As String, ByRef dsExistingDataSet As DataSet, ByVal strFillTableName As String) As DataSet
            Return _getDataSet(strSql, Nothing, dsExistingDataSet, strFillTableName)
        End Function

        Public Function GetDataSet(ByVal strSql As String, ByRef dsExistingDataSet As DataSet) As DataSet
            Return _getDataSet(strSql, Nothing, dsExistingDataSet, Nothing)
        End Function

        Public Function GetDataSet(ByVal strSql As String, ByVal transaction As SqlTransaction, ByRef dsExistingDataSet As DataSet) As DataSet
            Return _getDataSet(strSql, transaction, dsExistingDataSet, Nothing)
        End Function

        Public Function GetDataSet(ByVal strSql As String, ByVal transaction As SqlTransaction, ByRef dsExistingDataSet As DataSet, ByVal strFillTableName As String) As DataSet
            Return _getDataSet(strSql, transaction, dsExistingDataSet, strFillTableName)
        End Function

        Private Function _getDataSet(ByVal strSql As String, _
                                    ByVal transaction As SqlTransaction, _
                                    ByRef dsExistingDataSet As DataSet, _
                                    ByVal strFillTableName As String) As DataSet
            ' Returns a DataSet given the statement
            Dim adapter As SqlDataAdapter

            Try

                adapter = GetDataAdapter(strSql, transaction)

                'determine if dataset is to be filled with a specific table name
                If Not strFillTableName Is Nothing Then
                    'can be used when creating relations between multiple tables
                    adapter.Fill(dsExistingDataSet, strFillTableName)
                    Return dsExistingDataSet
                Else
                    ' Fill and return without specifying table name
                    Dim dataSet As New DataSet
                    adapter.Fill(dataSet)
                    Return dataSet
                End If
            Catch e As Exception
                throwException("Could not fill DataSet", e)
            End Try
        End Function

#End Region

#Region "       Update DataSet"

        Public Function UpdateDataSet(ByVal dataSet As DataSet, ByVal selectSQL As String, ByVal isStoredProcCall As Boolean) As Integer
            Return _updateDataSet(dataSet, selectSQL, isStoredProcCall, Nothing, Nothing)
        End Function

        Public Function UpdateDataSet(ByVal dataSet As DataSet, ByVal selectSQL As String, ByVal isStoredProcCall As Boolean, ByVal paramList As ArrayList) As Integer
            Return _updateDataSet(dataSet, selectSQL, isStoredProcCall, Nothing, paramList)
        End Function

        Public Function UpdateDataSet(ByVal dataSet As DataSet, ByVal selectSQL As String, ByVal isStoredProcCall As Boolean, ByVal transaction As SqlTransaction) As Integer
            Return _updateDataSet(dataSet, selectSQL, isStoredProcCall, transaction, Nothing)
        End Function

        Public Function UpdateDataSet(ByVal dataSet As DataSet, ByVal selectSQL As String, ByVal isStoredProcCall As Boolean, ByVal transaction As SqlTransaction, ByVal paramList As ArrayList) As Integer
            Return _updateDataSet(dataSet, selectSQL, isStoredProcCall, transaction, paramList)
        End Function

        Private Function _updateDataSet(ByVal dataSet As DataSet, ByVal selectSQL As String, ByVal isStoredProcCall As Boolean, ByVal transaction As SqlTransaction, ByVal paramList As ArrayList) As Integer
            Dim affectedRows As Integer
            Dim dataAdapter As SqlDataAdapter
            Dim commandBuilder As SqlCommandBuilder
            Dim command As SqlCommand

            ' Get the command
            command = GetDataCommand(selectSQL, transaction, isStoredProcCall)

            'add parameters if they are specified
            If Not paramList Is Nothing Then
                If paramList.Count > 0 Then
                    'add parameters to command obj before execution
                    Dim currentParam As DBParam
                    Dim newParm As SqlParameter
                    Dim paramCount As Integer

                    For paramCount = 0 To (paramList.Count - 1)
                        currentParam = CType(paramList(paramCount), DBParam)

                        ' Create the parameter object using proper parameter type for given provider
                        newParm = New SqlParameter

                        'setup name of parameter 
                        'SqlClient requires '@' as leading char
                        If (Not currentParam.Name.StartsWith("@")) Then
                            newParm.ParameterName = "@" + currentParam.Name
                        Else
                            newParm.ParameterName = currentParam.Name
                        End If

                        ' Get the type
                        newParm.SqlDbType = currentParam.SqlDbType

                        'OUTPUT params will not have values assigned
                        If Not currentParam.Value Is Nothing Then
                            'assign value to new provider parameter for input params
                            newParm.Value = currentParam.Value
                        Else
                            'set direction as OUTPUT if no value
                            newParm.Direction = ParameterDirection.Output
                        End If

                        'add parameter to command object
                        command.Parameters.Add(newParm)
                    Next
                End If
            End If

            ' create the DataAdapter
            dataAdapter = New SqlDataAdapter(command)

            ' commandBuilder generates the insert, update, and delete commands
            commandBuilder = New SqlCommandBuilder(dataAdapter)

            Logger.LogDebug("*** INSERT: " + commandBuilder.GetInsertCommand.CommandText, Me.GetType)
            Logger.LogDebug("*** UPDATE: " + commandBuilder.GetUpdateCommand.CommandText, Me.GetType)
            Logger.LogDebug("*** DELETE: " + commandBuilder.GetDeleteCommand.CommandText, Me.GetType)

            Try
                ' save the changes
                affectedRows = dataAdapter.Update(dataSet.Tables(0))
                dataSet.AcceptChanges()
            Catch ex As DBConcurrencyException
                ' results from multiple users updates conflicting
                Throw ex
            End Try

            Return affectedRows
        End Function

#End Region

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

        Public Function ExecuteNonQuery(ByVal strSql As String, ByVal paramList As ArrayList, ByVal transaction As SqlTransaction) As Integer
            Return _executeNonQuery(strSql, paramList, transaction)
        End Function

        Private Function _executeNonQuery(ByVal strSql As String, _
                                                        ByVal paramList As ArrayList, _
                                                        ByVal transaction As SqlTransaction) As Integer
            ' Execute the INSERT/UPDATE/DELETE and return number of rows affected
            Dim command As SqlCommand
            Dim rowsAffected As Integer
            Dim leaveOpen As Boolean = False
            Dim errorParamValues As New StringBuilder 'used to log param values to error table in event of error executing query

            ' Get the command
            command = GetDataCommand(strSql, transaction, False)

            Try
                'open connection
                If command.Connection.State = ConnectionState.Closed Then
                    OpenCommandConnection(command)
                ElseIf transaction IsNot Nothing Then
                    leaveOpen = True
                End If

                'add parameters if they are specified
                If Not paramList Is Nothing Then
                    If paramList.Count > 0 Then
                        'add parameters to SQL before execution
                        Dim currentParam As DBParam
                        Dim newParm As SqlParameter
                        Dim paramCount As Integer

                        For paramCount = 0 To (paramList.Count - 1)
                            currentParam = CType(paramList(paramCount), DBParam)

                            ' Create the parameter object using proper parameter type for given provider
                            newParm = New SqlParameter

                            'setup name of parameter 
                            'SqlClient requires '@' as leading char
                            If (Not currentParam.Name.StartsWith("@")) Then
                                newParm.ParameterName = "@" + currentParam.Name
                            Else
                                newParm.ParameterName = currentParam.Name
                            End If

                            ' Get the type
                            newParm.SqlDbType = currentParam.SqlDbType

                            'OUTPUT params will not have values assigned
                            If Not currentParam.Value Is Nothing Then
                                'assign value to new provider parameter for input params
                                newParm.Value = currentParam.Value

                                Try
                                    If newParm.Value Is DBNull.Value Then
                                        errorParamValues.Append("NULL")
                                    Else
                                        errorParamValues.Append(newParm.Value.ToString)
                                    End If
                                Catch ex As Exception
                                    'unable to script param value in event of error
                                End Try
                                errorParamValues.Append(", ")
                            Else
                                'set direction as OUTPUT if no value
                                newParm.Direction = ParameterDirection.Output
                            End If

                            'add parameter to command object
                            command.Parameters.Add(newParm)
                        Next
                    End If
                End If

                rowsAffected = command.ExecuteNonQuery()

                'Return rowsAffected
                Return rowsAffected
            Catch e As Exception
                throwException("Could not invoke ExecuteNonQuery for statement " & strSql & " (" & errorParamValues.ToString & ")", e)
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
            Finally
                If Not leaveOpen Then
                    command.Connection.Close()
                End If
            End Try
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
            Dim errorParamValues As New StringBuilder 'used to log param values to error table in event of error executing query

            ' Get the command
            command = GetDataCommand(strProcedureName, transaction, True)

            Try
                'open connection
                If command.Connection.State = ConnectionState.Closed Then
                    OpenCommandConnection(command)
                ElseIf transaction IsNot Nothing Then
                    'leave connection open because it is coming from the transaction, which will be used on subsequent database calls
                    leaveOpen = True
                End If

                'add parameters if they are specified
                If Not paramList Is Nothing Then
                    If paramList.Count > 0 Then
                        'add parameters to command obj before execution
                        Dim currentParam As DBParam
                        Dim newParm As SqlParameter
                        Dim paramCount As Integer

                        For paramCount = 0 To (paramList.Count - 1)
                            currentParam = CType(paramList(paramCount), DBParam)

                            ' Create the parameter object using proper parameter type for given provider
                            newParm = New SqlParameter

                            'setup name of parameter 
                            'SqlClient requires '@' as leading char
                            If Not currentParam.Name.StartsWith("@") Then
                                newParm.ParameterName = "@" + currentParam.Name
                            Else
                                newParm.ParameterName = currentParam.Name
                            End If

                            ' Get the type
                            newParm.SqlDbType = currentParam.SqlDbType

                            'OUTPUT params will not have values assigned
                            If Not currentParam.Value Is Nothing Then
                                'assign value to new provider parameter for input params
                                newParm.Value = currentParam.Value

                                Try
                                    If newParm.Value Is DBNull.Value Then
                                        errorParamValues.Append("NULL")
                                    Else
                                        errorParamValues.Append(newParm.Value.ToString)
                                    End If
                                Catch ex As Exception
                                    'unable to script param value in event of error
                                End Try
                                errorParamValues.Append(", ")
                            Else
                                'set direction as OUTPUT if no value
                                newParm.Direction = ParameterDirection.Output

                                'capture name of this parameter -- used below to send output param values back in array list
                                If Not currentParam.Name.StartsWith("@") Then
                                    outputParamLocations.Add("@" + currentParam.Name)
                                Else
                                    outputParamLocations.Add(currentParam.Name)
                                End If
                            End If

                            'add parameter to command object
                            command.Parameters.Add(newParm)
                        Next
                    End If
                End If

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
                    throwException("Could not invoke ExecuteNonQuery for statement " & strProcedureName & " (" & errorParamValues.ToString & ")", e)
                End If
            Finally
                If Not leaveOpen Then
                    command.Connection.Close()
                    command.Connection.Dispose()
                    command.Dispose()
                End If
            End Try
        End Function

#End Region

#Region "       StoredProc.DataReader"

        Public Function GetStoredProcedureDataReader(ByVal strProcedureName As String) As SqlDataReader
            Return _executeStoredProcedure_DataReader(strProcedureName, Nothing, Nothing)
        End Function

        Public Function GetStoredProcedureDataReader(ByVal strProcedureName As String, ByVal paramList As ArrayList) As SqlDataReader
            Return _executeStoredProcedure_DataReader(strProcedureName, paramList, Nothing)
        End Function

        Public Function GetStoredProcedureDataReader(ByVal strProcedureName As String, ByVal transaction As SqlTransaction) As SqlDataReader
            Return _executeStoredProcedure_DataReader(strProcedureName, Nothing, transaction)
        End Function

        Public Function GetStoredProcedureDataReader(ByVal strProcedureName As String, ByVal paramList As ArrayList, ByVal transaction As SqlTransaction) As SqlDataReader
            Return _executeStoredProcedure_DataReader(strProcedureName, paramList, transaction)
        End Function

        Private Function _executeStoredProcedure_DataReader(ByVal strProcedureName As String, _
                                                                ByVal paramList As ArrayList, _
                                                                ByVal transaction As SqlTransaction) As SqlDataReader
            'Call stored procedure with passed in parameter values and returns a DataReader
            Dim command As SqlCommand
            Dim dataReader As SqlDataReader
            Dim errorParamValues As New StringBuilder 'used to log param values to error table in event of error executing query

            ' Get the command
            command = GetDataCommand(strProcedureName, transaction, True)

            Try
                'open connection
                If command.Connection.State = ConnectionState.Closed Then
                    OpenCommandConnection(command)
                End If

                'add parameters if they are specified
                If Not paramList Is Nothing Then
                    If paramList.Count > 0 Then
                        'add parameters to command obj before execution
                        Dim currentParam As DBParam
                        Dim newParm As SqlParameter
                        Dim paramCount As Integer

                        For paramCount = 0 To (paramList.Count - 1)
                            currentParam = CType(paramList(paramCount), DBParam)

                            ' Create the parameter object using proper parameter type for given provider
                            newParm = New SqlParameter

                            'setup name of parameter 
                            'SqlClient requires '@' as leading char
                            If (Not currentParam.Name.StartsWith("@")) Then
                                newParm.ParameterName = "@" + currentParam.Name
                            Else
                                newParm.ParameterName = currentParam.Name
                            End If

                            ' Get the type
                            newParm.SqlDbType = currentParam.SqlDbType

                            'OUTPUT params will not have values assigned
                            If Not currentParam.Value Is Nothing Then
                                'assign value to new provider parameter for input params
                                newParm.Value = currentParam.Value

                                Try
                                    If newParm.Value Is DBNull.Value Then
                                        errorParamValues.Append("NULL")
                                    Else
                                        errorParamValues.Append(newParm.Value.ToString)
                                    End If
                                Catch ex As Exception
                                    'unable to script param value in event of error
                                End Try
                                errorParamValues.Append(", ")
                            Else
                                'set direction as OUTPUT if no value
                                newParm.Direction = ParameterDirection.Output
                            End If

                            'add parameter to command object
                            command.Parameters.Add(newParm)
                        Next
                    End If
                End If

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
                throwException("Could not invoke ExecuteReader for statement " & strProcedureName & " (" & errorParamValues.ToString & ")", e)
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

        Public Function GetStoredProcedureDataSet(ByVal strProcedureName As String, ByVal transaction As SqlTransaction) As DataSet
            Return _executeStoredProcedure_DataSet(strProcedureName, Nothing, transaction)
        End Function

        Public Function GetStoredProcedureDataSet(ByVal strProcedureName As String, ByVal paramList As ArrayList, ByVal transaction As SqlTransaction) As DataSet
            Return _executeStoredProcedure_DataSet(strProcedureName, paramList, transaction)
        End Function

        Private Function _executeStoredProcedure_DataSet(ByVal strProcedureName As String, _
                                                                ByVal paramList As ArrayList, _
                                                                ByVal transaction As SqlTransaction) As DataSet
            'Call stored procedure with passed in parameter values -- return DataSet
            Dim command As SqlCommand
            Dim errorParamValues As New StringBuilder 'used to log param values to error table in event of error executing query

            ' Get the command
            command = GetDataCommand(strProcedureName, transaction, True)

            Try
                'open connection
                If command.Connection.State = ConnectionState.Closed Then
                    OpenCommandConnection(command)
                End If

                'add parameters if they are specified
                If Not paramList Is Nothing Then
                    If paramList.Count > 0 Then
                        'add parameters to command obj before execution
                        Dim currentParam As DBParam
                        Dim newParm As SqlParameter
                        Dim paramCount As Integer

                        For paramCount = 0 To (paramList.Count - 1)
                            currentParam = CType(paramList(paramCount), DBParam)

                            ' Create the parameter object using proper parameter type for given provider
                            newParm = New SqlParameter

                            'setup name of parameter 
                            'SqlClient requires '@' as leading char
                            If (Not currentParam.Name.StartsWith("@")) Then
                                newParm.ParameterName = "@" + currentParam.Name
                            Else
                                newParm.ParameterName = currentParam.Name
                            End If

                            ' Get the type
                            newParm.SqlDbType = currentParam.SqlDbType

                            'OUTPUT params will not have values assigned
                            If Not currentParam.Value Is Nothing Then
                                'assign value to new provider parameter for input params
                                newParm.Value = currentParam.Value

                                Try
                                    If newParm.Value Is DBNull.Value Then
                                        errorParamValues.Append("NULL")
                                    Else
                                        errorParamValues.Append(newParm.Value.ToString)
                                    End If
                                Catch ex As Exception
                                    'unable to script param value in event of error
                                End Try
                                errorParamValues.Append(", ")
                            Else
                                'set direction as OUTPUT if no value
                                newParm.Direction = ParameterDirection.Output
                            End If

                            'add parameter to command object
                            command.Parameters.Add(newParm)
                        Next
                    End If
                End If

                ' get the data adapter for the command
                Dim adapter As IDbDataAdapter = GetDataAdapter(command, transaction)

                ' Fill and return without specifying table name
                Dim dataSet As New DataSet
                adapter.Fill(dataSet)
                Return dataSet
            Catch e As Exception
                throwException("Could not invoke DataAdapter.Fill for statement " & strProcedureName & " (" & errorParamValues.ToString & ")", e)
            End Try
        End Function

#End Region

#End Region

#Region "Database Helper Functions"

        Public Sub OpenCommandConnection(ByRef command As SqlCommand)
            'attempt to open new connection - if fails, look for backup connection
            Try
                If command.Connection.ConnectionString = "" Then
                    command.Connection.ConnectionString = Me.ConnectString
                End If

                command.Connection.Open()
            Catch e As Exception
                'if _backupConnection exists, reset the comman.Connection object, and attempt to open a connection
                If Not _backupConnection Is Nothing Then
                    command.Connection = _backupConnection

                    If command.Connection.ConnectionString = "" Then
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
                If _connection.ConnectionString = "" Then
                    _connection.ConnectionString = Me.ConnectString
                End If
                _connection.Open()
            Catch e As Exception
                'if _backupConnection exists, attempt to open a connection
                If Not _backupConnection Is Nothing Then
                    If _backupConnection.ConnectionString = "" Then
                        _backupConnection.ConnectionString = Me.ConnectString
                    End If

                    _backupConnection.Open()
                End If
            End Try
        End Sub

        Public Function BeginTransaction(ByVal iso As IsolationLevel) As SqlTransaction
            ' Start a new transaction. WARNING: This opens the connection
            Try
                ' Open the connection if it is close
                If (_connection Is Nothing) Then
                    OpenConnection()
                ElseIf _connection.State = ConnectionState.Closed Then
                    OpenConnection()
                End If

                'determine which connection (primary or backup) to start transaction from
                If _connection.State = ConnectionState.Open Then
                    Return _connection.BeginTransaction(iso)
                ElseIf _backupConnection.State = ConnectionState.Open Then
                    Return _backupConnection.BeginTransaction(iso)
                End If
            Catch e As Exception
                throwException("Could not begin the transaction", e)
            End Try
        End Function

        Public Function GetDataCommand(ByVal strSql As String, ByVal transaction As SqlTransaction, ByVal isStoredProcCall As Boolean) As SqlCommand
            'gets the proper command object, and opens a connection with it
            Dim command As New SqlCommand

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
            adapter.SelectCommand = command

            Return adapter
        End Function

#End Region

        Private Sub throwException(ByVal message As String, ByVal innerException As Exception)
            Dim newException As New DataFactoryException(message, innerException)

            ' Trace the error 
            If _dfSwitch.Enabled Then
                Trace.WriteLine(Now & ": " & message & "{" & innerException.Message & "}")
            End If

            'log exception to ODBCErrorLog table
            Dim odbcError As New ODBCErrorLog
            odbcError.ODBCStart = Now
            odbcError.ODBCEnd = Now
            odbcError.ErrorNumber = 0
            odbcError.ErrorDescription = innerException.Message
            odbcError.ODBCCall = message

            odbcError.InsertODBCErrorLog()

            ' Throw
            Throw newException
        End Sub

    End Class


End Namespace