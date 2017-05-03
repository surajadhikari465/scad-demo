Imports SLIM.WholeFoods.Utility.DataAccess

Public Class ODBCErrorLog

    Private _systemTime As Date
    Private _ODBCStart As Date
    Private _ODBCEnd As Date
    Private _errorNumber As Integer
    Private _errorDescription As String
    Private _ODBCCall As String
    Private _userName As String
    Private _computer As String

#Region "Properties"

    Public Property SystemTime() As Date
        Get
            Return _systemTime
        End Get
        Set(ByVal value As Date)
            _systemTime = value
        End Set
    End Property

    Public Property ODBCStart() As Date
        Get
            Return _ODBCStart
        End Get
        Set(ByVal value As Date)
            _ODBCStart = value
        End Set
    End Property

    Public Property ODBCEnd() As Date
        Get
            Return _ODBCEnd
        End Get
        Set(ByVal value As Date)
            _ODBCEnd = value
        End Set
    End Property

    Public Property ErrorNumber() As Integer
        Get
            Return _errorNumber
        End Get
        Set(ByVal value As Integer)
            _errorNumber = value
        End Set
    End Property

    Public Property ErrorDescription() As String
        Get
            Return _errorDescription
        End Get
        Set(ByVal value As String)
            _errorDescription = value
        End Set
    End Property

    Public Property ODBCCall() As String
        Get
            Return _ODBCCall
        End Get
        Set(ByVal value As String)
            _ODBCCall = value
        End Set
    End Property

    Public Property UserName() As String
        Get
            Return _userName
        End Get
        Set(ByVal value As String)
            _userName = value
        End Set
    End Property

    Public Property Computer() As String
        Get
            Return _computer
        End Get
        Set(ByVal value As String)
            _computer = value
        End Set
    End Property

#End Region

    Public Sub InsertODBCErrorLog()
        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim currentParam As DBParam
        Dim paramList As New ArrayList

        Try
            ' setup parameters for stored proc
            currentParam = New DBParam
            currentParam.Name = "ODBCStart"
            currentParam.Value = Me.ODBCStart
            currentParam.Type = DBParamType.DateTime
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "ODBCEnd"
            currentParam.Value = Me.ODBCEnd
            currentParam.Type = DBParamType.DateTime
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "ErrorNumber"
            currentParam.Value = Me.ErrorNumber
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "ErrorDescription"
            currentParam.Value = Me.ErrorDescription
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "ODBCCall"
            currentParam.Value = Me.ODBCCall
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            ' Execute the stored procedure 
            factory.ExecuteStoredProcedure("InsertODBCError", paramList, True)
        Catch ex As Exception
            Debug.WriteLine(ex.Message)
            'exception logging error info - log to system log

        End Try

    End Sub

End Class
