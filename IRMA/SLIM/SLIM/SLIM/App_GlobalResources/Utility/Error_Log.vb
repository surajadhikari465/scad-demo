Imports Microsoft.VisualBasic
Imports System.Diagnostics

Public Class Error_Log


#Region "Member Variables"
    Private myUserName As String
    Private myPassWord As String
    Private commonName As String
    Private domain As String = "wfm"
    Private domainUser As String = domain & "\" & myUserName
#End Region


#Region "Constructors"


    Sub New(ByVal name As String, ByVal pass As String)
        
    End Sub

 
#End Region


#Region "Properties"
    Public Property UserName() As String
        Get
            Return myUserName
        End Get
        Set(ByVal value As String)
            myUserName = value
        End Set
    End Property

    Public Property Password() As String
        Get
            Return myPassWord
        End Get
        Set(ByVal value As String)
            myPassWord = value
        End Set
    End Property
#End Region
#Region "Public Subs"

    Public Shared Sub throwException(ByVal message As String, ByVal innerException As Exception)
        'Dim newException As New DataFactoryException(message, innerException)

        ' Trace the error 
        'If _dfSwitch.Enabled Then
        '    Trace.WriteLine(Now & ": " & message & "{" & innerException.Message & "}")
        'End If

        'log exception to ODBCErrorLog table
        Dim odbcError As New ODBCErrorLog
        odbcError.ODBCStart = Now
        odbcError.ODBCEnd = Now
        odbcError.ErrorNumber = 0
        odbcError.ErrorDescription = "SLIM Error"
        odbcError.ODBCCall = message

        odbcError.InsertODBCErrorLog()

        ' Throw
        'Throw newException
    End Sub

#End Region


End Class
